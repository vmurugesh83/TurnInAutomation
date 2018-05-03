IF OBJECT_ID('dbo.rpt_PostageCategoryUsage') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.rpt_PostageCategoryUsage'
	DROP PROCEDURE dbo.rpt_PostageCategoryUsage
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.rpt_PostageCategoryUsage') IS NOT NULL
		PRINT '***********Drop of dbo.rpt_PostageCategoryUsage FAILED.'
END
GO
PRINT 'Creating dbo.rpt_PostageCategoryUsage'
GO

CREATE PROC dbo.rpt_PostageCategoryUsage
/*
* PARAMETERS:
* StartRunDate
* EndRunDate
* EST_Estimate_ID
* EST_PolybagGroup_ID
* AdNumber
* EST_Status_ID
* VendorSupplied - 1 = All Components, 2 = Only VS Components, 3 = Exclude VS Components
* EstimateMediaType - Xml formatted list of media types to query.  If null, all media types are queried.
*
* DESCRIPTION:
*		Returns data for the Postage Category Usage Report.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*
* PROCEDURES CALLED:
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
* 09/20/2007      BJS             Initial Creation
* 10/08/2007      BJS             Explicitly cast values (ie 1000) as decimal to prevent rounding errors in calculations.
* 10/23/2007      BJS             Changed Polybag_ID parameter to PolybagGroup_ID
* 11/21/2007      JRH             Fixed a Divide-by-Zero error in the last update.
* 11/26/2007      JRH             Return Rolled up costs.
* 11/26/2007      JRH             Fix quantity.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@StartRunDate datetime,
@EndRunDate datetime,
@EST_Estimate_ID bigint,
@EST_PolybagGroup_ID bigint,
@AdNumber int,
@EST_Status_ID int,
@VendorSupplied tinyint,
@EstimateMediaType varchar(2000)
as

set nocount on
declare @EstimateMediaTypeDocID int

create table #tempEstimateMediaType(EST_EstimateMediaType_ID int NOT NULL, Description varchar(35) NOT NULL)

if (@EstimateMediaType is null) begin
	insert into #tempEstimateMediaType(EST_EstimateMediaType_ID, Description)
	select EST_EstimateMediaType_ID, Description
	from EST_EstimateMediaType
end
else begin
	exec sp_xml_preparedocument @EstimateMediaTypeDocID output, @EstimateMediaType
	insert into #tempEstimateMediaType(EST_EstimateMediaType_ID, Description)
	select emt.EST_EstimateMediaType_ID, emt.Description
	from OPENXML(@EstimateMediaTypeDocID, '/root/est_estimatemediatype')
	with(est_estimatemediatype_id INT '@est_estimatemediatype_id') xdata
		join EST_EstimateMediaType emt on xdata.EST_EstimateMediaType_ID = emt.EST_EstimateMediaType_ID
end

/* Determine the costs of any polybags that match the search criteria */

create table #tempPolybag(
	EST_Polybag_ID bigint,
	PST_PostalMailerType_ID int,
	PST_PostalClass_ID int,
	PST_PostalCategory_ID int,
	FirstOverweightLimit decimal(12,4),
	StandardOverweightLimit decimal(12,4),
	UnderweightPieceRate money,
	OverweightPoundRate money,
	OverweightPieceRate money,
	Percentage decimal(10,4),
	Quantity int,
	PolybagWeight decimal(12,6),
	PolybagWeightIncludeBagWeight decimal(12,6),
	WeightDesc varchar(50),
	Cost money)

insert into #tempPolybag(EST_Polybag_ID, PST_PostalMailerType_ID, PST_PostalClass_ID, PST_PostalCategory_ID, FirstOverweightLimit,
	StandardOverweightLimit, UnderweightPieceRate, OverweightPoundRate, OverweightPieceRate, Percentage, Quantity, PolybagWeight,
	PolybagWeightIncludeBagWeight)
select pb.EST_Polybag_ID, max(pcrm.PST_PostalMailerType_ID) PST_PostalMailerType_ID, max(pcrm.PST_PostalClass_ID) PST_PostalClass_ID,
	max(pcrm.PST_PostalCategory_ID) PST_PostalCategory_ID, max(pw.FirstOverweightLimit) FirstOverweightLimit,
	max(pw.StandardOverweightLimit) StandardOverweightLimit, max(pcrm.UnderweightPieceRate) UnderweightPieceRate,
	max(pcrm.OverweightPoundRate) OverweightPoundRate, max(pcrm.OverweightPieceRate) OverweightPieceRate, max(pcsm.Percentage) Percentage,
	max(pb.Quantity) Quantity, dbo.PolybagWeight(pb.EST_Polybag_ID) PolybagWeight,
	dbo.PolybagWeightIncludeBagWeight(pb.EST_Polybag_ID) PolybagWeightIncludeBagWeight
from EST_Estimate e join vw_Estimate_excludeOldUploads eo on e.EST_Estimate_ID = eo.EST_Estimate_ID
	join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join #tempEstimateMediaType emt on c.EST_EstimateMediaType_ID = emt.EST_EstimateMediaType_ID
	join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
	join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	join EST_PackagePolybag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
	join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
	join PST_PostalScenario ps on pb.PST_PostalScenario_ID = ps.PST_PostalScenario_ID
	join PST_PostalCategoryScenario_Map pcsm on ps.PST_PostalScenario_ID = pcsm.PST_PostalScenario_ID
	join PST_PostalCategoryRate_Map pcrm on pcsm.PST_PostalCategoryRate_Map_ID = pcrm.PST_PostalCategoryRate_Map_ID
	join PST_PostalWeights pw on pcrm.PST_PostalWeights_ID = pw.PST_PostalWeights_ID
where (@StartRunDate is null or e.RunDate >= @StartRunDate)
	and (@EndRunDate is null or e.RunDate <= @EndRunDate)
	and (@EST_Estimate_ID is null or e.EST_Estimate_ID = @EST_Estimate_ID)
	and (@EST_PolybagGroup_ID is null or pb.EST_PolybagGroup_ID = @EST_PolybagGroup_ID)
	and (@AdNumber is null or dbo.EstimateHostAdNumber(e.EST_Estimate_ID) = @AdNumber)
	and (@EST_Status_ID is null or e.EST_Status_ID = @EST_Status_ID)
	and (@VendorSupplied = 1 or (@VendorSupplied = 2 and c.VendorSupplied = 1) or (@VendorSupplied = 3 and c.VendorSupplied = 0))
group by pb.EST_Polybag_ID, pcsm.PST_PostalCategoryRate_Map_ID

update #tempPolybag
	set
		WeightDesc =
			case PST_PostalClass_ID
				when 1 then /* First Class */
					case
						when PolybagWeightIncludeBagWeight < FirstOverweightLimit then 'Underweight'
						else 'Overweight'
					end
				else /* Standard */
					case
						when PolybagWeightIncludeBagWeight < StandardOverweightLimit then 'Underweight'
						else 'Overweight'
					end
			end,
		Cost =
			case PST_PostalClass_ID
				when 1 then /* First Class */
					case
						when PolybagWeightIncludeBagWeight < FirstOverweightLimit then Quantity * UnderWeightPieceRate * Percentage
						else (Quantity * OverWeightPieceRate + (Quantity * PolybagWeightIncludeBagWeight) * OverWeightPoundRate) * Percentage
					end
				else /* Standard */
					case
						when PolybagWeightIncludeBagWeight < StandardOverweightLimit then Quantity * UnderWeightPieceRate * Percentage
						else (Quantity * OverWeightPieceRate + (Quantity * PolybagWeightIncludeBagWeight) * OverWeightPoundRate) * Percentage
					end
			end

update #tempPolybag
	set
		Quantity = 
			case Percentage 
				when 0 then 0  
				else convert(int, round(Percentage * convert(decimal(20,4),Quantity), 0))
			end

/* Determine the Polybag Postage Costs of any matching Packages */

create table #tempPackagePolybagMap(
	EST_Package_ID bigint,
	EST_Polybag_ID bigint,
	PST_PostalMailerType_ID int,
	PST_PostalClass_ID int,
	PST_PostalCategory_ID int,
	PackagePieceWeight decimal(12,6),
	WeightDesc varchar(50),
	Quantity int,
	Cost money)

insert into #tempPackagePolybagMap(EST_Package_ID, EST_Polybag_ID, PST_PostalMailerType_ID, PST_PostalClass_ID, PST_PostalCategory_ID,
	PackagePieceWeight, WeightDesc, Quantity, Cost)
select ppm.EST_Package_ID, tpb.EST_Polybag_ID, tpb.PST_PostalMailerType_ID, tpb.PST_PostalClass_ID, tpb.PST_PostalCategory_ID,
	dbo.PackageWeight(ppm.EST_Package_ID) PackagePieceWeight, tpb.WeightDesc, tpb.Quantity,
	case
		when ppm.DistributionPct is null then tpb.Cost * dbo.PackageWeight(ppm.EST_Package_ID) / tpb.PolybagWeight
		else tpb.Cost * ppm.DistributionPct
	end Cost
from #tempPolybag tpb join EST_PackagePolybag_Map ppm on tpb.EST_Polybag_ID = ppm.EST_Polybag_ID

/* Determine the Component Polybag Postage Costs */
create table #tempComponentPolybag(
	EST_Polybag_ID bigint,
	EST_Component_ID bigint,
	PST_PostalMailerType_ID int,
	PST_PostalClass_ID int,
	PST_PostalCategory_ID int,
	WeightDesc varchar(50),
	Quantity int,
	Cost money)

insert into #tempComponentPolybag(EST_Polybag_ID, EST_Component_ID, PST_PostalMailerType_ID, PST_PostalClass_ID, PST_PostalCategory_ID,
	WeightDesc, Quantity, Cost)
select tpp.EST_Polybag_ID, c.EST_Component_ID, tpp.PST_PostalMailerType_ID, tpp.PST_PostalClass_ID, tpp.PST_PostalCategory_ID,
	tpp.WeightDesc, tpp.Quantity,
	case
		when tpp.PackagePieceWeight = 0 then 0
		else tpp.Cost * dbo.ComponentWeight(c.EST_Component_ID) / tpp.PackagePieceWeight
	end Cost
from EST_Estimate e join vw_Estimate_excludeOldUploads eo on e.EST_Estimate_ID = eo.EST_Estimate_ID
	join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join #tempEstimateMediaType emt on c.EST_EstimateMediaType_ID = emt.EST_EstimateMediaType_ID
	join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
	join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	join #tempPackagePolybagMap tpp on p.EST_Package_ID = tpp.EST_Package_ID
	join EST_Polybag pb on tpp.EST_Polybag_ID = pb.EST_Polybag_ID
where (@StartRunDate is null or e.RunDate >= @StartRunDate)
	and (@EndRunDate is null or e.RunDate <= @EndRunDate)
	and (@EST_Estimate_ID is null or e.EST_Estimate_ID = @EST_Estimate_ID)
	and (@EST_PolybagGroup_ID is null or pb.EST_PolybagGroup_ID = @EST_PolybagGroup_ID)
	and (@AdNumber is null or dbo.EstimateHostAdNumber(e.EST_Estimate_ID) = @AdNumber)
	and (@EST_Status_ID is null or e.EST_Status_ID = @EST_Status_ID)
	and (@VendorSupplied = 1 or (@VendorSupplied = 2 and c.VendorSupplied = 1) or (@VendorSupplied = 3 and c.VendorSupplied = 0))

/* Gather Solo Mail Data */
create table #tempComponentSolo(
	EST_Package_ID bigint,
	EST_Component_ID bigint,
	PST_PostalMailerType_ID int,
	PST_PostalClass_ID int,
	PST_PostalCategory_ID int,
	FirstOverweightLimit decimal(12,4),
	StandardOverweightLimit decimal(12,4),
	UnderweightPieceRate money,
	OverweightPoundRate money,
	OverweightPieceRate money,
	Percentage decimal(10,4),
	Quantity int,
	PackagePieceWeight decimal(12,6),
	ComponentPieceWeight decimal(12,6),
	WeightDesc varchar(50),
	Cost money)

insert into #tempComponentSolo(EST_Package_ID, EST_Component_ID, PST_PostalMailerType_ID, PST_PostalClass_ID, PST_PostalCategory_ID,
	FirstOverweightLimit, StandardOverweightLimit, UnderweightPieceRate, OverweightPoundRate, OverweightPieceRate, Percentage,
	Quantity, PackagePieceWeight, ComponentPieceWeight)
select p.EST_Package_ID, c.EST_Component_ID, pcrm.PST_PostalMailerType_ID, pcrm.PST_PostalClass_ID, pcrm.PST_PostalCategory_ID,
	pw.FirstOverweightLimit, pw.StandardOverweightLimit, pcrm.UnderweightPieceRate,
	pcrm.OverweightPoundRate, pcrm.OverweightPieceRate, pcsm.Percentage, p.SoloQuantity,
	dbo.PackageWeight(p.EST_Package_ID) PackagePieceWeight,
	dbo.ComponentWeight(c.EST_Component_ID) ComponentPieceWeight
from EST_Estimate e join vw_Estimate_excludeOldUploads eo on e.EST_Estimate_ID = eo.EST_Estimate_ID
	join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
	join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join #tempEstimateMediaType emt on c.EST_EstimateMediaType_ID = emt.EST_EstimateMediaType_ID
	join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
	join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	join PST_PostalScenario ps on ad.PST_PostalScenario_ID = ps.PST_PostalScenario_ID
	join PST_PostalCategoryScenario_Map pcsm on ps.PST_PostalScenario_ID = pcsm.PST_PostalScenario_ID
	join PST_PostalCategoryRate_Map pcrm on pcsm.PST_PostalCategoryRate_Map_ID = pcrm.PST_PostalCategoryRate_Map_ID
	join PST_PostalWeights pw on pcrm.PST_PostalWeights_ID = pw.PST_PostalWeights_ID
where p.SoloQuantity is not null and p.SoloQuantity > 0
	and (@StartRunDate is null or e.RunDate >= @StartRunDate)
	and (@EndRunDate is null or e.RunDate <= @EndRunDate)
	and (@EST_Estimate_ID is null or e.EST_Estimate_ID = @EST_Estimate_ID)
	and (@EST_PolybagGroup_ID is null)
	and (@AdNumber is null or dbo.EstimateHostAdNumber(e.EST_Estimate_ID) = @AdNumber)
	and (@EST_Status_ID is null or e.EST_Status_ID = @EST_Status_ID)
	and (@VendorSupplied = 1 or (@VendorSupplied = 2 and c.VendorSupplied = 1) or (@VendorSupplied = 3 and c.VendorSupplied = 0))

update #tempComponentSolo
	set
		WeightDesc =
			case PST_PostalClass_ID
				when 1 then /* First Class */
					case
						when PackagePieceWeight < FirstOverweightLimit then 'Underweight'
						else 'Overweight'
					end
				else
					case
						when PackagePieceWeight < StandardOverweightLimit then 'Underweight'
						else 'Overweight'
					end
			end,
		Cost = 
			case
				when PackagePieceWeight = 0 then 0
				else ComponentPieceWeight / PackagePieceWeight
			end *
			case PST_PostalClass_ID
				when 1 then /* First Class */
					case
						when PackagePieceWeight < FirstOverweightLimit then Quantity * UnderWeightPieceRate * Percentage
						else (Quantity * OverWeightPieceRate + (Quantity * PackagePieceWeight) * OverWeightPoundRate) * Percentage
					end
				else /* Standard */
					case
						when PackagePieceWeight < StandardOverweightLimit then Quantity * UnderWeightPieceRate * Percentage
						else (Quantity * OverWeightPieceRate + (Quantity * PackagePieceWeight) * OverWeightPoundRate) * Percentage
					end
			end

update #tempComponentSolo
	set
		Quantity = 
			case Percentage 
				when 0 then 0  
				else convert(int, round(Percentage * convert(decimal(20,4),Quantity), 0))
			end


create table #tempSummaryData(
	PST_PostalMailerType_ID int,
	MailerTypeDesc varchar(35),
	PST_PostalClass_ID int,
	ClassDesc varchar(35),
	WeightDesc varchar(35),
	PST_PostalCategory_ID int,
	CategoryDesc varchar(35),
	Quantity int,
	PostageCost money,
	PercentageQuantity decimal(20,4),
	PercentageAmount money)

insert into #tempSummaryData(PST_PostalMailerType_ID, MailerTypeDesc, PST_PostalClass_ID, ClassDesc, WeightDesc, PST_PostalCategory_ID,
	CategoryDesc, Quantity, PostageCost)
select mt.PST_PostalMailerType_ID, max(mt.Description) MailerTypeDesc, cl.PST_PostalClass_ID, max(cl.Description) ClassDesc,
	max(tcp.WeightDesc) WeightDesc,
	pc.PST_PostalCategory_ID, max(pc.Description) CategoryDesc, max(tcp.Quantity) Quantity, sum(tcp.Cost) Cost
from #tempComponentPolybag tcp join PST_PostalCategory pc on tcp.PST_PostalCategory_ID = pc.PST_PostalCategory_ID
	join PST_PostalClass cl on tcp.PST_PostalClass_ID = cl.PST_PostalClass_ID
	join PST_PostalMailerType mt on tcp.PST_PostalMailerType_ID = mt.PST_PostalMailerType_ID
group by tcp.EST_Polybag_ID, mt.PST_PostalMailerType_ID, cl.PST_PostalClass_ID, pc.PST_PostalCategory_ID

insert into #tempSummaryData(PST_PostalMailerType_ID, MailerTypeDesc, PST_PostalClass_ID, ClassDesc, WeightDesc, PST_PostalCategory_ID,
	CategoryDesc, Quantity, PostageCost)
select mt.PST_PostalMailerType_ID, max(mt.Description) MailerTypeDesc, cl.PST_PostalClass_ID, max(cl.Description) ClassDesc,
	max(tcs.WeightDesc) WeightDesc,
	pc.PST_PostalCategory_ID, max(pc.Description) CategoryDesc, max(tcs.Quantity) Quantity, sum(tcs.Cost) Cost
from #tempComponentSolo tcs join PST_PostalCategory pc on tcs.PST_PostalCategory_ID = pc.PST_PostalCategory_ID
	join PST_PostalClass cl on tcs.PST_PostalClass_ID = cl.PST_PostalClass_ID
	join PST_PostalMailerType mt on tcs.PST_PostalMailerType_ID = mt.PST_PostalMailerType_ID
group by EST_Package_ID, mt.PST_PostalMailerType_ID, cl.PST_PostalClass_ID, pc.PST_PostalCategory_ID

update ts1
	set
		PercentageQuantity = 
			case
				(select sum(Quantity) from #tempSummaryData ts2
					where ts1.PST_PostalMailerType_ID = ts2.PST_PostalMailerType_ID
						and ts1.PST_PostalClass_ID = ts2.PST_PostalClass_ID
						and ts1.WeightDesc = ts2.WeightDesc)
				when 0 then 0
				else
					case
						cast((select sum(Quantity) from #tempSummaryData ts2
							where ts1.PST_PostalMailerType_ID = ts2.PST_PostalMailerType_ID
								and ts1.PST_PostalClass_ID = ts2.PST_PostalClass_ID
								and ts1.WeightDesc = ts2.WeightDesc) as decimal)
						when 0 then 0
						else
							ts1.Quantity
							/ cast((select sum(Quantity) from #tempSummaryData ts2
								where ts1.PST_PostalMailerType_ID = ts2.PST_PostalMailerType_ID
									and ts1.PST_PostalClass_ID = ts2.PST_PostalClass_ID
									and ts1.WeightDesc = ts2.WeightDesc) as decimal)
					end
			end,
		PercentageAmount =
			case
				(select sum(Quantity) from #tempSummaryData ts2
					where ts1.PST_PostalMailerType_ID = ts2.PST_PostalMailerType_ID
						and ts1.PST_PostalClass_ID = ts2.PST_PostalClass_ID
						and ts1.WeightDesc = ts2.WeightDesc)
				when 0 then 0
				else
					case
						(select sum(PostageCost) from #tempSummaryData ts2
							where ts1.PST_PostalMailerType_ID = ts2.PST_PostalMailerType_ID
								and ts1.PST_PostalClass_ID = ts2.PST_PostalClass_ID
								and ts1.WeightDesc = ts2.WeightDesc)
						when 0 then 0
						else
							ts1.PostageCost
							/ (select sum(PostageCost) from #tempSummaryData ts2
								where ts1.PST_PostalMailerType_ID = ts2.PST_PostalMailerType_ID
									and ts1.PST_PostalClass_ID = ts2.PST_PostalClass_ID
									and ts1.WeightDesc = ts2.WeightDesc)
					end
			end
from #tempSummaryData ts1

set nocount off

-- Return rolled up costs
SELECT
	PST_PostalMailerType_ID,
	MailerTypeDesc,
	PST_PostalClass_ID,
	ClassDesc,
	WeightDesc,
	PST_PostalCategory_ID,
	CategoryDesc,
	sum(Quantity) Quantity,
	sum(PostageCost) PostageCost,
	sum(PercentageQuantity) PercentageQuantity,
	sum(PercentageAmount) PercentageAmount
FROM
	#tempSummaryData
GROUP BY
	PST_PostalMailerType_ID,
	MailerTypeDesc,
	PST_PostalClass_ID,
	ClassDesc,
	WeightDesc,
	PST_PostalCategory_ID,
	CategoryDesc
ORDER BY
	PST_PostalMailerType_ID, 
	PST_PostalClass_ID, 
	WeightDesc desc, 
	PST_PostalCategory_ID

set nocount on

drop table #tempEstimateMediaType
drop table #tempPolybag
drop table #tempPackagePolybagMap
drop table #tempComponentPolybag
drop table #tempComponentSolo
drop table #tempSummaryData
GO

GRANT  EXECUTE  ON [dbo].[rpt_PostageCategoryUsage]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
