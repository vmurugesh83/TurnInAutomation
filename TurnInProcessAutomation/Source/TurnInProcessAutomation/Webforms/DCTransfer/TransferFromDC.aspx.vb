Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BLL.MiscConstants
Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL.Enumerations
Imports BonTon.Common.ExportFunctions
Imports Telerik.Web

Public Class TransferFromDC
    Inherits PageBase

    Private _DCTRansferSearchCtrl As DCTransferSearchControl = Nothing
    Dim _TUAdInfo As New TUAdInfo
    Dim _TUCtlgAdPg As New TUCtlgAdPg

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

    Public ReadOnly Property DCTransferSearchCtrl() As DCTransferSearchControl
        Get
            Return _DCTRansferSearchCtrl
        End Get
    End Property

#End Region

#Region "Events"

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim control As Control = LoadControl("~/WebUserControls/DCTransfer/DCTransferSearchControl.ascx")
        If Not control Is Nothing Then
            Me.Master.SideBarPlaceHolder.Controls.Add(control)
        End If
        If TypeOf control Is DCTransferSearchControl Then
            Me._DCTRansferSearchCtrl = CType(control, DCTransferSearchControl)
        ElseIf TypeOf control Is PartialCachingControl And CType(control, PartialCachingControl).CachedControl IsNot Nothing Then
            Me._DCTRansferSearchCtrl = CType(CType(control, PartialCachingControl).CachedControl, DCTransferSearchControl)
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.Master.SideBar.Width = 250
            CreateBatchPanel.Visible = False

            If Request.QueryString("BatchId") IsNot Nothing Then
                mpeTransferFromDC.PopUpMessage = "New batch " & Request.QueryString("BatchId").ToString & " created and file has been sent to MIO."
            End If

        End If

        If IsNothing(Session("TransferFromDC.TransferLocations")) Then
            Dim TULoc As TULocation = New TULocation
            Dim TransferLocations As IList(Of LocationInfo)
            TransferLocations = TULoc.GetAll()
            Session("TransferFromDC.TransferLocations") = TransferLocations
        End If

    End Sub

    Private Sub rtbeTransferFromDC_ButtonClick(sender As Object, e As Telerik.Web.UI.RadToolBarEventArgs) Handles rtbeTransferFromDC.ButtonClick
        Try
            If TypeOf e.Item Is RadToolBarButton Then
                Dim radToolBarButton As RadToolBarButton = DirectCast(e.Item, RadToolBarButton)
                SetEditMode(False)
                Select Case radToolBarButton.CommandName
                    Case "Back"
                        Response.Redirect(PreviousPageUrl, False)
                    Case "Retrieve"
                        RadUPCGrid.Visible = True
                        RadUPCGrid.Rebind()
                        HideUnwantedColumnsOnEdit(True)
                        rtbeTransferFromDC.FindItemByText("Export").Enabled = True
                        rtbeTransferFromDC.FindItemByText("Create Batch").Visible = True
                        rtbeTransferFromDC.FindItemByText("Create Batch").Enabled = True
                        CreateBatchPanel.Visible = False
                    Case "Reset"
                        Response.Redirect(Request.Url.ToString, False)
                    Case "Export"
                        _isExportClicked = True
                        ExportToExcel()
                    Case "EditAll"
                        PutRowsInEditMode(RadUPCGrid, True)
                    Case "CancelAll"
                        PutRowsInEditMode(RadUPCGrid, False)
                    Case "SaveAll"
                        SaveRows(RadUPCGrid)
                        PutRowsInEditMode(RadUPCGrid, False)
                    Case "CreateBatch"
                        If RadUPCGrid.SelectedItems.Count <> 0 Then
                            rtbeTransferFromDC.FindItemByText("Create Batch").Enabled = False
                            ShowAdDetailPanel()
                        Else
                            mpeTransferFromDC.ErrorMessage = " Please select items to create a batch."
                            Exit Sub
                        End If
                End Select
            End If
        Catch ex As Exception
            RespondToError(System.Reflection.MethodBase.GetCurrentMethod.Name, ex.Message)
        End Try
    End Sub

    Private Sub ShowAdDetailPanel()
        CreateBatchPanel.Visible = True
        BindrcbAds()
    End Sub

    Public Sub SetEditMode(ByVal isOn As Boolean)
        'Session("EditAll") = isOn
        rtbeTransferFromDC.FindItemByText("Edit All").Visible = Not isOn
        rtbeTransferFromDC.FindItemByText("Save All").Visible = isOn
        rtbeTransferFromDC.FindItemByText("Cancel All").Visible = isOn
        rtbeTransferFromDC.FindItemByText("Export").Visible = Not isOn
        rtbeTransferFromDC.FindItemByText("Create Batch").Visible = Not isOn

        If CreateBatchPanel.Visible Then
            rtbeTransferFromDC.FindItemByText("Create Batch").Visible = False
        End If
    End Sub

#End Region

#Region "Methods"

   

#End Region

    Private Sub RespondToError(ByVal MethodName As String, ByVal Message As String)
        Session("ErrorMsg") = "Error in Method: " & MethodName & "<br/>Error Message: " & Message
        Response.Redirect("~/Error.aspx", False)
    End Sub

    Private Sub RadUPCGrid_CancelCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles RadUPCGrid.CancelCommand
        HideUnwantedColumnsOnEdit(True)
    End Sub

    

    Private Sub RadUPCGrid_ExportCellFormatting(sender As Object, e As Telerik.Web.UI.ExportCellFormattingEventArgs) Handles RadUPCGrid.ExportCellFormatting
        If e.FormattedColumn.UniqueName = "UPC" Or e.FormattedColumn.UniqueName = "SKU" Then
            e.Cell.Style("mso-number-format") = "0000"
        End If
    End Sub

    Private Class UPCInfo
        Private _UPC As Decimal
        Private _UPCDesc As String

        Public Property UPC() As Decimal
            Get
                Return _UPC
            End Get
            Set(ByVal value As Decimal)
                _UPC = value
            End Set
        End Property

        Public Property UPCDesc() As String
            Get
                Return _UPCDesc
            End Get
            Set(ByVal value As String)
                _UPCDesc = value
            End Set
        End Property

    End Class

    Private Sub HideUnwantedColumnsOnEdit(ByVal Show As Boolean)
        With RadUPCGrid.MasterTableView
            .GetColumn("EMMID").Display = Show
            .GetColumn("EMMDesc").Display = Show
            .GetColumn("SellArea").Display = False
            .GetColumn("DepartmentID").Display = Show
            .GetColumn("DepartmentDesc").Display = Show
            .GetColumn("BuyerID").Display = Show
            .GetColumn("BuyerDesc").Display = Show
            .GetColumn("CMGID").Display = False
            .GetColumn("CMGDesc").Display = False
            .GetColumn("ProductID").Display = False
            .GetColumn("ProductDesc").Display = False
            .GetColumn("ProductStatus").Display = False
            .GetColumn("MerchType").Display = False
            .GetColumn("Inventory").Display = False
            .GetColumn("SalesLast4Wks").Display = False
            .GetColumn("TotalOwnedAmount").Display = False
            .GetColumn("OriginalTicketPrice").Display = False
            .GetColumn("PurchaseOrderID").Display = False
            .GetColumn("POShipDate").Display = False
            .GetColumn("ReplenishFlag").Display = False
            .GetColumn("SKU").Display = False
            .GetColumn("VendorStyle").Display = Show
            .GetColumn("extracolumn").Display = False
            .GetColumn("OO").Display = False
            .GetColumn("TotalOwnedAmount").Display = False
            .GetColumn("UPCStatus").Display = False
        End With
        


    End Sub

    Private Sub RadUPCGrid_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles RadUPCGrid.ItemDataBound
        If (TypeOf e.Item Is GridEditableItem And e.Item.IsInEditMode) Then
            HideUnwantedColumnsOnEdit(False)

            Dim editItem As GridEditableItem = DirectCast(e.Item, GridEditableItem)
            Dim result As List(Of DCTransferReportInfo) = DB2Results.Where(Function(x) x.ISN = CDec(editItem.GetDataKeyValue("ISN")) And x.ColorCode = CDec(editItem.GetDataKeyValue("ColorCode"))).ToList
            Dim UPCs As List(Of UPCInfo) = result.Select(Function(y) New UPCInfo With { _
                                                                .UPC = y.UPC,
                                                                .UPCDesc = y.UPCDisplay}).Where(Function(n) Not n.UPCDesc.Contains("(0)")).ToList

            Dim rcbUPC As RadComboBox = DirectCast(editItem.FindControl("rcbUPC"), RadComboBox)
            Dim hfUPC As HiddenField = DirectCast(editItem.FindControl("hfUPC"), HiddenField)
            With rcbUPC
                Dim newUPC As New UPCInfo
                newUPC.UPC = 0
                newUPC.UPCDesc = " "
                UPCs.Add(newUPC)

                .DataSource = UPCs
                .DataTextField = "UPCDesc"
                .DataValueField = "UPC"
                .DataBind()
            End With

            If rcbUPC.FindItemByValue(hfUPC.Value.Trim) IsNot Nothing Then
                rcbUPC.FindItemByValue(hfUPC.Value.Trim).Selected = True
            Else
                If UPCs.Count >= 3 Then
                    rcbUPC.FindItemByValue("0").Selected = True
                End If
            End If

            Dim TransferLocations As List(Of LocationInfo) = CType(Session("TransferFromDC.TransferLocations"), Global.System.Collections.Generic.List(Of LocationInfo))
            Dim TULoc As TULocation = New TULocation
            Dim TransferFromLocations As List(Of LocationInfo) = CType(TULoc.GetAllByISNColor(CDec(editItem.GetDataKeyValue("ISN")), CInt(editItem.GetDataKeyValue("ColorCode"))), Global.System.Collections.Generic.List(Of Global.TurnInProcessAutomation.BusinessEntities.LocationInfo))

            Dim rcbFromLoc As RadComboBox = DirectCast(editItem.FindControl("rcbFromLoc"), RadComboBox)
            Dim hfFromLoc As HiddenField = DirectCast(editItem.FindControl("hfFromLoc"), HiddenField)

            With rcbFromLoc
                .DataSource = TransferFromLocations
                .DataTextField = "Loc_nme"
                .DataValueField = "Loc_id"
                .DataBind()
            End With

            If rcbFromLoc.FindItemByValue(hfFromLoc.Value.Trim) IsNot Nothing Then
                rcbFromLoc.FindItemByValue(hfFromLoc.Value.Trim).Selected = True
            End If

            Dim rcbToLoc As RadComboBox = DirectCast(editItem.FindControl("rcbToLoc"), RadComboBox)
            Dim hfToLoc As HiddenField = DirectCast(editItem.FindControl("hfToLoc"), HiddenField)
            With rcbToLoc
                .DataSource = TransferLocations
                .DataTextField = "Loc_nme"
                .DataValueField = "Loc_id"
                .DataBind()
                .SelectedValue = "955"
            End With

            If rcbToLoc.FindItemByValue(hfToLoc.Value.Trim) IsNot Nothing Then
                rcbToLoc.FindItemByValue(hfToLoc.Value.Trim).Selected = True
            End If

            DirectCast(editItem.FindControl("rtxtComments"), RadTextBox).Text = ""
        End If
    End Sub

    Private _isExportClicked As Boolean = False
    Private _isMIOClicked As Boolean = False

    Private Sub RadUPCGrid_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadUPCGrid.NeedDataSource
        Dim Dept As String = CStr(DCTransferSearchCtrl.SelectedDepartmentId)
        Dim Buyer As String = CStr(DCTransferSearchCtrl.SelectedBuyerId)
        Dim inVendor As String = CStr(DCTransferSearchCtrl.SelectedVendorId)
        Dim PriceStatusCodes As List(Of String) = DCTransferSearchCtrl.PriceStatusCodes

        RadUPCGrid.Visible = False
        If Dept = "0" And Buyer = "0" And inVendor = "0" Then
            mpeTransferFromDC.ErrorMessage = "Please enter atleast one select criteria to proceed."
            RadUPCGrid.Visible = False
            Exit Sub
        End If
        If Not _isExportClicked And Not _isMIOClicked Then
            GetReportData(Dept, Buyer, inVendor, PriceStatusCodes)
            If DB2Results.Count > 0 Then
                RadUPCGrid.MasterTableView.DataSource = (From b In DB2Results
                      Select b.EMMID,
                      b.EMMDesc,
                      b.BuyerID,
                      b.BuyerDesc,
                      b.InStoreDate,
                      b.DepartmentID,
                      b.DepartmentDesc,
                      b.Color,
                      b.ISN,
                    b.ColorCode,
                    b.OH,
                    b.OO,
                    b.OwnedRetailAmount,
                    b.VendorStyle,
                    b.ISNDesc,
                    b.SelectedUPC,
                    b.TransferFromDC,
                    b.TransferToDC,
                    b.Comments,
                    b.UPCDesc,
                    b.MerchStatus,
                    b.TransferQty,
                    b.Vendor
                ).Distinct()

                RadUPCGrid.Visible = True
            Else
                mpeTransferFromDC.ErrorMessage = "No records found to transfer."
                rtbeTransferFromDC.FindItemByText("Edit All").Visible = False
                rtbeTransferFromDC.FindItemByText("Save All").Visible = False
                rtbeTransferFromDC.FindItemByText("Cancel All").Visible = False
                rtbeTransferFromDC.FindItemByText("Export").Visible = False
                rtbeTransferFromDC.FindItemByText("Create Batch").Visible = False
                CreateBatchPanel.Visible = False
                RadUPCGrid.Visible = False
                Exit Sub
            End If

        End If
    End Sub

    Dim DB2Results As List(Of DCTransferReportInfo)

    Private Function GetReportData(ByVal DepartmentID As String, ByVal BuyerID As String, ByVal VendorId As String, ByVal PriceStatusCodes As List(Of String)) As List(Of DCTransferReportInfo)
        'Try
        Dim _TUDCTransfer As New TUDCTransfer
        DB2Results = _TUDCTransfer.GetDCTransferReportData(DepartmentID, BuyerID, VendorId, String.Join("|", PriceStatusCodes)).ToList
        Return DB2Results.ToList
        'Catch ex As Exception
        'Throw New Exception("GetReportData(" & DepartmentID & "," & BuyerID & "," & VendorId & ") : " & ex.Message)
        'End Try
    End Function

    Private Sub PutRowsInEditMode(ByRef rg As RadGrid, ByVal isEdit As Boolean)
        SetEditMode(isEdit)

        HideUnwantedColumnsOnEdit(Not isEdit)

        If rg.SelectedItems.Count > 0 Then
            For Each item As GridItem In rg.SelectedItems
                If TypeOf item Is GridEditableItem Then
                    Dim editableItem As GridEditableItem = CType(item, GridDataItem)
                    editableItem.Edit = isEdit
                End If
            Next
            rg.Rebind()
        Else
            If rg.Items.Count > 0 Then
                For Each item As GridItem In rg.Items
                    If TypeOf item Is GridEditableItem Then
                        Dim editableItem As GridEditableItem = CType(item, GridDataItem)
                        editableItem.Edit = isEdit
                    End If
                Next
                rg.Rebind()
            End If
        End If
        

    End Sub

    Private Sub RadUPCGrid_UpdateCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles RadUPCGrid.UpdateCommand
        SaveSingleRow(DirectCast(e.Item, GridEditableItem))
        HideUnwantedColumnsOnEdit(True)
    End Sub

    Private Sub SaveRows(ByRef rg As RadGrid)
        Try

            For Each item As GridItem In rg.Items
                If TypeOf item Is GridEditableItem And item.IsInEditMode Then
                    SaveSingleRow(item)
                End If
            Next

        Catch ex As Exception
            RespondToError("SaveRows()", ex.Message)
        End Try
    End Sub
    Private Sub SaveSingleRow(ByVal Item As GridItem)
        Dim transferRecordData As DCTransferReportInfo = GetScreenTransferData(Item)
        If CDbl(transferRecordData.SelectedUPC) <> 0 Then
            Dim _TUDCTransfer As New TUDCTransfer
            _TUDCTransfer.Save(transferRecordData)
        End If
        mpeTransferFromDC.ErrorMessage = "Data saved successfully."
    End Sub


    Private Function GetScreenTransferData(item As GridItem) As DCTransferReportInfo
        Dim transferRecordData As New DCTransferReportInfo()

        If (TypeOf item Is GridEditableItem And item.IsInEditMode) Then
            Dim editItem As GridEditableItem = DirectCast(item, GridEditableItem)
            If editItem.ItemIndex > -1 Then
                transferRecordData.ISN = CDec(editItem.GetDataKeyValue("ISN")) ' Primary Key
                transferRecordData.ColorCode = CDec(editItem.GetDataKeyValue("ColorCode")) ' Primary Key
                transferRecordData.SelectedUPC = CStr(DirectCast(editItem.FindControl("rcbUPC"), RadComboBox).SelectedValue)
                transferRecordData.TransferFromDC = CInt(DirectCast(editItem.FindControl("rcbFromLoc"), RadComboBox).SelectedValue)
                transferRecordData.TransferToDC = CInt(DirectCast(editItem.FindControl("rcbToLoc"), RadComboBox).SelectedValue)
                transferRecordData.Comments = CStr(DirectCast(editItem.FindControl("rtxtComments"), RadTextBox).Text)
                transferRecordData.TransferQty = CInt(DirectCast(editItem("TransferQtyClm").Controls(0), RadNumericTextBox).Text)
                transferRecordData.User = SessionWrapper.UserID
                transferRecordData.IsTransferred = "N"
            End If
        End If

        If (TypeOf item Is GridDataItem And Not item.IsInEditMode) Then
            Dim dataItem As GridEditableItem = DirectCast(item, GridDataItem)
            transferRecordData.ISN = CDec(dataItem.GetDataKeyValue("ISN")) ' Primary Key
            transferRecordData.ColorCode = CDec(dataItem.GetDataKeyValue("ColorCode")) ' Primary Key
            transferRecordData.Color = CStr(dataItem.GetDataKeyValue("Color"))
            transferRecordData.ISNDesc = CStr(dataItem.GetDataKeyValue("ISNDesc"))
            transferRecordData.DepartmentID = CStr(dataItem.GetDataKeyValue("DepartmentID"))
            transferRecordData.SelectedUPC = CStr(dataItem.GetDataKeyValue("SelectedUPC"))
            transferRecordData.User = SessionWrapper.UserID
            transferRecordData.IsTransferred = "N"
        End If

        Return transferRecordData
    End Function

    Private Function ValidData() As Boolean
        If rcbAds.SelectedValue.Length = 0 Or rcbPageNumber.SelectedValue.Length = 0 Then
            mpeTransferFromDC.ErrorMessage = " Ad Number and Page Number should be selected."
            Return False
        End If
        If RadUPCGrid.SelectedItems.Count <> 0 Then
            For Each item As GridDataItem In RadUPCGrid.MasterTableView.Items
                If item.Selected Then
                    Dim errorMessage As String = ""
                    If (TypeOf item Is GridDataItem And Not item.IsInEditMode) Then
                        If CInt(item("TransferQtyClm").Text) = 0 Or CInt(item("TransferQtyClm").Text) > CInt(item("OH").Text) Then
                            errorMessage += " Transfer Quantity should be more than 0 and less equal to Onhand Quantity."
                        End If
                        If CStr(DirectCast(item("UPC").Controls(0), System.Web.UI.DataBoundLiteralControl).Text).Trim = "" Then
                            errorMessage += " UPC is required."
                        End If
                        If CStr(DirectCast(item("TransferFromDC").Controls(0), System.Web.UI.DataBoundLiteralControl).Text).Trim = "" Then
                            errorMessage += "Transfer From DC is required."
                        End If
                        If CStr(DirectCast(item("TransferToDC").Controls(0), System.Web.UI.DataBoundLiteralControl).Text).Trim = "" Then
                            errorMessage += "Transfer To DC is required."
                        End If
                    End If


                    If errorMessage.Length > 0 Then
                        mpeTransferFromDC.ErrorMessage = "Error on ISN : " & item("ISN").Text.ToString & " " & errorMessage & ". Check for additional issues. "
                        Return False
                    End If

                End If
            Next
            Return True
        Else
            mpeTransferFromDC.ErrorMessage = " Please select items to create a batch."
            Return False
        End If
    End Function

    Private Sub SendToTurnIn()
        Try
            If ValidData() Then
                Dim _TUDCTransfer As New TUDCTransfer
                Dim BatchItems As New List(Of DCTransferReportInfo)
                Dim DeleteItems As New List(Of String)

                For Each item As GridItem In RadUPCGrid.SelectedItems
                    BatchItems.Add(GetScreenTransferData(item))
                Next

                'Transpose the comma separated values of UPC_NUM into XML records.
                Dim _transferUPCs As String = String.Empty
                _transferUPCs &= "<transferRecord>"

                For Each item As DCTransferReportInfo In BatchItems
                    _transferUPCs &= "<UPC num=""" & item.SelectedUPC & """ />"
                Next
                _transferUPCs &= "</transferRecord>"
                _TUDCTransfer.SendSampleRequest(_transferUPCs)

                Dim NewBatchId As Integer = _TUDCTransfer.CreateBatchInTurnIn(BatchItems, CDec(rcbAds.SelectedValue), CInt(rcbPageNumber.SelectedValue))

                If NewBatchId = 0 Then
                    mpeTransferFromDC.ErrorMessage = "Error in creating batch. "
                Else

                    'Transpose the comma separated values of UPC_NUM into XML records.
                    'Dim _transferUPCs As String = String.Empty
                    '_transferUPCs &= "<transferRecord>"
                    'Update the batch with new userid = userid + batchid
                    'Get UPCs to be used for the sample request
                    For Each item As DCTransferReportInfo In BatchItems
                        item.User = item.User + " (" + NewBatchId.ToString + ")"
                        _TUDCTransfer.UpdateDCTransferRecordAfterSubmit(item.ISN, item.ColorCode, item.User)
                        '_transferUPCs &= "<UPC num=""" & item.SelectedUPC & """ />"
                    Next
                    '_transferUPCs &= "</transferRecord>"


                    mpeTransferFromDC.PopUpMessage = "Batch Number " & NewBatchId.ToString & " created with " & BatchItems.Count.ToString & " records."
                    mpeTransferFromDC.ErrorMessage = "Batch Number " & NewBatchId.ToString & " created with " & BatchItems.Count.ToString & " records."
                    RadUPCGrid.Rebind()
                End If


            End If
        Catch ex As Exception
            RespondToError("SendToTurnIn()", ex.Message)
        End Try

    End Sub

    Private Sub ExportToExcel()
        'Show Required Columns
        ShowColumnsForExcel(True)
        RadUPCGrid.ExportSettings.IgnorePaging = True
        RadUPCGrid.ExportSettings.ExportOnlyData = True
        RadUPCGrid.ExportSettings.OpenInNewWindow = True
        RadUPCGrid.Columns(1).Visible = False ' Edit

        'Hide the not selected row
        If RadUPCGrid.SelectedItems.Count <> 0 Then
            For Each item As GridDataItem In RadUPCGrid.MasterTableView.Items
                If Not item.Selected Then
                    item.Visible = False
                End If
            Next
        End If

        RadUPCGrid.MasterTableView.ExportToExcel()
    End Sub

    Private Sub ShowColumnsForExcel(ByVal Show As Boolean)
        With RadUPCGrid.MasterTableView
            .GetColumn("ClientSelectColumn1").Display = Not Show
            .GetColumn("Vendor").Display = Not Show
            .GetColumn("ISN").Display = Not Show
            .GetColumn("ISNDesc").Display = Not Show
            .GetColumn("Color").Display = Not Show
            .GetColumn("OH").Display = Not Show

            .GetColumn("EMMID").Display = Show
            .GetColumn("EMMDesc").Display = Show
            .GetColumn("SellArea").Display = Show
            .GetColumn("DepartmentID").Display = Show
            .GetColumn("DepartmentDesc").Display = Show
            .GetColumn("BuyerID").Display = Show
            .GetColumn("BuyerDesc").Display = Show
            .GetColumn("CMGID").Display = Show
            .GetColumn("CMGDesc").Display = Show
            .GetColumn("ProductID").Display = Show
            .GetColumn("ProductDesc").Display = Show
            .GetColumn("ProductStatus").Display = Show

            .GetColumn("UPC").Display = Show
            .GetColumn("UPCDesc").Display = Show

            .GetColumn("UPCStatus").Display = Show
            .GetColumn("MerchType").Display = Show
            .GetColumn("MerchStatus").Display = Show
            .GetColumn("Inventory").Display = Show
            .GetColumn("SalesLast4Wks").Display = Show
            .GetColumn("OO").Display = Show
            .GetColumn("TotalOwnedAmount").Display = Show
            .GetColumn("OriginalTicketPrice").Display = Show
            .GetColumn("PurchaseOrderID").Display = Show
            .GetColumn("POShipDate").Display = Show
            .GetColumn("ReplenishFlag").Display = Show
            .GetColumn("SKU").Display = Show
            .GetColumn("VendorStyle").Display = Show

            .GetColumn("TransferFromDC").Display = Show
            .GetColumn("TransferToDC").Display = Show

            .GetColumn("extracolumn").Display = Show

            .GetColumn("TransferQtyClm").Display = Show
            .GetColumn("Comments").Display = Show

        End With
        
    End Sub
    Private Sub RadUPCGrid_GridExporting(sender As Object, e As Telerik.Web.UI.GridExportingArgs) Handles RadUPCGrid.GridExporting
        'If _isMIOClicked Then
        Try

        Catch ex As Exception
            Throw New Exception("Unable to write to the MIO folder. Please contact HelpDesk.")
        End Try
        '    Dim FileName As String = "\MIO_" + DateTime.Now.ToString("dd_mm_yyyy_hh_mm_ss_ffff") + ".xls"

        '    Dim path As String = ConfigurationManager.AppSettings("MIOTransferPath") + FileName

        '    Using fs As System.IO.FileStream = System.IO.File.Create(path)
        '        Dim info As Byte() = System.Text.Encoding.Default.GetBytes(e.ExportOutput)
        '        fs.Write(info, 0, info.Length)
        '    End Using
        'End If
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

    Private Sub rcbAds_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbAds.SelectedIndexChanged
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


    'Save/load expanded states Hash from the session
    Private _adList As List(Of AdInfoInfo)
    Private Property AdList() As List(Of AdInfoInfo)
        Get
            If Me._adList Is Nothing Then
                _adList = TryCast(Me.Session("TransferFromDC.AdInfo"), List(Of AdInfoInfo))
                If _adList Is Nothing Then
                    _adList = _TUAdInfo.GetTransferAds(True).ToList()
                    Me.Session("EMM_ExpandedState") = _adList
                End If
            End If

            Return Me._adList
        End Get
        Set(value As List(Of AdInfoInfo))
            _adList = value
        End Set
    End Property


    Private Sub BindrcbAds()
        With rcbAds
            .Items.Clear()
            .DataSource = AdList
            .DataValueField = "adnbr"
            .DataTextField = "AdNumberDesc"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Private Sub radBtnCreateBatch_Click(sender As Object, e As System.EventArgs) Handles radBtnCreateBatch.Click
        SendToTurnIn()
    End Sub
End Class



