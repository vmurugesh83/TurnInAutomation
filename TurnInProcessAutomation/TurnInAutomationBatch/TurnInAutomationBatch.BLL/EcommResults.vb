Imports TurnInAutomationBatch.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Text

Public Class EcommResults
    Dim ecommResultsDAO As EcommResultsDAO
    Public Sub New()
        ecommResultsDAO = New EcommResultsDAO()
    End Sub
    Public Function GetApprovedItemsWithPO(ByVal StartShipDate As Date, ByVal DepartmentID As Integer, ByVal IncludeOnlyWebEligibleItems As Boolean, _
                                           ByVal dtNoMerchImages As DataTable, ByRef invalidNoMerchISNs As StringBuilder) As IList(Of EcommSetupCreateInfo)
        Dim ecommResults As IList(Of EcommSetupCreateInfo)
        ecommResults = ecommResultsDAO.GetApprovedItemsWithPO(StartShipDate, DepartmentID, IncludeOnlyWebEligibleItems)
        GetNoMerchISNs(StartShipDate, DepartmentID, IncludeOnlyWebEligibleItems, dtNoMerchImages, ecommResults, invalidNoMerchISNs)
        Return ecommResults
    End Function
    Public Function GetApprovedEcommSetupCreateDetailByISN(ByVal InternalStyleNumber As String, ByVal StartShipDate As Date, ByVal IncludeOnlyWebEligibleItems As Boolean,
                                                           ByRef invalidNoMerchISNs As StringBuilder, ByRef invalidVendorImageISNs As StringBuilder,
                                                           ByVal dtNoMerchImages As DataTable, ByVal dtVendorImages As DataTable,
                                                           Optional ByVal IsVendorImage As Boolean = False) As IList(Of EcommSetupClrSzInfo)
        Dim ecommColorResults As IList(Of EcommSetupClrSzInfo)
        ecommColorResults = ecommResultsDAO.GetApprovedColorDetailsByISN(InternalStyleNumber, StartShipDate, IncludeOnlyWebEligibleItems, IsVendorImage)
        If IsVendorImage Then
            ecommColorResults = GetVendorImageColors(InternalStyleNumber, dtVendorImages, ecommColorResults, invalidVendorImageISNs)
        Else
            GetNoMerchColors(StartShipDate, InternalStyleNumber, IncludeOnlyWebEligibleItems, dtNoMerchImages, ecommColorResults, invalidNoMerchISNs)
        End If

        Return ecommColorResults
    End Function
    Public Function GetWebCategory(ByVal DeptID As Integer, ByVal VendorID As Integer, ByVal GenericClassID As Integer,
                                   ByVal GenericSubClassID As Integer, ByVal ClassID As Integer, ByVal SubClassID As Integer, Optional ByVal FromWebCatHistory As Boolean = False) As WebCat
        Return ecommResultsDAO.GetWebCategory(DeptID, VendorID, GenericClassID, GenericSubClassID, ClassID, SubClassID, FromWebCatHistory)
    End Function
    Public Function GetApprovedEcommSetupCreateDetailByVendorStyle(ByVal VendorStyle As String, ByVal StartShipDate As Date,
                                                                   ByVal IncludeOnlyWebEligibleItems As Boolean, ByVal IsVendorImage As Boolean, ByVal DeptID As Integer) As IList(Of EcommSetupClrSzInfo)
        Return ecommResultsDAO.GetApprovedColorDetailsByVendorStyle(VendorStyle, StartShipDate, IncludeOnlyWebEligibleItems, IsVendorImage, DeptID)
    End Function
    Function GetColorsFromSSKUByDeptVendorStyle(ByVal DeptID As Integer, ByVal VendorStyle As String) As IList(Of SampleRequestInfo)
        Return ecommResultsDAO.GetColorsFromSSKUByDeptVendorStyle(DeptID, VendorStyle)
    End Function
    Public Function GetVendorImageISNs(ByVal StartShipDate As Date, ByVal DepartmentID As Integer, ByVal IncludeOnlyWebEligibleItems As Boolean,
                                       ByVal dtVendorImages As DataTable, ByRef invalidVendorImageISNs As StringBuilder) As IList(Of EcommSetupCreateInfo)

        Dim drISN As DataRow()
        Dim internalStyleNumber As Decimal = 0
        Dim ecommResults As IList(Of EcommSetupCreateInfo) = Nothing
        Dim tempEcommResults As IList(Of EcommSetupCreateInfo) = Nothing
        Dim isns As ArrayList = Nothing

        If Not dtVendorImages Is Nothing AndAlso dtVendorImages.Rows.Count > 0 Then

            ecommResults = New List(Of EcommSetupCreateInfo)

            drISN = dtVendorImages.Select(String.Format("Department LIKE '{0} -%' AND [Merch Approval] = 'Submit Image'", DepartmentID))

            If Not drISN Is Nothing Then
                isns = New ArrayList()
                For Each dr As DataRow In drISN
                    internalStyleNumber = dr("ISN")
                    tempEcommResults = ecommResultsDAO.GetApprovedItemsWithPO(StartShipDate, 0, IncludeOnlyWebEligibleItems, internalStyleNumber, False)
                    If Not tempEcommResults Is Nothing AndAlso tempEcommResults.Count > 0 Then
                        For Each ecommResult As EcommSetupCreateInfo In tempEcommResults
                            ecommResult.IsVendorImage = True
                            ecommResults.Add(ecommResult)
                        Next
                    Else
                        If Not isns.Contains(internalStyleNumber) Then
                            isns.Add(internalStyleNumber)
                            invalidVendorImageISNs.AppendLine(String.Format("The ISN {0} is invalid or it is not meeting the other criteria to be eligible for Automated Turn-In.<br/>", internalStyleNumber))
                        End If
                    End If
                Next
            End If
        End If
        Return ecommResults
    End Function

    Private Sub GetNoMerchISNs(ByVal StartShipDate As Date, ByVal DepartmentID As Integer, ByVal IncludeOnlyWebEligibleItems As Boolean, ByVal dtNoMerchImages As DataTable,
                                       ByRef ecommResults As IList(Of EcommSetupCreateInfo), ByRef invalidNoMerchISNs As StringBuilder)
        Dim drISN As DataRow()
        Dim internalStyleNumber As Decimal = 0
        Dim tempEcommResults As IList(Of EcommSetupCreateInfo) = Nothing
        Dim isns As ArrayList = Nothing

        If Not dtNoMerchImages Is Nothing AndAlso dtNoMerchImages.Rows.Count > 0 Then

            If ecommResults Is Nothing Then
                ecommResults = New List(Of EcommSetupCreateInfo)
            End If

            drISN = dtNoMerchImages.Select(String.Format("Department LIKE '{0} -%' AND [Feature] >0 ", DepartmentID))

            If Not drISN Is Nothing Then
                isns = New ArrayList()
                For Each dr As DataRow In drISN
                    internalStyleNumber = dr("ISN")
                    tempEcommResults = ecommResultsDAO.GetApprovedItemsWithPO(StartShipDate, 0, IncludeOnlyWebEligibleItems, internalStyleNumber, False)
                    If Not tempEcommResults Is Nothing AndAlso tempEcommResults.Count > 0 Then
                        For Each ecommResult As EcommSetupCreateInfo In tempEcommResults
                            ecommResult.IsNoMerchImage = True
                            ecommResults.Add(ecommResult)
                        Next
                    Else
                        If Not isns.Contains(internalStyleNumber) Then
                            isns.Add(internalStyleNumber)
                            invalidNoMerchISNs.AppendLine(String.Format("The ISN {0} is invalid or it is not meeting the other criteria to be eligible for Automated Turn-In.<br/>", internalStyleNumber))
                        End If
                    End If
                Next
            End If

        End If
    End Sub
    Private Sub GetNoMerchColors(ByVal StartShipDate As Date, ByVal InternalStyleNumber As Decimal, ByVal IncludeOnlyWebEligibleItems As Boolean, ByVal dtNoMerchImages As DataTable,
                                       ByRef ecommColorResults As IList(Of EcommSetupClrSzInfo), ByRef invalidNoMerchColors As StringBuilder)
        Dim drISN As DataRow()
        Dim colorSizeInfo As EcommSetupClrSzInfo = Nothing
        Dim colorSizeResults As List(Of EcommSetupClrSzInfo) = Nothing
        Dim colorCode As Integer = 0
        Dim imageId As Integer = 0
        Dim isInvalidColor As Boolean = False

        If Not dtNoMerchImages Is Nothing AndAlso dtNoMerchImages.Rows.Count > 0 Then

            drISN = dtNoMerchImages.Select(String.Format("ISN = '{0}' AND [Feature] >0 ", InternalStyleNumber))

            If Not drISN Is Nothing Then
                ' If no other colors are eligible, then go and get the colors for the ISN
                If ecommColorResults Is Nothing OrElse ecommColorResults.Count = 0 Then
                    colorSizeResults = ecommResultsDAO.GetApprovedColorDetailsByISN(InternalStyleNumber, StartShipDate, IncludeOnlyWebEligibleItems, True)
                    ecommColorResults = New List(Of EcommSetupClrSzInfo)
                End If

                For Each dr As DataRow In drISN
                    Integer.TryParse(dr("Color"), colorCode)
                    Integer.TryParse(dr("Feature"), imageId)

                    If imageId > 0 Then
                        If Not colorSizeResults Is Nothing AndAlso colorSizeResults.Count > 0 Then
                            'Check if the color code already exists in the collection
                            colorSizeInfo = colorSizeResults.Find(Function(a) a.VendorColorCode = colorCode)
                            If Not colorSizeInfo Is Nothing Then
                                colorSizeInfo.ImageKind = "No Merch"
                                colorSizeInfo.IsNoMerchImage = True
                                colorSizeInfo.PuImageID = imageId
                            Else
                                isInvalidColor = True
                            End If
                        End If

                        'if the color code already exists, then update the image kind as No merch.  Otherwise add it to the collection
                        If Not ecommColorResults.ToList().Find(Function(b) b.VendorColorCode = colorCode) Is Nothing Then
                            ecommColorResults.ToList().Find(Function(a) a.VendorColorCode = colorCode).ImageKind = "No Merch"
                            ecommColorResults.ToList().Find(Function(a) a.VendorColorCode = colorCode).IsNoMerchImage = True
                            ecommColorResults.ToList().Find(Function(a) a.VendorColorCode = colorCode).PuImageID = imageId
                        Else
                            If Not colorSizeInfo Is Nothing Then
                                ecommColorResults.Add(colorSizeInfo)
                            Else
                                isInvalidColor = True
                            End If
                        End If

                        If isInvalidColor Then
                            invalidNoMerchColors.AppendLine(String.Format("The color code {0} for the ISN {1} is invalid or it is not meeting the other criteria to be eligible for Automated Turn-In.<br/>",
                                                                      colorCode, InternalStyleNumber))
                        End If
                    Else
                        invalidNoMerchColors.AppendLine(String.Format("The feature {0} for the ISN {1} and color code {2} should be greater than zero.<br/>",
                                                                  imageId, colorCode, InternalStyleNumber))
                    End If
                Next
            Else
                invalidNoMerchColors.AppendLine(String.Format("Please check the format of the ISN {0}. System was not able to find the ISN in the no merch color level search.<br/>",
                                          InternalStyleNumber))
            End If
        End If
    End Sub
    Private Function GetVendorImageColors(ByVal InternalStyleNumber As Decimal, ByVal dtVendorImages As DataTable,
                                       ByVal ecommColorResults As IList(Of EcommSetupClrSzInfo), ByRef invalidVendorImageColors As StringBuilder) As IList(Of EcommSetupClrSzInfo)
        Dim drISN As DataRow() = Nothing
        Dim tempEcommResults As IList(Of EcommSetupClrSzInfo) = Nothing
        Dim colorLevelDetail As EcommSetupClrSzInfo = Nothing
        Dim colorCode As Integer = 0
        Dim copyNotes As String = String.Empty

        If Not dtVendorImages Is Nothing AndAlso dtVendorImages.Rows.Count > 0 Then

            If ecommColorResults Is Nothing OrElse ecommColorResults.Count = 0 Then
                invalidVendorImageColors.AppendLine(String.Format("The ISN {0} is invalid or it is not meeting the other criteria to be eligible for Automated Turn-In.<br/>",
                                                              InternalStyleNumber))
            Else
                tempEcommResults = New List(Of EcommSetupClrSzInfo)

                drISN = dtVendorImages.Select(String.Format("ISN = '{0}' AND [Merch Approval] = 'Submit Image'", InternalStyleNumber))

                If Not drISN Is Nothing Then
                    For Each dr As DataRow In drISN
                        Integer.TryParse(dr("Color"), colorCode)
                        copyNotes = dr("Copy Notes").ToString()
                        colorLevelDetail = ecommColorResults.ToList().Find(Function(a) a.VendorColorCode = colorCode)
                        If Not colorLevelDetail Is Nothing Then
                            colorLevelDetail.IsVendorImage = True
                            colorLevelDetail.CopyNotes = copyNotes
                            tempEcommResults.Add(colorLevelDetail)
                        Else
                            invalidVendorImageColors.AppendLine(String.Format("The color code {0} for the ISN {1} is invalid or it is not meeting the other criteria to be eligible for Automated Turn-In.<br/>",
                                                                          colorCode, InternalStyleNumber))
                        End If
                    Next
                Else
                    invalidVendorImageColors.AppendLine(String.Format("Please check the format of the ISN {0}. System was not able to find the ISN in the vendor image color level search.<br/>",
                                              InternalStyleNumber))

                End If
            End If
        End If

        Return tempEcommResults
    End Function
End Class
