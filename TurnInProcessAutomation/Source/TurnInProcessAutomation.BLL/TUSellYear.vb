Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TUSellYear
    Private dal As MainframeDAL.SellYearDao = New MainframeDAL.SellYearDao

    Public Function GetAllFromSellYear() As IList(Of SellYearInfo)
        Try
            Return dal.GetAllFromSellYear()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
