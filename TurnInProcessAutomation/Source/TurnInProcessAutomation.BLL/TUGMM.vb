Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities

Public Class TUGMM
    Private dal As MainframeDAL.GMMDao = New MainframeDAL.GMMDao

    Public Function GetAllGMM() As IList(Of GMMInfo)
        Try
            Return dal.GetAllFromGMM()
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class