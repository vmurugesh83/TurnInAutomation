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
Public Class LocationDao
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all CMGInfo records.
    ''' </summary>	    	 
    Public Function GetLocations() As IList(Of LocationInfo)
        Dim LocInfo As New List(Of LocationInfo)

        Dim sql As String = _spSchema + ".TU1114SP"
        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql)
                While (rdr.Read())
                    'instantiate new CMGInfo object via factory method and add to list
                    LocInfo.Add(LocationFactory.Construct(rdr))
                End While
            End Using
            Return LocInfo
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to get all locations for an ISN and Color which has Onhand Quantity.
    ''' </summary>	    	 
    Public Function GetLocationsByISNColor(ByVal ISN As Decimal, ByVal ColorCode As Integer) As IList(Of LocationInfo)
        Dim LocInfo As New List(Of LocationInfo)

        Dim sql As String = _spSchema + ".TU1116SP"

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal), _
                                                          New DB2Parameter("@ColorCode", DB2Type.Integer)}

        parms(0).Value = ISN
        parms(1).Value = ColorCode

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new CMGInfo object via factory method and add to list
                    LocInfo.Add(LocationFactory.Construct(rdr))
                End While
            End Using
            Return LocInfo
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
    ''' <summary>
    ''' Method to get location details by location id.
    ''' </summary>	    	 
    Public Function GetLocationByLocationID(ByVal locationID As Integer) As LocationInfo
        Dim locationDetails As LocationInfo = Nothing

        Dim sql As String = _spSchema + ".TU1157SP"

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@LOCATION_ID", DB2Type.Integer)}

        parms(0).Value = locationID

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                If (rdr.HasRows()) Then
                    rdr.Read()
                    locationDetails = New LocationInfo()
                    'instantiate new CMGInfo object via factory method and add to list
                    locationDetails = LocationFactory.Construct(rdr)
                End If
            End Using
            Return locationDetails
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

End Class


