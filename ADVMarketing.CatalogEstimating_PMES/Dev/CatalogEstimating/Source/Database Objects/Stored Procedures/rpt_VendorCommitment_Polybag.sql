IF OBJECT_ID('dbo.rpt_VendorCommitment_Polybag') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.rpt_VendorCommitment_Polybag'
	DROP PROCEDURE dbo.rpt_VendorCommitment_Polybag
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.rpt_VendorCommitment_Polybag') IS NOT NULL
		PRINT '***********Drop of dbo.rpt_VendorCommitment_Polybag FAILED.'
END
GO
PRINT 'Creating dbo.rpt_VendorCommitment_Polybag'
GO

CREATE PROC dbo.rpt_VendorCommitment_Polybag
/*
* PARAMETERS:
* StartRunDate
* EndRunDate
* EST_Estimate_ID
* AdNumber
* EST_Status_ID
* VND_Vendor_ID
* Vendors - Xml formatted list of vendors to query.  If null, all vendors are queried.
*
* DESCRIPTION:
*		Returns data for the Vendor Commitment report for Polybag Costs.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   MANY
*
*
* PROCEDURES CALLED:
*   MANY
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
* -------------   --------        ------------------------------------------------------------------
* 12/17/2007      BJS             Initial Creation - Based on rpt_VendorCommitment_Vendor
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@StartRunDate datetime,
@EndRunDate datetime,
@EST_Estimate_ID bigint,
@AdNumber int,
@EST_Status_ID int,
@Vendors varchar(2000)
as

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
set nocount on

declare @VendorDocID int

create table #tempVendors(VND_Vendor_ID bigint NOT NULL, Description varchar(35) NOT NULL)

if (@Vendors is null) begin
	insert into #tempVendors(VND_Vendor_ID, Description)
	select VND_Vendor_ID, Description
	from VND_Vendor
end
else begin
	exec sp_xml_preparedocument @VendorDocID output, @Vendors
	insert into #tempVendors(VND_Vendor_ID, Description)
	select v.VND_Vendor_ID, v.Description
	from OPENXML(@VendorDocID, '/root/vnd_vendor')
	with(vnd_vendor_id INT '@vnd_vendor_id') xdata
		join VND_Vendor v on xdata.VND_Vendor_ID = v.VND_Vendor_ID
end

create table #tempComponentPolybag(
	EST_Component_ID bigint,
	PolybagVendor_ID bigint,
	PolybagVendor varchar(35),
	/* Main */
		RunDate datetime,
		AdNumber int,
		Description varchar(50),
	/* Specifications */
		PageCount int,
	/* Quantity */
		PolybagQuantity int,
		PolybagCost money
)


/* Get Raw Production Data */
insert into #tempComponentPolybag(EST_Component_ID, PolybagVendor_ID, PolybagVendor, RunDate, AdNumber, Description,
	PageCount, PolybagQuantity, PolybagCost)
select c.EST_Component_ID, max(prt_vnd.VND_Vendor_ID) PolybagVendor_ID, max(prt_vnd.Description) PolybagVendor,
	max(e.RunDate) RunDate, max(c.AdNumber) AdNumber, max(c.Description) Description, max(c.PageCount) PageCount,
	max(pb.Quantity) PolybagQuantity,
	sum(dbo.PolybagCost(pb.EST_Polybag_ID)
		* isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID)
			/ dbo.PolyBagWeight(pb.EST_Polybag_ID))) as PolybagCost
from EST_Estimate e join vw_Estimate_excludeOldUploads ve on e.EST_Estimate_ID = ve.EST_Estimate_ID
	join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join EST_EstimateMediaType emt on c.EST_EstimateMediaType_ID = emt.EST_EstimateMediaType_ID
	join EST_ComponentType ct on c.EST_ComponentType_ID = ct.EST_ComponentType_ID
	join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
	join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	join EST_PackagePolybag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
	join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
	join EST_PolybagGroup pbg on pb.EST_PolybagGroup_ID = pbg.EST_PolybagGroup_ID
	join VND_Printer prt on pbg.VND_Printer_ID = prt.VND_Printer_ID
	join VND_Vendor prt_vnd on prt.VND_Vendor_ID = prt_vnd.VND_Vendor_ID
where c.EST_ComponentType_ID = 1
	and (@StartRunDate is null or e.RunDate >= @StartRunDate)
	and (@EndRunDate is null or e.RunDate <= @EndRunDate)
	and (@EST_Estimate_ID is null or e.EST_Estimate_ID = @EST_Estimate_ID)
	and (@AdNumber is null or dbo.EstimateHostAdNumber(e.EST_Estimate_ID) = @AdNumber)
	and (@EST_Status_ID is null or e.EST_Status_ID = @EST_Status_ID)
	and exists(select 1 from #tempVendors tv where prt_vnd.VND_Vendor_ID = tv.VND_Vendor_ID)
group by c.EST_Component_ID, p.EST_Package_ID, pb.EST_Polybag_ID

set nocount off
select PolybagVendor_ID, AdNumber, max(PolybagVendor) VendorDescription,
	RunDate, max(Description) AdDescription, sum(PageCount) pages, sum(PolybagQuantity) MediaQuantity,
	sum(PolybagCost) GrossCost, sum(PolybagCost) NetCost
from #tempComponentPolybag
group by PolybagVendor_ID, AdNumber, RunDate
set nocount on

drop table #tempVendors
drop table #tempComponentPolybag
GO

GRANT  EXECUTE  ON [dbo].[rpt_VendorCommitment_Polybag]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

