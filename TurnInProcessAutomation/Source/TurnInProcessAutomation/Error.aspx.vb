Imports TurnInProcessAutomation.BLL

Partial Public Class [Error]
    Inherits System.Web.UI.Page


#Region "Members"

    Private _commonBO As TurnInProcessAutomation.BLL.CommonBO = Nothing

#End Region



#Region "Constructor"

    Public Sub New()
        Me._commonBO = New CommonBO
    End Sub

#End Region



#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ex As Exception = CType(Application("Exception"), Exception)
        If Not ex Is Nothing Then
            Me.litErrorMessage.Text = ex.Message
        End If
        Application("Exception") = Nothing

        If Session("ErrorMsg") IsNot Nothing Then
            Me.litErrorMessage.Text = Session("ErrorMsg").ToString
        End If
    End Sub

#End Region


End Class