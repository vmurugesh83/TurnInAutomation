using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.EstimatesTableAdapters;

namespace CatalogEstimating
{
    [Serializable()]
    public class CopyComponent
    {
        #region Private Members
        private long _originalComponentID;
        private string _Description;
        private string _Comments;
        private string _FinancialChangeComments;
        private int? _AdNumber;
        private int? _EstimateMediaTypeID;
        private int? _ComponentTypeID;
        private int? _MediaQtywoInsert;
        private decimal? _SpoilagePct;
        private int? _PageCount;
        private decimal? _Width;
        private decimal? _Height;
        private bool _VendorSupplied;
        private string _VendorSuppliedDesc;
        private decimal? _VendorSuppliedCPM;
        private string _CreativeDesc;
        private decimal? _CreativeCPP;
        private string _SeparatorDesc;
        private decimal? _SeparatorCPP;
        private string _PrinterDesc;
        private string _AssemblyVendorDesc;
        private bool _CalcPrintCost;
        private decimal? _ManualPrintCost;
        private int? _NumberofPlants;
        private int? _AdditionalPlates;
        private string _PlateCostDesc;
        private decimal? _ReplacementPlateCost;
        private decimal? _RunRate;
        private int? _NumberDigitalHandleandPrepare;
        private string _DigitalHandleandPrepareDesc;
        private string _StitchInDesc;
        private string _BlowInDesc;
        private string _OnsertDesc;
        private string _StitcherMakereadyDesc;
        private decimal? _StitcherMakereadyRate;
        private string _PressMakereadyDesc;
        private decimal? _PressMakereadyRate;
        private decimal? _EarlyPayPrintDiscount;
        private bool _PrinterApplyTax;
        private decimal? _PrinterTaxableMediaPct;
        private decimal? _PrinterSalesTaxPct;
        private string _PaperDesc;
        private string _PaperMapDesc;
        private int? _PaperWeight;
        private string _PaperGrade;
        private bool _CalcPaperCost;
        private decimal? _ManualPaperCost;
        private decimal? _RunPounds;
        private int? _MakereadyPounds;
        private decimal? _PlateChangePounds;
        private int? _PressStopPounds;
        private int? _NumberofPressStops;
        private decimal? _EarlyPayPaperDiscount;
        private bool _PaperApplyTax;
        private decimal? _PaperTaxableMediaPct;
        private decimal? _PaperSalesTaxPct;
        private decimal? _OtherProduction;
        #endregion

        #region Construction
        public CopyComponent()
        {
        }
        #endregion

        #region Public Methods
        public Estimates.est_componentRow CreateDatasetRecord(SqlConnection conn, Estimates dsEstimate, out string ErrorDescription)
        {
            // First, determine the foreign keys based on the descriptions from the original record.
            // If we cannot find a match return null

            long? VendorSupplied_ID = null;
            long? CreativeVendor_ID = null;
            long? Separator_ID = null;
            long? Printer_ID = null;
            long? AssemblyVendor_ID = null;
            long? PlateCost_ID = null;
            long? StitchIn_ID = null;
            long? BlowIn_ID = null;
            long? Onsert_ID = null;
            long? StitcherMakeready_ID = null;
            long? PressMakeready_ID = null;
            long? DigitalHandleandPrepare_ID = null;
            long? Paper_ID = null;
            long? PaperMap_ID = null;
            int? PaperWeight_ID = null;
            int? PaperGrade_ID = null;

            if (!string.IsNullOrEmpty(_VendorSuppliedDesc))
            {
                VendorSupplied_ID = FindVendor(conn, _VendorSuppliedDesc, 9);
                if (VendorSupplied_ID == null)
                {
                    ErrorDescription = "Vendor Supplied Vendor: " + _VendorSuppliedDesc + " not found in destination database.";
                    return null;
                }
            }

            if (!string.IsNullOrEmpty(_CreativeDesc))
            {
                CreativeVendor_ID = FindVendor(conn, _CreativeDesc, 3);
                if (CreativeVendor_ID == null)
                {
                    ErrorDescription = "Creative Vendor: " + _CreativeDesc + " not found in destination database.";
                    return null;
                }
            }

            if (!string.IsNullOrEmpty(_SeparatorDesc))
            {
                Separator_ID = FindVendor(conn, _SeparatorDesc, 4);
                if (Separator_ID == null)
                {
                    ErrorDescription = "Separator Vendor: " + _SeparatorDesc + " not found in destination database.";
                    return null;
                }
            }

            if (!string.IsNullOrEmpty(_PrinterDesc))
            {
                Printer_ID = FindPrinter(conn, _PrinterDesc, dsEstimate.est_estimate[0].rundate);
                if (Printer_ID == null)
                {
                    ErrorDescription = "Printer Rates for Vendor: " + _PrinterDesc + " not found in destination database for Run Date: " + dsEstimate.est_estimate[0].rundate.ToShortDateString() + ".";
                    return null;
                }
            }

            if (!string.IsNullOrEmpty(_AssemblyVendorDesc))
            {
                AssemblyVendor_ID = FindPrinter(conn, _AssemblyVendorDesc, dsEstimate.est_estimate[0].rundate);
                if (AssemblyVendor_ID == null)
                {
                    ErrorDescription = "Printer Rates for Vendor: " + _AssemblyVendorDesc + " not found in destination database for Run Date: " + dsEstimate.est_estimate[0].rundate.ToShortDateString() + ".";
                    return null;
                }
            }

            if (!string.IsNullOrEmpty(_PlateCostDesc))
            {
                PlateCost_ID = FindPrinterRate(conn, Printer_ID.Value, _PlateCostDesc, 8);
                if (PlateCost_ID == null)
                {
                    ErrorDescription = "Plate Cost Rate for Printer Vendor: " + _PrinterDesc + " not found in destination database for Run Date: " + dsEstimate.est_estimate[0].rundate.ToShortDateString() + ".";
                    return null;
                }
            }

            if (!string.IsNullOrEmpty(_DigitalHandleandPrepareDesc))
            {
                DigitalHandleandPrepare_ID = FindPrinterRate(conn, Printer_ID.Value, _DigitalHandleandPrepareDesc, 5);
                if (DigitalHandleandPrepare_ID == null)
                {
                    ErrorDescription = "Digital Handle & Prepare Rate for Printer Vendor: " + _PrinterDesc + " not found in destination database for Run Date: " + dsEstimate.est_estimate[0].rundate.ToShortDateString() + ".";
                    return null;
                }
            }

            if (!string.IsNullOrEmpty(_StitchInDesc))
            {
                StitchIn_ID = FindPrinterRate(conn, AssemblyVendor_ID.Value, _StitchInDesc, 1);
                if (StitchIn_ID == null)
                {
                    ErrorDescription = "Stitch-In Rate for Assembly Vendor: " + _AssemblyVendorDesc + " not found in destination database for Run Date: " + dsEstimate.est_estimate[0].rundate.ToShortDateString() + ".";
                    return null;
                }
            }

            if (!string.IsNullOrEmpty(_BlowInDesc))
            {
                BlowIn_ID = FindPrinterRate(conn, AssemblyVendor_ID.Value, _BlowInDesc, 2);
                if (BlowIn_ID == null)
                {
                    ErrorDescription = "Blow-In Rate for Assembly Vendor: " + _AssemblyVendorDesc + " not found in destination database for Run Date: " + dsEstimate.est_estimate[0].rundate.ToShortDateString() + ".";
                    return null;
                }
            }

            if (!string.IsNullOrEmpty(_OnsertDesc))
            {
                Onsert_ID = FindPrinterRate(conn, AssemblyVendor_ID.Value, _OnsertDesc, 9);
                if (Onsert_ID == null)
                {
                    ErrorDescription = "Onsert Rate for Assembly Vendor: " + _AssemblyVendorDesc + " not found in destination database for Run Date: " + dsEstimate.est_estimate[0].rundate.ToShortDateString() + ".";
                    return null;
                }
            }

            if (!string.IsNullOrEmpty(_StitcherMakereadyDesc))
            {
                StitcherMakeready_ID = FindPrinterRate(conn, Printer_ID.Value, _StitcherMakereadyDesc, 4);
                if (StitcherMakeready_ID == null)
                {
                    ErrorDescription = "Stitcher Makeready Rate for Printer Vendor: " + _PrinterDesc + " not found in destination database for Run Date: " + dsEstimate.est_estimate[0].rundate.ToShortDateString() + ".";
                    return null;
                }
            }

            if (!string.IsNullOrEmpty(_PressMakereadyDesc))
            {
                PressMakeready_ID = FindPrinterRate(conn, Printer_ID.Value, _PressMakereadyDesc, 10);
                if (PressMakeready_ID == null)
                {
                    ErrorDescription = "Press Makeready Rate for Printer Vendor: " + _PrinterDesc + " not found in destination database for Run Date: " + dsEstimate.est_estimate[0].rundate.ToShortDateString() + ".";
                    return null;
                }
            }

            if (!string.IsNullOrEmpty(_PaperDesc))
            {
                Paper_ID = FindPaper(conn, _PaperDesc, dsEstimate.est_estimate[0].rundate);
                if (Paper_ID == null)
                {
                    ErrorDescription = "Paper Rates for Vendor: " + _PaperDesc + " not found in destination database for Run Date: " + dsEstimate.est_estimate[0].rundate.ToShortDateString() + ".";
                    return null;
                }
            }

            PaperWeight_ID = FindPaperWeight(conn, _PaperWeight.Value);
            
            if (!string.IsNullOrEmpty(_PaperGrade))
            {
                PaperGrade_ID = FindPaperGrade(conn, _PaperGrade);
            }

            if (!string.IsNullOrEmpty(_PaperMapDesc))
            {
                if (PaperWeight_ID.HasValue && PaperGrade_ID.HasValue)
                {
                    PaperMap_ID = FindPaperMap(conn, Paper_ID.Value, _PaperMapDesc, PaperWeight_ID.Value, PaperGrade_ID.Value);
                    if (PaperMap_ID == null)
                    {
                        if (!FindPaperMap(conn, Paper_ID.Value, _PaperMapDesc, out PaperMap_ID, out PaperWeight_ID, out PaperGrade_ID))
                        {
                            ErrorDescription = "Paper Map for Vendor: " + _PaperDesc + " not found in destination database for Run Date: " + dsEstimate.est_estimate[0].rundate.ToShortDateString() + ".";
                            return null;
                        }
                    }
                }
                else
                {
                    if (!FindPaperMap(conn, Paper_ID.Value, _PaperMapDesc, out PaperMap_ID, out PaperWeight_ID, out PaperGrade_ID))
                    {
                        ErrorDescription = "Paper Map for Vendor: " + _PaperDesc + " not found in destination database for Run Date: " + dsEstimate.est_estimate[0].rundate.ToShortDateString() + ".";
                        return null;
                    }
                }
            }
            else
            {
                if (PaperWeight_ID == null)
                {
                    ErrorDescription = "Paper Weight: " + _PaperWeight + " not found in destination database.";
                    return null;
                }

                if (PaperGrade_ID == null && !string.IsNullOrEmpty(_PaperGrade))
                {
                    ErrorDescription = "Paper Grade: " + _PaperGrade + " not found in destination database.";
                    return null;
                }
            }

            // If all foreign keys have been found, a component row can be returned
            Estimates.est_componentRow c_row = dsEstimate.est_component.Newest_componentRow();
            c_row.est_estimate_id = dsEstimate.est_estimate[0].est_estimate_id;
            c_row.description = _Description;
            if (string.IsNullOrEmpty(_Comments))
                c_row.SetcommentsNull();
            else
                c_row.comments = _Comments;
            if (string.IsNullOrEmpty(_FinancialChangeComments))
                c_row.SetfinancialchangecommentNull();
            else
                c_row.financialchangecomment = _FinancialChangeComments;
            if (_AdNumber == null)
                c_row.SetadnumberNull();
            else
                c_row.adnumber = _AdNumber.Value;
            c_row.est_estimatemediatype_id = _EstimateMediaTypeID.Value;
            c_row.est_componenttype_id = _ComponentTypeID.Value;
            if (_MediaQtywoInsert == null)
                c_row.SetmediaqtywoinsertNull();
            else
                c_row.mediaqtywoinsert = _MediaQtywoInsert.Value;
            if (_SpoilagePct == null)
                c_row.SetspoilagepctNull();
            else
                c_row.spoilagepct = _SpoilagePct.Value;
            c_row.pagecount = _PageCount.Value;
            c_row.width = _Width.Value;
            c_row.height = _Height.Value;
            if (_OtherProduction == null)
                c_row.SetotherproductionNull();
            else
                c_row.otherproduction = _OtherProduction.Value;
            c_row.vendorsupplied = _VendorSupplied;
            if (VendorSupplied_ID == null)
                c_row.Setvendorsupplied_idNull();
            else
                c_row.vendorsupplied_id = VendorSupplied_ID.Value;
            if (_VendorSuppliedCPM == null)
                c_row.SetvendorcpmNull();
            else
                c_row.vendorcpm = _VendorSuppliedCPM.Value;
            if (CreativeVendor_ID.HasValue)
                c_row.creativevendor_id = CreativeVendor_ID.Value;
            else
                c_row.Setcreativevendor_idNull();
            if (_CreativeCPP == null)
                c_row.SetcreativecppNull();
            else
                c_row.creativecpp = _CreativeCPP.Value;
            if (Separator_ID.HasValue)
                c_row.separator_id = Separator_ID.Value;
            else
                c_row.Setseparator_idNull();
            if (_SeparatorCPP == null)
                c_row.SetseparatorcppNull();
            else
                c_row.separatorcpp = _SeparatorCPP.Value;
            if (Printer_ID.HasValue)
                c_row.printer_id = Printer_ID.Value;
            else
                c_row.Setprinter_idNull();
            if (AssemblyVendor_ID.HasValue)
                c_row.assemblyvendor_id = AssemblyVendor_ID.Value;
            else
                c_row.Setassemblyvendor_idNull();

            c_row.calculateprintcost = _CalcPrintCost;
            if (_ManualPrintCost == null)
                c_row.SetprintcostNull();
            else
                c_row.printcost = _ManualPrintCost.Value;
            if (_NumberofPlants == null)
                c_row.SetnumberofplantsNull();
            else
                c_row.numberofplants = _NumberofPlants.Value;
            if (_AdditionalPlates == null)
                c_row.SetadditionalplatesNull();
            else
                c_row.additionalplates = _AdditionalPlates.Value;
            if (PlateCost_ID == null)
                c_row.Setplatecost_idNull();
            else
                c_row.platecost_id = PlateCost_ID.Value;
            if (_ReplacementPlateCost == null)
                c_row.SetreplacementplatecostNull();
            else
                c_row.replacementplatecost = _ReplacementPlateCost.Value;
            if (_RunRate == null)
                c_row.SetrunrateNull();
            else
                c_row.runrate = _RunRate.Value;
            if (_NumberDigitalHandleandPrepare == null)
                c_row.SetnumberdigitalhandlenprepareNull();
            else
                c_row.numberdigitalhandlenprepare = _NumberDigitalHandleandPrepare.Value;
            if (DigitalHandleandPrepare_ID == null)
                c_row.Setdigitalhandlenprepare_idNull();
            else
                c_row.digitalhandlenprepare_id = DigitalHandleandPrepare_ID.Value;
            if (StitchIn_ID == null)
                c_row.Setstitchin_idNull();
            else
                c_row.stitchin_id = StitchIn_ID.Value;
            if (BlowIn_ID == null)
                c_row.Setblowin_idNull();
            else
                c_row.blowin_id = BlowIn_ID.Value;
            if (Onsert_ID == null)
                c_row.Setonsert_idNull();
            else
                c_row.onsert_id = Onsert_ID.Value;
            if (StitcherMakeready_ID == null)
                c_row.Setstitchermakeready_idNull();
            else
                c_row.stitchermakeready_id = StitcherMakeready_ID.Value;
            if (StitcherMakereadyRate == null)
                c_row.SetstitchermakereadyrateNull();
            else
                c_row.stitchermakereadyrate = StitcherMakereadyRate.Value;
            if (PressMakeready_ID == null)
                c_row.Setpressmakeready_idNull();
            else
                c_row.pressmakeready_id = PressMakeready_ID.Value;
            if (PressMakereadyRate == null)
                c_row.SetpressmakereadyrateNull();
            else
                c_row.pressmakereadyrate = PressMakereadyRate.Value;
            if (_EarlyPayPrintDiscount == null)
                c_row.SetearlypayprintdiscountNull();
            else
                c_row.earlypayprintdiscount = _EarlyPayPrintDiscount.Value;
            c_row.printerapplytax = _PrinterApplyTax;
            if (_PrinterTaxableMediaPct == null)
                c_row.SetprintertaxablemediapctNull();
            else
                c_row.printertaxablemediapct = _PrinterTaxableMediaPct.Value;
            if (_PrinterSalesTaxPct == null)
                c_row.SetprintersalestaxpctNull();
            else
                c_row.printersalestaxpct = _PrinterSalesTaxPct.Value;
            if (Paper_ID.HasValue)
                c_row.paper_id = Paper_ID.Value;
            else
                c_row.Setpaper_idNull();
            if (PaperMap_ID == null)
                c_row.Setpaper_map_idNull();
            else
                c_row.paper_map_id = PaperMap_ID.Value;
            c_row.paperweight_id = PaperWeight_ID.Value;
            if (PaperGrade_ID.HasValue)
                c_row.papergrade_id = PaperGrade_ID.Value;
            else
                c_row.Setpapergrade_idNull();
            c_row.calculatepapercost = _CalcPaperCost;
            if (_ManualPaperCost == null)
                c_row.SetpapercostNull();
            else
                c_row.papercost = _ManualPaperCost.Value;
            if (_RunPounds == null)
                c_row.SetrunpoundsNull();
            else
                c_row.runpounds = _RunPounds.Value;
            if (_MakereadyPounds == null)
                c_row.SetmakereadypoundsNull();
            else
                c_row.makereadypounds = _MakereadyPounds.Value;
            if (_PlateChangePounds == null)
                c_row.SetplatechangepoundsNull();
            else
                c_row.platechangepounds = _PlateChangePounds.Value;
            if (_PressStopPounds == null)
                c_row.SetpressstoppoundsNull();
            else
                c_row.pressstoppounds = _PressStopPounds.Value;
            if (_NumberofPressStops == null)
                c_row.SetnumberofpressstopsNull();
            else
                c_row.numberofpressstops = _NumberofPressStops.Value;
            if (_EarlyPayPaperDiscount == null)
                c_row.SetearlypaypaperdiscountNull();
            else
                c_row.earlypaypaperdiscount = _EarlyPayPaperDiscount.Value;
            c_row.paperapplytax = _PaperApplyTax;
            if (_PaperTaxableMediaPct == null)
                c_row.SetpapertaxablemediapctNull();
            else
                c_row.papertaxablemediapct = _PaperTaxableMediaPct.Value;
            if (_PaperSalesTaxPct == null)
                c_row.SetpapersalestaxpctNull();
            else
                c_row.papersalestaxpct = _PaperSalesTaxPct.Value;

            c_row.createdby = MainForm.AuthorizedUser.FormattedName;
            c_row.createddate = DateTime.Now;

            dsEstimate.est_component.Addest_componentRow(c_row);

            ErrorDescription = string.Empty;
            return c_row;
        }
        #endregion

        #region Private Methods
        private long? FindVendor(SqlConnection conn, string Description, int VendorTypeID)
        {
            long? retval = null;

            SqlCommand cmd = new SqlCommand("VndVendor_s_ByDescriptionandVendorType", conn);
            cmd.CommandTimeout = 7200;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@VND_VendorType_ID", VendorTypeID);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
                retval = dr.GetInt64(dr.GetOrdinal("VND_Vendor_ID"));

            dr.Close();
            return retval;
        }

        private long? FindPrinter(SqlConnection conn, string Description, DateTime RunDate)
        {
            long? retval = null;

            SqlCommand cmd = new SqlCommand("VndPrinter_s_ByDescriptionandRunDate", conn);
            cmd.CommandTimeout = 7200;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@RunDate", RunDate);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
                retval = dr.GetInt64(dr.GetOrdinal("VND_Printer_ID"));

            dr.Close();
            return retval;
        }

        private long? FindPaper(SqlConnection conn, string Description, DateTime RunDate)
        {
            long? retval = null;
            SqlCommand cmd = new SqlCommand("VndPaper_s_ByDescriptionandRunDate", conn);
            cmd.CommandTimeout = 7200;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@RunDate", RunDate);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
                retval = dr.GetInt64(dr.GetOrdinal("VND_Paper_ID"));
            dr.Close();
            return retval;
        }

        private long? FindPrinterRate(SqlConnection conn, long PrinterID, string Description, int PrinterRateTypeID)
        {
            long? retval = null;

            SqlCommand cmd = new SqlCommand("PrtPrinterRate_s_ByPrinterID_Description_PrinterRateTypeID", conn);
            cmd.CommandTimeout = 7200;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@VND_Printer_ID", PrinterID);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@PRT_PrinterRateType_ID", PrinterRateTypeID);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
                retval = dr.GetInt64(dr.GetOrdinal("PRT_PrinterRate_ID"));

            dr.Close();
            return retval;
        }

        private long? FindPaperMap(SqlConnection conn, long PaperID, string Description, int originalPaperWeight_ID, int originalPaperGrade_ID)
        {
            long? retval = null;

            SqlCommand cmd = new SqlCommand("PprPaperMap_s_ByPaperIDDescriptionWeightGrade", conn);
            cmd.CommandTimeout = 7200;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@VND_Paper_ID", PaperID);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@PPR_PaperWeight_ID", originalPaperWeight_ID);
            cmd.Parameters.AddWithValue("@PPR_PaperGrade_ID", originalPaperGrade_ID);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
                retval = dr.GetInt64(dr.GetOrdinal("PPR_Paper_Map_ID"));

            dr.Close();
            return retval;
        }

        private bool FindPaperMap(SqlConnection conn, long PaperID, string Description, out long? PaperMap_ID, out int? PaperWeight_ID, out int? PaperGrade_ID)
        {
            bool retval = false;

            SqlCommand cmd = new SqlCommand("PprPaperMap_s_ByPaperIDandDescription", conn);
            cmd.CommandTimeout = 7200;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@VND_Paper_ID", PaperID);
            cmd.Parameters.AddWithValue("@Description", Description);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                retval = true;
                PaperMap_ID = dr.GetInt64(dr.GetOrdinal("PPR_Paper_Map_ID"));
                PaperWeight_ID = dr.GetInt32(dr.GetOrdinal("PPR_PaperWeight_ID"));
                PaperGrade_ID = dr.GetInt32(dr.GetOrdinal("PPR_PaperGrade_ID"));
            }
            else
            {
                PaperMap_ID = null;
                PaperWeight_ID = null;
                PaperGrade_ID = null;
            }
            dr.Close();

            return retval;
        }

        private int? FindPaperWeight(SqlConnection conn, int Weight)
        {
            int? retval = null;

            SqlCommand cmd = new SqlCommand("PprPaperWeight_s_ByWeight", conn);
            cmd.CommandTimeout = 7200;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Weight", Weight);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                retval = dr.GetInt32(dr.GetOrdinal("PPR_PaperWeight_ID"));
            }
            dr.Close();

            return retval;
        }

        private int? FindPaperGrade(SqlConnection conn, string Grade)
        {
            int? retval = null;

            SqlCommand cmd = new SqlCommand("PprPaperGrade_s_ByGrade", conn);
            cmd.CommandTimeout = 7200;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Grade", Grade);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                retval = dr.GetInt32(dr.GetOrdinal("PPR_PaperGrade_ID"));
            }
            dr.Close();

            return retval;
        }
        #endregion

        #region Public Properties

        public long OriginalComponentID
        {
            get { return _originalComponentID; }
            set { _originalComponentID = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }

        public string FinancialChangeComments
        {
            get { return _FinancialChangeComments; }
            set { _FinancialChangeComments = value; }
        }

        public int? AdNumber
        {
            get { return _AdNumber; }
            set { _AdNumber = value; }
        }

        public int? EstimateMediaTypeID
        {
            get { return _EstimateMediaTypeID; }
            set { _EstimateMediaTypeID = value; }
        }

        public int? ComponentTypeID
        {
            get { return _ComponentTypeID; }
            set { _ComponentTypeID = value; }
        }

        public int? MediaQtywoInsert
        {
            get { return _MediaQtywoInsert; }
            set { _MediaQtywoInsert = value; }
        }

        public decimal? SpoilagePct
        {
            get { return _SpoilagePct; }
            set { _SpoilagePct = value; }
        }

        public int? PageCount
        {
            get { return _PageCount; }
            set { _PageCount = value; }
        }

        public decimal? Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        public decimal? Height
        {
            get { return _Height; }
            set { _Height = value; }
        }

        public bool VendorSupplied
        {
            get { return _VendorSupplied; }
            set { _VendorSupplied = value; }
        }

        public string VendorSuppliedDesc
        {
            get { return _VendorSuppliedDesc; }
            set { _VendorSuppliedDesc = value; }
        }

        public decimal? VendorSuppliedCPM
        {
            get { return _VendorSuppliedCPM; }
            set { _VendorSuppliedCPM = value; }
        }

        public string CreativeDesc
        {
            get { return _CreativeDesc; }
            set { _CreativeDesc = value; }
        }

        public decimal? CreativeCPP
        {
            get { return _CreativeCPP; }
            set { _CreativeCPP = value; }
        }

        public string SeparatorDesc
        {
            get { return _SeparatorDesc; }
            set { _SeparatorDesc = value; }
        }

        public decimal? SeparatorCPP
        {
            get { return _SeparatorCPP; }
            set { _SeparatorCPP = value; }
        }

        public string PrinterDesc
        {
            get { return _PrinterDesc; }
            set { _PrinterDesc = value; }
        }

        public string AssemblyVendorDesc
        {
            get { return _AssemblyVendorDesc; }
            set { _AssemblyVendorDesc = value; }
        }

        public bool CalcPrinterCost
        {
            get { return _CalcPrintCost; }
            set { _CalcPrintCost = value; }
        }

        public decimal? ManualPrinterCost
        {
            get { return _ManualPrintCost; }
            set { _ManualPrintCost = value; }
        }

        public int? NumberofPlants
        {
            get { return _NumberofPlants; }
            set { _NumberofPlants = value; }
        }

        public int? AdditionalPlates
        {
            get { return _AdditionalPlates; }
            set { _AdditionalPlates = value; }
        }

        public string PlateCostDesc
        {
            get { return _PlateCostDesc; }
            set { _PlateCostDesc = value; }
        }

        public decimal? ReplacementPlateCost
        {
            get { return _ReplacementPlateCost; }
            set { _ReplacementPlateCost = value; }
        }

        public decimal? RunRate
        {
            get { return _RunRate; }
            set { _RunRate = value; }
        }

        public int? NumberDigitalHandleandPrepare
        {
            get { return _NumberDigitalHandleandPrepare; }
            set { _NumberDigitalHandleandPrepare = value; }
        }

        public string DigitalHandleandPrepareDesc
        {
            get { return _DigitalHandleandPrepareDesc; }
            set { _DigitalHandleandPrepareDesc = value; }
        }

        public string StitchInDesc
        {
            get { return _StitchInDesc; }
            set { _StitchInDesc = value; }
        }

        public string BlowInDesc
        {
            get { return _BlowInDesc; }
            set { _BlowInDesc = value; }
        }

        public string OnsertDesc
        {
            get { return _OnsertDesc; }
            set { _OnsertDesc = value; }
        }

        public string StitcherMakereadyDesc
        {
            get { return _StitcherMakereadyDesc; }
            set { _StitcherMakereadyDesc = value; }
        }

        public decimal? StitcherMakereadyRate
        {
            get { return _StitcherMakereadyRate; }
            set { _StitcherMakereadyRate = value; }
        }

        public string PressMakereadyDesc
        {
            get { return _PressMakereadyDesc; }
            set { _PressMakereadyDesc = value; }
        }

        public decimal? PressMakereadyRate
        {
            get { return _PressMakereadyRate; }
            set { _PressMakereadyRate = value; }
        }

        public decimal? EarlyPayPrintDiscount
        {
            get { return _EarlyPayPrintDiscount; }
            set { _EarlyPayPrintDiscount = value; }
        }

        public bool PrinterApplyTax
        {
            get { return _PrinterApplyTax; }
            set { _PrinterApplyTax = value; }
        }

        public decimal? PrinterTaxableMediaPct
        {
            get { return _PrinterTaxableMediaPct; }
            set { _PrinterTaxableMediaPct = value; }
        }

        public decimal? PrinterSalesTaxPct
        {
            get { return _PrinterSalesTaxPct; }
            set { _PrinterSalesTaxPct = value; }
        }

        public string PaperDesc
        {
            get { return _PaperDesc; }
            set { _PaperDesc = value; }
        }

        public string PaperMapDesc
        {
            get { return _PaperMapDesc; }
            set { _PaperMapDesc = value; }
        }

        public int? PaperWeight
        {
            get { return _PaperWeight; }
            set { _PaperWeight = value; }
        }

        public string PaperGrade
        {
            get { return _PaperGrade; }
            set { _PaperGrade = value; }
        }

        public bool CalcPaperCost
        {
            get { return _CalcPaperCost; }
            set { _CalcPaperCost = value; }
        }

        public decimal? ManualPaperCost
        {
            get { return _ManualPaperCost; }
            set { _ManualPaperCost = value; }
        }

        public decimal? RunPounds
        {
            get { return _RunPounds; }
            set { _RunPounds = value; }
        }

        public int? MakereadyPounds
        {
            get { return _MakereadyPounds; }
            set { _MakereadyPounds = value; }
        }

        public decimal? PlateChangePounds
        {
            get { return _PlateChangePounds; }
            set { _PlateChangePounds = value; }
        }

        public int? PressStopPounds
        {
            get { return _PressStopPounds; }
            set { _PressStopPounds = value; }
        }

        public int? NumberofPressStops
        {
            get { return _NumberofPressStops; }
            set { _NumberofPressStops = value; }
        }

        public decimal? EarlyPayPaperDiscount
        {
            get { return _EarlyPayPaperDiscount; }
            set { _EarlyPayPaperDiscount = value; }
        }

        public bool PaperApplyTax
        {
            get { return _PaperApplyTax; }
            set { _PaperApplyTax = value; }
        }

        public decimal? PaperTaxableMediaPct
        {
            get { return _PaperTaxableMediaPct; }
            set { _PaperTaxableMediaPct = value; }
        }

        public decimal? PaperSalesTaxPct
        {
            get { return _PaperSalesTaxPct; }
            set { _PaperSalesTaxPct = value; }
        }

        public decimal? OtherProduction
        {
            get { return _OtherProduction; }
            set { _OtherProduction = value; }
        }
        #endregion

    }
}
