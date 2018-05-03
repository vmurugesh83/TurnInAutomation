Imports System.ComponentModel
Imports System.IO
Imports System.Xml.Serialization
Imports System.Text

Public Class GXSCopyViewInfo

    Private _IMAGE_ID As String
    <DataObjectField(True)> _
    Public Property IMAGE_ID() As String
        Get
            Return _IMAGE_ID
        End Get
        Set(ByVal value As String)
            _IMAGE_ID = value
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

    Private _LABEL As String = String.Empty
    <DataObjectField(False)> _
    Public Property LABEL() As String
        Get
            Return _LABEL
        End Get
        Set(ByVal value As String)
            _LABEL = value
        End Set
    End Property

    Private _PRODUCT_NAME As String = String.Empty
    <DataObjectField(False)> _
    Public Property PRODUCT_NAME() As String
        Get
            Return _PRODUCT_NAME
        End Get
        Set(ByVal value As String)
            _PRODUCT_NAME = value
        End Set
    End Property

    Private _OO As Integer
    <DataObjectField(False)> _
    Public Property OO() As Integer
        Get
            Return _OO
        End Get
        Set(ByVal value As Integer)
            _OO = value
        End Set
    End Property

    Private _OH As Integer
    <DataObjectField(False)> _
    Public Property OH() As Integer
        Get
            Return _OH
        End Get
        Set(ByVal value As Integer)
            _OH = value
        End Set
    End Property

    Private _PO_STARTSHIPDT As String = String.Empty
    <DataObjectField(False)> _
    Public Property PO_STARTSHIPDT() As String
        Get
            Return _PO_STARTSHIPDT
        End Get
        Set(ByVal value As String)
            _PO_STARTSHIPDT = value
        End Set
    End Property

    Dim _PRICESTATUS As String = String.Empty
    <DataObjectField(False)> _
    Public Property PRICESTATUS() As String
        Get
            Return _PRICESTATUS
        End Get
        Set(ByVal value As String)
            _PRICESTATUS = value
        End Set
    End Property

    Dim _FEATUREDCOLOR As String = String.Empty
    <DataObjectField(False)> _
    Public Property FEATUREDCOLOR() As String
        Get
            Return _FEATUREDCOLOR
        End Get
        Set(ByVal value As String)
            _FEATUREDCOLOR = value
        End Set
    End Property

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

    Dim _FEATURE As String = String.Empty
    <DataObjectField(False)> _
    Public Property FEATURE() As String
        Get
            Return _FEATURE
        End Get
        Set(ByVal value As String)
            _FEATURE = value
        End Set
    End Property

    Dim _COLOR As String = String.Empty
    <DataObjectField(False)> _
    Public Property COLOR() As String
        Get
            Return _COLOR
        End Get
        Set(ByVal value As String)
            _COLOR = value
        End Set
    End Property

    Dim _SIZE As String = String.Empty
    <DataObjectField(False)> _
    Public Property SIZE() As String
        Get
            Return _SIZE
        End Get
        Set(ByVal value As String)
            _SIZE = value
        End Set
    End Property

    Dim _PO_STATUS_CDE As String = String.Empty
    <DataObjectField(False)> _
    Public Property PO_STATUS_CDE() As String
        Get
            Return _PO_STATUS_CDE
        End Get
        Set(ByVal value As String)
            _PO_STATUS_CDE = value
        End Set
    End Property

    Public Sub New(ByVal image_id As String, ByVal internal_style_num As Decimal, ByVal label As String, ByVal product_name As String, ByVal oo As Integer, ByVal oh As Integer, _
                   ByVal po_startshipdt As String, ByVal pricestatus As String, ByVal featuredcolor As String, ByVal color As String)
        _IMAGE_ID = image_id
        _INTERNAL_STYLE_NUM = internal_style_num
        _LABEL = label
        _PRODUCT_NAME = product_name
        _OO = oo
        _OH = oh
        _PO_STARTSHIPDT = po_startshipdt
        _PRICESTATUS = pricestatus
        _FEATUREDCOLOR = featuredcolor
        _COLOR = color 'color.ToString.Split("-"c)(0).ToString
    End Sub

    Public Sub New(ByVal image_id As String, ByVal internal_style_num As Decimal, ByVal upc_num As Decimal, ByVal feature As String, ByVal color As String, ByVal size As String)
        _IMAGE_ID = If(image_id Is Nothing, "0", image_id)
        _INTERNAL_STYLE_NUM = internal_style_num
        _UPC_NUM = upc_num
        _FEATURE = feature
        _COLOR = color
        _SIZE = size
    End Sub

    Public Sub New()

    End Sub

    Public Shared Function Serialize(ByVal obj As Object) As String

        Dim stream As New MemoryStream
        Dim ds As New XmlSerializer(GetType(GXSCopyViewInfo), "http://schema.bonton.com/Schema/SampleSchema")
        ds.Serialize(stream, obj)
        Dim xmlString As String = Encoding.UTF8.GetString(stream.ToArray())
        stream.Close()
        Return xmlString

    End Function
End Class
