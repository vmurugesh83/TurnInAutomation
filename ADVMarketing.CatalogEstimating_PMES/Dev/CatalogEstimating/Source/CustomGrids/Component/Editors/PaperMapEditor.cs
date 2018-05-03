using System;
using System.Collections.Generic;
using System.Text;

using CatalogEstimating.Datasets;
using System.Data;
using System.Data.SqlClient;

namespace CatalogEstimating.CustomGrids.Component.Editors
{
    public class PaperMapEditor : SourceGrid.Cells.Editors.ComboBox
    {
        private LongPair _defaultPaperMap = null;

        public PaperMapEditor(Estimates dsEstimates, long PaperID, int PaperWeightID, int PaperGradeID, DateTime RunDate) : base(typeof(LongPair))
        {
            List<LongPair> paperMaps = new List<LongPair>();

            using (SqlConnection conn = (SqlConnection) MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("PaperMap_s_ByPaperIDPaperWeightIDPaperGradeIDRunDate", conn))
                {
                    cmd.CommandTimeout = 7200;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VND_Paper_ID", PaperID);
                    cmd.Parameters.AddWithValue("@PPR_PaperWeight_ID", PaperWeightID);
                    cmd.Parameters.AddWithValue("@PPR_PaperGrade_ID", PaperGradeID);
                    cmd.Parameters.AddWithValue("@RunDate", RunDate);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            LongPair curPaperMap = new LongPair(dr.GetInt64(dr.GetOrdinal("PPR_Paper_Map_ID")), dr["description"].ToString());
                            if (dr.GetBoolean(dr.GetOrdinal("default")))
                                _defaultPaperMap = curPaperMap;
                            paperMaps.Add(curPaperMap);
                        }
                        dr.Close();
                    }
                }
                conn.Close();
            }

            this.StandardValues = paperMaps;
            this.StandardValuesExclusive = true;
            this.Control.FormattingEnabled = true;
            this.AllowNull = true;
            this.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey | SourceGrid.EditableMode.SingleClick;
        }

        public LongPair DefaultPaperMap
        {
            get { return _defaultPaperMap; }
        }

        public LongPair GetPaperMapFromID(long PaperMapID)
        {
            foreach (LongPair pm in this.StandardValues)
                if (pm.Value == PaperMapID)
                    return pm;

            return null;
        }

        public LongPair GetPaperMapfromDescription(string PaperMapDescription)
        {
            foreach (LongPair pm in this.StandardValues)
                if (pm.Display == PaperMapDescription)
                    return pm;

            return null;
        }
    }
}
