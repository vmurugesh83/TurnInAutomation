Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.OleDb

Public Interface ISellYearDao

    ''' <summary>
    ''' Method to get all SellYearInfo records.
    ''' </summary>	    	 
    Function GetAllFromSellYear() As IList(Of SellYearInfo)


End Interface