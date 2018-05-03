
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common
Public Class CRGFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As CRGInfo
        Dim CRGInfo As New CRGInfo()

        With CRGInfo
            .DTE_MNT_TS = CStr(ReadColumn(reader, "DATE_MAINT_TS"))
            .CRG_ID = CInt(ReadColumn(reader, "CRG_ID"))
            .CRG_DSC = CStr(ReadColumn(reader, "CRG_DESC"))
            .CRG_SHRT_DSC = CStr(ReadColumn(reader, "CRG_SHORT_DESC"))
        End With
        Return CRGInfo
    End Function


End Class
