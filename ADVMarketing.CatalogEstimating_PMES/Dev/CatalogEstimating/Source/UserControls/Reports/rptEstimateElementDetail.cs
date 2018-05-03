#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CatalogEstimating.Datasets;
using System.Data.SqlClient;

#endregion

namespace CatalogEstimating.UserControls.Reports
{
    public partial class rptEstimateElementDetail : rptEstimateSummary
    {
        #region Construction

        public rptEstimateElementDetail()
        : base()
        {
            InitializeComponent();
        }

        public rptEstimateElementDetail( Lookup dsLookup )
        : base( dsLookup )
        {
            InitializeComponent();
        }

        #endregion

        #region Override Methods

        public override string ReportTemplate
        {
            get { return "Estimate Element Detail"; }
        }

        public override ReportExecutionStatus RunReport( ExcelWriter writer )
        {
            if ( !ValidateSearchCriteria() )
                return ReportExecutionStatus.InvalidSearchCriteria;

            ReportExecutionStatus retVal = ReportExecutionStatus.NoDataReturned;
            using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
            {
                conn.Open();

                SqlCommand myCmd  = new SqlCommand( "rpt_EstimateSummary", conn );
                myCmd.CommandType = CommandType.StoredProcedure;
                myCmd.CommandTimeout = 8600;

                FillCommand( myCmd );

                using ( SqlDataReader myDR = myCmd.ExecuteReader() )
                {
                    if ( myDR.HasRows )
                    {
                        FillReport( writer, myDR );
                        retVal = ReportExecutionStatus.Success;
                    }

                    myDR.Close();
                    conn.Close();
                }
            }
            return retVal;
        }

        #endregion

        #region Private Methods

        private void FillReport( ExcelWriter writer, SqlDataReader myDR )
        {
            writer.WriteTemplateLine("Header1a", string.Concat("Report:     ", "Estimate Element Detail"), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            writer.WriteTemplateLine("Header1b", string.Concat("Generated:  ", DateTime.Now.ToString()), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            writer.WriteTemplateLine("Header1b", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            writer.WriteTemplateLine("Header2", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            writer.WriteTemplateLine("Header3", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

            int PageCount = 0;
            decimal PieceWeight = 0;
            int AdditionalPlates = 0;
            int SoloQuantity = 0;
            int PolyBagQuantity = 0;
            int InsertQuantity = 0;
            int OtherQuantity = 0;
            int MediaQuantity = 0;
            int SampleQuantity = 0;
            int SpoilageQuantity = 0;
            int DirectMailQuantity = 0;
            int InternalMailQuantity = 0;
            int ExternalMailQuantity = 0;
            int TotalProductionQuantity = 0;
            decimal ProductionWeight = 0;
            decimal TotalEstimateWeight = 0;
            decimal CreativeCost = 0;
            decimal SeparatorCost = 0;
            decimal ManualPrintCost = 0;
            decimal GrossPrintCost = 0;
            decimal EarlyPayPrintDiscountAmount = 0;
            decimal NetPrintCost = 0;
            decimal PrinterSalesTaxAmount = 0;
            decimal TotalPrintCost = 0;
            decimal ManualPaperCost = 0;
            decimal TotalPaperPounds = 0;
            decimal GrossPaperCost = 0;
            decimal EarlyPayPaperDiscountAmount = 0;
            decimal NetPaperCost = 0;
            decimal PaperHandlingCost = 0;
            decimal PaperSalesTaxAmount = 0;
            decimal TotalPaperCost = 0;
            decimal ProductionSalesTaxAmount = 0;
            decimal VendorProductionCost = 0;
            decimal ProductionTotal = 0;
            decimal PolyBagCost = 0;
            decimal OnsertCost = 0;
            decimal PolybagTotal = 0;
            decimal StitchInCost = 0;
            decimal BlowInCost = 0;
            decimal MailListCost = 0;
            decimal InkjetCost = 0;
            decimal InkjetMakereadyCost = 0;
            decimal TotalInkJetCost = 0;
            decimal CornerGuardCost = 0;
            decimal SkidCost = 0;
            int NumberofCartons = 0;
            decimal CartonCost = 0;
            decimal InsertHandlingTotal = 0;
            decimal TimeValueSlipsCost = 0;
            decimal MailHouseAdminFee = 0;
            decimal GluetackCost = 0;
            decimal TabbingCost = 0;
            decimal LetterInsertionCost = 0;
            decimal OtherMailHandlingCost = 0;
            decimal MailHandlingTotal = 0;
            decimal HandlingTotal = 0;
            decimal AssemblyTotal = 0;
            decimal MailTrackingCost = 0;
            decimal SampleFreight = 0;
            decimal OtherFreight = 0;
            decimal InsertFreightCost = 0;
            decimal InsertFuelSurchargeCost = 0;
            decimal InsertFreightTotalCost = 0;
            decimal InsertGrossCost = 0;
            decimal InsertDiscount = 0;
            decimal InsertCost = 0;
            decimal PostalDropCost = 0;
            decimal PostalDropFuelSurchargeCost = 0;
            decimal TotalPostalDropCost = 0;
            decimal SoloPostageCost = 0;
            decimal PolyPostageCost = 0;
            decimal TotalPostageCost = 0;
            decimal PostalTotal = 0;
            decimal OtherProductionCost = 0;
            decimal DistributionTotal = 0;
            decimal GrandTotal = 0;

            while (myDR.Read())
            {
                writer.WriteTemplateLine("RegRow1",
                    myDR["EST_Estimate_ID"].ToString(),
                    myDR["AdNumber"].ToString(),
                    myDR["Description"].ToString(),
                    myDR["RunDate"].ToString(),
                    null,
                    myDR["EstimateMediaType"].ToString(),

                    myDR["ComponentType"].ToString(),
                    myDR["VendorSuppliedDesc"].ToString(),
                    null,
                    myDR["Height"].ToString(),
                    myDR["Width"].ToString(),

                    myDR["PaperWeight"].ToString(),
                    myDR["PaperGrade"].ToString(),
                    myDR["PageCount"].ToString(),
                    myDR["PieceWeight"].ToString(),
                    myDR["AdditionalPlates"].ToString(),

                    myDR["SoloQuantity"].ToString(),
                    myDR["PolyBagQuantity"].ToString(),
                    myDR["InsertQuantity"].ToString(),
                    myDR["OtherQuantity"].ToString(),
                    myDR["MediaQuantity"].ToString(),

                    myDR["SampleQuantity"].ToString(),
                    myDR["SpoilagePct"].ToString(),
                    myDR["SpoilageQuantity"].ToString(),
                    myDR["DirectMailQuantity"].ToString(),
                    myDR["InternalMailQuantity"].ToString(),

                    myDR["ExternalMailQuantity"].ToString(),
                    myDR["TotalProductionQuantity"].ToString(),
                    myDR["ProductionWeight"].ToString(),
                    myDR["TotalEstimateWeight"].ToString(),
                    myDR["PrinterVendor"].ToString(),

                    myDR["PaperVendor"].ToString(),
                    myDR["SeparatorVendor"].ToString(),
                    myDR["CreativeVendor"].ToString(),
                    myDR["AssemblyVendor"].ToString(),
                    myDR["MailingHouseVendor"].ToString(),
                    myDR["MailingListVendor"].ToString(),
                    myDR["MailTrackerVendor"].ToString(),
                    myDR["CreativeCost"].ToString(),

                    myDR["SeparatorCost"].ToString(),
                    myDR["ManualPrintCost"].ToString(),
                    myDR["RunRate"].ToString(),
                    myDR["NumberofPlants"].ToString(),
                    myDR["PrinterPlateCost"].ToString(),

                    myDR["NumberDigitalHandlePrepare"].ToString(),
                    myDR["DigitalHandlePrepareRate"].ToString(),
                    myDR["StitcherMakereadyRate"].ToString(),
                    myDR["PressMakereadyRate"].ToString(),
                    myDR["ReplacementPlateCost"].ToString(),

                    myDR["GrossPrintCost"].ToString(),
                    myDR["EarlyPayPrintDiscountPercent"].ToString(),
                    myDR["EarlyPayPrintDiscountAmount"].ToString(),
                    myDR["NetPrintCost"].ToString(),
                    myDR["PrinterTaxableMediaPct"].ToString(),

                    myDR["PrinterSalesTaxPct"].ToString(),
                    myDR["PrinterSalesTaxAmount"].ToString(),
                    myDR["TotalPrintCost"].ToString(),
                    myDR["ManualPaperCost"].ToString(),
                    myDR["RunPounds"].ToString(),

                    myDR["MakereadyPounds"].ToString(),
                    myDR["PlateChangePounds"].ToString(),
                    myDR["NumberofPressStops"].ToString(),
                    myDR["PressStopPounds"].ToString(),
                    myDR["TotalPaperPounds"].ToString(),
                    myDR["PaperCWTRate"].ToString(),

                    myDR["GrossPaperCost"].ToString(),
                    myDR["EarlyPayPaperDiscountPercent"].ToString(),
                    myDR["EarlyPayPaperDiscountAmount"].ToString(),
                    myDR["NetPaperCost"].ToString(),
                    myDR["PaperHandlingCWTRate"].ToString(),

                    myDR["PaperHandlingCost"].ToString(),
                    myDR["PaperTaxableMediaPct"].ToString(),
                    myDR["PaperSalesTaxPct"].ToString(),
                    myDR["PaperSalesTaxAmount"].ToString(),
                    myDR["TotalPaperCost"].ToString(),

                    myDR["ProductionSalesTaxAmount"].ToString(),
                    myDR["InternalMailCPM"].ToString(),
                    myDR["ExternalMailCPM"].ToString(),
                    myDR["BlendedMailListCPM"].ToString(),
                    myDR["MailListCost"].ToString(),

                    myDR["VendorProductionCPM"].ToString(),
                    myDR["VendorProductionCost"].ToString(),
                    myDR["ProductionTotal"].ToString(),
                    myDR["PolyBagCost"].ToString(),
                    myDR["OnsertRate"].ToString(),

                    myDR["OnsertCost"].ToString(),
                    myDR["PolybagTotal"].ToString(),
                    myDR["StitchInRate"].ToString(),
                    myDR["StitchInCost"].ToString(),
                    myDR["BlowInRate"].ToString(),

                    myDR["BlowInCost"].ToString(),
                    myDR["InkJetRate"].ToString(),
                    myDR["InkjetCost"].ToString(),
                    myDR["InkJetMakereadyRate"].ToString(),
                    myDR["InkjetMakereadyCost"].ToString(),

                    myDR["TotalInkJetCost"].ToString(),
                    myDR["CornerGuardRate"].ToString(),
                    myDR["CornerGuardCost"].ToString(),
                    myDR["SkidRate"].ToString(),
                    myDR["SkidCost"].ToString(),

                    myDR["NumberofCartons"].ToString(),
                    myDR["CartonRate"].ToString(),
                    myDR["CartonCost"].ToString(),
                    myDR["InsertHandlingTotal"].ToString(),
                    myDR["TimeValueSlipsCPM"].ToString(),

                    myDR["TimeValueSlipsCost"].ToString(),
                    myDR["GrossMailHouseAdminFee"].ToString(),
                    myDR["MailHouseAdminFee"].ToString(),
                    myDR["GlueTackCPM"].ToString(),
                    myDR["GluetackCost"].ToString(),

                    myDR["TabbingCPM"].ToString(),
                    myDR["TabbingCost"].ToString(),
                    myDR["LetterInsertionCPM"].ToString(),
                    myDR["LetterInsertionCost"].ToString(),
                    myDR["OtherMailHandlingCPM"].ToString(),

                    myDR["OtherMailHandlingCost"].ToString(),
                    myDR["MailHandlingTotal"].ToString(),
                    myDR["HandlingTotal"].ToString(),
                    myDR["AssemblyTotal"].ToString(),
                    myDR["MailTrackingCPMRate"].ToString(),

                    myDR["MailTrackingCost"].ToString(),
                    myDR["SampleFreight"].ToString(),
                    myDR["OtherFreight"].ToString(),
                    myDR["InsertFreightCWT"].ToString(),
                    myDR["InsertFreightCost"].ToString(),

                    myDR["InsertFuelSurchargePercent"].ToString(),
                    myDR["InsertFuelSurchargeCost"].ToString(),
                    myDR["InsertFreightTotalCost"].ToString(),
                    myDR["InsertGrossCost"].ToString(),
                    myDR["InsertDiscountPercent"].ToString(),
                    myDR["InsertDiscount"].ToString(),
                    myDR["InsertCost"].ToString(),
                    myDR["PostalDropCost"].ToString(),

                    myDR["PostalDropFuelSurchargeCost"].ToString(),
                    myDR["TotalPostalDropCost"].ToString(),
                    myDR["SoloPostageCost"].ToString(),
                    myDR["PolyPostageCost"].ToString(),
                    myDR["TotalPostageCost"].ToString(),

                    myDR["PostalTotal"].ToString(),
                    myDR["DistributionTotal"].ToString(),
                    myDR["OtherProductionCost"].ToString(),
                    myDR["GrandTotal"].ToString(),
                    null);

                if (!myDR.IsDBNull(myDR.GetOrdinal("PageCount")))
                    PageCount += myDR.GetInt32(myDR.GetOrdinal("PageCount"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("PieceWeight")))
                    PieceWeight += myDR.GetDecimal(myDR.GetOrdinal("PieceWeight"));
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
                if (!myDR.IsDBNull(myDR.GetOrdinal("SampleQuantity")))
                    SampleQuantity += myDR.GetInt32(myDR.GetOrdinal("SampleQuantity"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("SpoilageQuantity")))
                    SpoilageQuantity += myDR.GetInt32(myDR.GetOrdinal("SpoilageQuantity"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("DirectMailQuantity")))
                    DirectMailQuantity += myDR.GetInt32(myDR.GetOrdinal("DirectMailQuantity"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("InternalMailQuantity")))
                    InternalMailQuantity += myDR.GetInt32(myDR.GetOrdinal("InternalMailQuantity"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("ExternalMailQuantity")))
                    ExternalMailQuantity += myDR.GetInt32(myDR.GetOrdinal("ExternalMailQuantity"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("TotalProductionQuantity")))
                    TotalProductionQuantity += myDR.GetInt32(myDR.GetOrdinal("TotalProductionQuantity"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("ProductionWeight")))
                    ProductionWeight += myDR.GetDecimal(myDR.GetOrdinal("ProductionWeight"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("TotalEstimateWeight")))
                    TotalEstimateWeight += myDR.GetDecimal(myDR.GetOrdinal("TotalEstimateWeight"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("CreativeCost")))
                    CreativeCost += myDR.GetDecimal(myDR.GetOrdinal("CreativeCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("SeparatorCost")))
                    SeparatorCost += myDR.GetDecimal(myDR.GetOrdinal("SeparatorCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("ManualPrintCost")))
                    ManualPrintCost += myDR.GetDecimal(myDR.GetOrdinal("ManualPrintCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("GrossPrintCost")))
                    GrossPrintCost += myDR.GetDecimal(myDR.GetOrdinal("GrossPrintCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("EarlyPayPrintDiscountAmount")))
                    EarlyPayPrintDiscountAmount += myDR.GetDecimal(myDR.GetOrdinal("EarlyPayPrintDiscountAmount"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("NetPrintCost")))
                    NetPrintCost += myDR.GetDecimal(myDR.GetOrdinal("NetPrintCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("PrinterSalesTaxAmount")))
                    PrinterSalesTaxAmount += myDR.GetDecimal(myDR.GetOrdinal("PrinterSalesTaxAmount"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("TotalPrintCost")))
                    TotalPrintCost += myDR.GetDecimal(myDR.GetOrdinal("TotalPrintCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("ManualPaperCost")))
                    ManualPaperCost += myDR.GetDecimal(myDR.GetOrdinal("ManualPaperCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("TotalPaperPounds")))
                    TotalPaperPounds += myDR.GetDecimal(myDR.GetOrdinal("TotalPaperPounds"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("GrossPaperCost")))
                    GrossPaperCost += myDR.GetDecimal(myDR.GetOrdinal("GrossPaperCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("EarlyPayPaperDiscountAmount")))
                    EarlyPayPaperDiscountAmount += myDR.GetDecimal(myDR.GetOrdinal("EarlyPayPaperDiscountAmount"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("NetPaperCost")))
                    NetPaperCost += myDR.GetDecimal(myDR.GetOrdinal("NetPaperCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("PaperHandlingCost")))
                    PaperHandlingCost += myDR.GetDecimal(myDR.GetOrdinal("PaperHandlingCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("PaperSalesTaxAmount")))
                    PaperSalesTaxAmount += myDR.GetDecimal(myDR.GetOrdinal("PaperSalesTaxAmount"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("TotalPaperCost")))
                    TotalPaperCost += myDR.GetDecimal(myDR.GetOrdinal("TotalPaperCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("ProductionSalesTaxAmount")))
                    ProductionSalesTaxAmount += myDR.GetDecimal(myDR.GetOrdinal("ProductionSalesTaxAmount"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("VendorProductionCost")))
                    VendorProductionCost += myDR.GetDecimal(myDR.GetOrdinal("VendorProductionCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("ProductionTotal")))
                    ProductionTotal += myDR.GetDecimal(myDR.GetOrdinal("ProductionTotal"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("PolyBagCost")))
                    PolyBagCost += myDR.GetDecimal(myDR.GetOrdinal("PolyBagCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("OnsertCost")))
                    OnsertCost += myDR.GetDecimal(myDR.GetOrdinal("OnsertCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("PolybagTotal")))
                    PolybagTotal += myDR.GetDecimal(myDR.GetOrdinal("PolybagTotal"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("StitchInCost")))
                    StitchInCost += myDR.GetDecimal(myDR.GetOrdinal("StitchInCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("BlowInCost")))
                    BlowInCost += myDR.GetDecimal(myDR.GetOrdinal("BlowInCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("MailListCost")))
                    MailListCost += myDR.GetDecimal(myDR.GetOrdinal("MailListCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("InkjetCost")))
                    InkjetCost += myDR.GetDecimal(myDR.GetOrdinal("InkjetCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("InkjetMakereadyCost")))
                    InkjetMakereadyCost += myDR.GetDecimal(myDR.GetOrdinal("InkjetMakereadyCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("TotalInkJetCost")))
                    TotalInkJetCost += myDR.GetDecimal(myDR.GetOrdinal("TotalInkJetCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("CornerGuardCost")))
                    CornerGuardCost += myDR.GetDecimal(myDR.GetOrdinal("CornerGuardCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("SkidCost")))
                    SkidCost += myDR.GetDecimal(myDR.GetOrdinal("SkidCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("NumberofCartons")))
                    NumberofCartons += myDR.GetInt32(myDR.GetOrdinal("NumberofCartons"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("CartonCost")))
                    CartonCost += myDR.GetDecimal(myDR.GetOrdinal("CartonCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("InsertHandlingTotal")))
                    InsertHandlingTotal += myDR.GetDecimal(myDR.GetOrdinal("InsertHandlingTotal"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("TimeValueSlipsCost")))
                    TimeValueSlipsCost += myDR.GetDecimal(myDR.GetOrdinal("TimeValueSlipsCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("MailHouseAdminFee")))
                    MailHouseAdminFee += myDR.GetDecimal(myDR.GetOrdinal("MailHouseAdminFee"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("GluetackCost")))
                    GluetackCost += myDR.GetDecimal(myDR.GetOrdinal("GluetackCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("TabbingCost")))
                    TabbingCost += myDR.GetDecimal(myDR.GetOrdinal("TabbingCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("LetterInsertionCost")))
                    LetterInsertionCost += myDR.GetDecimal(myDR.GetOrdinal("LetterInsertionCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("OtherMailHandlingCost")))
                    OtherMailHandlingCost += myDR.GetDecimal(myDR.GetOrdinal("OtherMailHandlingCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("MailHandlingTotal")))
                    MailHandlingTotal += myDR.GetDecimal(myDR.GetOrdinal("MailHandlingTotal"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("HandlingTotal")))
                    HandlingTotal += myDR.GetDecimal(myDR.GetOrdinal("HandlingTotal"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("AssemblyTotal")))
                    AssemblyTotal += myDR.GetDecimal(myDR.GetOrdinal("AssemblyTotal"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("MailTrackingCost")))
                    MailTrackingCost += myDR.GetDecimal(myDR.GetOrdinal("MailTrackingCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("SampleFreight")))
                    SampleFreight += myDR.GetDecimal(myDR.GetOrdinal("SampleFreight"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("OtherFreight")))
                    OtherFreight += myDR.GetDecimal(myDR.GetOrdinal("OtherFreight"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("InsertFreightCost")))
                    InsertFreightCost += myDR.GetDecimal(myDR.GetOrdinal("InsertFreightCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("InsertFuelSurchargeCost")))
                    InsertFuelSurchargeCost += myDR.GetDecimal(myDR.GetOrdinal("InsertFuelSurchargeCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("InsertFreightTotalCost")))
                    InsertFreightTotalCost += myDR.GetDecimal(myDR.GetOrdinal("InsertFreightTotalCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("InsertGrossCost")))
                    InsertGrossCost += myDR.GetDecimal(myDR.GetOrdinal("InsertGrossCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("InsertDiscount")))
                    InsertDiscount += myDR.GetDecimal(myDR.GetOrdinal("InsertDiscount"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("InsertCost")))
                    InsertCost += myDR.GetDecimal(myDR.GetOrdinal("InsertCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("PostalDropCost")))
                    PostalDropCost += myDR.GetDecimal(myDR.GetOrdinal("PostalDropCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("PostalDropFuelSurchargeCost")))
                    PostalDropFuelSurchargeCost += myDR.GetDecimal(myDR.GetOrdinal("PostalDropFuelSurchargeCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("TotalPostalDropCost")))
                    TotalPostalDropCost += myDR.GetDecimal(myDR.GetOrdinal("TotalPostalDropCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("SoloPostageCost")))
                    SoloPostageCost += myDR.GetDecimal(myDR.GetOrdinal("SoloPostageCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("PolyPostageCost")))
                    PolyPostageCost += myDR.GetDecimal(myDR.GetOrdinal("PolyPostageCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("TotalPostageCost")))
                    TotalPostageCost += myDR.GetDecimal(myDR.GetOrdinal("TotalPostageCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("PostalTotal")))
                    PostalTotal += myDR.GetDecimal(myDR.GetOrdinal("PostalTotal"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("DistributionTotal")))
                    DistributionTotal += myDR.GetDecimal(myDR.GetOrdinal("DistributionTotal"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("OtherProductionCost")))
                    OtherProductionCost += myDR.GetDecimal(myDR.GetOrdinal("OtherProductionCost"));
                if (!myDR.IsDBNull(myDR.GetOrdinal("GrandTotal")))
                    GrandTotal += myDR.GetDecimal(myDR.GetOrdinal("GrandTotal"));
            }

            myDR.Close();

            writer.WriteTemplateLine("BlankRow1", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            writer.WriteTemplateLine("TotalRow1",
                    null, null, null, null, null, null,
                    
                    null, null, null, null, null,
                    
                    null,
                    null,
                    PageCount,
                    PieceWeight,
                    AdditionalPlates,

                    SoloQuantity,
                    PolyBagQuantity,
                    InsertQuantity,
                    OtherQuantity,
                    MediaQuantity,

                    SampleQuantity,
                    null,
                    SpoilageQuantity,
                    DirectMailQuantity,
                    InternalMailQuantity,

                    ExternalMailQuantity,
                    TotalProductionQuantity,
                    ProductionWeight,
                    TotalEstimateWeight,
                    null,
                    
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    CreativeCost,

                    SeparatorCost,
                    ManualPrintCost,
                    null,
                    null,
                    null,
                    
                    null, null, null, null, null,
                    
                    GrossPrintCost,
                    null,
                    EarlyPayPrintDiscountAmount,
                    NetPrintCost,
                    null,
                    
                    null,
                    PrinterSalesTaxAmount,
                    TotalPrintCost,
                    ManualPaperCost,
                    null,
                    
                    null,
                    null,
                    null, 
                    null,
                    TotalPaperPounds,
                    null,
                    
                    GrossPaperCost,
                    null,
                    EarlyPayPaperDiscountAmount,
                    NetPaperCost,
                    null,
                    
                    PaperHandlingCost,
                    null,
                    null,
                    PaperSalesTaxAmount,
                    TotalPaperCost,

                    ProductionSalesTaxAmount,
                    null,
                    null,
                    null,
                    MailListCost,

                    null,
                    VendorProductionCost,
                    ProductionTotal,
                    PolyBagCost,
                    null,

                    OnsertCost,
                    PolybagTotal,
                    null,
                    StitchInCost,
                    null,

                    BlowInCost,
                    null,
                    InkjetCost,
                    null,
                    InkjetMakereadyCost,

                    TotalInkJetCost,
                    null,
                    CornerGuardCost,
                    null,
                    SkidCost,

                    NumberofCartons,
                    null,
                    CartonCost,
                    InsertHandlingTotal,
                    null,

                    TimeValueSlipsCost,
                    null,
                    MailHouseAdminFee,
                    null,
                    GluetackCost,

                    null,
                    TabbingCost,
                    null,
                    LetterInsertionCost,
                    null,

                    OtherMailHandlingCost,
                    MailHandlingTotal,
                    HandlingTotal,
                    AssemblyTotal,
                    null,

                    MailTrackingCost,
                    SampleFreight,
                    OtherFreight,
                    null,
                    InsertFreightCost,

                    null,
                    InsertFuelSurchargeCost,
                    InsertFreightTotalCost,
                    InsertGrossCost,
                    null,
                    InsertDiscount,
                    InsertCost,
                    PostalDropCost,

                    PostalDropFuelSurchargeCost,
                    TotalPostalDropCost,
                    SoloPostageCost,
                    PolyPostageCost,
                    TotalPostageCost,

                    PostalTotal,
                    DistributionTotal,
                    OtherProductionCost,
                    GrandTotal,
                    null);

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

        #endregion

    }
}