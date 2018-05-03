using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using CatalogEstimating.Datasets;
using SourceGrid;

namespace CatalogEstimating.CustomGrids.Component.Editors
{
    public class PaperGradeEditor : SourceGrid.Cells.Editors.ComboBox
    {
        public PaperGradeEditor(Estimates dsEstimates) : base(typeof(IntPair))
        {
            List<IntPair> paperGrades = new List<IntPair>();
            foreach (DataRowView drv_row in dsEstimates.ppr_papergrade.DefaultView)
            {
                Estimates.ppr_papergradeRow pr = (Estimates.ppr_papergradeRow)drv_row.Row;
                paperGrades.Add(new IntPair(pr.ppr_papergrade_id, pr.grade));
            }

            this.StandardValues = paperGrades;
            this.StandardValuesExclusive = true;
            this.Control.FormattingEnabled = true;
            this.AllowNull = true;
            this.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
        }

        public IntPair GetGradefromID(long PaperGradeID)
        {
            foreach (IntPair pg in this.StandardValues)
            {
                if (pg.Value == PaperGradeID)
                    return pg;
            }

            return null;
        }

        public IntPair GetGradefromDescription(string PaperGradeDescription)
        {
            foreach (IntPair pg in this.StandardValues)
            {
                if (pg.Display == PaperGradeDescription)
                    return pg;
            }
            return null;
        }
    }
}
