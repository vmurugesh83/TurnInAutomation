using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Drawing;
using System.Data;
using CatalogEstimating.Datasets;
using CatalogEstimating.CustomGrids.Controllers;
using CatalogEstimating.CustomGrids.Component.Editors;
using CatalogEstimating.CustomGrids.Component.Controllers;
using SourceGrid;
using SourceGrid.Cells.Editors;

namespace CatalogEstimating.CustomGrids.Component
{
    public partial class DistMappingGrid : SourceGrid.Grid
    {
        public DistMappingGrid()
            : base()
        {
        }

        public void OnRowClicked(SourceGrid.CellContext sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (sender.Position.Column == 0)
            {
                if (this.SelectionMode != GridSelectionMode.Row)
                {
                    this.SelectionMode = GridSelectionMode.Row;
                    this.Selection.EnableMultiSelection = true;

                    this.Selection.ResetSelection(false);
                    this.Selection.Focus(sender.Position, false);
                    this.Selection.SelectRow(sender.Position.Row, true);
                }
            }
            else
            {
                if (this.SelectionMode != GridSelectionMode.Cell)
                {
                    this.SelectionMode = GridSelectionMode.Cell;
                    this.Selection.EnableMultiSelection = false;

                    this.Selection.ResetSelection(false);
                    if (sender.Cell.FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) == null)
                        this.Selection.SelectCell(sender.Position, true);
                }
            }
        }
    }
}
