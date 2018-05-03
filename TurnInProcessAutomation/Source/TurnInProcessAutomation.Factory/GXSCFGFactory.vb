
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common
Public Class GXSCFGFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As GXSCFGInfo
        Dim GXSCFGInfo As New GXSCFGInfo()
        With GXSCFGInfo
            .INTERNAL_STYLE_NUM = CDec(ReadColumn(reader, "INTERNAL_STYLE_NUM"))
            .DEPT_ID = CInt(ReadColumn(reader, "DEPT_ID"))
            '.FOB_ID = CInt(ReadColumn(reader, "FOB_ID"))
            .CRG_ID = CInt(ReadColumn(reader, "CRG_ID"))
            .CFG_ID = CInt(ReadColumn(reader, "CFG_ID"))
            .CMG_ID = CInt(ReadColumn(reader, "CMG_ID"))
            .ISN_LONG_DESC = CStr(ReadColumn(reader, "ISN_LONG_DESC"))
        End With
        Return GXSCFGInfo
    End Function
End Class
