Public Class WebcatProductInfo
    Public Property PRODUCT_CDE As Integer

    Public Property AGE_CDE As Integer

    Public Property GENDER_CDE As Integer

    Public Property DS_RETURN_IND As Integer

    Public Property DS_RETURN_EXT_IND As Integer

    Public Property BRAND_KEY_NUM As Integer

    Public Property PRODUCT_DESC As String

    Public Property COLOR_FLAG As Boolean

    Public Property SIZE_FLAG As Boolean

    Public Property UPCInfo As WebCatUPCInfo
End Class
Public Class WebCatUPCInfo
    Public Property UPC_NUM As Decimal

    Public Property COLOR_FAMILY As String

    Public Property SIZE_FAMILY As String

    Public Property SIZE_OPTION_CDE As Integer

    Public Property COLOR_OPTION_CDE As Integer

    Public Property SWATCH_IMAGE_ID As Integer

    Public Property LARGE_IMAGE_ID As String
End Class
