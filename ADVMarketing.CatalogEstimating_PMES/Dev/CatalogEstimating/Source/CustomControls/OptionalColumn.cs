using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace CatalogEstimating.CustomControls
{
    public class OptionalButtonColumn : DataGridViewButtonColumn
    {
        public OptionalButtonColumn()
        {
            this.CellTemplate = new OptionalButtonCell();
        }
    }

    public class OptionalButtonCell : DataGridViewButtonCell
    {
        private bool _display;
        public bool Display
        {
            get
            {
                return _display;
            }
            set
            {
                _display = value;
            }
        }

        // Override the Clone method so that the Display property is copied.
        public override object Clone()
        {
            OptionalButtonCell cell =
                (OptionalButtonCell)base.Clone();
            cell.Display = _display;
            return cell;
        }

        // By default, display the button cell.
        public OptionalButtonCell()
        {
            _display = true;
        }

        protected override void Paint(Graphics graphics,
            Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates elementState, object value,
            object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            // The button cell is disabled, so paint the border,  
            // background, and disabled button for the cell.
            if (!this.Display)
            {
                #region if block
                // Draw the cell background, if specified.
                if ((paintParts & DataGridViewPaintParts.Background) ==
                    DataGridViewPaintParts.Background)
                {
                    SolidBrush cellBackground =
                        new SolidBrush(cellStyle.BackColor);
                    graphics.FillRectangle(cellBackground, cellBounds);
                    cellBackground.Dispose();
                }

                // Draw the cell borders, if specified.
                if ((paintParts & DataGridViewPaintParts.Border) ==
                    DataGridViewPaintParts.Border)
                {
                    PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                        advancedBorderStyle);
                }

                //return;
                #endregion
            }
            else
            {
                // The button cell is enabled, so let the base class 
                // handle the painting.
                base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                    elementState, value, formattedValue, errorText,
                    cellStyle, advancedBorderStyle, paintParts);
            }
        }
    }
}
