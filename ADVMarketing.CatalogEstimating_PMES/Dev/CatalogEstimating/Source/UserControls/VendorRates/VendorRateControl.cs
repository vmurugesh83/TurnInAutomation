#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using CatalogEstimating.Datasets;
using CatalogEstimating.Properties;

#endregion

namespace CatalogEstimating.UserControls.VendorRates
{
    public partial class VendorRateControl : UserControl
    {
        #region Construction

        public VendorRateControl()
        {
            InitializeComponent();
        }

        protected VendorRateControl( Administration ds, Administration.vnd_vendorRow vendor )
        : this()
        {
            _dsAdministration = ds;
            _vendorRow = vendor;
        }

        #endregion

        #region Properties

        private Administration.vnd_vendorRow _vendorRow = null;
        [Browsable( false )]
        public Administration.vnd_vendorRow Vendor
        {
            get { return _vendorRow; }
        }

        private Administration _dsAdministration = null;
        [Browsable( false )]
        protected Administration Dataset
        {
            get { return _dsAdministration; }
        }

        public virtual DateTime? EffectiveDate
        {
            get { return null; }
        }

        #endregion

        #region Public Events

        public event EventHandler ControlDirty;

        #endregion

        #region Protected Methods

        protected bool ValidateEffectiveDate( DateTimePicker dtEffective, CancelEventArgs e )
        {
            if ( dtEffective.Enabled )
            {
                foreach ( DateTime dt in GetEffectiveDates().Keys )
                {
                    if ( dt.Date.Equals( dtEffective.Value.Date ) )
                    {
                        _Errors.SetError( dtEffective, Resources.DuplicateEffectiveDateError );
                        e.Cancel = true;
                        return false;
                    }
                }
            }

            _Errors.SetError( dtEffective, string.Empty );
            return true;
        }

        protected bool ValidateRequired( Control ctrl, CancelEventArgs e )
        {
            if ( ctrl.Enabled )
            {
                if ( string.IsNullOrEmpty( ctrl.Text ) )
                {
                    _Errors.SetError( ctrl, Resources.RequiredFieldError );
                    e.Cancel = true;
                    return false;
                }
            }

            _Errors.SetError( ctrl, string.Empty );
            return true;
        }

        protected virtual void OnControlDirty( EventArgs e )
        {
            if ( ControlDirty != null )
                ControlDirty( this, EventArgs.Empty );
        }

        #endregion

        #region Public Methods

        public virtual IDictionary<DateTime, long> GetEffectiveDates()
        {
            return new SortedList<DateTime, long>();
        }

        public virtual void EditRate( long? id )
        { }

        public virtual void Save()
        { }

        public virtual void Cancel()
        { }

        public virtual void Delete()
        { }

        public virtual void Export( ExcelWriter writer )
        { }

        #endregion

        #region EffectiveDateComparer Class

        protected class EffectiveDateComparer : IComparer<DateTime>
        {
            public int Compare( DateTime x, DateTime y )
            {
                return -1 * DateTime.Compare( x, y );
            }
        }

        #endregion
    }
}