Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TUImage
    Private dal As MainframeDAL.ImageDao = New MainframeDAL.ImageDao

    Public Function GetAllImages(ByVal SelectedStatus As String, ByVal FilterText As String) As IList(Of ImageInfo)
        Try
            Return dal.GetAllImages(SelectedStatus, FilterText)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetRendersSwatchesByFeatureImageID(ByVal FeatureImageID As Integer) As IList(Of CopyPrioritizationImageInfo)
        Try
            Return dal.GetRendersSwatchesByFeatureImageID(FeatureImageID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
