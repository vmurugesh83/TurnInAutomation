using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogEstimating
{
    public class CopyPubIssueDate
    {
        #region Private Members
        private bool _Override;
        private int _IssueDOW;
        private DateTime _IssueDate;
        private string _PubID;
        private int _PubLocID;
        #endregion

        #region Construction
        public CopyPubIssueDate(bool bOverride, int issueDOW, DateTime issueDate, string pubID, int pubLocID)
        {
            _Override = bOverride;
            _IssueDOW = issueDOW;
            _IssueDate = issueDate;
            _PubID = pubID;
            _PubLocID = pubLocID;
        }
        #endregion

        #region Public Properties
        public bool Override
        {
            get { return _Override; }
        }

        public int IssueDOW
        {
            get { return _IssueDOW; }
        }

        public DateTime IssueDate
        {
            get { return _IssueDate; }
        }

        public string PubID
        {
            get { return _PubID; }
        }

        public int PubLocID
        {
            get { return _PubLocID; }
        }

        #endregion
    }
}
