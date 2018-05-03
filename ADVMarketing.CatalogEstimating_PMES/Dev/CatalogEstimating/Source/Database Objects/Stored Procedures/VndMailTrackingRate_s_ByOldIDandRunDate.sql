IF OBJECT_ID('dbo.VndMailTrackingRate_s_ByOldIDandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailTrackingRate_s_ByOldIDandRunDate'
	DROP PROCEDURE dbo.VndMailTrackingRate_s_ByOldIDandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailTrackingRate_s_ByOldIDandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailTrackingRate_s_ByOldIDandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.VndMailTrackingRate_s_ByOldIDandRunDate'
GO

CREATE proc dbo.VndMailTrackingRate_s_ByOldIDandRunDate
/*
* PARAMETERS:
* VND_MailTrackingRate_ID - required
* RunDate              - required
*
* DESCRIPTION:
*		Determines the parent vendor record of VND_MailTrackingRate_ID.  Returns a MailTracking record with the same parent vendor for the specified run date
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   VND_MailTrackingRate   READ
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
* 11/02/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_MailTrackingRate_ID bigint,
@RunDate datetime
as

declare @VND_Vendor_ID bigint

select @VND_Vendor_ID = VND_Vendor_ID
from VND_MailTrackingRate
where VND_MailTrackingRate_ID = @VND_MailTrackingRate_ID

select top 1 mt.*
from VND_MailTrackingRate mt
where mt.VND_Vendor_ID = @VND_Vendor_ID and mt.EffectiveDate <= @RunDate
order by mt.EffectiveDate desc
GO

GRANT  EXECUTE  ON [dbo].[VndMailTrackingRate_s_ByOldIDandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
