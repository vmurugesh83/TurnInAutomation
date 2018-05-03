
Imports System.ComponentModel

<Serializable()> _
Partial Public Class SellYearInfo

    'Internal member variables
    Private _SellYear As String

    'Default constructor
    Public Sub New()
    End Sub


    <DataObjectField(True)> _
    Public Property SellYear As String
        Get
            Return _SellYear
        End Get
        Set(ByVal value As String)
            _SellYear = value
        End Set
    End Property

End Class

