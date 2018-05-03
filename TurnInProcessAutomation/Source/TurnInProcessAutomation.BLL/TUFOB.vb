Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TUFOB
    Private dal As MainframeDAL.FOBDao = New MainframeDAL.FOBDao

    Public Function GetAllFOB() As IList(Of FOBInfo)
        Try
            Return dal.GetAllFOB()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
