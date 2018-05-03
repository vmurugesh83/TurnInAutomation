Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class ClrSizLocLookupFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As ClrSizLocLookUp
        Dim ClrSizLocLookUpInfo As New ClrSizLocLookUp()

        With ClrSizLocLookUpInfo
            .Value = CStr(ReadColumn(reader, "VALFIELD"))
            .Text = CStr(ReadColumn(reader, "TXTFIELD")).Trim
            .Identifier = CStr(ReadColumn(reader, "IDENTIFIER"))
        End With

        Return ClrSizLocLookUpInfo
    End Function
End Class
