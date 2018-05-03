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


Partial Public Class AdInfo

    Public Function GetAllFromAdInfoFiltered(ByVal IsEcommerce As Boolean) As IList(Of AdInfoInfo)

        Dim AdinfoInfos As IList(Of AdInfoInfo) = New List(Of AdInfoInfo)()
        Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@IsEcommerce", SqlDbType.Int, 0), _
                                                          New SqlParameter("@NumDays", SqlDbType.Int, 0)}
        parms(0).Value = IsEcommerce
        parms(1).Value = CInt(ConfigurationManager.AppSettings("AdNumDays"))

        'Execute a query to read the AdInfoInfos
        Using rdr As SqlDataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "tu_ad_info_select_filtered", parms)
            While (rdr.Read())
                'instantiate new AdInfoInfo object via factory method and add to list
                AdinfoInfos.Add(AdInfoFactory.Construct(rdr))
            End While
        End Using
        Return AdinfoInfos
    End Function

    Public Function GetAllFromAdInfo(ByVal IsEcommerce As Boolean) As IList(Of AdInfoInfo)

        Dim AdinfoInfos As IList(Of AdInfoInfo) = New List(Of AdInfoInfo)()
        Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@IsEcommerce", SqlDbType.Int, 0), New SqlParameter("@NumDays", SqlDbType.Int, 0)}
        parms(0).Value = IsEcommerce
        parms(1).Value = 0

        'Execute a query to read the AdInfoInfos
        Using rdr As SqlDataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "tu_ad_info_select_filtered", parms)
            While (rdr.Read())
                'instantiate new AdInfoInfo object via factory method and add to list
                AdinfoInfos.Add(AdInfoFactory.Construct(rdr))
            End While
        End Using
        Return AdinfoInfos
    End Function

    Public Function GetAdInfoByAdNbr(ByVal AdNbr As Integer) As AdInfoInfo
        Dim ai As AdInfoInfo = Nothing

        Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@AdNbr", SqlDbType.Int, 0)}
        parms(0).Value = AdNbr

        'Execute a query to read the AdInfoInfos
        Using rdr As SqlDataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "tu_ad_info_selectby_ad_nbr", parms)
            While (rdr.Read())
                'instantiate new AdInfoInfos object via factory method
                ai = AdInfoFactory.Construct(rdr)
            End While
        End Using
        Return ai
    End Function

    Public Function GetRouteFrmAdLookUp(ByVal AdNbr As Integer) As List(Of RouteFrmAdInfo)
        Dim RouteFrmAdInfos As New List(Of RouteFrmAdInfo)

        Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@AdNbr", SqlDbType.Int)}
        parms(0).Value = AdNbr

        'Execute a query to read the AdInfoInfos
        Using rdr As SqlDataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "tu_route_ad_info_selectby_ad_nbr", parms)
            While (rdr.Read())
                'instantiate new AdInfoInfo object via factory method and add to list
                RouteFrmAdInfos.Add(RouteFromAdFactory.Construct(rdr))
            End While
        End Using
        Return RouteFrmAdInfos
    End Function

    Public Function GetTransferAds(ByVal IsEcommerce As Boolean) As IList(Of AdInfoInfo)

        Dim AdinfoInfos As IList(Of AdInfoInfo) = New List(Of AdInfoInfo)()
        Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@IsEcommerce", SqlDbType.Int, 0), New SqlParameter("@NumDays", SqlDbType.Int, 0)}
        parms(0).Value = IsEcommerce
        parms(1).Value = 0

        'Execute a query to read the AdInfoInfos
        Using rdr As SqlDataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "tu_ad_info_select_filtered_txf", parms)
            While (rdr.Read())
                'instantiate new AdInfoInfo object via factory method and add to list
                AdinfoInfos.Add(AdInfoFactory.Construct(rdr))
            End While
        End Using
        Return AdinfoInfos
    End Function
End Class


