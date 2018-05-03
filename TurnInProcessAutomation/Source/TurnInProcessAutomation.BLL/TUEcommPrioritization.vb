Imports TurnInProcessAutomation.BLL.CommonBO
Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.SqlDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient
Imports System.Text

Public Class TUEcommPrioritization
    Private dalDB2 As MainframeDAL.EcommPrioritizationDao = New MainframeDAL.EcommPrioritizationDao
    Private dalSQL As SqlDAL.CtlgAdPg = New SqlDAL.CtlgAdPg

    Public Function GetPrioritizationResults(ByVal strEMMId As String, ByVal strCMGId As String, ByVal strBuyerId As String, ByVal strLabelId As String, ByVal strTUWeek As String, ByVal strImageId As String, ByVal strVendStyleId As String, ByVal strStatus As String) As IList(Of ECommPrioritizationInfo)
        Try
            Dim DB2Results As IList(Of ECommPrioritizationInfo) = dalDB2.GetPrioritizationResults(strEMMId, strCMGId, strBuyerId, strLabelId, strTUWeek, strImageId, strVendStyleId, strStatus)
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetPrioritizationForExcel(ByVal CategoryID As String, ByVal Status As String) As IList(Of ECommPrioritizationInfo)
        Try
            Dim DB2Results As IList(Of ECommPrioritizationInfo) = dalDB2.GetPrioritizationResults(CategoryID, Status)
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    

    Public Function GetExportCategories(ByVal Status As String) As IList(Of String)
        Try
            Dim DB2Results As IList(Of String) = dalDB2.ExportCategories(Status)
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetColorLevelResults(ByVal ISN As Decimal, ByVal chrStatus As Char, ByVal decAdNum As Decimal) As IList(Of ECommPrioritizationInfo)
        Try
            Dim DB2Results As IList(Of ECommPrioritizationInfo) = dalDB2.GetColorLevelResults(ISN, chrStatus, decAdNum)
            Return GetAdminImageNotes(DB2Results).ToList
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function GetAdminImageNotes(ByVal DB2Results As IList(Of ECommPrioritizationInfo)) As List(Of ECommPrioritizationInfo)
        If DB2Results.Count > 0 Then
            Dim distinctAdminImageNotes As String = CommonBO.FormulateAdminXML(DB2Results.Select(Function(x) CStr(x.AdNbrAdminImgNbr)).Distinct.ToList)
            Dim SQLResults As IList(Of AdminImageNotesInfo) = dalSQL.GetAdminImageNotes(distinctAdminImageNotes)
            Dim typedresults As List(Of ECommPrioritizationInfo) = DB2Results.Select(Function(x) New ECommPrioritizationInfo With { _
                                .ISN = x.ISN, _
                                .TurnInMerchID = x.TurnInMerchID, _
                                .IsValidFlg = x.IsValidFlg, _
                                .FeatureID = x.FeatureID, _
                                .ImageID = x.ImageID, _
                                .ProductName = x.ProductName, _
                                .FriendlyColor = x.FriendlyColor, _
                                .NonSwatchClrCde = x.NonSwatchClrCde, _
                                .NonSwatchClrDesc = x.NonSwatchClrDesc, _
                                .ColorFamily = x.ColorFamily, _
                                .ImageGroup = x.ImageGroup, _
                                .FRS = x.FRS, _
                                .EMMNotes = x.EMMNotes, _
                                .VtPath = x.VtPath, _
                                .AdNbrAdminImgNbr = x.AdNbrAdminImgNbr, _
                                .StatusFlg = x.StatusFlg, _
                                .ImageSuffix = If(SQLResults.Where(Function(y) y.AdNbrAdminImgNbr = x.AdNbrAdminImgNbr).Count > 0, SQLResults.Where(Function(y) y.AdNbrAdminImgNbr = x.AdNbrAdminImgNbr)(0).ImageSuffix, ""), _
                                .ImageNotes = If(SQLResults.Where(Function(z) z.AdNbrAdminImgNbr = x.AdNbrAdminImgNbr).Count > 0, SQLResults.Where(Function(z) z.AdNbrAdminImgNbr = x.AdNbrAdminImgNbr)(0).ImageNotes, "")}).ToList
            Return typedresults
        Else
            Return DB2Results
        End If
    End Function

    Public Function GetSizeLevelResults(ByVal ISN As Decimal, ByVal MerchID As Integer, ByVal chrStatus As String) As IList(Of ECommPrioritizationInfo)
        Try
            Dim DB2Results As IList(Of ECommPrioritizationInfo) = dalDB2.GetSizeLevelResults(ISN, MerchID, chrStatus)
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetClrFamilyLookUp(ByVal ISN As Decimal) As List(Of ClrSizLocLookUp)
        Try
            Dim DB2Results As List(Of ClrSizLocLookUp) = dalDB2.GetClrSizFamilyLookUp(ISN, 2)
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetAllSizeFamilyLookUp() As List(Of ClrSizLocLookUp)
        Try
            Dim DB2Results As List(Of ClrSizLocLookUp) = dalDB2.GetAllSizeFamilyLookUp
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetNonSwatchClrLookUp() As List(Of ClrSizLocLookUp)
        Try
            Dim DB2Results As List(Of ClrSizLocLookUp) = dalDB2.GetNonSwatchClrLookUp()
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetWebcatSizeLookUp() As List(Of ClrSizLocLookUp)
        Try
            Dim DB2Results As List(Of ClrSizLocLookUp) = dalDB2.GetWebcatSizeLookUp()
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetSizFamilyLookUp(ByVal ISN As Decimal) As List(Of ClrSizLocLookUp)
        Try
            Dim DB2Results As List(Of ClrSizLocLookUp) = dalDB2.GetClrSizFamilyLookUp(ISN, 1)
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub UpdateISNLevelData(ByVal ISNLevelData As ECommPrioritizationInfo, ByVal UserId As String)
        Try
            dalDB2.UpdateISNLevelData(ISNLevelData, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateColorLevelData(ByVal ColorLevelData As ECommPrioritizationInfo, ByVal UserId As String)
        Try
            'Transpose the comma separated values of Color Family into XML records.
            Dim ClrFamilyXML As String = String.Empty

            ClrFamilyXML &= "<colorfamily>"
            For Each cf As String In ColorLevelData.ColorFamily.Split(","c)
                ClrFamilyXML &= "<color code=""" & cf.Trim & """ />"
            Next
            ClrFamilyXML &= "</colorfamily>"

            ColorLevelData.ColorFamily = ClrFamilyXML

            'Update image notes in admin
            If ColorLevelData.ImageID > 0 AndAlso Not String.IsNullOrEmpty(ColorLevelData.ImageNotes) Then
                Dim dalSQLAdminData As New AdminDataDao()
                dalSQLAdminData.UpdateImageNotesByImageID(ColorLevelData.AdNbr, 0, ColorLevelData.ImageID, ColorLevelData.ImageNotes)
            End If

            dalDB2.UpdateColorLevelData(ColorLevelData, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub DeleteColorLevelData(ByVal MerchId As Integer, ByVal chrStatus As Char, ByVal UserId As String)
        Try
            dalDB2.DeleteColorLevelData(MerchId, chrStatus, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateSizeLevelData(ByVal SizeLevelData As ECommPrioritizationInfo, ByVal UserId As String)
        Try
            'Transpose the comma separated values of Color Family into XML records.
            Dim SZFamilyXML As String = String.Empty

            SZFamilyXML &= "<sizefamily>"
            For Each cf As String In SizeLevelData.SizeFamily.Split(","c)
                SZFamilyXML &= "<size code=""" & cf.Trim & """ />"
            Next
            SZFamilyXML &= "</sizefamily>"

            SizeLevelData.SizeFamily = SZFamilyXML

            dalDB2.UpdateSizeLevelData(SizeLevelData, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateISNLevelDataFlood(ByVal ISNs As String, ByVal ISNLevelData As ECommPrioritizationInfo, ByVal UserId As String)
        Try
            dalDB2.UpdateISNLevelDataFlood(ISNs, ISNLevelData, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateSizeLevelDataFlood(ByVal strFindSize As String, ByVal strReplaceSize As String, ByVal strFindSizeFam As String, ByVal strReplaceSizeFam As String, _
                                        ByVal strFindVendorSize As String, ByVal strVendorReplaceSize As String, ByVal strVendorReplaceSizeFam As String, _
                                        ByVal UserId As String, ByVal strMerchIds As String)
        Try
            dalDB2.UpdateSizeLevelDataFlood(strFindSize, strReplaceSize, strFindSizeFam, strReplaceSizeFam, _
                                            strFindVendorSize, strVendorReplaceSize, strVendorReplaceSizeFam, _
                                            UserId, strMerchIds)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateColorLevelDataFlood(ByVal MerchIDs As String, ByVal ClrLevelData As ECommPrioritizationInfo, ByVal UserId As String)
        Try
            dalDB2.UpdateColorLevelDataFlood(MerchIDs, ClrLevelData, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function GetImageIDs(ByVal ISNs As String, ByVal chrStatus As Char, ByVal Ads As String, Optional ByVal FetchImageNotes As Boolean = True) As List(Of ImageInfo)
        Try
            Dim DB2Results As List(Of ImageInfo) = dalDB2.GetImageIDs(ISNs, chrStatus, Ads)
            If FetchImageNotes Then
                Return GetAdminImageNotesForSubmit(DB2Results).ToList
            Else
                Return DB2Results
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetImageIDsWithoutAdminNotes(ByVal ISNs As String, ByVal chrStatus As Char, ByVal Ads As String) As List(Of ImageInfo)
        Return GetImageIDs(ISNs, chrStatus, Ads, False)
    End Function

    Private Function GetAdminImageNotesForSubmit(ByVal DB2Results As IList(Of ImageInfo)) As List(Of ImageInfo)
        If DB2Results.Count > 0 Then
            FormulateAdminXML(DB2Results)
            Dim SQLResults As IList(Of AdminImageNotesInfo) = dalSQL.GetAdminImageNotes(CommonBO.FormulateAdminXML(DB2Results.Select(Function(x) CStr(x.AdNbrAdminImgNbr)).Distinct.ToList))
            Dim typedresults As List(Of ImageInfo) = DB2Results.Select(Function(x) New ImageInfo With { _
                                .ImageId = x.ImageId, _
                                .TurnInMerchId = x.TurnInMerchId, _
                                .AdNbrAdminImgNbr = x.AdNbrAdminImgNbr, _
                                .VendorStyle = x.VendorStyle, _
                                .ImageGroupNumber = x.ImageGroupNumber, _
                                .ImageCategoryCode = x.ImageCategoryCode, _
                                .ImageNotes = If(SQLResults.Where(Function(z) z.AdNbrAdminImgNbr = x.AdNbrAdminImgNbr).Count > 0, SQLResults.Where(Function(z) z.AdNbrAdminImgNbr = x.AdNbrAdminImgNbr)(0).ImageNotes, "")}).ToList
            Return typedresults
        Else
            Return DB2Results
        End If
    End Function

    Private Function FormulateAdminXML(ByVal DB2Results As IList(Of ImageInfo)) As String
        Dim returnString As New StringBuilder
        returnString.Append("<bonton>")
        For Each ImageInfo As ImageInfo In DB2Results
            returnString.Append("<admin id=""" + ImageInfo.AdNbrAdminImgNbr + """/>")
        Next
        returnString.Append("</bonton>")
        Return returnString.ToString
    End Function

    Public Function FeatureExistsOnReturnedProduct(ByVal FeatureIdNum As Integer) As Integer
        Try
            Return dalDB2.FeatureExistsOnReturnedProduct(FeatureIdNum)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub SubmitToWebCatStage(ByVal MerchId As Integer, ByVal AdminImgNotes As String, ByVal UserId As String)
        Try
            dalDB2.SubmitToWebCat(MerchId, AdminImgNotes, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function GetExistingUPCOnWebCat(ByVal TurnInMerchID As Integer) As List(Of String)
        Try
            Return dalDB2.GetExistingUPCOnWebCat(TurnInMerchID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub SubmitUPCdataToWebCatStage(ByVal MerchId As Integer, ByVal AdminImgNotes As String, ByVal UserId As String)
        Try
            dalDB2.SubmitUPCdataToWebCat(MerchId, AdminImgNotes, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function GetEmailAddress(ByVal UserId As String) As String
        Try
            Return dalDB2.GetEmailAddress(UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function IsImageProcessedInWebCat(ByVal ImageID As Integer, ByVal MerchID As Integer) As Boolean
        Try
            If Not dalDB2.IsImageProcessedInWebCat(ImageID) Then ' Image exists check if UPCs were imported.
                If dalDB2.AreUPCsProcessedForImage(MerchID) Then ' Do UPCs exist which got processed.
                    Return True
                Else
                    Return False
                End If
            Else
                Return True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub UpdateWebCatImportStatus(ByVal MerchId As Integer, ByVal Success As Char, ByVal UserId As String)
        Try
            dalDB2.UpdateWebCatImportStatus(MerchId, Success, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function GetWebCatImportProductDetailCopy(ByVal productCde As Integer) As String
        Try
            Return dalDB2.GetWebCatImportProductDetailCopy(productCde)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub UpdateWebCatImportProductDetailCopy(ByVal productCde As Integer, ByVal productLongDesc As String)
        Try
            dalDB2.UpdateWebCatImportProductDetailCopy(productCde, productLongDesc)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
