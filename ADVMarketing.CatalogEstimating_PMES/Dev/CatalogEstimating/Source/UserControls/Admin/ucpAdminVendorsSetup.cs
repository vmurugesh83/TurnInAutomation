#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CatalogEstimating.Datasets;
using CatalogEstimating.Properties;

#endregion

namespace CatalogEstimating.UserControls.Admin
{
    public partial class ucpAdminVendorsSetup : CatalogEstimating.UserControlPanel
    {
        #region Private Variables

        private Administration.vnd_vendorRow _currentVendor = null;
        private bool _bIgnoreValidation = false;

        #endregion

        #region Construction

        public ucpAdminVendorsSetup()
        {
            InitializeComponent();
            Name = "Setup";
        }

        public ucpAdminVendorsSetup( Administration ds )
        : this()
        {
            _dsAdministration = ds;
            vndvendorBindingSource.DataSource = ds;
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

        public override void Reload()
        {
            LoadGrid();

            if ( MainForm.AuthorizedUser.Right == UserRights.Admin ||
                 MainForm.AuthorizedUser.Right == UserRights.SuperAdmin )
            {
                _groupSetup.Enabled = ( _gridVendors.CurrentRow != null );
                _btnCancel.Enabled  = Dirty;
            }

            _gridVendors_SelectionChanged( this, EventArgs.Empty );
        }

        public override void PreSave( CancelEventArgs e )
        {
            if ( ValidateChildren() )
                _btnUpdate_Click( this, EventArgs.Empty );
            else
                e.Cancel = true;

            base.PreSave( e );
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

        public override void Export( ref ExcelWriter writer )
        {
            writer.WriteTable( _gridVendors, true );
        }

        #endregion

        #region Event Handlers

        private void ucpAdminVendorsSetup_Load( object sender, EventArgs e )
        {
            if ( MainForm.AuthorizedUser.Right != UserRights.Admin &&
                 MainForm.AuthorizedUser.Right != UserRights.SuperAdmin )
            {
                _btnNew.Enabled = false;
                _btnCancel.Enabled = false;
                _gridVendors.ReadOnly = true;
                _gridVendors.DefaultCellStyle.BackColor = SystemColors.Control;
                _groupSetup.Enabled = false;
            }
        }

        private void _btnNew_Click( object sender, EventArgs e )
        {
            _currentVendor = _dsAdministration.vnd_vendor.Newvnd_vendorRow();
            _currentVendor.description = "";
            _currentVendor.vendorcode  = "";
            _currentVendor.active      = true;  // Default new vendors to Active

            LoadVendor();

            _groupSetup.Enabled = true;
            Dirty = true;

            _bIgnoreValidation = true;
            _txtVendorName.Focus();
            _bIgnoreValidation = false;
        }

        private void _gridVendors_SelectionChanged( object sender, EventArgs e )
        {
            if ( _gridVendors.CurrentRow != null )
            {
                _currentVendor = (Administration.vnd_vendorRow)((DataRowView)_gridVendors.CurrentRow.DataBoundItem).Row;
                LoadVendor();

                if ( MainForm.AuthorizedUser.Right == UserRights.Admin ||
                     MainForm.AuthorizedUser.Right == UserRights.SuperAdmin )
                {
                    _groupSetup.Enabled = true;
                }
            }
        }

        private void _btnUpdate_Click( object sender, EventArgs e )
        {
            if ( !ValidateChildren() || ( _currentVendor == null ) )
                return;

            // Have to capture the state of the check boxes before adding anything to the dataset
            // otherwise it will get cleared during the binding operation to the grid on add
            Dictionary<int, bool> hashVendorTypeState = new Dictionary<int, bool>();
            foreach ( Control ctrl in _groupSetup.Controls )
            {
                CheckBox check = ctrl as CheckBox;
                if ( check != null && check.Tag != null )
                {
                    int vendorTypeId = Convert.ToInt32( check.Tag );
                    hashVendorTypeState.Add( vendorTypeId, check.Checked );
                }
            }

            // Now update the dataset
            _currentVendor.description = _txtVendorName.Text;
            _currentVendor.vendorcode  = _txtVendorID.Text;
            _currentVendor.active      = _chkActive.Checked;

            // This is a new row, not an updated row
            if ( _currentVendor.RowState == DataRowState.Detached )
            {
                _currentVendor.createdby = MainForm.AuthorizedUser.FormattedName;
                _currentVendor.createddate = DateTime.Now;

                _dsAdministration.vnd_vendor.Addvnd_vendorRow( _currentVendor );
            }
            else
            {
                _currentVendor.modifiedby = MainForm.AuthorizedUser.FormattedName;
                _currentVendor.modifieddate = DateTime.Now;
            }

            foreach ( KeyValuePair<int, bool> vendorTypePair in hashVendorTypeState )
            {
                // Try to find a current row for this vendor and vendor type
                Administration.vnd_vendorvendortype_mapRow mapRow = _dsAdministration.vnd_vendorvendortype_map.FindByvnd_vendor_idvnd_vendortype_id( _currentVendor.vnd_vendor_id, vendorTypePair.Key );

                // They've check the box, but a row doesn't currently exist.  Add it
                if ( vendorTypePair.Value && mapRow == null )
                {
                    mapRow = _dsAdministration.vnd_vendorvendortype_map.Newvnd_vendorvendortype_mapRow();
                    mapRow.vnd_vendor_id     = _currentVendor.vnd_vendor_id;
                    mapRow.vnd_vendortype_id = vendorTypePair.Key;
                    mapRow.createdby         = MainForm.AuthorizedUser.FormattedName;
                    mapRow.createddate       = DateTime.Now;
                    _dsAdministration.vnd_vendorvendortype_map.Addvnd_vendorvendortype_mapRow( mapRow );
                }
                // They've unchecked the box, but a row does currently exist.  Delete it
                else if ( !vendorTypePair.Value && mapRow != null )
                    mapRow.Delete();
            }

            LoadGrid();
            LoadVendor();
            _btnUpdate.Enabled = false;
        }

        private void _btnCancel_Click( object sender, EventArgs e )
        {
            if ( Dirty )
            {
                DialogResult result = MessageBox.Show( Resources.CancelChangesWarning, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
                if ( result == DialogResult.Yes )
                {
                    _dsAdministration.vnd_vendorvendortype_map.RejectChanges();
                    _dsAdministration.vnd_vendor.RejectChanges();
                    Dirty = false;
                    Reload();
                }
            }
        }

        private void _txtVendorName_Validating( object sender, CancelEventArgs e )
        {
            if ( _currentVendor == null )
                return;

            _txtVendorName.Text = _txtVendorName.Text.Trim();
            if ( string.IsNullOrEmpty( _txtVendorName.Text ) )
            {
                _Errors.SetError( _txtVendorName, Resources.RequiredFieldError );
                e.Cancel = true;
            }
        }

        private void _txtVendorName_Validated( object sender, EventArgs e )
        {
            _Errors.SetError( _txtVendorName, string.Empty );
        }

        private void _txtVendorID_Validating( object sender, CancelEventArgs e )
        {
            if ( _currentVendor == null || _bIgnoreValidation )
                return;

            _txtVendorID.Text = _txtVendorID.Text.Trim();
            if ( string.IsNullOrEmpty( _txtVendorID.Text ) )
            {
                _Errors.SetError( _txtVendorID, Resources.RequiredFieldError );
                e.Cancel = true;
            }
            else
            {
                foreach ( Administration.vnd_vendorRow row in _dsAdministration.vnd_vendor.Rows )
                {
                    if ( row.RowState == DataRowState.Deleted )
                        continue;

                    if ( row.vnd_vendor_id == _currentVendor.vnd_vendor_id )
                        continue;

                    if ( row.vendorcode.Trim() == _txtVendorID.Text.Trim() )
                    {
                        _Errors.SetError( _txtVendorID, Resources.DuplicateVendorID );
                        e.Cancel = true;
                        break;
                    }
                }
            }
        }

        private void _txtVendorID_Validated( object sender, EventArgs e )
        {
            _Errors.SetError( _txtVendorID, string.Empty );
        }

        private void Control_ValueChanged( object sender, EventArgs e )
        {
            _btnUpdate.Enabled = true;
            Dirty = true;
        }

        #endregion

        #region Private Methods

        private void LoadGrid()
        {
            foreach ( DataGridViewRow rowGrid in _gridVendors.Rows )
            {
                Administration.vnd_vendorRow rowVendor = (Administration.vnd_vendorRow)((DataRowView)rowGrid.DataBoundItem).Row;

                // Build up a string of the Vendor Types selected
                StringBuilder type = new StringBuilder();
                foreach ( Administration.vnd_vendorvendortype_mapRow rowMap in rowVendor.Getvnd_vendorvendortype_mapRows() )
                    type.AppendFormat( "{0}, ", rowMap.vnd_vendortypeRow.description );

                // Remoe the trailing comma and space
                if ( type.Length > 0 )
                    type.Remove( type.Length - 2, 2 );

                rowGrid.Cells["Type"].Value = type.ToString();
            }
        }

        private void LoadVendor()
        {
            bool tempDirty = Dirty;

            _txtVendorName.Text = _currentVendor.description;
            _txtVendorID.Text   = _currentVendor.vendorcode;
            _chkActive.Checked  = _currentVendor.active;

            // Find this checkbox in the Panel
            foreach ( Control ctrl in _groupSetup.Controls )
            {
                CheckBox check = ctrl as CheckBox;
                if ( check != null && check.Tag != null )
                {
                    int vendorTypeId = Convert.ToInt32( check.Tag );
                    Administration.vnd_vendorvendortype_mapRow mapRow = _dsAdministration.vnd_vendorvendortype_map.FindByvnd_vendor_idvnd_vendortype_id( _currentVendor.vnd_vendor_id, vendorTypeId );
                    if ( mapRow == null )
                        check.Checked = false;
                    else
                        check.Checked = true;
                }
            }

            _btnUpdate.Enabled = false;

            Dirty = tempDirty;
        }

        #endregion
    }
}