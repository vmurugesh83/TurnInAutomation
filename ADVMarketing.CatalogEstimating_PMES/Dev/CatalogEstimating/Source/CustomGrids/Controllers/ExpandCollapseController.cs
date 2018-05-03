using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogEstimating.CustomGrids.Controllers
{
    public class ExpandCollapseController : SourceGrid.Cells.Controllers.ControllerBase
    {
        private bool _isExpanded = true;
        private int _numRows;
        private string _cellExpandedText;
        private string _cellShrunkText;

        public ExpandCollapseController(bool initialExpandedState, int numRows, string cellExpandedText, string cellShrunkText)
        {
            _isExpanded = initialExpandedState;
            _numRows = numRows;
            _cellExpandedText = cellExpandedText;
            _cellShrunkText = cellShrunkText;
        }

        public override void OnClick(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnClick(sender, e);

            _isExpanded = !_isExpanded;

            SourceGrid.Grid mGrid = (SourceGrid.Grid) sender.Grid;

            if (_isExpanded)
                mGrid[sender.Position.Row, sender.Position.Column].Value = _cellExpandedText;
            else
                mGrid[sender.Position.Row, sender.Position.Column].Value = _cellShrunkText;

            for (int i = 1; i < (_numRows + 1); ++i)
            {
                mGrid.Rows[sender.Position.Row + i].Visible = _isExpanded;
            }

            if (_isExpanded)
            {
                for (int i = 1; i < (_numRows + 1); ++i)
                {
                    ExpandCollapseController childExpandCollapse = (ExpandCollapseController)mGrid[sender.Position.Row + i, 0].FindController(typeof(ExpandCollapseController));
                    if (childExpandCollapse != null)
                    {
                        childExpandCollapse.SetChildRowVisibility(new SourceGrid.CellContext(mGrid, new SourceGrid.Position(sender.Position.Row + i, 0)));
                    }
                }
            }
        }

        public void SetChildRowVisibility(SourceGrid.CellContext cellContext)
        {
            SourceGrid.Grid mGrid = (SourceGrid.Grid) cellContext.Grid;

            for (int i = 1; i < (_numRows + 1); ++i)
                mGrid.Rows[cellContext.Position.Row + i].Visible = _isExpanded;

            if (_isExpanded)
            {
                for (int i = 1; i < (_numRows + 1); ++i)
                {
                    ExpandCollapseController childExpandCollapse = (ExpandCollapseController)mGrid[cellContext.Position.Row + i, 0].FindController(typeof(ExpandCollapseController));
                    if (childExpandCollapse != null)
                    {
                        childExpandCollapse.SetChildRowVisibility(new SourceGrid.CellContext(mGrid, new SourceGrid.Position(cellContext.Position.Row + i, 0)));
                    }
                }
            }
        }
    }
}
