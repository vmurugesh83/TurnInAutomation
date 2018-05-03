IF OBJECT_ID('dbo.Printer_u_ComponentandPolybag') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.Printer_u_ComponentandPolybag'
	DROP PROCEDURE dbo.Printer_u_ComponentandPolybag
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.Printer_u_ComponentandPolybag') IS NOT NULL
		PRINT '***********Drop of dbo.Printer_u_ComponentandPolybag FAILED.'
END
GO
PRINT 'Creating dbo.Printer_u_ComponentandPolybag'
GO

CREATE PROC dbo.Printer_u_ComponentandPolybag
/*
* PARAMETERS:
* ModifiedBy - The current user
*
*
* DESCRIPTION:
*		Updates any Component and Polybag records with new printer and printer rate references.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate		UPDATE
*	EST_Component		READ/UPDATE
*   EST_PolybagGroup	READ/UPDATE
*   VND_Printer			READ
*   VND_Vendor			READ
*   VND_PrinterRate     READ
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
* 09/27/2007      BJS             Initial Creation 
* 11/01/2007      JRH             Incorporate AssemblyVendor_ID into update of Est_Component.
*                                 It affects AssemblyVendor_ID, StitchIn_ID, BlowIn_ID, and Onsert_ID
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
	join VND_Printer old_p on c.Printer_ID = old_p.VND_Printer_ID
	join VND_Vendor v on old_p.VND_Vendor_ID = v.VND_Vendor_ID
	join VND_Printer new_p on v.VND_Vendor_ID = new_p.VND_Vendor_ID
		and new_p.EffectiveDate > old_p.EffectiveDate
		and e.RunDate >= new_p.EffectiveDate
	left join VND_Printer next_p on v.VND_Vendor_ID = next_p.VND_Vendor_ID
		and next_p.EffectiveDate > new_p.EffectiveDate
		and e.RunDate >= next_p.EffectiveDate
where next_p.VND_Printer_ID is null

update e
	set
		e.ModifiedBy = @ModifiedBy,
		e.ModifiedDate = getdate()
from EST_Estimate e join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join VND_Printer old_p on c.AssemblyVendor_ID = old_p.VND_Printer_ID
	join VND_Vendor v on old_p.VND_Vendor_ID = v.VND_Vendor_ID
	join VND_Printer new_p on v.VND_Vendor_ID = new_p.VND_Vendor_ID
		and new_p.EffectiveDate > old_p.EffectiveDate
		and e.RunDate >= new_p.EffectiveDate
	left join VND_Printer next_p on v.VND_Vendor_ID = next_p.VND_Vendor_ID
		and next_p.EffectiveDate > new_p.EffectiveDate
		and e.RunDate >= next_p.EffectiveDate
where next_p.VND_Printer_ID is null

update c
	set
		Printer_ID = new_p.VND_Printer_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_Estimate e join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join VND_Printer old_p on c.Printer_ID = old_p.VND_Printer_ID
	join VND_Vendor v on old_p.VND_Vendor_ID = v.VND_Vendor_ID
	join VND_Printer new_p on v.VND_Vendor_ID = new_p.VND_Vendor_ID
		and new_p.EffectiveDate > old_p.EffectiveDate
		and e.RunDate >= new_p.EffectiveDate
	left join VND_Printer next_p on v.VND_Vendor_ID = next_p.VND_Vendor_ID
		and next_p.EffectiveDate > new_p.EffectiveDate
		and e.RunDate >= next_p.EffectiveDate
where next_p.VND_Printer_ID is null

update c
	set
		AssemblyVendor_ID = new_p.VND_Printer_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_Estimate e join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join VND_Printer old_p on c.AssemblyVendor_ID = old_p.VND_Printer_ID
	join VND_Vendor v on old_p.VND_Vendor_ID = v.VND_Vendor_ID
	join VND_Printer new_p on v.VND_Vendor_ID = new_p.VND_Vendor_ID
		and new_p.EffectiveDate > old_p.EffectiveDate
		and e.RunDate >= new_p.EffectiveDate
	left join VND_Printer next_p on v.VND_Vendor_ID = next_p.VND_Vendor_ID
		and next_p.EffectiveDate > new_p.EffectiveDate
		and e.RunDate >= next_p.EffectiveDate
where next_p.VND_Printer_ID is null

/* Update Plate Cost Rate if a new Plate Cost Rate exists with matching description */
update c
	set
		c.PlateCost_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate orig_pr on c.PlateCost_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID
	join PRT_PrinterRate new_pr on p.VND_Printer_ID = new_pr.VND_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and orig_pr.Description = new_pr.Description

/* Update Plate Cost Rate if a new Plate Cost Rate exists w/o match description (set to default)*/
update c
	set
		c.PlateCost_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate orig_pr on c.PlateCost_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID
	join PRT_PrinterRate new_pr on p.VND_Printer_ID = new_pr.VND_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and new_pr.[Default] = 1

if exists(select 1
	from EST_Component c join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
		join PRT_PrinterRate orig_pr on c.PlateCost_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID) begin

	raiserror('New Printer Effective Date must have a Plate Cost Rate.', 16, 1)
	return
end

/* Update D&H Rate if a new D&H Rate exists with matching description */
update c
	set
		c.DigitalHandlenPrepare_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate orig_pr on c.DigitalHandlenPrepare_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID
	join PRT_PrinterRate new_pr on p.VND_Printer_ID = new_pr.VND_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and orig_pr.Description = new_pr.Description

/* Update D&H Rate if a new D&H exists w/o match description (set to default)*/
update c
	set
		c.DigitalHandlenPrepare_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate orig_pr on c.DigitalHandlenPrepare_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID
	join PRT_PrinterRate new_pr on p.VND_Printer_ID = new_pr.VND_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and new_pr.[Default] = 1

if exists(select 1
	from EST_Component c join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
		join PRT_PrinterRate orig_pr on c.DigitalHandlenPrepare_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID) begin

	raiserror('New Printer Effective Date must have a Digital H&P Rate.', 16, 1)
	return
end

/* Update Stitch-In Rate if a new Stitch-In Rate exists with matching description */
update c
	set
		c.StitchIn_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join VND_Printer p on c.AssemblyVendor_ID = p.VND_Printer_ID
	join PRT_PrinterRate orig_pr on c.StitchIn_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID
	join PRT_PrinterRate new_pr on p.VND_Printer_ID = new_pr.VND_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and orig_pr.Description = new_pr.Description

/* Update Stitch-In Rate if a new Stitch-In exists w/o match description (set to default)*/
update c
	set
		c.StitchIn_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join VND_Printer p on c.AssemblyVendor_ID = p.VND_Printer_ID
	join PRT_PrinterRate orig_pr on c.StitchIn_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID
	join PRT_PrinterRate new_pr on p.VND_Printer_ID = new_pr.VND_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and new_pr.[Default] = 1

if exists(select 1
	from EST_Component c join VND_Printer p on c.AssemblyVendor_ID = p.VND_Printer_ID
		join PRT_PrinterRate orig_pr on c.StitchIn_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID) begin

	raiserror('New Printer Effective Date must have a Stitch-In Rate.', 16, 1)
	return
end

/* Update Blow-In Rate if a new Blow-In Rate exists with matching description */
update c
	set
		c.BlowIn_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join VND_Printer p on c.AssemblyVendor_ID = p.VND_Printer_ID
	join PRT_PrinterRate orig_pr on c.BlowIn_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID
	join PRT_PrinterRate new_pr on p.VND_Printer_ID = new_pr.VND_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and orig_pr.Description = new_pr.Description

/* Update Blow-In Rate if a new Blow-In exists w/o match description (set to default)*/
update c
	set
		c.BlowIn_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join VND_Printer p on c.AssemblyVendor_ID = p.VND_Printer_ID
	join PRT_PrinterRate orig_pr on c.BlowIn_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID
	join PRT_PrinterRate new_pr on p.VND_Printer_ID = new_pr.VND_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and new_pr.[Default] = 1

if exists(select 1
	from EST_Component c join VND_Printer p on c.AssemblyVendor_ID = p.VND_Printer_ID
		join PRT_PrinterRate orig_pr on c.BlowIn_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID) begin

	raiserror('New Printer Effective Date must have a Blow-In Rate.', 16, 1)
	return
end

/* Update Onsert Rate if a new Onsert Rate exists with matching description */
update c
	set
		c.Onsert_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join VND_Printer p on c.AssemblyVendor_ID = p.VND_Printer_ID
	join PRT_PrinterRate orig_pr on c.Onsert_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID
	join PRT_PrinterRate new_pr on p.VND_Printer_ID = new_pr.VND_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and orig_pr.Description = new_pr.Description

/* Update Onsert Rate if a new Onsert exists w/o match description (set to default)*/
update c
	set
		c.Onsert_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join VND_Printer p on c.AssemblyVendor_ID = p.VND_Printer_ID
	join PRT_PrinterRate orig_pr on c.Onsert_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID
	join PRT_PrinterRate new_pr on p.VND_Printer_ID = new_pr.VND_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and new_pr.[Default] = 1

if exists(select 1
	from EST_Component c join VND_Printer p on c.AssemblyVendor_ID = p.VND_Printer_ID
		join PRT_PrinterRate orig_pr on c.Onsert_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID) begin

	raiserror('New Printer Effective Date must have a Onsert Rate.', 16, 1)
	return
end

/* Update Stitcher MR Rate if a new Stitcher MR Rate exists with matching description */
update c
	set
		c.StitcherMakeready_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate orig_pr on c.StitcherMakeready_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID
	join PRT_PrinterRate new_pr on p.VND_Printer_ID = new_pr.VND_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and orig_pr.Description = new_pr.Description

/* Update Stitcher MR Rate if a new Stitcher MR exists w/o match description (set to default)*/
update c
	set
		c.StitcherMakeready_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate orig_pr on c.StitcherMakeready_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID
	join PRT_PrinterRate new_pr on p.VND_Printer_ID = new_pr.VND_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and new_pr.[Default] = 1

if exists(select 1
	from EST_Component c join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
		join PRT_PrinterRate orig_pr on c.StitcherMakeready_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID) begin

	raiserror('New Printer Effective Date must have a Stitcher Makeready Rate.', 16, 1)
	return
end

/* Update Press MR Rate if a new Press MR Rate exists with matching description */
update c
	set
		c.PressMakeready_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate orig_pr on c.PressMakeready_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID
	join PRT_PrinterRate new_pr on p.VND_Printer_ID = new_pr.VND_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and orig_pr.Description = new_pr.Description

/* Update Press MR Rate if a new Press MR exists w/o match description (set to default)*/
update c
	set
		c.PressMakeready_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate orig_pr on c.PressMakeready_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID
	join PRT_PrinterRate new_pr on p.VND_Printer_ID = new_pr.VND_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and new_pr.[Default] = 1

if exists(select 1
	from EST_Component c join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
		join PRT_PrinterRate orig_pr on c.PressMakeready_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID) begin

	raiserror('New Printer Effective Date must have a Press Makeready Rate.', 16, 1)
	return
end

-- Update Polybag Printer ID
update pbg
	set
		VND_Printer_ID = new_p.VND_Printer_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_PolybagGroup pbg join VND_Printer old_p on pbg.VND_Printer_ID = old_p.VND_Printer_ID
	join VND_Vendor v on old_p.VND_Vendor_ID = v.VND_Vendor_ID
	join VND_Printer new_p on v.VND_Vendor_ID = new_p.VND_Vendor_ID
		and new_p.EffectiveDate > old_p.EffectiveDate
		and dbo.PolybagGroupRunDate(pbg.EST_PolybagGroup_ID) >= new_p.EffectiveDate
	left join VND_Printer next_p on v.VND_Vendor_ID = next_p.VND_Vendor_ID
		and next_p.EffectiveDate > new_p.EffectiveDate
		and dbo.PolybagGroupRunDate(pbg.EST_PolybagGroup_ID) >= next_p.EffectiveDate
where next_p.VND_Printer_ID is null

/* Update Polybag Bag Rate */
update pbg
	set
		PRT_BagRate_ID = new_pr.PRT_PrinterRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_PolybagGroup pbg join VND_Printer p on pbg.VND_Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate orig_pr on pbg.PRT_BagRate_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID
	join PRT_PrinterRate new_pr on p.VND_Printer_ID = new_pr.VND_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and orig_pr.Description = new_pr.Description

/* Update Polybag Bag Rate if a new Polybag Bag Rate exists w/o match description (set to default)*/
update pbg
	set
		PRT_BagRate_ID = new_pr.PRT_PrinterRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_PolybagGroup pbg join VND_Printer p on pbg.VND_Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate orig_pr on pbg.PRT_BagRate_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID
	join PRT_PrinterRate new_pr on p.VND_Printer_ID = new_pr.VND_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and new_pr.[Default] = 1

if exists(select 1
	from EST_PolybagGroup pbg join VND_Printer p on pbg.VND_Printer_ID = p.VND_Printer_ID
		join PRT_PrinterRate orig_pr on pbg.PRT_BagRate_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID) begin

	raiserror('New Printer Effective Date must have a Polybag Bag Rate.', 16, 1)
	return
end

/* Update Polybag Bag Makeready Rate */
update pbg
	set
		PRT_BagMakereadyRate_ID = new_pr.PRT_PrinterRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_PolybagGroup pbg join VND_Printer p on pbg.VND_Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate orig_pr on pbg.PRT_BagMakereadyRate_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID
	join PRT_PrinterRate new_pr on p.VND_Printer_ID = new_pr.VND_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and orig_pr.Description = new_pr.Description

/* Update Polybag Bag MR Rate if a new Polybag Bag MR Rate exists w/o match description (set to default)*/
update pbg
	set
		PRT_BagMakereadyRate_ID = new_pr.PRT_PrinterRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_PolybagGroup pbg join VND_Printer p on pbg.VND_Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate orig_pr on pbg.PRT_BagMakereadyRate_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID
	join PRT_PrinterRate new_pr on p.VND_Printer_ID = new_pr.VND_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and new_pr.[Default] = 1

if exists(select 1
	from EST_PolybagGroup pbg join VND_Printer p on pbg.VND_Printer_ID = p.VND_Printer_ID
		join PRT_PrinterRate orig_pr on pbg.PRT_BagMakereadyRate_ID = orig_pr.PRT_PrinterRate_ID and orig_pr.VND_Printer_ID <> p.VND_Printer_ID) begin

	raiserror('New Printer Effective Date must have a Polybag Bag Makeready Rate.', 16, 1)
	return
end
GO

GRANT  EXECUTE  ON [dbo].[Printer_u_ComponentandPolybag]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
