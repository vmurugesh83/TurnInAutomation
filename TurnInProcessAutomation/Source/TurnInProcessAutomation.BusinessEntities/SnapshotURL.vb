Imports System.ComponentModel

Public Class URL

    Private _actual As String = String.Empty
    <DataObjectField(True)> _
    Public Property ActualURL() As String
        Get
            Return _actual
        End Get
        Set(ByVal value As String)
            _actual = value
        End Set
    End Property

    Private _thumbnail As String = String.Empty
    <DataObjectField(True)> _
    Public Property ThumbnailURL() As String
        Get
            Return _thumbnail
        End Get
        Set(ByVal value As String)
            _thumbnail = value
        End Set
    End Property

    Private _medium As String = String.Empty
    <DataObjectField(True)> _
    Public Property MediumURL() As String
        Get
            Return _medium
        End Get
        Set(ByVal value As String)
            _medium = value
        End Set
    End Property

End Class
