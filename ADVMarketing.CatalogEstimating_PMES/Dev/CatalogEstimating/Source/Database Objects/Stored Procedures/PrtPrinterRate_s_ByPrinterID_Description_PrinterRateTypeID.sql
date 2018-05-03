IF OBJECT_ID('dbo.PrtPrinterRate_s_ByPrinterID_Description_PrinterRateTypeID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PrtPrinterRate_s_ByPrinterID_Description_PrinterRateTypeID'
	DROP PROCEDURE dbo.PrtPrinterRate_s_ByPrinterID_Description_PrinterRateTypeID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PrtPrinterRate_s_ByPrinterID_Description_PrinterRateTypeID') IS NOT NULL
		PRINT '***********Drop of dbo.PrtPrinterRate_s_ByPrinterID_Description_PrinterRateTypeID FAILED.'
END
GO
PRINT 'Creating dbo.PrtPrinterRate_s_ByPrinterID_Description_PrinterRateTypeID'
GO

create proc dbo.PrtPrinterRate_s_ByPrinterID_Description_PrinterRateTypeID
/*
* PARAMETERS:
* VND_Printer_ID
* Description
* PRT_PrinterRateType_ID
*
* DESCRIPTION:
* Returns a printer rate matching the criteria.  If a match cannot be found on description the default rate will be returned.
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
@Description varchar(35),
@PRT_PrinterRateType_ID int
as

--Try to find an exact match
if exists(select 1 from PRT_PrinterRate where VND_Printer_ID = @VND_Printer_ID and PRT_PrinterRateType_ID = @PRT_PrinterRateType_ID and Description = @Description) begin
	select * from PRT_PrinterRate
	where VND_Printer_ID = @VND_Printer_ID and PRT_PrinterRateType_ID = @PRT_PrinterRateType_ID
		and Description = @Description
end
--If no exact match can be found, return the default rate
else begin
	select * from PRT_PrinterRate
	where VND_Printer_ID = @VND_Printer_ID and PRT_PrinterRateType_ID = @PRT_PrinterRateType_ID
		and [Default] = 1
end
GO

GRANT  EXECUTE  ON [dbo].[PrtPrinterRate_s_ByPrinterID_Description_PrinterRateTypeID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
