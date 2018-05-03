Option Infer On

Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL.MiscConstants
Imports BonTon.Common.ExportFunctions
Imports TurnInProcessAutomation.BLL.Enumerations
Public Class eCommTurnInMeet
    Inherits PageBase
    Private _eCommTurnInMaintCtrl As eCommTurnInMeetCtrl = Nothing
    Dim _TUECommTurnInMeetResults As New TUECommTurnInMeetResults
    Dim _TUTMS900PARAMETER As New TUTMS900PARAMETER
    Private _TUAdInfo As New TUAdInfo
    Private _emmExpandedState As Hashtable
    Private _ccExpandedState As Hashtable
    Private _ccSimpleExpandedState As Hashtable
    Private _copyWriterExpandedState As Hashtable
    Dim currentTabIndex As Integer
    Dim previousTabIndex As Integer
    Dim _TUEcommSetupCreate As New TUEcommSetupCreate
    Dim _TUWebCategories As New TUWebCat
    Dim _TULabel As New TULabel
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

    'Save/load expanded states Hash from the session
    Private ReadOnly Property EMMExpandedStates() As Hashtable
        Get
            If Me._emmExpandedState Is Nothing Then
                _emmExpandedState = TryCast(Me.Session("EMM_ExpandedState"), Hashtable)
                If _emmExpandedState Is Nothing Then
                    _emmExpandedState = New Hashtable()
                    Me.Session("EMM_ExpandedState") = _emmExpandedState
                End If
            End If

            Return Me._emmExpandedState
        End Get
    End Property

    Private ReadOnly Property CCExpandedStates() As Hashtable
        Get
            If Me._ccExpandedState Is Nothing Then
                _ccExpandedState = TryCast(Me.Session("CC_ExpandedState"), Hashtable)
                If _ccExpandedState Is Nothing Then
                    _ccExpandedState = New Hashtable()
                    Me.Session("CC_ExpandedState") = _ccExpandedState
                End If
            End If

            Return Me._ccExpandedState
        End Get
    End Property

    Private ReadOnly Property CCSimpleExpandedStates() As Hashtable
        Get
            If Me._ccSimpleExpandedState Is Nothing Then
                _ccSimpleExpandedState = TryCast(Me.Session("CCSimple_ExpandedState"), Hashtable)
                If _ccSimpleExpandedState Is Nothing Then
                    _ccSimpleExpandedState = New Hashtable()
                    Me.Session("CCSimple_ExpandedState") = _ccSimpleExpandedState
                End If
            End If

            Return Me._ccSimpleExpandedState
        End Get
    End Property

    Private ReadOnly Property CopyWriterExpandedStates() As Hashtable
        Get
            If Me._copyWriterExpandedState Is Nothing Then
                _copyWriterExpandedState = TryCast(Me.Session("CopyWriter_ExpandedState"), Hashtable)
                If _copyWriterExpandedState Is Nothing Then
                    _copyWriterExpandedState = New Hashtable()
                    Me.Session("CopyWriter_ExpandedState") = _copyWriterExpandedState
                End If
            End If

            Return Me._copyWriterExpandedState
        End Get
    End Property
#End Region

#Region "Page_Events"

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Not Page.IsPostBack Then

            End If
            Dim control As Control = LoadControl("~/WebUserControls/eCommerce/eCommTurnInMeetCtrl.ascx")

            If Not control Is Nothing Then
                control.ID = "eCommTurnInMeetCtrl"
                Me.Master.SideBarPlaceHolder.Controls.Add(control)
            End If

            If TypeOf control Is eCommTurnInMeetCtrl Then
                Me._eCommTurnInMaintCtrl = CType(control, eCommTurnInMeetCtrl)
            ElseIf TypeOf control Is PartialCachingControl And CType(control, PartialCachingControl).CachedControl IsNot Nothing Then
                Me._eCommTurnInMaintCtrl = CType(CType(control, PartialCachingControl).CachedControl, eCommTurnInMeetCtrl)
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Me.Master.SideBar.Width = 200
                EnableDisableButtons("Export", False)
                EnableDisableButtons("Submit", False)
                EnableDisableButtons("Reject", False)
                EnableDisableButtons("Delete", False)
                EnableDisableButtons("Sort", False)
                EnableDisableButtons("Retrieve", True)

                Me._emmExpandedState = Nothing
                Me.Session("EMM_ExpandedState") = Nothing

                Me._ccExpandedState = Nothing
                Me.Session("CC_ExpandedState") = Nothing

                Me._copyWriterExpandedState = Nothing
                Me.Session("CopyWriter_ExpandedState") = Nothing
            Else
                'SetAccessToEditAll()
                Dim action As String = Request.QueryString("Action")
                If action <> Nothing Then
                    If (Request.QueryString("Action").ToUpper = "INQUIRY") Then
                        With grdEMM
                            .MasterTableView.Columns.FindByUniqueName("EditCommandColumn").Visible = False
                            .MasterTableView.Columns.FindByUniqueName("DeleteColumn").Visible = False
                        End With

                        With grdCC
                            .MasterTableView.Columns.FindByUniqueName("EditCommandColumn").Visible = False
                            .MasterTableView.Columns.FindByUniqueName("DeleteColumn").Visible = False
                        End With

                        With grdCopyWriter
                            .MasterTableView.Columns.FindByUniqueName("EditCommandColumn").Visible = False
                            .MasterTableView.Columns.FindByUniqueName("DeleteColumn").Visible = False
                        End With

                        EnableDisableButtons("Submit", False)
                    End If
                End If
                If (checkAccessability("EmmTab") = "False") Then

                    With grdEMM
                        .MasterTableView.Columns.FindByUniqueName("EditCommandColumn").Visible = False
                        .MasterTableView.Columns.FindByUniqueName("DeleteColumn").Visible = False
                    End With
                End If

                If (checkAccessability("CoordTab") = "False") Then
                    With grdCC
                        .MasterTableView.Columns.FindByUniqueName("EditCommandColumn").Visible = False
                        .MasterTableView.Columns.FindByUniqueName("DeleteColumn").Visible = False
                    End With
                End If

                If (checkAccessability("CopyTab") = "False") Then
                    With grdCopyWriter
                        .MasterTableView.Columns.FindByUniqueName("EditCommandColumn").Visible = False
                        .MasterTableView.Columns.FindByUniqueName("DeleteColumn").Visible = False
                    End With
                End If
            End If

            AddHandler grdCC.HeaderContextMenu.ItemCreated, AddressOf Me.HeaderContextMenu_ItemCreated
            AddHandler Me.piModalExport.OkButton.Click, AddressOf Me.ExportList
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

#End Region

#Region "Methods"

    Private Sub rtsTurnInMeet_TabClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTabStripEventArgs) Handles rtsTurnInMeet.TabClick
        Try
            SetAccessToEditAll()
            Dim submitButton As RadToolBarItem = rtbeCommTurnInMeet.FindItemByText("Submit")
            Select Case e.Tab.PageViewID
                Case "pvCopyWriter"
                    grdCopyWriter.DataSource = Nothing
                    grdCopyWriter.Rebind()
                    previousTabIndex = CInt(Session("cpIndex"))
                    Session("ppIndex") = previousTabIndex
                    submitButton.Enabled = False
                    EnableDisableButtons("Reject", False)
                    currentTabIndex = 2
                    Session("cpIndex") = currentTabIndex
                Case "pveMM"
                    grdEMM.DataSource = Nothing
                    grdEMM.Rebind()
                    previousTabIndex = CInt(Session("cpIndex"))
                    Session("ppIndex") = previousTabIndex
                    If (Request.QueryString("Action").ToUpper <> "INQUIRY" And checkAccessability("EmmTab") = "True") Then
                        submitButton.Enabled = True
                        EnableDisableButtons("Reject", True)
                    End If
                    currentTabIndex = 0
                    Session("cpIndex") = currentTabIndex
                Case "pvCC"
                    grdCC.DataSource = Nothing
                    grdCC.Rebind()
                    previousTabIndex = CInt(Session("cpIndex"))
                    Session("ppIndex") = previousTabIndex
                    If (Request.QueryString("Action").ToUpper <> "INQUIRY" And checkAccessability("CoordTab") = "True") Then
                        submitButton.Enabled = True
                        EnableDisableButtons("Reject", True)
                    End If
                    currentTabIndex = 1
                    Session("cpIndex") = currentTabIndex
            End Select
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub rtbeCommTurnInMeet_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles rtbeCommTurnInMeet.ButtonClick
        Try
            If TypeOf e.Item Is RadToolBarButton Then
                Dim radToolBarButton As RadToolBarButton = DirectCast(e.Item, RadToolBarButton)
                Select Case radToolBarButton.CommandName
                    Case "Retrieve"
                        If _eCommTurnInMaintCtrl.IsSearchValid() Then
                            Me._emmExpandedState = Nothing
                            Me.Session("EMM_ExpandedState") = Nothing

                            Me._ccExpandedState = Nothing
                            Me.Session("CC_ExpandedState") = Nothing

                            Me._copyWriterExpandedState = Nothing
                            Me.Session("CopyWriter_ExpandedState") = Nothing

                            grdEMM.CurrentPageIndex = 0
                            grdEMM.Rebind()
                            grdEMM.ClientSettings.Scrolling.ScrollTop = "0" 'To reset Scroll position to top

                            grdCopyWriter.CurrentPageIndex = 0
                            grdCopyWriter.Rebind()
                            grdCopyWriter.ClientSettings.Scrolling.ScrollTop = "0" 'To reset Scroll position to top

                            grdCC.CurrentPageIndex = 0
                            grdCC.Rebind()
                            grdCC.ClientSettings.Scrolling.ScrollTop = "0" 'To reset Scroll position to top

                            EnableDisableButtons("Export", True)
                            EnableDisableButtons("Delete", True)

                            If grdEMM.Visible Or grdCopyWriter.Visible Or grdCC.Visible Then
                                SetAccessToEditAll()
                            End If

                            If (checkAccessability("CoordTab") = "True") Then
                                EnableDisableButtons("Sort", True)
                            End If

                            If Request.QueryString("Action").ToUpper <> "INQUIRY" _
                                And (checkAccessability("CoordTab") = "True" Or checkAccessability("EmmTab") = "True") _
                                And CInt(Session("cpIndex")) <> 2 Then
                                EnableDisableButtons("Submit", True)
                                EnableDisableButtons("Reject", True)
                            End If
                        Else
                            mpeCommTurnInMaint.ErrorMessage = "Batch is required."
                            Return
                        End If
                    Case "Back"
                        previousTabIndex = CInt(Session("ppIndex"))
                        currentTabIndex = CInt(Session("cpIndex"))
                        If pveMM.Selected Then
                            If grdEMM.MasterTableView.ChildEditItems.Count > 0 Then
                                mpeCommTurnInMaint.ErrorMessage = "At least one record is being edited. Save/Cancel the record and then click Back button."
                                Exit Sub
                            End If
                        ElseIf pvCC.Selected Then
                            If grdCC.MasterTableView.ChildEditItems.Count > 0 Then
                                mpeCommTurnInMaint.ErrorMessage = "At least one record is being edited. Save/Cancel the record and then click Back button."
                                Exit Sub
                            End If
                        ElseIf pvCopyWriter.Selected Then
                            If grdCopyWriter.MasterTableView.ChildEditItems.Count > 0 Then
                                mpeCommTurnInMaint.ErrorMessage = "At least one record is being edited. Save/Cancel the record and then click Back button."
                                Exit Sub
                            End If
                        End If

                        Select Case (previousTabIndex)
                            Case 0
                                rtsTurnInMeet.Tabs(0).Selected = True
                                rmpTurnInMeet.SelectedIndex = 0
                            Case 1
                                rtsTurnInMeet.Tabs(1).Selected = True
                                rmpTurnInMeet.SelectedIndex = 1
                            Case 2
                                rtsTurnInMeet.Tabs(2).Selected = True
                                rmpTurnInMeet.SelectedIndex = 2
                            Case Else
                                Response.Redirect(PreviousPageUrl)
                        End Select
                    Case "Submit"
                        'Nullify the objects that store Grid Group Header Item's Expand/Collapse state.
                        Me._emmExpandedState = Nothing
                        Me.Session("EMM_ExpandedState") = Nothing

                        Me._ccExpandedState = Nothing
                        Me.Session("CC_ExpandedState") = Nothing

                        Me._copyWriterExpandedState = Nothing
                        Me.Session("CopyWriter_ExpandedState") = Nothing

                        grdEMMSubmit()
                    Case "EditAll"
                        EditModeOnOff("On")
                        If rtsTurnInMeet.SelectedIndex = 0 Then
                            PutRowsInEditMode(grdEMM, True)
                        ElseIf rtsTurnInMeet.SelectedIndex = 1 Then
                            PutRowsInEditMode(grdCC, True)
                            'ElseIf rtsTurnInMeet.SelectedIndex = 2 Then
                            '    PutRowsInEditMode(grdCCSimple, True)
                        ElseIf rtsTurnInMeet.SelectedIndex = 2 Then
                            PutRowsInEditMode(grdCopyWriter, True)
                        End If
                    Case "SaveAll"
                        Session("eCommTurnInMeet.grdEmm.LastUpdatedRowId") = ""
                        Session("eCommTurnInMeet.grdCC.LastUpdatedRowId") = ""
                        'Session("eCommTurnInMeet.grdCCSimple.LastUpdatedRowId") = ""
                        Session("eCommTurnInMeet.grdCopyWriter.LastUpdatedRowId") = ""
                        If rtsTurnInMeet.SelectedIndex = 0 Then
                            If SaveRows(grdEMM) Then
                                PutRowsInEditMode(grdEMM, False)
                                grdEMM.Columns.FindByUniqueName("FriendlyProdDesc").HeaderText = "Friendly Product Desc"
                                grdEMM.Columns.FindByUniqueName("WebCategories").HeaderText = "Web Categories"
                                EditModeOnOff("Off")
                                grdEMM.Rebind()
                            End If
                        ElseIf rtsTurnInMeet.SelectedIndex = 1 Then
                            If SaveRows(grdCC) Then
                                PutRowsInEditMode(grdCC, False)
                                EditModeOnOff("Off")
                                grdCC.Rebind()
                                mpeCommTurnInMaint.ErrorMessage = "Data Saved Successfully."
                            End If
                        ElseIf rtsTurnInMeet.SelectedIndex = 2 Then
                            If SaveRows(grdCopyWriter) Then
                                PutRowsInEditMode(grdCopyWriter, False)
                                EditModeOnOff("Off")
                                grdCopyWriter.Rebind()
                                mpeCommTurnInMaint.ErrorMessage = "Data Saved Successfully."
                            End If
                        End If
                    Case "CancelAll"
                        EditModeOnOff("Off")
                        If rtsTurnInMeet.SelectedIndex = 0 Then
                            PutRowsInEditMode(grdEMM, False)
                        ElseIf rtsTurnInMeet.SelectedIndex = 1 Then
                            PutRowsInEditMode(grdCC, False)
                            'ElseIf rtsTurnInMeet.SelectedIndex = 2 Then
                            '    PutRowsInEditMode(grdCCSimple, False)
                        ElseIf rtsTurnInMeet.SelectedIndex = 2 Then
                            PutRowsInEditMode(grdCopyWriter, False)
                        End If
                    Case "Reject"
                        'Nullify the objects that store Grid Group Header Item's Expand/Collapse state.
                        Me._emmExpandedState = Nothing
                        Me.Session("EMM_ExpandedState") = Nothing

                        Me._ccExpandedState = Nothing
                        Me.Session("CC_ExpandedState") = Nothing

                        Me._copyWriterExpandedState = Nothing
                        Me.Session("CopyWriter_ExpandedState") = Nothing

                        RejectBatch() 'Revert the batch status to PEND, from RDFM.
                    Case "Delete"
                        DeleteBatchItems()
                    Case "Default"

                End Select
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Public Sub EditModeOnOff(ByVal OnOff As String)
        If OnOff = "Off" Then
            Session("EditAll") = False
            rtbeCommTurnInMeet.FindItemByText("Edit All").Visible = True
            rtbeCommTurnInMeet.FindItemByText("Save All").Visible = False
            rtbeCommTurnInMeet.FindItemByText("Cancel All").Visible = False
        Else
            Session("EditAll") = True
            rtbeCommTurnInMeet.FindItemByText("Edit All").Visible = False
            rtbeCommTurnInMeet.FindItemByText("Save All").Visible = True
            rtbeCommTurnInMeet.FindItemByText("Cancel All").Visible = True
        End If
    End Sub

    Private Function ExtractUserName() As String
        Dim userPath As String = HttpContext.Current.User.Identity.Name
        Dim splitPath As String() = userPath.Split(New Char() {"\"c})
        Return splitPath((splitPath.Length - 1))
    End Function

    Private Function checkAccessability(ByVal Field As String) As String
        Dim saccess As String = "False"
        Dim log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
        Dim secSrv As New SecurityService.SecurityServiceSoapClient()
        Try
            saccess = secSrv.GetFieldAccess("MerchandiseTurn-InSystem", ExtractUserName(), Field)
        Catch ex As Exception
            log.Error(ex.Message)
        Finally
            secSrv = Nothing
        End Try
        Return saccess
    End Function

#End Region

#Region "FLOOD"

    Private Sub btnFloodEMM_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFloodEMM.Click
        Try
            Page.Validate("ValidateFloodEMM")

            If Not IsValid Then
                mpeCommTurnInMaint.ErrorMessage = "Errors on Page."
                Exit Sub
            End If

            'Exit the Sub-routine if no data is entered for "Flood" option.
            If String.IsNullOrEmpty(rtxtFloodWebCategories.Text.Trim) And _
                String.IsNullOrEmpty(rtxtFloodFrndPrdDesc.Text.Trim) And _
                String.IsNullOrEmpty(rtxtFloodFriendlyColor.Text.Trim) And _
                String.IsNullOrEmpty(rcbFloodSizeCategory.SelectedValue) And _
                String.IsNullOrEmpty(rtxtFloodEMMNotes.Text.Trim) Then
                mpeCommTurnInMaint.ErrorMessage = "Enter value for at least one Flood option."
                Exit Sub
            End If

            If String.IsNullOrEmpty(rtxtFloodWebCategories.Text.Trim) Then
                hfFloodWebCatCde.Value = String.Empty
            End If

            Dim selRowCount As Integer = grdEMM.MasterTableView.GetSelectedItems.Where(Function(x) CChar(x.GetDataKeyValue("RemoveMerchFlag")) = "N"c).Count

            If selRowCount > 0 Then
                Dim tooManyCategories As Boolean = False
                For Each item As GridDataItem In grdEMM.MasterTableView.GetSelectedItems()
                    Dim categories As String = item.GetDataKeyValue("WebCatgyList").ToString()
                    If categories.Split(","c).Count() > 4 Then
                        tooManyCategories = True
                        mpeCommTurnInMaint.ErrorMessage = "Error: Some items already have a maximum of 6 allowable web categories."
                        Exit For
                    End If
                Next

                If Not tooManyCategories Then

                    'Clear the Edit mode, if any row is left in Edit mode.
                    grdEMM.MasterTableView.ClearEditItems()

                    Dim TurnInMerchIDs As New List(Of String)
                    For Each gridSelItem As GridDataItem In grdEMM.MasterTableView.GetSelectedItems.Where(Function(x) CChar(x.GetDataKeyValue("RemoveMerchFlag")) = "N"c)
                        'Get all the Merch IDs for the selected rows.
                        TurnInMerchIDs.Add(CStr(gridSelItem.GetDataKeyValue("turnInMerchID")))
                    Next

                    'Update the data in the database.
                    _TUECommTurnInMeetResults.UpdateEMMFlood(String.Join(",", TurnInMerchIDs), hfFloodWebCatCde.Value, rtxtFloodFrndPrdDesc.Text.Trim, rtxtFloodEMMNotes.Text.Trim, rtxtFloodFriendlyColor.Text.Trim, rcbFloodSizeCategory.SelectedValue, SessionWrapper.UserID)

                    grdEMM.Rebind()

                    'Reset the Flood values.
                    btnResetFloodEMM_Click(Nothing, Nothing)

                    mpeCommTurnInMaint.ErrorMessage = CStr(selRowCount) & If(selRowCount = 1, " Record ", " Records ") & "Saved Successfully."

                End If
            Else
                mpeCommTurnInMaint.ErrorMessage = "Select at least one record to Flood."
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub btnFloodCC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFloodCC.Click
        Try
            Page.Validate("ValidateFloodCC")

            If Not IsValid Then
                mpeCommTurnInMaint.ErrorMessage = "Errors on Page."
                Exit Sub
            End If

            'Exit the Sub-routine if no data is entered for "Flood" option.
            If String.IsNullOrEmpty(rcbFloodImgType.SelectedValue) And _
                String.IsNullOrEmpty(rtxtFloodImageNotes.Text.Trim) And _
                String.IsNullOrEmpty(rtxtFloodStylingNotes.Text.Trim) And _
                String.IsNullOrEmpty(cmbFloodFeatureRenderSwatch.SelectedValue) And _
                String.IsNullOrEmpty(rcbFloodModelCategory.SelectedValue) And _
                String.IsNullOrEmpty(rcbFloodAlternateView.SelectedValue) Then
                mpeCommTurnInMaint.ErrorMessage = "Enter value for at least one Flood option."
                Exit Sub
            End If

            Dim selRowCount As Integer = grdCC.MasterTableView.GetSelectedItems.Where(Function(x) CChar(x.GetDataKeyValue("RemoveMerchFlag")) = "N"c).Count

            If selRowCount > 0 Then
                'Clear the Edit mode, if any row is left in Edit mode.
                grdCC.MasterTableView.ClearEditItems()

                Dim TurnInMerchIDs As New List(Of String)
                For Each gridSelItem As GridDataItem In grdCC.MasterTableView.GetSelectedItems.Where(Function(x) CChar(x.GetDataKeyValue("RemoveMerchFlag")) = "N"c)
                    'Get all the Merch IDs for the selected rows.
                    TurnInMerchIDs.Add(CStr(gridSelItem.GetDataKeyValue("turnInMerchID")))
                Next

                'Update the data in the database.
                _TUECommTurnInMeetResults.UpdateCCFlood(String.Join(",", TurnInMerchIDs), rcbFloodImgType.SelectedValue, rcbFloodModelCategory.SelectedValue, rcbFloodAlternateView.SelectedValue, cmbFloodFeatureRenderSwatch.SelectedValue, rtxtFloodImageNotes.Text.Trim, rtxtFloodStylingNotes.Text.Trim, SessionWrapper.UserID)

                grdCC.Rebind()
                'grdCCSimple.Rebind()

                'Reset the Flood values.
                btnResetFloodCC_Click(Nothing, Nothing)

                mpeCommTurnInMaint.ErrorMessage = CStr(selRowCount) & If(selRowCount = 1, " Record ", " Records ") & "Saved Successfully."
            Else
                mpeCommTurnInMaint.ErrorMessage = "Select at least one record to Flood."
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
    Private Sub btnResetFloodEMM_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnResetFloodEMM.Click
        rtxtFloodWebCategories.Text = ""
        hfFloodWebCatCde.Value = String.Empty
        rtxtFloodFrndPrdDesc.Text = ""
        rtxtFloodEMMNotes.Text = ""
        rtxtFloodFriendlyColor.Text = ""

        rcbFloodSizeCategory.Text = ""
        rcbFloodSizeCategory.ClearSelection()
    End Sub

    Private Sub btnResetFloodCC_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnResetFloodCC.Click
        rtxtFloodImageNotes.Text = ""
        rtxtFloodStylingNotes.Text = ""

        rcbFloodImgType.ClearSelection()
        rcbFloodImgType.Text = ""

        rcbFloodModelCategory.ClearSelection()
        rcbFloodModelCategory.Text = ""

        rcbFloodAlternateView.ClearSelection()
        rcbFloodAlternateView.Text = ""

        cmbFloodFeatureRenderSwatch.ClearSelection()
        cmbFloodFeatureRenderSwatch.Text = ""
    End Sub

#End Region

#Region "EMM"

    Private Sub grdEMMSubmit()
        Try
            Dim adnumber As String = _eCommTurnInMaintCtrl.SelectedAdNumber.Trim
            Dim pagenum As String = _eCommTurnInMaintCtrl.cmbPage.SelectedValue.Trim
            Dim buyer As String = _eCommTurnInMaintCtrl.SelectedBuyerId.Trim
            Dim DeptID As String = _eCommTurnInMaintCtrl.SelectedDeptId.Trim
            Dim LblID As String = _eCommTurnInMaintCtrl.SelectedLabelID.Trim
            Dim VndrStylID As String = _eCommTurnInMaintCtrl.SelectedVendorStyleID.Trim
            Dim BatchID As String = _eCommTurnInMaintCtrl.SelectedBatchNum
            Dim MeetingResults As List(Of ECommTurnInMeetCreateInfo) = _TUECommTurnInMeetResults.GetEcommTurninMeet(adnumber, pagenum, buyer, DeptID, LblID, VndrStylID, BatchID).ToList
            Dim invalidGroupIds As String = String.Empty

            If MeetingResults.Where(Function(x) x.ColorSequence = 1).Count > 0 Then 'If items have already been sorted by CC
                MeetingResults = MeetingResults.OrderBy(Function(x) x.ColorSequence).ToList
            End If

            invalidGroupIds = GetInvalidImageGroups(MeetingResults)

            If Not String.IsNullOrEmpty(invalidGroupIds) Then
                mpeCommTurnInMaint.PopUpMessage = String.Format("Image group number cannot be greater than 9. Please check and correct the following image group(s): \n{0}", invalidGroupIds)
                grdEMM.AllowPaging = True
                grdEMM.MasterTableView.Rebind()
                grdCC.MasterTableView.Rebind()
                Exit Sub
            End If


            Dim readyToSubmitFriendlyProdDesc As Boolean = True
            Dim readyToSubmitWebCat As Boolean = True
            Dim readyToSubmitModelCategory As Boolean = True
            Dim readyToSubmitImageKind As Boolean = True
            Dim readyToSubmitHasRequiredSample As Boolean = True

            Dim SubmitList As New List(Of ECommTurnInMeetCreateInfo)

            For Each item As ECommTurnInMeetCreateInfo In MeetingResults
                If item.RemoveMerchFlag = "N"c Then
                    If item.FriendlyProdDesc = "" Then
                        readyToSubmitFriendlyProdDesc = False
                    ElseIf item.CategoryCode = 0 Then
                        'TODO: Confirm this logic is correct
                        readyToSubmitWebCat = False
                    ElseIf item.OnOff = "ON" And item.ModelCategory = "" Then
                        readyToSubmitModelCategory = False
                    ElseIf item.ImageKindCode = "NOMER" And item.ImageGrp = "0" Then
                        readyToSubmitImageKind = False
                    ElseIf item.MerchID = 0 And CInt(item.RoutefromAd) = 0 And
                        (String.Compare(item.ImageKindCode, "NEW", True) = 0 Or
                         (String.Compare(item.ImageKindCode, "DUP", True) = 0 And String.Compare(item.AltView.Trim(), "Swatch Ref", True) = 0)) Then
                        readyToSubmitHasRequiredSample = False
                    Else
                        Dim ECommTunInMeetCreateInfo As ECommTurnInMeetCreateInfo = _TUECommTurnInMeetResults.GetEcommTurninMeetByMerchId(item.turnInMerchID)
                        SubmitList.Add(ECommTunInMeetCreateInfo)
                        ECommTunInMeetCreateInfo = Nothing
                    End If
                End If
            Next

            If readyToSubmitFriendlyProdDesc = False Then
                mpeCommTurnInMaint.PopUpMessage = "Friendly Product Description was not entered for all records."
                grdEMM.Columns.FindByUniqueName("FriendlyProdDesc").HeaderText = "* Friendly Product Desc"
                grdCC.Columns.FindByUniqueName("FriendlyProdDesc").HeaderText = "* Friendly Product Desc"
            ElseIf readyToSubmitWebCat = False Then
                mpeCommTurnInMaint.PopUpMessage = "Web Categories was not entered for all records."
                grdEMM.Columns.FindByUniqueName("WebCategories").HeaderText = "* Web Categories"
            ElseIf readyToSubmitModelCategory = False Then
                mpeCommTurnInMaint.PopUpMessage = "Model Category must be selected for all records that are on figure."
                grdCC.Columns.FindByUniqueName("ModelCategory").HeaderText = "* Model Category"
            ElseIf readyToSubmitImageKind = False Then
                mpeCommTurnInMaint.PopUpMessage = "Records of Image Kind Type ""No Merch"" must be associated with an Image Group."
            ElseIf readyToSubmitHasRequiredSample = False Then
                mpeCommTurnInMaint.PopUpMessage = "Samples have not been selected for all merchandise."
                grdCC.Columns.FindByUniqueName("Thumbnail").HeaderText = "* Thumbnail"
            Else
                _TUECommTurnInMeetResults.SubmitMeetingPage(SubmitList, SessionWrapper.UserID)
                mpeCommTurnInMaint.ErrorMessage = "Data Submitted Successfully."
            End If

            grdEMM.AllowPaging = True
            grdEMM.MasterTableView.Rebind()
            grdCC.MasterTableView.Rebind()
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdEMM_DataBound(sender As Object, e As System.EventArgs) Handles grdEMM.DataBound
        Try
            'Retain Hierarchy state.
            Dim indexes As String() = New String(Me.EMMExpandedStates.Keys.Count - 1) {}
            Me.EMMExpandedStates.Keys.CopyTo(indexes, 0)

            Dim arr As New ArrayList(indexes)
            'Sort, so that a parent item is expanded before any of its children.
            arr.Sort()

            For Each key As String In arr
                Dim value As Boolean = CBool(Me.EMMExpandedStates(key))
                If value Then
                    If grdEMM.MasterTableView.GetItems(GridItemType.GroupHeader)(Integer.Parse(key)) IsNot Nothing Then
                        grdEMM.MasterTableView.GetItems(GridItemType.GroupHeader)(Integer.Parse(key)).Expanded = False
                    End If
                End If
            Next
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdEMM_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdEMM.DeleteCommand
        Try
            If (TypeOf e.Item Is GridDataItem) Then
                Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim MerhcID As Integer = CInt(dataItem.GetDataKeyValue("turnInMerchID"))
                Dim RemoveMerchFlg As Char = CChar(dataItem.GetDataKeyValue("RemoveMerchFlag"))

                If dataItem.ItemIndex > -1 Then
                    'To Delete / Activate a Color Size Level record in the database - Set the Remove Flag to Y / N.
                    _TUECommTurnInMeetResults.DeleteEMM(MerhcID, SessionWrapper.UserID)

                    If (RemoveMerchFlg = "N"c) Then
                        mpeCommTurnInMaint.ErrorMessage = "Selected row flagged for removal."
                    Else
                        mpeCommTurnInMaint.ErrorMessage = "Selected row has been Activated."
                    End If
                End If
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdEMM_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdEMM.ItemCommand
        Try
            If e.CommandName = "Edit" Then
                Dim turnInMerchID As Integer = CInt(CType(e.Item, GridDataItem).GetDataKeyValue("turnInMerchID"))

                If _TUECommTurnInMeetResults.IsKilled(turnInMerchID) Then
                    'If row has been killed, rebind and show message
                    mpeCommTurnInMaint.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1015)
                    e.Canceled = True
                    grdEMM.Rebind()
                End If

            End If

            If e.CommandName = "Export" Then
                grdEMM.AllowPaging = False
                grdEMM.Rebind()
                For Each column As GridColumn In grdEMM.MasterTableView.Columns
                    For Each item As GridDataItem In grdEMM.MasterTableView.Items
                        item(column.UniqueName).Text = item(column.UniqueName).Text.Replace(".", "&#46;")
                    Next
                Next
            End If

            'save the expanded state in the session
            If e.CommandName = RadGrid.ExpandCollapseCommandName Then
                'Is the item about to be expanded or collapsed
                If Not e.Item.Expanded Then
                    Me.EMMExpandedStates.Remove(e.Item.GroupIndex)
                Else
                    'Collapsed - Save its unique index among all the items in the hierarchy
                    Me.EMMExpandedStates(e.Item.GroupIndex) = True
                End If
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdEMM_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdEMM.NeedDataSource
        Try
            If CBool(Session("EditAll")) = True Then
                SaveRows(grdEMM, True)
            End If

            If e.RebindReason = GridRebindReason.InitialLoad Then Exit Sub
            Dim adnumber As String = _eCommTurnInMaintCtrl.SelectedAdNumber.Trim
            Dim pagenum As String = _eCommTurnInMaintCtrl.cmbPage.SelectedValue.Trim
            Dim buyer As String = _eCommTurnInMaintCtrl.SelectedBuyerId.Trim
            Dim DeptID As String = _eCommTurnInMaintCtrl.SelectedDeptId.Trim
            Dim LblID As String = _eCommTurnInMaintCtrl.SelectedLabelID.Trim
            Dim VndrStylID As String = _eCommTurnInMaintCtrl.SelectedVendorStyleID.Trim
            Dim BatchID As String = _eCommTurnInMaintCtrl.SelectedBatchNum

            Dim Items As List(Of ECommTurnInMeetCreateInfo) = _TUECommTurnInMeetResults.GetEcommTurninMeet(adnumber, pagenum, buyer, DeptID, LblID, VndrStylID, BatchID).ToList()

            '*********************Added to mimic cc grid KL 06/29/2015: ******************************
            If Items.Where(Function(x) x.ColorSequence = 1).Count > 0 Then 'If items have already been sorted by CC
                Items = Items.OrderBy(Function(x) x.ColorSequence).ToList
                grdEMM.GroupingEnabled = False
                grdEMM.MasterTableView.GroupByExpressions.Clear()
            Else
                grdEMM.GroupingEnabled = True
            End If

            '****************************End Add********************************************************

            grdEMM.DataSource = Items


            '*********************Added to mimic cc grid KL 06/29/2015: ******************************

            Dim rlbColorItems As RadListBox = CType(tuModalOrderColors.FindControl("rlbColorItems"), RadListBox)
            With rlbColorItems
                .DataSource = Items.Select(Function(x) New With {Key .turnInMerchID = x.turnInMerchID, .ColorSizeText = x.VendorStyleNumber & " - " & Truncate(x.FriendlyProdDesc, 50) & " - " & x.FriendlyColor & " - " & x.FeatureSwatch, x.Sequence, x.ColorSequence}).OrderBy(Function(x) x.Sequence).OrderBy(Function(x) x.ColorSequence).ToList
                .DataTextField = "ColorSizeText"
                .DataValueField = "turnInMerchID"
                .DataBind()
            End With

            '****************************End Add********************************************************

            grdEMM.Visible = True

            tblFloodEMM.Visible = True


            '*********************Added to mimic cc grid KL 06/29/2015: ******************************

            If rcbFloodImgType.Items.Count = 0 Then
                With rcbFloodImgType
                    .DataSource = _TUTMS900PARAMETER.GetAllImageTypeValues
                    .DataValueField = "CharIndex"
                    .DataTextField = "ShortDesc"
                    .DataBind()
                    .Items.Insert(0, New RadComboBoxItem(""))
                End With

                With rcbFloodModelCategory
                    .DataSource = _TUTMS900PARAMETER.GetAllModelCategories
                    .DataValueField = "CharIndex"
                    .DataTextField = "ShortDesc"
                    .DataBind()
                    .Items.Insert(0, New RadComboBoxItem(""))
                End With

                With rcbFloodAlternateView
                    .DataSource = _TUTMS900PARAMETER.GetAllAltViewValues
                    .DataValueField = "CharIndex"
                    .DataTextField = "ShortDesc"
                    .DataBind()
                    .Items.Insert(0, New RadComboBoxItem(""))
                End With

                With cmbFloodFeatureRenderSwatch
                    .Items.Clear()
                    .DataSource = _TUTMS900PARAMETER.GetAllFeatureRenderSwatchValues
                    .DataValueField = "CharIndex"
                    .DataTextField = "ShortDesc"
                    .DataBind()
                    .Items.Insert(0, New RadComboBoxItem(""))
                End With
            End If


            '****************************End Add********************************************************


            With rcbFloodSizeCategory
                .DataSource = _TUTMS900PARAMETER.GetAllSizeCategories
                .DataValueField = "CharIndex"
                .DataTextField = "ShortDesc"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With


        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdEMM_SortCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridSortCommandEventArgs) Handles grdEMM.SortCommand
        Try
            grdEMM.Rebind()
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Function GetWebCatToolTip(ByVal WebCatgyList As String) As String
        Dim returnVal As String = ""
        Dim OtherCategoryCodes As List(Of String) = WebCatgyList.Split(CChar(",")).ToList
        OtherCategoryCodes.Remove("0")
        If OtherCategoryCodes.Count > 0 Then
            For Each wc As String In OtherCategoryCodes
                Dim cde As Integer = CInt(wc)

                If AllWebCats.Where(Function(x) x.CategoryCode = cde).ToList.Count = 0 Then
                    Dim objGetApplicationObjectsService As New GetGlobalObjectsService
                    objGetApplicationObjectsService.GetAllApplicationObjects()  'this will create/populate all application objects.
                End If
                returnVal &= Server.HtmlDecode(AllWebCats.Where(Function(x) x.CategoryCode = cde).FirstOrDefault.CategoryLongDesc) & vbCrLf
            Next
        Else
            returnVal = "No additional Web Categories exist."
        End If
        Return returnVal
    End Function

    Private Function SetDefaultCategory(ByVal DefaultCategoryCode As Integer) As String
        If DefaultCategoryCode = 0 Then
            Return ""
            Exit Function
        End If
        Dim returnVal As String = ""
        If AllWebCats.Where(Function(x) x.CategoryCode = DefaultCategoryCode).ToList.Count = 0 Then
            Dim objGetApplicationObjectsService As New GetGlobalObjectsService
            objGetApplicationObjectsService.GetAllApplicationObjects()  'this will create/populate all application objects.
        End If
        returnVal = AllWebCats.Where(Function(x) x.CategoryCode = DefaultCategoryCode).FirstOrDefault.CategoryLongDesc
        Return returnVal
    End Function

    Private Sub grdEMM_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdEMM.ItemDataBound
        Try

            If (TypeOf e.Item Is GridDataItem And Not e.Item.IsInEditMode) Then
                Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim RemoveMerchFlg As Char = CChar(dataItem.GetDataKeyValue("RemoveMerchFlag"))

                If dataItem.ItemIndex > -1 Then
                    If Session("eCommTurnInMeet.grdEmm.LastUpdatedRowId") IsNot Nothing Then
                        If CStr(Session("eCommTurnInMeet.grdEmm.LastUpdatedRowId")).Split(CChar(",")).Contains(dataItem.GetDataKeyValue("turnInMerchID").ToString) Then
                            dataItem.BackColor = Drawing.Color.Yellow
                        End If
                    End If

                    'CType(dataItem("WebCategories").FindControl("ltrPrimaryWebCategory"), Literal).Text = SetDefaultCategory(CInt(dataItem.GetDataKeyValue("CategoryCode")))

                    ''Set Tooltip for Web Categories field: List of Secondary Web Categories.
                    'dataItem("WebCategories").ToolTip = GetWebCatToolTip(dataItem.GetDataKeyValue("WebCatgyList").ToString)

                    For i As Integer = 0 To dataItem.Controls.Count - 1
                        CType(dataItem.Controls(i), GridTableCell).Enabled = If(RemoveMerchFlg = "Y"c, False, True)
                    Next

                    If (RemoveMerchFlg = "Y"c) Then
                        dataItem.Font.Strikeout = True

                        CType(dataItem("DeleteColumn"), GridTableCell).Enabled = True
                        CType(dataItem("DeleteColumn").Controls(0), ImageButton).ImageUrl = "~/Images/CheckMark.gif"
                        CType(dataItem("DeleteColumn").Controls(0), ImageButton).ToolTip = "Activate"
                    End If
                End If

                CType(dataItem("WebCategories").FindControl("ltrPrimaryWebCategory"), Literal).Text = SetDefaultCategory(CInt(dataItem.GetDataKeyValue("CategoryCode")))
                If CType(dataItem("WebCategories").FindControl("ltrPrimaryWebCategory"), Literal).Text Is Nothing Then
                    CType(dataItem("WebCategories").FindControl("ltrPrimaryWebCategory"), Literal).Text = ""
                End If
                'Set Tooltip for Web Categories field: List of Secondary Web Categories.
                dataItem("WebCategories").ToolTip = GetWebCatToolTip(dataItem.GetDataKeyValue("WebCatgyList").ToString())
            End If


            'Handle EDIT command - Populate the values for all the columns with Comboboxes.
            If (TypeOf e.Item Is GridEditableItem And e.Item.IsInEditMode) Then
                Dim editItem As GridEditableItem = DirectCast(e.Item, GridEditableItem)

                CType(editItem("WebCategories").FindControl("rtxtWebCategories"), RadTextBox).Text = SetDefaultCategory(CInt(editItem.GetDataKeyValue("CategoryCode")))
                'Set Tooltip for Web Categories field: List of Secondary Web Categories.
                editItem("WebCategories").ToolTip = GetWebCatToolTip(editItem.GetDataKeyValue("WebCatgyList").ToString)

                Dim ISN As Decimal = CDec(editItem.GetDataKeyValue("ISN"))

                Dim cmbSizeCategory As RadComboBox = DirectCast(editItem.FindControl("rcbSizeCategory"), RadComboBox)
                Dim hfSizeCategory As New HiddenField
                hfSizeCategory = DirectCast(editItem.FindControl("hfSizeCategory"), HiddenField)
                With cmbSizeCategory
                    .DataSource = _TUTMS900PARAMETER.GetAllSizeCategories
                    .DataValueField = "CharIndex"
                    .DataTextField = "ShortDesc"
                    .DataBind()
                    .Items.Insert(0, New RadComboBoxItem(""))
                    .FindItemByText(hfSizeCategory.Value.Trim).Selected = True
                End With

                Dim cmbLabel As RadComboBox = DirectCast(editItem.FindControl("rcbLabel"), RadComboBox)
                With cmbLabel
                    .DataSource = _TULabel.GetLabelsByBrand(CStr(editItem.GetDataKeyValue("BrandId")))
                    .DataValueField = "LabelId"
                    .DataTextField = "LabelDesc"
                    .DataBind()
                End With
                Try
                    cmbLabel.FindItemByValue(editItem.GetDataKeyValue("LabelId").ToString.Trim).Selected = True
                Catch ex As Exception
                End Try

            End If
        Catch ex As Exception
            Session("Adinfo") = Nothing
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdEMM_UpdateCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdEMM.UpdateCommand
        Try

            Page.Validate("Update")

            If Not IsValid Then
                mpeCommTurnInMaint.ErrorMessage = "Errors on Page."
                e.Canceled = True
                Exit Sub
            End If

            If (TypeOf e.Item Is GridEditableItem And e.Item.IsInEditMode) Then
                Dim editItem As GridEditableItem = DirectCast(e.Item, GridEditableItem)
                Dim merchandiseInfo As ECommTurnInMeetCreateInfo = New ECommTurnInMeetCreateInfo With
                {
                    .turnInMerchID = CInt(editItem.GetDataKeyValue("turnInMerchID")),
                    .EMMFollowUpFlag = If(DirectCast(editItem.FindControl("cbxFollowUpFlag"), CheckBox).Checked, "Y", "N"),
                    .FriendlyProdDesc = Server.HtmlDecode(DirectCast(editItem.FindControl("rtxtFriendlyProdDesc"), RadTextBox).Text),
                    .FriendlyColor = DirectCast(editItem.FindControl("rtxtFriendlyColor"), RadTextBox).Text,
                    .LabelID = CInt(DirectCast(editItem.FindControl("rcbLabel"), RadComboBox).SelectedValue),
                    .SizeCategory = DirectCast(editItem.FindControl("rcbSizeCategory"), RadComboBox).SelectedValue,
                    .EMMNotes = Server.HtmlDecode(DirectCast(editItem.FindControl("rtxtEMMNotes"), RadTextBox).Text),
                    .ImageKindCode = DirectCast(editItem("ImageKind").Controls(1), System.Web.UI.WebControls.Literal).Text,
                    .MerchID = CInt(editItem.GetDataKeyValue("MerchID"))}

                _TUECommTurnInMeetResults.UpdateEMMInfo(merchandiseInfo, SessionWrapper.UserID)
                Session("eCommTurnInMeet.grdEmm.LastUpdatedRowId") = merchandiseInfo.turnInMerchID

                mpeCommTurnInMaint.ErrorMessage = "Data Saved Successfully."
                grdCC.Rebind()

                grdEMM.Columns.FindByUniqueName("FriendlyProdDesc").HeaderText = "Friendly Product Desc"
                grdEMM.Columns.FindByUniqueName("WebCategories").HeaderText = "Web Categories"

            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdEMM_ItemCreated(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grdEMM.ItemCreated
        If TypeOf e.Item Is GridEditableItem AndAlso e.Item.IsInEditMode Then
            Dim item As GridEditableItem = TryCast(e.Item, GridEditableItem)

            Dim rtxtWebCategories As RadTextBox = CType(item.FindControl("rtxtWebCategories"), RadTextBox)
            rtxtWebCategories.Attributes("OnClick") = "javascript:window.radopen('WebCategories.aspx?ID=" + item.GetDataKeyValue("ISN").ToString() + "', 'UserDialog');"

        End If
    End Sub

#End Region

#Region "Creative Coordinator"

    Private Sub grdCC_ItemCreated(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grdCC.ItemCreated
        If TypeOf e.Item Is GridEditableItem AndAlso e.Item.IsInEditMode Then
            Dim item As GridEditableItem = CType(e.Item, GridEditableItem)

            Dim rcbOnOff As RadComboBox = CType(item.FindControl("rcbOnOff"), RadComboBox)
            rcbOnOff.OnClientSelectedIndexChanged = "function(sender,args) {OnChangeOnOff(sender,args,'" + grdCC.ClientID + "','" + item.ItemIndex.ToString + "')}"

            Dim rcbImageKind As RadComboBox = CType(item.FindControl("rcbImageKind"), RadComboBox)
            rcbImageKind.OnClientSelectedIndexChanged = "function(sender,args) {OnChangeImageKind(sender,args,'" + grdCC.ClientID + "','" + item.ItemIndex.ToString + "')}"
            If TryCast((TryCast(sender, RadGrid)).Columns(21), GridTemplateColumn).UniqueName = "ModelCategory" Then
                Dim valcol As GridTemplateColumn = TryCast((TryCast(sender, RadGrid)).Columns(21), GridTemplateColumn)
                Dim ctl As ToolTipValidator = DirectCast(valcol.ColumnEditor.ContainerControl.Controls.Item(5), ToolTipValidator)
                If ctl.ID = "valModelCategory" Then
                    ctl.IsValid = True
                End If
            End If
            Dim rcbFeatureRenderSwatch As RadComboBox = CType(item.FindControl("rcbFeatureRenderSwatch"), RadComboBox)
            rcbFeatureRenderSwatch.OnClientSelectedIndexChanged = "function(sender,args) {OnChangeFRS(sender,args,'" + grdCC.ClientID + "','" + item.ItemIndex.ToString + "')}"

        End If
    End Sub

    Private Sub grdCC_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdCC.NeedDataSource
        Try
            If e.RebindReason = GridRebindReason.InitialLoad Then Exit Sub
            Dim adnumber As String = _eCommTurnInMaintCtrl.SelectedAdNumber.Trim
            Dim pagenum As String = _eCommTurnInMaintCtrl.cmbPage.SelectedValue.Trim
            Dim buyer As String = _eCommTurnInMaintCtrl.SelectedBuyerId.Trim
            Dim DeptID As String = _eCommTurnInMaintCtrl.SelectedDeptId.Trim
            Dim LblID As String = _eCommTurnInMaintCtrl.SelectedLabelID.Trim
            Dim VndrStylID As String = _eCommTurnInMaintCtrl.SelectedVendorStyleID.Trim
            Dim BatchID As String = _eCommTurnInMaintCtrl.SelectedBatchNum
            Dim Items As List(Of ECommTurnInMeetCreateInfo) = _TUECommTurnInMeetResults.GetEcommTurninMeet(adnumber, pagenum, buyer, DeptID, LblID, VndrStylID, BatchID).ToList

            If Items.Where(Function(x) x.ColorSequence = 1).Count > 0 Then 'If items have already been sorted by CC
                Items = Items.OrderBy(Function(x) x.ColorSequence).ToList
                grdCC.GroupingEnabled = False
                grdCC.MasterTableView.GroupByExpressions.Clear()
            Else
                grdCC.GroupingEnabled = True
            End If

            grdCC.DataSource = Items

            Dim rlbColorItems As RadListBox = CType(tuModalOrderColors.FindControl("rlbColorItems"), RadListBox)
            With rlbColorItems
                .DataSource = Items.Select(Function(x) New With {Key .turnInMerchID = x.turnInMerchID, .ColorSizeText = x.VendorStyleNumber & " - " & Truncate(x.FriendlyProdDesc, 50) & " - " & x.FriendlyColor & " - " & x.FeatureSwatch, x.Sequence, x.ColorSequence}).OrderBy(Function(x) x.Sequence).OrderBy(Function(x) x.ColorSequence).ToList
                .DataTextField = "ColorSizeText"
                .DataValueField = "turnInMerchID"
                .DataBind()
            End With

            grdCC.Visible = True
            tblFloodCC.Visible = True

            If rcbFloodImgType.Items.Count = 0 Then
                With rcbFloodImgType
                    .DataSource = _TUTMS900PARAMETER.GetAllImageTypeValues
                    .DataValueField = "CharIndex"
                    .DataTextField = "ShortDesc"
                    .DataBind()
                    .Items.Insert(0, New RadComboBoxItem(""))
                End With

                With rcbFloodModelCategory
                    .DataSource = _TUTMS900PARAMETER.GetAllModelCategories
                    .DataValueField = "CharIndex"
                    .DataTextField = "ShortDesc"
                    .DataBind()
                    .Items.Insert(0, New RadComboBoxItem(""))
                End With

                With rcbFloodAlternateView
                    .DataSource = _TUTMS900PARAMETER.GetAllAltViewValues
                    .DataValueField = "CharIndex"
                    .DataTextField = "ShortDesc"
                    .DataBind()
                    .Items.Insert(0, New RadComboBoxItem(""))
                End With

                With cmbFloodFeatureRenderSwatch
                    .Items.Clear()
                    .DataSource = _TUTMS900PARAMETER.GetAllFeatureRenderSwatchValues
                    .DataValueField = "CharIndex"
                    .DataTextField = "ShortDesc"
                    .DataBind()
                    .Items.Insert(0, New RadComboBoxItem(""))
                End With
            End If

        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    'Private Sub grdCC_DataBound(sender As Object, e As System.EventArgs) Handles grdCC.DataBound
    '    'Retain Hierarchy state.
    '    Dim indexes As String() = New String(Me.CCExpandedStates.Keys.Count - 1) {}
    '    Me.CCExpandedStates.Keys.CopyTo(indexes, 0)

    '    Dim arr As New ArrayList(indexes)
    '    'Sort, so that a parent item is expanded before any of its children.
    '    arr.Sort()

    '    For Each key As String In arr
    '        Dim value As Boolean = CBool(Me.CCExpandedStates(key))
    '        If value Then
    '            If grdCC.MasterTableView.GetItems(GridItemType.GroupHeader)(Integer.Parse(key)) IsNot Nothing Then
    '                grdCC.MasterTableView.GetItems(GridItemType.GroupHeader)(Integer.Parse(key)).Expanded = False
    '            End If
    '        End If
    '    Next
    'End Sub

    Private Sub grdCC_SortCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridSortCommandEventArgs) Handles grdCC.SortCommand
        Try
            grdCC.Rebind()
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdCC_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdCC.DeleteCommand
        Try
            If (TypeOf e.Item Is GridDataItem) Then
                Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim MerchID As Integer = CInt(dataItem.GetDataKeyValue("turnInMerchID"))
                Dim RemoveMerchFlg As Char = CChar(dataItem.GetDataKeyValue("RemoveMerchFlag"))

                If dataItem.ItemIndex > -1 Then
                    'To Delete / Activate a Color Size Level record in the database - Set the Remove Flag to Y / N.
                    _TUECommTurnInMeetResults.DeleteMediaCoord(MerchID, SessionWrapper.UserID)

                    If (RemoveMerchFlg = "N"c) Then
                        mpeCommTurnInMaint.ErrorMessage = "Selected row flagged for removal."
                    Else
                        mpeCommTurnInMaint.ErrorMessage = "Selected row has been Activated."
                    End If
                End If
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdCC_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdCC.ItemDataBound
        Try
            'Disable the rows that are flagged for Removal.
            If (TypeOf e.Item Is GridDataItem) Then
                Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim RemoveMerchFlg As Char = CChar(dataItem.GetDataKeyValue("RemoveMerchFlag"))

                If dataItem.ItemIndex > -1 Then
                    If Session("eCommTurnInMeet.grdCC.LastUpdatedRowId") IsNot Nothing Then
                        If CStr(Session("eCommTurnInMeet.grdCC.LastUpdatedRowId")).Split(CChar(",")).Contains(dataItem.GetDataKeyValue("turnInMerchID").ToString) Then
                            dataItem.BackColor = Drawing.Color.Yellow
                        End If
                    End If

                    For i As Integer = 0 To dataItem.Controls.Count - 1
                        CType(dataItem.Controls(i), GridTableCell).Enabled = If(RemoveMerchFlg = "Y"c, False, True)
                        If Trim(DirectCast(dataItem.Item("Thumbnail").FindControl("ibThumbnail"), ImageButton).AlternateText.ToString()) = "Sample not selected" Then
                            DirectCast(dataItem.Item("Thumbnail").FindControl("ibThumbnail"), ImageButton).BackColor = Drawing.ColorTranslator.FromHtml("#FFC0CB")
                            DirectCast(dataItem.Item("Thumbnail").FindControl("ibThumbnail"), ImageButton).AlternateText = ""
                            DirectCast(dataItem.Item("Thumbnail").FindControl("ibThumbnail"), ImageButton).ToolTip = "Sample not selected"

                        End If
                    Next

                    If (RemoveMerchFlg = "Y"c) Then
                        dataItem.Font.Strikeout = True

                        CType(dataItem("DeleteColumn"), GridTableCell).Enabled = True
                        CType(dataItem("DeleteColumn").Controls(0), ImageButton).ImageUrl = "~/Images/CheckMark.gif"
                        CType(dataItem("DeleteColumn").Controls(0), ImageButton).ToolTip = "Activate"
                    End If
                End If

                CType(dataItem("WebCategories").FindControl("ltrPrimaryWebCategory"), Literal).Text = SetDefaultCategory(CInt(dataItem.GetDataKeyValue("CategoryCode")))

                'Set Tooltip for Web Categories field: List of Secondary Web Categories.
                dataItem("WebCategories").ToolTip = GetWebCatToolTip(dataItem.GetDataKeyValue("WebCatgyList").ToString)
            End If
            'edit
            If (TypeOf e.Item Is GridEditableItem And e.Item.IsInEditMode) Then
                Dim editItem As GridEditableItem = DirectCast(e.Item, GridEditableItem)
                Dim ISN As Decimal = CDec(editItem.GetDataKeyValue("ISN"))
                Dim cmbModelCategory As RadComboBox = DirectCast(editItem.FindControl("rcbModelCategory"), RadComboBox)
                Dim hfModelCategory As HiddenField = DirectCast(editItem.FindControl("hfModelCategory"), HiddenField)

                With cmbModelCategory
                    .DataSource = _TUTMS900PARAMETER.GetAllModelCategories
                    .DataValueField = "CharIndex"
                    .DataTextField = "ShortDesc"
                    .DataBind()
                    .Items.Insert(0, New RadComboBoxItem(""))
                    .FindItemByText(hfModelCategory.Value.Trim).Selected = True
                End With

                Dim cmbImageKind As RadComboBox = DirectCast(editItem.FindControl("rcbImageKind"), RadComboBox)
                Dim hfImageKind As New HiddenField
                hfImageKind = DirectCast(editItem.FindControl("hfImageKind"), HiddenField)
                With cmbImageKind
                    .DataSource = _TUTMS900PARAMETER.GetAllImageKindValues
                    .DataValueField = "CharIndex"
                    .DataTextField = "ShortDesc"
                    .DataBind()
                    .FindItemByValue(If(String.IsNullOrEmpty(hfImageKind.Value.Trim), "New", hfImageKind.Value.Trim)).Selected = True

                End With

                Dim cmbOnOff As RadComboBox = DirectCast(editItem.FindControl("rcbOnOff"), RadComboBox)
                Dim hfOnOff As HiddenField = DirectCast(editItem.FindControl("hfOnOff"), HiddenField)

                With cmbOnOff
                    .DataSource = _TUTMS900PARAMETER.GetAllMDSEFigureCodes()
                    .DataValueField = "CharIndex"
                    .DataTextField = "ShortDesc"
                    .DataBind()
                    .Items.Insert(0, New RadComboBoxItem(""))
                    .FindItemByText(hfOnOff.Value.Trim).Selected = True
                End With

                'AlternateView
                Dim cmbAlternateView As RadComboBox = DirectCast(editItem.FindControl("rcbAlternateView"), RadComboBox)
                Dim hfAlternateView As HiddenField = DirectCast(editItem.FindControl("hfAlternateView"), HiddenField)

                With cmbAlternateView
                    .DataSource = _TUTMS900PARAMETER.GetAllAltViewValues
                    .DataValueField = "CharIndex"
                    .DataTextField = "ShortDesc"
                    .DataBind()
                    .Items.Insert(0, New RadComboBoxItem(""))
                    .FindItemByText(hfAlternateView.Value.Trim).Selected = True
                End With

                Dim cmbFeatureRenderSwatch As RadComboBox = DirectCast(editItem.FindControl("rcbFeatureRenderSwatch"), RadComboBox)
                Dim hfFeatureRenderSwatch As HiddenField = DirectCast(editItem.FindControl("hfFeatureRenderSwatch"), HiddenField)
                With cmbFeatureRenderSwatch
                    .DataSource = _TUTMS900PARAMETER.GetAllFeatureRenderSwatchValues
                    .DataValueField = "CharIndex"
                    .DataTextField = "ShortDesc"
                    .DataBind()
                    .Items.Insert(0, New RadComboBoxItem(""))
                    .FindItemByText(hfFeatureRenderSwatch.Value.Trim).Selected = True
                End With



            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdCC_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdCC.ItemCommand
        Try
            If e.CommandName = "Edit" Then
                Dim turnInMerchID As Integer = CInt(CType(e.Item, GridDataItem).GetDataKeyValue("turnInMerchID"))

                If _TUECommTurnInMeetResults.IsKilled(turnInMerchID) Then
                    'If row has been killed, rebind and show message
                    mpeCommTurnInMaint.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1015)
                    e.Canceled = True
                    grdCC.Rebind()
                End If
            End If

            'save the expanded state in the session
            If e.CommandName = RadGrid.ExpandCollapseCommandName Then
                'Is the item about to be expanded or collapsed
                If Not e.Item.Expanded Then
                    Me.CCExpandedStates.Remove(e.Item.GroupIndex)
                Else
                    'Collapsed - Save its unique index among all the items in the hierarchy
                    Me.CCExpandedStates(e.Item.GroupIndex) = True
                End If
            End If



        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdCC_UpdateCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdCC.UpdateCommand
        Try
            Page.Validate("Update")

            If Not IsValid Then
                mpeCommTurnInMaint.ErrorMessage = "Errors on Page."
                e.Canceled = True
                Exit Sub
            End If

            If (TypeOf e.Item Is GridEditableItem And e.Item.IsInEditMode) Then
                Dim editItem As GridEditableItem = DirectCast(e.Item, GridEditableItem)
                With editItem
                    If editItem.GetDataKeyValue("ISN").ToString() = "" Or editItem.GetDataKeyValue("ISN").ToString() = "0" Then

                    End If
                End With
                Dim merchandiseInfo As ECommTurnInMeetCreateInfo = New ECommTurnInMeetCreateInfo With
                {
                    .turnInMerchID = CInt(editItem.GetDataKeyValue("turnInMerchID")),
                    .ISN = CStr(editItem.GetDataKeyValue("ISN")),
                    .ImageGrp = DirectCast(editItem.FindControl("rtxtImageGrp"), RadNumericTextBox).Text,
                    .ImageDesc = DirectCast(editItem.FindControl("rtxtImageDesc"), RadTextBox).Text,
                    .ImageNotes = DirectCast(editItem.FindControl("rtxtImageNotes"), RadTextBox).Text,
                    .OnOff = DirectCast(editItem.FindControl("rcbOnOff"), RadComboBox).SelectedValue,
                    .ModelCategory = DirectCast(editItem.FindControl("rcbModelCategory"), RadComboBox).SelectedValue,
                    .AltView = DirectCast(editItem.FindControl("rcbAlternateView"), RadComboBox).SelectedValue,
                    .FeatureSwatch = DirectCast(editItem.FindControl("rcbFeatureRenderSwatch"), RadComboBox).SelectedValue,
                    .PickupImageID = DirectCast(editItem.FindControl("rtxtPickupImageID"), RadNumericTextBox).Text,
                    .ColorCorrect = CChar(DirectCast(editItem.FindControl("rcbColorCorrect"), RadComboBox).Text),
                    .HotListCDE = CChar(DirectCast(editItem.FindControl("rcbHotItem"), RadComboBox).SelectedValue),
                    .StylingNotes = DirectCast(editItem.FindControl("rtxtStylingNotes"), RadTextBox).Text,
                    .CCFollowUpFlag = If(DirectCast(editItem.FindControl("cbxFollowUpFlag"), CheckBox).Checked, "Y", "N"),
                    .ImageKindCode = DirectCast(editItem.FindControl("rcbImageKind"), RadComboBox).SelectedValue,
                    .RoutefromAd = CStr(editItem.GetDataKeyValue("RoutefromAd")),
                     .MerchID = CInt(DirectCast(editItem.FindControl("hfMerchId"), RadTextBox).Text),
                    .FriendlyColor = Trim(CStr(DirectCast(editItem.FindControl("radtxtFriendlyColor"), RadTextBox).Text)),
                     .ColorCode = CInt(editItem.GetDataKeyValue("ColorCode")),
                    .SampleSize = CStr(editItem.GetDataKeyValue("SampleSize")),
                    .PrimaryThumbnailUrl = CStr(editItem.GetDataKeyValue("PrimaryThumbnailUrl"))
                }
                '.ColorCode = CInt(DirectCast(editItem.FindControl("hfColorCode"), RadNumericTextBox).Text),
                If ValidateSampleRequired(merchandiseInfo) = True Then
                    _TUECommTurnInMeetResults.UpdateCCInfo(merchandiseInfo, SessionWrapper.UserID)
                    Session("eCommTurnInMeet.grdCC.LastUpdatedRowId") = merchandiseInfo.turnInMerchID
                    mpeCommTurnInMaint.ErrorMessage = "Data Saved Successfully."
                Else

                End If
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

#End Region

#Region "CopyWriter"

    Private Sub grdCopyWriter_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdCopyWriter.NeedDataSource
        Try
            If e.RebindReason = GridRebindReason.InitialLoad Then Exit Sub
            Dim adnumber As String = _eCommTurnInMaintCtrl.SelectedAdNumber.Trim
            Dim pagenum As String = _eCommTurnInMaintCtrl.cmbPage.SelectedValue.Trim
            Dim buyer As String = _eCommTurnInMaintCtrl.SelectedBuyerId.Trim
            Dim DeptID As String = _eCommTurnInMaintCtrl.SelectedDeptId.Trim
            Dim LblID As String = _eCommTurnInMaintCtrl.SelectedLabelID.Trim
            Dim VndrStylID As String = _eCommTurnInMaintCtrl.SelectedVendorStyleID.Trim
            Dim BatchID As String = _eCommTurnInMaintCtrl.SelectedBatchNum

            Dim Items As List(Of ECommTurnInMeetCreateInfo) = _TUECommTurnInMeetResults.GetEcommTurninMeet(adnumber, pagenum, buyer, DeptID, LblID, VndrStylID, BatchID).ToList
            grdCopyWriter.DataSource = Items
            grdCopyWriter.Visible = True

        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdCopyWriter_DataBound(sender As Object, e As System.EventArgs) Handles grdCopyWriter.DataBound
        'Retain Hierarchy state.
        Dim indexes As String() = New String(Me.CopyWriterExpandedStates.Keys.Count - 1) {}
        Me.CopyWriterExpandedStates.Keys.CopyTo(indexes, 0)

        Dim arr As New ArrayList(indexes)
        'Sort, so that a parent item is expanded before any of its children.
        arr.Sort()

        For Each key As String In arr
            Dim value As Boolean = CBool(Me.CopyWriterExpandedStates(key))
            If value Then
                If grdCopyWriter.MasterTableView.GetItems(GridItemType.GroupHeader)(Integer.Parse(key)) IsNot Nothing Then
                    grdCopyWriter.MasterTableView.GetItems(GridItemType.GroupHeader)(Integer.Parse(key)).Expanded = False
                End If
            End If
        Next
    End Sub

    Private Sub grdCopyWriter_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdCopyWriter.DeleteCommand
        Try
            If (TypeOf e.Item Is GridDataItem) Then
                Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim MerhcID As Integer = CInt(dataItem.GetDataKeyValue("turnInMerchID"))
                Dim RemoveMerchFlg As Char = CChar(dataItem.GetDataKeyValue("RemoveMerchFlag"))

                If dataItem.ItemIndex > -1 Then
                    'To Delete / Activate a Color Size Level record in the database - Set the Remove Flag to Y / N.
                    _TUECommTurnInMeetResults.DeleteMerchCoord(MerhcID, SessionWrapper.UserID)

                    If (RemoveMerchFlg = "N"c) Then
                        mpeCommTurnInMaint.ErrorMessage = "Selected row flagged for removal."
                    Else
                        mpeCommTurnInMaint.ErrorMessage = "Selected row has been Activated."
                    End If
                End If
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdCopyWriter_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdCopyWriter.ItemDataBound
        Try
            'Disable the rows that are flagged for Removal.
            If (TypeOf e.Item Is GridDataItem) Then
                Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim RemoveMerchFlg As Char = CChar(dataItem.GetDataKeyValue("RemoveMerchFlag"))

                If dataItem.ItemIndex > -1 Then
                    If Session("eCommTurnInMeet.grdCopyWriter.LastUpdatedRowId") IsNot Nothing Then
                        If CStr(Session("eCommTurnInMeet.grdCopyWriter.LastUpdatedRowId")).Split(CChar(",")).Contains(dataItem.GetDataKeyValue("turnInMerchID").ToString) Then
                            dataItem.BackColor = Drawing.Color.Yellow
                        End If
                    End If

                    For i As Integer = 0 To dataItem.Controls.Count - 1
                        CType(dataItem.Controls(i), GridTableCell).Enabled = If(RemoveMerchFlg = "Y"c, False, True)
                    Next

                    If (RemoveMerchFlg = "Y"c) Then
                        dataItem.Font.Strikeout = True

                        CType(dataItem("DeleteColumn"), GridTableCell).Enabled = True
                        CType(dataItem("DeleteColumn").Controls(0), ImageButton).ImageUrl = "~/Images/CheckMark.gif"
                        CType(dataItem("DeleteColumn").Controls(0), ImageButton).ToolTip = "Activate"
                    End If
                End If
            End If
            'edit

            'If (TypeOf e.Item Is GridEditableItem And e.Item.IsInEditMode) Then
            '    Dim editItem As GridEditableItem = DirectCast(e.Item, GridEditableItem)
            '    Dim ISN As Decimal = CDec(editItem.GetDataKeyValue("ISN"))
            'End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdCopyWriter_UpdateCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdCopyWriter.UpdateCommand
        Try
            Page.Validate("Update")

            If Not IsValid Then
                mpeCommTurnInMaint.ErrorMessage = "Errors on Page."
                e.Canceled = True
                Exit Sub
            End If

            If (TypeOf e.Item Is GridEditableItem And e.Item.IsInEditMode) Then
                Dim editItem As GridEditableItem = DirectCast(e.Item, GridEditableItem)
                Dim ECommTunInMeetCcInfo As New ECommTurnInMeetCreateInfo()
                Dim turnInMerchID As Integer = CInt(editItem.GetDataKeyValue("turnInMerchID"))

                _TUECommTurnInMeetResults.UpdateCWInfo(turnInMerchID, DirectCast(editItem.FindControl("rtxtCpyNotes"), RadTextBox).Text, If(DirectCast(editItem.FindControl("cbxFollowUpFlag"), CheckBox).Checked, "Y", "N"), SessionWrapper.UserID)
                Session("eCommTurnInMeet.grdCopyWriter.LastUpdatedRowId") = turnInMerchID

                mpeCommTurnInMaint.ErrorMessage = "Data Saved Successfully."
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdCopyWriter_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdCopyWriter.ItemCommand
        Session("eCommTurnInMeet.grdCopyWriter.LastUpdatedRowId") = Nothing
        Try
            If e.CommandName = "Edit" Then
                Dim turnInMerchID As Integer = CInt(CType(e.Item, GridDataItem).GetDataKeyValue("turnInMerchID"))

                If _TUECommTurnInMeetResults.IsKilled(turnInMerchID) Then
                    'If row has been killed, rebind and show message
                    mpeCommTurnInMaint.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1015)
                    e.Canceled = True
                    grdCopyWriter.Rebind()
                End If
            End If

            'save the expanded state in the session
            If e.CommandName = RadGrid.ExpandCollapseCommandName Then
                'Is the item about to be expanded or collapsed
                If Not e.Item.Expanded Then
                    Me.CopyWriterExpandedStates.Remove(e.Item.GroupIndex)
                Else
                    'Collapsed - Save its unique index among all the items in the hierarchy
                    Me.CopyWriterExpandedStates(e.Item.GroupIndex) = True
                End If
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

#End Region

#Region "Validations"

    Protected Sub valPickupImageID_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
        Dim value As String = DirectCast(validator.EvaluatedControl, RadNumericTextBox).Text
        Dim IMAGE_KIND_CDE As String = DirectCast(DirectCast(DirectCast(source, ToolTipValidator).Parent.Parent, GridDataItem)("ImageKind").Controls(3), RadComboBox).Text.ToUpper
        If value <> "" And Not (IMAGE_KIND_CDE = "P/U" Or IMAGE_KIND_CDE = "DUP") Then
            mpeCommTurnInMaint.ErrorMessage = "Errors on Page."
            validator.ErrorMessage = "Pickup Image ID can only be entered when Image Kind of pickup or duplicate is selected."
            args.IsValid = False
        End If

    End Sub
    Protected Sub valImageKind_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
        'Dim value As String = DirectCast(validator.EvaluatedControl, RadComboBox).SelectedValue.ToString()
        Dim value As String = DirectCast(validator.EvaluatedControl, RadTextBox).Text.ToString()
        Dim IMAGE_KIND_CDE As String = DirectCast(DirectCast(DirectCast(source, ToolTipValidator).Parent.Parent, GridDataItem)("ImageKind").Controls(3), RadComboBox).Text.ToUpper

        If (IMAGE_KIND_CDE = "NEW") Then


            If DirectCast(DirectCast(DirectCast(source, ToolTipValidator).Parent.Parent, GridDataItem)("RoutefromAd").Controls(0), TextBox).Text = "0" And DirectCast(DirectCast(DirectCast(source, ToolTipValidator).Parent.Parent, GridDataItem)("Thumbnail").Controls(3), RadTextBox).Text = "0" Then
                mpeCommTurnInMaint.ErrorMessage = "Image Kind Error on Page."
                validator.ErrorMessage = "Image Kind Error on Page."
                args.IsValid = False
                DirectCast(DirectCast(DirectCast(source, ToolTipValidator).Parent.Parent, GridDataItem)("Thumbnail").Controls(1), ImageButton).ToolTip = "Sample not selected"
                'DirectCast(DirectCast(DirectCast(source, ToolTipValidator).Parent.Parent, GridDataItem)("Thumbnail").Controls(1), ImageButton).BorderWidth = 5
                'DirectCast(DirectCast(DirectCast(source, ToolTipValidator).Parent.Parent, GridDataItem)("Thumbnail").Controls(1), ImageButton).BorderColor = Drawing.ColorTranslator.FromHtml("#FFC0CB")
                DirectCast(DirectCast(DirectCast(source, ToolTipValidator).Parent.Parent, GridDataItem)("Thumbnail").Controls(1), ImageButton).AlternateText = ""
                DirectCast(DirectCast(DirectCast(source, ToolTipValidator).Parent.Parent, GridDataItem)("Thumbnail").Controls(1), ImageButton).BackColor = Drawing.ColorTranslator.FromHtml("#FFC0CB")
            End If
        Else
            DirectCast(DirectCast(DirectCast(source, ToolTipValidator).Parent.Parent, GridDataItem)("Thumbnail").Controls(1), ImageButton).AlternateText = "Merchandise not required"

        End If
    End Sub

    Protected Sub valModelCategory_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
        Dim value As String = DirectCast(validator.EvaluatedControl, RadComboBox).SelectedValue
        If value = "" And DirectCast(DirectCast(DirectCast(source, ToolTipValidator).Parent.Parent, GridDataItem)("OnOff").Controls(3), RadComboBox).SelectedValue = "ON" Then
            mpeCommTurnInMaint.ErrorMessage = "Errors on Page."
            validator.ErrorMessage = "Model Category is required when ON figure is selected."
            args.IsValid = False
        ElseIf DirectCast(DirectCast(DirectCast(source, ToolTipValidator).Parent.Parent, GridDataItem)("OnOff").Controls(3), RadComboBox).SelectedValue = "OFF" And value <> "" Then
            value = ""
            DirectCast(validator.EvaluatedControl, RadComboBox).SelectedValue = value
        End If
    End Sub

    Protected Sub valAsciiChars_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
        Dim value As String = DirectCast(validator.EvaluatedControl, RadTextBox).Text.Trim
        Dim isValidASCII As Boolean = True

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
                mpeCommTurnInMaint.ErrorMessage = "Errors on Page."
                validator.ErrorMessage = "Invalid Character Found."
                args.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region "EXPORT"
    Private Sub ExportList(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim export As New Export
            If pveMM.Selected Then
                With grdEMM
                    .ExportSettings.IgnorePaging = True
                    .ExportSettings.OpenInNewWindow = True
                    .ExportSettings.ExportOnlyData = True
                    .AllowSorting = False
                    .MasterTableView.Columns(0).Visible = False
                    .MasterTableView.Columns.FindByUniqueName("selColumn").Visible = False
                    .MasterTableView.Columns.FindByUniqueName("ISN").Visible = True
                    '.MasterTableView.Columns.FindByUniqueName("HotListCDE").Visible = False
                    .MasterTableView.Columns.FindByUniqueName("EditCommandColumn").Visible = False
                    .MasterTableView.Columns.FindByUniqueName("DeleteColumn").Visible = False
                    For Each column As GridColumn In grdEMM.MasterTableView.Columns
                        For Each item As GridDataItem In grdEMM.MasterTableView.Items
                            item(column.UniqueName).Text = item(column.UniqueName).Text.Replace(".", "")
                        Next
                    Next
                    If Me.piModalExport.ExportText = "PDF" Then
                        export.Append(grdEMM, "     ")
                        export.MultipleExportToPDF(lblPageHeader.Text, True)
                    ElseIf Me.piModalExport.ExportText = "Excel" Then
                        export.Append(grdEMM, "     ")
                        export.MultipleExportToExcel(lblPageHeader.Text)
                    End If
                End With
            End If
            If pvCC.Selected Then
                With grdCC
                    .ExportSettings.IgnorePaging = True
                    .ExportSettings.OpenInNewWindow = True
                    .ExportSettings.ExportOnlyData = True
                    .AllowSorting = False
                    .MasterTableView.Columns(0).Visible = False
                    .MasterTableView.Columns.FindByUniqueName("selColumn").Visible = False
                    .MasterTableView.Columns.FindByUniqueName("ISN").Visible = True
                    '.MasterTableView.Columns.FindByUniqueName("HotListCDE").Visible = False
                    .MasterTableView.Columns.FindByUniqueName("EditCommandColumn").Visible = False
                    .MasterTableView.Columns.FindByUniqueName("DeleteColumn").Visible = False
                    If Me.piModalExport.ExportText = "PDF" Then
                        export.Append(grdCC, "     ")
                        export.MultipleExportToPDF(lblPageHeader.Text, True)
                    ElseIf Me.piModalExport.ExportText = "Excel" Then
                        export.Append(grdCC, "     ")
                        export.MultipleExportToExcel(lblPageHeader.Text)
                    End If
                End With
            End If

            If pvCopyWriter.Selected Then
                With grdCopyWriter
                    .ExportSettings.IgnorePaging = True
                    .ExportSettings.OpenInNewWindow = True
                    .ExportSettings.ExportOnlyData = True
                    .AllowSorting = False
                    .MasterTableView.Columns(0).Visible = False
                    .MasterTableView.Columns.FindByUniqueName("selColumn").Visible = False
                    .MasterTableView.Columns.FindByUniqueName("ISN").Visible = True
                    '.MasterTableView.Columns.FindByUniqueName("HotListCDE").Visible = False
                    .MasterTableView.Columns.FindByUniqueName("EditCommandColumn").Visible = False
                    .MasterTableView.Columns.FindByUniqueName("DeleteColumn").Visible = False
                    If Me.piModalExport.ExportText = "PDF" Then
                        export.Append(grdCopyWriter, "     ")
                        export.MultipleExportToPDF(lblPageHeader.Text, True)
                    ElseIf Me.piModalExport.ExportText = "Excel" Then
                        export.Append(grdCopyWriter, "     ")
                        export.MultipleExportToExcel(lblPageHeader.Text)
                    End If
                End With
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdEMM_GridExporting(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridExportingArgs) Handles grdEMM.GridExporting
        Try
            e.ExportOutput = e.ExportOutput.Replace(CChar(ChrW(&H0)).ToString(), "." & ChrW(26))
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub
#End Region

#Region "Private Methods"
    Private Sub SetAccessToEditAll()
        If Request.QueryString("Action").ToUpper = "INQUIRY" OrElse _
(rtsTurnInMeet.SelectedTab.PageViewID = "pvCopyWriter" And (checkAccessability("CopyTab") = "False")) Or _
(rtsTurnInMeet.SelectedTab.PageViewID = "pveMM" And (checkAccessability("EmmTab") = "False")) Or _
(rtsTurnInMeet.SelectedTab.PageViewID = "pvCC" And (checkAccessability("CoordTab") = "False")) Or _
(rtsTurnInMeet.SelectedTab.PageViewID = "pvCCSimple" And (checkAccessability("CoordTab") = "False")) Then
            rtbeCommTurnInMeet.FindItemByText("Edit All").Visible = False
            rtbeCommTurnInMeet.FindItemByText("Save All").Visible = False
            rtbeCommTurnInMeet.FindItemByText("Cancel All").Visible = False
            EnableDisableButtons("Delete", False)
        Else
            rtbeCommTurnInMeet.FindItemByText("Edit All").Visible = True
        End If
    End Sub

    Private Function SaveRows(ByRef rg As RadGrid, Optional ByVal isEditAll As Boolean = False) As Boolean

        Dim merchandiseInfo As ECommTurnInMeetCreateInfo

        For Each item As GridItem In rg.Items
            If TypeOf item Is GridEditableItem And item.IsInEditMode Then

                Page.Validate("Update")
                If Not IsValid Then
                    mpeCommTurnInMaint.ErrorMessage = "Errors on Page."
                    Return False
                End If

                Dim editItem As GridEditableItem = DirectCast(item, GridEditableItem)

                If rtsTurnInMeet.SelectedIndex = 0 Then

                    merchandiseInfo = New ECommTurnInMeetCreateInfo With
                    {
                        .turnInMerchID = CInt(editItem.GetDataKeyValue("turnInMerchID")),
                        .EMMFollowUpFlag = If(DirectCast(editItem.FindControl("cbxFollowUpFlag"), CheckBox).Checked, "Y", "N"),
                        .FriendlyProdDesc = Server.HtmlDecode(DirectCast(editItem.FindControl("rtxtFriendlyProdDesc"), RadTextBox).Text),
                        .FriendlyColor = DirectCast(editItem.FindControl("rtxtFriendlyColor"), RadTextBox).Text,
                        .LabelID = CInt(DirectCast(editItem.FindControl("rcbLabel"), RadComboBox).SelectedValue),
                        .SizeCategory = DirectCast(editItem.FindControl("rcbSizeCategory"), RadComboBox).SelectedValue,
                        .EMMNotes = Server.HtmlDecode(DirectCast(editItem.FindControl("rtxtEMMNotes"), RadTextBox).Text),
                        .ImageKindCode = DirectCast(editItem("ImageKind").Controls(1), System.Web.UI.WebControls.Literal).Text,
                        .MerchID = CInt(editItem.GetDataKeyValue("MerchID"))
                    }

                    _TUECommTurnInMeetResults.UpdateEMMInfo(merchandiseInfo, SessionWrapper.UserID)
                    ' Session("eCommTurnInMeet.grdEmm.LastUpdatedRowId") = Session("eCommTurnInMeet.grdEmm.LastUpdatedRowId").ToString & "," & merchandiseInfo.turnInMerchID

                ElseIf rtsTurnInMeet.SelectedIndex = 1 Then

                    merchandiseInfo = New ECommTurnInMeetCreateInfo With
                    {
                        .turnInMerchID = CInt(editItem.GetDataKeyValue("turnInMerchID")),
                        .ISN = CStr(editItem.GetDataKeyValue("ISN")),
                        .ImageGrp = DirectCast(editItem.FindControl("rtxtImageGrp"), RadNumericTextBox).Text,
                        .ImageDesc = DirectCast(editItem.FindControl("rtxtImageDesc"), RadTextBox).Text,
                        .ImageNotes = DirectCast(editItem.FindControl("rtxtImageNotes"), RadTextBox).Text,
                        .OnOff = DirectCast(editItem.FindControl("rcbOnOff"), RadComboBox).SelectedValue,
                        .ModelCategory = DirectCast(editItem.FindControl("rcbModelCategory"), RadComboBox).SelectedValue,
                        .AltView = DirectCast(editItem.FindControl("rcbAlternateView"), RadComboBox).SelectedValue,
                        .FeatureSwatch = DirectCast(editItem.FindControl("rcbFeatureRenderSwatch"), RadComboBox).SelectedValue,
                        .PickupImageID = DirectCast(editItem.FindControl("rtxtPickupImageID"), RadNumericTextBox).Text,
                        .ColorCorrect = CChar(DirectCast(editItem.FindControl("rcbColorCorrect"), RadComboBox).Text),
                        .HotListCDE = CChar(DirectCast(editItem.FindControl("rcbHotItem"), RadComboBox).SelectedValue),
                        .StylingNotes = DirectCast(editItem.FindControl("rtxtStylingNotes"), RadTextBox).Text,
                        .CCFollowUpFlag = If(DirectCast(editItem.FindControl("cbxFollowUpFlag"), CheckBox).Checked, "Y", "N"),
                        .ImageKindCode = DirectCast(editItem.FindControl("rcbImageKind"), RadComboBox).SelectedValue,
                        .RoutefromAd = CStr(editItem.GetDataKeyValue("RoutefromAd")),
                        .MerchID = CInt(editItem.GetDataKeyValue("MerchID")),
                        .FriendlyColor = Trim(CStr(DirectCast(editItem.FindControl("radtxtFriendlyColor"), RadTextBox).Text)),
                        .ColorCode = CInt(editItem.GetDataKeyValue("ColorCode")),
                        .SampleSize = CStr(editItem.GetDataKeyValue("SampleSize")),
                        .PrimaryThumbnailUrl = CStr(editItem.GetDataKeyValue("PrimaryThumbnailUrl"))
                    }

                    If ValidateSampleRequired(merchandiseInfo) = True Then
                        _TUECommTurnInMeetResults.UpdateCCInfo(merchandiseInfo, SessionWrapper.UserID)
                        Session("eCommTurnInMeet.grdCC.LastUpdatedRowId") = Session("eCommTurnInMeet.grdCC.LastUpdatedRowId").ToString & "," & merchandiseInfo.turnInMerchID
                    Else
                        Return False
                    End If
                ElseIf rtsTurnInMeet.SelectedIndex = 2 Then

                    Dim turnInMerchID As Integer = CInt(editItem.GetDataKeyValue("turnInMerchID"))
                    Dim copyNotes As String = DirectCast(editItem.FindControl("rtxtCpyNotes"), RadTextBox).Text
                    Dim followUpFlag As String = If(DirectCast(editItem.FindControl("cbxFollowUpFlag"), CheckBox).Checked, "Y", "N")

                    _TUECommTurnInMeetResults.UpdateCWInfo(turnInMerchID, copyNotes, followUpFlag, SessionWrapper.UserID)
                    Session("eCommTurnInMeet.grdCopyWriter.LastUpdatedRowId") = Session("eCommTurnInMeet.grdCopyWriter.LastUpdatedRowId").ToString & "," & turnInMerchID

                End If

            End If
        Next
        Return True

    End Function

    ''' <summary>
    ''' Business Rule: require a sample / Admin Merch Number unless:
    '''       - the image comes from another ad, or
    '''       - the Image Kind is either "No Merch", "Vendor", "P/U", "CR8", or
    '''       - the Image Kind is "DUP" AND the Alternate View is not "SWREF"
    ''' </summary>
    ''' <param name="ccInfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateSampleRequired(ByRef ccInfo As ECommTurnInMeetCreateInfo) As Boolean
        ''Try
        ''    If CInt(ccInfo.RoutefromAd) = 0 And
        ''            (String.Compare(ccInfo.ImageKindCode, "NEW", True) = 0 Or
        ''             (String.Compare(ccInfo.ImageKindCode, "DUP", True) = 0 And String.Compare(ccInfo.AltView.Trim(), "SWREF", True) = 0)) Then

        ''        ' validate the sample / merch ID has been set
        ''        If ccInfo.MerchID = 0 Then

        ''            mpeCommTurnInMaint.ErrorMessage = "Error on Page. Samples have not been selected for all merchandise."
        ''            Return False

        ''            ' End If

        ''        ElseIf ccInfo.MerchID <> 0 Then

        ''            ' Revert to no sample
        ''            ccInfo.MerchID = 0
        ''            ccInfo.SampleSize = "0"
        ''            ' TODO: This should not be necessary as the TTU410 will be updated subsequently, after returning True
        ''            _TUEcommSetupCreate.UpdateMerchId(ccInfo.turnInMerchID, ccInfo.MerchID, CInt(ccInfo.SampleSize), SessionWrapper.UserID)

        ''        End If
        ''    End If
        ''Catch ex As Exception
        ''    Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
        ''    Response.Redirect("~/Error.aspx", False)
        ''End Try

        Try
            Select Case CInt(ccInfo.RoutefromAd)

                Case 0
                    If (String.Compare(ccInfo.ImageKindCode, "NEW", True) = 0 Or
                         (String.Compare(ccInfo.ImageKindCode, "DUP", True) = 0 And String.Compare(ccInfo.AltView.Trim(), "SWREF", True) = 0)) Then

                        Select Case (ccInfo.MerchID)

                            Case 0
                                'pink error
                                mpeCommTurnInMaint.ErrorMessage = "Error on Page. Samples have not been selected for all merchandise."
                                Return False
                            Case Is > 0
                                'thumbnail or merch id
                                'ccInfo.MerchID = 0 this was previous code - do not think it is correct when MerchId is selected from SamplePicker.
                                'ccInfo.SampleSize = "0"

                                'later ? just set client ui
                                _TUEcommSetupCreate.UpdateMerchId(ccInfo.turnInMerchID, ccInfo.MerchID, CInt(ccInfo.SampleSize), SessionWrapper.UserID)
                            Case Else
                                'Error

                        End Select
                    ElseIf (String.Compare(ccInfo.ImageKindCode, "NEW", True) > 0) Then
                        'Merchandise Not Required
                        ccInfo.MerchID = 0
                        'what about MerchId ?? Just not display but leave it ?

                    End If

                Case Is > 0
                    'Routed
                    'what about merchid ?
                    ccInfo.MerchID = 0
                Case Else
                    'error

            End Select


            Return True
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Function

    Private Sub PutRowsInEditMode(ByRef rg As RadGrid, ByVal isEdit As Boolean)
        If rg.Items.Count > 0 Then
            For Each item As GridItem In rg.Items
                Dim turnInMerchID As Integer = CInt(CType(item, GridDataItem).GetDataKeyValue("turnInMerchID"))
                If TypeOf item Is GridEditableItem _
                    And Not _TUECommTurnInMeetResults.IsKilled(turnInMerchID) Then
                    Dim editableItem As GridEditableItem = CType(item, GridDataItem)
                    editableItem.Edit = isEdit
                End If
            Next
            rg.Rebind()
        End If
    End Sub

    Private Sub EnableDisableButtons(ByVal ButtonName As String, ByVal Show As Boolean)
        rtbeCommTurnInMeet.FindItemByText(ButtonName).Enabled = Show
    End Sub

    Private Sub RejectBatch()
        Try
            Dim BatchID As Integer = CInt(_eCommTurnInMaintCtrl.SelectedBatchNum)

            _TUECommTurnInMeetResults.RejectBatch(BatchID, SessionWrapper.UserID)

            mpeCommTurnInMaint.ErrorMessage = "Batch Rejected Successfully."

            grdEMM.MasterTableView.Rebind()
            grdCC.MasterTableView.Rebind()
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub DeleteBatchItems()
        Dim selRowCount As Integer = 0
        Try
            If pveMM.Selected Then
                selRowCount = grdEMM.MasterTableView.GetSelectedItems.Where(Function(x) CChar(x.GetDataKeyValue("RemoveMerchFlag")) = "N"c).Count
            ElseIf pvCC.Selected Then
                selRowCount = grdCC.MasterTableView.GetSelectedItems.Where(Function(x) CChar(x.GetDataKeyValue("RemoveMerchFlag")) = "N"c).Count
                'ElseIf pvCCSimple.Selected Then
                '    selRowCount = grdCCSimple.MasterTableView.GetSelectedItems.Where(Function(x) CChar(x.GetDataKeyValue("RemoveMerchFlag")) = "N"c).Count
            ElseIf pvCopyWriter.Selected Then
                selRowCount = grdCopyWriter.MasterTableView.GetSelectedItems.Where(Function(x) CChar(x.GetDataKeyValue("RemoveMerchFlag")) = "N"c).Count
            End If

            If selRowCount > 0 Then
                If pveMM.Selected Then
                    'Clear the Edit mode, if any row is left in Edit mode.
                    grdEMM.MasterTableView.ClearEditItems()

                    For Each gridSelItem As GridDataItem In grdEMM.MasterTableView.GetSelectedItems.Where(Function(x) CChar(x.GetDataKeyValue("RemoveMerchFlag")) = "N"c)
                        'Update the data in the database.
                        _TUECommTurnInMeetResults.DeleteEMM(CInt(gridSelItem.GetDataKeyValue("turnInMerchID")), SessionWrapper.UserID)
                    Next

                    grdEMM.Rebind()
                ElseIf pvCC.Selected Then
                    'Clear the Edit mode, if any row is left in Edit mode.
                    grdCC.MasterTableView.ClearEditItems()

                    For Each gridSelItem As GridDataItem In grdCC.MasterTableView.GetSelectedItems.Where(Function(x) CChar(x.GetDataKeyValue("RemoveMerchFlag")) = "N"c)
                        'Update the data in the database.
                        _TUECommTurnInMeetResults.DeleteMediaCoord(CInt(gridSelItem.GetDataKeyValue("turnInMerchID")), SessionWrapper.UserID)
                    Next

                    grdCC.Rebind()
                ElseIf pvCopyWriter.Selected Then
                    'Clear the Edit mode, if any row is left in Edit mode.
                    grdCopyWriter.MasterTableView.ClearEditItems()

                    For Each gridSelItem As GridDataItem In grdCopyWriter.MasterTableView.GetSelectedItems.Where(Function(x) CChar(x.GetDataKeyValue("RemoveMerchFlag")) = "N"c)
                        'Update the data in the database.
                        _TUECommTurnInMeetResults.DeleteMerchCoord(CInt(gridSelItem.GetDataKeyValue("turnInMerchID")), SessionWrapper.UserID)
                    Next

                    grdCopyWriter.Rebind()
                End If

                mpeCommTurnInMaint.ErrorMessage = CStr(selRowCount) & If(selRowCount = 1, " Record ", " Records ") & "Deleted Successfully."
            Else
                mpeCommTurnInMaint.ErrorMessage = "Select at least one record."
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub
#End Region

    Protected Sub tuModalOrderColors_SaveOrder(ByVal sender As Object, ByVal e As EventArgs) Handles tuModalOrderColors.SaveOrder
        Dim rlbColorItems As RadListBox = CType(tuModalOrderColors.FindControl("rlbColorItems"), RadListBox)

        For Each item As RadListBoxItem In rlbColorItems.Items
            _TUBatch.UpdateColorSequence(CInt(item.Value), item.Index + 1)
        Next
        grdCC.Rebind()
    End Sub

    Private Function Truncate(ByVal value As String, ByVal maxLength As Integer) As String
        If (String.IsNullOrEmpty(value)) Then
            Return value
        Else
            Return If(value.Length <= maxLength, value, value.Substring(0, maxLength))
        End If
    End Function

    Protected Sub cbxEMMFollowUpFlag_CheckedChanged(sender As Object, e As EventArgs)
        Dim chk As CheckBox = DirectCast(sender, CheckBox)
        _TUECommTurnInMeetResults.UpdateEMMFollowupFlag(CInt(DirectCast(chk.Parent.Parent, GridDataItem).GetDataKeyValue("turnInMerchID")), If(chk.Checked, "Y", "N"), SessionWrapper.UserID)
        mpeCommTurnInMaint.ErrorMessage = "Follow up flag updated."
    End Sub

    Protected Sub cbxCWFollowUpFlag_CheckedChanged(sender As Object, e As EventArgs)
        Dim chk As CheckBox = DirectCast(sender, CheckBox)
        _TUECommTurnInMeetResults.UpdateCWFollowupFlag(CInt(DirectCast(chk.Parent.Parent, GridDataItem).GetDataKeyValue("turnInMerchID")), If(chk.Checked, "Y", "N"), SessionWrapper.UserID)
        mpeCommTurnInMaint.ErrorMessage = "Follow up flag updated."
    End Sub

    Protected Sub cbxCCFollowUpFlag_CheckedChanged(sender As Object, e As EventArgs)
        Dim chk As CheckBox = DirectCast(sender, CheckBox)
        _TUECommTurnInMeetResults.UpdateCCFollowupFlag(CInt(DirectCast(chk.Parent.Parent, GridDataItem).GetDataKeyValue("turnInMerchID")), If(chk.Checked, "Y", "N"), SessionWrapper.UserID)
        mpeCommTurnInMaint.ErrorMessage = "Follow up flag updated."
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
            Or menuitem.Text = "Follow Up Flag" _
            Or menuitem.Text = "Image Group" _
            Or menuitem.Text = "Image Desc" _
            Or menuitem.Text = "Image Notes" _
            Or menuitem.Text = "Styling Notes" _
            Or menuitem.Text = "Image Kind" _
            Or menuitem.Text = "Feature/ Render/ Swatch" _
            Or menuitem.Text = "On/ off Figure" _
            Or menuitem.Text = "Model Category" _
            Or menuitem.Text = "Alternate View" _
            Or menuitem.Text = "Pickup Image ID" _
            Or menuitem.Text = "Color Correct" _
            Or menuitem.Text = "Rush Sample" _
                Then
                menuitem.Visible = False
            End If
        End If
    End Sub
    ''' <summary>
    ''' Iterates through all items in the collection and checks whether image group number is greater than 9. 
    ''' If any image group has value greater than 9, then it returns the image group id otherwise empty string
    ''' </summary>
    ''' <param name="meetingResults">List of items for which image group mappping needs be validated</param>
    ''' <returns>Image group number greater than 9</returns>
    ''' <remarks></remarks>
    Private Function GetInvalidImageGroups(ByRef meetingResults As List(Of ECommTurnInMeetCreateInfo)) As String
        Dim invalidGroupIds As String = String.Empty

        Dim imageGroupCount = From meetingResult In meetingResults
                              Select meetingResult
                              Where meetingResult.RemoveMerchFlag = "N" AndAlso meetingResult.ImageGrp <> String.Empty AndAlso meetingResult.ImageGrp.Trim() <> "0" AndAlso CInt(meetingResult.ImageGrp.Trim()) > 9

        If Not imageGroupCount Is Nothing Then
            For Each item In imageGroupCount
                invalidGroupIds = String.Concat(invalidGroupIds, item.ImageGrp.Trim(), "\n")
            Next
        End If
        Return invalidGroupIds
    End Function
End Class
