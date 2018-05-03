
Imports System.Data.SqlClient
Imports TurnInProcessAutomation.BusinessEntities

Public Interface ICtlgAdPg
        
    ''' <summary>
    ''' Method to get all CtlgAdPgInfo records.
    ''' </summary>	    	 
    Function GetAllFromCtlgAdPg(ByVal AdNbr As Integer) As IList(Of CtlgAdPgInfo)

    ''' <summary>
    ''' Method to add a CtlgAdPgInfo record to the database.
    ''' </summary>	    	 
    Sub InsertCtlgAdPg(ByVal AdNbr As Integer, ByVal SysPgNbr As Integer, ByVal PgDesc As String, ByVal PgNbr As Integer, Optional ByVal tran As SqlTransaction = Nothing)

    ''' <summary>
    ''' Method to update a CtlgAdPgInfo record to the database.
    ''' </summary>	    	 
    Sub UpdateCtlgAdPg(ByVal CtlgAdPgInfo As CtlgAdPgInfo, Optional ByVal tran As SqlTransaction = Nothing)

End Interface