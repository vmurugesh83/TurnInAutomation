Imports System.Configuration
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Text
Imports log4net
Imports BonTon.DBUtility
Imports BonTon.DBUtility.DB2Helper
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory

Public Class EcommPrioritizationDao
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")

    ''' <summary>
    ''' Method to get all the records for Prioritization page.
    ''' </summary>	    	 
    Public Function GetPrioritizationResults(ByVal strEMMId As String, ByVal strCMGId As String, ByVal strBuyerId As String, ByVal strLabelId As String, ByVal strTUWeek As String, ByVal strImageId As String, ByVal strVendStyleId As String, ByVal strStatus As String) As IList(Of ECommPrioritizationInfo)
        Dim EcommPrioritizationInfos As New List(Of ECommPrioritizationInfo)

        Dim sql As String = _spSchema + ".TU1044SP"

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@EMM_ID", DB2Type.SmallInt), _
                                                          New DB2Parameter("@CMG_ID", DB2Type.SmallInt), _
                                                          New DB2Parameter("@BUYER_ID", DB2Type.SmallInt), _
                                                          New DB2Parameter("@LABEL_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@TU_WEEK_AD_NBR", DB2Type.Integer), _
                                                          New DB2Parameter("@IMAGE_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@VENDOR_STYLE_NUM", DB2Type.VarChar), _
                                                          New DB2Parameter("@STATUS", DB2Type.Char), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar, 20)}

        parms(0).Value = IIf(strEMMId = "", DBNull.Value, strEMMId)
        parms(1).Value = IIf(strCMGId = "", DBNull.Value, strCMGId)
        parms(2).Value = IIf(strBuyerId = "", DBNull.Value, strBuyerId)
        parms(3).Value = IIf(strLabelId = "", DBNull.Value, strLabelId)
        parms(4).Value = IIf(strTUWeek = "", DBNull.Value, strTUWeek)
        parms(5).Value = IIf(strImageId = "", DBNull.Value, strImageId)
        parms(6).Value = IIf(strVendStyleId = "", DBNull.Value, strVendStyleId)
        parms(7).Value = IIf(strStatus = "", DBNull.Value, CChar(strStatus))
        parms(8).Value = _dbSchema

        Try

            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    EcommPrioritizationInfos.Add(EcommPrioritizationFactory.Construct(rdr))
                End While
            End Using
            Return EcommPrioritizationInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to get all the Color Level records for Prioritization page.
    ''' </summary>	    	 
    Public Function GetColorLevelResults(ByVal decISN As Decimal, ByVal chrStatus As Char, ByVal decAdNum As Decimal) As IList(Of ECommPrioritizationInfo)
        Dim EcommColorLevelInfos As New List(Of ECommPrioritizationInfo)

        Dim sql As String = _spSchema + ".TU1056SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal), _
                                                          New DB2Parameter("@STATUS", DB2Type.Char),
                                                          New DB2Parameter("@AD_NUM", DB2Type.Decimal)}

        parms(0).Value = decISN
        parms(1).Value = chrStatus
        parms(2).Value = decAdNum

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    EcommColorLevelInfos.Add(EcommPrioritizationFactory.Construct(rdr))
                End While
            End Using
            Return EcommColorLevelInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to get all the Size Level records for Prioritization page.
    ''' </summary>	    	 
    Public Function GetSizeLevelResults(ByVal ISN As Decimal, ByVal MerchID As Integer, ByVal chrStatus As String) As IList(Of ECommPrioritizationInfo)
        Dim EcommColorLevelInfos As New List(Of ECommPrioritizationInfo)

        Dim sql As String = _spSchema + ".TU1057SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@STATUS", DB2Type.Char)}

        parms(0).Value = MerchID
        parms(1).Value = chrStatus

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    EcommColorLevelInfos.Add(EcommPrioritizationFactory.Construct(rdr))
                End While
            End Using
            Return EcommColorLevelInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to populate the Color Family combo box at Color Hierarchy Level.
    ''' </summary>	    	 
    Public Function GetClrSizFamilyLookUp(ByVal ISN As Decimal, ByVal OptionCde As Short) As List(Of ClrSizLocLookUp)
        Dim ColorFamilyValues As New List(Of ClrSizLocLookUp)

        Dim sql As String = _spSchema + ".TU1048SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal), _
                                                          New DB2Parameter("@OPTION_CDE", DB2Type.SmallInt)}

        parms(0).Value = ISN
        parms(1).Value = OptionCde

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    ColorFamilyValues.Add(ClrSizLocLookupFactory.Construct(rdr))
                End While
            End Using
            Return ColorFamilyValues
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Public Function GetAllSizeFamilyLookUp() As List(Of ClrSizLocLookUp)
        Dim SizeFamilyValues As New List(Of ClrSizLocLookUp)

        Dim sql As String = _spSchema + ".TU1112SP"
        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, Nothing)
                While (rdr.Read())
                    SizeFamilyValues.Add(ClrSizLocLookupFactory.Construct(rdr))
                End While
            End Using
            Return SizeFamilyValues
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to populate the Non Swatch Color combo box at Color Hierarchy Level.
    ''' </summary>	    	 
    Public Function GetNonSwatchClrLookUp() As List(Of ClrSizLocLookUp)
        Dim NonSwatchClrValues As New List(Of ClrSizLocLookUp)

        Dim sql As String = _spSchema + ".TU1066SP"

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql)
                While (rdr.Read())
                    NonSwatchClrValues.Add(ClrSizLocLookupFactory.Construct(rdr))
                End While
            End Using
            Return NonSwatchClrValues
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to populate the Webcat Size combo box at Size Hierarchy Level.
    ''' </summary>	    	 
    Public Function GetWebcatSizeLookUp() As List(Of ClrSizLocLookUp)
        Dim WebcatSizeValues As New List(Of ClrSizLocLookUp)

        Dim sql As String = _spSchema + ".TU1067SP"

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql)
                While (rdr.Read())
                    WebcatSizeValues.Add(ClrSizLocLookupFactory.Construct(rdr))
                End While
            End Using
            Return WebcatSizeValues
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Public Sub UpdateISNLevelData(ByVal ISNLevelData As ECommPrioritizationInfo, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1055SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal), _
                                                          New DB2Parameter("@SWATCH_FLG", DB2Type.Char), _
                                                          New DB2Parameter("@COLOR_FLG", DB2Type.Char), _
                                                          New DB2Parameter("@SIZE_FLG", DB2Type.Char), _
                                                          New DB2Parameter("@CATGRY_CDE", DB2Type.Integer), _
                                                          New DB2Parameter("@LABEL_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@BRAND_ID", DB2Type.SmallInt), _
                                                          New DB2Parameter("@AGE_CDE", DB2Type.SmallInt), _
                                                          New DB2Parameter("@GENDER_CDE", DB2Type.SmallInt), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar)}

        parms(0).Value = ISNLevelData.ISN
        parms(1).Value = ISNLevelData.SwatchFlg
        parms(2).Value = ISNLevelData.ColorFlg
        parms(3).Value = ISNLevelData.SizeFlg
        parms(4).Value = ISNLevelData.WebCatgyCde
        parms(5).Value = ISNLevelData.LabelID
        parms(6).Value = ISNLevelData.BrandID
        parms(7).Value = ISNLevelData.AgeCde
        parms(8).Value = ISNLevelData.GenderCde
        parms(9).Value = UserId

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateColorLevelData(ByVal ColorLevelData As ECommPrioritizationInfo, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1058SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@FRNDLY_CLR", DB2Type.VarChar), _
                                                          New DB2Parameter("@FEATURE_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@IMAGE_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@CLR_FAMILY", DB2Type.VarChar), _
                                                          New DB2Parameter("@FRNDLY_PROD_DESC", DB2Type.VarChar), _
                                                          New DB2Parameter("@NON_SWATCH_CLR_CDE", DB2Type.Integer), _
                                                          New DB2Parameter("@F_R_S", DB2Type.VarChar), _
                                                          New DB2Parameter("@EMM_NOTES_TXT", DB2Type.VarChar), _
                                                          New DB2Parameter("@IMAGE_MDSE_GRP_NUM", DB2Type.SmallInt), _
                                                          New DB2Parameter("@IMG_NOTES", DB2Type.VarChar), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar)}

        parms(0).Value = ColorLevelData.TurnInMerchID
        parms(1).Value = ColorLevelData.FriendlyColor
        parms(2).Value = ColorLevelData.FeatureID
        parms(3).Value = ColorLevelData.ImageID
        parms(4).Value = ColorLevelData.ColorFamily
        parms(5).Value = ColorLevelData.ProductName
        parms(6).Value = ColorLevelData.NonSwatchClrCde
        parms(7).Value = ColorLevelData.FRS
        parms(8).Value = ColorLevelData.EMMNotes
        parms(9).Value = ColorLevelData.ImageGroup
        parms(10).Value = ColorLevelData.ImageNotes
        parms(11).Value = UserId

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub DeleteColorLevelData(ByVal MerchId As Integer, ByVal chrStatus As Char, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1092SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@STATUS", DB2Type.Char), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar)}

        parms(0).Value = MerchId
        parms(1).Value = chrStatus
        parms(2).Value = UserId

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateSizeLevelData(ByVal SizeLevelData As ECommPrioritizationInfo, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1068SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@UPC_NUM", DB2Type.Decimal), _
                                                          New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@WEBCAT_SIZE", DB2Type.Integer), _
                                                          New DB2Parameter("@SIZE_FAMILY", DB2Type.VarChar), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar)}

        parms(0).Value = SizeLevelData.UPC
        parms(1).Value = SizeLevelData.TurnInMerchID
        parms(2).Value = SizeLevelData.WebCatSizeID
        parms(3).Value = SizeLevelData.SizeFamily
        parms(4).Value = UserId

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateISNLevelDataFlood(ByVal ISNs As String, ByVal ISNLevelData As ECommPrioritizationInfo, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1070SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISNs", DB2Type.VarChar), _
                                                          New DB2Parameter("@SWATCH_FLG", DB2Type.Char), _
                                                          New DB2Parameter("@COLOR_FLG", DB2Type.Char), _
                                                          New DB2Parameter("@SIZE_FLG", DB2Type.Char), _
                                                          New DB2Parameter("@CATGRY_CDE", DB2Type.Integer), _
                                                          New DB2Parameter("@LABEL_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@BRAND_ID", DB2Type.SmallInt), _
                                                          New DB2Parameter("@AGE_CDE", DB2Type.SmallInt), _
                                                          New DB2Parameter("@GENDER_CDE", DB2Type.SmallInt), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar)}

        parms(0).Value = ISNs

        If ISNLevelData.SwatchFlg = CChar("") Then
            parms(1).Value = DBNull.Value
        Else
            parms(1).Value = ISNLevelData.SwatchFlg
        End If

        If ISNLevelData.ColorFlg = CChar("") Then
            parms(2).Value = DBNull.Value
        Else
            parms(2).Value = ISNLevelData.ColorFlg
        End If

        If ISNLevelData.SizeFlg = CChar("") Then
            parms(3).Value = DBNull.Value
        Else
            parms(3).Value = ISNLevelData.SizeFlg
        End If

        'parms(7).Value = If(String.IsNullOrEmpty(ISNLevelData.SampleStore), 0, CShort(ColorSizeData.SampleStore))

        parms(4).Value = ISNLevelData.WebCatgyCde
        parms(5).Value = ISNLevelData.LabelID
        parms(6).Value = ISNLevelData.BrandID
        parms(7).Value = ISNLevelData.AgeCde
        parms(8).Value = ISNLevelData.GenderCde
        parms(9).Value = UserId
        parms(10).Value = _dbSchema

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateColorLevelDataFlood(ByVal MerchIDs As String, ByVal ClrLevelData As ECommPrioritizationInfo, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1071SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_IDs", DB2Type.VarChar), _
                                                          New DB2Parameter("@FEATURE_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@FRNDLY_PROD_DESC", DB2Type.VarChar), _
                                                          New DB2Parameter("@MSDE_GROUP_SEQ_NUM", DB2Type.SmallInt), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar)}

        parms(0).Value = MerchIDs
        parms(1).Value = ClrLevelData.FeatureID
        parms(2).Value = ClrLevelData.ProductName
        parms(3).Value = ClrLevelData.ImageGroup
        parms(4).Value = UserId
        parms(5).Value = _dbSchema

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateSizeLevelDataFlood(ByVal strFindSize As String, ByVal strReplaceSize As String, ByVal strFindSizeFam As String, ByVal strReplaceSizeFam As String, _
                                        ByVal strFindVendorSize As String, ByVal strVendorReplaceSize As String, ByVal strVendorReplaceSizeFam As String, _
                                        ByVal UserId As String, ByVal strMerchIds As String)

        Dim sql As String = _spSchema + ".TU1078SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@FIND_SIZE", DB2Type.VarChar), _
                                                          New DB2Parameter("@REPLACE_SIZE", DB2Type.VarChar), _
                                                          New DB2Parameter("@FIND_SIZEFAM", DB2Type.VarChar), _
                                                          New DB2Parameter("@REPLACE_SIZEFAM", DB2Type.VarChar), _
                                                          New DB2Parameter("@FIND_VENDOR_SIZE", DB2Type.VarChar), _
                                                          New DB2Parameter("@REPLACE_WC_SIZE", DB2Type.VarChar), _
                                                          New DB2Parameter("@REPLACE_WC_SIZEFAM", DB2Type.VarChar), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar), _
                                                          New DB2Parameter("@MERCHIDs", DB2Type.VarChar), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar)}

        parms(0).Value = strFindSize
        parms(1).Value = strReplaceSize
        parms(2).Value = strFindSizeFam
        parms(3).Value = strReplaceSizeFam
        parms(4).Value = strFindVendorSize
        parms(5).Value = strVendorReplaceSize
        parms(6).Value = strVendorReplaceSizeFam
        parms(7).Value = UserId
        parms(8).Value = strMerchIds
        parms(9).Value = _dbSchema

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Method to get the list of Image IDs.
    ''' </summary>	    	 
    Public Function GetImageIDs(ByVal ISNs As String, ByVal chrStatus As Char, ByVal Ads As String) As List(Of ImageInfo)
        Dim ImageIDs As New List(Of ImageInfo)

        Dim sql As String = _spSchema + ".TU1064SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISNs", DB2Type.VarChar), _
                                                          New DB2Parameter("@STATUS", DB2Type.Char), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar), _
                                                          New DB2Parameter("@Ads", DB2Type.VarChar)}

        parms(0).Value = ISNs
        parms(1).Value = chrStatus
        parms(2).Value = _dbSchema
        parms(3).Value = Ads

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new EcommSetupClrSzInfo object via factory method and add to list
                    ImageIDs.Add(ImageFactory.Construct(rdr))
                End While
            End Using
            Return ImageIDs
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to retrieve an User's email id.
    ''' </summary>	    	 
    Public Function GetEmailAddress(ByVal UserId As String) As String
        Dim EmailID As String = String.Empty

        Dim sql As String = _spSchema + ".TU1069SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@USERID", DB2Type.VarChar)}

        parms(0).Value = UserId

        Try
            Return CStr(ExecuteScalar(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms))
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to check whether the Feature ID already exists in TTU720PRODUCT table.
    ''' </summary>	    	 
    Public Function FeatureExistsOnReturnedProduct(ByVal FeatureIdNum As Integer) As Integer

        Dim sql As String = _spSchema + ".TU1083SP"

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@FEATURE_ID", DB2Type.Integer)}
        parms(0).Value = FeatureIdNum

        Try
            Return CInt(ExecuteScalar(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms))
        Catch ex As Exception
            Throw ex
        End Try
        Return 0
    End Function

    ''' <summary>
    ''' Method to check whether the UPC data already exists in TTU750PRODUCT_UPC table.
    ''' </summary>	    	 
    Public Function GetExistingUPCOnWebCat(ByVal TurnInMerchID As Integer) As List(Of String)
        Dim retList As New List(Of String)
        Dim sql As String = _spSchema + ".TU1087SP"

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer)}
        parms(0).Value = TurnInMerchID

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    retList.Add(rdr("UPC_NUM").ToString)
                End While
            End Using
            Return retList
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub SubmitToWebCat(ByVal MerchId As Integer, ByVal AdminImgNotes As String, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1063SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@ADMIN_IMG_NOTES", DB2Type.VarChar), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar)}

        parms(0).Value = MerchId

        If AdminImgNotes.Length > 35 Then
            parms(1).Value = AdminImgNotes.Substring(0, 35).Trim
        Else
            parms(1).Value = AdminImgNotes.Trim
        End If

        parms(2).Value = UserId

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub SubmitUPCdataToWebCat(ByVal MerchId As Integer, ByVal AdminImgNotes As String, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1084SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@ADMIN_IMG_NOTES", DB2Type.VarChar), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar)}

        parms(0).Value = MerchId

        If AdminImgNotes.Length > 35 Then
            parms(1).Value = AdminImgNotes.Substring(0, 35).Trim
        Else
            parms(1).Value = AdminImgNotes.Trim
        End If

        parms(2).Value = UserId

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Method to check whether the Image is processed in TTU720PRODUCT table.
    ''' </summary>	    	 
    Public Function IsImageProcessedInWebCat(ByVal ImageID As Integer) As Boolean
        Dim intImageProcessedCount As Integer

        Dim sql As String = _spSchema + ".TU1090SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@IMAGE_ID", DB2Type.Integer)}

        parms(0).Value = ImageID

        Try
            intImageProcessedCount = CInt(ExecuteScalar(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms))

            If intImageProcessedCount > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Public Function AreUPCsProcessedForImage(ByVal ImageID As Integer) As Boolean
        Dim intImageProcessedCount As Integer

        Dim sql As String = _spSchema + ".TU1106SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@IMAGE_ID", DB2Type.Integer)}

        parms(0).Value = ImageID

        Try
            intImageProcessedCount = CInt(ExecuteScalar(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms))

            If intImageProcessedCount > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Public Sub UpdateWebCatImportStatus(ByVal MerchId As Integer, ByVal Status As Char, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1065SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@SUCCESS", DB2Type.Char), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar)}

        parms(0).Value = MerchId
        parms(1).Value = Status
        parms(2).Value = UserId

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function GetWebCatImportProductDetailCopy(ByVal productCde As Integer) As String
        Dim productLongDesc As String = ""
        Dim sql As String = "SELECT PRODUCT_LONG_DESC FROM " + _dbSchema + ".TEC120_PRODUCT WHERE PRODUCT_CDE = ? "
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@PRODUCT_CDE", DB2Type.Integer)}

        parms(0).Value = productCde

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.Text, sql, parms)
                While (rdr.Read())
                    productLongDesc = CStr(rdr("PRODUCT_LONG_DESC"))
                End While
            End Using
            Return productLongDesc
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Public Sub UpdateWebCatImportProductDetailCopy(ByVal productCde As Integer, ByVal productLongDesc As String)
        Dim sql As String = "UPDATE " + _dbSchema + ".TEC120_PRODUCT SET PRODUCT_LONG_DESC = ? WHERE PRODUCT_CDE = ?"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@PRODUCT_LONG_DESC", DB2Type.VarChar), _
                                                          New DB2Parameter("@PRODUCT_CDE", DB2Type.Integer)}

        parms(0).Value = productLongDesc
        parms(1).Value = productCde

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.Text, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Method to get all the records for exporting to excel. This is a temporary method and will be removed eventually.
    ''' </summary>	    	 
    Public Function GetPrioritizationResults(ByVal CategoryID As String, ByVal Status As String) As IList(Of ECommPrioritizationInfo)
        Dim EcommPrioritizationInfos As New List(Of ECommPrioritizationInfo)

        Dim sql As String = _spSchema + ".TU1105SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@CATEGORY_CODE", DB2Type.Integer), _
                                                          New DB2Parameter("@STATUS", DB2Type.Char)}

        parms(0).Value = IIf(CategoryID = "", DBNull.Value, CategoryID)
        parms(1).Value = IIf(Status = "", DBNull.Value, CChar(Status))

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    EcommPrioritizationInfos.Add(EcommPrioritizationFactory.Construct(rdr))
                End While
            End Using
            Return EcommPrioritizationInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to get all the categories for export. This is a temporary method and will be removed eventually.
    ''' </summary>	    	 
    Public Function ExportCategories(ByVal Status As String) As IList(Of String)
        Dim WebCatInTurnIn As New List(Of String)

        Dim sql As String = "SELECT WC.CATEGORY_CDE FROM " + _dbSchema + ".TTU600WEBCAT_STAGE WC "
        sql += " JOIN " + _dbSchema + ".TTU510ISN_CATEGORY TTU510 ON  WC.CATEGORY_CDE = TTU510.CATEGORY_CDE "
        sql += "	LEFT JOIN " + _dbSchema + ".TEC100_CATEGORY CAT ON TTU510.CATEGORY_CDE = CAT.CATEGORY_CDE "
        sql += "WHERE WC.WCAT_LOAD_STAT_FLG = '" + Status + "' GROUP BY WC.CATEGORY_CDE"

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.Text, sql)
                While (rdr.Read())
                    WebCatInTurnIn.Add(CStr(rdr("CATEGORY_CDE")))
                End While
            End Using
            Return WebCatInTurnIn
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
    ''' <summary>
    ''' Updates the image id and feature id of the merchandise in the TTU600 table
    ''' </summary>
    ''' <param name="ImageID"></param>
    ''' <param name="FeatureID"></param>
    ''' <param name="TurnInMerchID"></param>
    ''' <param name="UserId"></param>
    ''' <remarks></remarks>
    Public Sub UpdateImageIDFeatureID(ByVal ImageID As Integer, ByVal FeatureID As Integer, ByVal TurnInMerchID As Integer, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1170SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@FEATURE_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@IMAGE_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar)}

        parms(0).Value = TurnInMerchID
        parms(1).Value = FeatureID
        parms(2).Value = ImageID
        parms(3).Value = UserId

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class
