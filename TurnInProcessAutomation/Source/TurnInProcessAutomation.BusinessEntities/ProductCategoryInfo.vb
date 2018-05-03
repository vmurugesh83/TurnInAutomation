Imports System.Net

Public Class ProductCategoryInfo

    Private _CategoryCode As Integer
    Private _ParentCategoryCode As Integer
    Private _CategoryName As String
    Private _level As Integer
    Private _ordinal As Integer

    Public Sub New()
    End Sub

    Public Property CategoryCode() As Integer
        Get
            Return _CategoryCode
        End Get
        Set(ByVal value As Integer)
            _CategoryCode = value
        End Set
    End Property

    Public Property ParentCategoryCode() As Integer
        Get
            Return _ParentCategoryCode
        End Get
        Set(ByVal value As Integer)
            _ParentCategoryCode = value
        End Set
    End Property

    Public Property CategoryName() As String
        Get
            Return _CategoryName
        End Get
        Set(ByVal value As String)
            _CategoryName = value
        End Set
    End Property

    Public Property Level() As Integer
        Get
            Return _level
        End Get
        Set(ByVal value As Integer)
            _level = value
        End Set
    End Property

    Public Property Ordinal() As Integer
        Get
            Return _ordinal
        End Get
        Set(ByVal value As Integer)
            _ordinal = value
        End Set
    End Property

End Class

