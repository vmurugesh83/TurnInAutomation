using System;
using System.Collections.Generic;
using System.Text;

using CatalogEstimating.CustomGrids.Component.Editors;
using System.Drawing;

namespace CatalogEstimating.CustomGrids.Component.Controllers
{
    public class AssemblyRateController : SourceGrid.Cells.Controllers.ControllerBase
    {
        private SourceGrid.Cells.Views.Cell _disabledCell = new SourceGrid.Cells.Views.Cell();

        public AssemblyRateController()
        {
            _disabledCell.BackColor = SystemColors.Control;
            _disabledCell.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Gray), new DevAge.Drawing.BorderLine(Color.Gray));
        }

        public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnValueChanged(sender, e);

            ComponentGrid cGrid = (ComponentGrid)sender.Grid;

            // Deterimine the Component Type and Printer Vendor ID
            IntPair componentTypeKeyValuePair = cGrid[7, sender.Position.Column].Value as IntPair;
            LongPair assemblyKeyValuePair = cGrid[57, sender.Position.Column].Value as LongPair;


            // If a Component Type and Printer Vendor ID are selected and the component is NOT Vendor Supplied,
            // fill the stitch-in, blow-in and onsert dropdowns appropriately
            // Also check that Calc Printer Cost is checked
            if (componentTypeKeyValuePair != null && assemblyKeyValuePair != null)
            {
                switch (componentTypeKeyValuePair.Value)
                {
                    // Onsert
                    case 2:
                        if (cGrid[58, sender.Position.Column].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) == null)
                            cGrid[58, sender.Position.Column].AddController(new SourceGrid.Cells.Controllers.Unselectable());
                        cGrid[58, sender.Position.Column].Editor = null;
                        cGrid[58, sender.Position.Column].View = _disabledCell;
                        cGrid[58, sender.Position.Column].Value = null;

                        if (cGrid[59, sender.Position.Column].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) == null)
                            cGrid[59, sender.Position.Column].AddController(new SourceGrid.Cells.Controllers.Unselectable());
                        cGrid[59, sender.Position.Column].Editor = null;
                        cGrid[59, sender.Position.Column].View = _disabledCell;
                        cGrid[59, sender.Position.Column].Value = null;
                        
                        PrinterRateEditor edOnsert = new PrinterRateEditor(cGrid.EstimateDS, assemblyKeyValuePair.Value, 9);
                        edOnsert.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey | SourceGrid.EditableMode.SingleClick;
                        cGrid[60, sender.Position.Column].Editor = edOnsert;
                        cGrid[60, sender.Position.Column].Value = edOnsert.DefaultPrinterRate;
                        cGrid[60, sender.Position.Column].View = SourceGrid.Cells.Views.ComboBox.Default;
                        if (cGrid[60, sender.Position.Column].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) != null)
                            cGrid[60, sender.Position.Column].RemoveController(cGrid[60, sender.Position.Column].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)));
                        break;
                    // Stitch-In
                    case 3:
                        PrinterRateEditor edStitchIn = new PrinterRateEditor(cGrid.EstimateDS, assemblyKeyValuePair.Value, 1);
                        edStitchIn.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey | SourceGrid.EditableMode.SingleClick;
                        cGrid[58, sender.Position.Column].Editor = edStitchIn;
                        cGrid[58, sender.Position.Column].Value = edStitchIn.DefaultPrinterRate;
                        cGrid[58, sender.Position.Column].View = SourceGrid.Cells.Views.ComboBox.Default;
                        if (cGrid[58, sender.Position.Column].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) != null)
                            cGrid[58, sender.Position.Column].RemoveController(cGrid[58, sender.Position.Column].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)));

                        if (cGrid[59, sender.Position.Column].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) == null)
                            cGrid[59, sender.Position.Column].AddController(new SourceGrid.Cells.Controllers.Unselectable());
                        cGrid[59, sender.Position.Column].Editor = null;
                        cGrid[59, sender.Position.Column].View = _disabledCell;
                        cGrid[59, sender.Position.Column].Value = null;

                        if (cGrid[60, sender.Position.Column].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) == null)
                            cGrid[60, sender.Position.Column].AddController(new SourceGrid.Cells.Controllers.Unselectable());
                        cGrid[60, sender.Position.Column].Editor = null;
                        cGrid[60, sender.Position.Column].View = _disabledCell;
                        cGrid[60, sender.Position.Column].Value = null;
                        break;
                    // Blow-In
                    case 4:
                        if (cGrid[58, sender.Position.Column].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) == null)
                            cGrid[58, sender.Position.Column].AddController(new SourceGrid.Cells.Controllers.Unselectable());
                        cGrid[58, sender.Position.Column].Editor = null;
                        cGrid[58, sender.Position.Column].View = _disabledCell;
                        cGrid[58, sender.Position.Column].Value = null;

                        PrinterRateEditor edBlowIn = new PrinterRateEditor(cGrid.EstimateDS, assemblyKeyValuePair.Value, 2);
                        edBlowIn.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey | SourceGrid.EditableMode.SingleClick;
                        cGrid[59, sender.Position.Column].Editor = edBlowIn;
                        cGrid[59, sender.Position.Column].Value = edBlowIn.DefaultPrinterRate;
                        cGrid[59, sender.Position.Column].View = SourceGrid.Cells.Views.ComboBox.Default;
                        if (cGrid[59, sender.Position.Column].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) != null)
                            cGrid[59, sender.Position.Column].RemoveController(cGrid[59, sender.Position.Column].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)));

                        if (cGrid[60, sender.Position.Column].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) == null)
                            cGrid[60, sender.Position.Column].AddController(new SourceGrid.Cells.Controllers.Unselectable());
                        cGrid[60, sender.Position.Column].Editor = null;
                        cGrid[60, sender.Position.Column].View = _disabledCell;
                        cGrid[60, sender.Position.Column].Value = null;
                        break;
                    default:
                        if (cGrid[58, sender.Position.Column].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) == null)
                            cGrid[58, sender.Position.Column].AddController(new SourceGrid.Cells.Controllers.Unselectable());
                        cGrid[58, sender.Position.Column].Editor = null;
                        cGrid[58, sender.Position.Column].View = _disabledCell;
                        cGrid[58, sender.Position.Column].Value = null;
                        if (cGrid[59, sender.Position.Column].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) == null)
                            cGrid[59, sender.Position.Column].AddController(new SourceGrid.Cells.Controllers.Unselectable());
                        cGrid[59, sender.Position.Column].Editor = null;
                        cGrid[59, sender.Position.Column].View = _disabledCell;
                        cGrid[59, sender.Position.Column].Value = null;
                        if (cGrid[60, sender.Position.Column].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) == null)
                            cGrid[60, sender.Position.Column].AddController(new SourceGrid.Cells.Controllers.Unselectable());
                        cGrid[60, sender.Position.Column].Editor = null;
                        cGrid[60, sender.Position.Column].View = _disabledCell;
                        cGrid[60, sender.Position.Column].Value = null;
                        break;
                }
            }
            // If a ComponentType and Vendor ID have not been selected, clear the dropdowns.
            else
            {
                if (cGrid[58, sender.Position.Column].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) == null)
                    cGrid[58, sender.Position.Column].AddController(new SourceGrid.Cells.Controllers.Unselectable());
                cGrid[58, sender.Position.Column].Editor = null;
                cGrid[58, sender.Position.Column].View = _disabledCell;
                cGrid[58, sender.Position.Column].Value = null;
                if (cGrid[59, sender.Position.Column].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) == null)
                    cGrid[59, sender.Position.Column].AddController(new SourceGrid.Cells.Controllers.Unselectable());
                cGrid[59, sender.Position.Column].Editor = null;
                cGrid[59, sender.Position.Column].View = _disabledCell;
                cGrid[59, sender.Position.Column].Value = null;
                if (cGrid[60, sender.Position.Column].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) == null)
                    cGrid[60, sender.Position.Column].AddController(new SourceGrid.Cells.Controllers.Unselectable());
                cGrid[60, sender.Position.Column].Editor = null;
                cGrid[60, sender.Position.Column].View = _disabledCell;
                cGrid[60, sender.Position.Column].Value = null;
            }
        }
    }
}
