Imports System.Configuration
Imports BonTon.DBUtility
Imports BonTon.DBUtility.DB2Helper
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory
Public Class DMMDao
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all DMMInfo records.
    ''' </summary>	    	 
    Public Function GetAllFromDMM() As IList(Of DMMInfo)
        Dim EMMInfos As New List(Of DMMInfo)

        Dim sql As String = _spSchema + ".TU1142SP"
        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, Nothing)
                While (rdr.Read())
                    'instantiate new EMMInfos object via factory method and add to list
                    EMMInfos.Add(DMMFactory.Construct(rdr))
                End While
            End Using
            Return EMMInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

End Class
