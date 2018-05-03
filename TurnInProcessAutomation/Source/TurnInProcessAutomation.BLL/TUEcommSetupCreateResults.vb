Imports TurnInProcessAutomation.BLL.CommonBO
Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.SqlDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient
Imports System.Xml.Linq

Public Class TUEcommSetupCreate
    Private dalDB2 As MainframeDAL.EcommSetupCreateDao = New MainframeDAL.EcommSetupCreateDao
    Private dalSQL As SqlDAL.IsTurnedInDao = New SqlDAL.IsTurnedInDao
    Private dalSQLAdInfo As SqlDAL.AdInfo = New SqlDAL.AdInfo
    Private dalSQLAdminInfo As SqlDAL.AdminDataDao = New SqlDAL.AdminDataDao
    Public Function GetAllEcommSetupCreateResultsByPOShipDate(ByVal AdNum As Integer, ByVal PageNum As Integer, ByVal DMMID As Integer, ByVal BuyerID As Integer, ByVal DepartmenttId As Int16, ByVal ClassId As Int16, _
                                                         ByVal VendorID As Integer, ByVal VendorStyleNum As List(Of String), ByVal StartShipDate As Date, ByVal IncludeOnlyApprovedItems As Boolean) As IList(Of EcommSetupCreateInfo)
        Dim VendorIdsString As String = String.Empty
        Dim VendorStylesString As String = String.Empty
        Dim DB2Results As IList(Of EcommSetupCreateInfo)
        Try
            Select Case VendorStyleNum.Count
                Case Is > 0
                    VendorStylesString = RTrim("|" & String.Join("|", VendorStyleNum) & "|".ToString())
                Case Else
                    VendorStylesString = String.Empty
            End Select

            DB2Results = dalDB2.GetAllEcommSetupCreateResultsByPOShipDate(AdNum, PageNum, DMMID, BuyerID, DepartmenttId, ClassId, VendorID, VendorStylesString, StartShipDate, IncludeOnlyApprovedItems).Take(700).ToList

            UpdateSampleAvailableAndActiveOnWebStatus(DB2Results)

            Dim eCommResults = From results In DB2Results
                                Group By results.ISN
                                Into samples = Group
                                Select samples.OrderByDescending(Function(x) x.ActiveOnWeb).ThenByDescending(Function(x) x.AvailableForTurnIn).First()

            Return eCommResults.OrderByDescending(Function(a) a.OnOrder).Cast(Of EcommSetupCreateInfo).ToList()
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function GetAllEcommSetupCreateResultsByHeirarchy(ByVal AdNum As Integer, ByVal PageNum As Integer, ByVal StatusCodes As List(Of String), ByVal DepartmenttId As Int16, ByVal ClassId As Int16, _
                                                             ByVal SubClassId As Int16, ByVal VendorID As Integer, ByVal VendorStyleNum As List(Of String), ByVal ACode1 As String, _
                                                             ByVal ACode2 As String, ByVal ACode3 As String, ByVal ACode4 As String, ByVal SellYear As Int16, ByVal SeasonId As Integer, _
                                                             ByVal CreatedSince As Date?, ByVal TurnInFilter As TUFilter) As List(Of EcommSetupCreateInfo)
        Dim VendorIdsString As String = String.Empty
        Dim VendorStylesString As String = String.Empty
        Dim DB2Results As IList(Of EcommSetupCreateInfo)
        Try
            Select Case VendorStyleNum.Count
                Case Is > 0
                    VendorStylesString = RTrim("|" & String.Join("|", VendorStyleNum) & "|".ToString())
                Case Else
                    VendorStylesString = String.Empty
            End Select
            'Select Case VendorStyleNum.Count
            '    Case Is > 0
            'Dim DB2Results As IList(Of EcommSetupCreateInfo)
            DB2Results = dalDB2.GetAllEcommSetupCreateResultsByHierarchy(AdNum, PageNum, String.Join("|", StatusCodes), DepartmenttId, ClassId, SubClassId, VendorId, VendorStylesString, ACode1, ACode2, ACode3, ACode4, SellYear, SeasonId, CreatedSince).Take(700).ToList
            '    Case Else
            '        'Dim DB2Results As IList(Of EcommSetupCreateInfo) =
            '        DB2Results = dalDB2.GetAllEcommSetupCreateResultsByHierarchyVendor(AdNum, PageNum, String.Join("|", StatusCodes), DeptId, ClassId, SubClassId, VendorId, ACode1, ACode2, ACode3, ACode4, SellYear, SeasonId, CreatedSince).Take(700).ToList()
            'End Select

            'KL added Select Case above 06/02/2015.
            'Dim DB2Results As IList(Of EcommSetupCreateInfo) = dalDB2.GetAllEcommSetupCreateResultsByHierarchy(AdNum, PageNum, String.Join("|", StatusCodes), DeptId, ClassId, SubClassId, VendorId, String.Join("|", VendorStyleNum), ACode1, ACode2, ACode3, ACode4, SellYear, SeasonId, CreatedSince).Take(700).ToList

            'Return GetISNLevelResults(DB2Results).OrderBy(Function(x) x.VendorStyleNumber).ThenBy(Function(x) x.ISN).ToList

            'GetSampleRequestsForTurnInSetup(DB2Results, False)

            ' Sets the "Available For Turn-in" and "Active on Web" value for each sample
            UpdateSampleAvailableAndActiveOnWebStatus(DB2Results)

            Dim eCommResults = From results In DB2Results
                                Group By results.ISN
                                Into samples = Group
                                Select samples.OrderByDescending(Function(x) x.ActiveOnWeb).ThenByDescending(Function(x) x.AvailableForTurnIn).First()

            Return ApplyTurnInFilters(eCommResults, TurnInFilter)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetAllEcommSetupCreateResultsByISNs(ByVal AdNum As Integer, ByVal PageNum As Integer, ByVal ISNs As List(Of String), ByVal ReserveISNs As List(Of String), ByVal TurnInFilter As TUFilter) As IList(Of EcommSetupCreateInfo)
        Try
            Dim DB2Results As IList(Of EcommSetupCreateInfo) = dalDB2.GetAllEcommSetupCreateResultsByISNs(AdNum, PageNum, String.Join(",", ISNs), String.Join(",", ReserveISNs)).Take(700).ToList
            'Return GetISNLevelResults(DB2Results).OrderBy(Function(x) x.VendorStyleNumber).ThenBy(Function(x) x.ISN).ToList

            'GetSampleRequestsForTurnInSetup(DB2Results, False)

            UpdateSampleAvailableAndActiveOnWebStatus(DB2Results)

            Dim eCommResults = From results In DB2Results
                                Group By results.ISN
                                Into samples = Group
                                Select samples.OrderByDescending(Function(x) x.ActiveOnWeb).ThenByDescending(Function(x) x.AvailableForTurnIn).First()

            Return ApplyTurnInFilters(eCommResults, TurnInFilter)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function GetISNLevelResults(ByVal DB2Results As IList(Of EcommSetupCreateInfo)) As List(Of EcommSetupCreateInfo)
        If DB2Results.Count > 0 Then
            Dim SQLResults As IList(Of IsTurnedIn) = dalSQL.GetIsTurnedIn(String.Join(",", DB2Results.Select(Function(x) CDec(x.ISN)).ToList))
            If SQLResults.Count > 0 Then
                Dim groupedSQLResultsE = From sr In SQLResults.Where(Function(x) x.TUType = "E") _
                                         Group By Key = sr.ISN _
                                         Into Group _
                                         Select Key, Group.Count()
                Dim groupedSQLResultsP = From sr In SQLResults.Where(Function(x) x.TUType = "P") _
                                        Group By Key = sr.ISN _
                                        Into Group _
                                        Select Key, Group.Count()

                Dim results = From db2r In DB2Results
                           Group Join sqlrE In groupedSQLResultsE
                           On db2r.ISN Equals sqlrE.Key
                           Into g1 = Group
                           From sqlrEGroup In g1.DefaultIfEmpty
                           Group Join sqlrP In groupedSQLResultsP
                           On db2r.ISN Equals sqlrP.Key
                           Into g2 = Group
                           From sqlrPGroup In g2.DefaultIfEmpty
                           Select db2r, sqlrEGroup, sqlrPGroup
                Dim typedresults As List(Of EcommSetupCreateInfo) = results.Select(Function(x) New EcommSetupCreateInfo With { _
                                .DeptId = x.db2r.DeptId, _
                                .ACode = x.db2r.ACode, _
                                .ColorCode = x.db2r.ColorCode, _
                                .ColorDesc = x.db2r.ColorDesc, _
                                .ISN = x.db2r.ISN, _
                                .ISNDesc = x.db2r.ISNDesc, _
                                .IsReserve = x.db2r.IsReserve, _
                                .IsTurnedInEcomm = If(x.sqlrEGroup IsNot Nothing AndAlso x.sqlrEGroup.Count > 0, "Y", "N"), _
                                .IsTurnedInPrint = If(x.sqlrPGroup IsNot Nothing AndAlso x.sqlrPGroup.Count > 0, "Y", "N"), _
                                .TurnedInEcommAdNos = String.Join(",", SQLResults.Where(Function(s) s.ISN = x.db2r.ISN And s.TUType = "E").Select(Function(t) t.AdNumber).Distinct.ToList), _
                                .TurnedInPrintAdNos = String.Join(",", SQLResults.Where(Function(s) s.ISN = x.db2r.ISN And s.TUType = "P").Select(Function(t) t.AdNumber).Distinct.ToList), _
                                .OnHand = x.db2r.OnHand, _
                                .OnOrder = x.db2r.OnOrder, _
                                .SellSeason = x.db2r.SellSeason, _
                                .SellYear = x.db2r.SellYear, _
                                .VendorId = x.db2r.VendorId, _
                                .VendorName = x.db2r.VendorName, _
                                .VendorStyleNumber = x.db2r.VendorStyleNumber}).ToList
                Return typedresults
            Else
                Return DB2Results
            End If
        Else
            Return DB2Results
        End If
    End Function

    Private Function GetColorLevelResults(ByVal DB2Results As IList(Of EcommSetupCreateInfo)) As List(Of EcommSetupCreateInfo)
        If DB2Results.Count > 0 Then
            Dim SQLResults As IList(Of IsTurnedIn) = dalSQL.GetIsTurnedIn(String.Join(",", DB2Results.Select(Function(x) CDec(x.ISN)).ToList))
            If SQLResults.Count > 0 Then


                Dim SQLResultsE = From sr In SQLResults.Where(Function(x) x.TUType = "E")
                                    Group By Key = New With {sr.ISN, sr.VendorColorCode, sr.StyleNumber}
                                    Into Group
                                    Select Key, Group.Count()

                Dim SQLResultsP = From sr In SQLResults.Where(Function(x) x.TUType = "P")
                                    Group By Key = New With {sr.ISN, sr.VendorColorCode, sr.StyleNumber}
                                    Into Group
                                    Select Key, Group.Count()

                Dim results = From db2r In DB2Results
                           Group Join sqlrE In SQLResultsE
                           On db2r.ISN Equals sqlrE.Key.ISN And db2r.ColorCode Equals sqlrE.Key.VendorColorCode
                           Into g1 = Group
                           From sqlrEGroup In g1.DefaultIfEmpty
                           Group Join sqlrP In SQLResultsP
                           On db2r.ISN Equals sqlrP.Key.ISN And db2r.ColorCode Equals sqlrP.Key.VendorColorCode
                           Into g2 = Group
                           From sqlrPGroup In g2.DefaultIfEmpty
                           Select db2r, sqlrEGroup, sqlrPGroup
                Dim typedresults As List(Of EcommSetupCreateInfo) = results.Select(Function(x) New EcommSetupCreateInfo With { _
                                .ACode = x.db2r.ACode, _
                                .ColorCode = x.db2r.ColorCode, _
                                .ColorDesc = x.db2r.ColorDesc, _
                                .ISN = x.db2r.ISN, _
                                .ISNDesc = x.db2r.ISNDesc, _
                                .IsReserve = x.db2r.IsReserve, _
                                .IsTurnedInEcomm = If(x.sqlrEGroup IsNot Nothing AndAlso x.sqlrEGroup.Count > 0 AndAlso Not x.db2r.IsReserve, "Y", "N"), _
                                .IsTurnedInPrint = If(x.sqlrPGroup IsNot Nothing AndAlso x.sqlrPGroup.Count > 0 AndAlso Not x.db2r.IsReserve, "Y", "N"), _
                                .TurnedInEcommAdNos = String.Join(",", SQLResults.Where(Function(s) s.ISN = x.db2r.ISN And s.TUType = "E" And s.VendorColorCode = x.db2r.ColorCode AndAlso Not x.db2r.IsReserve).Select(Function(t) t.AdNumber).ToList), _
                                .TurnedInPrintAdNos = String.Join(",", SQLResults.Where(Function(s) s.ISN = x.db2r.ISN And s.TUType = "P" And s.VendorColorCode = x.db2r.ColorCode AndAlso Not x.db2r.IsReserve).Select(Function(t) t.AdNumber).ToList), _
                                .OnHand = x.db2r.OnHand, _
                                .OnOrder = x.db2r.OnOrder, _
                                .SellSeason = x.db2r.SellSeason, _
                                .SellYear = x.db2r.SellYear, _
                                .VendorId = x.db2r.VendorId, _
                                .VendorName = x.db2r.VendorName, _
                                .VendorStyleNumber = x.db2r.VendorStyleNumber}).ToList
                Return typedresults
            Else
                Return DB2Results
            End If
        Else
            Return DB2Results
        End If
    End Function

    Public Function GetAllEcommSetupCreateDetailByISN(ByVal ISN As Decimal, ByVal TurnInFilter As TUFilter) As List(Of EcommSetupCreateInfo)
        Try
            Dim DB2Results As List(Of EcommSetupCreateInfo) = dalDB2.GetAllEcommSetupCreateDetail(ISN)
            If DB2Results.Count = 0 Then
                Return New List(Of EcommSetupCreateInfo)
            Else
                'GetSampleRequestsForTurnInSetup(DB2Results, True)
                UpdateSampleAvailableAndActiveOnWebStatus(DB2Results)

                Dim eCommResults = From results In DB2Results
                 Group By results.ColorCode
                    Into samples = Group
                Select samples.OrderByDescending(Function(x) x.ActiveOnWeb).ThenByDescending(Function(x) x.AvailableForTurnIn).First()

                Return ApplyTurnInFilters(eCommResults, TurnInFilter)

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetISNExists(ByVal ISN As Decimal, ByVal IsReserve As Boolean) As Boolean
        Try
            Return dalDB2.GetISNExists(ISN, IsReserve)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetISNLevelDetailByISN(ByVal ISN As Decimal, ByVal IsReserve As Boolean) As EcommSetupCreateInfo
        Return dalDB2.GetISNLevelDetailByISN(ISN, IsReserve)
    End Function

    Public Sub InsertISNData(ByVal ISN As Decimal, ByVal LabelId As Integer, ByVal IsReserve As Boolean, ByVal SizeCategoryCode As String, ByVal WebCategories As List(Of WebCat), ByVal UserId As String)
        Try
            Dim WebCatXML As String = ""
            WebCatXML &= "<categories>"
            For Each wc As WebCat In WebCategories
                WebCatXML &= "<category categoryCode=""" & wc.CategoryCode & """ primaryFlag=""" & CommonBO.ConvertBooleantoYN(wc.DefaultCategoryFlag) & """ />"
            Next
            WebCatXML &= "</categories>"

            dalDB2.InsertISNData(ISN, LabelId, ConvertBooleantoYN(IsReserve), SizeCategoryCode, WebCatXML, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function InsertISNDataColorLevel(ByVal ISN As Decimal, ByVal DeptID As Integer, ByVal ColorCode As String, ByVal UsageCode As Integer, ByVal ImageName As String, ByVal AdNum As String,
                                            ByVal PageNum As String, ByVal IsReserve As Boolean, ByVal AdditionalSamplesFlag As String, ByVal ModelCategoryCode As String, ByVal VendorApprovalFlag As String,
                                            ByVal UserId As String, ByVal BatchId As Integer) As Integer
        Try
            Return dalDB2.InsertISNDataColorLevel(ISN, DeptID, ColorCode, UsageCode, ImageName, AdNum, PageNum, ConvertBooleantoYN(IsReserve), AdditionalSamplesFlag, ModelCategoryCode, VendorApprovalFlag, UserId, BatchId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function InsertISNDataColorLevel(ByVal ISN As Decimal, ByVal DeptID As Integer, ByVal ColorCode As String, ByVal UsageCode As Integer, ByVal ImageName As String, ByVal AdNum As String,
                                            ByVal PageNum As String, ByVal IsReserve As Boolean, ByVal AdditionalSamplesFlag As String, ByVal ModelCategoryCode As String, ByVal VendorApprovalFlag As String,
                                            ByVal UserId As String, ByVal BatchId As Integer, ByVal ImageCategoryCode As String, ByVal OnOffFigureCode As String, ByVal FriendlyProductDesc As String, ByRef TurnInMerchID As Integer) As Integer
        Try
            Return dalDB2.InsertISNDataColorLevel(ISN, DeptID, ColorCode, UsageCode, ImageName, AdNum, PageNum, ConvertBooleantoYN(IsReserve), AdditionalSamplesFlag, ModelCategoryCode,
                                                  VendorApprovalFlag, UserId, BatchId, ImageCategoryCode, OnOffFigureCode, FriendlyProductDesc, TurnInMerchID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Function GetEcommSetupMaintenanceResults(ByVal BatchId As Integer) As List(Of EcommSetupCreateInfo)
        Try
            Return dalDB2.GetEcommSetupMaintenanceResults(BatchId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetColorSizeResults(ByVal AdNbr As Integer, ByVal PageNbr As Short, ByVal ISNs As List(Of String), ByVal ClrCodes As List(Of String)) As IList(Of EcommSetupClrSzInfo)
        Try
            Dim DB2Results As IList(Of EcommSetupClrSzInfo) = dalDB2.GetColorSizeResults(AdNbr, PageNbr, String.Join(",", ISNs), String.Join(",", ClrCodes))

            Dim sampleRequestList As IList(Of SampleRequestInfo)

            ' When the Admin Merch Num has already been set in the TTU410 table, get the color code, color description, size ID and size description from TTU450 by AdminMerchNum
            ' Otherwise, get the color code, color description, size ID and size description from TTU450 by ISN + color code - when there is only a single match.
            For Each adMerchandise In DB2Results
                ' Get the sample description, size_id and sample_merch_id
                If (adMerchandise.AdminMerchNum <> 0) Then
                    sampleRequestList = dalDB2.GetSampleRequests(adMerchandise.AdminMerchNum, 0D, String.Empty).Where(Function(x) x.SampleApprovalFlag = "Y"c).ToList()
                Else
                    sampleRequestList = dalDB2.GetSampleRequestsForIsnAndColor(adMerchandise.ISN, adMerchandise.VendorColorCode).Where(Function(x) x.SampleApprovalFlag = "Y"c).ToList()
                End If

                If sampleRequestList.Count = 1 Then
                    Dim sampleRequest As SampleRequestInfo = sampleRequestList.First()
                    adMerchandise.SampleDescription = String.Format("{0}-{1}, {2}", sampleRequest.ColorCode, sampleRequest.ColorLongDesc, sampleRequest.SampleSizeDesc)
                    adMerchandise.SampleMerchId = sampleRequest.SampleMerchId
                    adMerchandise.SampleSize = sampleRequest.SampleSize
                End If
            Next

            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetKilledItems(ByVal BatchId As String, ByVal AdNbr As String, ByVal PageNbr As String) As IList(Of EcommSetupClrSzInfo)
        Try
            Dim DB2Results As IList(Of EcommSetupClrSzInfo) = dalDB2.GetKilledItems(BatchId, AdNbr, PageNbr)
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetSavedISNCount(ByVal ISNs As List(Of String)) As Integer
        Try
            Dim DB2Results As Integer = dalDB2.GetSavedISNCount(String.Join(",", ISNs))
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetClrSizeLocLookUp(ByVal ISN As Decimal) As List(Of ClrSizLocLookUp)
        Try
            Dim DB2Results As List(Of ClrSizLocLookUp) = dalDB2.GetClrSizeLocLookUp(ISN)
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetAllColorFamily() As List(Of ClrSizLocLookUp)
        Try
            Dim DB2Results As List(Of ClrSizLocLookUp) = dalDB2.GetAllColorFamily
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Public Function GetAllSampleSizes(ByVal ISNs As String) As List(Of ClrSizLocLookUp)
    '    Try
    '        Dim DB2Results As List(Of ClrSizLocLookUp) = dalDB2.GetAllSampleSizes(ISNs)
    '        Return DB2Results
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function

    Public Function GetRouteFrmAdLookUp(ByVal AdNbr As Integer) As List(Of RouteFrmAdInfo)
        Try
            Dim SQLResults As List(Of RouteFrmAdInfo) = dalSQLAdInfo.GetRouteFrmAdLookUp(AdNbr)
            Return SQLResults
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetUPC(ByVal MerchId As Integer, ByVal SizeId As Integer) As Decimal
        Try
            Dim DB2Results As Decimal = dalDB2.GetUPC(MerchId, SizeId)
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GenerateAdminData() As Integer
        Try
            Dim SQLResults As Integer = dalSQLAdminInfo.GenerateAdminData()
            Return SQLResults
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub DeleteColorSize(ByVal MerchId As Integer, ByVal UserId As String)
        Try
            dalDB2.DeleteColorSize(MerchId, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateColorSize(ByVal ColorSizeData As EcommSetupClrSzInfo, ByVal UserId As String)
        'Transpose the comma separated values of Color Family into XML records.
        Dim ClrFamilyXML As String = String.Empty

        ClrFamilyXML &= "<colorfamily>"
        For Each cf As String In ColorSizeData.ColorFamily.Split(","c)
            ClrFamilyXML &= "<color code=""" & cf.Trim & """ />"
        Next
        ClrFamilyXML &= "</colorfamily>"

        ColorSizeData.ColorFamily = ClrFamilyXML

        dalDB2.UpdateColorSize(ColorSizeData, UserId)

    End Sub

    Public Sub UpdateMerchId(ByVal turnInMerchId As Integer, ByVal sampleMerchId As Integer, ByVal sampleSize As Integer, ByVal userId As String)

        dalDB2.UpdateAdminMdseNumber(turnInMerchId, sampleMerchId, sampleSize, userId)

    End Sub

    Public Sub UpdateColorFamilyFlood(ByVal MerchId As Integer, ColorFamilyList As String, ByVal UserId As String)
        Try
            'Transpose the comma separated values of Color Family into XML records.
            Dim ClrFamilyXML As String = String.Empty

            ClrFamilyXML &= "<colorfamily>"
            For Each cf As String In ColorFamilyList.Split(","c)
                ClrFamilyXML &= "<color code=""" & cf.Trim & """ />"
            Next
            ClrFamilyXML &= "</colorfamily>"

            dalDB2.UpdateColorFamilyFlood(MerchId, ClrFamilyXML, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateColorSizeFlood(ByVal TurnInMerchIDs As String, ByVal ColorSizeData As EcommSetupClrSzInfo, ByVal UserId As String)
        Try
            dalDB2.UpdateColorSizeFlood(TurnInMerchIDs, ColorSizeData, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateImageTypeFlood(ByVal intTurnInMerchID As Integer, ByVal strImageType As String, ByVal UserId As String)
        Try
            dalDB2.UpdateImageTypeFlood(intTurnInMerchID, strImageType, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    'Public Sub UpdateSampleSizeFlood(ByVal MerchId As Integer, ByVal IsReserve As Char, ByVal SampleSize As String, ByVal UserId As String)
    '    Try
    '        dalDB2.UpdateSampleSizeFlood(MerchId, IsReserve, SampleSize, UserId)
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub

    Public Sub UpdatePrintLblFlg(ByVal TurnInMerchIDs As String, ByVal UserId As String)
        Try
            dalDB2.UpdatePrintLblFlg(TurnInMerchIDs, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub SubmitColorSizeData(ByVal BatchId As Integer, ByVal UserId As String)
        Try
            dalDB2.SubmitColorSizeData(BatchId, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub SubmitColorSizeDataforTIA(ByVal BatchId As Integer, ByVal UserId As String, ByVal TurnInMerchandiseID As Integer)
        Try
            dalDB2.SubmitColorSizeDataforTIA(BatchId, UserId, TurnInMerchandiseID)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function GeneratePrintBatchID(ByVal TurnInMerchIDs As String, ByVal UserId As String) As Integer
        Try
            Dim DB2Results As Integer = dalDB2.GeneratePrintBatchID(TurnInMerchIDs, UserId)
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function AddToBatch(ByVal TurnInMerchId As Integer, ByVal AdminMerchId As Integer, ByVal BatchId As Integer, ByVal AdNbr As Integer, ByVal PageNbr As Integer, ByVal UserId As String) As Integer
        Try
            'Step 1: Update the Admin (SQL Server) database.
            If AdminMerchId > 0 Then
                dalSQLAdminInfo.UpdateAdminData(AdminMerchId, AdNbr, PageNbr)
            End If

            'Step 2: Update the DB2 database.
            Return dalDB2.AddToBatch(TurnInMerchId, BatchId, AdNbr, PageNbr, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UpdateAdminStatus(ByVal AdminMerchId As Integer, ByVal AdNbr As Integer, ByVal PageNbr As Integer, ByVal Status As String) As Integer
        Try
            If AdminMerchId > 0 AndAlso AdNbr > 0 AndAlso PageNbr > 0 Then

                If Status = "Submit" Then
                    dalSQLAdminInfo.UpdateAdminData(AdminMerchId, AdNbr, PageNbr)
                Else
                    dalSQLAdminInfo.UpdateAdminStatus(AdminMerchId, AdNbr, PageNbr, Status)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    '04/14/2015 Added below. KL ?


    ''' <summary>
    ''' Retrieve sample request information from the TTU450Sample_Req table using optional parameters
    ''' </summary>
    ''' <param name="sampleMerchId"></param>
    ''' <param name="internalStyleNumber"></param>
    ''' <param name="vendorStyleNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAvailableSampleRequests(ByVal sampleMerchId As Integer, ByVal internalStyleNumber As Decimal, ByVal vendorStyleNumber As String) As IList(Of SampleRequestInfo)

        Dim matchedSampleRequests As IList(Of SampleRequestInfo) = New List(Of SampleRequestInfo)

        matchedSampleRequests = dalDB2.GetSampleRequests(sampleMerchId, internalStyleNumber, vendorStyleNumber)

        For Each sample As SampleRequestInfo In matchedSampleRequests
            If String.IsNullOrWhiteSpace(sample.PrimaryThumbnailUrl) Then
                sample.PrimaryThumbnailUrl = "~/Images/Camera.gif"
            End If
        Next

        Return matchedSampleRequests

    End Function

    Private Sub GetSampleRequestsForTurnInSetup(ByRef SetupCreateResults As IList(Of EcommSetupCreateInfo), ByVal UseColorId As Boolean)

        For Each item As EcommSetupCreateInfo In SetupCreateResults
            'Get any TTU450 records for this ad with matching ISN's
            Dim SampleRequests As IList(Of SampleRequestInfo)
            If (UseColorId = True) Then
                SampleRequests = dalDB2.GetSampleRequestsForIsnAndColor(item.ISN, item.ColorCode)
            Else
                SampleRequests = dalDB2.GetSampleRequestsForIsn(item.ISN)
            End If

            If SampleRequests.Any() Then
                item.SampleAvailable = "Y"c
            Else
                item.SampleAvailable = "N"c
            End If

            If SampleRequests.Any(Function(x) x.SampleApprovalFlag = "Y"c) Then
                item.SampleApproved = "Y"c
            Else
                item.SampleApproved = "N"c
            End If
        Next

    End Sub
    ''' <summary>
    ''' Sets the "Available For Turn-in" and "Active on Web" value for each sample in the eCommSamples collection
    ''' </summary>
    ''' <param name="eCommSamples">Collection of Samples</param>
    ''' <remarks>
    ''' **Available for Turn-in**
    ''' Available for Turn in should be "Y" if it meets the following conditions
    ''' Sample Approval Flag equals to "Y" and Sample status description is not empty 
    ''' and sample status description not in "REQUESTED", "DISPOSED" and "RETURNED".
    ''' Available for Turn in should be "N" in all other cases
    ''' **Active on Web**
    ''' Active on Web should be "Y" if it meets the following conditions
    ''' Active UPC Flag equals to "Y" and active flag is not empty and active flag equals to "A"
    ''' and color code not equals to zero.
    ''' Active on Web should be "N" in all other cases
    ''' </remarks>
    Private Sub UpdateSampleAvailableAndActiveOnWebStatus(ByRef eCommSamples As IList(Of EcommSetupCreateInfo))
        Dim sampleStatusDescription() As String = {"REQUESTED", "DISPOSED", "RETURNED"}
        For Each eCommSample As EcommSetupCreateInfo In eCommSamples
            With eCommSample
                ' Available for Turn-in
                If .SampleDetails.SampleApprovalFlag.ToString().Trim().Equals("Y") AndAlso (Not String.IsNullOrEmpty(.SampleDetails.SampleStatusDesc)) _
                    AndAlso Array.IndexOf(sampleStatusDescription, .SampleDetails.SampleStatusDesc.ToUpper()) = -1 Then
                    .AvailableForTurnIn = "Y"
                Else
                    .AvailableForTurnIn = "N"
                End If

                'Active on Web
                If .ActiveUPCFlag.Equals("Y") AndAlso Not String.IsNullOrEmpty(.ActiveFlag) AndAlso .ActiveFlag.Equals("A") Then
                    .ActiveOnWeb = "Y"
                Else
                    .ActiveOnWeb = "N"
                End If
            End With
        Next
    End Sub
    ''' <summary>
    ''' Applies the Turn In Filters in the ecommReulsts collection and returns the records which satisfy the search filters
    ''' </summary>
    ''' <param name="eCommResults">Collection of eComm results</param>
    ''' <param name="turnInFilters">Turn in filters selected by user</param>
    ''' <returns>eComm results which satisfy the search filters</returns>
    ''' <remarks></remarks>
    Private Function ApplyTurnInFilters(ByVal eCommResults As IEnumerable(Of EcommSetupCreateInfo), ByVal turnInFilters As TUFilter) As List(Of EcommSetupCreateInfo)
        Dim turnInSearchResults = (From eCommResult In eCommResults
                            Where (Not turnInFilters.NotTurnedIn OrElse eCommResult.IsTurnedInEcomm = "N")
                            Where IIf(turnInFilters.AvailableForTurnIn AndAlso turnInFilters.NotAvailableForTurnIn, True, _
                                      IIf(turnInFilters.NotAvailableForTurnIn, eCommResult.AvailableForTurnIn = "N", _
                                          IIf(turnInFilters.AvailableForTurnIn, eCommResult.AvailableForTurnIn = "Y", True)))
                            Where IIf(turnInFilters.ActiveOnWeb AndAlso turnInFilters.NotActiveOnWeb, True, _
                                      IIf(turnInFilters.NotActiveOnWeb, eCommResult.ActiveOnWeb = "N", _
                                          IIf(turnInFilters.ActiveOnWeb, eCommResult.ActiveOnWeb = "Y", True)))
                            Select eCommResult).ToList()

        Return turnInSearchResults.Cast(Of EcommSetupCreateInfo).ToList()
    End Function
    ''' <summary>
    ''' Gets the default ship date for the parameters passed.
    ''' </summary>
    ''' <param name="DMMID"></param>
    ''' <param name="BuyerID"></param>
    ''' <param name="DepartmentID"></param>
    ''' <param name="VendorStyleNumber"></param>
    ''' <returns>The default ship date for the CFG defined in the TMS900Parameter table</returns>
    ''' <remarks></remarks>
    Public Function GetCFGDefaultShipDays(ByVal DMMID As Integer, ByVal BuyerID As Integer, ByVal DepartmentID As Integer, ByVal VendorStyleNumber As List(Of String)) As Integer
        Dim VendorStylesString As String = String.Empty

        If VendorStyleNumber.Count > 0 Then
            VendorStylesString = RTrim("|" & String.Join("|", VendorStyleNumber) & "|".ToString())
        End If

        Return dalDB2.GetCFGDefaultShipDays(DMMID, BuyerID, DepartmentID, VendorStylesString)
    End Function
    ''' <summary>
    ''' Get color level items that are approved for the ISN passed
    ''' </summary>
    ''' <param name="ISN">ISN for which color level itmes to be retrieved</param>
    ''' <returns>Returns the color level results for the ISN passed</returns>
    ''' <remarks></remarks>
    Public Function GetApprovedEcommSetupCreateDetailByISN(ByVal ISN As Decimal, ByVal StartShipDate As Date, ByVal IncludeOnlyApprovedItems As Boolean) As List(Of EcommSetupCreateInfo)
        Try
            Dim DB2Results As List(Of EcommSetupCreateInfo) = dalDB2.GetApprovedEcommSetupCreateDetail(ISN, StartShipDate, IncludeOnlyApprovedItems)
            Dim eCommResults As IEnumerable(Of EcommSetupCreateInfo) = Nothing
            If DB2Results.Count = 0 Then
                Return New List(Of EcommSetupCreateInfo)
            Else
                UpdateSampleAvailableAndActiveOnWebStatus(DB2Results)

                If IncludeOnlyApprovedItems Then
                    eCommResults = From results In DB2Results.FindAll(Function(a) a.AvailableForTurnIn = "Y")
                     Group By results.ColorCode
                        Into samples = Group
                    Select samples.OrderByDescending(Function(x) x.ActiveOnWeb).ThenByDescending(Function(x) x.AvailableForTurnIn).First()
                Else
                    eCommResults = From results In DB2Results
                     Group By results.ColorCode
                        Into samples = Group
                    Select samples.OrderByDescending(Function(x) x.ActiveOnWeb).ThenByDescending(Function(x) x.AvailableForTurnIn).First()
                End If

                Return eCommResults.Cast(Of EcommSetupCreateInfo).ToList()

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
