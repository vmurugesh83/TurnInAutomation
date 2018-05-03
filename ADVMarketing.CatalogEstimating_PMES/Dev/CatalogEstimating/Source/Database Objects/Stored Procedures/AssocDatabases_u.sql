IF OBJECT_ID('dbo.AssocDatabases_u') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.AssocDatabases_u'
	DROP PROCEDURE dbo.AssocDatabases_u
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.AssocDatabases_u') IS NOT NULL
		PRINT '***********Drop of dbo.AssocDatabases_u FAILED.'
END
GO
PRINT 'Creating dbo.AssocDatabases_u'
GO

CREATE PROCEDURE dbo.AssocDatabases_u
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Updates a assoc_databases record
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   assoc_databases		  Update
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
* 07/05/2007      BJS             Initial Creation 
*
*/
@Database_ID int,
@Description varchar(50),
@Display bit,
@DisplayOrder int
as

update assoc_databases
	set
		Description = @Description,
		Display = @Display,
		DisplayOrder = @DisplayOrder
where Database_ID = @Database_ID
GO

GRANT  EXECUTE  ON [dbo].[AssocDatabases_u]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

