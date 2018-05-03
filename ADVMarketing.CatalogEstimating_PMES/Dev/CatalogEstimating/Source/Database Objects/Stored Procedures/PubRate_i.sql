IF OBJECT_ID('dbo.PubRate_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRate_i'
	DROP PROCEDURE dbo.PubRate_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRate_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubRate_i FAILED.'
END
GO
PRINT 'Creating dbo.PubRate_i'
GO

create proc dbo.PubRate_i
/*
* PARAMETERS:
* PUB_PubRate_ID - Output.  If a value is passed in it is ignored and the value is set to the ID of the new record.
*
*
* DESCRIPTION:
*   Inserts a record into the PUB_PubRate table.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubRate
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
* 07/05/2007      BJS             Added logic to check for duplicate effective dates
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PUB_PubRate_ID bigint output,
@PUB_RateType_ID int,
@ChargeBlowIn bit,
@BlowInRate int,
@EffectiveDate datetime,
@QuantityChargeType int,
@BilledPct decimal,
@PUB_PubRate_Map_ID bigint,
@CreatedBy varchar(50)
as

begin tran t
	if exists(select 1 from PUB_PubRate where PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID and EffectiveDate = @EffectiveDate) begin
		rollback tran t
		raiserror('Rates already exist for the publication location on the same effective date.', 16, 1)
		return
	end

	insert into PUB_PubRate(PUB_RateType_ID, ChargeBlowIn, BlowInRate, EffectiveDate, QuantityChargeType, BilledPct, PUB_PubRate_Map_ID, CreatedBy,
		CreatedDate)
	values(@PUB_RateType_ID, @ChargeBlowIn, @BlowInRate, @EffectiveDate, @QuantityChargeType, @BilledPct, @PUB_PubRate_Map_ID, @CreatedBy, getdate())
	set @PUB_PubRate_ID = @@identity
	if (@@error <> 0) begin
		rollback tran t
		raiserror('Error inserting PUB_PubRate record.', 16, 1)
		return
	end

commit tran t
GO

GRANT  EXECUTE  ON [dbo].[PubRate_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
