
Imports TurnInProcessAutomation.BusinessEntities

Public Interface ITEC100_CATEGORYDao
        
    ''' <summary>
    ''' Method to get all TEC100_CATEGORYInfo records.
    ''' </summary>	    	 
    Function GetAllFromTEC100_CATEGORY() as IList(Of TEC100_CATEGORYInfo)

    ''' <summary>
    ''' Method to get a TEC100_CATEGORYInfo record by its primary key column(s).
    ''' </summary>	    	 
    Function GetTEC100_CATEGORYByParentCde(ByVal parentCde As Integer) As IList(Of TEC100_CATEGORYInfo)
	
    ''' <summary>
    ''' Method to add a TEC100_CATEGORYInfo record to the database.
    ''' </summary>	    	 
    Sub InsertTEC100_CATEGORY(ByVal categoryCde As Integer, ByVal parentCde As Integer, ByVal startDte As DateTime, ByVal endDte As DateTime, ByVal activeFlg As String, ByVal activeDte As DateTime?, ByVal ordinalNum As Integer, ByVal imageIdNum As Integer, ByVal displayOnlyFlg As String, ByVal sesTitle As String, ByVal createDte As DateTime, ByVal modifyDte As DateTime, ByVal logonUser As String, ByVal parentDefaultFlg As String, ByVal sesUrlValue As String, ByVal inactiveTs As DateTime?, ByVal categoryDesc As String, ByVal categoryNme As String, ByVal sesMetaDesc As String, ByVal sesMetaKeyWords As String, ByVal templateId As Integer, ByVal revenueTierCde As Integer, ByVal dfltSeoTitleNme As String, ByVal cusSeoTitleNme As String, ByVal seoTitleCde As Integer, ByVal addTitleNameCde As Integer, ByVal defaultSeoDesc As String, ByVal customSeoDesc As String, ByVal seoDescCde As Integer, ByVal addDescNameCde As Integer, ByVal addShipInfoCde As Integer, ByVal seoShipInfoTxt As String, ByVal affilCategoryTxt As String, ByVal catgySrtOrdrNum As Integer, ByVal disColorFamCde As Integer, ByVal disSizeFamCde As Integer)
    
    ''' <summary>
    ''' Method to update a TEC100_CATEGORYInfo record to the database.
    ''' </summary>	    	 
    Sub UpdateTEC100_CATEGORY(ByVal tec100CategoryInfo as TEC100_CATEGORYInfo)
    
    ''' <summary>
    ''' Method to delete a TEC100_CATEGORYInfo record.
    ''' </summary>	    	 
    Sub DeleteTEC100_CATEGORY(ByVal categoryCde as Integer, ByVal parentCde as Integer)
	                    
End Interface