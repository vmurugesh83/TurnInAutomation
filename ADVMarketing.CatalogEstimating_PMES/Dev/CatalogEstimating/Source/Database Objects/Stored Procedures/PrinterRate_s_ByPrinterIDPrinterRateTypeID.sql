IF OBJECT_ID('dbo.PrinterRate_s_ByPrinterIDPrinterRateTypeID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PrinterRate_s_ByPrinterIDPrinterRateTypeID'
	DROP PROCEDURE dbo.PrinterRate_s_ByPrinterIDPrinterRateTypeID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PrinterRate_s_ByPrinterIDPrinterRateTypeID') IS NOT NULL
		PRINT '***********Drop of dbo.PrinterRate_s_ByPrinterIDPrinterRateTypeID FAILED.'
END
GO
PRINT 'Creating dbo.PrinterRate_s_ByPrinterIDPrinterRateTypeID'
GO

CREATE PROC dbo.PrinterRate_s_ByPrinterIDPrinterRateTypeID
@VND_Printer_ID bigint,
@PRT_PrinterRateType_ID int
/*
* PARAMETERS:
* VND_Vendor_ID - required
* PRT_PrinterRateType_ID required
*
* DESCRIPTION:
* Returns all printer rates for the printer and rate type.
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   PRT_PrinterRate                READ
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
as

select pr.*
from PRT_PrinterRate pr
where pr.VND_Printer_ID = @VND_Printer_ID and pr.PRT_PrinterRateType_ID = @PRT_PrinterRateType_ID
order by pr.[default] desc
GO

GRANT  EXECUTE  ON [dbo].[PrinterRate_s_ByPrinterIDPrinterRateTypeID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO