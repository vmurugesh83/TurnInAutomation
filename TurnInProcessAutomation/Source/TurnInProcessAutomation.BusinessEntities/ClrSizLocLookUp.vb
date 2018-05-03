Imports System.ComponentModel

<Serializable()> _
Public Class ClrSizLocLookUp
    'Internal member variables
    Private _value As String
    Private _text As String
    Private _identifier As String

    'Default constructor
    Public Sub New()
    End Sub

    <DataObjectField(True)> _
    Public Property Value() As String
        Get
            Return _value
        End Get
        Set(ByVal value As String)
            _value = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property Text() As String
        Get
            Return _text
        End Get
        Set(ByVal value As String)
            _text = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property Identifier() As String
        Get
            Return _identifier
        End Get
        Set(ByVal value As String)
            _identifier = value
        End Set
    End Property
End Class
