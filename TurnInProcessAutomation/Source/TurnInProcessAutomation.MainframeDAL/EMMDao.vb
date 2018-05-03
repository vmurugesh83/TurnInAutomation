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

Public Class EMMDao
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all EMMInfo records.
    ''' </summary>	    	 
    Public Function GetAllFromEMM() As IList(Of EMMInfo)
        Dim EMMInfos As New List(Of EMMInfo)

        Dim sql As String = _spSchema + ".TU1076SP"
        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, Nothing)
                While (rdr.Read())
                    'instantiate new EMMInfos object via factory method and add to list
                    EMMInfos.Add(EMMFactory.Construct(rdr))
                End While
            End Using
            Return EMMInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
End Class
