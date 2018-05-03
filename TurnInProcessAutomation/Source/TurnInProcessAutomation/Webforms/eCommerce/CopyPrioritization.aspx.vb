Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BLL.Enumerations
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL
Imports System.IO
Imports System.Net

Public Class CopyPrioritization
    Inherits System.Web.UI.Page

    Private _TUCopyPrioritization As New TUCopyPrioritization
    Private _copyPrioritizationSearchCtrl As CopyPrioritizationSearchCtrl = Nothing
    Private _TUTMS900Parameter As TUTMS900PARAMETER = Nothing

    Public ReadOnly Property CopyPrioritizationSearchCtrl() As CopyPrioritizationSearchCtrl
        Get
            Dim control As Control = Me.Master.SideBarPlaceHolder.FindControl("CopyPrioritizationSearchCtrl1")
            Me._copyPrioritizationSearchCtrl = DirectCast(control, CopyPrioritizationSearchCtrl)
            Return _copyPrioritizationSearchCtrl
        End Get
    End Property

    Public Property CopyPrioritizationInfos As List(Of CopyPrioritizationInfo)
        Get
            If Session("CopyPrioritizationInfos") Is Nothing Then
                Session("CopyPrioritizationInfos") = New List(Of CopyPrioritizationInfo)
            End If
            Return CType(Session("CopyPrioritizationInfos"), List(Of CopyPrioritizationInfo))
        End Get
        Set(ByVal value As List(Of CopyPrioritizationInfo))
            Session("CopyPrioritizationInfos") = value
        End Set
    End Property

    Public Property CopyPrioritizationInfo As CopyPrioritizationInfo
        Get
            If Session("CopyPrioritizationInfo") Is Nothing Then
                Session("CopyPrioritizationInfo") = New CopyPrioritizationInfo
            End If
            Return CType(Session("CopyPrioritizationInfo"), CopyPrioritizationInfo)
        End Get
        Set(ByVal value As CopyPrioritizationInfo)
            Session("CopyPrioritizationInfo") = value
        End Set
    End Property

    Public Property CopyPrioritizationColorInfo As List(Of CopyPrioritizationColorInfo)
        Get
            If Session("CopyPrioritizationColorInfo") Is Nothing Then
                Session("CopyPrioritizationColorInfo") = New List(Of CopyPrioritizationColorInfo)
            End If
            Return CType(Session("CopyPrioritizationColorInfo"), List(Of CopyPrioritizationColorInfo))
        End Get
        Set(ByVal value As List(Of CopyPrioritizationColorInfo))
            Session("CopyPrioritizationColorInfo") = value
        End Set
    End Property

    Private Sub CopyPrioritization_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            Dim control As Control = LoadControl("~/WebUserControls/GXS/CopyPrioritizationSearchCtrl.ascx")
            If Not control Is Nothing Then
                control.ID = "CopyPrioritizationSearchCtrl1"
                Me.Master.SideBarPlaceHolder.Controls.Add(control)
                Me.Master.SideBar.Width = Unit.Pixel(300)
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                CopyPrioritizations = Nothing
                If grdCopyPrioritization.Items.Count = 0 Then
                    EnableDisableButtons("Save to Pending", False)
                    EnableDisableButtons("Set to Ready", False)
                End If
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub rtbCopyPrioritization_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles rtbCopyPrioritization.ButtonClick
        Try
            If TypeOf e.Item Is RadToolBarButton Then
                Dim radToolBarButton As RadToolBarButton = DirectCast(e.Item, RadToolBarButton)

                Select Case radToolBarButton.CommandName
                    Case "Retrieve"
                        RetrieveCopyPrioritizationResultList()
                        grdCopyPrioritization.Visible = True
                        grdCopyPrioritization.Rebind()
                        EnableDisableButtons("Save to Pending", False)
                        EnableDisableButtons("Set to Ready", False)
                    Case "Submit"
                        If String.IsNullOrEmpty(txtCopyPreview.Text.Trim()) OrElse String.IsNullOrEmpty(txtProductName.Text.Trim()) Then
                            MessagePanel1.ErrorMessage = "Product Name and Copy Preview notes are required."
                            Exit Sub
                        End If
                        SubmitCopyToWebCat()
                    Case "Reset"
                        Select Case rtsCopyPrioritization.SelectedIndex
                            Case 0
                                Session("CopyPrioritizations") = Nothing
                                Response.Redirect(Request.Url.ToString(), False)
                            Case 1
                                rtsCopyPrioritization.SelectedIndex = 0
                                rmpCopyPrioritization.SelectedIndex = 0
                                ShowHideTabs("Edit Copy", False)
                                EnableDisableButtons("Retrieve", True)
                                EnableDisableButtons("Save to Pending", False)
                                EnableDisableButtons("Set to Ready", False)
                                Response.Redirect(Request.Url.ToString(), False)
                        End Select
                    Case "Ready"
                        If String.IsNullOrEmpty(txtCopyPreview.Text.Trim()) OrElse String.IsNullOrEmpty(txtProductName.Text.Trim()) Then
                            MessagePanel1.ErrorMessage = "Product Name and Copy Preview notes are required."
                            Exit Sub
                        End If
                        SubmitCopyToWebCat(True)
                End Select
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try
    End Sub

    Private Sub RetrieveCopyPrioritizationResultList()

        Dim departmentID As Integer = 0
        Dim Upc As String = String.Empty
        Dim categoryId As Integer = 0
        CopyPrioritizations = Nothing
        Dim SelectedVendorStyles As New List(Of String)
        Dim vendorStyles As String = String.Empty
        Dim isnList As List(Of String) = Nothing
        Dim skuUpcList As List(Of String) = Nothing
        Dim imageIDList As List(Of String) = Nothing
        Dim itemFiltersExist As Boolean = False
        Dim thirdPartyFulfilment As Integer = 0
        Dim adNumber As Integer = 0
        Dim deptID As Integer = 0

        Try
            If CopyPrioritizationSearchCtrl.chkbDropship.Checked Then
                thirdPartyFulfilment = 2
            End If

            If CopyPrioritizationSearchCtrl.ResultsTabRadPanelBar.Items(0).Expanded Then
                If CopyPrioritizationSearchCtrl.rddtCategory.EmbeddedTree.SelectedNode Is Nothing Then
                    MessagePanel1.ErrorMessage = "A criteria is required."
                    Exit Sub
                End If
                categoryId = CInt(CopyPrioritizationSearchCtrl.rddtCategory.EmbeddedTree.SelectedNode.Value)
                CopyPrioritizations = _TUCopyPrioritization.GetCopyPrioritizationResultsByCriteria(categoryId,
                                                                                                   CopyPrioritizationSearchCtrl.PriceStatusCodes,
                                                                                                   GetImageFilters(), GetCopyFilters(),
                                                                                                   GetSKUUseFilters(),
                                                                                                   thirdPartyFulfilment,
                                                                                                   If(CopyPrioritizationSearchCtrl.chkbINFC.Checked, 192, 0),
                                                                                                   CopyPrioritizationSearchCtrl.chkbOnOrder.Checked,
                                                                                                   CopyPrioritizationSearchCtrl.chkbOnHand.Checked).ToList()

            ElseIf CopyPrioritizationSearchCtrl.ResultsTabRadPanelBar.Items(1).Expanded Then
                If String.IsNullOrEmpty(CopyPrioritizationSearchCtrl.cmbAdNo.SelectedValue) AndAlso String.IsNullOrEmpty(CopyPrioritizationSearchCtrl.cmbDept.SelectedValue) Then
                    MessagePanel1.ErrorMessage = "Ad and/or Department and/or vendor style is required."
                    Exit Sub
                End If

                SelectedVendorStyles = CopyPrioritizationSearchCtrl.hiddenVendorStyles.Value.Split(","c).ToList()

                If CopyPrioritizationSearchCtrl.cmbVendorStyle.SelectedValue <> String.Empty AndAlso Not SelectedVendorStyles.Contains(CopyPrioritizationSearchCtrl.cmbVendorStyle.SelectedValue.Trim()) Then
                    SelectedVendorStyles.Add(CopyPrioritizationSearchCtrl.cmbVendorStyle.SelectedValue.Trim)
                End If

                Select Case SelectedVendorStyles.Count
                    Case Is > 0
                        vendorStyles = String.Join("|", SelectedVendorStyles)
                    Case Else
                        vendorStyles = String.Empty
                End Select

                Integer.TryParse(CopyPrioritizationSearchCtrl.cmbAdNo.SelectedValue, adNumber)
                Integer.TryParse(CopyPrioritizationSearchCtrl.cmbDept.SelectedValue, deptID)

                CopyPrioritizations = _TUCopyPrioritization.GetCopyPrioritizationResultsByDept(deptID,
                                                                                               vendorStyles,
                                                                                               CopyPrioritizationSearchCtrl.dpStartShipDate.SelectedDate,
                                                                                               CopyPrioritizationSearchCtrl.PriceStatusCodes,
                                                                                               adNumber,
                                                                                               GetImageFilters(), GetCopyFilters(), GetSKUUseFilters(),
                                                                                               thirdPartyFulfilment,
                                                                                               If(CopyPrioritizationSearchCtrl.chkbINFC.Checked, 192, 0),
                                                                                                   CopyPrioritizationSearchCtrl.chkbOnOrder.Checked,
                                                                                                   CopyPrioritizationSearchCtrl.chkbOnHand.Checked).ToList()
            Else
                isnList = New List(Of String)
                skuUpcList = New List(Of String)
                imageIDList = New List(Of String)

                If CopyPrioritizationSearchCtrl.radLBISN.Items.Count > 0 Then
                    itemFiltersExist = True
                    For Each item As RadListBoxItem In CopyPrioritizationSearchCtrl.radLBISN.Items
                        isnList.Add(item.Text.Trim())
                    Next
                End If

                If CopyPrioritizationSearchCtrl.radLBSKU.Items.Count > 0 Then
                    itemFiltersExist = True
                    For Each item As RadListBoxItem In CopyPrioritizationSearchCtrl.radLBSKU.Items
                        skuUpcList.Add(item.Text.TrimStart("0"c))
                    Next
                End If

                If CopyPrioritizationSearchCtrl.radLBImage.Items.Count > 0 Then
                    itemFiltersExist = True
                    For Each item As RadListBoxItem In CopyPrioritizationSearchCtrl.radLBImage.Items
                        imageIDList.Add(item.Text.Trim())
                    Next
                End If

                If Not itemFiltersExist Then
                    MessagePanel1.ErrorMessage = "ISN or SKU/UPC or Image is required."
                    Exit Sub
                End If

                CopyPrioritizations = _TUCopyPrioritization.GetCopyPrioritizationResultsByItem(isnList,
                                                                                               skuUpcList,
                                                                                               imageIDList).ToList()
            End If

            grdCopyPrioritization.Visible = True
            EnableDisableButtons("Retrieve", True)
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try

    End Sub

    Public Property CopyPrioritizations As List(Of CopyPrioritizationInfo)
        Get
            If Session("CopyPrioritizations") Is Nothing Then
                Session("CopyPrioritizations") = New List(Of CopyPrioritizationInfo)
            End If
            Return CType(Session("CopyPrioritizations"), List(Of CopyPrioritizationInfo))
        End Get
        Set(ByVal value As List(Of CopyPrioritizationInfo))
            Session("CopyPrioritizations") = value
        End Set
    End Property

    Protected Sub grdCopyPrioritization_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdCopyPrioritization.PreRender
        Try
            If Not IsPostBack And Me.grdCopyPrioritization.MasterTableView.Items.Count > 1 Then
                Me.grdCopyPrioritization.MasterTableView.Items(1).Edit = True
                Me.grdCopyPrioritization.MasterTableView.Rebind()
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try

    End Sub

    Private Sub grdCopyPrioritization_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdCopyPrioritization.NeedDataSource
        Try
            grdCopyPrioritization.DataSource = CopyPrioritizations
            grdCopyPrioritization.MasterTableView.DataKeyNames = New String() {"ImageID", "CategoryCode", "ISN"}
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try

    End Sub

    Protected Sub chkItem_CheckedChanged(sender As Object, e As EventArgs)
        Dim cbx As CheckBox = TryCast(sender, CheckBox)
        Try
            EnableDisableEditCopyandResults()
            'txtCopyPreview.Content = String.Empty
            'For Each item As GridDataItem In grdCopyPrioritization.MasterTableView.Items
            '    Dim currentRowCheckBox As CheckBox = CType(item.FindControl("chkItem"), CheckBox)
            '    If cbx.Checked And Not currentRowCheckBox.Checked Then
            '        'disable checkbox
            '        'currentRowCheckBox.Enabled = False
            '    ElseIf Not cbx.Checked Then
            '        'enable checkbox
            '        currentRowCheckBox.Enabled = True
            '        ShowHideTabs("Edit Copy", False)
            '        EnableDisableButtons("Save to Pending", False)
            '        EnableDisableButtons("Set to Ready", False)
            '    End If
            'Next
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try

    End Sub

    Private Sub EnableDisableEditCopyandResults()
        Dim itemChecked As Boolean = False

        Try
            For Each item As GridDataItem In grdCopyPrioritization.MasterTableView.Items
                If CType(item.FindControl("chkItem"), CheckBox).Checked Then
                    With CopyPrioritizationInfo
                        .ImageID = CInt(item.GetDataKeyValue("ImageId"))
                        .CategoryCode = CInt(item.GetDataKeyValue("CategoryCode"))
                    End With

                    Dim _selectedISN As Decimal = CDec(item.GetDataKeyValue("ISN"))

                    RetrieveProductInfo(CopyPrioritizationInfo.ImageID, CopyPrioritizationInfo.CategoryCode, _selectedISN)
                    itemChecked = True
                    ShowHideTabs("Edit Copy", True)
                    EnableDisableButtons("Save to Pending", True)
                    EnableDisableButtons("Set to Ready", True)
                    EnableDisableButtons("Retrieve", False)
                    EnableDisableButtons("Export", False)
                    EnableDisableButtons("Reset", False)
                    Exit For
                End If
            Next
            If Not itemChecked Then
                ShowHideTabs("Edit Copy", False)
                EnableDisableButtons("Save to Pending", False)
                EnableDisableButtons("Set to Ready", False)
                EnableDisableButtons("Retrieve", True)
                EnableDisableButtons("Export", True)
                EnableDisableButtons("Reset", True)
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try

    End Sub

    Private Sub RetrieveProductInfo(ByVal ImageId As Integer, ByVal CategoryCode As Integer, ByVal SelectedISN As Decimal)
        Dim _TUEcommPrioritization As New TUCopyPrioritization
        Dim productInfo As GXSProductInfo = Nothing
        Dim Isn As String = String.Empty
        Dim uniqueItems As List(Of String) = New List(Of String)()

        Try
            CopyPrioritizationInfo = _TUEcommPrioritization.GetCopyPrioritizationResult(ImageId, CategoryCode)

            'make the tab visible
            pvEditCopy.Visible = True
            rtsCopyPrioritization.Tabs.Item(1).Selected = True
            rmpCopyPrioritization.SelectedIndex = 1

            If CopyPrioritizationInfo IsNot Nothing Then

                CopyPrioritizationColorInfo = CopyPrioritizationInfo.AvailableColors
                lblAvailableColors.Text = String.Empty
                lblAvailableSizes.Text = String.Empty
                If Not IsNothing(CopyPrioritizationColorInfo) AndAlso CopyPrioritizationColorInfo.Count > 0 Then
                    'lblAvailableColors.Text = lblAvailableColors.Text & "Color Family: " & CopyPrioritizationColorInfo(0).ColorFamily & "<BR/>"
                    'Dim uniqueColors As List(Of String) = CopyPrioritizationColorInfo.Select(Function(a) _
                    '                                                                             New With {a.FriendlyColorName,
                    '                                                                                       a.ColorSequenceNumber}).Distinct().OrderBy(Function(x) _
                    '                                                                                                                                      x.ColorSequenceNumber).Select(Function(y) _
                    '                                                                                                                                                                        y.FriendlyColorName).ToList()
                    For Each color As CopyPrioritizationColorInfo In CopyPrioritizationColorInfo.OrderBy(Function(a) a.ColorSequenceNumber)
                        If Not String.IsNullOrEmpty(color.FriendlyColorName) AndAlso Not uniqueItems.Contains(color.FriendlyColorName) Then
                            lblAvailableColors.Text = String.Concat(lblAvailableColors.Text,
                                                                    If(String.IsNullOrEmpty(lblAvailableColors.Text), String.Empty, "&nbsp;&nbsp;|&nbsp;&nbsp;"), color.FriendlyColorName)
                            uniqueItems.Add(color.FriendlyColorName)
                        End If
                    Next

                    uniqueItems = New List(Of String)()

                    For Each size As CopyPrioritizationColorInfo In CopyPrioritizationColorInfo.OrderBy(Function(a) a.SizeSequenceNumber)
                        If Not String.IsNullOrEmpty(size.FriendlySizeName) AndAlso Not uniqueItems.Contains(size.FriendlySizeName) Then
                            lblAvailableSizes.Text = String.Concat(lblAvailableSizes.Text,
                                                                   If(String.IsNullOrEmpty(lblAvailableSizes.Text), String.Empty, "&nbsp;&nbsp;|&nbsp;&nbsp;"), size.FriendlySizeName)
                            uniqueItems.Add(size.FriendlySizeName)
                        End If
                    Next

                    'For Each color As CopyPrioritizationColorInfo In CopyPrioritizationInfo.AvailableColors
                    '    If Not String.IsNullOrEmpty(color.FriendlyColorName) Then
                    '        lblAvailableColors.Text = lblAvailableColors.Text & color.FriendlyColorName & "<BR/>"
                    '    End If
                    '    If Not String.IsNullOrEmpty(color.FriendlySizeName) Then
                    '        lblAvailableSizes.Text = lblAvailableSizes.Text & color.FriendlySizeName & "<BR/>"
                    '    End If
                    'Next
                End If

                lblImageNotes.Text = CopyPrioritizationInfo.ImageDetails
                lblProductNotes.Text = CopyPrioritizationInfo.ProductNotes
                txtProductName.Text = CopyPrioritizationInfo.ProductName
                'lblProductDetails.Text = CopyPrioritizationInfo.ProductDetails
                'lblCategoryPath.Text = CopyPrioritizationInfo.CategoryName
                txtCopyPreview.Content = CopyPrioritizationInfo.ProductDetails

                rigProduct.Rebind()

                rlbSsku.Items.Clear()
                rlbCopy.Items.Clear()
                rlbGxs.Items.Clear()

                rlbSsku.Items.Add("Name: " & Server.HtmlDecode(CopyPrioritizationInfo.ProductName))
                rlbSsku.Items.Add("Brand: " & CopyPrioritizationInfo.BrandDesc)
                rlbSsku.Items.Add("Category: " & CopyPrioritizationInfo.CategoryName)

                If Not CopyPrioritizationInfo.FabricDescription = String.Empty Then
                    rlbSsku.Items.Add("Fabric Material: " & CopyPrioritizationInfo.FabricDescription)
                End If
                If Not CopyPrioritizationInfo.Origination = String.Empty Then
                    rlbSsku.Items.Add("Country of Origin: " & CopyPrioritizationInfo.Origination)
                End If

                rlbSsku.Items.Add("Dept: " & CopyPrioritizationInfo.DeptIdDesc)
                rlbSsku.Items.Add("ISN: " & CopyPrioritizationInfo.ISN)
                rlbSsku.Items.Add("Featured Color/Size: " & CopyPrioritizationInfo.FeaturedColorSize)
                If Not CopyPrioritizationInfo.VendorStyleNumber = String.Empty Then
                    rlbSsku.Items.Add("Vendor Style Number: " & CopyPrioritizationInfo.VendorStyleNumber)
                End If

                rlbSsku.Items.Add("Age: " & CopyPrioritizationInfo.AgeDesc)
                rlbSsku.Items.Add("Gender: " & CopyPrioritizationInfo.GenderDesc)
                rlbSsku.Items.Add("Image ID: " & CopyPrioritizationInfo.ImageID)

                productInfo = GetFabOrigGxsData(CopyPrioritizationInfo.UPC)
                If Not IsNothing(productInfo) Then

                    If Not productInfo.ProductVendorStyle = String.Empty Then
                        rlbGxs.Items.Add("Vendor Style: " & productInfo.ProductVendorStyle)
                    End If
                    If Not productInfo.FullProductName = String.Empty Then
                        rlbGxs.Items.Add("Full Product Name: " & productInfo.FullProductName)
                    End If
                    If Not productInfo.UPC = String.Empty Then
                        rlbGxs.Items.Add("UPC: " & productInfo.UPC)
                    End If
                    If Not productInfo.NRFColorCode = String.Empty Then
                        rlbGxs.Items.Add("NRF Color Code: " & productInfo.NRFColorCode)
                    End If
                    If Not productInfo.ColorDescription = String.Empty Then
                        rlbGxs.Items.Add("Color Description: " & productInfo.ColorDescription)
                    End If
                    If Not IsNothing(productInfo.NRFSizeCode) AndAlso productInfo.NRFSizeCode <> 0 Then
                        rlbGxs.Items.Add("NRF Size Code: " & productInfo.NRFSizeCode)
                    End If
                    If Not productInfo.SizeDescription = String.Empty Then
                        rlbGxs.Items.Add("Size Description: " & productInfo.SizeDescription)
                    End If
                    If Not productInfo.BrandName = String.Empty Then
                        rlbGxs.Items.Add("Brand Name: " & productInfo.BrandName)
                    End If
                    If Not productInfo.VendorCollectionName = String.Empty Then
                        rlbGxs.Items.Add("Vendor Collection Name: " & productInfo.VendorCollectionName)
                    End If
                    If Not productInfo.TeamName = String.Empty Then
                        rlbGxs.Items.Add("Team Name: " & productInfo.TeamName)
                    End If
                    If Not productInfo.ConsumerQtyOfUnitInPkg = String.Empty Then
                        rlbGxs.Items.Add("Consumer Qty: " & productInfo.ConsumerQtyOfUnitInPkg)
                    End If
                    If Not productInfo.CountryOfOrigin = String.Empty Then
                        rlbGxs.Items.Add("Country of Origin: " & productInfo.CountryOfOrigin)
                    End If
                    If Not productInfo.FeaturesBenefitsMarketingMessage = String.Empty Then
                        rlbGxs.Items.Add("Marketing Message: " & productInfo.FeaturesBenefitsMarketingMessage)
                    End If
                    If Not productInfo.FabricOfMaterialDescription = String.Empty Then
                        rlbGxs.Items.Add("Fabric Material: " & productInfo.FabricOfMaterialDescription)
                    End If
                    If Not productInfo.LiningMaterial = String.Empty Then
                        rlbGxs.Items.Add("Lining Material: " & productInfo.LiningMaterial)
                    End If
                    If Not productInfo.ConsumerItemLength = String.Empty Then
                        rlbGxs.Items.Add("Item Length: " & productInfo.ConsumerItemLength)
                    End If
                    If Not productInfo.ConsumerItemWidth = String.Empty Then
                        rlbGxs.Items.Add("Item Width: " & productInfo.ConsumerItemWidth)
                    End If
                    If Not productInfo.ConsumerItemDepth = String.Empty Then
                        rlbGxs.Items.Add("Item Depth: " & productInfo.ConsumerItemDepth)
                    End If
                    If Not productInfo.ConsumerItemHeight = String.Empty Then
                        rlbGxs.Items.Add("Item Height: " & productInfo.ConsumerItemHeight)
                    End If
                    If Not productInfo.ConsumerPackageDepth = String.Empty Then
                        rlbGxs.Items.Add("Package Depth: " & productInfo.ConsumerPackageDepth)
                    End If
                    If Not productInfo.ConsumerPackageHeight = String.Empty Then
                        rlbGxs.Items.Add("Package Height: " & productInfo.ConsumerPackageHeight)
                    End If
                    If Not productInfo.ConsumerPackageWidth = String.Empty Then
                        rlbGxs.Items.Add("Package Width: " & productInfo.ConsumerPackageWidth)
                    End If
                    If Not productInfo.ConsumerPackageGrossWeight = String.Empty Then
                        rlbGxs.Items.Add("Package Weight: " & productInfo.ConsumerPackageGrossWeight)
                    End If
                    If Not productInfo.CareInfo = String.Empty Then
                        rlbGxs.Items.Add("Care Info: " & productInfo.CareInfo)
                    End If
                    If Not productInfo.WarrantyDescription = String.Empty Then
                        rlbGxs.Items.Add("Warranty Description: " & productInfo.WarrantyDescription)
                    End If
                    If Not productInfo.ConsumerProductCapacityOrVolume = String.Empty Then
                        rlbGxs.Items.Add("Product Volume: " & productInfo.ConsumerProductCapacityOrVolume)
                    End If
                    If Not productInfo.DoesNotContain = String.Empty Then
                        rlbGxs.Items.Add("Does Not Contain: " & productInfo.DoesNotContain)
                    End If
                    If Not productInfo.AerosolProduct = String.Empty Then
                        rlbGxs.Items.Add("Aerosol Product: " & productInfo.AerosolProduct)
                    End If
                    If Not productInfo.Closure = String.Empty Then
                        rlbGxs.Items.Add("Closure: " & productInfo.Closure)
                    End If
                    If Not productInfo.FauxFur = String.Empty Then
                        rlbGxs.Items.Add("Faux Fur: " & productInfo.FauxFur)
                    End If
                    If Not productInfo.FurAnimalName = String.Empty Then
                        rlbGxs.Items.Add("Fur Animal Name: " & productInfo.FurAnimalName)
                    End If
                    If Not productInfo.FurCountryOfOrigin = String.Empty Then
                        rlbGxs.Items.Add("Fur Country of Origin: " & productInfo.FurCountryOfOrigin)
                    End If
                    If Not productInfo.FurTreatment = String.Empty Then
                        rlbGxs.Items.Add("Fur Treatment: " & productInfo.FurTreatment)
                    End If
                End If

            End If

            'Load default attributes
            Dim _TUGXSCatalog As New TUGXSCatalog
            Dim _gxsCatalogInfo As GXSCatalogInfo = _TUGXSCatalog.GetAllFromTU1135SP(SelectedISN)
            Dim _view As String = GXSCopyViewHelper.GetView(_gxsCatalogInfo.GXSCFG(0).CRG_ID, _gxsCatalogInfo.GXSCFG(0).CMG_ID, _gxsCatalogInfo.GXSCFG(0).CFG_ID, _gxsCatalogInfo.GXSCFG(0).DEPT_ID)
            Dim tuConfigBAO As New TUConfig()
            Dim attribsList As IList(Of TTUConfig) = tuConfigBAO.GetConfigurationByKey(_view)
            Dim isAdded As Boolean = False
            For Each attrib As TTUConfig In attribsList
                If attrib.ConfigDescription <> "Brand Name" AndAlso attrib.ConfigDescription <> "Size Description" AndAlso attrib.ConfigDescription <> "Vendor Collection Name" AndAlso attrib.ConfigDescription <> "Product Name" Then
                    isAdded = False
                    For Each cItem As RadListBoxItem In rlbGxs.Items
                        If cItem.Text.Contains(attrib.ConfigDescription) Then
                            rlbCopy.Items.Add(cItem.Text)
                            isAdded = True
                            Exit For
                        End If
                    Next
                    If Not isAdded Then
                        For Each cItem As RadListBoxItem In rlbSsku.Items
                            If cItem.Text.Contains(attrib.ConfigDescription) Then
                                rlbCopy.Items.Add(cItem.Text)
                                isAdded = True
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try

    End Sub

    Public Function GetFabOrigGxsData(ByVal UPC As Long) As GXSProductInfo
        Try
            Dim gxs As New GXSServiceHelper()
            Dim gxsProductInfo As GXSProductInfo = Nothing

            gxsProductInfo = gxs.FindItemsByGTIN(UPC, CatalogViews.Jewelry)

            Return gxsProductInfo
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Return Nothing
        End Try
    End Function

    Private Sub ShowHideTabs(ByVal TabId As String, ByVal Show As Boolean)
        Try
            Dim aTab As RadTab = rtsCopyPrioritization.FindTabByText(TabId)
            aTab.Visible = Show
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try

    End Sub

    Protected Sub rlbSsku_Transferred(sender As Object, e As RadListBoxTransferredEventArgs)
        txtCopyPreview.Text = String.Empty
        For Each item As RadListBoxItem In rlbCopy.Items
            txtCopyPreview.Text += item.Text & "<BR/>"
        Next
    End Sub

    Protected Sub rigProduct_NeedDataSource(sender As Object, e As ImageGalleryNeedDataSourceEventArgs)
        Dim imageBAO As TUImage = Nothing
        Dim imageDetails As List(Of CopyPrioritizationImageInfo) = Nothing
        Dim sampleRequestList As IList(Of SampleRequestInfo) = Nothing
        Dim featureSample As SampleRequestInfo = Nothing
        Dim sampleInfo As SampleRequestInfo = Nothing
        Dim ecommSetupCreate As TUEcommSetupCreate = Nothing
        Dim rowIndex As Integer = 1
        Try
            If Not IsNothing(CopyPrioritizationInfo) Then
                Dim table As New DataTable()
                table.Columns.Add("ID", GetType(Integer))
                table.Columns.Add("imageURL", GetType(String))
                table.Columns.Add("imageTitle", GetType(String))

                'Get renders and swatches
                imageBAO = New TUImage()
                If CopyPrioritizationInfo.ImageID > 0 Then
                    imageDetails = CType(imageBAO.GetRendersSwatchesByFeatureImageID(CopyPrioritizationInfo.ImageID), List(Of CopyPrioritizationImageInfo))
                End If

                If Not String.IsNullOrEmpty(CopyPrioritizationInfo.PrimaryThumbnailURL) AndAlso CopyPrioritizationInfo.PrimaryThumbnailURL.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) Then
                    CopyPrioritizationInfo.IsFinalImageReady = True
                    table.Rows.Add(rowIndex, "http://s7d4.scene7.com/is/image/BonTon/" & CopyPrioritizationInfo.ImageID & "?$x_large$", CopyPrioritizationInfo.ImageID.ToString())
                    rowIndex = rowIndex + 1
                    For Each imageInfo As CopyPrioritizationImageInfo In imageDetails
                        If Not String.IsNullOrEmpty(imageInfo.LargeImageID) AndAlso table.Select(String.Format("imageURL='{0}'", "http://s7d4.scene7.com/is/image/BonTon/" & imageInfo.LargeImageID & "?$x_large$")).Length = 0 Then
                            table.Rows.Add(rowIndex, "http://s7d4.scene7.com/is/image/BonTon/" & imageInfo.LargeImageID & "?$x_large$", imageInfo.LargeImageID)
                            rowIndex = rowIndex + 1
                        End If
                    Next
                Else
                    If CopyPrioritizationInfo.ISN > 0 OrElse Not CopyPrioritizationInfo.VendorStyleNumber Is Nothing Then
                        ecommSetupCreate = New TUEcommSetupCreate()

                        sampleRequestList = ecommSetupCreate.GetAvailableSampleRequests(0, CopyPrioritizationInfo.ISN, CopyPrioritizationInfo.VendorStyleNumber).Where(
                            Function(x) x.SampleApprovalFlag = "Y"c _
                                And x.SampleApprovalType.ToUpper().Equals("APPROVED")).ToList()

                        If Not sampleRequestList Is Nothing AndAlso sampleRequestList.Count > 0 Then

                            If Not sampleRequestList Is Nothing AndAlso sampleRequestList.Count > 0 Then
                                featureSample = sampleRequestList.ToList().Find(Function(a) a.InternalStyleNum = CopyPrioritizationInfo.ISN AndAlso a.ColorCode = CopyPrioritizationInfo.ColorCode)
                                If Not featureSample Is Nothing Then
                                    CopyPrioritizationInfo.PrimaryThumbnailURL = featureSample.PrimaryActualUrl
                                    table.Rows.Add(rowIndex, CopyPrioritizationInfo.PrimaryThumbnailURL, CopyPrioritizationInfo.ImageID.ToString())
                                    rowIndex = rowIndex + 1

                                    If Not imageDetails Is Nothing AndAlso imageDetails.Count > 0 Then
                                        For Each imageInfo As CopyPrioritizationImageInfo In imageDetails.Where(Function(a) a.ISN = CopyPrioritizationInfo.ISN _
                                                                                                                    AndAlso a.ColorCode <> CopyPrioritizationInfo.ColorCode)
                                            sampleInfo = sampleRequestList.ToList().Find(Function(a) a.InternalStyleNum = imageInfo.ISN AndAlso a.ColorCode = imageInfo.ColorCode)

                                            If Not sampleInfo Is Nothing AndAlso Not String.IsNullOrEmpty(sampleInfo.PrimaryActualUrl) _
                                                AndAlso table.Select(String.Format("imageURL='{0}'", sampleInfo.PrimaryActualUrl)).Length = 0 Then
                                                table.Rows.Add(rowIndex, sampleInfo.PrimaryActualUrl, imageInfo.LargeImageID)
                                                rowIndex = rowIndex + 1
                                            End If
                                        Next
                                    End If
                                Else
                                    For Each imageInfo As CopyPrioritizationImageInfo In imageDetails
                                        sampleInfo = sampleRequestList.ToList().Find(Function(a) a.InternalStyleNum = imageInfo.ISN AndAlso a.ColorCode = imageInfo.ColorCode)

                                        If Not sampleInfo Is Nothing AndAlso Not String.IsNullOrEmpty(sampleInfo.PrimaryActualUrl) _
                                            AndAlso table.Select(String.Format("imageURL='{0}'", sampleInfo.PrimaryActualUrl)).Length = 0 Then
                                            table.Rows.Add(rowIndex, sampleInfo.PrimaryActualUrl, imageInfo.LargeImageID)
                                            rowIndex = rowIndex + 1
                                        End If
                                    Next
                                End If
                            End If
                        End If

                    End If
                End If

                TryCast(sender, RadImageGallery).DataSource = table
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try

    End Sub

    Private Sub ShowHideButtons(ByVal ButtonName As String, ByVal Show As Boolean)
        rtbCopyPrioritization.FindItemByText(ButtonName).Visible = Show
    End Sub

    Private Sub EnableDisableButtons(ByVal ButtonName As String, ByVal Show As Boolean)
        rtbCopyPrioritization.FindItemByText(ButtonName).Enabled = Show
    End Sub

    Private Sub SubmitCopyToWebCat(Optional ByVal IsSetToReady As Boolean = False)
        Dim htmlCopy As String = hdnCopy.Value
        Dim copyInfo As CopyPrioritizationInfo = New CopyPrioritizationInfo
        Dim tuWebCat As TUWebCat = Nothing
        Dim productInfo As List(Of WebcatProductInfo) = Nothing
        Dim errorExists As Boolean = False
        Dim errorMessages As StringBuilder = Nothing
        Dim webcatComments As String = String.Empty
        Dim productDesc As String = String.Empty

        Try
            tuWebCat = New TUWebCat()
            copyInfo = TryCast(Session("CopyPrioritizationInfo"), CopyPrioritizationInfo)

            If IsSetToReady Then
                errorMessages = New StringBuilder()

                If Not ImageExistsInScene7(copyInfo.ImageID) _
                    AndAlso (String.IsNullOrEmpty(copyInfo.PrimaryThumbnailURL) OrElse (Not copyInfo.PrimaryThumbnailURL.Contains("s7d4.scene7") AndAlso _
                                                                              Not copyInfo.PrimaryThumbnailURL.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))) Then
                    errorMessages.AppendLine(" Final Image is required for the Product.")
                End If

                productInfo = tuWebCat.GetProductAndUPCDetailsByProductCode(copyInfo.ProductCode).ToList()

                If errorMessages.Length <= 0 AndAlso Not productInfo Is Nothing AndAlso productInfo.Count > 0 Then

                    'If Not productInfo(0).COLOR_FLAG AndAlso Not productInfo(0).SIZE_FLAG Then
                    '    errorMessages.AppendLine(" Size and/or Color option not set for the Product.")
                    'End If

                    If errorMessages.Length <= 0 AndAlso productInfo(0).AGE_CDE <= 0 Then
                        errorMessages.AppendLine(" Age is required for the Product.")
                    End If

                    If errorMessages.Length <= 0 AndAlso productInfo(0).GENDER_CDE <= 0 Then
                        errorMessages.AppendLine(" Gender is required for the Product.")
                    End If

                    If errorMessages.Length <= 0 AndAlso productInfo(0).DS_RETURN_IND <= 0 Then
                        errorMessages.AppendLine(" Internal return code is required for the Product.")
                    End If

                    If errorMessages.Length <= 0 AndAlso productInfo(0).DS_RETURN_EXT_IND <= 0 Then
                        errorMessages.AppendLine(" External return code is required for the Product.")
                    End If

                    If errorMessages.Length <= 0 AndAlso productInfo(0).BRAND_KEY_NUM <= 0 Then
                        errorMessages.AppendLine(" Brand is required for the Product.")
                    End If

                    If String.IsNullOrEmpty(productInfo(0).PRODUCT_DESC.Trim()) Then
                        productDesc = "&nbsp;"
                    Else
                        productDesc = productInfo(0).PRODUCT_DESC.Trim()
                    End If

                    If errorMessages.Length > 0 Then
                        IsSetToReady = False
                    Else
                        For Each product As WebcatProductInfo In productInfo
                            'If String.IsNullOrEmpty(product.UPCInfo.COLOR_FAMILY) Then
                            '    errorMessages.AppendLine(String.Format(" Color Family is required for the UPC {0}.", product.UPCInfo.UPC_NUM))
                            'End If

                            'If errorMessages.Length <= 0 AndAlso String.IsNullOrEmpty(product.UPCInfo.SIZE_FAMILY) Then
                            '    errorMessages.AppendLine(String.Format(" Size Family is required for the UPC {0}.", product.UPCInfo.UPC_NUM))
                            'End If

                            'If product.COLOR_FLAG Then
                            '    If errorMessages.Length <= 0 AndAlso product.UPCInfo.COLOR_OPTION_CDE <= 0 Then
                            '        errorMessages.AppendLine(String.Format(" Color is required for the UPC {0}.", product.UPCInfo.UPC_NUM))
                            '    End If

                            '    If errorMessages.Length <= 0 AndAlso product.UPCInfo.SWATCH_IMAGE_ID <= 0 Then
                            '        errorMessages.AppendLine(String.Format(" Swatch image id is required for the UPC {0}.", product.UPCInfo.UPC_NUM))
                            '    End If

                            '    If errorMessages.Length <= 0 AndAlso String.IsNullOrEmpty(product.UPCInfo.LARGE_IMAGE_ID) Then
                            '        errorMessages.AppendLine(String.Format(" Large image id is required for the UPC {0}.", product.UPCInfo.UPC_NUM))
                            '    End If
                            'End If

                            'If errorMessages.Length <= 0 AndAlso product.SIZE_FLAG AndAlso product.UPCInfo.SIZE_OPTION_CDE <= 0 Then
                            '    errorMessages.AppendLine(String.Format(" Size is required for the UPC {0}.", product.UPCInfo.UPC_NUM))
                            'End If

                            If errorMessages.Length <= 0 AndAlso product.SIZE_FLAG AndAlso product.UPCInfo.SIZE_OPTION_CDE <= 0 Then
                                errorMessages.AppendLine(String.Format(" No option is set for the UPC {0}.", product.UPCInfo.UPC_NUM))
                            End If

                            If errorMessages.Length > 0 Then
                                IsSetToReady = False
                                Exit For
                            End If
                        Next

                        If errorMessages.Length > 0 Then
                            IsSetToReady = False
                        End If
                    End If
                Else
                    If errorMessages.Length <= 0 Then
                        errorMessages.AppendLine(" No UPCs found for the Product.")
                    End If
                    IsSetToReady = False
                End If
            End If

            If IsSetToReady Then
                webcatComments = String.Concat(DateTime.Now, "- Copy uploaded and set to Ready from Copy Prioritization.")
            Else
                If Not errorMessages Is Nothing Then
                    webcatComments = String.Concat(DateTime.Now, "- Copy uploaded and status unchanged from Copy Prioritization.", errorMessages.ToString())
                Else
                    webcatComments = String.Concat(DateTime.Now, "- Copy uploaded and status unchanged from Copy Prioritization.")
                End If
            End If

            _TUCopyPrioritization.SaveCopy(copyInfo.ProductCode, txtCopyPreview.Content, txtProductName.Text.Trim(), SessionWrapper.UserID, IsSetToReady, webcatComments, productDesc)
            rtsCopyPrioritization.Tabs.Item(0).Selected = True
            rtsCopyPrioritization.Tabs.Item(1).Visible = False
            rmpCopyPrioritization.SelectedIndex = 0
            RetrieveCopyPrioritizationResultList()
            grdCopyPrioritization.Rebind()
            EnableDisableButtons("Save to Pending", False)
            EnableDisableButtons("Set to Ready", False)

            If IsSetToReady Then
                lblUpdateStatus.Text = "Product was set to Ready!"
            Else
                lblUpdateStatus.Text = "Copy submitted successfully!"
            End If

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "HideStatusDiv", "hideStatusDiv();", True)
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try

    End Sub

    Private Function GetImageFilters() As String
        If (CopyPrioritizationSearchCtrl.chkbImageAvailable.Checked AndAlso CopyPrioritizationSearchCtrl.chkbImageNotAvailable.Checked) OrElse
            (Not CopyPrioritizationSearchCtrl.chkbImageAvailable.Checked AndAlso Not CopyPrioritizationSearchCtrl.chkbImageNotAvailable.Checked) Then
            Return String.Empty
        ElseIf CopyPrioritizationSearchCtrl.chkbImageAvailable.Checked Then
            Return "1"
        Else
            Return "0"
        End If
    End Function
    Private Function GetCopyFilters() As String
        If (CopyPrioritizationSearchCtrl.chkbCopyReady.Checked AndAlso CopyPrioritizationSearchCtrl.chkbCopyNotReady.Checked) OrElse
            (Not CopyPrioritizationSearchCtrl.chkbCopyReady.Checked AndAlso Not CopyPrioritizationSearchCtrl.chkbCopyNotReady.Checked) Then
            Return String.Empty
        ElseIf CopyPrioritizationSearchCtrl.chkbCopyReady.Checked Then
            Return "1"
        Else
            Return "0"
        End If
    End Function
    Private Function GetSKUUseFilters() As String
        Dim skuUseList As List(Of TMS900PARAMETERInfo) = Nothing
        Dim skuUseInfo As TMS900PARAMETERInfo = Nothing
        Dim selectedSKUUses As List(Of String) = Nothing
        Dim skuFilters As String = String.Empty

        Try
            _TUTMS900Parameter = New TUTMS900PARAMETER()

            skuUseList = CType(_TUTMS900Parameter.GetAllSKUUse(), List(Of TMS900PARAMETERInfo))
            selectedSKUUses = New List(Of String)()

            If CopyPrioritizationSearchCtrl.chkbBasic.Checked Then
                skuUseInfo = skuUseList.Find(Function(a) a.ShortDesc.Trim().ToUpper() = CopyPrioritizationSearchCtrl.chkbBasic.Text.ToUpper())
                If Not skuUseInfo Is Nothing Then
                    selectedSKUUses.Add(skuUseInfo.CharIndex)
                End If
            End If

            If CopyPrioritizationSearchCtrl.chkbFashion.Checked Then
                skuUseInfo = skuUseList.Find(Function(a) a.ShortDesc.Trim().ToUpper() = CopyPrioritizationSearchCtrl.chkbFashion.Text.ToUpper())
                If Not skuUseInfo Is Nothing Then
                    selectedSKUUses.Add(skuUseInfo.CharIndex)
                End If
            End If

            If CopyPrioritizationSearchCtrl.chkbPWP.Checked Then
                skuUseInfo = skuUseList.Find(Function(a) a.ShortDesc.Trim().ToUpper() = CopyPrioritizationSearchCtrl.chkbPWP.Text.ToUpper())
                If Not skuUseInfo Is Nothing Then
                    selectedSKUUses.Add(skuUseInfo.CharIndex)
                End If
            End If

            If CopyPrioritizationSearchCtrl.chkbGWP.Checked Then
                skuUseInfo = skuUseList.Find(Function(a) a.ShortDesc.Trim().ToUpper() = CopyPrioritizationSearchCtrl.chkbGWP.Text.ToUpper())
                If Not skuUseInfo Is Nothing Then
                    selectedSKUUses.Add(skuUseInfo.CharIndex)
                End If
            End If

            If CopyPrioritizationSearchCtrl.chkbSpecial.Checked Then
                skuUseInfo = skuUseList.Find(Function(a) a.ShortDesc.Trim().ToUpper() = CopyPrioritizationSearchCtrl.chkbSpecial.Text.ToUpper())
                If Not skuUseInfo Is Nothing Then
                    selectedSKUUses.Add(skuUseInfo.CharIndex)
                End If
            End If

            If CopyPrioritizationSearchCtrl.chkbCollateral.Checked Then
                skuUseInfo = skuUseList.Find(Function(a) a.ShortDesc.Trim().ToUpper() = CopyPrioritizationSearchCtrl.chkbCollateral.Text.ToUpper())
                If Not skuUseInfo Is Nothing Then
                    selectedSKUUses.Add(skuUseInfo.CharIndex)
                End If
            End If

            If CopyPrioritizationSearchCtrl.chkbVirtualGC.Checked Then
                skuUseInfo = skuUseList.Find(Function(a) a.ShortDesc.Trim().ToUpper() = CopyPrioritizationSearchCtrl.chkbVirtualGC.Text.ToUpper())
                If Not skuUseInfo Is Nothing Then
                    selectedSKUUses.Add(skuUseInfo.CharIndex)
                End If
            End If

            If CopyPrioritizationSearchCtrl.chkbPlasticGC.Checked Then
                skuUseInfo = skuUseList.Find(Function(a) a.ShortDesc.Trim().ToUpper() = CopyPrioritizationSearchCtrl.chkbPlasticGC.Text.ToUpper())
                If Not skuUseInfo Is Nothing Then
                    selectedSKUUses.Add(skuUseInfo.CharIndex)
                End If
            End If

            If selectedSKUUses.Count > 0 Then
                skuFilters = String.Join("|", selectedSKUUses)
            End If
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try

        Return skuFilters

    End Function

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Try
            With grdCopyPrioritization

                .MasterTableView.Caption = "Copy Prioritization"
                .AllowPaging = False
                .AllowSorting = False
                .ExportSettings.OpenInNewWindow = True
                .ExportSettings.ExportOnlyData = True
                .ExportSettings.IgnorePaging = True
                .ExportSettings.Excel.Format = GridExcelExportFormat.Xlsx

                .MasterTableView.Columns.FindByUniqueName("Select").Visible = False
                .MasterTableView.Columns.FindByUniqueName("ProductNotes").Visible = True
                .MasterTableView.Columns.FindByUniqueName("Image").Visible = False
                .MasterTableView.ExportToExcel()

            End With
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try

    End Sub

    Private Function ImageExistsInScene7(ByVal ImageID As Integer) As Boolean
        Try
            Dim httpRequest As HttpWebRequest = CType(WebRequest.Create(String.Concat("http://s7d4.scene7.com/is/image/BonTon/", ImageID.ToString(), "?defaultImage")), HttpWebRequest)
            httpRequest.UseDefaultCredentials = True
            httpRequest.Method = "HEAD"
            Dim httpResponse As HttpWebResponse = CType(httpRequest.GetResponse(), HttpWebResponse)
            Return httpResponse.StatusCode = HttpStatusCode.OK
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class