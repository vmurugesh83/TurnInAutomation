Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities
Imports Telerik.Web.UI


Public Class WebForm2
    Inherits System.Web.UI.Page

    Private dalSQL As BLL.TUCtlgAdPg = New BLL.TUCtlgAdPg

    Public ReadOnly Property AllWebCats As List(Of WebCat)
        Get
            If Application("ApplWebCatsObject") Is Nothing Then
                Dim objGetApplicationObjectsService As New GetGlobalObjectsService
                objGetApplicationObjectsService.GetAllApplicationObjects()  'this will create/populate all application objects.
            End If
            Return DirectCast(Application("ApplWebCatsObject"), List(Of WebCat))
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            PopulateStatus()
            PopulateCategories()
            PopulateWeekDropdown()
            Submit.Enabled = False
            ExportToExcelButton.Enabled = False
            ErrorLabel.Text = ""
            RadUPCGrid.Visible = False
            Submit.Visible = False
        End If
    End Sub

    Private Sub PopulateCategories()
        ddlCategoryDropDown.Items.Clear()

        Dim _TUEcommPrioritization As New TUEcommPrioritization
        Dim list As IList(Of String) = CType(_TUEcommPrioritization.GetExportCategories(CStr(ddlStatusDropDown.SelectedValue)), Global.System.Collections.Generic.IList(Of String))

        If list.Count > 0 Then
            Dim i As Integer = 0
            For Each key As String In list
                ddlCategoryDropDown.Items.Insert(i, New ListItem(SetDefaultCategory(CInt(key)), CStr(key)))
                i += 1
            Next
            ddlCategoryDropDown.Items.Insert(0, "------Select a category -----")
        Else
            ddlCategoryDropDown.Enabled = True
            Submit.Enabled = True
        End If
    End Sub

    Private Sub PopulateWeekDropdown()
        ddlTurnInWeek.Visible = False
        'ddlTurnInWeek.Items.Clear()
        'Dim _TUAdInfo As New TUAdInfo
        'With ddlTurnInWeek
        '    .DataSource = _TUAdInfo.GetAllFromAdInfoFiltered(True).ToList()
        '    .DataValueField = "adnbr"
        '    .DataTextField = "TurnInWeek"
        '    .DataBind()
        '    .Items.Insert(0, "")
        'End With
    End Sub

    Private Sub RadUPCGrid_ExportCellFormatting(sender As Object, e As Telerik.Web.UI.ExportCellFormattingEventArgs) Handles RadUPCGrid.ExportCellFormatting
        If e.FormattedColumn.UniqueName = "UPC" Then
            e.Cell.Style("mso-number-format") = "0000"
        End If
    End Sub

    'Private Sub RadUPCGrid_GridExporting(sender As Object, e As Telerik.Web.UI.GridExportingArgs) Handles RadUPCGrid.GridExporting
    '    If e.ExportType = ExportType.Excel Then
    '        Dim css As String = "<style type='text/css'> br { mso-data-placement: same-cell; } </style>"
    '        e.ExportOutput = e.ExportOutput.Replace("</head>", css + "</head>")
    '    End If
    'End Sub
    Private Sub RadUPCGrid_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles RadUPCGrid.ItemDataBound
        If (TypeOf e.Item Is GridDataItem And Not e.Item.IsInEditMode) Then 'first level data bind 
            Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)
            CType(dataItem("WebCategories").FindControl("ltrPrimaryWebCategory"), Literal).Text = SetDefaultCategory(CInt(ddlCategoryDropDown.SelectedValue))

            Dim Item As GridDataItem = DirectCast(e.Item, GridDataItem)
            Item("MerchantNotes").Text = Regex.Replace(Item("MerchantNotes").Text.Replace("<br />", "").Replace(vbCrLf, "").Replace(vbCr, "").Replace("vbCrLf", ""), "<.*?>", "  ")
        End If


    End Sub

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


    Private Sub RadUPCGrid_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadUPCGrid.NeedDataSource
        RadUPCGrid.MasterTableView.DataSource = GetPrioritizationData(ddlCategoryDropDown.SelectedItem.Value, ddlStatusDropDown.SelectedValue)
    End Sub

    Private Sub LoadUPCGrid()
        RadUPCGrid.MasterTableView.DataSource = GetPrioritizationData(ddlCategoryDropDown.SelectedItem.Value, ddlStatusDropDown.SelectedValue)
    End Sub

    Private Function GetPrioritizationData(ByVal CategoryID As String, ByVal Status As String) As List(Of ECommPrioritizationInfo)
        If CategoryID <> " -- Select Category -- " Then
            Dim _TUEcommPrioritization As New TUEcommPrioritization
            Dim DB2Results As List(Of ECommPrioritizationInfo) = _TUEcommPrioritization.GetPrioritizationForExcel(CategoryID, Status).ToList
            Return GetAdminImageNotes(DB2Results).ToList
        Else
            Return Nothing
        End If
    End Function

    Private Function GetAdminImageNotes(ByVal DB2Results As List(Of ECommPrioritizationInfo)) As List(Of ECommPrioritizationInfo)
        If DB2Results.Count > 0 Then
            Dim distinctAdminImageNotes As String = CommonBO.FormulateAdminXML(DB2Results.Select(Function(x) CStr(x.AdNbrAdminImgNbr)).Distinct.ToList)
            Dim dalSQL As SqlDAL.CtlgAdPg = New SqlDAL.CtlgAdPg
            Dim SQLResults As IList(Of AdminImageNotesInfo) = dalSQL.GetAdminImageNotes(distinctAdminImageNotes)
            Dim typedresults As List(Of ECommPrioritizationInfo) = DB2Results.Select(Function(x) New ECommPrioritizationInfo With { _
                                .ISN = x.ISN, _
                                .TurnInMerchID = x.TurnInMerchID, _
                                .IsValidFlg = x.IsValidFlg, _
                                .FeatureID = x.FeatureID, _
                                .ImageID = x.ImageID, _
                                .WebCatSizeDesc = x.WebCatSizeDesc, _
                                .ProductName = x.ProductName, _
                                .FriendlyColor = x.FriendlyColor, _
                                .NonSwatchClrCde = x.NonSwatchClrCde, _
                                .NonSwatchClrDesc = x.NonSwatchClrDesc, _
                                .ColorFamily = x.ColorFamily, _
                                .VtPath = x.VtPath, _
                                .AdNbrAdminImgNbr = x.AdNbrAdminImgNbr, _
                                .UPC = x.UPC, _
                                .DropShipID = x.DropShipID, _
                                .IntReturnInstrct = x.IntReturnInstrct, _
                                .ExtReturnInstrct = x.ExtReturnInstrct, _
                                .BrandDesc = x.BrandDesc, _
                                .VendorStyleNumber = x.VendorStyleNumber, _
                                .SizeFamily = x.SizeFamily, _
                                .GenderDesc = x.GenderDesc, _
                                .AgeDesc = x.AgeDesc, _
                                .FRS = x.FRS, _
                                .EMMNotes = x.EMMNotes, _
                                .MerchantNotes = x.MerchantNotes, _
                                .ImageSuffix = If(SQLResults.Where(Function(y) y.AdNbrAdminImgNbr = x.AdNbrAdminImgNbr).Count > 0, SQLResults.Where(Function(y) y.AdNbrAdminImgNbr = x.AdNbrAdminImgNbr)(0).ImageSuffix, ""), _
                                .ImageNotes = If(SQLResults.Where(Function(z) z.AdNbrAdminImgNbr = x.AdNbrAdminImgNbr).Count > 0, SQLResults.Where(Function(z) z.AdNbrAdminImgNbr = x.AdNbrAdminImgNbr)(0).ImageNotes, "")}).ToList

            Return typedresults
        Else
            Return DB2Results
        End If
    End Function

    Private Sub PopulateStatus()
        Dim StatusHash As New Hashtable
        StatusHash.Add("P", "Pending")
        'StatusHash.Add("F", "Failed Upload")
        'StatusHash.Add("U", "Upload Success")
        'StatusHash.Add("D", "Deleted")
        ddlStatusDropDown.DataSource = StatusHash
        ddlStatusDropDown.DataTextField = "value"
        ddlStatusDropDown.DataValueField = "key"
        ddlStatusDropDown.DataBind()
        ddlStatusDropDown.SelectedValue = "P"
    End Sub


    Private Sub ExportToExcelButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ExportToExcelButton.Click
        RadUPCGrid.ExportSettings.IgnorePaging = True
        RadUPCGrid.ExportSettings.ExportOnlyData = True
        RadUPCGrid.ExportSettings.OpenInNewWindow = True
        RadUPCGrid.ExportSettings.FileName = ddlCategoryDropDown.SelectedItem.Text.Replace(" ", "_") + "_" + DateTime.Now.Date.ToString("ddMM")
        RadUPCGrid.MasterTableView.ExportToExcel()
    End Sub

    Private Sub Submit_Click(sender As Object, e As System.EventArgs) Handles Submit.Click
        Dim log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
        Try
            RadUPCGrid.Rebind()
            RadUPCGrid.Visible = True
            ExportToExcelButton.Enabled = True
        Catch ex As Exception
            log.Error(ex.Message)
            ErrorLabel.Text = ex.Message
        Finally
            log = Nothing
        End Try
    End Sub

    Private Sub Clear_Click(sender As Object, e As System.EventArgs) Handles Clear.Click
        PopulateCategories()
        CategoriesLabel.Text = ""
        RadUPCGrid.Visible = False
        Submit.Visible = False
        ExportToExcelButton.Enabled = False
        ErrorLabel.Text = ""
    End Sub

    Private Sub ddlCategoryDropDown_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCategoryDropDown.SelectedIndexChanged
        CategoriesLabel.Text = ddlCategoryDropDown.SelectedItem.Text & " > "
        RadUPCGrid.Rebind()
        RadUPCGrid.Visible = True
        ExportToExcelButton.Enabled = True
        Submit.Visible = False
    End Sub
End Class