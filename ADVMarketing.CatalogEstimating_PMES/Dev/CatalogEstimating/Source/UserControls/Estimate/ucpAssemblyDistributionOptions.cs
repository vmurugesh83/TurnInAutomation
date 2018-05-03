#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.EstimatesTableAdapters;
using CatalogEstimating.Properties;
using CatalogEstimating.CustomControls;
using System.Data.SqlClient;

#endregion

namespace CatalogEstimating.UserControls.Estimate
{
    public partial class ucpAssemblyDistributionOptions : CatalogEstimating.UserControlPanel
    {
        #region Private Variables

        private static Dictionary<int, string> InsertDOWDictionary = new Dictionary<int, string>();
        private Estimates.est_assemdistriboptionsRow _databaseRow;
        private SortedList<string, long> _lstFreightVendors = new SortedList<string, long>();
        private bool _bInReload = false;
        private bool _bReadOnly = false;

        #endregion

        #region Construction

        static ucpAssemblyDistributionOptions()
        {
            // Fill in the dictionaries for the lookup tables that won't change
            foreach ( int value in Enum.GetValues( typeof( InsertDOW ) ) )
            {
                InsertDOWDictionary.Add( value, Enum.GetName( typeof( InsertDOW ), value ) );
            }
        }

        public ucpAssemblyDistributionOptions()
        {
            InitializeComponent();

            Name = "Assembly/Distribution Options";
        }

        public ucpAssemblyDistributionOptions( Estimates dsEstimate, bool bReadOnly )
        : this()
        {
            _dsEstimate = dsEstimate;
            _bReadOnly  = bReadOnly;

            // Rebind the datasource because got a different one than originally constructed
            mailhousesMailhouseIDandDescriptionByRunDateBindingSource.DataSource           = _dsEstimate;
            mailListsMailListIDandDescriptionByRunDateBindingSource.DataSource             = _dsEstimate;
            mailTrackingsMailTrackingIDandDescriptionByRunDateBindingSource.DataSource     = _dsEstimate;
            postalScenariosPostalScenarioIDandDescriptionByRunDateBindingSource.DataSource = _dsEstimate;
        }

        #endregion

        #region Override Methods

        public override void Reload()
        {
            _bInReload = true;
            bool bTempDirty = Dirty;

            // Disable all the fields if a read only user
            ChildForm child = (ChildForm)ParentForm;
            if ( child.ReadOnly )
                Enabled = false;

            LoadVendors();

            if ( _databaseRow == null )
            {
                if ( !_bReadOnly )
                {
                    _bInReload = false;     // Want dirty to fire and defaults to propogate down in this case
                    bTempDirty = true;      // Force dirty to true
                }

                // The Default Day of Week is the day of week of the run date
                // Need to convert one enumeration to the other to ensure right ints are used
                DayOfWeek dtDOW = _dsEstimate.est_estimate[0].rundate.DayOfWeek;
                InsertDOW insertDOW = (InsertDOW)Enum.Parse( typeof( InsertDOW ), dtDOW.ToString() );
                _cboInsertDOW.SelectedValue = (int)insertDOW;
                
                // Default Values
                _txtFreightCWT.Value          = 0;
                _txtFuelSurcharge.Value       = 0;
                _txtPostalFuelSurcharge.Value = 0;
                _txtOtherHandling.Value       = 0;
                _txtExternalQty.Value         = 0;
                _txtExternalCPM.Value         = 0;
                _txtNumberOfCartons.Value     = 0;
                _txtOtherFreight.Value        = 0;
                _txtPostalDropFlat.Value      = null;
            }
            else
                InitializeFromDataset();    // From the dataset

            // Refresh the appropriate read only fields for the vendors
            _cboMailHouseVendor_SelectedValueChanged( this, EventArgs.Empty );
            _cboPostalCategoryScenario_SelectedValueChanged( this, EventArgs.Empty );

            // Recalculate the weights
            decimal insertWeight, mailingWeight;
            CalculateWeights( out insertWeight, out mailingWeight );

            _txtTotalInsertWeight.Value  = insertWeight;
            _txtTotalMailingWeight.Value = mailingWeight;

            _bInReload = false;
            Dirty = bTempDirty;

            base.Reload();
        }

        public override void Export( ref ExcelWriter writer )
        {
            // Insert Options
            writer.WriteLine( _groupInsertOptions.Text );
            writer.WriteLine( _lblInsertDOW.Text, _cboInsertDOW.Text, _radAMPM.Checked ? _radAMPM.Text : _radPMAM.Text );
            writer.WriteLine( _lblFreightVendor.Text, _cboFreightVendor.Text );
            writer.WriteLine( _chkCornerGuards.Text, _chkCornerGuards.Checked );
            writer.WriteLine( _chkSkids.Text, _chkSkids.Checked );
            writer.WriteLine( _lblFreightCWT.Text, _txtFreightCWT.Text );
            writer.WriteLine( _lblFuelSurcharge.Text, _txtFuelSurcharge.Text );
            writer.WriteLine();

            // Mail Options
            writer.WriteLine( _groupMailOptions.Text );
            writer.WriteLine();

            // Postage
            writer.WriteLine( _groupPostage.Text );
            writer.WriteLine( _lblPostalCategoryScenario.Text, _cboPostalCategoryScenario.Text );
            writer.WriteLine( _lblPostalClass.Text, _txtPostalClass.Text );
            writer.WriteLine( _lblPostalType.Text, _txtPostalType.Text );
            writer.WriteLine( _lblPostalFuelSurcharge.Text, _txtPostalFuelSurcharge.Text );
            writer.WriteLine();

            // Mailhouse
            writer.WriteLine( _groupMailhouse.Text );
            writer.WriteLine( _lblMailHouseVendor.Text, _cboMailHouseVendor.Text );
            writer.WriteLine( _lblOtherHandling.Text, _txtOtherHandling.Text );
            writer.WriteLine( _lblPostalDropCWT.Text, _txtPostalDropCWT.Text );
            writer.WriteLine( _lblPostalDropFlat.Text, _txtPostalDropFlat.Text );
            writer.WriteLine( _chkGlueTack.Text, _chkGlueTack.Checked );
            writer.WriteLine( _chkTabbing.Text, _chkTabbing.Checked );
            writer.WriteLine( _chkLetterInsertion.Text, _chkLetterInsertion.Checked );
            writer.WriteLine();

            // Mail List
            writer.WriteLine( _groupMailList.Text );
            writer.WriteLine( _lblMailListResource.Text, _cboMailListResource.Text );
            writer.WriteLine( _chkUseExternalMailList.Text, _chkUseExternalMailList.Checked );
            writer.WriteLine( _lblExternalQty.Text, _txtExternalQty.Text );
            writer.WriteLine( _lblExternalCPM.Text, _txtExternalCPM.Text );
            writer.WriteLine();

            // # of Cartons
            writer.WriteLine( _lblNumberOfCartons.Text, _txtNumberOfCartons.Text );
            writer.WriteLine();

            // Mail Tracking
            writer.WriteLine( _chkUseMailTracking.Text, _chkUseMailTracking.Checked );
            writer.WriteLine( _lblMailTracker.Text, _cboMailTracker.Text );
            writer.WriteLine();

            // Other Options
            writer.WriteLine( _groupOtherOptions.Text );
            writer.WriteLine( _lblOtherFreight.Text, _txtOtherFreight.Text );

            base.Export( ref writer );
        }

        public override void PreSave( CancelEventArgs e )
        {
            // If they never saw this screen, then couldn't have done anything requiring a save
            if ( !Loaded )
                return;

            if ( !ValidateChildren() )
                e.Cancel = true;
            else
                SaveToDataset();

            base.PreSave( e );
        }

        public override void OnLeaving( CancelEventArgs e )
        {
            if ( !_bReadOnly )
            {
                SaveToDataset();
                
                bool valid = ValidateChildren() && _dsEstimate.est_assemdistriboptions[0].IsValid();
                if ( !valid )
                {
                    DialogResult result = MessageBox.Show( this, Resources.InvalidDataWarning, this.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
                    if ( result == DialogResult.No )
                        e.Cancel = true;
                }
            }
            base.OnLeaving( e );
        }

        #endregion

        #region Event Handlers

        private void Control_ValueChanged( object sender, EventArgs e )
        {
            Dirty = true;
        }

        private void _chkUseExternalMailList_CheckedChanged( object sender, EventArgs e )
        {
            _txtExternalQty.Enabled = _chkUseExternalMailList.Checked;
            _txtExternalCPM.Enabled = _chkUseExternalMailList.Checked;
            Dirty = true;
        }

        private void _chkUseMailTracking_CheckedChanged( object sender, EventArgs e )
        {
            _cboMailTracker.Enabled = _chkUseMailTracking.Checked;
            Dirty = true;
        }

        private void Control_ValidatingRequiredCombo( object sender, CancelEventArgs e )
        {
            ComboBox combo = (ComboBox)sender;
            if ( combo.SelectedIndex == -1 )
                _errorProvider.SetError( combo, Resources.RequiredFieldError );
            else
                _errorProvider.SetError( combo, string.Empty );
        }

        private void Control_ValidatedRequiredField( object sender, EventArgs e )
        {
            _errorProvider.SetError( (Control)sender, string.Empty );
        }

        private void Control_ValidatingRequiredText( object sender, CancelEventArgs e )
        {
            TextBox text = (TextBox)sender;
            if ( text.Text.Trim().Length == 0 )
            {
                _errorProvider.SetError( text, Resources.RequiredFieldError );
                e.Cancel = true;
            }
        }

        private void Control_ValidatingRequiredPercent( object sender, CancelEventArgs e )
        {
            DecimalTextBox text = (DecimalTextBox)sender;
            if ( text.Text.Trim().Length == 0 )
            {
                _errorProvider.SetError( text, Resources.RequiredFieldError );
                e.Cancel = true;
            }
            else if ( text.Value.Value < 0 || text.Value.Value > 100 )
            {
                _errorProvider.SetError( text, Resources.InvalidPercentageError );
                e.Cancel = true;
            }
        }

        private void Control_ValidatingRequiredMailList( object sender, CancelEventArgs e )
        {
            if ( _chkUseExternalMailList.Checked )
            {
                TextBox text = (TextBox)sender;
                if ( text.Text.Trim().Length == 0 )
                {
                    _errorProvider.SetError( text, Resources.RequiredFieldError );
                    e.Cancel = true;
                }
            }
        }

        private void _cboPostalCategoryScenario_SelectedValueChanged( object sender, EventArgs e )
        {
            if ( _cboPostalCategoryScenario.SelectedIndex == -1 )
                return;

            Estimates.PostalScenario_s_PostalScenarioIDandDescription_ByRunDateRow row =
                _dsEstimate.PostalScenario_s_PostalScenarioIDandDescription_ByRunDate.FindBypst_postalscenario_id( (long)_cboPostalCategoryScenario.SelectedValue );

            _txtPostalClass.Text = row.postalclass;
            _txtPostalType.Text = row.postalmailertype;

            Dirty = true;
        }

        private void _cboMailHouseVendor_SelectedValueChanged( object sender, EventArgs e )
        {
            if ( _cboMailHouseVendor.SelectedIndex == -1 )
                return;

            Estimates.Mailhouse_s_MailhouseIDandDescription_ByRunDateRow row =
                _dsEstimate.Mailhouse_s_MailhouseIDandDescription_ByRunDate.FindByvnd_mailhouserate_id( (long)_cboMailHouseVendor.SelectedValue );

            _txtPostalDropCWT.Value = row.postaldropcwt;

            if ( _bInReload )
                return;

            // The user selected a combo box item.  Force the defaults properly
            _chkGlueTack.Checked = row.gluetackdefault;
            _chkTabbing.Checked = row.tabbingdefault;
            _chkLetterInsertion.Checked = row.letterinsertiondefault;

            Dirty = true;
        }

        #endregion

        #region Private Methods

        private void LoadVendors()
        {
            DateTime dtRunDate = _dsEstimate.est_estimate[0].rundate;

            _cboInsertDOW.DataSource    = new BindingSource( InsertDOWDictionary, null );
            _cboInsertDOW.DisplayMember = "Value";
            _cboInsertDOW.ValueMember   = "Key";

            _lstFreightVendors.Clear();
            foreach ( Estimates.vnd_vendorRow vendor in _dsEstimate.vnd_vendor.Rows )
            {
                if ( vendor.active && _dsEstimate.vnd_vendorvendortype_map.FindByvnd_vendor_idvnd_vendortype_id( vendor.vnd_vendor_id, 10 ) != null )
                    _lstFreightVendors.Add( vendor.description, vendor.vnd_vendor_id );
            }

            using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
            {
                // First grab all the mail list vendors
                using ( MailList_s_MailListIDandDescription_ByRunDateTableAdapter adapter = new MailList_s_MailListIDandDescription_ByRunDateTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.ClearBeforeFill = true;
                    adapter.Fill( _dsEstimate.MailList_s_MailListIDandDescription_ByRunDate, dtRunDate );
                }

                // Now the Mail House Vendords
                using ( Mailhouse_s_MailhouseIDandDescription_ByRunDateTableAdapter adapter = new Mailhouse_s_MailhouseIDandDescription_ByRunDateTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.ClearBeforeFill = true;
                    adapter.Fill( _dsEstimate.Mailhouse_s_MailhouseIDandDescription_ByRunDate, dtRunDate );
                }

                using ( MailTracking_s_MailTrackingIDandDescription_ByRunDateTableAdapter adapter = new MailTracking_s_MailTrackingIDandDescription_ByRunDateTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.ClearBeforeFill = true;
                    adapter.Fill( _dsEstimate.MailTracking_s_MailTrackingIDandDescription_ByRunDate, dtRunDate );
                }

                using ( PostalScenario_s_PostalScenarioIDandDescription_ByRunDateTableAdapter adapter = new PostalScenario_s_PostalScenarioIDandDescription_ByRunDateTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.ClearBeforeFill = true;
                    adapter.Fill( _dsEstimate.PostalScenario_s_PostalScenarioIDandDescription_ByRunDate, dtRunDate );
                }

                conn.Close();
            }

            if ( _lstFreightVendors.Count > 0 )
            {
                _cboFreightVendor.DataSource    = new BindingSource( _lstFreightVendors, null );
                _cboFreightVendor.DisplayMember = "Key";
                _cboFreightVendor.ValueMember   = "Value";
            }

            if ( _dsEstimate.est_assemdistriboptions.Count > 0 )
                _databaseRow = _dsEstimate.est_assemdistriboptions[0];
        }

        private void SaveToDataset()
        {
            // Never been in here, need to create a row
            if ( _dsEstimate.est_assemdistriboptions.Count == 0 )
            {
                _databaseRow = _dsEstimate.est_assemdistriboptions.Newest_assemdistriboptionsRow();
                _databaseRow.est_estimate_id = _dsEstimate.est_estimate[0].est_estimate_id;
                _databaseRow.createdby       = MainForm.AuthorizedUser.FormattedName;
                _databaseRow.createddate     = DateTime.Now;
            }

            // Insert Options
            _databaseRow.insertdow = (int)_cboInsertDOW.SelectedValue;
            if ( _cboFreightVendor.Text == string.Empty )
                _databaseRow.Setinsertfreightvendor_idNull();
            else
                _databaseRow.insertfreightvendor_id = (long)_cboFreightVendor.SelectedValue;

            _databaseRow.inserttime             = _radPMAM.Checked;
            _databaseRow.cornerguards           = _chkCornerGuards.Checked;
            _databaseRow.skids                  = _chkSkids.Checked;
            _databaseRow.insertfreightcwt       = _txtFreightCWT.Value.Value;
            _databaseRow.insertfuelsurcharge    = _txtFuelSurcharge.Value.Value / 100M;         // %

            // Postage
            if ( _cboPostalCategoryScenario.Text == string.Empty )
                _databaseRow.Setpst_postalscenario_idNull();
            else
                _databaseRow.pst_postalscenario_id = (long)_cboPostalCategoryScenario.SelectedValue;

            _databaseRow.mailfuelsurcharge     = _txtPostalFuelSurcharge.Value.Value / 100M;    // %
            _databaseRow.firstclass            = _txtPostalClass.Text == "First";
            if ( _txtPostalDropFlat.Value == null )
                _databaseRow.SetpostaldropflatNull();
            else
                _databaseRow.postaldropflat = _txtPostalDropFlat.Value.Value;

            // Mailhouse
            if ( _cboMailHouseVendor.Text == string.Empty )
                _databaseRow.Setmailhouse_idNull();
            else
                _databaseRow.mailhouse_id       = (long)_cboMailHouseVendor.SelectedValue;

            _databaseRow.mailhouseotherhandling = _txtOtherHandling.Value.Value;
            _databaseRow.usegluetack            = _chkGlueTack.Checked;
            _databaseRow.usetabbing             = _chkTabbing.Checked;
            _databaseRow.useletterinsertion     = _chkLetterInsertion.Checked;

            // Mail List
            if ( _cboMailListResource.Text == string.Empty )
                _databaseRow.Setmaillistresource_idNull();
            else
                _databaseRow.maillistresource_id = (long)_cboMailListResource.SelectedValue;

            _databaseRow.useexternalmaillist = _chkUseExternalMailList.Checked;
            _databaseRow.externalmailqty     = _txtExternalQty.Value.Value;
            _databaseRow.externalmailcpm     = _txtExternalCPM.Value.Value;

            _databaseRow.nbrofcartons = _txtNumberOfCartons.Value.Value;

            // Mail Tracking
            _databaseRow.usemailtracking = _chkUseMailTracking.Checked;
            if ( _chkUseMailTracking.Checked && _cboMailTracker.Text != string.Empty )
                _databaseRow.mailtracking_id = (long)_cboMailTracker.SelectedValue;
            else
                _databaseRow.Setmailtracking_idNull();

            // Other options
            _databaseRow.otherfreight = _txtOtherFreight.Value.Value;

            if ( _dsEstimate.est_assemdistriboptions.Count != 0 )
            {
                _databaseRow.modifiedby   = MainForm.AuthorizedUser.FormattedName;
                _databaseRow.modifieddate = DateTime.Now;
            }
            else
                _dsEstimate.est_assemdistriboptions.Addest_assemdistriboptionsRow( _databaseRow );
        }

        private void InitializeFromDataset()
        {
            // Insert Options
            _cboInsertDOW.SelectedValue     = _databaseRow.insertdow;
            if ( !_databaseRow.Isinsertfreightvendor_idNull() )
                _cboFreightVendor.SelectedValue = _databaseRow.insertfreightvendor_id;

            _txtFreightCWT.Value            = _databaseRow.insertfreightcwt;
            _txtFuelSurcharge.Value         = _databaseRow.insertfuelsurcharge * 100M;  // %
            _radPMAM.Checked                = _databaseRow.inserttime;
            _radAMPM.Checked                = !_databaseRow.inserttime;
            _chkCornerGuards.Checked        = _databaseRow.cornerguards;
            _chkSkids.Checked               = _databaseRow.skids;

            // Postage
            if ( !_databaseRow.Ispst_postalscenario_idNull() )
                _cboPostalCategoryScenario.SelectedValue = _databaseRow.pst_postalscenario_id;

            _txtPostalFuelSurcharge.Value            = _databaseRow.mailfuelsurcharge * 100M;   // %
            if ( !_databaseRow.IspostaldropflatNull() )
                _txtPostalDropFlat.Value = _databaseRow.postaldropflat;
            else
                _txtPostalDropFlat.Value = null;

            // Mailhouse
            if ( !_databaseRow.Ismailhouse_idNull() )
                _cboMailHouseVendor.SelectedValue = _databaseRow.mailhouse_id;

            _txtOtherHandling.Value           = _databaseRow.mailhouseotherhandling;
            _chkGlueTack.Checked              = _databaseRow.usegluetack;
            _chkTabbing.Checked               = _databaseRow.usetabbing;
            _chkLetterInsertion.Checked       = _databaseRow.useletterinsertion;

            // Mail List
            if ( !_databaseRow.Ismaillistresource_idNull() )
                _cboMailListResource.SelectedValue = _databaseRow.maillistresource_id;

            _chkUseExternalMailList.Checked = _databaseRow.useexternalmaillist;
            _txtExternalQty.Value           = _databaseRow.externalmailqty;
            _txtExternalCPM.Value           = _databaseRow.externalmailcpm;

            _txtNumberOfCartons.Value = _databaseRow.nbrofcartons;

            // Mail Tracking
            _chkUseMailTracking.Checked = _databaseRow.usemailtracking;
            if ( _chkUseMailTracking.Checked )
                _cboMailTracker.SelectedValue = _databaseRow.mailtracking_id;

            // Other options
            _txtOtherFreight.Value = _databaseRow.otherfreight;
        }

        private void CalculateWeights( out decimal insertWeight, out decimal mailingWeight )
        {
            insertWeight  = 0M;
            mailingWeight = 0M;

            foreach ( Estimates.est_packageRow package in _dsEstimate.est_package )
            {
                if ( package.RowState == DataRowState.Deleted )
                    continue;

                decimal packageWeight = 0M;
                foreach ( Estimates.est_packagecomponentmappingRow mapRow in package.Getest_packagecomponentmappingRows() )
                {
                    if ( mapRow.RowState != DataRowState.Deleted )
                        packageWeight += mapRow.est_componentRow.CalculateWeight();
                }

                if ( package.GetEstPackage_Quantities_ByEstimateIDRows().Length > 0 )
                {
                    Estimates.EstPackage_Quantities_ByEstimateIDRow qtys = package.GetEstPackage_Quantities_ByEstimateIDRows()[0];
                    if ( qtys.RowState == DataRowState.Deleted )
                        continue;

                    if ( !qtys.IsinsertqtyNull() )
                        insertWeight += packageWeight * qtys.insertqty;

                    if ( !qtys.IspolybagqtyNull() )
                        mailingWeight += packageWeight * ( qtys.polybagqty + package.soloquantity );
                }
            }
        }

        #endregion
    }
}