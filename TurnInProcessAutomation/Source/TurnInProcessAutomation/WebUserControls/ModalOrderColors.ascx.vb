Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BusinessEntities

Partial Public Class ModalOrderColors
    Inherits System.Web.UI.UserControl

#Region "Properties"

    Public ReadOnly Property SaveButton() As LinkButton
        Get
            Return Me.lnkSave
        End Get
    End Property

#End Region

#Region "Methods"

#End Region
    Public Event SaveOrder As System.EventHandler

    Private Sub lnkSave_Click(sender As Object, e As System.EventArgs) Handles lnkSave.Click
        RaiseEvent SaveOrder(Me, New EventArgs())
    End Sub
End Class