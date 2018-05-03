Imports System.IO
Imports System.Data.OleDb
Imports DocumentFormat.OpenXml
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Spreadsheet
Imports System.Text.RegularExpressions

Public Class ImportExcel
    Public Shared Function ExcelToDataTable(ByVal filePath As String, ByVal sheetName As String) As DataTable
        Dim connectionString As String = String.Empty
        Dim oledbConnection As OleDbConnection = Nothing
        Dim dataAdapter As OleDbDataAdapter = Nothing
        Dim dtExcel As New DataTable
        Dim excelFileInfo As New FileInfo(filePath)
        Dim dtSchema As New DataTable

        If String.IsNullOrEmpty(sheetName) Then
            sheetName = "Sheet1"
        End If

        If Not excelFileInfo.Exists Then
            Throw New Exception("Excel File doesn't exist")
        End If

        connectionString = GetExcelConnectionString(filePath, excelFileInfo.Extension)
        oledbConnection = New OleDbConnection(connectionString)
        oledbConnection.Open()
        dtSchema = oledbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
        If Not dtSchema Is Nothing Then
            sheetName = dtSchema.Rows(0)("TABLE_NAME").ToString()
        End If
        dataAdapter = New OleDbDataAdapter(String.Format("SELECT * FROM [{0}]", sheetName), oledbConnection)
        dataAdapter.Fill(dtExcel)

        Return dtExcel
    End Function

    Private Shared Function GetExcelConnectionString(ByVal filePath As String, ByVal fileExtension As String) As String
        Select Case fileExtension
            Case ".xlsx"
                Return String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'", filePath)
            Case Else
                Return String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'", filePath)
        End Select
    End Function
    Public Shared Function ExcelToDataTableOpenXML(ByVal filePath As String, ByVal sheetName As String) As DataTable
        Dim dtExcel As New DataTable
        Dim excelFileInfo As New FileInfo(filePath)
        Dim dtSchema As New DataTable
        Dim rowIndex As Integer = 0

        If Not excelFileInfo.Exists Then
            Throw New Exception("Excel File doesn't exist")
        End If

        'Open the Excel file in Read Mode using OpenXml.
        Using doc As SpreadsheetDocument = SpreadsheetDocument.Open(filePath, False)
            Dim workbookPart As WorkbookPart = doc.WorkbookPart
            Dim sheets As IEnumerable(Of Sheet) = doc.WorkbookPart.Workbook.GetFirstChild(Of Sheets)().Elements(Of Sheet)()
            Dim relationshipId As String = sheets.First().Id.Value
            Dim worksheetPart As WorksheetPart = DirectCast(doc.WorkbookPart.GetPartById(relationshipId), WorksheetPart)
            Dim workSheet As Worksheet = worksheetPart.Worksheet
            Dim sheetData As SheetData = workSheet.GetFirstChild(Of SheetData)()
            Dim rows As IEnumerable(Of Row) = sheetData.Descendants(Of Row)()
            For Each cell As Cell In rows.ElementAt(0)
                dtExcel.Columns.Add(GetCellValue(doc, cell))
            Next
            Try

                For Each row As Row In rows
                    'this will also include your header row...
                    Dim tempRow As DataRow = dtExcel.NewRow()
                    Dim columnIndex As Integer = 0
                    For Each cell As Cell In row.Descendants(Of Cell)()
                        ' Gets the column index of the cell with data
                        Dim cellColumnIndex As Integer = CInt(GetColumnIndexFromName(GetColumnName(cell.CellReference)))
                        cellColumnIndex -= 1
                        'zero based index
                        If columnIndex < cellColumnIndex Then
                            Do
                                tempRow(columnIndex) = String.Empty
                                'Insert blank data here;
                                columnIndex += 1
                            Loop While columnIndex < cellColumnIndex
                        End If
                        tempRow(columnIndex) = GetCellValue(doc, cell)

                        columnIndex += 1
                    Next
                    dtExcel.Rows.Add(tempRow)
                Next
                '...so i'm taking it out here.
                dtExcel.Rows.RemoveAt(0)
            Catch ex As Exception
                Throw
            End Try
        End Using
        Return dtExcel
    End Function
    Private Shared Function GetValue(doc As SpreadsheetDocument, cell As Cell) As String
        Dim value As String = If(Not cell.CellValue Is Nothing, cell.CellValue.InnerText, cell.InnerText)
        If cell.DataType IsNot Nothing AndAlso cell.DataType.Value = CellValues.SharedString Then
            Return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(Integer.Parse(value)).InnerText
        End If
        Return value
    End Function
    ''' <summary>
    ''' Given a cell name, parses the specified cell to get the column name.
    ''' </summary>
    ''' <param name="cellReferece"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function GetColumnName(ByVal cellReferece As String) As String
        Dim regEx As Regex = New Regex("[A-Za-z]+")
        Dim matchEx As Match = regEx.Match(cellReferece)
        Return matchEx.Value
    End Function
    Private Shared Function GetColumnIndexFromName(ByVal columnName As String) As Integer
        Dim colName As String = columnName.ToCharArray()
        Dim number As Integer = 0
        Dim pow As Integer = 1

        For i As Integer = colName.Length - 1 To 0 Step -1
            number += (Asc(colName(i)) - Asc("A") + 1) * pow
            pow *= 26
        Next
        Return number
    End Function

    Private Shared Function GetCellValue(document As SpreadsheetDocument, cell As Cell) As String
        Dim stringTablePart As SharedStringTablePart = document.WorkbookPart.SharedStringTablePart
        If cell.CellValue Is Nothing Then
            Return ""
        End If
        Dim value As String = cell.CellValue.InnerXml
        If cell.DataType IsNot Nothing AndAlso cell.DataType.Value = CellValues.SharedString Then
            Return stringTablePart.SharedStringTable.ChildElements(Int32.Parse(value)).InnerText
        Else
            Return value
        End If
    End Function
End Class
