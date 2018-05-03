using System;
using System.Collections.Generic;
using System.Text;

using CatalogEstimating.Datasets;
using System.Data;
using System.Data.SqlClient;

namespace CatalogEstimating.CustomGrids.Component.Editors
{
    public class PaperEditor : SourceGrid.Cells.Editors.ComboBox
    {
        public PaperEditor(Estimates dsEstimates, DateTime RunDate, long? PaperID)
            : base(typeof(LongPair))
        {
            List<LongPair> vendors = new List<LongPair>();
            vendors.Add(new LongPair(-1, string.Empty));

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("Paper_s_PaperIDandDescription_ByRunDate", conn))
                {
                    cmd.CommandTimeout = 7200;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (PaperID.HasValue)
                        cmd.Parameters.AddWithValue("@VND_Paper_ID", PaperID.Value);
                    else
                        cmd.Parameters.AddWithValue("@VND_Paper_ID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@RunDate", RunDate);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            LongPair curVendor = new LongPair(dr.GetInt64(dr.GetOrdinal("VND_Paper_ID")), dr["Description"].ToString());
                            vendors.Add(curVendor);
                        }
                        dr.Close();
                    }
                }
                conn.Close();
            }

            this.StandardValues = vendors;
            this.StandardValuesExclusive = true;
            this.Control.FormattingEnabled = true;
            this.AllowNull = true;
            this.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey | SourceGrid.EditableMode.SingleClick;
        }

        public LongPair GetPaperfromDesc(string PaperDescription)
        {
            foreach (LongPair p in this.StandardValues)
                if (p.Display == PaperDescription)
                    return p;

            return null;
        }

        public LongPair GetPaperfromID(long PaperID)
        {
            foreach (LongPair p in this.StandardValues)
                if (p.Value == PaperID)
                    return p;

            return null;
        }
    }
}
