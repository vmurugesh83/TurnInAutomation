Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common
Public Class EcommTurnInMeetFactory

    Public Shared Function Construct(ByVal reader As DB2DataReader) As ECommTurnInMeetCreateInfo
        Dim EcommTurninMeetInfo As New ECommTurnInMeetCreateInfo
        Try
            With EcommTurninMeetInfo
                If (ReadColumn(reader, "MDSE_TRNIN_BTCH_ID") IsNot DBNull.Value) Then
                    .BatchNum = CInt(ReadColumn(reader, "MDSE_TRNIN_BTCH_ID"))
                End If

                If (ReadColumn(reader, "REMOVE_MDSE_FLG")) IsNot DBNull.Value Then
                    .RemoveMerchFlag = Trim(CStr(ReadColumn(reader, "REMOVE_MDSE_FLG")))
                End If

                If (ReadColumn(reader, "INTERNAL_STYLE_NUM")) IsNot DBNull.Value Then
                    .ISN = Trim(CStr(ReadColumn(reader, "INTERNAL_STYLE_NUM")))
                End If

                If (ReadColumn(reader, "ISN_LONG_DESC")) IsNot DBNull.Value Then
                    .ISNDesc = CStr(ReadColumn(reader, "ISN_LONG_DESC"))
                End If

                If (ReadColumn(reader, "AD_NUM") IsNot DBNull.Value) Then
                    .AdNumber = CInt(ReadColumn(reader, "AD_NUM"))
                End If
                If (ReadColumn(reader, "AD_SYSTEM_PAGE_NUM") IsNot DBNull.Value) Then
                    .PageNumber = CInt(ReadColumn(reader, "AD_SYSTEM_PAGE_NUM"))
                End If
                If (ReadColumn(reader, "VENDOR_ID") IsNot DBNull.Value) Then
                    .VendorId = CInt(ReadColumn(reader, "VENDOR_ID"))
                End If

                If (ReadColumn(reader, "DEPT_ID") IsNot DBNull.Value) Then
                    .DeptID = CInt(ReadColumn(reader, "DEPT_ID"))
                End If
                If (ReadColumn(reader, "LABEL_ID") IsNot DBNull.Value) Then
                    .LabelID = CInt(ReadColumn(reader, "LABEL_ID"))
                End If
                If (ReadColumn(reader, "LABEL") IsNot DBNull.Value) Then
                    .Label = Trim(CStr(ReadColumn(reader, "LABEL")))
                End If
                If (ReadColumn(reader, "BUYER_ID") IsNot DBNull.Value) Then
                    .BuyerId = CInt(ReadColumn(reader, "BUYER_ID"))
                End If
                '.AdDesc = Trim(CStr(ReadColumn(reader, "AD_DESC"))
                If (ReadColumn(reader, "VENDOR_STYLE_NUM") IsNot DBNull.Value) Then
                    .VendorStyleNumber = Trim(CStr(ReadColumn(reader, "VENDOR_STYLE_NUM")))
                End If
                If (ReadColumn(reader, "BRAND_ID") IsNot DBNull.Value) Then
                    .BrandId = CInt(ReadColumn(reader, "BRAND_ID"))
                End If
                If (ReadColumn(reader, "IMAGE_MERCHANT_TXT") IsNot DBNull.Value) Then
                    .MerchantNotes = Trim(CStr(ReadColumn(reader, "IMAGE_MERCHANT_TXT")))
                End If
                If (ReadColumn(reader, "MERCHANDISE_AD_NUM") IsNot DBNull.Value) Then
                    .RoutefromAd = Trim(CStr(ReadColumn(reader, "MERCHANDISE_AD_NUM")))
                End If
                If (ReadColumn(reader, "MDSE_FIGURE_CDE") IsNot DBNull.Value) Then
                    .OnOff = Trim(CStr(ReadColumn(reader, "MDSE_FIGURE_CDE")))
                End If
                If (ReadColumn(reader, "PICKUP_IMAGE_FLG") IsNot DBNull.Value) Then
                    .pickup = Trim(CStr(ReadColumn(reader, "PICKUP_IMAGE_FLG"))) 'not created yet
                End If
                If (ReadColumn(reader, "ADMIN_IMAGE_NUM") IsNot DBNull.Value) Then
                    .PickupImageID = Trim(CStr(ReadColumn(reader, "ADMIN_IMAGE_NUM")))
                End If
                If (ReadColumn(reader, "SIZE_ID") IsNot DBNull.Value) Then
                    .SampleSize = Trim(CStr(ReadColumn(reader, "SIZE_ID")))
                End If
                If (ReadColumn(reader, "IMGE_CLR_CORCT_FLG") IsNot DBNull.Value) Then
                    .ColorCorrect = CChar(ReadColumn(reader, "IMGE_CLR_CORCT_FLG"))
                End If
                If (ReadColumn(reader, "ADMIN_MDSE_NUM") IsNot DBNull.Value) Then
                    .MerchID = CInt(ReadColumn(reader, "ADMIN_MDSE_NUM"))
                End If
                If (ReadColumn(reader, "FRNDLY_PRDCT_DESC") IsNot DBNull.Value) Then
                    .FriendlyProdDesc = Trim(CStr(ReadColumn(reader, "FRNDLY_PRDCT_DESC")))
                End If
                If (ReadColumn(reader, "CATEGORY_CDE") IsNot DBNull.Value) Then
                    .CategoryCode = CInt(ReadColumn(reader, "CATEGORY_CDE"))
                End If
                If (ReadColumn(reader, "FRIENDLY_COLOR_NME") IsNot DBNull.Value) Then
                    .FriendlyColor = Trim(CStr(ReadColumn(reader, "FRIENDLY_COLOR_NME")))
                End If
                If (ReadColumn(reader, "IMAGE_CATEGORY_CDE") IsNot DBNull.Value) Then
                    .FeatureSwatch = Trim(CStr(ReadColumn(reader, "IMAGE_CATEGORY_CDE")))
                End If
                If (ReadColumn(reader, "FEATURE_IMAGE_NUM") IsNot DBNull.Value) Then
                    .FeatureID = CInt(ReadColumn(reader, "FEATURE_IMAGE_NUM"))
                End If
                If (ReadColumn(reader, "IMAGE_EMM_NOTE_TXT") IsNot DBNull.Value) Then
                    .EMMNotes = Trim(CStr(ReadColumn(reader, "IMAGE_EMM_NOTE_TXT")))
                End If

                If (ReadColumn(reader, "IMAGE_KIND_TYP") IsNot DBNull.Value) Then
                    .ImageKindCode = Trim(CStr(ReadColumn(reader, "IMAGE_KIND_TYP")))
                End If
                If (ReadColumn(reader, "IMAGE_KIND_DESC") IsNot DBNull.Value) Then
                    .ImageKindDescription = Trim(CStr(ReadColumn(reader, "IMAGE_KIND_DESC")))
                End If

                If (ReadColumn(reader, "TURN_IN_MDSE_ID") IsNot DBNull.Value) Then
                    .turnInMerchID = CInt(ReadColumn(reader, "TURN_IN_MDSE_ID"))
                End If
                If (ReadColumn(reader, "LAST_MODIFIED_ID") IsNot DBNull.Value) Then
                    .LastModBy = Trim(CStr(ReadColumn(reader, "LAST_MODIFIED_ID")))
                End If

                If (ReadColumn(reader, "MDSE_HOT_LIST_CDE") IsNot DBNull.Value) Then
                    .HotListCDE = Trim(CStr(ReadColumn(reader, "MDSE_HOT_LIST_CDE")))
                End If
                If (ReadColumn(reader, "RESERVED_ISN_FLG") IsNot DBNull.Value) Then
                    .IsReserve = Trim(CStr(ReadColumn(reader, "RESERVED_ISN_FLG")))
                End If
                If (ReadColumn(reader, "SIZE_CATEGORY") IsNot DBNull.Value) Then
                    .SizeCategory = Trim(CStr(ReadColumn(reader, "SIZE_CATEGORY")))
                End If
                If (ReadColumn(reader, "ADMIN_IMAGE_DESC") IsNot DBNull.Value) Then
                    .ImageDesc = Trim(CStr(ReadColumn(reader, "ADMIN_IMAGE_DESC")))
                End If
                If (ReadColumn(reader, "IMAGE_MDSE_GRP_NUM") IsNot DBNull.Value) Then
                    .ImageGrp = Trim(CStr(ReadColumn(reader, "IMAGE_MDSE_GRP_NUM")))
                End If
                If (ReadColumn(reader, "ADMIN_IMGENOTE_TXT") IsNot DBNull.Value) Then
                    .ImageNotes = Trim(CStr(ReadColumn(reader, "ADMIN_IMGENOTE_TXT")))
                End If
                If (ReadColumn(reader, "MODEL_CATEGORY_CDE") IsNot DBNull.Value) Then
                    .ModelCategory = Trim(CStr(ReadColumn(reader, "MODEL_CATEGORY_CDE")))
                End If
                If (ReadColumn(reader, "IMAGE_STYLING_TXT") IsNot DBNull.Value) Then
                    .StylingNotes = Trim(CStr(ReadColumn(reader, "IMAGE_STYLING_TXT")))
                End If
                If (ReadColumn(reader, "TURNIN_IMGE_REQ_ID") IsNot DBNull.Value) Then
                    .TIImageId = Trim(CStr(ReadColumn(reader, "TURNIN_IMGE_REQ_ID")))
                End If
                If (ReadColumn(reader, "IMAGE_CPY_NOTE_TXT") IsNot DBNull.Value) Then
                    .CpyNotes = Trim(CStr(ReadColumn(reader, "IMAGE_CPY_NOTE_TXT")))
                End If
                If (ReadColumn(reader, "FAB_LONG_DESC") IsNot DBNull.Value) Then
                    .Fabrication = Trim(CStr(ReadColumn(reader, "FAB_LONG_DESC")))
                End If
                If (ReadColumn(reader, "PRDCT_FEATURES_TXT") IsNot DBNull.Value) Then
                    .FeaturesBenefits = Trim(CStr(ReadColumn(reader, "PRDCT_FEATURES_TXT")))
                End If

                If (ReadColumn(reader, "BUYER_NME") IsNot DBNull.Value) Then
                    .BuyerName = Trim(CStr(ReadColumn(reader, "BUYER_NME")))
                End If
                If (ReadColumn(reader, "BUYER_EXT") IsNot DBNull.Value) Then
                    .BuyerExt = Trim(CStr(ReadColumn(reader, "BUYER_EXT")))
                End If
                If (ReadColumn(reader, "MDSE_FIGURE_CDE") IsNot DBNull.Value) Then
                    .FigureCode = Trim(CStr(ReadColumn(reader, "MDSE_FIGURE_CDE")))
                End If
                If (ReadColumn(reader, "IMAGE_CATEGORY_CDE") IsNot DBNull.Value) Then
                    .ImageCategoryCode = Trim(CStr(ReadColumn(reader, "IMAGE_CATEGORY_CDE")))
                End If
                If (ReadColumn(reader, "ALT_VIEW") IsNot DBNull.Value) Then
                    .AltView = CStr(ReadColumn(reader, "ALT_VIEW"))
                End If
                If (ReadColumn(reader, "VENDOR_COLOR") IsNot DBNull.Value) Then
                    .VendorColor = CStr(ReadColumn(reader, "VENDOR_COLOR"))
                End If
                'KL added below 05/08/15
                If (ReadColumn(reader, "CLR_CDE") IsNot DBNull.Value) Then
                    .ColorCode = CInt(ReadColumn(reader, "CLR_CDE"))
                End If
                .WebCatgyList = CStr(ReadColumn(reader, "CTGY_LIST"))
                .EMMFollowUpFlag = CStr(ReadColumn(reader, "EMM_FLWUP_FLG"))
                .CCFollowUpFlag = CStr(ReadColumn(reader, "CC_FLWUP_FLG"))
                .CWFollowUpFlag = CStr(ReadColumn(reader, "CW_FLWUP_FLG"))
                .Sequence = CInt(ReadColumn(reader, "MDSE_BTCH_ORDER_ID"))
                .ColorSequence = CInt(ReadColumn(reader, "MDSE_BTCH_SEQ_NUM"))
                If (ReadColumn(reader, "PRIM_THB_URL_TXT") IsNot Nothing) Then
                    .PrimaryThumbnailUrl = CStr(ReadColumn(reader, "PRIM_THB_URL_TXT"))
                End If
            End With
        Catch ex As Exception
            Throw ex
        End Try
        Return EcommTurninMeetInfo
    End Function
    Public Shared Function ConstructFeatureID(ByVal reader As DB2DataReader) As FeatureIDInfo
        Dim FeatureIDInfo As New FeatureIDInfo

        With FeatureIDInfo
            If (ReadColumn(reader, "INTERNAL_STYLE_NUM") IsNot DBNull.Value) Then
                .ISN = CDec(ReadColumn(reader, "INTERNAL_STYLE_NUM"))
            End If
            If (ReadColumn(reader, "IMAGE_ID_NUM") IsNot DBNull.Value) Then
                .ImageIdNum = CInt(ReadColumn(reader, "IMAGE_ID_NUM"))
            End If

            If (ReadColumn(reader, "UPC_NUM") IsNot DBNull.Value) Then
                .upcNum = CDec(ReadColumn(reader, "UPC_NUM"))
            End If
        End With
        Return FeatureIDInfo
    End Function

End Class
