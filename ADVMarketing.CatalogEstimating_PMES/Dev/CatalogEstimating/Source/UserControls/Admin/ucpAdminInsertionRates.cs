using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using CatalogEstimating.CustomControls;
using CatalogEstimating.Datasets;
using CatalogEstimating.Properties;

namespace CatalogEstimating.UserControls.Admin
{
    public partial class ucpAdminInsertionRates : CatalogEstimating.UserControlPanel
    {
        // Current IDs.  Use these to repopulate the control after a save.
        private string _Pub_ID = "";
        private int _PubLoc_ID = -1;
        private DateTime? _PubEffectiveDate = null;
        private DateTime? _PubRateEffectiveDate = null;
        private DateTime? _PubQuantityEffectiveDate = null;
        private string _prevPublicationSelection = "";
        private int _prevLocationSelection = -1;

        // Values used to clone pub rate and quantity
        private string _PubRateCopiedText = String.Empty;
        private string _PubQtyCopiedText = String.Empty;

        // Values to prevent event handling logic from executing
        private bool _revertPublicationSelection = false;
        private bool _revertLocationSelection = false;

        // Current values, detached from the dataset
        private Publications.pub_pubrate_mapRow _curPubRateMap = null;
        private Publications.pub_pubrate_map_activateRow _curPubRateMapActivate = null;
        private Publications.pub_pubrateRow _curPubRate = null;
        private Publications.pub_insertdiscountsDataTable _curPubInsertDiscounts = new Publications.pub_insertdiscountsDataTable();
        private Publications.pub_dayofweekratetypesDataTable _curDayofWeekRateTypes = new Publications.pub_dayofweekratetypesDataTable();
        private Publications.pub_dayofweekratesDataTable _curDayofWeekRates = new Publications.pub_dayofweekratesDataTable();
        private Publications.pub_pubquantityRow _curPubQuantity = null;
        private Publications.pub_dayofweekquantityDataTable _curDayofWeekQuantities = null;

        private DataView _DataView_PubRateMapActive = new DataView();

        #region Construction

        public ucpAdminInsertionRates(Publications ds)
        {
            bool tmpIsLoading = IsLoading;
            IsLoading = true;

            InitializeComponent();
            Name = "Rates";

            _cboRateType.SelectedValueChanged += new System.EventHandler(this.Control_Changed);
            _chkChargeForBlowIn.CheckedChanged += new System.EventHandler(this.Control_Changed);
            _radioBilled.CheckedChanged += new System.EventHandler(this.Control_Changed);
            _radioSent.CheckedChanged += new System.EventHandler(this.Control_Changed);

            // Set all private members
            _dsPublications = ds;

            _dsPublications.PubLoc.DefaultView.RowFilter = "0 = 1";
            _dsPublications.PubLoc.DefaultView.Sort = "PubLoc_ID";
            _dsPublications.pub_pubrate_map_activate.DefaultView.RowFilter = "0 = 1";
            _dsPublications.pub_pubrate_map_activate.DefaultView.Sort = "effectivedate desc";
            _dsPublications.pub_pubrate.DefaultView.RowFilter = "0 = 1";
            _dsPublications.pub_pubrate.DefaultView.Sort = "effectivedate desc";
            _dsPublications.pub_insertdiscounts.DefaultView.RowFilter = "0 = 1";
            _dsPublications.pub_insertdiscounts.DefaultView.Sort = "insert";
            _dsPublications.pub_pubquantity.DefaultView.RowFilter = "0 = 1";
            _dsPublications.pub_pubquantity.DefaultView.Sort = "effectivedate desc";

            _cboRateType.DataSource = _dsPublications.pub_ratetype;
            _cboRateType.DisplayMember = "description";
            _cboRateType.ValueMember = "pub_ratetype_id";

            _cboPublication.DataSource = _dsPublications.Pub;
            _cboPublication.DisplayMember = "Pub_NM";
            _cboPublication.ValueMember = "Pub_ID";

            _cboLocation.DataSource = _dsPublications.PubLoc;
            _cboLocation.DisplayMember = "PubLoc_ID";
            _cboLocation.ValueMember = "PubLoc_ID";

            _cboEffectiveDate.DataSource = _dsPublications.pub_pubrate_map_activate;
            _cboEffectiveDate.DisplayMember = "EffectiveDate";
            _cboEffectiveDate.ValueMember = "EffectiveDate";

            _cboPubRateEffectiveDate.DataSource = _dsPublications.pub_pubrate;
            _cboPubRateEffectiveDate.DisplayMember = "EffectiveDate";
            _cboPubRateEffectiveDate.ValueMember = "PUB_PubRate_ID";

            _cboPubQuantityEffectiveDate.DataSource = _dsPublications.pub_pubquantity;
            _cboPubQuantityEffectiveDate.DisplayMember = "EffectiveDate";
            _cboPubQuantityEffectiveDate.ValueMember = "PUB_PubQuantity_ID";

            _gridInsertsPerDayDiscount.AutoGenerateColumns = false;
            _gridInsertsPerDayDiscount.DataSource = _curPubInsertDiscounts;

            _gridPubQuantitiesRates.Rows.Add(new object[] { "Minimum", null, null, null, null, null, null, null, null, null, null, null, null, null, null });
            _gridPubQuantitiesRates.Rows.Add(new object[] { "Contract Send", null, null, null, null, null, null, null, null, null, null, null, null, null, null });
            _gridPubQuantitiesRates.Rows.Add(new object[] { "Full Run", null, null, null, null, null, null, null, null, null, null, null, null, null, null });

            IsLoading = tmpIsLoading;
        }

        #endregion

        #region Overrides

        public override void Reload()
        {
            bool tmpIsLoading = this.IsLoading;
            IsLoading = true;

            ClearErrorText();

            _curPubRateMap = null;
            _curPubRateMapActivate = null;
            _curPubRate = null;
            _curPubInsertDiscounts.Clear();
            _curDayofWeekRateTypes.Clear();
            _curDayofWeekRates.Clear();
            _curPubQuantity = null;
            _curDayofWeekQuantities = null;

            if (_Pub_ID != "")
            {
                _cboPublication.SelectedValue = _Pub_ID;
                InitLocationCombo();

                if (_PubLoc_ID != -1)
                {
                    _cboLocation.SelectedValue = _PubLoc_ID;
                    InitSearchCombos();

                    if (_PubRateEffectiveDate != null)
                    {
                        foreach (DataRowView r in _dsPublications.pub_pubrate.DefaultView)
                        {
                            if (_PubRateEffectiveDate == ((Publications.pub_pubrateRow)r.Row).effectivedate)
                            {
                                _cboPubRateEffectiveDate.SelectedValue = ((Publications.pub_pubrateRow)r.Row).pub_pubrate_id;
                                break;
                            }
                        }
                    }

                    if (_PubQuantityEffectiveDate != null)
                    {
                        foreach (DataRowView r in _dsPublications.pub_pubquantity.DefaultView)
                        {
                            if (_PubQuantityEffectiveDate == ((Publications.pub_pubquantityRow)r.Row).effectivedate)
                            {
                                _cboPubQuantityEffectiveDate.SelectedValue = ((Publications.pub_pubquantityRow)r.Row).pub_pubquantity_id;
                                break;
                            }
                        }
                    }

                    InitPubInfo();
                    InitPubRate();
                    InitPubQuantity();
                }
            }
            // Clear the bottom tabs, they are unbound
            else
            {
                // Pub Rate
                _dtPubRateEffectiveDate.Value = DateTime.Today;
                _cboRateType.SelectedIndex = 0;
                _cboBlowInRateType.SelectedIndex = 0;
                _chkChargeForBlowIn.Checked = false;
                _radioBilled.Checked = false;
                _radioSent.Checked = true;
                _txtBilledPctLess.Text = String.Empty;
                _txtBilledPctLess.Enabled = false;
                _curPubInsertDiscounts.Clear();
                //                _dsPublications.pub_insertdiscounts.DefaultView.RowFilter = "0 = 1";
                foreach (DataGridViewRow r in _gridPubRates.Rows)
                    for (int i = 0; i < r.Cells.Count; ++i)
                        r.Cells[i].Value = null;

                // Pub Quantity
                _dtPubRateQuantitiesEffDate.Value = DateTime.Today;
                foreach (DataGridViewRow r in _gridPubQuantitiesRates.Rows)
                    for (int i = 1; i < r.Cells.Count; ++i)
                        r.Cells[i].Value = null;

                _txtThanksgiving.Tag = null;
                _txtThanksgiving.Text = String.Empty;
                _txtChristmas.Tag = null;
                _txtChristmas.Text = String.Empty;
                _txtNewYears.Tag = null;
                _txtNewYears.Text = String.Empty;

                _cboLocation.Enabled = false;
                _cboEffectiveDate.Enabled = false;
                _cboPubRateEffectiveDate.Enabled = false;
                _cboPubQuantityEffectiveDate.Enabled = false;
                _btnSearch.Enabled = false;
                _cboRateType.SelectedValue = 1;
                _cboBlowInRateType.SelectedIndex = 0;

                if (_cboPublication.Items.Count > 0)
                {
                    if (_cboPublication.SelectedIndex == -1)
                    {
                        _cboPublication.SelectedIndex = 0;
                    }
                    _cboPublication_SelectedValueChanged(this, EventArgs.Empty);
                }
            }

            base.Reload();

            this.IsLoading = tmpIsLoading;
        }

        public override void LoadData()
        {
            IsLoading = true;
            ClearErrorText();

            _curPubRateMap = null;
            _curPubRateMapActivate = null;
            _curPubRate = null;
            _curPubInsertDiscounts.Clear();
            _curDayofWeekRateTypes.Clear();
            _curDayofWeekRates.Clear();
            _curPubQuantity = null;
            _curDayofWeekQuantities = null;

            LockPubRateMapTab();
            LockPubRateTab();
            LockPubQuantityTab();

            if (_Pub_ID != "")
            {
                _cboPublication.SelectedValue = _Pub_ID;
                InitLocationCombo();

                if (_PubLoc_ID != -1)
                {
                    _cboLocation.SelectedValue = _PubLoc_ID;
                    InitSearchCombos();

                    if (_PubRateEffectiveDate != null)
                    {
                        foreach (DataRowView r in _dsPublications.pub_pubrate.DefaultView)
                        {
                            if (_PubRateEffectiveDate == ((Publications.pub_pubrateRow)r.Row).effectivedate)
                            {
                                _cboPubRateEffectiveDate.SelectedValue = ((Publications.pub_pubrateRow)r.Row).pub_pubrate_id;
                                break;
                            }
                        }
                    }

                    if (_PubQuantityEffectiveDate != null)
                    {
                        foreach (DataRowView r in _dsPublications.pub_pubquantity.DefaultView)
                        {
                            if (_PubQuantityEffectiveDate == ((Publications.pub_pubquantityRow)r.Row).effectivedate)
                            {
                                _cboPubQuantityEffectiveDate.SelectedValue = ((Publications.pub_pubquantityRow)r.Row).pub_pubquantity_id;
                                break;
                            }
                        }
                    }

                    InitPubInfo();
                    InitPubRate();
                    InitPubQuantity();
                }
            }
            // Clear the bottom tabs, they are unbound
            else
            {
                _lblPubLocInfo.Text = String.Empty;
                _lblPubRateInfo.Text = String.Empty;
                _lblPubQtyInfo.Text = String.Empty;
                
                // Pub Rate
                _dtPubRateEffectiveDate.Value = DateTime.Today;
                _cboRateType.SelectedIndex = 0;
                _cboBlowInRateType.SelectedIndex = 0;
                _chkChargeForBlowIn.Checked = false;
                _radioBilled.Checked = false;
                _radioSent.Checked = true;
                _txtBilledPctLess.Text = String.Empty;
                _txtBilledPctLess.Enabled = false;
                _curPubInsertDiscounts.Clear();
//                _dsPublications.pub_insertdiscounts.DefaultView.RowFilter = "0 = 1";
                foreach (DataGridViewRow r in _gridPubRates.Rows)
                    for (int i = 0; i < r.Cells.Count; ++i)
                        r.Cells[i].Value = null;

                _gridInsertsPerDayDiscount.CurrentCell = null;
                _gridPubRates.CurrentCell = null;

                // Pub Quantity
                _dtPubRateQuantitiesEffDate.Value = DateTime.Today;
                foreach (DataGridViewRow r in _gridPubQuantitiesRates.Rows)
                    for (int i = 1; i < r.Cells.Count; ++i)
                        r.Cells[i].Value = null;

                _txtThanksgiving.Tag = null;
                _txtThanksgiving.Text = String.Empty;
                _txtChristmas.Tag = null;
                _txtChristmas.Text = String.Empty;
                _txtNewYears.Tag = null;
                _txtNewYears.Text = String.Empty;

                _cboLocation.Enabled = false;
                _cboEffectiveDate.Enabled = false;
                _cboPubRateEffectiveDate.Enabled = false;
                _cboPubQuantityEffectiveDate.Enabled = false;
                _btnSearch.Enabled = false;
                _cboRateType.SelectedIndex = 0;
                _cboBlowInRateType.SelectedIndex = 0;

                _gridPubQuantitiesRates.CurrentCell = null;
            }

            ClearErrorText();
            base.LoadData();
        }

        private void ClearErrorText()
        {
            _errorProvider.Clear();

            foreach (DataGridViewRow gvr in _gridInsertsPerDayDiscount.Rows)
                gvr.ErrorText = String.Empty;
            foreach (DataGridViewRow gvr in _gridPubRates.Rows)
                gvr.ErrorText = String.Empty;
            foreach (DataGridViewRow gvr in _gridPubQuantitiesRates.Rows)
                for (int i = 0; i < gvr.Cells.Count; ++i)
                    gvr.Cells[i].ErrorText = String.Empty;
        }

        public override void Export(ref ExcelWriter writer)
        {
            writer.WriteLine(new object[] { "Admin Insertion Rates" });
            writer.WriteLine(new object[] { _lblPublication.Text, _cboPublication.Text });
            writer.WriteLine(new object[] { _lblLocation.Text, _cboLocation.Text, _lblEffectiveDateRates.Text, _dtPubRateEffectiveDate.Value.ToShortDateString() });
            writer.WriteLine(new object[] { _lblEffectiveDate.Text, _dtEffectiveDate.Value.ToShortDateString() });
            writer.WriteLine(new object[] { });
            writer.WriteLine(new object[] { "Rate Activation" });
            writer.WriteLine(new object[] { _chkActive.Text, _chkActive.Checked, _lblPubEffectiveDate.Text, _dtEffectiveDate.Value.ToShortDateString() });
            writer.WriteLine(new object[] { });

            writer.WriteLine(new object[] { "Rates" });
            writer.WriteLine(new object[] { _lblPubRateEffectiveDate.Text, _dtPubRateEffectiveDate.Value.ToShortDateString() });
            writer.WriteLine(new object[] { _lblRateType.Text, _cboRateType.Text });
            writer.WriteLine(new object[] { _chkChargeForBlowIn.Text, _chkChargeForBlowIn.Checked });
            writer.WriteLine(new object[] { _lblBlowInRateType.Text, _cboBlowInRateType.Text });
            writer.WriteLine(new object[] { _groupChargeForQuantity.Text });
            writer.WriteLine(new object[] { _radioBilled.Text, _radioBilled.Checked, _radioSent.Text, _radioSent.Checked });
            writer.WriteLine(new object[] { _lblInsertsPerDayDiscount.Text });
            writer.WriteTable(_gridInsertsPerDayDiscount, true);
            writer.WriteLine(new object[] { _groupPublicationRates.Text });
            writer.WriteTable(_gridPubRates, true);

            writer.WriteLine(new object[] { "Quantities" });
            writer.WriteLine(new object[] { _lblPubRateQuantitiesEffDate.Text, _dtPubRateQuantitiesEffDate.Value.ToShortDateString() });
            writer.WriteTable(_gridPubQuantitiesRates, true);
            writer.WriteLine(new object[] { _lblThanksgiving.Text, _txtThanksgiving.Text });
            writer.WriteLine(new object[] { _lblChristmas.Text, _txtChristmas.Text });
            writer.WriteLine(new object[] { _lblNewYears.Text, _txtNewYears.Text });
        }

        public override void SaveData()
        {
            bool tmpIsLoading = IsLoading;
            IsLoading = true;

            if (Dirty)
                WriteToDataSet();

            IsLoading = tmpIsLoading;
            this.Reload();

            base.SaveData();
        }

        public override void PreSave(CancelEventArgs e)
        {
            // Force value to be sync'd with what may have been typed into date control.
            MainForm.UpdateDateControl(_dtEffectiveDate);
            MainForm.UpdateDateControl(_dtPubRateEffectiveDate);
            MainForm.UpdateDateControl(_dtPubRateQuantitiesEffDate);

            if (!ValidateControl())
            {
                MessageBox.Show("There is invalid data.", "Cannot Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        public override void OnLeaving(CancelEventArgs e)
        {
            if (!ValidateControl())
                e.Cancel = true;
            else
            {
                WriteToDataSet();
                _curPubRateMap = null;
                _curPubRateMapActivate = null;
                _curPubRate = null;
                _curPubQuantity = null;
            }
        }

        public override ToolStrip Toolbar
        {
            get
            {
                return _toolStrip;
            }
        }

        #endregion

        #region Private Methods

        private void WriteToDataSet()
        {
            // Pub Info
            WritePubRateMapToDataSet();

            //Pub Rate
            if (_curPubRate != null)
                WritePubRateToDataSet();


            // Pub Quantity
            if (_curPubQuantity != null)
                WritePubQuantityToDataSet();
        }

        private void InitPubInfo()
        {
            bool tmpIsLoading = IsLoading;
            IsLoading = true;

            // Initialize the Pub Info
            if (_cboEffectiveDate.SelectedItem == null)
            {
                _curPubRateMap = null;
                _curPubRateMapActivate = null;
                _chkActive.Checked = true;
                _dtEffectiveDate.Value = DateTime.Today;
                LockPubRateMapTab();
            }
            else
            {
                _curPubRateMapActivate = (Publications.pub_pubrate_map_activateRow)((DataRowView)_cboEffectiveDate.SelectedItem).Row;
                _lblPubLocInfo.Text = "Publication: " + _cboPublication.Text.Trim() + " Location: " + _cboLocation.Text.Trim() + " Effective Date: " + _curPubRateMapActivate.effectivedate.ToShortDateString();

                _chkActive.Checked = _curPubRateMapActivate.active;
                _dtEffectiveDate.Value = _curPubRateMapActivate.effectivedate;
                _dtEffectiveDate.Enabled = false;
                if (MainForm.AuthorizedUser.Right == UserRights.SuperAdmin || MainForm.AuthorizedUser.Right == UserRights.Admin)
                    _chkActive.Enabled = true;
            }

            IsLoading = tmpIsLoading;
        }

        private void InitPubRate()
        {
            // If there are no pub rates we can't display anything in the pub rate tab
            if (_cboPubRateEffectiveDate.SelectedItem == null)
            {
                _curPubRate = null;
                ClearPubRate();

                // Lock the control
                LockPubRateTab();
            }
            else
            {
                bool tmpIsLoading = IsLoading;
                IsLoading = true;

                _curPubRate = (Publications.pub_pubrateRow)((DataRowView)_cboPubRateEffectiveDate.SelectedItem).Row;
                _lblPubRateInfo.Text = "Publication: " + _cboPublication.Text.Trim() + " Location: " + _cboLocation.Text.Trim() + " Effective Date: " + _curPubRate.effectivedate.ToShortDateString();
                _PubRateCopiedText = "Rate Info copied from " + "Publication: " + _cboPublication.Text.Trim() + " Location: " + _cboLocation.Text.Trim() + " Effective Date: " + _curPubRate.effectivedate.ToShortDateString();

                // Set the fields on the control
                _dtPubRateEffectiveDate.Value = _curPubRate.effectivedate;
                _cboRateType.SelectedValue = _curPubRate.pub_ratetype_id;
                _chkChargeForBlowIn.Checked = _curPubRate.chargeblowin;
                _cboBlowInRateType.SelectedIndex = _curPubRate.blowinrate;
                if (_curPubRate.quantitychargetype == 1)
                    _radioBilled.Checked = true;
                else
                    _radioSent.Checked = true;
                _txtBilledPctLess.Text = _curPubRate.billedpct.ToString();

                _dsPublications.pub_insertdiscounts.DefaultView.RowFilter = "pub_pubrate_id = " + _curPubRate.pub_pubrate_id.ToString();
                _curPubInsertDiscounts = (Publications.pub_insertdiscountsDataTable)_dsPublications.pub_insertdiscounts.Clone();
                _gridInsertsPerDayDiscount.CancelEdit();
                _gridInsertsPerDayDiscount.DataSource = _curPubInsertDiscounts;

                int row_index = 0;
                foreach (DataRowView drv in _dsPublications.pub_insertdiscounts.DefaultView)
                {
                    Publications.pub_insertdiscountsRow idr = (Publications.pub_insertdiscountsRow)drv.Row;
                    _curPubInsertDiscounts.Rows.Add(idr.ItemArray);

                    _curPubInsertDiscounts.Rows[row_index]["discount"] = Convert.ToDecimal(_curPubInsertDiscounts.Rows[row_index]["discount"]) * 100;
                    ++row_index;
                }

                _curPubInsertDiscounts.pub_pubrate_idColumn.DefaultValue = _curPubRate.pub_pubrate_id.ToString();
                _curPubInsertDiscounts.createdbyColumn.DefaultValue = MainForm.AuthorizedUser.FormattedName;
                _curPubInsertDiscounts.createddateColumn.DefaultValue = DateTime.Now;
                _curPubInsertDiscounts.modifiedbyColumn.DefaultValue = MainForm.AuthorizedUser.FormattedName;
                _curPubInsertDiscounts.modifieddateColumn.DefaultValue = DateTime.Now;

                _dsPublications.pub_dayofweekratetypes.DefaultView.RowFilter = "pub_pubrate_id = " + _curPubRate.pub_pubrate_id.ToString();
                InitPubRateGrid();
                if (MainForm.AuthorizedUser.Right == UserRights.SuperAdmin || MainForm.AuthorizedUser.Right == UserRights.Admin)
                    UnlockPubRateTab();
                _dtPubRateEffectiveDate.Enabled = false;
                _gridInsertsPerDayDiscount.CurrentCell = null;
                _gridPubRates.CurrentCell = null;

                if ( (int)_cboRateType.SelectedValue == 1 )
                {
                    _chkChargeForBlowIn.Enabled = true;
                }
                else
                {
                    _chkChargeForBlowIn.Checked = false;
                    _chkChargeForBlowIn.Enabled = false;
                }

                IsLoading = tmpIsLoading;
            }
        }

        private void InitPubRateGrid()
        {
            // Clear the local data tables
            _curDayofWeekRateTypes = (Publications.pub_dayofweekratetypesDataTable)_dsPublications.pub_dayofweekratetypes.Clone();
            _curDayofWeekRateTypes.DefaultView.Sort = "ratetypedescription";
            _curDayofWeekRateTypes.pub_pubrate_idColumn.DefaultValue = _curPubRate.pub_pubrate_id;
            _curDayofWeekRateTypes.ratetypedescriptionColumn.DefaultValue = 0;
            _curDayofWeekRateTypes.createdbyColumn.DefaultValue = MainForm.AuthorizedUser.FormattedName;
            _curDayofWeekRateTypes.createddateColumn.DefaultValue = DateTime.Now;

            _curDayofWeekRates = (Publications.pub_dayofweekratesDataTable)_dsPublications.pub_dayofweekrates.Clone();
            _curDayofWeekRates.rateColumn.DefaultValue = 0;
            _curDayofWeekRates.createdbyColumn.DefaultValue = MainForm.AuthorizedUser.FormattedName;
            _curDayofWeekRates.createddateColumn.DefaultValue = DateTime.Now;

            // Clear the grid
            _gridPubRates.Rows.Clear();
            _gridPubRates.Columns[1].HeaderText = _dsPublications.pub_ratetype.FindBypub_ratetype_id(_curPubRate.pub_ratetype_id).description;

            // Populate the dow rate types datatable
            _dsPublications.pub_dayofweekratetypes.DefaultView.RowFilter = "pub_pubrate_id = " + _curPubRate.pub_pubrate_id.ToString();
            _dsPublications.pub_dayofweekratetypes.DefaultView.Sort = "ratetypedescription";
            foreach (DataRowView r in _dsPublications.pub_dayofweekratetypes.DefaultView)
                _curDayofWeekRateTypes.ImportRow((Publications.pub_dayofweekratetypesRow)r.Row);


            // Populate the dow rates datatable
            foreach (Publications.pub_dayofweekratetypesRow ratetype_row in _curDayofWeekRateTypes)
            {
                // pub_dayofweekratetypes_id, ratetypedescription, pub_dayofweekrates_id (seven), rate (seven)
                object[] grid_rate_row = new object[16];
                grid_rate_row[0] = ratetype_row.pub_dayofweekratetypes_id;
                if (_curPubRate.pub_ratetype_id == 1)
                    grid_rate_row[1] = Convert.ToInt32(ratetype_row.ratetypedescription);
                else if(_curPubRate.pub_ratetype_id == 2)
                    grid_rate_row[1] = null;
                else
                    grid_rate_row[1] = ratetype_row.ratetypedescription;

                _dsPublications.pub_dayofweekrates.DefaultView.RowFilter = "pub_dayofweekratetypes_id = " + ratetype_row.pub_dayofweekratetypes_id.ToString();
                _dsPublications.pub_dayofweekrates.DefaultView.Sort = "insertdow";

                foreach (DataRowView rate_row in _dsPublications.pub_dayofweekrates.DefaultView)
                {
                    Publications.pub_dayofweekratesRow dow_rate_row = (Publications.pub_dayofweekratesRow)rate_row.Row;
                    _curDayofWeekRates.ImportRow(dow_rate_row);
                    grid_rate_row[dow_rate_row.insertdow + 1] = dow_rate_row.pub_dayofweekrates_id;
                    grid_rate_row[dow_rate_row.insertdow + 8] = dow_rate_row.rate;
                }

                _gridPubRates.Rows.Add(grid_rate_row);
            }
        }

        private void InitPubQuantity()
        {
            bool tmpIsLoading = IsLoading;
            IsLoading = true;

            // Clear the Grid
            foreach (DataGridViewRow r in _gridPubQuantitiesRates.Rows)
                for (int i = 1; i < r.Cells.Count; ++i)
                    r.Cells[i].Value = null;

            // Clear Holiday fields
            _txtThanksgiving.Tag = null;
            _txtThanksgiving.Text = String.Empty;
            _txtChristmas.Tag = null;
            _txtChristmas.Text = String.Empty;
            _txtNewYears.Tag = null;
            _txtNewYears.Text = String.Empty;

            // If there is no pub quantity selected.  We can't displaying anything on the pub quantity tab.
            if (_cboPubQuantityEffectiveDate.SelectedItem == null)
            {
                _curPubQuantity = null;
                _dtPubRateQuantitiesEffDate.Value = DateTime.Today;
                _dsPublications.pub_dayofweekquantity.DefaultView.RowFilter = "0 = 1";
                _curDayofWeekQuantities = null;
                LockPubQuantityTab();
            }
            else
            {
                _curPubQuantity = (Publications.pub_pubquantityRow)((DataRowView)_cboPubQuantityEffectiveDate.SelectedItem).Row;
                _lblPubQtyInfo.Text = "Publication: " + _cboPublication.Text.Trim() + " Location: " + _cboLocation.Text.Trim() + " Effective Date: " + _curPubQuantity.effectivedate.ToShortDateString();
                _PubQtyCopiedText = "Quantities copied from " + "Publication: " + _cboPublication.Text.Trim() + " Location: " + _cboLocation.Text.Trim() + " Effective Date: " + _curPubQuantity.effectivedate.ToShortDateString();

                _dtPubRateQuantitiesEffDate.Enabled = false;

                // Set the fields on the control
                _dtPubRateQuantitiesEffDate.Value = _curPubQuantity.effectivedate;


                // Populate local datatables
                _dsPublications.pub_dayofweekquantity.DefaultView.RowFilter = "pub_pubquantity_id = " + _curPubQuantity.pub_pubquantity_id.ToString();
                _dsPublications.pub_dayofweekquantity.DefaultView.Sort = "pub_pubquantitytype_id";

                _curDayofWeekQuantities = (Publications.pub_dayofweekquantityDataTable)_dsPublications.pub_dayofweekquantity.Clone();
                _curDayofWeekQuantities.pub_pubquantity_idColumn.DefaultValue = _curPubQuantity.pub_pubquantity_id;
                _curDayofWeekQuantities.quantityColumn.DefaultValue = 0;
                _curDayofWeekQuantities.createdbyColumn.DefaultValue = MainForm.AuthorizedUser.FormattedName;
                _curDayofWeekQuantities.createddateColumn.DefaultValue = DateTime.Now;

                foreach (DataRowView qty_row in _dsPublications.pub_dayofweekquantity.DefaultView)
                {
                    Publications.pub_dayofweekquantityRow dow_qty_row = (Publications.pub_dayofweekquantityRow)qty_row.Row;
                    _curDayofWeekQuantities.ImportRow(dow_qty_row);

                    if (dow_qty_row.pub_pubquantitytype_id < 4)
                    {
                        _gridPubQuantitiesRates.Rows[dow_qty_row.pub_pubquantitytype_id - 1].Cells[dow_qty_row.insertdow].Value = dow_qty_row.pub_dayofweekquantity_id;
                        _gridPubQuantitiesRates.Rows[dow_qty_row.pub_pubquantitytype_id - 1].Cells[dow_qty_row.insertdow + 7].Value = dow_qty_row.quantity;
                    }
                    else if (dow_qty_row.pub_pubquantitytype_id == 4) // Thanksgiving
                    {
                        _txtThanksgiving.Tag   = dow_qty_row.pub_dayofweekquantity_id;
                        _txtThanksgiving.Value = dow_qty_row.quantity;
                    }
                    else if (dow_qty_row.pub_pubquantitytype_id == 5) // Christmas
                    {
                        _txtChristmas.Tag   = dow_qty_row.pub_dayofweekquantity_id;
                        _txtChristmas.Value = dow_qty_row.quantity;
                    }
                    else if (dow_qty_row.pub_pubquantitytype_id == 6) // New Years
                    {
                        _txtNewYears.Tag   = dow_qty_row.pub_dayofweekquantity_id;
                        _txtNewYears.Value = dow_qty_row.quantity;
                    }
                }

                if (MainForm.AuthorizedUser.Right == UserRights.SuperAdmin || MainForm.AuthorizedUser.Right == UserRights.Admin)
                    UnlockPubQuantityTab();
                _dtPubRateQuantitiesEffDate.Enabled = false;

                _gridPubQuantitiesRates.CurrentCell = null;
            }

            IsLoading = tmpIsLoading;
        }

        private void InitLocationCombo()
        {
            if (_cboPublication.SelectedValue == null)
            {
                _cboLocation.Enabled = false;
                _dsPublications.PubLoc.DefaultView.RowFilter = "Pub_ID is null";
            }
            else
            {
                _cboLocation.Enabled = true;
                _dsPublications.PubLoc.DefaultView.RowFilter = "Pub_ID = '" + _cboPublication.SelectedValue.ToString() + "'";
            }
        }

        private void InitSearchCombos()
        {
            _curPubRateMap = null;
            _curPubRateMapActivate = null;

            foreach (Publications.pub_pubrate_mapRow r in _dsPublications.pub_pubrate_map)
                if (r.pub_id == _cboPublication.SelectedValue.ToString() && r.publoc_id == Convert.ToInt32(_cboLocation.SelectedValue))
                {
                    _curPubRateMap = r;
                    break;
                }

            if (_curPubRateMap == null)
            {
                _cboEffectiveDate.Enabled = false;
                _cboPubRateEffectiveDate.Enabled = false;
                _cboPubQuantityEffectiveDate.Enabled = false;
                _btnSearch.Enabled = false;
                _dsPublications.pub_pubrate_map_activate.DefaultView.RowFilter = "0 = 1";
                _dsPublications.pub_pubrate.DefaultView.RowFilter = "0 = 1";
                _dsPublications.pub_pubquantity.DefaultView.RowFilter = "0 = 1";
            }
            else
            {
                if (_tabControl.SelectedIndex == 0)
                {
                    _cboEffectiveDate.Enabled = true;
                    _cboPubRateEffectiveDate.Enabled = false;
                    _cboPubQuantityEffectiveDate.Enabled = false;
                }
                else if (_tabControl.SelectedIndex == 1)
                {
                    _cboEffectiveDate.Enabled = false;
                    _cboPubRateEffectiveDate.Enabled = true;
                    _cboPubQuantityEffectiveDate.Enabled = false;
                }
                else if (_tabControl.SelectedIndex == 2)
                {
                    _cboEffectiveDate.Enabled = false;
                    _cboPubRateEffectiveDate.Enabled = false;
                    _cboPubQuantityEffectiveDate.Enabled = true;
                }
                _btnSearch.Enabled = true;
                _dsPublications.pub_pubrate_map_activate.DefaultView.RowFilter = "pub_pubrate_map_id = " + _curPubRateMap.pub_pubrate_map_id.ToString();
                _dsPublications.pub_pubrate.DefaultView.RowFilter = "pub_pubrate_map_id = " + _curPubRateMap.pub_pubrate_map_id.ToString();
                _dsPublications.pub_pubquantity.DefaultView.RowFilter = "pub_pubrate_map_id = " + _curPubRateMap.pub_pubrate_map_id.ToString();
            }
        }

        private void WritePubRateMapToDataSet()
        {
            if (_curPubRateMap == null || _curPubRateMapActivate == null)
                return;

            _curPubRateMapActivate.active = _chkActive.Checked;
            _curPubRateMapActivate.effectivedate = _dtEffectiveDate.Value.Date;

            if (_curPubRateMap.RowState == DataRowState.Detached)
                _dsPublications.pub_pubrate_map.Addpub_pubrate_mapRow(_curPubRateMap);

            if (_curPubRateMapActivate.RowState == DataRowState.Detached)
                _dsPublications.pub_pubrate_map_activate.Addpub_pubrate_map_activateRow(_curPubRateMapActivate);
        }

        private void WritePubQuantityToDataSet()
        {
            _curPubQuantity.effectivedate = _dtPubRateQuantitiesEffDate.Value.Date;
            _curPubQuantity.modifiedby = MainForm.AuthorizedUser.FormattedName;
            _curPubQuantity.modifieddate = DateTime.Now;

            foreach (DataGridViewRow r in _gridPubQuantitiesRates.Rows)
            {
                for (int i = 1; i < 8; ++i)
                {
                    Publications.pub_dayofweekquantityRow qty_row;
                    if (r.Cells[i].Value == null)
                    {
                        qty_row = _curDayofWeekQuantities.Newpub_dayofweekquantityRow();
                        qty_row.pub_pubquantitytype_id = r.Index + 1;
                        qty_row.insertdow = i;
                        _curDayofWeekQuantities.Addpub_dayofweekquantityRow(qty_row);
                    }
                    else
                        qty_row = _curDayofWeekQuantities.FindBypub_dayofweekquantity_id((long)r.Cells[i].Value);
                    qty_row.quantity = Convert.ToInt32(r.Cells[i + 7].Value.ToString().Replace(",", ""));
                    qty_row.modifiedby = MainForm.AuthorizedUser.FormattedName;
                    qty_row.modifieddate = DateTime.Now;
                }
            }

            Publications.pub_dayofweekquantityRow holiday_qty_row;

            if (_txtThanksgiving.Tag == null)
            {
                holiday_qty_row = _curDayofWeekQuantities.Newpub_dayofweekquantityRow();
                holiday_qty_row.pub_pubquantitytype_id = 4;
                holiday_qty_row.SetinsertdowNull();
                _curDayofWeekQuantities.Addpub_dayofweekquantityRow(holiday_qty_row);
            }
            else
                holiday_qty_row = _curDayofWeekQuantities.FindBypub_dayofweekquantity_id((long)_txtThanksgiving.Tag);
            holiday_qty_row.quantity = _txtThanksgiving.Value.Value;
            holiday_qty_row.modifiedby = MainForm.AuthorizedUser.FormattedName;
            holiday_qty_row.modifieddate = DateTime.Now;

            if (_txtChristmas.Tag == null)
            {
                holiday_qty_row = _curDayofWeekQuantities.Newpub_dayofweekquantityRow();
                holiday_qty_row.pub_pubquantitytype_id = 5;
                holiday_qty_row.SetinsertdowNull();
                _curDayofWeekQuantities.Addpub_dayofweekquantityRow(holiday_qty_row);
            }
            else
                holiday_qty_row = _curDayofWeekQuantities.FindBypub_dayofweekquantity_id((long)_txtChristmas.Tag);
            holiday_qty_row.quantity = _txtChristmas.Value.Value;
            holiday_qty_row.modifiedby = MainForm.AuthorizedUser.FormattedName;
            holiday_qty_row.modifieddate = DateTime.Now;

            if (_txtNewYears.Tag == null)
            {
                holiday_qty_row = _curDayofWeekQuantities.Newpub_dayofweekquantityRow();
                holiday_qty_row.pub_pubquantitytype_id = 6;
                holiday_qty_row.SetinsertdowNull();
                _curDayofWeekQuantities.Addpub_dayofweekquantityRow(holiday_qty_row);
            }
            else
                holiday_qty_row = _curDayofWeekQuantities.FindBypub_dayofweekquantity_id((long)_txtNewYears.Tag);
            holiday_qty_row.quantity = _txtNewYears.Value.Value;
            holiday_qty_row.modifiedby = MainForm.AuthorizedUser.FormattedName;
            holiday_qty_row.modifieddate = DateTime.Now;

            if (_curPubQuantity.RowState == DataRowState.Detached)
                _dsPublications.pub_pubquantity.Addpub_pubquantityRow(_curPubQuantity);
            _dsPublications.pub_dayofweekquantity.Merge(_curDayofWeekQuantities);
        }

        private void WritePubRateToDataSet()
        {
            _curPubRate.pub_ratetype_id = Convert.ToInt32(_cboRateType.SelectedValue);
            _curPubRate.chargeblowin = _chkChargeForBlowIn.Checked;
            if (_cboBlowInRateType.SelectedIndex == -1)
                _curPubRate.blowinrate = 0;
            else
                _curPubRate.blowinrate = _cboBlowInRateType.SelectedIndex;
            _curPubRate.effectivedate = _dtPubRateEffectiveDate.Value.Date;
            if (_radioBilled.Checked)
                _curPubRate.quantitychargetype = 1;
            else
                _curPubRate.quantitychargetype = 2;
            if (_txtBilledPctLess.Text.Trim() == String.Empty)
                _curPubRate.billedpct = 0;
            else
                _curPubRate.billedpct = Convert.ToDecimal(_txtBilledPctLess.Text.Trim());
            _curPubRate.modifiedby = MainForm.AuthorizedUser.FormattedName;
            _curPubRate.modifieddate = DateTime.Now;

            if (_curPubRate.RowState == DataRowState.Detached)
                _dsPublications.pub_pubrate.Addpub_pubrateRow(_curPubRate);

            foreach (DataRowView drv in _dsPublications.pub_insertdiscounts.DefaultView)
            {
                Publications.pub_insertdiscountsRow idr = (Publications.pub_insertdiscountsRow)drv.Row;
                if (idr.RowState != DataRowState.Deleted && _curPubInsertDiscounts.FindBypub_insertdiscount_id(idr.pub_insertdiscount_id) == null)
                    idr.Delete();
            }

            foreach (DataRow dr in _curPubInsertDiscounts)
            {
                ((Publications.pub_insertdiscountsRow)dr).discount = ((Publications.pub_insertdiscountsRow)dr).discount / 100;
            }

            _dsPublications.pub_insertdiscounts.Merge(_curPubInsertDiscounts);

            // Rows deleted from the pub rate grid may need to be deleted from the datatable
            List<bool> dow_ratetypes = new List<bool>();
            foreach (Publications.pub_dayofweekratetypesRow r in _curDayofWeekRateTypes)
            {
                bool _rowExists = false;
                foreach (DataGridViewRow dg_row in _gridPubRates.Rows)
                    if (dg_row.Cells[0].Value != null && r.pub_dayofweekratetypes_id == (long)dg_row.Cells[0].Value)
                    {
                        _rowExists = true;
                        break;
                    }

                if (!_rowExists)
                {
                    // Delete corresponding pub day of week rate records from the datatable
                    List<bool> dow_rates = new List<bool>();
                    foreach (Datasets.Publications.pub_dayofweekratesRow dow_row in _curDayofWeekRates)
                    {
                        if (dow_row.pub_dayofweekratetypes_id == r.pub_dayofweekratetypes_id)
                            dow_rates.Add(true);
                        else
                            dow_rates.Add(false);
                    }

                    for (int i = (dow_rates.Count - 1); i >= 0; --i)
                    {
                        if (dow_rates[i])
                        {
                            // If the status is added, we may have to explicitly delete the record from the dataset
                            if (_curDayofWeekRates[i].RowState == DataRowState.Added
                                && _dsPublications.pub_dayofweekrates.FindBypub_dayofweekrates_id(_curDayofWeekRates[i].pub_dayofweekrates_id) != null)
                            {
                                _dsPublications.pub_dayofweekrates.FindBypub_dayofweekrates_id(_curDayofWeekRates[i].pub_dayofweekrates_id).Delete();
                            }
                            _curDayofWeekRates[i].Delete();
                        }
                    }

                    dow_ratetypes.Add(true);
                }
                else
                {
                    dow_ratetypes.Add(false);
                }
            }

            for (int i = (dow_ratetypes.Count - 1); i >= 0; --i)
            {
                if (dow_ratetypes[i])
                {
                    if (_curDayofWeekRateTypes[i].RowState == DataRowState.Added
                        && _dsPublications.pub_dayofweekratetypes.FindBypub_dayofweekratetypes_id(_curDayofWeekRateTypes[i].pub_dayofweekratetypes_id) != null)
                    {
                        _dsPublications.pub_dayofweekratetypes.FindBypub_dayofweekratetypes_id(_curDayofWeekRateTypes[i].pub_dayofweekratetypes_id).Delete();
                    }
                    _curDayofWeekRateTypes[i].Delete();
                }
            }

            // Set the values in the pub day of week tables according to the grid
            _gridPubRates.CancelEdit();

            foreach (DataGridViewRow r in _gridPubRates.Rows)
            {
                // Rows added to the grid need to be added to the datatable
                if (!r.IsNewRow)
                {
                    if (r.Cells[0].Value == null)
                    {
                        Publications.pub_dayofweekratetypesRow dow_rate_row = _curDayofWeekRateTypes.Newpub_dayofweekratetypesRow();
                        r.Cells[0].Value = dow_rate_row.pub_dayofweekratetypes_id;
                        // Flat Rates do not have a "rate type description
                        if (_curPubRate.pub_ratetype_id == 2)
                            dow_rate_row.ratetypedescription = 0;
                        else
                            dow_rate_row.ratetypedescription = Convert.ToDecimal(r.Cells[1].Value);
                        _curDayofWeekRateTypes.Addpub_dayofweekratetypesRow(dow_rate_row);

                        for (int i = 0; i < 7; ++i)
                        {
                            Publications.pub_dayofweekratesRow rate_row = _curDayofWeekRates.Newpub_dayofweekratesRow();
                            r.Cells[i + 2].Value = rate_row.pub_dayofweekrates_id;
                            rate_row.pub_dayofweekratetypes_id = dow_rate_row.pub_dayofweekratetypes_id;
                            rate_row.insertdow = i + 1;
                            rate_row.rate = Convert.ToDecimal(r.Cells[i + 9].Value);
                            _curDayofWeekRates.Addpub_dayofweekratesRow(rate_row);
                        }
                    }
                    else
                    {
                        Publications.pub_dayofweekratetypesRow dow_rate_row = _curDayofWeekRateTypes.FindBypub_dayofweekratetypes_id((long)r.Cells[0].Value);
                        dow_rate_row.ratetypedescription = Convert.ToDecimal(r.Cells[1].Value);
                        dow_rate_row.modifiedby = MainForm.AuthorizedUser.FormattedName;
                        dow_rate_row.modifieddate = DateTime.Now;

                        for (int i = 0; i < 7; ++i)
                        {
                            Publications.pub_dayofweekratesRow rate_row = _curDayofWeekRates.FindBypub_dayofweekrates_id((long)r.Cells[i + 2].Value);
                            rate_row.rate = Convert.ToDecimal(r.Cells[i + 9].Value);
                            rate_row.modifiedby = MainForm.AuthorizedUser.FormattedName;
                            rate_row.modifieddate = DateTime.Now;
                        }
                    }
                }
            }

            _dsPublications.pub_dayofweekratetypes.Merge(_curDayofWeekRateTypes, false);
            _dsPublications.pub_dayofweekrates.Merge(_curDayofWeekRates, false);
        }

        private void LockPubRateTab()
        {
            _dtPubRateEffectiveDate.Enabled = false;
            _cboRateType.Enabled = false;
            _chkChargeForBlowIn.Enabled = false;
            _cboBlowInRateType.Enabled = false;
            _groupChargeForQuantity.Enabled = false;
            
            _gridInsertsPerDayDiscount.ReadOnly = true;
            _gridInsertsPerDayDiscount.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
            _gridInsertsPerDayDiscount.AllowUserToAddRows = false;

            _gridPubRates.ReadOnly = true;
            _gridPubRates.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
            _gridPubRates.AllowUserToAddRows = false;
        }

        #endregion

        #region New Records

        private void NewPubQuantity()
        {
            _lblPubQtyInfo.Text = _PubQtyCopiedText;
            _dtPubRateQuantitiesEffDate.Value = DateTime.Today;

            _curPubQuantity = _dsPublications.pub_pubquantity.Newpub_pubquantityRow();
            _curPubQuantity.pub_pubrate_map_id = _curPubRateMap.pub_pubrate_map_id;
            _curPubQuantity.effectivedate = _dtPubRateQuantitiesEffDate.Value.Date;
            _curPubQuantity.createdby = MainForm.AuthorizedUser.FormattedName;
            _curPubQuantity.createddate = DateTime.Now;

            _curDayofWeekQuantities = (Publications.pub_dayofweekquantityDataTable)_dsPublications.pub_dayofweekquantity.Clone();
            _curDayofWeekQuantities.pub_pubquantity_idColumn.DefaultValue = _curPubQuantity.pub_pubquantity_id;
            _curDayofWeekQuantities.createdbyColumn.DefaultValue = MainForm.AuthorizedUser.FormattedName;
            _curDayofWeekQuantities.createddateColumn.DefaultValue = DateTime.Now;
            foreach (DataGridViewRow r in _gridPubQuantitiesRates.Rows)
            {
                for (int i = 1; i < 8; ++i)
                {
                    Publications.pub_dayofweekquantityRow dow_qty_row = _curDayofWeekQuantities.Newpub_dayofweekquantityRow();
                    r.Cells[i].Value = dow_qty_row.pub_dayofweekquantity_id;
                    dow_qty_row.pub_pubquantity_id = _curPubQuantity.pub_pubquantity_id;
                    dow_qty_row.pub_pubquantitytype_id = r.Index + 1;
                    dow_qty_row.insertdow = i;
                    if (r.Cells[i + 7].Value == null)
                        dow_qty_row.quantity = 0;
                    else
                        dow_qty_row.quantity = Convert.ToInt32(r.Cells[i + 7].Value);
                    dow_qty_row.createdby = MainForm.AuthorizedUser.FormattedName;
                    dow_qty_row.createddate = DateTime.Now;
                    _curDayofWeekQuantities.Rows.Add(dow_qty_row);
                }
            }


            Publications.pub_dayofweekquantityRow holiday_qty_row = _curDayofWeekQuantities.Newpub_dayofweekquantityRow();
            _txtThanksgiving.Tag = holiday_qty_row.pub_dayofweekquantity_id;
            holiday_qty_row.pub_pubquantity_id = _curPubQuantity.pub_pubquantity_id;
            holiday_qty_row.pub_pubquantitytype_id = 4;
            holiday_qty_row.SetinsertdowNull();
            if (_txtThanksgiving.Text.Trim() == String.Empty)
                holiday_qty_row.quantity = 0;
            else
                holiday_qty_row.quantity = Convert.ToInt32(_txtThanksgiving.Value);
            holiday_qty_row.createdby = MainForm.AuthorizedUser.FormattedName;
            holiday_qty_row.createddate = DateTime.Now;
            _curDayofWeekQuantities.Rows.Add(holiday_qty_row);

            holiday_qty_row = _curDayofWeekQuantities.Newpub_dayofweekquantityRow();
            _txtChristmas.Tag = holiday_qty_row.pub_dayofweekquantity_id;
            holiday_qty_row.pub_pubquantity_id = _curPubQuantity.pub_pubquantity_id;
            holiday_qty_row.pub_pubquantitytype_id = 5;
            holiday_qty_row.SetinsertdowNull();
            if (_txtChristmas.Text.Trim() == String.Empty)
                holiday_qty_row.quantity = 0;
            else
                holiday_qty_row.quantity = Convert.ToInt32(_txtChristmas.Value);
            holiday_qty_row.createdby = MainForm.AuthorizedUser.FormattedName;
            holiday_qty_row.createddate = DateTime.Now;
            _curDayofWeekQuantities.Rows.Add(holiday_qty_row);

            holiday_qty_row = _curDayofWeekQuantities.Newpub_dayofweekquantityRow();
            _txtNewYears.Tag = holiday_qty_row.pub_dayofweekquantity_id;
            holiday_qty_row.pub_pubquantity_id = _curPubQuantity.pub_pubquantity_id;
            holiday_qty_row.pub_pubquantitytype_id = 6;
            holiday_qty_row.SetinsertdowNull();
            if (_txtNewYears.Text.Trim() == String.Empty)
                holiday_qty_row.quantity = 0;
            else
                holiday_qty_row.quantity = Convert.ToInt32(_txtNewYears.Value);
            holiday_qty_row.createdby = MainForm.AuthorizedUser.FormattedName;
            holiday_qty_row.createddate = DateTime.Now;
            _curDayofWeekQuantities.Rows.Add(holiday_qty_row);

            _gridPubQuantitiesRates.CurrentCell = null;
        }

        private void NewPubRate()
        {
            _lblPubRateInfo.Text = _PubRateCopiedText;
            _dtPubRateEffectiveDate.Value = DateTime.Today;
            _gridPubRates.Columns[1].HeaderText = _cboRateType.Text;

            _curPubRate = _dsPublications.pub_pubrate.Newpub_pubrateRow();
            _curPubRate.pub_pubrate_map_id = _curPubRateMap.pub_pubrate_map_id;
            _curPubRate.pub_ratetype_id = Convert.ToInt32(_cboRateType.SelectedValue);
            _curPubRate.chargeblowin = _chkChargeForBlowIn.Checked;
            _curPubRate.effectivedate = _dtPubRateEffectiveDate.Value.Date;
            if (_radioBilled.Checked)
                _curPubRate.quantitychargetype = 1;
            else
                _curPubRate.quantitychargetype = 2;
            if (_txtBilledPctLess.Text.Trim() == String.Empty)
                _curPubRate.billedpct = 0;
            else
                _curPubRate.billedpct = Convert.ToDecimal(_txtBilledPctLess.Text.Trim());
            _curPubRate.createdby = MainForm.AuthorizedUser.FormattedName;
            _curPubRate.createddate = DateTime.Now;


            // Reset insert discounts too
            Publications.pub_insertdiscountsDataTable tmpInsertDiscounts;
            if (_curPubInsertDiscounts.Count == 0)
                tmpInsertDiscounts = new Publications.pub_insertdiscountsDataTable();
            else
                tmpInsertDiscounts = (Publications.pub_insertdiscountsDataTable)_curPubInsertDiscounts.Copy();

            _curPubInsertDiscounts.Clear();
            _curPubInsertDiscounts.pub_pubrate_idColumn.DefaultValue = _curPubRate.pub_pubrate_id;
            _curPubInsertDiscounts.createdbyColumn.DefaultValue = MainForm.AuthorizedUser.FormattedName;
            _curPubInsertDiscounts.createddateColumn.DefaultValue = DateTime.Now;
            _curPubInsertDiscounts.modifiedbyColumn.DefaultValue = MainForm.AuthorizedUser.FormattedName;
            _curPubInsertDiscounts.modifieddateColumn.DefaultValue = DateTime.Now;

            foreach (Publications.pub_insertdiscountsRow old_row in tmpInsertDiscounts.Rows)
            {
                Publications.pub_insertdiscountsRow new_row = _curPubInsertDiscounts.Newpub_insertdiscountsRow();
                new_row.insert = old_row.insert;
                new_row.discount = old_row.discount;
                _curPubInsertDiscounts.Addpub_insertdiscountsRow(new_row);
            }

            _dsPublications.pub_insertdiscounts.DefaultView.RowFilter = "pub_pubrate_id = " + _curPubRate.pub_pubrate_id.ToString();
           
            // Reset pub rates grid
            _curDayofWeekRateTypes = (Publications.pub_dayofweekratetypesDataTable)_dsPublications.pub_dayofweekratetypes.Clone();
            _curDayofWeekRateTypes.DefaultView.Sort = "ratetypedescription";
            _curDayofWeekRateTypes.pub_pubrate_idColumn.DefaultValue = _curPubRate.pub_pubrate_id;
            _curDayofWeekRateTypes.ratetypedescriptionColumn.DefaultValue = 0;
            _curDayofWeekRateTypes.createdbyColumn.DefaultValue = MainForm.AuthorizedUser.FormattedName;
            _curDayofWeekRateTypes.createddateColumn.DefaultValue = DateTime.Now;

            _curDayofWeekRates = (Publications.pub_dayofweekratesDataTable)_dsPublications.pub_dayofweekrates.Clone();
            _curDayofWeekRates.rateColumn.DefaultValue = 0;
            _curDayofWeekRates.createdbyColumn.DefaultValue = MainForm.AuthorizedUser.FormattedName;
            _curDayofWeekRates.createddateColumn.DefaultValue = DateTime.Now;

            foreach (DataGridViewRow r in _gridPubRates.Rows)
                if (!r.IsNewRow)
                {

                    r.Cells[0].Value = null;
                    r.Cells[2].Value = null;
                    r.Cells[3].Value = null;
                    r.Cells[4].Value = null;
                    r.Cells[5].Value = null;
                    r.Cells[6].Value = null;
                    r.Cells[7].Value = null;
                    r.Cells[8].Value = null;
                }

            _gridInsertsPerDayDiscount.CurrentCell = null;
            _gridPubRates.CurrentCell = null;
        }

        private void NewPubRateMapActivate()
        {
            _lblPubLocInfo.Text = "Publication: " + _cboPublication.Text.Trim() + " Location: " + _cboLocation.Text.Trim();

            _chkActive.Checked = true;
            _dtEffectiveDate.Value = DateTime.Today;

            _curPubRateMap = null;
            _curPubRateMapActivate = null;

            foreach (Publications.pub_pubrate_mapRow r in _dsPublications.pub_pubrate_map)
                if (r.pub_id == _cboPublication.SelectedValue.ToString() && r.publoc_id == Convert.ToInt32(_cboLocation.SelectedValue))
                {
                    _curPubRateMap = r;
                    break;
                }

            if (_curPubRateMap == null)
            {
                _curPubRateMap = _dsPublications.pub_pubrate_map.Newpub_pubrate_mapRow();
                _curPubRateMap.pub_id = _cboPublication.SelectedValue.ToString();
                _curPubRateMap.publoc_id = Convert.ToInt32(_cboLocation.SelectedValue);
                _curPubRateMap.createdby = MainForm.AuthorizedUser.FormattedName;
                _curPubRateMap.createddate = DateTime.Now;
            }

            _curPubRateMapActivate = _dsPublications.pub_pubrate_map_activate.Newpub_pubrate_map_activateRow();
            _curPubRateMapActivate.pub_pubrate_map_id = _curPubRateMap.pub_pubrate_map_id;
            _curPubRateMapActivate.effectivedate = _dtEffectiveDate.Value.Date;
            _curPubRateMapActivate.createdby = MainForm.AuthorizedUser.FormattedName;
            _curPubRateMapActivate.createddate = DateTime.Now;
        }

        #endregion

        #region Lock / Unlock Controls
        private void UnlockPubRateTab()
        {
            _dtPubRateEffectiveDate.Enabled = true;
            _cboRateType.Enabled = true;
            _chkChargeForBlowIn.Enabled = true;
            _cboBlowInRateType.Enabled = _chkChargeForBlowIn.Checked;
            _groupChargeForQuantity.Enabled = true;
            
            _gridInsertsPerDayDiscount.ReadOnly = false;
            _gridInsertsPerDayDiscount.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
            _gridInsertsPerDayDiscount.AllowUserToAddRows = true;

            _gridPubRates.ReadOnly = false;
            _gridPubRates.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
            _gridPubRates.AllowUserToAddRows = true;
        }

        private void LockPubQuantityTab()
        {
            _dtPubRateQuantitiesEffDate.Enabled = false;
            _txtThanksgiving.ReadOnly = true;
            _txtChristmas.ReadOnly = true;
            _txtNewYears.ReadOnly = true;

            _gridPubQuantitiesRates.ReadOnly = true;
            _gridPubQuantitiesRates.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
        }

        private void UnlockPubQuantityTab()
        {
            _dtPubRateQuantitiesEffDate.Enabled = true;
            _txtThanksgiving.ReadOnly = false;
            _txtChristmas.ReadOnly = false;
            _txtNewYears.ReadOnly = false;

            _gridPubQuantitiesRates.ReadOnly = false;
            _gridPubQuantitiesRates.Columns[0].ReadOnly = true;
            _gridPubQuantitiesRates.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
        }

        private void LockPubRateMapTab()
        {
            _dtEffectiveDate.Enabled = false;
            _chkActive.Enabled = false;
        }

        private void UnlockPubRateMapTab()
        {
            _dtEffectiveDate.Enabled = true;
            _chkActive.Enabled = true;
        }

        #endregion

        #region Event Handlers

        private void _btnSearch_Click(object sender, EventArgs e)
        {
            if (!IsLoading)
            {
                if (_curPubRateMap != null && _curPubRateMapActivate != null)
                {
                    if (!ValidatePubRateMap())
                    {
                        DialogResult result = MessageBox.Show("The publication rate activation date you entered contains invalid data." + System.Environment.NewLine + "If you continue, any unsaved changes will be lost.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No)
                            return;
                    }
                    else
                        WritePubRateMapToDataSet();
                }

                if (_curPubRate != null)
                {
                    if (!ValidatePubRate())
                    {
                        DialogResult result = MessageBox.Show("The publication rate you have entered contains invalid data." + System.Environment.NewLine + "If you continue, any unsaved changes will be lost.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No)
                            return;
                    }
                    else
                    {
                        WritePubRateMapToDataSet();
                        WritePubRateToDataSet();
                    }
                }

                if (_curPubQuantity != null)
                {
                    if (!ValidatePubQuantity())
                    {
                        DialogResult result = MessageBox.Show("The publication quantity you have entered contains invalid data." + System.Environment.NewLine + "If you continue, any unsaved changes will be lost.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No)
                            return;
                    }
                    else
                    {
                        WritePubRateMapToDataSet();
                        WritePubQuantityToDataSet();
                    }
                }
            }

            if (_cboPublication.SelectedValue == null || _cboLocation.SelectedValue == null)
            {
                MessageBox.Show("Please select a Publication and Location to search for.", "No Search Criteria", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (_curPubRateMap != null)
            {
                InitPubInfo();
                InitPubRate();
                InitPubQuantity();
            }
            else
                MessageBox.Show("The selected publication location does not have any rate information available." + System.Environment.NewLine + "Click New to create a new set of rates.", "No Data Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            ClearErrorText();
        }

        private void _btnNew_Click(object sender, EventArgs e)
        {
            if (_cboPublication.SelectedValue == null || _cboLocation.SelectedValue == null)
            {
                MessageBox.Show(Properties.Resources.SelectPubLocation, Properties.Resources.SelectPubLocationCaption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (_tabControl.SelectedIndex == 0)
            {
                if (_cboPublication.SelectedValue == null || _cboLocation.SelectedValue == null)
                {
                    MessageBox.Show(Properties.Resources.SelectPubLocation, Properties.Resources.SelectPubLocationCaption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                this.Dirty = true;
                bool tmpIsLoading = IsLoading;
                IsLoading = true;
                NewPubRateMapActivate();
                UnlockPubRateMapTab();
                _dtEffectiveDate.Focus();
                IsLoading = tmpIsLoading;
            }
            else if (_tabControl.SelectedIndex == 1)
            {
                if (_curPubRateMap == null || _curPubRateMapActivate == null)
                {
                    MessageBox.Show("You must select or create an effective date for the publication location before entering rates.", "Create an Effective Date.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                this.Dirty = true;
                if (_curPubRate != null && !ValidatePubRate())
                {
                    DialogResult result = MessageBox.Show("The current Publication Rate contains invalid data and cannot be updated." + System.Environment.NewLine + "Do you want to continue and lose any changes made?", "Lose Changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        bool tmpIsLoading = IsLoading;
                        IsLoading = true;
                        NewPubRate();
                        UnlockPubRateTab();
                        _dtPubRateEffectiveDate.Focus();
                        IsLoading = tmpIsLoading;
                    }
                    else
                        return;
                }
                else
                {
                    bool tmpIsLoading = IsLoading;
                    IsLoading = true;
                    if (_curPubRate != null)
                    {
                        WritePubRateMapToDataSet();
                        WritePubRateToDataSet();
                    }
                    NewPubRate();
                    UnlockPubRateTab();
                    _dtPubRateEffectiveDate.Focus();
                    IsLoading = tmpIsLoading;
                }
            }
            else if (_tabControl.SelectedIndex == 2)
            {
                if (_curPubRateMap == null || _curPubRateMapActivate == null)
                {
                    MessageBox.Show("You must select or create an effective date for the publication location before entering new quantities.", "Create an Effective Date.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                this.Dirty = true;
                if (_curPubQuantity != null && !ValidatePubQuantity())
                {
                    DialogResult result = MessageBox.Show("The current Publication Quantity contains invalid data and cannot be updated." + System.Environment.NewLine + "Do you want to continue and lose any changes made?", "Lose Changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        bool tmpIsLoading = IsLoading;
                        IsLoading = true;
                        NewPubQuantity();
                        UnlockPubQuantityTab();
                        _dtPubRateQuantitiesEffDate.Focus();
                        IsLoading = tmpIsLoading;
                    }
                    else
                        return;
                }
                else
                {
                    bool tmpIsLoading = IsLoading;
                    IsLoading = true;
                    if (_curPubQuantity != null)
                    {
                        WritePubRateMapToDataSet();
                        WritePubQuantityToDataSet();
                    }
                    NewPubQuantity();
                    UnlockPubQuantityTab();
                    _dtPubRateQuantitiesEffDate.Focus();
                    IsLoading = tmpIsLoading;
                }
            }

            ClearErrorText();
        }

        private void _btnDelete_Click(object sender, EventArgs e)
        {
            if (_tabControl.SelectedIndex == 1)
            {
                if (_curPubRate == null)
                {
                    MessageBox.Show("You must select a publication rate to delete.", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                DateTime pubRateMapActivate_EffectiveDate = _curPubRateMapActivate.effectivedate;

                DialogResult result = MessageBox.Show("This will delete the entire publication rate from the system.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                    return;

                _curPubRate.Delete();
                _curPubRate = null;

                IsLoading = true;
                ClearPubRate();
                LockPubRateTab();
                IsLoading = false;

                _errorProvider.Clear();
                this.Dirty = true;
            }
            else if (_tabControl.SelectedIndex == 2)
            {
                if (_curPubQuantity == null)
                {
                    MessageBox.Show("You must select a publication quantity to delete.", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                DateTime pubRateMapActivate_EffectiveDate = _curPubRateMapActivate.effectivedate;

                DialogResult result = MessageBox.Show("This will delete the entire publication quantity from the system.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                    return;

                _curPubQuantity.Delete();
                _curPubQuantity = null;

                IsLoading = true;
                ClearPubQuantity();
                LockPubQuantityTab();
                IsLoading = false;

                _errorProvider.Clear();
                this.Dirty = true;
            }
        }

        private void ucpAdminInsertionRates_Load(object sender, EventArgs e)
        {
            if (MainForm.AuthorizedUser.Right == UserRights.SuperAdmin || MainForm.AuthorizedUser.Right == UserRights.Admin)
            {
                _btnNew.Enabled = true;
                _btnDelete.Enabled = ( _tabControl.SelectedIndex > 0 );
            }
            else
            {
                _btnNew.Enabled = false;
                _btnDelete.Enabled = false;
            }
        }

        private void _cboRateType_SelectedValueChanged(object sender, EventArgs e)
        {
            _gridPubRates.Columns[1].HeaderText = _cboRateType.Text;
            if ( _cboRateType.SelectedValue != null )
            {
                if ( (int)_cboRateType.SelectedValue == 1 )
                {
                    _chkChargeForBlowIn.Enabled = true;
                }
                else
                {
                    _chkChargeForBlowIn.Checked = false;
                    _chkChargeForBlowIn.Enabled = false;
                }
            }
        }

        private void _chkChargeForBlowIn_CheckedChanged(object sender, EventArgs e)
        {
            //if (IsLoading)
            //    return;

            if (!_chkChargeForBlowIn.Checked)
                _cboBlowInRateType.SelectedIndex = 0;

            _cboBlowInRateType.Enabled = _chkChargeForBlowIn.Checked;
        }

        private void _cboPublication_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_revertPublicationSelection)
                return;

            if (!IsLoading)
            {
                _lblPubLocInfo.Text = String.Empty;
                _lblPubRateInfo.Text = String.Empty;
                _lblPubQtyInfo.Text = String.Empty;

                if (_curPubRateMap != null && _curPubRateMapActivate != null)
                {
                    if (!ValidatePubRateMap())
                    {
                        DialogResult result = MessageBox.Show("The publication rate activation date you entered contains invalid data." + System.Environment.NewLine + "If you continue, any unsaved changes will be lost.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No)
                        {
                            _revertPublicationSelection = true;
                            _cboPublication.SelectedValue = _prevPublicationSelection;
                            _revertPublicationSelection = false;
                            return;
                        }
                        else
                        {
                            _errorProvider.SetError(_dtEffectiveDate, String.Empty);
                        }
                    }
                    else
                        WritePubRateMapToDataSet();
                }

                if (_curPubRate != null)
                {
                    if (!ValidatePubRate())
                    {
                        DialogResult result = MessageBox.Show("The publication rate you have entered contains invalid data." + System.Environment.NewLine + "If you continue, any unsaved changes will be lost.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No)
                        {
                            _revertPublicationSelection = true;
                            _cboPublication.SelectedValue = _prevPublicationSelection;
                            _revertPublicationSelection = false;
                            return;
                        }
                        else
                        {
                            ClearPubRate();
                            _PubRateCopiedText = "New Rate";
                        }
                    }
                    else
                    {
                        if (_curPubRateMap != null && _curPubRateMapActivate != null)
                            WritePubRateMapToDataSet();
                        WritePubRateToDataSet();
                        ClearPubRate();
                        _PubRateCopiedText = "New Rate";
                    }
                }

                if (_curPubQuantity != null)
                {
                    if (!ValidatePubQuantity())
                    {
                        DialogResult result = MessageBox.Show("The publication quantity you have entered contains invalid data." + System.Environment.NewLine + "If you continue, any unsaved changes will be lost.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No)
                        {
                            _revertPublicationSelection = true;
                            _cboPublication.SelectedValue = _prevPublicationSelection;
                            _revertPublicationSelection = false;
                            return;
                        }
                        else
                        {
                            ClearPubQuantity();
                            _PubQtyCopiedText = "New Quantities";
                        }

                    }
                    else
                    {
                        WritePubRateMapToDataSet();
                        WritePubQuantityToDataSet();
                        ClearPubQuantity();
                        _PubQtyCopiedText = "New Quantities";
                    }
                }

                _prevPublicationSelection = _cboPublication.SelectedValue.ToString();

                ClearPubRateMap();
                ClearPubRate();
                ClearPubQuantity();
                _curPubRateMap = null;
                _curPubRateMapActivate = null;
                _curPubRate = null;
                _curPubQuantity = null;
            }

            InitLocationCombo();

            _dsPublications.pub_pubrate_map_activate.DefaultView.RowFilter = "0 = 1";
            _dsPublications.pub_pubrate.DefaultView.RowFilter = "0 = 1";
            _dsPublications.pub_pubquantity.DefaultView.RowFilter = "0 = 1";

            _cboEffectiveDate.Enabled = false;
            _cboPubRateEffectiveDate.Enabled = false;
            _cboPubQuantityEffectiveDate.Enabled = false;
            _btnSearch.Enabled = false;
            LockPubRateMapTab();
            LockPubRateTab();
            LockPubQuantityTab();

            _curPubRate = null;
            _curPubQuantity = null;


            if (_cboLocation.Items.Count > 0)
            {
                if (_cboLocation.SelectedIndex == -1)
                {
                    _cboLocation.SelectedIndex = 0;
                }
                _cboLocation_SelectedValueChanged(sender, e);
            }

            ClearErrorText();
        }

        private void _cboLocation_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_revertLocationSelection)
                return;

            if (!IsLoading)
            {
                if (_curPubRateMap != null && _curPubRateMapActivate != null)
                {
                    if (!ValidatePubRateMap())
                    {
                        DialogResult result = MessageBox.Show("The publication rate activation date you entered contains invalid data." + System.Environment.NewLine + "If you continue, any unsaved changes will be lost.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No)
                            return;
                    }
                    else
                        WritePubRateMapToDataSet();
                }

                if (_curPubRate != null)
                {
                    if (!ValidatePubRate())
                    {
                        DialogResult result = MessageBox.Show("The publication rate you have entered contains invalid data." + System.Environment.NewLine + "If you continue, any unsaved changes will be lost.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No)
                        {
                            _revertLocationSelection = true;
                            _cboLocation.SelectedValue = _prevLocationSelection;
                            _revertLocationSelection = false;
                            return;
                        }
                    }
                    else
                    {
                        if (_curPubRateMap != null)
                            WritePubRateMapToDataSet();
                        WritePubRateToDataSet();
                    }
                }

                if (_curPubQuantity != null)
                {
                    if (!ValidatePubQuantity())
                    {
                        DialogResult result = MessageBox.Show("The publication quantity you have entered contains invalid data." + System.Environment.NewLine + "If you continue, any unsaved changes will be lost.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No)
                            return;
                    }
                    else
                    {
                        if (_curPubRateMap != null)
                            WritePubRateMapToDataSet();
                        WritePubQuantityToDataSet();
                    }
                }

                _prevLocationSelection = Convert.ToInt32(_cboLocation.SelectedValue);
            }

            InitSearchCombos();

            LockPubRateMapTab();
            LockPubRateTab();
            LockPubQuantityTab();
        }

        private void _gridInsertsPerDayDiscount_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!IsLoading)
                this.Dirty = true;
        }

        private void _gridInsertsPerDayDiscount_UserDeletedRow( object sender, DataGridViewRowEventArgs e )
        {
            if ( !IsLoading )
                this.Dirty = true;
        }

        private void _gridPubRates_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!IsLoading)
                this.Dirty = true;
        }

        private void _gridPubQuantitiesRates_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!IsLoading)
                this.Dirty = true;
        }

        private void Control_Changed(object sender, EventArgs e)
        {
            if (!IsLoading)
            {
                this.Dirty = true;
                _gridPubQuantitiesRates.EndEdit();
            }
        }

        private void _radioBilled_CheckedChanged(object sender, EventArgs e)
        {
            _txtBilledPctLess.Enabled = _radioBilled.Checked;
        }

        private void _radioSent_CheckedChanged(object sender, EventArgs e)
        {
            _txtBilledPctLess.Enabled = !_radioSent.Checked;
        }

        private void _gridPubQuantitiesRates_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (!IsLoading)
                this.Dirty = true;
        }

        #endregion

        #region Validation Methods
        private bool ValidatePubQuantity()
        {
            CancelEventArgs e = new CancelEventArgs();

            _dtPubRateQuantitiesEffDate_Validating(this, e);
            if (Validate_PubQuantitiesGrid())
            {
                if (_gridPubQuantitiesRates.CurrentRow != null && _gridPubQuantitiesRates.CurrentRow.IsNewRow)
                {
                    _gridPubQuantitiesRates.CancelEdit();
                    _gridPubQuantitiesRates.BindingContext[_curDayofWeekQuantities].CancelCurrentEdit();
                }
                else
                {
                    _gridPubQuantitiesRates.EndEdit();
                    _gridPubQuantitiesRates.BindingContext[_curDayofWeekQuantities].EndCurrentEdit();
                }
            }
            else
                e.Cancel = true;
            ValidateRequired(_txtThanksgiving, e);
            ValidateRequired(_txtChristmas, e);
            ValidateRequired(_txtNewYears, e);

            if (e.Cancel)
                return false;
            else
                return true;
        }

        private bool ValidateRequired(Control ctrl, CancelEventArgs e)
        {
            if (IsLoading)
                return true;

            if (string.IsNullOrEmpty(ctrl.Text))
            {
                _errorProvider.SetError(ctrl, Resources.RequiredFieldError);
                e.Cancel = true;
                return false;
            }

            _errorProvider.SetError(ctrl, string.Empty);
            return true;
        }

        private void _gridInsertsPerDayDiscount_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (IsLoading || _gridInsertsPerDayDiscount.Rows[e.RowIndex].IsNewRow)
                return;

            List<int> InsertNumbers = new List<int>();
            for (int iRow = 0; iRow < _gridInsertsPerDayDiscount.RowCount; ++iRow)
                if (!_gridInsertsPerDayDiscount.Rows[iRow].IsNewRow && iRow != e.RowIndex)
                    InsertNumbers.Add((int) _gridInsertsPerDayDiscount.Rows[iRow].Cells[0].Value);


            if (_gridInsertsPerDayDiscount.Rows[e.RowIndex].Cells[0].FormattedValue == DBNull.Value || _gridInsertsPerDayDiscount.Rows[e.RowIndex].Cells[0].FormattedValue.ToString() == String.Empty)
            {
                _gridInsertsPerDayDiscount.Rows[e.RowIndex].ErrorText = Resources.RequiredFieldError;
                e.Cancel = true;
            }

            else if (_gridInsertsPerDayDiscount.Rows[e.RowIndex].Cells[1].FormattedValue == DBNull.Value || _gridInsertsPerDayDiscount.Rows[e.RowIndex].Cells[1].FormattedValue.ToString() == String.Empty)
            {
                _gridInsertsPerDayDiscount.Rows[e.RowIndex].ErrorText = Resources.RequiredFieldError;
                e.Cancel = true;
            }

            else if (Convert.ToInt32(_gridInsertsPerDayDiscount.Rows[e.RowIndex].Cells[0].FormattedValue) < 2)
            {
                _gridInsertsPerDayDiscount.Rows[e.RowIndex].ErrorText = "Insert Number must be greater than 1";
                e.Cancel = true;
            }

            else if (InsertNumbers.Contains(Convert.ToInt32(_gridInsertsPerDayDiscount.Rows[e.RowIndex].Cells[0].FormattedValue)))
            {
                _gridInsertsPerDayDiscount.Rows[e.RowIndex].ErrorText = "Insert Number Already Exists";
                e.Cancel = true;
            }

            else if (Convert.ToDecimal(_gridInsertsPerDayDiscount.Rows[e.RowIndex].Cells[1].FormattedValue) > 100)
            {
                _gridInsertsPerDayDiscount.Rows[e.RowIndex].ErrorText = "Insert Discount cannot exceed 100%";
                e.Cancel = true;
            }
            else
                _gridInsertsPerDayDiscount.Rows[e.RowIndex].ErrorText = String.Empty;
        }

        private bool ValidateControl ()
        {
            CancelEventArgs e = new CancelEventArgs();

            // Validation occurs prior to saving or leaving the control.  Store any values needed to repopulate the control now.
            if (_curPubRateMap != null)
            {
                _dtPubRateMapEffectiveDate_Validating(this, e);
                _Pub_ID = _cboPublication.SelectedValue.ToString();
                _PubLoc_ID = Convert.ToInt32(_cboLocation.SelectedValue);
                _PubEffectiveDate = _dtEffectiveDate.Value.Date;
                _PubRateEffectiveDate = _dtPubRateEffectiveDate.Value.Date;
                _PubQuantityEffectiveDate = _dtPubRateQuantitiesEffDate.Value.Date;
            }
            else
            {
                _Pub_ID = "";
                _PubLoc_ID = -1;
                _PubEffectiveDate = null;
                _PubRateEffectiveDate = null;
                _PubQuantityEffectiveDate = null;
            }

            if (!ValidatePubRate())
                e.Cancel = true;

            if (_curPubQuantity != null)
            {
                _dtPubRateQuantitiesEffDate_Validating(this, e);

                if (Validate_PubQuantitiesGrid())
                {
                    if (_gridPubQuantitiesRates.CurrentRow != null &&  _gridPubQuantitiesRates.CurrentRow.IsNewRow)
                    {
                        _gridPubQuantitiesRates.CancelEdit();
                    }
                    else
                    {
                        _gridPubQuantitiesRates.EndEdit();
                    }
                }
                    else
                        e.Cancel = true;
                ValidateRequired(_txtThanksgiving, e);
                ValidateRequired(_txtChristmas, e);
                ValidateRequired(_txtNewYears, e);
            }

            return (!e.Cancel);
        }

        private bool Validate_InsertsPerDayDiscountGrid()
        {
            bool retval = true;

            if (IsLoading)
                return retval;

            List<int> InsertNumbers = new List<int>();
            for (int iRow = 0; iRow < _gridInsertsPerDayDiscount.RowCount; ++iRow)
                if (!_gridInsertsPerDayDiscount.Rows[iRow].IsNewRow)
                {
                    if (_gridInsertsPerDayDiscount.Rows[iRow].Cells[0].EditedFormattedValue == DBNull.Value || _gridInsertsPerDayDiscount.Rows[iRow].Cells[0].EditedFormattedValue.ToString() == String.Empty)
                    {
                        _gridInsertsPerDayDiscount.Rows[iRow].ErrorText = Resources.RequiredFieldError;
                        retval = false;
                    }

                    else if (_gridInsertsPerDayDiscount.Rows[iRow].Cells[1].EditedFormattedValue == DBNull.Value || _gridInsertsPerDayDiscount.Rows[iRow].Cells[1].EditedFormattedValue.ToString() == String.Empty)
                    {
                        _gridInsertsPerDayDiscount.Rows[iRow].ErrorText = Resources.RequiredFieldError;
                        retval = false;
                    }
                    else if (Convert.ToDecimal(_gridInsertsPerDayDiscount.Rows[iRow].Cells[1].EditedFormattedValue) > 100)
                    {
                        _gridInsertsPerDayDiscount.Rows[iRow].ErrorText = "Discount Amount must be between 0 and 100";
                        retval = false;
                    }
                    else if (Convert.ToInt32(_gridInsertsPerDayDiscount.Rows[iRow].Cells[0].EditedFormattedValue) < 2)
                    {
                        _gridInsertsPerDayDiscount.Rows[iRow].ErrorText = "Insert Number must be greater than 1";
                        retval = false;
                    }

                    else if (InsertNumbers.Contains(Convert.ToInt32(_gridInsertsPerDayDiscount.Rows[iRow].Cells[0].EditedFormattedValue)))
                    {
                        _gridInsertsPerDayDiscount.Rows[iRow].ErrorText = "Insert Number Already Exists";
                        retval = false;
                    }

                    else
                    {
                        _gridInsertsPerDayDiscount.Rows[iRow].ErrorText = String.Empty;
                        InsertNumbers.Add(Convert.ToInt32(_gridInsertsPerDayDiscount.Rows[iRow].Cells[0].EditedFormattedValue));
                    }
                }

            return retval;
        }

        private bool Validate_PubRatesGrid()
        {
            bool retval = true;

            if (IsLoading)
                return retval;

            // First clear any errors.  (If they are still present, they'll be flagged again
            foreach (DataGridViewRow r in _gridPubRates.Rows)
                r.ErrorText = String.Empty;

            System.Text.RegularExpressions.Regex regex_Integer = new System.Text.RegularExpressions.Regex("[0-9]+");

            int RateType = (int)_cboRateType.SelectedValue;

            // RateType Description must be integer for tab page count
            if (RateType == 1)
            {
                foreach (DataGridViewRow r in _gridPubRates.Rows)
                    if (!r.IsNewRow && (r.Cells[1].EditedFormattedValue == null || !regex_Integer.IsMatch(r.Cells[1].EditedFormattedValue.ToString())))
                    {
                        r.ErrorText = "Tab Page Count publication rates must be of type integer";
                        retval = false;
                    }
            }

            // Flat Rate can only have one rate
            if (RateType == 2)
            {
                if (_gridPubRates.Rows[0].Cells[1] != null && !String.IsNullOrEmpty(_gridPubRates.Rows[0].Cells[1].EditedFormattedValue.ToString()))
                {
                    _gridPubRates.Rows[0].ErrorText = "Flat publication rates cannot have a description";
                    retval = false;
                }

                if (_gridPubRates.Rows.Count > 2)
                {
                    foreach (DataGridViewRow r in _gridPubRates.Rows)
                        if (!r.IsNewRow && r.Index != 0)
                            r.ErrorText = "Flat publication rates can only have one rate";

                    retval = false;
                }
            }

            List<string> PubRateDescriptions = new List<string>();
            foreach (DataGridViewRow r in _gridPubRates.Rows)
            {
                if (!r.IsNewRow)
                {
                    if (r.Cells[1].EditedFormattedValue != null && PubRateDescriptions.Contains(r.Cells[1].EditedFormattedValue.ToString()))
                    {
                        r.ErrorText = "Duplicate publication rates have been entered";
                        retval = false;
                    }

                    if (r.Cells[1].EditedFormattedValue != null)
                    {
                        PubRateDescriptions.Add(r.Cells[1].EditedFormattedValue.ToString());
                    }

                    for (int i = 0; i < 7; ++i)
                        if (r.Cells[i + 9].EditedFormattedValue == null || r.Cells[i + 9].EditedFormattedValue.ToString() == String.Empty)
                        {
                            r.ErrorText = "Missing publication rate amount";
                            retval = false;
                        }
                }
            }

            return retval;
        }

        private bool Validate_PubQuantitiesGrid()
        {
            bool retval = true;

            if (IsLoading)
                return retval;

            foreach (DataGridViewRow r in _gridPubQuantitiesRates.Rows)
                for (int i = 0; i < 7; ++i)
                    if (r.Cells[i + 8].FormattedValue == null || r.Cells[i + 8].FormattedValue == DBNull.Value || r.Cells[i + 8].FormattedValue.ToString() == String.Empty)
                    {
                        r.Cells[i + 8].ErrorText = "All publication quantities must be entered";
                        retval = false;
                    }
                    else
                        r.Cells[i + 8].ErrorText = String.Empty;

            return retval;
        }

        private void _gridPubQuantitiesRates_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (IsLoading || e.ColumnIndex < 8 || e.ColumnIndex > 14 || _gridPubQuantitiesRates.ReadOnly)
                return;

            if (e.FormattedValue == null
                    || e.FormattedValue == DBNull.Value
                    || e.FormattedValue.ToString() == String.Empty)
                _gridPubQuantitiesRates.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "All publication quantities must be entered";
            else
                _gridPubQuantitiesRates.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = String.Empty;
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

        private void ValidateDecimalRange(DecimalTextBox ctl, Decimal min, Decimal max, CancelEventArgs e)
        {
            if (IsLoading || ctl.Text == String.Empty)
                return;

            if (Convert.ToDecimal(ctl.Text) < min || Convert.ToDecimal(ctl.Text) > max)
            {
                _errorProvider.SetError(ctl, "Value must be between " + min.ToString() + " and " + max.ToString());
                e.Cancel = true;
            }
            else
                _errorProvider.SetError(ctl, string.Empty);
        }

        private bool ValidatePubRateMap()
        {
            if (_curPubRateMap == null && _curPubRateMapActivate == null)
                return true;

            CancelEventArgs cea = new CancelEventArgs();
            _dtPubRateMapEffectiveDate_Validating(this, cea);
            if (cea.Cancel)
                return false;
            else
                return true;
        }

        private bool ValidatePubRate()
        {
            if (_curPubRate == null)
                return true;

            CancelEventArgs cea = new CancelEventArgs();

            _dtPubRateEffectiveDate_Validating(this, cea);
            ValidateRequiredSelection(_cboRateType, cea);
            if (_chkChargeForBlowIn.Checked)
            {
                if (_cboBlowInRateType.SelectedIndex < 1)
                {
                    _errorProvider.SetError(_cboBlowInRateType, "Blow-In Rate Type cannot be none if Charge for Blow-In is checked");
                    cea.Cancel = true;
                }
                else
                {
                    _errorProvider.SetError(_cboBlowInRateType, String.Empty);
                }
            }
            else
            {
                _errorProvider.SetError(_cboBlowInRateType, String.Empty);
            }
            ValidateDecimalRange(_txtBilledPctLess, 0, 100, cea);
            if (Validate_InsertsPerDayDiscountGrid())
            {
                if (_gridInsertsPerDayDiscount.CurrentRow != null && _gridInsertsPerDayDiscount.CurrentRow.IsNewRow)
                {
                    _gridInsertsPerDayDiscount.CancelEdit();
                    _gridInsertsPerDayDiscount.BindingContext[_curPubInsertDiscounts].CancelCurrentEdit();
                }
                else
                {
                    _gridInsertsPerDayDiscount.EndEdit();
                    _gridInsertsPerDayDiscount.BindingContext[_curPubInsertDiscounts].EndCurrentEdit();
                }
            }
            else
                cea.Cancel = true;

            if (Validate_PubRatesGrid())
                _gridPubRates.EndEdit();
            else
                cea.Cancel = true;

            if (cea.Cancel)
                return false;
            else
                return true;
        }

        private void _dtPubRateEffectiveDate_Validating(object sender, CancelEventArgs e)
        {
            DataView PubRates_DataView = new DataView();
            PubRates_DataView.Table = _dsPublications.pub_pubrate;
            PubRates_DataView.RowFilter = "pub_pubrate_map_id = " + _curPubRate.pub_pubrate_map_id.ToString() + " and effectivedate = '" + _dtPubRateEffectiveDate.Value.ToShortDateString() + "' and pub_pubrate_id <> " + _curPubRate.pub_pubrate_id.ToString();
            if (PubRates_DataView.Count > 0)
            {
                _errorProvider.SetError(_dtPubRateEffectiveDate, "A publication rate with the same effective date already exists");
                e.Cancel = true;
            }
            else
                _errorProvider.SetError(_dtPubRateEffectiveDate, String.Empty);
        }

        private void _dtPubRateMapEffectiveDate_Validating(object sender, CancelEventArgs e)
        {
            foreach (DataRow dr in _dsPublications.pub_pubrate_map_activate.Select("pub_pubrate_map_id = " + _curPubRateMap.pub_pubrate_map_id.ToString() + " and effectivedate = '" + _dtEffectiveDate.Value.ToShortDateString() + "'"))
            {
                if (((Publications.pub_pubrate_map_activateRow) dr) != _curPubRateMapActivate)
                {
                    _errorProvider.SetError(_dtEffectiveDate, "The publication location already has an activation record with the specified effective date.");
                    e.Cancel = true;
                    return;
                }
            }

            _errorProvider.SetError(_dtEffectiveDate, String.Empty);
        }

        private void _dtPubRateQuantitiesEffDate_Validating(object sender, CancelEventArgs e)
        {
            _errorProvider.SetError(_dtPubRateQuantitiesEffDate, String.Empty);

            if (_curPubQuantity == null)
                return;

            DataView PubQty_DataView = new DataView();
            PubQty_DataView.Table = _dsPublications.pub_pubquantity;
            PubQty_DataView.RowFilter = "pub_pubrate_map_id = " + _curPubQuantity.pub_pubrate_map_id.ToString() + " and effectivedate = '" + _dtPubRateQuantitiesEffDate.Value.ToShortDateString() + "' and pub_pubquantity_id <> " + _curPubQuantity.pub_pubquantity_id.ToString();
            if (PubQty_DataView.Count > 0)
            {
                _errorProvider.SetError(_dtPubRateQuantitiesEffDate, "Quantities already exist for the publication with the same effective date");
                e.Cancel = true;
            }
        }

        #endregion

        private void ClearPubRateMap()
        {
            bool tmpIsLoading = IsLoading;
            IsLoading = true;

            _chkActive.Checked = true;
            _dtEffectiveDate.Value = DateTime.Today;

            IsLoading = tmpIsLoading;
        }

        private void ClearPubRate()
        {
            bool tmpIsLoading = IsLoading;
            IsLoading = true;

            _dtPubRateEffectiveDate.Value = DateTime.Today;
            _cboRateType.SelectedIndex = 0;
            _chkChargeForBlowIn.Checked = false;
            _cboBlowInRateType.SelectedIndex = 0;
            _radioBilled.Checked = false;
            _radioSent.Checked = true;
            _txtBilledPctLess.Text = String.Empty;
            _txtBilledPctLess.Enabled = false;
            _curPubInsertDiscounts.Clear();
            _curDayofWeekRateTypes.Clear();
            _curDayofWeekRates.Clear();
            _gridPubRates.Rows.Clear();
            _dsPublications.pub_dayofweekratetypes.DefaultView.RowFilter = "0 = 1";

            _errorProvider.SetError(_dtPubRateEffectiveDate, String.Empty);
            _errorProvider.SetError(_cboRateType, String.Empty);
            _errorProvider.SetError(_cboBlowInRateType, String.Empty);
            _errorProvider.SetError(_txtBilledPctLess, String.Empty);

            IsLoading = tmpIsLoading;
        }

        private void ClearPubQuantity()
        {
            bool tmpIsLoading = IsLoading;
            IsLoading = true;

            _dtPubRateQuantitiesEffDate.Value = DateTime.Today;
            _gridPubQuantitiesRates.Rows.Clear();
            _gridPubQuantitiesRates.Rows.Add(new object[] { "Minimum", null, null, null, null, null, null, null, null, null, null, null, null, null, null });
            _gridPubQuantitiesRates.Rows.Add(new object[] { "Contract Send", null, null, null, null, null, null, null, null, null, null, null, null, null, null });
            _gridPubQuantitiesRates.Rows.Add(new object[] { "Full Run", null, null, null, null, null, null, null, null, null, null, null, null, null, null });
            _txtThanksgiving.Text = String.Empty;
            _txtThanksgiving.Tag = null;
            _txtChristmas.Text = String.Empty;
            _txtChristmas.Tag = null;
            _txtNewYears.Text = String.Empty;
            _txtNewYears.Tag = null;

            _errorProvider.SetError(_dtPubRateEffectiveDate, String.Empty);
            _errorProvider.SetError(_txtThanksgiving, String.Empty);
            _errorProvider.SetError(_txtChristmas, String.Empty);
            _errorProvider.SetError(_txtNewYears, String.Empty);

            IsLoading = tmpIsLoading;
        }

        private void _gridPubRates_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (!IsLoading)
                this.Dirty = true;
        }

        private void _tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MainForm.AuthorizedUser.Right == UserRights.SuperAdmin || MainForm.AuthorizedUser.Right == UserRights.Admin)
            {
                // Can't delete if showing the Activation tab (index 0)
                _btnDelete.Enabled = (_tabControl.SelectedIndex > 0);
            }

            if (_tabControl.SelectedIndex == 0)
            {
                _cboEffectiveDate.Enabled = true;
                _cboPubRateEffectiveDate.Enabled = false;
                _cboPubQuantityEffectiveDate.Enabled = false;
            }
            else if (_tabControl.SelectedIndex == 1)
            {
                _cboEffectiveDate.Enabled = false;
                _cboPubRateEffectiveDate.Enabled = true;
                _cboPubQuantityEffectiveDate.Enabled = false;
            }
            else if (_tabControl.SelectedIndex == 2)
            {
                _cboEffectiveDate.Enabled = false;
                _cboPubRateEffectiveDate.Enabled = false;
                _cboPubQuantityEffectiveDate.Enabled = true;
            }
        }

        private void _gridInsertsPerDayDiscount_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}