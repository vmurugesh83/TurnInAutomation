Imports System.Configuration
Imports BonTon.DBUtility
Imports BonTon.DBUtility.DB2Helper
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.Factory
Imports TurnInAutomationBatch.BusinessEntities

Public Class TIAExceptionsReportDOA

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")

    Public Function GetTIAExceptionsReportInfo() As IList(Of TIAExceptionsReportInfo)
        Dim TIAExceptionsReportInfos As New List(Of TIAExceptionsReportInfo)
        Dim TIAExceptionsReportInfo As TIAExceptionsReportInfo
        Dim sql As String = _spSchema + ".TU1171SP"

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, Nothing)
                While (rdr.Read())
                    TIAExceptionsReportInfo = New TIAExceptionsReportInfo()
                    With TIAExceptionsReportInfo

                        If Common.HasColumn(rdr, "MDSE_TRNIN_BTCH_ID") Then
                            .BatchId = CInt(rdr("MDSE_TRNIN_BTCH_ID"))
                        End If

                        If Common.HasColumn(rdr, "TURN_IN_ITEM_ID") Then
                            .ItemId = CInt(rdr("TURN_IN_ITEM_ID"))
                        End If

                        If Common.HasColumn(rdr, "TURN_IN_ITEM_TYP") Then
                            .ItemType = CStr(rdr("TURN_IN_ITEM_TYP")).Trim
                        End If

                        If Common.HasColumn(rdr, "AUTO_BTCH_STAT_TXT") Then
                            .BatchStatus = CStr(rdr("AUTO_BTCH_STAT_TXT")).Trim
                        End If
                    End With
                    TIAExceptionsReportInfos.Add(TIAExceptionsReportInfo)
                End While
            End Using
            Return TIAExceptionsReportInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return TIAExceptionsReportInfos
    End Function

End Class
