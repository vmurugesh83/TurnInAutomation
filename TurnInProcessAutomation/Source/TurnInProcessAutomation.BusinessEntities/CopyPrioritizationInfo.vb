Imports System.ComponentModel

<Serializable()> _
Public Class CopyPrioritizationInfo

    Private _rowId As Integer
    Private _imageID As Integer
    Private _adminImageId As Integer
    Private _deptID As Integer
    Private _deptIdDesc As String
    Private _vendorStyleNumber As String
    Private _poStartShipDate As Date
    Private _dateMaintained As Date
    Private _isn As Decimal
    Private _upc As Long
    Private _categoryCode As Integer
    Private _categoryName As String
    Private _productCode As Integer
    Private _topCat As Integer
    Private _hierarchy As String
    Private _productName As String
    Private _genderDesc As String
    Private _ageDesc As String
    Private _imageUrl As String
    Private _largeImageUrl As String
    Private _imageDetails As String
    Private _availableQty As Integer
    Private _brandID As Short
    Private _brandDesc As String
    Private _weightedInventory As Integer
    Private _productDetails As String
    Private _productDetailsMore As String
    Private _featuredColorSize As String
    Private _fabricDesc As String
    Private _origination As String
    Private _availableColors As List(Of CopyPrioritizationColorInfo)

    Public Sub New()
    End Sub

    <DataObjectField(True)> _
    Public Property RowID() As Integer
        Get
            Return _rowId
        End Get
        Set(ByVal value As Integer)
            _rowId = value
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
    Public Property AdminImageID() As Integer
        Get
            Return _adminImageId
        End Get
        Set(ByVal value As Integer)
            _adminImageId = value
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
    Public Property ProductName() As String
        Get
            Return _productName
        End Get
        Set(ByVal value As String)
            _productName = value
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
    Public Property AgeDesc() As String
        Get
            Return _ageDesc
        End Get
        Set(ByVal value As String)
            _ageDesc = value
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
    Public Property VendorStyleNumber() As String
        Get
            Return _vendorStyleNumber
        End Get
        Set(ByVal value As String)
            _vendorStyleNumber = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property UPC() As Long
        Get
            Return _upc
        End Get
        Set(ByVal value As Long)
            _upc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property CategoryCode() As Integer
        Get
            Return _categoryCode
        End Get
        Set(ByVal value As Integer)
            _categoryCode = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property CategoryName() As String
        Get
            Return _categoryName
        End Get
        Set(ByVal value As String)
            _categoryName = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property AvailableQty() As Integer
        Get
            Return _availableQty
        End Get
        Set(ByVal value As Integer)
            _availableQty = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ProductCode() As Integer
        Get
            Return _productCode
        End Get
        Set(ByVal value As Integer)
            _productCode = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property TopCat() As Integer
        Get
            Return _topCat
        End Get
        Set(ByVal value As Integer)
            _topCat = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property WeightedInventory() As Integer
        Get
            Return _weightedInventory
        End Get
        Set(ByVal value As Integer)
            _weightedInventory = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property POStartShipDate() As Date
        Get
            Return _poStartShipDate
        End Get
        Set(ByVal value As Date)
            _poStartShipDate = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property DateMaintained() As Date
        Get
            Return _dateMaintained
        End Get
        Set(ByVal value As Date)
            _dateMaintained = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ProductDetails() As String
        Get
            Return _productDetails
        End Get
        Set(ByVal value As String)
            _productDetails = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ProductDetailsMore() As String
        Get
            Return _productDetailsMore
        End Get
        Set(ByVal value As String)
            _productDetailsMore = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property Hierarchy() As String
        Get
            Return _hierarchy
        End Get
        Set(ByVal value As String)
            _hierarchy = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ImageUrl() As String
        Get
            Return _imageUrl
        End Get
        Set(ByVal value As String)
            _imageUrl = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property LargeImageUrl() As String
        Get
            Return _largeImageUrl
        End Get
        Set(ByVal value As String)
            _largeImageUrl = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ImageDetails() As String
        Get
            Return _imageDetails
        End Get
        Set(ByVal value As String)
            _imageDetails = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property FeaturedColorSize() As String
        Get
            Return _featuredColorSize
        End Get
        Set(ByVal value As String)
            _featuredColorSize = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property FabricDescription() As String
        Get
            Return _fabricDesc
        End Get
        Set(ByVal value As String)
            _fabricDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property Origination() As String
        Get
            Return _origination
        End Get
        Set(ByVal value As String)
            _origination = value
        End Set
    End Property

    Public Property AvailableColors() As List(Of CopyPrioritizationColorInfo)
        Get
            Return _availableColors
        End Get
        Set(ByVal value As List(Of CopyPrioritizationColorInfo))
            _availableColors = value
        End Set
    End Property

    ' Automatic property for getting and setting web cat available quantity
    <DataObjectField(True)>
    Public Property WebCatAvailableQty() As Integer

    <DataObjectField(True)>
    Public Property PrimaryThumbnailURL() As String

    <DataObjectField(True)>
    Public Property PrimaryActualURL() As String

    <DataObjectField(True)>
    Public Property OnOrder() As Integer

    <DataObjectField(True)>
    Public Property OnHand() As Integer

    <DataObjectField(True)>
    Public Property PriceStatusCode() As String

    <DataObjectField(True)>
    Public Property IsFinalImageReady() As Boolean

    <DataObjectField(True)>
    Public Property SKUUseCode() As String

    <DataObjectField(True)>
    Public Property ProductReadyDate() As Date

    <DataObjectField(True)>
    Public Property ThirdPartyFulfilmentCode() As Integer

    <DataObjectField(True)>
    Public Property OwnedPrice() As Decimal

    <DataObjectField(True)>
    Public Property OHMultiplier() As Integer

    <DataObjectField(True)>
    Public Property OOMultiplier() As Integer

    <DataObjectField(True)>
    Public Property ShipDateMultiplier() As Integer

    <DataObjectField(True)>
    Public Property FinalImageMultiplier() As Integer

    <DataObjectField(True)>
    Public Property PriceStatusMultiplier() As Integer

    <DataObjectField(True)>
    Public Property SKUUseMultiplier() As Integer

    <DataObjectField(True)>
        Public Property ProductDateMultiplier() As Integer

    <DataObjectField(True)>
    Public Property DirectShipMultiplier() As Integer

    <DataObjectField(True)>
    Public Property OwnedPriceMultiplier() As Integer

    <DataObjectField(True)>
    Public Property OwnedPriceOO() As Decimal

    <DataObjectField(True)>
    Public Property OwnedPriceOH() As Decimal

    <DataObjectField(True)>
    Public Property ProductNotes() As String

    <DataObjectField(True)> _
    Public Property ColorCode() As Integer
End Class

<Serializable()> _
Public Class CopyPrioritizationColorInfo

    Private _colorFamily As String
    Private _friendlyColor As String

    Public Sub New()
    End Sub

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
    Public Property FriendlyColorName() As String
        Get
            Return _friendlyColor
        End Get
        Set(ByVal value As String)
            _friendlyColor = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property FriendlySizeName() As String

    <DataObjectField(True)> _
    Public Property SizeFamily() As String

    <DataObjectField(True)> _
    Public Property SizeSequenceNumber() As Integer

    <DataObjectField(True)> _
    Public Property ColorSequenceNumber() As Integer
End Class

<Serializable()> _
Public Class CopyPrioritizationImageInfo
    <DataObjectField(True)> _
    Public Property UPC() As Long

    <DataObjectField(True)> _
    Public Property LargeImageID() As String

    <DataObjectField(True)> _
    Public Property ISN() As Decimal

    <DataObjectField(True)> _
    Public Property ColorCode() As Integer

End Class
