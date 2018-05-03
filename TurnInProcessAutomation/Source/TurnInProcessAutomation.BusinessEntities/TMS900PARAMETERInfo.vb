
Imports System.ComponentModel

<Serializable()> _
Partial Public Class TMS900PARAMETERInfo

    'Internal member variables
    Private _CharIndex As String
    Private _ShortDesc As String
    Private _LongDesc As String
    Private _LIST_SEQ_NUM As Integer

    'Default constructor
    Public Sub New()
    End Sub

    <DataObjectField(True)> _
    Public Property CharIndex() As String
        Get
            Return _CharIndex
        End Get
        Set(ByVal value As String)
            _CharIndex = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ShortDesc() As String
        Get
            Return _ShortDesc
        End Get
        Set(ByVal value As String)
            _ShortDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property LongDesc() As String
        Get
            Return _LongDesc
        End Get
        Set(ByVal value As String)
            _LongDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property LIST_SEQ_NUM() As Integer
        Get
            Return _LIST_SEQ_NUM
        End Get
        Set(ByVal value As Integer)
            _LIST_SEQ_NUM = value
        End Set
    End Property
End Class

