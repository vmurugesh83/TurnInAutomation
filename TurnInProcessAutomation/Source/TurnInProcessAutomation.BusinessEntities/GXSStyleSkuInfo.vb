Imports System.ComponentModel
Imports System.IO
Imports System.Xml.Serialization
Imports System.Text

Public Class GXSStyleSkuInfo

    Private _UPC_NUM As Decimal
    <DataObjectField(False)> _
    Public Property UPC_NUM() As Decimal
        Get
            Return _UPC_NUM
        End Get
        Set(ByVal value As Decimal)
            _UPC_NUM = value
        End Set
    End Property

    Private _SKU_NUM As Decimal
    <DataObjectField(False)> _
    Public Property SKU_NUM() As Decimal
        Get
            Return _SKU_NUM
        End Get
        Set(ByVal value As Decimal)
            _SKU_NUM = value
        End Set
    End Property

    Private _INTERNAL_STYLE_NUM As Decimal
    <DataObjectField(False)> _
    Public Property INTERNAL_STYLE_NUM() As Decimal
        Get
            Return _INTERNAL_STYLE_NUM
        End Get
        Set(ByVal value As Decimal)
            _INTERNAL_STYLE_NUM = value
        End Set
    End Property

    Private _Fabrication As String
    <DataObjectField(False)> _
    Public Property Fabrication() As String
        Get
            Return _Fabrication
        End Get
        Set(ByVal value As String)
            _Fabrication = value.Trim
        End Set
    End Property

    Private _SellingLoc As String
    <DataObjectField(False)> _
    Public Property SellingLoc() As String
        Get
            Return _SellingLoc
        End Get
        Set(ByVal value As String)
            _SellingLoc = value.Trim
        End Set
    End Property

    Private _ProdDtl1 As String
    <DataObjectField(False)> _
    Public Property ProdDtl1() As String
        Get
            Return _ProdDtl1
        End Get
        Set(ByVal value As String)
            _ProdDtl1 = value.Trim
        End Set
    End Property

    Private _ProdDtl2 As String
    <DataObjectField(False)> _
    Public Property ProdDtl2() As String
        Get
            Return _ProdDtl2
        End Get
        Set(ByVal value As String)
            _ProdDtl2 = value.Trim
        End Set
    End Property

    Private _ProdDtl3 As String
    <DataObjectField(False)> _
    Public Property ProdDtl3() As String
        Get
            Return _ProdDtl3
        End Get
        Set(ByVal value As String)
            _ProdDtl3 = value.Trim
        End Set
    End Property

    Private _AssembledIn As String
    <DataObjectField(False)> _
    Public Property AssembledIn() As String
        Get
            Return _AssembledIn
        End Get
        Set(ByVal value As String)
            _AssembledIn = value.Trim
        End Set
    End Property

    Private _GenClass As String
    <DataObjectField(False)> _
    Public Property GenClass() As String
        Get
            Return _GenClass
        End Get
        Set(ByVal value As String)
            _GenClass = value.Trim
        End Set
    End Property

    Private _GenSubcl As String
    <DataObjectField(False)> _
    Public Property GenSubcl() As String
        Get
            Return _GenSubcl
        End Get
        Set(ByVal value As String)
            _GenSubcl = value.Trim
        End Set
    End Property

    Private _Brand As String
    <DataObjectField(False)> _
    Public Property Brand() As String
        Get
            Return _Brand
        End Get
        Set(ByVal value As String)
            _Brand = value.Trim
        End Set
    End Property

    Private _Label As String
    <DataObjectField(False)> _
    Public Property Label() As String
        Get
            Return _Label
        End Get
        Set(ByVal value As String)
            _Label = value.Trim
        End Set
    End Property

    Private _FabDtl As String
    <DataObjectField(False)> _
    Public Property FabDtl() As String
        Get
            Return _FabDtl
        End Get
        Set(ByVal value As String)
            _FabDtl = value.Trim
        End Set
    End Property

    Private _Lifestyle As String
    <DataObjectField(False)> _
    Public Property Lifestyle() As String
        Get
            Return _Lifestyle
        End Get
        Set(ByVal value As String)
            _Lifestyle = value.Trim
        End Set
    End Property

    Private _Season As String
    <DataObjectField(False)> _
    Public Property Season() As String
        Get
            Return _Season
        End Get
        Set(ByVal value As String)
            _Season = value.Trim
        End Set
    End Property

    Private _Occasion As String
    <DataObjectField(False)> _
    Public Property Occasion() As String
        Get
            Return _Occasion
        End Get
        Set(ByVal value As String)
            _Occasion = value.Trim
        End Set
    End Property

    Private _Theme As String
    <DataObjectField(False)> _
    Public Property Theme() As String
        Get
            Return _Theme
        End Get
        Set(ByVal value As String)
            _Theme = value.Trim
        End Set
    End Property

    Public Shared Function Serialize(ByVal obj As Object) As String

        Dim stream As New MemoryStream
        Dim ds As New XmlSerializer(GetType(GXSStyleSkuInfo), "http://schema.bonton.com/Schema/SampleSchema")
        ds.Serialize(stream, obj)
        Dim xmlString As String = Encoding.UTF8.GetString(stream.ToArray())
        stream.Close()
        Return xmlString

    End Function

End Class
