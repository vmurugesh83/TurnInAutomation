#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

using CatalogEstimating.Properties;
using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.PostalTableAdapters;

#endregion

namespace CatalogEstimating.UserControls.Admin
{
    public partial class uctAdminPostal : CatalogEstimating.UserControlTab
    {
        #region Private Variables

        private Postal _dsPostal = new Postal();

        #endregion

        #region Construction

        public uctAdminPostal()
        {
            InitializeComponent();
            
            Name = "Postal";
            ChildControls.Add(new ucpPostalSetup(_dsPostal));
            ChildControls.Add(new ucpPostalScenarioSetup(_dsPostal));
            ChildControls.Add(new ucpPostalCategorySetup(_dsPostal));
        }

        #endregion

        #region Override Methods
        public override ToolStrip Toolbar
        {
            get
            {
                return toolStrip1;
            }
        }

        public override void LoadData()
        {
            _dsPostal.Clear();

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                using (pst_postalclassTableAdapter adapter = new pst_postalclassTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPostal.pst_postalclass);
                }

                using (pst_postalcategoryTableAdapter adapter = new pst_postalcategoryTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPostal.pst_postalcategory);
                }

                using (vnd_vendorTableAdapter adapter = new vnd_vendorTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPostal.vnd_vendor);
                    _dsPostal.vnd_vendor.DefaultView.Sort = "description";
                }

                using (pst_postalweightsTableAdapter adapter = new pst_postalweightsTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPostal.pst_postalweights);
                }

                using (pst_postalmailertypeTableAdapter adapter = new pst_postalmailertypeTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPostal.pst_postalmailertype);
                }

                using (pst_postalcategoryrate_mapTableAdapter adapter = new pst_postalcategoryrate_mapTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPostal.pst_postalcategoryrate_map);
                }

                using (pst_postalscenarioTableAdapter adapter = new pst_postalscenarioTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPostal.pst_postalscenario);
                }

                using (pst_postalcategoryscenario_mapTableAdapter adapter = new pst_postalcategoryscenario_mapTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsPostal.pst_postalcategoryscenario_map);
                }

                conn.Close();
            }

            base.LoadData();

            SetDirtyFalseOnChildControls();
        }

        public override void SaveData()
        {
            // Postal Scenarios cannot be deleted if they are being referenced by estimates.
            // These references are checked in the SQL logic and if an error is returned we need to inform the user.
            bool SaveError = false;

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                pst_postalcategoryTableAdapter cat_adapter = new pst_postalcategoryTableAdapter();
                pst_postalweightsTableAdapter pw_adapter = new pst_postalweightsTableAdapter();
                pst_postalcategoryrate_mapTableAdapter pcr_adapter = new pst_postalcategoryrate_mapTableAdapter();

                try
                {
                    cat_adapter.Connection = conn;
                    pw_adapter.Connection = conn;
                    pcr_adapter.Connection = conn;

                    pcr_adapter.Update(_dsPostal.pst_postalcategoryrate_map.Select("", "", DataViewRowState.Deleted));
                    pw_adapter.Update(_dsPostal.pst_postalweights.Select("", "", DataViewRowState.Deleted));
                    cat_adapter.Update(_dsPostal.pst_postalcategory.Select("", "", DataViewRowState.Deleted));

                    cat_adapter.Update(_dsPostal.pst_postalcategory);
                    pw_adapter.Update(_dsPostal.pst_postalweights);
                    pcr_adapter.Update(_dsPostal.pst_postalcategoryrate_map);
                }
                catch (SqlException se)
                {
                    if (se.Number == 50000)
                    {
                        SaveError = true;
                        MessageBox.Show(se.Message, "Cannot Save Postal Changes");
                    }
                    else
                        throw;
                }
                finally
                {
                    cat_adapter.Dispose();
                    pw_adapter.Dispose();
                    pcr_adapter.Dispose();
                }

                if (!SaveError)
                {
                    pst_postalscenarioTableAdapter scenario_adapter = new pst_postalscenarioTableAdapter();
                    pst_postalcategoryscenario_mapTableAdapter pcs_adapter = new pst_postalcategoryscenario_mapTableAdapter();

                    try
                    {
                        scenario_adapter.Connection = conn;
                        pcs_adapter.Connection = conn;

                        pcs_adapter.Update(_dsPostal.pst_postalcategoryscenario_map.Select("", "", DataViewRowState.Deleted));
                        scenario_adapter.Update(_dsPostal.pst_postalscenario.Select("", "", DataViewRowState.Deleted));

                        scenario_adapter.Update(_dsPostal.pst_postalscenario);
                        pcs_adapter.Update(_dsPostal.pst_postalcategoryscenario_map);
                    }
                    catch (SqlException se)
                    {
                        if (se.Number == 50000)
                        {
                            SaveError = true;
                            MessageBox.Show(se.Message, "Cannot Save Postal Scenario");
                        }
                        else
                            throw;
                    }
                    finally
                    {
                        scenario_adapter.Dispose();
                        pcs_adapter.Dispose();
                    }

                    conn.Close();
                }
            }

            if (!SaveError)
            {
                this.LoadData();
                SetDirtyFalseOnChildControls();

                foreach (UserControlPanel p in ChildControls)
                {
                    p.Reload();
                }
                base.SaveData();
            }
        }

        protected override void OnDirtyChanged(object sender, EventArgs e)
        {
            base.OnDirtyChanged(sender, e);
            _btnCancel.Enabled = this.Dirty;
        }
        #endregion

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult answer = MessageBox.Show(this, Resources.CancelChangesWarning, "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (answer == DialogResult.Yes)
            {
                this.LoadData();
                SetDirtyFalseOnChildControls();

                foreach (UserControlPanel p in ChildControls)
                {
                    p.Reload();
                }
            }
        }
    }
}

