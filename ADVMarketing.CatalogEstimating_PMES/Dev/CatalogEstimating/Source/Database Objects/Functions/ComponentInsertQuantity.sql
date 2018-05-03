IF OBJECT_ID('dbo.ComponentInsertQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentInsertQuantity'
	DROP FUNCTION dbo.ComponentInsertQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentInsertQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentInsertQuantity FAILED.'
END
GO

PRINT 'Creating dbo.ComponentInsertQuantity'
GO

CREATE function dbo.ComponentInsertQuantity(@EST_Component_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Component_ID - The Component ID
*
* DESCRIPTION:
*	Returns the total insert quantity for the specified component
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   EST_PackageComponentMapping  READ
*   PUB_PubGroup                 READ
*   PUB_PubPubGroup              READ
*   PUB_PubRate_Map              READ
*   PUB_PubQuantity              READ
*   PUB_DayOfWeekQuantity        READ
*   EST_PubIssueDates            READ
*   IsPubRateMapActive           READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total insert quantity for the specified component.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/23/2007      BJS             Initial Creation
* 09/13/2007      BJS             Updated reference to EST_PubInsertDates to EST_PubIssueDates
*                                 Removed join to PUB_PubRate_Map
* 05/21/2008      BJS             Removed call to IsPubRateMapActive, replaced with join and where logic
*/
as
begin
	return(select sum(dowqty.Quantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
		join PUB_PubGroup pg on p.PUB_PubGroup_ID = pg.PUB_PubGroup_ID
		join PUB_PubPubGroup_Map pppgm on pg.PUB_PubGroup_ID = pppgm.PUB_PubGroup_ID
		join EST_PubIssueDates pid on p.EST_Estimate_ID = pid.EST_Estimate_ID and pppgm.PUB_PubRate_Map_ID = pid.PUB_PubRate_Map_ID
		--join PUB_PubRate_Map pprm on pppgm.PUB_PubRate_Map_ID = pprm.PUB_PubRate_Map_ID
		join PUB_PubQuantity ppq on pppgm.PUB_PubRate_Map_ID = ppq.PUB_PubRate_Map_ID
			and ppq.PUB_PubQuantity_ID = dbo.CalcPubQuantityID(pppgm.PUB_PubRate_Map_ID, pid.IssueDate)
		join PUB_DayOfWeekQuantity dowqty on ppq.PUB_PubQuantity_ID = dowqty.PUB_PubQuantity_ID
			and p.PUB_PubQuantityType_ID = dowqty.PUB_PubQuantityType_ID
		JOIN PUB_PubRate_Map_Activate prma on pppgm.PUB_PubRate_Map_ID = prma.PUB_PubRate_Map_ID AND prma.Active = 1
			and prma.EffectiveDate <= pid.IssueDate
		LEFT JOIN PUB_PubRate_Map_Activate prma_newer ON pppgm.PUB_PubRate_Map_ID = prma_newer.PUB_PubRate_Map_ID
			AND prma_newer.Active = 1 AND prma_newer.EffectiveDate <= pid.IssueDate
			AND prma_newer.EffectiveDate > prma.EffectiveDate
	where pcm.EST_Component_ID = @EST_Component_ID
		AND prma_newer.PUB_PubRate_Map_ID IS NULL
		and (/*holidays*/ p.PUB_PubQuantityType_ID > 3 or /*fullrun / contract send*/ pid.IssueDOW = dowqty.InsertDow))
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentInsertQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
