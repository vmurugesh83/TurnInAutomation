IF OBJECT_ID('dbo.VndMailListResourceRate_s_ByDescriptionandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailListResourceRate_s_ByDescriptionandRunDate'
	DROP PROCEDURE dbo.VndMailListResourceRate_s_ByDescriptionandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailListResourceRate_s_ByDescriptionandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailListResourceRate_s_ByDescriptionandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.VndMailListResourceRate_s_ByDescriptionandRunDate'
GO

create proc dbo.VndMailListResourceRate_s_ByDescriptionandRunDate
/*
* PARAMETERS:
* Description - Required
* RunDate - Required
*
*
* DESCRIPTION:
*		Returns a MailListResourceRate Record for the given Vendor Description and Run Date.
*
*
* TABLES:
*		Table Name			       Access
*		==========			       ======
*		VND_Vendor                 READ
*		VND_MailListResourceRate   READ
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

select ml.*
from VND_Vendor v join VND_MailListResourceRate ml on v.VND_Vendor_ID = ml.VND_Vendor_ID
	left join VND_MailListResourceRate newer_ml on v.VND_Vendor_ID = newer_ml.VND_Vendor_ID and newer_ml.EffectiveDate <= @RunDate and newer_ml.EffectiveDate > ml.EffectiveDate
where v.Description = @Description and ml.EffectiveDate <= @RunDate and newer_ml.VND_MailListResourceRate_ID is null
GO

GRANT  EXECUTE  ON [dbo].[VndMailListResourceRate_s_ByDescriptionandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
