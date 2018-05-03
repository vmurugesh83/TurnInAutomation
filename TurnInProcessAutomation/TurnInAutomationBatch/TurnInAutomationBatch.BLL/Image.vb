Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInAutomationBatch.MainframeDAL
Imports System.Xml
Imports TurnInAutomationBatch.BusinessEntities
Imports TurnInAutomationBatch.BLL.Enumerations

Public Class Image
    Dim imageDAO As ImageDAO = Nothing
    Public Sub New()
        imageDAO = New ImageDAO()
    End Sub
    'Public Function CreateImageRequest(ByVal ISNDetails As EcommSetupCreateInfo, ByVal ColorDetails As SampleRequestInfo) As Integer
    '    Dim commomMethods As New Common
    '    Dim imageRequestNode As XmlNode = Nothing
    '    Dim imageRequestID As Integer = 0
    '    Try
    '        imageRequestNode = commomMethods.GetDefaultValues("ImageRequest")
    '        If Not imageRequestNode Is Nothing AndAlso imageRequestNode.HasChildNodes Then
    '            imageRequestNode.Item("IMAGE_CATEGORY_CDE").InnerText = GetImageCategoryCodeByISN(ISNDetails.ISN, ColorDetails.VendorStyleNumber, ColorDetails.ColorCode)
    '            imageRequestNode.Item("IMAGE_NME").InnerText = String.Concat(ColorDetails.ColorLongDesc.Trim(), " ", ISNDetails.ISNDesc.Trim(), " ", ISNDetails.LabelDesc.Trim())
    '            imageRequestNode.Item("ADMIN_IMAGE_DESC").InnerText = String.Concat(ISNDetails.ISNDesc.Trim(), " ", ColorDetails.ColorLongDesc.Trim(), " ", ISNDetails.LabelDesc.Trim()).Substring(0, 29)
    '            imageRequestNode.Item("ADMIN_IMAGE_NUM").InnerText = ColorDetails.SampleMerchId

    '            imageDAO = New ImageDAO()
    '            imageRequestID = imageDAO.CreateImageRequest(imageRequestNode)
    '        End If

    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    '    Return imageRequestID
    'End Function

    Public Function GetImageCategoryCodeByISN(ByVal MerchLevelAttr As MerchLevelAttribute, ByVal ColorLevelDetails As List(Of EcommSetupClrSzInfo), ByVal ColorCode As Integer,
                                              ByVal ColorLevelDetailsByVendorStyle As List(Of EcommSetupClrSzInfo),
                                              ByVal StyleSKUColorsByVendorStyle As List(Of SampleRequestInfo)) As String
        Dim imageCategoryCode As String = String.Empty
        Dim otherColorsInSameBatchCount As Integer = 0
        Dim highestOHOOColorSizeInfo As EcommSetupClrSzInfo = Nothing
        Dim colorSizeInfo As List(Of EcommSetupClrSzInfo) = Nothing
        Dim colorSizeInfoWithoutImageCategory As List(Of EcommSetupClrSzInfo) = Nothing
        Dim hasHighestOHOO As Boolean = False
        Dim featureExistsInISN As Boolean = False
        Dim otherColorsExistInSSKU As Boolean = False

        Try
            If Not MerchLevelAttr Is Nothing Then
                Select Case MerchLevelAttr.MerchLevelNumber
                    Case MerchLevelType.COLORONLY, MerchLevelType.COLORSIZE
                        If Not MerchLevelAttr.WebCatVendorStyleNumber.Trim().Equals(String.Empty) Then
                            imageCategoryCode = [Enum].GetName(GetType(ImageCategoryCode), Enumerations.ImageCategoryCode.REND)
                        Else
                            'There may be multiple UPCs for a same color, so select distinct color codes
                            otherColorsInSameBatchCount = (ColorLevelDetailsByVendorStyle.FindAll(Function(a) a.VendorColorCode <> ColorCode)).Count
                            colorSizeInfoWithoutImageCategory = ColorLevelDetailsByVendorStyle.FindAll(Function(a) Not a.FeatureRenderSwatch Is Nothing)
                            If Not colorSizeInfoWithoutImageCategory Is Nothing AndAlso colorSizeInfoWithoutImageCategory.Count > 0 Then
                                featureExistsInISN = colorSizeInfoWithoutImageCategory.Exists(Function(a) a.FeatureRenderSwatch.ToUpper().Equals("FEAT"))
                            End If

                            'Black or white can be a feature if they are the only colors available under an ISN.  If there are
                            'other colors under the ISN, then black and white cannot be a feature.
                            colorSizeInfo = ColorLevelDetailsByVendorStyle.FindAll(Function(a) a.ColorFamily.Trim().ToUpper() <> "WHITE" And a.ColorFamily.Trim().ToUpper() <> "BLACK")
                            If colorSizeInfo Is Nothing OrElse colorSizeInfo.Count = 0 Then
                                colorSizeInfo = ColorLevelDetailsByVendorStyle
                            End If

                            If Not colorSizeInfo Is Nothing Then
                                highestOHOOColorSizeInfo = colorSizeInfo.OrderByDescending(Function(a) a.OnOrder + a.OnHand).FirstOrDefault()
                                If Not highestOHOOColorSizeInfo Is Nothing AndAlso highestOHOOColorSizeInfo.VendorColorCode > 0 _
                                    AndAlso ColorCode = highestOHOOColorSizeInfo.VendorColorCode Then
                                    hasHighestOHOO = True
                                End If
                            End If

                            If otherColorsInSameBatchCount > 0 Then
                                If hasHighestOHOO AndAlso Not featureExistsInISN Then
                                    imageCategoryCode = [Enum].GetName(GetType(ImageCategoryCode), Enumerations.ImageCategoryCode.FEAT)
                                Else
                                    imageCategoryCode = [Enum].GetName(GetType(ImageCategoryCode), Enumerations.ImageCategoryCode.REND)
                                End If
                            Else
                                For Each colorDetail As SampleRequestInfo In StyleSKUColorsByVendorStyle
                                    If Not ColorLevelDetailsByVendorStyle.Exists(Function(a) a.VendorColorCode = colorDetail.ColorCode) Then
                                        otherColorsExistInSSKU = True
                                        Exit For
                                    End If
                                Next

                                If otherColorsExistInSSKU Then
                                    imageCategoryCode = [Enum].GetName(GetType(ImageCategoryCode), Enumerations.ImageCategoryCode.SSF)
                                Else
                                    imageCategoryCode = [Enum].GetName(GetType(ImageCategoryCode), Enumerations.ImageCategoryCode.STDALN)
                                End If
                            End If
                        End If
                    Case Else
                        imageCategoryCode = [Enum].GetName(GetType(ImageCategoryCode), Enumerations.ImageCategoryCode.STDALN)
                End Select
            Else
                imageCategoryCode = [Enum].GetName(GetType(ImageCategoryCode), Enumerations.ImageCategoryCode.STDALN)
            End If
            Return imageCategoryCode
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetImageNotes(ByVal ImageCategory As String, ByVal ModelCategoryDesc As String, ByVal FeatureID As Integer,
                                  ByVal MerchandiseFigureCode As String, ByVal IsWaistDownSubClass As Boolean,
                                  ByVal ShouldAddSize1And2 As Boolean, ByVal Size1Desc As String, ByVal Size2Desc As String) As String
        Dim imageNotes As String = String.Empty
        Dim mdseFigureCode As String = String.Empty
        Dim waistDownCaption As String = String.Empty
        Dim size1And2Desc As String = String.Empty

        Try
            mdseFigureCode = IIf(String.IsNullOrEmpty(MerchandiseFigureCode.Trim()) OrElse MerchandiseFigureCode.Trim().ToUpper().Equals("OFF"), String.Empty, String.Concat(MerchandiseFigureCode.Trim().ToUpper(), " "))
            ModelCategoryDesc = IIf(String.IsNullOrEmpty(ModelCategoryDesc.Trim) AndAlso Not String.IsNullOrEmpty(mdseFigureCode), "*", ModelCategoryDesc)
            waistDownCaption = IIf(IsWaistDownSubClass AndAlso Not String.IsNullOrEmpty(mdseFigureCode), "WD ", String.Empty)
            If Not String.IsNullOrEmpty(mdseFigureCode) AndAlso ShouldAddSize1And2 Then
                size1And2Desc = IIf(Not String.IsNullOrEmpty(Size1Desc) AndAlso Not String.IsNullOrEmpty(Size2Desc), String.Concat(" ", Size1Desc.Trim(), Size2Desc.Trim()), String.Empty)
            End If

            Select Case ImageCategory
                Case [Enum].GetName(GetType(ImageCategoryCode), Enumerations.ImageCategoryCode.STDALN)
                    'imageNotes = String.Concat("SSF ", ModelCategory, If(FeatureID > 0, (String.Concat(" ", FeatureID.ToString())), String.Empty))
                    imageNotes = String.Concat(waistDownCaption, mdseFigureCode, ModelCategoryDesc.Trim(), size1And2Desc)
                Case [Enum].GetName(GetType(ImageCategoryCode), Enumerations.ImageCategoryCode.SWTCH)
                    'imageNotes = String.Concat(mdseFigureCode, ModelCategoryDesc, " ", "SW", If(FeatureID > 0, (String.Concat(" to ", FeatureID.ToString())), String.Empty))
                    'image notes will be added for swatch once the new image id gets created
                    imageNotes = "(swatch"
                Case [Enum].GetName(GetType(ImageCategoryCode), Enumerations.ImageCategoryCode.FEAT)
                    imageNotes = String.Concat(waistDownCaption, mdseFigureCode, ModelCategoryDesc, size1And2Desc, " ", "(sf) ")
                Case [Enum].GetName(GetType(ImageCategoryCode), Enumerations.ImageCategoryCode.REND)
                    imageNotes = String.Concat(waistDownCaption, mdseFigureCode, ModelCategoryDesc, size1And2Desc, " ", "(rs ")
                Case [Enum].GetName(GetType(ImageCategoryCode), Enumerations.ImageCategoryCode.SSF)
                    imageNotes = String.Concat(waistDownCaption, mdseFigureCode, ModelCategoryDesc, size1And2Desc, " ", "SSF")
            End Select
        Catch ex As Exception
            Throw ex
        End Try
        Return imageNotes
    End Function
End Class
