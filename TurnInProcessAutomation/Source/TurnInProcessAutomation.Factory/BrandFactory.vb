Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class BrandFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As BrandInfo
        Dim BrndInfo As New BrandInfo()

        With BrndInfo
            .BrandId = CShort(ReadColumn(reader, "BRAND_ID"))
            .BrandDesc = CStr(ReadColumn(reader, "BRAND_DESC"))
        End With

        Return BrndInfo
    End Function
End Class
