Imports System.ComponentModel

<Serializable()> _
Partial Public Class TEC998PARMInfo
    'Internal member variables
    Private _parmValue As Short
    Private _parmText As String
    Private _parmDefault As Short

    'Default constructor
    Public Sub New()
    End Sub

    <DataObjectField(True)> _
    Public Property ParmValue() As Short
        Get
            Return _parmValue
        End Get
        Set(ByVal value As Short)
            _parmValue = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ParmText() As String
        Get
            Return _parmText
        End Get
        Set(ByVal value As String)
            _parmText = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ParmDefault() As Short
        Get
            Return _parmDefault
        End Get
        Set(ByVal value As Short)
            _parmDefault = value
        End Set
    End Property
End Class
