using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogEstimating.Exceptions
{
    class PolybagGroupNotExistException : System.Exception
    {
        public PolybagGroupNotExistException( long polybagGroupId )
        : base( string.Format( "Polybag Group {0} Does Not Exist", polybagGroupId ) )
        {
            _polybagGroupId = polybagGroupId;
        }

        private long _polybagGroupId;
        public long PolybagGroupId
        {
            get { return _polybagGroupId;  }
            set { _polybagGroupId = value; }
        }
    }
}
