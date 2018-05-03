using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using CatalogEstimating.Datasets;

namespace CatalogEstimating
{
    public class CopyPackage
    {
        #region Private Members
        private long _OriginalPackageID;
        private string _Description;
        private string _Comments;
        private int _SoloQuantity;
        private int _OtherQuantity;
        private int? _PubQuantityTypeID;
        private long? _PubGroupID;
        private CopyPubGroup _PubGroup;
        #endregion

        #region Construction
        public CopyPackage()
        {
        }
        #endregion

        #region Public Methods
        public void LoadData()
        {
            if (_PubGroupID == null)
                return;

            _PubGroup = new CopyPubGroup(_PubGroupID.Value);
            _PubGroup.LoadData();

        }

        public Estimates.est_packageRow CreatePackageRecord_NoPubGroup(SqlConnection conn, Estimates dsEstimate, out string ErrorDescription)
        {
            Estimates.est_packageRow p_row = dsEstimate.est_package.Newest_packageRow();
            p_row.est_estimate_id = dsEstimate.est_estimate[0].est_estimate_id;
            p_row.description = _Description;
            if (string.IsNullOrEmpty(_Comments))
                p_row.SetcommentsNull();
            else
                p_row.comments = _Comments;
            p_row.soloquantity = _SoloQuantity;
            p_row.otherquantity = _OtherQuantity;
            p_row.Setpub_pubquantitytype_idNull();
            p_row.Setpub_pubgroup_idNull();
            p_row.createdby = MainForm.AuthorizedUser.FormattedName;
            p_row.createddate = DateTime.Now;
            dsEstimate.est_package.Addest_packageRow(p_row);

            ErrorDescription = string.Empty;
            return p_row;
        }

        public Estimates.est_packageRow CreatePackageRecord_CustomPubGroup(SqlConnection conn, Estimates dsEstimate, List<CopyPubRateMap> pubRateMaps, out string ErrorDescription)
        {
            Estimates.est_packageRow p_row = dsEstimate.est_package.Newest_packageRow();
            p_row.est_estimate_id = dsEstimate.est_estimate[0].est_estimate_id;
            p_row.description = _Description;
            if (string.IsNullOrEmpty(_Comments))
                p_row.SetcommentsNull();
            else
                p_row.comments = _Comments;
            p_row.soloquantity = _SoloQuantity;
            p_row.otherquantity = _OtherQuantity;
            p_row.pub_pubquantitytype_id = _PubQuantityTypeID.Value;

            long? customPubGroupID = _PubGroup.CreateCustomPubGroup(conn, dsEstimate, pubRateMaps, false, out ErrorDescription);
            if (customPubGroupID == null)
                return null;

            p_row.pub_pubgroup_id = customPubGroupID.Value;

            p_row.createdby = MainForm.AuthorizedUser.FormattedName;
            p_row.createddate = DateTime.Now;
            dsEstimate.est_package.Addest_packageRow(p_row);

            ErrorDescription = string.Empty;
            return p_row;
        }

        public Estimates.est_packageRow CreatePackageRecord_StandardPubGroup(SqlConnection conn, Estimates dsEstimate, List<CopyPubRateMap> pubRateMaps, out string ErrorDescription)
        {
            // Try to find the PUB Group in the Destination DB
            // If one is found that does not conflict with the list of pubratemaps used by the estimate, use it.
            long? pubGroupID = _PubGroup.FindStandardPubGroupID(conn, dsEstimate, pubRateMaps, out ErrorDescription);
            if (pubGroupID == null)
            {
                // If it couldn't be found or the group would have created a conflict, try to create a new custom pub group
                // Before creating a custom pub group check to see that it will contain at least one pubratemap
                bool bCustomGroupContainsPubRateMap = false;
                pubRateMaps.Sort();
                foreach (CopyPubRateMap prm in _PubGroup.PubRateMaps)
                {
                    if (pubRateMaps.BinarySearch(prm) < 0)
                    {
                        bCustomGroupContainsPubRateMap = true;
                        break;
                    }
                }

                if (bCustomGroupContainsPubRateMap)
                {
                    pubGroupID = _PubGroup.CreateCustomPubGroup(conn, dsEstimate, pubRateMaps, true, out ErrorDescription);
                    if (pubGroupID == null)
                        return null;
                }
            }

            Estimates.est_packageRow p_row = dsEstimate.est_package.Newest_packageRow();
            p_row.est_estimate_id = dsEstimate.est_estimate[0].est_estimate_id;
            p_row.description = _Description;
            if (string.IsNullOrEmpty(_Comments))
                p_row.SetcommentsNull();
            else
                p_row.comments = _Comments;
            p_row.soloquantity = _SoloQuantity;
            p_row.otherquantity = _OtherQuantity;

            // It is possible that the package no longer references any pubratemaps.
            // If that is the case, set the pubgroup and pubquantitytype to null
            if (PubGroupID == null)
            {
                p_row.Setpub_pubquantitytype_idNull();
                p_row.Setpub_pubgroup_idNull();
            }
            else
            {
                p_row.pub_pubquantitytype_id = _PubQuantityTypeID.Value;
                p_row.pub_pubgroup_id = pubGroupID.Value;
            }
            p_row.createdby = MainForm.AuthorizedUser.FormattedName;
            p_row.createddate = DateTime.Now;
            dsEstimate.est_package.Addest_packageRow(p_row);

            ErrorDescription = string.Empty;
            return p_row;
        }
        #endregion

        #region Properties

        public long OriginalPackageID
        {
            get { return _OriginalPackageID; }
            set { _OriginalPackageID = value; }
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

        public int SoloQuantity
        {
            get { return _SoloQuantity; }
            set { _SoloQuantity = value; }
        }

        public int OtherQuantity
        {
            get { return _OtherQuantity; }
            set { _OtherQuantity = value; }
        }

        public int? PubQuantityTypeID
        {
            get { return _PubQuantityTypeID; }
            set { _PubQuantityTypeID = value; }
        }

        public long? PubGroupID
        {
            get { return _PubGroupID; }
            set { _PubGroupID = value; }
        }

        public CopyPubGroup PubGroup
        {
            get { return _PubGroup; }
        }

        #endregion
    }
}
