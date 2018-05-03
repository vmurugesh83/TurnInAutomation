IF OBJECT_ID('dbo.Paper_u_Component') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.Paper_u_Component'
	DROP PROCEDURE dbo.Paper_u_Component
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.Paper_u_Component') IS NOT NULL
		PRINT '***********Drop of dbo.Paper_u_Component FAILED.'
END
GO
PRINT 'Creating dbo.Paper_u_Component'
GO

CREATE PROC dbo.Paper_u_Component
/*
* PARAMETERS:
* ModifiedBy - The current user
*
*
* DESCRIPTION:
*		Updates any Component and Polybag records with new paper and paper map rate references.
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
@ModifiedBy varchar(50)
as

update e
	set
		e.ModifiedBy = @ModifiedBy,
		e.ModifiedDate = getdate()
from EST_Estimate e join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join VND_Paper old_p on c.Paper_ID = old_p.VND_Paper_ID
	join VND_Vendor v on old_p.VND_Vendor_ID = v.VND_Vendor_ID
	join VND_Paper new_p on v.VND_Vendor_ID = new_p.VND_Vendor_ID
		and new_p.EffectiveDate > old_p.EffectiveDate
		and e.RunDate >= new_p.EffectiveDate
	left join VND_Paper next_p on v.VND_Vendor_ID = next_p.VND_Vendor_ID
		and next_p.EffectiveDate > new_p.EffectiveDate
		and e.RunDate >= next_p.EffectiveDate
where next_p.VND_Paper_ID is null

update c
	set
		Paper_ID = new_p.VND_Paper_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_Estimate e join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join VND_Paper old_p on c.Paper_ID = old_p.VND_Paper_ID
	join VND_Vendor v on old_p.VND_Vendor_ID = v.VND_Vendor_ID
	join VND_Paper new_p on v.VND_Vendor_ID = new_p.VND_Vendor_ID
		and new_p.EffectiveDate > old_p.EffectiveDate
		and e.RunDate >= new_p.EffectiveDate
	left join VND_Paper next_p on v.VND_Vendor_ID = next_p.VND_Vendor_ID
		and next_p.EffectiveDate > new_p.EffectiveDate
		and e.RunDate >= next_p.EffectiveDate
where next_p.VND_Paper_ID is null

/* Update Paper Map if a new Paper Map exists with matching description */
update c
	set
		c.Paper_Map_ID = new_pm.PPR_Paper_Map_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join VND_Paper p on c.Paper_ID = p.VND_Paper_ID
	join PPR_Paper_Map orig_pm on c.Paper_Map_ID = orig_pm.PPR_Paper_Map_ID and orig_pm.VND_Paper_ID <> p.VND_Paper_ID
	join PPR_Paper_Map new_pm on p.VND_Paper_ID = new_pm.VND_Paper_ID
		and orig_pm.PPR_PaperGrade_ID = new_pm.PPR_PaperGrade_ID
		and orig_pm.PPR_PaperWeight_ID = new_pm.PPR_PaperWeight_ID
		and orig_pm.Description = new_pm.Description

/* Update Paper Map if a new Paper Map exists w/o match description (set to default)*/
update c
	set
		c.PaperGrade_ID = new_pm.PPR_PaperGrade_ID,
		c.PaperWeight_ID = new_pm.PPR_PaperWeight_ID,
		c.Paper_Map_ID = new_pm.PPR_Paper_Map_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join VND_Paper p on c.Paper_ID = p.VND_Paper_ID
	join PPR_Paper_Map orig_pm on c.Paper_Map_ID = orig_pm.PPR_Paper_Map_ID and orig_pm.VND_Paper_ID <> p.VND_Paper_ID
	join PPR_Paper_Map new_pm on p.VND_Paper_ID = new_pm.VND_Paper_ID
		and new_pm.[Default] = 1

if exists(select 1
	from EST_Component c join VND_Paper p on c.Paper_ID = p.VND_Paper_ID
		join PPR_Paper_Map orig_pm on c.Paper_Map_ID = orig_pm.PPR_Paper_Map_ID and orig_pm.VND_Paper_ID <> p.VND_Paper_ID) begin

	raiserror('New Paper Effective Date missing a Paper Map record.', 16, 1)
	return
end
GO

GRANT  EXECUTE  ON [dbo].[Paper_u_Component]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
