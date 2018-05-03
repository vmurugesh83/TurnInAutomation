Imports IBM.Data.DB2
Imports System.Configuration
Imports TurnInProcessAutomation.BusinessEntities
Imports BonTon.DBUtility
Imports BonTon.DBUtility.DB2Helper


Public Class ApplicationDBLayer

    Private _connString As String = ConfigurationManager.ConnectionStrings("DB2Connect").ToString
    Private _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")

    '   STORED PROCEDURES LISTED
    Private SP_UpdateTransferRecord As String = ".TU1118SP"
    Private SP_getExcelReportData As String = ".TU1117SP"

    Public Sub UpdateTransferRecord(ByVal TransferRecords As String)
        Using conn As New DB2Connection(_connString)
            If conn.State <> ConnectionState.Open Then
                conn.Open()
            End If
            Using cmd As New DB2Command()
                cmd.Connection = conn
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = _spSchema & SP_UpdateTransferRecord

                Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@XFR", DB2Type.VarChar)}
                parms(0).Value = TransferRecords

                My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - Call " & SP_UpdateTransferRecord & " with @XFR = " & TransferRecords, TraceEventType.Information)

                cmd.Parameters.AddRange(parms)
                cmd.ExecuteScalar()
                cmd.Parameters.Clear()
            End Using
        End Using
    End Sub

    Public Sub ReadINDCTransferData(ByRef ExportData As DataSet)
        ExportData.Clear()

        Using conn As New DB2Connection(_connString)
            Dim db2ConnectionString As New DB2ConnectionStringBuilder(_connString)

            My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - Connection -> " & db2ConnectionString.Server & _
                                          " :: DB -> " & db2ConnectionString.Database & _
                                          " :: UID -> " & db2ConnectionString.UserID, TraceEventType.Information)
            If conn.State <> ConnectionState.Open Then
                conn.Open()
            End If
            Using cmd As New DB2Command()
                cmd.Connection = conn
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = _spSchema & SP_getExcelReportData

                My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - Call " & SP_getExcelReportData, TraceEventType.Information)

                Using rdr As DB2DataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    ExportData.Tables(0).Load(rdr)
                End Using
                cmd.Parameters.Clear()
            End Using
        End Using

    End Sub

    Public Sub New()
        My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - SP Schema : " & _spSchema.ToString, TraceEventType.Information)
        My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - DB Schema : " & _dbSchema.ToString, TraceEventType.Information)
    End Sub

    Public Function GetTransferID() As Integer
        Dim transferID As Integer = 0

        Dim sql As String = _spSchema + ".TU1158SP"
        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql)
                If (rdr.HasRows) Then
                    rdr.Read()
                    If Not rdr("TRANSFER_ID") Is Nothing Then
                        transferID = CInt(rdr("TRANSFER_ID"))
                    End If
                End If
            End Using

            Return transferID
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
End Class

