Imports System.Configuration
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Text
Imports log4net
Imports BonTon.DBUtility
Imports BonTon.DBUtility.DB2Helper
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory

Public Class DCTransferReportDAO

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")
    Private _connString As String = ConfigurationManager.ConnectionStrings("DB2Connect").ToString
    Private SP_getSampleRequestData As String = ".TU1125SP"

    Public Sub InsertDCTransferRecord(ByVal ISN As Decimal, _
                                      ByVal ColorCode As Integer, _
                                      ByVal UPC As Decimal, _
                                      ByVal FromLocation As Integer, _
                                      ByVal ToLocation As Integer, _
                                      ByVal isTransferred As String, _
                                      ByVal Comments As String, _
                                      ByVal UserId As String, _
                                      ByVal TransferQty As Integer)

        Dim sql As String = _spSchema + ".TU1115SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal), _
                                                          New DB2Parameter("@ColorCode", DB2Type.Integer), _
                                                          New DB2Parameter("@UPC", DB2Type.Decimal), _
                                                          New DB2Parameter("@FROM_LOC", DB2Type.SmallInt), _
                                                          New DB2Parameter("@TO_LOC", DB2Type.SmallInt), _
                                                          New DB2Parameter("@ISTRANSFERRED", DB2Type.Char), _
                                                          New DB2Parameter("@COMMENTS", DB2Type.VarChar), _
                                                          New DB2Parameter("@USER", DB2Type.VarChar), _
                                                          New DB2Parameter("@TRFR_QTY", DB2Type.SmallInt)}

        parms(0).Value = ISN
        parms(1).Value = ColorCode
        parms(2).Value = UPC
        parms(3).Value = FromLocation
        parms(4).Value = ToLocation
        parms(5).Value = isTransferred
        parms(6).Value = Comments
        parms(7).Value = UserId
        parms(8).Value = TransferQty

        ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)

    End Sub

    Public Sub UpdateDCTransferRecordAfterSubmit(ByVal ISN As Decimal, _
                                      ByVal ColorCode As Integer, _
                                      ByVal UserId As String)

        Dim sql As String = _spSchema + ".TU1124SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal), _
                                                          New DB2Parameter("@ColorCode", DB2Type.Integer), _
                                                          New DB2Parameter("@USER", DB2Type.VarChar)}

        parms(0).Value = ISN
        parms(1).Value = ColorCode
        parms(2).Value = UserId

        ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)

    End Sub

    Public Function GetData(ByVal DepartmentID As String, ByVal BuyerID As String, ByVal VendorID As String, ByVal PriceStatusCodes As String) As IList(Of DCTransferReportInfo)
        Dim DCTransferReportInfos As New List(Of DCTransferReportInfo)

        Dim sql As String = _spSchema + ".TU1113SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@DepartmentID", DB2Type.Integer),
                                                          New DB2Parameter("@BuyerID", DB2Type.Integer),
                                                          New DB2Parameter("@VendorID", DB2Type.Integer),
                                                          New DB2Parameter("@StatusCodes", DB2Type.VarChar),
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar)}

        parms(0).Value = IIf(DepartmentID = "0", DBNull.Value, DepartmentID)
        parms(1).Value = IIf(BuyerID = "0", DBNull.Value, BuyerID)
        parms(2).Value = IIf(VendorID = "0", DBNull.Value, VendorID)
        parms(3).Value = PriceStatusCodes
        parms(4).Value = _dbSchema

        Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            While (rdr.Read())
                DCTransferReportInfos.Add(DCTransferReportFactory.Construct(rdr))
            End While
        End Using
        Return DCTransferReportInfos
    End Function

    Public Function ReadSampleRequestData(ByVal UPCList As String) As List(Of MerchandiseSample)
        Dim SampleRequests As New List(Of MerchandiseSample)

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
                cmd.CommandText = _spSchema & SP_getSampleRequestData

                Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@UPC_LIST", DB2Type.VarChar)}
                parms(0).Value = UPCList

                My.Application.Log.WriteEntry(DateTime.Now.ToString() & " - Call " & SP_getSampleRequestData & " with @UPC_LIST = " & UPCList, TraceEventType.Information)

                cmd.Parameters.AddRange(parms)

                Using rdr As DB2DataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    While (rdr.Read())
                        SampleRequests.Add(MerchandiseFactory.Construct(rdr))
                    End While
                End Using
                cmd.Parameters.Clear()
            End Using
        End Using

        Return SampleRequests

    End Function
End Class
