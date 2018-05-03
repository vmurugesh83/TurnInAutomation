
Imports System.ComponentModel

<Serializable()> _
Public Class TIAExceptionsReportInfo

    Private _BatchId As Integer
    Private _ItemId As Integer
    Private _ItemType As String
    Private _BatchStatus As String

    <DataObjectField(True)> _
    Public Property BatchId() As Integer
        Get
            Return _BatchId
        End Get
        Set(ByVal value As Integer)
            _BatchId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ItemId() As Integer
        Get
            Return _ItemId
        End Get
        Set(ByVal value As Integer)
            _ItemId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ItemType() As String
        Get
            Return _ItemType
        End Get
        Set(ByVal value As String)
            _ItemType = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property BatchStatus() As String
        Get
            Return _BatchStatus
        End Get
        Set(ByVal value As String)
            _BatchStatus = value
        End Set
    End Property

End Class
