IF OBJECT_ID('dbo.VndMailTrackingRate_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailTrackingRate_i'
	DROP PROCEDURE dbo.VndMailTrackingRate_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailTrackingRate_i') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailTrackingRate_i FAILED.'
END
GO
PRINT 'Creating dbo.VndMailTrackingRate_i'
GO

CREATE PROC dbo.VndMailTrackingRate_i
@VND_MailTrackingRate_ID bigint output,
@VND_Vendor_ID bigint,
@MailTracking money,
@EffectiveDate datetime,
@CreatedBy varchar(50)
/*
* PARAMETERS:
* VND_MailTrackingRate_ID
* VND_Vendor_ID
* MailTracking
* EffectiveDate
* CreatedBy
*
* DESCRIPTION:
* Inserts a Mail Tracking record.  Updates any A&D records that may be affected. Returns the Mail Tracking ID.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   VND_MailTrackingRate           INSERT
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

declare @old_MailTrackingRate_ID bigint
select top 1 @old_MailTrackingRate_ID = VND_MailTrackingRate_ID
from VND_MailTrackingRate
where VND_Vendor_ID = @VND_Vendor_ID and EffectiveDate < @EffectiveDate
order by EffectiveDate

insert into VND_MailTrackingRate(VND_Vendor_ID, MailTracking, EffectiveDate, CreatedBy, CreatedDate)
values(@VND_Vendor_ID, @MailTracking, @EffectiveDate, @CreatedBy, getdate())
set @VND_MailTrackingRate_ID = @@identity
if (@@error <> 0) begin
	raiserror('Cannot create VND_MailTrackingRate record.', 16, 1)
	return
end

update e
	set e.ModifiedBy = @CreatedBy,
		e.ModifiedDate = getdate()
from EST_Estimate e join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
where e.RunDate >= @EffectiveDate and ad.MailTracking_ID = @old_MailTrackingRate_ID
if (@@error <> 0) begin
	raiserror('Cannot update EST_Estimate record.', 16, 1)
	return
end

update ad
	set
		ad.MailHouse_ID = @VND_MailTrackingRate_ID,
		ad.ModifiedBy = @CreatedBy,
		ad.ModifiedDate = getdate()
from EST_Estimate e join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
where e.RunDate >= @EffectiveDate and ad.MailTracking_ID = @old_MailTrackingRate_ID
if (@@error <> 0) begin
	raiserror('Cannot update EST_AssemDistribOptions record.', 16, 1)
	return
end
GO

GRANT  EXECUTE  ON [dbo].[VndMailTrackingRate_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
