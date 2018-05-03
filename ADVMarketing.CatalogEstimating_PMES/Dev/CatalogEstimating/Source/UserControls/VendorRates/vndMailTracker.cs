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
    public partial class vndMailTracker : CatalogEstimating.UserControls.VendorRates.VendorRateControl
    {
        #region Private Variables

        private Administration.vnd_mailtrackingrateRow _currentRow = null;

        #endregion

        #region Construction

        public vndMailTracker()
        : this( null, null )
        { }

        public vndMailTracker( Administration ds, Administration.vnd_vendorRow vendor )
        : base( ds, vendor )
        {
            InitializeComponent();
            Name = "Mail Tracker Rates";
        }

        #endregion

        #region Override Methods

        public override IDictionary<DateTime, long> GetEffectiveDates()
        {
            IDictionary<DateTime, long> allEffectiveDates = new SortedList<DateTime, long>( new EffectiveDateComparer() );
            foreach ( Administration.vnd_mailtrackingrateRow detailRow in Dataset.vnd_mailtrackingrate.Rows )
            {
                if ( ( detailRow.RowState != DataRowState.Deleted ) && ( detailRow.vnd_vendor_id == Vendor.vnd_vendor_id ) )
                    allEffectiveDates.Add( detailRow.effectivedate, detailRow.vnd_mailtrackingrate_id );
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

        public override void EditRate( long? id )
        {
            if ( id == null )
            {
                Administration.vnd_mailtrackingrateRow newRow = Dataset.vnd_mailtrackingrate.Newvnd_mailtrackingrateRow();
                newRow.effectivedate = DateTime.Today;

                if ( _currentRow != null && _currentRow.RowState != DataRowState.Detached )
                {
                    // Copy the current row to the new row
                    newRow.mailtracking  = _currentRow.mailtracking;
                }
                else
                {
                    // Brand new row from scratch
                    newRow.mailtracking  = 0M;
                }

                _dtEffectiveDate.Enabled = true;
                _currentRow = newRow;
            }
            else
            {
                // Editing a row out of the database
                _currentRow = Dataset.vnd_mailtrackingrate.FindByvnd_mailtrackingrate_id( id.Value );
                _dtEffectiveDate.Enabled = false;
            }

            // Initialize the GUI to the current row state
            _dtEffectiveDate.Value     = _currentRow.effectivedate;
            _txtMailTrackingRate.Value = _currentRow.mailtracking;
        }

        public override void Save()
        {
            // Force value to be sync'd with what may have been typed into date control.
            MainForm.UpdateDateControl(_dtEffectiveDate);

            if (_currentRow == null)
                return;

            // Save the current state of the GUI to the datarow
            _currentRow.mailtracking = _txtMailTrackingRate.Value.Value;
            if ( _dtEffectiveDate.Enabled )
                _currentRow.effectivedate = _dtEffectiveDate.Value;

            // This is a new row, not an updated row
            if ( _currentRow.RowState == DataRowState.Detached )
            {
                _currentRow.createdby     = MainForm.AuthorizedUser.FormattedName;
                _currentRow.createddate   = DateTime.Now;
                _currentRow.vnd_vendor_id = Vendor.vnd_vendor_id;

                Dataset.vnd_mailtrackingrate.Addvnd_mailtrackingrateRow( _currentRow );
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
            writer.WriteLine( _lblMailTrackingRate.Text, _txtMailTrackingRate.Text );
        }

        #endregion

        #region Event Handlers

        private void vndMailTracker_Validating( object sender, CancelEventArgs e )
        {
            if ( _currentRow == null )
                return;

            ValidateEffectiveDate( _dtEffectiveDate, e );
            ValidateRequired( _txtMailTrackingRate, e );
        }

        private void vndMailTracker_Load( object sender, EventArgs e )
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

        #endregion
    }
}