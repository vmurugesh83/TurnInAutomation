Imports System.Configuration
Imports BonTon.DBUtility
Imports BonTon.DBUtility.DB2Helper
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.Factory
Imports TurnInAutomationBatch.BusinessEntities

Public Class ImageGroupingReportDOA

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")

    Public Function GetImageGroupingReportInfo() As IList(Of ImageGroupingReportInfo)
        Dim ImageGroupingReportInfos As New List(Of ImageGroupingReportInfo)
        Dim ImageGroupingReportInfo As ImageGroupingReportInfo
        Dim sql As String = _spSchema + ".TU1169SP"

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, Nothing)
                While (rdr.Read())
                    ImageGroupingReportInfo = New ImageGroupingReportInfo()
                    With ImageGroupingReportInfo
                        If Common.HasColumn(rdr, "CMG_ID") Then
                            .CmgId = CShort(rdr("CMG_ID"))
                        End If

                        If Common.HasColumn(rdr, "IMAGE_MDSE_GRP_NUM") Then
                            .ImageGroup = CInt(rdr("IMAGE_MDSE_GRP_NUM"))
                        End If

                        If Common.HasColumn(rdr, "ADMIN_IMAGE_NUM") Then
                            .ImageID = CInt(rdr("ADMIN_IMAGE_NUM"))
                        End If

                        If Common.HasColumn(rdr, "VENDOR_STYLE_NUM") Then
                            .VendorStyleNumber = CStr(rdr("VENDOR_STYLE_NUM")).Trim
                        End If

                        If Common.HasColumn(rdr, "TURN_IN_MDSE_ID") Then
                            .TurnInMerchId = CInt(rdr("TURN_IN_MDSE_ID"))
                        End If

                        If Common.HasColumn(rdr, "FRNDLY_PRDCT_DESC") Then
                            .ProductDesc = CStr(rdr("FRNDLY_PRDCT_DESC")).Trim
                        End If
                    End With
                    ImageGroupingReportInfos.Add(ImageGroupingReportInfo)
                End While
            End Using
            Return ImageGroupingReportInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return ImageGroupingReportInfos
    End Function

End Class
