IF OBJECT_ID('dbo.Printer_s_PrinterID_ByOldPrinterIDandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.Printer_s_PrinterID_ByOldPrinterIDandRunDate'
	DROP PROCEDURE dbo.Printer_s_PrinterID_ByOldPrinterIDandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.Printer_s_PrinterID_ByOldPrinterIDandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.Printer_s_PrinterID_ByOldPrinterIDandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.Printer_s_PrinterID_ByOldPrinterIDandRunDate'
GO

CREATE proc dbo.Printer_s_PrinterID_ByOldPrinterIDandRunDate
/*
* PARAMETERS:
* VND_Printer_ID - required
* RunDate - required
*
* DESCRIPTION:
*		Determines the parent vendor record of VND_Printer_ID.  Returns a printer record with the same parent vendor for the specified run date
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   VND_Printer         READ
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
* 08/16/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Printer_ID bigint,
@RunDate datetime
as

declare @VND_Vendor_ID bigint

select @VND_Vendor_ID = VND_Vendor_ID
from VND_Printer
where VND_Printer_ID = @VND_Printer_ID

select top 1 p.*
from VND_Printer p
where p.VND_Vendor_ID = @VND_Vendor_ID and p.EffectiveDate <= @RunDate
order by p.EffectiveDate desc
GO

GRANT  EXECUTE  ON [dbo].[Printer_s_PrinterID_ByOldPrinterIDandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
