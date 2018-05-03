Imports System.ComponentModel

<Serializable()> _
Public Class BatchInfo
    'Internal member variables
    Private _batchId As Integer = 0
    Private _AdNumber As Integer
    Private _PageNumber As Integer
    Private _UserId As String
    Private _ColorSizeItems As New List(Of EcommSetupCreateInfo)
    Private _Buyer As String
    Private _Departments As String
    Private _BatchStatus As String

    'Default constructor
    Public Sub New()
    End Sub

    <DataObjectField(True)> _
    Public Property BatchId() As Integer
        Get
            Return _batchId
        End Get
        Set(ByVal value As Integer)
            _batchId = value
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
    Public Property PageNumber() As Integer
        Get
            Return _PageNumber
        End Get
        Set(ByVal value As Integer)
            _PageNumber = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property UserId() As String
        Get
            Return _UserId
        End Get
        Set(ByVal value As String)
            _UserId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ColorSizeItems() As List(Of EcommSetupCreateInfo)
        Get
            Return _ColorSizeItems
        End Get
        Set(ByVal value As List(Of EcommSetupCreateInfo))
            _ColorSizeItems = value
        End Set
    End Property

    Public Property Buyer() As String
        Get
            Return _Buyer
        End Get
        Set(ByVal value As String)
            _Buyer = value
        End Set
    End Property


    Public Property Departments As String
        Get
            Return _Departments
        End Get
        Set(ByVal value As String)
            _Departments = value
        End Set
    End Property

    Public Property BatchStatus As String
        Get
            Return _BatchStatus
        End Get
        Set(ByVal value As String)
            _BatchStatus = value
        End Set
    End Property
End Class

