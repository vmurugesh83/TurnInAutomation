Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class EcommPrioritizationFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As ECommPrioritizationInfo
        Dim ECommPriorityInfo As New ECommPrioritizationInfo()

        With ECommPriorityInfo
            .TurnInMerchID = CInt(ReadColumn(reader, "TURN_IN_MDSE_ID"))
            .DeptID = CInt(ReadColumn(reader, "DEPT_ID"))
            .DeptIdDesc = CStr(ReadColumn(reader, "DEPT_ID")) + " " + CStr(ReadColumn(reader, "DEPT_LONG_DESC"))
            .OnOrder = CInt(ReadColumn(reader, "ON_ORDER"))
            .OnHand = CInt(ReadColumn(reader, "ON_HAND"))
            .DeliverDate = CStr(ReadColumn(reader, "IN_STORE_DATE"))
            .ImageShot = CChar(ReadColumn(reader, "IMG_SHOT"))
            .VtPath = CStr(ReadColumn(reader, "VT_PATH"))
            .WebCatStatus = CStr(ReadColumn(reader, "STATUS"))
            .StatusFlg = CChar(ReadColumn(reader, "WCAT_LOAD_STAT_FLG"))
            .SwatchFlg = CChar(ReadColumn(reader, "IS_SWATCH_FLG"))
            .ColorFlg = CChar(ReadColumn(reader, "PRODUCT_COLOR_FLG"))
            .SizeFlg = CChar(ReadColumn(reader, "PRODUCT_SIZE_FLG"))
            .WebCatgyCde = CInt(ReadColumn(reader, "CATEGORY_CDE"))
            .WebCatgyDesc = CStr(ReadColumn(reader, "CATEGORY_LONG_DESC"))
            .WebCatgyList = CStr(ReadColumn(reader, "CTGY_LIST"))
            .ProductName = CStr(ReadColumn(reader, "FRNDLY_PRDCT_DESC"))
            .LabelID = CInt(ReadColumn(reader, "LABEL_ID"))
            .LabelDesc = CStr(ReadColumn(reader, "LABEL_LONG_DESC"))
            .BrandID = CShort(ReadColumn(reader, "BRAND_ID"))
            .BrandDesc = CStr(ReadColumn(reader, "BRAND_DESC"))
            .ISN = CDec(ReadColumn(reader, "INTERNAL_STYLE_NUM"))
            .Vendor = CStr(ReadColumn(reader, "VENDOR_NME"))
            .VendorStyleNumber = CStr(ReadColumn(reader, "VENDOR_STYLE_NUM"))
            .DropShipFlg = CChar(ReadColumn(reader, "DROP_SHIP_FLG"))
            .DropShipID = CShort(ReadColumn(reader, "DROP_SHIP_ID"))
            .DropShipDesc = CStr(ReadColumn(reader, "DROP_SHIP_DESC"))
            .IntRetInsCde = CShort(ReadColumn(reader, "IRI_CDE"))
            .IntReturnInstrct = CStr(ReadColumn(reader, "IRI_DESC"))
            .ExtRetInsCde = CShort(ReadColumn(reader, "ERI_CDE"))
            .ExtReturnInstrct = CStr(ReadColumn(reader, "ERI_DESC"))
            .AgeCde = CShort(ReadColumn(reader, "AGE_CDE"))
            .AgeDesc = CStr(ReadColumn(reader, "AGE_DESC"))
            .GenderCde = CShort(ReadColumn(reader, "GENDER_CDE"))
            .GenderDesc = CStr(ReadColumn(reader, "GENDER_DESC"))
            .FriendlyColor = CStr(ReadColumn(reader, "FRNDLY_CLR"))
            .FeatureID = CInt(ReadColumn(reader, "FEATURE_IMAGE_NUM"))
            .ImageID = CInt(ReadColumn(reader, "IMAGE_ID_NUM"))
            .NonSwatchClrCde = CInt(ReadColumn(reader, "NON_SWATCH_CLR_CDE"))
            .NonSwatchClrDesc = CStr(ReadColumn(reader, "NON_SWATCH_CLR_DESC"))
            .ColorFamily = CStr(ReadColumn(reader, "COLOR_FAMILY"))
            .FRS = CStr(ReadColumn(reader, "F_R_S"))
            .AdNbrAdminImgNbr = CStr(ReadColumn(reader, "AD_NUM_ADMIN_IMG_NUM"))
            .ImageNotes = ""
            .UPC = CDec(ReadColumn(reader, "UPC_NUM"))
            .VendorSize = CStr(ReadColumn(reader, "VENDOR_SIZE"))
            .WebCatSizeID = CInt(ReadColumn(reader, "WEBCAT_SIZE_ID"))
            .WebCatSizeDesc = CStr(ReadColumn(reader, "WEBCAT_SIZE_DESC"))
            .SizeFamID = CShort(ReadColumn(reader, "SIZE_FAM_ID"))
            .SizeFamily = CStr(ReadColumn(reader, "SIZE_FAMILY"))
            .IsValidFlg = CChar(ReadColumn(reader, "IS_VALID"))
            .MerchantNotes = Trim(CStr(ReadColumn(reader, "IMAGE_MERCHANT_TXT")))
            .EMMNotes = Trim(CStr(ReadColumn(reader, "IMAGE_EMM_NOTE_TXT")))
            .LastModifiedDate = CDate(ReadColumn(reader, "LAST_MOD_TS"))
            .AdNbr = CStr(ReadColumn(reader, "AD_NUM"))
            .ISNBrandID = CInt(ReadColumn(reader, "ISN_BRAND_ID"))
            .WebCatAvailableQty = CStr(ReadColumn(reader, "WEBCAT_QTY"))
            .ImageGroup = CShort(ReadColumn(reader, "IMAGE_MDSE_GRP_NUM"))
            .PrimaryActualURL = CStr(ReadColumn(reader, "PRIMARY_ACTUAL_URL"))
            .PrimaryThumbnailURL = CStr(ReadColumn(reader, "PRIMARY_THUMBNAIL_URL"))
        End With

        Return ECommPriorityInfo
    End Function


End Class
