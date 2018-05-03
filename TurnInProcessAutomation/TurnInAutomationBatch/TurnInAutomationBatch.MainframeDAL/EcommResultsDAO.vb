Imports BonTon.DBUtility
Imports BonTon.DBUtility.DB2Helper
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory
Imports System.Configuration


Public Class EcommResultsDAO
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")

    Public Function GetApprovedItemsWithPO(ByVal StartShipDate As Date, ByVal DepartmentID As Integer, ByVal IncludeOnlyWebEligibleItems As Boolean, _
                                           Optional ByVal InternalStyleNumber As Decimal = 0, Optional ByVal IncludeOnlyApprovedItems As Boolean = True,
                                           Optional ByVal IsTurnInAutomation As Boolean = False) As IList(Of EcommSetupCreateInfo)
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

        parms(0).Value = 0
        parms(1).Value = 0
        parms(2).Value = DepartmentID
        parms(3).Value = 0
        parms(4).Value = 0
        parms(5).Value = String.Empty
        parms(6).Value = StartShipDate
        parms(7).Value = 0
        parms(8).Value = 0
        parms(9).Value = IIf(IncludeOnlyApprovedItems, 1, 0)
        parms(10).Value = IIf(IncludeOnlyWebEligibleItems, 1, 0)
        parms(11).Value = InternalStyleNumber
        parms(12).Value = _dbSchema
        parms(13).Value = 1

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
    ''' Calls the stored procedure to get color level results for the ISN passed
    ''' </summary>
    ''' <param name="InternalStyleNumber">ISN for which color level results to be retrieved</param>
    ''' <param name="StartShipDate">Start ship date to be matched with PO ship date</param>
    ''' <param name="IncludeOnlyWebEligibleItems">Indicator for web eligible items</param>
    ''' <param name="IsVendorImage">Vendor image indicator</param>
    ''' <returns>Returns the color level results for the ISN passed</returns>
    ''' <remarks></remarks>
    Function GetApprovedColorDetailsByISN(ByVal InternalStyleNumber As Decimal, ByVal StartShipDate As Date, ByVal IncludeOnlyWebEligibleItems As Boolean, Optional ByVal IsVendorImage As Boolean = False) As IList(Of EcommSetupClrSzInfo)
        Dim EcommSetupCreateInfos As New List(Of EcommSetupClrSzInfo)
        Dim EcommSetupClrSzInfo As EcommSetupClrSzInfo = Nothing

        Dim sql As String = _spSchema + ".TU1148SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal), New DB2Parameter("@POStartShipDate", DB2Type.Date), _
                                                          New DB2Parameter("@IncludeOnlyWebEligibleItems", DB2Type.SmallInt), _
                                                          New DB2Parameter("@IsVendorImage", DB2Type.Integer)}

        parms(0).Value = InternalStyleNumber
        If Not IsDate(StartShipDate) Then
            parms(1).Value = DBNull.Value
        Else
            parms(1).Value = StartShipDate
        End If
        parms(2).Value = IIf(IncludeOnlyWebEligibleItems, 1, 0)
        parms(3).Value = If(IsVendorImage, 1, 0)

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    EcommSetupClrSzInfo = EcommSetupCreateFactory.ConstructColorSize(rdr)
                    If Not EcommSetupClrSzInfo Is Nothing AndAlso Not EcommSetupCreateInfos.Exists(Function(a) a.VendorColorCode = EcommSetupClrSzInfo.VendorColorCode) Then
                        'instantiate new EcommSetupCreateInfo object via factory method and add to list
                        EcommSetupCreateInfos.Add(EcommSetupClrSzInfo)
                    End If
                End While
            End Using
            Return EcommSetupCreateInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
    ''' <summary>
    ''' Gets the maximum used web category for the department, vendor, generic class, generic sub class and sub class
    ''' </summary>
    ''' <param name="DeptID"></param>
    ''' <param name="VendorID"></param>
    ''' <param name="GenericClassID"></param>
    ''' <param name="GenericSubClassID"></param>
    ''' <param name="ClassID"></param>
    ''' <param name="SubClassID"></param>
    ''' <returns>Returns the maximum used web category</returns>
    ''' <remarks></remarks>
    Public Function GetWebCategory(ByVal DeptID As Integer, ByVal VendorID As Integer, ByVal GenericClassID As Integer, ByVal GenericSubClassID As Integer,
                                   ByVal ClassID As Integer, ByVal SubClassID As Integer, Optional ByVal FromWebCatHistory As Boolean = False) As WebCat
        Dim webCategory As WebCat = Nothing
        Dim sql As String = String.Empty
        Dim parms As DB2Parameter() = Nothing

        Try
            sql = String.Concat(_spSchema, ".", If(FromWebCatHistory, "TU1177SP", "TU1156SP"))

            parms = New DB2Parameter() {New DB2Parameter("@DEPT_ID", DB2Type.Integer), New DB2Parameter("@VENDOR_ID", DB2Type.Integer),
                                        New DB2Parameter("@GEN_CLA_ID", DB2Type.Integer), New DB2Parameter("@GEN_SCLA_ID", DB2Type.Integer),
                                        New DB2Parameter("@CLASS_ID", DB2Type.Integer), New DB2Parameter("@SUBCLASS_ID", DB2Type.Integer)}

            parms(0).Value = DeptID
            parms(1).Value = VendorID
            parms(2).Value = GenericClassID
            parms(3).Value = GenericSubClassID
            parms(4).Value = ClassID
            parms(5).Value = SubClassID

            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                If rdr.Read() Then
                    webCategory = WebCatFactory.Construct(rdr)
                End If
            End Using

        Catch ex As Exception
            Throw ex
        End Try
        Return webCategory
    End Function
    ''' <summary>
    ''' Calls the stored procedure to get color level results for the Vendor passed
    ''' </summary>
    ''' <param name="VendorStyle">Vendor style for which color level results to be retrieved</param>
    ''' <param name="StartShipDate">Start ship date to be matched with PO ship date</param>
    ''' <returns>Returns the color level results for the vendor style passed</returns>
    ''' <remarks></remarks>
    Function GetApprovedColorDetailsByVendorStyle(ByVal VendorStyle As String, ByVal StartShipDate As Date, ByVal IncludeOnlyWebEligibleItems As Boolean,
                                                  ByVal IsVendorImage As Boolean, ByVal DeptID As Integer) As IList(Of EcommSetupClrSzInfo)
        Dim EcommSetupCreateInfos As New List(Of EcommSetupClrSzInfo)
        Dim EcommSetupColorSizeInfo As EcommSetupClrSzInfo = Nothing

        Dim sql As String = _spSchema + ".TU1149SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@VENDOR_STYLE_NUM", DB2Type.Char, 20), New DB2Parameter("@POStartShipDate", DB2Type.Date), _
                                                          New DB2Parameter("@IncludeOnlyWebEligibleItems", DB2Type.SmallInt),
                                                          New DB2Parameter("@IsVendorImage", DB2Type.Integer),
                                                          New DB2Parameter("@DeptID", DB2Type.Integer)}

        parms(0).Value = VendorStyle
        If Not IsDate(StartShipDate) Then
            parms(1).Value = DBNull.Value
        Else
            parms(1).Value = StartShipDate
        End If
        parms(2).Value = IIf(IncludeOnlyWebEligibleItems, 1, 0)
        parms(3).Value = IIf(IsVendorImage, 1, 0)
        parms(4).Value = DeptID

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new EcommSetupCreateInfo object via factory method and add to list
                    EcommSetupColorSizeInfo = EcommSetupCreateFactory.ConstructColorSize(rdr)
                    If Not EcommSetupColorSizeInfo Is Nothing AndAlso Not EcommSetupCreateInfos.Exists(Function(a) a.ISN = EcommSetupColorSizeInfo.ISN And a.VendorColorCode = EcommSetupColorSizeInfo.VendorColorCode) Then
                        EcommSetupCreateInfos.Add(EcommSetupCreateFactory.ConstructColorSize(rdr))
                    End If
                End While
            End Using
            Return EcommSetupCreateInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Function GetColorsFromSSKUByDeptVendorStyle(ByVal DeptID As Integer, ByVal VendorStyle As String) As IList(Of SampleRequestInfo)
        Dim styleSKUColors As New List(Of SampleRequestInfo)

        Dim sql As String = _spSchema + ".TU1192SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@DeptID", DB2Type.Integer),
                                                            New DB2Parameter("@VENDOR_STYLE_NUM", DB2Type.Char, 20)}

        parms(0).Value = DeptID
        parms(1).Value = VendorStyle

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new EcommSetupCreateInfo object via factory method and add to list
                    styleSKUColors.Add(EcommSetupCreateFactory.ConstructPartialSampleRequests(rdr))
                End While
            End Using
        Catch ex As Exception
            Throw
        End Try

        Return styleSKUColors
    End Function
End Class
