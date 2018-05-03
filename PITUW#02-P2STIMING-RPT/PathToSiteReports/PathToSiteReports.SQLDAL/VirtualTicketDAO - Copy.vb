Imports System.Data.SqlClient
Imports BonTon.DBUtility.SqlHelper
Imports PathToSiteReports.BusinessEntities

Public Class VirtualTicketDAO
    Public Function GetImageShotDateByImageID(ByVal imageID As Integer) As List(Of VirtualTicket)
        Dim thumbnailDate As Date = Nothing
        Dim sqlQuery As String = String.Empty
        Dim vtImageDetails As List(Of VirtualTicket) = Nothing
        Dim vtImageDetail As VirtualTicket = Nothing

        Try
            sqlQuery = String.Format("SELECT IMAGE_ID, " &
                                     " [Document Type] AS Image_Extn, " &
                                     " [Thumbnailed Date] AS Thumbnail_Date " &
                                     "  FROM  [view Digital Resources] AS VDR " &
                                     " WHERE VDR.IMAGE_ID = {0} " &
                                     " AND [File Location] LIKE '%ECOMMERCE:ECOMM_IMAGES%' " &
                                     " AND [Document Type] != 'Folder' " &
                                     " ORDER BY [Thumbnailed Date] ASC", _
                imageID)

            vtImageDetails = New List(Of VirtualTicket)()

            'Execute a query to read the adInfo
            Using rdr As SqlDataReader = ExecuteReader(ConnectionStringVirtualTicket, CommandType.Text, sqlQuery)
                While (rdr.Read())
                    vtImageDetail = New VirtualTicket()
                    vtImageDetail.ImageID = CInt(rdr("IMAGE_ID"))
                    vtImageDetail.ImageModifiedDate = CDate(rdr("Thumbnail_Date"))
                    vtImageDetail.ImageExtension = CStr(rdr("Image_Extn"))
                    vtImageDetails.Add(vtImageDetail)
                End While
            End Using

        Catch ex As Exception
            Throw
        End Try
        Return vtImageDetails
    End Function

    Public Function GetFinalImageReadyDateByImageID(ByVal imageID As Integer) As Date
        Dim thumbnailDate As Date = Nothing
        Dim sqlQuery As String = String.Empty

        Try
            sqlQuery = String.Format("SELECT TOP 1 [Thumbnailed Date] AS Thumbnail_Date " &
                                     " FROM  [view Digital Resources] AS VDR " &
                                     " WHERE VDR.IMAGE_ID = {0} " &
                                     " AND [File Location] LIKE '%ECOMMERCE:ECOMM_IMAGES%' " &
                                     " AND [Document Type] != 'Folder' " &
                                     " AND [Document Type] = 'jpg' " &
                                     " ORDER BY [Thumbnailed Date] ASC", _
                imageID)

            'Execute a query to read the adInfo
            Using rdr As SqlDataReader = ExecuteReader(ConnectionStringVirtualTicket, CommandType.Text, sqlQuery)
                If rdr.HasRows Then
                    rdr.Read()
                    thumbnailDate = CDate(rdr("Thumbnail_Date"))
                End If
            End Using

        Catch ex As Exception
            Throw
        End Try
        Return thumbnailDate
    End Function
    Public Function UpdateImageShotAndFinalImageReadyDate(ByVal skuDetailsTable As DataTable) As DataTable
        Dim tempTableQuery As String = String.Empty
        Dim updateStatement As String = String.Empty
        Dim selectQuery As String = String.Empty
        Dim updatedTable As DataTable = Nothing
        Dim rdr As IDataReader = Nothing
        Try
            tempTableQuery = GetSKUDetailsTempTableQuery()
            Using sqlCon As SqlConnection = New SqlConnection(ConnectionStringVirtualTicket)
                sqlCon.Open()

                Using sqlComm As SqlCommand = New SqlCommand(String.Empty, sqlCon)

                    sqlComm.CommandTimeout = 600
                    'Create the temp table
                    sqlComm.CommandText = tempTableQuery
                    sqlComm.ExecuteNonQuery()

                    'Write the data from SKU details data table to the temp table
                    Using bulkCopy As SqlBulkCopy = New SqlBulkCopy(sqlCon)
                        bulkCopy.BulkCopyTimeout = 700
                        bulkCopy.DestinationTableName = "#TmpSKUDetail"
                        bulkCopy.WriteToServer(skuDetailsTable)
                        bulkCopy.Close()
                    End Using

                    'Now we have the temp table with data, so update the image shot date
                    updateStatement = GetImageShotDateUpdateStatement()
                    sqlComm.CommandText = updateStatement
                    sqlComm.ExecuteNonQuery()

                    'Now we have the temp table with data, so update the final image ready date
                    updateStatement = GetFinalImageReadyDateUpdateStatement()
                    sqlComm.CommandText = updateStatement
                    sqlComm.ExecuteNonQuery()

                    'Read the results to a database
                    selectQuery = "SELECT ISNULL(GMM,'') AS GMM," &
                        " ISNULL(DMM,'') AS DMM, " &
                        " ISNULL(BUYER,'') AS BUYER, " &
                        " ISNULL(FOB,'') AS FOB, " &
                        " ISNULL(DEPT,'') AS DEPT, " &
                        " ISNULL(Class,'') AS Class, " &
                        " ISNULL(INTERNAL_STYLE_NUM,0) AS INTERNAL_STYLE_NUM, " &
                        " CLR_CDE, SKU_NUM, UPC_NUM," &
                        " SS_DATE, SAMPLE_REQ_DATE, CMR_DATE, " &
                        " SAMPLE_DUE_DATE, SMPL_PRIM_LOC_NME, SAMPLE_STATUS_DATE, SAMPLE_STATUS_DESC, PO_APPROVAL_DTE, DC_SHIP_DTE, " &
                        " TURN_IN_DATE, FIRST_RC_DATE, COPY_READY_DATE, " &
                        " PRODUCT_READY_DATE, PRODUCT_ACTIVE_DATE, " &
                        " QUANTITY, OWN_PRICE_AMT, REPORT_ITEM_TYPE, " &
                        " ISNULL(AD_NUM,0) AS AD_NUM, " &
                        " ISNULL(AD_DESC, '') AS AD_DESC, ISNULL(AD_TYPE, '') AS AD_TYPE, " &
                        " IMAGE_SHOT_DATE, IMAGE_READY_DATE, JOB_SCHEDULE_DATE" &
                        " FROM #TmpSKUDetail"
                    sqlComm.CommandText = selectQuery
                    rdr = sqlComm.ExecuteReader(CommandBehavior.CloseConnection)
                    updatedTable = ConvertToDataTable(rdr)
                End Using
            End Using
        Catch ex As Exception
            Throw
        End Try

        Return updatedTable

    End Function
    Private Function GetSKUDetailsTempTableQuery() As String
        Dim tempTableQuery As String = String.Empty

        tempTableQuery = "CREATE TABLE #TmpSKUDetail (GMM VARCHAR(100), DMM VARCHAR(100)" &
            ", BUYER VARCHAR(100), FOB VARCHAR(100), DEPT VARCHAR(100), Class VARCHAR(100)" &
            ", INTERNAL_STYLE_NUM DECIMAL(15,0), CLR_CDE INTEGER, SKU_NUM DECIMAL(15,0)" &
            ", UPC_NUM DECIMAL(15,0), SS_DATE DATETIME " &
            ", SAMPLE_REQ_DATE DATETIME, CMR_DATE DATETIME, SAMPLE_DUE_DATE DATETIME, SMPL_PRIM_LOC_NME VARCHAR(200)" &
            ", SAMPLE_STATUS_DATE DATETIME, SAMPLE_STATUS_DESC VARCHAR(30), PO_APPROVAL_DTE DATETIME, DC_SHIP_DTE DATETIME, TURN_IN_DATE DATETIME" &
            ", FIRST_RC_DATE DATETIME, COPY_READY_DATE DATETIME, PRODUCT_READY_DATE DATETIME" &
            ", PRODUCT_ACTIVE_DATE DATETIME, QUANTITY INTEGER, OWN_PRICE_AMT DECIMAL(9,2), " &
            " REPORT_ITEM_TYPE VARCHAR(10), AD_NUM INTEGER, AD_DESC VARCHAR(200)" &
            ", AD_TYPE VARCHAR(100), IMAGE_ID_NUM INTEGER, IMAGE_SHOT_DATE DATETIME" &
            ", IMAGE_READY_DATE DATETIME, JOB_SCHEDULE_DATE DATETIME)"

        Return tempTableQuery
    End Function

    Private Function GetImageShotDateUpdateStatement() As String
        Dim updateStatement As String = "UPDATE [#TmpSKUDetail] " &
            " SET IMAGE_SHOT_DATE= (SELECT TOP 1 [Thumbnailed Date] FROM  [view Digital Resources] AS VDR WHERE VDR.IMAGE_ID = VTI.image_id_num" &
            " AND [File Location] LIKE '%ECOMMERCE:ECOMM_IMAGES%'" &
            " AND [Document Type] != 'Folder' " &
            " AND [Document Type] != 'jpg'" &
            " ORDER BY [Thumbnailed Date] ASC)" &
            " FROM  [#TmpSKUDetail] AS VTI; "
        Return updateStatement
    End Function
    Private Function GetFinalImageReadyDateUpdateStatement() As String
        Dim updateStatement As String = "UPDATE [#TmpSKUDetail] " &
            " SET IMAGE_READY_DATE = (SELECT TOP 1 [Thumbnailed Date] FROM  [view Digital Resources] AS VDR WHERE VDR.IMAGE_ID = VTI.image_id_num" &
            " AND [File Location] LIKE '%ECOMMERCE:ECOMM_IMAGES%'" &
            " AND [Document Type] != 'Folder' " &
            " AND [Document Type] = 'jpg'" &
            " ORDER BY [Thumbnailed Date] ASC)" &
            " FROM  [#TmpSKUDetail] AS VTI; "

        'Dim updateStatement As String = "UPDATE [#TmpSKUDetail] " &
        '    " SET IMAGE_SHOT_DATE= VT.[Thumbnailed Date]" &
        '    " FROM  (SELECT TOP 1 [Thumbnailed Date], image_id_num FROM  [view Digital Resources] AS VDR" &
        '    " INNER JOIN [#TmpSKUDetail] AS VTI" &
        '    " ON VDR.IMAGE_ID = VTI.image_id_num" &
        '    " WHERE [File Location] LIKE '%ECOMMERCE:ECOMM_IMAGES%'" &
        '    " AND [Document Type] != 'Folder' " &
        '    " AND [Document Type] = 'jpg'" &
        '    " ORDER BY [Thumbnailed Date] ASC) AS VT" &
        '    " WHERE VT. image_id_num = [#TmpSKUDetail].image_id_num ;"
        Return updateStatement
    End Function
    Private Function ConvertToDataTable(ByVal rdr As SqlDataReader) As DataTable
        Dim dtReturn As DataTable = New DataTable("dt")
        Dim dtSchemaTable As DataTable = rdr.GetSchemaTable
        Dim cField As Integer = dtSchemaTable.Rows.Count
        Dim iField As Integer = 0
        While (iField < cField)
            dtReturn.Columns.Add(New DataColumn(dtSchemaTable.Rows(iField)("ColumnName").ToLower(), _
                                                dtSchemaTable.Rows(iField)("DataType")))
            iField += 1
        End While
        '
        If (rdr.HasRows) Then
            Dim drRow As DataRow
            While rdr.Read
                drRow = dtReturn.NewRow
                iField = 0
                While (iField < cField)
                    drRow(iField) = rdr(iField)

                    iField += 1
                End While
                dtReturn.Rows.Add(drRow)
            End While
        End If

        Return dtReturn
    End Function
End Class
