IF OBJECT_ID('dbo.EstPackage_s_InsertScenario') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPackage_s_InsertScenario'
	DROP PROCEDURE dbo.EstPackage_s_InsertScenario
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPackage_s_InsertScenario') IS NOT NULL
		PRINT '***********Drop of dbo.EstPackage_s_InsertScenario FAILED.'
END
GO
PRINT 'Creating dbo.EstPackage_s_InsertScenario'
GO

CREATE PROCEDURE dbo.EstPackage_s_InsertScenario
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Returns a list of insert scenarios that are actively being used by estimates.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   est_package		    Read
*   pub_insertscenario  Read
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
* 10/22/2007      NLS             Initial Creation 
*
*/
AS BEGIN

SELECT DISTINCT pub.pub_insertscenario_id, pub.description
    FROM pub_insertscenario pub LEFT JOIN est_package pkg ON pub.pub_insertscenario_id = pkg.pub_insertscenario_id
    WHERE pkg.pub_insertscenario_id is not null
ORDER BY pub.description ASC

END
GO

GRANT  EXECUTE  ON [dbo].[EstPackage_s_InsertScenario]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
