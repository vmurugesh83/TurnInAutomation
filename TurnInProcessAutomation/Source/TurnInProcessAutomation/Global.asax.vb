Imports System.Web.SessionState
Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities
Imports PeterBlum
Imports System.Net

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        'initialize the log4net logging dll
        log4net.Config.XmlConfigurator.Configure()

        Application("ApplicationName") = "Merchandise Turn-In System"
        Application("AllowUnmatchedUrls") = True

        'set Peter Blum License Keys
        DES.Globals.ProfessionalValidation_LicenseKey = "610-151203273|localhost"
        DES.Globals.MoreValidators_LicenseKey = "620-151203225|localhost"

        If Application("ApplWebCatsObject") Is Nothing Then

            Dim log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
            Dim objGetApplicationObjectsService As New GetGlobalObjectsService
            Try
                objGetApplicationObjectsService.GetAllApplicationObjects()  'this will create/populate all application objects.
            Catch ex As Exception
                log.Error(ex.Message)
            Finally
                objGetApplicationObjectsService = Nothing
                log = Nothing
            End Try
        End If
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
        Dim ex As Exception = Server.GetLastError.GetBaseException()

        'Write to the Log4Net logger
        Dim log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
        log.Error(ex)

        'Store exception for custom error page.
        Application("Exception") = ex
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started         
        Dim log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        'place userid into session
        SessionWrapper.UserID = UCase(ExtractUserName())

        'place useripaddress into session
        SessionWrapper.UserIP = ExtractUserIPAddress()

        'establish unique id for session
        SessionWrapper.GUID = Guid.NewGuid().ToString

        'retrieve and store application menu items  
        Dim secSrv As New SecurityService.SecurityServiceSoapClient()
        Try
            SessionWrapper.XMLMenu = secSrv.GetMenuItems("MerchandiseTurn-InSystem", ExtractUserName())
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            secSrv = Nothing
        End Try

        'load and place ToolBarButtons object into session wrapper.
        Dim toolbarButtons As ToolBarButtons = _
        toolbarButtons.Instance()
        SessionWrapper.ToolBarButtons = toolbarButtons


    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub

    Private Function ExtractUserName() As String
        Dim userPath As String = HttpContext.Current.User.Identity.Name
        Dim splitPath As String() = userPath.Split(New Char() {"\"c})
        Return splitPath((splitPath.Length - 1))
    End Function

    Public Shared Function ExtractUserIPAddress() As String
        Dim strHostName As String
        strHostName = System.Net.Dns.GetHostName()
        ExtractUserIPAddress = System.Net.Dns.GetHostEntry(strHostName).AddressList(1).ToString()
    End Function
End Class