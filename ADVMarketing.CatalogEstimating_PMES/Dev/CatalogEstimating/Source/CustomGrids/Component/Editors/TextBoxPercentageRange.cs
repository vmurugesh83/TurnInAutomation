using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogEstimating.CustomGrids.Component.Editors
{
    public class TextBoxPercentageRange : SourceGrid.Cells.Editors.TextBoxNumeric
    {
        public TextBoxPercentageRange(decimal? minValue, decimal? maxValue) : base(typeof(decimal))
        {
            TypeConverter = new DevAge.ComponentModel.Converter.PercentTypeConverter(typeof(decimal));

            if (minValue.HasValue)
                this.MinimumValue = minValue.Value;

            if (maxValue.HasValue)
                this.MaximumValue = maxValue.Value;
        }
    }
}
