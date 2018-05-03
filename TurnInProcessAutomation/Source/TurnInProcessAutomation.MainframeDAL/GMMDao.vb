Imports System.Configuration
Imports BonTon.DBUtility
Imports BonTon.DBUtility.DB2Helper
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory
Public Class GMMDao
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all GMMInfo records.
    ''' </summary>	    	 
    Public Function GetAllFromGMM() As IList(Of GMMInfo)
        Dim GMMInfos As New List(Of GMMInfo)

        Dim sql As String = _spSchema + ".TU1194SP"
        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, Nothing)
                While (rdr.Read())
                    'instantiate new GMMInfos object via factory method and add to list
                    GMMInfos.Add(GMMFactory.Construct(rdr))
                End While
            End Using
            Return GMMInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

End Class
