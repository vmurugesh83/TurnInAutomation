Public Partial Class LogOff
    Inherits System.Web.UI.Page

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Try
            Dim script As String = String.Format("if(!window.close())window.location= '{0}';", Me.Page.ResolveUrl("~/Default.aspx"))
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Redirect", script, True)
        Catch ex As Exception
        End Try
    End Sub

#End Region

    


End Class