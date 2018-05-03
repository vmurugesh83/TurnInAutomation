Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class TMS900PARAMETERFactory

    Public Shared Function Construct(ByVal reader As DB2DataReader) As TMS900PARAMETERInfo
        Dim TMS900PARAMETERInfo As New TMS900PARAMETERInfo()

        With TMS900PARAMETERInfo
            .CharIndex = CStr(ReadColumn(reader, "CHAR_INDEX"))
            .ShortDesc = CStr(ReadColumn(reader, "SHORT_DESC")).Trim
            .LongDesc = CStr(ReadColumn(reader, "LONG_DESC")).Trim
            .LIST_SEQ_NUM = CInt(ReadColumn(reader, "LIST_SEQ_NUM"))
        End With

        Return TMS900PARAMETERInfo
    End Function
End Class

