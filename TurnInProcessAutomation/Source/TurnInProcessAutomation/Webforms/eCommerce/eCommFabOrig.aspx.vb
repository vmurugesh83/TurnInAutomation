Option Infer On
Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BLL.MiscConstants
Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL.Enumerations
Imports System.IO
Imports System.Net
Imports BonTon.Common.ExportFunctions
Imports Telerik.Web.UI.ExportInfrastructure

Public Class eCommFabOrig
    Inherits PageBase

    Private WithEvents _eCommFabOrigCtrl As eCommFabOrigCtrl = Nothing
    Private _TUEcommFabOrig As New TUEcommFabOrig
    Private _TUEcommSetupCreate As New TUEcommSetupCreate
    Private _gxsSourceCode As String = "GXS"
    Private _ssSourceCode As String = "SSKU"
    Private _ttuSourceCode As String = "TURNIN"
    Private upcList As List(Of FabOrig) = Nothing
    Private isExporting As Boolean = False
    Private _selectedISNs As List(Of String)

#Region "LHN Event Handlers"
    Protected Sub cmbClass_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.cmbClassSelectedIndexChanged
        If grdFabOrig.Items.Count <> 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DisableEditButton", "DisableEditButton();", True)
        End If
    End Sub

    Protected Sub cmbDept_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.cmbDeptSelectedIndexChanged
        If grdFabOrig.Items.Count <> 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DisableEditButton", "DisableEditButton();", True)
        End If
    End Sub

    Protected Sub cmbVendor_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.cmbVendorSelectedIndexChanged
        If grdFabOrig.Items.Count <> 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DisableEditButton", "DisableEditButton();", True)
        End If
    End Sub

    Protected Sub cmbVendorStyle_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.cmbVendorStyleSelectedIndexChanged
        If grdFabOrig.Items.Count <> 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DisableEditButton", "DisableEditButton();", True)
            EnableDisableButtons("Edit", False)
        End If
    End Sub

    Protected Sub cmbACode1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.cmbACode1SelectedIndexChanged
        If grdFabOrig.Items.Count <> 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DisableEditButton", "DisableEditButton();", True)
        End If
    End Sub

    Protected Sub cmbACode2_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.cmbACode2SelectedIndexChanged
        If grdFabOrig.Items.Count <> 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DisableEditButton", "DisableEditButton();", True)
        End If
    End Sub

    Protected Sub cmbACode3_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.cmbACode3SelectedIndexChanged
        If grdFabOrig.Items.Count <> 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DisableEditButton", "DisableEditButton();", True)
        End If
    End Sub

    Protected Sub cmbACode4_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.cmbACode4SelectedIndexChanged
        If grdFabOrig.Items.Count <> 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DisableEditButton", "DisableEditButton();", True)
        End If
    End Sub

    Protected Sub cmbSellYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.cmbSellYearSelectedIndexChanged
        If grdFabOrig.Items.Count <> 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DisableEditButton", "DisableEditButton();", True)
        End If
    End Sub

    Protected Sub cmbSellSeason_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.cmbSellSeasonSelectedIndexChanged
        If grdFabOrig.Items.Count <> 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DisableEditButton", "DisableEditButton();", True)
        End If
    End Sub

    Protected Sub lbVendorStyles_DataBinding(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.lbVendorStylesDataBinding
        If grdFabOrig.Items.Count <> 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DisableEditButton", "DisableEditButton();", True)
        End If
    End Sub

    Protected Sub imgAddVendorStyle_Click(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.imgAddVendorStyleClick
        If grdFabOrig.Items.Count <> 0 Then
            EnableDisableButtons("Edit", False)
        End If
    End Sub

    Protected Sub dpCreatedSince_SelectedDateChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.dpCreatedSinceDateChanged
        If grdFabOrig.Items.Count <> 0 Then
            EnableDisableButtons("Edit", False)
        End If
    End Sub

    Protected Sub cblPriceStatusCodes_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.cblPriceStatusCodesSelectedIndexChanged
        If grdFabOrig.Items.Count <> 0 Then
            EnableDisableButtons("Edit", False)
        End If
    End Sub

    Protected Sub cmbDMM_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.cmbDMMSelectedIndexChanged
        If grdFabOrig.Items.Count <> 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DisableEditButton", "DisableEditButton();", True)
        End If
    End Sub

    Protected Sub cmbPOBuyer_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.cmbPOBuyerSelectedIndexChanged
        If grdFabOrig.Items.Count <> 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DisableEditButton", "DisableEditButton();", True)
        End If
    End Sub

    Protected Sub cmbPODept_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.cmbPODeptSelectedIndexChanged
        If grdFabOrig.Items.Count <> 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DisableEditButton", "DisableEditButton();", True)
        End If
    End Sub

    Protected Sub cmbPOClass_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.cmbPOClassSelectedIndexChanged
        If grdFabOrig.Items.Count <> 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DisableEditButton", "DisableEditButton();", True)
        End If
    End Sub

    Protected Sub cmbPOVendor_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.cmbPOVendorSelectedIndexChanged
        If grdFabOrig.Items.Count <> 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DisableEditButton", "DisableEditButton();", True)
        End If
    End Sub

    Protected Sub cmbPOVendorStyle_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.cmbPOVendorStyleSelectedIndexChanged
        If grdFabOrig.Items.Count <> 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DisableEditButton", "DisableEditButton();", True)
            EnableDisableButtons("Edit", False)
        End If
    End Sub

    Protected Sub imgAddPOVendorStyle_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles _eCommFabOrigCtrl.imgAddPOVendorStyleClick
        If grdFabOrig.Items.Count <> 0 Then
            EnableDisableButtons("Edit", False)
        End If
    End Sub

    Protected Sub dpStartShipDate_SelectedDateChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.dpStartShipDateDateChanged
        If grdFabOrig.Items.Count <> 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DisableEditButton", "DisableEditButton();", True)
            EnableDisableButtons("Edit", False)
        End If
    End Sub

    Protected Sub imgAddISN_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles _eCommFabOrigCtrl.imgAddISNClick
        If grdFabOrig.Items.Count <> 0 Then
            EnableDisableButtons("Edit", False)
        End If
    End Sub

    Protected Sub txtSku_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _eCommFabOrigCtrl.txtSkuTextChanged
        If grdFabOrig.Items.Count <> 0 Then
            EnableDisableButtons("Edit", False)
        End If
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property eCommFabOrigCtrl() As eCommFabOrigCtrl
        Get
            Dim control As Control = Me.Master.SideBarPlaceHolder.FindControl("eCommFabOrigCtrl1")
            Me._eCommFabOrigCtrl = DirectCast(control, eCommFabOrigCtrl)
            Return _eCommFabOrigCtrl
        End Get
    End Property

    Public Property FabOrigISNs As List(Of EcommFabOrigInfo)
        Get
            If Session("FabOrigISNs") Is Nothing Then
                Session("FabOrigISNs") = New List(Of EcommFabOrigInfo)
            End If
            Return CType(Session("FabOrigISNs"), List(Of EcommFabOrigInfo))
        End Get
        Set(ByVal value As List(Of EcommFabOrigInfo))
            Session("FabOrigISNs") = value
        End Set
    End Property

    Public ReadOnly Property GxsSourceCode() As String
        Get
            Return _gxsSourceCode
        End Get
    End Property

    Public ReadOnly Property SSkuSourceCode() As String
        Get
            Return _ssSourceCode
        End Get
    End Property

    Public ReadOnly Property TtuSourceCode() As String
        Get
            Return _ttuSourceCode
        End Get
    End Property

    Public ReadOnly Property ExportText() As String
        Get
            Return Me.rblExport.SelectedValue
        End Get
    End Property

#End Region

#Region "Page Events"

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try

            Dim control As Control = LoadControl("~/WebUserControls/eCommerce/eCommFabOrigCtrl.ascx")
            If Not control Is Nothing Then
                control.ID = "eCommFabOrigCtrl1"
                Me.Master.SideBarPlaceHolder.Controls.Add(control)
            End If
            Me._eCommFabOrigCtrl = CType(control, eCommFabOrigCtrl)

        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Me.Master.SideBar.Width = 260
                If Request.QueryString("Action").ToUpper = Modes.MAINTENANCE.ToString _
                            Or Request.QueryString("Action").ToUpper = Modes.INQUIRY.ToString Then
                    SetupFabOrigTab()

                End If
                Session("EditRows") = False

                lblPageHeader.Text = lblPageHeader.Text & Request.QueryString("Action") & "<br/>"
            End If

        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub SetupFabOrigTab()

        rtsAPAdjustment.FindTabByText("Fabrication/Origination").Selected = True
        rmpFabricationOrigination.SelectedIndex = 0

        eCommFabOrigCtrl.ChangeMultiView("Fabrication/Origination")
        Me.Master.SideBar.Visible = True

        eCommFabOrigCtrl.ResultsTabRadPanelBar.Items(0).Text = "Find by PO Start Ship Date"
        eCommFabOrigCtrl.ResultsTabRadPanelBar.Items(1).Text = "Find by Hierarchy"
        eCommFabOrigCtrl.ResultsTabRadPanelBar.Items(2).Text = "Find by ISN"

        ShowHideButtons("Reset", True)

        ShowHideButtons("Retrieve", True)
        ShowHideButtons("Save", False)

        ShowHideButtons("Edit", False)
        ShowHideButtons("Cancel", False)

        grdFabOrig.Visible = False
        pnlFlood.Visible = False
        pnlGrid.Visible = False

    End Sub

#End Region

#Region "Button Events"

    Private Sub rtbFabricationOrigination_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles rtbFabricationOrigination.ButtonClick
        Try
            If TypeOf e.Item Is RadToolBarButton Then
                Dim radToolBarButton As RadToolBarButton = DirectCast(e.Item, RadToolBarButton)
                Dim rpbResultsTab As RadPanelBar = DirectCast(eCommFabOrigCtrl.FindControl("rpbResultsTab"), RadPanelBar)

                ClearMessagePanel()

                Select Case radToolBarButton.CommandName
                    Case "Retrieve"
                        Session("EditRows") = False
                        RetrieveFabOrigResultList(False)
                        grdFabOrig.Rebind()
                        If Request.QueryString("Action").ToUpper = Modes.INQUIRY.ToString Then
                            rtbFabricationOrigination.FindItemByText("Edit").Visible = False
                            rtbFabricationOrigination.FindItemByText("Save").Visible = False
                            rtbFabricationOrigination.FindItemByText("Cancel").Visible = False
                            pnlFlood.Visible = False
                            rtxtFloodFabrication.Visible = False
                            rtxtFloodOrigination.Visible = False
                        End If
                        grdFabOrig.ClientSettings.Selecting.AllowRowSelect = True
                        grdFabOrig.MasterTableView.AlternatingItemStyle.BackColor = Drawing.Color.LightGray
                        grdFabOrig.AllowSorting = True
                        grdFabOrig.CurrentPageIndex = 0

                    Case "Reset"
                        Session("EditRows") = False
                        Me.Master.SideBar.Collapsed = False
                        ResetFabOrigSearch()
                        ClearFlood()
                        Response.Redirect(Request.Url.ToString(), False)
                        grdFabOrig.ClientSettings.Selecting.AllowRowSelect = True
                        grdFabOrig.AllowSorting = True
                        ShowHideButtons("Edit", False)
                        pnlFlood.Visible = False

                    Case "Edit"
                        EditModeOnOff("On")
                        Session("EditRows") = True
                        PutRowsInEditMode(grdFabOrig, True)
                        RetrieveFabOrigResultList(True)
                        For Each g As GridEditableItem In grdFabOrig.Items
                            g.Selected = True
                        Next
                        PutRowsInEditMode(grdFabOrig, True)
                        ShowHideButtons("Retrieve", False)
                        grdFabOrig.AllowSorting = False
                        pnlFlood.Visible = True
                        SetFabOrigPageIndex(grdFabOrig.CurrentPageIndex)

                    Case "Save"
                        Session("EditRows") = False
                        If SaveRows(grdFabOrig) Then
                            PutRowsInEditMode(grdFabOrig, False)
                            EditModeOnOff("Off")
                            ClearFlood()
                        End If
                        grdFabOrig.ClientSettings.Selecting.AllowRowSelect = True
                        grdFabOrig.AllowSorting = True
                        pnlFlood.Visible = False
                        GetFabOrigPageIndex()
                        ShowHideButtons("Retrieve", True)

                    Case "CancelAll"
                        Session("EditRows") = False
                        RetrieveFabOrigResultList(False)
                        grdFabOrig.Rebind()
                        PutRowsInEditMode(grdFabOrig, False)
                        EditModeOnOff("Off")
                        ClearFlood()
                        grdFabOrig.ClientSettings.Selecting.AllowRowSelect = True
                        ShowHideButtons("Retrieve", True)
                        grdFabOrig.AllowSorting = True
                        pnlFlood.Visible = False
                        GetFabOrigPageIndex()

                End Select
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    ''' <summary>
    ''' Handles the FLOOD option.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnFlood_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFlood.Click
        Dim strFabrication As String = String.Empty
        Dim strOrigination As String = String.Empty
        Dim ColorFamilyFloodErrorList As New List(Of String)

        Try
            Page.Validate("FloodUpdate")

            If Not IsValid Then
                mpFabricationOrigination.ErrorMessage = "Errors on Page."
                Exit Sub
            End If

            'Exit the Sub-routine if no data is entered for "Flood" option.
            If String.IsNullOrEmpty(rtxtFloodFabrication.Text.Trim) And _
                    String.IsNullOrEmpty(rtxtFloodOrigination.Text.Trim) Then
                mpFabricationOrigination.PopUpMessage = "Enter value for at least one Flood option."
                Exit Sub
            End If

            Dim rowCount As Integer = grdFabOrig.MasterTableView.GetItems(GridItemType.EditItem).Count

            If rowCount > 0 Then

                Session("Flood") = True
                Session("FloodFabrication") = rtxtFloodFabrication.Text.Trim
                Session("FloodOrigination") = rtxtFloodOrigination.Text.Trim

                'Reset the Flood values.
                btnResetFlood_Click(Nothing, Nothing)

            Else
                mpFabricationOrigination.PopUpMessage = "Select at least one record to Flood."
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    ''' <summary>
    ''' Clear the User Entered Flood values.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnResetFlood_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnResetFlood.Click
        rtxtFloodFabrication.Text = ""
        rtxtFloodOrigination.Text = ""
    End Sub

#End Region

#Region "Methods"

    Private Sub ClearFlood()
        Session("Flood") = False
        Session("FloodFabrication") = ""
        Session("FloodOrigination") = ""
    End Sub

    Private Function SaveRows(ByRef rg As RadGrid) As Boolean

        For Each item As GridItem In rg.MasterTableView.Items
            If TypeOf item Is GridEditableItem And item.IsInEditMode Then
                If Not grdFabOrigUpdateRow(CType(item, GridEditableItem)) Then
                    Return False
                End If
            End If
        Next
        mpFabricationOrigination.ErrorMessage = "Data Saved Successfully."
        RetrieveFabOrigResultList(False)
        grdFabOrig.Rebind()
        Return True

    End Function

    Private Function grdFabOrigUpdateRow(editItem As GridEditableItem) As Boolean

        Dim isn As Decimal = 0
        Dim labelId As Integer = 0
        Dim fabricationDesc As String = String.Empty
        Dim origDesc As String = String.Empty
        Dim fabSrce As String = String.Empty
        Dim origSrce As String = String.Empty

        Try
            Page.Validate("Update")

            If Not IsValid Then
                mpFabricationOrigination.PopUpMessage = "Errors on Page."
                Return False
            End If

            If editItem.ItemIndex > -1 Then
                'Get all the required values from the Grid Row being Updated.
                Dim txtFab As TextBox = DirectCast(editItem("Fabrication").Controls(0), TextBox)
                Dim txtOrig As TextBox = DirectCast(editItem("Origination").Controls(0), TextBox)

                upcList = DirectCast(Session("UPCList"), List(Of FabOrig))

                isn = CDec(editItem.GetDataKeyValue("ISN"))

                Dim fabOrig As FabOrig = upcList.FirstOrDefault(Function(q) q.ISN = isn)

                fabricationDesc = txtFab.Text.Trim

                origDesc = txtOrig.Text.Trim

                Dim fabSourceCell As TableCell = editItem("FabSource")
                Dim origSourceCell As TableCell = editItem("OrigSource")

                If Not String.IsNullOrEmpty(WebUtility.HtmlDecode(fabSourceCell.Text).Trim) Then
                    fabOrig.FabricationOriginalSource = fabSourceCell.Text.Trim
                End If



                If Not String.IsNullOrEmpty(WebUtility.HtmlDecode(origSourceCell.Text).Trim) Then
                    fabOrig.OriginationOriginalSource = origSourceCell.Text.Trim
                End If

                'User has possibly entered new values
                If txtFab.Text.Trim <> fabOrig.FabricationOriginalValue.Trim Then
                    fabOrig.FabricationOriginalSource = TtuSourceCode
                End If

                If txtOrig.Text.Trim <> fabOrig.OriginationOriginalValue.Trim Then
                    fabOrig.OriginationOriginalSource = TtuSourceCode
                End If

                labelId = fabOrig.LabelId

                _TUEcommFabOrig.UpdateFabOrigByISN(isn, labelId, fabricationDesc, origDesc, fabOrig.FabricationOriginalSource, fabOrig.OriginationOriginalSource, SessionWrapper.UserID)

                mpFabricationOrigination.ErrorMessage = "Data Saved Successfully."
                Return True
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Function

    Private Sub ClearMessagePanel()
        mpFabricationOrigination.ErrorMessage = ""
    End Sub

    Private Sub ResetComboBox(ByVal rcbComboBox As RadComboBox, ByVal isEnabled As Boolean)
        rcbComboBox.ClearSelection()
        rcbComboBox.Text = ""
        rcbComboBox.Enabled = isEnabled
    End Sub

    ''' <summary>
    ''' Gets the default ship days for the fitlers mentioned and sets the date for Start Ship Date picker
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetDefaultShipDays()
        Dim defaultShipDays As Integer = 0
        Dim vendorStyles As New List(Of String)

        For Each item In eCommFabOrigCtrl.lbPOVendorStyles.Items
            vendorStyles.Add(CType(item, ListItem).Text.Trim())
        Next

        If eCommFabOrigCtrl.cmbPOVendorStyle.SelectedValue <> String.Empty AndAlso Not vendorStyles.Contains(eCommFabOrigCtrl.cmbPOVendorStyle.SelectedValue.Trim()) Then
            vendorStyles.Add(eCommFabOrigCtrl.cmbPOVendorStyle.SelectedValue)
        End If

        defaultShipDays = _TUEcommSetupCreate.GetCFGDefaultShipDays(If(eCommFabOrigCtrl.cmbDMM.SelectedValue = String.Empty, 0, CInt(eCommFabOrigCtrl.cmbDMM.SelectedValue)),
                                                                    If(eCommFabOrigCtrl.cmbPOBuyer.SelectedValue = String.Empty, 0, CInt(eCommFabOrigCtrl.cmbPOBuyer.SelectedValue)),
                                                                    If(eCommFabOrigCtrl.cmbPODept.SelectedValue = String.Empty, 0, CInt(eCommFabOrigCtrl.cmbPODept.SelectedValue)),
                                                                    vendorStyles)
        If defaultShipDays > 0 Then
            eCommFabOrigCtrl.dpStartShipDate.SelectedDate = Date.Now().AddDays(defaultShipDays)
        End If
    End Sub

    Private Sub PutRowsInEditMode(ByRef rg As RadGrid, ByVal isEdit As Boolean)
        Try
            Dim upc As Long? = 0
            Dim isn As Integer = 0
            Dim labelId As Integer = 0
            Dim fabOrig As FabOrig = Nothing
            Dim decodeString As String = String.Empty

            upcList = New List(Of FabOrig)

            Select Case rg.SelectedItems.Count

                Case Is > 0
                    For Each item As GridEditableItem In rg.SelectedItems

                        If Not IsNothing(item) Then
                            If TypeOf item Is GridEditableItem Then

                                If isEdit Then

                                    If Not String.IsNullOrEmpty(WebUtility.HtmlDecode(item("UPC").Text.ToString()).Trim()) Then
                                        upc = CLng(item("UPC").Text.ToString().Trim)
                                    End If

                                    If Not String.IsNullOrEmpty(WebUtility.HtmlDecode(item("ISN").Text.ToString()).Trim()) Then
                                        isn = CInt(item("ISN").Text.ToString().Trim)
                                    End If

                                    If Not String.IsNullOrEmpty(WebUtility.HtmlDecode(item("LabelId").Text.ToString()).Trim()) Then
                                        labelId = CInt(item("LabelId").Text.ToString().Trim)
                                    End If

                                    fabOrig = New FabOrig()
                                    fabOrig.ISN = isn
                                    fabOrig.UPC = upc
                                    fabOrig.LabelId = labelId

                                    item.Edit = isEdit

                                    If String.IsNullOrEmpty(WebUtility.HtmlDecode(item("Fabrication").Text.ToString()).Trim()) Then
                                        fabOrig.NeedsFabInfo = True
                                    End If
                                    If String.IsNullOrEmpty(WebUtility.HtmlDecode(item("Origination").Text.ToString()).Trim()) Then
                                        fabOrig.NeedsOrigInfo = True
                                    End If
                                    If Not String.IsNullOrEmpty(WebUtility.HtmlDecode(item("StyleSkuFabDescription").Text.ToString()).Trim()) Then
                                        fabOrig.StyleSkuFabDescription = item("StyleSkuFabDescription").Text.ToString().Trim
                                    End If
                                    If Not String.IsNullOrEmpty(WebUtility.HtmlDecode(item("StyleSkuOrigDescription").Text.ToString()).Trim()) Then
                                        fabOrig.StyleSkuOrigDescription = item("StyleSkuOrigDescription").Text.ToString().Trim
                                    End If
                                    Dim fabSourceCell As TableCell = item("FabSource")
                                    fabOrig.FabricationOriginalSource = WebUtility.HtmlDecode(fabSourceCell.Text).Trim()

                                    Dim origSourceCell As TableCell = item("OrigSource")
                                    fabOrig.OriginationOriginalSource = WebUtility.HtmlDecode(origSourceCell.Text).Trim()

                                    item.Selected = True
                                    upcList.Add(fabOrig)
                                Else
                                    item.Edit = isEdit
                                End If
                            End If
                        End If
                    Next
                    Session("UPCList") = upcList
                Case 0
                    For Each item As GridEditableItem In rg.Items

                        If Not IsNothing(item) Then
                            If TypeOf item Is GridEditableItem Then
                                If isEdit Then

                                    If Not String.IsNullOrEmpty(WebUtility.HtmlDecode(item("UPC").Text.ToString()).Trim()) Then
                                        upc = CLng(item("UPC").Text.ToString().Trim)
                                    End If

                                    If Not String.IsNullOrEmpty(WebUtility.HtmlDecode(item("ISN").Text.ToString()).Trim()) Then
                                        isn = CInt(item("ISN").Text.ToString().Trim)
                                    End If

                                    If Not String.IsNullOrEmpty(WebUtility.HtmlDecode(item("LabelId").Text.ToString()).Trim()) Then
                                        labelId = CInt(item("LabelId").Text.ToString().Trim)
                                    End If

                                    fabOrig = New FabOrig()
                                    fabOrig.ISN = isn
                                    fabOrig.UPC = upc
                                    fabOrig.LabelId = labelId

                                    item.Edit = isEdit

                                    If String.IsNullOrEmpty(WebUtility.HtmlDecode(item("Fabrication").Text.ToString()).Trim()) Then
                                        fabOrig.NeedsFabInfo = True
                                    End If
                                    If String.IsNullOrEmpty(WebUtility.HtmlDecode(item("Origination").Text.ToString()).Trim()) Then
                                        fabOrig.NeedsOrigInfo = True
                                    End If
                                    If Not String.IsNullOrEmpty(WebUtility.HtmlDecode(item("StyleSkuFabDescription").Text.ToString()).Trim()) Then
                                        fabOrig.StyleSkuFabDescription = item("StyleSkuFabDescription").Text.ToString().Trim
                                    End If
                                    If Not String.IsNullOrEmpty(WebUtility.HtmlDecode(item("StyleSkuOrigDescription").Text.ToString()).Trim()) Then
                                        fabOrig.StyleSkuOrigDescription = item("StyleSkuOrigDescription").Text.ToString().Trim
                                    End If
                                    Dim fabSourceCell As TableCell = item("FabSource")
                                    fabOrig.FabricationOriginalSource = WebUtility.HtmlDecode(fabSourceCell.Text).Trim()

                                    Dim origSourceCell As TableCell = item("OrigSource")
                                    fabOrig.OriginationOriginalSource = WebUtility.HtmlDecode(origSourceCell.Text).Trim()

                                    item.Selected = True
                                    upcList.Add(fabOrig)
                                Else
                                    item.Edit = isEdit
                                End If
                            End If
                        End If
                    Next
                    Session("UPCList") = upcList
            End Select
            grdFabOrig.ClientSettings.Selecting.AllowRowSelect = False
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

#End Region

#Region "Get Data"

    Private Sub RetrieveFabOrigResultList(ByVal getEdited As Boolean)
        Dim SelectedVendorStyles As New List(Of String)
        Dim SelectedPOVendorStyles As New List(Of String)
        Dim SelISNList As New List(Of String)
        Dim SelReserveISNList As New List(Of String)
        Dim IsValid As Boolean = True
        Dim IsUpcValid As Boolean = True
        Dim turnInFilter As TUFilter = Nothing
        Dim departmentID As Int16 = 0
        Dim vendorID As Integer = 0
        Dim dmmID As Integer = 0
        Dim buyerID As Integer = 0
        Dim includeOnlyApprovedItems As Boolean = False
        Dim Upc As String = String.Empty
        FabOrigISNs = Nothing

        If getEdited Then
            upcList = DirectCast(Session("UPCList"), List(Of FabOrig))
            Dim isnList As List(Of String) = Nothing

            If Not IsNothing(upcList) AndAlso upcList.Count > 0 Then
                isnList = New List(Of String)
                For Each editRecord As FabOrig In upcList
                    isnList.Add(editRecord.ISN.ToString())
                Next

            End If

            If Not IsNothing(isnList) AndAlso isnList.Count > 0 Then
                turnInFilter = GetTurnInFilters("ISN")
                FabOrigISNs = _TUEcommFabOrig.GetAllEcommFabOrigResultsByISNs(isnList, Upc, SelReserveISNList.Distinct.ToList, turnInFilter).OrderBy(Function(m) m.ISN).ToList
                grdFabOrig.Visible = True
                pnlFlood.Visible = True
                pnlGrid.Visible = True
                Exit Sub
            End If
            Exit Sub
        End If

        If eCommFabOrigCtrl.ResultsTabRadPanelBar.Items(0).Expanded Then

            If eCommFabOrigCtrl.cmbDMM.SelectedValue = String.Empty And eCommFabOrigCtrl.cmbPOBuyer.SelectedValue = String.Empty And eCommFabOrigCtrl.cmbPODept.SelectedValue = String.Empty Then
                mpFabricationOrigination.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1016, False)
                grdFabOrig.Visible = False
                pnlGrid.Visible = False

            Else
                For Each item In eCommFabOrigCtrl.lbPOVendorStyles.Items
                    SelectedPOVendorStyles.Add(CType(item, ListItem).Text.Trim())
                Next

                If eCommFabOrigCtrl.cmbDMM.SelectedValue <> String.Empty Then
                    dmmID = CInt(eCommFabOrigCtrl.cmbDMM.SelectedValue)
                End If

                If eCommFabOrigCtrl.cmbPOBuyer.SelectedValue <> String.Empty Then
                    buyerID = CInt(eCommFabOrigCtrl.cmbPOBuyer.SelectedValue)
                End If

                If Not SelectedPOVendorStyles Is Nothing AndAlso SelectedPOVendorStyles.Count = 0 Then
                    vendorID = CInt(If(eCommFabOrigCtrl.cmbPOVendor.SelectedValue = String.Empty, 0, CInt(eCommFabOrigCtrl.cmbPOVendor.SelectedValue)))
                    departmentID = CShort(CInt(If(eCommFabOrigCtrl.cmbPODept.SelectedValue = String.Empty, 0, CInt(eCommFabOrigCtrl.cmbPODept.SelectedValue))))
                End If

                If eCommFabOrigCtrl.cmbPOVendorStyle.SelectedValue <> String.Empty AndAlso Not SelectedPOVendorStyles.Contains(eCommFabOrigCtrl.cmbPOVendorStyle.SelectedValue.Trim()) Then
                    SelectedPOVendorStyles.Add(eCommFabOrigCtrl.cmbPOVendorStyle.SelectedValue.Trim)
                End If

                turnInFilter = GetTurnInFilters("PO")

                includeOnlyApprovedItems = turnInFilter.AvailableForTurnIn AndAlso Not turnInFilter.NotAvailableForTurnIn

                FabOrigISNs = _TUEcommFabOrig.GetAllEcommFabOrigResultsByPOShipDate(dmmID, buyerID, departmentID, _
                                                                                              CShort(If(eCommFabOrigCtrl.cmbPOClass.SelectedValue = String.Empty, 0, CShort(eCommFabOrigCtrl.cmbPOClass.SelectedValue))), _
                                                                                              vendorID, SelectedPOVendorStyles.Distinct.ToList, CDate(eCommFabOrigCtrl.dpStartShipDate.SelectedDate), includeOnlyApprovedItems).ToList()



                If eCommFabOrigCtrl.cmbVendorStyle.SelectedValue <> "" Then
                    SelectedPOVendorStyles.Remove(eCommFabOrigCtrl.cmbVendorStyle.SelectedValue)
                End If

                grdFabOrig.Visible = True
            End If

        ElseIf eCommFabOrigCtrl.ResultsTabRadPanelBar.Items(1).Expanded Then
            ' Department is required.
            If eCommFabOrigCtrl.SelectedDepartmentId = 0 Then
                mpFabricationOrigination.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1010)
                grdFabOrig.Visible = False
                pnlGrid.Visible = False
            Else

                For Each item In eCommFabOrigCtrl.lbVendorStyles.Items
                    SelectedVendorStyles.Add(CType(item, ListItem).Text.Trim())
                Next

                If Not SelectedVendorStyles Is Nothing AndAlso SelectedVendorStyles.Count = 0 Then
                    vendorID = eCommFabOrigCtrl.SelectedVendorId
                    departmentID = eCommFabOrigCtrl.SelectedDepartmentId
                End If

                If eCommFabOrigCtrl.cmbVendorStyle.SelectedValue <> "" AndAlso Not SelectedVendorStyles.Contains(eCommFabOrigCtrl.cmbVendorStyle.SelectedValue.Trim()) Then
                    SelectedVendorStyles.Add(eCommFabOrigCtrl.cmbVendorStyle.SelectedValue.Trim)
                End If

                turnInFilter = GetTurnInFilters("Hierarchy")

                FabOrigISNs = _TUEcommFabOrig.GetAllEcommFabOrigResultsByHeirarchy(eCommFabOrigCtrl.PriceStatusCodes, _
                                                                              departmentID, eCommFabOrigCtrl.SelectedClassId, _
                                                                              eCommFabOrigCtrl.SelectedSubClassId, vendorID, _
                                                                              SelectedVendorStyles.Distinct.ToList, eCommFabOrigCtrl.SelectedACode1, _
                                                                              eCommFabOrigCtrl.SelectedACode2, eCommFabOrigCtrl.SelectedACode3, _
                                                                              eCommFabOrigCtrl.SelectedACode4, eCommFabOrigCtrl.SelectedYear, _
                                                                              eCommFabOrigCtrl.SelectedSeasonId, eCommFabOrigCtrl.SelectedCreatedSince, turnInFilter).ToList()

                If eCommFabOrigCtrl.cmbVendorStyle.SelectedValue <> "" Then
                    SelectedVendorStyles.Remove(eCommFabOrigCtrl.cmbVendorStyle.SelectedValue)
                End If

                grdFabOrig.Visible = True
            End If
        Else
            If eCommFabOrigCtrl.SelectedISNs.Count = 0 _
                And eCommFabOrigCtrl.txtISN.Text.Trim = "" _
                AndAlso eCommFabOrigCtrl.txtSku.Text.Trim = "" Then

                mpFabricationOrigination.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1009)
                grdFabOrig.Visible = False
                pnlGrid.Visible = False
            Else
                If eCommFabOrigCtrl.SelectedISNs.Count > 0 Then
                    SelISNList = eCommFabOrigCtrl.SelectedISNs
                End If

                If eCommFabOrigCtrl.txtISN.Text.Trim <> "" Then
                    If ValidateISN(eCommFabOrigCtrl.txtISN.Text.Trim, False) Then
                        SelISNList.Add(eCommFabOrigCtrl.txtISN.Text.Trim)
                    Else
                        IsValid = False
                    End If
                End If

                If eCommFabOrigCtrl.txtSku.Text.Trim <> "" Then
                    If ValidateUPCSKU(eCommFabOrigCtrl.txtSku.Text.Trim) Then
                        Upc = eCommFabOrigCtrl.txtSku.Text.Trim
                    Else
                        IsUpcValid = False
                    End If
                End If

                If IsValid AndAlso IsUpcValid Then
                    turnInFilter = GetTurnInFilters("ISN")
                    FabOrigISNs = _TUEcommFabOrig.GetAllEcommFabOrigResultsByISNs(SelISNList.Distinct.ToList, Upc, SelReserveISNList.Distinct.ToList, turnInFilter).ToList
                    grdFabOrig.Visible = True
                    pnlGrid.Visible = True
                Else
                    If Not IsValid Then
                        mpFabricationOrigination.ErrorMessage = "Invalid ISN or Reserve ISN."
                        grdFabOrig.Visible = False
                        pnlGrid.Visible = False
                    End If
                    If Not IsUpcValid Then
                        mpFabricationOrigination.ErrorMessage = "Invalid UPC or SKU."
                        grdFabOrig.Visible = False
                        pnlGrid.Visible = False
                    End If

                End If

                SelISNList.Remove(eCommFabOrigCtrl.txtISN.Text.Trim)

            End If
        End If

        If FabOrigISNs.Count > 0 Then
            ShowHideButtons("Edit", True)
            EnableDisableButtons("Edit", True)
            pnlGrid.Visible = True
        Else
            ShowHideButtons("Edit", False)
            EnableDisableButtons("Edit", False)
            pnlGrid.Visible = True
        End If

    End Sub

#End Region

#Region "Search Reset"

    Private Sub ResetFabOrigSearch()
        If eCommFabOrigCtrl.ResultsTabRadPanelBar.Items(0).Expanded Then
            If Not IsNothing(eCommFabOrigCtrl.cblPriceStatus) Then
                eCommFabOrigCtrl.cblPriceStatus.ClearSelection()
                eCommFabOrigCtrl.cblPriceStatus.Items(0).Selected = True
                eCommFabOrigCtrl.cblPriceStatus.Items(1).Selected = True
            End If

            ResetComboBox(eCommFabOrigCtrl.cmbDept, True)
            ResetComboBox(eCommFabOrigCtrl.cmbClass, False)
            ResetComboBox(eCommFabOrigCtrl.cmbSubClass, False)
            ResetComboBox(eCommFabOrigCtrl.cmbVendor, False)
            ResetComboBox(eCommFabOrigCtrl.cmbVendorStyle, False)
            eCommFabOrigCtrl.lbVendorStyles.Items.Clear()
            ResetComboBox(eCommFabOrigCtrl.cmbACode1, False)
            ResetComboBox(eCommFabOrigCtrl.cmbACode2, False)
            ResetComboBox(eCommFabOrigCtrl.cmbACode3, False)
            ResetComboBox(eCommFabOrigCtrl.cmbACode4, False)
            ResetComboBox(eCommFabOrigCtrl.cmbSellYear, False)
            ResetComboBox(eCommFabOrigCtrl.cmbSellSeason, False)
            eCommFabOrigCtrl.dpCreatedSince.Clear()

        ElseIf eCommFabOrigCtrl.ResultsTabRadPanelBar.Items(1).Expanded Then
            eCommFabOrigCtrl.txtISN.Text = ""
            eCommFabOrigCtrl.lbISNs.Items.Clear()

        End If

    End Sub

#End Region

#Region "Show/Hide Page Elements"

    Private Sub ShowHideButtons(ByVal ButtonName As String, ByVal Show As Boolean)
        rtbFabricationOrigination.FindItemByText(ButtonName).Visible = Show
    End Sub

    Private Sub EnableDisableButtons(ByVal ButtonName As String, ByVal Show As Boolean)
        rtbFabricationOrigination.FindItemByText(ButtonName).Enabled = Show
    End Sub

    Public Sub EditModeOnOff(ByVal OnOff As String)
        If OnOff = "Off" Then
            rtbFabricationOrigination.FindItemByText("Edit").Visible = True
            rtbFabricationOrigination.FindItemByText("Save").Visible = False
            rtbFabricationOrigination.FindItemByText("Cancel").Visible = False
        Else
            rtbFabricationOrigination.FindItemByText("Edit").Visible = False
            rtbFabricationOrigination.FindItemByText("Save").Visible = True
            rtbFabricationOrigination.FindItemByText("Cancel").Visible = True
        End If
    End Sub

#End Region

#Region "Grid Events"

    Private Sub grdFabOrig_BiffExporting(sender As Object, e As GridBiffExportingEventArgs) Handles grdFabOrig.BiffExporting
        For Each c As Cell In e.ExportStructure.Tables(0).Cells
            If c.RowIndex > 1 Then
                c.Style.BorderTopStyle = BorderStyle.Solid
                c.Style.BorderTopWidth = Unit.Pixel(1)
                c.Style.BorderTopColor = Drawing.Color.Black
                c.Style.BorderRightStyle = BorderStyle.Solid
                c.Style.BorderRightWidth = Unit.Pixel(1)
                c.Style.BorderRightColor = Drawing.Color.Black
                c.Style.BorderBottomStyle = BorderStyle.Solid
                c.Style.BorderBottomWidth = Unit.Pixel(1)
                c.Style.BorderBottomColor = Drawing.Color.Black
                c.Style.BorderLeftStyle = BorderStyle.Solid
                c.Style.BorderLeftWidth = Unit.Pixel(1)
                c.Style.BorderLeftColor = Drawing.Color.Black
            End If
        Next
    End Sub

    Private Sub grdFabOrig_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdFabOrig.NeedDataSource
        grdFabOrig.DataSource = FabOrigISNs
        grdFabOrig.MasterTableView.DataKeyNames = New String() {"ISN"}
    End Sub

    Protected Sub grdFabOrig_PreRender(sender As Object, e As EventArgs) Handles grdFabOrig.PreRender
        grdFabOrig.Rebind()
        If grdFabOrig.MasterTableView.Items.Count > 0 AndAlso Not CType(Session("EditRows"), Boolean) Then
            rtbFabricationOrigination.FindItemByText("Export").Enabled = True
        Else
            rtbFabricationOrigination.FindItemByText("Export").Enabled = False
        End If

        If isExporting Then
            Dim isn As String
            For Each gItem As GridDataItem In Me.grdFabOrig.MasterTableView.Items
                isn = gItem.GetDataKeyValue("ISN").ToString
                If _selectedISNs IsNot Nothing AndAlso _selectedISNs.Contains(isn) Then
                    gItem.Visible = True
                    gItem.Display = True
                    gItem.BackColor = Drawing.Color.White
                Else
                    gItem.Visible = False
                    gItem.Display = False
                End If
            Next
        End If
    End Sub

    Protected Sub grdFabOrig_ItemCreated(sender As Object, e As GridItemEventArgs)
        If e.Item.IsInEditMode AndAlso TypeOf e.Item Is GridEditableItem Then
            Dim item As GridEditableItem = TryCast(e.Item, GridEditableItem)
            item.CssClass = ""
            item("StartShipDate").BackColor = Drawing.Color.LightGoldenrodYellow
            item("ISN").BackColor = Drawing.Color.LightGoldenrodYellow
            item("VendorStyleNumber").BackColor = Drawing.Color.LightGoldenrodYellow
            item("ISNDesc").BackColor = Drawing.Color.LightGoldenrodYellow
            item("FabSource").BackColor = Drawing.Color.LightGoldenrodYellow
            item("Fabrication").BackColor = Drawing.Color.LightGoldenrodYellow
            Dim txtFab As TextBox = DirectCast(item("Fabrication").Controls(0), TextBox)
            txtFab.Width = Unit.Percentage(100)
            item("OrigSource").BackColor = Drawing.Color.LightGoldenrodYellow
            item("Origination").BackColor = Drawing.Color.LightGoldenrodYellow
            Dim txtOrig As TextBox = DirectCast(item("Origination").Controls(0), TextBox)
            txtOrig.Width = Unit.Percentage(100)
            item("LastModBy").BackColor = Drawing.Color.LightGoldenrodYellow
            item("LastModDate").BackColor = Drawing.Color.LightGoldenrodYellow
        End If
    End Sub

    Protected Sub grdFabOrig_ItemDataBound(sender As Object, e As GridItemEventArgs)

        Dim upc As Long = 0
        Dim isn As Integer = 0

        Dim needsFabInfo As Boolean = False
        Dim needsOrigInfo As Boolean = False

        Dim styleSkuFabDescription As String = String.Empty
        Dim styleSkuOrigDescription As String = String.Empty

        Dim productInfo As GXSProductInfo = Nothing

        Dim editRows As Boolean = CBool(Session("EditRows"))

        If editRows Then

            If (TypeOf e.Item Is GridEditableItem) AndAlso (e.Item.IsInEditMode) Then

                Dim item As GridEditableItem = DirectCast(e.Item, GridEditableItem)
                Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)

                isn = CInt(item.GetDataKeyValue("ISN").ToString())
                If Not String.IsNullOrEmpty(WebUtility.HtmlDecode(item.GetDataKeyValue("UPC").ToString())) Then
                    upc = CLng(item.GetDataKeyValue("UPC").ToString())
                End If

                upcList = DirectCast(Session("UPCList"), List(Of FabOrig))

                Dim fabOrig As FabOrig = upcList.FirstOrDefault(Function(q) q.ISN = isn)

                If Not IsNothing(fabOrig) Then
                    styleSkuFabDescription = fabOrig.StyleSkuFabDescription.ToString()
                    styleSkuOrigDescription = fabOrig.StyleSkuOrigDescription.ToString()
                    If upc > 0 Then
                        productInfo = GetFabOrigGxsData(upc)
                    End If
                    Dim txtFab As TextBox = DirectCast(item("Fabrication").Controls(0), TextBox)

                    If String.IsNullOrEmpty(WebUtility.HtmlDecode(txtFab.Text).Trim) Then
                        needsFabInfo = True
                    End If

                    If needsFabInfo Then
                        If Not IsNothing(productInfo) AndAlso Not IsNothing(productInfo.FabricOfMaterialDescription) Then
                            If item.IsInEditMode Then
                                If Not IsNothing(txtFab) Then
                                    If CBool(Session("Flood")) Then
                                        If Not String.IsNullOrEmpty(Session("FloodFabrication").ToString()) Then
                                            txtFab.Text = Session("FloodFabrication").ToString()
                                        End If
                                        fabOrig.FabricationOriginalValue = txtFab.Text.Trim
                                        Dim sourceCell As TableCell = item("FabSource")
                                        sourceCell.Text = TtuSourceCode
                                    Else
                                        txtFab.Text = productInfo.FabricOfMaterialDescription.ToString().Trim
                                        Dim sourceCell As TableCell = item("FabSource")
                                        sourceCell.Text = GxsSourceCode
                                        fabOrig.FabricationOriginalValue = txtFab.Text.Trim
                                    End If

                                End If
                            End If
                        Else
                            If Not IsNothing(txtFab) Then
                                If CBool(Session("Flood")) Then
                                    If Not String.IsNullOrEmpty(Session("FloodFabrication").ToString()) Then
                                        txtFab.Text = Session("FloodFabrication").ToString().Trim
                                    End If
                                    fabOrig.FabricationOriginalValue = txtFab.Text.Trim
                                    Dim sourceCell As TableCell = item("FabSource")
                                    sourceCell.Text = TtuSourceCode
                                Else
                                    txtFab.Text = styleSkuFabDescription
                                    Dim sourceCell As TableCell = item("FabSource")
                                    sourceCell.Text = SSkuSourceCode
                                    fabOrig.FabricationOriginalValue = txtFab.Text.Trim
                                End If

                            End If
                        End If
                    Else
                        If CBool(Session("Flood")) Then
                            If Not String.IsNullOrEmpty(Session("FloodFabrication").ToString()) Then
                                txtFab.Text = Session("FloodFabrication").ToString().Trim
                            End If
                            Dim sourceCell As TableCell = item("FabSource")
                            sourceCell.Text = TtuSourceCode
                        End If
                        fabOrig.FabricationOriginalValue = txtFab.Text.Trim
                    End If

                    Dim txtOrig As TextBox = DirectCast(item("Origination").Controls(0), TextBox)
                    If String.IsNullOrEmpty(WebUtility.HtmlDecode(txtOrig.Text).Trim) Then
                        needsOrigInfo = True
                    End If
                    If needsOrigInfo Then
                        If Not IsNothing(productInfo) AndAlso Not IsNothing(productInfo.CountryOfOrigin) Then
                            If item.IsInEditMode Then
                                If Not IsNothing(txtOrig) Then
                                    If CBool(Session("Flood")) Then
                                        If Not String.IsNullOrEmpty(Session("FloodOrigination").ToString()) Then
                                            txtOrig.Text = Session("FloodOrigination").ToString().Trim
                                        End If
                                        fabOrig.OriginationOriginalValue = txtOrig.Text.Trim
                                        Dim sourceCell As TableCell = item("OrigSource")
                                        sourceCell.Text = TtuSourceCode
                                    Else
                                        txtOrig.Text = productInfo.CountryOfOrigin.ToString()
                                        Dim sourceCell As TableCell = item("OrigSource")
                                        sourceCell.Text = GxsSourceCode
                                        fabOrig.OriginationOriginalValue = txtOrig.Text.Trim
                                    End If

                                End If
                            End If
                        Else
                            If Not IsNothing(txtOrig) Then
                                If CBool(Session("Flood")) Then
                                    If Not String.IsNullOrEmpty(Session("FloodOrigination").ToString()) Then
                                        txtOrig.Text = Session("FloodOrigination").ToString().Trim
                                    End If
                                    fabOrig.OriginationOriginalValue = txtOrig.Text.Trim
                                    Dim sourceCell As TableCell = item("OrigSource")
                                    sourceCell.Text = TtuSourceCode
                                Else
                                    txtOrig.Text = styleSkuOrigDescription
                                    Dim sourceCell As TableCell = item("OrigSource")
                                    sourceCell.Text = SSkuSourceCode
                                    fabOrig.OriginationOriginalValue = txtOrig.Text.Trim
                                End If

                            End If
                        End If
                    Else
                        If CBool(Session("Flood")) Then
                            If Not String.IsNullOrEmpty(Session("FloodOrigination").ToString()) Then
                                txtOrig.Text = Session("FloodOrigination").ToString().Trim
                            End If
                            Dim sourceCell As TableCell = item("OrigSource")
                            sourceCell.Text = TtuSourceCode
                        End If
                        fabOrig.OriginationOriginalValue = txtOrig.Text.Trim
                    End If
                    For Each foo As FabOrig In upcList
                        If foo.ISN = isn Then
                            upcList.Remove(fabOrig)
                            Exit For
                        End If
                    Next
                    upcList.Add(fabOrig)
                    Session("UPCList") = upcList
                End If
            End If
        End If
        If TypeOf e.Item Is GridDataItem Then
            Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)
            If Not IsNothing(DataBinder.Eval(item.DataItem, "LastModTs")) Then
                Dim lastModTs As DateTime = DirectCast(DataBinder.Eval(item.DataItem, "LastModTs"), DateTime)
                If lastModTs = DateTime.MinValue Then
                    item("LastModDate").Text = ""
                End If
            End If

        End If

    End Sub

#End Region

#Region "GXS Service Methods"

    Public Function GetFabOrigGxsData(ByVal UPC As Long) As GXSProductInfo
        Try
            Dim gxs As New GXSServiceHelper()
            Dim gxsProductInfo As GXSProductInfo = Nothing

            gxsProductInfo = gxs.FindItemsByGTIN(UPC, CatalogViews.Jewelry)

            Return gxsProductInfo
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Return Nothing
        End Try
    End Function

#End Region

#Region "Validate Methods"

    Private Function ValidateISN(ByVal txtISN As String, ByVal IsReserve As Boolean) As Boolean
        Dim isn As Decimal
        If Not Decimal.TryParse(txtISN, isn) Then
            Return False
        ElseIf Not (IsValidISN(isn, IsReserve)) Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Function IsValidISN(ByVal ISN As Decimal, ByVal IsReserve As Boolean) As Boolean
        Return _TUEcommFabOrig.GetISNExists(ISN, IsReserve)
    End Function

    Private Function ValidateUPCSKU(ByVal txtUPC As String) As Boolean
        Dim upc As Decimal
        If Not Decimal.TryParse(txtUPC, upc) Then
            Return False
        ElseIf Not (IsValidUPCSKU(upc)) Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Function IsValidUPCSKU(ByVal UPC As Decimal) As Boolean
        Return _TUEcommFabOrig.GetUPCSKUExists(UPC)
    End Function

    Protected Sub valAsciiChars_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
        Dim value As String = DirectCast(validator.EvaluatedControl, RadTextBox).Text.Trim
        Dim isValidASCII As Boolean = True

        Try
            If Len(value) > 0 Then
                For i As Int32 = 0 To Len(value) - 1
                    Dim strChar As String = value.Substring(i, 1)
                    If Asc(strChar) < 32 Or Asc(strChar) > 126 Then
                        If Asc(strChar) = 13 Or Asc(strChar) = 10 Then 'Carriage return and Line feed are valid.
                        Else
                            isValidASCII = False
                            Exit For
                        End If
                    End If

                    'Find the Caret
                    If Asc(strChar) = 94 Then
                        isValidASCII = False
                        Exit For
                    End If

                    'Compare the text value to the ASCII representation 
                    If strChar <> Chr(Asc(strChar)) Then
                        isValidASCII = False
                        Exit For
                    End If
                Next

                If Not isValidASCII Then
                    mpFabricationOrigination.ErrorMessage = "Errors on Page."
                    validator.ErrorMessage = "Invalid Character Found."
                    args.IsValid = False
                End If
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

#End Region

#Region "Helper Methods"
    ''' <summary>
    ''' Gets turn in filter user selections
    ''' </summary>
    ''' <returns>Turn in filter user selections</returns>
    ''' <remarks>
    ''' This method will throw runtime exception if any of these controls removed from the user control.  So this method should be updated if the below mentioned control(s) are removed.
    ''' </remarks>
    Private Function GetTurnInFilters(ByVal searchType As String) As TUFilter
        Dim turnInFilter As New TUFilter
        Select Case searchType
            Case "PO"
                turnInFilter.AvailableForTurnIn = True
                turnInFilter.NotAvailableForTurnIn = False
                turnInFilter.ActiveOnWeb = False
                turnInFilter.NotActiveOnWeb = True
                turnInFilter.NotTurnedIn = True

            Case "Hierarchy"
                turnInFilter.AvailableForTurnIn = False
                turnInFilter.NotAvailableForTurnIn = False
                turnInFilter.ActiveOnWeb = False
                turnInFilter.NotActiveOnWeb = False
                turnInFilter.NotTurnedIn = False

            Case "ISN"
                turnInFilter.AvailableForTurnIn = False
                turnInFilter.NotAvailableForTurnIn = False
                turnInFilter.ActiveOnWeb = False
                turnInFilter.NotActiveOnWeb = False
                turnInFilter.NotTurnedIn = False

        End Select

        Return turnInFilter
    End Function

    Private Sub SetFabOrigPageIndex(PageIndex As Integer)
        Session("FabOrigCurrentPageIndex") = PageIndex
    End Sub

    Private Sub GetFabOrigPageIndex()

        Dim currentPageIndex As Integer = 0

        currentPageIndex = Convert.ToInt32(Session("FabOrigCurrentPageIndex"))

        If Session("FabOrigCurrentPageIndex") IsNot Nothing Then
            If currentPageIndex > 0 Then
                grdFabOrig.CurrentPageIndex = Convert.ToInt32(Session("FabOrigCurrentPageIndex"))
            End If
        End If

    End Sub

#End Region

#Region "Export Methods"

    Private Sub lnkOK_Click(sender As Object, e As EventArgs) Handles lnkOK.Click
        If grdFabOrig.SelectedItems.Count > 0 Then
            Dim export As New Export
            Dim isExpToFreelance As Boolean = False
            Dim s As New StringBuilder
            Dim isn As String
            isExporting = True
            _selectedISNs = New List(Of String)
            With grdFabOrig
                .ExportSettings.OpenInNewWindow = True
                .ExportSettings.ExportOnlyData = True
                .AllowSorting = False
                .GridLines = GridLines.Both
                For Each gItem As GridDataItem In .Items
                    If gItem.Selected Then
                        isn = gItem.GetDataKeyValue("ISN").ToString
                        _selectedISNs.Add(isn)
                    End If
                Next
                If ExportText = "PDF" Then
                    .MasterTableView.Columns.FindByUniqueName("ISN").HeaderStyle.Width = Unit.Pixel(80)
                    .MasterTableView.Columns.FindByUniqueName("ISNDesc").HeaderStyle.Width = Unit.Pixel(180)
                    .MasterTableView.Columns.FindByUniqueName("FabSource").HeaderStyle.Width = Unit.Pixel(90)
                    .MasterTableView.Columns.FindByUniqueName("OrigSource").HeaderStyle.Width = Unit.Pixel(90)
                    .MasterTableView.Columns.FindByUniqueName("Origination").HeaderStyle.Width = Unit.Pixel(90)
                    .ExportSettings.Pdf.PageHeight = Unit.Parse("210mm")
                    .ExportSettings.Pdf.PageWidth = Unit.Parse("500mm")
                    .ExportSettings.IgnorePaging = True
                    .ExportSettings.Pdf.PageTitle = lblPageHeader.Text.Replace("<br/>", "")
                    .MasterTableView.ExportToPdf()
                ElseIf ExportText = "Excel" Then
                    .ExportSettings.Excel.Format = GridExcelExportFormat.Biff
                    .ExportSettings.IgnorePaging = False
                    .MasterTableView.Caption = lblPageHeader.Text.Replace("<br/>", "")
                    .MasterTableView.ExportToExcel()
                End If
            End With
        Else
            mpFabricationOrigination.ErrorMessage = "Select at least one record."
        End If
    End Sub

#End Region

End Class


