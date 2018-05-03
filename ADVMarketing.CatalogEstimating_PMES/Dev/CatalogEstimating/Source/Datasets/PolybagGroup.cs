using System.Data.SqlClient;

namespace CatalogEstimating.Datasets.PolybagGroupTableAdapters
{
    partial class est_packagepolybag_mapTableAdapter
    {
        public void SetTransaction( SqlTransaction tran )
        {
            if ( Adapter.UpdateCommand != null )
                Adapter.UpdateCommand.Transaction = tran;

            if ( Adapter.InsertCommand != null )
                Adapter.InsertCommand.Transaction = tran;

            if ( Adapter.DeleteCommand != null )
                Adapter.DeleteCommand.Transaction = tran;
        }
    }

    partial class est_estimatepolybaggroup_mapTableAdapter
    {
        public void SetTransaction( SqlTransaction tran )
        {
            if ( Adapter.UpdateCommand != null )
                Adapter.UpdateCommand.Transaction = tran;

            if ( Adapter.InsertCommand != null )
                Adapter.InsertCommand.Transaction = tran;

            if ( Adapter.DeleteCommand != null )
                Adapter.DeleteCommand.Transaction = tran;
        }
    }

    partial class est_polybagTableAdapter
    {
        public void SetTransaction( SqlTransaction tran )
        {
            if ( Adapter.UpdateCommand != null )
                Adapter.UpdateCommand.Transaction = tran;

            if ( Adapter.InsertCommand != null )
                Adapter.InsertCommand.Transaction = tran;

            if ( Adapter.DeleteCommand != null )
                Adapter.DeleteCommand.Transaction = tran;
        }
    }

    partial class est_polybaggroupTableAdapter
    {
        public void SetTransaction( SqlTransaction tran )
        {
            if ( Adapter.UpdateCommand != null )
                Adapter.UpdateCommand.Transaction = tran;

            if ( Adapter.InsertCommand != null )
                Adapter.InsertCommand.Transaction = tran;

            if ( Adapter.DeleteCommand != null )
                Adapter.DeleteCommand.Transaction = tran;
        }
    }
}

namespace CatalogEstimating.Datasets 
{
    partial class PolybagGroup
    {
        partial class vnd_bag_weightDataTable
        {
        }
    
        partial class est_packageDataTable
        {
        }
    
        partial class est_polybagRow
        {
            public bool IsValid()
            {
                return !Ispst_postalscenario_idNull();
            }
        }
    }
}
