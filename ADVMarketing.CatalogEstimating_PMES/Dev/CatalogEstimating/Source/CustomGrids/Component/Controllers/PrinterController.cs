using System;
using System.Collections.Generic;
using System.Text;

using CatalogEstimating.CustomGrids.Component.Editors;
using System.Drawing;

namespace CatalogEstimating.CustomGrids.Component.Controllers
{
    public class PrinterController : SourceGrid.Cells.Controllers.ControllerBase
    {
        private SourceGrid.Cells.Views.Cell _vwDisabled = new SourceGrid.Cells.Views.Cell();

        public PrinterController()
        {
            _vwDisabled.BackColor = System.Drawing.SystemColors.Control;
            _vwDisabled.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Gray), new DevAge.Drawing.BorderLine(Color.Gray));
        }

        public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnValueChanged(sender, e);

            ComponentGrid cGrid = (ComponentGrid)sender.Grid;

            // Determine the Printer Vendor ID
            LongPair printerKeyValuePair = cGrid[23, sender.Position.Column].Value as LongPair;


            // If a Printer Vendor ID has been selected and the component is NOT vendor supplied,
            // fill the Plate Cost, Digi H&P, Stitcher MR and Press MR dropdown appropriately
            // Also check Calc Print Cost is checked
            if (printerKeyValuePair != null && !((bool)cGrid[15, sender.Position.Column].Value)
                && ((bool)cGrid[24, sender.Position.Column].Value))
            {
                cGrid[27, sender.Position.Column].Editor = new PrinterRateEditor(cGrid.EstimateDS, printerKeyValuePair.Value, 8); // Plate Cost
                cGrid[27, sender.Position.Column].Value = ((PrinterRateEditor)cGrid[27, sender.Position.Column].Editor).DefaultPrinterRate;
                cGrid[27, sender.Position.Column].View = SourceGrid.Cells.Views.ComboBox.Default;

                cGrid[31, sender.Position.Column].Editor = new PrinterRateEditor(cGrid.EstimateDS, printerKeyValuePair.Value, 5); // Digi H&P
                cGrid[31, sender.Position.Column].Value = ((PrinterRateEditor)cGrid[31, sender.Position.Column].Editor).DefaultPrinterRate;
                cGrid[31, sender.Position.Column].View = SourceGrid.Cells.Views.ComboBox.Default;

                cGrid[32, sender.Position.Column].Editor = new PrinterRateEditor(cGrid.EstimateDS, printerKeyValuePair.Value, 4); // Stitcher MR
                cGrid[32, sender.Position.Column].Value = ((PrinterRateEditor)cGrid[32, sender.Position.Column].Editor).DefaultPrinterRate;
                cGrid[32, sender.Position.Column].View = SourceGrid.Cells.Views.ComboBox.Default;

                cGrid[33, sender.Position.Column].View = SourceGrid.Cells.Views.Cell.Default;

                cGrid[34, sender.Position.Column].Editor = new PrinterRateEditor(cGrid.EstimateDS, printerKeyValuePair.Value, 10); // Press MR
                cGrid[34, sender.Position.Column].Value = ((PrinterRateEditor)cGrid[34, sender.Position.Column].Editor).DefaultPrinterRate;
                cGrid[34, sender.Position.Column].View = SourceGrid.Cells.Views.ComboBox.Default;

                cGrid[35, sender.Position.Column].View = SourceGrid.Cells.Views.Cell.Default;
            }
            else
            {
                cGrid[27, sender.Position.Column].Editor = null;
                cGrid[27, sender.Position.Column].Value = null;
                cGrid[27, sender.Position.Column].View = _vwDisabled;

                cGrid[31, sender.Position.Column].Editor = null;
                cGrid[31, sender.Position.Column].Value = null;
                cGrid[31, sender.Position.Column].View = _vwDisabled;

                cGrid[32, sender.Position.Column].Editor = null;
                cGrid[32, sender.Position.Column].Value = null;
                cGrid[32, sender.Position.Column].View = _vwDisabled;

                cGrid[33, sender.Position.Column].Value = null;
                cGrid[33, sender.Position.Column].View = _vwDisabled;

                cGrid[34, sender.Position.Column].Editor = null;
                cGrid[34, sender.Position.Column].Value = null;
                cGrid[34, sender.Position.Column].View = _vwDisabled;

                cGrid[35, sender.Position.Column].Value = null;
                cGrid[35, sender.Position.Column].View = _vwDisabled;
            }
        }
    }
}
