using System;
using System.Collections.Generic;
using System.Text;

using CatalogEstimating.Datasets;
using SourceGrid;

namespace CatalogEstimating.CustomGrids.Component.Editors
{
    public class EstimateMediaTypeEditor : SourceGrid.Cells.Editors.ComboBox
    {
        public EstimateMediaTypeEditor(Estimates dsEstimates)
            : base(typeof(IntPair))
        {
            List<IntPair> estimateMediaTypes = new List<IntPair>();

            foreach (Estimates.est_estimatemediatypeRow er in dsEstimates.est_estimatemediatype)
            {
                IntPair curEstimateMediaType = new IntPair(er.est_estimatemediatype_id, er.description);
                estimateMediaTypes.Add(curEstimateMediaType);
            }

            this.StandardValues = estimateMediaTypes;
            this.StandardValuesExclusive = true;
            this.Control.FormattingEnabled = true;
            this.AllowNull = true;
            this.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
        }

        public IntPair GetEstimateMediaTypefromID(int EstimateMediaTypeID)
        {
            foreach (IntPair emt in this.StandardValues)
                if (emt.Value == EstimateMediaTypeID)
                    return emt;

            return null;
        }
    }
}
