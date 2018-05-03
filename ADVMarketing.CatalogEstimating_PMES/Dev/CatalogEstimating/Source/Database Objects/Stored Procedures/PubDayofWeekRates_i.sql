IF OBJECT_ID('dbo.PubDayofWeekRates_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubDayofWeekRates_i'
	DROP PROCEDURE dbo.PubDayofWeekRates_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubDayofWeekRates_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubDayofWeekRates_i FAILED.'
END
GO
PRINT 'Creating dbo.PubDayofWeekRates_i'
GO

create proc dbo.PubDayofWeekRates_i
/*
* PARAMETERS:
* PUB_DayofWeekRates_ID - Output.  If a value is passed in it is ignored and the value is set to the ID of the new record.
*
*
* DESCRIPTION:
*		Inserts a record into the PUB_DayofWeekRates table.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_DayofWeekRates
*
*
* PROCEDURES CALLED:
*
*
* DATABASE:
*		All
*
*
* RETURN VALUE:
* None 
*
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 05/25/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PUB_DayofWeekRates_ID bigint output,
@PUB_DayofWeekRateTypes_ID bigint,
@Rate money,
@InsertDOW int,
@CreatedBy int
as

insert into PUB_DayofWeekRates(PUB_DayofWeekRateTypes_ID, Rate, InsertDOW, CreatedBy, CreatedDate)
values(@PUB_DayofWeekRateTypes_ID, @Rate, @InsertDOW, @CreatedBy, getdate())
set @PUB_DayofWeekRates_ID = @@identity


GO

GRANT  EXECUTE  ON [dbo].[PubDayofWeekRates_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
