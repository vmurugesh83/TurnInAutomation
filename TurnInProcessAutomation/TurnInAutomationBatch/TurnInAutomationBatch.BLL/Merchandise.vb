Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInAutomationBatch.MainframeDAL
Imports TurnInAutomationBatch.BusinessEntities
Imports System.Xml
Public Class Merchandise
    Dim merchandiseDAO As MerchandiseDAO = Nothing
    Public Sub New()
        merchandiseDAO = New MerchandiseDAO()
    End Sub

    Public Function GetModelAttributes(ByVal DepartmentID As Integer, ByVal VendorID As Integer) As Model
        Return merchandiseDAO.GetModelAttributes(DepartmentID, VendorID)
    End Function
    Public Function GetMerchLevelAttributesByISN(ByVal InternalStyleNumber As Integer, ByVal VendorStyleNumber As String,
                                                 ByVal ColorCode As Integer, ByVal DeptID As Integer) As MerchLevelAttribute
        Return merchandiseDAO.GetMerchLevelAttributesByISN(InternalStyleNumber, VendorStyleNumber, ColorCode, DeptID)
    End Function
End Class
