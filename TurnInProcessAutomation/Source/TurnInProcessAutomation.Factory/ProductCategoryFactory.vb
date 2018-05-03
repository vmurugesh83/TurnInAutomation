Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class ProductCategoryFactory

    Public Shared Function Construct(ByVal reader As DB2DataReader) As ProductCategoryInfo
        Dim productCategoryInfo As New ProductCategoryInfo

        With productCategoryInfo
            If CInt(ReadColumn(reader, "CATEGORY_CDE")) > 0 Then
                .CategoryCode = CInt(ReadColumn(reader, "CATEGORY_CDE"))
                .CategoryName = CStr(ReadColumn(reader, "CATEGORY_NME"))
                .Level = CInt(ReadColumn(reader, "LVL"))
                .Ordinal = CInt(ReadColumn(reader, "ORDINAL_NUM"))
                .ParentCategoryCode = CInt(ReadColumn(reader, "PARENT_CDE"))
            End If
        End With

        Return productCategoryInfo
    End Function
End Class

