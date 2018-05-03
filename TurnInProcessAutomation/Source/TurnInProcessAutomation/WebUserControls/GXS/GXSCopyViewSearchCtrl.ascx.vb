Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL.Enumerations

Public Class GXSCopyViewSearchCtrl
    Inherits System.Web.UI.UserControl

    'Hierarchy
    Dim _TUDepartment As New TUDepartment
    Dim _TUClass As New TUClass
    Dim _TUVendor As New TUVendor
    Dim _TUVendorStyle As New TUVendorStyle
    Dim _TUBatch As New TUBatch

    'Ad/Page
    Private Const ItemsPerRequest As Integer = 10
    Dim _TUAdInfo As New TUAdInfo
    Dim _TUCtlgAdPg As New TUCtlgAdPg

    'Label
    Dim _TULabel As New TULabel



    'Hierarchy Controls
    Public ReadOnly Property cmbDept As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(0).Items(0).FindControl("cmbDept"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property cmbClass As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(0).Items(0).FindControl("cmbClass"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property cmbVendor As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(0).Items(0).FindControl("cmbVendor"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property cmbVendorStyle As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(0).Items(0).FindControl("cmbVendorStyle"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property dpCreatedSince As RadDatePicker
        Get
            Return DirectCast(rpbResultsTab.Items(0).Items(0).FindControl("dpCreatedSince"), RadDatePicker)
        End Get
    End Property

    'Item Controls
    Public ReadOnly Property txtISN As RadTextBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("txtISN"), RadTextBox)
        End Get
    End Property

    Public ReadOnly Property lbISNs As WebControls.ListBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("lbISNs"), WebControls.ListBox)
        End Get
    End Property

    Public ReadOnly Property txtSKU As RadTextBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("txtSKU"), RadTextBox)
        End Get
    End Property

    Public ReadOnly Property lbSKUs As WebControls.ListBox
        Get
            Return DirectCast(rpbResultsTab.Items(1).Items(0).FindControl("lbSKUs"), WebControls.ListBox)
        End Get
    End Property

    'Ad Controls
    Public ReadOnly Property cmbAds As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(2).Items(0).FindControl("cmbAds"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property cmbPageNumber As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(2).Items(0).FindControl("cmbPageNumber"), RadComboBox)
        End Get
    End Property

    Public ReadOnly Property cmbBatch As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(2).Items(0).FindControl("cmbBatch"), RadComboBox)
        End Get
    End Property

    'Label Controls
    Public ReadOnly Property cmbLabel As RadComboBox
        Get
            Return DirectCast(rpbResultsTab.Items(3).Items(0).FindControl("cmbLabel"), RadComboBox)
        End Get
    End Property

    'Start Ship Date Controls
    Public ReadOnly Property dpFrom As RadDatePicker
        Get
            Return DirectCast(rpbResultsTab.Items(4).Items(0).FindControl("dpFrom"), RadDatePicker)
        End Get
    End Property

    Public ReadOnly Property dpTo As RadDatePicker
        Get
            Return DirectCast(rpbResultsTab.Items(4).Items(0).FindControl("dpTo"), RadDatePicker)
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

    Public Property SelectedISNs As List(Of String)
        Get
            If Session("GXSCopyViewCtrl.SelectedISNs") Is Nothing Then
                Session("GXSCopyViewCtrl.SelectedISNs") = New List(Of String)
            End If
            Return CType(Session("GXSCopyViewCtrl.SelectedISNs"), List(Of String))
        End Get
        Set(value As List(Of String))
            Session("GXSCopyViewCtrl.SelectedISNs") = value
        End Set
    End Property

    Public Property SelectedSKUs As List(Of String)
        Get
            If Session("GXSCopyViewCtrl.SelectedSKUs") Is Nothing Then
                Session("GXSCopyViewCtrl.SelectedSKUs") = New List(Of String)
            End If
            Return CType(Session("GXSCopyViewCtrl.SelectedSKUs"), List(Of String))
        End Get
        Set(value As List(Of String))
            Session("GXSCopyViewCtrl.SelectedSKUs") = value
        End Set
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

    Public Function GetIsnXml() As String
        Dim sb As New StringBuilder("<ISN>")
        If SelectedISNs.Count > 0 Then
            For i As Integer = 0 To SelectedISNs.Count - 1
                sb.Append("<no num=""")
                sb.Append(SelectedISNs(i))
                sb.Append(""" />")
            Next
        Else
            sb.Append("<no />")
        End If
        sb.Append("</ISN>")
        Return sb.ToString
    End Function

    Public Function GetSkuUpcXml() As String
        Dim sb As New StringBuilder("<upc>")
        If SelectedSKUs.Count > 0 Then
            For i As Integer = 0 To SelectedSKUs.Count - 1
                sb.Append("<no num=""")
                sb.Append(SelectedSKUs(i))
                sb.Append(""" />")
            Next
        Else
            sb.Append("<no />")
        End If
        sb.Append("</upc>")
        Return sb.ToString
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterClientScriptInclude(Me, Me.GetType, "PasteFunctionality", Page.ResolveUrl("~/JavaScript/PasteFunctionality.js"))
        If Not Page.IsPostBack Then
            SelectedISNs.Clear()
            SelectedSKUs.Clear()
            BindrcbAds()
            GetLabelDS()
        End If
    End Sub

    'Hierarchy
    Protected Sub cmbDept_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbDept
            .DataSource = _TUDepartment.GetAllFromDepartment
            .DataTextField = "DeptIdDesc"
            .DataValueField = "DeptId"
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

    Protected Sub cmbVendorStyle_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        If cmbVendor.SelectedValue <> String.Empty OrElse e.Text.Trim() <> String.Empty Then
            With cmbVendorStyle
                .ClearSelection()
                .Enabled = True
                .DataTextField = "VendorStyleNumber"
                .DataValueField = "VendorStyleNumber"
                .DataSource = _TUVendorStyle.GetAllFromVendorStyle(CInt(cmbDept.SelectedValue), cmbVendor.SelectedValue, cmbClass.SelectedValue, "", If(cmbVendor.SelectedValue = String.Empty, e.Text.Trim(), String.Empty))
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub

    Protected Sub cmbDept_SelectedIndexChanged(ByVal sender As Object, ByVal e As RadComboBoxSelectedIndexChangedEventArgs)
        cmbClass.ClearSelection()
        cmbClass.Text = ""
        cmbVendor.ClearSelection()
        cmbVendor.Text = ""
        cmbVendorStyle.ClearSelection()
        cmbVendorStyle.Text = ""

        cmbClass.Enabled = False
        cmbVendor.Enabled = False
        cmbVendorStyle.Enabled = False

        If cmbDept.SelectedValue <> "" Then
            cmbClass.Enabled = True
            cmbVendor.Enabled = True
            cmbVendorStyle.Enabled = True
        End If
    End Sub

    Protected Sub cmbClass_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)
        'cmbSubClass.ClearSelection()
        'cmbSubClass.Text = ""
        'cmbSubClass.Enabled = False
        'cmbVendorStyle.ClearSelection()
        'cmbVendorStyle.Text = ""

        'If cmbClass.SelectedValue <> "" Then
        '    cmbSubClass.Enabled = True
        'End If
    End Sub

    Protected Sub cmbVendor_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)
        'cmbVendorStyle.SelectedValue = Nothing
        'cmbVendorStyle.Text = ""
        'cmbVendorStyle.ClearSelection()

        'If cmbVendor.SelectedValue <> "" Then
        '    cmbVendorStyle.Enabled = True
        '    SelectedVendorIds.Clear()
        '    If Me.SelectedVendorIds.Contains(Trim(cmbVendor.SelectedValue.ToString())) = False Then
        '        SelectedVendorIds.Add(Trim(cmbVendor.SelectedValue.ToString()))
        '    End If

        'End If

    End Sub

    Protected Sub btnResetVendorStyles_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        'SelectedVendorStyles.Clear()
        'lbVendorStyles.Items.Clear()
    End Sub

    'Items
    Protected Sub imgAddISN_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        If Page.IsValid Then
            Dim isns() As String = txtISN.Text.ToString.Split(CChar(","))
            For Each isn As String In isns
                SelectedISNs.Add(Trim(isn))
            Next
            SelectedISNs.Sort()
            lbISNs.DataSource = SelectedISNs
            lbISNs.DataBind()
            txtISN.Text = ""
        End If
    End Sub

    Protected Sub btnResetISNs_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        SelectedISNs.Clear()
        lbISNs.Items.Clear()
    End Sub

    Protected Sub imgAddSKU_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        If Page.IsValid Then
            Dim skus() As String = txtSKU.Text.ToString.Split(CChar(","))
            For Each sku As String In skus
                SelectedSKUs.Add(Trim(sku))
            Next
            SelectedSKUs.Sort()
            lbSKUs.DataSource = SelectedSKUs
            lbSKUs.DataBind()
            txtSKU.Text = ""
        End If
    End Sub

    Protected Sub btnResetSKUs_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        SelectedSKUs.Clear()
        lbSKUs.Items.Clear()
    End Sub

    'AdsDim _TUAdInfo As New TUAdInfo
    Protected Sub cmbAds_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        Dim _TUAdInfo As New TUAdInfo
        Dim items As List(Of AdInfoInfo)

        cmbAds.Items.Clear()
        If Session("GXSCopyViewSearchCtrl.AdList") IsNot Nothing Then
            items = CType(Session("GXSCopyViewSearchCtrl.AdList"), List(Of AdInfoInfo))
        Else
            items = _TUAdInfo.GetAllFromAdInfoFiltered(False).ToList()
            Session("GXSCopyViewSearchCtrl.AdList") = items
        End If

        If e.Text <> "" Then
            items = items.Where(Function(item) item.adnbr.ToString.Contains(e.Text)).ToList()
        End If

        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, items.Count())
        e.EndOfItems = endOffset = items.Count()

        cmbAds.DataSource = items.GetRange(itemOffset, endOffset - itemOffset)
        cmbAds.DataTextField = "adnbr"
        cmbAds.DataBind()
        cmbAds.ShowMoreResultsBox = Not e.EndOfItems

        'e.Message = GetStatusMessage(endOffset, items.Count())
    End Sub

    Protected Sub cmbAds_SelectedIndexChanged(ByVal sender As Object, ByVal e As RadComboBoxSelectedIndexChangedEventArgs)
        cmbPageNumber.ClearSelection()
        cmbPageNumber.Items.Clear()
        If cmbAds.Text <> "" Then
            With cmbPageNumber
                .Enabled = True
                .DataValueField = "PgNbr"
                .DataTextField = "PageNumberDesc"
                .DataSource = _TUCtlgAdPg.GetAllFromCtlgAdPg(CInt(cmbAds.Text))
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        Else
            cmbPageNumber.Enabled = False
        End If
    End Sub

    Private Sub BindrcbAds()
        If Request.QueryString.Count > 0 Then
            With cmbAds
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

    'Label
    Protected Sub cmbLabel_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbLabel
            .DataSource = Nothing
            .DataSource = LabelDS
            .DataTextField = "LabelDesc"
            .DataValueField = "LabelId"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Private Sub GetLabelDS()
        If LastLabelDate = Date.MinValue OrElse DateTime.Compare(LastLabelDate, Now.Date) < 0 Then
            LabelDS = _TULabel.GetAllLabels()
            LastLabelDate = Now.Date
        End If
    End Sub

    Public Sub ResetControls()
        Select Case SelectedPanel()
            Case "Find By Hierarchy"

                cmbDept.ClearSelection()
                cmbDept.Text = ""
                cmbClass.ClearSelection()
                cmbClass.Text = ""
                cmbVendor.ClearSelection()
                cmbVendor.Text = ""
                cmbVendorStyle.ClearSelection()
                cmbVendorStyle.Text = ""

            Case "Find By Item(s)"

                txtISN.Text = ""
                txtSKU.Text = ""
                lbISNs.ClearSelection()
                lbSKUs.ClearSelection()

            Case "Find By Ads"

                cmbAds.ClearSelection()
                cmbAds.Text = ""
                cmbPageNumber.ClearCheckedItems()
                cmbPageNumber.ClearSelection()
                cmbPageNumber.Text = ""
                cmbBatch.ClearSelection()
                cmbBatch.Text = ""

            Case "Find By Label"

                cmbLabel.ClearSelection()
                cmbLabel.Text = ""

            Case "Find By PO Start Ship Date"

                dpFrom.Clear()
                dpTo.Clear()

        End Select
    End Sub

End Class