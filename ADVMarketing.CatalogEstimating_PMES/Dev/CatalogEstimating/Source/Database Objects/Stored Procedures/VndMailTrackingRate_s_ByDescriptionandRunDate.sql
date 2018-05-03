IF OBJECT_ID('dbo.VndMailTrackingRate_s_ByDescriptionandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailTrackingRate_s_ByDescriptionandRunDate'
	DROP PROCEDURE dbo.VndMailTrackingRate_s_ByDescriptionandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailTrackingRate_s_ByDescriptionandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailTrackingRate_s_ByDescriptionandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.VndMailTrackingRate_s_ByDescriptionandRunDate'
GO

create proc dbo.VndMailTrackingRate_s_ByDescriptionandRunDate
/*
* PARAMETERS:
* Description - Required
* RunDate - Required
*
*
* DESCRIPTION:
*		Returns a MailTrackingRate Record for the given Vendor Description and Run Date.
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		VND_Vendor          READ
*		VND_MailTrackingRate   READ
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
*	Date            Who             Comments
*	-------------   --------        -------------------------------------------------
*   09/05/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Description varchar(35),
@RunDate datetime
as

select mt.*
from VND_Vendor v join VND_MailTrackingRate mt on v.VND_Vendor_ID = mt.VND_Vendor_ID
	left join VND_MailTrackingRate newer_mt on v.VND_Vendor_ID = newer_mt.VND_Vendor_ID and newer_mt.EffectiveDate <= @RunDate and newer_mt.EffectiveDate > mt.EffectiveDate
where v.Description = @Description and mt.EffectiveDate <= @RunDate and newer_mt.VND_MailTrackingRate_ID is null
GO

GRANT  EXECUTE  ON [dbo].[VndMailTrackingRate_s_ByDescriptionandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
