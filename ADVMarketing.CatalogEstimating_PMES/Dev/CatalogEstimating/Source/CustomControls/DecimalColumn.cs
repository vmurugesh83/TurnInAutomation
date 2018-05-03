using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.ComponentModel;

namespace CatalogEstimating.CustomControls
{
    [DataGridViewColumnDesignTimeVisible(true)]
    class DecimalColumn : DataGridViewColumn
    {
        public DecimalColumn()
            : base(new DecimalCell())
        {
        }

        public int MaxInputLength
        {
            get
            {
                if (this.DecimalCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this IntegerColumn does not have a CellTemplate.");
                }
                return this.DecimalCellTemplate.MaxInputLength;
            }
            set
            {
                if (this.DecimalCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this IntegerColumn does not have a CellTemplate.");
                }
                // Update the template cell so that subsequent cloned cells use the new value.
                this.DecimalCellTemplate.MaxInputLength = value;
                if (this.DataGridView != null)
                {
                    // Update all the existing DecimalCell cells in the column accordingly.
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        // Be careful not to unshare rows unnecessarily. 
                        // This could have severe performance repercussions.
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        DecimalCell dataGridViewCell = dataGridViewRow.Cells[this.Index] as DecimalCell;
                        if (dataGridViewCell != null)
                        {
                            // Call the internal SetDecimalPlaces method instead of the property to avoid invalidation 
                            // of each cell. The whole column is invalidated later in a single operation for better performance.
                            dataGridViewCell.SetMaxInputLength(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                    // TODO: Call the grid's autosizing methods to autosize the column, rows, column headers / row headers as needed.
                }
            }
        }

        public bool AllowNegative
        {
            get
            {
                if (this.DecimalCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DecimalColumn does not have a CellTemplate.");
                }
                return this.DecimalCellTemplate.AllowNegative;
            }

            set
            {
                if (this.DecimalCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this IntegerColumn does not have a CellTemplate.");
                }
                // Update the template cell so that subsequent cloned cells use the new value.
                this.DecimalCellTemplate.AllowNegative = value;
                if (this.DataGridView != null)
                {
                    // Update all the existing DecimalCell cells in the column accordingly.
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        // Be careful not to unshare rows unnecessarily.
                        // This could have severe performance repercussions.
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        DecimalCell dataGridViewCell = dataGridViewRow.Cells[this.Index] as DecimalCell;
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

        [
            Category("Data"),
            Description("Indicates the decimal precision for the decimal cells"),
            DefaultValue(DecimalCell.DECIMALCELL_defaultDecimalPrecision)
        ]
        public int DecimalPrecision
        {
            get
            {
                if (this.DecimalCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DecimalColumn does not have a CellTemplate.");
                }
                return this.DecimalCellTemplate.DecimalPrecision;
            }

            set
            {
                if (this.DecimalCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this IntegerColumn does not have a CellTemplate.");
                }
                // Update the template cell so that subsequent cloned cells use the new value.
                this.DecimalCellTemplate.DecimalPrecision = value;
                if (this.DataGridView != null)
                {
                    // Update all the existing DecimalCell cells in the column accordingly.
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        // Be careful not to unshare rows unnecessarily.
                        // This could have severe performance repercussions.
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        DecimalCell dataGridViewCell = dataGridViewRow.Cells[this.Index] as DecimalCell;
                        if (dataGridViewCell != null)
                        {
                            // Call the internal SetAllowNegative method instead of the property to avoid invalidation
                            // of each cell.  The whole column is invalidated later in a single operation for better performance.
                            dataGridViewCell.SetDecimalPrecision(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                }
            }
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is an DecimalCell.
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DecimalCell)))
                {
                    throw new InvalidCastException("Must be a DecimalCell");
                }

                base.CellTemplate = value;
            }
        }

        private DecimalCell DecimalCellTemplate
        {
            get { return (DecimalCell)this.CellTemplate; }
        }
    }

    class DecimalCell : DataGridViewTextBoxCell
    {
        internal const bool DECIMALCELL_defaultAllowNegative = true;
        internal const int DECIMALCELL_defaultDecimalPrecision = 2;

        private bool _allowNegative;
        private int _decimalPrecision;

        public DecimalCell()
            : base()
        {
            this._allowNegative = DECIMALCELL_defaultAllowNegative;
            this._decimalPrecision = DECIMALCELL_defaultDecimalPrecision;
        }

        protected override void OnKeyPress(KeyPressEventArgs e, int rowIndex)
        {
            base.OnKeyPress(e, rowIndex);
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            DecimalEditingControl ctl = DataGridView.EditingControl as DecimalEditingControl;
            ctl.AllowNegative = this.AllowNegative;
            ctl.DecimalPrecision = this.DecimalPrecision;
            ctl.MaxLength = this.MaxInputLength;

            decimal dValue;
            if (Value != null && Decimal.TryParse(Value.ToString(), out dValue))
            {
                if (ctl.DisplayFormatString == null)
                    ctl.Text = Value.ToString();
                else
                    ctl.Text = string.Format( ctl.DisplayFormatString, Convert.ToDecimal( Value ) );
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
                // Return the type of the editing control that DecimalCell uses.
                return typeof(DecimalEditingControl);
            }
        }

        /// <summary>
        /// Clones an DecimalCell, copies all custom properties
        /// </summary>
        public override object Clone()
        {
            DecimalCell decimalCell = base.Clone() as DecimalCell;
            if (decimalCell != null)
            {
                decimalCell.MaxInputLength = this.MaxInputLength;
                decimalCell.AllowNegative = this.AllowNegative;
                decimalCell.DecimalPrecision = this.DecimalPrecision;
            }

            return decimalCell;
        }

        public override object DefaultNewRowValue
        {
            get
            {
                // Use an empty string as the default value
                return String.Empty;
            }
        }

        private bool OwnsEditingDecimalControl(int rowIndex)
        {
            if (rowIndex == -1 || this.DataGridView == null)
                return false;

            DecimalEditingControl decimalEditingControl = this.DataGridView.EditingControl as DecimalEditingControl;
            return decimalEditingControl != null && rowIndex == ((IDataGridViewEditingControl)decimalEditingControl).EditingControlRowIndex;
        }

        private DecimalEditingControl EditingDecimalControl
        {
            get { return this.DataGridView.EditingControl as DecimalEditingControl; }
        }

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

        public int DecimalPrecision
        {
            get { return this._decimalPrecision; }
            set
            {
                if (this._decimalPrecision != value)
                {
                    SetDecimalPrecision(this.RowIndex, value);
                    OnCommonChange(); // Assure that the cell or column gets repainted and autosized if needed
                }
            }
        }

        internal void SetMaxInputLength(int rowIndex, int value)
        {
            this.MaxInputLength = value;
            if (OwnsEditingDecimalControl(rowIndex))
                this.EditingDecimalControl.MaxLength = value;
        }

        internal void SetAllowNegative(int rowIndex, bool value)
        {
            this._allowNegative = value;
            if (OwnsEditingDecimalControl(rowIndex))
                this.EditingDecimalControl.AllowNegative = value;
        }

        internal void SetDecimalPrecision(int rowIndex, int value)
        {
            this._decimalPrecision = value;
            if (OwnsEditingDecimalControl(rowIndex))
                this.EditingDecimalControl.DecimalPrecision = value;
        }
    }

    class DecimalEditingControl : DecimalTextBox, IDataGridViewEditingControl
    {
        DataGridView _dataGridView;
        private bool _valueChanged = false;
        private int _rowIndex;

        public DecimalEditingControl()
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
                return Text;
            }
            set
            {
                decimal dValue;

                if ( value == null )
                    Text = string.Empty;
                else if ( DisplayFormatString == null || !Decimal.TryParse(value.ToString(), out dValue))
                    Text = value.ToString();
                else
                    Text = string.Format( DisplayFormatString, Convert.ToDecimal( value ) );
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
            switch (keyData & Keys.KeyCode)
            {
                case Keys.OemPeriod: case Keys.Decimal:
                    return true;
                default:
                    return false;
            }
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
