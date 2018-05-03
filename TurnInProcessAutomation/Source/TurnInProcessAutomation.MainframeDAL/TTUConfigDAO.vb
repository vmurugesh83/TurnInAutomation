Imports System.Configuration
Imports System.Collections.Generic
Imports IBM.Data.DB2
Imports BonTon.DBUtility
Imports BonTon.DBUtility.DB2Helper
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory

Public Class TTUConfigDAO
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get configurations by the key name
    ''' </summary>	    	 
    Public Function GetConfigurationByKey(ByVal ConfigKey As String) As IList(Of TTUConfig)
        Dim TurnInConfiguration As New List(Of TTUConfig)

        Dim sql As String = _spSchema + ".TU1196SP"

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@CONFIGKEY", DB2Type.VarChar)}

        parms(0).Value = ConfigKey

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new TTUConfig object via factory method and add to list
                    TurnInConfiguration.Add(TTUConfigFactory.Construct(rdr))
                End While
            End Using
            Return TurnInConfiguration
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
End Class
