using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CatalogEstimating.CustomGrids.Component.Controllers
{

    public class SelectColumn : SourceGrid.Cells.Controllers.ControllerBase
    {
        public readonly static SelectColumn Default = new SelectColumn();

        public override void OnMouseUp(SourceGrid.CellContext sender, System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(sender, e);

            ComponentGrid cGrid = (ComponentGrid) sender.Grid;
            cGrid.OnColumnClicked(sender, e);
        }
    }
}
