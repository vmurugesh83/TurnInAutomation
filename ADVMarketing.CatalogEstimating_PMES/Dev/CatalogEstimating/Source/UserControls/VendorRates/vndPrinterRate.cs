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
    public partial class vndPrinterRate : UserControl
    {
        #region Private Variables

        Administration.prt_printerratetypeRow _typeRow = null;
        DataView _view = null;

        #endregion

        #region Construction

        public vndPrinterRate()
        {
            InitializeComponent();
        }

        public vndPrinterRate( Administration.prt_printerratetypeRow typeRow )
        : this()
        {
            _typeRow = typeRow;
            _lblGridTitle.Text = typeRow.gridtitle;
        }

        #endregion

        #region Public Events

        public event EventHandler ControlDirty;

        #endregion

        #region Public Properties

        private Administration.vnd_printerRow _printerRow = null;
        public Administration.vnd_printerRow Printer
        {
            get { return _printerRow;  }
            set { _printerRow = value; }
        }

        private Administration.prt_printerrateDataTable _workingCopy = null;
        public Administration.prt_printerrateDataTable WorkingCopy
        {
            get { return _workingCopy;  }
            set 
            {
                _gridPrinterRate.CancelEdit();

                _workingCopy = value;
                _view = new DataView( _workingCopy );
                _view.RowFilter = string.Format( "prt_printerratetype_id = {0} AND vnd_printer_id = {1}", _typeRow.prt_printerratetype_id, _printerRow.vnd_printer_id );
                _view.Sort = prtprinterrateBindingSource.Sort;
                _gridPrinterRate.DataSource = _view;
            }
        }

        #endregion

        #region Event Handlers

        private void _gridPrinterRate_DefaultValuesNeeded( object sender, DataGridViewRowEventArgs e )
        {
            if ( ( _printerRow != null ) && ( _typeRow != null ) )
            {
                e.Row.Cells["createddateDataGridViewTextBoxColumn"].Value = DateTime.Now;
                e.Row.Cells["createdbyDataGridViewTextBoxColumn"].Value = MainForm.AuthorizedUser.FormattedName;
                e.Row.Cells["vndprinteridDataGridViewTextBoxColumn"].Value = _printerRow.vnd_printer_id;
                e.Row.Cells["prtprinterratetypeidDataGridViewTextBoxColumn"].Value = _typeRow.prt_printerratetype_id;
                e.Row.Cells["rateDataGridViewTextBoxColumn"].Value = 0;
                e.Row.Cells["defaultDataGridViewCheckBoxColumn"].Value = false;
                e.Row.Cells["descriptionDataGridViewTextBoxColumn"].Value = string.Empty;
            }
        }

        private void vndPrinterRate_Load( object sender, EventArgs e )
        {
            if ( MainForm.AuthorizedUser.Right != UserRights.Admin &&
                 MainForm.AuthorizedUser.Right != UserRights.SuperAdmin )
            {
                Enabled = false;
            }
        }

        private void _gridPrinterRate_CellValueChanged( object sender, DataGridViewCellEventArgs e )
        {
            OnControlDirty( EventArgs.Empty );
        }

        private void _gridPrinterRate_UserDeletingRow( object sender, DataGridViewRowCancelEventArgs e )
        {
            DataRowView drv = (DataRowView)e.Row.DataBoundItem;
            Datasets.Administration.prt_printerrateRow pr_row = (Datasets.Administration.prt_printerrateRow)drv.Row;
            pr_row.modifiedby = MainForm.AuthorizedUser.FormattedName;
            OnControlDirty( EventArgs.Empty );
        }

        private void _gridPrinterRate_RowValidating( object sender, DataGridViewCellCancelEventArgs e )
        {
            if ( _gridPrinterRate.Rows[e.RowIndex].IsNewRow )
                return;

            string rate = _gridPrinterRate.Rows[e.RowIndex].Cells["rateDataGridViewTextBoxColumn"].EditedFormattedValue.ToString();
            string desc = _gridPrinterRate.Rows[e.RowIndex].Cells["descriptionDataGridViewTextBoxColumn"].EditedFormattedValue.ToString().Trim();

            // Save back the trimmed value to the grid
            _gridPrinterRate.Rows[e.RowIndex].Cells["descriptionDataGridViewTextBoxColumn"].Value = desc;

            if ( string.IsNullOrEmpty( rate ) || string.IsNullOrEmpty( desc ) )
            {
                _gridPrinterRate.Rows[e.RowIndex].ErrorText = Resources.RequiredFieldError;
                e.Cancel = true;
                return;
            }

            // Make sure that they aren't trying to have multiple default's or duplicate descriptions
            bool defaultExists = false;
            bool descExists    = false;
            foreach ( DataGridViewRow rowView in _gridPrinterRate.Rows )
            {
                // Checks to see if there is already a default on an existing row that's not the one
                // we're currently editing in the grid
                if ( rowView.Index != e.RowIndex )
                {
                    bool rowDefault = Convert.ToBoolean( rowView.Cells["defaultDataGridViewCheckBoxColumn"].EditedFormattedValue );
                    if ( rowDefault )
                    {
                        defaultExists = true;
                        break;
                    }

                    string rowDesc = rowView.Cells["descriptionDataGridViewTextBoxColumn"].EditedFormattedValue.ToString();
                    if ( rowDesc == desc )
                    {
                        descExists = true;
                        break;
                    }
                }
            }

            // If another row has a default, and they're trying to check this one, then error
            if ( defaultExists && Convert.ToBoolean( _gridPrinterRate.Rows[e.RowIndex].Cells["defaultDataGridViewCheckBoxColumn"].EditedFormattedValue ) == true )
            {
                _gridPrinterRate.Rows[e.RowIndex].ErrorText = Resources.MultipleDefaultsError;
                e.Cancel = true;
            }

            if ( descExists )
            {
                _gridPrinterRate.Rows[e.RowIndex].ErrorText = Resources.DuplicateDescriptionError;
                e.Cancel = true;
            }
        }

        private void _gridPrinterRate_RowValidated( object sender, DataGridViewCellEventArgs e )
        {
            _gridPrinterRate.Rows[e.RowIndex].ErrorText = string.Empty;
        }

        private int GetRowCount()
        {
            // If there are any rate rows, then there must be a default
            int rowCount = 0;
            foreach ( DataGridViewRow rowView in _gridPrinterRate.Rows )
            {
                if ( !rowView.IsNewRow )
                    rowCount++;
            }

            return rowCount;
        }

        private void vndPrinterRate_Validating( object sender, CancelEventArgs e )
        {
            if ( GetRowCount() > 1 )
            {
                bool defaultExists = false;
                foreach ( DataGridViewRow rowView in _gridPrinterRate.Rows )
                {
                    bool rowDefault = Convert.ToBoolean( rowView.Cells["defaultDataGridViewCheckBoxColumn"].EditedFormattedValue );
                    if ( rowDefault )
                    {
                        defaultExists = true;
                        break;
                    }
                }

                if ( !defaultExists )
                {
                    _lblValidationError.Text = Resources.DefaultItemRequired;
                    _lblValidationError.Visible = true;
                    e.Cancel = true;
                }
            }
        }

        private void vndPrinterRate_Validated( object sender, EventArgs e )
        {
            _lblValidationError.Visible = false;
        }

        private void _gridPrinterRate_DataError( object sender, DataGridViewDataErrorEventArgs e )
        {
            _gridPrinterRate.Rows[e.RowIndex].ErrorText = Resources.RequiredFieldError;
            e.Cancel = true;
        }

        #endregion

        #region Public Methods

        public void Export( ExcelWriter writer )
        {
            writer.WriteTable( _gridPrinterRate, true );
        }

        public void Cancel()
        {
            _gridPrinterRate.CancelEdit();
        }

        public void ForceDefault()
        {
            // If there is only one row, then make it the default
            if ( GetRowCount() == 1 )
            {
                Administration.prt_printerrateRow row = (Administration.prt_printerrateRow)_view[0].Row;
                row._default = true;
            }
        }

        #endregion

        #region Protected Methods

        protected virtual void OnControlDirty( EventArgs e )
        {
            if ( ControlDirty != null )
                ControlDirty( this, e );
        }

        protected override void OnEnabledChanged( EventArgs e )
        {
            if ( MainForm.AuthorizedUser.Right == UserRights.Admin ||
                 MainForm.AuthorizedUser.Right == UserRights.SuperAdmin )
            {
                if ( Enabled )
                {
                    _gridPrinterRate.ReadOnly = false;
                    _gridPrinterRate.AllowUserToAddRows = true;
                    _gridPrinterRate.AllowUserToDeleteRows = true;
                    _gridPrinterRate.DefaultCellStyle.BackColor = SystemColors.Window;
                }
                else
                {
                    _gridPrinterRate.ReadOnly = true;
                    _gridPrinterRate.AllowUserToAddRows = false;
                    _gridPrinterRate.AllowUserToDeleteRows = false;
                    _gridPrinterRate.DefaultCellStyle.BackColor = SystemColors.Control;
                }
            }
            else
            {
                _gridPrinterRate.ReadOnly = true;
                _gridPrinterRate.AllowUserToAddRows = false;
                _gridPrinterRate.AllowUserToDeleteRows = false;
                _gridPrinterRate.DefaultCellStyle.BackColor = SystemColors.Control;
            }


            base.OnEnabledChanged( e );
        }

        #endregion
    }
}
