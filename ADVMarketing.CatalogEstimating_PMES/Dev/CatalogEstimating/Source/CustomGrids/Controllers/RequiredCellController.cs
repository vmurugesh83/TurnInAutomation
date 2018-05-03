using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogEstimating.CustomGrids.Controllers
{

    public class RequiredCellController : SourceGrid.Cells.Controllers.ControllerBase
    {
        public event CellValueChanged CellValueChanged;

        public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnValueChanged(sender, e);
            if (CellValueChanged != null)
                this.CellValueChanged(sender, e);
        }
    }
}
