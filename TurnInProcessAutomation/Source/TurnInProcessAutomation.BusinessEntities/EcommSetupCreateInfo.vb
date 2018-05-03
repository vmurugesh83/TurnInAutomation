
Imports System.ComponentModel

<Serializable()> _
Partial Public Class EcommSetupCreateInfo

    'Internal member variables
    Private _BatchNumber As Integer = 0
    Private _AdNumber As Integer
    Private _PageNumber As Integer
    Private _ACode As String
    Private _VendorId As Integer
    Private _VendorIds As String
    Private _VendorName As String
    Private _IsReserve As Boolean
    Private _ISN As Decimal
    Private _ISNDesc As String
    Private _VendorStyleNumber As String
    Private _SellYear As Integer
    Private _SellSeason As String
    Private _IsTurnedInPrint As Char = "N"c
    Private _IsTurnedInEcomm As Char = "N"c
    Private _OnOrder As Integer
    Private _OnHand As Integer
    Private _ColorCode As String
    Private _ColorDesc As String
    Private _SampleAvailable As Char = "N"c
    Private _SampleApproved As Char = "N"c

    Private _TurnedInEcommAdNos As String = ""
    Private _TurnedInPrintAdNos As String = ""

    Private _Selected As Boolean = False
    Private _Saved As Boolean = False
    Private _Sequence As Integer = 0

    Private _DeptId As Integer
    Private _DeptDesc As String
    Private _BuyerId As Integer
    Private _BuyerName As String
    Private _BuyerExt As String
    Private _BrandId As Integer
    Private _BrandDesc As String
    Private _LabelId As Integer
    Private _LabelDesc As String
    Private _ProductDetailId1 As Integer
    Private _ProductDetailDesc1 As String
    Private _ProductDetailId2 As Integer
    Private _ProductDetailDesc2 As String
    Private _ProductDetailId3 As Integer
    Private _ProductDetailDesc3 As String
    Private _PatternDesc As String
    Private _ExistingWebStyle As String
    Private _FeatureImageNum As String
    Private _SellingLocation As String
    Private _Fabrication As String
    Private _ImportedOrUSA As String
    Private _DropShipFlg As Char
    Private _SizeAvailable As String
    Private _SizeCategoryCode As String
    Private _smpl_altattr_desc As String
    Private _ModelCategoryCode As String
    Private _VndApprovalFlg As Char
    Private _AddnColorSamplesFlg As Char
    Private _TurnInUsageInd As Short

    Private _TurnInMerchId As Integer
    Private _LastModBy As String

    Private _AlreadyExistsInBatch As Boolean = False
    Private _AlreadyProcesssed As Boolean = False

    'Default constructor
    Public Sub New()
    End Sub

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

    <DataObjectField(True)> _
    Public Property ACode() As String
        Get
            Return _ACode
        End Get
        Set(ByVal value As String)
            _ACode = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property VendorId() As Integer
        Get
            Return _VendorId
        End Get
        Set(ByVal value As Integer)
            _VendorId = value
        End Set
    End Property
    '<DataObjectField(True)> _
    Public Property VendorIds() As String
        Get
            Return _VendorIds
        End Get
        Set(ByVal value As String)
            _VendorIds = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property VendorName() As String
        Get
            Return _VendorName
        End Get
        Set(ByVal value As String)
            _VendorName = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property IsReserve() As Boolean
        Get
            Return _IsReserve
        End Get
        Set(ByVal value As Boolean)
            _IsReserve = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ISN() As Decimal
        Get
            Return _ISN
        End Get
        Set(ByVal value As Decimal)
            _ISN = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ISNDesc() As String
        Get
            Return _ISNDesc
        End Get
        Set(ByVal value As String)
            _ISNDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property VendorStyleNumber() As String
        Get
            Return _VendorStyleNumber
        End Get
        Set(ByVal value As String)
            _VendorStyleNumber = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SellYear() As Integer
        Get
            Return _SellYear
        End Get
        Set(ByVal value As Integer)
            _SellYear = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SellSeason() As String
        Get
            Return _SellSeason
        End Get
        Set(ByVal value As String)
            _SellSeason = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property IsTurnedInPrint() As Char
        Get
            Return _IsTurnedInPrint
        End Get
        Set(ByVal value As Char)
            _IsTurnedInPrint = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property IsTurnedInEcomm() As Char
        Get
            Return _IsTurnedInEcomm
        End Get
        Set(ByVal value As Char)
            _IsTurnedInEcomm = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property OnOrder() As Integer
        Get
            Return _OnOrder
        End Get
        Set(ByVal value As Integer)
            _OnOrder = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property OnHand() As Integer
        Get
            Return _OnHand
        End Get
        Set(ByVal value As Integer)
            _OnHand = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ColorCode() As String
        Get
            Return _ColorCode
        End Get
        Set(ByVal value As String)
            _ColorCode = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ColorDesc() As String
        Get
            Return _ColorDesc
        End Get
        Set(ByVal value As String)
            _ColorDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SampleAvailable() As Char
        Get
            Return _SampleAvailable
        End Get
        Set(value As Char)
            _SampleAvailable = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SampleApproved() As Char
        Get
            Return _SampleApproved
        End Get
        Set(value As Char)
            _SampleApproved = value
        End Set
    End Property
    Public Property TurnedInEcommAdNos() As String
        Get
            Return _TurnedInEcommAdNos
        End Get
        Set(ByVal value As String)
            _TurnedInEcommAdNos = value
        End Set
    End Property


    Public Property TurnedInPrintAdNos() As String
        Get
            Return _TurnedInPrintAdNos
        End Get
        Set(ByVal value As String)
            _TurnedInPrintAdNos = value
        End Set
    End Property

    Public Property Selected() As Boolean
        Get
            Return _Selected
        End Get
        Set(ByVal value As Boolean)
            _Selected = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property Sequence() As Integer
        Get
            Return _Sequence
        End Get
        Set(ByVal value As Integer)
            _Sequence = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property BuyerId() As Integer
        Get
            Return _BuyerId
        End Get
        Set(ByVal value As Integer)
            _BuyerId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property DeptId() As Integer
        Get
            Return _DeptId
        End Get
        Set(ByVal value As Integer)
            _DeptId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property DeptDesc() As String
        Get
            Return _DeptDesc
        End Get
        Set(ByVal value As String)
            _DeptDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property BuyerName() As String
        Get
            Return _BuyerName
        End Get
        Set(ByVal value As String)
            _BuyerName = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property BuyerExt() As String
        Get
            Return _BuyerExt
        End Get
        Set(ByVal value As String)
            _BuyerExt = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property BrandId() As Integer
        Get
            Return _BrandId
        End Get
        Set(ByVal value As Integer)
            _BrandId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property BrandDesc() As String
        Get
            Return _BrandDesc
        End Get
        Set(ByVal value As String)
            _BrandDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property LabelId() As Integer
        Get
            Return _LabelId
        End Get
        Set(ByVal value As Integer)
            _LabelId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property LabelDesc() As String
        Get
            Return _LabelDesc
        End Get
        Set(ByVal value As String)
            _LabelDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ProductDetailId1() As Integer
        Get
            Return _ProductDetailId1
        End Get
        Set(ByVal value As Integer)
            _ProductDetailId1 = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ProductDetailDesc1() As String
        Get
            Return _ProductDetailDesc1
        End Get
        Set(ByVal value As String)
            _ProductDetailDesc1 = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ProductDetailId2() As Integer
        Get
            Return _ProductDetailId2
        End Get
        Set(ByVal value As Integer)
            _ProductDetailId2 = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ProductDetailDesc2() As String
        Get
            Return _ProductDetailDesc2
        End Get
        Set(ByVal value As String)
            _ProductDetailDesc2 = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ProductDetailId3() As Integer
        Get
            Return _ProductDetailId3
        End Get
        Set(ByVal value As Integer)
            _ProductDetailId3 = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ProductDetailDesc3() As String
        Get
            Return _ProductDetailDesc3
        End Get
        Set(ByVal value As String)
            _ProductDetailDesc3 = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property PatternDesc() As String
        Get
            Return _PatternDesc
        End Get
        Set(ByVal value As String)
            _PatternDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SizeAvailable() As String
        Get
            Return _SizeAvailable
        End Get
        Set(ByVal value As String)
            _SizeAvailable = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ExistingWebStyle() As String
        Get
            Return _ExistingWebStyle
        End Get
        Set(ByVal value As String)
            _ExistingWebStyle = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property FeatureImageNum() As String
        Get
            Return _FeatureImageNum
        End Get
        Set(ByVal value As String)
            _FeatureImageNum = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SellingLocation() As String
        Get
            Return _SellingLocation
        End Get
        Set(ByVal value As String)
            _SellingLocation = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property Fabrication() As String
        Get
            Return _Fabrication
        End Get
        Set(ByVal value As String)
            _Fabrication = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ImportedOrUSA() As String
        Get
            Return _ImportedOrUSA
        End Get
        Set(ByVal value As String)
            _ImportedOrUSA = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property DropShipFlg() As Char
        Get
            Return _DropShipFlg
        End Get
        Set(ByVal value As Char)
            _DropShipFlg = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SizeCategoryCode() As String
        Get
            Return _SizeCategoryCode
        End Get
        Set(ByVal value As String)
            _SizeCategoryCode = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ModelCategoryCode() As String
        Get
            Return _ModelCategoryCode
        End Get
        Set(ByVal value As String)
            _ModelCategoryCode = value
        End Set
    End Property
    '<DataObjectField(False)> _
    'Public Property SampleAltAttrDesc() As String
    '    Get
    '        Return _smpl_altattr_desc
    '    End Get
    '    Set(ByVal value As String)
    '        _smpl_altattr_desc = value
    '    End Set
    'End Property

    <DataObjectField(True)> _
    Public Property VndApprovalFlg() As Char
        Get
            Return _VndApprovalFlg
        End Get
        Set(ByVal value As Char)
            _VndApprovalFlg = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property AddnColorSamplesFlg() As Char
        Get
            Return _AddnColorSamplesFlg
        End Get
        Set(ByVal value As Char)
            _AddnColorSamplesFlg = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property TurnInUsageInd() As Short
        Get
            Return _TurnInUsageInd
        End Get
        Set(ByVal value As Short)
            _TurnInUsageInd = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property TurnInMerchId() As Integer
        Get
            Return _TurnInMerchId
        End Get
        Set(ByVal value As Integer)
            _TurnInMerchId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property LastModBy() As String
        Get
            Return _LastModBy
        End Get
        Set(ByVal value As String)
            _LastModBy = value
        End Set
    End Property

    Public Property Saved() As Boolean
        Get
            Return _Saved
        End Get
        Set(ByVal value As Boolean)
            _Saved = value
        End Set
    End Property

    Public Property AlreadyExistsInBatch() As Boolean
        Get
            Return _AlreadyExistsInBatch
        End Get
        Set(ByVal value As Boolean)
            _AlreadyExistsInBatch = value
        End Set
    End Property

    Public Property AlreadyProcessed() As Boolean
        Get
            Return _AlreadyProcesssed
        End Get
        Set(ByVal value As Boolean)
            _AlreadyProcesssed = value
        End Set
    End Property

    Private _GenericClassId As Integer
    <DataObjectField(True)> _
    Public Property GenericClassId() As Integer
        Get
            Return _GenericClassId
        End Get
        Set(ByVal value As Integer)
            _GenericClassId = value
        End Set
    End Property

    Private _GenericClass As String
    <DataObjectField(True)> _
    Public Property GenericClass() As String
        Get
            Return _GenericClass
        End Get
        Set(ByVal value As String)
            _GenericClass = value
        End Set
    End Property

    Private _GenericSubClassId As Integer
    <DataObjectField(True)> _
    Public Property GenericSubClassId() As Integer
        Get
            Return _GenericSubClassId
        End Get
        Set(ByVal value As Integer)
            _GenericSubClassId = value
        End Set
    End Property

    Private _GenericSubClass As String
    <DataObjectField(True)> _
    Public Property GenericSubClass() As String
        Get
            Return _GenericSubClass
        End Get
        Set(ByVal value As String)
            _GenericSubClass = value
        End Set
    End Property
    ' Automatic property for getting and setting sample details
    Public Property SampleDetails() As New SampleRequestInfo()

    ' Automatic property for getting and setting sample available for turn in value
    <DataObjectField(True)>
    Public Property AvailableForTurnIn() As String

    ' Automatic property for getting and setting active upc flag of 200SKU
    Public Property ActiveUPCFlag() As String

    ' Automatic property for getting and setting active upc flag of TEC150
    Public Property ActiveFlag() As String

    ' Automatic property for getting and setting active on web value
    <DataObjectField(True)>
    Public Property ActiveOnWeb() As String

    ' Automatic property for getting and setting purchase order start ship date
    <DataObjectField(True)>
    Public Property StartShipDate() As String

    ' Automatic property for getting and setting web cat available quantity
    <DataObjectField(True)>
    Public Property WebCatAvailableQty() As String

    ' Automatic property for getting and setting on order quantity by ship date
    <DataObjectField(True)>
    Public Property OnOrderByShipDate() As String

    ' Automatic property for getting and setting ISN sub class id
    <DataObjectField(True)>
    Public Property SubClassID() As Integer

    ' Automatic property for getting and setting ISN class id
    <DataObjectField(True)>
    Public Property ClassID() As Integer

    ' Automatic property for getting and setting Vendor Image indicator
    <DataObjectField(True)>
    Public Property IsVendorImage() As Boolean

    ' Automatic property for getting and setting No Merch indicator
    <DataObjectField(True)>
    Public Property IsNoMerchImage() As Boolean

End Class

