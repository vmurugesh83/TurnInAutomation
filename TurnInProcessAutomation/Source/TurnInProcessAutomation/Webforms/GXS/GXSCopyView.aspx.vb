Public Class GXSCopyView
    Inherits PageBase

    Private _gxsCopyViewSearchCtrl As GXSCopyViewSearchCtrl = Nothing

    Public ReadOnly Property GXSCopyViewSearchCtrl() As GXSCopyViewSearchCtrl
        Get
            Dim control As Control = Me.Master.SideBarPlaceHolder.FindControl("GXSCopyViewSearchCtrl1")
            Me._gxsCopyViewSearchCtrl = DirectCast(control, GXSCopyViewSearchCtrl)
            Return _gxsCopyViewSearchCtrl
        End Get
    End Property

    'Sub goBack() Handles GXSCopyViewSearchCtrl1.PreviousPageUrlEvent
    '    Response.Redirect(PreviousPageUrl)
    'End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then

                'If (Request.QueryString("CurrentISN") Is Nothing) Then
                '    ResultsTabISNs = Nothing
                '    SelectedMerch = Nothing

                'End If

            End If

            Dim control As Control = LoadControl("~/WebUserControls/GXS/GXSCopyViewSearchCtrl.ascx")
            If Not control Is Nothing Then
                control.ID = "GXSCopyViewSearchCtrl1"
                Me.Master.SideBarPlaceHolder.Controls.Add(control)
                Me.Master.SideBar.Width = Unit.Pixel(240)
            End If
            Me.GXSCopyViewCtrl1.GXSCopyViewSearchCtrl = CType(control, GXSCopyViewSearchCtrl)

        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

End Class