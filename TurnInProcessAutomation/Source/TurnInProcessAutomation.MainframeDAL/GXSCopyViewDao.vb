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

Partial Public Class GXSCopyViewDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    Public Function FindByHierarchy(ByVal DeptId As Int16, ByVal ClassId As Int16, ByVal VendorId As Integer, ByVal VendorStyle As String) As IList(Of GXSCopyViewInfo)
        Dim gxsInfo As New List(Of GXSCopyViewInfo)

        Dim sql As String = _spSchema + ".TU1131SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@DEPT_ID", DB2Type.SmallInt), _
                                                          New DB2Parameter("@ClassID", DB2Type.SmallInt), _
                                                          New DB2Parameter("@VendoID", DB2Type.Integer), _
                                                          New DB2Parameter("@VendorStyle", DB2Type.Char)}
        parms(0).Value = DeptId
        parms(1).Value = ClassId
        parms(2).Value = VendorId
        parms(3).Value = VendorStyle

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    gxsInfo.Add(GXSCopyViewFactory.Construct(rdr))
                End While
            End Using
            Return gxsInfo
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Public Function FindByItems(ByVal isnXml As String, ByVal skuUpcXml As String) As IList(Of GXSCopyViewInfo)
        Dim gxsInfo As New List(Of GXSCopyViewInfo)

        Dim sql As String = _spSchema + ".TU1133SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.LongVarChar), _
                                                          New DB2Parameter("@SKU_UPC", DB2Type.LongVarChar)}
        parms(0).Value = isnXml
        parms(1).Value = skuUpcXml

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    gxsInfo.Add(GXSCopyViewFactory.Construct(rdr))
                End While
            End Using
            Return gxsInfo
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Public Function FindByAds(ByVal adNum As Integer, ByVal pageNum As String, ByVal batchNum As Integer) As IList(Of GXSCopyViewInfo)
        Dim gxsInfo As New List(Of GXSCopyViewInfo)

        Dim sql As String = _spSchema + ".TU1132SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ADNUM", DB2Type.Integer), _
                                                          New DB2Parameter("@PAGENUM", DB2Type.LongVarChar), _
                                                          New DB2Parameter("@BATCHNUM", DB2Type.Integer)}
        parms(0).Value = adNum
        parms(1).Value = pageNum
        parms(2).Value = batchNum

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    gxsInfo.Add(GXSCopyViewFactory.Construct(rdr))
                End While
            End Using
            Return gxsInfo
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Public Function FindByLabel(ByVal labelId As Integer) As IList(Of GXSCopyViewInfo)
        Dim gxsInfo As New List(Of GXSCopyViewInfo)

        Dim sql As String = _spSchema + ".TU1129SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@LABELID", DB2Type.Integer)}

        parms(0).Value = labelId

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    gxsInfo.Add(GXSCopyViewFactory.Construct(rdr))
                End While
            End Using
            Return gxsInfo
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Public Function FindByStartShipDate(ByVal startShipDate As Date, endShipDate As Date) As IList(Of GXSCopyViewInfo)
        Dim gxsInfo As New List(Of GXSCopyViewInfo)

        Dim sql As String = _spSchema + ".TU1130SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@STARTSHIPDT", DB2Type.Date), _
                                                          New DB2Parameter("@ENDSHIPDT", DB2Type.Date)}
        parms(0).Value = startShipDate
        parms(1).Value = endShipDate

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    gxsInfo.Add(GXSCopyViewFactory.Construct(rdr))
                End While
            End Using
            Return gxsInfo
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

End Class

