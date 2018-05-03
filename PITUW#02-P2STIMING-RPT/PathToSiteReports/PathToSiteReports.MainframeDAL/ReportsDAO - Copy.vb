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
    Public Function GetSKUDetailReport() As DataTable
        Dim sql As String = String.Empty
        Dim skuDetailTable As DataTable = Nothing

        Try
            'Execute OO Query
            sql = GetTimingReportSKUDetailsOOQuery()
            skuDetailTable = ExecGetDataTable(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)

            ''Execute OH Query
            sql = GetTimingReportSKUDetailsOHQuery()
            skuDetailTable.Merge(ExecGetDataTable(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing))

        Catch ex As Exception
            Throw ex
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
        Return ohReceiptTable
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
    Private Function GetTimingReportSKUDetailsOOQuery() As String
        Dim sqlQuery As String = String.Format("SELECT DISTINCT" &
                " CAST(TMS206.GMM_ID AS VARCHAR(4)) || ' - ' || TMS206.GMM_DESC AS GMM, " &
                " CAST(TMS207.DMM_ID AS VARCHAR(4)) || ' - ' || TMS207.DMM_DESC AS DMM," &
                " CAST(TMS208.BUYER_ID AS VARCHAR(4)) || ' - ' || TMS208.BUYER_DESC AS BUYER," &
                " CAST(TMS212.FOB_ID AS VARCHAR(4)) || ' - ' || TMS212.FOB_DESC AS FOB," &
                " CAST(TMS213.DEPT_ID AS VARCHAR(4)) || ' - ' || TMS213.DEPT_SHORT_DESC AS DEPT," &
                " CAST(TMS214.CLASS_ID AS VARCHAR(4)) || ' - ' || TMS214.CLASS_LONG_DESC AS Class," &
                " TSS100.INTERNAL_STYLE_NUM," &
                " TSS200.CLR_CDE, " &
                " TSS200.SKU_NUM, " &
                " TSS200.UPC_NUM " &
                " ,DATE((SELECT MIN(DATE_MAINT_TS)  FROM " &
                " {0}.TSS300ISN_SKU_AUD WHERE SKU_NUM = TSS200.SKU_NUM " &
                " AND INTERNAL_STYLE_NUM = TSS200.INTERNAL_STYLE_NUM AND DATE(DATE_MAINT_TS) > '0001-01-01')) AS SS_DATE " &
                " ,TTU450.SMPL_REQ_CRTE_TS AS SAMPLE_REQ_DATE " &
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
                " DATE(TTU410.LAST_MOD_TS) AS TURN_IN_DATE," &
                " (SELECT DATE(MIN(RECEIPT_CREATE_TS)) FROM" &
                " {0}.TDL101RECEIPT_DTL  WHERE PO_ID = PurchaseOrder.PO_ID) AS FIRST_RC_DATE," &
                " CAST(TEC120.COPY_READY_DTE AS VARCHAR(10)) AS COPY_READY_DATE," &
                " CAST(TEC120.PRODUCT_READY_DTE AS VARCHAR(10)) AS PRODUCT_READY_DATE," &
                " CAST(TEC120.ACTIVE_DTE AS VARCHAR(10)) AS PRODUCT_ACTIVE_DATE,    " &
                " TPO110.LOC_REMAIN_OO_QTY AS QUANTITY, " &
                " COALESCE((SELECT OWN_PRICE_AMT FROM {0}.TPM190CUR_SKU_PRC WHERE SKU_NUM = TSS200.SKU_NUM FETCH FIRST 1 ROWS ONLY),0.0) AS OWN_PRICE_AMT,  " &
                " 'OO' AS REPORT_ITEM_TYPE " &
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
                " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150" &
                " ON TSS200.UPC_NUM = TEC150.UPC_NUM " &
                " AND TEC150.ACTIVE_FLG IN ('D','R','I','A')" &
                " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150_UPC" &
                " ON TSS200.UPC_NUM = TEC150_UPC.UPC_NUM" &
                " LEFT OUTER JOIN {0}.TEC120_PRODUCT AS TEC120" &
                " ON TEC150_UPC.PRODUCT_CDE = TEC120.PRODUCT_CDE" &
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
                " FETCH FIRST 100 ROWS ONLY WITH UR;", _dbSchema)
        Return sqlQuery
    End Function
    Private Function GetTimingReportSKUDetailsOHQuery() As String
        Dim sqlQuery As String = String.Format("SELECT DISTINCT" &
                " CAST(TMS206.GMM_ID AS VARCHAR(4)) || ' - ' || TMS206.GMM_DESC AS GMM, " &
                " CAST(TMS207.DMM_ID AS VARCHAR(4)) || ' - ' || TMS207.DMM_DESC AS DMM," &
                " CAST(TMS208.BUYER_ID AS VARCHAR(4)) || ' - ' || TMS208.BUYER_DESC AS BUYER," &
                " CAST(TMS212.FOB_ID AS VARCHAR(4)) || ' - ' || TMS212.FOB_DESC AS FOB," &
                " CAST(TMS213.DEPT_ID AS VARCHAR(4)) || ' - ' || TMS213.DEPT_SHORT_DESC AS DEPT," &
                " CAST(TMS214.CLASS_ID AS VARCHAR(4)) || ' - ' || TMS214.CLASS_LONG_DESC AS Class," &
                " TSS100.INTERNAL_STYLE_NUM," &
                " TSS200.CLR_CDE, " &
                " TSS200.SKU_NUM, " &
                " TSS200.UPC_NUM " &
                " ,DATE((SELECT MIN(DATE_MAINT_TS)  FROM " &
                "{0}.TSS300ISN_SKU_AUD WHERE SKU_NUM = TSS200.SKU_NUM AND INTERNAL_STYLE_NUM = TSS200.INTERNAL_STYLE_NUM " &
                "AND DATE(DATE_MAINT_TS) > '0001-01-01')) AS SS_DATE " &
                " ,TTU450.SMPL_REQ_CRTE_TS AS SAMPLE_REQ_DATE " &
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
                " DATE(TTU410.LAST_MOD_TS) AS TURN_IN_DATE," &
                " (SELECT DATE(MIN(RECEIPT_CREATE_TS)) FROM" &
                " {0}.TDL101RECEIPT_DTL  WHERE PO_ID = PurchaseOrder.PO_ID) AS FIRST_RC_DATE," &
                " CAST(TEC120.COPY_READY_DTE AS VARCHAR(10)) AS COPY_READY_DATE," &
                " CAST(TEC120.PRODUCT_READY_DTE AS VARCHAR(10)) AS PRODUCT_READY_DATE," &
                " CAST(TEC120.ACTIVE_DTE AS VARCHAR(10)) AS PRODUCT_ACTIVE_DATE,    " &
                " OH.SKU_LOC_CUR_OH_QTY AS QUANTITY, " &
                " COALESCE((SELECT OWN_PRICE_AMT FROM {0}.TPM190CUR_SKU_PRC WHERE SKU_NUM = TSS200.SKU_NUM FETCH FIRST 1 ROWS ONLY),0.0) AS OWN_PRICE_AMT,  " &
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
                " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150" &
                " ON TSS200.UPC_NUM = TEC150.UPC_NUM " &
                " AND TEC150.ACTIVE_FLG IN ('D','R','I','A')" &
                " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150_UPC" &
                " ON TSS200.UPC_NUM = TEC150_UPC.UPC_NUM" &
                " LEFT OUTER JOIN {0}.TEC120_PRODUCT AS TEC120" &
                " ON TEC150_UPC.PRODUCT_CDE = TEC120.PRODUCT_CDE" &
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
                " FETCH FIRST 100 ROWS ONLY WITH UR", _dbSchema)
        Return sqlQuery
    End Function
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

    Private Function GetPathToSiteReportOHQuery(ByVal startDate As Date, ByVal endDate As Date) As String
        Dim sqlQuery As String = String.Format("SELECT DISTINCT " &
                " CAST(TMS206.GMM_ID AS VARCHAR(4)) || ' - ' || TMS206.GMM_DESC AS GMM, " &
                " CAST(TMS207.DMM_ID AS VARCHAR(4)) || ' - ' || TMS207.DMM_DESC AS DMM," &
                " CAST(TMS208.BUYER_ID AS VARCHAR(4)) || ' - ' || TMS208.BUYER_DESC AS BUYER," &
                " CAST(TMS212.FOB_ID AS VARCHAR(4)) || ' - ' || TMS212.FOB_DESC AS FOB," &
                " CAST(TMS213.DEPT_ID AS VARCHAR(4)) || ' - ' || TMS213.DEPT_SHORT_DESC AS DEPT," &
                " CAST(TMS214.CLASS_ID AS VARCHAR(4)) || ' - ' || TMS214.CLASS_LONG_DESC AS Class," &
                " TSS100.INTERNAL_STYLE_NUM," &
                " TSS200.CLR_CDE," &
                " TSS200.SKU_NUM," &
                " TSS200.UPC_NUM" &
                " ,DATE((SELECT MIN(DATE_MAINT_TS)  FROM " &
                " {0}.TSS300ISN_SKU_AUD WHERE SKU_NUM = TSS200.SKU_NUM AND INTERNAL_STYLE_NUM = TSS200.INTERNAL_STYLE_NUM " &
                " AND DATE(DATE_MAINT_TS) > '0001-01-01')) AS SS_DATE" &
                " ,TTU450.SMPL_REQ_CRTE_TS AS SAMPLE_REQ_DATE " &
                " ,CAST(TTU450.CMR_CHECK_IN_DTE AS VARCHAR(10)) AS CMR_DATE, " &
                " TTU450.SAMPLE_DUE_DTE AS SAMPLE_DUE_DATE, " &
                " COALESCE(TTU450.SMPL_PRIM_LOC_NME,'') AS SMPL_PRIM_LOC_NME, " &
                " DATE(TTU450.LAST_MOD_TS) AS SAMPLE_STATUS_DATE, " &
                " COALESCE(TTU450.SAMPLE_STATUS_DESC, '') AS SAMPLE_STATUS_DESC, " &
                " (SELECT MIN(DATE(TPO105.PO_TRACK_TS)) FROM " &
                " {0}.TPO105PROCESS_LOG AS TPO105 " &
                " WHERE TPO105.PO_ID = PurchaseOrder.PO_ID" &
                " AND TPO105.PO_TRACK_CDE IN ('AP') " &
                " AND DATE(TPO105.PO_TRACK_TS) > '0001-01-01') AS PO_APPROVAL_DTE, " &
                " (SELECT MIN(DATE(TPO100.DC_SHIP_DTE)) FROM " &
                " {0}.TPO100SUMMARY AS TPO100 " &
                " WHERE TPO100.PO_ID = PurchaseOrder.PO_ID" &
                " AND TPO100.DC_SHIP_DTE > '0001-01-01') AS DC_SHIP_DTE," &
                " DATE(TTU410.LAST_MOD_TS) AS TURN_IN_DATE," &
                " (SELECT DATE(MIN(RECEIPT_CREATE_TS)) FROM" &
                " {0}.TDL101RECEIPT_DTL  WHERE PO_ID = PurchaseOrder.PO_ID) AS FIRST_RC_DATE," &
                " CAST(TEC120.COPY_READY_DTE AS VARCHAR(10)) AS COPY_READY_DATE," &
                " CAST(TEC120.PRODUCT_READY_DTE AS VARCHAR(10)) AS PRODUCT_READY_DATE," &
                " CAST(TEC120.ACTIVE_DTE AS VARCHAR(10)) AS PRODUCT_ACTIVE_DATE," &
                " COALESCE(OH.SKU_LOC_CUR_OH_QTY,0) AS QUANTITY," &
                " COALESCE((SELECT OWN_PRICE_AMT FROM {0}.TPM190CUR_SKU_PRC WHERE SKU_NUM = TSS200.SKU_NUM FETCH FIRST 1 ROWS ONLY),0.0) AS OWN_PRICE_AMT,  " &
                " 'OH' AS REPORT_ITEM_TYPE " &
                " FROM {0}.VUI100SKU_LOC_OH AS OH " &
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
                " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150" &
                " ON TSS200.UPC_NUM = TEC150.UPC_NUM " &
                " AND TEC150.ACTIVE_FLG IN ('D','R','I','A')" &
                " LEFT OUTER JOIN {0}.TEC150_PRODUCT_UPC AS TEC150_UPC" &
                " ON TSS200.UPC_NUM = TEC150_UPC.UPC_NUM" &
                " LEFT OUTER JOIN {0}.TEC120_PRODUCT AS TEC120" &
                " ON TEC150_UPC.PRODUCT_CDE = TEC120.PRODUCT_CDE" &
                " LEFT OUTER JOIN (SELECT VUI100.SKU_NUM, MIN(TPO107.PO_ID) AS PO_ID FROM " &
                " {0}.VUI100SKU_LOC_OH AS VUI100 " &
                " INNER JOIN {0}.TPO107SKU AS TPO107" &
                " ON VUI100.SKU_NUM = TPO107.SKU_NUM" &
                " WHERE VUI100.LOC_ID IN (192,193,195)" &
                " AND VUI100.SKU_LOC_CUR_OH_QTY > 0" &
                " GROUP BY VUI100.SKU_NUM) AS PurchaseOrder " &
                " ON OH.SKU_NUM = PurchaseOrder.SKU_NUM" &
                " WHERE OH.LOC_ID IN (192,193,195)" &
                " AND OH.SKU_LOC_CUR_OH_QTY > 0" &
                " AND TEC150.UPC_NUM IS NULL " &
                " AND EXISTS(SELECT 1 FROM {0}.TDL100RECEIPT_HDR " &
                " WHERE PO_ID = PurchaseOrder.PO_ID AND LOC_ID IN (192,193,195) " &
                " AND DATE(RECEIPT_CREATE_TS) BETWEEN '{1}' AND '{2}')" &
                " FETCH FIRST 100 ROWS ONLY WITH UR;",
                _dbSchema,
                startDate.ToShortDateString(),
                endDate.ToShortDateString())

        Return sqlQuery
    End Function

    Private Function GetSSSetupToSampleRequestQuery() As String
        Dim sqlQuery As String = String.Format("SELECT " &
                                               " COALESCE((SELECT DATE(SMPL_REQ_CRTE_TS) FROM {0}.TTU450SAMPLE_REQ " &
                                               " WHERE INTERNAL_STYLE_NUM = TSS300.INTERNAL_STYLE_NUM " &
                                               " AND UPPER(SAMPLE_STATUS_DESC) = 'REQUESTED' FETCH FIRST 1 ROWS ONLY) " &
                                               "- DATE(MIN(TSS300.DATE_MAINT_TS)),0) AS SAMPLE_REQ_DATE_TO_SS_DATE,  " &
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
        Dim sqlQuery As String = String.Format("SELECT COALESCE((CMR_CHECK_IN_DTE - " &
                                               " DATE(SMPL_REQ_CRTE_TS)),0) AS SR_TO_RECEIPT_DAYS  " &
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
                                               " COALESCE((DATE(MAX(TPO105.PO_TRACK_TS)) - (SELECT DATE(MIN(TSS300.DATE_MAINT_TS)) " &
                                               " FROM {0}.TSS300ISN_SKU_AUD AS TSS300 WHERE " &
                                               " SKU_NUM = TPO107.SKU_NUM AND " &
                                               " INTERNAL_STYLE_NUM = TPO107.INTERNAL_STYLE_NUM " &
                                               " AND DATE(DATE_MAINT_TS) > '0001-01-01')),0) AS PO_APP_TO_SS_SETUP " &
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
                                               " SELECT COALESCE((TRNIN_DATE - (SELECT DATE(MIN(TSS300.DATE_MAINT_TS)) " &
                                               " FROM {0}.TSS300ISN_SKU_AUD AS TSS300 " &
                                               " WHERE SKU_NUM = CTE.SKU_NUM AND INTERNAL_STYLE_NUM = CTE.INTERNAL_STYLE_NUM " &
                                               " AND DATE(DATE_MAINT_TS) > '0001-01-01')),0) AS TRNIN_TO_SS_SETUP " &
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
                                               " SELECT COALESCE(TRNIN_DATE - (SELECT DATE(MAX(TPO105.PO_TRACK_TS)) " &
                                               " FROM {0}.TPO105PROCESS_LOG AS TPO105 " &
                                               " INNER JOIN {0}.TPO107SKU AS TPO107 " &
                                               " ON TPO105.PO_ID = TPO107.PO_ID " &
                                               " WHERE TPO107.SKU_NUM = CTE.SKU_NUM " &
                                               " AND TPO105.PO_TRACK_CDE IN ('AP') " &
                                               " AND DATE(TPO105.PO_TRACK_TS) > '0001-01-01'),0) AS TRNIN_TO_PO_APPROVAL " &
                                               " FROM CTE WITH UR;", _dbSchema)

        Return sqlQuery
    End Function

    Private Function GetTurnInToSampleReceiptQuery() As String
        Dim sqlQuery As String = String.Format(" SELECT COALESCE((DATE(TTU410.LAST_MOD_TS) - " &
                                               " TTU450.CMR_CHECK_IN_DTE),0) AS TRNIN_TO_SMPL_RECEIPT " &
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
                                               " SELECT  COALESCE(TRNIN_DATE - " &
                                               " (SELECT DATE(MIN(RECEIPT_CREATE_TS)) FROM {0}.TDL101RECEIPT_DTL AS TDL101 " &
                                               " INNER JOIN {0}.TPO107SKU AS TPO107 " &
                                               " ON TDL101.PO_ID = TPO107.PO_ID " &
                                               " WHERE TPO107.SKU_NUM = CTE.SKU_NUM " &
                                               " AND DATE(RECEIPT_CREATE_TS) > '0001-01-01'),0) AS TRNIN_TO_PO_RECEIPT " &
                                               " FROM CTE WITH UR;", _dbSchema)

        Return sqlQuery
    End Function
End Class
