IF OBJECT_ID('dbo.VndMailHouseRate_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailHouseRate_i'
	DROP PROCEDURE dbo.VndMailHouseRate_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailHouseRate_i') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailHouseRate_i FAILED.'
END
GO
PRINT 'Creating dbo.VndMailHouseRate_i'
GO

CREATE PROC dbo.VndMailHouseRate_i
@VND_MailHouseRate_ID bigint output,
@VND_Vendor_ID bigint,
@TimeValueSlips money,
@InkjetRate money,
@InkjetMakeready money,
@AdminFee money,
@PostalDropCWT money,
@GlueTackDefault bit,
@GlueTackRate money,
@TabbingDefault bit,
@TabbingRate money,
@LetterInsertionDefault bit,
@LetterInsertionRate money,
@EffectiveDate datetime,
@CreatedBy varchar(50)
/*
* PARAMETERS:
* VND_MailHouseRate_ID
* VND_Vendor_ID
* TimeValueSlips
* InkjetRate
* InkjetMakeready
* AdminFee
* PostalDropCWT
* GlueTackDefault
* GlueTackRate
* TabbingDefault
* TabbingRate
* LetterInsertionDefault
* LetterInsertionRate
* EffectiveDate
* CreatedBy
*
* DESCRIPTION:
* Inserts a MailHouse record.  Updates any A&D records that may be affected. Returns the MailHouse ID.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   VND_MailHouseRate              INSERT
*   EST_AssemDistribOptions        UPDATE
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
* 10/05/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
as

declare @old_MailHouseRate_ID bigint
select top 1 @old_MailHouseRate_ID = VND_MailHouseRate_ID
from VND_MailHouseRate
where VND_Vendor_ID = @VND_Vendor_ID and EffectiveDate < @EffectiveDate
order by EffectiveDate

insert into VND_MailHouseRate(VND_Vendor_ID, TimeValueSlips, InkjetRate, InkjetMakeready, AdminFee, PostalDropCWT, GlueTackDefault,
	GlueTackRate, TabbingDefault, TabbingRate, LetterInsertionDefault, LetterInsertionRate, EffectiveDate, CreatedBy, CreatedDate)
values(@VND_Vendor_ID, @TimeValueSlips, @InkjetRate, @InkjetMakeready, @AdminFee, @PostalDropCWT, @GlueTackDefault,
	@GlueTackRate, @TabbingDefault, @TabbingRate, @LetterInsertionDefault, @LetterInsertionRate, @EffectiveDate, @CreatedBy, getdate())
set @VND_MailHouseRate_ID = @@identity
if (@@error <> 0) begin
	raiserror('Cannot create VND_MailHouseRate record.', 16, 1)
	return
end

update e
	set e.ModifiedBy = @CreatedBy,
		e.ModifiedDate = getdate()
from EST_Estimate e join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
where e.RunDate >= @EffectiveDate and ad.MailHouse_ID = @old_MailHouseRate_ID
if (@@error <> 0) begin
	raiserror('Cannot update EST_Estimate record.', 16, 1)
	return
end

update ad
	set
		ad.MailHouse_ID = @VND_MailHouseRate_ID,
		ad.ModifiedBy = @CreatedBy,
		ad.ModifiedDate = getdate()
from EST_Estimate e join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
where e.RunDate >= @EffectiveDate and ad.MailHouse_ID = @old_MailHouseRate_ID
if (@@error <> 0) begin
	raiserror('Cannot update EST_AssemDistribOptions record.', 16, 1)
	return
end
GO

GRANT  EXECUTE  ON [dbo].[VndMailHouseRate_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
