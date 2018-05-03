Partial Public Class _Default
    Inherits System.Web.UI.Page



#Region "Members"

    Private _environment As String = ConfigurationManager.AppSettings("Environment").ToString()

#End Region


#Region "Events"

    'Private Property lblEnvironmentLeft As Object

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            If Session("RTSSessionID") Is Nothing Then
                SaveSessionInfo()
            Else
                If Page.Request.UrlReferrer Is Nothing Then
                    Response.Redirect("InvalidAccess.aspx", True)
                End If
            End If

        End If

        DisableBrowserBack()

        'Read from Config File
        If UCase(_environment) = "TEST" Then
            lblEnvironmentLeft.Text = "<font color='red'>" + "**Test**" + "</font>"
            lblEnvironmentRight.Text = "<font color='red'>" + "**Test**" + "</font>"
        ElseIf UCase(_environment) = "QA" Then
            lblEnvironmentLeft.Text = "<font color='red'>" + "**QA**" + "</font>"
            lblEnvironmentRight.Text = "<font color='red'>" + "**QA**" + "</font>"
        ElseIf UCase(_environment) = "PROD" Then
            lblEnvironmentRight.Text = ""
        End If
    End Sub

#End Region



#Region "Methods"

    Private Sub SaveSessionInfo()
        Dim guid As System.Guid = System.Guid.NewGuid()
        Session.Add("RTSSessionID", guid.ToString())
    End Sub


    Protected Sub DisableBrowserBack()
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType, "DisableBrowserBackButton", "window.history.go(+1);", True)
    End Sub

#End Region
    

    

End Class