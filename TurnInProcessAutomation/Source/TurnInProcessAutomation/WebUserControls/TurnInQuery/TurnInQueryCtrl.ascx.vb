Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities
Public Class TurnInQueryCtrl
    Inherits System.Web.UI.UserControl
    Private _TUBuyer As New TUBuyer
    Private _TUCtlgAdPg As New TUCtlgAdPg
    Protected _TUAdInfo As New TUAdInfo
    Dim _TUDepartment As New TUDepartment
    Dim _TUVendorStyle As New TUVendorStyle
    Dim _TUVendor As New TUVendor
    Dim _TUClass As New TUClass
    Dim _TUACode As New TUACode
    Dim _TUTMS900PARAMETER As New TUTMS900PARAMETER
    Dim _TUFOB As New TUFOB
    Dim _TUCMG As New TUCMG
    Dim _TUCRG As New TUCRG
    Dim _TUCFG As New TUCFG
    Dim _TUSuffix As New TUImageSuffix
    Dim _TUWebCat As New TUWebCat

#Region "Properties"
    Public ReadOnly Property rcbRouteFromAd() As RadComboBox
        Get
            Return cmbRouteFromAd
        End Get
    End Property

    Public ReadOnly Property rcbCRG() As RadComboBox
        Get
            Return cmbCRG
        End Get
    End Property

    Public ReadOnly Property rcbCMG() As RadComboBox
        Get
            Return cmbCMG
        End Get
    End Property

    Public ReadOnly Property rcbCFG() As RadComboBox
        Get
            Return cmbCFG
        End Get
    End Property

    Public ReadOnly Property rcbBuyer() As RadComboBox
        Get
            Return cmbBuyer
        End Get
    End Property

    Public ReadOnly Property rcbFOB() As RadComboBox
        Get
            Return cmbFOB
        End Get
    End Property

    Public ReadOnly Property rcbDept() As RadComboBox
        Get
            Return cmbDept
        End Get
    End Property

    Public ReadOnly Property rcbClass() As RadComboBox
        Get
            Return cmbClass
        End Get
    End Property

    Public ReadOnly Property rcbAcode() As RadComboBox
        Get
            Return cmbACode
        End Get
    End Property

    Public ReadOnly Property rcbVendor() As RadComboBox
        Get
            Return cmbVendor
        End Get
    End Property

    Public ReadOnly Property rcbVendorStyle() As RadComboBox
        Get
            Return cmbVendorStyle
        End Get
    End Property

    Public ReadOnly Property rcbTurnInStatus() As RadComboBox
        Get
            Return cmbTurnInStatus
        End Get
    End Property

    Public ReadOnly Property rcbTurnInType() As RadComboBox
        Get
            Return cmbTurnInType
        End Get
    End Property
   
    Public ReadOnly Property rblView() As RadioButtonList
        Get
            Return rblViewType
        End Get
    End Property

    Public ReadOnly Property SelectedCMGID() As String
        Get
            Return cmbCMG.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedCFGID() As String
        Get
            Return cmbCFG.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedCRGID() As String
        Get
            Return cmbCRG.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedFOBID() As String
        Get
            Return cmbFOB.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedAd() As String
        Get
            Return cmbAdNo.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedAdPM() As String
        Get
            Return cmbAdNoPM.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedPageNumber() As String
        Get
            Return rcbPageNumber.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedBuyerId() As String
        Get
            Return cmbBuyer.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedVendorStyleID() As String
        Get
            Return cmbVendorStyle.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedDepartmentId() As Int16
        Get
            If Int16.TryParse(cmbDept.SelectedValue, Nothing) Then
                Return CShort(cmbDept.SelectedValue)
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property SelectedClassId() As Int16
        Get
            If Int16.TryParse(cmbClass.SelectedValue, Nothing) Then
                Return CShort(cmbClass.SelectedValue)
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property SelectedVendorId() As Integer
        Get
            If Integer.TryParse(cmbVendor.SelectedValue, Nothing) Then
                Return CInt(cmbVendor.SelectedValue)
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property SelectedACode() As String
        Get
            Try
                Return cmbACode.SelectedValue
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
    End Property

    Public ReadOnly Property Selectedtistatus() As String
        Get
            Try
                Return cmbTurnInStatus.SelectedValue
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
    End Property

    Public ReadOnly Property Selectedtintype() As String
        Get
            Try
                Return cmbTurnInType.SelectedValue
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
    End Property

    Public ReadOnly Property Selectedtidtefrm() As Date?
        Get
            Return dpTurninFrom.SelectedDate
        End Get
    End Property

    Public ReadOnly Property SelectedtidteTo() As Date?
        Get
            Return dpTurninTo.SelectedDate
        End Get
    End Property

    Public ReadOnly Property SelectedView() As String
        Get
            Try
                Return rblViewType.SelectedValue
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
    End Property

    Public ReadOnly Property SelectedInWebCat() As String
        Get
            Try
                Return cmbInWebCat.SelectedValue
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
    End Property

    Public ReadOnly Property SelectedSuffix() As String
        Get
            Try
                Return cmbSuffix.SelectedValue
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
    End Property

    Public ReadOnly Property SelectedModelCategory() As String
        Get
            Try
                Return rcbModelCategory.SelectedValue
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
    End Property

    Public ReadOnly Property SelectedImageType() As String
        Get
            Try
                Return cmbImageType.SelectedValue
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
    End Property

    Public ReadOnly Property SelectedFeatWebCat() As String
        Get
            Try
                Return rcbFeatWebCat.SelectedValue
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
    End Property

    Public ReadOnly Property BatchNumberValue() As String
        Get
            Return txtBatchNumber.Text
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        modalPopup.OpenerElementID = rbShowDialog.ClientID
        If Not Page.IsPostBack Then
            If Request.QueryString("View") = "PM" Then
                rblViewType.SelectedIndex = 1
                tblPMfields.Visible = True
                tblSQFields.Visible = False
            End If
        End If
    End Sub

#Region "Filters"
    Protected Sub cmbCRG_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        cmbCMG.ClearSelection()
        cmbCFG.ClearSelection()
        With cmbCRG
            .DataSource = Nothing
            .DataSource = _TUCRG.GetAllFromCRG().ToList()
            .DataValueField = "CRG_ID"
            .DataTextField = "CRG_DSC"
            .DataBind()
        End With
    End Sub

    Protected Sub cmbCFG_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        If cmbCRG.Text.Length = 0 And cmbCMG.Text.Length = 0 Then
            With cmbCFG
                .DataSource = Nothing
                .DataSource = If(String.IsNullOrEmpty(cmbCMG.SelectedValue), _TUCFG.GetAllFromCFG().ToList(), _TUCFG.GetAllFromCFG().ToList().FindAll(Function(x) x.CMG_ID = CDbl(cmbCMG.SelectedValue)))
                .DataValueField = "CFG_ID"
                .DataTextField = "CFG_DESC"
                .DataBind()
            End With
        End If
    End Sub

    Protected Sub cmbCMG_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        If cmbCRG.Text.Length = 0 Then
            With cmbCMG
                .DataSource = Nothing
                .DataSource = _TUCMG.GetAllFromCMG().ToList()
                .DataValueField = "CMG_ID"
                .DataTextField = "CMG_DESC"
                .DataBind()
            End With
        End If
    End Sub

    Protected Sub cmbFOB_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        With cmbFOB
            .DataSource = Nothing
            .DataSource = _TUFOB.GetAllFOB().ToList()
            .DataValueField = "FOB_ID"
            .DataTextField = "FOB_DESC"
            .DataBind()
        End With
    End Sub

    Protected Sub cmbAdNo_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        cmbAdNo.DataSource = Nothing
        cmbAdNo.DataSource = _TUAdInfo.GetAdsForQueryTool().ToList()
        cmbAdNo.DataValueField = "adnbr"
        cmbAdNo.DataTextField = "AdNumberDesc"
        cmbAdNo.DataBind()
    End Sub

    Protected Sub cmbAdNoPM_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        cmbAdNoPM.DataSource = Nothing
        cmbAdNoPM.DataSource = _TUAdInfo.GetAdsForQueryTool().Where(Function(x) CDate(x.TurnInDate) > Now.AddMonths(-6)).OrderByDescending(Function(x) x.adnbr).ToList()
        cmbAdNoPM.DataValueField = "adnbr"
        cmbAdNoPM.DataTextField = "AdNumberDesc"
        cmbAdNoPM.DataBind()
    End Sub

    Private Sub cmbBuyer_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs) Handles cmbBuyer.ItemsRequested
        With cmbBuyer
            .DataSource = Nothing
            .DataSource = _TUBuyer.GetAllFromBuyer
            .DataTextField = "BuyerName"
            .DataValueField = "BuyerId"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbDept_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbDept
            .DataSource = Nothing
            .DataSource = _TUDepartment.GetAllFromDepartment
            .DataTextField = "DeptIdDesc"
            .DataValueField = "DeptId"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbVendorStyle_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        If e.Text.Length >= 3 Then
            With cmbVendorStyle
                .DataSource = Nothing
                .DataSource = _TUVendorStyle.GetAllFromVendorStyle(CInt(cmbDept.SelectedValue), e.Text)
                .DataTextField = "VENDORSTYLENUMBER"
                .DataValueField = "VENDORSTYLENUMBER"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub

    Protected Sub cmbSuffix_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbSuffix
            .DataSource = Nothing
            .DataSource = _TUSuffix.GetAllFromImageSuffix
            .DataTextField = "imgsfxANDdesc"
            .DataValueField = "imgsfxcd"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbVendor_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        If cmbDept.Text <> "" Then
            With cmbVendor
                .Enabled = True
                .DataTextField = "VendorIdName"
                .DataValueField = "VendorId"
                .DataSource = _TUVendor.GetAllFromVendorByDepartment(CInt(cmbDept.SelectedValue))
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub

    Protected Sub cmbClass_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbClass
            .Enabled = True
            .DataTextField = "ClassIdDesc"
            .DataValueField = "ClassId"
            .DataSource = _TUClass.GetAllFromClassByDepartment(CInt(cmbDept.SelectedValue))
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbACode_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbACode
            .Enabled = True
            .DataTextField = "ACodeCompoundDesc"
            .DataValueField = "ACode"
            .DataSource = _TUACode.GetAllFromACodeByDepartment(CInt(cmbDept.SelectedValue))
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbDept_SelectedIndexChanged(ByVal sender As Object, ByVal e As RadComboBoxSelectedIndexChangedEventArgs)
        cmbVendor.ClearSelection()
        cmbClass.ClearSelection()
        cmbACode.ClearSelection()
        cmbVendorStyle.ClearSelection()
        cmbVendorStyle.Items.Clear()
        cmbVendorStyle.Text = String.Empty

        If cmbDept.Text <> "" Then
            cmbVendor.DataSource = Nothing
            cmbClass.DataSource = Nothing
            cmbACode.DataSource = Nothing
            cmbVendorStyle.Enabled = True
            With cmbVendor
                .Enabled = True
                .DataTextField = "VendorIdName"
                .DataValueField = "VendorId"
                .DataSource = _TUVendor.GetAllFromVendorByDepartment(CInt(cmbDept.SelectedValue))
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With

            With cmbClass
                .Enabled = True
                .DataTextField = "ClassIdDesc"
                .DataValueField = "ClassId"
                .DataSource = _TUClass.GetAllFromClassByDepartment(CInt(cmbDept.SelectedValue))
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With

            With cmbACode
                .Enabled = True
                .DataTextField = "ACodeCompoundDesc"
                .DataValueField = "ACode"
                .DataSource = _TUACode.GetAllFromACodeByDepartment(CInt(cmbDept.SelectedValue))
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        Else
            cmbClass.Enabled = False
            cmbVendor.Enabled = False
            cmbACode.Enabled = False
            cmbVendorStyle.Enabled = False
        End If
    End Sub


    'TODO: lblTurnInType and cmbTurnInType controls are hidden. Un-Hide them during Print Phase.
    Protected Sub cmbTurnInType_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs) Handles cmbBuyer.ItemsRequested
        With cmbTurnInType
            .DataSource = Nothing
            .DataSource = _TUTMS900PARAMETER.GetAllTurnInTypeValues
            .DataValueField = "CharIndex"
            .DataTextField = "LongDesc"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub rcbModelCategory_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With rcbModelCategory
            .DataSource = Nothing
            .DataSource = _TUTMS900PARAMETER.GetAllModelCategories
            .DataValueField = "CharIndex"
            .DataTextField = "LongDesc"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub rcbFeatWebCat_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With rcbFeatWebCat
            .DataSource = Nothing
            .DataSource = _TUWebCat.GetFeatureWebCat
            .DataValueField = "CategoryName"
            .DataTextField = "CategoryName"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

#End Region

#Region "Validations"
    Public Sub ValidateSearch()
        Page.Validate("Search")
    End Sub
    Public Sub ttvAds_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
        If CType(validator.EvaluatedControl, RadComboBox).SelectedValue = "" Then
            validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1003)
            args.IsValid = False
        End If
    End Sub

    Public Sub ttvBatch_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
        If CType(validator.EvaluatedControl, TextBox).Text = "" Then
            validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1003)
            args.IsValid = False
        End If
    End Sub

    Public Sub ttvPageNumber_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
        If CType(validator.EvaluatedControl, RadComboBox).SelectedValue = "" Then
            validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1004)
            args.IsValid = False
        End If
    End Sub
#End Region

    Protected Sub cmbAdNoPM_SelectedIndexChanged(ByVal sender As Object, ByVal e As RadComboBoxSelectedIndexChangedEventArgs)
        rcbPageNumber.ClearSelection()
        rcbPageNumber.Items.Clear()
        If cmbAdNoPM.Text <> "" Then
            rcbPageNumber.DataSource = Nothing

            With rcbPageNumber
                .Enabled = True
                .DataValueField = "PgNbr"
                .DataTextField = "PageNumberDesc"
                .DataSource = _TUCtlgAdPg.GetAllFromCtlgAdPg(CInt(cmbAdNoPM.SelectedValue))
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
            ' _eCommTurnInMeet.GetAdInfo(rcbAds.SelectedValue)
        Else
            rcbPageNumber.Enabled = False
        End If
    End Sub

    Private Sub rblViewType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles rblViewType.SelectedIndexChanged
        If rblViewType.SelectedValue = "2" Then
            tblPMfields.Visible = True
            tblSQFields.Visible = False
        Else
            tblPMfields.Visible = False
            tblSQFields.Visible = True
        End If
    End Sub


    Private Sub cmbCRG_TextChanged(sender As Object, e As System.EventArgs) Handles cmbCRG.TextChanged
        cmbCMG.Items.Clear()
        cmbCMG.Text = ""
        cmbCFG.Items.Clear()
        cmbCFG.Text = ""
        If cmbCRG.Text.Length > 0 Then
            Dim CRG As Integer = CInt(cmbCRG.SelectedValue)

            With cmbCMG
                .DataSource = Nothing
                .DataSource = _TUCMG.GetAllFromCMG().ToList().FindAll(Function(x) x.CRG_ID = CRG)
                .DataValueField = "CMG_ID"
                .DataTextField = "CMG_DESC"
                cmbCMG.Items.Insert(0, New RadComboBoxItem("", ""))
                .DataBind()
            End With

            'if only one item in the CMG dropdown, populate CFG
            If _TUCMG.GetAllFromCMG().ToList().FindAll(Function(x) x.CRG_ID = CRG).Count >= 2 Then
                cmbCMG.SelectedIndex = 0
                PopulateCFG()
            End If
            
        End If
    End Sub

    Private Sub cmbCMG_TextChanged(sender As Object, e As System.EventArgs) Handles cmbCMG.TextChanged
        PopulateCFG()
    End Sub

    Private Sub PopulateCFG()
        cmbCFG.Items.Clear()
        cmbCFG.Text = ""

        If cmbCMG.SelectedValue.Length > 0 Then
            Dim CMG As Integer = CInt(cmbCMG.SelectedValue)

            With cmbCFG
                .DataSource = Nothing
                .DataSource = _TUCFG.GetAllFromCFG().ToList().FindAll(Function(x) x.CMG_ID = CMG)
                .DataValueField = "CFG_ID"
                .DataTextField = "CFG_DESC"
                .DataBind()
                cmbCFG.Items.Insert(0, New RadComboBoxItem("", ""))
                cmbCFG.SelectedIndex = 0
            End With
        End If
        
    End Sub
End Class
