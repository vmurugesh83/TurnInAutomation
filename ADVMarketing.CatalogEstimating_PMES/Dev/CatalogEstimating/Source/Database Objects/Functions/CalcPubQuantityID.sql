IF OBJECT_ID('dbo.CalcPubQuantityID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.CalcPubQuantityID'
	DROP FUNCTION dbo.CalcPubQuantityID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.CalcPubQuantityID') IS NOT NULL
		PRINT '***********Drop of dbo.CalcPubQuantityID FAILED.'
END
GO
PRINT 'Creating dbo.CalcPubQuantityID'
GO

create function dbo.CalcPubQuantityID(@PubRate_Map_ID bigint, @InsertDate datetime)
returns bigint
/*
* PARAMETERS:
*	PUB_PubRateMap_ID - the pub rate map
*   InsertDate        - Insert Date
*
* DESCRIPTION:
*	Determines the PUB Quantity record for the given pub rate map and insert date
*
* TABLES:
*   Table Name              Access
*   ==========              ======
*   PUB_PubQuantity         READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	PubQuantityID
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/10/2007      BJS             Initial Creation
*
*/
as
begin
	return(select top 1 PUB_PubQuantity_ID
		from PUB_PubQuantity
		where PUB_PubRate_Map_ID = @PubRate_Map_ID and EffectiveDate <= @InsertDate
		order by EffectiveDate desc)
end
GO

GRANT  EXECUTE  ON [dbo].[CalcPubQuantityID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO