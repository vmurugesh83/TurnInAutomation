Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common
Public Class CMGFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As CMGInfo
        Dim CMGInfo As New CMGInfo()
        With CMGInfo
            .DTE_MNT_TS = CStr(ReadColumn(reader, "DATE_MAINT_TS"))
            .CMG_ID = CInt(ReadColumn(reader, "CMG_ID"))
            .CRG_ID = CInt(ReadColumn(reader, "CRG_ID"))
            .CMG_DESC = CStr(ReadColumn(reader, "CMG_DESC"))
            .CMG_SHRT_DSC = CStr(ReadColumn(reader, "CMG_SHORT_DESC"))
        End With
        Return CMGInfo
    End Function
End Class
