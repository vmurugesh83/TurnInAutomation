Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL

Public Class GXSCatalogueExtendedPropertiesCtrl
    Inherits System.Web.UI.UserControl

    Dim _view As String
    Dim _gxsCatalogInfo As GXSCatalogInfo

    Public Sub New(ByVal gxsCatalogInfo As GXSCatalogInfo)
        _gxsCatalogInfo = gxsCatalogInfo
    End Sub

    Public Sub New()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        SetView()
        PopulateProperties()
    End Sub

    Private Sub SetView()

        _view = GXSCopyViewHelper.GetView(_gxsCatalogInfo.GXSCFG(0).CRG_ID, _gxsCatalogInfo.GXSCFG(0).CMG_ID, _gxsCatalogInfo.GXSCFG(0).CFG_ID, _gxsCatalogInfo.GXSCFG(0).DEPT_ID)

        'For testing purposes
        '_view = GetView(100, 1, 1, 1)           'Apparel
        '_view = GetView(500, 412, 2, 2)         'Footwear
        '_view = GetView(500, 411, 240, 3)       'Jewelry
        '_view = GetView(300, 509, 190, 435)     'Accessories
        '_view = GetView(300, 4, 190, 4)         'Apparel Exclusion (No view)
        '_view = GetView(400, 5, 5, 5)           'Beauty
        '_view = GetView(600, 6, 6, 6)           'Home
        '_view = GetView(300, 7, 191, 7)         'Apparel

        SetViewVisibility()
        SetViewColoring()

    End Sub

    Private Sub SetViewVisibility()

        Select Case _view
            Case "Apparel"
                '
                divConsumerItemDepth.Visible = False
                divConsumerItemHeight.Visible = False
                '
                divHeelHeight.Visible = False
                divPlatformHeight.Visible = False
                divSoleType.Visible = False
                divBootLegCircumference.Visible = False
                divBootShaftHeight.Visible = False
                '
                divGoldCarat.Visible = False
                divStoneDetails.Visible = False
                divClosure.Visible = False
                '
                divEarringsDrop.Visible = False
                divWatchBandWidth.Visible = False
                divWatchCaseSize.Visible = False
                '
                divHandbagShoulderDrop.Visible = False
                '
                divConsumerPackageDepth.Visible = False
                divConsumerPackageHeight.Visible = False
                divConsumerPackageWidth.Visible = False
                divConsumerPackageGrossWeight.Visible = False
                '
                divCareInfo2.Visible = False
                '
                divWarrantyDescription.Visible = False
                divConsumerProductCapacity.Visible = False
                divKeyActiveIngredient.Visible = False
                divDoesNotContain.Visible = False
                divAerosolProduct.Visible = False
                '
                divClosure3.Visible = False
                '

            Case "Footwear"
                '
                divConsumerItemLength.Visible = False
                divConsumerItemWidth.Visible = False
                divConsumerItemDepth.Visible = False
                divConsumerItemHeight.Visible = False
                '
                divClosure2.Visible = False
                divCollarType.Visible = False
                divPantInseamLength.Visible = False
                divSleeveMeasurement.Visible = False
                divSleeveType.Visible = False
                '
                divGoldCarat.Visible = False
                divStoneDetails.Visible = False
                divClosure.Visible = False
                '
                divEarringsDrop.Visible = False
                divWatchBandWidth.Visible = False
                divWatchCaseSize.Visible = False
                '
                divHandbagShoulderDrop.Visible = False
                '
                divConsumerPackageDepth.Visible = False
                divConsumerPackageHeight.Visible = False
                divConsumerPackageWidth.Visible = False
                divConsumerPackageGrossWeight.Visible = False
                '
                divCareInfo2.Visible = False
                '
                divWarrantyDescription.Visible = False
                divConsumerProductCapacity.Visible = False
                divKeyActiveIngredient.Visible = False
                divDoesNotContain.Visible = False
                divAerosolProduct.Visible = False
                '

            Case "Jewelry"
                '
                divLiningMaterial.Visible = False
                '
                divCareInfo.Visible = False
                '                '                '
                divClosure2.Visible = False
                divCollarType.Visible = False
                divPantInseamLength.Visible = False
                divSleeveMeasurement.Visible = False
                divSleeveType.Visible = False
                '
                divHeelHeight.Visible = False
                divPlatformHeight.Visible = False
                divSoleType.Visible = False
                divBootLegCircumference.Visible = False
                divBootShaftHeight.Visible = False
                '
                divHandbagShoulderDrop.Visible = False
                '
                divConsumerPackageDepth.Visible = False
                divConsumerPackageHeight.Visible = False
                divConsumerPackageWidth.Visible = False
                divConsumerPackageGrossWeight.Visible = False
                '
                divCareInfo2.Visible = False
                '
                divConsumerProductCapacity.Visible = False
                divKeyActiveIngredient.Visible = False
                divDoesNotContain.Visible = False
                divAerosolProduct.Visible = False
                '
                divClosure3.Visible = False
                '

            Case "Accessories"
                '                '
                divClosure2.Visible = False
                divCollarType.Visible = False
                divPantInseamLength.Visible = False
                divSleeveMeasurement.Visible = False
                divSleeveType.Visible = False
                '
                divHeelHeight.Visible = False
                divPlatformHeight.Visible = False
                divSoleType.Visible = False
                divBootLegCircumference.Visible = False
                divBootShaftHeight.Visible = False
                '
                divClosure.Visible = False
                '
                divEarringsDrop.Visible = False
                divWatchBandWidth.Visible = False
                divWatchCaseSize.Visible = False
                '
                divConsumerPackageDepth.Visible = False
                divConsumerPackageHeight.Visible = False
                divConsumerPackageWidth.Visible = False
                divConsumerPackageGrossWeight.Visible = False
                '
                divCareInfo2.Visible = False
                '
                divWarrantyDescription.Visible = False
                divConsumerProductCapacity.Visible = False
                divKeyActiveIngredient.Visible = False
                divDoesNotContain.Visible = False
                divAerosolProduct.Visible = False
                '

            Case "Beauty"
                '
                divTeamName.Visible = False
                divFabricOrMaterialDescription.Visible = False
                divLiningMaterial.Visible = False
                '
                divCareInfo.Visible = False
                '
                divConsumerItemLength.Visible = False
                divConsumerItemWidth.Visible = False
                divConsumerItemDepth.Visible = False
                divConsumerItemHeight.Visible = False
                '
                divClosure2.Visible = False
                divCollarType.Visible = False
                divPantInseamLength.Visible = False
                divSleeveMeasurement.Visible = False
                divSleeveType.Visible = False
                '
                divHeelHeight.Visible = False
                divPlatformHeight.Visible = False
                divSoleType.Visible = False
                divBootLegCircumference.Visible = False
                divBootShaftHeight.Visible = False
                '
                divGoldCarat.Visible = False
                divStoneDetails.Visible = False
                divClosure.Visible = False
                '
                divEarringsDrop.Visible = False
                divWatchBandWidth.Visible = False
                divWatchCaseSize.Visible = False
                '
                divHandbagShoulderDrop.Visible = False
                '
                divConsumerPackageDepth.Visible = False
                divConsumerPackageHeight.Visible = False
                divConsumerPackageWidth.Visible = False
                divConsumerPackageGrossWeight.Visible = False
                '
                divCareInfo2.Visible = False
                '
                divWarrantyDescription.Visible = False
                '
                divClosure3.Visible = False
                '
                divFauxFur.Visible = False
                divFurAnimalName.Visible = False
                divFurCountryOfOrigin.Visible = False
                divFurTreatment.Visible = False

            Case "Home"
                '
                divCareInfo.Visible = False
                '
                divClosure2.Visible = False
                divCollarType.Visible = False
                divPantInseamLength.Visible = False
                divSleeveMeasurement.Visible = False
                divSleeveType.Visible = False
                '
                divHeelHeight.Visible = False
                divPlatformHeight.Visible = False
                divSoleType.Visible = False
                divBootLegCircumference.Visible = False
                divBootShaftHeight.Visible = False
                '
                divGoldCarat.Visible = False
                divStoneDetails.Visible = False
                divClosure.Visible = False
                '
                divEarringsDrop.Visible = False
                divWatchBandWidth.Visible = False
                divWatchCaseSize.Visible = False
                '
                divHandbagShoulderDrop.Visible = False
                '
                divKeyActiveIngredient.Visible = False
                '
            Case Else
                'Hide all controls
                divCareInfo.Visible = False
                '
                divConsumerItemLength.Visible = False
                divConsumerItemWidth.Visible = False
                divConsumerItemDepth.Visible = False
                divConsumerItemHeight.Visible = False
                '
                divClosure2.Visible = False
                divCollarType.Visible = False
                divPantInseamLength.Visible = False
                divSleeveMeasurement.Visible = False
                divSleeveType.Visible = False
                '
                divHeelHeight.Visible = False
                divPlatformHeight.Visible = False
                divSoleType.Visible = False
                divBootLegCircumference.Visible = False
                divBootShaftHeight.Visible = False
                '
                divGoldCarat.Visible = False
                divStoneDetails.Visible = False
                divClosure.Visible = False
                '
                divEarringsDrop.Visible = False
                divWatchBandWidth.Visible = False
                divWatchCaseSize.Visible = False
                '
                divHandbagShoulderDrop.Visible = False
                '
                divConsumerPackageDepth.Visible = False
                divConsumerPackageHeight.Visible = False
                divConsumerPackageWidth.Visible = False
                divConsumerPackageGrossWeight.Visible = False
                '
                divCareInfo2.Visible = False
                '
                divWarrantyDescription.Visible = False
                divConsumerProductCapacity.Visible = False
                divKeyActiveIngredient.Visible = False
                divDoesNotContain.Visible = False
                divAerosolProduct.Visible = False
                '
                divClosure3.Visible = False
                '
                divFauxFur.Visible = False
                divFurAnimalName.Visible = False
                divFurCountryOfOrigin.Visible = False
                divFurTreatment.Visible = False
                '

        End Select
    End Sub

    Private Sub SetViewColoring()

        Dim divBlue As String = "tdGXS tdGXSBlue"
        Dim divWhite As String = "tdGXS"

        'Column 1
        spVendorStyle.Attributes.Add("class", divBlue)
        spFullProductName.Attributes.Add("class", divWhite)
        spUPC.Attributes.Add("class", divBlue)
        spNRFColorCode.Attributes.Add("class", divWhite)
        spColorDescription.Attributes.Add("class", divBlue)
        spNRFSizeCode.Attributes.Add("class", divWhite)
        spSizeDescription.Attributes.Add("class", divBlue)
        spBrandName.Attributes.Add("class", divWhite)
        spVendorCollectionName.Attributes.Add("class", divBlue)
        spTeamName.Attributes.Add("class", divWhite)
        spConsumerQtyOfUnitsInPkg.Attributes.Add("class", divBlue)
        spCountryOfOrigin.Attributes.Add("class", divWhite)
        spMarketingMessage.Attributes.Add("class", divBlue)
        spFabricOrMaterialDescription.Attributes.Add("class", divWhite)
        spLiningMaterials.Attributes.Add("class", divBlue)

        Select _view
            Case "Apparel"

                'Column 2
                spCareInfo.Attributes.Add("class", divWhite)
                spConsumerItemLength.Attributes.Add("class", divBlue)
                spConsumerItemWidth.Attributes.Add("class", divWhite)
                spClosure2.Attributes.Add("class", divBlue)
                spCollarType.Attributes.Add("class", divWhite)
                spPantInseamLength.Attributes.Add("class", divBlue)
                spSleeveMeasurement.Attributes.Add("class", divWhite)
                spSleeveType.Attributes.Add("class", divBlue)
                spFauxFur.Attributes.Add("class", divWhite)
                spFurAnimalName.Attributes.Add("class", divBlue)
                spFurCountryOfOrigin.Attributes.Add("class", divWhite)
                spFurTreatment.Attributes.Add("class", divBlue)

            Case "Footwear"

                spCareInfo.Attributes.Add("class", divWhite)
                spHeelHeight.Attributes.Add("class", divBlue)
                spPlatformHeight.Attributes.Add("class", divWhite)
                spSoleType.Attributes.Add("class", divBlue)
                spBootLegCircumference.Attributes.Add("class", divWhite)
                spBootShaftHeight.Attributes.Add("class", divBlue)
                spClosure3.Attributes.Add("class", divWhite)
                spFauxFur.Attributes.Add("class", divBlue)
                spFurAnimalName.Attributes.Add("class", divWhite)
                spFurCountryOfOrigin.Attributes.Add("class", divBlue)
                spFurTreatment.Attributes.Add("class", divWhite)

            Case "Jewelry"

                spGoldCarat.Attributes.Add("class", divWhite)
                spStoneDetails.Attributes.Add("class", divBlue)
                spClosure.Attributes.Add("class", divWhite)
                spConsumerItemLength.Attributes.Add("class", divBlue)
                spConsumerItemWidth.Attributes.Add("class", divWhite)
                spConsumerItemDepth.Attributes.Add("class", divBlue)
                spConsumerItemHeight.Attributes.Add("class", divWhite)
                spEarringsDrop.Attributes.Add("class", divBlue)
                spWatchBandWidth.Attributes.Add("class", divWhite)
                spWatchCaseSize.Attributes.Add("class", divBlue)
                spWarrantyDescription.Attributes.Add("class", divWhite)
                spFauxFur.Attributes.Add("class", divBlue)
                spFurAnimalName.Attributes.Add("class", divWhite)
                spFurCountryOfOrigin.Attributes.Add("class", divBlue)
                spFurTreatment.Attributes.Add("class", divWhite)

            Case "Accessories"

                spGoldCarat.Attributes.Add("class", divWhite)
                spStoneDetails.Attributes.Add("class", divBlue)
                spCareInfo.Attributes.Add("class", divWhite)
                spConsumerItemLength.Attributes.Add("class", divBlue)
                spConsumerItemWidth.Attributes.Add("class", divWhite)
                spConsumerItemDepth.Attributes.Add("class", divBlue)
                spConsumerItemHeight.Attributes.Add("class", divWhite)
                spHandbagShoulderDrop.Attributes.Add("class", divBlue)
                spClosure3.Attributes.Add("class", divWhite)
                spFauxFur.Attributes.Add("class", divBlue)
                spFurAnimalName.Attributes.Add("class", divWhite)
                spFurCountryOfOrigin.Attributes.Add("class", divBlue)
                spFurTreatment.Attributes.Add("class", divWhite)

            Case "Beauty"
                'Column 1
                spConsumerQtyOfUnitsInPkg.Attributes.Add("class", divWhite)
                spCountryOfOrigin.Attributes.Add("class", divBlue)
                spMarketingMessage.Attributes.Add("class", divWhite)
                'Column 2
                spConsumerProductCapacity.Attributes.Add("class", divWhite)
                spKeyActiveIngredient.Attributes.Add("class", divBlue)
                spDoesNotContain.Attributes.Add("class", divWhite)
                spAerosolProduct.Attributes.Add("class", divBlue)

            Case "Home"

                spConsumerItemLength.Attributes.Add("class", divWhite)
                spConsumerItemWidth.Attributes.Add("class", divBlue)
                spConsumerItemDepth.Attributes.Add("class", divWhite)
                spConsumerItemHeight.Attributes.Add("class", divBlue)
                spConsumerPackageDepth.Attributes.Add("class", divWhite)
                spConsumerPackageHeight.Attributes.Add("class", divBlue)
                spConsumerPackageWidth.Attributes.Add("class", divWhite)
                spConsumerPackageGrossWeight.Attributes.Add("class", divBlue)
                spCareInfo2.Attributes.Add("class", divWhite)
                spWarrantyDescription.Attributes.Add("class", divBlue)
                spConsumerProductCapacity.Attributes.Add("class", divWhite)
                spDoesNotContain.Attributes.Add("class", divBlue)
                spAerosolProduct.Attributes.Add("class", divWhite)
                spClosure3.Attributes.Add("class", divBlue)
                spFauxFur.Attributes.Add("class", divWhite)
                spFurAnimalName.Attributes.Add("class", divBlue)
                spFurCountryOfOrigin.Attributes.Add("class", divWhite)
                spFurTreatment.Attributes.Add("class", divBlue)

            Case Else
        End Select
    End Sub

    Private Sub PopulateProperties()

        Dim gxs As New GXSServiceHelper
        'Dim gxsProductInfo As GXSProductInfo = gxs.FindItemsByProduct("128035870700", "010377")
        Dim UPC_NUM As Long = CLng(CDec(Me.Request.QueryString("upc")))
        Dim gxsProductInfo As GXSProductInfo = Nothing
        Select Case _view
            Case "Apparel"
                gxsProductInfo = gxs.FindItemsByGTIN(UPC_NUM, CatalogViews.Apparel)
            Case "Footwear"
                gxsProductInfo = gxs.FindItemsByGTIN(UPC_NUM, CatalogViews.Footwear)
            Case "Jewelry"
                gxsProductInfo = gxs.FindItemsByGTIN(UPC_NUM, CatalogViews.Jewelry)
            Case "Accessories"
                gxsProductInfo = gxs.FindItemsByGTIN(UPC_NUM, CatalogViews.HandBagAccessories)
            Case "Beauty"
                gxsProductInfo = gxs.FindItemsByGTIN(UPC_NUM, CatalogViews.Beauty)
            Case "Home"
                gxsProductInfo = gxs.FindItemsByGTIN(UPC_NUM, CatalogViews.Home)
        End Select
        If gxsProductInfo IsNot Nothing Then
            With gxsProductInfo

                Me.txtVendorStyle.Text = .ProductVendorStyle
                Me.txtFullProductName.Text = .FullProductName
                Me.txtUPC.Text = .UPC
                Me.txtColorDescription.Text = .ColorDescription
                Me.txtNRFColorCode.Text = .NRFColorCode
                Me.txtSizeDescription.Text = .SizeDescription
                Me.txtNRFSizeCode.Text = CStr(.NRFSizeCode)
                '
                Me.txtBrandName.Text = .BrandName
                Me.txtVendorCollectionName.Text = .VendorCollectionName
                Me.txtTeamName.Text = .TeamName
                Me.txtConsumerQtyOfUnitsInPkg.Text = If(.ConsumerQtyOfUnitInPkg Is Nothing, "", .ConsumerQtyOfUnitInPkg.ToString)
                Me.txtCountryOfOrigin.Text = .CountryOfOrigin
                Me.txtMarketingMessage.Text = .FeaturesBenefitsMarketingMessage
                Me.txtFabricOrMaterialDescription.Text = .FabricOfMaterialDescription
                Me.txtLiningMaterial.Text = .LiningMaterial

                Select Case _view
                    Case "Apparel"

                        Me.txtConsumerItemLength.Text = .ConsumerItemLength
                        Me.txtConsumerItemWidth.Text = .ConsumerItemWidth
                        Me.txtCollarType.Text = .CollarType
                        Me.txtPantInseamLength.Text = .PantInseamLength
                        Me.txtSleeveMeasurement.Text = .SleeveMeasurement
                        Me.txtSleeveType.Text = .SleeveType

                    Case "Footwear"

                        Me.txtHeelHeight.Text = .HeelHeight
                        Me.txtPlatformHeight.Text = .PlatformHeight
                        Me.txtSoleType.Text = .SoleType
                        Me.txtBootLegCircumference.Text = .BootLegCircumference
                        Me.txtBootShaftHeight.Text = .BootShaftHeight

                    Case "Jewelry"

                        Me.txtGoldCarat.Text = .GoldKarat
                        Me.txtStoneDetails.Text = .StoneDetails
                        Me.txtConsumerItemLength.Text = .ConsumerItemLength
                        Me.txtConsumerItemWidth.Text = .ConsumerItemWidth
                        Me.txtConsumerItemDepth.Text = .ConsumerItemDepth
                        Me.txtConsumerItemHeight.Text = .ConsumerItemHeight
                        Me.txtEarringsDrop.Text = .EarringsDrop
                        Me.txtWatchBandWidth.Text = .WatchBandWidth
                        Me.txtWatchCaseSize.Text = .WatchCaseSize
                        Me.txtWarrantyDescription.Text = .WarrantyDescription

                    Case "Accessories"

                        Me.txtGoldCarat.Text = .GoldKarat
                        Me.txtStoneDetails.Text = .StoneDetails
                        Me.txtConsumerItemLength.Text = .ConsumerItemLength
                        Me.txtConsumerItemWidth.Text = .ConsumerItemWidth
                        Me.txtConsumerItemDepth.Text = .ConsumerItemDepth
                        Me.txtConsumerItemHeight.Text = .ConsumerItemHeight
                        Me.txtHandbagShoulderDrop.Text = .HandbagShoulderDrop

                    Case "Beauty"

                        Me.txtKeyActiveIngredient.Text = .KeyActiveIngredient

                    Case "Home"

                        Me.txtConsumerItemLength.Text = .ConsumerItemLength
                        Me.txtConsumerItemWidth.Text = .ConsumerItemWidth
                        Me.txtConsumerItemDepth.Text = .ConsumerItemDepth
                        Me.txtConsumerItemHeight.Text = .ConsumerItemHeight
                        Me.txtConsumerPackageDepth.Text = .ConsumerPackageDepth
                        Me.txtConsumerPackageHeight.Text = .ConsumerPackageHeight
                        Me.txtConsumerPackageWidth.Text = .ConsumerPackageWidth
                        Me.txtConsumerPackageGrossWeight.Text = .ConsumerPackageGrossWeight
                        Me.txtWarrantyDescription.Text = .WarrantyDescription
                        Me.txtConsumerProductCapacity.Text = .ConsumerProductCapacityOrVolume
                        Me.txtDoesNotContain.Text = .DoesNotContain
                        Me.txtAerosolProduct.Text = .AerosolProduct

                End Select

                Me.txtCareInfo.Text = .CareInfo
                Me.txtCareInfo2.Text = .CareInfo

                Me.txtClosure.Text = .Closure
                Me.txtClosure2.Text = .Closure
                Me.txtClosure3.Text = .Closure

                Me.txtFauxFur.Text = .FauxFur
                Me.txtFurAnimalName.Text = .FurAnimalName
                Me.txtFurCountryOfOrigin.Text = .FurCountryOfOrigin
                Me.txtFurTreatment.Text = .FurTreatment

            End With
        End If

    End Sub

End Class