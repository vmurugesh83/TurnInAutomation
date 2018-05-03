Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL.MiscConstants
Imports System.Data.SqlClient

Public Class AdPageSetupCtrl
    Inherits System.Web.UI.UserControl

    Public Delegate Sub PreviousPageUrl()
    Public Event PreviousPageUrlEvent As PreviousPageUrl

    Private _IsEcommerce As Boolean = False
    Private _adPageSetUpSearchCtrl As AdPageSetUpSearchCtrl = Nothing

#Region "Properties"

    Public ReadOnly Property Action() As String
        Get
            Try
                Return UCase(Request.QueryString("ACTION"))
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property

    Public Property IsEcommerce() As Boolean
        Get
            Return _IsEcommerce
        End Get
        Set(ByVal value As Boolean)
            _IsEcommerce = value
        End Set
    End Property

    Public Property AdPageSetUpSearchCtrl() As AdPageSetUpSearchCtrl
        Get
            Return _adPageSetUpSearchCtrl
        End Get
        Set(ByVal value As AdPageSetUpSearchCtrl)
            _adPageSetUpSearchCtrl = value
        End Set
    End Property

    Public Property AdPageSetupPages() As List(Of CtlgAdPgInfo)
        Get
            Try
                Dim _TuCtlgAdPg As New TUCtlgAdPg
                If Session("AdPageSetupCtrl.AdPageSetupPages") Is Nothing Then
                    Session("AdPageSetupCtrl.AdPageSetupPages") = _TuCtlgAdPg.GetAllFromCtlgAdPg(AdPageSetUpSearchCtrl.SelectedAd).ToList()
                End If

                Return DirectCast(Session("AdPageSetupCtrl.AdPageSetupPages"), List(Of CtlgAdPgInfo)).OrderBy(Function(p) p.pgnbr).ToList()

            Catch ex As Exception
                Return New List(Of CtlgAdPgInfo)
            End Try
        End Get
        Set(ByVal value As List(Of CtlgAdPgInfo))
            Session("AdPageSetupCtrl.AdPageSetupPages") = value
        End Set
    End Property
#End Region

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If IsEcommerce Then
                lblPageHeader.Text = "E-Comm"
                btnSave.Visible = False
            Else
                lblPageHeader.Text = "Print"
            End If
            lblPageHeader.Text &= " Ad Page Setup "
            If Action = Enumerations.Modes.MAINTENANCE.ToString Then
                lblPageHeader.Text &= "- Maintenance"
            ElseIf Action = Enumerations.Modes.INQUIRY.ToString Then
                lblPageHeader.Text &= "- Inquiry"
            End If
        End If

    End Sub

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

    Private Sub grdAdPageSetup_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdAdPageSetup.ItemCommand
        If e.CommandName = RadGrid.PerformInsertCommandName Then
            If Not Page.IsValid Then
                HandleValidation(e)
                Return
            Else
                Dim pg As New CtlgAdPgInfo(AdPageSetUpSearchCtrl.SelectedAd, CInt(DirectCast(e.Item.FindControl("SystemPageNumber"), Label).Text), DirectCast(e.Item.FindControl("txtDescription"), RadTextBox).Text, CInt(DirectCast(e.Item.FindControl("txtPageOrder"), RadTextBox).Text))
                pg.state = "Insert"
                Dim pages As List(Of CtlgAdPgInfo) = AdPageSetupPages
                pages.Add(pg)
                AdPageSetupPages = pages
                lblMasterPageText.Text = DirectCast(e.Item.FindControl("SystemPageNumber"), Label).Text
            End If
        ElseIf e.CommandName = RadGrid.UpdateCommandName Then
            If Not Page.IsValid Then
                HandleValidation(e)
                Return
            Else
                Dim pg As CtlgAdPgInfo = AdPageSetupPages.Where(Function(a) a.syspgnbr = CInt(DirectCast(e.Item.FindControl("SystemPageNumber"), Label).Text)).FirstOrDefault()
                pg.pgdesc = DirectCast(e.Item.FindControl("txtDescription"), RadTextBox).Text
                pg.pgnbr = CInt(DirectCast(e.Item.FindControl("txtPageOrder"), RadTextBox).Text)
                pg.state = "Update"
            End If
        ElseIf e.CommandName = RadGrid.InitInsertCommandName Then
            grdAdPageSetup.MasterTableView.ClearEditItems()
        ElseIf e.CommandName = RadGrid.EditCommandName Then
            grdAdPageSetup.MasterTableView.IsItemInserted = False
        End If
        BindGrid()
    End Sub

    Private Sub grdAdPageSetup_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdAdPageSetup.PageIndexChanged
        grdAdPageSetup.Rebind()
    End Sub

    Private Sub grdeCommTurnInMaint_SortCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridSortCommandEventArgs) Handles grdAdPageSetup.SortCommand
        grdAdPageSetup.Rebind()
    End Sub

    Private Sub rtbAdPageSetup_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles rtbAdPageSetup.ButtonClick
        If TypeOf e.Item Is RadToolBarButton Then
            Dim _TUAdInfo As New TUAdInfo
            Dim _TUCtlgAdPg As New TUCtlgAdPg
            Dim radToolBarButton As RadToolBarButton = DirectCast(e.Item, RadToolBarButton)
            ClearMessagePanel()

            Select Case radToolBarButton.CommandName

                Case "Retrieve"
                    Session("AdPageSetupCtrl.AdPageSetupPages") = Nothing
                    Dim ad As AdInfoInfo = _TUAdInfo.GetAdInfoByAdNbr(AdPageSetUpSearchCtrl.SelectedAd)
                    If ad IsNot Nothing Then
                        lblAdNoText.Text = CStr(ad.adnbr)
                        lblAdNoDescText.Text = ad.addesc
                        lblRunStartText.Text = ad.adrunstartdt.ToShortDateString
                        If ad.adrunenddt.ToShortDateString <> "1/1/1900" Then
                            lblRunEndText.Text = ad.adrunenddt.ToShortDateString
                        Else
                            lblRunEndText.Text = ""
                        End If
                        lblBasePageText.Text = CStr(ad.basepages)
                        lblMasterPageText.Text = CStr(ad.masterpages)

                        Reset(True)
                        BindGrid()

                    End If

                Case "Reset"
                    Reset(False)
                    Me.AdPageSetUpSearchCtrl.ResetControls()
                Case "Save"
                    Dim addItems As List(Of CtlgAdPgInfo) = AdPageSetupPages.Where(Function(ad) ad.state = "Insert").ToList()
                    If addItems.Count > 0 Then
                        For Each pg As CtlgAdPgInfo In addItems
                            _TUCtlgAdPg.InsertCtlgAdPg(pg.adnbr, pg.syspgnbr, pg.pgdesc, pg.pgnbr)
                        Next
                    End If
                    Dim updateItems As List(Of CtlgAdPgInfo) = AdPageSetupPages.Where(Function(ad) ad.state = "Update").ToList()
                    If updateItems.Count > 0 Then
                        For Each pg As CtlgAdPgInfo In updateItems
                            _TUCtlgAdPg.UpdateCtlgAdPg(pg)
                        Next
                    End If
                    Session("AdPageSetupCtrl.AdPageSetupPages") = Nothing
                    BindGrid()
                    MessagePanel1.GeneralMessage = PageBase.GetValidationMessage(MessageCode.GenericInformational001)

                Case "Back"
                    RaiseEvent PreviousPageUrlEvent()
            End Select
        End If

    End Sub

    Private Sub Reset(ByVal TrueORFalse As Boolean)
        lblAdNoLabel.Visible = TrueORFalse
        lblRunStartLabel.Visible = TrueORFalse
        lblBasePageLabel.Visible = TrueORFalse
        lblRunEndLabel.Visible = TrueORFalse
        lblMasterPageLabel.Visible = TrueORFalse
        grdAdPageSetup.Visible = TrueORFalse
        btnSave.Enabled = False

        If TrueORFalse = False Then
            lblAdNoText.Text = ""
            lblAdNoDescText.Text = ""
            lblRunStartText.Text = ""
            lblBasePageText.Text = ""
            lblRunEndText.Text = ""
            lblMasterPageText.Text = ""
        End If

    End Sub

    Private Sub BindGrid()
        grdAdPageSetup.DataSource = AdPageSetupPages
        grdAdPageSetup.DataBind()

        If Action = Enumerations.Modes.INQUIRY.ToString Or IsEcommerce Then
            grdAdPageSetup.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None
            grdAdPageSetup.MasterTableView.Columns(0).Visible = False
            grdAdPageSetup.Rebind()
        Else
            btnSave.Enabled = True
        End If

    End Sub

    Public Function GetIncrementedSysPage() As Integer
        Return CInt(lblMasterPageText.Text) + 1
    End Function

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
            Dim SystemPageNumber As Integer = CInt(DirectCast(validator.Parent.Parent.FindControl("SystemPageNumber"), Label).Text)
            Dim duplicate As CtlgAdPgInfo = AdPageSetupPages.Where(Function(t) t.pgnbr = CInt(value) And t.syspgnbr <> SystemPageNumber).FirstOrDefault()
            If duplicate IsNot Nothing Then
                MessagePanel1.WarningMessage = String.Format(PageBase.GetValidationMessage(MessageCode.TurnInError1002), SystemPageNumber, duplicate.syspgnbr)
            End If
        End If
    End Sub


#End Region
End Class