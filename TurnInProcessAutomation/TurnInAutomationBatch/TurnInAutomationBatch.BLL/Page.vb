Imports TurnInAutomationBatch.MainframeDAL
Public Class Page
    Dim pageDAO As PageDAO = Nothing
    Public Sub New()
        pageDAO = New PageDAO()
    End Sub
    Public Function GetPageNumberConfigByDeptID(ByVal DepartmentID As Integer) As Integer
        Dim pageNumber As Integer = 0
        pageNumber = pageDAO.GetPageNumberConfigByDeptID(DepartmentID)

        If pageNumber <= 0 Then
            pageNumber = 1
        End If

        Return pageNumber
    End Function
End Class
