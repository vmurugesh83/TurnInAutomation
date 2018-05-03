#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.AdministrationTableAdapters;

using Microsoft.Practices.EnterpriseLibrary.Data;

#endregion

namespace CatalogEstimating
{
    public partial class DatabaseForm : Form
    {
        #region PInvoke Structures and Methods

        public delegate int EnumWindowsProc( IntPtr hwnd, int lParam );

        [DllImport( "user32.dll" )]
        public static extern int EnumWindows( EnumWindowsProc callback, int lParam );

        [DllImport( "user32", CharSet = CharSet.Auto, SetLastError = false )]
        public extern static IntPtr GetProp( IntPtr hwnd, string lpString );

        #endregion

        #region Construction

        public DatabaseForm()
        {
            InitializeComponent();

            _databases = DatabaseList.GetDatabases();
            foreach ( CESDatabase database in _databases )
            {
                if ( database.Display && database.Type != DatabaseType.Admin )
                    _cboDatabases.Items.Add( database );
            }
            _cboDatabases.SelectedIndex = 0;
        }

        #endregion

        #region Public Properties

        private List<CESDatabase> _databases = null;
        public List<CESDatabase> Databases
        {
            get { return _databases; }
        }

        #endregion

        #region Event Handlers

        private void _btnOK_Click( object sender, EventArgs e )
        {
            if ( _cboDatabases.SelectedItem != null )
            {
                CESDatabase selectedDb = (CESDatabase)_cboDatabases.SelectedItem;

                // Verify that no other instances of this application are running using this database
                if ( InUse( selectedDb ) )
                    MessageBox.Show( "This database is in use by another instance of the application" );
                else
                {
                    bool success = false;
                    DbConnection conn = null;
                    try
                    {
                        conn = selectedDb.Database.CreateConnection();
                        conn.Open();
                        success = true;
                    }
                    catch
                    {
                        MessageBox.Show( "Failed to connect to selected database" );
                        // TODO: NLS - Log this exception
                    }
                    finally
                    {
                        if ( conn != null )
                        {
                            conn.Close();
                            conn.Dispose();
                        }
                    }

                    if ( success )
                    {
                        selectedDb.IsWorking = true;
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private static bool _inUse = false;
        private bool InUse( CESDatabase selectedDb )
        {
            _inUse = false;

            // Enumerate windows
            EnumWindowsProc enumWindowsProc = new EnumWindowsProc( WindowEnumProc );
            EnumWindows( enumWindowsProc, selectedDb.Id );

            return _inUse;
        }

        private static int WindowEnumProc( IntPtr hWnd, int lParam )
        {
            IntPtr propPtr = GetProp( hWnd, "WorkingDatabase" );
            if ( propPtr  != IntPtr.Zero )
            {
                if ( propPtr.ToInt32() == lParam )
                {
                    _inUse = true;
                    return 0;
                }
            }

            return 1;
        }

        #endregion
    }
}