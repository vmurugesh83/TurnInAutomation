Imports System.ComponentModel

<Serializable(), DataObject()>
Public Class SampleRequestInfo

    Private _sample_merch_id As Integer
    Private _internal_style_num As Decimal
    Private _vendor_style_num As String
    Private _isn_long_desc As String
    Private _clr_cde As Integer
    Private _clr_long_desc As String
    Private _ad_num As Integer
    Private _ad_system_page_num As Integer
    Private _size_id As String
    Private _upc_num As Decimal
    Private _smpl_altattr_desc As String
    Private _sample_due_date As Date
    Private _sample_apvl_flg As Char
    Private _sample_apvl_typ As String
    Private _sample_request_typ As String
    Private _sample_size_desc As String
    Private _sample_status_desc As String
    Private _smpl_prim_loc_nme As String
    Private _prim_actl_url_txt As String
    Private _prim_med_url_txt As String
    Private _prim_thb_url_txt As String
    Private _sec_actl_url_txt As String
    Private _sec_med_url_txt As String
    Private _sec_thb_url_txt As String
    Private _smpl_req_crte_nme As String
    Private _smpl_req_crte_ts As Date
    Private _last_mod_id As String
    Private _last_mod_ts As Date

    Public Sub New()
    End Sub

    <DataObjectField(True, False, False)> _
    Public Property SampleMerchId() As Integer
        Get
            Return _sample_merch_id
        End Get
        Set(ByVal value As Integer)
            _sample_merch_id = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property InternalStyleNum() As Decimal
        Get
            Return _internal_style_num
        End Get
        Set(ByVal value As Decimal)
            _internal_style_num = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property VendorStyleNumber As String
        Get
            Return _vendor_style_num
        End Get
        Set(value As String)
            _vendor_style_num = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property IsnLongDesc() As String
        Get
            Return _isn_long_desc
        End Get
        Set(ByVal value As String)
            _isn_long_desc = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property ColorCode() As Integer
        Get
            Return _clr_cde
        End Get
        Set(ByVal value As Integer)
            _clr_cde = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property ColorLongDesc() As String
        Get
            Return _clr_long_desc
        End Get
        Set(ByVal value As String)
            _clr_long_desc = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property AdNumber() As Integer
        Get
            Return _ad_num
        End Get
        Set(ByVal value As Integer)
            _ad_num = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property AdSystemPageNum() As Integer
        Get
            Return _ad_system_page_num
        End Get
        Set(ByVal value As Integer)
            _ad_system_page_num = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property SampleSize() As String
        Get
            Return _size_id
        End Get
        Set(ByVal value As String)
            _size_id = value
        End Set
    End Property
    <DataObjectField(False)> _
     Public Property SampleAltAttrDesc() As String
        Get
            Return _smpl_altattr_desc
        End Get
        Set(ByVal value As String)
            _smpl_altattr_desc = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property UpcNumber() As Decimal
        Get
            Return _upc_num
        End Get
        Set(ByVal value As Decimal)
            _upc_num = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property SampleDueDate As Date
        Get
            Return _sample_due_date
        End Get
        Set(value As Date)
            _sample_due_date = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property SampleApprovalFlag As Char
        Get
            Return _sample_apvl_flg
        End Get
        Set(value As Char)
            _sample_apvl_flg = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property SampleRequestType() As String
        Get
            Return _sample_request_typ
        End Get
        Set(ByVal value As String)
            _sample_request_typ = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property SampleSizeDesc() As String
        Get
            Return _sample_size_desc
        End Get
        Set(ByVal value As String)
            _sample_size_desc = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property SampleStatusDesc() As String
        Get
            Return _sample_status_desc
        End Get
        Set(ByVal value As String)
            _sample_status_desc = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property SampleApprovalType() As String
        Get
            Return _sample_apvl_typ
        End Get
        Set(ByVal value As String)
            _sample_apvl_typ = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property PrimaryLocationName() As String
        Get
            Return _smpl_prim_loc_nme
        End Get
        Set(ByVal value As String)
            _smpl_prim_loc_nme = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property PrimaryActualUrl() As String
        Get
            Return _prim_actl_url_txt
        End Get
        Set(ByVal value As String)
            _prim_actl_url_txt = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property PrimaryMediumUrl() As String
        Get
            Return _prim_med_url_txt
        End Get
        Set(ByVal value As String)
            _prim_med_url_txt = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property PrimaryThumbnailUrl() As String
        Get
            Return _prim_thb_url_txt
        End Get
        Set(ByVal value As String)
            _prim_thb_url_txt = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property SecondaryActualUrl() As String
        Get
            Return _sec_actl_url_txt
        End Get
        Set(ByVal value As String)
            _sec_actl_url_txt = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property SecondaryMediumUrl() As String
        Get
            Return _sec_med_url_txt
        End Get
        Set(ByVal value As String)
            _sec_med_url_txt = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property SecondaryThumbnailUrl() As String
        Get
            Return _sec_thb_url_txt
        End Get
        Set(ByVal value As String)
            _sec_thb_url_txt = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property SampleRequestCreateName() As String
        Get
            Return _smpl_req_crte_nme
        End Get
        Set(ByVal value As String)
            _smpl_req_crte_nme = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property SampleRequestCreateTimestamp() As Date
        Get
            Return _smpl_req_crte_ts
        End Get
        Set(ByVal value As Date)
            _smpl_req_crte_ts = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property LastModifiedId() As String
        Get
            Return _last_mod_id
        End Get
        Set(ByVal value As String)
            _last_mod_id = value
        End Set
    End Property

    <DataObjectField(False)> _
    Public Property LastModifiedTimestamp() As Date
        Get
            Return _last_mod_ts
        End Get
        Set(ByVal value As Date)
            _last_mod_ts = value
        End Set
    End Property

    Public Property CMRCheckInDate() As String
    Public Property SampleApprovalTimestamp() As String
End Class
