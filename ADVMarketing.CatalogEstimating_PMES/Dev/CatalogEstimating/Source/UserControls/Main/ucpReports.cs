#region Using Directives

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Data.SqlClient;
using System.Data.Common;
using System.Reflection;

using CatalogEstimating.UserControls.Reports;
using CatalogEstimating.Properties;
using CatalogEstimating.Datasets;
using CatalogEstimating.Datasets.RptReportTableAdapters;
using CatalogEstimating.Datasets.LookupTableAdapters;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Excel = Microsoft.Office.Interop.Excel;

#endregion

namespace CatalogEstimating.UserControls.Main
{
    public partial class ucpReports : CatalogEstimating.UserControlPanel
    {
        #region Private Variables

        private Lookup _dsLookup = new Lookup();
        private ReportControl _reportControl = null;
        private static readonly Dictionary<int, string> ControlMap = new Dictionary<int,string>();

        #endregion

        #region Construction

        static ucpReports()
        {
            ControlMap.Add( 1, "rptAdPublicationCosts" );
            ControlMap.Add( 2, "rptDirectMailCost" );
            ControlMap.Add( 3, "rptPostageCategoryUsage" );
            ControlMap.Add( 4, "rptEstimate" );
            ControlMap.Add( 5, "rptEstimateSummary" );
            ControlMap.Add( 7, "rptVendorCommitment" );
            ControlMap.Add( 8, "rptTyLyComparison" );
            ControlMap.Add( 9, "rptEstimateElementDetail" );
        }

        public ucpReports()
        {
            InitializeComponent();
            Name = "Reports";
        }

        #endregion

        #region Overrides

        public override void LoadData()
        {
            this.IsLoading = true;

            _dsLookup.Clear();

            using (SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection())
            {
                using (EstEstimate_s_FiscalYearTableAdapter adapter = new EstEstimate_s_FiscalYearTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsLookup.EstEstimate_s_FiscalYear);
                }

                using (est_seasonTableAdapter adapter = new est_seasonTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsLookup.est_season);
                }

                using ( est_statusTableAdapter adapter = new est_statusTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsLookup.est_status);
                }

                using (PublicationListTableAdapter adapter = new PublicationListTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsLookup.PublicationList);
                }

                using (CurrentVendorsTableAdapter adapter = new CurrentVendorsTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsLookup.CurrentVendors);
                }

                using (vnd_vendortypeTableAdapter adapter = new vnd_vendortypeTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsLookup.vnd_vendortype);
                }

                using (est_estimatemediatypeTableAdapter adapter = new est_estimatemediatypeTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsLookup.est_estimatemediatype);
                }

                using (est_componenttypeTableAdapter adapter = new est_componenttypeTableAdapter())
                {
                    adapter.Connection = conn;
                    adapter.Fill(_dsLookup.est_componenttype);
                }

                conn.Close();
            }

            // Add Blank Rows to some of the tables so that the users can select "none"
            Lookup.est_statusRow es_row = _dsLookup.est_status.Newest_statusRow();
            es_row.est_status_id = -1;
            es_row.description = String.Empty;
            es_row.createdby = String.Empty;
            es_row.createddate = DateTime.Now;
            _dsLookup.est_status.Addest_statusRow(es_row);
            _dsLookup.est_status.DefaultView.Sort = "est_status_id";

            Lookup.CurrentVendorsRow vnd_row = _dsLookup.CurrentVendors.NewCurrentVendorsRow();
            vnd_row.vnd_vendor_id = -1;
            vnd_row.vendorcode = String.Empty;
            vnd_row.description = String.Empty;
            vnd_row.active = true;
            vnd_row.createdby = String.Empty;
            vnd_row.createddate = DateTime.Now;
            _dsLookup.CurrentVendors.AddCurrentVendorsRow(vnd_row);
            _dsLookup.CurrentVendors.DefaultView.Sort = "description";

            Lookup.vnd_vendortypeRow vndtype_row = _dsLookup.vnd_vendortype.Newvnd_vendortypeRow();
            vndtype_row.vnd_vendortype_id = -1;
            vndtype_row.description = String.Empty;
            vndtype_row.createdby = String.Empty;
            vndtype_row.createddate = DateTime.Now;
            _dsLookup.vnd_vendortype.Addvnd_vendortypeRow(vndtype_row);
            _dsLookup.vnd_vendortype.DefaultView.Sort = "description";

            Lookup.est_seasonRow season_row = _dsLookup.est_season.Newest_seasonRow();
            season_row.est_season_id = -1;
            season_row.description = string.Empty;
            season_row.createdby = String.Empty;
            season_row.createddate = DateTime.Now;
            _dsLookup.est_season.Addest_seasonRow(season_row);
            _dsLookup.est_season.DefaultView.Sort = "description";

            base.LoadData();

            this.IsLoading = false;
        }

        public override void Reload()
        {
            base.Reload();

            LoadHistory();
        }
        public override ToolStrip Toolbar
        {
            get { return _toolStrip; }
        }

        #endregion

        #region Event Handlers

        /// <summary>Initializes the Report Control with report types and report history.</summary>
        private void ucpReports_Load( object sender, EventArgs e )
        {
            if ( !DesignMode )
            {
                LoadReportTypes();

                // Select the first report by default
                _listReportType.SelectedIndex = 0;
                _listReportType_SelectedValueChanged( this, EventArgs.Empty );
            }
        }

        /// <summary>Switches out the details control when a newly selected report type is chosen.</summary>
        private void _listReportType_SelectedValueChanged( object sender, EventArgs e )
        {
            // Properly dispose of the old report control
            if ( _reportControl != null )
            {
                _panelReportDetails.Controls.Remove( _reportControl );
                _reportControl.Dispose();
            }

            if ( _listReportType.SelectedIndex != -1 && Convert.ToInt32(_listReportType.SelectedValue) != 0)
            {
                // Create a new report control for the newly selected report
                int reportTypeId = Convert.ToInt32( _listReportType.SelectedValue );
                string strReportType = string.Format( "CatalogEstimating.UserControls.Reports.{0}", ControlMap[reportTypeId] );
                object[] constructorArgs = new object[] { _dsLookup };
                _reportControl = (ReportControl)GetType().Assembly.CreateInstance( strReportType, false, BindingFlags.CreateInstance, null, constructorArgs, null, null );
                _reportControl.Dock = DockStyle.Fill;
                _panelReportDetails.Controls.Add( _reportControl );
            }
        }

        /// <summary>Runs the selected report.</summary>
        private void _btnRunReport_Click( object sender, EventArgs e )
        {
            bool excelErrorOccured = false;
            ReportExecutionStatus retval = ReportExecutionStatus.InvalidSearchCriteria;
            ExcelWriter writerExcel = null;
            Cursor = Cursors.WaitCursor;
            try
            {
                writerExcel = new ExcelWriter(_reportControl.ReportTemplate, "ReportTemplate.xls");

                string logEventName = string.Format( "Report - {0}", _reportControl.ReportTemplate );
                PerformanceLog.Start(logEventName);
                retval = _reportControl.RunReport(writerExcel);
                PerformanceLog.End(logEventName);

                if (retval == ReportExecutionStatus.Success)
                {
                    writerExcel.AutoFitCells();
                    ReportWriter.SaveReportToDatabase( Convert.ToInt32( _listReportType.SelectedValue ), _reportControl.ReportTemplate, writerExcel, _dsReports );
                    writerExcel.Show();
                    LoadHistory();
                }

            }
            catch (System.Runtime.InteropServices.COMException)
            {
                writerExcel.Quit();
                excelErrorOccured = true;
            }
            finally
            {
                writerExcel.Dispose();
                Cursor = Cursors.Default;
            }

            if (excelErrorOccured)
            {
                MessageBox.Show(this, "An unknown error occured while creating the Excel document", "Reports", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (retval == ReportExecutionStatus.NoDataReturned)
            {
                MessageBox.Show(this, Resources.EmptyReportWarning, "Reports", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>Retrieves the specified report from the database and opens it in Excel.</summary>
        private void _gridReportHistory_CellDoubleClick( object sender, DataGridViewCellEventArgs e )
        {
            if ( e.RowIndex < 0 )
                return;

            // Get the binary date for the selected report out of the database
            DataRowView drv_row = (DataRowView)_gridReportHistory.Rows[e.RowIndex].DataBoundItem;
            RptReport.Rpt_ReportHistoryRow row = (RptReport.Rpt_ReportHistoryRow)drv_row.Row;
            RptReport tempReportSet = new RptReport();
            byte[] reportBlob;

            using ( RptReport_s_Report_ByReportIDTableAdapter adapter = new RptReport_s_Report_ByReportIDTableAdapter() )
            {
                using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( tempReportSet.RptReport_s_Report_ByReportID, row.rpt_report_id );
                    reportBlob = tempReportSet.RptReport_s_Report_ByReportID[0].Report;
                }
            }

            // Save the blob out to a temporary location
            string tempPath = Path.Combine( Path.GetTempPath(),
                Path.GetFileNameWithoutExtension( Path.GetTempFileName() ) + ".xls" );

            using ( FileStream stream = new FileStream( tempPath, FileMode.CreateNew ) )
            {
                stream.Write( reportBlob, 0, reportBlob.Length );
                stream.Close();
            }

            // Open up Excel and point it to the newly file
            Excel.Application excel = new Excel.Application();
            Excel.Workbook tempWorkbook = excel.Workbooks.Open( tempPath, Missing.Value, Missing.Value, Missing.Value, 
                Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, 
                Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value );
            Excel.Worksheet tempWorksheet = (Excel.Worksheet)tempWorkbook.Worksheets[1];

            // Move the sheet to a new workbook so it's treated like a "New" instead of something opened.
            Excel.Workbook newWorkbook = excel.Workbooks.Add( Missing.Value );
            tempWorksheet.Move( newWorkbook.Worksheets[1], Missing.Value );

            // Excel automatically adds a bunch of blank worksheets to a new workbook.  Remove those
            List<Excel.Worksheet> listDelete = new List<Excel.Worksheet>();
            for ( int iDelete = 2; iDelete <= newWorkbook.Worksheets.Count; iDelete++ )
                listDelete.Add( (Excel.Worksheet)newWorkbook.Worksheets[iDelete] );
            foreach ( Excel.Worksheet deleteWorksheet in listDelete )
                deleteWorksheet.Delete();

            // Delete the temp file
            File.Delete( tempPath );

            excel.Visible = true;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void _btnRefresh_Click( object sender, EventArgs e )
        {
            LoadHistory();
        }

        #endregion

        #region Private Methods

        /// <summary>Loads the List of Available Report Types into the list box on the screen.</summary>
        private void LoadReportTypes()
        {
            using ( rpt_reporttypeTableAdapter adapter = new rpt_reporttypeTableAdapter() )
            {
                using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsReports.rpt_reporttype );

                    _gridReportHistory.DataSource = _dsReports.Rpt_ReportHistory;
                }
            }
        }

        /// <summary>Loads the the grid with all previously run reports.</summary>
        private void LoadHistory()
        {
            using ( Rpt_ReportHistoryTableAdapter adapter = new Rpt_ReportHistoryTableAdapter() )
            {
                using ( SqlConnection conn = (SqlConnection)MainForm.WorkingDatabase.Database.CreateConnection() )
                {
                    adapter.Connection = conn;
                    adapter.Fill( _dsReports.Rpt_ReportHistory );

                    _gridReportHistory.DataSource = _dsReports.Rpt_ReportHistory;
                }
            }
        }

        #endregion
    }
}