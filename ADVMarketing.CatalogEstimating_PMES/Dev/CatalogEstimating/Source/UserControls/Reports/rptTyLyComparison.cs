using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CatalogEstimating.Datasets;
using System.Data.SqlClient;
using System.IO;
using System.Xml;

namespace CatalogEstimating.UserControls.Reports
{
    public partial class rptTyLyComparison : CatalogEstimating.UserControls.Reports.ReportControl
    {
        #region Construction

        public rptTyLyComparison()
            : base()
        {
            InitializeComponent();
        }

        public rptTyLyComparison(Lookup dsLookup) : base(dsLookup)
        {
            InitializeComponent();

            _cboSeason.DataSource = _dsLookup.est_season;
            _cboSeason.DisplayMember = "description";
            _cboSeason.ValueMember = "est_season_id";

            _dsLookup.EstEstimate_s_FiscalYear.DefaultView.Sort = "fiscalyear";
            _cboFiscalYear.Items.Add(new IntPair(-1, string.Empty));
            foreach (DataRowView drv_row in _dsLookup.EstEstimate_s_FiscalYear.DefaultView)
            {
                Lookup.EstEstimate_s_FiscalYearRow fy_row = (Lookup.EstEstimate_s_FiscalYearRow)drv_row.Row;
                _cboFiscalYear.Items.Add(new IntPair(fy_row.fiscalyear, fy_row.fiscalyear.ToString()));
            }

            _lstEstMediaType.DataSource = _dsLookup.est_estimatemediatype;
            _lstEstMediaType.DisplayMember = "description";
            _lstEstMediaType.ValueMember = "est_estimatemediatype_id";
            _lstEstMediaType.SelectedIndex = -1;

            _lstComponentType.DataSource = _dsLookup.est_componenttype;
            _lstComponentType.DisplayMember = "description";
            _lstComponentType.ValueMember = "est_componenttype_id";
            _lstComponentType.SelectedIndex = -1;
        }

        #endregion

        #region Overrides

        public override string ReportTemplate
        {
            get
            {
                return "TY vs LY";
            }
        }

        public override ReportExecutionStatus RunReport(ExcelWriter writer)
        {

            if (!ValidateSearchCriteria())
                return ReportExecutionStatus.InvalidSearchCriteria;


            int? seasonID = null;
            int vendorSupplied;

            string xmlEstimateMediaType;
            string xmlComponentType;

            if (_cboSeason.SelectedIndex > 0)
                seasonID = Convert.ToInt32(_cboSeason.SelectedValue);

            if (_radAll.Checked)
                vendorSupplied = 1;
            else if (_radOnlyVS.Checked)
                vendorSupplied = 2;
            else
                vendorSupplied = 3;

            xmlEstimateMediaType = EstimateMediaType_Xml();
            xmlComponentType = ComponentType_Xml();

            DataTable dtYear1 = GetFiscalData(seasonID, Convert.ToInt32(_cboFiscalYear.Text), vendorSupplied, xmlEstimateMediaType, xmlComponentType);
            DataTable dtYear2 = GetFiscalData(seasonID, Convert.ToInt32(_cboFiscalYear.Text) - 1, vendorSupplied, xmlEstimateMediaType, xmlComponentType);

            // If there is no data returned, inform the user
            if (dtYear1.Rows.Count == 0 && dtYear2.Rows.Count == 0)
                return ReportExecutionStatus.NoDataReturned;


            DataTable dtFiscalData = GetMergedReportData(dtYear1, dtYear2);

            bool bFirstRow = true;
            int? prevSeasonID = null;
            
            int subTotalPreAdCount = 0;
            int subTotalPrePageCount = 0;
            decimal subTotalPreEstimatedCost = 0;
            int subTotalPostAdCount = 0;
            int subTotalPostPageCount = 0;
            decimal subTotalPostEstimatedCost = 0;

            writer.WriteTemplateLine("Header1a", string.Concat("Report:     ", "This Year Versus Last Year Comparison"), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            writer.WriteTemplateLine("Header1b", string.Concat("Generated:  ", DateTime.Now.ToString()), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            writer.WriteTemplateLine("Header1b", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

            foreach (DataRow dr in dtFiscalData.Rows)
            {
                if (bFirstRow)
                {
                    writer.WriteLine(new object[] { });
                    writer.WriteTemplateLine("HeaderSeason", dr["SeasonDesc"].ToString() + " " + _cboFiscalYear.Text, null, null, null, null, null,
                        dr["SeasonDesc"].ToString() + " " + Convert.ToString(Convert.ToInt32(_cboFiscalYear.Text) - 1), null, null, null, null, null, null, null, null, null);
                    writer.WriteTemplateLine("HeaderColumn", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                    bFirstRow = false;
                }
                else if (prevSeasonID.Value != Convert.ToInt32(dr["SeasonID"]))
                {
                    writer.WriteTemplateLine("SubRow", subTotalPreAdCount, null, null, subTotalPrePageCount, null, subTotalPreEstimatedCost, subTotalPostAdCount, null, null, subTotalPostPageCount, null, subTotalPostEstimatedCost, (subTotalPreAdCount - subTotalPostAdCount), (subTotalPrePageCount - subTotalPostPageCount), null, (subTotalPreEstimatedCost - subTotalPostEstimatedCost));
                    writer.WriteLine(new object[] { });
                    writer.WriteTemplateLine("HeaderSeason", dr["SeasonDesc"].ToString() + " " + _cboFiscalYear.Text, null, null, null, null, null,
                        dr["SeasonDesc"].ToString() + " " + Convert.ToString(Convert.ToInt32(_cboFiscalYear.Text) - 1), null, null, null, null, null, null, null, null, null);
                    writer.WriteTemplateLine("HeaderColumn", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

                    subTotalPrePageCount      = 0;
                    subTotalPreEstimatedCost  = 0;
                    subTotalPostPageCount     = 0;
                    subTotalPostEstimatedCost = 0;
                }

                writer.WriteTemplateLine("RegRow", dr["PreRunDate"].ToString(), dr["PreAdNumber"].ToString(), dr["PreDescription"].ToString(),
                    dr["PrePageCount"].ToString(), dr["PreMediaQuantity"].ToString(), dr["PreEstimatedCost"].ToString(),
                    dr["PostRunDate"].ToString(), dr["PostAdNumber"].ToString(), dr["PostDescription"].ToString(), dr["PostPageCount"].ToString(), dr["PostMediaQuantity"].ToString(),
                    dr["PostEstimatedCost"].ToString(), null, null, null, null);

                prevSeasonID = Convert.ToInt32(dr["SeasonID"]);

                if (dr["PrePageCount"] != DBNull.Value)
                {
                    ++subTotalPreAdCount;
                    subTotalPrePageCount += Convert.ToInt32(dr["PrePageCount"]);
                }
                if (dr["PreEstimatedCost"] != DBNull.Value)
                    subTotalPreEstimatedCost += Convert.ToDecimal(dr["PreEstimatedCost"]);

                if (dr["PostPageCount"] != DBNull.Value)
                {
                    ++subTotalPostAdCount;
                    subTotalPostPageCount += Convert.ToInt32(dr["PostPageCount"]);
                }
                if (dr["PostEstimatedCost"] != DBNull.Value)
                    subTotalPostEstimatedCost += Convert.ToDecimal(dr["PostEstimatedCost"]);
            }

            writer.WriteTemplateLine("SubRow", subTotalPreAdCount, null, null, subTotalPrePageCount, null, subTotalPreEstimatedCost, subTotalPostAdCount, null, null, subTotalPostPageCount, null, subTotalPostEstimatedCost, (subTotalPreAdCount - subTotalPostAdCount), (subTotalPrePageCount - subTotalPostPageCount), null, (subTotalPreEstimatedCost - subTotalPostEstimatedCost));
            writer.WriteLine(new object[] { });

            writer.WriteTemplateLine("Criteria1", "Filter and Selection Criteria", null, null, null, null);
            writer.WriteTemplateLine("Criteria2", "Vendor Supplied", null, null, null, null);
            if (_radAll.Checked)
                writer.WriteTemplateLine("Criteria3", null, "All", null, null, null, null);
            else if (_radOnlyVS.Checked)
                writer.WriteTemplateLine("Criteria3", null, "Only VS", null, null, null, null);
            else
                writer.WriteTemplateLine("Criteria3", null, "Exclude VS", null, null, null, null);

            writer.WriteTemplateLine("Criteria2", "Estimate Media Types", null, null, null, null);
            if (_lstEstMediaType.SelectedItems.Count == 0)
                writer.WriteTemplateLine("Criteria3", null, "All", null, null, null, null);
            else
            {
                for (int i = 0; i < _lstEstMediaType.SelectedItems.Count; ++i)
                    writer.WriteTemplateLine("Criteria3", null, ((Lookup.est_estimatemediatypeRow)((DataRowView)_lstEstMediaType.SelectedItems[i]).Row).description, null, null, null, null);
            }

            writer.WriteTemplateLine("Criteria2", "Component Types", null, null, null, null);
            if (_lstComponentType.SelectedItems.Count == 0)
                writer.WriteTemplateLine("Criteria3", null, "All", null, null, null, null);
            else
            {
                for (int i = 0; i < _lstComponentType.SelectedItems.Count; ++i)
                    writer.WriteTemplateLine("Criteria3", null, ((Lookup.est_componenttypeRow)((DataRowView)_lstComponentType.SelectedItems[i]).Row).description, null, null, null, null);
            }

            writer.SetTemplateFreezePanes("FreezePanes");

            return ReportExecutionStatus.Success;
        }

        private DataTable GetMergedReportData(DataTable dtYear1, DataTable dtYear2)
        {
            DataTable dtFiscalData = new DataTable();
            dtFiscalData.Columns.Add("FiscalYear");
            dtFiscalData.Columns.Add("SeasonID");
            dtFiscalData.Columns.Add("SeasonDesc");
            dtFiscalData.Columns.Add("PreRunDate");
            dtFiscalData.Columns.Add("PreAdNumber");
            dtFiscalData.Columns.Add("PreDescription");
            dtFiscalData.Columns.Add("PrePageCount");
            dtFiscalData.Columns.Add("PreMediaQuantity");
            dtFiscalData.Columns.Add("PreEstimatedCost");
            dtFiscalData.Columns.Add("PostRunDate");
            dtFiscalData.Columns.Add("PostAdNumber");
            dtFiscalData.Columns.Add("PostDescription");
            dtFiscalData.Columns.Add("PostPageCount");
            dtFiscalData.Columns.Add("PostMediaQuantity");
            dtFiscalData.Columns.Add("PostEstimatedCost");

            int y1_idx = 0;
            int y2_idx = 0;

            if (_cboSeason.SelectedIndex < 1)
            {
                bool y1s1_complete = false;
                bool y2s1_complete = false;

                while (dtYear1.Rows.Count > y1_idx && !y1s1_complete)
                {
                    if (Convert.ToInt32(dtYear1.Rows[y1_idx]["SeasonID"]) == 2)
                        y1s1_complete = true;
                    else
                    {
                        dtFiscalData.Rows.Add(
                            dtYear1.Rows[y1_idx]["FiscalYear"],
                            dtYear1.Rows[y1_idx]["SeasonID"],
                            dtYear1.Rows[y1_idx]["SeasonDesc"],
                            dtYear1.Rows[y1_idx]["RunDate"],
                            dtYear1.Rows[y1_idx]["AdNumber"],
                            dtYear1.Rows[y1_idx]["Description"],
                            dtYear1.Rows[y1_idx]["PageCount"],
                            dtYear1.Rows[y1_idx]["MediaQuantity"],
                            dtYear1.Rows[y1_idx]["EstimatedCost"],
                            null, null, null, null, null, null);
                        ++y1_idx;
                    }
                }

                while (dtYear2.Rows.Count > y2_idx && !y2s1_complete)
                {
                    if (Convert.ToInt32(dtYear2.Rows[y2_idx]["SeasonID"]) == 2)
                        y2s1_complete = true;
                    else if (y2_idx > y1_idx || y2_idx >= dtFiscalData.Rows.Count)
                    {
                        dtFiscalData.Rows.Add(
                            dtYear2.Rows[y2_idx]["FiscalYear"],
                            dtYear2.Rows[y2_idx]["SeasonID"],
                            dtYear2.Rows[y2_idx]["SeasonDesc"],
                            null, null, null, null, null, null,
                            dtYear2.Rows[y2_idx]["RunDate"],
                            dtYear2.Rows[y2_idx]["AdNumber"],
                            dtYear2.Rows[y2_idx]["Description"],
                            dtYear2.Rows[y2_idx]["PageCount"],
                            dtYear2.Rows[y2_idx]["MediaQuantity"],
                            dtYear2.Rows[y2_idx]["EstimatedCost"]);
                        ++y2_idx;
                    }
                    else
                    {
                        dtFiscalData.Rows[y2_idx]["PostRunDate"] = dtYear2.Rows[y2_idx]["RunDate"];
                        dtFiscalData.Rows[y2_idx]["PostAdNumber"] = dtYear2.Rows[y2_idx]["AdNumber"];
                        dtFiscalData.Rows[y2_idx]["PostDescription"] = dtYear2.Rows[y2_idx]["Description"];
                        dtFiscalData.Rows[y2_idx]["PostPageCount"] = dtYear2.Rows[y2_idx]["PageCount"];
                        dtFiscalData.Rows[y2_idx]["PostMediaQuantity"] = dtYear2.Rows[y2_idx]["MediaQuantity"];
                        dtFiscalData.Rows[y2_idx]["PostEstimatedCost"] = dtYear2.Rows[y2_idx]["EstimatedCost"];
                        ++y2_idx;
                    }
                }
            }

            int merged_idx = Math.Max(y1_idx, y2_idx);

            while (dtYear1.Rows.Count > y1_idx)
            {
                dtFiscalData.Rows.Add(
                    dtYear1.Rows[y1_idx]["FiscalYear"],
                    dtYear1.Rows[y1_idx]["SeasonID"],
                    dtYear1.Rows[y1_idx]["SeasonDesc"],
                    dtYear1.Rows[y1_idx]["RunDate"],
                    dtYear1.Rows[y1_idx]["AdNumber"],
                    dtYear1.Rows[y1_idx]["Description"],
                    dtYear1.Rows[y1_idx]["PageCount"],
                    dtYear1.Rows[y1_idx]["MediaQuantity"],
                    dtYear1.Rows[y1_idx]["EstimatedCost"],
                    null, null, null, null, null, null);
                ++y1_idx;
            }

            while (dtYear2.Rows.Count > y2_idx)
            {
                // If Year 2 Index <= Merged Index update the dtFiscalData row
                if ((y2_idx - merged_idx) >= 0)
                {
                    dtFiscalData.Rows.Add(
                        dtYear2.Rows[y2_idx]["FiscalYear"],
                        dtYear2.Rows[y2_idx]["SeasonID"],
                        dtYear2.Rows[y2_idx]["SeasonDesc"],
                        null, null, null, null, null, null,
                        dtYear2.Rows[y2_idx]["RunDate"],
                        dtYear2.Rows[y2_idx]["AdNumber"],
                        dtYear2.Rows[y2_idx]["Description"],
                        dtYear2.Rows[y2_idx]["PageCount"],
                        dtYear2.Rows[y2_idx]["MediaQuantity"],
                        dtYear2.Rows[y2_idx]["EstimatedCost"]);
                    ++y2_idx;
                }
                // If the Year 2 Index > Merged Index add the dtFiscalData row
                else
                {
                    dtFiscalData.Rows[y2_idx]["PostRunDate"] = dtYear2.Rows[y2_idx]["RunDate"];
                    dtFiscalData.Rows[y2_idx]["PostAdNumber"] = dtYear2.Rows[y2_idx]["AdNumber"];
                    dtFiscalData.Rows[y2_idx]["PostDescription"] = dtYear2.Rows[y2_idx]["Description"];
                    dtFiscalData.Rows[y2_idx]["PostPageCount"] = dtYear2.Rows[y2_idx]["PageCount"];
                    dtFiscalData.Rows[y2_idx]["PostMediaQuantity"] = dtYear2.Rows[y2_idx]["MediaQuantity"];
                    dtFiscalData.Rows[y2_idx]["PostEstimatedCost"] = dtYear2.Rows[y2_idx]["EstimatedCost"];
                    ++y2_idx;
                }
            }
            return dtFiscalData;
        }
        #endregion

        private string EstimateMediaType_Xml()
        {
            string EstimateMediaTypes = String.Empty;

            if (_lstEstMediaType.SelectedItems.Count > 0 && _lstEstMediaType.SelectedItems.Count != _lstEstMediaType.Items.Count)
            {
                StringWriter writerString = new StringWriter();
                XmlTextWriter writerXml = new XmlTextWriter(writerString);
                writerXml.WriteStartElement("root");
                for (int i = 0; i < _lstEstMediaType.SelectedItems.Count; ++i)
                {
                    Lookup.est_estimatemediatypeRow r = (Lookup.est_estimatemediatypeRow)((DataRowView)_lstEstMediaType.SelectedItems[i]).Row;
                    writerXml.WriteStartElement("est_estimatemediatype");
                    writerXml.WriteAttributeString("est_estimatemediatype_id", r.est_estimatemediatype_id.ToString());
                    writerXml.WriteEndElement();
                }
                writerXml.WriteEndElement();

                EstimateMediaTypes = writerString.ToString();
            }

            return EstimateMediaTypes;
        }

        private string ComponentType_Xml()
        {
            string ComponentTypes = String.Empty;

            if (_lstComponentType.SelectedItems.Count > 0 && _lstComponentType.SelectedItems.Count != _lstComponentType.Items.Count)
            {
                StringWriter writerString = new StringWriter();
                XmlTextWriter writerXml = new XmlTextWriter(writerString);
                writerXml.WriteStartElement("root");
                for (int i = 0; i < _lstComponentType.SelectedItems.Count; ++i)
                {
                    Lookup.est_componenttypeRow r = (Lookup.est_componenttypeRow)((DataRowView)_lstComponentType.SelectedItems[i]).Row;
                    writerXml.WriteStartElement("est_componenttype");
                    writerXml.WriteAttributeString("est_componenttype_id", r.est_componenttype_id.ToString());
                    writerXml.WriteEndElement();
                }
                writerXml.WriteEndElement();

                ComponentTypes = writerString.ToString();
            }

            return ComponentTypes;
        }

        private bool ValidateSearchCriteria()
        {
            bool isValid = true;
            _errorProvider.Clear();

            if (_cboFiscalYear.SelectedIndex < 1)
            {
                _errorProvider.SetError(_cboFiscalYear, CatalogEstimating.Properties.Resources.RequiredFieldError);
                isValid = false;
            }

            return isValid;
        }

        private DataTable GetFiscalData(int? seasonID, int fiscalYear, int vendorSupplied, string xmlEstimateMediaType, string xmlComponentType)
        {
            DataTable dtFiscalData = new DataTable();
            dtFiscalData.Columns.Add("FiscalYear");
            dtFiscalData.Columns.Add("SeasonID");
            dtFiscalData.Columns.Add("AdNumber");
            dtFiscalData.Columns.Add("RunDate");
            dtFiscalData.Columns.Add("SeasonDesc");
            dtFiscalData.Columns.Add("Description");
            dtFiscalData.Columns.Add("PageCount");
            dtFiscalData.Columns.Add("MediaQuantity");
            dtFiscalData.Columns.Add("EstimatedCost");

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("rpt_TYLY", conn);
                cmd.CommandTimeout = 600;
                cmd.CommandType = CommandType.StoredProcedure;
                if (seasonID.HasValue)
                    cmd.Parameters.AddWithValue("@EST_Season_ID", seasonID.Value);
                else
                    cmd.Parameters.AddWithValue("@EST_Season_ID", DBNull.Value);
                cmd.Parameters.AddWithValue("@FiscalYear", fiscalYear);
                cmd.Parameters.AddWithValue("@VendorSupplied", vendorSupplied);
                if (xmlEstimateMediaType != string.Empty)
                    cmd.Parameters.AddWithValue("@EstimateMediaType", xmlEstimateMediaType);
                else
                    cmd.Parameters.AddWithValue("@EstimateMediaType", DBNull.Value);
                if (xmlComponentType != string.Empty)
                    cmd.Parameters.AddWithValue("@ComponentType", xmlComponentType);
                else
                    cmd.Parameters.AddWithValue("@ComponentType", DBNull.Value);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dtFiscalData.Rows.Add(dr["FiscalYear"].ToString(), dr["SeasonID"].ToString(),
                        dr["AdNumber"].ToString(), dr.GetDateTime(dr.GetOrdinal("RunDate")),
                        dr["SeasonDesc"].ToString(), dr["Description"].ToString(), dr["PageCount"].ToString(), dr["MediaQuantity"].ToString(),
                        dr["EstimatedCost"].ToString());
                }

                conn.Close();
            }

            return dtFiscalData;
        }
    }
}

