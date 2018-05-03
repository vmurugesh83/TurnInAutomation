Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TUCRG
    Private dal As MainframeDAL.CRGDao = New MainframeDAL.CRGDao

    Public Function GetAllFromCRG() As IList(Of CRGInfo)
        Try
            Return dal.GetAllCRG()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class


