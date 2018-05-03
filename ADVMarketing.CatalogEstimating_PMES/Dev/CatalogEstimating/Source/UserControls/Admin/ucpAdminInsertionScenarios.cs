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
    public partial class ucpAdminInsertionScenarios : CatalogEstimating.UserControlPanel
    {
        private enum CtlState
        {
            ReadOnly = 1,
            ReadOnly_AllowNew = 2,
            New = 3,
            EditExisting_Clean = 4,
            EditExisting_Dirty = 5
        }
        private CtlState _ctlState = CtlState.ReadOnly;

        private string _ScenarioDescription = "";

        private Publications.pub_insertscenarioRow _curInsertScenario = null;
        private DataView _DataView_AvailableGroups = new DataView();
        private DataView _DataView_GroupsInScenario = new DataView();

        public ucpAdminInsertionScenarios(Publications ds)
        {
            InitializeComponent();
            Name = "Scenarios";

            _dsPublications = ds;
        }

        #region Overrides

        public override void LoadData()
        {
            IsLoading = true;

            _curInsertScenario = null;
            _lstScenarios.SelectedItem = null;

            if (_ScenarioDescription != "")
            {
                foreach (object li in _lstScenarios.Items)
                {
                    Publications.pub_insertscenarioRow r = (Publications.pub_insertscenarioRow)((DataRowView)li).Row;
                    if (r.description == _ScenarioDescription)
                    {
                        _curInsertScenario = r;
                        break;
                    }
                }

                if (_curInsertScenario == null)
                    SetControlState(CtlState.ReadOnly_AllowNew);
                else
                    SetControlState(CtlState.EditExisting_Clean);
            }
            else if (_ctlState != CtlState.ReadOnly)
                SetControlState(CtlState.ReadOnly_AllowNew);

            InitScenario();

            if (_curInsertScenario == null)
                _lblScenarioInfo.Text = String.Empty;
            else
                _lblScenarioInfo.Text = "Insert Scenario: " + _curInsertScenario.description;

            base.LoadData();

            IsLoading = false;
        }

        public override void Reload()
        {
            IsLoading = true;

            _dsPublications.pub_insertscenario.DefaultView.Sort = "description";
            _lstScenarios.DataSource = _dsPublications.pub_insertscenario;
            _lstScenarios.DisplayMember = "description";
            _lstScenarios.ValueMember = "pub_insertscenario_id";

            _curInsertScenario = null;
            _lstScenarios.SelectedItem = null;

            if (_ScenarioDescription != "")
            {
                foreach (object li in _lstScenarios.Items)
                {
                    Publications.pub_insertscenarioRow r = (Publications.pub_insertscenarioRow) ((DataRowView)li).Row;
                    if (r.description == _ScenarioDescription)
                    {
                        _curInsertScenario = r;
                        _lstScenarios.SelectedValue = r.pub_insertscenario_id;
                        break;
                    }
                }

                SetControlState(CtlState.EditExisting_Clean);
            }

            if (_curInsertScenario == null)
                _lblScenarioInfo.Text = String.Empty;
            else
                _lblScenarioInfo.Text = "Insert Scenario: " + _curInsertScenario.description;

            base.Reload();

            IsLoading = false;
        }

        public override ToolStrip Toolbar
        {
            get { return _toolStrip; }
        }

        public override void PreSave(CancelEventArgs e)
        {
            if (!ValidateControl())
                e.Cancel = true;
            else
                WriteToDataSet();
        }

        public override void OnLeaving(CancelEventArgs e)
        {
            if (!ValidateControl())
            {
                DialogResult result = MessageBox.Show(Properties.Resources.InsertScenarioChangesLine1 + System.Environment.NewLine + Properties.Resources.InsertScenarioChangesLine2, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                    RejectChangesToInsertScenario();
                else
                    e.Cancel = true;
            }
            else
            {
                WriteToDataSet();
            }
        }

        public override void Export(ref ExcelWriter writer)
        {
            writer.WriteLine("Admin Insert Scenarios");
            writer.WriteLine(_lblScenarios.Text);
            foreach (DataRowView r in _lstScenarios.Items)
                writer.WriteLine(((Publications.pub_insertscenarioRow)r.Row).description);
            writer.WriteLine(_groupMapping.Text);
            writer.WriteLine(_lblDescription.Text, _txtDescription.Text, _chkActive.Checked.ToString(), _chkActive.Text);
            writer.WriteLine(_lblComments.Text, _txtComments.Text);
            writer.WriteLine(_lblAvailableGroups.Text);
            foreach (DataRowView r in _lstAvailableGroups.Items)
                writer.WriteLine(((Publications.pub_pubgroupRow)r.Row).description);
            writer.WriteLine(_lblGroupsInScenario.Text);
            foreach (DataRowView r in _lstGroupsInScenario.Items)
                writer.WriteLine(((Publications.pub_pubgroupRow)r.Row).description);

        }
        #endregion

        #region Event Handlers

        private void _lstScenarios_SelectedValueChanged(object sender, EventArgs e)
        {
            if (IsLoading)
                return;

            if (_lstScenarios.SelectedItem == null)
                return;

            IsLoading = true;

            // Get the Insert Scenario Record
            _curInsertScenario = (Publications.pub_insertscenarioRow)((DataRowView)_lstScenarios.SelectedItem).Row;
            _lblScenarioInfo.Text = "Insert Scenario: " + _curInsertScenario.description;
            _ScenarioDescription = _curInsertScenario.description;
            SetControlState(CtlState.EditExisting_Clean);
            InitScenario();

            IsLoading = false;

        }

        private void _btnAdd_Click(object sender, EventArgs e)
        {
            if (_lstAvailableGroups.SelectedItems.Count > 0)
                this.Dirty = true;

            System.Collections.Hashtable htSelection = new System.Collections.Hashtable();
            foreach (DataRowView r in _lstAvailableGroups.SelectedItems)
                htSelection.Add(((Publications.pub_pubgroupRow)r.Row).description, true);

            for (int i = _DataView_AvailableGroups.Table.Rows.Count; i > 0; --i)
            {
                Publications.pub_pubgroupRow r = (Publications.pub_pubgroupRow)_DataView_AvailableGroups.Table.Rows[i - 1];
                if (htSelection.ContainsKey(r.description))
                {
                    _DataView_GroupsInScenario.Table.Rows.Add(r.ItemArray);

                    Publications.pub_groupinsertscenario_mapRow ds_row = _dsPublications.pub_groupinsertscenario_map.Newpub_groupinsertscenario_mapRow();
                    ds_row.pubgroupdescription = r.description;
                    ds_row.pub_insertscenario_id = _curInsertScenario.pub_insertscenario_id;
                    ds_row.createdby = MainForm.AuthorizedUser.FormattedName;
                    ds_row.createddate = DateTime.Now;
                    _dsPublications.pub_groupinsertscenario_map.Addpub_groupinsertscenario_mapRow(ds_row);

                    _DataView_AvailableGroups.Table.Rows.Remove((DataRow)r);
                }
            }
        }

        private void _btnAddAll_Click(object sender, EventArgs e)
        {
            if (_DataView_AvailableGroups.Table.Rows.Count > 0)
                this.Dirty = true;

            for (int i = _DataView_AvailableGroups.Table.Rows.Count; i > 0; --i)
            {
                Publications.pub_pubgroupRow r = (Publications.pub_pubgroupRow)_DataView_AvailableGroups.Table.Rows[i - 1];
                _DataView_GroupsInScenario.Table.Rows.Add(r.ItemArray);

                Publications.pub_groupinsertscenario_mapRow ds_row = _dsPublications.pub_groupinsertscenario_map.Newpub_groupinsertscenario_mapRow();
                ds_row.pubgroupdescription = r.description;
                ds_row.pub_insertscenario_id = _curInsertScenario.pub_insertscenario_id;
                ds_row.createdby = MainForm.AuthorizedUser.FormattedName;
                ds_row.createddate = DateTime.Now;
                _dsPublications.pub_groupinsertscenario_map.Addpub_groupinsertscenario_mapRow(ds_row);

                _DataView_AvailableGroups.Table.Rows.Remove((DataRow)r);
            }
        }

        private void _btnRemove_Click(object sender, EventArgs e)
        {
            if (_lstGroupsInScenario.SelectedItems.Count > 0)
                this.Dirty = true;

            System.Collections.Hashtable htSelection = new System.Collections.Hashtable();
            foreach (DataRowView r in _lstGroupsInScenario.SelectedItems)
                htSelection.Add(((Publications.pub_pubgroupRow)r.Row).description, true);

            for (int i = _DataView_GroupsInScenario.Table.Rows.Count; i > 0; --i)
            {
                Publications.pub_pubgroupRow r = (Publications.pub_pubgroupRow)_DataView_GroupsInScenario.Table.Rows[i - 1];
                if (htSelection.ContainsKey(r.description))
                {
                    _DataView_AvailableGroups.Table.Rows.Add(r.ItemArray);
                    _dsPublications.pub_groupinsertscenario_map.FindBypub_insertscenario_idpubgroupdescription(_curInsertScenario.pub_insertscenario_id, r.description).Delete();
                    _DataView_GroupsInScenario.Table.Rows.Remove((DataRow)r);
                }
            }
        }

        private void _btnRemoveAll_Click(object sender, EventArgs e)
        {
            if (_DataView_GroupsInScenario.Table.Rows.Count > 0)
                this.Dirty = true;

            for (int i = _DataView_GroupsInScenario.Table.Rows.Count; i > 0; --i)
            {
                Publications.pub_pubgroupRow r = (Publications.pub_pubgroupRow)_DataView_GroupsInScenario.Table.Rows[i - 1];

                _DataView_AvailableGroups.Table.Rows.Add(r.ItemArray);
                _dsPublications.pub_groupinsertscenario_map.FindBypub_insertscenario_idpubgroupdescription(_curInsertScenario.pub_insertscenario_id, r.description).Delete();
                _DataView_GroupsInScenario.Table.Rows.Remove((DataRow)r);
            }
        }

        private void _btnNew_Click(object sender, EventArgs e)
        {
            IsLoading = true;

            // If there is a record open, write it back to the dataset
            if (_curInsertScenario != null)
            {
                if (ValidateControl())
                {
                    _lblScenarioInfo.Text = "Copied from Insert Scenario: " + _curInsertScenario.description;
                    WriteToDataSet();
                }
                else
                {
                    DialogResult result = MessageBox.Show(Properties.Resources.NewRecordWarningLine1 + System.Environment.NewLine + Properties.Resources.NewRecordWarningLine2, Properties.Resources.NewRecordWarningCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        foreach (DataRow dr in _dsPublications.pub_groupinsertscenario_map.Select("pub_insertscenario_id = " + _curInsertScenario.pub_insertscenario_id.ToString()))
                            dr.RejectChanges();
                        _curInsertScenario.RejectChanges();
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                _lblScenarioInfo.Text = "New Insert Scenario";
            }

            _errorProvider.Clear();

            // Initialize the controls

            // Instantiate a new Insert Scenario record.
            _curInsertScenario = _dsPublications.pub_insertscenario.Newpub_insertscenarioRow();
            _curInsertScenario.description = _txtDescription.Text.Trim();
            _curInsertScenario.comments = _txtComments.Text.Trim();
            _curInsertScenario.createdby = MainForm.AuthorizedUser.FormattedName;
            _curInsertScenario.createddate = DateTime.Now;
            _curInsertScenario.active = _chkActive.Checked;
            _dsPublications.pub_insertscenario.Addpub_insertscenarioRow(_curInsertScenario);

            // If the lists have not been populated, do a deep copy.
            if (_DataView_AvailableGroups.Table == null)
            {
                DataView tmpPubGroups = new DataView();
                tmpPubGroups.Table = _dsPublications.pub_pubgroup.Copy();
                tmpPubGroups.RowFilter = "CustomGroupForPackage = false";
                tmpPubGroups.Sort = "Description";

                _DataView_AvailableGroups.Table = new Publications.pub_pubgroupDataTable();
                string prevDescription = null;
                foreach (DataRowView r in tmpPubGroups)
                {
                    Publications.pub_pubgroupRow pg_row = (Publications.pub_pubgroupRow)r.Row;
                    if (prevDescription == null || pg_row.description != prevDescription)
                    {
                        prevDescription = pg_row.description;
                        _DataView_AvailableGroups.Table.Rows.Add(pg_row.ItemArray);
                    }
                }

                _DataView_GroupsInScenario.Table = new Publications.pub_pubgroupDataTable();
            }

            // Add each Pub Group to the Map
            System.Collections.Hashtable htGroupsInScenario = new System.Collections.Hashtable();
            for (int i = 0; i < _DataView_GroupsInScenario.Count; ++i)
            {
                Publications.pub_pubgroupRow r = (Publications.pub_pubgroupRow)_DataView_GroupsInScenario[i].Row;
                Publications.pub_groupinsertscenario_mapRow ds_row = _dsPublications.pub_groupinsertscenario_map.Newpub_groupinsertscenario_mapRow();
                ds_row.pub_insertscenario_id = _curInsertScenario.pub_insertscenario_id;
                ds_row.pubgroupdescription = r.description;
                ds_row.createdby = MainForm.AuthorizedUser.FormattedName;
                ds_row.createddate = DateTime.Now;
                _dsPublications.pub_groupinsertscenario_map.Addpub_groupinsertscenario_mapRow(ds_row);
            }

            _lstAvailableGroups.DataSource = _DataView_AvailableGroups;
            _lstAvailableGroups.DisplayMember = "description";
            _lstAvailableGroups.ValueMember = "pub_pubgroup_id";
            _lstAvailableGroups.SelectedItems.Clear();
            _lstGroupsInScenario.DataSource = _DataView_GroupsInScenario;
            _lstGroupsInScenario.DisplayMember = "description";
            _lstGroupsInScenario.ValueMember = "pub_pubgroup_id";
            _lstGroupsInScenario.SelectedItems.Clear();

            IsLoading = false;
            SetControlState(CtlState.New);
        }

        private void ucpAdminInsertionScenarios_Load(object sender, EventArgs e)
        {
            if (MainForm.AuthorizedUser.Right == UserRights.SuperAdmin || MainForm.AuthorizedUser.Right == UserRights.Admin)
            {
                _btnNew.Enabled = true;
            }
            else
            {
                _btnNew.Enabled = false;
                _btnDelete.Enabled = false;
            }
        }

        private void Control_Changed(object sender, EventArgs e)
        {
            if (!IsLoading)
                if (_ctlState == CtlState.EditExisting_Clean)
                    SetControlState(CtlState.EditExisting_Dirty);
        }

        private void _btnDelete_Click(object sender, EventArgs e)
        {
            if (_ctlState == CtlState.ReadOnly || _ctlState == CtlState.ReadOnly_AllowNew)
            {
                MessageBox.Show("Please select an Insert Scenario to delete.", "Select an Insert Scenario", MessageBoxButtons.OK);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                _curInsertScenario.Delete();
                SetControlState(CtlState.ReadOnly_AllowNew);
                this.Dirty = true;
            }
        }

        #endregion

        #region Private Methods

        private void InitScenario()
        {
            if (_ctlState == CtlState.ReadOnly || _ctlState == CtlState.ReadOnly_AllowNew)
            {
                _txtDescription.Text = String.Empty;
                _txtComments.Text = String.Empty;
                _chkActive.Checked = true;
            }

            else
            {
                // Set the Controls
                _txtDescription.Text = _curInsertScenario.description;
                if (_curInsertScenario.IscommentsNull())
                    _txtComments.Text = String.Empty;
                else
                    _txtComments.Text = _curInsertScenario.comments;
                _chkActive.Checked = _curInsertScenario.active;

                // Populate the Lists
                /*Do a deep copy of the pub groups.*/
                DataView tmpPubGroups = new DataView();
                tmpPubGroups.Table = _dsPublications.pub_pubgroup.Copy();
                tmpPubGroups.RowFilter = "CustomGroupForPackage = false";
                tmpPubGroups.Sort = "Description";

                _DataView_AvailableGroups.Table = new Publications.pub_pubgroupDataTable();
                string prevDescription = "";
                foreach (DataRowView r in tmpPubGroups)
                {
                    Publications.pub_pubgroupRow pg_row = (Publications.pub_pubgroupRow)r.Row;
                    if (pg_row.description != prevDescription)
                    {
                        prevDescription = pg_row.description;
                        _DataView_AvailableGroups.Table.Rows.Add(pg_row.ItemArray);
                    }
                }

                _DataView_GroupsInScenario.Table = new Publications.pub_pubgroupDataTable();

                /*Move groups already in scenario to GroupsInScenario*/
                System.Collections.Hashtable htGroupsInScenario = new System.Collections.Hashtable();
                _dsPublications.pub_groupinsertscenario_map.DefaultView.RowFilter = "PUB_InsertScenario_ID = " + _curInsertScenario.pub_insertscenario_id.ToString();
                for (int i = 0; i < _dsPublications.pub_groupinsertscenario_map.DefaultView.Count; ++i)
                    htGroupsInScenario.Add(((Publications.pub_groupinsertscenario_mapRow)_dsPublications.pub_groupinsertscenario_map.DefaultView[i].Row).pubgroupdescription, true);

                for (int i = _DataView_AvailableGroups.Table.Rows.Count; i > 0; --i)
                {
                    Publications.pub_pubgroupRow r = (Publications.pub_pubgroupRow)_DataView_AvailableGroups.Table.Rows[i - 1];
                    if (htGroupsInScenario.ContainsKey(r.description))
                    {
                        _DataView_GroupsInScenario.Table.Rows.Add(r.ItemArray);
                        _DataView_AvailableGroups.Table.Rows.Remove((DataRow)r);
                    }
                }

                /*Bind the Lists*/
                _lstAvailableGroups.DataSource = _DataView_AvailableGroups;
                _lstAvailableGroups.DisplayMember = "description";
                _lstAvailableGroups.ValueMember = "pub_pubgroup_id";

                _lstGroupsInScenario.DataSource = _DataView_GroupsInScenario;
                _lstGroupsInScenario.DisplayMember = "description";
                _lstGroupsInScenario.ValueMember = "pub_pubgroup_id";

                // Ensure that no items are selected by default in either list
                _lstAvailableGroups.SelectedItems.Clear();
                _lstGroupsInScenario.SelectedItems.Clear();
            }
        }

        private bool ValidateControl()
        {
            _errorProvider.Clear();
            bool isValid = true;

            if (_ctlState == CtlState.New || _ctlState == CtlState.EditExisting_Dirty)
            {
                if (_txtDescription.Text.Trim() == String.Empty)
                {
                    _errorProvider.SetError(_txtDescription, Properties.Resources.RequiredFieldError);
                    isValid = false;
                }

                if (_dsPublications.pub_insertscenario.Select("description = '" + _txtDescription.Text.Trim().Replace("'", "''") + "' and pub_insertscenario_id <> " + _curInsertScenario.pub_insertscenario_id.ToString()).Length > 0)
                {
                    _errorProvider.SetError(_txtDescription, "An insert scenario already exists with the specified description");
                    isValid = false;
                }

                if (_DataView_GroupsInScenario.Count < 2)
                {
                    _errorProvider.SetError(_lblGroupsInScenario, "A scenario must contain at least two groups");
                    isValid = false;
                }
            }
            return isValid;
        }

        private void RejectChangesToInsertScenario()
        {
            if (_ctlState == CtlState.New || _ctlState == CtlState.EditExisting_Dirty)
            {
                foreach (DataRow dr in _dsPublications.pub_groupinsertscenario_map.Select("pub_insertscenario_id = " + _curInsertScenario.pub_insertscenario_id.ToString()))
                    dr.RejectChanges();

                _curInsertScenario.RejectChanges();
            }

            SetControlState(CtlState.ReadOnly_AllowNew);
        }

        private void ClearControl()
        {
            _txtDescription.Text = String.Empty;
            _txtComments.Text = String.Empty;
            _chkActive.Checked = true;
            _DataView_AvailableGroups.Table.Clear();
            _DataView_GroupsInScenario.Table.Clear();
            _errorProvider.Clear();
        }

        private void SetControlState(CtlState state)
        {
            if ((MainForm.AuthorizedUser.Right != UserRights.Admin) &&
                 (MainForm.AuthorizedUser.Right != UserRights.SuperAdmin))
            {
                state = CtlState.ReadOnly;
                return;
            }

            _ctlState = state;
            if (state == CtlState.ReadOnly)
            {
                _ScenarioDescription = String.Empty;
                _curInsertScenario = null;
                _btnNew.Enabled = false;
                _btnDelete.Enabled = false;
                _txtDescription.ReadOnly = true;
                _txtComments.ReadOnly = true;
                _chkActive.Enabled = false;
                _btnAdd.Enabled = false;
                _btnAddAll.Enabled = false;
                _btnRemove.Enabled = false;
                _btnRemoveAll.Enabled = false;
                _btnClear.Enabled = false;
                ClearControl();
            }

            else if (state == CtlState.ReadOnly_AllowNew)
            {
                _ScenarioDescription = String.Empty;
                _curInsertScenario = null;
                _btnNew.Enabled = true;
                _btnDelete.Enabled = false;
                _txtDescription.ReadOnly = true;
                _txtComments.ReadOnly = true;
                _chkActive.Enabled = false;
                _btnAdd.Enabled = false;
                _btnAddAll.Enabled = false;
                _btnRemove.Enabled = false;
                _btnRemoveAll.Enabled = false;
                _btnClear.Enabled = false;
                ClearControl();
            }

            else if (state == CtlState.New)
            {
                this.Dirty = true;
                _btnNew.Enabled = true;
                _btnDelete.Enabled = true;
                _txtDescription.ReadOnly = false;
                _txtComments.ReadOnly = false;
                _chkActive.Enabled = true;
                _btnAdd.Enabled = true;
                _btnAddAll.Enabled = true;
                _btnRemove.Enabled = true;
                _btnRemoveAll.Enabled = true;
                _btnClear.Enabled = true;
                _txtDescription.Focus();
                _txtDescription.SelectionStart = 0;
                _txtDescription.SelectionLength = _txtDescription.Text.Length;
            }
            else if (state == CtlState.EditExisting_Clean)
            {
                _btnNew.Enabled = true;
                _btnDelete.Enabled = true;
                _txtDescription.ReadOnly = false;
                _txtComments.ReadOnly = false;
                _chkActive.Enabled = true;
                _btnAdd.Enabled = true;
                _btnAddAll.Enabled = true;
                _btnRemove.Enabled = true;
                _btnRemoveAll.Enabled = true;
                _btnClear.Enabled = false;
                _txtDescription.Focus();
                _txtDescription.SelectionStart = 0;
                _txtDescription.SelectionLength = _txtDescription.Text.Length;
            }
            else if (state == CtlState.EditExisting_Dirty)
            {
                this.Dirty = true;
                _btnNew.Enabled = true;
                _btnDelete.Enabled = true;
                _txtDescription.ReadOnly = false;
                _txtComments.ReadOnly = false;
                _chkActive.Enabled = true;
                _btnAdd.Enabled = true;
                _btnAddAll.Enabled = true;
                _btnRemove.Enabled = true;
                _btnRemoveAll.Enabled = true;
                _btnClear.Enabled = false;
            }
        }

        private void WriteToDataSet()
        {
            bool tmpIsLoading = this.IsLoading;
            this.IsLoading = true;

            // If there is a current Insert Scenario changes may have been made.  Let's update the dataset record so that it can save to the db.
            if (_ctlState == CtlState.New || _ctlState == CtlState.EditExisting_Dirty)
            {
                _ScenarioDescription = _txtDescription.Text.Trim();
                _curInsertScenario.description = _txtDescription.Text.Trim();
                _curInsertScenario.comments = _txtComments.Text.Trim();
                _curInsertScenario.active = _chkActive.Checked;

                if (_curInsertScenario.RowState == DataRowState.Modified)
                {
                    _curInsertScenario.modifiedby = MainForm.AuthorizedUser.FormattedName;
                    _curInsertScenario.modifieddate = DateTime.Now;
                }
                else if (_curInsertScenario.RowState == DataRowState.Detached)
                    _dsPublications.pub_insertscenario.Addpub_insertscenarioRow(_curInsertScenario);

                _curInsertScenario = null;
            }

            this.IsLoading = tmpIsLoading;
        }

        #endregion

        private void _btnClear_Click(object sender, EventArgs e)
        {
            _txtDescription.Text = string.Empty;
            _txtComments.Text = string.Empty;
            _chkActive.Checked = true;
            _btnRemoveAll_Click(sender, e);
            _btnClear.Enabled = false;
            _txtDescription.Focus();
        }
    }
}