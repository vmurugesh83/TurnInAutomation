using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using CatalogEstimating.Properties;
using CatalogEstimating.CustomControls;
using CatalogEstimating.Datasets.LookupTableAdapters;

namespace CatalogEstimating.UserControls.Main
{
    public partial class ucpPolybagSearch : CatalogEstimating.UserControlPanel
    {

        #region Private Variables

        private Datasets.Lookup _dsLookup = new CatalogEstimating.Datasets.Lookup();
        private static Dictionary<int, string> fiscalMonthDictionary = new Dictionary<int, string>();

        #endregion

        #region Construction

        public ucpPolybagSearch()
        {
            InitializeComponent();
            Name = "Polybag Search";
        }

        static ucpPolybagSearch()
        {
            // Fill in the dictionaries for the lookup tables that won't change
            fiscalMonthDictionary.Add(-1, string.Empty);
            foreach ( int value in Enum.GetValues( typeof( FiscalMonths ) ) )
            {
                fiscalMonthDictionary.Add( value, Enum.GetName( typeof( FiscalMonths ), value ) );
            }
        }

        #endregion

        #region Public Overrides

        public override ToolStrip Toolbar
        {
            get { return _toolStrip; }
        }

        public override void LoadData()
        {
            _dsLookup.Clear();

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                using (est_statusTableAdapter adapter = new est_statusTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsLookup.est_status);
                }

                using (est_seasonTableAdapter adapter = new est_seasonTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsLookup.est_season);
                }

                using (EstEstimate_s_CreatedByTableAdapter adapter = new EstEstimate_s_CreatedByTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsLookup.EstEstimate_s_CreatedBy);
                }

                using (EstEstimate_s_FiscalYearTableAdapter adapter = new EstEstimate_s_FiscalYearTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsLookup.EstEstimate_s_FiscalYear);
                }

                conn.Close();
            }

            // Bind the dropdown controls
            Dictionary<int, string> seasonDictionary = new Dictionary<int,string>();
            seasonDictionary.Add(-1, string.Empty);
            foreach (Datasets.Lookup.est_seasonRow s_row in _dsLookup.est_season)
            {
                seasonDictionary.Add(s_row.est_season_id, s_row.description);
            }
            _cboSeason.DataSource = new BindingSource(seasonDictionary, null);
            _cboSeason.DisplayMember = "Value";
            _cboSeason.ValueMember = "Key";

            Dictionary<int, string> fiscalYearDictionary = new Dictionary<int,string>();
            fiscalYearDictionary.Add(-1, string.Empty);
            foreach (Datasets.Lookup.EstEstimate_s_FiscalYearRow y_row in _dsLookup.EstEstimate_s_FiscalYear)
            {
                fiscalYearDictionary.Add(y_row.fiscalyear, y_row.fiscalyear.ToString());
            }
            _cboFiscalYear.DataSource = new BindingSource(fiscalYearDictionary, null);
            _cboFiscalYear.DisplayMember = "Value";
            _cboFiscalYear.ValueMember = "Key";


            _cboFiscalMonth.DataSource = new BindingSource(fiscalMonthDictionary, null);
            _cboFiscalMonth.DisplayMember = "Value";
            _cboFiscalMonth.ValueMember = "Key";

            List<string> createdByList = new List<string>();
            createdByList.Add(string.Empty);
            foreach (Datasets.Lookup.EstEstimate_s_CreatedByRow c_row in _dsLookup.EstEstimate_s_CreatedBy)
            {
                createdByList.Add(c_row.createdby);
            }
            _cboCreatedBy.DataSource = createdByList;

            Dictionary<int, string> statusDictionary = new Dictionary<int, string>();
            statusDictionary.Add(-1, string.Empty);
            foreach (Datasets.Lookup.est_statusRow s_row in _dsLookup.est_status)
            {
                statusDictionary.Add(s_row.est_status_id, s_row.description);
            }
            _cboEstimateStatus.DataSource = new BindingSource(statusDictionary, null);
            _cboEstimateStatus.DisplayMember = "Value";
            _cboEstimateStatus.ValueMember = "Key";

            _btnReset_Click(this, EventArgs.Empty);
            EnableToolbarButtons();
        }

        public override void Export(ref ExcelWriter writer)
        {
            writer.WriteLine("Polybag Search Screen");
            writer.WriteLine(_groupFilter.Text);
            writer.WriteLine(_lblPolybagID, _txtPolybagID.Text.Trim(), _lblAdNumber.Text, _txtAdNumber.Text.Trim());
            writer.WriteLine(_lblPolybagDesc, _txtPolybagDesc.Text.Trim(), _lblRunDateRangeStart.Text, label1.Text, _dtStartRunDate.Text, label2.Text, _dtEndRunDate.Text);
            writer.WriteLine(_lblPolybagComments, _txtPolybagComments.Text.Trim(), _lblCreatedBy.Text, _cboCreatedBy.Text);
            writer.WriteLine(_lblSeason.Text, _cboSeason.Text, _lblStatus.Text, _cboEstimateStatus.Text);
            writer.WriteLine(_lblFiscalYear.Text, _cboFiscalYear.Text);
            writer.WriteLine(_lblFiscalMonth.Text, _cboFiscalMonth.Text);
            writer.WriteLine(_lblPolybags.Text);
            writer.WriteTable(_gridPolybags, true);
        }
        #endregion

        #region Private Methods

        private void EnableSearchButton()
        {
            bool bEnable = false;
            foreach (Control ctrl in _groupFilter.Controls)
            {
                TextBox text = ctrl as TextBox;
                if (text != null)
                {
                    if (text.Text.Trim().Length > 0)
                    {
                        bEnable = true;
                        break;
                    }
                    else
                        continue;
                }

                ComboBox combo = ctrl as ComboBox;
                if (combo != null)
                {
                    if (combo.SelectedIndex > 0)
                    {
                        bEnable = true;
                        break;
                    }
                    else
                        continue;
                }

                NullableDateTimePicker date = ctrl as NullableDateTimePicker;
                if (date != null)
                {
                    if (date.Value != null)
                    {
                        bEnable = true;
                        break;
                    }
                    else
                        continue;
                }
            }

            _btnSearch.Enabled = bEnable;
        }

        private void EnableToolbarButtons()
        {
            bool bEditUser = (MainForm.AuthorizedUser.Right != UserRights.ReadOnly);
            bool bHasSelected = false;

            if (_gridPolybags.SelectedRows.Count > 0)
            {
                bHasSelected = true;
            }

            _btnNewPolybag.Enabled = bEditUser;
            _btnOpenPolybag.Enabled = bHasSelected && bEditUser;
            _btnOpenPolybagReadOnly.Enabled = bHasSelected;
            _btnDelete.Enabled = bHasSelected && bEditUser;
            _menuContextNew.Enabled = bEditUser;
            _menuContextOpen.Enabled = bHasSelected && bEditUser;
            _menuContextOpenReadOnly.Enabled = bHasSelected;
            _menuContextDelete.Enabled = bHasSelected && bEditUser;
        }

        private void OpenSelectedPolybags(bool bForceReadOnly)
        {
            bool exceptionOccurred = false;

            MainForm main = (MainForm)ParentForm;

            if (MainForm.AuthorizedUser.Right == UserRights.ReadOnly)
                bForceReadOnly = true;

            foreach (DataGridViewRow gridViewRow in _gridPolybags.SelectedRows)
            {
                long polybagID = (long)gridViewRow.Cells[0].Value;

                try
                {
                    main.OpenPolybagGroup(bForceReadOnly, polybagID);
                }
                catch (CatalogEstimating.Exceptions.PolybagGroupNotExistException)
                {
                    exceptionOccurred = true;
                }
            }

            if (exceptionOccurred)
            {
                MessageBox.Show("Some of the polybag group(s) have been deleted.  You may need to perform your search again.", "Cannot Open Polybag Group(s)", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void DeleteSelectedPolybag()
        {
            using (SqlConnection conn = (SqlConnection) MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();
                foreach (DataGridViewRow gridViewRow in _gridPolybags.SelectedRows)
                {
                    long polybagID = (long)gridViewRow.Cells[0].Value;

                    SqlCommand cmd = new SqlCommand("EstPolybag_d_ByPolybagID", conn);
                    cmd.CommandTimeout = 7200;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EST_Polybag_ID", polybagID);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

            // Remove any deleted records from the grid
            for (int i = _gridPolybags.Rows.Count - 1; i >= 0; ++i)
            {
                if (_gridPolybags.Rows[i].Selected)
                {
                    _gridPolybags.Rows.Remove(_gridPolybags.Rows[i]);
                }
            }

            EnableToolbarButtons();
        }

        #endregion

        #region Event Handlers

        private void _btnOpenPolybag_Click(object sender, EventArgs e)
        {
            OpenSelectedPolybags(false);
        }

        private void _btnOpenPolybagReadOnly_Click(object sender, EventArgs e)
        {
            OpenSelectedPolybags(true);
        }

        private void _btnNewPolybag_Click(object sender, EventArgs e)
        {
            MainForm main = (MainForm)ParentForm;
            main.OpenPolybagGroup( false, null );
        }

        private void SearchCriteria_Changed(object sender, EventArgs e)
        {
            EnableSearchButton();
        }

        private void _btnSearch_Click(object sender, EventArgs e)
        {
            _gridPolybags.Rows.Clear();

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("EstPolybag_Search", conn);
                cmd.CommandTimeout = 7200;
                cmd.CommandType = CommandType.StoredProcedure;

                if ( _txtPolybagID.Text.Trim().Length > 0 )
                {
                    cmd.Parameters.AddWithValue( "@EST_Polybag_ID", _txtPolybagID.Text.Trim() );
                    cmd.Parameters.AddWithValue( "@Description", DBNull.Value );
                    cmd.Parameters.AddWithValue( "@Comments", DBNull.Value );
                    cmd.Parameters.AddWithValue( "@EST_Season_ID", DBNull.Value );
                    cmd.Parameters.AddWithValue( "@FiscalYear", DBNull.Value );
                    cmd.Parameters.AddWithValue( "@FiscalMonth", DBNull.Value );
                    cmd.Parameters.AddWithValue( "@HostAdNumber", DBNull.Value );
                    cmd.Parameters.AddWithValue( "@StartRunDate", DBNull.Value );
                    cmd.Parameters.AddWithValue( "@EndRunDate", DBNull.Value );
                    cmd.Parameters.AddWithValue( "@CreatedBy", DBNull.Value );
                    cmd.Parameters.AddWithValue( "@EST_Status_ID", DBNull.Value );
                }
                else
                {
                    cmd.Parameters.AddWithValue( "@EST_Polybag_ID", DBNull.Value );
                    if ( _txtPolybagDesc.Text.Trim().Length > 0 )
                        cmd.Parameters.AddWithValue( "@Description", _txtPolybagDesc.Text.Trim() );
                    else
                        cmd.Parameters.AddWithValue( "@Description", DBNull.Value );
                    if ( _txtPolybagComments.Text.Trim().Length > 0 )
                        cmd.Parameters.AddWithValue( "@Comments", _txtPolybagComments.Text.Trim() );
                    else
                        cmd.Parameters.AddWithValue( "@Comments", DBNull.Value );
                    if ( _cboSeason.SelectedIndex > 0 )
                        cmd.Parameters.AddWithValue( "@EST_Season_ID", _cboSeason.SelectedValue );
                    else
                        cmd.Parameters.AddWithValue( "@EST_Season_ID", DBNull.Value );
                    if ( _cboFiscalYear.SelectedIndex > 0 )
                        cmd.Parameters.AddWithValue( "@FiscalYear", _cboFiscalYear.SelectedValue );
                    else
                        cmd.Parameters.AddWithValue( "@FiscalYear", DBNull.Value );
                    if ( _cboFiscalMonth.SelectedIndex > 0 )
                        cmd.Parameters.AddWithValue( "@FiscalMonth", _cboFiscalMonth.SelectedValue );
                    else
                        cmd.Parameters.AddWithValue( "@FiscalMonth", DBNull.Value );
                    if ( _txtAdNumber.Text.Trim().Length > 0 )
                        cmd.Parameters.AddWithValue( "@HostAdNumber", _txtAdNumber.Text.Trim() );
                    else
                        cmd.Parameters.AddWithValue( "@HostAdNumber", DBNull.Value );
                    if ( _dtStartRunDate.Value.HasValue )
                        cmd.Parameters.AddWithValue( "@StartRunDate", _dtStartRunDate.Value );
                    else
                        cmd.Parameters.AddWithValue( "@StartRunDate", DBNull.Value );
                    if ( _dtEndRunDate.Value.HasValue )
                        cmd.Parameters.AddWithValue( "@EndRunDate", _dtEndRunDate.Value );
                    else
                        cmd.Parameters.AddWithValue( "@EndRunDate", DBNull.Value );
                    if ( _cboCreatedBy.SelectedIndex > 0 )
                        cmd.Parameters.AddWithValue( "@CreatedBy", _cboCreatedBy.SelectedValue );
                    else
                        cmd.Parameters.AddWithValue( "@CreatedBy", DBNull.Value );
                    if ( _cboEstimateStatus.SelectedIndex > 0 )
                        cmd.Parameters.AddWithValue( "@EST_Status_ID", _cboEstimateStatus.SelectedValue );
                    else
                        cmd.Parameters.AddWithValue( "@EST_Status_ID", DBNull.Value );
                }

                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows)
                {
                    dr.Close();
                    conn.Close();
                    MessageBox.Show(Resources.PolybagSearchNoRecords);
                    _lblPolybags.Text = string.Concat((_gridPolybags.RowCount).ToString(), " Polybag(s) Returned on: ", DateTime.Now.ToString());
                    return;
                }

                while (dr.Read())
                {
                    _gridPolybags.Rows.Add(dr.GetInt64(dr.GetOrdinal("EST_Polybag_ID")), dr.GetDateTime(dr.GetOrdinal("RunDate")).ToShortDateString(),
                        dr["Description"].ToString(), dr["Comments"].ToString(), dr["Season"].ToString(), dr["FiscalYear"].ToString(),
                        dr["EstimateStatus"].ToString());
                }

                dr.Close();
                conn.Close();
            }

            _lblPolybags.Text = string.Concat((_gridPolybags.RowCount).ToString(), " Polybag(s) Returned on: ", DateTime.Now.ToString());
        }

        private void _dtEndRunDate_Validating(object sender, CancelEventArgs e)
        {
            if (_dtStartRunDate.Value != null && _dtEndRunDate.Value != null)
            {
                if (_dtEndRunDate.Value < _dtStartRunDate.Value)
                {
                    _errorProvider.SetError(_dtEndRunDate, "End Date must be on or after the Begin Date");
                    e.Cancel = true;
                }
            }
        }

        private void _dtEndRunDate_Validated(object sender, EventArgs e)
        {
            _errorProvider.SetError(_dtEndRunDate, string.Empty);
        }

        private void _btnReset_Click(object sender, EventArgs e)
        {
            _errorProvider.Clear();
            _txtPolybagID.Text = string.Empty;
            _txtPolybagDesc.Text = string.Empty;
            _txtPolybagComments.Text = string.Empty;
            _cboSeason.SelectedIndex = 0;
            _cboFiscalYear.SelectedIndex = 0;
            _cboFiscalMonth.SelectedIndex = 0;
            _txtAdNumber.Text = string.Empty;
            _dtStartRunDate.Value = null;
            _dtEndRunDate.Value = null;
            _cboCreatedBy.SelectedIndex = 0;
            _cboEstimateStatus.SelectedIndex = 0;

            _btnSearch.Enabled = false;
        }

        private void _btnPrint_Click(object sender, EventArgs e)
        {
            ExcelWriter writer = null;
            try
            {
                writer = new ExcelWriter();
                this.Export(ref writer);
                writer.Show();
            }
            finally
            {
                writer.Dispose();
            }
        }

        private void _btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(Resources.DeletePolybagGroupWarning, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
                {
                    conn.Open();

                    foreach (DataGridViewRow dg_row in _gridPolybags.Rows)
                    {
                        if (dg_row.Selected)
                        {
                            SqlCommand cmd = new SqlCommand("EstPolybagGroup_d_ByPolybagID", conn);
                            cmd.CommandTimeout = 7200;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@est_polybaggroup_id", (long)dg_row.Cells[0].Value);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    conn.Close();
                }

                _btnSearch_Click(sender, e);
            }
        }

        private void _txtPolybagDesc_Validated(object sender, EventArgs e)
        {
            _txtPolybagDesc.Text = _txtPolybagDesc.Text.Trim();
        }

        private void _txtPolybagComments_Validated(object sender, EventArgs e)
        {
            _txtPolybagComments.Text = _txtPolybagComments.Text.Trim();
        }

        private void _gridPolybags_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            OpenSelectedPolybags(false);
        }

        #endregion

        private void _gridPolybags_SelectionChanged(object sender, EventArgs e)
        {
            EnableToolbarButtons();
        }
    }
}