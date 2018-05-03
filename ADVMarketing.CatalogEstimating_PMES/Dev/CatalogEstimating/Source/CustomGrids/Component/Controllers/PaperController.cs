using System;
using System.Collections.Generic;
using System.Text;

using CatalogEstimating.CustomGrids.Component.Editors;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;

namespace CatalogEstimating.CustomGrids.Component.Controllers
{
    public class PaperController : SourceGrid.Cells.Controllers.ControllerBase
    {
        private LongPair _previousValue = null;
        private SourceGrid.Cells.Views.Cell _disabledCell = new SourceGrid.Cells.Views.Cell();

        public PaperController()
        {
            _disabledCell.BackColor = SystemColors.Control;
            _disabledCell.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Gray), new DevAge.Drawing.BorderLine(Color.Gray));
        }

        public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnValueChanged(sender, e);

            ComponentGrid cGrid = (ComponentGrid)sender.Grid;

            // Determine the PaperVendorID
            LongPair vendorKeyValuePair = cGrid[39, sender.Position.Column].Value as LongPair;

            if (vendorKeyValuePair == _previousValue)
                return;
            else
                _previousValue = vendorKeyValuePair;

            long? PPR_PaperMap_ID = null;
            int? PPR_PaperWeight_ID = null;
            int? PPR_PaperGrade_ID = null;

            // Identify the default Paper Map for the Paper Vendor
            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("PprPaperMap_s_Default_ByPaperID", conn))
                {
                    cmd.CommandTimeout = 7200;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VND_Paper_ID", vendorKeyValuePair.Value);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            PPR_PaperMap_ID = dr.GetInt64(dr.GetOrdinal("PPR_Paper_Map_ID"));
                            PPR_PaperWeight_ID = dr.GetInt32(dr.GetOrdinal("PPR_PaperWeight_ID"));
                            PPR_PaperGrade_ID = dr.GetInt32(dr.GetOrdinal("PPR_PaperGrade_ID"));
                        }
                        dr.Close();
                    }
                }
                conn.Close();
            }

            // If a default rate has been found set the Paper Weight, Paper Grade and Paper Description cells accordingly
            if (PPR_PaperMap_ID.HasValue && !((bool) cGrid[14, sender.Position.Column].Value))
            {
                cGrid[42, sender.Position.Column].Value = ((PaperWeightEditor)cGrid[42, sender.Position.Column].Editor).GetWeightFromID(PPR_PaperWeight_ID.Value);
                cGrid[43, sender.Position.Column].Value = ((PaperGradeEditor)cGrid[43, sender.Position.Column].Editor).GetGradefromID(PPR_PaperGrade_ID.Value);
                cGrid[44, sender.Position.Column].Editor = new PaperMapEditor(cGrid.EstimateDS, vendorKeyValuePair.Value, PPR_PaperWeight_ID.Value, PPR_PaperGrade_ID.Value, cGrid.EstimateDS.est_estimate[0].rundate);
                cGrid[44, sender.Position.Column].Value = ((PaperMapEditor)cGrid[44, sender.Position.Column].Editor).GetPaperMapFromID(PPR_PaperMap_ID.Value);
                cGrid[44, sender.Position.Column].View = SourceGrid.Cells.Views.ComboBox.Default;
            }
            else
            {
                cGrid[44, sender.Position.Column].Editor = null;
                cGrid[44, sender.Position.Column].View = _disabledCell;
                cGrid[44, sender.Position.Column].Value = null;
            }
        }
    }
}
