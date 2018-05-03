#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using CatalogEstimating.CustomControls;
using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.EstimatesTableAdapters;
using CatalogEstimating.Datasets.DistributionMappingTableAdapters;
using CatalogEstimating.Properties;
#endregion

namespace CatalogEstimating.UserControls.Estimate
{
    public partial class ucpInsertSetup : CatalogEstimating.UserControlPanel
    {
        #region Private Variables

        private bool _readOnly = true;
        public Datasets.Estimates _dsEstimate;
        public Datasets.DistributionMapping _dsDistMapping;
        private List<long> _overlappedRatemapIDs = new List<long>();
        private string _invalidMsg = string.Empty;

        #endregion

        #region Construction

        public ucpInsertSetup()
        {
            this.IsLoading = true;
            InitializeComponent();
            Name = "Insert Setup";
        }

        public ucpInsertSetup(Datasets.Estimates dsEstimate, Datasets.DistributionMapping dsDistMapping, bool readOnly)
            :this()
        {
            _dsEstimate = dsEstimate;
            _dsDistMapping = dsDistMapping;
            _readOnly = readOnly;
        }

        #endregion

        #region Overrides

        public override void Reload()
        {
            bool isValid = false;

            isValid = ValidateInsertSetup();
            this.IsLoading = true;
            _lblInfoText.Text = string.Empty;

            _dsDistMapping.InsertSetupInfo.DefaultView.Sort = "displaySortOrder";
            insertSetupInfoBindingSource.DataSource = _dsDistMapping;

            InitializePlusMinus();
            CalcTotalQuantity();

            if (_dsEstimate.est_assemdistriboptions.Count == 0)
            {
                SetAccessLevel(false);
                _lblInfoText.Text = Resources.AssemblyDistributionRequired;
            }
            else 
            {
                if (!isValid && !_readOnly)
                {
                    SetAccessLevel(false);
                    _grdInsertSetup.Enabled = true;
                    _grdInsertSetup.AllowUserToDeleteRows = true;
                }
                else
                     SetAccessLevel(!_readOnly);
           }

            if (!isValid)
            {
                HighlightOverlaps();
                MessageBox.Show(_invalidMsg, "Invalid Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            base.LoadData();
            _cboScenario.Focus();
            this.IsLoading = false;
            SetTotalsLocation();

            if (_grdInsertSetup.RowCount > 0)
                _grdInsertSetup.CurrentCell = _grdInsertSetup[0, 0];
        }

        public override void Export( ref ExcelWriter writer )
        {
            writer.WriteTable( _grdInsertSetup, true );

            // Make the total column fall under the Quantity Column which is the 5th Column
            writer.WriteLine();
            writer.WriteLine( "", "", "", _lblTotal.Text, _txtTotalQuantity.Text );
        }

        public override void OnLeaving(CancelEventArgs e)
        {
            if (ValidateInsertSetup())
            {
                UpdateOverrides();
            }
            else
            {
                HighlightOverlaps();
                string msg = string.Concat(_invalidMsg, "\n\nChanges to this screen may be lost if you leave.  Do you wish to continue?");
                
                DialogResult dr = MessageBox.Show(msg, "Invalid Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (dr == DialogResult.No)
                    e.Cancel = true;
            }

            base.OnLeaving(e);
        }

        public override void PreSave(CancelEventArgs e)
        {
            if (ValidateInsertSetup())
            {
                UpdateOverrides();
            }
            else
            {
                HighlightOverlaps();
                e.Cancel = true;
            }

            base.PreSave(e);
        }

        #endregion

        #region Public Methods

        public bool ValidateInsertSetup()
        {
            bool isValid = false;
            List<long> ratemapIDs = new List<long>();

            ratemapIDs.Clear();
            _overlappedRatemapIDs.Clear();
            _invalidMsg = string.Empty;

            this.IsLoading = true;

            if (_dsEstimate.est_assemdistriboptions.Count > 0)
            {
                if (_readOnly)
                {
                    ResetInsertInfo(_dsDistMapping);

                    if (_dsDistMapping.InsertSetupInfo.Count > 0)
                    {
                        _dsDistMapping.InsertSetupInfo.DefaultView.Sort = "displaySortOrder";
                        insertSetupInfoBindingSource.DataSource = _dsDistMapping;

                        InitializePlusMinus();
                        CalcTotalQuantity();
                    }

                    isValid = true;
                }
                else
                {
                    UpdateOverrides();
                    ReloadCombos();
                    isValid = AutoCorrectPubLocs(ref _invalidMsg);

                    if (isValid)
                    {
                        ResetInsertInfo(_dsDistMapping);

                        if (_dsDistMapping.InsertSetupInfo.Count > 0)
                        {
                            _dsDistMapping.InsertSetupInfo.DefaultView.Sort = "displaySortOrder";
                            insertSetupInfoBindingSource.DataSource = _dsDistMapping;

                            InitializePlusMinus();
                            CalcTotalQuantity();

                            #region PubLoc Overlap
                            foreach (DistributionMapping.InsertSetupInfoRow isir in _dsDistMapping.InsertSetupInfo)
                            {
                                if (!isir.Ispub_pubrate_map_idNull())
                                {
                                    if (ratemapIDs.Contains(isir.pub_pubrate_map_id))
                                    {
                                        if (!_overlappedRatemapIDs.Contains(isir.pub_pubrate_map_id))
                                        {
                                            _overlappedRatemapIDs.Add(isir.pub_pubrate_map_id);
                                        }
                                    }
                                    else
                                    {
                                        ratemapIDs.Add(isir.pub_pubrate_map_id);
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
            }

            if (_invalidMsg == string.Empty)
            {
                if (_overlappedRatemapIDs.Count == 0)
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                    DataView pubs = new DataView(_dsDistMapping.Pub_Active);
                    DataView locs = new DataView(_dsDistMapping.PubLoc_Active);
                    _invalidMsg = "The following publications exist in more than one group.\nThis estimate cannot be saved until this is corrected.\n";

                    for (int idx = 0; idx < _overlappedRatemapIDs.Count; idx++)
                    {
                        locs.RowFilter = string.Concat("pub_pubrate_map_id = ", _overlappedRatemapIDs[idx].ToString());
                        pubs.RowFilter = string.Concat("pub_id = '", locs[0]["pub_id"].ToString(), "'");
                        _invalidMsg += string.Concat("\n\t", pubs[0]["pub_nm"].ToString(),
                            "(", locs[0]["publoc_id"].ToString(), ")");
                    }
                }
            }

            this.IsLoading = false;

            return isValid;
        }

        #endregion

        #region Event Handlers

        private void ucpInsertSetup_Resize(object sender, EventArgs e)
        {
            SetTotalsLocation();
        }

        private void btnAddScenario_Click(object sender, EventArgs e)
        {
            if (_cboScenario.SelectedValue == null)
            {
                MessageBox.Show("A Scenario is not selected.  Please select a Scenario and retry.", "Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #region Calculate InsertDate and DOW
            DateTime insertDate = _dsEstimate.est_estimate[0].rundate;
            int runDOW = ((int)_dsEstimate.est_estimate[0].rundate.DayOfWeek) + 1;
            int insDOW = _dsEstimate.est_assemdistriboptions[0].insertdow;

            if (runDOW < insDOW)
            {
                insertDate = insertDate.AddDays(insDOW - runDOW - 7);
            }
            else if (runDOW > insDOW)
            {
                insertDate = insertDate.AddDays(insDOW - runDOW);
            }
            #endregion

            long scenario_id = (long)_cboScenario.SelectedValue;
            long pubgroup_id = 0;

            DataView scenario = new DataView(_dsDistMapping.PubInsertScenario_Active);
            scenario.RowFilter = string.Concat("pub_insertscenario_id = ", scenario_id.ToString());

            #region Check for Scenario Overlap
            DataView dv = new DataView(_dsEstimate.est_package);
            dv.RowFilter = string.Concat("pub_insertscenario_id = ", scenario_id.ToString());

            if (dv.Count > 0)
            {
                MessageBox.Show(Resources.OverlapScenario, "Pub Scenario Overlap", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            #endregion

            #region Check Groups for overlap
            #region Reset Datatables
            _dsDistMapping.pub_groupinsertscenario_map.Clear();
            _dsDistMapping.pub_insertscenario.Clear();
            _dsDistMapping.pub_pubrate_map_activate.Clear();
            _dsDistMapping.pub_pubrate.Clear();
            _dsDistMapping.pubpubpubgroup_map.Clear();
            _dsDistMapping.pubpubgroup.Clear();
            #endregion

            #region Get scenario info from DB for pub_insertscenario_id
            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();
                using (pub_insertscenarioTableAdapter adapter = new pub_insertscenarioTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsDistMapping.pub_insertscenario, scenario_id);
                }

                using (pub_groupinsertscenario_mapTableAdapter adapter = new pub_groupinsertscenario_mapTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsDistMapping.pub_groupinsertscenario_map, scenario_id);
                }

                conn.Close();
            }
            #endregion

            #region Loop thru Groups checking for overlap
            DataView groups = new DataView(_dsDistMapping.InsertSetupInfo);
            string overlapping_Groups = string.Empty;

            foreach (DistributionMapping.pub_groupinsertscenario_mapRow row in _dsDistMapping.pub_groupinsertscenario_map.Select("", "", DataViewRowState.CurrentRows))
            {
                groups.RowFilter = string.Concat("GroupName = '", row.pubgroupdescription.Replace("'", "''"), "'");
                if (groups.Count > 0)
                    overlapping_Groups += string.Concat("\n\tGroup: ", row.pubgroupdescription);
            }

            if (!string.IsNullOrEmpty(overlapping_Groups))
            {
                overlapping_Groups = string.Concat(Resources.OverlapScenarioInternalPubLoc, overlapping_Groups);
                MessageBox.Show(overlapping_Groups, "PubLoc Overlap", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            #endregion
            #endregion
            
            #region Group Check Each Group in Scenario for Pub Loc Overlap
            List<long> publoclist = new List<long>();
            string overlapping_Group_Locs = string.Empty;

            string overlapping_Locs = string.Empty;
            int qtyType = (int)_cboQuantityGroup.SelectedValue;
            string qtyTypeDesc = _cboQuantityGroup.Text;
            DataView group = new DataView(_dsDistMapping.PubPubGroup_Active);
            DataView locs = new DataView(_dsDistMapping.InsertSetupInfo);

            foreach (DistributionMapping.pub_groupinsertscenario_mapRow grpRow in _dsDistMapping.pub_groupinsertscenario_map.Select("", "", DataViewRowState.CurrentRows))
            {
                group.RowFilter = string.Concat("description = '", grpRow.pubgroupdescription.Replace("'", "''"), "'");

                if (group.Count > 0)
                {
                    pubgroup_id = (long)group[0]["pub_pubgroup_id"];

                    #region Get Pubs and PubLocs from DB for group_id
                    using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
                    {
                        conn.Open();
                        using (pub_pubrate_mapTableAdapter adapter = new pub_pubrate_mapTableAdapter())
                        {
                            adapter.Connection = conn;
                            adapter.Fill(_dsDistMapping.pub_pubrate_map, _dsEstimate.est_estimate[0].rundate, pubgroup_id);
                        }

                        using (pubpubgroupTableAdapter adapter = new pubpubgroupTableAdapter())
                        {
                            adapter.Connection = conn;
                            adapter.Fill(_dsDistMapping.pubpubgroup, pubgroup_id);
                        }

                        using (pubpubpubgroup_mapTableAdapter adapter = new pubpubpubgroup_mapTableAdapter())
                        {
                            adapter.Connection = conn;
                            adapter.Fill(_dsDistMapping.pubpubpubgroup_map, pubgroup_id);
                        }

                        conn.Close();
                    }
                    #endregion

                    #region Loop thru PubLocs checking for overlap
                    foreach (DistributionMapping.pub_pubrate_mapRow plRow in _dsDistMapping.pub_pubrate_map.Select("", "", DataViewRowState.CurrentRows))
                    {
                        locs.RowFilter = string.Concat("pub_pubrate_map_id = ", plRow.pub_pubrate_map_id.ToString());
                        if (locs.Count > 0)
                            overlapping_Locs += string.Concat("\n\tPub-Loc: ", plRow.pub_id, "-", plRow.publoc_id);

                        if (publoclist.IndexOf(plRow.pub_pubrate_map_id) < 0)
                        {
                            publoclist.Add(plRow.pub_pubrate_map_id);
                        }
                        else
                        {
                            overlapping_Group_Locs += string.Concat("\n\tPub-Loc: ", plRow.pub_id, "-", plRow.publoc_id);
                        }
                    }

                    #endregion
                }
            }

            if (!string.IsNullOrEmpty(overlapping_Locs))
            {
                overlapping_Locs = string.Concat(Resources.OverlapScenarioPubLoc, overlapping_Locs);
                MessageBox.Show(overlapping_Locs, "Pub Loc Overlap", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            if (!string.IsNullOrEmpty(overlapping_Group_Locs))
            {
                overlapping_Group_Locs = string.Concat(Resources.OverlapScenarioPubLoc, overlapping_Group_Locs);
                MessageBox.Show(overlapping_Group_Locs, "Pub Loc Overlap", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            #endregion

            #region Add Each Group in Scenario
            foreach (DistributionMapping.pub_groupinsertscenario_mapRow grpRow in _dsDistMapping.pub_groupinsertscenario_map.Select("", "", DataViewRowState.CurrentRows))
            {
                group.RowFilter = string.Concat("description = '", grpRow.pubgroupdescription.Replace("'", "''"), "'");

                if (group.Count > 0)
                {
                    pubgroup_id = (long)group[0]["pub_pubgroup_id"];

                    #region Get Pubs and PubLocs from DB for group_id
                    using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
                    {
                        conn.Open();
                        using (pub_pubrate_mapTableAdapter adapter = new pub_pubrate_mapTableAdapter())
                        {
                            adapter.Connection = conn;
                            adapter.Fill(_dsDistMapping.pub_pubrate_map, _dsEstimate.est_estimate[0].rundate, pubgroup_id);
                        }

                        using (pubpubgroupTableAdapter adapter = new pubpubgroupTableAdapter())
                        {
                            adapter.Connection = conn;
                            adapter.Fill(_dsDistMapping.pubpubgroup, pubgroup_id);
                        }

                        using (pubpubpubgroup_mapTableAdapter adapter = new pubpubpubgroup_mapTableAdapter())
                        {
                            adapter.Connection = conn;
                            adapter.Fill(_dsDistMapping.pubpubpubgroup_map, pubgroup_id);
                        }

                        conn.Close();
                    }
                    #endregion

                    #region Add Group to Estimate
                    if (_dsDistMapping.pub_pubrate_map.Count > 0)
                    {
                        Dirty = true;

                        #region Add new row to pub_pubgroup
                        Estimates.pub_pubgroupRow gr = null;
                        DataView estGroups = new DataView(_dsEstimate.pub_pubgroup);
                        estGroups.RowFilter = string.Concat("pub_pubgroup_id = ", pubgroup_id.ToString());

                        if (estGroups.Count == 0)
                        {
                            gr = _dsEstimate.pub_pubgroup.Newpub_pubgroupRow();
                            gr.BeginEdit();
                            gr.pub_pubgroup_id = pubgroup_id;
                            gr.description = _dsDistMapping.pubpubgroup[0].description;
                            gr.effectivedate = _dsDistMapping.pubpubgroup[0].effectivedate;
                            gr.sortorder = _dsDistMapping.pubpubgroup[0].sortorder;
                            gr.customgroupforpackage = _dsDistMapping.pubpubgroup[0].customgroupforpackage;
                            gr.active = _dsDistMapping.pubpubgroup[0].active;
                            gr.createdby = _dsDistMapping.pubpubgroup[0].createdby;
                            gr.createddate = _dsDistMapping.pubpubgroup[0].createddate;
                            if (_dsDistMapping.pubpubgroup[0].IsmodifiedbyNull())
                                gr.SetmodifiedbyNull();
                            else
                                gr.modifiedby = _dsDistMapping.pubpubgroup[0].modifiedby;
                            if (_dsDistMapping.pubpubgroup[0].IsmodifieddateNull())
                                gr.SetmodifieddateNull();
                            else
                                gr.modifieddate = _dsDistMapping.pubpubgroup[0].modifieddate;
                            gr.EndEdit();
                            _dsEstimate.pub_pubgroup.Addpub_pubgroupRow(gr);
                            gr.AcceptChanges();
                        }
                        else
                        {
                            gr = (Estimates.pub_pubgroupRow)estGroups[0].Row;
                        }
                        #endregion

                        #region Add new row to est_package
                        Estimates.est_packageRow pr = _dsEstimate.est_package.Newest_packageRow();
                        pr.BeginEdit();
                        pr.est_estimate_id = _dsEstimate.est_estimate[0].est_estimate_id;
                        pr.description = gr.description;
                        pr.soloquantity = 0;
                        pr.otherquantity = 0;
                        pr.pub_pubquantitytype_id = qtyType;
                        pr.pub_pubgroup_id = pubgroup_id;
                        pr.pub_insertscenario_id = scenario_id;
                        pr.createdby = MainForm.AuthorizedUser.FormattedName;
                        pr.createddate = DateTime.Now;
                        pr.EndEdit();
                        _dsEstimate.est_package.Addest_packageRow(pr);
                        #endregion

                        #region Add new row to DistributionMapping table for Group
                        DistributionMapping.InsertSetupInfoRow isir = _dsDistMapping.InsertSetupInfo.NewInsertSetupInfoRow();
                        isir.BeginEdit();
                        isir.est_package_id = pr.est_package_id;
                        isir.PackageName = pr.description;
                        isir.pub_pubquantitytype_id = qtyType;
                        isir.pub_pubquantitytype = qtyTypeDesc;
                        isir.pub_pubgroup_id = pubgroup_id;
                        isir.GroupName = gr.description;
                        isir.GroupActiveFlag = gr.active;
                        isir.GroupEffectiveDate = gr.effectivedate;
                        isir.sortorder = gr.sortorder;
                        isir.customgroupforpackage = gr.customgroupforpackage;
                        isir.Setpub_pubrate_map_idNull();
                        isir._override = false;
                        isir.SetquantityNull();
                        isir.GridDescription = gr.description.Trim();
                        isir.ScenarioFlag = true;
                        isir.GroupFlag = true;
                        isir.display = 1;
                        isir.displaySortOrder = CalcDisplaySortValue(gr.sortorder, gr.pub_pubgroup_id, "   ", 0);
                        isir.EndEdit();
                        _dsDistMapping.InsertSetupInfo.AddInsertSetupInfoRow(isir);
                        #endregion

                        // foreach publoc in group
                        foreach (DistributionMapping.pub_pubrate_mapRow row in _dsDistMapping.pub_pubrate_map.Select("", "", DataViewRowState.CurrentRows))
                        {
                            #region Get info from DB by Pub and PubLoc
                            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
                            {
                                conn.Open();

                                using (PubIssueInfoTableAdapter adapter = new PubIssueInfoTableAdapter())
                                {
                                    adapter.Connection = conn;
                                    adapter.Fill(_dsDistMapping.PubIssueInfo, row.pub_pubrate_map_id,
                                        qtyType, insertDate, _dsEstimate.est_assemdistriboptions[0].inserttime);
                                }

                                conn.Close();
                            }
                            #endregion

                            #region Add new row to pub_pubpubgroupmap
                            Estimates.pub_pubpubgroup_mapRow mr = null;
                            DataView estGroupMaps = new DataView(_dsEstimate.pub_pubpubgroup_map);
                            estGroupMaps.RowFilter = string.Concat("pub_pubgroup_id = ", pubgroup_id.ToString(),
                                " and pub_pubrate_map_id = ", row.pub_pubrate_map_id.ToString());

                            if (estGroupMaps.Count == 0)
                            {
                                mr = _dsEstimate.pub_pubpubgroup_map.Newpub_pubpubgroup_mapRow();
                                mr.BeginEdit();
                                mr.pub_pubgroup_id = pubgroup_id;
                                mr.pub_pubrate_map_id = row.pub_pubrate_map_id;
                                mr.createdby = MainForm.AuthorizedUser.FormattedName;
                                mr.createddate = DateTime.Now;
                                mr.EndEdit();
                                _dsEstimate.pub_pubpubgroup_map.Addpub_pubpubgroup_mapRow(mr);
                                mr.AcceptChanges();
                            }
                            else
                            {
                                mr = (Estimates.pub_pubpubgroup_mapRow)estGroupMaps[0].Row;
                            }
                            #endregion

                            #region Add new row to est_pubinsertdates
                            Estimates.est_pubissuedatesRow ir = null;
                            DataView id = new DataView(_dsEstimate.est_pubissuedates);
                            id.RowFilter = string.Concat("est_estimate_id = ", _dsEstimate.est_estimate[0].est_estimate_id.ToString(),
                                " and pub_pubrate_map_id = ", row.pub_pubrate_map_id.ToString());
                            if (id.Count > 0)
                            {
                                ir = (Estimates.est_pubissuedatesRow)id[0].Row;
                                if (ir.issuedow != _dsDistMapping.PubIssueInfo[0].issuedow)
                                {
                                    ir.issuedow = _dsDistMapping.PubIssueInfo[0].issuedow;
                                    ir.modifiedby = MainForm.AuthorizedUser.FormattedName;
                                    ir.modifieddate = DateTime.Now;
                                }
                                if (ir.issuedate != _dsDistMapping.PubIssueInfo[0].issuedate)
                                {
                                    ir.issuedate = _dsDistMapping.PubIssueInfo[0].issuedate;
                                    ir.modifiedby = MainForm.AuthorizedUser.FormattedName;
                                    ir.modifieddate = DateTime.Now;
                                }
                            }
                            else
                            {
                                ir = _dsEstimate.est_pubissuedates.Newest_pubissuedatesRow();
                                ir.BeginEdit();
                                ir.est_estimate_id = _dsEstimate.est_estimate[0].est_estimate_id;
                                ir.pub_pubrate_map_id = row.pub_pubrate_map_id;
                                ir.issuedow = _dsDistMapping.PubIssueInfo[0].issuedow;
                                ir.issuedate = _dsDistMapping.PubIssueInfo[0].issuedate;
                                ir._override = false;
                                ir.createdby = MainForm.AuthorizedUser.FormattedName;
                                ir.createddate = DateTime.Now;
                                ir.EndEdit();
                                _dsEstimate.est_pubissuedates.Addest_pubissuedatesRow(ir);
                            }
                            #endregion

                            #region Add new row to DistributionMapping table for PubLoc
                            isir = _dsDistMapping.InsertSetupInfo.NewInsertSetupInfoRow();
                            isir.BeginEdit();
                            isir.est_package_id = pr.est_package_id;
                            isir.PackageName = pr.description;
                            isir.pub_pubquantitytype_id = qtyType;
                            isir.pub_pubgroup_id = pubgroup_id;
                            isir.GroupName = gr.description;
                            isir.GroupActiveFlag = gr.active;
                            isir.GroupEffectiveDate = gr.effectivedate;
                            isir.sortorder = gr.sortorder;
                            isir.customgroupforpackage = gr.customgroupforpackage;
                            isir.pub_pubrate_map_id = row.pub_pubrate_map_id;
                            isir.pub_nm = _dsDistMapping.PubIssueInfo[0].pub_nm;
                            isir.publoc_nm = _dsDistMapping.PubIssueInfo[0].publoc_nm;
                            isir._override = false;
                            isir.issuedow = _dsDistMapping.PubIssueInfo[0].issuedow;
                            isir.DisplayDOW = DOW_Text(isir.issuedow);
                            isir.issuedate = _dsDistMapping.PubIssueInfo[0].issuedate;
                            isir.quantity = _dsDistMapping.PubIssueInfo[0].quantity;
                            isir.GridDescription = string.Concat("        ", _dsDistMapping.PubIssueInfo[0].pub_nm.Trim(), " - (", _dsDistMapping.PubIssueInfo[0].publoc_id.ToString(), ")");
                            isir.ScenarioFlag = false;
                            isir.GroupFlag = false;
                            isir.display = 1;
                            isir.displaySortOrder = CalcDisplaySortValue(gr.sortorder, gr.pub_pubgroup_id, _dsDistMapping.PubIssueInfo[0].pub_id, _dsDistMapping.PubIssueInfo[0].publoc_id);
                            isir.EndEdit();
                            _dsDistMapping.InsertSetupInfo.AddInsertSetupInfoRow(isir);
                            #endregion
                        }
                    }
                    #endregion
                }
            }
            #endregion

            CalcTotalQuantity();
            InitializePlusMinus();
        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            if (_cboPublicationGroup.SelectedValue == null)
            {
                MessageBox.Show("A Group is not selected.  Please select a Group and retry.", "Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #region Calculate InsertDate and DOW
            DateTime insertDate = _dsEstimate.est_estimate[0].rundate;
            int runDOW = ((int)_dsEstimate.est_estimate[0].rundate.DayOfWeek) + 1;
            int insDOW = _dsEstimate.est_assemdistriboptions[0].insertdow;

            if (runDOW < insDOW)
            {
                insertDate = insertDate.AddDays(insDOW - runDOW - 7);
            }
            else if (runDOW > insDOW)
            {
                insertDate = insertDate.AddDays(insDOW - runDOW);
            }
            #endregion

            long pubgroup_id = (long)_cboPublicationGroup.SelectedValue;
            int qtyType = (int)_cboQuantityGroup.SelectedValue;
            string qtyTypeDesc = _cboQuantityGroup.Text;

            DataView group = new DataView(_dsDistMapping.PubPubGroup_Active);
            group.RowFilter = string.Concat("pub_pubgroup_id = ", pubgroup_id.ToString());

            #region Check for Group Overlap
            DataView dv = new DataView(_dsDistMapping.InsertSetupInfo);
            dv.RowFilter = string.Concat("pub_pubgroup_id = ", pubgroup_id.ToString());

            if (dv.Count > 0)
            {
                MessageBox.Show(Resources.OverlapGroup, "Pub Group Overlap", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            #endregion

            #region Check Group for Pub Loc Overlap
            #region Reset Datatables
            _dsDistMapping.pub_pubrate_map_activate.Clear();
            _dsDistMapping.pub_pubrate.Clear();
            _dsDistMapping.pubpubpubgroup_map.Clear();
            _dsDistMapping.pubpubgroup.Clear();
            #endregion

            #region Get Pubs and PubLocs from DB for group_id
            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();
                using (pub_pubrate_mapTableAdapter adapter = new pub_pubrate_mapTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsDistMapping.pub_pubrate_map, _dsEstimate.est_estimate[0].rundate, pubgroup_id);
                }

                using (pubpubgroupTableAdapter adapter = new pubpubgroupTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsDistMapping.pubpubgroup, pubgroup_id);
                }

                using (pubpubpubgroup_mapTableAdapter adapter = new pubpubpubgroup_mapTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsDistMapping.pubpubpubgroup_map, pubgroup_id);
                }

                conn.Close();
            }
            #endregion

            #region Loop thru PubLocs checking for overlap
            DataView locs = new DataView(_dsDistMapping.InsertSetupInfo);
            string overlapping_Locs = string.Empty;

            foreach (DistributionMapping.pub_pubrate_mapRow row in _dsDistMapping.pub_pubrate_map.Select("", "", DataViewRowState.CurrentRows))
            {
                locs.RowFilter = string.Concat("pub_pubrate_map_id = ", row.pub_pubrate_map_id.ToString());
                if (locs.Count > 0)
                    overlapping_Locs += string.Concat("\n\tPub-Loc: ", row.pub_id, "-", row.publoc_id);
            }

            if (!string.IsNullOrEmpty(overlapping_Locs))
            {
                overlapping_Locs = string.Concat(Resources.OverlapGroupPubLoc, overlapping_Locs);
                MessageBox.Show(overlapping_Locs, "PubLoc Overlap", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            #endregion
            #endregion

            Dirty = true;

            #region Add Group to Estimate
            #region Add new row to pub_pubgroup
            Estimates.pub_pubgroupRow gr = null;
            DataView estGroups = new DataView(_dsEstimate.pub_pubgroup);
            estGroups.RowFilter = string.Concat("pub_pubgroup_id = ", _dsDistMapping.pubpubgroup[0].pub_pubgroup_id.ToString());

            if (estGroups.Count == 0)
            {
                gr = _dsEstimate.pub_pubgroup.Newpub_pubgroupRow();
                gr.BeginEdit();
                gr.pub_pubgroup_id = _dsDistMapping.pubpubgroup[0].pub_pubgroup_id;
                gr.description = _dsDistMapping.pubpubgroup[0].description;
                gr.effectivedate = _dsDistMapping.pubpubgroup[0].effectivedate;
                gr.sortorder = _dsDistMapping.pubpubgroup[0].sortorder;
                gr.customgroupforpackage = _dsDistMapping.pubpubgroup[0].customgroupforpackage;
                gr.active = _dsDistMapping.pubpubgroup[0].active;
                gr.createdby = _dsDistMapping.pubpubgroup[0].createdby;
                gr.createddate = _dsDistMapping.pubpubgroup[0].createddate;
                if (_dsDistMapping.pubpubgroup[0].IsmodifiedbyNull())
                    gr.SetmodifiedbyNull();
                else
                    gr.modifiedby = _dsDistMapping.pubpubgroup[0].modifiedby;
                if (_dsDistMapping.pubpubgroup[0].IsmodifieddateNull())
                    gr.SetmodifieddateNull();
                else
                    gr.modifieddate = _dsDistMapping.pubpubgroup[0].modifieddate;
                gr.EndEdit();
                _dsEstimate.pub_pubgroup.Addpub_pubgroupRow(gr);
                gr.AcceptChanges();
            }
            else
            {
                gr = (Estimates.pub_pubgroupRow)estGroups[0].Row;
            }
            #endregion

            #region Add new row to est_package
            Estimates.est_packageRow pr = _dsEstimate.est_package.Newest_packageRow();
            pr.BeginEdit();
            pr.est_estimate_id = _dsEstimate.est_estimate[0].est_estimate_id;
            pr.description = gr.description;
            pr.soloquantity = 0;
            pr.otherquantity = 0;
            pr.pub_pubquantitytype_id = qtyType;
            pr.pub_pubgroup_id = gr.pub_pubgroup_id;
            pr.Setpub_insertscenario_idNull();
            pr.createdby = MainForm.AuthorizedUser.FormattedName;
            pr.createddate = DateTime.Now;
            pr.EndEdit();
            _dsEstimate.est_package.Addest_packageRow(pr);
            #endregion

            #region Add new row to DistributionMapping table for Group
            DistributionMapping.InsertSetupInfoRow isir = _dsDistMapping.InsertSetupInfo.NewInsertSetupInfoRow();
            isir.BeginEdit();
            isir.est_package_id = pr.est_package_id;
            isir.PackageName = pr.description;
            isir.pub_pubquantitytype_id = qtyType;
            isir.pub_pubquantitytype = qtyTypeDesc;
            isir.pub_pubgroup_id = gr.pub_pubgroup_id;
            isir.GroupName = gr.description;
            isir.GroupActiveFlag = gr.active;
            isir.GroupEffectiveDate = gr.effectivedate;
            isir.sortorder = gr.sortorder;
            isir.customgroupforpackage = gr.customgroupforpackage;
            isir.Setpub_pubrate_map_idNull();
            isir._override = false;
            isir.quantity = 0;
            isir.GridDescription = gr.description.Trim();
            isir.ScenarioFlag = false;
            isir.GroupFlag = true;
            isir.display = 1;
            isir.displaySortOrder = CalcDisplaySortValue(gr.sortorder, gr.pub_pubgroup_id, "   ", 0);
            isir.EndEdit();
            _dsDistMapping.InsertSetupInfo.AddInsertSetupInfoRow(isir);
            #endregion

            // foreach publoc in group
            foreach (DistributionMapping.pub_pubrate_mapRow row in _dsDistMapping.pub_pubrate_map.Select("", "", DataViewRowState.CurrentRows))
            {
                #region Get info from DB by Pub and PubLoc
                using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
                {
                    conn.Open();

                    using (PubIssueInfoTableAdapter adapter = new PubIssueInfoTableAdapter())
                    {
                        adapter.Connection = conn;
                        adapter.Fill(_dsDistMapping.PubIssueInfo, row.pub_pubrate_map_id,
                            qtyType, insertDate, _dsEstimate.est_assemdistriboptions[0].inserttime);
                    }

                    conn.Close();
                }
                #endregion

                #region Add new row to pub_pubpubgroupmap
                Estimates.pub_pubpubgroup_mapRow mr = null;
                DataView estGroupMaps = new DataView(_dsEstimate.pub_pubpubgroup_map);
                estGroupMaps.RowFilter = string.Concat("pub_pubgroup_id = ", gr.pub_pubgroup_id.ToString(),
                    " and pub_pubrate_map_id = ", row.pub_pubrate_map_id.ToString());

                if (estGroupMaps.Count == 0)
                {
                    mr = _dsEstimate.pub_pubpubgroup_map.Newpub_pubpubgroup_mapRow();
                    mr.BeginEdit();
                    mr.pub_pubgroup_id = gr.pub_pubgroup_id;
                    mr.pub_pubrate_map_id = row.pub_pubrate_map_id;
                    mr.createdby = MainForm.AuthorizedUser.FormattedName;
                    mr.createddate = DateTime.Now;
                    mr.EndEdit();
                    _dsEstimate.pub_pubpubgroup_map.Addpub_pubpubgroup_mapRow(mr);
                    mr.AcceptChanges();
                }
                else
                {
                    mr = (Estimates.pub_pubpubgroup_mapRow)estGroupMaps[0].Row;
                }
                #endregion

                #region Add new row to est_pubinsertdates
                Estimates.est_pubissuedatesRow ir = null;
                DataView id = new DataView(_dsEstimate.est_pubissuedates);
                id.RowFilter = string.Concat("est_estimate_id = ", _dsEstimate.est_estimate[0].est_estimate_id.ToString(),
                    " and pub_pubrate_map_id = ", row.pub_pubrate_map_id.ToString());
                if (id.Count > 0)
                {
                    ir = (Estimates.est_pubissuedatesRow)id[0].Row;
                    if (ir.issuedow != _dsDistMapping.PubIssueInfo[0].issuedow)
                    {
                        ir.issuedow = _dsDistMapping.PubIssueInfo[0].issuedow;
                        ir.modifiedby = MainForm.AuthorizedUser.FormattedName;
                        ir.modifieddate = DateTime.Now;
                    }
                    if (ir.issuedate != _dsDistMapping.PubIssueInfo[0].issuedate)
                    {
                        ir.issuedate = _dsDistMapping.PubIssueInfo[0].issuedate;
                        ir.modifiedby = MainForm.AuthorizedUser.FormattedName;
                        ir.modifieddate = DateTime.Now;
                    }
                }
                else
                {
                    ir = _dsEstimate.est_pubissuedates.Newest_pubissuedatesRow();
                    ir.BeginEdit();
                    ir.est_estimate_id = _dsEstimate.est_estimate[0].est_estimate_id;
                    ir.pub_pubrate_map_id = row.pub_pubrate_map_id;
                    ir.issuedow = _dsDistMapping.PubIssueInfo[0].issuedow;
                    ir.issuedate = _dsDistMapping.PubIssueInfo[0].issuedate;
                    ir._override = false;
                    ir.createdby = MainForm.AuthorizedUser.FormattedName;
                    ir.createddate = DateTime.Now;
                    ir.EndEdit();
                    _dsEstimate.est_pubissuedates.Addest_pubissuedatesRow(ir);
                }
                #endregion

                #region Add new row to DistributionMapping table for PubLoc
                isir = _dsDistMapping.InsertSetupInfo.NewInsertSetupInfoRow();
                isir.BeginEdit();
                isir.est_package_id = pr.est_package_id;
                isir.PackageName = pr.description;
                isir.pub_pubquantitytype_id = qtyType;
                isir.pub_pubgroup_id = gr.pub_pubgroup_id;
                isir.GroupName = gr.description;
                isir.GroupActiveFlag = gr.active;
                isir.GroupEffectiveDate = gr.effectivedate;
                isir.sortorder = gr.sortorder;
                isir.customgroupforpackage = gr.customgroupforpackage;
                isir.pub_pubrate_map_id = row.pub_pubrate_map_id;
                isir.pub_nm = _dsDistMapping.PubIssueInfo[0].pub_nm;
                isir.publoc_nm = _dsDistMapping.PubIssueInfo[0].publoc_nm;
                isir._override = false;
                isir.issuedow = _dsDistMapping.PubIssueInfo[0].issuedow;
                isir.DisplayDOW = DOW_Text(isir.issuedow);
                isir.issuedate = _dsDistMapping.PubIssueInfo[0].issuedate;
                isir.quantity = _dsDistMapping.PubIssueInfo[0].quantity;
                isir.GridDescription = string.Concat("        ", _dsDistMapping.PubIssueInfo[0].pub_nm.Trim(), " - (", _dsDistMapping.PubIssueInfo[0].publoc_id.ToString(), ")");
                isir.ScenarioFlag = false;
                isir.GroupFlag = false;
                isir.display = 1;
                isir.displaySortOrder = CalcDisplaySortValue(gr.sortorder, gr.pub_pubgroup_id, _dsDistMapping.PubIssueInfo[0].pub_id, _dsDistMapping.PubIssueInfo[0].publoc_id);
                isir.EndEdit();
                _dsDistMapping.InsertSetupInfo.AddInsertSetupInfoRow(isir);
                #endregion
            }
            #endregion

            CalcTotalQuantity();
            InitializePlusMinus();
        }

        private void btnAddPubLoc_Click(object sender, EventArgs e)
        {
            if (_cboPublication.SelectedValue == null)
            {
                MessageBox.Show("A Publication is not selected.  Please select a Publication and retry.", "Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_cboLocation.SelectedValue == null)
            {
                MessageBox.Show("A Location is not selected.  Please select a Location and retry.", "Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string pub_id = _cboPublication.SelectedValue.ToString();
            int publoc_id = Convert.ToInt32(_cboLocation.Text);
            int qtyType = (int)_cboQuantityGroup.SelectedValue;
            string qtyTypeDesc = _cboQuantityGroup.Text;

            DataView loc = new DataView(_dsDistMapping.PubLoc_Active);
            loc.RowFilter = string.Concat("pub_id = '", pub_id, "' and publoc_id = ", publoc_id.ToString());

            #region Check for Pub Loc Overlap
            DataView dv = new DataView(_dsEstimate.pub_pubpubgroup_map);
            dv.RowFilter = string.Concat("pub_pubrate_map_id = ", loc[0]["pub_pubrate_map_id"].ToString());

            if (dv.Count > 0)
            {
                MessageBox.Show(Resources.OverlapSinglePubLoc, "PubLoc Overlap", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            #endregion

            #region Get info from DB by Pub and PubLoc
            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                using (PubIssueInfoTableAdapter adapter = new PubIssueInfoTableAdapter())
                {
                    DateTime insertDate = _dsEstimate.est_estimate[0].rundate;
                    int runDOW = ((int)_dsEstimate.est_estimate[0].rundate.DayOfWeek) + 1;
                    int insDOW = _dsEstimate.est_assemdistriboptions[0].insertdow;

                    if (runDOW < insDOW)
                    {
                        insertDate = insertDate.AddDays(insDOW - runDOW - 7);
                    }
                    else if (runDOW > insDOW)
                    {
                        insertDate = insertDate.AddDays(insDOW - runDOW);
                    }

                    adapter.Connection = conn;
                    adapter.Fill(_dsDistMapping.PubIssueInfo, (long)loc[0]["pub_pubrate_map_id"],
                        qtyType, insertDate, _dsEstimate.est_assemdistriboptions[0].inserttime);
                }

                conn.Close();
            }
            #endregion

            Dirty = true;

            #region Add new row to pub_pubgroup
            Estimates.pub_pubgroupRow gr = _dsEstimate.pub_pubgroup.Newpub_pubgroupRow();
            gr.BeginEdit();
            gr.description = string.Concat("Custom Group: ", _cboPublication.SelectedValue.ToString(), "(", publoc_id.ToString(), ")");
            if (gr.description.Length > _dsEstimate.pub_pubgroup.descriptionColumn.MaxLength)
                gr.description = gr.description.Substring(0, _dsEstimate.pub_pubgroup.descriptionColumn.MaxLength);
            gr.effectivedate = Convert.ToDateTime("1/1/1900");
            gr.sortorder = 1000000;
            gr.customgroupforpackage = true;
            gr.active = true;
            gr.createdby = MainForm.AuthorizedUser.FormattedName;
            gr.createddate = DateTime.Now;
            gr.EndEdit();
            _dsEstimate.pub_pubgroup.Addpub_pubgroupRow(gr);
            #endregion

            #region Add new row to pub_pubpubgroupmap
            Estimates.pub_pubpubgroup_mapRow mr = _dsEstimate.pub_pubpubgroup_map.Newpub_pubpubgroup_mapRow();
            mr.BeginEdit();
            mr.pub_pubgroup_id = gr.pub_pubgroup_id;
            mr.pub_pubrate_map_id = (long)loc[0]["pub_pubrate_map_id"];
            mr.createdby = MainForm.AuthorizedUser.FormattedName;
            mr.createddate = DateTime.Now;
            mr.EndEdit();
            _dsEstimate.pub_pubpubgroup_map.Addpub_pubpubgroup_mapRow(mr);
            #endregion

            #region Add new row to est_package
            Estimates.est_packageRow pr = _dsEstimate.est_package.Newest_packageRow();
            pr.BeginEdit();
            pr.est_estimate_id = _dsEstimate.est_estimate[0].est_estimate_id;
            pr.description = gr.description;
            pr.soloquantity = 0;
            pr.otherquantity = 0;
            pr.pub_pubquantitytype_id = qtyType;
            pr.pub_pubgroup_id = gr.pub_pubgroup_id;
            pr.createdby = MainForm.AuthorizedUser.FormattedName;
            pr.createddate = DateTime.Now;
            pr.EndEdit();
            _dsEstimate.est_package.Addest_packageRow(pr);
            #endregion

            #region Add new row to est_pubinsertdates
            Estimates.est_pubissuedatesRow ir = null;
            DataView id = new DataView(_dsEstimate.est_pubissuedates);
            id.RowFilter = string.Concat("est_estimate_id = ", _dsEstimate.est_estimate[0].est_estimate_id.ToString(),
                " and pub_pubrate_map_id = ", loc[0]["pub_pubrate_map_id"].ToString());
            if (id.Count > 0)
            {
                ir = (Estimates.est_pubissuedatesRow)id[0].Row;
                if (ir.issuedow != _dsDistMapping.PubIssueInfo[0].issuedow)
                {
                    ir.issuedow = _dsDistMapping.PubIssueInfo[0].issuedow;
                    ir.modifiedby = MainForm.AuthorizedUser.FormattedName;
                    ir.modifieddate = DateTime.Now;
                }
                if (ir.issuedate != _dsDistMapping.PubIssueInfo[0].issuedate)
                {
                    ir.issuedate = _dsDistMapping.PubIssueInfo[0].issuedate;
                    ir.modifiedby = MainForm.AuthorizedUser.FormattedName;
                    ir.modifieddate = DateTime.Now;
                }
            }
            else
            {
                ir = _dsEstimate.est_pubissuedates.Newest_pubissuedatesRow();
                ir.BeginEdit();
                ir.issuedow = _dsDistMapping.PubIssueInfo[0].issuedow;
                ir.issuedate = _dsDistMapping.PubIssueInfo[0].issuedate;
                ir._override = false;
                ir.est_estimate_id = _dsEstimate.est_estimate[0].est_estimate_id;
                ir.pub_pubrate_map_id = (long)loc[0]["pub_pubrate_map_id"];
                ir.createdby = MainForm.AuthorizedUser.FormattedName;
                ir.createddate = DateTime.Now;
                ir.EndEdit();
                _dsEstimate.est_pubissuedates.Addest_pubissuedatesRow(ir);
            }
            #endregion

            #region Add new row to DistributionMapping table for Group
            DistributionMapping.InsertSetupInfoRow isir = _dsDistMapping.InsertSetupInfo.NewInsertSetupInfoRow();
            isir.BeginEdit();
            isir.est_package_id = pr.est_package_id;
            isir.PackageName = pr.description;
            isir.pub_pubquantitytype_id = qtyType;
            isir.pub_pubquantitytype = qtyTypeDesc;
            isir.pub_pubgroup_id = gr.pub_pubgroup_id;
            isir.GroupName = gr.description;
            isir.GroupActiveFlag = gr.active;
            isir.GroupEffectiveDate = gr.effectivedate;
            isir.sortorder = gr.sortorder;
            isir.customgroupforpackage = gr.customgroupforpackage;
            isir.Setpub_pubrate_map_idNull();
            isir._override = false;
            isir.SetquantityNull();
            isir.GridDescription = gr.description;
            isir.ScenarioFlag = false;
            isir.GroupFlag = true;
            isir.display = 1;
            isir.displaySortOrder = CalcDisplaySortValue(gr.sortorder, gr.pub_pubgroup_id, "   ", 0);
            isir.EndEdit();
            _dsDistMapping.InsertSetupInfo.AddInsertSetupInfoRow(isir);
            #endregion

            #region Add new row to DistributionMapping table for PubLoc
            isir = _dsDistMapping.InsertSetupInfo.NewInsertSetupInfoRow();
            isir.BeginEdit();
            isir.est_package_id = pr.est_package_id;
            isir.PackageName = pr.description;
            isir.pub_pubquantitytype_id = qtyType;
            isir.pub_pubgroup_id = gr.pub_pubgroup_id;
            isir.GroupName = gr.description;
            isir.GroupActiveFlag = gr.active;
            isir.GroupEffectiveDate = gr.effectivedate;
            isir.sortorder = gr.sortorder;
            isir.customgroupforpackage = gr.customgroupforpackage;
            isir.pub_pubrate_map_id = (long)loc[0]["pub_pubrate_map_id"];
            isir.pub_nm = _cboPublication.Text;
            isir.publoc_nm = _cboLocation.SelectedValue.ToString();
            isir._override = false;
            isir.issuedow = _dsDistMapping.PubIssueInfo[0].issuedow;
            isir.DisplayDOW = DOW_Text(isir.issuedow);
            isir.issuedate = _dsDistMapping.PubIssueInfo[0].issuedate;
            isir.quantity = _dsDistMapping.PubIssueInfo[0].quantity;
            isir.GridDescription = string.Concat("        ", _cboPublication.SelectedValue.ToString(), " - (", publoc_id, ")");
            isir.ScenarioFlag = false;
            isir.GroupFlag = false;
            isir.display = 1;
            isir.displaySortOrder = CalcDisplaySortValue(gr.sortorder, gr.pub_pubgroup_id, pub_id, publoc_id);
            isir.EndEdit();
            _dsDistMapping.InsertSetupInfo.AddInsertSetupInfoRow(isir);
            #endregion

            CalcTotalQuantity();
            InitializePlusMinus();
        }

        private void _cboPublication_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cboPublication.SelectedIndex > -1)
            {
                string pub = _cboPublication.SelectedValue.ToString();
                string pub_filter = string.Concat("pub_id = '", pub, "'");

                if (pub_filter != _dsDistMapping.PubLoc_Active.DefaultView.RowFilter)
                {
                    _dsDistMapping.PubLoc_Active.DefaultView.RowFilter = pub_filter;
                    if ((_cboLocation.SelectedIndex == -1) && (_cboLocation.Items.Count > 0))
                        _cboLocation.SelectedIndex = 0;
                }
            }
        }

        private void _grdInsertSetup_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex > -1) && (_invalidMsg == string.Empty))
            {
                // Toggle the view state of the child estimate search items found
                if ((e.ColumnIndex == 0) && (_grdInsertSetup[e.ColumnIndex, e.RowIndex] != null))
                {
                    DataGridViewCell buttonCell = _grdInsertSetup[e.ColumnIndex, e.RowIndex];
                    if (buttonCell.Value != null)
                    {
                        long pub_pubgroup_id = (long)_grdInsertSetup["pubpubgroupidDataGridViewTextBoxColumn", e.RowIndex].Value;
                        int display;

                        // Toggle the +/- on the button to show expanded or collapsed
                        if (buttonCell.Value.ToString() == "+")
                        {
                            buttonCell.Value = "-";
                            display = 1;
                        }
                        else
                        {
                            buttonCell.Value = "+";
                            display = 0;
                        }

                        // Toggle the display of all the rows that have the clicked row as a parent
                        DataView dv = new DataView(_dsDistMapping.InsertSetupInfo);
                        dv.RowFilter = "pub_pubgroup_id = " + pub_pubgroup_id.ToString();

                        foreach (DataRowView rowView in dv)
                        {
                            DistributionMapping.InsertSetupInfoRow row = (DistributionMapping.InsertSetupInfoRow)rowView.Row;
                            row.display = display;
                        }
                        InitializePlusMinus();
                    }
                }

                if (e.ColumnIndex == 3)
                    _grdInsertSetup.EndEdit();

                _grdInsertSetup.Refresh();
            }
        }

        private void _grdInsertSetup_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (_readOnly)
                e.Cancel = true;

            if ((e.ColumnIndex == 2) && ((bool)_grdInsertSetup[3, e.RowIndex].EditedFormattedValue == false))
                e.Cancel = true;

            if (_invalidMsg != string.Empty)
                e.Cancel = true;
        }

        private void _grdInsertSetup_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((!this.IsLoading) && (_invalidMsg == string.Empty))
            {
                if (e.ColumnIndex == 2)
                {
                    Dirty = true;

                    int i = e.RowIndex;
                    DistributionMapping.InsertSetupInfoRow isir = (DistributionMapping.InsertSetupInfoRow)((DataRowView)_grdInsertSetup.Rows[e.RowIndex].DataBoundItem).Row;

                    isir._override = true;
                    ResetIssueDate(isir);
                }

                // if cancelling issue date override, 
                if (e.ColumnIndex == 3)
                {
                    Dirty = true;

                    if (!(bool)_grdInsertSetup[e.ColumnIndex, e.RowIndex].EditedFormattedValue)
                    {
                        ResetDefaultIssueDate(e.RowIndex);
                        _grdInsertSetup.Refresh();
                    }
                }

                _grdInsertSetup.Refresh();
            }
        }

        private void _grdInsertSetup_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DistributionMapping.InsertSetupInfoRow isir = (DistributionMapping.InsertSetupInfoRow)((DataRowView)e.Row.DataBoundItem).Row;

            if (!isir.GroupFlag)
            {
                DataView dv = new DataView(_dsDistMapping.InsertSetupInfo);
                dv.RowFilter = string.Concat("GroupFlag = false and pub_pubgroup_id = ", isir.pub_pubgroup_id.ToString());
                if (dv.Count == 1)
                {
                    dv.RowFilter = string.Concat("GroupFlag = true and pub_pubgroup_id = ", isir.pub_pubgroup_id.ToString());
                    isir = (DistributionMapping.InsertSetupInfoRow)dv[0].Row;
                }
            }

            DeleteRow(isir);

            if (_invalidMsg != string.Empty)
            {
                Reload();
            }

            e.Cancel = true;
        }

        #endregion

        #region Private Methods

        private void ReloadCombos()
        {
            _errorProvider.Clear();
            _btnAddScenario.Enabled = false;
            _btnAddGroup.Enabled = false;
            _btnAddPubLoc.Enabled = false;

            #region Reset Combo Boxes

            _dsDistMapping.PubInsertScenario_Active.Clear();
            _dsDistMapping.PubPubGroup_Active.Clear();
            _dsDistMapping.PubLoc_Active.Clear();
            _dsDistMapping.Pub_Active.Clear();
            ResetActiveLists();

            _cboScenario.DataSource = _dsDistMapping.PubInsertScenario_Active;
            _cboScenario.DisplayMember = "description";
            _cboScenario.ValueMember = "pub_insertscenario_id";

            _cboPublicationGroup.DataSource = _dsDistMapping.PubPubGroup_Active;
            _cboPublicationGroup.DisplayMember = "description";
            _cboPublicationGroup.ValueMember = "pub_pubgroup_id";

            _cboPublication.DataSource = _dsDistMapping.Pub_Active;
            _cboPublication.DisplayMember = "pub_nm";
            _cboPublication.ValueMember = "pub_id";

            _cboLocation.DataSource = _dsDistMapping.PubLoc_Active;
            _cboLocation.DisplayMember = "publoc_id";
            _cboLocation.ValueMember = "publoc_nm";

            _cboQuantityGroup.DataSource = _dsEstimate.pub_pubquantitytype;
            _cboQuantityGroup.DisplayMember = "description";
            _cboQuantityGroup.ValueMember = "pub_pubquantitytype_id";

            _dsDistMapping.PubInsertScenario_Active.DefaultView.Sort = "description";
            _dsDistMapping.PubPubGroup_Active.DefaultView.Sort = "sortorder";
            _dsDistMapping.Pub_Active.DefaultView.Sort = "pub_nm";
            _dsDistMapping.PubLoc_Active.DefaultView.Sort = "publoc_id";

            if ((_cboQuantityGroup.SelectedIndex == -1) && (_cboQuantityGroup.Items.Count > 0))
                _cboQuantityGroup.SelectedIndex = 0;
            if ((_cboScenario.SelectedIndex == -1) && (_cboScenario.Items.Count > 0))
                _cboScenario.SelectedIndex = 0;
            if ((_cboPublicationGroup.SelectedIndex == -1) && (_cboPublicationGroup.Items.Count > 0))
                _cboPublicationGroup.SelectedIndex = 0;
            if ((_cboPublication.SelectedIndex == -1) && (_cboPublication.Items.Count > 0))
            {
                _cboPublication.SelectedIndex = 0;
            }
            _cboPublication_SelectedIndexChanged(this, EventArgs.Empty);
            #endregion
        }

        private void CalcTotalQuantity()
        {
            Estimates.EstPackage_Poly_QuantitiesRow polyQtyRow = null;
            DataView polyQty = new DataView(_dsEstimate.EstPackage_Poly_Quantities);
            Estimates.EstPackage_Quantities_ByEstimateIDRow qtyRow = null;
            DataView qty = new DataView(_dsEstimate.EstPackage_Quantities_ByEstimateID);
            int total = 0;

            Clear_PubLoc_Quantities();

            foreach (DistributionMapping.InsertSetupInfoRow row in _dsDistMapping.InsertSetupInfo.Select("", "", DataViewRowState.CurrentRows))
            {
                qty.RowFilter = string.Concat("est_package_id = ", row.est_package_id);
                if (qty.Count == 0)
                {
                    qtyRow = _dsEstimate.EstPackage_Quantities_ByEstimateID.NewEstPackage_Quantities_ByEstimateIDRow();
                    qtyRow.BeginEdit();
                    qtyRow.est_package_id = row.est_package_id;
                    qtyRow.insertqty = 0;
                    qtyRow.polybagqty = 0;
                    qtyRow.EndEdit();
                    _dsEstimate.EstPackage_Quantities_ByEstimateID.AddEstPackage_Quantities_ByEstimateIDRow(qtyRow);
                }
                else
                {
                    qtyRow = (Estimates.EstPackage_Quantities_ByEstimateIDRow)qty[0].Row;
                }

                if (!row.IsquantityNull())
                {
                    total += row.quantity;
                    qtyRow.insertqty += row.quantity;
                }

                polyQty.RowFilter = string.Concat("est_package_id = ", row.est_package_id);
                if (polyQty.Count > 0)
                {
                    polyQtyRow = (Estimates.EstPackage_Poly_QuantitiesRow)polyQty[0].Row;
                    qtyRow.polybagqty = polyQtyRow.polybagqty;
                }
            }

            _txtTotalQuantity.Value = total;
        }

        private string CalcDisplaySortValue(int groupSortOrder, long groupID, string pubID, int publocID)
        {
            string sortVal = string.Empty;
            string sortOrder = groupSortOrder.ToString();
            string publoc = publocID.ToString();
            string grpID = groupID.ToString();
            char zero = '0';

            sortVal = string.Concat(sortOrder.PadLeft(10, zero), ".", grpID.PadLeft(20, zero), ".", pubID, ".", publoc.PadLeft(5, zero));

            return sortVal;
        }

        private void InitializePlusMinus()
        {
            foreach (DataGridViewRow rowView in _grdInsertSetup.Rows)
            {
                if (rowView.Visible)
                {
                    DistributionMapping.InsertSetupInfoRow isir = (DistributionMapping.InsertSetupInfoRow)((DataRowView)rowView.DataBoundItem).Row;
                    OptionalButtonCell bc = new OptionalButtonCell();
                    DataGridViewCheckBoxCell cbc = new DataGridViewCheckBoxCell();

                    if (isir.GroupFlag)
                    {
                        if (isir.display == 1)
                            bc.Value = "-";
                        else
                            bc.Value = "+";
                    }
                    else
                    {
                        bc.Display = false;
                    }

                    rowView.Cells[0] = bc;
                }
            }
        }

        private void DeleteRow(DistributionMapping.InsertSetupInfoRow isir)
        {
            DataView infoRows = new DataView(_dsDistMapping.InsertSetupInfo);
            DistributionMapping.InsertSetupInfoRow infoRow = null;
            DataView pkgs = new DataView(_dsEstimate.est_package);
            pkgs.RowFilter = string.Concat("est_package_id = ", isir.est_package_id.ToString());
            Estimates.est_packageRow package = (Estimates.est_packageRow)pkgs[0].Row;
            long pubGoupID = package.pub_pubgroup_id;

            if (isir.GroupFlag)
            {
                #region Remove Scenario Link
                if (isir.ScenarioFlag)
                {
                    pkgs.RowFilter = string.Concat("pub_insertscenario_id = ", package.pub_insertscenario_id.ToString());
                    pkgs.Sort = "est_package_id";

                    for (int i = pkgs.Count - 1; i >= 0; i--)
                    {
                        Estimates.est_packageRow pkg = (Estimates.est_packageRow)pkgs[i].Row;
                        pkg.Setpub_insertscenario_idNull();

                        infoRows.RowFilter = string.Concat("est_package_id = ", isir.est_package_id.ToString());

                        if (infoRows.Count > 0)
                        {
                            infoRow = (DistributionMapping.InsertSetupInfoRow)infoRows[0].Row;
                            infoRow.ScenarioFlag = false;
                        }
                    }
                }
                #endregion

                #region Delete Package, Maps to Components, Quantities
                foreach (Estimates.est_packagecomponentmappingRow pcMap in _dsEstimate.est_packagecomponentmapping.Select(string.Concat("est_package_id = ", package.est_package_id.ToString()), "", DataViewRowState.CurrentRows))
                {
                    pcMap.Delete();
                }

                foreach (Estimates.EstPackage_Quantities_ByEstimateIDRow pq in _dsEstimate.EstPackage_Quantities_ByEstimateID.Select(string.Concat("est_package_id = ", package.est_package_id.ToString()), "", DataViewRowState.CurrentRows))
                {
                    pq.Delete();
                }

                package.Delete();
                #endregion

                #region Clear Pub Group and Maps
                foreach (Estimates.pub_pubgroupRow grp in _dsEstimate.pub_pubgroup.Select(string.Concat("pub_pubgroup_id = ", pubGoupID.ToString()), "", DataViewRowState.CurrentRows))
                {
                    bool custom = grp.customgroupforpackage;
                    foreach (Estimates.pub_pubpubgroup_mapRow grpMap in _dsEstimate.pub_pubpubgroup_map.Select(string.Concat("pub_pubgroup_id = ", pubGoupID.ToString()), "", DataViewRowState.CurrentRows))
                    {
                        grpMap.Delete();
                        if (!custom)
                            grpMap.AcceptChanges();
                    }

                    grp.Delete();
                    if (!custom)
                    {
                        grp.AcceptChanges();
                    }
                }
                #endregion

                #region Delete Issue Dates and InsertSetupInfoRows for Pub-Locs
                foreach (DistributionMapping.InsertSetupInfoRow isiRow in _dsDistMapping.InsertSetupInfo.Select(string.Concat("pub_pubgroup_id = ", pubGoupID.ToString(), " and GroupFlag = false"), "", DataViewRowState.CurrentRows))
                {
                    if (!_overlappedRatemapIDs.Contains(isiRow.pub_pubrate_map_id))
                    {
                        foreach (Estimates.est_pubissuedatesRow idr in _dsEstimate.est_pubissuedates.Select(string.Concat("pub_pubrate_map_id = ", isiRow.pub_pubrate_map_id.ToString()), "", DataViewRowState.CurrentRows))
                        {
                            idr.Delete();
                        }
                    }

                    isiRow.Delete();
                }
                #endregion

                #region Delete InsertSetupInfoRow for Pub Group
                isir.Delete();
                #endregion
            }
            else
            {
                DataView grps = new DataView(_dsEstimate.pub_pubgroup);
                grps.RowFilter = string.Concat("pub_pubgroup_id = ", pubGoupID.ToString());
                Estimates.pub_pubgroupRow origGroup = (Estimates.pub_pubgroupRow)grps[0].Row;
                DataView gm = new DataView(_dsEstimate.pub_pubpubgroup_map);
                gm.RowFilter = string.Concat("pub_pubgroup_id = ", pubGoupID.ToString(), " and pub_pubrate_map_id = ", isir.pub_pubrate_map_id.ToString());

                if (origGroup.customgroupforpackage)
                {
                    gm[0].Delete();
                }
                else
                {
                    #region Remove Scenario Link
                    if (!package.Ispub_insertscenario_idNull())
                    {
                        pkgs.RowFilter = string.Concat("pub_insertscenario_id = ", package.pub_insertscenario_id.ToString());
                        pkgs.Sort = "est_package_id";

                        for (int i = pkgs.Count - 1; i >= 0; i--)
                        {
                            Estimates.est_packageRow pkg = (Estimates.est_packageRow)pkgs[i].Row;
                            pkg.Setpub_insertscenario_idNull();

                            infoRows.RowFilter = string.Concat("est_package_id = ", isir.est_package_id.ToString());

                            if (infoRows.Count > 0)
                            {
                                infoRow = (DistributionMapping.InsertSetupInfoRow)infoRows[0].Row;
                                infoRow.ScenarioFlag = false;
                            }
                        }
                    }
                    #endregion

                    #region Add New Custom Group
                    Estimates.pub_pubgroupRow newGroup = _dsEstimate.pub_pubgroup.Newpub_pubgroupRow();
                    newGroup.BeginEdit();
                    newGroup.description = (string.Concat("Custom Group: ", origGroup.description));
                    if (newGroup.description.Length > _dsEstimate.pub_pubgroup.descriptionColumn.MaxLength)
                        newGroup.description = newGroup.description.Substring(0, _dsEstimate.pub_pubgroup.descriptionColumn.MaxLength);
                    if (origGroup.IscommentsNull())
                        newGroup.SetcommentsNull();
                    else
                        newGroup.comments = origGroup.comments;
                    newGroup.effectivedate = Convert.ToDateTime("1/1/1900");
                    newGroup.sortorder = 1000000;
                    newGroup.customgroupforpackage = true;
                    newGroup.active = true;
                    newGroup.createdby = MainForm.AuthorizedUser.FormattedName;
                    newGroup.createddate = DateTime.Now;
                    newGroup.EndEdit();
                    _dsEstimate.pub_pubgroup.Addpub_pubgroupRow(newGroup);
                    #endregion

                    #region Add New Custom Group to Pub-Loc Maps
                    foreach (Estimates.pub_pubpubgroup_mapRow origMapRow in _dsEstimate.pub_pubpubgroup_map.Select(string.Concat("pub_pubgroup_id = ", pubGoupID.ToString(), " and pub_pubrate_map_id <> ", isir.pub_pubrate_map_id.ToString()), "", DataViewRowState.CurrentRows))
                    {
                        Estimates.pub_pubpubgroup_mapRow newMapRow = _dsEstimate.pub_pubpubgroup_map.Newpub_pubpubgroup_mapRow();
                        newMapRow.BeginEdit();
                        newMapRow.pub_pubgroup_id = newGroup.pub_pubgroup_id;
                        newMapRow.pub_pubrate_map_id = origMapRow.pub_pubrate_map_id;
                        newMapRow.createdby = MainForm.AuthorizedUser.FormattedName;
                        newMapRow.createddate = DateTime.Now;
                        newMapRow.EndEdit();
                        _dsEstimate.pub_pubpubgroup_map.Addpub_pubpubgroup_mapRow(newMapRow);
                    }
                    #endregion

                    #region Update Group ID on Package
                    package.pub_pubgroup_id = newGroup.pub_pubgroup_id;
                    package.description = newGroup.description;
                    #endregion

                    #region Delete Original Custom Group to Pub-Loc Maps
                    foreach (Estimates.pub_pubpubgroup_mapRow origMapRow in _dsEstimate.pub_pubpubgroup_map.Select(string.Concat("pub_pubgroup_id = ", pubGoupID.ToString()), "", DataViewRowState.CurrentRows))
                    {
                        origMapRow.Delete();
                        origMapRow.AcceptChanges();
                    }
                    #endregion

                    #region Delete Original Custom Group
                    origGroup.Delete();
                    origGroup.AcceptChanges();
                    #endregion
                }

                #region Update Package Quantity
                foreach (Estimates.EstPackage_Quantities_ByEstimateIDRow pq in _dsEstimate.EstPackage_Quantities_ByEstimateID.Select(string.Concat("est_package_id = ", package.est_package_id.ToString()), "", DataViewRowState.CurrentRows))
                {
                    pq.insertqty -= isir.quantity;
                }
                #endregion

                #region Delete Issue Date and InsertSetupInfoRow for Pub-Loc
                if (!_overlappedRatemapIDs.Contains(isir.pub_pubrate_map_id))
                {
                    foreach (Estimates.est_pubissuedatesRow idr in _dsEstimate.est_pubissuedates.Select(string.Concat("pub_pubrate_map_id = ", isir.pub_pubrate_map_id.ToString()), "", DataViewRowState.CurrentRows))
                    {
                        idr.Delete();
                    }
                }

                isir.Delete();
                #endregion
            }

            Dirty = true;

            ((uctDistributionMapping)this.Parent.Parent.Parent)._ucpMappings.Reload();
            ResetInsertInfo(_dsDistMapping);
            InitializePlusMinus();
            CalcTotalQuantity();
        }

        private bool AutoCorrectPubLocs(ref string errorText)
        {
            bool result = false;
            DataView scenarios = new DataView(_dsDistMapping.PubInsertScenario_Active);
            DistributionMapping.PubInsertScenario_ActiveRow sr = null;
            List<long> scenarioIDs = new List<long>();
            List<long> groupIDs = new List<long>();
            DataView groups = new DataView(_dsDistMapping.PubPubGroup_Active);
            DistributionMapping.PubPubGroup_ActiveRow pg = null;
            DataView pkgs = new DataView(_dsEstimate.est_package, "groupFlag = 1", "est_package_id", DataViewRowState.CurrentRows);
            Estimates.est_packageRow pkg = null;
            DataView issueDates = new DataView(_dsEstimate.est_pubissuedates);
            DataView pkgCompMaps = new DataView(_dsEstimate.est_packagecomponentmapping);
            //DateTime origRunDate = (DateTime)_dsEstimate.est_estimate[0]["rundate", DataRowVersion.Original];
            errorText = string.Empty;

            #region Build List of Active Scenarios & Seed List with Active Groups Already in Estimate
            foreach (Estimates.est_packageRow p in _dsEstimate.est_package.Select("groupFlag = 1 and pub_insertscenario_id is not null", "", DataViewRowState.CurrentRows))
            {
                if (!p.Ispub_insertscenario_idNull())
                {
                    scenarios.RowFilter = string.Concat("pub_insertscenario_id = ", p.pub_insertscenario_id.ToString());
                    if (scenarios.Count > 0)
                    {
                        if (!scenarioIDs.Contains(p.pub_insertscenario_id))
                            scenarioIDs.Add(p.pub_insertscenario_id);
                    }
                }

                if (!p.Ispub_pubgroup_idNull())
                {
                    groups.RowFilter = string.Concat("pub_pubgroup_id = ", p.pub_pubgroup_id.ToString());
                    if (groups.Count > 0)
                    {
                        if (!groupIDs.Contains(p.pub_pubgroup_id))
                            groupIDs.Add(p.pub_pubgroup_id);
                    }
                }
            }
            #endregion

            #region Check for Overlapping Groups by Scenario
            for (int j = 0; j < scenarioIDs.Count; j++)
            {
                #region Reset Datatables
                _dsDistMapping.pub_groupinsertscenario_map.Clear();
                _dsDistMapping.pub_insertscenario.Clear();
                _dsDistMapping.pub_pubrate_map_activate.Clear();
                _dsDistMapping.pub_pubrate.Clear();
                _dsDistMapping.pubpubpubgroup_map.Clear();
                _dsDistMapping.pubpubgroup.Clear();
                #endregion

                #region Get scenario info from DB for pub_insertscenario_id
                using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
                {
                    conn.Open();
                    using (pub_insertscenarioTableAdapter adapter = new pub_insertscenarioTableAdapter())
                    {
                        adapter.Connection = conn;
                        adapter.Fill(_dsDistMapping.pub_insertscenario, scenarioIDs[j]);
                    }

                    using (pub_groupinsertscenario_mapTableAdapter adapter = new pub_groupinsertscenario_mapTableAdapter())
                    {
                        adapter.Connection = conn;
                        adapter.Fill(_dsDistMapping.pub_groupinsertscenario_map, scenarioIDs[j]);
                    }

                    conn.Close();
                }
                #endregion

                #region Loop thru Groups checking for overlap
                foreach (DistributionMapping.pub_groupinsertscenario_mapRow row in _dsDistMapping.pub_groupinsertscenario_map.Select("", "", DataViewRowState.CurrentRows))
                {
                    groups.RowFilter = string.Concat("description = '", row.pubgroupdescription.Replace("'", "''"), "'");

                    if (groups.Count > 0)
                    {
                        pg = (DistributionMapping.PubPubGroup_ActiveRow)groups[0].Row;

                        if (!groupIDs.Contains(pg.pub_pubgroup_id))
                            groupIDs.Add(pg.pub_pubgroup_id);
                        else
                        {
                            pkgs.RowFilter = string.Concat("groupFlag = 1 and pub_pubgroup_id = ", pg.pub_pubgroup_id.ToString());
                            if (pkgs.Count > 0)
                            {
                                pkg = (Estimates.est_packageRow)pkgs[0].Row;
                                if (pkg.pub_insertscenario_id != scenarioIDs[j])
                                {
                                    scenarios.RowFilter = string.Concat("pub_insertscenario_id = ", scenarioIDs[j]);
                                    sr = (DistributionMapping.PubInsertScenario_ActiveRow)scenarios[0].Row;
                                    errorText = string.Concat(errorText, "\n\tGroup:", pkg.description, " from Scenario:",
                                        sr.description, " cannot be added because it already exists in this estimate.");
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            #endregion

            if (!string.IsNullOrEmpty(errorText))
            {
                errorText = string.Concat("Distribution Mapping Insert Setup Cannot be Auto-Corrected.", errorText);
                return result;
            }
            else
            {
                result = true;
                scenarios.RowFilter = string.Empty;
                groups.RowFilter = string.Empty;
                pkgs.RowFilter = "groupFlag = 1";
            }

            #region Clear Links to Inactive Scenarios
            foreach (Estimates.est_packageRow p in _dsEstimate.est_package.Select("pub_insertscenario_id is not null", "", DataViewRowState.CurrentRows))
            {
                scenarios.RowFilter = string.Concat("pub_insertscenario_id = ", p.pub_insertscenario_id.ToString());
                if (scenarios.Count == 0)
                    p.Setpub_insertscenario_idNull();
            }
            #endregion

            #region Remove Inactive Groups and Update Groups with new Effective Dates.
            for (int idx = pkgs.Count - 1; idx >= 0; idx--)
            {
                pkg = (Estimates.est_packageRow)pkgs[idx].Row;

                if (!pkg.pub_pubgroupRow.customgroupforpackage)
                {
                    groups.RowFilter = string.Concat("pub_pubgroup_id = ", pkg.pub_pubgroup_id.ToString());
                    
                    if (groups.Count == 0)
                    {
                        groups.RowFilter = string.Concat("description = '", pkg.pub_pubgroupRow.description, "'");

                        if (groups.Count > 0)
                        {
                            pg = (DistributionMapping.PubPubGroup_ActiveRow)groups[0].Row;
                            AddGroupToEstimate(pg.pub_pubgroup_id, pkg.pub_pubquantitytype_id);
                            pkg.pub_pubgroup_id = pg.pub_pubgroup_id;
                        }
                        else
                        {
                            #region Remove Package/Component Maps
                            pkgCompMaps.RowFilter = string.Concat("est_package_id = ", pkg.est_package_id.ToString());

                            for (int mIdx = pkgCompMaps.Count - 1; mIdx >= 0; mIdx--)
                            {
                                pkgCompMaps[mIdx].Delete();
                            }
                            #endregion

                            #region Remove Package
                            pkg.Delete();
                            #endregion
                        }
                    }
                }
            }
            #endregion

            #region Add Newly Effective Groups by Scenario

            for (int j = 0; j < scenarioIDs.Count; j++)
            {
                #region Reset Datatables
                _dsDistMapping.pub_groupinsertscenario_map.Clear();
                _dsDistMapping.pub_insertscenario.Clear();
                _dsDistMapping.pub_pubrate_map_activate.Clear();
                _dsDistMapping.pub_pubrate.Clear();
                _dsDistMapping.pubpubpubgroup_map.Clear();
                _dsDistMapping.pubpubgroup.Clear();
                #endregion

                #region Get scenario info from DB for pub_insertscenario_id
                using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
                {
                    conn.Open();
                    using (pub_insertscenarioTableAdapter adapter = new pub_insertscenarioTableAdapter())
                    {
                        adapter.Connection = conn;
                        adapter.Fill(_dsDistMapping.pub_insertscenario, scenarioIDs[j]);
                    }

                    using (pub_groupinsertscenario_mapTableAdapter adapter = new pub_groupinsertscenario_mapTableAdapter())
                    {
                        adapter.Connection = conn;
                        adapter.Fill(_dsDistMapping.pub_groupinsertscenario_map, scenarioIDs[j]);
                    }

                    conn.Close();
                }
                #endregion

                #region Loop thru Groups Adding New Packages as Needed
                foreach (DistributionMapping.pub_groupinsertscenario_mapRow row in _dsDistMapping.pub_groupinsertscenario_map.Select("", "", DataViewRowState.CurrentRows))
                {
                    groups.RowFilter = string.Concat("description = '", row.pubgroupdescription.Replace("'", "''"), "'");

                    if (groups.Count > 0)
                    {
                        pg = (DistributionMapping.PubPubGroup_ActiveRow)groups[0].Row;
                        AddGroupToEstimate(pg.pub_pubgroup_id, pkg.pub_pubquantitytype_id);
                        pkgs.RowFilter = string.Concat("pub_pubgroup_id = ", pg.pub_pubgroup_id.ToString());

                        if (pkgs.Count == 0)
                        {
                            #region Add new row to est_package
                            Estimates.est_packageRow pr = _dsEstimate.est_package.Newest_packageRow();
                            pr.BeginEdit();
                            pr.est_estimate_id = _dsEstimate.est_estimate[0].est_estimate_id;
                            pr.description = row.pubgroupdescription;
                            pr.soloquantity = 0;
                            pr.otherquantity = 0;
                            pr.pub_pubquantitytype_id = 1;
                            pr.pub_pubgroup_id = pg.pub_pubgroup_id;
                            pr.pub_insertscenario_id = scenarioIDs[j];
                            pr.createdby = MainForm.AuthorizedUser.FormattedName;
                            pr.createddate = DateTime.Now;
                            pr.EndEdit();
                            _dsEstimate.est_package.Addest_packageRow(pr);
                            #endregion
                        }
                    }
                }
                #endregion

            }

            #endregion

            return result;
        }

        private void AddGroupToEstimate(long pubgroup_id, int qtyType)
        {
            Estimates.pub_pubgroupRow gr = null;
            DataView est_groups = new DataView(_dsEstimate.pub_pubgroup);
            Estimates.pub_pubpubgroup_mapRow mr = null;
            DataView est_groupmaps = new DataView(_dsEstimate.pub_pubpubgroup_map);
            est_groups.RowFilter = string.Concat("pub_pubgroup_id = ", pubgroup_id.ToString());

            #region Calculate InsertDate and DOW
            DateTime insertDate = _dsEstimate.est_estimate[0].rundate;
            int runDOW = ((int)_dsEstimate.est_estimate[0].rundate.DayOfWeek) + 1;
            int insDOW = _dsEstimate.est_assemdistriboptions[0].insertdow;

            if (runDOW < insDOW)
            {
                insertDate = insertDate.AddDays(insDOW - runDOW - 7);
            }
            else if (runDOW > insDOW)
            {
                insertDate = insertDate.AddDays(insDOW - runDOW);
            }
            #endregion

            DataView group = new DataView(_dsDistMapping.PubPubGroup_Active);
            group.RowFilter = string.Concat("pub_pubgroup_id = ", pubgroup_id.ToString());

            #region Reset Datatables
            _dsDistMapping.pub_pubrate.Clear();
            _dsDistMapping.pubpubpubgroup_map.Clear();
            _dsDistMapping.pubpubgroup.Clear();
            #endregion

            #region Get Pubs and PubLocs from DB for group_id
            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();
                using (pub_pubrate_mapTableAdapter adapter = new pub_pubrate_mapTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsDistMapping.pub_pubrate_map, _dsEstimate.est_estimate[0].rundate, pubgroup_id);
                }

                using (pubpubgroupTableAdapter adapter = new pubpubgroupTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsDistMapping.pubpubgroup, pubgroup_id);
                }

                using (pubpubpubgroup_mapTableAdapter adapter = new pubpubpubgroup_mapTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsDistMapping.pubpubpubgroup_map, pubgroup_id);
                }

                conn.Close();
            }
            #endregion

            #region Add Group to Estimate

            if (est_groups.Count == 0)
            {
                #region Add new row to pub_pubgroup
                gr = _dsEstimate.pub_pubgroup.Newpub_pubgroupRow();
                gr.BeginEdit();
                gr.pub_pubgroup_id = _dsDistMapping.pubpubgroup[0].pub_pubgroup_id;
                gr.description = _dsDistMapping.pubpubgroup[0].description;
                gr.effectivedate = _dsDistMapping.pubpubgroup[0].effectivedate;
                gr.sortorder = _dsDistMapping.pubpubgroup[0].sortorder;
                gr.customgroupforpackage = _dsDistMapping.pubpubgroup[0].customgroupforpackage;
                gr.active = _dsDistMapping.pubpubgroup[0].active;
                gr.createdby = _dsDistMapping.pubpubgroup[0].createdby;
                gr.createddate = _dsDistMapping.pubpubgroup[0].createddate;
                if (_dsDistMapping.pubpubgroup[0].IsmodifiedbyNull())
                    gr.SetmodifiedbyNull();
                else
                    gr.modifiedby = _dsDistMapping.pubpubgroup[0].modifiedby;
                if (_dsDistMapping.pubpubgroup[0].IsmodifieddateNull())
                    gr.SetmodifieddateNull();
                else
                    gr.modifieddate = _dsDistMapping.pubpubgroup[0].modifieddate;
                gr.EndEdit();
                _dsEstimate.pub_pubgroup.Addpub_pubgroupRow(gr);
                gr.AcceptChanges();
                #endregion
            }
            else
            {
                gr = (Estimates.pub_pubgroupRow)est_groups[0].Row;
            }

            // foreach publoc in group
            foreach (DistributionMapping.pub_pubrate_mapRow row in _dsDistMapping.pub_pubrate_map.Select("", "", DataViewRowState.CurrentRows))
            {
                #region Add new row to pub_pubpubgroupmap
                est_groupmaps.RowFilter = string.Concat("pub_pubrate_map_id = ", row.pub_pubrate_map_id.ToString(),
                    " and pub_pubgroup_id = ", gr.pub_pubgroup_id.ToString());

                if (est_groupmaps.Count == 0)
                {
                    mr = _dsEstimate.pub_pubpubgroup_map.Newpub_pubpubgroup_mapRow();
                    mr.BeginEdit();
                    mr.pub_pubgroup_id = gr.pub_pubgroup_id;
                    mr.pub_pubrate_map_id = row.pub_pubrate_map_id;
                    mr.createdby = MainForm.AuthorizedUser.FormattedName;
                    mr.createddate = DateTime.Now;
                    mr.EndEdit();
                    _dsEstimate.pub_pubpubgroup_map.Addpub_pubpubgroup_mapRow(mr);
                    mr.AcceptChanges();
                }
                #endregion

                #region Add new row to est_pubinsertdates
                Estimates.est_pubissuedatesRow ir = null;
                DataView id = new DataView(_dsEstimate.est_pubissuedates);
                id.RowFilter = string.Concat("est_estimate_id = ", _dsEstimate.est_estimate[0].est_estimate_id.ToString(),
                    " and pub_pubrate_map_id = ", row.pub_pubrate_map_id.ToString());
                if (id.Count == 0)
                {
                    ir = _dsEstimate.est_pubissuedates.Newest_pubissuedatesRow();
                    ir.BeginEdit();
                    ir.est_estimate_id = _dsEstimate.est_estimate[0].est_estimate_id;
                    ir.pub_pubrate_map_id = row.pub_pubrate_map_id;
                    ir.issuedow = _dsDistMapping.PubIssueInfo[0].issuedow;
                    ir.issuedate = _dsDistMapping.PubIssueInfo[0].issuedate;
                    ir._override = false;
                    ir.createdby = MainForm.AuthorizedUser.FormattedName;
                    ir.createddate = DateTime.Now;
                    ir.EndEdit();
                    _dsEstimate.est_pubissuedates.Addest_pubissuedatesRow(ir);

                    Dirty = true;
                }
                #endregion
            }

            #endregion
        }

        private void HighlightOverlaps()
        {
            for (int rowIdx = 0; rowIdx < _grdInsertSetup.RowCount; rowIdx++)
            {
                DistributionMapping.InsertSetupInfoRow isir = (DistributionMapping.InsertSetupInfoRow)((DataRowView)_grdInsertSetup.Rows[rowIdx].DataBoundItem).Row;
                if (!isir.Ispub_pubrate_map_idNull())
                {
                    if (_overlappedRatemapIDs.Contains(isir.pub_pubrate_map_id))
                    {
                        _grdInsertSetup.Rows[rowIdx].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
            }
        }

        private void ResetInsertInfo(DistributionMapping dsDistMapping)
        {
            DataView pubs = new DataView(_dsEstimate.pub_pubgroup);
            Estimates.pub_pubgroupRow pub = null;
            DataView publocmaps = new DataView(_dsEstimate.pub_pubpubgroup_map);
            Estimates.pub_pubpubgroup_mapRow publocmap = null;
            DataView publocs = new DataView(dsDistMapping.PubLoc_Active);
            DistributionMapping.PubLoc_ActiveRow publoc = null;
            DataView issuedates = new DataView(_dsEstimate.est_pubissuedates);
            Estimates.est_pubissuedatesRow issuedate = null;
            DistributionMapping.PubIssueInfoRow issueInfo = null;
            DataView qty = new DataView(_dsEstimate.pub_pubquantitytype);
            Estimates.pub_pubquantitytypeRow qtyType = null;

            dsDistMapping.InsertSetupInfo.Clear();

            foreach (Estimates.est_packageRow pkg in _dsEstimate.est_package.Select("", "", DataViewRowState.CurrentRows))
            {
                if (!pkg.Ispub_pubgroup_idNull())
                {
                    pubs.RowFilter = string.Concat("pub_pubgroup_id = ", pkg.pub_pubgroup_id.ToString());
                    pub = (Estimates.pub_pubgroupRow)pubs[0].Row;

                    #region Add new row to DistributionMapping table for Group
                    DistributionMapping.InsertSetupInfoRow isir = dsDistMapping.InsertSetupInfo.NewInsertSetupInfoRow();
                    isir.BeginEdit();
                    isir.est_package_id = pkg.est_package_id;
                    isir.PackageName = pkg.description;
                    isir.pub_pubquantitytype_id = pkg.pub_pubquantitytype_id;
                    qty.RowFilter = string.Concat("pub_pubquantitytype_id = ", pkg.pub_pubquantitytype_id);
                    if (qty.Count > 0)
                    {
                        qtyType = (Estimates.pub_pubquantitytypeRow)qty[0].Row;
                        isir.pub_pubquantitytype = qtyType.description;
                    }
                    isir.pub_pubgroup_id = pkg.pub_pubgroup_id;
                    isir.GroupName = pkg.description;
                    isir.GroupActiveFlag = pub.active;
                    isir.GroupEffectiveDate = pub.effectivedate;
                    isir.sortorder = pub.sortorder;
                    isir.customgroupforpackage = pub.customgroupforpackage;
                    isir.Setpub_pubrate_map_idNull();
                    isir._override = false;
                    //isir.quantity = 0;
                    isir.GridDescription = pkg.description.Trim();
                    isir.ScenarioFlag = !pkg.Ispub_insertscenario_idNull();
                    isir.GroupFlag = true;
                    isir.display = 1;
                    isir.displaySortOrder = CalcDisplaySortValue(pub.sortorder, pkg.pub_pubgroup_id, "   ", 0);
                    isir.EndEdit();
                    dsDistMapping.InsertSetupInfo.AddInsertSetupInfoRow(isir);
                    #endregion

                    publocmaps.RowFilter = string.Concat("pub_pubgroup_id = ", pkg.pub_pubgroup_id.ToString());

                    foreach (DataRowView rv in publocmaps)
                    {
                        publocmap = (Estimates.pub_pubpubgroup_mapRow)rv.Row;
                        publocs.RowFilter = string.Concat("pub_pubrate_map_id = ", publocmap.pub_pubrate_map_id.ToString());
                        publoc = (DistributionMapping.PubLoc_ActiveRow)publocs[0].Row;
                        issuedates.RowFilter = string.Concat("pub_pubrate_map_id = ", publocmap.pub_pubrate_map_id.ToString());
                        issuedate = (Estimates.est_pubissuedatesRow)issuedates[0].Row;

                        #region Get info from DB by Pub and PubLoc
                        using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
                        {
                            conn.Open();

                            using (PubIssueInfoTableAdapter adapter = new PubIssueInfoTableAdapter())
                            {
                                DateTime insertDate = _dsEstimate.est_estimate[0].rundate;
                                int runDOW = ((int)_dsEstimate.est_estimate[0].rundate.DayOfWeek) + 1;
                                int insDOW = _dsEstimate.est_assemdistriboptions[0].insertdow;

                                if (runDOW < insDOW)
                                {
                                    insertDate = insertDate.AddDays(insDOW - runDOW - 7);
                                }
                                else if (runDOW > insDOW)
                                {
                                    insertDate = insertDate.AddDays(insDOW - runDOW);
                                }

                                adapter.Connection = conn;
                                adapter.Fill(dsDistMapping.PubIssueInfo, publocmap.pub_pubrate_map_id,
                                    pkg.pub_pubquantitytype_id, insertDate, _dsEstimate.est_assemdistriboptions[0].inserttime);
                            }

                            issueInfo = (DistributionMapping.PubIssueInfoRow)dsDistMapping.PubIssueInfo[0];

                            conn.Close();
                        }
                        #endregion

                        #region Add new row to DistributionMapping table for PubLoc
                        isir = dsDistMapping.InsertSetupInfo.NewInsertSetupInfoRow();
                        isir.BeginEdit();
                        isir.est_package_id = pkg.est_package_id;
                        isir.PackageName = pkg.description;
                        isir.pub_pubquantitytype_id = pkg.pub_pubquantitytype_id;
                        isir.pub_pubgroup_id = pkg.pub_pubgroup_id;
                        isir.GroupName = pkg.description;
                        isir.GroupActiveFlag = pub.active;
                        isir.GroupEffectiveDate = pub.effectivedate;
                        isir.sortorder = pub.sortorder;
                        isir.customgroupforpackage = pub.customgroupforpackage;
                        isir.pub_pubrate_map_id = publocmap.pub_pubrate_map_id;
                        isir.pub_nm = pub.description;
                        isir.publoc_nm = publoc.publoc_nm;
                        isir._override = issuedate._override;
                        if (!issuedate._override)
                        {
                            if (issuedate.issuedow != issueInfo.issuedow)
                            {
                                Dirty = true;
                                issuedate.issuedow = issueInfo.issuedow;
                            }
                            if (issuedate.issuedate != issueInfo.issuedate)
                            {
                                Dirty = true;
                                issuedate.issuedate = issueInfo.issuedate;
                            }
                        }
                        isir.issuedow = issuedate.issuedow;
                        isir.DisplayDOW = DOW_Text(issuedate.issuedow);
                        isir.issuedate = issuedate.issuedate;
                        isir.quantity = dsDistMapping.PubIssueInfo[0].quantity;
                        isir.GridDescription = string.Concat("        ", issueInfo.pub_nm.Trim(), " - (", publoc.publoc_id.ToString(), ")");
                        isir.ScenarioFlag = false;
                        isir.GroupFlag = false;
                        isir.display = 1;
                        isir.displaySortOrder = CalcDisplaySortValue(pub.sortorder, pkg.pub_pubgroup_id, publoc.pub_id, publoc.publoc_id);
                        isir.EndEdit();
                        dsDistMapping.InsertSetupInfo.AddInsertSetupInfoRow(isir);
                        #endregion
                    }
                }
            }
        }

        private void ResetDefaultIssueDate(int gridRowIndex)
        {
            DistributionMapping.PubIssueInfoRow issueInfo = null;
            DistributionMapping.InsertSetupInfoRow isir = (DistributionMapping.InsertSetupInfoRow)((DataRowView)_grdInsertSetup.Rows[gridRowIndex].DataBoundItem).Row;

            #region Get info from DB by Pub and PubLoc
            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                using (PubIssueInfoTableAdapter adapter = new PubIssueInfoTableAdapter())
                {
                    DateTime insertDate = _dsEstimate.est_estimate[0].rundate;
                    int runDOW = ((int)_dsEstimate.est_estimate[0].rundate.DayOfWeek) + 1;
                    int insDOW = _dsEstimate.est_assemdistriboptions[0].insertdow;

                    if (runDOW < insDOW)
                    {
                        insertDate = insertDate.AddDays(insDOW - runDOW - 7);
                    }
                    else if (runDOW > insDOW)
                    {
                        insertDate = insertDate.AddDays(insDOW - runDOW);
                    }

                    adapter.Connection = conn;
                    adapter.Fill(_dsDistMapping.PubIssueInfo, isir.pub_pubrate_map_id,
                        isir.pub_pubquantitytype_id, insertDate, _dsEstimate.est_assemdistriboptions[0].inserttime);
                }

                issueInfo = (DistributionMapping.PubIssueInfoRow)_dsDistMapping.PubIssueInfo[0];

                conn.Close();
            }
            #endregion

            isir.quantity = issueInfo.quantity;
            isir.issuedate = issueInfo.issuedate;
            isir.issuedow = issueInfo.issuedow;
            isir.DisplayDOW = DOW_Text(isir.issuedow);
        }

        private void ResetIssueDate(DistributionMapping.InsertSetupInfoRow isir)
        {
            DistributionMapping.PubIssueInfoRow issueInfo = null;

            #region Get info from DB by Pub and PubLoc
            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                using (PubIssueInfoTableAdapter adapter = new PubIssueInfoTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsDistMapping.PubIssueInfo, isir.pub_pubrate_map_id,
                        isir.pub_pubquantitytype_id, isir.issuedate, _dsEstimate.est_assemdistriboptions[0].inserttime);
                }

                issueInfo = (DistributionMapping.PubIssueInfoRow)_dsDistMapping.PubIssueInfo[0];

                conn.Close();
            }
            #endregion

            isir.quantity = issueInfo.quantity;
            isir.issuedow = DOW_Integer(isir.issuedate);
            isir.DisplayDOW = DOW_Text(isir.issuedow);
        }

        private void ResetActiveLists()
        {
            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

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
                #endregion

                conn.Close();
            }
        }

        private void SetAccessLevel(bool enable)
        {
            _btnAddScenario.Enabled = enable;
            _btnAddGroup.Enabled = enable;
            _btnAddPubLoc.Enabled = enable;
            _grdInsertSetup.AllowUserToDeleteRows = enable;
        }

        private void Clear_PubLoc_Quantities()
        {
            DataView pqv = new DataView(_dsEstimate.EstPackage_Poly_Quantities);
            Estimates.EstPackage_Poly_QuantitiesRow pqr = null;
 
            _dsEstimate.EstPackage_Quantities_ByEstimateID.Clear();

            #region Refresh EstPackage_Poly_Quantities
            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();
                using (EstPackage_Poly_QuantitiesTableAdapter adapter = new EstPackage_Poly_QuantitiesTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsEstimate.EstPackage_Poly_Quantities, _dsEstimate.est_estimate[0].est_estimate_id);
                }
                conn.Close();
            }
            #endregion

            foreach (Estimates.est_packageRow pkg in _dsEstimate.est_package.Select("", "", DataViewRowState.CurrentRows))
            {
                if (pkg.Ispub_pubgroup_idNull())
                {
                    Estimates.EstPackage_Quantities_ByEstimateIDRow newQty = _dsEstimate.EstPackage_Quantities_ByEstimateID.NewEstPackage_Quantities_ByEstimateIDRow();
                    newQty.BeginEdit();
                    newQty.est_package_id = pkg.est_package_id;
                    pqv.RowFilter = string.Concat("est_package_id = ", pkg.est_package_id.ToString());
                    if (pqv.Count > 0)
                    {
                        pqr = (Estimates.EstPackage_Poly_QuantitiesRow)pqv[0].Row;
                        newQty.insertqty = pqr.insertqty;
                        newQty.polybagqty = pqr.polybagqty;
                    }
                    else
                    {
                        newQty.insertqty = 0;
                        newQty.polybagqty = 0;
                    }
                    newQty.EndEdit();
                    _dsEstimate.EstPackage_Quantities_ByEstimateID.AddEstPackage_Quantities_ByEstimateIDRow(newQty);
                }
            }
        }

        private int DOW_Integer(DateTime date)
        {
            DayOfWeek day = date.DayOfWeek;
            int dow = 0;

            switch (day.ToString().ToUpper())
            {
                case "MONDAY":
                    dow = (int)InsertDOW.Monday;
                    break;
                case "TUESDAY":
                    dow = (int)InsertDOW.Tuesday;
                    break;
                case "WEDNESDAY":
                    dow = (int)InsertDOW.Wednesday;
                    break;
                case "THURSDAY":
                    dow = (int)InsertDOW.Thursday;
                    break;
                case "FRIDAY":
                    dow = (int)InsertDOW.Friday;
                    break;
                case "SATURDAY":
                    dow = (int)InsertDOW.Saturday;
                    break;
                default:
                    dow = (int)InsertDOW.Sunday;
                    break;
            }

            return dow;
        }

        private string DOW_Text(int dayOfWeek)
        {
            string dowText = string.Empty;

            switch (dayOfWeek)
            {
                case (int)InsertDOW.Sunday:
                    dowText = InsertDOW.Sunday.ToString();
                    break;
                case (int)InsertDOW.Monday:
                    dowText = InsertDOW.Monday.ToString();
                    break;
                case (int)InsertDOW.Tuesday:
                    dowText = InsertDOW.Tuesday.ToString();
                    break;
                case (int)InsertDOW.Wednesday:
                    dowText = InsertDOW.Wednesday.ToString();
                    break;
                case (int)InsertDOW.Thursday:
                    dowText = InsertDOW.Thursday.ToString();
                    break;
                case (int)InsertDOW.Friday:
                    dowText = InsertDOW.Friday.ToString();
                    break;
                case (int)InsertDOW.Saturday:
                    dowText = InsertDOW.Saturday.ToString();
                    break;
                default:
                    dowText = string.Concat("Invalid Day of Week: ", dayOfWeek.ToString());
                    break;
            }

            return dowText;
        }

        private void UpdateOverrides()
        {
            _grdInsertSetup.EndEdit();

            DataView issueDates = new DataView(_dsEstimate.est_pubissuedates);
            Estimates.est_pubissuedatesRow issueDate = null;

            foreach (DistributionMapping.InsertSetupInfoRow isir in _dsDistMapping.InsertSetupInfo)
            {
                if (!isir.GroupFlag)
                {
                    issueDates.RowFilter = string.Concat("pub_pubrate_map_id = ", isir.pub_pubrate_map_id.ToString());
                    issueDate = (Estimates.est_pubissuedatesRow)issueDates[0].Row;

                    if ((issueDate._override != isir._override) || (issueDate.issuedate != isir.issuedate))
                    {
                        isir.issuedow = DOW_Integer(isir.issuedate);
                        isir.DisplayDOW = DOW_Text(isir.issuedow);

                        issueDate._override = isir._override;
                        issueDate.issuedate = isir.issuedate;
                        issueDate.issuedow = isir.issuedow;
                        issueDate.modifiedby = MainForm.AuthorizedUser.FormattedName;
                        issueDate.modifieddate = DateTime.Now;
                    }
                }
            }
        }

        private void SetTotalsLocation()
        {
            _txtTotalQuantity.Size = new Size(_grdInsertSetup.Columns[4].Width, _txtTotalQuantity.Size.Height);
            _txtTotalQuantity.Location = new Point(_grdInsertSetup.Location.X + _grdInsertSetup.Columns[0].Width + _grdInsertSetup.Columns[1].Width + _grdInsertSetup.Columns[2].Width + _grdInsertSetup.Columns[3].Width + 2, _grdInsertSetup.Location.Y + _grdInsertSetup.Size.Height + 4);

            _lblTotal.Location = new Point(_txtTotalQuantity.Location.X - _lblTotal.Width - 4, _grdInsertSetup.Location.Y + _grdInsertSetup.Size.Height + 6);
        }

        #endregion

    }
}