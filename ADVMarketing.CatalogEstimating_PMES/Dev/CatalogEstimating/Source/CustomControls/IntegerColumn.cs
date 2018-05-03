using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.ComponentModel;

namespace CatalogEstimating.CustomControls
{
    [DataGridViewColumnDesignTimeVisible(true)]
    class IntegerColumn : DataGridViewColumn
    {
        public IntegerColumn()
            : base(new IntegerCell())
        {
        }

        /// <summary>
        /// Represents the implicit cell that gets cloned when adding rows to the grid.
        /// </summary>
        [
            Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is an IntegerCell.
                if (value != null && !value.GetType().IsAssignableFrom(typeof(IntegerCell)))
                {
                    throw new InvalidCastException("Must be a IntegerCell");
                }

                base.CellTemplate = value;
            }
        }

        /// <summary>
        /// Replicates the MaximumInputLength property of the IntegerCell cell type.
        /// </summary>
        [
            Category("Data"),
            Description("Indicates the maximum input length for the integer cells."),
            RefreshProperties(RefreshProperties.All)
        ]
        public int MaxInputLength
        {
            get
            {
                if (this.IntegerCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this IntegerColumn does not have a CellTemplate.");
                }
                return this.IntegerCellTemplate.MaxInputLength;
            }
            set
            {
                if (this.IntegerCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this IntegerColumn does not have a CellTemplate.");
                }
                // Update the template cell so that subsequent cloned cells use the new value.
                this.IntegerCellTemplate.MaxInputLength = value;
                if (this.DataGridView != null)
                {
                    // Update all the existing IntegerCell cells in the column accordingly.
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        // Be careful not to unshare rows unnecessarily. 
                        // This could have severe performance repercussions.
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        IntegerCell dataGridViewCell = dataGridViewRow.Cells[this.Index] as IntegerCell;
                        if (dataGridViewCell != null)
                        {
                            // Call the internal SetMaxInputLength method instead of the property to avoid invalidation 
                            // of each cell. The whole column is invalidated later in a single operation for better performance.
                            dataGridViewCell.SetMaxInputLength(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                    // TODO: Call the grid's autosizing methods to autosize the column, rows, column headers / row headers as needed.
                }
            }
        }

        /// <summary>
        /// Replicates the AllowNegative property of the IntegerCell cell type.
        /// </summary>
        [
            Category("Data"),
            DefaultValue(IntegerCell.INTEGERCELL_defaultAllowNegative),
            Description("Indicates whether negative values can be entered.")
        ]
        public bool AllowNegative
        {
            get
            {
                if (this.IntegerCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this IntegerColumn does not have a CellTemplate.");
                }
                return this.IntegerCellTemplate.AllowNegative;
            }
            set
            {
                if (this.IntegerCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this IntegerColumn does not have a CellTemplate.");
                }

                // Update the template cell so that subsequent cloned cells use the new value.
                this.IntegerCellTemplate.AllowNegative = value;
                if (this.DataGridView != null)
                {
                    // Update all the existing IntegerCell cells in the column accordingly.
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        // Be careful not to unshare rows unnecessarily.
                        // This could have severe performance repercussions.
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        IntegerCell dataGridViewCell = dataGridViewRow.Cells[this.Index] as IntegerCell;
                        if (dataGridViewCell != null)
                        {
                            // Call the internal SetAllowNegative method instead of the property to avoid invalidation
                            // of each cell.  The whole column is invalidated later in a single operation for better performance.
                            dataGridViewCell.SetAllowNegative(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                }
            }
        }

        private IntegerCell IntegerCellTemplate
        {
            get { return (IntegerCell)this.CellTemplate; }
        }
    }

    class IntegerCell : DataGridViewTextBoxCell
    {
        internal const bool INTEGERCELL_defaultAllowNegative = true;

        private bool _allowNegative;

        public IntegerCell()
            : base()
        {
            this._allowNegative = INTEGERCELL_defaultAllowNegative;
        }

        /// <summary>
        /// The AllowNegative property replicates the one from the IntegerTextBox control
        /// </summary>
        [
            DefaultValue(INTEGERCELL_defaultAllowNegative)
        ]
        public bool AllowNegative
        {
            get { return this._allowNegative; }
            set
            {
                if (this._allowNegative != value)
                {
                    SetAllowNegative(this.RowIndex, value);
                    OnCommonChange(); // Assure that the cell or column gets repainted and autosized if needed
                }
            }
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            IntegerTextBox ctl = DataGridView.EditingControl as IntegerEditingControl;
            ctl.AllowNegative = this.AllowNegative;
            ctl.MaxLength = this.MaxInputLength;

            if (this.Value != null)
            {
                ctl.Text = this.Value.ToString();
            }
            else
            {
                ctl.Text = string.Empty;
            }
        }

        /// <summary>
        /// Called when a cell characteristic that affects its rendering and/or preferred size has changed.
        /// This implementation only takes care of repainting the cells. The DataGridView's autosizing methods
        /// also need to be called in cases where some grid elements autosize.
        /// </summary>
        private void OnCommonChange()
        {
            if (this.DataGridView != null && !this.DataGridView.IsDisposed && !this.DataGridView.Disposing)
            {
                if (this.RowIndex == -1)
                {
                    // Invalidate and autosize column
                    this.DataGridView.InvalidateColumn(this.ColumnIndex);

                    // TODO: Add code to autosize the cell's column, the rows, the column headers 
                    // and the row headers depending on their autosize settings.
                    // The DataGridView control does not expose a public method that takes care of this.
                }
                else
                {
                    // The DataGridView control exposes a public method called UpdateCellValue
                    // that invalidates the cell so that it gets repainted and also triggers all
                    // the necessary autosizing: the cell's column and/or row, the column headers
                    // and the row headers are autosized depending on their autosize settings.
                    this.DataGridView.UpdateCellValue(this.ColumnIndex, this.RowIndex);
                }
            }
        }

        public override Type EditType
        {
            get
            {
                // Return the type of the editing control that IntegerCell uses.
                return typeof(IntegerEditingControl);
            }
        }

        /// <summary>
        /// Clones an IntegerCell, copies all custom properties
        /// </summary>
        public override object Clone()
        {
            IntegerCell integerCell = base.Clone() as IntegerCell;
            if (integerCell != null)
            {
                integerCell.MaxInputLength = this.MaxInputLength;
                integerCell.AllowNegative = this.AllowNegative;
            }

            return integerCell;
        }

        public override object DefaultNewRowValue
        {
            get
            {
                // Use an empty string as the default value
                return String.Empty;
            }
        }

        private bool OwnsEditingIntegerControl(int rowIndex)
        {
            if (rowIndex == -1 || this.DataGridView == null)
                return false;

            IntegerEditingControl integerEditingControl = this.DataGridView.EditingControl as IntegerEditingControl;
            return integerEditingControl != null && rowIndex == ((IDataGridViewEditingControl)integerEditingControl).EditingControlRowIndex;
        }

        private IntegerEditingControl EditingIntegerControl
        {
            get { return this.DataGridView.EditingControl as IntegerEditingControl; }
        }

        internal void SetMaxInputLength(int rowIndex, int value)
        {
            this.MaxInputLength = value;
            if (OwnsEditingIntegerControl(rowIndex))
                this.EditingIntegerControl.MaxLength = value;
        }

        internal void SetAllowNegative(int rowIndex, bool value)
        {
            this._allowNegative = value;
            if (OwnsEditingIntegerControl(RowIndex))
                this.EditingIntegerControl.AllowNegative = value;
        }
    }

    class IntegerEditingControl : IntegerTextBox, IDataGridViewEditingControl
    {
        DataGridView _dataGridView;
        private bool _valueChanged = false;
        private int _rowIndex;

        public IntegerEditingControl()
        {
        }

        #region IDataGridViewEditingControl Members

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
        }

        public DataGridView EditingControlDataGridView
        {
            get
            {
                return _dataGridView;
            }
            set
            {
                _dataGridView = value;
            }
        }

        public object EditingControlFormattedValue
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value.ToString();
            }
        }

        public int EditingControlRowIndex
        {
            get
            {
                return _rowIndex;
            }
            set
            {
                _rowIndex = value;
            }
        }

        public bool EditingControlValueChanged
        {
            get
            {
                return _valueChanged;
            }
            set
            {
                _valueChanged = value;
            }
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            return false;
        }

        public Cursor EditingPanelCursor
        {
            get { return base.Cursor; }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return this.Text;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
        }

        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        #endregion

        protected override void OnTextChanged(EventArgs e)
        {
            _valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnTextChanged(e);
        }
    }
}
