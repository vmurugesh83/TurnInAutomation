Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL.Enumerations

Public Class CopyPrioritizationSearchCtrl
    Inherits System.Web.UI.UserControl

    'Hierarchy
    Dim _TUDepartment As New TUDepartment
    Dim _TUClass As New TUClass
    Dim _TUVendor As New TUVendor
    Dim _TUVendorStyle As New TUVendorStyle
    Dim _TUBatch As New TUBatch
    Dim _TUCopyPrioritization As New TUCopyPrioritization
    Dim _TUGMM As New TUGMM

    'Ad/Page
    Private Const ItemsPerRequest As Integer = 10
    Dim _TUAdInfo As New TUAdInfo
    Dim _TUCtlgAdPg As New TUCtlgAdPg

    'Label
    Dim _TULabel As New TULabel

    'Hierarchy Controls
    Public ReadOnly Property rddtWebCats As RadDropDownTree
        Get
            Return DirectCast(rpbResultsTab.Items(0).Items(0).FindControl("rddtWebCats"), RadDropDownTree)
        End Get
    End Property

    Public ReadOnly Property rddtCategory As RadDropDownTree
        Get
            Return DirectCast(rpbResultsTab.Items(0).Items(0).FindControl("rddtCategory"), RadDropDownTree)
        End Get
    End Property

    Public ReadOnly Property cmbGMM As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("cmbGMM"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property cmbDept As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("cmbDept"), RadComboBox)
        End Get
    End Property
    Public ReadOnly Property cmbVendorStyle As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("cmbVendorStyle"), RadComboBox)
        End Get
    End Property
    Public ReadOnly Property cmbAdNo As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("cmbAdNo"), RadComboBox)
        End Get
    End Property


    Public ReadOnly Property lbVendorStyles As WebControls.ListBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("lbVendorStyles"), WebControls.ListBox)
        End Get
    End Property

    Public ReadOnly Property btnResetVendorStyles As WebControls.ImageButton
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("btnResetVendorStyles"), WebControls.ImageButton)
        End Get
    End Property

    Public ReadOnly Property dpStartShipDate As RadDatePicker
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("dpStartShipDate"), RadDatePicker)
        End Get
    End Property
    Public ReadOnly Property hiddenVendorStyles As HiddenField
        Get
            Return hdnVendorStyles
        End Get
    End Property


    'Public ReadOnly Property dpDateMaintained As RadDatePicker
    '    Get
    '        Return DirectCast(rpbResultsTab.Items(0).Items(0).FindControl("dpDateMaintained"), RadDatePicker)
    '    End Get
    'End Property

    'Item Controls
    Public ReadOnly Property txtISN As RadTextBox
        Get
            Return DirectCast(rpbResultsTab.Items(2).Items(0).FindControl("txtISN"), RadTextBox)
        End Get
    End Property
    Public ReadOnly Property radLBISN As RadListBox
        Get
            Return DirectCast(rpbResultsTab.Items(2).Items(0).FindControl("radLBISN"), RadListBox)
        End Get
    End Property
    Public ReadOnly Property radLBSKU As RadListBox
        Get
            Return DirectCast(rpbResultsTab.Items(2).Items(0).FindControl("radLBSKU"), RadListBox)
        End Get
    End Property
    Public ReadOnly Property radLBImage As RadListBox
        Get
            Return DirectCast(rpbResultsTab.Items(2).Items(0).FindControl("radLBImage"), RadListBox)
        End Get
    End Property


    Public ReadOnly Property txtUPC As RadTextBox
        Get
            Return DirectCast(rpbResultsTab.Items(2).Items(0).FindControl("txtUPC"), RadTextBox)
        End Get
    End Property
    Public ReadOnly Property txtImage As RadTextBox
        Get
            Return DirectCast(rpbResultsTab.Items(2).Items(0).FindControl("txtImageID"), RadTextBox)
        End Get
    End Property
    Public ReadOnly Property chkbImageAvailable As WebControls.CheckBox
        Get
            Return chkImageAvailable
        End Get
    End Property
    Public ReadOnly Property chkbImageNotAvailable As WebControls.CheckBox
        Get
            Return chkImageNotAvailable
        End Get
    End Property
    Public ReadOnly Property chkbCopyReady As WebControls.CheckBox
        Get
            Return chkCopyReady
        End Get
    End Property
    Public ReadOnly Property chkbCopyNotReady As WebControls.CheckBox
        Get
            Return chkCopyNotReady
        End Get
    End Property
    Public ReadOnly Property chkbBasic As WebControls.CheckBox
        Get
            Return chkBasic
        End Get
    End Property
    Public ReadOnly Property chkbFashion As WebControls.CheckBox
        Get
            Return chkFashion
        End Get
    End Property
    Public ReadOnly Property chkbPWP As WebControls.CheckBox
        Get
            Return chkPWP
        End Get
    End Property
    Public ReadOnly Property chkbGWP As WebControls.CheckBox
        Get
            Return chkGWP
        End Get
    End Property
    Public ReadOnly Property chkbSpecial As WebControls.CheckBox
        Get
            Return chkSpecial
        End Get
    End Property
    Public ReadOnly Property chkbCollateral As WebControls.CheckBox
        Get
            Return chkCollateral
        End Get
    End Property
    Public ReadOnly Property chkbVirtualGC As WebControls.CheckBox
        Get
            Return chkVirtualGC
        End Get
    End Property
    Public ReadOnly Property chkbPlasticGC As WebControls.CheckBox
        Get
            Return chkPlasticGC
        End Get
    End Property
    Public ReadOnly Property chkbDropship As WebControls.CheckBox
        Get
            Return chkDropship
        End Get
    End Property
    Public ReadOnly Property chkbINFC As WebControls.CheckBox
        Get
            Return chkINFC
        End Get
    End Property
    Public ReadOnly Property chkbOnOrder As WebControls.CheckBox
        Get
            Return chkOnOrder
        End Get
    End Property
    Public ReadOnly Property chkbOnHand As WebControls.CheckBox
        Get
            Return chkOnHand
        End Get
    End Property

    Public ReadOnly Property SelectedPanel() As String
        Get
            Dim ret As Integer = 0
            For i As Integer = 0 To rpbResultsTab.Items.Count - 1
                If rpbResultsTab.Items(i).Expanded Then
                    ret = i
                    Exit For
                End If
            Next

            Return rpbResultsTab.Items(ret).Text
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
    Public ReadOnly Property CategoryRadioButton As RadioButton
        Get
            Return DirectCast(rpbResultsTab.Items(0).Items(0).FindControl("rdoCategory"), RadioButton)
        End Get
    End Property
    Public ReadOnly Property DeptRadioButton As RadioButton
        Get
            Return DirectCast(rpbResultsTab.Items(0).Items(0).FindControl("rdoDept"), RadioButton)
        End Get
    End Property
    Public Property LastLabelDate As Date
        Get
            Return CType(Application("GXSCopyViewSearchCtrl.LastLabelDate"), Date)
        End Get
        Set(value As Date)
            Application("GXSCopyViewSearchCtrl.LastLabelDate") = value
        End Set
    End Property

    Public Property LabelDS As IList(Of LabelInfo)
        Get
            Return CType(Application("GXSCopyViewSearchCtrl.LabelDS"), IList(Of LabelInfo))
        End Get
        Set(value As IList(Of LabelInfo))
            Application("GXSCopyViewSearchCtrl.LabelDS") = value
        End Set
    End Property
    Public ReadOnly Property PriceStatusCodes As List(Of String)
        Get
            Dim StatusCodes As New List(Of String)
            For Each cb As ListItem In cblPriceStatusCodes.Items
                If cb.Selected Then
                    StatusCodes.Add(cb.Value)
                End If
            Next
            Return StatusCodes
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterClientScriptInclude(Me, Me.GetType, "PasteFunctionality", Page.ResolveUrl("~/JavaScript/PasteFunctionality.js"))
        If Not Page.IsPostBack Then

            rddtCategory.DataFieldID = "CategoryCode"
            rddtCategory.DataFieldParentID = "ParentCategoryCode"
            rddtCategory.DataValueField = "CategoryCode"
            rddtCategory.DataTextField = "CategoryName"
            rddtCategory.DataSource = _TUCopyPrioritization.GetProductCategories(0)
            rddtCategory.EmbeddedTree.DataBindings.Add(New RadTreeNodeBinding() With { _
                .Depth = 0, _
                .ExpandMode = TreeNodeExpandMode.ClientSide _
            })
            rddtCategory.DataBind()
        Else
            Dim commaPosition As Integer = rddtCategory.SelectedValue.LastIndexOf(",")
            If commaPosition <> -1 Then
                rddtCategory.SelectedValue = rddtCategory.SelectedValue.Substring(commaPosition + 1)
            End If
        End If
    End Sub

    Protected Sub rddtCategory_Load(sender As Object, e As EventArgs)
        'Dim embeddedTree As RadTreeView = TryCast(sender, RadDropDownTree).EmbeddedTree
        'AddHandler embeddedTree.NodeExpand, AddressOf embeddedTree_NodeExpand
    End Sub

    Private Sub embeddedTree_NodeExpand(sender As Object, e As RadTreeNodeEventArgs)
        Dim expandedNode As RadTreeNode = e.Node
        Dim val As String = expandedNode.Value.ToString()
        Dim txt As String = expandedNode.Text.ToString()

        Dim subCategories As List(Of ProductCategoryInfo) = _TUCopyPrioritization.GetProductCategories(CInt(val))

        For Each category As ProductCategoryInfo In subCategories
            Dim newNode As RadTreeNode = New RadTreeNode(category.CategoryName, category.CategoryCode.ToString())
            newNode.Expanded = False
            newNode.ExpandMode = TreeNodeExpandMode.ServerSideCallBack
            expandedNode.Nodes.Add(newNode)
        Next
    End Sub

    'Hierarchy
    Protected Sub cmbDept_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        If Not String.IsNullOrEmpty(cmbGMM.SelectedValue.Trim()) Then
            With cmbDept
                .DataSource = _TUDepartment.GetAllDepartmentbyGMM(CInt(cmbGMM.SelectedValue))
                .DataTextField = "DeptIdDesc"
                .DataValueField = "DeptId"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub
    Protected Sub cmbVendorStyle_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        If cmbDept.SelectedValue <> String.Empty OrElse e.Text.Trim() <> String.Empty Then
            With cmbVendorStyle
                .ClearSelection()
                .Enabled = True
                .DataTextField = "VendorStyleNumber"
                .DataValueField = "VendorStyleNumber"
                .DataSource = _TUVendorStyle.GetAllFromVendorStyle(CInt(cmbDept.SelectedValue), String.Empty, String.Empty, String.Empty, e.Text.Trim())
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub

    Protected Sub cmbDept_SelectedIndexChanged(ByVal sender As Object, ByVal e As RadComboBoxSelectedIndexChangedEventArgs)
        cmbVendorStyle.ClearSelection()
        cmbVendorStyle.Text = ""
        cmbVendorStyle.Enabled = False

        If cmbDept.SelectedValue <> "" Then
            cmbVendorStyle.Enabled = True
        End If
    End Sub
    Protected Sub btnResetVendorStyles_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        lbVendorStyles.Items.Clear()
    End Sub

    ''Items
    'Protected Sub imgAddISN_Click(sender As Object, e As EventArgs)
    '    If Page.IsValid Then
    '        If txtISN.Text <> "" AndAlso lbISNs.Items.FindByValue(txtISN.Text.Trim()) Is Nothing Then
    '            lbISNs.Items.Add(txtISN.Text.Trim())
    '            txtISN.Text = String.Empty
    '        End If
    '    End If
    'End Sub

    'Private Sub GetLabelDS()
    '    If LastLabelDate = Date.MinValue OrElse DateTime.Compare(LastLabelDate, Now.Date) < 0 Then
    '        LabelDS = _TULabel.GetAllLabels()
    '        LastLabelDate = Now.Date
    '    End If
    'End Sub

    Public Sub ResetControls()
        Select Case SelectedPanel()
            Case "Find By Hierarchy"

                cmbDept.ClearSelection()
                cmbDept.Text = String.Empty
                cmbVendorStyle.ClearSelection()
                cmbVendorStyle.Text = String.Empty

            Case "Find By Item(s)"

                txtISN.Text = String.Empty
                txtUPC.Text = String.Empty
                txtImage.Text = String.Empty
                radLBISN.ClearSelection()
                radLBSKU.ClearSelection()
                radLBImage.ClearSelection()
        End Select
    End Sub

    Protected Sub imgAddVendorStyle_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        If cmbVendorStyle.SelectedValue <> "" AndAlso lbVendorStyles.Items.FindByValue(cmbVendorStyle.SelectedValue.Trim()) Is Nothing Then
            lbVendorStyles.Items.Add(cmbVendorStyle.SelectedValue.Trim())
            cmbVendorStyle.ClearSelection()
            cmbVendorStyle.SelectedValue = Nothing
            cmbVendorStyle.Text = ""
        End If
    End Sub
    'Public Sub ttvISN_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
    '    Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
    '    Dim value As String = DirectCast(validator.EvaluatedControl, RadTextBox).Text.Trim.ToUpper
    '    Dim isn As Decimal
    '    If Not txtISN Is Nothing Then
    '        If txtISN.Text = "" Then
    '            validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1006) 'required
    '            args.IsValid = False
    '        ElseIf Not lbISNs.Items.FindByText(txtISN.Text.Trim()) Is Nothing Then
    '            validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1007) 'You already added this ISN
    '            args.IsValid = False
    '        ElseIf Not Decimal.TryParse(Trim(txtISN.Text.Trim()), isn) Then
    '            validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1008) 'not valid
    '            args.IsValid = False
    '        ElseIf Not (IsValidISN(isn, False)) Then
    '            validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1008) 'not a real ISN
    '            args.IsValid = False
    '        End If
    '    Else
    '        args.IsValid = True
    '    End If
    'End Sub
    Private Function IsValidISN(ByVal ISN As Decimal, ByVal IsReserve As Boolean) As Boolean
        Dim _TUEcommSetupCreateResults As New TUEcommSetupCreate
        Return _TUEcommSetupCreateResults.GetISNExists(ISN, IsReserve)
    End Function

    Private Function IsValidUPC(ByVal UPC As Decimal) As Boolean
        Dim tuEcommFabOrig As New TUEcommFabOrig
        Return tuEcommFabOrig.GetUPCSKUExists(UPC)
    End Function

    'Protected Sub imgAddUPC_Click(sender As Object, e As ImageClickEventArgs)
    '    If Page.IsValid Then
    '        If txtUPC.Text <> "" AndAlso lbUPCs.Items.FindByValue(txtUPC.Text.Trim()) Is Nothing Then
    '            lbUPCs.Items.Add(txtUPC.Text.Trim())
    '            txtUPC.Text = String.Empty
    '        End If
    '    End If

    'End Sub

    'Protected Sub imgAddImage_Click(sender As Object, e As ImageClickEventArgs)
    '    If Page.IsValid Then
    '        If txtImage.Text <> "" AndAlso lbImageIDs.Items.FindByValue(txtImage.Text.Trim()) Is Nothing Then
    '            lbImageIDs.Items.Add(txtImage.Text.Trim())
    '            txtImage.Text = String.Empty
    '        End If
    '    End If

    'End Sub

    'Protected Sub ttvUPC_ServerValidate(source As Object, args As ServerValidateEventArgs)
    '    Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
    '    Dim value As String = DirectCast(validator.EvaluatedControl, RadTextBox).Text.Trim.ToUpper
    '    Dim upc As Decimal
    '    If Not txtUPC Is Nothing Then
    '        If txtUPC.Text = "" Then
    '            validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1018) 'required
    '            args.IsValid = False
    '        ElseIf Not lbUPCs.Items.FindByText(txtUPC.Text.Trim()) Is Nothing Then
    '            validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1019) 'You already added this UPC
    '            args.IsValid = False
    '        ElseIf Not Decimal.TryParse(Trim(txtUPC.Text.Trim()), upc) Then
    '            validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1020) 'not valid
    '            args.IsValid = False
    '        ElseIf Not (IsValidUPC(upc)) Then
    '            validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1020) 'not a real UPC
    '            args.IsValid = False
    '        End If
    '    Else
    '        args.IsValid = True
    '    End If
    'End Sub

    'Protected Sub ttvImage_ServerValidate(source As Object, args As ServerValidateEventArgs)
    '    Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
    '    Dim value As String = DirectCast(validator.EvaluatedControl, RadTextBox).Text.Trim.ToUpper
    '    Dim imageID As Decimal
    '    If Not txtImage Is Nothing Then
    '        If txtImage.Text = "" Then
    '            validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1021) 'required
    '            args.IsValid = False
    '        ElseIf Not lbImageIDs.Items.FindByText(txtImage.Text.Trim()) Is Nothing Then
    '            validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1022) 'You already added this Image
    '            args.IsValid = False
    '        ElseIf Not Decimal.TryParse(Trim(txtImage.Text.Trim()), imageID) Then
    '            validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1023) 'not valid
    '            args.IsValid = False
    '            'ElseIf Not (IsValidISN(imageID, False)) Then
    '            '    validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1023) 'not a real Image
    '            '    args.IsValid = False
    '        End If
    '    Else
    '        args.IsValid = True
    '    End If
    'End Sub

    'Protected Sub imgResetImage_Click(sender As Object, e As ImageClickEventArgs)
    '    lbImageIDs.Items.Clear()
    'End Sub

    'Protected Sub imgResetUPC_Click(sender As Object, e As ImageClickEventArgs)
    '    lbUPCs.Items.Clear()
    'End Sub

    'Protected Sub imgResetISN_Click(sender As Object, e As ImageClickEventArgs)
    '    lbISNs.Items.Clear()
    'End Sub

    Protected Sub cmbGMM_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs)
        If cmbGMM.Items.Count = 0 Then
            With cmbGMM
                .DataSource = _TUGMM.GetAllGMM().ToList()
                .DataTextField = "GMMIDDesc"
                .DataValueField = "GMMId"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub

    Protected Sub cmbGMM_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
        cmbDept.ClearSelection()
        cmbDept.Text = ""
        cmbDept.Enabled = False
        cmbVendorStyle.ClearSelection()
        cmbVendorStyle.Text = ""
        cmbVendorStyle.Enabled = False

        If cmbGMM.SelectedValue <> "" Then
            cmbDept.Enabled = True
        End If
    End Sub
    Protected Sub cmbAdNo_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        cmbAdNo.DataSource = Nothing
        cmbAdNo.DataSource = _TUAdInfo.GetAdsForQueryTool().ToList()
        cmbAdNo.DataValueField = "adnbr"
        cmbAdNo.DataTextField = "AdNumberDesc"
        cmbAdNo.DataBind()
    End Sub
    'Private Sub LoadGMM()
    '    If cmbGMM.Items.Count = 0 Then
    '        With cmbGMM
    '            .DataSource = _TUGMM.GetAllGMM().ToList()
    '            .DataTextField = "GMMIDDesc"
    '            .DataValueField = "GMMId"
    '            .DataBind()
    '            .Items.Insert(0, New RadComboBoxItem(""))
    '        End With
    '    End If
    'End Sub
End Class