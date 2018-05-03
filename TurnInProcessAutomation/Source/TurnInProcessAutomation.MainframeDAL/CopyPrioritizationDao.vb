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

Partial Public Class CopyPrioritizationDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")

    ''' <summary>
    ''' Method to get all CopyPrioritizationInfo records.
    ''' </summary>
    Public Function GetCopyPrioritizationResultsByCriteria(ByVal CategoryCode As Integer,
                                                           ByVal PriceStausCodes As String, ByVal ImageReady As String,
                                                       ByVal CopyReady As String, ByVal SKUUseFilters As String,
                                                        ByVal ThirdPartyFulfilment As Integer, ByVal LocationID As Integer) As IList(Of CopyPrioritizationInfo)
        Dim CopyPrioritizationInfos As New List(Of CopyPrioritizationInfo)
        Dim sql As String = _spSchema + ".TU1182SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@CategoryCode", DB2Type.Integer),
                                                          New DB2Parameter("@StatusCodes", DB2Type.VarChar),
                                                          New DB2Parameter("@ImageReady", DB2Type.VarChar),
                                                          New DB2Parameter("@CopyReady", DB2Type.VarChar),
                                                          New DB2Parameter("@SKUUseCodes", DB2Type.VarChar),
                                                          New DB2Parameter("@ThirdPartyFulfilment", DB2Type.Integer),
                                                          New DB2Parameter("@LocationID", DB2Type.Integer)}

        parms(0).Value = CategoryCode
        parms(1).Value = PriceStausCodes
        parms(2).Value = ImageReady
        parms(3).Value = CopyReady
        parms(4).Value = SKUUseFilters
        parms(5).Value = ThirdPartyFulfilment
        parms(6).Value = LocationID

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new EcommFabOrigInfo object via factory method and add to list
                    CopyPrioritizationInfos.Add(CopyPrioritizationFactory.Construct(rdr))
                End While
            End Using
            Return CopyPrioritizationInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to get one CopyPrioritizationInfo record.
    ''' </summary>
    Public Function GetCopyPrioritizationResults(ByVal ImageId As Integer) As List(Of CopyPrioritizationInfo)
        Dim CopyPrioritizationInfos As New List(Of CopyPrioritizationInfo)
        Dim sql As String = _spSchema + ".TU1181SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ImageID", DB2Type.Integer)}

        parms(0).Value = ImageId

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new EcommFabOrigInfo object via factory method and add to list
                    CopyPrioritizationInfos.Add(CopyPrioritizationFactory.Construct(rdr))
                End While
            End Using
            Return CopyPrioritizationInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to get one CopyPrioritizationInfo record.
    ''' </summary>
    Public Function GetCopyPrioritizationResultsByDept(ByVal DeptId As Integer, ByVal VendorStyleNumber As String, ByVal StartShipDate? As Date,
                                                       ByVal PriceStausCodes As String, ByVal AdNumber As Integer,
                                                       ByVal ImageReady As String,
                                                       ByVal CopyReady As String,
                                                       ByVal SKUUseFilters As String,
                                                       ByVal ThirdPartyFulfilment As Integer, ByVal LocationID As Integer) As List(Of CopyPrioritizationInfo)
        Dim CopyPrioritizationInfos As New List(Of CopyPrioritizationInfo)
        Dim sql As String = _spSchema + ".TU1189SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@DeptID", DB2Type.Integer),
                                                          New DB2Parameter("@VendorStyleNum", DB2Type.VarChar),
                                                          New DB2Parameter("@POStartShipDate", DB2Type.Date),
                                                          New DB2Parameter("@StatusCodes", DB2Type.VarChar),
                                                          New DB2Parameter("@AdNumber", DB2Type.Integer),
                                                          New DB2Parameter("@ImageReady", DB2Type.VarChar),
                                                          New DB2Parameter("@CopyReady", DB2Type.VarChar),
                                                          New DB2Parameter("@SKUUseCodes", DB2Type.VarChar),
                                                          New DB2Parameter("@ThirdPartyFulfilment", DB2Type.Integer),
                                                          New DB2Parameter("@LocationID", DB2Type.Integer),
                                                          New DB2Parameter("@Schema", DB2Type.VarChar)}

        parms(0).Value = DeptId
        parms(1).Value = VendorStyleNumber
        parms(2).Value = StartShipDate
        parms(3).Value = PriceStausCodes
        parms(4).Value = AdNumber
        parms(5).Value = ImageReady
        parms(6).Value = CopyReady
        parms(7).Value = SKUUseFilters
        parms(8).Value = ThirdPartyFulfilment
        parms(9).Value = LocationID
        parms(10).Value = _dbSchema

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new EcommFabOrigInfo object via factory method and add to list
                    CopyPrioritizationInfos.Add(CopyPrioritizationFactory.Construct(rdr))
                End While
            End Using
            Return CopyPrioritizationInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
    Public Function GetCopyPrioritizationResultsByItem(ByVal ISNs As String, ByVal SKUsUPCs As String, ByVal ImageIDs As String) As List(Of CopyPrioritizationInfo)
        Dim CopyPrioritizationInfos As New List(Of CopyPrioritizationInfo)
        Dim sql As String = _spSchema + ".TU1190SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISNs", DB2Type.VarChar),
                                                          New DB2Parameter("@SKUsUPCs", DB2Type.VarChar),
                                                          New DB2Parameter("@ImageIDs", DB2Type.VarChar),
                                                          New DB2Parameter("@Schema", DB2Type.VarChar)}

        parms(0).Value = ISNs
        parms(1).Value = SKUsUPCs
        parms(2).Value = ImageIDs
        parms(3).Value = _dbSchema

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new EcommFabOrigInfo object via factory method and add to list
                    CopyPrioritizationInfos.Add(CopyPrioritizationFactory.Construct(rdr))
                End While
            End Using
            Return CopyPrioritizationInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
    ''' <summary>
    ''' Method to get one CopyPrioritizationInfo record.
    ''' </summary>
    Public Function GetCopyPrioritizationResult(ByVal ImageId As Integer, ByVal CategoryCode As Integer) As CopyPrioritizationInfo
        Dim CopyPrioritizationResult As New CopyPrioritizationInfo
        Dim sql As String = _spSchema + ".TU1181SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ImageID", DB2Type.Integer), New DB2Parameter("@CategoryCode", DB2Type.Integer)}

        parms(0).Value = ImageId
        parms(1).Value = CategoryCode

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    CopyPrioritizationResult = CopyPrioritizationFactory.Construct(rdr)
                End While
            End Using
            CopyPrioritizationResult.AvailableColors = GetColorLevelResults(ImageId)
            Return CopyPrioritizationResult
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

    Function GetUPCSKUExists(ByVal UPC As Long) As Boolean
        Dim sql As String = _spSchema + ".TU1172SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@UPC", DB2Type.Decimal), _
                                                          New DB2Parameter("@ISVALID", DB2Type.Integer) _
                                                         }

        parms(0).Value = UPC
        parms(1).Direction = ParameterDirection.ReturnValue

        ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Return CBool(parms(1).Value)
    End Function

    ''' <summary>
    ''' Method to get all the Available Color records for Copy Prioritization edit page.
    ''' </summary>	    	 
    Public Function GetColorLevelResults(ByVal ImageID As Integer) As List(Of CopyPrioritizationColorInfo)
        Dim EcommColorLevelInfos As New List(Of CopyPrioritizationColorInfo)

        Dim sql As String = _spSchema + ".TU1184SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ImageID", DB2Type.Integer)}

        parms(0).Value = ImageID

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    EcommColorLevelInfos.Add(CopyPrioritizationFactory.ConstructAvailableColors(rdr))
                End While
            End Using
            Return EcommColorLevelInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to get all the Web Categories
    ''' </summary>	    	 
    Public Function GetProductCategories(ByVal ParentCode As Integer) As List(Of ProductCategoryInfo)
        Dim WebCategories As New List(Of ProductCategoryInfo)

        'Dim sql As String = _spSchema + ".TU1183SP"
        Dim sql As String = _spSchema + ".TU1186SP"

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ParentCode", DB2Type.Integer)}

        parms(0).Value = ParentCode

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    WebCategories.Add(ProductCategoryFactory.Construct(rdr))
                End While
            End Using
            Return WebCategories
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Public Sub SaveCopy(ByVal ProductCode As Integer, ByVal Copy As String, ByVal ProductName As String, ByVal UserID As String,
                        ByVal IsSetToReady As Boolean, ByVal WebCatComments As String, ByVal ProductDesc As String)
        'Dim sql As String = _spSchema + ".TU1188SP"
        Dim sql As String = _spSchema + ".TU1188SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ProductCode", DB2Type.Integer),
                                                          New DB2Parameter("@Copy", DB2Type.VarChar),
                                                          New DB2Parameter("@ProductName", DB2Type.VarChar),
                                                          New DB2Parameter("@UserID", DB2Type.VarChar),
                                                          New DB2Parameter("@IsProductReady", DB2Type.Integer),
                                                          New DB2Parameter("@Comments", DB2Type.VarChar),
                                                          New DB2Parameter("@ProductDesc", DB2Type.VarChar)}

        parms(0).Value = ProductCode
        parms(1).Value = Copy
        parms(2).Value = ProductName
        parms(3).Value = UserID
        parms(4).Value = If(IsSetToReady, 1, 0)
        parms(5).Value = WebCatComments
        parms(6).Value = ProductDesc

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class
