using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using CatalogEstimating.Datasets;

namespace CatalogEstimating
{
    public class CopyPubGroup
    {
        #region Private Members
        private long _originalPubGroupID;
        private string _Description;
        private string _Comments;

        private bool _CustomPubGroupFlag;

        private List<CopyPubRateMap> _PubRateMaps = new List<CopyPubRateMap>();
        #endregion

        #region Construction
        public CopyPubGroup(long PubGroupID)
        {
            _originalPubGroupID = PubGroupID;
        }
        #endregion

        #region Public Methods

        public void LoadData()
        {
            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                SqlCommand pg_cmd = new SqlCommand("PubGroup_s_ByPubGroupID", conn);
                pg_cmd.CommandTimeout = 7200;
                pg_cmd.CommandType = CommandType.StoredProcedure;
                pg_cmd.Parameters.AddWithValue("@PUB_PubGroup_ID", _originalPubGroupID);
                SqlDataReader pg_dr = pg_cmd.ExecuteReader();
                if (pg_dr.Read())
                {
                    _Description = pg_dr["Description"].ToString();
                    _Comments = pg_dr["Comments"].ToString();
                    _CustomPubGroupFlag = pg_dr.GetBoolean(pg_dr.GetOrdinal("CustomGroupForPackage"));
                }
                pg_dr.Close();

                SqlCommand cmd = new SqlCommand("PubRateMap_s_ByPubGroupID", conn);
                cmd.CommandTimeout = 7200;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PUB_PubGroup_ID", _originalPubGroupID);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    _PubRateMaps.Add(new CopyPubRateMap(dr["PUB_ID"].ToString(), dr.GetInt32(dr.GetOrdinal("PUBLoc_ID"))));
                }
                dr.Close();

                conn.Close();
            }
        }

        /// <summary>
        /// Creates a custom pub group.  Any pub-locs that were included by the original group will be included in the new pub group.
        /// Unless they are already reference by another package (in the list of pubRateMaps).
        /// </summary>
        /// <param name="conn">Connection to Destination Database</param>
        /// <param name="dsEstimate">Current Estimate Dataset</param>
        /// <param name="pubRateMaps">List of all Pub Rate Map records referenced by the estimate</param>
        /// <param name="ErrorDescription"></param>
        /// <returns></returns>
        public long? CreateCustomPubGroup(SqlConnection conn, Estimates dsEstimate, List<CopyPubRateMap> pubRateMaps, bool bAddCustomPrefix, out string ErrorDescription)
        {
            // Create the Pub Group record in the destination database
            Estimates.pub_pubgroupRow pg_row = dsEstimate.pub_pubgroup.Newpub_pubgroupRow();
            if (bAddCustomPrefix)
                pg_row.description = string.Concat("Custom Group: ", _Description).Substring(0, Math.Min(14 + _Description.Length, 35));
            else
                pg_row.description = _Description;

            if (string.IsNullOrEmpty(_Comments))
                pg_row.SetcommentsNull();
            else
                pg_row.comments = _Comments;
            pg_row.active = true;
            pg_row.effectivedate = new DateTime(1900, 1, 1); // dsEstimate.est_estimate[0].rundate;
            pg_row.sortorder = 1000000;
            pg_row.customgroupforpackage = true;
            pg_row.createdby = MainForm.AuthorizedUser.FormattedName;
            pg_row.createddate = DateTime.Now;
            dsEstimate.pub_pubgroup.Addpub_pubgroupRow(pg_row);

            // Map the new pub group record to the pub-locs that it should include.
            foreach (CopyPubRateMap prm in _PubRateMaps)
            {
                pubRateMaps.Sort();
                if (pubRateMaps.BinarySearch(prm) < 0)
                {
                    long? PubRateMapID = prm.FindPubRateMapID(conn, dsEstimate.est_estimate[0].rundate, out ErrorDescription);
                    if (PubRateMapID == null)
                        return null;

                    Estimates.pub_pubpubgroup_mapRow ppgm = dsEstimate.pub_pubpubgroup_map.Newpub_pubpubgroup_mapRow();
                    ppgm.pub_pubrate_map_id = PubRateMapID.Value;
                    ppgm.pub_pubgroup_id = pg_row.pub_pubgroup_id;
                    ppgm.createdby = MainForm.AuthorizedUser.FormattedName;
                    ppgm.createddate = DateTime.Now;
                    dsEstimate.pub_pubpubgroup_map.Addpub_pubpubgroup_mapRow(ppgm);
                    pubRateMaps.Add(prm);
                }
            }

            ErrorDescription = string.Empty;
            return pg_row.pub_pubgroup_id;
        }

        /// <summary>
        /// Returns the Pub Group ID in the destination database which contains the same Pub Rate Maps as the original Pub Group
        /// from the source database.
        /// </summary>
        /// <param name="conn">Destination Database</param>
        /// <param name="dsEstimate">Dataset containing the Estimate record to be saved to the destination database.</param>
        /// <param name="pubRateMaps">List of pub rate maps referenced by the estimate</param>
        /// <param name="ErrorDescription"></param>
        /// <returns></returns>
        public long? FindStandardPubGroupID(SqlConnection conn, Estimates dsEstimate, List<CopyPubRateMap> pubRateMaps, out string ErrorDescription)
        {
            long? pubGroupID = null;

            SqlCommand cmd = new SqlCommand("PubGroup_s_ByDescriptionandRunDate", conn);
            cmd.CommandTimeout = 7200;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Description", _Description);
            cmd.Parameters.AddWithValue("@RunDate", dsEstimate.est_estimate[0].rundate);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                pubGroupID = dr.GetInt64(dr.GetOrdinal("PUB_PubGroup_ID"));
            }
            dr.Close();

            if (pubGroupID == null)
            {
                ErrorDescription = string.Empty;
                return pubGroupID;
            }

            List<CopyPubRateMap> prm_InGroup = new List<CopyPubRateMap>();
            SqlCommand cmd_prm = new SqlCommand("PubRateMap_s_ByPubGroupID", conn);
            cmd_prm.CommandTimeout = 7200;
            cmd_prm.CommandType = CommandType.StoredProcedure;
            cmd_prm.Parameters.AddWithValue("@PUB_PubGroup_ID", pubGroupID.Value);
            SqlDataReader dr_prm = cmd_prm.ExecuteReader();
            while (dr_prm.Read())
            {
                prm_InGroup.Add(new CopyPubRateMap(dr_prm["PUB_ID"].ToString(), dr_prm.GetInt32(dr_prm.GetOrdinal("PubLoc_ID"))));
            }
            dr_prm.Close();

            // Look to see if any of the pubratemaps already exist in the parent collection
            bool bCollisionFound = false;
            PubRateMaps.Sort();
            foreach (CopyPubRateMap prm in prm_InGroup)
            {
                if (pubRateMaps.BinarySearch(prm) >= 0)
                {
                    bCollisionFound = true;
                    break;
                }
            }

            // If a collision was found we can't use the stanard pub group, return null;
            if (bCollisionFound)
            {
                ErrorDescription = string.Empty;
                return null;
            }
            // If no collision was found, add all of the pubratemaps to the parent collection and return the ID
            else
            {
                pubRateMaps.AddRange(prm_InGroup);
                ErrorDescription = string.Empty;
                return pubGroupID;
            }
        }

        #endregion

        #region Public Properties

        public bool CustomPubGroupFlag
        {
            get { return _CustomPubGroupFlag; }
            set { _CustomPubGroupFlag = value; }
        }

        public List<CopyPubRateMap> PubRateMaps
        {
            get { return _PubRateMaps; }
            set { _PubRateMaps = value; }
        }
        #endregion
    }
}
