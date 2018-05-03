Imports System.ComponentModel

<Serializable()> _
Public Class EcommSetupClrSzInfo

    Private _turnInMerchID As Integer
    Private _adminMerchNum As Integer
    Private _sampleMerchId As Integer
    Private _removeMerchFlag As Char
    Private _status As String
    Private _deptID As Integer
    Private _deptIdDesc As String
    Private _isn As Decimal
    Private _vendorStyleNum As String
    Private _isnDesc As String
    Private _vendorName As String

    Private _isReserve As Char
    Private _isTurnedIn As Char
    Private _isHotItem As Char
    Private _friendlyProdDesc As String
    Private _friendlyProdFeatures As String
    Private _color As String
    Private _VendorColorCode As Integer
    Private _friendlyColor As String
    Private _colorFamily As String
    Private _Sample As String
    Private _sampleSize As String
    Private _colorCorrect As Char
    Private _ImageKind As String
    Private _puImageID As Integer
    Private _routeFromAD As Integer
    Private _groupNum As Short
    Private _featureRenderSwatch As String
    Private _imageType As String
    Private _altView As String
    Private _sampleStoreNum As Short
    Private _sampleStore As String
    Private _merchantNotes As String
    Private _labelName As String

    Private _upc As Decimal
    Private _turnedInAdNos As String
    Private _sequence As Integer

    Private _BatchNumber As Integer = 0
    Private _AdNumber As Integer
    Private _PageNumber As Integer

    'Default constructor
    Public Sub New()
    End Sub

    <DataObjectField(True)> _
    Public Property Sequence() As Integer
        Get
            Return _sequence
        End Get
        Set(ByVal value As Integer)
            _sequence = value
        End Set
    End Property

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
    Public Property AdminMerchNum() As Integer
        Get
            Return _adminMerchNum
        End Get
        Set(ByVal value As Integer)
            _adminMerchNum = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SampleMerchId() As Integer
        Get
            Return _sampleMerchId
        End Get
        Set(value As Integer)
            _sampleMerchId = value
        End Set
    End Property


    <DataObjectField(True)> _
    Public Property RemoveMerchFlag() As Char
        Get
            Return _removeMerchFlag
        End Get
        Set(ByVal value As Char)
            _removeMerchFlag = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
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
    Public Property ISN() As Decimal
        Get
            Return _isn
        End Get
        Set(ByVal value As Decimal)
            _isn = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property VendorStyleNum() As String
        Get
            Return _vendorStyleNum
        End Get
        Set(ByVal value As String)
            _vendorStyleNum = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property IsnDesc() As String
        Get
            Return _isnDesc
        End Get
        Set(ByVal value As String)
            _isnDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property VendorName() As String
        Get
            Return _vendorName
        End Get
        Set(ByVal value As String)
            _vendorName = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property IsReserve() As Char
        Get
            Return _isReserve
        End Get
        Set(ByVal value As Char)
            _isReserve = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property IsTurnedIn() As Char
        Get
            Return _isTurnedIn
        End Get
        Set(ByVal value As Char)
            _isTurnedIn = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property IsHotItem() As Char
        Get
            Return _isHotItem
        End Get
        Set(ByVal value As Char)
            _isHotItem = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property FriendlyProdDesc() As String
        Get
            Return _friendlyProdDesc
        End Get
        Set(ByVal value As String)
            _friendlyProdDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property FriendlyProdFeatures() As String
        Get
            Return _friendlyProdFeatures
        End Get
        Set(ByVal value As String)
            _friendlyProdFeatures = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property Color() As String
        Get
            Return _color
        End Get
        Set(ByVal value As String)
            _color = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property VendorColorCode() As Integer
        Get
            Return _VendorColorCode
        End Get
        Set(ByVal value As Integer)
            _VendorColorCode = value
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
    Public Property ColorFamily() As String
        Get
            Return _colorFamily
        End Get
        Set(ByVal value As String)
            _colorFamily = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SampleDescription() As String
        Get
            Return _Sample
        End Get
        Set(value As String)
            _Sample = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SampleSize() As String
        Get
            Return _sampleSize
        End Get
        Set(ByVal value As String)
            _sampleSize = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ColorCorrect() As Char
        Get
            Return _colorCorrect
        End Get
        Set(ByVal value As Char)
            _colorCorrect = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ImageKind() As String
        Get
            Return _ImageKind
        End Get
        Set(ByVal value As String)
            _ImageKind = value
        End Set
    End Property

    Public Property PuImageID() As Integer
        Get
            Return _puImageID
        End Get
        Set(ByVal value As Integer)
            _puImageID = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property RouteFromAD() As Integer
        Get
            Return _routeFromAD
        End Get
        Set(ByVal value As Integer)
            _routeFromAD = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property GroupNum() As Short
        Get
            Return _groupNum
        End Get
        Set(ByVal value As Short)
            _groupNum = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property FeatureRenderSwatch() As String
        Get
            Return _featureRenderSwatch
        End Get
        Set(ByVal value As String)
            _featureRenderSwatch = value
        End Set
    End Property

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
    Public Property AltView() As String
        Get
            Return _altView
        End Get
        Set(ByVal value As String)
            _altView = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SampleStoreNum() As Short
        Get
            Return _sampleStoreNum
        End Get
        Set(ByVal value As Short)
            _sampleStoreNum = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SampleStore() As String
        Get
            Return _sampleStore
        End Get
        Set(ByVal value As String)
            _sampleStore = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property MerchantNotes() As String
        Get
            Return _merchantNotes
        End Get
        Set(ByVal value As String)
            _merchantNotes = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property LabelName() As String
        Get
            Return _labelName
        End Get
        Set(ByVal value As String)
            _labelName = value
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
    Public Property TurnedInAdNos() As String
        Get
            Return _turnedInAdNos
        End Get
        Set(ByVal value As String)
            _turnedInAdNos = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property BatchNumber() As Integer
        Get
            Return _BatchNumber
        End Get
        Set(ByVal value As Integer)
            _BatchNumber = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property AdNumber() As Integer
        Get
            Return _AdNumber
        End Get
        Set(ByVal value As Integer)
            _AdNumber = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property PageNumber() As Integer
        Get
            Return _PageNumber
        End Get
        Set(ByVal value As Integer)
            _PageNumber = value
        End Set
    End Property
    <DataObjectField(True)>
    Public Property VTImageAvailable() As String

    <DataObjectField(True)>
    Public Property VTImageURL() As String

    <DataObjectField(True)>
    Public Property OnHand As Integer

    <DataObjectField(True)>
    Public Property OnOrder As Integer

    <DataObjectField(True)>
    Public Property FabricationDesc As String

    ' Automatic property for getting and setting No Merch indicator
    <DataObjectField(True)>
    Public Property IsNoMerchImage() As Boolean

    ' Automatic property for getting and setting Vendor Image indicator
    <DataObjectField(True)>
    Public Property IsVendorImage() As Boolean

    ' Automatic property for getting and setting Size 1 Description
    <DataObjectField(True)>
    Public Property Size1Desc() As String

    ' Automatic property for getting and setting Size 2 Description
    <DataObjectField(True)>
    Public Property Size2Desc() As String

    ' Automatic property for getting and setting Copy Notes
    <DataObjectField(True)>
    Public Property CopyNotes() As String

End Class
