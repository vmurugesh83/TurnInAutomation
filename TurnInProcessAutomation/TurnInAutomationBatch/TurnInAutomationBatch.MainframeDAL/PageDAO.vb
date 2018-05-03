Imports BonTon.DBUtility
Imports BonTon.DBUtility.DB2Helper
Imports IBM.Data.DB2
Imports System.Configuration
Imports TurnInProcessAutomation.Factory

Public Class PageDAO
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")

    Public Function GetPageNumberConfigByDeptID(ByVal DepartmentID As Integer) As Integer
        Dim pageNumber As Integer = 0
        Dim sql As String = _spSchema + ".TU1160SP"

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@DEPT_ID", DB2Type.Integer)}


        parms(0).Value = DepartmentID

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                If (rdr.HasRows) Then
                    rdr.Read()
                    If Common.HasColumn(rdr, "DECIMAL_NUM") Then
                        pageNumber = CInt(rdr("DECIMAL_NUM"))
                    End If
                End If
            End Using
        Catch ex As Exception
            Throw ex
        End Try

        Return pageNumber
    End Function
End Class
