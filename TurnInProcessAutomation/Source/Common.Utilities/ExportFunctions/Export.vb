Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.HttpContext
Imports Telerik.Web.UI
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Text


Namespace ExportFunctions

    Public Class Export

#Region " Members "

        Private dictionary As Dictionary(Of String, Object)
        Private radGridExportHandler As RadGridExportHandler
        Private HTMLExportHandler As HTMLExportHandler
        Private multiExportHandler As MultiExportHandler
        Public Shared ReadOnly LandscapePageHeight As Unit = Unit.Parse("210mm")
        Public Shared ReadOnly LandscapePageWidth As Unit = Unit.Parse("500mm")

#End Region

        Public Sub New()

            radGridExportHandler = New RadGridExportHandler()
            HTMLExportHandler = New HTMLExportHandler()
            multiExportHandler = New MultiExportHandler()
            dictionary = New Dictionary(Of String, Object)

        End Sub

        Public Sub ExportToExcel(ByVal table As Object)
            ExportToExcel(table, String.Empty)
        End Sub

        Public Sub ExportToExcel(ByVal singleTable As Object, ByVal pageheader As String)
            dictionary.Add(pageheader, singleTable)
            ExportToExcel(dictionary, pageheader, False)
        End Sub

        Public Sub ExportToExcel(ByVal tableList As Dictionary(Of String, Object), ByVal pageheader As String, ByVal IsTabbed As Boolean)
            If radGridExportHandler.CanExport(tableList) Then
                radGridExportHandler.ExportToExcel(tableList, pageheader, IsTabbed)
            ElseIf HTMLExportHandler.CanExport(tableList) Then
                HTMLExportHandler.ExportToExcel(tableList, pageheader, IsTabbed)
            ElseIf multiExportHandler.CanExport(tableList) Then
                multiExportHandler.ExportToExcel(tableList, pageheader, IsTabbed)
            End If
        End Sub

        Public Sub MultipleExportToExcel(ByVal pageheader As String)
            ExportToExcel(dictionary, pageheader, True)
        End Sub

        Public Sub ExportToPDF(ByVal singleTable As Object, ByVal pageheader As String)
            ExportToPDF(singleTable, pageheader, False)
        End Sub

        Public Sub ExportToPDF(ByVal singleTable As Object, ByVal pageheader As String, ByVal isLandscape As Boolean)
            dictionary.Add(pageheader, singleTable)
            ExportToPDF(dictionary, pageheader, isLandscape, False)
        End Sub

        Public Sub ExportToPDF(ByVal tableList As Dictionary(Of String, Object), ByVal pageheader As String, ByVal isLandscape As Boolean, ByVal IsTabbed As Boolean)
            If radGridExportHandler.CanExport(tableList) Then
                radGridExportHandler.ExportToPDF(tableList, pageheader, isLandscape, IsTabbed)
            ElseIf HTMLExportHandler.CanExport(tableList) Then
                HTMLExportHandler.ExportToPDF(tableList, pageheader, isLandscape, IsTabbed)
            ElseIf multiExportHandler.CanExport(tableList) Then
                multiExportHandler.ExportToPDF(tableList, pageheader, isLandscape, IsTabbed)
            End If
        End Sub

        Public Sub MultipleExportToPDF(ByVal pageheader As String)
            ExportToPDF(dictionary, pageheader, False, True)
        End Sub

        Public Sub MultipleExportToPDF(ByVal pageheader As String, ByVal IsLandscape As Boolean)
            ExportToPDF(dictionary, pageheader, IsLandscape, True)
        End Sub

        Public Sub Append(ByVal table As Object, ByVal tabHeader As String)
            dictionary.Add(tabHeader, table)
        End Sub

        'Export to Excel with a centered title
        Private Function FormatHeader(ByVal pageheader As String, ByVal visibleColumns As Integer) As StringBuilder
            Dim sb As New StringBuilder

            sb.Append("<table style=""width:100%;"">")

            sb.Append("<tr>")
            sb.Append("<td align =""center""  colspan=""" & visibleColumns & """>")
            sb.Append("<span style=""font-size:large"">")
            sb.Append(pageheader)
            sb.Append("</span>")
            sb.Append("</td>")
            sb.Append("</tr>")
            sb.Append("<tr>")
            sb.Append("<table style=""width:100%;"">")

            Return sb
        End Function

        Private Sub FormatTableView(ByVal tableView As GridTableView)
            Dim col As GridColumn = tableView.Columns.FindByUniqueNameSafe("ISN")
            If col IsNot Nothing Then
                'Covert ISN type to integer to avoid decimal places
                col.DataType = GetType(Integer)
            End If
        End Sub

        Private Function GetDisplayableColumns(ByVal grid As RadGrid) As Integer
            Dim iDisplayCols As Integer = 0
            For i As Integer = 0 To grid.MasterTableView.Columns.Count - 1
                If grid.MasterTableView.Columns(i).Display Then
                    iDisplayCols += 1
                End If
            Next
            Return iDisplayCols
        End Function

        Public Sub ExportToExcel(ByVal grid As RadGrid, ByVal pageheader As String)
            Dim iDisplayCols As Integer = GetDisplayableColumns(grid)
            Dim header As StringBuilder = FormatHeader(pageheader, iDisplayCols)
            grid.MasterTableView.Caption = header.ToString
            FormatTableView(grid.MasterTableView)
            grid.MasterTableView.ExportToExcel()
            header.Append("</table>")
            header.Append("</tr>")
            header.Append("</table>")
        End Sub

    End Class
End Namespace

