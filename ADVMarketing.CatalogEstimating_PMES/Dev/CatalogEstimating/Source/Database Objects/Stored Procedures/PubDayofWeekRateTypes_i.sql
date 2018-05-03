IF OBJECT_ID('dbo.PubDayofWeekRateTypes_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubDayofWeekRateTypes_i'
	DROP PROCEDURE dbo.PubDayofWeekRateTypes_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubDayofWeekRateTypes_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubDayofWeekRateTypes_i FAILED.'
END
GO
PRINT 'Creating dbo.PubDayofWeekRateTypes_i'
GO

create proc dbo.PubDayofWeekRateTypes_i
/*
* PARAMETERS:
* PUB_DayofWeekRateTypes_ID - Output.  If a value is passed in it is ignored and the value is set to the ID of the new record.
*
*
* DESCRIPTION:
*		Inserts a record into the PUB_DayofWeekRateTypes table.
*
*
* TABLES:
*   Table Name              Access
*   ==========              ======
*   PUB_DayofWeekRateTypes  INSERT
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
@PUB_DayofWeekRateTypes_ID bigint output,
@RateTypeDescription decimal,
@PUB_PubRate_ID bigint,
@CreatedBy varchar(50)
as

insert into PUB_DayofWeekRateTypes(RateTypeDescription, PUB_PubRate_ID, CreatedBy, CreatedDate)
values(@RateTypeDescription, @PUB_PubRate_ID, @CreatedBy, getdate())
set @PUB_DayofWeekRateTypes_ID = @@identity


GO

GRANT  EXECUTE  ON [dbo].[PubDayofWeekRateTypes_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
