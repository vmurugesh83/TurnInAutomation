using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using CatalogEstimating.Datasets;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace CatalogEstimating
{
    public static class ReportWriter
    {
        public static int SaveReportToDatabase(int reportType, ExcelWriter writer)
        {
            // Save the Excel file out to a temporary location
            string tempPath = Path.Combine(Path.GetTempPath(),
                Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ".xls");
            writer.Save(tempPath);

            // Now read in the binary data to memory
            FileInfo reportFile = new FileInfo(tempPath);
            byte[] fileBlob;
            using (FileStream reportStream = reportFile.OpenRead())
            {
                fileBlob = new byte[reportStream.Length];
                reportStream.Read(fileBlob, 0, Convert.ToInt32(reportStream.Length));
                reportStream.Close();
            }

            // Delete the temporary file
            reportFile.Delete();

            int reportId = -1;

            // Write the report out to the database
            Database working = MainForm.WorkingDatabase.Database;
            using (DbCommand insertCommand = working.GetStoredProcCommand("RptReport_i"))
            {
                using (DbConnection conn = working.CreateConnection())
                {
                    conn.Open();
                    insertCommand.Connection = conn;
                    working.AddOutParameter(insertCommand, "@RPT_Report_ID", DbType.Int32, 4);
                    working.AddInParameter(insertCommand, "@RPT_ReportType_ID", DbType.Int32, reportType);
                    working.AddInParameter(insertCommand, "@Report", DbType.Binary, fileBlob);
                    working.AddInParameter(insertCommand, "@CreatedBy", DbType.String, MainForm.AuthorizedUser.FormattedName);
                    insertCommand.ExecuteNonQuery();
                    reportId = (int)insertCommand.Parameters["@RPT_Report_ID"].Value;
                    conn.Close();
                }
            }

            return reportId;
        }

        /// <summary>Saves the specified report to the database.</summary>
        public static void SaveReportToDatabase(int reportType, string reportTemplate, ExcelWriter writer, Datasets.RptReport dsReports)
        {
            int reportId = SaveReportToDatabase(reportType, writer);

            // Now add this report to the report history grid
            //RptReport.Rpt_ReportHistoryRow newRow = dsReports.Rpt_ReportHistory.NewRpt_ReportHistoryRow();
            //newRow.rpt_report_id = reportId;
            //newRow.createdby = MainForm.AuthorizedUser.FormattedName;
            //newRow.createddate = DateTime.Now;
            //newRow.description = reportTemplate;
            //dsReports.Rpt_ReportHistory.AddRpt_ReportHistoryRow(newRow);
        }

    }
}
