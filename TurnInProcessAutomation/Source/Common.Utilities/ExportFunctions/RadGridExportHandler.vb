Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.HttpContext
Imports Telerik.Web.UI
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class RadGridExportHandler
    Implements IExportHandler

    Private WithEvents radGrid As Telerik.Web.UI.RadGrid
    Private header As String
    Private dictionary As Dictionary(Of String, Object)

    Public Function CanExport(ByVal dictionary As System.Collections.Generic.Dictionary(Of String, Object)) As Boolean Implements IExportHandler.CanExport
        Dim totalCount As Integer = dictionary.Count()
        Dim gridCount As Integer = dictionary.Where(Function(item As KeyValuePair(Of String, Object)) TypeOf item.Value Is RadGrid).Count
        If totalCount = gridCount Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub ExportToExcel(ByVal tableList As System.Collections.Generic.Dictionary(Of String, Object), ByVal pageheader As String, ByVal IsTabbed As Boolean) Implements IExportHandler.ExportToExcel
        Try
            If tableList.Count > 1 Then
                Throw New Exception("Multiple RadGrids cannot be exported using List feature. Wrap in one RadGrid and export together.")
            End If
            header = pageheader
            radGrid = DirectCast(tableList.FirstOrDefault.Value, RadGrid)
            radGrid.ExportSettings.IgnorePaging = True
            radGrid.ExportSettings.OpenInNewWindow = True
            radGrid.MasterTableView.ExportToExcel()
        Catch ex As Exception
        End Try
    End Sub

    Public Sub ExportToPDF(ByVal tableList As System.Collections.Generic.Dictionary(Of String, Object), ByVal pageheader As String, ByVal isLandscape As Boolean, ByVal IsTabbed As Boolean) Implements IExportHandler.ExportToPDF
        Try
            If tableList.Count > 1 Then
                Throw New Exception("Multiple RadGrids cannot be exported using List feature. Wrap in one RadGrid and export together.")
            End If
            radGrid = DirectCast(tableList.FirstOrDefault.Value, RadGrid)
            radGrid.MasterTableView.HierarchyDefaultExpanded = True
            radGrid.ExportSettings.IgnorePaging = True
            radGrid.ExportSettings.Pdf.PageTitle = pageheader
            radGrid.ExportSettings.OpenInNewWindow = True

            If isLandscape Then
                radGrid.ExportSettings.Pdf.PageHeight = Unit.Parse("210mm")
                radGrid.ExportSettings.Pdf.PageWidth = Unit.Parse("500mm")
            End If
            radGrid.MasterTableView.ExportToPdf()
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub RadGrid1_GridExporting(ByVal source As Object, ByVal e As Telerik.Web.UI.GridExportingArgs) Handles radGrid.GridExporting
        Select Case e.ExportType
            Case Telerik.Web.UI.ExportType.Excel
                e.ExportOutput = e.ExportOutput.Replace("<body>", "<body><table><tr><td>" + header + "</td></tr></table>")
        End Select
    End Sub

End Class
