Imports TurnInProcessAutomation.SqlDAL

Public Class VirtualTicketInfo
    Public Function GetVTImageURLByVendorStyleNumber(ByVal vendorStyleNumbers As String) As Dictionary(Of String, String)
        Dim virtualTicketDAO As SqlDAL.VirtualTicketDAO = New SqlDAL.VirtualTicketDAO
        Try
            Return virtualTicketDAO.GetVTImageURLByVendorStyleNumber(vendorStyleNumbers)
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
