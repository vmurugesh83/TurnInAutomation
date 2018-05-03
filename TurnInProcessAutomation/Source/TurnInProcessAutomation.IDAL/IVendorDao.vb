Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.OleDb

Public Interface IVendorDao

    Function GetAllFromVendorByDepartment(ByVal deptId As Integer) As IList(Of VendorInfo)

End Interface