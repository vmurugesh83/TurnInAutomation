Imports System.Data.SqlClient
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class RouteFromAdFactory
    Public Shared Function Construct(ByVal reader As SqlDataReader) As RouteFrmAdInfo
        Dim RteFrmAdInfo As New RouteFrmAdInfo()

        With RteFrmAdInfo
            .AdNbr = CInt(ReadColumn(reader, "ad_nbr"))
        End With

        Return RteFrmAdInfo
    End Function
End Class
