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

Partial Public Class SellSeasonDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all SellSeasonInfo records.
    ''' </summary>	    	 
    Public Function GetAllFromSellSeason() As IList(Of SellSeasonInfo)
        Dim SellSeasonInfos As New List(Of SellSeasonInfo)

        Dim sql As String = _spSchema + ".TU1006SP"
        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, Nothing)
                While (rdr.Read())
                    'instantiate new SellSeasonInfo object via factory method and add to list
                    SellSeasonInfos.Add(SellSeasonFactory.ConstructBasic(rdr))
                End While
            End Using
            Return SellSeasonInfos
        Catch ex As Exception
            Throw
        End Try
        Return Nothing
    End Function

End Class

