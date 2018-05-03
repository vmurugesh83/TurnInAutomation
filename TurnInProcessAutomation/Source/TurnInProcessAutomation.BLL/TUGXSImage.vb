Imports TurnInProcessAutomation.SqlDAL
Imports TurnInProcessAutomation.BusinessEntities

Public Class TUGXSImage

    Private dalDB2 As MainframeDAL.GXSImageDao = New MainframeDAL.GXSImageDao
    Private dalSQL As SqlDAL.GXSImageDao = New SqlDAL.GXSImageDao

    Public Function GetImageData(ByVal upc As Decimal) As IList(Of GXSImageInfo)
        Try
            Dim ImageList As New List(Of GXSImageInfo)
            ImageList.AddRange(dalDB2.GetImageData(upc))
            ImageList.AddRange(dalSQL.GetImageData(upc))
            Return ImageList
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub New()

    End Sub

End Class
