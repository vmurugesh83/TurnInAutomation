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

Partial Public Class ClassDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

     ''' <summary>
     ''' Method to get all ClassInfo records.
     ''' </summary>	    	 
    Public Function GetAllFromClassByDepartment(ByVal DeptId As Integer) As IList(Of ClassInfo)
        Dim ClassInfos As New List(Of ClassInfo)

        Dim sql As String = _spSchema + ".TU1002SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@DEPT_ID", DB2Type.Integer)}
        parms(0).Value = DeptId

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new ClassInfo object via factory method and add to list
                    ClassInfos.Add(ClassFactory.ConstructBasic(rdr))
                End While
            End Using
            Return ClassInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

End Class

