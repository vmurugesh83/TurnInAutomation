Imports System.Text
Imports log4net
Imports BonTon.DBUtility
Imports BonTon.DBUtility.DB2Helper
Imports System.Data.OleDb
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory
Imports System.Configuration
Imports IBM.Data.DB2

Public Class BatchDao
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _db2Schema As String = ConfigurationManager.AppSettings("DB2Schema")

    ''' <summary>
    ''' Method to get all Batch IDs.
    ''' </summary>	    	 
    Public Function GetAllBatches(Status As String) As IList(Of BatchInfo)
        Dim BatchInfos As New List(Of BatchInfo)

        Dim sql As String = _spSchema + ".TU1074SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@STATUS", OleDbType.Char), _
                                                          New DB2Parameter("@SCHEMA", OleDbType.VarChar)}
        parms(0).Value = Status
        parms(1).Value = _db2Schema

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new LabelInfo object via factory method and add to list
                    BatchInfos.Add(BatchFactory.Construct(rdr))
                End While
            End Using
            Return BatchInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Function GetBatches(ByVal AdNumber As String, ByVal PageNumber As String, ByVal BuyerId As String, _
                        ByVal DeptId As String, ByVal LabelID As String, ByVal VendorStyleID As String, _
                        ByVal EMMID As String) As IList(Of BatchInfo)
        Dim BatchInfos As New List(Of BatchInfo)

        Dim sql As String = _spSchema + ".TU1103SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@AdNumber", OleDbType.Integer), _
                                                          New DB2Parameter("@PageNumber", OleDbType.SmallInt), _
                                                          New DB2Parameter("@BuyerID", OleDbType.SmallInt), _
                                                          New DB2Parameter("@DeptID", OleDbType.SmallInt), _
                                                          New DB2Parameter("@LabelID", OleDbType.Integer), _
                                                          New DB2Parameter("@VendorStyleID", OleDbType.Char), _
                                                          New DB2Parameter("@EMM_ID", OleDbType.Integer), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar)}
        If Not String.IsNullOrEmpty(AdNumber) Then
            parms(0).Value = Convert.ToInt32(AdNumber)
        Else
            parms(0).Value = DBNull.Value
        End If

        If Not String.IsNullOrEmpty(PageNumber) Then
            parms(1).Value = Convert.ToInt32(PageNumber)
        Else
            parms(1).Value = DBNull.Value
        End If

        parms(2).Value = IIf(BuyerId = "", DBNull.Value, BuyerId)
        parms(3).Value = IIf(DeptId = "", DBNull.Value, DeptId)
        parms(4).Value = IIf(LabelID = "", DBNull.Value, LabelID)
        parms(5).Value = IIf(VendorStyleID = "", DBNull.Value, VendorStyleID)
        parms(6).Value = IIf(EMMID = "", DBNull.Value, EMMID)
        parms(7).Value = _db2Schema

        Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            While (rdr.Read())
                BatchInfos.Add(BatchFactory.Construct(rdr))
            End While
        End Using
        Return BatchInfos
    End Function

    Public Function GetBatchesByUser(ByVal AdNumber As Integer, ByVal PageNumber As Integer, ByVal UserId As String) As Boolean
        Dim HasResults As Boolean = False

        Dim sql As String = _spSchema + ".TU1072SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@AD_NUMBER", OleDbType.Integer), _
                                                          New DB2Parameter("@PAGE_NUMBER", OleDbType.Integer), _
                                                          New DB2Parameter("@USERID", OleDbType.VarChar)}

        parms(0).Value = AdNumber
        parms(1).Value = PageNumber
        parms(2).Value = UserId

        Try
            Dim rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            If rdr.HasRows Then
                HasResults = True
            End If
            Return HasResults
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Public Function GetMaintenanceBatches(ByVal AdNumber As Integer, ByVal PageNumber As Integer, ByVal BuyerId As String, ByVal DeptId As String, ByVal BatchId As String) As List(Of BatchInfo)
        Dim Batches As New List(Of BatchInfo)
        Dim Departments As New List(Of BatchInfo)
        Dim Buyers As New List(Of BatchInfo)
        Dim sql As String = _spSchema + ".TU1020SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@AdNumber", OleDbType.Integer), _
                                                          New DB2Parameter("@PageNumber", OleDbType.Integer), _
                                                          New DB2Parameter("@BuyerId", OleDbType.Integer), _
                                                          New DB2Parameter("@DeptId", OleDbType.Integer), _
                                                          New DB2Parameter("@BatchId", OleDbType.Integer)
                                                         }

        parms(0).Value = IIf(AdNumber = 0, DBNull.Value, AdNumber)
        parms(1).Value = IIf(PageNumber = 0, DBNull.Value, PageNumber)
        parms(2).Value = IIf(BuyerId = "", DBNull.Value, BuyerId)
        parms(3).Value = IIf(DeptId = "", DBNull.Value, DeptId)
        parms(4).Value = IIf(BatchId = "", DBNull.Value, BatchId)

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    Departments.Add(BatchFactory.Construct(rdr))
                End While

                rdr.NextResult()
                While (rdr.Read())
                    Buyers.Add(BatchFactory.Construct(rdr))
                End While

                rdr.NextResult()
                While (rdr.Read())
                    Dim batch As BatchInfo = BatchFactory.Construct(rdr)
                    batch.Departments = String.Join("<br />", Departments.Where(Function(x) x.BatchId = batch.BatchId).Select(Function(x) x.Departments))
                    batch.Buyer = String.Join("<br />", Buyers.Where(Function(x) x.BatchId = batch.BatchId).Select(Function(x) x.Buyer))
                    Batches.Add(batch)
                End While
            End Using

            Return Batches
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Public Function GetAdsForMeetingOrQuery(StatusCode As String) As IList(Of AdInfoInfo)
        Dim AdinfoInfos As IList(Of AdInfoInfo) = New List(Of AdInfoInfo)()

        Dim sql As String = _spSchema + ".TU1077SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@TRNIN_MDSESTAT_CDE", OleDbType.VarChar)}

        parms(0).Value = StatusCode

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    Dim ai As New AdInfoInfo()
                    ai.adnbr = CInt(rdr("AD_NUM"))
                    AdinfoInfos.Add(ai)
                    ai = Nothing
                End While
            End Using

            Return AdinfoInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Public Function GetAdsForMaintenance() As IList(Of AdInfoInfo)
        Dim AdinfoInfos As IList(Of AdInfoInfo) = New List(Of AdInfoInfo)()

        Dim sql As String = _spSchema + ".TU1073SP"

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, Nothing)
                While (rdr.Read())
                    Dim ai As New AdInfoInfo()
                    ai.adnbr = CInt(rdr("AD_NUM"))
                    AdinfoInfos.Add(ai)
                    ai = Nothing
                End While
            End Using

            Return AdinfoInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Public Function UpdateBatchNumber(ByVal BatchId As Integer, ByVal oldAdNum As Integer, ByVal newAdNum As Integer) As List(Of String)
        Dim sql As String = _spSchema + ".TU1108SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@BATCH_ID", OleDbType.Integer), _
                                                          New DB2Parameter("@oldAdNum", OleDbType.Integer), _
                                                          New DB2Parameter("@newAdNum", OleDbType.Integer)}

        parms(0).Value = BatchId
        parms(1).Value = oldAdNum
        parms(2).Value = newAdNum

        Dim ADMIN_MDSE_NUMs As New List(Of String)
        Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            While (rdr.Read())
                ADMIN_MDSE_NUMs.Add(CStr(rdr("ADMIN_MDSE_NUM")))
            End While
        End Using


        Return ADMIN_MDSE_NUMs
    End Function

    Public Sub UpdateBatchSequence(ByVal BatchId As Integer, ByVal ISN As Decimal, ByVal Sequence As Integer)
        Dim sql As String = _spSchema + ".TU1075SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@BATCH_ID", OleDbType.Integer), _
                                                          New DB2Parameter("@ISN", OleDbType.Decimal), _
                                                          New DB2Parameter("@SEQUENCE", OleDbType.Integer)}

        parms(0).Value = BatchId
        parms(1).Value = ISN
        parms(2).Value = Sequence

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateColorSequence(ByVal MerchId As Integer, ByVal ColorSequence As Integer)
        Dim sql As String = _spSchema + ".TU1096SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", OleDbType.Integer), _
                                                          New DB2Parameter("@SEQUENCE", OleDbType.Integer)}

        parms(0).Value = MerchId
        parms(1).Value = ColorSequence

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class
