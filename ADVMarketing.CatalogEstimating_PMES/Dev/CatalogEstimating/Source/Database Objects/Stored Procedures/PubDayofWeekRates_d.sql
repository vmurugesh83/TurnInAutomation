IF OBJECT_ID('dbo.PubDayofWeekRates_d') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubDayofWeekRates_d'
	DROP PROCEDURE dbo.PubDayofWeekRates_d
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubDayofWeekRates_d') IS NOT NULL
		PRINT '***********Drop of dbo.PubDayofWeekRates_d FAILED.'
END
GO
PRINT 'Creating dbo.PubDayofWeekRates_d'
GO

CREATE PROCEDURE dbo.PubDayofWeekRates_d
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Deletes a pub_dayofweekrates record
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_DayofWeekRates  Delete
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
@PUB_DayofWeekRates_ID bigint
as

delete from PUB_DayofWeekRates
where PUB_DayofWeekRates_ID = @PUB_DayofWeekRates_ID
GO

GRANT  EXECUTE  ON [dbo].[PubDayofWeekRates_d]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
