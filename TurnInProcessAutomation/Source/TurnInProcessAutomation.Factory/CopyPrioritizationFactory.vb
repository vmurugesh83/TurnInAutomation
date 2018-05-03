Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class CopyPrioritizationFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As CopyPrioritizationInfo
        Dim CopyPrioritizationInfo As New CopyPrioritizationInfo()

        With CopyPrioritizationInfo
            .RowID = CInt(ReadColumn(reader, "ROWID"))
            .ImageID = CInt(ReadColumn(reader, "IMAGE_ID_NUM"))
            If Not ReadColumn(reader, "ADMIN_IMAGE_NUM") Is Nothing AndAlso Not IsDBNull(reader.Item("ADMIN_IMAGE_NUM")) Then
                .AdminImageID = CInt(ReadColumn(reader, "ADMIN_IMAGE_NUM"))
            Else
                .AdminImageID = 0
            End If
            .ProductCode = CInt(ReadColumn(reader, "PRODUCT_CDE"))
            .DeptID = CInt(ReadColumn(reader, "DEPT_ID"))
            .DeptIdDesc = CStr(ReadColumn(reader, "DEPT_ID")) + " " + CStr(ReadColumn(reader, "DEPT_LONG_DESC"))
            .VendorStyleNumber = CStr(ReadColumn(reader, "VENDOR_STYLE_NUM"))
            .ProductName = CStr(ReadColumn(reader, "PRODUCT_NME"))
            .BrandID = CShort(ReadColumn(reader, "BRAND_KEY_NUM"))
            .BrandDesc = CStr(ReadColumn(reader, "BRAND"))
            .ISN = CDec(ReadColumn(reader, "INTERNAL_STYLE_NUM"))
            .ColorCode = CInt(ReadColumn(reader, "CLR_CDE"))
            .UPC = CLng(ReadColumn(reader, "UPC_NUM"))
            .WebCatAvailableQty = CInt(ReadColumn(reader, "AVAIL_QTY"))
            .PrimaryActualURL = CStr(ReadColumn(reader, "IMAGE_URL"))
            .PrimaryThumbnailURL = CStr(ReadColumn(reader, "IMAGE_FILE_URL_TXT"))
            .LargeImageUrl = CStr(ReadColumn(reader, "LARGE_IMAGE_URL"))
            .WeightedInventory = 1
            'If String.IsNullOrEmpty(CStr(ReadColumn(reader, "DC_SHIP_DTE"))) OrElse CDate(ReadColumn(reader, "DC_SHIP_DTE")).Equals(Date.MinValue) Then
            '    .POStartShipDate = String.Empty
            'Else
            .POStartShipDate = CDate(ReadColumn(reader, "DC_SHIP_DTE"))
            'End If
            .FeaturedColorSize = CStr(ReadColumn(reader, "SIZE_COLOR"))
            .ProductDetails = CStr(ReadColumn(reader, "PRODUCT_LONG_DESC"))
            .ProductDetailsMore = CStr(ReadColumn(reader, "PRODUCT_DESC_MORE"))
            .Hierarchy = CStr(ReadColumn(reader, "HIERARCHY"))
            .CategoryName = CStr(ReadColumn(reader, "CATEGORY_NAME"))
            .FabricDescription = CStr(ReadColumn(reader, "TURNIN_FABRIC_DESC"))
            .Origination = CStr(ReadColumn(reader, "PRDCT_ORIG_DESC"))
            .CategoryCode = CInt(ReadColumn(reader, "CATEGORY_CDE"))
            .ImageDetails = CStr(ReadColumn(reader, "IMAGE_DETAILS"))
            .OnHand = CInt(ReadColumn(reader, "ON_HAND"))
            .OnOrder = CInt(ReadColumn(reader, "ON_ORDER"))
            .PriceStatusCode = CStr(ReadColumn(reader, "CUR_STATUS_CDE"))
            .SKUUseCode = CStr(ReadColumn(reader, "SKU_USE_CDE"))
            .ProductReadyDate = CDate(ReadColumn(reader, "PRODUCT_CREATE_DTE"))
            .ThirdPartyFulfilmentCode = CInt(ReadColumn(reader, "SHIP_FF_3PARTY_CDE"))
            .OwnedPrice = CDec(ReadColumn(reader, "OWN_PRICE_AMT"))
            .GenderDesc = CStr(ReadColumn(reader, "GENDER_DESC"))
            .AgeDesc = CStr(ReadColumn(reader, "AGE_DESC"))
            .ProductNotes = CStr(ReadColumn(reader, "WC_NOTES_TXT"))
        End With

        Return CopyPrioritizationInfo
    End Function

    Public Shared Function ConstructAvailableColors(ByVal reader As DB2DataReader) As CopyPrioritizationColorInfo
        Dim CopyPrioritizationColorInfo As New CopyPrioritizationColorInfo()

        With CopyPrioritizationColorInfo
            .ColorFamily = CStr(ReadColumn(reader, "COLOR_FAMILY"))
            .FriendlyColorName = CStr(ReadColumn(reader, "FRIENDLY_COLOR"))
            .ColorSequenceNumber = CInt(ReadColumn(reader, "COLOR_SEQ_NUM"))
            .FriendlySizeName = CStr(ReadColumn(reader, "FRIENDLY_SIZE"))
            .SizeFamily = CStr(ReadColumn(reader, "SIZE_FAMILY"))
            .SizeSequenceNumber = CInt(ReadColumn(reader, "SIZE_SEQ_NUM"))
        End With

        Return CopyPrioritizationColorInfo
    End Function

    Public Shared Function ConstructImages(ByVal reader As DB2DataReader) As CopyPrioritizationImageInfo
        Dim CopyPrioritizationImageInfo As New CopyPrioritizationImageInfo()

        With CopyPrioritizationImageInfo
            .UPC = CLng(ReadColumn(reader, "UPC_NUM"))
            .LargeImageID = CStr(ReadColumn(reader, "LARGE_IMAGE_ID"))
            .ISN = CDec(ReadColumn(reader, "INTERNAL_STYLE_NUM"))
            .ColorCode = CInt(ReadColumn(reader, "CLR_CDE"))
        End With

        Return CopyPrioritizationImageInfo
    End Function
End Class
