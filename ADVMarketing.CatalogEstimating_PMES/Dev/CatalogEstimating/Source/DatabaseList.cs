using System;
using System.Collections.Generic;
using System.Text;

using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.AdministrationTableAdapters;

namespace CatalogEstimating
{
    public static class DatabaseList
    {
        public static List<CESDatabase> GetDatabases()
        {
            AssocDatabases_s_AllTableAdapter adapter = new AssocDatabases_s_AllTableAdapter();
            Administration ds = new Administration();
            adapter.Fill(ds.AssocDatabases_s_All);

            List<CESDatabase> lstDatabases = new List<CESDatabase>();
            foreach (Administration.AssocDatabases_s_AllRow row in ds.AssocDatabases_s_All)
            {
                lstDatabases.Add(new CESDatabase(row));
            }

            return lstDatabases;
        }
    }
}
