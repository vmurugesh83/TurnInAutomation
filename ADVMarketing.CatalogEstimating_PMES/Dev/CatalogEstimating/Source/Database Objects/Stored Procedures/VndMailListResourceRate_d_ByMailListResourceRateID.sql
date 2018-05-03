IF OBJECT_ID('dbo.VndMailListResourceRate_d_ByMailListResourceRateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailListResourceRate_d_ByMailListResourceRateID'
	DROP PROCEDURE dbo.VndMailListResourceRate_d_ByMailListResourceRateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailListResourceRate_d_ByMailListResourceRateID') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailListResourceRate_d_ByMailListResourceRateID FAILED.'
END
GO
PRINT 'Creating dbo.VndMailListResourceRate_d_ByMailListResourceRateID'
GO

CREATE PROC dbo.VndMailListResourceRate_d_ByMailListResourceRateID
@VND_MailListResourceRate_ID bigint,
@ModifiedBy varchar(50)
/*
* PARAMETERS:
* VND_MailListResourceRate_ID
*
* DESCRIPTION:
* Deletes a MailListResource record.  Updates any A&D records that may be affected.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   VND_MailListResourceRate       DELETE
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

declare @new_MailListResourceRate_ID bigint
select top 1 @new_MailListResourceRate_ID = new_ml.VND_MailListResourceRate_ID
from VND_MailListResourceRate orig_ml join VND_MailListResourceRate new_ml on orig_ml.VND_Vendor_ID = new_ml.VND_Vendor_ID
where orig_ml.VND_MailListResourceRate_ID = @VND_MailListResourceRate_ID and orig_ml.VND_MailListResourceRate_ID <> new_ml.VND_MailListResourceRate_ID
order by new_ml.EffectiveDate

--If there is not an earlier set of Mail House Rates for this Vendor, delete the record
if (@new_MailListResourceRate_ID is null) begin
	if exists(select 1 from EST_AssemDistribOptions where MailListResource_ID = @VND_MailListResourceRate_ID) begin
		raiserror('Cannot delete Mail List Resource Rate.  It is being referenced by an estimate.', 16, 1)
		return
	end
	else begin
		delete from VND_MailListResourceRate
		where VND_MailListResourceRate_ID = @VND_MailListResourceRate_ID
		return
	end
end

update e
	set
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_Estimate e join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
where ad.MailListResource_ID = @VND_MailListResourceRate_ID

update ad
	set
		MailListResource_ID = @new_MailListResourceRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_AssemDistribOptions ad
where ad.MailListResource_ID = @VND_MailListResourceRate_ID

if exists(select 1 from EST_AssemDistribOptions where MailListResource_ID = @VND_MailListResourceRate_ID) begin
	raiserror('Cannot delete Mail List Resource Rate.  It is being referenced by an estimate.', 16, 1)
	return
end
else begin
	delete from VND_MailListResourceRate
	where VND_MailListResourceRate_ID = @VND_MailListResourceRate_ID
	return
end
GO

GRANT  EXECUTE  ON [dbo].[VndMailListResourceRate_d_ByMailListResourceRateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
