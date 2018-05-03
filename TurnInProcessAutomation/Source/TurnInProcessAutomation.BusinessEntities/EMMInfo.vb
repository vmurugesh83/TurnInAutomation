Imports System.ComponentModel

<Serializable()> _
Public Class EMMInfo
    'Internal member variables
    Private _EMMId As Short
    Private _EMMDesc As String

    'Default constructor
    Public Sub New()
    End Sub


    <DataObjectField(True)> _
    Public Property EMMId() As Short
        Get
            Return _EMMId
        End Get
        Set(ByVal value As Short)
            _EMMId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property EMMDesc() As String
        Get
            Return _EMMDesc
        End Get
        Set(ByVal value As String)
            _EMMDesc = value
        End Set
    End Property
End Class
