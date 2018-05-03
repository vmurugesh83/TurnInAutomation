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

Partial Public Class TMS900PARAMETERDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all TMS900PARAMETERInfo records.
    ''' </summary>	    	 
    Public Function GetAllFromTMS900PARAMETER(ByVal CodeType As String, ByVal DeptId As Integer) As IList(Of TMS900PARAMETERInfo)
        Dim TMS900PARAMETERInfos As New List(Of TMS900PARAMETERInfo)

        Dim sql As String = _spSchema + ".TU1015SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@CodeType", DB2Type.VarChar), _
                                                         New DB2Parameter("@DeptID", DB2Type.SmallInt)}
        parms(0).Value = CodeType
        parms(1).Value = DeptId

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new TMS900PARAMETERInfo object via factory method and add to list
                    TMS900PARAMETERInfos.Add(TMS900PARAMETERFactory.Construct(rdr))
                End While
            End Using
            Return TMS900PARAMETERInfos
        Catch ex As Exception
            Throw
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to get all TMS900PARAMETERInfo records.
    ''' </summary>	    	 
    Public Function GetAllFromTMS900PARAMETERByIntColumns(ByVal CodeType As String, ByVal DeptId As Integer, ByVal ClassID As Integer) As IList(Of TMS900PARAMETERInfo)
        Dim TMS900PARAMETERInfos As New List(Of TMS900PARAMETERInfo)

        Dim sql As String = _spSchema + ".TU1193SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@CodeType", DB2Type.VarChar), _
                                                         New DB2Parameter("@DeptID", DB2Type.SmallInt), _
                                                          New DB2Parameter("@ClassID", DB2Type.SmallInt)}
        parms(0).Value = CodeType
        parms(1).Value = DeptId
        parms(2).Value = ClassID

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new TMS900PARAMETERInfo object via factory method and add to list
                    TMS900PARAMETERInfos.Add(TMS900PARAMETERFactory.Construct(rdr))
                End While
            End Using
            Return TMS900PARAMETERInfos
        Catch ex As Exception
            Throw
        End Try
        Return Nothing
    End Function
End Class

