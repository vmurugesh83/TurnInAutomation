Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TUSubClass
    Private dal As MainframeDAL.SubClassDao = New MainframeDAL.SubClassDao

    Public Function GetAllFromSubClassByDeptClass(ByVal deptID As Integer, ByVal classID As Integer) As IList(Of SubClassInfo)
        Try
            Return dal.GetAllFromSubClassByDeptClass(deptID, classID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
