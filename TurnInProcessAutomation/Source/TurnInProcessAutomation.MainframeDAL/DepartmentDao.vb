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

Partial Public Class DepartmentDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all DepartmentInfo records.
    ''' </summary>	    	 
    Public Function GetAllFromDepartment() As IList(Of DepartmentInfo)
        Dim departmentInfos As New List(Of DepartmentInfo)

        Dim sql As String = _spSchema + ".TU1001SP"
        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, Nothing)
                While (rdr.Read())
                    'instantiate new DepartmentInfo object via factory method and add to list
                    departmentInfos.Add(DepartmentFactory.ConstructBasic(rdr))
                End While
            End Using
            Return departmentInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to get all DepartmentInfo records by GMM.
    ''' </summary>	    	 
    Public Function GetAllDepartmentbyGMM(ByVal GMMID As Integer) As IList(Of DepartmentInfo)
        Dim departmentInfos As New List(Of DepartmentInfo)

        Dim sql As String = _spSchema + ".TU1195SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@GMMID", DB2Type.Integer)}

        parms(0).Value = GMMID

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new DepartmentInfo object via factory method and add to list
                    departmentInfos.Add(DepartmentFactory.ConstructBasic(rdr))
                End While
            End Using
            Return departmentInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
End Class

