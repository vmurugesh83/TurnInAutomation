Imports Telerik.Web.UI

Partial Class PopupPage
    Inherits System.Web.UI.MasterPage

    Private ReadOnly Environment As String = ConfigurationManager.AppSettings("Environment")

    Public WriteOnly Property SessionTimeoutEnabled() As Boolean
        Set(ByVal value As Boolean)
            sessionTimeout.Enabled = value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'Disable browser "back" functionality.
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType, "DisableBrowserBackButton", "window.history.go(+1);", True)

        'Set session timeout watcher control.
        sessionTimeout.TimeoutMinutes = HttpContext.Current.Session.Timeout
        sessionTimeout.AboutToTimeoutMinutes = HttpContext.Current.Session.Timeout - 1

        If IsPostBack Then Return

        'SetEnvironmentHeading()
    End Sub

    'Private Sub SetEnvironmentHeading()
    '    Const environmentMarkup As String = "<span style=""color:red"">**{0}**</span>"

    '    Select Case Environment.ToUpper()
    '        Case "TEST"
    '            lblEnvironmentLeft.Text = String.Format(environmentMarkup, "Test")
    '            lblEnvironmentRight.Text = String.Format(environmentMarkup, "Test")

    '        Case "QA"
    '            lblEnvironmentLeft.Text = String.Format(environmentMarkup, "QA")
    '            lblEnvironmentRight.Text = String.Format(environmentMarkup, "QA")

    '        Case Else
    '            lblEnvironmentLeft.Text = String.Empty
    '            lblEnvironmentRight.Text = String.Empty

    '    End Select
    'End Sub

End Class
