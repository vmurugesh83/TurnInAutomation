Imports System.ComponentModel

<Serializable()> _
Public Class AdPageInfo

    'Internal member variables
    Private _AdNbr As Integer
    Private _PgNbr As Integer
    Private _AdDesc As String
    Private _PageDesc As String
    Private _TUDate As Date
    Private _VendorSTyleNumber As String
    Private _UPCNum As String


    'Default constructor
    Public Sub New()
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
    Public Property PgNbr() As Integer
        Get
            Return _PgNbr
        End Get
        Set(ByVal value As Integer)
            _PgNbr = value
        End Set
    End Property

    Public Property AdDesc() As String
        Get
            Return _AdDesc
        End Get
        Set(ByVal value As String)
            _AdDesc = value
        End Set
    End Property

    Public Property PageDesc() As String
        Get
            Return _PageDesc
        End Get
        Set(ByVal value As String)
            _PageDesc = value
        End Set
    End Property

    Public Property TUDate() As Date
        Get
            Return _TUDate
        End Get
        Set(ByVal value As Date)
            _TUDate = value
        End Set
    End Property

    Public Property VendorISNNumber() As String
        Get
            Return _VendorSTyleNumber
        End Get
        Set(ByVal value As String)
            _VendorSTyleNumber = value
        End Set
    End Property

    Public Property UPCNumber() As String
        Get
            Return _UPCNum
        End Get
        Set(ByVal value As String)
            _UPCNum = value
        End Set
    End Property

End Class

