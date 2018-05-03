Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities

Public Class TUEMM
    Private dal As MainframeDAL.EMMDao = New MainframeDAL.EMMDao

    Public Function GetAllFromBuyer() As IList(Of EMMInfo)
        Try
            Return dal.GetAllFromEMM()
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
