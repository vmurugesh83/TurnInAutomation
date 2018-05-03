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
    public partial class rptPostageCategoryUsage : CatalogEstimating.UserControls.Reports.ReportControl
    {
        public rptPostageCategoryUsage()
            : base()
        {
            InitializeComponent();
        }

        public rptPostageCategoryUsage(Lookup dsLookup) : base(dsLookup)
        {
            InitializeComponent();

            _cboEstimateStatus.DataSource = _dsLookup.est_status;
            _cboEstimateStatus.DisplayMember = "description";
            _cboEstimateStatus.ValueMember = "est_status_id";

            _lstEstMediaType.DataSource = _dsLookup.est_estimatemediatype;
            _lstEstMediaType.DisplayMember = "description";
            _lstEstMediaType.ValueMember = "est_estimatemediatype_id";
            _lstEstMediaType.SelectedIndex = -1;

        }

        public override string ReportTemplate
        {
            get { return "Postage Category Usage"; }
        }

        private bool ValidateSearchCriteria()
        {
            _errorProvider.Clear();

            if (_dtStartRunDate.Value == null && _dtEndRunDate.Value == null && _txtEstimateID.Text == String.Empty && _txtHostAdNumber.Text == String.Empty && _txtPolybagID.Text == string.Empty)
            {
                _errorProvider.SetError(_dtStartRunDate, "Range of Run Dates, Estimate ID, Polybag ID or Host Ad Number is required");
                return false;
            }

            if (_dtStartRunDate.Value != null && _dtStartRunDate.Value > _dtEndRunDate.Value)
            {
                _errorProvider.SetError(_dtEndRunDate, "End Date must be on or after the Begin Date");
                return false;
            }

            return true;
        }

        public override ReportExecutionStatus RunReport(ExcelWriter writer)
        {
            if (!ValidateSearchCriteria())
                return ReportExecutionStatus.InvalidSearchCriteria;

            string mediatypes = string.Empty;

            // If no media types or all media types are selected, pass a null to the stored proc.  It will query all media types
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

                mediatypes = writerString.ToString();
            }

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("rpt_PostageCategoryUsage", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 600;

                if (_dtStartRunDate.Value == null)
                    cmd.Parameters.AddWithValue("@StartRunDate", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@StartRunDate", _dtStartRunDate.Value);
                if (_dtEndRunDate.Value == null)
                    cmd.Parameters.AddWithValue("@EndRunDate", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@EndRunDate", _dtEndRunDate.Value);
                if (_txtEstimateID.Text.Trim() != "")
                    cmd.Parameters.AddWithValue("@EST_Estimate_ID", Convert.ToInt64(_txtEstimateID.Text));
                else
                    cmd.Parameters.AddWithValue("@EST_Estimate_ID", DBNull.Value);
                if (_txtPolybagID.Text.Trim() != "")
                    cmd.Parameters.AddWithValue("@EST_PolybagGroup_ID", Convert.ToInt64(_txtPolybagID.Text));
                else
                    cmd.Parameters.AddWithValue("@EST_PolybagGroup_ID", DBNull.Value);
                if (_txtHostAdNumber.Text.Trim() != "")
                    cmd.Parameters.AddWithValue("@AdNumber", _txtHostAdNumber.Text.Trim());
                else
                    cmd.Parameters.AddWithValue("@AdNumber", DBNull.Value);
                if (_cboEstimateStatus.SelectedIndex > 0)
                    cmd.Parameters.AddWithValue("@EST_Status_ID", _cboEstimateStatus.SelectedValue);
                else
                    cmd.Parameters.AddWithValue("@EST_Status_ID", DBNull.Value);
                if (_radAll.Checked)
                    cmd.Parameters.AddWithValue("@VendorSupplied", 1);
                else if (_radOnlyVS.Checked)
                    cmd.Parameters.AddWithValue("@VendorSupplied", 2);
                else
                    cmd.Parameters.AddWithValue("@VendorSupplied", 3);
                if (mediatypes == string.Empty)
                    cmd.Parameters.AddWithValue("@EstimateMediaType", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@EstimateMediaType", mediatypes);

                SqlDataReader myDR = cmd.ExecuteReader();
                if (!myDR.HasRows)
                {
                    myDR.Close();
                    conn.Close();
                    return ReportExecutionStatus.NoDataReturned;
                }

                writer.WriteTemplateLine("Header1a", string.Concat("Report:     ", "Postage Category Usage"), null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header1b", string.Concat("Generated:  ", DateTime.Now.ToString()), null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header1b", null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Title1", null, null, null, null, null, null, null, null, null);

                bool bFirstRecord     = true;
                int prevMailerTypeID  = -1;
                int prevClassID       = -1;
                string prevWeightDesc = string.Empty;

                int classSubTotalQuantity    = 0;
                decimal classSubTotalAmount  = 0;
                int weightSubTotalQuantity   = 0;
                decimal weightSubTotalAmount = 0;
                int totalQuantity            = 0;
                decimal totalAmount          = 0;

                while (myDR.Read())
                {
                    // Write subtotal or footer records if the group by info has changed (except for first record)
                    if (!bFirstRecord)
                    {
                        if (myDR.GetInt32(myDR.GetOrdinal("PST_PostalMailerType_ID")) != prevMailerTypeID)
                        {
                            writer.WriteTemplateLine("WeightSubTotal", null, null, null, null, null, weightSubTotalQuantity, null, weightSubTotalAmount, null);
                            writer.WriteTemplateLine("ClassSubTotal", null, null, null, null, null, classSubTotalQuantity, null, classSubTotalAmount, null);
                            writer.WriteTemplateLine("Footer1", null, null, null, null, null, null, null, null, null);

                            classSubTotalQuantity  = 0;
                            classSubTotalAmount    = 0;
                            weightSubTotalQuantity = 0;
                            weightSubTotalAmount   = 0;
                        }
                        else if (myDR.GetInt32(myDR.GetOrdinal("PST_PostalClass_ID")) != prevClassID)
                        {
                            writer.WriteTemplateLine("WeightSubTotal", null, null, null, null, null, weightSubTotalQuantity, null, weightSubTotalAmount, null);
                            writer.WriteTemplateLine("ClassSubTotal", null, null, null, null, null, classSubTotalQuantity, null, classSubTotalAmount, null);

                            classSubTotalQuantity  = 0;
                            classSubTotalAmount    = 0;
                            weightSubTotalQuantity = 0;
                            weightSubTotalAmount   = 0;
                        }
                        else if (myDR["WeightDesc"].ToString() != prevWeightDesc)
                        {
                            writer.WriteTemplateLine("WeightSubTotal", null, null, null, null, null, weightSubTotalQuantity, null, weightSubTotalAmount, null);
                            writer.WriteLine(new object[] { });

                            weightSubTotalQuantity = 0;
                            weightSubTotalAmount   = 0;
                        }
                    }

                    bFirstRecord = false;

                    // Write Title rows if group by info has changed
                    if (myDR.GetInt32(myDR.GetOrdinal("PST_PostalMailerType_ID")) != prevMailerTypeID)
                    {
                        writer.WriteTemplateLine("MailerTypeTitle", myDR["MailerTypeDesc"].ToString(), null, null, null, null, null, null, null, null);
                        writer.WriteTemplateLine("ClassTitle", null, myDR["ClassDesc"].ToString(), null, null, null, null, null, null, null);
                        writer.WriteTemplateLine("WeightTitle", null, null, myDR["WeightDesc"].ToString(), null, null, null, null, null, null);
                    }
                    else if (myDR.GetInt32(myDR.GetOrdinal("PST_PostalClass_ID")) != prevClassID)
                    {
                        writer.WriteTemplateLine("ClassTitle", null, myDR["ClassDesc"].ToString(), null, null, null, null, null, null, null);
                        writer.WriteTemplateLine("WeightTitle", null, null, myDR["WeightDesc"].ToString(), null, null, null, null, null, null);
                    }
                    else if (myDR["WeightDesc"].ToString() != prevWeightDesc)
                    {
                        writer.WriteTemplateLine("WeightTitle", null, null, myDR["WeightDesc"].ToString(), null, null, null, null, null, null);
                    }

                    // Write the data
                    writer.WriteTemplateLine("RegRow", null, null, null, myDR["CategoryDesc"].ToString(), null, myDR["Quantity"].ToString(), myDR["PercentageQuantity"].ToString(), myDR["PostageCost"].ToString(), myDR["PercentageAmount"].ToString());

                    // Set the previous values to the current
                    prevMailerTypeID = myDR.GetInt32(myDR.GetOrdinal("PST_PostalMailerType_ID"));
                    prevClassID = myDR.GetInt32(myDR.GetOrdinal("PST_PostalClass_ID"));
                    prevWeightDesc = myDR["WeightDesc"].ToString();

                    // Add to the subtotals and totals
                    weightSubTotalQuantity += myDR.GetInt32(myDR.GetOrdinal("Quantity"));
                    weightSubTotalAmount   += myDR.GetDecimal(myDR.GetOrdinal("PostageCost"));
                    classSubTotalQuantity  += myDR.GetInt32(myDR.GetOrdinal("Quantity"));
                    classSubTotalAmount    += myDR.GetDecimal(myDR.GetOrdinal("PostageCost"));
                    totalQuantity          += myDR.GetInt32(myDR.GetOrdinal("Quantity"));
                    totalAmount            += myDR.GetDecimal(myDR.GetOrdinal("PostageCost"));
                }

                // Write Footer rows
                writer.WriteTemplateLine("WeightSubTotal", null, null, null, null, null, weightSubTotalQuantity, null, weightSubTotalAmount, null);
                writer.WriteTemplateLine("ClassSubTotal", null, null, null, null, null, classSubTotalQuantity, null, classSubTotalAmount, null);
                writer.WriteLine(new object[] { });
                writer.WriteTemplateLine("Total", null, null, null, null, null, totalQuantity, null, totalAmount, null);

                conn.Close();

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

                writer.WriteTemplateLine("Criteria2", "Estimate ID", null, null, null, null);
                if (_txtEstimateID.Text.Trim() != "")
                    writer.WriteTemplateLine("Criteria3", null, _txtEstimateID.Text, null, null, null, null);
                else
                    writer.WriteTemplateLine("Criteria3", null, "All", null, null, null, null);

                writer.WriteTemplateLine("Criteria2", "Polybag ID", null, null, null, null);
                if (_txtPolybagID.Text.Trim() != "")
                    writer.WriteTemplateLine("Criteria3", null, _txtPolybagID.Text, null, null, null, null);
                else
                    writer.WriteTemplateLine("Criteria3", null, "All", null, null, null, null);

                writer.WriteTemplateLine("Criteria2", "Host Ad Number", null, null, null, null);
                if (_txtHostAdNumber.Text.Trim() != "")
                    writer.WriteTemplateLine("Criteria3", null, _txtHostAdNumber.Text.Trim(), null, null, null, null);
                else
                    writer.WriteTemplateLine("Criteria3", null, "All", null, null, null, null);

                writer.WriteTemplateLine("Criteria2", "Estimate Status", null, null, null, null);
                if (_cboEstimateStatus.SelectedIndex > 0)
                    writer.WriteTemplateLine("Criteria3", null, _cboEstimateStatus.Text, null, null, null, null);
                else
                    writer.WriteTemplateLine("Criteria3", null, "All", null, null, null, null);

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

                writer.SetTemplateFreezePanes("FreezePanes");
            }

            return ReportExecutionStatus.Success;
        }
    }
}

