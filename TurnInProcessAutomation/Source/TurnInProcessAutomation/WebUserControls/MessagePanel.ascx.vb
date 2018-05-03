Imports TurnInProcessAutomation.BLL.MiscConstants

Partial Public Class MessagePanel
    Inherits System.Web.UI.UserControl

#Region "Members"

    Private _messageText As String = String.Empty
    Private _popmessageText As String = String.Empty
    Private _messageType As SeverityType = SeverityType.Error 'For backwards compatibility, make messages errors by default.

#End Region

#Region "Properties"

    Public WriteOnly Property ErrorMessage() As String
        Set(ByVal value As String)
            MessageText = value
            MessageType = SeverityType.Error
        End Set
    End Property

    Public WriteOnly Property GeneralMessage() As String
        Set(ByVal value As String)
            MessageText = value
            MessageType = SeverityType.Informational
        End Set
    End Property

    Public WriteOnly Property WarningMessage() As String
        Set(ByVal value As String)
            MessageText = value
            MessageType = SeverityType.Warning
        End Set
    End Property

    Public WriteOnly Property PopUpMessage() As String
        Set(ByVal value As String)
            PopMessageText = value
        End Set
    End Property

    Public WriteOnly Property MessageCode() As MessageCode
        Set(ByVal value As MessageCode)
            Dim message As Message = PageBase.GetMessage(value)

            If message IsNot Nothing Then
                MessageText = message.CodeWithDescription
                MessageType = message.Severity
            End If
        End Set
    End Property

    Public Property MessageText() As String
        Get
            Return _messageText
        End Get
        Private Set(ByVal value As String)
            _messageText = value
        End Set
    End Property

    Public Property PopMessageText() As String
        Get
            Return _popmessageText
        End Get
        Private Set(ByVal value As String)
            _popmessageText = value
        End Set
    End Property

    Private Property MessageType() As SeverityType
        Get
            Return _messageType
        End Get
        Set(ByVal value As SeverityType)
            _messageType = value
        End Set
    End Property

#End Region

#Region "Events"

    Private Sub Control_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If PopMessageText.Length > 0 Then
            Dim script As String = "alert('" + PopMessageText + "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ServerControlScript", script, True)
        End If

        pnlMessage.Visible = Not String.IsNullOrEmpty(MessageText)
        If Not pnlMessage.Visible Then Return

        lblMessage.Text = MessageText
        lblMessage.CssClass = GetCssClass()

    End Sub

#End Region

#Region "Methods"

    Private Function GetCssClass() As String
        If MessageType = SeverityType.Undefined Then Return String.Empty

        Dim cssClass As String = String.Format("message-{0}", MessageType.ToString().ToLower())

        'If the Errors on Page message is being displayed, make it a larger size than other messages.
        If MessageText = ErrorOnPage Then cssClass += " message-larger"

        Return cssClass
    End Function

#End Region

End Class
