Option Infer On
Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BLL.MiscConstants
Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL.Enumerations
Imports System.IO

Public Class PreTurnInSetUpCreate
    Inherits PageBase

    Private _preTurnInSetUpCtrl As PreTurnInSetUpCtrl = Nothing
    Private _eCommPreTurnInSetUpCtrl As PreTurnInSetUpCtrl = Nothing
    Dim _TUTMS900PARAMETER As New TUTMS900PARAMETER
    Dim _TUEcommSetupCreate As New TUEcommSetupCreate
    Dim _TUWebCat As New TUWebCat
    Dim _TUCtlgAdPg As New TUCtlgAdPg
    Dim _TUBatch As New TUBatch

#Region "Properties"
    Public ReadOnly Property AllWebCats As List(Of WebCat)
        Get
            If Application("ApplWebCatsObject") Is Nothing Then
                Dim objGetApplicationObjectsService As New GetGlobalObjectsService
                objGetApplicationObjectsService.GetAllApplicationObjects()  'this will create/populate all application objects.
            End If
            Return DirectCast(Application("ApplWebCatsObject"), List(Of WebCat))
        End Get
    End Property

    Public ReadOnly Property CurrentISN() As String
        Get
            Return cmbISNLabel.SelectedValue
        End Get
    End Property

    Public ReadOnly Property eCommPreTurnInSetUpCtrl() As PreTurnInSetUpCtrl
        Get
            Dim control As Control = Me.Master.SideBarPlaceHolder.FindControl("eCommPreTurnInSetUpCtrl1")
            Me._eCommPreTurnInSetUpCtrl = DirectCast(control, PreTurnInSetUpCtrl)
            Return _eCommPreTurnInSetUpCtrl
        End Get
    End Property

    Public Property CurrentBatch As BatchInfo
        Get
            If Session("PreTurnInSetUpCreate.CurrentBatch") Is Nothing Then
                Session("PreTurnInSetUpCreate.CurrentBatch") = New BatchInfo
            End If
            Return CType(Session("PreTurnInSetUpCreate.CurrentBatch"), BatchInfo)
        End Get
        Set(ByVal value As BatchInfo)
            Session("PreTurnInSetUpCreate.CurrentBatch") = value
        End Set

    End Property

    Public Property ResultsTabISNs As List(Of EcommSetupCreateInfo)
        Get
            If Session("PreTurnInSetUpCreate.ResultsTabISNs") Is Nothing Then
                Session("PreTurnInSetUpCreate.ResultsTabISNs") = New List(Of EcommSetupCreateInfo)
            End If
            Return CType(Session("PreTurnInSetUpCreate.ResultsTabISNs"), List(Of EcommSetupCreateInfo))
        End Get
        Set(ByVal value As List(Of EcommSetupCreateInfo))
            Session("PreTurnInSetUpCreate.ResultsTabISNs") = value
        End Set
    End Property

    Public Property SelectedMerch As List(Of EcommSetupCreateInfo)
        Get
            If Session("PreTurnInSetUpCreate.SelectedMerchandiseItems") Is Nothing Then
                Session("PreTurnInSetUpCreate.SelectedMerchandiseItems") = New List(Of EcommSetupCreateInfo)
            End If
            Return CType(Session("PreTurnInSetUpCreate.SelectedMerchandiseItems"), List(Of EcommSetupCreateInfo))
        End Get
        Set(ByVal value As List(Of EcommSetupCreateInfo))
            Session("PreTurnInSetUpCreate.SelectedMerchandiseItems") = value

        End Set
    End Property

    Public Property SelectedWebCats() As List(Of WebCat)
        Get
            If Session("PreTurnInSetUp.SelectedWebCats") Is Nothing Then
                Session("PreTurnInSetUp.SelectedWebCats") = New List(Of WebCat)
            End If
            Return CType(Session("PreTurnInSetUp.SelectedWebCats"), List(Of WebCat))
        End Get
        Set(value As List(Of WebCat))
            Session("PreTurnInSetUp.SelectedWebCats") = value
        End Set
    End Property

    Public ReadOnly Property AllSelectedISNs As List(Of EcommSetupCreateInfo)
        Get
            Return CurrentBatch.ColorSizeItems.Union(ResultsTabISNs.Where(Function(x) x.Selected = True)).Union(SelectedMerch).OrderBy(Function(x) x.Sequence).Distinct.ToList
        End Get
    End Property

    Public ReadOnly Property ISNTabList As List(Of String)
        Get
            Return AllSelectedISNs.OrderBy(Function(x) x.ISN).Select(Function(x) CStr(x.ISN)).Distinct.ToList
        End Get
    End Property
#End Region

#Region "PAGE: Events"

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Not Page.IsPostBack Then

                If (Request.QueryString("CurrentISN") Is Nothing) Then
                    ResultsTabISNs = Nothing
                    SelectedMerch = Nothing

                End If


            End If

            Dim control As Control = LoadControl("~/WebUserControls/eCommerce/eCommPreTurnInSetUpCtrl.ascx")
            If Not control Is Nothing Then
                control.ID = "eCommPreTurnInSetUpCtrl1"
                Me.Master.SideBarPlaceHolder.Controls.Add(control)
            End If
            Me._preTurnInSetUpCtrl = CType(control, PreTurnInSetUpCtrl)

        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Me.Master.SideBar.Width = 260
                If Request.QueryString("Action").ToUpper = Modes.MAINTENANCE.ToString And Request.QueryString("CurrentISN") <> Nothing Then
                    ShowHideTabs("Ad List", False)
                    ShowHideTabs("Ad Level", False)
                    ShowHideTabs("Result List", False)
                    ShowHideTabs("Killed Items", False)
                    ShowHideTabs("ISN Level", True)
                    ShowHideTabs("Color/Size Level", False)
                    rtsAPAdjustment.FindTabByText("ISN Level").Selected = True
                    SetupISNTab()
                ElseIf Request.QueryString("Action").ToUpper = Modes.MAINTENANCE.ToString _
                            Or Request.QueryString("Action").ToUpper = Modes.INQUIRY.ToString Then
                    Session("PreTurnInSetUpCreate.PrevTabIndex") = "-1"
                    Session("PreTurnInSetUpCreate.CurrTabIndex") = "0"
                    SetupAdListTab()
                ElseIf Request.QueryString("Action").ToUpper = Modes.CREATE.ToString Then
                    Session("PreTurnInSetUpCreate.PrevTabIndex") = "-1"
                    Session("PreTurnInSetUpCreate.CurrTabIndex") = "1"
                    Session("PreTurnInSetUpCreate.CurrentBatch") = Nothing
                    SetupAdLevelTab()
                    ShowHideTabs("Ad List", False)
                    ShowHideTabs("Ad Level", True)
                    ShowHideTabs("Result List", False)
                    ShowHideTabs("Killed Items", False)
                    ShowHideTabs("ISN Level", False)
                    ShowHideTabs("Color/Size Level", False)
                End If

                lblBatchIdText.Text = CStr(CurrentBatch.BatchId)
                lblPageHeader.Text = lblPageHeader.Text & Request.QueryString("Action") & "<br/>"
                AddHandler Me.tuModalOrderISN.SaveButton.Click, AddressOf Me.OrderByISN
            End If
            AddHandler grdColorSizeLevel.HeaderContextMenu.ItemCreated, AddressOf Me.HeaderContextMenu_ItemCreated

        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub OrderByISN(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Private Sub SetupAdListTab()
        Session("PreTurnInSetUpCreate.PrevTabIndex") = Session("PreTurnInSetUpCreate.CurrTabIndex")
        Session("PreTurnInSetUpCreate.CurrTabIndex") = "0"

        eCommPreTurnInSetUpCtrl.ChangeMultiView("Ad List")
        Me.Master.SideBar.Visible = True

        ShowHideTabs("Ad List", True)
        ShowHideTabs("Ad Level", rtsAPAdjustment.FindTabByText("Ad Level").Visible)
        ShowHideTabs("Result List", rtsAPAdjustment.FindTabByText("Result List").Visible)
        ShowHideTabs("Killed Items", rtsAPAdjustment.FindTabByText("Killed Items").Visible)
        ShowHideTabs("ISN Level", rtsAPAdjustment.FindTabByText("ISN Level").Visible)
        ShowHideTabs("Color/Size Level", rtsAPAdjustment.FindTabByText("Color/Size Level").Visible)

        rtsAPAdjustment.FindTabByText("Ad List").Selected = True
        rmpPreTurnInCreate.SelectedIndex = 0

        ShowHideButtons("Reset", True)
        ShowHideButtons("Sort", False)
        ShowHideButtons("Level Down", False)
        ShowHideButtons("Level Up", False)
        ShowHideButtons("Save", False)
        ShowHideButtons("Add To Batch", False)
        ShowHideButtons("Retrieve", True)
        ShowHideButtons("Print Labels", False)
        ShowHideButtons("Print Report", False)
        ShowHideButtons("Submit", False)
        ShowHideButtons("Edit All", False)
        ShowHideButtons("Save All", False)
        ShowHideButtons("Cancel All", False)
    End Sub

    Private Sub SetupAdLevelTab()
        If Page.IsPostBack Then
            Session("PreTurnInSetUpCreate.PrevTabIndex") = Session("PreTurnInSetUpCreate.CurrTabIndex")
        End If
        Session("PreTurnInSetUpCreate.CurrTabIndex") = "1"

        eCommPreTurnInSetUpCtrl.ChangeMultiView("Ad Level")
        Me.Master.SideBar.Visible = True

        ShowHideTabs("Ad List", rtsAPAdjustment.FindTabByText("Ad List").Visible)
        ShowHideTabs("Ad Level", True)
        ShowHideTabs("Result List", rtsAPAdjustment.FindTabByText("Result List").Visible)
        ShowHideTabs("Killed Items", rtsAPAdjustment.FindTabByText("Killed Items").Visible)
        ShowHideTabs("ISN Level", rtsAPAdjustment.FindTabByText("ISN Level").Visible)
        ShowHideTabs("Color/Size Level", rtsAPAdjustment.FindTabByText("Color/Size Level").Visible)

        rtsAPAdjustment.FindTabByText("Ad Level").Selected = True
        rmpPreTurnInCreate.SelectedIndex = 1

        ShowHideButtons("Reset", True)
        ShowHideButtons("Sort", False)
        ShowHideButtons("Level Down", False)
        ShowHideButtons("Level Up", False)
        ShowHideButtons("Save", False)
        ShowHideButtons("Add To Batch", False)
        ShowHideButtons("Retrieve", True)
        ShowHideButtons("Print Labels", False)
        ShowHideButtons("Print Report", False)
        ShowHideButtons("Submit", False)
        ShowHideButtons("Edit All", False)
        ShowHideButtons("Save All", False)
        ShowHideButtons("Cancel All", False)
    End Sub

    Private Sub SetupResultListTab()
        Session("PreTurnInSetUpCreate.PrevTabIndex") = Session("PreTurnInSetUpCreate.CurrTabIndex")
        Session("PreTurnInSetUpCreate.CurrTabIndex") = "2"

        ShowHideTabs("Ad List", rtsAPAdjustment.FindTabByText("Ad List").Visible)
        ShowHideTabs("Ad Level", rtsAPAdjustment.FindTabByText("Ad Level").Visible)
        ShowHideTabs("Result List", True)
        ShowHideTabs("Killed Items", rtsAPAdjustment.FindTabByText("Killed Items").Visible)
        ShowHideTabs("ISN Level", rtsAPAdjustment.FindTabByText("ISN Level").Visible)
        ShowHideTabs("Color/Size Level", rtsAPAdjustment.FindTabByText("Color/Size Level").Visible)


        rtsAPAdjustment.FindTabByText("Result List").Selected = True
        rmpPreTurnInCreate.SelectedIndex = 2

        eCommPreTurnInSetUpCtrl.ChangeMultiView("Result List")
        Me.Master.SideBar.Visible = True

        ShowHideButtons("Reset", True)
        ShowHideButtons("Sort", False)
        ShowHideButtons("Level Down", True)
        ShowHideButtons("Level Up", True)
        ShowHideButtons("Retrieve", True)
        ShowHideButtons("Save", False)
        ShowHideButtons("Add To Batch", False)
        ShowHideButtons("Print Labels", False)
        ShowHideButtons("Print Report", False)
        ShowHideButtons("Submit", False)
        ShowHideButtons("Edit All", False)
        ShowHideButtons("Save All", False)
        ShowHideButtons("Cancel All", False)

        If Not grdResultList.Visible Then
            EnableDisableButtons("Level Down", False)
            EnableDisableButtons("Level Up", False)
        End If
    End Sub

    Private Sub SetupKilledTab()
        Session("PreTurnInSetUpCreate.PrevTabIndex") = Session("PreTurnInSetUpCreate.CurrTabIndex")
        Session("PreTurnInSetUpCreate.CurrTabIndex") = "3"

        ShowHideTabs("Ad List", rtsAPAdjustment.FindTabByText("Ad List").Visible)
        ShowHideTabs("Ad Level", rtsAPAdjustment.FindTabByText("Ad Level").Visible)
        ShowHideTabs("Result List", rtsAPAdjustment.FindTabByText("Result List").Visible)
        ShowHideTabs("Killed Items", True)
        ShowHideTabs("ISN Level", rtsAPAdjustment.FindTabByText("ISN Level").Visible)
        ShowHideTabs("Color/Size Level", rtsAPAdjustment.FindTabByText("Color/Size Level").Visible)

        rtsAPAdjustment.FindTabByText("Killed Items").Selected = True
        rmpPreTurnInCreate.SelectedIndex = 3

        eCommPreTurnInSetUpCtrl.ChangeMultiView("Killed")
        Me.Master.SideBar.Visible = True

        ShowHideButtons("Reset", True)
        ShowHideButtons("Sort", False)
        ShowHideButtons("Level Down", True)
        ShowHideButtons("Level Up", True)
        ShowHideButtons("Retrieve", True)
        ShowHideButtons("Save", False)
        ShowHideButtons("Add To Batch", True)
        ShowHideButtons("Print Labels", False)
        ShowHideButtons("Print Report", False)
        ShowHideButtons("Submit", False)
        ShowHideButtons("Edit All", False)
        ShowHideButtons("Save All", False)
        ShowHideButtons("Cancel All", False)

        EnableDisableButtons("Level Down", False)
        EnableDisableButtons("Level Up", False)
    End Sub

    Private Sub rtsAPAdjustment_TabClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTabStripEventArgs) Handles rtsAPAdjustment.TabClick
        Try
            Select Case e.Tab.PageViewID.ToString()
                Case "pvAdList", "pvAdLevel", "pvResultList", "pvKilled", "pvColorSizeLevel"
                    If hdnISNTabSaveFlg.Value = "Y" Then
                        Page.Validate("ISNLevel")
                        If Page.IsValid Then
                            SaveISNData()
                        Else
                            rtsAPAdjustment.FindTabByText("ISN Level").Selected = True
                            rmpPreTurnInCreate.SelectedIndex = 4
                            Exit Sub
                        End If
                    End If
            End Select
            Session("PreTurnInSetUp.Tab") = e.Tab.PageViewID
            Select Case e.Tab.PageViewID
                Case "pvAdList"
                    Me.Master.SideBar.Collapsed = False
                    SetupAdListTab()
                Case "pvAdLevel"
                    Me.Master.SideBar.Collapsed = False
                    SetupAdLevelTab()
                Case "pvResultList"
                    Me.Master.SideBar.Collapsed = False
                    SetupResultListTab()
                Case "pvKilled"
                    Me.Master.SideBar.Collapsed = False
                    SetupKilledTab()
                Case "pvISNLevel"
                    Me.Master.SideBar.Collapsed = True
                    SetupISNTab()
                Case "pvColorSizeLevel"
                    Me.Master.SideBar.Collapsed = True
                    SetupColorSizeTab()
            End Select
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub rtbPreTurnInCreate_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles rtbPreTurnInCreate.ButtonClick
        Try
            If TypeOf e.Item Is RadToolBarButton Then
                Dim radToolBarButton As RadToolBarButton = DirectCast(e.Item, RadToolBarButton)

                ClearMessagePanel()

                Select Case radToolBarButton.CommandName
                    Case "Retrieve"
                        Select Case rtsAPAdjustment.SelectedTab.Index
                            Case 0
                                If eCommPreTurnInSetUpCtrl.ValidateMaintSearch Then

                                    grdMaint.Visible = True
                                    grdMaint.Rebind()

                                    ShowHideTabs("Ad Level", False)
                                    ShowHideTabs("Result List", False)
                                    ShowHideTabs("Killed Items", False)
                                    ShowHideTabs("ISN Level", False)
                                    ShowHideTabs("Color/Size Level", False)
                                Else
                                    MessagePanel1.ErrorMessage = "Please select criteria from left navigation."
                                    Return
                                End If
                            Case 1
                                CurrentBatch = Nothing
                                eCommPreTurnInSetUpCtrl.ValidateSearch()

                                If Page.IsValid() Then

                                    If _TUBatch.IsBatchExists(eCommPreTurnInSetUpCtrl.SelectedAd, CInt(eCommPreTurnInSetUpCtrl.PageNumberComboBox.SelectedValue), SessionWrapper.UserID) Then
                                        MessagePanel1.WarningMessage = "Batch already exists for Ad/Page/User combination. Please use maintenance mode to edit that batch or continue to create a new batch."
                                    End If

                                    lblBatchIdText.Text = ""
                                    RetrieveAd(eCommPreTurnInSetUpCtrl.SelectedAd, eCommPreTurnInSetUpCtrl.PageNumberComboBox.Text)

                                    CurrentBatch.AdNumber = eCommPreTurnInSetUpCtrl.SelectedAd
                                    CurrentBatch.PageNumber = CInt(eCommPreTurnInSetUpCtrl.PageNumberComboBox.SelectedValue)
                                Else
                                    MessagePanel1.ErrorMessage = ErrorOnPage
                                    Return
                                End If
                            Case 2
                                RetrieveResultList()
                                grdResultList.Rebind()
                                EnableDisableButtons("Level Down", True)
                                EnableDisableButtons("Level Up", True)
                                ShowHideTabs("Color/Size Level", False)
                            Case 3
                                grdKilled.Rebind()
                                EnableDisableButtons("Level Down", True)
                                EnableDisableButtons("Level Up", True)
                                ShowHideTabs("Color/Size Level", False)
                            Case 4
                            Case 5
                        End Select
                    Case "LevelUp"
                        For Each item As GridDataItem In grdResultList.MasterTableView.Items
                            item.Expanded = False
                        Next
                    Case "LevelDown"
                        For Each item As GridDataItem In grdResultList.MasterTableView.Items
                            item.Expanded = True
                        Next
                    Case "Reset"
                        Dim CurrTabIndex As Integer = CInt(Session("PreTurnInSetUpCreate.CurrTabIndex"))

                        Select Case CurrTabIndex
                            Case 0
                                Session("PreTurnInSetUpCreate.AllISNs") = Nothing
                                SelectedMerch = Nothing
                                Response.Redirect(Request.Url.ToString(), False)
                            Case 1
                                If Request.QueryString("Action").ToUpper = Modes.CREATE.ToString Then
                                    Session("PreTurnInSetUpCreate.AllISNs") = Nothing
                                    SelectedMerch = Nothing
                                    Response.Redirect(Request.Url.ToString(), False)
                                Else
                                    ResetAdLevelTab()
                                End If
                            Case 2
                                Me.Master.SideBar.Collapsed = False
                                ResetResultListTab()
                            Case 3
                                Me.Master.SideBar.Collapsed = True
                                SetupISNTab()
                        End Select
                    Case "EditAll"
                        EditModeOnOff("On")
                        PutRowsInEditMode(grdColorSizeLevel, True)
                    Case "SaveAll"
                        Session("PreTurnInSetUp.LastUpdatedRowId") = ""
                        If SaveRows(grdColorSizeLevel) Then
                            PutRowsInEditMode(grdColorSizeLevel, False)
                            EditModeOnOff("Off")
                        End If
                    Case "CancelAll"
                        EditModeOnOff("Off")
                        PutRowsInEditMode(grdColorSizeLevel, False)
                    Case "AddToBatch"
                        For Each item As GridDataItem In grdKilled.MasterTableView.GetSelectedItems
                            Dim selectedTurnInMerchID As Integer = CInt(item.GetDataKeyValue("TurnInMerchID"))
                            Dim selectedAdminMerchID As Integer = CInt(item.GetDataKeyValue("AdminMerchNum"))

                            'Add the selected Killed Item to the current batch.
                            CurrentBatch.BatchId = _TUEcommSetupCreate.AddToBatch(selectedTurnInMerchID, selectedAdminMerchID, CurrentBatch.BatchId, CurrentBatch.AdNumber, CurrentBatch.PageNumber, SessionWrapper.UserID)

                            CurrentBatch.ColorSizeItems = _TUEcommSetupCreate.GetEcommSetupMaintenanceResults(CurrentBatch.BatchId)
                            lblBatchIdText.Text = CStr(CurrentBatch.BatchId)

                        Next
                    Case "Save"
                        Page.Validate("ISNLevel")
                        If Page.IsValid Then
                            SaveISNData()

                            'To track ISN tab level changes.
                            hdnISNTabChanges.Value = cmbLabel.SelectedItem.Text & cmbSizeCategory.SelectedItem.Text & rblVendorApproval.SelectedValue _
                                                    & cmbModelCategory.SelectedItem.Text & rblAdditionalColorsSamples.SelectedValue

                            For Each li As ListItem In cmbTurnInUsageIndicator.Items
                                If li.Selected Then
                                    hdnISNTabChanges.Value += li.Value
                                End If
                            Next
                        End If
                    Case "Back"
                        Dim CurrTabIndex As Integer = -1
                        Dim PrevTabIndex As Integer = -1
                        If Session("PreTurnInSetUpCreate.CurrTabIndex") IsNot Nothing Then
                            CurrTabIndex = CInt(Session("PreTurnInSetUpCreate.CurrTabIndex"))
                        End If
                        If Session("PreTurnInSetUpCreate.PrevTabIndex") IsNot Nothing Then
                            PrevTabIndex = CInt(Session("PreTurnInSetUpCreate.PrevTabIndex"))
                        End If

                        Select Case CurrTabIndex
                            Case 3
                                If hdnISNTabSaveFlg.Value = "Y" Then
                                    Page.Validate("ISNLevel")
                                    If Page.IsValid Then
                                        SaveISNData()
                                    Else
                                        Exit Sub
                                    End If
                                End If
                            Case 4
                                If grdColorSizeLevel.MasterTableView.ChildEditItems.Count > 0 Then
                                    mpPreTurnInCreate.PopUpMessage = "At least one record is being edited. Save/Cancel the record and then click Back button."
                                    Exit Sub
                                End If
                        End Select

                        Select Case PrevTabIndex
                            Case 0
                                Me.Master.SideBar.Collapsed = False
                                SetupAdListTab()
                            Case 1
                                Me.Master.SideBar.Collapsed = False
                                SetupAdLevelTab()
                            Case 2
                                Me.Master.SideBar.Collapsed = False
                                SetupResultListTab()
                            Case 3
                                Me.Master.SideBar.Collapsed = True
                                SetupISNTab()
                            Case 4
                                Me.Master.SideBar.Collapsed = True
                                SetupColorSizeTab()
                            Case Else
                                Response.Redirect(PreviousPageUrl, False)
                        End Select
                    Case "PrintLabel"
                        UpdatePrintLabelFlg()
                    Case "Submit"
                        SubmitColorSizeData()
                End Select
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub cmbISNLabel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbISNLabel.SelectedIndexChanged
        If hdnISNTabSaveFlg.Value = "Y" Then
            Page.Validate("ISNLevel")
            If Page.IsValid Then
                Session("PreTurnInSetUpCreate.NextISNIndex") = cmbISNLabel.SelectedIndex
                cmbISNLabel.SelectedIndex = CInt(Session("PreTurnInSetUpCreate.CurrISNIndex"))
                SaveISNData()
                cmbISNLabel.SelectedIndex = CInt(Session("PreTurnInSetUpCreate.NextISNIndex"))
            Else
                cmbISNLabel.SelectedIndex = CInt(Session("PreTurnInSetUpCreate.CurrISNIndex"))
                Exit Sub
            End If
        End If

        BindISNTabElements()
    End Sub

    Protected Sub tuModalOrderISN_SaveISNOrder(ByVal sender As Object, ByVal e As EventArgs) Handles tuModalOrderISN.SaveISNOrder
        Dim rlbISNs As RadListBox = CType(tuModalOrderISN.FindControl("rlbISNs"), RadListBox)

        For Each item As RadListBoxItem In rlbISNs.Items
            Dim ISN As Decimal = CDec(item.Value)
            AllSelectedISNs.Find(Function(x) x.ISN = ISN).Sequence = item.Index + 1
            _TUBatch.UpdateBatchSequence(CurrentBatch.BatchId, ISN, item.Index + 1)
        Next
        grdColorSizeLevel.Rebind()
    End Sub

    Protected Sub tuModalMoveBatch_MoveBatch(ByVal sender As Object, ByVal e As MoveBatchEventArgs)
        Dim tuModalMoveBatch As ModalMoveBatch = CType(sender, ModalMoveBatch)
        _TUBatch.UpdateBatchNumber(tuModalMoveBatch.BatchNum, tuModalMoveBatch.oldAdNum, e.newAdNum)
        grdMaint.Rebind()
    End Sub
#End Region

#Region "METHODS"
    Private Sub ClearMessagePanel()
        MessagePanel1.ErrorMessage = ""
    End Sub

    Private Sub EnableDisableAdLevelandResults()
        CurrentBatch = Nothing
        For Each item As GridDataItem In grdMaint.MasterTableView.Items
            If CType(item.FindControl("chkItem"), CheckBox).Checked Then
                With CurrentBatch
                    .BatchId = CInt(item.GetDataKeyValue("BatchId"))
                    .AdNumber = CInt(item("AdNumber").Text)
                    .PageNumber = CInt(item("PageNumber").Text)
                    .UserId = item("User").Text
                    .Buyer = item("Buyer").Text
                    .Departments = item("Depts").Text
                    .ColorSizeItems.AddRange(_TUEcommSetupCreate.GetEcommSetupMaintenanceResults(CurrentBatch.BatchId))
                End With

                lblBatchIdText.Text = CStr(CurrentBatch.BatchId)
                RetrieveAd(CurrentBatch.AdNumber, CStr(CurrentBatch.PageNumber) & " - " & item("PageDescription").Text)
                ShowHideTabs("Ad Level", True)
                ShowHideTabs("Result List", True)
                ShowHideTabs("Killed Items", True)
                ShowHideTabs("ISN Level", True)
                ShowHideTabs("Color/Size Level", True)

                Exit For
            End If
        Next
        If CurrentBatch.AdNumber = 0 Then
            lblBatchIdText.Text = ""
            lblAdNoText.Text = ""
            lblAdNoDescText.Text = ""
            lblTurnInDateText.Text = ""
            lblPageNumberText.Text = ""

            ShowHideTabs("Ad Level", False)
            ShowHideTabs("Result List", False)
            ShowHideTabs("Killed Items", False)
            ShowHideTabs("ISN Level", False)
            ShowHideTabs("Color/Size Level", False)
        End If
    End Sub

    Private Sub ResetComboBox(ByVal rcbComboBox As RadComboBox, ByVal isEnabled As Boolean)
        rcbComboBox.ClearSelection()
        rcbComboBox.Text = ""
        rcbComboBox.Enabled = isEnabled
    End Sub
#End Region

#Region "AD LIST TAB: Events & Methods"
    'Protected Sub AdListToggleSelectALL(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim headerCheckBox As CheckBox = TryCast(sender, CheckBox)

    '    'Check all visible items on the page
    '    For Each dataItem As GridDataItem In grdMaint.MasterTableView.Items
    '        TryCast(dataItem.FindControl("chkItem"), CheckBox).Checked = headerCheckBox.Checked
    '    Next

    '    EnableDisableAdLevelandResults()
    'End Sub

    Protected Sub AdListToggleSelectItem(ByVal sender As Object, ByVal e As EventArgs)
        Dim cbx As CheckBox = TryCast(sender, CheckBox)
        'Dim headerItem As GridHeaderItem = TryCast(grdMaint.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        'Dim chkAll As CheckBox = TryCast(headerItem.FindControl("chkAll"), CheckBox)
        'If Not cbx.Checked Then
        '    chkAll.Checked = False
        'Else
        '    Dim checkAllFlag As Boolean = True
        '    For Each item As GridDataItem In grdMaint.MasterTableView.Items
        '        If Not CType(item.FindControl("chkItem"), CheckBox).Checked Then
        '            checkAllFlag = False
        '            Exit For
        '        End If
        '    Next
        '    chkAll.Checked = checkAllFlag
        'End If
        EnableDisableAdLevelandResults()
        For Each item As GridDataItem In grdMaint.MasterTableView.Items
            Dim currentRowCheckBox = CType(item.FindControl("chkItem"), CheckBox)
            If cbx.Checked And Not currentRowCheckBox.Checked Then
                'disable checkbox
                currentRowCheckBox.Enabled = False
            ElseIf Not cbx.Checked Then
                'enable checkbox
                currentRowCheckBox.Enabled = True
            End If
        Next
    End Sub
#End Region

#Region "AD TAB: Events & Methods"
    Private Sub ResetAdLevelTab()
        ResetComboBox(eCommPreTurnInSetUpCtrl.AdComboBox, True)
        ResetComboBox(eCommPreTurnInSetUpCtrl.PageNumberComboBox, False)
    End Sub

    Private Sub RetrieveAd(ByVal SelectedAd As Integer, ByVal PageNum As String)
        Dim _TUCtlgAdPg As New TUCtlgAdPg
        Dim _TUAdInfo As New TUAdInfo

        Dim ad As AdInfoInfo = _TUAdInfo.GetAdInfoByAdNbr(SelectedAd)
        If ad IsNot Nothing Then
            lblAdNoText.Text = CStr(ad.adnbr)
            lblAdNoDescText.Text = ad.addesc
            lblTurnInDateText.Text = ad.TurnInDate
            hdnAdStartDate.Value = CStr(ad.adrunstartdt)
            hdnAdEndDate.Value = CStr(ad.adrunenddt)
            lblPageNumberText.Text = PageNum
            pnlAdLevel.Visible = True
            ShowHideTabs("Result List", True)
            ShowHideTabs("Killed Items", True)
            ShowHideTabs("ISN Level", True)
        End If

    End Sub
#End Region

#Region "RESULTS TAB: Events & Methods"
    Private Sub ResetResultListTab()
        If eCommPreTurnInSetUpCtrl.ResultsTabRadPanelBar.Items(0).Expanded Then
            eCommPreTurnInSetUpCtrl.cblPriceStatus.ClearSelection()
            eCommPreTurnInSetUpCtrl.cblPriceStatus.Items(0).Selected = True
            eCommPreTurnInSetUpCtrl.cblPriceStatus.Items(1).Selected = True

            ResetComboBox(eCommPreTurnInSetUpCtrl.cmbDept, True)
            ResetComboBox(eCommPreTurnInSetUpCtrl.cmbClass, False)
            ResetComboBox(eCommPreTurnInSetUpCtrl.cmbSubClass, False)
            ResetComboBox(eCommPreTurnInSetUpCtrl.cmbVendor, False)
            ResetComboBox(eCommPreTurnInSetUpCtrl.cmbVendorStyle, False)
            eCommPreTurnInSetUpCtrl.lbVendorStyles.Items.Clear()
            ResetComboBox(eCommPreTurnInSetUpCtrl.cmbACode1, False)
            ResetComboBox(eCommPreTurnInSetUpCtrl.cmbACode2, False)
            ResetComboBox(eCommPreTurnInSetUpCtrl.cmbACode3, False)
            ResetComboBox(eCommPreTurnInSetUpCtrl.cmbACode4, False)
            ResetComboBox(eCommPreTurnInSetUpCtrl.cmbSellYear, False)
            ResetComboBox(eCommPreTurnInSetUpCtrl.cmbSellSeason, False)
            eCommPreTurnInSetUpCtrl.dpCreatedSince.Clear()
        ElseIf eCommPreTurnInSetUpCtrl.ResultsTabRadPanelBar.Items(1).Expanded Then
            eCommPreTurnInSetUpCtrl.txtISN.Text = ""
            eCommPreTurnInSetUpCtrl.lbISNs.Items.Clear()
        End If

        'Show ISN tab all the time.
        ShowHideTabs("ISN Level", True)
        ResultsTabISNs = Nothing
        grdResultList.Visible = False
    End Sub

    Private Sub RetrieveResultList()
        Dim SelectedVendorStyles As New List(Of String)
        Dim SelISNList As New List(Of String)
        Dim SelReserveISNList As New List(Of String)
        Dim IsValid As Boolean = True
        Dim turnInFilter As TUFilter = Nothing
        Dim departmentID As Int16 = 0
        Dim vendorID As Integer = 0
        Dim dmmID As Integer = 0
        Dim buyerID As Integer = 0
        Dim includeOnlyApprovedItems As Boolean = False
        ResultsTabISNs = Nothing

        If eCommPreTurnInSetUpCtrl.ResultsTabRadPanelBar.Items(0).Expanded Then

            If eCommPreTurnInSetUpCtrl.cmbDMM.SelectedValue = String.Empty And eCommPreTurnInSetUpCtrl.cmbPOBuyer.SelectedValue = String.Empty And eCommPreTurnInSetUpCtrl.cmbPODept.SelectedValue = String.Empty Then
                MessagePanel1.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1016, False)
                grdResultList.Visible = False
            Else
                For Each item In eCommPreTurnInSetUpCtrl.lbPOVendorStyles.Items
                    SelectedVendorStyles.Add(CType(item, ListItem).Text.Trim())
                Next

                If eCommPreTurnInSetUpCtrl.cmbDMM.SelectedValue <> String.Empty Then
                    dmmID = CInt(eCommPreTurnInSetUpCtrl.cmbDMM.SelectedValue)
                End If

                If eCommPreTurnInSetUpCtrl.cmbPOBuyer.SelectedValue <> String.Empty Then
                    buyerID = CInt(eCommPreTurnInSetUpCtrl.cmbPOBuyer.SelectedValue)
                End If

                If Not SelectedVendorStyles Is Nothing AndAlso SelectedVendorStyles.Count = 0 Then
                    vendorID = CInt(If(eCommPreTurnInSetUpCtrl.cmbPOVendor.SelectedValue = String.Empty, 0, CInt(eCommPreTurnInSetUpCtrl.cmbPOVendor.SelectedValue)))
                    departmentID = CShort(CInt(If(eCommPreTurnInSetUpCtrl.cmbPODept.SelectedValue = String.Empty, 0, CInt(eCommPreTurnInSetUpCtrl.cmbPODept.SelectedValue))))
                End If

                If eCommPreTurnInSetUpCtrl.cmbPOVendorStyle.SelectedValue <> String.Empty AndAlso Not SelectedVendorStyles.Contains(eCommPreTurnInSetUpCtrl.cmbPOVendorStyle.SelectedValue.Trim()) Then
                    SelectedVendorStyles.Add(eCommPreTurnInSetUpCtrl.cmbPOVendorStyle.SelectedValue.Trim)
                End If

                turnInFilter = GetTurnInFilters()

                includeOnlyApprovedItems = turnInFilter.AvailableForTurnIn AndAlso Not turnInFilter.NotAvailableForTurnIn

                ResultsTabISNs = _TUEcommSetupCreate.GetAllEcommSetupCreateResultsByPOShipDate(CurrentBatch.AdNumber, CurrentBatch.PageNumber, dmmID, buyerID, departmentID, _
                                                                                              CShort(If(eCommPreTurnInSetUpCtrl.cmbPOClass.SelectedValue = String.Empty, 0, CShort(eCommPreTurnInSetUpCtrl.cmbPOClass.SelectedValue))), _
                                                                                              vendorID, SelectedVendorStyles.Distinct.ToList, CDate(eCommPreTurnInSetUpCtrl.dpStartShipDate.SelectedDate), includeOnlyApprovedItems).ToList()

                If eCommPreTurnInSetUpCtrl.cmbVendorStyle.SelectedValue <> "" Then
                    SelectedVendorStyles.Remove(eCommPreTurnInSetUpCtrl.cmbVendorStyle.SelectedValue)
                End If

                grdResultList.Visible = True
            End If

        ElseIf eCommPreTurnInSetUpCtrl.ResultsTabRadPanelBar.Items(1).Expanded Then
            ' Department is required.
            If eCommPreTurnInSetUpCtrl.SelectedDepartmentId = 0 Then
                MessagePanel1.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1010)
                grdResultList.Visible = False
            Else

                For Each item In eCommPreTurnInSetUpCtrl.lbVendorStyles.Items
                    SelectedVendorStyles.Add(CType(item, ListItem).Text.Trim())
                Next

                If Not SelectedVendorStyles Is Nothing AndAlso SelectedVendorStyles.Count = 0 Then
                    vendorID = eCommPreTurnInSetUpCtrl.SelectedVendorId
                    departmentID = eCommPreTurnInSetUpCtrl.SelectedDepartmentId
                End If

                If eCommPreTurnInSetUpCtrl.cmbVendorStyle.SelectedValue <> "" AndAlso Not SelectedVendorStyles.Contains(eCommPreTurnInSetUpCtrl.cmbVendorStyle.SelectedValue.Trim()) Then
                    SelectedVendorStyles.Add(eCommPreTurnInSetUpCtrl.cmbVendorStyle.SelectedValue.Trim)
                End If

                turnInFilter = GetTurnInFilters()

                ResultsTabISNs = _TUEcommSetupCreate.GetAllEcommSetupCreateResultsByHeirarchy(CurrentBatch.AdNumber, CurrentBatch.PageNumber, eCommPreTurnInSetUpCtrl.PriceStatusCodes, _
                                                                                              departmentID, eCommPreTurnInSetUpCtrl.SelectedClassId, _
                                                                                              eCommPreTurnInSetUpCtrl.SelectedSubClassId, vendorID, _
                                                                                              SelectedVendorStyles.Distinct.ToList, eCommPreTurnInSetUpCtrl.SelectedACode1, _
                                                                                              eCommPreTurnInSetUpCtrl.SelectedACode2, eCommPreTurnInSetUpCtrl.SelectedACode3, _
                                                                                              eCommPreTurnInSetUpCtrl.SelectedACode4, eCommPreTurnInSetUpCtrl.SelectedYear, _
                                                                                              eCommPreTurnInSetUpCtrl.SelectedSeasonId, eCommPreTurnInSetUpCtrl.SelectedCreatedSince, turnInFilter).ToList()

                If eCommPreTurnInSetUpCtrl.cmbVendorStyle.SelectedValue <> "" Then
                    SelectedVendorStyles.Remove(eCommPreTurnInSetUpCtrl.cmbVendorStyle.SelectedValue)
                End If

                grdResultList.Visible = True
            End If
        Else
            If eCommPreTurnInSetUpCtrl.SelectedISNs.Count = 0 _
                And eCommPreTurnInSetUpCtrl.txtISN.Text.Trim = "" Then

                MessagePanel1.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1009)
                grdResultList.Visible = False
            Else
                If eCommPreTurnInSetUpCtrl.SelectedISNs.Count > 0 Then
                    SelISNList = eCommPreTurnInSetUpCtrl.SelectedISNs
                End If

                If eCommPreTurnInSetUpCtrl.txtISN.Text.Trim <> "" Then
                    If ValidateISN(eCommPreTurnInSetUpCtrl.txtISN.Text.Trim, False) Then
                        SelISNList.Add(eCommPreTurnInSetUpCtrl.txtISN.Text.Trim)
                    Else
                        IsValid = False
                    End If
                End If

                If IsValid Then
                    turnInFilter = GetTurnInFilters()
                    ResultsTabISNs = _TUEcommSetupCreate.GetAllEcommSetupCreateResultsByISNs(CurrentBatch.AdNumber, CurrentBatch.PageNumber, SelISNList.Distinct.ToList, SelReserveISNList.Distinct.ToList, turnInFilter).ToList
                    grdResultList.Visible = True
                Else
                    MessagePanel1.ErrorMessage = "Invalid ISN or Reserve ISN."
                    grdResultList.Visible = False
                End If

                SelISNList.Remove(eCommPreTurnInSetUpCtrl.txtISN.Text.Trim)

            End If
        End If
    End Sub

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
        Return _TUEcommSetupCreate.GetISNExists(ISN, IsReserve)
    End Function

    Private Sub grdResultList_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdResultList.DataBound
        For Each gdi As GridDataItem In grdResultList.MasterTableView.Items
            Dim ISN As Decimal = CDec(gdi.GetDataKeyValue("ISN"))
            If SelectedMerch.Find(Function(x) x.ISN = ISN) IsNot Nothing Then
                gdi.Expanded = True
            End If
        Next
    End Sub

    Private Sub grdResultList_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdResultList.NeedDataSource

        grdResultList.DataSource = ResultsTabISNs
        grdResultList.MasterTableView.DataKeyNames = New String() {"ISN", "IsTurnedInEcomm", "AlreadyProcessed"}

    End Sub

    Private Sub grdResultList_DetailTableDataBind(ByVal source As Object, ByVal e As Telerik.Web.UI.GridDetailTableDataBindEventArgs) Handles grdResultList.DetailTableDataBind
        Dim parentItem As GridDataItem = CType(e.DetailTableView.ParentItem, GridDataItem)
        Dim turnInFilter As TUFilter
        Dim colors As List(Of EcommSetupCreateInfo) = Nothing
        Dim includeOnlyApprovedItems As Boolean = False
        If parentItem.Edit Then
            Return
        End If

        If (e.DetailTableView.Name = "grdSecondLevel") Then   'second level data bind 
            Dim ISN As Integer = CInt(CType(e.DetailTableView.ParentItem, GridDataItem).GetDataKeyValue("ISN"))
            turnInFilter = GetTurnInFilters()
            'Only approved items should be displayed when Filter By PO start ship date LHN is selected
            If eCommPreTurnInSetUpCtrl.ResultsTabRadPanelBar.Items(0).Expanded Then
                includeOnlyApprovedItems = turnInFilter.AvailableForTurnIn AndAlso Not turnInFilter.NotAvailableForTurnIn

                colors = _TUEcommSetupCreate.GetApprovedEcommSetupCreateDetailByISN(ISN, CDate(eCommPreTurnInSetUpCtrl.dpStartShipDate.SelectedDate), includeOnlyApprovedItems)
            Else
                colors = _TUEcommSetupCreate.GetAllEcommSetupCreateDetailByISN(ISN, turnInFilter)
            End If
            e.DetailTableView.DataSource = colors
            e.DetailTableView.DataKeyNames = New String() {"ISN", "ColorCode", "ColorDesc", "IsTurnedInEcomm", "AlreadyProcessed"}
        End If

    End Sub

    Private Sub grdResultList_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdResultList.ItemDataBound
        If (TypeOf e.Item Is GridDataItem) Then
            Dim gdi As GridDataItem = DirectCast(e.Item, GridDataItem)
            Dim parentItem As GridDataItem = CType(gdi.OwnerTableView.ParentItem, GridDataItem)
            If parentItem IsNot Nothing Then
                Dim ISN As Decimal = CDec(parentItem.GetDataKeyValue("ISN").ToString)
                Dim cc As String = gdi.GetDataKeyValue("ColorCode").ToString
                Dim cd As String = gdi.GetDataKeyValue("ColorDesc").ToString
                If TryCast(parentItem.FindControl("chkISNLevel"), CheckBox).Checked Then
                    parentItem.Expanded = True
                    DirectCast(gdi.FindControl("chkColorLevel"), CheckBox).Checked = True
                    AddSelectedMerchandise(ISN, cc, cd)
                Else
                    If SelectedMerch.Find(Function(x) x.ISN = ISN And x.ColorCode = cc And x.ColorDesc = cd) IsNot Nothing Then
                        DirectCast(gdi.FindControl("chkColorLevel"), CheckBox).Checked = True
                    End If
                End If

            Else
                'Set the checkbox at the ISN level
                Dim ISN As Decimal = CDec(gdi.GetDataKeyValue("ISN"))
                DirectCast(gdi.FindControl("chkISNLevel"), CheckBox).Checked = ResultsTabISNs.Where(Function(x) x.Selected = True And x.ISN = ISN).Count > 0
            End If

            If CBool(gdi.GetDataKeyValue("AlreadyProcessed")) Then
                gdi.BackColor = Drawing.Color.Yellow
            End If

            'Show/Hide Warning Icon with a ToolTip.
            If gdi.GetDataKeyValue("IsTurnedInEcomm").ToString = "Y" Then
                CType(gdi("TUWarning").Controls(0), Image).ImageUrl = "~/Images/Warning.png"
                CType(gdi("TUWarning").Controls(0), Image).Visible = True
                gdi("TUWarning").ToolTip = "Already turned in"
            Else
                CType(gdi("TUWarning").Controls(0), Image).ImageUrl = ""
                CType(gdi("TUWarning").Controls(0), Image).Visible = False
            End If

            'Set Tool Tips
            gdi("IsTurnedInPrint").ToolTip = DirectCast(gdi("IsTurnedInPrint").FindControl("hdnAdNoP"), HiddenField).Value
            gdi("IsTurnedInEcomm").ToolTip = DirectCast(gdi("IsTurnedInEcomm").FindControl("hdnAdNoE"), HiddenField).Value
        ElseIf (TypeOf e.Item Is GridHeaderItem) Then
            Dim ghi As GridHeaderItem = DirectCast(e.Item, GridHeaderItem)
            Dim parentItem As GridDataItem = DirectCast(ghi.OwnerTableView.ParentItem, GridDataItem)
            If parentItem IsNot Nothing Then
                'check the header item of the child grid
                DirectCast(ghi.FindControl("chkAll"), CheckBox).Checked = (ResultsTabISNs.Where(Function(x) x.Selected = True And x.ISN = CDec(parentItem.GetDataKeyValue("ISN"))).Count > 0)
            Else
                'Check the header item of the parent grid
                DirectCast(ghi.FindControl("chkAll"), CheckBox).Checked = (ResultsTabISNs.Where(Function(x) x.Selected = False).Count = 0)
            End If

        End If

    End Sub

#End Region

#Region "RESULTS TAB: Checkboxes"

    Private Sub EnableDisableISNTab()
        If AllSelectedISNs.Count > 0 Then
            rtsAPAdjustment.FindTabByText("ISN Level").Visible = True
        Else
            rtsAPAdjustment.FindTabByText("ISN Level").Visible = False
            rtsAPAdjustment.FindTabByText("Color/Size Level").Visible = False
        End If
    End Sub

    Protected Sub ToggleSelectALL(ByVal sender As Object, ByVal e As EventArgs)
        Dim headerCheckBox As CheckBox = TryCast(sender, CheckBox)

        'Check all visible items on the page
        For Each dataItem As GridDataItem In grdResultList.MasterTableView.Items
            TryCast(dataItem.FindControl("chkISNLevel"), CheckBox).Checked = headerCheckBox.Checked
            dataItem.Expanded = False
        Next

        'Check or uncheck AllISNs list
        For Each ISNItem As EcommSetupCreateInfo In ResultsTabISNs
            ISNItem.Selected = headerCheckBox.Checked
        Next

        'Clear selected colors because it's not needed if we know all ISNs are selected or not.
        SelectedMerch.Clear()

        'Enable/disable next tab
        EnableDisableISNTab()
    End Sub

    Protected Sub ToggleSelectISN(ByVal sender As Object, ByVal e As EventArgs)
        Dim cbx As CheckBox = TryCast(sender, CheckBox)
        Dim gdi As GridDataItem = DirectCast(cbx.NamingContainer, GridDataItem)

        ToggleISNLevel(gdi, cbx.Checked)
    End Sub

    Protected Sub ToggleSelectISNFromNestedGrid(ByVal sender As Object, ByVal e As EventArgs)
        Dim cbx As CheckBox = TryCast(sender, CheckBox)
        Dim gdi As GridDataItem = DirectCast(DirectCast(cbx.NamingContainer, GridHeaderItem).OwnerTableView.ParentItem, GridDataItem)
        TryCast(gdi.FindControl("chkISNLevel"), CheckBox).Checked = cbx.Checked

        ToggleISNLevel(gdi, cbx.Checked)
    End Sub

    Private Sub ToggleISNLevel(ByVal gdi As GridDataItem, ByVal isChecked As Boolean)
        Dim ISN As Decimal = CDec(gdi.GetDataKeyValue("ISN").ToString)

        If gdi.ChildItem.NestedTableViews(0).Items.Count > 0 Then
            'If child grid is showing, check or uncheck those items
            For Each dataItem As GridDataItem In gdi.ChildItem.NestedTableViews(0).Items
                TryCast(dataItem.FindControl("chkColorLevel"), CheckBox).Checked = isChecked
                If isChecked Then
                    Dim cc As String = dataItem.GetDataKeyValue("ColorCode").ToString
                    Dim cd As String = dataItem.GetDataKeyValue("ColorDesc").ToString

                    AddSelectedMerchandise(ISN, cc, cd)
                End If
            Next

            'Set the header item checkbox on the child grid
            Dim childGridHeaderItem As GridHeaderItem = TryCast(gdi.ChildItem.NestedTableViews(0).GetItems(GridItemType.Header)(0), GridHeaderItem)
            TryCast(childGridHeaderItem.FindControl("chkAll"), CheckBox).Checked = isChecked
        End If

        'Update the ISN in the AllISns list
        ResultsTabISNs.Where(Function(x) x.ISN = ISN).FirstOrDefault.Selected = isChecked

        'Clear selected colors for this ISN
        If Not isChecked Then
            SelectedMerch.RemoveAll(Function(x) x.ISN = ISN)

        End If

        'Sets Header checkbox only if all items are selected
        Dim headerItem As GridHeaderItem = TryCast(grdResultList.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        TryCast(headerItem.FindControl("chkAll"), CheckBox).Checked = (ResultsTabISNs.Where(Function(x) x.Selected = False).Count = 0)

        EnableDisableISNTab()
    End Sub

    Protected Sub ToggleColorLevel(ByVal sender As Object, ByVal e As EventArgs)
        Dim cbx As CheckBox = TryCast(sender, CheckBox)
        Dim currentColorDataItem As GridDataItem = DirectCast(cbx.NamingContainer, GridDataItem)
        Dim isChecked As Boolean = cbx.Checked
        Dim ColorTable As GridTableView = currentColorDataItem.OwnerTableView

        Dim checkISN As Boolean = True
        'Loop through colors
        For Each dataItem As GridDataItem In ColorTable.Items
            If Not TryCast(dataItem.FindControl("chkColorLevel"), CheckBox).Checked Then
                checkISN = False
                Exit For
            End If
        Next

        'Set the Header checkbox on the child grid
        Dim childGridHeaderItem As GridHeaderItem = TryCast(ColorTable.GetItems(GridItemType.Header)(0), GridHeaderItem)
        TryCast(childGridHeaderItem.FindControl("chkAll"), CheckBox).Checked = checkISN

        'Set the ISN level checkbox
        Dim parentItem As GridDataItem = DirectCast(ColorTable.ParentItem, GridDataItem)
        TryCast(parentItem.FindControl("chkISNLevel"), CheckBox).Checked = checkISN

        'Update the ISN in the AllISns list
        Dim ISN As Decimal = CDec(parentItem.GetDataKeyValue("ISN").ToString)
        ResultsTabISNs.Where(Function(x) x.ISN = ISN).FirstOrDefault.Selected = checkISN

        Dim cc As String = currentColorDataItem.GetDataKeyValue("ColorCode").ToString
        Dim cd As String = currentColorDataItem.GetDataKeyValue("ColorDesc").ToString

        If isChecked Then
            AddSelectedMerchandise(ISN, cc, cd)
        Else
            SelectedMerch.RemoveAll(Function(x) x.ISN = ISN And x.ColorCode = cc And x.ColorDesc = cd)
        End If

        'Sets Header checkbox only if all items are selected
        Dim headerItem As GridHeaderItem = TryCast(grdResultList.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
        TryCast(headerItem.FindControl("chkAll"), CheckBox).Checked = (ResultsTabISNs.Where(Function(x) x.Selected = False).Count = 0)
        EnableDisableISNTab()
    End Sub

    Private Sub AddSelectedMerchandise(ByVal ISN As Decimal, ByVal merchColorCode As String, ByVal merchColorDesc As String)
        Dim merch As New EcommSetupCreateInfo
        With merch
            .ISN = ISN
            .ColorCode = merchColorCode
            .ColorDesc = merchColorDesc
            .Saved = True
            Dim lookupISNinfo As EcommSetupCreateInfo = ResultsTabISNs.Union(ResultsTabISNs.Where(Function(x) x.Selected = True)).Union(SelectedMerch).OrderBy(Function(x) x.Sequence).Distinct.ToList.Find(Function(x) x.ISN = ISN)
            'these fields are needed when only certain colors are selected
            If Not IsNothing(lookupISNinfo) Then
                .DeptId = lookupISNinfo.DeptId
                .VendorStyleNumber = lookupISNinfo.VendorStyleNumber
                .ISNDesc = lookupISNinfo.ISNDesc

                'Not sure which of these fields are actually needed, but adding them anyway.
                .BatchNumber = lookupISNinfo.BatchNumber
                .AdNumber = lookupISNinfo.AdNumber
                .PageNumber = lookupISNinfo.PageNumber
                .ACode = lookupISNinfo.ACode
                .VendorId = lookupISNinfo.VendorId
                .VendorName = lookupISNinfo.VendorName
                .IsReserve = lookupISNinfo.IsReserve
                .SellYear = lookupISNinfo.SellYear
                .SellSeason = lookupISNinfo.SellSeason
                .OnOrder = lookupISNinfo.OnOrder
                .OnHand = lookupISNinfo.OnHand
                .BrandId = lookupISNinfo.BrandId
                .LabelId = lookupISNinfo.LabelId
                .ProductDetailId1 = lookupISNinfo.ProductDetailId1
                .ProductDetailId2 = lookupISNinfo.ProductDetailId2
                .ProductDetailId3 = lookupISNinfo.ProductDetailId3
                .DeptDesc = lookupISNinfo.DeptDesc
                .BuyerId = lookupISNinfo.BuyerId
                .BuyerName = lookupISNinfo.BuyerName
                .BuyerExt = lookupISNinfo.BuyerExt
                .BrandDesc = lookupISNinfo.BrandDesc
                .LabelDesc = lookupISNinfo.LabelDesc
                .ProductDetailDesc1 = lookupISNinfo.ProductDetailDesc1
                .ProductDetailDesc2 = lookupISNinfo.ProductDetailDesc2
                .ProductDetailDesc3 = lookupISNinfo.ProductDetailDesc3
                .PatternDesc = lookupISNinfo.PatternDesc
                .ExistingWebStyle = lookupISNinfo.ExistingWebStyle
                .FeatureImageNum = lookupISNinfo.FeatureImageNum
            End If
        End With
        If SelectedMerch.Find(Function(x) x.ISN = merch.ISN And x.ColorCode = merch.ColorCode And x.ColorDesc = merch.ColorDesc) Is Nothing Then
            If Not IsNothing(merch.ISNDesc) Then
                SelectedMerch.Add(merch)
            End If
        End If
    End Sub
    ''' <summary>
    ''' Gets the default ship days for the fitlers mentioned and sets the date for Start Ship Date picker
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetDefaultShipDays()
        Dim defaultShipDays As Integer = 0
        Dim vendorStyles As New List(Of String)

        For Each item In eCommPreTurnInSetUpCtrl.lbPOVendorStyles.Items
            vendorStyles.Add(CType(item, ListItem).Text.Trim())
        Next

        If eCommPreTurnInSetUpCtrl.cmbPOVendorStyle.SelectedValue <> String.Empty AndAlso Not vendorStyles.Contains(eCommPreTurnInSetUpCtrl.cmbPOVendorStyle.SelectedValue.Trim()) Then
            vendorStyles.Add(eCommPreTurnInSetUpCtrl.cmbPOVendorStyle.SelectedValue)
        End If

        defaultShipDays = _TUEcommSetupCreate.GetCFGDefaultShipDays(If(eCommPreTurnInSetUpCtrl.cmbDMM.SelectedValue = String.Empty, 0, CInt(eCommPreTurnInSetUpCtrl.cmbDMM.SelectedValue)),
                                                                    If(eCommPreTurnInSetUpCtrl.cmbPOBuyer.SelectedValue = String.Empty, 0, CInt(eCommPreTurnInSetUpCtrl.cmbPOBuyer.SelectedValue)),
                                                                    If(eCommPreTurnInSetUpCtrl.cmbPODept.SelectedValue = String.Empty, 0, CInt(eCommPreTurnInSetUpCtrl.cmbPODept.SelectedValue)),
                                                                    vendorStyles)
        If defaultShipDays > 0 Then
            eCommPreTurnInSetUpCtrl.dpStartShipDate.SelectedDate = Date.Now().AddDays(defaultShipDays)
        End If
    End Sub
#End Region

#Region "ISN LEVEL TAB: Events"
    Private Sub SetupISNTab()
        Dim ISNs As New List(Of String)

        Session("PreTurnInSetUpCreate.PrevTabIndex") = Session("PreTurnInSetUpCreate.CurrTabIndex")
        Session("PreTurnInSetUpCreate.CurrTabIndex") = "4"

        ShowHideTabs("Ad List", rtsAPAdjustment.FindTabByText("Ad List").Visible)
        ShowHideTabs("Ad Level", rtsAPAdjustment.FindTabByText("Ad Level").Visible)
        ShowHideTabs("Result List", rtsAPAdjustment.FindTabByText("Result List").Visible)
        ShowHideTabs("Killed Items", rtsAPAdjustment.FindTabByText("Killed Items").Visible)
        ShowHideTabs("ISN Level", True)
        ShowHideTabs("Color/Size Level", rtsAPAdjustment.FindTabByText("Color/Size Level").Visible)

        rtsAPAdjustment.FindTabByText("ISN Level").Selected = True
        rmpPreTurnInCreate.SelectedIndex = 4

        ShowHideButtons("Reset", True)
        ShowHideButtons("Sort", False)
        ShowHideButtons("Level Down", False)
        ShowHideButtons("Level Up", False)
        ShowHideButtons("Save", True)
        ShowHideButtons("Add To Batch", False)
        ShowHideButtons("Retrieve", False)
        ShowHideButtons("Print Labels", False)
        ShowHideButtons("Print Report", False)
        ShowHideButtons("Submit", False)
        ShowHideButtons("Edit All", False)
        ShowHideButtons("Save All", False)
        ShowHideButtons("Cancel All", False)


        With cmbISNLabel
            .DataSource = AllSelectedISNs.Select(Function(x) New With {Key .ISN = x.ISN, .ISNText = If(x.Saved, "* ", "") & x.ISN & " - " & x.ISNDesc & " - " & x.VendorStyleNumber}). _
                        Distinct.OrderBy(Function(x) x.ISN).ToList
            .DataValueField = "ISN"
            .DataTextField = "ISNText"
            .DataBind()
            .SelectedIndex = 0
        End With

        BindISNTabElements()

        If Request.QueryString("Action").ToUpper = Modes.INQUIRY.ToString Then
            'Disable all the Buttons, Dropdowns, Check Boxes, and Radio Buttons in Inquiry mode.
            EnableDisableButtons("Save", False)
            cmbLabel.Enabled = False
            cmbSizeCategory.Enabled = False
            cmbModelCategory.Enabled = False
            cmbWebCategoriesLevel1.Enabled = False
            cmbWebCategoriesLevel2.Enabled = False
            cmbWebCategoriesLevel3.Enabled = False
            cmbWebCategoriesLevel4.Enabled = False
            cmbWebCategoriesLevel5.Enabled = False
            cmbWebCategoriesLevel6.Enabled = False
            rblVendorApproval.Enabled = False
            rblAdditionalColorsSamples.Enabled = False
            cmbTurnInUsageIndicator.Enabled = False
            grdWebCategories.Enabled = False
        Else
            cmbWebCategoriesLevel1.Focus()
        End If

        EnableDisableColorSizeTab()
    End Sub

    Private Sub BindISNTabElements()
        Dim selectedISN As EcommSetupCreateInfo = AllSelectedISNs.Where(Function(x) x.ISN = CDec(CurrentISN)).FirstOrDefault()
        Dim ISNPositionInList As Integer = ISNTabList.FindIndex(Function(x) x = CStr(CurrentISN)) + 1
        lblPageNumber.Text = "ISN " & ISNPositionInList & " of " & ISNTabList.Count
        Dim _TULabel As New TULabel

        Dim selectedISNDetail As EcommSetupCreateInfo = _TUEcommSetupCreate.GetISNLevelDetailByISN(selectedISN.ISN, selectedISN.IsReserve)

        'Save the current ISN Index. It will be used in SelectedIndexChanged event.
        Session("PreTurnInSetUpCreate.CurrISNIndex") = cmbISNLabel.SelectedIndex

        With ddlCopyWebCatsFromISN
            .DataSource = AllSelectedISNs.Where(Function(x) x.ISN <> selectedISN.ISN).Select(Function(x) New With {Key .ISN = x.ISN, .ISNText = x.ISN & " - " & x.ISNDesc & " - " & x.VendorStyleNumber}).Distinct.OrderBy(Function(x) x.ISN).ToList
            .DataValueField = "ISN"
            .DataTextField = "ISNText"
            .DataBind()
            .Items.Insert(0, New ListItem("", ""))
            .ClearSelection()
            .Text = ""
        End With

        'Label
        With cmbLabel
            .DataSource = _TULabel.GetLabelsByBrand(CStr(selectedISNDetail.BrandId))
            .DataValueField = "LabelId"
            .DataTextField = "LabelDesc"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem("", "0"))
            .ClearSelection()
            .Text = ""
        End With

        cmbLabel.SelectedIndex = 0
        If cmbLabel.FindItemByValue(CStr(selectedISNDetail.LabelId)) IsNot Nothing AndAlso selectedISN.Saved Then
            If cmbLabel.FindItemByValue(CStr(selectedISNDetail.LabelId)).Value.ToString <> "0" Then
                cmbLabel.FindItemByValue(CStr(selectedISNDetail.LabelId)).Selected = True
            Else
                If cmbLabel.Items.Count = 2 Then cmbLabel.SelectedIndex = 1 ' only one item, select it.
            End If
        Else
            If cmbLabel.Items.Count = 2 Then cmbLabel.SelectedIndex = 1 ' only one item, select it.
        End If

        'Size Category
        With cmbSizeCategory
            .DataSource = _TUTMS900PARAMETER.GetAllSizeCategories
            .DataValueField = "CharIndex"
            .DataTextField = "LongDesc"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
            .ClearSelection()
            .Text = ""
        End With

        If cmbSizeCategory.FindItemByValue(selectedISNDetail.SizeCategoryCode) IsNot Nothing Then
            cmbSizeCategory.FindItemByValue(selectedISNDetail.SizeCategoryCode).Selected = True
        End If

        'Model Category
        With cmbModelCategory
            .DataSource = _TUTMS900PARAMETER.GetAllModelCategories(selectedISNDetail.DeptId)
            .DataValueField = "CharIndex"
            .DataTextField = "LongDesc"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
            .ClearSelection()
            .Text = ""
        End With

        If cmbModelCategory.FindItemByValue(selectedISNDetail.ModelCategoryCode) IsNot Nothing Then
            cmbModelCategory.FindItemByValue(selectedISNDetail.ModelCategoryCode).Selected = True
        End If

        'Vendor Approval
        rblVendorApproval.ClearSelection()
        If selectedISNDetail.VndApprovalFlg = "Y"c Then
            rblVendorApproval.Items(0).Selected = True
        Else
            rblVendorApproval.Items(1).Selected = True
        End If

        'Additional Colors/Samples
        rblAdditionalColorsSamples.ClearSelection()
        If selectedISNDetail.AddnColorSamplesFlg = "Y"c Then
            rblAdditionalColorsSamples.Items(0).Selected = True
        Else
            rblAdditionalColorsSamples.Items(1).Selected = True
        End If

        'Turn-In Usage Indicator
        cmbTurnInUsageIndicator.ClearSelection()
        Select Case selectedISNDetail.TurnInUsageInd
            Case 0
                cmbTurnInUsageIndicator.Items(0).Selected = False
                cmbTurnInUsageIndicator.Items(1).Selected = False
            Case 1
                cmbTurnInUsageIndicator.Items(0).Selected = True
                cmbTurnInUsageIndicator.Items(1).Selected = False
            Case 2
                cmbTurnInUsageIndicator.Items(0).Selected = False
                cmbTurnInUsageIndicator.Items(1).Selected = True
            Case 3
                cmbTurnInUsageIndicator.Items(0).Selected = True
                cmbTurnInUsageIndicator.Items(1).Selected = True
        End Select

        lblISNLongDescText.Text = selectedISNDetail.ISNDesc
        lblUserModifyDateText.Text = selectedISNDetail.LastModBy
        lblVendorText.Text = selectedISNDetail.VendorId & " - " & selectedISNDetail.VendorName
        lblBrandText.Text = selectedISNDetail.BrandId & " - " & selectedISNDetail.BrandDesc
        lblVendorStyleLabelText.Text = selectedISNDetail.VendorStyleNumber
        lblGenericClassTxt.Text = If(selectedISNDetail.GenericClassId = 0, "", selectedISNDetail.GenericClassId.ToString) & " - " & selectedISNDetail.GenericClass
        lblGenericSubclassText.Text = If(selectedISNDetail.GenericClassId = 0, "", selectedISNDetail.GenericClassId.ToString) & " - " & selectedISNDetail.GenericClass
        lblProductDetail1Text.Text = If(selectedISNDetail.ProductDetailId1 = 0, "", selectedISNDetail.ProductDetailId1.ToString) & " - " & selectedISNDetail.ProductDetailDesc1
        lblProductDetail2Text.Text = If(selectedISNDetail.ProductDetailId2 = 0, "", selectedISNDetail.ProductDetailId2.ToString) & " - " & selectedISNDetail.ProductDetailDesc2
        lblProductDetail3Text.Text = If(selectedISNDetail.ProductDetailId3 = 0, "", selectedISNDetail.ProductDetailId3.ToString) & " - " & selectedISNDetail.ProductDetailDesc3
        lblPatternText.Text = selectedISNDetail.PatternDesc

        lblDeptText.Text = selectedISNDetail.DeptId & " - " & selectedISNDetail.DeptDesc
        lblBuyerExtText.Text = selectedISNDetail.BuyerName & "/" & selectedISNDetail.BuyerExt
        lblExistingWebStyleText.Text = selectedISNDetail.ExistingWebStyle

        litFeatureImageText.Text = ""

        If Not String.IsNullOrEmpty(selectedISNDetail.FeatureImageNum) Then
            For Each ImageID As String In selectedISNDetail.FeatureImageNum.Split(","c)
                litFeatureImageText.Text += "<a target=""_blank"" href=""http://www.bostonstore.com/shop/?catalogId=10051&storeId=10001&langId=-1&query=" & ImageID.Trim & """>" & ImageID.Trim & "</a>, "
            Next
            litFeatureImageText.Text = litFeatureImageText.Text.Substring(0, litFeatureImageText.Text.LastIndexOf(","c))
        End If

        lblSellingLocText.Text = selectedISNDetail.SellingLocation
        lblFabricationText.Text = selectedISNDetail.Fabrication
        lblMadeInText.Text = selectedISNDetail.ImportedOrUSA
        lblDropShipText.Text = selectedISNDetail.DropShipFlg

        If cmbWebCategoriesLevel1.Items.Count = 0 Then
            BindWebCat(cmbWebCategoriesLevel1, "0")
            DisableWebCatSelections(cmbWebCategoriesLevel2)
            DisableWebCatSelections(cmbWebCategoriesLevel3)
            DisableWebCatSelections(cmbWebCategoriesLevel4)
            DisableWebCatSelections(cmbWebCategoriesLevel5)
            DisableWebCatSelections(cmbWebCategoriesLevel6)

        End If

        cmbTurnInUsageIndicator.DataSource = ""

        SelectedWebCats.Clear()
        SelectedWebCats = _TUWebCat.GetWebCatByISN(selectedISN.ISN).ToList
        For Each wc As WebCat In SelectedWebCats
            Dim CatCode As Integer = wc.CategoryCode
            If AllWebCats.Where(Function(x) x.CategoryCode = CatCode).Count = 0 Then
                Dim objGetApplicationObjectsService As New GetGlobalObjectsService
                objGetApplicationObjectsService.GetAllApplicationObjects()  'this will create/populate all application objects.
                Exit For
            End If
        Next

        SelectedWebCats = SelectedWebCats.Join(AllWebCats, Function(x) x.CategoryCode, Function(y) y.CategoryCode, Function(x, y) New WebCat With {.CategoryCode = x.CategoryCode, .DefaultCategoryFlag = x.DefaultCategoryFlag, .CategoryLongDesc = y.CategoryLongDesc}).ToList

        grdWebCategories.Rebind()
        'BindWebCategoriesGrid()
        EnableDisableWebCatAddButtons()

        'To track ISN tab level changes.
        hdnISNTabChanges.Value = cmbLabel.SelectedItem.Text & cmbSizeCategory.SelectedItem.Text & rblVendorApproval.SelectedValue _
                                & cmbModelCategory.SelectedItem.Text & rblAdditionalColorsSamples.SelectedValue

        For Each li As ListItem In cmbTurnInUsageIndicator.Items
            If li.Selected Then
                hdnISNTabChanges.Value += li.Value
            End If
        Next
    End Sub

    Private Sub EnableDisableColorSizeTab()
        'Dim ISNs As New List(Of String)
        'Dim SavedRecordCount As Integer = 0

        'If ResultsTabISNs.Where(Function(x) x.Selected = True).Count > 0 Then
        '    ISNs = ResultsTabISNs.Where(Function(x) x.Selected = True).Select(Function(x) x.ISN.ToString).ToList
        '    SavedRecordCount = ResultsTabISNs.Where(Function(x) x.Selected = True).Count
        'Else
        '    ISNs = SelectedColors.Select(Function(x) x.ISN.ToString).Distinct.ToList
        '    SavedRecordCount = SelectedColors.Select(Function(x) x.ISN.ToString).Distinct.Count
        'End If

        'If SavedRecordCount > 0 AndAlso SavedRecordCount = _TUEcommSetupCreate.GetSavedISNCount(ISNs) Then
        '    rtsAPAdjustment.FindTabByText("Color/Size Level").Visible = True
        'End If

        If CurrentBatch.BatchId <> 0 Then
            rtsAPAdjustment.FindTabByText("Color/Size Level").Visible = True
        End If
    End Sub

    Private Sub SaveISNData()
        Dim selectedISN As EcommSetupCreateInfo = AllSelectedISNs.Where(Function(x) x.ISN = CDec(CurrentISN)).FirstOrDefault()
        Dim turnInFilter As TUFilter
        If AllSelectedISNs.Where(Function(x) x.ISN = CDec(CurrentISN)).FirstOrDefault().Saved = False Then
            AllSelectedISNs.Where(Function(x) x.ISN = CDec(CurrentISN)).FirstOrDefault().Saved = True
            AllSelectedISNs.Where(Function(x) x.ISN = CDec(CurrentISN)).FirstOrDefault().Sequence = AllSelectedISNs.Max(Function(x) x.Sequence) + 1
            cmbISNLabel.Items.FindByValue(CurrentISN).Text = "* " & cmbISNLabel.Items.FindByValue(CurrentISN).Text
            ShowHideTabs("Color/Size Level", True)
        End If

        _TUEcommSetupCreate.InsertISNData(selectedISN.ISN, CInt(cmbLabel.SelectedValue), selectedISN.IsReserve, cmbSizeCategory.SelectedValue, SelectedWebCats, SessionWrapper.UserID)

        Dim TurnInUsageCode As Integer = 0
        For Each li As ListItem In cmbTurnInUsageIndicator.Items
            If li.Selected Then
                TurnInUsageCode += CInt(li.Value)
            End If
        Next

        Dim MerchandiseList As List(Of EcommSetupCreateInfo) = SelectedMerch.Where(Function(x) x.ISN = selectedISN.ISN).ToList
        If MerchandiseList.Count = 0 Then
            turnInFilter = GetTurnInFilters()
            MerchandiseList = _TUEcommSetupCreate.GetAllEcommSetupCreateDetailByISN(selectedISN.ISN, turnInFilter)
            If CurrentBatch.BatchId > 0 AndAlso Not MerchandiseList Is Nothing AndAlso MerchandiseList.Count > 0 AndAlso
                Not MerchandiseList(0).SampleDetails Is Nothing AndAlso MerchandiseList(0).SampleDetails.SampleRequestCreateName.Contains("INDC\") Then
                MerchandiseList = MerchandiseList.FindAll(Function(x) x.BatchNumber = CurrentBatch.BatchId)
            End If
        End If

        For Each merch As EcommSetupCreateInfo In MerchandiseList
            Dim merchColorCode As String = merch.ColorCode
            If selectedISN.IsReserve Then
                merchColorCode = merch.ColorDesc
            End If

            CurrentBatch.BatchId = _TUEcommSetupCreate.InsertISNDataColorLevel(selectedISN.ISN, selectedISN.DeptId, _
                                                                               merchColorCode, TurnInUsageCode, _
                                                                               merch.ColorDesc + " " + selectedISN.ISNDesc, _
                                                                               lblAdNoText.Text, _
                                                                               lblPageNumberText.Text, selectedISN.IsReserve, _
                                                                               rblAdditionalColorsSamples.SelectedValue, _
                                                                               cmbModelCategory.SelectedValue, _
                                                                               rblVendorApproval.SelectedValue, _
                                                                               SessionWrapper.UserID, _
                                                                               CurrentBatch.BatchId)


            AddSelectedMerchandise(selectedISN.ISN, merchColorCode, merch.ColorDesc)


        Next
        lblBatchIdText.Text = CStr(CurrentBatch.BatchId)

        EnableDisableColorSizeTab()

        mpPreTurnInCreate.ErrorMessage = "Data Saved Successfully."
    End Sub

    Private Sub EnableDisableWebCatAddButtons()
        imgAddWebCategoriesLevel1.Visible = cmbWebCategoriesLevel1.Enabled AndAlso Not cmbWebCategoriesLevel2.Enabled AndAlso (cmbWebCategoriesLevel1.Items.Count - 1 > cmbWebCategoriesLevel1.Items.Where(Function(x) x.Text.ToLower.Contains("display only")).Count) AndAlso cmbWebCategoriesLevel1.SelectedIndex > 0 AndAlso Not cmbWebCategoriesLevel1.SelectedItem.Text.ToLower.Contains("display only")
        imgAddWebCategoriesLevel2.Visible = cmbWebCategoriesLevel2.Enabled AndAlso Not cmbWebCategoriesLevel3.Enabled AndAlso (cmbWebCategoriesLevel2.Items.Count - 1 > cmbWebCategoriesLevel2.Items.Where(Function(x) x.Text.ToLower.Contains("display only")).Count) AndAlso cmbWebCategoriesLevel2.SelectedIndex > 0 AndAlso Not cmbWebCategoriesLevel2.SelectedItem.Text.ToLower.Contains("display only")
        imgAddWebCategoriesLevel3.Visible = cmbWebCategoriesLevel3.Enabled AndAlso Not cmbWebCategoriesLevel4.Enabled AndAlso (cmbWebCategoriesLevel3.Items.Count - 1 > cmbWebCategoriesLevel3.Items.Where(Function(x) x.Text.ToLower.Contains("display only")).Count) AndAlso cmbWebCategoriesLevel3.SelectedIndex > 0 AndAlso Not cmbWebCategoriesLevel3.SelectedItem.Text.ToLower.Contains("display only")
        imgAddWebCategoriesLevel4.Visible = cmbWebCategoriesLevel4.Enabled AndAlso Not cmbWebCategoriesLevel5.Enabled AndAlso (cmbWebCategoriesLevel4.Items.Count - 1 > cmbWebCategoriesLevel4.Items.Where(Function(x) x.Text.ToLower.Contains("display only")).Count) AndAlso cmbWebCategoriesLevel4.SelectedIndex > 0 AndAlso Not cmbWebCategoriesLevel4.SelectedItem.Text.ToLower.Contains("display only")
        imgAddWebCategoriesLevel5.Visible = cmbWebCategoriesLevel5.Enabled AndAlso Not cmbWebCategoriesLevel6.Enabled AndAlso (cmbWebCategoriesLevel5.Items.Count - 1 > cmbWebCategoriesLevel5.Items.Where(Function(x) x.Text.ToLower.Contains("display only")).Count) AndAlso cmbWebCategoriesLevel5.SelectedIndex > 0 AndAlso Not cmbWebCategoriesLevel5.SelectedItem.Text.ToLower.Contains("display only")
        imgAddWebCategoriesLevel6.Visible = cmbWebCategoriesLevel6.Enabled AndAlso (cmbWebCategoriesLevel6.Items.Count - 1 > cmbWebCategoriesLevel6.Items.Where(Function(x) x.Text.ToLower.Contains("display only")).Count) AndAlso cmbWebCategoriesLevel6.SelectedIndex > 0 AndAlso Not cmbWebCategoriesLevel6.SelectedItem.Text.ToLower.Contains("display only")
    End Sub

    Private Sub cmbWebCategoriesLevel1_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbWebCategoriesLevel1.SelectedIndexChanged
        If cmbWebCategoriesLevel1.SelectedIndex > 0 Then
            BindWebCat(cmbWebCategoriesLevel2, cmbWebCategoriesLevel1.SelectedValue)
        Else
            DisableWebCatSelections(cmbWebCategoriesLevel2)
        End If
        DisableWebCatSelections(cmbWebCategoriesLevel3)
        DisableWebCatSelections(cmbWebCategoriesLevel4)
        DisableWebCatSelections(cmbWebCategoriesLevel5)
        DisableWebCatSelections(cmbWebCategoriesLevel6)
        EnableDisableWebCatAddButtons()
    End Sub

    Private Sub cmbWebCategoriesLevel2_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbWebCategoriesLevel2.SelectedIndexChanged
        If cmbWebCategoriesLevel2.SelectedIndex > 0 Then
            BindWebCat(cmbWebCategoriesLevel3, cmbWebCategoriesLevel2.SelectedValue)
        Else
            DisableWebCatSelections(cmbWebCategoriesLevel3)
        End If
        DisableWebCatSelections(cmbWebCategoriesLevel4)
        DisableWebCatSelections(cmbWebCategoriesLevel5)
        DisableWebCatSelections(cmbWebCategoriesLevel6)
        EnableDisableWebCatAddButtons()
    End Sub

    Private Sub cmbWebCategoriesLevel3_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbWebCategoriesLevel3.SelectedIndexChanged
        If cmbWebCategoriesLevel3.SelectedIndex > 0 Then
            BindWebCat(cmbWebCategoriesLevel4, cmbWebCategoriesLevel3.SelectedValue)
        Else
            DisableWebCatSelections(cmbWebCategoriesLevel4)
        End If
        DisableWebCatSelections(cmbWebCategoriesLevel5)
        DisableWebCatSelections(cmbWebCategoriesLevel6)
        EnableDisableWebCatAddButtons()
    End Sub

    Private Sub cmbWebCategoriesLevel4_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbWebCategoriesLevel4.SelectedIndexChanged
        If cmbWebCategoriesLevel4.SelectedIndex > 0 Then
            BindWebCat(cmbWebCategoriesLevel5, cmbWebCategoriesLevel4.SelectedValue)
        Else
            DisableWebCatSelections(cmbWebCategoriesLevel5)
        End If
        DisableWebCatSelections(cmbWebCategoriesLevel6)
        EnableDisableWebCatAddButtons()
    End Sub

    Private Sub cmbWebCategoriesLevel5_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbWebCategoriesLevel5.SelectedIndexChanged
        If cmbWebCategoriesLevel5.SelectedIndex > 0 Then
            BindWebCat(cmbWebCategoriesLevel6, cmbWebCategoriesLevel5.SelectedValue)
        Else
            DisableWebCatSelections(cmbWebCategoriesLevel6)
        End If
        EnableDisableWebCatAddButtons()
    End Sub

    Private Sub cmbWebCategoriesLevel6_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbWebCategoriesLevel6.SelectedIndexChanged
        EnableDisableWebCatAddButtons()
    End Sub

    Private Sub BindWebCat(ByVal cmb As RadComboBox, ByVal SelectedValue As String)
        cmb.Text = ""
        cmb.ClearSelection()
        Dim list As IList(Of WebCat) = _TUWebCat.GetWebCatByParentCde(CInt(SelectedValue))
        If list.Count > 0 Then
            With cmb
                .DataSource = list
                .DataTextField = "CategoryNameDisplayOnlyText"
                .DataValueField = "CategoryCode"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem())
                .Enabled = True
            End With
        Else
            cmb.Enabled = False
        End If
    End Sub

    Private Sub DisableWebCatSelections(ByVal cmb As RadComboBox)
        With cmb
            .Text = ""
            .ClearSelection()
            .Enabled = False
            .DataSource = ""
            .DataBind()
        End With
    End Sub

    Private Sub imgAddWebCategoriesLevel1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgAddWebCategoriesLevel1.Click
        If cmbWebCategoriesLevel1.Enabled = True AndAlso cmbWebCategoriesLevel1.SelectedItem.Text <> "" Then
            AddWebCat(CInt(cmbWebCategoriesLevel1.SelectedValue), cmbWebCategoriesLevel1.SelectedItem.Text)
        End If
    End Sub

    Private Sub imgAddWebCategoriesLevel2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgAddWebCategoriesLevel2.Click
        If cmbWebCategoriesLevel2.Enabled = True AndAlso cmbWebCategoriesLevel2.SelectedItem.Text <> "" Then
            AddWebCat(CInt(cmbWebCategoriesLevel2.SelectedValue), cmbWebCategoriesLevel1.SelectedItem.Text & " > " & cmbWebCategoriesLevel2.SelectedItem.Text)
        End If
    End Sub

    Private Sub imgAddWebCategoriesLevel3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgAddWebCategoriesLevel3.Click
        If cmbWebCategoriesLevel3.Enabled = True AndAlso cmbWebCategoriesLevel3.SelectedItem.Text <> "" Then
            AddWebCat(CInt(cmbWebCategoriesLevel3.SelectedValue), cmbWebCategoriesLevel1.SelectedItem.Text & " > " & cmbWebCategoriesLevel2.SelectedItem.Text & " > " & cmbWebCategoriesLevel3.SelectedItem.Text)
        End If
    End Sub

    Private Sub imgAddWebCategoriesLevel4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgAddWebCategoriesLevel4.Click
        If cmbWebCategoriesLevel4.Enabled = True AndAlso cmbWebCategoriesLevel4.SelectedItem.Text <> "" Then
            AddWebCat(CInt(cmbWebCategoriesLevel4.SelectedValue), cmbWebCategoriesLevel1.SelectedItem.Text & " > " & cmbWebCategoriesLevel2.SelectedItem.Text & " > " & cmbWebCategoriesLevel3.SelectedItem.Text & " > " & cmbWebCategoriesLevel4.SelectedItem.Text)
        End If
    End Sub

    Private Sub imgAddWebCategoriesLevel5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgAddWebCategoriesLevel5.Click
        If cmbWebCategoriesLevel5.Enabled = True AndAlso cmbWebCategoriesLevel5.SelectedItem.Text <> "" Then
            AddWebCat(CInt(cmbWebCategoriesLevel5.SelectedValue), cmbWebCategoriesLevel1.SelectedItem.Text & " > " & cmbWebCategoriesLevel2.SelectedItem.Text & " > " & cmbWebCategoriesLevel3.SelectedItem.Text & " > " & cmbWebCategoriesLevel4.SelectedItem.Text & " > " & cmbWebCategoriesLevel5.SelectedItem.Text)
        End If
    End Sub

    Private Sub imgAddWebCategoriesLevel6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgAddWebCategoriesLevel6.Click
        If cmbWebCategoriesLevel6.Enabled = True AndAlso cmbWebCategoriesLevel6.SelectedItem.Text <> "" Then
            AddWebCat(CInt(cmbWebCategoriesLevel6.SelectedValue), cmbWebCategoriesLevel1.SelectedItem.Text & " > " & cmbWebCategoriesLevel2.SelectedItem.Text & " > " & cmbWebCategoriesLevel3.SelectedItem.Text & " > " & cmbWebCategoriesLevel4.SelectedItem.Text & " > " & cmbWebCategoriesLevel5.SelectedItem.Text & " > " & cmbWebCategoriesLevel6.SelectedItem.Text)
        End If
    End Sub

    Private Sub AddWebCat(ByVal cmbValue As Integer, ByVal cmbText As String)
        If SelectedWebCats.Count < 6 Then
            Dim webcat As New WebCat
            webcat.CategoryCode = cmbValue
            webcat.CategoryLongDesc = cmbText.Replace(" (Display Only)", "")
            If SelectedWebCats.Where(Function(x) x.DefaultCategoryFlag = True).Count = 0 Then
                webcat.DefaultCategoryFlag = True
            End If

            If SelectedWebCats.Find(Function(x) x.CategoryCode = webcat.CategoryCode) Is Nothing Then
                SelectedWebCats.Add(webcat)
            End If
        Else
            MessagePanel1.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1012)
        End If
        'BindWebCategoriesGrid()
        grdWebCategories.Rebind()
    End Sub

    Protected Sub cbDefaultCategoryFlag_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim rb As RadioButton = CType(sender, RadioButton)
        Dim cc As Integer = CInt(CType(rb.NamingContainer, GridDataItem).GetDataKeyValue("CategoryCode"))
        If SelectedWebCats.Where(Function(x) x.DefaultCategoryFlag = True).Count > 0 Then
            SelectedWebCats.Find(Function(x) x.DefaultCategoryFlag = True).DefaultCategoryFlag = False
        End If
        SelectedWebCats.Find(Function(x) x.CategoryCode = cc).DefaultCategoryFlag = True
        ' BindWebCategoriesGrid()
        grdWebCategories.Rebind()
    End Sub

    'Private Sub BindWebCategoriesGrid()
    ' grdWebCategories.DataSource = SelectedWebCats
    ' grdWebCategories.DataBind()
    'grdWebCategories.MasterTableView.DataKeyNames = New String() {"CategoryCode"}
    'End Sub

    Private Sub grdWebCategories_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdWebCategories.DeleteCommand
        Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)
        Dim cc As Integer = CInt(item.GetDataKeyValue("CategoryCode"))
        If SelectedWebCats.Count >= 1 And SelectedWebCats.Find(Function(x) x.CategoryCode = cc And x.DefaultCategoryFlag = True) IsNot Nothing Then
            MessagePanel1.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1011)
            e.Canceled = True
        Else
            SelectedWebCats.RemoveAll(Function(x) x.CategoryCode = cc)
            'BindWebCategoriesGrid()
            grdWebCategories.Rebind()
        End If
    End Sub

    Private Sub btnCopyWebCatsFromISN_Click(sender As Object, e As System.EventArgs) Handles btnCopyWebCatsFromISN.Click
        If ddlCopyWebCatsFromISN.SelectedIndex > 0 Then
            Dim copyCats As List(Of WebCat) = _TUWebCat.GetWebCatByISN(CDec(ddlCopyWebCatsFromISN.SelectedValue)).ToList()
            copyCats = copyCats.Join(AllWebCats, Function(x) x.CategoryCode, Function(y) y.CategoryCode, Function(x, y) New WebCat With {.CategoryCode = x.CategoryCode, .DefaultCategoryFlag = x.DefaultCategoryFlag, .CategoryLongDesc = y.CategoryLongDesc}).ToList

            For Each wc As WebCat In copyCats
                Dim currentWC As WebCat = wc
                If SelectedWebCats.Where(Function(x) x.CategoryCode = currentWC.CategoryCode).Count = 0 Then
                    If SelectedWebCats.Where(Function(x) x.DefaultCategoryFlag = True).Count > 0 Then
                        currentWC.DefaultCategoryFlag = False
                    End If

                    SelectedWebCats.Add(currentWC)
                End If
            Next

            ' BindWebCategoriesGrid()
            grdWebCategories.Rebind()
        End If
    End Sub
#End Region

#Region "COLOR/SIZE TAB: Events"
    Private Sub SetupColorSizeTab()
        Session("PreTurnInSetUpCreate.PrevTabIndex") = Session("PreTurnInSetUpCreate.CurrTabIndex")
        Session("PreTurnInSetUpCreate.CurrTabIndex") = "5"

        ShowHideTabs("Ad List", rtsAPAdjustment.FindTabByText("Ad List").Visible)
        ShowHideTabs("Ad Level", rtsAPAdjustment.FindTabByText("Ad Level").Visible)
        ShowHideTabs("Result List", rtsAPAdjustment.FindTabByText("Result List").Visible)
        ShowHideTabs("Killed Items", rtsAPAdjustment.FindTabByText("Killed Items").Visible)
        ShowHideTabs("ISN Level", rtsAPAdjustment.FindTabByText("ISN Level").Visible)
        ShowHideTabs("Color/Size Level", True)

        For Each selectedISN As EcommSetupCreateInfo In AllSelectedISNs
            If _TUWebCat.GetWebCatByISN(selectedISN.ISN).Where(Function(x) x.DefaultCategoryFlag = True).ToList.Count = 0 Then
                mpPreTurnInCreate.PopUpMessage = "Return to the ISN tab and select at least one primary Web Category for ISN: " & selectedISN.ISN & "."
                Me.Master.SideBar.Collapsed = True
                SetupISNTab()
                Exit Sub
            End If
        Next

        Dim rlbISNs As RadListBox = CType(tuModalOrderISN.FindControl("rlbISNs"), RadListBox)
        With rlbISNs
            .DataSource = AllSelectedISNs.Select(Function(x) New With {Key .ISN = x.ISN, .ISNText = x.ISN & " - " & x.ISNDesc & " - " & x.VendorStyleNumber, x.Sequence}).Distinct.OrderBy(Function(x) x.Sequence).ToList
            .DataTextField = "ISNText"
            .DataValueField = "ISN"
            .DataBind()
        End With



        rtsAPAdjustment.FindTabByText("Color/Size Level").Selected = True
        rmpPreTurnInCreate.SelectedIndex = 5

        ShowHideButtons("Reset", False)
        ShowHideButtons("Sort", True)
        ShowHideButtons("Level Down", False)
        ShowHideButtons("Level Up", False)
        ShowHideButtons("Save", False)
        ShowHideButtons("Add To Batch", False)
        ShowHideButtons("Retrieve", False)
        ShowHideButtons("Print Labels", False)
        ShowHideButtons("Print Report", True)
        ShowHideButtons("Submit", True)
        EditModeOnOff("Off")

        'Populate ColorSize Grid data.
        grdColorSizeLevel.Visible = True
        grdColorSizeLevel.Rebind()

        If Request.QueryString("Action").ToUpper = Modes.INQUIRY.ToString Then
            'Disable all the Buttons and Grid in Inquiry mode.
            EnableDisableButtons("Print Labels", False)
            EnableDisableButtons("Print Report", True)
            EnableDisableButtons("Submit", False)
            'btnFlood.Enabled = False
            'btnResetFlood.Enabled = False
            tblFloodOptions.Visible = False

            For Each gridDataItem As GridDataItem In grdColorSizeLevel.MasterTableView.Items
                CType(gridDataItem("EditCommandColumn"), GridTableCell).Enabled = False
                CType(gridDataItem("DeleteColumn"), GridTableCell).Enabled = False
            Next
        Else
            'Get the Sys Page Number for the selected Ad Nbr and Page Nbr.
            Dim CtlgAdPgInfos As IList(Of CtlgAdPgInfo) = New List(Of CtlgAdPgInfo)()
            Dim PgNbr As Integer = 0

            If String.IsNullOrEmpty(lblPageNumberText.Text) Then
                PgNbr = 0
            ElseIf lblPageNumberText.Text.Contains("-"c) Then
                PgNbr = CInt(lblPageNumberText.Text.Substring(0, lblPageNumberText.Text.IndexOf("-"c)).Trim)
            Else
                PgNbr = CInt(lblPageNumberText.Text.Trim)
            End If

            CtlgAdPgInfos = _TUCtlgAdPg.GetAllFromCtlgAdPg(CInt(lblAdNoText.Text), PgNbr)
            If CtlgAdPgInfos.Count > 0 Then
                hdnSysPgNbr.Value = CtlgAdPgInfos(0).syspgnbr.ToString
            End If

            'Populate the Combo Boxes for Flood Option.
            'Color Family
            With rcbFloodColorFamily
                .DataSource = _TUEcommSetupCreate.GetAllColorFamily
                .DataValueField = "Value"
                .DataTextField = "Text"
                .DataBind()
            End With

            ''Sample Size
            'With rcbFloodSampleSize
            '    .DataSource = _TUEcommSetupCreate.GetAllSampleSizes(String.Join(",", AllSelectedISNs.Select(Function(x) x.ISN)))
            '    .DataValueField = "Value"
            '    .DataTextField = "Text"
            '    .DataBind()
            'End With

            'ImageKind
            With rcbFloodImgKind
                .DataSource = _TUTMS900PARAMETER.GetAllImageKindValues
                .DataValueField = "CharIndex"
                .DataTextField = "ShortDesc"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With

            'ImageType
            With rcbFloodImgType
                .DataSource = _TUTMS900PARAMETER.GetAllImageTypeValues
                .DataValueField = "CharIndex"
                .DataTextField = "ShortDesc"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With

            'AlternateView
            With rcbFloodAltView
                .DataSource = _TUTMS900PARAMETER.GetAllAltViewValues
                .DataValueField = "CharIndex"
                .DataTextField = "ShortDesc"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With

            'SampleStore
            With rcbFloodSampleStr
                .DataSource = _TUEcommSetupCreate.GetClrSizeLocLookUp(0).Where(Function(x) x.Identifier = "LOC")
                .DataValueField = "Value"
                .DataTextField = "Text"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub

    Private Sub grdColorSizeLevel_ColumnCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridColumnCreatedEventArgs) Handles grdColorSizeLevel.ColumnCreated
        'Hide the Grid's Expand/Collapse Column
        If TypeOf e.Column Is GridGroupSplitterColumn Then
            e.Column.HeaderStyle.Width = Unit.Pixel(1)
            e.Column.HeaderStyle.Font.Size = FontUnit.Point(1)
            e.Column.ItemStyle.Width = Unit.Pixel(1)
            e.Column.ItemStyle.Font.Size = FontUnit.Point(1)
            e.Column.Resizable = False
        End If
    End Sub

    Private Sub grdColorSizeLevel_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdColorSizeLevel.ItemCreated
        'Apply style to Group Headers.  
        If TypeOf e.Item Is GridGroupHeaderItem Then
            Dim grpHeaderItem As GridGroupHeaderItem = DirectCast(e.Item, GridGroupHeaderItem)
            If grpHeaderItem.GroupIndex.Split("_"c).Length = 2 Then
                e.Item.BackColor = Drawing.Color.White
            ElseIf grpHeaderItem.GroupIndex.Split("_"c).Length = 1 Then
                e.Item.BackColor = Drawing.Color.White
            End If
        ElseIf TypeOf e.Item Is GridEditableItem AndAlso e.Item.IsInEditMode Then
            Dim item As GridEditableItem = CType(e.Item, GridEditableItem)

            Dim rcbImageKind As RadComboBox = CType(item.FindControl("rcbImageKind"), RadComboBox)
            rcbImageKind.OnClientSelectedIndexChanged = "function(sender,args) {OnChangeImageKind(sender,args,'" + item.ItemIndex.ToString + "')}"

            Dim rcbFeatureRenderSwatch As RadComboBox = CType(item.FindControl("rcbFeatureRenderSwatch"), RadComboBox)
            rcbFeatureRenderSwatch.OnClientSelectedIndexChanged = "function(sender,args) {OnChangeFRS(sender,args,'" + item.ItemIndex.ToString + "')}"
        End If
    End Sub

    Private Function grdColorSizeLevelUpdateRow(editItem As GridEditableItem) As Boolean
        Dim ColorSizeData As New EcommSetupClrSzInfo()

        Page.Validate("Update")

        ''Validate Friendly Product Features
        'Dim fpf As String = Trim(DirectCast(editItem.FindControl("rtxtFriendlyProdFeatures"), RadTextBox).Text)
        'If fpf.Length > 2000 Then
        '    mpPreTurnInCreate.PopUpMessage = "Friendly Product Features must be 2000 characters or less. You have entered " & fpf.Length & " characters."
        '    Return False
        'End If

        ' ''Validate Friendly Color                
        'If String.IsNullOrEmpty(DirectCast(editItem.FindControl("rtxtFriendlyColor"), RadTextBox).Text.Trim) Then
        '    mpPreTurnInCreate.PopUpMessage = "Friendly Color must be entered."
        '    Return False
        'End If

        'If DirectCast(editItem.FindControl("rcbSampleSize"), RadComboBox).Visible = True Then
        '    ColorSizeData.ImageKind = DirectCast(editItem.FindControl("rcbSampleSize"), RadComboBox).SelectedValue
        '    ColorSizeData.AltView = ""
        'Else
        '    ColorSizeData.ImageKind = ""
        '    ColorSizeData.AltView = ""
        'End If

        'Removing sample size  validations
        'If Not IsNothing(DirectCast(editItem.FindControl("rcbSampleSize"), RadComboBox)) Then
        '    If DirectCast(editItem.FindControl("rcbSampleSize"), RadComboBox).Visible = True And DirectCast(editItem.FindControl("rcbSampleSize"), RadComboBox).Enabled = True Then
        '        'If ColorSizeData.ImageKind <> "NEW" Then
        '        '    If DirectCast(editItem.FindControl("rcbSampleSize"), RadComboBox).SelectedItem.Text.Length = 0 Then
        '        '        mpPreTurnInCreate.PopUpMessage = "Sample Size must be selected."
        '        '        Return False
        '        '    End If
        '        'End If

        '        ColorSizeData.SampleSize = DirectCast(editItem.FindControl("rcbSampleSize"), RadComboBox).SelectedValue
        '    Else
        '        ColorSizeData.SampleSize = "0"
        '        'If DirectCast(editItem.FindControl("rcbSampleSize"), RadComboBox).Enabled = True Then ' Sample Size is required.
        '        '    mpPreTurnInCreate.PopUpMessage = "Sample Size must be selected."
        '        '    Return False
        '        'End If
        '    End If
        'End If

        If Not IsValid Then
            mpPreTurnInCreate.PopUpMessage = "Errors on Page."
            Return False
        End If

        If editItem.ItemIndex > -1 Then
            'Get all the required values from the Grid Row being Updated.
            ColorSizeData.TurnInMerchID = CInt(editItem.GetDataKeyValue("TurnInMerchID"))
            ColorSizeData.UPC = CDec(editItem.GetDataKeyValue("UPC"))
            ColorSizeData.VendorColorCode = CInt(editItem.GetDataKeyValue("VendorColorCode"))
            ColorSizeData.DeptID = CInt(editItem.GetDataKeyValue("DeptID"))
            ColorSizeData.ISN = CDec(editItem.GetDataKeyValue("ISN"))
            ColorSizeData.IsnDesc = CStr(editItem.GetDataKeyValue("IsnDesc"))
            ColorSizeData.VendorStyleNum = CStr(editItem.GetDataKeyValue("VendorStyleNum"))
            ColorSizeData.LabelName = CStr(editItem.GetDataKeyValue("LabelName"))

            ColorSizeData.IsHotItem = CChar(DirectCast(editItem.FindControl("rcbHotItem"), RadComboBox).SelectedValue)
            ColorSizeData.FriendlyProdDesc = DirectCast(editItem.FindControl("rtxtFriendlyProdDesc"), RadTextBox).Text.Trim
            ColorSizeData.FriendlyProdFeatures = DirectCast(editItem.FindControl("rtxtFriendlyProdFeatures"), RadTextBox).Text
            ColorSizeData.FriendlyColor = DirectCast(editItem.FindControl("rtxtFriendlyColor"), RadTextBox).Text.Trim

            Dim rcbEditClrFam As RadComboBox = DirectCast(editItem.FindControl("rcbColorFamily"), RadComboBox)
            ColorSizeData.ColorFamily = If(rcbEditClrFam.CheckedItems.Count > 0, String.Join(",", rcbEditClrFam.CheckedItems.Select(Function(a) a.Value)), "0")

            ColorSizeData.AdminMerchNum = CInt(DirectCast(editItem.FindControl("hfMerchId"), RadTextBox).Text.Trim())
            ColorSizeData.SampleDescription = DirectCast(editItem.FindControl("radtxtSample"), RadTextBox).Text.Trim()
            ColorSizeData.SampleSize = If(DirectCast(editItem.FindControl("hfSampleSize"), RadTextBox).Text.Trim(), "0")

            ColorSizeData.ColorCorrect = CChar(DirectCast(editItem.FindControl("rcbColorCorrect"), RadComboBox).SelectedValue)
            ColorSizeData.ImageKind = DirectCast(editItem.FindControl("rcbImageKind"), RadComboBox).SelectedValue
            ColorSizeData.PuImageID = Convert.ToInt32(DirectCast(editItem.FindControl("rtxtPUImgID"), RadNumericTextBox).Value)
            ColorSizeData.RouteFromAD = Convert.ToInt32(DirectCast(editItem.FindControl("rcbRouteFromAd"), RadComboBox).SelectedValue.Trim)
            ColorSizeData.GroupNum = Convert.ToInt16(DirectCast(editItem.FindControl("rtxtGroupNum"), RadNumericTextBox).Value)
            ColorSizeData.FeatureRenderSwatch = DirectCast(editItem.FindControl("rcbFeatureRenderSwatch"), RadComboBox).SelectedValue

            ColorSizeData.ImageType = DirectCast(editItem.FindControl("rcbOnOff"), RadComboBox).SelectedValue

            ColorSizeData.AltView = DirectCast(editItem.FindControl("rcbAlternateView"), RadComboBox).SelectedValue
            ColorSizeData.SampleStore = DirectCast(editItem.FindControl("rcbSampleStore"), RadComboBox).SelectedValue
            ColorSizeData.MerchantNotes = DirectCast(editItem.FindControl("rtxtMerchantNotes"), RadTextBox).Text
            ColorSizeData.IsReserve = CChar(editItem.GetDataKeyValue("IsReserve"))

            'if reserve, upc is blank
            If ColorSizeData.SampleSize <> "0" And ColorSizeData.IsReserve = "N" Then
                ColorSizeData.UPC = _TUEcommSetupCreate.GetUPC(ColorSizeData.TurnInMerchID, CInt(ColorSizeData.SampleSize))
            End If

            'If Current Batch is expired, redirect to the expired page. Not sure how this would occur but it did.
            If CurrentBatch.AdNumber = 0 Then
                Response.Redirect("~/SessionExpired.aspx")
            End If

            _TUEcommSetupCreate.UpdateColorSize(ColorSizeData, SessionWrapper.UserID)

            Session("PreTurnInSetUp.LastUpdatedRowId") = ColorSizeData.TurnInMerchID
            mpPreTurnInCreate.ErrorMessage = "Data Saved Successfully."
            Return True
        End If
    End Function

    Private Sub grdColorSizeLevel_UpdateCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdColorSizeLevel.UpdateCommand
        Try
            If (TypeOf e.Item Is GridEditableItem And e.Item.IsInEditMode) Then
                Dim editItem As GridEditableItem = DirectCast(e.Item, GridEditableItem)
                If Not grdColorSizeLevelUpdateRow(editItem) Then
                    e.Canceled = True
                End If

            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdColorSizeLevel_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdColorSizeLevel.DeleteCommand
        Try
            If (TypeOf e.Item Is GridDataItem) Then
                Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim MerchID As Integer = CInt(dataItem.GetDataKeyValue("TurnInMerchID"))
                Dim AdminMerchID As Integer = CInt(dataItem.GetDataKeyValue("AdminMerchNum"))
                Dim RemoveMerchFlg As Char = CChar(dataItem.GetDataKeyValue("RemoveMerchFlag"))

                If dataItem.ItemIndex > -1 Then
                    'To Delete / Activate a Color Size Level record in the database - Set the Remove Flag to Y / N.
                    _TUEcommSetupCreate.DeleteColorSize(MerchID, SessionWrapper.UserID)

                    'Below: Commented out for a Fix to AdminInsert issue. 04/13/2015 KL.

                    If (RemoveMerchFlg = "N"c) Then
                        mpPreTurnInCreate.ErrorMessage = "Selected row flagged for removal."
                        ' Commented out below 10/13/2015 KL.
                        ' _TUEcommSetupCreate.UpdateAdminStatus(AdminMerchID, CurrentBatch.AdNumber, CurrentBatch.PageNumber, "K")
                    Else
                        mpPreTurnInCreate.ErrorMessage = "Selected row has been Activated."
                        ' Commented out below 10/13/2015 KL.
                        ' _TUEcommSetupCreate.UpdateAdminStatus(AdminMerchID, CurrentBatch.AdNumber, CurrentBatch.PageNumber, "A")
                    End If
                End If
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdColorSizeLevel_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdColorSizeLevel.ItemDataBound
        Try
            'Disable the rows that are flagged for Removal.
            If (TypeOf e.Item Is GridDataItem) Then
                Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim RemoveMerchFlg As Char = CChar(dataItem.GetDataKeyValue("RemoveMerchFlag"))
                Dim status As String = CStr(dataItem.GetDataKeyValue("Status"))
                Dim TurnInMerchID As Integer = CInt(dataItem.GetDataKeyValue("TurnInMerchID"))
                'Dim imageLink As HyperLink = Nothing

                If dataItem.ItemIndex > -1 Then
                    If Session("PreTurnInSetUp.LastUpdatedRowId") IsNot Nothing Then
                        If dataItem.GetDataKeyValue("TurnInMerchID").ToString = CStr(Session("PreTurnInSetUp.LastUpdatedRowId")) Then
                            dataItem.BackColor = Drawing.Color.Yellow
                            Session("PreTurnInSetUp.LastUpdatedRowId") = Nothing
                        End If
                    End If

                    If (RemoveMerchFlg = "Y"c) Then
                        dataItem.Font.Strikeout = True

                        For i As Integer = 0 To dataItem.Controls.Count - 1
                            CType(dataItem.Controls(i), GridTableCell).Enabled = False
                        Next

                        CType(dataItem("DeleteColumn"), GridTableCell).Enabled = True
                        CType(dataItem("DeleteColumn").Controls(0), ImageButton).ImageUrl = "~/Images/CheckMark.gif"
                        CType(dataItem("DeleteColumn").Controls(0), ImageButton).ToolTip = "Activate"
                    End If

                    If (status = "RDFM") Then
                        For i As Integer = 0 To dataItem.Controls.Count - 1
                            CType(dataItem.Controls(i), GridTableCell).Enabled = False
                        Next

                        CType(dataItem("selColumn"), GridTableCell).Enabled = True
                    End If

                    ''Enable/Disable image available column
                    'If Not dataItem("VTImageAvailable") Is Nothing AndAlso dataItem("VTImageAvailable").Controls.Count > 0 Then
                    '    imageLink = CType(dataItem("VTImageAvailable").Controls(0), HyperLink)
                    '    If imageLink.Text.Equals("N") Then
                    '        imageLink.ForeColor = Drawing.Color.Black
                    '        imageLink.Font.Underline = False
                    '    Else
                    '        imageLink.Font.Underline = True
                    '        imageLink.ForeColor = Drawing.Color.Blue
                    '        imageLink.Attributes.Add("onclick", String.Format("DisplayVTImage('{0}')", imageLink.NavigateUrl))
                    '        imageLink.Attributes.Add("style", "cursor:pointer")
                    '    End If
                    'End If
                End If
            End If

            'Handle EDIT command - Populate the values for all the columns with Comboboxes.
            If (TypeOf e.Item Is GridEditableItem And e.Item.IsInEditMode) Then
                Dim editItem As GridEditableItem = DirectCast(e.Item, GridEditableItem)
                Dim ISN As Decimal = CDec(editItem.GetDataKeyValue("ISN"))
                Dim IsReserveISN As Char = CChar(editItem.GetDataKeyValue("IsReserve"))
                Dim ClrSizeLocLookupVals As New List(Of ClrSizLocLookUp)

                ClrSizeLocLookupVals = _TUEcommSetupCreate.GetClrSizeLocLookUp(ISN)
                '1. HotItem - Not required as the Combo box values are hard-coded ("", H, R)

                '2. Color Family
                Dim cmbColorFamily As RadComboBox = DirectCast(editItem.FindControl("rcbColorFamily"), RadComboBox)
                Dim hfColorFamily As HiddenField = DirectCast(editItem.FindControl("hfColorFamily"), HiddenField)
                Dim ltrError As Literal = DirectCast(editItem.FindControl("ltrError"), Literal)

                Dim ColorFamilies As List(Of ClrSizLocLookUp) = ClrSizeLocLookupVals.Where(Function(x) x.Identifier = "CLR").ToList

                If ColorFamilies.Count > 0 Then
                    With cmbColorFamily
                        .DataSource = ColorFamilies
                        .DataValueField = "Value"
                        .DataTextField = "Text"
                        .DataBind()
                    End With
                Else
                    cmbColorFamily.Visible = False
                    ltrError.Visible = True
                    ltrError.Text = "Color Family cannot be selected because no primary Web Category is assigned."
                End If

                If Not String.IsNullOrEmpty(hfColorFamily.Value.Trim) Then
                    For Each color As String In hfColorFamily.Value.Trim.Split(","c)
                        If cmbColorFamily.FindItemByText(color) IsNot Nothing Then
                            cmbColorFamily.FindItemByText(color).Checked = True
                        End If
                    Next
                End If

                Dim hfMerchId As RadTextBox = DirectCast(editItem.FindControl("hfMerchId"), RadTextBox)

                '3. Sample Size
                'Dim cmbSampleSize As RadComboBox = DirectCast(editItem.FindControl("rcbSampleSize"), RadComboBox)
                'Dim hfSampleSize As HiddenField = DirectCast(editItem.FindControl("hfSampleSize"), HiddenField)
                'Dim ltrSizeError As Literal = DirectCast(editItem.FindControl("ltrSizeError"), Literal)

                'Dim SizeFamilies As List(Of ClrSizLocLookUp)
                'If IsReserveISN = "Y"c Then
                '    SizeFamilies = ClrSizeLocLookupVals.Where(Function(x) x.Identifier = "SIZ_RSV").ToList
                'Else
                '    SizeFamilies = ClrSizeLocLookupVals.Where(Function(x) x.Identifier = "SIZ").ToList
                'End If

                'If SizeFamilies.Count > 0 Then
                '    cmbSampleSize.DataSource = SizeFamilies
                '    cmbSampleSize.DataTextField = "Text"
                '    cmbSampleSize.DataValueField = "Value"
                '    cmbSampleSize.DataBind()
                '    cmbSampleSize.Items.Insert(0, New RadComboBoxItem("", "0"))
                'Else
                '    cmbSampleSize.Visible = True
                '    cmbSampleSize.Enabled = False
                'End If

                'This try block handles bad data in the size field.
                'Try
                '    cmbSampleSize.FindItemByText(hfSampleSize.Value.Trim).Selected = True
                'Catch ex As Exception
                'End Try

                '4. Color Correct - Not required as the Combo box values are hard-coded ("", Y, N)

                '5. ImageKind
                Dim cmbImageKind As RadComboBox = DirectCast(editItem.FindControl("rcbImageKind"), RadComboBox)
                Dim hfImageKind As HiddenField = DirectCast(editItem.FindControl("hfImageKind"), HiddenField)

                With cmbImageKind
                    .DataSource = Session("ImageKind")
                    .DataValueField = "CharIndex"
                    .DataTextField = "ShortDesc"
                    .DataBind()
                    .FindItemByText(If(String.IsNullOrEmpty(hfImageKind.Value.Trim), "New", hfImageKind.Value.Trim)).Selected = True
                End With

                '6. RouteFromAd
                Dim cmbRouteFromAd As RadComboBox = DirectCast(editItem.FindControl("rcbRouteFromAd"), RadComboBox)
                Dim hfRouteFromAd As HiddenField = DirectCast(editItem.FindControl("hfRouteFromAd"), HiddenField)

                With cmbRouteFromAd
                    .DataSource = Session("RouteAd")
                    .DataValueField = "AdNbr"
                    .DataTextField = "AdNbr"
                    .DataBind()
                    .Items.Insert(0, New RadComboBoxItem("", "0"))
                End With

                If cmbRouteFromAd.FindItemByValue(hfRouteFromAd.Value.Trim) IsNot Nothing Then
                    cmbRouteFromAd.FindItemByValue(hfRouteFromAd.Value.Trim).Selected = True
                End If

                '7. FeatureRenderSwatch
                Dim cmbFeatureRenderSwatch As RadComboBox = DirectCast(editItem.FindControl("rcbFeatureRenderSwatch"), RadComboBox)
                Dim hfFeatureRenderSwatch As HiddenField = DirectCast(editItem.FindControl("hfFeatureRenderSwatch"), HiddenField)

                With cmbFeatureRenderSwatch
                    .DataSource = Session("RenderSwatch")
                    .DataValueField = "CharIndex"
                    .DataTextField = "ShortDesc"
                    .DataBind()
                    .Items.Insert(0, New RadComboBoxItem(""))
                    .FindItemByText(hfFeatureRenderSwatch.Value.Trim).Selected = True
                End With

                '9. AlternateView
                Dim cmbAlternateView As RadComboBox = DirectCast(editItem.FindControl("rcbAlternateView"), RadComboBox)
                Dim hfAlternateView As HiddenField = DirectCast(editItem.FindControl("hfAlternateView"), HiddenField)

                With cmbAlternateView
                    .DataSource = Session("AltView")
                    .DataValueField = "CharIndex"
                    .DataTextField = "ShortDesc"
                    .DataBind()
                    .Items.Insert(0, New RadComboBoxItem(""))
                    .FindItemByText(hfAlternateView.Value.Trim).Selected = True
                End With

                '10. SampleStore
                Dim cmbSampleStore As RadComboBox = DirectCast(editItem.FindControl("rcbSampleStore"), RadComboBox)
                Dim hfSampleStore As HiddenField = DirectCast(editItem.FindControl("hfSampleStore"), HiddenField)

                With cmbSampleStore
                    .DataSource = ClrSizeLocLookupVals.Where(Function(x) x.Identifier = "LOC")
                    .DataValueField = "Value"
                    .DataTextField = "Text"
                    .DataBind()
                    .Items.Insert(0, New RadComboBoxItem("", "0"))
                    .FindItemByValue(hfSampleStore.Value.Trim).Selected = True
                End With

                Dim cmbOnOff As RadComboBox = DirectCast(editItem.FindControl("rcbOnOff"), RadComboBox)
                Dim hfOnOff As HiddenField = DirectCast(editItem.FindControl("hfOnOff"), HiddenField)

                With cmbOnOff
                    .DataSource = Session("MDSEFigureCode")
                    .DataValueField = "CharIndex"
                    .DataTextField = "ShortDesc"
                    .DataBind()
                    .Items.Insert(0, New RadComboBoxItem(""))
                    .FindItemByText(hfOnOff.Value.Trim).Selected = True
                End With

                If cmbImageKind.SelectedValue = "VND" Then
                    cmbOnOff.Enabled = False
                Else
                    cmbOnOff.Enabled = True
                End If

            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdColorSizeLevel_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdColorSizeLevel.NeedDataSource
        Dim ISNs As New List(Of String)
        Dim ISNFrmSelClrs As New List(Of String)
        Dim ClrCodes As New List(Of String)
        Dim vendorStyleNumbers As String = String.Empty
        Dim DB2ColorSizeResults As New List(Of EcommSetupClrSzInfo)
        Dim selectedISN As New EcommSetupCreateInfo
        Dim selectedISNDetail As New EcommSetupCreateInfo
        Dim virtualTicketInfo As New VirtualTicketInfo
        Dim imageURLs As Dictionary(Of String, String) = Nothing

        Try
            If AllSelectedISNs.Count > 0 Then
                ISNs = AllSelectedISNs.Where(Function(x) x.Saved = True).Select(Function(x) x.ISN.ToString).Distinct.ToList
                ClrCodes.Add("")
                DB2ColorSizeResults = CType(_TUEcommSetupCreate.GetColorSizeResults(CurrentBatch.AdNumber, CShort(CurrentBatch.PageNumber), ISNs, ClrCodes), List(Of EcommSetupClrSzInfo))
                selectedISN = AllSelectedISNs.FirstOrDefault()
            End If

            'Populate DeptIDDesc and BuyerNameExt values.        
            selectedISNDetail = _TUEcommSetupCreate.GetISNLevelDetailByISN(selectedISN.ISN, selectedISN.IsReserve)

            hdnBuyer.Value = selectedISNDetail.BuyerName & "/" & selectedISNDetail.BuyerExt

            'vendorStyleNumbers = String.Join(",", DB2ColorSizeResults.Select(Function(x) x.VendorStyleNum.Trim()).Distinct.ToList())
            'imageURLs = virtualTicketInfo.GetVTImageURLByVendorStyleNumber(vendorStyleNumbers)

            'TODO: IsTurnedIn and TurnedInAdNos won't work in maint mode
            Dim ColorSizeResults As List(Of EcommSetupClrSzInfo) = DB2ColorSizeResults.Select(Function(x) New EcommSetupClrSzInfo With { _
                                                    .TurnInMerchID = x.TurnInMerchID, _
                                                    .AdminMerchNum = x.AdminMerchNum, _
                                                    .SampleMerchId = x.SampleMerchId, _
                                                    .RemoveMerchFlag = x.RemoveMerchFlag, _
                                                    .Status = x.Status, _
                                                    .DeptID = x.DeptID, _
                                                    .DeptIdDesc = x.DeptIdDesc, _
                                                    .ISN = x.ISN, _
                                                    .VendorStyleNum = x.VendorStyleNum, _
                                                    .VendorName = x.VendorName, _
                                                    .IsnDesc = x.IsnDesc, _
                                                    .IsReserve = x.IsReserve, _
                                                    .IsTurnedIn = If(AllSelectedISNs.Where(Function(s) s.ISN = x.ISN And s.IsTurnedInEcomm = "Y"c _
                                                                    And s.IsTurnedInPrint = "Y"c).Count > 0, "B"c, _
                                                                    If(AllSelectedISNs.Where(Function(s) s.ISN = x.ISN And s.IsTurnedInEcomm = "Y"c).Count > 0, _
                                                                    "E"c, If(AllSelectedISNs.Where(Function(s) s.ISN = x.ISN _
                                                                    And s.IsTurnedInPrint = "Y"c).Count > 0, "P"c, "N"c))), _
                                                    .TurnedInAdNos = String.Join(",", AllSelectedISNs.Where(Function(s) s.ISN = x.ISN).Select(Function(y) y.TurnedInEcommAdNos + " " + y.TurnedInPrintAdNos).ToList), _
                                                    .BatchNumber = x.BatchNumber, _
                                                    .IsHotItem = x.IsHotItem, _
                                                    .FriendlyProdDesc = Trim(x.FriendlyProdDesc), _
                                                    .FriendlyProdFeatures = x.FriendlyProdFeatures, _
                                                    .Color = x.Color, _
                                                    .VendorColorCode = x.VendorColorCode, _
                                                    .FriendlyColor = x.FriendlyColor, _
                                                    .ColorFamily = x.ColorFamily, _
                                                    .SampleSize = x.SampleSize, _
                                                    .SampleDescription = x.SampleDescription, _
                                                    .ColorCorrect = x.ColorCorrect, _
                                                    .ImageKind = x.ImageKind, _
                                                    .PuImageID = x.PuImageID, _
                                                    .RouteFromAD = x.RouteFromAD, _
                                                    .GroupNum = x.GroupNum, _
                                                    .FeatureRenderSwatch = x.FeatureRenderSwatch, _
                                                    .ImageType = x.ImageType, _
                                                    .AltView = x.AltView, _
                                                    .SampleStoreNum = x.SampleStoreNum, _
                                                    .SampleStore = x.SampleStore, _
                                                    .MerchantNotes = x.MerchantNotes, _
                                                    .UPC = x.UPC, _
                                                    .Sequence = x.Sequence, _
                                                    .LabelName = x.LabelName}).Where(Function(x) x.BatchNumber = CurrentBatch.BatchId).OrderBy(Function(x) x.Sequence).OrderBy(Function(x) x.FeatureRenderSwatch).ToList

            If ColorSizeResults.Count > 0 Then
                CacheBatchLevelData()
                grdColorSizeLevel.GroupingEnabled = True
                grdColorSizeLevel.DataSource = ColorSizeResults
            Else
                grdColorSizeLevel.DataSource = Nothing
                grdColorSizeLevel.GroupingEnabled = False
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdColorSizeLevel_SortCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridSortCommandEventArgs) Handles grdColorSizeLevel.SortCommand
        grdColorSizeLevel.Rebind()
    End Sub

    ''' <summary>
    ''' Handles the FLOOD option.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnFlood_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFlood.Click
        Dim intTurnInMerchID As Integer = 0
        Dim strImageKind As String = String.Empty
        Dim strImageCatgy As String = String.Empty
        Dim strImageType As String = String.Empty
        Dim ColorFamilyFloodErrorList As New List(Of String)

        Try
            Page.Validate("FloodUpdate")

            If Not IsValid Then
                mpPreTurnInCreate.ErrorMessage = "Errors on Page."
                Exit Sub
            End If

            'Exit the Sub-routine if no data is entered for "Flood" option.
            If String.IsNullOrEmpty(rtxtFloodFrndPrdDesc.Text.Trim) And _
                    String.IsNullOrEmpty(rtxtFloodFrndPrdFeat.Text.Trim) And _
                    String.IsNullOrEmpty(rtxtFloodFriendlyColor.Text.Trim) And _
                    rcbFloodColorFamily.CheckedItems.Count = 0 And _
                    String.IsNullOrEmpty(rcbFloodClrCorrect.SelectedValue) And _
                    String.IsNullOrEmpty(rcbFloodImgKind.SelectedValue) And _
                    String.IsNullOrEmpty(rtxtFloodGroupNum.Text.Trim) And _
                    String.IsNullOrEmpty(rcbFloodImgType.SelectedValue) And _
                    String.IsNullOrEmpty(rcbFloodAltView.SelectedValue) And _
                    String.IsNullOrEmpty(rcbFloodSampleStr.SelectedValue) And _
                    String.IsNullOrEmpty(rtxtFloodMerchNotes.Text.Trim) Then
                mpPreTurnInCreate.PopUpMessage = "Enter value for at least one Flood option."
                Exit Sub
            End If

            Dim selRowCount As Integer = grdColorSizeLevel.MasterTableView.GetSelectedItems.Where(Function(x) CChar(x.GetDataKeyValue("RemoveMerchFlag")) = "N"c).Count

            If selRowCount > 0 Then
                If grdColorSizeLevel.MasterTableView.GetSelectedItems.Where(Function(x) CStr(x.GetDataKeyValue("Status")) = "RDFM").Count = 0 Then
                    'Clear the Edit mode, if any row is left in Edit mode.
                    grdColorSizeLevel.MasterTableView.ClearEditItems()

                    Dim ColorSizeData As New EcommSetupClrSzInfo()
                    Dim TurnInMerchIDs As New List(Of String)

                    ColorSizeData.FriendlyProdDesc = rtxtFloodFrndPrdDesc.Text.Trim
                    ColorSizeData.FriendlyProdFeatures = rtxtFloodFrndPrdFeat.Text.Trim
                    ColorSizeData.FriendlyColor = rtxtFloodFriendlyColor.Text.Trim
                    ColorSizeData.ColorCorrect = CChar(rcbFloodClrCorrect.SelectedValue)
                    ColorSizeData.ImageKind = rcbFloodImgKind.SelectedValue
                    If rtxtFloodGroupNum.Text.Trim <> "" Then
                        ColorSizeData.GroupNum = CShort(rtxtFloodGroupNum.Text.Trim)
                    End If
                    ColorSizeData.ImageType = rcbFloodImgType.SelectedValue
                    ColorSizeData.AltView = rcbFloodAltView.SelectedValue
                    ColorSizeData.SampleStore = rcbFloodSampleStr.SelectedValue
                    ColorSizeData.MerchantNotes = rtxtFloodMerchNotes.Text.Trim

                    For Each gridSelItem As GridDataItem In grdColorSizeLevel.MasterTableView.GetSelectedItems.Where(Function(x) CChar(x.GetDataKeyValue("RemoveMerchFlag")) = "N"c)
                        'Get all the Merch IDs for the selected rows.
                        Dim TurnInMerchID As String = CStr(gridSelItem.GetDataKeyValue("TurnInMerchID"))
                        TurnInMerchIDs.Add(TurnInMerchID)

                        If rcbFloodColorFamily.CheckedItems.Count > 0 Then
                            Dim ISN As Decimal = CDec(gridSelItem.GetDataKeyValue("ISN"))
                            Dim AllColorFamiliesForISN As List(Of ClrSizLocLookUp) = _TUEcommSetupCreate.GetClrSizeLocLookUp(ISN).Where(Function(x) x.Identifier = "CLR").ToList
                            Dim FloodColorFamilies As New List(Of String)

                            For Each item As RadComboBoxItem In rcbFloodColorFamily.CheckedItems.ToList
                                Dim FloodColorFamilyId As String = item.Value
                                Dim FloodColorFamilyName As String = item.Text

                                If AllColorFamiliesForISN.Exists(Function(x) x.Value = FloodColorFamilyId) Then
                                    FloodColorFamilies.Add(FloodColorFamilyId)
                                Else
                                    ColorFamilyFloodErrorList.Add(TurnInMerchID)
                                End If

                            Next
                            If FloodColorFamilies.Count > 0 Then
                                _TUEcommSetupCreate.UpdateColorFamilyFlood(CInt(TurnInMerchID), String.Join(",", FloodColorFamilies), SessionWrapper.UserID)
                            End If

                        End If
                    Next

                    'Update the data in the database.
                    _TUEcommSetupCreate.UpdateColorSizeFlood(String.Join(",", TurnInMerchIDs), ColorSizeData, SessionWrapper.UserID)

                    'Update Image Type (On/Off) based on Image Kind (NEW, DUP, VND, CR8, PU, NOMER) and Image Category (Feature/Render/Swatch).
                    strImageKind = rcbFloodImgKind.SelectedValue

                    If Not String.IsNullOrEmpty(strImageKind.Trim) Then
                        For Each gridSelectedItem As GridDataItem In grdColorSizeLevel.MasterTableView.GetSelectedItems.Where(Function(x) CChar(x.GetDataKeyValue("RemoveMerchFlag")) = "N"c)
                            intTurnInMerchID = CInt(gridSelectedItem.GetDataKeyValue("TurnInMerchID"))
                            strImageCatgy = CType(gridSelectedItem("FeatureRenderSwatch").Controls(0), DataBoundLiteralControl).Text.Trim
                            strImageType = String.Empty

                            If ((strImageKind.ToUpper = "NEW") And _
                                (strImageCatgy.ToUpper = "FEATURE" Or strImageCatgy.ToUpper = "STAND ALONE" Or strImageCatgy.ToUpper = "RENDER")) Then
                                strImageType = "ON"
                            ElseIf ((strImageKind.ToUpper <> "NEW") And _
                                    (strImageCatgy.ToUpper = "SWATCH" Or strImageCatgy.ToUpper = "STATIC SWATCH")) Then
                                strImageType = "OFF"
                            End If

                            If Not String.IsNullOrEmpty(strImageType) Then
                                _TUEcommSetupCreate.UpdateImageTypeFlood(intTurnInMerchID, strImageType, SessionWrapper.UserID)
                            End If
                        Next
                    End If

                    'Refresh the Grid.
                    grdColorSizeLevel.Rebind()

                    For Each dataItem As GridDataItem In grdColorSizeLevel.Items
                        If ColorFamilyFloodErrorList.Count > 0 Then
                            If ColorFamilyFloodErrorList.Contains(CStr(dataItem.GetDataKeyValue("TurnInMerchID").ToString)) Then
                                CType(dataItem("ColorFamily").FindControl("imgCFError"), Image).Visible = True
                            End If
                        End If
                    Next

                    'Reset the Flood values.
                    btnResetFlood_Click(Nothing, Nothing)
                    Dim ErrorRowCount As Integer = ColorFamilyFloodErrorList.Distinct.Count
                    Dim SavedRowCount As Integer = selRowCount - ErrorRowCount

                    mpPreTurnInCreate.ErrorMessage = CStr(SavedRowCount) & If(SavedRowCount = 1, " Record ", " Records ") & "Saved Successfully. " & If(ErrorRowCount > 0, CStr(ErrorRowCount) & If(ErrorRowCount = 1, " Record ", " Records ") & "with errors. ", "")
                Else
                    mpPreTurnInCreate.PopUpMessage = "FLOOD not allowed on already Submitted record(s). Uncheck Submitted record(s)."
                End If
            Else
                mpPreTurnInCreate.PopUpMessage = "Select at least one record to Flood."
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
        rtxtFloodFrndPrdDesc.Text = ""
        rtxtFloodFrndPrdFeat.Text = ""
        rtxtFloodFriendlyColor.Text = ""

        For Each cb As RadComboBoxItem In rcbFloodColorFamily.Items
            If cb.Checked Then
                cb.Checked = False
            End If
        Next
        rcbFloodColorFamily.Text = ""

        rcbFloodClrCorrect.ClearSelection()
        rcbFloodClrCorrect.Text = ""

        rcbFloodImgKind.ClearSelection()
        rcbFloodImgKind.Text = ""

        rcbFloodImgType.ClearSelection()
        rcbFloodImgType.Text = ""

        rcbFloodAltView.ClearSelection()
        rcbFloodAltView.Text = ""

        rcbFloodSampleStr.ClearSelection()
        rcbFloodSampleStr.Text = ""

        rtxtFloodMerchNotes.Text = ""
    End Sub

    Protected Sub ttvcmbLabel_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
        Dim ctrl As RadComboBox = DirectCast(validator.EvaluatedControl, RadComboBox)
        If Not ctrl.SelectedIndex > 0 Then
            mpPreTurnInCreate.ErrorMessage = "Errors on Page."
            validator.ErrorMessage = "Required."
            args.IsValid = False
        End If
    End Sub
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
                    mpPreTurnInCreate.ErrorMessage = "Errors on Page."
                    validator.ErrorMessage = "Invalid Character Found."
                    args.IsValid = False
                End If
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub UpdatePrintLabelFlg()
        Try
            Dim selRowCount As Integer = grdColorSizeLevel.MasterTableView.GetSelectedItems.Count

            If selRowCount > 0 Then
                Dim TurnInMerchIDs As New List(Of String)

                For Each gridSelItem As GridDataItem In grdColorSizeLevel.MasterTableView.GetSelectedItems
                    'Get all the TurnIn MerchIDs for the selected rows that have a valid Admin Merch Number.
                    If CInt(gridSelItem.GetDataKeyValue("AdminMerchNum").ToString) > 0 Then
                        TurnInMerchIDs.Add(CStr(gridSelItem.GetDataKeyValue("TurnInMerchID")))
                    End If
                Next

                If TurnInMerchIDs.Count > 0 Then
                    'Update the Print Label Flag to Y, in the database.
                    _TUEcommSetupCreate.UpdatePrintLblFlg(String.Join(",", TurnInMerchIDs), SessionWrapper.UserID)
                    grdColorSizeLevel.Rebind()

                    mpPreTurnInCreate.ErrorMessage = TurnInMerchIDs.Count.ToString & " record(s) printed successfully."
                Else
                    mpPreTurnInCreate.PopUpMessage = "Select at least one valid record for Printing."
                End If

            Else
                mpPreTurnInCreate.PopUpMessage = "Select at least one record for Printing."
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub SubmitColorSizeData()
        Try
            Dim readyToSubmit As Boolean = True
            grdColorSizeLevel.AllowPaging = False
            grdColorSizeLevel.MasterTableView.Rebind()

            Dim invalidImageGroupIDs As String = String.Empty
            invalidImageGroupIDs = GetInvalidImageGroups(grdColorSizeLevel)
            If Not String.IsNullOrEmpty(invalidImageGroupIDs) Then
                mpPreTurnInCreate.PopUpMessage = String.Format("Image group number cannot be greater than 9. Please check and correct the following image group(s): \n{0}", invalidImageGroupIDs)
                grdColorSizeLevel.AllowPaging = True
                grdColorSizeLevel.MasterTableView.Rebind()
                Exit Sub
            End If


            For Each item As GridDataItem In grdColorSizeLevel.MasterTableView.Items

                If CChar(item.GetDataKeyValue("RemoveMerchFlag")) = "N"c Then

                    If Trim(DirectCast(item("FriendlyProdDesc").FindControl("ltrFriendlyProdDesc"), Literal).Text) = "" Then
                        readyToSubmit = False
                        grdColorSizeLevel.Columns.FindByUniqueName("FriendlyProdDesc").HeaderText = "* Friendly Product Desc"
                        mpPreTurnInCreate.PopUpMessage = "Friendly Product Description was not entered for all rows."
                        Exit For
                    End If

                    If DirectCast(item("FriendlyColor").Controls(0), DataBoundLiteralControl).Text.Trim = "" Then
                        readyToSubmit = False
                        grdColorSizeLevel.Columns.FindByUniqueName("FriendlyColor").HeaderText = "* Friendly Color"
                        mpPreTurnInCreate.PopUpMessage = "Friendly Color was not entered for all rows."
                        Exit For
                    End If

                    If DirectCast(item("ColorFamily").FindControl("lblColorFamily"), Label).Text.Trim = "" Then
                        readyToSubmit = False
                        grdColorSizeLevel.Columns.FindByUniqueName("ColorFamily").HeaderText = "* Color Family"
                        mpPreTurnInCreate.PopUpMessage = "Color Family was not entered for all rows."
                        Exit For
                    End If

                    'Dim SizeFamilies As List(Of ClrSizLocLookUp)
                    'Dim ClrSizeLocLookupVals As New List(Of ClrSizLocLookUp)
                    'Dim ISN As Decimal = CDec(item.GetDataKeyValue("ISN"))
                    'Dim isreserveISN As String = item.GetDataKeyValue("IsReserve").ToString
                    'ClrSizeLocLookupVals = _TUEcommSetupCreate.GetClrSizeLocLookUp(ISN)
                    'If isreserveISN = "Y"c Then
                    '    SizeFamilies = ClrSizeLocLookupVals.Where(Function(x) x.Identifier = "SIZ_RSV").ToList
                    'Else
                    '    SizeFamilies = ClrSizeLocLookupVals.Where(Function(x) x.Identifier = "SIZ").ToList
                    'End If

                    'If SizeFamilies.Count > 0 And DirectCast(item("SampleSize").Controls(0), DataBoundLiteralControl).Text.Trim = "" Then
                    '    readyToSubmit = False
                    '    grdColorSizeLevel.Columns.FindByUniqueName("SampleSize").HeaderText = "* Sample Size"
                    '    mpPreTurnInCreate.PopUpMessage = "Sample Size was not entered for all rows."
                    '    Exit For
                    'End If

                    Dim selectedISN As Decimal = CDec(item.GetDataKeyValue("ISN"))
                    If _TUWebCat.GetWebCatByISN(selectedISN).Where(Function(x) x.DefaultCategoryFlag = True).ToList.Count = 0 Then
                        readyToSubmit = False
                        mpPreTurnInCreate.PopUpMessage = "Return to the ISN tab and select at least one primary Web Category for ISN: " & selectedISN & "."
                        Exit For
                    End If

                    ' -----     Submit page validation    -----
                    '   Require a sample / Admin Merch Number unless:
                    '       - the image comes from another ad, or
                    '       - the Image Kind is either "No Merch", "Vendor", "P/U", "CR8", or
                    '       - the Image Kind is "DUP" AND the Alternate View is not "SWREF"
                    Dim routeFromAd As Integer = CInt(DirectCast(item("RouteFromAd").FindControl("ltrlRouteFromAd"), Literal).Text.Trim())
                    Dim imageKind As String = DirectCast(item("ImageKind").FindControl("ltrlImageKind"), Literal).Text.Trim()
                    Dim alternateView As String = DirectCast(item("AlternateView").FindControl("ltrlAltView"), Literal).Text.Trim()

                    Dim adminMerchNum = CInt(item.GetDataKeyValue("AdminMerchNum"))
                    Dim turnInMerchID = CInt(item.GetDataKeyValue("TurnInMerchID"))

                    If routeFromAd = 0 And (String.Compare(imageKind, "new", True) = 0 Or (String.Compare(imageKind, "DUP", True) = 0 And String.Compare(alternateView, "Swatch Ref", True) = 0)) Then

                        ' validate the sample / merch ID has been set
                        Dim sampleMerchId = CInt(item.GetDataKeyValue("SampleMerchId"))

                        ' A sample is required
                        If sampleMerchId = 0 Then

                            readyToSubmit = False
                            grdColorSizeLevel.Columns.FindByUniqueName("Sample").HeaderText = "* Sample"
                            mpPreTurnInCreate.PopUpMessage = "Samples have not been selected for all merchandise."
                            Exit For

                        ElseIf sampleMerchId <> adminMerchNum Then

                            Dim sampleSize = CInt(item.GetDataKeyValue("SampleSize"))
                            _TUEcommSetupCreate.UpdateMerchId(turnInMerchID, sampleMerchId, sampleSize, SessionWrapper.UserID)

                        End If
                    ElseIf (adminMerchNum <> 0) Then

                        ' Revert to no sample
                        _TUEcommSetupCreate.UpdateMerchId(turnInMerchID, 0, 0, SessionWrapper.UserID)
                    End If

                    ' -----     Submit page validation    -----

                End If
            Next

            If readyToSubmit Then
                _TUEcommSetupCreate.SubmitColorSizeData(CurrentBatch.BatchId, SessionWrapper.UserID)
                grdColorSizeLevel.Rebind()
                mpPreTurnInCreate.ErrorMessage = "Record(s) submitted successfully."
            End If

            grdColorSizeLevel.AllowPaging = True
            grdColorSizeLevel.MasterTableView.Rebind()
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Function SaveRows(ByRef rg As RadGrid) As Boolean

        For Each item As GridItem In rg.MasterTableView.Items
            If TypeOf item Is GridEditableItem And item.IsInEditMode Then
                If Not grdColorSizeLevelUpdateRow(CType(item, GridEditableItem)) Then
                    Return False
                End If
            End If
        Next
        mpPreTurnInCreate.ErrorMessage = "Data Saved Successfully."
        rg.Rebind()
        Return True

    End Function
#End Region

#Region "Show/Hide Page Elements"

    Private Sub PutRowsInEditMode(ByRef rg As RadGrid, ByVal isEdit As Boolean)
        For Each item As GridItem In rg.MasterTableView.Items
            If TypeOf item Is GridEditableItem Then
                Dim editableItem As GridEditableItem = CType(item, GridDataItem)
                If editableItem.GetDataKeyValue("RemoveMerchFlag").ToString = "N" Then
                    editableItem.Edit = isEdit
                End If
            End If
        Next
        rg.Rebind()
    End Sub

    Private Sub ShowHideButtons(ByVal ButtonName As String, ByVal Show As Boolean)
        rtbPreTurnInCreate.FindItemByText(ButtonName).Visible = Show
    End Sub

    Private Sub ShowHideTabs(ByVal TabId As String, ByVal Show As Boolean)
        rtsAPAdjustment.FindTabByText(TabId).Visible = Show
    End Sub

    Private Sub EnableDisableButtons(ByVal ButtonName As String, ByVal Show As Boolean)
        rtbPreTurnInCreate.FindItemByText(ButtonName).Enabled = Show
    End Sub

    Public Sub EditModeOnOff(ByVal OnOff As String)
        If OnOff = "Off" Then
            rtbPreTurnInCreate.FindItemByText("Edit All").Visible = True
            rtbPreTurnInCreate.FindItemByText("Save All").Visible = False
            rtbPreTurnInCreate.FindItemByText("Cancel All").Visible = False
        Else
            rtbPreTurnInCreate.FindItemByText("Edit All").Visible = False
            rtbPreTurnInCreate.FindItemByText("Save All").Visible = True
            rtbPreTurnInCreate.FindItemByText("Cancel All").Visible = True
        End If
    End Sub

#End Region

#Region "Validations"
    'Public Sub ttvWebCatLevel2_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
    '    Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
    '    Dim cb As RadComboBox = DirectCast(validator.EvaluatedControl, RadComboBox)
    '    If cb.SelectedItem.Text.ToLower.Contains("display only") Then
    '        validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1015)
    '        args.IsValid = False
    '    End If
    'End Sub

    'Public Sub ttvWebCatLevel3_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
    '    Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
    '    Dim cb As RadComboBox = DirectCast(validator.EvaluatedControl, RadComboBox)
    '    If cb.SelectedItem.Text.ToLower.Contains("display only") Then
    '        validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1015)
    '        args.IsValid = False
    '    End If
    'End Sub

    'Public Sub ttvWebCatLevel4_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
    '    Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
    '    Dim cb As RadComboBox = DirectCast(validator.EvaluatedControl, RadComboBox)
    '    If cb.SelectedItem.Text.ToLower.Contains("display only") Then
    '        validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1015)
    '        args.IsValid = False
    '    End If
    'End Sub

    'Public Sub ttvWebCatLevel5_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
    '    Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
    '    Dim cb As RadComboBox = DirectCast(validator.EvaluatedControl, RadComboBox)
    '    If cb.SelectedItem.Text.ToLower.Contains("display only") Then
    '        validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1015)
    '        args.IsValid = False
    '    End If
    'End Sub

    'Public Sub ttvWebCatLevel6_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
    '    Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
    '    Dim cb As RadComboBox = DirectCast(validator.EvaluatedControl, RadComboBox)
    '    If cb.SelectedItem.Text.ToLower.Contains("display only") Then
    '        validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1015)
    '        args.IsValid = False
    '    End If
    'End Sub
#End Region

#Region "Grid Events"
    Private Sub grdMaint_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdMaint.ItemDataBound
        If TypeOf (e.Item) Is GridDataItem Then
            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
            Dim api As AdPageInfo = _TUCtlgAdPg.GetAdPageDetail(CInt(dataItem("AdNumber").Text), CInt(dataItem("PageNumber").Text))
            dataItem("AdDescription").Text = api.AdDesc
            dataItem("TurnInDate").Text = CStr(api.TUDate)
            dataItem("PageDescription").Text = api.PageDesc
        End If
    End Sub

    Private Sub grdKilled_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdKilled.NeedDataSource
        grdKilled.DataSource = _TUEcommSetupCreate.GetKilledItems(eCommPreTurnInSetUpCtrl.SelectedBatchId, eCommPreTurnInSetUpCtrl.AdComboBox.SelectedValue, eCommPreTurnInSetUpCtrl.PageNumberComboBox.SelectedValue).ToList
        grdKilled.Visible = True
    End Sub

    Private Sub grdWebCategories_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdWebCategories.NeedDataSource
        grdWebCategories.DataSource = SelectedWebCats
    End Sub

    Private Sub HeaderContextMenu_ItemCreated(ByVal sender As Object, ByVal e As RadMenuEventArgs)
        Dim menuitem As RadMenuItem = e.Item
        If menuitem.Level = 1 Then
            If menuitem.Text = "Group By" Or menuitem.Text = "Ungroup" Then
                menuitem.Visible = False
            End If
        End If
        If menuitem.Level = 2 AndAlso DirectCast(menuitem.Parent, RadMenuItem).Value = "ColumnsContainer" Then
            If menuitem.Text = "selColumn" _
            Or menuitem.Text = "EditCommandColumn" _
            Or menuitem.Text = "DeleteColumn" _
            Or menuitem.Text = "Rush Sample" _
            Or menuitem.Text = "Friendly Product Desc" _
            Or menuitem.Text = "Friendly Product Features" _
            Or menuitem.Text = "Friendly Color" _
            Or menuitem.Text = "Color Family" _
            Or menuitem.Text = "Sample Size" _
            Or menuitem.Text = "Color Correct" _
            Or menuitem.Text = "Image Kind" _
            Or menuitem.Text = "P/U ImageID" _
            Or menuitem.Text = "Route from Ad" _
            Or menuitem.Text = "Image Group #" _
            Or menuitem.Text = "Feature/ Render/ Swatch" _
            Or menuitem.Text = "On/ off Figure" _
            Or menuitem.Text = "Alt. View" _
            Or menuitem.Text = "Sample Store #" _
            Or menuitem.Text = "Merchant Notes" Then
                menuitem.Visible = False
            End If
        End If
    End Sub
    Private Sub grdMaint_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMaint.NeedDataSource
        grdMaint.DataSource = _TUBatch.GetMaintenanceBatches(eCommPreTurnInSetUpCtrl.SelectedAd, _
                        CInt(If(String.IsNullOrEmpty(eCommPreTurnInSetUpCtrl.PageNumberComboBox.SelectedValue), "0", eCommPreTurnInSetUpCtrl.PageNumberComboBox.SelectedValue)), _
                        eCommPreTurnInSetUpCtrl.SelectedBuyerId, eCommPreTurnInSetUpCtrl.SelectedDeptId, eCommPreTurnInSetUpCtrl.SelectedBatchId)
        grdMaint.MasterTableView.DataKeyNames = New String() {"BatchId"}
    End Sub
#End Region

#Region "Helper Methods"
    Function GetAdWeek(ByVal adNo As String) As String
        Dim _TUCtlgAdPg As New TUCtlgAdPg
        Dim _TUAdInfo As New TUAdInfo
        Dim returnVal As String = ""

        Dim ad As AdInfoInfo = _TUAdInfo.GetAdInfoByAdNbr(CInt(adNo))
        If ad IsNot Nothing Then
            returnVal = ad.addesc
        End If
        Return returnVal
    End Function
    ''' <summary>
    ''' Gets turn in filter user selections
    ''' </summary>
    ''' <returns>Turn in filter user selections</returns>
    ''' <remarks>
    ''' This method will throw runtime exception if any of these controls removed from the user control.  So this method should be updated if the below mentioned control(s) are removed.
    ''' </remarks>
    Private Function GetTurnInFilters() As TUFilter
        Dim turnInFilter As New TUFilter
        turnInFilter.AvailableForTurnIn = DirectCast(Me.eCommPreTurnInSetUpCtrl.FindControl("chkAvailableAndApproved"), CheckBox).Checked
        turnInFilter.NotAvailableForTurnIn = DirectCast(Me.eCommPreTurnInSetUpCtrl.FindControl("chkNotAvailable"), CheckBox).Checked
        turnInFilter.ActiveOnWeb = DirectCast(Me.eCommPreTurnInSetUpCtrl.FindControl("chkActiveOnWeb"), CheckBox).Checked
        turnInFilter.NotActiveOnWeb = DirectCast(Me.eCommPreTurnInSetUpCtrl.FindControl("chkNotActiveOnWeb"), CheckBox).Checked
        turnInFilter.NotTurnedIn = DirectCast(Me.eCommPreTurnInSetUpCtrl.FindControl("chkTurnInType"), CheckBox).Checked
        Return turnInFilter
    End Function
    ''' <summary>
    ''' Caches the data at batch level to avoid repetitive calls to the database
    ''' </summary>
    ''' <remarks>This was added to fix timeout/runtime errors on color/size tab with large batches</remarks>
    Private Sub CacheBatchLevelData()
        If Session("RouteAd") Is Nothing Then
            Session("RouteAd") = _TUEcommSetupCreate.GetRouteFrmAdLookUp(CurrentBatch.AdNumber)
        End If

        If Session("ImageKind") Is Nothing Then
            Session("ImageKind") = _TUTMS900PARAMETER.GetAllImageKindValues
        End If

        If Session("RenderSwatch") Is Nothing Then
            Session("RenderSwatch") = _TUTMS900PARAMETER.GetAllFeatureRenderSwatchValues
        End If

        If Session("AltView") Is Nothing Then
            Session("AltView") = _TUTMS900PARAMETER.GetAllAltViewValues
        End If

        If Session("MDSEFigureCode") Is Nothing Then
            Session("MDSEFigureCode") = _TUTMS900PARAMETER.GetAllMDSEFigureCodes()
        End If
    End Sub
    ''' <summary>
    ''' Iterates through all items in the collection and checks whether image group number is greater than 9. 
    ''' If any image group has value greater than 9, then it returns the image group id otherwise empty string
    ''' </summary>
    ''' <param name="grid">Grid for which image group mappping needs be validated</param>
    ''' <returns>Image group number greater than 9</returns>
    ''' <remarks></remarks>
    Private Function GetInvalidImageGroups(ByRef grid As RadGrid) As String
        Dim invalidGroupIds As String = String.Empty

        Dim imageGroupCount = From gridItems In grid.MasterTableView.Items
                              Select gridItems
                              Where CType(gridItems, GridDataItem).GetDataKeyValue("RemoveMerchFlag").ToString().ToUpper() = "N" _
                              AndAlso DirectCast(CType(gridItems, GridDataItem)("GroupNum").Controls(0), DataBoundLiteralControl).Text.Trim() <> String.Empty AndAlso _
                                DirectCast(CType(gridItems, GridDataItem)("GroupNum").Controls(0), DataBoundLiteralControl).Text.Trim() <> "0" AndAlso _
                                CInt(DirectCast(CType(gridItems, GridDataItem)("GroupNum").Controls(0), DataBoundLiteralControl).Text.Trim()) > 9

        If Not imageGroupCount Is Nothing Then
            For Each item In imageGroupCount
                invalidGroupIds = String.Concat(invalidGroupIds, DirectCast(CType(item, GridDataItem)("GroupNum").Controls(0), DataBoundLiteralControl).Text.Trim(), "\n")
            Next
        End If

        Return invalidGroupIds
    End Function
#End Region

    Private Sub grdResultList_PreRender(sender As Object, e As EventArgs) Handles grdResultList.PreRender
        If eCommPreTurnInSetUpCtrl.ResultsTabRadPanelBar.Items(0).Expanded Then
            'The below fields should be displayed only if "Filter by PO Start Ship Date" filter was selected
            grdResultList.MasterTableView.GetColumn("WebCatAvailableQty").Visible = True
            grdResultList.MasterTableView.GetColumn("StartShipDate").Visible = True
            grdResultList.MasterTableView.DetailTables(0).Columns.FindByUniqueName("OnOrderByShipDate").Visible = True
            grdResultList.MasterTableView.DetailTables(0).Columns.FindByUniqueName("WebCatAvailableQuantity").Visible = True
            grdResultList.MasterTableView.DetailTables(0).Columns.FindByUniqueName("POStartShipDate").Visible = True

            'The below fields should be hidden if "Filter by PO Start Ship Date" filter was selected
            grdResultList.MasterTableView.GetColumn("IsReserve").Visible = False
            grdResultList.MasterTableView.GetColumn("ACode").Visible = False
            grdResultList.MasterTableView.GetColumn("SellYear").Visible = False
            grdResultList.MasterTableView.GetColumn("SellSeason").Visible = False
            grdResultList.MasterTableView.GetColumn("IsTurnedInPrint").Visible = False
            grdResultList.MasterTableView.GetColumn("IsTurnedInEcomm").Visible = False
            grdResultList.MasterTableView.GetColumn("OH").Visible = False
            'second level grid
            grdResultList.MasterTableView.DetailTables(0).Columns.FindByUniqueName("SellYear").Visible = False
            grdResultList.MasterTableView.DetailTables(0).Columns.FindByUniqueName("SellSeason").Visible = False
            grdResultList.MasterTableView.DetailTables(0).Columns.FindByUniqueName("OnHand").Visible = False
            grdResultList.MasterTableView.DetailTables(0).Columns.FindByUniqueName("OnOrder").Visible = False
            grdResultList.MasterTableView.DetailTables(0).Columns.FindByUniqueName("IsTurnedInPrint").Visible = False
            grdResultList.MasterTableView.DetailTables(0).Columns.FindByUniqueName("IsTurnedInEcomm").Visible = False
        Else
            'If the page doesn't load from scratch, these fields might stay invisible, so make them visible
            grdResultList.MasterTableView.GetColumn("IsReserve").Visible = True
            grdResultList.MasterTableView.GetColumn("ACode").Visible = True
            grdResultList.MasterTableView.GetColumn("SellYear").Visible = True
            grdResultList.MasterTableView.GetColumn("SellSeason").Visible = True
            grdResultList.MasterTableView.GetColumn("IsTurnedInPrint").Visible = True
            grdResultList.MasterTableView.GetColumn("IsTurnedInEcomm").Visible = True
            grdResultList.MasterTableView.GetColumn("OH").Visible = True
            grdResultList.MasterTableView.DetailTables(0).Columns.FindByUniqueName("SellYear").Visible = True
            grdResultList.MasterTableView.DetailTables(0).Columns.FindByUniqueName("SellSeason").Visible = True
            grdResultList.MasterTableView.DetailTables(0).Columns.FindByUniqueName("OnHand").Visible = True
            grdResultList.MasterTableView.DetailTables(0).Columns.FindByUniqueName("OnOrder").Visible = True
            grdResultList.MasterTableView.DetailTables(0).Columns.FindByUniqueName("IsTurnedInPrint").Visible = True
            grdResultList.MasterTableView.DetailTables(0).Columns.FindByUniqueName("IsTurnedInEcomm").Visible = True


            'The below fields should be hidden if "Filter by PO Start Ship Date" filter was not selected
            grdResultList.MasterTableView.GetColumn("WebCatAvailableQty").Visible = False
            grdResultList.MasterTableView.GetColumn("StartShipDate").Visible = False
            grdResultList.MasterTableView.DetailTables(0).Columns.FindByUniqueName("OnOrderByShipDate").Visible = False
            grdResultList.MasterTableView.DetailTables(0).Columns.FindByUniqueName("WebCatAvailableQuantity").Visible = False
            grdResultList.MasterTableView.DetailTables(0).Columns.FindByUniqueName("POStartShipDate").Visible = False
        End If
    End Sub
End Class


