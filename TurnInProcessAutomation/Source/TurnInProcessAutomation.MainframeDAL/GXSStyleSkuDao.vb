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

Public Class GXSStyleSkuDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    Public Function GetStyleSKUData(ByVal upc As Decimal) As IList(Of GXSStyleSkuInfo)
        Dim gxsInfo As New List(Of GXSStyleSkuInfo)

        Dim sql As String = _spSchema + ".TU1134SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@UPC", DB2Type.Decimal)}
        parms(0).Value = upc

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    gxsInfo.Add(GXSStyleSkuFactory.Construct(rdr))
                End While
            End Using
            Return gxsInfo
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

End Class
