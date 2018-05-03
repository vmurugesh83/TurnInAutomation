
Imports System.ComponentModel

<Serializable()> _
Public Class CtlgAdPgInfo

    Private _AdNbr As Integer
    Private _SysPgNbr As Integer
    Private _PgDesc As String
    Private _PgNbr As Integer
    Private _CvrPgInd As String
    Private _LogoPgInd As String
    Private _ColorCd As String
    Private _PaprStkDesc As String
    Private _PaprWghtDesc As String
    Private _BleedHghtQty As Decimal
    Private _BleedWdthQty As Decimal
    Private _LiveHghtQty As Decimal
    Private _LiveWdthQty As Decimal
    Private _TrimHghtQty As Decimal
    Private _TrimWdthQty As Decimal
    Private _FlatHghtQty As Decimal
    Private _FlatWdthQty As Decimal
    Private _PrepressJobNbr As String
    Private _state As String = "Unchanged"

    'Default constructor
    Public Sub New()
    End Sub

    'Constructor with initialized values
    Public Sub New(ByVal AdNbr As Integer, _
                      ByVal SysPgNbr As Integer, _
                      ByVal PgDesc As String, _
                      ByVal PgNbr As Integer)
        Me._AdNbr = AdNbr
        Me._SysPgNbr = SysPgNbr
        Me._PgDesc = PgDesc
        Me._PgNbr = PgNbr
    End Sub

    Public Sub New(ByVal AdNbr As Integer, _
                  ByVal SysPgNbr As Integer, _
                  ByVal PgDesc As String, _
                  ByVal PgNbr As Integer, _
                  ByVal CvrPgInd As String, _
                  ByVal LogoPgInd As String, _
                  ByVal ColorCd As String, _
                  ByVal PaprStkDesc As String, _
                  ByVal PaprWghtDesc As String, _
                  ByVal BleedHghtQty As Decimal, _
                  ByVal BleedWdthQty As Decimal, _
                  ByVal LiveHghtQty As Decimal, _
                  ByVal LiveWdthQty As Decimal, _
                  ByVal TrimHghtQty As Decimal, _
                  ByVal TrimWdthQty As Decimal, _
                  ByVal FlatHghtQty As Decimal, _
                  ByVal FlatWdthQty As Decimal, _
                  ByVal PrepressJobNbr As String)
        Me._AdNbr = AdNbr
        Me._SysPgNbr = SysPgNbr
        Me._PgDesc = PgDesc
        Me._PgNbr = PgNbr
        Me._CvrPgInd = CvrPgInd
        Me._LogoPgInd = LogoPgInd
        Me._ColorCd = ColorCd
        Me._PaprStkDesc = PaprStkDesc
        Me._PaprWghtDesc = PaprWghtDesc
        Me._BleedHghtQty = BleedHghtQty
        Me._BleedWdthQty = BleedWdthQty
        Me._LiveHghtQty = LiveHghtQty
        Me._LiveWdthQty = LiveWdthQty
        Me._TrimHghtQty = TrimHghtQty
        Me._TrimWdthQty = TrimWdthQty
        Me._FlatHghtQty = FlatHghtQty
        Me._FlatWdthQty = FlatWdthQty
        Me._PrepressJobNbr = PrepressJobNbr
    End Sub


    <DataObjectField(True)> _
    Public Property adnbr() As Integer
        Get
            Return _AdNbr
        End Get
        Set(ByVal value As Integer)
            _AdNbr = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property syspgnbr() As Integer
        Get
            Return _SysPgNbr
        End Get
        Set(ByVal value As Integer)
            _SysPgNbr = value
        End Set
    End Property

    Public Property pgdesc() As String
        Get
            Return _PgDesc
        End Get
        Set(ByVal value As String)
            _PgDesc = value
        End Set
    End Property

    Public Property pgnbr() As Integer
        Get
            Return _PgNbr
        End Get
        Set(ByVal value As Integer)
            _PgNbr = value
        End Set
    End Property

    Public ReadOnly Property PageNumberDesc As String
        Get
            Return pgnbr & " - " & pgdesc
        End Get
    End Property

    Public Property cvrpgind() As String
        Get
            Return _CvrPgInd
        End Get
        Set(ByVal value As String)
            _CvrPgInd = value
        End Set
    End Property

    Public Property logopgind() As String
        Get
            Return _LogoPgInd
        End Get
        Set(ByVal value As String)
            _LogoPgInd = value
        End Set
    End Property

    Public Property colorcd() As String
        Get
            Return _ColorCd
        End Get
        Set(ByVal value As String)
            _ColorCd = value
        End Set
    End Property

    Public Property paprstkdesc() As String
        Get
            Return _PaprStkDesc
        End Get
        Set(ByVal value As String)
            _PaprStkDesc = value
        End Set
    End Property

    Public Property paprwghtdesc() As String
        Get
            Return _PaprWghtDesc
        End Get
        Set(ByVal value As String)
            _PaprWghtDesc = value
        End Set
    End Property

    Public Property bleedhghtqty() As Decimal
        Get
            Return _BleedHghtQty
        End Get
        Set(ByVal value As Decimal)
            _BleedHghtQty = value
        End Set
    End Property

    Public Property bleedwdthqty() As Decimal
        Get
            Return _BleedWdthQty
        End Get
        Set(ByVal value As Decimal)
            _BleedWdthQty = value
        End Set
    End Property

    Public Property livehghtqty() As Decimal
        Get
            Return _LiveHghtQty
        End Get
        Set(ByVal value As Decimal)
            _LiveHghtQty = value
        End Set
    End Property

    Public Property livewdthqty() As Decimal
        Get
            Return _LiveWdthQty
        End Get
        Set(ByVal value As Decimal)
            _LiveWdthQty = value
        End Set
    End Property

    Public Property trimhghtqty() As Decimal
        Get
            Return _TrimHghtQty
        End Get
        Set(ByVal value As Decimal)
            _TrimHghtQty = value
        End Set
    End Property

    Public Property trimwdthqty() As Decimal
        Get
            Return _TrimWdthQty
        End Get
        Set(ByVal value As Decimal)
            _TrimWdthQty = value
        End Set
    End Property

    Public Property flathghtqty() As Decimal
        Get
            Return _FlatHghtQty
        End Get
        Set(ByVal value As Decimal)
            _FlatHghtQty = value
        End Set
    End Property

    Public Property flatwdthqty() As Decimal
        Get
            Return _FlatWdthQty
        End Get
        Set(ByVal value As Decimal)
            _FlatWdthQty = value
        End Set
    End Property

    Public Property prepressjobnbr() As String
        Get
            Return _PrepressJobNbr
        End Get
        Set(ByVal value As String)
            _PrepressJobNbr = value
        End Set
    End Property

    Public Property state() As String
        Get
            Return _state
        End Get
        Set(ByVal value As String)
            _state = value
        End Set
    End Property

End Class

