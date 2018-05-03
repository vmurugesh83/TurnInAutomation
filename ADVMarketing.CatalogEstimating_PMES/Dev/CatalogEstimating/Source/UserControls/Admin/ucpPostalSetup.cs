#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.PostalTableAdapters;
using CatalogEstimating.Properties;

#endregion

namespace CatalogEstimating.UserControls.Admin
{
    public partial class ucpPostalSetup : CatalogEstimating.UserControlPanel
    {
        #region Private Variables

        private enum CtlState
        {
            ReadOnly = 1,
            ReadOnly_AllowNew = 2,
            New = 3,
            EditExisting_Clean = 4,
            EditExisting_Dirty = 5
        }
        private CtlState _ctlState = CtlState.ReadOnly;
        private Postal _postal = null;
        private Postal.vnd_vendorRow _currentVendor = null;
        private DataView _dvPostalWeights = new DataView();
        private DataView _dvPostalCategoryRateMaps = new DataView();
        private Postal.pst_postalweightsRow _curPostalWeight = null;
        private string _CurVendorName = string.Empty;
        private string _CurDate = string.Empty;

        #endregion

        #region Construction

        public ucpPostalSetup()
        {
            InitializeComponent();

            Name = "Setup";

        }

        public ucpPostalSetup(Postal ds)
            : this()
        {
            _postal = ds;
        }

        #endregion

        #region Overrides

        public override void LoadData()
        {
            base.LoadData();

            IsLoading = true;

            _cboVendor.DataSource = _postal.vnd_vendor;
            _cboVendor.DisplayMember = "description";
            _cboVendor.ValueMember = "vnd_vendor_id";

            _dvPostalWeights.Table = _postal.pst_postalweights;
            _dvPostalWeights.RowFilter = "0 = 1";
            _dvPostalWeights.Sort = "effectivedate desc";

            _cboEffectiveDate.DataSource = _dvPostalWeights;
            _cboEffectiveDate.DisplayMember = "effectivedate";
            _cboEffectiveDate.ValueMember = "pst_postalweights_id";

            DataGridViewComboBoxColumn postalCategoryColumn = (DataGridViewComboBoxColumn)_gridPostalRates.Columns["pst_postalcategory_id"];
            _postal.pst_postalcategory.DefaultView.Sort = "description";
            postalCategoryColumn.DataSource = _postal.pst_postalcategory;
            postalCategoryColumn.DataPropertyName = "pst_postalcategory_id";
            postalCategoryColumn.DisplayMember = "description";
            postalCategoryColumn.ValueMember = "pst_postalcategory_id";

            DataGridViewComboBoxColumn postalClassColumn = (DataGridViewComboBoxColumn)_gridPostalRates.Columns["pst_postalclass_id"];
            _postal.pst_postalclass.DefaultView.Sort = "description";
            postalClassColumn.DataSource = _postal.pst_postalclass;
            postalClassColumn.DataPropertyName = "pst_postalclass_id";
            postalClassColumn.DisplayMember = "description";
            postalClassColumn.ValueMember = "pst_postalclass_id";

            DataGridViewComboBoxColumn postalMailerTypeColumn = (DataGridViewComboBoxColumn)_gridPostalRates.Columns["pst_postalmailertype_id"];
            _postal.pst_postalmailertype.DefaultView.Sort = "description";
            postalMailerTypeColumn.DataSource = _postal.pst_postalmailertype;
            postalMailerTypeColumn.DataPropertyName = "pst_postalmailertype_id";
            postalMailerTypeColumn.DisplayMember = "description";
            postalMailerTypeColumn.ValueMember = "pst_postalmailertype_id";
            postalMailerTypeColumn.SortMode = DataGridViewColumnSortMode.Automatic;

            _dvPostalCategoryRateMaps.Table = _postal.pst_postalcategoryrate_map;
            _dvPostalCategoryRateMaps.RowFilter = "0 = 1";
            _gridPostalRates.DataSource = _dvPostalCategoryRateMaps;

            SetControlState(CtlState.ReadOnly_AllowNew);

            IsLoading = false;
        }

        public override void Reload()
        {
            string vndName = _CurVendorName;
            string effDate = _CurDate;
            _errorProvider.Clear();

            SetControlState(CtlState.ReadOnly);

            if (_cboVendor.Items.Count > 0)
            {
                int idx = -1;

                idx = _cboVendor.FindStringExact(vndName);
                if ((idx > -1) && (idx != _cboVendor.SelectedIndex))
                {
                    _cboVendor.SelectedIndex = idx;
                }
                else
                {
                    _cboVendor_SelectedIndexChanged(this, EventArgs.Empty);
                }

                idx = _cboEffectiveDate.FindStringExact(effDate);
                if ((idx > -1) && (idx != _cboEffectiveDate.SelectedIndex))
                {
                    _cboEffectiveDate.SelectedIndex = idx;
                }
                else
                {
                    _cboEffectiveDate_SelectedIndexChanged(this, EventArgs.Empty);
                }
            }
        }

        public override void Export(ref ExcelWriter writer)
        {
            writer.WriteLine("Vendor", _cboVendor.Text);
            writer.WriteLine("Effective Date", _dtEffectiveDate.Text);
            writer.WriteLine("First Class Overweight Limit", _txtFirstClassOverweightLimit.Text);
            writer.WriteLine("Standard Overweight Limit", _txtStandardOverweightLimit.Text);
            writer.WriteLine("");
            writer.WriteTable(_gridPostalRates, true);
        }

        public override void PreSave(CancelEventArgs e)
        {
            // Force value to be sync'd with what may have been typed into date control.
            MainForm.UpdateDateControl(_dtEffectiveDate);

            if (!this.ValidateChildren() || !ValidateControl())
                e.Cancel = true;
            else if (_ctlState == CtlState.New)
            {
                IsLoading = true;
                _curPostalWeight.firstoverweightlimit = (decimal)_txtFirstClassOverweightLimit.Value;
                _curPostalWeight.standardoverweightlimit = (decimal)_txtStandardOverweightLimit.Value;
                _curPostalWeight.effectivedate = _dtEffectiveDate.Value.Date;
                IsLoading = false;
                SetControlState(CtlState.ReadOnly_AllowNew);
                _CurDate = _dtEffectiveDate.Text;
            }
            else if (_ctlState == CtlState.EditExisting_Dirty)
            {
                IsLoading = true;
                _curPostalWeight.firstoverweightlimit = (decimal)_txtFirstClassOverweightLimit.Value;
                _curPostalWeight.standardoverweightlimit = (decimal)_txtStandardOverweightLimit.Value;
                _curPostalWeight.modifiedby = MainForm.AuthorizedUser.FormattedName;
                _curPostalWeight.modifieddate = DateTime.Now;
                foreach (DataRowView drv in _dvPostalCategoryRateMaps)
                {
                    Postal.pst_postalcategoryrate_mapRow pcr_row = (Postal.pst_postalcategoryrate_mapRow)drv.Row;
                    pcr_row.modifiedby = MainForm.AuthorizedUser.FormattedName;
                    pcr_row.modifieddate = DateTime.Now;
                }
                IsLoading = false;
                SetControlState(CtlState.ReadOnly_AllowNew);
            }
            else if (_ctlState == CtlState.EditExisting_Clean)
            {
                SetControlState(CtlState.ReadOnly_AllowNew);
            }
        }

        public override void OnLeaving(CancelEventArgs e)
        {
            if (!this.ValidateChildren() || !ValidateControl())
                e.Cancel = true;

            if ((_ctlState == CtlState.New)
                || (_ctlState == CtlState.EditExisting_Dirty))
            {
                DialogResult result = MessageBox.Show(Resources.LeaveTabCancelWarning, "Are you sure?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    foreach (DataRow row in _postal.pst_postalcategoryrate_map.Select(string.Concat("pst_postalweights_id = ", _curPostalWeight.pst_postalweights_id)))
                        row.RejectChanges();
                    _curPostalWeight.RejectChanges();

                    Reload();
                }
                else if (result == DialogResult.No)
                {
                    if (ValidateControl())
                    {
                        _btnUpdate_Click(this, EventArgs.Empty);
                    }
                    else
                        e.Cancel = true;
                }
                else if (result == DialogResult.Cancel)
                    e.Cancel = true;
            }
            base.OnLeaving(e);
        }

        public override ToolStrip Toolbar
        {
            get
            {
                return _toolStrip;
            }
        }

        public override void SaveData()
        {
            Reload();
            base.SaveData();
        }

        #endregion

        #region Event Handlers

        private void ucpPostalSetup_Load(object sender, EventArgs e)
        {
            _gridPostalRates.AutoGenerateColumns = false;
        }

        private void _gridPostalRates_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (_ctlState == CtlState.EditExisting_Clean)
                {
                    SetControlState(CtlState.EditExisting_Dirty);
                }

                if ((_gridPostalRates.Columns[e.ColumnIndex].DataPropertyName == "underweightpiecerate" ||
                         _gridPostalRates.Columns[e.ColumnIndex].DataPropertyName == "overweightpiecerate" ||
                         _gridPostalRates.Columns[e.ColumnIndex].DataPropertyName == "overweightpoundrate")
                    && (!string.IsNullOrEmpty(_gridPostalRates[e.ColumnIndex, e.RowIndex].Value.ToString())))
                {
                    string nonFmt = _gridPostalRates[e.ColumnIndex, e.RowIndex].Value.ToString();
                    string fmt = decimal.Parse(nonFmt).ToString("0.0000");

                    if (nonFmt != fmt)
                        _gridPostalRates[e.ColumnIndex, e.RowIndex].Value = fmt;
                }
            }
        }

        private void _gridPostalRates_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataRowView drv = (DataRowView)_gridPostalRates.Rows[e.RowIndex].DataBoundItem;

            if (drv != null)
            {
                if (IsUsedByScenario(drv))
                {
                    if (_gridPostalRates.Columns[e.ColumnIndex].DataPropertyName == "pst_postalcategory_id" ||
                             _gridPostalRates.Columns[e.ColumnIndex].DataPropertyName == "pst_postalclass_id" ||
                             _gridPostalRates.Columns[e.ColumnIndex].DataPropertyName == "pst_postalmailertype_id" ||
                             _gridPostalRates.Columns[e.ColumnIndex].DataPropertyName == "active")
                    {
                        MessageBox.Show(Resources.PostalRateRefModifyWarning, "Cannot Modify", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        e.Cancel = true;
                    }
                }
            }
        }

        private void _gridPostalRates_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (IsUsedByScenario((DataRowView)e.Row.DataBoundItem))
            {
                MessageBox.Show(Resources.PostalRateRefDeleteWarning, "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }
        }

        private void _gridPostalRates_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (_ctlState == CtlState.EditExisting_Clean)
            {
                SetControlState(CtlState.EditExisting_Dirty);
            }
        }

        private void _gridPostalRates_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            _gridPostalRates.Rows[e.RowIndex].ErrorText = string.Empty;
        }

        private void _gridPostalRates_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (_gridPostalRates.Rows[e.RowIndex] == null || _gridPostalRates.Rows[e.RowIndex].IsNewRow || !_gridPostalRates.IsCurrentRowDirty)
                return;

            if (_gridPostalRates.Rows[e.RowIndex].Cells["active"].Value == DBNull.Value)
            {
                _gridPostalRates.Rows[e.RowIndex].Cells["active"].Value = false;
            }

            if (String.IsNullOrEmpty(_gridPostalRates.Rows[e.RowIndex].Cells["pst_postalcategory_id"].FormattedValue.ToString())
                || String.IsNullOrEmpty(_gridPostalRates.Rows[e.RowIndex].Cells["pst_postalclass_id"].FormattedValue.ToString())
                || String.IsNullOrEmpty(_gridPostalRates.Rows[e.RowIndex].Cells["pst_postalmailertype_id"].FormattedValue.ToString())
                || String.IsNullOrEmpty(_gridPostalRates.Rows[e.RowIndex].Cells["underweightpiecerate"].FormattedValue.ToString())
                || String.IsNullOrEmpty(_gridPostalRates.Rows[e.RowIndex].Cells["overweightpiecerate"].FormattedValue.ToString())
                || String.IsNullOrEmpty(_gridPostalRates.Rows[e.RowIndex].Cells["overweightpoundrate"].FormattedValue.ToString()))
            {
                _gridPostalRates.Rows[e.RowIndex].ErrorText = Resources.RequiredFieldError;
                e.Cancel = true;
                return;
            }

            Postal.pst_postalcategoryrate_mapRow pcr_row = (Postal.pst_postalcategoryrate_mapRow)((DataRowView)_gridPostalRates.Rows[e.RowIndex].DataBoundItem).Row;

            if (pcr_row.active && _postal.pst_postalcategoryrate_map.Select(string.Concat("pst_postalweights_id = ", pcr_row.pst_postalweights_id.ToString()
               , " and pst_postalcategory_id = ", pcr_row.pst_postalcategory_id.ToString()
               , " and pst_postalclass_id = ", pcr_row.pst_postalclass_id.ToString()
               , " and pst_postalmailertype_id = ", pcr_row.pst_postalmailertype_id.ToString()
               , " and active = 1"
               , " and pst_postalcategoryrate_map_id <> ", pcr_row.pst_postalcategoryrate_map_id.ToString())).Length > 0)
            {
                _gridPostalRates.Rows[e.RowIndex].ErrorText = "The combination of Postal Category, Postal Class, and Postal Mailer, has been marked active multiple times.";
                e.Cancel = true;
            }
        }

        private void _txtFirstClassOverweightLimit_TextChanged(object sender, EventArgs e)
        {
            if (_ctlState == CtlState.EditExisting_Clean)
                SetControlState(CtlState.EditExisting_Dirty);
        }

        private void _txtStandardOverweightLimit_TextChanged(object sender, EventArgs e)
        {
            if (_ctlState == CtlState.EditExisting_Clean)
                SetControlState(CtlState.EditExisting_Dirty);
        }

        private void _txtFirstClassOverweightLimit_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_txtFirstClassOverweightLimit.Text)
                && _txtFirstClassOverweightLimit.Text != ".")
                _txtFirstClassOverweightLimit.Text = decimal.Parse(_txtFirstClassOverweightLimit.Text).ToString("0.0000");
            else if (_txtFirstClassOverweightLimit.Text == ".")
                _txtFirstClassOverweightLimit.Text = string.Empty;
        }

        private void _txtStandardOverweightLimit_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_txtStandardOverweightLimit.Text)
                && _txtStandardOverweightLimit.Text != ".")
                _txtStandardOverweightLimit.Text = decimal.Parse(_txtStandardOverweightLimit.Text).ToString("0.0000");
            else if (_txtStandardOverweightLimit.Text == ".")
                _txtStandardOverweightLimit.Text = string.Empty;
        }

        private void _cboVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsLoading)
                return;

            if (_cboVendor.SelectedItem == null)
            {
                SetControlState(CtlState.ReadOnly);
                return;
            }

            _currentVendor = _postal.vnd_vendor.FindByvnd_vendor_id((long)_cboVendor.SelectedValue);
            _dvPostalWeights.RowFilter = string.Concat("vnd_vendor_id = ", _currentVendor.vnd_vendor_id.ToString());

            if (_dvPostalWeights.Count > 0)
            {
                _cboEffectiveDate.Enabled = true;
                _cboEffectiveDate.SelectedIndex = 0;
                _cboEffectiveDate_SelectedIndexChanged(sender, e);
            }
            else
            {
                SetControlState(CtlState.ReadOnly_AllowNew);
                ClearControl();
                _dvPostalCategoryRateMaps.RowFilter = "0 = 1";
                _cboEffectiveDate.Enabled = false;
            }
        }

        private void _cboEffectiveDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsLoading)
                return;

            SetControlState(CtlState.ReadOnly_AllowNew);

            if (_cboEffectiveDate.SelectedItem == null)
            {
                _curPostalWeight = null;
                return;
            }

            _curPostalWeight = (Postal.pst_postalweightsRow) ((DataRowView) _cboEffectiveDate.SelectedItem).Row;

            _lblInfoText.Text = string.Concat("Vendor: ", _cboVendor.Text.Trim(), "    Effective Date: ", _cboEffectiveDate.Text);

            _txtFirstClassOverweightLimit.Value = _curPostalWeight.firstoverweightlimit;
            _txtStandardOverweightLimit.Value = _curPostalWeight.standardoverweightlimit;
            _dtEffectiveDate.Value = _curPostalWeight.effectivedate;

            _postal.pst_postalcategoryrate_map.pst_postalweights_idColumn.DefaultValue = _curPostalWeight.pst_postalweights_id;
            _postal.pst_postalcategoryrate_map.createdbyColumn.DefaultValue = MainForm.AuthorizedUser.FormattedName;
            _postal.pst_postalcategoryrate_map.createddateColumn.DefaultValue = DateTime.Now;
            _dvPostalCategoryRateMaps.RowFilter = string.Concat("pst_postalweights_id = ", _curPostalWeight.pst_postalweights_id.ToString());
            SetControlState(CtlState.EditExisting_Clean);

            if (_txtFirstClassOverweightLimit.Enabled)
                _txtFirstClassOverweightLimit.Focus();
        }

        private void _btnNew_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren() || !ValidateControl())
                return;

            WriteToDataset();

            IsLoading = true;
            Postal.pst_postalweightsRow oldPostalWeight = null;

            if (_curPostalWeight != null)
                oldPostalWeight = _curPostalWeight;

            _curPostalWeight = _postal.pst_postalweights.Newpst_postalweightsRow();
            _curPostalWeight.createdby = MainForm.AuthorizedUser.FormattedName;
            _curPostalWeight.createddate = DateTime.Now;
            _curPostalWeight.vnd_vendor_id = (long)_cboVendor.SelectedValue;

            if (oldPostalWeight != null)
            {
                _lblInfoText.Text = string.Concat("Postal Rate Setup copied from Vendor: ", oldPostalWeight.vnd_vendorRow.description, "    Effective Date: ", oldPostalWeight.effectivedate.ToShortDateString());

                _curPostalWeight.firstoverweightlimit = oldPostalWeight.firstoverweightlimit;
                _curPostalWeight.standardoverweightlimit = oldPostalWeight.standardoverweightlimit;
                _curPostalWeight.effectivedate = oldPostalWeight.effectivedate;
                _postal.pst_postalweights.Addpst_postalweightsRow(_curPostalWeight);

                foreach (DataRow dr in _postal.pst_postalcategoryrate_map.Select(string.Concat("pst_postalweights_id = ", oldPostalWeight.pst_postalweights_id.ToString())))
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        Postal.pst_postalcategoryrate_mapRow old_row = (Postal.pst_postalcategoryrate_mapRow)dr;
                        Postal.pst_postalcategoryrate_mapRow pcr_row = _postal.pst_postalcategoryrate_map.Newpst_postalcategoryrate_mapRow();
                        pcr_row.createdby = MainForm.AuthorizedUser.FormattedName;
                        pcr_row.createddate = DateTime.Now;
                        pcr_row.pst_postalweights_id = _curPostalWeight.pst_postalweights_id;
                        pcr_row.pst_postalcategory_id = old_row.pst_postalcategory_id;
                        pcr_row.pst_postalclass_id = old_row.pst_postalclass_id;
                        pcr_row.pst_postalmailertype_id = old_row.pst_postalmailertype_id;
                        pcr_row.underweightpiecerate = old_row.underweightpiecerate;
                        pcr_row.overweightpiecerate = old_row.overweightpiecerate;
                        pcr_row.overweightpoundrate = old_row.overweightpoundrate;
                        pcr_row.active = old_row.active;
                        _postal.pst_postalcategoryrate_map.Addpst_postalcategoryrate_mapRow(pcr_row);
                    }
                }
            }
            else
            {
                _lblInfoText.Text = string.Concat("New Postal Rate Setup for Vendor: ", _cboVendor.Text.Trim());

                _curPostalWeight.firstoverweightlimit = 0;
                _curPostalWeight.standardoverweightlimit = 0;
                _curPostalWeight.effectivedate = DateTime.Today;
                _postal.pst_postalweights.Addpst_postalweightsRow(_curPostalWeight);
            }

            _postal.pst_postalcategoryrate_map.pst_postalweights_idColumn.DefaultValue = _curPostalWeight.pst_postalweights_id;
            _postal.pst_postalcategoryrate_map.createdbyColumn.DefaultValue = MainForm.AuthorizedUser.FormattedName;
            _postal.pst_postalcategoryrate_map.createddateColumn.DefaultValue = DateTime.Now;
            _dvPostalCategoryRateMaps.RowFilter = string.Concat("pst_postalweights_id = ", _curPostalWeight.pst_postalweights_id.ToString());
            
            SetControlState(CtlState.New);
            _txtFirstClassOverweightLimit.SelectionStart = 0;
            _txtFirstClassOverweightLimit.SelectionLength = _txtFirstClassOverweightLimit.Text.Length;
            _txtFirstClassOverweightLimit.Focus();

            IsLoading = false;
        }

        private void _btnUpdate_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren() && ValidateControl())
            {
                WriteToDataset();
                Reload();
            }
        }

        private void _btnDelete_Click(object sender, EventArgs e)
        {
            bool isReferenced = false;

            DialogResult answer = MessageBox.Show(Resources.DeleteRateSetWarning, "Delete Rates", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (answer == DialogResult.Yes)
            {

                foreach (DataRowView drv in _dvPostalCategoryRateMaps)
                {
                    Postal.pst_postalcategoryrate_mapRow pcr_row = (Postal.pst_postalcategoryrate_mapRow)drv.Row;
                    if (_postal.pst_postalcategoryscenario_map.Select(string.Concat("pst_postalcategoryrate_map_id = ", pcr_row.pst_postalcategoryrate_map_id.ToString())).Length > 0)
                    {
                        isReferenced = true;
                    }
                }

                if (isReferenced)
                    MessageBox.Show(Resources.PostalRateRefDeleteWarning, "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    _curPostalWeight.Delete();
                    _curPostalWeight = null;

                    if (_ctlState == CtlState.New)
                    {
                        _CurDate = _cboEffectiveDate.Text;
                    }

                    Dirty = true;
                    Reload();
                }
            }
        }

        private void _gridPostalRates_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void _gridPostalRates_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["Active"].Value = true;
        }

        #endregion

        #region Private Methods

        private void WriteToDataset()
        {
            if (_ctlState == CtlState.New)
            {
                IsLoading = true;
                _curPostalWeight.firstoverweightlimit = (decimal)_txtFirstClassOverweightLimit.Value;
                _curPostalWeight.standardoverweightlimit = (decimal)_txtStandardOverweightLimit.Value;
                _curPostalWeight.effectivedate = _dtEffectiveDate.Value.Date;
                SetControlState(CtlState.EditExisting_Clean);
                _CurDate = _dtEffectiveDate.Text;
                IsLoading = false;
            }
            else if (_ctlState == CtlState.EditExisting_Dirty)
            {
                IsLoading = true;
                _curPostalWeight.firstoverweightlimit = (decimal)_txtFirstClassOverweightLimit.Value;
                _curPostalWeight.standardoverweightlimit = (decimal)_txtStandardOverweightLimit.Value;
                _curPostalWeight.modifiedby = MainForm.AuthorizedUser.FormattedName;
                _curPostalWeight.modifieddate = DateTime.Now;
                foreach (DataRowView drv in _dvPostalCategoryRateMaps)
                {
                    Postal.pst_postalcategoryrate_mapRow pcr_row = (Postal.pst_postalcategoryrate_mapRow)drv.Row;
                    pcr_row.modifiedby = MainForm.AuthorizedUser.FormattedName;
                    pcr_row.modifieddate = DateTime.Now;
                }
                SetControlState(CtlState.EditExisting_Clean);
                IsLoading = false;
            }
        }

        private bool ValidateControl()
        {
            bool isValid = true;
            _errorProvider.Clear();

            if (_ctlState == CtlState.New)
            {
                if (_postal.pst_postalweights.Select(string.Concat("vnd_vendor_id = ", _curPostalWeight.vnd_vendor_id.ToString()
                   , " and effectivedate = '", _dtEffectiveDate.Value.ToShortDateString(), "' and pst_postalweights_id <> ", _curPostalWeight.pst_postalweights_id.ToString())).Length > 0)
                {
                    _errorProvider.SetError(_dtEffectiveDate, "A set of rates already exist for this vendor on the specified effective date");
                    isValid = false;
                }
            }

            if (_ctlState == CtlState.New || _ctlState == CtlState.EditExisting_Dirty)
            {
                if (_txtFirstClassOverweightLimit.Text.Trim() == string.Empty)
                {
                    _errorProvider.SetError(_txtFirstClassOverweightLimit, Properties.Resources.RequiredFieldError);
                    isValid = false;
                }

                if (_txtStandardOverweightLimit.Text.Trim() == string.Empty)
                {
                    _errorProvider.SetError(_txtStandardOverweightLimit, Properties.Resources.RequiredFieldError);
                    isValid = false;
                }
            }

            return isValid;
        }

        private void SetControlState(CtlState state)
        {
            if ((MainForm.AuthorizedUser.Right != UserRights.Admin) &&
                 (MainForm.AuthorizedUser.Right != UserRights.SuperAdmin))
            {
                state = CtlState.ReadOnly;
            }

            _ctlState = state;
            if (state == CtlState.ReadOnly)
            {
                _gridPostalRates.ReadOnly = true;
                _gridPostalRates.AllowUserToAddRows = false;
                _gridPostalRates.AllowUserToDeleteRows = false;
                _gridPostalRates.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;

                _btnNew.Enabled = false;
                _btnDelete.Enabled = false;
                _btnUpdate.Enabled = false;
                _txtFirstClassOverweightLimit.ReadOnly = true;
                _txtStandardOverweightLimit.ReadOnly = true;
                _dtEffectiveDate.Enabled = false;
                _cboEffectiveDate.Enabled = true;
                _cboVendor.Enabled = true;
            }

            else if (state == CtlState.ReadOnly_AllowNew)
            {
                _gridPostalRates.ReadOnly = true;
                _gridPostalRates.AllowUserToAddRows = false;
                _gridPostalRates.AllowUserToDeleteRows = false;
                _gridPostalRates.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;

                _btnNew.Enabled = true;
                _btnDelete.Enabled = false;
                _btnUpdate.Enabled = false;
                _txtFirstClassOverweightLimit.ReadOnly = true;
                _txtStandardOverweightLimit.ReadOnly = true;
                _dtEffectiveDate.Enabled = false;
                _cboEffectiveDate.Enabled = true;
                _cboVendor.Enabled = true;
            }

            else if (state == CtlState.New)
            {
                this.Dirty = true;
                _btnNew.Enabled = true;
                _btnDelete.Enabled = true;
                _btnUpdate.Enabled = true;
                _txtFirstClassOverweightLimit.ReadOnly = false;
                _txtStandardOverweightLimit.ReadOnly = false;
                _dtEffectiveDate.Enabled = true;
                _cboEffectiveDate.Enabled = false;
                _cboVendor.Enabled = false;

                _gridPostalRates.ReadOnly = false;
                _gridPostalRates.AllowUserToAddRows = true;
                _gridPostalRates.AllowUserToDeleteRows = true;
                _gridPostalRates.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;

                _CurVendorName = _cboVendor.Text;
                _CurDate = _cboEffectiveDate.Text;
            }
            else if (state == CtlState.EditExisting_Clean)
            {
                _btnNew.Enabled = true;
                _btnDelete.Enabled = true;
                _btnUpdate.Enabled = false;
                _txtFirstClassOverweightLimit.ReadOnly = false;
                _txtStandardOverweightLimit.ReadOnly = false;
                _dtEffectiveDate.Enabled = false;
                _cboEffectiveDate.Enabled = true;
                _cboVendor.Enabled = true;

                _gridPostalRates.ReadOnly = false;
                _gridPostalRates.AllowUserToAddRows = true;
                _gridPostalRates.AllowUserToDeleteRows = true;
                _gridPostalRates.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;

                _CurVendorName = _cboVendor.Text;
                _CurDate = _cboEffectiveDate.Text;
            }
            else if (state == CtlState.EditExisting_Dirty)
            {
                this.Dirty = true;
                _btnNew.Enabled = true;
                _btnDelete.Enabled = true;
                _btnUpdate.Enabled = true;
                _txtStandardOverweightLimit.ReadOnly = false;
                _txtStandardOverweightLimit.ReadOnly = false;
                _dtEffectiveDate.Enabled = false;
                _cboEffectiveDate.Enabled = false;
                _cboVendor.Enabled = false;

                _gridPostalRates.ReadOnly = false;
                _gridPostalRates.AllowUserToAddRows = true;
                _gridPostalRates.AllowUserToDeleteRows = true;
                _gridPostalRates.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
            }
        }

        private void ClearControl()
        {
            _lblInfoText.Text = string.Empty;
            _txtFirstClassOverweightLimit.Text = string.Empty;
            _txtStandardOverweightLimit.Text = string.Empty;
            _dtEffectiveDate.Value = DateTime.Today;

            _CurVendorName = string.Empty;
            _CurDate = string.Empty;
        }

        private bool IsUsedByScenario(DataRowView dataRow)
        {
            bool used = false;
            Postal.pst_postalcategoryrate_mapRow crr = (Postal.pst_postalcategoryrate_mapRow)dataRow.Row;

            if (_postal.pst_postalcategoryscenario_map.Select(string.Concat("pst_postalcategoryrate_map_id = ", crr.pst_postalcategoryrate_map_id.ToString())).Length > 0)
                used = true;

            return used;
        }

        #endregion

    }
}

