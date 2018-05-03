Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TUCMG
    Private dal As MainframeDAL.CMGDao = New MainframeDAL.CMGDao

    Public Function GetAllFromCMG() As IList(Of CMGInfo)
        Try
            Return dal.GetAllCMG()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class




