Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class EMMFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As EMMInfo
        Dim EMMInformation As New EMMInfo()

        With EMMInformation
            .EMMId = CShort(ReadColumn(reader, "EMM_ID"))
            .EMMDesc = CStr(ReadColumn(reader, "EMM_DESC"))
        End With

        Return EMMInformation
    End Function
End Class
