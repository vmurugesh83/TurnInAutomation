
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class GXSCopyViewFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As GXSCopyViewInfo
        Dim GXSCopyViewInfo As New GXSCopyViewInfo()
        With GXSCopyViewInfo
            .IMAGE_ID = CStr(IIf(IsDBNull(ReadColumn(reader, "Image_Id")), 0, ReadColumn(reader, "Image_Id")))
            .INTERNAL_STYLE_NUM = CDec(IIf(IsDBNull(ReadColumn(reader, "INTERNAL_STYLE_NUM")), 0, ReadColumn(reader, "INTERNAL_STYLE_NUM")))
            .LABEL = CStr(IIf(IsDBNull(ReadColumn(reader, "Label")), "", ReadColumn(reader, "Label")))
            .PRODUCT_NAME = CStr(IIf(IsDBNull(ReadColumn(reader, "Product Name")), "", ReadColumn(reader, "Product Name")))
            .OO = CInt(IIf(IsDBNull(ReadColumn(reader, "OO")), 0, ReadColumn(reader, "OO")))
            .OH = CInt(IIf(IsDBNull(ReadColumn(reader, "OH")), 0, ReadColumn(reader, "OH")))
            .PO_STARTSHIPDT = CStr(IIf(IsDBNull(ReadColumn(reader, "PO_STARTSHIPDT")), "", ReadColumn(reader, "PO_STARTSHIPDT")))
            .PRICESTATUS = CStr(IIf(IsDBNull(ReadColumn(reader, "PRICESTATUS")), "", ReadColumn(reader, "PRICESTATUS")))
            .FEATUREDCOLOR = CStr(IIf(IsDBNull(ReadColumn(reader, "FEATUREDCOLOR")), "", ReadColumn(reader, "FEATUREDCOLOR")))
            .UPC_NUM = CDec(IIf(IsDBNull(ReadColumn(reader, "UPC_NUM")), 0, ReadColumn(reader, "UPC_NUM")))
            .FEATURE = CStr(IIf(IsDBNull(ReadColumn(reader, "FEATURE")), "", ReadColumn(reader, "FEATURE")))
            .COLOR = CStr(IIf(IsDBNull(ReadColumn(reader, "Color")), "", ReadColumn(reader, "Color")))
            .SIZE = CStr(IIf(IsDBNull(ReadColumn(reader, "SIZE")), "", ReadColumn(reader, "SIZE")))
            .PO_STATUS_CDE = CStr(IIf(IsDBNull(ReadColumn(reader, "PO_STATUS_CDE")), "", ReadColumn(reader, "PO_STATUS_CDE")))
        End With
        Return GXSCopyViewInfo
    End Function
End Class
