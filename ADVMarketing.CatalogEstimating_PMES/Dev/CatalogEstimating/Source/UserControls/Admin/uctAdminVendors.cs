#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.AdministrationTableAdapters;

#endregion

namespace CatalogEstimating.UserControls.Admin
{
    public partial class uctAdminVendors : CatalogEstimating.UserControlTab
    {
        #region Private Variables

        private Administration _dsAdministration;

        #endregion

        #region Construction

        public uctAdminVendors()
        {
            InitializeComponent();
            Name = "Vendors";
        }

        public uctAdminVendors( Administration ds )
        : this()
        {
            _dsAdministration = ds;

            ChildControls.Add( new ucpAdminVendorsSetup( ds ) );
            ChildControls.Add( new ucpAdminVendorsRates( ds ) );
        }

        #endregion

        #region Override Methods

        public override void LoadData()
        {
            using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
            {
                using ( vnd_vendorTableAdapter adapter = new vnd_vendorTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsAdministration.vnd_vendor );
                }

                using ( vnd_vendortypeTableAdapter adapter = new vnd_vendortypeTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsAdministration.vnd_vendortype );
                }

                using ( vnd_vendorvendortype_mapTableAdapter adapter = new vnd_vendorvendortype_mapTableAdapter() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsAdministration.vnd_vendorvendortype_map );
                }

                conn.Close();
            }

            base.LoadData();
        }

        public override void SaveData()
        {
            // Save the top level vendor data
            using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    using ( vnd_vendorTableAdapter adapter = new vnd_vendorTableAdapter() )
                    {
                        adapter.Connection = conn;
                        adapter.SetTransaction( tran );
                        adapter.Update( _dsAdministration.vnd_vendor );
                    }

                    using ( vnd_vendorvendortype_mapTableAdapter adapter = new vnd_vendorvendortype_mapTableAdapter() )
                    {
                        adapter.Connection = conn;
                        adapter.SetTransaction( tran );
                        adapter.Update( _dsAdministration.vnd_vendorvendortype_map );
                    }
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    tran.Dispose();
                }

                conn.Close();
            }

            // Have each of the children call their save if they have one
            base.SaveData();
        }

        #endregion
    }
}