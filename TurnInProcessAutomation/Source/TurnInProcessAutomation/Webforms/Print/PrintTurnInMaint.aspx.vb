Imports Telerik.Web.UI

Public Class PrintTurnInMaint
    Inherits System.Web.UI.Page

    Private _printTurnInMaintctrl As PrintTurnInMaintctrl = Nothing


    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim control As Control = LoadControl("~/WebUserControls/Print/PrintTurnInMaintctrl.ascx")
        If Not control Is Nothing Then
            Me.Master.SideBarPlaceHolder.Controls.Add(control)
        End If
        If TypeOf control Is PrintTurnInMaintctrl Then
            Me._printTurnInMaintctrl = CType(control, PrintTurnInMaintctrl)
        ElseIf TypeOf control Is PartialCachingControl And CType(control, PartialCachingControl).CachedControl IsNot Nothing Then
            Me._printTurnInMaintctrl = CType(CType(control, PartialCachingControl).CachedControl, PrintTurnInMaintctrl)
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.Master.SideBar.Width = 200
        End If
    End Sub

#Region "MerchCoord"

    Private Sub grdMerchCoord_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMerchCoord.NeedDataSource
        grdMerchCoord.DataSource = GetMerchCoordTopLevel()
    End Sub

    Private Sub grdMerchCoord_SortCommand(sender As Object, e As Telerik.Web.UI.GridSortCommandEventArgs) Handles grdMerchCoord.SortCommand
        grdMerchCoord.Rebind()
    End Sub

    'details data bind
    Private Sub grdMerchCoord_DetailTableDataBind(ByVal source As Object, ByVal e As Telerik.Web.UI.GridDetailTableDataBindEventArgs) Handles grdMerchCoord.DetailTableDataBind
        Dim parentItem As GridDataItem = CType(e.DetailTableView.ParentItem, GridDataItem)
        If parentItem.Edit Then
            Return
        End If

        If (e.DetailTableView.Name = "grdSecondLevel") Then   'second level data bind 
            e.DetailTableView.DataSource = GetMerchCoordSecondLevel()
        End If

    End Sub

    'this function derives a dataset.
    Private Function GetMerchCoordTopLevel() As DataSet
        Dim ds As New DataSet
        Dim dt As New DataTable
        dt.Columns.Add("PageName", GetType(String))
        dt.Columns.Add("OfferType", GetType(String))
        dt.Columns.Add("OfferName", GetType(String))

        dt.Rows.Add("005-RTW_BONUS_B", "Bonus Buy", "B45 CAP BONUS RH 29.97 SWTRS AND BLAZERS")
        dt.Rows.Add("005-RTW_BONUS_B", "Bonus Buy", "B45 CAP BONUS RH 19.97 RIBBED TANK")
        ds.Tables.Add(dt)
        Return ds
    End Function

    'this function derives a dataset.
    Private Function GetMerchCoordSecondLevel() As DataSet
        Dim ds As New DataSet
        Dim dt As New DataTable
        dt.Columns.Add("VendorStyle", GetType(String))
        dt.Columns.Add("StyleDesc", GetType(String))
        dt.Columns.Add("FriendlyColor", GetType(String))
        dt.Columns.Add("StylingNotes", GetType(String))
        dt.Columns.Add("RouteFromAd", GetType(String))
        dt.Columns.Add("Status", GetType(String))
        dt.Columns.Add("StatusDate", GetType(String))

        dt.Rows.Add("RH2H557M", "RH L/SCOWL HOODIE SWEATER", "Charcoal", "Don't put hood", "", "", "")
        dt.Rows.Add("RH2F150M", "RH COLORED BOOTCUT DENIM", "Plum Eclipse", "Make sure fitted", "31204", "", "")
        dt.Rows.Add("RH2H573M", "RH L/S FAIRISLE YOKE", "Pacific Wabe/Gab", "Dont tuck..", "", "", "")
        dt.Rows.Add("RHOR103M", "RH REPLEN BOOTCUT DENIM", "Ocean", "Make sure fitted", "31187", "", "")

        dt.Rows.Add("RH2H556M", "RH 4*4 RIBBED T-NECK SPACEDYE ", "Black", "Don't put hood", "", "", "")
        dt.Rows.Add("RHOR105M", "RH REPPLEN BOOTCUT DEN (PROP)", "New Rinse", "Make sure fitted", "", "", "")
        dt.Rows.Add("RH2H556M", "RH 4*4 Ribbed T-Neck", "Plum Eclipse Hea", "Dont tuck fold..", "31204", "", "")
        dt.Rows.Add("RH2R107M", "RH REPLEN BOOTCUT DENIM(PROP)", "Boston", "Make sure fitted", "", "", "")


        ds.Tables.Add(dt)
        Return ds
    End Function


#End Region

   



#Region "MediaCoord"

    Private Sub grdMediaCoord_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMediaCoord.NeedDataSource
        grdMediaCoord.DataSource = GetMediaCoordTopLevel()
    End Sub

    Private Sub grdMediaCoord_SortCommand(sender As Object, e As Telerik.Web.UI.GridSortCommandEventArgs) Handles grdMediaCoord.SortCommand
        grdMediaCoord.Rebind()
    End Sub

    'details data bind
    Private Sub grdMediaCoord_DetailTableDataBind(ByVal source As Object, ByVal e As Telerik.Web.UI.GridDetailTableDataBindEventArgs) Handles grdMediaCoord.DetailTableDataBind
        Dim parentItem As GridDataItem = CType(e.DetailTableView.ParentItem, GridDataItem)
        If parentItem.Edit Then
            Return
        End If

        If (e.DetailTableView.Name = "grdSecondLevel") Then   'second level data bind 
            e.DetailTableView.DataSource = GetMediaCoordSecondLevel()
        End If

    End Sub

    'this function derives a dataset.
    Private Function GetMediaCoordTopLevel() As DataSet
        Dim ds As New DataSet
        Dim dt As New DataTable
        dt.Columns.Add("PageName", GetType(String))
        dt.Columns.Add("OfferType", GetType(String))
        dt.Columns.Add("OfferName", GetType(String))

        dt.Rows.Add("005-RTW_BONUS_B", "Bonus Buy", "B45 CAP BONUS RH 29.97 SWTRS AND BLAZERS")
        dt.Rows.Add("005-RTW_BONUS_B", "Bonus Buy", "B45 CAP BONUS RH 19.97 RIBBED TANK")
        ds.Tables.Add(dt)
        Return ds
    End Function

    'this function derives a dataset.
    Private Function GetMediaCoordSecondLevel() As DataSet
        Dim ds As New DataSet
        Dim dt As New DataTable
        dt.Columns.Add("VendorStyle", GetType(String))
        dt.Columns.Add("StyleDesc", GetType(String))
        dt.Columns.Add("FriendlyColor", GetType(String))
        dt.Columns.Add("StylingNotes", GetType(String))
        dt.Columns.Add("OnOff", GetType(String))
        dt.Columns.Add("PU", GetType(String))
        dt.Columns.Add("PUImageID", GetType(String))
        dt.Columns.Add("ModelCatg", GetType(String))
        dt.Columns.Add("ModelAge", GetType(String))
        dt.Columns.Add("NoOfModels", GetType(String))
        dt.Columns.Add("ImageName", GetType(String))
        dt.Columns.Add("Group", GetType(String))
        dt.Columns.Add("Seq", GetType(String))
        dt.Columns.Add("Shot", GetType(String))

        dt.Rows.Add("RH2H557M", "RH L/SCOWL HOODIE SWEATER", "Charcoal", "Don't put hood", "On", "", "", "", "", "", "RH Sweaters/Denim", "1", "1", "1")
        dt.Rows.Add("RH2F150M", "RH COLORED BOOTCUT DENIM", "Plum Eclipse", "Make sure fitted", "On", "", "", "", "", "", "RH Sweaters/Denim", "1", "2", "1")
        dt.Rows.Add("RH2H573M", "RH L/S FAIRISLE YOKE", "Pacific Wabe/Gab", "Dont tuck..", "On", "", "", "", "", "", "RH Sweaters/Denim", "2", "1", "1")
        dt.Rows.Add("RHOR103M", "RH REPLEN BOOTCUT DENIM", "Ocean", "Make sure fitted", "Off", "", "", "", "", "", "RH Sweaters/Denim", "2", "2", "1")

        dt.Rows.Add("RH2H556M", "RH 4*4 RIBBED T-NECK SPACEDYE ", "Black", "Don't put hood", "On", "", "", "", "", "", "RH Sweaters/Denim", "1", "1", "1")
        dt.Rows.Add("RHOR105M", "RH REPPLEN BOOTCUT DEN (PROP)", "New Rinse", "Make sure fitted", "On", "", "", "", "", "", "RH Sweaters/Denim", "1", "2", "1")
        dt.Rows.Add("RH2H556M", "RH 4*4 Ribbed T-Neck", "Plum Eclipse Hea", "Dont tuck fold..", "On", "", "", "", "", "", "RH Sweaters/Denim", "2", "1", "1")
        dt.Rows.Add("RH2R107M", "RH REPLEN BOOTCUT DENIM(PROP)", "Boston", "Make sure fitted", "On", "", "", "", "", "", "RH Sweaters/Denim", "2", "2", "1")


        ds.Tables.Add(dt)
        Return ds
    End Function


#End Region


End Class