IF OBJECT_ID('dbo.Printer_u_ComponentandPolybag_ByPrinterID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.Printer_u_ComponentandPolybag_ByPrinterID'
	DROP PROCEDURE dbo.Printer_u_ComponentandPolybag_ByPrinterID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.Printer_u_ComponentandPolybag_ByPrinterID') IS NOT NULL
		PRINT '***********Drop of dbo.Printer_u_ComponentandPolybag_ByPrinterID FAILED.'
END
GO
PRINT 'Creating dbo.Printer_u_ComponentandPolybag_ByPrinterID'
GO

CREATE PROC dbo.Printer_u_ComponentandPolybag_ByPrinterID
/*
* PARAMETERS:
* VND_Printer_ID - The Printer that will be deleted.
*
*
* DESCRIPTION:
* Updates any Component and Polybag records, removing references to the printer and the corresponding printer rates
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
@VND_Printer_ID bigint,
@ModifiedBy varchar(50)
as

declare @new_Printer_ID bigint
select top 1 @new_Printer_ID = new_p.VND_Printer_ID
from VND_Printer orig_p join VND_Vendor v on orig_p.VND_Vendor_ID = v.VND_Vendor_ID
	join VND_Printer new_p on v.VND_Vendor_ID = new_p.VND_Vendor_ID and new_p.EffectiveDate < orig_p.EffectiveDate
where orig_p.VND_Printer_ID = @VND_Printer_ID
order by new_p.EffectiveDate desc

if (@new_Printer_ID is null) begin
	raiserror('Cannot delete printer effective date record.  No earlier printer effective date records can be found for vendor.', 16, 1)
	return
end

update e
	set
		e.ModifiedBy = @ModifiedBy,
		e.ModifiedDate = getdate()
from EST_Estimate e join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
where c.Printer_ID = @VND_Printer_ID
	or c.AssemblyVendor_ID = @VND_Printer_ID

/* Update Plate Cost Rate if another Plate Cost Rate exists with matching description */
update c
	set
		c.PlateCost_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join PRT_PrinterRate orig_pr on c.PlateCost_ID = orig_pr.PRT_PrinterRate_ID
	join PRT_PrinterRate new_pr on new_pr.VND_Printer_ID = @new_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and orig_pr.Description = new_pr.Description
where c.Printer_ID = @VND_Printer_ID

/* Update Plate Cost Rate if a new Plate Cost Rate exists w/o match description (set to default)*/
update c
	set
		c.PlateCost_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join PRT_PrinterRate orig_pr on c.PlateCost_ID = orig_pr.PRT_PrinterRate_ID
	join PRT_PrinterRate new_pr on new_pr.VND_Printer_ID = @new_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and new_pr.[Default] = 1
where c.Printer_ID = @VND_Printer_ID

if exists(select 1
	from EST_Component c join PRT_PrinterRate pr on c.PlateCost_ID = pr.PRT_PrinterRate_ID
	where pr.VND_Printer_ID = @VND_Printer_ID) begin

	raiserror('Old Printer Effective Date must have a Plate Cost Rate.', 16, 1)
	return
end

/* Update D&H Rate if another D&H Rate exists with matching description */
update c
	set
		c.DigitalHandlenPrepare_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join PRT_PrinterRate orig_pr on c.DigitalHandlenPrepare_ID = orig_pr.PRT_PrinterRate_ID
	join PRT_PrinterRate new_pr on new_pr.VND_Printer_ID = @new_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and orig_pr.Description = new_pr.Description
where c.Printer_ID = @VND_Printer_ID

/* Update D&H Rate if a new D&H Rate exists w/o match description (set to default)*/
update c
	set
		c.DigitalHandlenPrepare_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join PRT_PrinterRate orig_pr on c.DigitalHandlenPrepare_ID = orig_pr.PRT_PrinterRate_ID
	join PRT_PrinterRate new_pr on new_pr.VND_Printer_ID = @new_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and new_pr.[Default] = 1
where c.Printer_ID = @VND_Printer_ID

if exists(select 1
	from EST_Component c join PRT_PrinterRate pr on c.DigitalHandlenPrepare_ID = pr.PRT_PrinterRate_ID
	where pr.VND_Printer_ID = @VND_Printer_ID) begin

	raiserror('Old Printer Effective Date must have a Digital H&P Rate.', 16, 1)
	return
end


/* Update Stitch-In Rate if another Stitch-In Rate exists with matching description */
update c
	set
		c.StitchIn_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join PRT_PrinterRate orig_pr on c.StitchIn_ID = orig_pr.PRT_PrinterRate_ID
	join PRT_PrinterRate new_pr on new_pr.VND_Printer_ID = @new_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and orig_pr.Description = new_pr.Description
where c.AssemblyVendor_ID = @VND_Printer_ID

/* Update Stitch-In Rate if a new Stitch-In Rate exists w/o match description (set to default)*/
update c
	set
		c.StitchIn_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join PRT_PrinterRate orig_pr on c.StitchIn_ID = orig_pr.PRT_PrinterRate_ID
	join PRT_PrinterRate new_pr on new_pr.VND_Printer_ID = @new_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and new_pr.[Default] = 1
where c.AssemblyVendor_ID = @VND_Printer_ID

if exists(select 1
	from EST_Component c join PRT_PrinterRate pr on c.StitchIn_ID = pr.PRT_PrinterRate_ID
	where pr.VND_Printer_ID = @VND_Printer_ID) begin

	raiserror('Old Printer Effective Date must have a Stitch-In Rate.', 16, 1)
	return
end


/* Update Blow-In Rate if another Blow-In Rate exists with matching description */
update c
	set
		c.BlowIn_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join PRT_PrinterRate orig_pr on c.BlowIn_ID = orig_pr.PRT_PrinterRate_ID
	join PRT_PrinterRate new_pr on new_pr.VND_Printer_ID = @new_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and orig_pr.Description = new_pr.Description
where c.AssemblyVendor_ID = @VND_Printer_ID

/* Update Blow-In Rate if a new Blow-In Rate exists w/o match description (set to default)*/
update c
	set
		c.BlowIn_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join PRT_PrinterRate orig_pr on c.BlowIn_ID = orig_pr.PRT_PrinterRate_ID
	join PRT_PrinterRate new_pr on new_pr.VND_Printer_ID = @new_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and new_pr.[Default] = 1
where c.AssemblyVendor_ID = @VND_Printer_ID

if exists(select 1
	from EST_Component c join PRT_PrinterRate pr on c.BlowIn_ID = pr.PRT_PrinterRate_ID
	where pr.VND_Printer_ID = @VND_Printer_ID) begin

	raiserror('Old Printer Effective Date must have a Blow-In Rate.', 16, 1)
	return
end


/* Update Onsert Rate if another Onsert Rate exists with matching description */
update c
	set
		c.Onsert_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join PRT_PrinterRate orig_pr on c.Onsert_ID = orig_pr.PRT_PrinterRate_ID
	join PRT_PrinterRate new_pr on new_pr.VND_Printer_ID = @new_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and orig_pr.Description = new_pr.Description
where c.AssemblyVendor_ID = @VND_Printer_ID

/* Update Onsert Rate if a new Onsert Rate exists w/o match description (set to default)*/
update c
	set
		c.Onsert_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join PRT_PrinterRate orig_pr on c.Onsert_ID = orig_pr.PRT_PrinterRate_ID
	join PRT_PrinterRate new_pr on new_pr.VND_Printer_ID = @new_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and new_pr.[Default] = 1
where c.AssemblyVendor_ID = @VND_Printer_ID

if exists(select 1
	from EST_Component c join PRT_PrinterRate pr on c.Onsert_ID = pr.PRT_PrinterRate_ID
	where pr.VND_Printer_ID = @VND_Printer_ID) begin

	raiserror('Old Printer Effective Date must have an Onsert Rate.', 16, 1)
	return
end


/* Update Stitcher Makeready Rate if another Stitcher Makeready Rate exists with matching description */
update c
	set
		c.StitcherMakeready_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join PRT_PrinterRate orig_pr on c.StitcherMakeready_ID = orig_pr.PRT_PrinterRate_ID
	join PRT_PrinterRate new_pr on new_pr.VND_Printer_ID = @new_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and orig_pr.Description = new_pr.Description
where c.Printer_ID = @VND_Printer_ID

/* Update Stitcher MR Rate if a new Stitcher MR Rate exists w/o match description (set to default)*/
update c
	set
		c.StitcherMakeready_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join PRT_PrinterRate orig_pr on c.StitcherMakeready_ID = orig_pr.PRT_PrinterRate_ID
	join PRT_PrinterRate new_pr on new_pr.VND_Printer_ID = @new_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and new_pr.[Default] = 1
where c.Printer_ID = @VND_Printer_ID

if exists(select 1
	from EST_Component c join PRT_PrinterRate pr on c.StitcherMakeready_ID = pr.PRT_PrinterRate_ID
	where pr.VND_Printer_ID = @VND_Printer_ID) begin

	raiserror('Old Printer Effective Date must have a Stitcher Makeready Rate.', 16, 1)
	return
end


/* Update Press Makeready Rate if another Press Makeready Rate exists with matching description */
update c
	set
		c.PressMakeready_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join PRT_PrinterRate orig_pr on c.PressMakeready_ID = orig_pr.PRT_PrinterRate_ID
	join PRT_PrinterRate new_pr on new_pr.VND_Printer_ID = @new_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and orig_pr.Description = new_pr.Description
where c.Printer_ID = @VND_Printer_ID

/* Update Plate Cost Rate if a new Press Makeready Rate exists w/o match description (set to default)*/
update c
	set
		c.PressMakeready_ID = new_pr.PRT_PrinterRate_ID,
		c.ModifiedBy = @ModifiedBy,
		c.ModifiedDate = getdate()
from EST_Component c join PRT_PrinterRate orig_pr on c.PressMakeready_ID = orig_pr.PRT_PrinterRate_ID
	join PRT_PrinterRate new_pr on new_pr.VND_Printer_ID = @new_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and new_pr.[Default] = 1
where c.Printer_ID = @VND_Printer_ID

if exists(select 1
	from EST_Component c join PRT_PrinterRate pr on c.PressMakeready_ID = pr.PRT_PrinterRate_ID
	where pr.VND_Printer_ID = @VND_Printer_ID) begin

	raiserror('Old Printer Effective Date must have a Press Makeready Rate.', 16, 1)
	return
end

--Update AssemblyVendorID on Component records
update EST_Component
	set
		AssemblyVendor_ID = @new_Printer_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where AssemblyVendor_ID = @VND_Printer_ID

--Update PrinterID on Component records
update EST_Component
	set
		Printer_ID = @new_Printer_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where Printer_ID = @VND_Printer_ID

/* Update Polybag Bag Rate */
update pbg
	set
		PRT_BagRate_ID = new_pr.PRT_PrinterRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_PolybagGroup pbg join PRT_PrinterRate orig_pr on pbg.PRT_BagRate_ID = orig_pr.PRT_PrinterRate_ID
	join PRT_PrinterRate new_pr on new_pr.VND_Printer_ID = @new_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and orig_pr.Description = new_pr.Description
where pbg.VND_Printer_ID = @VND_Printer_ID

/* Update Polybag Bag Rate if a new Polybag Bag Rate exists w/o match description (set to default)*/
update pbg
	set
		PRT_BagRate_ID = new_pr.PRT_PrinterRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_PolybagGroup pbg join PRT_PrinterRate orig_pr on pbg.PRT_BagRate_ID = orig_pr.PRT_PrinterRate_ID
	join PRT_PrinterRate new_pr on new_pr.VND_Printer_ID = @new_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and new_pr.[Default] = 1
where pbg.VND_Printer_ID = @VND_Printer_ID

if exists(select 1
	from EST_PolybagGroup pbg join PRT_PrinterRate pr on pbg.PRT_BagRate_ID = pr.PRT_PrinterRate_ID
	where pr.VND_Printer_ID = @VND_Printer_ID) begin

	raiserror('Old Printer Effective Date must have a Polybag Bag Rate.', 16, 1)
	return
end

/* Update Polybag Bag Makeready Rate */
update pbg
	set
		PRT_BagMakereadyRate_ID = new_pr.PRT_PrinterRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_PolybagGroup pbg join PRT_PrinterRate orig_pr on pbg.PRT_BagMakereadyRate_ID = orig_pr.PRT_PrinterRate_ID
	join PRT_PrinterRate new_pr on new_pr.VND_Printer_ID = @new_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and orig_pr.Description = new_pr.Description
where pbg.VND_Printer_ID = @VND_Printer_ID

/* Update Polybag Bag MR Rate if a new Polybag Bag MR Rate exists w/o match description (set to default)*/
update pbg
	set
		PRT_BagMakereadyRate_ID = new_pr.PRT_PrinterRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_PolybagGroup pbg join PRT_PrinterRate orig_pr on pbg.PRT_BagMakereadyRate_ID = orig_pr.PRT_PrinterRate_ID
	join PRT_PrinterRate new_pr on new_pr.VND_Printer_ID = @new_Printer_ID and orig_pr.PRT_PrinterRateType_ID = new_pr.PRT_PrinterRateType_ID and new_pr.[Default] = 1
where pbg.VND_Printer_ID = @VND_Printer_ID

if exists(select 1
	from EST_PolybagGroup pbg join PRT_PrinterRate pr on pbg.PRT_BagMakereadyRate_ID = pr.PRT_PrinterRate_ID
	where pr.VND_Printer_ID = @VND_Printer_ID) begin

	raiserror('New Printer Effective Date must have a Polybag Bag Makeready Rate.', 16, 1)
	return
end

-- Update Polybag Printer ID
update pbg
	set
		VND_Printer_ID = @new_Printer_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_PolybagGroup pbg
where pbg.VND_Printer_ID = @VND_Printer_ID
GO

GRANT  EXECUTE  ON [dbo].[Printer_u_ComponentandPolybag_ByPrinterID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

