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

Partial Public Class SellYearDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all SellYearInfo records.
    ''' </summary>	    	 
    Public Function GetAllFromSellYear() As IList(Of SellYearInfo)
        Dim SellYearInfos As New List(Of SellYearInfo)

        Dim sql As String = _spSchema + ".TU1007SP"
        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, Nothing)
                While (rdr.Read())
                    'instantiate new SellYearInfo object via factory method and add to list
                    SellYearInfos.Add(SellYearFactory.ConstructBasic(rdr))
                End While
            End Using
            Return SellYearInfos
        Catch ex As Exception
            Throw
        End Try
        Return Nothing
    End Function

End Class

