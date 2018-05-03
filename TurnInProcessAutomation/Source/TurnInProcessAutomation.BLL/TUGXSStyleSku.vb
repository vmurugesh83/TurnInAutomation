Imports TurnInProcessAutomation.SqlDAL
Imports TurnInProcessAutomation.BusinessEntities

Public Class TUGXSStyleSku

    Private dalDB2 As MainframeDAL.GXSStyleSkuDao = New MainframeDAL.GXSStyleSkuDao

    Public Function GetStyleSKUData(ByVal upc As Decimal) As IList(Of GXSStyleSkuInfo)
        Try
            Return dalDB2.GetStyleSKUData(upc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub New()

    End Sub

End Class
