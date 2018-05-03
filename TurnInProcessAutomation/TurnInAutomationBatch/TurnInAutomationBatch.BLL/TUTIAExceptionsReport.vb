Imports TurnInAutomationBatch.BusinessEntities
Imports TurnInAutomationBatch.MainframeDAL

Public Class TUTIAExceptionsReport

    Private dalDB2 As MainframeDAL.TIAExceptionsReportDOA = New MainframeDAL.TIAExceptionsReportDOA

    Public Function GetTIAExceptionsReport() As IList(Of TIAExceptionsReportInfo)
        Try
            Dim DB2Results As IList(Of TIAExceptionsReportInfo) = dalDB2.GetTIAExceptionsReportInfo()
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
