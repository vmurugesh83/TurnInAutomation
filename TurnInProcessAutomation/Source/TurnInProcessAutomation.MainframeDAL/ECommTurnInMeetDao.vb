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
Imports System.Data.SqlClient
Public Class ECommTurnInMeetDao
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")

    Public Sub InsertWebcat(ByVal ISN As Decimal, ByVal WebCategories As String, ByVal UserId As String)
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal), _
                                                          New DB2Parameter("@xmlCategories", DB2Type.VarChar), _
                                                          New DB2Parameter("@last_mod_id", DB2Type.VarChar)
                                                         }
        Dim sql As String = _spSchema + ".TU1043SP"

        parms(0).Value = ISN
        parms(1).Value = WebCategories
        parms(2).Value = UserId

        ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
    End Sub

    Function GetColorSizeDataByMerchId(ByVal TurnInMerchId As Integer) As EcommSetupClrSzInfo
        Dim _ECommColorSizeInfo As New EcommSetupClrSzInfo

        Dim sql As String = _spSchema + ".TU1094SP "
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar)}
        parms(0).Value = TurnInMerchId
        parms(1).Value = _dbSchema

        Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            While (rdr.Read())
                _ECommColorSizeInfo = EcommSetupCreateFactory.ConstructColorSize(rdr)
            End While
        End Using
        Return _ECommColorSizeInfo
    End Function

    Public Function IsKilled(ByVal turnInMerchID As Integer) As Boolean
        Dim sql As String = _spSchema + ".TU1098SP "
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@AdNumber", DB2Type.Integer)}
        parms(0).Value = turnInMerchID

        Dim YorN As String = ""

        YorN = ExecuteScalar(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms).ToString

        If YorN = "Y" Then
            Return True
        Else : Return False
        End If
    End Function

    Function GetEcommTurninMeetByMerchId(ByVal TurnInMerchId As Integer, Optional ByVal IsTIABatch As Boolean = False) As ECommTurnInMeetCreateInfo
        Dim _ECommTunInMeetCreateInfo As New ECommTurnInMeetCreateInfo

        Dim sql As String = _spSchema + ".TU1031SP "
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@AdNumber", DB2Type.Integer), _
                                                          New DB2Parameter("@PageNumber", DB2Type.SmallInt), _
                                                          New DB2Parameter("@BuyerId", DB2Type.SmallInt), _
                                                          New DB2Parameter("@DeptId", DB2Type.SmallInt), _
                                                          New DB2Parameter("@LabelID", DB2Type.Integer), _
                                                          New DB2Parameter("@VendorStyleID", DB2Type.Char), _
                                                          New DB2Parameter("@BatchID", DB2Type.Integer), _
                                                          New DB2Parameter("@TurnInMerchId", DB2Type.Integer), _
                                                          New DB2Parameter("@IsTIABatch", DB2Type.Integer), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar, 20)}
        parms(0).Value = DBNull.Value
        parms(1).Value = DBNull.Value
        parms(2).Value = DBNull.Value
        parms(3).Value = DBNull.Value
        parms(4).Value = DBNull.Value
        parms(5).Value = DBNull.Value
        parms(6).Value = DBNull.Value
        parms(7).Value = TurnInMerchId
        parms(8).Value = IIf(IsTIABatch, 1, 0)
        parms(9).Value = _dbSchema

        Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            While (rdr.Read())
                _ECommTunInMeetCreateInfo = EcommTurnInMeetFactory.Construct(rdr)
            End While
        End Using
        Return _ECommTunInMeetCreateInfo

    End Function

    Function GetEcommTurninMeet(ByVal AdNumber As String, ByVal PageNumber As String, ByVal BuyerId As String, ByVal DeptId As String, ByVal LabelID As String, ByVal VendorStyleID As String,
                                ByVal BatchID As String, ByVal TurnInMerchId As Integer, ByVal IsTIABatch As Boolean) As List(Of ECommTurnInMeetCreateInfo)
        Dim _ECommTunInMeetCreateInfo As New List(Of ECommTurnInMeetCreateInfo)

        Dim sql As String = _spSchema + ".TU1031SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@AdNumber", DB2Type.Integer), _
                                                          New DB2Parameter("@PageNumber", DB2Type.SmallInt), _
                                                          New DB2Parameter("@BuyerId", DB2Type.SmallInt), _
                                                          New DB2Parameter("@DeptId", DB2Type.SmallInt), _
                                                          New DB2Parameter("@LabelID", DB2Type.Integer), _
                                                          New DB2Parameter("@VendorStyleID", DB2Type.Char), _
                                                          New DB2Parameter("@BatchID", DB2Type.Integer), _
                                                          New DB2Parameter("@TurnInMerchId", DB2Type.Integer), _
                                                          New DB2Parameter("@IsTIABatch", DB2Type.Integer), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar, 20)}
        If Not String.IsNullOrEmpty(AdNumber) Then
            parms(0).Value = Convert.ToInt32(AdNumber)
        Else
            parms(0).Value = DBNull.Value
        End If
        If Not String.IsNullOrEmpty(PageNumber) Then
            parms(1).Value = Convert.ToInt32(PageNumber)
        Else
            parms(1).Value = DBNull.Value
        End If

        parms(2).Value = IIf(BuyerId = "", DBNull.Value, BuyerId)
        parms(3).Value = IIf(DeptId = "", DBNull.Value, DeptId)
        parms(4).Value = IIf(LabelID = "", DBNull.Value, LabelID)
        parms(5).Value = IIf(VendorStyleID = "", DBNull.Value, VendorStyleID)
        parms(6).Value = IIf(BatchID = "", DBNull.Value, BatchID)
        parms(7).Value = IIf(TurnInMerchId = 0, DBNull.Value, TurnInMerchId)
        parms(8).Value = IIf(IsTIABatch, 1, 0)
        parms(9).Value = _dbSchema

        Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            While (rdr.Read())
                _ECommTunInMeetCreateInfo.Add(EcommTurnInMeetFactory.Construct(rdr))
            End While
        End Using
        Return _ECommTunInMeetCreateInfo
    End Function

    Public Function UpdateImageRequest(ByVal NewImageId As Integer, ByVal TurnInImageRequestId As Integer, ByVal UserId As String) As Boolean
        Try
            ' Update TTU300IMGE_REQUEST table
            Dim sql As String = _spSchema + ".TU1081SP"
            Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@NewImageId", DB2Type.Integer), _
                                                              New DB2Parameter("@turnin_imge_req_id", DB2Type.Integer), _
                                                              New DB2Parameter("@UserId", DB2Type.VarChar)}
            parms(0).Value = NewImageId
            parms(1).Value = TurnInImageRequestId
            parms(2).Value = UserId
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function UpdateImageFileUrl(ByVal NewImageId As Integer, ByVal UserId As String) As Boolean
        Try
            ' Update TTU310Image table
            Dim sql As String = _spSchema + ".TU1080SP"
            Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@NewImageId", DB2Type.Integer), _
                                                              New DB2Parameter("@image_file_url_txt", DB2Type.VarChar), _
                                                              New DB2Parameter("@UserId", DB2Type.VarChar)}
            parms(0).Value = NewImageId
            parms(1).Value = "" 'TODO
            parms(2).Value = UserId
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Sub SubmitEMM(ByVal ECommTunInMeetCreateInfo As ECommTurnInMeetCreateInfo, ByVal UserId As String, Optional ByVal ShouldUpdateAgeAndGender As Boolean = False)
        Dim sql As String = _spSchema + ".TU1046SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal), _
                                                          New DB2Parameter("@MERCHID", DB2Type.Integer), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar), _
                                                          New DB2Parameter("@FRNDLYPRODDESC", DB2Type.VarChar), _
                                                          New DB2Parameter("@VENDORSTYLNUM", DB2Type.VarChar), _
                                                          New DB2Parameter("@FRNDLYCLR", DB2Type.VarChar), _
                                                          New DB2Parameter("@PICKUP", DB2Type.Integer), _
                                                          New DB2Parameter("@FEATUREID", DB2Type.Integer), _
                                                          New DB2Parameter("@SIZEID", DB2Type.Integer), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar), _
                                                          New DB2Parameter("@UPDATEAGEANDGENDER", DB2Type.SmallInt)
                                                         }

        parms(0).Value = CDec(ECommTunInMeetCreateInfo.ISN)
        parms(1).Value = ECommTunInMeetCreateInfo.turnInMerchID
        parms(2).Value = CStr(UserId)
        parms(3).Value = CStr(ECommTunInMeetCreateInfo.FriendlyProdDesc).Replace("&nbsp;", "")
        parms(4).Value = CStr(ECommTunInMeetCreateInfo.VendorStyleNumber)
        parms(5).Value = CStr(ECommTunInMeetCreateInfo.FriendlyColor).Replace("&nbsp;", "")

        Dim pickupImageId As Integer
        parms(6).Value = If(Int32.TryParse(ECommTunInMeetCreateInfo.PickupImageID, pickupImageId), pickupImageId, 0)

        parms(7).Value = ECommTunInMeetCreateInfo.FeatureID

        Dim sampleSize As Integer
        parms(8).Value = If(Int32.TryParse(ECommTunInMeetCreateInfo.SampleSize, sampleSize), sampleSize, 0)

        parms(9).Value = _dbSchema
        parms(10).Value = IIf(ShouldUpdateAgeAndGender, 1, 0) 'Age and gender will be populated based on history if the value is set as 1

        ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)

    End Sub

    Public Sub RejectBatch(ByVal BatchId As Integer, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1099SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@BatchId", DB2Type.VarChar),
                                                          New DB2Parameter("@USERID", DB2Type.VarChar)}

        parms(0).Value = BatchId
        parms(1).Value = UserId

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function GetAllFeatureID(ByVal ISN As Decimal) As List(Of FeatureIDInfo)
        Dim _FeatureIDInfo As New List(Of FeatureIDInfo)
        Dim sql As String = _spSchema + ".TU1037SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal)}

        parms(0).Value = ISN

        Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            While (rdr.Read())
                _FeatureIDInfo.Add(EcommTurnInMeetFactory.ConstructFeatureID(rdr))
            End While
        End Using
        Return _FeatureIDInfo

    End Function

    Public Sub UpdateEMMFollowupFlag(ByVal TurnInMerchID As Integer, ByVal EMMFollowUpFlag As String, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1100SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@EMM_FLWUP_FLG", DB2Type.VarChar), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar)}

        parms(0).Value = TurnInMerchID
        parms(1).Value = EMMFollowUpFlag
        parms(2).Value = UserId

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateCWFollowupFlag(ByVal TurnInMerchID As Integer, ByVal CWFollowUpFlag As String, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1101SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@CW_FLWUP_FLG", DB2Type.VarChar), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar)}

        parms(0).Value = TurnInMerchID
        parms(1).Value = CWFollowUpFlag
        parms(2).Value = UserId

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateCCFollowupFlag(ByVal TurnInMerchID As Integer, ByVal CCFollowUpFlag As String, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1102SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@CC_FLWUP_FLG", DB2Type.VarChar), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar)}

        parms(0).Value = TurnInMerchID
        parms(1).Value = CCFollowUpFlag
        parms(2).Value = UserId

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateEMMInfo(ByVal merchandiseInfo As ECommTurnInMeetCreateInfo, ByVal UserId As String)

        Dim sql As String = _spSchema + ".TU1038SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                            New DB2Parameter("@FRNDLY_PRD_DESC", DB2Type.VarChar), _
                                                            New DB2Parameter("@FRNDLY_CLR", DB2Type.VarChar), _
                                                            New DB2Parameter("@EMM_NOTES", DB2Type.VarChar), _
                                                            New DB2Parameter("@USERID", DB2Type.VarChar), _
                                                            New DB2Parameter("@SIZE_CATEGORY", DB2Type.VarChar), _
                                                            New DB2Parameter("@EMM_FLWUP_FLG", DB2Type.VarChar), _
                                                            New DB2Parameter("@LABEL_ID", DB2Type.Integer), _
                                                            New DB2Parameter("@ADMIN_MDSE_NUM", DB2Type.Integer)}

        parms(0).Value = merchandiseInfo.turnInMerchID
        parms(1).Value = merchandiseInfo.FriendlyProdDesc
        parms(2).Value = merchandiseInfo.FriendlyColor
        parms(3).Value = merchandiseInfo.EMMNotes
        parms(4).Value = UserId
        parms(5).Value = merchandiseInfo.SizeCategory
        parms(6).Value = merchandiseInfo.EMMFollowUpFlag
        parms(7).Value = merchandiseInfo.LabelID
        parms(8).Value = merchandiseInfo.MerchID

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateCCInfo(ByVal CCInfo As ECommTurnInMeetCreateInfo, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1040SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@HOT_ITM", DB2Type.VarChar), _
                                                          New DB2Parameter("@IMG_GRP", DB2Type.Integer), _
                                                          New DB2Parameter("@IMG_DESC", DB2Type.VarChar), _
                                                          New DB2Parameter("@IMG_NOTES", DB2Type.VarChar), _
                                                          New DB2Parameter("@ALT_VIEW", DB2Type.VarChar), _
                                                          New DB2Parameter("@PICKUPIMGID", DB2Type.Integer), _
                                                          New DB2Parameter("@MODELCATG", DB2Type.VarChar), _
                                                          New DB2Parameter("@CLRCRCT", DB2Type.VarChar), _
                                                          New DB2Parameter("@STYLING_NOTES", DB2Type.VarChar), _
                                                          New DB2Parameter("@CC_FLWUP_FLG", DB2Type.VarChar), _
                                                          New DB2Parameter("@ONOFF_FIGURE", DB2Type.VarChar), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar), _
                                                          New DB2Parameter("@IMAGE_CATEGORY_CDE", DB2Type.VarChar), _
                                                          New DB2Parameter("@IMG_KIND", DB2Type.VarChar), _
                                                          New DB2Parameter("@ADMIN_MDSE_NUM", DB2Type.Integer),
                                                          New DB2Parameter("@INTERNAL_STYLE_NUM", DB2Type.Integer),
                                                          New DB2Parameter("@CLR_CDE", DB2Type.SmallInt),
                                                          New DB2Parameter("@FRIENDLY_COLOR_NME", DB2Type.VarChar),
                                                          New DB2Parameter("@FEATURE_IMAGE_NUM", DB2Type.Integer)
                                                         }

        parms(0).Value = CCInfo.turnInMerchID
        parms(1).Value = CCInfo.HotListCDE
        parms(2).Value = CCInfo.ImageGrp
        parms(3).Value = CCInfo.ImageDesc
        parms(4).Value = CCInfo.ImageNotes
        parms(5).Value = CCInfo.AltView
        parms(6).Value = CCInfo.PickupImageID
        parms(7).Value = CCInfo.ModelCategory
        If CCInfo.ColorCorrect = CChar("") Then
            parms(8).Value = " "
        Else
            parms(8).Value = CCInfo.ColorCorrect
        End If
        parms(9).Value = CCInfo.StylingNotes
        parms(10).Value = CCInfo.CCFollowUpFlag
        parms(11).Value = CCInfo.OnOff
        parms(12).Value = UserId
        parms(13).Value = CCInfo.FeatureSwatch
        parms(14).Value = CCInfo.ImageKindCode
        parms(15).Value = CCInfo.MerchID
        parms(16).Value = CInt(CCInfo.ISN)
        parms(17).Value = CInt(CCInfo.ColorCode)
        parms(18).Value = Trim(CStr(CCInfo.FriendlyColor))
        parms(19).Value = CCInfo.FeatureID

        Try
            ' TODO: TTU410 needs size_id
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateCWInfo(ByVal MerchID As Integer, ByVal CPYNotes As String, FollowUpFlag As String, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1039SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                            New DB2Parameter("@CPYNotes", DB2Type.VarChar), _
                                                            New DB2Parameter("@CW_FLWUP_FLG", DB2Type.VarChar), _
                                                             New DB2Parameter("@USERID", DB2Type.VarChar)
                                                         }

        parms(0).Value = MerchID
        parms(1).Value = CPYNotes
        parms(2).Value = FollowUpFlag
        parms(3).Value = UserId

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateEMMFlood(ByVal TurnInMerchIDs As String, ByVal WebCategoryCode As String, ByVal FriendlyProductDescription As String, ByVal EMMNotes As String, ByVal FriendlyColor As String, ByVal SizeCategory As String, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1086SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_IDS", DB2Type.VarChar), _
                                                          New DB2Parameter("@CATGRY_CDE", DB2Type.Integer), _
                                                          New DB2Parameter("@FRNDLY_PRDCT_DESC", DB2Type.VarChar), _
                                                          New DB2Parameter("@EMM_NOTES", DB2Type.VarChar), _
                                                          New DB2Parameter("@FRNDLY_CLR", DB2Type.VarChar), _
                                                          New DB2Parameter("@SIZE_CATEGORY", DB2Type.VarChar), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar)}

        parms(0).Value = TurnInMerchIDs
        If WebCategoryCode = "" Then
            parms(1).Value = 0
        Else
            parms(1).Value = WebCategoryCode
        End If
        parms(2).Value = FriendlyProductDescription
        parms(3).Value = EMMNotes
        parms(4).Value = FriendlyColor
        parms(5).Value = SizeCategory
        parms(6).Value = UserId
        parms(7).Value = _dbSchema

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateCCFlood(ByVal TurnInMerchIDs As String, ImageType As String, ModelCategory As String, AlternateView As String, ByVal FRS As String, ByVal ImageNotes As String, ByVal StylingNotes As String, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1091SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_IDS", DB2Type.VarChar), _
                                                          New DB2Parameter("@IMG_TYPE", DB2Type.VarChar), _
                                                          New DB2Parameter("@MODEL_CATEGORY_CODE", DB2Type.VarChar), _
                                                          New DB2Parameter("@ALT_VIEW", DB2Type.VarChar), _
                                                          New DB2Parameter("@IMAGE_CATEGORY_CDE", DB2Type.VarChar), _
                                                          New DB2Parameter("@IMG_NOTES", DB2Type.VarChar), _
                                                          New DB2Parameter("@STYLING_NOTES", DB2Type.VarChar), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar)}

        parms(0).Value = TurnInMerchIDs
        parms(1).Value = ImageType
        parms(2).Value = ModelCategory
        parms(3).Value = AlternateView
        parms(4).Value = FRS
        parms(5).Value = ImageNotes
        parms(6).Value = StylingNotes
        parms(7).Value = UserId
        parms(8).Value = _dbSchema

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class
