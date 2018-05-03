Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.OleDb

Public Interface ISubClassDao

    Function GetAllFromSubClassByDeptClass(ByVal deptId As Integer, ByVal classID As Integer) As IList(Of SubClassInfo)

End Interface