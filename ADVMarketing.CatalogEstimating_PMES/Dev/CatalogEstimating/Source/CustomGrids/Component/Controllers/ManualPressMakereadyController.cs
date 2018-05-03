using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogEstimating.CustomGrids.Component.Controllers
{
    public class ManualPressMakereadyController : SourceGrid.Cells.Controllers.ControllerBase
    {
        public ManualPressMakereadyController()
        {
        }

        public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnValueChanged(sender, e);

            if (sender.Value != null)
                ((ComponentGrid)sender.Grid)[sender.Position.Row - 1, sender.Position.Column].Value = null;
        }
    }
}