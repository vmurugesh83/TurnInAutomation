Partial Public Class SessionExpired
    Inherits System.Web.UI.Page

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Master.SessionTimeoutEnabled = False

        Session.Abandon()
    End Sub

#End Region
    

End Class