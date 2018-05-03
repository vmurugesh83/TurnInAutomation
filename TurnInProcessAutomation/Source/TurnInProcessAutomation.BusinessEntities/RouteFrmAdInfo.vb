Imports System.ComponentModel

<Serializable()> _
Public Class RouteFrmAdInfo
    'Internal member variables
    Private _adNbr As Integer

    'Default constructor
    Public Sub New()
    End Sub

    <DataObjectField(True)> _
    Public Property AdNbr() As Integer
        Get
            Return _adNbr
        End Get
        Set(ByVal value As Integer)
            _adNbr = value
        End Set
    End Property
End Class
