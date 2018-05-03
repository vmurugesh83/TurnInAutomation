IF OBJECT_ID('dbo.PubRateMap_s_WithDescription_ByPubGroupID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRateMap_s_WithDescription_ByPubGroupID'
	DROP PROCEDURE dbo.PubRateMap_s_WithDescription_ByPubGroupID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRateMap_s_WithDescription_ByPubGroupID') IS NOT NULL
		PRINT '***********Drop of dbo.PubRateMap_s_WithDescription_ByPubGroupID FAILED.'
END
GO
PRINT 'Creating dbo.PubRateMap_s_WithDescription_ByPubGroupID'
GO

CREATE PROCEDURE dbo.PubRateMap_s_WithDescription_ByPubGroupID
/*
* PARAMETERS:
*	PUB_PubGroup_ID - The pub group that limits the result set.
*
* DESCRIPTION:
*	Returns all PUB_PubRate_Map, Publication descriptions and whether or not they are being referenced by the specified Pub Group.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubPubGroupMap
*   PUB_PubRate_Map
*   ADMINSYSTEM.Pub
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
* 06/25/2007      BJS             Initial Creation 
*
*/
@PUB_PubGroup_ID bigint
as
select @PUB_PubGroup_ID PUB_PubGroup_ID, prm.PUB_PubRate_Map_ID, max(prm.Pub_ID) Pub_ID, max(prm.PubLoc_ID) PubLoc_ID, max(p.pub_nm) Pub_NM,
	case
		when max(ppgm.PUB_PubGroup_ID) is not null then 1
		else 0
	end InPubGroup
from PUB_PubRate_Map prm join DBADVPROD.informix.Pub p on prm.Pub_ID = p.Pub_ID
	left join PUB_PubPubGroup_Map ppgm on prm.PUB_PubRate_Map_ID = ppgm.PUB_PubRate_Map_ID and ppgm.PUB_PubGroup_ID = @PUB_PubGroup_ID
group by prm.PUB_PubRate_Map_ID
order by prm.PUB_ID, prm.PubLoc_ID
GO

GRANT  EXECUTE  ON [dbo].[PubRateMap_s_WithDescription_ByPubGroupID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
