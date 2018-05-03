using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SourceGrid;

namespace CatalogEstimating.UserControls.Estimate
{
    public partial class ucpComponents : CatalogEstimating.UserControlPanel
    {
        private bool _readOnly = true;

        private Datasets.Estimates _dsEstimate;

        #region Construction
        public ucpComponents(Datasets.Estimates dsEstimate, bool readOnly)
        {
            InitializeComponent();
            this._gridComponents.BackColor = Color.Transparent;
            this._gridComponents.CellValueChanged += new CatalogEstimating.CustomGrids.Controllers.CellValueChanged(_gridComponents_OnCellValueChanged);
            this._gridComponents.ComponentAdded += new EventHandler(_gridComponents_OnComponentAdded);
            this._gridComponents.ComponentRemoving += new CancelEventHandler(_gridComponents_OnComponentRemoving);
            this._gridComponents.ComponentRemoved += new EventHandler(_gridComponents_OnComponentRemoved);
            this._gridComponents.ComponentBeginOverwrite += new System.ComponentModel.CancelEventHandler(_gridComponents_OnComponentBeginOverwrite);

            Name = "Components";

            _dsEstimate = dsEstimate;
            _readOnly = readOnly;

            _btnCut.Enabled = !_readOnly;
            _btnPaste.Enabled = !_readOnly;
            _btnDelete.Enabled = !_readOnly;
            _btnCopy.Enabled = !_readOnly;
        }
        #endregion

        #region Overrides

        public override void Reload()
        {
            base.Reload();
            this.IsLoading = true;
            
            _gridComponents.EstimateDS = _dsEstimate;
            _gridComponents.Initialize(_readOnly);
            this.IsLoading = false;
        }

        public override void OnLeaving(CancelEventArgs e)
        {
            if (this.ValidateChildren())
            {
                _gridComponents.WriteToDataset();

                base.OnLeaving(e);
            }
            else
                e.Cancel = true;
        }

        public override void PreSave(CancelEventArgs e)
        {
            if (this.ValidateChildren())
            {
                _gridComponents.WriteToDataset();

                base.PreSave(e);
            }
            else
                e.Cancel = true;
        }

        public override ToolStrip Toolbar
        {
            get
            {
                return toolStrip1;
            }
        }

        public override void Export(ref ExcelWriter writer)
        {
            for ( int iRow = 0; iRow < _gridComponents.Rows.Count; iRow++ )
            {
                string[] rowParams = new string[_gridComponents.Columns.Count];
                for ( int iCol = 0; iCol < _gridComponents.Columns.Count; iCol++ )
                {
                    if ( _gridComponents[iRow, iCol] != null )
                        rowParams[iCol] = _gridComponents[iRow, iCol].DisplayText;
                }
                writer.WriteLine( rowParams );
            }
        }

        #endregion

        #region Event Handlers
        private void componentGrid1_Validating(object sender, CancelEventArgs e)
        {
            if (!_gridComponents.Validate())
                e.Cancel = true;
        }

        private void _gridComponents_OnCellValueChanged(SourceGrid.CellContext cellContext, EventArgs e)
        {
            if (!IsLoading)
                this.Dirty = true;
        }

        private void _gridComponents_OnComponentAdded(object sender, EventArgs e)
        {
            if (!IsLoading)
                this.Dirty = true;
        }

        private void _gridComponents_OnComponentRemoving(object sender, CancelEventArgs e)
        {
            long componentID = (long)sender;

            if (_dsEstimate.est_packagecomponentmapping.Select("est_component_id = " + componentID.ToString()).Length > 0)
            {
                e.Cancel = true;
                MessageBox.Show("The component is being referenced by a package.", "Cannot Delete Component", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void _gridComponents_OnComponentRemoved(object sender, EventArgs e)
        {
            if (!IsLoading)
                this.Dirty = true;
        }

        private void _gridComponents_OnComponentBeginOverwrite(object sender, CancelEventArgs e)
        {
            DialogResult result = MessageBox.Show("You are about to overwrite a component.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
                e.Cancel = true;
        }

        private void _btnCut_Click(object sender, EventArgs e)
        {
            _gridComponents.CutSelectedComponents();
        }

        private void _btnCopy_Click(object sender, EventArgs e)
        {
            _gridComponents.CopySelectedComponents();
        }

        private void _btnPaste_Click(object sender, EventArgs e)
        {
            _gridComponents.PasteComponents();
        }

        private void _btnDelete_Click(object sender, EventArgs e)
        {
            _gridComponents.DeleteSelectedComponents();
        }

        private void _gridComponents_HScrollPositionChanged(object sender, SourceGrid.ScrollPositionChangedEventArgs e)
        {
            Position pos = _gridComponents.Selection.ActivePosition;
            if ((pos.Row > -1) && (pos.Column > -1))
            {
                CellContext focusCellContext = new CellContext(_gridComponents, _gridComponents.Selection.ActivePosition);
                if (focusCellContext.IsEditing())
                    focusCellContext.EndEdit(false);
            }
        }

        private void _gridComponents_VScrollPositionChanged(object sender, SourceGrid.ScrollPositionChangedEventArgs e)
        {
            Position pos = _gridComponents.Selection.ActivePosition;
            if ((pos.Row > -1) && (pos.Column > -1))
            {
                CellContext focusCellContext = new CellContext(_gridComponents, _gridComponents.Selection.ActivePosition);
                if (focusCellContext.IsEditing())
                    focusCellContext.EndEdit(false);
            }
        }
        #endregion
    }
}