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
using CatalogEstimating.Datasets.AdministrationTableAdapters;
using CatalogEstimating.UserControls.VendorRates;
using CatalogEstimating.Properties;

#endregion

namespace CatalogEstimating.UserControls.Admin
{
    public partial class ucpAdminVendorsRates : CatalogEstimating.UserControlPanel
    {
        #region Private Variables

        private static readonly Dictionary<int, string> ControlMap = new Dictionary<int, string>();

        private VendorRateControl _rateControl = null;
        private Administration.vnd_vendorRow _currentVendor = null;
        private Administration.vnd_vendortypeRow _currentVendorType = null;

        private List<Administration.vnd_vendortypeRow> _lstVendorTypes = new List<Administration.vnd_vendortypeRow>();
        private List<Administration.vnd_vendorRow> _lstVendors = new List<Administration.vnd_vendorRow>();
        private IDictionary<DateTime, long> _dictEffectiveDates = null;

        private bool _loadingRate = false;

        #endregion

        #region Construction

        /// <summary>Static Constructor is purely used to build up a mapping of Vendor Type items
        /// that will appear in the combo box, and the class name that is used for the details control
        /// for that vendor type.</summary>
        static ucpAdminVendorsRates()
        {
            ControlMap.Add( 1, "vndPrinter" );
            ControlMap.Add( 2, "vndPaper" );
            ControlMap.Add( 5, "vndMailHouse" );
            ControlMap.Add( 6, "vndMailList" );
            ControlMap.Add( 7, "vndMailTracker" );
        }

        public ucpAdminVendorsRates()
        {
            InitializeComponent();
            Name = "Rates";
        }

        public ucpAdminVendorsRates( Administration ds )
        : this()
        {
            _dsAdministration = ds;
        }

        #endregion

        #region Override Methods

        protected override void OnDirtyChanged( object sender, EventArgs e )
        {
            if ( MainForm.AuthorizedUser.Right == UserRights.Admin ||
                 MainForm.AuthorizedUser.Right == UserRights.SuperAdmin )
            {
                _btnCancel.Enabled = Dirty;
            }
            base.OnDirtyChanged( sender, e );
        }

        public override ToolStrip Toolbar
        {
            get { return _toolStrip; }
        }

        public override void PreSave( CancelEventArgs e )
        {
            if ( ValidateChildren() )
                _btnUpdate_Click( this, EventArgs.Empty );
            else
                e.Cancel = true;

            base.PreSave( e );
        }

        public override void LoadData()
        {
            using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
            {
                _dsAdministration.vnd_mailhouserate.Clear();
                _dsAdministration.vnd_maillistresourcerate.Clear();
                _dsAdministration.vnd_mailtrackingrate.Clear();
                _dsAdministration.ppr_paper_map.Clear();
                _dsAdministration.vnd_paper.Clear();
                _dsAdministration.prt_printerrate.Clear();
                _dsAdministration.vnd_printer.Clear();
                _dsAdministration.ppr_papergrade.Clear();
                _dsAdministration.ppr_paperweight.Clear();

                // Reload the Vendor and VendorTypeMap tables, but do a merge so don't lose any changes
                // that they made in the other tab
                using ( vnd_vendorTableAdapter adapter = new vnd_vendorTableAdapter() )
                {
                    Administration.vnd_vendorDataTable tempVendor = new Administration.vnd_vendorDataTable();
                    adapter.Connection = conn;
                    adapter.Fill( tempVendor );
                    _dsAdministration.vnd_vendor.Merge( tempVendor, true );
                }

                using ( vnd_vendorvendortype_mapTableAdapter adapter = new vnd_vendorvendortype_mapTableAdapter() )
                {
                    Administration.vnd_vendorvendortype_mapDataTable tempMap = new Administration.vnd_vendorvendortype_mapDataTable();
                    adapter.Connection = conn;
                    adapter.Fill( tempMap );
                    _dsAdministration.vnd_vendorvendortype_map.Merge( tempMap, true );
                }

                // Now reload all the rate tables
                using ( vnd_mailhouserateTableAdapter adapter = new vnd_mailhouserateTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsAdministration.vnd_mailhouserate );
                }

                using ( vnd_maillistresourcerateTableAdapter adapter = new vnd_maillistresourcerateTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsAdministration.vnd_maillistresourcerate );
                }

                using ( vnd_mailtrackingrateTableAdapter adapter = new vnd_mailtrackingrateTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsAdministration.vnd_mailtrackingrate );
                }

                using ( vnd_paperTableAdapter adapter = new vnd_paperTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsAdministration.vnd_paper );
                }

                using ( vnd_printerTableAdapter adapter = new vnd_printerTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsAdministration.vnd_printer );
                }

                if ( _dsAdministration.prt_printerratetype.Count == 0 )
                {
                    using ( prt_printerratetypeTableAdapter adapter = new prt_printerratetypeTableAdapter() )
                    {
                        adapter.Connection = conn;
                        adapter.Fill( _dsAdministration.prt_printerratetype );
                    }
                }

                using ( prt_printerrateTableAdapter adapter = new prt_printerrateTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsAdministration.prt_printerrate );
                }

                using ( ppr_papergradeTableAdapter adapter = new ppr_papergradeTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsAdministration.ppr_papergrade );
                }

                using ( ppr_paperweightTableAdapter adapter = new ppr_paperweightTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsAdministration.ppr_paperweight );
                }

                using ( ppr_paper_mapTableAdapter adapter = new ppr_paper_mapTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsAdministration.ppr_paper_map );
                }
            }
            
            base.LoadData();
        }

        public override void SaveData()
        {
            bool bKnownSqlErrorOccurred = false;
            Exception SaveException = null;
            string SqlErrorMessage = string.Empty;

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                #region Define Adapters
                ppr_papergradeTableAdapter adapter_papergrade = new ppr_papergradeTableAdapter();
                ppr_paperweightTableAdapter adapter_paperweight = new ppr_paperweightTableAdapter();
                vnd_mailhouserateTableAdapter adapter_mailhouse = new vnd_mailhouserateTableAdapter();
                vnd_maillistresourcerateTableAdapter adapter_maillist = new vnd_maillistresourcerateTableAdapter();
                vnd_mailtrackingrateTableAdapter adapter_mailtracking = new vnd_mailtrackingrateTableAdapter();
                vnd_printerTableAdapter adapter_printer = new vnd_printerTableAdapter();
                prt_printerrateTableAdapter adapter_printerrate = new prt_printerrateTableAdapter();
                vnd_paperTableAdapter adapter_paper = new vnd_paperTableAdapter();
                ppr_paper_mapTableAdapter adapter_papermap = new ppr_paper_mapTableAdapter();
                #endregion

                #region Set SQL Connection on Adapters
                adapter_papergrade.Connection = conn;
                adapter_paperweight.Connection = conn;
                adapter_mailhouse.Connection = conn;
                adapter_maillist.Connection = conn;
                adapter_mailtracking.Connection = conn;
                adapter_printer.Connection = conn;
                adapter_printerrate.Connection = conn;
                adapter_paper.Connection = conn;
                adapter_papermap.Connection = conn;
                #endregion

                try
                {
                    adapter_papergrade.Update(_dsAdministration.ppr_papergrade);
                }
                catch (Exception e)
                {
                    SaveException = e;
                }
                finally
                {
                    adapter_papergrade.Dispose();
                }

                if (SaveException == null)
                {
                    try
                    {
                        adapter_paperweight.Update(_dsAdministration.ppr_paperweight);
                    }
                    catch (Exception e)
                    {
                        SaveException = e;
                    }
                    finally
                    {
                        adapter_paperweight.Dispose();
                    }
                }

                if (SaveException == null)
                {
                    SqlTransaction tran_mailhouse = null;

                    try
                    {
                        tran_mailhouse = conn.BeginTransaction();
                        adapter_mailhouse.SetTransaction(tran_mailhouse);
                        adapter_mailhouse.Update(_dsAdministration.vnd_mailhouserate.Select("", "", DataViewRowState.CurrentRows));
                        adapter_mailhouse.Update(_dsAdministration.vnd_mailhouserate.Select("", "", DataViewRowState.Deleted));
                        tran_mailhouse.Commit();
                    }
                    catch (SqlException se)
                    {
                        tran_mailhouse.Rollback();

                        if (se.Number == 50000)
                        {
                            bKnownSqlErrorOccurred = true;
                            SqlErrorMessage = se.Message;
                        }

                        SaveException = se;
                    }

                    catch (Exception e)
                    {
                        tran_mailhouse.Rollback();
                        SaveException = e;
                    }

                    finally
                    {
                        adapter_mailhouse.Dispose();
                        tran_mailhouse.Dispose();
                    }
                }

                if (SaveException == null)
                {
                    SqlTransaction tran_maillist = null;

                    try
                    {
                        tran_maillist = conn.BeginTransaction();
                        adapter_maillist.SetTransaction(tran_maillist);
                        adapter_maillist.Update(_dsAdministration.vnd_maillistresourcerate.Select("", "", DataViewRowState.CurrentRows));
                        adapter_maillist.Update(_dsAdministration.vnd_maillistresourcerate.Select("", "", DataViewRowState.Deleted));
                        tran_maillist.Commit();
                    }
                    catch (SqlException se)
                    {
                        tran_maillist.Rollback();

                        if (se.Number == 50000)
                        {
                            bKnownSqlErrorOccurred = true;
                            SqlErrorMessage = se.Message;
                        }

                        SaveException = se;
                    }

                    catch (Exception e)
                    {
                        tran_maillist.Rollback();
                        SaveException = e;
                    }

                    finally
                    {
                        adapter_maillist.Dispose();
                        tran_maillist.Dispose();
                    }
                }

                if (SaveException == null)
                {
                    SqlTransaction tran_mailtracking = null;
                    try
                    {
                        tran_mailtracking = conn.BeginTransaction();
                        adapter_mailtracking.SetTransaction(tran_mailtracking);
                        adapter_mailtracking.Update(_dsAdministration.vnd_mailtrackingrate.Select("", "", DataViewRowState.CurrentRows));
                        adapter_mailtracking.Update(_dsAdministration.vnd_mailtrackingrate.Select("", "", DataViewRowState.Deleted));
                        tran_mailtracking.Commit();
                    }
                    catch (SqlException se)
                    {
                        tran_mailtracking.Rollback();

                        if (se.Number == 50000)
                        {
                            bKnownSqlErrorOccurred = true;
                            SqlErrorMessage = se.Message;
                        }

                        SaveException = se;
                    }

                    catch (Exception e)
                    {
                        tran_mailtracking.Rollback();
                        SaveException = e;
                    }
                    finally
                    {
                        adapter_mailtracking.Dispose();
                        tran_mailtracking.Dispose();
                    }
                }

                if (SaveException == null)
                {
                    SqlTransaction tran_printer = null;
                    try
                    {
                        tran_printer = conn.BeginTransaction();
                        adapter_printer.SetTransaction(tran_printer);
                        adapter_printerrate.SetTransaction(tran_printer);

                        // VND_Printer table inserts and updates
                        adapter_printer.Update(_dsAdministration.vnd_printer.Select("", "", DataViewRowState.CurrentRows));
                        // PRT_PrinterRate table inserts
                        adapter_printerrate.Update(_dsAdministration.prt_printerrate.Select("", "", DataViewRowState.Added));

                        SqlCommand cmd_newPrinterRecords = new SqlCommand("Printer_u_ComponentandPolybag", conn);
                        cmd_newPrinterRecords.CommandTimeout = 600;
                        cmd_newPrinterRecords.CommandType = CommandType.StoredProcedure;
                        cmd_newPrinterRecords.Transaction = tran_printer;
                        cmd_newPrinterRecords.Parameters.AddWithValue("@ModifiedBy", MainForm.AuthorizedUser.FormattedName);
                        cmd_newPrinterRecords.ExecuteNonQuery();

                        foreach (DataRow row in _dsAdministration.vnd_printer.Select("", "", DataViewRowState.Deleted))
                        {
                            Datasets.Administration.vnd_printerRow prt_row = row as Datasets.Administration.vnd_printerRow;
                            long origPrinterID = (long)prt_row[_dsAdministration.vnd_printer.vnd_printer_idColumn, DataRowVersion.Original];


                            SqlCommand cmd_delPrinterRecords = new SqlCommand("Printer_u_ComponentandPolybag_ByPrinterID", conn);
                            cmd_delPrinterRecords.CommandTimeout = 600;
                            cmd_delPrinterRecords.CommandType = CommandType.StoredProcedure;
                            cmd_delPrinterRecords.Transaction = tran_printer;
                            cmd_delPrinterRecords.Parameters.AddWithValue("@VND_Printer_ID", origPrinterID);
                            cmd_delPrinterRecords.Parameters.AddWithValue("@ModifiedBy", MainForm.AuthorizedUser.FormattedName);
                            cmd_delPrinterRecords.ExecuteNonQuery();
                        }

                        adapter_printerrate.Update(_dsAdministration.prt_printerrate.Select("", "", DataViewRowState.ModifiedCurrent));
                        adapter_printerrate.Update(_dsAdministration.prt_printerrate.Select("", "", DataViewRowState.Deleted));
                        adapter_printer.Update(_dsAdministration.vnd_printer.Select("", "", DataViewRowState.Deleted));

                        tran_printer.Commit();
                    }
                    catch (SqlException se)
                    {
                        tran_printer.Rollback();

                        if (se.Number == 50000)
                        {
                            bKnownSqlErrorOccurred = true;
                            SqlErrorMessage = se.Message;
                        }

                        SaveException = se;
                    }
                    catch (Exception e)
                    {
                        tran_printer.Rollback();
                        SaveException = e;
                    }
                    finally
                    {
                        adapter_printer.Dispose();
                        adapter_printerrate.Dispose();
                        tran_printer.Dispose();
                    }
                }

                if (SaveException == null)
                {
                    SqlTransaction tran_paper = null;
                    try
                    {
                        // First setup transactions
                        tran_paper = conn.BeginTransaction();
                        adapter_paper.SetTransaction(tran_paper);
                        adapter_papermap.SetTransaction(tran_paper);


                        // VND_Paper table inserts and updates
                        adapter_paper.Update(_dsAdministration.vnd_paper.Select("", "", DataViewRowState.CurrentRows));
                        // PPR_Paper_Map table inserts
                        adapter_papermap.Update(_dsAdministration.ppr_paper_map.Select("", "", DataViewRowState.Added));

                        // Update any components that need to reference any of the new paper or paper map records
                        SqlCommand cmd_newPaperRecords = new SqlCommand("Paper_u_Component", conn);
                        cmd_newPaperRecords.CommandTimeout = 600;
                        cmd_newPaperRecords.CommandType = CommandType.StoredProcedure;
                        cmd_newPaperRecords.Transaction = tran_paper;
                        cmd_newPaperRecords.Parameters.AddWithValue("@ModifiedBy", MainForm.AuthorizedUser.FormattedName);
                        cmd_newPaperRecords.ExecuteNonQuery();

                        foreach (DataRow row in _dsAdministration.vnd_paper.Select("", "", DataViewRowState.Deleted))
                        {
                            Datasets.Administration.vnd_paperRow ppr_row = row as Datasets.Administration.vnd_paperRow;
                            long origPaperID = (long)ppr_row[_dsAdministration.vnd_paper.vnd_paper_idColumn, DataRowVersion.Original];

                            SqlCommand cmd_delPaperRecords = new SqlCommand("Paper_u_Component_ByPaperID", conn);
                            cmd_delPaperRecords.CommandTimeout = 600;
                            cmd_delPaperRecords.CommandType = CommandType.StoredProcedure;
                            cmd_delPaperRecords.Transaction = tran_paper;
                            cmd_delPaperRecords.Parameters.AddWithValue("@VND_Paper_ID", origPaperID);
                            cmd_delPaperRecords.Parameters.AddWithValue("@ModifiedBy", MainForm.AuthorizedUser.FormattedName);
                            cmd_delPaperRecords.ExecuteNonQuery();
                        }

                        adapter_papermap.Update(_dsAdministration.ppr_paper_map.Select("", "", DataViewRowState.ModifiedCurrent));
                        adapter_papermap.Update(_dsAdministration.ppr_paper_map.Select("", "", DataViewRowState.Deleted));
                        adapter_paper.Update(_dsAdministration.vnd_paper.Select("", "", DataViewRowState.Deleted));

                        tran_paper.Commit();
                    }
                    catch (SqlException se)
                    {
                        tran_paper.Rollback();

                        if (se.Number == 50000)
                        {
                            bKnownSqlErrorOccurred = true;
                            SqlErrorMessage = se.Message;
                        }

                        SaveException = se;
                    }
                    catch (Exception e)
                    {
                        tran_paper.Rollback();
                        SaveException = e;
                    }
                    finally
                    {
                        adapter_paper.Dispose();
                        adapter_papermap.Dispose();
                        tran_paper.Dispose();
                    }
                }
                
                conn.Close();
            }

            if (bKnownSqlErrorOccurred)
                MessageBox.Show(SqlErrorMessage, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (SaveException != null)
            {
                ExceptionForm formException = new ExceptionForm(SaveException);
                formException.ShowDialog();
            }
            else
            {
                base.SaveData();
                Reload();
            }
        }

        public override void Reload()
        {
            FillVendorList();
            _cboVendor_SelectedValueChanged( this, EventArgs.Empty );

            if ( MainForm.AuthorizedUser.Right == UserRights.Admin ||
                 MainForm.AuthorizedUser.Right == UserRights.SuperAdmin )
            {
                _btnCancel.Enabled = Dirty;
            }

            base.Reload();
        }

        public override void Export( ref ExcelWriter writer )
        {
            writer.WriteLine( _lblVendor.Text, _cboVendor.Text );
            writer.WriteLine( _lblVendorRateType.Text, _cboVendorRateType.Text );
            writer.WriteLine( _lblEffectiveDate.Text, _cboEffectiveDate.Text );
            writer.WriteLine();
            writer.WriteLine( _groupVendorRates.Text );

            if ( _rateControl != null )
                _rateControl.Export( writer );
        }

        public override void OnLeaving( CancelEventArgs e )
        {
            if ( _btnUpdate.Enabled )
            {
                DialogResult result = MessageBox.Show( Resources.LeaveTabCancelWarning, "Are you sure?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning );
                if ( result == DialogResult.Yes )
                {
                    LoadData();
                }
                else if ( result == DialogResult.No )
                {
                    if ( ValidateChildren() )
                        _btnUpdate_Click( this, EventArgs.Empty );
                    else
                        e.Cancel = true;
                }
                else if ( result == DialogResult.Cancel )
                    e.Cancel = true;
            }
            base.OnLeaving( e );
        }

        #endregion

        #region Private Methods

        private void ToggleDirty( bool dirty )
        {
            if ( MainForm.AuthorizedUser.Right == UserRights.Admin ||
                 MainForm.AuthorizedUser.Right == UserRights.SuperAdmin )
            {
                _btnUpdate.Enabled = dirty;

                _cboVendor.Enabled = !dirty;
                _cboVendorRateType.Enabled = !dirty;
                _cboEffectiveDate.Enabled = !dirty;
            }
        }

        private void FillEffectiveDates()
        {
            DateTime? selectedDate = null;
            if ( _rateControl != null )
                selectedDate = _rateControl.EffectiveDate;

            // Get the Effective Dates for this Vendor and VendorType
            _dictEffectiveDates = _rateControl.GetEffectiveDates();
            
            if ( _dictEffectiveDates.Count > 0 )
            {
                _cboEffectiveDate.DataSource    = new BindingSource( _dictEffectiveDates, null );
                _cboEffectiveDate.DisplayMember = "Key";
                _cboEffectiveDate.ValueMember   = "Value";
                _cboEffectiveDate.FormatString  = "d";
            }
            else
            {
                _cboEffectiveDate.DataSource = null;
                _groupVendorRates.Enabled = false;
            }

            _cboEffectiveDate.Enabled = true;

            if ( selectedDate == null )
                return;

            // Now reselect the correct one in the list
            for ( int index = 0; index < _cboEffectiveDate.Items.Count; index++ )
            {
                KeyValuePair<DateTime, long> pair = (KeyValuePair<DateTime, long>)_cboEffectiveDate.Items[index];
                if ( pair.Key == selectedDate.Value )
                {
                    _cboEffectiveDate.SelectedIndex = index;
                    break;
                }
            }
        }

        private void FillVendorList()
        {
            _cboVendor.DataSource = null;
            _lstVendors.Clear();

            foreach ( Administration.vnd_vendorRow vendorRow in _dsAdministration.vnd_vendor )
            {
                if ( !vendorRow.active )
                    continue;

                foreach ( Administration.vnd_vendorvendortype_mapRow vendorMapRow in vendorRow.Getvnd_vendorvendortype_mapRows() )
                {
                    if ( ControlMap.ContainsKey( vendorMapRow.vnd_vendortype_id ) )
                    {
                        _lstVendors.Add( vendorRow );
                        break;
                    }
                }
            }

            if ( _lstVendors.Count > 0 )
            {
                _lstVendors.Sort( SortVendors );
                _cboVendor.DataSource = _lstVendors;
                _cboVendor.DisplayMember = "description";
                _cboVendor.ValueMember = "vnd_vendor_id";
            }
        }

        #endregion

        #region Event Handlers

        private void ucpAdminVendorsRates_Load( object sender, EventArgs e )
        {
            if ( MainForm.AuthorizedUser.Right != UserRights.Admin &&
                 MainForm.AuthorizedUser.Right != UserRights.SuperAdmin )
            {
                _btnUpdate.Enabled = false;
                _btnNew.Enabled    = false;
                _btnCancel.Enabled = false;
                _btnDelete.Enabled = false;
            }
        }

        private void _cboVendor_SelectedValueChanged( object sender, EventArgs e )
        {
            // Clear the current list of Vendor Rates
            _cboVendorRateType.DataSource = null;
            _cboEffectiveDate.DataSource  = null;
            _lstVendorTypes.Clear();

            _lblInformational.Text = String.Empty;

            // Get the selected row
            if ( _cboVendor.SelectedItem != null )
            {
                _currentVendor = (Administration.vnd_vendorRow)_cboVendor.SelectedItem;

                foreach ( Administration.vnd_vendorvendortype_mapRow mapRow in _currentVendor.Getvnd_vendorvendortype_mapRows() )
                {
                    // Check to see if this is a flag type or if there are details that can be specified
                    if ( ControlMap.ContainsKey( mapRow.vnd_vendortype_id ) )
                        _lstVendorTypes.Add( mapRow.vnd_vendortypeRow );
                }

                _cboVendorRateType.Enabled = true;
                _lstVendorTypes.Sort( SortVendorRateTypes );
            }
            else
            {
                _currentVendor = null;
                _cboVendorRateType.Enabled = false;
                _cboEffectiveDate.Enabled  = false;
                _btnNew.Enabled = false;
            }

            _cboVendorRateType.DataSource    = _lstVendorTypes;
            _cboVendorRateType.DisplayMember = "description";

            ToggleDirty( false );
            _btnDelete.Enabled = false;

            // If for some strange reason there isn't an item selected, then select one
            if ( _cboVendorRateType.SelectedIndex == -1 && _cboVendorRateType.Items.Count > 0 )
                _cboVendorRateType.SelectedIndex = 0;

            _cboVendorRateType_SelectedValueChanged( this, EventArgs.Empty );
        }

        public static int SortVendors( Administration.vnd_vendorRow left, Administration.vnd_vendorRow right )
        {
            if ( left == null )
            {
                if ( right == null )
                    return 0;   // Both null so equal
                else
                    return -1;  // left is null and right is not so left is less than right
            }
            else
            {
                if ( right == null )
                    return 1;   // left is not null and right is null so left is greater than right
                else
                    return string.Compare( left.description, right.description );   // Do String Compare
            }
        }

        public static int SortVendorRateTypes( Administration.vnd_vendortypeRow left, Administration.vnd_vendortypeRow right )
        {
            if ( left == null )
            {
                if ( right == null )
                    return 0;   // Both null so equal
                else
                    return -1;  // left is null and right is not so left is less than right
            }
            else
            {
                if ( right == null )
                    return 1;   // left is not null and right is null so left is greater than right
                else
                    return string.Compare( left.description, right.description );   // Do String Compare
            }
        }

        private void _cboVendorRateType_SelectedValueChanged( object sender, EventArgs e )
        {
            _lblInformational.Text = string.Empty;

            if ( _cboVendorRateType.SelectedIndex != -1 )
            {
                _currentVendorType = (Administration.vnd_vendortypeRow)_cboVendorRateType.SelectedValue;
                string strControlType = string.Format( "CatalogEstimating.UserControls.VendorRates.{0}", ControlMap[_currentVendorType.vnd_vendortype_id] );

                bool createControl = false;

                // Dispose of the old control if there is one
                if ( _rateControl != null && 
                   ( _rateControl.GetType().FullName != strControlType ||
                     _rateControl.GetEffectiveDates().Count == 0 ||
                     _rateControl.Vendor.vnd_vendor_id != _currentVendor.vnd_vendor_id ) )
                {
                    _panelRateControl.Controls.Remove( _rateControl );
                    _rateControl.Dispose();
                    createControl = true;
                }
                else if ( _rateControl == null )
                    createControl = true;

                if ( createControl )
                {
                    // Create a new rate control for the newly selected vendor type
                    object[] constructorArgs = new object[] { _dsAdministration, _currentVendor };
                    _rateControl = (VendorRateControl)GetType().Assembly.CreateInstance( strControlType, false, System.Reflection.BindingFlags.CreateInstance, null, constructorArgs, null, null );
                    _rateControl.Dock = DockStyle.Fill;
                    _rateControl.ControlDirty += new EventHandler( _rateControl_ControlDirty );
                    _groupVendorRates.Text = _rateControl.Name;
                    _loadingRate = true;
                    _panelRateControl.Controls.Add( _rateControl );
                    _loadingRate = false;
                }

                FillEffectiveDates();

                // At this point they can add a new
                if ( MainForm.AuthorizedUser.Right == UserRights.Admin ||
                     MainForm.AuthorizedUser.Right == UserRights.SuperAdmin )
                {
                    _btnNew.Enabled = true;
                }

                // Force a selection on the effective date
                _cboEffectiveDate_SelectedValueChanged( this, EventArgs.Empty );
            }
            else
            {
                _currentVendorType = null;
                _cboEffectiveDate.Enabled = false;
                _btnNew.Enabled = false;
                _btnDelete.Enabled = false;
            }

            ToggleDirty( false );
        }

        private void _cboEffectiveDate_SelectedValueChanged( object sender, EventArgs e )
        {
            if ( _cboEffectiveDate.SelectedIndex != -1 )
            {
                _loadingRate = true;
                _rateControl.EditRate( (long)_cboEffectiveDate.SelectedValue );
                _loadingRate = false;

                _groupVendorRates.Enabled = true;
                
                if ( MainForm.AuthorizedUser.Right == UserRights.Admin ||
                     MainForm.AuthorizedUser.Right == UserRights.SuperAdmin )
                {
                    _btnDelete.Enabled = true;
                }
            }

            ToggleDirty( false );
        }

        private void _btnNew_Click( object sender, EventArgs e )
        {
            bool bDoNew = true;

            // If Update is Enabled, then there must be a pending change
            // Validate, and if valid, push the current data to the dataset
            // If invalid, then prevent the New
            if ( _btnUpdate.Enabled )
            {
                bDoNew = ValidateChildren();
                if ( bDoNew )
                    _btnUpdate_Click( this, EventArgs.Empty );
            }

            if ( bDoNew )
            {
                _rateControl.EditRate( null );
                _groupVendorRates.Enabled = true;
                _btnDelete.Enabled = false;
                Dirty = true;

                if ( _cboEffectiveDate.Items.Count == 0 )
                    _lblInformational.Text = string.Format( "New {0}", _cboVendorRateType.Text );
                else
                    _lblInformational.Text = string.Format( "{0} Rate Copied from {1} Effective Date: {2}",
                        _cboVendorRateType.Text, _cboVendor.Text, _cboEffectiveDate.Text );

                _rateControl.Focus();
            }
        }

        private void _btnUpdate_Click( object sender, EventArgs e )
        {
            if ( ValidateChildren() )
            {
                if ( _rateControl != null && _rateControl.Vendor != null )
                {
                    _rateControl.Save();

                    FillEffectiveDates();
                    _cboEffectiveDate_SelectedValueChanged( this, EventArgs.Empty );

                    ToggleDirty( false );
                    Dirty = true;

                    _lblInformational.Text = string.Empty;
                }
            }
        }

        private void _btnCancel_Click( object sender, EventArgs e )
        {
            DialogResult result = MessageBox.Show( Resources.CancelChangesWarning, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
            if ( result == DialogResult.Yes )
            {
                _rateControl.Cancel();
                LoadData();
                Reload();
                ToggleDirty( false );
                Dirty = false;
                _rateControl.Cancel();
            }
        }

        private void _btnDelete_Click( object sender, EventArgs e )
        {
            DialogResult result = MessageBox.Show( Resources.DeleteRateWarning, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
            if ( result == DialogResult.Yes )
            {
                _rateControl.Delete();
                Reload();
                ToggleDirty( false );
                _btnDelete.Enabled = false;
                Dirty = true;
            }
        }

        private void _rateControl_ControlDirty( object sender, EventArgs e )
        {
            if ( !_loadingRate )
            {
                ToggleDirty( true );
                Dirty = true;

                if ( _lblInformational.Text.Length == 0 )
                {
                    _lblInformational.Text = string.Format( "{0} Rate for {1} Effective Date: {2}", 
                        _cboVendorRateType.Text, _cboVendor.Text, _cboEffectiveDate.Text );
                }
            }
        }

        #endregion
    }
}