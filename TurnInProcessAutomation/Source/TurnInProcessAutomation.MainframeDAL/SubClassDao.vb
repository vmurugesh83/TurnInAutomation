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

Partial Public Class SubClassDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all SubClassInfo records.
    ''' </summary>	    	 
    Public Function GetAllFromSubClassByDeptClass(ByVal deptId As Integer, ByVal classID As Integer) As IList(Of SubClassInfo)
        Dim SubClassInfos As New List(Of SubClassInfo)

        Dim sql As String = _spSchema + ".TU1003SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@DEPT_ID", DB2Type.Integer), New DB2Parameter("@CLASS_ID", DB2Type.Integer)}
        parms(0).Value = deptId
        parms(1).Value = classID

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new SubClassInfo object via factory method and add to list
                    SubClassInfos.Add(SubClassFactory.ConstructBasic(rdr))
                End While
            End Using
            Return SubClassInfos
        Catch ex As Exception
            Throw
        End Try
        Return Nothing
    End Function

End Class

