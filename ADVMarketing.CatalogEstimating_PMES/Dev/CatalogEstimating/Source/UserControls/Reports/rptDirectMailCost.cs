using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CatalogEstimating.Datasets;
using System.Data.SqlClient;

namespace CatalogEstimating.UserControls.Reports
{
    public partial class rptDirectMailCost : CatalogEstimating.UserControls.Reports.ReportControl
    {
        public rptDirectMailCost()
            : base()
        {
            InitializeComponent();
        }

        public rptDirectMailCost(Lookup dsLookup) : base(dsLookup)
        {
            InitializeComponent();

            _cboEstimateStatus.DataSource = _dsLookup.est_status;
            _cboEstimateStatus.DisplayMember = "description";
            _cboEstimateStatus.ValueMember = "est_status_id";
        }

        public override string ReportTemplate
        {
            get
            {
                return "Direct Mail Costs";
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

            if (_dtStartRunDate.Value == null && _dtEndRunDate.Value == null && _txtHostAdNumber.Text.Trim() == String.Empty && _txtEstimateID.Text.Trim() == String.Empty)
            {
                _errorProvider.SetError( _dtStartRunDate, "Range of Run Dates, Estimate ID or Host Ad Number is required" );
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

                SqlCommand myCmd = new SqlCommand("rpt_DirectMailCosts", myConn);
                myCmd.CommandType = CommandType.StoredProcedure;
                myCmd.CommandTimeout = 600;

                if (_dtStartRunDate.Value == null)
                    myCmd.Parameters.AddWithValue("@StartRunDate", DBNull.Value);
                else
                    myCmd.Parameters.AddWithValue("@StartRunDate", _dtStartRunDate.Value);
                if (_dtEndRunDate.Value == null)
                    myCmd.Parameters.AddWithValue("@EndRunDate", DBNull.Value);
                else
                    myCmd.Parameters.AddWithValue("@EndRunDate", _dtEndRunDate.Value);
                if (_txtHostAdNumber.Text.Trim() != "")
                    myCmd.Parameters.AddWithValue("@AdNumber", _txtHostAdNumber.Text.Trim());
                else
                    myCmd.Parameters.AddWithValue("@AdNumber", DBNull.Value);
                if (_txtEstimateID.Text.Trim() != "")
                    myCmd.Parameters.AddWithValue("@EST_Estimate_ID", Convert.ToInt32(_txtEstimateID.Text));
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
                int? prevAdNumber = -1;
                int subTotalDirectMailQuantity = 0;
                decimal subTotalCostWithoutDistribution = 0;
                decimal subTotalDistributionCost = 0;
                decimal subTotalTotalCost = 0;
                int totalDirectMailQuantity = 0;
                decimal totalCostWithoutDistribution = 0;
                decimal totalDistributionCost = 0;
                decimal totalTotalCost = 0;

                writer.WriteTemplateLine("Header1a", string.Concat("Report:     ", "Direct Mail Costs"), null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header1b", string.Concat("Generated:  ", DateTime.Now.ToString()), null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header1b", null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Title1", null, null, null, null, null, null, null, null);

                while (myDR.Read())
                {
                    if (firstrow)
                    {
                        writer.WriteTemplateLine("TopRow", myDR.GetDateTime(myDR.GetOrdinal("RunDate")).ToShortDateString(), myDR["AdNumber"].ToString(), myDR["Description"].ToString(), myDR["PieceCost"].ToString(), myDR["DirectMailQuantity"].ToString(), myDR["CostWithoutDistribution"].ToString(), myDR["DistributionTotal"].ToString(), myDR["TotalCost"].ToString());
                        firstrow = false;
                    }
                    else if (((myDR.IsDBNull(myDR.GetOrdinal("AdNumber")) && prevAdNumber != null)
                        || (!myDR.IsDBNull(myDR.GetOrdinal("AdNumber")) && (prevAdNumber == null || myDR.GetInt32(myDR.GetOrdinal("AdNumber")) != prevAdNumber.Value))))
                    {
                        writer.WriteTemplateLine("SubTotal1", null, null, null, null, subTotalDirectMailQuantity, subTotalCostWithoutDistribution, subTotalDistributionCost, subTotalTotalCost);

                        subTotalDirectMailQuantity = 0;
                        subTotalCostWithoutDistribution = 0;
                        subTotalDistributionCost = 0;
                        subTotalTotalCost = 0;

                        writer.WriteTemplateLine("RegRow", myDR.GetDateTime(myDR.GetOrdinal("RunDate")).ToShortDateString(), myDR["AdNumber"].ToString(), myDR["Description"].ToString(), myDR["PieceCost"].ToString(), myDR["DirectMailQuantity"].ToString(), myDR["CostWithoutDistribution"].ToString(), myDR["DistributionTotal"].ToString(), myDR["TotalCost"].ToString());
                    }
                    else
                    {
                        writer.WriteTemplateLine("RegRow", null, null, null, myDR["PieceCost"].ToString(), myDR["DirectMailQuantity"].ToString(), myDR["CostWithoutDistribution"].ToString(), myDR["DistributionTotal"].ToString(), myDR["TotalCost"].ToString());
                    }

                    if (myDR.IsDBNull(myDR.GetOrdinal("AdNumber")))
                        prevAdNumber = null;
                    else
                        prevAdNumber = myDR.GetInt32(myDR.GetOrdinal("AdNumber"));

                    subTotalDirectMailQuantity      += myDR.GetInt32(myDR.GetOrdinal("DirectMailQuantity"));
                    subTotalCostWithoutDistribution += myDR.GetDecimal(myDR.GetOrdinal("CostWithoutDistribution"));
                    subTotalDistributionCost        += myDR.GetDecimal(myDR.GetOrdinal("DistributionTotal"));
                    subTotalTotalCost               += myDR.GetDecimal(myDR.GetOrdinal("TotalCost"));

                    totalDirectMailQuantity      += myDR.GetInt32(myDR.GetOrdinal("DirectMailQuantity"));
                    totalCostWithoutDistribution += myDR.GetDecimal(myDR.GetOrdinal("CostWithoutDistribution"));
                    totalDistributionCost        += myDR.GetDecimal(myDR.GetOrdinal("DistributionTotal"));
                    totalTotalCost               += myDR.GetDecimal(myDR.GetOrdinal("TotalCost"));
                }
                myDR.Close();

                writer.WriteTemplateLine("SubTotal1", null, null, null, null, subTotalDirectMailQuantity, subTotalCostWithoutDistribution, subTotalDistributionCost, subTotalTotalCost);
                writer.WriteTemplateLine("Total", null, null, null, null, totalDirectMailQuantity, totalCostWithoutDistribution, totalDistributionCost, totalTotalCost);
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

                writer.WriteTemplateLine("Criteria2", "Host Ad Number", null, null, null, null);
                if (_txtHostAdNumber.Text.Trim() != "")
                    writer.WriteTemplateLine("Criteria3", null, _txtHostAdNumber.Text.Trim(), null, null, null, null);
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

