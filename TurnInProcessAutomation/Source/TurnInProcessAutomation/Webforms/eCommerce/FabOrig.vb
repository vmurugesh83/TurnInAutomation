Public Class FabOrig

    Private _ISN As Integer
    Private _UPC? As Long
    Private _StyleSkuFabDescription As String
    Private _StyleSkuOrigDescription As String
    Private _NeedsFabInfo As Boolean = False
    Private _NeedsOrigInfo As Boolean = False
    Private _fabOriginalValue As String
    Private _fabOriginalSource As String
    Private _origOriginalValue As String
    Private _origOriginalSource As String
    Private _labelId As Integer

    Public Property ISN() As Integer
        Get
            Return _ISN
        End Get
        Set(ByVal value As Integer)
            _ISN = value
        End Set
    End Property

    Public Property UPC() As Long?
        Get
            Return _UPC
        End Get
        Set(ByVal value As Long?)
            _UPC = value
        End Set
    End Property

    Public Property StyleSkuFabDescription() As String
        Get
            Return _StyleSkuFabDescription
        End Get
        Set(ByVal value As String)
            _StyleSkuFabDescription = value
        End Set
    End Property

    Public Property StyleSkuOrigDescription() As String
        Get
            Return _StyleSkuOrigDescription
        End Get
        Set(ByVal value As String)
            _StyleSkuOrigDescription = value
        End Set
    End Property

    Public Property NeedsFabInfo() As Boolean
        Get
            Return _NeedsFabInfo
        End Get
        Set(ByVal value As Boolean)
            _NeedsFabInfo = value
        End Set
    End Property

    Public Property NeedsOrigInfo() As Boolean
        Get
            Return _NeedsOrigInfo
        End Get
        Set(ByVal value As Boolean)
            _NeedsOrigInfo = value
        End Set
    End Property

    Public Property FabricationOriginalValue() As String
        Get
            Return _fabOriginalValue
        End Get
        Set(ByVal value As String)
            _fabOriginalValue = value
        End Set
    End Property

    Public Property FabricationOriginalSource() As String
        Get
            Return _fabOriginalSource
        End Get
        Set(ByVal value As String)
            _fabOriginalSource = value
        End Set
    End Property

    Public Property OriginationOriginalValue() As String
        Get
            Return _origOriginalValue
        End Get
        Set(ByVal value As String)
            _origOriginalValue = value
        End Set
    End Property

    Public Property OriginationOriginalSource() As String
        Get
            Return _origOriginalSource
        End Get
        Set(ByVal value As String)
            _origOriginalSource = value
        End Set
    End Property

    Public Property LabelId() As Integer
        Get
            Return _labelId
        End Get
        Set(ByVal value As Integer)
            _labelId = value
        End Set
    End Property

End Class
