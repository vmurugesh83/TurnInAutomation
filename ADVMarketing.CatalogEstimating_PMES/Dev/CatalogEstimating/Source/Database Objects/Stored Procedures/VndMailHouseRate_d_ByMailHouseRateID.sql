IF OBJECT_ID('dbo.VndMailHouseRate_d_ByMailHouseRateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailHouseRate_d_ByMailHouseRateID'
	DROP PROCEDURE dbo.VndMailHouseRate_d_ByMailHouseRateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailHouseRate_d_ByMailHouseRateID') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailHouseRate_d_ByMailHouseRateID FAILED.'
END
GO
PRINT 'Creating dbo.VndMailHouseRate_d_ByMailHouseRateID'
GO

CREATE PROC dbo.VndMailHouseRate_d_ByMailHouseRateID
@VND_MailHouseRate_ID bigint,
@ModifiedBy varchar(50)
/*
* PARAMETERS:
* VND_MailHouseRate_ID
*
* DESCRIPTION:
* Deletes a MailHouse record.  Updates any A&D records that may be affected.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   VND_MailHouseRate              DELETE
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

declare @new_MailHouseRate_ID bigint
select top 1 @new_MailHouseRate_ID = new_mh.VND_MailHouseRate_ID
from VND_MailHouseRate orig_mh join VND_MailHouseRate new_mh on orig_mh.VND_Vendor_ID = new_mh.VND_Vendor_ID
where orig_mh.VND_MailHouseRate_ID = @VND_MailHouseRate_ID and orig_mh.VND_MailHouseRate_ID <> new_mh.VND_MailHouseRate_ID
order by new_mh.EffectiveDate

--If there is not an earlier set of Mail House Rates for this Vendor, delete the record
if (@new_MailHouseRate_ID is null) begin
	if exists(select 1 from EST_AssemDistribOptions where MailHouse_ID = @VND_MailHouseRate_ID) begin
		raiserror('Cannot delete Mail House Rate.  It is being referenced by an estimate.', 16, 1)
		return
	end
	else begin
		delete from VND_MailHouseRate
		where VND_MailHouseRate_ID = @VND_MailHouseRate_ID
		return
	end
end

update e
	set
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_Estimate e join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
where ad.MailHouse_ID = @VND_MailHouseRate_ID

update ad
	set
		MailHouse_ID = @new_MailHouseRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_AssemDistribOptions ad
where ad.MailHouse_ID = @VND_MailHouseRate_ID

if exists(select 1 from EST_AssemDistribOptions where MailHouse_ID = @VND_MailHouseRate_ID) begin
	raiserror('Cannot delete Mail House Rate.  It is being referenced by an estimate.', 16, 1)
	return
end
else begin
	delete from VND_MailHouseRate
	where VND_MailHouseRate_ID = @VND_MailHouseRate_ID
	return
end
GO

GRANT  EXECUTE  ON [dbo].[VndMailHouseRate_d_ByMailHouseRateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
