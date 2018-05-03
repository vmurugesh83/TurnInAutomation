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

Partial Public Class TEC100_CATEGORYDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    Public Function GetTEC100_CATEGORYByParentCde(ByVal parentCde As Integer) As IList(Of TEC100_CATEGORYInfo)
        Dim tec100CategoryInfos As IList(Of TEC100_CATEGORYInfo) = New List(Of TEC100_CATEGORYInfo)()
        Dim sql As String = _spSchema + ".TU1016SP"

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@PARENT_CDE", DB2Type.Integer, 4)}
        parms(0).Value = parentCde

        'Execute a query to read the TEC100_CATEGORYInfos
        Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            While (rdr.Read())
                'instantiate new TEC100_CATEGORYInfo object via factory method
                tec100CategoryInfos.Add(TEC100_CATEGORYFactory.Construct(rdr))
            End While
        End Using
        Return tec100CategoryInfos
    End Function
    Public Function GetTEC100_CATEGORY(ByVal ISN As Integer) As IList(Of TEC100_CATEGORYInfo)
        Dim tec100CategoryInfos As IList(Of TEC100_CATEGORYInfo) = New List(Of TEC100_CATEGORYInfo)()
        Dim sql As String = _spSchema + ".TU1042SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Integer, 4)}
        parms(0).Value = ISN

        'Execute a query to read the TEC100_CATEGORYInfos
        Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            While (rdr.Read())
                'instantiate new TEC100_CATEGORYInfo object via factory method
                tec100CategoryInfos.Add(TEC100_CATEGORYFactory.Construct(rdr))
            End While
        End Using
        Return tec100CategoryInfos
    End Function

End Class

