Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BLL


Public Class eCommercePrioritizationCtrl
    Inherits System.Web.UI.UserControl

    Private _TUEMM As New TUEMM
    Private _TUCMG As New TUCMG
    Private _TUBuyer As New TUBuyer
    Private _TULabel As New TULabel
    Private _TUAdInfo As New TUAdInfo
    Private _TUImage As New TUImage
    Private _TUVendorStyleNum As New TUVendorStyle

#Region "Properties"
    Public ReadOnly Property SelectedEMMId() As String
        Get
            Return rcbEMM.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedCMGId() As String
        Get
            Return rcbCMG.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedBuyerId() As String
        Get
            Return rcbBuyer.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedLabelId() As String
        Get
            Return rcbLabel.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedTUWeek() As String
        Get
            Return rcbTUWeek.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedImageId() As String
        Get
            Return rcbImageId.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedVendStyId() As String
        Get
            Return rcbVndSty.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedStatus() As String
        Get
            Return rcbStatus.SelectedValue
        End Get
    End Property
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Protected Sub rcbEMM_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        If rcbEMM.Items.Count = 0 Then
            With rcbEMM
                .DataSource = _TUEMM.GetAllFromBuyer().ToList()
                .DataTextField = "EMMDesc"
                .DataValueField = "EMMId"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub

    Protected Sub rcbCMG_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        If rcbCMG.Items.Count = 0 Then
            With rcbCMG
                .DataSource = _TUCMG.GetAllFromCMG().ToList()
                .DataTextField = "CMG_DESC"
                .DataValueField = "CMG_ID"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub

    Protected Sub rcbBuyer_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        If rcbBuyer.Items.Count = 0 Then
            With rcbBuyer
                .DataSource = _TUBuyer.GetBuyersForCMG(SelectedCMGId).ToList()
                .DataTextField = "BuyerNameId"
                .DataValueField = "BuyerId"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub

    Protected Sub rcbLabel_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        If rcbLabel.Items.Count = 0 Then
            With rcbLabel
                .DataSource = _TULabel.GetAllLabels().ToList()
                .DataTextField = "LabelDesc"
                .DataValueField = "LabelId"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub

    Protected Sub rcbTUWeek_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        If rcbTUWeek.Items.Count = 0 Then
            With rcbTUWeek
                .DataSource = _TUAdInfo.GetAllFromAdInfoFiltered(True).ToList()
                .DataValueField = "adnbr"
                .DataTextField = "TurnInWeek"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub

    Protected Sub rcbImageId_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        If e.Text.Length >= 3 Then
            With rcbImageId
                .Items.Clear()
                .DataSource = _TUImage.GetAllImages(SelectedStatus, e.Text).ToList()
                .DataTextField = "ImageId"
                .DataValueField = "ImageId"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub

    Protected Sub rcbVndSty_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        If rcbVndSty.Items.Count = 0 Then
            With rcbVndSty
                .Items.Clear()
                .DataSource = _TUVendorStyleNum.GetVendorStyleNumPrioritization(SelectedStatus).ToList()
                .DataTextField = "VendorStyleNumber"
                .DataValueField = "VendorStyleNumber"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub
#End Region

    Private Sub rcbStatus_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbStatus.SelectedIndexChanged
        rcbImageId.Items.Clear()
        rcbVndSty.Items.Clear()
    End Sub

    Private Sub rcbCMG_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbCMG.SelectedIndexChanged
        rcbBuyer.Items.Clear()
    End Sub
End Class