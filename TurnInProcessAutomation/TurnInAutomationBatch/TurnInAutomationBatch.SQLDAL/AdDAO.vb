Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports BonTon.DBUtility
Imports BonTon.DBUtility.SqlHelper
Imports TurnInProcessAutomation.Factory
Imports TurnInProcessAutomation.BusinessEntities

Public Class AdDAO
    Public Function GetTheLatestAdforTIA(Optional ByVal isVendorImage As Boolean = False) As AdInfoInfo
        Dim adInfo As New AdInfoInfo

        'Execute a query to read the adInfo
        Using rdr As SqlDataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure,
                                                   IIf(isVendorImage, "tu_select_vendorimage_ad_for_tia", "tu_select_ads_for_tia"))
            If rdr.HasRows Then
                rdr.Read()
                adInfo = AdInfoFactory.Construct(rdr)
            End If
        End Using
        Return adInfo
    End Function
End Class
