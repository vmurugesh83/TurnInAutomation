#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data;

using CatalogEstimating.CustomControls;
using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.LookupTableAdapters;
using CatalogEstimating.Properties;

#endregion

namespace CatalogEstimating.UserControls.Main
{
    public partial class ucpEstimateSearch : CatalogEstimating.UserControlPanel
    {
        #region Private Variables

        private static Dictionary<int, string> FiscalMonthDictionary = new Dictionary<int, string>();
        private static Dictionary<int, string> InsertDOWDictionary = new Dictionary<int, string>();

        #endregion

        #region Construction

        public ucpEstimateSearch()
        {
            InitializeComponent();
        }

        static ucpEstimateSearch()
        {
            // Fill in the dictionaries for the lookup tables that won't change
            foreach ( int value in Enum.GetValues( typeof( FiscalMonths ) ) )
            {
                FiscalMonthDictionary.Add( value, Enum.GetName( typeof( FiscalMonths ), value ) );
            }

            // Fill in the dictionaries for the lookup tables that won't change
            foreach ( int value in Enum.GetValues( typeof( InsertDOW ) ) )
            {
                InsertDOWDictionary.Add( value, Enum.GetName( typeof( InsertDOW ), value ) );
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
            using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
            {
                using ( est_statusTableAdapter adapter = new est_statusTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsLookup.est_status );
                }

                using ( est_estimatemediatypeTableAdapter adapter = new est_estimatemediatypeTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsLookup.est_estimatemediatype );
                }

                using ( est_seasonTableAdapter adapter = new est_seasonTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsLookup.est_season );
                }

                using ( EstEstimate_s_CreatedByTableAdapter adapter = new EstEstimate_s_CreatedByTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsLookup.EstEstimate_s_CreatedBy );
                }

                using ( EstEstimate_s_FiscalYearTableAdapter adapter = new EstEstimate_s_FiscalYearTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsLookup.EstEstimate_s_FiscalYear );
                }

                using ( EstPackage_s_InsertScenarioTableAdapter adapter = new EstPackage_s_InsertScenarioTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsLookup.EstPackage_s_InsertScenario );
                }

                conn.Close();
            }

            // Manually bind to the dictionaries I created for some of the lookup values
            _cboFiscalMonth.DataSource    = new BindingSource( FiscalMonthDictionary, null );
            _cboFiscalMonth.DisplayMember = "Value";
            _cboFiscalMonth.ValueMember   = "Key";

            _cboInsertDOW.DataSource    = new BindingSource( InsertDOWDictionary, null );
            _cboInsertDOW.DisplayMember = "Value";
            _cboInsertDOW.ValueMember   = "Key";

            _btnReset_Click( this, EventArgs.Empty );
        }

        #endregion

        #region Private Methods

        private object GetProcParam( Control ctrl, Type paramType )
        {
            if ( ctrl.Text == null || ctrl.Text.Trim().Length == 0 )
                return DBNull.Value;
            else
                return Convert.ChangeType( ctrl.Text, paramType );
        }

        private object GetProcInt( IntegerTextBox ctrl )
        {
            if ( ctrl.Value == null )
                return DBNull.Value;
            else
                return ctrl.Value.Value;
        }

        private object GetProcID( ComboBox combo )
        {
            if ( combo.Text == null || combo.Text.Trim().Length == 0 )
                return DBNull.Value;
            else
                return combo.SelectedValue;
        }

        private void CalculateTotalCost()
        {
            using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
            {
                conn.Open();

                // Setup the stored proc call that will calculate the total cost of the estimate
                SqlCommand costCmd     = new SqlCommand();
                costCmd.Connection     = conn;
                costCmd.CommandTimeout = 600;
                costCmd.CommandType    = CommandType.StoredProcedure;
                costCmd.CommandText    = "EstEstimate_s_CostSummary_ByEstimateID";

                SqlParameter estIdParam = new SqlParameter( "@EST_Estimate_ID", SqlDbType.BigInt );
                costCmd.Parameters.Add( estIdParam );

                foreach ( EstimateSearch.EstEstimate_SearchRow searchRow in _dsSearchResults.EstEstimate_Search.Rows )
                {
                    costCmd.Parameters["@EST_Estimate_ID"].Value = searchRow.EST_Estimate_ID;
                    using ( SqlDataReader costDr = costCmd.ExecuteReader() )
                    {
                        if ( costDr.HasRows )
                        {
                            // The first row returned by the stored proc has the estimate total in it
                            costDr.Read();
                            int col = costDr.GetOrdinal( "GrandTotal" );
                            if ( !costDr.IsDBNull( col ) )
                                searchRow.TotalCost = costDr.GetDecimal( col );
                        }
                        costDr.Close();
                    }
                }

                conn.Close();
            }
        }

        private void InitializePlusMinus()
        {
            // Go through all the rows in the returned set and if it has a parent id, but there is no row
            // make it visible
            foreach ( EstimateSearch.EstEstimate_SearchRow row in _dsSearchResults.EstEstimate_Search.Rows )
            {
                if ( !row.IsParentIdNull() && _dsSearchResults.EstEstimate_Search.FindByEST_Estimate_ID( row.ParentId ) == null )
                    row.displayed = 1;
            }

            foreach ( DataGridViewRow rowGridView in _gridEstimates.Rows )
            {
                if ( rowGridView.Visible )
                {
                    DataRowView viewRow = (DataRowView)rowGridView.DataBoundItem;
                    EstimateSearch.EstEstimate_SearchRow row = (EstimateSearch.EstEstimate_SearchRow)viewRow.Row;

                    OptionalButtonCell bc = new OptionalButtonCell();
                    bc.Value = "+";
                    rowGridView.Cells[0] = bc;

                    // If there are no children, then use a blank in the button
                    if ( row.GetEstEstimate_SearchRows().Length == 0 )
                        bc.Value = string.Empty;

                    if ( !row.IsParentIdNull() )
                        bc.Display = false;
                }
            }
        }

        private void EnableSearchButton()
        {
            bool bEnable = false;
            foreach ( Control ctrl in _groupFilter.Controls )
            {
                TextBox text = ctrl as TextBox;
                if ( text != null )
                {
                    if ( text.Text.Trim().Length > 0 )
                    {
                        bEnable = true;
                        break;
                    }
                    else
                        continue;
                }

                ComboBox combo = ctrl as ComboBox;
                if ( combo != null )
                {
                    if ( combo.Text.Length > 0 )
                    {
                        bEnable = true;
                        break;
                    }
                    else
                        continue;
                }

                NullableDateTimePicker date = ctrl as NullableDateTimePicker;
                if ( date != null )
                {
                    if ( date.Value != null )
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
            bool bEditUser = ( MainForm.AuthorizedUser.Right != UserRights.ReadOnly );
            bool bHasKilled = false;
            bool bHasActive = false;
            bool bHasUploaded = false;

            foreach ( DataGridViewRow gridViewRow in _gridEstimates.SelectedRows )
            {
                DataRowView viewRow = (DataRowView)gridViewRow.DataBoundItem;
                EstimateSearch.EstEstimate_SearchRow row = (EstimateSearch.EstEstimate_SearchRow)viewRow.Row;

                if ( row.StatusDesc == "Active" )
                    bHasActive = true;
                else if ( row.StatusDesc == "Killed" )
                    bHasKilled = true;
                else
                    bHasUploaded = true;
            }

            _btnNew.Enabled = _menuContextNew.Enabled = bEditUser;
            _btnOpen.Enabled = _menuContextOpen.Enabled = bHasActive && bEditUser;
            _btnOpenReadOnly.Enabled = _menuContextOpenReadOnly.Enabled = bHasActive || bHasUploaded || bHasKilled;
            _btnCopyEstimates.Enabled = _menuContextCopy.Enabled = _btnOpenReadOnly.Enabled && bEditUser;
            _btnKill.Enabled = _menuContextKill.Enabled = bHasActive && bEditUser;
            _btnUnkill.Enabled = _menuContextUnkill.Enabled = bHasKilled && bEditUser;
            _btnUpload.Enabled = _menuContextUpload.Enabled =  bHasActive && bEditUser && ( MainForm.WorkingDatabase.Type == DatabaseType.Live );
            _btnNewPolybag.Enabled = _menuContextAddPolybagGroup.Enabled = bHasActive && bEditUser;
        }

        private void OpenSelectedEstimates( bool bForceReadOnly )
        {
            MainForm main = (MainForm)ParentForm;

            if ( MainForm.AuthorizedUser.Right == UserRights.ReadOnly )
                bForceReadOnly = true;

            foreach ( DataGridViewRow gridViewRow in _gridEstimates.SelectedRows )
            {
                DataRowView viewRow = (DataRowView)gridViewRow.DataBoundItem;
                EstimateSearch.EstEstimate_SearchRow row = (EstimateSearch.EstEstimate_SearchRow)viewRow.Row;
                
                long? parentId = null;
                if ( !row.IsParentIdNull() )
                    parentId = row.ParentId;

                bool readOnly = bForceReadOnly;
                if ( row.StatusDesc != "Active" )
                    readOnly = true;

                main.OpenEstimate( readOnly, row.EST_Estimate_ID, parentId, null );
            }
        }

        private bool IsEstimateInPolybag(long EstimateID)
        {
            bool retval = false;

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("EstEstimatePolybagGroupMap_s_ByEstimateID", conn);
                cmd.CommandTimeout = 7200;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EST_Estimate_ID", EstimateID);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                    retval = true;

                dr.Close();
                conn.Close();

                return retval;
            }
        }

        private void KillSelectedEstimates()
        {
            bool bEstimateNotKilled = false;
            MainForm main = (MainForm) ParentForm;

            Database db = MainForm.WorkingDatabase.Database;
            using (SqlConnection conn = (SqlConnection)db.CreateConnection())
            {
                conn.Open();

                foreach (DataGridViewRow gridViewRow in _gridEstimates.SelectedRows)
                {
                    DataRowView viewRow = (DataRowView)gridViewRow.DataBoundItem;
                    EstimateSearch.EstEstimate_SearchRow row = (EstimateSearch.EstEstimate_SearchRow) viewRow.Row;

                    // Can't kill an estimate that is not active
                    if (row.StatusDesc != EstimateStatus.Active.ToString())
                        bEstimateNotKilled = true;
                    // Can't kill an open estimate
                    else if (main.IsEstimateOpen(row.EST_Estimate_ID))
                        bEstimateNotKilled = true;
                    //check if it is part of a polybag
                    else if (IsEstimateInPolybag(row.EST_Estimate_ID))
                        bEstimateNotKilled = true;
                    // Kill the Estimate
                    else
                    {
                        SqlCommand cmd = new SqlCommand("EstEstimate_u_Status", conn);
                        cmd.CommandTimeout = 7200;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EST_Estimate_ID", row.EST_Estimate_ID);
                        cmd.Parameters.AddWithValue("@EST_Status_ID", (int)EstimateStatus.Killed);
                        cmd.Parameters.AddWithValue("@ModifiedBy", MainForm.AuthorizedUser.FormattedName);
                        cmd.ExecuteNonQuery();

                        // Update the grid UI right away without requerying
                        _dsSearchResults.EstEstimate_Search.StatusDescColumn.ReadOnly     = false;
                        _dsSearchResults.EstEstimate_Search.UploadKillDateColumn.ReadOnly = false;
                        row.StatusDesc     = EstimateStatus.Killed.ToString();
                        row.UploadKillDate = DateTime.Now;
                    }
                }

                conn.Close();
            }

            EnableToolbarButtons();

            if (bEstimateNotKilled)
            {
                MessageBox.Show("Some Estimates could not be killed.  They are either open, are part of a polybag or do not have a status of active.", "Cannot Kill Estimate(s)", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UnkillSelectedEstimates()
        {
            bool bEstimateNotUnKilled = false;
            MainForm main = (MainForm)ParentForm;

            Database db = MainForm.WorkingDatabase.Database;
            using (SqlConnection conn = (SqlConnection)db.CreateConnection())
            {
                conn.Open();

                foreach (DataGridViewRow gridViewRow in _gridEstimates.SelectedRows)
                {
                    DataRowView viewRow = (DataRowView)gridViewRow.DataBoundItem;
                    EstimateSearch.EstEstimate_SearchRow row = (EstimateSearch.EstEstimate_SearchRow)viewRow.Row;

                    // Can't unkill an estimate that is not killed
                    if (row.StatusDesc != EstimateStatus.Killed.ToString())
                        bEstimateNotUnKilled = true;
                    // Can't unkill an open estimate
                    else if (main.IsEstimateOpen(row.EST_Estimate_ID))
                        bEstimateNotUnKilled = true;
                    // Unkill the Estimate
                    else
                    {
                        SqlCommand cmd = new SqlCommand("EstEstimate_u_Status", conn);
                        cmd.CommandTimeout = 7200;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EST_Estimate_ID", row.EST_Estimate_ID);
                        cmd.Parameters.AddWithValue("@EST_Status_ID", (int)EstimateStatus.Active);
                        cmd.Parameters.AddWithValue("@ModifiedBy", MainForm.AuthorizedUser.FormattedName);
                        cmd.ExecuteNonQuery();

                        // Update the grid UI right away without requerying
                        _dsSearchResults.EstEstimate_Search.StatusDescColumn.ReadOnly     = false;
                        _dsSearchResults.EstEstimate_Search.UploadKillDateColumn.ReadOnly = false;
                        row.StatusDesc     = EstimateStatus.Active.ToString();
                        row.SetUploadKillDateNull();
                    }
                }

                conn.Close();
            }

            EnableToolbarButtons();

            if (bEstimateNotUnKilled)
            {
                MessageBox.Show("Some Estimates could not be unkilled.  They are either open or do not have a status of killed.", "Cannot Unkill Estimate(s)", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region Event Handlers

        private void _gridEstimates_CellDoubleClick( object sender, DataGridViewCellEventArgs e )
        {
            if (e.RowIndex < 0)
                return;

            OpenSelectedEstimates( false );
        }

        private void _btnOpen_Click( object sender, EventArgs e )
        {
            OpenSelectedEstimates( false );
        }

        private void _btnOpenReadOnly_Click( object sender, EventArgs e )
        {
            OpenSelectedEstimates( true );
        }

        private void _btnKill_Click( object sender, EventArgs e )
        {
            KillSelectedEstimates();
        }

        private void _btnUnkill_Click( object sender, EventArgs e )
        {
            UnkillSelectedEstimates();
        }

        private void _btnNew_Click( object sender, EventArgs e )
        {
            MainForm main = (MainForm)ParentForm;
            main.OpenEstimate( false, null, null, null );
        }

        private void _btnNewPolybag_Click( object sender, EventArgs e )
        {
            MainForm main = (MainForm)ParentForm;
            main.OpenPolybagGroup( false, null );
        }

        private void _gridEstimates_CellContentClick( object sender, DataGridViewCellEventArgs e )
        {
            // Toggle the view state of the child estimate search items found
            if ( ( e.ColumnIndex == 0 ) && ( e.RowIndex >= 0 ) && ( _gridEstimates[e.ColumnIndex, e.RowIndex] != null ) )
            {
                DataGridViewCell buttonCell = _gridEstimates[e.ColumnIndex, e.RowIndex];
                if ( buttonCell.Value != null )
                {
                    long estimateId = (long)_gridEstimates["eSTEstimateIDDataGridViewTextBoxColumn", e.RowIndex].Value;
                    int display = 0;

                    // Toggle the +/- on the button to show expanded or collapsed
                    if ( buttonCell.Value.ToString() == "+" )
                    {
                        buttonCell.Value = "-";
                        display = 1;
                    }
                    else if ( buttonCell.Value.ToString() == "-" )
                    {
                        buttonCell.Value = "+";
                        display = 0;
                    }

                    if ( buttonCell.Value.ToString().Length > 0 )
                    {
                        // Toggle the display of all the rows that have the clicked row as a parent
                        DataView dView = new DataView( _dsSearchResults.Tables[0] );
                        dView.RowFilter = "ParentId = " + estimateId.ToString();

                        foreach ( DataRowView rowView in dView )
                        {
                            DataRow row = rowView.Row;
                            row["displayed"] = display;
                        }

                        // Hide the +/- button on all child rows
                        foreach ( DataGridViewRow viewRow in _gridEstimates.Rows )
                        {
                            if ( _gridEstimates["ParentId", viewRow.Index].Value != DBNull.Value )
                            {
                                OptionalButtonCell bc = new OptionalButtonCell();
                                bc.Display = false;
                                viewRow.Cells[0] = bc;
                            }
                        }
                    }
                }
            }
        }

        private void SearchField_TextChanged( object sender, EventArgs e )
        {
            EnableSearchButton();
        }

        private void SearchDate_ValueChanged( object sender, EventArgs e )
        {
            EnableSearchButton();
        }

        private void _gridEstimates_SelectionChanged( object sender, EventArgs e )
        {
            EnableToolbarButtons();
        }

        private void _btnSearch_Click( object sender, EventArgs e )
        {
            Database db = MainForm.WorkingDatabase.Database;
            using ( SqlConnection conn = (SqlConnection)db.CreateConnection() )
            {
                Cursor = Cursors.WaitCursor;
                conn.Open();

                SqlCommand searchCmd = new SqlCommand();
                searchCmd.CommandTimeout = 7200;
                searchCmd.Connection = conn;
                searchCmd.CommandType = CommandType.StoredProcedure;

                if ( _txtEstimateID.Text.Trim().Length > 0 )
                {
                    searchCmd.CommandText = "EstEstimate_s_Search";
                    db.AddInParameter( searchCmd, "@EstEstimateId", DbType.Int64, GetProcParam( _txtEstimateID, typeof( long ) ) );
                }
                else
                {
                    searchCmd.CommandText = "EstEstimate_Search";

                    // First add all the parameters that are not ID based
                    db.AddInParameter( searchCmd, "@Description", DbType.String, GetProcParam( _txtEstimateDesc, typeof( string ) ) );
                    db.AddInParameter( searchCmd, "@AdNumber", DbType.Int32, GetProcParam( _txtAdNumber, typeof( int ) ) );
                    db.AddInParameter( searchCmd, "@ComponentDescription", DbType.String, GetProcParam( _txtComponentDesc, typeof( string ) ) );
                    db.AddInParameter( searchCmd, "@HostPageCount", DbType.Int32, GetProcInt( _txtHostPageCount ) );
                    db.AddInParameter( searchCmd, "@HostMediaQtyLow", DbType.Int32, GetProcInt( _txtHostMediaQtyStart ) );
                    db.AddInParameter( searchCmd, "@HostMediaQtyHigh", DbType.Int32, GetProcInt( _txtHostMediaQtyEnd ) );
                    db.AddInParameter( searchCmd, "@CreatedBy", DbType.String, GetProcParam( _cboCreatedBy, typeof( string ) ) );
                    db.AddInParameter( searchCmd, "@RunDateStart", DbType.DateTime, GetProcParam( _dtRunDateRangeStart, typeof( DateTime ) ) );
                    db.AddInParameter( searchCmd, "@RunDateEnd", DbType.DateTime, GetProcParam( _dtRunDateRangeEnd, typeof( DateTime ) ) );
                    db.AddInParameter( searchCmd, "@ModifiedDateStart", DbType.DateTime, GetProcParam( _dtModifiedDateStart, typeof( DateTime ) ) );
                    db.AddInParameter( searchCmd, "@ModifiedDateEnd", DbType.DateTime, GetProcParam( _dtModifiedDateEnd, typeof( DateTime ) ) );
                    db.AddInParameter( searchCmd, "@EstimateComments", DbType.String, GetProcParam( _txtEstimateComments, typeof( string ) ) );
                    db.AddInParameter( searchCmd, "@FiscalYear", DbType.Int32, GetProcParam( _cboFiscalYear, typeof( int ) ) );

                    // Now go get these ID's
                    db.AddInParameter( searchCmd, "@PUB_InsertScenario_ID", DbType.Int64, GetProcID( _cboInsertScenario ) );
                    db.AddInParameter( searchCmd, "@HostMediaType", DbType.Int32, GetProcID( _cboHostEstMedia ) );
                    db.AddInParameter( searchCmd, "@EST_Status_ID", DbType.Int32, GetProcID( _cboEstimateStatus ) );
                    db.AddInParameter( searchCmd, "@EST_Season_ID", DbType.Int32, GetProcID( _cboSeason ) );
                    db.AddInParameter( searchCmd, "@FiscalMonth", DbType.Int32, GetProcID( _cboFiscalMonth ) );
                    db.AddInParameter( searchCmd, "@InsertDOW", DbType.Int32, GetProcID( _cboInsertDOW ) );
                }

                DataSet dsSearchResults = db.ExecuteDataSet( searchCmd );
                _dsSearchResults.EstEstimate_Search.Clear();
                _dsSearchResults.EstEstimate_Search.Merge( dsSearchResults.Tables[0] );
                conn.Close();

                if ( _chkCalculateCost.Checked )
                    CalculateTotalCost();

                Cursor = Cursors.Default;

                if ( _dsSearchResults.EstEstimate_Search.Rows.Count > 0 )
                {
                    estEstimateSearchBindingSource.DataSource = _dsSearchResults;
                    InitializePlusMinus();
                }
                else
                {
                    MessageBox.Show( Resources.EstimateSearchNoRecords );
                }
            }

            _lblEstimates.Text = string.Concat( ( _dsSearchResults.EstEstimate_Search.Rows.Count ).ToString(), " Estimate(s) Returned on: ", DateTime.Now.ToString() );
        }

        private void _btnReset_Click( object sender, EventArgs e )
        {
            _txtAdNumber.Text          = string.Empty;
            _txtComponentDesc.Text     = string.Empty;
            _txtEstimateComments.Text  = string.Empty;
            _txtEstimateDesc.Text      = string.Empty;
            _txtEstimateID.Text        = string.Empty;
            _txtHostMediaQtyEnd.Text   = string.Empty;
            _txtHostMediaQtyStart.Text = string.Empty;
            _txtHostPageCount.Text     = string.Empty;

            _dtModifiedDateEnd.Value   = null;
            _dtModifiedDateStart.Value = null;
            _dtRunDateRangeEnd.Value   = null;
            _dtRunDateRangeStart.Value = null;

            _cboCreatedBy.Text      = string.Empty;
            _cboEstimateStatus.Text = string.Empty;
            _cboFiscalMonth.Text    = string.Empty;
            _cboFiscalYear.Text     = string.Empty;
            _cboHostEstMedia.Text   = string.Empty;
            _cboInsertDOW.Text      = string.Empty;
            _cboInsertScenario.Text = string.Empty;
            _cboSeason.Text         = string.Empty;

            _btnSearch.Enabled = false;
        }

        private void _btnPrint_Click( object sender, EventArgs e )
        {
            ExcelWriter writer = null;
            try
            {
                writer = new ExcelWriter();
                writer.WriteLine( _groupFilter.Text );

                // First "column" of filter data
                writer.WriteLine( _lblEstimateID.Text, _txtEstimateID.Text );
                writer.WriteLine( _lblEstimateDesc.Text, _txtEstimateDesc.Text );
                writer.WriteLine( _lblEstimateStatus.Text, _cboEstimateStatus.Text );
                writer.WriteLine( _lblAdNumber.Text, _txtAdNumber.Text );
                writer.WriteLine( _lblComponentDesc.Text, _txtComponentDesc.Text );
                writer.WriteLine( _lblHostEstMedia.Text, _cboHostEstMedia.Text );
                writer.WriteLine( _lblHostPageCount.Text, _txtHostPageCount.Text );
                writer.WriteLine( _lblHostMediaQty.Text, _txtHostMediaQtyStart.Text, _txtHostMediaQtyEnd.Text );
                writer.WriteLine( _lblInsertScenario.Text, _cboInsertScenario.Text );

                // Second "column" of filter data
                writer.WriteLine( _lblRunDateRangeStart.Text, _dtRunDateRangeStart.Text, _dtRunDateRangeEnd.Text );
                writer.WriteLine( _lblModifiedDateStart.Text, _dtModifiedDateStart.Text, _dtModifiedDateEnd.Text );
                writer.WriteLine( _lblCreatedBy.Text, _cboCreatedBy.Text );
                writer.WriteLine( _lblSeason.Text, _cboSeason.Text );
                writer.WriteLine( _lblFiscalYear.Text, _cboFiscalYear.Text );
                writer.WriteLine( _lblFiscalMonth.Text, _cboFiscalMonth.Text );
                writer.WriteLine( _lblInsertDOW.Text, _cboInsertDOW.Text );
                writer.WriteLine( _lblEstimateComments.Text, _txtEstimateComments.Text );

                // Search results grid
                writer.WriteLine();
                writer.WriteLine( _lblEstimates.Text );
                writer.WriteTable( _gridEstimates, true );

                writer.Show();
            }
            catch ( System.Runtime.InteropServices.COMException )
            {
                writer.Quit();
            }
            finally
            {
                writer.Dispose();
            }
        }

        private void _menuContextAddPolybagGroup_Click( object sender, EventArgs e )
        {
            MainForm main = (MainForm)ParentForm;
            List<long> estimateIds = new List<long>();

            foreach ( DataGridViewRow gridViewRow in _gridEstimates.SelectedRows )
            {
                DataRowView viewRow = (DataRowView)gridViewRow.DataBoundItem;
                EstimateSearch.EstEstimate_SearchRow row = (EstimateSearch.EstEstimate_SearchRow)viewRow.Row;

                if ( row.StatusDesc == "Active" )
                    estimateIds.Add( row.EST_Estimate_ID );
            }

            main.AddEstimatesToPolybagGroup( estimateIds );
        }

        private void _btnCopyEstimates_Click(object sender, EventArgs e)
        {
            MainForm main = (MainForm)ParentForm;
            List<long> estimateIds = new List<long>();

            foreach (DataGridViewRow gridViewRow in _gridEstimates.SelectedRows)
            {
                DataRowView viewRow = (DataRowView)gridViewRow.DataBoundItem;
                EstimateSearch.EstEstimate_SearchRow row = (EstimateSearch.EstEstimate_SearchRow)viewRow.Row;

                estimateIds.Add(row.EST_Estimate_ID);
            }

            CopyNumberDialog dialogCopyNumber = new CopyNumberDialog(estimateIds);
            dialogCopyNumber.ShowDialog();
            dialogCopyNumber.Activate();
        }

        private void _btnUpload_Click(object sender, EventArgs e)
        {
            MainForm main = (MainForm)ParentForm;

            List<long> estimateIds = new List<long>();
            foreach (DataGridViewRow gridViewRow in _gridEstimates.SelectedRows)
            {
                DataRowView viewRow = (DataRowView)gridViewRow.DataBoundItem;
                EstimateSearch.EstEstimate_SearchRow row = (EstimateSearch.EstEstimate_SearchRow)viewRow.Row;
                if (row.StatusDesc == EstimateStatus.Active.ToString())
                    estimateIds.Add(row.EST_Estimate_ID);
            }

            main.DisplayUploadControl(estimateIds);
        }

        private void _dtRunDateRangeEnd_Validating( object sender, CancelEventArgs e )
        {
            if ( _dtRunDateRangeStart.Value != null && _dtRunDateRangeEnd.Value != null )
            {
                if ( _dtRunDateRangeEnd.Value < _dtRunDateRangeStart.Value )
                {
                    _errorProvider.SetError( _dtRunDateRangeEnd, "To Date must be on or after the From Date" );
                    e.Cancel = true;
                }
            }
        }

        private void _dtModifiedDateEnd_Validating( object sender, CancelEventArgs e )
        {
            if ( _dtModifiedDateStart.Value != null && _dtModifiedDateEnd.Value != null )
            {
                if ( _dtModifiedDateEnd.Value < _dtModifiedDateStart.Value )
                {
                    _errorProvider.SetError( _dtModifiedDateEnd, "To Date must be on or after the From Date" );
                    e.Cancel = true;
                }
            }

        }

        private void _txtHostMediaQtyEnd_Validating( object sender, CancelEventArgs e )
        {
            if ( _txtHostMediaQtyStart.Value != null && _txtHostMediaQtyEnd.Value != null )
            {
                if ( _txtHostMediaQtyEnd.Value < _txtHostMediaQtyStart.Value )
                {
                    _errorProvider.SetError( _txtHostMediaQtyEnd, "To Quantity must be less than the From Quantity" );
                    e.Cancel = true;
                }
            }
        }

        private void Control_Validated( object sender, EventArgs e )
        {
            _errorProvider.SetError( sender as Control, string.Empty );
        }

        private void TextBox_Validated( object sender, EventArgs e )
        {
            TextBox box = sender as TextBox;
            if ( box != null )
                box.Text = box.Text.Trim();
        }

        private void ucpEstimateSearch_Load(object sender, EventArgs e)
        {
            EnableToolbarButtons();
        }

        #endregion
    }
}