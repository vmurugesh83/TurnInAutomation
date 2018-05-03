Imports System.ComponentModel

<Serializable()> _
Public Class BrandInfo
    'Internal member variables
    Private _BrandId As Short
    Private _BrandDesc As String

    'Default constructor
    Public Sub New()
    End Sub

    <DataObjectField(True)> _
    Public Property BrandId() As Short
        Get
            Return _BrandId
        End Get
        Set(ByVal value As Short)
            _BrandId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property BrandDesc() As String
        Get
            Return _BrandDesc
        End Get
        Set(ByVal value As String)
            _BrandDesc = value
        End Set
    End Property
End Class
