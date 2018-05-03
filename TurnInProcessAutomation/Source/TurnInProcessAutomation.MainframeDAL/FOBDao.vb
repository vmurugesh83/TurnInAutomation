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
Public Class FOBDao
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all FOBInfo records.
    ''' </summary>	    	 
    Public Function GetAllFOB() As IList(Of FOBInfo)
        Dim FOBInfo As New List(Of FOBInfo)

        Dim sql As String = _spSchema + ".TU1052SP"
        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql)
                While (rdr.Read())
                    'instantiate new FOBInfo object via factory method and add to list
                    FOBInfo.Add(FOBFactory.Construct(rdr))
                End While
            End Using
            Return FOBInfo
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
End Class

