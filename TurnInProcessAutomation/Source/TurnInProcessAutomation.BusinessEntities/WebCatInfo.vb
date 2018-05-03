Imports System.Net

Public Class WebCat
    'Internal member variables
    Private _CategoryCode As Integer
    Private _ParentCategoryCode As Integer
    Private _CategoryName As String
    Private _CategoryLongDesc As String
    Private _DefaultCategoryFlag As Boolean = False
    Private _DisplayOnlyFlag As String

    'Default constructor
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

    Public Property CategoryLongDesc() As String
        Get
            Return _CategoryLongDesc
        End Get
        Set(ByVal value As String)
            _CategoryLongDesc = value
        End Set
    End Property

    Public Property DefaultCategoryFlag() As Boolean
        Get
            Return _DefaultCategoryFlag
        End Get
        Set(ByVal value As Boolean)
            _DefaultCategoryFlag = value
        End Set
    End Property

    Public Property DisplayOnlyFlag() As String
        Get
            Return _DisplayOnlyFlag
        End Get
        Set(ByVal value As String)
            _DisplayOnlyFlag = value
        End Set
    End Property

    Public ReadOnly Property CategoryNameDisplayOnlyText As String
        Get
            If DisplayOnlyFlag = "Y" Then
                Return WebUtility.HtmlDecode(CategoryName) + " (Display Only)"
            Else
                Return WebUtility.HtmlDecode(CategoryName)
            End If

        End Get
    End Property
End Class
