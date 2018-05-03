Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities

Public Class TUDMM
    Private dal As MainframeDAL.DMMDao = New MainframeDAL.DMMDao

    Public Function GetAllDMM() As IList(Of DMMInfo)
        Try
            Return dal.GetAllFromDMM()
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
