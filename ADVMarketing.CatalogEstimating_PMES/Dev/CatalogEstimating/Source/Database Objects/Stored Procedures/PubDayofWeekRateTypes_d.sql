IF OBJECT_ID('dbo.PubDayofWeekRateTypes_d') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubDayofWeekRateTypes_d'
	DROP PROCEDURE dbo.PubDayofWeekRateTypes_d
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubDayofWeekRateTypes_d') IS NOT NULL
		PRINT '***********Drop of dbo.PubDayofWeekRateTypes_d FAILED.'
END
GO
PRINT 'Creating dbo.PubDayofWeekRateTypes_d'
GO

CREATE PROCEDURE dbo.PubDayofWeekRateTypes_d
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Deletes a pub_dayofweekratetypes record
*
* TABLES:
*   Table Name              Access
*   ==========              ======
*   PUB_DayofWeekRateTypes  DELETE
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
* 07/02/2007      BJS             Initial Creation 
*
*/
@PUB_DayofWeekRateTypes_ID bigint
as

delete from PUB_DayofWeekRateTypes
where PUB_DayofWeekRateTypes_ID = @PUB_DayofWeekRateTypes_ID
GO

GRANT  EXECUTE  ON [dbo].[PubDayofWeekRateTypes_d]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
