IF OBJECT_ID('dbo.PubQuantity_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubQuantity_i'
	DROP PROCEDURE dbo.PubQuantity_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubQuantity_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubQuantity_i FAILED.'
END
GO
PRINT 'Creating dbo.PubQuantity_i'
GO

create proc dbo.PubQuantity_i
/*
* PARAMETERS:
* PUB_PubQuantity_ID - Output.  If a value is passed in it is ignored and the value is set to the ID of the new record.
*
*
* DESCRIPTION:
*		Inserts a record into the PUB_PubQuantity table.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubQuantity
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
* 07/05/2007      BJS             Added logic to check for duplicate effective date
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PUB_PubQuantity_ID bigint output,
@EffectiveDate datetime,
@PUB_PubRate_Map_ID bigint,
@CreatedBy varchar(50)
as

begin tran t
	if exists(select 1 from PUB_PubQuantity where PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID and EffectiveDate = @EffectiveDate) begin
		rollback tran t
		raiserror('The publication location already has quantities set on the same effective date.', 16, 1)
		return
	end

	insert into PUB_PubQuantity(EffectiveDate, PUB_PubRate_Map_ID, CreatedBy, CreatedDate)
	values(@EffectiveDate, @PUB_PubRate_Map_ID, @CreatedBy, getdate())
	set @PUB_PubQuantity_ID = @@identity
	if (@@error <> 0) begin
		rollback tran t
		raiserror('Error inserting PUB_PubQuantity record.', 16, 1)
		return
	end

commit tran t
GO

GRANT  EXECUTE  ON [dbo].[PubQuantity_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

