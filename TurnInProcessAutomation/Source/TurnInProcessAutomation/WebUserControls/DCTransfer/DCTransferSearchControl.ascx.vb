Imports TurnInProcessAutomation.BLL
Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BusinessEntities

Public Class DCTransferSearchControl
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public ReadOnly Property SelectedDepartmentId() As Integer
        Get
            If Integer.TryParse(rcbDept.SelectedValue, Nothing) Then
                Return CInt(rcbDept.SelectedValue)
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property SelectedBuyerId() As Integer
        Get
            If Integer.TryParse(rcbBuyer.SelectedValue, Nothing) Then
                Return CInt(rcbBuyer.SelectedValue)
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property SelectedVendorId() As Integer
        Get
            If Integer.TryParse(rcbVendor.SelectedValue, Nothing) Then
                Return CInt(rcbVendor.SelectedValue)
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property PriceStatusCodes As List(Of String)
        Get
            Dim StatusCodes As New List(Of String)
            For Each cb As ListItem In cblPriceStatusCodes.Items
                If cb.Selected Then
                    StatusCodes.Add(cb.Value)
                End If
            Next
            Return StatusCodes
        End Get
    End Property


    Private Sub rcbDept_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs) Handles rcbDept.ItemsRequested
        If rcbDept.Items.Count = 0 Then
            Dim _TUDepartment As New TUDepartment
            With rcbDept
                .DataSource = _TUDepartment.GetAllFromDepartment
                .DataTextField = "DeptIdDesc"
                .DataValueField = "DeptId"
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub

    Private Sub rcbVendor_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs) Handles rcbVendor.ItemsRequested
        If SelectedDepartmentId > 0 Then
            With rcbVendor
                Dim _TUVendor As New TUVendor
                .Enabled = True
                .DataTextField = "VendorIdName"
                .DataValueField = "VendorId"
                .DataSource = _TUVendor.GetAllFromVendorByDepartment(CInt(SelectedDepartmentId))
                .DataBind()
                .Items.Insert(0, New RadComboBoxItem(""))
            End With
        End If
    End Sub

    'Get values from Session else make the fetch from server.
    Private ReadOnly Property AllBuyers() As List(Of BuyerInfo)
        Get
            If IsNothing(Session("AllBuyerList")) Then
                Dim _TUBuyer As New TUBuyer
                Session("AllBuyerList") = CType(_TUBuyer.GetAllFromBuyer(), List(Of BuyerInfo))
            End If
            Return TryCast(Session("AllBuyerList"), List(Of BuyerInfo))
        End Get
    End Property

    Private Sub rcbBuyer_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs) Handles rcbBuyer.ItemsRequested
        With rcbBuyer
            Dim _TUBuyer As New TUBuyer
            .DataSource = _TUBuyer.GetAllFromBuyer
            .DataTextField = "BuyerNameId"
            .DataValueField = "BuyerId"
            .DataBind()
            .Items.Insert(0, New RadComboBoxItem(""))
        End With
    End Sub
End Class