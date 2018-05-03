using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace CatalogEstimating.CustomGrids
{
    public class RotatedText : DevAge.Drawing.VisualElements.TextGDI
    {
        public RotatedText(float angle)
        {
            Angle = angle;
        }

        public float Angle = 0;

        protected override void OnDraw(DevAge.Drawing.GraphicsCache graphics, RectangleF area)
        {
            System.Drawing.Drawing2D.GraphicsState state = graphics.Graphics.Save();
            try
            {
                float width2 = area.Width / 2;
                float height2 = area.Height / 2;

                //For a better drawing use the clear type rendering
                graphics.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                //Move the origin to the center of the cell (for a more easy rotation)
                graphics.Graphics.TranslateTransform(area.X + width2, area.Y + height2);

                graphics.Graphics.RotateTransform(Angle);

                StringFormat.Alignment = StringAlignment.Center;
                StringFormat.LineAlignment = StringAlignment.Center;
                graphics.Graphics.DrawString(Value, Font, graphics.BrushsCache.GetBrush(ForeColor), 0, 0, StringFormat);
            }
            finally
            {
                graphics.Graphics.Restore(state);
            }
        }

        //TODO Implement Clone and MeasureContent
        //Here I should also implement MeasureContent (think also for a solution to allow rotated text with any kind of allignment)
    }
}
