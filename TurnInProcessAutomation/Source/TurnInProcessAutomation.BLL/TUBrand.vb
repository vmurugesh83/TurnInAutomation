Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities

Public Class TUBrand
    Private dal As MainframeDAL.BrandDao = New MainframeDAL.BrandDao

    Public Function GetAllBrands() As IList(Of BrandInfo)
        Try
            Return dal.GetAllBrands()
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
