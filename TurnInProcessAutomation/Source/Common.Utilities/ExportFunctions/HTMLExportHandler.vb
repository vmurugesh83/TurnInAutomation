Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.HttpContext
Imports Telerik.Web.UI
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.html

Public Class HTMLExportHandler
    Implements IExportHandler

    Private header As String
    Private dictionary As New Dictionary(Of String, Object)
    Private document As Document
    Private writer As PdfWriter
    Private nestedParagraph As New Paragraph
    Private nestedParagraph1 As New Paragraph
    Private nestedParagraph2 As New Paragraph
    Private IsNestedTable As Boolean


    Public Function CanExport(ByVal dictionary As System.Collections.Generic.Dictionary(Of String, Object)) As Boolean Implements IExportHandler.CanExport
        Dim totalCount As Integer = dictionary.Count()
        Dim htmlCount As Integer = dictionary.Where(Function(item As KeyValuePair(Of String, Object)) TypeOf item.Value Is HtmlTable).Count
        If totalCount = htmlCount Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub ExportToExcel(ByVal tableList As System.Collections.Generic.Dictionary(Of String, Object), ByVal pageheader As String, ByVal IsTabbed As Boolean) Implements IExportHandler.ExportToExcel
        Try
            Current.Response.Clear()
            Current.Response.Charset = String.Empty
            Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Current.Response.ContentType = "application/vnd.ms-excel"
            Current.Response.AddHeader("content-disposition", "attachment; filename=ExcelExport.xls")

            Current.Response.Write(GetExcelHTML(tableList, pageheader, IsTabbed))
            Current.Response.End()

            header = Nothing
            dictionary.Clear()
        Catch ex As Exception
        End Try
    End Sub

    Public Sub ExportToPDF(ByVal tableList As System.Collections.Generic.Dictionary(Of String, Object), ByVal pageheader As String, ByVal isLandscape As Boolean, ByVal IsTabbed As Boolean) Implements IExportHandler.ExportToPDF
        Try
            Current.Response.Clear()
            Current.Response.Charset = String.Empty
            Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Current.Response.ContentType = "application/pdf"
            Current.Response.AddHeader("content-disposition", "attachment; filename=PDFExport.pdf")

            Dim ms As System.IO.MemoryStream = GetPDFMemoryStream(tableList, pageheader, isLandscape, IsTabbed)

            Current.Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer.Length)
            Current.Response.End()
        Catch ex As Exception
        End Try
    End Sub

    Private Function getMaxRowCount(ByVal htmlTable As HtmlTable) As Integer
        Dim maxRow As Integer = 0
        Dim tempRow As Integer = 0
        For Each row As HtmlTableRow In htmlTable.Rows
            For Each cell As HtmlTableCell In row.Cells
                If cell.ColSpan = -1 Then
                    tempRow = tempRow + 1
                Else
                    tempRow = tempRow + cell.ColSpan
                End If

            Next
            If tempRow > maxRow Then
                maxRow = tempRow
            End If
            tempRow = 0
        Next
        Return maxRow
    End Function

    Private Function prepareForExcelExport(ByVal control As Control) As TableCell
        Dim tableCell As New TableCell()

        For Each ctrl As Control In control.Controls
            If Not ctrl.Visible Then Continue For 'Exclude invisible controls.

            If TypeOf ctrl Is Label Then
                Dim label As Label = DirectCast(ctrl, Label)

                If label.ID = "lblRunningCostTextOne" Or label.ID = "lblRunningRetailTextOne" Then
                    Dim formattedtext As String = FormatCurrency(HandleEmptyString(label.Text), , , TriState.True, TriState.True)
                    tableCell.Controls.Add(New LiteralControl(formattedtext))
                ElseIf label.Visible = True Then
                    tableCell.Controls.Add(New LiteralControl("  " + label.Text + "  "))
                End If
            ElseIf TypeOf ctrl Is TextBox Then
                tableCell.Controls.Add(New LiteralControl((DirectCast(ctrl, TextBox)).Text))
            ElseIf TypeOf ctrl Is Telerik.Web.UI.RadComboBox Then
                tableCell.Controls.Add(New LiteralControl(DirectCast(ctrl, RadComboBox).Text))
            ElseIf TypeOf ctrl Is Telerik.Web.UI.RadNumericTextBox Then
                Dim radnumctrl As RadNumericTextBox = DirectCast(ctrl, RadNumericTextBox)
                Dim formattedtext As String
                If radnumctrl.ID = "txtBatchId" Or radnumctrl.ID = "txtTotalDocuments" Or radnumctrl.ID = "txtVendor" Or radnumctrl.ID = "txtCompany" Or radnumctrl.ID = "txtAccount" Or radnumctrl.ID = "txtSubAccount" Or radnumctrl.ID = "txtTotalQtyRet" Or radnumctrl.ID = "txtCartonsRet" Or radnumctrl.ID = "txtDCRet" Or radnumctrl.ID = "txtWeightRet" Or radnumctrl.ID = "txtAccountVen" Or radnumctrl.ID = "txtSubAcctVen" Or radnumctrl.ID = "txtAccountTran" Or radnumctrl.ID = "txtSubAcctTran" Or radnumctrl.ID = "txtWeightTran" Or radnumctrl.ID = "txtDept" Or radnumctrl.ID = "txtPONumber" Or radnumctrl.ID = "txtPO" Or radnumctrl.ID = "txtInvoiceId" Or radnumctrl.ID = "txtStore" Or radnumctrl.ID = "txtYear" Or radnumctrl.ID = "txtFolderId" Or radnumctrl.ID = "txtRefVendor" Or radnumctrl.ID = "txtReferenceVendor" Or radnumctrl.ID = "txtStoreNo" Or radnumctrl.ID = "txtVendorNo" Or radnumctrl.ID = "txtDeptNo" Or radnumctrl.ID = "txtAddTerms" Then
                    formattedtext = radnumctrl.Text
                Else
                    formattedtext = FormatCurrency(HandleEmptyString(radnumctrl.Text), , , TriState.True, TriState.True)
                End If

                tableCell.Controls.Add(New LiteralControl(formattedtext))
            ElseIf TypeOf ctrl Is Telerik.Web.UI.RadTextBox Then
                tableCell.Controls.Add(New LiteralControl((DirectCast(ctrl, RadTextBox)).Text))
            ElseIf TypeOf ctrl Is Telerik.Web.UI.RadDatePicker Then
                Dim datecontrol As RadDatePicker = DirectCast(ctrl, RadDatePicker)
                If Not datecontrol.SelectedDate Is Nothing Then
                    tableCell.Controls.Add(New LiteralControl((DirectCast(ctrl, RadDatePicker)).SelectedDate))
                End If

            ElseIf TypeOf ctrl Is CheckBox Then
                Dim checkBox As CheckBox = DirectCast(ctrl, CheckBox)
                tableCell.Controls.Add(New LiteralControl(checkBox.Text + ":" + IIf(checkBox.Checked, " Selected ", " Unselected ").ToString()))
            ElseIf TypeOf ctrl Is CheckBoxList Then
                Dim checkBoxList As CheckBoxList = DirectCast(ctrl, CheckBoxList)
                For Each listItem As WebControls.ListItem In checkBoxList.Items
                    tableCell.Controls.Add(New LiteralControl(listItem.Text + ":" + IIf(listItem.Selected, " Selected ", " Unselected ").ToString()))
                Next

            ElseIf TypeOf ctrl Is RadioButtonList Then
                Dim radioButtonList As RadioButtonList = DirectCast(ctrl, RadioButtonList)
                If radioButtonList.Visible = True Then
                    For Each listItem As WebControls.ListItem In radioButtonList.Items
                        tableCell.Controls.Add(New LiteralControl(listItem.Text + ":" + IIf(listItem.Selected, " Selected ", " Unselected ").ToString()))
                    Next
                End If

            ElseIf TypeOf ctrl Is ListBox Then
                Dim listBox As ListBox = DirectCast(ctrl, ListBox)
                For Each item As WebControls.ListItem In listBox.Items
                    tableCell.Controls.Add(New LiteralControl(item.Text + "<br>"))
                Next

            ElseIf TypeOf ctrl Is HtmlTable Then
                Dim htmlTable As HtmlTable = DirectCast(ctrl, HtmlTable)
                tableCell.Controls.Add(prepareExportTable(htmlTable, New WebControls.Table))

            ElseIf TypeOf ctrl Is LiteralControl Then
                tableCell.Controls.Add(New LiteralControl(DirectCast(ctrl, LiteralControl).Text))
            End If
        Next

        Return tableCell

    End Function

    Private Function prepareForPDFExport(ByVal control As Control, ByRef pdfExportTable As PdfPTable) As PdfPCell

        Dim unchecked As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(Current.Server.MapPath("\Images\Unchecked.png"))
        Dim checked As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(Current.Server.MapPath("\Images\Checked.png"))
        Dim radiobutton As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(Current.Server.MapPath("\Images\Indeterminate.png"))

        Dim tableCell As New PdfPCell
        Dim phrase As New Phrase

        For Each ctrl As Control In control.Controls
            If Not ctrl.Visible Then Continue For 'Exclude invisible controls.

            If TypeOf ctrl Is Label Then
                Dim label As Label = DirectCast(ctrl, Label)

                If label.ID = "lblRunningCostTextOne" Or label.ID = "lblRunningRetailTextOne" Then
                    Dim formattedtext As String = FormatCurrency(HandleEmptyString(label.Text), , , TriState.True, TriState.True)
                    phrase.Add(New Chunk(formattedtext))
                ElseIf label.Visible = True Then
                    phrase.Add(New Chunk("  " + label.Text + "  "))
                End If
            ElseIf TypeOf ctrl Is TextBox Then
                phrase.Add(New Chunk((DirectCast(ctrl, TextBox)).Text))
            ElseIf TypeOf ctrl Is Telerik.Web.UI.RadComboBox Then
                phrase.Add(New Chunk(DirectCast(ctrl, RadComboBox).Text))
            ElseIf TypeOf ctrl Is Telerik.Web.UI.RadNumericTextBox Then
                Dim radnumctrl As RadNumericTextBox = DirectCast(ctrl, RadNumericTextBox)
                Dim formattedtext As String

                If radnumctrl.ID = "txtBatchId" Or radnumctrl.ID = "txtTotalDocuments" Or radnumctrl.ID = "txtVendor" Or radnumctrl.ID = "txtCompany" Or radnumctrl.ID = "txtAccount" Or radnumctrl.ID = "txtSubAccount" Or radnumctrl.ID = "txtTotalQtyRet" Or radnumctrl.ID = "txtCartonsRet" Or radnumctrl.ID = "txtDCRet" Or radnumctrl.ID = "txtWeightRet" Or radnumctrl.ID = "txtAccountVen" Or radnumctrl.ID = "txtSubAcctVen" Or radnumctrl.ID = "txtAccountTran" Or radnumctrl.ID = "txtSubAcctTran" Or radnumctrl.ID = "txtWeightTran" Or radnumctrl.ID = "txtDept" Or radnumctrl.ID = "txtPONumber" Or radnumctrl.ID = "txtPO" Or radnumctrl.ID = "txtInvoiceId" Or radnumctrl.ID = "txtStore" Or radnumctrl.ID = "txtYear" Or radnumctrl.ID = "txtFolderId" Or radnumctrl.ID = "txtRefVendor" Or radnumctrl.ID = "txtReferenceVendor" Or radnumctrl.ID = "txtStoreNo" Or radnumctrl.ID = "txtVendorNo" Or radnumctrl.ID = "txtDeptNo" Or radnumctrl.ID = "txtAddTerms" Then
                    formattedtext = radnumctrl.Text
                Else
                    formattedtext = FormatCurrency(HandleEmptyString(radnumctrl.Text), , , TriState.True, TriState.True)
                End If
                phrase.Add(New Chunk(formattedtext))
            ElseIf TypeOf ctrl Is Telerik.Web.UI.RadTextBox Then
                phrase.Add(New Chunk((DirectCast(ctrl, RadTextBox)).Text))
            ElseIf TypeOf ctrl Is Telerik.Web.UI.RadDatePicker Then
                Dim datecontrol As RadDatePicker = DirectCast(ctrl, RadDatePicker)
                If Not datecontrol.SelectedDate Is Nothing Then
                    phrase.Add(New Chunk((DirectCast(ctrl, RadDatePicker)).SelectedDate))
                End If
            ElseIf TypeOf ctrl Is CheckBox Then
                Dim checkBox As CheckBox = DirectCast(ctrl, CheckBox)
                Dim textField As New Chunk(checkBox.Text + ": ")
                Dim boxField As Chunk
                If checkBox.Checked Then
                    boxField = New Chunk(checked, 0, 0)
                Else
                    boxField = New Chunk(unchecked, 0, 0)
                End If
                phrase.Add(textField)
                phrase.Add(boxField)
            ElseIf TypeOf ctrl Is CheckBoxList Then
                Dim checkBoxList As CheckBoxList = DirectCast(ctrl, CheckBoxList)

                For Each listItem As WebControls.ListItem In checkBoxList.Items
                    Dim textField As New Chunk(listItem.Text + ": ")
                    Dim boxField As Chunk
                    If listItem.Selected Then
                        boxField = New Chunk(checked, 0, 0)
                    Else
                        boxField = New Chunk(unchecked, 0, 0)
                    End If
                    phrase.Add(textField)
                    phrase.Add(boxField)
                Next
            ElseIf TypeOf ctrl Is RadioButtonList Then
                Dim radioButtonList As RadioButtonList = DirectCast(ctrl, RadioButtonList)

                For Each listItem As WebControls.ListItem In radioButtonList.Items
                    Dim textField As New Chunk(listItem.Text + ": ")
                    Dim boxField As Chunk
                    If listItem.Selected Then
                        boxField = New Chunk(radiobutton, 0, 0)
                    Else
                        boxField = New Chunk(unchecked, 0, 0)
                    End If
                    phrase.Add(textField)
                    phrase.Add(boxField)
                Next
            ElseIf TypeOf ctrl Is ListBox Then
                Dim listBox As ListBox = DirectCast(ctrl, ListBox)

                For Each item As WebControls.ListItem In listBox.Items
                    Dim textField As New Chunk(item.Text + Environment.NewLine)
                    phrase.Add(textField)
                Next
            ElseIf TypeOf ctrl Is HtmlTable Then
                Dim htmlTable As HtmlTable = DirectCast(ctrl, HtmlTable)
                Dim newExportTable As New PdfPTable(getMaxRowCount(htmlTable))
                Dim nestedTable As New Anchor("Click to see table", FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.UNDERLINE, New BaseColor(0, 0, 255)))

                If htmlTable.ID = "lineCodeList" Then
                    nestedTable.Reference = "#nestedTable1"
                    nestedTable.Name = "top1"
                    phrase.Add(nestedTable)
                ElseIf htmlTable.ID = "vendorclassList" Then
                    nestedTable.Reference = "#nestedTable2"
                    nestedTable.Name = "top2"
                    phrase.Add(nestedTable)
                Else
                    nestedTable.Reference = "#nestedTable"
                    nestedTable.Name = "top"
                    phrase.Add(nestedTable)
                End If


                IsNestedTable = True
                WriteNestedTable(htmlTable, newExportTable)
            ElseIf TypeOf ctrl Is LiteralControl Then
                phrase.Add(New Chunk(DirectCast(ctrl, LiteralControl).Text.Replace("&nbsp;", " ").Trim))
            End If

        Next

        If phrase.Count > 0 Then
            tableCell.AddElement(phrase)
        End If

        Return tableCell

    End Function

    Public Function GetExcelHTML(ByVal tableList As Dictionary(Of String, Object), ByVal pageheader As String, ByVal IsTabbed As Boolean) As String
        Dim tw As New System.IO.StringWriter
        Dim hw As New HtmlTextWriter(tw)
        Dim exportTable As New WebControls.Table

        exportTable.GridLines = GridLines.Both

        'initial max row count for header row
        Dim maxRow As Integer = getMaxRowCount(tableList.FirstOrDefault.Value)

        'insert page header
        Dim pageHeaderCell As New TableCell
        Dim pageHeaderRow As New TableRow
        Dim pageHeaderLabel As New Label

        pageHeaderLabel.Text = pageheader
        pageHeaderCell.ColumnSpan = maxRow
        pageHeaderCell.HorizontalAlign = HorizontalAlign.Center
        pageHeaderCell.Controls.Add(pageHeaderLabel)
        pageHeaderRow.Cells.Add(pageHeaderCell)

        exportTable.Rows.Add(pageHeaderRow)

        'loop through tables in Dictionary
        For Each tableEntry As KeyValuePair(Of String, Object) In tableList.Where(Function(item As KeyValuePair(Of String, Object)) TypeOf item.Value Is HtmlTable)

            Dim htmlTable As HtmlTable = DirectCast(tableEntry.Value, HtmlTable)

            'get row count for individual tables
            maxRow = getMaxRowCount(htmlTable)

            If IsTabbed Then
                'if exporting tables from multiple tabs, add tab headers
                Dim tabHeaderCell As New TableCell
                Dim tabHeaderRow As New TableRow
                Dim tabHeaderLabel As New Label

                tabHeaderLabel.Text = tableEntry.Key
                tabHeaderCell.ColumnSpan = maxRow
                tabHeaderCell.HorizontalAlign = HorizontalAlign.Center
                tabHeaderCell.Controls.Add(tabHeaderLabel)
                tabHeaderRow.Cells.Add(tabHeaderCell)

                exportTable.Rows.Add(tabHeaderRow)

            End If

            exportTable = prepareExportTable(htmlTable, exportTable)
        Next

        exportTable.RenderControl(hw)

        Return tw.ToString()
    End Function

    Public Function GetPDFMemoryStream(ByVal tableList As System.Collections.Generic.Dictionary(Of String, Object), ByVal pageheader As String, ByVal isLandscape As Boolean, ByVal IsTabbed As Boolean)
        Dim sm As New System.IO.MemoryStream

        'Dim document As Document
        Select Case pageheader.ToUpper
            Case "VENDOR ALLOWANCE", "CHARGEBACK", "DOCUMENT LIST", "PO HEADER", "RECEIPT LIST"
                document = New Document(PageSize.LEDGER, 70, 70, 36, 36)
            Case Else
                If isLandscape Then
                    document = New Document(PageSize.LETTER.Rotate, 36, 36, 36, 36)
                Else
                    document = New Document(PageSize.LETTER, 36, 36, 36, 36)
                End If

        End Select
     
        'Dim writer As PdfWriter = PdfWriter.GetInstance(document, sm)
        writer = PdfWriter.GetInstance(document, sm)
        document.Open()

        'get initial row count
        Dim maxRow As Integer = getMaxRowCount(tableList.FirstOrDefault.Value)

        'create PDF Page Header Table
        Dim pdfHeaderTable As New PdfPTable(maxRow)

        If UCase(pageheader) = "VENDOR ALLOWANCE" Then
            pdfHeaderTable.WidthPercentage = 105
        ElseIf UCase(pageheader) = "PO AUDIT LIST" Then
            pdfHeaderTable.WidthPercentage = 80
        Else
            pdfHeaderTable.WidthPercentage = 110
        End If

        Dim pageHeaderCell As New PdfPCell(New Phrase(pageheader))
        pageHeaderCell.Colspan = maxRow
        pageHeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        pdfHeaderTable.AddCell(pageHeaderCell)

        document.Add(pdfHeaderTable)

        For Each tableEntry As KeyValuePair(Of String, Object) In tableList.Where(Function(item As KeyValuePair(Of String, Object)) TypeOf item.Value Is HtmlTable)

            Dim htmlTable As HtmlTable = DirectCast(tableEntry.Value, HtmlTable)

            'get individual row counts
            maxRow = getMaxRowCount(htmlTable)

            'create individual content tables
            Dim pdfExportTable As New PdfPTable(maxRow)
            If UCase(pageheader) = "VENDOR ALLOWANCE" Then
                pdfExportTable.WidthPercentage = 105
            ElseIf UCase(pageheader) = "PO AUDIT LIST" Then
                pdfHeaderTable.WidthPercentage = 80
            Else
                pdfExportTable.WidthPercentage = 110
            End If

            'if exporting tabs, include tab headers
            If IsTabbed Then
                Dim tabHeaderCell As New PdfPCell(New Phrase(tableEntry.Key))
                tabHeaderCell.Colspan = maxRow
                tabHeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                pdfExportTable.AddCell(tabHeaderCell)
            End If

            pdfExportTable = prepareExportTable(htmlTable, pdfExportTable)
            Dim widths As Integer() = getPDFWidths(pageheader)
            If Not widths Is Nothing Then
                pdfExportTable.SetWidths(widths)
            End If

            document.Add(pdfExportTable)
        Next

        If IsNestedTable Then

            If nestedParagraph.Count > 1 Then
                document.NewPage()
                document.Add(nestedParagraph)
            End If

            If nestedParagraph1.Count > 1 Then
                document.NewPage()
                document.Add(nestedParagraph1)
            End If

            If nestedParagraph2.Count > 1 Then
                document.NewPage()
                document.Add(nestedParagraph2)
            End If

        End If

        document.Close()
        'Return document
        Return sm
    End Function

    Private Function prepareExportTable(ByVal htmlTable As HtmlTable, ByVal pdfExportTable As PdfPTable) As PdfPTable
        For Each row As HtmlTableRow In htmlTable.Rows
            For Each cell As HtmlTableCell In row.Cells
                Dim pdfCell As PdfPCell = prepareForPDFExport(cell, pdfExportTable)
                pdfCell.Colspan = cell.ColSpan
                pdfCell.NoWrap = False
                pdfExportTable.AddCell(pdfCell)
            Next
        Next
        Return pdfExportTable
    End Function

    Private Function prepareExportTable(ByVal htmlTable As HtmlTable, ByVal exportTable As WebControls.Table) As WebControls.Table
        For Each row As HtmlTableRow In htmlTable.Rows
            Dim exportRow As New TableRow

            For Each cell As HtmlTableCell In row.Cells
                Dim exportCell As TableCell = prepareForExcelExport(cell)
                If cell.ColSpan <> -1 Then
                    exportCell.ColumnSpan = cell.ColSpan
                End If
                exportCell.Wrap = False
                exportRow.Cells.Add(exportCell)
            Next
            exportTable.Rows.Add(exportRow)
        Next
        Return exportTable
    End Function

    Private Function getPDFWidths(ByVal pageheader As String) As Integer()
        Dim pdfWidths As Integer() = Nothing
        If pageheader = "Line Code" Then
            pdfWidths = New Integer() {5, 3, 3, 3, 3, 6}
        ElseIf pageheader = "Reason Code" Then
            pdfWidths = New Integer() {4, 3, 3, 3, 4}
        ElseIf pageheader = "Transaction Code" Then
            pdfWidths = New Integer() {5, 3, 3, 4}
        ElseIf pageheader = "PO Header" Then
            pdfWidths = New Integer() {4, 2, 8, 4, 3, 8}
        ElseIf pageheader = "PO Audit List" Then
            pdfWidths = New Integer() {2, 3, 4, 8}
        ElseIf pageheader = "" Then

        End If

        Return pdfWidths
    End Function

    Private Sub WriteNestedTable(ByVal htmlTable As HtmlTable, ByVal pdfExportTable As PdfPTable)
     

        If htmlTable.ID = "lineCodeList" Then
            Dim nestedTable1 = prepareExportTable(htmlTable, pdfExportTable)
            Dim achorPhrase1 As New Phrase
            Dim anchor1 As New Anchor("Go Back", FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.UNDERLINE, New BaseColor(0, 0, 255)))
            nestedParagraph = New Paragraph("Click to ")
            anchor1.Name = "nestedTable1"
            anchor1.Reference = "#top1"
            nestedParagraph1.Add(anchor1)
            nestedParagraph1.Add(Environment.NewLine)
            nestedParagraph1.Add(nestedTable1)
        ElseIf htmlTable.ID = "vendorclassList" Then
            Dim nestedTable2 = prepareExportTable(htmlTable, pdfExportTable)
            Dim achorPhrase2 As New Phrase
            Dim anchor2 As New Anchor("Go Back", FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.UNDERLINE, New BaseColor(0, 0, 255)))
            nestedParagraph = New Paragraph("Click to ")
            anchor2.Name = "nestedTable2"
            anchor2.Reference = "#top2"
            nestedParagraph2.Add(anchor2)
            nestedParagraph2.Add(Environment.NewLine)
            nestedParagraph2.Add(nestedTable2)
        Else
            Dim nestedTable = prepareExportTable(htmlTable, pdfExportTable)
            Dim achorPhrase As New Phrase
            Dim anchor As New Anchor("Go Back", FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.UNDERLINE, New BaseColor(0, 0, 255)))
            nestedParagraph = New Paragraph("Click to ")
            anchor.Name = "nestedTable"
            anchor.Reference = "#top"
            nestedParagraph.Add(anchor)
            nestedParagraph.Add(Environment.NewLine)
            nestedParagraph.Add(nestedTable)
        End If


    End Sub

    Private Function HandleEmptyString(ByRef strValue As String) As String

        If Trim(strValue) = String.Empty Then
            Return "0"
        Else
            Return strValue
        End If

    End Function

End Class
