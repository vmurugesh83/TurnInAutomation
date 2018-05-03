#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.AdministrationTableAdapters;
using System.Data.SqlClient;
using CatalogEstimating.Properties;

#endregion

namespace CatalogEstimating.UserControls.Admin
{
    public partial class ucpAdminDatabases : CatalogEstimating.UserControlPanel
    {
        #region Construction

        public ucpAdminDatabases()
        {
            InitializeComponent();
            Name = "Databases";
        }

        public ucpAdminDatabases( Administration ds )
        : this()
        {
            _dsAdministration = ds;
            assocDatabasessAllBindingSource.DataSource = ds;
        }

        #endregion

        #region Overrides

        public override ToolStrip Toolbar
        {
            get { return _toolStrip; }
        }

        /// <summary>Fill the database grid with all the databases.</summary>
        public override void LoadData()
        {
            using ( AssocDatabases_s_AllTableAdapter adapter = new AssocDatabases_s_AllTableAdapter() )
            {
                adapter.Fill( _dsAdministration.AssocDatabases_s_All );
                _txtCurrentDatabase.Text = MainForm.WorkingDatabase.FriendlyName;
            }


            DataView dvDatabases = new DataView(_dsAdministration.AssocDatabases_s_All);
            dvDatabases.RowFilter = "databasetype_id <> 1 and database_id <> " + MainForm.WorkingDatabase.Id.ToString() + " and display = 1";
            _cboDatabases.DataSource = dvDatabases;
            _cboDatabases.DisplayMember = "description";
            _cboDatabases.ValueMember = "connectionstring";

            base.LoadData();
        }

        public override void Reload()
        {
            base.Reload();

            _progressBar.Value = 0;
            _lblSynchStatus.Text = string.Empty;
        }
        public override void SaveData()
        {
            if ( _gridDatabases.CurrentRow != null )
            {
                _gridDatabases.EndEdit();
                assocDatabasessAllBindingSource.EndEdit();
            }

            using ( AssocDatabases_s_AllTableAdapter adapter = new AssocDatabases_s_AllTableAdapter() )
            {
                adapter.Update( _dsAdministration.AssocDatabases_s_All );
            }
            LoadData();

            base.SaveData();
        }

        public override void Export( ref ExcelWriter writer )
        {
            writer.WriteTable( _gridDatabases, true );
        }

        #endregion

        #region Event Handlers

        private void ucpAdminDatabases_Load( object sender, EventArgs e )
        {
            if ( MainForm.AuthorizedUser.Right != UserRights.SuperAdmin )
                _groupPurging.Enabled = false;

            if ( MainForm.AuthorizedUser.Right != UserRights.SuperAdmin || MainForm.WorkingDatabase.Type != DatabaseType.Live )
            {
                _btnCancel.Enabled = false;
                _gridDatabases.ReadOnly = true;
                _gridDatabases.ReadOnly = true;
                _gridDatabases.DefaultCellStyle.BackColor = SystemColors.Control;
            }

            if ( MainForm.AuthorizedUser.Right != UserRights.SuperAdmin &&
                 MainForm.AuthorizedUser.Right != UserRights.Admin )
            {
                _btnPurge.Enabled     = false;
                _cboDatabases.Enabled = false;
                _progressBar.Enabled  = false;
            }
        }

        /// <summary>Don't allow the user to change the Display property of the Live database.</summary>
        private void _gridDatabases_CellBeginEdit( object sender, DataGridViewCellCancelEventArgs e )
        {
            if ( _gridDatabases.Columns[e.ColumnIndex].DataPropertyName == "display" )
            {
                if ( _dsAdministration.AssocDatabases_s_All[e.RowIndex].databasetype_id == (int)DatabaseType.Live )
                    e.Cancel = true;
            }
        }

        private void _gridDatabases_CellEndEdit( object sender, DataGridViewCellEventArgs e )
        {
            if ( _gridDatabases.Columns[e.ColumnIndex].DataPropertyName == "displayorder" )
            {
                _gridDatabases.Rows[e.RowIndex].ErrorText = string.Empty;
            }
        }

        private void _gridDatabases_CellValidating( object sender, DataGridViewCellValidatingEventArgs e )
        {
            // Make sure that they aren't trying to have multiple rows with the same display order
            if ( _gridDatabases.Columns[e.ColumnIndex].DataPropertyName == "displayorder" )
            {
                int newDisplayOrder = Convert.ToInt32( e.FormattedValue );
                bool orderExists = false;

                foreach ( DataGridViewRow rowView in _gridDatabases.Rows )
                {
                    // Checks to see if there is already a default on an existing row that's not the one
                    // we're currently editing in the grid
                    if ( rowView.Index != e.RowIndex && rowView.DataBoundItem != null )
                    {
                        Administration.AssocDatabases_s_AllRow dbRow = (Administration.AssocDatabases_s_AllRow)( (DataRowView)rowView.DataBoundItem ).Row;
                        if ( dbRow.displayorder == newDisplayOrder )
                        {
                            orderExists = true;
                            break;
                        }
                    }
                }

                // If another row has this display order number, then error
                if ( orderExists )
                {
                    _gridDatabases.Rows[e.RowIndex].ErrorText = Resources.DuplicateDisplayOrderError;
                    e.Cancel = true;
                }
                else
                    _gridDatabases.Rows[e.RowIndex].ErrorText = string.Empty;
            }

        }

        private void _btnCancel_Click( object sender, EventArgs e )
        {
            DialogResult result = MessageBox.Show( Resources.CancelChangesWarning, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
            if ( result == DialogResult.Yes )
            {
                LoadData();
            }
        }

        private void _btnPurge_Click( object sender, EventArgs e )
        {
            // First check the purge date to make sure it passes business rules
            if ( MainForm.WorkingDatabase.Type == DatabaseType.Live )
            {
                // If trying to purge the live database, then the date has to be at least 3 years ago
                if ( _dtPurgeDate.Value > DateTime.Now.AddYears( -3 ) )
                {
                    _errorProvider.SetError( _dtPurgeDate, "The Live Database must retain at least 3 years of records." );
                    return;
                }
            }

            _errorProvider.SetError( _dtPurgeDate, string.Empty );
            bool bSuccess = false;

            DialogResult result = MessageBox.Show( Resources.PurgeWarning, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
            if ( result == DialogResult.Yes )
            {
                Cursor = Cursors.WaitCursor;

                using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
                {
                    conn.Open();

                    SqlCommand purgeCmd     = new SqlCommand();
                    purgeCmd.Connection     = conn;
                    purgeCmd.CommandType    = CommandType.StoredProcedure;
                    purgeCmd.CommandText    = "db_Purge_ByDate";
                    purgeCmd.CommandTimeout = 1200;

                    SqlParameter dateParam = new SqlParameter( "@PurgeDate", SqlDbType.DateTime );
                    dateParam.Value = _dtPurgeDate.Value;   
                    purgeCmd.Parameters.Add( dateParam );

                    try
                    {
                        purgeCmd.ExecuteNonQuery();
                        bSuccess = true;
                    }
                    catch ( SqlException sqlEx )
                    {
                        // Rethrow the exception if it's a critical failure.
                        // Otherwise could be an expected failure due to foreign key constraints
                        if ( sqlEx.Class > 16 )
                            throw;
                        else
                            bSuccess = true;
                    }
                    finally
                    {
                        conn.Close();
                        Cursor = Cursors.Default;
                    }
                }
                
                if ( bSuccess )
                    MessageBox.Show( MainForm.WorkingDatabase.FriendlyName + " was purged successfully.", "Database Purge Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information );
            }
        }

        private void _btnSynch_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("All data in the " + _cboDatabases.Text + " database will be lost and replaced with data from the " + MainForm.WorkingDatabase.FriendlyName + " database.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                this.Cursor = Cursors.WaitCursor;
                
                _progressBar.Value = 25;
                _lblSynchStatus.Text = "Purging Destination Database";
                this.Refresh();

                using (SqlConnection conn = new SqlConnection(_cboDatabases.SelectedValue.ToString()))
                {
                    conn.Open();

                    bool sqlErrorOccurred = false;
                    string SqlErrorMessage = string.Empty;

                    try
                    {
                        SqlCommand purge_cmd = new SqlCommand("db_Purge", conn);
                        purge_cmd.CommandTimeout = 600;
                        purge_cmd.CommandType = CommandType.StoredProcedure;
                        purge_cmd.ExecuteNonQuery();
                    }
                    catch (SqlException se)
                    {
                        sqlErrorOccurred = true;
                        SqlErrorMessage = se.Message;
                    }

                    _progressBar.Value = 50;

                    if (sqlErrorOccurred)
                    {
                        _lblSynchStatus.Text = "Error Purging Destination Database";
                        this.Refresh();

                        conn.Close();
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("An error occurred while purging the " + _cboDatabases.Text + "." + System.Environment.NewLine + SqlErrorMessage, "Database Synchronization Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    _lblSynchStatus.Text = "Copying Data to Destination Database";
                    this.Refresh();

                    try
                    {
                        SqlCommand copy_cmd = new SqlCommand("db_Copy", conn);
                        copy_cmd.CommandTimeout = 600;

                        copy_cmd.CommandType = CommandType.StoredProcedure;
                        copy_cmd.Parameters.AddWithValue("@SourceDBName", MainForm.WorkingDatabase.DatabaseName);
                        copy_cmd.ExecuteNonQuery();
                    }
                    catch (SqlException se)
                    {
                        sqlErrorOccurred = true;
                        SqlErrorMessage = se.Message;
                    }

                    conn.Close();

                    this.Cursor = Cursors.Default;

                    _progressBar.Value = 100;

                    if (sqlErrorOccurred)
                    {
                        _lblSynchStatus.Text = "Synchronization Failed";
                        this.Refresh();

                        MessageBox.Show("An error occurred while copying data from the " + MainForm.WorkingDatabase.FriendlyName + " to the " + _cboDatabases.Text + "." + System.Environment.NewLine + SqlErrorMessage, "Database Synchronization Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        _lblSynchStatus.Text = "Synchronization Successful";
                        this.Refresh();

                        MessageBox.Show("Successfully synchronized the " + _cboDatabases.Text + ".", "Synchronization Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        #endregion
    }
}