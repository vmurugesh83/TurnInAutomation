Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities

Public Class DepartmentFactory

    Public Shared Function ConstructBasic(ByVal reader As DB2DataReader) As DepartmentInfo
        Dim departmentInfo As New DepartmentInfo()

        Dim col1 As Integer = reader.GetOrdinal("DEPT_ID")
        If Not reader.Item(col1) Is Nothing Then
            departmentInfo.DeptId = CInt(reader.GetValue(col1))
        End If

        Dim col2 As Integer = reader.GetOrdinal("DEPT_SHORT_DESC")
        If Not reader.Item(col2) Is Nothing Then
            departmentInfo.DeptShortDesc = CStr(reader.GetValue(col2))
        End If

        Return departmentInfo
    End Function
End Class

