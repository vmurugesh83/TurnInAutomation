
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class GXSStyleSkuFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As GXSStyleSkuInfo
        Dim GXSStyleSkuInfo As New GXSStyleSkuInfo()
        With GXSStyleSkuInfo
            .UPC_NUM = CDec(ReadColumn(reader, "UPC_NUM"))
            .SKU_NUM = CDec(ReadColumn(reader, "SKU_NUM"))
            .INTERNAL_STYLE_NUM = CDec(ReadColumn(reader, "INTERNAL_STYLE_NUM"))
            .Fabrication = CStr(ReadColumn(reader, "Fabrication"))
            .SellingLoc = CStr(ReadColumn(reader, "Selling Loc"))
            .ProdDtl1 = CStr(ReadColumn(reader, "Prod Dtl 1"))
            .ProdDtl2 = CStr(ReadColumn(reader, "Prod Dtl 2"))
            .ProdDtl3 = CStr(ReadColumn(reader, "Prod Dtl 3"))
            .AssembledIn = CStr(ReadColumn(reader, "Assembled In"))
            .GenClass = CStr(ReadColumn(reader, "Gen Class"))
            .GenSubcl = CStr(ReadColumn(reader, "Gen Subcl"))
            .Brand = CStr(ReadColumn(reader, "Brand"))
            .Label = CStr(ReadColumn(reader, "Label"))
            .FabDtl = CStr(ReadColumn(reader, "Fab Dtl"))
            .Lifestyle = CStr(ReadColumn(reader, "Lifestyle"))
            .Season = CStr(ReadColumn(reader, "Season"))
            .Occasion = CStr(ReadColumn(reader, "Occasion"))
            .Theme = CStr(ReadColumn(reader, "Theme"))
        End With
        Return GXSStyleSkuInfo
    End Function

End Class
