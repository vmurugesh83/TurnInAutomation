Imports BonTon.DBUtility
Imports BonTon.DBUtility.DB2Helper
Imports IBM.Data.DB2
Imports System.Configuration
Imports TurnInAutomationBatch.BusinessEntities

Public Class CommonDAO
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")

    Public Sub WriteToLogTable(ByVal LogDetails As LogDetail)
        Dim imageRequestID As Integer = 0
        Dim sql As String = _spSchema + ".TU1155SP"

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MDSE_TRNIN_BTCH_ID", DB2Type.Integer), _
                                                  New DB2Parameter("@TURN_IN_ITEM_ID", DB2Type.Integer), _
                                                  New DB2Parameter("@TURN_IN_ITEM_TYP", DB2Type.VarChar, 50), _
                                                  New DB2Parameter("@AUTO_BTCH_STAT_CDE", DB2Type.Char, 1), _
                                                  New DB2Parameter("@AUTO_BTCH_STAT_TXT", DB2Type.VarChar, 4000), _
                                                  New DB2Parameter("@LAST_MOD_ID", DB2Type.VarChar, 30)}


        parms(0).Value = LogDetails.TurnInBatchID
        parms(1).Value = LogDetails.TurnInItemTypeID
        parms(2).Value = LogDetails.TurnInItemType
        parms(3).Value = LogDetails.BatchStatusCode
        parms(4).Value = LogDetails.BatchStatusMessage
        parms(5).Value = LogDetails.LastModifiedBy

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function GetTIAEnabledDepartments(ByVal ConfigurationKey As String) As List(Of DeptPageNumber)
        Dim departments As List(Of DeptPageNumber) = Nothing
        Dim sql As String = String.Empty
        Dim parms As DB2Parameter() = Nothing
        Dim department As DeptPageNumber = Nothing
        Try
            sql = _spSchema + ".TU1164SP"
            parms = New DB2Parameter() {New DB2Parameter("@ConfigurationKey", DB2Type.VarChar, 25)}

            parms(0).Value = ConfigurationKey

            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                departments = New List(Of DeptPageNumber)()
                While rdr.Read()
                    If rdr.HasRows Then
                        department = New DeptPageNumber()
                        department.DeptID = CInt(rdr("DEPT_ID"))
                        department.PageNumber = CInt(rdr("PAGE_NO"))
                        department.SequenceNumber = CInt(rdr("SEQ_NUM"))
                        departments.Add(department)
                    End If
                End While
            End Using

        Catch ex As Exception
            Throw ex
        End Try
        Return departments
    End Function
End Class
