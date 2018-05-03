Imports TurnInAutomationBatch.BusinessEntities
Imports TurnInAutomationBatch.MainframeDAL

Public Class TUNoFinalImageReport

    Private dalDB2 As MainframeDAL.NoFinalImageReportDAO = New MainframeDAL.NoFinalImageReportDAO

    Public Function GetNoFinalImageReport(ByVal POStartShipDate As Date) As IList(Of NoFinalImageReportInfo)
        Try
            Dim DB2Results As IList(Of NoFinalImageReportInfo) = dalDB2.GetNoFinalImageReportInfo(POStartShipDate)
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
