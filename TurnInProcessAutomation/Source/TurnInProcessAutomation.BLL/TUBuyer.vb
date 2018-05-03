Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TUBuyer
    Private dal As MainframeDAL.BuyerDao = New MainframeDAL.BuyerDao

    Public Function GetAllFromBuyer() As IList(Of BuyerInfo)
        Try
            Return dal.GetAllFromBuyer()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetBuyersForCMG(ByVal strCMGId As String) As IList(Of BuyerInfo)
        Try
            Return dal.GetBuyersForCMG(strCMGId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetBuyersByDMM(ByVal dmmNumber As Integer) As IList(Of BuyerInfo)
        Try
            Return dal.GetBuyersByDMM(dmmNumber)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
