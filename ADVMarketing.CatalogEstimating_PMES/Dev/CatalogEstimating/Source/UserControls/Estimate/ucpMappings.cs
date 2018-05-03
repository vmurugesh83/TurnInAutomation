#region Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.EstimatesTableAdapters;
using CatalogEstimating.Datasets.DistributionMappingTableAdapters;
using CatalogEstimating.Properties;
using CatalogEstimating.CustomGrids.Controllers;
using CatalogEstimating.CustomGrids.Component.Editors;
using CatalogEstimating.CustomGrids.Component.Controllers;
using SourceGrid;
using SourceGrid.Cells.Editors;
#endregion

namespace CatalogEstimating.UserControls.Estimate
{
    public partial class ucpMappings : CatalogEstimating.UserControlPanel
    {
        #region Local Variables and Constants
        private const int _InsertQty = 2;
        private const int _SoloQty = 3;
        private const int _PolyQty = 4;
        private const int _TotalMailQty = 5;
        private const int _OtherQty = 6;
        private const int _TotalQty = 7;
        private const int _PackageWeight = 8;

        private bool _readOnly = true;
        public Datasets.Estimates _dsEstimate;
        public Datasets.DistributionMapping _dsDistMapping;

        List<long> _pkgIDs = new List<long>();
        List<long> _compIDs = new List<long>();

        private bool _GridInitialized = false;
        private int _NumberOfComponents = 0;
        private int _NumberOfPackages = 0;

        private int _total_InsertQty = 0;
        private int _total_SoloQty = 0;
        private int _total_PolyQty = 0;
        private int _total_TotalMailQty = 0;
        private int _total_OtherQty = 0;
        private int _total_TotalQty = 0;

        private SourceGrid.Cells.Views.ColumnHeader _viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
        private SourceGrid.Cells.Views.ColumnHeader _viewRotate = new SourceGrid.Cells.Views.ColumnHeader();
        private SourceGrid.Cells.Views.ColumnHeader _viewRotateDisagree = new SourceGrid.Cells.Views.ColumnHeader();
        private SourceGrid.Cells.Views.RowHeader _viewRowHeader = new SourceGrid.Cells.Views.RowHeader();

        private SourceGrid.Cells.Views.Cell _viewDisableScenarioCell = new SourceGrid.Cells.Views.Cell();
        private SourceGrid.Cells.Views.Cell _viewInvalidDisableScenarioCell = new SourceGrid.Cells.Views.Cell();
        private SourceGrid.Cells.Views.Cell _viewEnableTextCell = new SourceGrid.Cells.Views.Cell();
        private SourceGrid.Cells.Views.Cell _viewDisableTextCell = new SourceGrid.Cells.Views.Cell();
        private SourceGrid.Cells.Views.Cell _viewInvalidEnabledTextCell = new SourceGrid.Cells.Views.Cell();
        private SourceGrid.Cells.Views.Cell _viewInvalidDisabledTextCell = new SourceGrid.Cells.Views.Cell();

        private SourceGrid.Cells.Views.Cell _viewEnableCell = new SourceGrid.Cells.Views.Cell();
        private SourceGrid.Cells.Views.Cell _viewDisableCell = new SourceGrid.Cells.Views.Cell();

        private SourceGrid.Cells.Views.Cell _viewInvalidEnabledCell = new SourceGrid.Cells.Views.Cell();

        private SourceGrid.Cells.Views.CheckBox _viewCheckBoxDisable = new SourceGrid.Cells.Views.CheckBox();
        private SourceGrid.Cells.Views.CheckBox _viewCheckBoxEnable = new SourceGrid.Cells.Views.CheckBox();
        #endregion

        #region Construction

        public ucpMappings()
        {
            InitializeComponent();
            Name = "Mappings";
        }

        public ucpMappings(Datasets.Estimates dsEstimate, Datasets.DistributionMapping dsDistMapping, bool readOnly)
            :this()
        {
            _dsEstimate = dsEstimate;
            _dsDistMapping = dsDistMapping;
            _readOnly = readOnly;

            if (_readOnly)
            {
                _btnDelete.Enabled = false;
            }
        }

        #endregion

        #region Overrides

        public override void Reload()
        {
            if (_dsEstimate.est_component.Count > 0)
            {
                _lblInfoText.Visible = false;
                _lblInfoText.Text = string.Empty;
                _gridDistMapping.Visible = true;

                _NumberOfComponents = _dsEstimate.est_component.Select("", "", DataViewRowState.CurrentRows).Length;
                _NumberOfPackages = _dsEstimate.est_package.Select("", "", DataViewRowState.CurrentRows).Length;

                _GridInitialized = false;
                InitMappingGrid();
                _GridInitialized = true;
            }
            else
            {
                _lblInfoText.Visible = true;
                _lblInfoText.Text = Resources.ComponentRequiredToMap;
                _gridDistMapping.Visible = false;
            }
        }

        public override void OnLeaving(CancelEventArgs e)
        {
            WriteToDataTable();
        }

        public override void PreSave(CancelEventArgs e)
        {
            Position pos = _gridDistMapping.Selection.ActivePosition;
            if ((pos.Row > -1) && (pos.Column > -1))
            {
                CellContext focusCellContext = new CellContext(_gridDistMapping, _gridDistMapping.Selection.ActivePosition);
                if (focusCellContext.IsEditing())
                    focusCellContext.EndEdit(false);
            }

            WriteToDataTable();

            if (!Valid())
                e.Cancel = true;
        }

        public override ToolStrip Toolbar
        {
            get
            {
                return toolStrip1;
            }
        }

        public override void Export( ref ExcelWriter writer )
        {
            // The Column headers appear in either row 0 or row 1.  So first check 0 for a value, 
            // and if there is no value, check 1
            string[] headerParams = new string[_gridDistMapping.Columns.Count];
            for ( int iCol = 0; iCol < _gridDistMapping.Columns.Count; iCol++ )
            {
                SourceGrid.Cells.ColumnHeader header = null;
                if ( _gridDistMapping[0,iCol] != null )
                    header = (SourceGrid.Cells.ColumnHeader)_gridDistMapping[0,iCol];
                else if ( _gridDistMapping[1,iCol] != null )
                    header = (SourceGrid.Cells.ColumnHeader)_gridDistMapping[1,iCol];

                if ( header != null )
                    headerParams[iCol] = header.DisplayText;
            }
            writer.WriteLine( headerParams );

            // Now write out all the rows in the grid excluding the header rows and the "new row" at the bottom
            for ( int iRow = 2; iRow < _gridDistMapping.RowsCount - 1; iRow++ )
            {
                string[] rowParams = new string[_gridDistMapping.Columns.Count];
                for ( int iCol = 0; iCol < _gridDistMapping.Columns.Count; iCol++ )
                    rowParams[iCol] = _gridDistMapping[iRow, iCol].DisplayText;

                writer.WriteLine( rowParams );
            }

            writer.WriteLine();

            // Finally, write out the Totals row
            string[] totalParams = new string[_gridDistMapping.Columns.Count];

            totalParams[_NumberOfComponents + _InsertQty - 1] = _gridTotals[0, 0].Value.ToString();
            totalParams[_NumberOfComponents + _InsertQty] = _gridTotals[0, _NumberOfComponents + _InsertQty].Value.ToString();
            totalParams[_NumberOfComponents + _SoloQty] = _gridTotals[0, _NumberOfComponents + _SoloQty].Value.ToString();
            totalParams[_NumberOfComponents + _PolyQty] = _gridTotals[0, _NumberOfComponents + _PolyQty].Value.ToString();
            totalParams[_NumberOfComponents + _TotalMailQty] = _gridTotals[0, _NumberOfComponents + _TotalMailQty].Value.ToString();
            totalParams[_NumberOfComponents + _OtherQty] = _gridTotals[0, _NumberOfComponents + _OtherQty].Value.ToString();
            totalParams[_NumberOfComponents + _TotalQty] = _gridTotals[0, _NumberOfComponents + _TotalQty].Value.ToString();
            
            writer.WriteLine( totalParams );
        }

        #endregion

        #region Event Handlers

        private void ucpMappings_Load( object sender, EventArgs e )
        {
            #region Cell Visual Styles
            _viewColumnHeader.BackColor = System.Drawing.SystemColors.Control;
            _viewColumnHeader.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

            _viewRotate.ElementText = new CustomGrids.RotatedText(-90);
            _viewRotate.TextAlignment = DevAge.Drawing.ContentAlignment.BottomLeft;
            _viewRotate.BackColor = System.Windows.Forms.Control.DefaultBackColor;

            _viewRotateDisagree.ElementText = new CustomGrids.RotatedText(-90);
            _viewRotateDisagree.TextAlignment = DevAge.Drawing.ContentAlignment.BottomLeft;
            _viewRotateDisagree.BackColor = System.Windows.Forms.Control.DefaultBackColor;
            //_viewRotateDisagree.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(System.Drawing.Color.Blue, 2));
            //_viewRotateDisagree.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Blue, 5), new DevAge.Drawing.BorderLine(Color.DarkGray));
            _viewRotateDisagree.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(System.Drawing.Color.Blue, 10), new DevAge.Drawing.BorderLine(System.Drawing.Color.DarkGray), new DevAge.Drawing.BorderLine(System.Drawing.Color.DarkGray), new DevAge.Drawing.BorderLine(System.Drawing.Color.DarkGray));

            _viewRowHeader.BackColor = System.Windows.Forms.Control.DefaultBackColor;
            _viewRowHeader.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

            _viewDisableScenarioCell.BackColor = System.Drawing.SystemColors.Control;
            _viewDisableScenarioCell.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
            _viewDisableScenarioCell.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Blue, 5), new DevAge.Drawing.BorderLine(Color.DarkGray));
            _viewInvalidDisableScenarioCell.BackColor = System.Drawing.SystemColors.Control;
            _viewInvalidDisableScenarioCell.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
            _viewInvalidDisableScenarioCell.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(System.Drawing.Color.Red, 2), new DevAge.Drawing.BorderLine(System.Drawing.Color.Red, 2),new DevAge.Drawing.BorderLine(System.Drawing.Color.Red, 2), new DevAge.Drawing.BorderLine(System.Drawing.Color.Blue, 6));

            _viewEnableCell.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            _viewDisableCell.BackColor = System.Drawing.SystemColors.Control;
            _viewDisableCell.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            _viewDisableCell.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.DarkGray), new DevAge.Drawing.BorderLine(Color.DarkGray));

            _viewEnableTextCell.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
            _viewDisableTextCell.BackColor = System.Drawing.SystemColors.Control;
            _viewDisableTextCell.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

            _viewInvalidEnabledCell.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(System.Drawing.Color.Red, 2));
            _viewInvalidEnabledCell.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

            _viewInvalidEnabledTextCell.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(System.Drawing.Color.Red, 2));
            _viewInvalidEnabledTextCell.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
            _viewInvalidDisabledTextCell.BackColor = System.Drawing.SystemColors.Control;
            _viewInvalidDisabledTextCell.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
            _viewInvalidDisabledTextCell.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(System.Drawing.Color.Red, 2));

            _viewCheckBoxDisable.BackColor = System.Windows.Forms.Control.DefaultBackColor;
            _viewCheckBoxDisable.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.DarkGray), new DevAge.Drawing.BorderLine(Color.DarkGray));
            #endregion
        }

        private void ucpMappings_Resize(object sender, EventArgs e)
        {
            if (_GridInitialized)
                CalcSizeLocOfTotals();
        }

        private void _btnDelete_Click(object sender, EventArgs e)
        {
            string deleteGroup = string.Empty;
            string deletePolybag = string.Empty;
            string deleteResult = string.Empty;
            string filter = string.Empty;
            DataView compMap = new DataView(_dsEstimate.est_packagecomponentmapping);
            DataView pkgQty = new DataView(_dsEstimate.EstPackage_Quantities_ByEstimateID);
            DataView pkgs = new DataView(_dsEstimate.est_package);
            Estimates.est_packageRow package = null;

            if (_gridDistMapping.SelectionMode != GridSelectionMode.Row)
                return;

            List<int> idxs = new List<int>();

            int startingRow = _gridDistMapping.RowsCount - 2;

            for (int rowIndex = startingRow; rowIndex > 1; --rowIndex)
            {
                if (_gridDistMapping.Selection.IsSelectedRow(rowIndex))
                {
                    if (rowIndex - 1 > _pkgIDs.Count)
                    {
                        idxs.Add(rowIndex);
                        //_gridDistMapping.Rows.Remove(rowIndex);
                    }
                    else
                    {
                        //get package
                        filter = string.Concat("est_package_id = ", _pkgIDs[rowIndex - 2].ToString());
                        pkgs.RowFilter = filter;
                        package = (Estimates.est_packageRow)pkgs[0].Row;
                        if (package.groupFlag)
                        {
                            if (deleteGroup == string.Empty)
                                deleteGroup = "The following packages must be removed using the 'Insert Setup' tab:";

                            deleteGroup += string.Concat("\n\t", package.description);
                        }
                        else
                        {
                            if (IsInPolybag(package.est_package_id))
                            {
                                //error msg "cannot delete (id) because of polybag"     
                                if (deletePolybag == string.Empty)
                                    deletePolybag = "The following packages are in polybags and cannot be deleted:";

                                deletePolybag += string.Concat("\n\t", package.description);
                            }
                            else
                            {
                                Dirty = true;

                                //delete package to component maps
                                compMap.RowFilter = filter;
                                pkgQty.RowFilter = string.Concat("est_package_id = ", package.est_package_id);

                                for (int comp = compMap.Count - 1; comp >= 0; comp--)
                                {
                                    compMap[comp].Delete();
                                }

                                for (int pQty = pkgQty.Count - 1; pQty >= 0; pQty--)
                                {
                                    pkgQty[pQty].Delete();
                                }

                                DataView pts = new DataView(_dsDistMapping.MappingTotals);
                                pts.RowFilter = string.Concat("item_type = 'package' and item_id = ", package.est_package_id);
                                if (pts.Count > 0)
                                    pts[0].Delete();

                                _pkgIDs.RemoveAt(rowIndex - 2);
                                _NumberOfPackages = _pkgIDs.Count;

                                //delete package
                                package.Delete();
                                idxs.Add(rowIndex);
                                //_gridDistMapping.Rows.Remove(rowIndex);
                            }
                        }
                    }
                }
            }

            foreach (int i in idxs)
            {
                _gridDistMapping.Rows.Remove(i);
            }

            if ((deleteGroup != string.Empty) && (deletePolybag != string.Empty))
                deleteResult = string.Concat(deleteGroup, "\n\n", deletePolybag);
            else if ((deleteGroup != string.Empty) && (deletePolybag == string.Empty))
                deleteResult = deleteGroup;
            else if ((deleteGroup == string.Empty) && (deletePolybag != string.Empty))
                deleteResult = deletePolybag;

            if (deleteResult != string.Empty)
                MessageBox.Show(deleteResult, "Delete Packages", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _gridDistMapping.SelectionMode = GridSelectionMode.Cell;
            _gridDistMapping.Selection.EnableMultiSelection = false;

            CalculateGridTotals();
            UpdateStats();
        }

        void Grid_CellValueChanged(SourceGrid.CellContext sender, EventArgs e)
        {
            if ((sender.CellRange.Start.Column > 1) && (sender.CellRange.Start.Column < _NumberOfComponents + 2))
            {
                // Call manually for Checkboxes.
                _gridDistMapping.OnRowClicked(sender, null);
            }

            if (sender.CellRange.Start.Row < (_gridDistMapping.RowsCount - 1))
            {
                Dirty = true;
                CalculateGridTotals();
            }

            if (sender.CellRange.Start.Column == 1 && sender.CellRange.Start.Row == (_gridDistMapping.RowsCount - 1))
            {
                //if (_gridDistMapping[row, 1].Value.ToString().Length > 35)
                //{
                //    MessageBox("The description must be no more than 35 characters.  Please correct.", "Max length Exceeded.);
                //}
                Dirty = true;
                AddMailerPackage(sender.CellRange.Start.Row);
                AddGridRow();
                CalculateGridTotals();
            }

            UpdateStats();
        }

        private void _gridDistMapping_HScrollPositionChanged(object sender, ScrollPositionChangedEventArgs e)
        {
            _gridTotals.HScrollBar.Value = _gridDistMapping.HScrollBar.Value;

            Position pos = _gridDistMapping.Selection.ActivePosition;
            if ((pos.Row > -1) && (pos.Column > -1))
            {
                CellContext focusCellContext = new CellContext(_gridDistMapping, _gridDistMapping.Selection.ActivePosition);
                if (focusCellContext.IsEditing())
                    focusCellContext.EndEdit(false);
            }
        }

        private void _gridDistMapping_VScrollPositionChanged(object sender, ScrollPositionChangedEventArgs e)
        {
            Position pos = _gridDistMapping.Selection.ActivePosition;
            if ((pos.Row > -1) && (pos.Column > -1))
            {
                CellContext focusCellContext = new CellContext(_gridDistMapping, _gridDistMapping.Selection.ActivePosition);
                if (focusCellContext.IsEditing())
                    focusCellContext.EndEdit(false);
            }
        }

        #endregion

        #region Private Methods

        private void InitMappingGrid()
        {
            SourceGrid.Grid.MaxSpan = 100;

            int packages = _NumberOfPackages;
            string ad = string.Empty;

            if (!_gridDistMapping.Selection.ActivePosition.IsEmpty())
            {
                SourceGrid.Cells.Cell curCell = (SourceGrid.Cells.Cell)_gridDistMapping[_gridDistMapping.Selection.ActivePosition.Row, _gridDistMapping.Selection.ActivePosition.Column];
                EditorControlBase curEditor = curCell.Editor as EditorControlBase;
                if (curEditor != null)
                {
                    LinkedControlValue curLinkedControlValue = _gridDistMapping.LinkedControls.GetByControl(curEditor.Control);
                    if (curLinkedControlValue != null)
                        _gridDistMapping.LinkedControls.Remove(curLinkedControlValue);
                }
            }

            SourceGrid.Cells.Controllers.ToolTipText toolTipController = new SourceGrid.Cells.Controllers.ToolTipText();

            ValueChangedController valueChangedController = new ValueChangedController();
            SelectDistMapRow selectRowCtrl = new SelectDistMapRow();
            bool checkValue = false;

            _dsEstimate.est_package.DefaultView.Sort = "groupFlag DESC, description ASC";

            if (_readOnly)
            {
                _gridDistMapping.Redim(2 + packages, 9 + _NumberOfComponents);
            }
            else
            {
                _gridDistMapping.Redim(3 + packages, 9 + _NumberOfComponents);
            }
            _gridTotals.Redim(1, 9 + _NumberOfComponents);

            _gridDistMapping.FixedRows = 2;

            _pkgIDs = new List<long>();
            _compIDs = new List<long>();
            _dsDistMapping.MappingTotals.Clear();

            #region Editors
            CatalogEstimating.CustomGrids.Component.Editors.TextBoxIntegerRange editorQuantity = new CatalogEstimating.CustomGrids.Component.Editors.TextBoxIntegerRange(0, 999999999);
            editorQuantity.AllowNull = true;
            SourceGrid.Cells.Editors.TextBox editorText = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            editorText.Control.MaxLength = 35;
            editorText.AllowNull = false;
            if (_readOnly)
            {
                editorQuantity.EnableEdit = false;
                editorText.EnableEdit = false;
            }
            #endregion

            #region Headers

            #region Row Headers

            if (!_readOnly)
            {
                SourceGrid.Cells.Cell cellAddRecord = new SourceGrid.Cells.Cell("*");
                cellAddRecord.View = _viewRowHeader;
                _gridDistMapping[packages + 2, 0] = cellAddRecord;
                selectRowCtrl = new SelectDistMapRow();
                _gridDistMapping[packages + 2, 0].AddController(selectRowCtrl);
            }

            for (int i = 2; i < (packages + 2); ++i)
            {
                _gridDistMapping[i, 0] = new SourceGrid.Cells.Cell();
                if (_readOnly)
                    _gridDistMapping[i, 0].View = _viewDisableCell;
                else
                {
                    _gridDistMapping[i, 0].View = _viewRowHeader;
                    selectRowCtrl = new SelectDistMapRow();
                    _gridDistMapping[i, 0].AddController(selectRowCtrl);
                }
            }
            #endregion

            #region Column Headers
            SourceGrid.Cells.Controllers.IController sortCtrl = null;
            
            _gridDistMapping[1, 1] = new SourceGrid.Cells.ColumnHeader("Dist Breakdown");
            _gridDistMapping[1, 1].View = _viewColumnHeader;
            _gridDistMapping.Columns[1].Width = 200;
            _gridTotals.Columns[1].Width = 200;
            sortCtrl = _gridDistMapping[1, 1].FindController(typeof(SourceGrid.Cells.Controllers.SortableHeader));
            if (sortCtrl != null)
                _gridDistMapping[1, 1].RemoveController(sortCtrl);

            Estimates.est_componentRow compRow = null;
            DataView compDS = new DataView(_dsEstimate.est_component, "", "", DataViewRowState.CurrentRows);
            int colOffset = 2;

            #region Host Component
            compDS.RowFilter = string.Concat("est_componenttype_id = 1");

            if (compDS.Count > 0)
            {
                compRow = (Estimates.est_componentRow)compDS[0].Row;
                _compIDs.Add(compRow.est_component_id);
                _gridDistMapping[1, colOffset] = null;
                if (compRow.IsadnumberNull())
                    ad = string.Empty;
                else
                    ad = string.Concat("[", compRow.adnumber.ToString(), "] ");
                _gridDistMapping[0, colOffset] = new SourceGrid.Cells.ColumnHeader(string.Concat("(", compRow.est_component_id.ToString(), ")* - ", ad, compRow.description));
                _gridDistMapping[0, colOffset].RowSpan = 2;
                _gridDistMapping.Columns[colOffset].Width = 25;
                _gridTotals.Columns[colOffset].Width = 25;

                _gridDistMapping[0, colOffset].AddController(new SourceGrid.Cells.Controllers.ToolTipText());
                sortCtrl = _gridDistMapping[0, colOffset].FindController(typeof(SourceGrid.Cells.Controllers.SortableHeader));
                if (sortCtrl != null)
                    _gridDistMapping[0, colOffset].RemoveController(sortCtrl);

                _gridDistMapping[0, colOffset].View = _viewRotate;

                DistributionMapping.MappingTotalsRow mtr = _dsDistMapping.MappingTotals.NewMappingTotalsRow();
                mtr.BeginEdit();
                mtr.item_id = compRow.est_component_id;
                mtr.item_type = "component";
                mtr.Weight = compRow.CalculateWeight();
                mtr.QtyInserts = 0;
                mtr.QtyPoly = 0;
                mtr.QtySolo = 0;
                mtr.QtyOther = 0;
                mtr.EndEdit();
                _dsDistMapping.MappingTotals.AddMappingTotalsRow(mtr);

                colOffset += 1;
            }
            #endregion

            #region Non-Host Existing Component
            compDS.RowFilter = string.Concat("est_componenttype_id > 1 and est_component_id > 0");
            compDS.Sort = "est_component_id ASC";

            // Rotated text for each component
            for (int i = 0; i <= compDS.Count - 1; ++i)
            {
                compRow = (Estimates.est_componentRow)compDS[i].Row;
                _compIDs.Add(compRow.est_component_id);
                _gridDistMapping[1, i + colOffset] = null;
                if (compRow.IsadnumberNull())
                    ad = string.Empty;
                else
                    ad = string.Concat("[", compRow.adnumber.ToString(), "] ");
                _gridDistMapping[0, i + colOffset] = new SourceGrid.Cells.ColumnHeader(string.Concat("(", compRow.est_component_id.ToString(), ") - ", ad, compRow.description));
                _gridDistMapping[0, i + colOffset].RowSpan = 2;
                _gridDistMapping.Columns[i + colOffset].Width = 25;
                _gridTotals.Columns[i + colOffset].Width = 25;

                _gridDistMapping[0, i + colOffset].AddController(new SourceGrid.Cells.Controllers.ToolTipText());
                sortCtrl = _gridDistMapping[0, i + colOffset].FindController(typeof(SourceGrid.Cells.Controllers.SortableHeader));
                if (sortCtrl != null)
                    _gridDistMapping[0, i + colOffset].RemoveController(sortCtrl);

                _gridDistMapping[0, i + colOffset].View = _viewRotate;

                DistributionMapping.MappingTotalsRow mtr = _dsDistMapping.MappingTotals.NewMappingTotalsRow();
                mtr.BeginEdit();
                mtr.item_id = compRow.est_component_id;
                mtr.item_type = "component";
                mtr.Weight = compRow.CalculateWeight();
                mtr.QtyInserts = 0;
                mtr.QtyPoly = 0;
                mtr.QtySolo = 0;
                mtr.QtyOther = 0;
                mtr.EndEdit();
                _dsDistMapping.MappingTotals.AddMappingTotalsRow(mtr);
            }

            colOffset += compDS.Count;
            #endregion

            #region Non-Host New Component
            compDS.RowFilter = string.Concat("est_componenttype_id > 1 and est_component_id < 0");
            compDS.Sort = "est_component_id DESC";

            // Rotated text for each component
            for (int i = 0; i <= compDS.Count - 1; ++i)
            {
                compRow = (Estimates.est_componentRow)compDS[i].Row;
                _compIDs.Add(compRow.est_component_id);
                _gridDistMapping[1, i + colOffset] = null;
                if (compRow.IsadnumberNull())
                    ad = string.Empty;
                else
                    ad = string.Concat("[", compRow.adnumber.ToString(), "] ");
                _gridDistMapping[0, i + colOffset] = new SourceGrid.Cells.ColumnHeader(string.Concat("(", compRow.est_component_id.ToString(), ") - ", ad, compRow.description));
                _gridDistMapping[0, i + colOffset].RowSpan = 2;
                _gridDistMapping.Columns[i + colOffset].Width = 25;
                _gridTotals.Columns[i + colOffset].Width = 25;

                _gridDistMapping[0, i + colOffset].AddController(new SourceGrid.Cells.Controllers.ToolTipText());
                sortCtrl = _gridDistMapping[0, i + colOffset].FindController(typeof(SourceGrid.Cells.Controllers.SortableHeader));
                if (sortCtrl != null)
                    _gridDistMapping[0, i + colOffset].RemoveController(sortCtrl);

                _gridDistMapping[0, i + colOffset].View = _viewRotate;

                DistributionMapping.MappingTotalsRow mtr = _dsDistMapping.MappingTotals.NewMappingTotalsRow();
                mtr.BeginEdit();
                mtr.item_id = compRow.est_component_id;
                mtr.item_type = "component";
                mtr.Weight = compRow.CalculateWeight();
                mtr.QtyInserts = 0;
                mtr.QtyPoly = 0;
                mtr.QtySolo = 0;
                mtr.QtyOther = 0;
                mtr.EndEdit();
                _dsDistMapping.MappingTotals.AddMappingTotalsRow(mtr);
            }

            colOffset += compDS.Count;
            #endregion

            #region Clear Unused Columns
            for (int i = colOffset; i < colOffset + 6; i++)
            {
                _gridDistMapping[0, i] = null;
            }
            #endregion

            _gridDistMapping.Rows[0].Height = 180;

            _gridDistMapping[1, _NumberOfComponents + _InsertQty] = new SourceGrid.Cells.ColumnHeader("Insert Qty");
            _gridDistMapping[1, _NumberOfComponents + _InsertQty].View = _viewColumnHeader;
            _gridDistMapping.Columns[_NumberOfComponents + _InsertQty].Width = 78;
            _gridDistMapping[1, _NumberOfComponents + _SoloQty] = new SourceGrid.Cells.ColumnHeader("Solo Qty");
            _gridDistMapping[1, _NumberOfComponents + _SoloQty].View = _viewColumnHeader;
            _gridDistMapping.Columns[_NumberOfComponents + _SoloQty].Width = 78;
            _gridDistMapping[1, _NumberOfComponents + _PolyQty] = new SourceGrid.Cells.ColumnHeader("Poly Qty");
            _gridDistMapping[1, _NumberOfComponents + _PolyQty].View = _viewColumnHeader;
            _gridDistMapping.Columns[_NumberOfComponents + _PolyQty].Width = 78;
            _gridDistMapping[1, _NumberOfComponents + _TotalMailQty] = new SourceGrid.Cells.ColumnHeader("Total Mail Qty");
            _gridDistMapping[1, _NumberOfComponents + _TotalMailQty].View = _viewColumnHeader;
            _gridDistMapping.Columns[_NumberOfComponents + _TotalMailQty].Width = 88;
            _gridDistMapping[1, _NumberOfComponents + _OtherQty] = new SourceGrid.Cells.ColumnHeader("Other Qty");
            _gridDistMapping[1, _NumberOfComponents + _OtherQty].View = _viewColumnHeader;
            _gridDistMapping.Columns[_NumberOfComponents + _OtherQty].Width = 78;
            _gridDistMapping[1, _NumberOfComponents + _TotalQty] = new SourceGrid.Cells.ColumnHeader("Total Qty");
            _gridDistMapping[1, _NumberOfComponents + _TotalQty].View = _viewColumnHeader;
            _gridDistMapping.Columns[_NumberOfComponents + _TotalQty].Width = 78;
            _gridDistMapping[1, _NumberOfComponents + _PackageWeight] = new SourceGrid.Cells.ColumnHeader("Pkg. Wgt.");
            _gridDistMapping[1, _NumberOfComponents + _PackageWeight].View = _viewColumnHeader;
            _gridDistMapping.Columns[_NumberOfComponents + _PackageWeight].Width = 78;

            _gridTotals[0, 0] = new SourceGrid.Cells.Cell();
            _gridTotals[0, 0].ColumnSpan = 2 + _NumberOfComponents;
            _gridTotals[0, 0].Editor = editorQuantity;
            _gridTotals[0, 0].View = _viewDisableCell;
            _gridTotals[0, 0].Value = "Totals:";
            _gridTotals[0, _NumberOfComponents + _InsertQty] = new SourceGrid.Cells.Cell();
            _gridTotals[0, _NumberOfComponents + _InsertQty].Editor = editorQuantity;
            _gridTotals[0, _NumberOfComponents + _InsertQty].View = _viewDisableCell;
            _gridTotals.Columns[_NumberOfComponents + _InsertQty].Width = 78;
            _gridTotals[0, _NumberOfComponents + _InsertQty].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            _gridTotals[0, _NumberOfComponents + _SoloQty] = new SourceGrid.Cells.Cell();
            _gridTotals[0, _NumberOfComponents + _SoloQty].Editor = editorQuantity;
            _gridTotals[0, _NumberOfComponents + _SoloQty].View = _viewDisableCell;
            _gridTotals.Columns[_NumberOfComponents + _SoloQty].Width = 78;
            _gridTotals[0, _NumberOfComponents + _SoloQty].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            _gridTotals[0, _NumberOfComponents + _PolyQty] = new SourceGrid.Cells.Cell();
            _gridTotals[0, _NumberOfComponents + _PolyQty].Editor = editorQuantity;
            _gridTotals[0, _NumberOfComponents + _PolyQty].View = _viewDisableCell;
            _gridTotals.Columns[_NumberOfComponents + _PolyQty].Width = 78;
            _gridTotals[0, _NumberOfComponents + _PolyQty].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            _gridTotals[0, _NumberOfComponents + _TotalMailQty] = new SourceGrid.Cells.Cell();
            _gridTotals[0, _NumberOfComponents + _TotalMailQty].Editor = editorQuantity;
            _gridTotals[0, _NumberOfComponents + _TotalMailQty].View = _viewDisableCell;
            _gridTotals.Columns[_NumberOfComponents + _TotalMailQty].Width = 88;
            _gridTotals[0, _NumberOfComponents + _TotalMailQty].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            _gridTotals[0, _NumberOfComponents + _OtherQty] = new SourceGrid.Cells.Cell();
            _gridTotals[0, _NumberOfComponents + _OtherQty].Editor = editorQuantity;
            _gridTotals[0, _NumberOfComponents + _OtherQty].View = _viewDisableCell;
            _gridTotals.Columns[_NumberOfComponents + _OtherQty].Width = 78;
            _gridTotals[0, _NumberOfComponents + _OtherQty].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            _gridTotals[0, _NumberOfComponents + _TotalQty] = new SourceGrid.Cells.Cell();
            _gridTotals[0, _NumberOfComponents + _TotalQty].Editor = editorQuantity;
            _gridTotals[0, _NumberOfComponents + _TotalQty].View = _viewDisableCell;
            _gridTotals.Columns[_NumberOfComponents + _TotalQty].Width = 78;
            _gridTotals[0, _NumberOfComponents + _TotalQty].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            _gridTotals[0, _NumberOfComponents + _PackageWeight] = new SourceGrid.Cells.Cell();
            _gridTotals[0, _NumberOfComponents + _PackageWeight].Editor = editorQuantity;
            _gridTotals[0, _NumberOfComponents + _PackageWeight].View = _viewDisableCell;
            _gridTotals.Columns[_NumberOfComponents + _PackageWeight].Width = 78;
            _gridTotals[0, _NumberOfComponents + _PackageWeight].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);

            #region Remove Sort Controller on Column Headers

            for (int col = 2 + _NumberOfComponents; col <= _gridDistMapping.ColumnsCount - 1; col++)
            {
                sortCtrl = _gridDistMapping[1, col].FindController(typeof(SourceGrid.Cells.Controllers.SortableHeader));
                if (sortCtrl != null)
                    _gridDistMapping[1, col].RemoveController(sortCtrl);
            }
            #endregion

            #endregion

            #endregion

            DataView pcMap = new DataView(_dsEstimate.est_packagecomponentmapping);
            DataView qty = new DataView(_dsEstimate.EstPackage_Quantities_ByEstimateID);
            DataView hostComp = new DataView(_dsEstimate.est_component, "est_componenttype_id = 1", "", DataViewRowState.CurrentRows);
            string existCompFilter = string.Concat("est_componenttype_id > 1 and est_component_id > 0");
            DataView existComp = new DataView(_dsEstimate.est_component, existCompFilter, "est_component_id ASC", DataViewRowState.CurrentRows);
            string newCompFilter = string.Concat("est_componenttype_id > 1 and est_component_id < 0");
            DataView newComp = new DataView(_dsEstimate.est_component, newCompFilter, "est_component_id DESC", DataViewRowState.CurrentRows);
            Estimates.EstPackage_Quantities_ByEstimateIDRow qtyRow = null;
            Estimates.est_packageRow pkgRow = null;
            int maxRowNumber = packages + 2;

            if (_readOnly)
                maxRowNumber = packages + 1;

            for (int r = 2; r <= maxRowNumber; ++r)
            {
                #region Package Name Column
                if (r == (packages + 2))
                {
                    _gridDistMapping[r, 1] = new SourceGrid.Cells.Cell("[Enter Dist Name]");
                    valueChangedController = new ValueChangedController("[Enter Dist Name]");
                    valueChangedController.CellValueChanged += new CellValueChanged(Grid_CellValueChanged);
                    _gridDistMapping[r, 1].AddController(valueChangedController);
                    selectRowCtrl = new SelectDistMapRow();
                    _gridDistMapping[r, 1].AddController(selectRowCtrl);
                }
                else
                {
                    pkgRow = (Estimates.est_packageRow)_dsEstimate.est_package.DefaultView[r - 2].Row;
                    _pkgIDs.Add(pkgRow.est_package_id);
                    _NumberOfPackages = _pkgIDs.Count;

                    _gridDistMapping[r, 1] = new SourceGrid.Cells.Cell(pkgRow.description);

                    _gridDistMapping[r, 1].AddController(toolTipController);

                    if (pkgRow.Ispub_pubgroup_idNull())
                    {
                        valueChangedController = new ValueChangedController(pkgRow.description);
                        valueChangedController.CellValueChanged += new CellValueChanged(Grid_CellValueChanged);
                        _gridDistMapping[r, 1].AddController(valueChangedController);
                        selectRowCtrl = new SelectDistMapRow();
                        _gridDistMapping[r, 1].AddController(selectRowCtrl);
                    }

                    qty.RowFilter = string.Concat("est_package_id = ", pkgRow.est_package_id);
                    qtyRow = (Estimates.EstPackage_Quantities_ByEstimateIDRow)qty[0].Row;

                    DistributionMapping.MappingTotalsRow mtr = _dsDistMapping.MappingTotals.NewMappingTotalsRow();
                    mtr.BeginEdit();
                    mtr.item_id = pkgRow.est_package_id;
                    mtr.item_type = "package";
                    mtr.Weight = 0M;
                    mtr.QtyInserts = 0;
                    mtr.QtyPoly = 0;
                    mtr.QtySolo = 0;
                    mtr.QtyOther = 0;
                    mtr.EndEdit();
                    _dsDistMapping.MappingTotals.AddMappingTotalsRow(mtr);
                }
                #endregion

                #region CheckBoxes
                for (int c = 2; c <= _NumberOfComponents + 1; ++c)
                {
                    if (r == (packages + 2))
                        pcMap.RowFilter = "1 = 2";
                    else
                    {
                        existComp.RowFilter = existCompFilter;
                        newComp.RowFilter = newCompFilter;

                        if ((c - 1) <= hostComp.Count)
                            pcMap.RowFilter = string.Concat("est_package_id = ", pkgRow.est_package_id, " and est_component_id = ", ((Estimates.est_componentRow)hostComp[c - 2].Row).est_component_id);
                        else if (((c - 1) > hostComp.Count) && ((c - 1) <= (existComp.Count + hostComp.Count)))
                            pcMap.RowFilter = string.Concat("est_package_id = ", pkgRow.est_package_id, " and est_component_id = ", ((Estimates.est_componentRow)existComp[c - 2 - hostComp.Count].Row).est_component_id);
                        else if (((c - 1) > (hostComp.Count + existComp.Count)) && ((c - 1) <= (existComp.Count + hostComp.Count + newComp.Count)))
                            pcMap.RowFilter = string.Concat("est_package_id = ", pkgRow.est_package_id, " and est_component_id = ", ((Estimates.est_componentRow)newComp[c - 2 - hostComp.Count - existComp.Count].Row).est_component_id);
                    }
                    _gridDistMapping[r, c] = new SourceGrid.Cells.CheckBox();
                    
                    if (pcMap.Count == 0)
                        checkValue = false;
                    else
                        checkValue = true;

                    _gridDistMapping[r, c].Value = checkValue;
                    valueChangedController = new ValueChangedController(checkValue);
                    valueChangedController.CellValueChanged += new CellValueChanged(Grid_CellValueChanged);
                    _gridDistMapping[r, c].AddController(valueChangedController);
                    if (_readOnly)
                    {
                        _gridDistMapping[r, c].Editor.EnableEdit = false;
                        _gridDistMapping[r, c].View = _viewCheckBoxDisable;
                        _gridDistMapping[r, c].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    }
                    else
                    {
                        _gridDistMapping[r, c].View = _viewCheckBoxEnable;
                        selectRowCtrl = new SelectDistMapRow();
                        _gridDistMapping[r, 1].AddController(selectRowCtrl);
                    }
                }
                #endregion

                #region Create Cells
                _gridDistMapping[r, _NumberOfComponents + _InsertQty] = new SourceGrid.Cells.Cell();
                _gridDistMapping[r, _NumberOfComponents + _SoloQty] = new SourceGrid.Cells.Cell();
                _gridDistMapping[r, _NumberOfComponents + _PolyQty] = new SourceGrid.Cells.Cell();
                _gridDistMapping[r, _NumberOfComponents + _TotalMailQty] = new SourceGrid.Cells.Cell();
                _gridDistMapping[r, _NumberOfComponents + _OtherQty] = new SourceGrid.Cells.Cell();
                _gridDistMapping[r, _NumberOfComponents + _TotalQty] = new SourceGrid.Cells.Cell();
                _gridDistMapping[r, _NumberOfComponents + _PackageWeight] = new SourceGrid.Cells.Cell();
                #endregion

                #region Set Views, Editors, and Controllers
                _gridDistMapping[r, _NumberOfComponents + _InsertQty].Editor = editorQuantity;
                _gridDistMapping[r, _NumberOfComponents + _InsertQty].View = _viewDisableCell;
                _gridDistMapping[r, _NumberOfComponents + _InsertQty].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                _gridDistMapping[r, _NumberOfComponents + _SoloQty].Editor = editorQuantity;
                _gridDistMapping[r, _NumberOfComponents + _PolyQty].Editor = editorQuantity;
                _gridDistMapping[r, _NumberOfComponents + _PolyQty].View = _viewDisableCell;
                _gridDistMapping[r, _NumberOfComponents + _PolyQty].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                _gridDistMapping[r, _NumberOfComponents + _TotalMailQty].Editor = editorQuantity;
                _gridDistMapping[r, _NumberOfComponents + _TotalMailQty].View = _viewDisableCell;
                _gridDistMapping[r, _NumberOfComponents + _TotalMailQty].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                _gridDistMapping[r, _NumberOfComponents + _OtherQty].Editor = editorQuantity;
                _gridDistMapping[r, _NumberOfComponents + _TotalQty].Editor = editorQuantity;
                _gridDistMapping[r, _NumberOfComponents + _PackageWeight].Editor = editorQuantity;
                if (_readOnly)
                {
                    _gridDistMapping[r, _NumberOfComponents + _OtherQty].View = _viewDisableCell;
                    _gridDistMapping[r, _NumberOfComponents + _OtherQty].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                }
                else
                {
                    _gridDistMapping[r, _NumberOfComponents + _OtherQty].View = _viewEnableCell;
                }
                _gridDistMapping[r, _NumberOfComponents + _TotalQty].View = _viewDisableCell;
                _gridDistMapping[r, _NumberOfComponents + _TotalQty].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                _gridDistMapping[r, _NumberOfComponents + _PackageWeight].View = _viewDisableCell;
                _gridDistMapping[r, _NumberOfComponents + _PackageWeight].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                #endregion

                if ((r == packages + 2) || (pkgRow.Ispub_pubgroup_idNull()))
                {
                    #region Set Views, Editors, and Controllers
                    _gridDistMapping[r, 1].Editor = editorText;
                    if (_readOnly)
                    {
                        _gridDistMapping[r, 1].View = _viewDisableTextCell;
                        _gridDistMapping[r, 1].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                        _gridDistMapping[r, _NumberOfComponents + _SoloQty].View = _viewDisableCell;
                        _gridDistMapping[r, _NumberOfComponents + _SoloQty].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    }
                    else
                    {
                        _gridDistMapping[r, 1].View = _viewEnableTextCell;
                        _gridDistMapping[r, _NumberOfComponents + _SoloQty].View = _viewEnableCell;
                    }
                    #endregion

                    #region Quantities
                    if (r == packages + 2)
                    {
                        _gridDistMapping[r, _NumberOfComponents + _InsertQty].Value = 0;
                        _gridDistMapping[r, _NumberOfComponents + _SoloQty].Value = 0;
                        valueChangedController = new ValueChangedController(0);
                        valueChangedController.CellValueChanged += new CellValueChanged(Grid_CellValueChanged);
                        _gridDistMapping[r, _NumberOfComponents + _SoloQty].AddController(valueChangedController);
                        selectRowCtrl = new SelectDistMapRow();
                        _gridDistMapping[r, _NumberOfComponents + _SoloQty].AddController(selectRowCtrl);
                        _gridDistMapping[r, _NumberOfComponents + _PolyQty].Value = 0;
                        _gridDistMapping[r, _NumberOfComponents + _TotalMailQty].Value = 0;
                        _gridDistMapping[r, _NumberOfComponents + _OtherQty].Value = 0;
                        valueChangedController = new ValueChangedController(0);
                        valueChangedController.CellValueChanged += new CellValueChanged(Grid_CellValueChanged);
                        _gridDistMapping[r, _NumberOfComponents + _OtherQty].AddController(valueChangedController);
                        selectRowCtrl = new SelectDistMapRow();
                        _gridDistMapping[r, _NumberOfComponents + _OtherQty].AddController(selectRowCtrl);
                        _gridDistMapping[r, _NumberOfComponents + _TotalQty].Value = 0;
                        _gridDistMapping[r, _NumberOfComponents + _PackageWeight].Value = 0;
                    }
                    else
                    {
                        _gridDistMapping[r, _NumberOfComponents + _InsertQty].Value = 0;
                        _gridDistMapping[r, _NumberOfComponents + _SoloQty].Value = pkgRow.soloquantity;
                        valueChangedController = new ValueChangedController(pkgRow.soloquantity);
                        valueChangedController.CellValueChanged += new CellValueChanged(Grid_CellValueChanged);
                        _gridDistMapping[r, _NumberOfComponents + _SoloQty].AddController(valueChangedController);
                        selectRowCtrl = new SelectDistMapRow();
                        _gridDistMapping[r, _NumberOfComponents + _SoloQty].AddController(selectRowCtrl);
                        _gridDistMapping[r, _NumberOfComponents + _PolyQty].Value = qtyRow.polybagqty;
                        _gridDistMapping[r, _NumberOfComponents + _TotalMailQty].Value = (pkgRow.soloquantity + qtyRow.polybagqty);
                        _gridDistMapping[r, _NumberOfComponents + _OtherQty].Value = pkgRow.otherquantity;
                        valueChangedController = new ValueChangedController(pkgRow.otherquantity);
                        valueChangedController.CellValueChanged += new CellValueChanged(Grid_CellValueChanged);
                        _gridDistMapping[r, _NumberOfComponents + _OtherQty].AddController(valueChangedController);
                        selectRowCtrl = new SelectDistMapRow();
                        _gridDistMapping[r, _NumberOfComponents + _OtherQty].AddController(selectRowCtrl);
                        _gridDistMapping[r, _NumberOfComponents + _TotalQty].Value = (pkgRow.soloquantity + qtyRow.polybagqty + pkgRow.otherquantity);
                        _gridDistMapping[r, _NumberOfComponents + _PackageWeight].Value = 0;
                    }
                    #endregion
                }
                else 
                {
                    #region Set Views and Editors
                    if (pkgRow.Ispub_insertscenario_idNull())
                        _gridDistMapping[r, 1].View = _viewDisableTextCell;
                    else
                        _gridDistMapping[r, 1].View = _viewDisableScenarioCell;
                    _gridDistMapping[r, 1].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    _gridDistMapping[r, _NumberOfComponents + _SoloQty].View = _viewDisableCell;
                    _gridDistMapping[r, _NumberOfComponents + _SoloQty].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    #endregion

                    #region Quantities
                    _gridDistMapping[r, _NumberOfComponents + _InsertQty].Value = qtyRow.insertqty;
                    _gridDistMapping[r, _NumberOfComponents + _SoloQty].Value = 0;
                    _gridDistMapping[r, _NumberOfComponents + _PolyQty].Value = 0;
                    _gridDistMapping[r, _NumberOfComponents + _TotalMailQty].Value = 0;
                    _gridDistMapping[r, _NumberOfComponents + _OtherQty].Value = pkgRow.otherquantity;
                    valueChangedController = new ValueChangedController(pkgRow.otherquantity);
                    valueChangedController.CellValueChanged += new CellValueChanged(Grid_CellValueChanged);
                    _gridDistMapping[r, _NumberOfComponents + _OtherQty].AddController(valueChangedController);
                    selectRowCtrl = new SelectDistMapRow();
                    _gridDistMapping[r, _NumberOfComponents + _OtherQty].AddController(selectRowCtrl);
                    _gridDistMapping[r, _NumberOfComponents + _TotalQty].Value = (qtyRow.insertqty + pkgRow.otherquantity);
                    _gridDistMapping[r, _NumberOfComponents + _PackageWeight].Value = 0;
                    #endregion
                }
            }

            _gridTotals.HScrollBar.Visible = false;
            _gridTotals.VScrollBar.Visible = false;

            CalculateGridTotals();
            UpdateStats();
        }

        private void DisplayTotals()
        {
            _gridTotals[0, _NumberOfComponents + _InsertQty].Value = _total_InsertQty;
            _gridTotals[0, _NumberOfComponents + _SoloQty].Value = _total_SoloQty;
            _gridTotals[0, _NumberOfComponents + _PolyQty].Value = _total_PolyQty;
            _gridTotals[0, _NumberOfComponents + _OtherQty].Value = _total_OtherQty;
            _gridTotals[0, _NumberOfComponents + _TotalMailQty].Value = _total_TotalMailQty;
            _gridTotals[0, _NumberOfComponents + _TotalQty].Value = _total_TotalQty;

            CalcSizeLocOfTotals();
        }

        private void CalcSizeLocOfTotals()
        {
            _gridTotals.HScrollBar.Maximum = _gridDistMapping.HScrollBar.Maximum;

            for (int colIdx = 0; colIdx < _gridDistMapping.ColumnsCount; colIdx++)
            {
                _gridTotals.Columns[colIdx].Width = _gridDistMapping.Columns[colIdx].Width;
            }
        }

        private void ResetTotals()
        {
            _total_InsertQty = 0;
            _total_SoloQty = 0;
            _total_PolyQty = 0;
            _total_TotalMailQty = 0;
            _total_OtherQty = 0;
            _total_TotalQty = 0;

            DisplayTotals();
        }

        private void CalculateGridTotals()
        {
            ResetTotals();

            for (int row = 2; row < _gridDistMapping.RowsCount - 1; row++)
            {
                _total_InsertQty += Convert.ToInt32(_gridDistMapping[row, _NumberOfComponents + _InsertQty].Value);
                _total_SoloQty += Convert.ToInt32(_gridDistMapping[row, _NumberOfComponents + _SoloQty].Value);
                _total_PolyQty += Convert.ToInt32(_gridDistMapping[row, _NumberOfComponents + _PolyQty].Value);
                _gridDistMapping[row, _NumberOfComponents + _TotalMailQty].Value =
                    Convert.ToInt32(_gridDistMapping[row, _NumberOfComponents + _SoloQty].Value)
                    + Convert.ToInt32(_gridDistMapping[row, _NumberOfComponents + _PolyQty].Value);
                _total_TotalMailQty += Convert.ToInt32(_gridDistMapping[row, _NumberOfComponents + _TotalMailQty].Value);
                _total_OtherQty += Convert.ToInt32(_gridDistMapping[row, _NumberOfComponents + _OtherQty].Value);
                _gridDistMapping[row, _NumberOfComponents + _TotalQty].Value =
                    Convert.ToInt32(_gridDistMapping[row, _NumberOfComponents + _InsertQty].Value)
                    + Convert.ToInt32(_gridDistMapping[row, _NumberOfComponents + _TotalMailQty].Value)
                    + Convert.ToInt32(_gridDistMapping[row, _NumberOfComponents + _OtherQty].Value);
                _total_TotalQty += Convert.ToInt32(_gridDistMapping[row, _NumberOfComponents + _TotalQty].Value);
            }

            DisplayTotals();
        }

        private void UpdateStats()
        {
            DataView dvMTP = new DataView(_dsDistMapping.MappingTotals);
            DataView dvMTC = new DataView(_dsDistMapping.MappingTotals);
            DataView dvComp = new DataView(_dsEstimate.est_component);
            DistributionMapping.MappingTotalsRow pkg = null;
            DistributionMapping.MappingTotalsRow comp = null;
            Estimates.est_componentRow cp = null;
            int rowOffset = 2;
            int colOffset = 2;

            if (_dsDistMapping.MappingTotals.Count > 0)
            {
                #region Clear All Values
                for (int idx = 0; idx < _dsDistMapping.MappingTotals.Count; idx++)
                {
                    _dsDistMapping.MappingTotals[idx].Weight = 0M;
                    _dsDistMapping.MappingTotals[idx].QtyInserts = 0;
                    _dsDistMapping.MappingTotals[idx].QtyPoly = 0;
                    _dsDistMapping.MappingTotals[idx].QtySolo = 0;
                    _dsDistMapping.MappingTotals[idx].QtyOther = 0;
                }
                #endregion

                for (int r = 0; r < _NumberOfPackages; r++)
                {
                    dvMTP.RowFilter = string.Concat("item_id = ", _pkgIDs[r].ToString(), " and item_type = 'package'");
                    pkg = (DistributionMapping.MappingTotalsRow)dvMTP[0].Row;

                    pkg.Weight = 0M;
                    pkg.QtyInserts = (int)_gridDistMapping[r + rowOffset, _NumberOfComponents + _InsertQty].Value;
                    pkg.QtyPoly = (int)_gridDistMapping[r + rowOffset, _NumberOfComponents + _PolyQty].Value;
                    pkg.QtySolo = (int)_gridDistMapping[r + rowOffset, _NumberOfComponents + _SoloQty].Value;
                    pkg.QtyOther = (int)_gridDistMapping[r + rowOffset, _NumberOfComponents + _OtherQty].Value;

                    for (int c = 0; c < _NumberOfComponents; c++)
                    {
                        dvMTC.RowFilter = string.Concat("item_id = ", _compIDs[c].ToString(), " and item_type = 'component'");
                        comp = (DistributionMapping.MappingTotalsRow)dvMTC[0].Row;
                        dvComp.RowFilter = string.Concat("est_component_id = ", _compIDs[c].ToString());
                        cp = (Estimates.est_componentRow)dvComp[0].Row;
                        comp.Weight = cp.CalculateWeight();

                        if ((bool)_gridDistMapping[r + rowOffset, c + colOffset].Value)
                        {
                            comp.QtyInserts += pkg.QtyInserts;
                            comp.QtyPoly += pkg.QtyPoly;
                            comp.QtySolo += pkg.QtySolo;
                            comp.QtyOther += pkg.QtyOther;

                            pkg.Weight += comp.Weight;
                        }

                        int mqwi = 0;
                        if (!cp.IsmediaqtywoinsertNull())
                            mqwi = cp.mediaqtywoinsert;

                        if (comp.QtyMediaNoInsert == mqwi)
                            _gridDistMapping[0, c + colOffset].View = _viewRotate;
                        else
                            _gridDistMapping[0, c + colOffset].View = _viewRotateDisagree;

                        _gridDistMapping[0, c + colOffset].ToolTipText = ComposeComponentStats(comp, c, colOffset, cp);
                    }

                    _gridDistMapping[r + rowOffset, 1].ToolTipText = ComposePackageStats(pkg, r, rowOffset);
                    _gridDistMapping[r + rowOffset, _NumberOfComponents + _PackageWeight].Value = pkg.Weight.ToString("#,##0.0000");
                }
            }
        }

        private string ComposeComponentStats(DistributionMapping.MappingTotalsRow mtr, int index, int offset, Estimates.est_componentRow compRow)
        {
            string stats = string.Empty;
            int mqwi = 0;
            if (!compRow.IsmediaqtywoinsertNull())
                mqwi = compRow.mediaqtywoinsert;

            stats = string.Concat(_gridDistMapping[0, index + offset].Value.ToString()
                , "\n\tComponent Media Qty w/o Inserts = ", mqwi.ToString("#,##0")
                , "\n\tMapped Media Qty w/o Inserts = ", mtr.QtyMediaNoInsert.ToString("#,##0")
                , "\n\tComponent Weight = ", mtr.Weight.ToString("#,##0.0000")
                //, "\n\n\tInsert Qty = ", mtr.QtyInserts.ToString("#,##0")
                //, "\n\tInsert Weight = ", mtr.WeightInserts.ToString("#,##0.0000")
                //, "\n\n\tPoly Qty = ", mtr.QtyPoly.ToString("#,##0")
                //, "\n\tPoly Weight = ", mtr.WeightPoly.ToString("#,##0.0000")
                //, "\n\n\tSolo Qty = ", mtr.QtySolo.ToString("#,##0")
                //, "\n\tSolo Weight = ", mtr.WeightSolo.ToString("#,##0.0000")
                //, "\n\n\tTotal Qty = ", mtr.QtyTotal.ToString("#,##0")
                //, "\n\tTotal Weight = ", mtr.WeightTotal.ToString("#,##0.0000")
                );

            return stats;
        }

        private string ComposePackageStats(DistributionMapping.MappingTotalsRow mtr, int index, int offset)
        {
            string stats = string.Empty;

            stats = string.Concat(_gridDistMapping[index + offset, 1].Value.ToString()
                , "\n\tPackage Weight = ", mtr.Weight.ToString("0.0000")
                //, "\n\n\tInsert Qty = ", mtr.QtyInserts.ToString("#,##0")
                //, "\n\tInsert Weight = ", mtr.WeightInserts.ToString("#,##0.0000")
                //, "\n\n\tPoly Qty = ", mtr.QtyPoly.ToString("#,##0")
                //, "\n\tPoly Weight = ", mtr.WeightPoly.ToString("#,##0.0000")
                //, "\n\n\tSolo Qty = ", mtr.QtySolo.ToString("#,##0")
                //, "\n\tSolo Weight = ", mtr.WeightSolo.ToString("#,##0.0000")
                , "\n\n\tTotal Qty = ", mtr.QtyTotal.ToString("#,##0")
                , "\n\tTotal Weight = ", mtr.WeightTotal.ToString("#,##0.0000")
                );

            return stats;
        }

        private void AddGridRow()
        {
            int row = _gridDistMapping.RowsCount;
            ValueChangedController valueChangedController = new ValueChangedController();

            #region Editors
            CatalogEstimating.CustomGrids.Component.Editors.TextBoxIntegerRange editorQuantity = new CatalogEstimating.CustomGrids.Component.Editors.TextBoxIntegerRange(0, 999999999);
            editorQuantity.AllowNull = true;
            SourceGrid.Cells.Editors.TextBox editorText = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            editorText.AllowNull = false;
            #endregion

            _gridDistMapping.Rows.Insert(row);

            #region Add Row Header
            _gridDistMapping[row - 1, 0].Value = string.Empty;
            SourceGrid.Cells.Cell cellAddRecord = new SourceGrid.Cells.Cell("*");
            cellAddRecord.View = _viewRowHeader;
            _gridDistMapping[row, 0] = cellAddRecord;
            #endregion

            #region Add Package Name Column
            _gridDistMapping[row, 1] = new SourceGrid.Cells.Cell("[Enter Dist Name]");
            valueChangedController = new ValueChangedController("[Enter Dist Name]");
            valueChangedController.CellValueChanged += new CellValueChanged(Grid_CellValueChanged);
            _gridDistMapping[row, 1].AddController(valueChangedController);
            #endregion

            #region Add CheckBoxes
            for (int c = 2; c <= _NumberOfComponents + 1; ++c)
            {
                _gridDistMapping[row, c] = new SourceGrid.Cells.CheckBox();
                _gridDistMapping[row, c].Value = false;
                valueChangedController = new ValueChangedController(false);
                valueChangedController.CellValueChanged += new CellValueChanged(Grid_CellValueChanged);
                _gridDistMapping[row, c].AddController(valueChangedController);
            }
            #endregion

            #region Add Totals
            #region Quantities
            _gridDistMapping[row, _NumberOfComponents + _InsertQty] = new SourceGrid.Cells.Cell(0);
            _gridDistMapping[row, _NumberOfComponents + _SoloQty] = new SourceGrid.Cells.Cell(0);
            _gridDistMapping[row, _NumberOfComponents + _PolyQty] = new SourceGrid.Cells.Cell(0);
            _gridDistMapping[row, _NumberOfComponents + _TotalMailQty] = new SourceGrid.Cells.Cell(0);
            _gridDistMapping[row, _NumberOfComponents + _OtherQty] = new SourceGrid.Cells.Cell(0);
            _gridDistMapping[row, _NumberOfComponents + _TotalQty] = new SourceGrid.Cells.Cell(0);
            _gridDistMapping[row, _NumberOfComponents + _PackageWeight] = new SourceGrid.Cells.Cell(0);
            #endregion

            #region Set Views and Editors
            _gridDistMapping[row, 1].Editor = editorText;
            _gridDistMapping[row, _NumberOfComponents + _InsertQty].Editor = editorQuantity;
            _gridDistMapping[row, _NumberOfComponents + _InsertQty].View = _viewDisableCell;
            _gridDistMapping[row, _NumberOfComponents + _InsertQty].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            _gridDistMapping[row, _NumberOfComponents + _SoloQty].Editor = editorQuantity;
            _gridDistMapping[row, _NumberOfComponents + _SoloQty].View = _viewEnableCell;
            valueChangedController = new ValueChangedController(0);
            valueChangedController.CellValueChanged += new CellValueChanged(Grid_CellValueChanged);
            _gridDistMapping[row, _NumberOfComponents + _SoloQty].AddController(valueChangedController);
            _gridDistMapping[row, _NumberOfComponents + _PolyQty].Editor = editorQuantity;
            _gridDistMapping[row, _NumberOfComponents + _PolyQty].View = _viewDisableCell;
            _gridDistMapping[row, _NumberOfComponents + _PolyQty].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            _gridDistMapping[row, _NumberOfComponents + _TotalMailQty].Editor = editorQuantity;
            _gridDistMapping[row, _NumberOfComponents + _TotalMailQty].View = _viewDisableCell;
            _gridDistMapping[row, _NumberOfComponents + _TotalMailQty].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            _gridDistMapping[row, _NumberOfComponents + _OtherQty].Editor = editorQuantity;
            _gridDistMapping[row, _NumberOfComponents + _OtherQty].View = _viewEnableCell;
            valueChangedController = new ValueChangedController(0);
            valueChangedController.CellValueChanged += new CellValueChanged(Grid_CellValueChanged);
            _gridDistMapping[row, _NumberOfComponents + _OtherQty].AddController(valueChangedController);
            _gridDistMapping[row, _NumberOfComponents + _TotalQty].Editor = editorQuantity;
            _gridDistMapping[row, _NumberOfComponents + _TotalQty].View = _viewDisableCell;
            _gridDistMapping[row, _NumberOfComponents + _TotalQty].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            _gridDistMapping[row, _NumberOfComponents + _PackageWeight].Editor = editorQuantity;
            _gridDistMapping[row, _NumberOfComponents + _PackageWeight].View = _viewDisableCell;
            _gridDistMapping[row, _NumberOfComponents + _PackageWeight].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            #endregion
            #endregion
        }

        private void AddMailerPackage(int row)
        {
            Estimates.est_packageRow newPackage = _dsEstimate.est_package.Newest_packageRow();
            newPackage.BeginEdit();
            newPackage.est_estimate_id = _dsEstimate.est_estimate[0].est_estimate_id;
            newPackage.description = _gridDistMapping[row, 1].Value.ToString();
            newPackage.comments = _gridDistMapping[row, 1].Value.ToString();
            newPackage.soloquantity = Convert.ToInt32(_gridDistMapping[row, _NumberOfComponents + _SoloQty].Value);
            newPackage.otherquantity = Convert.ToInt32(_gridDistMapping[row, _NumberOfComponents + _OtherQty].Value);
            newPackage.Setpub_pubquantitytype_idNull();
            newPackage.Setpub_pubgroup_idNull();
            newPackage.Setpub_insertscenario_idNull();
            newPackage.createdby = MainForm.AuthorizedUser.FormattedName;
            newPackage.createddate = DateTime.Now;
            newPackage.EndEdit();
            _dsEstimate.est_package.Addest_packageRow(newPackage);

            _pkgIDs.Add(newPackage.est_package_id);
            _NumberOfPackages = _pkgIDs.Count;

            DistributionMapping.MappingTotalsRow mtr = _dsDistMapping.MappingTotals.NewMappingTotalsRow();
            mtr.BeginEdit();
            mtr.item_id = newPackage.est_package_id;
            mtr.item_type = "package";
            mtr.Weight = 0M;
            mtr.QtyInserts = 0;
            mtr.QtyPoly = 0;
            mtr.QtySolo = 0;
            mtr.QtyOther = 0;
            mtr.EndEdit();
            _dsDistMapping.MappingTotals.AddMappingTotalsRow(mtr);

            Estimates.EstPackage_Quantities_ByEstimateIDRow newQty = _dsEstimate.EstPackage_Quantities_ByEstimateID.NewEstPackage_Quantities_ByEstimateIDRow();
            newQty.BeginEdit();
            newQty.est_package_id = newPackage.est_package_id;
            newQty.insertqty = 0;
            newQty.polybagqty = 0;
            newQty.EndEdit();
            _dsEstimate.EstPackage_Quantities_ByEstimateID.AddEstPackage_Quantities_ByEstimateIDRow(newQty);
        }

        private bool Valid()
        {
            bool valid = true;
            DataView dv = new DataView(_dsEstimate.est_package);

            _errorProvider.SetError(_gridDistMapping, string.Empty);

            for (int row = 2; row < _gridDistMapping.RowsCount - 1; row++)
            {
                dv.RowFilter = string.Concat("description = '", _gridDistMapping[row, 1].Value.ToString().Replace("'", "''"), "'");

                if (dv.Count > 1)
                {
                    if (_gridDistMapping[row, 1].View == _viewEnableTextCell)
                        _gridDistMapping[row, 1].View = _viewInvalidEnabledTextCell;
                    else if (_gridDistMapping[row, 1].View == _viewDisableTextCell)
                        _gridDistMapping[row, 1].View = _viewInvalidDisabledTextCell;
                    else if (_gridDistMapping[row, 1].View == _viewDisableScenarioCell)
                        _gridDistMapping[row, 1].View = _viewInvalidDisableScenarioCell;

                    _errorProvider.SetError(_gridDistMapping, CatalogEstimating.Properties.Resources.UniqueDistBreakdownDesc);
                    valid = false;
                }
                else
                {
                    if (_gridDistMapping[row, 1].View == _viewInvalidEnabledTextCell)
                        _gridDistMapping[row, 1].View = _viewEnableTextCell;
                    else if (_gridDistMapping[row, 1].View == _viewInvalidDisabledTextCell)
                        _gridDistMapping[row, 1].View = _viewDisableTextCell;
                    else if (_gridDistMapping[row, 1].View == _viewInvalidDisableScenarioCell)
                        _gridDistMapping[row, 1].View = _viewDisableScenarioCell;
                }

                if ((string.IsNullOrEmpty(_gridDistMapping[row, 1].Value.ToString()))
                    && (_gridDistMapping[row, 1].View == _viewEnableCell))
                {
                    _gridDistMapping[row, 1].View = _viewInvalidEnabledCell;
                    _errorProvider.SetError(_gridDistMapping, CatalogEstimating.Properties.Resources.UniqueDistBreakdownDesc);
                    valid = false;
                }
            }

            _gridDistMapping.Invalidate();

            return valid;
        }

        private void WriteToDataTable()
        {
            bool isChecked = false;
            string mapFilter = string.Empty;
            string pkgFilter = string.Empty;
            DataView compMap = new DataView(_dsEstimate.est_packagecomponentmapping);
            DataView pkgs = new DataView(_dsEstimate.est_package);
            Estimates.est_packageRow package = null;

            for (int pack = 0; pack < _pkgIDs.Count; pack++)
            {
                mapFilter = string.Concat("est_package_id = ", _pkgIDs[pack].ToString(), " and est_component_id = ");
                pkgFilter = string.Concat("est_package_id = ", _pkgIDs[pack].ToString());

                for (int comp = 0; comp < _compIDs.Count; comp++)
                {
                    compMap.RowFilter = string.Concat(mapFilter, _compIDs[comp].ToString());
                    isChecked = (bool)_gridDistMapping[pack + 2, comp + 2].Value;

                    if ((compMap.Count > 0) && !isChecked)
                    {
                        compMap[0].Delete();
                    }
                    else if ((compMap.Count == 0) && isChecked)
                    {
                        Estimates.est_packagecomponentmappingRow m = _dsEstimate.est_packagecomponentmapping.Newest_packagecomponentmappingRow();
                        m.est_package_id = _pkgIDs[pack];
                        m.est_component_id = _compIDs[comp];
                        m.createdby = MainForm.AuthorizedUser.FormattedName;
                        m.createddate = DateTime.Now;
                        _dsEstimate.est_packagecomponentmapping.Addest_packagecomponentmappingRow(m);
                    }
                }

                pkgs.RowFilter = pkgFilter;

                // Only Update If Necessary
                if ( pkgs.Count > 0 )
                {
                    package = (Estimates.est_packageRow)pkgs[0].Row;
                    if ( package.description != _gridDistMapping[pack + 2, 1].Value.ToString() )
                    {
                        package.description = _gridDistMapping[pack + 2, 1].Value.ToString();
                        package.modifiedby = MainForm.AuthorizedUser.FormattedName;
                        package.modifieddate = DateTime.Now;
                    }
                    if ( package.soloquantity != Convert.ToInt32( _gridDistMapping[pack + 2, _compIDs.Count + _SoloQty].Value ) )
                    {
                        package.soloquantity = Convert.ToInt32( _gridDistMapping[pack + 2, _compIDs.Count + _SoloQty].Value );
                        package.modifiedby = MainForm.AuthorizedUser.FormattedName;
                        package.modifieddate = DateTime.Now;
                    }
                    if ( package.otherquantity != Convert.ToInt32( _gridDistMapping[pack + 2, _compIDs.Count + _OtherQty].Value ) )
                    {
                        package.otherquantity = Convert.ToInt32( _gridDistMapping[pack + 2, _compIDs.Count + _OtherQty].Value );
                        package.modifiedby = MainForm.AuthorizedUser.FormattedName;
                        package.modifieddate = DateTime.Now;
                    }
                }
            }
        }

        private bool IsInPolybag(long package_id)
        {
            bool inPolybag = false;
            long estimate_id = _dsEstimate.est_estimate[0].est_estimate_id;

            if ((estimate_id > -1) && (package_id > -1))
            {
                using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
                {
                    conn.Open();

                    using (EstPackage_Poly_QuantitiesTableAdapter adapter = new EstPackage_Poly_QuantitiesTableAdapter())
                    {
                        adapter.Connection = conn;
                        adapter.Fill(_dsEstimate.EstPackage_Poly_Quantities, estimate_id);
                    }

                    conn.Close();
                }

                foreach (Estimates.EstPackage_Poly_QuantitiesRow qr in _dsEstimate.EstPackage_Poly_Quantities.Select(string.Concat("est_package_id = ", package_id.ToString())))
                {
                    if (qr.inPolyBag)
                        inPolybag = true;
                }
            }

            return inPolybag;
        }

        #endregion
    }
}

