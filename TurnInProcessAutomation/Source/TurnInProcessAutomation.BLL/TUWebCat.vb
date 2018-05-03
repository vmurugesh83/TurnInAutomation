Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TUWebCat
    Private dal As MainframeDAL.WebCatDao = New MainframeDAL.WebCatDao

    Public Function GetWebCatByISN(ByVal ISN As Decimal) As IList(Of WebCat)
        Try
            Return dal.GetWebCatByISN(ISN)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetAllWebCat() As IList(Of WebCat)
        Try
            Return dal.GetAllWebCat()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetFeatureWebCat() As IList(Of WebCat)
        Try
            Return dal.GetFeatureWebCat()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetWebCatByParentCde(ByVal parentCode As Integer) As IList(Of WebCat)
        Try
            Return dal.GetWebCatByParentCde(parentCode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetProductAndUPCDetailsByProductCode(ByVal productCode As Integer) As IList(Of WebcatProductInfo)
        Try
            Return dal.GetProductAndUPCDetailsByProductCode(productCode)
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
