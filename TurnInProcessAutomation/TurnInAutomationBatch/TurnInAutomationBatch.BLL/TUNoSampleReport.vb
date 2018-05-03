Imports TurnInAutomationBatch.BusinessEntities
Imports TurnInAutomationBatch.MainframeDAL

Public Class TUNoSampleReport

    Private dalDB2 As MainframeDAL.NoSampleReportDao = New MainframeDAL.NoSampleReportDao

    Public Function GetNoSampleReport(ByVal POStartShipDate As Date) As IList(Of NoSampleReportInfo)
        Try
            Dim DB2Results As IList(Of NoSampleReportInfo) = dalDB2.GetNoSampleReportInfo(POStartShipDate)
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
