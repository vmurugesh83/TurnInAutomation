Imports Telerik.Web.UI

Partial Class SidebarTreeView
    Inherits System.Web.UI.UserControl

    Public ReadOnly Property TreeView() As RadTreeView
        Get
            Return rtvTreeView
        End Get
    End Property

End Class