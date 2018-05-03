Imports System.Configuration
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Text
Imports log4net
Imports BonTon.DBUtility
Imports BonTon.DBUtility.SqlHelper
Imports System.Data.SqlClient
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory


Partial Public Class GXSImageDao

    Public Function GetImageData(ByVal upc As Decimal) As IList(Of GXSImageInfo)
        Dim gxsImageInfo As New List(Of GXSImageInfo)

        Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@UPC_NUM", SqlDbType.Decimal, 0)}
        parms(0).Value = upc

        'Execute a query to read the AdInfoInfos
        Using rdr As SqlDataReader = ExecuteReader(ConnectionStringVirtualTicket, CommandType.StoredProcedure, "GetImagesFromVT", parms)
            While (rdr.Read())
                'instantiate new AdInfoInfos object via factory method
                gxsImageInfo.Add(GXSImageFactory.Construct(rdr))
            End While
        End Using
        Return gxsImageInfo
    End Function

End Class


