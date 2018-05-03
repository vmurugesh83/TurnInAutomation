Imports System.Xml
Imports System.Xml.Linq
Imports System.Xml.Serialization
Imports TurnInProcessAutomation.BLL.MiscConstants
Imports TurnInProcessAutomation.BusinessEntities

''' <summary>
''' SessionWrapper provides a storage space for all session objects for easy accessibility and maintenance.
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class SessionWrapper

#Region "Members"

    Private Log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    'constants that represent the "key" for session object values

    Private Const _XMLMenuItems As String = "XMLMenuItems"
    Private Const _XMLTabStrip As String = "XMLTabStrip"
    Private Const _ContentTreeView As String = "ContentTreeView"
    Private Const _ToolbarButtons As String = "ToolBarButtons"
    Private Const _ViewOnly As String = "ViewOnly"

    Private Const _UserID As String = "UserID"
    Private Const _UserRoles As String = "UserRoles"
    Private Const _GUID As String = "GUID"

    Private Const _UserIP As String = "UserIP"
    Private Const _Messages As String = "Session_Messages"
#End Region


#Region "Properties"
    Public Shared Property Action() As String
        Get
            Return CStr(HttpContext.Current.Session(SES_PAGEACTION))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session(SES_PAGEACTION) = value
        End Set
    End Property

    ''' <summary>
    ''' The user name of the current session
    ''' </summary>    
    Public Shared ReadOnly Property Username() As String
        Get
            Return HttpContext.Current.User.Identity.Name
        End Get
    End Property

    ''' <summary>
    ''' The server name that is running the web application
    ''' </summary>    
    Public Shared ReadOnly Property Servername() As String
        Get
            Return HttpContext.Current.Server.MachineName
        End Get
    End Property

    ''' <summary>
    ''' Display web page in a view only state
    ''' </summary>
    Public Shared Property ViewOnly() As Boolean
        Get
            Return CBool(HttpContext.Current.Session(_ViewOnly))
        End Get
        Set(ByVal value As Boolean)
            HttpContext.Current.Session(_ViewOnly) = value
        End Set
    End Property

    ''' <summary>
    ''' The string that stores the application menu items
    ''' </summary>    
    Public Shared Property XMLMenu() As String
        Get
            Return CStr(HttpContext.Current.Session(_XMLMenuItems))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session(_XMLMenuItems) = value
        End Set
    End Property

    Public Shared Property XMLTabStrip() As String
        Get
            Return CStr(HttpContext.Current.Session(_XMLTabStrip))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session(_XMLTabStrip) = value
        End Set
    End Property

    ''' <summary>
    ''' Stores the toolbar xml button string
    ''' </summary>   
    Public Shared Property ToolBarButtons() As ToolBarButtons
        Get
            Return CType(HttpContext.Current.Session(_ToolbarButtons), ToolBarButtons)
        End Get
        Set(ByVal value As ToolBarButtons)
            HttpContext.Current.Session(_ToolbarButtons) = value
        End Set
    End Property

    '''' <summary>
    '''' Content Tree View XML object
    '''' </summary>    
    ''Public Shared Property ContentTreeView() As ContentTreeView
    ''    Get
    ''        Return CType(HttpContext.Current.Session(_contentTreeView), ContentTreeView)
    ''    End Get
    ''    Set(ByVal value As ContentTreeView)
    ''        HttpContext.Current.Session(_contentTreeView) = value
    ''    End Set
    ''End Property

    ''' <summary>
    ''' User ID
    ''' </summary>    
    Public Shared Property UserID() As String
        Get
            Return CStr(HttpContext.Current.Session(_UserID))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session(_UserID) = value
        End Set
    End Property

    ''' <summary>
    ''' User ID
    ''' </summary>    
    Public Shared Property UserRoles() As List(Of String)
        Get
            Return CType(HttpContext.Current.Session(_UserRoles), List(Of String))
        End Get
        Set(ByVal value As List(Of String))
            HttpContext.Current.Session(_UserRoles) = value
        End Set
    End Property

    ''' <summary>
    ''' GUID - unique session identifier
    ''' </summary>    
    Public Shared Property GUID() As String
        Get
            Return CStr(HttpContext.Current.Session(_GUID))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session(_GUID) = value
        End Set
    End Property

    ''' <summary>
    ''' User IP address
    ''' </summary>    
    Public Shared Property UserIP() As String
        Get
            Return CStr(HttpContext.Current.Session(_UserIP))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session(_UserIP) = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the messages from the ValidationMessages.xml file.
    ''' </summary>    
    Public Shared ReadOnly Property Messages() As List(Of Message)
        Get
            If Not IsSessionItemExist(_Messages) Then
                Dim collection As List(Of Message)

                Try
                    Dim xDoc As XDocument = XDocument.Load(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings("ValidationMessageFilePath")))
                    collection = DirectCast(New XmlSerializer(GetType(ApplicationMessages)) _
                        .Deserialize(xDoc.CreateReader()), ApplicationMessages) _
                        .Messages
                    
                Catch ex As IO.FileNotFoundException
                    Throw New ApplicationException("Messages XML file not found.")
                Catch ex As Exception
                    Throw New ApplicationException("Error occurred while parsing Messages XML file.")
                End Try

                SessionWrapper.AddSessionItem(_Messages, collection)
            End If

            Return DirectCast(SessionWrapper.GetSessionItem(_Messages), List(Of Message))
        End Get
    End Property
#End Region

#Region "Methods"

    ''' <summary>
    ''' Remove Session item from Session collection
    ''' </summary>
    ''' <param name="sessionKey"></param>
    ''' <remarks></remarks>
    Public Shared Sub RemoveSessionItem(ByVal sessionKey As String)

        If Not HttpContext.Current.Session.Item(sessionKey) Is Nothing Then
            HttpContext.Current.Session.Remove(sessionKey)
        End If

    End Sub

    ''' <summary>
    ''' Add Session item
    ''' </summary>
    ''' <param name="sessionKey"></param>
    ''' <param name="sessionValue"></param>
    ''' <remarks>if any item is already in session, that will be reinitialized</remarks>
    Public Shared Sub AddSessionItem(ByVal sessionKey As String, ByVal sessionValue As Object)

        If HttpContext.Current.Session.Item(sessionKey) Is Nothing Then
            HttpContext.Current.Session.Add(sessionKey, sessionValue)
        Else
            HttpContext.Current.Session(sessionKey) = sessionValue
        End If

    End Sub

    ''' <summary>
    ''' Get a specific session item
    ''' </summary>
    ''' <param name="sessionKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetSessionItem(ByVal sessionKey As String) As Object
        Return HttpContext.Current.Session.Item(sessionKey)
    End Function

    ''' <summary>
    ''' Check if a session item exist or not
    ''' </summary>
    ''' <param name="sessionKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsSessionItemExist(ByVal sessionKey As String) As Boolean
        Return HttpContext.Current.Session.Item(sessionKey) IsNot Nothing
    End Function

#End Region

End Class
