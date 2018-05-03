IF OBJECT_ID('dbo.Paper_u_Component_ByPaperID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.Paper_u_Component_ByPaperID'
	DROP PROCEDURE dbo.Paper_u_Component_ByPaperID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.Paper_u_Component_ByPaperID') IS NOT NULL
		PRINT '***********Drop of dbo.Paper_u_Component_ByPaperID FAILED.'
END
GO
PRINT 'Creating dbo.Paper_u_Component_ByPaperID'
GO

CREATE PROC dbo.Paper_u_Component_ByPaperID
/*
* PARAMETERS:
* VND_Paper_ID - The Paper that will be deleted.
*
*
* DESCRIPTION:
* Updates any Component records, removing references to the paper record and the corresponding paper maps
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate		UPDATE
*	EST_Component		READ/UPDATE
*   VND_Paper           READ
*   VND_Vendor			READ
*   PPR_Paper_Map       READ
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
@VND_Paper_ID bigint,
@ModifiedBy varchar(50)
as

declare @new_Paper_ID bigint
select top 1 @new_Paper_ID = new_p.VND_Paper_ID
from VND_Paper orig_p join VND_Vendor v on orig_p.VND_Vendor_ID = v.VND_Vendor_ID
	join VND_Paper new_p on v.VND_Vendor_ID = new_p.VND_Vendor_ID and new_p.EffectiveDate < orig_p.EffectiveDate
where orig_p.VND_Paper_ID = @VND_Paper_ID
order by new_p.EffectiveDate desc

if (@new_Paper_ID is null) begin
	raiserror('Cannot delete paper effective date record.  No earlier paper effective date records can be found for vendor.', 16, 1)
	return
end

update e
	set
		e.ModifiedBy = @ModifiedBy,
		e.ModifiedDate = getdate()
from EST_Estimate e join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
where c.Paper_ID = @VND_Paper_ID

/* Update Paper Map if another Paper Map exists with matching description */
update c
	set
		c.PaperGrade_ID = new_pm.PPR_PaperGrade_ID,
		c.PaperWeight_ID = new_pm.PPR_PaperWeight_ID,
		c.Paper_Map_ID = new_pm.PPR_Paper_Map_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join PPR_Paper_Map orig_pm on c.Paper_Map_ID = orig_pm.PPR_Paper_Map_ID
	join PPR_Paper_Map new_pm on new_pm.VND_Paper_ID = @new_Paper_ID
		and orig_pm.PPR_PaperGrade_ID = new_pm.PPR_PaperGrade_ID
		and orig_pm.PPR_PaperWeight_ID = new_pm.PPR_PaperWeight_ID
		and orig_pm.Description = new_pm.Description
where c.Paper_ID = @VND_Paper_ID

/* Update Paper Map if a new Paper Map exists w/o match description (set to default)*/
update c
	set
		c.PaperGrade_ID = new_pm.PPR_PaperGrade_ID,
		c.PaperWeight_ID = new_pm.PPR_PaperWeight_ID,
		c.Paper_Map_ID = new_pm.PPR_Paper_Map_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join PPR_Paper_Map orig_pm on c.Paper_Map_ID = orig_pm.PPR_Paper_Map_ID
	join PPR_Paper_Map new_pm on new_pm.VND_Paper_ID = @new_Paper_ID
		and new_pm.[Default] = 1
where c.Paper_ID = @VND_Paper_ID

if exists(select 1
	from EST_Component c join PPR_Paper_Map pm on c.Paper_Map_ID = pm.PPR_Paper_Map_ID
	where pm.VND_Paper_ID = @VND_Paper_ID) begin

	raiserror('Old Paper Effective Date is missing a Paper Map record.', 16, 1)
	return
end

--Update PaperID on Component records
update EST_Component
	set
		Paper_ID = @new_Paper_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where Paper_ID = @VND_Paper_ID
GO

GRANT  EXECUTE  ON [dbo].[Paper_u_Component_ByPaperID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

