using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogEstimating.CustomGrids.Controllers
{
    public delegate void CellValueChanged(SourceGrid.CellContext sender, EventArgs e);

    public class ValueChangedController : SourceGrid.Cells.Controllers.ControllerBase
    {
        private object _previousValue = null;

        public event CellValueChanged CellValueChanged;

        public ValueChangedController()
        {
        }

        public ValueChangedController(object initialValue)
            : this()
        {
            _previousValue = initialValue;
        }

        public object InitialValue
        {
            set { _previousValue = value; }
        }

        public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnValueChanged(sender, e);

            if ((!(_previousValue == null && sender.Value == null)
                && ((_previousValue != null && sender.Value == null) || (_previousValue == null && sender.Value != null) || !_previousValue.Equals(sender.Value))))
                if (CellValueChanged != null)
                    this.CellValueChanged(sender, e);

            _previousValue = sender.Value;
        }
    }
}
