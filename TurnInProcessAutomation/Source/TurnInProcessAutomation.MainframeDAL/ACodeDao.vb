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

Partial Public Class ACodeDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all ACodeInfo records.
    ''' </summary>	    	 
    Public Function GetAllFromACodeByDepartment(ByVal DeptId As Integer) As IList(Of ACodeInfo)
        Dim ACodeInfos As New List(Of ACodeInfo)

        Dim sql As String = _spSchema + ".TU1005SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@DEPT_ID", DB2Type.Integer)}
        parms(0).Value = DeptId

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new ACodeInfo object via factory method and add to list
                    ACodeInfos.Add(ACodeFactory.ConstructBasic(rdr))
                End While
            End Using
            Return ACodeInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

End Class

