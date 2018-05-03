Imports IBM.Data.DB2
Imports PathToSiteReports.BusinessEntities
Imports PathToSiteReports.Factory.Common
Public Class ReportFactory
    Public Shared Function ConstructSKUDetail(ByVal reader As DB2DataReader) As SKUDetail
        Dim skuDetailReportEntry As New SKUDetail

        With skuDetailReportEntry
            .GMM = CStr(ReadColumn(reader, "GMM"))
            .DMM = CStr(ReadColumn(reader, "DMM"))
            .BUYER = CStr(ReadColumn(reader, "BUYER"))
            .FOB = CStr(ReadColumn(reader, "FOB"))
            .DEPT = CStr(ReadColumn(reader, "DEPT"))
            .DEPT_CLASS = CStr(ReadColumn(reader, "Class"))
            .ISN = CDec(ReadColumn(reader, "INTERNAL_STYLE_NUM"))
            .Color = CInt(ReadColumn(reader, "CLR_CDE"))
            .SKU = CDec(ReadColumn(reader, "SKU_NUM"))
            .UPC = CDec(ReadColumn(reader, "UPC_NUM"))
            .SSSetupDate = CDate(ConvertToDate(ReadColumn(reader, "SS_DATE")))
            .SampleRequestDate = CDate(ConvertToDate(ReadColumn(reader, "SAMPLE_REQ_DATE")))
            .SampleReceiptDate = CDate(ConvertToDate(ReadColumn(reader, "CMR_DATE")))
            .SampleDueDate = CDate(ConvertToDate(ReadColumn(reader, "SAMPLE_DUE_DATE")))
            .SampleStatusDate = ConvertToDate(ReadColumn(reader, "SAMPLE_STATUS_DATE"))
            .SampleStatusDesc = CStr(ReadColumn(reader, "SAMPLE_STATUS_DESC"))
            .POApprovalDate = CDate(ConvertToDate(ReadColumn(reader, "PO_APPROVAL_DTE")))
            .POShipDate = CDate(ConvertToDate(ReadColumn(reader, "DC_SHIP_DTE")))
            .TurnInDate = CDate(ConvertToDate(ReadColumn(reader, "TURN_IN_DATE")))
            .POReceiptDate = CDate(ConvertToDate(ReadColumn(reader, "FIRST_RC_DATE")))
            .CopyReadyDate = CDate(ConvertToDate(ReadColumn(reader, "COPY_READY_DATE")))
            .ProductReadyDate = CDate(ConvertToDate(ReadColumn(reader, "PRODUCT_READY_DATE")))
            .ProductActiveDate = CDate(ConvertToDate(ReadColumn(reader, "PRODUCT_ACTIVE_DATE")))
            .SamplePrimaryLocationName = CStr(ReadColumn(reader, "SMPL_PRIM_LOC_NME"))
        End With

        Return skuDetailReportEntry
    End Function

    Public Shared Function ConstructSKUDetailFromDataTableReader(ByVal reader As DataTableReader) As SKUDetail
        Dim skuDetailReportEntry As New SKUDetail

        With skuDetailReportEntry
            .GMM = ConvertToString(ReadColumn(reader, "GMM"))
            .DMM = ConvertToString(ReadColumn(reader, "DMM"))
            .Buyer = ConvertToString(ReadColumn(reader, "BUYER"))
            .FOB = ConvertToString(ReadColumn(reader, "FOB"))
            .Dept = ConvertToString(ReadColumn(reader, "DEPT"))
            .Dept_Class = ConvertToString(ReadColumn(reader, "Class"))
            .ISN = CDec(ReadColumn(reader, "INTERNAL_STYLE_NUM"))
            .Color = ConvertToInt(ReadColumn(reader, "CLR_CDE"))
            .SKU = CDec(ReadColumn(reader, "SKU_NUM"))
            .UPC = CDec(ReadColumn(reader, "UPC_NUM"))
            .DeptID = ConvertToInt(ReadColumn(reader, "DEPT_ID"))
            .ClassID = ConvertToInt(ReadColumn(reader, "CLASS_ID"))
            .VendorID = ConvertToInt(ReadColumn(reader, "VENDOR_ID"))
            .SSSetupDate = ConvertToDate(ReadColumn(reader, "SS_DATE"))
            .SampleRequestDate = ConvertToDate(ReadColumn(reader, "SAMPLE_REQ_DATE"))
            .SampleReceiptDate = ConvertToDate(ReadColumn(reader, "CMR_DATE"))
            .SampleDueDate = ConvertToDate(ReadColumn(reader, "SAMPLE_DUE_DATE"))
            .SampleStatusDate = ConvertToDate(ReadColumn(reader, "SAMPLE_STATUS_DATE"))
            .SampleStatusDesc = ConvertToString(ReadColumn(reader, "SAMPLE_STATUS_DESC"))
            .POApprovalDate = ConvertToDate(ReadColumn(reader, "PO_APPROVAL_DTE"))
            .POShipDate = ConvertToDate(ReadColumn(reader, "DC_SHIP_DTE"))
            .TurnInDate = ConvertToDate(ReadColumn(reader, "TURN_IN_DATE"))
            .POReceiptDate = ConvertToDate(ReadColumn(reader, "FIRST_RC_DATE"))
            .CopyReadyDate = ConvertToDate(ReadColumn(reader, "COPY_READY_DATE"))
            .ProductReadyDate = ConvertToDate(ReadColumn(reader, "PRODUCT_READY_DATE"))
            .ProductActiveDate = ConvertToDate(ReadColumn(reader, "PRODUCT_ACTIVE_DATE"))
            .AdNumber = ConvertToInt(ReadColumn(reader, "AD_NUM"))
            .AdDesc = ConvertToString(ReadColumn(reader, "AD_DESC"))
            .AdType = ConvertToString(ReadColumn(reader, "AD_TYPE"))
            .ImageShotDate = ConvertToDate(ReadColumn(reader, "IMAGE_SHOT_DATE"))
            .FinalImageReadyDate = ConvertToDate(ReadColumn(reader, "IMAGE_READY_DATE"))
            .OWNEDPRICE = CDec(ReadColumn(reader, "OWN_PRICE_AMT"))
            .Quantity = ConvertToInt(ReadColumn(reader, "QUANTITY"))
            .ReportItemType = ConvertToString(ReadColumn(reader, "REPORT_ITEM_TYPE"))
            .AdJobStepDueDate = ConvertToDate(ReadColumn(reader, "JOB_SCHEDULE_DATE"))
            .SamplePrimaryLocationName = ConvertToString(ReadColumn(reader, "SMPL_PRIM_LOC_NME"))
            .SKUActiveDate = ConvertToDate(ReadColumn(reader, "SKU_ACTIVE_DATE"))
            .RemoveMerchandiseFlag = ConvertToString(ReadColumn(reader, "REMOVE_MDSE_FLG"))
        End With

        Return skuDetailReportEntry
    End Function

End Class
