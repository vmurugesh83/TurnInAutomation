﻿Imports DocumentFormat.OpenXml.Spreadsheet
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml
Imports PathToSiteReports.BusinessEntities
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Reflection
Imports PathToSiteReports.MainframeDAL

Public Class ExcelHelper
#Region "SKU Details Report"
    Public Shared Sub ExportSKUDetailsReportToExcel(ByVal excelFileName As String, ByVal pageTitle As String,
                                                    ByVal SKUDetails As List(Of SKUDetail), ByVal sampleDueDateDays As Integer)
        Dim workbookPart As WorkbookPart = Nothing
        Dim worksheetPart As WorksheetPart = Nothing
        Dim stylesheetPart As WorkbookStylesPart = Nothing
        Dim workbook As Workbook = Nothing
        Dim sheets As Sheets = Nothing
        Dim sheet As Sheet = Nothing
        Dim worksheet As Worksheet = Nothing
        Dim sheetData As SheetData = Nothing
        Dim excelRow As Row = Nothing
        Dim lastMonthActiveSKUs As List(Of SKUDetail) = Nothing
        Dim pathToSiteDays As PathToSiteTiming = Nothing

        Try
            Using excelPackage As SpreadsheetDocument = SpreadsheetDocument.Create(excelFileName, SpreadsheetDocumentType.Workbook)

                workbookPart = excelPackage.AddWorkbookPart()

                workbook = New Workbook()
                workbook.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships")

                sheets = New Sheets()
                sheet = New Sheet With {.Name = "Detail Report", .SheetId = 1, .Id = "r1"}

                sheets.Append(sheet)
                workbook.Append(sheets)
                workbookPart.Workbook = workbook

                worksheetPart = workbookPart.AddNewPart(Of WorksheetPart)("r1")

                worksheet = New Worksheet()
                sheetData = New SheetData()

                'Add Style Sheet
                stylesheetPart = workbookPart.AddNewPart(Of WorkbookStylesPart)()
                stylesheetPart.Stylesheet = GenerateStyleSheet()
                stylesheetPart.Stylesheet.Save()

                WriteSKUDetailsListToExcelWorksheet(SKUDetails, worksheetPart, worksheet, sheetData, sampleDueDateDays)

            End Using
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Shared Sub ExportPathToSiteTimingReportToExcel(ByVal excelFileName As String, ByVal pageTitle As String,
                                                    ByVal SKUDetails As List(Of SKUDetail),
                                                    ByVal photoShootDays As Integer,
                                                    ByVal imageProductionDays As Integer)
        Dim workbookPart As WorkbookPart = Nothing
        Dim worksheetPart As WorksheetPart = Nothing
        Dim stylesheetPart As WorkbookStylesPart = Nothing
        Dim workbook As Workbook = Nothing
        Dim sheets As Sheets = Nothing
        Dim sheet As Sheet = Nothing
        Dim worksheet As Worksheet = Nothing
        Dim sheetData As SheetData = Nothing
        Dim excelRow As Row = Nothing
        Dim lastMonthActiveSKUs As List(Of SKUDetail) = Nothing
        Dim pathToSiteDays As PathToSiteTiming = Nothing

        Try
            lastMonthActiveSKUs = GetSKUsBecameActiveLastMonth(SKUDetails)
            If Not lastMonthActiveSKUs Is Nothing AndAlso lastMonthActiveSKUs.Count > 0 Then
                Using excelPackage As SpreadsheetDocument = SpreadsheetDocument.Create(excelFileName, SpreadsheetDocumentType.Workbook)

                    workbookPart = excelPackage.AddWorkbookPart()

                    workbook = New Workbook()
                    workbook.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships")


                    lastMonthActiveSKUs = lastMonthActiveSKUs.OrderBy(Function(a) _
                                                                          a.GMM).ThenBy(Function(b) _
                                                                                            b.DMM).ThenBy(Function(c) _
                                                                                                              c.Buyer).ThenBy(Function(d) _
                                                                                                                                  d.Dept).ToList()
                    sheets = New Sheets()

                    sheet = New Sheet With {.Name = "Summary Stage Perf Rept ", .SheetId = 1, .Id = "r1"}
                    sheets.Append(sheet)

                    worksheetPart = workbookPart.AddNewPart(Of WorksheetPart)("r1")

                    worksheet = New Worksheet()
                    sheetData = New SheetData()

                    'Add Style Sheet
                    stylesheetPart = workbookPart.AddNewPart(Of WorkbookStylesPart)()
                    stylesheetPart.Stylesheet = GenerateStyleSheet()
                    stylesheetPart.Stylesheet.Save()

                    WritePerfSummaryListToExcelWorksheet(lastMonthActiveSKUs, worksheetPart, worksheet, sheetData)

                    pathToSiteDays = GetPathToSiteExpectedDays()

                    sheet = New Sheet With {.Name = "Summary Stage Exception Rept", .SheetId = 2, .Id = "r2"}
                    sheets.Append(sheet)

                    worksheetPart = workbookPart.AddNewPart(Of WorksheetPart)("r2")

                    worksheet = New Worksheet()
                    sheetData = New SheetData()

                    WritePerfExceptionListToExcelWorksheet(lastMonthActiveSKUs, worksheetPart, worksheet,
                                                           sheetData, pathToSiteDays, imageProductionDays)


                    sheet = New Sheet With {.Name = "Path To Site Expected Time", .SheetId = 3, .Id = "r3"}
                    sheets.Append(sheet)

                    worksheetPart = workbookPart.AddNewPart(Of WorksheetPart)("r3")

                    worksheet = New Worksheet()
                    sheetData = New SheetData()

                    WritePathToSiteExpctedTimeListToExcelWorksheet(pathToSiteDays, worksheetPart, worksheet, sheetData)

                    sheet = New Sheet With {.Name = "Summary First Recpt Perf Rpt", .SheetId = 4, .Id = "r4"}
                    sheets.Append(sheet)
                    workbook.Append(sheets)
                    workbookPart.Workbook = workbook

                    worksheetPart = workbookPart.AddNewPart(Of WorksheetPart)("r4")

                    worksheet = New Worksheet()
                    sheetData = New SheetData()

                    WriteFirstReceiptPerformanceListToExcelWorksheet(lastMonthActiveSKUs, worksheetPart, worksheet, sheetData)
                End Using
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Shared Sub WriteSKUDetailsListToExcelWorksheet(ByVal SKUDetails As List(Of SKUDetail),
                                                           ByVal worksheetPart As WorksheetPart,
                                                           ByVal worksheet As Worksheet,
                                                           ByVal sheetData As SheetData,
                                                           ByVal sampleDueDateDays As Integer)

        Dim cellValue As String = String.Empty
        Dim columnIndex As Integer = 0

        '  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
        '
        '  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
        '  cells of data, we'll know if to write Text values or Numeric cell values.
        Dim numberOfColumns As Integer = 25
        Dim IsNumericColumn(numberOfColumns) As Boolean

        Dim excelColumnNames([numberOfColumns]) As String

        For n As Integer = 0 To numberOfColumns
            excelColumnNames(numberOfColumns) = GetExcelColumnName(n)
        Next n

        '
        '  Create the Header row in our Excel Worksheet
        '
        Dim rowIndex As UInt32 = 1


        'Append Headers
        Dim headerRow As Row = New Row
        headerRow.RowIndex = rowIndex            ' add a row at the top of spreadsheet
        headerRow = GetSKUDetailsReportHeaders()
        sheetData.Append(headerRow)

        '
        '  Now, step through each row of data in our DataTable...
        '
        Dim cellNumericValue As Double = 0

        If Not SKUDetails Is Nothing AndAlso SKUDetails.Count > 0 Then

            For Each item As SKUDetail In SKUDetails
                ' ...create a new row, and append a set of this row's data to it.
                rowIndex = rowIndex + 1
                Dim newExcelRow As New Row
                newExcelRow.RowIndex = rowIndex         '  add a row at the top of spreadsheet
                sheetData.Append(newExcelRow)

                columnIndex = 0
                'GMM
                cellValue = item.GMM
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'DMM
                cellValue = item.DMM
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Buyer
                cellValue = item.Buyer
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Department
                columnIndex = columnIndex + 1
                cellValue = item.Dept
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Class
                columnIndex = columnIndex + 1
                cellValue = item.Dept_Class
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'ISN
                columnIndex = columnIndex + 1
                cellValue = item.ISN
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, 7)

                'Color
                columnIndex = columnIndex + 1
                cellValue = item.Color
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'SKU
                columnIndex = columnIndex + 1
                cellValue = item.SKU
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, 7)

                'UPC
                columnIndex = columnIndex + 1
                cellValue = item.UPC
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, 7)

                'SKU Active Date
                columnIndex = columnIndex + 1
                cellValue = If(item.SKUActiveDate <= Date.MinValue, "", item.SKUActiveDate.ToShortDateString())
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, 7)

                'Ad Details
                columnIndex = columnIndex + 1
                cellValue = If(item.AdNumber > 0 _
                               AndAlso Not String.IsNullOrEmpty(item.AdDesc),
                               String.Concat(item.AdNumber, " - ", item.AdDesc), String.Empty)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'AD Type
                columnIndex = columnIndex + 1
                cellValue = item.AdType
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Style SKU Setup date
                columnIndex = columnIndex + 1
                cellValue = item.SSSetupDate
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Sample Request DAte
                columnIndex = columnIndex + 1
                cellValue = If(item.SampleRequestDate <= Date.MinValue, "", item.SampleRequestDate.ToShortDateString())
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Sample Receipt Date
                columnIndex = columnIndex + 1
                cellValue = If(item.SampleReceiptDate <= Date.MinValue, "", item.SampleReceiptDate.ToShortDateString())
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Sample Due Date - 8 or 12 weeks prior to the po ship date, configured in the web.config
                columnIndex = columnIndex + 1
                cellValue = If(item.POShipDate <= Date.MinValue OrElse item.SampleRequestDate <= Date.MinValue, "",
                               item.POShipDate.AddDays(sampleDueDateDays * -1).ToShortDateString())
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'PO Approval DAte
                columnIndex = columnIndex + 1
                cellValue = If(item.POApprovalDate <= Date.MinValue, "", item.POApprovalDate.ToShortDateString())
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'PO Ship date
                columnIndex = columnIndex + 1
                cellValue = If(item.POShipDate <= Date.MinValue, "", item.POShipDate.ToShortDateString())
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Turn in date
                columnIndex = columnIndex + 1
                cellValue = If(item.TurnInDate <= Date.MinValue, "", item.TurnInDate.ToShortDateString())
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Image Shot DAte
                columnIndex = columnIndex + 1
                cellValue = If(item.ImageShotDate <= Date.MinValue, "", item.ImageShotDate.ToShortDateString())
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Final Image Ready DAte
                columnIndex = columnIndex + 1
                cellValue = If(item.FinalImageReadyDate <= Date.MinValue, "", item.FinalImageReadyDate.ToShortDateString())
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'PO Receipt date
                columnIndex = columnIndex + 1
                cellValue = If(item.POReceiptDate <= Date.MinValue, "", item.POReceiptDate.ToShortDateString())
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Copy Ready Date
                columnIndex = columnIndex + 1
                cellValue = If(item.CopyReadyDate <= Date.MinValue, "", item.CopyReadyDate.ToShortDateString())
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Product REady date
                columnIndex = columnIndex + 1
                cellValue = If(item.ProductReadyDate <= Date.MinValue, "", item.ProductReadyDate.ToShortDateString())
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Product Active date
                columnIndex = columnIndex + 1
                cellValue = If(item.ProductActiveDate <= Date.MinValue, "", item.ProductActiveDate.ToShortDateString())
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

            Next
        End If

        worksheet.Append(sheetData)
        worksheetPart.Worksheet = worksheet

    End Sub
    Private Shared Function GetSKUDetailsReportHeaders() As Row
        Dim excelRow As Row = Nothing
        Dim excelCell As Cell = Nothing
        Dim headerString As InlineString = Nothing
        Dim cellText As InlineString = Nothing
        Dim cellValue As CellValue = Nothing
        Dim runProp As RunProperties = Nothing
        Dim runCell As Run = Nothing
        Dim columnCount As Integer = 0
        excelRow = New Row()

        columnCount += 1
        excelRow.Append(AddHeaderColumn("GMM", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("DMM", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Buyer", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Department", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Class", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("ISN", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Color Code", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("SKU", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("UPC", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("SKU Active Date", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("AD#", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Ad Type", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("SS Setup Date", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Sample Request Date", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Sample Receipt Date", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Sample Due Date", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("PO Approval Date", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("PO Start Ship", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Turn in Date", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Image Shot Date", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Final Image Ready Date", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("PO 1st Receipt Date", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Copy Ready Date", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Product Ready Date", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Product Active Date (1st Date)", GetExcelColumnName(columnCount)))

        Return excelRow
    End Function
#End Region

#Region "Stage Perf Report Summary"
    Private Shared Sub WritePerfSummaryListToExcelWorksheet(ByVal SKUDetails As List(Of SKUDetail), ByVal worksheetPart As WorksheetPart,
                                                 ByVal worksheet As Worksheet, ByVal sheetData As SheetData,
                                                 Optional ByVal shouldFormatAsError As Boolean = False,
                                                 Optional ByVal shouldAddExceptionType As Boolean = False)


        Dim cellValue As String = String.Empty
        Dim columnIndex As Integer = 0
        Dim styleIndex As Integer = 0
        '  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
        '
        '  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
        '  cells of data, we'll know if to write Text values or Numeric cell values.
        Dim numberOfColumns As Integer = 18
        Dim IsNumericColumn(numberOfColumns) As Boolean

        Dim excelColumnNames([numberOfColumns]) As String

        For n As Integer = 0 To numberOfColumns
            excelColumnNames(numberOfColumns) = GetExcelColumnName(n)
        Next n

        Dim outputList = (From detail In SKUDetails
                         Group By detail.GMM, detail.DMM, detail.Buyer, detail.Dept, detail.AdType, detail.ExceptionType
                         Into SSSetupToSampleRequest = Average(detail.SSSetupToSampleRequest),
                             SampleRequestToSampleReceipt = Average(detail.SampleRequestToSampleReceipt),
                             SSSetupToPOApproval = Average(detail.SSSetupToPOApproval),
                             SSSetupToTurnIn = Average(detail.SSSetupToTurnIn),
                             POApprovalToTurnIn = Average(detail.POApprovalToTurnIn),
                             SampleReceiptToTurnin = Average(detail.SampleReceiptToTurnIn),
                             TurnInToImageShot = Average(detail.TurnInToImageShot),
                             ImageShotToImageReady = Average(detail.ImageShotToImageReady),
                             TotalPhotoTime = Average(detail.TurnInToImageReady),
                             TurnInToFirstPOReceipt = Average(detail.TurnInToPOReceipt),
                             TurnInToReadyStatus = Average(detail.TurnInToProductReady),
                             TurnInToCopyReady = Average(detail.TurnInToCopyReady),
                             TurnInToProductActive = Average(detail.TurnInToProductActive)
                         Select GMM, DMM, Buyer, Dept, AdType, ExceptionType,
                         SSSetupToSampleRequest,
                         SampleRequestToSampleReceipt,
                         SSSetupToPOApproval,
                         SSSetupToTurnIn,
                         POApprovalToTurnIn,
                         SampleReceiptToTurnin,
                         TurnInToImageShot,
                         ImageShotToImageReady,
                         TotalPhotoTime,
                         TurnInToFirstPOReceipt,
                         TurnInToReadyStatus,
                         TurnInToCopyReady,
                         TurnInToProductActive).ToList()

        '
        '  Create the Header row in our Excel Worksheet
        '
        Dim rowIndex As UInt32 = 1

        'Append Title
        Dim titleRow As Row = New Row
        titleRow.RowIndex = rowIndex            ' add the title at the top of spreadsheet
        titleRow = GetStagePerfReportTitle()
        sheetData.Append(titleRow)

        rowIndex = rowIndex + 1
        'Append Headers
        Dim headerRow As Row = New Row
        headerRow.RowIndex = rowIndex            ' add the headers at the top of spreadsheet
        headerRow = GetStagePerfReportHeaders(shouldAddExceptionType)
        sheetData.Append(headerRow)

        '
        '  Now, step through each row of data in our DataTable...
        '
        Dim cellNumericValue As Double = 0

        If shouldFormatAsError Then
            styleIndex = 3
        End If

        If Not outputList Is Nothing AndAlso outputList.Count > 0 Then

            For Each item In outputList
                ' ...create a new row, and append a set of this row's data to it.
                rowIndex = rowIndex + 1
                Dim newExcelRow As New Row
                newExcelRow.RowIndex = rowIndex         '  add a row at the top of spreadsheet
                sheetData.Append(newExcelRow)

                columnIndex = 0

                'GMM
                cellValue = item.GMM
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String, styleIndex)

                'DMM
                columnIndex = columnIndex + 1
                cellValue = item.DMM
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String, styleIndex)

                'Buyer
                columnIndex = columnIndex + 1
                cellValue = item.Buyer
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String, styleIndex)

                'Department
                columnIndex = columnIndex + 1
                cellValue = item.Dept
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String, styleIndex)

                'Ad Type
                columnIndex = columnIndex + 1
                cellValue = item.AdType
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String, styleIndex)

                If shouldAddExceptionType Then
                    'Exception Type
                    cellValue = item.ExceptionType
                    AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String, styleIndex)
                    columnIndex = columnIndex + 1
                End If

                'SS Setup to Sample Request
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(item.SSSetupToSampleRequest)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, styleIndex)

                'Sample Request To Sample Receipt
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(item.SampleRequestToSampleReceipt)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, styleIndex)

                'Style SKU Setup To PO Approval
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(item.SSSetupToPOApproval)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, styleIndex)

                'Style SKU Setup To Turn-In
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(item.SSSetupToTurnIn)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, styleIndex)

                'PO Approval To Turn-In
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(item.POApprovalToTurnIn)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, styleIndex)

                'Sample Receipt To Turn-IN
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(item.SampleReceiptToTurnin)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, styleIndex)

                'Turn in date to Image Shot days
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(item.TurnInToImageShot)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, styleIndex)

                'Image shot to Final Image ready(premedia) days
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(item.ImageShotToImageReady)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, styleIndex)

                'Total Photo Time
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(item.TotalPhotoTime)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, styleIndex)

                'Turn-In To First PO Receipt
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(item.TurnInToFirstPOReceipt)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, styleIndex)

                'Turn-In To Copy Ready
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(item.TurnInToCopyReady)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, styleIndex)

                'Turn-In To Ready Status
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(item.TurnInToReadyStatus)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, styleIndex)

                'Turn-In To Product Active
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(item.TurnInToProductActive)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, styleIndex)
            Next
        End If

        worksheet.Append(sheetData)
        worksheetPart.Worksheet = worksheet

        MergeTwoCells(worksheet, "A1", "C1")
    End Sub

    Private Shared Function GetStagePerfReportHeaders(Optional ByVal shouldAddExceptionType As Boolean = False) As Row
        Dim excelRow As Row = Nothing
        Dim excelCell As Cell = Nothing
        Dim headerString As InlineString = Nothing
        Dim cellText As InlineString = Nothing
        Dim cellValue As CellValue = Nothing
        Dim runProp As RunProperties = Nothing
        Dim runCell As Run = Nothing
        Dim columnCount As Integer = 0
        excelRow = New Row()

        columnCount += 1
        excelRow.Append(AddHeaderColumn("GMM", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("DMM", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Buyer", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Department", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("AD Type", GetExcelColumnName(columnCount)))

        If shouldAddExceptionType Then
            columnCount += 1
            excelRow.Append(AddHeaderColumn("Exception Type", GetExcelColumnName(columnCount)))
        End If

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Average of SS Setup to Sample request days", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Average of Sample request to Sample Receipt Days", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Average of SS setup to PO Approval Days", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Average of SS Setup to Turn in Days", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Average of PO Approval to Turn in Days", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Average of Sample receipt to Turn in Days", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Average of Turn in to Image Shot days", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Average of Image shot to Final Image ready(premedia) days", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Average of Total Photo Time", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Average of Turn in to Date of first PO receipt", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Average of Turn in to Copy ready", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Average of Turn in to Ready status ( Ttl Patht to Site Time?)", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Average of Turn-in to Product Active status", GetExcelColumnName(columnCount)))

        Return excelRow
    End Function

    Private Shared Function GetStagePerfReportTitle() As Row
        Dim excelRow As Row = Nothing
        Dim excelCell As Cell = Nothing
        Dim columnCount As Integer = 0
        excelRow = New Row()

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Summary Stage Performance Metrics", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Run Date", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn(Date.Today().ToShortDateString(), GetExcelColumnName(columnCount)))

        Return excelRow
    End Function
#End Region

#Region "First Receipt Performance Report"
    Private Shared Sub WriteFirstReceiptPerformanceListToExcelWorksheet(ByVal SKUDetails As List(Of SKUDetail), ByVal worksheetPart As WorksheetPart,
                                                 ByVal worksheet As Worksheet, ByVal sheetData As SheetData)


        Dim cellValue As String = String.Empty
        Dim columnIndex As Integer = 0
        Dim overallSummaryAdded As Boolean = False
        '  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
        '
        '  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
        '  cells of data, we'll know if to write Text values or Numeric cell values.
        Dim numberOfColumns As Integer = 28
        Dim IsNumericColumn(numberOfColumns) As Boolean

        Dim excelColumnNames([numberOfColumns]) As String

        For n As Integer = 0 To numberOfColumns
            excelColumnNames(numberOfColumns) = GetExcelColumnName(n)
        Next n

        Dim outputList = (From detail In SKUDetails
                         Select detail.SSSetupToPOReceipt).Average()

        '
        '  Create the Header row in our Excel Worksheet
        '
        Dim rowIndex As UInt32 = 1

        'Append First Level Title
        Dim firstLevelTitleRow As Row = New Row
        firstLevelTitleRow.RowIndex = rowIndex            ' add the title at the top of spreadsheet
        firstLevelTitleRow = GetFirstReceiptFirstLevelReportTitle()
        sheetData.Append(firstLevelTitleRow)

        rowIndex = rowIndex + 1
        'Append Second Level Title
        Dim secondLevelTitleRow As Row = New Row
        secondLevelTitleRow.RowIndex = rowIndex            ' add the title at the top of spreadsheet
        secondLevelTitleRow = GetFirstReceiptSecondLevelReportTitle()
        sheetData.Append(secondLevelTitleRow)

        rowIndex = rowIndex + 1
        'Append Headers
        Dim headerRow As Row = New Row
        headerRow.RowIndex = rowIndex            ' add the headers at the top of spreadsheet
        headerRow = GetFirstReceiptReportHeaders()
        sheetData.Append(headerRow)

        '
        '  Now, step through each row of data in our DataTable...
        '
        Dim cellNumericValue As Double = 0

        If Not SKUDetails Is Nothing AndAlso SKUDetails.Count > 0 Then

            Dim firstReceiptOutputList = (From detail In SKUDetails
                             Group By detail.GMM, detail.DMM, detail.Buyer, detail.Dept
                             Into SSSetupToFirstReceipt = Average(detail.SSSetupToPOReceipt),
                                 SampleRequestToFirstReceipt = Average(detail.SampleRequestToPOReceipt),
                                 POApprovalToFirstReceipt = Average(detail.POApprovalToPOReceipt),
                                 SampleReceivedToFirstReceipt = Average(detail.SampleReceiptToPOReceipt),
                                 TurnInToFirstReceipt = Average(detail.TurnInToPOReceipt),
                                 ImageShotToFirstReceipt = Average(detail.ImageShotToPOReceipt),
                                 ImageReadyToFirstReceipt = Average(detail.ImageReadyToPOReceipt),
                                 CopyReadyToFirstReceipt = Average(detail.CopyReadyToPOReceipt),
                                 ProductReadyToFirstReceipt = Average(detail.ProductReadyToPOReceipt),
                                 StylesReadyAtFirstReceipt = Count(detail.ProductReadyDate <= detail.POReceiptDate),
                                 StylesReadyAfterFirstReceipt = Count(detail.ProductReadyDate > detail.POReceiptDate),
                                 SSSetupToPOApproval = Average(detail.SSSetupToPOApproval),
                                 POApprovalToTurnIn = Average(detail.POApprovalToTurnIn),
                                 TurnInToImageShot = Average(detail.TurnInToImageShot),
                                 ImageShotToImageReady = Average(detail.ImageShotToImageReady),
                                 TurnInToImageReady = Average(detail.TurnInToImageReady),
                                 TurnInToCopyReady = Average(detail.TurnInToCopyReady),
                                 ImageReadyToCopyReady = Average(detail.ImageReadyToCopyReady),
                                 FirstReceiptToProductReady = Average(detail.FirstReceiptToProductReady)
                             Select GMM, DMM, Buyer, Dept,
                             SSSetupToFirstReceipt,
                             SampleRequestToFirstReceipt,
                             POApprovalToFirstReceipt,
                             SampleReceivedToFirstReceipt,
                             TurnInToFirstReceipt,
                             ImageShotToFirstReceipt,
                             ImageReadyToFirstReceipt,
                             CopyReadyToFirstReceipt,
                             ProductReadyToFirstReceipt,
                             StylesReadyAtFirstReceipt,
                             StylesReadyAfterFirstReceipt,
                             SSSetupToPOApproval,
                             POApprovalToTurnIn,
                             TurnInToImageShot,
                             ImageShotToImageReady,
                             TurnInToImageReady,
                             TurnInToCopyReady,
                             ImageReadyToCopyReady,
                             FirstReceiptToProductReady).ToList()

            For Each firstReceiptItem In firstReceiptOutputList

                ' ...create a new row, and append a set of this row's data to it.
                rowIndex = rowIndex + 1
                Dim newExcelRow As New Row
                newExcelRow.RowIndex = rowIndex         '  add a row at the top of spreadsheet
                sheetData.Append(newExcelRow)

                columnIndex = 0
                'GMM
                cellValue = firstReceiptItem.GMM
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'DMM
                columnIndex = columnIndex + 1
                cellValue = firstReceiptItem.DMM
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Buyer
                columnIndex = columnIndex + 1
                cellValue = firstReceiptItem.Buyer
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Dept
                columnIndex = columnIndex + 1
                cellValue = firstReceiptItem.Dept
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'SS Setup to First Receipt
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.SSSetupToFirstReceipt)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Sample Request to First Receipt
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.SampleRequestToFirstReceipt)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'PO Approval to First Receipt
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.POApprovalToFirstReceipt)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Sample Received to First Receipt
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.SampleReceivedToFirstReceipt)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Turn in to First Receipt
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.TurnInToFirstReceipt)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Image Shot to First Receipt
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.ImageShotToFirstReceipt)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Image Ready to First Receipt
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.ImageReadyToFirstReceipt)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Copy Ready to First Receipt
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.CopyReadyToFirstReceipt)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Product Ready to First Receipt
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.ProductReadyToFirstReceipt)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'First Receipt Column
                columnIndex = columnIndex + 1
                cellValue = String.Empty
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, 12)

                'Count of Styles Ready at First Receipt
                columnIndex = columnIndex + 1
                cellValue = firstReceiptItem.StylesReadyAtFirstReceipt
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Count of Styles Ready after First Receipt
                columnIndex = columnIndex + 1
                cellValue = firstReceiptItem.StylesReadyAfterFirstReceipt
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Days b/t Style/Sku & First Receipt
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.SSSetupToFirstReceipt)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Days b/t Sample Received & First Receipt
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.SampleReceivedToFirstReceipt)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Days b/t PO Approved & First Receipt
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.POApprovalToFirstReceipt)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Days b/t Turn In & First Receipt
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.TurnInToFirstReceipt)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Days b/t Style SKU & PO Approved
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.SSSetupToPOApproval)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Days b/t PO Approved & Turn in
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.POApprovalToTurnIn)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Days b/t Turn in & Image Shot
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.TurnInToImageShot)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Days b/t Image Shot & Image Ready
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.ImageShotToImageReady)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Days b/t Turn In & Image Ready
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.TurnInToImageReady)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Days b/t Turn In & Copy Ready
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.TurnInToCopyReady)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Days b/t Image Ready & Copy Ready
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.ImageReadyToCopyReady)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

                'Days b/t First Receipt & Product Ready
                columnIndex = columnIndex + 1
                cellValue = System.Math.Round(firstReceiptItem.FirstReceiptToProductReady)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)
            Next
        End If


        'Append totals at the bottom
        rowIndex = rowIndex + 1
        Dim totalByGMMRow As New Row
        totalByGMMRow.RowIndex = rowIndex         '  add a row at the top of spreadsheet
        sheetData.Append(totalByGMMRow)

        'columnIndex = 0
        ''Totals by GMM
        'cellValue = "Totals by GMM"
        'AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, totalByGMMRow, CellValues.String)

        'If Not overallSummaryAdded Then
        '    'First Receipt Column
        '    columnIndex = columnIndex + 1
        '    cellValue = String.Empty
        '    AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, totalByGMMRow, CellValues.Number)

        '    'Count of Styles Ready at First Receipt
        '    columnIndex = columnIndex + 1
        '    cellValue = (From detail In SKUDetails
        '                 Where detail.ProductReadyDate <= detail.POReceiptDate
        '                 Select detail.ISN).Count()
        '    AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, totalByGMMRow, CellValues.Number)

        '    'Count of Styles Ready after First Receipt
        '    columnIndex = columnIndex + 1
        '    cellValue = (From detail In SKUDetails
        '                 Where detail.ProductReadyDate > detail.POReceiptDate
        '                 Select detail.ISN).Count()
        '    AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, totalByGMMRow, CellValues.Number)

        '    'Days b/t Style/Sku & First Receipt
        '    columnIndex = columnIndex + 1
        '    cellValue = System.Math.Round((From detail In SKUDetails
        '                 Select detail.SSSetupToPOReceipt).Average())
        '    AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, totalByGMMRow, CellValues.Number)

        '    'Days b/t Sample Received & First Receipt
        '    columnIndex = columnIndex + 1
        '    cellValue = System.Math.Round((From detail In SKUDetails
        '                 Select detail.SampleReceiptToPOReceipt).Average())
        '    AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, totalByGMMRow, CellValues.Number)

        '    'Days b/t PO Approved & First Receipt
        '    columnIndex = columnIndex + 1
        '    cellValue = System.Math.Round((From detail In SKUDetails
        '                 Select detail.POApprovalToPOReceipt).Average())
        '    AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, totalByGMMRow, CellValues.Number)

        '    'Days b/t Turn In & First Receipt
        '    columnIndex = columnIndex + 1
        '    cellValue = System.Math.Round((From detail In SKUDetails
        '                 Select detail.TurnInToPOReceipt).Average())
        '    AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, totalByGMMRow, CellValues.Number)

        '    'Days b/t Style SKU & PO Approved
        '    columnIndex = columnIndex + 1
        '    cellValue = System.Math.Round((From detail In SKUDetails
        '                 Select detail.SSSetupToPOApproval).Average())
        '    AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, totalByGMMRow, CellValues.Number)

        '    'Days b/t PO Approved & Turn in
        '    columnIndex = columnIndex + 1
        '    cellValue = System.Math.Round((From detail In SKUDetails
        '                 Select detail.POApprovalToTurnIn).Average())
        '    AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, totalByGMMRow, CellValues.Number)

        '    'Days b/t Turn in & Image Shot
        '    columnIndex = columnIndex + 1
        '    cellValue = System.Math.Round((From detail In SKUDetails
        '                 Select detail.TurnInToImageShot).Average())
        '    AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, totalByGMMRow, CellValues.Number)

        '    'Days b/t Image Shot & Image Ready
        '    columnIndex = columnIndex + 1
        '    cellValue = System.Math.Round((From detail In SKUDetails
        '     Select detail.ImageShotToImageReady).Average())
        '    AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, totalByGMMRow, CellValues.Number)

        '    'Days b/t Turn In & Image Ready
        '    columnIndex = columnIndex + 1
        '    cellValue = System.Math.Round((From detail In SKUDetails
        '                 Select detail.TurnInToImageReady).Average())
        '    AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, totalByGMMRow, CellValues.Number)

        '    'Days b/t Turn In & Copy Ready
        '    columnIndex = columnIndex + 1
        '    cellValue = System.Math.Round((From detail In SKUDetails
        '                 Select detail.TurnInToCopyReady).Average())
        '    AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, totalByGMMRow, CellValues.Number)

        '    'Days b/t Image Ready & Copy Ready
        '    columnIndex = columnIndex + 1
        '    cellValue = System.Math.Round((From detail In SKUDetails
        '                 Select detail.ImageReadyToCopyReady).Average())
        '    AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, totalByGMMRow, CellValues.Number)

        '    'Days b/t First Receipt & Product Ready
        '    columnIndex = columnIndex + 1
        '    cellValue = System.Math.Round((From detail In SKUDetails
        '                 Select detail.FirstReceiptToProductReady).Average())
        '    AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, totalByGMMRow, CellValues.Number)

        '    overallSummaryAdded = True
        'End If

        rowIndex = rowIndex + 1
        Dim totalByADType As New Row
        totalByADType.RowIndex = rowIndex         '  add a row at the top of spreadsheet
        sheetData.Append(totalByADType)

        ''Totals by AD types
        'columnIndex = 0
        'cellValue = "Totals by AD types all GMMS"
        'AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, totalByADType, CellValues.String)

        worksheet.Append(sheetData)
        worksheetPart.Worksheet = worksheet

        MergeTwoCells(worksheet, "A1", "AB1")
        MergeTwoCells(worksheet, "A2", "N2")
        MergeTwoCells(worksheet, "O2", "AB2")

    End Sub

    Private Shared Function GetFirstReceiptReportHeaders() As Row
        Dim excelRow As Row = Nothing
        Dim excelCell As Cell = Nothing
        Dim headerString As InlineString = Nothing
        Dim cellText As InlineString = Nothing
        Dim cellValue As CellValue = Nothing
        Dim runProp As RunProperties = Nothing
        Dim runCell As Run = Nothing
        Dim columnCount As Integer = 0
        excelRow = New Row()

        columnCount += 1
        excelRow.Append(AddHeaderColumn("GMM", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("DMM", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Buyer", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Dept", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Style Sku Setup", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Sample Request", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("PO Approval", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Sample/Image Received", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Turn In", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Image Shot (studio)", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Image Ready (premedia)", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Copy Complete", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Set to Ready", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("First Receipt", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Count of Styles Ready at First Receipt", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Count of Styles Ready after First Receipt", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Days b/t Style/Sku & First Receipt", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Days b/t Sample Rcvd & First Receipt", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Days b/t PO Approved & First Receipt", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Days b/t Turn In & First Receipt", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Days b/t Style/Sku & PO Approved", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Days b/t PO Approved & Turn In", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Days b/t Turn In & Image (studio)", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Days b/t Image (studio) & Image (premedia)", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Days b/t Turn In & Image (premedia)", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Days b/t Turn in & Copy", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Days b/t Image ready & Copy", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Days b/t First Rcpt & Ready", GetExcelColumnName(columnCount)))

        Return excelRow
    End Function

    Private Shared Function GetFirstReceiptFirstLevelReportTitle() As Row
        Dim excelRow As Row = Nothing
        Dim excelCell As Cell = Nothing
        Dim columnCount As Integer = 0
        excelRow = New Row()

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Summary First Receipt Performance Report", GetExcelColumnName(columnCount)))

        Return excelRow
    End Function

    Private Shared Function GetFirstReceiptSecondLevelReportTitle() As Row
        Dim excelRow As Row = Nothing
        Dim excelCell As Cell = Nothing
        Dim columnCount As Integer = 0
        excelRow = New Row()

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Days back from first receipt (average)", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Performance Measure (overall)", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("", GetExcelColumnName(columnCount)))

        Return excelRow
    End Function
#End Region

#Region "Common Functions"

    Private Shared Function GetExcelColumnName(ByVal columnIndex As Integer) As String
        If (columnIndex < 26) Then
            Return Chr(Asc("A") + columnIndex)
        End If

        Dim firstChar As Char,
            secondChar As Char

        firstChar = Chr(Asc("A") + (columnIndex \ 26) - 1)
        secondChar = Chr(Asc("A") + (columnIndex Mod 26))

        Return firstChar + secondChar
    End Function

    Private Shared Sub AppendCell(ByVal cellReference As String, ByVal cellStringValue As String, ByVal excelRow As Row,
                                  ByVal cellDataType As CellValues, Optional ByVal styleIndex As Integer = 0)
        '/  Add a new Excel Cell to our Row 
        Dim cell As New Cell
        cell.CellReference = cellReference
        cell.DataType = cellDataType
        If styleIndex > 0 Then
            cell.StyleIndex = styleIndex
        End If

        Dim cellValue As New CellValue
        cellValue.Text = cellStringValue

        cell.Append(cellValue)

        excelRow.Append(cell)
    End Sub

    Private Shared Function GenerateStyleSheet() As Stylesheet
        Dim workbookstylesheet As Stylesheet = Nothing

        Dim fonts As New Fonts(New Font(New FontSize() With {.Val = 10}),
                               New Font(New FontSize() With {.Val = 10},
                                        New Bold(), New Color() With {.Rgb = "FFFFFF"}
                                        ),
                                    New Font(New FontSize() With {.Val = 10}, New Color() With {.Rgb = "FF0000"}),
                                    New Font(New FontSize() With {.Val = 10}, New Color() With {.Rgb = "000000"}))

        ' Index 0 - default
        ' Index 1 - Gray
        ' Index 2 - header
        ' Index 3 - Excel Bad - Conditional Formatting
        ' Index 4 - 40% accent 4 - Excel Conditional Formatting
        ' Index 5 - 40% accent 6 - Excel Conditional Formatting
        ' Index 6 - Yellow Color
        ' Index 7 - Previous week turn-in ads
        ' Index 8 - Current week turn-in ads
        Dim fills As New Fills(New Fill(New PatternFill() With {.PatternType = PatternValues.None}),
                               New Fill(New PatternFill() With {.PatternType = PatternValues.Solid, .ForegroundColor = New ForegroundColor() With {.Rgb = "696969"}}),
                               New Fill(New PatternFill() With {.PatternType = PatternValues.None}),
                               New Fill(New PatternFill() With {.PatternType = PatternValues.Solid, .BackgroundColor = New BackgroundColor() With {.Rgb = "FFC7CE"},
                                                                .ForegroundColor = New ForegroundColor() With {.Rgb = "FFC7CE"}}),
                               New Fill(New PatternFill() With {.PatternType = PatternValues.Solid, .ForegroundColor = New ForegroundColor() With {.Rgb = "FFE699"}}),
                               New Fill(New PatternFill() With {.PatternType = PatternValues.Solid, .ForegroundColor = New ForegroundColor() With {.Rgb = "C6E0B4"}}),
                               New Fill(New PatternFill() With {.PatternType = PatternValues.Solid, .ForegroundColor = New ForegroundColor() With {.Rgb = "FFFF00"}}),
                               New Fill(New PatternFill() With {.PatternType = PatternValues.Solid, .ForegroundColor = New ForegroundColor() With {.Rgb = "FFF2CC"}}),
                               New Fill(New PatternFill() With {.PatternType = PatternValues.Solid, .ForegroundColor = New ForegroundColor() With {.Rgb = "E2EFDA"}}))

        ' index 0 default
        ' index 1 black border
        Dim borders As New Borders(New Border(),
                                   New Border(New LeftBorder(New Color() With {.Auto = True}) With {.Style = BorderStyleValues.Thin},
                                              New RightBorder(New Color() With {.Auto = True}) With {.Style = BorderStyleValues.Thin},
                                              New TopBorder(New Color() With {.Auto = True}) With {.Style = BorderStyleValues.Thin},
                                              New BottomBorder(New Color() With {.Auto = True}) With {.Style = BorderStyleValues.Thin},
                                              New DiagonalBorder()))

        ' default
        Dim cellFormats As New CellFormats(New CellFormat())

        ' body
        Dim cellFormat1 As New CellFormat() With {
            .FontId = 0,
            .FillId = 0,
            .BorderId = 1,
            .ApplyBorder = True}

        'header
        Dim cellFormat2 As New CellFormat() With {
            .FontId = 1,
            .FillId = 2,
            .BorderId = 1,
            .ApplyFill = True}

        ' custom - index 3
        Dim cellFormat3 As New CellFormat() With {
            .FontId = 2,
            .FillId = 2,
            .BorderId = 1,
            .ApplyBorder = True,
            .ApplyFill = True}

        'header With Fill id 4
        Dim cellFormat4 As New CellFormat() With {
            .FontId = 1,
            .FillId = 4,
            .BorderId = 1,
            .ApplyFill = True,
            .Alignment = New Alignment() With {.WrapText = True, .Horizontal = HorizontalAlignmentValues.Center}}

        Dim cellFormat5 As New CellFormat() With {
            .FontId = 1,
            .FillId = 5,
            .BorderId = 1,
            .ApplyFill = True,
            .Alignment = New Alignment() With {.WrapText = True, .Horizontal = HorizontalAlignmentValues.Center}}

        Dim cellFormat6 As New CellFormat() With {
            .FontId = 1,
            .FillId = 6,
            .BorderId = 1,
            .ApplyFill = True,
            .Alignment = New Alignment() With {.WrapText = True, .Horizontal = HorizontalAlignmentValues.Center}}

        cellFormat2.AppendChild(New Alignment With {.WrapText = True, .Horizontal = HorizontalAlignmentValues.Center})

        ' long numbers
        Dim cellFormat7 As New CellFormat() With {
            .FontId = 0,
            .FillId = 0,
            .BorderId = 0,
            .ApplyNumberFormat = True,
            .NumberFormatId = 1}

        ' long numbers
        Dim cellFormat8 As New CellFormat() With {
            .FontId = 0,
            .FillId = 0,
            .BorderId = 0,
            .ApplyNumberFormat = True,
            .NumberFormatId = 14
            }

        'Previous week's turn-in ads
        Dim cellFormat9 As New CellFormat() With {
            .FontId = 0,
            .FillId = 7,
            .BorderId = 0,
            .ApplyFill = True}

        'Current turn-in week ads
        Dim cellFormat10 As New CellFormat() With {
            .FontId = 0,
            .FillId = 8,
            .BorderId = 0,
            .ApplyFill = True}

        'Future turn-in week ads
        Dim cellFormat11 As New CellFormat() With {
            .FontId = 0,
            .FillId = 5,
            .BorderId = 0,
            .ApplyFill = True}

        ' Gray fill
        Dim cellFormat12 As New CellFormat() With {
        .FillId = 1,
        .ApplyFill = True,
        .FontId = 0
            }

        cellFormats.AppendChild(cellFormat1)

        cellFormats.AppendChild(cellFormat2)

        cellFormats.AppendChild(cellFormat3)

        cellFormats.AppendChild(cellFormat4)

        cellFormats.AppendChild(cellFormat5)

        cellFormats.AppendChild(cellFormat6)

        cellFormats.AppendChild(cellFormat7)

        cellFormats.AppendChild(cellFormat8)

        cellFormats.AppendChild(cellFormat9)

        cellFormats.AppendChild(cellFormat10)

        cellFormats.AppendChild(cellFormat11)

        cellFormats.AppendChild(cellFormat12)

        workbookstylesheet = New Stylesheet(fonts, fills, borders, cellFormats)
        Return workbookstylesheet

    End Function

    Private Shared Function AddHeaderColumn(ByVal columnHeader As String, ByVal cellReference As StringValue, Optional ByVal styleIndex As Integer = 2) As Cell
        Dim excelCell As Cell = Nothing
        Dim cellText As InlineString = Nothing
        Dim cellValue As CellValue = Nothing
        Dim runCell As Run = Nothing

        excelCell = New Cell With {.CellReference = cellReference, .DataType = CellValues.InlineString, .StyleIndex = styleIndex}
        runCell = New Run()
        runCell.Append(New Text(columnHeader))
        runCell.RunProperties = New RunProperties(New Bold, New Underline)
        cellText = New InlineString()
        cellText.Append(runCell)
        excelCell.Append(cellText)

        Return excelCell
    End Function
    ' Given a worksheet name, and the names of two adjacent cells, merges the two cells.
    ' When two cells are merged, only the content from one cell is preserved:
    ' the upper-left cell for left-to-right languages or the upper-right cell for right-to-left languages.
    Private Shared Sub MergeTwoCells(ByVal docWorkSheet As Worksheet, ByVal cell1Name As String, ByVal cell2Name As String)
        Dim worksheet As Worksheet = docWorkSheet
        If ((worksheet Is Nothing) OrElse (String.IsNullOrEmpty(cell1Name) OrElse String.IsNullOrEmpty(cell2Name))) Then
            Return
        End If

        Dim mergeCells As MergeCells
        If (worksheet.Elements(Of MergeCells)().Count() > 0) Then
            mergeCells = worksheet.Elements(Of MergeCells).First()
        Else
            mergeCells = New MergeCells()

            ' Insert a MergeCells object into the specified position.
            If (worksheet.Elements(Of CustomSheetView)().Count() > 0) Then
                worksheet.InsertAfter(mergeCells, worksheet.Elements(Of CustomSheetView)().First())
            ElseIf (worksheet.Elements(Of DataConsolidate)().Count() > 0) Then
                worksheet.InsertAfter(mergeCells, worksheet.Elements(Of DataConsolidate)().First())
            ElseIf (worksheet.Elements(Of SortState)().Count() > 0) Then
                worksheet.InsertAfter(mergeCells, worksheet.Elements(Of SortState)().First())
            ElseIf (worksheet.Elements(Of AutoFilter)().Count() > 0) Then
                worksheet.InsertAfter(mergeCells, worksheet.Elements(Of AutoFilter)().First())
            ElseIf (worksheet.Elements(Of Scenarios)().Count() > 0) Then
                worksheet.InsertAfter(mergeCells, worksheet.Elements(Of Scenarios)().First())
            ElseIf (worksheet.Elements(Of ProtectedRanges)().Count() > 0) Then
                worksheet.InsertAfter(mergeCells, worksheet.Elements(Of ProtectedRanges)().First())
            ElseIf (worksheet.Elements(Of SheetProtection)().Count() > 0) Then
                worksheet.InsertAfter(mergeCells, worksheet.Elements(Of SheetProtection)().First())
            ElseIf (worksheet.Elements(Of SheetCalculationProperties)().Count() > 0) Then
                worksheet.InsertAfter(mergeCells, worksheet.Elements(Of SheetCalculationProperties)().First())
            Else
                worksheet.InsertAfter(mergeCells, worksheet.Elements(Of SheetData)().First())
            End If
        End If

        ' Create the merged cell and append it to the MergeCells collection.
        Dim mergeCell As MergeCell = New MergeCell()
        mergeCell.Reference = New StringValue((cell1Name + (":" + cell2Name)))
        mergeCells.Append(mergeCell)

        worksheet.Save()
    End Sub

    Private Shared Function GetSKUsBecameActiveLastMonth(ByVal skuDetails As List(Of SKUDetail))
        Dim lastMonthActiveSKUs As List(Of SKUDetail) = Nothing
        Dim fiscalDataDAO As FiscalDataDAO = Nothing
        Dim fiscalYear As Integer = 0
        Dim fiscalMonth As Integer = 0
        Dim lastMonthStartDate As DateTime = Date.MinValue
        Dim lastMonthEndDate As DateTime = Date.MinValue

        fiscalDataDAO = New FiscalDataDAO()

        fiscalYear = fiscalDataDAO.GetFiscalYearByDate(Date.Today())
        fiscalMonth = fiscalDataDAO.GetFiscalMonthByDate(Date.Today())

        'Get Last Month
        If (fiscalMonth > 0) Then
            fiscalMonth = If(fiscalMonth = 1, 12, (fiscalMonth - 1))
        End If

        lastMonthStartDate = fiscalDataDAO.GetFiscalMonthStartDate(fiscalYear, fiscalMonth)
        lastMonthEndDate = fiscalDataDAO.GetFiscalMonthEndDate(fiscalYear, fiscalMonth)

        lastMonthActiveSKUs = skuDetails.FindAll(Function(a) a.SKUActiveDate >= lastMonthStartDate AndAlso a.SKUActiveDate <= lastMonthEndDate)

        Return lastMonthActiveSKUs
    End Function
#End Region

#Region "Stage Perf Exception Report"
    Private Shared Sub WritePerfExceptionListToExcelWorksheet(ByVal SKUDetails As List(Of SKUDetail),
                                                              ByVal worksheetPart As WorksheetPart,
                                                              ByVal worksheet As Worksheet,
                                                              ByVal sheetData As SheetData,
                                                              ByVal pathToSiteDays As PathToSiteTiming,
                                                              ByVal imageProductionDays As Integer)
        Dim exceptionItems As List(Of SKUDetail) = Nothing
        Dim exceptionTypeList As List(Of SKUDetail) = Nothing
        Dim reportBAO As New Report()

        exceptionItems = New List(Of SKUDetail)()

        'Style SKU setup to Sample Request Exceptions
        exceptionTypeList = SKUDetails.FindAll(Function(a) a.SSSetupToSampleRequest > pathToSiteDays.SSSetupToSampleRequestAverageDays)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Style SKU setup to Sample Request")

        'Sample Request to Sample Receipt Exceptions
        exceptionTypeList = SKUDetails.FindAll(Function(a) a.SampleRequestToSampleReceipt > pathToSiteDays.SampleRequestToSampleReceiptAverageDays)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Sample Request to Sample Receipt")

        'Style SKU setup to PO Approval Exceptions
        exceptionTypeList = SKUDetails.FindAll(Function(a) a.SSSetupToPOApproval > pathToSiteDays.SSSetupToPOApprovalAverageDays)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Style SKU setup to PO Approval")

        'Style SKU setup to Turn-In Exceptions
        exceptionTypeList = SKUDetails.FindAll(Function(a) a.SSSetupToTurnIn > pathToSiteDays.SSSetupToTurnInAverageDays)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Style SKU setup to Turn-In")

        'PO Approval to Turn-In Exceptions
        exceptionTypeList = SKUDetails.FindAll(Function(a) a.POApprovalToTurnIn > pathToSiteDays.POApprovalToTurnInAverageDays)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "PO Approval to Turn-In")

        'Sample Receipt to Turn-In Exceptions
        exceptionTypeList = SKUDetails.FindAll(Function(a) a.SampleReceiptToTurnIn > pathToSiteDays.SampleReceiptToTurnInAverageDays)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Sample Receipt to Turn-In")

        'Turn-In To Image Shot Exceptions
        'exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("VENDOR IMAGE") _
        '                                           AndAlso a.TurnInToImageShot > pathToSiteDays.TurnInToImageShotAverageDaysVI)
        'AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Turn-In To Image Shot - Vendor Image ads")

        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("TURN IN") _
                                                   AndAlso a.TurnInToImageShot > pathToSiteDays.TurnInToImageShotAverageDaysTI)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Turn-In To Image Shot - Turn-In ads")

        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("LIFT") _
                                                   AndAlso a.TurnInToImageShot > pathToSiteDays.TurnInToImageShotAverageDaysLIFT)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Turn-In To Image Shot - LIFT ads")

        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("INFC TRANSFER") _
                                                   AndAlso a.TurnInToImageShot > pathToSiteDays.TurnInToImageShotAverageDaysINFC)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Turn-In To Image Shot - INFC ads")

        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("EXTRA HOT") _
                                                   AndAlso a.TurnInToImageShot > pathToSiteDays.TurnInToImageShotAverageDaysExtraHot)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Turn-In To Image Shot - Extra Hot ads")

        'Image Shot to Final Image Ready Exceptions
        'exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("VENDOR IMAGE") _
        '                                           AndAlso a.ImageShotToImageReady > pathToSiteDays.ImageShotToFInalImageDaysVI)
        'AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Image Shot to Final Image Ready - Vendor Image ads")

        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("TURN IN") _
                                                   AndAlso a.ImageShotToImageReady > pathToSiteDays.ImageShotToFInalImageDaysTI)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Image Shot to Final Image Ready - Turn-In ads")

        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("LIFT") _
                                                   AndAlso a.ImageShotToImageReady > pathToSiteDays.ImageShotToFInalImageDaysLIFT)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Image Shot to Final Image Ready - LIFT ads")

        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("INFC TRANSFER") _
                                                   AndAlso a.ImageShotToImageReady > pathToSiteDays.ImageShotToFInalImageDaysINFC)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Image Shot to Final Image Ready - INFC ads")

        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("EXTRA HOT") _
                                                   AndAlso a.ImageShotToImageReady > pathToSiteDays.ImageShotToFInalImageDaysExtraHot)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Image Shot to Final Image Ready - Extra Hot ads")

        'Total Photo Time Exceptions
        'exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("VENDOR IMAGE") _
        '                                           AndAlso a.TurnInToImageReady > pathToSiteDays.AveragePhotoTimeVI)
        'AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Total Photo Time - Vendor Image ads")

        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("TURN IN") _
                                                   AndAlso a.TurnInToImageReady > pathToSiteDays.AveragePhotoTimeTI)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Total Photo Time - Turn-In ads")

        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("LIFT") _
                                                   AndAlso a.TurnInToImageReady > pathToSiteDays.AveragePhotoTimeLIFT)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Total Photo Time - LIFT ads")

        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("INFC TRANSFER") _
                                                   AndAlso a.TurnInToImageReady > pathToSiteDays.AveragePhotoTimeINFC)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Total Photo Time - INFC ads")

        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("EXTRA HOT") _
                                                   AndAlso a.TurnInToImageReady > pathToSiteDays.AveragePhotoTimeExtraHot)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Total Photo Time - Extra Hot ads")

        'PO Receipt to Turn-In Exceptions
        exceptionTypeList = SKUDetails.FindAll(Function(a) a.SampleReceiptToTurnIn > pathToSiteDays.TurnInToPOReceiptAverageDays)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "PO Receipt to Turn-In")

        'Turn-in to Copy Ready Exceptions
        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("VENDOR IMAGE") _
                                                   AndAlso a.TurnInToCopyReady > pathToSiteDays.TurnInToCopyReadyAverageDaysVI)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Turn-in to Copy Ready - Vendor Image ads")

        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("TURN IN") _
                                                   AndAlso a.TurnInToCopyReady > pathToSiteDays.TurnInToCopyReadyAverageDaysTI)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Turn-in to Copy Ready - Turn-In ads")

        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("LIFT") _
                                                   AndAlso a.TurnInToCopyReady > pathToSiteDays.TurnInToCopyReadyAverageDaysLIFT)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Turn-in to Copy Ready - LIFT ads")

        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("INFC TRANSFER") _
                                                   AndAlso a.TurnInToCopyReady > pathToSiteDays.TurnInToCopyReadyAverageDaysINFC)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Turn-in to Copy Ready - INFC ads")

        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("EXTRA HOT") _
                                                   AndAlso a.TurnInToCopyReady > pathToSiteDays.TurnInToCopyReadyAverageDaysExtraHot)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Turn-in to Copy Ready - Extra Hot ads")

        'Vendor Image Ad Exceptions
        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("VENDOR IMAGE") _
                                                   AndAlso a.TurnInToProductActive > pathToSiteDays.VendorImageAdsAverageDays)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Turn-in to Ready Status - Vendor Image ads")
        'Turn in Ad Exceptions
        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("TURN IN") _
                                                   AndAlso a.TurnInToProductActive > pathToSiteDays.TurnInAdsAverageDays)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Turn-in to Ready Status - Turn-In ads")

        'LIFT Ad Exceptions
        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("LIFT") _
                                                   AndAlso a.TurnInToProductActive > pathToSiteDays.LiftAdsAverageDays)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Turn-in to Ready Status - LIFT ads")

        'INFC Ad Exceptions
        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("INFC TRANSFER") _
                                                   AndAlso a.TurnInToProductActive > pathToSiteDays.INFCAdsAverageDays)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Turn-in to Ready Status - INFC ads")

        'Extra Hot Ad Exceptions
        exceptionTypeList = SKUDetails.FindAll(Function(a) a.AdType.ToUpper().Equals("EXTRA HOT") _
                                                   AndAlso a.TurnInToProductActive > pathToSiteDays.ExtraHotAdsAverageDays)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "Turn-in to Ready Status - Extra Hot ads")

        ''In Image Production Exceptions
        exceptionTypeList = SKUDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                                 AndAlso a.ImageShotDate <= Date.MinValue _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) > imageProductionDays)
        AddExceptionsToTheResultList(exceptionItems, exceptionTypeList, "In Image Production")

        exceptionItems = exceptionItems.OrderBy(Function(a) a.GMM).ThenBy(Function(a) a.DMM).ThenBy(Function(a) a.Buyer).ThenBy(Function(a) a.Dept).ToList()

        WritePerfSummaryListToExcelWorksheet(exceptionItems, worksheetPart, worksheet, sheetData, True, True)

    End Sub

#End Region

#Region "Path To Site Report"
    Public Shared Sub ExportPathToSiteMerchantReportToExcel(ByVal excelFileName As String, ByVal pageTitle As String,
                                                            ByVal ohReceiptDetails As List(Of SKUDetail),
                                                            ByVal skuDetails As List(Of SKUDetail),
                                                            ByVal photoShootDays As Integer,
                                                            ByVal imageProductionDays As Integer)
        Dim workbookPart As WorkbookPart = Nothing
        Dim worksheetPart As WorksheetPart = Nothing
        Dim stylesheetPart As WorkbookStylesPart = Nothing
        Dim fiscalDataDAO As FiscalDataDAO = Nothing
        Dim workbook As Workbook = Nothing
        Dim sheets As Sheets = Nothing
        Dim sheet As Sheet = Nothing
        Dim worksheet As Worksheet = Nothing
        Dim sheetData As SheetData = Nothing
        Dim excelRow As Row = Nothing
        Dim lastMonthActiveSKUs As List(Of SKUDetail) = Nothing
        Dim fiscalWeek As Integer = 0
        Dim fiscalMonth As Integer = 0
        Dim fiscalYear As Integer = 0
        Dim notActiveSKUDetails As List(Of SKUDetail) = Nothing

        Try
            Using excelPackage As SpreadsheetDocument = SpreadsheetDocument.Create(excelFileName, SpreadsheetDocumentType.Workbook)

                workbookPart = excelPackage.AddWorkbookPart()

                workbook = New Workbook()
                workbook.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships")

                sheets = New Sheets()
                sheet = New Sheet With {.Name = "Path to Site Merchant Status", .SheetId = 1, .Id = "r1"}

                sheets.Append(sheet)

                worksheetPart = workbookPart.AddNewPart(Of WorksheetPart)("r1")

                worksheet = New Worksheet()
                sheetData = New SheetData()

                'Add Style Sheet
                stylesheetPart = workbookPart.AddNewPart(Of WorkbookStylesPart)()
                stylesheetPart.Stylesheet = GenerateStyleSheet()
                stylesheetPart.Stylesheet.Save()

                fiscalDataDAO = New FiscalDataDAO()

                fiscalWeek = fiscalDataDAO.GetFiscalWeekByDate(Date.Today)
                fiscalMonth = fiscalDataDAO.GetFiscalMonthByDate(Date.Today)
                fiscalYear = fiscalDataDAO.GetFiscalYearByDate(Date.Today)

                notActiveSKUDetails = skuDetails.FindAll(Function(a) a.SKUActiveDate > Date.MinValue)

                ohReceiptDetails = ohReceiptDetails.FindAll(Function(b) b.SKUActiveDate > Date.MinValue)

                WritePathToSiteMerchantListToExcelWorksheet(ohReceiptDetails, worksheetPart, worksheet,
                                                            sheetData, notActiveSKUDetails,
                                                            photoShootDays,
                                                            imageProductionDays,
                                                            fiscalWeek,
                                                            fiscalMonth,
                                                            fiscalYear)

                sheet = New Sheet With {.Name = "Path To Site Merch Status GMM", .SheetId = 2, .Id = "r2"}
                sheets.Append(sheet)

                worksheetPart = workbookPart.AddNewPart(Of WorksheetPart)("r2")

                worksheet = New Worksheet()
                sheetData = New SheetData()

                WritePathToSiteMerchantStatusGMMListToExcelWorksheet(ohReceiptDetails, worksheetPart,
                                                                     worksheet, sheetData, notActiveSKUDetails,
                                                                     photoShootDays,
                                                                     imageProductionDays,
                                                                     fiscalWeek,
                                                                     fiscalMonth,
                                                                     fiscalYear)

                sheet = New Sheet With {.Name = "Path to Site Ad # Status", .SheetId = 3, .Id = "r3"}
                sheets.Append(sheet)
                workbook.Append(sheets)
                workbookPart.Workbook = workbook

                worksheetPart = workbookPart.AddNewPart(Of WorksheetPart)("r3")

                worksheet = New Worksheet()
                sheetData = New SheetData()

                WritePathToSiteAdStatusListToExcelWorksheet(ohReceiptDetails, worksheetPart, worksheet,
                                                            sheetData, notActiveSKUDetails,
                                                            photoShootDays, imageProductionDays,
                                                            fiscalWeek,
                                                            fiscalMonth,
                                                            fiscalYear)

            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Shared Sub WritePathToSiteMerchantListToExcelWorksheet(ByVal ohReceiptDetails As List(Of SKUDetail),
                                                                   ByVal worksheetPart As WorksheetPart,
                                                 ByVal worksheet As Worksheet,
                                                 ByVal sheetData As SheetData,
                                                 ByVal skuDetails As List(Of SKUDetail),
                                                 ByVal photoShootDays As Integer,
                                                 ByVal imageProductionDays As Integer,
                                                 ByVal fiscalWeek As Integer,
                                                 ByVal fiscalMonth As Integer,
                                                 ByVal fiscalYear As Integer)

        Dim cellValue As String = String.Empty
        Dim columnIndex As Integer = 0

        Dim dateTimeInfo As DateTimeFormatInfo = Nothing
        Dim cal As Calendar = Nothing
        Dim currentMonthStartDate As Date = Nothing
        Dim currentMonthEndDate As Date = Nothing
        Dim nextMonthStartDate As Date = Nothing
        Dim nextMonthEndDate As Date = Nothing
        Dim nextToNextMonthStartDate As Date = Nothing
        Dim nextToNextMonthEndDate As Date = Nothing
        Dim fiscalDataDAO As FiscalDataDAO = Nothing

        '  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
        '
        '  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
        '  cells of data, we'll know if to write Text values or Numeric cell values.
        Dim numberOfColumns As Integer = 42
        Dim IsNumericColumn(numberOfColumns) As Boolean

        Dim excelColumnNames([numberOfColumns]) As String

        For n As Integer = 0 To numberOfColumns
            excelColumnNames(numberOfColumns) = GetExcelColumnName(n)
        Next n

        'Get datetime info
        dateTimeInfo = DateTimeFormatInfo.CurrentInfo
        cal = dateTimeInfo.Calendar
        'fiscalWeek = cal.GetWeekOfYear(Date.Today(), dateTimeInfo.CalendarWeekRule, dateTimeInfo.FirstDayOfWeek)

        '
        '  Create the Header row in our Excel Worksheet
        '
        Dim rowIndex As UInt32 = 1

        'Append Title
        Dim titleRow As Row = New Row
        titleRow.RowIndex = rowIndex            ' add the title at the top of spreadsheet
        titleRow = GetPathToSiteMerchantReportTitle()
        sheetData.Append(titleRow)

        rowIndex = rowIndex + 1
        'Append OH Locations
        Dim ohLocationsRow As Row = New Row
        ohLocationsRow.RowIndex = rowIndex            ' add the headers at the top of spreadsheet
        ohLocationsRow.Append(AddHeaderColumn("ON HAND - 192,193,195", "G2"))
        columnIndex = 6
        For index = 1 To 35
            columnIndex = columnIndex + 1
            ohLocationsRow.Append(AddHeaderColumn("", GetExcelColumnName(columnIndex)))
        Next

        sheetData.Append(ohLocationsRow)

        rowIndex = rowIndex + 1
        'Append OH section Headers
        Dim ohSectionHeaders As Row = New Row
        ohSectionHeaders.RowIndex = rowIndex            ' add the headers at the top of spreadsheet
        GetPathToSiteMerchantSectionHeaders(ohSectionHeaders, True, 6, 5)
        sheetData.Append(ohSectionHeaders)

        rowIndex = rowIndex + 1
        'Append OH Headers
        Dim headerRow As Row = New Row
        headerRow.RowIndex = rowIndex            ' add the headers at the top of spreadsheet
        headerRow = GetPathToSiteMerchantReportHeaders(True)
        sheetData.Append(headerRow)

        rowIndex = rowIndex + 1
        Dim newExcelRow As New Row
        newExcelRow.RowIndex = rowIndex         '  add a row at the top of spreadsheet
        sheetData.Append(newExcelRow)

        'Fiscal Year
        columnIndex = 0
        cellValue = fiscalYear
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

        'Fiscal Month
        columnIndex = columnIndex + 1
        cellValue = fiscalMonth
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

        'Fiscal Week
        columnIndex = columnIndex + 1
        cellValue = fiscalWeek
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

        'FC OH $
        columnIndex = columnIndex + 1
        cellValue = skuDetails.FindAll(Function(a) a.ReportItemType = "OH").Sum(Function(b) b.SKUCost)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

        'FC Received $
        columnIndex = columnIndex + 1
        cellValue = ohReceiptDetails.Sum(Function(a) a.SKUCost)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

        GetCommonFieldsValues(ohReceiptDetails, skuDetails, excelColumnNames, newExcelRow, rowIndex,
                              columnIndex,
                              photoShootDays, imageProductionDays,
                              True, True)

        rowIndex = 14

        'Append OH Locations
        Dim ooLocationsRow As Row = New Row
        ooLocationsRow.RowIndex = rowIndex            ' add the headers at the top of spreadsheet
        ooLocationsRow.Append(AddHeaderColumn("ON ORDER - 192,193,195", "G14"))
        columnIndex = 6
        For index = 1 To 35
            columnIndex = columnIndex + 1
            ooLocationsRow.Append(AddHeaderColumn("", GetExcelColumnName(columnIndex)))
        Next
        sheetData.Append(ooLocationsRow)

        rowIndex = rowIndex + 1
        'Append OH section Headers
        Dim ooSectionHeaders As Row = New Row
        ooSectionHeaders.RowIndex = rowIndex            ' add the headers at the top of spreadsheet
        GetPathToSiteMerchantSectionHeaders(ooSectionHeaders, True, 6, 5)
        sheetData.Append(ooSectionHeaders)

        rowIndex = rowIndex + 1
        'Append OO Headers
        Dim ooHeaderRow As Row = New Row
        ooHeaderRow.RowIndex = rowIndex            ' add the headers at the top of spreadsheet
        ooHeaderRow = GetPathToSiteMerchantReportHeaders(False)
        sheetData.Append(ooHeaderRow)

        rowIndex = rowIndex + 1
        Dim firstFiscalWeekRow As New Row
        firstFiscalWeekRow.RowIndex = rowIndex         '  add a row at the top of spreadsheet
        sheetData.Append(firstFiscalWeekRow)

        'Fiscal Year
        columnIndex = 0
        cellValue = fiscalYear
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, firstFiscalWeekRow, CellValues.Number)

        'Fiscal Month
        columnIndex = columnIndex + 1
        cellValue = fiscalMonth
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, firstFiscalWeekRow, CellValues.Number)

        'Fiscal Week
        columnIndex = columnIndex + 1
        cellValue = fiscalWeek
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, firstFiscalWeekRow, CellValues.Number)

        currentMonthStartDate = New DateTime(Date.Today().Year, Date.Today().Month, 1)
        currentMonthEndDate = currentMonthStartDate.AddMonths(1).AddDays(-1)

        'Current Month OO $
        columnIndex = columnIndex + 1
        cellValue = skuDetails.FindAll(Function(a) a.ReportItemType = "OO" _
                                           AndAlso a.POShipDate >= currentMonthStartDate _
                                           AndAlso a.POShipDate <= currentMonthEndDate).Sum(Function(b) b.SKUCost)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, firstFiscalWeekRow, CellValues.Number)

        nextMonthStartDate = New DateTime(Date.Today().Year, Date.Today().AddMonths(1).Month, 1)
        nextMonthEndDate = nextMonthStartDate.AddMonths(1).AddDays(-1)

        'Next Month OO $
        columnIndex = columnIndex + 1
        cellValue = skuDetails.FindAll(Function(a) a.ReportItemType = "OO" _
                                           AndAlso a.POShipDate >= nextMonthStartDate _
                                           AndAlso a.POShipDate <= nextMonthEndDate).Sum(Function(b) b.SKUCost)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, firstFiscalWeekRow, CellValues.Number)

        GetCommonFieldsValues(skuDetails.FindAll(Function(a) a.ReportItemType = "OO" _
                                           AndAlso a.POShipDate >= currentMonthStartDate _
                                           AndAlso a.POShipDate <= currentMonthEndDate),
                              skuDetails.FindAll(Function(a) a.ReportItemType = "OO"),
                              excelColumnNames, firstFiscalWeekRow, rowIndex,
                              columnIndex,
                              photoShootDays,
                              imageProductionDays,
                              True, True)

        'Second Fiscal Month Row
        rowIndex = rowIndex + 3
        Dim secondFiscalWeekRow As New Row
        secondFiscalWeekRow.RowIndex = rowIndex         '  add a row at the top of spreadsheet
        sheetData.Append(secondFiscalWeekRow)

        'Fiscal Year
        columnIndex = 0
        cellValue = String.Empty
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, secondFiscalWeekRow, CellValues.Number)

        'Fiscal Month
        fiscalDataDAO = New FiscalDataDAO()

        columnIndex = columnIndex + 1
        cellValue = fiscalDataDAO.GetFiscalMonthByDate(Date.Today().AddMonths(1).Date)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, secondFiscalWeekRow, CellValues.Number)

        'Fiscal Week
        columnIndex = columnIndex + 1
        cellValue = String.Empty
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, secondFiscalWeekRow, CellValues.Number)

        columnIndex = columnIndex + 1
        cellValue = String.Empty
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, secondFiscalWeekRow, CellValues.Number)

        columnIndex = columnIndex + 1
        cellValue = String.Empty
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, secondFiscalWeekRow, CellValues.Number)

        GetCommonFieldsValues(skuDetails.FindAll(Function(a) a.ReportItemType = "OO" _
                                           AndAlso a.POShipDate >= nextMonthStartDate _
                                           AndAlso a.POShipDate <= nextMonthEndDate),
                              skuDetails.FindAll(Function(a) a.ReportItemType = "OO"),
                              excelColumnNames, secondFiscalWeekRow, rowIndex,
                              columnIndex,
                              photoShootDays, imageProductionDays,
                              True, True)

        'Third Fiscal Month Row
        rowIndex = rowIndex + 3
        Dim thirdFiscalWeekRow As New Row
        thirdFiscalWeekRow.RowIndex = rowIndex         '  add a row at the top of spreadsheet
        sheetData.Append(thirdFiscalWeekRow)

        nextToNextMonthStartDate = New DateTime(Date.Today().Year, Date.Today().AddMonths(2).Month, 1)
        nextToNextMonthEndDate = nextMonthStartDate.AddMonths(2).AddDays(-1)

        'Fiscal Year
        columnIndex = 0
        cellValue = String.Empty
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, thirdFiscalWeekRow, CellValues.Number)

        'Fiscal Month
        columnIndex = columnIndex + 1
        cellValue = fiscalDataDAO.GetFiscalMonthByDate(Date.Today().AddMonths(2).Date)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, thirdFiscalWeekRow, CellValues.Number)

        'Fiscal Week
        columnIndex = columnIndex + 1
        cellValue = String.Empty
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, thirdFiscalWeekRow, CellValues.Number)

        columnIndex = columnIndex + 1
        cellValue = String.Empty
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, thirdFiscalWeekRow, CellValues.Number)

        columnIndex = columnIndex + 1
        cellValue = String.Empty
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, thirdFiscalWeekRow, CellValues.Number)

        GetCommonFieldsValues(skuDetails.FindAll(Function(a) a.ReportItemType = "OO" _
                                           AndAlso a.POShipDate >= nextToNextMonthStartDate _
                                           AndAlso a.POShipDate <= nextToNextMonthEndDate),
                              skuDetails.FindAll(Function(a) a.ReportItemType = "OO"),
                              excelColumnNames, thirdFiscalWeekRow, rowIndex,
                              columnIndex,
                              photoShootDays,
                              imageProductionDays,
                              True, True)

        worksheet.Append(sheetData)
        worksheetPart.Worksheet = worksheet

        MergeTwoCells(worksheet, "A1", "B1")
        MergeTwoCells(worksheet, "G2", "AP2")
        MergeTwoCells(worksheet, "G3", "L3")
        MergeTwoCells(worksheet, "M3", "R3")
        MergeTwoCells(worksheet, "S3", "X3")
        MergeTwoCells(worksheet, "Y3", "AD3")
        MergeTwoCells(worksheet, "AE3", "AJ3")
        MergeTwoCells(worksheet, "AK3", "AP3")
        MergeTwoCells(worksheet, "G14", "AP14")
        MergeTwoCells(worksheet, "G15", "L15")
        MergeTwoCells(worksheet, "M15", "R15")
        MergeTwoCells(worksheet, "S15", "X15")
        MergeTwoCells(worksheet, "Y15", "AD15")
        MergeTwoCells(worksheet, "AE15", "AJ15")
        MergeTwoCells(worksheet, "AK15", "AP15")
    End Sub

    Private Shared Function GetPathToSiteMerchantReportTitle() As Row
        Dim excelRow As Row = Nothing
        Dim excelCell As Cell = Nothing
        Dim columnCount As Integer = 0
        excelRow = New Row()

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Path to Site Report", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn(Date.Today().ToShortDateString(), GetExcelColumnName(columnCount)))

        Return excelRow
    End Function

    Private Shared Function GetPathToSiteMerchantReportHeaders(ByVal isOH As Boolean) As Row
        Dim excelRow As Row = Nothing
        Dim excelCell As Cell = Nothing
        Dim headerString As InlineString = Nothing
        Dim cellText As InlineString = Nothing
        Dim cellValue As CellValue = Nothing
        Dim runProp As RunProperties = Nothing
        Dim runCell As Run = Nothing
        Dim columnCount As Integer = 0
        excelRow = New Row()

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Fiscal Year", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Fiscal Month", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Fiscal Week", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn(If(isOH, "FC OH $", "Current Month OO $"), GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn(If(isOH, "FC Received $", "Next Month OO $"), GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("", GetExcelColumnName(columnCount)))

        GetCommonReportHeaders(excelRow, columnCount, True)

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("Not Turned in # Styles/Colors", GetExcelColumnName(columnCount), 4))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("% to ttl  OH FC # Styles/Colors not Turned in ", GetExcelColumnName(columnCount), 4))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("Not Turned in $ Styles/Colors", GetExcelColumnName(columnCount), 5))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("% to ttl OH FC $ Styles/Colors not Turned in", GetExcelColumnName(columnCount), 5))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("IN CMR # Style/colors", GetExcelColumnName(columnCount), 4))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("% to TTL In CMR # Style/colors", GetExcelColumnName(columnCount), 4))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("IN CMR $ Style/colors", GetExcelColumnName(columnCount), 5))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("% to TTL In CMR $ Style/colors", GetExcelColumnName(columnCount), 5))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("AT STUDIO # Style/colors", GetExcelColumnName(columnCount), 4))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("% to TTL AT STUDIO # Style/colors", GetExcelColumnName(columnCount), 4))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("AT STUDIO  $Style/colors", GetExcelColumnName(columnCount), 5))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("% to TTL AT STUDIO $ Style/colors", GetExcelColumnName(columnCount), 5))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("In Image Production  Style/colors #", GetExcelColumnName(columnCount), 4))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("In Image Production % to ttl OH FC # Style/Colors", GetExcelColumnName(columnCount), 4))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn(" In Image Production Style/colors $", GetExcelColumnName(columnCount), 5))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("In Image Production % to ttl OH FC $   Style/Colors", GetExcelColumnName(columnCount), 5))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("Awaiting Premedia Style/colors", GetExcelColumnName(columnCount), 4))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("% to ttl OH FC # awaiting Premedia style/ color", GetExcelColumnName(columnCount), 4))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("Awaiting Premedia Style/colors $", GetExcelColumnName(columnCount), 5))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("% to TTLOH FC $ awaiting Premedia style/ color", GetExcelColumnName(columnCount), 5))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("Awaiting copy Products #", GetExcelColumnName(columnCount), 4))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("% to ttl OH FC # awaiting copy", GetExcelColumnName(columnCount), 4))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("Awaiting Copy Products $", GetExcelColumnName(columnCount), 5))

        'columnCount += 1
        'excelRow.Append(AddHeaderColumn("% to ttl OH FC $ awaiting copy Products", GetExcelColumnName(columnCount), 5))

        Return excelRow
    End Function

    Private Shared Function GetPathToSiteMerchantSectionHeaders(ByRef excelRow As Row,
                                                                ByVal isOH As Boolean,
                                                                ByVal blankColumnCount As Integer,
                                                                ByVal headerCellMergeCount As Integer) As Row
        Dim excelCell As Cell = Nothing
        Dim headerString As InlineString = Nothing
        Dim cellText As InlineString = Nothing
        Dim cellValue As CellValue = Nothing
        Dim runProp As RunProperties = Nothing
        Dim runCell As Run = Nothing
        Dim columnCount As Integer = 0

        For index = 1 To blankColumnCount
            columnCount += 1
            excelRow.Append(AddHeaderColumn(String.Empty, GetExcelColumnName(columnCount)))
        Next

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Not Turned In", GetExcelColumnName(columnCount)))

        For index = 1 To headerCellMergeCount
            columnCount += 1
            excelRow.Append(AddHeaderColumn(String.Empty, GetExcelColumnName(columnCount)))
        Next

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Turned in - In CMR", GetExcelColumnName(columnCount)))

        For index = 1 To headerCellMergeCount
            columnCount += 1
            excelRow.Append(AddHeaderColumn(String.Empty, GetExcelColumnName(columnCount)))
        Next

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Turned in - At Studio", GetExcelColumnName(columnCount)))

        For index = 1 To headerCellMergeCount
            columnCount += 1
            excelRow.Append(AddHeaderColumn(String.Empty, GetExcelColumnName(columnCount)))
        Next

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Turned in - In Image Production", GetExcelColumnName(columnCount)))

        For index = 1 To headerCellMergeCount
            columnCount += 1
            excelRow.Append(AddHeaderColumn(String.Empty, GetExcelColumnName(columnCount)))
        Next

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Awaiting Premedia", GetExcelColumnName(columnCount)))

        For index = 1 To headerCellMergeCount
            columnCount += 1
            excelRow.Append(AddHeaderColumn(String.Empty, GetExcelColumnName(columnCount)))
        Next

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Awaiting Copy", GetExcelColumnName(columnCount)))

        For index = 1 To headerCellMergeCount
            columnCount += 1
            excelRow.Append(AddHeaderColumn(String.Empty, GetExcelColumnName(columnCount)))
        Next

        Return excelRow
    End Function

    Private Shared Function GetPercentageFromTwoNumbers(ByVal divident As Integer, ByVal divisor As Integer) As Double
        If divident <= 0 OrElse divisor <= 0 Then
            Return 0.0
        Else
            Return System.Math.Round(divident / divisor, 2)
        End If

    End Function

    Private Shared Sub GetCommonFieldsValues(ByVal weeklySKUDetails As List(Of SKUDetail),
                                             ByVal skuDetails As List(Of SKUDetail),
                                                  ByRef excelColumnNames() As String, ByRef newExcelRow As Row,
                                                  ByRef rowIndex As Integer, ByVal columnIndex As Integer,
                                                  ByVal photoShootDays As Integer, ByVal imageProductionDays As Integer,
                                                  Optional ByVal includeBlankColumn As Boolean = True,
                                                  Optional ByVal includeTotalCategoryValues As Boolean = False,
                                                  Optional ByVal gmmName As String = "",
                                                  Optional ByVal styleIndex As Integer = 0)
        Dim cellValue As String = String.Empty

        If includeBlankColumn Then
            AddBlankColumns(excelColumnNames, newExcelRow, rowIndex, columnIndex, 1)
        End If

        'Not Turned in # Styles/Colors
        columnIndex = columnIndex + 1
        cellValue = weeklySKUDetails.FindAll(Function(a) a.TurnInDate <= Date.MinValue).Sum(Function(b) b.Quantity)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue,
                   newExcelRow, CellValues.Number, styleIndex)

        '% to total OH FC# Styles/Colors Not Turned in 
        columnIndex = columnIndex + 1
        If String.IsNullOrEmpty(gmmName) Then
            cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.TurnInDate <=
                                                                                 Date.MinValue).Sum(Function(b) b.Quantity),
                skuDetails.FindAll(Function(a) a.TurnInDate <= Date.MinValue).Sum(Function(b) b.Quantity))
        Else
            cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.TurnInDate <=
                                                                                 Date.MinValue).Sum(Function(b) b.Quantity),
                skuDetails.FindAll(Function(a) a.TurnInDate <= Date.MinValue _
                                       AndAlso a.GMM = gmmName).Sum(Function(b) b.Quantity))

        End If
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total # Styles/Colors Not Turned in 
        If includeTotalCategoryValues Then
            columnIndex = columnIndex + 1
            If String.IsNullOrEmpty(gmmName) Then
                cellValue = 100
            Else
                cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.TurnInDate <=
                                                                                     Date.MinValue).Sum(Function(b) b.Quantity),
                    skuDetails.FindAll(Function(a) a.TurnInDate <= Date.MinValue).Sum(Function(b) b.Quantity))
            End If
            AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                       cellValue, newExcelRow, CellValues.Number, styleIndex)
        End If

        'Not Turned in $ Styles/Colors
        columnIndex = columnIndex + 1
        cellValue = weeklySKUDetails.FindAll(Function(a) a.TurnInDate <=
                                                 Date.MinValue).Sum(Function(b) b.SKUCost)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total OH FC $ Styles/Colors Not Turned in
        columnIndex = columnIndex + 1

        If String.IsNullOrEmpty(gmmName) Then
            cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.TurnInDate <=
                                                                                 Date.MinValue).Sum(Function(b) b.SKUCost),
                skuDetails.FindAll(Function(a) a.TurnInDate <= Date.MinValue).Sum(Function(b) b.SKUCost))
        Else
            cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.TurnInDate <=
                                                                                 Date.MinValue).Sum(Function(b) b.SKUCost),
                skuDetails.FindAll(Function(a) a.TurnInDate <= Date.MinValue _
                                       AndAlso a.GMM = gmmName).Sum(Function(b) b.SKUCost))
        End If

        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total $ Styles/Colors Not Turned in
        If includeTotalCategoryValues Then
            columnIndex = columnIndex + 1
            If String.IsNullOrEmpty(gmmName) Then
                cellValue = 100
            Else
                cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.TurnInDate <=
                                                                                     Date.MinValue).Sum(Function(b) b.SKUCost),
                    skuDetails.FindAll(Function(a) a.TurnInDate <= Date.MinValue).Sum(Function(b) b.SKUCost))

            End If
            AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                       cellValue, newExcelRow, CellValues.Number, styleIndex)
        End If


        'In CMR # Styles/Colors
        columnIndex = columnIndex + 1
        cellValue = weeklySKUDetails.FindAll(Function(a) a.SampleReceiptDate > Date.MinValue _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("RECEIVED") _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("CMR - MILWAUKEE CORP")
                                                 ).Sum(Function(b) b.Quantity)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total In CMR # Styles/Colors
        columnIndex = columnIndex + 1
        If String.IsNullOrEmpty(gmmName) Then
            cellValue = GetPercentageFromTwoNumbers(
                weeklySKUDetails.FindAll(Function(a) a.SampleReceiptDate > Date.MinValue _
                                            AndAlso a.SampleStatusDesc.ToUpper().Equals("RECEIVED") _
                                             AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("CMR - MILWAUKEE CORP")
                                             ).Sum(Function(b) b.Quantity),
                skuDetails.FindAll(Function(a) a.SampleReceiptDate > Date.MinValue _
                                        AndAlso a.SampleStatusDesc.ToUpper().Equals("RECEIVED") _
                                       AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("CMR - MILWAUKEE CORP")
                                       ).Sum(Function(b) b.Quantity))
        Else
            cellValue = GetPercentageFromTwoNumbers(
                weeklySKUDetails.FindAll(Function(a) a.SampleReceiptDate > Date.MinValue _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("RECEIVED") _
                                             AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("CMR - MILWAUKEE CORP")
                                             ).Sum(Function(b) b.Quantity),
                skuDetails.FindAll(Function(a) a.SampleReceiptDate > Date.MinValue _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("RECEIVED") _
                                       AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("CMR - MILWAUKEE CORP") _
                                       AndAlso a.GMM = gmmName
                                       ).Sum(Function(b) b.Quantity))
        End If
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total In CMR # Styles/Colors
        If includeTotalCategoryValues Then
            columnIndex = columnIndex + 1
            If String.IsNullOrEmpty(gmmName) Then
                cellValue = 100
            Else
                cellValue = GetPercentageFromTwoNumbers(
                    weeklySKUDetails.FindAll(Function(a) a.SampleReceiptDate > Date.MinValue _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("RECEIVED") _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("CMR - MILWAUKEE CORP")
                                                 ).Sum(Function(b) b.Quantity),
                    skuDetails.FindAll(Function(a) a.SampleReceiptDate > Date.MinValue _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("RECEIVED") _
                                           AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("CMR - MILWAUKEE CORP")
                                           ).Sum(Function(b) b.Quantity))
            End If
            AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                       cellValue, newExcelRow, CellValues.Number, styleIndex)
        End If

        'In CMR $ Styles/Colors
        columnIndex = columnIndex + 1
        cellValue = weeklySKUDetails.FindAll(Function(a) a.SampleReceiptDate > Date.MinValue _
                                                         AndAlso a.SampleStatusDesc.ToUpper().Equals("RECEIVED") _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("CMR - MILWAUKEE CORP")
                                                 ).Sum(Function(b) b.SKUCost)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total In CMR $ Styles/Colors
        columnIndex = columnIndex + 1
        If String.IsNullOrEmpty(gmmName) Then
            cellValue = GetPercentageFromTwoNumbers(
                weeklySKUDetails.FindAll(Function(a) a.SampleReceiptDate > Date.MinValue _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("RECEIVED") _
                                             AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("CMR - MILWAUKEE CORP")
                                             ).Sum(Function(b) b.SKUCost),
                skuDetails.FindAll(Function(a) a.SampleReceiptDate > Date.MinValue _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("RECEIVED") _
                                       AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("CMR - MILWAUKEE CORP")
                                       ).Sum(Function(b) b.SKUCost))
        Else
            cellValue = GetPercentageFromTwoNumbers(
                weeklySKUDetails.FindAll(Function(a) a.SampleReceiptDate > Date.MinValue _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("RECEIVED") _
                                             AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("CMR - MILWAUKEE CORP")
                                             ).Sum(Function(b) b.SKUCost),
                skuDetails.FindAll(Function(a) a.SampleReceiptDate > Date.MinValue _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("RECEIVED") _
                                       AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("CMR - MILWAUKEE CORP") _
                                       AndAlso a.GMM = gmmName
                                       ).Sum(Function(b) b.SKUCost))
        End If
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total In CMR $ Styles/Colors
        If includeTotalCategoryValues Then
            columnIndex = columnIndex + 1
            If String.IsNullOrEmpty(gmmName) Then
                cellValue = 100
            Else
                cellValue = GetPercentageFromTwoNumbers(
                    weeklySKUDetails.FindAll(Function(a) a.SampleReceiptDate > Date.MinValue _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("RECEIVED") _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("CMR - MILWAUKEE CORP")
                                                 ).Sum(Function(b) b.SKUCost),
                    skuDetails.FindAll(Function(a) a.SampleReceiptDate > Date.MinValue _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("RECEIVED") _
                                           AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("CMR - MILWAUKEE CORP")
                                           ).Sum(Function(b) b.SKUCost))
            End If
            AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                       cellValue, newExcelRow, CellValues.Number, styleIndex)
        End If

        'At Studio # Styles/Colors
        columnIndex = columnIndex + 1
        cellValue = weeklySKUDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                                 AndAlso a.ImageShotDate <= Date.MinValue _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) <= photoShootDays
                                                 ).Sum(Function(b) b.Quantity)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total At Studio # Styles/Colors
        columnIndex = columnIndex + 1
        If String.IsNullOrEmpty(gmmName) Then
            cellValue = GetPercentageFromTwoNumbers(
                weeklySKUDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                             AndAlso a.ImageShotDate <= Date.MinValue _
                                             AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                FirstWeekOfYear.Jan1) <= photoShootDays
                                             ).Sum(Function(b) b.Quantity),
                skuDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                       AndAlso a.ImageShotDate <= Date.MinValue _
                                       AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) <= photoShootDays
                                       ).Sum(Function(b) b.Quantity))
        Else
            cellValue = GetPercentageFromTwoNumbers(
                weeklySKUDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                             AndAlso a.ImageShotDate <= Date.MinValue _
                                             AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) <= photoShootDays
                                             ).Sum(Function(b) b.Quantity),
                skuDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                       AndAlso a.ImageShotDate <= Date.MinValue _
                                       AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) <= photoShootDays _
                                       AndAlso a.GMM = gmmName
                                       ).Sum(Function(b) b.Quantity))
        End If
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total At Studio # Styles/Colors
        If includeTotalCategoryValues Then
            columnIndex = columnIndex + 1
            If String.IsNullOrEmpty(gmmName) Then
                cellValue = 100
            Else
                cellValue = GetPercentageFromTwoNumbers(
                    weeklySKUDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                                 AndAlso a.ImageShotDate <= Date.MinValue _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) <= photoShootDays
                                                 ).Sum(Function(b) b.Quantity),
                    skuDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                           AndAlso a.ImageShotDate <= Date.MinValue _
                                           AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) <= photoShootDays
                                           ).Sum(Function(b) b.Quantity))
            End If
            AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                       cellValue, newExcelRow, CellValues.Number, styleIndex)
        End If

        'At Studio $ Styles/Colors
        columnIndex = columnIndex + 1
        cellValue = weeklySKUDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                                 AndAlso a.ImageShotDate <= Date.MinValue _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) <= photoShootDays
                                                 ).Sum(Function(b) b.SKUCost)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total At Studio $ Styles/Colors
        columnIndex = columnIndex + 1
        If String.IsNullOrEmpty(gmmName) Then
            cellValue = GetPercentageFromTwoNumbers(
                weeklySKUDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                             AndAlso a.ImageShotDate <= Date.MinValue _
                                             AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) <= photoShootDays
                                             ).Sum(Function(b) b.SKUCost),
                skuDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                       AndAlso a.ImageShotDate <= Date.MinValue _
                                       AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) <= photoShootDays
                                       ).Sum(Function(b) b.SKUCost))
        Else
            cellValue = GetPercentageFromTwoNumbers(
                weeklySKUDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                             AndAlso a.ImageShotDate <= Date.MinValue _
                                             AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) <= photoShootDays
                                             ).Sum(Function(b) b.SKUCost),
                skuDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                       AndAlso a.ImageShotDate <= Date.MinValue _
                                       AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) <= photoShootDays _
                                       AndAlso a.GMM = gmmName
                                       ).Sum(Function(b) b.SKUCost))

        End If
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total At Studio $ Styles/Colors
        If includeTotalCategoryValues Then
            columnIndex = columnIndex + 1
            If String.IsNullOrEmpty(gmmName) Then
                cellValue = 100
            Else
                cellValue = GetPercentageFromTwoNumbers(
                    weeklySKUDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                                 AndAlso a.ImageShotDate <= Date.MinValue _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) <= photoShootDays
                                                 ).Sum(Function(b) b.SKUCost),
                    skuDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                           AndAlso a.ImageShotDate <= Date.MinValue _
                                           AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) <= photoShootDays
                                           ).Sum(Function(b) b.SKUCost))
            End If
            AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                       cellValue, newExcelRow, CellValues.Number, styleIndex)
        End If

        'In Image Production # Styles/Colors
        columnIndex = columnIndex + 1
        cellValue = weeklySKUDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue _
                                                 AndAlso a.FinalImageReadyDate <= Date.MinValue _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) > photoShootDays
                                                 ).Sum(Function(b) b.Quantity)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total In Image Production # Styles/Colors
        columnIndex = columnIndex + 1
        If String.IsNullOrEmpty(gmmName) Then
            cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.ImageShotDate >
                                                                                 Date.MinValue _
                                                                                 AndAlso a.FinalImageReadyDate <=
                                                                                 Date.MinValue _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) > photoShootDays).Sum(Function(b) b.Quantity),
                skuDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue AndAlso a.FinalImageReadyDate <=
                                       Date.MinValue _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) > photoShootDays).Sum(Function(b) b.Quantity))
        Else
            cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.ImageShotDate >
                                                                                 Date.MinValue _
                                                                                 AndAlso a.FinalImageReadyDate <=
                                                                                 Date.MinValue _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) > photoShootDays).Sum(Function(b) b.Quantity),
                skuDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue AndAlso a.FinalImageReadyDate <=
                                       Date.MinValue _
                                       AndAlso a.GMM = gmmName _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) > photoShootDays).Sum(Function(b) b.Quantity))

        End If
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total In Image Production # Styles/Colors
        If includeTotalCategoryValues Then
            columnIndex = columnIndex + 1
            If String.IsNullOrEmpty(gmmName) Then
                cellValue = 100
            Else
                cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.ImageShotDate >
                                                                                     Date.MinValue _
                                                                                     AndAlso a.FinalImageReadyDate <=
                                                                                     Date.MinValue _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) > photoShootDays).Sum(Function(b) b.Quantity),
                    skuDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue AndAlso a.FinalImageReadyDate <=
                                           Date.MinValue _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) > photoShootDays).Sum(Function(b) b.Quantity))

            End If

            AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                       cellValue, newExcelRow, CellValues.Number, styleIndex)
        End If

        'In Image Production $ Styles/Colors
        columnIndex = columnIndex + 1
        cellValue = weeklySKUDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue _
                                                 AndAlso a.FinalImageReadyDate <=
                                                 Date.MinValue _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) > photoShootDays).Sum(Function(b) b.SKUCost)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total In Image Production $ Styles/Colors
        columnIndex = columnIndex + 1
        If String.IsNullOrEmpty(gmmName) Then
            cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.ImageShotDate >
                                                                                 Date.MinValue _
                                                                                 AndAlso a.FinalImageReadyDate <=
                                                                                 Date.MinValue _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) > photoShootDays).Sum(Function(b) b.SKUCost),
                skuDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue AndAlso a.FinalImageReadyDate <=
                                       Date.MinValue).Sum(Function(b) b.SKUCost))
        Else
            cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.ImageShotDate >
                                                                                 Date.MinValue _
                                                                                 AndAlso a.FinalImageReadyDate <=
                                                                                 Date.MinValue _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) > photoShootDays).Sum(Function(b) b.SKUCost),
                skuDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue AndAlso a.FinalImageReadyDate <=
                                       Date.MinValue _
                                       AndAlso a.GMM = gmmName).Sum(Function(b) b.SKUCost))
        End If
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total In Image Production $ Styles/Colors
        If includeTotalCategoryValues Then
            columnIndex = columnIndex + 1
            If String.IsNullOrEmpty(gmmName) Then
                cellValue = 100
            Else
                cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.ImageShotDate >
                                                                                     Date.MinValue _
                                                                                     AndAlso a.FinalImageReadyDate <=
                                                                                     Date.MinValue _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) > photoShootDays).Sum(Function(b) b.SKUCost),
                    skuDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue AndAlso a.FinalImageReadyDate <=
                                           Date.MinValue _
                                                 AndAlso a.SamplePrimaryLocationName.ToUpper().Equals("BON-TON LAYTON") _
                                                 AndAlso a.SampleStatusDesc.ToUpper().Equals("IN PRODUCTION") _
                                                 AndAlso DateDiff(DateInterval.Day,
                                                                  a.SampleStatusDate,
                                                                  Date.Today(),
                                                                  FirstDayOfWeek.Sunday,
                                                                  FirstWeekOfYear.Jan1) > photoShootDays).Sum(Function(b) b.SKUCost))

            End If
            AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                       cellValue, newExcelRow, CellValues.Number, styleIndex)
        End If

        'Awaiting Premedia # Styles/Colors
        columnIndex = columnIndex + 1
        cellValue = weeklySKUDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue _
                                                 AndAlso a.FinalImageReadyDate <= Date.MinValue).Sum(Function(b) b.Quantity)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total Awaiting Premedia # Styles/Colors
        columnIndex = columnIndex + 1
        If String.IsNullOrEmpty(gmmName) Then
            cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue _
                                                                                 AndAlso a.FinalImageReadyDate <=
                                                                                 Date.MinValue).Sum(Function(b) b.Quantity),
                skuDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue AndAlso a.FinalImageReadyDate <=
                                       Date.MinValue).Sum(Function(b) b.Quantity))
        Else
            cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue _
                                                                     AndAlso a.FinalImageReadyDate <=
                                                                     Date.MinValue).Sum(Function(b) b.Quantity),
                skuDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue AndAlso a.FinalImageReadyDate <=
                           Date.MinValue _
                           AndAlso a.GMM = gmmName).Sum(Function(b) b.Quantity))
        End If
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total Awaiting Premedia # Styles/Colors
        If includeTotalCategoryValues Then
            columnIndex = columnIndex + 1
            If String.IsNullOrEmpty(gmmName) Then
                cellValue = 100
            Else
                cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue _
                                                                                     AndAlso a.FinalImageReadyDate <=
                                                                                     Date.MinValue).Sum(Function(b) b.Quantity),
                    skuDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue AndAlso a.FinalImageReadyDate <=
                                           Date.MinValue).Sum(Function(b) b.Quantity))

            End If
            AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                       cellValue, newExcelRow, CellValues.Number, styleIndex)
        End If

        'Awaiting Premedia $ Styles/Colors
        columnIndex = columnIndex + 1
        cellValue = weeklySKUDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue _
                                                 AndAlso a.FinalImageReadyDate <= Date.MinValue).Sum(Function(b) b.SKUCost)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total Awaiting Premedia $ Styles/Colors
        columnIndex = columnIndex + 1
        If String.IsNullOrEmpty(gmmName) Then
            cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue _
                                                                     AndAlso a.FinalImageReadyDate <=
                                                                     Date.MinValue).Sum(Function(b) b.SKUCost),
                                        skuDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue _
                                                               AndAlso a.FinalImageReadyDate <=
                                                               Date.MinValue).Sum(Function(b) b.SKUCost))
        Else
            cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue _
                                                                                 AndAlso a.FinalImageReadyDate <=
                                                                                 Date.MinValue).Sum(Function(b) b.SKUCost),
                                                    skuDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue _
                                                                           AndAlso a.FinalImageReadyDate <=
                                                                           Date.MinValue _
                                                                           AndAlso a.GMM = gmmName).Sum(Function(b) b.SKUCost))
        End If
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total Awaiting Premedia $ Styles/Colors
        If includeTotalCategoryValues Then
            columnIndex = columnIndex + 1
            If String.IsNullOrEmpty(gmmName) Then
                cellValue = 100
            Else
                cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue _
                                                                                     AndAlso a.FinalImageReadyDate <=
                                                                                     Date.MinValue).Sum(Function(b) b.SKUCost),
                                                        skuDetails.FindAll(Function(a) a.ImageShotDate > Date.MinValue _
                                                                               AndAlso a.FinalImageReadyDate <=
                                                                               Date.MinValue).Sum(Function(b) b.SKUCost))
            End If
            AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                       cellValue, newExcelRow, CellValues.Number, styleIndex)
        End If

        'Awaiting Copy # Styles/Colors
        columnIndex = columnIndex + 1
        cellValue = weeklySKUDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue AndAlso a.CopyReadyDate <=
                                                 Date.MinValue).Sum(Function(b) b.Quantity)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total Awaiting Copy # Styles/Colors
        columnIndex = columnIndex + 1
        If String.IsNullOrEmpty(gmmName) Then
            cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                                                                 AndAlso a.CopyReadyDate <=
                                                                                 Date.MinValue).Sum(Function(b) b.Quantity),
                skuDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue AndAlso a.CopyReadyDate <=
                                       Date.MinValue).Sum(Function(b) b.Quantity))
        Else
            cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                                                     AndAlso a.CopyReadyDate <=
                                                                     Date.MinValue).Sum(Function(b) b.Quantity),
                skuDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue AndAlso a.CopyReadyDate <=
                           Date.MinValue _
                           AndAlso a.GMM = gmmName).Sum(Function(b) b.Quantity))

        End If
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total Awaiting Copy # Styles/Colors
        If includeTotalCategoryValues Then
            columnIndex = columnIndex + 1
            If String.IsNullOrEmpty(gmmName) Then
                cellValue = 100
            Else
                cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                                                     AndAlso a.CopyReadyDate <=
                                                                     Date.MinValue).Sum(Function(b) b.Quantity),
                skuDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue AndAlso a.CopyReadyDate <=
                           Date.MinValue).Sum(Function(b) b.Quantity))

            End If
            AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                       cellValue, newExcelRow, CellValues.Number, styleIndex)
        End If

        'Awaiting Copy $ Styles/Colors
        columnIndex = columnIndex + 1
        cellValue = weeklySKUDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue AndAlso a.CopyReadyDate <=
                                                 Date.MinValue).Sum(Function(b) b.SKUCost)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total Awaiting Copy $ Styles/Colors
        columnIndex = columnIndex + 1
        If String.IsNullOrEmpty(gmmName) Then
            cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                                                                 AndAlso a.CopyReadyDate <=
                                                                                 Date.MinValue).Sum(Function(b) b.SKUCost),
                skuDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue AndAlso a.CopyReadyDate <= Date.MinValue).Sum(Function(b) b.SKUCost))
        Else
            cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                                                                 AndAlso a.CopyReadyDate <=
                                                                                 Date.MinValue).Sum(Function(b) b.SKUCost),
                skuDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                       AndAlso a.CopyReadyDate <= Date.MinValue _
                                       AndAlso a.GMM = gmmName).Sum(Function(b) b.SKUCost))
        End If
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                   cellValue, newExcelRow, CellValues.Number, styleIndex)

        '% to total Awaiting Copy $ Styles/Colors
        If includeTotalCategoryValues Then
            columnIndex = columnIndex + 1
            If String.IsNullOrEmpty(gmmName) Then
                cellValue = 100
            Else
                cellValue = GetPercentageFromTwoNumbers(weeklySKUDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue _
                                                                                     AndAlso a.CopyReadyDate <=
                                                                                     Date.MinValue).Sum(Function(b) b.SKUCost),
                    skuDetails.FindAll(Function(a) a.TurnInDate > Date.MinValue AndAlso a.CopyReadyDate <= Date.MinValue).Sum(Function(b) b.SKUCost))
            End If
            AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(),
                       cellValue, newExcelRow, CellValues.Number, styleIndex)
        End If


    End Sub

    Private Shared Sub WritePathToSiteAdStatusListToExcelWorksheet(ByVal ohReceiptDetails As List(Of SKUDetail),
                                                                   ByVal worksheetPart As WorksheetPart,
                                                 ByVal worksheet As Worksheet,
                                                 ByVal sheetData As SheetData,
                                                 ByVal skuDetails As List(Of SKUDetail),
                                                 ByVal photoShootDays As Integer,
                                                 ByVal imageProductionDays As Integer,
                                                 ByVal fiscalWeek As Integer,
                                                 ByVal fiscalMonth As Integer,
                                                 ByVal fiscalYear As Integer)

        Dim cellValue As String = String.Empty
        Dim columnIndex As Integer = 0

        Dim dateTimeInfo As DateTimeFormatInfo = Nothing
        Dim cal As Calendar = Nothing
        Dim currentMonthStartDate As Date = Nothing
        Dim currentMonthEndDate As Date = Nothing
        Dim nextMonthStartDate As Date = Nothing
        Dim nextMonthEndDate As Date = Nothing
        Dim ooADLevelDetails As List(Of SKUDetail) = Nothing
        Dim ohADLevelDetails As List(Of SKUDetail) = Nothing
        Dim adDetail As SKUDetail = Nothing

        Dim thisWeekStartDate As Date = Nothing
        Dim thisWeekEndDate As Date = Nothing

        '  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
        '
        '  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
        '  cells of data, we'll know if to write Text values or Numeric cell values.
        Dim numberOfColumns As Integer = 60
        Dim IsNumericColumn(numberOfColumns) As Boolean

        Dim excelColumnNames([numberOfColumns]) As String

        For n As Integer = 0 To numberOfColumns
            excelColumnNames(numberOfColumns) = GetExcelColumnName(n)
        Next n

        'Get datetime info
        dateTimeInfo = DateTimeFormatInfo.CurrentInfo
        cal = dateTimeInfo.Calendar
        'fiscalWeek = cal.GetWeekOfYear(Date.Today(),
        '                               dateTimeInfo.CalendarWeekRule, dateTimeInfo.FirstDayOfWeek)
        thisWeekStartDate = Date.Today().AddDays(-DirectCast(Date.Today().DayOfWeek, Integer))
        thisWeekEndDate = thisWeekStartDate.AddDays(7)

        '
        '  Create the Header row in our Excel Worksheet
        '
        Dim rowIndex As UInt32 = 1

        'Append Title
        Dim titleRow As Row = New Row
        titleRow.RowIndex = rowIndex            ' add the title at the top of spreadsheet
        titleRow = GetPathToSiteAdStatusReportTitle(fiscalYear, fiscalMonth, fiscalWeek)
        sheetData.Append(titleRow)

        rowIndex = 3
        'Append OH Locations
        Dim ohLocationsRow As Row = New Row
        ohLocationsRow.RowIndex = rowIndex            ' add the headers at the top of spreadsheet
        ohLocationsRow.Append(AddHeaderColumn("ON HAND - 192,193,195", "D3"))
        columnIndex = 3
        For index = 1 To 23
            columnIndex = columnIndex + 1
            ohLocationsRow.Append(AddHeaderColumn("", GetExcelColumnName(columnIndex)))
        Next
        ohLocationsRow.Append(AddHeaderColumn("ON ORDER - 192,193,195", "AC3"))
        columnIndex = 29
        For index = 1 To 23
            columnIndex = columnIndex + 1
            ohLocationsRow.Append(AddHeaderColumn("", GetExcelColumnName(columnIndex)))
        Next
        sheetData.Append(ohLocationsRow)

        rowIndex = rowIndex + 1
        'Append OH section Headers
        Dim ohSectionHeaders As Row = New Row
        ohSectionHeaders.RowIndex = rowIndex            ' add the headers at the top of spreadsheet
        GetPathToSiteMerchantSectionHeaders(ohSectionHeaders, True, 3, 3)
        GetPathToSiteMerchantSectionHeaders(ohSectionHeaders, True, 1, 3)
        sheetData.Append(ohSectionHeaders)

        rowIndex = rowIndex + 1
        'Append OH Headers
        Dim headerRow As Row = New Row
        headerRow.RowIndex = rowIndex            ' add the headers at the top of spreadsheet
        headerRow = GetPathToSiteAdStatusReportHeaders(True)
        sheetData.Append(headerRow)

        skuDetails = skuDetails.FindAll(Function(x) x.AdJobStepDueDate >
                                            Date.MinValue).OrderBy(Function(a) a.AdJobStepDueDate).ToList()

        'Old ads
        Dim distinctAdNumbers = skuDetails.FindAll(Function(a) a.AdNumber > 0 _
                                                       AndAlso a.AdJobStepDueDate > Date.MinValue _
                                                       AndAlso a.AdJobStepDueDate > Date.Today().AddYears(-1) _
                                                       AndAlso a.AdJobStepDueDate <=
                                                       thisWeekStartDate).Select(Function(b) b.AdNumber).Distinct().ToList()
        ExportAdLevelData(sheetData, skuDetails, excelColumnNames, rowIndex,
                          columnIndex, fiscalWeek, thisWeekStartDate, distinctAdNumbers,
                          photoShootDays, imageProductionDays,
                          False, String.Empty, 9)

        'This week ads
        Dim distinctCurrentAdNumbers = skuDetails.FindAll(Function(a) a.AdNumber > 0 _
                                                              AndAlso a.AdJobStepDueDate > Date.MinValue _
                                                              AndAlso a.AdJobStepDueDate >= thisWeekStartDate _
                                                              AndAlso a.AdJobStepDueDate <=
                                                              thisWeekEndDate).Select(Function(b) b.AdNumber).Distinct().ToList()
        If Not distinctCurrentAdNumbers Is Nothing AndAlso distinctCurrentAdNumbers.Count > 0 Then
            ExportAdLevelData(sheetData, skuDetails, excelColumnNames, rowIndex, columnIndex,
                              fiscalWeek, thisWeekStartDate, distinctCurrentAdNumbers,
                              photoShootDays, imageProductionDays, True,
                              "** current turn in week **", 10)
        End If

        'Future week ads
        Dim distinctFutureAdNumbers = skuDetails.FindAll(Function(a) a.AdNumber > 0 _
                                                              AndAlso a.AdJobStepDueDate > Date.MinValue _
                                                             AndAlso a.AdJobStepDueDate >
                                                             thisWeekEndDate).Select(Function(b) b.AdNumber).Distinct().ToList()
        If Not distinctFutureAdNumbers Is Nothing AndAlso distinctFutureAdNumbers.Count > 0 Then
            ExportAdLevelData(sheetData, skuDetails, excelColumnNames, rowIndex, columnIndex,
                              fiscalWeek, thisWeekStartDate, distinctFutureAdNumbers,
                              photoShootDays, imageProductionDays,
                              True, "** future turn in weeks **", 11)
        End If

        worksheet.Append(sheetData)
        worksheetPart.Worksheet = worksheet

        MergeTwoCells(worksheet, "A1", "B1")
        MergeTwoCells(worksheet, "D3", "AA3")
        MergeTwoCells(worksheet, "D4", "G4")
        MergeTwoCells(worksheet, "H4", "K4")
        MergeTwoCells(worksheet, "L4", "O4")
        MergeTwoCells(worksheet, "P4", "S4")
        MergeTwoCells(worksheet, "T4", "W4")
        MergeTwoCells(worksheet, "X4", "AA4")
        MergeTwoCells(worksheet, "AC4", "AF4")
        MergeTwoCells(worksheet, "AG4", "AJ4")
        MergeTwoCells(worksheet, "AK4", "AN4")
        MergeTwoCells(worksheet, "AO4", "AR4")
        MergeTwoCells(worksheet, "AS4", "AV4")
        MergeTwoCells(worksheet, "AW4", "AZ4")
        MergeTwoCells(worksheet, "AC3", "AZ3")
    End Sub

    Private Shared Sub ExportAdLevelData(ByRef sheetData As SheetData,
                                                 ByVal skuDetails As List(Of SKUDetail),
                                                 ByVal excelColumnNames() As String,
                                                 ByRef rowIndex As Integer,
                                                 ByVal columnIndex As Integer,
                                                 ByVal fiscalWeek As Integer,
                                                 ByVal thisWeekStartDate As Date,
                                         ByVal distinctAdNumbers As List(Of Integer),
                                         ByVal photoShootDays As Integer,
                                         ByVal imageProductionDays As Integer,
                                         Optional ByVal addBlankRow As Boolean = False,
                                         Optional ByVal blankRowHeader As String = "",
                                         Optional ByVal styleIndex As Integer = 0)

        Dim cellValue As String = String.Empty
        Dim ooADLevelDetails As List(Of SKUDetail) = Nothing
        Dim ohADLevelDetails As List(Of SKUDetail) = Nothing
        Dim adDetail As SKUDetail = Nothing

        If addBlankRow Then
            'Append blank row
            rowIndex = rowIndex + 1
            Dim blankRow As Row = New Row
            blankRow.RowIndex = rowIndex
            AppendCell(excelColumnNames(0) + rowIndex.ToString(), blankRowHeader, blankRow, CellValues.String)
            sheetData.Append(blankRow)
        End If

        For Each adNum As Integer In distinctAdNumbers
            ohADLevelDetails = skuDetails.FindAll(Function(a) a.AdNumber = adNum _
                                                      AndAlso a.AdJobStepDueDate > Date.MinValue _
                                                      AndAlso a.ReportItemType = "OH")
            ooADLevelDetails = skuDetails.FindAll(Function(a) a.AdNumber = adNum _
                                                      AndAlso a.AdJobStepDueDate > Date.MinValue _
                                                      AndAlso a.ReportItemType = "OO")
            adDetail = Nothing

            If (Not ooADLevelDetails Is Nothing AndAlso ooADLevelDetails.Count > 0) OrElse
                (Not ohADLevelDetails Is Nothing AndAlso ohADLevelDetails.Count > 0) Then

                If Not ooADLevelDetails Is Nothing AndAlso ooADLevelDetails.Count > 0 Then
                    adDetail = ooADLevelDetails.Item(0)
                End If

                If adDetail Is Nothing AndAlso Not ohADLevelDetails Is Nothing AndAlso ohADLevelDetails.Count > 0 Then
                    adDetail = ohADLevelDetails.Item(0)
                End If

                If Not adDetail Is Nothing Then
                    ' ...create a new row, and append a set of this row's data to it.
                    rowIndex = rowIndex + 1
                    Dim newExcelRow As New Row
                    newExcelRow.RowIndex = rowIndex         '  add a row at the top of spreadsheet
                    sheetData.Append(newExcelRow)

                    'Turn-in week
                    columnIndex = 0
                    cellValue = System.Math.Round(fiscalWeek +
                                                  (DateDiff(DateInterval.Day, thisWeekStartDate, adDetail.AdJobStepDueDate,
                                                            FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 7))
                    AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, styleIndex)

                    'Ad Number
                    columnIndex = columnIndex + 1
                    cellValue = adNum
                    AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number, styleIndex)

                    'Ad Type
                    columnIndex = columnIndex + 1
                    cellValue = adDetail.AdType
                    AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String, styleIndex)

                    GetCommonFieldsValues(skuDetails.FindAll(Function(a) a.AdNumber = adNum AndAlso a.ReportItemType = "OH"),
                                          skuDetails, excelColumnNames, newExcelRow, rowIndex, columnIndex,
                                          photoShootDays, imageProductionDays,
                                          False, False, String.Empty, styleIndex)

                    GetCommonFieldsValues(skuDetails.FindAll(Function(a) a.AdNumber = adNum AndAlso a.ReportItemType = "OO"),
                                          skuDetails, excelColumnNames, newExcelRow, rowIndex, columnIndex,
                                          photoShootDays, imageProductionDays,
                                          True, False, String.Empty, styleIndex)
                End If

            End If
        Next

    End Sub

    Private Shared Function GetPathToSiteAdStatusReportTitle(ByVal fiscalYear As Integer,
                                                             ByVal fiscalMonth As Integer,
                                                             ByVal fiscalWeek As Integer) As Row
        Dim excelRow As Row = Nothing
        Dim excelCell As Cell = Nothing
        Dim columnCount As Integer = 0
        Dim dateTimeInfo As DateTimeFormatInfo = Nothing
        Dim cal As Calendar = Nothing

        'Get datetime info
        dateTimeInfo = DateTimeFormatInfo.CurrentInfo
        cal = dateTimeInfo.Calendar
        fiscalWeek = cal.GetWeekOfYear(Date.Today(), dateTimeInfo.CalendarWeekRule, dateTimeInfo.FirstDayOfWeek)

        excelRow = New Row()

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Path to Site Ad Number Status", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Report Date", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn(Date.Today().ToShortDateString(), GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Fiscal Year", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn(fiscalYear, GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Fiscal Month", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn(fiscalMonth, GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Fiscal Week", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn(fiscalWeek, GetExcelColumnName(columnCount)))

        Return excelRow
    End Function

    Private Shared Function GetPathToSiteAdStatusReportHeaders(ByVal isOH As Boolean) As Row
        Dim excelRow As Row = Nothing
        Dim excelCell As Cell = Nothing
        Dim headerString As InlineString = Nothing
        Dim cellText As InlineString = Nothing
        Dim cellValue As CellValue = Nothing
        Dim runProp As RunProperties = Nothing
        Dim runCell As Run = Nothing
        Dim columnCount As Integer = 0
        excelRow = New Row()

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Turn In Week", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Ad Number", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Ad Type", GetExcelColumnName(columnCount)))

        GetCommonReportHeaders(excelRow, columnCount)

        columnCount += 1
        excelRow.Append(AddHeaderColumn("", GetExcelColumnName(columnCount)))

        GetCommonReportHeaders(excelRow, columnCount)

        Return excelRow
    End Function

    Private Shared Sub GetCommonReportHeaders(ByRef excelRow As Row,
                                              ByVal columnCount As Integer, Optional ByVal needTotalHeaders As Boolean = False)
        columnCount += 1
        excelRow.Append(AddHeaderColumn("Not Turned in # Styles/Colors", GetExcelColumnName(columnCount), 4))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("% to ttl  OH FC # Styles/Colors not Turned in", GetExcelColumnName(columnCount), 4))

        If needTotalHeaders Then
            columnCount += 1
            excelRow.Append(AddHeaderColumn("% to ttl # Styles/Colors not Turned in", GetExcelColumnName(columnCount), 4))
        End If

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Not Turned in $ Styles/Colors", GetExcelColumnName(columnCount), 5))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("% to ttl OH FC $ Styles/Colors not Turned in", GetExcelColumnName(columnCount), 5))

        If needTotalHeaders Then
            columnCount += 1
            excelRow.Append(AddHeaderColumn("% to ttl $ Styles/Colors not Turned in", GetExcelColumnName(columnCount), 5))
        End If

        columnCount += 1
        excelRow.Append(AddHeaderColumn("IN CMR # Style/colors", GetExcelColumnName(columnCount), 4))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("% to TTL In CMR # Style/colors", GetExcelColumnName(columnCount), 4))

        If needTotalHeaders Then
            columnCount += 1
            excelRow.Append(AddHeaderColumn("% to TTL  In CMR # Style/colors", GetExcelColumnName(columnCount), 4))
        End If

        columnCount += 1
        excelRow.Append(AddHeaderColumn("IN CMR $ Style/colors", GetExcelColumnName(columnCount), 5))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("% to TTL In CMR $ Style/colors", GetExcelColumnName(columnCount), 5))

        If needTotalHeaders Then
            columnCount += 1
            excelRow.Append(AddHeaderColumn("% to TTL In CMR $ Style/colors", GetExcelColumnName(columnCount), 5))
        End If

        columnCount += 1
        excelRow.Append(AddHeaderColumn("At Studio # Style/colors", GetExcelColumnName(columnCount), 4))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("% to TTL At Studio # Style/colors", GetExcelColumnName(columnCount), 4))

        If needTotalHeaders Then
            columnCount += 1
            excelRow.Append(AddHeaderColumn("% to TTL At Studio # Style/colors", GetExcelColumnName(columnCount), 4))
        End If

        columnCount += 1
        excelRow.Append(AddHeaderColumn("AT STUDIO  $Style/colors", GetExcelColumnName(columnCount), 5))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("% to TTL AT STUDIO $ Style/colors", GetExcelColumnName(columnCount), 5))

        If needTotalHeaders Then
            columnCount += 1
            excelRow.Append(AddHeaderColumn("% to TTL At Studio $ Style/colors", GetExcelColumnName(columnCount), 5))
        End If

        columnCount += 1
        excelRow.Append(AddHeaderColumn("In Image Production  Style/colors #", GetExcelColumnName(columnCount), 4))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("In Image Production % to ttl OH FC # Style/Colors", GetExcelColumnName(columnCount), 4))

        If needTotalHeaders Then
            columnCount += 1
            excelRow.Append(AddHeaderColumn("% to ttl In Image Production # Style/Colors", GetExcelColumnName(columnCount), 4))
        End If

        columnCount += 1
        excelRow.Append(AddHeaderColumn(" In Image Production Style/colors $", GetExcelColumnName(columnCount), 5))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("In Image Production % to ttl OH FC $   Style/Colors", GetExcelColumnName(columnCount), 5))

        If needTotalHeaders Then
            columnCount += 1
            excelRow.Append(AddHeaderColumn("% to ttl In Image Production $ Style/Colors", GetExcelColumnName(columnCount), 5))
        End If

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Awaiting Premedia Style/colors", GetExcelColumnName(columnCount), 4))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("% to ttl OH FC # awaiting Premedia style/ color", GetExcelColumnName(columnCount), 4))

        If needTotalHeaders Then
            columnCount += 1
            excelRow.Append(AddHeaderColumn("% to ttl  # awaiting Premedia style/ color", GetExcelColumnName(columnCount), 4))
        End If

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Awaiting Premedia Style/colors $", GetExcelColumnName(columnCount), 5))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("% to TTLOH FC $ awaiting Premedia style/ color", GetExcelColumnName(columnCount), 5))

        If needTotalHeaders Then
            columnCount += 1
            excelRow.Append(AddHeaderColumn("% to TTL $ awaiting Premedia style/ color", GetExcelColumnName(columnCount), 5))
        End If

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Awaiting copy Products #", GetExcelColumnName(columnCount), 6))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("% to ttl OH FC # awaiting copy", GetExcelColumnName(columnCount), 6))

        If needTotalHeaders Then
            columnCount += 1
            excelRow.Append(AddHeaderColumn("% to ttl  # awaiting copy", GetExcelColumnName(columnCount), 6))
        End If

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Awaiting Copy Products $", GetExcelColumnName(columnCount), 4))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("% to ttl OH FC $ awaiting copy Products", GetExcelColumnName(columnCount), 4))

        If needTotalHeaders Then
            columnCount += 1
            excelRow.Append(AddHeaderColumn("% to ttl  $ awaiting copy Products", GetExcelColumnName(columnCount), 4))
        End If

    End Sub

    Private Shared Sub WritePathToSiteMerchantStatusGMMListToExcelWorksheet(ByVal ohReceiptDetails As List(Of SKUDetail),
                                                                   ByVal worksheetPart As WorksheetPart,
                                                 ByVal worksheet As Worksheet,
                                                 ByVal sheetData As SheetData,
                                                 ByVal skuDetails As List(Of SKUDetail),
                                                 ByVal photoShootDays As Integer,
                                                 ByVal imageProductionDays As Integer,
                                                 ByVal fiscalWeek As Integer,
                                                 ByVal fiscalMonth As Integer,
                                                 ByVal fiscalYear As Integer)

        Dim cellValue As String = String.Empty
        Dim columnIndex As Integer = 0

        Dim dateTimeInfo As DateTimeFormatInfo = Nothing
        Dim cal As Calendar = Nothing
        Dim currentMonthStartDate As Date = Nothing
        Dim currentMonthEndDate As Date = Nothing
        Dim nextMonthStartDate As Date = Nothing
        Dim nextMonthEndDate As Date = Nothing
        Dim nextToNextMonthStartDate As Date = Nothing
        Dim nextToNextMonthEndDate As Date = Nothing
        Dim gmmLevelDetails As List(Of SKUDetail) = Nothing
        Dim ooHeaderRowIndex As Integer = 0
        Dim fiscalDataDAO As FiscalDataDAO = Nothing

        '  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
        '
        '  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
        '  cells of data, we'll know if to write Text values or Numeric cell values.
        Dim numberOfColumns As Integer = 42
        Dim IsNumericColumn(numberOfColumns) As Boolean

        Dim excelColumnNames([numberOfColumns]) As String

        For n As Integer = 0 To numberOfColumns
            excelColumnNames(numberOfColumns) = GetExcelColumnName(n)
        Next n

        'Get datetime info
        dateTimeInfo = DateTimeFormatInfo.CurrentInfo
        cal = dateTimeInfo.Calendar
        'fiscalWeek = cal.GetWeekOfYear(Date.Today(), dateTimeInfo.CalendarWeekRule, dateTimeInfo.FirstDayOfWeek)

        '
        '  Create the Header row in our Excel Worksheet
        '
        Dim rowIndex As UInt32 = 1

        'Append Title
        Dim titleRow As Row = New Row
        titleRow.RowIndex = rowIndex            ' add the title at the top of spreadsheet
        titleRow = GetPathToSiteMerchantReportTitle()
        sheetData.Append(titleRow)

        rowIndex = rowIndex + 1
        'Append OH Locations
        Dim ohLocationsRow As Row = New Row
        ohLocationsRow.RowIndex = rowIndex            ' add the headers at the top of spreadsheet
        ohLocationsRow.Append(AddHeaderColumn("ON HAND - 192,193,195", "G2"))
        columnIndex = 6
        For index = 1 To 35
            columnIndex = columnIndex + 1
            ohLocationsRow.Append(AddHeaderColumn("", GetExcelColumnName(columnIndex)))
        Next
        sheetData.Append(ohLocationsRow)

        rowIndex = rowIndex + 1
        'Append OH section Headers
        Dim ohSectionHeaders As Row = New Row
        ohSectionHeaders.RowIndex = rowIndex            ' add the headers at the top of spreadsheet
        GetPathToSiteMerchantSectionHeaders(ohSectionHeaders, True, 6, 5)
        sheetData.Append(ohSectionHeaders)

        rowIndex = rowIndex + 1
        'Append OH Headers
        Dim headerRow As Row = New Row
        headerRow.RowIndex = rowIndex            ' add the headers at the top of spreadsheet
        headerRow = GetPathToSiteMerchantReportHeaders(True)
        sheetData.Append(headerRow)

        rowIndex = rowIndex + 1
        Dim newExcelRow As New Row
        newExcelRow.RowIndex = rowIndex         '  add a row at the top of spreadsheet
        sheetData.Append(newExcelRow)

        'Fiscal Year
        columnIndex = 0
        cellValue = fiscalYear
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

        'Fiscal Month
        columnIndex = columnIndex + 1
        cellValue = fiscalMonth
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

        'Fiscal Week
        columnIndex = columnIndex + 1
        cellValue = fiscalWeek
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

        'FC OH $
        columnIndex = columnIndex + 1
        cellValue = skuDetails.FindAll(Function(a) a.ReportItemType = "OH").Sum(Function(b) b.SKUCost)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

        'FC Received $
        columnIndex = columnIndex + 1
        cellValue = ohReceiptDetails.Sum(Function(a) a.SKUCost)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.Number)

        GetCommonFieldsValues(ohReceiptDetails, skuDetails, excelColumnNames, newExcelRow, rowIndex,
                              columnIndex, photoShootDays, imageProductionDays,
                              True, True)

        Dim distinctGMM = skuDetails.OrderBy(Function(a) a.GMM).Select(Function(b) b.GMM).Distinct().ToList()

        For Each gmm As String In distinctGMM
            gmmLevelDetails = ohReceiptDetails.FindAll(Function(a) a.GMM = gmm)

            If Not gmmLevelDetails Is Nothing AndAlso gmmLevelDetails.Count > 0 Then
                rowIndex = rowIndex + 1
                Dim gmmLevelRow As Row = New Row
                gmmLevelRow.RowIndex = rowIndex
                sheetData.Append(gmmLevelRow)

                AddBlankColumns(excelColumnNames, gmmLevelRow, rowIndex, columnIndex, 5)

                'Append GMM name
                columnIndex = 5
                cellValue = gmm
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, gmmLevelRow, CellValues.String)

                GetCommonFieldsValues(gmmLevelDetails, skuDetails.FindAll(Function(a) a.ReportItemType = "OH"),
                                      excelColumnNames, gmmLevelRow, rowIndex,
                                      columnIndex, photoShootDays, imageProductionDays,
                                      False, True, gmm)
            End If
        Next

        rowIndex = rowIndex + 8

        'Append OO Locations
        Dim ooLocationsRow As Row = New Row
        ooLocationsRow.RowIndex = rowIndex
        ooLocationsRow.Append(AddHeaderColumn("ON ORDER - 192,193,195", String.Concat("G", rowIndex.ToString())))
        columnIndex = 6
        For index = 1 To 35
            columnIndex = columnIndex + 1
            ooLocationsRow.Append(AddHeaderColumn("", GetExcelColumnName(columnIndex)))
        Next
        sheetData.Append(ooLocationsRow)

        ooHeaderRowIndex = rowIndex

        rowIndex = rowIndex + 1
        'Append OH section Headers
        Dim ooSectionHeaders As Row = New Row
        ooSectionHeaders.RowIndex = rowIndex
        GetPathToSiteMerchantSectionHeaders(ooSectionHeaders, True, 6, 5)
        sheetData.Append(ooSectionHeaders)

        rowIndex = rowIndex + 1
        'Append OO Headers
        Dim ooHeaderRow As Row = New Row
        ooHeaderRow.RowIndex = rowIndex
        ooHeaderRow = GetPathToSiteMerchantReportHeaders(False)
        sheetData.Append(ooHeaderRow)

        rowIndex = rowIndex + 1
        Dim firstFiscalWeekRow As New Row
        firstFiscalWeekRow.RowIndex = rowIndex
        sheetData.Append(firstFiscalWeekRow)

        'Fiscal Year
        columnIndex = 0
        cellValue = fiscalYear
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, firstFiscalWeekRow, CellValues.Number)

        'Fiscal Month
        columnIndex = columnIndex + 1
        cellValue = fiscalMonth
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, firstFiscalWeekRow, CellValues.Number)

        'Fiscal Week
        columnIndex = columnIndex + 1
        cellValue = fiscalWeek
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, firstFiscalWeekRow, CellValues.Number)

        currentMonthStartDate = New DateTime(Date.Today().Year, Date.Today().Month, 1)
        currentMonthEndDate = currentMonthStartDate.AddMonths(1).AddDays(-1)

        'Current Month OO $
        columnIndex = columnIndex + 1
        cellValue = skuDetails.FindAll(Function(a) a.ReportItemType = "OO" _
                                           AndAlso a.POShipDate >= currentMonthStartDate _
                                           AndAlso a.POShipDate <= currentMonthEndDate).Sum(Function(b) b.SKUCost)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, firstFiscalWeekRow, CellValues.Number)

        nextMonthStartDate = New DateTime(Date.Today().Year, Date.Today().AddMonths(1).Month, 1)
        nextMonthEndDate = nextMonthStartDate.AddMonths(1).AddDays(-1)

        'Next Month OO $
        columnIndex = columnIndex + 1
        cellValue = skuDetails.FindAll(Function(a) a.ReportItemType = "OO" _
                                           AndAlso a.POShipDate >= nextMonthStartDate _
                                           AndAlso a.POShipDate <= nextMonthEndDate).Sum(Function(b) b.SKUCost)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, firstFiscalWeekRow, CellValues.Number)

        GetCommonFieldsValues(skuDetails.FindAll(Function(a) a.ReportItemType = "OO" _
                                           AndAlso a.POShipDate >= currentMonthStartDate _
                                           AndAlso a.POShipDate <= currentMonthEndDate),
                              skuDetails.FindAll(Function(a) a.ReportItemType = "OO"),
                              excelColumnNames, firstFiscalWeekRow, rowIndex,
                              columnIndex,
                              photoShootDays, imageProductionDays,
                              True, True)

        For Each gmm As String In distinctGMM
            gmmLevelDetails = skuDetails.FindAll(Function(a) a.ReportItemType = "OO" _
                                           AndAlso a.POShipDate >= currentMonthStartDate _
                                           AndAlso a.POShipDate <= currentMonthEndDate _
                                           AndAlso a.GMM = gmm)

            If Not gmmLevelDetails Is Nothing AndAlso gmmLevelDetails.Count > 0 Then
                rowIndex = rowIndex + 1
                Dim gmmLevelRow As Row = New Row
                gmmLevelRow.RowIndex = rowIndex
                sheetData.Append(gmmLevelRow)

                AddBlankColumns(excelColumnNames, gmmLevelRow, rowIndex, columnIndex, 5)

                'Append GMM name
                columnIndex = 5
                cellValue = gmm
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, gmmLevelRow, CellValues.String)

                GetCommonFieldsValues(gmmLevelDetails, skuDetails.FindAll(Function(a) a.ReportItemType = "OO"),
                                      excelColumnNames, gmmLevelRow, rowIndex,
                                      columnIndex, photoShootDays, imageProductionDays,
                                      False, True, gmm)
            End If
        Next

        'Second Fiscal Month Row
        rowIndex = rowIndex + 3
        Dim secondFiscalWeekRow As New Row
        secondFiscalWeekRow.RowIndex = rowIndex
        sheetData.Append(secondFiscalWeekRow)

        'Fiscal Year
        columnIndex = 0
        cellValue = String.Empty
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, secondFiscalWeekRow, CellValues.Number)

        'FiscaDl Month
        columnIndex = columnIndex + 1
        fiscalDataDAO = New FiscalDataDAO()
        cellValue = fiscalDataDAO.GetFiscalMonthByDate(Date.Today().AddMonths(1).Date)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, secondFiscalWeekRow, CellValues.Number)

        'Fiscal Week
        columnIndex = columnIndex + 1
        cellValue = String.Empty
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, secondFiscalWeekRow, CellValues.Number)

        columnIndex = columnIndex + 1
        cellValue = String.Empty
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, secondFiscalWeekRow, CellValues.Number)

        columnIndex = columnIndex + 1
        cellValue = String.Empty
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, secondFiscalWeekRow, CellValues.Number)

        GetCommonFieldsValues(skuDetails.FindAll(Function(a) a.ReportItemType = "OO" _
                                           AndAlso a.POShipDate >= nextMonthStartDate _
                                           AndAlso a.POShipDate <= nextMonthEndDate),
                              skuDetails.FindAll(Function(a) a.ReportItemType = "OO"),
                              excelColumnNames, secondFiscalWeekRow, rowIndex,
                              columnIndex, photoShootDays, imageProductionDays,
                              True, True)

        For Each gmm As String In distinctGMM
            gmmLevelDetails = skuDetails.FindAll(Function(a) a.ReportItemType = "OO" _
                                           AndAlso a.POShipDate >= nextMonthStartDate _
                                           AndAlso a.POShipDate <= nextMonthEndDate _
                                           AndAlso a.GMM = gmm)

            If Not gmmLevelDetails Is Nothing AndAlso gmmLevelDetails.Count > 0 Then
                rowIndex = rowIndex + 1
                Dim gmmLevelRow As Row = New Row
                gmmLevelRow.RowIndex = rowIndex
                sheetData.Append(gmmLevelRow)

                AddBlankColumns(excelColumnNames, gmmLevelRow, rowIndex, columnIndex, 5)

                'Append GMM name
                columnIndex = 5
                cellValue = gmm
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, gmmLevelRow, CellValues.String)

                GetCommonFieldsValues(gmmLevelDetails, skuDetails.FindAll(Function(a) a.ReportItemType = "OO"),
                                      excelColumnNames, gmmLevelRow, rowIndex,
                                      columnIndex, photoShootDays, imageProductionDays,
                                      False, True, gmm)
            End If
        Next

        'Third Fiscal Month Row
        rowIndex = rowIndex + 3
        Dim thirdFiscalWeekRow As New Row
        thirdFiscalWeekRow.RowIndex = rowIndex         '  add a row at the top of spreadsheet
        sheetData.Append(thirdFiscalWeekRow)

        nextToNextMonthStartDate = New DateTime(Date.Today().Year, Date.Today().AddMonths(2).Month, 1)
        nextToNextMonthEndDate = nextMonthStartDate.AddMonths(2).AddDays(-1)

        'Fiscal Year
        columnIndex = 0
        cellValue = String.Empty
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, thirdFiscalWeekRow, CellValues.Number)

        'Fiscal Month
        columnIndex = columnIndex + 1
        cellValue = fiscalDataDAO.GetFiscalMonthByDate(Date.Today().AddMonths(2).Date)
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, thirdFiscalWeekRow, CellValues.Number)

        'Fiscal Week
        columnIndex = columnIndex + 1
        cellValue = String.Empty
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, thirdFiscalWeekRow, CellValues.Number)

        columnIndex = columnIndex + 1
        cellValue = String.Empty
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, thirdFiscalWeekRow, CellValues.Number)

        columnIndex = columnIndex + 1
        cellValue = String.Empty
        AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, thirdFiscalWeekRow, CellValues.Number)

        GetCommonFieldsValues(skuDetails.FindAll(Function(a) a.ReportItemType = "OO" _
                                           AndAlso a.POShipDate >= nextToNextMonthStartDate _
                                           AndAlso a.POShipDate <= nextToNextMonthEndDate),
                              skuDetails.FindAll(Function(a) a.ReportItemType = "OO"),
                              excelColumnNames, thirdFiscalWeekRow, rowIndex,
                              columnIndex, photoShootDays, imageProductionDays,
                              True, True)

        For Each gmm As String In distinctGMM
            gmmLevelDetails = skuDetails.FindAll(Function(a) a.ReportItemType = "OO" _
                                           AndAlso a.POShipDate >= nextToNextMonthStartDate _
                                           AndAlso a.POShipDate <= nextToNextMonthEndDate _
                                           AndAlso a.GMM = gmm)

            If Not gmmLevelDetails Is Nothing AndAlso gmmLevelDetails.Count > 0 Then
                rowIndex = rowIndex + 1
                Dim gmmLevelRow As Row = New Row
                gmmLevelRow.RowIndex = rowIndex
                sheetData.Append(gmmLevelRow)

                AddBlankColumns(excelColumnNames, gmmLevelRow, rowIndex, columnIndex, 5)

                'Append GMM name
                columnIndex = 5
                cellValue = gmm
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, gmmLevelRow, CellValues.String)

                GetCommonFieldsValues(gmmLevelDetails, skuDetails.FindAll(Function(a) a.ReportItemType = "OO"),
                                      excelColumnNames, gmmLevelRow, rowIndex,
                                      columnIndex, photoShootDays, imageProductionDays,
                                      False, True, gmm)
            End If
        Next

        worksheet.Append(sheetData)
        worksheetPart.Worksheet = worksheet

        MergeTwoCells(worksheet, "A1", "B1")
        MergeTwoCells(worksheet, "G2", "AP2")
        MergeTwoCells(worksheet, "G3", "L3")
        MergeTwoCells(worksheet, "M3", "R3")
        MergeTwoCells(worksheet, "S3", "X3")
        MergeTwoCells(worksheet, "Y3", "AD3")
        MergeTwoCells(worksheet, "AE3", "AJ3")
        MergeTwoCells(worksheet, "AK3", "AP3")
        MergeTwoCells(worksheet, String.Concat("G", ooHeaderRowIndex.ToString()), String.Concat("AP", ooHeaderRowIndex.ToString()))
        ooHeaderRowIndex = ooHeaderRowIndex + 1
        MergeTwoCells(worksheet, String.Concat("G", ooHeaderRowIndex.ToString()), String.Concat("L", ooHeaderRowIndex.ToString()))
        MergeTwoCells(worksheet, String.Concat("M", ooHeaderRowIndex.ToString()), String.Concat("R", ooHeaderRowIndex.ToString()))
        MergeTwoCells(worksheet, String.Concat("S", ooHeaderRowIndex.ToString()), String.Concat("X", ooHeaderRowIndex.ToString()))
        MergeTwoCells(worksheet, String.Concat("Y", ooHeaderRowIndex.ToString()), String.Concat("AD", ooHeaderRowIndex.ToString()))
        MergeTwoCells(worksheet, String.Concat("AE", ooHeaderRowIndex.ToString()), String.Concat("AJ", ooHeaderRowIndex.ToString()))
        MergeTwoCells(worksheet, String.Concat("AK", ooHeaderRowIndex.ToString()), String.Concat("AP", ooHeaderRowIndex.ToString()))
    End Sub

    Private Shared Sub AddBlankColumns(ByRef excelColumnNames() As String, ByRef newExcelRow As Row,
                                                  ByRef rowIndex As Integer, ByVal columnIndex As Integer,
                                                  ByVal noOfBlankColumns As Integer)

        For index = 1 To noOfBlankColumns
            'blank column
            columnIndex = columnIndex + 1
            AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), String.Empty, newExcelRow, CellValues.String)
        Next
    End Sub
    Private Shared Sub AddExceptionsToTheResultList(ByRef resultList As List(Of SKUDetail),
                                                    ByVal exceptions As List(Of SKUDetail),
                                                    ByVal exceptionType As String)
        If Not exceptions Is Nothing AndAlso exceptions.Count > 0 Then
            SetExceptionType(exceptions, exceptionType)
            resultList.AddRange(exceptions)
        End If

    End Sub
    Private Shared Sub SetExceptionType(ByRef skuDetails As List(Of SKUDetail), ByVal exceptionType As String)
        For Each item As SKUDetail In skuDetails
            item.ExceptionType = exceptionType
        Next
    End Sub
    Private Shared Function GetPathToSiteExpectedDays() As PathToSiteTiming
        Dim reportBAO As New Report()
        Dim p2sTiming As PathToSiteTiming = New PathToSiteTiming()

        p2sTiming.VendorImageAdsAverageDays = reportBAO.GetAverageDaysFromTurnInToActive_VendorImages()
        p2sTiming.TurnInAdsAverageDays = reportBAO.GetAverageDaysFromTurnInToActive_TurnIn()
        p2sTiming.LiftAdsAverageDays = reportBAO.GetAverageDaysFromTurnInToActive_LIFT()
        p2sTiming.INFCAdsAverageDays = reportBAO.GetAverageDaysFromTurnInToActive_INFC()
        p2sTiming.ExtraHotAdsAverageDays = reportBAO.GetAverageDaysFromTurnInToActive_ExtraHot()
        p2sTiming.SSSetupToSampleRequestAverageDays = reportBAO.GetAverageDaysFromSSSetupToSampleRequest()
        p2sTiming.SampleRequestToSampleReceiptAverageDays = reportBAO.GetAverageDaysFromSampleRequestToReceipt()
        p2sTiming.SSSetupToPOApprovalAverageDays = reportBAO.GetAverageDaysFromSSSetupToPOApproval()
        p2sTiming.SSSetupToTurnInAverageDays = reportBAO.GetAverageDaysFromSSSetupToTurnIn()
        p2sTiming.POApprovalToTurnInAverageDays = reportBAO.GetAverageDaysFromPOAprovalToTurnIn()
        p2sTiming.SampleReceiptToTurnInAverageDays = reportBAO.GetAverageDaysFromSampleReceiptToTurnIn()
        'p2sTiming.TurnInToImageShotAverageDaysVI = reportBAO.GetAverageDaysFromImageShotToTurnIn_VendorImages()
        p2sTiming.TurnInToImageShotAverageDaysLIFT = reportBAO.GetAverageDaysFromImageShotToTurnIn_LIFT()
        p2sTiming.TurnInToImageShotAverageDaysTI = reportBAO.GetAverageDaysFromImageShotToTurnIn_TurnIn()
        p2sTiming.TurnInToImageShotAverageDaysINFC = reportBAO.GetAverageDaysFromImageShotToTurnIn_INFC()
        p2sTiming.TurnInToImageShotAverageDaysExtraHot = reportBAO.GetAverageDaysFromImageShotToTurnIn_ExtraHot()
        'p2sTiming.ImageShotToFInalImageDaysVI = reportBAO.GetAverageDaysFromImageShotToFinalImageReady_VendorImages()
        p2sTiming.ImageShotToFInalImageDaysLIFT = reportBAO.GetAverageDaysFromImageShotToFinalImageReady_LIFT()
        p2sTiming.ImageShotToFInalImageDaysTI = reportBAO.GetAverageDaysFromImageShotToFinalImageReady_TurnIn()
        p2sTiming.ImageShotToFInalImageDaysINFC = reportBAO.GetAverageDaysFromImageShotToFinalImageReady_INFC()
        p2sTiming.ImageShotToFInalImageDaysExtraHot = reportBAO.GetAverageDaysFromImageShotToFinalImageReady_ExtraHot()
        'p2sTiming.AveragePhotoTimeVI = reportBAO.GetAverageTotalPhotoTime_VendorImages()
        p2sTiming.AveragePhotoTimeLIFT = reportBAO.GetAverageTotalPhotoTime_LIFT()
        p2sTiming.AveragePhotoTimeTI = reportBAO.GetAverageTotalPhotoTime_TurnIn()
        p2sTiming.AveragePhotoTimeINFC = reportBAO.GetAverageTotalPhotoTime_INFC()
        p2sTiming.AveragePhotoTimeExtraHot = reportBAO.GetAverageTotalPhotoTime_ExtraHot()
        p2sTiming.TurnInToPOReceiptAverageDays = reportBAO.GetAverageDaysFromPOReceiptToTurnIn()
        p2sTiming.TurnInToCopyReadyAverageDaysVI = reportBAO.GetAverageDaysFromTurnInToCopyReady_VendorImages()
        p2sTiming.TurnInToCopyReadyAverageDaysLIFT = reportBAO.GetAverageDaysFromTurnInToCopyReady_LIFT()
        p2sTiming.TurnInToCopyReadyAverageDaysTI = reportBAO.GetAverageDaysFromTurnInToCopyReady_TurnIn()
        p2sTiming.TurnInToCopyReadyAverageDaysINFC = reportBAO.GetAverageDaysFromTurnInToCopyReady_INFC()
        p2sTiming.TurnInToCopyReadyAverageDaysExtraHot = reportBAO.GetAverageDaysFromTurnInToCopyReady_ExtraHot()

        Return p2sTiming
    End Function
#End Region

#Region "Path To Site Expected Time Report"
    Private Shared Sub WritePathToSiteExpctedTimeListToExcelWorksheet(ByVal pathToSiteDays As PathToSiteTiming, ByVal worksheetPart As WorksheetPart,
                                                 ByVal worksheet As Worksheet, ByVal sheetData As SheetData)


        Dim cellValue As String = String.Empty
        Dim columnIndex As Integer = 0
        Dim styleIndex As Integer = 0
        '  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
        '
        '  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
        '  cells of data, we'll know if to write Text values or Numeric cell values.
        Dim numberOfColumns As Integer = 2
        Dim IsNumericColumn(numberOfColumns) As Boolean

        Dim excelColumnNames([numberOfColumns]) As String

        For n As Integer = 0 To numberOfColumns
            excelColumnNames(numberOfColumns) = GetExcelColumnName(n)
        Next n

        '
        '  Create the Header row in our Excel Worksheet
        '
        Dim rowIndex As UInt32 = 1

        'Append Headers
        Dim headerRow As Row = New Row
        headerRow.RowIndex = rowIndex            ' add the headers at the top of spreadsheet
        headerRow = GetPathToSiteTimingReportHeaders()
        sheetData.Append(headerRow)

        '
        '  Now, step through each row of data in our DataTable...
        '
        Dim cellNumericValue As Double = 0

        If Not pathToSiteDays Is Nothing Then
            Dim outputList() As PropertyInfo = pathToSiteDays.GetType().GetProperties()

            For Each item In outputList
                ' ...create a new row, and append a set of this row's data to it.
                rowIndex = rowIndex + 1
                Dim newExcelRow As New Row
                newExcelRow.RowIndex = rowIndex         '  add a row at the top of spreadsheet
                sheetData.Append(newExcelRow)

                columnIndex = 0

                'Exception Type
                cellValue = item.Name
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String, styleIndex)

                'Value
                columnIndex = columnIndex + 1
                cellValue = item.GetValue(pathToSiteDays)
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String, styleIndex)

            Next
        End If

        worksheet.Append(sheetData)
        worksheetPart.Worksheet = worksheet

        MergeTwoCells(worksheet, "A1", "C1")
    End Sub
    Private Shared Function GetPathToSiteTimingReportHeaders() As Row
        Dim excelRow As Row = Nothing
        Dim excelCell As Cell = Nothing
        Dim headerString As InlineString = Nothing
        Dim cellText As InlineString = Nothing
        Dim cellValue As CellValue = Nothing
        Dim runProp As RunProperties = Nothing
        Dim runCell As Run = Nothing
        Dim columnCount As Integer = 0
        excelRow = New Row()

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Exception Type", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Expected Days", GetExcelColumnName(columnCount)))

        Return excelRow
    End Function
#End Region

End Class