IF OBJECT_ID('dbo.PubRateMap_s_ByPubIDandPubLocID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRateMap_s_ByPubIDandPubLocID'
	DROP PROCEDURE dbo.PubRateMap_s_ByPubIDandPubLocID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRateMap_s_ByPubIDandPubLocID') IS NOT NULL
		PRINT '***********Drop of dbo.PubRateMap_s_ByPubIDandPubLocID FAILED.'
END
GO
PRINT 'Creating dbo.PubRateMap_s_ByPubIDandPubLocID'
GO

CREATE PROCEDURE dbo.PubRateMap_s_ByPubIDandPubLocID
/*
* PARAMETERS:
*	Pub_ID - required
*   PubLoc_ID - required
*   RunDate - required.
*
* DESCRIPTION:
*	Returns the PubRate_Map for the Pub_ID and PubLoc_ID specified.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubRate_Map     READ
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
* 09/05/2007      BJS             Initial Creation
* 09/07/2007      BJS             Added RunDate as criteria.  A record is only returned if the pubratemap is active on the RunDate.
* 05/21/2008      BJS             Removed reference to dbo.IsPubRateMapActive to improve performance
*
*/
@Pub_ID char(3),
@PubLoc_ID int,
@RunDate datetime
as

select * from PUB_PubRate_Map prm
	JOIN PUB_PubRate_Map_Activate prma on prm.PUB_PubRate_Map_ID = prma.PUB_PubRate_Map_ID AND prma.Active = 1
		and prma.EffectiveDate <= @RunDate
WHERE prm.Pub_ID = @Pub_ID AND prm.PubLoc_ID = @PubLoc_ID
	AND NOT EXISTS (SELECT 1 FROM PUB_PubRate_Map_Activate prma_newer
		WHERE prma.PUB_PubRate_Map_ID = prma_newer.PUB_PubRate_Map_ID and prma_newer.Active = 1
			and prma_newer.EffectiveDate <= @RunDate and prma_newer.EffectiveDate > prma.EffectiveDate)
GO

GRANT  EXECUTE  ON [dbo].[PubRateMap_s_ByPubIDandPubLocID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
