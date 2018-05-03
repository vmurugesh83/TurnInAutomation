IF OBJECT_ID('dbo.VndMailListResourceRate_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailListResourceRate_i'
	DROP PROCEDURE dbo.VndMailListResourceRate_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailListResourceRate_i') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailListResourceRate_i FAILED.'
END
GO
PRINT 'Creating dbo.VndMailListResourceRate_i'
GO

CREATE PROC dbo.VndMailListResourceRate_i
@VND_MailListResourceRate_ID bigint output,
@VND_Vendor_ID bigint,
@InternalListRate money,
@EffectiveDate datetime,
@CreatedBy varchar(50)
/*
* PARAMETERS:
* VND_MailListResourceRate_ID
* VND_Vendor_ID
* EffectiveDate
* CreatedBy
*
* DESCRIPTION:
* Inserts a Mail List Resource record.  Updates any A&D records that may be affected. Returns the Mail List ID.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   VND_MailListResourceRate       INSERT
*   EST_Estimate                   UPDATE
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
* 10/08/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
as

declare @old_MailListResourceRate_ID bigint
select top 1 @old_MailListResourceRate_ID = VND_MailListResourceRate_ID
from VND_MailListResourceRate
where VND_Vendor_ID = @VND_Vendor_ID and EffectiveDate < @EffectiveDate
order by EffectiveDate

insert into VND_MailListResourceRate(VND_Vendor_ID, InternalListRate, EffectiveDate, CreatedBy, CreatedDate)
values(@VND_Vendor_ID, @InternalListRate, @EffectiveDate, @CreatedBy, getdate())
set @VND_MailListResourceRate_ID = @@identity
if (@@error <> 0) begin
	raiserror('Cannot create VND_MailListResourceRate record.', 16, 1)
	return
end

update e
	set e.ModifiedBy = @CreatedBy,
		e.ModifiedDate = getdate()
from EST_Estimate e join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
where e.RunDate >= @EffectiveDate and ad.MailListResource_ID = @old_MailListResourceRate_ID
if (@@error <> 0) begin
	raiserror('Cannot update EST_Estimate record.', 16, 1)
	return
end

update ad
	set
		ad.MailListResource_ID = @VND_MailListResourceRate_ID,
		ad.ModifiedBy = @CreatedBy,
		ad.ModifiedDate = getdate()
from EST_Estimate e join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
where e.RunDate >= @EffectiveDate and ad.MailListResource_ID = @old_MailListResourceRate_ID
if (@@error <> 0) begin
	raiserror('Cannot update EST_AssemDistribOptions record.', 16, 1)
	return
end
GO

GRANT  EXECUTE  ON [dbo].[VndMailListResourceRate_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
