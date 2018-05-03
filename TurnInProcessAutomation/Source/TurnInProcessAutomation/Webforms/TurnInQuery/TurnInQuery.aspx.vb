Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL.MiscConstants
Imports BonTon.Common.ExportFunctions
Imports TurnInProcessAutomation.BLL.Enumerations

Public Class TurnInQuery
    Inherits PageBase
    Private _turnInQueryCtrl As TurnInQueryCtrl = Nothing
    Private _TUEcommQueryTool As New TUEcommQueryTool
    Private _TUCtlgAdPg As New TUCtlgAdPg

#Region "Page Events"

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim control As Control = LoadControl("~/WebUserControls/TurnInQuery/TurnInQueryCtrl.ascx")
        If Not control Is Nothing Then
            Me.Master.SideBarPlaceHolder.Controls.Add(control)
            control.ID = "TurnInQueryCtrl"
        End If

        If TypeOf control Is TurnInQueryCtrl Then
            Me._turnInQueryCtrl = CType(control, TurnInQueryCtrl)
        ElseIf TypeOf control Is PartialCachingControl And CType(control, PartialCachingControl).CachedControl IsNot Nothing Then
            Me._turnInQueryCtrl = CType(CType(control, PartialCachingControl).CachedControl, TurnInQueryCtrl)
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Me.Master.SideBar.Width = 250
                grdUPCGrid.Visible = False
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub rtbTurnInQuery_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles rtbTurnInQuery.ButtonClick
        Try
            If TypeOf e.Item Is RadToolBarButton Then
                Dim radToolBarButton As RadToolBarButton = DirectCast(e.Item, RadToolBarButton)
                Select Case radToolBarButton.CommandName
                    Case "Retrieve"
                        _turnInQueryCtrl.ValidateSearch()
                        If Page.IsValid() Then
                            Session("ExportClicked") = Nothing
                            If CInt(_turnInQueryCtrl.SelectedView) = 1 Then
                                grdTurnInQuery.Visible = True
                                grdPreMedia.Visible = False
                                grdTurnInQuery.Rebind()
                            Else
                                grdTurnInQuery.Visible = False
                                grdPreMedia.Visible = True
                                grdPreMedia.Rebind()
                            End If
                            grdUPCGrid.Visible = False
                        End If
                    Case "Reset"
                        Me.Session("ErrorMsg") = Nothing
                        Me.Session("ExportClicked") = Nothing
                        If _turnInQueryCtrl.SelectedView = "2" Then
                            Response.Redirect(Request.Url.ToString & "?View=PM", False)
                        Else : Response.Redirect(Request.Url.ToString, False)
                        End If
                    Case "Back"
                        Session("ExportClicked") = Nothing
                        Response.Redirect(PreviousPageUrl, False)
                End Select
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

#End Region

#Region "Methods"

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

#Region "StandardView"

    Private Sub grdTurnInQuery_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grdTurnInQuery.ItemDataBound
        If TypeOf e.Item Is GridPagerItem Then
            IncreasePagingOptionsForGrid(grdTurnInQuery, DirectCast(e.Item, GridPagerItem))
        End If
        StrikeOutRecord(e)
    End Sub

    Private Sub StrikeOutRecord(e As Telerik.Web.UI.GridItemEventArgs)
        If (TypeOf e.Item Is GridDataItem) Then
            Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)
            Dim KilledInAdmin As Char = CChar(DirectCast(dataItem.DataItem, TurnInProcessAutomation.BusinessEntities.ECommTurnInQueryToolInfo).ImageSuffix)
            Dim KilledInApp As Char = CChar(DirectCast(dataItem.DataItem, TurnInProcessAutomation.BusinessEntities.ECommTurnInQueryToolInfo).RemoveMerchFlg)
            ' If it has been killed in application
            If (KilledInApp = "Y"c) Then
                dataItem.Font.Strikeout = True
                Exit Sub
            End If
            ' If it has been killed in admin
            If (KilledInAdmin = "Y"c) Then
                dataItem.Font.Strikeout = True
                Exit Sub
            End If
        End If
    End Sub

    Private Sub grdTurnInQuery_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdTurnInQuery.NeedDataSource
        Try
            If e.RebindReason = GridRebindReason.InitialLoad Then Exit Sub

            grdTurnInQueryDatabind()
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdTurnInQueryDatabind()
        Try
            If (Session("ExportClicked") IsNot Nothing) AndAlso (Session("ExportClicked").ToString = "Y") Then
                Session("ExportClicked") = Nothing
                Exit Sub
            End If

            If (_turnInQueryCtrl.rcbCMG.SelectedValue = "" And _turnInQueryCtrl.rcbCFG.SelectedValue = "" And _turnInQueryCtrl.rcbCRG.SelectedValue = "" And _turnInQueryCtrl.rcbBuyer.SelectedValue = "" And _turnInQueryCtrl.rcbFOB.SelectedValue = "" And _turnInQueryCtrl.rcbDept.SelectedValue = "" And _turnInQueryCtrl.rcbClass.SelectedValue = "" And _turnInQueryCtrl.rcbAcode.SelectedValue = "" And _turnInQueryCtrl.rcbVendor.SelectedValue = "" And _turnInQueryCtrl.rcbVendorStyle.SelectedValue = "" And _turnInQueryCtrl.rcbTurnInStatus.SelectedValue = "" And _turnInQueryCtrl.rcbTurnInType.SelectedValue = "" And _turnInQueryCtrl.SelectedView = "" And _turnInQueryCtrl.Selectedtidtefrm Is Nothing And _turnInQueryCtrl.SelectedtidteTo Is Nothing) Then
                mpeCommTurnInQuery.ErrorMessage = "Please select atleast one filter"
            Else
                'Fetch data
                grdTurnInQuery.DataSource = Nothing
                grdTurnInQuery.Visible = True
                grdTurnInQuery.ClientSettings.Selecting.AllowRowSelect = True
                Dim cmgid As String = _turnInQueryCtrl.SelectedCMGID
                Dim crgid As String = _turnInQueryCtrl.SelectedCRGID
                Dim cfgid As String = _turnInQueryCtrl.SelectedCFGID
                Dim fobid As String = _turnInQueryCtrl.SelectedFOBID
                Dim vendorid As String = CStr(_turnInQueryCtrl.SelectedVendorId)
                Dim acode As String = _turnInQueryCtrl.SelectedACode
                Dim classid As String = CStr(_turnInQueryCtrl.SelectedClassId)
                Dim tistatusid As String = _turnInQueryCtrl.Selectedtistatus
                Dim tintyp As String = _turnInQueryCtrl.Selectedtintype
                Dim tidatefrm As Date
                If Not (_turnInQueryCtrl.Selectedtidtefrm) Is Nothing Then
                    tidatefrm = CDate(_turnInQueryCtrl.Selectedtidtefrm)
                Else
                    tidatefrm = CDate("1/1/1900")
                End If
                Dim tidateto As Date
                If Not (_turnInQueryCtrl.SelectedtidteTo) Is Nothing Then
                    tidateto = CDate(_turnInQueryCtrl.SelectedtidteTo)
                Else
                    tidateto = CDate("12/12/9999")
                End If
                Dim viewid As String = _turnInQueryCtrl.SelectedView
                Dim adnumber As String = _turnInQueryCtrl.SelectedAd
                Dim buyer As String = _turnInQueryCtrl.SelectedBuyerId
                Dim DeptID As String = CStr(_turnInQueryCtrl.SelectedDepartmentId)
                Dim VndrStylID As String = _turnInQueryCtrl.SelectedVendorStyleID
                Dim BatchNum As String = _turnInQueryCtrl.BatchNumberValue
                Dim DB2Results As New List(Of ECommTurnInQueryToolInfo)

                DB2Results = _TUEcommQueryTool.GetEcommQueryResult(crgid, cmgid, cfgid, buyer, fobid, DeptID, classid, acode, vendorid, VndrStylID, adnumber, tistatusid, tintyp, CStr(tidatefrm), CStr(tidateto), viewid, BatchNum)

                'Add TurnInDate field, Where Clause for TurnInDate, and Orderby Clause
                Dim TurnInQueryResults As List(Of ECommTurnInQueryToolInfo) = DB2Results.Select(Function(x) New ECommTurnInQueryToolInfo With { _
                .TurnInMerchID = x.TurnInMerchID, _
                .AD_NUM = x.AD_NUM, _
                .PAGE_NUM = x.PAGE_NUM, _
                .Turn_in_Indicator = x.Turn_in_Indicator, _
                .Turn_in_Date = x.Turn_in_Date, _
                .OO = x.OO, _
                .OH = x.OH, _
                .Ship_Date = x.Ship_Date, _
                .ReserveFlag = x.ReserveFlag, _
                .TIStatus = x.TIStatus, _
                .Department = x.Department, _
                .Buyer = x.Buyer, _
                .Vendor = x.Vendor, _
                .Vendor_Style = x.Vendor_Style, _
                .ISN = x.ISN, _
                .Style_Desc = x.Style_Desc, _
                .Feature_Web_Cat = x.Feature_Web_Cat, _
                .InWebCat = x.InWebCat, _
                .Image_ID = x.Image_ID, _
                .Friendly_Product_Description = x.Friendly_Product_Description, _
                .Color = x.Color, _
                .Friendly_color = x.Friendly_color, _
                .VT_Path = x.VT_Path, _
                .OnOff_Figure = x.OnOff_Figure, _
                .Model_Category = x.Model_Category, _
                .Feature_Render_Swatch = x.Feature_Render_Swatch, _
                .MerchID = x.MerchID, _
                .AltView = x.AltView, _
                .Feature_Image_ID = x.Feature_Image_ID, _
                .RemoveMerchFlg = x.RemoveMerchFlg, _
                .Hot_Rushed = x.Hot_Rushed, _
                .ClrCorrectFlg = x.ClrCorrectFlg, _
                .EMMNotes = x.EMMNotes, _
                .AdNbrAdminImgNbr = x.AdNbrAdminImgNbr, _
                .MerchantNotes = x.MerchantNotes, _
                .BatchNum = x.BatchNum, _
                .ImageNotes = x.ImageNotes, _
                .RouteFromAD = x.RouteFromAD, _
                .ImageSuffix = x.ImageSuffix}) _
            .ToList()

                If _turnInQueryCtrl.rcbRouteFromAd.SelectedValue = "WITH" Then
                    TurnInQueryResults = TurnInQueryResults.Where(Function(x) x.RouteFromAD > 0).ToList
                ElseIf _turnInQueryCtrl.rcbRouteFromAd.SelectedValue = "WITHOUT" Then
                    TurnInQueryResults = TurnInQueryResults.Where(Function(x) x.RouteFromAD = 0).ToList
                End If

                grdTurnInQuery.DataSource = TurnInQueryResults
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdTurnInQuery_PreRender(sender As Object, e As System.EventArgs) Handles grdTurnInQuery.PreRender
        'Enable the Toolbar buttons if Grid contains at least one row.
        rtbTurnInQuery.FindItemByText("Export External").Enabled = False
        If grdTurnInQuery.MasterTableView.Items.Count > 0 Then
            rtbTurnInQuery.FindItemByText("Export").Enabled = True
        Else
            rtbTurnInQuery.FindItemByText("Export").Enabled = False
        End If
    End Sub

    Private Sub grdTurnInQuery_SortCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridSortCommandEventArgs) Handles grdTurnInQuery.SortCommand
        grdTurnInQuery.Rebind()
    End Sub

#End Region

#Region "PreMediaView"

    Private Sub grdPreMedia_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grdPreMedia.ItemDataBound
        If TypeOf e.Item Is GridPagerItem Then
            IncreasePagingOptionsForGrid(grdPreMedia, DirectCast(e.Item, GridPagerItem))
        End If
        StrikeOutRecord(e)
    End Sub

    Private Sub IncreasePagingOptionsForGrid(ByVal grid As Telerik.Web.UI.RadGrid, ByVal gridPagerItem As GridPagerItem)
        Dim PageSizeCombo As RadComboBox = DirectCast(gridPagerItem.FindControl("PageSizeComboBox"), RadComboBox)
        PageSizeCombo.Items.Add(New RadComboBoxItem("100"))
        PageSizeCombo.FindItemByText("100").Attributes.Add("ownerTableViewId", grid.MasterTableView.ClientID)
        PageSizeCombo.Items.Add(New RadComboBoxItem("150"))
        PageSizeCombo.FindItemByText("150").Attributes.Add("ownerTableViewId", grid.MasterTableView.ClientID)
        PageSizeCombo.Items.Add(New RadComboBoxItem("200"))
        PageSizeCombo.FindItemByText("200").Attributes.Add("ownerTableViewId", grid.MasterTableView.ClientID)
        PageSizeCombo.FindItemByText(gridPagerItem.OwnerTableView.PageSize.ToString()).Selected = True
    End Sub

    Private Sub grdPreMedia_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdPreMedia.NeedDataSource
        Try
            If e.RebindReason = GridRebindReason.InitialLoad Then Exit Sub

            grdPreMediaDatabind()
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdPreMediaDatabind()
        Try
            If (Session("ExportClicked") IsNot Nothing) AndAlso (Session("ExportClicked").ToString = "Y") Then
                Session("ExportClicked") = Nothing
                Exit Sub
            End If

            If (_turnInQueryCtrl.rcbCMG.SelectedValue = "" And _turnInQueryCtrl.rcbCFG.SelectedValue = "" And _turnInQueryCtrl.rcbCRG.SelectedValue = "" And _turnInQueryCtrl.rcbBuyer.SelectedValue = "" And _turnInQueryCtrl.rcbFOB.SelectedValue = "" And _turnInQueryCtrl.rcbDept.SelectedValue = "" And _turnInQueryCtrl.rcbClass.SelectedValue = "" And _turnInQueryCtrl.rcbAcode.SelectedValue = "" And _turnInQueryCtrl.rcbVendor.SelectedValue = "" And _turnInQueryCtrl.rcbVendorStyle.SelectedValue = "" And _turnInQueryCtrl.rcbTurnInStatus.SelectedValue = "" And _turnInQueryCtrl.rcbTurnInType.SelectedValue = "" And _turnInQueryCtrl.SelectedView = "" And _turnInQueryCtrl.Selectedtidtefrm Is Nothing And _turnInQueryCtrl.SelectedtidteTo Is Nothing) Then
                mpeCommTurnInQuery.ErrorMessage = "Please select atleast one filter"
            Else
                'Fetch data
                grdPreMedia.DataSource = Nothing
                grdPreMedia.Visible = True
                grdPreMedia.ClientSettings.Selecting.AllowRowSelect = True
                Dim cmgid As String = _turnInQueryCtrl.SelectedCMGID
                Dim crgid As String = _turnInQueryCtrl.SelectedCRGID
                Dim cfgid As String = _turnInQueryCtrl.SelectedCFGID
                Dim fobid As String = _turnInQueryCtrl.SelectedFOBID
                Dim vendorid As String = CStr(_turnInQueryCtrl.SelectedVendorId)
                Dim acode As String = _turnInQueryCtrl.SelectedACode
                Dim classid As String = CStr(_turnInQueryCtrl.SelectedClassId)
                Dim tistatusid As String = _turnInQueryCtrl.Selectedtistatus
                Dim tintyp As String = _turnInQueryCtrl.Selectedtintype
                Dim tidatefrm As Date
                If Not (_turnInQueryCtrl.Selectedtidtefrm) Is Nothing Then
                    tidatefrm = CDate(_turnInQueryCtrl.Selectedtidtefrm)
                Else
                    tidatefrm = Date.MinValue
                End If
                Dim tidateto As Date
                If Not (_turnInQueryCtrl.SelectedtidteTo) Is Nothing Then
                    tidateto = CDate(_turnInQueryCtrl.SelectedtidteTo)
                Else
                    tidateto = Date.MaxValue
                End If
                Dim viewid As String = _turnInQueryCtrl.SelectedView
                Dim adnumber As String = _turnInQueryCtrl.SelectedAdPM
                Dim pagenumber As String = _turnInQueryCtrl.SelectedPageNumber
                Dim buyer As String = _turnInQueryCtrl.SelectedBuyerId
                Dim DeptID As String = CStr(_turnInQueryCtrl.SelectedDepartmentId)
                Dim VndrStylID As String = _turnInQueryCtrl.SelectedVendorStyleID
                Dim TurnInPreMediaResults As New List(Of ECommTurnInQueryToolInfo)

                Dim InWebCat As String = CStr(_turnInQueryCtrl.SelectedInWebCat)
                Dim Suffix As String = CStr(_turnInQueryCtrl.SelectedSuffix)
                Dim ImageType As String = CStr(_turnInQueryCtrl.SelectedImageType)
                Dim ModelCategoryCode As String = CStr(_turnInQueryCtrl.SelectedModelCategory)
                Dim FeatureWebCat As String = CStr(_turnInQueryCtrl.SelectedFeatWebCat)
                Dim BatchNum As String = CStr(_turnInQueryCtrl.BatchNumberValue)

                TurnInPreMediaResults = _TUEcommQueryTool.GetEcommPreMediaResult(crgid, cmgid, cfgid, buyer, fobid, DeptID, classid, acode, vendorid, VndrStylID, adnumber, pagenumber, tistatusid, tintyp, tidatefrm, tidateto, InWebCat, Suffix, ImageType, ModelCategoryCode, FeatureWebCat, BatchNum)

                If _turnInQueryCtrl.rcbRouteFromAd.SelectedValue = "WITH" Then
                    TurnInPreMediaResults = TurnInPreMediaResults.Where(Function(x) x.RouteFromAD > 0).ToList
                ElseIf _turnInQueryCtrl.rcbRouteFromAd.SelectedValue = "WITHOUT" Then
                    TurnInPreMediaResults = TurnInPreMediaResults.Where(Function(x) x.RouteFromAD = 0).ToList
                End If

                grdPreMedia.DataSource = TurnInPreMediaResults
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub grdPreMedia_PreRender(sender As Object, e As System.EventArgs) Handles grdPreMedia.PreRender
        'Enable the Toolbar buttons if Grid contains at least one row.
        If grdPreMedia.MasterTableView.Items.Count > 0 Then
            'Disable/Enable Export buttons based on User Role.
            If CInt(_turnInQueryCtrl.SelectedView) = 2 Then
                If (checkAccessability("Premedia") = "False") Then
                    rtbTurnInQuery.FindItemByText("Export External").Enabled = False
                    rtbTurnInQuery.FindItemByText("Export").Enabled = False
                    rtbTurnInQuery.FindItemByText("Print").Enabled = False
                Else
                    rtbTurnInQuery.FindItemByText("Export External").Enabled = True
                    rtbTurnInQuery.FindItemByText("Export").Enabled = True
                    rtbTurnInQuery.FindItemByText("Print").Enabled = True
                End If
            End If
        Else
            rtbTurnInQuery.FindItemByText("Export External").Enabled = False
            rtbTurnInQuery.FindItemByText("Export").Enabled = False
            rtbTurnInQuery.FindItemByText("Print").Enabled = True
        End If

    End Sub

    Private Sub grdPreMedia_SortCommand(sender As Object, e As Telerik.Web.UI.GridSortCommandEventArgs) Handles grdPreMedia.SortCommand
        grdPreMedia.Rebind()
    End Sub

#End Region

#Region "EXPORT"
    ''' <summary>
    '''  Validation for Export
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExportValidated() As Boolean
        mpeCommTurnInQuery.ErrorMessage = ""
        Select Case hdnExport.Value.ToString
            Case "Export"
                If ((CInt(_turnInQueryCtrl.SelectedView) = 1) And grdTurnInQuery.SelectedItems.Count = 0) Or ((CInt(_turnInQueryCtrl.SelectedView) = 2) And grdPreMedia.SelectedItems.Count = 0) Then
                    mpeCommTurnInQuery.ErrorMessage = "Select at least one record."
                    Return False
                End If
            Case "ExportExtr", "Print"
                'No Validation
            Case "Report"
                If _turnInQueryCtrl.SelectedAd.Trim.Length = 0 Then
                    mpeCommTurnInQuery.ErrorMessage = "Please select Week or Ad Number."
                    Return False
                End If
        End Select
        Return True
    End Function

    ''' <summary>
    ''' Export Functionality
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Export(ByVal sender As Object, ByVal e As EventArgs)
        If ExportValidated() Then
            Select Case hdnExport.Value.ToString
                Case "Export"
                    ExportList()
                Case "ExportExtr"
                    ExportExtrList()
                Case "Report" ' Export of UPC Report
                    Dim export As New Export
                    grdUPCGrid.ExportSettings.Excel.Format = GridExcelExportFormat.Html
                    grdUPCGrid.ExportSettings.IgnorePaging = True
                    grdUPCGrid.ExportSettings.ExportOnlyData = True
                    grdUPCGrid.ExportSettings.OpenInNewWindow = True
                    grdUPCGrid.MasterTableView.ExportToExcel()

                    If ExportText = "PDF" Then
                        export.Append(grdUPCGrid, "     ")
                        export.MultipleExportToPDF("UPC Report", True)
                    ElseIf ExportText = "Excel" Then
                        export.Append(grdUPCGrid, "     ")
                        export.MultipleExportToExcel("UPC Report")
                    End If

                    mpeCommTurnInQuery.ErrorMessage = ""
                Case "Print"
                    PrintList()
            End Select
        End If
    End Sub

    Private Sub PrintList()
        Try
            Session("ExportClicked") = "Y"

            If (CInt(_turnInQueryCtrl.SelectedView) = 1) Then
                PrintGrid(grdTurnInQuery) ' Standard
            Else
                PrintGrid(grdPreMedia) 'Pre-Media
            End If

        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub PrintGrid(ByVal DataGrid As RadGrid)
        DataGrid.ExportSettings.Excel.Format = GridExcelExportFormat.Html
        For Each GrdDataItem As GridDataItem In DataGrid.Items
            GrdDataItem.Visible = GrdDataItem.Selected
        Next

        With DataGrid
            .ExportSettings.IgnorePaging = True
            .ExportSettings.OpenInNewWindow = True
            .ExportSettings.ExportOnlyData = True
            .AllowSorting = False
            .MasterTableView.Columns(0).Visible = False
        End With

        Dim export As New Export
        export.Append(DataGrid, "     ")
        If ExportText = "PDF" Then
            export.MultipleExportToPDF(lblPageHeader.Text, True)
        ElseIf ExportText = "Excel" Then
            export.MultipleExportToExcel(lblPageHeader.Text)
        End If
    End Sub

    Private Sub ExportList()
        Try
            Session("ExportClicked") = "Y"

            Dim export As New Export
            Dim isExpToFreelance As Boolean = False

            If (CInt(_turnInQueryCtrl.SelectedView) = 1) Then
                grdTurnInQuery.ExportSettings.Excel.Format = GridExcelExportFormat.Html

                For Each GrdDataItem As GridDataItem In grdTurnInQuery.Items
                    GrdDataItem.Visible = GrdDataItem.Selected
                Next

                With grdTurnInQuery
                    .ExportSettings.IgnorePaging = True
                    .ExportSettings.OpenInNewWindow = True
                    .ExportSettings.ExportOnlyData = True
                    .AllowSorting = False
                    .MasterTableView.Columns(0).Visible = False
                    export.Append(grdTurnInQuery, "     ")
                    If ExportText = "PDF" Then
                        export.MultipleExportToPDF(lblPageHeader.Text, True)
                    ElseIf ExportText = "Excel" Then
                        export.MultipleExportToExcel(lblPageHeader.Text)
                    End If
                End With
            Else
                'Check whether any of the selected items are already in EXTR status.
                For Each gridSelItem As GridDataItem In grdPreMedia.MasterTableView.GetSelectedItems
                    If CType(gridSelItem("TIStatus"), GridTableCell).Text.Trim.ToUpper.Contains("EXPORTED TO FREELANCE") Then
                        isExpToFreelance = True
                        Exit For
                    End If
                Next

                If Not isExpToFreelance Then
                    UpdateExportStatus("INTR")

                    For Each GrdDataItem As GridDataItem In grdPreMedia.MasterTableView.Items
                        If Not GrdDataItem.Selected Then
                            GrdDataItem.Visible = False
                        End If
                    Next

                    With grdPreMedia
                        .ExportSettings.IgnorePaging = True
                        .ExportSettings.OpenInNewWindow = True
                        .ExportSettings.ExportOnlyData = True
                        .AllowSorting = False
                        .MasterTableView.Columns(0).Visible = False

                        If ExportText = "PDF" Then
                            export.Append(grdPreMedia, "     ")
                            export.MultipleExportToPDF(lblPageHeader.Text, True)
                        ElseIf ExportText = "Excel" Then
                            export.Append(grdPreMedia, "     ")
                            export.MultipleExportToExcel(lblPageHeader.Text)
                        End If
                    End With
                Else
                    mpeCommTurnInQuery.ErrorMessage = "Please deselect the records that are already exported to Freelance."
                End If
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub ExportExtrList()
        Try
            Dim export As New Export

            If grdPreMedia.SelectedItems.Count > 0 Then
                UpdateExportStatus("EXTR")

                Session("ExportClicked") = "Y"

                For Each GrdDataItem As GridDataItem In grdPreMedia.MasterTableView.Items
                    If Not GrdDataItem.Selected Then
                        GrdDataItem.Visible = False
                    Else
                        GrdDataItem("TIStatus").Text = "Exported to Freelance"
                    End If
                Next

                With grdPreMedia
                    .ExportSettings.IgnorePaging = True
                    .ExportSettings.OpenInNewWindow = True
                    .ExportSettings.ExportOnlyData = True
                    .AllowSorting = False
                    .MasterTableView.Columns(0).Visible = False

                    If ExportText = "PDF" Then
                        export.Append(grdPreMedia, "     ")
                        export.MultipleExportToPDF(lblPageHeader.Text, True)
                    ElseIf ExportText = "Excel" Then
                        export.Append(grdPreMedia, "     ")
                        export.MultipleExportToExcel(lblPageHeader.Text)
                    End If
                End With

                'grdPreMedia.Rebind()
            Else
                mpeCommTurnInQuery.ErrorMessage = "Select at least one record."
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Public Sub UpdateExportStatus(ByVal strStatusCde As String)
        Dim TurnInMerchIDs As New List(Of String)

        For Each gridSelItem As GridDataItem In grdPreMedia.MasterTableView.GetSelectedItems
            TurnInMerchIDs.Add(CStr(gridSelItem.GetDataKeyValue("TurnInMerchID")))
        Next

        'Update the status to 'INTR' or 'EXTR', in the database.
        _TUEcommQueryTool.UpdateStatus(String.Join(",", TurnInMerchIDs), strStatusCde, SessionWrapper.UserID)
    End Sub

#End Region

#End Region

#Region "Modal"

#Region "Members"

    Private _enablePDFExport As Boolean = True

#End Region

#Region "Properties"

    Public ReadOnly Property ExportText() As String
        Get
            Return Me.rblExport.SelectedValue
        End Get
    End Property

    Public ReadOnly Property ExportCommandName() As String
        Get
            Select Case rblExport.SelectedValue
                Case "PDF"
                    Return RadGrid.ExportToPdfCommandName

                Case "Excel"
                    Return RadGrid.ExportToExcelCommandName

                Case Else
                    Return Nothing

            End Select
        End Get
    End Property

    Public ReadOnly Property OkButton() As LinkButton
        Get
            Return Me.lnkOK
        End Get
    End Property

    Public Property EnablePDFExport() As Boolean
        Get
            Return _enablePDFExport
        End Get
        Set(ByVal value As Boolean)
            _enablePDFExport = value
        End Set
    End Property

#End Region

#Region "Events"

    Private Sub Control_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        rblExport.Items.FindByValue("PDF").Enabled = EnablePDFExport
    End Sub

#End Region

#Region "Methods"

    Public Sub Hide()
        If Not Me.mPopup Is Nothing Then
            Me.mPopup.Hide()
        End If
    End Sub

    Public Sub Show()
        If Not Me.mPopup Is Nothing Then
            Me.mPopup.Show()
        End If
    End Sub

#End Region

#End Region

    Private Sub lnkOK_Click(sender As Object, e As System.EventArgs) Handles lnkOK.Click
        Export(sender, e)
    End Sub

    Private Sub grdUPCGrid_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdUPCGrid.NeedDataSource
        'Clear all the sort expressions to Reset the sorting on the Grid to Default one.
        If grdUPCGrid.MasterTableView.SortExpressions.GetSortString() Is Nothing Then
            grdUPCGrid.MasterTableView.SortExpressions.Clear()
        End If

        'Populate the Grid with the data.
        grdUPCGrid.MasterTableView.DataSource = _TUEcommQueryTool.GetUPCReport(_turnInQueryCtrl.SelectedAd)
    End Sub
End Class
