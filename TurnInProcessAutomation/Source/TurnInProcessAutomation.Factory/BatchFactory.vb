Imports System.Data.OleDb
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common
Imports IBM.Data.DB2

Public Class BatchFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As BatchInfo
        Dim BatchInf As New BatchInfo()

        With BatchInf
            .BatchId = CInt(ReadColumn(reader, "MDSE_TRNIN_BTCH_ID"))
            .AdNumber = CInt(ReadColumn(reader, "AD_NUM"))
            .PageNumber = CInt(ReadColumn(reader, "AD_SYSTEM_PAGE_NUM"))
            .UserId = CStr(ReadColumn(reader, "LAST_MOD_ID"))
            .Buyer = CStr(ReadColumn(reader, "BUYER_NME"))
            .Departments = CStr(ReadColumn(reader, "DEPT_LONG_DESC"))
            .BatchStatus = CStr(ReadColumn(reader, "STATUS"))
        End With

        Return BatchInf
    End Function
End Class
