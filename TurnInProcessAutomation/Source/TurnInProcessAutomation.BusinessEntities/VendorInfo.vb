
Imports System.ComponentModel

<Serializable()> _
Partial Public Class VendorInfo

    'Default constructor
    Public Sub New()
    End Sub

    Private _VendorId As Integer
    <DataObjectField(True)> _
    Public Property VendorId() As Integer
        Get
            Return _VendorId
        End Get
        Set(ByVal value As Integer)
            _VendorId = value
        End Set
    End Property

    Private _VendorName As String
    <DataObjectField(True)> _
    Public Property VendorName() As String
        Get
            Return _VendorName
        End Get
        Set(ByVal value As String)
            _VendorName = value
        End Set
    End Property

    Public ReadOnly Property VendorIdName As String
        Get
            Return VendorId & " - " & _VendorName
        End Get
    End Property

    Private _ReturnAddress As String
    <DataObjectField(True)> _
    Public Property ReturnAddress() As String
        Get
            Return _ReturnAddress
        End Get
        Set(ByVal value As String)
            _ReturnAddress = value
        End Set
    End Property

    Private _Email As String
    <DataObjectField(True)> _
    Public Property VendorEmail() As String
        Get
            Return _Email
        End Get
        Set(ByVal value As String)
            _Email = value
        End Set
    End Property

   
End Class

