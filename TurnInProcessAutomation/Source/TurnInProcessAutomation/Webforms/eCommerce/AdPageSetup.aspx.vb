Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL.MiscConstants

Public Class AdPageSetupEcomm
    Inherits PageBase

    Private _adPageSetUpSearchCtrl As AdPageSetUpSearchCtrl = Nothing

    Public ReadOnly Property AdPageSetUpSearchCtrl() As AdPageSetUpSearchCtrl
        Get
            Dim control As Control = Me.Master.SideBarPlaceHolder.FindControl("AdPageSetUpSearchCtrl1")
            Me._adPageSetUpSearchCtrl = DirectCast(control, AdPageSetUpSearchCtrl)
            Return _adPageSetUpSearchCtrl
        End Get
    End Property

    Sub goBack() Handles AdPageSetupCtrl1.PreviousPageUrlEvent
        Response.Redirect(PreviousPageUrl)
    End Sub

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim control As Control = LoadControl("~/WebUserControls/Maintenance/AdPageSetUpSearchCtrl.ascx")
        If Not control Is Nothing Then
            control.ID = "AdPageSetUpSearchCtrl1"
            Me.Master.SideBarPlaceHolder.Controls.Add(control)
        End If

        Me.AdPageSetUpSearchCtrl.IsEcommerce = True
        Me.AdPageSetupCtrl1.IsEcommerce = True
        Me.AdPageSetupCtrl1.AdPageSetUpSearchCtrl = AdPageSetUpSearchCtrl
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.Master.SideBar.Width = 200
        End If
    End Sub

End Class