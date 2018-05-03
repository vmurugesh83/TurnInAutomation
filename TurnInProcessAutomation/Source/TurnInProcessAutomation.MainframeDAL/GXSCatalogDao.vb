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
Imports System.Data.OleDb

Partial Public Class GXSCatalogDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all GXSCFGInfo records.
    ''' </summary>	    	 
    Public Function GetAllFromTU1135SP(ByVal isn As Decimal) As GXSCatalogInfo
        Dim GXSCatalogInfo As New GXSCatalogInfo
        GXSCatalogInfo.GXSCFG = New List(Of GXSCFGInfo)
        GXSCatalogInfo.GXSUPC = New List(Of GXSUPCInfo)
        Dim sql As String = _spSchema + ".TU1135SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@INTERNAL_STYLE_NUM", DB2Type.Decimal)}
        parms(0).Value = isn
        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new GXSCFGInfo object via factory method and add to list
                    GXSCatalogInfo.GXSCFG.Add(GXSCFGFactory.Construct(rdr))
                End While

                rdr.NextResult()
                While (rdr.Read())
                    GXSCatalogInfo.GXSUPC.Add(GXSUPCFactory.Construct(rdr))
                End While
            End Using
            Return GXSCatalogInfo
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

End Class

