Imports TurnInProcessAutomation.BLL
Imports TurnInProcessAutomation.BusinessEntities

Public Class GXSCatalogueStyleSKUPropertiesCtrl
    Inherits System.Web.UI.UserControl

    Private _gxsStyleSkuInfo As IList(Of GXSStyleSkuInfo)

    Public Sub New(ByVal gxsStyleSkuInfo As IList(Of GXSStyleSkuInfo))
        _gxsStyleSkuInfo = gxsStyleSkuInfo
    End Sub

    Public Sub New()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PopulateProperties()
    End Sub

    Private Sub PopulateProperties()

        If _gxsStyleSkuInfo IsNot Nothing AndAlso _gxsStyleSkuInfo.Count > 0 Then
            With _gxsStyleSkuInfo(0)
                Me.txtFabrication.Text = .Fabrication
                Me.txtSellingLoc.Text = .SellingLoc
                Me.txtProdDtl1.Text = .ProdDtl1
                Me.txtProdDtl2.Text = .ProdDtl2
                Me.txtProdDtl3.Text = .ProdDtl3
                Me.txtAssembledIn.Text = .AssembledIn
                Me.txtGenClass.Text = .GenClass
                Me.txtGenSubClass.Text = .GenSubcl
                Me.txtBrand.Text = .Brand
                Me.txtLabel.Text = .Label
                Me.txtFabDtl.Text = .FabDtl
                Me.txtLifestyle.Text = .Lifestyle
                Me.txtSeason.Text = .Season
                Me.txtOccasion.Text = .Occasion
                Me.txtTheme.Text = .Theme
            End With
        End If

    End Sub

End Class