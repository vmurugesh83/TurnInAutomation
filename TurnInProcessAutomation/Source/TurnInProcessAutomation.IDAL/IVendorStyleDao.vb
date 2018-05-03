Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.OleDb

Public Interface IVendorStyleDao

    Function GetAllFromVendorStyle(ByVal DeptId As Integer, ByVal VendorId As Integer, ByVal ClassId As Integer, ByVal SubClassId As Integer) As IList(Of VendorStyleInfo)

End Interface