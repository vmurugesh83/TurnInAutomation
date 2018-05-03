Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common
Public Class GMMFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As GMMInfo
        Dim GMMInformation As New GMMInfo()

        With GMMInformation
            .GMMId = CShort(ReadColumn(reader, "GMM_ID"))
            .GMMDesc = CStr(ReadColumn(reader, "GMM_NME"))
        End With

        Return GMMInformation
    End Function
End Class
