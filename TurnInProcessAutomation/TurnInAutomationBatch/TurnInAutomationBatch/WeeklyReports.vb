Imports System.IO
Imports System.Configuration
Imports TurnInAutomationBatch.BLL
Imports TurnInAutomationBatch.BLL.Enumerations
Imports TurnInAutomationBatch.BusinessEntities
Imports TurnInAutomationBatch.Common
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL

Public Class WeeklyReports

    Public Sub GenerateReport()

        LogHelper.WriteToConsole("Begin On Order, Action Required Report.")
        LogHelper.WriteToLogFile(LogEntryType.Information, "Begin On Order, Action Required Report.")

        'Weekly Report #1 for EMS & CMR
        Dim datePart As String = IIf(Now.Month.ToString.Length = 1, "0" & Now.Month.ToString, Now.Month.ToString) & IIf(Now.Day.ToString.Length = 1, "0" & Now.Day.ToString, Now.Day.ToString) & Now.Year
        Dim pdfName As String = "OnOrderActionRequired_" & datePart & ".pdf"
        Dim outputDir As String = GetOutputDir()
        Dim htmlDir As String = GetExportDir()
        Dim pdfOutputFile As String = outputDir & pdfName
        Dim htmlTemplateFile As String = htmlDir & "ExportCSS.html"
        Dim recipients As New List(Of String)
        Dim startShipDate As Date = DateAdd(DateInterval.Day, 63, Now)
        Dim commonBLL As BLL.Common = Nothing
        Dim excelName = "OnOrderActionRequired_" & datePart & ".xlsx"
        Dim excelOutputFile As String = outputDir & excelName
        Dim poShipDays As Integer = ConfigurationManager.AppSettings("OOARPOShipDays")
        Dim buyerLevelData As List(Of NoSampleReportInfo) = Nothing
        Dim buyerIDs As List(Of String) = Nothing
        Dim buyerBAO As TUBuyer = Nothing
        Dim buyerDetails As List(Of BuyerInfo) = Nothing
        Dim buyerInfo As BuyerInfo = Nothing
        Dim buyerRACFID As String = String.Empty

        startShipDate = DateAdd(DateInterval.Day, poShipDays, Now)
        'On Order, Action Required Report
        Dim _TUNoSampleReport As New TUNoSampleReport
        Dim noSampleReportData As List(Of NoSampleReportInfo) = _TUNoSampleReport.GetNoSampleReport(startShipDate)

        commonBLL = New BLL.Common()
        If Not noSampleReportData Is Nothing AndAlso noSampleReportData.Count > 0 Then
            LogHelper.WriteToConsole("Export On Order, Action Required Report.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "Export On Order, Action Required Report.")

            'Export PDF
            iTextSharpHelper.GenerateNoSampleReport(pdfOutputFile, htmlTemplateFile, noSampleReportData)

            Try

                'Export Excel
                'CreateExcelFile.CreateExcelDocument(noSampleReportData, excelOutputFile)
                iTextSharpHelper.ExportNoSampleReportToExcel(excelOutputFile, "On Order, Action Required", noSampleReportData)
                'iTextSharpHelper.GenerateNoSampleReportExcel(excelOutputFile, "On Order, Action Required", noSampleReportData)
            Catch ex As Exception
                Throw
            End Try

            LogHelper.WriteToConsole(String.Concat("Email the whole On Order, Action Required Files. ", excelOutputFile))
            LogHelper.WriteToLogFile(LogEntryType.Information, String.Concat("Email the whole On Order, Action Required Files. ", excelOutputFile))

            'Email Files
            recipients = New List(Of String)()
            commonBLL.GetRecipients("EMS", recipients)
            commonBLL.GetRecipients("CMR", recipients)
            EmailHelper.Send(recipients, ConfigurationManager.AppSettings("FromEmailAddress"), "On Order, Action Required Report", "", pdfName & ";" & excelName)

            'Delete PDF
            For Each f As String In Directory.GetFiles(outputDir, pdfName.Replace(datePart, "*"))
                File.Delete(f)
            Next

            'Delete Excel
            For Each f As String In Directory.GetFiles(outputDir, excelName.Replace(datePart, "*"))
                File.Delete(f)
            Next


            buyerBAO = New TUBuyer()
            buyerDetails = buyerBAO.GetAllFromBuyer()

            buyerIDs = noSampleReportData.Where(Function(a) a.TurnInMerchStatus.Trim() = String.Empty _
                                                    OrElse a.TurnInMerchStatus.Trim().ToUpper() = "AVAILABLE" _
                                                    OrElse a.TurnInMerchStatus.Trim().ToUpper() = "CHECKED-OUT" _
                                                    OrElse a.TurnInMerchStatus.Trim().ToUpper() = "RETURNED" _
                                                    OrElse a.TurnInMerchStatus.Trim().ToUpper() = "DISPOSED" _
                                                    OrElse a.TurnInMerchStatus.Trim().ToUpper() = "MISSING").Select(Function(b) b.BuyerID).Distinct.ToList()

            If Not buyerIDs Is Nothing AndAlso buyerIDs.Count > 0 Then
                For Each buyerID As String In buyerIDs
                    buyerLevelData = noSampleReportData.FindAll(Function(a) a.BuyerID = buyerID AndAlso (a.TurnInMerchStatus.Trim() = String.Empty _
                                                                                                         OrElse a.TurnInMerchStatus.Trim().ToUpper() = "AVAILABLE" _
                                                                                                         OrElse a.TurnInMerchStatus.Trim().ToUpper() = "CHECKED-OUT" _
                                                                                                         OrElse a.TurnInMerchStatus.Trim().ToUpper() = "RETURNED" _
                                                                                                         OrElse a.TurnInMerchStatus.Trim().ToUpper() = "DISPOSED" _
                                                                                                         OrElse a.TurnInMerchStatus.Trim().ToUpper() = "MISSING"))
                    buyerInfo = buyerDetails.Find(Function(a) a.BuyerId = CInt(buyerID))
                    If Not buyerInfo Is Nothing Then
                        buyerRACFID = buyerInfo.BuyerRACFID
                    End If

                    If Not String.IsNullOrEmpty(buyerRACFID) Then
                        pdfName = String.Concat("OnOrderActionRequired_", buyerRACFID.Trim(), "_", datePart, ".pdf")
                        excelName = String.Concat("OnOrderActionRequired_", buyerRACFID.Trim(), "_", datePart, ".xlsx")
                        pdfOutputFile = String.Concat(outputDir, pdfName)
                        excelOutputFile = String.Concat(outputDir, excelName)

                        'Export PDF
                        iTextSharpHelper.GenerateNoSampleReport(pdfOutputFile, htmlTemplateFile, buyerLevelData)

                        Try

                            'Export Excel
                            'CreateExcelFile.CreateExcelDocument(noSampleReportData, excelOutputFile)
                            iTextSharpHelper.ExportNoSampleReportToExcel(excelOutputFile, "On Order, Action Required", buyerLevelData)
                            'iTextSharpHelper.GenerateNoSampleReportExcel(excelOutputFile, "On Order, Action Required", noSampleReportData)
                        Catch ex As Exception
                            Throw
                        End Try

                        LogHelper.WriteToConsole(String.Concat("Email On Order, Action Required Files. ", excelOutputFile))
                        LogHelper.WriteToLogFile(LogEntryType.Information, String.Concat("Email On Order, Action Required Files. ", excelOutputFile))

                        'Email Files
                        recipients = New List(Of String)()
                        commonBLL.GetRecipients("BuyerLevelOOARReportReceipients", recipients)
                        recipients.Add(String.Concat(buyerRACFID.Trim(), "@bonton.com"))
                        EmailHelper.Send(recipients, ConfigurationManager.AppSettings("FromEmailAddress"), "On Order, Action Required Report", "", pdfName & ";" & excelName, False)

                        LogHelper.WriteToConsole(String.Concat("Delete On Order, Action Required Report Files. ", excelOutputFile))
                        LogHelper.WriteToLogFile(LogEntryType.Information, String.Concat("Delete On Order, Action Required Report Files. ", excelOutputFile))

                        'Delete PDF
                        For Each f As String In Directory.GetFiles(outputDir, pdfName.Replace(datePart, "*"))
                            File.Delete(f)
                        Next

                        'Delete Excel
                        For Each f As String In Directory.GetFiles(outputDir, excelName.Replace(datePart, "*"))
                            File.Delete(f)
                        Next
                    Else
                        LogHelper.WriteToConsole(String.Format("Buyer email address not found for the buyer {0}. ", buyerID))
                        LogHelper.WriteToLogFile(LogEntryType.Warning, String.Format("Buyer email address not found for the buyer {0}. ", buyerID))
                    End If
                Next
            End If
        Else
            LogHelper.WriteToConsole("No items found for the On Order, Action Required Report.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "No items found for the On Order, Action Required Report.")
            recipients = New List(Of String)
            commonBLL.GetRecipients("NoEligibleItemsReceipients", recipients)
            EmailHelper.Send(recipients, ConfigurationManager.AppSettings("FromEmailAddress"),
                             "No items eligible for the On Order, Action Required Report",
                             String.Format("There are no items eligible for the On Order, Action Required Report for the purchase order ship date lesser than or equal to {0}. ", startShipDate.ToShortDateString()),
                             String.Empty)
        End If

        LogHelper.WriteToConsole("Begin Items to Auto Turn-In Next Week Report.")
        LogHelper.WriteToLogFile(LogEntryType.Information, "Begin Items to Auto Turn-In Next Week Report.")

        poShipDays = ConfigurationManager.AppSettings("AutoTurnInNextWeekPOShipDays")
        startShipDate = DateAdd(DateInterval.Day, poShipDays, Now)

        'Items to Auto Turn-In Next Week Report
        Dim _TUAutoTurnInReport As New TUAutoTurnInReport
        Dim autoTurnInReportData As List(Of AutoTurnInReportInfo) = _TUAutoTurnInReport.GetAutoTurnInReport(startShipDate)
        pdfName = "ItemsToAutoTurnInNextWeekReport_" & datePart & ".pdf"
        pdfOutputFile = outputDir & pdfName

        If Not autoTurnInReportData Is Nothing AndAlso autoTurnInReportData.Count > 0 Then
            LogHelper.WriteToConsole("Export Items to Auto Turn-In Next Week Report.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "Export Items to Auto Turn-In Next Week Report.")

            'Export PDF
            iTextSharpHelper.GenerateAutoTurnInReport(pdfOutputFile, htmlTemplateFile, autoTurnInReportData)

            LogHelper.WriteToConsole("Email Items to Auto Turn-In Next Week PDF.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "Email Items to Auto Turn-In Next Week Report PDF.")

            'Email PDF
            recipients = New List(Of String)
            commonBLL.GetRecipients("EMS", recipients)
            EmailHelper.Send(recipients, ConfigurationManager.AppSettings("FromEmailAddress"), "Items to Auto Turn-In Next Week", "", pdfName)

            LogHelper.WriteToConsole("Delete Items to Auto Turn-In Next Week PDF.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "Delete Items to Auto Turn-In Next Week PDF.")

            'Delete PDF
            For Each f As String In Directory.GetFiles(outputDir, pdfName.Replace(datePart, "*"))
                File.Delete(f)
            Next
        Else
            LogHelper.WriteToConsole("No items found for the Items to Auto Turn-In Next Week Report.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "No items found for the Items to Auto Turn-In Next Week Report.")
            recipients = New List(Of String)
            commonBLL.GetRecipients("NoEligibleItemsReceipients", recipients)
            EmailHelper.Send(recipients, ConfigurationManager.AppSettings("FromEmailAddress"),
                             "No items eligible for the Items to Auto Turn-In Next Week Report",
                             "There are no items eligible for the Items to Auto Turn-In Next Week Report.",
                             String.Empty)
        End If

        LogHelper.WriteToConsole("Begin No Final Image Report.")
        LogHelper.WriteToLogFile(LogEntryType.Information, "Begin No Final Image Report.")

        'No Final Image Report
        Dim _TUNoFinalImageReport As New TUNoFinalImageReport
        Dim noFinalImageReportPOShipDays As Integer = CInt(ConfigurationManager.AppSettings("NoFinalImageReportPOShipDays"))
        Dim noFinalImageReportData As List(Of NoFinalImageReportInfo) = _TUNoFinalImageReport.GetNoFinalImageReport(DateAdd(DateInterval.Day, noFinalImageReportPOShipDays, Now))
        pdfName = "NoFinalImageReport_" & datePart & ".pdf"
        pdfOutputFile = outputDir & pdfName
        Dim finalImageExcelName = "NoFinalImageReport_" & datePart & ".xls"
        Dim finalImageExcelOutputFile As String = outputDir & finalImageExcelName

        If Not noFinalImageReportData Is Nothing AndAlso noFinalImageReportData.Count > 0 Then
            LogHelper.WriteToConsole("Export No Final Image Report.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "Export No Final Image Report.")

            'Export PDF
            iTextSharpHelper.GenerateNoFinalImageReport(pdfOutputFile, htmlTemplateFile, noFinalImageReportData)

            'Export Excel
            iTextSharpHelper.GenerateNoFinalImageReportExcel(finalImageExcelOutputFile, "No Final Image Report", noFinalImageReportData)

            LogHelper.WriteToConsole("Email No Final Image Report PDF.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "Email No Final Image Report PDF.")

            'Email PDF
            recipients = New List(Of String)
            commonBLL.GetRecipients("Supervisors", recipients)
            EmailHelper.Send(recipients, ConfigurationManager.AppSettings("FromEmailAddress"), "No Final Image Report", "", pdfName & ";" & finalImageExcelName)

            LogHelper.WriteToConsole("Delete No Final Image Report PDF.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "Delete No Final Image Report PDF.")

            'Delete PDF
            For Each f As String In Directory.GetFiles(outputDir, pdfName.Replace(datePart, "*"))
                File.Delete(f)
            Next

            LogHelper.WriteToConsole("Delete No Final Image Report Excel.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "Delete No Final Image Report Excel.")

            'Delete Excel
            For Each f As String In Directory.GetFiles(outputDir, finalImageExcelName.Replace(datePart, "*"))
                File.Delete(f)
            Next
        Else
            LogHelper.WriteToConsole("No items found for the No Final Image Report.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "No items found for the No Final Image Report.")
            recipients = New List(Of String)
            commonBLL.GetRecipients("NoEligibleItemsReceipients", recipients)
            EmailHelper.Send(recipients, ConfigurationManager.AppSettings("FromEmailAddress"),
                             "No items eligible for the No Final Image Report",
                             "There are no items eligible for the No Final Image Report.",
                             String.Empty)
        End If

        LogHelper.WriteToConsole("Begin Image Grouping Report.")
        LogHelper.WriteToLogFile(LogEntryType.Information, "Begin Image Grouping Report.")

        'Image Grouping Report 
        Dim _TUImageGroupingReport As New TUImageGroupingReport
        Dim imageGroupingReportData As List(Of ImageGroupingReportInfo) = _TUImageGroupingReport.GetImageGroupingReport()
        pdfName = "ImageGroupingReport_" & datePart & ".pdf"
        pdfOutputFile = outputDir & pdfName

        If Not imageGroupingReportData Is Nothing AndAlso imageGroupingReportData.Count > 0 Then
            LogHelper.WriteToConsole("Export Image Grouping Report.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "Export Image Grouping Report.")

            'Export PDF
            iTextSharpHelper.GenerateImageGroupReport(pdfOutputFile, htmlTemplateFile, imageGroupingReportData)

            LogHelper.WriteToConsole("Email Image Grouping Report PDF.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "Email Image Grouping Report PDF.")

            'Email PDF
            recipients = New List(Of String)
            commonBLL.GetRecipients("EMS", recipients)
            EmailHelper.Send(recipients, ConfigurationManager.AppSettings("FromEmailAddress"), "Image Grouping Report", "", pdfName)

            LogHelper.WriteToConsole("Delete Image Grouping Report PDF.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "Delete Image Grouping Report PDF.")

            'Delete PDF
            For Each f As String In Directory.GetFiles(outputDir, pdfName.Replace(datePart, "*"))
                File.Delete(f)
            Next
        Else
            LogHelper.WriteToConsole("No records found for the Image Grouping Report.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "No records found for the Image Grouping Report.")
            recipients = New List(Of String)
            commonBLL.GetRecipients("NoEligibleItemsReceipients", recipients)
            EmailHelper.Send(recipients, ConfigurationManager.AppSettings("FromEmailAddress"),
                             "No items eligible for the Image Grouping Report",
                             "There are no items eligible for the Image Grouping Report.",
                             String.Empty)
        End If

        LogHelper.WriteToConsole("Begin TIA Exceptions Report.")
        LogHelper.WriteToLogFile(LogEntryType.Information, "Begin TIA Exceptions Report.")

        'TIA Exceptions Report
        Dim _TUTIAExceptionsReport As New TUTIAExceptionsReport
        Dim tiaExceptionsReportData As List(Of TIAExceptionsReportInfo) = _TUTIAExceptionsReport.GetTIAExceptionsReport()
        pdfName = "TUTIAExceptionsReport_" & datePart & ".pdf"
        pdfOutputFile = Path.Combine(outputDir, pdfName)

        If Not tiaExceptionsReportData Is Nothing AndAlso tiaExceptionsReportData.Count > 0 Then
            LogHelper.WriteToConsole("Export TIA Exceptions Report.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "Export TIA Exceptions Report.")

            'Export PDF
            iTextSharpHelper.GenerateTIAExceptionsReport(pdfOutputFile, htmlTemplateFile, tiaExceptionsReportData)

            LogHelper.WriteToConsole("Email TIA Exceptions Report PDF.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "Email TIA Exceptions Report PDF.")

            'Email PDF
            recipients = New List(Of String)
            commonBLL.GetRecipients("ITAdvertising", recipients)
            EmailHelper.Send(recipients, ConfigurationManager.AppSettings("FromEmailAddress"), "TIA Exceptions Report", "", pdfName)

            LogHelper.WriteToConsole("Delete TIA Exceptions Report PDF.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "Delete TIA Exceptions Report PDF.")

            'Delete PDF
            For Each f As String In Directory.GetFiles(outputDir, pdfName.Replace(datePart, "*"))
                File.Delete(f)
            Next
        Else
            LogHelper.WriteToConsole("No records found for the TIA Exceptions Report.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "No records found for the TIA Exceptions Report.")
        End If
    End Sub

    Private Function GetOutputDir() As String
        'Dim keys() As String = AppDomain.CurrentDomain.BaseDirectory.Split("\")
        'Dim ret As String = ""
        'If keys(keys.Length - 2) = "Debug" Or keys(keys.Length - 2) = "Release" Then
        '    ReDim Preserve keys(keys.Length - 4)
        'ElseIf keys(keys.Length - 2) = "bin" Then
        '    ReDim Preserve keys(keys.Length - 3)
        'End If
        Return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output") & "\"
    End Function

    Private Function GetExportDir() As String
        'Dim keys() As String = AppDomain.CurrentDomain.BaseDirectory.Split("\")
        'Dim ret As String = ""
        'If keys(keys.Length - 2) = "Debug" Or keys(keys.Length - 2) = "Release" Then
        '    ReDim Preserve keys(keys.Length - 4)
        'ElseIf keys(keys.Length - 2) = "bin" Then
        '    ReDim Preserve keys(keys.Length - 3)
        'End If
        'Return String.Join("\", keys) & "\Export\"
        Return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Export") & "\"
    End Function
End Class
