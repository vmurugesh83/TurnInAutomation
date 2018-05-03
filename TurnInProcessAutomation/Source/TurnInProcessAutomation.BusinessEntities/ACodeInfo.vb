
Imports System.ComponentModel

<Serializable()> _
Partial Public Class ACodeInfo

    'Internal member variables
    Private _ACode As String
    Private _ACodeDesc As String


    'Default constructor
    Public Sub New()
    End Sub

    <DataObjectField(True)> _
    Public Property ACode() As String
        Get
            Return _ACode
        End Get
        Set(ByVal value As String)
            _ACode = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ACodeDesc() As String
        Get
            Return _ACodeDesc
        End Get
        Set(ByVal value As String)
            _ACodeDesc = value
        End Set
    End Property

    Public ReadOnly Property ACodeCompoundDesc As String
        Get
            Return ACode & " - " & _ACodeDesc
        End Get
    End Property

End Class

