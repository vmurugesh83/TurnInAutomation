
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common
Public Class CFGFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As CFGInfo
        Dim CFGInfo As New CFGInfo()
        'CFG_ID, CMG_ID, DATE_MAINT_TS,  CFG_DESC, CFG_SHORT_DESC
        With CFGInfo
            .DTE_MNT_TS = CStr(ReadColumn(reader, "DATE_MAINT_TS"))
            .CFG_ID = CInt(ReadColumn(reader, "CFG_ID"))
            .CMG_ID = CInt(ReadColumn(reader, "CMG_ID"))
            .CFG_DESC = CStr(ReadColumn(reader, "CFG_DESC"))
            .CFG_SHRT_DSC = CStr(ReadColumn(reader, "CFG_SHORT_DESC"))
        End With
        Return CFGInfo
    End Function
End Class
