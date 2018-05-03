Imports Telerik.Web.UI
Imports System.Web.Services
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Configuration
Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities

Public Class AdPageSetUpSearchCtrl
    Inherits System.Web.UI.UserControl
    Private Const ItemsPerRequest As Integer = 10
    Private _IsEcommerce As Boolean = False

    Public Property IsEcommerce() As Boolean
        Get
            Return _IsEcommerce
        End Get
        Set(ByVal value As Boolean)
            _IsEcommerce = value
        End Set
    End Property

    Public ReadOnly Property SelectedAd() As Integer
        Get
            Try
                Return CInt(rcbAds.Text)

            Catch ex As Exception
                Return 0
            End Try
        End Get
    End Property

    Protected Sub rcbAds_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        Dim _TUAdInfo As New TUAdInfo
        Dim items As List(Of AdInfoInfo)

        rcbAds.Items.Clear()
        If Session("AdPageSetUpSearchCtrl.AdList") IsNot Nothing Then
            items = CType(Session("AdPageSetUpSearchCtrl.AdList"), List(Of AdInfoInfo))
        Else
            items = _TUAdInfo.GetAllFromAdInfoFiltered(IsEcommerce).ToList()
            Session("AdPageSetUpSearchCtrl.AdList") = items
        End If

        If e.Text <> "" Then
            items = items.Where(Function(item) item.adnbr.ToString.Contains(e.Text)).ToList()
        End If

        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, items.Count())
        e.EndOfItems = endOffset = items.Count()

        rcbAds.DataSource = items.GetRange(itemOffset, endOffset - itemOffset)
        rcbAds.DataTextField = "adnbr"
        rcbAds.DataBind()
        rcbAds.ShowMoreResultsBox = Not e.EndOfItems

        e.Message = GetStatusMessage(endOffset, items.Count())
    End Sub

    Private Shared Function GetStatusMessage(ByVal offset As Integer, ByVal total As Integer) As String
        If total <= 0 Then
            Return "No matches"
        End If

        Return [String].Format("Items <b>1</b>-<b>{0}</b> of <b>{1}</b>", offset, total)
    End Function

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Session("AdPageSetUpSearchCtrl.AdList") = Nothing
    End Sub

    Public Sub ResetControls()
        rcbAds.ClearSelection()
        rcbAds.Text = ""
    End Sub

End Class