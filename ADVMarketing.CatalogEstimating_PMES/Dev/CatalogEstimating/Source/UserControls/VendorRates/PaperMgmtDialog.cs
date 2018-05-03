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
    public partial class PaperMgmtDialog : Form
    {
        #region Private Variables

        private Administration.ppr_papergradeDataTable  _workingGrades;
        private Administration.ppr_paperweightDataTable _workingWeights;

        #endregion

        #region Construction

        public PaperMgmtDialog()
        {
            InitializeComponent();
        }

        public PaperMgmtDialog( Administration ds )
        : this()
        {
            _dsAdministration = ds;
            _workingGrades    = (Administration.ppr_papergradeDataTable)ds.ppr_papergrade.Copy();
            _workingWeights   = (Administration.ppr_paperweightDataTable)ds.ppr_paperweight.Copy();

            pprpapergradeBindingSource.DataSource  = _workingGrades;
            pprpaperweightBindingSource.DataSource = _workingWeights;
        }

        #endregion

        #region Event Handlers

        private void _btnAddWeight_Click( object sender, EventArgs e )
        {
            bool found = false;
            foreach ( Administration.ppr_paperweightRow checkRow in _workingWeights.Rows )
            {
                if ( checkRow.weight.ToString() == _txtWeight.Value.ToString() )
                {
                    found = true;
                    break;
                }
            }

            if ( !found )
            {
                Administration.ppr_paperweightRow newRow = _workingWeights.Newppr_paperweightRow();
                newRow.createdby = MainForm.AuthorizedUser.FormattedName;
                newRow.createddate = DateTime.Now;
                newRow.weight = _txtWeight.Value.Value;
                _workingWeights.Addppr_paperweightRow( newRow );

                _txtWeight.Text = string.Empty;
                _txtWeight.Focus();

                _Errors.SetError( _txtWeight, string.Empty );
            }
            else
            {
                _Errors.SetError( _txtWeight, Resources.DuplicateItemError );
            }
        }

        private void _btnAddGrade_Click( object sender, EventArgs e )
        {
            bool found = false;
            foreach ( Administration.ppr_papergradeRow checkRow in _workingGrades.Rows )
            {
                if ( string.Compare( checkRow.grade, _txtGrade.Text, true ) == 0 )
                {
                    found = true;
                    break;
                }
            }

            if ( !found )
            {
                Administration.ppr_papergradeRow newRow = _workingGrades.Newppr_papergradeRow();
                newRow.createdby = MainForm.AuthorizedUser.FormattedName;
                newRow.createddate = DateTime.Now;
                newRow.grade = this._txtGrade.Text;
                _workingGrades.Addppr_papergradeRow( newRow );

                _txtGrade.Text = string.Empty;
                _txtGrade.Focus();
            }
            else
            {
                _Errors.SetError( _txtGrade, Resources.DuplicateItemError );
            }
        }

        private void _btnSave_Click( object sender, EventArgs e )
        {
            _dsAdministration.ppr_papergrade.Merge( _workingGrades );
            _dsAdministration.ppr_paperweight.Merge( _workingWeights );
        }

        private void _txtWeight_TextChanged( object sender, EventArgs e )
        {
            _btnAddWeight.Enabled = _txtWeight.Value != null;
        }

        private void _txtGrade_TextChanged( object sender, EventArgs e )
        {
            _btnAddGrade.Enabled = !string.IsNullOrEmpty( _txtGrade.Text );
        }

        #endregion
    }
}