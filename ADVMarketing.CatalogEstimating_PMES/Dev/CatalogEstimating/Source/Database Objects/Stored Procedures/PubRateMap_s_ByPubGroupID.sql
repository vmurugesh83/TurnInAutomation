IF OBJECT_ID('dbo.PubRateMap_s_ByPubGroupID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRateMap_s_ByPubGroupID'
	DROP PROCEDURE dbo.PubRateMap_s_ByPubGroupID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRateMap_s_ByPubGroupID') IS NOT NULL
		PRINT '***********Drop of dbo.PubRateMap_s_ByPubGroupID FAILED.'
END
GO
PRINT 'Creating dbo.PubRateMap_s_ByPubGroupID'
GO

CREATE PROCEDURE dbo.PubRateMap_s_ByPubGroupID
/*
* PARAMETERS:
*	PUB_PubGroup_ID - required
*
* DESCRIPTION:
*	Returns all publication locations for the given Pub Group ID.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubRate_Map     READ
*   PUB_PubPubGroup_Map READ
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
* 09/04/2007      BJS             Initial Creation
*
*/
@PUB_PubGroup_ID bigint
as
select prm.*
from PUB_PubPubGroup_Map ppgm join PUB_PubRate_Map prm on ppgm.PUB_PubRate_Map_ID = prm.PUB_PubRate_Map_ID
where ppgm.PUB_PubGroup_ID = @PUB_PubGroup_ID
GO

GRANT  EXECUTE  ON [dbo].[PubRateMap_s_ByPubGroupID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
