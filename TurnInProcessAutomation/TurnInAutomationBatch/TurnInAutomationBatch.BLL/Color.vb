Imports TurnInAutomationBatch.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities

Public Class Color
    Dim colorDAO As ColorDAO
    Public Sub New()
        colorDAO = New ColorDAO()
    End Sub

    Public Function GetFrequentlyUsedColorFamily(ByVal DeptID As Integer, ByVal VendorID As Integer, ByVal ColorCoode As String, Optional ByVal FromWebCatHistory As Boolean = False) As ClrSizLocLookUp
        Return colorDAO.GetFrequentlyUsedColorFamily(DeptID, VendorID, ColorCoode, FromWebCatHistory)
    End Function

    Public Function GetFrequentlyUsedFriendlyColor(ByVal DeptID As Integer, ByVal VendorID As Integer, ByVal ColorCoode As String, Optional ByVal FromWebCatHistory As Boolean = False) As String
        Return colorDAO.GetFrequentlyUsedFriendlyColor(DeptID, VendorID, ColorCoode, FromWebCatHistory)
    End Function
    Public Function GetFrequentlyUsedColorFamilyFromStyleSKU(ByVal DeptID As Integer, ByVal VendorID As Integer, ByVal ColorCoode As String) As ClrSizLocLookUp
        Return colorDAO.GetFrequentlyUsedColorFamilyFromStyleSKU(DeptID, VendorID, ColorCoode)
    End Function

End Class
