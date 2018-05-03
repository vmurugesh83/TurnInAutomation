Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class EcommSetupCreateFactory

    Public Shared Function Construct(ByVal reader As DB2DataReader) As EcommSetupCreateInfo
        Dim EcommSetupCreateInfo As New EcommSetupCreateInfo()

        With EcommSetupCreateInfo
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
            '.GenericClass = CInt(ReadColumn(reader, "PROD_DTL_ID1"))
            '.GenericSubClass = CInt(ReadColumn(reader, "PROD_DTL_ID1"))
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
            .Fabrication = CStr(ReadColumn(reader, "FAB_ID_DESC"))
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
            .Sequence = CInt(ReadColumn(reader, "MDSE_BTCH_ORDER_ID"))
            .AlreadyExistsInBatch = CBool(ReadColumn(reader, "EXISTS_IN_BATCH"))
            .AlreadyProcessed = CBool(ReadColumn(reader, "IN_PROCESS_FLAG"))
            .IsTurnedInEcomm = CChar(IIf(CBool(ReadColumn(reader, "IN_PROCESS_FLAG")) = True, "Y", "N"))
            .IsTurnedInPrint = CChar("N")
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
            If Not ReadColumn(reader, "SMPL_REQ_CRTE_NME") Is Nothing AndAlso Not IsDBNull(reader.Item("SMPL_REQ_CRTE_NME")) Then
                .SampleDetails.SampleRequestCreateName = CStr(ReadColumn(reader, "SMPL_REQ_CRTE_NME"))
            Else
                .SampleDetails.SampleRequestCreateName = String.Empty
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
            .StartShipDate = CStr(ReadColumn(reader, "START_SHIP_DATE"))
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
            If Not ReadColumn(reader, "CLASS_ID") Is Nothing AndAlso Not IsDBNull(reader.Item("CLASS_ID")) Then
                .ClassID = CInt(ReadColumn(reader, "CLASS_ID"))
            Else
                .ClassID = 0
            End If
        End With

        Return EcommSetupCreateInfo
    End Function

    Public Shared Function ConstructColorSize(ByVal reader As DB2DataReader) As EcommSetupClrSzInfo
        Dim EcommSetupClrSzInfo As New EcommSetupClrSzInfo()

        With EcommSetupClrSzInfo
            .AdNumber = CInt(ReadColumn(reader, "AD_NUM"))
            .PageNumber = CInt(ReadColumn(reader, "AD_SYSTEM_PAGE_NUM"))
            .BatchNumber = CInt(ReadColumn(reader, "MDSE_TRNIN_BTCH_ID"))
            .TurnInMerchID = CInt(ReadColumn(reader, "TURN_IN_MDSE_ID"))
            .AdminMerchNum = CInt(ReadColumn(reader, "ADMIN_MDSE_NUM"))
            .RemoveMerchFlag = CChar(ReadColumn(reader, "REMOVE_MDSE_FLG"))
            .Status = If(String.IsNullOrEmpty(CStr(ReadColumn(reader, "STATUS"))), "NOTSAVED", CStr(ReadColumn(reader, "STATUS")))
            .DeptID = CInt(ReadColumn(reader, "DEPT_ID"))
            If Not ReadColumn(reader, "DEPT_ID") Is Nothing AndAlso Not ReadColumn(reader, "DEPT_LONG_DESC") Is Nothing Then
                .DeptIdDesc = CStr(ReadColumn(reader, "DEPT_ID")) + " " + CStr(ReadColumn(reader, "DEPT_LONG_DESC")).PadRight(200)
            End If
            .ISN = CDec(ReadColumn(reader, "ISN"))
            .VendorStyleNum = CStr(ReadColumn(reader, "VENDOR_STYLE_NUM"))
            .VendorName = CStr(ReadColumn(reader, "VENDOR_NME"))
            .IsnDesc = CStr(ReadColumn(reader, "ISN_LONG_DESC"))
            .IsReserve = CChar(ReadColumn(reader, "RESERVE"))
            .IsHotItem = CChar(ReadColumn(reader, "HOT_ITEM"))
            .FriendlyProdDesc = CStr(ReadColumn(reader, "FRNDLY_PRD_DESC"))
            .FriendlyProdFeatures = CStr(ReadColumn(reader, "FRNDLY_PRD_FEATURES"))
            .Color = CStr(ReadColumn(reader, "COLOR"))
            .VendorColorCode = CInt(ReadColumn(reader, "CLR_CDE"))
            .FriendlyColor = CStr(ReadColumn(reader, "FRIENDLY_CLR"))
            .ColorFamily = CStr(ReadColumn(reader, "COLOR_FAMILY"))
            .SampleSize = CStr(ReadColumn(reader, "SIZE_ID"))
            .ColorCorrect = CChar(ReadColumn(reader, "CLR_CORRECT"))
            .ImageKind = CStr(ReadColumn(reader, "IMAGE_KIND"))
            .PuImageID = CInt(ReadColumn(reader, "PU_IMGID"))
            .RouteFromAD = CInt(ReadColumn(reader, "ROUTE_FROM_AD"))
            .GroupNum = CShort(ReadColumn(reader, "GRP_NUM"))
            .FeatureRenderSwatch = CStr(ReadColumn(reader, "F_R_S"))
            .ImageType = CStr(ReadColumn(reader, "IMAGE_TYPE"))
            .AltView = CStr(ReadColumn(reader, "ALT_VIEW"))
            .SampleStoreNum = CShort(ReadColumn(reader, "SMPL_STR_ID"))
            .SampleStore = CStr(ReadColumn(reader, "SMPL_STR_ID")) + " " + CStr(ReadColumn(reader, "SMPL_STR"))
            .MerchantNotes = CStr(ReadColumn(reader, "MERCHANT_NOTES"))
            .UPC = CDec(ReadColumn(reader, "UPC_NUM"))
            .Sequence = CInt(ReadColumn(reader, "MDSE_BTCH_ORDER_ID"))
            .LabelName = CStr(ReadColumn(reader, "LABEL_NAME"))

            If Not ReadColumn(reader, "SAMPLE_MERCH_ID") Is Nothing Then
                .SampleMerchId = CInt(reader.Item("SAMPLE_MERCH_ID"))
            End If

            If Not ReadColumn(reader, "ON_HAND") Is Nothing Then
                .OnHand = CInt(reader.Item("ON_HAND"))
            End If

            If Not ReadColumn(reader, "ON_ORDER") Is Nothing Then
                .OnOrder = CInt(reader.Item("ON_ORDER"))
            End If

            If Not ReadColumn(reader, "FAB_DTL_DESC") Is Nothing Then
                .FabricationDesc = CStr(reader.Item("FAB_DTL_DESC"))
            End If

            If Not ReadColumn(reader, "SIZE_1_DESC") Is Nothing Then
                .Size1Desc = CStr(reader.Item("SIZE_1_DESC"))
            End If

            If Not ReadColumn(reader, "SIZE_2_DESC") Is Nothing Then
                .Size2Desc = CStr(reader.Item("SIZE_2_DESC"))
            End If

        End With

        Return EcommSetupClrSzInfo
    End Function

    Public Shared Function ConstructSampleRequests(ByVal reader As DB2DataReader) As SampleRequestInfo
        Dim SampleRequestInfo As New SampleRequestInfo

        If Not reader.Item("SAMPLE_MERCH_ID") Is Nothing Then
            SampleRequestInfo.SampleMerchId = CInt(reader.Item("SAMPLE_MERCH_ID"))
        End If

        If Not IsDBNull(reader.Item("VENDOR_STYLE_NUM")) Then
            SampleRequestInfo.VendorStyleNumber = CStr(reader.Item("VENDOR_STYLE_NUM"))
        End If

        If Not IsDBNull(reader.Item("INTERNAL_STYLE_NUM")) Then
            SampleRequestInfo.InternalStyleNum = CDec(reader.Item("INTERNAL_STYLE_NUM"))
        End If

        If Not IsDBNull(reader.Item("ISN_LONG_DESC")) Then
            SampleRequestInfo.IsnLongDesc = CStr(reader.Item("ISN_LONG_DESC")).Trim()
        End If

        If Not IsDBNull(reader.Item("CLR_CDE")) Then
            SampleRequestInfo.ColorCode = CInt(reader.Item("CLR_CDE"))
        End If

        If Not IsDBNull(reader.Item("CLR_LONG_DESC")) Then
            SampleRequestInfo.ColorLongDesc = CStr(reader.Item("CLR_LONG_DESC")).Trim()
        End If

        If Not IsDBNull(reader.Item("AD_NUM")) Then
            SampleRequestInfo.AdNumber = CInt(reader.Item("AD_NUM"))
        End If

        If Not IsDBNull(reader.Item("AD_SYSTEM_PAGE_NUM")) Then
            SampleRequestInfo.AdSystemPageNum = CInt(reader.Item("AD_SYSTEM_PAGE_NUM"))
        End If

        If Not IsDBNull(reader.Item("SIZE_ID")) Then
            SampleRequestInfo.SampleSize = CStr(reader.Item("SIZE_ID"))
        End If

        If Not IsDBNull(reader.Item("UPC_NUM")) Then
            SampleRequestInfo.UpcNumber = CDec(reader.Item("UPC_NUM"))
        End If

        If Not IsDBNull(reader.Item("SAMPLE_DUE_DTE")) Then
            SampleRequestInfo.SampleDueDate = CDate(reader.Item("SAMPLE_DUE_DTE"))
        End If

        If Not IsDBNull(reader.Item("SAMPLE_APVL_FLG")) Then
            SampleRequestInfo.SampleApprovalFlag = CChar(reader.Item("SAMPLE_APVL_FLG"))
        End If

        If Not IsDBNull(reader.Item("SAMPLE_REQUEST_TYP")) Then
            SampleRequestInfo.SampleRequestType = CStr(reader.Item("SAMPLE_REQUEST_TYP")).Trim()
        End If

        If Not IsDBNull(reader.Item("SAMPLE_SIZE_DESC")) Then
            SampleRequestInfo.SampleSizeDesc = CStr(reader.Item("SAMPLE_SIZE_DESC")).Trim()
        End If

        If Not IsDBNull(reader.Item("SAMPLE_APPROVAL_TS")) Then
            SampleRequestInfo.SampleApprovalTimestamp = CStr(reader.Item("SAMPLE_APPROVAL_TS")).Trim()
        End If

        If Not IsDBNull(reader.Item("CMR_CHECK_IN_DTE")) Then
            SampleRequestInfo.CMRCheckInDate = CStr(reader.Item("CMR_CHECK_IN_DTE")).Trim()
        End If

        If Not IsDBNull(reader.Item("SAMPLE_STATUS_DESC")) Then
            SampleRequestInfo.SampleStatusDesc = CStr(reader.Item("SAMPLE_STATUS_DESC")).Trim()
        End If

        If Not IsDBNull(reader.Item("SAMPLE_APVL_TYP")) Then
            SampleRequestInfo.SampleApprovalType = CStr(reader.Item("SAMPLE_APVL_TYP")).Trim()
        End If

        If Not IsDBNull(reader.Item("SMPL_PRIM_LOC_NME")) Then
            SampleRequestInfo.PrimaryLocationName = CStr(reader.Item("SMPL_PRIM_LOC_NME")).Trim()
        End If

        If Not IsDBNull(reader.Item("PRIM_ACTL_URL_TXT")) Then
            SampleRequestInfo.PrimaryActualUrl = CStr(reader.Item("PRIM_ACTL_URL_TXT")).Trim()
        End If

        If Not IsDBNull(reader.Item("PRIM_MED_URL_TXT")) Then
            SampleRequestInfo.PrimaryMediumUrl = CStr(reader.Item("PRIM_MED_URL_TXT")).Trim()
        End If

        If Not IsDBNull(reader.Item("PRIM_THB_URL_TXT")) Then
            SampleRequestInfo.PrimaryThumbnailUrl = CStr(reader.Item("PRIM_THB_URL_TXT")).Trim()
        End If

        If Not IsDBNull(reader.Item("SEC_ACTL_URL_TXT")) Then
            SampleRequestInfo.SecondaryActualUrl = CStr(reader.Item("SEC_ACTL_URL_TXT")).Trim()
        End If

        If Not IsDBNull(reader.Item("SEC_MED_URL_TXT")) Then
            SampleRequestInfo.SecondaryMediumUrl = CStr(reader.Item("SEC_MED_URL_TXT")).Trim()
        End If

        If Not IsDBNull(reader.Item("SEC_THB_URL_TXT")) Then
            SampleRequestInfo.SecondaryThumbnailUrl = CStr(reader.Item("SEC_THB_URL_TXT")).Trim()
        End If

        If Not IsDBNull(reader.Item("SMPL_REQ_CRTE_NME")) Then
            SampleRequestInfo.SampleRequestCreateName = CStr(reader.Item("SMPL_REQ_CRTE_NME")).Trim()
        End If

        If Not IsDBNull(reader.Item("SMPL_REQ_CRTE_TS")) Then
            SampleRequestInfo.SampleRequestCreateTimestamp = CDate(reader.Item("SMPL_REQ_CRTE_TS"))
        End If

        If Not IsDBNull(reader.Item("LAST_MOD_ID")) Then
            SampleRequestInfo.LastModifiedId = CStr(reader.Item("LAST_MOD_ID")).Trim()
        End If

        If Not IsDBNull(reader.Item("LAST_MOD_TS")) Then
            SampleRequestInfo.LastModifiedTimestamp = CDate(reader.Item("LAST_MOD_TS"))
        End If

        If Not IsDBNull(reader.Item("ALT_ATTRIB_DESC")) Then
            SampleRequestInfo.SampleAltAttrDesc = CStr(reader.Item("ALT_ATTRIB_DESC"))
        End If
        Return SampleRequestInfo
    End Function

    Public Shared Function ConstructPartialSampleRequests(ByVal reader As DB2DataReader) As SampleRequestInfo
        Dim SampleRequestInfo As New SampleRequestInfo

        If Not reader.Item("SAMPLE_MERCH_ID") Is Nothing Then
            SampleRequestInfo.SampleMerchId = CInt(reader.Item("SAMPLE_MERCH_ID"))
        End If

        If Not IsDBNull(reader.Item("INTERNAL_STYLE_NUM")) Then
            SampleRequestInfo.InternalStyleNum = CDec(reader.Item("INTERNAL_STYLE_NUM"))
        End If

        If Not IsDBNull(reader.Item("CLR_CDE")) Then
            SampleRequestInfo.ColorCode = CInt(reader.Item("CLR_CDE"))
        End If

        If Not IsDBNull(reader.Item("CLR_LONG_DESC")) Then
            SampleRequestInfo.ColorLongDesc = CStr(reader.Item("CLR_LONG_DESC")).Trim()
        End If

        If Not IsDBNull(reader.Item("SIZE_ID")) Then
            SampleRequestInfo.SampleSize = CStr(reader.Item("SIZE_ID"))
        End If

        If Not IsDBNull(reader.Item("SAMPLE_SIZE_DESC")) Then
            SampleRequestInfo.SampleSizeDesc = CStr(reader.Item("SAMPLE_SIZE_DESC")).Trim()
        End If

        If Not IsDBNull(reader.Item("SAMPLE_APVL_FLG")) Then
            SampleRequestInfo.SampleApprovalFlag = CChar(reader.Item("SAMPLE_APVL_FLG"))
        End If

        Return SampleRequestInfo
    End Function

End Class

