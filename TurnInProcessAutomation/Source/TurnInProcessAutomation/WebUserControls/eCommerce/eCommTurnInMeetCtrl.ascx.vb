Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities

Public Class eCommTurnInMeetCtrl
    Inherits System.Web.UI.UserControl

    Private _TUEMM As New TUEMM
    Private _TUBuyer As New TUBuyer
    Private _TUCtlgAdPg As New TUCtlgAdPg
    Protected _TUAdInfo As New TUAdInfo
    Dim _TUDepartment As New TUDepartment
    Dim _TULabel As New TULabel
    Dim _TUVendorStyle As New TUVendorStyle
    Dim _TUBatch As New TUBatch
    Dim _eCommTurnInMeet As eCommTurnInMeet = Nothing

#Region "Properties"

    Public ReadOnly Property SelectedAdNumber() As String
        Get
            Return rcbAds.SelectedValue
        End Get
    End Property

    Public ReadOnly Property cmbAdd() As RadComboBox
        Get
            Return rcbAds
        End Get
    End Property

    Public ReadOnly Property cmbPage() As RadComboBox
        Get
            Return rcbPageNumber
        End Get
    End Property
    
    Public ReadOnly Property rcbBuyer() As RadComboBox
        Get
            Return cmbBuyer
        End Get
    End Property

    Public ReadOnly Property rcbDept() As RadComboBox
        Get
            Return cmbDept
        End Get
    End Property

    Public ReadOnly Property rcbVendorStyle() As RadComboBox
        Get
            Return cmbVendorStyle
        End Get
    End Property

    Public ReadOnly Property rcbLabel() As RadComboBox
        Get
            Return cmbLabel
        End Get
    End Property

    Public ReadOnly Property rcbBatchNum() As RadComboBox
        Get
            Return cmbBatchNum
        End Get
    End Property

    Public ReadOnly Property SelectedPageNumber() As String
        Get
            Return rcbPageNumber.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedDeptId() As String
        Get
            Return cmbDept.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedEMMId() As String
        Get
            Return rcbEMM.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedBuyerId() As String
        Get
            Return cmbBuyer.SelectedValue
        End Get
    End Property
    Public ReadOnly Property SelectedLabelID() As String
        Get
            Return cmbLabel.SelectedValue
        End Get
    End Property
    Public ReadOnly Property SelectedVendorStyleID() As String
        Get
            Return cmbVendorStyle.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedBatchNum() As String
        Get
            Return cmbBatchNum.SelectedValue
        End Get
    End Property
#End Region

    Protected Sub rcbAds_SelectedIndexChanged(ByVal sender As Object, ByVal e As RadComboBoxSelectedIndexChangedEventArgs)
        rcbPageNumber.ClearSelection()
        rcbPageNumber.Items.Clear()

        ClearBatchFilter()

        If rcbAds.Text <> "" Then
            rcbPageNumber.DataSource = Nothing

            With rcbPageNumber
                .Enabled = True
                .DataValueField = "PgNbr"
                .DataTextField = "PageNumberDesc"
                .DataSource = _TUCtlgAdPg.GetAllFromCtlgAdPg(CInt(rcbAds.SelectedValue))
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        Else
            rcbPageNumber.Enabled = False
        End If
    End Sub
    Protected Sub rcbEMM_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
        ClearBatchFilter()
    End Sub

    Private Sub rcbPageNumber_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbPageNumber.SelectedIndexChanged
        ClearBatchFilter()
    End Sub

    Private Sub cmbBuyer_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbBuyer.SelectedIndexChanged
        ClearBatchFilter()
    End Sub

    Private Sub cmbDept_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbDept.SelectedIndexChanged
        cmbVendorStyle.ClearSelection()
        cmbVendorStyle.Items.Clear()
        cmbVendorStyle.Text = String.Empty

        ClearBatchFilter()

        If cmbDept.Text.Trim() <> String.Empty Then
            cmbVendorStyle.Enabled = True
        Else
            cmbVendorStyle.Enabled = False
        End If
    End Sub

    Private Sub cmbLabel_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbLabel.SelectedIndexChanged
        ClearBatchFilter()
    End Sub

    Private Sub cmbVendorStyle_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbVendorStyle.SelectedIndexChanged
        ClearBatchFilter()
    End Sub
    Protected Sub rcbAds_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        With rcbAds
            .Items.Clear()
            .DataSource = _TUAdInfo.GetAdsForMeeting().ToList()
            .DataValueField = "adnbr"
            .DataTextField = "AdNumberDesc"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub rcbEMM_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        With rcbEMM
            .Items.Clear()
            .DataSource = _TUEMM.GetAllFromBuyer().ToList()
            .DataTextField = "EMMDesc"
            .DataValueField = "EMMId"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Private Sub cmbBuyer_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs) Handles cmbBuyer.ItemsRequested
        With cmbBuyer
            .DataSource = Nothing
            .DataSource = _TUBuyer.GetAllFromBuyer
            .DataTextField = "BuyerNameId"
            .DataValueField = "BuyerId"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbDept_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbDept
            .DataSource = Nothing
            .DataSource = _TUDepartment.GetAllFromDepartment
            .DataTextField = "DeptIdDesc"
            .DataValueField = "DeptId"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbLabel_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        With cmbLabel
            .DataSource = Nothing
            .DataSource = _TULabel.GetAllLabels
            .DataTextField = "LabelDesc"
            .DataValueField = "LabelId"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub

    Protected Sub cmbVendorStyle_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        If e.Text.Length >= 3 Then
            With cmbVendorStyle
                .DataSource = Nothing
                .DataSource = _TUVendorStyle.GetAllFromVendorStyle(CInt(cmbDept.SelectedValue), e.Text)
                .DataTextField = "VENDORSTYLENUMBER"
                .DataValueField = "VENDORSTYLENUMBER"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub

    Protected Sub cmbBatchNum_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        Dim strAdNbr As String = SelectedAdNumber.Trim
        Dim strPageNbr As String = cmbPage.SelectedValue.Trim
        Dim strBuyerID As String = SelectedBuyerId.Trim
        Dim strDeptID As String = SelectedDeptId.Trim
        Dim strLblID As String = SelectedLabelID.Trim
        Dim strVndrStylID As String = SelectedVendorStyleID.Trim
        Dim strEMM As String = SelectedEMMId.Trim

        With cmbBatchNum
            .DataSource = Nothing
            .DataSource = _TUBatch.GetBatches(strAdNbr, strPageNbr, strBuyerID, strDeptID, strLblID, strVndrStylID, strEMM)
            .DataTextField = "BatchId"
            .DataValueField = "BatchId"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With    
    End Sub

    Private Sub ClearBatchFilter()
        cmbBatchNum.ClearSelection()
        cmbBatchNum.Items.Clear()
        cmbBatchNum.Text = ""
    End Sub

#Region "Validations"
    Public Function IsSearchValid() As Boolean
        If cmbBatchNum.SelectedValue = "" Then
            Return False
        Else
            Return True
        End If

    End Function
#End Region
End Class