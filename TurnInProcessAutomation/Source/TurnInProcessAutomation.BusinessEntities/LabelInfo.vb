
Imports System.ComponentModel

<Serializable()> _
Partial Public Class LabelInfo

    'Internal member variables
    Private _LabelId As Integer
    Private _LabelDesc As String


    'Default constructor
    Public Sub New()
    End Sub

    <DataObjectField(True)> _
    Public Property LabelId() As Integer
        Get
            Return _LabelId
        End Get
        Set(ByVal value As Integer)
            _LabelId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property LabelDesc() As String
        Get
            Return _LabelDesc
        End Get
        Set(ByVal value As String)
            _LabelDesc = value
        End Set
    End Property


End Class

