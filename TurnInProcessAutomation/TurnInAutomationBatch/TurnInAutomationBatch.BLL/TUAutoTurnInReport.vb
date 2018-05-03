Imports TurnInAutomationBatch.BusinessEntities
Imports TurnInAutomationBatch.MainframeDAL

Public Class TUAutoTurnInReport

    Private dalDB2 As MainframeDAL.AutoTurnInReportDOA = New MainframeDAL.AutoTurnInReportDOA

    Public Function GetAutoTurnInReport(ByVal POStartShipDate As Date) As IList(Of AutoTurnInReportInfo)
        Try
            Dim DB2Results As IList(Of AutoTurnInReportInfo) = dalDB2.GetAutoTurnInReportInfo(POStartShipDate)
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
