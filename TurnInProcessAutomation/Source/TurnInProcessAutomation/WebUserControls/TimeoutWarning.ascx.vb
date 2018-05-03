Partial Public Class TimeoutWarning
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim milliseconds As Integer = 60000
        TimerTimeout.Interval = Session.Timeout * milliseconds
    End Sub

    Private _sessionExpiredRedirect As String
    Public Property SessionExpiredRedirect() As String
        Get
            Return _sessionExpiredRedirect
        End Get
        Set(ByVal value As String)
            _sessionExpiredRedirect = value
        End Set
    End Property

    Protected Sub TimerTimeout_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles TimerTimeout.Tick
        If Not String.IsNullOrEmpty(SessionExpiredRedirect) Then
            If SessionExpiredRedirect.IndexOf("~") = 0 Then
                Response.Redirect(VirtualPathUtility.ToAppRelative(SessionExpiredRedirect))
            Else
                Response.Redirect(SessionExpiredRedirect)
            End If

        Else
            Me.PanelTimeout.Visible = True
        End If
    End Sub
End Class