Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities

Public Class SubClassFactory

    Public Shared Function ConstructBasic(ByVal reader As DB2DataReader) As SubClassInfo
        Dim SubClassInfo As New SubClassInfo()

        Dim col0 As Integer = reader.GetOrdinal("CLASS_ID")
        If Not reader.Item(col0) Is Nothing Then
            SubClassInfo.DeptId = CInt(reader.GetValue(col0))
        End If

        Dim col1 As Integer = reader.GetOrdinal("DEPT_ID")
        If Not reader.Item(col1) Is Nothing Then
            SubClassInfo.DeptId = CInt(reader.GetValue(col1))
        End If

        Dim col2 As Integer = reader.GetOrdinal("SUBCLASS_ID")
        If Not reader.Item(col2) Is Nothing Then
            SubClassInfo.SubClassId = CInt(reader.GetValue(col2))
        End If

        Dim col3 As Integer = reader.GetOrdinal("SUBCLA_SHORT_DESC")
        If Not reader.Item(col3) Is Nothing Then
            SubClassInfo.SubClassShortDesc = CStr(reader.GetValue(col3))
        End If

        Return SubClassInfo
    End Function
End Class

