
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
Public Class CRGDao
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all CRGInfo records.
    ''' </summary>	    	 
    Public Function GetAllCRG() As IList(Of CRGInfo)
        Dim CRGInfo As New List(Of CRGInfo)

        Dim sql As String = _spSchema + ".TU1049SP"
        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql)
                While (rdr.Read())
                    'instantiate new CrgInfo object via factory method and add to list
                    CRGInfo.Add(CRGFactory.Construct(rdr))
                End While
            End Using
            Return CRGInfo
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
End Class
