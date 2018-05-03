
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;

using Microsoft.Practices.EnterpriseLibrary.Data;

using CatalogEstimating;
using CatalogEstimating.Properties;
using CatalogEstimating.CustomControls;
using CatalogEstimating.UserControls.Estimate;
using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.EstimateSearchTableAdapters;
using CatalogEstimating.Datasets.DatabasesTableAdapters;
using CatalogEstimating.Datasets.EstimatesTableAdapters;
using CatalogEstimating.Datasets.PublicationsTableAdapters;


namespace CatalogEstimating.UserControls.Main
{


    public partial class ucpEstimateUpload : CatalogEstimating.UserControlPanel
    {
        private enum EstimateUploadStatus
        {
            Valid,
            Warning,
            UploadError,
            CloneError,
            UploadComplete
        }

        private DataTable _dtEstimateStatus = new DataTable();
        private List<long> _uploadEstimates = new List<long>();

        public ucpEstimateUpload()
        {
            InitializeComponent();
            Name = "Estimate Upload";

            _dtEstimateStatus.Columns.Add("EST_Estimate_ID");
            _dtEstimateStatus.Columns.Add("Parent_ID");
            _dtEstimateStatus.Columns.Add("StatusCode");
            _dtEstimateStatus.Columns.Add("ErrorMessage");
        }

        public override void Reload()
        {
            base.Reload();
        }

        public override ToolStrip Toolbar
        {
            get { return _toolStrip; }
        }

        private void _btnValidate_Click(object sender, EventArgs e)
        {
            ValidateEstimates();
        }

        public void ValidateEstimates()
        {
            _gridUpload.Rows.Clear();
            _dtEstimateStatus.Rows.Clear();
            _progressBar.Value = 0;
            _lblUploadStatus.Text = string.Empty;

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                foreach (long estimateID in _uploadEstimates)
                {
                    EstimateUploadStatus curEstimateUploadStatus = EstimateUploadStatus.Valid;
                    long? curParentID = null;

                    SqlCommand cmd = new SqlCommand("EstEstimate_Upload_Search", conn);
                    cmd.CommandTimeout = 7200;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EST_Estimate_ID", estimateID);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (!dr.IsDBNull(dr.GetOrdinal("Parent_ID")))
                            curParentID = dr.GetInt64(dr.GetOrdinal("Parent_ID"));

                        _gridUpload.Rows.Add(dr["EST_Estimate_ID"].ToString(), dr["Parent_ID"].ToString(), dr.GetDateTime(dr.GetOrdinal("EST_RunDate")).ToShortDateString(), dr["EST_Description"].ToString(), dr["ESTc_Description"].ToString(), dr["AdNumber"].ToString(), dr["StatusCode"].ToString(), dr["Validation_msg"].ToString());
                        switch (dr.GetInt32(dr.GetOrdinal("StatusCode")))
                        {
                            case 100:
                                _gridUpload.Rows[_gridUpload.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                                _gridUpload.Rows[_gridUpload.Rows.Count - 1].DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LimeGreen;
                                break;
                            case 200:
                                _gridUpload.Rows[_gridUpload.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.LemonChiffon;
                                _gridUpload.Rows[_gridUpload.Rows.Count - 1].DefaultCellStyle.SelectionBackColor = System.Drawing.Color.PaleGoldenrod;
                                _gridUpload.Rows[_gridUpload.Rows.Count - 1].DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
                                if (curEstimateUploadStatus == EstimateUploadStatus.Valid)
                                    curEstimateUploadStatus = EstimateUploadStatus.Warning;
                                break;
                            default:
                                _gridUpload.Rows[_gridUpload.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Salmon;
                                _gridUpload.Rows[_gridUpload.Rows.Count - 1].DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Coral;
                                curEstimateUploadStatus = EstimateUploadStatus.UploadError;
                                break;
                        }
                    }
                    dr.Close();

                    _dtEstimateStatus.Rows.Add(estimateID, curParentID, curEstimateUploadStatus, string.Empty);
                }

                conn.Close();
            }
        }

        public bool CloneUpload(long estimateID, out string ErrorDescription)
        {
            CatalogEstimating.CopyEstimate cloneEstimate = new CopyEstimate();

            cloneEstimate.LoadData(estimateID);
            cloneEstimate.LoadDetails();

            cloneEstimate.StatusID = 2;
            cloneEstimate.UploadDate = System.DateTime.Now;
            cloneEstimate.ParentID = estimateID;

            return cloneEstimate.SaveData(MainForm.WorkingDatabase, out ErrorDescription);
        }

        private void _btnUpload_Click(object sender, EventArgs e)
        {
            _progressBar.Value = 5;
            _lblUploadStatus.Text = "Beginning Upload Process";
            this.Refresh();

            this.Cursor = Cursors.WaitCursor;

            // Create the Estimate Clones, if there are errors update the grid and prevent the estimate from being uploaded.
            _progressBar.Value = 10;
            _lblUploadStatus.Text = "Creating Uploaded Estimate records";
            this.Refresh();

            PerformanceLog.Start("Upload - Click");

            PerformanceLog.Start("Upload - Create Clones");
            foreach (DataRow dr in _dtEstimateStatus.Rows)
            {
                if (dr["StatusCode"].ToString() == EstimateUploadStatus.Valid.ToString() || dr["StatusCode"].ToString() == EstimateUploadStatus.Warning.ToString())
                {
                    string errorDescription;
                    if (!CloneUpload(Convert.ToInt64(dr["EST_Estimate_ID"]), out errorDescription))
                    {
                        dr["StatusCode"] = EstimateUploadStatus.CloneError;
                        dr["ErrorMessage"] = errorDescription;
                    }
                }
            }
            PerformanceLog.End("Upload - Create Clones");

            UpdateGridwithCloneErrors();

            // Build the list of estimates that need to be uploaded
            bool bUploadNeeded = false;
            StringWriter writerString = new StringWriter();
            XmlTextWriter writerXml = new XmlTextWriter(writerString);
            
            writerXml.WriteStartElement("root");
            foreach (DataRow dr in _dtEstimateStatus.Rows)
            {
                if (dr["StatusCode"].ToString() == EstimateUploadStatus.Valid.ToString() || dr["StatusCode"].ToString() == EstimateUploadStatus.Warning.ToString())
                {
                    bUploadNeeded = true;
                    writerXml.WriteStartElement("estimate");
                    writerXml.WriteAttributeString("id", dr["EST_Estimate_ID"].ToString());
                    writerXml.WriteEndElement();
                }
            }
            writerXml.WriteEndElement();

            string strDialogMessage = null;
            string strDialogTitle = null;
            if (bUploadNeeded)
            {
                _progressBar.Value = 15;
                _lblUploadStatus.Text = "Generating Persistent Direct Mail Cost Data";
                this.Refresh();

                PerformanceLog.Start("Upload - Compute Mail Costs");
                ComputeDirectMailCosts(writerString.ToString());
                PerformanceLog.End("Upload - Compute Mail Costs");

                _progressBar.Value = 20;
                _lblUploadStatus.Text = "Generating Persistent Ad Pub Cost Data";
                this.Refresh();
                PerformanceLog.Start("Upload - Compute Ad Pub Costs");
                ComputeAdPubCosts(writerString.ToString());
                PerformanceLog.End("Upload - Compute Ad Pub Costs");

                _progressBar.Value = 25;
                _lblUploadStatus.Text = "Generating Persistent Vendor - Cost Code Data";
                this.Refresh();
                PerformanceLog.Start("Upload - Compute Vendor Cost Code");
                ComputeVendorCostCode(writerString.ToString());
                PerformanceLog.End("Upload - Compute Vendor Cost Code");

                _progressBar.Value = 30;
                _lblUploadStatus.Text = "Generating Persistent Polybag - Cost Code Data";
                this.Refresh();
                PerformanceLog.Start("Upload - Compute Polybag Cost Code");
                ComputePolybagCostCode(writerString.ToString());
                PerformanceLog.End("Upload - Compute Polybag Cost Code");

                _progressBar.Value = 35;
                _lblUploadStatus.Text = "Generating Persistent Publisher - Cost Code Data";
                this.Refresh();
                PerformanceLog.Start("Upload - Compute Pub Cost Code");
                ComputePubCostCode(writerString.ToString());
                PerformanceLog.End("Upload - Compute Pub Cost Code");

                _progressBar.Value = 40;
                _lblUploadStatus.Text = "Gathering Pre-Upload Financial Change Data";
                this.Refresh();
                PerformanceLog.Start("Upload - Gather Pre-Upload Financial Change Report");
                DataTable preUploadFinancialChangeData = FinancialChangeReport_UploadData(writerString.ToString());
                PerformanceLog.End("Upload - Gather Pre-Upload Financial Change Report");

                _progressBar.Value = 45;
                _lblUploadStatus.Text = "Gathering Pre-Upload Pub Change Data";
                this.Refresh();
                PerformanceLog.Start("Upload - Gather Pre-Upload Pub Change Report");
                DataTable preUploadPubChangeData = PubChangeReport_UploadData(writerString.ToString());
                PerformanceLog.End("Upload - Gather Pre-Upload Pub Change Report");

                _progressBar.Value = 50;
                _lblUploadStatus.Text = "Uploading Data";
                this.Refresh();
                PerformanceLog.Start("Upload - Perform Upload");
                PerformUpload(writerString.ToString());
                PerformanceLog.End("Upload - Perform Upload");

                _progressBar.Value = 55;
                _lblUploadStatus.Text = "Gathering Post-Upload Financial Change Data";
                this.Refresh();
                PerformanceLog.Start("Upload - Gather Post-Upload Financial Change Report");
                DataTable postUploadFinancialChangeData = FinancialChangeReport_UploadData(writerString.ToString());
                PerformanceLog.End("Upload - Gather Post-Upload Financial Change Report");

                _progressBar.Value = 60;
                _lblUploadStatus.Text = "Gathering Post-Upload Pub Change Data";
                this.Refresh();
                PerformanceLog.Start("Upload - Gather Post-Upload Pub Change Report");
                DataTable postUploadPubChangeData = PubChangeReport_UploadData(writerString.ToString());
                PerformanceLog.End("Upload - Gather Post-Upload Pub Change Report");

                _progressBar.Value = 70;
                _lblUploadStatus.Text = "Generating Financial Change Report";
                this.Refresh();
                PerformanceLog.Start("Upload - Generate Financial Change Report");
                GenerateFinancialChangeReport(preUploadFinancialChangeData, postUploadFinancialChangeData);
                PerformanceLog.End("Upload - Generate Financial Change Report");

                _progressBar.Value = 80;
                _lblUploadStatus.Text = "Generating Pub Change Report";
                this.Refresh();
                PerformanceLog.Start("Upload - Generate Pub Change Report");
                GeneratePubChangesReport(preUploadPubChangeData, postUploadPubChangeData);
                PerformanceLog.End("Upload - Generate Pub Change Report");

                _progressBar.Value = 90;
                _lblUploadStatus.Text = "Updating Grid Status";
                this.Refresh();
                UpdateGridwithUploadStatus();

                this.Cursor = Cursors.Default;

                _progressBar.Value = 100;
                _lblUploadStatus.Text = "Upload Successful";
                this.Refresh();

                strDialogMessage = "Upload Successful.";
                strDialogTitle = "Upload Estimate";
            }
            else
            {
                _progressBar.Value = 100;
                _lblUploadStatus.Text = "No Estimates available to upload";
                this.Refresh();

                strDialogMessage = "There are no valid estimates.";
                strDialogTitle = "Cannot Upload";
            }

            PerformanceLog.End("Upload - Click");
            MessageBox.Show(strDialogMessage, strDialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ComputePubCostCode(string xmlString)
        {
            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("vendorcost_pub_i", conn);
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EstimateIDs", xmlString);
                cmd.Parameters.AddWithValue("@CreatedBy", MainForm.AuthorizedUser.FormattedName);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        private void ComputeVendorCostCode(string xmlString)
        {
            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("vendorcost_vnd_i", conn);
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EstimateIDs", xmlString);
                cmd.Parameters.AddWithValue("@CreatedBy", MainForm.AuthorizedUser.FormattedName);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        private void ComputePolybagCostCode(string xmlString)
        {
            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("vendorcost_polybag_i", conn);
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EstimateIDs", xmlString);
                cmd.Parameters.AddWithValue("@CreatedBy", MainForm.AuthorizedUser.FormattedName);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        private void ComputeAdPubCosts(string xmlString)
        {
            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("adpubcost_i", conn);
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EstimateIDs", xmlString);
                cmd.Parameters.AddWithValue("@CreatedBy", MainForm.AuthorizedUser.FormattedName);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        private void ComputeDirectMailCosts(string xmlString)
        {
            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("directmailcost_i", conn);
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EstimateIDs", xmlString);
                cmd.Parameters.AddWithValue("@CreatedBy", MainForm.AuthorizedUser.FormattedName);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        private bool GeneratePubChangesReport(DataTable dtPreUpload, DataTable dtPostUpload)
        {
            // Combine the Pre-Upload and Post-Upload data into a datatable
            // The report will display the contents of the datatable
            DataTable dtUploadData = new DataTable();
            dtUploadData.Columns.Add("RunDate");
            dtUploadData.Columns.Add("AdNumber");
            dtUploadData.Columns.Add("AdDescription");
            dtUploadData.Columns.Add("Pub_ID");
            dtUploadData.Columns.Add("PubDesc");
            dtUploadData.Columns.Add("PubLoc_ID");
            dtUploadData.Columns.Add("PreMediaQuantity");
            dtUploadData.Columns.Add("PreIssueDate");
            dtUploadData.Columns.Add("PostMediaQuantity");
            dtUploadData.Columns.Add("PostIssueDate");
            dtUploadData.Columns.Add("VarianceMediaQuantity");
            dtUploadData.Columns.Add("VarianceDays");

            int pre_idx = 0;
            int post_idx = 0;

            while (dtPreUpload.Rows.Count > pre_idx && dtPostUpload.Rows.Count > post_idx)
            {
                if (Convert.ToDateTime(dtPreUpload.Rows[pre_idx]["RunDate"]) < Convert.ToDateTime(dtPostUpload.Rows[post_idx]["RunDate"])
                    || (Convert.ToDateTime(dtPreUpload.Rows[pre_idx]["RunDate"]) == Convert.ToDateTime(dtPostUpload.Rows[post_idx]["RunDate"]) && Convert.ToInt32(dtPreUpload.Rows[pre_idx]["AdNumber"]) < Convert.ToInt32(dtPostUpload.Rows[post_idx]["AdNumber"]))
                    || (Convert.ToDateTime(dtPreUpload.Rows[pre_idx]["RunDate"]) == Convert.ToDateTime(dtPostUpload.Rows[post_idx]["RunDate"]) && Convert.ToInt32(dtPreUpload.Rows[pre_idx]["AdNumber"]) == Convert.ToInt32(dtPostUpload.Rows[post_idx]["AdNumber"]) && dtPreUpload.Rows[pre_idx]["Pub_ID"].ToString().CompareTo(dtPostUpload.Rows[post_idx]["Pub_ID"].ToString()) < 0)
                    || (Convert.ToDateTime(dtPreUpload.Rows[pre_idx]["RunDate"]) == Convert.ToDateTime(dtPostUpload.Rows[post_idx]["RunDate"]) && Convert.ToInt32(dtPreUpload.Rows[pre_idx]["AdNumber"]) == Convert.ToInt32(dtPostUpload.Rows[post_idx]["AdNumber"]) && dtPreUpload.Rows[pre_idx]["Pub_ID"].ToString() == dtPostUpload.Rows[post_idx]["Pub_ID"].ToString() && Convert.ToInt32(dtPreUpload.Rows[pre_idx]["PubLoc_ID"]) < Convert.ToInt32(dtPostUpload.Rows[post_idx]["PubLoc_ID"])))
                {
                    // pre
                    dtUploadData.Rows.Add(dtPreUpload.Rows[pre_idx]["RunDate"],
                        dtPreUpload.Rows[pre_idx]["AdNumber"],
                        dtPreUpload.Rows[pre_idx]["AdDescription"],
                        dtPreUpload.Rows[pre_idx]["Pub_ID"],
                        dtPreUpload.Rows[pre_idx]["PubDesc"],
                        dtPreUpload.Rows[pre_idx]["PubLoc_ID"],
                        dtPreUpload.Rows[pre_idx]["MediaQuantity"],
                        dtPreUpload.Rows[pre_idx]["IssueDate"], null, null,
                        -Convert.ToInt32(dtPreUpload.Rows[pre_idx]["MediaQuantity"]), null); 
                    ++pre_idx;
                }
                else if (Convert.ToDateTime(dtPostUpload.Rows[post_idx]["RunDate"]) < Convert.ToDateTime(dtPreUpload.Rows[pre_idx]["RunDate"])
                    || (Convert.ToDateTime(dtPostUpload.Rows[post_idx]["RunDate"]) == Convert.ToDateTime(dtPreUpload.Rows[pre_idx]["RunDate"]) && Convert.ToInt32(dtPostUpload.Rows[post_idx]["AdNumber"]) < Convert.ToInt32(dtPreUpload.Rows[pre_idx]["AdNumber"]))
                    || (Convert.ToDateTime(dtPostUpload.Rows[post_idx]["RunDate"]) == Convert.ToDateTime(dtPreUpload.Rows[pre_idx]["RunDate"]) && Convert.ToInt32(dtPostUpload.Rows[post_idx]["AdNumber"]) == Convert.ToInt32(dtPreUpload.Rows[pre_idx]["AdNumber"]) && dtPostUpload.Rows[post_idx]["Pub_ID"].ToString().CompareTo(dtPreUpload.Rows[pre_idx]["Pub_ID"].ToString()) < 0)
                    || (Convert.ToDateTime(dtPostUpload.Rows[post_idx]["RunDate"]) == Convert.ToDateTime(dtPreUpload.Rows[pre_idx]["RunDate"]) && Convert.ToInt32(dtPostUpload.Rows[post_idx]["AdNumber"]) == Convert.ToInt32(dtPreUpload.Rows[pre_idx]["AdNumber"]) && dtPostUpload.Rows[post_idx]["Pub_ID"].ToString() == dtPreUpload.Rows[pre_idx]["Pub_ID"].ToString() && Convert.ToInt32(dtPostUpload.Rows[post_idx]["PubLoc_ID"]) < Convert.ToInt32(dtPreUpload.Rows[pre_idx]["PubLoc_ID"])))
                {
                    // post
                    dtUploadData.Rows.Add(dtPostUpload.Rows[post_idx]["RunDate"],
                        dtPostUpload.Rows[post_idx]["AdNumber"],
                        dtPostUpload.Rows[post_idx]["AdDescription"],
                        dtPostUpload.Rows[post_idx]["Pub_ID"],
                        dtPostUpload.Rows[post_idx]["PubDesc"],
                        dtPostUpload.Rows[post_idx]["PubLoc_ID"],
                        null, null, dtPostUpload.Rows[post_idx]["MediaQuantity"], dtPostUpload.Rows[post_idx]["IssueDate"],
                        Convert.ToInt32(dtPostUpload.Rows[post_idx]["MediaQuantity"]), null);
                    ++post_idx;
                }
                else
                {
                    // both
                    dtUploadData.Rows.Add(dtPreUpload.Rows[pre_idx]["RunDate"],
                        dtPreUpload.Rows[pre_idx]["AdNumber"],
                        dtPreUpload.Rows[pre_idx]["AdDescription"],
                        dtPreUpload.Rows[pre_idx]["Pub_ID"],
                        dtPreUpload.Rows[pre_idx]["PubDesc"],
                        dtPreUpload.Rows[pre_idx]["PubLoc_ID"],
                        dtPreUpload.Rows[pre_idx]["MediaQuantity"],
                        dtPreUpload.Rows[pre_idx]["IssueDate"],
                        dtPostUpload.Rows[post_idx]["MediaQuantity"],
                        dtPostUpload.Rows[post_idx]["IssueDate"],
                        Convert.ToInt32(dtPostUpload.Rows[post_idx]["MediaQuantity"]) - Convert.ToInt32(dtPreUpload.Rows[pre_idx]["MediaQuantity"]),
                        FiscalCalculator.DaysApart(Convert.ToDateTime(dtPreUpload.Rows[pre_idx]["IssueDate"]), Convert.ToDateTime(dtPostUpload.Rows[post_idx]["IssueDate"])));

                    ++pre_idx;
                    ++post_idx;
                }

            }

            while (dtPreUpload.Rows.Count > pre_idx)
            {
                dtUploadData.Rows.Add(dtPreUpload.Rows[pre_idx]["RunDate"],
                    dtPreUpload.Rows[pre_idx]["AdNumber"],
                    dtPreUpload.Rows[pre_idx]["AdDescription"],
                    dtPreUpload.Rows[pre_idx]["Pub_ID"],
                    dtPreUpload.Rows[pre_idx]["PubDesc"],
                    dtPreUpload.Rows[pre_idx]["PubLoc_ID"],
                    dtPreUpload.Rows[pre_idx]["MediaQuantity"],
                    dtPreUpload.Rows[pre_idx]["IssueDate"], null, null,
                    -Convert.ToInt32(dtPreUpload.Rows[pre_idx]["MediaQuantity"]), null);

                ++pre_idx;
            }

            while (dtPostUpload.Rows.Count > post_idx)
            {
                dtUploadData.Rows.Add(dtPostUpload.Rows[post_idx]["RunDate"],
                    dtPostUpload.Rows[post_idx]["AdNumber"],
                    dtPostUpload.Rows[post_idx]["AdDescription"],
                    dtPostUpload.Rows[post_idx]["Pub_ID"],
                    dtPostUpload.Rows[post_idx]["PubDesc"],
                    dtPostUpload.Rows[post_idx]["PubLoc_ID"],
                    null, null, dtPostUpload.Rows[post_idx]["MediaQuantity"], dtPostUpload.Rows[post_idx]["IssueDate"],
                    Convert.ToInt32(dtPostUpload.Rows[post_idx]["MediaQuantity"]), null);
                    
                ++post_idx;
            }

            bool excelErrorOccurred = false;
            bool bFirstRecord = true;

            DateTime? prevRunDate = null;
            int? prevAdNumber = null;

            ExcelWriter writer = null;
            try
            {
                writer = new ExcelWriter("Pub Change", "ReportTemplate.xls");

                writer.WriteTemplateLine("Header1a", string.Concat("Report:     ", "Estimate Pub Changes"), null, null, null, null, null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header1b", string.Concat("Generated:  ", DateTime.Now.ToString()), null, null, null, null, null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header1b", null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header2", null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header3", null, null, null, null, null, null, null, null, null, null, null, null, null, null);

                for (int i = 0; i < dtUploadData.Rows.Count; ++i)
                {
                    object[] excelLine = new object[14];

                    if (!bFirstRecord && (prevRunDate.Value != Convert.ToDateTime(dtUploadData.Rows[i]["RunDate"]) || prevAdNumber.Value != Convert.ToInt32(dtUploadData.Rows[i]["AdNumber"])))
                    {
                        excelLine[0] = null;
                        excelLine[1] = null;
                        excelLine[2] = null;
                    }
                    else
                    {
                        excelLine[0] = Convert.ToDateTime(dtUploadData.Rows[i]["RunDate"]).ToShortDateString();
                        excelLine[1] = dtUploadData.Rows[i]["AdNumber"].ToString();
                        excelLine[2] = dtUploadData.Rows[i]["PubDesc"].ToString();
                    }

                    excelLine[3] = dtUploadData.Rows[i]["Pub_ID"].ToString();
                    excelLine[4] = dtUploadData.Rows[i]["PubDesc"].ToString();
                    excelLine[5] = dtUploadData.Rows[i]["PubLoc_ID"].ToString();
                    if (dtUploadData.Rows[i]["PreMediaQuantity"] == DBNull.Value)
                        excelLine[6] = null;
                    else
                        excelLine[6] = dtUploadData.Rows[i]["PreMediaQuantity"].ToString();
                    if (dtUploadData.Rows[i]["PreIssueDate"] == DBNull.Value)
                        excelLine[7] = null;
                    else
                        excelLine[7] = Convert.ToDateTime(dtUploadData.Rows[i]["PreIssueDate"]).ToShortDateString();
                    excelLine[8] = null;
                    if (dtUploadData.Rows[i]["PostMediaQuantity"] == DBNull.Value)
                        excelLine[9] = null;
                    else
                        excelLine[9] = dtUploadData.Rows[i]["PostMediaQuantity"].ToString();
                    if (dtUploadData.Rows[i]["PostIssueDate"] == DBNull.Value)
                        excelLine[10] = null;
                    else
                        excelLine[10] = Convert.ToDateTime(dtUploadData.Rows[i]["PostIssueDate"]).ToShortDateString();
                    excelLine[11] = null;
                    if (dtUploadData.Rows[i]["VarianceMediaQuantity"] == DBNull.Value)
                        excelLine[12] = null;
                    else
                        excelLine[12] = dtUploadData.Rows[i]["VarianceMediaQuantity"].ToString();
                    if (dtUploadData.Rows[i]["VarianceDays"] == DBNull.Value)
                        excelLine[13] = null;
                    else
                        excelLine[13] = dtUploadData.Rows[i]["VarianceDays"].ToString();

                    writer.WriteTemplateLine("RegRow1", excelLine);

                    prevRunDate = Convert.ToDateTime(dtUploadData.Rows[i]["RunDate"]);
                    prevAdNumber = Convert.ToInt32(dtUploadData.Rows[i]["AdNumber"]);

                    if (bFirstRecord)
                        bFirstRecord = false;
                }

                writer.AutoFitCells();
                writer.SetTemplateFreezePanes("FreezePanes");

                ReportWriter.SaveReportToDatabase(10, writer);
            }

            catch (System.Runtime.InteropServices.COMException)
            {
                excelErrorOccurred = true;
            }
            finally
            {
                writer.Quit();
                writer.Dispose();
            }

            return !excelErrorOccurred;
        }

        private bool GenerateFinancialChangeReport(DataTable dtPreUpload, DataTable dtPostUpload)
        {
            // Combine the Pre-Upload and Post-Upload data into a datatable
            // The report will display the contents of the datatable
            DataTable dtUploadData = new DataTable();
            dtUploadData.Columns.Add("FiscalYear");
            dtUploadData.Columns.Add("FiscalMonth");
            dtUploadData.Columns.Add("RunDate");
            dtUploadData.Columns.Add("AdNumber");
            dtUploadData.Columns.Add("AdDescription");
            dtUploadData.Columns.Add("PreMediaQty");
            dtUploadData.Columns.Add("PrePageCount");
            dtUploadData.Columns.Add("PreAdCost");
            dtUploadData.Columns.Add("PostMediaQty");
            dtUploadData.Columns.Add("PostPageCount");
            dtUploadData.Columns.Add("PostAdCost");
            dtUploadData.Columns.Add("VarianceMediaQty");
            dtUploadData.Columns.Add("VariancePageCount");
            dtUploadData.Columns.Add("VarianceAdCost");

            int pre_idx = 0;
            int post_idx = 0;

            while (dtPreUpload.Rows.Count > pre_idx && dtPostUpload.Rows.Count > post_idx)
            {
                if (Convert.ToDateTime(dtPreUpload.Rows[pre_idx]["RunDate"]) < Convert.ToDateTime(dtPostUpload.Rows[post_idx]["RunDate"])
                    || (Convert.ToDateTime(dtPreUpload.Rows[pre_idx]["RunDate"]) == Convert.ToDateTime(dtPostUpload.Rows[post_idx]["RunDate"]) && Convert.ToInt32(dtPreUpload.Rows[pre_idx]["AdNumber"]) < Convert.ToInt32(dtPostUpload.Rows[post_idx]["AdNumber"])))
                {
                    // pre
                    dtUploadData.Rows.Add(dtPreUpload.Rows[pre_idx]["FiscalYear"],
                        dtPreUpload.Rows[pre_idx]["FiscalMonth"],
                        dtPreUpload.Rows[pre_idx]["RunDate"],
                        dtPreUpload.Rows[pre_idx]["AdNumber"],
                        dtPreUpload.Rows[pre_idx]["AdDescription"],
                        dtPreUpload.Rows[pre_idx]["MediaQty"],
                        dtPreUpload.Rows[pre_idx]["PageCount"],
                        dtPreUpload.Rows[pre_idx]["AdCost"],
                        null, null, null,
                        -Convert.ToInt32(dtPreUpload.Rows[pre_idx]["MediaQty"]),
                        -Convert.ToInt32(dtPreUpload.Rows[pre_idx]["PageCount"]),
                        -Convert.ToDecimal(dtPreUpload.Rows[pre_idx]["AdCost"]));
                    ++pre_idx;
                }
                else if (Convert.ToDateTime(dtPostUpload.Rows[post_idx]["RunDate"]) < Convert.ToDateTime(dtPreUpload.Rows[pre_idx]["RunDate"])
                    || (Convert.ToDateTime(dtPostUpload.Rows[post_idx]["RunDate"]) == Convert.ToDateTime(dtPreUpload.Rows[pre_idx]["RunDate"]) && Convert.ToInt32(dtPostUpload.Rows[post_idx]["AdNumber"]) < Convert.ToInt32(dtPreUpload.Rows[pre_idx]["AdNumber"])))
                {
                    // post
                    dtUploadData.Rows.Add(dtPostUpload.Rows[post_idx]["FiscalYear"],
                        dtPostUpload.Rows[post_idx]["FiscalMonth"],
                        dtPostUpload.Rows[post_idx]["RunDate"],
                        dtPostUpload.Rows[post_idx]["AdNumber"],
                        dtPostUpload.Rows[post_idx]["AdDescription"],
                        null, null, null,
                        dtPostUpload.Rows[post_idx]["MediaQty"],
                        dtPostUpload.Rows[post_idx]["PageCount"],
                        dtPostUpload.Rows[post_idx]["AdCost"],
                        Convert.ToInt32(dtPostUpload.Rows[post_idx]["MediaQty"]),
                        Convert.ToInt32(dtPostUpload.Rows[post_idx]["PageCount"]),
                        Convert.ToDecimal(dtPostUpload.Rows[post_idx]["AdCost"]));
                    ++post_idx;
                }
                else
                {
                    // both
                    dtUploadData.Rows.Add(dtPreUpload.Rows[pre_idx]["FiscalYear"],
                        dtPreUpload.Rows[pre_idx]["FiscalMonth"],
                        dtPreUpload.Rows[pre_idx]["RunDate"],
                        dtPreUpload.Rows[pre_idx]["AdNumber"],
                        dtPreUpload.Rows[pre_idx]["AdDescription"],
                        dtPreUpload.Rows[pre_idx]["MediaQty"],
                        dtPreUpload.Rows[pre_idx]["PageCount"],
                        dtPreUpload.Rows[pre_idx]["AdCost"],
                        dtPostUpload.Rows[post_idx]["MediaQty"],
                        dtPostUpload.Rows[post_idx]["PageCount"],
                        dtPostUpload.Rows[post_idx]["AdCost"],
                        Convert.ToInt32(dtPostUpload.Rows[post_idx]["MediaQty"]) - Convert.ToInt32(dtPreUpload.Rows[pre_idx]["MediaQty"]),
                        Convert.ToInt32(dtPostUpload.Rows[post_idx]["PageCount"]) - Convert.ToInt32(dtPreUpload.Rows[pre_idx]["PageCount"]),
                        Convert.ToDecimal(dtPostUpload.Rows[post_idx]["AdCost"]) - Convert.ToDecimal(dtPreUpload.Rows[pre_idx]["AdCost"]));

                    ++pre_idx;
                    ++post_idx;
                }
            }

            while (dtPreUpload.Rows.Count > pre_idx)
            {
                dtUploadData.Rows.Add(dtPreUpload.Rows[pre_idx]["FiscalYear"],
                    dtPreUpload.Rows[pre_idx]["FiscalMonth"],
                    dtPreUpload.Rows[pre_idx]["RunDate"],
                    dtPreUpload.Rows[pre_idx]["AdNumber"],
                    dtPreUpload.Rows[pre_idx]["AdDescription"],
                    dtPreUpload.Rows[pre_idx]["MediaQty"],
                    dtPreUpload.Rows[pre_idx]["PageCount"],
                    dtPreUpload.Rows[pre_idx]["AdCost"],
                    null, null, null,
                    -Convert.ToInt32(dtPreUpload.Rows[pre_idx]["MediaQty"]),
                    -Convert.ToInt32(dtPreUpload.Rows[pre_idx]["PageCount"]),
                    -Convert.ToDecimal(dtPreUpload.Rows[pre_idx]["AdCost"]));

                ++pre_idx;
            }

            while (dtPostUpload.Rows.Count > post_idx)
            {
                dtUploadData.Rows.Add(dtPostUpload.Rows[post_idx]["FiscalYear"],
                    dtPostUpload.Rows[post_idx]["FiscalMonth"],
                    dtPostUpload.Rows[post_idx]["RunDate"],
                    dtPostUpload.Rows[post_idx]["AdNumber"],
                    dtPostUpload.Rows[post_idx]["AdDescription"],
                    null, null, null,
                    dtPostUpload.Rows[post_idx]["MediaQty"],
                    dtPostUpload.Rows[post_idx]["PageCount"],
                    dtPostUpload.Rows[post_idx]["AdCost"],
                    dtPostUpload.Rows[post_idx]["MediaQty"],
                    dtPostUpload.Rows[post_idx]["PageCount"],
                    dtPostUpload.Rows[post_idx]["AdCost"]);
                    
                ++post_idx;
            }

            bool excelErrorOccurred = false;
            bool bFirstRecord = true;

            int? prevFiscalYear = null;
            int? prevFiscalMonth = null;

            int subtotal_preMediaQuantity = 0;
            int subtotal_prePageCount = 0;
            decimal subtotal_preAdCost = 0;
            int subtotal_postMediaQuantity = 0;
            int subtotal_postPageCount = 0;
            decimal subtotal_postAdCost = 0;

            int total_preMediaQuantity = 0;
            int total_prePageCount = 0;
            decimal total_preAdCost = 0;
            int total_postMediaQuantity = 0;
            int total_postPageCount = 0;
            decimal total_postAdCost = 0;

            ExcelWriter writer = null;
            try
            {
                writer = new ExcelWriter("Financial Change", "ReportTemplate.xls");

                writer.WriteTemplateLine("Header1a", string.Concat("Report:     ", "Financial Change"), null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header1b", string.Concat("Generated:  ", DateTime.Now.ToString()), null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header1b", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header2", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header3", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

                for (int i = 0; i < dtUploadData.Rows.Count; ++i)
                {

                    if (!bFirstRecord && (prevFiscalYear.Value != Convert.ToInt32(dtUploadData.Rows[i]["FiscalYear"]) || prevFiscalMonth.Value != Convert.ToInt32(dtUploadData.Rows[i]["FiscalMonth"])))
                    {
                        writer.WriteTemplateLine("SubTotalRow1", null, null, null, null, null,
                            subtotal_preMediaQuantity, subtotal_prePageCount, subtotal_preAdCost,
                            subtotal_postMediaQuantity, subtotal_postPageCount, subtotal_postAdCost,
                            subtotal_postMediaQuantity - subtotal_preMediaQuantity,
                            subtotal_postPageCount - subtotal_prePageCount,
                            subtotal_postAdCost - subtotal_preAdCost, null);

                        subtotal_preMediaQuantity  = 0;
                        subtotal_prePageCount      = 0;
                        subtotal_preAdCost         = 0;
                        subtotal_postMediaQuantity = 0;
                        subtotal_postPageCount     = 0;
                        subtotal_postAdCost        = 0;
                    }

                    if (bFirstRecord)
                        bFirstRecord = false;

                    // Write a Regular Line
                    writer.WriteTemplateLine("RegRow1",
                        dtUploadData.Rows[i]["FiscalYear"],
                        FiscalCalculator.MonthName(Convert.ToInt32(dtUploadData.Rows[i]["FiscalMonth"])),
                        dtUploadData.Rows[i]["RunDate"],
                        dtUploadData.Rows[i]["AdNumber"],
                        dtUploadData.Rows[i]["AdDescription"],
                        dtUploadData.Rows[i]["PreMediaQty"],
                        dtUploadData.Rows[i]["PrePageCount"],
                        dtUploadData.Rows[i]["PreAdCost"],
                        dtUploadData.Rows[i]["PostMediaQty"],
                        dtUploadData.Rows[i]["PostPageCount"],
                        dtUploadData.Rows[i]["PostAdCost"],
                        dtUploadData.Rows[i]["VarianceMediaQty"],
                        dtUploadData.Rows[i]["VariancePageCount"],
                        dtUploadData.Rows[i]["VarianceAdCost"], null);

                    // add to subtotals
                    if (dtUploadData.Rows[i]["PreMediaQty"] != DBNull.Value)
                        subtotal_preMediaQuantity  += Convert.ToInt32(dtUploadData.Rows[i]["PreMediaQty"]);
                    if (dtUploadData.Rows[i]["PrePageCount"] != DBNull.Value)
                        subtotal_prePageCount      += Convert.ToInt32(dtUploadData.Rows[i]["PrePageCount"]);
                    if (dtUploadData.Rows[i]["PreAdCost"] != DBNull.Value)
                        subtotal_preAdCost         += Convert.ToDecimal(dtUploadData.Rows[i]["PreAdCost"]);
                    if (dtUploadData.Rows[i]["PostMediaQty"] != DBNull.Value)
                        subtotal_postMediaQuantity += Convert.ToInt32(dtUploadData.Rows[i]["PostMediaQty"]);
                    if (dtUploadData.Rows[i]["PostPageCount"] != DBNull.Value)
                        subtotal_postPageCount     += Convert.ToInt32(dtUploadData.Rows[i]["PostPageCount"]);
                    if (dtUploadData.Rows[i]["PostAdCost"] != DBNull.Value)
                        subtotal_postAdCost        += Convert.ToDecimal(dtUploadData.Rows[i]["PostAdCost"]);

                    if (dtUploadData.Rows[i]["PreMediaQty"] != DBNull.Value)
                        total_preMediaQuantity     += Convert.ToInt32(dtUploadData.Rows[i]["PreMediaQty"]);
                    if (dtUploadData.Rows[i]["PrePageCount"] != DBNull.Value)
                        total_prePageCount         += Convert.ToInt32(dtUploadData.Rows[i]["PrePageCount"]);
                    if (dtUploadData.Rows[i]["PreAdCost"] != DBNull.Value)
                        total_preAdCost            += Convert.ToDecimal(dtUploadData.Rows[i]["PreAdCost"]);
                    if (dtUploadData.Rows[i]["PostMediaQty"] != DBNull.Value)
                        total_postMediaQuantity    += Convert.ToInt32(dtUploadData.Rows[i]["PostMediaQty"]);
                    if (dtUploadData.Rows[i]["PostPageCount"] != DBNull.Value)
                        total_postPageCount        += Convert.ToInt32(dtUploadData.Rows[i]["PostPageCount"]);
                    if (dtUploadData.Rows[i]["PostAdCost"] != DBNull.Value)
                        total_postAdCost           += Convert.ToDecimal(dtUploadData.Rows[i]["PostAdCost"]);

                    // Set previous values, used to determine when to write a subtotal row
                    prevFiscalYear = Convert.ToInt32(dtUploadData.Rows[i]["FiscalYear"]);
                    prevFiscalMonth = Convert.ToInt32(dtUploadData.Rows[i]["FiscalMonth"]);
                }

                // Write Subtotal Line
                writer.WriteTemplateLine("SubTotalRow1", null, null, null, null, null,
                    subtotal_preMediaQuantity, subtotal_prePageCount, subtotal_preAdCost,
                    subtotal_postMediaQuantity, subtotal_postPageCount, subtotal_postAdCost,
                    subtotal_postMediaQuantity - subtotal_preMediaQuantity,
                    subtotal_postPageCount - subtotal_prePageCount,
                    subtotal_postAdCost - subtotal_preAdCost, null);
                // Write Total Line
                writer.WriteTemplateLine("TotalRow1", null, null, null, null, null,
                    total_preMediaQuantity, total_prePageCount, total_preAdCost,
                    total_postMediaQuantity, total_postPageCount, total_postAdCost,
                    total_postMediaQuantity - total_preMediaQuantity,
                    total_postPageCount - total_prePageCount,
                    total_postAdCost - total_preAdCost, null);

                writer.AutoFitCells();
                writer.SetTemplateFreezePanes("FreezePanes");

                ReportWriter.SaveReportToDatabase( 6, writer );
            }

            catch (System.Runtime.InteropServices.COMException)
            {
                excelErrorOccurred = true;
            }
            finally
            {
                writer.Quit();
                writer.Dispose();
            }

            return !excelErrorOccurred;
        }

        private void PerformUpload(string xmlString)
        {
            using (SqlConnection conn = (SqlConnection) MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("EstEstimate_Upload", conn);
                cmd.CommandTimeout = 7200;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EstimateIDs", xmlString);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        private DataTable PubChangeReport_UploadData(string xmlString)
        {
            DataTable dtUploadData = new DataTable();
            dtUploadData.Columns.Add("RunDate");
            dtUploadData.Columns.Add("AdNumber");
            dtUploadData.Columns.Add("AdDescription");
            dtUploadData.Columns.Add("Pub_ID");
            dtUploadData.Columns.Add("PubDesc");
            dtUploadData.Columns.Add("PubLoc_ID");
            dtUploadData.Columns.Add("MediaQuantity");
            dtUploadData.Columns.Add("IssueDate");

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("rpt_PubChange_UploadData", conn);
                cmd.CommandTimeout = 7200;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EstimateIDs", xmlString);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int mediaQuantity = 0;
                    if (!dr.IsDBNull(dr.GetOrdinal("MediaQuantity")))
                        mediaQuantity = dr.GetInt32(dr.GetOrdinal("MediaQuantity"));

                    dtUploadData.Rows.Add(dr.GetDateTime(dr.GetOrdinal("RunDate")), dr.GetInt32(dr.GetOrdinal("AdNumber")), dr["AdDescription"].ToString(), dr["Pub_ID"].ToString(), dr["PubDesc"].ToString(), dr.GetInt16(dr.GetOrdinal("PubLoc_ID")), mediaQuantity, dr.GetDateTime(dr.GetOrdinal("IssueDate")));
                }
                dr.Close();
                conn.Close();
            }

            return dtUploadData;
        }

        private DataTable FinancialChangeReport_UploadData(string xmlString)
        {
            DataTable dtUploadData = new DataTable();
            dtUploadData.Columns.Add("RunDate");
            dtUploadData.Columns.Add("Season");
            dtUploadData.Columns.Add("FiscalYear");
            dtUploadData.Columns.Add("FiscalMonth");
            dtUploadData.Columns.Add("AdNumber");
            dtUploadData.Columns.Add("AdDescription");
            dtUploadData.Columns.Add("MediaQty");
            dtUploadData.Columns.Add("PageCount");
            dtUploadData.Columns.Add("AdCost");

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("rpt_FinancialChange_UploadData", conn);
                cmd.CommandTimeout = 7200;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EstimateIDs", xmlString);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dtUploadData.Rows.Add(dr.GetDateTime(dr.GetOrdinal("RunDate")), dr["Season"].ToString(), dr.GetInt32(dr.GetOrdinal("FiscalYear")), dr.GetInt32(dr.GetOrdinal("FiscalMonth")), dr.GetInt32(dr.GetOrdinal("AdNumber")), dr["AdDescription"].ToString(), dr.GetInt32(dr.GetOrdinal("MediaQty")), dr.GetInt32(dr.GetOrdinal("PageCount")), dr.GetDecimal(dr.GetOrdinal("AdCost")));
                }
                dr.Close();

                conn.Close();
            }

            return dtUploadData;
        }

        private void UpdateGridwithCloneErrors()
        {
            foreach (DataRow dr in _dtEstimateStatus.Rows)
            {
                if (dr["StatusCode"].ToString() == EstimateUploadStatus.CloneError.ToString())
                {
                    foreach (DataGridViewRow dg_row in _gridUpload.Rows)
                    {
                        if (Convert.ToInt64(dg_row.Cells[0].Value) == Convert.ToInt64(dr["EST_Estimate_ID"]))
                        {
                            dg_row.DefaultCellStyle.BackColor = System.Drawing.Color.Salmon;
                            dg_row.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Coral;
                            dg_row.Cells[7].Value = dr["ErrorMessage"].ToString();
                        }
                    }
                }
            }
        }

        private void UpdateGridwithUploadStatus()
        {
            foreach (DataRow dr in _dtEstimateStatus.Rows)
            {
                if (dr["StatusCode"].ToString() == EstimateUploadStatus.Valid.ToString() || dr["StatusCode"].ToString() == EstimateUploadStatus.Warning.ToString())
                {
                    dr["StatusCode"] = EstimateUploadStatus.UploadComplete;

                    foreach (DataGridViewRow dg_row in _gridUpload.Rows)
                    {
                        if (Convert.ToInt64(dg_row.Cells[0].Value) == Convert.ToInt64(dr["EST_Estimate_ID"]))
                        {
                            dg_row.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                            dg_row.Cells[7].Value = "Upload Complete";
                        }
                    }
                }
            }
        }

        #region Public Properties

        public List<long> UploadEstimates
        {
            set { _uploadEstimates = value; }
        }

        #endregion

        private void _gridUpload_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            MainForm main = (MainForm) ParentForm;
            long estimateID = Convert.ToInt64(_gridUpload[0, e.RowIndex].Value);
            long? parentID = null;
            if (!string.IsNullOrEmpty(_gridUpload[1, e.RowIndex].Value.ToString()))
                parentID = Convert.ToInt64(_gridUpload[1, e.RowIndex].Value);

            main.OpenEstimate(false, estimateID, parentID, null);
        }

    }
}
