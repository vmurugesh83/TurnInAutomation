IF OBJECT_ID('dbo.VndMailTrackingRate_d_ByMailTrackingRateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailTrackingRate_d_ByMailTrackingRateID'
	DROP PROCEDURE dbo.VndMailTrackingRate_d_ByMailTrackingRateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailTrackingRate_d_ByMailTrackingRateID') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailTrackingRate_d_ByMailTrackingRateID FAILED.'
END
GO
PRINT 'Creating dbo.VndMailTrackingRate_d_ByMailTrackingRateID'
GO

CREATE PROC dbo.VndMailTrackingRate_d_ByMailTrackingRateID
@VND_MailTrackingRate_ID bigint,
@ModifiedBy varchar(50)
/*
* PARAMETERS:
* VND_MailTrackingRate_ID
*
* DESCRIPTION:
* Deletes a MailTracking record.  Updates any A&D records that may be affected.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   VND_MailTrackingRate           DELETE
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

declare @new_MailTrackingRate_ID bigint
select top 1 @new_MailTrackingRate_ID = new_mh.VND_MailTrackingRate_ID
from VND_MailTrackingRate orig_mh join VND_MailTrackingRate new_mh on orig_mh.VND_Vendor_ID = new_mh.VND_Vendor_ID
where orig_mh.VND_MailTrackingRate_ID = @VND_MailTrackingRate_ID and orig_mh.VND_MailTrackingRate_ID <> new_mh.VND_MailTrackingRate_ID
order by new_mh.EffectiveDate

--If there is not an earlier set of Mail House Rates for this Vendor, delete the record
if (@new_MailTrackingRate_ID is null) begin
	if exists(select 1 from EST_AssemDistribOptions where MailTracking_ID = @VND_MailTrackingRate_ID) begin
		raiserror('Cannot delete Mail Tracking Rate.  It is being referenced by an estimate.', 16, 1)
		return
	end
	else begin
		delete from VND_MailTrackingRate
		where VND_MailTrackingRate_ID = @VND_MailTrackingRate_ID
		return
	end
end

update e
	set
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_Estimate e join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
where ad.MailTracking_ID = @VND_MailTrackingRate_ID

update ad
	set
		MailTracking_ID = @new_MailTrackingRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_AssemDistribOptions ad
where ad.MailTracking_ID = @VND_MailTrackingRate_ID

if exists(select 1 from EST_AssemDistribOptions where MailTracking_ID = @VND_MailTrackingRate_ID) begin
	raiserror('Cannot delete Mail Tracking Rate.  It is being referenced by an estimate.', 16, 1)
	return
end
else begin
	delete from VND_MailTrackingRate
	where VND_MailTrackingRate_ID = @VND_MailTrackingRate_ID
	return
end
GO

GRANT  EXECUTE  ON [dbo].[VndMailTrackingRate_d_ByMailTrackingRateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
