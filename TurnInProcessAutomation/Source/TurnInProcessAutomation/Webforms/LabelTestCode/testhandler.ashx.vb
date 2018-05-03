Imports System.Web
Imports System.Web.Services
Imports System.Threading
'Imports Microsoft.Reporting.WebForms

Public Class testhandler
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim MerchID As String = String.Empty

        'context.Response.ContentType = "text/plain"
        'context.Request.Form("MerchID")
        MerchID = "123"
        'context.Response.Write(MerchID)

        'PrintReport(context)
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    'Private Sub PrintReport(ByVal context As HttpContext)
    '    Dim rptVwrColorSize As New ReportViewer
    '    Dim strReportURL As String = "http://ssrs2.test.bonton.com/ReportServer"
    '    Dim strRptPathName As String = "/Turn In Reports/eComm TI Report"

    '    Dim warnings As Warning()
    '    Dim streamIds As String()
    '    Dim mimeType As String = String.Empty
    '    Dim encoding As String = String.Empty
    '    Dim extension As String = String.Empty
    '    Dim cred As ReportServerCredentials = New ReportServerCredentials("test", "test", "test")

    '    Try

    '        rptVwrColorSize.ShowCredentialPrompts = False
    '        rptVwrColorSize.ProcessingMode = ProcessingMode.Remote
    '        rptVwrColorSize.ServerReport.ReportServerUrl = New System.Uri(strReportURL)
    '        rptVwrColorSize.ServerReport.ReportPath = strRptPathName
    '        rptVwrColorSize.ServerReport.ReportServerCredentials = cred
    '        rptVwrColorSize.ShowParameterPrompts = False

    '        rptVwrColorSize.ServerReport.SetParameters(New ReportParameter("Dept", "380 YC Knits/Wovens"))
    '        rptVwrColorSize.ServerReport.SetParameters(New ReportParameter("Buyer", "Smith – 1234"))
    '        rptVwrColorSize.ServerReport.SetParameters(New ReportParameter("Ad", "2027 - Test"))
    '        rptVwrColorSize.ServerReport.SetParameters(New ReportParameter("PgDesc", "1 - Women"))
    '        rptVwrColorSize.ServerReport.SetParameters(New ReportParameter("TIDate", "08/06/2013"))
    '        rptVwrColorSize.ServerReport.SetParameters(New ReportParameter("BatchID", "1"))

    '        rptVwrColorSize.ServerReport.Refresh()

    '        Dim returnValue As Byte() = rptVwrColorSize.ServerReport.Render("EXCEL", Nothing, mimeType, encoding, extension, streamIds, warnings)

    '        context.Response.Buffer = True
    '        context.Response.Clear()
    '        context.Response.ClearHeaders()
    '        context.Response.ContentType = mimeType
    '        context.Response.AddHeader("content-disposition", "inline; filename=ColorSizeReport." + extension)
    '        context.Response.BinaryWrite(returnValue)
    '        context.Response.Flush()
    '        'context.Response.End()
    '        'Response.Redirect(Request.Url.ToString(), False)
    '        context.ApplicationInstance.CompleteRequest()
    '    Catch ex As Exception
    '    End Try
    'End Sub
End Class