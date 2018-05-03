#region Using Directives

using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using CatalogEstimating.Datasets;

#endregion

namespace CatalogEstimating
{
    public class CESDatabase
    {
        #region Construction

        public CESDatabase( Administration.AssocDatabases_s_AllRow row )
        {
            _id = row.database_id;
            _friendlyName = row.description;
            _dbType = (DatabaseType)row.databasetype_id;
            _display = row.display;
            _databasename = row.databasename;

            _db = new SqlDatabase( row.connectionstring );
        }

        #endregion

        #region Public Properties

        private int _id;
        public int Id
        {
            get { return _id;  }
        }

        private string _friendlyName;
        public string FriendlyName
        {
            get { return _friendlyName;  }
        }

        private Database _db = null;
        public Database Database
        {
            get { return _db; }
        }

        private DatabaseType _dbType;
        public DatabaseType Type
        {
            get { return _dbType; }
        }

        private bool _isWorking = false;
        public bool IsWorking
        {
            get { return _isWorking;  }
            set { _isWorking = value; }
        }

        private bool _display = true;
        public bool Display
        {
            get { return _display;  }
        }

        private string _databasename = string.Empty;
        public string DatabaseName
        {
            get { return _databasename; }
        }

        #endregion

        #region Public Overrides

        public override string ToString()
        {
            return FriendlyName;
        }

        #endregion
    }
}