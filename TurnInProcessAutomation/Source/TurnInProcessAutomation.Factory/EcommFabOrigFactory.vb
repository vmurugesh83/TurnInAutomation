Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class EcommFabOrigFactory

    Public Shared Function Construct(ByVal reader As DB2DataReader) As EcommFabOrigInfo
        Dim EcommFabOrigInfo As New EcommFabOrigInfo()

        With EcommFabOrigInfo
            .BatchNumber = CInt(ReadColumn(reader, "MDSE_TRNIN_BTCH_ID"))
            .AdNumber = CInt(ReadColumn(reader, "AD_NUM"))
            .PageNumber = CInt(ReadColumn(reader, "AD_SYSTEM_PAGE_NUM"))
            .ACode = CStr(ReadColumn(reader, "A_CD_1"))
            .VendorId = CInt(ReadColumn(reader, "VENDOR_ID"))
            .VendorName = CStr(ReadColumn(reader, "VENDOR_NME"))
            .IsReserve = CBool(ReadColumn(reader, "IS_RESERVE"))
            .ISN = CDec(ReadColumn(reader, "INTERNAL_STYLE_NUM"))
            .ISNDesc = CStr(ReadColumn(reader, "ISN_LONG_DESC"))
            .VendorStyleNumber = CStr(ReadColumn(reader, "VENDOR_STYLE_NUM"))
            .SellYear = CInt(ReadColumn(reader, "SELL_YEAR"))
            .SellSeason = CStr(ReadColumn(reader, "SEASON_DESC"))
            .OnOrder = CInt(ReadColumn(reader, "ON_ORDER"))
            .OnHand = CInt(ReadColumn(reader, "ON_HAND"))
            .ColorCode = CStr(ReadColumn(reader, "CLR_CDE"))
            .ColorDesc = CStr(ReadColumn(reader, "CLR_DESC"))

            .DeptId = CInt(ReadColumn(reader, "DEPT_ID"))
            .BrandId = CInt(ReadColumn(reader, "BRAND_ID"))
            .LabelId = CInt(ReadColumn(reader, "LABEL_ID"))

            .ProductDetailId1 = CInt(ReadColumn(reader, "PROD_DTL_ID1"))
            .ProductDetailId2 = CInt(ReadColumn(reader, "PROD_DTL_ID2"))
            .ProductDetailId3 = CInt(ReadColumn(reader, "PROD_DTL_ID3"))
            .DeptDesc = CStr(ReadColumn(reader, "DEPT_LONG_DESC"))
            .BuyerId = CInt(ReadColumn(reader, "BUYER_ID"))
            .BuyerName = CStr(ReadColumn(reader, "BUYER_NME"))
            .BuyerExt = CStr(ReadColumn(reader, "EXT"))
            .BrandDesc = CStr(ReadColumn(reader, "BRAND_LONG_DESC"))
            .LabelDesc = CStr(ReadColumn(reader, "LABEL_LONG_DESC"))
            .ProductDetailDesc1 = CStr(ReadColumn(reader, "PROD_DTL_LONG_DESC1"))
            .ProductDetailDesc2 = CStr(ReadColumn(reader, "PROD_DTL_LONG_DESC2"))
            .ProductDetailDesc3 = CStr(ReadColumn(reader, "PROD_DTL_LONG_DESC3"))
            .PatternDesc = CStr(ReadColumn(reader, "LONG_PATTERN_DESC"))
            .ExistingWebStyle = CStr(ReadColumn(reader, "EXISTING_WEB_STYLE"))
            .FeatureImageNum = CStr(ReadColumn(reader, "FEATURE_IMAGE_NUM"))

            .SellingLocation = CStr(ReadColumn(reader, "SELLING_LOC"))
            .FabricationId = CInt(ReadColumn(reader, "FAB_ID"))
            .Fabrication = CStr(ReadColumn(reader, "TURNIN_FABRIC_DESC"))
            .FabricationSource = CStr(ReadColumn(reader, "FAB_DESC_SRCE_TYP"))
            .StyleSkuFabDescription = Trim(CStr(ReadColumn(reader, "FAB_LONG_DESC")))
            .OriginationCode = CInt(ReadColumn(reader, "PRDCT_ORIGINATE_CDE"))
            .Origination = CStr(ReadColumn(reader, "PRDCT_ORIG_DESC"))
            .OriginationSource = CStr(ReadColumn(reader, "PRDCT_ORIGSRCE_TYP"))
            .StyleSkuOrigDescription = Trim(CStr(ReadColumn(reader, "ORIG_DESC")))
            .ImportedOrUSA = CStr(ReadColumn(reader, "IMP_USA"))
            .DropShipFlg = CChar(ReadColumn(reader, "DROP_SHIP_FLG"))

            .SizeAvailable = CStr(ReadColumn(reader, "SIZE_AVAILABLE"))
            .SizeCategoryCode = CStr(ReadColumn(reader, "SIZE_CATGY_CDE"))
            .ModelCategoryCode = CStr(ReadColumn(reader, "MODEL_CATGY_CDE"))
            .VndApprovalFlg = CChar(ReadColumn(reader, "VENDOR_APPRVL_FLG"))
            .AddnColorSamplesFlg = CChar(ReadColumn(reader, "ADDL_SAMPLES_FLG"))
            .TurnInUsageInd = CShort(ReadColumn(reader, "TURN_IN_USAGE_CDE"))

            .TurnInMerchId = CInt(ReadColumn(reader, "TURN_IN_MDSE_ID"))
            .LastModBy = CStr(ReadColumn(reader, "LAST_MOD_ID"))
            If Not ReadColumn(reader, "LAST_MOD_TS") Is Nothing AndAlso Not IsDBNull(reader.Item("LAST_MOD_TS")) AndAlso Not String.IsNullOrEmpty(CStr(reader.Item("LAST_MOD_TS"))) Then
                .LastModTs = CDate(ReadColumn(reader, "LAST_MOD_TS"))
            Else
                .LastModTs = Nothing
            End If
            .Sequence = CInt(ReadColumn(reader, "MDSE_BTCH_ORDER_ID"))
            .AlreadyExistsInBatch = CBool(ReadColumn(reader, "EXISTS_IN_BATCH"))
            .AlreadyProcessed = CBool(ReadColumn(reader, "IN_PROCESS_FLAG"))
            .IsTurnedInEcomm = CChar(IIf(CBool(ReadColumn(reader, "IN_PROCESS_FLAG")) = True, "Y", "N"))
            .IsTurnedInPrint = CChar("N")
            .UPC = CLng(ReadColumn(reader, "UPC_NUM"))
            .GenericClassId = CInt(ReadColumn(reader, "GEN_CLA_ID"))
            .GenericClass = CStr(ReadColumn(reader, "CLASS"))
            If Not IsNothing(ReadColumn(reader, "GEN_SCLA_ID")) Then
                .GenericSubClassId = CInt(ReadColumn(reader, "GEN_SCLA_ID"))
            Else
                .GenericSubClassId = 0
            End If
            .GenericSubClass = CStr(ReadColumn(reader, "SUBCLASS"))

            If Not ReadColumn(reader, "SUBCLASS_ID") Is Nothing AndAlso Not IsDBNull(reader.Item("SUBCLASS_ID")) Then
                .SubClassID = CInt(ReadColumn(reader, "SUBCLASS_ID"))
            Else
                .SubClassID = 0
            End If

            If Not ReadColumn(reader, "SAMPLE_APVL_FLG") Is Nothing AndAlso Not IsDBNull(reader.Item("SAMPLE_APVL_FLG")) Then
                .SampleDetails.SampleApprovalFlag = CChar(ReadColumn(reader, "SAMPLE_APVL_FLG"))
            Else
                .SampleDetails.SampleApprovalFlag = "N"c
            End If
            If Not ReadColumn(reader, "SAMPLE_STATUS_DESC") Is Nothing AndAlso Not IsDBNull(reader.Item("SAMPLE_STATUS_DESC")) Then
                .SampleDetails.SampleStatusDesc = CStr(ReadColumn(reader, "SAMPLE_STATUS_DESC"))
            Else
                .SampleDetails.SampleStatusDesc = String.Empty
            End If
            If Not ReadColumn(reader, "SKU_ACTIVE_UPC_FLG") Is Nothing AndAlso Not IsDBNull(reader.Item("SKU_ACTIVE_UPC_FLG")) Then
                .ActiveUPCFlag = CStr(ReadColumn(reader, "SKU_ACTIVE_UPC_FLG"))
            Else
                .ActiveUPCFlag = "N"
            End If
            If Not ReadColumn(reader, "TEC_ACTIVE_FLG") Is Nothing AndAlso Not IsDBNull(reader.Item("TEC_ACTIVE_FLG")) Then
                .ActiveFlag = CStr(ReadColumn(reader, "TEC_ACTIVE_FLG"))
            Else
                .ActiveFlag = "N"
            End If
            If Not ReadColumn(reader, "START_SHIP_DATE") Is Nothing AndAlso Not IsDBNull(reader.Item("START_SHIP_DATE")) Then
                .StartShipDate = CStr(ReadColumn(reader, "START_SHIP_DATE"))
            Else
                .StartShipDate = DateTime.Now.ToShortDateString()
            End If

            If Not ReadColumn(reader, "WEBCAT_QTY") Is Nothing AndAlso Not IsDBNull(reader.Item("WEBCAT_QTY")) Then
                .WebCatAvailableQty = CStr(ReadColumn(reader, "WEBCAT_QTY"))
            Else
                .WebCatAvailableQty = "0"
            End If
            If Not ReadColumn(reader, "ON_ORDER_BYSHIPDATE") Is Nothing AndAlso Not IsDBNull(reader.Item("ON_ORDER_BYSHIPDATE")) Then
                .OnOrderByShipDate = CStr(ReadColumn(reader, "ON_ORDER_BYSHIPDATE"))
            Else
                .OnOrderByShipDate = "0"
            End If
        End With

        Return EcommFabOrigInfo
    End Function

End Class

