Imports PathToSiteReports.BusinessEntities
Imports PathToSiteReports.MainframeDAL
Imports PathToSiteReports.SQLDAL
Imports PathToSiteReports.Common.Common
Imports System.Reflection

Public Class Report
    Dim adminDAO As AdminDAO = Nothing
    Dim virtualTicketDAO As VirtualTicketDAO = Nothing
    Dim reportDAO As ReportsDAO = Nothing
    Public Sub New()
        adminDAO = New AdminDAO()
        virtualTicketDAO = New VirtualTicketDAO()
        reportDAO = New ReportsDAO()
    End Sub
    Public Function GetAutoTurnInReportInfo() As List(Of SKUDetail)
        Dim skuDetails As List(Of SKUDetail) = Nothing
        Dim skuDetailsTable As DataTable = Nothing
        Dim adminData As AdminData = Nothing
        Dim vtImageDetails As List(Of VirtualTicket) = Nothing
        Dim resultsTable As DataTable = Nothing

        skuDetailsTable = Me.GetSKUDetailReport()

        If Not skuDetailsTable Is Nothing AndAlso skuDetailsTable.Rows.Count > 0 Then
            skuDetailsTable = adminDAO.UpdateAdNumberAndImageID(skuDetailsTable)

            'Update Ad Type by the ad description
            UpdateAdTypeByAdDescription(skuDetailsTable)

            skuDetailsTable = virtualTicketDAO.UpdateImageShotAndFinalImageReadyDate(skuDetailsTable)
        End If

        'Update Vendor Image from the TMS900 table
        skuDetailsTable = UpdateVendorImageAdDesc(skuDetailsTable)

        'Update ad type as not assigned for items without an ad
        skuDetailsTable = UpdateNotAssignedAdDesc(skuDetailsTable)

        skuDetails = reportDAO.GetSKUDetailsListFromDataTable(skuDetailsTable)

        skuDetails = skuDetails.FindAll(Function(a) a.SSSetupToSampleRequest >= 0 _
                                            AndAlso a.AdType.ToUpper() <> "PRINT" _
                                            AndAlso a.RemoveMerchandiseFlag = "N").ToList()

        Return skuDetails
    End Function
    Public Function GetAverageDaysFromTurnInToActive_VendorImages() As Integer
        Return adminDAO.GetAverageDaysFromTurnInToActive_TurnIn(AdType.VendorImage)
    End Function
    Public Function GetAverageDaysFromTurnInToActive_TurnIn() As Integer
        Return adminDAO.GetAverageDaysFromTurnInToActive_TurnIn(AdType.Ecommerce)
    End Function
    Public Function GetAverageDaysFromTurnInToActive_INFC() As Integer
        Return adminDAO.GetAverageDaysFromTurnInToActive_TurnIn(AdType.INFC)
    End Function
    Public Function GetAverageDaysFromTurnInToActive_LIFT() As Integer
        Return adminDAO.GetAverageDaysFromTurnInToActive_TurnIn(AdType.Lift)
    End Function
    Public Function GetAverageDaysFromTurnInToActive_ExtraHot() As Integer
        Return adminDAO.GetAverageDaysFromTurnInToActive_TurnIn(AdType.ExtraHot)
    End Function

    Public Function GetAverageDaysFromImageShotToTurnIn_VendorImages() As Integer
        Return adminDAO.GetAverageDaysFromTurnInToActive_TurnIn(AdType.VendorImage)
    End Function
    Public Function GetAverageDaysFromImageShotToTurnIn_TurnIn() As Integer
        Return adminDAO.GetAverageDaysFromImageShotToTurnIn(AdType.Ecommerce)
    End Function
    Public Function GetAverageDaysFromImageShotToTurnIn_INFC() As Integer
        Return adminDAO.GetAverageDaysFromImageShotToTurnIn(AdType.INFC)
    End Function
    Public Function GetAverageDaysFromImageShotToTurnIn_LIFT() As Integer
        Return adminDAO.GetAverageDaysFromImageShotToTurnIn(AdType.Lift)
    End Function

    Public Function GetAverageDaysFromImageShotToTurnIn_ExtraHot() As Integer
        Return adminDAO.GetAverageDaysFromImageShotToTurnIn(AdType.ExtraHot)
    End Function

    Public Function GetAverageDaysFromImageShotToFinalImageReady_VendorImages() As Integer
        Return adminDAO.GetAverageDaysFromImageShotToFinalImageReady(AdType.VendorImage)
    End Function
    Public Function GetAverageDaysFromImageShotToFinalImageReady_TurnIn() As Integer
        Return adminDAO.GetAverageDaysFromImageShotToFinalImageReady(AdType.Ecommerce)
    End Function
    Public Function GetAverageDaysFromImageShotToFinalImageReady_INFC() As Integer
        Return adminDAO.GetAverageDaysFromImageShotToFinalImageReady(AdType.INFC)
    End Function
    Public Function GetAverageDaysFromImageShotToFinalImageReady_LIFT() As Integer
        Return adminDAO.GetAverageDaysFromImageShotToFinalImageReady(AdType.Lift)
    End Function
    Public Function GetAverageDaysFromImageShotToFinalImageReady_ExtraHot() As Integer
        Return adminDAO.GetAverageDaysFromImageShotToFinalImageReady(AdType.ExtraHot)
    End Function

    Public Function GetAverageTotalPhotoTime_VendorImages() As Integer
        Return adminDAO.GetAverageTotalPhotoTime(AdType.VendorImage)
    End Function
    Public Function GetAverageTotalPhotoTime_TurnIn() As Integer
        Return adminDAO.GetAverageTotalPhotoTime(AdType.Ecommerce)
    End Function
    Public Function GetAverageTotalPhotoTime_INFC() As Integer
        Return adminDAO.GetAverageTotalPhotoTime(AdType.INFC)
    End Function
    Public Function GetAverageTotalPhotoTime_LIFT() As Integer
        Return adminDAO.GetAverageTotalPhotoTime(AdType.Lift)
    End Function
    Public Function GetAverageTotalPhotoTime_ExtraHot() As Integer
        Return adminDAO.GetAverageTotalPhotoTime(AdType.ExtraHot)
    End Function

    Public Function GetAverageDaysFromTurnInToCopyReady_VendorImages() As Integer
        Return adminDAO.GetAverageDaysFromTurnInToCopyReady(AdType.VendorImage)
    End Function
    Public Function GetAverageDaysFromTurnInToCopyReady_TurnIn() As Integer
        Return adminDAO.GetAverageDaysFromTurnInToCopyReady(AdType.Ecommerce)
    End Function
    Public Function GetAverageDaysFromTurnInToCopyReady_INFC() As Integer
        Return adminDAO.GetAverageDaysFromTurnInToCopyReady(AdType.INFC)
    End Function
    Public Function GetAverageDaysFromTurnInToCopyReady_LIFT() As Integer
        Return adminDAO.GetAverageDaysFromTurnInToCopyReady(AdType.Lift)
    End Function
    Public Function GetAverageDaysFromTurnInToCopyReady_ExtraHot() As Integer
        Return adminDAO.GetAverageDaysFromTurnInToCopyReady(AdType.ExtraHot)
    End Function

    Public Function GetSKUWithOHReceipt(ByVal startDate As Date, ByVal endDate As Date) As List(Of SKUDetail)
        Dim ohReceiptTable As DataTable = Nothing
        Dim skuDetails As List(Of SKUDetail) = Nothing

        Try
            ohReceiptTable = reportDAO.GetSKUWithOHReceipt(startDate, endDate)

            skuDetails = reportDAO.GetSKUDetailsListFromDataTable(ohReceiptTable)

            Return skuDetails
        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Sub UpdateAdTypeByAdDescription(ByRef skuDetails As DataTable)
        If Not skuDetails Is Nothing AndAlso skuDetails.Rows.Count > 0 Then

            'All records with ads
            For Each vendorImageRow As DataRow In skuDetails.Select("ad_num >0")
                vendorImageRow("ad_type") = "Print"
            Next

            'Vendor Images
            For Each vendorImageRow As DataRow In skuDetails.Select("ad_desc NOT LIKE '%LIFT%' AND ad_desc LIKE '%VS Imgs%'")
                vendorImageRow("ad_type") = "Vendor Image"
            Next

            'Turn in
            For Each vendorImageRow As DataRow In skuDetails.Select("ad_desc NOT LIKE '%LIFT%' AND ad_desc LIKE '%TI%'")
                vendorImageRow("ad_type") = "Turn In"
            Next

            'INFC
            For Each vendorImageRow As DataRow In skuDetails.Select("ad_desc NOT LIKE '%LIFT%' AND ad_desc LIKE '%TXF%'")
                vendorImageRow("ad_type") = "INFC Transfer"
            Next

            'LIFT
            For Each vendorImageRow As DataRow In skuDetails.Select("ad_desc LIKE '%LIFT%'")
                vendorImageRow("ad_type") = "LIFT"
            Next

            'Extra Hot
            For Each vendorImageRow As DataRow In skuDetails.Select("ad_desc LIKE '%EC In FC VS Images%'")
                vendorImageRow("ad_type") = "Extra Hot"
            Next

            'Q bucket Ad
            For Each vendorImageRow As DataRow In skuDetails.Select("ad_desc LIKE '%Merch Id Basket%'")
                vendorImageRow("ad_type") = "Q Bucket"
            Next

        End If
    End Sub

    Public Function GetAverageDaysFromSSSetupToSampleRequest() As Integer
        Dim averageDays As Integer = 0
        Dim resultList As List(Of Integer) = Nothing
        Try
            resultList = reportDAO.GetSSSetupToSampleRequestDays()

            If Not resultList Is Nothing AndAlso resultList.Count > 0 Then
                averageDays = resultList.Average()
            End If

        Catch ex As Exception
            Throw
        End Try

        Return averageDays
    End Function

    Public Function GetAverageDaysFromSampleRequestToReceipt() As Integer
        Dim averageDays As Integer = 0
        Dim resultList As List(Of Integer) = Nothing
        Try
            resultList = reportDAO.GetSampleRequestToReceiptDays()

            If Not resultList Is Nothing AndAlso resultList.Count > 0 Then
                averageDays = resultList.Average()
            End If

        Catch ex As Exception
            Throw
        End Try

        Return averageDays
    End Function

    Public Function GetAverageDaysFromSSSetupToPOApproval() As Integer
        Dim averageDays As Integer = 0
        Dim resultList As List(Of Integer) = Nothing
        Try
            resultList = reportDAO.GetSSSetupToPOApprovalDays()

            If Not resultList Is Nothing AndAlso resultList.Count > 0 Then
                averageDays = resultList.Average()
            End If

        Catch ex As Exception
            Throw
        End Try

        Return averageDays
    End Function

    Public Function GetAverageDaysFromSSSetupToTurnIn() As Integer
        Dim averageDays As Integer = 0
        Dim resultList As List(Of Integer) = Nothing
        Try
            resultList = reportDAO.GetSSSetupToTurnInDays()

            If Not resultList Is Nothing AndAlso resultList.Count > 0 Then
                averageDays = resultList.Average()
            End If

        Catch ex As Exception
            Throw
        End Try

        Return averageDays
    End Function

    Public Function GetAverageDaysFromPOAprovalToTurnIn() As Integer
        Dim averageDays As Integer = 0
        Dim resultList As List(Of Integer) = Nothing
        Try
            resultList = reportDAO.GetPOAprovalToTurnInDays()

            If Not resultList Is Nothing AndAlso resultList.Count > 0 Then
                averageDays = resultList.Average()
            End If

        Catch ex As Exception
            Throw
        End Try

        Return averageDays
    End Function

    Public Function GetAverageDaysFromSampleReceiptToTurnIn() As Integer
        Dim averageDays As Integer = 0
        Dim resultList As List(Of Integer) = Nothing
        Try
            resultList = reportDAO.GetSampleReceiptToTurnInDays()

            If Not resultList Is Nothing AndAlso resultList.Count > 0 Then
                averageDays = resultList.Average()
            End If

        Catch ex As Exception
            Throw
        End Try

        Return averageDays
    End Function

    Public Function GetAverageDaysFromPOReceiptToTurnIn() As Integer
        Dim averageDays As Integer = 0
        Dim resultList As List(Of Integer) = Nothing
        Try
            resultList = reportDAO.GetPOReceiptToTurnInDays()

            If Not resultList Is Nothing AndAlso resultList.Count > 0 Then
                averageDays = resultList.Average()
            End If

        Catch ex As Exception
            Throw
        End Try

        Return averageDays
    End Function

    Private Function UpdateVendorImageAdDesc(ByVal skuDetailsTable As DataTable) As DataTable
        Dim vendorImageEligibleDepartments As DataTable = reportDAO.GetVendorImageEligibleDepartments()

        'Update vendor image description by dept and vendor
        For Each vendorImageRow As DataRow In vendorImageEligibleDepartments.Select("SUB_CAT = 'VENDOR'")
            For Each skuRow As DataRow In skuDetailsTable.Select(String.Format("DEPT_ID = {0} AND VENDOR_ID = {1} AND AD_DESC = ''", vendorImageRow("DEPT_ID"), vendorImageRow("SUB_CAT_ID")))
                skuRow("ad_type") = "Vendor Image"
            Next
        Next

        Return skuDetailsTable
    End Function
    Private Function UpdateNotAssignedAdDesc(ByVal skuDetailsTable As DataTable) As DataTable

        'Update ad type as not assigned, then update it as not assigned
        For Each skuRow As DataRow In skuDetailsTable.Select("AD_TYPE = '' AND AD_DESC = ''")
            skuRow("ad_type") = "Not Assigned"
        Next

        Return skuDetailsTable
    End Function

    Private Function RemoveDuplicateSamples(ByVal skuDetails As List(Of SKUDetail), ByVal skuDetailsTable As DataTable) As DataTable
        Dim drSampleDetails As DataRow() = Nothing
        Dim skuDetailsListWithDuplicates As List(Of SKUDetail) = Nothing
        Dim reportDAO As ReportsDAO = Nothing
        Dim skuDetailItem As SKUDetail = Nothing

        reportDAO = New ReportsDAO()

        skuDetailsListWithDuplicates = skuDetails

        For Each detail As SKUDetail In skuDetails.FindAll(Function(a) (a.TurnInDate > Date.MinValue AndAlso a.SampleReceiptDate <= Date.MinValue) OrElse a.SampleRequestDate > Date.MinValue)
            skuDetailItem = skuDetailsListWithDuplicates.Find(Function(a) a.ISN = detail.ISN _
                                                                  AndAlso a.Color = detail.Color _
                                                                  AndAlso a.SampleReceiptDate > Date.MinValue _
                                                                  AndAlso a.TurnInDate > Date.MinValue)

            If Not skuDetailItem Is Nothing Then
                detail.SampleRequestDate = skuDetailItem.SampleRequestDate
                detail.SampleReceiptDate = skuDetailItem.SampleReceiptDate
                detail.SampleDueDate = skuDetailItem.SampleDueDate
                detail.SamplePrimaryLocationName = skuDetailItem.SamplePrimaryLocationName
                detail.SampleStatusDate = skuDetailItem.SampleStatusDate
                detail.SampleStatusDesc = skuDetailItem.SampleStatusDesc
            End If
        Next

        'For Each skuRow As DataRow In skuDetailsTable.Select("TURN_IN_DATE > '0001-01-01' AND (CMR_DATE = '' OR CMR_DATE = '0001-01-01')")
        '    drSampleDetails = skuDetailsWithDuplicates.Select(String.Format("INTERNAL_STYLE_NUM={0} AND CLR_CDE = {1} AND CMR_DATE>'0001-01-01' AND TURN_IN_DATE > '0001-01-01'",
        '                                                  skuRow("INTERNAL_STYLE_NUM"), skuRow("CLR_CDE")))
        '    If Not drSampleDetails Is Nothing AndAlso drSampleDetails.Count > 0 Then
        '        skuRow("SAMPLE_REQUEST_DATE") = drSampleDetails(0)("SAMPLE_REQUEST_DATE")
        '        skuRow("CMR_DATE") = drSampleDetails(0)("CMR_DATE")
        '        skuRow("SAMPLE_DUE_DATE") = drSampleDetails(0)("SAMPLE_DUE_DATE")
        '        skuRow("SMPL_PRIM_LOC_NME") = drSampleDetails(0)("SMPL_PRIM_LOC_NME")
        '        skuRow("SAMPLE_STATUS_DATE") = drSampleDetails(0)("SAMPLE_STATUS_DATE")
        '        skuRow("SAMPLE_STATUS_DESC") = drSampleDetails(0)("SAMPLE_STATUS_DESC")
        '    End If
        'Next

        'For Each skuRow As DataRow In skuDetailsTable.Select("SAMPLE_REQUEST_DATE > '0001-01-01'")
        '    drSampleDetails = skuDetailsWithDuplicates.Select(String.Format("INTERNAL_STYLE_NUM={0} AND CLR_CDE = {1} AND SAMPLE_REQUEST_DATE>'0001-01-01'",
        '                                                  skuRow("INTERNAL_STYLE_NUM"), skuRow("CLR_CDE")))
        '    If Not drSampleDetails Is Nothing AndAlso drSampleDetails.Count > 0 Then
        '        skuRow("SAMPLE_REQUEST_DATE") = drSampleDetails(0)("SAMPLE_REQUEST_DATE")
        '        skuRow("CMR_DATE") = drSampleDetails(0)("CMR_DATE")
        '        skuRow("SAMPLE_DUE_DATE") = drSampleDetails(0)("SAMPLE_DUE_DATE")
        '        skuRow("SMPL_PRIM_LOC_NME") = drSampleDetails(0)("SMPL_PRIM_LOC_NME")
        '        skuRow("SAMPLE_STATUS_DATE") = drSampleDetails(0)("SAMPLE_STATUS_DATE")
        '        skuRow("SAMPLE_STATUS_DESC") = drSampleDetails(0)("SAMPLE_STATUS_DESC")
        '    End If
        'Next

        'Dim skuDetails1 As List(Of SKUDetail) = (From detail In skuDetails
        '             Where detail.DeptID > 0
        '             Select New SKUDetail With {.DeptID = detail.DeptID, .ClassID = detail.ClassID}).Distinct.ToList()

        'Dim skuDetails2 = (From detail In skuDetails
        '     Where detail.DeptID > 0
        '     Select detail.GMM, detail.DMM, detail.Buyer, detail.FOB, detail.Dept,
        '     detail.Dept_Class, detail.INTERNAL_STYLE_NUM, detail.CLR_CDE, detail.SKU, detail.UPC, detail.SSSetupDate,
        '     detail.SampleRequestDate, detail.SampleReceiptDate, detail.SampleDueDate, detail.SamplePrimaryLocationName,
        '     detail.SampleStatusDate, detail.SampleStatusDesc, detail.POApprovalDate, detail.POShipDate,
        '     detail.POReceiptDate, detail.CopyReadyDate, detail.ProductReadyDate, detail.Quantity,
        '     detail.OwnedPrice, detail.AdNumber, detail.ReportItemType
        '     Distinct).ToList()

        skuDetails = skuDetails.Distinct.ToList()

        If Not skuDetails Is Nothing And skuDetails.Count > 0 Then
            skuDetailsTable = ConvertListToDataTable(skuDetails, skuDetailsTable)
        End If

        Return skuDetailsTable
    End Function

    Private Function ConvertListToDataTable(ByVal collectionList As List(Of SKUDetail), ByVal sourceTable As DataTable) As DataTable
        Dim outputTable As DataTable = Nothing
        Dim itemDetail As SKUDetail = Nothing
        Dim outputList() As PropertyInfo = Nothing

        outputTable = sourceTable.Clone()
        itemDetail = collectionList(0)
        outputList = itemDetail.GetType().GetProperties()


        For Each itemDetail In collectionList
            Dim dr As DataRow = outputTable.NewRow()
            For Each item In outputList
                dr("GMM") = itemDetail.GMM
                dr("DMM") = itemDetail.DMM
                dr("BUYER") = itemDetail.Buyer
                dr("FOB") = itemDetail.FOB
                dr("DEPT") = itemDetail.Dept
                dr("Class") = itemDetail.Dept_Class
                dr("DEPT_ID") = itemDetail.DeptID
                dr("CLASS_ID") = itemDetail.ClassID
                dr("VENDOR_ID") = itemDetail.VendorID
                dr("INTERNAL_STYLE_NUM") = itemDetail.ISN
                dr("CLR_CDE") = itemDetail.Color
                dr("SKU_NUM") = itemDetail.SKU
                dr("UPC_NUM") = itemDetail.UPC
                dr("SS_DATE") = itemDetail.SSSetupDate
                dr("SAMPLE_REQ_DATE") = itemDetail.SampleRequestDate
                dr("CMR_DATE") = itemDetail.SampleReceiptDate
                dr("SAMPLE_DUE_DATE") = itemDetail.SampleDueDate
                dr("SMPL_PRIM_LOC_NME") = itemDetail.SamplePrimaryLocationName
                dr("SAMPLE_STATUS_DATE") = itemDetail.SampleStatusDate
                dr("SAMPLE_STATUS_DESC") = itemDetail.SampleStatusDesc
                dr("PO_APPROVAL_DTE") = itemDetail.POApprovalDate
                dr("DC_SHIP_DTE") = itemDetail.POShipDate
                dr("TURN_IN_DATE") = itemDetail.TurnInDate
                dr("FIRST_RC_DATE") = itemDetail.POReceiptDate
                dr("COPY_READY_DATE") = itemDetail.CopyReadyDate
                dr("PRODUCT_READY_DATE") = itemDetail.ProductReadyDate
                dr("PRODUCT_ACTIVE_DATE") = itemDetail.ProductActiveDate
                dr("QUANTITY") = itemDetail.Quantity
                dr("OWN_PRICE_AMT") = itemDetail.OwnedPrice
                dr("ADMIN_IMAGE_NUM") = itemDetail.ImageID
                dr("AD_NUM") = itemDetail.AdNumber
                dr("SKU_ACTIVE_DATE") = itemDetail.SKUActiveDate
                dr("REPORT_ITEM_TYPE") = itemDetail.ReportItemType
                dr("REMOVE_MDSE_FLG") = itemDetail.RemoveMerchandiseFlag
            Next

            outputTable.Rows.Add(dr)
        Next

        Return outputTable
    End Function

    Private Function GetSKUDetailReport() As DataTable
        Dim skuDetailsTable As DataTable = Nothing
        Dim ooSKUDetailTable As DataTable = Nothing
        Dim ohWebCatDetails As DataTable = Nothing
        Dim ohPurchaseOrderDetails As DataTable = Nothing
        Dim ohGeneralDetails As DataTable = Nothing
        Dim ohTurnInDetails As DataTable = Nothing
        Dim ooWebCatDetails As DataTable = Nothing
        Dim ooPurchaseOrderDetails As DataTable = Nothing
        Dim ooGeneralDetails As DataTable = Nothing
        Dim ooTurnInDetails As DataTable = Nothing
        Dim skuDetailsCollection As List(Of SKUDetail) = Nothing
        Dim ooSKUDetailsCollection As List(Of SKUDetail) = Nothing
        Dim skuDetail As SKUDetail = Nothing
        Dim skuRow As DataRow() = Nothing

        Try

            ooWebCatDetails = reportDAO.GetWebCatDetails(False)

            ooPurchaseOrderDetails = reportDAO.GetPurchaseOrderDetails(False)

            ooGeneralDetails = reportDAO.GetGeneralDetails(False)

            ooTurnInDetails = reportDAO.GetTurnInDetails(False)

            skuDetailsCollection = CreateSKUDetailsList(ooWebCatDetails, ooPurchaseOrderDetails, ooGeneralDetails, ooTurnInDetails)

            'Read on hand details, query was timing out when I tried to get all the details in a single query
            ' so split the query into four different chunks and merge the details
            ohWebCatDetails = reportDAO.GetWebCatDetails()

            ohPurchaseOrderDetails = reportDAO.GetPurchaseOrderDetails()

            ohGeneralDetails = reportDAO.GetGeneralDetails()

            ohTurnInDetails = reportDAO.GetTurnInDetails()

            skuDetailsCollection.AddRange(CreateSKUDetailsList(ohWebCatDetails, ohPurchaseOrderDetails, ohGeneralDetails, ohTurnInDetails))

            'Get datatable structure by selecting 1 row from the database
            skuDetailsTable = reportDAO.GetOOSKUDetailReport()

            If Not skuDetailsCollection Is Nothing And skuDetailsCollection.Count > 0 Then
                skuDetailsCollection = skuDetailsCollection.Distinct.ToList()
                skuDetailsTable = ConvertListToDataTable(skuDetailsCollection, skuDetailsTable)
            End If

        Catch ex As Exception
            Throw
        End Try

        Return skuDetailsTable
    End Function

    Private Function GetDateValueFromDataRow(ByVal dr As DataRow, ByVal columnName As String) As Date
        Dim resultDate As Date = Date.MinValue

        If Not IsDBNull(dr(columnName)) Then
            resultDate = dr(columnName)
        End If

        Return resultDate
    End Function

    Private Function CreateSKUDetailsList(ByVal dtWebcatDetails As DataTable,
                                          ByVal dtPurchaseOrderDetails As DataTable,
                                          ByVal dtGeneralDetails As DataTable,
                                          ByVal dtTurnInDetails As DataTable) As List(Of SKUDetail)

        Dim skuDetail As SKUDetail = Nothing
        Dim skuRow As DataRow() = Nothing
        Dim skuDetailsCollection As List(Of SKUDetail) = New List(Of SKUDetail)

        For Each row As DataRow In dtWebcatDetails.Rows
            skuDetail = New SKUDetail()
            skuDetail.SKU = row("SKU_NUM")
            skuDetail.Quantity = row("QUANTITY")
            skuDetail.ISN = row("INTERNAL_STYLE_NUM")
            skuDetail.Color = row("CLR_CDE")
            skuDetail.UPC = row("UPC_NUM")
            skuDetail.SKUActiveDate = GetDateValueFromDataRow(row, "SKU_ACTIVE_DATE")
            skuDetail.CopyReadyDate = GetDateValueFromDataRow(row, "COPY_READY_DATE")
            skuDetail.ProductReadyDate = GetDateValueFromDataRow(row, "PRODUCT_READY_DATE")
            skuDetail.ProductActiveDate = GetDateValueFromDataRow(row, "PRODUCT_ACTIVE_DATE")
            skuDetail.SSSetupDate = GetDateValueFromDataRow(row, "SS_DATE")

            skuRow = dtPurchaseOrderDetails.Select(String.Format("SKU_NUM={0}", skuDetail.SKU))

            If Not skuRow Is Nothing AndAlso skuRow.Count > 0 Then
                skuDetail.POShipDate = GetDateValueFromDataRow(skuRow(0), "DC_SHIP_DTE")
                skuDetail.POApprovalDate = GetDateValueFromDataRow(skuRow(0), "PO_APPROVAL_DTE")
                skuDetail.POReceiptDate = GetDateValueFromDataRow(skuRow(0), "FIRST_RC_DATE")
            End If

            skuRow = dtGeneralDetails.Select(String.Format("INTERNAL_STYLE_NUM={0}", skuDetail.ISN))
            If Not skuRow Is Nothing AndAlso skuRow.Count > 0 Then
                skuDetail.GMM = skuRow(0)("GMM")
                skuDetail.DMM = skuRow(0)("DMM")
                skuDetail.Buyer = skuRow(0)("BUYER")
                skuDetail.FOB = skuRow(0)("FOB")
                skuDetail.Dept = skuRow(0)("DEPT")
                skuDetail.Dept_Class = skuRow(0)("Class")
                skuDetail.DeptID = skuRow(0)("DEPT_ID")
                skuDetail.ClassID = skuRow(0)("CLASS_ID")
                skuDetail.VendorID = skuRow(0)("VENDOR_ID")
            End If

            skuRow = dtTurnInDetails.Select(String.Format("SKU_NUM={0}", skuDetail.SKU))
            If Not skuRow Is Nothing AndAlso skuRow.Count > 0 Then
                skuDetail.SampleRequestDate = GetDateValueFromDataRow(skuRow(0), "SAMPLE_REQ_DATE")
                skuDetail.SampleReceiptDate = GetDateValueFromDataRow(skuRow(0), "CMR_DATE")
                skuDetail.SampleDueDate = GetDateValueFromDataRow(skuRow(0), "SAMPLE_DUE_DATE")
                skuDetail.SamplePrimaryLocationName = skuRow(0)("SMPL_PRIM_LOC_NME")
                skuDetail.SampleStatusDate = GetDateValueFromDataRow(skuRow(0), "SAMPLE_STATUS_DATE")
                skuDetail.SampleStatusDesc = skuRow(0)("SAMPLE_STATUS_DESC")
                skuDetail.ImageID = skuRow(0)("ADMIN_IMAGE_NUM")
                skuDetail.AdNumber = skuRow(0)("AD_NUM")
                skuDetail.TurnInDate = GetDateValueFromDataRow(skuRow(0), "TURN_IN_DATE")
                skuDetail.OwnedPrice = skuRow(0)("OWN_PRICE_AMT")
                skuDetail.ReportItemType = skuRow(0)("REPORT_ITEM_TYPE")
                skuDetail.RemoveMerchandiseFlag = skuRow(0)("REMOVE_MDSE_FLG")
            End If

            skuDetailsCollection.Add(skuDetail)
        Next

        Return skuDetailsCollection

    End Function

    Private Function ConvertToDate(ByVal inputValue As Object) As Date
        If Not IsDBNull(inputValue) AndAlso CDate(inputValue) > Date.MinValue Then
            Return CDate(inputValue)
        Else
            Return Date.MinValue
        End If

    End Function
End Class

