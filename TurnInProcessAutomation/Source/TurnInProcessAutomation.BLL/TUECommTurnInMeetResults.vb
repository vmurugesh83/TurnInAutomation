
Imports TurnInProcessAutomation.BLL.CommonBO
Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.SqlDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient
Imports System.Xml.Linq
Public Class TUECommTurnInMeetResults
    Private dalDB2 As MainframeDAL.ECommTurnInMeetDao = New MainframeDAL.ECommTurnInMeetDao
    Private dalDB2Setup As MainframeDAL.EcommSetupCreateDao = New MainframeDAL.EcommSetupCreateDao
    Private dalSQLAdminData As SqlDAL.AdminDataDao = New SqlDAL.AdminDataDao
    Private dalSQL As SqlDAL.IsTurnedInDao = New SqlDAL.IsTurnedInDao
    Private dalSQLAdInfo As SqlDAL.AdInfo = New SqlDAL.AdInfo
    Public Sub InsertWebcat(ByVal ISN As Decimal, ByVal WebCategories As List(Of WebCat), ByVal UserId As String)
        Try
            Dim WebCatXML As String = ""
            WebCatXML &= "<categories>"
            For Each wc As WebCat In WebCategories
                WebCatXML &= "<category categoryCode=""" & wc.CategoryCode & """ primaryFlag=""" & CommonBO.ConvertBooleantoYN(wc.DefaultCategoryFlag) & """ />"
            Next
            WebCatXML &= "</categories>"

            dalDB2.InsertWebcat(ISN, WebCatXML, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function getallFeatureID(ByVal ISN As Decimal) As List(Of FeatureIDInfo)
        Try
            Return dalDB2.GetAllFeatureID(ISN)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function IsKilled(ByVal turnInMerchID As Integer) As Boolean
        Try
            Return dalDB2.IsKilled(turnInMerchID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetEcommTurninMeet(ByVal AdNumber As String, ByVal PageNumber As String, ByVal BuyerId As String, ByVal DeptId As String, ByVal LabelID As String, ByVal VendorStyleID As String,
                                ByVal BatchID As String, Optional MerchId As Integer = 0, Optional IsTIABatch As Boolean = False) As List(Of ECommTurnInMeetCreateInfo)

        Try
            Dim items As List(Of ECommTurnInMeetCreateInfo) = dalDB2.GetEcommTurninMeet(AdNumber, PageNumber, BuyerId, DeptId, LabelID, VendorStyleID, BatchID, MerchId, IsTIABatch)

            ' When the Admin Merch Num has already been set in the TTU410 table, get the color code, color description, size ID and size description from TTU450 by AdminMerchNum
            ' Otherwise, get the color code, color description, size ID and size description from TTU450 by ISN + color code - when there is only a single match.
            For Each adMerchandise In items

                Dim sampleRequestList As List(Of SampleRequestInfo) = New List(Of SampleRequestInfo)
                Dim sampleRequest As SampleRequestInfo = New SampleRequestInfo

                Try
                    Select Case CInt(adMerchandise.RoutefromAd)

                        Case 0

                            If (String.Compare(adMerchandise.ImageKindCode, "NEW", True) = 0 Or
                                  (String.Compare(adMerchandise.ImageKindCode, "DUP", True) = 0 And String.Compare(adMerchandise.AltView.Trim(), "SWREF", True) = 0)) Then

                                Select Case (adMerchandise.MerchID)

                                    Case 0
                                        'pink error - ui
                                        'mpeCommTurnInMaint.ErrorMessage = "Error on Page. Samples have not been selected for all merchandise."
                                        'Sample Picker
                                        adMerchandise.PrimaryThumbnailUrlAltText = "Sample not selected"
                                    Case Is > 0
                                        'thumbnail or merch id
                                        sampleRequestList = dalDB2Setup.GetSampleRequests(adMerchandise.MerchID, 0D, String.Empty).Where(Function(x) x.SampleApprovalFlag = "Y"c).ToList()
                                        adMerchandise.PrimaryThumbnailUrlAltText = Trim(CStr(adMerchandise.MerchID))

                                        If Not sampleRequestList Is Nothing AndAlso sampleRequestList.Count = 1 Then
                                            sampleRequest = sampleRequestList.First()
                                            If Not (String.IsNullOrWhiteSpace(sampleRequest.PrimaryThumbnailUrl)) Then
                                                adMerchandise.PrimaryThumbnailUrl = sampleRequest.PrimaryThumbnailUrl
                                            Else
                                                adMerchandise.PrimaryThumbnailUrl = String.Empty  '- displays MerchID
                                            End If
                                        End If
                                    Case Else
                                        'Error

                                End Select
                            ElseIf (String.Compare(adMerchandise.ImageKindCode, "NEW", True) <> 0) Then
                                'Merchandise Not Required
                                adMerchandise.PrimaryThumbnailUrl = String.Empty
                                adMerchandise.PrimaryThumbnailUrlAltText = "Merchandise not required"
                                'what about MerchId ?? Just not display but leave it ?

                            End If

                        Case Is > 0
                            'Routed
                            'what about merchid ?
                            adMerchandise.PrimaryThumbnailUrlAltText = "Routed"
                            adMerchandise.MerchID = 0 '?
                        Case Else
                            'error
                    End Select

                Catch ex As Exception
                    'System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message

                End Try

            Next


            If items.Count > 0 Then

                Dim i As Integer = 1
                items(0).VendorStyleSequence = 0
                While i < items.Count
                    If items(i - 1).VendorStyleNumber = items(i).VendorStyleNumber Then
                        items(i).VendorStyleSequence = items(i - 1).VendorStyleSequence
                    Else
                        items(i).VendorStyleSequence = items(i - 1).VendorStyleSequence + 1
                    End If
                    i = i + 1
                End While
            End If
            Return items.OrderBy(Function(x) x.Sequence).ThenBy(Function(x) x.ImageCategoryCode).ToList

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetEcommTurninMeetByMerchId(ByVal TurnInMerchId As Integer) As ECommTurnInMeetCreateInfo
        Try
            Return dalDB2.GetEcommTurninMeetByMerchId(TurnInMerchId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub DeleteEMM(ByVal MerchId As Integer, ByVal UserId As String)
        Try
            dalDB2Setup.DeleteColorSize(MerchId, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub DeleteMediaCoord(ByVal MerchId As Integer, ByVal UserId As String)
        Try
            dalDB2Setup.DeleteColorSize(MerchId, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub DeleteMerchCoord(ByVal MerchId As Integer, ByVal UserId As String)
        Try
            dalDB2Setup.DeleteColorSize(MerchId, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub SubmitMeetingPage(ByRef SubmitList As List(Of ECommTurnInMeetCreateInfo), ByVal UserId As String, Optional ByVal IsTIABatch As Boolean = False)
        Dim ignoreImageCreation As Boolean = False
        Dim featureImageID As Integer = 0
        Dim featureItemWIthNewImageID As ECommTurnInMeetCreateInfo = Nothing
        Dim imageCategoryCode As String = String.Empty
        Dim imageNotes As String = String.Empty
        Dim turnInMeetCreateInfoList As List(Of ECommTurnInMeetCreateInfo) = Nothing
        Dim ecommTurnInCreateInfo As ECommTurnInMeetCreateInfo = Nothing
        Dim ecommMeeting As TUECommTurnInMeetResults = Nothing
        Dim featureItem As ECommTurnInMeetCreateInfo = Nothing
        Dim ecommPrioritizationDAO As EcommPrioritizationDao = Nothing

        If IsTIABatch Then
            ecommTurnInCreateInfo = SubmitList(0)
            ecommMeeting = New TUECommTurnInMeetResults()
            turnInMeetCreateInfoList = ecommMeeting.GetEcommTurninMeet(ecommTurnInCreateInfo.AdNumber.ToString(), ecommTurnInCreateInfo.PageNumber.ToString(), String.Empty,
                                                                       String.Empty, String.Empty, String.Empty, ecommTurnInCreateInfo.BatchNum, 0, True).ToList()
        End If

        'First submit all items that have a single image
        For Each item As ECommTurnInMeetCreateInfo In SubmitList.Where(Function(x) x.ImageGrp = "0")

            If Not item.ImageKindDescription.ToUpper.Equals("NO MERCH") Then

                Dim NewImageId As Integer = CreateImageAndAssignToAd(item, UserId, item.ImageNotes)
                imageNotes = String.Empty
                'Update image notes with feature id for TIA batches
                If IsTIABatch AndAlso Not turnInMeetCreateInfoList Is Nothing AndAlso turnInMeetCreateInfoList.Count > 0 _
                    AndAlso (item.ImageCategoryCode.ToUpper().Equals("RENDER") OrElse item.ImageCategoryCode.ToUpper().Equals("SWATCH")) Then
                    'For swathes and renders the notes shoule have the feature image id.
                    featureItem = turnInMeetCreateInfoList.Find(Function(a) a.VendorStyleNumber = item.VendorStyleNumber AndAlso a.FeatureSwatch.ToUpper() = "FEATURE")
                    If Not featureItem Is Nothing AndAlso featureItem.turnInMerchID > 0 Then
                        featureItemWIthNewImageID = dalDB2.GetEcommTurninMeetByMerchId(featureItem.turnInMerchID, True)
                        If Not featureItemWIthNewImageID Is Nothing Then
                            featureImageID = featureItemWIthNewImageID.PickupImageID
                        End If
                    Else
                        featureImageID = item.FeatureID
                    End If

                    If featureImageID > 0 Then
                        If item.ImageCategoryCode.ToUpper().Equals("RENDER") Then
                            imageCategoryCode = "REND"
                        Else
                            imageCategoryCode = "SWTCH"
                        End If

                        imageNotes = String.Concat(item.ImageNotes, " to ", featureImageID.ToString(), ")")

                        If imageNotes.Trim().Length > 35 Then
                            imageNotes = imageNotes.Substring(0, 35)
                        End If

                        dalSQLAdminData.UpdateImageNotesByImageID(item.AdNumber, item.PageNumber, NewImageId, imageNotes)
                        dalDB2.UpdateCCFlood(item.turnInMerchID, String.Empty, String.Empty, String.Empty, imageCategoryCode, imageNotes, String.Empty, UserId)
                        item.FeatureID = featureImageID
                    End If
                End If

                dalDB2.UpdateImageFileUrl(NewImageId, UserId)

                If Not String.IsNullOrEmpty(item.AltView.Trim) Then
                    ' Per Issue #: 329 - Don't auto-populate RUSH SAMPLE in Image Creation interface to Admin/Merch
                    CreateImageAndAssignToAd(item, UserId, item.ImageNotes & item.AltView.Trim())
                End If

                AssociateImageWithMerchandise(NewImageId, item, UserId, IsTIABatch)
            Else
                dalDB2.SubmitEMM(item, UserId, IsTIABatch)
            End If
        Next

        'Check and update the feature image id for the render, if they were not updated already
        If IsTIABatch Then
            turnInMeetCreateInfoList = ecommMeeting.GetEcommTurninMeet(ecommTurnInCreateInfo.AdNumber.ToString(), ecommTurnInCreateInfo.PageNumber.ToString(), String.Empty,
                                                           String.Empty, String.Empty, String.Empty, ecommTurnInCreateInfo.BatchNum, 0, True).ToList()

            If Not turnInMeetCreateInfoList Is Nothing AndAlso turnInMeetCreateInfoList.Count > 0 Then
                ecommPrioritizationDAO = New EcommPrioritizationDao()
                For Each ecommTurnInMeeet As ECommTurnInMeetCreateInfo In turnInMeetCreateInfoList.FindAll(Function(a) a.ImageCategoryCode.ToUpper.Equals("RENDER"))
                    If ecommTurnInMeeet.FeatureID <= 0 Then
                        featureItem = turnInMeetCreateInfoList.Find(Function(a) a.VendorStyleNumber = ecommTurnInMeeet.VendorStyleNumber _
                                                                        And (a.ImageCategoryCode.ToUpper().Equals("FEATURE") OrElse a.ImageCategoryCode.ToUpper().Equals("SWATCH")))

                        If Not featureItem Is Nothing AndAlso featureItem.PickupImageID > 0 Then
                            If Not ecommTurnInMeeet.ImageNotes.Contains(String.Concat("to ", featureItem.PickupImageID)) Then
                                If ecommTurnInMeeet.ImageCategoryCode.ToUpper().Equals("RENDER") Then
                                    imageCategoryCode = "REND"
                                Else
                                    imageCategoryCode = "SWTCH"
                                End If

                                imageNotes = String.Concat(ecommTurnInMeeet.ImageNotes, " to ", featureItem.PickupImageID.ToString(), ")")

                                If imageNotes.Trim().Length > 35 Then
                                    imageNotes = imageNotes.Substring(0, 35)
                                End If

                                dalSQLAdminData.UpdateImageNotesByImageID(ecommTurnInMeeet.AdNumber, ecommTurnInMeeet.PageNumber, ecommTurnInMeeet.PickupImageID, imageNotes)
                                dalDB2.UpdateCCFlood(ecommTurnInMeeet.turnInMerchID, String.Empty, String.Empty, String.Empty, imageCategoryCode, imageNotes, String.Empty, UserId)
                                ecommPrioritizationDAO.UpdateColorLevelDataFlood(ecommTurnInMeeet.turnInMerchID.ToString(), New ECommPrioritizationInfo With {.FeatureID = featureItem.PickupImageID}, UserId)
                            End If
                        End If
                    End If
                Next
            End If
        End If

        'Next submit all items that are grouped into images
        Dim ImageGroups As List(Of String) = SubmitList.Where(Function(x) x.ImageGrp <> "0").Select(Function(x) x.ImageGrp).Distinct.ToList
        For Each ImageGroupNumber As String In ImageGroups
            Dim ImageGrpNum As String = ImageGroupNumber
            ignoreImageCreation = False
            Dim item As ECommTurnInMeetCreateInfo = SubmitList.FirstOrDefault(Function(x) x.ImageGrp = ImageGrpNum)

            'If there is only one record for an image group and if the record image kind type is "No Merch", then no images should be created
            Dim noMerchItems As List(Of ECommTurnInMeetCreateInfo) = SubmitList.FindAll(Function(a) a.ImageGrp = ImageGrpNum)
            If Not noMerchItems Is Nothing AndAlso noMerchItems.Count = 1 AndAlso noMerchItems(0).ImageKindDescription.ToUpper.Equals("NO MERCH") Then
                ignoreImageCreation = True
            End If

            If Not ignoreImageCreation Then
                Dim NewImageId As Integer = CreateImageAndAssignToAd(item, UserId, item.ImageNotes)

                dalDB2.UpdateImageFileUrl(NewImageId, UserId)

                For Each detailItem As ECommTurnInMeetCreateInfo In SubmitList.Where(Function(x) x.ImageGrp = ImageGrpNum)

                    If Not String.IsNullOrEmpty(detailItem.AltView.Trim) Then
                        ' Per Issue #: 329 - Don't auto-populate RUSH SAMPLE in Image Creation interface to Admin/Merch
                        CreateImageAndAssignToAd(detailItem, UserId, item.ImageNotes & item.AltView.Trim())
                    End If

                    AssociateImageWithMerchandise(NewImageId, detailItem, UserId, IsTIABatch)

                Next
            Else
                dalDB2.SubmitEMM(noMerchItems(0), UserId, IsTIABatch)
            End If
        Next

    End Sub

    Private Function CreateImageAndAssignToAd(ByVal item As ECommTurnInMeetCreateInfo, ByVal UserId As String, ByVal imageNote As String) As Integer

        Dim ImageClass As String = "N"
        If item.FigureCode = "ON" Then
            ImageClass = "F"
        ElseIf item.FigureCode = "OFF" Then
            ImageClass = "N"
        End If

        'Refer to Issue #282 and #283 for business rules.
        Dim ImageSuffix As String = String.Empty

        If item.ImageKindCode = "VND" Then
            ImageSuffix = "V"
        ElseIf item.ImageKindCode = "CR8" Or _
                item.ImageKindCode = "DUP" Or _
                item.ImageKindCode = "PU" Or _
                item.ImageKindCode = "NOMER" Or _
                (item.ImageKindCode = "NEW" And item.ImageCategoryCode.Trim.ToUpper = "SWATCH") Or _
                item.AltView.Trim.ToUpper = "SWATCH REF" Or _
                item.ImageCategoryCode.Trim.ToUpper = "SWATCH" Or _
                item.ImageCategoryCode.Trim.ToUpper = "STATIC SWATCH" Then
            ImageSuffix = "C"
        End If

        If item.ImageKindCode = "NEW" And item.ImageCategoryCode.Trim.ToUpper <> "SWATCH" Then
            ImageSuffix = String.Empty
        End If

        ' NOTE: Insert the batch number in the buyer extension field for convenience
        Dim NewImageId As Integer = dalSQLAdminData.CreateImageAndAssignToAd(item.BuyerName, item.BatchNum.ToString(), item.ImageDesc, ImageClass, item.AdNumber, item.PageNumber, ImageSuffix, imageNote)
        Return NewImageId

    End Function

    Private Sub AssociateImageWithMerchandise(ByVal newImageId As Integer, ByVal item As ECommTurnInMeetCreateInfo, ByVal userId As String, ByVal shouldUpdateAgeAndGender As Boolean)

        ' Update Admin
        If (item.MerchID <> 0) Then

            ' Update the Admin Merch Ad & Page numbers
            dalSQLAdminData.UpdateAdminData(item.MerchID, item.AdNumber, item.PageNumber)

            ' Associate the merchandise with the new image
            dalSQLAdminData.CreateNewImageMerch(newImageId, item.MerchID, item.ImageGrp, item.StylingNotes)

            dalSQLAdminData.UpdateAdminStatus(item.MerchID, item.AdNumber, item.PageNumber, item.RemoveMerchFlag)

        End If

        ' Update Turn-In
        dalDB2.UpdateImageRequest(newImageId, item.TIImageId, userId)
        dalDB2.SubmitEMM(item, userId, shouldUpdateAgeAndGender)

    End Sub

    Public Sub RejectBatch(ByVal BatchId As Integer, ByVal userId As String)
        Try
            dalDB2.RejectBatch(BatchId, userId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateEMMInfo(ByVal merchandiseInfo As ECommTurnInMeetCreateInfo, ByVal UserId As String)
        Try

            dalDB2.UpdateEMMInfo(merchandiseInfo, UserId)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateCCInfo(ByVal CCInfo As ECommTurnInMeetCreateInfo, ByVal UserId As String)

        ' TODO: Push size ID into TTU410
        dalDB2.UpdateCCInfo(CCInfo, UserId)

    End Sub


    Public Sub UpdateCWInfo(ByVal MerchID As Integer, ByVal CpyNotes As String, FollowUpFlag As String, ByVal UserId As String)
        Try
            dalDB2.UpdateCWInfo(MerchID, CpyNotes, FollowUpFlag, UserId)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub UpdateEMMFlood(ByVal TurnInMerchIDs As String, ByVal WebCategoryCode As String, ByVal FriendlyProductDescription As String, ByVal EMMNotes As String, ByVal FriendlyColor As String, ByVal SizeCategory As String, ByVal UserId As String)
        Try
            dalDB2.UpdateEMMFlood(TurnInMerchIDs, WebCategoryCode, FriendlyProductDescription, EMMNotes, FriendlyColor, SizeCategory, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateEMMFollowupFlag(ByVal TurnInMerchID As Integer, ByVal EMMFollowUpFlag As String, ByVal UserId As String)
        Try
            dalDB2.UpdateEMMFollowupFlag(TurnInMerchID, EMMFollowUpFlag, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateCCFollowupFlag(ByVal TurnInMerchID As Integer, ByVal CCFollowUpFlag As String, ByVal UserId As String)
        Try
            dalDB2.UpdateCCFollowupFlag(TurnInMerchID, CCFollowUpFlag, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateCWFollowupFlag(ByVal TurnInMerchID As Integer, ByVal CWFollowUpFlag As String, ByVal UserId As String)
        Try
            dalDB2.UpdateCWFollowupFlag(TurnInMerchID, CWFollowUpFlag, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateCCFlood(ByVal TurnInMerchIDs As String, ImageType As String, ModelCategory As String, AlternateView As String, ByVal FRS As String, ByVal ImageNotes As String, ByVal StylingNotes As String, ByVal UserId As String)
        Try
            dalDB2.UpdateCCFlood(TurnInMerchIDs, ImageType, ModelCategory, AlternateView, FRS, ImageNotes, StylingNotes, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Reads the image group numbers entered through prioritization screen and creates an image for the image group and assign the image id to all the merchandise of that image group
    ''' </summary>
    ''' <param name="imageInfos"></param>
    ''' <param name="UserId"></param>
    ''' <remarks></remarks>
    Public Sub UpdateImageGroupNumber(ByVal imageInfos As List(Of ImageInfo), ByVal UserId As String)
        Dim _TUECommTurnInMeetResults As New TUECommTurnInMeetResults
        Dim detailItem As ECommTurnInMeetCreateInfo = Nothing
        Dim imageGroupNumberFromAdmin As Integer = 0
        Dim ImageGroups As List(Of Integer) = imageInfos.Where(Function(x) x.ImageGroupNumber <> 0).Select(Function(x) x.ImageGroupNumber).Distinct.ToList
        For Each ImageGroupNumber As Integer In ImageGroups
            Dim ImageGrpNum As Integer = ImageGroupNumber

            Dim imageGroupInfo As ImageInfo = imageInfos.FirstOrDefault(Function(x) x.ImageGroupNumber = ImageGroupNumber)
            Dim ECommTunInMeetCreateInfo As ECommTurnInMeetCreateInfo = _TUECommTurnInMeetResults.GetEcommTurninMeetByMerchId(imageGroupInfo.TurnInMerchId, True)

            'If the image group number already exists in Admin, then no new image group should be generated for the merch id
            If ECommTunInMeetCreateInfo.MerchID > 0 Then
                imageGroupNumberFromAdmin = dalSQLAdminData.GetImageMerchAssociationByMerchID(ECommTunInMeetCreateInfo.MerchID)
            End If

            If Not ECommTunInMeetCreateInfo Is Nothing AndAlso ECommTunInMeetCreateInfo.turnInMerchID > 0 AndAlso
                imageGroupNumberFromAdmin = 0 Then
                Dim NewImageId As Integer = _TUECommTurnInMeetResults.CreateImageAndAssignToAd(ECommTunInMeetCreateInfo, UserId, imageGroupInfo.ImageNotes)

                dalDB2.UpdateImageFileUrl(NewImageId, UserId)

                For Each imageInfo As ImageInfo In imageInfos.Where(Function(x) x.ImageGroupNumber = ImageGroupNumber)
                    detailItem = _TUECommTurnInMeetResults.GetEcommTurninMeetByMerchId(imageInfo.TurnInMerchId, True)
                    If Not String.IsNullOrEmpty(detailItem.AltView.Trim) Then
                        ' Per Issue #: 329 - Don't auto-populate RUSH SAMPLE in Image Creation interface to Admin/Merch
                        CreateImageAndAssignToAd(detailItem, UserId, imageGroupInfo.ImageNotes & detailItem.AltView.Trim())
                    End If

                    AssociateImageWithMerchandiseNoEMMSubmit(NewImageId, detailItem, imageInfo.ImageCategoryCode, UserId)

                Next
            End If
        Next
    End Sub
    ''' <summary>
    ''' Associates the newly generated imaged for all the merchandise in the image group
    ''' </summary>
    ''' <param name="newImageId"></param>
    ''' <param name="item"></param>
    ''' <param name="imageCategoryCode"></param>
    ''' <param name="userId"></param>
    ''' <remarks></remarks>
    Private Sub AssociateImageWithMerchandiseNoEMMSubmit(ByVal newImageId As Integer, ByVal item As ECommTurnInMeetCreateInfo, ByVal imageCategoryCode As String, ByVal userId As String)
        Dim ecommPrioritizationDAO As EcommPrioritizationDao = Nothing
        ' Update Admin
        If (item.MerchID <> 0) Then

            ' Update the Admin Merch Ad & Page numbers
            dalSQLAdminData.UpdateAdminData(item.MerchID, item.AdNumber, item.PageNumber)

            ' Associate the merchandise with the new image
            dalSQLAdminData.CreateNewImageMerch(newImageId, item.MerchID, item.ImageGrp, item.StylingNotes)

            dalSQLAdminData.UpdateAdminStatus(item.MerchID, item.AdNumber, item.PageNumber, item.RemoveMerchFlag)

        End If
    End Sub
    ''' <summary>
    ''' Gets the details of the merchandise by id.  This is for getting the merchs that were created through TIA, other merchs won't be returned.
    ''' </summary>
    ''' <param name="TurnInMerchId"></param>
    ''' <param name="IsTIABatch"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetEcommTurninMeetByMerchId(ByVal TurnInMerchId As Integer, Optional ByVal IsTIABatch As Boolean = False) As ECommTurnInMeetCreateInfo
        Try
            Return dalDB2.GetEcommTurninMeetByMerchId(TurnInMerchId, IsTIABatch)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
