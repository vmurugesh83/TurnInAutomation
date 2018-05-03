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

Public Class GXSImageDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    Public Function GetImageData(ByVal upc As Decimal) As IList(Of GXSImageInfo)
        Dim gxsImageInfo As New List(Of GXSImageInfo)

        Dim sql As String = _spSchema + ".TU1136SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@UPC_NUM", DB2Type.Decimal)}
        parms(0).Value = upc

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    gxsImageInfo.Add(GXSImageFactory.Construct(rdr))
                End While
            End Using
            Return gxsImageInfo
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

End Class
