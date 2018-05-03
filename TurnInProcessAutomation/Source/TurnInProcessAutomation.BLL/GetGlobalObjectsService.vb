Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities

Public Class GetGlobalObjectsService

    Private dal As MainframeDAL.GlobalObjectDao = New MainframeDAL.GlobalObjectDao

    Public Sub GetAllApplicationObjects()
        Try
            dal.GetAllApplicationObjects()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class
