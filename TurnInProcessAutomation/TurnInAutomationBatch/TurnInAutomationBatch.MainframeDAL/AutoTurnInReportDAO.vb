Imports System.Configuration
Imports BonTon.DBUtility
Imports BonTon.DBUtility.DB2Helper
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.Factory
Imports TurnInAutomationBatch.BusinessEntities

Public Class AutoTurnInReportDOA

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")

    Public Function GetAutoTurnInReportInfo(ByVal POStartShipDate As Date) As IList(Of AutoTurnInReportInfo)
        Dim autoTurnInReportInfoInfos As New List(Of AutoTurnInReportInfo)
        Dim autoTurnInReportInfoInfo As AutoTurnInReportInfo
        Dim sql As String = _spSchema + ".TU1162SP"

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@POStartShipDate", DB2Type.Date)}

        parms(0).Value = POStartShipDate

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    autoTurnInReportInfoInfo = New AutoTurnInReportInfo()
                    With autoTurnInReportInfoInfo
                        If Common.HasColumn(rdr, "INTERNAL_STYLE_NUM") Then
                            .ISN = CDec(rdr("INTERNAL_STYLE_NUM"))
                        End If

                        If Common.HasColumn(rdr, "ISN_LONG_DESC") Then
                            .ISNDesc = CStr(rdr("ISN_LONG_DESC")).Trim()
                        End If

                        If Common.HasColumn(rdr, "VENDOR_STYLE_NUM") Then
                            .VendorStyleNumber = CStr(rdr("VENDOR_STYLE_NUM")).Trim()
                        End If

                        If Common.HasColumn(rdr, "DEPT_ID") Then
                            .DeptId = CInt(rdr("DEPT_ID"))
                        End If

                        If Common.HasColumn(rdr, "DEPT_SHORT_DESC") Then
                            .Dept_Short_Desc = CStr(rdr("DEPT_SHORT_DESC"))
                        End If

                        If Common.HasColumn(rdr, "CLR_CDE") Then
                            .ColorCode = CInt(rdr("CLR_CDE"))
                        End If

                        If Common.HasColumn(rdr, "CLR_LONG_DESC") Then
                            .ColorDesc = CStr(rdr("CLR_LONG_DESC"))
                        End If

                        If Common.HasColumn(rdr, "TURNIN_FABRIC_DESC") Then
                            .Fabrication = CStr(rdr("TURNIN_FABRIC_DESC"))
                        End If

                        If Common.HasColumn(rdr, "ORIGINATION") Then
                            .Origination = CStr(rdr("ORIGINATION"))
                        End If

                        If Common.HasColumn(rdr, "ON_ORDER") Then
                            .OnOrder = CStr(rdr("ON_ORDER"))
                        End If

                        If Common.HasColumn(rdr, "START_SHIP_DATE") Then
                            .StartShipDate = CStr(rdr("START_SHIP_DATE"))
                        End If
                    End With
                    autoTurnInReportInfoInfos.Add(autoTurnInReportInfoInfo)
                End While
            End Using
            Return autoTurnInReportInfoInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return autoTurnInReportInfoInfos
    End Function

End Class
