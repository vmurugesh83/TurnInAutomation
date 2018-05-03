Imports TurnInProcessAutomation.SqlDAL
Imports TurnInProcessAutomation.BusinessEntities

Public Class TUGXSCopyView

    Private dalDB2 As MainframeDAL.GXSCopyViewDao = New MainframeDAL.GXSCopyViewDao
    
    Public Function FindByHierarchy(ByVal DeptId As Int16, ByVal ClassId As Int16, ByVal VendorId As Integer, ByVal VendorStyle As String) As IList(Of GXSCopyViewInfo)
        Try
            Return dalDB2.FindByHierarchy(DeptId, ClassId, VendorId, VendorStyle)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function FindByItems(ByVal isnXml As String, ByVal skuUpcXml As String) As IList(Of GXSCopyViewInfo)
        Try
            Return dalDB2.FindByItems(isnXml, skuUpcXml)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function FindByAds(ByVal adNum As Integer, ByVal pageNum As String, ByVal batchNum As Integer) As IList(Of GXSCopyViewInfo)
        Try
            Return dalDB2.FindByAds(adNum, pageNum, batchNum)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function FindByLabel(ByVal labelId As Integer) As IList(Of GXSCopyViewInfo)
        Try
            Return dalDB2.FindByLabel(labelId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function FindByStartShipDate(ByVal startShipDate As Date, endShipDate As Date) As IList(Of GXSCopyViewInfo)
        Try
            Return dalDB2.FindByStartShipDate(startShipDate, endShipDate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub New()

    End Sub
End Class
