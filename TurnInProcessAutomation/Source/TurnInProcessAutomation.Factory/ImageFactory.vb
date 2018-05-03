Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class ImageFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As ImageInfo
        Dim ImgInfo As New ImageInfo()

        With ImgInfo
            .ImageId = CInt(ReadColumn(reader, "FEATURE_IMAGE_NUM"))
            .TurnInMerchId = CInt(ReadColumn(reader, "TURN_IN_MDSE_ID"))
            .AdNbrAdminImgNbr = CStr(ReadColumn(reader, "AD_NUM_ADMIN_IMG_NUM"))
            .VendorStyle = CStr(ReadColumn(reader, "VENDOR_STYLE_NUM"))
            .ImageGroupNumber = CInt(ReadColumn(reader, "IMAGE_MDSE_GRP_NUM"))
            .ImageCategoryCode = CStr(ReadColumn(reader, "IMAGE_CATEGORY_CDE"))
        End With

        Return ImgInfo
    End Function
End Class
