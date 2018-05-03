IF OBJECT_ID('dbo.PubDayofWeekQuantity_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubDayofWeekQuantity_i'
	DROP PROCEDURE dbo.PubDayofWeekQuantity_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubDayofWeekQuantity_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubDayofWeekQuantity_i FAILED.'
END
GO
PRINT 'Creating dbo.PubDayofWeekQuantity_i'
GO

create proc dbo.PubDayofWeekQuantity_i
/*
* PARAMETERS:
* PUB_DayofWeekQuantity_ID - Output.  If a value is passed in it is ignored and the value is set to the ID of the new record.
*
*
* DESCRIPTION:
*		Inserts a record into the PUB_DayofWeekQuantity table.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_DayofWeekQuantity
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
@PUB_DayofWeekQuantity_ID bigint output,
@PUB_PubQuantity_ID bigint,
@PUB_PubQuantityType_ID bigint,
@InsertDOW int,
@Quantity int,
@CreatedBy int
as

insert into PUB_DayofWeekQuantity(PUB_PubQuantity_ID, PUB_PubQuantityType_ID, Quantity, InsertDOW, CreatedBy, CreatedDate)
values(@PUB_PubQuantity_ID, @PUB_PubQuantityType_ID, @Quantity, @InsertDOW, @CreatedBy, getdate())
set @PUB_DayofWeekQuantity_ID = @@identity


GO

GRANT  EXECUTE  ON [dbo].[PubDayofWeekQuantity_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
