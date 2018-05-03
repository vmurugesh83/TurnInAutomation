Imports System.ComponentModel
<Serializable()> _
Public Class GMMInfo
    'Internal member variables
    Private _GMMId As Short
    Private _GMMDesc As String

    'Default constructor
    Public Sub New()
    End Sub


    <DataObjectField(True)> _
    Public Property GMMId() As Short
        Get
            Return _GMMId
        End Get
        Set(ByVal value As Short)
            _GMMId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property GMMDesc() As String
        Get
            Return _GMMDesc
        End Get
        Set(ByVal value As String)
            _GMMDesc = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public ReadOnly Property GMMIDDesc() As String
        Get
            Return String.Concat(_GMMId, " - ", _GMMDesc)
        End Get
    End Property

End Class


