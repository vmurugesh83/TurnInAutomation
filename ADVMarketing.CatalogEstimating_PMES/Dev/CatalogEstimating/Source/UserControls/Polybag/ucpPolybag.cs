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
using CatalogEstimating.Datasets.PolybagGroupTableAdapters;
using CatalogEstimating.Properties;

#endregion

namespace CatalogEstimating.UserControls.Polybag
{
    public partial class ucpPolybag : CatalogEstimating.UserControlPanel
    {
        #region Private Variables

        private PolybagGroup.est_polybaggroupRow _polybagGroupRow = null;
        private Dictionary<long, PolybagEstimate> _dictCustomColumns = new Dictionary<long, PolybagEstimate>();
        private bool _bIsLoading  = false;
        private bool _bIsDeleting = false;

        #endregion

        #region Construction

        public ucpPolybag()
        {
            InitializeComponent();
        }

        #endregion

        #region Override Methods

        public override ToolStrip Toolbar
        {
            get { return _toolStrip; }
        }

        public override void LoadData()
        {
            // This is a lookup table.  Only have to fill it once
            if ( _dsPolybagGroup.prt_printerratetype.Rows.Count == 0 )
            {
                using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
                {
                    using ( prt_printerratetypeTableAdapter adapter = new prt_printerratetypeTableAdapter() )
                    {
                        adapter.Connection = conn;
                        adapter.Fill( _dsPolybagGroup.prt_printerratetype );
                    }

                    conn.Close();
                }
            }

            if ( _polybagGroupId == null )
            {
                CreatePolybagGroup();
                _btnDelete.Enabled = false;
            }
            else
            {
                LoadPolybagGroup();
                ChildForm childForm = (ChildForm)ParentForm;
                if ( !childForm.ReadOnly )
                    _btnDelete.Enabled = true;
            }

            base.LoadData();
        }

        public override void Reload()
        {
            _bIsLoading = true;
            bool tempDirty = Dirty;

            _txtDescription.Text = _polybagGroupRow.description;
            _txtComments.Text = _polybagGroupRow.comments;
            _chkUseMessage.Checked = _polybagGroupRow.usemessage;

            SetTitleBar();

            BindGrid();
            BindPackagePercent();
            BindPrinter();
            ToggleEstimateDependentControls();

            // Select the printer from the database
            if ( !_polybagGroupRow.Isvnd_printer_idNull() )
            {
                _cboPrinterVendor.SelectedValue = _polybagGroupRow.vnd_printer_id;

                // Fill in the possible rate values
                _cboPrinterVendor_SelectedValueChanged( this, EventArgs.Empty );

                // Select the proper rates from the database after the binding occurred.
                if ( !_polybagGroupRow.Isprt_bagrate_idNull() )
                    _cboBagRate.SelectedValue = _polybagGroupRow.prt_bagrate_id;
                else
                    _cboBagRate.SelectedIndex = -1;

                if ( !_polybagGroupRow.Isprt_bagmakereadyrate_idNull() )
                    _cboMakereadyRate.SelectedValue = _polybagGroupRow.prt_bagmakereadyrate_id;
                else
                    _cboMakereadyRate.SelectedIndex = -1;
            }

            _bIsLoading = false;
            Dirty = tempDirty;
        }

        #endregion

        #region Private Methods

        private void SetTitleBar()
        {
            if ( _polybagGroupRow.est_polybaggroup_id < 0 )
                ParentForm.Text = string.Format( "{0} {1} Group - New", Resources.ApplicationEnvironment, Resources.PolybagFormTitle );
            else
                ParentForm.Text = string.Format( "{0} {1} Group {2} - {3}", Resources.ApplicationEnvironment, Resources.PolybagFormTitle, _polybagGroupRow.est_polybaggroup_id, _polybagGroupRow.description );
        }

        private void LoadPolybagGroup()
        {
            _bIsLoading = true;

            using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
            {
                // Have to grab the rows in such a way that constraints will temprorarily be broken
                // Make sure that an exception isn't thrown until we're totally done
                _dsPolybagGroup.EnforceConstraints = false;

                // First grab the polybag group parent row
                using ( est_polybaggroupTableAdapter adapter = new est_polybaggroupTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.ClearBeforeFill = true;
                    adapter.Fill( _dsPolybagGroup.est_polybaggroup, _polybagGroupId.Value );
                }

                // Grab all the polybags that are a child of this group
                using ( est_polybagTableAdapter adapter = new est_polybagTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.ClearBeforeFill = true;
                    adapter.Fill( _dsPolybagGroup.est_polybag, _polybagGroupId.Value );
                }

                // Get the EstimatePolybagGroup map table records for this group
                using ( est_estimatepolybaggroup_mapTableAdapter adapter = new est_estimatepolybaggroup_mapTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.ClearBeforeFill = true;
                    adapter.Fill( _dsPolybagGroup.est_estimatepolybaggroup_map, _polybagGroupId.Value );
                }

                // Get the PackagePolybagMap table records for every polybag in the group
                _dsPolybagGroup.est_packagepolybag_map.Clear();
                using ( est_packagepolybag_mapTableAdapter adapter = new est_packagepolybag_mapTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.ClearBeforeFill = false;
                    foreach ( PolybagGroup.est_polybagRow row in _dsPolybagGroup.est_polybag.Rows )
                        adapter.Fill( _dsPolybagGroup.est_packagepolybag_map, row.est_polybag_id );
                }

                // Get all the estimates that are references by this polybag group
                _dsPolybagGroup.est_estimate.Clear();
                using ( est_estimateTableAdapter adapter = new est_estimateTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.ClearBeforeFill = false;
                    foreach ( PolybagGroup.est_estimatepolybaggroup_mapRow row in _dsPolybagGroup.est_estimatepolybaggroup_map.Rows )
                        adapter.Fill( _dsPolybagGroup.est_estimate, row.est_estimate_id );
                }

                // Finally get all the packages for all of the estimates
                _dsPolybagGroup.est_package.Clear();

                // Add an "empty" package so users can unselect a package
                _dsPolybagGroup.est_package.Addest_packageRow(_dsPolybagGroup.est_package.Newest_packageRow());

                using ( est_packageTableAdapter adapter = new est_packageTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.ClearBeforeFill = false;
                    foreach ( PolybagGroup.est_estimateRow row in _dsPolybagGroup.est_estimate.Rows )
                        adapter.Fill( _dsPolybagGroup.est_package, row.est_estimate_id );
                }

                using ( PostalScenario_s_PostalScenarioIDandDescription_ByRunDateTableAdapter adapter = new PostalScenario_s_PostalScenarioIDandDescription_ByRunDateTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.ClearBeforeFill = true;
                    adapter.Fill( _dsPolybagGroup.PostalScenario_s_PostalScenarioIDandDescription_ByRunDate, GetRunDate() );
                }

                // Should be all done with our select statement now.  Make sure everything looks good
                _dsPolybagGroup.EnforceConstraints = true;
                conn.Close();
            }

            _polybagGroupRow = (PolybagGroup.est_polybaggroupRow)_dsPolybagGroup.est_polybaggroup.Rows[0];

            _bIsLoading = false;
        }

        private void CreatePolybagGroup()
        {
            _polybagGroupRow = _dsPolybagGroup.est_polybaggroup.Newest_polybaggroupRow();
            _polybagGroupRow.usemessage  = false;
            _polybagGroupRow.description = string.Empty;
            _polybagGroupRow.comments    = string.Empty;
            _polybagGroupRow.createdby   = MainForm.AuthorizedUser.FormattedName;
            _polybagGroupRow.createddate = DateTime.Now;
            _dsPolybagGroup.est_polybaggroup.Addest_polybaggroupRow( _polybagGroupRow );
        }

        private void BindGrid()
        {
            int insertIndex = 1;
            foreach ( PolybagGroup.est_estimateRow estimateRow in _dsPolybagGroup.est_estimate )
            {
                if ( _dictCustomColumns.ContainsKey( estimateRow.est_estimate_id ) )
                {
                    insertIndex += 2;
                    continue;
                }

                // Create the DataVew that we'll bind the package combo box against
                DataView packageView  = new DataView( _dsPolybagGroup.est_package );
                packageView.RowFilter = "est_package_id = -1 or est_estimate_id = " + estimateRow.est_estimate_id;
                packageView.Sort      = "description ASC";

                // First create the ComboBox Column for the packages
                DataGridViewComboBoxColumn packageColumn = new DataGridViewComboBoxColumn();
                packageColumn.HeaderText    = "Package";
                packageColumn.Name          = "Package" + estimateRow.est_estimate_id;
                packageColumn.Width         = 80;
                packageColumn.DataSource    = packageView;
                packageColumn.DisplayMember = "description";
                packageColumn.ValueMember   = "est_package_id";
                _gridPolybag.Columns.Insert( insertIndex++, packageColumn );

                // Create the TextBox Column for the percentages
                DecimalColumn percentColumn    = new DecimalColumn();
                percentColumn.HeaderText       = "Percent";
                percentColumn.Name             = "Percent" + estimateRow.est_estimate_id;
                percentColumn.Width            = 80;
                percentColumn.AllowNegative    = false;
                percentColumn.DecimalPrecision = 2;
                percentColumn.DefaultCellStyle.BackColor = SystemColors.Control;
                percentColumn.DefaultCellStyle.Format    = "N2";
                percentColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                _gridPolybag.Columns.Insert( insertIndex++, percentColumn );

                // Finally Create the Label that goes over these two columns
                Label columnHeader            = new Label();
                columnHeader.Tag              = estimateRow.est_estimate_id;
                columnHeader.Text             = estimateRow.description;
                columnHeader.Height           = 23;
                columnHeader.TextAlign        = ContentAlignment.MiddleCenter;
                columnHeader.BackColor        = SystemColors.Control;
                columnHeader.BorderStyle      = BorderStyle.FixedSingle;
                columnHeader.Top              = _gridPolybag.Top - columnHeader.Height;
                columnHeader.ContextMenuStrip = _menuColumnHeader;
                _toolTipProvider.SetToolTip( columnHeader, "Estimate " + estimateRow.est_estimate_id );
                Controls.Add( columnHeader );
                
                // Create a Control Structure to help me group the new columns and labels together
                // into logical units
                PolybagEstimate pbEstimate = new PolybagEstimate();
                pbEstimate.EstimateId      = estimateRow.est_estimate_id;
                pbEstimate.Package         = packageColumn;
                pbEstimate.PackageView     = packageView;
                pbEstimate.Percent         = percentColumn;
                pbEstimate.HeaderLabel     = columnHeader;
                _dictCustomColumns.Add( estimateRow.est_estimate_id, pbEstimate );
            }

            ResizeLabels();
            CalculateTotal();
        }

        private void BindPackagePercent()
        {
            foreach ( DataGridViewRow viewRow in _gridPolybag.Rows )
            {
                if ( viewRow.IsNewRow )
                    continue;

                bool percentRequired = false;
                long polybagId = Convert.ToInt64( viewRow.Cells[estpolybagidDataGridViewTextBoxColumn.Index].FormattedValue );

                foreach ( PolybagGroup.est_packagepolybag_mapRow mapRow in _dsPolybagGroup.est_packagepolybag_map.Select( "est_polybag_id = " + polybagId ) )
                {
                    PolybagEstimate controlGroup = _dictCustomColumns[mapRow.est_packageRow.est_estimate_id];
                    viewRow.Cells[controlGroup.Package.Index].Value = mapRow.est_package_id;

                    percentRequired = !mapRow.IsdistributionpctNull();
                    if (percentRequired)
                    {
                        viewRow.Cells[controlGroup.Percent.Index].Value = mapRow.distributionpct * 100M;    // %
                        viewRow.Cells[controlGroup.Percent.Index].Style.BackColor = SystemColors.Window;
                    }
                }

                viewRow.Cells[AllocateByPercent.Index].Value = percentRequired;
            }
        }

        private void BindPrinter()
        {
            // Save the currently selected printer vendor so doesn't get blown away
            string strCurrentPrinter = null;
            if ( _cboPrinterVendor.SelectedIndex >= 0 )
                strCurrentPrinter = _cboPrinterVendor.Text;

            DateTime? dtRunDate = GetRunDate();
            if ( dtRunDate == null )
                _cboPrinterVendor.DataSource = null;
            else
            {
                using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
                {
                    // First grab all the valid printers
                    using ( Printer_s_PrinterIDandDescription_ByRunDateTableAdapter adapter = new Printer_s_PrinterIDandDescription_ByRunDateTableAdapter() )
                    {
                        adapter.Connection = conn;
                        adapter.ClearBeforeFill = true;
                        if (_polybagGroupRow.Isvnd_printer_idNull())
                            adapter.Fill(_dsPolybagGroup.Printer_s_PrinterIDandDescription_ByRunDate, null, dtRunDate);
                        else
                            adapter.Fill( _dsPolybagGroup.Printer_s_PrinterIDandDescription_ByRunDate, _polybagGroupRow.vnd_printer_id, dtRunDate );
                    }

                    conn.Close();
                }

                if ( _cboPrinterVendor.DataSource == null )
                {
                    printersPrinterIDandDescriptionByRunDateBindingSource.DataSource = _dsPolybagGroup;
                    _cboPrinterVendor.DataSource    = printersPrinterIDandDescriptionByRunDateBindingSource;
                    _cboPrinterVendor.DisplayMember = "Description";
                    _cboPrinterVendor.ValueMember   = "VND_Printer_ID";
                }
            }

            // Now reselect the old vendor
            if ( strCurrentPrinter != null )
            {
                foreach ( DataRowView viewVendor in _cboPrinterVendor.Items )
                {
                    PolybagGroup.Printer_s_PrinterIDandDescription_ByRunDateRow row = (PolybagGroup.Printer_s_PrinterIDandDescription_ByRunDateRow)viewVendor.Row;
                    if ( row.Description == strCurrentPrinter )
                    {
                        _cboPrinterVendor.SelectedValue = row.VND_Printer_ID;
                        break;
                    }
                }
            }

            _cboPrinterVendor_SelectedValueChanged( this, EventArgs.Empty );
        }

        private void RefreshPackageLists()
        {
            _dsPolybagGroup.EnforceConstraints = false;

            // Clear my package list
            _dsPolybagGroup.est_package.Clear();

            // Add an "empty" package so users can unselect a package
            _dsPolybagGroup.est_package.Addest_packageRow(_dsPolybagGroup.est_package.Newest_packageRow());

            using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
            {
                // Refresh my list of all the packages for all the estimates currently added
                using ( est_packageTableAdapter adapter = new est_packageTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.ClearBeforeFill = false;
                    foreach ( PolybagEstimate controlGroup in _dictCustomColumns.Values )
                        adapter.Fill( _dsPolybagGroup.est_package, controlGroup.EstimateId );
                }

                conn.Close();
            }

            // Lastly, some packages could have been deleted that maps exist for.  Find them
            // and remove them before enforcing constraints
            for ( int iMapRow = _dsPolybagGroup.est_packagepolybag_map.Count - 1; iMapRow >= 0; iMapRow-- )
            {
                PolybagGroup.est_packagepolybag_mapRow mapRow = (PolybagGroup.est_packagepolybag_mapRow)_dsPolybagGroup.est_packagepolybag_map[iMapRow];

                // If the package in this map no longer exists, then delete the mapping
                if ( _dsPolybagGroup.est_package.FindByest_package_id( mapRow.est_package_id ) == null )
                    mapRow.Delete();
            }

            // Can just accept these changes right away
            _dsPolybagGroup.est_packagepolybag_map.AcceptChanges();
            _dsPolybagGroup.EnforceConstraints = true;

            // Refresh those settings on the grid
            BindPackagePercent();
        }

        private void ResizeLabels()
        {
            int offset = _gridPolybag.Bounds.Left - 1;

            // Move the Quantity Total Text Box appropriately
            Rectangle rectQuantityCol = _gridPolybag.GetColumnDisplayRectangle( Quantity.Index, false );
            _txtTotal.Left  = rectQuantityCol.Left  + offset;
            _txtTotal.Width = rectQuantityCol.Width + 1;
            _lblTotal.Left  = _txtTotal.Left - _lblTotal.Width - 8;

            // Appropriately resize each of the labels over the columns in the grid
            foreach ( PolybagEstimate controlStruct in _dictCustomColumns.Values )
            {
                Rectangle pkgRect = _gridPolybag.GetColumnDisplayRectangle( controlStruct.Package.Index, false );
                Rectangle pctRect = _gridPolybag.GetColumnDisplayRectangle( controlStruct.Percent.Index, false );

                controlStruct.HeaderLabel.Left  = pkgRect.Left  + offset;
                controlStruct.HeaderLabel.Width = pkgRect.Width + pctRect.Width + 1;
            }
        }

        private void CalculateTotal()
        {
            int total = 0;
            foreach ( DataGridViewRow rowView in _gridPolybag.Rows )
            {
                if ( !rowView.IsNewRow )
                {
                    int quantity = Utilities.Convert<int>( rowView.Cells["Quantity"].EditedFormattedValue.ToString() );
                    total += quantity;
                }
            }

            _txtTotal.Value = total;
        }

        private DateTime? GetRunDate()
        {
            if ( _dsPolybagGroup.est_estimate.Rows.Count == 0 )
                return null;
            else
            {
                DateTime runDate = DateTime.MinValue;
                foreach ( PolybagGroup.est_estimateRow estRow in _dsPolybagGroup.est_estimate.Rows )
                {
                    if ( estRow.rundate > runDate )
                        runDate = estRow.rundate;
                }
                return runDate;
            }
        }

        private void CheckReadOnly()
        {
            ChildForm childForm = (ChildForm)ParentForm;
            if ( childForm.ReadOnly )
            {
                _btnNew.Enabled = false;
                _btnSave.Enabled = false;
                _btnDelete.Enabled = false;
                _btnUpload.Enabled = false;

                _txtDescription.ReadOnly = true;
                _txtComments.ReadOnly = true;
                _cboPrinterVendor.Enabled = false;
                _cboMakereadyRate.Enabled = false;
                _cboBagRate.Enabled = false;
                _chkUseMessage.Enabled = false;

                _gridPolybag.ReadOnly = true;
                _gridPolybag.AllowUserToAddRows = false;
                _gridPolybag.AllowUserToDeleteRows = false;
                _gridPolybag.DefaultCellStyle.BackColor = SystemColors.Control;
            }
        }

        private void RecreateMaps()
        {
            _dsPolybagGroup.est_estimatepolybaggroup_map.Clear();
            _dsPolybagGroup.est_estimatepolybaggroup_map.AcceptChanges();

            foreach ( PolybagEstimate controlGroup in _dictCustomColumns.Values )
            {
                PolybagGroup.est_estimatepolybaggroup_mapRow newMapRow = _dsPolybagGroup.est_estimatepolybaggroup_map.Newest_estimatepolybaggroup_mapRow();
                newMapRow.est_estimate_id = controlGroup.EstimateId;
                newMapRow.est_polybaggroup_id = _polybagGroupRow.est_polybaggroup_id;
                newMapRow.createdby = MainForm.AuthorizedUser.FormattedName;
                newMapRow.createddate = DateTime.Now;
                newMapRow.estimateorder = _dsPolybagGroup.est_estimatepolybaggroup_map.Rows.Count + 1;
                _dsPolybagGroup.est_estimatepolybaggroup_map.Addest_estimatepolybaggroup_mapRow( newMapRow );
            }
            
            PolybagGroup cloneDataset = new PolybagGroup();
            cloneDataset.EnforceConstraints = false;

            foreach ( PolybagGroup.est_packagepolybag_mapRow origRow in _dsPolybagGroup.est_packagepolybag_map.Rows )
            {
                if ( origRow.RowState == DataRowState.Deleted )
                    continue;

                PolybagGroup.est_packagepolybag_mapRow cloneRow = cloneDataset.est_packagepolybag_map.Newest_packagepolybag_mapRow();
                cloneRow.createdby = MainForm.AuthorizedUser.FormattedName;
                cloneRow.createddate = DateTime.Now;
                cloneRow.est_package_id = origRow.est_package_id;
                cloneRow.est_polybag_id = origRow.est_polybag_id;

                if ( origRow.IsdistributionpctNull() )
                    cloneRow.SetdistributionpctNull();
                else
                    cloneRow.distributionpct = origRow.distributionpct;

                cloneDataset.est_packagepolybag_map.Addest_packagepolybag_mapRow(cloneRow);
            }

            _dsPolybagGroup.est_packagepolybag_map.Clear();
            _dsPolybagGroup.est_packagepolybag_map.AcceptChanges();

            _dsPolybagGroup.est_packagepolybag_map.Merge(cloneDataset.est_packagepolybag_map);
        }

        private bool FoundKilled()
        {
            // Double check to ensure that the user isn't trying to create a polybag group with
            // any killed estimates
            using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
            {
                conn.Open();

                SqlCommand searchCmd = new SqlCommand();
                searchCmd.CommandTimeout = 7200;
                searchCmd.Connection = conn;
                searchCmd.CommandType = CommandType.StoredProcedure;
                searchCmd.CommandText = "EstEstimate_s_Search";

                SqlParameter estIdParam = new SqlParameter( "@EstEstimateId", SqlDbType.BigInt );
                searchCmd.Parameters.Add( estIdParam );

                foreach ( long estimateId in _dictCustomColumns.Keys )
                {
                    searchCmd.Parameters["@EstEstimateId"].Value = estimateId;
                    using ( SqlDataReader searchDr = searchCmd.ExecuteReader() )
                    {
                        searchDr.Read();
                        bool bFoundKilled = false;
                        if ( searchDr["StatusDesc"].ToString() == "Killed" )
                        {
                            bFoundKilled = true;
                            MessageBox.Show( string.Format( "Cannot Save Polybag Group with Killed Estimate {0}", estimateId ) );
                        }
                        searchDr.Close();

                        if ( bFoundKilled )
                            return true;
                    }
                }
            }

            return false;
        }

        private void ToggleEstimateDependentControls()
        {
            bool bEnabled = ( _dictCustomColumns.Count > 0 );
            _cboPrinterVendor.Enabled = bEnabled;
            _cboBagRate.Enabled       = bEnabled;
            _cboMakereadyRate.Enabled = bEnabled;
            _chkUseMessage.Enabled    = bEnabled;
        }

        #endregion

        #region Public Methods

        private long? _polybagGroupId = null;
        public long? PolybagGroupId
        {
            get { return _polybagGroupRow.est_polybaggroup_id; }
            set { _polybagGroupId = value; }
        }

        public bool AddEstimate( long estimateId )
        {
            DateTime? beforeDate = GetRunDate();

            // Make sure this estimate isn't already referenced by this polybag group
            if ( _dictCustomColumns.ContainsKey( estimateId ) )
                return false;

            _bIsLoading = true;     // Prevent Validation For Now
            _gridPolybag.EndEdit();

            // Create a new map row to link this estimate to this polybag group
            PolybagGroup.est_estimatepolybaggroup_mapRow newMapRow = _dsPolybagGroup.est_estimatepolybaggroup_map.Newest_estimatepolybaggroup_mapRow();
            newMapRow.est_estimate_id     = estimateId;
            newMapRow.est_polybaggroup_id = _polybagGroupRow.est_polybaggroup_id;
            newMapRow.createdby           = MainForm.AuthorizedUser.FormattedName;
            newMapRow.createddate         = DateTime.Now;
            newMapRow.estimateorder       = _dsPolybagGroup.est_estimatepolybaggroup_map.Rows.Count + 1;

            using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
            {
                // Get the estimate for the ID passed in
                using ( est_estimateTableAdapter adapter = new est_estimateTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.ClearBeforeFill = false;
                    adapter.Fill( _dsPolybagGroup.est_estimate, estimateId );
                }

                // Get all the packages for all of the estimates
                using ( est_packageTableAdapter adapter = new est_packageTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.ClearBeforeFill = false;
                    adapter.Fill( _dsPolybagGroup.est_package, estimateId );
                }

                // Update the postal scenario combo box if the date changed
                DateTime? afterDate = GetRunDate();
                if ( beforeDate != afterDate )
                {
                    using ( PostalScenario_s_PostalScenarioIDandDescription_ByRunDateTableAdapter adapter = new PostalScenario_s_PostalScenarioIDandDescription_ByRunDateTableAdapter() )
                    {
                        adapter.Connection = conn;
                        adapter.ClearBeforeFill = true;
                        adapter.Fill( _dsPolybagGroup.PostalScenario_s_PostalScenarioIDandDescription_ByRunDate, afterDate );
                    }
                }

                conn.Close();
            }

            // Finally add the map row to map this estimate to this polybag group
            _dsPolybagGroup.est_estimatepolybaggroup_map.Addest_estimatepolybaggroup_mapRow( newMapRow );

            BindGrid();
            BindPrinter();
            ToggleEstimateDependentControls();

            _bIsLoading = false;
            Dirty = true;
            return true;
        }

        #endregion

        #region Event Handlers

        private void ucpPolybag_Resize( object sender, EventArgs e )
        {
            _txtTotal.Top = Bottom - 8 - _txtTotal.Height;
            _lblTotal.Top = _txtTotal.Top + (_txtTotal.Height - _lblTotal.Height ) / 2;

            ResizeLabels();
        }

        private void _gridPolybag_ColumnWidthChanged( object sender, DataGridViewColumnEventArgs e )
        {
            ResizeLabels();
        }

        private void _menuRemoveEstimate_Click( object sender, EventArgs e )
        {
            DateTime? beforeDate = GetRunDate();

            // Make sure to cancel out of any edits before trying to modify the grid
            _bIsLoading = true;     // Prevent Validation For Now
            _gridPolybag.EndEdit();

            // Get the estimate ID they want to remove
            Label headerLabel = (Label)_menuColumnHeader.SourceControl;
            long estimateId   = (long)headerLabel.Tag;
            
            // First remove the columns from the grid and the header label
            PolybagEstimate controlStruct = _dictCustomColumns[estimateId];
            _gridPolybag.Columns.Remove( controlStruct.Package );
            _gridPolybag.Columns.Remove( controlStruct.Percent );
            Controls.Remove( controlStruct.HeaderLabel );

            // Then remove the package mappings from the database, and the packages from the dataset
            for ( int iPkgRow = _dsPolybagGroup.est_package.Count -1; iPkgRow >= 0; iPkgRow-- )
            {
                PolybagGroup.est_packageRow packageRow = (PolybagGroup.est_packageRow)_dsPolybagGroup.est_package[iPkgRow];
                if (packageRow.est_estimateRow == null)
                    continue;
                if (packageRow.est_estimate_id != estimateId)
                    continue;

                // Delete all the packages in this estimate from any polybags
                foreach ( PolybagGroup.est_packagepolybag_mapRow pkgPolybagMapRow in packageRow.Getest_packagepolybag_mapRows() )
                    pkgPolybagMapRow.Delete();

                // This will remove this memory copy of the row from the dataset, but not
                // commit the change to the database.
                packageRow.Delete();
                packageRow.AcceptChanges();
            }

            // Now remove the estimate mapping from the database
            _dsPolybagGroup.est_estimatepolybaggroup_map.FindByest_estimate_idest_polybaggroup_id( estimateId, _polybagGroupRow.est_polybaggroup_id ).Delete();

            // This will remove this memory copy of the row from the dataset, but not
            // commit the change to the database.
            PolybagGroup.est_estimateRow estimateRow = _dsPolybagGroup.est_estimate.FindByest_estimate_id( estimateId );
            estimateRow.Delete();
            estimateRow.AcceptChanges();

            // Remove my control structure for the items I just removed
            _dictCustomColumns.Remove( estimateId );

            // Update the postal scenario combo box if the date changed
            DateTime? afterDate = GetRunDate();
            if ( beforeDate != afterDate )
            {
                using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
                {
                    using ( PostalScenario_s_PostalScenarioIDandDescription_ByRunDateTableAdapter adapter = new PostalScenario_s_PostalScenarioIDandDescription_ByRunDateTableAdapter() )
                    {
                        adapter.Connection = conn;
                        adapter.ClearBeforeFill = true;
                        adapter.Fill( _dsPolybagGroup.PostalScenario_s_PostalScenarioIDandDescription_ByRunDate, afterDate );
                    }

                    conn.Close();
                }
            }

            ResizeLabels();

            // If this was the last estimate, then clear the Printer Rate Combos
            if ( _dictCustomColumns.Count == 0 )
            {
                _dsPolybagGroup.Printer_s_PrinterIDandDescription_ByRunDate.Clear();
                _dsPolybagGroup.prt_printerrate.Clear();
            }

            ToggleEstimateDependentControls();

            _bIsLoading = false;

            Dirty = true;

            // Now validate the grid
            ValidateChildren();
        }

        private void _btnPrint_Click( object sender, EventArgs e )
        {
            ExcelWriter writer = null;
            try
            {
                writer = new ExcelWriter();

                // Write out General Polybag Group info
                writer.WriteLine( _lblDescription.Text, _txtDescription.Text );
                writer.WriteLine( _lblComments.Text, _txtComments.Text );
                writer.WriteLine( _lblPrinterVendor.Text, _cboPrinterVendor.Text );
                writer.WriteLine( _lblBagRate.Text, _cboBagRate.Text );
                writer.WriteLine( _lblMakereadyRate.Text, _cboMakereadyRate.Text );

                // Message data
                writer.WriteLine( _chkUseMessage.Text, _chkUseMessage.Checked );
                writer.WriteLine( _lblMessageRate.Text, _txtMessageRate.Text );
                writer.WriteLine( _lblMessageMakereadyRate.Text, _txtMessageMakereadyRate.Text );

                writer.WriteLine();

                // Polybag Data
                writer.WriteTable( _gridPolybag, true );

                // Make sure the Total Row lines up under quantity
                string[] writerParams = new string[_dictCustomColumns.Count * 2 + 3];
                writerParams[writerParams.Length - 2] = _lblTotal.Text;
                writerParams[writerParams.Length - 1] = _txtTotal.Text;
                writer.WriteLine( writerParams );

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

        private void _btnNew_Click( object sender, EventArgs e )
        {
            if ( Dirty )
            {
                DialogResult result = MessageBox.Show( Resources.CancelChangesWarning, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
                if ( result == DialogResult.No )
                    return;     // Don't clear the screen after all
            }

            _gridPolybag.EndEdit();
            _bIsLoading = true;

            // Create a new dataset and rebind to the correct controls
            _dsPolybagGroup = new PolybagGroup();
            printersPrinterIDandDescriptionByRunDateBindingSource.DataSource = _dsPolybagGroup;
            postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource.DataSource = _dsPolybagGroup;
            estpolybagBindingSource.DataSource = _dsPolybagGroup;

            // Remove the custom columns for any existing estimates
            foreach ( PolybagEstimate controlStruct in _dictCustomColumns.Values )
            {
                _gridPolybag.Columns.Remove( controlStruct.Package );
                _gridPolybag.Columns.Remove( controlStruct.Percent );
                Controls.Remove( controlStruct.HeaderLabel );
            }

            // Default my other internal collections
            _dictCustomColumns = new Dictionary<long, PolybagEstimate>();
            _polybagGroupId    = null;

            _cboBagRate.DataSource       = null;
            _cboMakereadyRate.DataSource = null;
            _bIsLoading = false;

            LoadData();
            Reload();
        }

        private void _btnRefresh_Click( object sender, EventArgs e )
        {
            if ( Dirty )
            {
                DialogResult result = MessageBox.Show( Resources.CancelChangesWarning, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
                if ( result == DialogResult.No )
                    return;     // Don't clear the screen after all
            }

            _gridPolybag.EndEdit();
            _bIsLoading = true;

            // Remove the custom columns for any existing estimates
            foreach ( PolybagEstimate controlStruct in _dictCustomColumns.Values )
            {
                _gridPolybag.Columns.Remove( controlStruct.Package );
                _gridPolybag.Columns.Remove( controlStruct.Percent );
                Controls.Remove( controlStruct.HeaderLabel );
            }
            _dictCustomColumns = new Dictionary<long, PolybagEstimate>();

            // If this was a new, then simply create a new row for them again
            if ( _polybagGroupRow.RowState == DataRowState.Added )
            {
                _polybagGroupId = null;
                _dsPolybagGroup = new PolybagGroup();
                printersPrinterIDandDescriptionByRunDateBindingSource.DataSource = _dsPolybagGroup;
                postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource.DataSource = _dsPolybagGroup;
                estpolybagBindingSource.DataSource = _dsPolybagGroup;
                _cboBagRate.DataSource = null;
                _cboMakereadyRate.DataSource = null;
            }
            _bIsLoading = false;

            LoadData();
            Reload();

            Dirty = false;
        }

        public void _btnSave_Click( object sender, EventArgs e )
        {
            _gridPolybag.EndEdit();

            Database db = MainForm.WorkingDatabase.Database;

            // Check to see if this Polybag has been deleted behind my back
            if ( _polybagGroupRow.RowState != DataRowState.Added )
            {
                if ( !PolybagEditForm.PolybagGroupExists( _polybagGroupRow.est_polybaggroup_id ) )
                {
                    MessageBox.Show( "Polybag Group Has Been Deleted by Another User.  Cannot Save." );
                    ChildForm childForm = (ChildForm)ParentForm;
                    childForm.ReadOnly = true;
                    CheckReadOnly();
                    return;
                }
            }

            // Cannot save a polybag group with less than two estimates in it
            if ( _dictCustomColumns.Count < 2 )
            {
                MessageBox.Show( Resources.NotEnoughEstimatesError, "Save" );
                return;
            }

            // Validate all the sub controls
            if ( !ParentForm.ValidateChildren() )
                return;

            if ( FoundKilled() )
                return;

            // First persist all the basic data from the polybag fields to the dataset
            _polybagGroupRow.description             = _txtDescription.Text;
            _polybagGroupRow.comments                = _txtComments.Text;
            _polybagGroupRow.usemessage              = _chkUseMessage.Checked;
            _polybagGroupRow.vnd_printer_id          = (long)_cboPrinterVendor.SelectedValue;
            _polybagGroupRow.prt_bagrate_id          = (long)_cboBagRate.SelectedValue;
            _polybagGroupRow.prt_bagmakereadyrate_id = (long)_cboMakereadyRate.SelectedValue;

            if ( _polybagGroupRow.RowState != DataRowState.Added )
            {
                _polybagGroupRow.modifiedby   = MainForm.AuthorizedUser.FormattedName;
                _polybagGroupRow.modifieddate = DateTime.Now;
            }

            // Clear and recreate all my map rows in the dataset for a fresh re-insert
            RecreateMaps();

            // First do all the deletes on the child map tables
            using ( SqlConnection conn = (SqlConnection)db.CreateConnection() )
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    // First clear both map tables, because I'm doing to just re-insert into them
                    if ( _polybagGroupRow.RowState != DataRowState.Added )
                    {
                        using ( SqlCommand cmdClearMaps = new SqlCommand( "EstPolybagGroup_ClearMaps_ByPolybagGroupId", conn ) )
                        {
                            cmdClearMaps.CommandTimeout = 7200;
                            cmdClearMaps.CommandType = CommandType.StoredProcedure;
                            cmdClearMaps.Transaction = tran;
                            cmdClearMaps.Parameters.AddWithValue( "@est_polybaggroup_id", _polybagGroupRow.est_polybaggroup_id );
                            cmdClearMaps.ExecuteNonQuery();
                        }
                    }

                    // Clean up any rows that were added by the DataGridView that shouldn't have been
                    PolybagGroup.est_polybagRow[] addedRows = (PolybagGroup.est_polybagRow[])_dsPolybagGroup.est_polybag.Select( "", "", DataViewRowState.Added );
                    for ( int iAdded = addedRows.Length - 1; iAdded >= 0; iAdded-- )
                    {
                        if ( !addedRows[iAdded].IsValid() )
                            addedRows[iAdded].Delete();
                    }


                    // First delete out of all my child rows
                    using ( est_polybagTableAdapter adapter = new est_polybagTableAdapter() )
                    {
                        adapter.Connection = conn;
                        adapter.SetTransaction( tran );
                        adapter.Update( _dsPolybagGroup.est_polybag.Select( "", "", DataViewRowState.Deleted ) );
                    }

                    // Now update the polybag group table and do the adds and updates in the reverse order
                    using ( est_polybaggroupTableAdapter adapter = new est_polybaggroupTableAdapter() )
                    {
                        adapter.Connection = conn;
                        adapter.SetTransaction( tran );
                        adapter.Update( _dsPolybagGroup.est_polybaggroup );
                    }
                    using ( est_polybagTableAdapter adapter = new est_polybagTableAdapter() )
                    {
                        adapter.Connection = conn;
                        adapter.SetTransaction( tran );
                        adapter.Update( _dsPolybagGroup.est_polybag );
                    }
                    using ( est_estimatepolybaggroup_mapTableAdapter adapter = new est_estimatepolybaggroup_mapTableAdapter() )
                    {
                        adapter.Connection = conn;
                        adapter.SetTransaction( tran );
                        adapter.Update( _dsPolybagGroup.est_estimatepolybaggroup_map );
                    }
                    using ( est_packagepolybag_mapTableAdapter adapter = new est_packagepolybag_mapTableAdapter() )
                    {
                        adapter.Connection = conn;
                        adapter.SetTransaction( tran );
                        adapter.Update( _dsPolybagGroup.est_packagepolybag_map );
                    }

                    // Finally clean up any polybags what have no packages in them just in case
                    using ( SqlCommand cmdCleanUpPolybags = new SqlCommand( "EstPolybag_Cleanup_ByPolybagGroupId", conn ) )
                    {
                        cmdCleanUpPolybags.CommandTimeout = 7200;
                        cmdCleanUpPolybags.CommandType = CommandType.StoredProcedure;
                        cmdCleanUpPolybags.Transaction = tran;
                        cmdCleanUpPolybags.Parameters.AddWithValue( "@est_polybaggroup_id", _polybagGroupRow.est_polybaggroup_id );
                        cmdCleanUpPolybags.ExecuteNonQuery();
                    }
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    tran.Dispose();
                    conn.Close();
                    RefreshPackageLists();
                }
            }

            SetTitleBar();

            ChildForm child = (ChildForm)ParentForm;
            child.LastAction = string.Format( "Last Saved: {0}", DateTime.Now );

            _btnDelete.Enabled = true;
            Dirty = false;
        }

        private void _gridPolybag_RowValidating( object sender, DataGridViewCellCancelEventArgs e )
        {
            if ( _bIsLoading || _bIsDeleting || _gridPolybag.Rows[e.RowIndex].IsNewRow )
                return;

            // Get the AllocatedBy% field.  This will deterimine if the percentage is required
            bool requirePercent = Convert.ToBoolean( _gridPolybag.Rows[e.RowIndex].Cells[AllocateByPercent.Index].EditedFormattedValue );
            decimal totalPercent = 0;
            int packageCount = 0;

            // Ensure that all the percentages in the Percent columns are between 0 and 100
            // And make sure there are at least two packages for each polybag
            foreach ( PolybagEstimate controlGroup in _dictCustomColumns.Values )
            {
                if ( requirePercent )
                {
                    // If they've selected a package for this estimate, then they have to have a percentage
                    if ( !string.IsNullOrEmpty( _gridPolybag.Rows[e.RowIndex].Cells[controlGroup.Package.Index].EditedFormattedValue.ToString() ) &&
                          string.IsNullOrEmpty( _gridPolybag.Rows[e.RowIndex].Cells[controlGroup.Percent.Index].EditedFormattedValue.ToString() ) )
                    {
                        _gridPolybag.Rows[e.RowIndex].ErrorText = string.Format( "{0}: {1} - {2}", Resources.RequiredFieldError, controlGroup.HeaderLabel.Text, "Percent" );
                        e.Cancel = true;
                        return;
                    }
                    else if ( !string.IsNullOrEmpty( _gridPolybag.Rows[e.RowIndex].Cells[controlGroup.Package.Index].EditedFormattedValue.ToString() ) )
                    {
                        decimal pkgPercent = Convert.ToDecimal( _gridPolybag.Rows[e.RowIndex].Cells[controlGroup.Percent.Index].EditedFormattedValue );
                        if ( pkgPercent < 0 || pkgPercent > 100 )
                        {
                            _gridPolybag.Rows[e.RowIndex].ErrorText = Resources.InvalidPercentageError;
                            e.Cancel = true;
                            return;
                        }
                        else
                            totalPercent += pkgPercent;
                    }
                }

                if ( !string.IsNullOrEmpty( _gridPolybag.Rows[e.RowIndex].Cells[controlGroup.Package.Name].EditedFormattedValue.ToString() ) )
                    packageCount++;
            }

            if ( packageCount < 2 )
            {
                _gridPolybag.Rows[e.RowIndex].ErrorText = Resources.PolybagRequiresMinPackages;
                e.Cancel = true;
                return;
            }

            if ( requirePercent && ( totalPercent != 100 ) )
            {
                _gridPolybag.Rows[e.RowIndex].ErrorText = Resources.PercentNot100Error;
                e.Cancel = true;
                return;
            }

            // Make sure the quantity field is filled in
            if ( string.IsNullOrEmpty( _gridPolybag.Rows[e.RowIndex].Cells[Quantity.Index].EditedFormattedValue.ToString() ) )
            {
                _gridPolybag.Rows[e.RowIndex].ErrorText = string.Format( "{0}: {1}", Resources.RequiredFieldError, "Quantity" );
                e.Cancel = true;
                return;
            }

            // Make sure the postal scenario field is filled in
            if ( string.IsNullOrEmpty( _gridPolybag.Rows[e.RowIndex].Cells[PostalScenario.Index].EditedFormattedValue.ToString() ) )
            {
                _gridPolybag.Rows[e.RowIndex].ErrorText = string.Format( "{0}: {1}", Resources.RequiredFieldError, "Postal Scenario" );
                e.Cancel = true;
                return;
            }

        }

        private void _gridPolybag_RowValidated( object sender, DataGridViewCellEventArgs e )
        {
            if ( _bIsLoading || _bIsDeleting || _gridPolybag.Rows[e.RowIndex].IsNewRow )
                return;

            _gridPolybag.Rows[e.RowIndex].ErrorText = string.Empty;

            CalculateTotal();

            // Don't do any of the rest during initial load, only b/c of user interaction
            if ( _bIsLoading )
                return;

            // Update the PackagePolybag Map table to reflect any changes
            long polybagId = Convert.ToInt64( _gridPolybag.Rows[e.RowIndex].Cells[estpolybagidDataGridViewTextBoxColumn.Index].FormattedValue );

            // Delete all the old rows for this package
            for ( int i = _dsPolybagGroup.est_packagepolybag_map.Count - 1; i >= 0; i-- )
            {
                PolybagGroup.est_packagepolybag_mapRow mapRow = (PolybagGroup.est_packagepolybag_mapRow)_dsPolybagGroup.est_packagepolybag_map[i];
                if ( mapRow.RowState != DataRowState.Deleted && mapRow.est_polybag_id == polybagId )
                    mapRow.Delete();
            }

            // Allocate Percentage determines whether distributionpct is null or not
            bool requirePercent = Convert.ToBoolean( _gridPolybag.Rows[e.RowIndex].Cells[AllocateByPercent.Index].EditedFormattedValue );

            // Now go through and re-add all the ones here
            foreach ( PolybagEstimate controlGroup in _dictCustomColumns.Values )
            {
                if ( !string.IsNullOrEmpty( _gridPolybag.Rows[e.RowIndex].Cells[controlGroup.Package.Index].EditedFormattedValue.ToString() ) )
                {
                    PolybagGroup.est_packagepolybag_mapRow newMapRow = _dsPolybagGroup.est_packagepolybag_map.Newest_packagepolybag_mapRow();
                    newMapRow.est_polybag_id = polybagId;
                    newMapRow.est_package_id = Convert.ToInt64( _gridPolybag.Rows[e.RowIndex].Cells[controlGroup.Package.Index].Value );
                    newMapRow.createdby = MainForm.AuthorizedUser.FormattedName;
                    newMapRow.createddate = DateTime.Now;

                    if ( requirePercent )
                        newMapRow.distributionpct = Convert.ToDecimal( _gridPolybag.Rows[e.RowIndex].Cells[controlGroup.Percent.Index].FormattedValue ) / 100M; // %
                    else
                        newMapRow.SetdistributionpctNull();

                    _dsPolybagGroup.est_packagepolybag_map.Addest_packagepolybag_mapRow( newMapRow );
                }
            }
        }

        private void _gridPolybag_DataError( object sender, DataGridViewDataErrorEventArgs e )
        {
            // Do nothing right now
        }

        private void _txtDescription_Validating( object sender, CancelEventArgs e )
        {
            if ( _bIsDeleting )
                return;

            _txtDescription.Text = _txtDescription.Text.Trim();
            if ( string.IsNullOrEmpty( _txtDescription.Text ) )
            {
                _errorProvider.SetError( _txtDescription, Resources.RequiredFieldError );
                e.Cancel = true;
            }
        }

        private void _txtDescription_Validated( object sender, EventArgs e )
        {
            _txtDescription.Text = _txtDescription.Text.Trim();
            _errorProvider.SetError( _txtDescription, string.Empty );
        }

        private void _txtComments_Validated( object sender, EventArgs e )
        {
            _txtComments.Text = _txtComments.Text.Trim();
        }

        private void Control_ValueChanged( object sender, EventArgs e )
        {
            Dirty = true;
        }

        private void _gridPolybag_CellValueChanged( object sender, DataGridViewCellEventArgs e )
        {
            if ( e.RowIndex < 0 || _bIsLoading )
                return;

            // Make the Percent Cells look enabled based on the Allocated By Percent and whether there is a package
            bool percentRequired = Convert.ToBoolean( _gridPolybag.Rows[e.RowIndex].Cells[AllocateByPercent.Index].EditedFormattedValue );
            foreach ( PolybagEstimate controlGroup in _dictCustomColumns.Values )
            {
                bool hasPackage = !string.IsNullOrEmpty( _gridPolybag.Rows[e.RowIndex].Cells[controlGroup.Package.Index].EditedFormattedValue.ToString() );
                if ( percentRequired && hasPackage )
                    _gridPolybag.Rows[e.RowIndex].Cells[controlGroup.Percent.Index].Style.BackColor = SystemColors.Window;
                else
                {
                    // Make it look disabled and clear any percent that's in the cell
                    _gridPolybag.Rows[e.RowIndex].Cells[controlGroup.Percent.Index].Style.BackColor = SystemColors.Control;
                    _gridPolybag.Rows[e.RowIndex].Cells[controlGroup.Percent.Index].Value           = string.Empty;
                }
            }

            Dirty = true;
        }

        private void _gridPolybag_CellBeginEdit( object sender, DataGridViewCellCancelEventArgs e )
        {
            // If they're trying to edit a percentage column when the allocate by percentage
            // box for this row is unchecked, then cancel to make this cell look disabled
            foreach ( PolybagEstimate controlGroup in _dictCustomColumns.Values )
            {
                if ( e.ColumnIndex == controlGroup.Percent.Index )
                {
                    bool percentRequired = Convert.ToBoolean( _gridPolybag.Rows[e.RowIndex].Cells[AllocateByPercent.Index].EditedFormattedValue );
                    bool hasPackage = !string.IsNullOrEmpty( _gridPolybag.Rows[e.RowIndex].Cells[controlGroup.Package.Index].EditedFormattedValue.ToString() );

                    if ( !percentRequired || !hasPackage )
                        e.Cancel = true;

                    return;
                }
            }
        }

        private void _cboPrinterVendor_SelectedValueChanged( object sender, EventArgs e )
        {
            // Save the currently selected rates so not lost when rebound.
            string strCurrentBagRate = null;
            if ( _cboBagRate.SelectedIndex >= 0 )
                strCurrentBagRate = _cboBagRate.Text;
            
            string strCurrentMakereadyRate = null;
            if ( _cboMakereadyRate.SelectedIndex >= 0 )
                strCurrentMakereadyRate = _cboMakereadyRate.Text;

            if ( _cboPrinterVendor.SelectedIndex == -1 )
            {
                _txtMessageRate.Text          = string.Empty;
                _txtMessageMakereadyRate.Text = string.Empty;
            }
            else
            {
                long printerId = (long)_cboPrinterVendor.SelectedValue;

                // Now that they've selected a vendor, we can show them the available other rates
                // First fill in the readonly rates for the printer
                PolybagGroup.Printer_s_PrinterIDandDescription_ByRunDateRow printerRow =
                    _dsPolybagGroup.Printer_s_PrinterIDandDescription_ByRunDate.FindByVND_Printer_ID( printerId );

                _txtMessageRate.Value          = printerRow.PolybagMessage;
                _txtMessageMakereadyRate.Value = printerRow.PolybagMessageMakeready;

                // Now get the possible Bag Rates and Makeready Rates to fill the combo box with
                using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
                {
                    // First grab all the valid printer rates
                    using ( prt_printerrateTableAdapter adapter = new prt_printerrateTableAdapter() )
                    {
                        adapter.Connection = conn;
                        adapter.ClearBeforeFill = true;
                        adapter.Fill( _dsPolybagGroup.prt_printerrate, printerId );
                    }

                    // Next grab the printer vendor record
                    using (vnd_printerTableAdapter adapter = new vnd_printerTableAdapter())
                    {
                        adapter.Connection = conn;
                        adapter.ClearBeforeFill = true;
                        adapter.Fill(_dsPolybagGroup.vnd_printer, printerId);
                    }

                    conn.Close();
                }

                if (_dsPolybagGroup.vnd_bag_weight.Count == 0)
                {
                    PolybagGroup.vnd_bag_weightRow bag = _dsPolybagGroup.vnd_bag_weight.Newvnd_bag_weightRow();
                    bag.est_polybaggroup_id = _dsPolybagGroup.est_polybaggroup[0].est_polybaggroup_id;
                    bag.bag_weight = 0;
                    _dsPolybagGroup.vnd_bag_weight.Addvnd_bag_weightRow(bag);
                }

                if (_dsPolybagGroup.vnd_printer.Count > 0)
                {
                    _dsPolybagGroup.vnd_bag_weight[0].bag_weight = _dsPolybagGroup.vnd_printer[0].polybagbagweight;
                }

                DataView bagView          = new DataView( _dsPolybagGroup.prt_printerrate );
                bagView.RowFilter         = "prt_printerratetype_id = " + (int)PrinterRateType.PBBag;
                bagView.Sort              = "default DESC, description ASC";
                _cboBagRate.DataSource    = bagView;
                _cboBagRate.DisplayMember = "description";
                _cboBagRate.ValueMember   = "prt_printerrate_id";

                DataView makeReadView           = new DataView( _dsPolybagGroup.prt_printerrate );
                makeReadView.RowFilter          = "prt_printerratetype_id = " + (int)PrinterRateType.PBMakeready;
                makeReadView.Sort               = "default DESC, description ASC";
                _cboMakereadyRate.DataSource    = makeReadView;
                _cboMakereadyRate.DisplayMember = "description";
                _cboMakereadyRate.ValueMember   = "prt_printerrate_id";

                ReselectRate( _cboBagRate, strCurrentBagRate );
                ReselectRate( _cboMakereadyRate, strCurrentMakereadyRate );
            }
        }

        private void ReselectRate( ComboBox combo, string rateName )
        {
            if ( rateName == null )
                return;

            foreach ( DataRowView viewRate in combo.Items )
            {
                PolybagGroup.prt_printerrateRow row = (PolybagGroup.prt_printerrateRow)viewRate.Row;
                if ( row.description == rateName )
                {
                    combo.SelectedValue = row.prt_printerrate_id;
                    return;
                }
            }

        }

        private void _btnHome_Click( object sender, EventArgs e )
        {
            ChildForm childform = (ChildForm)ParentForm;
            if ( childform.MainForm.WindowState == FormWindowState.Minimized )
                childform.MainForm.WindowState = FormWindowState.Normal;
            childform.MainForm.Activate();
        }

        private void ucpPolybag_Load( object sender, EventArgs e )
        {
            CheckReadOnly();
        }

        private void ucpPolybag_Validating( object sender, CancelEventArgs e )
        {
            if ( _bIsDeleting )
                return;

            if ( _cboPrinterVendor.Items.Count == 0 )
            {
                _errorProvider.SetError( _cboPrinterVendor, Resources.VendorRateDoesntExist );
                e.Cancel = true;
            }
            else if ( _cboPrinterVendor.SelectedIndex == -1 )
            {
                _errorProvider.SetError( _cboPrinterVendor, Resources.RequiredFieldError );
                e.Cancel = true;
            }

            if ( _cboBagRate.Items.Count == 0 )
            {
                _errorProvider.SetError( _cboBagRate, Resources.VendorRateDoesntExist );
                e.Cancel = true;
            }
            else if ( _cboBagRate.SelectedIndex == -1 )
            {
                _errorProvider.SetError( _cboBagRate, Resources.RequiredFieldError );
                e.Cancel = true;
            }

            if ( _cboMakereadyRate.Items.Count == 0 )
            {
                _errorProvider.SetError( _cboMakereadyRate, Resources.VendorRateDoesntExist );
                e.Cancel = true;
            }
            else if ( _cboMakereadyRate.SelectedIndex == -1 )
            {
                _errorProvider.SetError( _cboMakereadyRate, Resources.RequiredFieldError );
                e.Cancel = true;
            }
        }

        private void ucpPolybag_Validated( object sender, EventArgs e )
        {
            _errorProvider.SetError( _cboPrinterVendor, string.Empty );
            _errorProvider.SetError( _cboBagRate, string.Empty );
            _errorProvider.SetError( _cboMakereadyRate, string.Empty );
        }

        private void _btnDelete_Click( object sender, EventArgs e )
        {
            DialogResult result = MessageBox.Show( Resources.DeletePolybagGroupWarning, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
            if ( result == DialogResult.Yes )
            {
                // First delete this polybag group and all it's associated rows, but only if 
                // this isn't a new row.  If it's new, then we simply don't do an update
                if ( _polybagGroupRow.RowState != DataRowState.Added )
                {
                    // Do the delete.  This will cascade down to all the child tables
                    _polybagGroupRow.Delete();

                    // Now update the appropriate tables
                    using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
                    {
                        // Update the map table for estimates to polybags
                        using ( est_estimatepolybaggroup_mapTableAdapter adapter = new est_estimatepolybaggroup_mapTableAdapter() )
                        {
                            adapter.Connection = conn;
                            adapter.Update( _dsPolybagGroup.est_estimatepolybaggroup_map );
                        }

                        // Update the map table for polybags to packages
                        using ( est_packagepolybag_mapTableAdapter adapter = new est_packagepolybag_mapTableAdapter() )
                        {
                            adapter.Connection = conn;
                            adapter.Update( _dsPolybagGroup.est_packagepolybag_map );
                        }

                        // Update the polybag table
                        using ( est_polybagTableAdapter adapter = new est_polybagTableAdapter() )
                        {
                            adapter.Connection = conn;
                            adapter.Update( _dsPolybagGroup.est_polybag );
                        }

                        // Finally update the polybaggroup table
                        using ( est_polybaggroupTableAdapter adapter = new est_polybaggroupTableAdapter() )
                        {
                            adapter.Connection = conn;
                            adapter.Update( _dsPolybagGroup.est_polybaggroup );
                        }

                        conn.Close();
                    }
                }

                _bIsDeleting = true;    // Prevent validation from firing
                Dirty = false;          // Prevents prompt on close

                // Then close the form since this polybag is no longer active.
                ParentForm.Close();
            }
        }

        private void _gridPolybag_DefaultValuesNeeded( object sender, DataGridViewRowEventArgs e )
        {
            e.Row.Cells[estpolybaggroupidDataGridViewTextBoxColumn.Index].Value = _polybagGroupRow.est_polybaggroup_id;
            e.Row.Cells[Quantity.Index].Value = 0;
            e.Row.Cells[createdbyDataGridViewTextBoxColumn.Index].Value = MainForm.AuthorizedUser.FormattedName;
            e.Row.Cells[createddateDataGridViewTextBoxColumn.Index].Value = DateTime.Now;

            // By default, the Allocate By % is unchecked, so make the backcolor look disabled
            // on all the percent cells in the grid
            foreach ( PolybagEstimate controlGroup in _dictCustomColumns.Values )
            {
                e.Row.Cells[controlGroup.Percent.Index].Style.BackColor = SystemColors.Control;
            }
        }

        private void _gridPolybag_KeyDown( object sender, KeyEventArgs e )
        {
            if ( e.KeyCode == Keys.Escape && !_gridPolybag.IsCurrentCellInEditMode )
            {
                _gridPolybag.CurrentRow.Cells[estpolybaggroupidDataGridViewTextBoxColumn.Index].Value = _polybagGroupRow.est_polybaggroup_id;
                estpolybagBindingSource.CancelEdit();
            }
        }

        private void _btnUpload_Click( object sender, EventArgs e )
        {
            ChildForm childForm = (ChildForm)ParentForm;
            childForm.MainForm.DisplayUploadControl( new List<long>( _dictCustomColumns.Keys ) );
            childForm.MainForm.Activate();
        }

        #endregion

        #region PolybagEstimate Class

        /// <summary>Used to group together the grid columns for a polybag row that 
        /// correspond to a single estimate</summary>
        private class PolybagEstimate
        {
            public long EstimateId = 0;
            public DataGridViewComboBoxColumn Package;
            public DecimalColumn Percent;
            public Label HeaderLabel;

            public DataView PackageView;
        }

        #endregion
    }
}