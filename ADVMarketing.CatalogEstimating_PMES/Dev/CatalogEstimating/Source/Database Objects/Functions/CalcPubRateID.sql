IF OBJECT_ID('dbo.CalcPubRateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.CalcPubRateID'
	DROP FUNCTION dbo.CalcPubRateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.CalcPubRateID') IS NOT NULL
		PRINT '***********Drop of dbo.CalcPubRateID FAILED.'
END
GO
PRINT 'Creating dbo.CalcPubRateID'
GO

create function dbo.CalcPubRateID(@PubRate_Map_ID bigint, @InsertDate datetime)
returns bigint
/*
* PARAMETERS:
*	PUB_PubRateMap_ID - the pub rate map
*   InsertDate        - Insert Date
*
* DESCRIPTION:
*	Determines the PUB Rate record for the given pub rate map and insert date
*
* TABLES:
*   Table Name              Access
*   ==========              ======
*   PUB_PubRate             READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	PubRateID
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
	declare @retval bigint
	declare @active bit

	select top 1 @active = Active
	from PUB_PubRate_Map_Activate
	where PUB_PubRate_Map_ID = @PubRate_Map_ID and EffectiveDate <= @InsertDate
	if (@active is null or @active = 1)
		begin
			select top 1 @retval = PUB_PubRate_ID
			from PUB_PubRate
			where PUB_PubRate_Map_ID = @PubRate_Map_ID and EffectiveDate <= @InsertDate
			order by EffectiveDate desc
		end
	else begin
		set @retval = null
	end
	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[CalcPubRateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
