using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.PublicationsTableAdapters;

namespace CatalogEstimating.UserControls.Admin
{
    public partial class uctAdminInsertions : CatalogEstimating.UserControlTab
    {
        private Publications _dsPublications;

        public uctAdminInsertions()
        {
            InitializeComponent();
            Name = "Insertions";

            _dsPublications = new Publications();
            _dsPublications.CaseSensitive = false;

            ChildControls.Add( new ucpAdminInsertionRates(_dsPublications) );
            ChildControls.Add( new uctAdminInsertionGroups(_dsPublications));
            ChildControls.Add( new ucpAdminInsertionScenarios(_dsPublications) );
        }

        public override void LoadData()
        {
            this.IsLoading = true;
            this.SetIsLoadingOnChildControls(true);

            _dsPublications.Clear();

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                using (PubTableAdapter adapter = new PubTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPublications.Pub);
                }

                using (PubLocTableAdapter adapter = new PubLocTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPublications.PubLoc);
                }

                using (pub_pubrate_mapTableAdapter adapter = new pub_pubrate_mapTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPublications.pub_pubrate_map);
                }

                using (pub_pubrate_map_activateTableAdapter adapter = new pub_pubrate_map_activateTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPublications.pub_pubrate_map_activate);
                }

                using (pub_pubrateTableAdapter adapter = new pub_pubrateTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPublications.pub_pubrate);
                }

                using (pub_pubquantityTableAdapter adapter = new pub_pubquantityTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPublications.pub_pubquantity);
                }

                using (pub_dayofweekquantityTableAdapter adapter = new pub_dayofweekquantityTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPublications.pub_dayofweekquantity);
                }

                using (pub_dayofweekratetypesTableAdapter adapter = new pub_dayofweekratetypesTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPublications.pub_dayofweekratetypes);
                }

                using (pub_dayofweekratesTableAdapter adapter = new pub_dayofweekratesTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPublications.pub_dayofweekrates);
                }

                using (pub_insertscenarioTableAdapter adapter = new pub_insertscenarioTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPublications.pub_insertscenario);
                }

                using (pub_groupinsertscenario_mapTableAdapter adapter = new pub_groupinsertscenario_mapTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPublications.pub_groupinsertscenario_map);
                }

                using (pub_insertdiscountsTableAdapter adapter = new pub_insertdiscountsTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPublications.pub_insertdiscounts);
                }

                using (pub_pubgroupTableAdapter adapter = new pub_pubgroupTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPublications.pub_pubgroup);
                }

                using (pub_pubpubgroup_mapTableAdapter adapter = new pub_pubpubgroup_mapTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPublications.pub_pubpubgroup_map);
                }

                using (pub_pubquantitytypeTableAdapter adapter = new pub_pubquantitytypeTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPublications.pub_pubquantitytype);
                }

                using (pub_ratetypeTableAdapter adapter = new pub_ratetypeTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPublications.pub_ratetype);
                }

                conn.Close();
            }

            base.LoadData();

            SetDirtyFalseOnChildControls();
            this.IsLoading = false;
            SetIsLoadingOnChildControls(false);
        }

        public override void SaveData()
        {
            // Gather the child control dirty flags
            bool[] bChildDirtyFlags = new bool[4];
            //ucpInsertionRates
            bChildDirtyFlags[0] = ChildControls[0].Dirty;
            //ucpAdminInsertionGroupSetup
            bChildDirtyFlags[1] = ((uctAdminInsertionGroups)ChildControls[1]).Dirty_AdminInsertionGroupSetup;
            //ucpAdminInsertionGroupOrder
            bChildDirtyFlags[2] = ((uctAdminInsertionGroups)ChildControls[1]).Dirty_AdminInsertionGroupOrder;
            //ucpAdminInsertionScenarios
            bChildDirtyFlags[3] = ChildControls[2].Dirty;

            this.IsLoading = true;
            SetIsLoadingOnChildControls(true);

            base.SaveData();

            bool bKnownSqlErrorOccurred = false;
            Exception SaveException = null;
            string SqlErrorMessage = string.Empty;

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();
                //conn.ConnectionTimeout = 900;

                #region Define Adapters
                pub_pubrate_mapTableAdapter adapter_pubratemap = new pub_pubrate_mapTableAdapter();
                pub_pubrate_map_activateTableAdapter adapter_pubratemap_activate = new pub_pubrate_map_activateTableAdapter();
                pub_insertdiscountsTableAdapter adapter_insertdiscounts = new pub_insertdiscountsTableAdapter();
                pub_dayofweekratesTableAdapter adapter_rate = new pub_dayofweekratesTableAdapter();
                pub_dayofweekratetypesTableAdapter adapter_ratetype = new pub_dayofweekratetypesTableAdapter();
                pub_pubrateTableAdapter adapter_pubrate = new pub_pubrateTableAdapter();
                pub_dayofweekquantityTableAdapter adapter_dow_qty = new pub_dayofweekquantityTableAdapter();
                pub_pubquantityTableAdapter adapter_qty = new pub_pubquantityTableAdapter();
                pub_pubgroupTableAdapter adapter_pubgroup = new pub_pubgroupTableAdapter();
                pub_pubpubgroup_mapTableAdapter adapter_pubpubgroup_map = new pub_pubpubgroup_mapTableAdapter();
                pub_insertscenarioTableAdapter adapter_insertscenario = new pub_insertscenarioTableAdapter();
                pub_groupinsertscenario_mapTableAdapter adapter_groupinsertscenario_map = new pub_groupinsertscenario_mapTableAdapter();

                #endregion

                #region Set SQL Connection on Adapters
                adapter_pubratemap.Connection = conn;
                adapter_pubratemap_activate.Connection = conn;
                adapter_insertdiscounts.Connection = conn;
                adapter_ratetype.Connection = conn;
                adapter_rate.Connection = conn;
                adapter_pubrate.Connection = conn;
                adapter_dow_qty.Connection = conn;
                adapter_qty.Connection = conn;
                adapter_pubgroup.Connection = conn;
                adapter_pubgroup.CommandTimeout = 1200;
                adapter_pubpubgroup_map.Connection = conn;
                adapter_pubpubgroup_map.CommandTimeout = 1200;
                adapter_insertscenario.Connection = conn;
                adapter_groupinsertscenario_map.Connection = conn;
                #endregion

                try
                {
                    adapter_pubratemap.Update(_dsPublications.pub_pubrate_map);
                    adapter_pubratemap_activate.Update(_dsPublications.pub_pubrate_map_activate);
                }
                catch (Exception e)
                {
                    SaveException = e;
                }
                finally
                {
                    adapter_pubratemap.Dispose();
                    adapter_pubratemap_activate.Dispose();
                }

                if (SaveException == null)
                {
                    try
                    {

                        adapter_insertdiscounts.Update(_dsPublications.pub_insertdiscounts.Select("", "", DataViewRowState.Deleted));
                        adapter_rate.Update(_dsPublications.pub_dayofweekrates.Select("", "", DataViewRowState.Deleted));
                        adapter_ratetype.Update(_dsPublications.pub_dayofweekratetypes.Select("", "", DataViewRowState.Deleted));
                        adapter_pubrate.Update(_dsPublications.pub_pubrate.Select("", "", DataViewRowState.Deleted));

                        adapter_pubrate.Update(_dsPublications.pub_pubrate);
                        adapter_insertdiscounts.Update(_dsPublications.pub_insertdiscounts);
                        adapter_ratetype.Update(_dsPublications.pub_dayofweekratetypes);
                        adapter_rate.Update(_dsPublications.pub_dayofweekrates);
                    }

                    catch (SqlException se)
                    {
                        if (se.Number == 50000)
                        {
                            bKnownSqlErrorOccurred = true;
                            SqlErrorMessage = se.Message;
                        }

                        SaveException = se;
                    }
                    catch (Exception e)
                    {
                        SaveException = e;
                    }
                    finally
                    {
                        adapter_insertdiscounts.Dispose();
                        adapter_rate.Dispose();
                        adapter_ratetype.Dispose();
                        adapter_pubrate.Dispose();
                    }
                }

                if (SaveException == null)
                {
                    try
                    {
                        adapter_dow_qty.Update(_dsPublications.pub_dayofweekquantity.Select("", "", DataViewRowState.Deleted));
                        adapter_qty.Update(_dsPublications.pub_pubquantity.Select("", "", DataViewRowState.Deleted));

                        adapter_qty.Update(_dsPublications.pub_pubquantity);
                        adapter_dow_qty.Update(_dsPublications.pub_dayofweekquantity);
                    }
                    catch (SqlException se)
                    {
                        if (se.Number == 50000)
                        {
                            bKnownSqlErrorOccurred = true;
                            SqlErrorMessage = se.Message;
                        }

                        SaveException = se;
                    }
                    catch (Exception e)
                    {
                        SaveException = e;
                    }
                    finally
                    {
                        adapter_dow_qty.Dispose();
                        adapter_qty.Dispose();
                    }
                }

                if (SaveException == null)
                {
                    SqlTransaction tran_pubgroup = null;

                    try
                    {
                        tran_pubgroup = conn.BeginTransaction();

                        adapter_pubgroup.SetTransaction(tran_pubgroup);
                        adapter_pubpubgroup_map.SetTransaction(tran_pubgroup);
                        adapter_pubgroup.Update(_dsPublications.pub_pubgroup);
                        adapter_pubpubgroup_map.Update(_dsPublications.pub_pubpubgroup_map);
                        tran_pubgroup.Commit();
                    }
                    catch (SqlException se)
                    {
                        tran_pubgroup.Rollback();

                        if (se.Number == 50000)
                        {
                            bKnownSqlErrorOccurred = true;
                            SqlErrorMessage = se.Message;
                        }

                        SaveException = se;
                    }
                    catch (Exception e)
                    {
                        tran_pubgroup.Rollback();
                        SaveException = e;
                    }
                    finally
                    {
                        adapter_pubgroup.Dispose();
                        adapter_pubpubgroup_map.Dispose();
                        tran_pubgroup.Dispose();
                    }
                }

                if (SaveException == null)
                {
                    try
                    {

                        adapter_groupinsertscenario_map.Update(_dsPublications.pub_groupinsertscenario_map.Select("", "", DataViewRowState.Deleted));
                        adapter_insertscenario.Update(_dsPublications.pub_insertscenario.Select("", "", DataViewRowState.Deleted));

                        adapter_insertscenario.Update(_dsPublications.pub_insertscenario);
                        adapter_groupinsertscenario_map.Update(_dsPublications.pub_groupinsertscenario_map);
                    }
                    catch (SqlException se)
                    {
                        if (se.Number == 50000)
                        {
                            bKnownSqlErrorOccurred = true;
                            SqlErrorMessage = se.Message;
                        }

                        SaveException = se;
                    }
                    catch (Exception e)
                    {
                        SaveException = e;
                    }
                    finally
                    {
                        adapter_insertscenario.Dispose();
                        adapter_groupinsertscenario_map.Dispose();
                    }
                }

                conn.Close();

                if (bKnownSqlErrorOccurred)
                {
                    this.ChildControls[0].Dirty = bChildDirtyFlags[0];
                    ((uctAdminInsertionGroups)this.ChildControls[1]).Dirty_AdminInsertionGroupSetup = bChildDirtyFlags[1];
                    ((uctAdminInsertionGroups)this.ChildControls[1]).Dirty_AdminInsertionGroupOrder = bChildDirtyFlags[2];
                    this.ChildControls[2].Dirty = bChildDirtyFlags[3];
                    
                    MessageBox.Show(SqlErrorMessage, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (SaveException != null)
                {
                    this.ChildControls[0].Dirty = bChildDirtyFlags[0];
                    ((uctAdminInsertionGroups)this.ChildControls[1]).Dirty_AdminInsertionGroupSetup = bChildDirtyFlags[1];
                    ((uctAdminInsertionGroups)this.ChildControls[1]).Dirty_AdminInsertionGroupOrder = bChildDirtyFlags[2];
                    this.ChildControls[2].Dirty = bChildDirtyFlags[3];

                    ExceptionForm formException = new ExceptionForm(SaveException);
                    formException.ShowDialog();
                }
                else
                {
                    this.SetDirtyFalseOnChildControls();
                    this.SetIsLoadingOnChildControls(false);
                    this.LoadData();
                }

                this.IsLoading = false;
            }
        }

        protected override void OnDirtyChanged(object sender, EventArgs e)
        {
            base.OnDirtyChanged(sender, e);
            _btnCancel.Enabled = this.Dirty;
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult answer = MessageBox.Show(this, Properties.Resources.CancelChangesWarning, "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (answer == DialogResult.Yes)
            {
                this.IsLoading = true;
                this.LoadData();
                this.IsLoading = false;
                SetDirtyFalseOnChildControls();
            }
        }

        public override ToolStrip Toolbar
        {
            get
            {
                return toolStrip1;
            }
        }
    }
}