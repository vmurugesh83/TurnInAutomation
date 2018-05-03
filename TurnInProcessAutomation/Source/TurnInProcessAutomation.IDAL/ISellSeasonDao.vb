Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.OleDb

Public Interface ISellSeasonDao

    ''' <summary>
    ''' Method to get all SellSeasonInfo records.
    ''' </summary>	    	 
    Function GetAllFromSellSeason() As IList(Of SellSeasonInfo)


End Interface