IF OBJECT_ID('dbo.vwComponentInsertQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vwComponentInsertQuantity'
	DROP VIEW dbo.vwComponentInsertQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vwComponentInsertQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.vwComponentInsertQuantity FAILED.'
END
GO
PRINT 'Creating dbo.vwComponentInsertQuantity'
GO

CREATE VIEW dbo.vwComponentInsertQuantity
/*
* DESCRIPTION:
*		Returns ComponentID, EstimateID and Component Insert Quantity.
*
*
* TABLES:
*   Table Name                  Access
*   ==========                  ======
*   EST_PackageComponentMapping READ
*   EST_Package                 READ
*   PUB_PubGroup                READ
*   PUB_PubPubGroup_Map         READ
*   EST_PubIssueDates           READ
*   PUB_PubQuantity             READ
*   PUB_DayOfWeekQuantity       READ
*   PUB_PubRate_Map_Activate    READ
*
* PROCEDURES CALLED:
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/13/2007      BJS             Initial Creation
* 05/20/2008      BJS             Added locking hints to improve performance
*                                 Replaced call to IsPubRateMapActive logic with join to pub_pubrate_map_activate
*/
AS
SELECT pcm.EST_Component_ID, max(p.EST_Estimate_ID) EST_Estimate_ID, sum(dowqty.Quantity) InsertQuantity
FROM EST_PackageComponentMapping pcm (nolock) join EST_Package p (nolock) on pcm.EST_Package_ID = p.EST_Package_ID
	join PUB_PubGroup pg (nolock) on p.PUB_PubGroup_ID = pg.PUB_PubGroup_ID
	join PUB_PubPubGroup_Map pppgm (nolock) on pg.PUB_PubGroup_ID = pppgm.PUB_PubGroup_ID
	join EST_PubIssueDates pid (nolock) on p.EST_Estimate_ID = pid.EST_Estimate_ID and pppgm.PUB_PubRate_Map_ID = pid.PUB_PubRate_Map_ID
	join PUB_PubQuantity ppq (nolock) on pppgm.PUB_PubRate_Map_ID = ppq.PUB_PubRate_Map_ID
		and ppq.PUB_PubQuantity_ID = dbo.CalcPubQuantityID(pppgm.PUB_PubRate_Map_ID, pid.IssueDate)
	join PUB_DayOfWeekQuantity dowqty (nolock) on ppq.PUB_PubQuantity_ID = dowqty.PUB_PubQuantity_ID
		and p.PUB_PubQuantityType_ID = dowqty.PUB_PubQuantityType_ID
	JOIN PUB_PubRate_Map_Activate prma on pppgm.PUB_PubRate_Map_ID = prma.PUB_PubRate_Map_ID AND prma.Active = 1
		and prma.EffectiveDate <= pid.IssueDate
WHERE NOT EXISTS (SELECT 1 FROM PUB_PubRate_Map_Activate prma_newer
		WHERE prma.PUB_PubRate_Map_ID = prma_newer.PUB_PubRate_Map_ID and prma_newer.Active = 1
			and prma_newer.EffectiveDate <= pid.IssueDate and prma_newer.EffectiveDate > prma.EffectiveDate)
	and (/*holidays*/ p.PUB_PubQuantityType_ID > 3 or /*fullrun / contract send*/ pid.IssueDOW = dowqty.InsertDow)
GROUP BY pcm.EST_Component_ID
GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vwComponentInsertQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO