Imports TurnInAutomationBatch.BusinessEntities
Imports TurnInAutomationBatch.MainframeDAL

Public Class TUImageGroupingReport

    Private dalDB2 As MainframeDAL.ImageGroupingReportDOA = New MainframeDAL.ImageGroupingReportDOA

    Public Function GetImageGroupingReport() As IList(Of ImageGroupingReportInfo)
        Try
            Dim DB2Results As IList(Of ImageGroupingReportInfo) = dalDB2.GetImageGroupingReportInfo()
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
