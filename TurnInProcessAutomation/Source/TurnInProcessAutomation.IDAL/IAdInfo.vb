Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Interface IAdInfo

    ''' <summary>
    ''' Method to get all AdInfoInfo records.
    ''' </summary>	    	 
    Function GetAllFromAdInfo() As IList(Of AdInfoInfo)

    ''' <summary>
    ''' Method to get all AdInfoInfo records.
    ''' </summary>	    	 
    Function GetAllFromAdInfoFiltered(ByVal IsEcommerce As Boolean) As IList(Of AdInfoInfo)

    ''' <summary>
    ''' Method to get a AdInfoInfo record by its primary key column(s).
    ''' </summary>	    	 
    Function GetAdInfoByAdNbr(ByVal AdNbr As Integer) As AdInfoInfo

    ''' <summary>
    ''' Method to add a AdInfoInfo record to the database.
    ''' </summary>	    	 
    Sub InsertAdInfo(ByVal AdNbr As Integer, ByVal AdDesc As String, ByVal AdStatCd As String, ByVal MediaCd As String, ByVal MediaTypeCd As String, ByVal AdRunStartDt As DateTime, ByVal AdRunEndDt As DateTime, ByVal EventNbr As Integer, ByVal SaleNbr As Integer, ByVal FiscalYr As Integer, ByVal SeasonCd As String, ByVal FiscalMthNbr As String, ByVal PrdctnSrcCd As String, ByVal DistbnMethCd As String, ByVal AdSponTypeCd As String, ByVal AdSponNbr As Integer, ByVal FinclSponTypeCd As String, ByVal FinclSponNbr As Integer, ByVal CoopAmt As Decimal, ByVal CoopPct As Decimal, ByVal AdBdgtAmt As Decimal, ByVal LeaseFiscalYr As Integer, ByVal LeaseFiscalMth As String, ByVal AdPabPct As Decimal, ByVal ClsdDscnryInd As String, ByVal LeaseClsdInd As String, ByVal CoopDscnryInd As String, ByVal CoopAdjmntInd As String, ByVal MediaChrgdNbr As String, ByVal DateAdded As DateTime, ByVal MerchCriteriaMin As Decimal, ByVal MerchCriteriaMax As Decimal, ByVal MerchCriteriaInd As String, ByVal SendToC3 As String, ByVal VrsnToC3 As String, ByVal DateToAprimo As DateTime, ByVal DateToC3 As DateTime, ByVal AdDescToC3 As String, Optional ByVal tran As SqlTransaction = Nothing)

    ''' <summary>
    ''' Method to update a AdInfoInfo record to the database.
    ''' </summary>	    	 
    Sub UpdateAdInfo(ByVal AdinfoInfo As AdInfoInfo, Optional ByVal tran As SqlTransaction = Nothing)

    ''' <summary>
    ''' Method to delete a AdInfoInfo record.
    ''' </summary>	    	 
    Sub DeleteAdInfo(ByVal AdNbr As Integer, Optional ByVal tran As SqlTransaction = Nothing)

End Interface