Imports Telerik.Web.UI

Partial Public Class ModalPopupControl
    Inherits System.Web.UI.UserControl

#Region "Members"

    Private _enablePDFExport As Boolean = True

#End Region

#Region "Properties"

    Public ReadOnly Property ExportText() As String
        Get
            Return Me.rblExport.SelectedValue
        End Get
    End Property

    Public ReadOnly Property ExportCommandName() As String
        Get
            Select Case rblExport.SelectedValue
                Case "PDF"
                    Return RadGrid.ExportToPdfCommandName

                Case "Excel"
                    Return RadGrid.ExportToExcelCommandName

                Case Else
                    Return Nothing

            End Select
        End Get
    End Property

    Public ReadOnly Property OkButton() As LinkButton
        Get
            Return Me.lnkOK
        End Get
    End Property

    Public Property EnablePDFExport() As Boolean
        Get
            Return _enablePDFExport
        End Get
        Set(ByVal value As Boolean)
            _enablePDFExport = value
        End Set
    End Property

#End Region

#Region "Events"

    Private Sub Control_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        rblExport.Items.FindByValue("PDF").Enabled = EnablePDFExport
    End Sub

#End Region

#Region "Methods"

    Public Sub Hide()
        If Not Me.mPopup Is Nothing Then
            Me.mPopup.Hide()
        End If
    End Sub

    Public Sub Show()
        If Not Me.mPopup Is Nothing Then
            Me.mPopup.Show()
        End If
    End Sub

#End Region

End Class