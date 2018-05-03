Imports BonTon.DBUtility
Imports BonTon.DBUtility.DB2Helper
Imports IBM.Data.DB2
Imports System.Configuration
Imports TurnInProcessAutomation.Factory
Imports TurnInProcessAutomation.BusinessEntities

Public Class ColorDAO
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")
    ''' <summary>
    ''' Gets the frequently used friendly color for the Dept, Vendor and Color code.
    ''' It looks up the history in web cat if the parameter FromWebCatHistory is true, 
    ''' otherwise it looks up the history in Turn in.
    ''' </summary>
    ''' <param name="DeptID"></param>
    ''' <param name="VendorID"></param>
    ''' <param name="ColorCode"></param>
    ''' <param name="FromWebCatHistory"></param>
    ''' <returns>Frequently used friendly color</returns>
    ''' <remarks></remarks>
    Public Function GetFrequentlyUsedFriendlyColor(ByVal DeptID As Integer, ByVal VendorID As Integer, ByVal ColorCode As Integer, Optional ByVal FromWebCatHistory As Boolean = False) As String
        Dim friendlyColorName As String = String.Empty
        Dim sql As String = String.Empty

        sql = String.Concat(_spSchema, ".", If(FromWebCatHistory, "TU1179SP", "TU1173SP"))

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@DEPT_ID", DB2Type.Integer), _
                                                  New DB2Parameter("@VENDOR_ID", DB2Type.Integer), _
                                                  New DB2Parameter("@CLR_CDE", DB2Type.Integer)}


        parms(0).Value = DeptID
        parms(1).Value = VendorID
        parms(2).Value = ColorCode

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                If (rdr.HasRows) Then
                    rdr.Read()
                    If Common.HasColumn(rdr, "FRIENDLY_COLOR_NME") Then
                        friendlyColorName = CStr(rdr("FRIENDLY_COLOR_NME"))
                    End If
                End If
            End Using
        Catch ex As Exception
            Throw ex
        End Try

        Return friendlyColorName
    End Function
    ''' <summary>
    ''' Gets the frequently used color family for the Dept, Vendor and Color code.
    ''' It looks up the history in web cat if the parameter FromWebCatHistory is true, 
    ''' otherwise it looks up the history in Turn in.
    ''' </summary>
    ''' <param name="DeptID"></param>
    ''' <param name="VendorID"></param>
    ''' <param name="ColorCode"></param>
    ''' <param name="FromWebCatHistory"></param>
    ''' <returns>The frequently used color family</returns>
    ''' <remarks></remarks>
    Public Function GetFrequentlyUsedColorFamily(ByVal DeptID As Integer, ByVal VendorID As Integer, ByVal ColorCode As Integer, Optional ByVal FromWebCatHistory As Boolean = False) As ClrSizLocLookUp

        Dim sql As String = String.Empty
        Dim colorFamily As New ClrSizLocLookUp()

        sql = String.Concat(_spSchema, ".", If(FromWebCatHistory, "TU1178SP", "TU1152SP"))

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@DEPT_ID", DB2Type.Integer), _
                                                  New DB2Parameter("@VENDOR_ID", DB2Type.Integer), _
                                                  New DB2Parameter("@CLR_CDE", DB2Type.Integer)}

        parms(0).Value = DeptID
        parms(1).Value = VendorID
        parms(2).Value = ColorCode

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                If rdr.HasRows Then
                    rdr.Read()
                    With colorFamily
                        .Value = CStr(rdr("COLOR_KEY_NUM"))
                        .Text = CStr(rdr("COLOR_SIZE_FAM_NME"))
                    End With
                End If
            End Using

        Catch ex As Exception
            Throw ex
        End Try

        Return colorFamily
    End Function
    ''' <summary>
    ''' Gets the frequently used color family for the Dept, Vendor and Color code.
    ''' It looks up the history in style sku.
    ''' </summary>
    ''' <param name="DeptID"></param>
    ''' <param name="VendorID"></param>
    ''' <param name="ColorCode"></param>
    ''' <returns>The frequently used color family</returns>
    ''' <remarks></remarks>
    Public Function GetFrequentlyUsedColorFamilyFromStyleSKU(ByVal DeptID As Integer, ByVal VendorID As Integer, ByVal ColorCode As Integer) As ClrSizLocLookUp

        Dim sql As String = String.Empty
        Dim colorFamily As New ClrSizLocLookUp()

        sql = String.Concat(_spSchema, ".", "TU1180SP")

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@DEPT_ID", DB2Type.Integer), _
                                                  New DB2Parameter("@VENDOR_ID", DB2Type.Integer), _
                                                  New DB2Parameter("@CLR_CDE", DB2Type.Integer)}

        parms(0).Value = DeptID
        parms(1).Value = VendorID
        parms(2).Value = ColorCode

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                If rdr.HasRows Then
                    rdr.Read()
                    With colorFamily
                        .Value = CStr(rdr("COLOR_KEY_NUM"))
                        .Text = CStr(rdr("COLOR_SIZE_FAM_NME"))
                    End With
                End If
            End Using

        Catch ex As Exception
            Throw ex
        End Try

        Return colorFamily
    End Function
End Class
