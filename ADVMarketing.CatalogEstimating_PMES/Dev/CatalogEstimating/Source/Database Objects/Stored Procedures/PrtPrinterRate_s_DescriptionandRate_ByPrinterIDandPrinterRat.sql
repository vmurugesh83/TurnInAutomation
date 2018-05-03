IF OBJECT_ID('dbo.PrtPrinterRate_s_DescriptionandRate_ByPrinterIDandPrinterRateTypeID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PrtPrinterRate_s_DescriptionandRate_ByPrinterIDandPrinterRateTypeID'
	DROP PROCEDURE dbo.PrtPrinterRate_s_DescriptionandRate_ByPrinterIDandPrinterRateTypeID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PrtPrinterRate_s_DescriptionandRate_ByPrinterIDandPrinterRateTypeID') IS NOT NULL
		PRINT '***********Drop of dbo.PrtPrinterRate_s_DescriptionandRate_ByPrinterIDandPrinterRateTypeID FAILED.'
END
GO
PRINT 'Creating dbo.PrtPrinterRate_s_DescriptionandRate_ByPrinterIDandPrinterRateTypeID'
GO

create proc dbo.PrtPrinterRate_s_DescriptionandRate_ByPrinterIDandPrinterRateTypeID
/*
* PARAMETERS:
* VND_Printer_ID
* PRT_PrinterRateType_ID
*
* DESCRIPTION:
* Returns Printer Rates for the PrinterID and Printer Rate specified.
*
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
* 09/28/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Printer_ID bigint,
@PRT_PrinterRateType_ID int
as
select PRT_PrinterRate_ID, Description, Rate
from PRT_PrinterRate
where VND_Printer_ID = @VND_Printer_ID and PRT_PrinterRateType_ID = @PRT_PrinterRateType_ID
order by [Default] desc, Description


GO

GRANT  EXECUTE  ON [dbo].[PrtPrinterRate_s_DescriptionandRate_ByPrinterIDandPrinterRateTypeID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
