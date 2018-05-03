using System;
using System.Collections.Generic;
using System.Text;

using CatalogEstimating.CustomGrids.Component.Editors;
using System.Drawing;

namespace CatalogEstimating.CustomGrids.Component.Controllers
{
    public class PaperRateController : SourceGrid.Cells.Controllers.ControllerBase
    {
        private SourceGrid.Cells.Views.Cell _disabledCell = new SourceGrid.Cells.Views.Cell();

        public PaperRateController()
        {
            _disabledCell.BackColor = SystemColors.Control;
            _disabledCell.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Gray), new DevAge.Drawing.BorderLine(Color.Gray));
        }

        public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnValueChanged(sender, e);

            ComponentGrid cGrid = (ComponentGrid)sender.Grid;

            // Determine the PaperVendorID, PaperWeightID and PaperGradeID
            LongPair vendorKeyValuePair = null;
            if (cGrid[41, sender.Position.Column] != null)
                vendorKeyValuePair = cGrid[41, sender.Position.Column].Value as LongPair;

            IntPair paperWeightKeyValuePair = null;
            if (cGrid[42, sender.Position.Column] != null)
                paperWeightKeyValuePair = cGrid[42, sender.Position.Column].Value as IntPair;

            IntPair paperGradeKeyValuePair = null;
            if (cGrid[43, sender.Position.Column] != null)
                paperGradeKeyValuePair = cGrid[43, sender.Position.Column].Value as IntPair;

            // If a Paper Grade and Paper Weight are selected and the Component is NOT Vendor Supplied, fill the Paper Description appropriately
            if (vendorKeyValuePair != null && paperWeightKeyValuePair != null && paperGradeKeyValuePair != null && !((bool) cGrid[15, sender.Position.Column].Value))
            {
                cGrid[44, sender.Position.Column].Editor = new PaperMapEditor(cGrid.EstimateDS, vendorKeyValuePair.Value, paperWeightKeyValuePair.Value, paperGradeKeyValuePair.Value, cGrid.EstimateDS.est_estimate[0].rundate);
                cGrid[44, sender.Position.Column].View = SourceGrid.Cells.Views.ComboBox.Default;
            }
            else
            {
                cGrid[44, sender.Position.Column].Editor = null;
                cGrid[44, sender.Position.Column].View = _disabledCell;
                cGrid[44, sender.Position.Column].Value = null;
            }
        }
    }
}
