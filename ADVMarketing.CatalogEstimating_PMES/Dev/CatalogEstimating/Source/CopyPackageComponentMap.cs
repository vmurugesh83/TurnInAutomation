using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogEstimating
{
    public class CopyPackageComponentMap
    {
        #region Private Members
        private long _originalPackageID;
        private long _originalComponentID;
        #endregion

        #region Construction
        public CopyPackageComponentMap(long PackageID, long ComponentID)
        {
            _originalPackageID = PackageID;
            _originalComponentID = ComponentID;
        }
        #endregion

        #region Public Properties
        public long OriginalPackageID
        {
            get { return _originalPackageID; }
        }

        public long OriginalComponentID
        {
            get { return _originalComponentID; }
        }
        #endregion
    }
}
