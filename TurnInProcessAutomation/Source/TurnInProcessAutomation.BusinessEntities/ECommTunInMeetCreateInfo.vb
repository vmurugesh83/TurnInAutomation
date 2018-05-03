Imports System.ComponentModel
<Serializable()> _
Public Class ECommTurnInMeetCreateInfo
    Private _batchNum As Integer
    Private _ISN As String
    Private _ISNDesc As String
    Private _AdNumber As Integer
    Private _turnInMerchID As Integer
    Private _RemoveMerchFlag As String
    Private _SizeCategory As String
    Private _PageNumber As Integer
    Private _VendorId As Integer
    Private _BuyerId As Integer
    Private _DeptID As Integer
    Private _LabelID As Integer
    Private _Label As String
    ' Private _VendorName As String
    Private _VendorStyleNumber As String
    Private _BrandId As Integer
    Private _MerchantNotes As String
    Private _RoutefromAd As String
    Private _OnOff As String  '
    Private _pickup As String '
    Private _PickupImageID As String
    Private _SampleSize As String
    Private _smpl_altattr_desc As String
    Private _prim_med_url_txt As String
    Private _prim_thb_url_txt As String
    Private _sec_med_url_txt As String
    Private _sec_thb_url_txt As String
    Private _prim_thb_url_alt_txt As String
    Private _ColorCorrect As Char
    Private _MerchID As Integer
    Private _FriendlyProdDesc As String
    Private _CategoryCode As Integer
    Private _FriendlyColor As String
    Private _FeatureSwatch As String
    Private _FeatureID As Integer
    Private _EMMNotes As String
    Private _ImageKindCode As String
    Private _ImageKindDescription As String
    'Private _Selected As Boolean = False
    Private _LastModBy As String
    Private _HotListCDE As String
    Private _IsReserve As String
    Private _ImageDesc As String
    Private _ImageGrp As String
    Private _ImageNotes As String
    Private _ModelCategory As String
    Private _StylingNotes As String
    Private _TIImageId As String
    Private _CpyNotes As String
    Private _Fabrication As String
    Private _FeaturesBenefits As String
    Private _altView As String
    Private _VendorColor As String
    Private _ColorCode As Integer
    Private _webCatgyList As String
    Private _sequence As Integer
    Private _ColorSequence As Integer = 0
    Private _VendorStyleSequence As Integer = 0

    Private _EMMFollowUpFlag As String
    Private _CCFollowUpFlag As String
    Private _CWFollowUpFlag As String


    'Fields for SUBMIT
    Private _BuyerName As String
    Private _BuyerExt As String
    Private _FigureCode As String
    Private _ImageCategoryCode As String

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
    Public Property BatchNum() As Integer
        Get
            Return _batchNum
        End Get
        Set(ByVal value As Integer)
            _batchNum = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property IsReserve() As String
        Get
            Return _IsReserve
        End Get
        Set(ByVal value As String)
            _IsReserve = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property RemoveMerchFlag() As String
        Get
            Return _RemoveMerchFlag
        End Get
        Set(ByVal value As String)
            _RemoveMerchFlag = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property ISN() As String
        Get
            Return _ISN
        End Get
        Set(ByVal value As String)
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
    Public Property DeptID() As Integer
        Get
            Return _DeptID
        End Get
        Set(ByVal value As Integer)
            _DeptID = value
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
    Public Property LabelID() As Integer
        Get
            Return _LabelID
        End Get
        Set(ByVal value As Integer)
            _LabelID = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property Label() As String
        Get
            Return _Label
        End Get
        Set(ByVal value As String)
            _Label = value
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

    '<DataObjectField(True)> _
    'Public Property AdDesc() As String
    '    Get
    '        Return _AdDesc
    '    End Get
    '    Set(ByVal value As String)
    '        _AdDesc = value
    '    End Set
    'End Property


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
    Public Property VendorId() As Integer
        Get
            Return _VendorId
        End Get
        Set(ByVal value As Integer)
            _VendorId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property HotListCDE() As String
        Get
            Return _HotListCDE
        End Get
        Set(ByVal value As String)
            _HotListCDE = value
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
    Public Property BrandId() As Integer
        Get
            Return _BrandId
        End Get
        Set(ByVal value As Integer)
            _BrandId = value
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
    Public Property RoutefromAd() As String
        Get
            Return _RoutefromAd
        End Get
        Set(ByVal value As String)
            _RoutefromAd = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property OnOff() As String
        Get
            Return _OnOff
        End Get
        Set(ByVal value As String)
            _OnOff = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property pickup() As String
        Get
            Return _pickup
        End Get
        Set(ByVal value As String)
            _pickup = value
        End Set
    End Property


    <DataObjectField(True)> _
    Public Property PickupImageID() As String
        Get
            Return _PickupImageID
        End Get
        Set(ByVal value As String)
            _PickupImageID = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property PrimaryThumbnailUrl() As String
        Get
            Return _prim_thb_url_txt
        End Get
        Set(value As String)
            _prim_thb_url_txt = value
        End Set
    End Property
    <DataObjectField(False)> _
    Public Property PrimaryThumbnailUrlAltText() As String
        Get
            Return _prim_thb_url_alt_txt
        End Get
        Set(ByVal value As String)
            _prim_thb_url_alt_txt = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property PrimaryMediumUrl() As String
        Get
            Return _prim_med_url_txt
        End Get
        Set(value As String)
            If String.IsNullOrEmpty(value) Then
                _prim_med_url_txt = ""
            Else
                _prim_med_url_txt = value

            End If
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SecondaryMediumUrl() As String
        Get
            Return _sec_med_url_txt
        End Get
        Set(value As String)
            If String.IsNullOrEmpty(value) Then
                _sec_med_url_txt = ""
            Else
                _sec_med_url_txt = value
            End If
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SecondaryThumbnailUrl() As String
        Get
            Return _sec_thb_url_txt
        End Get
        Set(value As String)
            If String.IsNullOrEmpty(value) Then
                _sec_thb_url_txt = ""
            Else
                _sec_thb_url_txt = value
            End If
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SampleSize() As String
        Get
            Return _SampleSize
        End Get
        Set(ByVal value As String)
            _SampleSize = value
        End Set
    End Property
    '<DataObjectField(True)> _
    '   Public Property SampleAltAttrDesc() As String
    '    Get
    '        Return _smpl_altattr_desc
    '    End Get
    '    Set(ByVal value As String)
    '        _smpl_altattr_desc = value
    '    End Set
    'End Property

    <DataObjectField(True)> _
    Public Property ColorCorrect() As Char
        Get
            Return _ColorCorrect
        End Get
        Set(ByVal value As Char)
            _ColorCorrect = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property MerchID() As Integer
        Get
            Return _MerchID
        End Get
        Set(ByVal value As Integer)
            _MerchID = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property FriendlyProdDesc() As String
        Get
            Return _FriendlyProdDesc
        End Get
        Set(ByVal value As String)
            _FriendlyProdDesc = value
        End Set
    End Property

    Public Property CategoryCode() As Integer
        Get
            Return _CategoryCode
        End Get
        Set(ByVal value As Integer)
            _CategoryCode = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property FriendlyColor() As String
        Get
            Return _FriendlyColor
        End Get
        Set(ByVal value As String)
            _FriendlyColor = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property FeatureSwatch() As String
        Get
            Return _FeatureSwatch
        End Get
        Set(ByVal value As String)
            _FeatureSwatch = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property FeatureID() As Integer
        Get
            Return _FeatureID
        End Get
        Set(ByVal value As Integer)
            _FeatureID = value
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
    Public Property ImageKindCode() As String
        Get
            Return _ImageKindCode
        End Get
        Set(ByVal value As String)
            _ImageKindCode = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ImageKindDescription() As String
        Get
            Return _ImageKindDescription
        End Get
        Set(ByVal value As String)
            _ImageKindDescription = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SizeCategory() As String
        Get
            Return _SizeCategory
        End Get
        Set(ByVal value As String)
            _SizeCategory = value
        End Set
    End Property

    '<DataObjectField(True)> _
    'Public Property Selected() As Boolean
    '    Get
    '        Return _Selected
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _Selected = value
    '    End Set
    'End Property

    '<DataObjectField(True)> _
    Public Property turnInMerchID() As Integer
        Get
            Return _turnInMerchID
        End Get
        Set(ByVal value As Integer)
            _turnInMerchID = value
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

    <DataObjectField(True)> _
    Public Property ImageDesc() As String
        Get
            Return _ImageDesc
        End Get
        Set(ByVal value As String)
            _ImageDesc = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ImageGrp() As String
        Get
            Return _ImageGrp
        End Get
        Set(ByVal value As String)
            _ImageGrp = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ImageNotes() As String
        Get
            Return _ImageNotes
        End Get
        Set(ByVal value As String)
            _ImageNotes = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ModelCategory() As String
        Get
            Return _ModelCategory
        End Get
        Set(ByVal value As String)
            _ModelCategory = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property StylingNotes() As String
        Get
            Return _StylingNotes
        End Get
        Set(ByVal value As String)
            _StylingNotes = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property TIImageId() As String
        Get
            Return _TIImageId
        End Get
        Set(ByVal value As String)
            _TIImageId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property CpyNotes() As String
        Get
            Return _CpyNotes
        End Get
        Set(ByVal value As String)
            _CpyNotes = value
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
    Public Property FeaturesBenefits() As String
        Get
            Return _FeaturesBenefits
        End Get
        Set(ByVal value As String)
            _FeaturesBenefits = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property FigureCode() As String
        Get
            Return _FigureCode
        End Get
        Set(ByVal value As String)
            _FigureCode = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ImageCategoryCode() As String
        Get
            Return _ImageCategoryCode
        End Get
        Set(ByVal value As String)
            _ImageCategoryCode = value
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
    Public Property VendorColor() As String
        Get
            Return _VendorColor
        End Get
        Set(ByVal value As String)
            _VendorColor = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property ColorCode() As Integer
        Get
            Return _ColorCode
        End Get
        Set(ByVal value As Integer)
            _ColorCode = value
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

    Public Property EMMFollowUpFlag As String
        Get
            Return _EMMFollowUpFlag
        End Get
        Set(value As String)
            _EMMFollowUpFlag = value
        End Set
    End Property
    Public Property CCFollowUpFlag As String
        Get
            Return _CCFollowUpFlag
        End Get
        Set(value As String)
            _CCFollowUpFlag = value
        End Set
    End Property
    Public Property CWFollowUpFlag As String
        Get
            Return _CWFollowUpFlag
        End Get
        Set(value As String)
            _CWFollowUpFlag = value
        End Set
    End Property

    Public Property ColorSequence() As Integer
        Get
            Return _ColorSequence
        End Get
        Set(ByVal value As Integer)
            _ColorSequence = value
        End Set
    End Property

    Public Property VendorStyleSequence() As Integer
        Get
            Return _VendorStyleSequence
        End Get
        Set(ByVal value As Integer)
            _VendorStyleSequence = value
        End Set
    End Property

    Public Sub New()

    End Sub
End Class
