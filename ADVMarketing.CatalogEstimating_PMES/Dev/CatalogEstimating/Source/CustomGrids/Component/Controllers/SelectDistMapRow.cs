using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CatalogEstimating.CustomGrids.Component.Controllers
{
    public class SelectDistMapRow : SourceGrid.Cells.Controllers.ControllerBase
    {
        public readonly static SelectDistMapRow Default = new SelectDistMapRow();

        public override void OnMouseUp(SourceGrid.CellContext sender, System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(sender, e);

            DistMappingGrid dmGrid = (DistMappingGrid)sender.Grid;
            dmGrid.OnRowClicked(sender, e);
        }
    }
}
