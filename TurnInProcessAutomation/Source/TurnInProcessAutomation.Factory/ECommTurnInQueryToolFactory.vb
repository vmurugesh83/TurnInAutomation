Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common
Public Class ECommTurnInQueryToolFactory

    Public Shared Function Construct(ByVal reader As DB2DataReader) As ECommTurnInQueryToolInfo
        Dim ECommTurnInQueryToolInfo As New ECommTurnInQueryToolInfo
        With ECommTurnInQueryToolInfo

            If (ReadColumn(reader, "TURN_IN_MDSE_ID")) IsNot DBNull.Value Then
                .TurnInMerchID = CInt(ReadColumn(reader, "TURN_IN_MDSE_ID"))
            End If
            If (ReadColumn(reader, "AD_NUM")) IsNot DBNull.Value Then
                .AD_NUM = CStr(ReadColumn(reader, "AD_NUM"))
            End If
            If (ReadColumn(reader, "AD_SYSTEM_PAGE_NUM")) IsNot DBNull.Value Then
                .PAGE_NUM = CInt(ReadColumn(reader, "AD_SYSTEM_PAGE_NUM"))
            End If
            If (ReadColumn(reader, "TURN_IN_USAGE_CDE")) IsNot DBNull.Value Then
                .Turn_in_Indicator = CStr(ReadColumn(reader, "TURN_IN_USAGE_CDE"))
            End If
            If (ReadColumn(reader, "OO")) IsNot DBNull.Value Then
                .OO = CStr(ReadColumn(reader, "OO"))
            End If
            If (ReadColumn(reader, "OH")) IsNot DBNull.Value Then
                .OH = CStr(ReadColumn(reader, "OH"))
            End If
            If (ReadColumn(reader, "INSTOREDATE")) IsNot DBNull.Value Then
                .Ship_Date = CStr(ReadColumn(reader, "INSTOREDATE"))
            End If
            If (ReadColumn(reader, "TRNIN_MDSESTAT_CDE")) IsNot DBNull.Value Then
                .TIStatus = CStr(ReadColumn(reader, "TRNIN_MDSESTAT_CDE"))
            End If
            If (ReadColumn(reader, "RESERVED_ISN_FLG")) IsNot DBNull.Value Then
                .ReserveFlag = CStr(ReadColumn(reader, "RESERVED_ISN_FLG"))
            End If
            If (ReadColumn(reader, "DPT_ID_DESC")) IsNot DBNull.Value Then
                .Department = CStr(ReadColumn(reader, "DPT_ID_DESC"))
            End If
            If (ReadColumn(reader, "BUYER")) IsNot DBNull.Value Then
                .Buyer = CStr(ReadColumn(reader, "BUYER"))
            End If
            If (ReadColumn(reader, "VND_ID_NME")) IsNot DBNull.Value Then
                .Vendor = CStr(ReadColumn(reader, "VND_ID_NME"))
            End If
            If (ReadColumn(reader, "VENDOR_STYLE_NUM")) IsNot DBNull.Value Then
                .Vendor_Style = CStr(ReadColumn(reader, "VENDOR_STYLE_NUM"))
            End If
            If (ReadColumn(reader, "INTERNAL_STYLE_NUM")) IsNot DBNull.Value Then
                .ISN = CStr(ReadColumn(reader, "INTERNAL_STYLE_NUM"))
            End If
            If (ReadColumn(reader, "ISN_LONG_DESC")) IsNot DBNull.Value Then
                .Style_Desc = CStr(ReadColumn(reader, "ISN_LONG_DESC"))
            End If
            If (ReadColumn(reader, "FRNDLY_PRDCT_DESC")) IsNot DBNull.Value Then
                .Friendly_Product_Description = CStr(ReadColumn(reader, "FRNDLY_PRDCT_DESC"))
            End If
            If (CStr(ReadColumn(reader, "CLR_DESC")) IsNot DBNull.Value) Then
                .Color = CStr(ReadColumn(reader, "CLR_DESC"))
            End If
            If (ReadColumn(reader, "FRIENDLY_COLOR_NME")) IsNot DBNull.Value Then
                .Friendly_color = CStr(ReadColumn(reader, "FRIENDLY_COLOR_NME"))
            End If
            If (ReadColumn(reader, "SIZE_1_DESC")) IsNot DBNull.Value Then
                .SampleSize = CStr(ReadColumn(reader, "SIZE_1_DESC"))
            End If
            If (ReadColumn(reader, "IMAGE_CATEGORY_CDE")) IsNot DBNull.Value Then
                .Feature_Render_Swatch = CStr(ReadColumn(reader, "IMAGE_CATEGORY_CDE"))
            End If
            If (ReadColumn(reader, "ADMIN_MDSE_NUM")) IsNot DBNull.Value Then
                .MerchID = CStr(ReadColumn(reader, "ADMIN_MDSE_NUM"))
            End If
            If (ReadColumn(reader, "ADMIN_IMAGE_NUM")) IsNot DBNull.Value Then
                .Image_ID = CStr(ReadColumn(reader, "ADMIN_IMAGE_NUM"))
            End If
            If (ReadColumn(reader, "IMAGE_FILE_URL_TXT")) IsNot DBNull.Value Then
                .VT_Path = CStr(ReadColumn(reader, "IMAGE_FILE_URL_TXT"))
            End If
            If (ReadColumn(reader, "MODEL_CATG_DESC")) IsNot DBNull.Value Then
                .Model_Category = CStr(ReadColumn(reader, "MODEL_CATG_DESC"))
            End If
            If (ReadColumn(reader, "MDSE_FIGURE_CDE")) IsNot DBNull.Value Then
                .OnOff_Figure = CStr(ReadColumn(reader, "MDSE_FIGURE_CDE"))
            End If
            If (ReadColumn(reader, "WEB_CATEGORY")) IsNot DBNull.Value Then
                .Feature_Web_Cat = CStr(ReadColumn(reader, "WEB_CATEGORY"))
            End If
            If (ReadColumn(reader, "FEATURE_IMAGE_NUM")) IsNot DBNull.Value Then
                .Feature_Image_ID = CStr(ReadColumn(reader, "FEATURE_IMAGE_NUM"))
            End If
            If (ReadColumn(reader, "MDSE_HOT_LIST_CDE")) IsNot DBNull.Value Then
                .Hot_Rushed = CStr(ReadColumn(reader, "MDSE_HOT_LIST_CDE"))
            End If
            If (ReadColumn(reader, "IMAGE_SUFFIX")) IsNot DBNull.Value Then
                .ImageSuffix = CStr(ReadColumn(reader, "IMAGE_SUFFIX"))
            End If
            If (ReadColumn(reader, "InWebCat")) IsNot DBNull.Value Then
                .InWebCat = CStr(ReadColumn(reader, "InWebCat"))
            End If
            If (ReadColumn(reader, "ALT_VIEW")) IsNot DBNull.Value Then
                .AltView = CStr(ReadColumn(reader, "ALT_VIEW"))
            End If
            If (ReadColumn(reader, "IMGE_CLR_CORCT_FLG")) IsNot DBNull.Value Then
                .ClrCorrectFlg = CChar(ReadColumn(reader, "IMGE_CLR_CORCT_FLG"))
            End If
            If (ReadColumn(reader, "IMAGE_EMM_NOTE_TXT")) IsNot DBNull.Value Then
                .EMMNotes = CStr(ReadColumn(reader, "IMAGE_EMM_NOTE_TXT"))
            End If
            If (ReadColumn(reader, "AD_NUM_ADMIN_IMG_NUM")) IsNot DBNull.Value Then
                .AdNbrAdminImgNbr = CStr(ReadColumn(reader, "AD_NUM_ADMIN_IMG_NUM"))
            End If
            If (ReadColumn(reader, "IMAGE_MERCHANT_TXT")) IsNot DBNull.Value Then
                .MerchantNotes = CStr(ReadColumn(reader, "IMAGE_MERCHANT_TXT"))
            End If
            If (ReadColumn(reader, "MDSE_TRNIN_BTCH_ID")) IsNot DBNull.Value Then
                .BatchNum = CInt(ReadColumn(reader, "MDSE_TRNIN_BTCH_ID"))
            End If
            If (ReadColumn(reader, "REMOVE_MDSE_FLG")) IsNot DBNull.Value Then
                .RemoveMerchFlg = CChar(ReadColumn(reader, "REMOVE_MDSE_FLG"))
            End If
            If (ReadColumn(reader, "ROUTE_FROM_AD")) IsNot DBNull.Value Then
                .RouteFromAD = CInt(ReadColumn(reader, "ROUTE_FROM_AD"))
            End If
            If (ReadColumn(reader, "AD_TURN_IN_DTE")) IsNot DBNull.Value Then
                .Turn_in_Date = CDate(ReadColumn(reader, "AD_TURN_IN_DTE"))
            End If
            If (ReadColumn(reader, "ADMIN_IMGENOTE_TXT")) IsNot DBNull.Value Then
                .ImageNotes = CStr(ReadColumn(reader, "ADMIN_IMGENOTE_TXT"))
            End If
        End With
        Return ECommTurnInQueryToolInfo
    End Function

End Class
