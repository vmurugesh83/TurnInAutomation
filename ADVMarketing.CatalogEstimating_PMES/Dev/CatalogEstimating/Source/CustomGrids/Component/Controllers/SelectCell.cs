using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogEstimating.CustomGrids.Component.Controllers
{
    public class SelectCell : SourceGrid.Cells.Controllers.ControllerBase
    {
        public readonly static SelectCell Default = new SelectCell();

        public override void  OnMouseUp(SourceGrid.CellContext sender, System.Windows.Forms.MouseEventArgs e)
        {
 	         base.OnMouseUp(sender, e);
            ComponentGrid cGrid = (ComponentGrid) sender.Grid;
            cGrid.OnCellClicked(sender, e);
        }
    }
}
