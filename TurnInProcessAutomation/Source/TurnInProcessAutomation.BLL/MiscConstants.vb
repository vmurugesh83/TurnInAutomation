Imports System.Configuration
Public Class MiscConstants

#Region "Page Constants"

    Public Const INVOICEDETAIL_ADD As String = "Invoice - Add"
    Public Const INVOICEDETAIL_MAINTENANCE As String = "Invoice - Maintenance"
    Public Const INVOICEDETAIL_INQUIRE As String = "Invoice - Inquire"
    Public Const INVOICEDETAIL_SPECIAL As String = "Invoice - Special"

    Public Const INVOICELIST_MAINTENANCE As String = "Invoice List -  Maintenance"
    Public Const INVOICELIST_SPECIAL As String = "Invoice List -  Special"
    Public Const INVOICELIST_INQUIRE As String = "Invoice List -  Inquire"

    Public Const MATCHIMPORTINVOICELEVEL_PAGE As String = "Match Import Invoice Level"
    Public Const MATCHPOSTORELEVEL_PAGE As String = "Match PO/Store Level"
    Public Const MATCHSTOREINVOICELEVEL_PAGE As String = "Match Store/Invoice Level"
    Public Const MATCHSTORESTYLELEVEL_PAGE As String = "Match Store/Style Level"
    Public Const MATCHWORKLIST_PAGE As String = "Match Work List"
    Public Const MATCHWORKLISTBYVENDOR_PAGE As String = "Match Work List By Vendor"

    Public Const InvoiceUpload_Add As String = "INV_ADD"
    Public Const InvoiceUpload_Update As String = "INV_UPD"
    Public Const InvoiceUpload_Cancel As String = "INV_CAN"
    Public Const InvoiceUpload_DueDate As String = "DUE_UPD"
    Public Const InvoiceUpload_Comments As String = "COM_ADD"

    'PO Module page contstants
    Public Const ADD_MODE As String = "ADD"
    Public Const MAINTENANCE_MODE As String = "MAINTENANCE"
    Public Const INQUIRE_MODE As String = "INQUIRE"
    Public Const VENDORGROUP As String = "0005"
    Public Const TMS900POSTATUSCODE As String = "PO_STATUS_CDE"
    Public Const TMS900WALKERCODE As String = "WALKER_PAYTERMS"
    Public Const TBTTABLESTPO As String = "ST-PO"
    Public Const TAP111UIPAYMENTTYPE As String = "PAYMENTTYPE"
    Public Const TAP111UIMATCHTYPE As String = "MATCHTYPE"
    Public Const TAP111UIPAYMENTALLOWED As String = "APPAYMENTTYPEALLOWED"

    Public Const PREV_PAGE_URL As String = "PreviousPageUrl"

    ' Session Variable for Un-Match Module
    Public Const STATUS_UNMATCH As String = "EXTRDY" + "," + "SUSP"

    ' String formats.
    Public Const DateStringFormat As String = "MM/dd/yyyy"

    'RTS - Detail page
    Public Const VA_Default_Store As String = "599"

#End Region


#Region "Session Variables"

    Public Const SES_RWA_LIST_PAGE As String = "RWA_LIST_PAGE"
    Public Const SES_RECEIPT_LIST_PAGE As String = "RECEIPT_LIST_PAGE"
    Public Const SES_RECEIPT_DETAIL_PAGE As String = "RECEIPT_DETAIL_PAGE"
    Public Const SES_RWA_PAGE As String = "RWA_PAGE"
    Public Const SES_AOCCODE_LIST_PAGE As String = "AOCCODE_LIST_PAGE"
    Public Const SES_MATCHIMPORTINVOICELEVEL_PAGE As String = "MATCHIMPORTINVOICELEVEL_PAGE"
    Public Const SES_MATCHIMPORTINVOICELEVEL_NOUNMATCHEDRECEIPTS_PAGE As String = "MATCHIMPORTINVOICELEVEL_NOUNMATCHEDRECEIPTS_PAGE"
    Public Const SES_MATCHPOSTORELEVEL_PAGE As String = "MATCHPOSTORELEVEL_PAGE"
    Public Const SES_MATCHWORKLIST_PAGE As String = "MATCHWORKLIST_PAGE"
    Public Const SES_MATCHSTOREINVOICELEVEL_PAGE As String = "MATCHSTOREINVOICELEVEL_PAGE"
    Public Const SES_MATCHSTORESTYLELEVEL_PAGE As String = "MATCHSTORESTYLELEVEL_PAGE"
    Public Const SES_AOCCODE_DETAIL_PAGE As String = "SES_AOCCODE_DETAIL_PAGE"
    Public Const SES_RWI_PAGE As String = "RWI_PAGE"
    Public Const SES_MATCHMINISET_PAGE As String = "MATCHMINISET_PAGE"
    Public Const SES_INVOICEDETAIL_PAGE As String = "INVOICEDETAIL_PAGE"
    Public Const SES_INVOICELIST_PAGE As String = "INVOICELIST_PAGE"
    Public Const SES_UNMATCH_PAGE As String = "UNMATCH_PAGE"
    Public Const SES_GUID As String = "GUID"

    ' Session Variable for PO Module
    Public Const SES_POID As String = "POID"
    Public Const SES_VENDOR As String = "Vendor"
    Public Const SES_DEPT As String = "Dept"
    Public Const SES_STATUS As String = "Status"
    Public Const SES_POTERMSCODE As String = "POTermsCode"
    Public Const SES_APTERMCODE As String = "APTermCode"
    Public Const SES_POPAYMENTTYPE As String = "POPaymentType"
    Public Const SES_APPAYMENTTYPE As String = "APPaymentType"
    Public Const SES_APMATCHTYPE As String = "APMatchType"
    Public Const SES_STARTDATE As String = "StartDate"
    Public Const SES_ENDDATE As String = "EndDate"
    Public Const SES_BLANKAPPAYMTCODE As String = "BlankAPPaymtCode"
    Public Const SES_ARCHIVEOPTION As String = "ArchiveOption"

    Public Const SES_PAGEACTION As String = "Action"
    Public Const SES_POLISTDATASET As String = "POListDataset"

    ' Session variables for Matcher vendor Assignment
    Public Const SES_USERID As String = "UserId"
    Public Const SES_MATCHTYPE As String = "MatchType"
    Public Const SES_RANGEFROM As String = "RangeFrom"
    Public Const SES_RANGETO As String = "RangeTo"

    ' Session Variable for Un-Match Module
    Public Const SES_UNMATCHPOID As String = "UnMatchPOID"
    Public Const SES_UNMATCHSETID As String = "UnMatchSetId"
    Public Const SES_UNMATCHCTRLPOID As String = "UnMatchControlPOID"
    Public Const SES_UNMATCHCTRLSETID As String = "UnMatchControlSetId"
    Public Const SES_UNMATCHMESSAGE As String = "UnMatchMessage"


    'Session variables for Potential Match pages
    '(Please note the Bon-Ton proposed standard (section 3.4 Constants) suggests the use of Pascal case.)
    Public Const SessionKeyPMatchSummary As String = "MAPS_PMatchSummary_Input"
    Public Const SessionKeyPMPOList As String = "MAPS_PMPOList_SelectedPoId"
#End Region

#Region "PAGE URL"
    Public Shared PAGE_URL_SIDE_BAR_TREE_VIEW As String = ConfigurationManager.AppSettings("SideBarTreeViewUrl").ToString()
    Public Shared PAGE_URL_GENERIC_CODE_LIST_SEARCH_CONTROL As String = ConfigurationManager.AppSettings("GenericCodeListSearchControlUrl").ToString()
    Public Shared PAGE_URL_RECEIPT_SEARCH_CONTROL As String = ConfigurationManager.AppSettings("ReceiptSearchControlUrl").ToString()
    Public Shared PAGE_URL_RECEIPT_DETAILS As String = ConfigurationManager.AppSettings("ReceiptDetailUrl").ToString()
    Public Shared PO_LIST_SEARCH_CONTROL_URL As String = ConfigurationManager.AppSettings("POListSearchControlUrl").ToString()
    Public Shared PO_AUDIT_SEARCH_CONTROL_URL As String = ConfigurationManager.AppSettings("POAuditSearchControlUrl").ToString()
    Public Shared PAGE_URL_PO_HEADER As String = ConfigurationManager.AppSettings("POHeaderUrl").ToString()
    Public Shared PAGE_URL_RWA_SEARCH_CONTROL As String = ConfigurationManager.AppSettings("RWASearchControlUrl").ToString()
    Public Shared PAGE_URL_RWALIST_SEARCH_CONTROL As String = ConfigurationManager.AppSettings("RWAListSearchControlUrl").ToString()
    Public Shared PAGE_URL_RWA As String = ConfigurationManager.AppSettings("RWAUrl").ToString()
    Public Shared INVOICE_SEARCH_CONTROL_URL As String = ConfigurationManager.AppSettings("InvoiceSearchControlUrl").ToString()
    Public Shared INVOICE_DETAIL_PAGE_URL As String = ConfigurationManager.AppSettings("InvoiceDetailPageUrl").ToString()
    Public Shared INVOICE_HEADERLIST_PAGE_URL As String = ConfigurationManager.AppSettings("InvoiceHeaderPageUrl").ToString()
    Public Shared PAGE_URL_RWA_LIST As String = ConfigurationManager.AppSettings("RWAListUrl").ToString()
    Public Shared PAGE_URL_AOCCODE_DETAIL As String = ConfigurationManager.AppSettings("AocCodeDetailUrl").ToString()
    Public Shared PAGE_URL_AOCCODE_SEARCH_CONTROL As String = ConfigurationManager.AppSettings("AocCodeSearchControlUrl")
    Public Shared PAGE_URL_LINE_CODE As String = ConfigurationManager.AppSettings("LineCodeUrl")
    Public Shared PAGE_URL_LINE_CODE_LIST_SEARCH_CONTROL As String = ConfigurationManager.AppSettings("LineCodeListSearchControlUrl")
    Public Shared PAGE_URL_REASON_CODE As String = ConfigurationManager.AppSettings("ReasonCodeUrl")
    Public Shared PAGE_URL_REASON_CODE_LIST_SEARCH_CONTROL As String = ConfigurationManager.AppSettings("ReasonCodeListSearchControlUrl")
    Public Shared PAGE_URL_TRANSACTION_CODE As String = ConfigurationManager.AppSettings("TransactionCodeUrl")
    Public Shared PAGE_URL_TRANSACTION_CODE_LIST_SEARCH_CONTROL As String = ConfigurationManager.AppSettings("TransactionCodeListSearchControlUrl")
    Public Shared PAGE_URL_MATCHIMPORTINVOICELEVEL As String = ConfigurationManager.AppSettings("MatchImportInvoiceLevelUrl")
    Public Shared PAGE_URL_MATCHWORKLISTURL As String = ConfigurationManager.AppSettings("MatchWorkListUrl")
    Public Shared PAGE_URL_MATCHPOSTORELEVEL As String = ConfigurationManager.AppSettings("MatchPOStoreLevelUrl")
    Public Shared PAGE_URL_MATCHSTOREINVOICELEVEL As String = ConfigurationManager.AppSettings("MatchStoreInvoiceLevelUrl")
    Public Shared PAGE_URL_MATCHIMPORTINVLEVELNOUNMATCHRCPTS As String = ConfigurationManager.AppSettings("MatchImportInvLevelNoUnMtchRcptsUrl")
    Public Shared PAGE_URL_MATCHSTORESTYLELEVEL As String = ConfigurationManager.AppSettings("MatchStoreStyleLevelUrl")
    Public Shared PAGE_URL_MATCHMINISETLIST As String = ConfigurationManager.AppSettings("MatchMiniSetListUrl")
    Public Shared PAGE_URL_UNMATCHCONTROL As String = ConfigurationManager.AppSettings("UnMatchControlUrl")
    Public Shared PAGE_URL_RWI_SEARCH_CONTROL As String = ConfigurationManager.AppSettings("RWISearchControlUrl").ToString()
    Public Shared PAGE_URL_RWI As String = ConfigurationManager.AppSettings("RWIUrl").ToString()
    Public Shared PAGE_URL_UNMATCH As String = ConfigurationManager.AppSettings("UnMatchUrl").ToString()

    Public Shared PAGE_URL_BATCHCONTROL As String = ConfigurationManager.AppSettings("BatchControlUrl")
    Public Shared PAGE_URL_BATCHCONTROLLIST As String = ConfigurationManager.AppSettings("BatchControlListUrl")
    Public Shared PAGE_URL_BATCHCONTROLSPECIAL As String = ConfigurationManager.AppSettings("BatchControlSpecialUrl")

    Public Shared VALIDATIONMSG_FILEPATH As String = ConfigurationManager.AppSettings("ValidationMessageFilePath")

#End Region



#Region "Lawson Configuration"

    Public Shared LAWSON_VENDORGROUP As String = ConfigurationManager.AppSettings("VendorGroup").ToString()
    Public Shared LAWSON_COMPANY As String = ConfigurationManager.AppSettings("Company").ToString()
    Public Shared LAWSON_CHARTNAME As String = ConfigurationManager.AppSettings("ChartName").ToString()
    Public Shared LAWSON_ACCTUNIT As String = ConfigurationManager.AppSettings("AccountUnit").ToString()
    Public Shared LAWSON_ACCOUNT As String = ConfigurationManager.AppSettings("Account").ToString()
    Public Shared LAWSON_SUBACCOUNT As String = ConfigurationManager.AppSettings("SubAccount").ToString()

    'Lawson AGS call
    Public Shared LAWSON_PORTALURL As String = ConfigurationManager.AppSettings("LawsonPortalUrl").ToString()
    Public Shared LAWSON_USERNAME As String = ConfigurationManager.AppSettings("LawsonUserName").ToString()
    Public Shared LAWSON_PASSWORD As String = ConfigurationManager.AppSettings("LawsonPassword").ToString()
    Public Shared LAWSON_USERAGENT As String = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; MCNA6020; .NET CLR 1.1.4322; .NET CLR 2.0.50727; OfficeLiveConnector.1.3; OfficeLivePatch.0.0)"
    Public Shared LAWSON_POST As String = "POST"
    Public Shared LAWSON_GET As String = "GET"
    Public Shared LAWSON_CONTENTTYPE As String = "application/x-www-form-urlencoded"



#End Region


#Region "MQ Series Constants"

    'Match Process
    Public Const MQ_APPL_ID As String = "AP"
    Public Const MQ_REQUEST_QUEUE_CDE As String = "MATCH_CICS_REQ"
    Public Const MQ_RESPONSE_QUEUE_CDE As String = "MATCH_RESPONSE"
    Public Shared MQ_RESPONSEWAITINTERVAL As Integer = CInt(ConfigurationManager.AppSettings("ResponseWaitPeriod").ToString())


    'Comments Process
    Public Const Comment_APPL_ID As String = "AP"
    Public Const Comment_REQUEST_QUEUE_CDE As String = "INVOICE_COMMENT"

    'Due Date Process
    Public Const DueDate_APPL_ID As String = "AP"
    Public Const DueDate_Request_Queue_Cde As String = "DUE_DATE_CHANGE"

#End Region


#Region "Error Handling"

    Public Const ErrorOnPage As String = "*** Errors on page."

#End Region

#Region "Validation Regular Expressions"

    Public Const RegexLettersNumbersSpaces As String = "^[a-zA-Z0-9-\s]+$"
    Public Const RegexNumbersSpaces As String = "^[0-9\s]+$"

#End Region

End Class
