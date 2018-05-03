Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.OleDb

Public Interface IACodeDao

    Function GetAllFromACodeByDepartment(ByVal deptId As Integer) As IList(Of ACodeInfo)

End Interface