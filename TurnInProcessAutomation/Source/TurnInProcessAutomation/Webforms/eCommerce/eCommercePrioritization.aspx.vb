Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BLL.MiscConstants
Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL.Enumerations
Imports BonTon.Common.ExportFunctions
Imports Telerik.Web

Public Class eCommercePrioritization
    Inherits PageBase

    Private _eCommercePrioritizationCtrl As eCommercePrioritizationCtrl = Nothing
    Private _TUEcommPrioritization As New TUEcommPrioritization
    Private _TUEcommSetupCreate As New TUEcommSetupCreate
    Private _TULabel As New TULabel
    Private _TUBrand As New TUBrand
    Private _TU998Parm As New TUTEC998PARM
    Private _TUTMS900PARAMETER As New TUTMS900PARAMETER
    Private _intGridLevel As Integer = 0

    Private _prioritizationExpandedState As Hashtable
    Private _selectedState As Hashtable
    'Private _colorgridExpandedState As Hashtable
    'Private _colorgridselectedState As Hashtable

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

    Public ReadOnly Property eCommPrioritizationCtrl() As eCommercePrioritizationCtrl
        Get
            Return _eCommercePrioritizationCtrl
        End Get
    End Property

    Public ReadOnly Property Sizes As List(Of ClrSizLocLookUp)
        Get
            If Session("eCommercePrioritization.Sizes") Is Nothing Then
                Session("eCommercePrioritization.Sizes") = _TUEcommPrioritization.GetWebcatSizeLookUp()
            End If
            Return CType(Session("eCommercePrioritization.Sizes"), List(Of ClrSizLocLookUp))
        End Get
    End Property

#End Region


#Region "Events"

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim control As Control = LoadControl("~/WebUserControls/eCommerce/eCommercePrioritizationCtrl.ascx")
        If Not control Is Nothing Then
            Me.Master.SideBarPlaceHolder.Controls.Add(control)
        End If

        If TypeOf control Is eCommercePrioritizationCtrl Then
            Me._eCommercePrioritizationCtrl = CType(control, eCommercePrioritizationCtrl)
        ElseIf TypeOf control Is PartialCachingControl And CType(control, PartialCachingControl).CachedControl IsNot Nothing Then
            Me._eCommercePrioritizationCtrl = CType(CType(control, PartialCachingControl).CachedControl, eCommercePrioritizationCtrl)
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.Master.SideBar.Width = 200

            Me._prioritizationExpandedState = Nothing
            Me.Session("_prioritizationExpandedState") = Nothing

            ''reset states
            'Me._prioritizationExpandedState = Nothing
            'Me.Session("_ordersExpandedState") = Nothing
            'Me._selectedState = Nothing
            'Me.Session("_selectedState") = Nothing

            'Me._colorgridExpandedState = Nothing
            'Me.Session("_colorgridExpandedState") = Nothing
            'Me._colorgridselectedState = Nothing
            'Me.Session("_colorgridselectedState") = Nothing

        End If

        'Assign Event Handler to Export button click.
        AddHandler Me.tuModalExport.OkButton.Click, AddressOf Me.ExportList

        'Me.tuModalExport.Visible = False
    End Sub

    Private Sub rtbeCommercePrioritization_ButtonClick(sender As Object, e As Telerik.Web.UI.RadToolBarEventArgs) Handles rtbeCommercePrioritization.ButtonClick
        Try
            If TypeOf e.Item Is RadToolBarButton Then
                Dim radToolBarButton As RadToolBarButton = DirectCast(e.Item, RadToolBarButton)

                Select Case radToolBarButton.CommandName
                    Case "Back"
                        Me._prioritizationExpandedState = Nothing
                        Me.Session("_prioritizationExpandedState") = Nothing
                        Response.Redirect(PreviousPageUrl, False)
                    Case "Submit"
                        SubmitData()
                    Case "Retrieve"
                        tblFloodOptions.Visible = True
                        tblFloodColorOptions.Visible = True
                        tblFloodSizeOptions.Visible = True
                        Me._prioritizationExpandedState = Nothing
                        Me.Session("_prioritizationExpandedState") = Nothing
                        grdeCommercePrioritization.Rebind()
                    Case "Reject"
                        ' On the success filter, user needs to push data back to Pending.
                        PushDataToPending()
                    Case "LevelDown"
                        ExpandGridHierarchyLevel(True)
                    Case "LevelUp"
                        ExpandGridHierarchyLevel(False)
                    Case "Reset"
                        Me._prioritizationExpandedState = Nothing
                        Me.Session("_prioritizationExpandedState") = Nothing
                        Response.Redirect(Request.Url.ToString, False)
                End Select
            End If
        Catch ex As Exception
            RespondToError(System.Reflection.MethodBase.GetCurrentMethod.Name, ex.Message)
        End Try
    End Sub



    Private Sub grdeCommercePrioritization_DataBound(sender As Object, e As System.EventArgs) Handles grdeCommercePrioritization.DataBound

        'Retain Hierarchy state.
        Dim indexes As String() = New String(Me.ExpandedStates.Keys.Count - 1) {}
        Me.ExpandedStates.Keys.CopyTo(indexes, 0)

        For Each item As GridDataItem In grdeCommercePrioritization.Items

            If item.OwnerTableView.Name = "grdFirstLevel" Then
                Dim isnKey As String = CStr(item.GetDataKeyValue("ISN"))

                If indexes.Contains(isnKey) Then
                    item.Expanded = True

                    For Each detailGridItem01 As GridDataItem In item.ChildItem.NestedTableViews(0).Items

                        Dim imageID As String = CStr(detailGridItem01.GetDataKeyValue("ImageID"))
                        Dim imageKey As String = isnKey & "_" & imageID

                        If indexes.Contains(imageKey) Then
                            detailGridItem01.Expanded = True
                        End If
                    Next
                End If
            End If

        Next

    End Sub

    Private Sub grdeCommercePrioritization_GridExporting(sender As Object, e As Telerik.Web.UI.GridExportingArgs) Handles grdeCommercePrioritization.GridExporting
        Select Case e.ExportType
            Case Telerik.Web.UI.ExportType.Excel
                e.ExportOutput = e.ExportOutput.Replace("<body>", "<body><table><tr><td>" + lblPageHeader.Text + "</td></tr></table>")
        End Select

    End Sub

    Private Sub grdeCommercePrioritization_PreRender(sender As Object, e As System.EventArgs) Handles grdeCommercePrioritization.PreRender
        If (rowIndex > -1) Then
            Dim masterTableItem As GridDataItem = DirectCast(grdeCommercePrioritization.MasterTableView.Items(rowIndex), GridDataItem)
            masterTableItem.Expanded = True
            rowIndex = -1
        End If

        'Show/Hide Flood Options based on existence of records in that level.
        tblFloodOptions.Visible = False
        tblFloodColorOptions.Visible = False
        tblFloodSizeOptions.Visible = False

        'Enable the Toolbar buttons if Grid contains at least one row.
        Dim PrioritizationRecords As GridItemCollection = grdeCommercePrioritization.MasterTableView.Items

        If PrioritizationRecords.Count > 0 Then
            'Show Flood buttons.
            tblFloodOptions.Visible = True

            'Hide Color and Size level Flood options if no data exists at those levels.
            For Each PrioritizationGridRecord As GridDataItem In PrioritizationRecords
                If PrioritizationGridRecord.Expanded Then
                    'Show color options grid when there are child items
                    Dim ColorGridRecords As GridItemCollection = PrioritizationGridRecord.ChildItem.NestedTableViews(0).Items

                    ' Check the all color level records;
                    For Each colorGridRecord As GridDataItem In ColorGridRecords

                        If ColorGridRecords.Count > 0 Then
                            tblFloodColorOptions.Visible = True
                        Else
                            tblFloodColorOptions.Visible = False
                        End If

                        If colorGridRecord.Expanded Then
                            'Show size option flood when we have a grid with rows.
                            Dim sizeGridRecords As GridItemCollection = colorGridRecord.ChildItem.NestedTableViews(0).Items
                            If sizeGridRecords.Count > 0 Then
                                tblFloodSizeOptions.Visible = True

                                LoadrcbFloodVendorWCSizeFamily()
                            Else
                                tblFloodSizeOptions.Visible = False
                            End If
                        End If


                    Next
                End If
            Next
        End If

        If _intGridLevel = 2 Or _intGridLevel = 3 Then
            grdeCommercePrioritization.MasterTableView.Rebind() 'Refresh ISN level data.

            If _intGridLevel = 3 Then
                grdeCommercePrioritization.MasterTableView.DetailTables(0).Rebind() 'Refresh Color level data.
            End If
        End If

        'Disable the Submit button when there are no records OR if the records are filtered as Update or Deleted
        rtbeCommercePrioritization.FindItemByText("Submit").Enabled = (PrioritizationRecords.Count > 0) AndAlso (_eCommercePrioritizationCtrl.SelectedStatus <> "U") AndAlso (_eCommercePrioritizationCtrl.SelectedStatus <> "D")
        rtbeCommercePrioritization.FindItemByText("Reject").Enabled = (PrioritizationRecords.Count > 0) AndAlso (_eCommercePrioritizationCtrl.SelectedStatus = "U")
        rtbeCommercePrioritization.FindItemByText("Export").Enabled = (PrioritizationRecords.Count > 0)
        rtbeCommercePrioritization.FindItemByText("Level Down").Enabled = (PrioritizationRecords.Count > 0)
        rtbeCommercePrioritization.FindItemByText("Level Up").Enabled = (PrioritizationRecords.Count > 0)

    End Sub

    Private Sub grdeCommercePrioritization_UpdateCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles grdeCommercePrioritization.UpdateCommand
        Try
            If (TypeOf e.Item Is GridEditableItem And e.Item.IsInEditMode) Then

                Dim editItem As GridEditableItem = DirectCast(e.Item, GridEditableItem)
                Dim GridRowData As New ECommPrioritizationInfo()
                Dim parentItem As GridDataItem
                Dim SwatchFlg As String = String.Empty
                Dim ColorFlg As String = String.Empty
                Dim SizeFlg As String = String.Empty

                If editItem.ItemIndex > -1 Then
                    'Get all the required values from the Grid Row being Updated.
                    Select Case editItem.OwnerTableView.Name
                        Case "grdFirstLevel"
                            GridRowData.ISN = CInt(editItem.GetDataKeyValue("ISN"))
                            GridRowData.ColorFlg = CChar(DirectCast(editItem.FindControl("rcbColorFlg"), RadComboBox).SelectedValue)

                            GridRowData.SwatchFlg = CChar(DirectCast(editItem.FindControl("rcbSwatchFlg"), RadComboBox).SelectedValue)
                            If GridRowData.ColorFlg = "N" Then
                                GridRowData.SwatchFlg = "N"c 'Set the product as Non-Swatchable if Color Flag is "N".
                            End If

                            GridRowData.SizeFlg = CChar(DirectCast(editItem.FindControl("rcbSizeFlg"), RadComboBox).SelectedValue)
                            GridRowData.WebCatgyCde = CInt(editItem.GetDataKeyValue("WebCatgyCde"))
                            GridRowData.LabelID = CInt(DirectCast(editItem.FindControl("rcbLabel"), RadComboBox).SelectedValue)
                            GridRowData.BrandID = CShort(DirectCast(editItem.FindControl("rcbWebCatBrand"), RadComboBox).SelectedValue)
                            GridRowData.AgeCde = CShort(DirectCast(editItem.FindControl("rcbAge"), RadComboBox).SelectedValue)
                            GridRowData.GenderCde = CShort(DirectCast(editItem.FindControl("rcbGender"), RadComboBox).SelectedValue)

                            'Validate Swatch Flag.
                            If GridRowData.SwatchFlg <> "Y"c And GridRowData.SwatchFlg <> "N"c Then
                                mpeCommercePrioritization.ErrorMessage = "Please select a value for Swatch Flag."
                                e.Canceled = True
                                Exit Sub
                            End If

                            'Validate Color Flag.
                            If GridRowData.ColorFlg <> "Y"c And GridRowData.ColorFlg <> "N"c Then
                                mpeCommercePrioritization.ErrorMessage = "Please select a value for Color Flag."
                                e.Canceled = True
                                Exit Sub
                            End If

                            'Validate Size Flag.
                            If GridRowData.SizeFlg <> "Y"c And GridRowData.SizeFlg <> "N"c Then
                                mpeCommercePrioritization.ErrorMessage = "Please select a value for Size Flag."
                                e.Canceled = True
                                Exit Sub
                            End If

                            'Validate Label ID
                            If GridRowData.LabelID = 0 Then
                                mpeCommercePrioritization.ErrorMessage = "Please select a value for Label."
                                e.Canceled = True
                                Exit Sub
                            End If

                            'Validate Web Cat Brand ID
                            If GridRowData.BrandID = 0 Then
                                mpeCommercePrioritization.ErrorMessage = "Please select a value for Web Cat Brand."
                                e.Canceled = True
                                Exit Sub
                            End If

                            'Update the ISN Level data in the database.
                            _TUEcommPrioritization.UpdateISNLevelData(GridRowData, SessionWrapper.UserID)

                            mpeCommercePrioritization.ErrorMessage = "Data Saved Successfully."
                        Case "grdSecondLevel"
                            Page.Validate("UpdateClrLvl")

                            If Not IsValid Then
                                mpeCommercePrioritization.ErrorMessage = "Errors on Page."
                                e.Canceled = True
                                Exit Sub
                            End If

                            parentItem = CType(editItem.OwnerTableView.ParentItem, GridDataItem)
                            SwatchFlg = parentItem.GetDataKeyValue("SwatchFlg").ToString
                            ColorFlg = parentItem.GetDataKeyValue("ColorFlg").ToString

                            GridRowData.TurnInMerchID = CInt(editItem.GetDataKeyValue("TurnInMerchID"))

                            If DirectCast(editItem.FindControl("rtxtFeatureId"), RadNumericTextBox).Value IsNot Nothing Then
                                GridRowData.FeatureID = CInt(DirectCast(editItem.FindControl("rtxtFeatureId"), RadNumericTextBox).Value)
                            End If

                            If DirectCast(editItem.FindControl("rtxtImageId"), RadNumericTextBox).Value IsNot Nothing Then
                                GridRowData.ImageID = CInt(DirectCast(editItem.FindControl("rtxtImageId"), RadNumericTextBox).Value)
                            End If

                            GridRowData.ProductName = DirectCast(editItem.FindControl("rtxtProductName"), RadTextBox).Text

                            If ColorFlg = "Y" And SwatchFlg = "N" Then
                                GridRowData.FriendlyColor = "" 'Void the Friendly Color for Non-Swatch Product.
                            Else
                                GridRowData.FriendlyColor = DirectCast(editItem.FindControl("rtxtFriendlyColor"), RadTextBox).Text
                            End If

                            'Validate Friendly Color for Swatchable Product.
                            If SwatchFlg = "Y" And String.IsNullOrEmpty(GridRowData.FriendlyColor.Trim) Then
                                mpeCommercePrioritization.ErrorMessage = "Please enter a value for Friendly Color."
                                e.Canceled = True
                                Exit Sub
                            End If

                            If ColorFlg = "Y" And SwatchFlg = "Y" Then
                                GridRowData.NonSwatchClrCde = 0 'Void the Non Swatch Color for Swatchable Product.
                            Else
                                GridRowData.NonSwatchClrCde = CInt(DirectCast(editItem.FindControl("rcbNonSwatchColor"), RadComboBox).SelectedValue)
                            End If

                            GridRowData.ImageGroup = CShort(DirectCast(editItem.FindControl("rtxtImageGroup"), RadTextBox).Text)

                            'Validate Non Swatch Color for Non-Swatch Product.
                            If SwatchFlg = "N" And GridRowData.NonSwatchClrCde = 0 And ColorFlg = "Y" Then
                                mpeCommercePrioritization.ErrorMessage = "Please select a Non Swatch Color."
                                e.Canceled = True
                                Exit Sub
                            End If

                            Dim rcbEditClrFam As RadComboBox = DirectCast(editItem.FindControl("rcbColorFamily"), RadComboBox)
                            GridRowData.ColorFamily = If(rcbEditClrFam.CheckedItems.Count > 0, String.Join(",", rcbEditClrFam.CheckedItems.Select(Function(a) a.Value)), "0")

                            'Validate Color Family.
                            If ColorFlg = "Y" And rcbEditClrFam.CheckedItems.Count = 0 Then
                                mpeCommercePrioritization.ErrorMessage = "Please select at least one Color from Color Family drop down."
                                e.Canceled = True
                                Exit Sub
                            End If

                            GridRowData.FRS = DirectCast(editItem.FindControl("rcbFRS"), RadComboBox).SelectedValue

                            'Validate Feature Render Swatch.
                            'If GridRowData.FeatureID = GridRowData.ImageID And GridRowData.FRS <> "FEAT" Then
                            '    mpeCommercePrioritization.ErrorMessage = "Please select the Image Category as Feature."
                            '    e.Canceled = True
                            '    Exit Sub
                            'End If

                            If (GridRowData.FRS.ToUpper() = "FEAT" Or GridRowData.FRS.ToUpper() = "STDALN") And GridRowData.FeatureID <> GridRowData.ImageID Then
                                mpeCommercePrioritization.ErrorMessage = String.Concat("Feature Id and Image Id should be same for a ", IIf(GridRowData.FRS.ToUpper() = "FEAT", "Feature.", "Stand Alone."))
                                e.Canceled = True
                                Exit Sub
                            End If

                            If GridRowData.FRS <> "FEAT" And GridRowData.FRS <> "STDALN" And GridRowData.FeatureID = 0 Then
                                mpeCommercePrioritization.ErrorMessage = "Feature Id cannot be Zero."
                                e.Canceled = True
                                Exit Sub
                            End If

                            GridRowData.ImageNotes = CStr(DirectCast(editItem.FindControl("rtxtImageNotes"), RadTextBox).Text)
                            GridRowData.EMMNotes = CStr(DirectCast(editItem.FindControl("rtxtEMMNotes"), RadTextBox).Text)

                            'Update the Color Level data in the database.
                            _TUEcommPrioritization.UpdateColorLevelData(GridRowData, SessionWrapper.UserID)
                            _intGridLevel = 2

                            mpeCommercePrioritization.ErrorMessage = "Data Saved Successfully."
                        Case "grdThirdLevel"
                            Dim clrLvlItem As GridDataItem = CType(editItem.OwnerTableView.ParentItem, GridDataItem)
                            parentItem = CType(editItem.OwnerTableView.ParentItem.OwnerTableView.ParentItem, GridDataItem)
                            SizeFlg = parentItem.GetDataKeyValue("SizeFlg").ToString

                            GridRowData.TurnInMerchID = CInt(clrLvlItem.GetDataKeyValue("TurnInMerchID"))
                            GridRowData.UPC = CDec(editItem.GetDataKeyValue("UPC"))

                            If SizeFlg = "N" Then
                                GridRowData.WebCatSizeID = 0 'Void the Webcat Size for the products having Size Flag as "N".
                            Else
                                Dim rcbWebCatSize As RadComboBox = DirectCast(editItem.FindControl("rcbWebCatSize"), RadComboBox)
                                If rcbWebCatSize.Text <> "" And rcbWebCatSize.SelectedValue = "" Then
                                    GridRowData.WebCatSizeID = CInt(DirectCast(editItem.FindControl("hfWebCatSizeID"), HiddenField).Value)
                                Else
                                    GridRowData.WebCatSizeID = CInt(rcbWebCatSize.SelectedValue)
                                End If
                            End If

                            Dim rcbEditSizeFam As RadComboBox = DirectCast(editItem.FindControl("rcbSizeFamily"), RadComboBox)
                            GridRowData.SizeFamily = If(rcbEditSizeFam.CheckedItems.Count > 0, String.Join(",", rcbEditSizeFam.CheckedItems.Select(Function(a) a.Value)), "0")

                            'Validate Color Family.
                            If SizeFlg = "Y" And rcbEditSizeFam.CheckedItems.Count = 0 Then
                                mpeCommercePrioritization.ErrorMessage = "Please select at least one Size from Size Family drop down."
                                e.Canceled = True
                                Exit Sub
                            End If


                            'Update the Size Level data in the database.
                            _TUEcommPrioritization.UpdateSizeLevelData(GridRowData, SessionWrapper.UserID)
                            _intGridLevel = 3

                            mpeCommercePrioritization.ErrorMessage = "Data Saved Successfully."
                    End Select
                End If
            End If
        Catch ex As Exception
            RespondToError(System.Reflection.MethodBase.GetCurrentMethod.Name, ex.Message)
        End Try
    End Sub
    Dim rowIndex As Integer = -1
    Private Sub grdeCommercePrioritization_DeleteCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles grdeCommercePrioritization.DeleteCommand
        Try

            If (TypeOf e.Item Is GridDataItem) Then
                Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim parentItem As GridDataItem = CType(dataItem.OwnerTableView.ParentItem, GridDataItem)
                rowIndex = parentItem.ItemIndex
                Dim MerchantID As Integer = CInt(dataItem.GetDataKeyValue("TurnInMerchID"))
                Dim chrStatus As Char = CChar(parentItem.GetDataKeyValue("StatusFlg"))

                If dataItem.ItemIndex > -1 Then
                    Me._prioritizationExpandedState = Nothing
                    Me.Session("_prioritizationExpandedState") = Nothing

                    If dataItem.OwnerTableView.Items.Count = 1 Then
                        _intGridLevel = 2 'This setting will allow the Grid to Rebind at ISN level (in grdeCommercePrioritization_PreRender event).
                    Else
                        _intGridLevel = 3 'This setting will allow the Grid to Rebind at Color level (in grdeCommercePrioritization_PreRender event).
                    End If

                    'To Delete / Activate a Color Level record in the database - Set the Status to D (Delete) / P (Pending) respectively.                
                    If (chrStatus <> "D"c) Then
                        _TUEcommPrioritization.DeleteColorLevelData(MerchantID, "D"c, SessionWrapper.UserID)
                        mpeCommercePrioritization.ErrorMessage = "Selected row has been flagged for removal."
                    Else
                        _TUEcommPrioritization.DeleteColorLevelData(MerchantID, "P"c, SessionWrapper.UserID)
                        mpeCommercePrioritization.ErrorMessage = "Selected row has been activated."
                    End If
                End If
            End If
        Catch ex As Exception
            RespondToError(System.Reflection.MethodBase.GetCurrentMethod.Name, ex.Message)
        End Try
    End Sub

    Private Function GetWebCatToolTip(ByVal WebCatgyList As String) As String
        Dim returnVal As String = ""
        Dim OtherCategoryCodes As List(Of String) = WebCatgyList.Split(CChar(",")).ToList
        OtherCategoryCodes.Remove("0")
        If OtherCategoryCodes.Count > 0 Then
            Try
                For Each wc As String In OtherCategoryCodes
                    Dim cde As Integer = CInt(wc)

                    If AllWebCats.Where(Function(x) x.CategoryCode = cde).ToList.Count = 0 Then
                        Dim objGetApplicationObjectsService As New GetGlobalObjectsService
                        objGetApplicationObjectsService.GetAllApplicationObjects()  'this will create/populate all application objects.
                    End If

                    returnVal &= Server.HtmlDecode(AllWebCats.Where(Function(x) x.CategoryCode = cde).FirstOrDefault.CategoryLongDesc) & vbCrLf
                Next

            Catch ex As Exception

            End Try
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

        Try
            If AllWebCats.Where(Function(x) x.CategoryCode = DefaultCategoryCode).ToList.Count = 0 Then
                Dim objGetApplicationObjectsService As New GetGlobalObjectsService
                objGetApplicationObjectsService.GetAllApplicationObjects()  'this will create/populate all application objects.
            End If
            returnVal = AllWebCats.Where(Function(x) x.CategoryCode = DefaultCategoryCode).FirstOrDefault.CategoryLongDesc
        Catch ex As Exception
        End Try
        Return returnVal
    End Function

    Private Sub grdeCommercePrioritization_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grdeCommercePrioritization.ItemDataBound

        Try
            'Non Editable Item
            If (TypeOf e.Item Is GridDataItem And Not e.Item.IsInEditMode) Then
                'Flag the rows that have missing data.
                Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim chrStatus As Char
                Dim strToolTip As String = "Fields with Missing Data:"
                Dim isValidData As Boolean = True

                Select Case dataItem.OwnerTableView.Name
                    ''
                    ''      PRODUCT LEVEL DATA 
                    ''
                    Case "grdFirstLevel"
                        chrStatus = CChar(dataItem.GetDataKeyValue("StatusFlg"))

                        CType(dataItem("WebCategories").FindControl("ltrPrimaryWebCategory"), Literal).Text = SetDefaultCategory(CInt(dataItem.GetDataKeyValue("WebCatgyCde")))

                        'Set Tooltip for Web Categories field: List of Secondary Web Categories.
                        dataItem("WebCategories").ToolTip = GetWebCatToolTip(dataItem.GetDataKeyValue("WebCatgyList").ToString)

                        If String.IsNullOrEmpty(dataItem.GetDataKeyValue("SwatchFlg").ToString.Trim) Then
                            strToolTip += vbCrLf & " - Swatch Flag"
                            isValidData = False
                        End If

                        If String.IsNullOrEmpty(dataItem.GetDataKeyValue("ColorFlg").ToString.Trim) Then
                            strToolTip += vbCrLf & " - Color Flag"
                            isValidData = False
                        End If

                        If String.IsNullOrEmpty(dataItem.GetDataKeyValue("SizeFlg").ToString.Trim) Then
                            strToolTip += vbCrLf & " - Size Flag"
                            isValidData = False
                        End If

                        If CInt(dataItem.GetDataKeyValue("BrandID").ToString) = 0 Then
                            If String.IsNullOrEmpty(CType(dataItem("WebCatBrand").Controls(0), DataBoundLiteralControl).Text.Trim) Then
                                strToolTip += vbCrLf & " - Web Cat Brand"
                            Else
                                strToolTip += vbCrLf & " - Web Cat Brand. The displayed value is a Label lookup."
                            End If

                            isValidData = False
                        End If

                        If String.IsNullOrEmpty(CType(dataItem("Age").Controls(0), DataBoundLiteralControl).Text.Trim) Then
                            strToolTip += vbCrLf & " - Age"
                            isValidData = False
                        End If

                        If String.IsNullOrEmpty(CType(dataItem("Gender").Controls(0), DataBoundLiteralControl).Text.Trim) Then
                            strToolTip += vbCrLf & " - Gender"
                            isValidData = False
                        End If

                        If dataItem.GetDataKeyValue("IsValidFlg").ToString = "N" Then
                            If isValidData Then
                                strToolTip += vbCrLf & " - Check Color level data."
                            End If
                            isValidData = False
                        End If
                        ''
                        ''      COLOR LEVEL DATA 
                        ''
                    Case "grdSecondLevel"
                        Dim parentDataItem As GridDataItem = CType(dataItem.OwnerTableView.ParentItem, GridDataItem)
                        Dim ColorFlag As String = parentDataItem.GetDataKeyValue("ColorFlg").ToString
                        Dim SwatchFlag As String = parentDataItem.GetDataKeyValue("SwatchFlg").ToString

                        chrStatus = CChar(parentDataItem.GetDataKeyValue("StatusFlg"))

                        If String.IsNullOrEmpty(CType(dataItem("FeatureId").Controls(0), DataBoundLiteralControl).Text.Trim) Or _
                            CType(dataItem("FeatureId").Controls(0), DataBoundLiteralControl).Text.Trim = "0" Then
                            strToolTip += vbCrLf & " - Feature Id"
                            isValidData = False
                        End If

                        If String.IsNullOrEmpty(CType(dataItem("ImageId").Controls(0), DataBoundLiteralControl).Text.Trim) Or _
                            CType(dataItem("ImageId").Controls(0), DataBoundLiteralControl).Text.Trim = "0" Then
                            strToolTip += vbCrLf & " - Image Id"
                            isValidData = False
                        End If

                        If String.IsNullOrEmpty(CType(dataItem("ProductName").Controls(0), DataBoundLiteralControl).Text.Trim) Then
                            strToolTip += vbCrLf & " - Product Name"
                            isValidData = False
                        End If

                        If ColorFlag = "Y" And SwatchFlag = "Y" And String.IsNullOrEmpty(CType(dataItem("FriendlyColor").Controls(0), DataBoundLiteralControl).Text.Trim) Then
                            strToolTip += vbCrLf & " - Friendly Color"
                            isValidData = False
                        End If

                        If ColorFlag = "Y" And SwatchFlag = "N" Then
                            If String.IsNullOrEmpty(CType(dataItem("NonSwatchColor").Controls(0), DataBoundLiteralControl).Text.Trim) Then
                                strToolTip += vbCrLf & " - Non Swatch Color"
                                isValidData = False
                            End If

                            If String.IsNullOrEmpty(CType(dataItem("ColorFamily").Controls(1), Label).Text.Trim) Then
                                strToolTip += vbCrLf & " - Color Family"
                                isValidData = False
                            End If
                        End If

                        If dataItem.GetDataKeyValue("IsValidFlg").ToString = "N" Then
                            If isValidData Then
                                strToolTip += vbCrLf & " - Check Size level data."
                            End If
                            isValidData = False
                        End If
                        ''
                        ''      SIZE LEVEL DATA 
                        ''
                    Case "grdThirdLevel"
                        Dim parentDtItem As GridDataItem = CType(dataItem.OwnerTableView.ParentItem.OwnerTableView.ParentItem, GridDataItem)
                        Dim SizeFlag As String = parentDtItem.GetDataKeyValue("SizeFlg").ToString

                        chrStatus = CChar(parentDtItem.GetDataKeyValue("StatusFlg"))

                        If SizeFlag = "Y" Then
                            If String.IsNullOrEmpty(CType(dataItem("WebcatSize").Controls(0), DataBoundLiteralControl).Text.Trim) Then
                                strToolTip += vbCrLf & " - Webcat Size"
                                isValidData = False
                            End If
                        End If

                        If String.IsNullOrEmpty(CType(dataItem("SizeFamily").Controls(1), Label).Text.Trim) Then
                            strToolTip += vbCrLf & " - Size Family"
                            isValidData = False
                        End If

                        If dataItem.GetDataKeyValue("IsValidFlg").ToString = "N" Then
                            If isValidData Then
                                strToolTip += vbCrLf & " - Save WebcatSize and/or SizeFamily."
                            End If
                            isValidData = False
                        End If
                End Select
                'Show/Hide Warning Icon with a ToolTip.
                If isValidData Then
                    CType(dataItem("MissingData").Controls(0), Image).ImageUrl = ""
                    CType(dataItem("MissingData").Controls(0), Image).Visible = False
                Else
                    CType(dataItem("MissingData").Controls(0), Image).ImageUrl = "~/Images/Warning.png"
                    CType(dataItem("MissingData").Controls(0), Image).Visible = True
                    dataItem("MissingData").ToolTip = strToolTip
                End If

                'Enable/Disable Edit button.
                If chrStatus = "U"c Then 'U - Uploaded Successfully
                    'EDIT button
                    CType(dataItem("EditColumn").Controls(0), ImageButton).Enabled = False
                    CType(dataItem("EditColumn").Controls(0), ImageButton).ToolTip = "Edit not allowed as this product is already imported to WebCat system."

                    If dataItem.OwnerTableView.Name = "grdSecondLevel" Then
                        'DELETE / ACTIVATE button
                        CType(dataItem("DeleteColumn").Controls(0), ImageButton).Enabled = False
                        CType(dataItem("DeleteColumn").Controls(0), ImageButton).ToolTip = "Delete not allowed as this product is already imported to WebCat system."
                    End If
                ElseIf chrStatus = "D"c Then 'D - Deleted / Inactive
                    'EDIT button
                    CType(dataItem("EditColumn").Controls(0), ImageButton).Enabled = False
                    CType(dataItem("EditColumn").Controls(0), ImageButton).ToolTip = "Edit not allowed as this product is Inactive."

                    If dataItem.OwnerTableView.Name = "grdSecondLevel" Then
                        'DELETE / ACTIVATE button
                        CType(dataItem("DeleteColumn").Controls(0), ImageButton).ImageUrl = "~/Images/CheckMark.gif"
                        CType(dataItem("DeleteColumn").Controls(0), ImageButton).ToolTip = "Activate"
                    End If
                Else 'P - Pending, F - Failed
                    CType(dataItem("EditColumn").Controls(0), ImageButton).Enabled = True
                End If
            End If


            'Handle EDIT command - Populate the values for all the columns with Comboboxes.
            If (TypeOf e.Item Is GridEditableItem And e.Item.IsInEditMode) Then
                Dim editItem As GridEditableItem = DirectCast(e.Item, GridEditableItem)

                If editItem.GetDataKeyValue("IsValidFlg").ToString = "N" Then
                    CType(editItem("MissingData").Controls(0), Image).ImageUrl = "~/Images/Warning.png"
                    CType(editItem("MissingData").Controls(0), Image).Visible = True
                Else
                    CType(editItem("MissingData").Controls(0), Image).ImageUrl = ""
                    CType(editItem("MissingData").Controls(0), Image).Visible = False
                End If

                Select Case editItem.OwnerTableView.Name
                    ''
                    ''      PRODUCT LEVEL DATA 
                    ''
                    Case "grdFirstLevel"
                        CType(editItem("WebCategories").FindControl("rtxtWebCategories"), RadTextBox).Text = SetDefaultCategory(CInt(editItem.GetDataKeyValue("WebCatgyCde")))

                        'Set Tooltip for Web Categories field: List of Secondary Web Categories.
                        editItem("WebCategories").ToolTip = GetWebCatToolTip(editItem.GetDataKeyValue("WebCatgyList").ToString)

                        Dim ClrFlg As String = editItem.GetDataKeyValue("ColorFlg").ToString

                        '1. Label
                        Dim cmbLabel As RadComboBox = DirectCast(editItem.FindControl("rcbLabel"), RadComboBox)
                        Dim hfLbl As HiddenField = DirectCast(editItem.FindControl("hfLabel"), HiddenField)

                        With cmbLabel
                            .DataSource = _TULabel.GetLabelsByBrand(CStr(editItem.GetDataKeyValue("ISNBrandID")))
                            '.DataSource = _TULabel.GetAllLabels()
                            .DataTextField = "LabelDesc"
                            .DataValueField = "LabelId"
                            .DataBind()
                            .Items.Insert(0, New RadComboBoxItem("", "0"))
                        End With

                        If cmbLabel.FindItemByValue(hfLbl.Value.Trim) IsNot Nothing Then
                            cmbLabel.FindItemByValue(hfLbl.Value.Trim).Selected = True
                        End If

                        '2. Brand
                        Dim cmbBrandID As RadComboBox = DirectCast(editItem.FindControl("rcbWebCatBrand"), RadComboBox)
                        Dim hfBrandID As HiddenField = DirectCast(editItem.FindControl("hfWebCatBrand"), HiddenField)

                        With cmbBrandID
                            .DataSource = _TUBrand.GetAllBrands().ToList()
                            .DataTextField = "BrandDesc"
                            .DataValueField = "BrandId"
                            .DataBind()
                            .Items.Insert(0, New RadComboBoxItem("", "0"))
                        End With

                        If cmbBrandID.FindItemByText(hfBrandID.Value.Trim) IsNot Nothing Then
                            cmbBrandID.FindItemByText(hfBrandID.Value.Trim).Selected = True
                        End If

                        '3. Age
                        Dim cmbAge As RadComboBox = DirectCast(editItem.FindControl("rcbAge"), RadComboBox)
                        Dim hfAg As HiddenField = DirectCast(editItem.FindControl("hfAge"), HiddenField)

                        cmbAge.DataSource = _TU998Parm.GetAllAgeCodes()
                        cmbAge.DataTextField = "ParmText"
                        cmbAge.DataValueField = "ParmValue"
                        cmbAge.DataBind()
                        cmbAge.Items.Insert(0, New RadComboBoxItem("", "0"))

                        If cmbAge.FindItemByValue(hfAg.Value.Trim) IsNot Nothing Then
                            cmbAge.FindItemByValue(hfAg.Value.Trim).Selected = True
                        End If

                        '4. Gender
                        Dim cmbGender As RadComboBox = DirectCast(editItem.FindControl("rcbGender"), RadComboBox)
                        Dim hfGend As HiddenField = DirectCast(editItem.FindControl("hfGender"), HiddenField)

                        cmbGender.DataSource = _TU998Parm.GetAllGenderCodes()
                        cmbGender.DataTextField = "ParmText"
                        cmbGender.DataValueField = "ParmValue"
                        cmbGender.DataBind()
                        cmbGender.Items.Insert(0, New RadComboBoxItem("", "0"))

                        If cmbGender.FindItemByValue(hfGend.Value.Trim) IsNot Nothing Then
                            cmbGender.FindItemByValue(hfGend.Value.Trim).Selected = True
                        End If

                        '5. Swatch Flag
                        Dim rcbSwatchFlg As RadComboBox = DirectCast(editItem.FindControl("rcbSwatchFlg"), RadComboBox)

                        If ClrFlg = "N" Then
                            rcbSwatchFlg.Enabled = False
                        End If


                        ''
                        ''      COLOR LEVEL DATA 
                        ''
                    Case "grdSecondLevel"
                        Dim ClrLookupVals As New List(Of ClrSizLocLookUp)
                        Dim parentItem As GridDataItem = CType(editItem.OwnerTableView.ParentItem, GridDataItem)
                        Dim ColorFlg As String = parentItem.GetDataKeyValue("ColorFlg").ToString
                        Dim SwatchFlg As String = parentItem.GetDataKeyValue("SwatchFlg").ToString
                        Dim ISN As Decimal = CDec(editItem.GetDataKeyValue("ISN"))

                        '1. Friendly Color
                        Dim rtxtFriendlyClr As RadTextBox = DirectCast(editItem.FindControl("rtxtFriendlyColor"), RadTextBox)

                        If ColorFlg = "Y" And SwatchFlg = "N" Then
                            rtxtFriendlyClr.Enabled = False
                        End If

                        '2. Non Swatch Color
                        Dim cmbNonSwatchColor As RadComboBox = DirectCast(editItem.FindControl("rcbNonSwatchColor"), RadComboBox)
                        Dim hfNonSwatchClr As HiddenField = DirectCast(editItem.FindControl("hfNonSwatchColor"), HiddenField)

                        With cmbNonSwatchColor
                            .DataSource = _TUEcommPrioritization.GetNonSwatchClrLookUp()
                            .DataValueField = "Value"
                            .DataTextField = "Text"
                            .DataBind()
                            .Items.Insert(0, New RadComboBoxItem("", "0"))
                        End With

                        If cmbNonSwatchColor.FindItemByValue(hfNonSwatchClr.Value.Trim) IsNot Nothing Then
                            cmbNonSwatchColor.FindItemByValue(hfNonSwatchClr.Value.Trim).Selected = True
                        End If

                        If ColorFlg = "Y" And SwatchFlg = "Y" Then
                            cmbNonSwatchColor.Enabled = False
                        End If

                        '3. Color Family
                        ClrLookupVals = _TUEcommPrioritization.GetClrFamilyLookUp(ISN)

                        Dim cmbColorFamily As RadComboBox = DirectCast(editItem.FindControl("rcbColorFamily"), RadComboBox)
                        Dim hfColorFamily As HiddenField = DirectCast(editItem.FindControl("hfColorFamily"), HiddenField)

                        With cmbColorFamily
                            .DataSource = ClrLookupVals
                            .DataValueField = "Value"
                            .DataTextField = "Text"
                            .DataBind()
                        End With

                        If Not String.IsNullOrEmpty(hfColorFamily.Value.Trim) Then
                            For Each color As String In hfColorFamily.Value.Trim.Split(","c)
                                If cmbColorFamily.FindItemByText(color) IsNot Nothing Then
                                    cmbColorFamily.FindItemByText(color).Checked = True
                                End If
                            Next
                        End If

                        '4. Feature Render Swatch
                        Dim cmbFRS As RadComboBox = DirectCast(editItem.FindControl("rcbFRS"), RadComboBox)
                        Dim hdfFRS As HiddenField = DirectCast(editItem.FindControl("hfFRS"), HiddenField)

                        With cmbFRS
                            .DataSource = _TUTMS900PARAMETER.GetAllFeatureRenderSwatchValues
                            .DataValueField = "CharIndex"
                            .DataTextField = "LongDesc"
                            .DataBind()
                        End With

                        If cmbFRS.FindItemByText(hdfFRS.Value.Trim) IsNot Nothing Then
                            cmbFRS.FindItemByText(hdfFRS.Value.Trim).Selected = True
                        End If

                        ''
                        ''      SIZE LEVEL DATA 
                        ''
                    Case "grdThirdLevel"
                        Dim SzFlg As String = String.Empty
                        Dim parItem As GridDataItem = CType(editItem.OwnerTableView.ParentItem.OwnerTableView.ParentItem, GridDataItem)
                        SzFlg = parItem.GetDataKeyValue("SizeFlg").ToString

                        Dim SizeFamLookupVals As New List(Of ClrSizLocLookUp)
                        Dim ISN As Decimal = CDec(editItem.GetDataKeyValue("ISN"))
                        SizeFamLookupVals = _TUEcommPrioritization.GetSizFamilyLookUp(ISN)

                        ''1. Webcat Size
                        Dim cmbWebCatSize As RadComboBox = DirectCast(editItem.FindControl("rcbWebCatSize"), RadComboBox)
                        Dim hfWebCatSiz As HiddenField = DirectCast(editItem.FindControl("hfWebCatSize"), HiddenField)

                        'With cmbWebCatSize
                        '    .DataSource = _TUEcommPrioritization.GetWebcatSizeLookUp()
                        '    .DataTextField = "Text"
                        '    .DataValueField = "Value"
                        '    .DataBind()
                        '    .Items.Insert(0, New RadComboBoxItem("", "0"))
                        'End With

                        'If cmbWebCatSize.FindItemByText(hfWebCatSiz.Value.Trim) IsNot Nothing Then
                        '    cmbWebCatSize.FindItemByText(hfWebCatSiz.Value.Trim).Selected = True
                        'End If
                        cmbWebCatSize.Text = hfWebCatSiz.Value.Trim

                        If SzFlg = "N" Then
                            cmbWebCatSize.Enabled = False
                        End If

                        '2. Size Family
                        Dim cmbSizeFamily As RadComboBox = DirectCast(editItem.FindControl("rcbSizeFamily"), RadComboBox)
                        Dim hfSizeFamily As HiddenField = DirectCast(editItem.FindControl("hfSizeFamily"), HiddenField)

                        With cmbSizeFamily
                            .DataSource = SizeFamLookupVals
                            .DataValueField = "Value"
                            .DataTextField = "Text"
                            .DataBind()
                            .Items.Insert(0, New RadComboBoxItem("", "0"))
                        End With

                        If Not String.IsNullOrEmpty(hfSizeFamily.Value.Trim) Then
                            For Each size As String In hfSizeFamily.Value.Trim.Split(","c)
                                If cmbSizeFamily.FindItemByText(size) IsNot Nothing Then
                                    cmbSizeFamily.FindItemByText(size).Checked = True
                                End If
                            Next
                        End If

                End Select
            End If

        Catch ex As Exception
            RespondToError(System.Reflection.MethodBase.GetCurrentMethod.Name, ex.Message)
        End Try

    End Sub

    Private Sub grdeCommercePrioritization_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdeCommercePrioritization.NeedDataSource

        If e.RebindReason = GridRebindReason.InitialLoad Then Exit Sub

        If Not e.IsFromDetailTable Then
            grdeCommercePrioritization.Visible = True

            Dim gridSortString As String = grdeCommercePrioritization.MasterTableView.SortExpressions.GetSortString()

            'Clear all the sort expressions to Reset the sorting on the Grid to Default one.
            If gridSortString Is Nothing Then
                grdeCommercePrioritization.MasterTableView.SortExpressions.Clear()
            End If

            'Populate the Grid with the data.
            grdeCommercePrioritization.MasterTableView.DataSource = _TUEcommPrioritization.GetPrioritizationResults(eCommPrioritizationCtrl.SelectedEMMId, _
                                                                    eCommPrioritizationCtrl.SelectedCMGId, eCommPrioritizationCtrl.SelectedBuyerId, _
                                                                    eCommPrioritizationCtrl.SelectedLabelId, eCommPrioritizationCtrl.SelectedTUWeek, _
                                                                    eCommPrioritizationCtrl.SelectedImageId, eCommPrioritizationCtrl.SelectedVendStyId, _
                                                                    eCommPrioritizationCtrl.SelectedStatus) _
                                                                    .OrderByDescending(Function(x) x.OnHand) _
                                                                    .ThenByDescending(Function(y) y.OnOrder) _
                                                                    .ThenByDescending(Function(z) z.ImageShot).ToList
        End If

    End Sub

    Private Sub grdeCommercePrioritization_SortCommand(sender As Object, e As Telerik.Web.UI.GridSortCommandEventArgs) Handles grdeCommercePrioritization.SortCommand
        Dim sortExpr As New GridSortExpression()

        Select Case e.OldSortOrder
            Case GridSortOrder.None
                sortExpr.FieldName = e.SortExpression

                If e.CommandArgument.ToString = "OnOrder" Or e.CommandArgument.ToString = "OnHand" Then
                    sortExpr.SortOrder = GridSortOrder.Descending
                Else
                    sortExpr.SortOrder = GridSortOrder.Ascending
                End If

                Exit Select
            Case GridSortOrder.Ascending
                sortExpr.FieldName = e.SortExpression

                If e.CommandArgument.ToString = "OnOrder" Or e.CommandArgument.ToString = "OnHand" Then
                    sortExpr.SortOrder = CType(IIf(grdeCommercePrioritization.MasterTableView.AllowNaturalSort, GridSortOrder.None, GridSortOrder.Descending), GridSortOrder)
                Else
                    sortExpr.SortOrder = GridSortOrder.Descending
                End If

                Exit Select
            Case GridSortOrder.Descending
                sortExpr.FieldName = e.SortExpression

                If e.CommandArgument.ToString = "OnOrder" Or e.CommandArgument.ToString = "OnHand" Then
                    sortExpr.SortOrder = GridSortOrder.Ascending
                Else
                    sortExpr.SortOrder = CType(IIf(grdeCommercePrioritization.MasterTableView.AllowNaturalSort, GridSortOrder.None, GridSortOrder.Ascending), GridSortOrder)
                End If

                Exit Select
        End Select

        e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpr)

        e.Canceled = True
        grdeCommercePrioritization.Rebind()
    End Sub

    Private Sub grdeCommercePrioritization_DetailTableDataBind(ByVal source As Object, ByVal e As Telerik.Web.UI.GridDetailTableDataBindEventArgs) Handles grdeCommercePrioritization.DetailTableDataBind
        Dim PrioritizationRecord As GridDataItem = CType(e.DetailTableView.ParentItem, GridDataItem)

        Dim decISN As Decimal
        Dim chrStatus As Char
        Dim intMerchID As Integer = 0
        Dim decAdNum As Decimal = 0


        tblFloodColorOptions.Visible = True
        tblFloodSizeOptions.Visible = True

        Select Case e.DetailTableView.Name
            Case "grdSecondLevel" ' Color Level
                decISN = CDec(PrioritizationRecord.GetDataKeyValue("ISN"))
                chrStatus = CChar(PrioritizationRecord.GetDataKeyValue("StatusFlg"))
                decAdNum = CDec(PrioritizationRecord.GetDataKeyValue("AdNbr"))
                e.DetailTableView.DataSource = _TUEcommPrioritization.GetColorLevelResults(decISN, chrStatus, decAdNum).OrderBy(Function(x) x.FRS).ToList

            Case "grdThirdLevel" 'Size Level              
                decISN = CDec(PrioritizationRecord.GetDataKeyValue("ISN"))
                chrStatus = CChar(PrioritizationRecord.GetDataKeyValue("StatusFlg"))
                intMerchID = CInt(PrioritizationRecord.GetDataKeyValue("TurnInMerchID"))
                e.DetailTableView.DataSource = _TUEcommPrioritization.GetSizeLevelResults(decISN, intMerchID, chrStatus).Distinct.ToList()
        End Select

    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    '''  Used to move data from Uploaded to Pending. This is done if there were some corrections on WebCat and so, the user will have to
    ''' delete records on WebCat and resubmit the records.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PushDataToPending()
        If grdeCommercePrioritization.MasterTableView.GetSelectedItems.Length = 0 Then
            mpeCommercePrioritization.ErrorMessage = "Please select ISNs that you need to push to pending"
            Exit Sub
        End If
        Dim LogMessage As String = ""
        Dim log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
        LogMessage += " Reject : User " & SessionWrapper.UserID & " requested a rejection."

        Dim ImageIDInfos As New List(Of ImageInfo)
        Dim ISNs As New List(Of String)
        Dim Ads As New List(Of String)
        Dim chrStatus As Char
        For Each gridSelItem As GridDataItem In grdeCommercePrioritization.MasterTableView.GetSelectedItems
            'Get all the ISNs for the selected rows.
            ISNs.Add(CStr(gridSelItem.GetDataKeyValue("ISN")))
            chrStatus = CChar(gridSelItem.GetDataKeyValue("StatusFlg"))
            Ads.Add(CStr(gridSelItem.GetDataKeyValue("AdNbr")))
        Next

        ImageIDInfos = _TUEcommPrioritization.GetImageIDsWithoutAdminNotes(String.Join(",", ISNs), chrStatus, String.Join(",", Ads))

        Try
            If ImageIDInfos.Count > 0 Then
                For Each ImgIdInfo As ImageInfo In ImageIDInfos
                    LogMessage += vbCrLf + " Rejecting : MerchId " & CInt(ImgIdInfo.TurnInMerchId).ToString & "."
                    _TUEcommPrioritization.UpdateWebCatImportStatus(CInt(ImgIdInfo.TurnInMerchId), "P"c, SessionWrapper.UserID)
                Next
            End If

            grdeCommercePrioritization.Rebind()

            log.Warn(LogMessage)

            mpeCommercePrioritization.ErrorMessage = "Rejection to Pending successful. Have you deleted the records in WebCat ?"
        Catch ex As Exception
            mpeCommercePrioritization.ErrorMessage = "Rejection to Pending failed."
            log.Error(LogMessage & vbCrLf & ex.Message & vbCrLf & ex.StackTrace.ToString)
        End Try

    End Sub

    Private Sub SubmitData()
        Dim log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Dim intSuccessCount As Integer = 0
        Dim intFailureCount As Integer = 0
        Dim lstImageIDs As New List(Of Integer)
        Dim ImageIDInfos As New List(Of ImageInfo)
        Dim VendorStyle As String = ""
        Dim imageIDsKilledInAdmin As String = String.Empty
        Dim _TUECommTurnInMeetResults As New TUECommTurnInMeetResults
        Dim selRowCount As Integer = grdeCommercePrioritization.MasterTableView.GetSelectedItems.Count
        Dim inValidRowCount As Integer = grdeCommercePrioritization.MasterTableView.GetSelectedItems.Where(Function(x) CChar(x.GetDataKeyValue("IsValidFlg")) = "N"c).Count

        Try
            'AXB: Only selected records which are valid go through.
            If selRowCount > 0 And inValidRowCount = 0 Then

                imageIDsKilledInAdmin = GetImagesKilledInAdmin(grdeCommercePrioritization)
                If Not String.IsNullOrEmpty(imageIDsKilledInAdmin) Then
                    mpeCommercePrioritization.ErrorMessage = String.Format("There are images that are killed in Admin. Please Kill/Delete the following image(s) prior to submit. <br \>{0}", imageIDsKilledInAdmin)
                    Exit Sub
                End If

                'AXB: Check for Swatch flags on the selected records.
                Dim SwatchFlgBlankRecords As List(Of String)
                SwatchFlgBlankRecords = grdeCommercePrioritization.MasterTableView.GetSelectedItems.Where(Function(x) CChar(x.GetDataKeyValue("SwatchFlg")) <> "Y"c And CChar(x.GetDataKeyValue("SwatchFlg")) <> "N"c).Select(Function(y) y.GetDataKeyValue("SwatchFlg").ToString).ToList()

                If SwatchFlgBlankRecords.Count = 0 Then

                    Dim ISNs As New List(Of String)
                    Dim Ads As New List(Of String)
                    Dim UPC As New List(Of String)
                    Dim chrStatus As Char
                    For Each gridSelItem As GridDataItem In grdeCommercePrioritization.MasterTableView.GetSelectedItems
                        'Get all the ISNs for the selected rows.
                        ISNs.Add(CStr(gridSelItem.GetDataKeyValue("ISN")))
                        Ads.Add(CStr(gridSelItem.GetDataKeyValue("AdNbr")))
                        chrStatus = CChar(gridSelItem.GetDataKeyValue("StatusFlg"))

                        'Validate Color Family.
                        If gridSelItem.ChildItem.NestedTableViews.Count > 0 Then
                            For Each detailGridItem As GridDataItem In gridSelItem.ChildItem.NestedTableViews(0).Items
                                Dim lblColorFamily As Label = CType(detailGridItem.FindControl("lblColorFamily"), Label)
                                If gridSelItem.GetDataKeyValue("ColorFlg").ToString().ToUpper().Equals("Y") AndAlso
                                    Not lblColorFamily Is Nothing AndAlso
                                    String.IsNullOrEmpty(lblColorFamily.Text.Trim()) Then
                                    mpeCommercePrioritization.ErrorMessage = "Please select at least one Color from Color Family drop down."
                                    Exit Sub
                                End If
                            Next
                        End If
                    Next

                    'AXB: Get ImageIds based on ISNs (TU1064) and populate with Admin Image Notes (tu_ctlg_get_img_notes on SQL)
                    ImageIDInfos = _TUEcommPrioritization.GetImageIDs(String.Join(",", ISNs), chrStatus, String.Join(",", Ads), True)

                    'Updates the image group number in admin
                    _TUECommTurnInMeetResults.UpdateImageGroupNumber(ImageIDInfos, SessionWrapper.UserID)

                    If Not IsNothing(ImageIDInfos) Then
                        'AXB Step 1. Move the data from TurnIn tables to WebCat Staging tables.
                        Dim intUPCExistsCount As Integer = 0

                        Dim ErrorMessage As String = ""
                        For Each ImgIdInfo As ImageInfo In ImageIDInfos
                            Dim ImageID As Integer = ImgIdInfo.ImageId
                            Dim MerchID As Integer = ImgIdInfo.TurnInMerchId

                            'Check whether FeatureID already exists in WebCat tables.
                            Dim FeatureExistsOn As Integer = _TUEcommPrioritization.FeatureExistsOnReturnedProduct(ImageID)
                            If FeatureExistsOn > 0 Then
                                ErrorMessage += String.Format("Feature ({0}) exists on Webcat as Product ({1}).<br />" & vbCrLf, ImageID, FeatureExistsOn)
                            End If

                            'Check if UPCs exists in WebCat tables
                            Dim UPCOnWebCat As List(Of String) = _TUEcommPrioritization.GetExistingUPCOnWebCat(MerchID)
                            Dim UPCOnWebCatCount As Integer = UPCOnWebCat.Count
                            If UPCOnWebCatCount > 0 Then
                                ErrorMessage += String.Format("UPCs ({0}) already exist, they will not be uploaded to WebCat.<br />", String.Join(",", UPCOnWebCat), ImageID, MerchID)
                            End If

                            'Product and UPCs do not exist on WebCat
                            VendorStyle = ImgIdInfo.VendorStyle
                            _TUEcommPrioritization.SubmitToWebCatStage(ImgIdInfo.TurnInMerchId, ImgIdInfo.ImageNotes, SessionWrapper.UserID)


                            lstImageIDs.Add(ImageID)
                        Next

                        mpeCommercePrioritization.ErrorMessage = ErrorMessage
                        'Step 2. Assign Input parameters and Call the WebCatImport Web Service to notify Web Cat system.

                        Dim webCatImp As New WebCatImport.WebCatImportClient
                        Dim webCatImpRequest As New WebCatImport.RequestMessage

                        Dim totalImagesSentToWebCat As Integer = 0
                        If Not lstImageIDs.Contains(0) Then
                            totalImagesSentToWebCat = lstImageIDs.Count

                            'log.Warn("WebCatImport : Images sent " & totalImagesSentToWebCat.ToString)

                            webCatImpRequest.Images = lstImageIDs.Distinct.ToArray()
                            webCatImpRequest.EmailAddress = _TUEcommPrioritization.GetEmailAddress(SessionWrapper.UserID)

                            Dim webCatImpResponse As New WebCatImport.ResponseMessage
                            webCatImpResponse = webCatImp.ImportProductsUsingImageIDs(webCatImpRequest)

                            'Step 3. Update status in TTU600 table.
                            If webCatImpResponse.Status <> "" Then
                                UpdateWebCatStatus(ImageIDInfos, lstImageIDs, intSuccessCount, intFailureCount)
                            End If

                            'Nullify the session object so that the Hierarchical Grid items are collapsed to ISN level.
                            Me._prioritizationExpandedState = Nothing
                            Me.Session("_prioritizationExpandedState") = Nothing

                            grdeCommercePrioritization.Rebind()

                            ErrorMessage += String.Format("Submitted: {0}; Imported : {1} ; Failed: {2} ", totalImagesSentToWebCat.ToString, intSuccessCount.ToString, intFailureCount.ToString)

                        Else
                            ErrorMessage += "Error : One or more records contain an ImageId of 0."
                        End If

                        mpeCommercePrioritization.ErrorMessage = ErrorMessage
                    Else
                        mpeCommercePrioritization.ErrorMessage = "Error: Swatch Flag is a required field."
                    End If
                Else
                    mpeCommercePrioritization.ErrorMessage = "Error: None / Invalid record selected."
                End If
            End If
        Catch ex As Exception
            Dim Message As String = ""
            If ex.Message.Contains("timeout") Then
                UpdateWebCatStatus(ImageIDInfos, lstImageIDs, intSuccessCount, intFailureCount, True)
                Message = "The system is waiting for service to complete. Please check your email for a status. "
                RespondToError(System.Reflection.MethodBase.GetCurrentMethod.Name, Message)
            ElseIf ex.Message.Contains("ITU750") Then
                mpeCommercePrioritization.ErrorMessage = "VendorStyle : " & VendorStyle & " has duplicate UPCs in the submission. "
            ElseIf ex.Message.Contains("ITU751") Then
                mpeCommercePrioritization.ErrorMessage = "VendorStyle : " & VendorStyle & " has duplicate Sizes in the submission. "
            Else
                Message = "VendorStyle : " & VendorStyle & " has an error." & ex.Message
                RespondToError(System.Reflection.MethodBase.GetCurrentMethod.Name, Message)
            End If


        End Try
    End Sub


    Private Sub UpdateWebCatStatus(ByVal ServiceImageInformation As List(Of ImageInfo), ByVal distinctImageIDs As List(Of Integer), ByRef SuccessCount As Integer, ByRef FailureCount As Integer, Optional ByVal isInError As Boolean = False)
        If Not IsNothing(ServiceImageInformation) Then
            For Each processedImgIdInfo As ImageInfo In ServiceImageInformation
                If distinctImageIDs.Contains(processedImgIdInfo.ImageId) Or isInError Then
                    'Check whether the WebCatImport process successfully processed the Image or not.
                    If _TUEcommPrioritization.IsImageProcessedInWebCat(processedImgIdInfo.ImageId, processedImgIdInfo.TurnInMerchId) Then
                        _TUEcommPrioritization.UpdateWebCatImportStatus(processedImgIdInfo.TurnInMerchId, "U"c, SessionWrapper.UserID)
                        SuccessCount += 1
                    Else
                        _TUEcommPrioritization.UpdateWebCatImportStatus(processedImgIdInfo.TurnInMerchId, "F"c, SessionWrapper.UserID)
                        FailureCount += 1
                    End If
                End If
            Next
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
                    mpeCommercePrioritization.ErrorMessage = "Errors on Page."
                    validator.ErrorMessage = "Invalid Character Found."
                    args.IsValid = False
                End If
            End If
        Catch ex As Exception
            RespondToError(System.Reflection.MethodBase.GetCurrentMethod.Name, ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' This sub routine prepares data for export to excel and pdf.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ExportList(ByVal sender As Object, ByVal e As EventArgs)
        Dim export As New Export

        Session("eCommPrioritizationExportClicked") = "Y"

        'Export the selected rows only.
        If grdeCommercePrioritization.SelectedItems.Count > 0 Then
            For Each GrdDataItem As GridDataItem In grdeCommercePrioritization.MasterTableView.Items
                If Not GrdDataItem.Selected Then
                    GrdDataItem.Visible = False
                Else
                    GrdDataItem.Expanded = True
                    GrdDataItem.Visible = True
                    For Each childgridItem As GridDataItem In GrdDataItem.ChildItem.NestedTableViews(0).Items
                        childgridItem.Expanded = True
                        childgridItem.Visible = True
                    Next
                End If
            Next
        End If

        grdeCommercePrioritization.MasterTableView.Columns(0).Visible = False
        grdeCommercePrioritization.MasterTableView.DetailTables(0).Columns(0).Visible = False
        grdeCommercePrioritization.MasterTableView.DetailTables(0).DetailTables(0).Columns(0).Visible = False
        grdeCommercePrioritization.MasterTableView.Columns.FindByUniqueName("selColumn").Visible = False
        grdeCommercePrioritization.MasterTableView.Columns.FindByUniqueName("EditColumn").Visible = False
        grdeCommercePrioritization.MasterTableView.DetailTables(0).Columns.FindByUniqueName("selColumn").Visible = False
        grdeCommercePrioritization.MasterTableView.DetailTables(0).Columns.FindByUniqueName("EditColumn").Visible = False
        grdeCommercePrioritization.MasterTableView.DetailTables(0).Columns.FindByUniqueName("DeleteColumn").Visible = False
        grdeCommercePrioritization.MasterTableView.DetailTables(0).DetailTables(0).Columns.FindByUniqueName("EditColumn").Visible = False
        grdeCommercePrioritization.ExportSettings.ExportOnlyData = True
        grdeCommercePrioritization.ExportSettings.OpenInNewWindow = True
        grdeCommercePrioritization.MasterTableView.ExportToExcel()

        If Me.tuModalExport.ExportText = "PDF" Then
            grdeCommercePrioritization.ExportSettings.Pdf.PageHeight = Unit.Parse("200mm")
            grdeCommercePrioritization.ExportSettings.Pdf.PageWidth = Unit.Parse("250mm")
            grdeCommercePrioritization.ExportSettings.Pdf.PageLeftMargin = Unit.Parse("5mm")
            grdeCommercePrioritization.ExportSettings.Pdf.PageRightMargin = Unit.Parse("5mm")
            grdeCommercePrioritization.ExportSettings.Pdf.PageTopMargin = Unit.Parse("5mm")
            grdeCommercePrioritization.ExportSettings.Pdf.PageBottomMargin = Unit.Parse("5mm")
            grdeCommercePrioritization.MasterTableView.ExportToPdf()
        ElseIf Me.tuModalExport.ExportText = "Excel" Then
            grdeCommercePrioritization.MasterTableView.ExportToExcel()
        End If

    End Sub

#Region "Flood Methods"

    ''' <summary>
    ''' Handles the FLOOD option at ISN level.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnFlood_Click(sender As Object, e As System.EventArgs) Handles btnFlood.Click
        Dim selRowCount As Integer
        Dim isSwatchFlgValid As Boolean = True

        Try
            'Exit the Sub-routine if no data is entered for "Flood" option.
            If String.IsNullOrEmpty(rcbFloodSwatchFlg.SelectedValue) And _
                    String.IsNullOrEmpty(rcbFloodColorFlg.SelectedValue) And _
                    String.IsNullOrEmpty(rcbFloodSizeFlg.SelectedValue) And _
                    String.IsNullOrEmpty(rtxtFloodWebCategories.Text.Trim) And _
                    String.IsNullOrEmpty(rcbFloodLabel.SelectedValue) And _
                    String.IsNullOrEmpty(rcbFloodBrand.SelectedValue) And _
                    String.IsNullOrEmpty(rcbFloodAge.SelectedValue) And _
                    String.IsNullOrEmpty(rcbFloodGender.SelectedValue) Then
                mpeCommercePrioritization.ErrorMessage = "Enter value for at least one Flood option at ISN level."
                Exit Sub
            End If

            selRowCount = grdeCommercePrioritization.MasterTableView.GetSelectedItems.Count

            If selRowCount = 0 Then
                mpeCommercePrioritization.ErrorMessage = "Error: Select at least one record to Flood ISN Level data."
                Exit Sub
            End If

            'Check whether the Swatch Flag is being set to "Y" for the products with a Color Flag value of "N".
            If CChar(rcbFloodSwatchFlg.SelectedValue) = "Y"c And String.IsNullOrEmpty(rcbFloodColorFlg.SelectedValue) Then
                For Each item As GridDataItem In grdeCommercePrioritization.MasterTableView.GetSelectedItems
                    If CChar(item.GetDataKeyValue("ColorFlg")) = "N"c Then
                        isSwatchFlgValid = False
                        mpeCommercePrioritization.ErrorMessage = "Error: Swatch Flag cannot be ""Y"" for the products with a Color Flag value of ""N""."
                        Exit For
                    End If
                Next
            End If

            If Not String.IsNullOrEmpty(rcbFloodLabel.SelectedValue) AndAlso CInt(rcbFloodLabel.SelectedValue) = 0 Then
                mpeCommercePrioritization.ErrorMessage = "Please select a value for Label."
                Exit Sub
            End If

            If Not String.IsNullOrEmpty(rcbFloodBrand.SelectedValue) AndAlso CInt(rcbFloodBrand.SelectedValue) = 0 Then
                mpeCommercePrioritization.ErrorMessage = "Please select a value for Web Cat Brand."
                Exit Sub
            End If

            Dim tooManyCategories As Boolean = False
            For Each item As GridDataItem In grdeCommercePrioritization.MasterTableView.GetSelectedItems()
                Dim categories As String = item.GetDataKeyValue("WebCatgyList").ToString()
                If categories.Split(","c).Count() > 4 Then
                    tooManyCategories = True
                    mpeCommercePrioritization.ErrorMessage = "Error: Some items already have a maximum of 6 allowable web categories."
                    Exit For
                End If
            Next

            If isSwatchFlgValid And Not tooManyCategories Then
                'Clear the Edit mode, if any row is left in Edit mode.
                grdeCommercePrioritization.MasterTableView.ClearEditItems()

                Dim ISNData As New ECommPrioritizationInfo()
                Dim ISNs As New List(Of String)

                ISNData.SwatchFlg = CChar(rcbFloodSwatchFlg.SelectedValue)
                ISNData.ColorFlg = CChar(rcbFloodColorFlg.SelectedValue)

                If ISNData.ColorFlg = "N" Then
                    ISNData.SwatchFlg = "N"c 'Set the product as Non-Swatchable if Color Flag is "N".
                End If

                ISNData.SizeFlg = CChar(rcbFloodSizeFlg.SelectedValue)
                ISNData.WebCatgyCde = If(String.IsNullOrEmpty(rtxtFloodWebCategories.Text.Trim), 0, CInt(hfFloodWebCatCde.Value.Trim))
                ISNData.LabelID = If(String.IsNullOrEmpty(rcbFloodLabel.SelectedValue), 0, CInt(rcbFloodLabel.SelectedValue))
                ISNData.BrandID = If(String.IsNullOrEmpty(rcbFloodBrand.SelectedValue), CShort(0), CShort(rcbFloodBrand.SelectedValue))
                ISNData.AgeCde = If(String.IsNullOrEmpty(rcbFloodAge.SelectedValue), CShort(0), CShort(rcbFloodAge.SelectedValue))
                ISNData.GenderCde = If(String.IsNullOrEmpty(rcbFloodGender.SelectedValue), CShort(0), CShort(rcbFloodGender.SelectedValue))

                For Each gridSelItem As GridDataItem In grdeCommercePrioritization.MasterTableView.GetSelectedItems
                    'Get all the ISNs for the selected rows.
                    ISNs.Add(CStr(gridSelItem.GetDataKeyValue("ISN")))
                Next

                'Update the data in the database.
                _TUEcommPrioritization.UpdateISNLevelDataFlood(String.Join(",", ISNs), ISNData, SessionWrapper.UserID)

                'grdeCommercePrioritization.Rebind()
                Dim tableView As GridTableView = DirectCast(grdeCommercePrioritization.MasterTableView.Items(0).OwnerTableView, GridTableView)
                tableView.Rebind()

                'Reset the Flood values.
                ResetFloodISN()

                mpeCommercePrioritization.ErrorMessage = CStr(selRowCount) & If(selRowCount = 1, " Row ", " Rows ") & "Saved Successfully."
            End If
        Catch ex As Exception
            RespondToError(System.Reflection.MethodBase.GetCurrentMethod.Name, ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Handles the FLOOD option at Color level.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnFloodClrLvl_Click(sender As Object, e As System.EventArgs) Handles btnFloodClrLvl.Click
        Dim selRowCount As Integer = 0

        Try
            Page.Validate("FloodUpdate")

            If Not IsValid Then
                mpeCommercePrioritization.ErrorMessage = "Errors on Page."
                Exit Sub
            End If

            'Exit the Sub-routine if no data is entered for "Flood" option.
            If String.IsNullOrEmpty(rtxtFloodFeatureId.Text.Trim) And _
                    String.IsNullOrEmpty(rtxtFloodProductName.Text.Trim) And _
                    String.IsNullOrEmpty(rtxtFloodImageGroup.Text.Trim) Then
                mpeCommercePrioritization.ErrorMessage = "Enter value for at least one Flood option at Color level."
                Exit Sub
            End If

            For Each ISNLvlDataItem As GridDataItem In grdeCommercePrioritization.MasterTableView.Items
                If ISNLvlDataItem.Expanded Then
                    selRowCount = ISNLvlDataItem.ChildItem.NestedTableViews(0).GetSelectedItems.Count

                    If selRowCount > 0 Then
                        Exit For
                    End If
                End If
            Next

            If selRowCount > 0 Then
                'Clear the Edit mode, if any row is left in Edit mode.
                grdeCommercePrioritization.MasterTableView.ClearEditItems()

                Dim ColorData As New ECommPrioritizationInfo()
                Dim TurnInMerchIDs As New List(Of String)

                ColorData.FeatureID = If(String.IsNullOrEmpty(rtxtFloodFeatureId.Text.Trim), 0, CInt(rtxtFloodFeatureId.Text.Trim))
                ColorData.ProductName = CStr(rtxtFloodProductName.Text.Trim)
                'ColorData.ProdCopyLongDesc = String.Empty
                ColorData.ImageGroup = CShort(If(String.IsNullOrEmpty(rtxtFloodImageGroup.Text.Trim), 0, CInt(rtxtFloodImageGroup.Text.Trim)))

                Dim rcbFRS As RadComboBox
                Dim imageID As Integer = 0
                Dim frs As String = ""
                Dim bUpdate As Boolean = True
                For Each ISNLevelDataItem As GridDataItem In grdeCommercePrioritization.MasterTableView.Items
                    If ISNLevelDataItem.Expanded Then
                        For Each ColorLevelGridItem As GridDataItem In ISNLevelDataItem.ChildItem.NestedTableViews(0).GetSelectedItems
                            'Get all the Merch IDs for the selected rows.
                            TurnInMerchIDs.Add(CStr(ColorLevelGridItem.GetDataKeyValue("TurnInMerchID")))
                            'Find rows where the imageID matches the flooding featureID.
                            imageID = CType(ColorLevelGridItem.GetDataKeyValue("ImageID"), Integer)
                            rcbFRS = CType(ColorLevelGridItem.FindControl("rcbFRS"), RadComboBox)
                            frs = ColorLevelGridItem.GetDataKeyValue("FRS").ToString
                            If ColorData.FeatureID = imageID AndAlso (frs = "Swatch" OrElse frs = "Render" OrElse frs = "Static Swatch Box") Then
                                bUpdate = False
                                Exit For
                            End If
                        Next
                        If Not bUpdate Then
                            Exit For
                        End If
                    End If
                Next

                If bUpdate Then

                    'Update the data in the database.
                    _TUEcommPrioritization.UpdateColorLevelDataFlood(String.Join(",", TurnInMerchIDs), ColorData, SessionWrapper.UserID)

                    'Dim tableView As GridTableView = DirectCast(grdeCommercePrioritization.MasterTableView.Items(0).ChildItem.NestedTableViews(0), GridTableView)
                    'tableView.Rebind()
                    grdeCommercePrioritization.Rebind()
                    'Reset the Flood values.
                    rtxtFloodFeatureId.Text = ""
                    rtxtFloodProductName.Text = ""
                    rtxtFloodImageGroup.Text = ""

                    selRowCount = TurnInMerchIDs.Count 'Get the count of selected Color level records.

                    mpeCommercePrioritization.ErrorMessage = CStr(selRowCount) & If(selRowCount = 1, " Row ", " Rows ") & "Saved Successfully."

                Else
                    mpeCommercePrioritization.ErrorMessage = "Feature ID must be different than Swatch ID."
                End If
            Else
                mpeCommercePrioritization.ErrorMessage = "Select at least one record to Flood Color Level data."
            End If
        Catch ex As Exception
            RespondToError(System.Reflection.MethodBase.GetCurrentMethod.Name, ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Handles the FLOOD option at Size level.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnFloodSizLvl_Click(sender As Object, e As System.EventArgs) Handles btnFloodSizLvl.Click
        Try
            'Exit the Sub-routine if no data is entered for "Flood" option.
            If String.IsNullOrEmpty(rtxtFloodFindWebCatSize.Text.Trim) And _
                    String.IsNullOrEmpty(rtxtFloodReplaceWebCatSize.Text.Trim) And _
                    String.IsNullOrEmpty(rtxtFloodFindSizeFam.Text.Trim) And _
                    String.IsNullOrEmpty(rtxtFloodReplaceSizeFam.Text.Trim) And _
                    String.IsNullOrEmpty(rtxtFloodFindVendorSize.Text.Trim) And _
                    String.IsNullOrEmpty(rtxtFloodVendorWCSize.Text.Trim) And _
                    rcbFloodVendorWCSizeFamily.CheckedItems.Count = 0 Then
                mpeCommercePrioritization.ErrorMessage = "Enter value for at least one Flood option at Size level."
                Exit Sub
            End If

            If (String.IsNullOrEmpty(rtxtFloodFindWebCatSize.Text.Trim) And _
                    Not String.IsNullOrEmpty(rtxtFloodReplaceWebCatSize.Text.Trim)) Or _
                (String.IsNullOrEmpty(rtxtFloodReplaceWebCatSize.Text.Trim) And _
                    Not String.IsNullOrEmpty(rtxtFloodFindWebCatSize.Text.Trim)) Then
                mpeCommercePrioritization.ErrorMessage = "Web Cat Size: Find and Replace fields should be Non-blank."
                Exit Sub
            End If

            If (String.IsNullOrEmpty(rtxtFloodFindSizeFam.Text.Trim) And _
                    Not String.IsNullOrEmpty(rtxtFloodReplaceSizeFam.Text.Trim)) Or _
                (String.IsNullOrEmpty(rtxtFloodReplaceSizeFam.Text.Trim) And _
                    Not String.IsNullOrEmpty(rtxtFloodFindSizeFam.Text.Trim)) Then
                mpeCommercePrioritization.ErrorMessage = "Size Family: Find and Replace fields should be Non-blank."
                Exit Sub
            End If

            If (String.IsNullOrEmpty(rtxtFloodFindVendorSize.Text.Trim) And _
                    Not String.IsNullOrEmpty(rtxtFloodVendorWCSize.Text.Trim) And _
                    rcbFloodVendorWCSizeFamily.CheckedItems.Count = 0) Then
                mpeCommercePrioritization.ErrorMessage = "Vendor Size Replace: Find and Replace fields should be Non-blank."
                Exit Sub
            End If

            'Clear the Edit mode, if any row is left in Edit mode.
            grdeCommercePrioritization.MasterTableView.ClearEditItems()

            Dim TurnInMerchIDs As New List(Of String)
            For Each ISNLevelDataItem As GridDataItem In grdeCommercePrioritization.MasterTableView.Items
                If ISNLevelDataItem.Expanded Then
                    For Each ColorLevelGridItem As GridDataItem In ISNLevelDataItem.ChildItem.NestedTableViews(0).GetSelectedItems
                        'Get all the Merch IDs for the selected rows.
                        TurnInMerchIDs.Add(CStr(ColorLevelGridItem.GetDataKeyValue("TurnInMerchID")))
                    Next
                End If
            Next
            'Update the data in the database.
            If TurnInMerchIDs.Count > 0 Then

                _TUEcommPrioritization.UpdateSizeLevelDataFlood(rtxtFloodFindWebCatSize.Text.Trim, _
                                                           rtxtFloodReplaceWebCatSize.Text.Trim, _
                                                           rtxtFloodFindSizeFam.Text.Trim, _
                                                           rtxtFloodReplaceSizeFam.Text.Trim, _
                                                           rtxtFloodFindVendorSize.Text.Trim.Replace(" ", ""), _
                                                           rtxtFloodVendorWCSize.Text.Trim, _
                                                           String.Join(",", rcbFloodVendorWCSizeFamily.CheckedItems.ToList.Select(Function(x) x.Value).ToList), _
                                                           SessionWrapper.UserID, _
                                                           String.Join(",", TurnInMerchIDs))



                'For Each ISNLevelDataItem As GridDataItem In grdeCommercePrioritization.MasterTableView.Items
                '    If ISNLevelDataItem.Expanded Then
                '        'Rebind Color Level views
                '        DirectCast(ISNLevelDataItem.ChildItem.NestedTableViews(0), GridTableView).Rebind()
                '        ISNLevelDataItem.Expanded = True
                '        Dim ISNIndex As Integer = ISNLevelDataItem.ItemIndex
                '        For Each ColorLevelGridItem As GridDataItem In ISNLevelDataItem.ChildItem.NestedTableViews(0).GetSelectedItems
                '            Dim tableView As GridTableView = DirectCast(ColorLevelGridItem.ChildItem.NestedTableViews(0), GridTableView)
                '            tableView.Rebind()
                '        Next
                '    End If
                'Next

                grdeCommercePrioritization.Rebind()


                'Reset the Flood values.
                rtxtFloodFindWebCatSize.Text = ""
                rtxtFloodReplaceWebCatSize.Text = ""
                rtxtFloodFindSizeFam.Text = ""
                rtxtFloodReplaceSizeFam.Text = ""
                rtxtFloodFindVendorSize.Text = ""
                rtxtFloodVendorWCSize.Text = ""
                rcbFloodVendorWCSizeFamily.ClearSelection()
                rcbFloodVendorWCSizeFamily.Items.Clear()
                rcbFloodVendorWCSizeFamily.Text = ""
                mpeCommercePrioritization.ErrorMessage = "Data saved Successfully."

            Else
                mpeCommercePrioritization.ErrorMessage = "Please select the rows that need to be flooded."
            End If

        Catch ex As Exception
            RespondToError(System.Reflection.MethodBase.GetCurrentMethod.Name, ex.Message)
        End Try
    End Sub

    'Clear all the values used for Flood.
    Private Sub ResetFloodISN()
        rtxtFloodWebCategories.Text = ""

        rcbFloodSwatchFlg.ClearSelection()
        rcbFloodSwatchFlg.Text = ""

        rcbFloodColorFlg.ClearSelection()
        rcbFloodColorFlg.Text = ""

        rcbFloodSizeFlg.ClearSelection()
        rcbFloodSizeFlg.Text = ""

        rcbFloodLabel.ClearSelection()
        rcbFloodLabel.Text = ""

        rcbFloodBrand.ClearSelection()
        rcbFloodBrand.Text = ""

        'rcbFloodDropShipID.ClearSelection()
        'rcbFloodDropShipID.Text = ""

        'rcbFloodIntRetInstrct.ClearSelection()
        'rcbFloodIntRetInstrct.Text = ""

        'rcbFloodExtRetInstrct.ClearSelection()
        'rcbFloodExtRetInstrct.Text = ""

        rcbFloodAge.ClearSelection()
        rcbFloodAge.Text = ""

        rcbFloodGender.ClearSelection()
        rcbFloodGender.Text = ""
    End Sub

    Protected Sub rcbFloodLabel_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        With rcbFloodLabel
            .DataSource = _TULabel.GetAllLabels()
            .DataTextField = "LabelDesc"
            .DataValueField = "LabelId"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem("", "0"))
        End With
    End Sub

    Protected Sub rcbFloodBrand_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        With rcbFloodBrand
            .DataSource = _TUBrand.GetAllBrands()
            .DataTextField = "BrandDesc"
            .DataValueField = "BrandId"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem("", "0"))
        End With
    End Sub

    Protected Sub rcbFloodAge_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        With rcbFloodAge
            .DataSource = _TU998Parm.GetAllAgeCodes()
            .DataTextField = "ParmText"
            .DataValueField = "ParmValue"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem("", "0"))
        End With
    End Sub

    Protected Sub rcbFloodGender_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        With rcbFloodGender
            .DataSource = _TU998Parm.GetAllGenderCodes()
            .DataTextField = "ParmText"
            .DataValueField = "ParmValue"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem("", "0"))
        End With
    End Sub


    Protected Sub LoadrcbFloodVendorWCSizeFamily()
        If rcbFloodVendorWCSizeFamily.Items.Count = 0 Then
            With rcbFloodVendorWCSizeFamily
                If .Items.Count = 0 Then
                    .DataSource = _TUEcommPrioritization.GetAllSizeFamilyLookUp
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataBind()
                End If
            End With
        End If
    End Sub
#End Region

#End Region


    Protected Sub rcbWebCatSize_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        Dim FilteredSizes As List(Of ClrSizLocLookUp) = Sizes.Where(Function(x) x.Text.StartsWith(e.Text, StringComparison.OrdinalIgnoreCase)).ToList

        Try
            Dim ItemsPerREquest As Integer = 10
            Dim itemOffset As Integer = e.NumberOfItems
            Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerREquest, FilteredSizes.Count)
            e.EndOfItems = endOffset = FilteredSizes.Count

            Dim result As New List(Of RadComboBoxItemData)(endOffset - itemOffset)

            For i As Integer = itemOffset To endOffset
                Dim itemData As New RadComboBoxItemData()
                itemData.Text = FilteredSizes.Item(i).Text.ToString()
                itemData.Value = FilteredSizes.Item(i).Value.ToString()
                result.Add(itemData)
            Next

            With CType(sender, RadComboBox)
                .DataSource = result
                .DataValueField = "Value"
                .DataTextField = "Text"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem("", "0"))
                If e.Text = CType(CType(sender, RadComboBox).Parent.FindControl("hfWebCatSize"), HiddenField).Value Then
                    .Text = e.Text
                End If
            End With

            e.Message = GetStatusMessage(endOffset, FilteredSizes.Count)

        Catch ex As Exception
            e.Message = "No matches"
        End Try
    End Sub

    Private Shared Function GetStatusMessage(ByVal offset As Integer, ByVal total As Integer) As String
        If total <= 0 Then
            Return "No matches"
        End If

        Return [String].Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total)
    End Function

    Private Sub RespondToError(ByVal MethodName As String, ByVal Message As String)
        Session("ErrorMsg") = "Error in Method: " & MethodName & "<br/>Error Message: " & Message
        Response.Redirect("~/Error.aspx", False)
    End Sub

    '#Region " Prioritization Grid Expansion "



    'Save/load expanded states Hash from the session
    'this can also be implemented in the ViewState
    Private ReadOnly Property ExpandedStates() As Hashtable
        Get
            If Me._prioritizationExpandedState Is Nothing Then
                _prioritizationExpandedState = TryCast(Me.Session("_prioritizationExpandedState"), Hashtable)
                If _prioritizationExpandedState Is Nothing Then
                    _prioritizationExpandedState = New Hashtable()
                    Me.Session("_prioritizationExpandedState") = _prioritizationExpandedState
                End If
            End If

            Return Me._prioritizationExpandedState
        End Get
    End Property

    Private Sub grdeCommercePrioritization_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles grdeCommercePrioritization.ItemCommand
        'save the expanded state in the session
        If e.CommandName = RadGrid.ExpandCollapseCommandName Then
            Dim item As GridDataItem = CType(e.Item, GridDataItem)
            Dim key As String = String.Empty

            If item.OwnerTableView.Name() = "grdFirstLevel" Then

                key = CStr(item.GetDataKeyValue("ISN"))

            ElseIf item.OwnerTableView.Name() = "grdSecondLevel" Then

                Dim isn As String = CStr(item.GetDataKeyValue("ISN"))
                Dim imageID As String = CStr(item.GetDataKeyValue("ImageID"))
                key = isn & "_" & imageID
            End If

            If (Not String.IsNullOrWhiteSpace(key)) Then

                If Not e.Item.Expanded Then
                    'Save its unique index among all the items in the hierarchy
                    Me.ExpandedStates(key) = True
                Else
                    'collapsed
                    Me.ExpandedStates.Remove(key)
                    Me.ClearExpandedChildren(key)
                End If
            End If

        End If

    End Sub

    'Clear the state for all expanded children if a parent item is collapsed
    Private Sub ClearExpandedChildren(parentHierarchicalIndex As String)
        Dim indexes As String() = New String(Me.ExpandedStates.Keys.Count - 1) {}
        Me.ExpandedStates.Keys.CopyTo(indexes, 0)

        For Each index As String In indexes
            'all indexes of child items
            If index.StartsWith(parentHierarchicalIndex & Convert.ToString("_")) OrElse index.StartsWith(parentHierarchicalIndex & Convert.ToString(":")) Then
                Me.ExpandedStates.Remove(index)
            End If
        Next
    End Sub

    Private Sub ExpandGridHierarchyLevel(ByVal blIsExpanded As Boolean)
        Dim ItemsAreSelected As Boolean = (grdeCommercePrioritization.MasterTableView.GetSelectedItems.Count > 0)

        If ItemsAreSelected Then
            For Each masterGridItem As GridDataItem In grdeCommercePrioritization.MasterTableView.GetSelectedItems()
                ProcessDataItem(masterGridItem, blIsExpanded)
            Next
        Else
            For Each masterGridItem As GridDataItem In grdeCommercePrioritization.MasterTableView.Items
                ProcessDataItem(masterGridItem, blIsExpanded)
            Next
        End If

    End Sub

    Private Sub ProcessDataItem(ByVal masterGridItem As GridDataItem, ByVal blIsExpanded As Boolean)
        masterGridItem.Expanded = blIsExpanded

        If masterGridItem.OwnerTableView.Name() = "grdFirstLevel" Then

            Dim isnKey As String = CStr(masterGridItem.GetDataKeyValue("ISN"))
            If blIsExpanded Then

                Me.ExpandedStates(isnKey) = True

                For Each detailGridItem01 As GridDataItem In masterGridItem.ChildItem.NestedTableViews(0).Items
                    detailGridItem01.Expanded = blIsExpanded

                    Dim imageID As String = CStr(detailGridItem01.GetDataKeyValue("ImageID"))
                    Dim imageKey As String = isnKey & "_" & imageID

                    If blIsExpanded Then
                        Me.ExpandedStates(imageKey) = True
                    Else
                        'collapsed
                        Me.ExpandedStates.Remove(imageKey)
                        Me.ClearExpandedChildren(imageKey)
                    End If
                Next
            Else
                Me.ExpandedStates.Remove(isnKey)
                Me.ClearExpandedChildren(isnKey)
            End If

        End If

    End Sub

    Private Sub ChkUnchkHierarchyLevel(ByVal blIsChecked As Boolean)
        If grdeCommercePrioritization.MasterTableView.GetSelectedItems.Count = 0 Then
            'Check all the records in Levels 1 and 2.
            For Each masterDataItem As GridDataItem In grdeCommercePrioritization.MasterTableView.Items
                masterDataItem.Selected = blIsChecked
                If masterDataItem.Expanded Then
                    For Each detailGridItem01 As GridDataItem In masterDataItem.ChildItem.NestedTableViews(0).Items
                        detailGridItem01.Selected = blIsChecked
                    Next
                End If
            Next
        End If
    End Sub
    ''' <summary>
    ''' Iterates through selected items in the collection and checks if there are any items that are killed in Admin. 
    ''' If any item that was killed in Admin, then it returns the image id otherwise empty string
    ''' </summary>
    ''' <param name="grid">Grid for which image id needs be validated</param>
    ''' <returns>Images that are killed in Admin</returns>
    ''' <remarks></remarks>
    Private Function GetImagesKilledInAdmin(ByRef grid As RadGrid) As String
        Dim imageIDs As List(Of String) = New List(Of String)()
        Dim keepItemExpanded As Boolean = False
        For Each item As GridDataItem In grdeCommercePrioritization.MasterTableView.GetSelectedItems
            If item.OwnerTableView.Name = "grdFirstLevel" Then
                'ISN Level row should be expanded if an image under the ISN was killed in Admin, otherwise don't do anything
                keepItemExpanded = item.Expanded
                'ISN Level data is expanded so that Image level values can be read 
                item.Expanded = CBool(IIf(item.Expanded, item.Expanded, True))
                For Each detailGridItem As GridDataItem In item.ChildItem.NestedTableViews(0).Items
                    If detailGridItem("ImageSuffix").Text.Trim() <> String.Empty AndAlso _
                                    detailGridItem("ImageSuffix").Text.Trim() = "Y" AndAlso _
                                    Not imageIDs.Contains(CStr(detailGridItem.GetDataKeyValue("ImageID"))) Then
                        keepItemExpanded = True
                        imageIDs.Add(CStr(detailGridItem.GetDataKeyValue("ImageID")))
                    End If
                Next
                item.Expanded = keepItemExpanded
            End If
        Next

        If imageIDs.Count > 0 Then
            Return String.Join(",", imageIDs)
        Else
            Return String.Empty
        End If

    End Function

    'Private Sub RadUPCGrid_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadUPCGrid.NeedDataSource
    '    mpeCommercePrioritization.ErrorMessage = "  Getting Data"

    '    If e.RebindReason = GridRebindReason.InitialLoad Then Exit Sub

    '    If Not e.IsFromDetailTable Then
    '        RadUPCGrid.Visible = True

    '        Dim gridSortString As String = grdeCommercePrioritization.MasterTableView.SortExpressions.GetSortString()

    '        'Clear all the sort expressions to Reset the sorting on the Grid to Default one.
    '        If gridSortString Is Nothing Then
    '            RadUPCGrid.MasterTableView.SortExpressions.Clear()
    '        End If

    '        'Populate the Grid with the data.
    '        RadUPCGrid.MasterTableView.DataSource = _TUEcommPrioritization.GetPrioritizationReport(eCommPrioritizationCtrl.SelectedTUWeek) 
    '    End If
    'End Sub

    'Protected Sub lbUPCReport_Click(sender As Object, e As EventArgs) Handles lbUPCReport.Click
    '    mpeCommercePrioritization.ErrorMessage = "Report Loaded"
    '    RadUPCGrid.ExportSettings.IgnorePaging = True
    '    RadUPCGrid.ExportSettings.ExportOnlyData = True
    '    RadUPCGrid.ExportSettings.OpenInNewWindow = True
    '    RadUPCGrid.MasterTableView.ExportToExcel()
    '    RadUPCGrid.Visible = True
    'End Sub
End Class



