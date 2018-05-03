#region Using Directives

using System;
using System.Collections;
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
    public partial class ucpPostalScenarioSetup : CatalogEstimating.UserControlPanel
    {
        #region Private Variables

        private enum CtlState
        {
            ReadOnly = 1,
            ReadOnly_AllowNew = 2,
            New = 3,
            New_WithEffectiveDate = 4,
            EditExisting_Clean = 5,
            EditExisting_Dirty = 6
        }
        private CtlState _ctlState = CtlState.ReadOnly;
        private Postal _postal = null;
        private Postal.pst_postalscenarioDataTable _EffectiveDatesforScenario = new Postal.pst_postalscenarioDataTable();
        private Postal.pst_postalscenarioRow _currentScenarioRow = null;
        private string _CurVendorName = string.Empty;
        private string _CurScenario = string.Empty;
        private string _CurDate = string.Empty;

        #endregion

        #region Construction

        public ucpPostalScenarioSetup()
        {
            InitializeComponent();

            Name = "Scenario Setup";
        }

        public ucpPostalScenarioSetup(Postal ds)
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
            _postal.vnd_vendor.DefaultView.RowFilter = "active = 1";
            _EffectiveDatesforScenario.DefaultView.Sort = "effectivedate DESC";
            _cboVendor.DataSource = _postal.vnd_vendor;
            _cboVendor.DisplayMember = "description";
            _cboVendor.ValueMember = "vnd_vendor_id";

            _cboEffectiveDate.DataSource = _EffectiveDatesforScenario;
            _cboEffectiveDate.DisplayMember = "effectivedate";
            _cboEffectiveDate.ValueMember = "pst_postalscenario_id";

            _cboPostageClass.DataSource = _postal.pst_postalclass;
            _cboPostageClass.DisplayMember = "description";
            _cboPostageClass.ValueMember = "pst_postalclass_id";

            _cboPostalMailerType.DataSource = _postal.pst_postalmailertype;
            _cboPostalMailerType.DisplayMember = "description";
            _cboPostalMailerType.ValueMember = "pst_postalmailertype_id";

            SetControlState(CtlState.ReadOnly);

            IsLoading = false; 
        }

        public override void Reload()
        {
            string vndName = _CurVendorName;
            string scenario = _CurScenario;
            string effDate = _CurDate;

            SetControlState(CtlState.ReadOnly_AllowNew);
            _errorProvider.Clear();

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

                idx = _cboPostalScenario.FindStringExact(scenario);
                if ((idx > -1) && (idx != _cboPostalScenario.SelectedIndex))
                {
                    _cboPostalScenario.SelectedIndex = idx;
                }
                else
                {
                    _cboPostalScenario_SelectedIndexChanged(this, EventArgs.Empty);
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

        public override void Export( ref ExcelWriter writer)
        {
            writer.WriteLine("Vendor", _cboVendor.Text);
            writer.WriteLine("Scenario", _cboPostalScenario.Text);
            writer.WriteLine("Effective Date", _dtEffectiveDate.Text);
            writer.WriteLine("");
            writer.WriteLine("Scenario Description", _txtDescription.Text);
            writer.WriteLine("Scenario Comments", _txtComments.Text);
            writer.WriteLine("");
            writer.WriteLine("Postage Class", _cboPostageClass.Text);
            writer.WriteLine("Mailer Type", _cboPostalMailerType.Text);
            writer.WriteLine("");
            writer.WriteTable(_gridPostalCategoryScenarioSetup, true);
        }

        public override void PreSave(CancelEventArgs e)
        {
            // Force value to be sync'd with what may have been typed into date control.
            MainForm.UpdateDateControl(_dtEffectiveDate);

            if (!ValidateControl())
            {
                MessageBox.Show(Resources.InvalidData, "Cannot Save", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
            else if (!ValidateChildren())
            {
                MessageBox.Show(Resources.InvalidData, "Cannot Save", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
            else if (_ctlState == CtlState.New)
            {
                DialogResult result = MessageBox.Show(Resources.PostalScenarioIncomplete, "Cannot Save", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
            else
            {
                // Do an implicit update if the data is valid
                WriteToDataSet();
            }
        }

        public override void OnLeaving(CancelEventArgs e)
        {
            if (!ValidateControl())
            {
                e.Cancel = true;
                return;
            }

            if (_ctlState == CtlState.New)
            {
                DialogResult result = MessageBox.Show(string.Concat(Resources.ChangesMade, "  ", Resources.LoseUnsavedChanges), "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    int idx = 0;

                    DataView dvPostalCategoryScenarioMap = new DataView(_postal.pst_postalcategoryscenario_map);
                    dvPostalCategoryScenarioMap.RowFilter = string.Concat("pst_postalscenario_id = ", _currentScenarioRow.pst_postalscenario_id.ToString());

                    for (idx = dvPostalCategoryScenarioMap.Count - 1; idx >= 0; idx--)
                        dvPostalCategoryScenarioMap[idx].Delete();

                    _currentScenarioRow.Delete();
                    _currentScenarioRow = null;

                    SetControlState(CtlState.ReadOnly_AllowNew);
                    _errorProvider.Clear();
                    Reload();
                }
                else 
                    e.Cancel = true;

                return;
            }
            else if ((_ctlState == CtlState.New_WithEffectiveDate)
                || (_ctlState == CtlState.EditExisting_Dirty))
            {
                DialogResult result = MessageBox.Show(Resources.LeaveTabCancelWarning, "Are you sure?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    int idx = 0;

                    DataView dvPostalCategoryScenarioMap = new DataView(_postal.pst_postalcategoryscenario_map);
                    dvPostalCategoryScenarioMap.RowFilter = string.Concat("pst_postalscenario_id = ", _currentScenarioRow.pst_postalscenario_id.ToString());

                    for (idx = dvPostalCategoryScenarioMap.Count - 1; idx >= 0; idx--)
                        dvPostalCategoryScenarioMap[idx].Delete();

                    _currentScenarioRow.Delete();
                    _currentScenarioRow = null;

                    SetControlState(CtlState.ReadOnly_AllowNew);
                    _errorProvider.Clear();
                    Reload();
                }
                else if (result == DialogResult.No)
                {
                    _btnUpdate_Click(this, EventArgs.Empty);
                }
                else if (result == DialogResult.Cancel)
                    e.Cancel = true;

                return;
            }

            //// Do an implicit "update" if the data is valid
            //WriteToDataSet();
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

        private void _btnNew_Click(object sender, EventArgs e)
        {
            SetControlState(CtlState.New);
            ClearControl();
            _errorProvider.Clear();

            Postal.pst_postalscenarioRow oldPostalScenario = null;

            if (_currentScenarioRow != null)
                oldPostalScenario = _currentScenarioRow;

            _lblInfoText.Text = string.Concat("New Postal Scenario for Vendor: ", _cboVendor.Text.Trim());

            InitCategorySetupNewScenario();
            if (_lbPostalCategory.Items.Count == 0)
            {
                _btnAdd.Enabled = false;
                _btnAddAll.Enabled = false;
            }
            else
            {
                _btnAdd.Enabled = true;
                _btnAddAll.Enabled = true;
            }

            _currentScenarioRow = _postal.pst_postalscenario.Newpst_postalscenarioRow();
            _currentScenarioRow.createdby = MainForm.AuthorizedUser.FormattedName;
            _currentScenarioRow.createddate = DateTime.Now;
            _txtDescription.SelectionStart = 0;
            _txtDescription.SelectionLength = _txtDescription.Text.Length;
            _txtDescription.Focus();
        }

        private void _txtDescription_TextChanged(object sender, EventArgs e)
        {
            if (_ctlState == CtlState.EditExisting_Clean)
            {
                SetControlState(CtlState.EditExisting_Dirty);
            }
        }

        private void _txtComments_TextChanged(object sender, EventArgs e)
        {
            if (_ctlState == CtlState.EditExisting_Clean)
            {
                SetControlState(CtlState.EditExisting_Dirty);
            }
        }

        private void _cboPostageClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((_cboPostageClass.SelectedIndex > -1) && (_ctlState == CtlState.New))
            {
                InitCategorySetupNewScenario();
                if (_lbPostalCategory.Items.Count == 0)
                {
                    _btnAdd.Enabled = false;
                    _btnAddAll.Enabled = false;
                }
                else
                {
                    _btnAdd.Enabled = true;
                    _btnAddAll.Enabled = true;
                }
            }
        }

        private void _cboPostalMailerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((_cboPostalMailerType.SelectedIndex > -1) && (_ctlState == CtlState.New))
            {
                InitCategorySetupNewScenario();
                if (_lbPostalCategory.Items.Count == 0)
                {
                    _btnAdd.Enabled = false;
                    _btnAddAll.Enabled = false;
                }
                else
                {
                    _btnAdd.Enabled = true;
                    _btnAddAll.Enabled = true;
                }
            }
        }

        private void _dtEffectiveDate_ValueChanged(object sender, EventArgs e)
        {
            if (_ctlState == CtlState.New)
            {
                InitCategorySetupNewScenario();
                if (_lbPostalCategory.Items.Count == 0)
                {
                    _btnAdd.Enabled = false;
                    _btnAddAll.Enabled = false;
                }
                else
                {
                    _btnAdd.Enabled = true;
                    _btnAddAll.Enabled = true;
                }
            }
        }

        private void _cboVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.IsLoading)
                return;

            // Clear the Scenario Description dropdown
            _cboPostalScenario.Items.Clear();
            // Clear the datasource for the effective date dropdown
            _EffectiveDatesforScenario.Clear();
            List<string> scenariosForVendor = new List<string>();

            // If a vendor has not been selected mark the control read only and return
            if (_cboVendor.SelectedItem == null)
            {
                SetControlState(CtlState.ReadOnly);
                return;
            }

            // Iterate through the _postal dataset to find the selected vendor's postal scenarios
            #region iterate through _postal dataset
            foreach (DataRow pwr in _postal.pst_postalweights.Select(string.Concat("vnd_vendor_id = ", _cboVendor.SelectedValue.ToString())))
            {
                Postal.pst_postalweightsRow pw_row = (Postal.pst_postalweightsRow)pwr;
                foreach (DataRow pcr in _postal.pst_postalcategoryrate_map.Select(string.Concat("pst_postalweights_id = ", pw_row.pst_postalweights_id.ToString())))
                {
                    Postal.pst_postalcategoryrate_mapRow pcr_row = (Postal.pst_postalcategoryrate_mapRow)pcr;
                    foreach (DataRow pcs in _postal.pst_postalcategoryscenario_map.Select(string.Concat("pst_postalcategoryrate_map_id = ", pcr_row.pst_postalcategoryrate_map_id.ToString())))
                    {
                        Postal.pst_postalcategoryscenario_mapRow pcs_row = (Postal.pst_postalcategoryscenario_mapRow) pcs;

                        if (!scenariosForVendor.Contains(pcs_row.pst_postalscenarioRow.description))
                        {
                            _cboPostalScenario.Items.Add(pcs_row.pst_postalscenarioRow.description);
                            scenariosForVendor.Add(pcs_row.pst_postalscenarioRow.description);
                        }
                    }
                }
            }
            #endregion

            _errorProvider.Clear();

            if (_cboPostalScenario.Items.Count == 0)
            {
                SetControlState(CtlState.ReadOnly_AllowNew);
                ClearControl();
                _cboPostalScenario.Enabled = false;
                _cboPostalScenario.Text = string.Empty;
                _cboEffectiveDate.Enabled = false;
                _cboEffectiveDate.Text = string.Empty;
            }
            else
            {
                if (!_cboPostalScenario.Enabled)
                    _cboPostalScenario.Enabled = true;
                if (_cboPostalScenario.SelectedIndex == -1)
                    _cboPostalScenario.SelectedIndex = 0;
                else
                    _cboPostalScenario_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        private void _cboPostalScenario_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.IsLoading)
                return;

            _EffectiveDatesforScenario.Clear();

            if (_cboPostalScenario.SelectedItem == null)
                return;

            // A Vendor and Postal Scenario have been selected.  Now find all postal scenarios for matching vendor with the same description.
            // This will allow us to populate the effective date dropdown.
            foreach (DataRow pw in _postal.pst_postalweights.Select(string.Concat("vnd_vendor_id = ", _cboVendor.SelectedValue.ToString())))
            {
                Postal.pst_postalweightsRow pw_row = (Postal.pst_postalweightsRow)pw;
                foreach (DataRow pcr in _postal.pst_postalcategoryrate_map.Select(string.Concat("pst_postalweights_id = ", pw_row.pst_postalweights_id.ToString())))
                {
                    Postal.pst_postalcategoryrate_mapRow pcr_row = (Postal.pst_postalcategoryrate_mapRow) pcr;
                    foreach (DataRow pcs in _postal.pst_postalcategoryscenario_map.Select(string.Concat("pst_postalcategoryrate_map_id = ", pcr_row.pst_postalcategoryrate_map_id.ToString())))
                    {
                        Postal.pst_postalcategoryscenario_mapRow pcs_row = (Postal.pst_postalcategoryscenario_mapRow) pcs;

                        DataView dvEffDates = new DataView(_EffectiveDatesforScenario);
                        dvEffDates.RowFilter = string.Concat("pst_postalscenario_id = ", pcs_row.pst_postalscenario_id);

                        Postal.pst_postalscenarioRow ps_row = _postal.pst_postalscenario.FindBypst_postalscenario_id(pcs_row.pst_postalscenario_id);
                        if ((dvEffDates.Count == 0) && (ps_row.description == _cboPostalScenario.Text))
                            ((DataTable) _EffectiveDatesforScenario).Rows.Add(ps_row.ItemArray);
                    }
                }
            }

            if (!_cboEffectiveDate.Enabled)
                _cboEffectiveDate.Enabled = true;
            if (_cboEffectiveDate.SelectedIndex == -1)
                _cboEffectiveDate.SelectedIndex = 0;
            else
                _cboEffectiveDate_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void _cboEffectiveDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.IsLoading)
                return;

            SetControlState(CtlState.ReadOnly_AllowNew);

            if (_cboEffectiveDate.SelectedItem == null)
                return;
            
            _currentScenarioRow = _postal.pst_postalscenario.FindBypst_postalscenario_id((long) _cboEffectiveDate.SelectedValue);

            _lblInfoText.Text = string.Concat("Vendor: ", _cboVendor.Text.Trim(), "    Scenario: ", _cboPostalScenario.Text.Trim(), "    Effective Date: ", _cboEffectiveDate.Text);
            _txtDescription.Text = _currentScenarioRow.description;

            if (_currentScenarioRow.IscommentsNull())
                _txtComments.Text = String.Empty;
            else
                _txtComments.Text = _currentScenarioRow.comments;

            _dtEffectiveDate.Value = _currentScenarioRow.effectivedate;
            _cboPostalMailerType.SelectedValue = _currentScenarioRow.pst_postalmailertype_id;
            _cboPostageClass.SelectedValue = _currentScenarioRow.pst_postalclass_id;
            SetControlState(CtlState.EditExisting_Clean);
            InitCategorySetupExistingScenario();
            Percent100();
            _errorProvider.Clear();
        }

        private void _gridPostalCategoryScenarioSetup_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                Percent100();

                if (_ctlState == CtlState.EditExisting_Clean)
                {
                    SetControlState(CtlState.EditExisting_Dirty);
                }

                if (!string.IsNullOrEmpty(_gridPostalCategoryScenarioSetup[e.ColumnIndex, e.RowIndex].Value.ToString()))
                {
                    string nonFmt = _gridPostalCategoryScenarioSetup[e.ColumnIndex, e.RowIndex].Value.ToString();
                    string fmt = decimal.Parse(nonFmt).ToString("0.00");

                    if (nonFmt != fmt)
                        _gridPostalCategoryScenarioSetup[e.ColumnIndex, e.RowIndex].Value = fmt;
                }
            }
        }

        private void _btnAdd_Click(object sender, EventArgs e)
        {
            if (_ctlState == CtlState.New && _lbPostalCategory.SelectedItems.Count > 0)
            {
                if (ValidateControl() && ValidateChildren())
                    SetControlState(CtlState.New_WithEffectiveDate);
                else
                    return;
            }

            foreach (KeyValuePair<long, string> catmap in _lbPostalCategory.SelectedItems)
                _gridPostalCategoryScenarioSetup.Rows.Add(catmap.Key, catmap.Value, 0);

            while (_lbPostalCategory.SelectedItems.Count > 0)
                _lbPostalCategory.Items.Remove(_lbPostalCategory.SelectedItems[0]);
        }

        private void _btnAddAll_Click(object sender, EventArgs e)
        {
            if (_ctlState == CtlState.New)
            {
                if (ValidateControl() && ValidateChildren())
                    SetControlState(CtlState.New_WithEffectiveDate);
                else
                    return;
            } 
            
            foreach (KeyValuePair<long, string> catmap in _lbPostalCategory.Items)
                _gridPostalCategoryScenarioSetup.Rows.Add(catmap.Key, catmap.Value, 0);

            _lbPostalCategory.Items.Clear();
        }

        private void _btnRemove_Click(object sender, EventArgs e)
        {
            if (_ctlState == CtlState.New)
            {
                if (ValidateControl() && ValidateChildren())
                    SetControlState(CtlState.New_WithEffectiveDate);
                else
                    return;
            } 
            
            List<int> idxs = new List<int>();
            for (int idx = _gridPostalCategoryScenarioSetup.Rows.Count - 1; idx >= 0; idx--)
            {
                if (_gridPostalCategoryScenarioSetup.Rows[idx].Cells[1].Selected || _gridPostalCategoryScenarioSetup.Rows[idx].Cells[2].Selected)
                {
                    _lbPostalCategory.Items.Add(new KeyValuePair<long, string>((long)_gridPostalCategoryScenarioSetup.Rows[idx].Cells["pst_postalcategoryrate_map_id"].Value, _gridPostalCategoryScenarioSetup.Rows[idx].Cells["Category"].Value.ToString()));
                    //_gridPostalCategoryScenarioSetup.Rows.RemoveAt(idx);
                    idxs.Add(idx);
                }
            }

            foreach (int idx in idxs)
            {
                _gridPostalCategoryScenarioSetup.Rows.RemoveAt(idx);
            }
        }

        private void _btnRemoveAll_Click(object sender, EventArgs e)
        {
            if (_ctlState == CtlState.New)
            {
                if (ValidateControl() && ValidateChildren())
                    SetControlState(CtlState.New_WithEffectiveDate);
                else
                    return;
            }

            for (int idx = _gridPostalCategoryScenarioSetup.Rows.Count - 1; idx >= 0; idx--)
            {
                _lbPostalCategory.Items.Add(new KeyValuePair<long, string>((long)_gridPostalCategoryScenarioSetup.Rows[idx].Cells["pst_postalcategoryrate_map_id"].Value, _gridPostalCategoryScenarioSetup.Rows[idx].Cells["Category"].Value.ToString()));
                _gridPostalCategoryScenarioSetup.Rows.RemoveAt(idx);
            }
        }

        private void _btnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateControl() && ValidateChildren())
            {
                _CurScenario = _txtDescription.Text;
                _CurDate = _dtEffectiveDate.Text;

                WriteToDataSet();
                Reload();
            }
        }

        private void _btnDelete_Click(object sender, EventArgs e)
        {
            if (_currentScenarioRow == null)
                return;

            DialogResult answer = MessageBox.Show(Resources.DeleteScenarioWarning, "Delete Scenario", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (answer == DialogResult.Yes)
            {
                int idx = 0;

                DataView dvPostalCategoryScenarioMap = new DataView(_postal.pst_postalcategoryscenario_map);
                dvPostalCategoryScenarioMap.RowFilter = string.Concat("pst_postalscenario_id = ", _currentScenarioRow.pst_postalscenario_id.ToString());

                for (idx = dvPostalCategoryScenarioMap.Count - 1; idx >= 0; idx--)
                    dvPostalCategoryScenarioMap[idx].Delete();

                _currentScenarioRow.Delete();
                _currentScenarioRow = null;

                SetControlState(CtlState.ReadOnly_AllowNew);
                _errorProvider.Clear();
                this.Dirty = true;
                Reload();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the Category Setup groupbox for a new Postal Scenario.
        /// Fills the Postal Category listbox.
        /// </summary>
        private void InitCategorySetupNewScenario()
        {
            // Clear the listbox and grid
            _lbPostalCategory.Items.Clear();
            _gridPostalCategoryScenarioSetup.Rows.Clear();

            // Get a list of all applicable postal categories
            DataView dvPostalWeight = new DataView(_postal.pst_postalweights);
            dvPostalWeight.RowFilter = string.Concat("vnd_vendor_id = ", _cboVendor.SelectedValue.ToString(), " and effectivedate <= '", _dtEffectiveDate.Value.ToShortDateString(), "'");
            dvPostalWeight.Sort = "effectivedate desc";

            if (dvPostalWeight.Count > 0)
            {
                Postal.pst_postalweightsRow curPostalWeight = ((Postal.pst_postalweightsRow)dvPostalWeight[0].Row);
                DataView dvPostalCategoryRateMap = new DataView(_postal.pst_postalcategoryrate_map);
                dvPostalCategoryRateMap.RowFilter = string.Concat("pst_postalweights_id = ", curPostalWeight.pst_postalweights_id.ToString()
                   , " and pst_postalclass_id = ", _cboPostageClass.SelectedValue.ToString()
                   , " and pst_postalmailertype_id = ", _cboPostalMailerType.SelectedValue.ToString()
                   , " and active = 1");

                foreach (DataRowView drv in dvPostalCategoryRateMap)
                {
                    Postal.pst_postalcategoryrate_mapRow pcr_row = (Postal.pst_postalcategoryrate_mapRow)drv.Row;
                    _lbPostalCategory.Items.Add(new KeyValuePair<long, string>(pcr_row.pst_postalcategoryrate_map_id, pcr_row.pst_postalcategoryRow.description));
                }
            }
        }

        /// <summary>
        /// Initializes the Category Setup groupbox for an existing Postal Scenario.
        /// Fills the Categories in Scenario grid.
        /// </summary>
        private void InitCategorySetupExistingScenario()
        {
            //Clear the listbox and the grid
            _lbPostalCategory.Items.Clear();
            _gridPostalCategoryScenarioSetup.Rows.Clear();

            // Get a list of the postal categories included in the scenario
            DataView dvPostalCategoryScenarioMap = new DataView(_postal.pst_postalcategoryscenario_map);
            dvPostalCategoryScenarioMap.RowFilter = string.Concat("pst_postalscenario_id = ", _currentScenarioRow.pst_postalscenario_id.ToString());
            foreach (DataRowView drv in dvPostalCategoryScenarioMap)
            {
                Postal.pst_postalcategoryscenario_mapRow pcs_row = (Postal.pst_postalcategoryscenario_mapRow)drv.Row;
                _gridPostalCategoryScenarioSetup.Rows.Add(pcs_row.pst_postalcategoryrate_map_id.ToString(),
                    pcs_row.pst_postalcategoryrate_mapRow.pst_postalcategoryRow.description, pcs_row.percentage * 100M);
            }
        }

        private void WriteToDataSet()
        {
            if (_ctlState == CtlState.New_WithEffectiveDate)
            {
                _currentScenarioRow.description = _txtDescription.Text.Trim();
                _currentScenarioRow.comments = _txtComments.Text.Trim();
                _currentScenarioRow.effectivedate = _dtEffectiveDate.Value.Date;
                _currentScenarioRow.pst_postalclass_id = (int)_cboPostageClass.SelectedValue;
                _currentScenarioRow.pst_postalmailertype_id = (int)_cboPostalMailerType.SelectedValue;
                _postal.pst_postalscenario.Addpst_postalscenarioRow(_currentScenarioRow);

                foreach (DataGridViewRow dgv_row in _gridPostalCategoryScenarioSetup.Rows)
                {
                    Postal.pst_postalcategoryscenario_mapRow pcs_row = _postal.pst_postalcategoryscenario_map.Newpst_postalcategoryscenario_mapRow();
                    pcs_row.pst_postalcategoryrate_map_id = (long)dgv_row.Cells["pst_postalcategoryrate_map_id"].Value;
                    pcs_row.pst_postalscenario_id = _currentScenarioRow.pst_postalscenario_id;
                    pcs_row.percentage = Convert.ToDecimal(dgv_row.Cells["Percentage"].Value) / 100M;
                    pcs_row.createdby = MainForm.AuthorizedUser.FormattedName;
                    pcs_row.createddate = DateTime.Now;
                    _postal.pst_postalcategoryscenario_map.Addpst_postalcategoryscenario_mapRow(pcs_row);
                }
            }
            else if (_ctlState == CtlState.EditExisting_Dirty)
            {
                _currentScenarioRow.description = _txtDescription.Text.Trim();
                _currentScenarioRow.comments = _txtComments.Text.Trim();
                _currentScenarioRow.modifiedby = MainForm.AuthorizedUser.FormattedName;
                _currentScenarioRow.modifieddate = DateTime.Now;
                _currentScenarioRow.comments = _txtComments.Text.Trim();

                foreach (DataGridViewRow dgv_row in _gridPostalCategoryScenarioSetup.Rows)
                {
                    Postal.pst_postalcategoryscenario_mapRow pcs_row = _postal.pst_postalcategoryscenario_map.FindBypst_postalscenario_idpst_postalcategoryrate_map_id(_currentScenarioRow.pst_postalscenario_id, Convert.ToInt64(dgv_row.Cells["pst_postalcategoryrate_map_id"].Value));
                    pcs_row.modifiedby = MainForm.AuthorizedUser.FormattedName;
                    pcs_row.modifieddate = DateTime.Now;
                    pcs_row.percentage = Convert.ToDecimal(dgv_row.Cells["Percentage"].Value) / 100M;
                }
            }
        }

        private bool ValidateRequired(Control ctrl, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(ctrl.Text.Trim()))
            {
                _errorProvider.SetError(ctrl, Resources.RequiredFieldError);
                e.Cancel = true;
                return false;
            }

            _errorProvider.SetError(ctrl, string.Empty);
            return true;
        }

        private void ValidateRequiredSelection(ComboBox cbo, CancelEventArgs e)
        {
            if (IsLoading)
                return;

            if (cbo.SelectedIndex == -1)
            {
                _errorProvider.SetError(cbo, Resources.RequiredFieldError);
                e.Cancel = true;
            }
            else
                _errorProvider.SetError(cbo, string.Empty);
        }

        /// <summary>
        /// Determines if a Postal Scenario with the same Description already exists for this vendor.
        /// </summary>
        /// <param name="e"></param>
        private void ValidateEffectiveDate(CancelEventArgs e)
        {
            // Get a list of all applicable postal categories
            DataView dvPostalWeight = new DataView(_postal.pst_postalweights);
            dvPostalWeight.RowFilter = string.Concat("vnd_vendor_id = ", _cboVendor.SelectedValue.ToString());
            foreach (DataRowView pw_drv in dvPostalWeight)
            {
                Postal.pst_postalweightsRow pw_row = (Postal.pst_postalweightsRow) pw_drv.Row;
                DataView dvPostalCategoryRateMap = new DataView(_postal.pst_postalcategoryrate_map);
                dvPostalCategoryRateMap.RowFilter = string.Concat("pst_postalweights_id = ", pw_row.pst_postalweights_id.ToString());
                foreach (DataRowView pcr_drv in dvPostalCategoryRateMap)
                {
                    Postal.pst_postalcategoryrate_mapRow pcr_row = (Postal.pst_postalcategoryrate_mapRow) pcr_drv.Row;
                    DataView dvPostalCategoryScenarioMap = new DataView(_postal.pst_postalcategoryscenario_map);
                    dvPostalCategoryScenarioMap.RowFilter = string.Concat("pst_postalcategoryrate_map_id = ", pcr_row.pst_postalcategoryrate_map_id.ToString());
                    foreach (DataRowView pcs_drv in dvPostalCategoryScenarioMap)
                    {
                        Postal.pst_postalcategoryscenario_mapRow pcs_row = (Postal.pst_postalcategoryscenario_mapRow) pcs_drv.Row;
                        Postal.pst_postalscenarioRow ps_row = pcs_row.pst_postalscenarioRow;
                        if (ps_row != _currentScenarioRow && ps_row.description == _txtDescription.Text.Trim() && ps_row.effectivedate == _dtEffectiveDate.Value.Date)
                        {
                            _errorProvider.SetError(_dtEffectiveDate, "A postal scenario already exists with the specified description and effective date.");
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }

            _errorProvider.SetError(_dtEffectiveDate, String.Empty);
            return;
        }

        private bool ValidateControl()
        {
            _errorProvider.Clear();
            _gridPostalCategoryScenarioSetup.EndEdit();

            CancelEventArgs cea = new CancelEventArgs();

            if (_ctlState == CtlState.ReadOnly || _ctlState == CtlState.ReadOnly_AllowNew || _ctlState == CtlState.EditExisting_Clean)
            {
                return true;
            }

            else if (_ctlState == CtlState.New)
            {
                ValidateRequired(_txtDescription, cea);
                ValidateEffectiveDate(cea);
                ValidateRequiredSelection(_cboPostageClass, cea);
                ValidateRequiredSelection(_cboPostalMailerType, cea);
                return !cea.Cancel;
            }

            else if (_ctlState == CtlState.New_WithEffectiveDate || _ctlState == CtlState.EditExisting_Dirty)
            {
                ValidateRequired(_txtDescription, cea);
                if (!Percent100())
                {
                    _errorProvider.SetError(_txtTotalPercentage, "Total must equal 100%");
                    cea.Cancel = true;
                }
                else
                {
                    _errorProvider.SetError(_txtTotalPercentage, String.Empty);
                }

                return !cea.Cancel;
            }

            throw new InvalidEnumArgumentException(string.Concat("Control State ", _ctlState.ToString(), " is undefined."));
        }

        private bool Percent100()
        {
            bool isPercent100 = false;
            decimal total = 0;

            for (int idx = 0; idx < _gridPostalCategoryScenarioSetup.RowCount; idx++)
            {
                if ( !string.IsNullOrEmpty( _gridPostalCategoryScenarioSetup["Percentage", idx].EditedFormattedValue.ToString() ) )
                    total += Convert.ToDecimal(_gridPostalCategoryScenarioSetup["Percentage", idx].Value);
            }

            _txtTotalPercentage.Value = total;

            if (total == Convert.ToDecimal(100))
            {
                isPercent100 = true;
            }

            return isPercent100;
        }

        private void SetControlState(CtlState state)
        {
            // If the user is not an Admin or SuperAdmin the control is always readonly
            if ((MainForm.AuthorizedUser.Right != UserRights.Admin) &&
                 (MainForm.AuthorizedUser.Right != UserRights.SuperAdmin))
            {
                state = CtlState.ReadOnly;
            }

            _ctlState = state;

            if (state == CtlState.ReadOnly)
            {
                _btnNew.Enabled = false;
                _btnDelete.Enabled = false;
                _cboVendor.Enabled = true;
                _cboPostalScenario.Enabled = false;
                _cboEffectiveDate.Enabled = false;

                _txtDescription.ReadOnly = true;
                _txtComments.ReadOnly = true;
                _dtEffectiveDate.Enabled = false;
                _cboPostalMailerType.Enabled = false;
                _cboPostageClass.Enabled = false;

                _lbPostalCategory.Enabled = false;
                _btnAdd.Enabled = false;
                _btnAddAll.Enabled = false;
                _btnRemove.Enabled = false;
                _btnRemoveAll.Enabled = false;
                _gridPostalCategoryScenarioSetup.ReadOnly = true;
                _gridPostalCategoryScenarioSetup.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                _btnUpdate.Enabled = false;
            }

            // If the state is editable the search criteria and new button are enabled
            if (state == CtlState.ReadOnly_AllowNew)
            {
                _btnNew.Enabled = true;
                _btnDelete.Enabled = false;
                _cboVendor.Enabled = true;
                _cboPostalScenario.Enabled = false;
                _cboEffectiveDate.Enabled = false;

                _gridPostalCategoryScenarioSetup.CurrentCell = null;

                _txtDescription.ReadOnly = true;
                _txtComments.ReadOnly = true;
                _dtEffectiveDate.Enabled = false;
                _cboPostalMailerType.Enabled = false;
                _cboPostageClass.Enabled = false;

                _lbPostalCategory.Enabled = false;
                _btnAdd.Enabled = false;
                _btnAddAll.Enabled = false;
                _btnRemove.Enabled = false;
                _btnRemoveAll.Enabled = false;
                _gridPostalCategoryScenarioSetup.ReadOnly = true;
                _gridPostalCategoryScenarioSetup.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                _btnUpdate.Enabled = false;
            }

            else if (state == CtlState.New)
            {
                this.Dirty = true;
                _btnNew.Enabled = false;
                _btnDelete.Enabled = false;
                _cboVendor.Enabled = false;
                _cboPostalScenario.Enabled = false;
                _cboEffectiveDate.Enabled = false;
                
                _txtDescription.Text = String.Empty;
                _txtComments.Text = String.Empty;
                _dtEffectiveDate.Value = DateTime.Today;
                _cboPostalMailerType.SelectedValue = 1;
                _cboPostageClass.SelectedValue = 1;
                _txtDescription.ReadOnly = false;
                _txtComments.ReadOnly = false;
                _dtEffectiveDate.Enabled = true;
                _cboPostalMailerType.Enabled = true;
                _cboPostageClass.Enabled = true;

                _lbPostalCategory.Items.Clear();
                _gridPostalCategoryScenarioSetup.Rows.Clear();
                _lbPostalCategory.Enabled = true;
                _btnAdd.Enabled = false;
                _btnAddAll.Enabled = false;
                _btnRemove.Enabled = false;
                _btnRemoveAll.Enabled = false;
                _gridPostalCategoryScenarioSetup.ReadOnly = true;
                _gridPostalCategoryScenarioSetup.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                _gridPostalCategoryScenarioSetup.CurrentCell = null;
                _txtTotalPercentage.Text = String.Empty;
                _btnUpdate.Enabled = false;

                _CurVendorName = _cboVendor.Text;
                _CurScenario = _cboPostalScenario.Text;
                _CurDate = _cboEffectiveDate.Text;
            }

            else if (state == CtlState.New_WithEffectiveDate)
            {
                this.Dirty = true;
                _btnNew.Enabled = false;
                _btnDelete.Enabled = false;
                _cboVendor.Enabled = false;
                _cboPostalScenario.Enabled = false;
                _cboEffectiveDate.Enabled = false;

                _txtDescription.ReadOnly = false;
                _txtComments.ReadOnly = false;
                _dtEffectiveDate.Enabled = false;
                _cboPostalMailerType.Enabled = false;
                _cboPostageClass.Enabled = false;

                //InitCategorySetupNewScenario();
                _lbPostalCategory.Enabled = true;
                _btnAdd.Enabled = true;
                _btnAddAll.Enabled = true;
                _btnRemove.Enabled = true;
                _btnRemoveAll.Enabled = true;
                _gridPostalCategoryScenarioSetup.ReadOnly = false;
                _gridPostalCategoryScenarioSetup.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
                _gridPostalCategoryScenarioSetup.CurrentCell = null;
                _btnUpdate.Enabled = true;

                _CurScenario = _txtDescription.Text;
                _CurDate = _dtEffectiveDate.Text;
            }

            else if (state == CtlState.EditExisting_Clean)
            {
                _btnNew.Enabled = true;
                _btnDelete.Enabled = true;
                _cboVendor.Enabled = true;
                _cboPostalScenario.Enabled = true;
                _cboEffectiveDate.Enabled = true;

                _txtDescription.ReadOnly = false;
                _txtComments.ReadOnly = false;
                _dtEffectiveDate.Enabled = false;
                _cboPostalMailerType.Enabled = false;
                _cboPostageClass.Enabled = false;

                _lbPostalCategory.Enabled = false;
                _btnAdd.Enabled = false;
                _btnAddAll.Enabled = false;
                _btnRemove.Enabled = false;
                _btnRemoveAll.Enabled = false;
                _gridPostalCategoryScenarioSetup.ReadOnly = false;
                _gridPostalCategoryScenarioSetup.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
                _gridPostalCategoryScenarioSetup.CurrentCell = null;
                _btnUpdate.Enabled = false;

                _CurVendorName = _cboVendor.Text;
                _CurScenario = _cboPostalScenario.Text;
                _CurDate = _cboEffectiveDate.Text;
            }

            else if (state == CtlState.EditExisting_Dirty)
            {
                this.Dirty = true;
                _btnNew.Enabled = false;
                _btnDelete.Enabled = false;
                _cboVendor.Enabled = false;
                _cboPostalScenario.Enabled = false;
                _cboEffectiveDate.Enabled = false;

                _txtDescription.ReadOnly = false;
                _txtComments.ReadOnly = false;
                _dtEffectiveDate.Enabled = false;
                _cboPostalMailerType.Enabled = false;
                _cboPostageClass.Enabled = false;

                _lbPostalCategory.Enabled = false;
                _btnAdd.Enabled = false;
                _btnAddAll.Enabled = false;
                _btnRemove.Enabled = false;
                _btnRemoveAll.Enabled = false;
                _gridPostalCategoryScenarioSetup.ReadOnly = false;
                _gridPostalCategoryScenarioSetup.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
                _btnUpdate.Enabled = true;
            }

            _gridPostalCategoryScenarioSetup.Columns["Category"].ReadOnly = true;
        }

        private void ClearControl()
        {
            _lblInfoText.Text = string.Empty;
            _txtDescription.Text = String.Empty;
            _txtComments.Text = String.Empty;
            _dtEffectiveDate.Value = DateTime.Today;
            _cboPostalMailerType.SelectedValue = 1;
            _cboPostageClass.SelectedValue = 1;

            _lbPostalCategory.Items.Clear();
            _gridPostalCategoryScenarioSetup.Rows.Clear();
            _txtTotalPercentage.Text = String.Empty;
        }

        #endregion

        private void _btnSelectCategories_Click(object sender, EventArgs e)
        {
            if (ValidateControl() && ValidateChildren())
                SetControlState(CtlState.New_WithEffectiveDate);
        }

        private void _lbPostalCategory_Enter(object sender, EventArgs e)
        {
            if (_lbPostalCategory.SelectedItems.Count == 0 && _lbPostalCategory.Items.Count > 0)
                _lbPostalCategory.SelectedItem = _lbPostalCategory.Items[0];
        }
    }
}

