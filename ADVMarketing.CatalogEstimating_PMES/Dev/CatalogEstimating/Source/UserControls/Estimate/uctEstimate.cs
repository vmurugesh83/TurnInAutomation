using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.EstimatesTableAdapters;
using CatalogEstimating.Datasets.DistributionMappingTableAdapters;
using CatalogEstimating.Properties;

namespace CatalogEstimating.UserControls.Estimate
{
    public partial class uctEstimate : CatalogEstimating.UserControlTab
    {
        private long? _EstimateID;
        private long? _ParentEstimateID;
        private bool _readOnly = true;
        private bool _preventFillDataset = true;

        internal ucpEstimateSetup _ucpEstimateSetup = null;
        internal ucpComponents _ucpComponents = null;
        internal uctDistributionMapping _uctDistributionMapping = null;
        internal ucpAssemblyDistributionOptions _ucpAssemblyDistributionOptions = null;

        private Datasets.Estimates _dsEstimate = new Datasets.Estimates();
        private Datasets.DistributionMapping _dsDistMapping = new Datasets.DistributionMapping();

        public uctEstimate()
        {
            InitializeComponent();
        }

        public void Initialize(long? EstimateID, long? ParentEstimateID, bool forceReadOnly)
        {
            _EstimateID       = EstimateID;
            _ParentEstimateID = ParentEstimateID;

            this.FillDataset();

            if (forceReadOnly || _dsEstimate.est_estimate[0].est_status_id != 1)
            {
                _readOnly = true;

                _btnSave.Enabled = false;
                _btnCancel.Enabled = false;
                _btnAddToPolybag.Enabled = false;
                _btnKill.Enabled = false;
                _btnUpload.Enabled = false;
            }
            else
            {
                _readOnly = false;
            }

            if ( MainForm.WorkingDatabase.Type != DatabaseType.Live )
                _btnUpload.Enabled = false;

            _ucpEstimateSetup = new ucpEstimateSetup(_dsEstimate, _readOnly);
            _ucpComponents = new ucpComponents(_dsEstimate, _readOnly);
            _uctDistributionMapping = new uctDistributionMapping(_dsEstimate, _dsDistMapping, _readOnly);
            _ucpAssemblyDistributionOptions = new ucpAssemblyDistributionOptions(_dsEstimate, _readOnly);

            ChildControls.Add(_ucpEstimateSetup);
            ChildControls.Add(_ucpComponents);
            ChildControls.Add(_uctDistributionMapping);
            ChildControls.Add(new ucpSamples(_dsEstimate, _readOnly));
            ChildControls.Add(_ucpAssemblyDistributionOptions);
            ChildControls.Add(new ucpCostSummary(_dsEstimate));
        }

        public override void LoadData()
        {
            // The first time LoadData is run the dataset has already been filled and we don't need to fill it again.
            if (_preventFillDataset)
                _preventFillDataset = false;
            else
                FillDataset();

            base.LoadData();
        }

        private void FillDataset()
        {
            _dsEstimate.Clear();
            _dsDistMapping.Clear();

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                using (est_estimatemediatypeTableAdapter adapter = new est_estimatemediatypeTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsEstimate.est_estimatemediatype);
                }

                using (est_componenttypeTableAdapter adapter = new est_componenttypeTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsEstimate.est_componenttype);
                }

                using (vnd_vendorTableAdapter adapter = new vnd_vendorTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsEstimate.vnd_vendor);
                }

                using (vnd_vendortypeTableAdapter adapter = new vnd_vendortypeTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsEstimate.vnd_vendortype);
                }

                using (vnd_vendorvendortype_mapTableAdapter adapter = new vnd_vendorvendortype_mapTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsEstimate.vnd_vendorvendortype_map);
                }

                using (ppr_papergradeTableAdapter adapter = new ppr_papergradeTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsEstimate.ppr_papergrade);
                    _dsEstimate.ppr_papergrade.DefaultView.Sort = "grade";
                }

                using (ppr_paperweightTableAdapter adapter = new ppr_paperweightTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsEstimate.ppr_paperweight);
                    _dsEstimate.ppr_paperweight.DefaultView.Sort = "weight";
                }

                if (!_EstimateID.HasValue)
                {
                    Estimates.est_estimateRow new_estimate = _dsEstimate.est_estimate.Newest_estimateRow();
                    if (_ParentEstimateID.HasValue)
                        new_estimate.parent_id = _ParentEstimateID.Value;
                    else
                        new_estimate.Setparent_idNull();

                    new_estimate.description = String.Empty;
                    new_estimate.rundate = DateTime.Today;
                    new_estimate.est_season_id = FiscalCalculator.SeasonID(new_estimate.rundate);
                    new_estimate.fiscalyear = FiscalCalculator.FiscalYear(new_estimate.rundate);
                    new_estimate.fiscalmonth = FiscalCalculator.FiscalMonth(new_estimate.rundate);
                    new_estimate.est_status_id = 1;

                    new_estimate.createdby = MainForm.AuthorizedUser.FormattedName;
                    new_estimate.createddate = DateTime.Now;

                    _dsEstimate.est_estimate.Addest_estimateRow(new_estimate);

                    ParentForm.Text = string.Concat(CatalogEstimating.Properties.Resources.ApplicationEnvironment, " ", CatalogEstimating.Properties.Resources.EstimateFormTitle, " - New");
                }
                else
                {
                    using (est_estimateTableAdapter adapter = new est_estimateTableAdapter())
                    {
                        adapter.Connection = conn;
                        adapter.Fill(_dsEstimate.est_estimate, _EstimateID.Value);
                    }

                    using (est_componentTableAdapter adapter = new est_componentTableAdapter())
                    {
                        adapter.Connection = conn;
                        adapter.Fill(_dsEstimate.est_component, _EstimateID.Value);
                    }

                    using (Datasets.EstimatesTableAdapters.pub_pubgroupTableAdapter adapter = new CatalogEstimating.Datasets.EstimatesTableAdapters.pub_pubgroupTableAdapter())
                    {
                        adapter.Connection = conn;
                        adapter.Fill(_dsEstimate.pub_pubgroup, _EstimateID.Value);
                    }

                    using (Datasets.EstimatesTableAdapters.pub_pubpubgroup_mapTableAdapter adapter = new CatalogEstimating.Datasets.EstimatesTableAdapters.pub_pubpubgroup_mapTableAdapter())
                    {
                        adapter.Connection = conn;
                        adapter.Fill(_dsEstimate.pub_pubpubgroup_map, _EstimateID.Value);
                    }

                    using (est_packageTableAdapter adapter = new est_packageTableAdapter())
                    {
                        adapter.Connection = conn;
                        adapter.Fill(_dsEstimate.est_package, _EstimateID.Value);
                    }

                    using (est_packagecomponentmappingTableAdapter adapter = new est_packagecomponentmappingTableAdapter())
                    {
                        adapter.Connection = conn;
                        adapter.Fill(_dsEstimate.est_packagecomponentmapping, _EstimateID.Value);
                    }

                    using (est_pubissuedatesTableAdapter adapter = new est_pubissuedatesTableAdapter())
                    {
                        adapter.Connection = conn;
                        adapter.Fill(_dsEstimate.est_pubissuedates, _EstimateID.Value);
                    }

                    using (EstPackage_Quantities_ByEstimateIDTableAdapter adapter = new EstPackage_Quantities_ByEstimateIDTableAdapter())
                    {
                        adapter.Connection = conn;
                        adapter.Fill(_dsEstimate.EstPackage_Quantities_ByEstimateID, _EstimateID.Value);
                    }

                    ParentForm.Text = string.Concat(CatalogEstimating.Properties.Resources.ApplicationEnvironment, " ", CatalogEstimating.Properties.Resources.EstimateFormTitle, "# ", _dsEstimate.est_estimate[0].est_estimate_id.ToString(), " - ", _dsEstimate.est_estimate[0].description);
                }

                using (est_samplesTableAdapter adapter = new est_samplesTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsEstimate.est_samples, _dsEstimate.est_estimate[0].est_estimate_id);
                }

                #region Samples
                if (_dsEstimate.est_samples.Count == 0)
                {
                    Datasets.Estimates.est_samplesRow new_sample = _dsEstimate.est_samples.Newest_samplesRow();

                    new_sample.est_estimate_id = _dsEstimate.est_estimate[0].est_estimate_id;
                    new_sample.quantity = 0;
                    new_sample.freightcwt = 0;
                    new_sample.freightflat = 0;
                    new_sample.createdby = MainForm.AuthorizedUser.FormattedName;
                    new_sample.createddate = DateTime.Now;

                    _dsEstimate.est_samples.Addest_samplesRow(new_sample);
                }

                using (est_assemdistriboptionsTableAdapter adapter = new est_assemdistriboptionsTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsEstimate.est_assemdistriboptions, _dsEstimate.est_estimate[0].est_estimate_id);
                }
                #endregion

                #region Distribution Mapping Insert Setup
                using (PubInsertScenario_ActiveTableAdapter adapter = new PubInsertScenario_ActiveTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsDistMapping.PubInsertScenario_Active, _dsEstimate.est_estimate[0].rundate);
                }

                using (PubPubGroup_ActiveTableAdapter adapter = new PubPubGroup_ActiveTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsDistMapping.PubPubGroup_Active, _dsEstimate.est_estimate[0].rundate);
                }

                using (Pub_ActiveTableAdapter adapter = new Pub_ActiveTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsDistMapping.Pub_Active, _dsEstimate.est_estimate[0].rundate);
                }

                using (PubLoc_ActiveTableAdapter adapter = new PubLoc_ActiveTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsDistMapping.PubLoc_Active, _dsEstimate.est_estimate[0].rundate);
                }

                using (pub_pubquantitytypeTableAdapter adapter = new pub_pubquantitytypeTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsEstimate.pub_pubquantitytype);
                }

                #endregion

                conn.Close();
            }
        }

        public override void PreSave(CancelEventArgs e)
        {
            base.PreSave(e);

            // Estimate must contain certain data first
            if (!e.Cancel)
            {
                string errorMsg = null;
                //if (_dsEstimate.est_component.Count == 0)
                //    errorMsg = "The estimate must contain a host component.";
                //else if ( _dsEstimate.est_assemdistriboptions.Count == 0 )
                //    errorMsg = "The estimate assembly and distribution options tab must be completed.";
                //else if ( !_dsEstimate.est_assemdistriboptions[0].IsValid() )
                //    errorMsg = "The estimate assembly and distribution options tab has invalid data.";
                if (_dsEstimate.est_assemdistriboptions.Count > 0)
                {
                    if (!_dsEstimate.est_assemdistriboptions[0].IsValid())
                        errorMsg = "The estimate assembly and distribution options tab has invalid data.";
                }

                if ( errorMsg != null )
                {
                    MessageBox.Show( errorMsg, "Cannot Save", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    e.Cancel = true;
                    return;
                }
            }
        }

        public override void OnLeaving(CancelEventArgs e)
        {
            base.OnLeaving(e);

            // Estimate requires a host component
            if (!e.Cancel)
            {
                if (_dsEstimate.est_component.Count == 0)
                {
                    DialogResult result = MessageBox.Show("The estimate must contain a host component.", "Cannot Leave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }
            }
        }

        public override void SaveData()
        {
            this.IsLoading = true;
            SetIsLoadingOnChildControls(true);

            bool SaveError = false;

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                #region Define Adapters
                est_estimateTableAdapter adapter_est = new est_estimateTableAdapter();
                est_samplesTableAdapter adapter_samples = new est_samplesTableAdapter();
                est_assemdistriboptionsTableAdapter adapter_assem_and_dist = new est_assemdistriboptionsTableAdapter();
                est_componentTableAdapter adapter_comp = new est_componentTableAdapter();
                pub_pubgroupTableAdapter adapter_group = new pub_pubgroupTableAdapter();
                pub_pubpubgroup_mapTableAdapter adapter_group_map = new pub_pubpubgroup_mapTableAdapter();
                est_packageTableAdapter adapter_pkg = new est_packageTableAdapter();
                est_packagecomponentmappingTableAdapter adapter_pkg_2_comp = new est_packagecomponentmappingTableAdapter();
                est_pubissuedatesTableAdapter adapter_issuedates = new est_pubissuedatesTableAdapter();
                #endregion

                #region Set SQL Connection on Adapters
                adapter_est.Connection = conn;
                adapter_samples.Connection = conn;
                adapter_assem_and_dist.Connection = conn;
                adapter_comp.Connection = conn;
                adapter_group.Connection = conn;
                adapter_group_map.Connection = conn;
                adapter_pkg.Connection = conn;
                adapter_pkg_2_comp.Connection = conn;
                adapter_issuedates.Connection = conn;
                #endregion

                // Update Top Level Tables
                adapter_est.Update(_dsEstimate.est_estimate);
                adapter_samples.Update(_dsEstimate.est_samples);
                adapter_assem_and_dist.Update(_dsEstimate.est_assemdistriboptions);

                // Run Deletes of Packages and Groups Prior Updating Components
                adapter_pkg_2_comp.Update(_dsEstimate.est_packagecomponentmapping.Select("", "", DataViewRowState.Deleted));
                adapter_pkg.Update(_dsEstimate.est_package.Select("", "", DataViewRowState.Deleted));
                adapter_group_map.Update(_dsEstimate.pub_pubpubgroup_map.Select("", "", DataViewRowState.Deleted));
                adapter_group.Update(_dsEstimate.pub_pubgroup.Select("", "", DataViewRowState.Deleted));

                // Update Components
                /* Components have no defined "sort order".
                 * Save the Host Component First and subsequent components in the order they were entered into the system.
                */
                adapter_comp.Update(_dsEstimate.est_component.Select("est_componenttype_id = 1"));
                adapter_comp.Update(_dsEstimate.est_component.Select("est_componenttype_id <> 1 and est_component_id > 0"));
                adapter_comp.Update(_dsEstimate.est_component.Select("est_componenttype_id <> 1 and est_component_id < 1", "est_component_id desc"));
                adapter_comp.Update(_dsEstimate.est_component.Select("", "", DataViewRowState.Deleted));

                // Update Packages and Groups
                adapter_group.Update(_dsEstimate.pub_pubgroup);
                adapter_group_map.Update(_dsEstimate.pub_pubpubgroup_map);
                adapter_pkg.Update(_dsEstimate.est_package);
                adapter_pkg_2_comp.Update(_dsEstimate.est_packagecomponentmapping);

                // Update Issue Dates
                adapter_issuedates.Update(_dsEstimate.est_pubissuedates);

                #region Dispose Adapters
                adapter_est.Dispose();
                adapter_samples.Dispose();
                adapter_assem_and_dist.Dispose();
                adapter_comp.Dispose();
                adapter_group.Dispose();
                adapter_group_map.Dispose();
                adapter_pkg.Dispose();
                adapter_pkg_2_comp.Dispose();
                adapter_issuedates.Dispose();
                #endregion

                conn.Close();
            }
            if (!SaveError)
            {
                this._EstimateID = _dsEstimate.est_estimate[0].est_estimate_id;
                this.SetDirtyFalseOnChildControls();
                this.SetIsLoadingOnChildControls(false);
                this.Reload();
                // Reload the uctDistributionMappings Control
                ((uctDistributionMapping)this.ChildControls[2]).Mappings.Reload();

                ChildForm child = (ChildForm)ParentForm;
                child.LastAction = string.Format( "Last Saved: {0}", DateTime.Now );

                base.SaveData();
            }

            if ( _dsEstimate.est_estimate[0].est_estimate_id < 0 )
                ParentForm.Text = string.Concat(CatalogEstimating.Properties.Resources.ApplicationEnvironment, " ", CatalogEstimating.Properties.Resources.EstimateFormTitle, " - New");
            else
                ParentForm.Text = string.Concat(CatalogEstimating.Properties.Resources.ApplicationEnvironment, " ", CatalogEstimating.Properties.Resources.EstimateFormTitle, "# ", _dsEstimate.est_estimate[0].est_estimate_id.ToString(), " - ", _dsEstimate.est_estimate[0].description);

            this.IsLoading = false;
        }

        public override ToolStrip Toolbar
        {
            get
            {
                return _toolStrip;
            }
        }

        private void _btnSave_Click(object sender, EventArgs e)
        {
            CancelEventArgs ctrlEvent = new CancelEventArgs();
            PreSave(ctrlEvent);

            if (!ctrlEvent.Cancel)
                SaveData();
        }

        private void _btnHome_Click( object sender, EventArgs e )
        {
            ChildForm childform = (ChildForm)ParentForm;
            if ( childform.MainForm.WindowState == FormWindowState.Minimized )
                childform.MainForm.WindowState = FormWindowState.Normal;
            childform.MainForm.Activate();
        }

        private void _btnPrint_Click( object sender, EventArgs e )
        {
            ExcelWriter writer = null;
            try
            {
                if (SelectedControl.GetType() == typeof(ucpCostSummary) )
                {
                    SelectedControl.Export(ref writer);
                }
                else
                {
                    writer = new ExcelWriter();
                    SelectedControl.Export(ref writer);
                    writer.Show();
                }
            }
            finally
            {
                writer.Dispose();
            }
        }

        private void _btnAddToPolybag_Click( object sender, EventArgs e )
        {
            if ( _dsEstimate.est_estimate[0].RowState == DataRowState.Added )
                MessageBox.Show( Resources.CannotAddUnsavedEstimateToPolybagError, "Cannot Add to Polybag Group" );
            else
            {
                ChildForm child = (ChildForm)ParentForm;

                List<long> estimateId = new List<long>();
                estimateId.Add( _dsEstimate.est_estimate[0].est_estimate_id );

                child.MainForm.AddEstimatesToPolybagGroup( estimateId );
            }
        }

        private void _btnKill_Click(object sender, EventArgs e)
        {
            CancelEventArgs ctrlEvent = new CancelEventArgs();
            PreSave(ctrlEvent);

            if (!ctrlEvent.Cancel)
            {
                bool flagInPolybag = false;

                using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("EstEstimatePolybagGroupMap_s_ByEstimateID", conn);
                    cmd.CommandTimeout = 7200;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EST_Estimate_ID", _dsEstimate.est_estimate[0].est_estimate_id);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                        flagInPolybag = true;

                    conn.Close();
                }

                if (flagInPolybag)
                {
                    MessageBox.Show("The Estimate is being referenced by a Polybag Group.", "Cannot Kill Estimate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                DialogResult result = MessageBox.Show("Are you sure you wish to kill this estimate?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                    return;

                _dsEstimate.est_estimate[0].est_status_id = 3;
                SaveData();
                ParentForm.Close();
            }
        }

        private void _btnUpload_Click(object sender, EventArgs e)
        {
            CancelEventArgs ctrlEvent = new CancelEventArgs();
            PreSave(ctrlEvent);

            if (!ctrlEvent.Cancel)
            {
                SaveData();

                List<long> estimateIds = new List<long>();
                estimateIds.Add(_EstimateID.Value);

                ChildForm child = (ChildForm)ParentForm;
                child.MainForm.DisplayUploadControl(estimateIds);
            }
        }

        private void _btnCopy_Click(object sender, EventArgs e)
        {
            CancelEventArgs ctrlEvent = new CancelEventArgs();
            PreSave(ctrlEvent);

            if (!ctrlEvent.Cancel)
            {
                SaveData();

                List<long> estimateIds = new List<long>();
                estimateIds.Add(_EstimateID.Value);
                CopyNumberDialog dialogCopyNumber = new CopyNumberDialog(estimateIds);
                dialogCopyNumber.ShowDialog();
                dialogCopyNumber.Activate();
            }
        }

        public ucpEstimateSetup EstimateSetupPanel
        {
            get { return (ucpEstimateSetup) ChildControls[0]; }
        }

        private void _btnCancel_Click( object sender, EventArgs e )
        {
            bool doCancel = false;
            if ( Dirty )
            {
                DialogResult result = MessageBox.Show( Resources.CancelChangesWarning, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
                if ( result == DialogResult.Yes )
                    doCancel = true;
            }
            else
                doCancel = true;

            if ( doCancel )
            {
                _preventFillDataset = false;
                LoadData();

                if (_ucpComponents.GetType() != SelectedControl.GetType())
                    _ucpComponents.Reload();
                SelectedControl.Reload();
                SetDirtyFalseOnChildControls();
            }
        }
    }
}
