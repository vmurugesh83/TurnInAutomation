IF OBJECT_ID('dbo.PrtPrinterRate_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PrtPrinterRate_i'
	DROP PROCEDURE dbo.PrtPrinterRate_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PrtPrinterRate_i') IS NOT NULL
		PRINT '***********Drop of dbo.PrtPrinterRate_i FAILED.'
END
GO
PRINT 'Creating dbo.PrtPrinterRate_i'
GO

CREATE PROC dbo.PrtPrinterRate_i
@PRT_PrinterRate_ID bigint output,
@VND_Printer_ID bigint,
@PRT_PrinterRateType_ID int,
@Rate money,
@Description varchar(35),
@Default bit,
@CreatedBy varchar(50)
/*
* PARAMETERS:
* PRT_PrinterRate_ID
* VND_Printer_ID
* PRT_PrinterRateType_ID
* Rate
* Description
* Default
* CreatedBy
*
* DESCRIPTION:
* Creates a PrinterRate record.  If it is the default, set the old default to "not-default".
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   PRT_PrinterRate                INSERT
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
			ModifiedBy = @CreatedBy,
			ModifiedDate = getdate()
	where VND_Printer_ID = @VND_Printer_ID
		and PRT_PrinterRateType_ID = @PRT_PrinterRateType_ID
		and [Default] = 1
end

insert PRT_PrinterRate(VND_Printer_ID, PRT_PrinterRateType_ID, Rate, Description, [Default], CreatedBy, CreatedDate)
values(@VND_Printer_ID, @PRT_PrinterRateType_ID, @Rate, @Description, @Default, @CreatedBy, getdate())
set @PRT_PrinterRate_ID = @@identity
GO

GRANT  EXECUTE  ON [dbo].[PrtPrinterRate_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
