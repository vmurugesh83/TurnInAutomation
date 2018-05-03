#region Using Directives

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

#endregion

namespace CatalogEstimating
{
    public class ExcelWriter : IDisposable
    {
        #region Private Variables

        // Application instance
        Microsoft.Office.Interop.Excel.Application _excel;

        // Workbook and worksheet for this report
        Workbook _reportWorkbook;
        Worksheet _reportWorksheet;

        // Workbook and worksheet for the template if applicable
        Workbook _templateWorkbook = null;
        Worksheet _templateWorksheet = null;

        private int _Row = 1;   // Excel is 1 based, not 0 based!
        private int _Col = 1;

        #endregion

        #region Construction and Destruction

        public ExcelWriter()
        : this(null , null)
        { }

        public ExcelWriter( string reportName )
        : this( reportName, null )
        { }

        public ExcelWriter(string reportName , string templateFileName)
        {
            _templateFileName = templateFileName;
            _reportName = reportName;
        }

        ~ExcelWriter()
        {
            Dispose( false );
        }

        public void Dispose( bool disposing )
        {
            if ( _excel != null )
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject( _excel );
                GC.Collect();
            }

            if ( disposing )
                GC.SuppressFinalize( this );
        }

        #endregion

        #region Public Properties

        private string _templateFileName = null;
        public string TemplateFileName
        {
            get { return _templateFileName; }
        }

        private string _reportName = null;
        private string ReportName
        {
            get { return _reportName; }
        }

        #endregion

        #region Public Methods

        public void AutoFitCells()
        {
            _reportWorksheet.Columns.AutoFit();
        }

        public void WriteTemplateHeaderColumn(string colTemplate, int rowSpan, int colSpan)
        {
            Initialize();

            // First find the column that corrsponds to this template
            int templateCol = -1;

            // Excel is 1 based, not 0 based!
            if (_templateWorksheet != null)
            {
                for (int col = 1; col <= _templateWorksheet.Columns.Count; ++col)
                {
                    Range tempCell = (Range)_templateWorksheet.Cells[1, col];
                    if ((string)tempCell.Text == colTemplate)
                    {
                        templateCol = col;
                        break;
                    }
                }
            }

            // If I find a template column, then apply that formatting, otherwise skip
            if (templateCol >= 0)
            {
                // The template is 2 rows off because the first row is the column template name
                // And also because Excel is 1 based, and not 0 based
                Range rangeTemplate = _templateWorksheet.Cells.get_Range(_templateWorksheet.Cells[2, templateCol], _templateWorksheet.Cells[rowSpan + 1, templateCol + colSpan - 1]);

                Range rangeReport = _reportWorksheet.Cells.get_Range(_reportWorksheet.Cells[_Row, _Col], _reportWorksheet.Cells[_Row + rowSpan - 1, _Col + colSpan - 1]);
                rangeTemplate.Copy(Type.Missing);
                rangeReport.PasteSpecial(XlPasteType.xlPasteAll, XlPasteSpecialOperation.xlPasteSpecialOperationNone, Type.Missing, Type.Missing);
                _Col += colSpan;
            }
        }

        public void WriteTemplateColumn(string colTemplate, params object[] data)
        {
            Initialize();

            // First find the column that corresponds to this template
            int templateCol = -1;

            // Excel is 1 based, not 0 based!
            if (_templateWorksheet != null)
            {
                for (int col = 1; col <= _templateWorksheet.Columns.Count; ++col)
                {
                    Range tempCell = (Range)_templateWorksheet.Cells[1, col];
                    if ((string)tempCell.Text == colTemplate)
                    {
                        templateCol = col;
                        break;
                    }
                }
            }

            // If I find a template column, then apply that formatting, otherwise skip and just write out the data
            if (templateCol >= 0)
            {
                // The template is 2 rows off because the first row is the column template name
                // And also because Excel is 1 based, and not 0 based
                Range rangeTemplate = _templateWorksheet.Cells.get_Range(_templateWorksheet.Cells[2, templateCol], _templateWorksheet.Cells[data.Length + 1, templateCol]);

                // The range in the report is only 1 row off because Excel is 1 based, not 0 based
                Range rangeReport = _reportWorksheet.Cells.get_Range(_reportWorksheet.Cells[_Row, _Col], _reportWorksheet.Cells[_Row + data.Length - 1, _Col]);

                // Now apply the styles from the the template
                rangeTemplate.Copy(Type.Missing);
                rangeReport.PasteSpecial(XlPasteType.xlPasteAll, XlPasteSpecialOperation.xlPasteSpecialOperationNone, Type.Missing, Type.Missing);
            }

            WriteColumn(data);
        }

        /// <summary>
        /// Writes a group of rows from the template spreadsheet to the report spreadsheet.
        /// </summary>
        public void WriteTemplateRowRegion(string rowTemplate, int rowSpan, int colSpan, int destRow, int destCol)
        {
            Initialize();

            // First find the row that corresponds to this row template
            int templateRow = -1;

            // Excel is 1 based, not 0 based!
            if (_templateWorksheet != null)
            {
                for (int row = 1; row <= _templateWorksheet.Rows.Count; ++row)
                {
                    Range tempCell = (Range)_templateWorksheet.Cells[row, 1];
                    if ((string)tempCell.Text == rowTemplate)
                    {
                        templateRow = row;
                        break;
                    }
                }
            }

            // If I find a template row, then apply that formatting.  Otherwise do nothing
            if (templateRow > 0)
            {
                // The template is 2 columns off because the first column is the row template name
                // And also because Excel is 1 based, and not 0 based
                Range rangeTemplate = _templateWorksheet.Cells.get_Range(_templateWorksheet.Cells[templateRow, 2], _templateWorksheet.Cells[templateRow + rowSpan - 1, colSpan + 1]);

                // The cell in the report is only 1 column off because Excel is 1 based, not 0 based
                Range rangeReport = _reportWorksheet.Cells.get_Range(_reportWorksheet.Cells[destRow, destCol], _reportWorksheet.Cells[destRow + rowSpan - 1, destCol + colSpan - 1]);

                // Now apply the styles from the template
                rangeTemplate.Copy(Type.Missing);
                rangeReport.PasteSpecial(XlPasteType.xlPasteAll, XlPasteSpecialOperation.xlPasteSpecialOperationNone, Type.Missing, Type.Missing);
            }
        }

        public void SetTemplateFreezePanes(string rowTemplate)
        {
            Initialize();
            #region
            // First find the FreezePanes row that corresponds to this row template
            int templateRow = -1;

            // Excel is 1 based, not 0 based!
            if (_templateWorksheet != null)
            {
                for (int row = 1; row <= _templateWorksheet.Rows.Count; row++)
                {
                    Range tempCell = (Range)_templateWorksheet.Cells[row, 1];
                    if ((string)tempCell.Text == rowTemplate)
                    {
                        templateRow = row;
                        break;
                    }
                }
            }
            #endregion
            // If I find a template row, then apply that formatting, otherwise skip and just write out the data
            if (templateRow >= 0)
            {
                int numRows = 0;
                int numColumns = 0;
                Range r = null;
                object o = null;

                // The template is 2 columns off because the first column is the row template name
                // And also because Excel is 1 based, and not 0 based

                try
                {
                    r = (Range)_templateWorksheet.Cells[templateRow, 2];
                    o = r.get_Value(Missing.Value);
                    if (o != null)
                        numRows = Convert.ToInt32(o);
                }
                catch
                {
                    numRows = 0;
                }

                try
                {
                    r = (Range)_templateWorksheet.Cells[templateRow, 3];
                    o = r.get_Value(Missing.Value);
                    if (o != null)
                        numColumns = Convert.ToInt32(o);
                }
                catch
                {
                    numColumns = 0;
                }

                if ((numRows > 0) || (numColumns > 0))
                {
                    _reportWorksheet.Activate();
                    Range myCell = (Range)_reportWorksheet.Cells[numRows + 1, numColumns + 1];
                    //myCell.Activate();
                    myCell.Application.ActiveWindow.FreezePanes = true;
                }
            }

        }

        /// <summary>Writes a row of data to the spreadsheet using the formatting from the Template
        /// sheet, using the named template row provided.</summary>
        /// <param name="rowTemplate">Name of the template row in the template spreadsheet to 
        /// use for formatting.</param>
        /// <param name="data"></param>
        public void WriteTemplateLine( string rowTemplate, params object[] data )
        {
            Initialize();
            #region
            // First find the row that corresponds to this row template
            int templateRow = -1;

            // Excel is 1 based, not 0 based!
            if ( _templateWorksheet != null )
            {
                for ( int row = 1; row <= _templateWorksheet.Rows.Count; row++ )
                {
                    Range tempCell = (Range)_templateWorksheet.Cells[row, 1];
                    if ( (string)tempCell.Text == rowTemplate )
                    {
                        templateRow = row;
                        break;
                    }
                }
            }
            #endregion
            // If I find a template row, then apply that formatting, otherwise skip and just write out the data
            if ( templateRow >= 0 )
            {
                // The template is 2 columns off because the first column is the row template name
                // And also because Excel is 1 based, and not 0 based
                Range rangeTemplate = _templateWorksheet.Cells.get_Range(_templateWorksheet.Cells[templateRow, 2], _templateWorksheet.Cells[templateRow, data.Length + 1]);

                // The cell in the report is only 1 column off because Excel is 1 based, not 0 based
                Range rangeReport = _reportWorksheet.Cells.get_Range(_reportWorksheet.Cells[_Row, 1], _reportWorksheet.Cells[_Row, data.Length]);

                // Now apply the styles from the the template
                rangeTemplate.Copy(Type.Missing);
                rangeReport.PasteSpecial(XlPasteType.xlPasteAll, XlPasteSpecialOperation.xlPasteSpecialOperationNone, Type.Missing, Type.Missing);
            }

            // Now write the data to the working report
            WriteLine( data );
        }

        /// <summary>Writes an array of data to the next row in the spreadsheet without any formatting.</summary>
        /// <param name="data"></param>
        public void WriteLine( params object[] data )
        {
            WriteLine(_Row, data);
            _Row++;
        }

        /// <summary>
        /// Writes an array of data to the spreadsheet at the specified row number without any formatting.
        /// </summary>
        /// <param name="rowNumber"></param>
        /// <param name="data"></param>
        public void WriteLine( int rowNumber, params object[] data)
        {
            Initialize();

            // Write out the data to the row one cell at a time
            for (int col = 0; col < data.Length; col++)
            {
                // If the value is null, don't write anything
                if (data[col] != null)
                {
                    // Excel is 1 based, not 0 based!
                    Range cell = (Range)_reportWorksheet.Cells[rowNumber, col + 1];
                    cell.set_Value(Type.Missing, data[col].ToString());
                }
            }
        }

        /// <summary>
        /// Writes an array of data to a column in the spreadsheet without any formatting.
        /// </summary>
        /// <param name="data"></param>
        public void WriteColumn(params object[] data)
        {
            Initialize();

            // Write out the data to the column one cell at a time
            for (int row = 0; row < data.Length; ++row)
            {
                // Excel is 1 based, not 0 base!
                Range cell = (Range)_reportWorksheet.Cells[_Row + row, _Col];
                cell.set_Value(Type.Missing, data[row]);
            }
            ++_Col;
        }

        public void WriteTable( DataGridView view, bool writeColumnHeaders )
        {
            List<string> line = new List<string>();

            if ( writeColumnHeaders )
            {
                foreach ( DataGridViewColumn col in view.Columns )
                {
                    if ( col.Visible )
                        line.Add( col.HeaderText );
                }
                WriteLine( line.ToArray() );
            }

            foreach ( DataGridViewRow row in view.Rows )
            {
                line.Clear();
                foreach ( DataGridViewCell cell in row.Cells )
                {
                    if ( cell.Visible )
                    {
                        if ( cell.Value == null )
                            line.Add( string.Empty );
                        else
                            line.Add( cell.EditedFormattedValue.ToString() );
                    }
                }
                WriteLine( line.ToArray() );
            }
        }

        public void Save( string fileName )
        {
            // Save the report workbook.  Don't save the template workbook
            _reportWorkbook.SaveCopyAs( fileName );
        }

        public void Show()
        {
            if (_templateWorkbook != null )
                _templateWorkbook.Close( false, Missing.Value, Missing.Value );

            if ( _reportWorkbook != null )
            {
                _reportWorkbook.Activate();
                _excel.Visible = true;
            }
        }

        private void NAR(object o)
        {
            if (o == null)
                return;

            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
            }
            catch
            {
            }
            finally
            {
                o = null;
            }
        }

        /// <summary>
        /// Attempts to close the excel application.
        /// Used when an unexpected error occurred while creating an excel document
        /// </summary>
        public void Quit()
        {
            NAR(_templateWorksheet);

            if (_templateWorkbook != null)
            {
                try
                {
                    _templateWorkbook.Close(false, Missing.Value, Missing.Value);
                }
                catch
                {
                }
            }
            NAR(_templateWorkbook);

            NAR(_reportWorksheet);

            if (_reportWorkbook != null)
            {
                try
                {
                    _reportWorkbook.Close(false, Missing.Value, Missing.Value);
                }
                catch
                {
                }
            }
            NAR(_reportWorkbook);

            if (_excel != null)
            {
                try
                {
                    _excel.Quit();
                }
                catch (Exception)
                {
                }
            }
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            // Check for an Excel Application object.  If it doesn't exist, then create it the first time
            if ( _excel != null )
                return;

            // Create the Excel object and the workbook that the report will be written to
            _excel = new Microsoft.Office.Interop.Excel.Application();
            string blankFullFullPath = Path.Combine( Environment.CurrentDirectory, "BlankTemplate.xls" );
            _reportWorkbook = _excel.Workbooks.Add( blankFullFullPath );
            _reportWorksheet = (Worksheet)_reportWorkbook.Worksheets[1];

            if ( !string.IsNullOrEmpty( _reportName ) )
                _reportWorksheet.Name = _reportName;

            // If there is a template for this excel report, than open up the template as a seperate
            // workbook, and find the worksheet that corresponds to this report
            if ( !string.IsNullOrEmpty( _templateFileName ) && !string.IsNullOrEmpty( _reportName ) )
            {
                string templateFullPath = Path.Combine( Environment.CurrentDirectory, _templateFileName );
                _templateWorkbook = _excel.Workbooks.Open( templateFullPath, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value );

                // Can't index a worksheet by name, so iterate through and compare names til I find it
                foreach ( Worksheet tempWorksheet in _templateWorkbook.Worksheets )
                {
                    if ( tempWorksheet.Name == _reportName )
                    {
                        // Found it.  Save it for later and bail early
                        _templateWorksheet = tempWorksheet;
                        CopyWorksheetAttributes();
                        break;
                    }
                }
            }

            // Be default, Excel adds three blank Worksheets.  We only want one so delete the other two
            List<Worksheet> listDelete = new List<Worksheet>();
            for ( int iDelete = 2; iDelete <= _reportWorkbook.Worksheets.Count; iDelete++ )
                listDelete.Add( (Worksheet)_reportWorkbook.Worksheets[iDelete] );
            foreach ( Worksheet deleteWorksheet in listDelete )
                deleteWorksheet.Delete();
        }

        private void CopyWorksheetAttributes()
        {
            if ((_reportWorksheet != null) && (_templateWorksheet != null))
            {
                _reportWorksheet.PageSetup.BottomMargin = _templateWorksheet.PageSetup.BottomMargin;
                _reportWorksheet.PageSetup.TopMargin = _templateWorksheet.PageSetup.TopMargin;
                _reportWorksheet.PageSetup.LeftMargin = _templateWorksheet.PageSetup.LeftMargin;
                _reportWorksheet.PageSetup.RightMargin = _templateWorksheet.PageSetup.RightMargin;

                _reportWorksheet.PageSetup.Orientation = _templateWorksheet.PageSetup.Orientation;

                _reportWorksheet.PageSetup.LeftHeader = _templateWorksheet.PageSetup.LeftHeader;
                _reportWorksheet.PageSetup.LeftFooter = _templateWorksheet.PageSetup.LeftFooter;
                _reportWorksheet.PageSetup.RightHeader = _templateWorksheet.PageSetup.RightHeader;
                _reportWorksheet.PageSetup.RightFooter = _templateWorksheet.PageSetup.RightFooter;
                _reportWorksheet.PageSetup.CenterHeader = _templateWorksheet.PageSetup.CenterHeader;
                _reportWorksheet.PageSetup.CenterFooter = _templateWorksheet.PageSetup.CenterFooter;

                _reportWorksheet.PageSetup.Zoom = _templateWorksheet.PageSetup.Zoom; 
                _reportWorksheet.PageSetup.FitToPagesWide = _templateWorksheet.PageSetup.FitToPagesWide;
                _reportWorksheet.PageSetup.FitToPagesTall = _templateWorksheet.PageSetup.FitToPagesTall;

                _reportWorksheet.PageSetup.PaperSize = _templateWorksheet.PageSetup.PaperSize;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}