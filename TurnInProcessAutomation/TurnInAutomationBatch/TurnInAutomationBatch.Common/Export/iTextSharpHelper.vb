Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Text
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.tool.xml
Imports TurnInAutomationBatch.BusinessEntities
Imports System.Web
Imports DocumentFormat.OpenXml.Spreadsheet
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml

Public Class iTextSharpHelper

    Public Shared Sub GenerateNoSampleReport(ByVal OutputFile As String, ByVal HtmlTemplateFile As String, ByVal noSampleReportData As List(Of NoSampleReportInfo)) ', ByVal Placeholders() As String, ByVal PlaceHolderValues() As String, ByVal workListData As SnObjectList(Of SKUInfo))

        Dim sr As System.IO.StreamReader
        Dim sbHtml As New StringBuilder
        Dim status As String = ""
        Dim approvalFlag As String = ""

        If File.Exists(HtmlTemplateFile) Then
            sr = System.IO.File.OpenText(HtmlTemplateFile)
            sbHtml.AppendLine(HttpUtility.HtmlEncode(sr.ReadToEnd()))
            sr.Close()
        End If

        sbHtml = New StringBuilder(sbHtml.ToString.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", """"))

        sbHtml.AppendLine("<table class=""tTitle""><tr><td>On Order, Action Required</td></tr></table>")
        sbHtml.AppendLine("<div style=""height: 20px;""></div>")
        sbHtml.AppendLine("<table border=""0"" class=""tData"">")
        sbHtml.AppendLine("    <tr class=""trHeader"">")
        sbHtml.AppendLine("       <td class=""tBorder"">Department</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">ISN</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">ISN Description</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Vendor Style</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Color</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Color Description</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">OO Qty</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Ship Date</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Merch ID</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Merch Status</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Merch Approval</td>")
        sbHtml.AppendLine("   </tr>")

        For i As Integer = 0 To noSampleReportData.Count - 1
            status = HttpUtility.HtmlEncode(noSampleReportData(i).TurnInMerchStatus)

            If noSampleReportData(i).IsVendorImage Then
                approvalFlag = "Submit Image"
            Else
                approvalFlag = IIf(status = "Received" AndAlso HttpUtility.HtmlEncode(noSampleReportData(i).SampleApprovalFlag) = "N", "Approval Needed", "")
            End If

            sbHtml.AppendLine("    <tr class=""trRow"">")
            sbHtml.AppendLine("       <td class=""tBorder tdDept"">" & HttpUtility.HtmlEncode(noSampleReportData(i).DeptId_With_Desc) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(noSampleReportData(i).ISN) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdISN"">" & HttpUtility.HtmlEncode(noSampleReportData(i).ISNDesc) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder"">" & HttpUtility.HtmlEncode(noSampleReportData(i).VendorStyleNumber) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(noSampleReportData(i).ColorCode) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder"">" & HttpUtility.HtmlEncode(noSampleReportData(i).ColorDesc) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(noSampleReportData(i).OnOrder) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(noSampleReportData(i).StartShipDate) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(noSampleReportData(i).TurnInMerchId) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder"">" & status & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder"">" & approvalFlag & "</td>")
            sbHtml.AppendLine("   </tr>")
        Next

        sbHtml.AppendLine("</table>")

        GeneratePDF(OutputFile, sbHtml.ToString)

    End Sub

    Public Shared Sub GenerateNoSampleReportExcel(ByVal OutputFile As String, ByVal pageTitle As String, ByVal noSampleReportData As List(Of NoSampleReportInfo))

        Dim sbHtml As New StringBuilder
        Dim status As String = ""
        Dim approvalFlag As String = ""
        Dim sw As New StreamWriter(OutputFile, False)
        sbHtml.AppendLine("<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">")
        sbHtml.AppendLine("<table><tr><td colspan='13' style='font: bold 16pt arial; text-align: center;'>On Order, Action Required Report</td></tr></table>")
        sbHtml.AppendLine("<br/><br/>")
        sbHtml.AppendLine("<table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Arial; background:white;'>")
        sbHtml.AppendLine("    <tr style='font-weight: bold; text-align: center; text-decoration: underline;'>")
        sbHtml.AppendLine("       <td>Buyer</td><td>Department</td><td>Vendor</td><td>ISN</td><td>ISN Description</td><td>Vendor Style</td><td>Color</td><td>Color Description</td><td>OO Qty</td><td>Ship Date</td><td>Merch ID</td><td>Merch Status</td><td>Merch Approval</td>")
        sbHtml.AppendLine("    </tr>")
        For i As Integer = 0 To noSampleReportData.Count - 1
            status = HttpUtility.HtmlEncode(noSampleReportData(i).TurnInMerchStatus)

            If noSampleReportData(i).IsVendorImage Then
                approvalFlag = "Submit Image"
            Else
                approvalFlag = IIf(status = "Received" AndAlso HttpUtility.HtmlEncode(noSampleReportData(i).SampleApprovalFlag) = "N", "Approval Needed", "")
            End If

            sbHtml.AppendLine("    <tr>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(noSampleReportData(i).BuyerId_With_Desc))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(noSampleReportData(i).DeptId_With_Desc))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(noSampleReportData(i).VendorId_With_Desc))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(noSampleReportData(i).ISN))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(noSampleReportData(i).ISNDesc))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td style='mso-number-format:\@;'>")
            sbHtml.Append(HttpUtility.HtmlEncode(noSampleReportData(i).VendorStyleNumber))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(noSampleReportData(i).ColorCode))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(noSampleReportData(i).ColorDesc))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(noSampleReportData(i).OnOrder))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(noSampleReportData(i).StartShipDate))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(noSampleReportData(i).TurnInMerchId))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(status)
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(approvalFlag)
            sbHtml.AppendLine("</td>")
            sbHtml.AppendLine("   </tr>")
        Next
        sbHtml.AppendLine("</table>")
        sw.Write(sbHtml.ToString)
        sw.Close()

    End Sub

    Public Shared Sub GenerateAutoTurnInReport(ByVal OutputFile As String, ByVal HtmlTemplateFile As String, ByVal autoTurnInReportInfoData As List(Of AutoTurnInReportInfo)) ', ByVal Placeholders() As String, ByVal PlaceHolderValues() As String, ByVal workListData As SnObjectList(Of SKUInfo))

        Dim sr As System.IO.StreamReader
        Dim sbHtml As New StringBuilder

        If File.Exists(HtmlTemplateFile) Then
            sr = System.IO.File.OpenText(HtmlTemplateFile)
            sbHtml.AppendLine(HttpUtility.HtmlEncode(sr.ReadToEnd()))
            sr.Close()
        End If

        sbHtml = New StringBuilder(sbHtml.ToString.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", """"))

        'For i As Integer = 0 To Placeholders.Length - 1
        '    html = html.Replace(Placeholders(i), HttpUtility.HtmlEncode(PlaceHolderValues(i)))
        'Next

        sbHtml.AppendLine("<table class=""tTitle""><tr><td>Items to Auto Turn-In Next Week</td></tr></table>")
        sbHtml.AppendLine("<div style=""height: 20px;""></div>")
        sbHtml.AppendLine("<table border=""0"" class=""tData"">")
        sbHtml.AppendLine("    <tr class=""trHeader"">")
        sbHtml.AppendLine("       <td class=""tBorder"">Department</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">ISN</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">ISN Description</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Vendor Style</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Color</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Color Description</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">OO Qty</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Ship Date</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Fabrication</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Origination</td>")
        sbHtml.AppendLine("   </tr>")

        For i As Integer = 0 To autoTurnInReportInfoData.Count - 1
            sbHtml.AppendLine("    <tr class=""trRow"">")
            sbHtml.AppendLine("       <td class=""tBorder tdDept"">" & HttpUtility.HtmlEncode(autoTurnInReportInfoData(i).DeptId_With_Desc) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(autoTurnInReportInfoData(i).ISN) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdISN"">" & HttpUtility.HtmlEncode(autoTurnInReportInfoData(i).ISNDesc) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder"">" & HttpUtility.HtmlEncode(autoTurnInReportInfoData(i).VendorStyleNumber) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(autoTurnInReportInfoData(i).ColorCode) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder"">" & HttpUtility.HtmlEncode(autoTurnInReportInfoData(i).ColorDesc) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(autoTurnInReportInfoData(i).OnOrder) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(autoTurnInReportInfoData(i).StartShipDate) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(autoTurnInReportInfoData(i).Fabrication) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(autoTurnInReportInfoData(i).Origination) & "</td>")
            sbHtml.AppendLine("   </tr>")
        Next

        sbHtml.AppendLine("</table>")

        GeneratePDF(OutputFile, sbHtml.ToString)

    End Sub

    Public Shared Sub GenerateNoFinalImageReport(ByVal OutputFile As String, ByVal HtmlTemplateFile As String, ByVal noFinalImageReportData As List(Of NoFinalImageReportInfo)) ', ByVal Placeholders() As String, ByVal PlaceHolderValues() As String, ByVal workListData As SnObjectList(Of SKUInfo))

        Dim sr As System.IO.StreamReader
        Dim sbHtml As New StringBuilder

        If File.Exists(HtmlTemplateFile) Then
            sr = System.IO.File.OpenText(HtmlTemplateFile)
            sbHtml.AppendLine(HttpUtility.HtmlEncode(sr.ReadToEnd()))
            sr.Close()
        End If

        sbHtml = New StringBuilder(sbHtml.ToString.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", """"))

        sbHtml.AppendLine("<table class=""tTitle""><tr><td>No Final Image Report</td></tr></table>")
        sbHtml.AppendLine("<div style=""height: 20px;""></div>")
        sbHtml.AppendLine("<table border=""0"" class=""tData"">")
        sbHtml.AppendLine("    <tr class=""trHeader"">")
        sbHtml.AppendLine("       <td class=""tBorder"">Department</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Image ID</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Merch ID</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">ISN</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">ISN Description</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Vendor Style</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Color</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Color Description</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">OO Qty</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Ship Date</td>")
        sbHtml.AppendLine("   </tr>")

        For i As Integer = 0 To noFinalImageReportData.Count - 1
            sbHtml.AppendLine("    <tr class=""trRow"">")
            sbHtml.AppendLine("       <td class=""tBorder tdNoFinalImageDesc"">" & HttpUtility.HtmlEncode(noFinalImageReportData(i).DeptId_With_Desc) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(noFinalImageReportData(i).ImageID) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(noFinalImageReportData(i).TurnInMerchId) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(noFinalImageReportData(i).ISN) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdNoFinalImageDesc"">" & HttpUtility.HtmlEncode(noFinalImageReportData(i).ISNDesc) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder"">" & HttpUtility.HtmlEncode(noFinalImageReportData(i).VendorStyleNumber) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(noFinalImageReportData(i).ColorCode) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder"">" & HttpUtility.HtmlEncode(noFinalImageReportData(i).ColorDesc) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(noFinalImageReportData(i).OnOrder) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(noFinalImageReportData(i).StartShipDate) & "</td>")
            sbHtml.AppendLine("   </tr>")
        Next

        sbHtml.AppendLine("</table>")

        GeneratePDF(OutputFile, sbHtml.ToString)

    End Sub

    Public Shared Sub GenerateImageGroupReport(ByVal OutputFile As String, ByVal HtmlTemplateFile As String, ByVal imageGroupingReportData As List(Of ImageGroupingReportInfo))

        Dim sr As System.IO.StreamReader
        Dim sbHtml As New StringBuilder

        If File.Exists(HtmlTemplateFile) Then
            sr = System.IO.File.OpenText(HtmlTemplateFile)
            sbHtml.AppendLine(HttpUtility.HtmlEncode(sr.ReadToEnd()))
            sr.Close()
        End If

        sbHtml = New StringBuilder(sbHtml.ToString.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", """"))

        sbHtml.AppendLine("<table class=""tTitle""><tr><td>Image Group Report</td></tr></table>")
        sbHtml.AppendLine("<div style=""height: 20px;""></div>")
        sbHtml.AppendLine("<table border=""0"" class=""tData"">")
        sbHtml.AppendLine("    <tr class=""trHeader"">")
        sbHtml.AppendLine("       <td class=""tBorder"">CMG</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Image Group</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Image ID</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Vendor Style</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Merch ID</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Product Description</td>")
        sbHtml.AppendLine("   </tr>")

        For i As Integer = 0 To imageGroupingReportData.Count - 1
            sbHtml.AppendLine("    <tr class=""trRow"">")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(imageGroupingReportData(i).CmgId) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(imageGroupingReportData(i).ImageGroup) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(imageGroupingReportData(i).ImageID) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder"">" & HttpUtility.HtmlEncode(imageGroupingReportData(i).VendorStyleNumber) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(imageGroupingReportData(i).TurnInMerchId) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder"">" & HttpUtility.HtmlEncode(imageGroupingReportData(i).ProductDesc) & "</td>")
            sbHtml.AppendLine("   </tr>")
        Next

        sbHtml.AppendLine("</table>")

        GeneratePDF(OutputFile, sbHtml.ToString)

    End Sub

    Public Shared Sub GenerateTIAExceptionsReport(ByVal OutputFile As String, ByVal HtmlTemplateFile As String, ByVal tiaExceptionsReportData As List(Of TIAExceptionsReportInfo))

        Dim sr As System.IO.StreamReader
        Dim sbHtml As New StringBuilder

        If File.Exists(HtmlTemplateFile) Then
            sr = System.IO.File.OpenText(HtmlTemplateFile)
            sbHtml.AppendLine(HttpUtility.HtmlEncode(sr.ReadToEnd()))
            sr.Close()
        End If

        sbHtml = New StringBuilder(sbHtml.ToString.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", """"))

        sbHtml.AppendLine("<table class=""tTitle""><tr><td>TIA Exceptions Report</td></tr></table>")
        sbHtml.AppendLine("<div style=""height: 20px;""></div>")
        sbHtml.AppendLine("<table border=""0"" class=""tData"">")
        sbHtml.AppendLine("    <tr class=""trHeader"">")
        sbHtml.AppendLine("       <td class=""tBorder"">Batch ID</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Item ID</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Item Type</td>")
        sbHtml.AppendLine("       <td class=""tBorder"">Batch Status</td>")
        sbHtml.AppendLine("   </tr>")

        For i As Integer = 0 To tiaExceptionsReportData.Count - 1
            sbHtml.AppendLine("    <tr class=""trRow"">")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(tiaExceptionsReportData(i).BatchId) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder tdRight"">" & HttpUtility.HtmlEncode(tiaExceptionsReportData(i).ItemId) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder"">" & HttpUtility.HtmlEncode(tiaExceptionsReportData(i).ItemType) & "</td>")
            sbHtml.AppendLine("       <td class=""tBorder"">" & HttpUtility.HtmlEncode(tiaExceptionsReportData(i).BatchStatus) & "</td>")
            sbHtml.AppendLine("   </tr>")
        Next

        sbHtml.AppendLine("</table>")

        GeneratePDF(OutputFile, sbHtml.ToString)

    End Sub

    Private Shared Sub GeneratePDF(ByVal OutputFile As String, ByVal html As String)

        'Create Temp PDF w/o Page Numbers
        Dim tempFile As String = OutputFile.Replace(".pdf", "_Temp.pdf")
        Dim document As New Document(PageSize.A4.Rotate)
        document.SetMargins(10, 10, 36, 36)
        Using output As FileStream = File.Create(tempFile)
            Dim writer As PdfWriter = PdfWriter.GetInstance(document, output)
            document.Open()
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, New StringReader(html))
            document.Close()
        End Using

        'Create PDF w/ Page Numbers
        Dim reader As New PdfReader(tempFile)
        Using fs As New FileStream(OutputFile, FileMode.Create, FileAccess.Write, FileShare.None)
            Using stamper As New PdfStamper(reader, fs)
                Dim PageCount As Integer = reader.NumberOfPages
                For i As Integer = 1 To PageCount
                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, New Phrase([String].Format("Page {0} of {1}", i, PageCount)), 830, 18, 0)
                Next
            End Using
        End Using

        reader.Close()
        reader.Dispose()

        'Delete Temp PDF
        Dim outputDir As String = tempFile.Substring(0, tempFile.LastIndexOf("\") + 1)
        Dim tempFileName As String = tempFile.Substring(outputDir.Length, tempFile.Length - outputDir.Length)
        For Each f As String In Directory.GetFiles(outputDir, tempFileName)
            File.Delete(f)
        Next

    End Sub
    Public Shared Sub GenerateNoFinalImageReportExcel(ByVal OutputFile As String, ByVal pageTitle As String, ByVal noFinalImageReportData As List(Of NoFinalImageReportInfo))

        Dim sbHtml As New StringBuilder
        Dim sw As New StreamWriter(OutputFile, False)
        sbHtml.AppendLine("<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">")
        sbHtml.AppendLine("<table><tr><td colspan='13' style='font: bold 16pt arial; text-align: center;'>No Final Image Report</td></tr></table>")
        sbHtml.AppendLine("<br/><br/>")
        sbHtml.AppendLine("<table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Arial; background:white;'>")
        sbHtml.AppendLine("    <tr style='font-weight: bold; text-align: center; text-decoration: underline;'>")
        sbHtml.AppendLine("       <td>Buyer</td><td>Department</td><td>Vendor</td><td>Ad Number</td><td>Page Number</td><td>Image ID</td><td>Merch ID</td><td>ISN</td><td>ISN Description</td><td>Vendor Style</td><td>Color</td><td>Color Description</td><td>OO Qty</td><td>Ship Date</td>")
        sbHtml.AppendLine("    </tr>")
        For i As Integer = 0 To noFinalImageReportData.Count - 1
            sbHtml.AppendLine("    <tr>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(String.Concat(noFinalImageReportData(i).BuyerID, " - ", noFinalImageReportData(i).BuyerDesc)))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(noFinalImageReportData(i).DeptId_With_Desc))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(String.Concat(noFinalImageReportData(i).VendorID, " - ", noFinalImageReportData(i).VendorName)))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(If(noFinalImageReportData(i).AdNumber > 0, noFinalImageReportData(i).AdNumber, String.Empty)))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(If(noFinalImageReportData(i).PageNumber > 0, noFinalImageReportData(i).PageNumber, String.Empty)))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(noFinalImageReportData(i).ImageID))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(noFinalImageReportData(i).TurnInMerchId))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td style='mso-number-format:\@;'>")
            sbHtml.Append(HttpUtility.HtmlEncode(noFinalImageReportData(i).ISN))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(noFinalImageReportData(i).ISNDesc))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(noFinalImageReportData(i).VendorStyleNumber))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(noFinalImageReportData(i).ColorCode))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(noFinalImageReportData(i).ColorDesc))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(noFinalImageReportData(i).OnOrder))
            sbHtml.AppendLine("</td>")
            sbHtml.Append("       <td>")
            sbHtml.Append(HttpUtility.HtmlEncode(noFinalImageReportData(i).StartShipDate))
            sbHtml.AppendLine("</td>")
            sbHtml.AppendLine("   </tr>")
        Next
        sbHtml.AppendLine("</table>")
        sw.Write(sbHtml.ToString)
        sw.Close()

    End Sub
    Public Shared Sub ExportNoSampleReportToExcel(ByVal excelFileName As String, ByVal pageTitle As String, ByVal noSampleReportData As List(Of NoSampleReportInfo))
        Dim workbookPart As WorkbookPart = Nothing
        Dim worksheetPart As WorksheetPart = Nothing
        Dim stylesheetPart As WorkbookStylesPart = Nothing
        Dim workbook As Workbook = Nothing
        Dim sheets As Sheets = Nothing
        Dim sheet As Sheet = Nothing
        Dim worksheet As Worksheet = Nothing
        Dim sheetData As SheetData = Nothing
        Dim excelRow As Row = Nothing

        Try
            Using excelPackage As SpreadsheetDocument = SpreadsheetDocument.Create(excelFileName, SpreadsheetDocumentType.Workbook)

                workbookPart = excelPackage.AddWorkbookPart()

                workbook = New Workbook()
                workbook.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships")

                sheets = New Sheets()
                sheet = New Sheet With {.Name = "Sheet1", .SheetId = 1, .Id = "r1"}

                sheets.Append(sheet)
                workbook.Append(sheets)
                workbookPart.Workbook = workbook

                worksheetPart = workbookPart.AddNewPart(Of WorksheetPart)("r1")

                worksheet = New Worksheet()
                sheetData = New SheetData()

                WriteListToExcelWorksheet(noSampleReportData, worksheetPart, worksheet, sheetData)

            End Using
        Catch ex As Exception
            Throw
        End Try

    End Sub
    Public Shared Function GetExcelColumnName(ByVal columnIndex As Integer) As String
        If (columnIndex < 26) Then
            Return Chr(Asc("A") + columnIndex)
        End If

        Dim firstChar As Char,
            secondChar As Char

        firstChar = Chr(Asc("A") + (columnIndex \ 26) - 1)
        secondChar = Chr(Asc("A") + (columnIndex Mod 26))

        Return firstChar + secondChar
    End Function
    Private Shared Sub WriteListToExcelWorksheet(ByVal noSampleReportData As List(Of NoSampleReportInfo), ByVal worksheetPart As WorksheetPart,
                                                 ByVal worksheet As Worksheet, ByVal sheetData As SheetData)

        Dim cellValue As String = String.Empty
        Dim columnIndex As Integer = 0

        '  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
        '
        '  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
        '  cells of data, we'll know if to write Text values or Numeric cell values.
        Dim numberOfColumns As Integer = 13
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
        headerRow = GetReportHeaders()
        sheetData.Append(headerRow)

        '
        '  Now, step through each row of data in our DataTable...
        '
        Dim cellNumericValue As Double = 0

        If Not noSampleReportData Is Nothing AndAlso noSampleReportData.Count > 0 Then

            For Each item As NoSampleReportInfo In noSampleReportData
                ' ...create a new row, and append a set of this row's data to it.
                rowIndex = rowIndex + 1
                Dim newExcelRow As New Row
                newExcelRow.RowIndex = rowIndex         '  add a row at the top of spreadsheet
                sheetData.Append(newExcelRow)

                columnIndex = 0
                'Buyer
                cellValue = item.BuyerId_With_Desc
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Department
                columnIndex = columnIndex + 1
                cellValue = item.DeptId_With_Desc
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Vendor
                columnIndex = columnIndex + 1
                cellValue = item.VendorId_With_Desc
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'ISN
                columnIndex = columnIndex + 1
                cellValue = item.ISN.ToString()
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'ISN Description
                columnIndex = columnIndex + 1
                cellValue = item.ISNDesc
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Vendor Style
                columnIndex = columnIndex + 1
                cellValue = item.VendorStyleNumber
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Color
                columnIndex = columnIndex + 1
                cellValue = item.ColorCode.ToString()
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Color Description
                columnIndex = columnIndex + 1
                cellValue = item.ColorDesc
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'OO Qty
                columnIndex = columnIndex + 1
                cellValue = item.OnOrder.ToString()
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Ship DAte
                columnIndex = columnIndex + 1
                cellValue = item.StartShipDate
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Merch ID
                columnIndex = columnIndex + 1
                cellValue = item.TurnInMerchId
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Merch Status
                columnIndex = columnIndex + 1
                cellValue = item.TurnInMerchStatus
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)

                'Merch Approval
                columnIndex = columnIndex + 1
                If item.IsVendorImage Then
                    cellValue = "Submit Image"
                Else
                    cellValue = IIf(item.TurnInMerchStatus = "Received" AndAlso HttpUtility.HtmlEncode(item.SampleApprovalFlag) = "N", "Approval Needed", String.Empty)
                End If
                AppendCell(excelColumnNames(columnIndex) + rowIndex.ToString(), cellValue, newExcelRow, CellValues.String)
            Next
        End If

        worksheet.Append(sheetData)
        worksheetPart.Worksheet = worksheet

    End Sub
    Private Shared Function GetReportHeaders() As Row
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
        excelRow.Append(AddHeaderColumn("Buyer", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Department", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Vendor", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("ISN", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("ISN Description", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Vendor Style", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Color", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Color Description", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("OO Qty", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Ship Date", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Merch ID", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Merch Status", GetExcelColumnName(columnCount)))

        columnCount += 1
        excelRow.Append(AddHeaderColumn("Merch Approval", GetExcelColumnName(columnCount)))

        Return excelRow
    End Function
    Public Shared Sub AppendCell(ByVal cellReference As String, ByVal cellStringValue As String, ByVal excelRow As Row, ByVal cellDataType As CellValues)
        '/  Add a new Excel Cell to our Row 
        Dim cell As New Cell
        cell.CellReference = cellReference
        cell.DataType = cellDataType

        Dim cellValue As New CellValue
        cellValue.Text = cellStringValue

        cell.Append(cellValue)

        excelRow.Append(cell)
    End Sub
    Private Shared Function AddStyleSheet(ByVal spreadsheet As SpreadsheetDocument) As WorkbookStylesPart
        Dim stylesheet As WorkbookStylesPart = spreadsheet.WorkbookPart.AddNewPart(Of WorkbookStylesPart)()

        Dim workbookstylesheet As New Stylesheet()
        Dim font0 As New Spreadsheet.Font()

        'Dim font1 As New Spreadsheet.Font()
        'Dim boldFont As New Spreadsheet.Bold()
        'Dim underline As New Spreadsheet.Underline()
        'font1.Append(boldFont)
        'font1.Append(underline)
        Dim fonts As New Fonts()
        fonts.Append(font0)
        'fonts.Append(font1)

        Dim border0 As New Border()
        Dim borders As New Borders()
        borders.Append(border0)

        Dim cellFormat0 As New CellFormat With {.FontId = 0, .BorderId = 0}
        'Dim cellFormat1 As New CellFormat With {.FontId = 1}

        Dim cellFormats As New CellFormats()
        cellFormats.Append(cellFormat0)
        'cellFormats.Append(cellFormat1)

        'Append FONTS, FILLS , BORDERS & CellFormats to stylesheet <Preserve the ORDER>
        workbookstylesheet.Append(fonts)
        workbookstylesheet.Append(borders)
        workbookstylesheet.Append(cellFormats)

        'Finalize
        stylesheet.Stylesheet = workbookstylesheet
        stylesheet.Stylesheet.Save()

        Return stylesheet

    End Function

    Private Shared Function AddHeaderColumn(ByVal columnHeader As String, ByVal cellReference As StringValue) As Cell
        Dim excelCell As Cell = Nothing
        Dim cellText As InlineString = Nothing
        Dim cellValue As CellValue = Nothing
        Dim runCell As Run = Nothing

        excelCell = New Cell With {.CellReference = cellReference, .DataType = CellValues.InlineString}
        runCell = New Run()
        runCell.Append(New Text(columnHeader))
        runCell.RunProperties = New RunProperties(New Bold, New Underline)
        cellText = New InlineString()
        cellText.Append(runCell)
        excelCell.Append(cellText)

        Return excelCell
    End Function
End Class