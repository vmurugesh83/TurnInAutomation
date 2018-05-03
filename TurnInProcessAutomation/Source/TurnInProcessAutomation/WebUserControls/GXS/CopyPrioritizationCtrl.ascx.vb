Option Infer On

Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL.MiscConstants
Imports System.Data.SqlClient

Public Class CopyPrioritizationCtrl
    Inherits System.Web.UI.UserControl

    Public Delegate Sub PreviousPageUrl()
    Public Event PreviousPageUrlEvent As PreviousPageUrl

    Private _copyPrioritizationSearchCtrl As CopyPrioritizationSearchCtrl = Nothing

#Region "Properties"

    Public Property CopyPrioritizationSearchCtrl() As CopyPrioritizationSearchCtrl
        Get
            Return _copyPrioritizationSearchCtrl
        End Get
        Set(ByVal value As CopyPrioritizationSearchCtrl)
            _copyPrioritizationSearchCtrl = value
        End Set
    End Property

    Private Property Datasource() As List(Of GXSCopyViewInfo)
        Get
            Return CType(Session("GXSCopyView.Datasource"), List(Of GXSCopyViewInfo))
        End Get
        Set(value As List(Of GXSCopyViewInfo))
            Session("GXSCopyView.Datasource") = value
        End Set
    End Property
#End Region

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Ajaxify Search control for reseting controls in LHN.
        'Dim ajaxSett As New AjaxSetting(Me.rtbGXSCopyView.UniqueID)
        'ajaxSett.UpdatedControls.Add(New AjaxUpdatedControl(Me.CopyPrioritizationSearchCtrl.UniqueID, String.Empty))
        'RadAjaxManagerProxy1.AjaxSettings.Add(ajaxSett)

        If Not Page.IsPostBack Then

            lblPageHeader.Text = "Copy Prioritization"

        End If

        'Create initial xml file
        'BuildDataTable()

    End Sub

    'Private Sub BuildDataTable()

    '    Dim dt As New DataTable("CatalogViews")
    '    dt.Columns.Add("View", GetType(String))
    '    dt.Columns.Add("CRG_ID", GetType(Integer))
    '    dt.Columns.Add("CMG_ID", GetType(Integer))
    '    dt.Columns.Add("CFG_ID", GetType(Integer))
    '    dt.Columns.Add("DEPT_ID", GetType(Integer))

    '    dt.Rows.Add("Apparel", 100, 0, 0, 0)
    '    dt.Rows.Add("Apparel", 200, 0, 0, 0)
    '    dt.Rows.Add("Apparel", 300, 0, -190, 0)
    '    dt.Rows.Add("Apparel", 300, 509, 190, 45)
    '    dt.Rows.Add("Apparel", 300, 509, 190, 62)
    '    dt.Rows.Add("Apparel", 500, 410, 0, 0)
    '    dt.Rows.Add("Footwear", 500, 412, 0, 0)
    '    dt.Rows.Add("Footwear", 300, 509, 190, 436)
    '    dt.Rows.Add("Footwear", 300, 509, 190, 758)
    '    dt.Rows.Add("Jewelry", 500, 411, 240, 0)
    '    dt.Rows.Add("Jewelry", 900, 0, 0, 0)
    '    dt.Rows.Add("Accessories", 500, 411, 225, 0)
    '    dt.Rows.Add("Accessories", 500, 411, 230, 0)
    '    dt.Rows.Add("Accessories", 300, 509, 190, 23)
    '    dt.Rows.Add("Accessories", 300, 509, 190, 27)
    '    dt.Rows.Add("Accessories", 300, 509, 190, 242)
    '    dt.Rows.Add("Accessories", 300, 509, 190, 273)
    '    dt.Rows.Add("Accessories", 300, 509, 190, 435)
    '    dt.Rows.Add("Beauty", 400, 0, 0, 0)
    '    dt.Rows.Add("Home", 600, 0, 0, 0)
    '    dt.Rows.Add("Home", 300, 509, 190, 98)

    '    Dim file As String = Server.MapPath("..\..\WebUserControls\GXS") & "\GXSCatalogViews.xml"
    '    dt.WriteXml(file, XmlWriteMode.WriteSchema)
    '    Dim dtNew As New DataTable
    '    dtNew.ReadXml(file)

    'End Sub

    Private Sub HandleValidation(ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Dim ttvValidateDescription As ToolTipValidator = DirectCast(e.Item.FindControl("ttvValidateDescription"), ToolTipValidator)
        Dim ttvValidatePageOrder As ToolTipValidator = DirectCast(e.Item.FindControl("ttvValidatePageOrder"), ToolTipValidator)
        If Not ttvValidateDescription.IsValid And Not ttvValidatePageOrder.IsValid Then
            MessagePanel1.ErrorMessage = ErrorOnPage
        Else
            If Not ttvValidateDescription.IsValid Then
                MessagePanel1.ErrorMessage = ttvValidateDescription.ErrorMessage
            Else
                MessagePanel1.ErrorMessage = ttvValidatePageOrder.ErrorMessage
            End If
        End If
        e.Canceled = True

    End Sub

    Private Sub grdGXSCopyView_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grdGXSCopyView.NeedDataSource
        If Datasource IsNot Nothing Then
            grdGXSCopyView.DataSource = GroupISNData(Datasource)
        End If
    End Sub


    Private Sub grdGXSCopyView_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grdGXSCopyView.PageIndexChanged
        grdGXSCopyView.Rebind()
    End Sub

    Private Function GetComboInt16(ByVal cmb As RadComboBox) As Int16
        Return If(cmb.SelectedValue = "", Short.Parse("0"), CType(cmb.SelectedValue, Int16))
    End Function

    Private Function GetComboInteger(ByVal cmb As RadComboBox) As Integer
        Return If(cmb.SelectedValue = "", 0, CInt(cmb.SelectedValue))
    End Function

    Private Function GetDatePickerDate(ByVal dp As RadDatePicker) As Date
        Return If(dp.SelectedDate Is Nothing, Date.MinValue, CDate(dp.SelectedDate))
    End Function

    Private Sub rtbGXSCopyView_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles rtbGXSCopyView.ButtonClick
        If TypeOf e.Item Is RadToolBarButton Then
            Dim _TUGXSInfo As New TUGXSCopyView
            Dim radToolBarButton As RadToolBarButton = DirectCast(e.Item, RadToolBarButton)
            ClearMessagePanel()

            'Select Case radToolBarButton.CommandName

            '    Case "Retrieve"
            '        Session("GXSCopyViewCtrl.ISNs") = Nothing
            '        Dim gxs As IList(Of GXSCopyViewInfo) = Nothing
            '        Select Case GXSCopyViewSearchCtrl.SelectedPanel
            '            Case "Find By Hierarchy"
            '                Dim dept As Int16 = GetComboInt16(GXSCopyViewSearchCtrl.cmbDept)
            '                Dim cla As Int16 = GetComboInt16(GXSCopyViewSearchCtrl.cmbClass)
            '                Dim ven As Integer = GetComboInteger(GXSCopyViewSearchCtrl.cmbVendor)
            '                Dim venS As String = GXSCopyViewSearchCtrl.cmbVendorStyle.SelectedValue

            '                'For quick testing
            '                'dept = 35
            '                'cla = 200
            '                'ven = 927
            '                'venS = "9450"

            '                If dept = 0 Then
            '                    'Message
            '                    MessagePanel1.ErrorMessage = "One or more criteria need to be selected"
            '                Else
            '                    gxs = _TUGXSInfo.FindByHierarchy(dept, cla, ven, venS)
            '                End If
            '            Case "Find By Item(s)"
            '                Dim isnXml As String = GXSCopyViewSearchCtrl.GetIsnXml()
            '                Dim skuUpcXml As String = GXSCopyViewSearchCtrl.GetSkuUpcXml()
            '                If GXSCopyViewSearchCtrl.SelectedISNs.Count = 0 AndAlso GXSCopyViewSearchCtrl.SelectedSKUs.Count = 0 Then
            '                    'Message
            '                    MessagePanel1.ErrorMessage = "One or more criteria need to be selected"
            '                Else
            '                    gxs = _TUGXSInfo.FindByItems(isnXml, skuUpcXml)
            '                End If
            '            Case "Find By Ads"
            '                Dim adnum As Integer = If(GXSCopyViewSearchCtrl.cmbAds.Text = "", 0, CInt(GXSCopyViewSearchCtrl.cmbAds.Text))
            '                Dim pageNum As Int16 = GetComboInt16(GXSCopyViewSearchCtrl.cmbPageNumber)
            '                Dim batchNum As Integer = GetComboInteger(GXSCopyViewSearchCtrl.cmbBatch)
            '                If adnum = 0 Then
            '                    'Message
            '                    MessagePanel1.ErrorMessage = "One or more criteria need to be selected"
            '                Else
            '                    gxs = _TUGXSInfo.FindByAds(adnum, "", batchNum)
            '                End If

            '            Case "Find By Label"
            '                Dim labelId As Integer = GetComboInteger(GXSCopyViewSearchCtrl.cmbLabel)
            '                If labelId = 0 Then
            '                    'Message
            '                    MessagePanel1.ErrorMessage = "One or more criteria need to be selected"
            '                Else
            '                    gxs = _TUGXSInfo.FindByLabel(labelId)
            '                End If
            '            Case "Find By PO Start Ship Date"
            '                Dim startShipDate As Date = GetDatePickerDate(GXSCopyViewSearchCtrl.dpFrom)
            '                Dim endShipDate As Date = GetDatePickerDate(GXSCopyViewSearchCtrl.dpTo)
            '                If startShipDate = Date.MinValue Then
            '                    'Message
            '                    MessagePanel1.ErrorMessage = "One or more criteria need to be selected"
            '                Else
            '                    gxs = _TUGXSInfo.FindByStartShipDate(startShipDate, endShipDate)
            '                End If
            '        End Select
            '        If gxs IsNot Nothing Then

            '            Reset(True)
            '            'BindGrid()

            '            Datasource = gxs.ToList

            '            grdGXSCopyView.DataSource = GroupISNData(gxs)
            '            grdGXSCopyView.DataBind()

            '        Else

            '            Datasource = Nothing

            '            grdGXSCopyView.Visible = False

            '        End If

            '    Case "Reset"
            '        Reset(False)
            '        Me.GXSCopyViewSearchCtrl.ResetControls()

            '    Case "Back"
            '        RaiseEvent PreviousPageUrlEvent()
            'End Select
        End If

    End Sub

    Private Sub Reset(ByVal TrueORFalse As Boolean)
        'lblAdNoLabel.Visible = TrueORFalse
        'lblRunStartLabel.Visible = TrueORFalse
        'lblBasePageLabel.Visible = TrueORFalse
        'lblRunEndLabel.Visible = TrueORFalse
        'lblMasterPageLabel.Visible = TrueORFalse
        grdGXSCopyView.Visible = TrueORFalse
        'btnSave.Enabled = False

        'If TrueORFalse = False Then
        '    lblAdNoText.Text = ""
        '    lblAdNoDescText.Text = ""
        '    lblRunStartText.Text = ""
        '    lblBasePageText.Text = ""
        '    lblRunEndText.Text = ""
        '    lblMasterPageText.Text = ""
        'End If

    End Sub

    Private Sub BindGrid()
        'grdGXSCopyView.DataSource = AdPageSetupPages
        'grdGXSCopyView.DataBind()

        'If Action = Enumerations.Modes.INQUIRY.ToString Or IsEcommerce Then
        '    grdAdPageSetup.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None
        '    grdAdPageSetup.MasterTableView.Columns(0).Visible = False
        '    grdAdPageSetup.Rebind()
        'Else
        '    btnSave.Enabled = True
        'End If

    End Sub

    'Public Function GetIncrementedSysPage() As Integer
    '    Return CInt(lblMasterPageText.Text) + 1
    'End Function

    Private Sub ClearMessagePanel()
        MessagePanel1.ErrorMessage = ""
    End Sub

#Region "Validations"

    Public Sub ttvValidateDescription_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim ttvValidateDescription As ToolTipValidator = DirectCast(source, ToolTipValidator)
        If String.IsNullOrEmpty(DirectCast(ttvValidateDescription.NamingContainer.FindControl("txtDescription"), RadTextBox).Text) Then
            ttvValidateDescription.ErrorMessage = PageBase.GetValidationMessage(MessageCode.GenericError006)
            args.IsValid = False
        End If
    End Sub

    Public Sub ttvValidatePageOrder_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim validator As ToolTipValidator = DirectCast(source, ToolTipValidator)
        Dim value As String = DirectCast(validator.EvaluatedControl, RadTextBox).Text.Trim.ToUpper
        If String.IsNullOrEmpty(value) Then
            validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1001)
            args.IsValid = False
        ElseIf Not Integer.TryParse(value, Nothing) Then
            validator.ErrorMessage = PageBase.GetValidationMessage(MessageCode.TurnInError1005)
            args.IsValid = False
        Else
            'Dim SystemPageNumber As Integer = CInt(DirectCast(validator.Parent.Parent.FindControl("SystemPageNumber"), Label).Text)
            'Dim duplicate As CtlgAdPgInfo = AdPageSetupPages.Where(Function(t) t.pgnbr = CInt(value) And t.syspgnbr <> SystemPageNumber).FirstOrDefault()
            'If duplicate IsNot Nothing Then
            '    MessagePanel1.WarningMessage = String.Format(PageBase.GetValidationMessage(MessageCode.TurnInError1002), SystemPageNumber, duplicate.syspgnbr)
            'End If
        End If
    End Sub

    Private Sub grdGXSCopyView_DetailTableDataBind(sender As Object, e As GridDetailTableDataBindEventArgs) Handles grdGXSCopyView.DetailTableDataBind
        'e.DetailTableView.DataSource = (From c In Datasource
        '                                Select c
        '                                Where c.INTERNAL_STYLE_NUM = CDec(e.DetailTableView.ParentItem.GetDataKeyValue("INTERNAL_STYLE_NUM")) And c.COLOR = CStr(e.DetailTableView.ParentItem.GetDataKeyValue("FEATUREDCOLOR")) And c.PO_STARTSHIPDT = e.DetailTableView.ParentItem.GetDataKeyValue("PO_STARTSHIPDT").ToString).ToList
        e.DetailTableView.DataSource = GroupUPCData(Datasource, CDec(e.DetailTableView.ParentItem.GetDataKeyValue("INTERNAL_STYLE_NUM")), e.DetailTableView.ParentItem.GetDataKeyValue("COLOR").ToString)
    End Sub

    Private Function GroupISNData(ByVal gxsInfo As IList(Of GXSCopyViewInfo)) As IList(Of GXSCopyViewInfo)
        If gxsInfo.Count > 0 Then
            Dim isns = From a In gxsInfo _
                       Group a By a.INTERNAL_STYLE_NUM, a.IMAGE_ID, a.LABEL, a.PRODUCT_NAME, a.PO_STARTSHIPDT, a.PRICESTATUS, a.FEATUREDCOLOR, a.COLOR Into Group _
                       Select New With {
                        .IMAGE_ID = IMAGE_ID, _
                        .INTERNAL_STYLE_NUM = INTERNAL_STYLE_NUM, _
                        .LABEL = LABEL, _
                        .PRODUCT_NAME = PRODUCT_NAME, _
                        .PO_STARTSHIPDT = PO_STARTSHIPDT, _
                        .PRICESTATUS = PRICESTATUS, _
                        .FEATUREDCOLOR = FEATUREDCOLOR, _
                        .COLOR = COLOR, _
                        .OO = Group.Sum(Function(x) x.OO), _
                        .OH = Group.Sum(Function(x) x.OH)
                       }

            gxsInfo = New List(Of GXSCopyViewInfo)
            For Each isn In isns
                gxsInfo.Add(New GXSCopyViewInfo(isn.IMAGE_ID, isn.INTERNAL_STYLE_NUM, isn.LABEL, isn.PRODUCT_NAME, isn.OO, isn.OH, isn.PO_STARTSHIPDT, isn.PRICESTATUS, isn.FEATUREDCOLOR, isn.COLOR))
            Next
        End If
        Return gxsInfo
    End Function

    Private Function GroupUPCData(ByVal gxsInfo As IList(Of GXSCopyViewInfo), ByVal isn As Decimal, ByVal FColor As String) As IList(Of GXSCopyViewInfo)
        If gxsInfo.Count > 0 Then
            Dim upcs = From u In Datasource _
                       Where u.INTERNAL_STYLE_NUM = isn
                       Group u By u.IMAGE_ID, u.INTERNAL_STYLE_NUM, u.UPC_NUM, u.FEATURE, u.COLOR, u.SIZE Into Group _
                       Select New With {
                        .IMAGE_ID = IMAGE_ID, _
                        .INTERNAL_STYLE_NUM = INTERNAL_STYLE_NUM, _
                        .UPC_NUM = UPC_NUM, _
                        .FEATURE = FEATURE, _
                        .COLOR = COLOR, _
                        .SIZE = SIZE
                       }

            gxsInfo = New List(Of GXSCopyViewInfo)
            For Each upc In upcs
                If Trim(FColor) = Trim(upc.COLOR) Or FColor = "" Then
                    gxsInfo.Add(New GXSCopyViewInfo(upc.IMAGE_ID, upc.INTERNAL_STYLE_NUM, upc.UPC_NUM, upc.FEATURE, upc.COLOR, upc.SIZE))
                End If
            Next
        End If
        Return gxsInfo
    End Function

#End Region
End Class