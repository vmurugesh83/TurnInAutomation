IF OBJECT_ID('dbo.vendorcost_polybag_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vendorcost_polybag_i'
	DROP PROCEDURE dbo.vendorcost_polybag_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vendorcost_polybag_i') IS NOT NULL
		PRINT '***********Drop of dbo.vendorcost_polybag_i FAILED.'
END
GO
PRINT 'Creating dbo.vendorcost_polybag_i'
GO

CREATE PROC dbo.vendorcost_polybag_i
/*
* PARAMETERS:
* EstimateIDs - XML string, the estimates that will be uploaded
*
* DESCRIPTION:
*		Populates vendor_cost table with polybag vendor costs associated with the specified Estimate Ids
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   MANY
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
* 12/17/2007      BJS             Initial Creation - Copied logic from vendorcost_vnd_i
* 05/21/2008      BJS             Modified @EstimateIDs from varchar to text.
* 05/22/2008      BJS             Added transaction isolation level to avoid locking issues.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EstimateIDs text,
@CreatedBy varchar(50)
as

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
set nocount on

declare @EstimateDocID int

create table #tempEstimate(EST_Estimate_ID BIGINT NOT NULL)

exec sp_xml_preparedocument @EstimateDocID output, @EstimateIDs
insert into #tempEstimate(EST_Estimate_ID)
select EST_Estimate_ID
from OPENXML(@EstimateDocID, '/root/estimate')
with(est_estimate_id BIGINT '@id')

create table #tempVendors(VND_Vendor_ID bigint NOT NULL, Description varchar(35) NOT NULL)
insert into #tempVendors(VND_Vendor_ID, Description)
select VND_Vendor_ID, Description
from VND_Vendor

/* Comments correlate to layout of Report/Extract #5 - Estimate Summary Report */
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
from EST_Estimate e join #tempEstimate te on e.EST_Estimate_ID = te.EST_Estimate_ID
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
where c.AdNumber is not null and c.EST_ComponentType_ID = 1
group by c.EST_Component_ID, p.EST_Package_ID, pb.EST_Polybag_ID

/* Insert Polybag cost into persistent table*/
/* 730 - Polybag */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select PolybagVendor_ID, 730, AdNumber, max(PolybagVendor) VendorDescription,
	RunDate, max(Description) AdDescription, sum(PageCount) pages, sum(PolybagQuantity) MediaQuantity,
	null PubQuantity, 'Polybag' CostDescription, sum(PolybagCost) GrossCost,
	null Discount, sum(PolybagCost) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponentPolybag
group by PolybagVendor_ID, AdNumber, RunDate

drop table #tempVendors
drop table #tempComponentPolybag
drop table #tempEstimate
GO

GRANT  EXECUTE  ON [dbo].[vendorcost_polybag_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO


