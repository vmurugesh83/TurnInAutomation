
Imports System.ComponentModel

<Serializable()> _
Partial Public Class IsTurnedIn

    'Internal member variables
    Private _ISN As Decimal
    Private _AdNumber As Integer
    Private _TUType As Char
    Private _StyleNumber As String = ""
    Private _ColorDesc As String = ""
    Private _VendorColorCode As Integer

    'Default constructor
    Public Sub New()
    End Sub

    <DataObjectField(True)> _
    Public Property ISN() As Decimal
        Get
            Return _ISN
        End Get
        Set(ByVal value As Decimal)
            _ISN = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property AdNumber() As Integer
        Get
            Return _AdNumber
        End Get
        Set(ByVal value As Integer)
            _AdNumber = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property TUType() As Char
        Get
            Return _TUType
        End Get
        Set(ByVal value As Char)
            _TUType = value
        End Set
    End Property

    Public Property StyleNumber() As String
        Get
            Return _StyleNumber
        End Get
        Set(ByVal value As String)
            _StyleNumber = value
        End Set
    End Property

    Public Property ColorDesc() As String
        Get
            Return _ColorDesc
        End Get
        Set(ByVal value As String)
            _ColorDesc = value
        End Set
    End Property

    Public Property VendorColorCode() As Integer
        Get
            Return _VendorColorCode
        End Get
        Set(ByVal value As Integer)
            _VendorColorCode = value
        End Set
    End Property

End Class

