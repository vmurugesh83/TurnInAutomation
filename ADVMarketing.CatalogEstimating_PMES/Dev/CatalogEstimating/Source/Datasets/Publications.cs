using System.Data.SqlClient;

namespace CatalogEstimating.Datasets {


    partial class Publications
    {
    }
}

namespace CatalogEstimating.Datasets.PublicationsTableAdapters
{
    partial class pub_pubgroupTableAdapter
    {
        public void SetTransaction(SqlTransaction tran)
        {
            if (Adapter.UpdateCommand != null)
                Adapter.UpdateCommand.Transaction = tran;

            if (Adapter.InsertCommand != null)
                Adapter.InsertCommand.Transaction = tran;

            if (Adapter.DeleteCommand != null)
                Adapter.DeleteCommand.Transaction = tran;
        }
    }

    partial class pub_pubgroupTableAdapter
    {
        public int CommandTimeout
        {
            get
            {
                if (Adapter.SelectCommand != null)
                    return Adapter.SelectCommand.CommandTimeout;
                else if (Adapter.UpdateCommand != null)
                    return Adapter.UpdateCommand.CommandTimeout;
                else if (Adapter.InsertCommand != null)
                    return Adapter.InsertCommand.CommandTimeout;
                else if (Adapter.DeleteCommand != null)
                    return Adapter.DeleteCommand.CommandTimeout;
                else
                    return -1;
            }
            set
            {
                if (Adapter.SelectCommand != null)
                    Adapter.SelectCommand.CommandTimeout = value;
                if (Adapter.UpdateCommand != null)
                    Adapter.UpdateCommand.CommandTimeout = value;
                if (Adapter.InsertCommand != null)
                    Adapter.InsertCommand.CommandTimeout = value;
                if (Adapter.DeleteCommand != null)
                    Adapter.DeleteCommand.CommandTimeout = value;
            }
        }
    }

    partial class pub_pubpubgroup_mapTableAdapter
    {
        public void SetTransaction(SqlTransaction tran)
        {
            if (Adapter.UpdateCommand != null)
                Adapter.UpdateCommand.Transaction = tran;

            if (Adapter.InsertCommand != null)
                Adapter.InsertCommand.Transaction = tran;

            if (Adapter.DeleteCommand != null)
                Adapter.DeleteCommand.Transaction = tran;
        }

        public int CommandTimeout
        {
            get
            {
                if (Adapter.SelectCommand != null)
                    return Adapter.SelectCommand.CommandTimeout;
                else if (Adapter.UpdateCommand != null)
                    return Adapter.UpdateCommand.CommandTimeout;
                else if (Adapter.InsertCommand != null)
                    return Adapter.InsertCommand.CommandTimeout;
                else if (Adapter.DeleteCommand != null)
                    return Adapter.DeleteCommand.CommandTimeout;
                else
                    return -1;
            }
            set
            {
                if (Adapter.SelectCommand != null)
                    Adapter.SelectCommand.CommandTimeout = value;
                if (Adapter.UpdateCommand != null)
                    Adapter.UpdateCommand.CommandTimeout = value;
                if (Adapter.InsertCommand != null)
                    Adapter.InsertCommand.CommandTimeout = value;
                if (Adapter.DeleteCommand != null)
                    Adapter.DeleteCommand.CommandTimeout = value;
            }
        }
    }
}
