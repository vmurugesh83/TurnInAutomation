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


Partial Public Class IsTurnedInDao
    'Static constants 
    Private Shared _sqldbSchema As String = ConfigurationManager.AppSettings("DBSchema")

    Public Function GetIsTurnedIn(ByVal ISNs As String) As IList(Of IsTurnedIn)
        Dim IsTurnedInInfos As New List(Of IsTurnedIn)

        Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@ISNs", SqlDbType.VarChar),
                                                          New SqlParameter("@SCHEMA", SqlDbType.Char)}

        parms(0).Value = ISNs
        parms(1).Value = _sqldbSchema

        'Execute a query to read the CtlgAdPgInfos
        Using rdr As SqlDataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "[tu_printecomm_info]", parms)
            While (rdr.Read())
                'instantiate new CtlgAdPgInfo object via factory method and add to list
                Dim iti As New IsTurnedIn
                With iti
                    iti.ISN = CDec(rdr("ISN"))
                    iti.AdNumber = CInt(rdr("ad_nbr"))
                    iti.TUType = CChar(rdr("TUType"))
                    iti.StyleNumber = CStr(rdr("StyleNumber"))
                    iti.ColorDesc = CStr(rdr("ColorDesc"))
                    iti.VendorColorCode = CInt(rdr("VendorColorCode"))
                End With
                IsTurnedInInfos.Add(iti)
            End While
        End Using
        Return IsTurnedInInfos
    End Function

End Class

