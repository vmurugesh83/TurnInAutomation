Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TUDepartment
    Private dal As MainframeDAL.DepartmentDao = New MainframeDAL.DepartmentDao

    Public Function GetAllFromDepartment() As IList(Of DepartmentInfo)
        Try
            Return dal.GetAllFromDepartment()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Method to get all DepartmentInfo records by GMM.
    ''' </summary>	    	 
    Public Function GetAllDepartmentbyGMM(ByVal GMMID As Integer) As IList(Of DepartmentInfo)
        Try
            Return dal.GetAllDepartmentbyGMM(GMMID)
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
