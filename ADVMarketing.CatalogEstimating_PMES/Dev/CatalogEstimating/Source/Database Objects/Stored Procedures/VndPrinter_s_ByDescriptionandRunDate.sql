IF OBJECT_ID('dbo.VndPrinter_s_ByDescriptionandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndPrinter_s_ByDescriptionandRunDate'
	DROP PROCEDURE dbo.VndPrinter_s_ByDescriptionandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndPrinter_s_ByDescriptionandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.VndPrinter_s_ByDescriptionandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.VndPrinter_s_ByDescriptionandRunDate'
GO

create proc dbo.VndPrinter_s_ByDescriptionandRunDate
/*
* PARAMETERS:
* Description - Required
* RunDate - Required
*
*
* DESCRIPTION:
*		Returns a Printer Record for the given Vendor Description and Run Date.
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		VND_Vendor      READ
*		VND_Printer     READ
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
*   09/04/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Description varchar(35),
@RunDate datetime
as

select p.*
from VND_Vendor v join VND_Printer p on v.VND_Vendor_ID = p.VND_Vendor_ID
	left join VND_Printer newer_p on v.VND_Vendor_ID = newer_p.VND_Vendor_ID and newer_p.EffectiveDate <= @RunDate and newer_p.EffectiveDate > p.EffectiveDate
where v.Description = @Description and p.EffectiveDate <= @RunDate and newer_p.VND_Printer_ID is null
GO

GRANT  EXECUTE  ON [dbo].[VndPrinter_s_ByDescriptionandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
