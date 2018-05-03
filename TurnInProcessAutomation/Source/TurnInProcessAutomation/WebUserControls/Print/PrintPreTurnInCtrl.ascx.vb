Public Class PrintPreTurnInCtrl
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            ShowAdLevelSearchControl()
        End If
    End Sub

    Public Sub ShowAdLevelSearchControl()
        tblPrintPreTurnInAdLevelCtrl.Style.Add("display", "inline")
        tblPrintPreTurnInCtrl.Style.Add("display", "none")
    End Sub

    Public Sub ShowWorkListSearchControl()
        tblPrintPreTurnInAdLevelCtrl.Style.Add("display", "none")
        tblPrintPreTurnInCtrl.Style.Add("display", "inline")
    End Sub

End Class