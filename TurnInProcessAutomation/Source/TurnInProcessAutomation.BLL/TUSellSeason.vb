Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TUSellSeason
    Private dal As MainframeDAL.SellSeasonDao = New MainframeDAL.SellSeasonDao

    Public Function GetAllFromSellSeason() As IList(Of SellSeasonInfo)
        Try
            Return dal.GetAllFromSellSeason()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
