using System.Data.SqlClient;
namespace CatalogEstimating.Datasets 
{
    
    partial class Administration
    {
    }
}

namespace CatalogEstimating.Datasets.AdministrationTableAdapters
{
    public partial class vnd_mailhouserateTableAdapter
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

    public partial class vnd_maillistresourcerateTableAdapter
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

    public partial class vnd_mailtrackingrateTableAdapter
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

    public partial class vnd_paperTableAdapter
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

    public partial class vnd_printerTableAdapter
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

    public partial class prt_printerrateTableAdapter
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

    public partial class ppr_papergradeTableAdapter
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

    public partial class ppr_paperweightTableAdapter
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

    public partial class ppr_paper_mapTableAdapter
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

    public partial class vnd_vendorTableAdapter
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

    public partial class vnd_vendorvendortype_mapTableAdapter
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