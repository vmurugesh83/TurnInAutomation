
Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TUCFG
    Private dal As MainframeDAL.CFGDao = New MainframeDAL.CFGDao

    Public Function GetAllFromCFG() As IList(Of CFGInfo)
        Try
            Return dal.GetAllFromCFG()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class


