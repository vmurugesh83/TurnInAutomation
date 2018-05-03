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

Partial Public Class CFGDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all CFGInfo records.
    ''' </summary>	    	 
    Public Function GetAllFromCFG() As IList(Of CFGInfo)
        Dim CFGInfo As New List(Of CFGInfo)
        Dim sql As String = _spSchema + ".TU1051SP"
        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql)
                While (rdr.Read())
                    'instantiate new CFGInfo object via factory method and add to list
                    CFGInfo.Add(CFGFactory.Construct(rdr))
                End While
            End Using
            Return CFGInfo
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

End Class

