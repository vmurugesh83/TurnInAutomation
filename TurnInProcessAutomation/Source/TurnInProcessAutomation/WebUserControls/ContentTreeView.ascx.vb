Imports Telerik.Web.UI

Partial Class WebUserControls_ContentTreeView
    Inherits System.Web.UI.UserControl

    Public ReadOnly Property lblStatusIdRef() As Label
        Get
            Return lblStatusId
        End Get
    End Property

    Public ReadOnly Property lblStatusRef() As Label
        Get
            Return lblStatus
        End Get
    End Property

    Public ReadOnly Property rtvContentRef() As RadTreeView
        Get
            Return rtvContent
        End Get
    End Property
End Class