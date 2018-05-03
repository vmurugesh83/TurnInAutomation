Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TUACode
    Private dal As MainframeDAL.ACodeDao = New MainframeDAL.ACodeDao

    Public Function GetAllFromACodeByDepartment(ByVal deptId As Integer) As IList(Of ACodeInfo)
        Try
            Return dal.GetAllFromACodeByDepartment(deptId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
