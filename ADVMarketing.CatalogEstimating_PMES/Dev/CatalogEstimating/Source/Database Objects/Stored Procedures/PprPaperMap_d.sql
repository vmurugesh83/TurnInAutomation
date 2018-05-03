IF OBJECT_ID('dbo.PprPaperMap_d') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperMap_d'
	DROP PROCEDURE dbo.PprPaperMap_d
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperMap_d') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperMap_d FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperMap_d'
GO

create proc dbo.PprPaperMap_d
/*
* PARAMETERS:
* PPR_Paper_Map_ID - The Paper Map that will be deleted.
*
*
* DESCRIPTION:
* Updates any Component records, removing references to the paper map.
* Deletes the Paper Map record.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*	EST_Component		READ/UPDATE
*   PPR_Paper_Map       READ/DELETE
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
@PPR_Paper_Map_ID BIGINT,
@ModifiedBy varchar(50)
as

declare @new_Paper_Map_ID bigint, @PaperGrade_ID int, @PaperWeight_ID int

select top 1 @new_Paper_Map_ID = new_pm.PPR_Paper_Map_ID, @PaperGrade_ID = new_pm.PPR_PaperGrade_ID,
	@PaperWeight_ID = new_pm.PPR_PaperWeight_ID
from PPR_Paper_Map orig_pm join PPR_Paper_Map new_pm on orig_pm.VND_Paper_ID = new_pm.VND_Paper_ID
	and new_pm.[Default] = 1
where orig_pm.PPR_Paper_Map_ID = @PPR_Paper_Map_ID and orig_pm.PPR_Paper_Map_ID <> new_pm.PPR_Paper_Map_ID

--If there is no default paper map.  This must be the default and there should no longer be any components referencing it.
--It can be deleted.
if (@new_Paper_Map_ID is null) begin
	delete from PPR_Paper_Map
	where PPR_Paper_Map_ID = @PPR_Paper_Map_ID
	return
end

update e
	set
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_Estimate e join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
where c.Paper_Map_ID = @PPR_Paper_Map_ID

update EST_Component
	set
		PaperGrade_ID = @PaperGrade_ID,
		PaperWeight_ID = @PaperWeight_ID,
		Paper_Map_ID = @new_Paper_Map_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where Paper_Map_ID = @PPR_Paper_Map_ID

delete from PPR_Paper_Map
where PPR_Paper_Map_ID = @PPR_Paper_Map_ID
GO

GRANT  EXECUTE  ON [dbo].[PprPaperMap_d]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
