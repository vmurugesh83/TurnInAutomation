Imports System.Data.SqlClient

Public Class WorkhorseAdminDao

    ''' <summary>
    '''  Get all changes from the tally table for an ad
    ''' </summary>
    ''' <param name="adNumber"> AdNumber requested by Message Broker </param>
    ''' <returns>List of all changes on the Ad.</returns>
    ''' <remarks> This function drives the service to generate records for individual changes. </remarks>
    Public Function GetChangesForAd(ByVal adNumber As Integer) As List(Of WorkhorseAdminChange)
        Dim workhorseChangesForAd As New List(Of WorkhorseAdminChange)

        Dim params As SqlParameter() = New SqlParameter() {New SqlParameter("@ad_nbr", SqlDbType.Int, 0)}
        params(0).Value = adNumber

        Dim connectString As String = ConfigurationManager.ConnectionStrings("SQLServer").ConnectionString

        Using connection As New SqlConnection(connectString)
            Using command As New SqlCommand("WH_GetChangesForAd", connection)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.AddRange(params)
                command.Connection.Open()
                Dim reader As SqlDataReader = command.ExecuteReader()

                While reader.Read()
                    workhorseChangesForAd.Add(BuildWorkhorseAdminChange(reader))
                End While
            End Using
        End Using

        Return workhorseChangesForAd
    End Function

    ''' <summary>
    ''' Get full Admin Data for the Ad requested by Message Broker
    ''' </summary>
    ''' <param name="adNumber">Ad requested by Message Broker</param>
    ''' <returns>List of Admin Full Data</returns>
    ''' <remarks></remarks>
    Public Function GetFullDataForAd(ByVal adNumber As Integer) As List(Of AdminFullData)

        Dim adData As New List(Of AdminFullData)

        Dim params As SqlParameter() = New SqlParameter() {New SqlParameter("@Ad_Number", SqlDbType.Int, 0)}
        params(0).Value = adNumber

        Dim connectionString As String = ConfigurationManager.ConnectionStrings("SQLServer").ConnectionString

        Using connection As New SqlConnection(connectionString)
            Using Command As New SqlCommand("CMR_FullData_Master", connection)
                Command.CommandType = CommandType.StoredProcedure
                Command.Parameters.AddRange(params)
                Command.Connection.Open()
                Dim reader As SqlDataReader = Command.ExecuteReader()

                While reader.Read()
                    adData.Add(BuildAdminFullData(reader))
                End While
            End Using
        End Using

        Return adData
    End Function

    ''' <summary>
    '''  Populate the WorkhorseAdminChange list.
    ''' </summary>
    ''' <param name="reader"> Data for the list</param>
    ''' <returns> A record for the Workhorse Admin Change List.</returns>
    ''' <remarks></remarks>
    Private Function BuildWorkhorseAdminChange(reader As SqlDataReader) As WorkhorseAdminChange
        Dim workhorseChange As New WorkhorseAdminChange()

        With workhorseChange

            .RowId = CInt(reader.Item("RowId"))
            .WHaction = CStr(reader.Item("WHaction"))

            If Not IsDBNull(reader.Item("wh_ad_nbr")) Then
                .wh_ad_nbr = CInt(reader.Item("wh_ad_nbr"))
            End If

            If Not IsDBNull(reader.Item("wh_sys_pg_nbr")) Then
                .wh_sys_pg_nbr = CInt(reader.Item("wh_sys_pg_nbr"))
            End If

            If Not IsDBNull(reader.Item("wh_shot_nbr")) Then
                .wh_shot_nbr = CInt(reader.Item("wh_shot_nbr"))
            End If

            If Not IsDBNull(reader.Item("wh_img_id")) Then
                .wh_img_id = CInt(reader.Item("wh_img_id"))
            End If

            If Not IsDBNull(reader.Item("wh_merch_id")) Then
                .wh_merch_id = CInt(reader.Item("wh_merch_id"))
            End If

            If Not IsDBNull(reader.Item("TriggerEvent")) Then
                .TriggerEvent = CStr(reader.Item("TriggerEvent"))
            End If

            If Not IsDBNull(reader.Item("Action")) Then
                .Action = CStr(reader.Item("Action"))
            End If

            If Not IsDBNull(reader.Item("TallyDateTime")) Then
                .TallyDateTime = CDate(reader.Item("TallyDateTime"))
            End If

            If Not IsDBNull(reader.Item("Source")) Then
                .Source = CStr(reader.Item("Source"))
            End If

            If Not IsDBNull(reader.Item("old_ad_nbr")) Then
                .old_ad_nbr = CInt(reader.Item("old_ad_nbr"))
            End If

            If Not IsDBNull(reader.Item("old_sys_pg_nbr")) Then
                .old_sys_pg_nbr = CInt(reader.Item("old_sys_pg_nbr"))
            End If

            If Not IsDBNull(reader.Item("old_shot_nbr")) Then
                .old_shot_nbr = CInt(reader.Item("old_shot_nbr"))
            End If

            If Not IsDBNull(reader.Item("old_img_id")) Then
                .old_img_id = CInt(reader.Item("old_img_id"))
            End If

            If Not IsDBNull(reader.Item("old_merch_id")) Then
                .old_merch_id = CInt(reader.Item("old_merch_id"))
            End If

        End With

        Return workhorseChange
    End Function

    ''' <summary>
    '''  Populate the AdminFullData list.
    ''' </summary>
    ''' <param name="reader"> Data for the list</param>
    ''' <returns> A record for the Full Admin Data List.</returns>
    ''' <remarks></remarks>
    Private Function BuildAdminFullData(reader As SqlDataReader) As AdminFullData

        Dim adminData As New AdminFullData()

        With adminData

            .AdNumber = CInt(reader.Item("AdNumber"))

            If Not IsDBNull(reader.Item("AdDescription")) Then
                .AdDescription = CStr(reader.Item("AdDescription"))
            End If

            If Not IsDBNull(reader.Item("AdEnd")) Then
                .AdEnd = CDate(reader.Item("AdEnd"))
            End If

            If Not IsDBNull(reader.Item("AdStart")) Then
                .AdStart = CDate(reader.Item("AdStart"))
            End If

            If Not IsDBNull(reader.Item("AdStatus")) Then
                .AdStatus = CChar(reader.Item("AdStatus"))
            End If

            If Not IsDBNull(reader.Item("AdVersion")) Then
                .AdVersion = CDec(reader.Item("AdVersion"))
            End If

            If Not IsDBNull(reader.Item("AssocFirst")) Then
                .AssocFirst = CStr(reader.Item("AssocFirst"))
            End If

            If Not IsDBNull(reader.Item("AssocId")) Then
                .AssocId = CStr(reader.Item("AssocId"))
            End If

            If Not IsDBNull(reader.Item("AssocLast")) Then
                .AssocLast = CStr(reader.Item("AssocLast"))
            End If

            If Not IsDBNull(reader.Item("AssocPhone")) Then
                .AssocPhone = CStr(reader.Item("AssocPhone"))
            End If

            If Not IsDBNull(reader.Item("EventEnd")) Then
                .EventEnd = CDate(reader.Item("EventEnd"))
            End If

            If Not IsDBNull(reader.Item("EventStart")) Then
                .EventStart = CDate(reader.Item("EventStart"))
            End If

            If Not IsDBNull(reader.Item("EventName")) Then
                .EventName = CStr(reader.Item("EventName"))
            End If

            If Not IsDBNull(reader.Item("MediaDescription")) Then
                .MediaDescription = CStr(reader.Item("MediaDescription"))
            End If

            If Not IsDBNull(reader.Item("MediaType")) Then
                .MediaType = CStr(reader.Item("MediaType"))
            End If

            If Not IsDBNull(reader.Item("PhotoStart")) Then
                .PhotoStart = CDate(reader.Item("PhotoStart"))
            End If

            If Not IsDBNull(reader.Item("PhotoEnd")) Then
                .PhotoEnd = CDate(reader.Item("PhotoEnd"))
            End If

            If Not IsDBNull(reader.Item("TurnInDate")) Then
                .TurnInDate = CDate(reader.Item("TurnInDate"))
            End If

            If Not IsDBNull(reader.Item("PageNumber")) Then
                .PageNumber = CInt(reader.Item("PageNumber"))
            End If

            If Not IsDBNull(reader.Item("PageDescription")) Then
                .PageDescription = CStr(reader.Item("PageDescription"))
            End If

            If Not IsDBNull(reader.Item("CoverPage")) Then
                .CoverPage = CStr(reader.Item("CoverPage"))
            End If

            If Not IsDBNull(reader.Item("PageActiveIndicator")) Then
                .PageActiveIndicator = CChar(reader.Item("PageActiveIndicator"))
            End If

            If Not IsDBNull(reader.Item("ShotNumber")) Then
                .ShotNumber = CInt(reader.Item("ShotNumber"))
            End If

            If Not IsDBNull(reader.Item("ImageNumber")) Then
                .ImageNumber = CInt(reader.Item("ImageNumber"))
            End If

            If Not IsDBNull(reader.Item("ImageDescription")) Then
                .ImageDescription = CStr(reader.Item("ImageDescription"))
            End If

            If Not IsDBNull(reader.Item("ImageClass")) Then
                .ImageClass = CStr(reader.Item("ImageClass"))
            End If

            If Not IsDBNull(reader.Item("ImageNotes")) Then
                .ImageNotes = CStr(reader.Item("ImageNotes"))
            End If

            If Not IsDBNull(reader.Item("ImageSource")) Then
                .ImageSource = CStr(reader.Item("ImageSource"))
            End If

            If Not IsDBNull(reader.Item("ImageSuffixType")) Then
                .ImageSuffixType = CStr(reader.Item("ImageSuffixType"))
            End If

            If Not IsDBNull(reader.Item("ImageMediaType")) Then
                .ImageMediaType = CStr(reader.Item("ImageMediaType"))
            End If

            If Not IsDBNull(reader.Item("ImageActiveIndicator")) Then
                .ImageActiveIndicator = CChar(reader.Item("ImageActiveIndicator"))
            End If

            If Not IsDBNull(reader.Item("MerchId")) Then
                .MerchId = CInt(reader.Item("MerchId"))
            End If

            If Not IsDBNull(reader.Item("MerchActiveIndicator")) Then
                .MerchActiveIndicator = CChar(reader.Item("MerchActiveIndicator"))
            End If

            If Not IsDBNull(reader.Item("StylingNotes")) Then
                .StylingNotes = CStr(reader.Item("StylingNotes"))
            End If

            'If Not IsDBNull(reader.Item("MerchGroup")) Then
            '    .MerchGroup = CStr(reader.Item("MerchGroup"))
            'End If

        End With

        Return adminData

    End Function

End Class
