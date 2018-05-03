using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

using CatalogEstimating.Datasets;

namespace CatalogEstimating.UserControls.Reports
{
    public partial class rptAdPublicationCosts : CatalogEstimating.UserControls.Reports.ReportControl
    {
        public rptAdPublicationCosts()
        {
            InitializeComponent();
        }

        public rptAdPublicationCosts(Lookup dsLookup) : base(dsLookup)
        {
            InitializeComponent();

            _cboEstimateStatus.DataSource = _dsLookup.est_status;
            _cboEstimateStatus.DisplayMember = "Description";
            _cboEstimateStatus.ValueMember = "EST_Status_ID";

            _cboPublication.DataSource = _dsLookup.PublicationList;
            _cboPublication.DisplayMember = "Pub_ID";
            _cboPublication.ValueMember = "Pub_ID";
            _cboPublication.SelectedIndex = -1;
        }


        public override string ReportTemplate
        {
            get
            {
                return "Ad Publication Costs";
            }
        }

        private bool ValidateSearchCriteria()
        {
            _errorProvider.Clear();

            if (_dtStartRunDate.Value != null && _dtStartRunDate.Value > _dtEndRunDate.Value)
            {
                _errorProvider.SetError( _dtEndRunDate, "You must enter a later ending date into the range of Run Dates." );
                return false;
            }

            if (_dtStartRunDate.Value == null && _dtEndRunDate.Value == null && _cboPublication.SelectedIndex == -1 && _txtAdNumber.Text.Trim() == String.Empty && _txtEstimateID.Text.Trim() == String.Empty)
            {
                _errorProvider.SetError( _dtStartRunDate, "Range of Run Dates, Publication, Estimate ID or Host Ad Number is required" );
                return false;
            }

            return true;
        }

        public override ReportExecutionStatus RunReport(ExcelWriter writer)
        {
            if (!ValidateSearchCriteria())
                return ReportExecutionStatus.InvalidSearchCriteria;

            using ( SqlConnection myConn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
            {
                myConn.Open();

                SqlCommand myCmd = new SqlCommand("rpt_AdPublicationCosts", myConn);
                myCmd.CommandType = CommandType.StoredProcedure;
                myCmd.CommandTimeout = 3600;

                if (_dtStartRunDate.Value == null)
                    myCmd.Parameters.AddWithValue("@StartRunDate", DBNull.Value);
                else
                    myCmd.Parameters.AddWithValue("@StartRunDate", _dtStartRunDate.Value);
                if (_dtEndRunDate.Value == null)
                    myCmd.Parameters.AddWithValue("@EndRunDate", DBNull.Value);
                else
                    myCmd.Parameters.AddWithValue("@EndRunDate", _dtEndRunDate.Value);
                if (_cboPublication.SelectedIndex > 0)
                    myCmd.Parameters.AddWithValue("@Pub_ID", _cboPublication.SelectedValue);
                else
                    myCmd.Parameters.AddWithValue("@Pub_ID", DBNull.Value);
                if (_txtAdNumber.Text.Trim() != "")
                    myCmd.Parameters.AddWithValue("@AdNumber", _txtAdNumber.Text.Trim());
                else
                    myCmd.Parameters.AddWithValue("@AdNumber", DBNull.Value);
                if (_txtEstimateID.Text.Trim() != "")
                    myCmd.Parameters.AddWithValue("@EST_Estimate_ID", Convert.ToInt64(_txtEstimateID.Text));
                else
                    myCmd.Parameters.AddWithValue("@EST_Estimate_ID", DBNull.Value);
                if (_cboEstimateStatus.SelectedIndex > 0)
                    myCmd.Parameters.AddWithValue("@EST_Status_ID", _cboEstimateStatus.SelectedValue);
                else
                    myCmd.Parameters.AddWithValue("@EST_Status_ID", DBNull.Value);

                SqlDataReader myDR = myCmd.ExecuteReader();
                if (!myDR.HasRows)
                {
                    myDR.Close();
                    myConn.Close();
                    return ReportExecutionStatus.NoDataReturned;
                }

                bool firstrow = true;
                string prevAdNumber = "";
                string prevRunDate = "";
                string prevIssueDate = "";
                string prevInsertTime = "";
                int PubRateMapInsertQuantitySubTotal = 0;
                decimal CostWithoutInsertSubTotal = 0;
                decimal PubRateMapInsertCostSubTotal = 0;
                decimal TotalCostSubTotal = 0;
                int PubRateMapInsertQuantityTotal = 0;
                decimal CostWithoutInsertTotal = 0;
                decimal PubRateMapInsertCostTotal = 0;
                decimal TotalCostTotal = 0;

                writer.WriteTemplateLine("Header1a", string.Concat("Report:     ", "Ad Publication Costs"), null, null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header1b", string.Concat("Generated:  ", DateTime.Now.ToString()), null, null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header1b", null, null, null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Title1", "Rundate", "Issue Date", "Ad #", "AM/PM", "PubID", "Pub Loc", "Piece Cost", "Quantity", "Cost w/o Insert", "Insert Cost", "Total Cost");

                while (myDR.Read())
                {
                    if (firstrow)
                    {
                        writer.WriteTemplateLine("TopRow", myDR.GetDateTime(myDR.GetOrdinal("RunDate")).ToShortDateString(), myDR.GetDateTime(myDR.GetOrdinal("IssueDate")).ToShortDateString(), myDR["AdNumber"].ToString(), myDR["InsertTime"].ToString(), myDR["Pub_ID"].ToString(), myDR["PubLoc_ID"].ToString(), myDR["PieceCost"].ToString(), myDR.GetInt32(myDR.GetOrdinal("PubRateMapInsertQuantity")), myDR["CostWithoutInsert"].ToString(), myDR["PubRateMapInsertCost"].ToString(), myDR["TotalCost"].ToString());
                        firstrow = false;
                    }
                    //RunDate, AdNumber, IssueDate, InsertTime
                    else if ((prevRunDate != myDR["RunDate"].ToString()) || (prevAdNumber != myDR["AdNumber"].ToString()))
                    {
                        writer.WriteTemplateLine("SubTotal1", null, null, null, null, null, null, null, PubRateMapInsertQuantitySubTotal, CostWithoutInsertSubTotal, PubRateMapInsertCostSubTotal, TotalCostSubTotal);

                        PubRateMapInsertQuantitySubTotal = 0;
                        CostWithoutInsertSubTotal = 0;
                        PubRateMapInsertCostSubTotal = 0;
                        TotalCostSubTotal = 0;

                        writer.WriteTemplateLine("RegRow", myDR.GetDateTime(myDR.GetOrdinal("RunDate")).ToShortDateString(), myDR.GetDateTime(myDR.GetOrdinal("IssueDate")).ToShortDateString(), myDR["AdNumber"].ToString(), myDR["InsertTime"].ToString(), myDR["Pub_ID"].ToString(), myDR["PubLoc_ID"].ToString(), myDR["PieceCost"].ToString(), myDR.GetInt32(myDR.GetOrdinal("PubRateMapInsertQuantity")), myDR["CostWithoutInsert"].ToString(), myDR["PubRateMapInsertCost"].ToString(), myDR["TotalCost"].ToString());
                    }
                    else if (prevIssueDate != myDR["IssueDate"].ToString())
                    {
                        writer.WriteTemplateLine("RegRow", null, myDR.GetDateTime(myDR.GetOrdinal("IssueDate")).ToShortDateString(), null, myDR["InsertTime"].ToString(), myDR["Pub_ID"].ToString(), myDR["PubLoc_ID"].ToString(), myDR["PieceCost"].ToString(), myDR.GetInt32(myDR.GetOrdinal("PubRateMapInsertQuantity")), myDR["CostWithoutInsert"].ToString(), myDR["PubRateMapInsertCost"].ToString(), myDR["TotalCost"].ToString());
                    }
                    else if ((prevIssueDate == myDR["IssueDate"].ToString()) && (prevInsertTime != myDR["InsertTime"].ToString()))
                    {
                        writer.WriteTemplateLine("RegRow", null, null, null, myDR["InsertTime"].ToString(), myDR["Pub_ID"].ToString(), myDR["PubLoc_ID"].ToString(), myDR["PieceCost"].ToString(), myDR.GetInt32(myDR.GetOrdinal("PubRateMapInsertQuantity")), myDR["CostWithoutInsert"].ToString(), myDR["PubRateMapInsertCost"].ToString(), myDR["TotalCost"].ToString());
                    }
                    else
                    {
                        writer.WriteTemplateLine("RegRow", null, null, null, null, myDR["Pub_ID"].ToString(), myDR["PubLoc_ID"].ToString(), myDR["PieceCost"].ToString(), myDR.GetInt32(myDR.GetOrdinal("PubRateMapInsertQuantity")), myDR["CostWithoutInsert"].ToString(), myDR["PubRateMapInsertCost"].ToString(), myDR["TotalCost"].ToString());
                    }

                    prevAdNumber = myDR["AdNumber"].ToString();
                    prevRunDate = myDR["RunDate"].ToString();
                    prevIssueDate = myDR["IssueDate"].ToString();
                    prevInsertTime = myDR["InsertTime"].ToString();
                    PubRateMapInsertQuantitySubTotal += myDR.GetInt32(myDR.GetOrdinal("PubRateMapInsertQuantity"));
                    CostWithoutInsertSubTotal += myDR.GetDecimal(myDR.GetOrdinal("CostWithoutInsert"));
                    PubRateMapInsertCostSubTotal += myDR.GetDecimal(myDR.GetOrdinal("PubRateMapInsertCost"));
                    TotalCostSubTotal += myDR.GetDecimal(myDR.GetOrdinal("TotalCost"));

                    PubRateMapInsertQuantityTotal += myDR.GetInt32(myDR.GetOrdinal("PubRateMapInsertQuantity"));
                    CostWithoutInsertTotal += myDR.GetDecimal(myDR.GetOrdinal("CostWithoutInsert"));
                    PubRateMapInsertCostTotal += myDR.GetDecimal(myDR.GetOrdinal("PubRateMapInsertCost"));
                    TotalCostTotal += myDR.GetDecimal(myDR.GetOrdinal("TotalCost"));
                }
                myDR.Close();

                writer.WriteTemplateLine("SubTotal1", null, null, null, null, null, null, null, PubRateMapInsertQuantitySubTotal, CostWithoutInsertSubTotal, PubRateMapInsertCostSubTotal, TotalCostSubTotal);
                writer.WriteTemplateLine("Total", null, null, null, null, null, null, null, PubRateMapInsertQuantityTotal, CostWithoutInsertTotal, PubRateMapInsertCostTotal, TotalCostTotal);
                writer.WriteLine(new object[] { });

                writer.WriteTemplateLine("Criteria1", "Filter and Selection Criteria", null, null, null, null);

                writer.WriteTemplateLine("Criteria2", "Run Date Range", null, null, null, null);
                if ((_dtStartRunDate.Value == null) && (_dtEndRunDate.Value == null))
                    writer.WriteTemplateLine("Criteria3", null, "All", null, null, null, null);
                else if ((_dtStartRunDate.Value != null) && (_dtEndRunDate.Value == null))
                    writer.WriteTemplateLine("Criteria3", null, string.Concat("From: ", _dtStartRunDate.Value, "    To: not entered"), null, null, null, null);
                else if ((_dtStartRunDate.Value == null) && (_dtEndRunDate.Value != null))
                    writer.WriteTemplateLine("Criteria3", null, string.Concat("From: not entered    To: ", _dtEndRunDate.Value), null, null, null, null);
                else
                    writer.WriteTemplateLine("Criteria3", null, string.Concat("From: ", _dtStartRunDate.Value, "    To: ", _dtEndRunDate.Value), null, null, null, null);

                writer.WriteTemplateLine("Criteria2", "Pub", null, null, null, null);
                if (_cboPublication.SelectedIndex > 0)
                    writer.WriteTemplateLine("Criteria3", null, _cboPublication.Text, null, null, null, null);
                else
                    writer.WriteTemplateLine("Criteria3", null, "All", null, null, null, null);

                writer.WriteTemplateLine("Criteria2", "Ad Number", null, null, null, null);
                if (_txtAdNumber.Text.Trim() != "")
                    writer.WriteTemplateLine("Criteria3", null, _txtAdNumber.Text.Trim(), null, null, null, null);
                else
                    writer.WriteTemplateLine("Criteria3", null, "All", null, null, null, null);

                writer.WriteTemplateLine("Criteria2", "Estimate ID", null, null, null, null);
                if (_txtEstimateID.Text.Trim() != "")
                    writer.WriteTemplateLine("Criteria3", null, _txtEstimateID.Text, null, null, null, null);
                else
                    writer.WriteTemplateLine("Criteria3", null, "All", null, null, null, null);

                writer.WriteTemplateLine("Criteria2", "Estimate Status", null, null, null, null);
                if (_cboEstimateStatus.SelectedIndex > 0)
                    writer.WriteTemplateLine("Criteria3", null, _cboEstimateStatus.Text, null, null, null, null);
                else
                    writer.WriteTemplateLine("Criteria3", null, "All", null, null, null, null);

                writer.SetTemplateFreezePanes("FreezePanes");
            }

            return ReportExecutionStatus.Success;
        }
    }
}

