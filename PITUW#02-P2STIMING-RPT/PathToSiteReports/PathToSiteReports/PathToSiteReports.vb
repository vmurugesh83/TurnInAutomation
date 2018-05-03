Imports PathToSiteReports.BusinessEntities
Imports PathToSiteReports.BLL
Imports PathToSiteReports.Common
Imports System.Configuration
Imports System.Globalization
Imports System.IO

Module PathToSiteReports

    Sub Main()
        Dim skuDetails As List(Of SKUDetail) = Nothing
        Dim ohReceiptDetails As List(Of SKUDetail) = Nothing
        Dim reportBAO As Report = Nothing
        Dim excelTemplatePath As String = String.Empty
        Dim thisWeekStartDate As Date = Nothing
        Dim thisWeekEndDate As Date = Nothing
        Dim skuDetailReportFileName As String = String.Empty
        Dim timingReportFileName As String = String.Empty
        Dim statisticsReportFileName As String = String.Empty
        Dim recipients As List(Of String) = Nothing
        Dim datePart As String = String.Concat(Right(String.Concat("0", Now.Month.ToString()), 2), Right(String.Concat("0", Now.Day.ToString()), 2), Now.Year)
        Dim studioPhotoShootDays As Integer = 0
        Dim studioImageProductionDays As Integer = 0
        Dim sampleDueDateDays As Integer = 0
        Dim attachmentFileNames As String = String.Empty
        Dim skuDetailReportPath As String = String.Empty
        Dim emailBody As String = String.Empty

        Try
            reportBAO = New Report()
            excelTemplatePath = ConfigurationManager.AppSettings("ReportPath")
            skuDetailReportPath = ConfigurationManager.AppSettings("SKUDetailReportPath")

            'Path to site timing reports
            skuDetails = reportBAO.GetAutoTurnInReportInfo()

            If Not skuDetails Is Nothing Then
                Console.WriteLine(String.Format("Total count is {0}", skuDetails.Count))

                Console.WriteLine(String.Format("Excel Template Path is {0}", excelTemplatePath))

                studioPhotoShootDays = ConfigurationManager.AppSettings("AtStudioDays")
                studioImageProductionDays = ConfigurationManager.AppSettings("InImageProductionDays")
                sampleDueDateDays = ConfigurationManager.AppSettings("sampleDueDateDays")

                If Not String.IsNullOrEmpty(skuDetailReportPath) Then
                    skuDetailReportFileName = Path.Combine(skuDetailReportPath, String.Format("PathtoSiteSKUDetailReport_{0}.xlsx", datePart))
                    ExcelHelper.ExportSKUDetailsReportToExcel(skuDetailReportFileName, "Detail Report", skuDetails, sampleDueDateDays)
                End If

                If Not String.IsNullOrEmpty(excelTemplatePath) Then

                    timingReportFileName = Path.Combine(excelTemplatePath, String.Format("PathtoSiteTimingReport_{0}.xlsx", datePart))
                    ExcelHelper.ExportPathToSiteTimingReportToExcel(timingReportFileName, "Timing Report",
                                                                    skuDetails,
                                                                    studioPhotoShootDays,
                                                                    studioImageProductionDays)
                End If

            End If

            'Path to site reports
            thisWeekStartDate = Date.Today().AddDays(-DirectCast(Date.Today().DayOfWeek, Integer))
            thisWeekEndDate = thisWeekStartDate.AddDays(7)

            ohReceiptDetails = reportBAO.GetSKUWithOHReceipt(thisWeekStartDate, thisWeekEndDate)

            If Not ohReceiptDetails Is Nothing AndAlso Not String.IsNullOrEmpty(excelTemplatePath) Then
                statisticsReportFileName = Path.Combine(excelTemplatePath, String.Format("PathtoSiteReport_{0}.xlsx", datePart))
                ExcelHelper.ExportPathToSiteMerchantReportToExcel(statisticsReportFileName, "Merchant Report",
                                                                  ohReceiptDetails, skuDetails,
                                                                    studioPhotoShootDays,
                                                                    studioImageProductionDays)
            End If

            'Send both the repots
            recipients = New List(Of String)()
            Common.Common.GetRecipients("ReportReceipients", recipients)

            emailBody = String.Concat("Please find attached the Path To Site Reports.  ",
                                       "These reports were created based on the SKU details report present in the following location: ",
                                       skuDetailReportFileName)

            attachmentFileNames = If(File.Exists(timingReportFileName), timingReportFileName, String.Empty)
            attachmentFileNames = If(File.Exists(statisticsReportFileName),
                                     If(Not String.IsNullOrEmpty(attachmentFileNames), String.Concat(attachmentFileNames, ";", statisticsReportFileName), statisticsReportFileName),
                                     attachmentFileNames)

            EmailHelper.Send(recipients, ConfigurationManager.AppSettings("FromEmailAddress"), "Path To Site Reports", emailBody,
            attachmentFileNames)

            'Delete the files
            'Delete PDF
            For Each f As String In Directory.GetFiles(excelTemplatePath)
                File.Delete(f)
            Next

        Catch ex As Exception
            Throw
        End Try

    End Sub

End Module
