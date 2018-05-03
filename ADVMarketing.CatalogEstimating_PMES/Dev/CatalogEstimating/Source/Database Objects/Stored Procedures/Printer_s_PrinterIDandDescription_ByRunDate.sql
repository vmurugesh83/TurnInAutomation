IF OBJECT_ID('dbo.Printer_s_PrinterIDandDescription_ByRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.Printer_s_PrinterIDandDescription_ByRunDate'
	DROP PROCEDURE dbo.Printer_s_PrinterIDandDescription_ByRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.Printer_s_PrinterIDandDescription_ByRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.Printer_s_PrinterIDandDescription_ByRunDate FAILED.'
END
GO
PRINT 'Creating dbo.Printer_s_PrinterIDandDescription_ByRunDate'
GO

create proc dbo.Printer_s_PrinterIDandDescription_ByRunDate
/*
* PARAMETERS:
* RunDate - Required
*
*
* DESCRIPTION:
*		Returns a list of Vendor Printer ID's and Vendor Descriptions for the specified RunDate
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
*	------------- 	--------        -------------------------------------------------
* 08/16/2007			BJS							Initial Creation 
* 08/23/2007			NLS							Added extra rates to the select
* 09/20/2007            BJS         Added VND_Printer_ID as an optional parameter
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Printer_ID bigint,
@RunDate datetime
as

select v.VND_Vendor_ID, max(p.VND_Printer_ID) VND_Printer_ID, max(v.Description) Description, max(p.polybagmessage) PolybagMessage, max(p.polybagmessagemakeready) PolybagMessageMakeready
from VND_Vendor v join VND_Printer p on v.VND_Vendor_ID = p.VND_Vendor_ID
	left join VND_Printer newer_p on v.VND_Vendor_ID = newer_p.VND_Vendor_ID and newer_p.EffectiveDate <= @RunDate and newer_p.EffectiveDate > p.EffectiveDate
where (v.Active = 1 or p.VND_Printer_ID = @VND_Printer_ID) and p.EffectiveDate <= @RunDate and newer_p.VND_Printer_ID is null
group by v.VND_Vendor_ID
order by v.Description
GO

GRANT  EXECUTE  ON [dbo].[Printer_s_PrinterIDandDescription_ByRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
