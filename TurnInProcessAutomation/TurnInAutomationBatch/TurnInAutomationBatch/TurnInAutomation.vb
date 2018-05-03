Imports BonTon.Common
Imports TurnInAutomationBatch.BLL
Imports TurnInAutomationBatch.BLL.Enumerations
Imports TurnInAutomationBatch.BusinessEntities
Imports TurnInAutomationBatch.Common
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory
Imports TurnInProcessAutomation.BLL
Imports System.Configuration
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Text
Imports Microsoft.Office.Interop
Imports System.Runtime.InteropServices
Imports System.Diagnostics

Module TurnInAutomation
    Public Const TurnInAutomationUserID As String = "TurnInAutomation"
    Sub Main()
        Dim commonBAO As BLL.Common = Nothing
        Dim recipients As List(Of String) = Nothing
        Dim spreadsheetImportRecipients As List(Of String) = Nothing
        Dim departments As List(Of DeptPageNumber) = Nothing
        Dim dtVendorImages As DataTable = Nothing
        Dim dtNoMerchImages As DataTable = Nothing
        Dim turnInBatchIDs As New List(Of Integer)
        Dim samplesCount As Integer = 0
        Dim startShipDate As Date = Date.MinValue
        Dim errorsCount As Integer = 0
        Dim defaultShipDays As Double = 0
        Dim turnInBatchID As Integer = 0
        Dim includeWebEligibleItemsOnly As Boolean = False
        Dim turnInMerchID As Integer = 0
        Dim tiaEligibleItemsExist As Boolean = False
        Dim vendorImageExcelPath As String = String.Empty
        Dim noMerchImageExcelPath As String = String.Empty
        Dim isVendorImageImportFailed As Boolean = False
        Dim isNoMerchImageImportFailed As Boolean = False
        Dim invalidNoMerchISNs As StringBuilder = Nothing
        Dim invalidVendorImageISNs As StringBuilder = Nothing

        Try
            LogHelper.WriteToConsole("Turn in automation batch process is started.")
            LogHelper.WriteToLogFile(LogEntryType.Information, "Turn in automation batch is started.")

            defaultShipDays = CDbl(ConfigurationManager.AppSettings("DefaultPOShipDays"))
            startShipDate = Date.Now().AddDays(defaultShipDays)

            LogHelper.WriteToConsole(String.Format("Default PO ship days configured is {0} and the PO ship date is {1}.", defaultShipDays.ToString(), startShipDate.ToShortDateString()))
            LogHelper.WriteToLogFile(LogEntryType.Information, String.Format("Default PO ship days configured is {0} and the PO ship date is {1}.", defaultShipDays.ToString(), startShipDate.ToShortDateString()))

            vendorImageExcelPath = ConfigurationManager.AppSettings("VendorImageExcelPath")
            noMerchImageExcelPath = ConfigurationManager.AppSettings("NoMerchImageExcelPath")

            'Delete the temporary vendor image excel files
            DeleteTemporaryExcelFiles(vendorImageExcelPath)

            dtVendorImages = ImportTurnInAutomationSpreadsheet(vendorImageExcelPath, True, isVendorImageImportFailed)

            'Delete the temporary no merch image excel files
            DeleteTemporaryExcelFiles(noMerchImageExcelPath)

            dtNoMerchImages = ImportTurnInAutomationSpreadsheet(noMerchImageExcelPath, False, isNoMerchImageImportFailed)

            commonBAO = New BLL.Common()
            recipients = New List(Of String)()
            commonBAO.GetRecipients("ITAdvertising", recipients)
            departments = commonBAO.GetTIAEnabledDepartments(ConfigurationManager.AppSettings("EnableTIADepartmentsKey"))

            If Not departments Is Nothing AndAlso departments.Count > 0 Then

                invalidNoMerchISNs = New StringBuilder()
                invalidVendorImageISNs = New StringBuilder()

                includeWebEligibleItemsOnly = IIf(ConfigurationManager.AppSettings("IncludeWebEligibleItems").ToString().ToUpper().Equals("Y"), True, False)

                TurnInEligibleItems(departments, startShipDate, includeWebEligibleItemsOnly, dtVendorImages, dtNoMerchImages, tiaEligibleItemsExist,
                                    samplesCount, turnInBatchIDs, invalidNoMerchISNs, invalidVendorImageISNs,
                                    True, recipients)

                TurnInEligibleItems(departments, startShipDate, includeWebEligibleItemsOnly, dtVendorImages, dtNoMerchImages, tiaEligibleItemsExist,
                                    samplesCount, turnInBatchIDs, invalidNoMerchISNs, invalidVendorImageISNs,
                                    False, recipients)

            End If

            'Send no merch import status
            If Not String.IsNullOrEmpty(invalidNoMerchISNs.ToString()) Then
                spreadsheetImportRecipients = New List(Of String)()
                commonBAO.GetRecipients("ImportSpreadsheetReceipients", spreadsheetImportRecipients)
                EmailHelper.Send(spreadsheetImportRecipients, ConfigurationManager.AppSettings("FromEmailAddress"),
                 "No Merch Spreadsheet Import Status",
                 invalidNoMerchISNs.ToString(),
                 String.Empty)

            End If

            'Send vendor image import status
            If Not String.IsNullOrEmpty(invalidVendorImageISNs.ToString()) Then
                spreadsheetImportRecipients = New List(Of String)()
                commonBAO.GetRecipients("ImportSpreadsheetReceipients", spreadsheetImportRecipients)
                EmailHelper.Send(spreadsheetImportRecipients, ConfigurationManager.AppSettings("FromEmailAddress"),
                 "Vendor Images Spreadsheet Import Status",
                 invalidVendorImageISNs.ToString(),
                 String.Empty)
            End If

            If Not isVendorImageImportFailed Then
                MoveProcessedExcelFiles(vendorImageExcelPath)
            End If

            If Not isNoMerchImageImportFailed Then
                MoveProcessedExcelFiles(noMerchImageExcelPath)
            End If

            LogHelper.WriteToLogFile(LogEntryType.Information, "Turn in automation batch process is complete.")
            LogHelper.WriteToConsole("Turn in automation batch process is complete.")

        Catch ex As Exception
            LogHelper.WriteToLogFile(LogEntryType.AppError, ex.ToString())
            LogHelper.WriteToConsole(ex.ToString())
            commonBAO.WriteToLogTable(turnInBatchID, turnInMerchID, [Enum].GetName(GetType(BLL.Enumerations.TurnInItemType), BLL.Enumerations.TurnInItemType.MerchandiseID),
                                      [Enum].GetName(GetType(BLL.Enumerations.BatchStatusCode), BLL.Enumerations.BatchStatusCode.F), ex.ToString(), TurnInAutomationUserID)
            EmailHelper.Send(recipients, ConfigurationManager.AppSettings("FromEmailAddress"),
                             "Turn in automation job failed",
                             String.Concat("An error occurred while generating the batches, please check log files and tables for more information about the error.",
                                                                                      "<br/><br/>Below is an abstract of the error:", vbNewLine, ex.ToString()),
                             String.Empty)
            errorsCount = errorsCount + 1
            Throw
        End Try

        GenerateReports(recipients, errorsCount)

        'Send notification if there are no items to be auto turned in
        If Not tiaEligibleItemsExist AndAlso errorsCount = 0 Then
            SendNoTIAItemsNotification(startShipDate)
        End If

        ' Deleting old log files
        LogHelper.DeleteOldLogs(ConfigurationManager.AppSettings("DeleteOldLogFilesThreshold"))
        LogHelper.WriteToLogFile(LogEntryType.Information, "Deleted old log files.")
        LogHelper.WriteToConsole("Deleted old log files.")

        'Send a successful completion notification if there was no error
        If errorsCount = 0 Then
            'Create a comma separated list of batch ID's
            Dim batchIDs As New StringBuilder
            If turnInBatchIDs.Count = 1 Then
                batchIDs.Append("The following batch was created: ")
            ElseIf turnInBatchIDs.Count > 1 Then
                batchIDs.Append("The following batches were created: ")
            End If
            For Each batchID As Integer In turnInBatchIDs
                batchIDs.Append(batchID)
                batchIDs.Append(", ")
            Next
            If batchIDs.ToString.Contains(",") Then
                batchIDs = batchIDs.Remove(batchIDs.Length - 2, 2)
            End If

            'Append the samples count
            If Not String.IsNullOrEmpty(batchIDs.ToString()) AndAlso samplesCount > 0 Then
                batchIDs.Append(String.Format(" and the number of samples turned-in was {0}.", samplesCount.ToString()))
            End If

            recipients = New List(Of String)()
            commonBAO.GetRecipients("JobCompletionEmailReceipients", recipients)

            EmailHelper.Send(recipients, ConfigurationManager.AppSettings("FromEmailAddress"),
                             "Turn in automation job completed",
                             "Turn in automation job completed.  " & batchIDs.ToString,
                             String.Empty)
        End If

    End Sub
#Region "Helper Methods"
    ''' <summary>
    ''' Reads ISN and color level details for the Ad and moves them to the next level
    ''' </summary>
    ''' <param name="adNumber"></param>
    ''' <param name="pageNumber"></param>
    ''' <param name="turnInBatchID"></param>
    ''' <remarks></remarks>
    Private Sub SubmitTurnInMeeting(ByVal adNumber As Integer, ByVal pageNumber As Integer, ByVal turnInBatchID As Integer)
        Dim turnInMeetCreateInfoList As List(Of ECommTurnInMeetCreateInfo) = Nothing
        Dim turnInMeetingSubmitList As List(Of ECommTurnInMeetCreateInfo) = Nothing
        Dim ecommMeeting As TUECommTurnInMeetResults = Nothing

        Try
            ecommMeeting = New TUECommTurnInMeetResults()
            turnInMeetCreateInfoList = ecommMeeting.GetEcommTurninMeet(adNumber.ToString(), pageNumber.ToString(), String.Empty, String.Empty, String.Empty, String.Empty, turnInBatchID).ToList()
            If Not turnInMeetCreateInfoList Is Nothing AndAlso turnInMeetCreateInfoList.Count > 0 Then
                turnInMeetingSubmitList = New List(Of ECommTurnInMeetCreateInfo)()
                For Each item As ECommTurnInMeetCreateInfo In turnInMeetCreateInfoList
                    turnInMeetingSubmitList.Add(ecommMeeting.GetEcommTurninMeetByMerchId(item.turnInMerchID))
                Next

                If turnInMeetingSubmitList.Count > 0 Then
                    turnInMeetingSubmitList.OrderBy(Function(a) a.FeatureSwatch)
                    ecommMeeting.SubmitMeetingPage(turnInMeetingSubmitList, TurnInAutomationUserID, True)
                End If
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Function ReplaceVowels(ByVal description As String) As String
        Dim vowelsExpression As String = "[aAeEiIoOuU]"
        Return Regex.Replace(description, vowelsExpression, String.Empty)
    End Function
    Private Function GetDayOfWeek() As String
        Dim dayOfWeek As String = Date.Today().ToString("dddd")
        'If Now.DayOfWeek = System.DayOfWeek.Sunday Then
        '    dayOfWeek = "Sunday"
        'ElseIf Now.DayOfWeek = System.DayOfWeek.Monday Then
        '    dayOfWeek = "Monday"
        'ElseIf Now.DayOfWeek = System.DayOfWeek.Tuesday Then
        '    dayOfWeek = "Tuesday"
        'ElseIf Now.DayOfWeek = System.DayOfWeek.Wednesday Then
        '    dayOfWeek = "Wednesday"
        'ElseIf Now.DayOfWeek = System.DayOfWeek.Thursday Then
        '    dayOfWeek = "Thursday"
        'ElseIf Now.DayOfWeek = System.DayOfWeek.Friday Then
        '    dayOfWeek = "Friday"
        'ElseIf Now.DayOfWeek = System.DayOfWeek.Saturday Then
        '    dayOfWeek = "Saturday"
        'End If
        Return dayOfWeek
    End Function
    Private Sub SendNoTIAItemsNotification(ByVal poShipDate As Date)
        Dim recipients As List(Of String) = Nothing
        Dim commonBLL As BLL.Common = Nothing

        Try
            recipients = New List(Of String)()
            commonBLL = New BLL.Common()
            commonBLL.GetRecipients("NoEligibleItemsReceipients", recipients)
            EmailHelper.Send(recipients, ConfigurationManager.AppSettings("FromEmailAddress"),
                             "No Items Eligible for Automated Turn in",
                             String.Format("There are no items eligible for Automated Turn in for the purchase order ship date lesser than or equal to {0}. ", poShipDate.ToShortDateString()),
                             String.Empty)
        Catch ex As Exception
            LogHelper.WriteToConsole(ex.ToString())
            LogHelper.WriteToLogFile(LogEntryType.AppError, ex.ToString())
        End Try
    End Sub
    Private Function ImportTurnInAutomationSpreadsheet(ByVal vendorImageExcelPath As String, ByVal isVendorImage As Boolean,
                                         ByRef isImportFailed As Boolean) As DataTable
        Dim dtVendorImageISNs As DataTable = Nothing
        Dim vendorImageExcelDirectory As DirectoryInfo = Nothing
        Dim existingExcelProcessIDs As List(Of Integer) = Nothing
        Dim recipients As List(Of String) = Nothing
        Dim commonBAO As BLL.Common = Nothing

        Try

            If Not String.IsNullOrEmpty(vendorImageExcelPath) Then
                commonBAO = New BLL.Common()
                recipients = New List(Of String)()
                commonBAO.GetRecipients("ImportSpreadsheetReceipients", recipients)

                vendorImageExcelDirectory = New DirectoryInfo(vendorImageExcelPath)

                dtVendorImageISNs = New DataTable()
                For Each excelFileInfo As FileInfo In vendorImageExcelDirectory.GetFiles
                    Try
                        If Not excelFileInfo Is Nothing _
                            AndAlso (excelFileInfo.Extension.ToLower().Equals(".xlsx")) Then
                            If dtVendorImageISNs.Rows.Count > 0 Then
                                dtVendorImageISNs.Merge(ImportExcel.ExcelToDataTableOpenXML(excelFileInfo.FullName, String.Empty))
                            Else
                                dtVendorImageISNs = ImportExcel.ExcelToDataTableOpenXML(excelFileInfo.FullName, String.Empty)
                            End If
                            MoveProcessedExcelFiles(vendorImageExcelPath, True, excelFileInfo.FullName)
                        End If
                    Catch ex As Exception
                        LogHelper.WriteToConsole(ex.ToString())
                        LogHelper.WriteToLogFile(LogEntryType.AppError, ex.ToString())
                        isImportFailed = True
                        EmailHelper.Send(recipients, ConfigurationManager.AppSettings("FromEmailAddress"),
                                         IIf(isVendorImage, "Vendor Image Import failed", "No Merch Import failed"),
                                         String.Concat(String.Format(IIf(isVendorImage,
                                         "An error occurred while importing vendor images spreadsheet {0}, please check log files and tables for more information about the error.",
                                         "An error occurred while importing no merch items spreadsheet {0}, please check log files and tables for more information about the error."), excelFileInfo.Name),
                                                                                "<br/><br/>Below is an abstract of the error:", vbNewLine, ex.ToString()),
                                         String.Empty)
                    End Try
                Next

            End If
        Catch ex As Exception
            LogHelper.WriteToConsole(ex.ToString())
            LogHelper.WriteToLogFile(LogEntryType.AppError, ex.ToString())
            isImportFailed = True
            EmailHelper.Send(recipients, ConfigurationManager.AppSettings("FromEmailAddress"),
                             IIf(isVendorImage, "Vendor Image Import failed", "No Merch Import failed"),
                             String.Concat(IIf(isVendorImage,
                             "An error occurred while importing vendor images, please check log files and tables for more information about the error.",
                             "An error occurred while importing no merch items, please check log files and tables for more information about the error."),
                                                                    "<br/><br/>Below is an abstract of the error:", vbNewLine, ex.ToString()),
                             String.Empty)
        Finally
            vendorImageExcelDirectory = Nothing
        End Try

        Return dtVendorImageISNs
    End Function
    Private Sub SaveExcelAsXLSX(ByVal directoryPath As String)
        Dim vendorImageExcelDirectory As DirectoryInfo = Nothing
        Dim excelApp As Excel.Application = Nothing
        Dim excelWorkBook As Excel.Workbook = Nothing
        Dim excelWorksheet As Excel.Worksheet = Nothing
        Dim headersRange As Excel.Range = Nothing

        Dim fileNameWithoutExtension As String = String.Empty
        Try

            vendorImageExcelDirectory = New DirectoryInfo(directoryPath)
            For Each excelFileInfo As FileInfo In vendorImageExcelDirectory.GetFiles
                If Not excelFileInfo Is Nothing AndAlso excelFileInfo.Extension.ToLower().Equals(".xls") Then
                    excelApp = New Excel.Application()
                    excelWorkBook = excelApp.Workbooks.Open(excelFileInfo.FullName)
                    excelWorksheet = excelWorkBook.ActiveSheet
                    For index = 1 To 5
                        headersRange = excelWorksheet.Cells(index, 2)
                        If String.IsNullOrEmpty(headersRange.Text) Then
                            headersRange.EntireRow.Delete()
                        End If
                    Next

                    fileNameWithoutExtension = excelFileInfo.FullName.Substring(0, excelFileInfo.FullName.LastIndexOf("."))
                    excelWorkBook.Saved = True
                    excelWorkBook.SaveCopyAs(String.Concat(fileNameWithoutExtension, "_Temp.xls"))
                    excelWorkBook.Close(True)
                    excelApp.Quit()
                    excelApp = Nothing
                    excelWorkBook = Nothing
                End If
            Next
        Catch ex As Exception
            Throw
        Finally
            ReleaseComObject(headersRange)
            headersRange = Nothing
            ReleaseComObject(excelWorksheet)
            excelWorksheet = Nothing
            ReleaseComObject(excelWorkBook)
            excelWorkBook = Nothing
            ReleaseComObject(excelApp)
            excelApp = Nothing
        End Try
    End Sub
    Private Sub ReleaseComObject(ByRef Reference As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(Reference)
            Reference = Nothing
        Catch
            Reference = Nothing
        Finally
            Reference = Nothing
            GC.Collect()
        End Try
    End Sub
    Private Function GetExistingExcelProcess() As List(Of Integer)
        Dim activeExcelProcesses As New List(Of Integer)
        Dim activeProcesses() As Process = Process.GetProcessesByName("excel")
        For Each excelProcess As Process In activeProcesses
            activeExcelProcesses.Add(excelProcess.Id)
        Next
        Return activeExcelProcesses
    End Function
    Private Sub KillExcelProcess(ByVal existingProcesses As List(Of Integer))
        Dim activeProcesses As Process() = Process.GetProcessesByName("excel")

        For Each excelProcess As Process In activeProcesses
            If Not existingProcesses.Contains(excelProcess.Id) Then
                excelProcess.Kill()
            End If
        Next

        activeProcesses = Nothing

    End Sub
    Private Sub CompleteTurnIn(ByVal internalStyleNumbers As List(Of EcommSetupCreateInfo), ByVal startShipDate As Date, ByVal includeWebEligibleItemsOnly As Boolean, ByVal adNumber As Integer, ByVal pageNumber As Integer,
                               ByVal dtNoMerchImages As DataTable, ByVal dtVendorImages As DataTable, ByRef tiaEligibleItemsExist As Boolean, ByRef turnInBatchID As Integer, ByRef samplesCount As Integer,
                               ByRef turnInBatchIDs As List(Of Integer), ByRef invalidNoMerchISNs As StringBuilder, ByRef invalidVendorImageISNs As StringBuilder)
        Dim ecommResultsBAO As EcommResults = Nothing
        Dim imageBAO As Image = Nothing
        Dim merchandiseBAO As Merchandise = Nothing
        Dim colorLevelDetails As List(Of EcommSetupClrSzInfo) = Nothing
        Dim modelAttributes As Model = Nothing
        Dim commonBAO As BLL.Common = Nothing
        Dim webCategories As List(Of WebCat) = Nothing
        Dim turnInMeetCreateInfo As ECommTurnInMeetCreateInfo = Nothing
        Dim turnInMeetingSubmitList As List(Of ECommTurnInMeetCreateInfo) = Nothing
        Dim turnInMeetResults As TUECommTurnInMeetResults = Nothing
        Dim colorBAO As Color = Nothing
        Dim colorFamily As ClrSizLocLookUp = Nothing
        Dim tms900Parameter As TUTMS900PARAMETER = Nothing
        Dim modelCategories As List(Of TMS900PARAMETERInfo) = Nothing
        Dim modelCategory As TMS900PARAMETERInfo = Nothing
        Dim merchLevelAttr As MerchLevelAttribute = Nothing
        Dim colorLevelDetailsByVendorStyle As List(Of EcommSetupClrSzInfo) = Nothing
        Dim ecommSetupCreate As TUEcommSetupCreate = Nothing
        Dim styleSKUColorsByVendorStyle As List(Of SampleRequestInfo) = Nothing
        Dim waistDownGenericSubclasses As List(Of TMS900PARAMETERInfo) = Nothing
        Dim size1And2Parameters As List(Of TMS900PARAMETERInfo) = Nothing
        Dim shouldAddSize1And2 As Boolean = False
        Dim isWaistDownSubClass As Boolean = False
        Dim imageRequestID As Integer = 0
        Dim merchandiseID As Integer = 0
        Dim turnInMerchID As Integer = 0
        Dim imageCategoryCode As String = String.Empty
        Dim imageDesc As String = String.Empty
        Dim friendlyColorDescription As String = String.Empty
        Dim imageName As String = String.Empty
        Dim colorFamilyDesc As String = String.Empty
        Dim modelCategoryDesc As String = String.Empty
        Dim colorFamilyDescription As String = String.Empty
        Dim labelDescription As String = String.Empty
        Dim genericClassDescription As String = String.Empty
        Dim vendorStyleNumber As String = String.Empty

        Try
            ecommResultsBAO = New EcommResults()
            ecommSetupCreate = New TUEcommSetupCreate()
            merchandiseBAO = New Merchandise()
            commonBAO = New BLL.Common()
            imageBAO = New Image()

            For Each approvedItem As EcommSetupCreateInfo In internalStyleNumbers

                tiaEligibleItemsExist = True

                approvedItem.PageNumber = pageNumber

                'Get web category from history by DeptID, Vendor id, generic class id, generic sub class id and sub class id of the ISN
                webCategories = New List(Of WebCat)()
                'Get the web category from Webcat, if nothing is found then check turn in for web category
                webCategories.Add(ecommResultsBAO.GetWebCategory(approvedItem.DeptId, approvedItem.VendorId, approvedItem.GenericClassId,
                                                                 approvedItem.GenericSubClassId, approvedItem.ClassID, approvedItem.SubClassID, True))
                If webCategories(0) Is Nothing Then
                    webCategories.Add(ecommResultsBAO.GetWebCategory(approvedItem.DeptId, approvedItem.VendorId, approvedItem.GenericClassId,
                                                                     approvedItem.GenericSubClassId, approvedItem.ClassID, approvedItem.SubClassID))
                End If

                If webCategories(0) Is Nothing Then
                    LogHelper.WriteToLogFile(LogEntryType.Warning, String.Format("Web Category information not found for ISN {0}, Dept {1}, Vendor {2}, Generic Class {3}, Generic SubClass {4} and SubClass {5}.",
                                                                       approvedItem.ISN, approvedItem.DeptId, approvedItem.VendorId,
                                                                       approvedItem.GenericClassId, approvedItem.GenericSubClassId, approvedItem.SubClassID))
                    commonBAO.WriteToLogTable(turnInBatchID, approvedItem.ISN, [Enum].GetName(GetType(BLL.Enumerations.TurnInItemType), BLL.Enumerations.TurnInItemType.InternalStyleNumber),
                                              [Enum].GetName(GetType(BLL.Enumerations.BatchStatusCode), BLL.Enumerations.BatchStatusCode.W),
                                              String.Format("Web Category information not found for ISN {0}, Dept {1}, Vendor {2}, Generic Class {3}, Generic SubClass {4} and SubClass {5}.",
                                                                       approvedItem.ISN, approvedItem.DeptId, approvedItem.VendorId,
                                                                       approvedItem.GenericClassId, approvedItem.GenericSubClassId, approvedItem.SubClassID), TurnInAutomationUserID)
                    webCategories = New List(Of WebCat)()
                    webCategories.Add(New WebCat With {
                                      .DefaultCategoryFlag = False, _
                                        .CategoryCode = 0})
                End If
                tms900Parameter = New TUTMS900PARAMETER()
                waistDownGenericSubclasses = tms900Parameter.GetAllWaistDownClasses()
                size1And2Parameters = tms900Parameter.GetAllSize1And2Classes(approvedItem.DeptId, approvedItem.ClassID)

                'Update the web category for the ISN
                ecommSetupCreate.InsertISNData(approvedItem.ISN, approvedItem.LabelId, approvedItem.IsReserve, String.Empty, webCategories, TurnInAutomationUserID)
                colorLevelDetails = ecommResultsBAO.GetApprovedEcommSetupCreateDetailByISN(approvedItem.ISN.ToString(), startShipDate, includeWebEligibleItemsOnly, invalidNoMerchISNs, invalidVendorImageISNs,
                                                                                           dtNoMerchImages, dtVendorImages, approvedItem.IsVendorImage)

                If Not colorLevelDetails Is Nothing AndAlso colorLevelDetails.Count > 0 Then
                    If approvedItem.IsVendorImage Then
                        LogHelper.WriteToLogFile(LogEntryType.Warning, String.Format("Model information default values set for the vendor image ISN {0}, Dept {1} and Vendor {2}.", approvedItem.ISN, approvedItem.DeptId, approvedItem.VendorId))
                        commonBAO.WriteToLogTable(turnInBatchID, approvedItem.ISN, [Enum].GetName(GetType(BLL.Enumerations.TurnInItemType), BLL.Enumerations.TurnInItemType.InternalStyleNumber),
                                                  [Enum].GetName(GetType(BLL.Enumerations.BatchStatusCode), BLL.Enumerations.BatchStatusCode.W),
                                                  String.Format("Model information default values set for the vendor image ISN {0}, Dept {1} and Vendor {2}.", approvedItem.ISN, approvedItem.DeptId, approvedItem.VendorId), TurnInAutomationUserID)

                        modelAttributes = New Model()
                        modelAttributes.ModelCategoryCode = "0"
                        'Set the default code as OFF
                        modelAttributes.MerchandiseFigureCode = [Enum].GetName(GetType(BLL.Enumerations.MerchandiseFigureCode), BLL.Enumerations.MerchandiseFigureCode.OFF)
                    Else
                        modelAttributes = merchandiseBAO.GetModelAttributes(approvedItem.DeptId, approvedItem.VendorId)

                        'If there is no model information found then move on to the next ISN
                        If modelAttributes Is Nothing OrElse modelAttributes.MerchandiseFigureCode Is Nothing OrElse modelAttributes.ModelCategoryCode Is Nothing Then
                            LogHelper.WriteToLogFile(LogEntryType.Warning, String.Format("Model information not found for ISN {0}, Dept {1} and Vendor {2}.", approvedItem.ISN, approvedItem.DeptId, approvedItem.VendorId))
                            commonBAO.WriteToLogTable(turnInBatchID, approvedItem.ISN, [Enum].GetName(GetType(BLL.Enumerations.TurnInItemType), BLL.Enumerations.TurnInItemType.InternalStyleNumber),
                                                      [Enum].GetName(GetType(BLL.Enumerations.BatchStatusCode), BLL.Enumerations.BatchStatusCode.W),
                                                      String.Format("Model information not found for ISN {0}, Dept {1} and Vendor {2}.", approvedItem.ISN, approvedItem.DeptId, approvedItem.VendorId), TurnInAutomationUserID)

                            modelAttributes = New Model()
                            modelAttributes.ModelCategoryCode = "0"
                            'Set the default code as OFF
                            modelAttributes.MerchandiseFigureCode = [Enum].GetName(GetType(BLL.Enumerations.MerchandiseFigureCode), BLL.Enumerations.MerchandiseFigureCode.OFF)
                        End If
                    End If

                    colorLevelDetails = colorLevelDetails.GroupBy(Function(a) a.VendorColorCode).Select(Function(b) b.First()).ToList()

                    'Update the sample count
                    samplesCount = samplesCount + colorLevelDetails.Count

                    If String.IsNullOrEmpty(vendorStyleNumber) OrElse vendorStyleNumber <> approvedItem.VendorStyleNumber.Trim() Then
                        vendorStyleNumber = approvedItem.VendorStyleNumber.Trim()
                        colorLevelDetailsByVendorStyle = ecommResultsBAO.GetApprovedEcommSetupCreateDetailByVendorStyle(approvedItem.VendorStyleNumber, startShipDate,
                                                                                                                        includeWebEligibleItemsOnly,
                                                                                                                        IIf(approvedItem.IsNoMerchImage, approvedItem.IsNoMerchImage, approvedItem.IsVendorImage),
                                                                                                                        approvedItem.DeptId)

                        styleSKUColorsByVendorStyle = ecommResultsBAO.GetColorsFromSSKUByDeptVendorStyle(approvedItem.DeptId, approvedItem.VendorStyleNumber)
                    End If

                    shouldAddSize1And2 = False

                    If Not size1And2Parameters Is Nothing AndAlso size1And2Parameters.Count > 0 Then
                        shouldAddSize1And2 = True
                    End If

                    'Instantiate color
                    colorBAO = New Color()

                    colorLevelDetails.OrderBy(Function(a) a.OnHand + a.OnOrder)

                    For Each colorDetails As EcommSetupClrSzInfo In colorLevelDetails
                        LogHelper.WriteToConsole(String.Format("ISN {0} and color {1} have been selected for processing.", approvedItem.ISN, String.Concat(colorDetails.VendorColorCode.ToString(), "-", colorDetails.Color.Trim())))
                        LogHelper.WriteToLogFile(LogEntryType.Information, String.Format("ISN {0} and color {1} have been selected for processing.", approvedItem.ISN,
                                                                               String.Concat(colorDetails.VendorColorCode.ToString(), "-", colorDetails.Color.Trim())))

                        imageName = String.Concat(approvedItem.ISNDesc.Trim(), "-", colorDetails.Color.Trim(), "-", approvedItem.LabelDesc.Trim())
                        'Maximum length for image name is 51, so truncate the characters at the end if the length is greater than 51
                        imageName = If(imageName.Trim().Length > 51, imageName.Substring(0, 50), imageName)
                        turnInBatchID = ecommSetupCreate.InsertISNDataColorLevel(approvedItem.ISN, approvedItem.DeptId, colorDetails.VendorColorCode, TurnInUsageCode.Ecommerce,
                                                                 imageName, adNumber, pageNumber, approvedItem.IsReserve, String.Empty,
                                                                               modelAttributes.ModelCategoryCode, "N", TurnInAutomationUserID, turnInBatchID,
                                                                       imageCategoryCode, modelAttributes.MerchandiseFigureCode, StrConv(approvedItem.ISNDesc.Trim(), VbStrConv.ProperCase), turnInMerchID)

                        'Get the color family from Webcat, if nothing is found then check turn in and style sku for color family
                        colorFamily = colorBAO.GetFrequentlyUsedColorFamily(approvedItem.DeptId, approvedItem.VendorId, colorDetails.VendorColorCode, True)

                        'Turn in history
                        If colorFamily Is Nothing OrElse String.IsNullOrEmpty(colorFamily.Value) Then
                            colorFamily = colorBAO.GetFrequentlyUsedColorFamily(approvedItem.DeptId, approvedItem.VendorId, colorDetails.VendorColorCode)
                        End If

                        'Style SKU history
                        If colorFamily Is Nothing OrElse String.IsNullOrEmpty(colorFamily.Value) Then
                            colorFamily = colorBAO.GetFrequentlyUsedColorFamilyFromStyleSKU(approvedItem.DeptId, approvedItem.VendorId, colorDetails.VendorColorCode)
                        End If

                        If Not colorFamily Is Nothing AndAlso Not String.IsNullOrEmpty(colorFamily.Value) Then
                            ecommSetupCreate.UpdateColorFamilyFlood(turnInMerchID, colorFamily.Value, TurnInAutomationUserID)
                        End If

                        'Get the color family description for the color family code
                        If Not colorFamily Is Nothing AndAlso Not colorFamily.Text Is Nothing Then
                            colorDetails.ColorFamily = colorFamily.Text
                        End If

                        turnInMeetResults = New TUECommTurnInMeetResults()

                        merchLevelAttr = merchandiseBAO.GetMerchLevelAttributesByISN(approvedItem.ISN, colorDetails.VendorStyleNum,
                                                                                     colorDetails.VendorColorCode, approvedItem.DeptId)

                        'Determine the image category
                        imageCategoryCode = imageBAO.GetImageCategoryCodeByISN(merchLevelAttr, colorLevelDetails, colorDetails.VendorColorCode,
                                                                               colorLevelDetailsByVendorStyle, styleSKUColorsByVendorStyle)

                        'Set feature/swatch/stand alone
                        colorDetails.FeatureRenderSwatch = imageCategoryCode

                        Dim colorDetailsByVendorStyle = colorLevelDetailsByVendorStyle.Find(Function(a) a.ISN = approvedItem.ISN And a.VendorColorCode = colorDetails.VendorColorCode)
                        If Not colorDetailsByVendorStyle Is Nothing Then
                            colorLevelDetailsByVendorStyle.Find(Function(a) a.ISN = approvedItem.ISN And a.VendorColorCode = colorDetails.VendorColorCode).FeatureRenderSwatch = imageCategoryCode
                        End If

                        'Update the image category for the merch id
                        turnInMeetResults.UpdateCCFlood(turnInMerchID.ToString(), String.Empty, String.Empty, String.Empty, imageCategoryCode, imageCategoryCode, String.Empty, TurnInAutomationUserID)

                        ''Update the copy notes for the merch id
                        If colorDetails.IsVendorImage Then
                            turnInMeetResults.UpdateCWInfo(turnInMerchID, colorDetails.CopyNotes, "N", TurnInAutomationUserID)
                        End If

                        'Update the other information in the TTU tables
                        turnInMeetCreateInfo = New ECommTurnInMeetCreateInfo()

                        colorFamilyDescription = colorDetails.ColorFamily.Trim()
                        colorFamilyDescription = If(colorFamilyDescription.Length() > 4,
                                                          colorFamilyDescription.Substring(0, 4),
                                                          colorFamilyDescription).Trim()

                        labelDescription = approvedItem.LabelDesc.Trim()
                        labelDescription = If(labelDescription.Length() > 6,
                                                          labelDescription.Substring(0, 6),
                                                          labelDescription).Trim()

                        genericClassDescription = approvedItem.GenericClass.Trim()
                        genericClassDescription = If(genericClassDescription.Length() > 11,
                                                          genericClassDescription.Substring(0, 11),
                                                          genericClassDescription).Trim()

                        imageDesc = String.Concat(labelDescription, "-", genericClassDescription, "-", If(approvedItem.VendorStyleNumber.Trim().Length() > 6,
                                                          approvedItem.VendorStyleNumber.Trim().Substring(approvedItem.VendorStyleNumber.Trim().Length - 6, 6),
                                                          approvedItem.VendorStyleNumber.Trim()), "-", colorFamilyDescription)

                        'Get the friendly color from style sku
                        friendlyColorDescription = colorDetails.FriendlyColor

                        'Get the friendly color from Webcat, if nothing is found then check turn in and style sku for friendly color
                        'friendlyColorDescription = colorBAO.GetFrequentlyUsedFriendlyColor(approvedItem.DeptId, approvedItem.VendorId, colorDetails.VendorColorCode, True)

                        'If String.IsNullOrEmpty(friendlyColorDescription) Then
                        '    friendlyColorDescription = colorBAO.GetFrequentlyUsedFriendlyColor(approvedItem.DeptId, approvedItem.VendorId, colorDetails.VendorColorCode)
                        'End If

                        tms900Parameter = New TUTMS900PARAMETER()
                        modelCategories = tms900Parameter.GetAllModelCategories()

                        modelCategoryDesc = String.Empty

                        If Not modelAttributes Is Nothing AndAlso modelCategories.Count > 0 Then
                            modelCategory = modelCategories.Find(Function(a) a.CharIndex = modelAttributes.ModelCategoryCode)
                            If Not modelCategory Is Nothing AndAlso Not String.IsNullOrEmpty(modelCategory.LongDesc) Then
                                If modelCategory.LongDesc().ToUpper().Contains("INTIMATE") Then
                                    modelCategoryDesc = modelCategory.ShortDesc().Trim()
                                Else
                                    modelCategoryDesc = modelCategory.LongDesc().Trim()
                                End If
                            End If
                        End If

                        isWaistDownSubClass = False

                        If Not waistDownGenericSubclasses Is Nothing AndAlso waistDownGenericSubclasses.Count > 0 _
                            AndAlso waistDownGenericSubclasses.Exists(Function(a) a.CharIndex = approvedItem.GenericSubClassId.ToString()) Then
                            isWaistDownSubClass = True
                        End If

                        With turnInMeetCreateInfo
                            .turnInMerchID = turnInMerchID
                            .HotListCDE = "N"
                            .ImageGrp = 0
                            'Image description field length is 30, so the stored procedure will fail if we pass more characters
                            .ImageDesc = If(imageDesc.Length() > 30, imageDesc.Substring(0, 29), imageDesc)
                            .ImageNotes = imageBAO.GetImageNotes(imageCategoryCode, modelCategoryDesc, .FeatureID,
                                                                 modelAttributes.MerchandiseFigureCode, isWaistDownSubClass,
                                                                 shouldAddSize1And2, colorDetails.Size1Desc, colorDetails.Size2Desc)
                            .AltView = String.Empty
                            .PickupImageID = colorDetails.PuImageID
                            .ModelCategory = If(imageCategoryCode = [Enum].GetName(GetType(ImageCategoryCode), TurnInAutomationBatch.BLL.Enumerations.ImageCategoryCode.SWTCH),
                                                String.Empty, modelAttributes.ModelCategoryCode)
                            .ColorCorrect = "N"
                            .StylingNotes = String.Empty
                            .CCFollowUpFlag = "N"
                            .OnOff = If(imageCategoryCode = [Enum].GetName(GetType(ImageCategoryCode), TurnInAutomationBatch.BLL.Enumerations.ImageCategoryCode.SWTCH),
                                        String.Empty, modelAttributes.MerchandiseFigureCode)
                            .FeatureSwatch = imageCategoryCode
                            .ImageCategoryCode = imageCategoryCode
                            .ImageKindCode = IIf(colorDetails.IsNoMerchImage, [Enum].GetName(GetType(BLL.Enumerations.ImageKindType), ImageKindType.NOMER),
                                                 IIf(colorDetails.IsVendorImage, [Enum].GetName(GetType(BLL.Enumerations.ImageKindType), ImageKindType.VND),
                                                     [Enum].GetName(GetType(BLL.Enumerations.ImageKindType), ImageKindType.NEW)))
                            .ColorCode = colorDetails.VendorColorCode
                            .SampleSize = colorDetails.SampleSize
                            .MerchID = IIf(colorDetails.IsVendorImage OrElse colorDetails.IsNoMerchImage, 0, colorDetails.SampleMerchId)
                            .FriendlyColor = If(String.IsNullOrEmpty(friendlyColorDescription), colorDetails.Color.Trim(), friendlyColorDescription)
                            .VendorStyleNumber = approvedItem.VendorStyleNumber
                            .ISN = approvedItem.ISN
                            .FriendlyProdDesc = approvedItem.ISNDesc.Trim()
                            .FeatureID = If(imageCategoryCode = [Enum].GetName(GetType(ImageCategoryCode), TurnInAutomationBatch.BLL.Enumerations.ImageCategoryCode.SWTCH) _
                                            OrElse imageCategoryCode = [Enum].GetName(GetType(ImageCategoryCode), TurnInAutomationBatch.BLL.Enumerations.ImageCategoryCode.REND),
                                            merchLevelAttr.FeatureImageID, 0)
                        End With

                        turnInMeetResults.UpdateCCInfo(turnInMeetCreateInfo, TurnInAutomationUserID)

                        If turnInBatchID > 0 AndAlso turnInMerchID > 0 Then
                            'Complete the turn in setuup
                            ecommSetupCreate.SubmitColorSizeDataforTIA(turnInBatchID, TurnInAutomationUserID, turnInMerchID)
                        End If
                    Next
                End If

                'Complete the turn in meeting
                SubmitTurnInMeeting(adNumber, pageNumber, turnInBatchID)
                commonBAO.WriteToLogTable(turnInBatchID, approvedItem.ISN, [Enum].GetName(GetType(BLL.Enumerations.TurnInItemType), BLL.Enumerations.TurnInItemType.InternalStyleNumber),
                                                  [Enum].GetName(GetType(BLL.Enumerations.BatchStatusCode), BLL.Enumerations.BatchStatusCode.S), "Batch Created Successfully", TurnInAutomationUserID)

                'Get a list of Batch ID's
                If turnInBatchID > 0 Then
                    If Not turnInBatchIDs.Contains(turnInBatchID) Then
                        turnInBatchIDs.Add(turnInBatchID)
                    End If
                End If
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub DeleteTemporaryExcelFiles(ByVal vendorExcelDirectoryPath As String)
        Dim vendorImageExcelDirectory As DirectoryInfo = Nothing
        Try
            vendorImageExcelDirectory = New DirectoryInfo(vendorExcelDirectoryPath)
            'Delete the temp files from the directory
            For Each excelFileInfo As FileInfo In vendorImageExcelDirectory.GetFiles
                If Not excelFileInfo Is Nothing AndAlso (excelFileInfo.Extension.ToLower().Equals(".xls") OrElse excelFileInfo.Extension.ToLower().Equals(".xlsx")) _
                    AndAlso excelFileInfo.Name.StartsWith("~") Then
                    excelFileInfo.Delete()
                End If
            Next
        Catch ex As Exception
            Throw
        Finally
            vendorImageExcelDirectory = Nothing
        End Try
    End Sub
    Private Sub MoveProcessedExcelFiles(ByVal vendorExcelDirectoryPath As String, Optional ByVal shouldMoveIndividualFile As Boolean = False, Optional ByVal excelFileName As String = "")
        Dim vendorImageExcelDirectory As DirectoryInfo = Nothing
        Dim destFileInfo As FileInfo = Nothing
        Dim individualFileInfo As FileInfo = Nothing

        Try
            If Not shouldMoveIndividualFile Then
                vendorImageExcelDirectory = New DirectoryInfo(vendorExcelDirectoryPath)
                'Delete the temp files from the directory
                'Move the processed files to the processed directory
                For Each excelFileInfo As FileInfo In vendorImageExcelDirectory.GetFiles
                    If Not excelFileInfo Is Nothing AndAlso (excelFileInfo.Extension.ToLower().Equals(".xls") OrElse excelFileInfo.Extension.ToLower().Equals(".xlsx")) _
                        AndAlso Not excelFileInfo.FullName.Contains("_Temp") Then
                        destFileInfo = New FileInfo(Path.Combine(vendorExcelDirectoryPath, "Processed", excelFileInfo.Name))
                        If destFileInfo.Exists Then
                            destFileInfo.Delete()
                        Else
                            excelFileInfo.MoveTo(Path.Combine(vendorExcelDirectoryPath, "Processed", excelFileInfo.Name))
                        End If
                    End If
                Next
            ElseIf Not String.IsNullOrEmpty(excelFileName) Then
                individualFileInfo = New FileInfo(excelFileName)
                destFileInfo = New FileInfo(Path.Combine(vendorExcelDirectoryPath, "Processed", individualFileInfo.Name))

                If destFileInfo.Exists Then
                    destFileInfo.Delete()
                End If

                individualFileInfo.MoveTo(Path.Combine(vendorExcelDirectoryPath, "Processed", individualFileInfo.Name))
            End If
        Catch ex As Exception
            Throw
        Finally
            vendorImageExcelDirectory = Nothing
        End Try
    End Sub
    Private Sub GenerateReports(ByVal recipients As List(Of String), ByRef errorsCount As Integer)
        'Only generate reports on day of week from config file
        LogHelper.WriteToConsole("Turn in automation reports process is started.")
        LogHelper.WriteToLogFile(LogEntryType.Information, "Turn in automation reports is started.")

        Try
            'Run weekly reports once a week
            If GetDayOfWeek() = ConfigurationManager.AppSettings("TIAReportingDay").ToString Then
                'Generate weekly reports
                Dim rpts As New WeeklyReports
                rpts.GenerateReport()
            End If
        Catch ex As Exception
            LogHelper.WriteToLogFile(LogEntryType.AppError, ex.ToString())
            LogHelper.WriteToConsole(ex.ToString())
            EmailHelper.Send(recipients, ConfigurationManager.AppSettings("FromEmailAddress"),
                             "Turn in automation report generation failed",
                             String.Concat("An error occurred while generating the reports, please check log files and tables for more information about the error.",
                                           "<br/><br/>Below is an abstract of the error:", vbNewLine, ex.ToString()),
                             String.Empty)
            errorsCount = errorsCount + 1
        End Try

        LogHelper.WriteToConsole("Turn in automation reports process has completed.")
        LogHelper.WriteToLogFile(LogEntryType.Information, "Turn in automation reports process has completed.")
    End Sub

    Private Sub TurnInEligibleItems(ByVal departments As List(Of DeptPageNumber), ByVal startShipDate As Date,
                                   ByVal includeWebEligibleItemsOnly As Boolean, ByVal dtVendorImages As DataTable,
                                   ByVal dtNoMerchImages As DataTable, ByRef tiaEligibleItemsExist As Boolean,
                                   ByRef samplesCount As Integer, ByRef turnInBatchIDs As List(Of Integer),
                                   ByRef invalidNoMerchISNs As StringBuilder, ByRef invalidVendorImageISNs As StringBuilder,
                                   ByVal isVendorImage As Boolean, ByVal recipients As List(Of String))
        Dim adInfo As Ad = Nothing
        Dim adsDetail As AdInfoInfo = Nothing
        Dim adNumber As Integer = 0
        Dim pageNumber As Integer = 0
        Dim turnInBatchID As Integer = 0
        Dim previousPageNumber As Integer = 0
        Dim ecommResultsBAO As EcommResults = Nothing
        Dim approvedItemDetails As List(Of EcommSetupCreateInfo) = Nothing
        Dim pageBAO As Page = Nothing
        Dim imageBAO As Image = Nothing
        Dim ecommSetupCreate As TUEcommSetupCreate = Nothing
        Dim merchandiseBAO As Merchandise = Nothing

        Try
            adInfo = New Ad()
            adsDetail = adInfo.GetTheLatestAdforTIA(isVendorImage)
            If Not adsDetail Is Nothing Then
                adNumber = adsDetail.adnbr
            End If

            If adNumber = 0 Then
                LogHelper.WriteToLogFile(LogEntryType.Warning, String.Format("No {0}Ad is found for this week.", IIf(isVendorImage, "Vendor Image ", String.Empty)))
                LogHelper.WriteToConsole(String.Format("No {0}Ad is found for this week.", IIf(isVendorImage, "Vendor Image ", String.Empty)))
                EmailHelper.Send(recipients, ConfigurationManager.AppSettings("FromEmailAddress"),
                                 String.Format("No {0}Ad is found for this week.", IIf(isVendorImage, "Vendor Image ", String.Empty)),
                                 String.Format("No {0}Ad is found for this week.  Please check Admin, setup the ads and run the TIA job.", IIf(isVendorImage, "Vendor Image ", String.Empty)),
                                 String.Empty)
                Exit Sub
            End If

            For Each department As DeptPageNumber In departments
                ecommResultsBAO = New EcommResults()

                If isVendorImage Then
                    approvedItemDetails = ecommResultsBAO.GetVendorImageISNs(startShipDate, department.DeptID, includeWebEligibleItemsOnly, dtVendorImages, invalidVendorImageISNs)
                Else
                    approvedItemDetails = ecommResultsBAO.GetApprovedItemsWithPO(startShipDate, department.DeptID, includeWebEligibleItemsOnly, dtNoMerchImages, invalidNoMerchISNs)
                End If

                If Not approvedItemDetails Is Nothing AndAlso approvedItemDetails.Count > 0 Then

                    approvedItemDetails = approvedItemDetails.GroupBy(Function(a) a.ISN).Select(Function(b) b.First()).ToList()

                    imageBAO = New Image()
                    ecommSetupCreate = New TUEcommSetupCreate()
                    merchandiseBAO = New Merchandise()
                    pageBAO = New Page()

                    'A separate batch should be created for every combination of an Ad number and a page number, so sort the list by page nuumber
                    approvedItemDetails.OrderBy(Function(a) a.PageNumber).ThenBy(Function(b) b.VendorStyleNumber)

                    'Page Number
                    pageNumber = pageBAO.GetPageNumberConfigByDeptID(department.DeptID)

                    'A separate batch should be created for every combination of an Ad number and a page number, so compare the page number 
                    'with previous page number and set the batch id as zero if they are different
                    If previousPageNumber = 0 Then
                        previousPageNumber = pageNumber
                    ElseIf previousPageNumber <> pageNumber Then
                        turnInBatchID = 0
                        previousPageNumber = pageNumber
                    End If

                    LogHelper.WriteToLogFile(LogEntryType.Information, String.Format("{0} ISNs have been selected for processing for the Dept {1}.", approvedItemDetails.Count.ToString(), department.DeptID.ToString()))

                    'Turn in the non vendor image items that are eligible
                    CompleteTurnIn(approvedItemDetails, startShipDate, includeWebEligibleItemsOnly, adNumber, pageNumber, dtNoMerchImages, dtVendorImages, tiaEligibleItemsExist,
                                   turnInBatchID, samplesCount, turnInBatchIDs, invalidNoMerchISNs, invalidVendorImageISNs)

                End If
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

End Module
