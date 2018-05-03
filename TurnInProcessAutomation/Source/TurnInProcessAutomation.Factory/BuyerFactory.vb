Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class BuyerFactory

    Public Shared Function ConstructBasic(ByVal reader As DB2DataReader) As BuyerInfo
        Dim BuyerInfo As New BuyerInfo()

        With BuyerInfo
            .BuyerId = CInt(ReadColumn(reader, "BUYER_ID"))
            .BuyerName = CStr(ReadColumn(reader, "BUYER_NME"))
            .BuyerDesc = CStr(ReadColumn(reader, "BUYER_DESC"))
            .BuyerRACFID = CStr(ReadColumn(reader, "BUYER_RACF_ID"))
        End With

        Return BuyerInfo
    End Function
End Class

