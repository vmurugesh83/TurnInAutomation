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
    public partial class vndPrinter : CatalogEstimating.UserControls.VendorRates.VendorRateControl
    {
        #region Private Variables

        private Administration _dsAdministration = null;
        private Administration.vnd_printerRow _currentRow = null;
        private Administration.prt_printerrateDataTable _workingCopy = null;

        #endregion

        #region Construction

        public vndPrinter()
        : this( null, null )
        { }

        public vndPrinter( Administration ds, Administration.vnd_vendorRow vendor )
        : base( ds, vendor )
        {
            InitializeComponent();
            Name = "Printer Rates";
            _dsAdministration = ds;

            // Create three empty rows
            _gridDefaultRates.Rows.Add( new object[] { "Corner Guard", false, "" } );
            _gridDefaultRates.Rows.Add( new object[] { "Skids", false, "" } );
            _gridDefaultRates.Rows.Add( new object[] { "Polybag Message", false, "" } );
            _gridDefaultRates.Rows.Add( new object[] { "Polybag Message Makeready", false, "" } );

            // Populate the tab control with a tab per printer rate type and an appropriate grid control
            foreach ( Administration.prt_printerratetypeRow typeRow in ds.prt_printerratetype )
            {
                // Store the type id in the Tab Page Tag for easy access later
                TabPage typePage = new TabPage( typeRow.description );
                typePage.Tag = typeRow.prt_printerratetype_id;
                typePage.UseVisualStyleBackColor = true;

                // Create the user control that encapsulates this rate type
                vndPrinterRate rateControl = new vndPrinterRate( typeRow );
                rateControl.Dock = DockStyle.Fill;
                rateControl.ControlDirty += new EventHandler( Control_ValueChanged );
                typePage.Controls.Add( rateControl );
                
                _tabControl.TabPages.Add( typePage );
            }
        }

        #endregion

        #region Override Methods

        public override IDictionary<DateTime, long> GetEffectiveDates()
        {
            IDictionary<DateTime, long> allEffectiveDates = new SortedList<DateTime, long>( new EffectiveDateComparer() );
            foreach ( Administration.vnd_printerRow detailRow in Dataset.vnd_printer.Rows )
            {
                if ( ( detailRow.RowState != DataRowState.Deleted ) && ( detailRow.vnd_vendor_id == Vendor.vnd_vendor_id ) )
                    allEffectiveDates.Add( detailRow.effectivedate, detailRow.vnd_printer_id );
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

            foreach ( TabPage ratePage in _tabControl.TabPages )
            {
                vndPrinterRate rateControl = ratePage.Controls[0] as vndPrinterRate;
                rateControl.Cancel();
            }

            base.Cancel();
        }

        public override void EditRate( long? id )
        {
            _workingCopy = (Administration.prt_printerrateDataTable)_dsAdministration.prt_printerrate.Copy();

            if ( id == null )
            {
                Administration.vnd_printerRow newRow = Dataset.vnd_printer.Newvnd_printerRow();
                newRow.effectivedate = DateTime.Today;

                if ( _currentRow != null && _currentRow.RowState != DataRowState.Detached )
                {
                    // Copy the current row to the new row
                    newRow.paperhandling                  = _currentRow.paperhandling;
                    newRow.polybagbagweight               = _currentRow.polybagbagweight;
                    newRow.cornerguarddefault             = _currentRow.cornerguarddefault;
                    newRow.cornerguard                    = _currentRow.cornerguard;
                    newRow.skiddefault                    = _currentRow.skiddefault;
                    newRow.skid                           = _currentRow.skid;
                    newRow.polybagmessagedefault          = _currentRow.polybagmessagedefault;
                    newRow.polybagmessage                 = _currentRow.polybagmessage;
                    newRow.polybagmessagemakereadydefault = _currentRow.polybagmessagemakereadydefault;
                    newRow.polybagmessagemakeready        = _currentRow.polybagmessagemakeready;

                    foreach ( Administration.prt_printerrateRow rateRow in _currentRow.Getprt_printerrateRows() )
                    {
                        Administration.prt_printerrateRow newRate = _workingCopy.Newprt_printerrateRow();
                        newRate.prt_printerratetype_id = rateRow.prt_printerratetype_id;
                        newRate.vnd_printer_id = newRow.vnd_printer_id;
                        newRate.createdby = MainForm.AuthorizedUser.FormattedName;
                        newRate.createddate = DateTime.Now;
                        newRate.SetmodifiedbyNull();
                        newRate.SetmodifieddateNull();
                        newRate.description = rateRow.description;
                        newRate.rate = rateRow.rate;
                        newRate._default = rateRow._default;

                        _workingCopy.Addprt_printerrateRow( newRate );
                    }
                }
                else
                {
                    // Brand new row from scratch
                    newRow.paperhandling                  = 0M;
                    newRow.polybagbagweight               = 0M;
                    newRow.cornerguarddefault             = false;
                    newRow.cornerguard                    = 0M;
                    newRow.skiddefault                    = false;
                    newRow.skid                           = 0M;
                    newRow.polybagmessagedefault          = false;
                    newRow.polybagmessage                 = 0M;
                    newRow.polybagmessagemakereadydefault = false;
                    newRow.polybagmessagemakeready        = 0M;
                }

                _dtEffectiveDate.Enabled = true;
                _currentRow = newRow;
            }
            else
            {
                // Editing a row out of the database
                _currentRow = Dataset.vnd_printer.FindByvnd_printer_id( id.Value );
                _dtEffectiveDate.Enabled = false;
            }

            // Initialize the GUI to the current row state
            _dtEffectiveDate.Value     = _currentRow.effectivedate;
            _txtPaperHandling.Value    = _currentRow.paperhandling;
            _txtPolybagBagWeight.Value = _currentRow.polybagbagweight;

            _gridDefaultRates.Rows[0].Cells[1].Value = _currentRow.cornerguarddefault;
            _gridDefaultRates.Rows[0].Cells[2].Value = string.Format( "{0:F2}", _currentRow.cornerguard );
            _gridDefaultRates.Rows[1].Cells[1].Value = _currentRow.skiddefault;
            _gridDefaultRates.Rows[1].Cells[2].Value = string.Format( "{0:F2}", _currentRow.skid );
            _gridDefaultRates.Rows[2].Cells[1].Value = _currentRow.polybagmessagedefault;
            _gridDefaultRates.Rows[2].Cells[2].Value = string.Format( "{0:F2}", _currentRow.polybagmessage );
            _gridDefaultRates.Rows[3].Cells[1].Value = _currentRow.polybagmessagemakereadydefault;
            _gridDefaultRates.Rows[3].Cells[2].Value = string.Format( "{0:F2}", _currentRow.polybagmessagemakeready );

            foreach ( TabPage ratePage in _tabControl.TabPages )
            {
                vndPrinterRate rateControl = ratePage.Controls[0] as vndPrinterRate;
                rateControl.Printer        = _currentRow;
                rateControl.WorkingCopy    = _workingCopy;
            }
        }

        public override void Save()
        {
            // Force value to be sync'd with what may have been typed into date control.
            MainForm.UpdateDateControl(_dtEffectiveDate);

            if (_currentRow == null)
                return;

            // Force a default on each rate tab if there is only one rate on the tab
            foreach ( TabPage ratePage in _tabControl.TabPages )
            {
                vndPrinterRate rateControl = ratePage.Controls[0] as vndPrinterRate;
                rateControl.ForceDefault();
            }
            
            // Save the current state of the GUI to the datarow
            _currentRow.paperhandling    = _txtPaperHandling.Value.Value;
            _currentRow.polybagbagweight = _txtPolybagBagWeight.Value.Value;

            // Grab the current values out of the default grid
            _currentRow.cornerguarddefault             = Convert.ToBoolean( _gridDefaultRates.Rows[0].Cells[1].Value );
            _currentRow.cornerguard                    = Convert.ToDecimal( _gridDefaultRates.Rows[0].Cells[2].Value );
            _currentRow.skiddefault                    = Convert.ToBoolean( _gridDefaultRates.Rows[1].Cells[1].Value );
            _currentRow.skid                           = Convert.ToDecimal( _gridDefaultRates.Rows[1].Cells[2].Value );
            _currentRow.polybagmessagedefault          = Convert.ToBoolean( _gridDefaultRates.Rows[2].Cells[1].Value );
            _currentRow.polybagmessage                 = Convert.ToDecimal( _gridDefaultRates.Rows[2].Cells[2].Value );
            _currentRow.polybagmessagemakereadydefault = Convert.ToBoolean( _gridDefaultRates.Rows[3].Cells[1].Value );
            _currentRow.polybagmessagemakeready        = Convert.ToDecimal( _gridDefaultRates.Rows[3].Cells[2].Value );

            if ( _dtEffectiveDate.Enabled )
                _currentRow.effectivedate = _dtEffectiveDate.Value;

            // This is a new row, not an updated row
            if ( _currentRow.RowState == DataRowState.Detached )
            {
                _currentRow.createdby     = MainForm.AuthorizedUser.FormattedName;
                _currentRow.createddate   = DateTime.Now;
                _currentRow.vnd_vendor_id = Vendor.vnd_vendor_id;

                Dataset.vnd_printer.Addvnd_printerRow( _currentRow );
            }
            else
            {
                _currentRow.modifiedby   = MainForm.AuthorizedUser.FormattedName;
                _currentRow.modifieddate = DateTime.Now;
            }

            _dsAdministration.prt_printerrate.Merge( _workingCopy );
        }

        public override void Export( ExcelWriter writer )
        {
            writer.WriteLine( _lblEffectiveDate.Text, _dtEffectiveDate.Text );
            writer.WriteLine( _lblPaperHandling.Text, _txtPaperHandling.Text );
            writer.WriteLine( _lblPolybagBagWeight.Text, _txtPolybagBagWeight.Text );
            writer.WriteLine();
            writer.WriteTable( _gridDefaultRates, true );

            foreach ( TabPage page in _tabControl.TabPages )
            {
                vndPrinterRate rateControl = page.Controls[0] as vndPrinterRate;
                if ( rateControl != null )
                {
                    writer.WriteLine();
                    writer.WriteLine( page.Text );
                    rateControl.Export( writer );
                }
            }
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
                    _gridDefaultRates.Columns[0].ReadOnly = true;
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

        private void vndPrinter_Validating( object sender, CancelEventArgs e )
        {
            if ( _currentRow == null )
                return;

            ValidateEffectiveDate( _dtEffectiveDate, e );
            ValidateRequired( _txtPaperHandling, e );
            ValidateRequired( _txtPolybagBagWeight, e );

            for ( int iRow = 0; iRow < _gridDefaultRates.RowCount; iRow++ )
            {
                if ( _gridDefaultRates.Rows[iRow].Cells[2].Value == null )
                {
                    _gridDefaultRates.Rows[iRow].ErrorText = Resources.RequiredFieldError;
                    e.Cancel = true;
                }
                else
                    _gridDefaultRates.Rows[iRow].ErrorText = string.Empty;
            }
        }

        private void _gridDefaultRates_RowValidating( object sender, DataGridViewCellCancelEventArgs e )
        {
            if ( _currentRow == null )
                return;

            if ( _gridDefaultRates.Rows[e.RowIndex].IsNewRow )
                return;

            if ( _gridDefaultRates.Rows[e.RowIndex].Cells[2].EditedFormattedValue.ToString() == string.Empty )
            {
                _gridDefaultRates.Rows[e.RowIndex].ErrorText = Resources.RequiredFieldError;
                e.Cancel = true;
            }
            else
                _gridDefaultRates.Rows[e.RowIndex].ErrorText = string.Empty;
        }

        private void vndPrinter_Load( object sender, EventArgs e )
        {
            if ( MainForm.AuthorizedUser.Right != UserRights.Admin &&
                 MainForm.AuthorizedUser.Right != UserRights.SuperAdmin )
            {
                Enabled = false;
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

        private void _tabControl_Selecting( object sender, TabControlCancelEventArgs e )
        {
            vndPrinterRate rateControl = _tabControl.SelectedTab.Controls[0] as vndPrinterRate;
            if ( rateControl != null )
            {
                if ( !rateControl.Validate() )
                    e.Cancel = true;
            }
        }

        #endregion
    }
}