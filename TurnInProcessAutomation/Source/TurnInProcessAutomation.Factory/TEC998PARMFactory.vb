Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class TEC998PARMFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As TEC998PARMInfo
        Dim TEC998ParmeterInfo As New TEC998PARMInfo()

        With TEC998ParmeterInfo
            .ParmValue = CShort(ReadColumn(reader, "ENTRY2_TXT"))
            .ParmText = CStr(ReadColumn(reader, "ENTRY_DESC"))
            .ParmDefault = CShort(ReadColumn(reader, "ENTRY_NUM"))
        End With

        Return TEC998ParmeterInfo
    End Function
End Class
