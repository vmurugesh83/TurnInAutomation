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
    public partial class vndPaper : CatalogEstimating.UserControls.VendorRates.VendorRateControl
    {
        #region Private Variables

        private Administration.vnd_paperRow _currentRow = null;
        private Administration.ppr_paper_mapDataTable _workingCopy = null;
        private long _paperId = -1;

        #endregion

        #region Construction

        public vndPaper()
        : this( null, null )
        { }

        public vndPaper( Administration ds, Administration.vnd_vendorRow vendor )
        : base( ds, vendor )
        {
            InitializeComponent();
            Name = "Paper Rates";

            _dsAdministration = ds;
            pprpapergradeBindingSource.DataSource = ds;
            pprpaperweightBindingSource.DataSource = ds;
        }

        #endregion

        #region Public Overrides

        public override IDictionary<DateTime, long> GetEffectiveDates()
        {
            IDictionary<DateTime, long> allEffectiveDates = new SortedList<DateTime, long>( new EffectiveDateComparer() );
            foreach ( Administration.vnd_paperRow detailRow in Dataset.vnd_paper.Rows )
            {
                if ( ( detailRow.RowState != DataRowState.Deleted ) && ( detailRow.vnd_vendor_id == Vendor.vnd_vendor_id ) )
                    allEffectiveDates.Add( detailRow.effectivedate, detailRow.vnd_paper_id );
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
            pprpapermapBindingSource.CancelEdit();
            base.Cancel();
        }

        public override void EditRate( long? id )
        {
            pprpapergradeBindingSource.CancelEdit();
            _workingCopy = (Administration.ppr_paper_mapDataTable)_dsAdministration.ppr_paper_map.Copy();
            pprpapermapBindingSource.DataSource = _workingCopy;

            if ( id == null )
            {
                Administration.vnd_paperRow newRow = Dataset.vnd_paper.Newvnd_paperRow();
                newRow.effectivedate = DateTime.Today;

                if ( _currentRow != null && _currentRow.RowState != DataRowState.Detached )
                {
                    // Copy the current row to the new row
                    foreach ( Administration.ppr_paper_mapRow mapRow in _currentRow.Getppr_paper_mapRows() )
                    {
                        Administration.ppr_paper_mapRow newMapRow = _workingCopy.Newppr_paper_mapRow();
                        newMapRow._default = mapRow._default;
                        newMapRow.createdby = MainForm.AuthorizedUser.FormattedName;
                        newMapRow.createddate = DateTime.Now;
                        newMapRow.SetmodifiedbyNull();
                        newMapRow.SetmodifieddateNull();
                        newMapRow.cwt = mapRow.cwt;
                        newMapRow.description = mapRow.description;
                        newMapRow.ppr_papergrade_id = mapRow.ppr_papergrade_id;
                        newMapRow.ppr_paperweight_id = mapRow.ppr_paperweight_id;
                        newMapRow.vnd_paper_id = newRow.vnd_paper_id;
                        _workingCopy.Addppr_paper_mapRow( newMapRow );
                    }
                }
                else
                {
                    // Brand new row from scratch.  Nothing to do here.  Just have the else
                    // to fit the pattern of the other rate controls
                }

                _dtEffectiveDate.Enabled = true;
                _currentRow = newRow;
                _paperId = _currentRow.vnd_paper_id;
            }
            else
            {
                // Editing a row out of the database
                _currentRow = Dataset.vnd_paper.FindByvnd_paper_id( id.Value );
                _dtEffectiveDate.Enabled = false;
                _paperId = id.Value;
            }

            // Initialize the GUI to the current row state
            _dtEffectiveDate.Value = _currentRow.effectivedate;
            pprpapermapBindingSource.Filter = string.Format( "vnd_paper_id = {0}", _currentRow.vnd_paper_id );

            // If there is not at least one weight and one grade, disable the grid
            if ( _dsAdministration.ppr_papergrade.Rows.Count == 0 || _dsAdministration.ppr_paperweight.Rows.Count == 0 )
                _gridPaper.Enabled = false;
            else
            {
                if ( MainForm.AuthorizedUser.Right != UserRights.Admin &&
                     MainForm.AuthorizedUser.Right != UserRights.SuperAdmin )
                {
                    _gridPaper.Enabled = true;
                }
            }
        }

        public override void Save()
        {
            // Force value to be sync'd with what may have been typed into date control.
            MainForm.UpdateDateControl(_dtEffectiveDate);

            if (_currentRow == null)
                return;

            // Force a default on the paper rate if there is only one
            if ( GetRowCount() == 1 )
            {
                DataRowView rowView = (DataRowView)_gridPaper.Rows[0].DataBoundItem;
                Administration.ppr_paper_mapRow row = (Administration.ppr_paper_mapRow)rowView.Row;
                row._default = true;
            }

            // Save the current state of the GUI to the datarow
            if ( _dtEffectiveDate.Enabled )
                _currentRow.effectivedate = _dtEffectiveDate.Value;

            // This is a new row, not an updated row
            if ( _currentRow.RowState == DataRowState.Detached )
            {
                _currentRow.createdby = MainForm.AuthorizedUser.FormattedName;
                _currentRow.createddate = DateTime.Now;
                _currentRow.vnd_vendor_id = Vendor.vnd_vendor_id;

                Dataset.vnd_paper.Addvnd_paperRow( _currentRow );
            }
            else
            {
                _currentRow.modifiedby = MainForm.AuthorizedUser.FormattedName;
                _currentRow.modifieddate = DateTime.Now;
            }

            pprpapergradeBindingSource.CancelEdit();
            _dsAdministration.ppr_paper_map.Merge( _workingCopy );
        }

        public override void Export( ExcelWriter writer )
        {
            writer.WriteLine( _lblEffectiveDate.Text, _dtEffectiveDate.Text );
            writer.WriteLine();
            writer.WriteTable( _gridPaper, true );
        }

        #endregion

        #region Event Handlers

        private void _btnManagePaper_Click( object sender, EventArgs e )
        {
            PaperMgmtDialog dlg = new PaperMgmtDialog( _dsAdministration );
            dlg.ShowDialog( this );

            // If there is now at least one weight and one grade, can enable the grid control
            if ( _dsAdministration.ppr_paperweight.Rows.Count > 0 && _dsAdministration.ppr_papergrade.Rows.Count > 0 )
                _gridPaper.Enabled = true;
            else
                _gridPaper.Enabled = false;
        }

        private void _gridPaper_DefaultValuesNeeded( object sender, DataGridViewRowEventArgs e )
        {
            if ( _currentRow != null )
            {
                e.Row.Cells["createddateDataGridViewTextBoxColumn"].Value = DateTime.Now;
                e.Row.Cells["createdbyDataGridViewTextBoxColumn"].Value   = MainForm.AuthorizedUser.FormattedName;
                e.Row.Cells["vndpaperidDataGridViewTextBoxColumn"].Value  = _currentRow.vnd_paper_id;
                e.Row.Cells["defaultDataGridViewCheckBoxColumn"].Value    = false;
                e.Row.Cells["descriptionDataGridViewTextBoxColumn"].Value = "";
                e.Row.Cells["cwtDataGridViewTextBoxColumn"].Value         = 0;
            }
        }

        private void _gridPaper_RowValidating( object sender, DataGridViewCellCancelEventArgs e )
        {
            if ( ( _currentRow == null ) || ( _gridPaper.Rows[e.RowIndex].IsNewRow ) )
                return;

            string strValidatingPaperId = _gridPaper.Rows[e.RowIndex].Cells["vndpaperidDataGridViewTextBoxColumn"].EditedFormattedValue.ToString();
            if ( string.IsNullOrEmpty( strValidatingPaperId ) )
                return;

            long validatingPaperId = Convert.ToInt64( strValidatingPaperId );
            if ( validatingPaperId != _paperId )
                return;

            // Verify that the other cells have values in them
            string weight = _gridPaper.Rows[e.RowIndex].Cells["pprpaperweightidDataGridViewTextBoxColumn"].EditedFormattedValue.ToString();
            string grade  = _gridPaper.Rows[e.RowIndex].Cells["pprpapergradeidDataGridViewTextBoxColumn"].EditedFormattedValue.ToString();
            string desc   = _gridPaper.Rows[e.RowIndex].Cells["descriptionDataGridViewTextBoxColumn"].EditedFormattedValue.ToString().Trim();

            // Save back the trimmed value to the grid
            _gridPaper.Rows[e.RowIndex].Cells["descriptionDataGridViewTextBoxColumn"].Value = desc;

            if ( string.IsNullOrEmpty( weight ) || string.IsNullOrEmpty( grade ) || string.IsNullOrEmpty( desc ) )
            {
                _gridPaper.Rows[e.RowIndex].ErrorText = Resources.RequiredFieldError;
                e.Cancel = true;
                return;
            }

            // Make sure that they aren't trying to have multiple default's.  Also make sure that the 
            // combination of description, weight and grade have to be unique.
            int defaultCount = 0;
            int uniqueCount  = 0;
            foreach ( DataGridViewRow rowView in _gridPaper.Rows )
            {
                // Checks to see if there is already a default on an existing row that's not the one
                // we're currently editing in the grid
                if ( string.IsNullOrEmpty( rowView.Cells["vndpaperidDataGridViewTextBoxColumn"].EditedFormattedValue.ToString() ) )
                    continue;

                long paperId = Convert.ToInt64( rowView.Cells["vndpaperidDataGridViewTextBoxColumn"].EditedFormattedValue );

                if ( paperId == _paperId )
                {
                    bool rowDefault = Convert.ToBoolean( rowView.Cells["defaultDataGridViewCheckBoxColumn"].EditedFormattedValue );
                    if ( rowDefault )
                        defaultCount++;

                    // Check for a duplicate description
                    string rowDesc   = rowView.Cells["descriptionDataGridViewTextBoxColumn"].EditedFormattedValue.ToString();
                    string rowWeight = rowView.Cells["pprpaperweightidDataGridViewTextBoxColumn"].EditedFormattedValue.ToString();
                    string rowGrade  = rowView.Cells["pprpapergradeidDataGridViewTextBoxColumn"].EditedFormattedValue.ToString();

                    if ( ( rowDesc == desc ) && ( rowWeight == weight ) && ( rowGrade == grade ) )
                        uniqueCount++;
                }
            }

            // If another row has a default, and they're trying to check this one, then error
            if ( defaultCount > 1 )
            {
                _gridPaper.Rows[e.RowIndex].ErrorText = Resources.MultipleDefaultsError;
                e.Cancel = true;
            }

            if ( uniqueCount > 1 )
            {
                _gridPaper.Rows[e.RowIndex].ErrorText = Resources.DuplicatePaperMapError;
                e.Cancel = true;
            }
        }

        private void _gridPaper_RowValidated( object sender, DataGridViewCellEventArgs e )
        {
            _gridPaper.Rows[e.RowIndex].ErrorText = string.Empty;
        }

        private int GetRowCount()
        {
            // If there are any rate rows, then there must be a default
            int rowCount = 0;
            foreach ( DataGridViewRow rowView in _gridPaper.Rows )
            {
                if ( !rowView.IsNewRow )
                    rowCount++;
            }
            return rowCount;
        }

        private void vndPaper_Validating( object sender, CancelEventArgs e )
        {
            if ( _currentRow == null )
                return;

            ValidateEffectiveDate( _dtEffectiveDate, e );

            if ( GetRowCount() > 1 )
            {
                bool defaultExists = false;
                foreach ( DataGridViewRow rowView in _gridPaper.Rows )
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

        private void vndPaper_Validated( object sender, EventArgs e )
        {
            _lblValidationError.Visible = false;
        }

        private void vndPaper_Load( object sender, EventArgs e )
        {
            if ( MainForm.AuthorizedUser.Right != UserRights.Admin &&
                 MainForm.AuthorizedUser.Right != UserRights.SuperAdmin )
            {
                Enabled = false;
            }
        }

        private void _dtEffectiveDate_ValueChanged( object sender, EventArgs e )
        {
            OnControlDirty( e );
        }

        private void _gridPaper_CellValueChanged( object sender, DataGridViewCellEventArgs e )
        {
            OnControlDirty( EventArgs.Empty );
        }

        private void _gridPaper_UserDeletingRow( object sender, DataGridViewRowCancelEventArgs e )
        {
            DataRowView dr_row = (DataRowView)e.Row.DataBoundItem;
            Datasets.Administration.ppr_paper_mapRow pm_row = (Datasets.Administration.ppr_paper_mapRow)dr_row.Row;
            pm_row.modifiedby = MainForm.AuthorizedUser.FormattedName;
            OnControlDirty( EventArgs.Empty );
        }

        private void _gridPaper_DataError( object sender, DataGridViewDataErrorEventArgs e )
        {
            if ( _gridPaper.Rows[e.RowIndex].Cells["vndpaperidDataGridViewTextBoxColumn"].EditedFormattedValue.ToString().Length > 0 )
            {
                long validatingPaperId = Convert.ToInt64( _gridPaper.Rows[e.RowIndex].Cells["vndpaperidDataGridViewTextBoxColumn"].EditedFormattedValue );
                if ( validatingPaperId == _paperId )
                {
                    _gridPaper.Rows[e.RowIndex].ErrorText = Resources.RequiredFieldError;
                    e.Cancel = true;
                }
            }
        }

        private void _gridPaper_EnabledChanged( object sender, EventArgs e )
        {
            if ( MainForm.AuthorizedUser.Right == UserRights.Admin ||
                 MainForm.AuthorizedUser.Right == UserRights.SuperAdmin )
            {
                if ( Enabled )
                {
                    _gridPaper.ReadOnly = false;
                    _gridPaper.AllowUserToAddRows = true;
                    _gridPaper.AllowUserToDeleteRows = true;
                    _gridPaper.DefaultCellStyle.BackColor = SystemColors.Window;
                }
                else
                {
                    _gridPaper.ReadOnly = true;
                    _gridPaper.AllowUserToAddRows = false;
                    _gridPaper.AllowUserToDeleteRows = false;
                    _gridPaper.DefaultCellStyle.BackColor = SystemColors.Control;
                }
            }
            else
            {
                _gridPaper.ReadOnly = true;
                _gridPaper.AllowUserToAddRows = false;
                _gridPaper.AllowUserToDeleteRows = false;
                _gridPaper.DefaultCellStyle.BackColor = SystemColors.Control;
            }
        }

        #endregion
    }
}