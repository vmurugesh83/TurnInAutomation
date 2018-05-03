using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using CatalogEstimating.Datasets;
using SourceGrid;

namespace CatalogEstimating.CustomGrids.Component.Editors
{
    public class PaperWeightEditor : SourceGrid.Cells.Editors.ComboBox
    {
        public PaperWeightEditor(Estimates dsEstimates)
            : base(typeof(IntPair))
        {
            List<IntPair> paperWeights = new List<IntPair>();
            foreach (DataRowView drv_row in dsEstimates.ppr_paperweight.DefaultView)
            {
                Estimates.ppr_paperweightRow pw = (Estimates.ppr_paperweightRow)drv_row.Row;
                paperWeights.Add(new IntPair(pw.ppr_paperweight_id, pw.weight.ToString()));
            }

            this.StandardValues = paperWeights;
            this.StandardValuesExclusive = true;
            this.Control.FormattingEnabled = true;
            this.AllowNull = true;
            this.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
        }

        public IntPair GetWeightFromID(long PaperWeightID)
        {
            foreach (IntPair pw in this.StandardValues)
            {
                if (pw.Value == PaperWeightID)
                    return pw;
            }

            return null;
        }

        public IntPair GetWeightFromDesc(string PaperWeightDesc)
        {
            foreach (IntPair pw in this.StandardValues)
            {
                if (pw.Display == PaperWeightDesc)
                    return pw;
            }

            return null;
        }
    }
}
