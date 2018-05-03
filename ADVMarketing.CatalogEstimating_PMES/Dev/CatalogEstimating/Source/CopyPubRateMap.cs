using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace CatalogEstimating
{
    public class CopyPubRateMap : IComparable<CopyPubRateMap>
    {
        #region Private Members
        private string _PubID;
        private int _PubLocID;
        #endregion

        #region Construction
        public CopyPubRateMap(string PubID, int PubLocID)
        {
            _PubID = PubID;
            _PubLocID = PubLocID;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Determines the PubRateMapID for a PubID and PubLocID
        /// </summary>
        /// <param name="conn">Destination Database</param>
        /// <param name="RunDate">Run Date the PubID and PubLocID must be effetive on</param>
        /// <param name="ErrorDescription"></param>
        /// <returns>null - No PubRateMap record found for the PubID and PubLocID on the RunDate
        /// non-null - The PubRateMap record found for the PubID and PubLocID on the RunDate</returns>
        public long? FindPubRateMapID(SqlConnection conn, DateTime RunDate, out string ErrorDescription)
        {
            long? retval = null;

            SqlCommand cmd = new SqlCommand("PubRateMap_s_ByPubIDandPubLocID", conn);
            cmd.CommandTimeout = 7200;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Pub_ID", _PubID);
            cmd.Parameters.AddWithValue("@PubLoc_ID", _PubLocID);
            cmd.Parameters.AddWithValue("@RunDate", RunDate);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                ErrorDescription = string.Empty;
                retval = dr.GetInt64(dr.GetOrdinal("PUB_PubRate_Map_ID"));
            }
            else
            {
                ErrorDescription = "Pub Rate Map for Pub_ID: " + _PubID + " PubLoc_ID: " + _PubLocID.ToString() + " not found in destination database for Run Date: " + RunDate.ToShortDateString() + ".";
            }
            dr.Close();

            return retval;
        }
        #endregion

        #region Public Properties
        public string PubID
        {
            get { return _PubID; }
        }

        public int PubLocID
        {
            get { return _PubLocID; }
        }
        #endregion

        #region IComparable<CopyPubRateMap> Members

        public int CompareTo(CopyPubRateMap other)
        {
            if (_PubID.CompareTo(other.PubID) == 0)
                return _PubLocID.CompareTo(other.PubLocID);
            else
                return _PubID.CompareTo(other.PubID);
        }

        #endregion
    }
}
