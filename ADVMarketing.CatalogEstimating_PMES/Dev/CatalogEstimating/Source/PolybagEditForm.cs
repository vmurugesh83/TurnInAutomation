#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

using CatalogEstimating.Exceptions;

#endregion

namespace CatalogEstimating
{
    public partial class PolybagEditForm : CatalogEstimating.ChildForm
    {
        #region Construction

        public PolybagEditForm()
        : this( null, false, null )
        { }

        public PolybagEditForm( MainForm parent, bool readOnly, long? polybagGroupId )
        : base( parent, readOnly )
        {
            // Check to make sure this polybag group hasn't been deleted
            if ( polybagGroupId != null && !PolybagGroupExists( polybagGroupId.Value ) )
                throw new PolybagGroupNotExistException( polybagGroupId.Value );

            InitializeComponent();
            _polybagControl.PolybagGroupId= polybagGroupId ;
            _mainControl = _polybagControl;
            _menuFileSave.Click += new EventHandler( _polybagControl._btnSave_Click );
        }

        #endregion

        #region Override Methods

        public override long? DatabaseId
        {
            get { return _polybagControl.PolybagGroupId; }
        }

        #endregion

        #region Public Methods

        public static bool PolybagGroupExists( long polybagGroupId )
        {
            bool bExists = false;
            using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("EstPolybag_Search", conn);
                cmd.CommandTimeout = 7200;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue( "@EST_Polybag_ID", polybagGroupId );
                cmd.Parameters.AddWithValue( "@Description", DBNull.Value );
                cmd.Parameters.AddWithValue( "@Comments", DBNull.Value );
                cmd.Parameters.AddWithValue( "@EST_Season_ID", DBNull.Value );
                cmd.Parameters.AddWithValue( "@FiscalYear", DBNull.Value );
                cmd.Parameters.AddWithValue( "@FiscalMonth", DBNull.Value );
                cmd.Parameters.AddWithValue( "@HostAdNumber", DBNull.Value );
                cmd.Parameters.AddWithValue( "@StartRunDate", DBNull.Value );
                cmd.Parameters.AddWithValue( "@EndRunDate", DBNull.Value );
                cmd.Parameters.AddWithValue( "@CreatedBy", DBNull.Value );
                cmd.Parameters.AddWithValue( "@EST_Status_ID", DBNull.Value );

                using ( SqlDataReader dr = cmd.ExecuteReader() )
                {
                    bExists = dr.HasRows;
                    dr.Close();
                }

                conn.Close();
            }

            return bExists;
        }

        public bool AddEstimate( long estimateId )
        {
            return _polybagControl.AddEstimate( estimateId );
        }

        #endregion
    }
}