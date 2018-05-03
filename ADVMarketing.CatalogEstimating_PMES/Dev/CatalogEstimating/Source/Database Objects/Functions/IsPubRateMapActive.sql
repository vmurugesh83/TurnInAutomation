IF OBJECT_ID('dbo.IsPubRateMapActive') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.IsPubRateMapActive'
	DROP FUNCTION dbo.IsPubRateMapActive
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.IsPubRateMapActive') IS NOT NULL
		PRINT '***********Drop of dbo.IsPubRateMapActive FAILED.'
END
GO
-- PRINT 'Creating dbo.IsPubRateMapActive'
-- GO

/* 05/21/2008 BJS  - DROPPED.  No Longer Used */

/*
CREATE function dbo.IsPubRateMapActive(@PUB_PubRate_Map_ID bigint, @InsertDate datetime)
*/

/*
* PARAMETERS:
*	PUB_PubRate_Map_ID
*   InsertDate
*
* DESCRIPTION:
*	Returns whether a Pub Rate Map is active on a specific date.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   PUB_PubRate_Map_Activate     READ
*
* PROCEDURES CALLED:
*   None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	0 - Inactive
*   1 - Active
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/07/2007      BJS             Initial Creation
*
*/


-- returns bit
-- as
-- begin
-- 	declare @retval bit
-- 	if not exists(select 1 from PUB_PubRate_Map_Activate where PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID and EffectiveDate <= @InsertDate)
-- 		set @retval = 0
-- 	else
-- 		select top 1 @retval = Active
-- 		from PUB_PubRate_Map_Activate
-- 		where PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID and EffectiveDate <= @InsertDate
-- 		order by EffectiveDate desc
-- 	return(@retval)
-- end
-- GO
-- 
-- GRANT  EXECUTE  ON [dbo].[IsPubRateMapActive]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
-- GO
