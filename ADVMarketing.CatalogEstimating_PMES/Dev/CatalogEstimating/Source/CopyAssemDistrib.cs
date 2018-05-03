using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using CatalogEstimating.Datasets;

namespace CatalogEstimating
{
    public class CopyAssemDistrib
    {
        #region Private Members
        private int _InsertDOW;
        private string _InsertFreightDesc;
        private decimal _InsertFreightCWT;
        private decimal _InsertFuelSurcharge;
        private bool _CornerGuards;
        private bool _Skids;
        private bool _InsertTime;
        private string _PostalScenarioDesc;
        private decimal _MailFuelSurcharge;
        private string _MailHouseDesc;
        private decimal _MailHouseOtherHandling;
        private bool _UseMailTracking;
        private string _MailTrackingDesc;
        private string _MailListDesc;
        private bool _UseExternalMailList;
        private int _ExternalMailQty;
        private decimal _ExternalMailCPM;
        private int _NbrOfCartons;
        private bool _UseGlueTack;
        private bool _UseTabbing;
        private bool _UseLetterInsertion;
        private bool _FirstClass;
        private decimal _OtherFreight;
        private decimal? _PostalDropFlat;
        #endregion

        #region Construction
        public CopyAssemDistrib()
        {
        }
        #endregion

        #region Public Methods
        public bool CreateDatasetRecord(SqlConnection conn, Estimates dsEstimate, out string ErrorDescription)
        {
            // Gather the new foreign keys
            long? PostalScenario_ID = null;
            long? InsertFreight_ID = null;
            long? MailHouse_ID = null;
            long? MailTracking_ID = null;
            long? MailList_ID = null;

            PostalScenario_ID = FindPostalScenario(conn, _PostalScenarioDesc, dsEstimate.est_estimate[0].rundate);
            if (PostalScenario_ID == null)
            {
                ErrorDescription = "Postal Scenario: " + _PostalScenarioDesc + " not found in destination database for Run Date: " + dsEstimate.est_estimate[0].rundate.ToShortDateString() + ".";
                return false;
            }

            InsertFreight_ID = FindVendor(conn, _InsertFreightDesc, 10);
            if (InsertFreight_ID == null)
            {
                ErrorDescription = "Insert Freight Vendor: " + _InsertFreightDesc + " not found in destination database.";
                return false;
            }

            MailHouse_ID = FindMailHouseRate(conn, _MailHouseDesc, dsEstimate.est_estimate[0].rundate);
            if (MailHouse_ID == null)
            {
                ErrorDescription = "MailHouse Rate for Vendor: " + _MailHouseDesc + " not found in destination database for Run Date: " + dsEstimate.est_estimate[0].rundate.ToShortDateString() + ".";
                return false;
            }

            if (!string.IsNullOrEmpty(_MailTrackingDesc))
            {
                MailTracking_ID = FindMailTrackingRate(conn, _MailTrackingDesc, dsEstimate.est_estimate[0].rundate);
                if (MailTracking_ID == null)
                {
                    ErrorDescription = "Mail Tracking Rate for Vendor: " + _MailTrackingDesc + " not found in destination database for Run Date: " + dsEstimate.est_estimate[0].rundate.ToShortDateString() + ".";
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(_MailListDesc))
            {
                MailList_ID = FindMailListResourceRate(conn, _MailListDesc, dsEstimate.est_estimate[0].rundate);
                if (MailList_ID == null)
                {
                    ErrorDescription = "Mail List Resource Rate for Vendor: " + _MailListDesc + " not found in destination database for Run Date: " + dsEstimate.est_estimate[0].rundate.ToShortDateString() + ".";
                    return false;
                }
            }

            // If all foreign keys have been found, create a record in the dataset
            Estimates.est_assemdistriboptionsRow ad_row = dsEstimate.est_assemdistriboptions.Newest_assemdistriboptionsRow();
            ad_row.est_estimate_id = dsEstimate.est_estimate[0].est_estimate_id;
            ad_row.insertdow = _InsertDOW;
            ad_row.insertfreightvendor_id = InsertFreight_ID.Value;
            ad_row.insertfreightcwt = _InsertFreightCWT;
            ad_row.insertfuelsurcharge = _InsertFuelSurcharge;
            ad_row.cornerguards = _CornerGuards;
            ad_row.skids = _Skids;
            ad_row.inserttime = _InsertTime;
            ad_row.pst_postalscenario_id = PostalScenario_ID.Value;
            ad_row.mailfuelsurcharge = _MailFuelSurcharge;
            ad_row.mailhouse_id = MailHouse_ID.Value;
            ad_row.mailhouseotherhandling = _MailHouseOtherHandling;
            ad_row.usemailtracking = _UseMailTracking;
            if (MailTracking_ID == null)
                ad_row.Setmailtracking_idNull();
            else
                ad_row.mailtracking_id = MailTracking_ID.Value;
            if (MailList_ID == null)
                ad_row.Setmaillistresource_idNull();
            else
                ad_row.maillistresource_id = MailList_ID.Value;
            ad_row.useexternalmaillist = _UseExternalMailList;
            ad_row.externalmailqty = _ExternalMailQty;
            ad_row.externalmailcpm = _ExternalMailCPM;
            ad_row.nbrofcartons = _NbrOfCartons;
            ad_row.usegluetack = _UseGlueTack;
            ad_row.usetabbing = _UseTabbing;
            ad_row.useletterinsertion = _UseLetterInsertion;
            ad_row.firstclass = _FirstClass;
            ad_row.otherfreight = _OtherFreight;
            if (PostalDropFlat == null)
                ad_row.SetpostaldropflatNull();
            else
                ad_row.postaldropflat = PostalDropFlat.Value;
            ad_row.createdby = MainForm.AuthorizedUser.FormattedName;
            ad_row.createddate = DateTime.Now;

            dsEstimate.est_assemdistriboptions.Addest_assemdistriboptionsRow(ad_row);

            ErrorDescription = string.Empty;
            return true;
        }
        #endregion

        #region Private Methods
        private long? FindPostalScenario(SqlConnection conn, string Description, DateTime RunDate)
        {
            long? retval = null;

            SqlCommand cmd = new SqlCommand("PstPostalScenario_s_ByDescriptionandRunDate", conn);
            cmd.CommandTimeout = 7200;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@RunDate", RunDate);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                retval = dr.GetInt64(dr.GetOrdinal("PST_PostalScenario_ID"));
            }
            dr.Close();

            return retval;
        }

        private long? FindMailHouseRate(SqlConnection conn, string Description, DateTime RunDate)
        {
            long? retval = null;
            SqlCommand cmd = new SqlCommand("VndMailHouseRate_s_ByDescriptionandRunDate", conn);
            cmd.CommandTimeout = 7200;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@RunDate", RunDate);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                retval = dr.GetInt64(dr.GetOrdinal("VND_MailHouseRate_ID"));
            }
            dr.Close();

            return retval;
        }

        private long? FindMailTrackingRate(SqlConnection conn, string Description, DateTime RunDate)
        {
            long? retval = null;
            SqlCommand cmd = new SqlCommand("VndMailTrackingRate_s_ByDescriptionandRunDate", conn);
            cmd.CommandTimeout = 7200;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@RunDate", RunDate);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                retval = dr.GetInt64(dr.GetOrdinal("VND_MailTrackingRate_ID"));
            }
            dr.Close();

            return retval;
        }

        private long? FindMailListResourceRate(SqlConnection conn, string Description, DateTime RunDate)
        {
            long? retval = null;
            SqlCommand cmd = new SqlCommand("VndMailListResourceRate_s_ByDescriptionandRunDate", conn);
            cmd.CommandTimeout = 7200;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@RunDate", RunDate);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                retval = dr.GetInt64(dr.GetOrdinal("VND_MailListResourceRate_ID"));
            }
            dr.Close();

            return retval;
        }

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
        #endregion

        #region Public Properties

        public int InsertDOW
        {
            get { return _InsertDOW; }
            set { _InsertDOW = value; }
        }

        public string InsertFreightDesc
        {
            get { return _InsertFreightDesc; }
            set { _InsertFreightDesc = value; }
        }

        public decimal InsertFreightCWT
        {
            get { return _InsertFreightCWT; }
            set { _InsertFreightCWT = value; }
        }

        public decimal InsertFuelSurcharge
        {
            get { return _InsertFuelSurcharge; }
            set { _InsertFuelSurcharge = value; }
        }

        public bool CornerGuards
        {
            get { return _CornerGuards; }
            set { _CornerGuards = value; }
        }

        public bool Skids
        {
            get { return _Skids; }
            set { _Skids = value; }
        }

        public bool InsertTime
        {
            get { return _InsertTime; }
            set { _InsertTime = value; }
        }

        public string PostalScenarioDesc
        {
            get { return _PostalScenarioDesc; }
            set { _PostalScenarioDesc = value; }
        }

        public decimal MailFuelSurcharge
        {
            get { return _MailFuelSurcharge; }
            set { _MailFuelSurcharge = value; }
        }

        public string MailHouseDesc
        {
            get { return _MailHouseDesc; }
            set { _MailHouseDesc = value; }
        }

        public decimal MailHouseOtherHandling
        {
            get { return _MailHouseOtherHandling; }
            set { _MailHouseOtherHandling = value; }
        }

        public bool UseMailTracking
        {
            get { return _UseMailTracking; }
            set { _UseMailTracking = value; }
        }

        public string MailTrackingDesc
        {
            get { return _MailTrackingDesc; }
            set { _MailTrackingDesc = value; }
        }

        public string MailListDesc
        {
            get { return _MailListDesc; }
            set { _MailListDesc = value; }
        }

        public bool UseExternalMailList
        {
            get { return _UseExternalMailList; }
            set { _UseExternalMailList = value; }
        }

        public int ExternalMailQty
        {
            get { return _ExternalMailQty; }
            set { _ExternalMailQty = value; }
        }

        public decimal ExternalMailCPM
        {
            get { return _ExternalMailCPM; }
            set { _ExternalMailCPM = value; }
        }

        public int NbrOfCartons
        {
            get { return _NbrOfCartons; }
            set { _NbrOfCartons = value; }
        }

        public bool UseGlueTack
        {
            get { return _UseGlueTack; }
            set { _UseGlueTack = value; }
        }

        public bool UseTabbing
        {
            get { return _UseTabbing; }
            set { _UseTabbing = value; }
        }

        public bool UseLetterInsertion
        {
            get { return _UseLetterInsertion; }
            set { _UseLetterInsertion = value; }
        }

        public bool FirstClass
        {
            get { return _FirstClass; }
            set { _FirstClass = value; }
        }

        public decimal OtherFreight
        {
            get { return _OtherFreight; }
            set { _OtherFreight = value; }
        }

        public decimal? PostalDropFlat
        {
            get { return _PostalDropFlat; }
            set { _PostalDropFlat = value; }
        }

        #endregion
    }
}
