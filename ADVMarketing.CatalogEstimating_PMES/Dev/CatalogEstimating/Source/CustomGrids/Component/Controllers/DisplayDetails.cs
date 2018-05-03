using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace CatalogEstimating.CustomGrids.Component
{
    public class WorkCompleteEventArgs : System.EventArgs
    {
        private string _value;

        public WorkCompleteEventArgs(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }
    }

    public delegate void WorkComplete(object sender, WorkCompleteEventArgs e);

    public class DisplayDetails : SourceGrid.Cells.Controllers.ControllerBase
    {
        private bool _readOnly = true;
        private frmDetailComments _DetailCommentsForm = null;
        private string _windowCaption = String.Empty;

        private ComponentGrid _grid = null;
        private int _colIndex = -1;
        private int _rowIndex = -1;

        public DisplayDetails(string windowCaption)
        {
            _windowCaption = windowCaption;
        }
        
        public override void OnDoubleClick(SourceGrid.CellContext sender, EventArgs e)
        {
            _grid = (ComponentGrid) sender.Grid;
            _colIndex = sender.Position.Column;
            _rowIndex = sender.Position.Row;
            
            sender.EndEdit(false);

            base.OnDoubleClick(sender, e);

            string initialValue = String.Empty;
            if (sender.Value != null)
                initialValue = sender.Value.ToString();

            _DetailCommentsForm = new frmDetailComments(_readOnly, _windowCaption, initialValue);
            _DetailCommentsForm.WorkComplete += new WorkComplete(DetailWorkComplete);
            _DetailCommentsForm.ShowDialog();
            _DetailCommentsForm.WorkComplete -= DetailWorkComplete;
            _DetailCommentsForm = null;
        }

        public void DetailWorkComplete(object sender, WorkCompleteEventArgs e)
        {
            if (e.Value == String.Empty)
                _grid[_rowIndex, _colIndex].Value = null;
            else
                _grid[_rowIndex, _colIndex].Value = e.Value;
        }

        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }
    }
}
