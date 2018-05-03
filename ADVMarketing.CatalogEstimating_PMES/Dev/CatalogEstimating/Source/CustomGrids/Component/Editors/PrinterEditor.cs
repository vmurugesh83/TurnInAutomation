using System;
using System.Collections.Generic;
using System.Text;

using CatalogEstimating.Datasets;
using System.Data;
using System.Data.SqlClient;

namespace CatalogEstimating.CustomGrids.Component.Editors
{
    public class PrinterEditor : SourceGrid.Cells.Editors.ComboBox
    {
        public PrinterEditor(Estimates dsEstimates, DateTime RunDate, long? PrinterID) : base(typeof(LongPair))
        {
            List<LongPair> vendors = new List<LongPair>();

            vendors.Add(new LongPair(-1, string.Empty));

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("Printer_s_PrinterIDandDescription_ByRunDate", conn))
                {
                    cmd.CommandTimeout = 7200;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (PrinterID.HasValue)
                        cmd.Parameters.AddWithValue("@VND_Printer_ID", PrinterID.Value);
                    else
                        cmd.Parameters.AddWithValue("@VND_Printer_ID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@RunDate", RunDate);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            LongPair curVendor = new LongPair(dr.GetInt64(dr.GetOrdinal("VND_Printer_ID")), dr["Description"].ToString());
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

        public LongPair GetPrinterfromID(long PrinterID)
        {
            foreach (LongPair p in this.StandardValues)
                if (p.Value == PrinterID)
                    return p;

            return null;
        }

        public LongPair GetPrinterfromDesc(string PrinterDescription)
        {
            foreach (LongPair p in this.StandardValues)
                if (p.Display == PrinterDescription)
                    return p;

            return null;
        }
    }
}
