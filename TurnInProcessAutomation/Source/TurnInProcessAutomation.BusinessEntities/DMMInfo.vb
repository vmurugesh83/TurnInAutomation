Imports System.ComponentModel
<Serializable()> _
Public Class DMMInfo
    'Internal member variables
    Private _DMMId As Short
    Private _DMMDesc As String

    'Default constructor
    Public Sub New()
    End Sub


    <DataObjectField(True)> _
    Public Property DMMId() As Short
        Get
            Return _DMMId
        End Get
        Set(ByVal value As Short)
            _DMMId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property DMMDesc() As String
        Get
            Return _DMMDesc
        End Get
        Set(ByVal value As String)
            _DMMDesc = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public ReadOnly Property DMMIDDesc() As String
        Get
            Return String.Concat(_DMMId, " - ", _DMMDesc)
        End Get
    End Property

End Class


