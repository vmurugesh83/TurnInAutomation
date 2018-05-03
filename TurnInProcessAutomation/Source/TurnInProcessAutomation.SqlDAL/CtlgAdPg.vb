Imports System.Configuration
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Text
Imports log4net
Imports BonTon.DBUtility
Imports BonTon.DBUtility.SqlHelper
Imports System.Data.SqlClient
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory


Partial Public Class CtlgAdPg
    'Static constants 
    Private Shared _sqldbSchema As String = ConfigurationManager.AppSettings("DBSchema")

    Public Function GetAllFromCtlgAdPg(ByVal AdNbr As Integer, Optional ByVal PageNbr As Integer = Nothing) As IList(Of CtlgAdPgInfo)
        Dim CtlgAdPgInfos As IList(Of CtlgAdPgInfo) = New List(Of CtlgAdPgInfo)()

        Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@AdNbr", SqlDbType.Int, 0), _
                                                         New SqlParameter("@PageNbr", SqlDbType.Int, 0)}
        parms(0).Value = AdNbr
        parms(1).Value = IIf(PageNbr = Nothing, DBNull.Value, PageNbr)

        'Execute a query to read the CtlgAdPgInfos
        Using rdr As SqlDataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "tu_ctlg_ad_pg_selectby_ad_nbr", parms)
            While (rdr.Read())
                'instantiate new CtlgAdPgInfo object via factory method and add to list
                CtlgAdPgInfos.Add(CtlgAdPgFactory.ConstructBasic(rdr))
            End While
        End Using
        Return CtlgAdPgInfos
    End Function

    Public Function GetAdPageDetail(ByVal AdNbr As Integer, ByVal PageNbr As Integer) As AdPageInfo
        Dim api As New AdPageInfo

        Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@AdNbr", SqlDbType.Int, 0), _
                                                         New SqlParameter("@PageNbr", SqlDbType.Int, 0)}
        parms(0).Value = AdNbr
        parms(1).Value = PageNbr

        'Execute a query to read the CtlgAdPgInfos
        Using rdr As SqlDataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "tu_ad_page_info", parms)
            While (rdr.Read())
                With api
                    .AdDesc = CStr(rdr("ad_desc"))
                    .PageDesc = CStr(rdr("pg_desc"))
                    .TUDate = CDate(rdr("tu_date"))
                End With
            End While
        End Using
        Return api
    End Function

    Public Function GetAdminImageNotes(ByVal AdNbrAdminImgNbr As String) As IList(Of AdminImageNotesInfo)
        Dim CtlgImgNotes As IList(Of AdminImageNotesInfo) = New List(Of AdminImageNotesInfo)()

        Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@ADs", SqlDbType.Text), _
                                                         New SqlParameter("@SCHEMA", SqlDbType.Char)}
        parms(0).Value = AdNbrAdminImgNbr
        parms(1).Value = _sqldbSchema

        'Execute a query to read the CtlgAdPgInfos
        Using rdr As SqlDataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "tu_ctlg_get_img_notes", parms)
            While (rdr.Read())
                CtlgImgNotes.Add(CtlgAdPgFactory.ConstructImgNotes(rdr))
            End While
        End Using
        Return CtlgImgNotes
    End Function

    Public Sub InsertCtlgAdPg(ByVal AdNbr As Integer, ByVal SysPgNbr As Integer, ByVal PgDesc As String, ByVal PgNbr As Integer, Optional ByVal tran As SqlTransaction = Nothing)
        ' Get each commands parameter arrays 
        Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@ad_nbr", SqlDbType.Int, 0), _
                                                          New SqlParameter("@sys_pg_nbr", SqlDbType.SmallInt, 0), _
                                                          New SqlParameter("@pg_desc", SqlDbType.Char, 30), _
                                                          New SqlParameter("@pg_nbr", SqlDbType.SmallInt, 0)}

        ' Set up the parameters 
        parms(0).Value = AdNbr
        parms(1).Value = SysPgNbr
        parms(2).Value = PgDesc
        parms(3).Value = PgNbr

        If tran IsNot Nothing Then
            ExecuteNonQuery(tran, CommandType.StoredProcedure, "tu_ctlg_ad_pg_insert", parms)
        Else
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "tu_ctlg_ad_pg_insert", parms)
        End If
    End Sub

    Public Sub UpdateCtlgAdPg(ByVal CtlgAdPgInfo As CtlgAdPgInfo, Optional ByVal tran As SqlTransaction = Nothing)
        ' Get each commands parameter arrays 
        Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@ad_nbr", SqlDbType.Int, 0), _
                                                          New SqlParameter("@sys_pg_nbr", SqlDbType.SmallInt, 0), _
                                                          New SqlParameter("@pg_desc", SqlDbType.Char, 30), _
                                                          New SqlParameter("@pg_nbr", SqlDbType.SmallInt, 0)}

        ' Set up the parameters 
        parms(0).Value = CtlgAdPgInfo.adnbr
        parms(1).Value = CtlgAdPgInfo.syspgnbr
        parms(2).Value = CtlgAdPgInfo.pgdesc
        parms(3).Value = CtlgAdPgInfo.pgnbr


        If tran IsNot Nothing Then
            ExecuteNonQuery(tran, CommandType.StoredProcedure, "tu_ctlg_ad_pg_update", parms)
        Else
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "tu_ctlg_ad_pg_update", parms)
        End If
    End Sub
End Class

