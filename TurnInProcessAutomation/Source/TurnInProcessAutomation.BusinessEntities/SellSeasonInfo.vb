
Imports System.ComponentModel

<Serializable()> _
Partial Public Class SellSeasonInfo

    'Internal member variables
    Private _SellSeasonId As Integer
    Private _SellSeasonDesc As String

    'Default constructor
    Public Sub New()
    End Sub


    <DataObjectField(True)> _
    Public Property SellSeasonId() As Integer
        Get
            Return _SellSeasonId
        End Get
        Set(ByVal value As Integer)
            _SellSeasonId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SellSeasonDesc() As String
        Get
            Return _SellSeasonDesc
        End Get
        Set(ByVal value As String)
            _SellSeasonDesc = value
        End Set
    End Property

    Public ReadOnly Property DescSellSeasonId As String
        Get
            Return SellSeasonId & " - " & SellSeasonDesc
        End Get
    End Property

End Class

