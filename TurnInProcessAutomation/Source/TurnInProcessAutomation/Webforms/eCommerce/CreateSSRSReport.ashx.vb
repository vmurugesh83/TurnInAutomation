Imports System.Web
Imports System.Web.Services
Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities
Imports Microsoft.Reporting.WebForms

Public Class CreateSSRSReport1
    Implements System.Web.IHttpHandler

    Private strBuyer As String = String.Empty
    Private strAd As String = String.Empty
    Private strPgDesc As String = String.Empty
    Private strTIDate As String = String.Empty
    Private strBatchID As String
    Dim _TUEcommSetupCreate As New TUEcommSetupCreate

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        strBatchID = If((context.Request.QueryString("BatchId") IsNot Nothing), context.Request.QueryString("BatchId").ToString, String.Empty)
        strBuyer = If((context.Request.QueryString("Buyer") IsNot Nothing), context.Request.QueryString("Buyer").ToString, String.Empty)
        strAd = If((context.Request.QueryString("Ad") IsNot Nothing), context.Request.QueryString("Ad").ToString, String.Empty)
        strPgDesc = If((context.Request.QueryString("Pg") IsNot Nothing), context.Request.QueryString("Pg").ToString, String.Empty)
        strTIDate = If((context.Request.QueryString("Dt") IsNot Nothing), context.Request.QueryString("Dt").ToString, String.Empty)

        Dim rptVwrColorSize As New ReportViewer
        Dim strReportURL As String = String.Empty
        Dim strRptPathName As String = String.Empty

        Dim warnings As Warning() = Nothing
        Dim streamIds As String() = Nothing
        Dim mimeType As String = String.Empty
        Dim encoding As String = String.Empty
        Dim extension As String = String.Empty
        Dim credentials As ReportServerCredentials
        Dim UID, PWD, Domain As String

        Try
            UID = ConfigurationManager.AppSettings("SSRS_UID")
            PWD = ConfigurationManager.AppSettings("SSRS_PWD")
            Domain = ConfigurationManager.AppSettings("SSRS_Domain")
            strReportURL = ConfigurationManager.AppSettings("SSRS_URL")
            strRptPathName = ConfigurationManager.AppSettings("SSRS_Path")

            credentials = New ReportServerCredentials(UID, PWD, Domain)

            rptVwrColorSize.ShowCredentialPrompts = False
            rptVwrColorSize.ProcessingMode = ProcessingMode.Remote
            rptVwrColorSize.ServerReport.ReportServerUrl = New System.Uri(strReportURL)
            rptVwrColorSize.ServerReport.ReportPath = strRptPathName
            rptVwrColorSize.ServerReport.ReportServerCredentials = credentials
            rptVwrColorSize.ShowParameterPrompts = False

            rptVwrColorSize.ServerReport.SetParameters(New ReportParameter("Buyer", strBuyer))
            rptVwrColorSize.ServerReport.SetParameters(New ReportParameter("Ad", strAd))
            rptVwrColorSize.ServerReport.SetParameters(New ReportParameter("PgDesc", strPgDesc))
            rptVwrColorSize.ServerReport.SetParameters(New ReportParameter("TIDate", strTIDate))
            rptVwrColorSize.ServerReport.SetParameters(New ReportParameter("BatchID", CStr(strBatchID)))

            rptVwrColorSize.ServerReport.Refresh()

            Dim returnValue As Byte() = rptVwrColorSize.ServerReport.Render("EXCEL", Nothing, mimeType, encoding, extension, streamIds, warnings)

            context.Response.Buffer = True
            context.Response.Clear()
            context.Response.ClearHeaders()
            context.Response.ContentType = mimeType
            context.Response.AddHeader("content-disposition", "inline; filename=TurnInECommReport." + extension)
            context.Response.BinaryWrite(returnValue)
            context.Response.Flush()
            'Response.End()
            Context.ApplicationInstance.CompleteRequest()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class