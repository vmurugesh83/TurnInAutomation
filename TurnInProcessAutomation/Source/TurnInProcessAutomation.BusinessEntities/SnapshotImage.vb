Imports System.ComponentModel

Public Class SnapshotImage

    Private _PrimaryView As URL
    <DataObjectField(True)> _
    Public Property PrimaryView() As URL
        Get
            Return _PrimaryView
        End Get
        Set(ByVal value As URL)
            _PrimaryView = value
        End Set
    End Property

    Private _SecondaryView As URL
    <DataObjectField(True)> _
    Public Property SecondaryView() As URL
        Get
            Return _SecondaryView
        End Get
        Set(ByVal value As URL)
            _SecondaryView = value
        End Set
    End Property
End Class
