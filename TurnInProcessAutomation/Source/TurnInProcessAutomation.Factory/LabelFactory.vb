Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class LabelFactory

    Public Shared Function Construct(ByVal reader As DB2DataReader) As LabelInfo
        Dim LabelInfo As New LabelInfo()

        With LabelInfo
            .LabelId = CInt(ReadColumn(reader, "LABEL_ID"))
            .LabelDesc = CStr(ReadColumn(reader, "LABEL_LONG_DESC"))
        End With

        Return LabelInfo
    End Function
End Class

