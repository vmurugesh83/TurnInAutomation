Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TULabel
    Private dal As MainframeDAL.LabelDao = New MainframeDAL.LabelDao

    Public Function GetAllLabels() As IList(Of LabelInfo)
        Try
            Return dal.GetLabelsByBrand("")
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetLabelsByBrand(ByVal BrandId As String) As IList(Of LabelInfo)
        Try
            Return dal.GetLabelsByBrand(BrandId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
