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

namespace CatalogEstimating.UserControls.VendorRates
{
    public partial class vndMailHouse : CatalogEstimating.UserControls.VendorRates.VendorRateControl
    {
        #region Private Variables

        private Administration.vnd_mailhouserateRow _currentRow = null;

        #endregion

        #region Construction

        public vndMailHouse()
        : this( null, null )
        { }

        public vndMailHouse( Administration ds, Administration.vnd_vendorRow vendor )
        : base( ds, vendor )
        {
            InitializeComponent();
            Name = "Mail House Rates";

            // Create three empty rows
            _gridDefaultRates.Rows.Add( new object[] { "Glue Tack", false, "" } );
            _gridDefaultRates.Rows.Add( new object[] { "Tabbing", false, "" } );
            _gridDefaultRates.Rows.Add( new object[] { "Letter Insertion", false, "" } );
        }

        #endregion

        #region Override Methods

        public override IDictionary<DateTime, long> GetEffectiveDates()
        {
            IDictionary<DateTime, long> allEffectiveDates = new SortedList<DateTime, long>( new EffectiveDateComparer() );
            foreach ( Administration.vnd_mailhouserateRow detailRow in Dataset.vnd_mailhouserate.Rows )
            {
                if ( ( detailRow.RowState != DataRowState.Deleted ) && ( detailRow.vnd_vendor_id == Vendor.vnd_vendor_id ) )
                    allEffectiveDates.Add( detailRow.effectivedate, detailRow.vnd_mailhouserate_id );
            }
            return allEffectiveDates;
        }

        public override DateTime? EffectiveDate
        {
            get 
            {
                if ( _currentRow != null )
                {
                    if ( _currentRow.RowState == DataRowState.Detached )
                        return _dtEffectiveDate.Value;
                    else if ( _currentRow.RowState != DataRowState.Deleted )
                        return _currentRow.effectivedate;
                }
                return null;
            }
        }

        public override void Delete()
        {
            if ( _currentRow.RowState != DataRowState.Detached )
                _currentRow.Delete();
        }

        public override void Cancel()
        {
            _gridDefaultRates.CancelEdit();
            base.Cancel();
        }

        public override void EditRate( long? id )
        {
            if ( id == null )
            {
                Administration.vnd_mailhouserateRow newRow = Dataset.vnd_mailhouserate.Newvnd_mailhouserateRow();
                newRow.effectivedate = DateTime.Today;

                if ( _currentRow != null && _currentRow.RowState != DataRowState.Detached )
                {
                    // Copy the current row to the new row
                    newRow.timevalueslips         = _currentRow.timevalueslips;
                    newRow.inkjetrate             = _currentRow.inkjetrate;
                    newRow.inkjetmakeready        = _currentRow.inkjetmakeready;
                    newRow.adminfee               = _currentRow.adminfee;
                    newRow.postaldropcwt          = _currentRow.postaldropcwt;
                    newRow.gluetackdefault        = _currentRow.gluetackdefault;
                    newRow.gluetackrate           = _currentRow.gluetackrate;
                    newRow.tabbingdefault         = _currentRow.tabbingdefault;
                    newRow.tabbingrate            = _currentRow.tabbingrate;
                    newRow.letterinsertiondefault = _currentRow.letterinsertiondefault;
                    newRow.letterinsertionrate    = _currentRow.letterinsertionrate;
                }
                else
                {
                    // Brand new row from scratch
                    newRow.timevalueslips         = 0M;
                    newRow.inkjetrate             = 0M;
                    newRow.inkjetmakeready        = 0M;
                    newRow.adminfee               = 0M;
                    newRow.postaldropcwt          = 0M;
                    newRow.gluetackdefault        = false;
                    newRow.gluetackrate           = 0M;
                    newRow.tabbingdefault         = false;
                    newRow.tabbingrate            = 0M;
                    newRow.letterinsertiondefault = false;
                    newRow.letterinsertionrate    = 0M;
                }

                _dtEffectiveDate.Enabled = true;
                _currentRow = newRow;
            }
            else
            {
                // Editing a row out of the database
                _currentRow = Dataset.vnd_mailhouserate.FindByvnd_mailhouserate_id( id.Value );
                _dtEffectiveDate.Enabled = false;
            }

            // Initialize the GUI to the current row state
            _dtEffectiveDate.Value    = _currentRow.effectivedate;
            _txtTimeValueSlips.Value  = _currentRow.timevalueslips;
            _txtInkJetRate.Value      = _currentRow.inkjetrate;
            _txtInkJetMakeready.Value = _currentRow.inkjetmakeready;
            _txtAdminFee.Value        = _currentRow.adminfee;
            _txtPostalDrop.Value      = _currentRow.postaldropcwt;

            _gridDefaultRates.Rows[0].Cells[1].Value = _currentRow.gluetackdefault;
            _gridDefaultRates.Rows[0].Cells[2].Value = string.Format( "{0:F2}", _currentRow.gluetackrate );
            _gridDefaultRates.Rows[1].Cells[1].Value =  _currentRow.tabbingdefault;
            _gridDefaultRates.Rows[1].Cells[2].Value = string.Format( "{0:F2}", _currentRow.tabbingrate );
            _gridDefaultRates.Rows[2].Cells[1].Value = _currentRow.letterinsertiondefault;
            _gridDefaultRates.Rows[2].Cells[2].Value = string.Format( "{0:F2}", _currentRow.letterinsertionrate );
        }

        public override void Save()
        {
            // Force value to be sync'd with what may have been typed into date control.
            MainForm.UpdateDateControl(_dtEffectiveDate);

            if (_currentRow == null)
                return;

            // Save the current state of the GUI to the datarow
            _currentRow.timevalueslips  = _txtTimeValueSlips.Value.Value;
            _currentRow.inkjetrate      = _txtInkJetRate.Value.Value;
            _currentRow.inkjetmakeready = _txtInkJetMakeready.Value.Value;
            _currentRow.adminfee        = _txtAdminFee.Value.Value;
            _currentRow.postaldropcwt   = _txtPostalDrop.Value.Value;

            // Grab the current values out of the default grid
            _currentRow.gluetackdefault        = Convert.ToBoolean( _gridDefaultRates.Rows[0].Cells[1].Value );
            _currentRow.gluetackrate           = Convert.ToDecimal( _gridDefaultRates.Rows[0].Cells[2].Value );
            _currentRow.tabbingdefault         = Convert.ToBoolean( _gridDefaultRates.Rows[1].Cells[1].Value );
            _currentRow.tabbingrate            = Convert.ToDecimal( _gridDefaultRates.Rows[1].Cells[2].Value );
            _currentRow.letterinsertiondefault = Convert.ToBoolean( _gridDefaultRates.Rows[2].Cells[1].Value );
            _currentRow.letterinsertionrate    = Convert.ToDecimal( _gridDefaultRates.Rows[2].Cells[2].Value );
            
            if ( _dtEffectiveDate.Enabled )
                _currentRow.effectivedate = _dtEffectiveDate.Value;

            // This is a new row, not an updated row
            if ( _currentRow.RowState == DataRowState.Detached )
            {
                _currentRow.createdby     = MainForm.AuthorizedUser.FormattedName;
                _currentRow.createddate   = DateTime.Now;
                _currentRow.vnd_vendor_id = Vendor.vnd_vendor_id;

                Dataset.vnd_mailhouserate.Addvnd_mailhouserateRow( _currentRow );
            }
            else
            {
                _currentRow.modifiedby   = MainForm.AuthorizedUser.FormattedName;
                _currentRow.modifieddate = DateTime.Now;
            }
        }

        public override void Export( ExcelWriter writer )
        {
            writer.WriteLine( _lblEffectiveDate.Text, _dtEffectiveDate.Text );
            writer.WriteLine( _lblTimeValueSlips.Text, _txtTimeValueSlips.Text );
            writer.WriteLine( _lblInkJetRate.Text, _txtInkJetRate.Text );
            writer.WriteLine( _lblInkJetMakeready.Text, _txtInkJetMakeready.Text );
            writer.WriteLine( _lblAdminFee.Text, _txtAdminFee.Text );
            writer.WriteLine( _lblPostalDrop.Text, _txtPostalDrop.Text );
            writer.WriteLine();
            writer.WriteTable( _gridDefaultRates, true );
        }

        protected override void OnEnabledChanged( EventArgs e )
        {
            if ( MainForm.AuthorizedUser.Right == UserRights.Admin ||
                 MainForm.AuthorizedUser.Right == UserRights.SuperAdmin )
            {
                if ( Enabled )
                {
                    _gridDefaultRates.ReadOnly = false;
                    _gridDefaultRates.DefaultCellStyle.BackColor = SystemColors.Window;
                }
                else
                {
                    _gridDefaultRates.ReadOnly = true;
                    _gridDefaultRates.DefaultCellStyle.BackColor = SystemColors.Control;
                }
            }
            else
            {
                _gridDefaultRates.ReadOnly = true;
                _gridDefaultRates.DefaultCellStyle.BackColor = SystemColors.Control;
            }

            base.OnEnabledChanged( e );
        }

        #endregion

        #region Event Handlers

        private void vndMailHouse_Validating( object sender, CancelEventArgs e )
        {
            if ( _currentRow == null )
                return;

            ValidateEffectiveDate( _dtEffectiveDate, e );
            ValidateRequired( _txtTimeValueSlips, e );
            ValidateRequired( _txtInkJetRate, e );
            ValidateRequired( _txtInkJetMakeready, e );
            ValidateRequired( _txtAdminFee, e );
            ValidateRequired( _txtPostalDrop, e );
        }

        private void vndMailHouse_Load( object sender, EventArgs e )
        {
            if ( MainForm.AuthorizedUser.Right != UserRights.Admin &&
                 MainForm.AuthorizedUser.Right != UserRights.SuperAdmin )
            {
                this.Enabled = false;
            }
        }

        private void Control_ValueChanged( object sender, EventArgs e )
        {
            OnControlDirty( e );
        }

        private void _gridDefaultRates_CellValueChanged( object sender, DataGridViewCellEventArgs e )
        {
            OnControlDirty( EventArgs.Empty );
        }

        private void _gridDefaultRates_CellValidating( object sender, DataGridViewCellValidatingEventArgs e )
        {
            if ( _currentRow == null )
                return;

            if ( _gridDefaultRates.Columns[e.ColumnIndex].Name == "Rate" )
            {
                if ( string.IsNullOrEmpty( e.FormattedValue.ToString() ) )
                {
                    _gridDefaultRates.Rows[e.RowIndex].ErrorText = Resources.RequiredFieldError;
                    e.Cancel = true;
                }
                else
                    _gridDefaultRates.Rows[e.RowIndex].ErrorText = string.Empty;
            }
        }

        private void _gridDefaultRates_CellEndEdit( object sender, DataGridViewCellEventArgs e )
        {
            _gridDefaultRates.Rows[e.RowIndex].ErrorText = string.Empty;
        }

        #endregion
    }
}