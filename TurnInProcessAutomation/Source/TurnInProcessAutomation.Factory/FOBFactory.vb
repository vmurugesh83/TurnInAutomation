Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common
Public Class FOBFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As FOBInfo
        Dim FOBInfo As New FOBInfo()
        With FOBInfo
            .DTE_MNT_TS = CStr(ReadColumn(reader, "DATE_MAINT_TS"))
            .FOB_ID = CInt(ReadColumn(reader, "FOB_ID"))
            .CFG_ID = CInt(ReadColumn(reader, "CFG_ID"))
            .FOB_DESC = CStr(ReadColumn(reader, "FOB_DESC"))
            .FOB_SHRT_DSC = CStr(ReadColumn(reader, "FOB_SHORT_DESC"))
        End With
        Return FOBInfo
    End Function


End Class

