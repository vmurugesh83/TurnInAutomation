using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CatalogEstimating.Datasets;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml;
using System.IO;

namespace CatalogEstimating.UserControls.Reports
{
    public partial class rptEstimateSummary : CatalogEstimating.UserControls.Reports.ReportControl
    {
        public rptEstimateSummary()
            : base()
        {
            InitializeComponent();
        }

        public rptEstimateSummary(Lookup dsLookup) : base(dsLookup)
        {
            InitializeComponent();

            _cboEstimateStatus.DataSource = _dsLookup.est_status;
            _cboEstimateStatus.DisplayMember = "description";
            _cboEstimateStatus.ValueMember = "est_status_id";

            _cboVendor.DataSource = _dsLookup.CurrentVendors;
            _cboVendor.DisplayMember = "description";
            _cboVendor.ValueMember = "vnd_vendor_id";

            _cboVendorType.DataSource = _dsLookup.vnd_vendortype;
            _cboVendorType.DisplayMember = "description";
            _cboVendorType.ValueMember = "vnd_vendortype_id";

            _lstEstMediaType.DataSource = _dsLookup.est_estimatemediatype;
            _lstEstMediaType.DisplayMember = "description";
            _lstEstMediaType.ValueMember = "est_estimatemediatype_id";
            _lstEstMediaType.SelectedIndex = -1;

            _lstComponentType.DataSource = _dsLookup.est_componenttype;
            _lstComponentType.DisplayMember = "description";
            _lstComponentType.ValueMember = "est_componenttype_id";
            _lstComponentType.SelectedIndex = -1;

        }

        public override string ReportTemplate
        {
            get { return "Estimate Summary"; }
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

            if (_cboVendor.SelectedIndex < 1 && _cboVendorType.SelectedIndex > 0)
            {
                _errorProvider.SetError(_cboVendor, "Vendor is required when Vendor Type is selected");
                return false;
            }

            if (_cboVendor.SelectedIndex > 0 && _cboVendorType.SelectedIndex < 1)
            {
                _errorProvider.SetError(_cboVendorType, "Vendor Type is required when Vendor is selected");
                return false;
            }

            return true;
        }

        protected void FillCommand( SqlCommand myCmd )
        {
            string EstimateMediaTypes = String.Empty;
            string ComponentTypes = String.Empty;

            if ( _lstEstMediaType.SelectedItems.Count > 0 )
            {
                StringWriter writerString = new StringWriter();
                XmlTextWriter writerXml = new XmlTextWriter( writerString );
                writerXml.WriteStartElement( "root" );
                for ( int i = 0; i < _lstEstMediaType.SelectedItems.Count; ++i )
                {
                    Lookup.est_estimatemediatypeRow r = (Lookup.est_estimatemediatypeRow)( (DataRowView)_lstEstMediaType.SelectedItems[i] ).Row;
                    writerXml.WriteStartElement( "est_estimatemediatype" );
                    writerXml.WriteAttributeString( "est_estimatemediatype_id", r.est_estimatemediatype_id.ToString() );
                    writerXml.WriteEndElement();
                }
                writerXml.WriteEndElement();

                EstimateMediaTypes = writerString.ToString();
            }

            if ( _lstComponentType.SelectedItems.Count > 0 )
            {
                StringWriter writerString = new StringWriter();
                XmlTextWriter writerXml = new XmlTextWriter( writerString );
                writerXml.WriteStartElement( "root" );
                for ( int i = 0; i < _lstComponentType.SelectedItems.Count; ++i )
                {
                    Lookup.est_componenttypeRow r = (Lookup.est_componenttypeRow)( (DataRowView)_lstComponentType.SelectedItems[i] ).Row;
                    writerXml.WriteStartElement( "est_componenttype" );
                    writerXml.WriteAttributeString( "est_componenttype_id", r.est_componenttype_id.ToString() );
                    writerXml.WriteEndElement();
                }
                writerXml.WriteEndElement();

                ComponentTypes = writerString.ToString();
            }

            if ( _dtStartRunDate.Value == null )
                myCmd.Parameters.AddWithValue( "@StartRunDate", DBNull.Value );
            else
                myCmd.Parameters.AddWithValue( "@StartRunDate", _dtStartRunDate.Value.Value.Date );
            if ( _dtEndRunDate.Value == null )
                myCmd.Parameters.AddWithValue( "@EndRunDate", DBNull.Value );
            else
                myCmd.Parameters.AddWithValue( "@EndRunDate", _dtEndRunDate.Value.Value.Date );
            if ( _txtEstimateID.Text != String.Empty )
                myCmd.Parameters.AddWithValue( "@EST_Estimate_ID", _txtEstimateID.Text );
            else
                myCmd.Parameters.AddWithValue( "@EST_Estimate_ID", DBNull.Value );
            if ( _txtHostAdNumber.Text.Trim() != "" )
                myCmd.Parameters.AddWithValue( "@AdNumber", _txtHostAdNumber.Text.Trim() );
            else
                myCmd.Parameters.AddWithValue( "@AdNumber", DBNull.Value );
            if ( _cboEstimateStatus.SelectedIndex > 0 )
                myCmd.Parameters.AddWithValue("@EST_Status_ID", _cboEstimateStatus.SelectedValue);
            else
                myCmd.Parameters.AddWithValue("@EST_Status_ID", DBNull.Value);
            if ( _cboVendor.SelectedIndex > 0 )
                myCmd.Parameters.AddWithValue( "@VND_Vendor_ID", _cboVendor.SelectedValue );
            else
                myCmd.Parameters.AddWithValue( "@VND_Vendor_ID", DBNull.Value );
            if ( _cboVendorType.SelectedIndex > 0 )
                myCmd.Parameters.AddWithValue( "@VND_VendorType_ID", _cboVendorType.SelectedValue );
            else
                myCmd.Parameters.AddWithValue( "@VND_VendorType_ID", DBNull.Value );
            if ( _radAll.Checked )
                myCmd.Parameters.AddWithValue( "@VendorSupplied", 1 );
            else if ( _radOnlyVS.Checked )
                myCmd.Parameters.AddWithValue( "@VendorSupplied", 2 );
            else
                myCmd.Parameters.AddWithValue( "@VendorSupplied", 3 );
            if ( EstimateMediaTypes != String.Empty )
                myCmd.Parameters.AddWithValue( "@EstimateMediaType", EstimateMediaTypes );
            else
                myCmd.Parameters.AddWithValue( "@EstimateMediaType", DBNull.Value );
            if ( ComponentTypes != String.Empty )
                myCmd.Parameters.AddWithValue( "@ComponentType", ComponentTypes );
            else
                myCmd.Parameters.AddWithValue( "@ComponentType", DBNull.Value );
        }

        public override ReportExecutionStatus RunReport( ExcelWriter writer )
        {
            if (!ValidateSearchCriteria())
                return ReportExecutionStatus.InvalidSearchCriteria;

            using ( SqlConnection myConn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
            {
                myConn.Open();

                SqlCommand myCmd = new SqlCommand("rpt_EstimateSummary", myConn);
                myCmd.CommandType = CommandType.StoredProcedure;
                myCmd.CommandTimeout = 600;

                FillCommand( myCmd );

                SqlDataReader myDR = myCmd.ExecuteReader();
                if (!myDR.HasRows)
                {
                    myDR.Close();
                    myConn.Close();
                    return ReportExecutionStatus.NoDataReturned;
                }

                writer.WriteTemplateLine("Header1a", string.Concat("Report:     ", "Estimate Summary"), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header1b", string.Concat("Generated:  ", DateTime.Now.ToString()), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header1b", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header2", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header3", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

                int PageCount = 0;
                int AdditionalPlates = 0;
                int SoloQuantity = 0;
                int PolyBagQuantity = 0;
                int InsertQuantity = 0;
                int OtherQuantity = 0;
                int MediaQuantity = 0;
                decimal CreativeCost = 0;
                decimal SeparatorCost = 0;
                decimal GrossPrintCost = 0;
                decimal EarlyPayPrintDiscountAmount = 0;
                decimal NetPrintCost = 0;
                decimal TotalPaperPounds = 0;
                decimal PaperCWTRate = 0;
                decimal GrossPaperCost = 0;
                decimal EarlyPayPaperDiscountAmount = 0;
                decimal NetPaperCost = 0;
                decimal ProductionSalesTaxAmount = 0;
                decimal VendorProductionCost = 0;
                decimal PolyBagCost = 0;
                decimal StitchInCost = 0;
                decimal BlowInCost = 0;
                decimal MailListCost = 0;
                decimal TotalInkJetCost = 0;
                decimal HandlingTotal = 0;
                decimal MailTrackingCost = 0;
                decimal SampleFreight = 0;
                decimal OtherFreight = 0;
                decimal TotalPostalDropCost = 0;
                decimal InsertFreightTotalCost = 0;
                decimal InsertCost = 0;
                decimal TotalPostageCost = 0;
                decimal OtherProductionCost = 0;
                decimal GrandTotal = 0;

                while (myDR.Read())
                {
                    writer.WriteTemplateLine("RegRow1", myDR["EST_Estimate_ID"].ToString(), myDR["AdNumber"].ToString(), myDR["Description"].ToString(), myDR["RunDate"].ToString(),
                        null, myDR["EstimateMediaType"].ToString(), myDR["ComponentType"].ToString(), myDR["VendorSuppliedDesc"].ToString(),
                        null, myDR["Height"].ToString(), myDR["Width"].ToString(), myDR["PaperWeight"].ToString(), myDR["PaperGrade"].ToString(),
                        myDR["PageCount"].ToString(), myDR["AdditionalPlates"].ToString(), myDR["SoloQuantity"].ToString(),
                        myDR["PolyBagQuantity"].ToString(), myDR["InsertQuantity"].ToString(), myDR["OtherQuantity"].ToString(),
                        myDR["MediaQuantity"].ToString(), myDR["PrinterVendor"].ToString(), myDR["PaperVendor"].ToString(),
                        myDR["SeparatorVendor"].ToString(), myDR["AssemblyVendor"].ToString(), myDR["MailingHouseVendor"].ToString(), myDR["MailTrackerVendor"].ToString(),
                        myDR["CreativeCost"].ToString(), myDR["SeparatorCost"].ToString(), myDR["GrossPrintCost"].ToString(),
                        myDR["EarlyPayPrintDiscountAmount"].ToString(), myDR["NetPrintCost"].ToString(), myDR["TotalPaperPounds"].ToString(),
                        myDR["PaperCWTRate"].ToString(), myDR["GrossPaperCost"].ToString(), myDR["EarlyPayPaperDiscountAmount"].ToString(),
                        myDR["NetPaperCost"].ToString(), myDR["ProductionSalesTaxAmount"].ToString(), myDR["MailListCost"].ToString(),
                        myDR["VendorProductionCost"].ToString(), myDR["PolyBagCost"].ToString(), myDR["StitchInCost"].ToString(),
                        myDR["BlowInCost"].ToString(), myDR["TotalInkJetCost"].ToString(),
                        myDR["HandlingTotal"].ToString(), myDR["MailTrackingCost"].ToString(), myDR["SampleFreight"].ToString(),
                        myDR["OtherFreight"].ToString(), myDR["TotalPostalDropCost"].ToString(),
                        myDR["InsertFreightTotalCost"].ToString(),
                        myDR["InsertCost"].ToString(), myDR["TotalPostageCost"].ToString(), myDR["OtherProductionCost"].ToString(),
                        myDR["GrandTotal"].ToString(), null);

                    PageCount += myDR.GetInt32(myDR.GetOrdinal("PageCount"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("AdditionalPlates")))
                        AdditionalPlates += myDR.GetInt32(myDR.GetOrdinal("AdditionalPlates"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("SoloQuantity")))
                        SoloQuantity += myDR.GetInt32(myDR.GetOrdinal("SoloQuantity"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("PolyBagQuantity")))
                        PolyBagQuantity += myDR.GetInt32(myDR.GetOrdinal("PolyBagQuantity"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("InsertQuantity")))
                        InsertQuantity += myDR.GetInt32(myDR.GetOrdinal("InsertQuantity"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("OtherQuantity")))
                        OtherQuantity += myDR.GetInt32(myDR.GetOrdinal("OtherQuantity"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("MediaQuantity")))
                        MediaQuantity += myDR.GetInt32(myDR.GetOrdinal("MediaQuantity"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("CreativeCost")))
                        CreativeCost += myDR.GetDecimal(myDR.GetOrdinal("CreativeCost"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("SeparatorCost")))
                        SeparatorCost += myDR.GetDecimal(myDR.GetOrdinal("SeparatorCost"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("GrossPrintCost")))
                        GrossPrintCost += myDR.GetDecimal(myDR.GetOrdinal("GrossPrintCost"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("EarlyPayPrintDiscountAmount")))
                        EarlyPayPrintDiscountAmount += myDR.GetDecimal(myDR.GetOrdinal("EarlyPayPrintDiscountAmount"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("NetPrintCost")))
                        NetPrintCost += myDR.GetDecimal(myDR.GetOrdinal("NetPrintCost"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("TotalPaperPounds")))
                        TotalPaperPounds += myDR.GetDecimal(myDR.GetOrdinal("TotalPaperPounds"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("PaperCWTRate")))
                        PaperCWTRate += myDR.GetDecimal(myDR.GetOrdinal("PaperCWTRate"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("GrossPaperCost")))
                        GrossPaperCost += myDR.GetDecimal(myDR.GetOrdinal("GrossPaperCost"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("EarlyPayPaperDiscountAmount")))
                        EarlyPayPaperDiscountAmount += myDR.GetDecimal(myDR.GetOrdinal("EarlyPayPaperDiscountAmount"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("NetPaperCost")))
                        NetPaperCost += myDR.GetDecimal(myDR.GetOrdinal("NetPaperCost"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("ProductionSalesTaxAmount")))
                        ProductionSalesTaxAmount += myDR.GetDecimal(myDR.GetOrdinal("ProductionSalesTaxAmount"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("VendorProductionCost")))
                        VendorProductionCost += myDR.GetDecimal(myDR.GetOrdinal("VendorProductionCost"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("PolyBagCost")))
                        PolyBagCost += myDR.GetDecimal(myDR.GetOrdinal("PolyBagCost"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("StitchInCost")))
                        StitchInCost += myDR.GetDecimal(myDR.GetOrdinal("StitchInCost"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("BlowInCost")))
                        BlowInCost += myDR.GetDecimal(myDR.GetOrdinal("BlowInCost"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("MailListCost")))
                        MailListCost += myDR.GetDecimal(myDR.GetOrdinal("MailListCost"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("TotalInkJetCost")))
                        TotalInkJetCost += myDR.GetDecimal(myDR.GetOrdinal("TotalInkJetCost"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("HandlingTotal")))
                        HandlingTotal += myDR.GetDecimal(myDR.GetOrdinal("HandlingTotal"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("MailTrackingCost")))
                        MailTrackingCost += myDR.GetDecimal(myDR.GetOrdinal("MailTrackingCost"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("SampleFreight")))
                        SampleFreight += myDR.GetDecimal(myDR.GetOrdinal("SampleFreight"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("OtherFreight")))
                        OtherFreight += myDR.GetDecimal(myDR.GetOrdinal("OtherFreight"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("TotalPostalDropCost")))
                        TotalPostalDropCost += myDR.GetDecimal(myDR.GetOrdinal("TotalPostalDropCost"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("InsertFreightTotalCost")))
                        InsertFreightTotalCost += myDR.GetDecimal(myDR.GetOrdinal("InsertFreightTotalCost"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("InsertCost")))
                        InsertCost += myDR.GetDecimal(myDR.GetOrdinal("InsertCost"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("TotalPostageCost")))
                        TotalPostageCost += myDR.GetDecimal(myDR.GetOrdinal("TotalPostageCost"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("OtherProductionCost")))
                        OtherProductionCost += myDR.GetDecimal(myDR.GetOrdinal("OtherProductionCost"));
                    if (!myDR.IsDBNull(myDR.GetOrdinal("GrandTotal")))
                        GrandTotal += myDR.GetDecimal(myDR.GetOrdinal("GrandTotal"));
                }

                myDR.Close();
                myConn.Close();

                writer.WriteTemplateLine("BlankRow1", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("TotalRow1", null, null, null, null, null, null, null, null, null, null, null, null, null, PageCount.ToString(),
                    AdditionalPlates.ToString(), SoloQuantity.ToString(), PolyBagQuantity.ToString(), InsertQuantity.ToString(),
                    OtherQuantity.ToString(), MediaQuantity.ToString(), null, null, null, null, null, null, CreativeCost.ToString(),
                    SeparatorCost.ToString(), GrossPrintCost.ToString(),
                    EarlyPayPrintDiscountAmount.ToString(), NetPrintCost.ToString(), TotalPaperPounds.ToString(), null, GrossPaperCost.ToString(),
                    EarlyPayPaperDiscountAmount.ToString(), NetPaperCost.ToString(), ProductionSalesTaxAmount.ToString(), MailListCost.ToString(), VendorProductionCost.ToString(),
                    PolyBagCost.ToString(), StitchInCost.ToString(), BlowInCost.ToString(), TotalInkJetCost.ToString(),
                    HandlingTotal.ToString(), MailTrackingCost.ToString(), SampleFreight.ToString(), OtherFreight.ToString(),
                    TotalPostalDropCost.ToString(), InsertFreightTotalCost.ToString(), InsertCost.ToString(), TotalPostageCost.ToString(),
                    OtherProductionCost.ToString(), GrandTotal.ToString(), null);

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

                writer.WriteTemplateLine("Criteria2", "Vendor", null, null, null, null);
                if (_cboVendor.SelectedIndex > 0)
                    writer.WriteTemplateLine("Criteria3", null, _cboVendor.Text, null, null, null, null);
                else
                    writer.WriteTemplateLine("Criteria3", null, "All", null, null, null, null);

                writer.WriteTemplateLine("Criteria2", "Vendor", null, null, null, null);
                if (_cboVendorType.SelectedIndex > 0)
                    writer.WriteTemplateLine("Criteria3", null, _cboVendorType.Text, null, null, null, null);
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

                writer.WriteTemplateLine("Criteria2", "Component Types", null, null, null, null);
                if (_lstComponentType.SelectedItems.Count == 0)
                    writer.WriteTemplateLine("Criteria3", null, "All", null, null, null, null);
                else
                {
                    for (int i = 0; i < _lstComponentType.SelectedItems.Count; ++i)
                        writer.WriteTemplateLine("Criteria3", null, ((Lookup.est_componenttypeRow)((DataRowView)_lstComponentType.SelectedItems[i]).Row).description, null, null, null, null);
                }

                writer.SetTemplateFreezePanes("FreezePanes");
            }

            return ReportExecutionStatus.Success;
        }
    }
}