Imports Telerik.Web.UI

Partial Class ContentPage
    Inherits System.Web.UI.MasterPage

    Private ReadOnly Environment As String = ConfigurationManager.AppSettings("Environment")

    'This property is referenced in code that is commented out.  In the event
    'that code is uncommented and used again, this is kept for reference.
    '
    'Public ReadOnly Property ValidationSummary() As BonTon.Web.Controls.ValidationSummary
    '    Get
    '        Return Me.ValidationSummary1
    '    End Get
    'End Property

    Public ReadOnly Property SideBar() As RadPane
        Get
            Return Me.rpSidebar
        End Get
    End Property

    Public ReadOnly Property SideBarPlaceHolder() As PlaceHolder
        Get
            Return Me.phSidebar
        End Get
    End Property

    Public WriteOnly Property SessionTimeoutEnabled() As Boolean
        Set(ByVal value As Boolean)
            sessionTimeout.Enabled = value
        End Set
    End Property

    ''' <remarks>Because of the way dynamically-added controls are handled, it is important that if this method is called, it is called during the Page's Init event.</remarks>
    Friend Function LoadSidebarControl(ByVal controlUrl As String, Optional ByVal controlId As String = Nothing) As Control
        Dim control As Control = LoadControl(controlUrl)
        If control Is Nothing Then Return Nothing

        'ID the control now so it can be found later.
        If Not String.IsNullOrEmpty(controlId) Then control.ID = controlId

        SideBarPlaceHolder.Controls.Add(control)

        Return control
    End Function

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'Disable browser "back" functionality.
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType, "DisableBrowserBackButton", "window.history.go(+1);", True)

        'Set session timeout watcher control.
        sessionTimeout.TimeoutMinutes = HttpContext.Current.Session.Timeout
        sessionTimeout.AboutToTimeoutMinutes = HttpContext.Current.Session.Timeout - 1

        If IsPostBack Then Return

        SetEnvironmentHeading()
    End Sub

    Private Sub SetEnvironmentHeading()
        Const environmentMarkup As String = "<span style=""color:red"">**{0}**</span>"

        Select Case Environment.ToUpper()
            Case "TEST"
                lblEnvironmentLeft.Text = String.Format(environmentMarkup, "Test")
                lblEnvironmentRight.Text = String.Format(environmentMarkup, "Test")

            Case "QA"
                lblEnvironmentLeft.Text = String.Format(environmentMarkup, "QA")
                lblEnvironmentRight.Text = String.Format(environmentMarkup, "QA")

            Case Else
                lblEnvironmentLeft.Text = String.Empty
                lblEnvironmentRight.Text = String.Empty

        End Select
    End Sub

    Public Sub HideRTSMenu()
        MerchandiseTurnInSystemMenu.Visible = False
    End Sub

    Public Sub ShowRTSMenu()
        MerchandiseTurnInSystemMenu.Visible = True
    End Sub

End Class
