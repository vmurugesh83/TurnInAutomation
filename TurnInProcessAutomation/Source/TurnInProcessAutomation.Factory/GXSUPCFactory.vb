
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common
Public Class GXSUPCFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As GXSUPCInfo
        Dim GXSUPCInfo As New GXSUPCInfo()
        With GXSUPCInfo
            .UPC_NUM = CDec(ReadColumn(reader, "UPC_NUM"))
            .CLR_LONG_DESC = CStr(ReadColumn(reader, "CLR_LONG_DESC"))
            .SIZE = CStr(ReadColumn(reader, "SIZE"))
        End With
        Return GXSUPCInfo
    End Function
End Class
