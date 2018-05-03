Imports TurnInProcessAutomation.SqlDAL
Imports TurnInProcessAutomation.BusinessEntities

Public Class TUImageSuffix
    Private dal As SqlDAL.ImageSuffixDao = New SqlDAL.ImageSuffixDao

    Public Function GetAllFromImageSuffix() As List(Of ImageSuffixInfo)
        Try
            Return dal.GetAllFromImageSuffix.ToList
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
