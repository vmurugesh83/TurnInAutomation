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

Public Class BrandDao
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all LabelInfo records.
    ''' </summary>	    	 
    Public Function GetAllBrands() As IList(Of BrandInfo)
        Dim BrandInfos As New List(Of BrandInfo)

        Dim sql As String = _spSchema + ".TU1062SP"

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, Nothing)
                While (rdr.Read())
                    'instantiate new LabelInfo object via factory method and add to list
                    BrandInfos.Add(BrandFactory.Construct(rdr))
                End While
            End Using
            Return BrandInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
End Class
