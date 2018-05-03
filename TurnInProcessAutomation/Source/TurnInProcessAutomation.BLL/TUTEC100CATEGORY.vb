Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TUTEC100CATEGORY
    Private dal As MainframeDAL.TEC100_CATEGORYDao = New MainframeDAL.TEC100_CATEGORYDao

    Public Function GetTEC100_CATEGORYByParentCde(ByVal parentCode As Integer) As IList(Of TEC100_CATEGORYInfo)
        Try
            Return dal.GetTEC100_CATEGORYByParentCde(parentCode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetTEC100_CATEGORY(ByVal ISN As Integer) As IList(Of TEC100_CATEGORYInfo)
        Try
            Return dal.GetTEC100_CATEGORY(ISN)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
