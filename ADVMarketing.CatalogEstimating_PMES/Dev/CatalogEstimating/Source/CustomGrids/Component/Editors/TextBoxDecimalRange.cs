using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogEstimating.CustomGrids.Component.Editors
{
    public class TextBoxDecimalRange : SourceGrid.Cells.Editors.TextBoxNumeric
    {
        public TextBoxDecimalRange( decimal? minimumValue, decimal? maximumValue )
        : this( minimumValue, maximumValue, 4 )
        { }

        public TextBoxDecimalRange( decimal? minimumValue, decimal? maximumValue, int precision )
        : base(typeof(decimal))
        {
            string format = "#,##0.";
            for ( int i = 0; i < precision; i++ )
                format += "0";

            TypeConverter = new DevAge.ComponentModel.Converter.NumberTypeConverter( typeof( decimal ), format );

            if (minimumValue.HasValue)
                this.MinimumValue = minimumValue;
            if (maximumValue.HasValue)
                this.MaximumValue = maximumValue;
        }
    }
}
