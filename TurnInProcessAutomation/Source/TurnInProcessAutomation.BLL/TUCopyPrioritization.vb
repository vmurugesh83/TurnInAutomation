Imports TurnInProcessAutomation.BLL.CommonBO
Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.SqlDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient
Imports System.Text
Imports System.IO

Public Class TUCopyPrioritization
    Private dalDB2 As MainframeDAL.CopyPrioritizationDao = New MainframeDAL.CopyPrioritizationDao

    Public Function GetCopyPrioritizationResultsByCriteria(ByVal categoryCode As Integer,
                                                           ByVal PriceStatusCodes As List(Of String),
                                                       ByVal ImageReady As String, ByVal CopyReady As String,
                                                       ByVal SKUUseFilters As String,
                                                       ByVal ThirdPartyFulfilment As Integer,
                                                       ByVal LocationID As Integer,
                                                       ByVal IncludeOOQuantity As Boolean,
                                                       ByVal IncludeOHQuantity As Boolean) As List(Of CopyPrioritizationInfo)
        Try
            Dim DB2Results As List(Of CopyPrioritizationInfo) = dalDB2.GetCopyPrioritizationResultsByCriteria(categoryCode, String.Join("|", PriceStatusCodes),
                                                                                                              ImageReady, CopyReady,
                                                                                                              SKUUseFilters,
                                                                                                              ThirdPartyFulfilment, LocationID)
            If Not DB2Results Is Nothing AndAlso DB2Results.Count > 0 Then
                CalculateWeightedInventoryAndUpdateImageURL(DB2Results)

                Return ApplyOnHandOnOrderFilters(DB2Results, IncludeOOQuantity, IncludeOHQuantity)
            Else
                Return DB2Results
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetCopyPrioritizationResultsByDept(ByVal DeptId As Integer, ByVal VendorStyleNumber As String,
                                                       ByVal StartShipDate? As Date, ByVal PriceStatusCodes As List(Of String),
                                                       ByVal AdNumber As Integer,
                                                       ByVal ImageReady As String, ByVal CopyReady As String,
                                                       ByVal SKUUseFilters As String,
                                                       ByVal ThirdPartyFulfilment As Integer,
                                                       ByVal LocationID As Integer,
                                                       ByVal IncludeOOQuantity As Boolean,
                                                       ByVal IncludeOHQuantity As Boolean) As List(Of CopyPrioritizationInfo)
        Try
            Dim DB2Results As List(Of CopyPrioritizationInfo) = dalDB2.GetCopyPrioritizationResultsByDept(DeptId, VendorStyleNumber, StartShipDate, String.Join("|", PriceStatusCodes),
                                                             AdNumber, ImageReady, CopyReady, SKUUseFilters, ThirdPartyFulfilment, LocationID)

            If Not DB2Results Is Nothing AndAlso DB2Results.Count > 0 Then
                CalculateWeightedInventoryAndUpdateImageURL(DB2Results)

                Return ApplyOnHandOnOrderFilters(DB2Results, IncludeOOQuantity, IncludeOHQuantity)
            Else
                Return DB2Results
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetCopyPrioritizationResultsByItem(ByVal ISNs As List(Of String), ByVal SKUsUPCs As List(Of String),
                                                       ByVal ImageIDs As List(Of String)) As List(Of CopyPrioritizationInfo)
        Dim internalStyleNumbers As String = String.Empty
        Dim uniqueProductCodes As String = String.Empty
        Dim imageIdentifiers As String = String.Empty

        Try
            If ISNs.Count > 0 Then
                internalStyleNumbers = String.Join(",", ISNs)
                'internalStyleNumbers = String.Concat("'", internalStyleNumbers, "'")
            End If

            If SKUsUPCs.Count > 0 Then
                uniqueProductCodes = String.Join(",", SKUsUPCs)
                'uniqueProductCodes = String.Concat("'", uniqueProductCodes, "'")
            End If

            If ImageIDs.Count > 0 Then
                imageIdentifiers = String.Join(",", ImageIDs)
                'imageIdentifiers = String.Concat("'", imageIdentifiers, "'")
            End If

            Dim DB2Results As List(Of CopyPrioritizationInfo) = dalDB2.GetCopyPrioritizationResultsByItem(internalStyleNumbers, uniqueProductCodes, imageIdentifiers)

            If Not DB2Results Is Nothing AndAlso DB2Results.Count > 0 Then
                CalculateWeightedInventoryAndUpdateImageURL(DB2Results)
            End If

            Return DB2Results

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetCopyPrioritizationResult(ByVal imageId As Integer, ByVal categoryCode As Integer) As CopyPrioritizationInfo
        Try
            Dim DB2Results As CopyPrioritizationInfo = dalDB2.GetCopyPrioritizationResult(imageId, categoryCode)
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetColorLevelResults(ByVal ImageID As Integer) As IList(Of CopyPrioritizationColorInfo)
        Try
            Dim DB2Results As IList(Of CopyPrioritizationColorInfo) = dalDB2.GetColorLevelResults(ImageID)
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetProductCategories(ByVal parentCode As Integer) As List(Of ProductCategoryInfo)
        Try
            Dim DB2Results As List(Of ProductCategoryInfo) = dalDB2.GetProductCategories(parentCode)
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub SaveCopy(ByVal ProductCode As Integer, ByVal Copy As String, ByVal ProductName As String, ByVal UserID As String,
                        ByVal IsSetToReady As Boolean, ByVal WebCatComments As String, ByVal ProductDesc As String)
        Try

            dalDB2.SaveCopy(ProductCode, Copy, ProductName, UserID, IsSetToReady, WebCatComments, ProductDesc)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Function ApplyOnHandOnOrderFilters(ByVal copyPrioritizationResults As List(Of CopyPrioritizationInfo), ByVal IncludeOOQuantity As Boolean, ByVal IncludeOHQuantity As Boolean) As List(Of CopyPrioritizationInfo)
        Dim copyPriResults = (From copyPrioritizationResult In copyPrioritizationResults
                            Where If(IncludeOOQuantity, copyPrioritizationResult.OnOrder > 0, True) And
                            If(IncludeOHQuantity, copyPrioritizationResult.OnHand > 0, True)
                            Select copyPrioritizationResult).ToList()

        Return copyPriResults.Cast(Of CopyPrioritizationInfo).ToList()
    End Function

    Private Sub CalculateWeightedInventoryAndUpdateImageURL(ByRef copyPrioritizationResults As List(Of CopyPrioritizationInfo))
        Dim tuConfigBAO As TUConfig = Nothing
        Dim onHandMultiplier As IList(Of TTUConfig) = Nothing
        Dim onOrderMultiplier As IList(Of TTUConfig) = Nothing
        Dim poShipDateMultiplier As IList(Of TTUConfig) = Nothing
        Dim finalImageReadyMultiplier As IList(Of TTUConfig) = Nothing
        Dim priceStatusMultiplier As IList(Of TTUConfig) = Nothing
        Dim skuUseMultiplier As IList(Of TTUConfig) = Nothing
        Dim productDateMultiplier As IList(Of TTUConfig) = Nothing
        Dim ecommSetupCreateDAO As EcommSetupCreateDao = Nothing
        Dim sampleRequestList As IList(Of SampleRequestInfo) = Nothing
        Dim directShipMultiplier As IList(Of TTUConfig) = Nothing
        Dim ownedPriceMultiplier As IList(Of TTUConfig) = Nothing
        Dim weightedInventory As Integer = 0
        Dim ttuConfig As TTUConfig = Nothing
        Dim poShipDateInterval As Long = 0
        Dim productDateInterval As Long = 0
        Dim ownedPriceWithWeight As Decimal = 0D

        Try
            tuConfigBAO = New TUConfig()
            onHandMultiplier = tuConfigBAO.GetConfigurationByKey("OH_MULTIPLIER")
            onOrderMultiplier = tuConfigBAO.GetConfigurationByKey("OO_MULTIPLIER")
            poShipDateMultiplier = tuConfigBAO.GetConfigurationByKey("PO_SHIP_DATE_MULTIPLIER")
            finalImageReadyMultiplier = tuConfigBAO.GetConfigurationByKey("FINAL_IMAGE_READY_MULTIPLIER")
            priceStatusMultiplier = tuConfigBAO.GetConfigurationByKey("PRICE_STATUS_MULTIPLIER")
            skuUseMultiplier = tuConfigBAO.GetConfigurationByKey("SKU_USE_MULTIPLIER")
            productDateMultiplier = tuConfigBAO.GetConfigurationByKey("PRODUCT_DATE_MULTIPLIER")
            ownedPriceMultiplier = tuConfigBAO.GetConfigurationByKey("OWNED_COST_MULTIPLIER")

            'Get the direct ship configuration if there is a direct ship in the result set
            If copyPrioritizationResults.Exists(Function(x) x.ThirdPartyFulfilmentCode = 2) Then
                directShipMultiplier = tuConfigBAO.GetConfigurationByKey("DIRECT_SHIP_MULTIPLIER")
            End If

            For Each copyPrioritization As CopyPrioritizationInfo In copyPrioritizationResults
                With copyPrioritization
                    'on hand multiplier
                    If Not onHandMultiplier Is Nothing AndAlso onHandMultiplier.Count > 0 Then
                        weightedInventory = .OnHand * onHandMultiplier(0).NumericConfigValue
                        .OHMultiplier = onHandMultiplier(0).NumericConfigValue
                    End If

                    'on order multiplier
                    If Not onOrderMultiplier Is Nothing AndAlso onOrderMultiplier.Count > 0 Then
                        weightedInventory = weightedInventory + (.OnOrder * onOrderMultiplier(0).NumericConfigValue)
                        .OOMultiplier = onOrderMultiplier(0).NumericConfigValue
                    End If

                    If weightedInventory <= 0 Then
                        weightedInventory = 1
                    End If

                    'po ship date multiplier
                    If Not poShipDateMultiplier Is Nothing AndAlso poShipDateMultiplier.Count > 0 Then
                        poShipDateInterval = DateDiff(DateInterval.Day, Date.Now, CDate(.POStartShipDate))
                        ' when the po ship date interval is negative, then check for a PO. 
                        'If a PO exists, then take the maximum ship date weight, otherwise take the minimum ship date weight.
                        If poShipDateInterval < 0 Then
                            poShipDateMultiplier = poShipDateMultiplier.ToList().FindAll(Function(a) CLng(a.SecondLevelConfigKey.Trim()) < 0)

                            If .OnHand <= 0 AndAlso .OnOrder <= 0 Then
                                poShipDateMultiplier = poShipDateMultiplier.OrderBy(Function(a) CLng(a.SecondLevelConfigKey.Trim())).ToList()
                            Else
                                poShipDateMultiplier = poShipDateMultiplier.OrderByDescending(Function(a) CLng(a.SecondLevelConfigKey.Trim())).ToList()
                            End If
                        Else
                            poShipDateMultiplier = poShipDateMultiplier.OrderBy(Function(a) CLng(a.SecondLevelConfigKey.Trim())).ToList()
                        End If

                        ttuConfig = poShipDateMultiplier.ToList().Find(Function(a) CLng(a.SecondLevelConfigKey.Trim()) >= poShipDateInterval)
                        If Not ttuConfig Is Nothing Then
                            weightedInventory = weightedInventory * ttuConfig.NumericConfigValue
                            .ShipDateMultiplier = ttuConfig.NumericConfigValue
                        End If
                    End If

                    'final image ready multiplier
                    If Not finalImageReadyMultiplier Is Nothing AndAlso finalImageReadyMultiplier.Count > 0 Then
                        weightedInventory = weightedInventory * finalImageReadyMultiplier(0).NumericConfigValue
                        .FinalImageMultiplier = finalImageReadyMultiplier(0).NumericConfigValue
                    End If

                    'price status multiplier
                    If Not priceStatusMultiplier Is Nothing AndAlso priceStatusMultiplier.Count > 0 Then
                        ttuConfig = priceStatusMultiplier.ToList().Find(Function(a) a.SecondLevelConfigKey.Trim() = .PriceStatusCode)
                        If Not ttuConfig Is Nothing Then
                            weightedInventory = weightedInventory * ttuConfig.NumericConfigValue
                            .PriceStatusMultiplier = ttuConfig.NumericConfigValue
                        End If
                    End If

                    'sku use multiplier
                    If Not skuUseMultiplier Is Nothing AndAlso skuUseMultiplier.Count > 0 Then
                        ttuConfig = skuUseMultiplier.ToList().Find(Function(a) a.SecondLevelConfigKey.Trim() = .SKUUseCode)
                        If Not ttuConfig Is Nothing Then
                            weightedInventory = weightedInventory * ttuConfig.NumericConfigValue
                            .SKUUseMultiplier = ttuConfig.NumericConfigValue
                        End If
                    End If

                    'product date multiplier
                    If Not productDateMultiplier Is Nothing AndAlso productDateMultiplier.Count > 0 AndAlso .ProductReadyDate <> Date.MinValue Then
                        productDateInterval = DateDiff(DateInterval.Day, Date.Now, .ProductReadyDate)
                        ttuConfig = productDateMultiplier.ToList().Find(Function(a) a.SecondLevelConfigKey.Trim() = productDateInterval.ToString())
                        If Not ttuConfig Is Nothing Then
                            weightedInventory = weightedInventory * ttuConfig.NumericConfigValue
                            .ProductDateMultiplier = ttuConfig.NumericConfigValue
                        End If
                    End If

                    'direct ship multiplier
                    If .ThirdPartyFulfilmentCode = 2 AndAlso Not directShipMultiplier Is Nothing AndAlso directShipMultiplier.Count > 0 Then
                        weightedInventory = weightedInventory * directShipMultiplier(0).NumericConfigValue
                        .DirectShipMultiplier = directShipMultiplier(0).NumericConfigValue
                    End If

                    'total owned price for OH
                    .OwnedPriceOH = If(.OnHand > 0, (.OnHand * .OwnedPrice), (1 * .OwnedPrice))

                    'total owned price for OO
                    .OwnedPriceOO = If(.OnOrder > 0, (.OnOrder * .OwnedPrice), (1 * .OwnedPrice))

                    'owned price multiplier
                    If Not ownedPriceMultiplier Is Nothing AndAlso ownedPriceMultiplier.Count > 0 Then
                        'Formula (OnHand*OHWeight*OwnedPrice*OwnedPriceWeight) + (OnOrder*OOWeight*OwnedPrice*OwnedPriceWeight)
                        ownedPriceWithWeight = If(ownedPriceMultiplier(0).NumericConfigValue > 0.0, ownedPriceMultiplier(0).NumericConfigValue, 1) * .OwnedPrice
                        weightedInventory = weightedInventory + ((If(.OnHand > 0, .OnHand, 1) * If(.OHMultiplier > 0, .OHMultiplier, 1) * ownedPriceWithWeight) +
                                             (If(.OnOrder > 0, .OnOrder, 1) * If(.OOMultiplier > 0, .OOMultiplier, 1) * ownedPriceWithWeight))
                        .OwnedPriceMultiplier = ownedPriceMultiplier(0).NumericConfigValue
                    End If

                    .WeightedInventory = weightedInventory

                    If Not String.IsNullOrEmpty(.PrimaryThumbnailURL) AndAlso .PrimaryThumbnailURL.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) Then
                        .IsFinalImageReady = True
                    Else
                        .IsFinalImageReady = False
                    End If

                    If Not .IsFinalImageReady Then
                        ecommSetupCreateDAO = New EcommSetupCreateDao()
                        sampleRequestList = ecommSetupCreateDAO.GetSampleRequests(0, .ISN, .VendorStyleNumber)

                        If Not sampleRequestList Is Nothing AndAlso sampleRequestList.Count > 0 Then
                            sampleRequestList = sampleRequestList.Where(
                            Function(x) x.InternalStyleNum = .ISN And x.ColorCode = .ColorCode _
                                And x.SampleApprovalFlag = "Y"c _
                                And x.SampleApprovalType.ToUpper().Equals("APPROVED")).ToList()

                            If Not sampleRequestList Is Nothing AndAlso sampleRequestList.Count > 0 Then
                                .PrimaryThumbnailURL = sampleRequestList(0).PrimaryThumbnailUrl
                            End If
                        End If

                    End If

                End With
            Next

            'Order by weighted inventory
            copyPrioritizationResults = copyPrioritizationResults.OrderByDescending(Function(a) a.WeightedInventory).ToList()
        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class
