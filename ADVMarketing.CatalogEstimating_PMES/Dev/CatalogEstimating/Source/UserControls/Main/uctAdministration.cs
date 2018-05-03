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

namespace CatalogEstimating.UserControls.Admin
{
    public partial class uctAdministration : CatalogEstimating.UserControlTab
    {
        #region Private Variables

        private Administration _dsAdministration = new Administration();
        private Publications   _dsPublications   = new Publications();

        #endregion

        #region Construction

        public uctAdministration()
        {
            InitializeComponent();
            Name = "Administration";

            ChildControls.Add( new uctAdminInsertions() );
            ChildControls.Add( new uctAdminVendors( _dsAdministration ) );
            ChildControls.Add( new uctAdminPostal() );
            ChildControls.Add( new ucpAdminDatabases( _dsAdministration ) );
        }

        #endregion

        #region Overrides

        public override ToolStrip Toolbar
        {
            get { return _toolStrip; }
        }

        #endregion

        #region Event Handlers

        private void uctAdministration_Load( object sender, EventArgs e )
        {
            if ( DesignMode )
                return;

            if ( MainForm.AuthorizedUser.Right != UserRights.SuperAdmin &&
                 MainForm.AuthorizedUser.Right != UserRights.Admin )
            {
                _btnSave.Enabled = false;
            }
        }

        private void _btnSave_Click( object sender, EventArgs e )
        {
            CancelEventArgs ctrlEvent = new CancelEventArgs();
            PreSave( ctrlEvent );

            if ( !ctrlEvent.Cancel )
            {
                SaveData();
                MainForm main = (MainForm)ParentForm;
                main.LastAction = string.Format( "Last Saved: {0}", DateTime.Now );
            }
        }

        private void _btnPrint_Click( object sender, EventArgs e )
        {
            ExcelWriter writer = null;
            try
            {
                writer = new ExcelWriter();
                SelectedControl.Export(ref writer);
                writer.Show();
            }
            finally
            {
                writer.Dispose();
            }
        }

        #endregion
    }
}
