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
    public class ComponentGrid : SourceGrid.Grid
    {
        #region Private Members
        private const decimal _cMaxWidth = 999;
        private const decimal _cMaxHeight = 999;
        private const decimal _cCurrencyMaxValue = 999999999;
        private const decimal _cMaxRunPounds = 99999999;
        private const int _cMaxMakereadyPounds = 99999;
        private const decimal _cMaxPlateChangePounds = 99999999;
        private const int _cMaxPressStopPounds = 99999;

        private Estimates _dsEstimates;
        private bool _readOnly = true;
        List<Estimates.est_componentRow> _Components = new List<Estimates.est_componentRow>();
        List<bool> _blankColumn = new List<bool>();
        #endregion

        public ComponentGrid()
            : base()
        {
        }

        #region Editors
        TextBox _edDescription = new TextBox(typeof(string));
        TextBox _edComments = new TextBox(typeof(string));
        TextBox _edAdNumber = new TextBoxNumeric(typeof(int));
        #endregion

        #region Controllers
        PopupMenu _menuPopup;
        DisplayDetails _displayComments = new DisplayDetails("Comments");
        DisplayDetails _displayFinancialComments = new DisplayDetails("Financial Change Comments");
        CheckBoxToggle _ctrVSCheckBox;
        CheckBoxToggle _ctrCalcPrintCostCheckBox;
        CheckBoxToggle _ctrCalcPaperCostCheckBox;
        CheckBoxToggle _ctrApplyTaxCheckBox;
        #endregion

        #region Views
        //SourceGrid.Cells.Views.Cell _vwEnabled = new SourceGrid.Cells.Views.Cell();
        SourceGrid.Cells.Views.Cell _vwDisabled = new SourceGrid.Cells.Views.Cell();
        SourceGrid.Cells.Views.CheckBox _vwCheckBoxDisabled = new SourceGrid.Cells.Views.CheckBox();
        SourceGrid.Cells.Views.Cell _vwInvalid = new SourceGrid.Cells.Views.Cell();
        SourceGrid.Cells.Views.Cell _vwComboInvalid = new SourceGrid.Cells.Views.Cell();
        SourceGrid.Cells.Views.RowHeader _vwRequiredField = new SourceGrid.Cells.Views.RowHeader();
        SourceGrid.Cells.Views.Cell _vwRightAlignEnabled = new SourceGrid.Cells.Views.Cell();
        SourceGrid.Cells.Views.Cell _vwRightAlignDisabled = new SourceGrid.Cells.Views.Cell();
        #endregion

        #region Initialization

        public void Initialize(bool readOnly)
        {
            _readOnly = readOnly;

            // Have to remove the current editor control from the linked controls otherwise an exception is thrown
            if (!this.Selection.ActivePosition.IsEmpty())
            {
                SourceGrid.Cells.Cell curCell = (SourceGrid.Cells.Cell) this[this.Selection.ActivePosition.Row, this.Selection.ActivePosition.Column];
                EditorControlBase curEditor = curCell.Editor as EditorControlBase;
                if (curEditor != null)
                {
                    LinkedControlValue curLinkedControlValue = this.LinkedControls.GetByControl(curEditor.Control);
                    if (curLinkedControlValue != null)
                        this.LinkedControls.Remove(curLinkedControlValue);
                }
            }

            this.Rows.Clear();
            this.Columns.Clear();
            _blankColumn.Clear();
            _Components.Clear();
            this.Redim(63, 1);
            this.FixedRows = 3;
            this.FixedColumns = 1;
            this.Rows[14].Height = 10;
            this.Rows[22].Height = 10;
            this.Rows[40].Height = 10;
            this.Rows[56].Height = 10;
            this.Rows[61].Height = 10;
            this.Columns[0].Width = 200;

            InitEditors();
            InitControllers();
            InitViews();

            CreateRowHeaders();

            if (_dsEstimates.est_component.Count == 0)
                AddComponent();
            else
                InitComponentColumns();

            this.Selection.Focus(new Position(2, 1), true);
        }

        private void InitEditors()
        {
            _edDescription.Control.MaxLength = 35;
            _edComments.Control.MaxLength = 255;
            _edAdNumber.Control.MaxLength = 5;
            _edAdNumber.MinimumValue = (int) 0;
            _edAdNumber.AllowNull = true;
        }

        private void InitControllers()
        {
            _displayComments.ReadOnly = _readOnly;
            _displayFinancialComments.ReadOnly = _readOnly;

            _menuPopup = new PopupMenu(_readOnly);

            List<CheckBoxToggleType> vsToggles = new List<CheckBoxToggleType>();
            vsToggles.Add(CheckBoxToggleType.EnableOnCheck | CheckBoxToggleType.DisableOnUnCheck); // VS Vendor
            vsToggles.Add(CheckBoxToggleType.EnableOnCheck | CheckBoxToggleType.DisableOnUnCheck); // Vendor CPM
            vsToggles.Add(CheckBoxToggleType.NoChange); // Creative Vendor
            vsToggles.Add(CheckBoxToggleType.NoChange); // Creative CPP
            vsToggles.Add(CheckBoxToggleType.NoChange); // Separator Vendor
            vsToggles.Add(CheckBoxToggleType.NoChange); // Separator CPP
            vsToggles.Add(CheckBoxToggleType.NoChange); // Blank Line
            vsToggles.Add(CheckBoxToggleType.NoChange); // Printer Vendor
            vsToggles.Add(CheckBoxToggleType.EnableOnUnCheck | CheckBoxToggleType.DisableOnCheck); // Calc Print Cost?
            vsToggles.Add(CheckBoxToggleType.DisableOnCheck); // Manual Print Cost
            vsToggles.Add(CheckBoxToggleType.DisableOnCheck); // Additional Plates
            vsToggles.Add(CheckBoxToggleType.DisableOnCheck); // Plate Cost
            vsToggles.Add(CheckBoxToggleType.DisableOnCheck); // Replacement Plate Cost
            vsToggles.Add(CheckBoxToggleType.DisableOnCheck); // Run Rate
            vsToggles.Add(CheckBoxToggleType.DisableOnCheck); // Number Digi Handle & Prepare
            vsToggles.Add(CheckBoxToggleType.DisableOnCheck); // Digi Handle & Prepare Rate
            vsToggles.Add(CheckBoxToggleType.DisableOnCheck); // Stitcher Makeready
            vsToggles.Add(CheckBoxToggleType.DisableOnCheck); // Manual Stitcher Makeready Rate
            vsToggles.Add(CheckBoxToggleType.DisableOnCheck); // Press Makeready
            vsToggles.Add(CheckBoxToggleType.DisableOnCheck); // Manual Press Makeready Rate
            vsToggles.Add(CheckBoxToggleType.EnableOnUnCheck | CheckBoxToggleType.DisableOnCheck); // Early Pay Discount
            vsToggles.Add(CheckBoxToggleType.EnableOnUnCheck | CheckBoxToggleType.DisableOnCheck); // Apply Tax
            vsToggles.Add(CheckBoxToggleType.EnableOnUnCheck | CheckBoxToggleType.DisableOnCheck); // Taxable Media
            vsToggles.Add(CheckBoxToggleType.EnableOnUnCheck | CheckBoxToggleType.DisableOnCheck); // Sales Tax
            vsToggles.Add(CheckBoxToggleType.NoChange); // Blank Line
            vsToggles.Add(CheckBoxToggleType.NoChange); // Paper Vendor
            vsToggles.Add(CheckBoxToggleType.NoChange); // Paper Weight
            vsToggles.Add(CheckBoxToggleType.NoChange); // Paper Grade
            vsToggles.Add(CheckBoxToggleType.EnableOnUnCheck | CheckBoxToggleType.DisableOnCheck); // Paper Map
            vsToggles.Add(CheckBoxToggleType.EnableOnUnCheck | CheckBoxToggleType.DisableOnCheck); // Calc Paper Cost?
            vsToggles.Add(CheckBoxToggleType.DisableOnCheck); // Manual Paper Cost
            vsToggles.Add(CheckBoxToggleType.DisableOnCheck); // Run Pounds
            vsToggles.Add(CheckBoxToggleType.DisableOnCheck); // Makeready Pounds
            vsToggles.Add(CheckBoxToggleType.DisableOnCheck); // Plate Change Pounds
            vsToggles.Add(CheckBoxToggleType.DisableOnCheck); // Press Stop Pounds
            vsToggles.Add(CheckBoxToggleType.DisableOnCheck); // Number of Press Stops
            vsToggles.Add(CheckBoxToggleType.EnableOnUnCheck | CheckBoxToggleType.DisableOnCheck); // Early Pay Discount
            vsToggles.Add(CheckBoxToggleType.EnableOnUnCheck | CheckBoxToggleType.DisableOnCheck); // Apply Tax
            vsToggles.Add(CheckBoxToggleType.EnableOnUnCheck | CheckBoxToggleType.DisableOnCheck); // Taxable Media
            vsToggles.Add(CheckBoxToggleType.EnableOnUnCheck | CheckBoxToggleType.DisableOnCheck); // Sales Tax
            _ctrVSCheckBox = new CheckBoxToggle(vsToggles);

            List<CheckBoxToggleType> printcostToggles = new List<CheckBoxToggleType>();
            printcostToggles.Add(CheckBoxToggleType.EnableOnUnCheck | CheckBoxToggleType.DisableOnCheck); // Manual Print Cost
            printcostToggles.Add(CheckBoxToggleType.EnableOnCheck | CheckBoxToggleType.DisableOnUnCheck); // Additional Plates
            printcostToggles.Add(CheckBoxToggleType.EnableOnCheck | CheckBoxToggleType.DisableOnUnCheck); // Plate Cost
            printcostToggles.Add(CheckBoxToggleType.EnableOnCheck | CheckBoxToggleType.DisableOnUnCheck); // Replacement Plate Cost
            printcostToggles.Add(CheckBoxToggleType.EnableOnCheck | CheckBoxToggleType.DisableOnUnCheck); // Run Rate
            printcostToggles.Add(CheckBoxToggleType.EnableOnCheck | CheckBoxToggleType.DisableOnUnCheck); // Number Digi Handle & Prepare
            printcostToggles.Add(CheckBoxToggleType.EnableOnCheck | CheckBoxToggleType.DisableOnUnCheck); // Digi Handle & Prepare Rate
            printcostToggles.Add(CheckBoxToggleType.EnableOnCheck | CheckBoxToggleType.DisableOnUnCheck); // Stitcher Makeready
            printcostToggles.Add(CheckBoxToggleType.EnableOnCheck | CheckBoxToggleType.DisableOnUnCheck); // Manual Stitcher Makeready
            printcostToggles.Add(CheckBoxToggleType.EnableOnCheck | CheckBoxToggleType.DisableOnUnCheck); // Press Makeready
            printcostToggles.Add(CheckBoxToggleType.EnableOnCheck | CheckBoxToggleType.DisableOnUnCheck); // Manual Press Makeready

            _ctrCalcPrintCostCheckBox = new CheckBoxToggle(printcostToggles);

            List<CheckBoxToggleType> papercostToggles = new List<CheckBoxToggleType>();
            papercostToggles.Add(CheckBoxToggleType.EnableOnUnCheck | CheckBoxToggleType.DisableOnCheck); // Manual Paper Cost
            papercostToggles.Add(CheckBoxToggleType.EnableOnCheck | CheckBoxToggleType.DisableOnUnCheck); // Run Pounds
            papercostToggles.Add(CheckBoxToggleType.EnableOnCheck | CheckBoxToggleType.DisableOnUnCheck); // Makeready Pounds
            papercostToggles.Add(CheckBoxToggleType.EnableOnCheck | CheckBoxToggleType.DisableOnUnCheck); // Plate Change Pounds
            papercostToggles.Add(CheckBoxToggleType.EnableOnCheck | CheckBoxToggleType.DisableOnUnCheck); // Press Stop Pounds
            papercostToggles.Add(CheckBoxToggleType.EnableOnCheck | CheckBoxToggleType.DisableOnUnCheck); // Number of Press Stops
            _ctrCalcPaperCostCheckBox = new CheckBoxToggle(papercostToggles);

            List<CheckBoxToggleType> applytaxToggles = new List<CheckBoxToggleType>();
            applytaxToggles.Add(CheckBoxToggleType.EnableOnCheck | CheckBoxToggleType.DisableOnUnCheck); // Taxable Media
            applytaxToggles.Add(CheckBoxToggleType.EnableOnCheck | CheckBoxToggleType.DisableOnUnCheck); // Sales Tax
            _ctrApplyTaxCheckBox = new CheckBoxToggle(applytaxToggles);
        }

        private void InitViews()
        {
            _vwDisabled.BackColor = System.Drawing.SystemColors.Control;
            _vwDisabled.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Gray), new DevAge.Drawing.BorderLine(Color.Gray));
            
            _vwCheckBoxDisabled.BackColor = System.Drawing.SystemColors.Control;
            _vwCheckBoxDisabled.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Gray), new DevAge.Drawing.BorderLine(Color.Gray));

            _vwInvalid.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(System.Drawing.Color.Red, 2));
            _vwComboInvalid.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(System.Drawing.Color.Red, 2));
            _vwRequiredField.Font = new Font(this.Font, FontStyle.Bold);

            _vwRightAlignEnabled.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

            _vwRightAlignDisabled.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            _vwRightAlignDisabled.BackColor = System.Drawing.SystemColors.Control;
            _vwRightAlignDisabled.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Gray), new DevAge.Drawing.BorderLine(Color.Gray));
        }

        private void CreateRowHeaders()
        {
            this[1, 0] = new SourceGrid.Cells.RowHeader("Component ID");
            this[2, 0] = new SourceGrid.Cells.RowHeader("Description*");
            this[2, 0].View = _vwRequiredField;
            this[3, 0] = new SourceGrid.Cells.RowHeader("Comments");
            this[4, 0] = new SourceGrid.Cells.RowHeader("Fx Change Comments");
            this[5, 0] = new SourceGrid.Cells.RowHeader("Ad Number");
            this[6, 0] = new SourceGrid.Cells.RowHeader("Media Type*");
            this[6, 0].View = _vwRequiredField;
            this[7, 0] = new SourceGrid.Cells.RowHeader("Component Type*");
            this[7, 0].View = _vwRequiredField;
            this[8, 0] = new SourceGrid.Cells.RowHeader("Media Qty w/o Insert");
            this[9, 0] = new SourceGrid.Cells.RowHeader("Spoilage Pct");
            this[10, 0] = new SourceGrid.Cells.RowHeader("Page Count*");
            this[10, 0].View = _vwRequiredField;
            this[11, 0] = new SourceGrid.Cells.RowHeader("Width (inches)*");
            this[11, 0].View = _vwRequiredField;
            this[12, 0] = new SourceGrid.Cells.RowHeader("Height (inches)*");
            this[12, 0].View = _vwRequiredField;
            this[13, 0] = new SourceGrid.Cells.RowHeader("Number of Plants");
            this[14, 0] = new SourceGrid.Cells.Cell(); // Blank Line
            this[14, 0].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            this[15, 0] = new SourceGrid.Cells.RowHeader("- Vendor Supplied?");
            this[15, 0].AddController(new ExpandCollapseController(true, 2, "- Vendor Supplied?", "+ Vendor Supplied?"));
            this[16, 0] = new SourceGrid.Cells.RowHeader("  VS Vendor");
            this[17, 0] = new SourceGrid.Cells.RowHeader("  VS Rate (CPM)");
            this[18, 0] = new SourceGrid.Cells.RowHeader("Creative Vendor");
            this[19, 0] = new SourceGrid.Cells.RowHeader("Creative CPP");
            this[20, 0] = new SourceGrid.Cells.RowHeader("Separator Vendor");
            this[21, 0] = new SourceGrid.Cells.RowHeader("Separator CPP");
            this[22, 0] = new SourceGrid.Cells.Cell(); // Blank Line
            this[22, 0].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            this[23, 0] = new SourceGrid.Cells.RowHeader("- Printer Vendor");
            this[23, 0].AddController(new ExpandCollapseController(true, 17, "- Printer Vendor", "+ Printer Vendor"));
            this[24, 0] = new SourceGrid.Cells.RowHeader("  Calc Print Cost");
            this[25, 0] = new SourceGrid.Cells.RowHeader("  Print Cost");
            this[26, 0] = new SourceGrid.Cells.RowHeader("  Additional Plates");
            this[27, 0] = new SourceGrid.Cells.RowHeader("  Plate Cost Rate");
            this[28, 0] = new SourceGrid.Cells.RowHeader("  Replacement Plate Cost");
            this[29, 0] = new SourceGrid.Cells.RowHeader("  Run Rate");
            this[30, 0] = new SourceGrid.Cells.RowHeader("  Number Digital H&P");
            this[31, 0] = new SourceGrid.Cells.RowHeader("  Digital H&P Rate");
            this[32, 0] = new SourceGrid.Cells.RowHeader("  Stitcher Makeready (per Plant)");
            this[33, 0] = new SourceGrid.Cells.RowHeader("  Manual Stitcher Makeready (per Plant)");
            this[34, 0] = new SourceGrid.Cells.RowHeader("  Press Makeready (per Plant)");
            this[35, 0] = new SourceGrid.Cells.RowHeader("  Manual Press Makeready (per Plant)");
            this[36, 0] = new SourceGrid.Cells.RowHeader("  Early Pay Discount");
            this[37, 0] = new SourceGrid.Cells.RowHeader("  - Apply Tax");
            this[37, 0].AddController(new ExpandCollapseController(true, 2, "  - Apply Tax", "  + Apply Tax"));
            this[38, 0] = new SourceGrid.Cells.RowHeader("    Taxable Media");
            this[39, 0] = new SourceGrid.Cells.RowHeader("    Sales Tax");
            this[40, 0] = new SourceGrid.Cells.Cell(); // Blank Line
            this[40, 0].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            this[41, 0] = new SourceGrid.Cells.RowHeader("- Paper Vendor");
            this[41, 0].AddController(new ExpandCollapseController(true, 14, "- Paper Vendor*", "+ Paper Vendor"));
            this[42, 0] = new SourceGrid.Cells.RowHeader("  Paper Weight*");
            this[42, 0].View = _vwRequiredField;
            this[43, 0] = new SourceGrid.Cells.RowHeader("  Paper Grade");
            this[44, 0] = new SourceGrid.Cells.RowHeader("  Paper Description");
            this[45, 0] = new SourceGrid.Cells.RowHeader("  Calc Paper Cost");
            this[46, 0] = new SourceGrid.Cells.RowHeader("  Paper Cost");
            this[47, 0] = new SourceGrid.Cells.RowHeader("  Run Pounds");
            this[48, 0] = new SourceGrid.Cells.RowHeader("  Makeready Pounds");
            this[49, 0] = new SourceGrid.Cells.RowHeader("  Plate Change Pounds");
            this[50, 0] = new SourceGrid.Cells.RowHeader("  Press Stop Pounds");
            this[51, 0] = new SourceGrid.Cells.RowHeader("  Number of Press Stops");
            this[52, 0] = new SourceGrid.Cells.RowHeader("  Early Pay Discount");
            this[53, 0] = new SourceGrid.Cells.RowHeader("  - Apply Tax");
            this[53, 0].AddController(new ExpandCollapseController(true, 2, "  - Apply Tax", "  + Apply Tax"));
            this[54, 0] = new SourceGrid.Cells.RowHeader("    Taxable Media");
            this[55, 0] = new SourceGrid.Cells.RowHeader("    Sales Tax");
            this[56, 0] = new SourceGrid.Cells.Cell(); // Blank Line
            this[56, 0].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            this[57, 0] = new SourceGrid.Cells.RowHeader("- Assembly Vendor");
            this[57, 0].AddController(new ExpandCollapseController(true, 3, "- Assembly Vendor", "+ Assembly Vendor"));
            this[58, 0] = new SourceGrid.Cells.RowHeader("  Stitch-In Rate (CPM)");
            this[59, 0] = new SourceGrid.Cells.RowHeader("  Blow-In Rate (CPM)");
            this[60, 0] = new SourceGrid.Cells.RowHeader("  Onsert Rate (CPM)");
            this[61, 0] = new SourceGrid.Cells.Cell(); // Blank Line
            this[61, 0].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            this[62, 0] = new SourceGrid.Cells.RowHeader("Other Production");

            for (int i = 1; i < 63; ++i)
                this[i, 0].AddController(SelectCell.Default);
        }

        private void InitComponentColumns()
        {
            int colIndex = 0;

            // The first column is the host component
            if (_dsEstimates.est_component.Count > 0)
            {
                DataRow host_row = _dsEstimates.est_component.Select("EST_ComponentType_ID = 1")[0];
                _Components.Add((Estimates.est_componentRow)host_row);
                _blankColumn.Add(false);
                ++colIndex;

                // Create an Empty Column
                this.Columns.Insert(colIndex);
                this.Columns[colIndex].Width = 125;

                foreach (DataRow dr in _dsEstimates.est_component.Select("EST_ComponentType_ID <> 1 and EST_Component_ID > 0", "EST_Component_ID"))
                {
                    _Components.Add((Estimates.est_componentRow)dr);
                    _blankColumn.Add(false);
                    ++colIndex;

                    // Create Empty Column
                    this.Columns.Insert(colIndex);
                    this.Columns[colIndex].Width = 125;
                }

                foreach (DataRow dr in _dsEstimates.est_component.Select("EST_ComponentType_ID <> 1 and EST_Component_ID < 1", "EST_Component_ID desc"))
                {
                    _Components.Add((Estimates.est_componentRow)dr);
                    _blankColumn.Add(false);
                    ++colIndex;

                    // Create Empty Column
                    this.Columns.Insert(colIndex);
                    this.Columns[colIndex].Width = 125;
                }
            }

            for (int i = 0; i < _Components.Count; ++i)
            {
                InitComponent(i + 1, _Components[i]);
                this.ComponentAdded(this, new EventArgs());
            }
        }

        private void InitComponent(int colIndex, Datasets.Estimates.est_componentRow c_row)
        {
            #region Set Cell Editors and Values
            this[0, colIndex] = new SourceGrid.Cells.Cell("Component " + colIndex.ToString());
            this[0, colIndex].View = SourceGrid.Cells.Views.ColumnHeader.Default;

            this[1, colIndex] = new SourceGrid.Cells.Cell(c_row.est_component_id); // Component ID
            this[2, colIndex] = new SourceGrid.Cells.Cell(c_row.description, _edDescription); // Description
            if (c_row.IscommentsNull())
                this[3, colIndex] = new SourceGrid.Cells.Cell(String.Empty, _edComments); // Comments
            else
                this[3, colIndex] = new SourceGrid.Cells.Cell(c_row.comments, _edComments); // Comments

            if (c_row.IsfinancialchangecommentNull())
                this[4, colIndex] = new SourceGrid.Cells.Cell(String.Empty, _edComments); // Financial Comments
            else
                this[4, colIndex] = new SourceGrid.Cells.Cell(c_row.financialchangecomment, _edComments); // Financial Comments

            if (c_row.IsadnumberNull())
                this[5, colIndex] = new SourceGrid.Cells.Cell(String.Empty, _edAdNumber); // Ad Number
            else
                this[5, colIndex] = new SourceGrid.Cells.Cell(c_row.adnumber, _edAdNumber); // Ad Number

            EstimateMediaTypeEditor edEstimateMediaType = new EstimateMediaTypeEditor(_dsEstimates);
            edEstimateMediaType.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            this[6, colIndex] = new SourceGrid.Cells.Cell(edEstimateMediaType.GetEstimateMediaTypefromID(c_row.est_estimatemediatype_id), edEstimateMediaType); // Estimate Media Type
            this[6, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default; // Estimate Media Type

            ComponentTypeEditor edComponentType = new ComponentTypeEditor(_dsEstimates);
            edComponentType.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;

            // The first component must be the HOST Component
            if (colIndex == 1)
            {
                this[7, colIndex] = new SourceGrid.Cells.Cell(new IntPair(1, "HOST"));
                this[7, colIndex].View = _vwDisabled;
            }
            // No other components can be of type HOST
            else
            {
                this[7, colIndex] = new SourceGrid.Cells.Cell(edComponentType.GetComponentTypefromID(c_row.est_componenttype_id), edComponentType);
                this[7, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            }

            if (c_row.IsmediaqtywoinsertNull())
                this[8, colIndex] = new SourceGrid.Cells.Cell(String.Empty, new TextBoxIntegerRange(0, null)); // Media Qty w/o Insert
            else
                this[8, colIndex] = new SourceGrid.Cells.Cell(c_row.mediaqtywoinsert, new TextBoxIntegerRange(0, null)); // Media Qty w/o Insert
            this[8, colIndex].View = _vwRightAlignEnabled;

            if (c_row.IsspoilagepctNull())
                this[9, colIndex] = new SourceGrid.Cells.Cell(string.Empty, new TextBoxPercentageRange(0, 1));
            else
                this[9, colIndex] = new SourceGrid.Cells.Cell(c_row.spoilagepct, new TextBoxPercentageRange(0, 1));
            this[9, colIndex].View = _vwRightAlignEnabled;

            this[10, colIndex] = new SourceGrid.Cells.Cell(c_row.pagecount, new NumericUpDown(typeof(int), 999, 1, 1));  // Page Count
            this[10, colIndex].View = _vwRightAlignEnabled;
            this[10, colIndex].Editor.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            this[11, colIndex] = new SourceGrid.Cells.Cell(c_row.width, new TextBoxDecimalRange(0, _cMaxWidth));  // Width
            this[11, colIndex].View = _vwRightAlignEnabled;
            this[12, colIndex] = new SourceGrid.Cells.Cell(c_row.height, new TextBoxDecimalRange(0, _cMaxHeight));  // Height
            this[12, colIndex].View = _vwRightAlignEnabled;
            if (c_row.IsnumberofplantsNull())
                this[13, colIndex] = new SourceGrid.Cells.Cell(null, new NumericUpDown(typeof(int), 999, 0, 1)); // Number of Plants
            else
                this[13, colIndex] = new SourceGrid.Cells.Cell(c_row.numberofplants, new NumericUpDown(typeof(int), 999, 0, 1)); // Number of Plants
            this[13, colIndex].View = _vwRightAlignEnabled;
            this[13, colIndex].Editor.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            this[13, colIndex].Editor.AllowNull = true;
            this[14, colIndex] = null; // Blank Line

            this[15, colIndex] = new SourceGrid.Cells.CheckBox(string.Empty, c_row.vendorsupplied);  // Vendor Supplied?

            if (c_row.Isvendorsupplied_idNull())
            {
                VendorEditor edVSVendor = new VendorEditor(_dsEstimates, 9, null);
                edVSVendor.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
                this[16, colIndex] = new SourceGrid.Cells.Cell(null, edVSVendor);
            }
            else
            {
                VendorEditor edVSVendor = new VendorEditor(_dsEstimates, 9, c_row.vendorsupplied_id);
                edVSVendor.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
                this[16, colIndex] = new SourceGrid.Cells.Cell(edVSVendor.GetVendorfromID(c_row.vendorsupplied_id), edVSVendor);
                this[16, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            }
            this[16, colIndex].Editor.AllowNull = true;

            if (c_row.IsvendorcpmNull())
                this[17, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));
            else
                this[17, colIndex] = new SourceGrid.Cells.Cell(c_row.vendorcpm, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));
            this[17, colIndex].View = _vwRightAlignEnabled;
            this[17, colIndex].Editor.AllowNull = true;

            if (c_row.Iscreativevendor_idNull())
            {
                VendorEditor edCreative = new VendorEditor(_dsEstimates, 3, null);
                this[18, colIndex] = new SourceGrid.Cells.Cell(null, edCreative); // Creative Vendor
            }
            else
            {
                VendorEditor edCreative = new VendorEditor(_dsEstimates, 3, c_row.creativevendor_id);
                this[18, colIndex] = new SourceGrid.Cells.Cell(edCreative.GetVendorfromID(c_row.creativevendor_id), edCreative); // Creative Vendor
            }
            this[18, colIndex].Editor.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            this[18, colIndex].Editor.AllowNull = true;
            this[18, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;

            if (c_row.IscreativecppNull())
                this[19, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));  // Creative CPP
            else
                this[19, colIndex] = new SourceGrid.Cells.Cell(c_row.creativecpp, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));  // Creative CPP
            this[19, colIndex].View = _vwRightAlignEnabled;
            this[19, colIndex].Editor.AllowNull = true;

            if (c_row.Isseparator_idNull())
            {
                VendorEditor edSeparator = new VendorEditor(_dsEstimates, 4, null);
                this[20, colIndex] = new SourceGrid.Cells.Cell(null, edSeparator); // Separator Vendor
            }
            else
            {
                VendorEditor edSeparator = new VendorEditor(_dsEstimates, 4, c_row.separator_id);
                this[20, colIndex] = new SourceGrid.Cells.Cell(edSeparator.GetVendorfromID(c_row.separator_id), edSeparator); // Separator Vendor
            }
            this[20, colIndex].Editor.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            this[20, colIndex].Editor.AllowNull = true;
            this[20, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;

            if (c_row.IsseparatorcppNull())
                this[21, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));  // Separator CPP
            else
                this[21, colIndex] = new SourceGrid.Cells.Cell(c_row.separatorcpp, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));  // Separator CPP
            this[21, colIndex].View = _vwRightAlignEnabled;
            this[21, colIndex].Editor.AllowNull = true;

            this[22, colIndex] = null; // Blank Line
            if (c_row.Isprinter_idNull())
            {
                PrinterEditor edPrinter = new PrinterEditor(_dsEstimates, _dsEstimates.est_estimate[0].rundate, null);
                this[23, colIndex] = new SourceGrid.Cells.Cell(null, edPrinter);
            }
            else
            {
                PrinterEditor edPrinter = new PrinterEditor(_dsEstimates, _dsEstimates.est_estimate[0].rundate, c_row.printer_id);
                this[23, colIndex] = new SourceGrid.Cells.Cell(edPrinter.GetPrinterfromID(c_row.printer_id), edPrinter);
            }
            this[23, colIndex].Editor.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            this[23, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            this[23, colIndex].Editor.AllowNull = true;

            this[24, colIndex] = new SourceGrid.Cells.CheckBox(string.Empty, c_row.calculateprintcost); // Calc Print Cost

            if (c_row.IsprintcostNull())
                this[25, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Manual Print Cost
            else
                this[25, colIndex] = new SourceGrid.Cells.Cell(c_row.printcost, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));
            this[25, colIndex].View = _vwRightAlignEnabled;
            if (c_row.IsadditionalplatesNull())
                this[26, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxIntegerRange(0, int.MaxValue)); // Additional Plates
            else
                this[26, colIndex] = new SourceGrid.Cells.Cell(c_row.additionalplates, new TextBoxIntegerRange(0, int.MaxValue)); // Additional Plates
            this[26, colIndex].View = _vwRightAlignEnabled;
            this[26, colIndex].Editor.AllowNull = true;

            if (!c_row.Isprinter_idNull() && c_row.calculateprintcost)
            {
                PrinterRateEditor edPlateCost = new PrinterRateEditor(_dsEstimates, c_row.printer_id, 8);
                edPlateCost.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
                edPlateCost.AllowNull = true;
                if (c_row.Isplatecost_idNull())
                    this[27, colIndex] = new SourceGrid.Cells.Cell(edPlateCost.DefaultPrinterRate, edPlateCost);
                else
                    this[27, colIndex] = new SourceGrid.Cells.Cell(edPlateCost.FindRatefromID(c_row.platecost_id), edPlateCost);
                this[27, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
                this[27, colIndex].Editor.AllowNull = true;
            }
            else
            {
                this[27, colIndex] = new SourceGrid.Cells.Cell();
                this[27, colIndex].View = _vwDisabled;
            }

            if (c_row.IsreplacementplatecostNull())
                this[28, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Replacement Plate Cost
            else
                this[28, colIndex] = new SourceGrid.Cells.Cell(c_row.replacementplatecost, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Replacement Plate Cost
            this[28, colIndex].View = _vwRightAlignEnabled;
            this[28, colIndex].Editor.AllowNull = true;

            if (c_row.IsrunrateNull())
                this[29, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Run Rate
            else
                this[29, colIndex] = new SourceGrid.Cells.Cell(c_row.runrate, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Run Rate
            this[29, colIndex].View = _vwRightAlignEnabled;
            this[29, colIndex].Editor.AllowNull = true;

            if (c_row.IsnumberdigitalhandlenprepareNull())
                this[30, colIndex] = new SourceGrid.Cells.Cell(null, new NumericUpDown(typeof(int), 999, 0, 1)); // Number Digital Handle & Prepare
            else
                this[30, colIndex] = new SourceGrid.Cells.Cell(c_row.numberdigitalhandlenprepare, new NumericUpDown(typeof(int), 999, 0, 1)); // Number Digital Handle & Prepare
            this[30, colIndex].View = _vwRightAlignEnabled;
            this[30, colIndex].Editor.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            this[30, colIndex].Editor.AllowNull = true;

            // Digital Handle & Prepare Rate
            if (!c_row.Isprinter_idNull() && c_row.calculateprintcost)
            {
                PrinterRateEditor edDigiHP = new PrinterRateEditor(_dsEstimates, c_row.printer_id, 5);
                edDigiHP.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
                if (c_row.Isdigitalhandlenprepare_idNull())
                    this[31, colIndex] = new SourceGrid.Cells.Cell(edDigiHP.DefaultPrinterRate, edDigiHP);
                else
                    this[31, colIndex] = new SourceGrid.Cells.Cell(edDigiHP.FindRatefromID(c_row.digitalhandlenprepare_id), edDigiHP);
                this[31, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
                this[31, colIndex].Editor.AllowNull = true;
            }
            else
            {
                this[31, colIndex] = new SourceGrid.Cells.Cell();
                this[31, colIndex].View = _vwDisabled;
            }

            // Stitcher Makeready
            if (!c_row.Isprinter_idNull() && c_row.calculateprintcost)
            {
                PrinterRateEditor edStitcherMakeready = new PrinterRateEditor(_dsEstimates, c_row.printer_id, 4);
                edStitcherMakeready.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
                edStitcherMakeready.AllowNull = true;
                if (c_row.Isstitchermakeready_idNull())
                    this[32, colIndex] = new SourceGrid.Cells.Cell(null, edStitcherMakeready);
                else
                    this[32, colIndex] = new SourceGrid.Cells.Cell(edStitcherMakeready.FindRatefromID(c_row.stitchermakeready_id), edStitcherMakeready);
                this[32, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            }
            else
            {
                this[32, colIndex] = new SourceGrid.Cells.Cell();
                this[32, colIndex].View = _vwDisabled;
            }

            // Manual Stitcher Makeready
            if (!c_row.Isprinter_idNull() && c_row.calculateprintcost)
            {
                if (c_row.IsstitchermakereadyrateNull())
                    this[33, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));
                else
                    this[33, colIndex] = new SourceGrid.Cells.Cell(c_row.stitchermakereadyrate, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));
                this[33, colIndex].View = _vwRightAlignEnabled;
                this[33, colIndex].Editor.AllowNull = true;
            }
            else
            {
                this[33, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));
                this[33, colIndex].View = _vwRightAlignDisabled;
            }

            // Press Makeready
            if (!c_row.Isprinter_idNull() && c_row.calculateprintcost)
            {
                PrinterRateEditor edPressMakeready = new PrinterRateEditor(_dsEstimates, c_row.printer_id, 10);
                edPressMakeready.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
                edPressMakeready.AllowNull = true;
                if (c_row.Ispressmakeready_idNull())
                    this[34, colIndex] = new SourceGrid.Cells.Cell(null, edPressMakeready);
                else
                    this[34, colIndex] = new SourceGrid.Cells.Cell(edPressMakeready.FindRatefromID(c_row.pressmakeready_id), edPressMakeready);
                this[34, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            }
            else
            {
                this[34, colIndex] = new SourceGrid.Cells.Cell();
                this[34, colIndex].View = _vwDisabled;
            }

            // Manual Press Makeready
            if (!c_row.Isprinter_idNull() && c_row.calculateprintcost)
            {
                if (c_row.IspressmakereadyrateNull())
                    this[35, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));
                else
                    this[35, colIndex] = new SourceGrid.Cells.Cell(c_row.pressmakereadyrate, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));
                this[35, colIndex].View = _vwRightAlignEnabled;
                this[35, colIndex].Editor.AllowNull = true;
            }
            else
            {
                this[35, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));
                this[35, colIndex].View = _vwRightAlignDisabled;
            }

            if (c_row.IsearlypayprintdiscountNull())
                this[36, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxPercentageRange(0, 1)); // Early Pay Discount
            else
                this[36, colIndex] = new SourceGrid.Cells.Cell(c_row.earlypayprintdiscount, new TextBoxPercentageRange(0, 1)); // Early Pay Discount
            this[36, colIndex].View = _vwRightAlignEnabled;
            this[36, colIndex].Editor.AllowNull = true;

            this[37, colIndex] = new SourceGrid.Cells.CheckBox(string.Empty, c_row.printerapplytax); // Printer Apply Tax;

            if (c_row.IsprintertaxablemediapctNull())
                this[38, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxPercentageRange(0, 1)); // Taxable Media
            else
                this[38, colIndex] = new SourceGrid.Cells.Cell(c_row.printertaxablemediapct, new TextBoxPercentageRange(0, 1)); // Taxable Media
            this[38, colIndex].View = _vwRightAlignEnabled;
            this[38, colIndex].Editor.AllowNull = true;

            if (c_row.IsprintersalestaxpctNull())
                this[39, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxPercentageRange(0, 1)); // Sales Tax
            else
                this[39, colIndex] = new SourceGrid.Cells.Cell(c_row.printersalestaxpct, new TextBoxPercentageRange(0, 1));
            this[39, colIndex].View = _vwRightAlignEnabled;
            this[39, colIndex].Editor.AllowNull = true;

            this[40, colIndex] = null; // Blank Line

            if (c_row.Ispaper_idNull())
            {
                PaperEditor edPaper = new PaperEditor(_dsEstimates, _dsEstimates.est_estimate[0].rundate, null);
                this[41, colIndex] = new SourceGrid.Cells.Cell(null, edPaper); // Paper Vendor
            }
            else
            {
                PaperEditor edPaper = new PaperEditor(_dsEstimates, _dsEstimates.est_estimate[0].rundate, c_row.paper_id);
                this[41, colIndex] = new SourceGrid.Cells.Cell(edPaper.GetPaperfromID(c_row.paper_id), edPaper);
            }
            this[41, colIndex].Editor.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            this[41, colIndex].Editor.AllowNull = true;
            this[41, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;

            PaperWeightEditor edPaperWeight = new PaperWeightEditor(_dsEstimates);
            edPaperWeight.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            this[42, colIndex] = new SourceGrid.Cells.Cell(edPaperWeight.GetWeightFromID(c_row.paperweight_id), edPaperWeight);
            this[42, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;

            PaperGradeEditor edPaperGrade = new PaperGradeEditor(_dsEstimates); // Paper Grade;
            edPaperGrade.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            edPaperGrade.AllowNull = true;
            if (c_row.Ispapergrade_idNull())
                this[43, colIndex] = new SourceGrid.Cells.Cell(null, edPaperGrade);
            else
                this[43, colIndex] = new SourceGrid.Cells.Cell(edPaperGrade.GetGradefromID(c_row.papergrade_id), edPaperGrade);
            this[43, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;

            if (c_row.Ispaper_idNull())
            {
                this[44, colIndex] = new SourceGrid.Cells.Cell();
                this[44, colIndex].View = _vwDisabled;
            }
            else if (c_row.Ispaper_map_idNull())
            {
                PaperMapEditor edPaperMap = new PaperMapEditor(_dsEstimates, c_row.paper_id, c_row.paperweight_id, c_row.papergrade_id, _dsEstimates.est_estimate[0].rundate);
                edPaperMap.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
                edPaperGrade.AllowNull = true;
                this[44, colIndex] = new SourceGrid.Cells.Cell(edPaperMap.DefaultPaperMap, edPaperMap);
                this[44, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            }
            else
            {
                PaperMapEditor edPaperMap = new PaperMapEditor(_dsEstimates, c_row.paper_id, c_row.paperweight_id, c_row.papergrade_id, _dsEstimates.est_estimate[0].rundate);
                edPaperMap.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
                edPaperGrade.AllowNull = true;
                this[44, colIndex] = new SourceGrid.Cells.Cell(edPaperMap.GetPaperMapFromID(c_row.paper_map_id), edPaperMap);
                this[44, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            }

            this[45, colIndex] = new SourceGrid.Cells.CheckBox(string.Empty, c_row.calculatepapercost);

            if (c_row.IspapercostNull())
            {
                this[46, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Paper Cost
            }
            else
                this[46, colIndex] = new SourceGrid.Cells.Cell(c_row.papercost, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));
            this[46, colIndex].View = _vwRightAlignEnabled;

            if (c_row.IsrunpoundsNull())
                this[47, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxDecimalRange(0, _cMaxRunPounds, 2)); // Run Pounds
            else
                this[47, colIndex] = new SourceGrid.Cells.Cell(c_row.runpounds, new TextBoxDecimalRange(0, _cMaxRunPounds, 2)); // Run Pounds
            this[47, colIndex].View = _vwRightAlignEnabled;
            this[47, colIndex].Editor.AllowNull = true;

            if (c_row.IsmakereadypoundsNull())
                this[48, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxIntegerRange(0, _cMaxMakereadyPounds)); // Makeready Pounds
            else
                this[48, colIndex] = new SourceGrid.Cells.Cell(c_row.makereadypounds, new TextBoxIntegerRange(0, _cMaxMakereadyPounds)); // Makeready Pounds
            this[48, colIndex].View = _vwRightAlignEnabled;
            this[48, colIndex].Editor.AllowNull = true;

            if (c_row.IsplatechangepoundsNull())
                this[49, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxDecimalRange(0, _cMaxPlateChangePounds, 2)); // Plate Change Pounds
            else
                this[49, colIndex] = new SourceGrid.Cells.Cell(c_row.platechangepounds, new TextBoxDecimalRange(0, _cMaxPlateChangePounds, 2)); // Plate Change Pounds
            this[49, colIndex].View = _vwRightAlignEnabled;
            this[49, colIndex].Editor.AllowNull = true;

            if (c_row.IspressstoppoundsNull())
                this[50, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxIntegerRange(0, _cMaxPressStopPounds)); // Press Stop Pounds
            else
                this[50, colIndex] = new SourceGrid.Cells.Cell(c_row.pressstoppounds, new TextBoxIntegerRange(0, _cMaxPressStopPounds));
            this[50, colIndex].View = _vwRightAlignEnabled;
            this[50, colIndex].Editor.AllowNull = true;

            if (c_row.IsnumberofpressstopsNull())
                this[51, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxIntegerRange(0, int.MaxValue)); // Number of Press Stops
            else
                this[51, colIndex] = new SourceGrid.Cells.Cell(c_row.numberofpressstops, new TextBoxIntegerRange(0, int.MaxValue)); // Number of Press Stops
            this[51, colIndex].View = _vwRightAlignEnabled;
            this[51, colIndex].Editor.AllowNull = true;

            if (c_row.IsearlypaypaperdiscountNull())
                this[52, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxPercentageRange(0, 1)); // Early Pay Discount
            else
                this[52, colIndex] = new SourceGrid.Cells.Cell(c_row.earlypaypaperdiscount, new TextBoxPercentageRange(0, 1)); // Early Pay Discount
            this[52, colIndex].View = _vwRightAlignEnabled;
            this[52, colIndex].Editor.AllowNull = true;

            this[53, colIndex] = new SourceGrid.Cells.CheckBox(string.Empty, c_row.paperapplytax); // Paper Apply Tax

            if (c_row.IspapertaxablemediapctNull())
                this[54, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxPercentageRange(0, 1)); // Taxable Media
            else
                this[54, colIndex] = new SourceGrid.Cells.Cell(c_row.papertaxablemediapct, new TextBoxPercentageRange(0, 1)); // Taxable Media
            this[54, colIndex].View = _vwRightAlignEnabled;
            this[54, colIndex].Editor.AllowNull = true;

            if (c_row.IspapersalestaxpctNull())
                this[55, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxPercentageRange(0, 1)); // Sales Tax
            else
                this[55, colIndex] = new SourceGrid.Cells.Cell(c_row.papersalestaxpct, new TextBoxPercentageRange(0, 1)); // Sales Tax
            this[55, colIndex].View = _vwRightAlignEnabled;
            this[55, colIndex].Editor.AllowNull = true;

            this[56, colIndex] = null; // Blank Line

            if (c_row.Isassemblyvendor_idNull())
            {
                PrinterEditor edAssemblyVendor = new PrinterEditor(_dsEstimates, _dsEstimates.est_estimate[0].rundate, null);
                this[57, colIndex] = new SourceGrid.Cells.Cell(null, edAssemblyVendor);
            }
            else
            {
                PrinterEditor edAssemblyVendor = new PrinterEditor(_dsEstimates, _dsEstimates.est_estimate[0].rundate, c_row.assemblyvendor_id);
                this[57, colIndex] = new SourceGrid.Cells.Cell(edAssemblyVendor.GetPrinterfromID(c_row.assemblyvendor_id), edAssemblyVendor);
            }
            this[57, colIndex].Editor.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            this[57, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            this[57, colIndex].Editor.AllowNull = true;


            if (!c_row.Isassemblyvendor_idNull())
            {
                switch (c_row.est_componenttype_id)
                {
                    case 3: // Stitch-In
                        PrinterRateEditor edStitchIn = new PrinterRateEditor(_dsEstimates, c_row.assemblyvendor_id, 1);
                        edStitchIn.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
                        if (c_row.Isstitchin_idNull())
                            this[58, colIndex] = new SourceGrid.Cells.Cell(edStitchIn.DefaultPrinterRate, edStitchIn);
                        else
                            this[58, colIndex] = new SourceGrid.Cells.Cell(edStitchIn.FindRatefromID(c_row.stitchin_id), edStitchIn);
                        this[58, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;

                        this[59, colIndex] = new SourceGrid.Cells.Cell();

                        this[60, colIndex] = new SourceGrid.Cells.Cell();
                        break;
                    case 4: // Blow-In
                        this[58, colIndex] = new SourceGrid.Cells.Cell();

                        PrinterRateEditor edBlowIn = new PrinterRateEditor(_dsEstimates, c_row.assemblyvendor_id, 2);
                        edBlowIn.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
                        if (c_row.Isblowin_idNull())
                            this[59, colIndex] = new SourceGrid.Cells.Cell(edBlowIn.DefaultPrinterRate, edBlowIn);
                        else
                            this[59, colIndex] = new SourceGrid.Cells.Cell(edBlowIn.FindRatefromID(c_row.blowin_id), edBlowIn);
                        this[59, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;

                        this[60, colIndex] = new SourceGrid.Cells.Cell();
                        break;
                    case 2: // Onsert
                        this[58, colIndex] = new SourceGrid.Cells.Cell();

                        this[59, colIndex] = new SourceGrid.Cells.Cell();

                        PrinterRateEditor edOnsert = new PrinterRateEditor(_dsEstimates, c_row.assemblyvendor_id, 9);
                        edOnsert.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
                        if (c_row.Isonsert_idNull())
                            this[60, colIndex] = new SourceGrid.Cells.Cell(edOnsert.DefaultPrinterRate, edOnsert);
                        else
                            this[60, colIndex] = new SourceGrid.Cells.Cell(edOnsert.FindRatefromID(c_row.onsert_id), edOnsert);
                        this[60, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
                        break;
                    default:
                        this[58, colIndex] = new SourceGrid.Cells.Cell();
                        this[59, colIndex] = new SourceGrid.Cells.Cell();
                        this[60, colIndex] = new SourceGrid.Cells.Cell();
                        break;
                }
            }
            else
            {
                this[58, colIndex] = new SourceGrid.Cells.Cell();  // Stitch-In Rate
                this[59, colIndex] = new SourceGrid.Cells.Cell();  // Blow-In Rate
                this[60, colIndex] = new SourceGrid.Cells.Cell();  // Onsert Rate
            }

            this[61, colIndex] = null; // Blank Line

            if (c_row.IsotherproductionNull())
                this[62, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Other Production
            else
                this[62, colIndex] = new SourceGrid.Cells.Cell(c_row.otherproduction, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Other Production
            this[62, colIndex].View = _vwRightAlignEnabled;
            this[62, colIndex].Editor.AllowNull = true;

            #endregion

            #region Disable Cells / Set to Unselectable

            this[1, colIndex].View = _vwDisabled; // Component ID
            this[1, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            if (c_row.vendorsupplied)
            {
                for (int i = 24; i < 40; ++i)
                {
                    if (this[i, colIndex].GetType() == typeof(SourceGrid.Cells.CheckBox))
                        this[i, colIndex].View = _vwCheckBoxDisabled;
                    else
                        this[i, colIndex].View = _vwDisabled;
                    this[i, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    if (this[i, colIndex].Editor != null)
                        this[i, colIndex].Editor.EnableEdit = false;
                }

                for (int i = 44; i < 56; ++i)
                {
                    if (this[i, colIndex].GetType() == typeof(SourceGrid.Cells.CheckBox))
                        this[i, colIndex].View = _vwCheckBoxDisabled;
                    else
                        this[i, colIndex].View = _vwDisabled;
                    this[i, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    if (this[i, colIndex].Editor != null)
                        this[i, colIndex].Editor.EnableEdit = false;
                }
            }
            else
            {
                this[16, colIndex].View = _vwDisabled; // VS Vendor
                this[16, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                if (this[16, colIndex].Editor != null)
                    this[16, colIndex].Editor.EnableEdit = false;

                this[17, colIndex].View = _vwDisabled; // VS Vendor CPM
                this[17, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                if (this[17, colIndex].Editor != null)
                    this[17, colIndex].Editor.EnableEdit = false;

                if (c_row.calculateprintcost)
                {
                    this[25, colIndex].View = _vwDisabled;
                    this[25, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    if (this[25, colIndex].Editor != null)
                        this[25, colIndex].Editor.EnableEdit = false;
                }
                else
                {
                    for (int i = 26; i < 36; ++i)
                    {
                        if (this[i, colIndex].GetType() == typeof(SourceGrid.Cells.CheckBox))
                            this[i, colIndex].View = _vwCheckBoxDisabled;
                        else
                            this[i, colIndex].View = _vwDisabled;
                        this[i, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[i, colIndex].Editor != null)
                            this[i, colIndex].Editor.EnableEdit = false;
                    }
                }

                if (!c_row.printerapplytax)
                {
                    this[38, colIndex].View = _vwDisabled;
                    this[38, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    if (this[38, colIndex].Editor != null)
                        this[38, colIndex].Editor.EnableEdit = false;

                    this[39, colIndex].View = _vwDisabled;
                    this[39, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    if (this[39, colIndex].Editor != null)
                        this[39, colIndex].Editor.EnableEdit = false;
                }

                if (c_row.calculatepapercost)
                {
                    this[46, colIndex].View = _vwDisabled;
                    this[46, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    if (this[46, colIndex].Editor != null)
                        this[46, colIndex].Editor.EnableEdit = false;
                }
                else
                {
                    for (int i = 47; i < 52; ++i)
                    {
                        if (this[i, colIndex].GetType() == typeof(SourceGrid.Cells.CheckBox))
                            this[i, colIndex].View = _vwCheckBoxDisabled;
                        else
                            this[i, colIndex].View = _vwDisabled;
                        this[i, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[i, colIndex].Editor != null)
                            this[i, colIndex].Editor.EnableEdit = false;
                    }
                }

                if (!c_row.paperapplytax)
                {
                    this[54, colIndex].View = _vwDisabled;
                    this[54, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    if (this[54, colIndex].Editor != null)
                        this[54, colIndex].Editor.EnableEdit = false;

                    this[55, colIndex].View = _vwDisabled;
                    this[55, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    if (this[55, colIndex].Editor != null)
                        this[55, colIndex].Editor.EnableEdit = false;
                }
            }

            if (!c_row.Isassemblyvendor_idNull())
            {
                switch (c_row.est_componenttype_id)
                {
                    case 3: // Stitch-In
                        this[59, colIndex].View = _vwDisabled; // Blow-In
                        this[59, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[59, colIndex].Editor != null)
                            this[59, colIndex].Editor.EnableEdit = false;

                        this[60, colIndex].View = _vwDisabled; // Onsert
                        this[60, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[60, colIndex].Editor != null)
                            this[60, colIndex].Editor.EnableEdit = false;
                        break;
                    case 4: // Blow-In
                        this[58, colIndex].View = _vwDisabled; // Stitch-In
                        this[58, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[58, colIndex].Editor != null)
                            this[58, colIndex].Editor.EnableEdit = false;

                        this[60, colIndex].View = _vwDisabled; // Onsert
                        this[60, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[60, colIndex].Editor != null)
                            this[60, colIndex].Editor.EnableEdit = false;
                        break;
                    case 2: // Onsert
                        this[58, colIndex].View = _vwDisabled; // Stitch-In
                        this[58, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[58, colIndex].Editor != null)
                            this[58, colIndex].Editor.EnableEdit = false;

                        this[59, colIndex].View = _vwDisabled; // Blow-In
                        this[59, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[59, colIndex].Editor != null)
                            this[59, colIndex].Editor.EnableEdit = false;
                        break;
                    default:
                        this[58, colIndex].View = _vwDisabled; // Stitch-In
                        this[58, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[58, colIndex].Editor != null)
                            this[58, colIndex].Editor.EnableEdit = false;

                        this[59, colIndex].View = _vwDisabled; // Blow-In
                        this[59, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[59, colIndex].Editor != null)
                            this[59, colIndex].Editor.EnableEdit = false;

                        this[60, colIndex].View = _vwDisabled; // Onsert
                        this[60, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[60, colIndex].Editor != null)
                            this[60, colIndex].Editor.EnableEdit = false;
                        break;
                }
            }
            else
            {
                this[58, colIndex].View = _vwDisabled; // Stitch-In
                this[58, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                if (this[58, colIndex].Editor != null)
                    this[58, colIndex].Editor.EnableEdit = false;

                this[59, colIndex].View = _vwDisabled; // Blow-In
                this[59, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                if (this[59, colIndex].Editor != null)
                    this[59, colIndex].Editor.EnableEdit = false;

                this[60, colIndex].View = _vwDisabled; // Onsert
                this[60, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                if (this[60, colIndex].Editor != null)
                    this[60, colIndex].Editor.EnableEdit = false;
            }
            #endregion

            #region Set Cell Controllers
            //this[0, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            this[0, colIndex].AddController(new SourceGrid.Cells.Controllers.MouseInvalidate());
            this[0, colIndex].AddController(new SourceGrid.Cells.Controllers.Resizable(CellResizeMode.Width));
            this[0, colIndex].AddController(_menuPopup);
            this[0, colIndex].AddController(new SelectColumn()); // Column Header

            this[3, colIndex].AddController(_displayComments); // Comments
            this[4, colIndex].AddController(_displayFinancialComments); // Financial Comments

            if (c_row.est_componenttype_id == 1) // Host
                this[7, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            else // Non Host
                this[7, colIndex].AddController(new AssemblyRateController()); // ComponentType <> 1

            // When Vendor Supplied value changes, also populate the stitch-in, blow-in, onsert, plate cost and paper map dropdowns
            this[15, colIndex].AddController(_ctrVSCheckBox);
            this[15, colIndex].AddController(new PrinterController());
            this[15, colIndex].AddController(new PaperRateController());

            this[23, colIndex].AddController(new PrinterController()); // Printer Vendor
            this[24, colIndex].AddController(_ctrCalcPrintCostCheckBox); // Calc Print Cost
            this[24, colIndex].AddController(new PrinterController());

            this[32, colIndex].AddController(new StitcherMakereadyController()); // Stitcher Makeready
            this[33, colIndex].AddController(new ManualStitcherMakereadyController()); // Manual Stitcher Makeready
            this[34, colIndex].AddController(new PressMakereadyController()); // Press Makeready
            this[35, colIndex].AddController(new ManualPressMakereadyController()); // Manual Press Makeready

            this[37, colIndex].AddController(_ctrApplyTaxCheckBox); // Printer Apply Tax

            this[41, colIndex].AddController(new PaperController()); // Paper Vendor
            this[42, colIndex].AddController(new PaperRateController()); // Paper Weight
            this[43, colIndex].AddController(new PaperRateController()); // Paper Grade
            this[45, colIndex].AddController(_ctrCalcPaperCostCheckBox); // Calc Paper Cost
            this[53, colIndex].AddController(_ctrApplyTaxCheckBox); // Paper Apply Tax
            this[57, colIndex].AddController(new AssemblyRateController()); // Assembly Vendor

            // Add the Required Controller to track cell value changes in the parent form
            // Make all cells readonly if the form is readonly
            for (int i = 1; i < 63; ++i)
            {
                // Add the "Value Changed" controller to all cells (except for separator cells)
                if (i != 14 && i != 22 && i != 40 && i != 56 && i != 61)
                {
                    ValueChangedController _ctrValueChanged = new ValueChangedController();
                    this[i, colIndex].AddController(_ctrValueChanged);
                    _ctrValueChanged.CellValueChanged += new CellValueChanged(this.CellValueChanged);
                    _ctrValueChanged.CellValueChanged += new CellValueChanged(this.ColumnContainsData);
                    _ctrValueChanged.InitialValue = this[i, colIndex].Value;

                    this[i, colIndex].AddController(SelectCell.Default);
                    // Mark cells read-only
                    if (_readOnly)
                    {
                        this[i, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[i, colIndex].Editor != null)
                            this[i, colIndex].Editor.EnableEdit = false;

                        if (this[i, colIndex].GetType() == typeof(SourceGrid.Cells.CheckBox))
                            this[i, colIndex].View = _vwCheckBoxDisabled;
                        else if (this[i, colIndex].Editor is TextBoxNumeric || this[i, colIndex].Editor is NumericUpDown)
                            this[i, colIndex].View = _vwRightAlignDisabled;
                        else
                            this[i, colIndex].View = _vwDisabled;
                        if (this[i, colIndex].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) == null)
                            this[i, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    }
                }
            }
            #endregion
        }

        #endregion

        #region Add / Remove Components
        public void AddComponent()
        {
            _Components.Add(_dsEstimates.est_component.Newest_componentRow());
            AddColumn(_Components.Count);

            if (this.SelectionMode != GridSelectionMode.Cell)
            {
                this.SelectionMode = GridSelectionMode.Cell;
                this.Selection.EnableMultiSelection = false;
            }

            this.Selection.Focus(new Position(2, _Components.Count), true);
        }

        private void AddColumn(int colIndex)
        {
            this.ComponentAdded(this, new EventArgs());

            this.Columns.Insert(colIndex);
            this.Columns[colIndex].Width = 125;
            
            this[0, colIndex] = new SourceGrid.Cells.Cell("Component " + colIndex.ToString());
            //this[0, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            this[0, colIndex].AddController(new SourceGrid.Cells.Controllers.MouseInvalidate());
            this[0, colIndex].AddController(new SourceGrid.Cells.Controllers.Resizable(CellResizeMode.Width));
            this[0, colIndex].View = SourceGrid.Cells.Views.ColumnHeader.Default;
            this[0, colIndex].AddController(_menuPopup);
            this[0, colIndex].AddController(new SelectColumn());

            this[1, colIndex] = new SourceGrid.Cells.Cell(); // Component ID
            this[1, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            this[1, colIndex].View = _vwDisabled;

            this[2, colIndex] = new SourceGrid.Cells.Cell(String.Empty, _edDescription); // Description
            this[3, colIndex] = new SourceGrid.Cells.Cell(String.Empty, _edComments); // Comments
            this[3, colIndex].AddController(_displayComments);
            this[4, colIndex] = new SourceGrid.Cells.Cell(String.Empty, _edComments); // Financial Comments
            this[4, colIndex].AddController(_displayFinancialComments);
            this[5, colIndex] = new SourceGrid.Cells.Cell(String.Empty, _edAdNumber); // Ad Number
            this[6, colIndex] = new SourceGrid.Cells.Cell(null, new EstimateMediaTypeEditor(_dsEstimates)); // Estimate Media Type
            this[6, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            // The first component must be the HOST Component
            if (colIndex == 1)
            {
                this[7, colIndex] = new SourceGrid.Cells.Cell(new IntPair(1, "HOST"));
                this[7, colIndex].View = _vwDisabled;
                this[7, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            }
            // No other components can be of type HOST
            else
            {
                this[7, colIndex] = new SourceGrid.Cells.Cell(null, new ComponentTypeEditor(_dsEstimates)); // Component Type
                this[7, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
                this[7, colIndex].AddController(new AssemblyRateController());
            }
            this[8, colIndex] = new SourceGrid.Cells.Cell(String.Empty, new TextBoxIntegerRange(0, null)); // Media Qty w/o Insert
            this[8, colIndex].View = _vwRightAlignEnabled;

            this[9, colIndex] = new SourceGrid.Cells.Cell(String.Empty, new TextBoxPercentageRange(0, 1)); // Spoilage %
            this[9, colIndex].View = _vwRightAlignEnabled;

            this[10, colIndex] = new SourceGrid.Cells.Cell(null, new NumericUpDown(typeof(int), 999, 1, 1));  // Page Count
            this[10, colIndex].View = _vwRightAlignEnabled;
            this[10, colIndex].Editor.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            this[11, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxDecimalRange(0, _cMaxWidth));  // Width
            this[11, colIndex].View = _vwRightAlignEnabled;
            this[12, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxDecimalRange(0, _cMaxHeight));  // Height
            this[12, colIndex].View = _vwRightAlignEnabled;
            this[13, colIndex] = new SourceGrid.Cells.Cell(null, new NumericUpDown(typeof(int), 999, 0, 1)); // Number of Plants
            this[13, colIndex].View = _vwRightAlignEnabled;
            this[13, colIndex].Editor.AllowNull = true;
            this[13, colIndex].Editor.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            this[14, colIndex] = null; // Blank Line
            this[15, colIndex] = new SourceGrid.Cells.CheckBox();  // Vendor Supplied?
            // When Vendor Supplied value changes, also populate the stitch-in, blow-in, onsert, plate cost and paper map dropdowns
            this[15, colIndex].AddController(_ctrVSCheckBox);
            this[15, colIndex].AddController(new PrinterController());
            this[15, colIndex].AddController(new PaperRateController());
            this[16, colIndex] = new SourceGrid.Cells.Cell(null, new VendorEditor(_dsEstimates, 9, null));  // VS Vendor
            this[16, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            this[16, colIndex].Editor.EnableEdit = false;
            this[16, colIndex].Editor.AllowNull = true;
            this[16, colIndex].View = _vwDisabled;
            this[17, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));  // VS CPM
            this[17, colIndex].View = _vwRightAlignEnabled;
            this[17, colIndex].Editor.AllowNull = true;
            this[17, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            this[17, colIndex].View = _vwDisabled;
            this[17, colIndex].Editor.EnableEdit = false;
            this[18, colIndex] = new SourceGrid.Cells.Cell(null, new VendorEditor(_dsEstimates, 3, null)); // Creative Vendor
            this[18, colIndex].Editor.AllowNull = true;
            this[18, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            this[19, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));  // Creative CPP
            this[19, colIndex].View = _vwRightAlignEnabled;
            this[19, colIndex].Editor.AllowNull = true;
            this[20, colIndex] = new SourceGrid.Cells.Cell(null, new VendorEditor(_dsEstimates, 4, null)); // Separator Vendor
            this[20, colIndex].Editor.AllowNull = true;
            this[20, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            this[21, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));  // Separator CPP
            this[21, colIndex].View = _vwRightAlignEnabled;
            this[21, colIndex].Editor.AllowNull = true;
            this[22, colIndex] = null; // Blank Line

            this[23, colIndex] = new SourceGrid.Cells.Cell(null, new PrinterEditor(_dsEstimates, _dsEstimates.est_estimate[0].rundate, null));  // Printer Vendor
            this[23, colIndex].Editor.AllowNull = true;
            this[23, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            this[23, colIndex].AddController(new PrinterController());
            this[24, colIndex] = new SourceGrid.Cells.CheckBox(String.Empty, true);  // Calc Print Cost
            this[24, colIndex].AddController(_ctrCalcPrintCostCheckBox);
            this[24, colIndex].AddController(new PrinterController());
            this[25, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Print Cost
            this[25, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            this[25, colIndex].View = _vwRightAlignDisabled;
            this[26, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxIntegerRange(0, int.MaxValue)); // Additional Plates
            this[26, colIndex].View = _vwRightAlignEnabled;
            this[26, colIndex].Editor.AllowNull = true;
            this[27, colIndex] = new SourceGrid.Cells.Cell(); // Plate Cost
            this[27, colIndex].View = _vwDisabled;
            this[28, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Replacement Plate Cost
            this[28, colIndex].View = _vwRightAlignEnabled;
            this[28, colIndex].Editor.AllowNull = true;
            this[29, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Run Rate
            this[29, colIndex].View = _vwRightAlignEnabled;
            this[29, colIndex].Editor.AllowNull = true;
            this[30, colIndex] = new SourceGrid.Cells.Cell(null, new NumericUpDown(typeof(int), 999, 0, 1)); // Number Digital Handle & Prepare
            this[30, colIndex].View = _vwRightAlignEnabled;
            this[30, colIndex].Editor.AllowNull = true;
            this[30, colIndex].Editor.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            this[31, colIndex] = new SourceGrid.Cells.Cell(); // Digital H&P Rate
            this[31, colIndex].View = _vwDisabled;
            this[32, colIndex] = new SourceGrid.Cells.Cell(); // Stitcher Makeready
            this[32, colIndex].AddController(new StitcherMakereadyController());
            this[33, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Manual Stitcher Makeready Rate
            this[33, colIndex].View = _vwRightAlignEnabled;
            this[33, colIndex].AddController(new ManualStitcherMakereadyController());
            this[33, colIndex].Editor.AllowNull = true;
            this[34, colIndex] = new SourceGrid.Cells.Cell(); // Press Makeready
            this[34, colIndex].AddController(new PressMakereadyController());
            this[35, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Manual Press Makeready Rate
            this[35, colIndex].View = _vwRightAlignEnabled;
            this[35, colIndex].AddController(new ManualPressMakereadyController());
            this[35, colIndex].Editor.AllowNull = true;
            this[36, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxPercentageRange(0, 1)); // Early Pay Discount
            this[36, colIndex].View = _vwRightAlignEnabled;
            this[36, colIndex].Editor.AllowNull = true;
            this[37, colIndex] = new SourceGrid.Cells.CheckBox(); // Apply Tax
            this[37, colIndex].AddController(_ctrApplyTaxCheckBox);
            this[38, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxPercentageRange(0, 1)); // Taxable Media
            this[38, colIndex].Editor.AllowNull = true;
            this[38, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            this[38, colIndex].View = _vwRightAlignDisabled;
            this[39, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxPercentageRange(0, 1)); // Sales Tax
            this[39, colIndex].Editor.AllowNull = true;
            this[39, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            this[39, colIndex].View = _vwRightAlignDisabled;
            this[39, colIndex].Editor.EnableEdit = false;
            this[40, colIndex] = null; // Blank Line
            this[41, colIndex] = new SourceGrid.Cells.Cell(null, new PaperEditor(_dsEstimates, _dsEstimates.est_estimate[0].rundate, null)); // Paper Vendor
            this[41, colIndex].Editor.AllowNull = true;
            this[41, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            this[41, colIndex].AddController(new PaperController());
            this[42, colIndex] = new SourceGrid.Cells.Cell(null, new PaperWeightEditor(_dsEstimates)); // Paper Weight
            this[42, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            this[42, colIndex].AddController(new PaperRateController());
            this[43, colIndex] = new SourceGrid.Cells.Cell(null, new PaperGradeEditor(_dsEstimates)); // Paper Grade;
            this[43, colIndex].Editor.AllowNull = true;
            this[43, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            this[43, colIndex].AddController(new PaperRateController());
            this[44, colIndex] = new SourceGrid.Cells.Cell(); // Paper Description
            this[44, colIndex].View = _vwDisabled;
            this[45, colIndex] = new SourceGrid.Cells.CheckBox(String.Empty, true); // Calc Paper Cost
            this[45, colIndex].AddController(_ctrCalcPaperCostCheckBox);
            this[46, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, null)); // Paper Cost
            this[46, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            this[46, colIndex].View = _vwRightAlignDisabled;
            this[47, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxDecimalRange(0, _cMaxRunPounds, 2)); // Run Pounds
            this[47, colIndex].View = _vwRightAlignEnabled;
            this[47, colIndex].Editor.AllowNull = true;
            this[48, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxIntegerRange(0, _cMaxMakereadyPounds)); // Makeready Pounds
            this[48, colIndex].View = _vwRightAlignEnabled;
            this[48, colIndex].Editor.AllowNull = true;
            this[49, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxDecimalRange(0, _cMaxPlateChangePounds, 2)); // Plate Change Pounds
            this[49, colIndex].View = _vwRightAlignEnabled;
            this[49, colIndex].Editor.AllowNull = true;
            this[50, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxIntegerRange(0, _cMaxPressStopPounds)); // Press Stop Pounds
            this[50, colIndex].View = _vwRightAlignEnabled;
            this[50, colIndex].Editor.AllowNull = true;
            this[51, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxIntegerRange(0, int.MaxValue)); // Number of Press Stops
            this[51, colIndex].View = _vwRightAlignEnabled;
            this[51, colIndex].Editor.AllowNull = true;
            this[52, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxPercentageRange(0, 1)); // Early Pay Discount
            this[52, colIndex].View = _vwRightAlignEnabled;
            this[52, colIndex].Editor.AllowNull = true;
            this[53, colIndex] = new SourceGrid.Cells.CheckBox(); // Apply Tax
            this[53, colIndex].AddController(_ctrApplyTaxCheckBox);
            this[54, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxPercentageRange(0, 1)); // Taxable Media
            this[54, colIndex].Editor.AllowNull = true;
            this[54, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            this[54, colIndex].View = _vwRightAlignDisabled;
            this[54, colIndex].Editor.EnableEdit = false;
            this[55, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxPercentageRange(0, 1)); // Sales Tax
            this[55, colIndex].Editor.AllowNull = true;
            this[55, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            this[55, colIndex].View = _vwRightAlignDisabled;
            this[55, colIndex].Editor.EnableEdit = false;
            this[56, colIndex] = null; // Blank Line
            this[57, colIndex] = new SourceGrid.Cells.Cell(null, new PrinterEditor(_dsEstimates, _dsEstimates.est_estimate[0].rundate, null)); // Assembly Vendor
            this[57, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            this[57, colIndex].AddController(new AssemblyRateController());
            this[58, colIndex] = new SourceGrid.Cells.Cell();  // Stitch-In Rate
            this[58, colIndex].View = _vwDisabled;
            this[59, colIndex] = new SourceGrid.Cells.Cell();  // Blow-In Rate
            this[59, colIndex].View = _vwDisabled;
            this[60, colIndex] = new SourceGrid.Cells.Cell();  // Onsert Rate
            this[60, colIndex].View = _vwDisabled;
            this[61, colIndex] = null; // Blank Line
            this[62, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Other Production
            this[62, colIndex].View = _vwRightAlignEnabled;
            this[62, colIndex].Editor.AllowNull = true;

            // Add the Required Controller to track cell value changes in the parent form (except for separator cells)
            // Make all cells readonly if the form is readonly
            for (int i = 1; i < 63; ++i)
            {
                if (i != 14 && i != 22 && i != 40 && i != 56 && i != 61)
                {
                    ValueChangedController _ctrValueChanged = new ValueChangedController();
                    _ctrValueChanged.InitialValue = this[i, colIndex].Value;
                    _ctrValueChanged.CellValueChanged += new CellValueChanged(this.CellValueChanged);
                    _ctrValueChanged.CellValueChanged += new CellValueChanged(this.ColumnContainsData);
                    this[i, colIndex].AddController(_ctrValueChanged);
                    this[i, colIndex].AddController(SelectCell.Default);

                    // Mark cells read-only
                    if (_readOnly)
                    {
                        this[i, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[i, colIndex].Editor != null)
                            this[i, colIndex].Editor.EnableEdit = false;

                        if (this[i, colIndex].GetType() == typeof(SourceGrid.Cells.CheckBox))
                            this[i, colIndex].View = _vwCheckBoxDisabled;
                        else if (this[i, colIndex].Editor is TextBoxNumeric || this[i, colIndex].Editor is NumericUpDown)
                            this[i, colIndex].View = _vwRightAlignDisabled;
                        else
                            this[i, colIndex].View = _vwDisabled;
                        if (this[i, colIndex].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) == null)
                            this[i, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    }
                }
            }

            // The host component is never "blank"
            if (colIndex == 1)
                _blankColumn.Add(false);
            // Any other new column begins as a "blank component"
            else
                _blankColumn.Add(true);
        }

        public void DeleteColumn(int colIndex)
        {
            CancelEventArgs cea = new CancelEventArgs();
            this.ComponentRemoving(_Components[colIndex - 1].est_component_id, cea);
            if (cea.Cancel)
                return;

            this.ComponentRemoved(this, new EventArgs());

            // Delete the component from the dataset
            _Components[colIndex - 1].Delete();
            // Delete the component from the list of components
            _Components.Remove(_Components[colIndex - 1]);
            _blankColumn.Remove(_blankColumn[colIndex - 1]);
            // Delete the column from the grid
            this.Columns.Remove(colIndex);

            // Renumber the Components
            for (int i = 1; i < this.ColumnsCount; ++i)
                this[0, i].Value = "Component " + i.ToString();
        }

        public void CutSelectedComponents()
        {
            if (this.SelectionMode != GridSelectionMode.Column)
                return;

            if (this.Selection.IsSelectedColumn(1))
                return;

            CopySelectedComponentsToClipboard();

            List<int> selectedColumns = new List<int>();

            for (int colIndex = (this.ColumnsCount - 1); colIndex > 0; --colIndex)
                if (this.Selection.IsSelectedColumn(colIndex))
                    selectedColumns.Add(colIndex);

            foreach(int selected_colIndex in selectedColumns)
                DeleteColumn(selected_colIndex);

            this.Selection.ResetSelection(false);
        }

        public void CopySelectedComponents()
        {
            if (this.SelectionMode != GridSelectionMode.Column)
                return;

            CopySelectedComponentsToClipboard();
        }

        public void CopySelectedComponentsToClipboard()
        {
            List<CopyComponent> componentList = new List<CopyComponent>();

            for (int colIndex = 1; colIndex < this.ColumnsCount; ++colIndex)
            {
                if (this.Selection.IsSelectedColumn(colIndex))
                    componentList.Add(CreateCopyComponent(colIndex));
            }

            if (componentList.Count > 0)
                System.Windows.Forms.Clipboard.SetDataObject(componentList);
        }

        private CopyComponent CreateCopyComponent(int colIndex)
        {
            CopyComponent componentCopy = new CopyComponent();
            componentCopy.Description = this[2, colIndex].Value.ToString();
            if (this[3, colIndex].Value != null)
                componentCopy.Comments = this[3, colIndex].Value.ToString();
            if (this[4, colIndex].Value != null)
                componentCopy.FinancialChangeComments = this[4, colIndex].Value.ToString();
            if (this[5, colIndex].Value == null || this[5, colIndex].Value.ToString() == string.Empty)
                componentCopy.AdNumber = null;
            else
                componentCopy.AdNumber = (int)this[5, colIndex].Value;
            if (this[6, colIndex].Value == null)
                componentCopy.EstimateMediaTypeID = null;
            else
                componentCopy.EstimateMediaTypeID = ((IntPair)this[6, colIndex].Value).Value;
            if (this[7, colIndex].Value == null)
                componentCopy.ComponentTypeID = null;
            else
                componentCopy.ComponentTypeID = ((IntPair)this[7, colIndex].Value).Value;
            if (this[8, colIndex].Value == null || this[8, colIndex].Value.ToString() == string.Empty)
                componentCopy.MediaQtywoInsert = null;
            else
                componentCopy.MediaQtywoInsert = (int)this[8, colIndex].Value;
            if (this[9, colIndex].Value == null || this[9, colIndex].Value.ToString() == string.Empty)
                componentCopy.SpoilagePct = null;
            else
                componentCopy.SpoilagePct = (decimal)this[9, colIndex].Value;
            if (this[10, colIndex].Value == null || this[10, colIndex].Value.ToString() == string.Empty)
                componentCopy.PageCount = null;
            else
                componentCopy.PageCount = (int)this[10, colIndex].Value;
            if (this[11, colIndex].Value == null || this[11, colIndex].Value.ToString() == string.Empty)
                componentCopy.Width = null;
            else
                componentCopy.Width = (decimal)this[11, colIndex].Value;
            if (this[12, colIndex].Value == null || this[12, colIndex].Value.ToString() == string.Empty)
                componentCopy.Height = null;
            else
                componentCopy.Height = (decimal)this[12, colIndex].Value;
            if (this[13, colIndex].Value == null || this[13, colIndex].Value.ToString() == string.Empty)
                componentCopy.NumberofPlants = null;
            else
                componentCopy.NumberofPlants = (int)this[13, colIndex].Value;
            componentCopy.VendorSupplied = (bool)this[15, colIndex].Value;
            if (this[16, colIndex].Value == null || this[16, colIndex].Value.ToString() == string.Empty)
                componentCopy.VendorSuppliedDesc = null;
            else
                componentCopy.VendorSuppliedDesc = ((LongPair)this[16, colIndex].Value).Display;
            if (this[17, colIndex].Value == null || this[17, colIndex].Value.ToString() == string.Empty)
                componentCopy.VendorSuppliedCPM = null;
            else
                componentCopy.VendorSuppliedCPM = (decimal)this[17, colIndex].Value;
            if (this[18, colIndex].Value == null || this[18, colIndex].Value.ToString() == string.Empty)
                componentCopy.CreativeDesc = null;
            else
                componentCopy.CreativeDesc = ((LongPair)this[18, colIndex].Value).Display;
            if (this[19, colIndex].Value == null || this[19, colIndex].Value.ToString() == string.Empty)
                componentCopy.CreativeCPP = null;
            else
                componentCopy.CreativeCPP = (decimal)this[19, colIndex].Value;
            if (this[20, colIndex].Value == null || this[20, colIndex].Value.ToString() == string.Empty)
                componentCopy.SeparatorDesc = null;
            else
                componentCopy.SeparatorDesc = ((LongPair)this[20, colIndex].Value).Display;
            if (this[21, colIndex].Value == null || this[21, colIndex].Value.ToString() == string.Empty)
                componentCopy.SeparatorCPP = null;
            else
                componentCopy.SeparatorCPP = (decimal)this[21, colIndex].Value;
            if (this[23, colIndex].Value == null || this[23, colIndex].Value.ToString() == string.Empty)
                componentCopy.PrinterDesc = null;
            else
                componentCopy.PrinterDesc = ((LongPair)this[23, colIndex].Value).Display;
            componentCopy.CalcPrinterCost = (bool)this[24, colIndex].Value;
            if (this[25, colIndex].Value == null || this[25, colIndex].Value.ToString() == string.Empty)
                componentCopy.ManualPrinterCost = null;
            else
                componentCopy.ManualPrinterCost = (decimal)this[25, colIndex].Value;
            if (this[26, colIndex].Value == null || this[26, colIndex].Value.ToString() == string.Empty)
                componentCopy.AdditionalPlates = null;
            else
                componentCopy.AdditionalPlates = (int) this[26, colIndex].Value;
            if (this[27, colIndex].Value == null || this[27, colIndex].Value.ToString() == string.Empty)
                componentCopy.PlateCostDesc = null;
            else
                componentCopy.PlateCostDesc = ((LongPair)this[27, colIndex].Value).Display;
            if (this[28, colIndex].Value == null || this[28, colIndex].Value.ToString() == string.Empty)
                componentCopy.ReplacementPlateCost = null;
            else
                componentCopy.ReplacementPlateCost = (decimal)this[28, colIndex].Value;
            if (this[29, colIndex].Value == null || this[29, colIndex].Value.ToString() == string.Empty)
                componentCopy.RunRate = null;
            else
                componentCopy.RunRate = (decimal)this[29, colIndex].Value;
            if (this[30, colIndex].Value == null || this[30, colIndex].Value.ToString() == string.Empty)
                componentCopy.NumberDigitalHandleandPrepare = null;
            else
                componentCopy.NumberDigitalHandleandPrepare = (int)this[30, colIndex].Value;
            if (this[31, colIndex].Value == null || this[31, colIndex].Value.ToString() == string.Empty)
                componentCopy.DigitalHandleandPrepareDesc = null;
            else
                componentCopy.DigitalHandleandPrepareDesc = ((LongPair)this[31, colIndex].Value).Display;
            if (this[32, colIndex].Value == null || this[32, colIndex].Value.ToString() == string.Empty)
                componentCopy.StitcherMakereadyDesc = null;
            else
                componentCopy.StitcherMakereadyDesc = ((LongPair)this[32, colIndex].Value).Display;
            if (this[33, colIndex].Value == null)
                componentCopy.StitcherMakereadyRate = null;
            else
                componentCopy.StitcherMakereadyRate = (decimal)this[33, colIndex].Value;
            if (this[34, colIndex].Value == null || this[34, colIndex].Value.ToString() == string.Empty)
                componentCopy.PressMakereadyDesc = null;
            else
                componentCopy.PressMakereadyDesc = ((LongPair)this[34, colIndex].Value).Display;
            if (this[35, colIndex].Value == null)
                componentCopy.PressMakereadyRate = null;
            else
                componentCopy.PressMakereadyRate = (decimal)this[35, colIndex].Value;
            if (this[36, colIndex].Value == null || this[36, colIndex].Value.ToString() == string.Empty)
                componentCopy.EarlyPayPrintDiscount = null;
            else
                componentCopy.EarlyPayPrintDiscount = (decimal)this[36, colIndex].Value;
            componentCopy.PrinterApplyTax = (bool)this[37, colIndex].Value;
            if (this[38, colIndex].Value == null || this[38, colIndex].Value.ToString() == string.Empty)
                componentCopy.PrinterTaxableMediaPct = null;
            else
                componentCopy.PrinterTaxableMediaPct = (decimal)this[38, colIndex].Value;
            if (this[39, colIndex].Value == null || this[39, colIndex].Value.ToString() == string.Empty)
                componentCopy.PrinterSalesTaxPct = null;
            else
                componentCopy.PrinterSalesTaxPct = (decimal)this[39, colIndex].Value;
            if (this[41, colIndex].Value == null || this[41, colIndex].Value.ToString() == string.Empty)
                componentCopy.PaperDesc = null;
            else
                componentCopy.PaperDesc = ((LongPair)this[41, colIndex].Value).Display;
            if (this[42, colIndex].Value == null || this[42, colIndex].Value.ToString() == string.Empty)
                componentCopy.PaperWeight = null;
            else
                componentCopy.PaperWeight = Convert.ToInt32(((IntPair)this[42, colIndex].Value).Display);
            if (this[43, colIndex].Value == null || this[43, colIndex].Value.ToString() == string.Empty)
                componentCopy.PaperGrade = null;
            else
                componentCopy.PaperGrade = ((IntPair)this[43, colIndex].Value).Display;
            if (this[44, colIndex].Value == null)
                componentCopy.PaperMapDesc = null;
            else
                componentCopy.PaperMapDesc = ((LongPair)this[44, colIndex].Value).Display;
            componentCopy.CalcPaperCost = (bool)this[45, colIndex].Value;
            if (this[46, colIndex].Value == null || this[46, colIndex].Value.ToString() == string.Empty)
                componentCopy.ManualPaperCost = null;
            else
                componentCopy.ManualPaperCost = (decimal)this[46, colIndex].Value;
            if (this[47, colIndex].Value == null || this[47, colIndex].Value.ToString() == string.Empty)
                componentCopy.RunPounds = null;
            else
                componentCopy.RunPounds = (decimal)this[47, colIndex].Value;
            if (this[48, colIndex].Value == null || this[48, colIndex].Value.ToString() == string.Empty)
                componentCopy.MakereadyPounds = null;
            else
                componentCopy.MakereadyPounds = (int)this[48, colIndex].Value;
            if (this[49, colIndex].Value == null || this[49, colIndex].Value.ToString() == string.Empty)
                componentCopy.PlateChangePounds = null;
            else
                componentCopy.PlateChangePounds = (decimal)this[49, colIndex].Value;
            if (this[50, colIndex].Value == null || this[50, colIndex].Value.ToString() == string.Empty)
                componentCopy.PressStopPounds = null;
            else
                componentCopy.PressStopPounds = (int)this[50, colIndex].Value;
            if (this[51, colIndex].Value == null || this[51, colIndex].Value.ToString() == string.Empty)
                componentCopy.NumberofPressStops = null;
            else
                componentCopy.NumberofPressStops = (int)this[51, colIndex].Value;
            if (this[52, colIndex].Value == null || this[52, colIndex].Value.ToString() == string.Empty)
                componentCopy.EarlyPayPaperDiscount = null;
            else
                componentCopy.EarlyPayPaperDiscount = (decimal)this[52, colIndex].Value;
            componentCopy.PaperApplyTax = (bool)this[53, colIndex].Value;
            if (this[54, colIndex].Value == null || this[54, colIndex].Value.ToString() == string.Empty)
                componentCopy.PaperTaxableMediaPct = null;
            else
                componentCopy.PaperTaxableMediaPct = (decimal)this[54, colIndex].Value;
            if (this[55, colIndex].Value == null || this[55, colIndex].Value.ToString() == string.Empty)
                componentCopy.PaperSalesTaxPct = null;
            else
                componentCopy.PaperSalesTaxPct = (decimal)this[55, colIndex].Value;
            if (this[57, colIndex].Value == null || this[57, colIndex].Value.ToString() == string.Empty)
                componentCopy.AssemblyVendorDesc = null;
            else
                componentCopy.AssemblyVendorDesc = ((LongPair)this[57, colIndex].Value).Display;
            if (this[58, colIndex].Value == null || this[58, colIndex].Value.ToString() == string.Empty)
                componentCopy.StitchInDesc = null;
            else
                componentCopy.StitchInDesc = ((LongPair)this[58, colIndex].Value).Display;
            if (this[59, colIndex].Value == null || this[59, colIndex].Value.ToString() == string.Empty)
                componentCopy.BlowInDesc = null;
            else
                componentCopy.BlowInDesc = ((LongPair)this[59, colIndex].Value).Display;
            if (this[60, colIndex].Value == null || this[60, colIndex].Value.ToString() == string.Empty)
                componentCopy.OnsertDesc = null;
            else
                componentCopy.OnsertDesc = ((LongPair)this[60, colIndex].Value).Display;
            if (this[62, colIndex].Value == null || this[62, colIndex].Value.ToString() == string.Empty)
                componentCopy.OtherProduction = null;
            else
                componentCopy.OtherProduction = (decimal)this[62, colIndex].Value;

            return componentCopy;
        }

        public void PasteComponents()
        {
            if (this.SelectionMode != GridSelectionMode.Column)
                return;

            if (!System.Windows.Forms.Clipboard.GetDataObject().GetDataPresent(typeof(List<CopyComponent>)))
                return;

            List<CopyComponent> componentList = (List<CopyComponent>)System.Windows.Forms.Clipboard.GetDataObject().GetData(typeof(List<CopyComponent>));

            // Do not perform paste operation if number of components on clipboard does not match the selection
            if (componentList.Count != this.SelectedComponentCount)
                return;

            // Do not perform the paste operation if user is attempting to paste a host component in any column other than first
            int compIndex = 0;
            for (int colIndex = 1; colIndex < this.ColumnsCount; ++colIndex)
            {
                if (this.Selection.IsSelectedColumn(colIndex))
                {
                    if (this[7, colIndex].Value != null
                        && ((((IntPair)this[7, colIndex].Value).Value == 1 && componentList[compIndex].ComponentTypeID != 1)
                        || (((IntPair)this[7, colIndex].Value).Value != 1 && componentList[compIndex].ComponentTypeID == 1)))
                        return;

                    ++compIndex;
                }
            }

            int clipboardItemIndex = 0;
            for (int colIndex = 1; colIndex < this.ColumnsCount; ++colIndex)
            {
                if (this.Selection.IsSelectedColumn(colIndex))
                {
                    SetComponentfromCopyComponent(componentList[clipboardItemIndex], colIndex);
                    ++clipboardItemIndex;
                }
            }
        }

        private void SetComponentfromCopyComponent(CopyComponent copyComponent, int colIndex)
        {
            // Clear anything that was previously in the specified column
            for (int i = 2; i < 63; ++i)
                this[i, colIndex] = null;

            #region Set Cell Editors and Values
            this[2, colIndex] = new SourceGrid.Cells.Cell(copyComponent.Description, _edDescription); // Description
            this[3, colIndex] = new SourceGrid.Cells.Cell(copyComponent.Comments, _edComments); // Comments
            this[4, colIndex] = new SourceGrid.Cells.Cell(copyComponent.FinancialChangeComments, _edComments); // Financial Comments
            this[5, colIndex] = new SourceGrid.Cells.Cell(copyComponent.AdNumber, _edAdNumber); // Ad Number
            this[5, colIndex].View = _vwRightAlignEnabled;

            EstimateMediaTypeEditor edEstimateMediaType = new EstimateMediaTypeEditor(_dsEstimates);
            edEstimateMediaType.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            if (copyComponent.EstimateMediaTypeID.HasValue)
                this[6, colIndex] = new SourceGrid.Cells.Cell(edEstimateMediaType.GetEstimateMediaTypefromID(copyComponent.EstimateMediaTypeID.Value), edEstimateMediaType); // Estimate Media Type
            else
                this[6, colIndex] = new SourceGrid.Cells.Cell(null, edEstimateMediaType); // Estimate Media Type
            this[6, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default; // Estimate Media Type

            ComponentTypeEditor edComponentType = new ComponentTypeEditor(_dsEstimates);
            edComponentType.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            IntPair curComponentType = null;

            // The first component must be the HOST Component
            if (colIndex == 1)
            {
                this[7, colIndex] = new SourceGrid.Cells.Cell(new IntPair(1, "HOST"));
                this[7, colIndex].View = _vwDisabled;
                curComponentType = new IntPair(1, "HOST");
            }
            // No other components can be of type HOST
            else
            {
                if (copyComponent.ComponentTypeID.HasValue)
                    curComponentType = edComponentType.GetComponentTypefromID(copyComponent.ComponentTypeID.Value);

                this[7, colIndex] = new SourceGrid.Cells.Cell(curComponentType, edComponentType);
                this[7, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            }

            if (copyComponent.MediaQtywoInsert.HasValue)
                this[8, colIndex] = new SourceGrid.Cells.Cell(copyComponent.MediaQtywoInsert, new TextBoxIntegerRange(0, null)); // Media Qty w/o Insert
            else
                this[8, colIndex] = new SourceGrid.Cells.Cell(String.Empty, new TextBoxIntegerRange(0, null)); // Media Qty w/o Insert
            this[8, colIndex].View = _vwRightAlignEnabled;

            if (copyComponent.SpoilagePct.HasValue)
                this[9, colIndex] = new SourceGrid.Cells.Cell(copyComponent.SpoilagePct.Value, new TextBoxPercentageRange(0, 1));
            else
                this[9, colIndex] = new SourceGrid.Cells.Cell(string.Empty, new TextBoxPercentageRange(0, 1));
            this[9, colIndex].View = _vwRightAlignEnabled;

            if (copyComponent.PageCount.HasValue)
                this[10, colIndex] = new SourceGrid.Cells.Cell(copyComponent.PageCount.Value, new NumericUpDown(typeof(int), 999, 1, 1));  // Page Count
            else
                this[10, colIndex] = new SourceGrid.Cells.Cell(null, new NumericUpDown(typeof(int), 999, 1, 1));  // Page Count
            this[10, colIndex].View = _vwRightAlignEnabled;
            this[10, colIndex].Editor.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;

            if (copyComponent.Width.HasValue)
                this[11, colIndex] = new SourceGrid.Cells.Cell(copyComponent.Width.Value, new TextBoxDecimalRange(0, _cMaxWidth));  // Width
            else
                this[11, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxDecimalRange(0, _cMaxWidth));  // Width
            this[11, colIndex].View = _vwRightAlignEnabled;

            if (copyComponent.Height.HasValue)
                this[12, colIndex] = new SourceGrid.Cells.Cell(copyComponent.Height.Value, new TextBoxDecimalRange(0, _cMaxHeight));  // Height
            else
                this[12, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxDecimalRange(0, _cMaxHeight));  // Height
            this[12, colIndex].View = _vwRightAlignEnabled;
            if (copyComponent.NumberofPlants.HasValue)
                this[13, colIndex] = new SourceGrid.Cells.Cell(copyComponent.NumberofPlants.Value, new NumericUpDown(typeof(int), 999, 0, 1)); // Number of Plants
            else
                this[13, colIndex] = new SourceGrid.Cells.Cell(null, new NumericUpDown(typeof(int), 999, 0, 1)); // Number of Plants
            this[13, colIndex].View = _vwRightAlignEnabled;
            this[13, colIndex].Editor.AllowNull = true;
            this[13, colIndex].Editor.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;

            this[14, colIndex] = null; // Blank Line
            this[15, colIndex] = new SourceGrid.Cells.CheckBox(string.Empty, copyComponent.VendorSupplied);  // Vendor Supplied?

            VendorEditor edVSVendor = new VendorEditor(_dsEstimates, 9, null);
            edVSVendor.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            if (string.IsNullOrEmpty(copyComponent.VendorSuppliedDesc))
            {
                this[16, colIndex] = new SourceGrid.Cells.Cell(null, edVSVendor);
            }
            else
            {
                this[16, colIndex] = new SourceGrid.Cells.Cell(edVSVendor.GetVendorfromDesc(copyComponent.VendorSuppliedDesc), edVSVendor);
                this[16, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            }
            this[16, colIndex].Editor.AllowNull = true;

            if (copyComponent.VendorSuppliedCPM.HasValue) // Vendor Supplied CPM
                this[17, colIndex] = new SourceGrid.Cells.Cell(copyComponent.VendorSuppliedCPM.Value, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));
            else
                this[17, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));
            this[17, colIndex].View = _vwRightAlignEnabled;
            this[17, colIndex].Editor.AllowNull = true;

            VendorEditor edCreative = new VendorEditor(_dsEstimates, 3, null); // Creative Vendor
            edCreative.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            if (string.IsNullOrEmpty(copyComponent.CreativeDesc))
                this[18, colIndex] = new SourceGrid.Cells.Cell(null, edCreative);
            else
                this[18, colIndex] = new SourceGrid.Cells.Cell(edCreative.GetVendorfromDesc(copyComponent.CreativeDesc), edCreative);
            this[18, colIndex].Editor.AllowNull = true;
            this[18, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;

            if (copyComponent.CreativeCPP.HasValue)
                this[19, colIndex] = new SourceGrid.Cells.Cell(copyComponent.CreativeCPP.Value, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));  // Creative CPP
            else
                this[19, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));  // Creative CPP
            this[19, colIndex].View = _vwRightAlignEnabled;
            this[19, colIndex].Editor.AllowNull = true;

            VendorEditor edSeparator = new VendorEditor(_dsEstimates, 4, null);
            edSeparator.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            if (string.IsNullOrEmpty(copyComponent.SeparatorDesc))
                this[20, colIndex] = new SourceGrid.Cells.Cell(null, edSeparator); // Separator Vendor
            else
                this[20, colIndex] = new SourceGrid.Cells.Cell(edSeparator.GetVendorfromDesc(copyComponent.SeparatorDesc), edSeparator); // Separator Vendor
            this[20, colIndex].Editor.AllowNull = true;
            this[20, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;

            if (copyComponent.SeparatorCPP.HasValue)
                this[21, colIndex] = new SourceGrid.Cells.Cell(copyComponent.SeparatorCPP.Value, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));  // Separator CPP
            else
                this[21, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));  // Separator CPP
            this[21, colIndex].View = _vwRightAlignEnabled;
            this[21, colIndex].Editor.AllowNull = true;

            this[22, colIndex] = null; // Blank Line

            PrinterEditor edPrinter = new PrinterEditor(_dsEstimates, _dsEstimates.est_estimate[0].rundate, null); // Printer Vendor
            edPrinter.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            if (string.IsNullOrEmpty(copyComponent.PrinterDesc))
            {
                this[23, colIndex] = new SourceGrid.Cells.Cell(null, edPrinter);
            }
            else
            {
                this[23, colIndex] = new SourceGrid.Cells.Cell(edPrinter.GetPrinterfromDesc(copyComponent.PrinterDesc), edPrinter);
            }
            this[23, colIndex].Editor.AllowNull = true;
            this[23, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;

            this[24, colIndex] = new SourceGrid.Cells.CheckBox(string.Empty, copyComponent.CalcPrinterCost); // Calc Print Cost

            if (copyComponent.ManualPrinterCost.HasValue)
                this[25, colIndex] = new SourceGrid.Cells.Cell(copyComponent.ManualPrinterCost.Value, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Manual Print Cost
            else
                this[25, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));
            this[25, colIndex].View = _vwRightAlignEnabled;



            if (copyComponent.AdditionalPlates.HasValue)
                this[26, colIndex] = new SourceGrid.Cells.Cell(copyComponent.AdditionalPlates.Value, new TextBoxIntegerRange(0, int.MaxValue)); // Additional Plates
            else
                this[26, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxIntegerRange(0, int.MaxValue)); // Additional Plates
            this[26, colIndex].View = _vwRightAlignEnabled;
            this[26, colIndex].Editor.AllowNull = true;

            LongPair curPrinter = this[23, colIndex].Value as LongPair;
            // Plate Cost
            if (curPrinter != null && !copyComponent.VendorSupplied && copyComponent.CalcPrinterCost)
            {
                PrinterRateEditor edPlateCost = new PrinterRateEditor(_dsEstimates, curPrinter.Value, 8);
                edPlateCost.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
                LongPair curPlateCost = null;
                if (string.IsNullOrEmpty(copyComponent.PlateCostDesc) || edPlateCost.FindRatefromDesc(copyComponent.PlateCostDesc) == null)
                    curPlateCost = edPlateCost.DefaultPrinterRate;
                else
                    curPlateCost = edPlateCost.FindRatefromDesc(copyComponent.PlateCostDesc);

                this[27, colIndex] = new SourceGrid.Cells.Cell(curPlateCost, edPlateCost);
                this[27, colIndex].Editor.AllowNull = true;
                this[27, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            }
            else
            {
                this[27, colIndex] = new SourceGrid.Cells.Cell();
                this[27, colIndex].View = _vwDisabled;
            }

            if (copyComponent.ReplacementPlateCost.HasValue)
                this[28, colIndex] = new SourceGrid.Cells.Cell(copyComponent.ReplacementPlateCost.Value, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Replacement Plate Cost
            else
                this[28, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Replacement Plate Cost
            this[28, colIndex].View = _vwRightAlignEnabled;
            this[28, colIndex].Editor.AllowNull = true;

            if (copyComponent.RunRate.HasValue)
                this[29, colIndex] = new SourceGrid.Cells.Cell(copyComponent.RunRate.Value, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Run Rate
            else
                this[29, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Run Rate
            this[29, colIndex].View = _vwRightAlignEnabled;
            this[29, colIndex].Editor.AllowNull = true;

            if (copyComponent.NumberDigitalHandleandPrepare.HasValue)
                this[30, colIndex] = new SourceGrid.Cells.Cell(copyComponent.NumberDigitalHandleandPrepare.Value, new NumericUpDown(typeof(int), 999, 0, 1)); // Number Digital Handle & Prepare
            else
                this[30, colIndex] = new SourceGrid.Cells.Cell(null, new NumericUpDown(typeof(int), 999, 0, 1)); // Number Digital Handle & Prepare
            this[30, colIndex].View = _vwRightAlignEnabled;
            this[30, colIndex].Editor.AllowNull = true;
            this[30, colIndex].Editor.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;

            // Digital Handle & Prepare Rate
            if (curPrinter != null && !copyComponent.VendorSupplied && copyComponent.CalcPrinterCost)
            {
                PrinterRateEditor edDigiHP = new PrinterRateEditor(_dsEstimates, curPrinter.Value, 5);
                edDigiHP.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
                LongPair curDigiHP = null;
                if (string.IsNullOrEmpty(copyComponent.DigitalHandleandPrepareDesc) || edDigiHP.FindRatefromDesc(copyComponent.DigitalHandleandPrepareDesc) == null)
                    curDigiHP = edDigiHP.DefaultPrinterRate;
                else
                    curDigiHP = edDigiHP.FindRatefromDesc(copyComponent.DigitalHandleandPrepareDesc);

                this[31, colIndex] = new SourceGrid.Cells.Cell(curDigiHP, edDigiHP);
                this[31, colIndex].Editor.AllowNull = true;
                this[31, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            }
            else
            {
                this[31, colIndex] = new SourceGrid.Cells.Cell();
                this[31, colIndex].View = _vwDisabled;
            }

            // Stitcher Makeready
            if (curPrinter != null && !copyComponent.VendorSupplied)
            {
                PrinterRateEditor edStitcherMakeready = new PrinterRateEditor(_dsEstimates, curPrinter.Value, 4);
                edStitcherMakeready.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
                LongPair curStitcherMakeready = null;
                if (string.IsNullOrEmpty(copyComponent.StitcherMakereadyDesc) || edStitcherMakeready.FindRatefromDesc(copyComponent.StitcherMakereadyDesc) == null)
                    curStitcherMakeready = null;
                else
                    curStitcherMakeready = edStitcherMakeready.FindRatefromDesc(copyComponent.StitcherMakereadyDesc);

                this[32, colIndex] = new SourceGrid.Cells.Cell(curStitcherMakeready, edStitcherMakeready);
                this[32, colIndex].Editor.AllowNull = true;
                this[32, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            }
            else
            {
                this[32, colIndex] = new SourceGrid.Cells.Cell();
                this[32, colIndex].View = _vwDisabled;
            }

            // Manual Stitcher Makererady
            if (curPrinter != null && !copyComponent.VendorSupplied)
            {
                if (copyComponent.StitcherMakereadyRate.HasValue)
                    this[33, colIndex] = new SourceGrid.Cells.Cell(copyComponent.StitcherMakereadyRate.Value, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));
                else
                    this[33, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));
                this[33, colIndex].View = _vwRightAlignEnabled;
                this[33, colIndex].Editor.AllowNull = true;
            }
            else
            {
                this[33, colIndex] = new SourceGrid.Cells.Cell();
                this[33, colIndex].View = _vwRightAlignDisabled;
            }

            // Press Makeready
            if (curPrinter != null && !copyComponent.VendorSupplied)
            {
                PrinterRateEditor edPressMakeready = new PrinterRateEditor(_dsEstimates, curPrinter.Value, 10);
                edPressMakeready.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
                LongPair curPressMakeready = null;
                if (string.IsNullOrEmpty(copyComponent.PressMakereadyDesc) || edPressMakeready.FindRatefromDesc(copyComponent.PressMakereadyDesc) == null)
                    curPressMakeready = null;
                else
                    curPressMakeready = edPressMakeready.FindRatefromDesc(copyComponent.PressMakereadyDesc);

                this[34, colIndex] = new SourceGrid.Cells.Cell(curPressMakeready, edPressMakeready);
                this[34, colIndex].Editor.AllowNull = true;
                this[34, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            }
            else
            {
                this[34, colIndex] = new SourceGrid.Cells.Cell();
                this[34, colIndex].View = _vwDisabled;
            }

            // Manual Stitcher Makererady
            if (curPrinter != null && !copyComponent.VendorSupplied)
            {
                if (copyComponent.PressMakereadyRate.HasValue)
                    this[35, colIndex] = new SourceGrid.Cells.Cell(copyComponent.PressMakereadyRate.Value, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));
                else
                    this[35, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));
                this[35, colIndex].View = _vwRightAlignEnabled;
                this[35, colIndex].Editor.AllowNull = true;
            }
            else
            {
                this[35, colIndex] = new SourceGrid.Cells.Cell();
                this[35, colIndex].View = _vwRightAlignDisabled;
            }

            if (copyComponent.EarlyPayPrintDiscount.HasValue)
                this[36, colIndex] = new SourceGrid.Cells.Cell(copyComponent.EarlyPayPrintDiscount.Value, new TextBoxPercentageRange(0, 1)); // Early Pay Discount
            else
                this[36, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxPercentageRange(0, 1)); // Early Pay Discount
            this[36, colIndex].View = _vwRightAlignEnabled;
            this[36, colIndex].Editor.AllowNull = true;

            this[37, colIndex] = new SourceGrid.Cells.CheckBox(string.Empty, copyComponent.PrinterApplyTax); // Printer Apply Tax;

            if (copyComponent.PrinterTaxableMediaPct.HasValue)
                this[38, colIndex] = new SourceGrid.Cells.Cell(copyComponent.PrinterTaxableMediaPct.Value, new TextBoxPercentageRange(0, 1)); // Taxable Media
            else
                this[38, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxPercentageRange(0, 1)); // Taxable Media
            this[38, colIndex].View = _vwRightAlignEnabled;
            this[38, colIndex].Editor.AllowNull = true;

            if (copyComponent.PrinterSalesTaxPct.HasValue)
                this[39, colIndex] = new SourceGrid.Cells.Cell(copyComponent.PrinterSalesTaxPct.Value, new TextBoxPercentageRange(0, 1));
            else
                this[39, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxPercentageRange(0, 1)); // Sales Tax
            this[39, colIndex].View = _vwRightAlignEnabled;
            this[39, colIndex].Editor.AllowNull = true;

            this[40, colIndex] = null; // Blank Line

            PaperEditor edPaper = new PaperEditor(_dsEstimates, _dsEstimates.est_estimate[0].rundate, null);
            edPaper.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            if (string.IsNullOrEmpty(copyComponent.PaperDesc))
                this[41, colIndex] = new SourceGrid.Cells.Cell(null, edPaper); // Paper Vendor
            else
                this[41, colIndex] = new SourceGrid.Cells.Cell(edPaper.GetPaperfromDesc(copyComponent.PaperDesc), edPaper);
            this[41, colIndex].Editor.AllowNull = true;
            this[41, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;

            LongPair curPaper = this[41, colIndex].Value as LongPair;


            PaperWeightEditor edPaperWeight = new PaperWeightEditor(_dsEstimates);
            edPaperWeight.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            if (copyComponent.PaperWeight == null)
                this[42, colIndex] = new SourceGrid.Cells.Cell(null, edPaperWeight); // Paper Weight
            else
                this[42, colIndex] = new SourceGrid.Cells.Cell(edPaperWeight.GetWeightFromDesc(copyComponent.PaperWeight.Value.ToString()), edPaperWeight);
            this[42, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            IntPair curPaperWeight = this[42, colIndex].Value as IntPair;

            PaperGradeEditor edPaperGrade = new PaperGradeEditor(_dsEstimates); // Paper Grade;
            edPaperGrade.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            if (string.IsNullOrEmpty(copyComponent.PaperGrade))
                this[43, colIndex] = new SourceGrid.Cells.Cell(null, edPaperGrade); // Paper Grade
            else
                this[43, colIndex] = new SourceGrid.Cells.Cell(edPaperGrade.GetGradefromDescription(copyComponent.PaperGrade), edPaperGrade);
            this[43, colIndex].Editor.AllowNull = true;
            this[43, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            IntPair curPaperGrade = this[43, colIndex].Value as IntPair;

            if (curPaper != null && curPaperWeight != null && curPaperGrade != null && !copyComponent.VendorSupplied)
            {
                PaperMapEditor edPaperMap = new PaperMapEditor(_dsEstimates, curPaper.Value, curPaperWeight.Value, curPaperGrade.Value, _dsEstimates.est_estimate[0].rundate);
                edPaperMap.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
                LongPair curPaperMap = null;
                if (string.IsNullOrEmpty(copyComponent.PaperMapDesc) || edPaperMap.GetPaperMapfromDescription(copyComponent.PaperMapDesc) == null)
                    curPaperMap = edPaperMap.DefaultPaperMap;
                else
                    curPaperMap = edPaperMap.GetPaperMapfromDescription(copyComponent.PaperMapDesc);
                this[44, colIndex] = new SourceGrid.Cells.Cell(curPaperMap, edPaperMap);
                this[44, colIndex].Editor.AllowNull = true;
                this[44, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
            }
            else
            {
                this[44, colIndex] = new SourceGrid.Cells.Cell();
            }

            this[45, colIndex] = new SourceGrid.Cells.CheckBox(string.Empty, copyComponent.CalcPaperCost);

            if (copyComponent.ManualPaperCost.HasValue)
                this[46, colIndex] = new SourceGrid.Cells.Cell(copyComponent.ManualPaperCost.Value, new TextBoxCurrencyRange(0, _cCurrencyMaxValue));
            else
                this[46, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Paper Cost
            this[46, colIndex].View = _vwRightAlignEnabled;

            if (copyComponent.RunPounds.HasValue)
                this[47, colIndex] = new SourceGrid.Cells.Cell(copyComponent.RunPounds.Value, new TextBoxDecimalRange(0, _cMaxRunPounds, 2)); // Run Pounds
            else
                this[47, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxDecimalRange(0, _cMaxRunPounds, 2)); // Run Pounds
            this[47, colIndex].View = _vwRightAlignEnabled;
            this[47, colIndex].Editor.AllowNull = true;

            if (copyComponent.MakereadyPounds.HasValue)
                this[48, colIndex] = new SourceGrid.Cells.Cell(copyComponent.MakereadyPounds.Value, new TextBoxIntegerRange(0, _cMaxMakereadyPounds)); // Makeready Pounds
            else
                this[48, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxIntegerRange(0, _cMaxMakereadyPounds)); // Makeready Pounds
            this[48, colIndex].View = _vwRightAlignEnabled;
            this[48, colIndex].Editor.AllowNull = true;

            if (copyComponent.PlateChangePounds.HasValue)
                this[49, colIndex] = new SourceGrid.Cells.Cell(copyComponent.PlateChangePounds.Value, new TextBoxDecimalRange(0, _cMaxPlateChangePounds, 2)); // Plate Change Pounds
            else
                this[49, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxDecimalRange(0, _cMaxPlateChangePounds, 2)); // Plate Change Pounds
            this[49, colIndex].View = _vwRightAlignEnabled;
            this[49, colIndex].Editor.AllowNull = true;

            if (copyComponent.PressStopPounds.HasValue)
                this[50, colIndex] = new SourceGrid.Cells.Cell(copyComponent.PressStopPounds.Value, new TextBoxIntegerRange(0, _cMaxPressStopPounds));
            else
                this[50, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxIntegerRange(0, _cMaxPressStopPounds)); // Press Stop Pounds
            this[50, colIndex].View = _vwRightAlignEnabled;
            this[50, colIndex].Editor.AllowNull = true;

            if (copyComponent.NumberofPressStops.HasValue)
                this[51, colIndex] = new SourceGrid.Cells.Cell(copyComponent.NumberofPressStops.Value, new TextBoxIntegerRange(0, int.MaxValue)); // Number of Press Stops
            else
                this[51, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxIntegerRange(0, int.MaxValue)); // Number of Press Stops
            this[51, colIndex].View = _vwRightAlignEnabled;
            this[51, colIndex].Editor.AllowNull = true;

            if (copyComponent.EarlyPayPaperDiscount.HasValue)
                this[52, colIndex] = new SourceGrid.Cells.Cell(copyComponent.EarlyPayPaperDiscount.Value, new TextBoxPercentageRange(0, 1)); // Early Pay Discount
            else
                this[52, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxPercentageRange(0, 1)); // Early Pay Discount
            this[52, colIndex].View = _vwRightAlignEnabled;
            this[52, colIndex].Editor.AllowNull = true;

            this[53, colIndex] = new SourceGrid.Cells.CheckBox(string.Empty, copyComponent.PaperApplyTax); // Paper Apply Tax

            if (copyComponent.PaperTaxableMediaPct.HasValue)
                this[54, colIndex] = new SourceGrid.Cells.Cell(copyComponent.PaperTaxableMediaPct.Value, new TextBoxPercentageRange(0, 1)); // Taxable Media
            else
                this[54, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxPercentageRange(0, 1)); // Taxable Media
            this[54, colIndex].View = _vwRightAlignEnabled;
            this[54, colIndex].Editor.AllowNull = true;

            if (copyComponent.PaperSalesTaxPct.HasValue)
                this[55, colIndex] = new SourceGrid.Cells.Cell(copyComponent.PaperSalesTaxPct, new TextBoxPercentageRange(0, 1)); // Sales Tax
            else
                this[55, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxPercentageRange(0, 1)); // Sales Tax
            this[55, colIndex].View = _vwRightAlignEnabled;
            this[55, colIndex].Editor.AllowNull = true;

            this[56, colIndex] = null; // Blank Line


            PrinterEditor edAssemblyVendor = new PrinterEditor(_dsEstimates, _dsEstimates.est_estimate[0].rundate, null); // Assembly Vendor
            edAssemblyVendor.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;
            if (string.IsNullOrEmpty(copyComponent.AssemblyVendorDesc))
            {
                this[57, colIndex] = new SourceGrid.Cells.Cell(null, edAssemblyVendor);
            }
            else
            {
                this[57, colIndex] = new SourceGrid.Cells.Cell(edPrinter.GetPrinterfromDesc(copyComponent.AssemblyVendorDesc), edAssemblyVendor);
            }
            this[57, colIndex].Editor.AllowNull = true;
            this[57, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;

            LongPair curAssemblyVendor = (LongPair)this[57, colIndex].Value; // Assembly Vendor


            if (curAssemblyVendor != null && curComponentType != null)
            {
                switch (curComponentType.Value)
                {
                    case 3: // Stitch-In
                        PrinterRateEditor edStitchIn = new PrinterRateEditor(_dsEstimates, curAssemblyVendor.Value, 1);
                        edStitchIn.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;

                        LongPair curStitchInRate = null;
                        if (string.IsNullOrEmpty(copyComponent.StitchInDesc) || edStitchIn.FindRatefromDesc(copyComponent.StitchInDesc) == null)
                            curStitchInRate = edStitchIn.DefaultPrinterRate;
                        else
                            curStitchInRate = edStitchIn.FindRatefromDesc(copyComponent.StitchInDesc);
                        this[58, colIndex] = new SourceGrid.Cells.Cell(curStitchInRate, edStitchIn);
                        this[58, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;

                        this[59, colIndex] = new SourceGrid.Cells.Cell();

                        this[60, colIndex] = new SourceGrid.Cells.Cell();
                        break;
                    case 4: // Blow-In
                        this[58, colIndex] = new SourceGrid.Cells.Cell();

                        PrinterRateEditor edBlowIn = new PrinterRateEditor(_dsEstimates, curAssemblyVendor.Value, 2);
                        edBlowIn.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;

                        LongPair curBlowInRate = null;
                        if (string.IsNullOrEmpty(copyComponent.BlowInDesc) || edBlowIn.FindRatefromDesc(copyComponent.BlowInDesc) == null)
                            curBlowInRate = edBlowIn.DefaultPrinterRate;
                        else
                            curBlowInRate = edBlowIn.FindRatefromDesc(copyComponent.BlowInDesc);
                        this[59, colIndex] = new SourceGrid.Cells.Cell(curBlowInRate, edBlowIn);
                        this[59, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;

                        this[60, colIndex] = new SourceGrid.Cells.Cell();
                        break;
                    case 2: // Onsert
                        this[58, colIndex] = new SourceGrid.Cells.Cell();

                        this[59, colIndex] = new SourceGrid.Cells.Cell();

                        PrinterRateEditor edOnsert = new PrinterRateEditor(_dsEstimates, curAssemblyVendor.Value, 9);
                        edOnsert.EditableMode = EditableMode.Focus | EditableMode.AnyKey | EditableMode.SingleClick;

                        LongPair curOnsertRate = null;
                        if (string.IsNullOrEmpty(copyComponent.OnsertDesc) || edOnsert.FindRatefromDesc(copyComponent.OnsertDesc) == null)
                            curOnsertRate = edOnsert.DefaultPrinterRate;
                        else
                            curOnsertRate = edOnsert.FindRatefromDesc(copyComponent.OnsertDesc);
                        this[60, colIndex] = new SourceGrid.Cells.Cell(curOnsertRate, edOnsert);
                        this[60, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
                        break;
                    default:
                        this[58, colIndex] = new SourceGrid.Cells.Cell();
                        this[59, colIndex] = new SourceGrid.Cells.Cell();
                        this[60, colIndex] = new SourceGrid.Cells.Cell();
                        break;
                }
            }
            else
            {
                this[58, colIndex] = new SourceGrid.Cells.Cell();  // Stitch-In Rate
                this[59, colIndex] = new SourceGrid.Cells.Cell();  // Blow-In Rate
                this[60, colIndex] = new SourceGrid.Cells.Cell();  // Onsert Rate
            }

            this[61, colIndex] = null; // Blank Line

            if (copyComponent.OtherProduction.HasValue)
                this[62, colIndex] = new SourceGrid.Cells.Cell(copyComponent.OtherProduction, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Other Production
            else
                this[62, colIndex] = new SourceGrid.Cells.Cell(null, new TextBoxCurrencyRange(0, _cCurrencyMaxValue)); // Other Production
            this[62, colIndex].View = _vwRightAlignEnabled;
            this[62, colIndex].Editor.AllowNull = true;

            #endregion

            #region Disable Cells / Set to Unselectable

            this[1, colIndex].View = _vwDisabled; // Component ID
            this[1, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            if (copyComponent.VendorSupplied)
            {
                for (int i = 24; i < 40; ++i)
                {
                    if (this[i, colIndex].GetType() == typeof(SourceGrid.Cells.CheckBox))
                        this[i, colIndex].View = _vwCheckBoxDisabled;
                    else
                        this[i, colIndex].View = _vwDisabled;
                    this[i, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    if (this[i, colIndex].Editor != null)
                        this[i, colIndex].Editor.EnableEdit = false;
                }

                for (int i = 44; i < 56; ++i)
                {
                    if (this[i, colIndex].GetType() == typeof(SourceGrid.Cells.CheckBox))
                        this[i, colIndex].View = _vwCheckBoxDisabled;
                    else
                        this[i, colIndex].View = _vwDisabled;
                    this[i, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    if (this[i, colIndex].Editor != null)
                        this[i, colIndex].Editor.EnableEdit = false;
                }
            }
            else
            {
                this[16, colIndex].View = _vwDisabled;
                this[16, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                if (this[16, colIndex].Editor != null)
                    this[16, colIndex].Editor.EnableEdit = false;

                this[17, colIndex].View = _vwDisabled;
                this[17, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                if (this[17, colIndex].Editor != null)
                    this[17, colIndex].Editor.EnableEdit = false;

                if (copyComponent.CalcPrinterCost)
                {
                    this[25, colIndex].View = _vwDisabled;
                    this[25, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    if (this[25, colIndex].Editor != null)
                        this[25, colIndex].Editor.EnableEdit = false;

                    if (curPrinter == null)
                    {
                        this[32, colIndex].View = _vwDisabled; // Stitcher Makeready
                        this[32, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[32, colIndex].Editor != null)
                            this[32, colIndex].Editor.EnableEdit = false;

                        this[33, colIndex].View = _vwDisabled; // Manual Stitcher Makeready
                        this[33, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[33, colIndex].Editor != null)
                            this[33, colIndex].Editor.EnableEdit = false;

                        this[34, colIndex].View = _vwDisabled; // Press Makeready
                        this[34, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[34, colIndex].Editor != null)
                            this[34, colIndex].Editor.EnableEdit = false;

                        this[35, colIndex].View = _vwDisabled; // Manual Press Makeready
                        this[35, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[35, colIndex].Editor != null)
                            this[35, colIndex].Editor.EnableEdit = false;
                    }

                    if (!copyComponent.PrinterApplyTax)
                    {
                        this[38, colIndex].View = _vwDisabled; // Printer Taxable Media
                        this[38, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[38, colIndex].Editor != null)
                            this[38, colIndex].Editor.EnableEdit = false;

                        this[39, colIndex].View = _vwDisabled; // Printer Sales Tax
                        this[39, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[39, colIndex].Editor != null)
                            this[39, colIndex].Editor.EnableEdit = false;
                    }
                }
                else
                {
                    for (int i = 26; i < 37; ++i)
                    {
                        if (this[i, colIndex].GetType() == typeof(SourceGrid.Cells.CheckBox))
                            this[i, colIndex].View = _vwCheckBoxDisabled;
                        else
                            this[i, colIndex].View = _vwDisabled;
                        this[i, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[i, colIndex].Editor != null)
                            this[i, colIndex].Editor.EnableEdit = false;
                    }
                }

                if (copyComponent.CalcPaperCost)
                {
                    this[46, colIndex].View = _vwDisabled;
                    this[46, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    if (this[46, colIndex].Editor != null)
                        this[46, colIndex].Editor.EnableEdit = false;
                }
                else
                {
                    for (int i = 47; i < 52; ++i)
                    {
                        if (this[i, colIndex].GetType() == typeof(SourceGrid.Cells.CheckBox))
                            this[i, colIndex].View = _vwCheckBoxDisabled;
                        else
                            this[i, colIndex].View = _vwDisabled;
                        this[i, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[i, colIndex].Editor != null)
                            this[i, colIndex].Editor.EnableEdit = false;
                    }
                }

                if ( !copyComponent.PaperApplyTax )
                {
                    this[54, colIndex].View = _vwDisabled;
                    this[54, colIndex].AddController( SourceGrid.Cells.Controllers.Unselectable.Default );
                    if ( this[54, colIndex].Editor != null )
                        this[54, colIndex].Editor.EnableEdit = false;

                    this[55, colIndex].View = _vwDisabled;
                    this[55, colIndex].AddController( SourceGrid.Cells.Controllers.Unselectable.Default );
                    if ( this[55, colIndex].Editor != null )
                        this[55, colIndex].Editor.EnableEdit = false;
                }

                if (copyComponent.CalcPrinterCost)
                {
                    this[25, colIndex].View = _vwDisabled;
                    this[25, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    if (this[25, colIndex].Editor != null)
                        this[25, colIndex].Editor.EnableEdit = false;
                }

                if (curAssemblyVendor != null && curComponentType != null)
                {
                    switch (curComponentType.Value)
                    {
                        case 3: // Stitch-In
                            this[59, colIndex].View = _vwDisabled; // Blow-In
                            this[59, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                            if (this[59, colIndex].Editor != null)
                                this[59, colIndex].Editor.EnableEdit = false;

                            this[60, colIndex].View = _vwDisabled; // Onsert
                            this[60, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                            if (this[60, colIndex].Editor != null)
                                this[60, colIndex].Editor.EnableEdit = false;
                            break;
                        case 4: // Blow-In
                            this[58, colIndex].View = _vwDisabled; // Stitch-In
                            this[58, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                            if (this[58, colIndex].Editor != null)
                                this[58, colIndex].Editor.EnableEdit = false;

                            this[60, colIndex].View = _vwDisabled; // Onsert
                            this[60, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                            if (this[60, colIndex].Editor != null)
                                this[60, colIndex].Editor.EnableEdit = false;
                            break;
                        case 2: // Onsert
                            this[58, colIndex].View = _vwDisabled; // Stitch-In
                            this[58, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                            if (this[58, colIndex].Editor != null)
                                this[58, colIndex].Editor.EnableEdit = false;

                            this[59, colIndex].View = _vwDisabled; // Blow-In
                            this[59, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                            if (this[59, colIndex].Editor != null)
                                this[59, colIndex].Editor.EnableEdit = false;
                            break;
                        default:
                            this[58, colIndex].View = _vwDisabled; // Stitch-In
                            this[58, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                            if (this[58, colIndex].Editor != null)
                                this[58, colIndex].Editor.EnableEdit = false;

                            this[59, colIndex].View = _vwDisabled; // Blow-In
                            this[59, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                            if (this[59, colIndex].Editor != null)
                                this[59, colIndex].Editor.EnableEdit = false;

                            this[60, colIndex].View = _vwDisabled; // Onsert
                            this[60, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                            if (this[60, colIndex].Editor != null)
                                this[60, colIndex].Editor.EnableEdit = false;
                            break;
                    }
                }
                else
                {
                    this[58, colIndex].View = _vwDisabled; // Stitch-In
                    this[58, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    if (this[58, colIndex].Editor != null)
                        this[58, colIndex].Editor.EnableEdit = false;

                    this[59, colIndex].View = _vwDisabled; // Blow-In
                    this[59, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    if (this[59, colIndex].Editor != null)
                        this[59, colIndex].Editor.EnableEdit = false;

                    this[60, colIndex].View = _vwDisabled; // Onsert
                    this[60, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    if (this[60, colIndex].Editor != null)
                        this[60, colIndex].Editor.EnableEdit = false;
                }
            }
            #endregion

            #region Set Cell Controllers
            //this[0, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            //this[0, colIndex].AddController(new SourceGrid.Cells.Controllers.MouseInvalidate());
            //this[0, colIndex].AddController(new SourceGrid.Cells.Controllers.Resizable(CellResizeMode.Width));
            //this[0, colIndex].AddController(_menuPopup);
            //this[0, colIndex].AddController(new SelectColumn()); // Column Header

            this[3, colIndex].AddController(_displayComments); // Comments
            this[4, colIndex].AddController(_displayFinancialComments); // Financial Comments

            if (curComponentType != null && curComponentType.Value == 1) // Host
                this[7, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            else // Non Host
                this[7, colIndex].AddController(new AssemblyRateController()); // ComponentType <> 1
            //this[14, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default); // Blank Line
            // When Vendor Supplied value changes, also populate the stitch-in, blow-in, onsert, plate cost and paper map dropdowns
            this[15, colIndex].AddController(_ctrVSCheckBox);
            this[15, colIndex].AddController(new PrinterController());
            this[15, colIndex].AddController(new PaperRateController());
            //this[22, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default); // Blank Line
            this[23, colIndex].AddController(new PrinterController()); // Printer Vendor
            this[24, colIndex].AddController(_ctrCalcPrintCostCheckBox); // Calc Print Cost
            this[24, colIndex].AddController(new PrinterController());
            this[32, colIndex].AddController(new StitcherMakereadyController()); // Stitcher Makeready
            this[33, colIndex].AddController(new ManualStitcherMakereadyController()); // Manual Stitcher Makeready
            this[34, colIndex].AddController(new PressMakereadyController()); // Press Makeready
            this[35, colIndex].AddController(new ManualPressMakereadyController()); // Manual Press Makeready
            this[37, colIndex].AddController(_ctrApplyTaxCheckBox); // Printer Apply Tax
            //this[40, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default); // Blank Line
            this[41, colIndex].AddController(new PaperController()); // Paper Vendor
            this[42, colIndex].AddController(new PaperRateController()); // Paper Weight
            this[43, colIndex].AddController(new PaperRateController()); // Paper Grade
            this[45, colIndex].AddController(_ctrCalcPaperCostCheckBox); // Calc Paper Cost
            this[53, colIndex].AddController(_ctrApplyTaxCheckBox); // Paper Apply Tax
            this[57, colIndex].AddController(new AssemblyRateController()); // Assembly Vendor

            // Add the Required Controller to track cell value changes in the parent form
            // Make all cells readonly if the form is readonly
            for (int i = 1; i < 63; ++i)
            {
                if (i != 14 && i != 22 && i != 40 && i != 56 && i != 61)
                {
                    ValueChangedController _ctrValueChanged = new ValueChangedController();
                    this[i, colIndex].AddController(_ctrValueChanged);
                    _ctrValueChanged.CellValueChanged += new CellValueChanged(this.CellValueChanged);
                    _ctrValueChanged.CellValueChanged += new CellValueChanged(this.ColumnContainsData);
                    _ctrValueChanged.InitialValue = this[i, colIndex].Value;

                    this[i, colIndex].AddController(SelectCell.Default);

                    // Mark cells read-only (except for separator cells)
                    if (_readOnly)
                    {
                        this[i, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        if (this[i, colIndex].Editor != null)
                            this[i, colIndex].Editor.EnableEdit = false;

                        if (this[i, colIndex] is SourceGrid.Cells.CheckBox)
                            this[i, colIndex].View = _vwCheckBoxDisabled;
                        else if (this[i, colIndex].Editor is TextBoxNumeric || this[i, colIndex].Editor is NumericUpDown)
                            this[i, colIndex].View = _vwRightAlignDisabled;
                        else
                            this[i, colIndex].View = _vwDisabled;
                        if (this[i, colIndex].FindController(typeof(SourceGrid.Cells.Controllers.Unselectable)) == null)
                            this[i, colIndex].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    }
                }
            }
            #endregion

            // flag this column as "not blank"
            _blankColumn[colIndex - 1] = false;

            // Repaint the control
            this.Invalidate();
        }

        #endregion

        #region Public Properties

        public int SelectedComponentCount
        {
            get
            {
                int colCount = 0;
                for (int colIndex = 1; colIndex < this.ColumnsCount; ++colIndex)
                {
                    if (this.Selection.IsSelectedColumn(colIndex))
                        ++colCount;
                }
                return colCount;
            }
        }

        #endregion

        #region Events

        public event CellValueChanged CellValueChanged;
        public event EventHandler ComponentAdded;
        public event CancelEventHandler ComponentRemoving;
        public event EventHandler ComponentRemoved;
        public event CancelEventHandler ComponentBeginOverwrite;

        #endregion

        #region Event Handlers

        public void OnColumnClicked(SourceGrid.CellContext sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.SelectionMode != GridSelectionMode.Column)
            {
                this.SelectionMode = GridSelectionMode.Column;
                this.Selection.EnableMultiSelection = true;

                this.Selection.ResetSelection(false);
                this.Selection.Focus(sender.Position, false);

                this.Selection.SelectColumn(sender.Position.Column, true);
            }
        }

        public void OnCellClicked(SourceGrid.CellContext sender, System.Windows.Forms.MouseEventArgs e)
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

        public void ColumnContainsData(SourceGrid.CellContext cellContext, EventArgs e)
        {
            _blankColumn[cellContext.Position.Column - 1] = false;
        }

        #endregion

        #region Public Events

        public override void ProcessSpecialGridKey(System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Handled)
                return;

            if (e.KeyCode == System.Windows.Forms.Keys.Enter || e.KeyCode == System.Windows.Forms.Keys.Tab)
            {
                if ((SpecialKeys & GridSpecialKeys.Enter) == GridSpecialKeys.Enter || (SpecialKeys & GridSpecialKeys.Tab) == GridSpecialKeys.Tab)
                {
                    CellContext focusCellContext = new CellContext(this, Selection.ActivePosition);
                    if (focusCellContext != null)
                    {
                        if (focusCellContext.IsEditing())
                            focusCellContext.EndEdit(false);
                        //else
                        //{
                        if (Selection.ActivePosition.Row == (this.RowsCount - 1))
                        {
                            if (Selection.ActivePosition.Column == (this.ColumnsCount - 1))
                                this.AddComponent();
                            {
                                Selection.MoveActiveCell(-(this.Rows.Count - 3), 1);
                            }
                        }
                        else
                        {
                            Selection.MoveActiveCell(1, 0);
                        }
                        //}
                    }
                }
                e.Handled = true;
            }

            base.ProcessSpecialGridKey(e);
        }

        public Estimates EstimateDS
        {
            get { return _dsEstimates; }
            set { _dsEstimates = value; }
        }

        public void WriteToDataset()
        {
            for (int componentIndex = (_Components.Count - 1); componentIndex >= 0; --componentIndex)
            {
                int colIndex = componentIndex + 1;

                if (_blankColumn[componentIndex])
                {
                    DeleteColumn(colIndex);
                }
                else
                {
                    Estimates.est_componentRow c_row = _Components[componentIndex];
                    c_row.description = this[2, colIndex].Value.ToString();
                    if (this[3, colIndex].Value == null || this[3, colIndex].Value.ToString().Trim().Length == 0)
                        c_row.SetcommentsNull();
                    else
                        c_row.comments = this[3, colIndex].Value.ToString();
                    if (this[4, colIndex].Value == null || this[4, colIndex].Value.ToString().Trim().Length == 0)
                        c_row.SetfinancialchangecommentNull();
                    else
                        c_row.financialchangecomment = this[4, colIndex].Value.ToString();
                    if (this[5, colIndex].Value == null || string.IsNullOrEmpty(this[5, colIndex].Value.ToString()))
                        c_row.SetadnumberNull();
                    else
                        c_row.adnumber = (int)this[5, colIndex].Value;
                    c_row.est_estimatemediatype_id = ((IntPair)this[6, colIndex].Value).Value;
                    c_row.est_componenttype_id = ((IntPair)this[7, colIndex].Value).Value;
                    if (this[8, colIndex].Value == null || this[8, colIndex].Value.ToString() == String.Empty)
                        c_row.SetmediaqtywoinsertNull();
                    else
                        c_row.mediaqtywoinsert = (int)this[8, colIndex].Value;
                    if (this[9, colIndex].Value == null || this[9, colIndex].Value.ToString() == string.Empty)
                        c_row.SetspoilagepctNull();
                    else
                        c_row.spoilagepct = (decimal)this[9, colIndex].Value;
                    c_row.pagecount = (int)this[10, colIndex].Value;
                    c_row.width = (decimal)this[11, colIndex].Value;
                    c_row.height = (decimal)this[12, colIndex].Value;
                    if (this[13, colIndex].Value == null)
                        c_row.SetnumberofplantsNull();
                    else
                        c_row.numberofplants = (int)this[13, colIndex].Value;
                    c_row.vendorsupplied = (bool)this[15, colIndex].Value;
                    if (this[16, colIndex].Value == null || ((LongPair)this[16,colIndex].Value).Value == -1)
                        c_row.Setvendorsupplied_idNull();
                    else
                        c_row.vendorsupplied_id = ((LongPair)this[16, colIndex].Value).Value;
                    if (this[17, colIndex].Value == null)
                        c_row.SetvendorcpmNull();
                    else
                        c_row.vendorcpm = (decimal)this[17, colIndex].Value;
                    if (this[18, colIndex].Value == null || ((LongPair)this[18, colIndex].Value).Value == -1)
                        c_row.Setcreativevendor_idNull();
                    else
                        c_row.creativevendor_id = ((LongPair)this[18, colIndex].Value).Value;
                    if (this[19, colIndex].Value == null)
                        c_row.SetcreativecppNull();
                    else
                        c_row.creativecpp = (decimal)this[19, colIndex].Value;
                    if (this[20, colIndex].Value == null || ((LongPair)this[20, colIndex].Value).Value == -1)
                        c_row.Setseparator_idNull();
                    else
                        c_row.separator_id = ((LongPair)this[20, colIndex].Value).Value;
                    if (this[21, colIndex].Value == null)
                        c_row.SetseparatorcppNull();
                    else
                        c_row.separatorcpp = (decimal)this[21, colIndex].Value;
                    if (this[23, colIndex].Value == null || ((LongPair)this[23, colIndex].Value).Value == -1)
                        c_row.Setprinter_idNull();
                    else
                        c_row.printer_id = ((LongPair)this[23, colIndex].Value).Value;
                    if (!c_row.vendorsupplied)
                        c_row.calculateprintcost = (bool)this[24, colIndex].Value;
                    else
                        c_row.calculateprintcost = false;
                    if (this[25, colIndex].Value == null)
                        c_row.SetprintcostNull();
                    else
                        c_row.printcost = (decimal)this[25, colIndex].Value;
                    if (this[26, colIndex].Value == null)
                        c_row.SetadditionalplatesNull();
                    else
                        c_row.additionalplates = (int)this[26, colIndex].Value;
                    if (this[27, colIndex].Value == null || ((LongPair)this[27, colIndex].Value).Value == -1)
                        c_row.Setplatecost_idNull();
                    else
                        c_row.platecost_id = ((LongPair)this[27, colIndex].Value).Value;
                    if (this[28, colIndex].Value == null)
                        c_row.SetreplacementplatecostNull();
                    else
                        c_row.replacementplatecost = (decimal)this[28, colIndex].Value;
                    if (this[29, colIndex].Value == null)
                        c_row.SetrunrateNull();
                    else
                        c_row.runrate = (decimal)this[29, colIndex].Value;
                    if (this[30, colIndex].Value == null)
                        c_row.SetnumberdigitalhandlenprepareNull();
                    else
                        c_row.numberdigitalhandlenprepare = (int)this[30, colIndex].Value;
                    if (this[31, colIndex].Value == null || ((LongPair)this[31, colIndex].Value).Value == -1)
                        c_row.Setdigitalhandlenprepare_idNull();
                    else
                        c_row.digitalhandlenprepare_id = ((LongPair)this[31, colIndex].Value).Value;
                    if (this[32, colIndex].Value == null || ((LongPair)this[32,colIndex].Value).Value == -1)
                        c_row.Setstitchermakeready_idNull();
                    else
                        c_row.stitchermakeready_id = ((LongPair)this[32, colIndex].Value).Value;
                    if (this[33, colIndex].Value == null)
                        c_row.SetstitchermakereadyrateNull();
                    else
                        c_row.stitchermakereadyrate = (decimal)this[33, colIndex].Value;
                    if (this[34, colIndex].Value == null || ((LongPair)this[34,colIndex].Value).Value == -1)
                        c_row.Setpressmakeready_idNull();
                    else
                        c_row.pressmakeready_id = ((LongPair)this[34, colIndex].Value).Value;
                    if (this[35, colIndex].Value == null)
                        c_row.SetpressmakereadyrateNull();
                    else
                        c_row.pressmakereadyrate = (decimal)this[35, colIndex].Value;
                    if (this[36, colIndex].Value == null)
                        c_row.SetearlypayprintdiscountNull();
                    else
                        c_row.earlypayprintdiscount = (decimal)this[36, colIndex].Value;
                    if (!c_row.vendorsupplied)
                        c_row.printerapplytax = (bool)this[37, colIndex].Value;
                    else
                        c_row.printerapplytax = false;
                    if (this[38, colIndex].Value == null)
                        c_row.SetprintertaxablemediapctNull();
                    else
                        c_row.printertaxablemediapct = (decimal)this[38, colIndex].Value;
                    if (this[39, colIndex].Value == null)
                        c_row.SetprintersalestaxpctNull();
                    else
                        c_row.printersalestaxpct = (decimal)this[39, colIndex].Value;
                    if (this[41, colIndex].Value == null || ((LongPair)this[41, colIndex].Value).Value == -1)
                        c_row.Setpaper_idNull();
                    else
                        c_row.paper_id = ((LongPair)this[41, colIndex].Value).Value;
                    c_row.paperweight_id = ((IntPair)this[42, colIndex].Value).Value;
                    if (this[43, colIndex].Value == null || ((IntPair)this[43, colIndex].Value).Value == -1)
                        c_row.Setpapergrade_idNull();
                    else
                        c_row.papergrade_id = ((IntPair)this[43, colIndex].Value).Value;
                    if (this[44, colIndex].Value == null || ((LongPair)this[44, colIndex].Value).Value == -1)
                        c_row.Setpaper_map_idNull();
                    else
                        c_row.paper_map_id = ((LongPair)this[44, colIndex].Value).Value;
                    if (!c_row.vendorsupplied)
                        c_row.calculatepapercost = (bool)this[45, colIndex].Value;
                    else
                        c_row.calculatepapercost = false;
                    if (this[46, colIndex].Value == null)
                        c_row.SetpapercostNull();
                    else
                        c_row.papercost = (decimal)this[46, colIndex].Value;
                    if (this[47, colIndex].Value == null)
                        c_row.SetrunpoundsNull();
                    else
                        c_row.runpounds = (decimal)this[47, colIndex].Value;
                    if (this[48, colIndex].Value == null)
                        c_row.SetmakereadypoundsNull();
                    else
                        c_row.makereadypounds = (int)this[48, colIndex].Value;
                    if (this[49, colIndex].Value == null)
                        c_row.SetplatechangepoundsNull();
                    else
                        c_row.platechangepounds = (decimal)this[49, colIndex].Value;
                    if (this[50, colIndex].Value == null)
                        c_row.SetpressstoppoundsNull();
                    else
                        c_row.pressstoppounds = (int)this[50, colIndex].Value;
                    if (this[51, colIndex].Value == null)
                        c_row.SetnumberofpressstopsNull();
                    else
                        c_row.numberofpressstops = (int)this[51, colIndex].Value;
                    if (this[52, colIndex].Value == null)
                        c_row.SetearlypaypaperdiscountNull();
                    else
                        c_row.earlypaypaperdiscount = (decimal)this[52, colIndex].Value;
                    if (!c_row.vendorsupplied)
                        c_row.paperapplytax = (bool)this[53, colIndex].Value;
                    else
                        c_row.paperapplytax = false;
                    if (this[54, colIndex].Value == null)
                        c_row.SetpapertaxablemediapctNull();
                    else
                        c_row.papertaxablemediapct = (decimal)this[54, colIndex].Value;
                    if (this[55, colIndex].Value == null)
                        c_row.SetpapersalestaxpctNull();
                    else
                        c_row.papersalestaxpct = (decimal)this[55, colIndex].Value;
                    if (this[57, colIndex].Value == null || ((LongPair)this[57, colIndex].Value).Value == -1)
                        c_row.Setassemblyvendor_idNull();
                    else
                        c_row.assemblyvendor_id = ((LongPair)this[57, colIndex].Value).Value;
                    if (this[58, colIndex].Value == null || ((LongPair)this[58, colIndex].Value).Value == -1)
                        c_row.Setstitchin_idNull();
                    else
                        c_row.stitchin_id = ((LongPair)this[58, colIndex].Value).Value;
                    if (this[59, colIndex].Value == null || ((LongPair)this[59, colIndex].Value).Value == -1)
                        c_row.Setblowin_idNull();
                    else
                        c_row.blowin_id = ((LongPair)this[59, colIndex].Value).Value;
                    if (this[60, colIndex].Value == null || ((LongPair)this[60, colIndex].Value).Value == -1)
                        c_row.Setonsert_idNull();
                    else
                        c_row.onsert_id = ((LongPair)this[60, colIndex].Value).Value;

                    if (this[62, colIndex].Value == null)
                        c_row.SetotherproductionNull();
                    else
                        c_row.otherproduction = (decimal)this[62, colIndex].Value;

                    if (c_row.RowState == DataRowState.Detached)
                    {
                        c_row.est_estimate_id = _dsEstimates.est_estimate[0].est_estimate_id;
                        c_row.createdby = MainForm.AuthorizedUser.FormattedName;
                        c_row.createddate = DateTime.Now;
                        _dsEstimates.est_component.Addest_componentRow(c_row);
                    }
                    else
                    {
                        c_row.modifiedby = MainForm.AuthorizedUser.FormattedName;
                        c_row.modifieddate = DateTime.Now;
                    }
                }
            }
        }

        private void ClearErrors()
        {
            for (int rowIndex = 1; rowIndex < this.Rows.Count; ++rowIndex)
                if (rowIndex != 14 && rowIndex != 22 && rowIndex != 40 && rowIndex != 56 && rowIndex != 61)
                {
                    for (int colIndex = 1; colIndex <= _Components.Count; ++colIndex)
                    {
                            if (this[rowIndex, colIndex].View == _vwInvalid)
                                this[rowIndex, colIndex].View = SourceGrid.Cells.Views.Cell.Default;
                            if (this[rowIndex, colIndex].View == _vwComboInvalid)
                                this[rowIndex, colIndex].View = SourceGrid.Cells.Views.ComboBox.Default;
                    }
                }
        }

        public bool Validate()
        {
            if (!this.Selection.ActivePosition.IsEmpty())
            {
                EditorBase currentEditor = this[this.Selection.ActivePosition.Row, this.Selection.ActivePosition.Column].Editor as EditorBase;
                if (currentEditor != null)
                    currentEditor.ApplyEdit();
            }

            bool isValid = true;

            ClearErrors();

            for (int i = 0; i < _Components.Count; ++i)
            {
                if (!_blankColumn[i])
                {
                    // Description is required
                    if (this[2, i + 1].Value == null || this[2, i + 1].Value.ToString().Trim() == String.Empty)
                    {
                        this[2, i + 1].View = _vwInvalid;
                        isValid = false;
                    }
                    // Estimate Media Type is required
                    if (this[6, i + 1].Value == null)
                    {
                        this[6, i + 1].View = _vwComboInvalid;
                        isValid = false;
                    }
                    // Component Type is required
                    if (this[7, i + 1].Value == null)
                    {
                        this[7, i + 1].View = _vwComboInvalid;
                        isValid = false;
                    }
                    // PageCount is required
                    if (this[10, i + 1].Value == null)
                    {
                        this[10, i + 1].View = _vwInvalid;
                        isValid = false;
                    }
                    // Width is required
                    if (this[11, i + 1].Value == null)
                    {
                        this[11, i + 1].View = _vwInvalid;
                        isValid = false;
                    }
                    // Height is required
                    if (this[12, i + 1].Value == null)
                    {
                        this[12, i + 1].View = _vwInvalid;
                        isValid = false;
                    }
                    // If Vendor Supplied is checked, a VS Vendor must be selected
                    if (((bool)this[15, i + 1].Value) && (this[16, i + 1].Value == null || ((LongPair) this[16,i+1].Value).Value == -1))
                    {
                        this[16, i + 1].View = _vwComboInvalid;
                        isValid = false;
                    }

                    // Creative Vendor is required

                    // Printer Vendor is required if the Component is NOT VS
                    if (!((bool)this[15,i+1].Value) && this[23, i + 1].Value == null)
                    {
                        this[23, i + 1].View = _vwComboInvalid;
                        isValid = false;
                    }

                    // If Calc Print Cost is unchecked a Manual Print Cost must be entered
                    if (!((bool)this[24, i + 1].Value))
                    {
                        if (this[25, i + 1].Value == null && !((bool)this[15, i + 1].Value))
                        {
                            this[25, i + 1].View = _vwInvalid;
                            isValid = false;
                        }
                    }
                    // If Calc Print Cost is checked many of the print cost fields are required
                    else
                    {

                        // Plate Cost
                        if (this[27, i + 1].Value == null && !((bool)this[15, i + 1].Value))
                        {
                            this[27, i + 1].View = _vwComboInvalid;
                            isValid = false;
                        }

                        // Digital Handle & Prepare Rate
                        if (this[31, i + 1].Value == null && !((bool)this[15, i + 1].Value))
                        {
                            this[31, i + 1].View = _vwComboInvalid;
                            isValid = false;
                        }
                    }

                    // Paper Vendor - required if VS is NOT checked
                    if (!((bool)this[15,i+1].Value) && this[41, i + 1].Value == null)
                    {
                        this[41, i + 1].View = _vwComboInvalid;
                        isValid = false;
                    }
                    // Paper Weight
                    if (this[42, i + 1].Value == null)
                    {
                        this[42, i + 1].View = _vwComboInvalid;
                        isValid = false;
                    }
                    // Paper Grade - required if VS is NOT checked
                    if (!((bool)this[15,i+1].Value) && this[43, i + 1].Value == null)
                    {
                        this[43, i + 1].View = _vwComboInvalid;
                        isValid = false;
                    }
                    
                    // If the Component is not VS, the Paper Description (Paper Map) is required.
                    if (!(bool)this[15, i + 1].Value && this[44, i+1].Value == null)
                    {
                        this[44, i + 1].View = _vwComboInvalid;
                        isValid = false;
                    }

                    // If Calc Paper Cost is not checked, Manual Paper Cost is required (if not VS)
                    if (!((bool)this[45, i + 1].Value))
                    {
                        if (this[46, i + 1].Value == null && !((bool)this[15, i + 1].Value))
                        {
                            this[46, i + 1].View = _vwInvalid;
                            isValid = false;
                        }
                    }

                    // Assembly Vendor (required for Stitch-In, Blow-In or Onsert)
                    if (this[7, i + 1].Value != null
                        && (((IntPair)this[7, i + 1].Value).Value == 2 || ((IntPair)this[7, i + 1].Value).Value == 3 || ((IntPair)this[7, i + 1].Value).Value == 4)
                        && (this[57,i+1].Value == null || ((LongPair)this[57,i+1].Value).Value == -1))
                    {
                        this[57, i + 1].View = _vwInvalid;
                        isValid = false;
                    }

                    // Stitch-In (required if ComponentType is Stitch-In)
                    if (this[7, i + 1].Value != null && ((IntPair)this[7, i + 1].Value).Value == 3 && (this[58, i + 1].Value == null || ((LongPair) this[58, i+1].Value).Value == -1))
                    {
                        this[58, i + 1].View = _vwComboInvalid;
                        isValid = false;
                    }
                    // Blow-In (required if ComponentType is Blow-In)
                    if (this[7, i + 1].Value != null && ((IntPair)this[7, i + 1].Value).Value == 4 && (this[59, i + 1].Value == null || ((LongPair) this[59,i+1].Value).Value == -1))
                    {
                        this[59, i + 1].View = _vwComboInvalid;
                        isValid = false;
                    }
                    // Onsert (required if ComponentType is Onsert)
                    if (this[7, i + 1].Value != null && ((IntPair)this[7, i + 1].Value).Value == 2 && (this[60, i + 1].Value == null || ((LongPair) this[60,i+1].Value).Value == -1))
                    {
                        this[60, i + 1].View = _vwComboInvalid;
                        isValid = false;
                    }
                }
            }

            if (!isValid)
                this.Selection.ResetSelection(false);

            this.Refresh();

            return isValid;
        }

        public void DeleteSelectedComponents()
        {
            if (this.SelectionMode != GridSelectionMode.Column)
                return;

            if (this.Selection.IsSelectedColumn(1))
                return;

            List<int> selectedColumns = new List<int>();

            for (int colIndex = (this.ColumnsCount - 1); colIndex > 0; --colIndex)
                if (this.Selection.IsSelectedColumn(colIndex))
                    selectedColumns.Add(colIndex);

            foreach (int selected_colIndex in selectedColumns)
                DeleteColumn(selected_colIndex);

            this.Selection.ResetSelection(false);
        }

        #endregion

    }
}
