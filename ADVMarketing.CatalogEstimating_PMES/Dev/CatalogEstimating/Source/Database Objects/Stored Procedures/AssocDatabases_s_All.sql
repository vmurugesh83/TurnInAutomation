IF OBJECT_ID('dbo.AssocDatabases_s_All') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.AssocDatabases_s_All'
	DROP PROCEDURE dbo.AssocDatabases_s_All
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.AssocDatabases_s_All') IS NOT NULL
		PRINT '***********Drop of dbo.AssocDatabases_s_All FAILED.'
END
GO
PRINT 'Creating dbo.AssocDatabases_s_All'
GO

CREATE PROCEDURE dbo.AssocDatabases_s_All
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Returns a list of Databases ordered by DisplayOrder
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   assoc_databases		Read
*	assoc_databasetype	Read
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/01/2007      NLS             Initial Creation 
* 06/22/2007	  NLS			  Modified to connect to new assoc_databasetype table
*
*/
AS BEGIN

SELECT 
	D.database_id, 
	D.description, 
	D.display, 
	D.displayorder, 
	D.databasetype_id,
	T.description AS databasetype, 
	D.connectionstring, 
	D.databasename,
	D.createdby, 
	D.createddate, 
	D.modifiedby, 
	D.modifieddate 

FROM dbo.assoc_databases AS D INNER JOIN dbo.assoc_databasetype AS T
ON D.databasetype_id = T.databasetype_id

ORDER BY D.displayorder ASC

END
GO

GRANT  EXECUTE  ON [dbo].[AssocDatabases_s_All]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
