IF OBJECT_ID('dbo.PrtPrinterRate_d_ByPrinterRateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PrtPrinterRate_d_ByPrinterRateID'
	DROP PROCEDURE dbo.PrtPrinterRate_d_ByPrinterRateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PrtPrinterRate_d_ByPrinterRateID') IS NOT NULL
		PRINT '***********Drop of dbo.PrtPrinterRate_d_ByPrinterRateID FAILED.'
END
GO
PRINT 'Creating dbo.PrtPrinterRate_d_ByPrinterRateID'
GO

create proc dbo.PrtPrinterRate_d_ByPrinterRateID
/*
* PARAMETERS:
* PRT_PrinterRate_ID - The Printer Rate that will be deleted.
*
*
* DESCRIPTION:
* Updates any Component and Polybag records, removing references to the printer rate.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*	EST_Component		READ/UPDATE
*   EST_PolybagGroup	READ/UPDATE
*   VND_PrinterRate     READ/DELETE
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
* 09/28/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PRT_PrinterRate_ID BIGINT,
@ModifiedBy varchar(50)
as

declare @new_PrinterRate_ID bigint, @PrinterRateType_ID int

select top 1 @new_PrinterRate_ID = new_pr.PRT_PrinterRate_ID, @PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID
from PRT_PrinterRate orig_pr join PRT_PrinterRate new_pr on orig_pr.VND_Printer_ID = new_pr.VND_Printer_ID
	and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and new_pr.[Default] = 1
where orig_pr.PRT_PrinterRate_ID = @PRT_PrinterRate_ID and orig_pr.PRT_PrinterRate_ID <> new_pr.PRT_PrinterRate_ID

--If there is no default printer rate.  This must be the default and there should no longer be any components or polybags referencing it.
--It can be deleted.
if (@new_PrinterRate_ID is null) begin
	delete from PRT_PrinterRate
	where PRT_PrinterRate_ID = @PRT_PrinterRate_ID
	return
end

update e
	set
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_Estimate e join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
where c.PlateCost_ID = @PRT_PrinterRate_ID

update EST_Component
	set
		PlateCost_ID = @new_PrinterRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where PlateCost_ID = @PRT_PrinterRate_ID

update e
	set
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_Estimate e join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
where c.DigitalHandlenPrepare_ID = @PRT_PrinterRate_ID

update EST_Component
	set
		DigitalHandlenPrepare_ID = @new_PrinterRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where DigitalHandlenPrepare_ID = @PRT_PrinterRate_ID

update e
	set
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_Estimate e join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
where c.StitchIn_ID = @PRT_PrinterRate_ID

update EST_Component
	set
		StitchIn_ID = @new_PrinterRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where StitchIn_ID = @PRT_PrinterRate_ID

update e
	set
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_Estimate e join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
where c.BlowIn_ID = @PRT_PrinterRate_ID

update EST_Component
	set
		BlowIn_ID = @new_PrinterRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where BlowIn_ID = @PRT_PrinterRate_ID

update e
	set
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_Estimate e join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
where c.Onsert_ID = @PRT_PrinterRate_ID

update EST_Component
	set
		Onsert_ID = @new_PrinterRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where Onsert_ID = @PRT_PrinterRate_ID

update e
	set
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_Estimate e join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
where c.StitcherMakeready_ID = @PRT_PrinterRate_ID

update EST_Component
	set
		StitcherMakeready_ID = @new_PrinterRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where StitcherMakeready_ID = @PRT_PrinterRate_ID

update e
	set
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_Estimate e join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
where c.PressMakeready_ID = @PRT_PrinterRate_ID

update EST_Component
	set
		PressMakeready_ID = @new_PrinterRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where PressMakeready_ID = @PRT_PrinterRate_ID

update EST_PolybagGroup
	set
		PRT_BagRate_ID = @new_PrinterRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where PRT_BagRate_ID = @PRT_PrinterRate_ID

update EST_PolybagGroup
	set
		PRT_BagMakereadyRate_ID = @new_PrinterRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where PRT_BagMakereadyRate_ID = @PRT_PrinterRate_ID

delete from PRT_PrinterRate
where PRT_PrinterRate_ID = @PRT_PrinterRate_ID
GO

GRANT  EXECUTE  ON [dbo].[PrtPrinterRate_d_ByPrinterRateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
