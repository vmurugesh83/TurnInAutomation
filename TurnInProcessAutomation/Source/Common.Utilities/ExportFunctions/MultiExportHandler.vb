Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.HttpContext
Imports Telerik.Web.UI
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO


Public Class MultiExportHandler
    Implements IExportHandler

    Private htmlExportHandler As New HTMLExportHandler
    Private WithEvents radGrid As Telerik.Web.UI.RadGrid
    Private header As String
    Private dictionary As New Dictionary(Of String, Object)
    Private html As String
    Private pdf As Document
    Private pdfStream As System.IO.MemoryStream


    Public Function CanExport(ByVal dictionary As System.Collections.Generic.Dictionary(Of String, Object)) As Boolean Implements IExportHandler.CanExport
        Dim totalCount As Integer = dictionary.Count()
        Dim gridCount As Integer = dictionary.Where(Function(item As KeyValuePair(Of String, Object)) TypeOf item.Value Is RadGrid Or TypeOf item.Value Is HtmlTable).Count
        If totalCount = gridCount Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub ExportToExcel(ByVal tableList As System.Collections.Generic.Dictionary(Of String, Object), ByVal pageheader As String, ByVal IsTabbed As Boolean) Implements IExportHandler.ExportToExcel
        Try
            header = String.Empty
            html = String.Empty

            html = htmlExportHandler.GetExcelHTML(tableList, pageheader, IsTabbed)

            Dim radGridDictionaryItem As KeyValuePair(Of String, Object) = tableList.Where(Function(item As KeyValuePair(Of String, Object)) TypeOf item.Value Is RadGrid).FirstOrDefault

            header = radGridDictionaryItem.Key
            radGrid = DirectCast(radGridDictionaryItem.Value, RadGrid)
            radGrid.ExportSettings.IgnorePaging = True
            radGrid.ExportSettings.OpenInNewWindow = True
            radGrid.MasterTableView.ExportToExcel()
        Catch ex As Exception
        End Try
    End Sub

    Public Sub ExportToPDF(ByVal tableList As System.Collections.Generic.Dictionary(Of String, Object), ByVal pageheader As String, ByVal isLandscape As Boolean, ByVal IsTabbed As Boolean) Implements IExportHandler.ExportToPDF
        Try
            pdfStream = htmlExportHandler.GetPDFMemoryStream(tableList, pageheader, isLandscape, IsTabbed)

            Dim radGridDictionaryItem As KeyValuePair(Of String, Object) = tableList.Where(Function(item As KeyValuePair(Of String, Object)) TypeOf item.Value Is RadGrid).FirstOrDefault

            radGrid = DirectCast(radGridDictionaryItem.Value, RadGrid)
            radGrid.MasterTableView.HierarchyDefaultExpanded = True
            radGrid.ExportSettings.IgnorePaging = True
            radGrid.ExportSettings.Pdf.PageTitle = radGridDictionaryItem.Key
            radGrid.ExportSettings.OpenInNewWindow = True
            radGrid.ExportSettings.Pdf.AllowModify = True
            If isLandscape Then
                radGrid.ExportSettings.Pdf.PageHeight = Unit.Parse("310mm")
                radGrid.ExportSettings.Pdf.PageWidth = Unit.Parse("450mm")
            End If
            radGrid.MasterTableView.ExportToPdf()
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub RadGrid1_GridExporting(ByVal source As Object, ByVal e As Telerik.Web.UI.GridExportingArgs) Handles radGrid.GridExporting
        Select Case e.ExportType
            Case Telerik.Web.UI.ExportType.Excel
                e.ExportOutput = e.ExportOutput.Replace("<body>", "<body>" + html + "<table><tr><td>" + header + "</td></tr></table>")
            Case ExportType.Pdf
                CancelRadGridExport()
                AddRadGridToPDFExport(e.ExportOutput)

        End Select

    End Sub

    Private Sub radGrid_ExportCellFormatting(sender As Object, e As Telerik.Web.UI.ExportCellFormattingEventArgs) Handles radGrid.ExportCellFormatting
        If e.FormattedColumn.UniqueName = "Cost" Or e.FormattedColumn.UniqueName = "Retail" Or e.FormattedColumn.UniqueName = "Discount" Or e.FormattedColumn.UniqueName = "Freight" Or e.FormattedColumn.UniqueName = "TotalCost" Or e.FormattedColumn.UniqueName = "TotalRetail" Or e.FormattedColumn.UniqueName = "TotalDiscount" Or e.FormattedColumn.UniqueName = "TotalFreight" Then
            e.Cell.Style("mso-number-format") = "Currency"
        End If
    End Sub

    Private Sub AddRadGridToPDFExport(ByVal output As String)
        Try
            Current.Response.Clear()
            Current.Response.Charset = String.Empty
            Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Current.Response.ContentType = "application/pdf"
            Current.Response.AddHeader("content-disposition", "attachment; filename=PDFExport.pdf")

            Dim ms As New System.IO.MemoryStream


            Dim htmlTables As Byte() = pdfStream.GetBuffer()
            Dim reader As PdfReader = New PdfReader(htmlTables)
            Dim document As Document = New Document(reader.GetPageSizeWithRotation(1))
            Dim writer As PdfWriter = PdfWriter.GetInstance(document, ms)

            document.Open()

            Dim rotation As Integer
            Dim importedPage As PdfImportedPage
            Dim cb As PdfContentByte = writer.DirectContent

            'For HTML tables
            document.NewPage()
            importedPage = writer.GetImportedPage(reader, 1)
            rotation = reader.GetPageRotation(1)

            If rotation = 90 Or rotation = 270 Then
                cb.AddTemplate(importedPage, 0, -1, 1, 0, 0, reader.GetPageSizeWithRotation(1).Height)
            Else
                cb.AddTemplate(importedPage, 1, 0, 0, 1, 0, 0)
            End If


            'For Radgrid
            Dim radGrid As Byte() = System.Text.Encoding.Default.GetBytes(output)
            Dim reader1 As PdfReader = New PdfReader(radGrid)
            Dim numberOfPages1 As Integer = reader1.NumberOfPages

            ' Iterate through all pages 
            For currentPageIndex As Integer = 1 To numberOfPages1
                ' Determine page size for the current page 
                document.SetPageSize(reader1.GetPageSizeWithRotation(currentPageIndex))

                ' Create page 
                document.NewPage()
                Dim importedPage1 As PdfImportedPage = writer.GetImportedPage(reader1, currentPageIndex)

                ' Determine page orientation 
                Dim pageOrientation As Integer = reader1.GetPageRotation(currentPageIndex)
                If (pageOrientation = 90) OrElse (pageOrientation = 270) Then
                    cb.AddTemplate(importedPage1, 0, -1, 1, 0, 0, reader1.GetPageSizeWithRotation(currentPageIndex).Height)
                Else
                    cb.AddTemplate(importedPage1, 1, 0, 0, 1, 0, 0)
                End If
            Next


            'close document
            document.Close()

            Current.Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer.Length)
            Current.Response.End()

        Catch ex As Exception
        End Try

    End Sub

    Public Shared Function MergeFiles(ByVal sourceFiles As List(Of Byte())) As Byte()
        Dim document As New Document()
        Dim output As New MemoryStream()

        Try
            ' Initialize pdf writer 
            Dim writer As PdfWriter = PdfWriter.GetInstance(document, output)
            'writer.PageEvent = New PdfPageEvents()

            ' Open document to write 
            document.Open()
            Dim content As PdfContentByte = writer.DirectContent

            ' Iterate through all pdf documents 
            For fileCounter As Integer = 0 To sourceFiles.Count - 1
                ' Create pdf reader 
                Dim reader As New PdfReader(sourceFiles(fileCounter))
                Dim numberOfPages As Integer = reader.NumberOfPages

                ' Iterate through all pages 
                For currentPageIndex As Integer = 1 To numberOfPages
                    ' Determine page size for the current page 
                    document.SetPageSize(reader.GetPageSizeWithRotation(currentPageIndex))

                    ' Create page 
                    document.NewPage()
                    Dim importedPage As PdfImportedPage = writer.GetImportedPage(reader, currentPageIndex)


                    ' Determine page orientation 
                    Dim pageOrientation As Integer = reader.GetPageRotation(currentPageIndex)
                    If (pageOrientation = 90) OrElse (pageOrientation = 270) Then
                        content.AddTemplate(importedPage, 0, -1.0F, 1.0F, 0, 0, _
                        reader.GetPageSizeWithRotation(currentPageIndex).Height)
                    Else
                        content.AddTemplate(importedPage, 1.0F, 0, 0, 1.0F, 0, _
                        0)
                    End If
                Next
            Next
        Catch exception As Exception
            Throw New Exception("There has an unexpected exception" & " occured during the pdf merging process.", exception)
        Finally
            document.Close()
        End Try
        Return output.GetBuffer()
    End Function




    Private Sub CancelRadGridExport()
        Current.Response.Clear()
        Current.Response.ClearContent()
        Current.Response.ClearHeaders()
    End Sub
End Class
