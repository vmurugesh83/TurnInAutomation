This service is referenced by the Complete Turn-in workflow from MessageBroker

This service references the following stored procedure on ADmin databases
which need to be deployed before go live.

*** Stored Procedures ***
WH_GetChangesForAd
CMR_FullData_Master
	CMR_FullData_BCastExtract
	CMR_FullData_CatExtract
	CMR_FullData_MerchCatNoImage
	CMR_FullData_ROPExtract

	Change the following code in the web.config before deployment to point to the production server.

<!-- PROD -->
<add name="SQLServer" connectionString="Server=M055-SQL-2V;Initial Catalog=DBADVPROD;User ID=INFORMIX;Password=XXXXXXXX;Persist Security Info=True" providerName=""/>

