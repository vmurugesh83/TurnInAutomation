Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.OleDb

Public Interface IDepartmentDao

    ''' <summary>
    ''' Method to get all DepartmentInfo records.
    ''' </summary>	    	 
    Function GetAllFromDepartment() As IList(Of DepartmentInfo)


End Interface