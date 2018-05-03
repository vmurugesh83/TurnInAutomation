using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CatalogEstimating.Properties;
using CatalogEstimating.Datasets;
using System.Data.SqlClient;

namespace CatalogEstimating.UserControls.Estimate
{
    public partial class ucpCostSummary : CatalogEstimating.UserControlPanel
    {
        private Datasets.Estimates _dsEstimates;
        SourceGrid.Cells.Views.Cell _vwCell = new SourceGrid.Cells.Views.Cell();

        public ucpCostSummary(Datasets.Estimates dsEstimates)
        {
            InitializeComponent();
            Name = "Cost Summary";

            _vwCell.BackColor = System.Drawing.SystemColors.Control;
            _vwCell.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Gray), new DevAge.Drawing.BorderLine(Color.Gray));
            _vwCell.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

            _dsEstimates = dsEstimates;
    
            _gridCostSummary.Redim(74, 1);

            InitRowHeaders();
        }

        private void InitRowHeaders()
        {
            _gridCostSummary.Columns[0].Width = 200;
            _gridCostSummary[1, 0] = new SourceGrid.Cells.RowHeader("Component ID");
            _gridCostSummary[2, 0] = new SourceGrid.Cells.RowHeader("Creative Cost");
            _gridCostSummary[3, 0] = new SourceGrid.Cells.RowHeader("Separator Cost");
            _gridCostSummary[4, 0] = new SourceGrid.Cells.RowHeader("Media Qty");
            _gridCostSummary[5, 0] = new SourceGrid.Cells.RowHeader("Sample Qty");
            _gridCostSummary[6, 0] = new SourceGrid.Cells.RowHeader("Spoilage Qty");
            _gridCostSummary[7, 0] = new SourceGrid.Cells.RowHeader("- Net Print");
            _gridCostSummary[7, 0].AddController(new CatalogEstimating.CustomGrids.Controllers.ExpandCollapseController(true, 12, "- Net Print", "+ Net Print"));
            _gridCostSummary[8, 0] = new SourceGrid.Cells.RowHeader("  + Print");
            _gridCostSummary[8, 0].AddController(new CatalogEstimating.CustomGrids.Controllers.ExpandCollapseController(false, 10, "  - Print", "  + Print"));
            _gridCostSummary[9, 0] = new SourceGrid.Cells.RowHeader("    Manual Print Cost");
            _gridCostSummary[10, 0] = new SourceGrid.Cells.RowHeader("    Run Rate");
            _gridCostSummary[11, 0] = new SourceGrid.Cells.RowHeader("    # of Plants");
            _gridCostSummary[12, 0] = new SourceGrid.Cells.RowHeader("    Stitcher Makeready Cost");
            _gridCostSummary[13, 0] = new SourceGrid.Cells.RowHeader("    Press Makeready Cost");
            _gridCostSummary[14, 0] = new SourceGrid.Cells.RowHeader("    Additional Plates");
            _gridCostSummary[15, 0] = new SourceGrid.Cells.RowHeader("    Plate Cost");
            _gridCostSummary[16, 0] = new SourceGrid.Cells.RowHeader("    Number Digital H&P");
            _gridCostSummary[17, 0] = new SourceGrid.Cells.RowHeader("    Digital H&P Rate");
            _gridCostSummary[18, 0] = new SourceGrid.Cells.RowHeader("    Replacement Plate Cost");
            _gridCostSummary[19, 0] = new SourceGrid.Cells.RowHeader("  Discount");

            _gridCostSummary[20, 0] = new SourceGrid.Cells.RowHeader("- Net Paper");
            _gridCostSummary[20, 0].AddController(new CatalogEstimating.CustomGrids.Controllers.ExpandCollapseController(true, 10, "- Net Paper", "+ Net Paper"));
            _gridCostSummary[21, 0] = new SourceGrid.Cells.RowHeader("  + Paper");
            _gridCostSummary[21, 0].AddController(new CatalogEstimating.CustomGrids.Controllers.ExpandCollapseController(false, 8, "  - Paper", "  + Paper"));
            _gridCostSummary[22, 0] = new SourceGrid.Cells.RowHeader("    Manual Paper Cost");
            _gridCostSummary[23, 0] = new SourceGrid.Cells.RowHeader("    Run Pounds");
            _gridCostSummary[24, 0] = new SourceGrid.Cells.RowHeader("    Makeready Pounds");
            _gridCostSummary[25, 0] = new SourceGrid.Cells.RowHeader("    Plate Change Pounds");
            _gridCostSummary[26, 0] = new SourceGrid.Cells.RowHeader("    # of Press Stops");
            _gridCostSummary[27, 0] = new SourceGrid.Cells.RowHeader("    Press Stop Pounds");
            _gridCostSummary[28, 0] = new SourceGrid.Cells.RowHeader("    Total Paper Pounds");
            _gridCostSummary[29, 0] = new SourceGrid.Cells.RowHeader("    Paper CWT Rate");
            _gridCostSummary[30, 0] = new SourceGrid.Cells.RowHeader("  Discount");
            _gridCostSummary[31, 0] = new SourceGrid.Cells.RowHeader("Paper Handling");
            _gridCostSummary[32, 0] = new SourceGrid.Cells.RowHeader("Sales Tax");
            _gridCostSummary[33, 0] = new SourceGrid.Cells.RowHeader("Mail Lists");
            _gridCostSummary[34, 0] = new SourceGrid.Cells.RowHeader("Vendor Cost");
            _gridCostSummary[35, 0] = new SourceGrid.Cells.RowHeader("Other Cost");
            _gridCostSummary[36, 0] = new SourceGrid.Cells.RowHeader("Production Cost");

            // Blank Line
            _gridCostSummary.Rows[37].Height = 10;
            _gridCostSummary[37, 0] = new SourceGrid.Cells.Cell();
            _gridCostSummary[37, 0].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);

            _gridCostSummary[38, 0] = new SourceGrid.Cells.RowHeader("Onsert");
            _gridCostSummary[39, 0] = new SourceGrid.Cells.RowHeader("Polybag");
            _gridCostSummary[40, 0] = new SourceGrid.Cells.RowHeader("Stitch-In");
            _gridCostSummary[41, 0] = new SourceGrid.Cells.RowHeader("Blow-In");
            _gridCostSummary[42, 0] = new SourceGrid.Cells.RowHeader("Inkjet");
            _gridCostSummary[43, 0] = new SourceGrid.Cells.RowHeader("- Handling");
            _gridCostSummary[43, 0].AddController(new CatalogEstimating.CustomGrids.Controllers.ExpandCollapseController(true, 11, "- Handling", "+ Handling"));
            _gridCostSummary[44, 0] = new SourceGrid.Cells.RowHeader("  + Direct Mail");
            _gridCostSummary[44, 0].AddController(new CatalogEstimating.CustomGrids.Controllers.ExpandCollapseController(false, 6, "  - Direct Mail", "  + Direct Mail"));
            _gridCostSummary[45, 0] = new SourceGrid.Cells.RowHeader("    Time Value Slips");
            _gridCostSummary[46, 0] = new SourceGrid.Cells.RowHeader("    Mailhouse Admin Fee");
            _gridCostSummary[47, 0] = new SourceGrid.Cells.RowHeader("    Gluetacking");
            _gridCostSummary[48, 0] = new SourceGrid.Cells.RowHeader("    Tabbing");
            _gridCostSummary[49, 0] = new SourceGrid.Cells.RowHeader("    Letter Insertion");
            _gridCostSummary[50, 0] = new SourceGrid.Cells.RowHeader("    Other Mail Handling");
            _gridCostSummary[51, 0] = new SourceGrid.Cells.RowHeader("  + Insert");
            _gridCostSummary[51, 0].AddController(new CatalogEstimating.CustomGrids.Controllers.ExpandCollapseController(false, 3, "  - Insert", "  + Insert"));
            _gridCostSummary[52, 0] = new SourceGrid.Cells.RowHeader("    Corner Guard");
            _gridCostSummary[53, 0] = new SourceGrid.Cells.RowHeader("    Skid");
            _gridCostSummary[54, 0] = new SourceGrid.Cells.RowHeader("    Carton");
            _gridCostSummary[55, 0] = new SourceGrid.Cells.RowHeader("Assembly Cost");

            // Blank Line
            _gridCostSummary.Rows[56].Height = 10;
            _gridCostSummary[56, 0] = new SourceGrid.Cells.Cell();
            _gridCostSummary[56, 0].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);

            _gridCostSummary[57, 0] = new SourceGrid.Cells.RowHeader("- Direct Mail");
            _gridCostSummary[57, 0].AddController(new CatalogEstimating.CustomGrids.Controllers.ExpandCollapseController(true, 6, "- Direct Mail", "+ Direct Mail"));
            _gridCostSummary[58, 0] = new SourceGrid.Cells.RowHeader("  Postal Drop");
            _gridCostSummary[59, 0] = new SourceGrid.Cells.RowHeader("  Postal Drop Fuel Surcharge");
            _gridCostSummary[60, 0] = new SourceGrid.Cells.RowHeader("  Mail Tracking");
            _gridCostSummary[61, 0] = new SourceGrid.Cells.RowHeader("  + Postage");
            _gridCostSummary[61, 0].AddController(new CatalogEstimating.CustomGrids.Controllers.ExpandCollapseController(false, 2, "  - Postage", "  + Postage"));
            _gridCostSummary[62, 0] = new SourceGrid.Cells.RowHeader("    Solo Mail");
            _gridCostSummary[63, 0] = new SourceGrid.Cells.RowHeader("    Poly Mail");
            _gridCostSummary[64, 0] = new SourceGrid.Cells.RowHeader("- Publication");
            _gridCostSummary[64, 0].AddController(new CatalogEstimating.CustomGrids.Controllers.ExpandCollapseController(false, 4, "- Publication", "+ Publication"));
            _gridCostSummary[65, 0] = new SourceGrid.Cells.RowHeader("  Insert");
            _gridCostSummary[66, 0] = new SourceGrid.Cells.RowHeader("  + Freight");
            _gridCostSummary[66, 0].AddController(new CatalogEstimating.CustomGrids.Controllers.ExpandCollapseController(false, 2, "  - Freight", "  + Freight"));
            _gridCostSummary[67, 0] = new SourceGrid.Cells.RowHeader("    Freight");
            _gridCostSummary[68, 0] = new SourceGrid.Cells.RowHeader("    Fuel Surcharge");
            _gridCostSummary[69, 0] = new SourceGrid.Cells.RowHeader("Sample Freight");
            _gridCostSummary[70, 0] = new SourceGrid.Cells.RowHeader("Other Freight");
            _gridCostSummary[71, 0] = new SourceGrid.Cells.RowHeader("Distribution Cost");

            // Blank Line
            _gridCostSummary.Rows[72].Height = 10;
            _gridCostSummary[72, 0] = new SourceGrid.Cells.Cell();
            _gridCostSummary[72, 0].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);

            _gridCostSummary[73, 0] = new SourceGrid.Cells.RowHeader("Total");

            for (int i = 9; i < 19; ++i)
                _gridCostSummary.Rows[i].Visible = false;

            for (int i = 22; i < 30; ++i)
                _gridCostSummary.Rows[i].Visible = false;

            for (int i = 45; i < 51; ++i)
                _gridCostSummary.Rows[i].Visible = false;

            _gridCostSummary.Rows[52].Visible = false;
            _gridCostSummary.Rows[53].Visible = false;
            _gridCostSummary.Rows[54].Visible = false;

            _gridCostSummary.Rows[62].Visible = false;
            _gridCostSummary.Rows[63].Visible = false;

            _gridCostSummary.Rows[67].Visible = false;
            _gridCostSummary.Rows[68].Visible = false;
        }

        #region Overrides

        public override ToolStrip Toolbar
        {
            get { return _toolStrip; }
        }

        public override void LoadData()
        {
            FillGrids();
        }

        public override void Export(ref ExcelWriter writer)
        {
            bool excelErrorOccured = false;

            try
            {
                writer = new ExcelWriter("Estimate", "ReportTemplate.xls");

                EstimateReport reportEstimate = new EstimateReport();
                if (reportEstimate.RunReport(writer, _dsEstimates.est_estimate[0].est_estimate_id))
                {
                    writer.AutoFitCells();
                    ReportWriter.SaveReportToDatabase( 4, writer );
                    writer.Show();
                }
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                writer.Quit();
                excelErrorOccured = true;
            }
            finally
            {
                writer.Dispose();
            }

            if (excelErrorOccured)
            {
                MessageBox.Show(this, "An unknown error occured while creating the Excel document", "Reports", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            base.Export(ref writer);
        }

        #endregion

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FillGrids();
        }

        private void FillGrids()
        {
            // Remove the old columns
            if (_gridCostSummary.ColumnsCount > 1)
                _gridCostSummary.Columns.RemoveRange(1, (_gridCostSummary.ColumnsCount - 1));

            long estimateID = _dsEstimates.est_estimate[0].est_estimate_id;

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("EstEstimate_s_CostSummary_ByEstimateID", conn);
                cmd.CommandTimeout = 4000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EST_Estimate_ID", estimateID);
                SqlDataReader dr = cmd.ExecuteReader();

                int colNumber = -1;
                int colIndex = 0;
                while (dr.Read())
                {
                    _gridCostSummary.Columns.Insert(++colIndex);
                    ++colNumber;

                    _gridCostSummary.Columns[colIndex].Width = 100;
                    // Total Cost Column
                    if (colNumber == 0)
                    {
                        _gridCostSummary[0, colIndex] = new SourceGrid.Cells.Cell("Total Cost");
                        _gridCostSummary[0, colIndex].AddController(new SourceGrid.Cells.Controllers.MouseInvalidate());
                        _gridCostSummary[0, colIndex].AddController(new SourceGrid.Cells.Controllers.Resizable(SourceGrid.CellResizeMode.Width));
                        _gridCostSummary[0, colIndex].View = SourceGrid.Cells.Views.ColumnHeader.Default;
                    }
                    // Component Columns
                    else
                    {
                        _gridCostSummary[0, colIndex] = new SourceGrid.Cells.Cell(dr["Description"].ToString());
                        _gridCostSummary[0, colIndex].AddController(new SourceGrid.Cells.Controllers.MouseInvalidate());
                        _gridCostSummary[0, colIndex].AddController(new SourceGrid.Cells.Controllers.Resizable(SourceGrid.CellResizeMode.Width));
                        _gridCostSummary[0, colIndex].View = SourceGrid.Cells.Views.ColumnHeader.Default;
                    }
                    _gridCostSummary[1, colIndex] = new SourceGrid.Cells.Cell(dr["EST_Component_ID"].ToString());
                    _gridCostSummary[1, colIndex].View = _vwCell;

                    if (dr.IsDBNull(dr.GetOrdinal("CreativeCost")))
                        BlankCell(2, colIndex);
                    else
                        FillCell(2, colIndex, dr.GetDecimal(dr.GetOrdinal("CreativeCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("SeparatorCost")))
                        BlankCell(3, colIndex);
                    else
                        FillCell(3, colIndex, dr.GetDecimal(dr.GetOrdinal("SeparatorCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("MediaQuantity")))
                        BlankCell(4, colIndex);
                    else
                        FillCell(4, colIndex, dr.GetInt32(dr.GetOrdinal("MediaQuantity")));
                    if (dr.IsDBNull(dr.GetOrdinal("SampleQuantity")))
                        BlankCell(5, colIndex);
                    else
                        FillCell(5, colIndex, dr.GetInt32(dr.GetOrdinal("SampleQuantity")));
                    if (dr.IsDBNull(dr.GetOrdinal("SpoilageQuantity")))
                        BlankCell(6, colIndex);
                    else
                        FillCell(6, colIndex, dr.GetInt32(dr.GetOrdinal("SpoilageQuantity")));
                    if (dr.IsDBNull(dr.GetOrdinal("NetPrintCost")))
                        BlankCell(7, colIndex);
                    else
                        FillCell(7, colIndex, dr.GetDecimal(dr.GetOrdinal("NetPrintCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("GrossPrintCost")))
                        BlankCell(8, colIndex);
                    else
                        FillCell(8, colIndex, dr.GetDecimal(dr.GetOrdinal("GrossPrintCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("ManualPrintCost")))
                        BlankCell(9, colIndex);
                    else
                        FillCell(9, colIndex, dr.GetDecimal(dr.GetOrdinal("ManualPrintCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("RunRate")))
                        BlankCell(10, colIndex);
                    else
                        FillCell(10, colIndex, dr.GetDecimal(dr.GetOrdinal("RunRate")));
                    _gridCostSummary[11, colIndex] = new SourceGrid.Cells.Cell(dr["NumberOfPlants"].ToString());
                    _gridCostSummary[11, colIndex].View = _vwCell;

                    if (dr.IsDBNull(dr.GetOrdinal("StitcherMakereadyCost")))
                        BlankCell(12, colIndex);
                    else
                        FillCell(12, colIndex, dr.GetDecimal(dr.GetOrdinal("StitcherMakereadyCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("PressMakereadyCost")))
                        BlankCell(13, colIndex);
                    else
                        FillCell(13, colIndex, dr.GetDecimal(dr.GetOrdinal("PressMakereadyCost")));
                    _gridCostSummary[14, colIndex] = new SourceGrid.Cells.Cell(dr["AdditionalPlates"].ToString());
                    _gridCostSummary[14, colIndex].View = _vwCell;
                    if (dr.IsDBNull(dr.GetOrdinal("PrinterPlateCost")))
                        BlankCell(15, colIndex);
                    else
                        FillCell(15, colIndex, dr.GetDecimal(dr.GetOrdinal("PrinterPlateCost")));
                    _gridCostSummary[16, colIndex] = new SourceGrid.Cells.Cell(dr["NumberDigitalHandlePrepare"].ToString());
                    _gridCostSummary[16, colIndex].View = _vwCell;
                    if (dr.IsDBNull(dr.GetOrdinal("DigitalHandlePrepareRate")))
                        BlankCell(17, colIndex);
                    else
                        FillCell(17, colIndex, dr.GetDecimal(dr.GetOrdinal("DigitalHandlePrepareRate")));
                    if (dr.IsDBNull(dr.GetOrdinal("ReplacementPlateCost")))
                        BlankCell(18, colIndex);
                    else
                        FillCell(18, colIndex, dr.GetDecimal(dr.GetOrdinal("ReplacementPlateCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("EarlyPayPrintDiscountAmount")))
                        BlankCell(19, colIndex);
                    else
                        FillCell(19, colIndex, dr.GetDecimal(dr.GetOrdinal("EarlyPayPrintDiscountAmount")));
                    if (dr.IsDBNull(dr.GetOrdinal("NetPaperCost")))
                        BlankCell(20, colIndex);
                    else
                        FillCell(20, colIndex, dr.GetDecimal(dr.GetOrdinal("NetPaperCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("GrossPaperCost")))
                        BlankCell(21, colIndex);
                    else
                        FillCell(21, colIndex, dr.GetDecimal(dr.GetOrdinal("GrossPaperCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("ManualPaperCost")))
                        BlankCell(22, colIndex);
                    else
                        FillCell(22, colIndex, dr.GetDecimal(dr.GetOrdinal("ManualPaperCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("RunPounds")))
                        BlankCell(23, colIndex);
                    else
                        FillCellDecimal2(23, colIndex, dr.GetDecimal(dr.GetOrdinal("RunPounds")));
                    if (dr.IsDBNull(dr.GetOrdinal("MakereadyPounds")))
                        BlankCell(24, colIndex);
                    else
                        FillCell(24, colIndex, dr.GetInt32(dr.GetOrdinal("MakereadyPounds")));
                    if (dr.IsDBNull(dr.GetOrdinal("PlateChangePounds")))
                        BlankCell(25, colIndex);
                    else
                        FillCellDecimal2(25, colIndex, dr.GetDecimal(dr.GetOrdinal("PlateChangePounds")));
                    _gridCostSummary[26, colIndex] = new SourceGrid.Cells.Cell(dr["NumberOfPressStops"].ToString());
                    _gridCostSummary[26, colIndex].View = _vwCell;
                    if (dr.IsDBNull(dr.GetOrdinal("PressStopPounds")))
                        BlankCell(27, colIndex);
                    else
                        FillCell(27, colIndex, dr.GetInt32(dr.GetOrdinal("PressStopPounds")));
                    if (dr.IsDBNull(dr.GetOrdinal("TotalPaperPounds")))
                        BlankCell(28, colIndex);
                    else
                        FillCellDecimal2(28, colIndex, dr.GetDecimal(dr.GetOrdinal("TotalPaperPounds")));
                    if (dr.IsDBNull(dr.GetOrdinal("PaperCwtRate")))
                        BlankCell(29, colIndex);
                    else
                        FillCell(29, colIndex, dr.GetDecimal(dr.GetOrdinal("PaperCWTRate")));
                    if (dr.IsDBNull(dr.GetOrdinal("EarlyPayPaperDiscountAmount")))
                        BlankCell(30, colIndex);
                    else
                        FillCell(30, colIndex, dr.GetDecimal(dr.GetOrdinal("EarlyPayPaperDiscountAmount")));
                    if (dr.IsDBNull(dr.GetOrdinal("PaperHandlingCost")))
                        BlankCell(31, colIndex);
                    else
                        FillCell(31, colIndex, dr.GetDecimal(dr.GetOrdinal("PaperHandlingCost")));
                    decimal salesTaxAmount = 0;
                    if (!dr.IsDBNull(dr.GetOrdinal("PrinterSalesTaxAmount")))
                        salesTaxAmount += dr.GetDecimal(dr.GetOrdinal("PrinterSalesTaxAmount"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PaperSalesTaxAmount")))
                        salesTaxAmount += dr.GetDecimal(dr.GetOrdinal("PaperSalesTaxAmount"));
                    if (salesTaxAmount > 0)
                        FillCell(32, colIndex, salesTaxAmount);
                    else
                        BlankCell(32, colIndex);
                    if (dr.IsDBNull(dr.GetOrdinal("MailListCost")))
                        BlankCell(33, colIndex);
                    else
                        FillCell(33, colIndex, dr.GetDecimal(dr.GetOrdinal("MailListCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("VendorProductionCost")))
                        BlankCell(34, colIndex);
                    else
                        FillCell(34, colIndex, dr.GetDecimal(dr.GetOrdinal("VendorProductionCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("OtherProductionCost")))
                        BlankCell(35, colIndex);
                    else
                        FillCell(35, colIndex, dr.GetDecimal(dr.GetOrdinal("OtherProductionCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("ProductionTotal")))
                        BlankCell(36, colIndex);
                    else
                        FillCell(36, colIndex, dr.GetDecimal(dr.GetOrdinal("ProductionTotal")));
                    if (dr.IsDBNull(dr.GetOrdinal("OnsertCost")))
                        BlankCell(38, colIndex);
                    else
                        FillCell(38, colIndex, dr.GetDecimal(dr.GetOrdinal("OnsertCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("PolybagCost")))
                        BlankCell(39, colIndex);
                    else
                        FillCell(39, colIndex, dr.GetDecimal(dr.GetOrdinal("PolybagCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("StitchInCost")))
                        BlankCell(40, colIndex);
                    else
                        FillCell(40, colIndex, dr.GetDecimal(dr.GetOrdinal("StitchInCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("BlowInCost")))
                        BlankCell(41, colIndex);
                    else
                        FillCell(41, colIndex, dr.GetDecimal(dr.GetOrdinal("BlowInCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("TotalInkjetCost")))
                        BlankCell(42, colIndex);
                    else
                        FillCell(42, colIndex, dr.GetDecimal(dr.GetOrdinal("TotalInkjetCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("HandlingTotal")))
                        BlankCell(43, colIndex);
                    else
                        FillCell(43, colIndex, dr.GetDecimal(dr.GetOrdinal("HandlingTotal")));
                    if (dr.IsDBNull(dr.GetOrdinal("MailHandlingTotal")))
                        BlankCell(44, colIndex);
                    else
                        FillCell(44, colIndex, dr.GetDecimal(dr.GetOrdinal("MailHandlingTotal")));
                    if (dr.IsDBNull(dr.GetOrdinal("TimeValueSlipsCost")))
                        BlankCell(45, colIndex);
                    else
                        FillCell(45, colIndex, dr.GetDecimal(dr.GetOrdinal("TimeValueSlipsCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("MailHouseAdminFee")))
                        BlankCell(46, colIndex);
                    else
                        FillCell(46, colIndex, dr.GetDecimal(dr.GetOrdinal("MailHouseAdminFee")));
                    if (dr.IsDBNull(dr.GetOrdinal("GlueTackCost")))
                        BlankCell(47, colIndex);
                    else
                        FillCell(47, colIndex, dr.GetDecimal(dr.GetOrdinal("GlueTackCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("TabbingCost")))
                        BlankCell(48, colIndex);
                    else
                        FillCell(48, colIndex, dr.GetDecimal(dr.GetOrdinal("TabbingCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("LetterInsertionCost")))
                        BlankCell(49, colIndex);
                    else
                        FillCell(49, colIndex, dr.GetDecimal(dr.GetOrdinal("LetterInsertionCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("OtherMailHandlingCost")))
                        BlankCell(50, colIndex);
                    else
                        FillCell(50, colIndex, dr.GetDecimal(dr.GetOrdinal("OtherMailHandlingCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("InsertHandlingTotal")))
                        BlankCell(51, colIndex);
                    else
                        FillCell(51, colIndex, dr.GetDecimal(dr.GetOrdinal("InsertHandlingTotal")));
                    if (dr.IsDBNull(dr.GetOrdinal("CornerGuardCost")))
                        BlankCell(52, colIndex);
                    else
                        FillCell(52, colIndex, dr.GetDecimal(dr.GetOrdinal("CornerGuardCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("SkidCost")))
                        BlankCell(53, colIndex);
                    else
                        FillCell(53, colIndex, dr.GetDecimal(dr.GetOrdinal("SkidCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("CartonCost")))
                        BlankCell(54, colIndex);
                    else
                        FillCell(54, colIndex, dr.GetDecimal(dr.GetOrdinal("CartonCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("AssemblyTotal")))
                        BlankCell(55, colIndex);
                    else
                        FillCell(55, colIndex, dr.GetDecimal(dr.GetOrdinal("AssemblyTotal")));
                    if (dr.IsDBNull(dr.GetOrdinal("PostalTotal")))
                        BlankCell(57, colIndex);
                    else
                        FillCell(57, colIndex, dr.GetDecimal(dr.GetOrdinal("PostalTotal")));
                    if (dr.IsDBNull(dr.GetOrdinal("PostalDropCost")))
                        BlankCell(58, colIndex);
                    else
                        FillCell(58, colIndex, dr.GetDecimal(dr.GetOrdinal("PostalDropCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("PostalDropFuelSurchargeCost")))
                        BlankCell(59, colIndex);
                    else
                        FillCell(59, colIndex, dr.GetDecimal(dr.GetOrdinal("PostalDropFuelSurchargeCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("MailTrackingCost")))
                        BlankCell(60, colIndex);
                    else
                        FillCell(60, colIndex, dr.GetDecimal(dr.GetOrdinal("MailTrackingCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("TotalPostageCost")))
                        BlankCell(61, colIndex);
                    else
                        FillCell(61, colIndex, dr.GetDecimal(dr.GetOrdinal("TotalPostageCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("SoloPostageCost")))
                        BlankCell(62, colIndex);
                    else
                        FillCell(62, colIndex, dr.GetDecimal(dr.GetOrdinal("SoloPostageCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("PolyPostageCost")))
                        BlankCell(63, colIndex);
                    else
                        FillCell(63, colIndex, dr.GetDecimal(dr.GetOrdinal("PolyPostageCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("InsertTotal")))
                        BlankCell(64, colIndex);
                    else
                        FillCell(64, colIndex, dr.GetDecimal(dr.GetOrdinal("InsertTotal")));
                    if (dr.IsDBNull(dr.GetOrdinal("InsertCost")))
                        BlankCell(65, colIndex);
                    else
                        FillCell(65, colIndex, dr.GetDecimal(dr.GetOrdinal("InsertCost")));

                    decimal totalInsertFreightCost = 0;
                    if (!dr.IsDBNull(dr.GetOrdinal("InsertFreightCost")))
                        totalInsertFreightCost += dr.GetDecimal(dr.GetOrdinal("InsertFreightCost"));
                    if (!dr.IsDBNull(dr.GetOrdinal("InsertFuelSurchargeCost")))
                        totalInsertFreightCost += dr.GetDecimal(dr.GetOrdinal("InsertFuelSurchargeCost"));
                    if (totalInsertFreightCost > 0)
                        FillCell(66, colIndex, totalInsertFreightCost);
                    else
                        BlankCell(66, colIndex);

                    if (dr.IsDBNull(dr.GetOrdinal("InsertFreightCost")))
                        BlankCell(67, colIndex);
                    else
                        FillCell(67, colIndex, dr.GetDecimal(dr.GetOrdinal("InsertFreightCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("InsertFuelSurchargeCost")))
                        BlankCell(68, colIndex);
                    else
                        FillCell(68, colIndex, dr.GetDecimal(dr.GetOrdinal("InsertFuelSurchargeCost")));
                    if (dr.IsDBNull(dr.GetOrdinal("SampleFreight")))
                        BlankCell(69, colIndex);
                    else
                        FillCell(69, colIndex, dr.GetDecimal(dr.GetOrdinal("SampleFreight")));
                    if (dr.IsDBNull(dr.GetOrdinal("OtherFreight")))
                        BlankCell(70, colIndex);
                    else
                        FillCell(70, colIndex, dr.GetDecimal(dr.GetOrdinal("OtherFreight")));
                    if (dr.IsDBNull(dr.GetOrdinal("DistributionTotal")))
                        BlankCell(71, colIndex);
                    else
                        FillCell(71, colIndex, dr.GetDecimal(dr.GetOrdinal("DistributionTotal")));

                    if (dr.IsDBNull(dr.GetOrdinal("GrandTotal")))
                        BlankCell(73, colIndex);
                    else
                        FillCell(73, colIndex, dr.GetDecimal(dr.GetOrdinal("GrandTotal")));
                }

                dr.Close();
                conn.Close();
            }
        }

        private void BlankCell(int row, int col)
        {
            _gridCostSummary[row, col] = new SourceGrid.Cells.Cell("-");
            _gridCostSummary[row, col].View = _vwCell;
        }

        private void FillCellDecimal2(int row, int col, decimal value)
        {
            _gridCostSummary[row, col] = new SourceGrid.Cells.Cell(value.ToString("n2"));
            _gridCostSummary[row, col].View = _vwCell;
        }

        private void FillCell(int row, int col, decimal value)
        {
            _gridCostSummary[row, col] = new SourceGrid.Cells.Cell(value.ToString("c"));
            _gridCostSummary[row, col].View = _vwCell;
        }

        private void FillCell(int row, int col, int value)
        {
            string sval = value.ToString("n");

            if (sval.IndexOf(".") > -1)
                sval = sval.Substring(0, sval.IndexOf("."));

            _gridCostSummary[row, col] = new SourceGrid.Cells.Cell(sval);
            _gridCostSummary[row, col].View = _vwCell;
        }
    }
}