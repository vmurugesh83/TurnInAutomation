Imports TurnInAutomationBatch.SQLDAL
Imports TurnInProcessAutomation.BusinessEntities
Public Class Ad
    Dim sqlDAO As AdDAO = Nothing
    Public Sub New()
        sqlDAO = New AdDAO()
    End Sub
    Public Function GetTheLatestAdforTIA(Optional ByVal isVendorImage As Boolean = False) As AdInfoInfo
        Try
            Return sqlDAO.GetTheLatestAdforTIA(isVendorImage)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
