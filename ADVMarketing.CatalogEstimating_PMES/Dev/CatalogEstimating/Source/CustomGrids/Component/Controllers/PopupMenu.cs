using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.Drawing;
using CatalogEstimating.Properties;

namespace CatalogEstimating.CustomGrids.Component
{
    public class PopupMenu : SourceGrid.Cells.Controllers.ControllerBase
    {
        private bool _readOnly = true;
        private ContextMenuStrip _menu = new ContextMenuStrip();

        private int _colIndex = -1;
        private int _rowIndex = -1;

        public PopupMenu(bool readOnly)
        {
            _readOnly = readOnly;

            _menu.Items.Add("Add New Component", null, new EventHandler(AddNewComponent_Click));
            _menu.Items.Add("Delete Component", Resources.Delete, new EventHandler(DeleteComponent_Click));
            _menu.Items.Add("Cut Component", Resources.Cut, new EventHandler(CutComponent_Click));
            _menu.Items.Add("Copy Component", Resources.Copy, new EventHandler(CopyComponent_Click));
            _menu.Items.Add("Paste Component", Resources.Paste, new EventHandler(PasteComponent_Click));
            _menu.Items[1].ImageTransparentColor = Color.Black;
            _menu.Items[2].ImageTransparentColor = Color.Black;
            _menu.Items[3].ImageTransparentColor = Color.Black;
            _menu.Items[4].ImageTransparentColor = Color.Black;
        }

        public override void OnMouseUp(SourceGrid.CellContext sender, MouseEventArgs e)
        {
            ComponentGrid grid = ((ComponentGrid)sender.Grid);

            _colIndex = sender.Position.Column;
            _rowIndex = sender.Position.Row;

            base.OnMouseUp(sender, e);
            if (e.Button == MouseButtons.Right)
            {
                bool bAddComponent = true;
                bool bDeleteComponent = true;
                bool bCutComponent = true;
                bool bCopyComponent = true;
                bool bPasteComponent = true;

                if (_colIndex == 1)
                {
                    bDeleteComponent = false;
                    bCutComponent = false;
                }

                if (System.Windows.Forms.Clipboard.GetDataObject().GetDataPresent(typeof(List<CopyComponent>)))
                {
                    List<CopyComponent> componentList = (List<CopyComponent>)System.Windows.Forms.Clipboard.GetDataObject().GetData(typeof(List<CopyComponent>));
                    if (componentList.Count != grid.SelectedComponentCount)
                        bPasteComponent = false;
                    else if (grid.Selection.IsSelectedColumn(1) && componentList[0].ComponentTypeID != 1)
                        bPasteComponent = false;
                    else if (!grid.Selection.IsSelectedColumn(1) && componentList[0].ComponentTypeID == 1)
                        bPasteComponent = false;
                }
                else
                    bPasteComponent = false;

                if (_readOnly)
                {
                    bAddComponent = false;
                    bDeleteComponent = false;
                    bCutComponent = false;
                    bPasteComponent = false;
                }

                _menu.Items[0].Enabled = bAddComponent;
                _menu.Items[1].Enabled = bDeleteComponent;
                _menu.Items[2].Enabled = bCutComponent;
                _menu.Items[3].Enabled = bCopyComponent;
                _menu.Items[4].Enabled = bPasteComponent;

                _menu.Show(sender.Grid, new Point(e.X, e.Y));
            }
        }

        private void AddNewComponent_Click(object sender, EventArgs e)
        {
            ComponentGrid grid = (ComponentGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;
            grid.AddComponent();
        }

        private void DeleteComponent_Click(object sender, EventArgs e)
        {
            ComponentGrid grid = (ComponentGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;
            grid.DeleteColumn(_colIndex);
        }

        private void CutComponent_Click(object sender, EventArgs e)
        {
            ComponentGrid cGrid = (ComponentGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;
            cGrid.CutSelectedComponents();
        }

        private void CopyComponent_Click(object sender, EventArgs e)
        {
            ComponentGrid cGrid = (ComponentGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;
            cGrid.CopySelectedComponentsToClipboard();
        }

        private void PasteComponent_Click(object sender, EventArgs e)
        {
            ComponentGrid cGrid = (ComponentGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;
            cGrid.PasteComponents();
        }

    }
}
