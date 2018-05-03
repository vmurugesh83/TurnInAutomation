Imports System.ComponentModel

Public Class ImageInfo
    'Internal member variables
    Private _imageId As Integer
    Private _turnInMerchId As Integer
    Private _adNbrAdminImgNbr As String
    Private _imageNotes As String
    Private _vendorStyle As String
    'Default constructor
    Public Sub New()
    End Sub


    <DataObjectField(True)> _
    Public Property ImageId() As Integer
        Get
            Return _imageId
        End Get
        Set(ByVal value As Integer)
            _imageId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property TurnInMerchId() As Integer
        Get
            Return _turnInMerchId
        End Get
        Set(ByVal value As Integer)
            _turnInMerchId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property AdNbrAdminImgNbr() As String
        Get
            Return _adNbrAdminImgNbr
        End Get
        Set(ByVal value As String)
            _adNbrAdminImgNbr = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ImageNotes() As String
        Get
            Return _imageNotes
        End Get
        Set(ByVal value As String)
            _imageNotes = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property VendorStyle() As String
        Get
            Return _vendorStyle
        End Get
        Set(ByVal value As String)
            _vendorStyle = value
        End Set
    End Property

    Private _imageType As String
    <DataObjectField(True)> _
    Public Property ImageType() As String
        Get
            Return _imageType
        End Get
        Set(ByVal value As String)
            _imageType = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ImageGroupNumber() As Integer

    <DataObjectField(True)> _
    Public Property ImageCategoryCode() As String
End Class
