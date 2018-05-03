IF OBJECT_ID('dbo.PubRateMapActivate_s_ByPubIDandPubLocID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRateMapActivate_s_ByPubIDandPubLocID'
	DROP PROCEDURE dbo.PubRateMapActivate_s_ByPubIDandPubLocID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRateMapActivate_s_ByPubIDandPubLocID') IS NOT NULL
		PRINT '***********Drop of dbo.PubRateMapActivate_s_ByPubIDandPubLocID FAILED.'
END
GO
PRINT 'Creating dbo.PubRateMapActivate_s_ByPubIDandPubLocID'
GO

create PROCEDURE dbo.PubRateMapActivate_s_ByPubIDandPubLocID
/*
* PARAMETERS:
*	none
*
* DESCRIPTION:
*	Retrieves a list of effective dates and the active status for a pub location.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubRate_Map
*   PUB_PubRate_Map_Activate
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
* 06/26/2007      BJS             Initial Creation 
*
*/
@Pub_ID char(3),
@PubLoc_ID int
as

select prma.EffectiveDate, prma.Active
from PUB_PubRate_Map prm
	join PUB_PubRate_Map_Activate prma on prm.PUB_PubRate_Map_ID = prma.PUB_PubRate_Map_ID
where prm.Pub_ID = @Pub_ID and prm.PubLoc_ID = @PubLoc_ID
order by prma.EffectiveDate desc
GO

GRANT  EXECUTE  ON [dbo].[PubRateMapActivate_s_ByPubIDandPubLocID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
