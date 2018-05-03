Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL

Partial Public Class ModalMoveBatch
    Inherits System.Web.UI.UserControl

    Dim _TUAdInfo As New TUAdInfo

#Region "Properties"

    Public ReadOnly Property SaveButton() As LinkButton
        Get
            Return Me.lnkSave
        End Get
    End Property

    Public Property BatchNum() As Integer
        Get
            Return CInt(Me.lblBatchNum.Text)
        End Get
        Set(value As Integer)
            Me.lblBatchNum.Text = CStr(value)
        End Set
    End Property

    Public Property oldAdNum() As Integer
        Get
            Return CInt(Me.lblAd.Text)
        End Get
        Set(value As Integer)
            Me.lblAd.Text = CStr(value)
        End Set
    End Property

    Public WriteOnly Property Week() As String
        Set(value As String)
            Me.lblWeek.Text = value
        End Set
    End Property

    Public ReadOnly Property newAdNum() As Integer
        Get
            Return CInt(Me.rcbAds.SelectedItem.Value)
        End Get
    End Property

#End Region

#Region "Methods"
    Protected Sub rcbAds_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        With rcbAds
            .Items.Clear()
            .DataSource = _TUAdInfo.GetAdsForMaintenance().ToList()
            .DataValueField = "adnbr"
            .DataTextField = "AdNumberDesc"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

#End Region
    Public Delegate Sub MoveBatchEventHandler(sender As Object, e As MoveBatchEventArgs)
    Public Event MoveBatch As MoveBatchEventHandler


    Private Sub lnkSave_Click(sender As Object, e As System.EventArgs) Handles lnkSave.Click
        Dim eMoveBatch As New MoveBatchEventArgs
        eMoveBatch.newAdNum = CInt(rcbAds.SelectedValue)
        RaiseEvent MoveBatch(Me, eMoveBatch)
    End Sub

    Public Sub SetLabels(ByVal batchId As String, ByVal Ad As String, ByVal Week As String)
        lblBatchNum.Text = batchId
        lblAd.Text = Ad
        lblWeek.Text = Week
    End Sub
End Class

Public Class MoveBatchEventArgs
    Inherits EventArgs
    Private newAd As Integer
    Public Property newAdNum() As Integer
        Get
            Return newAd
        End Get
        Set(value As Integer)
            newAd = value
        End Set
    End Property
End Class
