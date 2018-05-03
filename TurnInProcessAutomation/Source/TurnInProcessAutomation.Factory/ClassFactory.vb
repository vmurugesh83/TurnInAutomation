Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities

Public Class ClassFactory

    Public Shared Function ConstructBasic(ByVal reader As DB2DataReader) As ClassInfo
        Dim ClassInfo As New ClassInfo()

        Dim col1 As Integer = reader.GetOrdinal("CLASS_ID")
        If Not reader.Item(col1) Is Nothing Then
            ClassInfo.ClassId = CInt(reader.GetValue(col1))
        End If

        Dim col2 As Integer = reader.GetOrdinal("CLASS_SHORT_DESC")
        If Not reader.Item(col2) Is Nothing Then
            ClassInfo.ClassShortDesc = CStr(reader.GetValue(col2))
        End If

        Dim col3 As Integer = reader.GetOrdinal("DEPT_ID")
        If Not reader.Item(col3) Is Nothing Then
            ClassInfo.DeptId = CInt(reader.GetValue(col3))
        End If

        Return ClassInfo
    End Function
End Class

