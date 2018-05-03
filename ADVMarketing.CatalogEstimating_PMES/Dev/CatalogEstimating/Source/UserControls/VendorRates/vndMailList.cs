#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CatalogEstimating.Datasets;

#endregion

namespace CatalogEstimating.UserControls.VendorRates
{
    public partial class vndMailList : CatalogEstimating.UserControls.VendorRates.VendorRateControl
    {
        #region Private Variables

        Administration.vnd_maillistresourcerateRow _currentRow = null;

        #endregion

        #region Construction

        public vndMailList()
        : this( null, null )
        { }

        public vndMailList( Administration ds, Administration.vnd_vendorRow vendor )
        : base( ds, vendor )
        {
            InitializeComponent();
            Name = "Mail List Resource Rates";
        }

        #endregion

        #region Override Methods

        public override IDictionary<DateTime, long> GetEffectiveDates()
        {
            IDictionary<DateTime, long> allEffectiveDates = new SortedList<DateTime, long>( new EffectiveDateComparer() );
            foreach ( Administration.vnd_maillistresourcerateRow detailRow in Dataset.vnd_maillistresourcerate.Rows )
            {
                if ( ( detailRow.RowState != DataRowState.Deleted ) && ( detailRow.vnd_vendor_id == Vendor.vnd_vendor_id ) )
                    allEffectiveDates.Add( detailRow.effectivedate, detailRow.vnd_maillistresourcerate_id );
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
                Administration.vnd_maillistresourcerateRow newRow = Dataset.vnd_maillistresourcerate.Newvnd_maillistresourcerateRow();
                newRow.effectivedate = DateTime.Today;

                if ( _currentRow != null && _currentRow.RowState != DataRowState.Detached )
                {
                    // Copy the current row to the new row
                    newRow.internallistrate = _currentRow.internallistrate;
                }
                else
                {
                    // Brand new row from scratch
                    newRow.internallistrate = 0M;
                }

                _dtEffectiveDate.Enabled = true;
                _currentRow = newRow;
            }
            else
            {
                // Editing a row out of the database
                _currentRow = Dataset.vnd_maillistresourcerate.FindByvnd_maillistresourcerate_id( id.Value );
                _dtEffectiveDate.Enabled = false;
            }

            // Initialize the GUI to the current row state
            _dtEffectiveDate.Value     = _currentRow.effectivedate;
            _txtInternalListRate.Value = _currentRow.internallistrate;
        }

        public override void Save()
        {
            // Force value to be sync'd with what may have been typed into date control.
            MainForm.UpdateDateControl(_dtEffectiveDate);

            if (_currentRow == null)
                return;

            // Save the current state of the GUI to the datarow
            _currentRow.internallistrate = _txtInternalListRate.Value.Value;
            if ( _dtEffectiveDate.Enabled )
                _currentRow.effectivedate = _dtEffectiveDate.Value;

            // This is a new row, not an updated row
            if ( _currentRow.RowState == DataRowState.Detached )
            {
                _currentRow.createdby     = MainForm.AuthorizedUser.FormattedName;
                _currentRow.createddate   = DateTime.Now;
                _currentRow.vnd_vendor_id = Vendor.vnd_vendor_id;

                Dataset.vnd_maillistresourcerate.Addvnd_maillistresourcerateRow( _currentRow );
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
            writer.WriteLine( _lblInternalListRate.Text, _txtInternalListRate.Text );
        }

        #endregion

        #region Event Handlers

        private void vndMailList_Validating( object sender, CancelEventArgs e )
        {
            if ( _currentRow == null )
                return;

            ValidateEffectiveDate( _dtEffectiveDate, e );
            ValidateRequired( _txtInternalListRate, e );
        }

        private void vndMailList_Load( object sender, EventArgs e )
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