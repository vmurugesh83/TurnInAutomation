Imports System.Configuration
Imports BonTon.DBUtility.DB2Helper
Imports IBM.Data.DB2
Imports PathToSiteReports.BusinessEntities
Imports PathToSiteReports.Factory

Public Class ReportsDAO
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")
    'Public Function GetSKUDetailReport() As List(Of SKUDetail)
    Public Function GetOOSKUDetailReport() As DataTable
        Dim sql As String = String.Empty
        Dim skuDetailTable As DataTable = Nothing

        Try
            'Execute OO Query
            sql = GetTimingReportSKUDetailsOOQuery()
            skuDetailTable = ExecGetDataTable(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)

        Catch ex As Exception
            Throw ex
        End Try
        Return skuDetailTable
    End Function
    Public Function GetWebCatDetails(Optional ByVal isOH As Boolean = True) As DataTable
        Dim sql As String = String.Empty
        Dim skuDetailTable As DataTable = Nothing

        Try
            If isOH Then
                ''Execute OH Query
                sql = GetWebcatDetailsOHQuery()
            Else
                sql = GetWebcatDetailsOOQuery()
            End If

            skuDetailTable = ExecGetDataTable(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)
        Catch ex As Exception
            Throw
        End Try
        Return skuDetailTable

    End Function
    Public Function GetPurchaseOrderDetails(Optional ByVal isOH As Boolean = True) As DataTable
        Dim sql As String = String.Empty
        Dim skuDetailTable As DataTable = Nothing

        Try
            If isOH Then
                ''Execute OH Query
                sql = GetPurchaseOrderDetailsOHQuery()
            Else
                sql = GetPurchaseOrderDetailsOOQuery()
            End If

            skuDetailTable = ExecGetDataTable(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)
        Catch ex As Exception
            Throw
        End Try
        Return skuDetailTable

    End Function
    Public Function GetGeneralDetails(Optional ByVal isOH As Boolean = True) As DataTable
        Dim sql As String = String.Empty
        Dim skuDetailTable As DataTable = Nothing

        Try
            If isOH Then
                ''Execute OH Query
                sql = GetGeneralDetailsOHQuery()
            Else
                sql = GetGeneralDetailsOOQuery()
            End If

            skuDetailTable = ExecGetDataTable(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)
        Catch ex As Exception
            Throw
        End Try
        Return skuDetailTable

    End Function
    Public Function GetTurnInDetails(Optional ByVal isOH As Boolean = True) As DataTable
        Dim sql As String = String.Empty
        Dim skuDetailTable As DataTable = Nothing

        Try
            If isOH Then
                ''Execute OH Query
                sql = GetTurnInDetailsOHQuery()
            Else
                sql = GetTurnInDetailsOOQuery()
            End If

            skuDetailTable = ExecGetDataTable(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)
        Catch ex As Exception
            Throw
        End Try
        Return skuDetailTable

    End Function
    Public Function GetSKUDetailsListFromDataTable(ByVal skuDetails As DataTable) As List(Of SKUDetail)
        Dim resultList As New List(Of SKUDetail)

        Using rdr As DataTableReader = skuDetails.CreateDataReader()
            While (rdr.Read())
                resultList.Add(ReportFactory.ConstructSKUDetailFromDataTableReader(rdr))
            End While
        End Using

        Return resultList
    End Function

    Public Function GetSKUWithOHReceipt(ByVal startDate As Date, ByVal endDate As Date) As DataTable
        Dim sql As String = String.Empty
        Dim ohReceiptTable As DataTable = Nothing

        Try

            sql = GetPathToSiteReportOHQuery(startDate, endDate)
            ohReceiptTable = ExecGetDataTable(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)

        Catch ex As Exception
            Throw
        End Try
        Return ohReceiptTable.DefaultView.ToTable(True)
    End Function

    Public Function GetSSSetupToSampleRequestDays() As List(Of Integer)
        Dim resultList As New List(Of Integer)
        Dim sql As String = String.Empty

        Try
            sql = GetSSSetupToSampleRequestQuery()
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)
                While (rdr.Read())
                    resultList.Add(rdr("SAMPLE_REQ_DATE_TO_SS_DATE"))
                End While
            End Using
        Catch ex As Exception
            Throw
        End Try

        Return resultList
    End Function


    Public Function GetSampleRequestToReceiptDays() As List(Of Integer)
        Dim resultList As New List(Of Integer)
        Dim sql As String = String.Empty

        Try
            sql = GetSampleRequestToReceiptQuery()
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)
                While (rdr.Read())
                    resultList.Add(rdr("SR_TO_RECEIPT_DAYS"))
                End While
            End Using
        Catch ex As Exception
            Throw
        End Try

        Return resultList
    End Function

    Public Function GetSSSetupToPOApprovalDays() As List(Of Integer)
        Dim resultList As New List(Of Integer)
        Dim sql As String = String.Empty

        Try
            sql = GetSSSetupToPOApprovalQuery()
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)
                While (rdr.Read())
                    resultList.Add(rdr("PO_APP_TO_SS_SETUP"))
                End While
            End Using
        Catch ex As Exception
            Throw
        End Try

        Return resultList
    End Function
    Public Function GetSSSetupToTurnInDays() As List(Of Integer)
        Dim resultList As New List(Of Integer)
        Dim sql As String = String.Empty

        Try
            sql = GetSSSetupToTurnInQuery()
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)
                While (rdr.Read())
                    resultList.Add(rdr("TRNIN_TO_SS_SETUP"))
                End While
            End Using
        Catch ex As Exception
            Throw
        End Try

        Return resultList
    End Function

    Public Function GetPOAprovalToTurnInDays() As List(Of Integer)
        Dim resultList As New List(Of Integer)
        Dim sql As String = String.Empty

        Try
            sql = GetPOApprovalToTurnInQuery()
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)
                While (rdr.Read())
                    resultList.Add(rdr("TRNIN_TO_PO_APPROVAL"))
                End While
            End Using
        Catch ex As Exception
            Throw
        End Try

        Return resultList
    End Function
    Public Function GetSampleReceiptToTurnInDays() As List(Of Integer)
        Dim resultList As New List(Of Integer)
        Dim sql As String = String.Empty

        Try
            sql = GetTurnInToSampleReceiptQuery()
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)
                While (rdr.Read())
                    resultList.Add(rdr("TRNIN_TO_SMPL_RECEIPT"))
                End While
            End Using
        Catch ex As Exception
            Throw
        End Try

        Return resultList
    End Function

    Public Function GetPOReceiptToTurnInDays() As List(Of Integer)
        Dim resultList As New List(Of Integer)
        Dim sql As String = String.Empty

        Try
            sql = GetPOReceiptToTurnInQuery()
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)
                While (rdr.Read())
                    resultList.Add(rdr("TRNIN_TO_PO_RECEIPT"))
                End While
            End Using
        Catch ex As Exception
            Throw
        End Try

        Return resultList
    End Function

#Region "ON HAND"

    Private Function GetTimingReportSKUDetailsOHQuery() As String
        Dim sqlQuery As String = String.Format("SELECT " &
                " CAST(TMS206.GMM_ID AS VARCHAR(4)) || ' - ' || TMS206.GMM_DESC AS GMM, " &
                " CAST(TMS207.DMM_ID AS VARCHAR(4)) || ' - ' || TMS207.DMM_DESC AS DMM," &
                " CAST(TMS208.BUYER_ID AS VARCHAR(4)) || ' - ' || TMS208.BUYER_DESC AS BUYER," &
                " CAST(TMS212.FOB_ID AS VARCHAR(4)) || ' - ' || TMS212.FOB_DESC AS FOB," &
                " CAST(TMS213.DEPT_ID AS VARCHAR(4)) || ' - ' || TMS213.DEPT_SHORT_DESC AS DEPT," &
                " CAST(TMS214.CLASS_ID AS VARCHAR(4)) || ' - ' || TMS214.CLASS_LONG_DESC AS Class," &
                " TSS100.DEPT_ID, TSS100.CLASS_ID, TSS100.VENDOR_ID, " &
                " TSS100.INTERNAL_STYLE_NUM," &
                " TSS200.CLR_CDE, " &
                " TSS200.SKU_NUM, " &
                " TSS200.UPC_NUM " &
                " ,DATE((SELECT MIN(DATE_MAINT_TS)  FROM " &
                " {0}.TSS300ISN_SKU_AUD WHERE SKU_NUM = TSS200.SKU_NUM " &
                " AND INTERNAL_STYLE_NUM = TSS200.INTERNAL_STYLE_NUM " &
                " AND DATE(DATE_MAINT_TS) > '0001-01-01')) AS SS_DATE " &
                " ,DATE(TTU450.SMPL_REQ_CRTE_TS) AS SAMPLE_REQ_DATE " &
                " ,CAST(TTU450.CMR_CHECK_IN_DTE AS VARCHAR(10)) AS CMR_DATE, " &
                " TTU450.SAMPLE_DUE_DTE AS SAMPLE_DUE_DATE, " &
                " COALESCE(TTU450.SMPL_PRIM_LOC_NME,'') AS SMPL_PRIM_LOC_NME, " &
                " DATE(TTU450.LAST_MOD_TS) AS SAMPLE_STATUS_DATE, " &
                " COALESCE(TTU450.SAMPLE_STATUS_DESC, '') AS SAMPLE_STATUS_DESC, " &
                " (SELECT MIN(DATE(TPO105.PO_TRACK_TS)) FROM " &
                " {0}.TPO105PROCESS_LOG AS TPO105 " &
                " WHERE TPO105.PO_ID = PurchaseOrder.PO_ID " &
                " AND TPO105.PO_TRACK_CDE IN ('AP') " &
                " AND DATE(TPO105.PO_TRACK_TS) > '0001-01-01') AS PO_APPROVAL_DTE, " &
                " (SELECT MIN(DATE(TPO100.DC_SHIP_DTE)) FROM " &
                " {0}.TPO100SUMMARY AS TPO100 " &
                " WHERE TPO100.PO_ID = PurchaseOrder.PO_ID " &
                " AND TPO100.DC_SHIP_DTE > '0001-01-01') AS DC_SHIP_DTE," &
                " (SELECT WCAT_LOAD_STAT_DTE FROM {0}.TTU600WEBCAT_STAGE " &
                " WHERE UPC_NUM = TSS200.UPC_NUM AND WCAT_LOAD_STAT_FLG = 'U' FETCH FIRST 1 ROWS ONLY) AS TURN_IN_DATE," &
                " (SELECT DATE(MIN(RECEIPT_CREATE_TS)) FROM" &
                " {0}.TDL101RECEIPT_DTL  WHERE PO_ID = PurchaseOrder.PO_ID) AS FIRST_RC_DATE," &
                " CAST(TEC120.COPY_READY_DTE AS VARCHAR(10)) AS COPY_READY_DATE," &
                " CAST(TEC120.PRODUCT_READY_DTE AS VARCHAR(10)) AS PRODUCT_READY_DATE," &
                " CAST(TEC120.ACTIVE_DTE AS VARCHAR(10)) AS PRODUCT_ACTIVE_DATE,    " &
                " OH.SKU_LOC_CUR_OH_QTY AS QUANTITY, " &
                " COALESCE((SELECT OWN_PRICE_AMT FROM {0}.TPM190CUR_SKU_PRC " &
                " WHERE SKU_NUM = TSS200.SKU_NUM FETCH FIRST 1 ROWS ONLY),0.0) AS OWN_PRICE_AMT,  " &
                " COALESCE(TTU300.ADMIN_IMAGE_NUM,0) AS ADMIN_IMAGE_NUM, " &
                " COALESCE(TTU110.AD_NUM,0) AS AD_NUM, " &
                " TEC150.ACTIVE_DTE AS SKU_ACTIVE_DATE, " &
                " 'OH' AS REPORT_ITEM_TYPE " &
                " FROM {0}.VUI100SKU_LOC_OH AS OH " &
                " INNER JOIN {0}.TSS200SKU AS TSS200 " &
                " ON OH.SKU_NUM = TSS200.SKU_NUM" &
                " INNER JOIN {0}.TSS100ISN AS TSS100" &
                " ON TSS200.INTERNAL_STYLE_NUM = TSS100.INTERNAL_STYLE_NUM" &
                " INNER JOIN {0}.TMS213DEPARTMENT AS TMS213" &
                " ON TSS100.DEPT_ID = TMS213.DEPT_ID" &
                " INNER JOIN {0}.TMS208BUYER AS TMS208 " &
                " ON TMS213.BUYER_ID = TMS208.BUYER_ID " &
                " INNER JOIN {0}.TMS207DMM AS TMS207 " &
                " ON TMS208.DMM_ID = TMS207.DMM_ID " &
                " INNER JOIN {0}.TMS206GMM AS TMS206 " &
                " ON TMS207.GMM_ID = TMS206.GMM_ID " &
                " INNER JOIN {0}.TMS212FOB AS TMS212" &
                " ON TMS213.FOB_ID = TMS212.FOB_ID" &
                " INNER JOIN {0}.TMS214DEPT_CLASS AS TMS214" &
                " ON TSS100.CLASS_ID = TMS214.CLASS_ID" &
                " AND TSS100.DEPT_ID = TMS214.DEPT_ID" &
                " LEFT OUTER JOIN {0}.TTU450SAMPLE_REQ AS TTU450" &
                " ON TSS200.INTERNAL_STYLE_NUM = TTU450.INTERNAL_STYLE_NUM" &
                " AND TSS200.CLR_CDE = TTU450.CLR_CDE" &
                " AND UPPER(SAMPLE_STATUS_DESC) NOT IN ('RETURNED','DISPOSED','MISSING')" &
                " LEFT OUTER JOIN {0}.TTU410MERCHANDISE AS TTU410" &
                " ON TSS200.INTERNAL_STYLE_NUM = TTU410.INTERNAL_STYLE_NUM" &
                " AND TSS200.CLR_CDE = TTU410.CLR_CDE" &
                " LEFT OUTER JOIN {0}.TTU400MDSE_INSTRCT AS TTU400 " &
                " ON TTU410.TURN_IN_MDSE_ID = TTU400.TURN_IN_MDSE_ID " &
                " LEFT OUTER JOIN {0}.TTU300IMGE_REQUEST AS TTU300 " &
                " ON TTU400.TURNIN_IMGE_REQ_ID = TTU300.TURNIN_IMGE_REQ_ID " &
                " LEFT OUTER JOIN {0}.TTU110AD_POSITION AS TTU110 " &
                " ON TTU400.TURNIN_IMGE_REQ_ID = TTU110.TURNIN_IMGE_REQ_ID" &
                " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150" &
                " ON TSS200.UPC_NUM = TEC150.UPC_NUM " &
                " AND TEC150.ACTIVE_FLG IN ('D')" &
                " LEFT OUTER JOIN {0}.TEC120_PRODUCT AS TEC120" &
                " ON TEC150.PRODUCT_CDE = TEC120.PRODUCT_CDE" &
                " LEFT OUTER JOIN (SELECT VUI100.SKU_NUM, MIN(TPO107.PO_ID) AS PO_ID FROM " &
                " {0}.VUI100SKU_LOC_OH AS VUI100 " &
                " INNER JOIN {0}.TPO107SKU AS TPO107 " &
                " ON VUI100.SKU_NUM = TPO107.SKU_NUM" &
                " WHERE VUI100.LOC_ID IN (192,193,195)" &
                " AND VUI100.SKU_LOC_CUR_OH_QTY > 0" &
                " GROUP BY VUI100.SKU_NUM) AS PurchaseOrder " &
                " ON OH.SKU_NUM = PurchaseOrder.SKU_NUM" &
                " WHERE OH.LOC_ID IN (192,193,195)" &
                " AND OH.SKU_LOC_CUR_OH_QTY > 0" &
                " AND TEC150.UPC_NUM IS NULL " &
                " WITH UR", _dbSchema)
        Return sqlQuery
    End Function

    Private Function GetWebcatDetailsOHQuery() As String
        Dim sqlQuery As String = String.Format("WITH CTEOH AS (SELECT " &
                                               " OH.SKU_NUM, OH.LOC_ID,  " &
                                               " OH.SKU_LOC_CUR_OH_QTY AS QUANTITY, TSS200.INTERNAL_STYLE_NUM,  " &
                                               " TSS200.CLR_CDE,  TSS200.UPC_NUM, " &
                                               " DATE((SELECT MIN(DATE_MAINT_TS) AS SSDATE FROM {0}.TSS300ISN_SKU_AUD AS TSS300 " &
                                               " WHERE SKU_NUM = TSS200.SKU_NUM And TSS300.INTERNAL_STYLE_NUM = TSS200.INTERNAL_STYLE_NUM " &
                                               " AND DATE(TSS300.DATE_MAINT_TS) > '0001-01-01')) AS SS_DATE " &
                                               " FROM {0}.VUI100SKU_LOC_OH AS OH " &
                                               " INNER JOIN {0}.TSS200SKU AS TSS200 ON OH.SKU_NUM = TSS200.SKU_NUM " &
                                               " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150 " &
                                               " ON TSS200.UPC_NUM = TEC150.UPC_NUM  AND TEC150.ACTIVE_FLG IN ('D') " &
                                               " LEFT OUTER JOIN {0}.TEC120_PRODUCT AS TEC120 ON TEC150.PRODUCT_CDE = TEC120.PRODUCT_CDE " &
                                               " WHERE OH.LOC_ID IN (192,193,195) AND OH.SKU_LOC_CUR_OH_QTY > 0 AND TEC150.UPC_NUM IS NULL )" &
                                               " SELECT CTEOH.SKU_NUM, CTEOH.LOC_ID, CTEOH.QUANTITY, CTEOH.INTERNAL_STYLE_NUM," &
                                               " CTEOH.CLR_CDE,  CTEOH.UPC_NUM, CTEOH.SS_DATE, TEC150.ACTIVE_DTE AS SKU_ACTIVE_DATE," &
                                               " CAST(TEC120.COPY_READY_DTE AS VARCHAR(10)) AS COPY_READY_DATE," &
                                               " CAST(TEC120.PRODUCT_READY_DTE AS VARCHAR(10)) AS PRODUCT_READY_DATE," &
                                               " CAST(TEC120.ACTIVE_DTE AS VARCHAR(10)) AS PRODUCT_ACTIVE_DATE " &
                                               " FROM CTEOH " &
                                               " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150 " &
                                               " ON CTEOH.UPC_NUM = TEC150.UPC_NUM  AND TEC150.ACTIVE_FLG <> 'D' " &
                                               " LEFT OUTER JOIN {0}.TEC120_PRODUCT AS TEC120 ON TEC150.PRODUCT_CDE = TEC120.PRODUCT_CDE " &
                                               " WITH UR;", _dbSchema)
        Return sqlQuery
    End Function

    Private Function GetPurchaseOrderDetailsOHQuery() As String
        Dim sqlQuery As String = String.Format("WITH CTEOH AS (SELECT " &
                                               " OH.SKU_NUM, OH.LOC_ID  " &
                                               " FROM {0}.VUI100SKU_LOC_OH AS OH " &
                                               " INNER JOIN {0}.TSS200SKU AS TSS200 ON OH.SKU_NUM = TSS200.SKU_NUM " &
                                               " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150 " &
                                               " ON TSS200.UPC_NUM = TEC150.UPC_NUM  AND TEC150.ACTIVE_FLG IN ('D') " &
                                               " WHERE OH.LOC_ID IN (192,193,195) AND OH.SKU_LOC_CUR_OH_QTY > 0 AND TEC150.UPC_NUM IS NULL ) " &
                                               " ,CTEPURCHASEORDER AS (SELECT TPO107.SKU_NUM, MIN(TPO107.PO_ID) AS PO_ID " &
                                               " FROM {0}.TPO107SKU AS TPO107 " &
                                               " INNER JOIN CTEOH ON TPO107.SKU_NUM = CTEOH.SKU_NUM GROUP BY TPO107.SKU_NUM) " &
                                               " SELECT CTEPO.SKU_NUM, DATE(TPO100.DC_SHIP_DTE) AS DC_SHIP_DTE, " &
                                               " (SELECT MIN(DATE(TPO105.PO_TRACK_TS)) FROM  {0}.TPO105PROCESS_LOG AS TPO105  " &
                                               " WHERE TPO105.PO_ID = CTEPO.PO_ID AND TPO105.PO_TRACK_CDE IN ('AP')  " &
                                               " AND DATE(TPO105.PO_TRACK_TS) > '0001-01-01') AS PO_APPROVAL_DTE,  " &
                                               " (SELECT DATE(MIN(RECEIPT_CREATE_TS)) FROM {0}.TDL101RECEIPT_DTL " &
                                               " WHERE PO_ID = CTEPO.PO_ID) AS FIRST_RC_DATE " &
                                               " FROM {0}.TPO100SUMMARY AS TPO100 " &
                                               " INNER JOIN CTEPURCHASEORDER AS CTEPO ON CTEPO.PO_ID = TPO100.PO_ID " &
                                               " AND TPO100.DC_SHIP_DTE > '0001-01-01' " &
                                               " WITH UR;", _dbSchema)
        Return sqlQuery
    End Function

    Private Function GetGeneralDetailsOHQuery() As String
        Dim sqlQuery As String = String.Format("WITH CTEOH AS (SELECT " &
                                               " OH.SKU_NUM, OH.LOC_ID,  " &
                                               " TSS200.INTERNAL_STYLE_NUM,  TSS200.CLR_CDE  " &
                                               " FROM {0}.VUI100SKU_LOC_OH AS OH " &
                                               " INNER JOIN {0}.TSS200SKU AS TSS200 ON OH.SKU_NUM = TSS200.SKU_NUM " &
                                               " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150 ON TSS200.UPC_NUM = TEC150.UPC_NUM " &
                                               " AND TEC150.ACTIVE_FLG IN ('D') " &
                                               " WHERE OH.LOC_ID IN (192,193,195) AND OH.SKU_LOC_CUR_OH_QTY > 0 AND TEC150.UPC_NUM IS NULL) " &
                                               " SELECT CAST(TMS206.GMM_ID AS VARCHAR(4)) || ' - ' || TMS206.GMM_DESC AS GMM,  " &
                                               " CAST(TMS207.DMM_ID AS VARCHAR(4)) || ' - ' || TMS207.DMM_DESC AS DMM, " &
                                               " CAST(TMS208.BUYER_ID AS VARCHAR(4)) || ' - ' || TMS208.BUYER_DESC AS BUYER, " &
                                               " CAST(TMS212.FOB_ID AS VARCHAR(4)) || ' - ' || TMS212.FOB_DESC AS FOB, " &
                                               " CAST(TMS213.DEPT_ID AS VARCHAR(4)) || ' - ' || TMS213.DEPT_SHORT_DESC AS DEPT, " &
                                               " CAST(TMS214.CLASS_ID AS VARCHAR(4)) || ' - ' || TMS214.CLASS_LONG_DESC AS Class, " &
                                               " TSS100.DEPT_ID, TSS100.CLASS_ID, TSS100.VENDOR_ID, " &
                                               " TSS100.INTERNAL_STYLE_NUM FROM {0}.TSS100ISN AS TSS100 " &
                                               " INNER JOIN CTEOH ON TSS100.INTERNAL_STYLE_NUM = CTEOH.INTERNAL_STYLE_NUM " &
                                               " INNER JOIN {0}.TMS213DEPARTMENT AS TMS213 ON TSS100.DEPT_ID = TMS213.DEPT_ID " &
                                               " INNER JOIN {0}.TMS208BUYER AS TMS208  ON TMS213.BUYER_ID = TMS208.BUYER_ID  " &
                                               " INNER JOIN {0}.TMS207DMM AS TMS207  ON TMS208.DMM_ID = TMS207.DMM_ID  " &
                                               " INNER JOIN {0}.TMS206GMM AS TMS206  ON TMS207.GMM_ID = TMS206.GMM_ID  " &
                                               " INNER JOIN {0}.TMS212FOB AS TMS212 ON TMS213.FOB_ID = TMS212.FOB_ID " &
                                               " INNER JOIN {0}.TMS214DEPT_CLASS AS TMS214 " &
                                               " ON TSS100.CLASS_ID = TMS214.CLASS_ID AND TSS100.DEPT_ID = TMS214.DEPT_ID " &
                                               " WITH UR;", _dbSchema)
        Return sqlQuery
    End Function

    Private Function GetTurnInDetailsOHQuery() As String
        Dim sqlQuery As String = String.Format("WITH CTEOH AS (SELECT " &
                                               " OH.SKU_NUM, OH.LOC_ID,  " &
                                               " TSS200.INTERNAL_STYLE_NUM,  TSS200.CLR_CDE,  " &
                                               " COALESCE((SELECT SAMPLE_MERCH_ID FROM {0}.TTU450SAMPLE_REQ AS TTU450 " &
                                               " WHERE TTU450.INTERNAL_STYLE_NUM = TSS200.INTERNAL_STYLE_NUM AND TTU450.CLR_CDE = TSS200.CLR_CDE " &
                                               " AND UPPER(SAMPLE_STATUS_DESC) NOT IN ('RETURNED','DISPOSED','MISSING') FETCH FIRST 1 ROWS ONLY),0) AS SAMPLE_MERCH_ID  " &
                                               " FROM {0}.VUI100SKU_LOC_OH AS OH " &
                                               " INNER JOIN {0}.TSS200SKU AS TSS200 ON OH.SKU_NUM = TSS200.SKU_NUM " &
                                               " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150 ON TSS200.UPC_NUM = TEC150.UPC_NUM  AND TEC150.ACTIVE_FLG IN ('D') " &
                                               " WHERE OH.LOC_ID IN (192,193,195) AND OH.SKU_LOC_CUR_OH_QTY > 0 AND TEC150.UPC_NUM IS NULL) " &
                                               " SELECT CTEOH.SKU_NUM, TSS100.DEPT_ID, TSS100.CLASS_ID, TSS100.VENDOR_ID,  " &
                                               " TSS100.INTERNAL_STYLE_NUM, CTEOH.CLR_CDE,  " &
                                               " DATE(TTU450.SMPL_REQ_CRTE_TS) AS SAMPLE_REQ_DATE  ," &
                                               " CAST(TTU450.CMR_CHECK_IN_DTE AS VARCHAR(10)) AS CMR_DATE,   " &
                                               " TTU450.SAMPLE_DUE_DTE AS SAMPLE_DUE_DATE,  COALESCE(TTU450.SMPL_PRIM_LOC_NME,'') AS SMPL_PRIM_LOC_NME,  " &
                                               " DATE(TTU450.LAST_MOD_TS) AS SAMPLE_STATUS_DATE,  COALESCE(TTU450.SAMPLE_STATUS_DESC, '') AS SAMPLE_STATUS_DESC, " &
                                               " COALESCE(TTU300.ADMIN_IMAGE_NUM,0) AS ADMIN_IMAGE_NUM,  COALESCE(TTU110.AD_NUM,0) AS AD_NUM,  " &
                                                " (SELECT WCAT_LOAD_STAT_DTE FROM {0}.TTU600WEBCAT_STAGE " &
                                                " WHERE TURN_IN_MDSE_ID = TTU410.TURN_IN_MDSE_ID " &
                                                " AND WCAT_LOAD_STAT_FLG = 'U' FETCH FIRST 1 ROWS ONLY) AS TURN_IN_DATE," &
                                               " COALESCE(OWN_PRICE_AMT,0.0) AS OWN_PRICE_AMT, " &
                                               " COALESCE(TTU400.REMOVE_MDSE_FLG,'N') AS REMOVE_MDSE_FLG," &
                                               " 'OH' AS REPORT_ITEM_TYPE  " &
                                               " FROM {0}.TSS100ISN AS TSS100 " &
                                               " INNER JOIN CTEOH ON TSS100.INTERNAL_STYLE_NUM = CTEOH.INTERNAL_STYLE_NUM " &
                                               " LEFT OUTER JOIN {0}.TTU450SAMPLE_REQ AS TTU450 ON CTEOH.SAMPLE_MERCH_ID = TTU450.SAMPLE_MERCH_ID " &
                                               " LEFT OUTER JOIN {0}.TTU410MERCHANDISE AS TTU410 ON CTEOH.INTERNAL_STYLE_NUM = TTU410.INTERNAL_STYLE_NUM " &
                                               " AND CTEOH.CLR_CDE = TTU410.CLR_CDE " &
                                               " LEFT OUTER JOIN {0}.TTU400MDSE_INSTRCT AS TTU400  ON TTU410.TURN_IN_MDSE_ID = TTU400.TURN_IN_MDSE_ID  " &
                                               " LEFT OUTER JOIN {0}.TTU300IMGE_REQUEST AS TTU300  ON TTU400.TURNIN_IMGE_REQ_ID = TTU300.TURNIN_IMGE_REQ_ID  " &
                                               " LEFT OUTER JOIN {0}.TTU110AD_POSITION AS TTU110  ON TTU400.TURNIN_IMGE_REQ_ID = TTU110.TURNIN_IMGE_REQ_ID " &
                                               " INNER JOIN {0}.TPM190CUR_SKU_PRC AS TPM190 ON CTEOH.SKU_NUM = TPM190.SKU_NUM " &
                                               " WITH UR;", _dbSchema)
        Return sqlQuery
    End Function

    Private Function GetPathToSiteReportOHQuery(ByVal startDate As Date, ByVal endDate As Date) As String
        Dim sqlQuery As String = String.Format("WITH CTEOH AS (SELECT OH.SKU_NUM, OH.LOC_ID " &
                " FROM {0}.VUI100SKU_LOC_OH AS OH " &
                " INNER JOIN {0}.TSS200SKU AS TSS200 ON OH.SKU_NUM = TSS200.SKU_NUM " &
                " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150 ON TSS200.UPC_NUM = TEC150.UPC_NUM  AND TEC150.ACTIVE_FLG IN ('D') " &
                " WHERE OH.LOC_ID IN (192,193,195) AND OH.SKU_LOC_CUR_OH_QTY > 0 AND TEC150.UPC_NUM IS NULL) " &
                " ,CTEPURCHASEORDER AS (SELECT TPO107.SKU_NUM, MIN(TPO107.PO_ID) AS PO_ID " &
                " FROM {0}.TPO107SKU AS TPO107 " &
                " INNER JOIN CTEOH ON TPO107.SKU_NUM = CTEOH.SKU_NUM GROUP BY TPO107.SKU_NUM), " &
                " CTEPORECEIPT AS (SELECT DISTINCT CTEPO.SKU_NUM, DATE(TPO100.DC_SHIP_DTE) AS DC_SHIP_DTE, " &
                " (SELECT MIN(DATE(TPO105.PO_TRACK_TS)) FROM  {0}.TPO105PROCESS_LOG AS TPO105  " &
                " WHERE TPO105.PO_ID = CTEPO.PO_ID AND TPO105.PO_TRACK_CDE IN ('AP')  " &
                " AND DATE(TPO105.PO_TRACK_TS) > '0001-01-01') AS PO_APPROVAL_DTE,  " &
                " (SELECT DATE(MIN(RECEIPT_CREATE_TS)) FROM {0}.TDL101RECEIPT_DTL  WHERE PO_ID = CTEPO.PO_ID) AS FIRST_RC_DATE " &
                " FROM {0}.TPO100SUMMARY AS TPO100 " &
                " INNER JOIN CTEPURCHASEORDER AS CTEPO ON CTEPO.PO_ID = TPO100.PO_ID " &
                " AND TPO100.DC_SHIP_DTE > '0001-01-01' " &
                " INNER JOIN {0}.TDL100RECEIPT_HDR AS TDL100 " &
                " ON TDL100.PO_ID = CTEPO.PO_ID " &
                " AND DATE(RECEIPT_CREATE_TS) BETWEEN '{1}' AND '{2}') " &
                " SELECT  DISTINCT " &
                " CAST(TMS206.GMM_ID AS VARCHAR(4)) || ' - ' || TMS206.GMM_DESC AS GMM, " &
                " CAST(TMS207.DMM_ID AS VARCHAR(4)) || ' - ' || TMS207.DMM_DESC AS DMM," &
                " CAST(TMS208.BUYER_ID AS VARCHAR(4)) || ' - ' || TMS208.BUYER_DESC AS BUYER," &
                " CAST(TMS212.FOB_ID AS VARCHAR(4)) || ' - ' || TMS212.FOB_DESC AS FOB," &
                " CAST(TMS213.DEPT_ID AS VARCHAR(4)) || ' - ' || TMS213.DEPT_SHORT_DESC AS DEPT," &
                " CAST(TMS214.CLASS_ID AS VARCHAR(4)) || ' - ' || TMS214.CLASS_LONG_DESC AS Class," &
                " TSS100.DEPT_ID, TSS100.CLASS_ID, TSS100.VENDOR_ID, " &
                " TSS100.INTERNAL_STYLE_NUM," &
                " TSS200.CLR_CDE," &
                " TSS200.SKU_NUM," &
                " TSS200.UPC_NUM" &
                " ,DATE((SELECT MIN(DATE_MAINT_TS)  FROM " &
                " {0}.TSS300ISN_SKU_AUD WHERE SKU_NUM = TSS200.SKU_NUM AND INTERNAL_STYLE_NUM = TSS200.INTERNAL_STYLE_NUM " &
                " AND DATE(DATE_MAINT_TS) > '0001-01-01')) AS SS_DATE" &
                " ,DATE(TTU450.SMPL_REQ_CRTE_TS) AS SAMPLE_REQ_DATE " &
                " ,CAST(TTU450.CMR_CHECK_IN_DTE AS VARCHAR(10)) AS CMR_DATE, " &
                " TTU450.SAMPLE_DUE_DTE AS SAMPLE_DUE_DATE, " &
                " COALESCE(TTU450.SMPL_PRIM_LOC_NME,'') AS SMPL_PRIM_LOC_NME, " &
                " DATE(TTU450.LAST_MOD_TS) AS SAMPLE_STATUS_DATE, " &
                " COALESCE(TTU450.SAMPLE_STATUS_DESC, '') AS SAMPLE_STATUS_DESC, " &
                " CTEPORECEIPT.PO_APPROVAL_DTE, " &
                " CTEPORECEIPT.DC_SHIP_DTE," &
                " (SELECT WCAT_LOAD_STAT_DTE FROM {0}.TTU600WEBCAT_STAGE " &
                " WHERE TURN_IN_MDSE_ID = TTU410.TURN_IN_MDSE_ID " &
                " AND WCAT_LOAD_STAT_FLG = 'U' FETCH FIRST 1 ROWS ONLY) AS TURN_IN_DATE," &
                " CTEPORECEIPT.FIRST_RC_DATE," &
                " CAST(TEC120.COPY_READY_DTE AS VARCHAR(10)) AS COPY_READY_DATE," &
                " CAST(TEC120.PRODUCT_READY_DTE AS VARCHAR(10)) AS PRODUCT_READY_DATE," &
                " CAST(TEC120.ACTIVE_DTE AS VARCHAR(10)) AS PRODUCT_ACTIVE_DATE," &
                " COALESCE(OH.SKU_LOC_CUR_OH_QTY,0) AS QUANTITY," &
                " COALESCE((SELECT OWN_PRICE_AMT FROM {0}.TPM190CUR_SKU_PRC " &
                " WHERE SKU_NUM = TSS200.SKU_NUM FETCH FIRST 1 ROWS ONLY),0.0) AS OWN_PRICE_AMT,  " &
                " COALESCE(TTU300.ADMIN_IMAGE_NUM,0) AS ADMIN_IMAGE_NUM, " &
                " COALESCE(TTU110.AD_NUM,0) AS AD_NUM, " &
                " (SELECT ACTIVE_DTE FROM {0}.TEC150_PRODUCT_UPC WHERE UPC_NUM = TSS200.UPC_NUM " &
                " AND ACTIVE_FLG <> 'D' FETCH FIRST 1 ROWS ONLY) AS SKU_ACTIVE_DATE, " &
                " 'OH' AS REPORT_ITEM_TYPE " &
                " FROM CTEPORECEIPT " &
                " INNER JOIN {0}.VUI100SKU_LOC_OH AS OH ON OH.SKU_NUM = CTEPORECEIPT.SKU_NUM" &
                " INNER JOIN {0}.TSS200SKU AS TSS200 " &
                " ON OH.SKU_NUM = TSS200.SKU_NUM" &
                " INNER JOIN {0}.TSS100ISN AS TSS100" &
                " ON TSS200.INTERNAL_STYLE_NUM = TSS100.INTERNAL_STYLE_NUM" &
                " INNER JOIN {0}.TMS213DEPARTMENT AS TMS213" &
                " ON TSS100.DEPT_ID = TMS213.DEPT_ID" &
                " INNER JOIN {0}.TMS208BUYER AS TMS208 " &
                " ON TMS213.BUYER_ID = TMS208.BUYER_ID" &
                " INNER JOIN {0}.TMS207DMM AS TMS207 " &
                " ON TMS208.DMM_ID = TMS207.DMM_ID " &
                " INNER JOIN {0}.TMS206GMM AS TMS206 " &
                " ON TMS207.GMM_ID = TMS206.GMM_ID " &
                " INNER JOIN {0}.TMS212FOB AS TMS212" &
                " ON TMS213.FOB_ID = TMS212.FOB_ID" &
                " INNER JOIN {0}.TMS214DEPT_CLASS AS TMS214" &
                " ON TSS100.CLASS_ID = TMS214.CLASS_ID" &
                " AND TSS100.DEPT_ID = TMS214.DEPT_ID" &
                " LEFT OUTER JOIN {0}.TTU450SAMPLE_REQ AS TTU450" &
                " ON TSS200.INTERNAL_STYLE_NUM = TTU450.INTERNAL_STYLE_NUM" &
                " AND TSS200.CLR_CDE = TTU450.CLR_CDE" &
                " AND UPPER(SAMPLE_STATUS_DESC) NOT IN ('RETURNED','DISPOSED','MISSING')" &
                " LEFT OUTER JOIN {0}.TTU410MERCHANDISE AS TTU410" &
                " ON TSS200.INTERNAL_STYLE_NUM = TTU410.INTERNAL_STYLE_NUM" &
                " AND TSS200.CLR_CDE = TTU410.CLR_CDE" &
                " LEFT OUTER JOIN {0}.TTU400MDSE_INSTRCT AS TTU400 " &
                " ON TTU410.TURN_IN_MDSE_ID = TTU400.TURN_IN_MDSE_ID " &
                " LEFT OUTER JOIN {0}.TTU300IMGE_REQUEST AS TTU300 " &
                " ON TTU400.TURNIN_IMGE_REQ_ID = TTU300.TURNIN_IMGE_REQ_ID " &
                " LEFT OUTER JOIN {0}.TTU110AD_POSITION AS TTU110 " &
                " ON TTU400.TURNIN_IMGE_REQ_ID = TTU110.TURNIN_IMGE_REQ_ID" &
                " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150" &
                " ON TSS200.UPC_NUM = TEC150.UPC_NUM " &
                " AND TEC150.ACTIVE_FLG IN ('D')" &
                " LEFT OUTER JOIN {0}.TEC120_PRODUCT AS TEC120" &
                " ON TEC150.PRODUCT_CDE = TEC120.PRODUCT_CDE" &
                " WHERE OH.LOC_ID IN (192,193,195)" &
                " AND OH.SKU_LOC_CUR_OH_QTY > 0" &
                " AND TEC150.UPC_NUM IS NULL " &
                " WITH UR;",
                _dbSchema,
                startDate.ToShortDateString(),
                endDate.ToShortDateString())

        Return sqlQuery
    End Function

#End Region

#Region "ON ORDER"

    Private Function GetTimingReportSKUDetailsOOQuery() As String
        Dim sqlQuery As String = String.Format("SELECT " &
                " CAST(TMS206.GMM_ID AS VARCHAR(4)) || ' - ' || TMS206.GMM_DESC AS GMM, " &
                " CAST(TMS207.DMM_ID AS VARCHAR(4)) || ' - ' || TMS207.DMM_DESC AS DMM," &
                " CAST(TMS208.BUYER_ID AS VARCHAR(4)) || ' - ' || TMS208.BUYER_DESC AS BUYER," &
                " CAST(TMS212.FOB_ID AS VARCHAR(4)) || ' - ' || TMS212.FOB_DESC AS FOB," &
                " CAST(TMS213.DEPT_ID AS VARCHAR(4)) || ' - ' || TMS213.DEPT_SHORT_DESC AS DEPT," &
                " CAST(TMS214.CLASS_ID AS VARCHAR(4)) || ' - ' || TMS214.CLASS_LONG_DESC AS Class," &
                " TSS100.DEPT_ID, TSS100.CLASS_ID, TSS100.VENDOR_ID, " &
                " TSS100.INTERNAL_STYLE_NUM," &
                " TSS200.CLR_CDE, " &
                " TSS200.SKU_NUM, " &
                " TSS200.UPC_NUM " &
                " ,DATE((SELECT MIN(DATE_MAINT_TS)  FROM " &
                " {0}.TSS300ISN_SKU_AUD WHERE SKU_NUM = TSS200.SKU_NUM " &
                " AND INTERNAL_STYLE_NUM = TSS200.INTERNAL_STYLE_NUM AND DATE(DATE_MAINT_TS) > '0001-01-01')) AS SS_DATE " &
                " ,DATE(TTU450.SMPL_REQ_CRTE_TS) AS SAMPLE_REQ_DATE " &
                " ,CAST(TTU450.CMR_CHECK_IN_DTE AS VARCHAR(10)) AS CMR_DATE, " &
                " TTU450.SAMPLE_DUE_DTE AS SAMPLE_DUE_DATE, " &
                " COALESCE(TTU450.SMPL_PRIM_LOC_NME,'') AS SMPL_PRIM_LOC_NME, " &
                " DATE(TTU450.LAST_MOD_TS) AS SAMPLE_STATUS_DATE, " &
                " COALESCE(TTU450.SAMPLE_STATUS_DESC, '') AS SAMPLE_STATUS_DESC, " &
                " (SELECT MIN(DATE(TPO105.PO_TRACK_TS)) FROM " &
                " {0}.TPO105PROCESS_LOG AS TPO105 " &
                " WHERE TPO105.PO_ID = PurchaseOrder.PO_ID " &
                " AND TPO105.PO_TRACK_CDE IN ('AP') " &
                " AND DATE(TPO105.PO_TRACK_TS) > '0001-01-01') AS PO_APPROVAL_DTE, " &
                " TPO110.DC_SHIP_DTE, " &
                " (SELECT WCAT_LOAD_STAT_DTE FROM {0}.TTU600WEBCAT_STAGE " &
                " WHERE TURN_IN_MDSE_ID = TTU410.TURN_IN_MDSE_ID " &
                " AND WCAT_LOAD_STAT_FLG = 'U' FETCH FIRST 1 ROWS ONLY) AS TURN_IN_DATE," &
                " (SELECT DATE(MIN(RECEIPT_CREATE_TS)) FROM" &
                " {0}.TDL101RECEIPT_DTL  WHERE PO_ID = PurchaseOrder.PO_ID) AS FIRST_RC_DATE," &
                " CAST(TEC120.COPY_READY_DTE AS VARCHAR(10)) AS COPY_READY_DATE," &
                " CAST(TEC120.PRODUCT_READY_DTE AS VARCHAR(10)) AS PRODUCT_READY_DATE," &
                " CAST(TEC120.ACTIVE_DTE AS VARCHAR(10)) AS PRODUCT_ACTIVE_DATE,    " &
                " TPO110.LOC_REMAIN_OO_QTY AS QUANTITY, " &
                " COALESCE((SELECT OWN_PRICE_AMT FROM {0}.TPM190CUR_SKU_PRC " &
                " WHERE SKU_NUM = TSS200.SKU_NUM FETCH FIRST 1 ROWS ONLY),0.0) AS OWN_PRICE_AMT,  " &
                " COALESCE(TTU300.ADMIN_IMAGE_NUM,0) AS ADMIN_IMAGE_NUM, " &
                " COALESCE(TTU110.AD_NUM,0) AS AD_NUM, " &
                " TEC150.ACTIVE_DTE AS SKU_ACTIVE_DATE, " &
                " 'OO' AS REPORT_ITEM_TYPE ," &
                " COALESCE(TTU400.REMOVE_MDSE_FLG,'N') AS REMOVE_MDSE_FLG " &
                " FROM {0}.TPO110OPEN_ORDER AS TPO110" &
                " INNER JOIN {0}.TSS200SKU AS TSS200 " &
                " ON TPO110.SKU_NUM = TSS200.SKU_NUM" &
                " INNER JOIN {0}.TSS100ISN AS TSS100" &
                " ON TSS200.INTERNAL_STYLE_NUM = TSS100.INTERNAL_STYLE_NUM" &
                " INNER JOIN {0}.TMS213DEPARTMENT AS TMS213" &
                " ON TSS100.DEPT_ID = TMS213.DEPT_ID" &
                " INNER JOIN {0}.TMS208BUYER AS TMS208 " &
                " ON TMS213.BUYER_ID = TMS208.BUYER_ID " &
                " INNER JOIN {0}.TMS207DMM AS TMS207 " &
                " ON TMS208.DMM_ID = TMS207.DMM_ID " &
                " INNER JOIN {0}.TMS206GMM AS TMS206 " &
                " ON TMS207.GMM_ID = TMS206.GMM_ID " &
                " INNER JOIN {0}.TMS212FOB AS TMS212 " &
                " ON TMS213.FOB_ID = TMS212.FOB_ID" &
                " INNER JOIN {0}.TMS214DEPT_CLASS AS TMS214" &
                " ON TSS100.CLASS_ID = TMS214.CLASS_ID" &
                " AND TSS100.DEPT_ID = TMS214.DEPT_ID" &
                " LEFT OUTER JOIN {0}.TTU450SAMPLE_REQ AS TTU450" &
                " ON TSS200.INTERNAL_STYLE_NUM = TTU450.INTERNAL_STYLE_NUM" &
                " AND TSS200.CLR_CDE = TTU450.CLR_CDE " &
                " AND UPPER(TTU450.SAMPLE_STATUS_DESC) NOT IN ('RETURNED','DISPOSED','MISSING')" &
                " LEFT OUTER JOIN {0}.TTU410MERCHANDISE AS TTU410 " &
                " ON TSS200.INTERNAL_STYLE_NUM = TTU410.INTERNAL_STYLE_NUM" &
                " AND TSS200.CLR_CDE = TTU410.CLR_CDE " &
                " LEFT OUTER JOIN {0}.TTU400MDSE_INSTRCT AS TTU400 " &
                " ON TTU410.TURN_IN_MDSE_ID = TTU400.TURN_IN_MDSE_ID " &
                " LEFT OUTER JOIN {0}.TTU300IMGE_REQUEST AS TTU300 " &
                " ON TTU400.TURNIN_IMGE_REQ_ID = TTU300.TURNIN_IMGE_REQ_ID " &
                " LEFT OUTER JOIN {0}.TTU110AD_POSITION AS TTU110 " &
                " ON TTU400.TURNIN_IMGE_REQ_ID = TTU110.TURNIN_IMGE_REQ_ID" &
                " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150" &
                " ON TSS200.UPC_NUM = TEC150.UPC_NUM " &
                " AND TEC150.ACTIVE_FLG IN ('D')" &
                " LEFT OUTER JOIN {0}.TEC120_PRODUCT AS TEC120" &
                " ON TEC150.PRODUCT_CDE = TEC120.PRODUCT_CDE" &
                " LEFT OUTER JOIN (SELECT TPO110.SKU_NUM, MIN(TPO107.PO_ID) AS PO_ID FROM " &
                " {0}.TPO110OPEN_ORDER AS TPO110 " &
                " INNER JOIN {0}.TPO107SKU AS TPO107" &
                " ON TPO110.SKU_NUM = TPO107.SKU_NUM" &
                " WHERE TPO110.LOC_ID IN (192,193,195)" &
                " AND TPO110.PO_STATUS_CDE IN ('AU','AP','VR')" &
                " AND TPO110.LOC_REMAIN_OO_QTY > 0" &
                " GROUP BY TPO110.SKU_NUM) AS PurchaseOrder " &
                " ON TPO110.SKU_NUM = PurchaseOrder.SKU_NUM" &
                " WHERE TPO110.LOC_ID IN (192,193,195)" &
                " AND TPO110.PO_STATUS_CDE IN ('AU','AP','VR')" &
                " AND TPO110.LOC_REMAIN_OO_QTY > 0" &
                " AND TEC150.UPC_NUM IS NULL " &
                " FETCH FIRST 1 ROWS ONLY WITH UR;", _dbSchema)
        Return sqlQuery
    End Function

    Private Function GetWebcatDetailsOOQuery() As String
        Dim sqlQuery As String = String.Format(" WITH CTEOO AS (SELECT TPO110.SKU_NUM, " &
                                               " TPO110.LOC_ID, TPO110.LOC_REMAIN_OO_QTY AS QUANTITY, " &
                                               " TSS200.INTERNAL_STYLE_NUM, TSS200.CLR_CDE, TSS200.UPC_NUM, " &
                                               " DATE((SELECT MIN(DATE_MAINT_TS) AS SSDATE FROM {0}.TSS300ISN_SKU_AUD AS TSS300 " &
                                               " WHERE SKU_NUM = TSS200.SKU_NUM And TSS300.INTERNAL_STYLE_NUM = TSS200.INTERNAL_STYLE_NUM " &
                                               " AND DATE(TSS300.DATE_MAINT_TS) > '0001-01-01')) AS SS_DATE " &
                                               " FROM {0}.TPO110OPEN_ORDER AS TPO110 " &
                                               " INNER JOIN {0}.TSS200SKU AS TSS200 ON TPO110.SKU_NUM = TSS200.SKU_NUM " &
                                               " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150 " &
                                               " ON TSS200.UPC_NUM = TEC150.UPC_NUM  AND TEC150.ACTIVE_FLG IN ('D') " &
                                               " LEFT OUTER JOIN {0}.TEC120_PRODUCT AS TEC120 ON TEC150.PRODUCT_CDE = TEC120.PRODUCT_CDE " &
                                               " WHERE TPO110.LOC_ID IN (192,193,195) AND TPO110.PO_STATUS_CDE IN ('AU','AP','VR') " &
                                               " AND TPO110.LOC_REMAIN_OO_QTY > 0 AND TEC150.UPC_NUM IS NULL ) " &
                                               " SELECT CTEOO.SKU_NUM, " &
                                               " CTEOO.LOC_ID, CTEOO.QUANTITY, CTEOO.SS_DATE, " &
                                               " CTEOO.INTERNAL_STYLE_NUM, CTEOO.CLR_CDE, CTEOO.UPC_NUM, " &
                                               " TEC150.ACTIVE_DTE AS SKU_ACTIVE_DATE, " &
                                               " CAST(TEC120.COPY_READY_DTE AS VARCHAR(10)) AS COPY_READY_DATE, " &
                                               " CAST(TEC120.PRODUCT_READY_DTE AS VARCHAR(10)) AS PRODUCT_READY_DATE, " &
                                               " CAST(TEC120.ACTIVE_DTE AS VARCHAR(10)) AS PRODUCT_ACTIVE_DATE " &
                                               " FROM CTEOO " &
                                               " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150 " &
                                               " ON CTEOO.UPC_NUM = TEC150.UPC_NUM  AND TEC150.ACTIVE_FLG <> 'D' " &
                                               " LEFT OUTER JOIN {0}.TEC120_PRODUCT AS TEC120 ON TEC150.PRODUCT_CDE = TEC120.PRODUCT_CDE " &
                                               " WITH UR;", _dbSchema)
        Return sqlQuery
    End Function

    Private Function GetPurchaseOrderDetailsOOQuery() As String
        Dim sqlQuery As String = String.Format("WITH CTEOO AS (SELECT " &
                                               " TPO110.SKU_NUM, TPO110.LOC_ID, TSS200.INTERNAL_STYLE_NUM, " &
                                               " TSS200.CLR_CDE FROM {0}.TPO110OPEN_ORDER AS TPO110 " &
                                               " INNER JOIN {0}.TSS200SKU AS TSS200 ON TPO110.SKU_NUM = TSS200.SKU_NUM " &
                                               " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150 " &
                                               " ON TSS200.UPC_NUM = TEC150.UPC_NUM  AND TEC150.ACTIVE_FLG IN ('D') " &
                                               " WHERE TPO110.LOC_ID IN (192,193,195) AND TPO110.PO_STATUS_CDE IN ('AU','AP','VR') " &
                                               " AND TPO110.LOC_REMAIN_OO_QTY > 0 AND TEC150.UPC_NUM IS NULL )" &
                                               " ,CTEPURCHASEORDER AS (SELECT TPO107.SKU_NUM, MIN(TPO107.PO_ID) AS PO_ID " &
                                               " FROM {0}.TPO107SKU AS TPO107 " &
                                               " INNER JOIN CTEOO ON TPO107.SKU_NUM = CTEOO.SKU_NUM GROUP BY TPO107.SKU_NUM) " &
                                               " SELECT CTEPO.SKU_NUM, DATE(TPO100.DC_SHIP_DTE) AS DC_SHIP_DTE, " &
                                               " (SELECT MIN(DATE(TPO105.PO_TRACK_TS)) FROM  {0}.TPO105PROCESS_LOG AS TPO105  " &
                                               " WHERE TPO105.PO_ID = CTEPO.PO_ID AND TPO105.PO_TRACK_CDE IN ('AP')  " &
                                               " AND DATE(TPO105.PO_TRACK_TS) > '0001-01-01') AS PO_APPROVAL_DTE,  " &
                                               " (SELECT DATE(MIN(RECEIPT_CREATE_TS)) FROM {0}.TDL101RECEIPT_DTL " &
                                               " WHERE PO_ID = CTEPO.PO_ID) AS FIRST_RC_DATE " &
                                               " FROM {0}.TPO100SUMMARY AS TPO100 " &
                                               " INNER JOIN CTEPURCHASEORDER AS CTEPO ON CTEPO.PO_ID = TPO100.PO_ID " &
                                               " AND TPO100.DC_SHIP_DTE > '0001-01-01' " &
                                               " WITH UR;", _dbSchema)
        Return sqlQuery
    End Function

    Private Function GetGeneralDetailsOOQuery() As String
        Dim sqlQuery As String = String.Format("WITH CTEOO AS (SELECT " &
                                               " TPO110.SKU_NUM, TPO110.LOC_ID, TSS200.INTERNAL_STYLE_NUM, " &
                                               " TSS200.CLR_CDE FROM {0}.TPO110OPEN_ORDER AS TPO110 " &
                                               " INNER JOIN {0}.TSS200SKU AS TSS200 ON TPO110.SKU_NUM = TSS200.SKU_NUM " &
                                               " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150 " &
                                               " ON TSS200.UPC_NUM = TEC150.UPC_NUM  AND TEC150.ACTIVE_FLG IN ('D') " &
                                               " WHERE TPO110.LOC_ID IN (192,193,195) AND TPO110.PO_STATUS_CDE IN ('AU','AP','VR') " &
                                               " AND TPO110.LOC_REMAIN_OO_QTY > 0 AND TEC150.UPC_NUM IS NULL )" &
                                               " SELECT CAST(TMS206.GMM_ID AS VARCHAR(4)) || ' - ' || TMS206.GMM_DESC AS GMM,  " &
                                               " CAST(TMS207.DMM_ID AS VARCHAR(4)) || ' - ' || TMS207.DMM_DESC AS DMM, " &
                                               " CAST(TMS208.BUYER_ID AS VARCHAR(4)) || ' - ' || TMS208.BUYER_DESC AS BUYER, " &
                                               " CAST(TMS212.FOB_ID AS VARCHAR(4)) || ' - ' || TMS212.FOB_DESC AS FOB, " &
                                               " CAST(TMS213.DEPT_ID AS VARCHAR(4)) || ' - ' || TMS213.DEPT_SHORT_DESC AS DEPT, " &
                                               " CAST(TMS214.CLASS_ID AS VARCHAR(4)) || ' - ' || TMS214.CLASS_LONG_DESC AS Class, " &
                                               " TSS100.DEPT_ID, TSS100.CLASS_ID, TSS100.VENDOR_ID, " &
                                               " TSS100.INTERNAL_STYLE_NUM FROM {0}.TSS100ISN AS TSS100 " &
                                               " INNER JOIN CTEOO ON TSS100.INTERNAL_STYLE_NUM = CTEOO.INTERNAL_STYLE_NUM " &
                                               " INNER JOIN {0}.TMS213DEPARTMENT AS TMS213 ON TSS100.DEPT_ID = TMS213.DEPT_ID " &
                                               " INNER JOIN {0}.TMS208BUYER AS TMS208  ON TMS213.BUYER_ID = TMS208.BUYER_ID  " &
                                               " INNER JOIN {0}.TMS207DMM AS TMS207  ON TMS208.DMM_ID = TMS207.DMM_ID  " &
                                               " INNER JOIN {0}.TMS206GMM AS TMS206  ON TMS207.GMM_ID = TMS206.GMM_ID  " &
                                               " INNER JOIN {0}.TMS212FOB AS TMS212 ON TMS213.FOB_ID = TMS212.FOB_ID " &
                                               " INNER JOIN {0}.TMS214DEPT_CLASS AS TMS214 ON TSS100.CLASS_ID = TMS214.CLASS_ID " &
                                               " AND TSS100.DEPT_ID = TMS214.DEPT_ID " &
                                               " WITH UR;", _dbSchema)
        Return sqlQuery
    End Function

    Private Function GetTurnInDetailsOOQuery() As String
        Dim sqlQuery As String = String.Format("WITH CTEOO AS (SELECT " &
                                               " TPO110.SKU_NUM, TPO110.LOC_ID, TSS200.INTERNAL_STYLE_NUM, " &
                                               " TSS200.CLR_CDE, " &
                                               " COALESCE((SELECT SAMPLE_MERCH_ID FROM {0}.TTU450SAMPLE_REQ AS TTU450 " &
                                               " WHERE TTU450.INTERNAL_STYLE_NUM = TSS200.INTERNAL_STYLE_NUM AND TTU450.CLR_CDE = TSS200.CLR_CDE " &
                                               " AND UPPER(SAMPLE_STATUS_DESC) NOT IN ('RETURNED','DISPOSED','MISSING') " &
                                               " FETCH FIRST 1 ROWS ONLY),0) AS SAMPLE_MERCH_ID  " &
                                               " FROM {0}.TPO110OPEN_ORDER AS TPO110 " &
                                               " INNER JOIN {0}.TSS200SKU AS TSS200 ON TPO110.SKU_NUM = TSS200.SKU_NUM " &
                                               " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150 " &
                                               " ON TSS200.UPC_NUM = TEC150.UPC_NUM  AND TEC150.ACTIVE_FLG IN ('D') " &
                                               " WHERE TPO110.LOC_ID IN (192,193,195) AND TPO110.PO_STATUS_CDE IN ('AU','AP','VR') " &
                                               " AND TPO110.LOC_REMAIN_OO_QTY > 0 AND TEC150.UPC_NUM IS NULL )" &
                                               " SELECT CTEOO.SKU_NUM, TSS100.DEPT_ID, TSS100.CLASS_ID, TSS100.VENDOR_ID,  " &
                                               " TSS100.INTERNAL_STYLE_NUM, CTEOO.CLR_CDE,  " &
                                               " DATE(TTU450.SMPL_REQ_CRTE_TS) AS SAMPLE_REQ_DATE  ," &
                                               " CAST(TTU450.CMR_CHECK_IN_DTE AS VARCHAR(10)) AS CMR_DATE,   " &
                                               " TTU450.SAMPLE_DUE_DTE AS SAMPLE_DUE_DATE,  " &
                                               " COALESCE(TTU450.SMPL_PRIM_LOC_NME,'') AS SMPL_PRIM_LOC_NME,  " &
                                               " DATE(TTU450.LAST_MOD_TS) AS SAMPLE_STATUS_DATE,  " &
                                               " COALESCE(TTU450.SAMPLE_STATUS_DESC, '') AS SAMPLE_STATUS_DESC, " &
                                               " COALESCE(TTU300.ADMIN_IMAGE_NUM,0) AS ADMIN_IMAGE_NUM,  " &
                                               " COALESCE(TTU110.AD_NUM,0) AS AD_NUM,  " &
                                                " (SELECT WCAT_LOAD_STAT_DTE FROM {0}.TTU600WEBCAT_STAGE " &
                                                " WHERE TURN_IN_MDSE_ID = TTU410.TURN_IN_MDSE_ID " &
                                                " AND WCAT_LOAD_STAT_FLG = 'U' FETCH FIRST 1 ROWS ONLY) AS TURN_IN_DATE," &
                                               " COALESCE(OWN_PRICE_AMT,0.0) AS OWN_PRICE_AMT, " &
                                               " COALESCE(TTU400.REMOVE_MDSE_FLG,'N') AS REMOVE_MDSE_FLG," &
                                               " 'OO' AS REPORT_ITEM_TYPE  " &
                                               " FROM {0}.TSS100ISN AS TSS100 " &
                                               " INNER JOIN CTEOO ON TSS100.INTERNAL_STYLE_NUM = CTEOO.INTERNAL_STYLE_NUM " &
                                               " LEFT OUTER JOIN {0}.TTU450SAMPLE_REQ AS TTU450 ON CTEOO.SAMPLE_MERCH_ID = TTU450.SAMPLE_MERCH_ID " &
                                               " LEFT OUTER JOIN {0}.TTU410MERCHANDISE AS TTU410 ON CTEOO.INTERNAL_STYLE_NUM = TTU410.INTERNAL_STYLE_NUM " &
                                               " AND CTEOO.CLR_CDE = TTU410.CLR_CDE " &
                                               " LEFT OUTER JOIN {0}.TTU400MDSE_INSTRCT AS TTU400  ON TTU410.TURN_IN_MDSE_ID = TTU400.TURN_IN_MDSE_ID  " &
                                               " LEFT OUTER JOIN {0}.TTU300IMGE_REQUEST AS TTU300  ON TTU400.TURNIN_IMGE_REQ_ID = TTU300.TURNIN_IMGE_REQ_ID  " &
                                               " LEFT OUTER JOIN {0}.TTU110AD_POSITION AS TTU110  ON TTU400.TURNIN_IMGE_REQ_ID = TTU110.TURNIN_IMGE_REQ_ID " &
                                               " INNER JOIN {0}.TPM190CUR_SKU_PRC AS TPM190 ON CTEOO.SKU_NUM = TPM190.SKU_NUM " &
                                               " WITH UR;", _dbSchema)
        Return sqlQuery
    End Function


#End Region


    Private Function GetThisWeekINFCReceiptQuery(ByVal startDate As Date, ByVal endDate As Date) As String
        Dim sqlQuery As String = String.Format("SELECT DISTINCT VUI100.SKU_NUM FROM " &
                                               " {0}.TPO107SKU AS TPO107" &
                                               " INNER JOIN {0}.TDL101RECEIPT_DTL AS TDL101 " &
                                               " ON TPO107.PO_ID = TDL101.PO_ID " &
                                               " INNER JOIN {0}.VUI100SKU_LOC_OH AS VUI100 " &
                                               " ON TPO107.SKU_NUM = VUI100.SKU_NUM " &
                                               " WHERE DATE(RECEIPT_CREATE_TS) BETWEEN '{1}' AND '{2}' " &
                                               " AND VUI100.SKU_LOC_CUR_OH_QTY > 0 WITH UR;",
                                               _dbSchema,
                                               startDate.ToShortDateString(),
                                               endDate.ToShortDateString())

        Return sqlQuery
    End Function

    Private Function GetSSSetupToSampleRequestQuery() As String
        Dim sqlQuery As String = String.Format("SELECT " &
                                               " COALESCE(DAYS((SELECT DATE(SMPL_REQ_CRTE_TS) FROM {0}.TTU450SAMPLE_REQ " &
                                               " WHERE INTERNAL_STYLE_NUM = TSS300.INTERNAL_STYLE_NUM " &
                                               " AND UPPER(SAMPLE_STATUS_DESC) = 'REQUESTED' FETCH FIRST 1 ROWS ONLY)) " &
                                               "- DAYS(DATE(MIN(TSS300.DATE_MAINT_TS))),0) AS SAMPLE_REQ_DATE_TO_SS_DATE,  " &
                                               " TSS300.INTERNAL_STYLE_NUM FROM " &
                                               " {0}.TSS300ISN_SKU_AUD AS TSS300 " &
                                               " WHERE INTERNAL_STYLE_NUM IN (SELECT TTU450.INTERNAL_STYLE_NUM	  " &
                                               " FROM {0}.TTU450SAMPLE_REQ AS TTU450 WHERE UPPER(TTU450.SAMPLE_STATUS_DESC) = 'REQUESTED' " &
                                               " AND TTU450.SMPL_REQ_CRTE_NME LIKE 'SR%' ORDER BY TTU450.SMPL_REQ_CRTE_TS DESC " &
                                               " FETCH FIRST 200 ROWS ONLY) AND DATE(DATE_MAINT_TS) > '0001-01-01' " &
                                               " GROUP BY TSS300.INTERNAL_STYLE_NUM WITH UR;", _dbSchema)

        Return sqlQuery
    End Function

    Private Function GetSampleRequestToReceiptQuery() As String
        Dim sqlQuery As String = String.Format("SELECT COALESCE((DAYS(CMR_CHECK_IN_DTE) - " &
                                               " DAYS(DATE(SMPL_REQ_CRTE_TS))),0) AS SR_TO_RECEIPT_DAYS  " &
                                               " FROM {0}.TTU450SAMPLE_REQ AS TTU450 " &
                                               " WHERE UPPER(TTU450.SAMPLE_STATUS_DESC) = 'RECEIVED' " &
                                               " AND TTU450.SMPL_REQ_CRTE_NME LIKE 'SR%'" &
                                               " AND CMR_CHECK_IN_DTE > '0001-01-01' " &
                                               " ORDER BY CMR_CHECK_IN_DTE DESC " &
                                               " FETCH FIRST 200 ROWS ONLY WITH UR;", _dbSchema)

        Return sqlQuery
    End Function

    Private Function GetSSSetupToPOApprovalQuery() As String
        Dim sqlQuery As String = String.Format(" SELECT TPO107.INTERNAL_STYLE_NUM, " &
                                               " COALESCE((DAYS(DATE(MAX(TPO105.PO_TRACK_TS))) - DAYS((SELECT DATE(MIN(TSS300.DATE_MAINT_TS)) " &
                                               " FROM {0}.TSS300ISN_SKU_AUD AS TSS300 WHERE " &
                                               " SKU_NUM = TPO107.SKU_NUM AND " &
                                               " INTERNAL_STYLE_NUM = TPO107.INTERNAL_STYLE_NUM " &
                                               " AND DATE(DATE_MAINT_TS) > '0001-01-01'))),0) AS PO_APP_TO_SS_SETUP " &
                                               " FROM {0}.TPO105PROCESS_LOG AS TPO105 " &
                                               " INNER JOIN {0}.TPO107SKU AS TPO107 " &
                                               " ON TPO105.PO_ID = TPO107.PO_ID " &
                                               " WHERE TPO105.PO_TRACK_CDE IN ('AP') " &
                                               " AND DATE(TPO105.PO_TRACK_TS) > '0001-01-01' " &
                                               " GROUP BY TPO107.INTERNAL_STYLE_NUM, TPO107.SKU_NUM " &
                                               " ORDER BY TPO107.INTERNAL_STYLE_NUM DESC " &
                                               " FETCH FIRST 200 ROWS ONLY WITH UR;", _dbSchema)

        Return sqlQuery
    End Function

    Private Function GetSSSetupToTurnInQuery() As String
        Dim sqlQuery As String = String.Format(" WITH CTE AS (SELECT " &
                                               " DATE(TTU410.LAST_MOD_TS) AS TRNIN_DATE,  " &
                                               " TSS200.INTERNAL_STYLE_NUM, TSS200.SKU_NUM " &
                                               " FROM {0}.TTU400MDSE_INSTRCT AS TTU400 " &
                                               " INNER JOIN {0}.TTU410MERCHANDISE AS TTU410 " &
                                               " ON TTU400.TURN_IN_MDSE_ID = TTU410.TURN_IN_MDSE_ID " &
                                               " INNER JOIN {0}.TSS200SKU AS TSS200 " &
                                               " ON TTU410.INTERNAL_STYLE_NUM = TSS200.INTERNAL_STYLE_NUM " &
                                               " AND TTU410.CLR_CDE = TSS200.CLR_CDE " &
                                               " WHERE TRNIN_MDSESTAT_CDE = 'COMP' " &
                                               " AND TTU400.REMOVE_MDSE_FLG = 'N' " &
                                               " ORDER BY TTU410.LAST_MOD_TS DESC " &
                                               " FETCH FIRST 200 ROWS ONLY) " &
                                               " SELECT COALESCE((DAYS(TRNIN_DATE) - DAYS((SELECT DATE(MIN(TSS300.DATE_MAINT_TS)) " &
                                               " FROM {0}.TSS300ISN_SKU_AUD AS TSS300 " &
                                               " WHERE SKU_NUM = CTE.SKU_NUM AND INTERNAL_STYLE_NUM = CTE.INTERNAL_STYLE_NUM " &
                                               " AND DATE(DATE_MAINT_TS) > '0001-01-01'))),0) AS TRNIN_TO_SS_SETUP " &
                                               " FROM CTE WITH UR;", _dbSchema)

        Return sqlQuery
    End Function
    Private Function GetPOApprovalToTurnInQuery() As String
        Dim sqlQuery As String = String.Format(" WITH CTE AS (SELECT " &
                                               " DATE(TTU410.LAST_MOD_TS) AS TRNIN_DATE,  " &
                                               " TSS200.INTERNAL_STYLE_NUM, TSS200.SKU_NUM " &
                                               " FROM {0}.TTU400MDSE_INSTRCT AS TTU400 " &
                                               " INNER JOIN {0}.TTU410MERCHANDISE AS TTU410 " &
                                               " ON TTU400.TURN_IN_MDSE_ID = TTU410.TURN_IN_MDSE_ID " &
                                               " INNER JOIN {0}.TSS200SKU AS TSS200 " &
                                               " ON TTU410.INTERNAL_STYLE_NUM = TSS200.INTERNAL_STYLE_NUM " &
                                               " AND TTU410.CLR_CDE = TSS200.CLR_CDE " &
                                               " WHERE TRNIN_MDSESTAT_CDE = 'COMP' " &
                                               " AND TTU400.REMOVE_MDSE_FLG = 'N' " &
                                               " ORDER BY TTU410.LAST_MOD_TS DESC " &
                                               " FETCH FIRST 200 ROWS ONLY) " &
                                               " SELECT COALESCE(DAYS(TRNIN_DATE) - DAYS((SELECT DATE(MAX(TPO105.PO_TRACK_TS)) " &
                                               " FROM {0}.TPO105PROCESS_LOG AS TPO105 " &
                                               " INNER JOIN {0}.TPO107SKU AS TPO107 " &
                                               " ON TPO105.PO_ID = TPO107.PO_ID " &
                                               " WHERE TPO107.SKU_NUM = CTE.SKU_NUM " &
                                               " AND TPO105.PO_TRACK_CDE IN ('AP') " &
                                               " AND DATE(TPO105.PO_TRACK_TS) > '0001-01-01')),0) AS TRNIN_TO_PO_APPROVAL " &
                                               " FROM CTE WITH UR;", _dbSchema)

        Return sqlQuery
    End Function

    Private Function GetTurnInToSampleReceiptQuery() As String
        Dim sqlQuery As String = String.Format(" SELECT COALESCE((DAYS(DATE(TTU410.LAST_MOD_TS)) - " &
                                               " DAYS(TTU450.CMR_CHECK_IN_DTE)),0) AS TRNIN_TO_SMPL_RECEIPT " &
                                               " FROM {0}.TTU400MDSE_INSTRCT AS TTU400 " &
                                               " INNER JOIN {0}.TTU410MERCHANDISE AS TTU410 " &
                                               " ON TTU400.TURN_IN_MDSE_ID = TTU410.TURN_IN_MDSE_ID " &
                                               " INNER JOIN {0}.TTU450SAMPLE_REQ AS TTU450 " &
                                               " ON TTU410.INTERNAL_STYLE_NUM = TTU450.INTERNAL_STYLE_NUM " &
                                               " AND TTU410.CLR_CDE = TTU450.CLR_CDE " &
                                               " WHERE TRNIN_MDSESTAT_CDE = 'COMP' AND TTU400.REMOVE_MDSE_FLG = 'N' " &
                                               " AND TTU450.CMR_CHECK_IN_DTE > '0001-01-01' " &
                                               " ORDER BY TTU410.LAST_MOD_TS DESC FETCH FIRST 200 ROWS ONLY " &
                                               " WITH UR;", _dbSchema)

        Return sqlQuery
    End Function

    Private Function GetPOReceiptToTurnInQuery() As String
        Dim sqlQuery As String = String.Format(" WITH CTE AS (SELECT " &
                                               " DATE(TTU410.LAST_MOD_TS) AS TRNIN_DATE,  " &
                                               " TSS200.INTERNAL_STYLE_NUM, TSS200.SKU_NUM " &
                                               " FROM {0}.TTU400MDSE_INSTRCT AS TTU400 " &
                                               " INNER JOIN {0}.TTU410MERCHANDISE AS TTU410 " &
                                               " ON TTU400.TURN_IN_MDSE_ID = TTU410.TURN_IN_MDSE_ID " &
                                               " INNER JOIN {0}.TSS200SKU AS TSS200 " &
                                               " ON TTU410.INTERNAL_STYLE_NUM = TSS200.INTERNAL_STYLE_NUM " &
                                               " AND TTU410.CLR_CDE = TSS200.CLR_CDE " &
                                               " WHERE TRNIN_MDSESTAT_CDE = 'COMP' " &
                                               " AND TTU400.REMOVE_MDSE_FLG = 'N' " &
                                               " ORDER BY TTU410.LAST_MOD_TS DESC " &
                                               " FETCH FIRST 200 ROWS ONLY) " &
                                               " SELECT  COALESCE(DAYS(TRNIN_DATE) - " &
                                               " DAYS((SELECT DATE(MIN(RECEIPT_CREATE_TS)) FROM {0}.TDL101RECEIPT_DTL AS TDL101 " &
                                               " INNER JOIN {0}.TPO107SKU AS TPO107 " &
                                               " ON TDL101.PO_ID = TPO107.PO_ID " &
                                               " WHERE TPO107.SKU_NUM = CTE.SKU_NUM " &
                                               " AND DATE(RECEIPT_CREATE_TS) > '0001-01-01')),0) AS TRNIN_TO_PO_RECEIPT " &
                                               " FROM CTE WITH UR;", _dbSchema)

        Return sqlQuery
    End Function
    Public Function GetVendorImageEligibleDepartments() As DataTable
        Dim vendorImageTable As DataTable = Nothing
        Dim sql As String = String.Empty

        Try
            sql = String.Format("SELECT SMALL_INT_INDEX AS DEPT_ID, UCASE(CHAR_INDEX) AS SUB_CAT, INT_INDEX AS SUB_CAT_ID " &
                " FROM {0}.TMS900PARAMETER WHERE COLUMN_NAME = 'VENDOR_IMAGE_ELIGIBLE' WITH UR;", _dbSchema)
            vendorImageTable = ExecGetDataTable(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)
        Catch ex As Exception
            Throw
        End Try

        Return vendorImageTable
    End Function
End Class
