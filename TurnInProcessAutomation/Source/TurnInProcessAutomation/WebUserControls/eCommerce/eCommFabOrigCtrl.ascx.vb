Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL.Enumerations

Public Class eCommFabOrigCtrl
    Inherits System.Web.UI.UserControl

    Dim _isTestMode As Boolean = False
    Private Const ItemsPerRequest As Integer = 10

    Dim _TUAdInfo As New TUAdInfo
    Dim _TUCtlgAdPg As New TUCtlgAdPg
    Private _TUBuyer As New TUBuyer
    Dim _TUDepartment As New TUDepartment
    Dim _TUSellSeason As New TUSellSeason
    Dim _TUSellYear As New TUSellYear
    Dim _TUClass As New TUClass
    Dim _TUVendor As New TUVendor
    Dim _TUACode As New TUACode
    Dim _TUSubClass As New TUSubClass
    Dim _TUVendorStyle As New TUVendorStyle
    Dim _TUBatch As New TUBatch
    Dim _TUDMM As New TUDMM
    Dim _TUEcommFabOrig As New TUEcommFabOrig
    Dim _TUEcommSetupCreate As New TUEcommSetupCreate

    Public Event cmbDeptSelectedIndexChanged As System.EventHandler
    Public Event cmbClassSelectedIndexChanged As System.EventHandler
    Public Event cmbSubClassSelectedIndexChanged As System.EventHandler
    Public Event cmbVendorSelectedIndexChanged As System.EventHandler
    Public Event cmbVendorStyleSelectedIndexChanged As System.EventHandler
    Public Event cmbACode1SelectedIndexChanged As System.EventHandler
    Public Event cmbACode2SelectedIndexChanged As System.EventHandler
    Public Event cmbACode3SelectedIndexChanged As System.EventHandler
    Public Event cmbACode4SelectedIndexChanged As System.EventHandler   
    Public Event cmbSellYearSelectedIndexChanged As System.EventHandler
    Public Event cmbSellSeasonSelectedIndexChanged As System.EventHandler
    Public Event lbVendorStylesDataBinding As System.EventHandler
    Public Event imgAddVendorStyleClick As System.EventHandler(Of System.Web.UI.ImageClickEventArgs)
    Public Event dpCreatedSinceDateChanged As System.EventHandler
    Public Event cblPriceStatusCodesSelectedIndexChanged As System.EventHandler

    Public Event cmbDMMSelectedIndexChanged As System.EventHandler
    Public Event cmbPOBuyerSelectedIndexChanged As System.EventHandler
    Public Event cmbPODeptSelectedIndexChanged As System.EventHandler
    Public Event cmbPOClassSelectedIndexChanged As System.EventHandler
    Public Event cmbPOVendorSelectedIndexChanged As System.EventHandler
    Public Event cmbPOVendorStyleSelectedIndexChanged As System.EventHandler
    Public Event imgAddPOVendorStyleClick As System.EventHandler(Of System.Web.UI.ImageClickEventArgs)
    Public Event dpStartShipDateDateChanged As System.EventHandler

    Public Event imgAddISNClick As System.EventHandler(Of System.Web.UI.ImageClickEventArgs)
    Public Event txtSkuTextChanged As System.EventHandler

#Region "Properties"

    Public ReadOnly Property SelectedAd() As Integer
        Get
            If Integer.TryParse(rcbAds.SelectedValue, Nothing) Then
                Return CInt(rcbAds.SelectedValue)
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property AdComboBox() As RadComboBox
        Get
            Return rcbAds
        End Get
    End Property

    Public ReadOnly Property PageNumberComboBox() As RadComboBox
        Get
            Return rcbPageNumber
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

    Public ReadOnly Property SelectedSubClassId() As Int16
        Get
            If Int16.TryParse(cmbSubClass.SelectedValue, Nothing) Then
                Return CShort(cmbSubClass.SelectedValue)
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

    Public ReadOnly Property SelectedACode1() As String
        Get
            Return cmbACode1.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedACode2() As String
        Get
            Return cmbACode2.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedACode3() As String
        Get
            Return cmbACode3.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedACode4() As String
        Get
            Return cmbACode4.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedYear() As Int16
        Get
            If Int16.TryParse(cmbSellYear.SelectedValue, Nothing) Then
                Return CShort(cmbSellYear.SelectedValue)
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property SelectedSeasonId() As Integer
        Get
            If Integer.TryParse(cmbSellSeason.SelectedValue, Nothing) Then
                Return CInt(cmbSellSeason.SelectedValue)
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property SelectedCreatedSince() As Date?
        Get
            Return dpCreatedSince.SelectedDate
        End Get
    End Property

    Public Property SelectedVendorStyles As List(Of String)
        Get
            If Session("EComFabOrigCtrl.SelectedVendorStyles") Is Nothing Then
                Session("EComFabOrigCtrl.SelectedVendorStyles") = New List(Of String)
            End If
            Return CType(Session("EComFabOrigCtrl.SelectedVendorStyles"), List(Of String))
        End Get
        Set(value As List(Of String))
            Session("EComFabOrigCtrl.SelectedVendorStyles") = value
        End Set
    End Property

    Public Property SelectedPOVendorStyles As List(Of String)
        Get
            If Session("EComFabOrigCtrl.SelectedPOVendorStyles") Is Nothing Then
                Session("EComFabOrigCtrl.SelectedPOVendorStyles") = New List(Of String)
            End If
            Return CType(Session("EComFabOrigCtrl.SelectedPOVendorStyles"), List(Of String))
        End Get
        Set(value As List(Of String))
            Session("EComFabOrigCtrl.SelectedPOVendorStyles") = value
        End Set
    End Property

    Public Property SelectedVendorIds As List(Of String)
        Get
            If Session("PreTurnInSetUpCtrl.SelectedVendorIds") Is Nothing Then
                Session("PreTurnInSetUpCtrl.SelectedVendorIds") = New List(Of String)
            End If
            Return CType(Session("PreTurnInSetUpCtrl.SelectedVendorIds"), List(Of String))
        End Get
        Set(value As List(Of String))
            Session("PreTurnInSetUpCtrl.SelectedVendorIds") = value
        End Set
    End Property

    Public Property SelectedISNs As List(Of String)
        Get
            If Session("PreTurnInSetUpCtrl.SelectedISNs") Is Nothing Then
                Session("PreTurnInSetUpCtrl.SelectedISNs") = New List(Of String)
            End If
            Return CType(Session("PreTurnInSetUpCtrl.SelectedISNs"), List(Of String))
        End Get
        Set(value As List(Of String))
            Session("PreTurnInSetUpCtrl.SelectedISNs") = value
        End Set
    End Property

    Public Property SelectedReserveISNs As List(Of String)
        Get
            If Session("PreTurnInSetUpCtrl.SelectedReserveISNs") Is Nothing Then
                Session("PreTurnInSetUpCtrl.SelectedReserveISNs") = New List(Of String)
            End If
            Return CType(Session("PreTurnInSetUpCtrl.SelectedReserveISNs"), List(Of String))
        End Get
        Set(value As List(Of String))
            Session("PreTurnInSetUpCtrl.SelectedReserveISNs") = value
        End Set
    End Property

    Public ReadOnly Property cblPriceStatus As CheckBoxList
        Get
            Return DirectCast(rpbResultsTab.Items(0).Items(0).FindControl("cblPriceStatusCodes"), CheckBoxList)
        End Get
    End Property

    Public ReadOnly Property PriceStatusCodes As List(Of String)
        Get
            Dim StatusCodes As New List(Of String)
            For Each cb As ListItem In DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("cblPriceStatusCodes"), CheckBoxList).Items
                If cb.Selected Then
                    StatusCodes.Add(cb.Value)
                End If
            Next
            Return StatusCodes
        End Get
    End Property

    Public ReadOnly Property cmbDept As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("cmbDept"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property cmbClass As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("cmbClass"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property cmbSubClass As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("cmbSubClass"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property cmbVendor As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("cmbVendor"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property cmbVendorStyle As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("cmbVendorStyle"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property cmbACode1 As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("cmbACode1"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property cmbACode2 As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("cmbACode2"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property cmbACode3 As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("cmbACode3"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property cmbACode4 As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("cmbACode4"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property cmbSellYear As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("cmbSellYear"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property cmbSellSeason As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("cmbSellSeason"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property dpCreatedSince As RadDatePicker
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("dpCreatedSince"), RadDatePicker)
        End Get
    End Property

    Public ReadOnly Property imgAddVendorStyle As ImageButton
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("imgAddVendorStyle"), ImageButton)
        End Get
    End Property

    Public ReadOnly Property lbVendorStyles As WebControls.ListBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("lbVendorStyles"), WebControls.ListBox)
        End Get
    End Property

    Public ReadOnly Property txtISN As RadTextBox
        Get
            Return DirectCast(rpbResultsTab.Items(2).Items(0).FindControl("txtISN"), RadTextBox)
        End Get
    End Property

    Public ReadOnly Property txtSku As RadTextBox
        Get
            Return DirectCast(rpbResultsTab.Items(2).Items(0).FindControl("txtSku"), RadTextBox)
        End Get
    End Property

    Public ReadOnly Property lbISNs As WebControls.ListBox
        Get
            Return DirectCast(rpbResultsTab.Items(2).Items(0).FindControl("lbISNs"), WebControls.ListBox)
        End Get
    End Property

    Public Property ResultsTabRadPanelBar As RadPanelBar
        Get
            Return rpbResultsTab
        End Get
        Set(value As RadPanelBar)
            rpbResultsTab = value
        End Set
    End Property

    Public ReadOnly Property SelectedBatchId() As String
        Get
            Return cmbBatch.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedDeptId() As String
        Get
            Return cmbDeptMaint.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedBuyerId() As String
        Get
            Return cmbBuyer.SelectedValue
        End Get
    End Property
    Public ReadOnly Property cmbDMM As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(0).Items(0).FindControl("cmbDMM"), RadComboBox)
        End Get
    End Property
    Public ReadOnly Property cmbPOBuyer As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(0).Items(0).FindControl("cmbPOBuyer"), RadComboBox)
        End Get
    End Property
    Public ReadOnly Property cmbPODept As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(0).Items(0).FindControl("cmbPODept"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property cmbPOClass As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(0).Items(0).FindControl("cmbPOClass"), RadComboBox)
        End Get
    End Property
    Public ReadOnly Property cmbPOVendor As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(0).Items(0).FindControl("cmbPoVendor"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property cmbPOVendorStyle As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(0).Items(0).FindControl("cmbPOVendorStyle"), RadComboBox)
        End Get
    End Property
    Public ReadOnly Property dpStartShipDate As RadDatePicker
        Get
            Return DirectCast(rpbResultsTab.Items(0).Items(0).FindControl("dpStartShipDate"), RadDatePicker)
        End Get
    End Property

    Public ReadOnly Property lbPOVendorStyles As WebControls.ListBox
        Get
            Return DirectCast(rpbResultsTab.Items(0).Items(0).FindControl("lbPOVendorStyles"), WebControls.ListBox)
        End Get
    End Property
#End Region

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            SelectedReserveISNs.Clear()
            SelectedISNs.Clear()
            SelectedPOVendorStyles.Clear()
            SelectedVendorStyles.Clear()
            BindrcbAds()
            Me.SetDefaultShipDays()
        End If
    End Sub

    Public Function ValidateMaintSearch() As Boolean
        If rcbAds.SelectedValue = "" And cmbDeptMaint.SelectedValue = "" And cmbBuyer.SelectedValue = "" And cmbBatch.SelectedValue = "" Then
            Return False
        Else : Return True
        End If
    End Function

    Public Sub ChangeMultiView(ByVal TabName As String)
        ttvAdNbr.ValidationGroup = "Search"
        ttvPageNumber.ValidationGroup = "Search"
        Select Case TabName
            Case "Ad List"
                mvSearchOptions.ActiveViewIndex = 0
                BuyerRow.Visible = True
                DeptRow.Visible = True
                BatchRow.Visible = True
                ttvAdNbr.ValidationGroup = "None"
                ttvPageNumber.ValidationGroup = "None"
            Case "Killed"
                mvSearchOptions.ActiveViewIndex = 0
                BuyerRow.Visible = False
                DeptRow.Visible = False
                BatchRow.Visible = True
                ttvAdNbr.ValidationGroup = "None"
                ttvPageNumber.ValidationGroup = "None"

                ResetComboBox(rcbAds)
                ResetComboBox(rcbPageNumber)
                ResetComboBox(cmbBatch)
            Case "Ad Level"
                mvSearchOptions.ActiveViewIndex = 0
                BuyerRow.Visible = False
                DeptRow.Visible = False
                BatchRow.Visible = False
            Case "Result List"
                mvSearchOptions.ActiveViewIndex = 1
            Case "Fabrication/Origination"
                mvSearchOptions.ActiveViewIndex = 1
        End Select
    End Sub

    Private Sub ResetComboBox(ByVal rcbComboBox As RadComboBox)
        rcbComboBox.ClearSelection()
        rcbComboBox.Text = ""
    End Sub


#Region "AD LIST: Filters"
    Private Sub cmbBuyer_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs) Handles cmbBuyer.ItemsRequested
        With cmbBuyer
            .DataSource = _TUBuyer.GetAllFromBuyer
            .DataTextField = "BuyerNameId"
            .DataValueField = "BuyerId"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbDeptMaint_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbDeptMaint
            .DataSource = _TUDepartment.GetAllFromDepartment
            .DataTextField = "DeptIdDesc"
            .DataValueField = "DeptId"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbBatch_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbBatch
            .DataSource = Nothing
            If Not IsNothing(Session("PreTurnInSetUp.Tab")) Then
                If Session("PreTurnInSetUp.Tab").ToString = "pvKilled" Then
                    .DataSource = _TUBatch.GetAllBatches("KLLD")
                Else
                    .DataSource = _TUBatch.GetAllBatches("PEND")
                End If
            Else
                .DataSource = _TUBatch.GetAllBatches("PEND")
            End If


            .DataTextField = "BatchId"
            .DataValueField = "BatchId"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub
#End Region

#Region "AD TAB: Filters"

    Protected Sub rcbAds_SelectedIndexChanged(ByVal sender As Object, ByVal e As RadComboBoxSelectedIndexChangedEventArgs)
        rcbPageNumber.ClearSelection()
        rcbPageNumber.Items.Clear()
        If rcbAds.Text <> "" Then
            With rcbPageNumber
                .Enabled = True
                .DataValueField = "PgNbr"
                .DataTextField = "PageNumberDesc"
                .DataSource = _TUCtlgAdPg.GetAllFromCtlgAdPg(CInt(rcbAds.SelectedValue))
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        Else
            rcbPageNumber.Enabled = False
        End If
    End Sub

    Private Sub BindrcbAds()
        If Request.QueryString.Count > 0 Then
            With rcbAds
                .Items.Clear()
                If Request.QueryString("Action").ToUpper = Modes.MAINTENANCE.ToString Then
                    .DataSource = _TUAdInfo.GetAdsForMaintenance().ToList()
                Else
                    .DataSource = _TUAdInfo.GetAllFromAdInfoFiltered(True).ToList()
                End If
                .DataValueField = "adnbr"
                .DataTextField = "AdNumberDesc"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub

#End Region

#Region "AD TAB: Validations"
    Public Sub ValidateSearch()
        Page.Validate("Search")
    End Sub

    Public Sub ttvAdNbr_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
        Dim value As String = DirectCast(validator.EvaluatedControl, RadComboBox).Text.Trim.ToUpper
        If String.IsNullOrEmpty(value) Then
            validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1003)
            args.IsValid = False
        End If
    End Sub

    Public Sub ttvPageNumber_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
        Dim value As String = DirectCast(validator.EvaluatedControl, RadComboBox).Text.Trim.ToUpper
        If String.IsNullOrEmpty(value) Then
            validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1004)
            args.IsValid = False
        End If
    End Sub

    Private Shared Function GetStatusMessage(ByVal offset As Integer, ByVal total As Integer) As String
        If total <= 0 Then
            Return "No matches"
        End If

        Return [String].Format("Items <b>1</b>-<b>{0}</b> of <b>{1}</b>", offset, total)
    End Function
#End Region

#Region "RESULTS TAB: Validations"

    Public Sub ttvISN_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
        Dim value As String = DirectCast(validator.EvaluatedControl, RadTextBox).Text.Trim.ToUpper
        Dim isn As Decimal
        If Not txtISN Is Nothing Then
            If txtISN.Text = "" Then
                validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1006) 'required
                args.IsValid = False
            ElseIf SelectedISNs.Contains(txtISN.Text) Then
                validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1007) 'You already added this ISN
                args.IsValid = False
            ElseIf Not Decimal.TryParse(Trim(txtISN.Text), isn) Then
                validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1008) 'not valid
                args.IsValid = False
            ElseIf Not (IsValidISN(isn, False)) Then
                validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1008) 'not a real ISN
                args.IsValid = False
            End If
        Else
            args.IsValid = True
        End If
    End Sub

#End Region

#Region "RESULTS TAB: Filters"

    Protected Sub cmbDept_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbDept
            .DataSource = _TUDepartment.GetAllFromDepartment
            .DataTextField = "DeptIdDesc"
            .DataValueField = "DeptId"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbSellSeason_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbSellSeason
            .DataSource = _TUSellSeason.GetAllFromSellSeason
            .DataTextField = "DescSellSeasonId"
            .DataValueField = "SellSeasonId"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbSellYear_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbSellYear
            .DataSource = _TUSellYear.GetAllFromSellYear
            .DataTextField = "SellYear"
            .DataValueField = "SellYear"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbClass_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbClass
            .Enabled = True
            .DataTextField = "ClassIdDesc"
            .DataValueField = "ClassId"
            .DataSource = _TUClass.GetAllFromClassByDepartment(CInt(cmbDept.SelectedValue))
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbVendor_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbVendor
            .Enabled = True
            .DataTextField = "VendorIdName"
            .DataValueField = "VendorId"
            .DataSource = _TUVendor.GetAllFromVendorByDepartment(CInt(cmbDept.SelectedValue))
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbACode1_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbACode1
            .Enabled = True
            .DataTextField = "ACodeCompoundDesc"
            .DataValueField = "ACode"
            .DataSource = _TUACode.GetAllFromACodeByDepartment(CInt(cmbDept.SelectedValue))
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbACode2_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbACode2
            .Enabled = True
            .DataTextField = "ACodeCompoundDesc"
            .DataValueField = "ACode"
            .DataSource = _TUACode.GetAllFromACodeByDepartment(CInt(cmbDept.SelectedValue))
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbACode3_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbACode3
            .Enabled = True
            .DataTextField = "ACodeCompoundDesc"
            .DataValueField = "ACode"
            .DataSource = _TUACode.GetAllFromACodeByDepartment(CInt(cmbDept.SelectedValue))
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbACode4_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbACode4
            .Enabled = True
            .DataTextField = "ACodeCompoundDesc"
            .DataValueField = "ACode"
            .DataSource = _TUACode.GetAllFromACodeByDepartment(CInt(cmbDept.SelectedValue))
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbSubClass_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbSubClass
            .Enabled = True
            .DataTextField = "SubClassIdDesc"
            .DataValueField = "SubClassId"
            .DataSource = _TUSubClass.GetAllFromSubClassByDeptClass(CInt(cmbDept.SelectedValue), CInt(cmbClass.SelectedValue))
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbVendorStyle_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)
        RaiseEvent cmbVendorStyleSelectedIndexChanged(Me, New EventArgs())
    End Sub

    Protected Sub cmbACode1_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)
        RaiseEvent cmbACode1SelectedIndexChanged(Me, New EventArgs())
    End Sub

    Protected Sub cmbACode2_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)
        RaiseEvent cmbACode2SelectedIndexChanged(Me, New EventArgs())
    End Sub

    Protected Sub cmbACode3_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)
        RaiseEvent cmbACode3SelectedIndexChanged(Me, New EventArgs())
    End Sub

    Protected Sub cmbACode4_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)
        RaiseEvent cmbACode4SelectedIndexChanged(Me, New EventArgs())
    End Sub

    Protected Sub cmbSellYear_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)
        RaiseEvent cmbSellYearSelectedIndexChanged(Me, New EventArgs())
    End Sub

    Protected Sub cmbSellSeason_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)
        RaiseEvent cmbSellSeasonSelectedIndexChanged(Me, New EventArgs())
    End Sub

    Protected Sub cmbVendorStyle_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        If cmbVendor.SelectedValue <> String.Empty OrElse e.Text.Trim() <> String.Empty Then
            With cmbVendorStyle
                .ClearSelection()
                .Enabled = True
                .DataTextField = "VendorStyleNumber"
                .DataValueField = "VendorStyleNumber"
                .DataSource = _TUVendorStyle.GetAllFromVendorStyle(CInt(cmbDept.SelectedValue), cmbVendor.SelectedValue, cmbClass.SelectedValue, cmbSubClass.SelectedValue, If(cmbVendor.SelectedValue = String.Empty, e.Text.Trim(), String.Empty))
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub

    Protected Sub cmbDept_SelectedIndexChanged(ByVal sender As Object, ByVal e As RadComboBoxSelectedIndexChangedEventArgs)
        cmbClass.ClearSelection()
        cmbClass.Text = ""
        cmbSubClass.ClearSelection()
        cmbSubClass.Text = ""
        cmbACode1.ClearSelection()
        cmbACode1.Text = ""
        cmbACode2.ClearSelection()
        cmbACode2.Text = ""
        cmbACode3.ClearSelection()
        cmbACode3.Text = ""
        cmbACode4.ClearSelection()
        cmbACode4.Text = ""
        cmbVendor.ClearSelection()
        cmbVendor.Text = ""
        cmbVendorStyle.ClearSelection()
        cmbVendorStyle.Text = ""

        cmbACode4.Enabled = False
        cmbACode3.Enabled = False
        cmbACode2.Enabled = False
        cmbACode1.Enabled = False
        cmbSubClass.Enabled = False
        cmbClass.Enabled = False
        cmbVendor.Enabled = False
        cmbVendorStyle.Enabled = False
        cmbSellSeason.Enabled = False
        cmbSellYear.Enabled = False

        If cmbDept.SelectedValue <> "" Then
            cmbClass.Enabled = True
            cmbVendor.Enabled = True
            cmbVendorStyle.Enabled = True
            cmbACode1.Enabled = True
            cmbACode2.Enabled = True
            cmbACode3.Enabled = True
            cmbACode4.Enabled = True
            cmbSellSeason.Enabled = True
            cmbSellYear.Enabled = True
        End If
        RaiseEvent cmbDeptSelectedIndexChanged(cmbDept, New EventArgs())
    End Sub

    Protected Sub cmbClass_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)
        cmbSubClass.ClearSelection()
        cmbSubClass.Text = ""
        cmbSubClass.Enabled = False
        cmbVendorStyle.ClearSelection()
        cmbVendorStyle.Text = ""

        If cmbClass.SelectedValue <> "" Then
            cmbSubClass.Enabled = True
        End If
        RaiseEvent cmbClassSelectedIndexChanged(Me, New EventArgs())
    End Sub

    Protected Sub cmbVendor_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)
        cmbVendorStyle.SelectedValue = Nothing
        cmbVendorStyle.Text = ""
        cmbVendorStyle.ClearSelection()

        If cmbVendor.SelectedValue <> "" Then
            cmbVendorStyle.Enabled = True
            SelectedVendorIds.Clear()
            If Me.SelectedVendorIds.Contains(Trim(cmbVendor.SelectedValue.ToString())) = False Then
                SelectedVendorIds.Add(Trim(cmbVendor.SelectedValue.ToString()))
            End If

        End If
        RaiseEvent cmbVendorSelectedIndexChanged(Me, New EventArgs())
    End Sub

    Protected Sub imgAddVendorStyle_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        If cmbVendorStyle.SelectedValue <> "" And Not (SelectedVendorStyles.Contains(cmbVendorStyle.SelectedValue.ToString().Trim())) And Not (lbVendorStyles.Items.Contains(TryCast(TryCast(cmbVendorStyle.SelectedValue.Trim(), Object), ListItem))) Then
            SelectedVendorStyles.Add(cmbVendorStyle.SelectedValue.ToString().Trim())
            lbVendorStyles.DataSource = SelectedVendorStyles
            lbVendorStyles.DataBind()
            cmbVendorStyle.ClearSelection()
            cmbVendorStyle.SelectedValue = Nothing
            cmbVendorStyle.Text = ""
        End If
        RaiseEvent imgAddVendorStyleClick(Me, e)
    End Sub

    Protected Sub imgAddPOVendorStyle_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        If cmbPOVendorStyle.SelectedValue <> "" And Not (SelectedPOVendorStyles.Contains(cmbPOVendorStyle.SelectedValue.ToString().Trim())) And Not (lbPOVendorStyles.Items.Contains(TryCast(TryCast(cmbPOVendorStyle.SelectedValue.Trim(), Object), ListItem))) Then
            SelectedPOVendorStyles.Add(cmbPOVendorStyle.SelectedValue.ToString().Trim())
            lbPOVendorStyles.DataSource = SelectedPOVendorStyles
            lbPOVendorStyles.DataBind()
            cmbPOVendorStyle.ClearSelection()
            cmbPOVendorStyle.SelectedValue = Nothing
            cmbPOVendorStyle.Text = ""
        End If
        RaiseEvent imgAddPOVendorStyleClick(Me, e)
    End Sub

    Protected Sub imgAddISN_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        If Page.IsValid Then
            SelectedISNs.Add(Trim(txtISN.Text))
            lbISNs.DataSource = SelectedISNs
            lbISNs.DataBind()
            txtISN.Text = ""
        End If
        RaiseEvent imgAddISNClick(Me, e)
    End Sub

    Protected Sub btnResetVendorStyles_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        SelectedVendorStyles.Clear()
        lbVendorStyles.Items.Clear()
    End Sub

    Protected Sub btnResetISNs_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        SelectedISNs.Clear()
        lbISNs.Items.Clear()
    End Sub

#End Region

    Public Sub SetISNListBoxes()
        lbISNs.DataSource = SelectedISNs
        lbISNs.DataBind()
    End Sub

    Private Function IsValidISN(ByVal ISN As Decimal, ByVal IsReserve As Boolean) As Boolean
        Dim _TUEcommSetupCreateResults As New TUEcommSetupCreate
        Return _TUEcommSetupCreateResults.GetISNExists(ISN, IsReserve)
    End Function
    Protected Sub cmbDMM_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs)
        If cmbDMM.Items.Count = 0 Then
            With cmbDMM
                .DataSource = _TUDMM.GetAllDMM().ToList()
                .DataTextField = "DMMIDDesc"
                .DataValueField = "DMMId"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub

    Protected Sub cmbPOBuyer_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs)
        With cmbPOBuyer
            .DataSource = _TUBuyer.GetBuyersByDMM(CInt(If(String.IsNullOrEmpty(cmbDMM.SelectedValue), 0, CInt(cmbDMM.SelectedValue))))
            .DataTextField = "BuyerNameId"
            .DataValueField = "BuyerId"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbPOVendorStyle_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs)
        If cmbPOVendor.SelectedValue <> String.Empty OrElse e.Text.Trim() <> String.Empty Then
            With cmbPOVendorStyle
                .ClearSelection()
                .Enabled = True
                .DataTextField = "VendorStyleNumber"
                .DataValueField = "VendorStyleNumber"
                .DataSource = _TUVendorStyle.GetAllFromVendorStyle(CInt(cmbPODept.SelectedValue), cmbPOVendor.SelectedValue, cmbPOClass.SelectedValue, String.Empty, If(cmbPOVendor.SelectedValue = String.Empty, e.Text.Trim(), String.Empty))
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub

    Protected Sub cmbPOVendor_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
        cmbPOVendorStyle.SelectedValue = Nothing
        cmbPOVendorStyle.Text = String.Empty
        cmbPOVendorStyle.ClearSelection()

        If cmbPOVendor.SelectedValue <> String.Empty Then
            cmbPOVendorStyle.Enabled = True
        End If
        RaiseEvent cmbPOVendorSelectedIndexChanged(Me, New EventArgs())
    End Sub

    Protected Sub cmbPOVendor_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs)
        With cmbPOVendor
            .Enabled = True
            .DataTextField = "VendorIdName"
            .DataValueField = "VendorId"
            .DataSource = _TUVendor.GetAllFromVendorByDepartment(CInt(cmbPODept.SelectedValue))
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbPOClass_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
        cmbPOVendorStyle.ClearSelection()
        cmbPOVendorStyle.Text = String.Empty
        RaiseEvent cmbPOClassSelectedIndexChanged(Me, New EventArgs())
    End Sub

    Protected Sub cmbPOClass_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs)
        With cmbPOClass
            .Enabled = True
            .DataTextField = "ClassIdDesc"
            .DataValueField = "ClassId"
            .DataSource = _TUClass.GetAllFromClassByDepartment(CInt(cmbPODept.SelectedValue))
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbPODept_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs)
        With cmbPODept
            .DataSource = _TUDepartment.GetAllFromDepartment
            .DataTextField = "DeptIdDesc"
            .DataValueField = "DeptId"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbPODept_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
        cmbPOClass.ClearSelection()
        cmbPOClass.Text = String.Empty
        cmbPOVendor.ClearSelection()
        cmbPOVendor.Text = String.Empty
        cmbPOVendorStyle.ClearSelection()
        cmbPOVendorStyle.Text = String.Empty

        cmbPOClass.Enabled = False
        cmbPOVendor.Enabled = False
        cmbPOVendorStyle.Enabled = False

        SetDefaultShipDays()
        If cmbPODept.SelectedValue <> String.Empty Then
            cmbPOClass.Enabled = True
            cmbPOVendor.Enabled = True
            cmbPOVendorStyle.Enabled = True
        End If
        RaiseEvent cmbPODeptSelectedIndexChanged(Me, New EventArgs())
    End Sub

    Protected Sub btnPOResetVendorStyles_Click(sender As Object, e As ImageClickEventArgs)
        lbPOVendorStyles.Items.Clear()
        SelectedPOVendorStyles.Clear()
    End Sub

    Protected Sub cmbDMM_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
        cmbPOBuyer.ClearSelection()
        cmbPOBuyer.Text = String.Empty
        SetDefaultShipDays()
        RaiseEvent cmbDMMSelectedIndexChanged(Me, New EventArgs())
    End Sub

    ''' <summary>
    ''' Gets the default ship days for the fitlers mentioned and sets the date for Start Ship Date picker
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetDefaultShipDays()
        Dim defaultShipDays As Integer = 0
        Dim customizedShipDays As Integer = 0
        Dim vendorStyles As New List(Of String)

        customizedShipDays = _TUEcommSetupCreate.GetCFGDefaultShipDays(If(cmbDMM.SelectedValue = String.Empty, 0, CInt(cmbDMM.SelectedValue)),
                                                                    If(cmbPOBuyer.SelectedValue = String.Empty, 0, CInt(cmbPOBuyer.SelectedValue)),
                                                                    If(cmbPODept.SelectedValue = String.Empty, 0, CInt(cmbPODept.SelectedValue)),
                                                                    vendorStyles)
        If customizedShipDays > 0 Then
            defaultShipDays = customizedShipDays
        End If
        dpStartShipDate.SelectedDate = Date.Now().AddDays(defaultShipDays)
    End Sub

    Protected Sub cmbPOVendorStyle_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
        If cmbPOVendorStyle.SelectedValue <> String.Empty Then
            SetDefaultShipDays()
        End If
        RaiseEvent cmbPOVendorStyleSelectedIndexChanged(Me, New EventArgs())
    End Sub

    Protected Sub cmbPOBuyer_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
        SetDefaultShipDays()
        RaiseEvent cmbPOBuyerSelectedIndexChanged(Me, New EventArgs())
    End Sub

    Protected Sub lbVendorStyles_DataBinding(sender As Object, e As EventArgs)
        RaiseEvent lbVendorStylesDataBinding(Me, New EventArgs())
    End Sub

    Protected Sub dpCreatedSince_SelectedDateChanged(sender As Object, e As Calendar.SelectedDateChangedEventArgs)
        RaiseEvent dpCreatedSinceDateChanged(Me, New EventArgs())
    End Sub

    Protected Sub dpStartShipDate_SelectedDateChanged(sender As Object, e As Calendar.SelectedDateChangedEventArgs)
        RaiseEvent dpStartShipDateDateChanged(Me, New EventArgs())
    End Sub

    Protected Sub cblPriceStatusCodes_SelectedIndexChanged(sender As Object, e As EventArgs)
        RaiseEvent cblPriceStatusCodesSelectedIndexChanged(Me, New EventArgs())
    End Sub

    Protected Sub txtSku_TextChanged(sender As Object, e As EventArgs)
        RaiseEvent txtSkuTextChanged(Me, New EventArgs())
    End Sub
End Class