Imports System.Configuration
Imports BonTon.DBUtility
Imports BonTon.DBUtility.DB2Helper
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.Factory
Imports TurnInAutomationBatch.BusinessEntities

Public Class NoFinalImageReportDAO

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")

    Public Function GetNoFinalImageReportInfo(ByVal POStartShipDate As Date) As IList(Of NoFinalImageReportInfo)
        Dim reportWeekly1Infos As New List(Of NoFinalImageReportInfo)
        Dim reportWeekly1Info As NoFinalImageReportInfo
        Dim sql As String = _spSchema + ".TU1163SP"

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@POStartShipDate", DB2Type.Date)}

        parms(0).Value = POStartShipDate

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    reportWeekly1Info = New NoFinalImageReportInfo()
                    With reportWeekly1Info

                        If Common.HasColumn(rdr, "BUYER_ID") Then
                            .BuyerID = CStr(rdr("BUYER_ID"))
                        End If

                        If Common.HasColumn(rdr, "BUYER_NAME") Then
                            .BuyerDesc = CStr(rdr("BUYER_NAME"))
                        End If

                        If Common.HasColumn(rdr, "VENDOR_ID") Then
                            .VendorID = CInt(rdr("VENDOR_ID"))
                        End If

                        If Common.HasColumn(rdr, "VENDOR_NME") Then
                            .VendorName = CStr(rdr("VENDOR_NME"))
                        End If


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

                        If Common.HasColumn(rdr, "TURN_IN_MDSE_ID") Then
                            .TurnInMerchId = CStr(rdr("TURN_IN_MDSE_ID"))
                        End If

                        If Common.HasColumn(rdr, "ADMIN_IMAGE_NUM") Then
                            .ImageID = CInt(rdr("ADMIN_IMAGE_NUM"))
                        End If

                        If Common.HasColumn(rdr, "ON_ORDER") Then
                            .OnOrder = CStr(rdr("ON_ORDER"))
                        End If

                        If Common.HasColumn(rdr, "START_SHIP_DATE") Then
                            .StartShipDate = CStr(rdr("START_SHIP_DATE"))
                        End If

                        If Common.HasColumn(rdr, "AD_NUM") Then
                            .AdNumber = CInt(rdr("AD_NUM"))
                        End If

                        If Common.HasColumn(rdr, "AD_SYSTEM_PAGE_NUM") Then
                            .PageNumber = CInt(rdr("AD_SYSTEM_PAGE_NUM"))
                        End If

                    End With
                    reportWeekly1Infos.Add(reportWeekly1Info)
                End While
            End Using
            Return reportWeekly1Infos
        Catch ex As Exception
            Throw ex
        End Try
        Return reportWeekly1Infos
    End Function

End Class
