Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities

Public Class ACodeFactory

    Public Shared Function ConstructBasic(ByVal reader As DB2DataReader) As ACodeInfo
        Dim ACodeInfo As New ACodeInfo()

        Dim col1 As Integer = reader.GetOrdinal("A_CD")
        If Not reader.Item(col1) Is Nothing Then
            ACodeInfo.ACode = CStr(reader.GetValue(col1))
        End If

        Dim col2 As Integer = reader.GetOrdinal("A_CD_DESC")
        If Not reader.Item(col2) Is Nothing Then
            ACodeInfo.ACodeDesc = CStr(reader.GetValue(col2))
        End If

        Return ACodeInfo
    End Function
End Class

