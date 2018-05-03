Imports BonTon.DBUtility
Imports BonTon.DBUtility.SqlHelper
Imports System.Data.SqlClient
Imports TurnInProcessAutomation.Factory.Common

Public Class VirtualTicketDAO
    Public Function GetVTImageURLByVendorStyleNumber(ByVal vendorStyleNumbers As String) As Dictionary(Of String, String)
        Dim imageURLs As Dictionary(Of String, String) = Nothing
        Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@VendorStyleNumbers", SqlDbType.VarChar)}

        parms(0).Value = vendorStyleNumbers

        'Using reader As SqlDataReader = ExecuteReader(ConnectionStringVirtualTicket, CommandType.StoredProcedure, "GetImageDetailsByVendorStyleNumber", parms)
        '    imageURLs = New Dictionary(Of String, String)
        '    While (reader.Read())
        '        imageURLs.Add(ConvertToString(reader("VendorStyleNumber")), ConvertToString(reader("ImageURL")))
        '    End While
        'End Using
        Return imageURLs
    End Function
End Class
