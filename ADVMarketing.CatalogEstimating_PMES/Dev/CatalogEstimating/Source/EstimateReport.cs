using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace CatalogEstimating
{
    public class EstimateReport
    {
        public bool RunReport(ExcelWriter writer, long estimateID)
        {
            string estimateComments = null;

            using (SqlConnection myConn = (SqlConnection) MainForm.WorkingDatabase.Database.CreateConnection())
            {
                myConn.Open();

                SqlCommand myCmd = new SqlCommand("EstEstimate_s_CostSummary_ByEstimateID", myConn);
                myCmd.CommandTimeout = 600;
                myCmd.CommandType = CommandType.StoredProcedure;
                myCmd.Parameters.AddWithValue("@EST_Estimate_ID", estimateID);

                SqlDataReader myDR = myCmd.ExecuteReader();

                if (!myDR.HasRows)
                {
                    myDR.Close();
                    myConn.Close();
                    return false;
                }

                writer.WriteTemplateLine("Header1a", string.Concat("Report:     ", "Estimate"), null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header1b", string.Concat("Generated:  ", DateTime.Now.ToString()), null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("Header1b", null, null, null, null, null, null, null, null, null, null);
                writer.WriteTemplateLine("IDRow", string.Concat("Estimate ID: ", estimateID, null, null, null));
                writer.WriteTemplateHeaderColumn("Header1", 69, 5);

                while (myDR.Read())
                {
                    if (estimateComments == null)
                        estimateComments = myDR["Comments"].ToString();

                    writer.WriteTemplateColumn("RegCol", myDR["AdNumber"].ToString(), myDR["Description"].ToString(),
                        myDR["PageCount"].ToString(), myDR["Width"].ToString(), myDR["Height"].ToString(), myDR["PaperWeight"].ToString(),
                        myDR["PaperGradeDescription"].ToString(), myDR["InsertQuantity"].ToString(), myDR["SoloQuantity"].ToString(),
                        myDR["PolybagQuantity"].ToString(), myDR["OtherQuantity"].ToString(), myDR["MediaQuantity"].ToString(),
                        myDR["SampleQuantity"].ToString(), myDR["SpoilageQuantity"].ToString(), myDR["TotalProductionQuantity"].ToString(),
                        myDR["ProductionWeight"].ToString(), myDR["CreativeCost"].ToString(), myDR["SeparatorCost"].ToString(),
                        myDR["GrossPrintCost"].ToString(), myDR["EarlyPayPrintDiscountAmount"].ToString(), myDR["NetPrintCost"].ToString(),
                        myDR["PrinterTaxableMediaPct"].ToString(), myDR["PrinterSalesTaxPct"].ToString(), myDR["PrinterSalesTaxAmount"].ToString(),
                        myDR["TotalPrintCost"].ToString(), myDR["TotalPaperPounds"].ToString(), myDR["GrossPaperCost"].ToString(),
                        myDR["EarlyPayPaperDiscountAmount"].ToString(), myDR["NetPaperCost"].ToString(), myDR["PaperHandlingCost"].ToString(),
                        myDR["PaperTaxableMediaPct"].ToString(), myDR["PaperSalesTaxPct"].ToString(), myDR["PaperSalesTaxAmount"].ToString(),
                        myDR["TotalPaperCost"].ToString(), myDR["MailListCost"].ToString(),
                        myDR["VendorProductionCost"].ToString(), myDR["OtherProductionCost"].ToString(), myDR["ProductionTotal"].ToString(),
                        myDR["PolybagCost"].ToString(), myDR["OnsertCost"].ToString(), myDR["PolybagTotal"].ToString(),
                        myDR["StitchInCost"].ToString(), myDR["BlowInCost"].ToString(), myDR["InkJetCost"].ToString(),
                        myDR["InkJetMakereadyCost"].ToString(), myDR["TotalInkJetCost"].ToString(), myDR["CornerGuardCost"].ToString(),
                        myDR["SkidCost"].ToString(), myDR["CartonCost"].ToString(), myDR["InsertHandlingTotal"].ToString(),
                        myDR["GlueTackCost"].ToString(), myDR["TabbingCost"].ToString(), myDR["LetterInsertionCost"].ToString(),
                        myDR["OtherMailHandlingCost"].ToString(), myDR["MailHandlingTotal"].ToString(), myDR["HandlingTotal"].ToString(),
                        myDR["AssemblyTotal"].ToString(), myDR["InsertCost"].ToString(), myDR["InsertFreightCost"].ToString(),
                        myDR["InsertFuelSurchargeCost"].ToString(), myDR["InsertTotal"].ToString(), myDR["PostalDropCost"].ToString(),
                        myDR["PostalDropFuelSurchargeCost"].ToString(), myDR["MailTrackingCost"].ToString(), myDR["TotalPostageCost"].ToString(), myDR["PostalTotal"].ToString(),
                        myDR["SampleFreight"].ToString(), myDR["OtherFreight"].ToString(), myDR["DistributionTotal"].ToString(), myDR["GrandTotal"].ToString());
                }
                myDR.Close();
                myConn.Close();
            }

            writer.SetTemplateFreezePanes("FreezePanes");

            //TODO: Get the Estimate Comments from the datareader above
            writer.WriteTemplateRowRegion("CommentRow", 4, 9, 75, 1);
            writer.WriteLine(75, new object[] { null, null, null, null, estimateComments });
            return true;
        }
    }
}
