Imports System.ComponentModel

<Serializable()> _
Public Class ECommPrioritizationInfo

    Private _turnInMerchID As Integer
    Private _deptID As Integer
    Private _deptIdDesc As String
    Private _onOrder As Integer
    Private _onHand As Integer
    Private _deliverDate As String
    Private _imageShot As Char
    Private _vtPath As String
    Private _webCatStatus As String
    Private _featureID As Integer
    Private _statusFlg As Char
    Private _swatchFlg As Char
    Private _colorFlg As Char
    Private _sizeFlg As Char
    Private _webCatgyCde As Integer
    Private _webCatgyDesc As String
    Private _webCatgyList As String
    Private _productName As String
    Private _labelID As Integer
    Private _labelDesc As String
    Private _brandID As Short
    Private _brandDesc As String
    Private _isn As Decimal
    Private _vendor As String
    Private _vendorStyleNumber As String
    Private _dropShipFlg As Char
    Private _dropShipID As Short
    Private _dropShipDesc As String
    Private _intRetInsCde As Short
    Private _intReturnInstrct As String
    Private _extRetInsCde As Short
    Private _extReturnInstrct As String
    Private _ageCde As Short
    Private _ageDesc As String
    Private _genderCde As Short
    Private _genderDesc As String
    Private _friendlyColor As String
    Private _imageID As Integer
    Private _nonSwatchClrCde As Integer
    Private _nonSwatchClrDesc As String
    Private _colorFamily As String
    Private _frs As String
    Private _adNbrAdminImgNbr As String
    Private _imageNotes As String
    Private _imageSuffix As String
    Private _upc As Decimal
    Private _vendorSize As String
    Private _sizeID As Integer
    Private _sizeDesc As String
    Private _sizeFamID As Short
    Private _sizeFamily As String
    Private _isValidFlg As Char
    Private _MerchantNotes As String
    Private _EMMNotes As String
    Private _LastModifiedDate As Date
    Private _adNbr As String
    Private _isnBrandID As Integer
    Private _imageGroup As Short

    Public Sub New()
    End Sub

    <DataObjectField(True)> _
    Public Property TurnInMerchID() As Integer
        Get
            Return _turnInMerchID
        End Get
        Set(ByVal value As Integer)
            _turnInMerchID = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property DeptID() As Integer
        Get
            Return _deptID
        End Get
        Set(ByVal value As Integer)
            _deptID = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property DeptIdDesc() As String
        Get
            Return _deptIdDesc
        End Get
        Set(ByVal value As String)
            _deptIdDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property OnOrder() As Integer
        Get
            Return _onOrder
        End Get
        Set(ByVal value As Integer)
            _onOrder = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property OnHand() As Integer
        Get
            Return _onHand
        End Get
        Set(ByVal value As Integer)
            _onHand = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property DeliverDate() As String
        Get
            Return _deliverDate
        End Get
        Set(ByVal value As String)
            _deliverDate = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ImageShot() As Char
        Get
            Return _imageShot
        End Get
        Set(ByVal value As Char)
            _imageShot = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property VtPath() As String
        Get
            Return _vtPath
        End Get
        Set(ByVal value As String)
            _vtPath = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property WebCatStatus() As String
        Get
            Return _webCatStatus
        End Get
        Set(ByVal value As String)
            _webCatStatus = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property FeatureID() As Integer
        Get
            Return _featureID
        End Get
        Set(ByVal value As Integer)
            _featureID = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property StatusFlg() As Char
        Get
            Return _statusFlg
        End Get
        Set(ByVal value As Char)
            _statusFlg = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SwatchFlg() As Char
        Get
            Return _swatchFlg
        End Get
        Set(ByVal value As Char)
            _swatchFlg = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ColorFlg() As Char
        Get
            Return _colorFlg
        End Get
        Set(ByVal value As Char)
            _colorFlg = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SizeFlg() As Char
        Get
            Return _sizeFlg
        End Get
        Set(ByVal value As Char)
            _sizeFlg = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property WebCatgyCde() As Integer
        Get
            Return _webCatgyCde
        End Get
        Set(ByVal value As Integer)
            _webCatgyCde = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property WebCatgyDesc() As String
        Get
            Return _webCatgyDesc
        End Get
        Set(ByVal value As String)
            _webCatgyDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property WebCatgyList() As String
        Get
            Return _webCatgyList
        End Get
        Set(ByVal value As String)
            _webCatgyList = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ProductName() As String
        Get
            Return _productName
        End Get
        Set(ByVal value As String)
            _productName = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property LabelID() As Integer
        Get
            Return _labelID
        End Get
        Set(ByVal value As Integer)
            _labelID = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property LabelDesc() As String
        Get
            Return _labelDesc
        End Get
        Set(ByVal value As String)
            _labelDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property BrandID() As Short
        Get
            Return _brandID
        End Get
        Set(ByVal value As Short)
            _brandID = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property BrandDesc() As String
        Get
            Return _brandDesc
        End Get
        Set(ByVal value As String)
            _brandDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ISN() As Decimal
        Get
            Return _isn
        End Get
        Set(ByVal value As Decimal)
            _isn = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property Vendor() As String
        Get
            Return _vendor
        End Get
        Set(ByVal value As String)
            _vendor = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property VendorStyleNumber() As String
        Get
            Return _vendorStyleNumber
        End Get
        Set(ByVal value As String)
            _vendorStyleNumber = value
        End Set
    End Property

   <DataObjectField(True)> _
    Public Property DropShipFlg() As Char
        Get
            Return _dropShipFlg
        End Get
        Set(ByVal value As Char)
            _dropShipFlg = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property DropShipID() As Short
        Get
            Return _dropShipID
        End Get
        Set(ByVal value As Short)
            _dropShipID = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property DropShipDesc() As String
        Get
            Return _dropShipDesc
        End Get
        Set(ByVal value As String)
            _dropShipDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property IntRetInsCde() As Short
        Get
            Return _intRetInsCde
        End Get
        Set(ByVal value As Short)
            _intRetInsCde = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property IntReturnInstrct() As String
        Get
            Return _intReturnInstrct
        End Get
        Set(ByVal value As String)
            _intReturnInstrct = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ExtRetInsCde() As Short
        Get
            Return _extRetInsCde
        End Get
        Set(ByVal value As Short)
            _extRetInsCde = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ExtReturnInstrct() As String
        Get
            Return _extReturnInstrct
        End Get
        Set(ByVal value As String)
            _extReturnInstrct = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property AgeCde() As Short
        Get
            Return _ageCde
        End Get
        Set(ByVal value As Short)
            _ageCde = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property AgeDesc() As String
        Get
            Return _ageDesc
        End Get
        Set(ByVal value As String)
            _ageDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property GenderCde() As Short
        Get
            Return _genderCde
        End Get
        Set(ByVal value As Short)
            _genderCde = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property GenderDesc() As String
        Get
            Return _genderDesc
        End Get
        Set(ByVal value As String)
            _genderDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property FriendlyColor() As String
        Get
            Return _friendlyColor
        End Get
        Set(ByVal value As String)
            _friendlyColor = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ImageID() As Integer
        Get
            Return _imageID
        End Get
        Set(ByVal value As Integer)
            _imageID = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property NonSwatchClrCde() As Integer
        Get
            Return _nonSwatchClrCde
        End Get
        Set(ByVal value As Integer)
            _nonSwatchClrCde = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property NonSwatchClrDesc() As String
        Get
            Return _nonSwatchClrDesc
        End Get
        Set(ByVal value As String)
            _nonSwatchClrDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ColorFamily() As String
        Get
            Return _colorFamily
        End Get
        Set(ByVal value As String)
            _colorFamily = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property FRS() As String
        Get
            Return _frs
        End Get
        Set(ByVal value As String)
            _frs = value
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
    Public Property ImageSuffix() As String
        Get
            Return _imageSuffix
        End Get
        Set(ByVal value As String)
            _imageSuffix = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property UPC() As Decimal
        Get
            Return _upc
        End Get
        Set(ByVal value As Decimal)
            _upc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property VendorSize() As String
        Get
            Return _vendorSize
        End Get
        Set(ByVal value As String)
            _vendorSize = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property WebCatSizeID() As Integer
        Get
            Return _sizeID
        End Get
        Set(ByVal value As Integer)
            _sizeID = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property WebCatSizeDesc() As String
        Get
            Return _sizeDesc
        End Get
        Set(ByVal value As String)
            _sizeDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SizeFamID() As Short
        Get
            Return _sizeFamID
        End Get
        Set(ByVal value As Short)
            _sizeFamID = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SizeFamily() As String
        Get
            Return _sizeFamily
        End Get
        Set(ByVal value As String)
            _sizeFamily = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property IsValidFlg() As Char
        Get
            Return _isValidFlg
        End Get
        Set(ByVal value As Char)
            _isValidFlg = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property MerchantNotes() As String
        Get
            Return _MerchantNotes
        End Get
        Set(ByVal value As String)
            _MerchantNotes = value
        End Set
    End Property


    <DataObjectField(True)> _
    Public Property EMMNotes() As String
        Get
            Return _EMMNotes
        End Get
        Set(ByVal value As String)
            _EMMNotes = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property LastModifiedDate() As Date
        Get
            Return _LastModifiedDate
        End Get
        Set(ByVal value As Date)
            _LastModifiedDate = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property AdNbr() As String
        Get
            Return _adNbr
        End Get
        Set(ByVal value As String)
            _adNbr = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ISNBrandID() As Integer
        Get
            Return _isnBrandID
        End Get
        Set(ByVal value As Integer)
            _isnBrandID = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ImageGroup() As Short
        Get
            Return _imageGroup
        End Get
        Set(ByVal value As Short)
            _imageGroup = value
        End Set
    End Property

    ' Automatic property for getting and setting web cat available quantity
    <DataObjectField(True)>
    Public Property WebCatAvailableQty() As String

    ' Automatic property for getting and setting web cat available quantity
    <DataObjectField(True)>
    Public Property PrimaryThumbnailURL() As String

    ' Automatic property for getting and setting web cat available quantity
    <DataObjectField(True)>
    Public Property PrimaryActualURL() As String

End Class
