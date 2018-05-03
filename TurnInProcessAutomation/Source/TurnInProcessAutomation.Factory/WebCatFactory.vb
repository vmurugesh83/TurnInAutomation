Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class WebCatFactory

    Public Shared Function Construct(ByVal reader As DB2DataReader) As WebCat
        Dim WebCatInfo As New WebCat

        With WebCatInfo
            If CInt(ReadColumn(reader, "CATEGORY_CDE")) > 0 Then
                .CategoryCode = CInt(ReadColumn(reader, "CATEGORY_CDE"))
                .ParentCategoryCode = CInt(ReadColumn(reader, "PARENT_CDE"))
                .CategoryName = CStr(ReadColumn(reader, "CATEGORY_DESC"))
                .CategoryLongDesc = CStr(ReadColumn(reader, "CATEGORY_LONG_DESC"))
                If .CategoryLongDesc IsNot Nothing Then
                    .CategoryLongDesc = .CategoryLongDesc.Replace("Linked from", "").Replace("above category nav", "").Replace("""", "")
                End If

                .DefaultCategoryFlag = CBool(ReadColumn(reader, "PRIM_WEB_CATGY_FLG"))
                .DisplayOnlyFlag = CStr(ReadColumn(reader, "DISPLAY_ONLY_FLG"))
            End If
        End With

        Return WebCatInfo
    End Function

    Public Shared Function ConstructProductAndUPC(ByVal reader As DB2DataReader) As WebcatProductInfo
        Dim webCatProductUPCInfo As New WebcatProductInfo
        Dim webcatUPCInfo As New WebCatUPCInfo

        With webCatProductUPCInfo
            .PRODUCT_CDE = CInt(ReadColumn(reader, "PRODUCT_CDE"))
            .AGE_CDE = CInt(ReadColumn(reader, "AGE_CDE"))
            .GENDER_CDE = CInt(ReadColumn(reader, "GENDER_CDE"))
            .DS_RETURN_IND = CInt(ReadColumn(reader, "DS_RETURN_IND"))
            .DS_RETURN_EXT_IND = CInt(ReadColumn(reader, "DS_RETURN_EXT_IND"))
            .BRAND_KEY_NUM = CInt(ReadColumn(reader, "BRAND_KEY_NUM"))
            .PRODUCT_DESC = CStr(ReadColumn(reader, "PRODUCT_DESC"))
            If CStr(ReadColumn(reader, "SIZE_FLG")) = "Y" Then
                .SIZE_FLAG = True
            Else
                .SIZE_FLAG = False
            End If
            If CStr(ReadColumn(reader, "COLOR_FLG")) = "Y" Then
                .COLOR_FLAG = True
            Else
                .COLOR_FLAG = False
            End If

            With webcatUPCInfo
                .UPC_NUM = CDec(ReadColumn(reader, "UPC_NUM"))
                .COLOR_FAMILY = CStr(ReadColumn(reader, "COLOR_FAMILY"))
                .SIZE_FAMILY = CStr(ReadColumn(reader, "SIZE_FAMILY"))
                .SIZE_OPTION_CDE = CInt(ReadColumn(reader, "UPC_SIZE_OPTION"))
                .COLOR_OPTION_CDE = CInt(ReadColumn(reader, "UPC_COLOR_OPTION"))
                .SWATCH_IMAGE_ID = CInt(ReadColumn(reader, "SWATCH_ID"))
                .LARGE_IMAGE_ID = CStr(ReadColumn(reader, "LARGE_IMAGE_ID"))
            End With
            .UPCInfo = webcatUPCInfo
        End With

        Return webCatProductUPCInfo
    End Function
End Class

