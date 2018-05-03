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

Partial Public Class EcommFabOrigDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")

    ''' <summary>
    ''' Method to get all EcommFabOrigInfo records.
    ''' </summary>
    Public Function GetAllFabOrigResultsByHierarchy(ByVal StatusCodes As String, ByVal DeptId As Int16, ByVal ClassId As Int16, ByVal SubClassId As Int16, _
                                                             ByVal VendorId As Integer, ByVal VendorStyleNum As String, ByVal ACode1 As String, ByVal ACode2 As String, ByVal ACode3 As String, ByVal ACode4 As String, _
                                                             ByVal SellYear As Int16, ByVal SeasonId As Integer, ByVal CreatedSince As Date?) As IList(Of EcommFabOrigInfo)
        Dim EcommFabOrigInfos As New List(Of EcommFabOrigInfo)
        Dim sql As String = _spSchema + ".TU1166SP"
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
                                                          New DB2Parameter("@CreatedSince", DB2Type.Date)}

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

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new EcommFabOrigInfo object via factory method and add to list
                    EcommFabOrigInfos.Add(EcommFabOrigFactory.Construct(rdr))
                End While
            End Using
            Return EcommFabOrigInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to get all EcommFabOrigInfo records.
    ''' </summary>	    	 
    Public Function GetAllEcommFabOrigResultsByISNs(ByVal ISNs As String, ByVal Upc As String, ByVal ReserveISNs As String) As IList(Of EcommFabOrigInfo)
        Dim EcommFabOrigInfos As New List(Of EcommFabOrigInfo)

        Dim sql As String = _spSchema + ".TU1167SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISNs", DB2Type.VarChar, 8000), _
                                                          New DB2Parameter("@UPC", DB2Type.VarChar, 8000), _
                                                          New DB2Parameter("@RESERVE_ISNs", DB2Type.VarChar, 8000), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar, 20)}

        parms(0).Value = ISNs
        parms(1).Value = Upc
        parms(2).Value = ReserveISNs
        parms(3).Value = _dbSchema

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new EcommFabOrigInfo object via factory method and add to list
                    EcommFabOrigInfos.Add(EcommFabOrigFactory.Construct(rdr))
                End While
            End Using
            Return EcommFabOrigInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Public Sub UpdateFabOrigByISN(ByVal ISN As Decimal, LabelId As Integer, Fabrication As String, ByVal Origination As String, FabSrce As String, OrigSrce As String, UserID As String)

        Dim sql As String = _spSchema + ".TU1174SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal), _
                                                          New DB2Parameter("@label_id", DB2Type.Integer), _
                                                          New DB2Parameter("@reserved_isn_flg", DB2Type.VarChar), _
                                                          New DB2Parameter("@isn_size_catgy_cde", DB2Type.VarChar), _
                                                          New DB2Parameter("@xmlCategories", DB2Type.VarChar), _
                                                          New DB2Parameter("@last_mod_id", DB2Type.VarChar), _
                                                          New DB2Parameter("@TURNIN_FABRIC_DESC", DB2Type.VarChar), _
                                                          New DB2Parameter("@PRDCT_ORIG_DESC", DB2Type.VarChar), _
                                                          New DB2Parameter("@PRDCT_ORIGSRCE_TYP", DB2Type.VarChar), _
                                                          New DB2Parameter("@FAB_DESC_SRCE_TYP", DB2Type.VarChar)}

        parms(0).Value = ISN
        parms(1).Value = LabelId
        parms(2).Value = String.Empty
        parms(3).Value = String.Empty
        parms(4).Value = String.Empty
        parms(5).Value = UserID
        parms(6).Value = Fabrication
        parms(7).Value = Origination
        parms(8).Value = OrigSrce
        parms(9).Value = FabSrce

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

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
    ''' Method to get all isns match the search criteria defined
    ''' </summary>
    Public Function GetAllEcommFabOrigResultsByPOShipDate(ByVal DMMID As Integer, ByVal BuyerID As Integer, ByVal DeptId As Int16, ByVal ClassId As Int16, _
                                                             ByVal VendorId As Integer, ByVal VendorStyleNum As String, ByVal StartShipDate As Date, ByVal IncludeOnlyApprovedItems As Boolean) As IList(Of EcommFabOrigInfo)
        Dim EcommFabOrigInfos As New List(Of EcommFabOrigInfo)

        Dim sql As String = _spSchema + ".TU1165SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@DMMID", DB2Type.Integer), _
                                                          New DB2Parameter("@BuyerID", DB2Type.Integer), _
                                                          New DB2Parameter("@DeptID", DB2Type.SmallInt), _
                                                          New DB2Parameter("@ClassID", DB2Type.SmallInt), _
                                                          New DB2Parameter("@VendorID", DB2Type.Integer), _
                                                          New DB2Parameter("@VendorStyleNum", DB2Type.VarChar), _
                                                          New DB2Parameter("@POStartShipDate", DB2Type.Date), _
                                                          New DB2Parameter("@IncludeOnlyApprovedItems", DB2Type.SmallInt)}

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
        parms(7).Value = IIf(IncludeOnlyApprovedItems, 1, 0)
        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new EcommFabOrigInfo object via factory method and add to list
                    EcommFabOrigInfos.Add(EcommFabOrigFactory.Construct(rdr))
                End While
            End Using
            Return EcommFabOrigInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

End Class
