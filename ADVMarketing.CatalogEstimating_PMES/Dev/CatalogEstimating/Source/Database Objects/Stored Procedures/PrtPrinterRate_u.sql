IF OBJECT_ID('dbo.PrtPrinterRate_u') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PrtPrinterRate_u'
	DROP PROCEDURE dbo.PrtPrinterRate_u
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PrtPrinterRate_u') IS NOT NULL
		PRINT '***********Drop of dbo.PrtPrinterRate_u FAILED.'
END
GO
PRINT 'Creating dbo.PrtPrinterRate_u'
GO

CREATE PROC dbo.PrtPrinterRate_u
@PRT_PrinterRate_ID bigint,
@VND_Printer_ID bigint,
@PRT_PrinterRateType_ID int,
@Rate money,
@Description varchar(35),
@Default bit,
@ModifiedBy varchar(50)
/*
* PARAMETERS:
* PRT_PrinterRate_ID
* VND_Printer_ID
* PRT_PrinterRateType_ID
* Rate
* Description
* Default
* ModifiedBy
*
* DESCRIPTION:
* Updates a PrinterRate record.  If it is the default, set the old default to "not-default".
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   PRT_PrinterRate                UPDATE
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
as

if (@Default = 1) begin
	update PRT_PrinterRate
		set
			[Default] = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedDate = getdate()
	where PRT_PrinterRate_ID <> @PRT_PrinterRate_ID
		and VND_Printer_ID = @VND_Printer_ID
		and PRT_PrinterRateType_ID = @PRT_PrinterRateType_ID
		and [Default] = 1
end

update PRT_PrinterRate
	set
		Rate = @Rate,
		Description = @Description,
		[Default] = @Default,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where PRT_PrinterRate_ID = @PRT_PrinterRate_ID
GO

GRANT  EXECUTE  ON [dbo].[PrtPrinterRate_u]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
