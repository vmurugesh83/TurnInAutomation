Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.OleDb

Public Interface IClassDao

    Function GetAllFromClassByDepartment(ByVal deptId As Integer) As IList(Of ClassInfo)

End Interface