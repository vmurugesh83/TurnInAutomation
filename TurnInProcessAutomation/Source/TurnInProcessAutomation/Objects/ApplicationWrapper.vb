Public NotInheritable Class ApplicationWrapper
    

#Region "Members"

    Private Log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    'constants that represent the "key" for session object values
    Private Const _db2Connect As String = "DB2Connect"
    Private Const _db2Schema As String = "DB2Schema"
    Private Const _environment As String = "Environment"

    Private Const _rtsConnect As String = "RTSConnect"
    Private Const _rtsSchema As String = "RTSSchema"

#End Region


  
#Region "Enumerators"
#End Region



#Region "Properties"
    ''' <summary>
    ''' DB2 connection string
    ''' </summary>    
    Public Shared Property DB2Connect() As String
        Get
            Return CStr(HttpContext.Current.Application(_db2Connect))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Application(_db2Connect) = value
        End Set
    End Property
    ''' <summary>
    ''' DB2 schema
    ''' </summary>    
    Public Shared Property DB2Schema() As String
        Get
            Return CStr(HttpContext.Current.Application(_db2Schema))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Application(_db2Schema) = value
        End Set
    End Property
    ''' <summary>
    ''' Environment
    ''' </summary>    
    Public Shared Property Environment() As String
        Get
            Return CStr(HttpContext.Current.Application(_environment))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Application(_environment) = value
        End Set
    End Property

    ''' <summary>
    ''' RTS connection string
    ''' </summary>    
    Public Shared Property RTSConnect() As String
        Get
            Return CStr(HttpContext.Current.Application(_rtsConnect))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Application(_rtsConnect) = value
        End Set
    End Property
    ''' <summary>
    ''' RTS schema
    ''' </summary>    
    Public Shared Property RTSSchema() As String
        Get
            Return CStr(HttpContext.Current.Application(_rtsSchema))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Application(_rtsSchema) = value
        End Set
    End Property





#End Region


End Class
