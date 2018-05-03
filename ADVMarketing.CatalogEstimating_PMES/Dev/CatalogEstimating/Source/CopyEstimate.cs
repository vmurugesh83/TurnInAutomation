using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.EstimatesTableAdapters;

namespace CatalogEstimating
{
    public class CopyEstimate
    {
        #region Private Members
        private long _originalEstimateID;
        private string _description;
        private string _comments;
        private DateTime _runDate;
        private int _est_status_id;
        private long? _parent_id;
        private DateTime? _uploaddate;

        private int? _SampleQuantity;
        private decimal? _SampleFreightCWT;
        private decimal? _SampleFreightFlat;

        private List<CopyComponent> _components = new List<CopyComponent>();
        private CopyAssemDistrib _adoptions = null;
        private List<CopyPackage> _packages = new List<CopyPackage>();
        private List<CopyPackageComponentMap> _pc_maps = new List<CopyPackageComponentMap>();
        private List<CopyPubRateMap> _pubratemaps = new List<CopyPubRateMap>();
        private List<CopyPubIssueDate> _pubissuedates = new List<CopyPubIssueDate>();

        private bool _bClearIssueDateOverrides = false;
        #endregion

        #region Construction
        public CopyEstimate()
        {
        }
        #endregion

        #region Public Methods
        public void LoadData(long originalEstimateID)
        {
            _originalEstimateID = originalEstimateID;

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("EstEstimate_s_ByEstimateID", conn);
                cmd.CommandTimeout = 7200;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EST_Estimate_ID", _originalEstimateID);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    _description = dr["Description"].ToString();
                    _comments = dr["Comments"].ToString();
                    _runDate = dr.GetDateTime(dr.GetOrdinal("RunDate"));
                    if (dr.IsDBNull(dr.GetOrdinal("Parent_ID")))
                        _parent_id = null;
                    else
                        _parent_id = dr.GetInt64(dr.GetOrdinal("Parent_ID"));
                }
                dr.Close();

                SqlCommand s_cmd = new SqlCommand("EstSamples_s_ByEstimateID", conn);
                s_cmd.CommandTimeout = 7200;
                s_cmd.CommandType = CommandType.StoredProcedure;
                s_cmd.Parameters.AddWithValue("@EST_Estimate_ID", _originalEstimateID);
                SqlDataReader s_dr = s_cmd.ExecuteReader();
                if (s_dr.Read())
                {
                    _SampleQuantity = s_dr.GetInt32(s_dr.GetOrdinal("Quantity"));
                    _SampleFreightCWT = s_dr.GetDecimal(s_dr.GetOrdinal("FreightCWT"));
                    _SampleFreightFlat = s_dr.GetDecimal(s_dr.GetOrdinal("FreightFlat"));
                }
                s_dr.Close();

                conn.Close();
            }
        }

        public void LoadDetails()
        {
            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                #region Gather Component Data

                SqlCommand comp_cmd = new SqlCommand("EstComponent_s_ForEstimateCopy", conn);
                comp_cmd.CommandTimeout = 7200;
                comp_cmd.CommandType = CommandType.StoredProcedure;
                comp_cmd.Parameters.AddWithValue("@EST_Estimate_ID", _originalEstimateID);
                SqlDataReader comp_dr = comp_cmd.ExecuteReader();
                while (comp_dr.Read())
                {
                    CopyComponent curComponent = new CopyComponent();
                    curComponent.OriginalComponentID = comp_dr.GetInt64(comp_dr.GetOrdinal("EST_Component_ID"));
                    curComponent.Description = comp_dr["Description"].ToString();
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("Comments")))
                        curComponent.Comments = null;
                    else
                        curComponent.Comments = comp_dr["Comments"].ToString();
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("FinancialChangeComment")))
                        curComponent.FinancialChangeComments = null;
                    else
                        curComponent.FinancialChangeComments = comp_dr["FinancialChangeComment"].ToString();
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("AdNumber")))
                        curComponent.AdNumber = null;
                    else
                        curComponent.AdNumber = comp_dr.GetInt32(comp_dr.GetOrdinal("AdNumber"));
                    curComponent.EstimateMediaTypeID = comp_dr.GetInt32(comp_dr.GetOrdinal("EST_EstimateMediaType_ID"));
                    curComponent.ComponentTypeID = comp_dr.GetInt32(comp_dr.GetOrdinal("EST_ComponentType_ID"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("MediaQtywoInsert")))
                        curComponent.MediaQtywoInsert = null;
                    else
                        curComponent.MediaQtywoInsert = comp_dr.GetInt32(comp_dr.GetOrdinal("MediaQtywoInsert"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("SpoilagePct")))
                        curComponent.SpoilagePct = null;
                    else
                        curComponent.SpoilagePct = comp_dr.GetDecimal(comp_dr.GetOrdinal("SpoilagePct"));
                    curComponent.PageCount = comp_dr.GetInt32(comp_dr.GetOrdinal("PageCount"));
                    curComponent.Width = comp_dr.GetDecimal(comp_dr.GetOrdinal("Width"));
                    curComponent.Height = comp_dr.GetDecimal(comp_dr.GetOrdinal("Height"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("OtherProduction")))
                        curComponent.OtherProduction = null;
                    else
                        curComponent.OtherProduction = comp_dr.GetDecimal(comp_dr.GetOrdinal("OtherProduction"));
                    curComponent.VendorSupplied = comp_dr.GetBoolean(comp_dr.GetOrdinal("VendorSupplied"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("VendorSuppliedDesc")))
                        curComponent.VendorSuppliedDesc = null;
                    else
                        curComponent.VendorSuppliedDesc = comp_dr["VendorSuppliedDesc"].ToString();
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("VendorCPM")))
                        curComponent.VendorSuppliedCPM = null;
                    else
                        curComponent.VendorSuppliedCPM = comp_dr.GetDecimal(comp_dr.GetOrdinal("VendorCPM"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("CreativeDesc")))
                        curComponent.CreativeDesc = null;
                    else
                        curComponent.CreativeDesc = comp_dr["CreativeDesc"].ToString();
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("CreativeCPP")))
                        curComponent.CreativeCPP = null;
                    else
                        curComponent.CreativeCPP = comp_dr.GetDecimal(comp_dr.GetOrdinal("CreativeCPP"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("SeparatorDesc")))
                        curComponent.SeparatorDesc = null;
                    else
                        curComponent.SeparatorDesc = comp_dr["SeparatorDesc"].ToString();
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("SeparatorCPP")))
                        curComponent.SeparatorCPP = null;
                    else
                        curComponent.SeparatorCPP = comp_dr.GetDecimal(comp_dr.GetOrdinal("SeparatorCPP"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("PrinterDesc")))
                        curComponent.PrinterDesc = null;
                    else
                        curComponent.PrinterDesc = comp_dr["PrinterDesc"].ToString();
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("AssemblyVendorDesc")))
                        curComponent.AssemblyVendorDesc = null;
                    else
                        curComponent.AssemblyVendorDesc = comp_dr["AssemblyVendorDesc"].ToString();
                    curComponent.CalcPrinterCost = comp_dr.GetBoolean(comp_dr.GetOrdinal("CalculatePrintCost"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("PrintCost")))
                        curComponent.ManualPrinterCost = null;
                    else
                        curComponent.ManualPrinterCost = comp_dr.GetDecimal(comp_dr.GetOrdinal("PrintCost"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("NumberofPlants")))
                        curComponent.NumberofPlants = null;
                    else
                        curComponent.NumberofPlants = comp_dr.GetInt32(comp_dr.GetOrdinal("NumberofPlants"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("AdditionalPlates")))
                        curComponent.AdditionalPlates = null;
                    else
                        curComponent.AdditionalPlates = comp_dr.GetInt32(comp_dr.GetOrdinal("AdditionalPlates"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("PlateCostDesc")))
                        curComponent.PlateCostDesc = null;
                    else
                        curComponent.PlateCostDesc = comp_dr["PlateCostDesc"].ToString();
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("ReplacementPlateCost")))
                        curComponent.ReplacementPlateCost = null;
                    else
                        curComponent.ReplacementPlateCost = comp_dr.GetDecimal(comp_dr.GetOrdinal("ReplacementPlateCost"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("RunRate")))
                        curComponent.RunRate = null;
                    else
                        curComponent.RunRate = comp_dr.GetDecimal(comp_dr.GetOrdinal("RunRate"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("NumberDigitalHandlenPrepare")))
                        curComponent.NumberDigitalHandleandPrepare = null;
                    else
                        curComponent.NumberDigitalHandleandPrepare = comp_dr.GetInt32(comp_dr.GetOrdinal("NumberDigitalHandlenPrepare"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("StitchInDesc")))
                        curComponent.StitchInDesc = null;
                    else
                        curComponent.StitchInDesc = comp_dr["StitchInDesc"].ToString();
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("BlowInDesc")))
                        curComponent.BlowInDesc = null;
                    else
                        curComponent.BlowInDesc = comp_dr["BlowInDesc"].ToString();
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("OnsertDesc")))
                        curComponent.OnsertDesc = null;
                    else
                        curComponent.OnsertDesc = comp_dr["OnsertDesc"].ToString();
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("StitcherMakereadyDesc")))
                        curComponent.StitcherMakereadyDesc = null;
                    else
                        curComponent.StitcherMakereadyDesc = comp_dr["StitcherMakereadyDesc"].ToString();
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("StitcherMakereadyRate")))
                        curComponent.StitcherMakereadyRate = null;
                    else
                        curComponent.StitcherMakereadyRate = comp_dr.GetDecimal(comp_dr.GetOrdinal("StitcherMakereadyRate"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("PressMakereadyDesc")))
                        curComponent.PressMakereadyDesc = null;
                    else
                        curComponent.PressMakereadyDesc = comp_dr["PressMakereadyDesc"].ToString();
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("PressMakereadyRate")))
                        curComponent.PressMakereadyRate = null;
                    else
                        curComponent.PressMakereadyRate = comp_dr.GetDecimal(comp_dr.GetOrdinal("PressMakereadyRate"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("DigitalHandlenPrepareDesc")))
                        curComponent.DigitalHandleandPrepareDesc = null;
                    else
                        curComponent.DigitalHandleandPrepareDesc = comp_dr["DigitalHandlenPrepareDesc"].ToString();
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("EarlyPayPrintDiscount")))
                        curComponent.EarlyPayPrintDiscount = null;
                    else
                        curComponent.EarlyPayPrintDiscount = comp_dr.GetDecimal(comp_dr.GetOrdinal("EarlyPayPrintDiscount"));
                    curComponent.PrinterApplyTax = comp_dr.GetBoolean(comp_dr.GetOrdinal("PrinterApplyTax"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("PrinterTaxableMediaPct")))
                        curComponent.PrinterTaxableMediaPct = null;
                    else
                        curComponent.PrinterTaxableMediaPct = comp_dr.GetDecimal(comp_dr.GetOrdinal("PrinterTaxableMediaPct"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("PrinterSalesTaxPct")))
                        curComponent.PrinterSalesTaxPct = null;
                    else
                        curComponent.PrinterSalesTaxPct = comp_dr.GetDecimal(comp_dr.GetOrdinal("PrinterSalesTaxPct"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("PaperDesc")))
                        curComponent.PaperDesc = null;
                    else
                        curComponent.PaperDesc = comp_dr["PaperDesc"].ToString();
                    curComponent.PaperMapDesc = comp_dr["PaperMapDesc"].ToString();
                    curComponent.PaperWeight = comp_dr.GetInt32(comp_dr.GetOrdinal("PaperWeight"));
                    curComponent.PaperGrade = comp_dr["PaperGrade"].ToString();
                    curComponent.CalcPaperCost = comp_dr.GetBoolean(comp_dr.GetOrdinal("CalculatePaperCost"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("PaperCost")))
                        curComponent.ManualPaperCost = null;
                    else
                        curComponent.ManualPaperCost = comp_dr.GetDecimal(comp_dr.GetOrdinal("PaperCost"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("RunPounds")))
                        curComponent.RunPounds = null;
                    else
                        curComponent.RunPounds = comp_dr.GetDecimal(comp_dr.GetOrdinal("RunPounds"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("MakereadyPounds")))
                        curComponent.MakereadyPounds = null;
                    else
                        curComponent.MakereadyPounds = comp_dr.GetInt32(comp_dr.GetOrdinal("MakereadyPounds"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("PlateChangePounds")))
                        curComponent.PlateChangePounds = null;
                    else
                        curComponent.PlateChangePounds = comp_dr.GetDecimal(comp_dr.GetOrdinal("PlateChangePounds"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("PressStopPounds")))
                        curComponent.PressStopPounds = null;
                    else
                        curComponent.PressStopPounds = comp_dr.GetInt32(comp_dr.GetOrdinal("PressStopPounds"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("NumberofPressStops")))
                        curComponent.NumberofPressStops = null;
                    else
                        curComponent.NumberofPressStops = comp_dr.GetInt32(comp_dr.GetOrdinal("NumberofPressStops"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("EarlyPayPaperDiscount")))
                        curComponent.EarlyPayPaperDiscount = null;
                    else
                        curComponent.EarlyPayPaperDiscount = comp_dr.GetDecimal(comp_dr.GetOrdinal("EarlyPayPaperDiscount"));
                    curComponent.PaperApplyTax = comp_dr.GetBoolean(comp_dr.GetOrdinal("PaperApplyTax"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("PaperTaxableMediaPct")))
                        curComponent.PaperTaxableMediaPct = null;
                    else
                        curComponent.PaperTaxableMediaPct = comp_dr.GetDecimal(comp_dr.GetOrdinal("PaperTaxableMediaPct"));
                    if (comp_dr.IsDBNull(comp_dr.GetOrdinal("PaperSalesTaxPct")))
                        curComponent.PaperSalesTaxPct = null;
                    else
                        curComponent.PaperSalesTaxPct = comp_dr.GetDecimal(comp_dr.GetOrdinal("PaperSalesTaxPct"));

                    _components.Add(curComponent);
                }
                comp_dr.Close();
                #endregion

                #region Gather Assembly and Distribution Data
                SqlCommand ad_cmd = new SqlCommand("EstAssemDistribOptions_s_ForEstimateCopy", conn);
                ad_cmd.CommandTimeout = 7200;
                ad_cmd.CommandType = CommandType.StoredProcedure;
                ad_cmd.Parameters.AddWithValue("@EST_Estimate_ID", _originalEstimateID);
                SqlDataReader ad_dr = ad_cmd.ExecuteReader();
                if (ad_dr.Read())
                {
                    _adoptions = new CopyAssemDistrib();
                    _adoptions.InsertDOW = ad_dr.GetInt32(ad_dr.GetOrdinal("InsertDOW"));
                    _adoptions.InsertFreightDesc = ad_dr["InsertFreightDesc"].ToString();
                    _adoptions.InsertFreightCWT = ad_dr.GetDecimal(ad_dr.GetOrdinal("InsertFreightCWT"));
                    _adoptions.InsertFuelSurcharge = ad_dr.GetDecimal(ad_dr.GetOrdinal("InsertFuelSurcharge"));
                    _adoptions.CornerGuards = ad_dr.GetBoolean(ad_dr.GetOrdinal("CornerGuards"));
                    _adoptions.Skids = ad_dr.GetBoolean(ad_dr.GetOrdinal("Skids"));
                    _adoptions.InsertTime = ad_dr.GetBoolean(ad_dr.GetOrdinal("InsertTime"));
                    _adoptions.PostalScenarioDesc = ad_dr["PostalScenarioDesc"].ToString();
                    _adoptions.MailFuelSurcharge = ad_dr.GetDecimal(ad_dr.GetOrdinal("MailFuelSurcharge"));
                    _adoptions.MailHouseDesc = ad_dr["MailHouseDesc"].ToString();
                    _adoptions.MailHouseOtherHandling = ad_dr.GetDecimal(ad_dr.GetOrdinal("MailHouseOtherHandling"));
                    _adoptions.UseMailTracking = ad_dr.GetBoolean(ad_dr.GetOrdinal("UseMailTracking"));
                    if (ad_dr.IsDBNull(ad_dr.GetOrdinal("MailTrackingDesc")))
                        _adoptions.MailTrackingDesc = null;
                    else
                        _adoptions.MailTrackingDesc = ad_dr["MailTrackingDesc"].ToString();
                    if (ad_dr.IsDBNull(ad_dr.GetOrdinal("MailListDesc")))
                        _adoptions.MailListDesc = null;
                    else
                        _adoptions.MailListDesc = ad_dr["MailListDesc"].ToString();
                    _adoptions.UseExternalMailList = ad_dr.GetBoolean(ad_dr.GetOrdinal("UseExternalMailList"));
                    _adoptions.ExternalMailQty = ad_dr.GetInt32(ad_dr.GetOrdinal("ExternalMailQty"));
                    _adoptions.ExternalMailCPM = ad_dr.GetDecimal(ad_dr.GetOrdinal("ExternalMailCPM"));
                    _adoptions.NbrOfCartons = ad_dr.GetInt32(ad_dr.GetOrdinal("NbrOfCartons"));
                    _adoptions.UseGlueTack = ad_dr.GetBoolean(ad_dr.GetOrdinal("UseGlueTack"));
                    _adoptions.UseTabbing = ad_dr.GetBoolean(ad_dr.GetOrdinal("UseTabbing"));
                    _adoptions.UseLetterInsertion = ad_dr.GetBoolean(ad_dr.GetOrdinal("UseLetterInsertion"));
                    _adoptions.FirstClass = ad_dr.GetBoolean(ad_dr.GetOrdinal("FirstClass"));
                    _adoptions.OtherFreight = ad_dr.GetDecimal(ad_dr.GetOrdinal("OtherFreight"));
                    if (ad_dr.IsDBNull(ad_dr.GetOrdinal("PostalDropFlat")))
                        _adoptions.PostalDropFlat = null;
                    else
                        _adoptions.PostalDropFlat = ad_dr.GetDecimal(ad_dr.GetOrdinal("PostalDropFlat"));
                }
                else
                    _adoptions = null;
                ad_dr.Close();
                #endregion

                #region Gather Packages

                SqlCommand p_cmd = new SqlCommand("EstPackage_s_ForEstimateCopy", conn);
                p_cmd.CommandTimeout = 7200;
                p_cmd.CommandType = CommandType.StoredProcedure;
                p_cmd.Parameters.AddWithValue("@EST_Estimate_ID", _originalEstimateID);
                SqlDataReader p_dr = p_cmd.ExecuteReader();
                while (p_dr.Read())
                {
                    CopyPackage curPackage = new CopyPackage();
                    curPackage.OriginalPackageID = p_dr.GetInt64(p_dr.GetOrdinal("EST_Package_ID"));
                    curPackage.Description = p_dr["Description"].ToString();
                    if (p_dr.IsDBNull(p_dr.GetOrdinal("Comments")))
                        curPackage.Comments = null;
                    else
                        curPackage.Comments = p_dr["Comments"].ToString();
                    curPackage.SoloQuantity = p_dr.GetInt32(p_dr.GetOrdinal("SoloQuantity"));
                    curPackage.OtherQuantity = p_dr.GetInt32(p_dr.GetOrdinal("OtherQuantity"));
                    if (p_dr.IsDBNull(p_dr.GetOrdinal("PUB_PubQuantityType_ID")))
                        curPackage.PubQuantityTypeID = null;
                    else
                        curPackage.PubQuantityTypeID = p_dr.GetInt32(p_dr.GetOrdinal("PUB_PubQuantityType_ID"));
                    if (p_dr.IsDBNull(p_dr.GetOrdinal("PUB_PubGroup_ID")))
                        curPackage.PubGroupID = null;
                    else
                        curPackage.PubGroupID = p_dr.GetInt64(p_dr.GetOrdinal("PUB_PubGroup_ID"));

                    curPackage.LoadData();

                    _packages.Add(curPackage);
                }
                p_dr.Close();

                #endregion

                #region Gather Package-Component Mappings
                SqlCommand pcm_cmd = new SqlCommand("EstPackageComponentMapping_s_ByEstimateID", conn);
                pcm_cmd.CommandTimeout = 7200;
                pcm_cmd.CommandType = CommandType.StoredProcedure;
                pcm_cmd.Parameters.AddWithValue("@EST_Estimate_ID", _originalEstimateID);
                SqlDataReader pcm_dr = pcm_cmd.ExecuteReader();
                while (pcm_dr.Read())
                {
                    _pc_maps.Add(new CopyPackageComponentMap(pcm_dr.GetInt64(pcm_dr.GetOrdinal("EST_Package_ID")), pcm_dr.GetInt64(pcm_dr.GetOrdinal("EST_Component_ID"))));
                }
                pcm_dr.Close();
                #endregion

                #region Gather Estimate Pub Issue Dates
                SqlCommand pid_cmd = new SqlCommand("EstPubIssueDates_s_Overrides_ByEstimateID", conn);
                pid_cmd.CommandTimeout = 7200;
                pid_cmd.CommandType = CommandType.StoredProcedure;
                pid_cmd.Parameters.AddWithValue("@EST_Estimate_ID", _originalEstimateID);
                SqlDataReader pid_dr = pid_cmd.ExecuteReader();
                while (pid_dr.Read())
                {
                    _pubissuedates.Add(new CopyPubIssueDate(pid_dr.GetBoolean(pid_dr.GetOrdinal("Override")), pid_dr.GetInt32(pid_dr.GetOrdinal("IssueDOW")), pid_dr.GetDateTime(pid_dr.GetOrdinal("IssueDate")), pid_dr["Pub_ID"].ToString(), pid_dr.GetInt32(pid_dr.GetOrdinal("PubLoc_ID"))));
                }
                pid_dr.Close();

                #endregion

                conn.Close();
            }
        }

        /// <summary>
        /// Attempts to Save the Estimate to the database specified by conn
        /// </summary>
        /// <param name="DestinationConnectionString">Destination Database</param>
        public bool SaveData(CESDatabase DestinationDB, out string ErrorDescription)
        {
            using (SqlConnection conn = (SqlConnection) DestinationDB.Database.CreateConnection())
            {
                conn.Open();
                Estimates dsEstimate = new Estimates();
                dsEstimate.EnforceConstraints = false;

                Estimates.est_estimateRow est_row = dsEstimate.est_estimate.Newest_estimateRow();

                est_row.description = _description;
                if (string.IsNullOrEmpty(_comments))
                    est_row.SetcommentsNull();
                else
                    est_row.comments = _comments;
                est_row.rundate = _runDate;
                est_row.fiscalmonth = FiscalCalculator.FiscalMonth(_runDate);
                est_row.fiscalyear = FiscalCalculator.FiscalYear(_runDate);
                est_row.est_season_id = FiscalCalculator.SeasonID(_runDate);
                est_row.est_status_id = _est_status_id;
                if (_parent_id == null)
                    est_row.Setparent_idNull();
                else
                    est_row.parent_id = _parent_id.Value;
                if (_uploaddate == null)
                    est_row.SetuploaddateNull();
                else
                    est_row.uploaddate = _uploaddate.Value;
                est_row.createdby = MainForm.AuthorizedUser.FormattedName;
                est_row.createddate = DateTime.Now;

                dsEstimate.est_estimate.Addest_estimateRow(est_row);

                if (_SampleQuantity.HasValue)
                {
                    Estimates.est_samplesRow s_row = dsEstimate.est_samples.Newest_samplesRow();
                    s_row.est_estimate_id = est_row.est_estimate_id;
                    s_row.quantity = _SampleQuantity.Value;
                    s_row.freightcwt = _SampleFreightCWT.Value;
                    s_row.freightflat = _SampleFreightFlat.Value;
                    s_row.createdby = MainForm.AuthorizedUser.FormattedName;
                    s_row.createddate = DateTime.Now;
                    dsEstimate.est_samples.Addest_samplesRow(s_row);
                }

                Dictionary<long, long> old_new_ComponentIDs = new Dictionary<long, long>();
                foreach (CopyComponent comp in _components)
                {
                    Estimates.est_componentRow c_row = comp.CreateDatasetRecord(conn, dsEstimate, out ErrorDescription);
                    if (c_row == null)
                        return false;

                    old_new_ComponentIDs.Add(comp.OriginalComponentID, c_row.est_component_id);
                }

                if (_adoptions != null && !_adoptions.CreateDatasetRecord(conn, dsEstimate, out ErrorDescription))
                        return false;

                Dictionary<long, long> old_new_PackageIDs = new Dictionary<long, long>();
                // First handle packages that do not reference a pub group
                foreach (CopyPackage package in _packages)
                {
                    if (package.PubGroupID == null)
                    {
                        Estimates.est_packageRow p_row = package.CreatePackageRecord_NoPubGroup(conn, dsEstimate, out ErrorDescription);
                        if (p_row == null)
                            return false;
                        old_new_PackageIDs.Add(package.OriginalPackageID, p_row.est_package_id);
                    }
                }

                // Next handle packages that include custom pub groups
                foreach (CopyPackage package in _packages)
                {
                    if (package.PubGroupID.HasValue && package.PubGroup.CustomPubGroupFlag)
                    {
                        Estimates.est_packageRow p_row = package.CreatePackageRecord_CustomPubGroup(conn, dsEstimate, _pubratemaps, out ErrorDescription);
                        if (p_row == null)
                            return false;
                        old_new_PackageIDs.Add(package.OriginalPackageID, p_row.est_package_id);
                    }
                }

                // Finally handle packages that include standard pub groups
                foreach (CopyPackage package in _packages)
                {
                    if (package.PubGroupID.HasValue && !package.PubGroup.CustomPubGroupFlag)
                    {
                        Estimates.est_packageRow p_row = package.CreatePackageRecord_StandardPubGroup(conn, dsEstimate, _pubratemaps, out ErrorDescription);
                        if (p_row == null)
                            return false;
                        old_new_PackageIDs.Add(package.OriginalPackageID, p_row.est_package_id);
                    }
                }

                foreach (CopyPackageComponentMap pcm in _pc_maps)
                {
                    Estimates.est_packagecomponentmappingRow pcm_row = dsEstimate.est_packagecomponentmapping.Newest_packagecomponentmappingRow();
                    pcm_row.est_package_id = old_new_PackageIDs[pcm.OriginalPackageID];
                    pcm_row.est_component_id = old_new_ComponentIDs[pcm.OriginalComponentID];
                    pcm_row.createdby = MainForm.AuthorizedUser.FormattedName;
                    pcm_row.createddate = DateTime.Now;
                    dsEstimate.est_packagecomponentmapping.Addest_packagecomponentmappingRow(pcm_row);
                }

                

                // Write the information to the Database
                using (est_estimateTableAdapter adapter = new est_estimateTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Update(dsEstimate.est_estimate);
                }

                using (est_samplesTableAdapter adapter = new est_samplesTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Update(dsEstimate.est_samples);
                }

                using (est_assemdistriboptionsTableAdapter adapter = new est_assemdistriboptionsTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Update(dsEstimate.est_assemdistriboptions);
                }

                using (est_componentTableAdapter adapter = new est_componentTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Update(dsEstimate.est_component);
                }

                using (pub_pubgroupTableAdapter adapter = new pub_pubgroupTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Update(dsEstimate.pub_pubgroup);
                }

                using (pub_pubpubgroup_mapTableAdapter adapter = new pub_pubpubgroup_mapTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Update(dsEstimate.pub_pubpubgroup_map);
                }

                using (est_packageTableAdapter adapter = new est_packageTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Update(dsEstimate.est_package);
                }

                using (est_packagecomponentmappingTableAdapter adapter = new est_packagecomponentmappingTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Update(dsEstimate.est_packagecomponentmapping);
                }

                // If the Run Date is the same as the original, and the source db = dest db copy the Issue Date Overrides
                if (!_bClearIssueDateOverrides)
                {
                    // Create all override EST_PubInsertDates records
                    foreach (CopyPubIssueDate pid in _pubissuedates)
                    {
                        SqlCommand pid_cmd = new SqlCommand("EstPubIssueDates_i_Override", conn);
                        pid_cmd.CommandTimeout = 7200;
                        pid_cmd.CommandType = CommandType.StoredProcedure;
                        pid_cmd.Parameters.AddWithValue("@Override", true);
                        pid_cmd.Parameters.AddWithValue("@IssueDOW", pid.IssueDOW);
                        pid_cmd.Parameters.AddWithValue("@IssueDate", pid.IssueDate);
                        pid_cmd.Parameters.AddWithValue("@EST_Estimate_ID", dsEstimate.est_estimate[0].est_estimate_id);
                        pid_cmd.Parameters.AddWithValue("@Pub_ID", pid.PubID);
                        pid_cmd.Parameters.AddWithValue("@PubLoc_ID", pid.PubLocID);
                        pid_cmd.Parameters.AddWithValue("@CreatedBy", MainForm.AuthorizedUser.FormattedName);
                        pid_cmd.ExecuteNonQuery();
                    }
                }

                // Create all standard EST_PubIssueDates records
                SqlCommand stdpid_cmd = new SqlCommand("EstPubIssueDates_i_ByEstimateID", conn);
                stdpid_cmd.CommandTimeout = 7200;
                stdpid_cmd.CommandType = CommandType.StoredProcedure;
                stdpid_cmd.Parameters.AddWithValue("@EST_Estimate_ID", dsEstimate.est_estimate[0].est_estimate_id);
                stdpid_cmd.Parameters.AddWithValue("@CreatedBy", MainForm.AuthorizedUser.FormattedName);
                stdpid_cmd.ExecuteNonQuery();

                conn.Close();
            }

            ErrorDescription = string.Empty;
            return true;
        }
        #endregion

        #region Public Properties

        public long OriginalEstimateID
        {
            get { return _originalEstimateID; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        public DateTime RunDate
        {
            get { return _runDate; }
            set { _runDate = value; }
        }

        public int StatusID
        {
            get { return _est_status_id; }
            set { _est_status_id = value; }
        }

        public long? ParentID
        {
            get { return _parent_id; }
            set { _parent_id = value; }
        }

        public DateTime? UploadDate
        {
            get { return _uploaddate; }
            set { _uploaddate = value; }
        }

        public List<CopyComponent> Components
        {
            get { return _components; }
        }

        public CopyAssemDistrib ADOptions
        {
            get { return _adoptions; }
        }

        public List<CopyPackage> Packages
        {
            get { return _packages; }
        }

        public List<CopyPackageComponentMap> PackageComponentMaps
        {
            get { return _pc_maps; }
        }

        public bool ClearIssueDateOverrides
        {
            set { _bClearIssueDateOverrides = value; }
        }

        #endregion
    }
}
