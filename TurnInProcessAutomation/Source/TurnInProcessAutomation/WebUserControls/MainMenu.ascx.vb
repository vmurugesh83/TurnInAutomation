Partial Class WebUserControls_MainMenu
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'bind xml menu items
        Try
            xmlMenuItems.Data = SessionWrapper.XMLMenu
            rmMain.DataSource = xmlMenuItems
            rmMain.DataBind()

        Catch ex As Exception

        End Try
    End Sub

End Class
