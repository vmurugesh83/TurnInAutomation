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


Partial Public Class EcommSetupCreateDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")

    ''' <summary>
    ''' Method to get all EcommSetupCreateInfo records.
    ''' </summary>
    Public Function GetAllEcommSetupCreateResultsByHierarchy(ByVal AdNum As Integer, ByVal PageNum As Integer, ByVal StatusCodes As String, ByVal DeptId As Int16, ByVal ClassId As Int16, ByVal SubClassId As Int16, _
                                                             ByVal VendorId As Integer, ByVal VendorStyleNum As String, ByVal ACode1 As String, ByVal ACode2 As String, ByVal ACode3 As String, ByVal ACode4 As String, _
                                                             ByVal SellYear As Int16, ByVal SeasonId As Integer, ByVal CreatedSince As Date?) As IList(Of EcommSetupCreateInfo)
        Dim EcommSetupCreateInfos As New List(Of EcommSetupCreateInfo)
        'Dim sql As String = "TC" + ".TU1013SP_TEST_TWO"
        Dim sql As String = _spSchema + ".TU1013SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@StatusCodes", DB2Type.VarChar), _
                                                          New DB2Parameter("@DeptId", DB2Type.SmallInt), _
                                                          New DB2Parameter("@ClassId", DB2Type.SmallInt), _
                                                          New DB2Parameter("@SubClassId", DB2Type.SmallInt), _
                                                          New DB2Parameter("@VendorId", DB2Type.Integer), _
                                                          New DB2Parameter("@VendorStyleNum", DB2Type.VarChar), _
                                                          New DB2Parameter("@ACode1", DB2Type.VarChar), _
                                                          New DB2Parameter("@ACode2", DB2Type.VarChar), _
                                                          New DB2Parameter("@ACode3", DB2Type.VarChar), _
                                                          New DB2Parameter("@ACode4", DB2Type.VarChar), _
                                                          New DB2Parameter("@SellYear", DB2Type.SmallInt), _
                                                          New DB2Parameter("@SeasonId", DB2Type.Integer), _
                                                          New DB2Parameter("@CreatedSince", DB2Type.Date), _
                                                          New DB2Parameter("@ad_num", DB2Type.Integer), _
                                                          New DB2Parameter("@ad_system_page_num", DB2Type.SmallInt)}

        parms(0).Value = StatusCodes
        parms(1).Value = DeptId
        parms(2).Value = ClassId
        parms(3).Value = SubClassId
        parms(4).Value = VendorId
        parms(5).Value = VendorStyleNum
        parms(6).Value = ACode1
        parms(7).Value = ACode2
        parms(8).Value = ACode3
        parms(9).Value = ACode4
        parms(10).Value = SellYear
        parms(11).Value = SeasonId
        If CreatedSince Is Nothing Then
            parms(12).Value = DBNull.Value
        Else
            parms(12).Value = CreatedSince
        End If
        parms(13).Value = AdNum
        parms(14).Value = PageNum

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new EcommSetupCreateInfo object via factory method and add to list
                    EcommSetupCreateInfos.Add(EcommSetupCreateFactory.Construct(rdr))
                End While
            End Using
            Return EcommSetupCreateInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
    Public Function GetAllEcommSetupCreateResultsByHierarchyVendor(ByVal AdNum As Integer, ByVal PageNum As Integer, ByVal StatusCodes As String, ByVal DeptId As Int16, ByVal ClassId As Int16, ByVal SubClassId As Int16, ByVal VendorId As Integer, ByVal ACode1 As String, ByVal ACode2 As String, ByVal ACode3 As String, ByVal ACode4 As String, ByVal SellYear As Int16, ByVal SeasonId As Integer, ByVal CreatedSince As Date?) As IList(Of EcommSetupCreateInfo)
        Dim EcommSetupCreateInfos As New List(Of EcommSetupCreateInfo)
        Dim sql As String = _spSchema + ".TU1013SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@StatusCodes", DB2Type.VarChar), _
                                                          New DB2Parameter("@DeptId", DB2Type.SmallInt), _
                                                          New DB2Parameter("@ClassId", DB2Type.SmallInt), _
                                                          New DB2Parameter("@SubClassId", DB2Type.SmallInt), _
                                                          New DB2Parameter("@VendorId", DB2Type.Integer), _
                                                          New DB2Parameter("@ACode1", DB2Type.VarChar), _
                                                          New DB2Parameter("@ACode2", DB2Type.VarChar), _
                                                          New DB2Parameter("@ACode3", DB2Type.VarChar), _
                                                          New DB2Parameter("@ACode4", DB2Type.VarChar), _
                                                          New DB2Parameter("@SellYear", DB2Type.SmallInt), _
                                                          New DB2Parameter("@SeasonId", DB2Type.Integer), _
                                                          New DB2Parameter("@CreatedSince", DB2Type.Date), _
                                                          New DB2Parameter("@ad_num", DB2Type.Integer), _
                                                          New DB2Parameter("@ad_system_page_num", DB2Type.SmallInt)}

        parms(0).Value = StatusCodes
        parms(1).Value = DeptId
        parms(2).Value = ClassId
        parms(3).Value = SubClassId
        parms(4).Value = IIf(VendorId = Nothing, 0, VendorId)
        'parms(5).Value = VendorStyleNum
        parms(5).Value = ACode1
        parms(6).Value = ACode2
        parms(7).Value = ACode3
        parms(8).Value = ACode4
        parms(9).Value = SellYear
        parms(10).Value = SeasonId
        If CreatedSince Is Nothing Then
            parms(11).Value = DBNull.Value
        Else
            parms(11).Value = CreatedSince
        End If
        parms(12).Value = AdNum
        parms(13).Value = PageNum

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new EcommSetupCreateInfo object via factory method and add to list
                    EcommSetupCreateInfos.Add(EcommSetupCreateFactory.Construct(rdr))
                End While
            End Using
            Return EcommSetupCreateInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
    ''' <summary>
    ''' Method to get all EcommSetupCreateInfo records.
    ''' </summary>	    	 
    Public Function GetAllEcommSetupCreateResultsByISNs(ByVal AdNum As Integer, ByVal PageNum As Integer, ByVal ISNs As String, ByVal ReserveISNs As String) As IList(Of EcommSetupCreateInfo)
        Dim EcommSetupCreateInfos As New List(Of EcommSetupCreateInfo)

        Dim sql As String = _spSchema + ".TU1012SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISNs", DB2Type.VarChar), _
                                                          New DB2Parameter("@RESERVE_ISNs", DB2Type.VarChar), _
                                                          New DB2Parameter("@ad_num", DB2Type.Integer), _
                                                          New DB2Parameter("@ad_system_page_num", DB2Type.SmallInt), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar)}

        parms(0).Value = ISNs
        parms(1).Value = ReserveISNs
        parms(2).Value = AdNum
        parms(3).Value = PageNum
        parms(4).Value = _dbSchema

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new EcommSetupCreateInfo object via factory method and add to list
                    EcommSetupCreateInfos.Add(EcommSetupCreateFactory.Construct(rdr))
                End While
            End Using
            Return EcommSetupCreateInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Function GetAllEcommSetupCreateDetail(ByVal ISN As Decimal) As IList(Of EcommSetupCreateInfo)
        Dim EcommSetupCreateInfos As New List(Of EcommSetupCreateInfo)

        Dim sql As String = _spSchema + ".TU1010SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal)}

        parms(0).Value = ISN

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new EcommSetupCreateInfo object via factory method and add to list
                    EcommSetupCreateInfos.Add(EcommSetupCreateFactory.Construct(rdr))
                End While
            End Using
            Return EcommSetupCreateInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Function GetISNExists(ByVal ISN As Decimal, ByVal IsReserve As Boolean) As Boolean
        Dim sql As String = _spSchema + ".TU1011SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal), _
                                                          New DB2Parameter("@IsReserve", DB2Type.Integer), _
                                                          New DB2Parameter("@ISVALID", DB2Type.Integer) _
                                                         }

        parms(0).Value = ISN
        parms(1).Value = If(IsReserve, 1, 0)
        parms(2).Direction = ParameterDirection.ReturnValue

        ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Return CBool(parms(2).Value)
    End Function

    Function GetISNLevelDetailByISN(ByVal ISN As Decimal, ByVal IsReserve As Boolean) As EcommSetupCreateInfo
        Dim EcommSetupCreateInfos As New EcommSetupCreateInfo

        Dim sql As String = _spSchema + ".TU1014SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal), _
                                                          New DB2Parameter("@IsReserve", DB2Type.Integer)
                                                         }

        parms(0).Value = ISN
        parms(1).Value = If(IsReserve, 1, 0)

        Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            While (rdr.Read())
                EcommSetupCreateInfos = EcommSetupCreateFactory.Construct(rdr)
                EcommSetupCreateInfos.ISN = ISN
                EcommSetupCreateInfos.IsReserve = IsReserve
            End While
        End Using
        Return EcommSetupCreateInfos
    End Function

    Function GetEcommSetupMaintenanceResults(ByVal BatchId As Integer) As List(Of EcommSetupCreateInfo)
        Dim EcommSetupCreateInfos As New List(Of EcommSetupCreateInfo)

        Dim sql As String = _spSchema + ".TU1021SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@BatchId", DB2Type.Integer)}

        parms(0).Value = BatchId

        Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            While (rdr.Read())
                Dim item As EcommSetupCreateInfo = EcommSetupCreateFactory.Construct(rdr)
                item.Saved = True
                EcommSetupCreateInfos.Add(item)
            End While
        End Using
        Return EcommSetupCreateInfos
    End Function

    Public Sub InsertISNData(ByVal ISN As Decimal, ByVal LabelId As Integer, ByVal IsReserve As String, ByVal SizeCategoryCode As String, ByVal WebCategories As String, ByVal UserId As String)
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal), _
                                                          New DB2Parameter("@label_id", DB2Type.Integer), _
                                                          New DB2Parameter("@reserved_isn_flg", DB2Type.VarChar), _
                                                          New DB2Parameter("@isn_size_catgy_cde", DB2Type.VarChar), _
                                                          New DB2Parameter("@xmlCategories", DB2Type.VarChar), _
                                                          New DB2Parameter("@last_mod_id", DB2Type.VarChar)
                                                         }
        Dim sql As String = _spSchema + ".TU1018SP"

        parms(0).Value = ISN
        parms(1).Value = LabelId
        parms(2).Value = IsReserve
        parms(3).Value = SizeCategoryCode
        parms(4).Value = WebCategories
        parms(5).Value = UserId

        ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
    End Sub

    Public Function InsertISNDataColorLevel(ByVal ISN As Decimal, ByVal DeptID As Integer, ByVal ColorCode As String, ByVal UsageCode As Integer, ByVal ImageName As String, ByVal AdNum As String,
                                            ByVal PageNum As String, ByVal IsReserve As String, ByVal AdditionalSamplesFlag As String, ByVal ModelCategoryCode As String, ByVal VendorApprovalFlag As String,
                                            ByVal UserId As String, ByVal BatchId As String) As Integer

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal), _
                                                          New DB2Parameter("@dept_id", DB2Type.SmallInt), _
                                                          New DB2Parameter("@turn_in_usage_cde", DB2Type.Integer), _
                                                          New DB2Parameter("@image_nme", DB2Type.VarChar), _
                                                          New DB2Parameter("@ad_num", DB2Type.Integer), _
                                                          New DB2Parameter("@ad_system_page_num", DB2Type.SmallInt), _
                                                          New DB2Parameter("@reserved_isn_flg", DB2Type.VarChar), _
                                                          New DB2Parameter("@addl_samples_flg", DB2Type.VarChar), _
                                                          New DB2Parameter("@model_category_cde", DB2Type.VarChar), _
                                                          New DB2Parameter("@imge_vend_appv_flg", DB2Type.VarChar), _
                                                          New DB2Parameter("@color_code", DB2Type.VarChar), _
                                                          New DB2Parameter("@last_mod_id", DB2Type.VarChar), _
                                                          New DB2Parameter("@BatchId", DB2Type.Integer), _
                                                          New DB2Parameter("@image_category_code", DB2Type.Char, 6), _
                                                          New DB2Parameter("@mdse_figure_code", DB2Type.Char, 3), _
                                                          New DB2Parameter("@turn_in_merch_id", DB2Type.Integer), _
                                                          New DB2Parameter("@friendly_prod_desc", DB2Type.VarChar, 255)
                                                         }
        Dim sql As String = _spSchema + ".TU1019SP"

        parms(0).Value = ISN
        parms(1).Value = CShort(DeptID)
        parms(2).Value = UsageCode
        parms(3).Value = ImageName
        parms(4).Value = CInt(AdNum)

        If PageNum.Contains("-"c) Then
            parms(5).Value = CShort(PageNum.Substring(0, PageNum.IndexOf("-"c)).Trim)
        Else
            parms(5).Value = CShort(PageNum.Trim)
        End If

        parms(6).Value = IsReserve
        parms(7).Value = AdditionalSamplesFlag
        parms(8).Value = ModelCategoryCode
        parms(9).Value = VendorApprovalFlag
        parms(10).Value = ColorCode
        parms(11).Value = UserId

        parms(12).Direction = ParameterDirection.InputOutput
        parms(12).Value = BatchId
        parms(13).Value = String.Empty
        parms(14).Value = String.Empty
        parms(15).Direction = ParameterDirection.InputOutput
        parms(15).Value = 0
        parms(16).Value = String.Empty

        ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)

        Return CInt(parms(12).Value)
    End Function
    Public Function InsertISNDataColorLevel(ByVal ISN As Decimal, ByVal DeptID As Integer, ByVal ColorCode As String, ByVal UsageCode As Integer, ByVal ImageName As String, ByVal AdNum As String,
                                        ByVal PageNum As String, ByVal IsReserve As String, ByVal AdditionalSamplesFlag As String, ByVal ModelCategoryCode As String, ByVal VendorApprovalFlag As String,
                                        ByVal UserId As String, ByVal BatchId As String, ByVal ImageCategoryCode As String, ByVal OnOffFigureCode As String,
                                        ByVal FriendlyProductDesc As String, ByRef TurnInMerchID As Integer) As Integer

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal), _
                                                          New DB2Parameter("@dept_id", DB2Type.SmallInt), _
                                                          New DB2Parameter("@turn_in_usage_cde", DB2Type.Integer), _
                                                          New DB2Parameter("@image_nme", DB2Type.VarChar), _
                                                          New DB2Parameter("@ad_num", DB2Type.Integer), _
                                                          New DB2Parameter("@ad_system_page_num", DB2Type.SmallInt), _
                                                          New DB2Parameter("@reserved_isn_flg", DB2Type.VarChar), _
                                                          New DB2Parameter("@addl_samples_flg", DB2Type.VarChar), _
                                                          New DB2Parameter("@model_category_cde", DB2Type.VarChar), _
                                                          New DB2Parameter("@imge_vend_appv_flg", DB2Type.VarChar), _
                                                          New DB2Parameter("@color_code", DB2Type.VarChar), _
                                                          New DB2Parameter("@last_mod_id", DB2Type.VarChar), _
                                                          New DB2Parameter("@BatchId", DB2Type.Integer), _
                                                          New DB2Parameter("@image_category_code", DB2Type.Char, 6), _
                                                          New DB2Parameter("@mdse_figure_code", DB2Type.Char, 3), _
                                                          New DB2Parameter("@turn_in_merch_id", DB2Type.Integer), _
                                                          New DB2Parameter("@friendly_prod_desc", DB2Type.VarChar, 255)
                                                         }
        Dim sql As String = _spSchema + ".TU1019SP"

        parms(0).Value = ISN
        parms(1).Value = CShort(DeptID)
        parms(2).Value = UsageCode
        parms(3).Value = ImageName
        parms(4).Value = CInt(AdNum)

        If PageNum.Contains("-"c) Then
            parms(5).Value = CShort(PageNum.Substring(0, PageNum.IndexOf("-"c)).Trim)
        Else
            parms(5).Value = CShort(PageNum.Trim)
        End If

        parms(6).Value = IsReserve
        parms(7).Value = AdditionalSamplesFlag
        parms(8).Value = ModelCategoryCode
        parms(9).Value = VendorApprovalFlag
        parms(10).Value = ColorCode
        parms(11).Value = UserId

        parms(12).Direction = ParameterDirection.InputOutput
        parms(12).Value = BatchId
        parms(13).Value = ImageCategoryCode
        parms(14).Value = OnOffFigureCode
        parms(15).Direction = ParameterDirection.InputOutput
        parms(15).Value = TurnInMerchID
        parms(16).Value = FriendlyProductDesc
        ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)

        TurnInMerchID = CInt(parms(15).Value)

        Return CInt(parms(12).Value)
    End Function


    ''' <summary>
    ''' Method to get all EcommSetupClrSzInfo records.
    ''' </summary>	    	 
    Public Function GetColorSizeResults(ByVal AdNbr As Integer, ByVal PageNbr As Short, ByVal ISNs As String, ByVal ClrCodes As String) As IList(Of EcommSetupClrSzInfo)
        Dim EcommSetupClrSzInfos As New List(Of EcommSetupClrSzInfo)

        Dim sql As String = _spSchema + ".TU1024SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.VarChar), _
                                                          New DB2Parameter("@CLR_CDES", DB2Type.VarChar), _
                                                          New DB2Parameter("@AD_NUM", DB2Type.Integer), _
                                                          New DB2Parameter("@PAGE_NUM", DB2Type.SmallInt), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar)}

        parms(0).Value = ISNs
        parms(1).Value = ClrCodes
        parms(2).Value = AdNbr
        parms(3).Value = PageNbr
        parms(4).Value = _dbSchema

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new EcommSetupClrSzInfo object via factory method and add to list
                    EcommSetupClrSzInfos.Add(EcommSetupCreateFactory.ConstructColorSize(rdr))
                End While
            End Using
            Return EcommSetupClrSzInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to get all EcommSetupClrSzInfo records.
    ''' </summary>	    	 
    Public Function GetKilledItems(ByVal BatchId As String, ByVal AdNbr As String, ByVal PageNbr As String) As IList(Of EcommSetupClrSzInfo)
        Dim EcommSetupClrSzInfos As New List(Of EcommSetupClrSzInfo)

        Dim sql As String = _spSchema + ".TU1088SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@BATCH_ID", DB2Type.Integer), _
                                                  New DB2Parameter("@AD_NUM", DB2Type.Integer), _
                                                  New DB2Parameter("@PAGE_NUM", DB2Type.SmallInt)}

        parms(0).Value = IIf(BatchId = "", DBNull.Value, BatchId)
        parms(1).Value = IIf(AdNbr = "", DBNull.Value, AdNbr)
        parms(2).Value = IIf(PageNbr = "", DBNull.Value, PageNbr)

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new EcommSetupClrSzInfo object via factory method and add to list
                    EcommSetupClrSzInfos.Add(EcommSetupCreateFactory.ConstructColorSize(rdr))
                End While
            End Using
            Return EcommSetupClrSzInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to get the count of Saved ISNs among the Selected list of ISNs.
    ''' </summary>	    	 
    Public Function GetSavedISNCount(ByVal ISNs As String) As Integer
        Dim ISNCount As Integer

        Dim sql As String = _spSchema + ".TU1025SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.VarChar), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar)}

        parms(0).Value = ISNs
        parms(1).Value = _dbSchema

        Try
            ISNCount = CInt(ExecuteScalar(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms))

            Return ISNCount
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Public Function GetUPC(ByVal MerchId As Integer, ByVal SizeId As Integer) As Decimal
        Dim UPC As Decimal

        Dim sql As String = _spSchema + ".TU1041SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@SIZE_ID", DB2Type.Integer)}

        parms(0).Value = MerchId
        parms(1).Value = SizeId

        Try
            UPC = CDec(ExecuteScalar(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms))

            Return UPC
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to populate the Color, Sample Size, and Sample Store Combo boxes in ColorSize tab.
    ''' </summary>	    	 
    Public Function GetClrSizeLocLookUp(ByVal ISN As Decimal) As List(Of ClrSizLocLookUp)
        Dim EcommSetupClrSzLocValues As New List(Of ClrSizLocLookUp)

        Dim sql As String = _spSchema + ".TU1028SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal)}

        parms(0).Value = ISN

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new EcommSetupClrSzInfo object via factory method and add to list
                    EcommSetupClrSzLocValues.Add(ClrSizLocLookupFactory.Construct(rdr))
                End While
            End Using
            Return EcommSetupClrSzLocValues
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    'Public Function GetAllSampleSizes(ByVal ISNs As String) As List(Of ClrSizLocLookUp)
    '    Dim EcommSetupColorFamily As New List(Of ClrSizLocLookUp)

    '    Dim sql As String = _spSchema + ".TU1096SP"
    '    Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISNs", DB2Type.VarChar), _
    '                                                      New DB2Parameter("@SCHEMA", DB2Type.VarChar)}

    '    parms(0).Value = ISNs
    '    parms(1).Value = _dbSchema

    '    Try
    '        Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
    '            While (rdr.Read())
    '                'instantiate new EcommSetupClrSzInfo object via factory method and add to list
    '                EcommSetupColorFamily.Add(ClrSizLocLookupFactory.Construct(rdr))
    '            End While
    '        End Using
    '        Return EcommSetupColorFamily
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    '    Return Nothing
    'End Function

    Public Function GetAllColorFamily() As List(Of ClrSizLocLookUp)
        Dim EcommSetupColorFamily As New List(Of ClrSizLocLookUp)

        Dim sql As String = _spSchema + ".TU1095SP"

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, Nothing)
                While (rdr.Read())
                    'instantiate new EcommSetupClrSzInfo object via factory method and add to list
                    EcommSetupColorFamily.Add(ClrSizLocLookupFactory.Construct(rdr))
                End While
            End Using
            Return EcommSetupColorFamily
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Public Sub DeleteColorSize(ByVal MerchId As Integer, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1026SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar)}

        parms(0).Value = MerchId
        parms(1).Value = UserId

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateColorSize(ByVal ColorSizeData As EcommSetupClrSzInfo, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1029SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@ADMIN_MDSE_NUM", DB2Type.Integer), _
                                                          New DB2Parameter("@HOT_ITEM_FLG", DB2Type.Char), _
                                                          New DB2Parameter("@FRND_PRD_DESC", DB2Type.VarChar), _
                                                          New DB2Parameter("@FRND_PRD_FEAT", DB2Type.VarChar), _
                                                          New DB2Parameter("@FRND_CLR", DB2Type.VarChar), _
                                                          New DB2Parameter("@CLR_FAM", DB2Type.VarChar), _
                                                          New DB2Parameter("@SMPL_SIZ", DB2Type.VarChar), _
                                                          New DB2Parameter("@CLR_CORRECT", DB2Type.Char), _
                                                          New DB2Parameter("@IMG_KIND", DB2Type.VarChar), _
                                                          New DB2Parameter("@PU_IMG_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@RTE_FRM_AD", DB2Type.Integer), _
                                                          New DB2Parameter("@GRP_NUM", DB2Type.SmallInt), _
                                                          New DB2Parameter("@F_R_S", DB2Type.VarChar), _
                                                          New DB2Parameter("@IMG_TYPE", DB2Type.VarChar), _
                                                          New DB2Parameter("@ALT_VW", DB2Type.VarChar), _
                                                          New DB2Parameter("@SMPL_STR", DB2Type.SmallInt), _
                                                          New DB2Parameter("@MERCH_NOTES", DB2Type.VarChar), _
                                                          New DB2Parameter("@IS_RESERVE", DB2Type.Char), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar)}

        parms(0).Value = ColorSizeData.TurnInMerchID
        parms(1).Value = ColorSizeData.AdminMerchNum
        parms(2).Value = ColorSizeData.IsHotItem
        parms(3).Value = ColorSizeData.FriendlyProdDesc
        parms(4).Value = ColorSizeData.FriendlyProdFeatures
        parms(5).Value = ColorSizeData.FriendlyColor
        parms(6).Value = ColorSizeData.ColorFamily
        parms(7).Value = ColorSizeData.SampleSize
        parms(8).Value = ColorSizeData.ColorCorrect
        If ColorSizeData.ColorCorrect = CChar("") Then
            parms(8).Value = " "
        Else
            parms(8).Value = ColorSizeData.ColorCorrect
        End If
        parms(9).Value = ColorSizeData.ImageKind
        parms(10).Value = ColorSizeData.PuImageID
        parms(11).Value = ColorSizeData.RouteFromAD
        parms(12).Value = ColorSizeData.GroupNum
        parms(13).Value = ColorSizeData.FeatureRenderSwatch
        parms(14).Value = ColorSizeData.ImageType
        parms(15).Value = ColorSizeData.AltView
        parms(16).Value = CShort(ColorSizeData.SampleStore)
        parms(17).Value = ColorSizeData.MerchantNotes
        parms(18).Value = ColorSizeData.IsReserve
        parms(19).Value = UserId

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateColorFamilyFlood(ByVal MerchId As Integer, ColorFamilyList As String, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1032SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@CLR_FAM", DB2Type.VarChar), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar)}

        parms(0).Value = MerchId
        parms(1).Value = ColorFamilyList
        parms(2).Value = UserId

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    'Public Sub UpdateSampleSizeFlood(ByVal MerchId As Integer, ByVal IsReserve As Char, ByVal SampleSize As String, ByVal UserId As String)
    '    Dim sql As String = _spSchema + ".TU1033SP"
    '    Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
    '                                                      New DB2Parameter("@IS_RESERVE", DB2Type.Char), _
    '                                                      New DB2Parameter("@SAMPLE_SIZE", DB2Type.VarChar), _
    '                                                      New DB2Parameter("@USERID", DB2Type.VarChar)}

    '    parms(0).Value = MerchId
    '    parms(1).Value = IsReserve
    '    parms(2).Value = SampleSize
    '    parms(3).Value = UserId

    '    Try
    '        ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub

    Public Sub UpdateColorSizeFlood(ByVal TurnInMerchIDs As String, ByVal ColorSizeData As EcommSetupClrSzInfo, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1030SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_IDS", DB2Type.VarChar), _
                                                          New DB2Parameter("@FRND_PRD_DESC", DB2Type.VarChar), _
                                                          New DB2Parameter("@FRND_PRD_FEAT", DB2Type.VarChar),
                                                          New DB2Parameter("@FRIENDLY_COLOR", DB2Type.VarChar),
                                                          New DB2Parameter("@CLR_CORRECT", DB2Type.Char, 1), _
                                                          New DB2Parameter("@IMG_KIND", DB2Type.VarChar),
                                                          New DB2Parameter("@GRP_NUM", DB2Type.VarChar),
                                                          New DB2Parameter("@IMG_TYPE", DB2Type.VarChar), _
                                                          New DB2Parameter("@ALT_VW", DB2Type.VarChar), _
                                                          New DB2Parameter("@SMPL_STR", DB2Type.SmallInt), _
                                                          New DB2Parameter("@MERCH_NOTES", DB2Type.VarChar), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar)}

        parms(0).Value = TurnInMerchIDs
        parms(1).Value = ColorSizeData.FriendlyProdDesc
        parms(2).Value = ColorSizeData.FriendlyProdFeatures
        parms(3).Value = ColorSizeData.FriendlyColor

        If ColorSizeData.ColorCorrect = CChar("") Then
            parms(4).Value = DBNull.Value
        Else
            parms(4).Value = ColorSizeData.ColorCorrect
        End If

        parms(5).Value = ColorSizeData.ImageKind
        parms(6).Value = ColorSizeData.GroupNum.ToString
        parms(7).Value = ColorSizeData.ImageType
        parms(8).Value = ColorSizeData.AltView
        parms(9).Value = If(String.IsNullOrEmpty(ColorSizeData.SampleStore), 0, CShort(ColorSizeData.SampleStore))
        parms(10).Value = ColorSizeData.MerchantNotes
        parms(11).Value = UserId
        parms(12).Value = _dbSchema

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateImageTypeFlood(ByVal intTurnInMerchID As Integer, ByVal strImageType As String, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1097SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@IMG_TYPE", DB2Type.VarChar), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar)}

        parms(0).Value = intTurnInMerchID
        parms(1).Value = strImageType
        parms(2).Value = UserId

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdatePrintLblFlg(ByVal TurnInMerchIDs As String, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1045SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_IDS", DB2Type.VarChar),
                                                          New DB2Parameter("@USERID", DB2Type.VarChar), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar)}

        parms(0).Value = TurnInMerchIDs
        parms(1).Value = UserId
        parms(2).Value = _dbSchema

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub SubmitColorSizeData(ByVal BatchId As Integer, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1034SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@BatchId", DB2Type.VarChar),
                                                          New DB2Parameter("@USERID", DB2Type.VarChar),
                                                          New DB2Parameter("@TURNIN_MDSE_ID", DB2Type.Integer)}

        parms(0).Value = BatchId
        parms(1).Value = UserId
        parms(2).Value = 0

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub SubmitColorSizeDataforTIA(ByVal BatchId As Integer, ByVal UserId As String, ByVal TurnInMerchandiseID As Integer)
        Dim sql As String = _spSchema + ".TU1034SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@BatchId", DB2Type.VarChar),
                                                          New DB2Parameter("@USERID", DB2Type.VarChar),
                                                          New DB2Parameter("@TURNIN_MDSE_ID", DB2Type.Integer)}

        parms(0).Value = BatchId
        parms(1).Value = UserId
        parms(2).Value = TurnInMerchandiseID

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function GeneratePrintBatchID(ByVal TurnInMerchIDs As String, ByVal UserId As String) As Integer
        Dim sql As String = _spSchema + ".TU1036SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_IDS", DB2Type.VarChar),
                                                          New DB2Parameter("@USERID", DB2Type.VarChar), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar)}

        parms(0).Value = TurnInMerchIDs
        parms(1).Value = UserId
        parms(2).Value = _dbSchema

        Try
            GeneratePrintBatchID = CInt(ExecuteScalar(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms))
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function AddToBatch(ByVal TurnInMerchId As Integer, ByVal BatchId As Integer, ByVal AdNbr As Integer, ByVal PageNbr As Short, ByVal UserId As String) As Integer
        Dim sql As String = _spSchema + ".TU1089SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer),
                                                          New DB2Parameter("@BATCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@AD_NUM", DB2Type.Integer), _
                                                          New DB2Parameter("@PAGE_NUM", DB2Type.SmallInt), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar)}

        parms(0).Value = TurnInMerchId
        parms(1).Direction = ParameterDirection.InputOutput
        parms(1).Value = BatchId
        parms(2).Value = AdNbr
        parms(3).Value = PageNbr
        parms(4).Value = UserId

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            Return CInt(parms(1).Value)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetSampleRequestsForIsn(ByVal Isn As Decimal) As IList(Of SampleRequestInfo)
        Dim SampleRequests As IList(Of SampleRequestInfo) = New List(Of SampleRequestInfo)()

        Dim sql As String = _spSchema + ".TU1120SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal)}

        parms(0).Value = Isn

        'Execute a query to read the SampleRequestInfo
        Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            While (rdr.Read())
                'instantiate new SampleRequestInfo object via factory method and add to list
                SampleRequests.Add(EcommSetupCreateFactory.ConstructPartialSampleRequests(rdr))
            End While
        End Using
        Return SampleRequests
    End Function

    Public Function GetSampleRequestsForIsnAndColor(ByVal Isn As Decimal, ByVal color_id As Integer) As IList(Of SampleRequestInfo)
        Dim SampleRequests As IList(Of SampleRequestInfo) = New List(Of SampleRequestInfo)()

        Dim sql As String = _spSchema + ".TU1121SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal), _
                                                          New DB2Parameter("@CLR_CDE", DB2Type.Integer)}

        parms(0).Value = Isn
        parms(1).Value = color_id

        'Execute a query to read the SampleRequestInfo
        Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            While (rdr.Read())
                'instantiate new SampleRequestInfo object via factory method and add to list
                SampleRequests.Add(EcommSetupCreateFactory.ConstructPartialSampleRequests(rdr))
            End While
        End Using
        Return SampleRequests
    End Function

    Public Function GetSampleRequests(ByVal sampleMerchId As Integer, ByVal internalStyleNumber As Decimal, ByVal vendorStyleNumber As String) As IList(Of SampleRequestInfo)

        Dim sampleRequestInfo As New List(Of SampleRequestInfo)

        Dim sql As String = _spSchema + ".TU1122SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@ISN", DB2Type.Decimal), _
                                                          New DB2Parameter("@VENDOR_STYLE", DB2Type.VarChar), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar)}

        parms(0).Value = sampleMerchId
        parms(1).Value = internalStyleNumber
        parms(2).Value = vendorStyleNumber
        parms(3).value = _dbSchema

        Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            While (rdr.Read())
                'instantiate new SampleRequestInfo object via factory method and add to list
                sampleRequestInfo.Add(EcommSetupCreateFactory.ConstructSampleRequests(rdr))
            End While
        End Using

        Return sampleRequestInfo
    End Function

    Public Sub UpdateAdminMdseNumber(ByVal turnInMerchId As Integer, ByVal sampleMerchId As Integer, ByVal sampleSize As Integer, ByVal userId As String)

        Dim sql As String = _spSchema + ".TU1123SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_ID", DB2Type.Integer), _
                                                          New DB2Parameter("@ADMIN_MDSE_NUM", DB2Type.Integer), _
                                                          New DB2Parameter("@SAMPLE_SIZE", DB2Type.Integer), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar)}

        parms(0).Value = turnInMerchId
        parms(1).Value = sampleMerchId
        parms(2).Value = sampleSize
        parms(3).Value = UserId

        ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)

    End Sub

    ''' <summary>
    ''' Method to get all isns match the search criteria defined
    ''' </summary>
    Public Function GetAllEcommSetupCreateResultsByPOShipDate(ByVal AdNum As Integer, ByVal PageNum As Integer, ByVal DMMID As Integer, ByVal BuyerID As Integer, ByVal DeptId As Int16, ByVal ClassId As Int16, _
                                                             ByVal VendorId As Integer, ByVal VendorStyleNum As String, ByVal StartShipDate As Date, ByVal IncludeOnlyApprovedItems As Boolean) As IList(Of EcommSetupCreateInfo)
        Dim EcommSetupCreateInfos As New List(Of EcommSetupCreateInfo)

        Dim sql As String = _spSchema + ".TU1144SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@DMMID", DB2Type.Integer), _
                                                          New DB2Parameter("@BuyerID", DB2Type.Integer), _
                                                          New DB2Parameter("@DeptID", DB2Type.SmallInt), _
                                                          New DB2Parameter("@ClassID", DB2Type.SmallInt), _
                                                          New DB2Parameter("@VendorID", DB2Type.Integer), _
                                                          New DB2Parameter("@VendorStyleNum", DB2Type.VarChar), _
                                                          New DB2Parameter("@POStartShipDate", DB2Type.Date), _
                                                          New DB2Parameter("@ad_num", DB2Type.Integer), _
                                                          New DB2Parameter("@ad_system_page_num", DB2Type.SmallInt), _
                                                          New DB2Parameter("@IncludeOnlyApprovedItems", DB2Type.SmallInt), _
                                                          New DB2Parameter("@IncludeOnlyWebEligibleItems", DB2Type.SmallInt),
                                                          New DB2Parameter("@ISN", DB2Type.Decimal),
                                                          New DB2Parameter("@Schema", DB2Type.VarChar),
                                                          New DB2Parameter("@IsTIA", DB2Type.Integer)}

        parms(0).Value = DMMID
        parms(1).Value = BuyerID
        parms(2).Value = DeptId
        parms(3).Value = ClassId
        parms(4).Value = VendorId
        parms(5).Value = VendorStyleNum
        If Not IsDate(StartShipDate) Then
            parms(6).Value = DBNull.Value
        Else
            parms(6).Value = StartShipDate
        End If
        parms(7).Value = AdNum
        parms(8).Value = PageNum
        parms(9).Value = IIf(IncludeOnlyApprovedItems, 1, 0)
        parms(10).Value = 0
        parms(11).Value = 0
        parms(12).Value = _dbSchema
        parms(13).Value = 0

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new EcommSetupCreateInfo object via factory method and add to list
                    EcommSetupCreateInfos.Add(EcommSetupCreateFactory.Construct(rdr))
                End While
            End Using
            Return EcommSetupCreateInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
    ''' <summary>
    ''' Gets the default ship days for the parameters passed.  
    ''' </summary>
    ''' <param name="DMMID"></param>
    ''' <param name="BuyerID"></param>
    ''' <param name="DepartmentId"></param>
    ''' <param name="VendorStyleNumber"></param>
    ''' <returns>Returns the default ship days defined for the CFG in the TMS900Parameter table</returns>
    ''' <remarks>
    ''' If there are multiple vendor style numbers passed, then department will be ignored.  Otherwise nothing will be returned if the vendor styles are from two different deparments.
    ''' </remarks>
    Public Function GetCFGDefaultShipDays(ByVal DMMID As Integer, ByVal BuyerID As Integer, ByVal DepartmentId As Int16, ByVal VendorStyleNumber As String) As Integer
        Dim sql As String = _spSchema + ".TU1145SP"
        Dim defaultShipDays As Integer = 0
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@DMMID", DB2Type.Integer), _
                                                          New DB2Parameter("@BuyerID", DB2Type.Integer), _
                                                          New DB2Parameter("@DepartmentID", DB2Type.Integer), _
                                                          New DB2Parameter("@VendorStyleNumber", DB2Type.VarChar)}

        If DMMID > 0 Then
            parms(0).Value = DMMID
        Else
            parms(0).Value = DBNull.Value
        End If
        If BuyerID > 0 Then
            parms(1).Value = BuyerID
        Else
            parms(1).Value = DBNull.Value
        End If

        If DepartmentId > 0 Then
            parms(2).Value = DepartmentId
        Else
            parms(2).Value = DBNull.Value
        End If

        parms(3).Value = VendorStyleNumber

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    defaultShipDays = CInt(rdr("START_SHIP_DAYS"))
                End While
            End Using

        Catch ex As Exception
            Throw ex
        End Try
        Return defaultShipDays
    End Function
    ''' <summary>
    ''' Calls the stored procedure to get color level results for the ISN passed
    ''' </summary>
    ''' <param name="ISN">ISN for which color level results to be retrieved</param>
    ''' <param name="StartShipDate">Start ship date to be matched with PO ship date</param>
    ''' <param name="IncludeOnlyApprovedItems">Indicator to decide the items (Approved, not approved or both) to be retrieved</param>
    ''' <returns>Returns the color level results for the ISN passed</returns>
    ''' <remarks></remarks>
    Function GetApprovedEcommSetupCreateDetail(ByVal ISN As Decimal, ByVal StartShipDate As Date, ByVal IncludeOnlyApprovedItems As Boolean) As IList(Of EcommSetupCreateInfo)
        Dim EcommSetupCreateInfos As New List(Of EcommSetupCreateInfo)
        
        Dim sql As String = _spSchema + ".TU1146SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal), New DB2Parameter("@POStartShipDate", DB2Type.Date), New DB2Parameter("@IncludeOnlyApprovedItems", DB2Type.SmallInt)}

        parms(0).Value = ISN
        If Not IsDate(StartShipDate) Then
            parms(1).Value = DBNull.Value
        Else
            parms(1).Value = StartShipDate
        End If
        parms(2).Value = IIf(IncludeOnlyApprovedItems, 1, 0)

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new EcommSetupCreateInfo object via factory method and add to list
                    EcommSetupCreateInfos.Add(EcommSetupCreateFactory.Construct(rdr))
                End While
            End Using
            Return EcommSetupCreateInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
End Class
