Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common
Public Class DMMFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As DMMInfo
        Dim DMMInformation As New DMMInfo()

        With DMMInformation
            .DMMId = CShort(ReadColumn(reader, "DMM_ID"))
            .DMMDesc = CStr(ReadColumn(reader, "DMM_NME"))
        End With

        Return DMMInformation
    End Function
End Class
