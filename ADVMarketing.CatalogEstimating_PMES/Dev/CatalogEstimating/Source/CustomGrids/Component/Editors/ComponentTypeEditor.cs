using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using CatalogEstimating.Datasets;
using SourceGrid;

namespace CatalogEstimating.CustomGrids.Component.Editors
{
    public class ComponentTypeEditor : SourceGrid.Cells.Editors.ComboBox
    {
        public ComponentTypeEditor(Estimates dsEstimates)
            : base(typeof(IntPair))
        {
            List<IntPair> componentTypes = new List<IntPair>();

            foreach (DataRow dr in dsEstimates.est_componenttype.Select("EST_ComponentType_ID <> 1"))
            {
                Estimates.est_componenttypeRow cr = (Estimates.est_componenttypeRow)dr;
                IntPair curComponentType = new IntPair(cr.est_componenttype_id, cr.description);
                componentTypes.Add(curComponentType);
            }

            this.StandardValues = componentTypes;
            this.StandardValuesExclusive = true;
            this.Control.FormattingEnabled = true;
            this.AllowNull = true;
            this.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
        }
        public IntPair GetComponentTypefromID(int ComponentTypeID)
        {
            foreach (IntPair c in this.StandardValues)
                if (c.Value == ComponentTypeID)
                    return c;

            return null;
        }
    }
}
