using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.PublicationsTableAdapters;

namespace CatalogEstimating.UserControls.Admin
{
    public partial class ucpAdminInsertionGroupSetup : CatalogEstimating.UserControlPanel
    {
        private string _curPubGroupDescription = "";
        private DateTime? _curPubGroupEffectiveDate = null;
        private int _curLocationFilter = 5;

        private Publications.pub_pubgroupRow _curPubGroup = null;
        private DataView _DataView_PubGroupEffectiveDate = new DataView();
        private DataView _DataView_AvailablePubs = new DataView();
        private DataView _DataView_PubsInGroup = new DataView();

        #region Construction

        public ucpAdminInsertionGroupSetup(Publications ds)
        {
            InitializeComponent();
            Name = "Group Setup";
            _dsPublications = ds;
        }

        #endregion

        #region Overrides

        public override void LoadData()
        {
            _curPubGroup = null;
            _errorProvider.Clear();

            if (_curPubGroupDescription == "" && _cboGroup.SelectedItem != null)
                _curPubGroupDescription = _cboGroup.SelectedText;

            BindControls();
            InitDetails();
            InitGrids();

            base.LoadData();
        }

        public override void Reload()
        {
            IsLoading = true;

            BindControls();

            InitDetails();
            InitGrids();

            if ( _curPubGroup != null && _curPubGroup.RowState != DataRowState.Detached )
                _txtDescription.ReadOnly = true;

            base.Reload();

            IsLoading = false;
        }

        public override void SaveData()
        {
            if (Dirty)
                WriteToDataset(); 
            
            _dtEffectiveDateMapping.Enabled = false;
            _txtDescription.ReadOnly = true;

            base.SaveData();
        }

        public override void PreSave(CancelEventArgs e)
        {
            // Force value to be sync'd with what may have been typed into date control.
            MainForm.UpdateDateControl(_dtEffectiveDateMapping);

            if (!ValidateControl())
            {
                e.Cancel = true;
                base.PreSave(e);
                return;
            }

            base.PreSave(e);
        }

        public override ToolStrip Toolbar
        {
            get { return _toolStrip; }
        }

        public override void Export(ref ExcelWriter writer)
        {
            writer.WriteLine(new object[] { "Insertion Group Setup Screen" });
            writer.WriteLine(new object[] { _lblGroup.Text, _cboGroup.Text });
            writer.WriteLine(new object[] { _lblEffectiveDateSearch.Text, _cboEffectiveDate.Text });
            writer.WriteLine(new object[] { _groupMapping.Text });
            writer.WriteLine(new object[] { _lblDescription.Text, _txtDescription.Text, _lblEffectiveDateMapping.Text, _dtEffectiveDateMapping.Value.Date, _chkActive.Text, _chkActive.Checked });
            writer.WriteLine(new object[] { _lblComments.Text, _txtComments.Text });
            writer.WriteLine(new object[] { _lblLocationFilter.Text, _cboLocationFilter.Text });
            writer.WriteLine(new object[] { _lblAvailablePubs.Text });
            foreach (DataGridViewRow r in _gridAvailablePubs.Rows)
            {
                Publications.pub_pubrate_mapRow pr = (Publications.pub_pubrate_mapRow) ((DataRowView) r.DataBoundItem).Row;
                writer.WriteLine(new object[] { pr.Pub_NM, pr.pub_id, pr.publoc_id });
            }

            writer.WriteLine(new object[] { _lblPubsInGroup.Text });
            foreach (DataGridViewRow r in _gridPubsInGroup.Rows)
            {
                Publications.pub_pubrate_mapRow pr = (Publications.pub_pubrate_mapRow) ((DataRowView) r.DataBoundItem).Row;
                writer.WriteLine(new object[] {pr.Pub_NM, pr.pub_id, pr.publoc_id });
            }

            base.Export(ref writer);
        }

        public override void OnLeaving(CancelEventArgs e)
        {
            if (!ValidateControl())
            {
                e.Cancel = true;
                base.OnLeaving(e);
                return;
            }

            if (Dirty)
                WriteToDataset();

            _curPubGroup = null;

            base.OnLeaving(e);
        }

        #endregion

        #region Event Handlers

        private void ucpAdminInsertionGroupSetup_Load(object sender, EventArgs e)
        {
            InitControl();
        }

        private void _btnNew_Click(object sender, EventArgs e)
        {
            if (_curPubGroup != null && !ValidateControl())
            {
                DialogResult result = MessageBox.Show(Properties.Resources.NewRecordWarningLine1 + System.Environment.NewLine + Properties.Resources.NewRecordWarningLine2, Properties.Resources.NewRecordWarningCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                    return;
                else
                {
                    foreach (DataRow r in _dsPublications.pub_pubpubgroup_map.Select("pub_pubgroup_id = " + _curPubGroup.pub_pubgroup_id.ToString()))
                        r.RejectChanges();

                    _curPubGroup.RejectChanges();
                }
            }
            else
            {
                if (_curPubGroup != null)
                    _lblGroupInformation.Text = "Copied from Publication Group: " + _txtDescription.Text.Trim() + " Effective Date: " + _dtEffectiveDateMapping.Value.ToShortDateString();
                else
                    _lblGroupInformation.Text = "New Publication Group";

                WriteToDataset();
                _curPubGroup = null;
            }

            IsLoading = true;

            // Enable the controls
            UnlockEffectiveDate();
            UnlockDetails();

            // Instantiate a new Pub Group record.
            _curPubGroup = _dsPublications.pub_pubgroup.Newpub_pubgroupRow();
            _curPubGroup.description = _txtDescription.Text.Trim();
            _curPubGroup.comments = _txtComments.Text.Trim();
            _curPubGroup.createdby = MainForm.AuthorizedUser.FormattedName;
            _curPubGroup.createddate = DateTime.Now;
            _curPubGroup.effectivedate = _dtEffectiveDateMapping.Value.Date;
            _curPubGroup.active = _chkActive.Checked;
            _curPubGroup.sortorder = -1;
            _curPubGroup.customgroupforpackage = false;
            _dsPublications.pub_pubgroup.Addpub_pubgroupRow(_curPubGroup);

            // if the grids have not been populated, do a deep copy of locations.
            if (_DataView_AvailablePubs.Table == null)
            {
                _DataView_AvailablePubs.Table = _dsPublications.pub_pubrate_map.Copy();
                _DataView_PubsInGroup.Table = new Publications.pub_pubrate_mapDataTable();
                _DataView_AvailablePubs.Sort = "Pub_ID, PubLoc_ID";
                _DataView_PubsInGroup.Sort = "Pub_ID, PubLoc_ID";
            }

            // Add each pub location to the pubpubgroup_map
            for (int i = 0; i < _DataView_PubsInGroup.Count; ++i)
            {
                Publications.pub_pubpubgroup_mapRow ds_row = _dsPublications.pub_pubpubgroup_map.Newpub_pubpubgroup_mapRow();
                ds_row.pub_pubgroupRow = _curPubGroup;

                ds_row.pub_pubgroup_id = _curPubGroup.pub_pubgroup_id;
                ds_row.pub_pubrate_map_id = ((Publications.pub_pubrate_mapRow)_DataView_PubsInGroup[i].Row).pub_pubrate_map_id;
                ds_row.createdby = MainForm.AuthorizedUser.FormattedName;
                ds_row.createddate = DateTime.Now;
                _dsPublications.pub_pubpubgroup_map.Addpub_pubpubgroup_mapRow(ds_row);
            }

            _gridAvailablePubs.DataSource = _DataView_AvailablePubs;
            _gridPubsInGroup.DataSource = _DataView_PubsInGroup;
            _gridAvailablePubs.CurrentCell = null;
            _gridPubsInGroup.CurrentCell = null;
            _txtDescription.Focus();
            _txtDescription.SelectionStart = 0;
            _txtDescription.SelectionLength = _txtDescription.Text.Length;

            // Reset the Location Filter to 5
            _curLocationFilter = 5;
            BindLocationFilter();

            _btnClear.Enabled = true;

            IsLoading = false;
            this.Dirty = true;
        }

        private bool ValidateControl()
        {
            _errorProvider.Clear();

            if (_curPubGroup == null)
                return true;

            bool retval = true;
            DataView PubGroups_DataView = new DataView();
            PubGroups_DataView.Table = _dsPublications.pub_pubgroup;
            PubGroups_DataView.RowFilter = "description = '" + _txtDescription.Text.Trim().Replace("'", "''") + "' and effectivedate = '" + _dtEffectiveDateMapping.Value.ToShortDateString() + "' and pub_pubgroup_id <> " + _curPubGroup.pub_pubgroup_id.ToString();
            if (PubGroups_DataView.Count > 0)
            {
                _errorProvider.SetError(_dtEffectiveDateMapping, "Pub Group already exists on the effective date specified");
                retval = false;
            }

            if (_txtDescription.Text.Trim() == String.Empty)
            {
                _errorProvider.SetError(_txtDescription, Properties.Resources.RequiredFieldError);
                retval = false;
            }
            if (_gridPubsInGroup.Rows.Count < 2)
            {
                _errorProvider.SetError(_gridPubsInGroup, "The group must contain at least two publications.");
                retval = false;
            }

            return retval;
        }

        private void _btnSearch_Click(object sender, EventArgs e)
        {
            if (_curPubGroup != null && !ValidateControl())
            {
                DialogResult result = MessageBox.Show("The current Publication Group contains invalid data and cannot be updated." + System.Environment.NewLine + "Do you want to continue and lose any changes made?", "Lose Changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                    return;
                else
                {
                    foreach (DataRow r in _dsPublications.pub_pubpubgroup_map.Select("pub_pubgroup_id = " + _curPubGroup.pub_pubgroup_id.ToString()))
                        r.RejectChanges();

                    _curPubGroup.RejectChanges();
                }

            }
            else
            {
                WriteToDataset();
                _curPubGroup = null;
            }

            IsLoading = true;
            _errorProvider.Clear();

            // In order to modify a group, you need to select a group name and effective date
            if (_cboGroup.SelectedIndex == -1 || _cboEffectiveDate.SelectedIndex == -1)
            {
                ClearDetails();
                LockDetails();
                MessageBox.Show("Please select a Publication Group and Effective Date.", "No Records Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                _curPubGroup = (Publications.pub_pubgroupRow)((DataRowView)_cboEffectiveDate.SelectedItem).Row;

                _curLocationFilter = 5;
                BindLocationFilter();
                InitDetails();
                InitGrids();

                _txtDescription.ReadOnly = true;
                _dtEffectiveDateMapping.Enabled = false;
                _txtComments.Focus();
            }

            IsLoading = false;
        }

        private void _cboLocationFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsLoading)
                return;

            if (_DataView_AvailablePubs.Table == null || _DataView_PubsInGroup.Table == null)
                return;

            if (_cboLocationFilter.SelectedIndex < 1)
            {
                _curLocationFilter = -1;
                _DataView_AvailablePubs.RowFilter = "";
            }
            else
            {
                _curLocationFilter = (int)_cboLocationFilter.SelectedValue;
                _DataView_AvailablePubs.RowFilter = "PubLoc_ID = " + _cboLocationFilter.SelectedValue.ToString();
            }
        }

        private void _cboGroup_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_cboGroup.SelectedValue == null)
                _cboEffectiveDate.DataSource = null;
            else
            {
                _DataView_PubGroupEffectiveDate.Table = _dsPublications.pub_pubgroup.DefaultView.Table;
                _DataView_PubGroupEffectiveDate.RowFilter = "description = '" + _cboGroup.SelectedValue.ToString().Replace("'", "''") + "'";
                _DataView_PubGroupEffectiveDate.Sort = "effectivedate desc";
                _cboEffectiveDate.DataSource = _DataView_PubGroupEffectiveDate;
                _cboEffectiveDate.DisplayMember = "effectivedate";
                _cboEffectiveDate.ValueMember = "effectivedate";


                if (_curPubGroupEffectiveDate == null && _cboEffectiveDate.SelectedItem != null)
                    _curPubGroupEffectiveDate = (DateTime) _cboEffectiveDate.SelectedValue;
            }
        }

        private void _btnAdd_Click(object sender, EventArgs e)
        {
            if (_gridAvailablePubs.SelectedRows.Count > 0)
                this.Dirty = true;

            System.Collections.Hashtable htSelection = new System.Collections.Hashtable();
            foreach (DataGridViewRow r in _gridAvailablePubs.SelectedRows)
                htSelection.Add(((Publications.pub_pubrate_mapRow)((DataRowView)r.DataBoundItem).Row).pub_pubrate_map_id, true);

            for (int i = _DataView_AvailablePubs.Table.Rows.Count; i > 0; --i)
            {
                Publications.pub_pubrate_mapRow r = (Publications.pub_pubrate_mapRow)_DataView_AvailablePubs.Table.Rows[i - 1];
                if (htSelection.ContainsKey(r.pub_pubrate_map_id))
                {
                    _DataView_PubsInGroup.Table.Rows.Add(r.ItemArray);

                    Publications.pub_pubpubgroup_mapRow ds_row = _dsPublications.pub_pubpubgroup_map.Newpub_pubpubgroup_mapRow();
                    ds_row.pub_pubrate_map_id = r.pub_pubrate_map_id;
                    ds_row.pub_pubgroup_id = _curPubGroup.pub_pubgroup_id;
                    ds_row.createdby = MainForm.AuthorizedUser.FormattedName;
                    ds_row.createddate = DateTime.Now;
                    _dsPublications.pub_pubpubgroup_map.Addpub_pubpubgroup_mapRow(ds_row);

                    _DataView_AvailablePubs.Table.Rows.Remove((DataRow)r);
                }
            }
        }

        private void _btnAddAll_Click(object sender, EventArgs e)
        {
            if (_DataView_AvailablePubs.Table.Rows.Count > 0)
                this.Dirty = true;

            for (int i = _DataView_AvailablePubs.Table.Rows.Count; i > 0; --i)
            {
                Publications.pub_pubrate_mapRow r = (Publications.pub_pubrate_mapRow)_DataView_AvailablePubs.Table.Rows[i - 1];
                if (_cboLocationFilter.SelectedIndex < 1 || ((int) _cboLocationFilter.SelectedValue == r.publoc_id))
                {
                    _DataView_PubsInGroup.Table.Rows.Add(r.ItemArray);

                    Publications.pub_pubpubgroup_mapRow ds_row = _dsPublications.pub_pubpubgroup_map.Newpub_pubpubgroup_mapRow();
                    ds_row.pub_pubrate_map_id = r.pub_pubrate_map_id;
                    ds_row.pub_pubgroup_id = _curPubGroup.pub_pubgroup_id;
                    ds_row.createdby = MainForm.AuthorizedUser.FormattedName;
                    ds_row.createddate = DateTime.Now;
                    _dsPublications.pub_pubpubgroup_map.Addpub_pubpubgroup_mapRow(ds_row);

                    _DataView_AvailablePubs.Table.Rows.Remove((DataRow)r);
                }
            }
        }

        private void _btnRemove_Click(object sender, EventArgs e)
        {
            if (_gridPubsInGroup.SelectedRows.Count > 0)
                this.Dirty = true;

            System.Collections.Hashtable htSelection = new System.Collections.Hashtable();
            foreach (DataGridViewRow r in _gridPubsInGroup.SelectedRows)
                htSelection.Add(((Publications.pub_pubrate_mapRow)((DataRowView)r.DataBoundItem).Row).pub_pubrate_map_id, true);

            for (int i = _DataView_PubsInGroup.Table.Rows.Count; i > 0; --i)
            {
                Publications.pub_pubrate_mapRow r = (Publications.pub_pubrate_mapRow)_DataView_PubsInGroup.Table.Rows[i - 1];
                if (htSelection.ContainsKey(r.pub_pubrate_map_id))
                {
                    _DataView_AvailablePubs.Table.Rows.Add(r.ItemArray);
                    _dsPublications.pub_pubpubgroup_map.FindBypub_pubrate_map_idpub_pubgroup_id(r.pub_pubrate_map_id, _curPubGroup.pub_pubgroup_id).Delete();
                    _DataView_PubsInGroup.Table.Rows.Remove((DataRow)r); 
                }
            }
        }

        private void _btnRemoveAll_Click(object sender, EventArgs e)
        {
            if (_DataView_PubsInGroup.Table.Rows.Count > 0)
                this.Dirty = true;

            for (int i = _DataView_PubsInGroup.Table.Rows.Count; i > 0; --i)
            {
                Publications.pub_pubrate_mapRow r = (Publications.pub_pubrate_mapRow)_DataView_PubsInGroup.Table.Rows[i - 1];

                _DataView_AvailablePubs.Table.Rows.Add(r.ItemArray);

                _dsPublications.pub_pubpubgroup_map.FindBypub_pubrate_map_idpub_pubgroup_id(r.pub_pubrate_map_id, _curPubGroup.pub_pubgroup_id).Delete();
                _DataView_PubsInGroup.Table.Rows.Remove((DataRow)r);
            }
        }

        private void _chkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (!IsLoading)
                this.Dirty = true;
        }

        private void _txtDescription_TextChanged(object sender, EventArgs e)
        {
            if (!IsLoading)
                this.Dirty = true;
        }

        private void _txtComments_TextChanged(object sender, EventArgs e)
        {
            if (!IsLoading)
                this.Dirty = true;
        }

        #endregion

        #region Private Methods

        private void BindControls()
        {
            DataTable distinctPubGroups = _dsPublications.pub_pubgroup.DefaultView.ToTable(true, new string[] { "Description", "SortOrder" });
            distinctPubGroups.DefaultView.Sort = "sortorder";
            _cboGroup.DataSource = distinctPubGroups;
            _cboGroup.DisplayMember = "Description";
            _cboGroup.ValueMember = "Description";
            _cboGroup.SelectedIndex = -1;

            if (_curPubGroupDescription == "" && _cboGroup.SelectedItem != null)
                _curPubGroupDescription = _cboGroup.SelectedValue.ToString();

            DataView distinctPubLocations = new DataView();
            distinctPubLocations.Table = _dsPublications.pub_pubrate_map.DefaultView.ToTable(true, new string[] { "PubLoc_ID" });
            distinctPubLocations.Table.Columns[0].AllowDBNull = true;
            distinctPubLocations.Table.Rows.Add(new object[] { DBNull.Value });
            distinctPubLocations.Sort = "PubLoc_ID";

            _cboLocationFilter.DataSource = distinctPubLocations;
            _cboLocationFilter.DisplayMember = "PubLoc_ID";
            _cboLocationFilter.ValueMember = "PubLoc_ID";
            _curLocationFilter = 5;
            BindLocationFilter();


            _curPubGroup = null;
            if (_curPubGroupDescription != "")
            {
                _cboGroup.SelectedValue = _curPubGroupDescription;

                foreach (object li in _cboEffectiveDate.Items)
                {

                    Publications.pub_pubgroupRow r = (Publications.pub_pubgroupRow)((DataRowView)li).Row;
                    if (r.effectivedate == _curPubGroupEffectiveDate)
                    {
                        _curPubGroup = r;
                        break;
                    }
                }
            }
        }

        private void BindLocationFilter()
        {
            if (_curLocationFilter == -1)
            {
                _cboLocationFilter.SelectedValue = DBNull.Value;
                _DataView_AvailablePubs.RowFilter = "";
            }
            else
            {
                _cboLocationFilter.SelectedValue = _curLocationFilter;
                // Still have to check for SelectedValue == null in case the filter does not exist
                if (_cboLocationFilter.SelectedValue == null)
                    _DataView_AvailablePubs.RowFilter = "";
                else
                    _DataView_AvailablePubs.RowFilter = "PubLoc_ID = " + _cboLocationFilter.SelectedValue.ToString();
            }
        }

        private void ClearDetails()
        {
            _txtDescription.Text = String.Empty;
            _txtComments.Text = String.Empty;
            _dtEffectiveDateMapping.Value = DateTime.Today;
            _chkActive.Checked = false;
            _gridAvailablePubs.DataSource = null;
            _gridPubsInGroup.DataSource = null;
        }

        private void InitDetails()
        {
            _btnClear.Enabled = false;

            if (_curPubGroup == null)
            {
                _lblGroupInformation.Text = String.Empty;
                _txtDescription.Text = String.Empty;
                _txtComments.Text = String.Empty;
                _dtEffectiveDateMapping.Value = DateTime.Today;
                _chkActive.Checked = true;
                LockDetails();
            }
            else
            {
                _lblGroupInformation.Text = "Publication Group: " + _curPubGroup.description + " Effective Date: " + _curPubGroup.effectivedate.ToShortDateString();

                _txtDescription.Text = _curPubGroup.description;
                _txtComments.Text = _curPubGroup.comments;
                _dtEffectiveDateMapping.Value = _curPubGroup.effectivedate;
                _chkActive.Checked = _curPubGroup.active;

                UnlockDetails();
                _txtDescription.ReadOnly = true;
                _dtEffectiveDateMapping.Enabled = false;
                _txtComments.Focus();
            }
        }

        private void InitGrids()
        {
            /*Do a deep copy of the pub locations.*/
            _DataView_AvailablePubs.Table = _dsPublications.pub_pubrate_map.Copy();
            if (_curLocationFilter != -1)
                _DataView_AvailablePubs.RowFilter = "PubLoc_ID = " + _curLocationFilter.ToString();

            _DataView_PubsInGroup.Table = new Publications.pub_pubrate_mapDataTable();
            _DataView_AvailablePubs.Sort = "Pub_ID, PubLoc_ID";
            _DataView_PubsInGroup.Sort = "Pub_ID, PubLoc_ID";

            if (_curPubGroup != null)
            {
                /*Move pubs already in group to PubsInGroup*/
                System.Collections.Hashtable htPubsInGroup = new System.Collections.Hashtable();
                _dsPublications.pub_pubpubgroup_map.DefaultView.RowFilter = "PUB_PubGroup_ID = " + _curPubGroup.pub_pubgroup_id.ToString();
                for (int i = 0; i < _dsPublications.pub_pubpubgroup_map.DefaultView.Count; ++i)
                    htPubsInGroup.Add(((Publications.pub_pubpubgroup_mapRow)_dsPublications.pub_pubpubgroup_map.DefaultView[i].Row).pub_pubrate_map_id, true);

                for (int i = _DataView_AvailablePubs.Table.Rows.Count; i > 0; --i)
                {
                    Publications.pub_pubrate_mapRow r = (Publications.pub_pubrate_mapRow)_DataView_AvailablePubs.Table.Rows[i - 1];
                    if (htPubsInGroup.ContainsKey(r.pub_pubrate_map_id))
                    {
                        _DataView_PubsInGroup.Table.Rows.Add(r.ItemArray);
                        _DataView_AvailablePubs.Table.Rows.Remove((DataRow)r);
                    }
                }
            }

            /*Bind the Grids*/
            _gridAvailablePubs.DataSource = _DataView_AvailablePubs;
            _gridPubsInGroup.DataSource = _DataView_PubsInGroup;

            _gridAvailablePubs.CurrentCell = null;
            _gridPubsInGroup.CurrentCell = null;
        }

        private void InitControl()
        {
            if (MainForm.AuthorizedUser.Right == UserRights.SuperAdmin || MainForm.AuthorizedUser.Right == UserRights.Admin)
            {
                _btnNew.Enabled = true;
            }
        }

        private void WriteToDataset()
        {
            if (_curPubGroup == null)
                return;

            _curPubGroup.description = _txtDescription.Text.Trim();
            _curPubGroup.comments = _txtComments.Text.Trim();
            _curPubGroup.effectivedate = _dtEffectiveDateMapping.Value.Date;
            _curPubGroup.active = _chkActive.Checked;

            Publications.pub_pubrate_mapDataTable PubsSelected = (Publications.pub_pubrate_mapDataTable)_DataView_PubsInGroup.Table;
            DataView PubsInCurrentGroup = new DataView();

            PubsInCurrentGroup.Table = _dsPublications.pub_pubpubgroup_map;
            PubsInCurrentGroup.RowFilter = "pub_pubgroup_id = " + _curPubGroup.pub_pubgroup_id.ToString();
            foreach (DataRowView r in PubsInCurrentGroup)
            {
                Publications.pub_pubpubgroup_mapRow pgm = (Publications.pub_pubpubgroup_mapRow)r.Row;
                if (PubsSelected.FindBypub_pubrate_map_id(pgm.pub_pubrate_map_id) == null)
                {
                    pgm.Delete();
                }
            }

            foreach (Publications.pub_pubrate_mapRow r in PubsSelected)
            {
                if (((Publications.pub_pubpubgroup_mapDataTable)PubsInCurrentGroup.Table).FindBypub_pubrate_map_idpub_pubgroup_id(r.pub_pubrate_map_id, _curPubGroup.pub_pubgroup_id) == null)
                {
                    Publications.pub_pubpubgroup_mapRow new_row = _dsPublications.pub_pubpubgroup_map.Newpub_pubpubgroup_mapRow();
                    new_row.pub_pubgroup_id = _curPubGroup.pub_pubgroup_id;
                    new_row.pub_pubrate_map_id = r.pub_pubrate_map_id;
                    new_row.createdby = MainForm.AuthorizedUser.FormattedName;
                    new_row.createddate = DateTime.Now;
                    _dsPublications.pub_pubpubgroup_map.Addpub_pubpubgroup_mapRow(new_row);
                }
            }

            if (_curPubGroup.RowState == DataRowState.Detached)
                _dsPublications.pub_pubgroup.Addpub_pubgroupRow(_curPubGroup);
            else
            {
                _curPubGroup.modifiedby = MainForm.AuthorizedUser.FormattedName;
                _curPubGroup.modifieddate = DateTime.Now;
            }

            _curPubGroupDescription = _curPubGroup.description;
            _curPubGroupEffectiveDate = _curPubGroup.effectivedate;
        }

        private void UnlockEffectiveDate()
        {
            if (MainForm.AuthorizedUser.Right == UserRights.SuperAdmin || MainForm.AuthorizedUser.Right == UserRights.Admin)
                _dtEffectiveDateMapping.Enabled = true;
        }

        private void UnlockDetails()
        {
            if (MainForm.AuthorizedUser.Right == UserRights.SuperAdmin || MainForm.AuthorizedUser.Right == UserRights.Admin)
            {
                _txtDescription.ReadOnly = false;
                _chkActive.Enabled = true;
                _txtComments.ReadOnly = false;
                _cboLocationFilter.Enabled = true;
                _btnAdd.Enabled = true;
                _btnAddAll.Enabled = true;
                _btnRemove.Enabled = true;
                _btnRemoveAll.Enabled = true;
            }
        }

        private void LockDetails()
        {
            _txtDescription.ReadOnly = true;
            _dtEffectiveDateMapping.Enabled = false;
            _chkActive.Enabled = false;
            _txtComments.ReadOnly = true;
            _cboLocationFilter.Enabled = false;
            _btnAdd.Enabled = false;
            _btnAddAll.Enabled = false;
            _btnRemove.Enabled = false;
            _btnRemoveAll.Enabled = false;
        }

        #endregion

        private void _btnClear_Click(object sender, EventArgs e)
        {

            _txtDescription.Text = String.Empty;
            _txtComments.Text = String.Empty;
            _chkActive.Checked = true;
            _dtEffectiveDateMapping.Value = DateTime.Today;
            _btnRemoveAll_Click(sender, e);

            // Reset the Location Filter to 5
            _curLocationFilter = 5;
            BindLocationFilter();

            _lblGroupInformation.Text = "New Publication Group";

            _btnClear.Enabled = false;
        }
    }
}