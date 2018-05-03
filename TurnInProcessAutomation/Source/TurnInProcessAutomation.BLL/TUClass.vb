Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TUClass
    Private dal As MainframeDAL.ClassDao = New MainframeDAL.ClassDao

    Public Function GetAllFromClassByDepartment(ByVal deptId As Integer) As IList(Of ClassInfo)
        Try
            Return dal.GetAllFromClassByDepartment(deptId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
