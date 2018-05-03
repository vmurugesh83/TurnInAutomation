IF OBJECT_ID('dbo.PubPubGroupMap_s') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubPubGroupMap_s'
	DROP PROCEDURE dbo.PubPubGroupMap_s
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubPubGroupMap_s') IS NOT NULL
		PRINT '***********Drop of dbo.PubPubGroupMap_s FAILED.'
END
GO
PRINT 'Creating dbo.PubPubGroupMap_s'
GO

CREATE PROCEDURE dbo.PubPubGroupMap_s
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Returns all PUB_PubPubGroup_Map records.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubPubGroupMap
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
* 06/22/2007      BJS             Initial Creation 
*
*/
as
select *
from PUB_PubPubGroup_Map

GO

GRANT  EXECUTE  ON [dbo].[PubPubGroupMap_s]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
