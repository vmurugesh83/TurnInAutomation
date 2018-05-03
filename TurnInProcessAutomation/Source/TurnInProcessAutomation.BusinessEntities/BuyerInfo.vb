
Imports System.ComponentModel

<Serializable()> _
Partial Public Class BuyerInfo

    'Internal member variables
    Private _BuyerId As Integer
    Private _BuyerName As String
    Private _BuyerDesc As String

    'Default constructor
    Public Sub New()
    End Sub


    <DataObjectField(True)> _
    Public Property BuyerId() As Integer
        Get
            Return _BuyerId
        End Get
        Set(ByVal value As Integer)
            _BuyerId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property BuyerName() As String
        Get
            Return _BuyerName
        End Get
        Set(ByVal value As String)
            _BuyerName = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property BuyerDesc() As String
        Get
            Return _BuyerDesc
        End Get
        Set(ByVal value As String)
            _BuyerDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public ReadOnly Property BuyerNameId() As String
        Get
            Return _BuyerName.Trim & " - " & _BuyerId.ToString.Trim
        End Get
    End Property

    Private _email As String
    <DataObjectField(True)> _
    Public Property BuyerOfficeEmail() As String
        Get
            Return _email
        End Get
        Set(ByVal value As String)
            _email = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property BuyerRACFID() As String

End Class

