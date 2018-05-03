IF OBJECT_ID('dbo.VndMailHouseRate_s_ByDescriptionandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailHouseRate_s_ByDescriptionandRunDate'
	DROP PROCEDURE dbo.VndMailHouseRate_s_ByDescriptionandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailHouseRate_s_ByDescriptionandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailHouseRate_s_ByDescriptionandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.VndMailHouseRate_s_ByDescriptionandRunDate'
GO

create proc dbo.VndMailHouseRate_s_ByDescriptionandRunDate
/*
* PARAMETERS:
* Description - Required
* RunDate - Required
*
*
* DESCRIPTION:
*		Returns a MailHouseRate Record for the given Vendor Description and Run Date.
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		VND_Vendor          READ
*		VND_MailHouseRate   READ
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

select mh.*
from VND_Vendor v join VND_MailHouseRate mh on v.VND_Vendor_ID = mh.VND_Vendor_ID
	left join VND_MailHouseRate newer_mh on v.VND_Vendor_ID = newer_mh.VND_Vendor_ID and newer_mh.EffectiveDate <= @RunDate and newer_mh.EffectiveDate > mh.EffectiveDate
where v.Description = @Description and mh.EffectiveDate <= @RunDate and newer_mh.VND_MailHouseRate_ID is null
GO

GRANT  EXECUTE  ON [dbo].[VndMailHouseRate_s_ByDescriptionandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
