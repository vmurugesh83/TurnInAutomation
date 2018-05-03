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
    public partial class rptVendorCommitment : CatalogEstimating.UserControls.Reports.ReportControl
    {
        Dictionary<int, string> _CostCodes = new Dictionary<int, string>();

        public rptVendorCommitment()
            : base()
        {
            InitializeComponent();
        }

        public rptVendorCommitment(Lookup dsLookup) : base(dsLookup)
        {
            InitializeComponent();

            DataView dvVendor = new DataView(_dsLookup.CurrentVendors);
            dvVendor.RowFilter = "vnd_vendor_id <> -1";
            _lstVendor.DataSource = dvVendor;
            _lstVendor.DisplayMember = "description";
            _lstVendor.ValueMember = "vnd_vendor_id";
            _lstVendor.SelectedIndex = -1;

            _lstPublisher.DataSource = _dsLookup.PublicationList;
            _lstPublisher.DisplayMember = "pub_id";
            _lstPublisher.ValueMember = "pub_id";
            _lstPublisher.SelectedIndex = -1;

            _CostCodes.Add(530, "530 - Creative / Photography");
            _CostCodes.Add(595, "595 - Color Separations");
            _CostCodes.Add(610, "610 - Print");
            _CostCodes.Add(605, "605 - Paper");
            _CostCodes.Add(606, "606 - Paper Handling");
            _CostCodes.Add(615, "615 - Sales Tax");
            _CostCodes.Add(760, "760 - Mail List");
            _CostCodes.Add(880, "880 - Specialty Other");
            _CostCodes.Add(870, "870 - VS Production");
            _CostCodes.Add(730, "730 - Polybag");
            _CostCodes.Add(720, "720 - Stitch-Ins / Blow-Ins");
            _CostCodes.Add(745, "745 - Ink Jet");
            _CostCodes.Add(740, "740 - Handling");
            _CostCodes.Add(820, "820 - Postal Drop");
            _CostCodes.Add(830, "830 - Newspaper Freight");
            _CostCodes.Add(810, "810 - Sample Shipping");
            _CostCodes.Add(855, "855 - Other Distribution");
            _CostCodes.Add(750, "750 - Mail Tracking");
            _CostCodes.Add(840, "840 - Postage");
            _CostCodes.Add(850, "850 - Insertion");

            _lstCostCodes.DataSource = new BindingSource(_CostCodes, null);
            _lstCostCodes.DisplayMember = "Value";
            _lstCostCodes.ValueMember = "Key";
            _lstCostCodes.SelectedIndex = -1;

            _cboEstimateStatus.DataSource = _dsLookup.est_status;
            _cboEstimateStatus.DisplayMember = "description";
            _cboEstimateStatus.ValueMember = "est_status_id";
        }

        public override string ReportTemplate
        {
            get
            {
                return "Vendor Commitment";
            }
        }

        protected bool ValidateSearchCriteria()
        {
            _errorProvider.Clear();

            if (_dtStartRunDate.Value == null && _dtEndRunDate.Value == null && _txtEstimateID.Text == String.Empty && _txtHostAdNumber.Text == String.Empty)
            {
                _errorProvider.SetError(_dtStartRunDate, "Range of Run Dates, Estimate ID or Host Ad Number is required");
                return false;
            }

            if (_dtStartRunDate.Value != null && _dtStartRunDate.Value > _dtEndRunDate.Value)
            {
                _errorProvider.SetError(_dtEndRunDate, "End Date must be on or after the Begin Date");
                return false;
            }

            return true;
        }

        protected void FillVendorCommand(SqlCommand myCmd)
        {
            string vendors = string.Empty;

            if (_lstVendor.SelectedItems.Count != _lstVendor.Items.Count)
            {
                StringWriter writerString = new StringWriter();
                XmlTextWriter writerXml = new XmlTextWriter(writerString);
                writerXml.WriteStartElement("root");
                for (int i = 0; i < _lstVendor.SelectedItems.Count; ++i)
                {
                    Lookup.CurrentVendorsRow r = (Lookup.CurrentVendorsRow)((DataRowView)_lstVendor.SelectedItems[i]).Row;
                    writerXml.WriteStartElement("vnd_vendor");
                    writerXml.WriteAttributeString("vnd_vendor_id", r.vnd_vendor_id.ToString());
                    writerXml.WriteEndElement();
                }
                writerXml.WriteEndElement();

                vendors = writerString.ToString();
            }

            if (_dtStartRunDate.Value == null)
                myCmd.Parameters.AddWithValue("@StartRunDate", DBNull.Value);
            else
                myCmd.Parameters.AddWithValue("@StartRunDate", _dtStartRunDate.Value.Value.Date);
            if (_dtEndRunDate.Value == null)
                myCmd.Parameters.AddWithValue("@EndRunDate", DBNull.Value);
            else
                myCmd.Parameters.AddWithValue("@EndRunDate", _dtEndRunDate.Value.Value.Date);
            if (_txtEstimateID.Text != String.Empty)
                myCmd.Parameters.AddWithValue("@EST_Estimate_ID", _txtEstimateID.Text);
            else
                myCmd.Parameters.AddWithValue("@EST_Estimate_ID", DBNull.Value);
            if (_txtHostAdNumber.Text.Trim() != "")
                myCmd.Parameters.AddWithValue("@AdNumber", _txtHostAdNumber.Text.Trim());
            else
                myCmd.Parameters.AddWithValue("@AdNumber", DBNull.Value);
            if (_cboEstimateStatus.SelectedIndex > 0)
                myCmd.Parameters.AddWithValue("@EST_Status_ID", _cboEstimateStatus.SelectedValue);
            else
                myCmd.Parameters.AddWithValue("@EST_Status_ID", DBNull.Value);
            if (vendors != String.Empty)
                myCmd.Parameters.AddWithValue("@Vendors", vendors);
            else
                myCmd.Parameters.AddWithValue("@Vendors", DBNull.Value);
        }

        protected void FillPublisherCommand(SqlCommand myCmd)
        {
            string pubs = string.Empty;

            // If all pubs are selected, don't create the XML pub list, the stored proc will handle it
            if (_lstPublisher.SelectedItems.Count != _lstPublisher.Items.Count)
            {
                StringWriter writerString = new StringWriter();
                XmlTextWriter writerXml = new XmlTextWriter(writerString);
                writerXml.WriteStartElement("root");
                for (int i = 0; i < _lstPublisher.SelectedItems.Count; ++i)
                {
                    Lookup.PublicationListRow r = (Lookup.PublicationListRow)((DataRowView)_lstPublisher.SelectedItems[i]).Row;
                    writerXml.WriteStartElement("pub");
                    writerXml.WriteAttributeString("pub_id", r.pub_id.ToString());
                    writerXml.WriteEndElement();
                }
                writerXml.WriteEndElement();

                pubs = writerString.ToString();
            }

            if (_dtStartRunDate.Value == null)
                myCmd.Parameters.AddWithValue("@StartRunDate", DBNull.Value);
            else
                myCmd.Parameters.AddWithValue("@StartRunDate", _dtStartRunDate.Value.Value.Date);
            if (_dtEndRunDate.Value == null)
                myCmd.Parameters.AddWithValue("@EndRunDate", DBNull.Value);
            else
                myCmd.Parameters.AddWithValue("@EndRunDate", _dtEndRunDate.Value.Value.Date);
            if (_txtEstimateID.Text != String.Empty)
                myCmd.Parameters.AddWithValue("@EST_Estimate_ID", _txtEstimateID.Text);
            else
                myCmd.Parameters.AddWithValue("@EST_Estimate_ID", DBNull.Value);
            if (_txtHostAdNumber.Text.Trim() != "")
                myCmd.Parameters.AddWithValue("@AdNumber", _txtHostAdNumber.Text.Trim());
            else
                myCmd.Parameters.AddWithValue("@AdNumber", DBNull.Value);
            if (_cboEstimateStatus.SelectedIndex > 0)
                myCmd.Parameters.AddWithValue("@EST_Status_ID", _cboEstimateStatus.SelectedValue);
            else
                myCmd.Parameters.AddWithValue("@EST_Status_ID", DBNull.Value);
            if (pubs != String.Empty)
                myCmd.Parameters.AddWithValue("@Pubs", pubs);
            else
                myCmd.Parameters.AddWithValue("@Pubs", DBNull.Value);
        }

        protected void FillPolybagCommand(SqlCommand myCmd)
        {
            string vendors = string.Empty;

            if (_lstVendor.SelectedItems.Count != _lstVendor.Items.Count)
            {
                StringWriter writerString = new StringWriter();
                XmlTextWriter writerXml = new XmlTextWriter(writerString);
                writerXml.WriteStartElement("root");
                for (int i = 0; i < _lstVendor.SelectedItems.Count; ++i)
                {
                    Lookup.CurrentVendorsRow r = (Lookup.CurrentVendorsRow)((DataRowView)_lstVendor.SelectedItems[i]).Row;
                    writerXml.WriteStartElement("vnd_vendor");
                    writerXml.WriteAttributeString("vnd_vendor_id", r.vnd_vendor_id.ToString());
                    writerXml.WriteEndElement();
                }
                writerXml.WriteEndElement();

                vendors = writerString.ToString();
            }

            if (_dtStartRunDate.Value == null)
                myCmd.Parameters.AddWithValue("@StartRunDate", DBNull.Value);
            else
                myCmd.Parameters.AddWithValue("@StartRunDate", _dtStartRunDate.Value.Value.Date);
            if (_dtEndRunDate.Value == null)
                myCmd.Parameters.AddWithValue("@EndRunDate", DBNull.Value);
            else
                myCmd.Parameters.AddWithValue("@EndRunDate", _dtEndRunDate.Value.Value.Date);
            if (_txtEstimateID.Text != String.Empty)
                myCmd.Parameters.AddWithValue("@EST_Estimate_ID", _txtEstimateID.Text);
            else
                myCmd.Parameters.AddWithValue("@EST_Estimate_ID", DBNull.Value);
            if (_txtHostAdNumber.Text.Trim() != "")
                myCmd.Parameters.AddWithValue("@AdNumber", _txtHostAdNumber.Text.Trim());
            else
                myCmd.Parameters.AddWithValue("@AdNumber", DBNull.Value);
            if (_cboEstimateStatus.SelectedIndex > 0)
                myCmd.Parameters.AddWithValue("@EST_Status_ID", _cboEstimateStatus.SelectedValue);
            else
                myCmd.Parameters.AddWithValue("@EST_Status_ID", DBNull.Value);
            if (vendors != String.Empty)
                myCmd.Parameters.AddWithValue("@Vendors", vendors);
            else
                myCmd.Parameters.AddWithValue("@Vendors", DBNull.Value);
        }

        private DataTable GetVendorData()
        {
            // Create DataTable to return
            DataTable dtVendorQueryData = new DataTable();
            dtVendorQueryData.Columns.Add("VND_Vendor_ID");
            dtVendorQueryData.Columns.Add("VendorDescription");
            dtVendorQueryData.Columns.Add("RunDate");
            dtVendorQueryData.Columns.Add("AdNumber");
            dtVendorQueryData.Columns.Add("AdDescription");
            dtVendorQueryData.Columns.Add("PageCount");
            dtVendorQueryData.Columns.Add("MediaQuantity");
            dtVendorQueryData.Columns.Add("CostCode");
            dtVendorQueryData.Columns.Add("CostCodeDescription");
            dtVendorQueryData.Columns.Add("GrossCost");
            dtVendorQueryData.Columns.Add("Discount");
            dtVendorQueryData.Columns.Add("NetCost");

            // If all vendors, publishers and cost codes are selected make sure to report ALL costs
            bool bIncludeAllCosts = false;
            if (_lstVendor.SelectedItems.Count == _lstVendor.Items.Count
                && _lstPublisher.SelectedItems.Count == _lstPublisher.Items.Count
                && _lstCostCodes.SelectedItems.Count == _lstCostCodes.Items.Count)
            {
                bIncludeAllCosts = true;
            }

            // Create List of Vendor ID's
            Dictionary<long, long> selectedVendors = new Dictionary<long, long>();

            for (int i = 0; i < _lstVendor.SelectedItems.Count; ++i)
            {
                Lookup.CurrentVendorsRow vr_row = (Lookup.CurrentVendorsRow) ((DataRowView) _lstVendor.SelectedItems[i]).Row;
                selectedVendors.Add(vr_row.vnd_vendor_id, vr_row.vnd_vendor_id);
            }

            // Create List of Cost Codes
            Dictionary<int, string> selectedCostCodes = new Dictionary<int, string>();
            for (int i = 0; i < _lstCostCodes.SelectedItems.Count; ++i)
            {
                KeyValuePair<int, string> curCostCode = (KeyValuePair<int, string>)_lstCostCodes.SelectedItems[i];
                selectedCostCodes.Add(curCostCode.Key, curCostCode.Value);
            }

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {

                conn.Open();
                SqlCommand myCmd = new SqlCommand("rpt_VendorCommitment_Vendor", conn);
                myCmd.CommandType = CommandType.StoredProcedure;
                myCmd.CommandTimeout = 600;

                FillVendorCommand(myCmd);

                SqlDataReader myDR = myCmd.ExecuteReader();

                while (myDR.Read())
                {
                    #region Fill dtVendorQueryData
                    // 530 - Creative / Photography
                    if (selectedCostCodes.ContainsKey(530) && !myDR.IsDBNull(myDR.GetOrdinal("CreativeVendor_ID")) && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("CreativeVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["CreativeVendor_ID"].ToString(),
                            myDR["CreativeVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "530",
                            "Creative / Photography",
                            myDR["CreativeCost"].ToString(),
                            string.Empty,
                            myDR["CreativeCost"].ToString());
                    }

                    // 595 - Color Separations
                    if (selectedCostCodes.ContainsKey(595) && !myDR.IsDBNull(myDR.GetOrdinal("SeparatorVendor_ID")) && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("SeparatorVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["SeparatorVendor_ID"].ToString(),
                            myDR["SeparatorVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "595",
                            "Color Separations",
                            myDR["SeparatorCost"].ToString(),
                            string.Empty,
                            myDR["SeparatorCost"].ToString());
                    }

                    // 610 - Print
                    if (selectedCostCodes.ContainsKey(610) && !myDR.IsDBNull(myDR.GetOrdinal("PrinterVendor_ID")) && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("PrinterVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["PrinterVendor_ID"].ToString(),
                            myDR["PrinterVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "610",
                            "Print",
                            myDR["GrossPrintCost"].ToString(),
                            myDR["EarlyPayPrintDiscountAmount"].ToString(),
                            myDR["NetPrintCost"].ToString());
                    }

                    // 605 - Paper
                    if (selectedCostCodes.ContainsKey(605) && !myDR.IsDBNull(myDR.GetOrdinal("PaperVendor_ID")) && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("PaperVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["PaperVendor_ID"].ToString(),
                            myDR["PaperVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "605",
                            "Paper",
                            myDR["GrossPaperCost"].ToString(),
                            myDR["EarlyPayPaperDiscountAmount"].ToString(),
                            myDR["NetPaperCost"].ToString());
                    }

                    // 606 - Paper Handling
                    if (selectedCostCodes.ContainsKey(606) && !myDR.IsDBNull(myDR.GetOrdinal("PaperVendor_ID")) && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("PaperVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["PaperVendor_ID"].ToString(),
                            myDR["PaperVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "606",
                            "Paper Handling",
                            myDR["PaperHandlingCost"].ToString(),
                            string.Empty,
                            myDR["PaperHandlingCost"].ToString());
                    }

                    // 615 - Sales Tax (Printer)
                    if (selectedCostCodes.ContainsKey(615) && !myDR.IsDBNull(myDR.GetOrdinal("PrinterVendor_ID")) && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("PrinterVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["PrinterVendor_ID"].ToString(),
                            myDR["PrinterVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "615",
                            "Sales Tax",
                            myDR["PrinterSalesTaxAmount"].ToString(),
                            string.Empty,
                            myDR["PrinterSalesTaxAmount"].ToString());
                    }

                    // 615 - Sales Tax (Paper)
                    if (selectedCostCodes.ContainsKey(615) && !myDR.IsDBNull(myDR.GetOrdinal("PaperVendor_ID")) && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("PaperVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["PaperVendor_ID"].ToString(),
                            myDR["PaperVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "615",
                            "Sales Tax",
                            myDR["PaperSalesTaxAmount"].ToString(),
                            string.Empty,
                            myDR["PaperSalesTaxAmount"].ToString());
                    }

                    // 760 - Mail List (Internal)
                    if (selectedCostCodes.ContainsKey(760) && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("MailListResourceVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["MailListResourceVendor_ID"].ToString(),
                            myDR["MailListResourceVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "760",
                            "Mail List",
                            myDR["MailListCost"].ToString(),
                            string.Empty,
                            myDR["MailListCost"].ToString());
                    }

                    // 880 - Specialty Other
                    if (bIncludeAllCosts)
                    {
                        if (myDR.IsDBNull(myDR.GetOrdinal("PrinterVendor_ID")))
                        {
                            dtVendorQueryData.Rows.Add(-1,
                                "N/A",
                                myDR["RunDate"].ToString(),
                                myDR["AdNumber"].ToString(),
                                myDR["Description"].ToString(),
                                myDR["PageCount"].ToString(),
                                myDR["MediaQuantity"].ToString(),
                                "880",
                                "Specialty Other",
                                myDR["OtherProductionCost"].ToString(),
                                string.Empty,
                                myDR["OtherProductionCost"].ToString());
                        }
                        else
                        {
                            dtVendorQueryData.Rows.Add(myDR["PrinterVendor_ID"].ToString(),
                                myDR["PrinterVendor"].ToString(),
                                myDR["RunDate"].ToString(),
                                myDR["AdNumber"].ToString(),
                                myDR["Description"].ToString(),
                                myDR["PageCount"].ToString(),
                                myDR["MediaQuantity"].ToString(),
                                "880",
                                "Specialty Other",
                                myDR["OtherProductionCost"].ToString(),
                                string.Empty,
                                myDR["OtherProductionCost"].ToString());
                        }
                    }
                    else if (!myDR.IsDBNull(myDR.GetOrdinal("PrinterVendor_ID"))
                        && selectedCostCodes.ContainsKey(880)
                        && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("PrinterVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["PrinterVendor_ID"].ToString(),
                            myDR["PrinterVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "880",
                            "Specialty Other",
                            myDR["OtherProductionCost"].ToString(),
                            string.Empty,
                            myDR["OtherProductionCost"].ToString());
                    }

                    // 870 - VS Production
                    if (selectedCostCodes.ContainsKey(870) && !myDR.IsDBNull(myDR.GetOrdinal("VendorSupplied_ID")) && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("VendorSupplied_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["VendorSupplied_ID"].ToString(),
                            myDR["VendorSuppliedDesc"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "870",
                            "VS Production",
                            myDR["VendorProductionCost"].ToString(),
                            string.Empty,
                            myDR["VendorProductionCost"].ToString());
                    }

                    // 730 - Polybag (Onserts)
                    if (selectedCostCodes.ContainsKey(730) && !myDR.IsDBNull(myDR.GetOrdinal("OnsertVendor_ID")) && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("OnsertVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["OnsertVendor_ID"].ToString(),
                            myDR["OnsertVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "730",
                            "Polybag",
                            myDR["OnsertCost"].ToString(),
                            string.Empty,
                            myDR["OnsertCost"].ToString());
                    }

                    // 720 - Stitch-Ins / Blow-Ins (Stitch-Ins)
                    if (selectedCostCodes.ContainsKey(720) && !myDR.IsDBNull(myDR.GetOrdinal("AssemblyVendor_ID")) && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("AssemblyVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["AssemblyVendor_ID"].ToString(),
                            myDR["AssemblyVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "720",
                            "Stitch-Ins / Blow-Ins",
                            myDR["StitchInCost"].ToString(),
                            string.Empty,
                            myDR["StitchInCost"].ToString());
                    }

                    // 720 - Stitch-Ins / Blow-Ins (Blow-Ins)
                    if (selectedCostCodes.ContainsKey(720) && !myDR.IsDBNull(myDR.GetOrdinal("AssemblyVendor_ID")) && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("AssemblyVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["AssemblyVendor_ID"].ToString(),
                            myDR["AssemblyVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "720",
                            "Stitch-Ins / Blow-Ins",
                            myDR["BlowInCost"].ToString(),
                            string.Empty,
                            myDR["BlowInCost"].ToString());
                    }

                    // 745 - Ink Jet
                    if (selectedCostCodes.ContainsKey(745) && !myDR.IsDBNull(myDR.GetOrdinal("MailHouseVendor_ID")) && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("MailHouseVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["MailHouseVendor_ID"].ToString(),
                            myDR["MailHouseVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "745",
                            "Ink Jet",
                            myDR["TotalInkJetCost"].ToString(),
                            string.Empty,
                            myDR["TotalInkJetCost"].ToString());
                    }

                    // 740 - Handling (Mail House)
                    if (selectedCostCodes.ContainsKey(740) && !myDR.IsDBNull(myDR.GetOrdinal("MailHouseVendor_ID")) && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("MailHouseVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["MailHouseVendor_ID"].ToString(),
                            myDR["MailHouseVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "740",
                            "Handling",
                            myDR["MailHandlingTotal"].ToString(),
                            string.Empty,
                            myDR["MailHandlingTotal"].ToString());
                    }

                    // 740 - Handling (Insert)
                    if (selectedCostCodes.ContainsKey(740) && !myDR.IsDBNull(myDR.GetOrdinal("PrinterVendor_ID")) && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("PrinterVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["PrinterVendor_ID"].ToString(),
                            myDR["PrinterVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "740",
                            "Handling",
                            myDR["InsertHandlingTotal"].ToString(),
                            string.Empty,
                            myDR["InsertHandlingTotal"].ToString());
                    }

                    // 820 - Postal Drop
                    if (bIncludeAllCosts)
                    {
                        if (myDR.IsDBNull(myDR.GetOrdinal("PrinterVendor_ID")))
                        {
                            dtVendorQueryData.Rows.Add(-1,
                                "N/A",
                                myDR["RunDate"].ToString(),
                                myDR["AdNumber"].ToString(),
                                myDR["Description"].ToString(),
                                myDR["PageCount"].ToString(),
                                myDR["MediaQuantity"].ToString(),
                                "820",
                                "Postal Drop",
                                myDR["TotalPostalDropCost"].ToString(),
                                string.Empty,
                                myDR["TotalPostalDropCost"].ToString());
                        }
                        else
                        {
                            dtVendorQueryData.Rows.Add(myDR["PrinterVendor_ID"].ToString(),
                                myDR["PrinterVendor"].ToString(),
                                myDR["RunDate"].ToString(),
                                myDR["AdNumber"].ToString(),
                                myDR["Description"].ToString(),
                                myDR["PageCount"].ToString(),
                                myDR["MediaQuantity"].ToString(),
                                "820",
                                "Postal Drop",
                                myDR["TotalPostalDropCost"].ToString(),
                                string.Empty,
                                myDR["TotalPostalDropCost"].ToString());
                        }
                    }
                    else if (!myDR.IsDBNull(myDR.GetOrdinal("PrinterVendor_ID"))
                        && selectedCostCodes.ContainsKey(820)
                        && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("PrinterVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["PrinterVendor_ID"].ToString(),
                            myDR["PrinterVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "820",
                            "Postal Drop",
                            myDR["TotalPostalDropCost"].ToString(),
                            string.Empty,
                            myDR["TotalPostalDropCost"].ToString());
                    }


                    // 830 - Newspaper Freight
                    if (selectedCostCodes.ContainsKey(830) && !myDR.IsDBNull(myDR.GetOrdinal("PrinterVendor_ID")) && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("PrinterVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["PrinterVendor_ID"].ToString(),
                            myDR["PrinterVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "830",
                            "Newspaper Freight",
                            myDR["InsertFreightTotalCost"].ToString(),
                            string.Empty,
                            myDR["InsertFreightTotalCost"].ToString());
                    }

                    // 810 - Sample Shipping
                    if (bIncludeAllCosts)
                    {
                        if (myDR.IsDBNull(myDR.GetOrdinal("SampleFreightVendor_ID")))
                        {
                            dtVendorQueryData.Rows.Add(-1,
                                "N/A",
                                myDR["RunDate"].ToString(),
                                myDR["AdNumber"].ToString(),
                                myDR["Description"].ToString(),
                                myDR["PageCount"].ToString(),
                                myDR["MediaQuantity"].ToString(),
                                "810",
                                "Sample Shipping",
                                myDR["SampleFreight"].ToString(),
                                string.Empty,
                                myDR["SampleFreight"].ToString());
                        }
                        else
                        {
                            dtVendorQueryData.Rows.Add(myDR["SampleFreightVendor_ID"].ToString(),
                                myDR["SampleFreightVendor"].ToString(),
                                myDR["RunDate"].ToString(),
                                myDR["AdNumber"].ToString(),
                                myDR["Description"].ToString(),
                                myDR["PageCount"].ToString(),
                                myDR["MediaQuantity"].ToString(),
                                "810",
                                "Sample Shipping",
                                myDR["SampleFreight"].ToString(),
                                string.Empty,
                                myDR["SampleFreight"].ToString());
                        }
                    }
                    else if (!myDR.IsDBNull(myDR.GetOrdinal("SampleFreightVendor_ID"))
                        && selectedCostCodes.ContainsKey(810)
                        && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("SampleFreightVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["SampleFreightVendor_ID"].ToString(),
                            myDR["SampleFreightVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "810",
                            "Sample Shipping",
                            myDR["SampleFreight"].ToString(),
                            string.Empty,
                            myDR["SampleFreight"].ToString());
                    }

                    // 855 - Other Distribution
                    if (bIncludeAllCosts)
                    {
                        if (myDR.IsDBNull(myDR.GetOrdinal("PrinterVendor_ID")))
                        {
                            dtVendorQueryData.Rows.Add(-1,
                                "N/A",
                                myDR["RunDate"].ToString(),
                                myDR["AdNumber"].ToString(),
                                myDR["Description"].ToString(),
                                myDR["PageCount"].ToString(),
                                myDR["MediaQuantity"].ToString(),
                                "855",
                                "Other Distribution",
                                myDR["OtherFreight"].ToString(),
                                string.Empty,
                                myDR["OtherFreight"].ToString());
                        }
                        else
                        {
                            dtVendorQueryData.Rows.Add(myDR["PrinterVendor_ID"].ToString(),
                                myDR["PrinterVendor"].ToString(),
                                myDR["RunDate"].ToString(),
                                myDR["AdNumber"].ToString(),
                                myDR["Description"].ToString(),
                                myDR["PageCount"].ToString(),
                                myDR["MediaQuantity"].ToString(),
                                "855",
                                "Other Distribution",
                                myDR["OtherFreight"].ToString(),
                                string.Empty,
                                myDR["OtherFreight"].ToString());
                        }
                    }
                    else if (!myDR.IsDBNull(myDR.GetOrdinal("PrinterVendor_ID"))
                        && selectedCostCodes.ContainsKey(855)
                        && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("PrinterVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["PrinterVendor_ID"].ToString(),
                            myDR["PrinterVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "855",
                            "Other Distribution",
                            myDR["OtherFreight"].ToString(),
                            string.Empty,
                            myDR["OtherFreight"].ToString());
                    }


                    // 750 - Mail Tracking
                    if (selectedCostCodes.ContainsKey(750) && !myDR.IsDBNull(myDR.GetOrdinal("MailTrackingVendor_ID")) && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("MailTrackingVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["MailTrackingVendor_ID"].ToString(),
                            myDR["MailTrackingVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "750",
                            "Mail Tracking",
                            myDR["MailTrackingCost"].ToString(),
                            string.Empty,
                            myDR["MailTrackingCost"].ToString());
                    }

                    // 840 - Postage
                    if (selectedCostCodes.ContainsKey(840) && !myDR.IsDBNull(myDR.GetOrdinal("PostalVendor_ID")) && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("PostalVendor_ID"))))
                    {
                        dtVendorQueryData.Rows.Add(myDR["PostalVendor_ID"].ToString(),
                            myDR["PostalVendor"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["Description"].ToString(),
                            myDR["PageCount"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "840",
                            "Postal",
                            myDR["TotalPostageCost"].ToString(),
                            string.Empty,
                            myDR["TotalPostageCost"].ToString());
                    }
                    #endregion
                }
                myDR.Close();
                conn.Close();
            }

            return dtVendorQueryData;
        }

        private DataTable GetPublisherData()
        {
            // Create DataTable to return
            DataTable dtPublisherQueryData = new DataTable();
            dtPublisherQueryData.Columns.Add("Pub_ID");
            dtPublisherQueryData.Columns.Add("RunDate");
            dtPublisherQueryData.Columns.Add("AdNumber");
            dtPublisherQueryData.Columns.Add("AdDescription");
            dtPublisherQueryData.Columns.Add("PageCount");
            dtPublisherQueryData.Columns.Add("MediaQuantity");
            dtPublisherQueryData.Columns.Add("InsertQuantity");
            dtPublisherQueryData.Columns.Add("CostCode");
            dtPublisherQueryData.Columns.Add("CostCodeDescription");
            dtPublisherQueryData.Columns.Add("GrossCost");
            dtPublisherQueryData.Columns.Add("Discount");
            dtPublisherQueryData.Columns.Add("NetCost");

            // Create List of Pub ID's
            List<string> selectedPublishers = new List<string>();

            for (int i = 0; i < _lstPublisher.SelectedItems.Count; ++i)
            {
                Lookup.PublicationListRow p_row = (Lookup.PublicationListRow) ((DataRowView)_lstPublisher.SelectedItems[i]).Row;
                selectedPublishers.Add(p_row.pub_id);
            }

            // Create List of Cost Codes
            Dictionary<int, string> selectedCostCodes = new Dictionary<int, string>();
            for (int i = 0; i < _lstCostCodes.SelectedItems.Count; ++i)
            {
                KeyValuePair<int, string> curCostCode = (KeyValuePair<int, string>)_lstCostCodes.SelectedItems[i];
                selectedCostCodes.Add(curCostCode.Key, curCostCode.Value);
            }

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                SqlCommand myCmd = new SqlCommand("rpt_VendorCommitment_Publisher", conn);
                myCmd.CommandType = CommandType.StoredProcedure;
                myCmd.CommandTimeout = 600;

                FillPublisherCommand(myCmd);

                SqlDataReader myDR = myCmd.ExecuteReader();

                while (myDR.Read())
                {
                    // 850 - Insertion
                    if (selectedCostCodes.ContainsKey(850) && selectedPublishers.Contains(myDR["Pub_ID"].ToString()))
                    {
                        if (selectedPublishers.Contains(myDR["Pub_ID"].ToString()))
                        {
                            dtPublisherQueryData.Rows.Add(myDR["Pub_ID"].ToString(),
                                myDR["RunDate"].ToString(),
                                myDR["AdNumber"].ToString(),
                                myDR["Description"].ToString(),
                                myDR["PageCount"].ToString(),
                                myDR["MediaQuantity"].ToString(),
                                myDR["InsertQuantity"].ToString(),
                                "850",
                                "Insertion",
                                myDR["GrossInsertCost"].ToString(),
                                myDR["InsertDiscount"].ToString(),
                                myDR["NetInsertCost"].ToString());
                        }
                    }
                }
                myDR.Close();
                conn.Close();
            }

            return dtPublisherQueryData;
        }

        private DataTable GetPolybagData()
        {
            // Create DataTable to return
            DataTable dtPolybagQueryData = new DataTable();
            dtPolybagQueryData.Columns.Add("VND_Vendor_ID");
            dtPolybagQueryData.Columns.Add("VendorDescription");
            dtPolybagQueryData.Columns.Add("RunDate");
            dtPolybagQueryData.Columns.Add("AdNumber");
            dtPolybagQueryData.Columns.Add("AdDescription");
            dtPolybagQueryData.Columns.Add("PageCount");
            dtPolybagQueryData.Columns.Add("MediaQuantity");
            dtPolybagQueryData.Columns.Add("CostCode");
            dtPolybagQueryData.Columns.Add("CostCodeDescription");
            dtPolybagQueryData.Columns.Add("GrossCost");
            dtPolybagQueryData.Columns.Add("Discount");
            dtPolybagQueryData.Columns.Add("NetCost");

            // Create List of Vendor ID's
            Dictionary<long, long> selectedVendors = new Dictionary<long, long>();

            for (int i = 0; i < _lstVendor.SelectedItems.Count; ++i)
            {
                Lookup.CurrentVendorsRow vr_row = (Lookup.CurrentVendorsRow)((DataRowView)_lstVendor.SelectedItems[i]).Row;
                selectedVendors.Add(vr_row.vnd_vendor_id, vr_row.vnd_vendor_id);
            }

            // Create List of Cost Codes
            Dictionary<int, string> selectedCostCodes = new Dictionary<int, string>();
            for (int i = 0; i < _lstCostCodes.SelectedItems.Count; ++i)
            {
                KeyValuePair<int, string> curCostCode = (KeyValuePair<int, string>)_lstCostCodes.SelectedItems[i];
                selectedCostCodes.Add(curCostCode.Key, curCostCode.Value);
            }

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {

                conn.Open();
                SqlCommand myCmd = new SqlCommand("rpt_VendorCommitment_Polybag", conn);
                myCmd.CommandType = CommandType.StoredProcedure;
                myCmd.CommandTimeout = 600;

                FillPolybagCommand(myCmd);

                SqlDataReader myDR = myCmd.ExecuteReader();

                while (myDR.Read())
                {
                    #region Fill dtPolybagQueryData

                    // 730 - Polybag
                    if (selectedCostCodes.ContainsKey(730) && !myDR.IsDBNull(myDR.GetOrdinal("PolybagVendor_ID")) && selectedVendors.ContainsKey(myDR.GetInt64(myDR.GetOrdinal("PolybagVendor_ID"))))
                    {
                        dtPolybagQueryData.Rows.Add(myDR["PolybagVendor_ID"].ToString(),
                            myDR["VendorDescription"].ToString(),
                            myDR["RunDate"].ToString(),
                            myDR["AdNumber"].ToString(),
                            myDR["AdDescription"].ToString(),
                            myDR["Pages"].ToString(),
                            myDR["MediaQuantity"].ToString(),
                            "730",
                            "Polybag",
                            myDR["GrossCost"].ToString(),
                            string.Empty,
                            myDR["NetCost"].ToString());
                    }

                    #endregion
                }
                myDR.Close();
                conn.Close();
            }

            return dtPolybagQueryData;
        }

        public override ReportExecutionStatus RunReport(ExcelWriter writer)
        {
            if (!ValidateSearchCriteria())
                return ReportExecutionStatus.InvalidSearchCriteria;

            DataTable dtVendorQueryData = null;
            DataTable dtPublisherQueryData = null;
            DataTable dtPolybagQueryData = null;

            if (_lstVendor.SelectedItems.Count > 0)
            {
                PerformanceLog.Start("Vendor Commitment Report - GetVendorData");
                dtVendorQueryData = GetVendorData();
                PerformanceLog.End("Vendor Commitment Report - GetVendorData");

                PerformanceLog.Start("Vendor Commitment Report - GetPolybagData");
                dtPolybagQueryData = GetPolybagData();
                PerformanceLog.End("Vendor Commitment Report - GetPolybagData");
            }

            if (_lstPublisher.SelectedItems.Count > 0)
            {
                PerformanceLog.Start("Vendor Commitment Report - GetPublisherData");
                dtPublisherQueryData = GetPublisherData();
                PerformanceLog.End("Vendor Commitment Report - GetPublisherData");
            }

            // Remove any rows with zero cost
            // TODO: Possibly use the .Select command to remove rows more efficiently
            PerformanceLog.Start("Vendor Commitment Report - Group Costs");
            #region Group Costs by Vendor, Run Date, Ad#, Cost Code
            if (dtVendorQueryData != null)
            {
                for (int i = (dtVendorQueryData.Rows.Count - 1); i >= 0; --i)
                {
                    if (
                        (string.IsNullOrEmpty(dtVendorQueryData.Rows[i]["GrossCost"].ToString()) || Convert.ToDecimal(dtVendorQueryData.Rows[i]["GrossCost"]) == 0M)
                        && (string.IsNullOrEmpty(dtVendorQueryData.Rows[i]["Discount"].ToString()) || Convert.ToDecimal(dtVendorQueryData.Rows[i]["Discount"]) == 0M)
                        && (string.IsNullOrEmpty(dtVendorQueryData.Rows[i]["NetCost"].ToString()) || Convert.ToDecimal(dtVendorQueryData.Rows[i]["NetCost"]) == 0M))
                    {
                        dtVendorQueryData.Rows.RemoveAt(i);
                    }
                }
            }

            if (dtPublisherQueryData != null)
            {
                for (int i = (dtPublisherQueryData.Rows.Count - 1); i >= 0; --i)
                {
                    if (
                        (string.IsNullOrEmpty(dtPublisherQueryData.Rows[i]["GrossCost"].ToString()) || Convert.ToDecimal(dtPublisherQueryData.Rows[i]["GrossCost"]) == 0M)
                        && (string.IsNullOrEmpty(dtPublisherQueryData.Rows[i]["Discount"].ToString()) || Convert.ToDecimal(dtPublisherQueryData.Rows[i]["Discount"]) == 0M)
                        && (string.IsNullOrEmpty(dtPublisherQueryData.Rows[i]["NetCost"].ToString()) || Convert.ToDecimal(dtPublisherQueryData.Rows[i]["NetCost"]) == 0M))
                    {
                        dtPublisherQueryData.Rows.RemoveAt(i);
                    }
                }
            }

            if (dtPolybagQueryData != null)
            {
                for (int i = (dtPolybagQueryData.Rows.Count - 1); i >= 0; --i)
                {
                    if (
                        (string.IsNullOrEmpty(dtPolybagQueryData.Rows[i]["GrossCost"].ToString()) || Convert.ToDecimal(dtPolybagQueryData.Rows[i]["GrossCost"]) == 0M)
                        && (string.IsNullOrEmpty(dtPolybagQueryData.Rows[i]["Discount"].ToString()) || Convert.ToDecimal(dtPolybagQueryData.Rows[i]["Discount"]) == 0M)
                        && (string.IsNullOrEmpty(dtPolybagQueryData.Rows[i]["NetCost"].ToString()) || Convert.ToDecimal(dtPolybagQueryData.Rows[i]["NetCost"]) == 0M))
                    {
                        dtPolybagQueryData.Rows.RemoveAt(i);
                    }
                }
            }

            DataTable dtSummaryData = new DataTable();
            dtSummaryData.Columns.Add("ID");
            dtSummaryData.Columns.Add("PubVndDescription");
            dtSummaryData.Columns.Add("RunDate");
            dtSummaryData.Columns.Add("FiscalYear");
            dtSummaryData.Columns.Add("FiscalMonth");
            dtSummaryData.Columns.Add("AdNumber");
            dtSummaryData.Columns.Add("AdDescription");
            dtSummaryData.Columns.Add("PageCount");
            dtSummaryData.Columns.Add("MediaQuantity");
            dtSummaryData.Columns.Add("InsertQuantity");
            dtSummaryData.Columns.Add("CostCode");
            dtSummaryData.Columns.Add("CostCodeDescription");
            dtSummaryData.Columns.Add("GrossCost");
            dtSummaryData.Columns.Add("Discount");
            dtSummaryData.Columns.Add("NetCost");

            DataSet dsDump = new DataSet();
            dsDump.Tables.Add(dtSummaryData);

            if (dtVendorQueryData != null)
            {
                foreach (DataRow dr in dtVendorQueryData.Rows)
                {
                    int pageCount = 0;
                    int mediaQuantity = 0;
                    decimal grossCost = 0;
                    decimal discount = 0;
                    decimal netCost = 0;

                    if (!string.IsNullOrEmpty(dr["PageCount"].ToString()))
                        pageCount = Convert.ToInt32(dr["PageCount"].ToString());
                    if (!string.IsNullOrEmpty(dr["MediaQuantity"].ToString()))
                        mediaQuantity = Convert.ToInt32(dr["MediaQuantity"].ToString());
                    if (!string.IsNullOrEmpty(dr["GrossCost"].ToString()))
                        grossCost = Convert.ToDecimal(dr["GrossCost"].ToString());
                    if (!string.IsNullOrEmpty(dr["Discount"].ToString()))
                        discount = Convert.ToDecimal(dr["Discount"].ToString());
                    if (!string.IsNullOrEmpty(dr["PageCount"].ToString()))
                        netCost = Convert.ToDecimal(dr["NetCost"].ToString());

                    DataRow[] sd_rows = null;
                    try
                    {
                        sd_rows = dtSummaryData.Select("ID = " + dr["VND_Vendor_ID"].ToString()
                            + " and RunDate = '" + Convert.ToDateTime(dr["RunDate"]).ToShortDateString() + "'"
                            + " and AdNumber = '" + dr["AdNumber"].ToString() + "'"
                            + " and CostCode = '" + dr["CostCode"].ToString() + "'");
                    }
                    catch (ArgumentException ae)
                    {
                        dsDump.WriteXml("dsDump.xml");
                        throw ae;
                    }

                    if (sd_rows.Length > 0)
                    {
                        sd_rows[0]["PageCount"] = Convert.ToInt32(sd_rows[0]["PageCount"]) + pageCount;
                        sd_rows[0]["MediaQuantity"] = Convert.ToInt32(sd_rows[0]["MediaQuantity"]) + mediaQuantity;
                        sd_rows[0]["GrossCost"] = Convert.ToDecimal(sd_rows[0]["GrossCost"]) + grossCost;
                        sd_rows[0]["Discount"] = Convert.ToDecimal(sd_rows[0]["Discount"]) + discount;
                        sd_rows[0]["NetCost"] = Convert.ToDecimal(sd_rows[0]["NetCost"]) + netCost;
                    }
                    else
                    {
                        dtSummaryData.Rows.Add(dr["VND_Vendor_ID"], dr["VendorDescription"], dr["RunDate"],
                            Convert.ToDateTime(dr["RunDate"]).Year,
                            FiscalCalculator.FiscalMonth(Convert.ToDateTime(dr["RunDate"].ToString())),
                            dr["AdNumber"], dr["AdDescription"], pageCount, mediaQuantity, string.Empty, dr["CostCode"], dr["CostCodeDescription"],
                            grossCost, discount, netCost);
                    }
                }
            }

            if (dtPublisherQueryData != null)
            {
                foreach (DataRow dr in dtPublisherQueryData.Rows)
                {
                    int pageCount = 0;
                    int mediaQuantity = 0;
                    int insertQuantity = 0;
                    decimal grossCost = 0;
                    decimal discount = 0;
                    decimal netCost = 0;

                    if (!string.IsNullOrEmpty(dr["PageCount"].ToString()))
                        pageCount = Convert.ToInt32(dr["PageCount"].ToString());
                    if (!string.IsNullOrEmpty(dr["MediaQuantity"].ToString()))
                        mediaQuantity = Convert.ToInt32(dr["MediaQuantity"].ToString());
                    if (!string.IsNullOrEmpty(dr["InsertQuantity"].ToString()))
                        insertQuantity = Convert.ToInt32(dr["InsertQuantity"].ToString());
                    if (!string.IsNullOrEmpty(dr["GrossCost"].ToString()))
                        grossCost = Convert.ToDecimal(dr["GrossCost"].ToString());
                    if (!string.IsNullOrEmpty(dr["Discount"].ToString()))
                        discount = Convert.ToDecimal(dr["Discount"].ToString());
                    if (!string.IsNullOrEmpty(dr["PageCount"].ToString()))
                        netCost = Convert.ToDecimal(dr["NetCost"].ToString());


                    DataRow[] sd_rows = dtSummaryData.Select("ID = '" + dr["Pub_ID"].ToString() + "'"
                        + " and RunDate = '" + Convert.ToDateTime(dr["RunDate"]).ToShortDateString() + "'"
                        + " and AdNumber = '" + dr["AdNumber"].ToString() + "'"
                        + " and CostCode = '" + dr["CostCode"].ToString() + "'");


                    if (sd_rows.Length > 0)
                    {
                        sd_rows[0]["PageCount"] = Convert.ToInt32(sd_rows[0]["PageCount"]) + pageCount;
                        sd_rows[0]["MediaQuantity"] = Convert.ToInt32(sd_rows[0]["MediaQuantity"]) + mediaQuantity;
                        sd_rows[0]["InsertQuantity"] = Convert.ToInt32(sd_rows[0]["InsertQuantity"]) + insertQuantity;
                        sd_rows[0]["GrossCost"] = Convert.ToDecimal(sd_rows[0]["GrossCost"]) + grossCost;
                        sd_rows[0]["Discount"] = Convert.ToDecimal(sd_rows[0]["Discount"]) + discount;
                        sd_rows[0]["NetCost"] = Convert.ToDecimal(sd_rows[0]["NetCost"]) + netCost;
                    }
                    else
                    {
                        dtSummaryData.Rows.Add(dr["Pub_ID"], dr["Pub_ID"], dr["RunDate"],
                            Convert.ToDateTime(dr["RunDate"]).Year,
                            FiscalCalculator.FiscalMonth(Convert.ToDateTime(dr["RunDate"].ToString())),
                            dr["AdNumber"], dr["AdDescription"], pageCount, mediaQuantity, insertQuantity, dr["CostCode"], dr["CostCodeDescription"],
                            grossCost, discount, netCost);
                    }
                }

            }

            if (dtPolybagQueryData != null)
            {
                foreach (DataRow dr in dtPolybagQueryData.Rows)
                {
                    int pageCount = 0;
                    int mediaQuantity = 0;
                    decimal grossCost = 0;
                    decimal discount = 0;
                    decimal netCost = 0;

                    if (!string.IsNullOrEmpty(dr["PageCount"].ToString()))
                        pageCount = Convert.ToInt32(dr["PageCount"].ToString());
                    if (!string.IsNullOrEmpty(dr["MediaQuantity"].ToString()))
                        mediaQuantity = Convert.ToInt32(dr["MediaQuantity"].ToString());
                    if (!string.IsNullOrEmpty(dr["GrossCost"].ToString()))
                        grossCost = Convert.ToDecimal(dr["GrossCost"].ToString());
                    if (!string.IsNullOrEmpty(dr["Discount"].ToString()))
                        discount = Convert.ToDecimal(dr["Discount"].ToString());
                    if (!string.IsNullOrEmpty(dr["PageCount"].ToString()))
                        netCost = Convert.ToDecimal(dr["NetCost"].ToString());


                    DataRow[] sd_rows = dtSummaryData.Select("ID = '" + dr["VND_Vendor_ID"].ToString() + "'"
                        + " and RunDate = '" + dr["RunDate"].ToString() + "'"
                        + " and AdNumber = '" + dr["AdNumber"].ToString() + "'"
                        + " and CostCode = " + dr["CostCode"].ToString());

                    if (sd_rows.Length > 0)
                    {
                        sd_rows[0]["PageCount"] = Convert.ToInt32(sd_rows[0]["PageCount"]) + pageCount;
                        sd_rows[0]["MediaQuantity"] = Convert.ToInt32(sd_rows[0]["MediaQuantity"]) + mediaQuantity;
                        sd_rows[0]["GrossCost"] = Convert.ToDecimal(sd_rows[0]["GrossCost"]) + grossCost;
                        sd_rows[0]["Discount"] = Convert.ToDecimal(sd_rows[0]["Discount"]) + discount;
                        sd_rows[0]["NetCost"] = Convert.ToDecimal(sd_rows[0]["NetCost"]) + netCost;
                    }
                    else
                    {
                        dtSummaryData.Rows.Add(dr["VND_Vendor_ID"], dr["VendorDescription"], dr["RunDate"],
                            Convert.ToDateTime(dr["RunDate"]).Year,
                            FiscalCalculator.FiscalMonth(Convert.ToDateTime(dr["RunDate"].ToString())),
                            dr["AdNumber"], dr["AdDescription"], pageCount, mediaQuantity, string.Empty, dr["CostCode"], dr["CostCodeDescription"],
                            grossCost, discount, netCost);
                    }
                }
            }
            #endregion
            PerformanceLog.End("Vendor Commitment Report - Group Costs");

            // If there is no data in dtSummaryData, return 0
            if (dtSummaryData.Rows.Count == 0)
                return ReportExecutionStatus.NoDataReturned;

            PerformanceLog.Start("Vendor Commitment Report - Write to Excel");
            #region Sort Data and Write it to the Excel Document
            DataView dvSummaryData = new DataView(dtSummaryData);
            dvSummaryData.Sort = "FiscalYear, FiscalMonth, PubVndDescription, RunDate, AdNumber";

            int fiscalYear = -1;
            int fiscalMonth = -1;
            string pubvndDescription = null;

            decimal subTotalGrossCost = 0;
            decimal subTotalDiscount = 0;
            decimal subtotalNetCost = 0;
            decimal totalGrossCost = 0;
            decimal totalDiscount = 0;
            decimal totalNetCost = 0;

            bool bFirstRecord = true;

            writer.WriteTemplateLine("Header1a", string.Concat("Report:     ", "Vendor Commitment"), null, null, null, null, null, null, null, null, null, null, null);
            writer.WriteTemplateLine("Header1b", string.Concat("Generated:  ", DateTime.Now.ToString()), null, null, null, null, null, null, null, null, null, null, null);
            writer.WriteTemplateLine("Header1b", null, null, null, null, null, null, null, null, null, null, null, null);
            writer.WriteTemplateLine("Header2", null, null, null, null, null, null, null, null, null, null, null, null);

            foreach (DataRowView drv in dvSummaryData)
            {
                if (!bFirstRecord
                    && (fiscalYear != Convert.ToInt32(drv["FiscalYear"])
                        || fiscalMonth != Convert.ToInt32(drv["FiscalMonth"])
                        || pubvndDescription != drv["PubVndDescription"].ToString()))
                {
                    writer.WriteTemplateLine("SubTotalRow1", pubvndDescription, null, null, null, null, null, null,
                        null, null, subTotalGrossCost.ToString(), subTotalDiscount.ToString(), subtotalNetCost.ToString());

                    subTotalGrossCost = 0;
                    subTotalDiscount = 0;
                    subtotalNetCost = 0;
                }

                bFirstRecord = false;

                fiscalYear = Convert.ToInt32(drv["FiscalYear"]);
                fiscalMonth = Convert.ToInt32(drv["FiscalMonth"]);
                pubvndDescription = drv["PubVndDescription"].ToString();

                subTotalGrossCost += Convert.ToDecimal(drv["GrossCost"].ToString());
                subTotalDiscount += Convert.ToDecimal(drv["Discount"].ToString());
                subtotalNetCost += Convert.ToDecimal(drv["NetCost"].ToString());

                totalGrossCost += Convert.ToDecimal(drv["GrossCost"].ToString());
                totalDiscount += Convert.ToDecimal(drv["Discount"].ToString());
                totalNetCost += Convert.ToDecimal(drv["NetCost"].ToString());

                writer.WriteTemplateLine("RegRow1", drv["PubVndDescription"].ToString(), drv["RunDate"].ToString(),
                    drv["AdNumber"].ToString(), drv["AdDescription"].ToString(), drv["PageCount"].ToString(),
                    drv["MediaQuantity"].ToString(), drv["InsertQuantity"].ToString(), drv["CostCode"].ToString(), drv["CostCodeDescription"].ToString(),
                    drv["GrossCost"].ToString(), drv["Discount"].ToString(), drv["NetCost"].ToString());
            }

            writer.WriteTemplateLine("SubTotalRow1", pubvndDescription, null, null, null, null, null, null,
                null, null, subTotalGrossCost.ToString(), subTotalDiscount.ToString(), subtotalNetCost.ToString());

            writer.WriteTemplateLine("TotalRow1", "Grand Total", null, null, null, null, null, null, null, null,
                totalGrossCost.ToString(), totalDiscount.ToString(), totalNetCost.ToString());

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

            writer.WriteTemplateLine("Criteria2", "Vendors", null, null, null, null);
            if (this._lstVendor.SelectedItems.Count == 0)
                writer.WriteTemplateLine("Criteria3", null, "None Selected", null, null, null, null);
            else if (this._lstVendor.SelectedItems.Count == _lstVendor.Items.Count)
                writer.WriteTemplateLine("Criteria3", null, "All", null, null, null, null);
            else
            {
                for (int i = 0; i < _lstVendor.SelectedItems.Count; ++i)
                    writer.WriteTemplateLine("Criteria3", null, ((Lookup.CurrentVendorsRow)((DataRowView)_lstVendor.SelectedItems[i]).Row).description, null, null, null, null);
            }

            writer.WriteTemplateLine("Criteria2", "Pubs", null, null, null, null);
            if (this._lstPublisher.SelectedItems.Count == 0)
                writer.WriteTemplateLine("Criteria3", null, "None Selected", null, null, null, null);
            else if (this._lstPublisher.SelectedItems.Count == _lstPublisher.Items.Count)
                writer.WriteTemplateLine("Criteria3", null, "All", null, null, null, null);
            else
            {
                for (int i = 0; i < _lstPublisher.SelectedItems.Count; ++i)
                    writer.WriteTemplateLine("Criteria3", null, ((Lookup.PublicationListRow)((DataRowView)_lstPublisher.SelectedItems[i]).Row).pub_id, null, null, null, null);
            }

            writer.WriteTemplateLine("Criteria2", "Cost Codes", null, null, null, null);
            if (this._lstCostCodes.SelectedItems.Count == 0)
                writer.WriteTemplateLine("Criteria3", null, "None Selected", null, null, null, null);
            else if (this._lstCostCodes.SelectedItems.Count == _lstCostCodes.Items.Count)
                writer.WriteTemplateLine("Criteria3", null, "All", null, null, null, null);
            else
            {
                KeyValuePair<int, string> curCostCode;
                for (int i = 0; i < _lstCostCodes.SelectedItems.Count; ++i)
                {
                    curCostCode = (KeyValuePair<int, string>)_lstCostCodes.SelectedItems[i];
                    writer.WriteTemplateLine("Criteria3", null, curCostCode.Value, null, null, null, null);
                }
            }

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

            #endregion
            PerformanceLog.End("Vendor Commitment Report - Write to Excel");


            return ReportExecutionStatus.Success;
        }

        private void _btnClearPubs_Click(object sender, EventArgs e)
        {
            _lstPublisher.SelectedIndex = -1;
        }

        private void _btnSelectAllPubs_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _lstPublisher.Items.Count; ++i)
                _lstPublisher.SelectedItem = _lstPublisher.Items[i];
        }

        private void _btnClearVendors_Click(object sender, EventArgs e)
        {
            _lstVendor.SelectedIndex = -1;
        }

        private void _btnSelectAllVendors_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _lstVendor.Items.Count; ++i)
                _lstVendor.SelectedItem = _lstVendor.Items[i];
        }

        private void _btnClearCostCodes_Click( object sender, EventArgs e )
        {
            _lstCostCodes.SelectedIndex = -1;
        }

        private void _btnSelectAllCostCodes_Click( object sender, EventArgs e )
        {
            for ( int i = 0; i < _lstCostCodes.Items.Count; ++i )
                _lstCostCodes.SelectedItem = _lstCostCodes.Items[i];
        }
    }
}

