using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogEstimating.CustomGrids.Component.Editors
{
    public class TextBoxIntegerRange : SourceGrid.Cells.Editors.TextBoxNumeric
    {
        public TextBoxIntegerRange(int? minimumValue, int? maximumValue)
            : base(typeof(int))
        {
            TypeConverter = new DevAge.ComponentModel.Converter.NumberTypeConverter(typeof(int), "#,##0");

            if (minimumValue.HasValue)
                this.MinimumValue = minimumValue.Value;
            if (maximumValue.HasValue)
                this.MaximumValue = maximumValue.Value;
        }
    }
}
