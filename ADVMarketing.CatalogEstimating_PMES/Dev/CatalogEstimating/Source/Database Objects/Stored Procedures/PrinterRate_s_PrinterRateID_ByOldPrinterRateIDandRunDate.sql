IF OBJECT_ID('dbo.PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate'
	DROP PROCEDURE dbo.PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate'
GO

CREATE proc dbo.PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate
/*
* PARAMETERS:
* PRT_PrinterRate_ID - required
* RunDate - required
*
* DESCRIPTION:
*		Determines the parent vendor record of PRT_PrinterRate_ID.  Returns a printer rate record with the same parent vendor and rate type
*   for the specified run date
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   VND_Printer         READ
*   PRT_PrinterRate     READ
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
@PRT_PrinterRate_ID bigint,
@RunDate datetime
as

declare @VND_Vendor_ID bigint, @VND_Printer_ID bigint, @PrinterRateDesc varchar(35)

-- Identify the VendorID and the original Printer Rate description
select @VND_Vendor_ID = VND_Vendor_ID, @PrinterRateDesc = pr.Description
from VND_Printer p join PRT_PrinterRate pr on p.VND_Printer_ID = pr.VND_Printer_ID
where pr.PRT_PrinterRate_ID = @PRT_PrinterRate_ID

-- Find the new PrinterID
select top 1 @VND_Printer_ID = p.VND_Printer_ID
from VND_Printer p
where p.VND_Vendor_ID = @VND_Vendor_ID and p.EffectiveDate <= @RunDate
order by p.EffectiveDate desc

-- If the original Printer Rate is still available return it
if exists(select 1 from PRT_PrinterRate where VND_Printer_ID = @VND_Printer_ID and PRT_PrinterRate_ID = @PRT_PrinterRate_ID) begin
	select * from PRT_PrinterRate where PRT_PrinterRate_ID = @PRT_PrinterRate_ID
end
-- Otherwise try to return a printer rate with an identical description
else if exists(select 1 from PRT_PrinterRate where VND_Printer_ID = @VND_Printer_ID and Description = @PrinterRateDesc) begin
	select * from PRT_PrinterRate where VND_Printer_ID = @VND_Printer_ID and Description = @PrinterRateDesc
end
-- The last resort is to try to return the default rate
else begin
	select * from PRT_PrinterRate where VND_Printer_ID = @VND_Printer_ID and [default] = 1
end

GO

GRANT  EXECUTE  ON [dbo].[PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
