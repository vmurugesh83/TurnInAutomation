using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using CatalogEstimating.Datasets;

namespace CatalogEstimating.CustomGrids.Component.Editors
{
    public class PrinterRateEditor : SourceGrid.Cells.Editors.ComboBox
    {
        private LongPair _defaultPrinterRate = null;

        public PrinterRateEditor(Estimates dsEstimates, long PrinterID, int PrinterRateTypeID)
            : base(typeof(LongPair))
        {
            List<LongPair> printerRates = new List<LongPair>();

            printerRates.Add(new LongPair(-1, string.Empty));

            using (SqlConnection conn = (SqlConnection) MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("PrinterRate_s_ByPrinterIDPrinterRateTypeID", conn))
                {
                    cmd.CommandTimeout = 7200;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VND_Printer_ID", PrinterID);
                    cmd.Parameters.AddWithValue("@PRT_PrinterRateType_ID", PrinterRateTypeID);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            LongPair curPrinterRate = new LongPair(dr.GetInt64(dr.GetOrdinal("PRT_PrinterRate_ID")), dr["description"].ToString());
                            printerRates.Add(curPrinterRate);
                            if (dr.GetBoolean(dr.GetOrdinal("default")))
                            {
                                _defaultPrinterRate = curPrinterRate;
                            }
                        }
                        dr.Close();
                    }
                }
                conn.Close();
            }

            this.StandardValues = printerRates;
            this.StandardValuesExclusive = true;
            this.Control.FormattingEnabled = true;
            this.AllowNull = true;
            this.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey | SourceGrid.EditableMode.SingleClick;
        }

        public LongPair FindRatefromDesc(string RateDescription)
        {
            foreach (LongPair r in this.StandardValues)
                if (r.Display == RateDescription)
                    return r;

            return null;
        }

        public LongPair FindRatefromID(long RateID)
        {
            foreach (LongPair r in this.StandardValues)
                if (r.Value == RateID)
                    return r;

            return null;
        }

        public LongPair DefaultPrinterRate
        {
            get { return _defaultPrinterRate; }
        }
    }
}
