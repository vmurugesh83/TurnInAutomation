Imports Telerik.Web.UI

Public Class PrintPreTurnInSetUpCreate
    Inherits System.Web.UI.Page

    Private _printPreTurnInCtrl As PrintPreTurnInCtrl = Nothing


    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim control As Control = LoadControl("~/WebUserControls/Print/PrintPreTurnInCtrl.ascx")
        If Not control Is Nothing Then
            Me.Master.SideBarPlaceHolder.Controls.Add(control)
        End If
        If TypeOf control Is PrintPreTurnInCtrl Then
            Me._printPreTurnInCtrl = CType(control, PrintPreTurnInCtrl)
        ElseIf TypeOf control Is PartialCachingControl And CType(control, PartialCachingControl).CachedControl IsNot Nothing Then
            Me._printPreTurnInCtrl = CType(CType(control, PartialCachingControl).CachedControl, PrintPreTurnInCtrl)
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.Master.SideBar.Width = 300
            HidePreviousNextItemsButtons()
            HidePrintButtons()
        End If
    End Sub

    Private Sub rtsPrintPreTurnInSetUpCreate_TabClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTabStripEventArgs) Handles rtsPrintPreTurnInSetUpCreate.TabClick
        Select Case e.Tab.Text
            Case "Ad Level"
                _printPreTurnInCtrl.ShowAdLevelSearchControl()
                Me.Master.SideBar.Visible = True
                HidePreviousNextItemsButtons()
                HidePrintButtons()

            Case "Result List"
                _printPreTurnInCtrl.ShowWorkListSearchControl()
                Me.Master.SideBar.Visible = True
                HidePreviousNextItemsButtons()
                HidePrintButtons()

            Case "Item Level"
                Me.Master.SideBar.Collapsed = True
                ShowPreviousNextItemsButtons()
                HidePrintButtons()

            Case "Color/Size Level"
                Me.Master.SideBar.Collapsed = True
                HidePreviousNextItemsButtons()
                ShowPrintButtons()

        End Select

    End Sub

    Private Sub grdResultList_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdResultList.NeedDataSource
        grdResultList.DataSource = GetResultListTopLevel()
    End Sub

    Private Sub grdResultList_SortCommand(sender As Object, e As Telerik.Web.UI.GridSortCommandEventArgs) Handles grdResultList.SortCommand
        grdResultList.Rebind()
    End Sub

    'details data bind
    Private Sub grdResultList_DetailTableDataBind(ByVal source As Object, ByVal e As Telerik.Web.UI.GridDetailTableDataBindEventArgs) Handles grdResultList.DetailTableDataBind
        Dim parentItem As GridDataItem = CType(e.DetailTableView.ParentItem, GridDataItem)
        If parentItem.Edit Then
            Return
        End If

        If (e.DetailTableView.Name = "grdSecondLevel") Then   'second level data bind 
            e.DetailTableView.DataSource = GetSecondLevelData()
        End If

    End Sub

    Protected Sub ToggleSelectedRow(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, CheckBox).Parent.Parent, GridItem).Selected = CType(sender, CheckBox).Checked
    End Sub

    'this function does the checkall / uncheck all on the grid items.
    Protected Sub ToggleSelectDeselectAll(ByVal sender As Object, ByVal e As EventArgs)
        If (CType(sender, CheckBox)).Checked Then

        End If
    End Sub

    Private Sub ShowPreviousNextItemsButtons()
        rtbPrintPreTurnInSetUpCreate.FindItemByText("Next Item").Visible = True                 'Next Item Button
        rtbPrintPreTurnInSetUpCreate.FindItemByText("Previous Item").Visible = True         'Previous Item Button
    End Sub

    Private Sub HidePreviousNextItemsButtons()
        rtbPrintPreTurnInSetUpCreate.FindItemByText("Next Item").Visible = False                 'Next Item Button
        rtbPrintPreTurnInSetUpCreate.FindItemByText("Previous Item").Visible = False         'Previous Item Button
    End Sub

    Private Sub ShowPrintButtons()
        rtbPrintPreTurnInSetUpCreate.FindItemByText("Print Labels").Visible = True
        rtbPrintPreTurnInSetUpCreate.FindItemByText("Print Report").Visible = True
    End Sub

    Private Sub HidePrintButtons()
        rtbPrintPreTurnInSetUpCreate.FindItemByText("Print Labels").Visible = False
        rtbPrintPreTurnInSetUpCreate.FindItemByText("Print Report").Visible = False
    End Sub

    'this function derives a dataset.
    Private Function GetResultListTopLevel() As DataSet
        Dim ds As New DataSet
        Dim dt As New DataTable
        dt.Columns.Add("ACode", GetType(String))
        dt.Columns.Add("Vendor", GetType(String))
        dt.Columns.Add("ISN", GetType(String))
        dt.Columns.Add("ISNLongDesc", GetType(String))
        dt.Columns.Add("VendorStyle", GetType(String))
        dt.Columns.Add("TurnedInPrint", GetType(String))
        dt.Columns.Add("TurnedIneComm", GetType(String))
        dt.Columns.Add("OO", GetType(String))
        dt.Columns.Add("OH", GetType(String))

        dt.Rows.Add("E221 - EP DEC12 FASHION", "23680  - EVAN PICONE", "406100660", "BRIGHT FLUTTER SLV DRAPE NK", "49035692", "Y/N", "Y/N", "125 - 11/25", "2000")
        dt.Rows.Add("E221A - EP DEC12 FASHION", "23680  - EVAN PICONE", "406100660", "BRIGHT FLUTTER SLV DRAPE NK", "49035692", "Y/N", "Y/N", "75 - 11/25", "2500")
        dt.Rows.Add("E221A - EP DEC12 FASHION", "23680  - EVAN PICONE", "406100660", "BRIGHT FLUTTER SLV DRAPE NK", "49035692", "Y/N", "Y/N", "50 - 11/25", "2550")
        dt.Rows.Add("D1211 - 11/12 DELIVERY", "40204 - TRUE  LOVE ACCESSORIES", "375102552", "NOV MULTI CHEETAH SCARF", "SC-19297", "Y/N", "Y/N", "0", "1000")
        dt.Rows.Add("D1212 - 12/12 DELIVERY", "26487 - CURRANTS", "400100656", "STRIPPED L/S DROP NEEDLE TEE", "60237064", "Y/N", "Y/N", "100 - 12/20", "50")
        ds.Tables.Add(dt)
        Return ds
    End Function

    'this function derives a dataset.
    Private Function GetSecondLevelData() As DataSet
        Dim ds As New DataSet
        Dim dt As New DataTable
        dt.Columns.Add("VendorColor", GetType(String))
        dt.Columns.Add("TurnedInPrint", GetType(String))
        dt.Columns.Add("TurnedIneComm", GetType(String))
        dt.Columns.Add("OO", GetType(String))
        dt.Columns.Add("OH", GetType(String))

        dt.Rows.Add("1-BLACK", "Y/N", "Y/N", "0", "25")
        dt.Rows.Add("50-GREY", "Y/N", "Y/N", "0", "10")
        dt.Rows.Add("300-GREEN", "Y/N", "Y/N", "0", "5")
        dt.Rows.Add("420-BLUE", "Y/N", "Y/N", "0", "10")
        dt.Rows.Add("530-PURPLE", "Y/N", "Y/N", "100 - 12/20", "0")

        ds.Tables.Add(dt)
        Return ds
    End Function

    Private Sub grdColorSizeLevel_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdColorSizeLevel.NeedDataSource
        grdColorSizeLevel.DataSource = GetColorSizeLevel()
    End Sub

    Private Sub grdColorSizeLevel_SortCommand(sender As Object, e As Telerik.Web.UI.GridSortCommandEventArgs) Handles grdColorSizeLevel.SortCommand
        grdColorSizeLevel.Rebind()
    End Sub

    'this function derives a dataset.
    Private Function GetColorSizeLevel() As DataSet
        Dim ds As New DataSet
        Dim dt As New DataTable
        dt.Columns.Add("Vendor", GetType(String))
        dt.Columns.Add("VendorStyle", GetType(String))
        dt.Columns.Add("Color", GetType(String))
        dt.Columns.Add("Size", GetType(String))
        dt.Columns.Add("ImageName", GetType(String))
        dt.Columns.Add("OnOff", GetType(String))
        dt.Columns.Add("ImageType", GetType(String))
        dt.Columns.Add("ModelCount", GetType(String))
        dt.Columns.Add("ModelAge", GetType(String))
        dt.Columns.Add("ModelCat", GetType(String))
        dt.Columns.Add("ProductFeatures", GetType(String))
        dt.Columns.Add("StyleDescription", GetType(String))
        dt.Columns.Add("SampleStatus", GetType(String))
        dt.Columns.Add("SampleStatusDetails", GetType(String))
        dt.Columns.Add("RouteFromAd", GetType(String))
        dt.Columns.Add("Group", GetType(String))
        dt.Columns.Add("Sequence", GetType(String))
        dt.Columns.Add("Qty", GetType(String))
        dt.Columns.Add("Notes", GetType(String))
        dt.Columns.Add("Prop", GetType(String))
        dt.Columns.Add("Offer", GetType(String))

        dt.Rows.Add("37365 - SURE FIT INC", "107927275A", "20 - GREY", "16W", "Sample Image Name", "ON", "Vendor", "2", "15", "Snr Msy", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus ac risus libero. Duis metus nunc, mollis tincidunt faucibus nec, accumsan quis augue. Fusce scelerisque diam vel auctor fringilla. Duis volutpat urna a urna tincidunt accumsan nullam.", "Lorem ipsum dolor sit posuere.", "Available", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus ac risus libero. Duis metus nunc, mollis tincidunt faucibus nec, accumsan quis augue. Fusce scelerisque diam vel auctor fringilla. Duis volutpat urna a urna tincidunt accumsan nullam.", "456", "??", "1", "2", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus ac risus libero. Duis metus nunc, mollis tincidunt faucibus nec, accumsan quis augue. Fusce scelerisque diam vel auctor fringilla. Duis volutpat urna a urna tincidunt accumsan nullam.", "Yes", "")
        dt.Rows.Add("37365 - SURE FIT INC", "107927275A", "20 - GREY", "16W", "Sample Image Name", "ON", "Vendor", "2", "15", "Snr Msy", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus ac risus libero. Duis metus nunc, mollis tincidunt faucibus nec, accumsan quis augue. Fusce scelerisque diam vel auctor fringilla. Duis volutpat urna a urna tincidunt accumsan nullam.", "Lorem ipsum dolor sit posuere.", "Available", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus ac risus libero. Duis metus nunc, mollis tincidunt faucibus nec, accumsan quis augue. Fusce scelerisque diam vel auctor fringilla. Duis volutpat urna a urna tincidunt accumsan nullam.", "456", "??", "1", "2", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus ac risus libero. Duis metus nunc, mollis tincidunt faucibus nec, accumsan quis augue. Fusce scelerisque diam vel auctor fringilla. Duis volutpat urna a urna tincidunt accumsan nullam.", "Yes", "")
        dt.Rows.Add("37365 - SURE FIT INC", "107927275A", "20 - GREY", "16W", "Sample Image Name", "ON", "Vendor", "2", "15", "Snr Msy", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus ac risus libero. Duis metus nunc, mollis tincidunt faucibus nec, accumsan quis augue. Fusce scelerisque diam vel auctor fringilla. Duis volutpat urna a urna tincidunt accumsan nullam.", "Lorem ipsum dolor sit posuere.", "Available", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus ac risus libero. Duis metus nunc, mollis tincidunt faucibus nec, accumsan quis augue. Fusce scelerisque diam vel auctor fringilla. Duis volutpat urna a urna tincidunt accumsan nullam.", "456", "??", "1", "2", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus ac risus libero. Duis metus nunc, mollis tincidunt faucibus nec, accumsan quis augue. Fusce scelerisque diam vel auctor fringilla. Duis volutpat urna a urna tincidunt accumsan nullam.", "Yes", "")

        ds.Tables.Add(dt)
        Return ds
    End Function

End Class