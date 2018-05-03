Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TULocation
    Private dal As MainframeDAL.LocationDao = New MainframeDAL.LocationDao

    Public Function GetAll() As IList(Of LocationInfo)
        Try
            Return dal.GetLocations
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetLocationByLocationID(ByVal LocationID As Integer) As LocationInfo
        Dim locationDetails As LocationInfo = Nothing

        Try
            locationDetails = dal.GetLocationByLocationID(LocationID)
            Return locationDetails
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Public Function GetAllByISNColor(ByVal ISN As Decimal, ByVal ColorCode As Integer) As IList(Of LocationInfo)
        Try
            Return dal.GetLocationsByISNColor(ISN, ColorCode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class




