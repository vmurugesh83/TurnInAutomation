using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CatalogEstimating.CustomGrids.Component
{
    public enum CheckBoxToggleType
    {
        NoChange = 0x0,
        DisableOnCheck = 0x1,
        DisableOnUnCheck = 0x2,
        EnableOnCheck = 0x4,
        EnableOnUnCheck = 0x8
    }

    public class CheckBoxToggle : SourceGrid.Cells.Controllers.ControllerBase
    {
        private List<CheckBoxToggleType> _cellToggles;

        private SourceGrid.Cells.Views.Cell _disabledCell = new SourceGrid.Cells.Views.Cell();
        private SourceGrid.Cells.Views.Cell _disabledNumeric = new SourceGrid.Cells.Views.Cell();
        private SourceGrid.Cells.Views.Cell _enabledCell = new SourceGrid.Cells.Views.Cell();
        private SourceGrid.Cells.Views.Cell _enabledNumeric = new SourceGrid.Cells.Views.Cell();
        private SourceGrid.Cells.Views.CheckBox _disabledCheckBox =     new SourceGrid.Cells.Views.CheckBox();
        private SourceGrid.Cells.Views.CheckBox _enabledCheckBox = new SourceGrid.Cells.Views.CheckBox();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellToggles">List of values to determine how cells react to a value changed event.</param>
        public CheckBoxToggle(List<CheckBoxToggleType> cellToggles)
        {
            _cellToggles = cellToggles;

            _disabledCell.BackColor = SystemColors.Control;
            _disabledCell.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Gray), new DevAge.Drawing.BorderLine(Color.Gray));

            _disabledNumeric.BackColor = SystemColors.Control;
            _disabledNumeric.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Gray), new DevAge.Drawing.BorderLine(Color.Gray));
            _disabledNumeric.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

            _enabledCell.BackColor = SystemColors.Window;

            _enabledNumeric.BackColor = SystemColors.Window;
            _enabledNumeric.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

            _disabledCheckBox.BackColor = SystemColors.Control;
            _disabledCheckBox.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Gray), new DevAge.Drawing.BorderLine(Color.Gray));

            _enabledCheckBox.BackColor = SystemColors.Window;
        }

        public override void OnKeyPress(SourceGrid.CellContext sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            base.OnKeyPress(sender, e);

            if ((System.Windows.Forms.Keys) e.KeyChar == System.Windows.Forms.Keys.Space)
            {
                if (sender.Cell.Editor != null && sender.Cell.Editor.EnableEdit == true && sender.Cell.FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) == null)
                   UserChangedValue(sender, e);
            }
        }
        public override void OnClick(SourceGrid.CellContext sender, EventArgs e)
        {            
            base.OnClick(sender, e);

            if (sender.Cell.Editor != null && sender.Cell.Editor.EnableEdit == true && sender.Cell.FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) == null)
                UserChangedValue(sender, e);
        }

        private void UserChangedValue(SourceGrid.CellContext sender, EventArgs e)
        {
            ComponentGrid cGrid = (ComponentGrid)sender.Grid;

            for (int i = 1; i <= _cellToggles.Count; ++i)
            {
                CheckBoxToggleType curToggle = _cellToggles[i - 1];
                SourceGrid.Cells.ICell childCell = cGrid[sender.Position.Row + i, sender.Position.Column];

                // Enable the cell
                if ((curToggle & CheckBoxToggleType.EnableOnCheck) == CheckBoxToggleType.EnableOnCheck && ((bool)sender.Value)
                    || (curToggle & CheckBoxToggleType.EnableOnUnCheck) == CheckBoxToggleType.EnableOnUnCheck && !((bool)sender.Value))
                {
                    if (childCell.Editor != null)
                        childCell.Editor.EnableEdit = true;

                    if (childCell.FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) != null)
                        childCell.RemoveController(childCell.FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)));
                    if (childCell is SourceGrid.Cells.CheckBox)
                        childCell.View = _enabledCheckBox;
                    else if (childCell.Editor is SourceGrid.Cells.Editors.TextBoxNumeric || childCell.Editor is SourceGrid.Cells.Editors.NumericUpDown)
                        childCell.View = _enabledNumeric;
                    else if (childCell.Editor is SourceGrid.Cells.Editors.ComboBox)
                        childCell.View = SourceGrid.Cells.Views.ComboBox.Default;
                    else
                        childCell.View = _enabledCell;
                }

                // Disable the Cell
                if ((curToggle & CheckBoxToggleType.DisableOnCheck) == CheckBoxToggleType.DisableOnCheck && ((bool)sender.Value)
                    || (curToggle & CheckBoxToggleType.DisableOnUnCheck) == CheckBoxToggleType.DisableOnUnCheck && !((bool)sender.Value))
                {
                    if (childCell.Editor != null)
                        childCell.Editor.EnableEdit = false;

                    if (childCell.FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) == null)
                        childCell.AddController(new SourceGrid.Cells.Controllers.Unselectable());

                    if (childCell is SourceGrid.Cells.CheckBox)
                        childCell.View = _disabledCheckBox;
                    else if (childCell.Editor is SourceGrid.Cells.Editors.TextBoxNumeric || childCell.Editor is SourceGrid.Cells.Editors.NumericUpDown)
                    {
                        childCell.View = _disabledNumeric;
                        childCell.Value = null;
                    }
                    else
                    {
                        childCell.View = _disabledCell;
                        childCell.Value = null;
                    }
                }
            }

            // Now loop through the child cells.
            for (int i = 1; i <= _cellToggles.Count; ++i)
            {
                CheckBoxToggleType curToggle = _cellToggles[i - 1];

                // If a child cell contains a CheckBoxToggle controller fire the onclick event
                if ((curToggle & CheckBoxToggleType.EnableOnCheck) == CheckBoxToggleType.EnableOnCheck && ((bool)sender.Value)
                    || (curToggle & CheckBoxToggleType.EnableOnUnCheck) == CheckBoxToggleType.EnableOnUnCheck && !((bool)sender.Value))
                {
                    SourceGrid.Cells.ICell childCell = cGrid[sender.Position.Row + i, sender.Position.Column];
                    CheckBoxToggle childCheckBoxToggle = (CheckBoxToggle)childCell.FindController(typeof(CheckBoxToggle));
                    if (childCheckBoxToggle != null)
                        childCheckBoxToggle.OnClick(new SourceGrid.CellContext(cGrid, new SourceGrid.Position(sender.Position.Row + i, sender.Position.Column)), e);
                }

                // If a checkbox cell has been disabled, uncheck it
                if ((curToggle & CheckBoxToggleType.DisableOnCheck) == CheckBoxToggleType.DisableOnCheck && ((bool)sender.Value)
                    || (curToggle & CheckBoxToggleType.DisableOnUnCheck) == CheckBoxToggleType.DisableOnUnCheck && !((bool)sender.Value))
                {
                    SourceGrid.Cells.CheckBox childCheckboxCell = cGrid[sender.Position.Row + i, sender.Position.Column] as SourceGrid.Cells.CheckBox;
                    if (childCheckboxCell != null)
                        childCheckboxCell.Checked = false;
                }
            }
        }
    }
}
