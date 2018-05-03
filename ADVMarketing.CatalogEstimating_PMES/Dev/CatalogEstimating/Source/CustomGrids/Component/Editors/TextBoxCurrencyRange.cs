using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogEstimating.CustomGrids.Component.Editors
{
    public class TextBoxCurrencyRange : SourceGrid.Cells.Editors.TextBoxCurrency
    {
        public TextBoxCurrencyRange(decimal? minimumValue, decimal? maximumValue)
            : base(typeof(decimal))
        {
            if (minimumValue.HasValue)
                this.MinimumValue = minimumValue;
            if (maximumValue.HasValue)
                this.MaximumValue = maximumValue;
        }
    }
}
