Imports System.ComponentModel
<Serializable()> _
Public Class ECommTurnInQueryToolInfo
    Private _batchNum As Integer
    Private _TurnInMerchID As Integer
    Private _Department As String
    Private _TIStatus As String
    Private _ReserveFlag As String
    Private _ISN As String
    Private _Turn_in_Indicator As String
    Private _MerchID As String
    Private _Image_ID As String
    Private _VT_Path As String
    Private _Buyer As String
    Private _Vendor As String
    Private _Vendor_Style As String
    Private _Style_Desc As String
    Private _Friendly_Product_Description As String
    Private _Color As String
    Private _Friendly_color As String
    Private _Ship_Date As String
    Private _Hot_Rushed As String
    Private _Turn_in_Date As Date
    Private _AD_NUM As String
    Private _Page_Num As Integer
    Private _OO As String
    Private _OH As String
    Private _Feature_Web_Cat As String
    Private _Feature_Image_ID As String
    Private _OnOff_Figure As String
    Private _Feature_Render_Swatch As String
    Private _Model_Category As String
    Private _SampleSize As String
    Private _ImageSuffix As String
    Private _AltView As String
    Private _ClrCorrectFlg As Char
    Private _EMMNotes As String
    Private _imageNotes As String
    Private _adNbrAdminImgNbr As String
    Private _InWebCat As String
    Private _MerchantNotes As String
    Private _RemoveMerchFlg As String
    Private _routeFromAD As Integer

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
    Public Property TurnInMerchID() As Integer
        Get
            Return _TurnInMerchID
        End Get
        Set(ByVal value As Integer)
            _TurnInMerchID = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property Department() As String
        Get
            Return _Department
        End Get
        Set(ByVal value As String)
            _Department = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property Image_ID() As String
        Get
            Return _Image_ID
        End Get
        Set(ByVal value As String)
            _Image_ID = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property MerchID() As String
        Get
            Return _MerchID
        End Get
        Set(ByVal value As String)
            _MerchID = value
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
    Public Property Turn_in_Indicator() As String
        Get
            Return _Turn_in_Indicator
        End Get
        Set(ByVal value As String)
            _Turn_in_Indicator = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property Model_Category() As String
        Get
            Return _Model_Category
        End Get
        Set(ByVal value As String)
            _Model_Category = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property Feature_Render_Swatch() As String
        Get
            Return _Feature_Render_Swatch
        End Get
        Set(ByVal value As String)
            _Feature_Render_Swatch = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property TIStatus() As String
        Get
            Return _TIStatus
        End Get
        Set(ByVal value As String)
            _TIStatus = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property ReserveFlag() As String
        Get
            Return _ReserveFlag
        End Get
        Set(ByVal value As String)
            _ReserveFlag = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property VT_Path() As String
        Get
            Return _VT_Path
        End Get
        Set(ByVal value As String)
            _VT_Path = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property Buyer() As String
        Get
            Return _Buyer
        End Get
        Set(ByVal value As String)
            _Buyer = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property Vendor() As String
        Get
            Return _Vendor
        End Get
        Set(ByVal value As String)
            _Vendor = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property Vendor_Style() As String
        Get
            Return _Vendor_Style
        End Get
        Set(ByVal value As String)
            _Vendor_Style = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property Style_Desc() As String
        Get
            Return _Style_Desc
        End Get
        Set(ByVal value As String)
            _Style_Desc = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property Friendly_Product_Description() As String
        Get
            Return _Friendly_Product_Description
        End Get
        Set(ByVal value As String)
            _Friendly_Product_Description = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property Color() As String
        Get
            Return _Color
        End Get
        Set(ByVal value As String)
            _Color = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property Friendly_color() As String
        Get
            Return _Friendly_color
        End Get
        Set(ByVal value As String)
            _Friendly_color = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property Ship_Date() As String
        Get
            Return _Ship_Date
        End Get
        Set(ByVal value As String)
            _Ship_Date = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property Hot_Rushed() As String
        Get
            Return _Hot_Rushed
        End Get
        Set(ByVal value As String)
            _Hot_Rushed = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property Turn_in_Date() As Date
        Get
            Return _Turn_in_Date
        End Get
        Set(ByVal value As Date)
            _Turn_in_Date = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property AD_NUM() As String
        Get
            Return _AD_NUM
        End Get
        Set(ByVal value As String)
            _AD_NUM = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property PAGE_NUM() As Integer
        Get
            Return _Page_Num
        End Get
        Set(ByVal value As Integer)
            _Page_Num = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property OO() As String
        Get
            Return _OO
        End Get
        Set(ByVal value As String)
            _OO = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property OH() As String
        Get
            Return _OH
        End Get
        Set(ByVal value As String)
            _OH = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property Feature_Web_Cat() As String
        Get
            Return _Feature_Web_Cat
        End Get
        Set(ByVal value As String)
            _Feature_Web_Cat = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property Feature_Image_ID() As String
        Get
            Return _Feature_Image_ID
        End Get
        Set(ByVal value As String)
            _Feature_Image_ID = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property OnOff_Figure() As String
        Get
            Return _OnOff_Figure
        End Get
        Set(ByVal value As String)
            _OnOff_Figure = value
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
    <DataObjectField(True)> _
    Public Property ImageSuffix() As String
        Get
            Return _ImageSuffix
        End Get
        Set(ByVal value As String)
            _ImageSuffix = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property AltView() As String
        Get
            Return _AltView
        End Get
        Set(ByVal value As String)
            _AltView = value
        End Set
    End Property
    <DataObjectField(True)> _
    Public Property ClrCorrectFlg() As Char
        Get
            Return _ClrCorrectFlg
        End Get
        Set(ByVal value As Char)
            _ClrCorrectFlg = value
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
    Public Property ImageNotes() As String
        Get
            Return _imageNotes
        End Get
        Set(ByVal value As String)
            _imageNotes = value
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
    Public Property InWebCat() As String
        Get
            Return _InWebCat
        End Get
        Set(value As String)
            _InWebCat = value
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
    Public Property RemoveMerchFlg() As String
        Get
            Return _RemoveMerchFlg
        End Get
        Set(ByVal value As String)
            _RemoveMerchFlg = value
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

End Class
