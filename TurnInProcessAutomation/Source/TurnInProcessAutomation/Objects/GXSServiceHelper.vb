Imports System.ServiceModel
Imports TurnInProcessAutomation.com.gxs.catalogue
Imports System.Net
Imports System.Web.Services.Protocols

Imports System.Configuration
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Text
Imports System.IO
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.FileIO
Imports System.Data.SqlClient
Imports System.Web
Imports System.Xml
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.BLL

Public Enum CatalogViews
    Apparel
    Footwear
    Jewelry
    HandBagAccessories
    Beauty
    Home
End Enum

Public Class GXSServiceHelper

    Dim GXSAddress As String = "https://catalogue.gxs.com/QRSGUI/services/ItemWebService"
    Dim BontonGXS_Account As Long = 124143475354
    Dim BonTonGXS_NetworkID As String = "PAB1"  ' PAB1 Login to UI
    Dim BonTonGXS_UserName As String = "WEBSRVC" ' BYB102 Login to UI
    Dim BonTonGXS_Password As String = "WEBSRVC" ' UPCCAT01 Login to UI

    Private _GXSProductInfo As GXSProductInfo

    Function FindItemsByGTIN(ByVal GTIN As Long, ByVal View As CatalogViews) As GXSProductInfo
        Try
            Dim user As New com.gxs.catalogue.UserIdentifier
            user.accountIdSpecified = True
            user.accountId = BontonGXS_Account

            user.networkId = BonTonGXS_NetworkID
            user.username = BonTonGXS_UserName
            user.password = BonTonGXS_Password

            Dim _TUVendor As New TUVendor
            Dim vendorAccountID As Long = _TUVendor.GetExternalVendorNumberByUPC(GTIN)

            Dim itemId As New com.gxs.catalogue.ItemIdentifier
            itemId.gtinSpecified = True
            itemId.gtin = GTIN

            itemId.gtinTypeSpecified = True
            itemId.accountSpecified = True
            itemId.account = vendorAccountID
            itemId.gtinType = GtinType.UP

            Dim itemOpt As New com.gxs.catalogue.ItemOptions
            itemOpt.attributeFilterName = "GMA Filter Extended"
            itemOpt.exportOptions = Nothing
            itemOpt.returnContainersSpecified = False
            itemOpt.returnComponents = False
            itemOpt.returnComponentsSpecified = False
            itemOpt.returnContainers = False

            Dim proxy As New ItemWebService
            proxy.AllowAutoRedirect = True
            proxy.Url = GXSAddress

            Dim item As New Item
            item = proxy.getItem(user, itemId, itemOpt)

            ProcessItem(View, item)

            ' Reponses to SOAP requests can take one of two forms: success responses or error responses. 
            ' For an error response, the response could contain either HTTP errors or SOAP faults. 
            ' A success response is always a SOAP message. 
        Catch ex As WebException
            'If ex.Status = WebExceptionStatus.ProtocolError Then
            '    Console.WriteLine("URL :: " & ex.Response.ResponseUri.ToString)
            '    Dim httpResponse As HttpWebResponse = CType(ex.Response, HttpWebResponse)
            '    Console.WriteLine(CInt(httpResponse.StatusCode).ToString() & _
            '       " - " & httpResponse.StatusCode.ToString())

            '    Console.WriteLine("Status Code : {0}", CType(ex.Response, HttpWebResponse).StatusCode)
            '    Console.WriteLine("Status Description : {0}", CType(ex.Response, HttpWebResponse).StatusDescription)
            'Else
            '    Console.WriteLine(" Stack Trace : {0}", ex.StackTrace)
            'End If
        Catch s As SoapException
            'Console.WriteLine(s.Message)
            'Console.WriteLine(s.StackTrace)
        Finally
            ' Console.Read()
        End Try
        Return _GXSProductInfo
    End Function

    Private Sub ProcessItem(ByVal CatalogView As CatalogViews, ByVal i As Item)
        If IsNothing(i) Then
            Console.WriteLine("No records returned.")
            Exit Sub
        End If

        _GXSProductInfo = New GXSProductInfo

        CoreCommon(i)
        eCommCommon(i)
        SupplyChainCommon(i)
        'SupplyChainSpecific(i)
        eCommSpecific(i)
    End Sub

    Private Sub CoreCommon(ByVal i As Item)

        With _GXSProductInfo
            .ProductVendorStyle = i.productName
            .FullProductName = i.productDescription + i.productExtendedDescription
            .UPC = i.gtinString
            .ColorDescription = i.commonRetailGroup.colorDescription
            .NRFColorCode = i.commonRetailGroup.nrfColorCode.ToString
            .SizeDescription = i.commonRetailGroup.sizeDescription
        End With

    End Sub

    Private Sub eCommCommon(ByVal i As Item)

        With _GXSProductInfo
            'Column1
            .BrandName = i.commonRetailGroup.tradeName
            If i.gdsnOptionalGroup IsNot Nothing Then
                .VendorCollectionName = i.gdsnOptionalGroup.vendorCollection
                'Else
                '    .VendorCollectionName = "Not Found"
            End If
            If Not IsNothing(i.extendedRetailGroup) Then
                If i.extendedRetailGroup.featureBenefitList.Length > 0 Then
                    .FeaturesBenefitsMarketingMessage = i.extendedRetailGroup.featureBenefitList(0).ToString
                End If
                .TeamName = i.extendedRetailGroup.team
                '.LiningMaterial = i.extendedRetailGroup.liningMaterial
            Else
                '.FeaturesBenefitsMarketingMessage = "Features - Benefits - Marketing Message : Not Found"
                '.TeamName = "Not Found"
                '.LiningMaterial = "Not Found"
            End If

            If i.commonLogisticsGroup.originCountryList.Length > 0 Then
                .CountryOfOrigin = i.commonLogisticsGroup.originCountryList(0).ToString
                'Else
                '    .CountryOfOrigin = "Not Found."
            End If
            .FabricOfMaterialDescription = i.commonRetailGroup.fabricDescription

            'Column 2
            If Not IsNothing(i.extendedLogisticsGroup) Then
                .ConsumerItemLength = i.extendedLogisticsGroup.consumerLengthDescription
                .ConsumerQtyOfUnitInPkg = CStr(i.extendedLogisticsGroup.contentPerConsumerPackage)
                If Not IsNothing(i.extendedLogisticsGroup.itemClearanceWidth) Then
                    .ConsumerItemWidth = i.extendedLogisticsGroup.itemClearanceWidth.value.ToString & i.extendedLogisticsGroup.itemClearanceWidth.uom.ToString
                    .ConsumerItemDepth = i.extendedLogisticsGroup.itemClearanceLength.value.ToString & i.extendedLogisticsGroup.itemClearanceLength.uom.ToString
                    .ConsumerItemHeight = i.extendedLogisticsGroup.itemClearanceHeight.value.ToString & i.extendedLogisticsGroup.itemClearanceHeight.uom.ToString
                End If
                'Else
                '    .ConsumerItemLength = "Not Found"
                '    .ConsumerItemWidth = "Not Found"
                '    .ConsumerItemDepth = "Not Found"
                '    .ConsumerItemHeight = "Not Found"
            End If
        End With
    End Sub

    Private Sub eCommSpecific(ByVal i As Item)

        With _GXSProductInfo
            If i.extendedRetailGroup IsNot Nothing Then

                'Apparel
                .CollarType = i.extendedRetailGroup.collarType
                .PantInseamLength = i.extendedRetailGroup.pantInseamLength
                If i.extendedRetailGroup.sleeveMeasurement IsNot Nothing Then
                    .SleeveMeasurement = i.extendedRetailGroup.sleeveMeasurement.value & i.extendedRetailGroup.sleeveMeasurement.uom
                End If
                .SleeveType = i.extendedRetailGroup.sleeveLength
                'Footwear
                If i.extendedRetailGroup.heelHeight IsNot Nothing Then
                    .HeelHeight = i.extendedRetailGroup.heelHeight.value & i.extendedRetailGroup.heelHeight.uom
                End If
                If i.extendedRetailGroup.platformHeight IsNot Nothing Then
                    .PlatformHeight = i.extendedRetailGroup.platformHeight.value & i.extendedRetailGroup.platformHeight.uom
                End If
                .SoleType = i.extendedRetailGroup.soleType
                If i.extendedRetailGroup.bootLegCircumference IsNot Nothing Then
                    .BootLegCircumference = i.extendedRetailGroup.bootLegCircumference.value & i.extendedRetailGroup.bootLegCircumference.uom
                End If
                If i.extendedRetailGroup.shaftHeight IsNot Nothing Then
                    .BootShaftHeight = i.extendedRetailGroup.shaftHeight.value & i.extendedRetailGroup.shaftHeight.uom
                End If
                'Jewelry
                .GoldKarat = i.extendedRetailGroup.goldKarat
                .StoneDetails = i.extendedRetailGroup.stone
                If i.extendedRetailGroup.earringDrop IsNot Nothing Then
                    .EarringsDrop = i.extendedRetailGroup.earringDrop.value & i.extendedRetailGroup.earringDrop.uom
                End If
                If i.extendedRetailGroup.watchBandWidth IsNot Nothing Then
                    .WatchBandWidth = i.extendedRetailGroup.watchBandWidth.value & i.extendedRetailGroup.watchBandWidth.uom
                End If
                If i.extendedRetailGroup.watchCaseDimensionAssociation IsNot Nothing AndAlso i.extendedRetailGroup.watchCaseDimensionAssociation.watchCaseHeight IsNot Nothing AndAlso i.extendedRetailGroup.watchCaseDimensionAssociation.watchCaseWidth IsNot Nothing Then
                    .WatchCaseSize = i.extendedRetailGroup.watchCaseDimensionAssociation.watchCaseHeight.value & " X " & i.extendedRetailGroup.watchCaseDimensionAssociation.watchCaseWidth.value & i.extendedRetailGroup.watchCaseDimensionAssociation.watchCaseWidth.uom
                End If
                'Handbags & Accessories
                If i.extendedRetailGroup.handbagShoulderDrop IsNot Nothing Then
                    .HandbagShoulderDrop = i.extendedRetailGroup.handbagShoulderDrop.value & " " & i.extendedRetailGroup.handbagShoulderDrop.uom
                End If
                'Home
                .WarrantyDescription = i.commonRetailGroup.warrantyDesc
                If i.extendedLogisticsGroup IsNot Nothing AndAlso i.extendedLogisticsGroup.consumerItemVolume IsNot Nothing Then
                    .ConsumerProductCapacityOrVolume = i.extendedLogisticsGroup.consumerItemVolume.value & i.extendedLogisticsGroup.consumerItemVolume.uom
                End If
                If i.extendedLogisticsGroup IsNot Nothing AndAlso i.extendedLogisticsGroup.aerosol IsNot Nothing Then
                    .AerosolProduct = i.extendedLogisticsGroup.aerosol
                End If

                If i.gdsnOptionalGroup IsNot Nothing Then
                    'Beauty
                    .KeyActiveIngredient = i.gdsnOptionalGroup.keyActiveIngredient_Name
                    'Home
                    .DoesNotContain = i.gdsnOptionalGroup.doesNotContain
                    'Shared
                    .CareInfo = i.gdsnOptionalGroup.careInformation_Url
                End If

                'Shared
                .LiningMaterial = i.extendedRetailGroup.liningMaterial
                .Closure = i.extendedRetailGroup.closure
                .FauxFur = i.extendedRetailGroup.fauxFur

                If i.extendedRetailGroup.furAssociationList IsNot Nothing Then
                    .FurAnimalName = i.extendedRetailGroup.furAssociationList(0).furAnimalName
                    .FurCountryOfOrigin = i.extendedRetailGroup.furAssociationList(0).furCountryOfOrigin
                    .FurTreatment = i.extendedRetailGroup.furAssociationList(0).furTreatment
                End If

            End If

        End With

    End Sub

    Private Sub SupplyChainCommon(ByVal i As Item)

        If Not IsNothing(i.extendedLogisticsGroup) Then
            If Not IsNothing(i.extendedLogisticsGroup.consumerItemDimensionAssociation.consumerItemDepth) Then
                _GXSProductInfo.ConsumerPackageDepth = i.extendedLogisticsGroup.consumerItemDimensionAssociation.consumerItemDepth.value.ToString & i.extendedLogisticsGroup.consumerItemDimensionAssociation.consumerItemDepth.uom
            End If
        End If

        If Not IsNothing(i.commonLogisticsGroup.consumerPackageDimensionAssociation.consumerPackageHeight) Then
            _GXSProductInfo.ConsumerPackageHeight = i.commonLogisticsGroup.consumerPackageDimensionAssociation.consumerPackageHeight.value.ToString + i.commonLogisticsGroup.consumerPackageDimensionAssociation.consumerPackageHeight.uom
            _GXSProductInfo.ConsumerPackageWidth = i.commonLogisticsGroup.consumerPackageDimensionAssociation.consumerPackageWidth.value.ToString + i.commonLogisticsGroup.consumerPackageDimensionAssociation.consumerPackageWidth.uom
            If Not IsNothing(i.commonLogisticsGroup.consumerWeightAssociation.consumerItemWeight) Then
                _GXSProductInfo.ConsumerPackageGrossWeight = i.commonLogisticsGroup.consumerWeightAssociation.consumerItemWeight.value.ToString + i.commonLogisticsGroup.consumerWeightAssociation.consumerItemWeight.uom
            End If
        End If

    End Sub

    'Private Sub SupplyChainSpecific(ByVal i As Item)
    '    Console.WriteLine(" SUPPLY CHAIN : SPECIFIC ")
    '    If IsNothing(i.commonLogisticsGroup.hazardousMaterialAssociationList) Then
    '        Console.WriteLine("Hazardous Material Class Code: " & i.commonLogisticsGroup.hazardousMaterialAssociationList(0).hazMatClassCode.ToString)
    '        Console.WriteLine("Hazardous Material Class Description: " & i.commonLogisticsGroup.hazardousMaterialAssociationList(0).hazMatMaterialDescription.ToString)
    '    Else
    '        Console.WriteLine("Hazardous Material Class Code: Not Found")
    '        Console.WriteLine("Hazardous Material Class Description: Not Found")
    '    End If
    '    If Not IsNothing(i.gdsnOptionalGroup) Then
    '        Console.WriteLine("Special Handling Code: " & i.gdsnOptionalGroup.transportHandlingCodeList(0).ToString)
    '    Else
    '        Console.WriteLine("Special Handling Code: Not Found.")
    '    End If
    'End Sub

End Class
