IF OBJECT_ID('dbo.vendorcost_pub_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vendorcost_pub_i'
	DROP PROCEDURE dbo.vendorcost_pub_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vendorcost_pub_i') IS NOT NULL
		PRINT '***********Drop of dbo.vendorcost_pub_i FAILED.'
END
GO
PRINT 'Creating dbo.vendorcost_pub_i'
GO

CREATE PROC dbo.vendorcost_pub_i
/*
* PARAMETERS:
* EstimateIDs - XML string, the estimates that will be uploaded
*
* DESCRIPTION:
*		Populates vendor_cost table with pub costs associated with the specified Estimate Ids
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
* 12/06/2007      BJS             Initial Creation - Copied logic from rpt_VendorCommitment_Publisher
* 12/11/2007      BJS             Modified EstimateIDs parameter to allow 4000 characters
* 05/21/2008      BJS             Modified @EstimateIDs from varchar to text
* 05/22/2008      BJS             Added transaction isolation level to avoid locking issues.
* 06/18/2008      BJS             All pub rate types are prorated by weight.
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


declare @PubDocID int

create table #tempPubs(Pub_ID char(3) NOT NULL)
insert into #tempPubs(Pub_ID)
select distinct Pub_ID
from PUB_PubRate_Map

/* Comments correlate to layout of Report/Extract #4 - For Single Estimate Only */
create table #tempComponentPubRateMap(
	EST_Component_ID bigint,
	EST_Estimate_ID bigint,
	EST_Package_ID bigint,
	EST_EstimateMediaType_ID int,
	EST_ComponentType_ID int,
	/* Main */
		AdNumber int,
		Description varchar(50),
	/* Specifications */
		PageCount int,
		Width decimal(10,4),
		Height decimal(10,4),
		/* Paper */
			PaperWeight int,
		PieceWeight decimal(12,6), /* (Width * Height) / 950,000 * PageCount * PaperWeight * 1.03 */
		PackageWeight decimal(12,6), /* total component piece weights in package) */
		PackageSize decimal(12,6), /* size of package host component in square inches */
	/* Quantity */
		MediaQuantity int, /* Insert + Solo + PolyBag */

	PubRate_Map_ID bigint,
	Pub_ID char(3),
	PubLoc_ID int,
	PubQuantityType_ID int,
	PubRate_ID bigint,
	PubRateType_ID int,
	ProductionPieceCost money,
	RunDate datetime,
	IssueDate datetime,
	IssueDOW int,
	InsertTime bit,
	AssemblyPieceCost money,
	ComponentTabPageCount decimal,
	PackageTabPageCount decimal,
	InsertQuantity int,
	GrossInsertCost money,
	InsertDiscountPercent decimal(10,4),
	InsertDiscount money,
	NetInsertCost money
)

/* Get Raw Production Data */
insert into #tempComponentPubRateMap(EST_Component_ID, EST_Estimate_ID, EST_Package_ID, EST_EstimateMediaType_ID,
	EST_ComponentType_ID, AdNumber, Description, PageCount, Width, Height, PaperWeight, 
	PubRate_Map_ID, Pub_ID, PubLoc_ID, PubQuantityType_ID, PubRate_ID,
	RunDate, IssueDate, IssueDOW, InsertTime)
select distinct c.EST_Component_ID, e.EST_Estimate_ID, p.EST_Package_ID, c.EST_EstimateMediaType_ID,
	c.EST_ComponentType_ID, c.AdNumber, c.Description, c.PageCount, c.Width, c.Height, pw.Weight,
	prm.PUB_PubRate_Map_ID, prm.Pub_ID, prm.PubLoc_ID, p.PUB_PubQuantityType_ID, dbo.CalcPubRateID(prm.PUB_PubRate_Map_ID, pid.IssueDate),
	e.RunDate, pid.IssueDate,
	case p.PUB_PubQuantityType_ID
		when 4 then 1 /* Thanksgiving always uses Sunday rates */
		when 5 then 1 /* Christmas always uses Sunday rates */
		when 6 then 1 /* New Years always uses Sunday rates */
		else pid.IssueDOW
	end,
	ad.InsertTime
from EST_Estimate e join #tempEstimate te on e.EST_Estimate_ID = te.EST_Estimate_ID
	join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
	join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
	join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
	join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	join PUB_PubGroup pg on p.PUB_PubGroup_ID = pg.PUB_PubGroup_ID
	join PUB_PubPubGroup_Map ppgm on pg.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID
	join PUB_PubRate_Map prm on ppgm.PUB_PubRate_Map_ID = prm.PUB_PubRate_Map_ID
	join #tempPubs tp on prm.Pub_ID = tp.Pub_ID
	join EST_PubIssueDates pid on e.EST_Estimate_ID = pid.EST_Estimate_ID and prm.PUB_PubRate_Map_ID = pid.PUB_PubRate_Map_ID
where c.AdNumber is not null or c.EST_ComponentType_ID = 5

/* Components of type +CVR share the Host Ad Number if they do not have an Ad Number*/
update cvrcomp
	set cvrcomp.AdNumber = hostcomp.AdNumber
from #tempComponentPubRateMap hostcomp join #tempComponentPubRateMap cvrcomp on hostcomp.EST_Estimate_ID = cvrcomp.EST_Estimate_ID
where hostcomp.EST_ComponentType_ID = 1 and cvrcomp.EST_ComponentType_ID = 5 and cvrcomp.AdNumber is null

/* Perform Specification Calculations */
update #tempComponentPubRateMap
	set PieceWeight = (Width * Height) / cast(950000 as decimal) * PageCount * PaperWeight * 1.03

update #tempComponentPubRateMap
	set PackageWeight = dbo.PackageWeight(EST_Package_ID)

/* Set the PackageSize to the first matching component */
update tc
	set PackageWeight = c.Width * c.Height
from #tempComponentPubRateMap tc join EST_PackageComponentMapping pcm on tc.EST_Package_ID = pcm.EST_Package_ID
	join EST_Component c on pcm.EST_Component_ID = c.EST_Component_ID

/* Set the PackageSize to the size of the host component, if the package contains one */
update tc
	set PackageWeight = c.Width * c.Height
from #tempComponentPubRateMap tc join EST_PackageComponentMapping pcm on tc.EST_Package_ID = pcm.EST_Package_ID
	join EST_Component c on pcm.EST_Component_ID = c.EST_Component_ID
where c.EST_ComponentType_ID = 1

/* Perform Quantity Calculations */
update tc
	set MediaQuantity = mq.MediaQuantity
from #tempComponentPubRateMap tc join vwComponentMediaQuantity mq on tc.EST_Component_ID = mq.EST_Component_ID

update tc
	set PubRateType_ID = pr.PUB_RateType_ID
from #tempComponentPubRateMap tc join PUB_PubRate pr on tc.PubRate_ID = pr.PUB_PubRate_ID

update tc
	set InsertQuantity = (
		select top 1 dowqty.Quantity
		from PUB_PubQuantity ppq join PUB_DayOfWeekQuantity dowqty on ppq.PUB_PubQuantity_ID = dowqty.PUB_PubQuantity_ID
			join PUB_PubQuantityType pqt on dowqty.PUB_PubQuantityType_ID = pqt.PUB_PubQuantityType_ID
		where ppq.PUB_PubRate_Map_ID = tc.PubRate_Map_ID and dowqty.PUB_PubQuantityType_ID = tc.PubQuantityType_ID
			and (pqt.Special = 1 or tc.IssueDOW = dowqty.InsertDow)
			and ppq.EffectiveDate <= tc.IssueDate
		order by EffectiveDate desc)
from #tempComponentPubRateMap tc


update tc
	set ComponentTabPageCount =
		case
			when tc.EST_EstimateMediaType_ID = 2 then 2 -- Broadsheet
			else 1
		end
		*
		case
			when tc.EST_ComponentType_ID = 4 and pr.BlowInRate = 0 then 0 --Blow-In not charged
			when tc.EST_ComponentType_ID = 4 and pr.BlowInRate = 1 then cast(tc.PageCount as decimal) / 2 -- Blow-In charged at 1/2 page
			else tc.PageCount
		end
from #tempComponentPubRateMap tc join PUB_PubRate pr on tc.PubRate_ID = pr.PUB_PubRate_ID

update tc
	set PackageTabPageCount = (
		select sum(
			case
				when c.EST_EstimateMediaType_ID = 2 then 2 --Broadsheet
				else 1
			end
			*
			case
				when c.EST_ComponentType_ID = 4 and pr.BlowInRate = 0 then 0 --Blow-In not charged
				when c.EST_ComponentType_ID = 4 and pr.BlowInRate = 1 then cast(c.PageCount as decimal) / 2 --Blow-In charged at 1/2 page
				else c.PageCount
			end)
		from EST_PackageComponentMapping pcm join EST_Component c on pcm.EST_Component_ID = c.EST_Component_ID,
			PUB_PubRate pr
		where pcm.EST_Package_ID = tc.EST_Package_ID and pr.PUB_PubRate_ID = tc.PubRate_ID)
from #tempComponentPubRateMap tc

/* Calculate the Insert Cost */
-- Tab Page Count Rate Type
update tc
	set tc.GrossInsertCost =
		case tc.PackageTabPageCount
			when 0 then 0
			else (
				select top 1 wr.Rate
				from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
				where wrt.PUB_PubRate_ID = tc.PubRate_ID and wrt.RateTypeDescription >= tc.PackageTabPageCount and tc.IssueDOW = wr.InsertDow
				order by wrt.RateTypeDescription)
				*
				case pr.QuantityChargeType
					when 1 then tc.InsertQuantity - (tc.InsertQuantity * pr.BilledPct)
					else tc.InsertQuantity
				end
				/ cast(1000 as decimal)
				* tc.PieceWeight / tc.PackageWeight
		end
from #tempComponentPubRateMap tc join PUB_PubRate pr on tc.PubRate_ID = pr.PUB_PubRate_ID
where tc.PubRateType_ID = 1

-- Flat Rate Type
update tc
	set tc.GrossInsertCost = (
		select wr.Rate
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = tc.PubRate_ID and tc.IssueDOW = wr.InsertDow) --* tc.PieceWeight / tc.PackageWeight
from #tempComponentPubRateMap tc
where tc.PubRateType_ID = 2

-- CPM
update tc
	set tc.GrossInsertCost = (
		select top 1 wr.Rate
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = tc.PubRate_ID and wrt.RateTypeDescription >= tc.InsertQuantity and tc.IssueDOW = wr.InsertDow
		order by wrt.RateTypeDescription)
		*
		case pr.QuantityChargeType
			when 1 then tc.InsertQuantity - (tc.InsertQuantity * pr.BilledPct)
			else tc.InsertQuantity
		end
		/ cast(1000 as decimal)
		* tc.PieceWeight / tc.PackageWeight
from #tempComponentPubRateMap tc join PUB_PubRate pr on tc.PubRate_ID = pr.PUB_PubRate_ID
where tc.PubRateType_ID = 3

-- Weight
update tc
	set tc.GrossInsertCost = (
		select top 1 wr.Rate
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = tc.PubRate_ID and wrt.RateTypeDescription >= tc.PackageWeight and tc.IssueDOW = wr.InsertDow
		order by wrt.RateTypeDescription) * tc.PieceWeight / tc.PackageWeight
from #tempComponentPubRateMap tc
where tc.PubRateType_ID = 4

--Size
update tc
	set tc.GrossInsertCost = (
		select top 1 wr.Rate
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = tc.PubRate_ID and wrt.RateTypeDescription >= tc.PackageSize and tc.IssueDOW = wr.InsertDow
		order by wrt.RateTypeDescription) * tc.PieceWeight / tc.PackageWeight
from #tempComponentPubRateMap tc
where tc.PubRateType_ID = 5

/* Determine the Insert Discount Percentage */
/*----------------------------------------- */

create table #tempPackageInsertNumber(
	EST_Package_ID bigint,
	PUB_PubRate_Map_ID bigint,
	PackageWeight decimal(12,6),
	InsertDate datetime,
	InsertNumber int)

insert into #tempPackageInsertNumber(EST_Package_ID, PUB_PubRate_Map_ID, PackageWeight, InsertDate)
select p.EST_Package_ID, ppgm.PUB_PubRate_Map_ID, isnull(dbo.PackageWeight(p.EST_Package_ID), 0), pid.IssueDate
from EST_Estimate e join EST_PubIssueDates pid on e.EST_Estimate_ID = pid.EST_Estimate_ID
	join EST_Package p on e.EST_Estimate_ID = p.EST_Estimate_ID
	join PUB_PubGroup pg on p.PUB_PubGroup_ID = pg.PUB_PubGroup_ID
	join PUB_PubPubGroup_Map ppgm on pg.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID and pid.PUB_PubRate_Map_ID = ppgm.PUB_PubRate_Map_ID
where e.EST_Status_ID = 1


declare @num_prev_PubRate_Map_ID bigint, @num_prev_InsertDate datetime
declare @num_EST_Package_ID bigint, @num_PUB_PubRate_Map_ID bigint, @num_PackageWeight decimal(12,6), @num_InsertDate datetime,
	@num_InsertNumber int

declare insertnum_curs cursor for
select EST_Package_ID, PUB_PubRate_Map_ID, PackageWeight, InsertDate
from #tempPackageInsertNumber
order by PUB_PubRate_Map_ID, InsertDate, PackageWeight desc, EST_Package_ID

open insertnum_curs
fetch next from insertnum_curs into @num_EST_Package_ID, @num_PUB_PubRate_Map_ID, @num_PackageWeight, @num_InsertDate
while @@fetch_status = 0 begin
	if (@num_PUB_PubRate_Map_ID = @num_prev_PubRate_Map_ID and @num_InsertDate = @num_prev_InsertDate) begin
		set @num_InsertNumber = @num_InsertNumber + 1
	end
	else begin
		set @num_prev_PubRate_Map_ID = @num_PUB_PubRate_Map_ID
		set @num_prev_InsertDate = @num_InsertDate
		set @num_InsertNumber = 1
	end
	update #tempPackageInsertNumber
		set InsertNumber = @num_InsertNumber
	where EST_Package_ID = @num_EST_Package_ID and PUB_PubRate_Map_ID = @num_PUB_PubRate_Map_ID and InsertDate = @num_InsertDate

	fetch next from insertnum_curs into @num_EST_Package_ID, @num_PUB_PubRate_Map_ID, @num_PackageWeight, @num_InsertDate
end
close insertnum_curs
deallocate insertnum_curs

update t
	set InsertDiscountPercent = d.[Discount]
from #tempComponentPubRateMap t join #tempPackageInsertNumber pin on t.EST_Package_ID = pin.EST_Package_ID and t.PubRate_Map_ID = pin.PUB_PubRate_Map_ID
	join PUB_InsertDiscounts d on t.PubRate_ID = d.PUB_PubRate_ID
where pin.InsertNumber = d.[Insert]

drop table #tempPackageInsertNumber

/*----------------------------------------- */

update #tempComponentPubRateMap
	set
		InsertDiscount = GrossInsertCost * isnull(InsertDiscountPercent, 0)

update #tempComponentPubRateMap
	set
		NetInsertCost = GrossInsertCost - InsertDiscount

/* Insert cost records into vendor_cost table */
/* 850 - Insertion */
insert into vendor_cost(Pub_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select Pub_ID, 850, AdNumber, Pub_ID VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, MediaQuantity, InsertQuantity,
	'Insertion' CostDescription,
	sum(GrossInsertCost) GrossCost, sum(InsertDiscount) Discount, sum(NetInsertCost) NetCost,
	@CreatedBy, getdate() CreatedDate
from #tempComponentPubRateMap
where Pub_ID is not null and (GrossInsertCost > 0 or InsertDiscount > 0 or NetInsertCost > 0)
group by Pub_ID, RunDate, AdNumber, Description, MediaQuantity, InsertQuantity

drop table #tempComponentPubRateMap
drop table #tempPubs
drop table #tempEstimate
GO

GRANT  EXECUTE  ON [dbo].[vendorcost_pub_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

