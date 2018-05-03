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
Public Class CMGDao
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all CMGInfo records.
    ''' </summary>	    	 
    Public Function GetAllCMG() As IList(Of CMGInfo)
        Dim CMGInfo As New List(Of CMGInfo)

        Dim sql As String = _spSchema + ".TU1050SP"
        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql)
                While (rdr.Read())
                    'instantiate new CMGInfo object via factory method and add to list
                    CMGInfo.Add(CMGFactory.Construct(rdr))
                End While
            End Using
            Return CMGInfo
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
End Class


