IF OBJECT_ID('dbo.adpubcost_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.adpubcost_i'
	DROP PROCEDURE dbo.adpubcost_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.adpubcost_i') IS NOT NULL
		PRINT '***********Drop of dbo.adpubcost_i FAILED.'
END
GO
PRINT 'Creating dbo.adpubcost_i'
GO

CREATE PROCEDURE dbo.adpubcost_i
/*
* PARAMETERS:
* EstimateIDs - XML string, the estimates that will be uploaded
*
*
* DESCRIPTION:
*		Populates adpub_cost table with insertion costs associated with the specified Estimate Id's
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   MANY                READ
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
* 12/04/2007      BJS             Initial Creation - Copied logic from rpt_AdPublicationCosts
* 12/11/2007      BJS             Modified EstimateIDs parameter to allow 4000 characters
*                                   Added Freight Costs into CostWithoutInsert
*                                   Changed calculation and precision of production and assembly piece costs.
* 12/17/2007      BJS             Added #tempComponent to improve performance.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EstimateIDs varchar(4000),
@CreatedBy varchar(50)
as

set nocount on

declare @EstimateDocID int

create table #tempEstimate(EST_Estimate_ID BIGINT NOT NULL)

exec sp_xml_preparedocument @EstimateDocID output, @EstimateIDs
insert into #tempEstimate(EST_Estimate_ID)
select EST_Estimate_ID
from OPENXML(@EstimateDocID, '/root/estimate')
with(est_estimate_id BIGINT '@id')

/* Make resource intensive queries on the component level only.  Then write results into #tempComponentPubRateMap */
create table #tempComponent(
	EST_Component_ID bigint,
	EST_Estimate_ID bigint,
	EST_ComponentType_ID int,
	SoloQuantity int,
	OtherQuantity int,
	SampleQuantity int,
	MediaQuantity int,
	TotalEstimateWeight decimal(14,6)
)

insert into #tempComponent(EST_Component_ID, EST_Estimate_ID, EST_ComponentType_ID)
select c.EST_Component_ID, e.EST_Estimate_ID, c.EST_ComponentType_ID
from EST_Estimate e join #tempEstimate te on e.EST_Estimate_ID = te.EST_Estimate_ID
	join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
where c.AdNumber is not null or c.EST_ComponentType_ID = 5

update tc
	set tc.TotalEstimateWeight = tw.TotalEstimateWeight
from #tempComponent tc join vwTotalEstimateWeight tw on tc.EST_Estimate_ID = tw.EST_Estimate_ID
where tc.EST_ComponentType_ID = 1

/* Perform Quantity Calculations */
update tc
	set SoloQuantity = isnull(sq.SoloQuantity, 0)
from #tempComponent tc left join vwComponentSoloQuantity sq on tc.EST_Component_ID = sq.EST_Component_ID

update tc
	set OtherQuantity = isnull(oq.OtherQuantity, 0)
from #tempComponent tc left join vwComponentOtherQuantity oq on tc.EST_Component_ID = oq.EST_Component_ID

update tc
	set SampleQuantity = isnull(sq.SampleQuantity, 0)
from #tempComponent tc left join vwComponentSampleQuantity sq on tc.EST_Component_ID = sq.EST_Component_ID

update tc
	set MediaQuantity = mq.MediaQuantity
from #tempComponent tc join vwComponentMediaQuantity mq on tc.EST_Component_ID = mq.EST_Component_ID

/* Comments correlate to layout of Report/Extract #4 - For Single Estimate Only */
create table #tempComponentPubRateMap(
	EST_Component_ID bigint,
	EST_Estimate_ID bigint,
	EST_Package_ID bigint,
	PUB_PubRate_Map_ID bigint,
	EST_EstimateMediaType_ID int,
	EST_ComponentType_ID int,
	PST_PostalScenario_ID bigint,
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
		SoloQuantity int,
		PolybagQuantity int,
		OtherQuantity int,
		SampleQuantity int,
		MediaQuantity int, /* Insert + Solo + PolyBag */
		SpoilagePct decimal(10,4),
		SpoilageQuantity int, /* SpoilagePct * Media Quantity */
		DirectMailQuantity int, /* Solo + Poly */
		InternalMailQuantity int, /* DirectMail - External List Quantity */
		ExternalMailQuantity int, /* External List Quantity */
		TotalProductionQuantity int, /* MediaQuantity + SpoilageQuantity + OtherQuantity + SampleQuantity */
	/* Production */
		ProductionWeight decimal(14,6), /* TotalProductionQuantity * PieceWeight */
		TotalEstimateWeight decimal(14,6), /* sum(ProductionWeight) group by EST_Estimate_ID*/
		CreativeCPP money,
		CreativeCost money,
		SeparatorCPP money,
		SeparatorCost money,
		/* Print */
			ManualPrintCost money,
			RunRate money,
			NumberOfPlants int,
			AdditionalPlates int,
			PrinterPlateCost money,
			NumberDigitalHandlePrepare int,
			DigitalHandlePrepareRate money,
			StitcherMakeReadyRate money,
			PressMakeReadyRate money,
			ReplacementPlateCost money,
			GrossPrintCost money, /* See Estimate Calculation Steps for formula */
			EarlyPayPrintDiscountPercent decimal(10,4),
			EarlyPayPrintDiscountAmount money, /* GrossPrintCost * EarlyPayPrintDiscountPercent */
			NetPrintCost money, /* GrossPrintCost - EarlyPayPrintDiscountAmount */
			PrinterTaxableMediaPct decimal(10,4),
			PrinterSalesTaxPct decimal(10,4),
			PrinterSalesTaxAmount money, /* (NetPrinterCost + PaperHandling) * PrinterTaxableMediaPct * PrinterSalesTaxPct */
			TotalPrintCost money, /* NetPrintCost + PrinterSalesTaxAmount */
		/* Paper */
			ManualPaperCost money,
			RunPounds decimal(10,2),
			MakereadyPounds int,
			PlateChangePounds decimal(10,2),
			NumberOfPressStops int,
			PressStopPounds int,
			TotalPaperPounds decimal(20,2), /* See Estimate Calculation Steps for formula */
			PaperCWTRate money, /* PPR_Paper_Map.CWT */
			GrossPaperCost money, /* TotalPaperPounds / 100 * PaperCWTRate */
			EarlyPayPaperDiscountPercent decimal(10,4),
			EarlyPayPaperDiscountAmount money, /* GrossPaperCost * EarlyPayPaperDiscountPercent */
			NetPaperCost money, /* GrossPaperCost - EarlyPayPaperDiscountAmount */
			PaperHandlingCWTRate money, /* VND_Printer.PaperHandling */
			PaperHandlingCost money, /* TotalPaperPounds * PaperHandlingCWTRate */
			PaperTaxableMediaPct decimal(10,4),
			PaperSalesTaxPct decimal(10,4),
			PaperSalesTaxAmount money, /* NetPaperCost * PaperTaxableMediaPct * PaperSalesTaxPct */
			TotalPaperCost money, /* NetPaperCost + PaperSalesTaxAmount */
		/* Mail List */
			InternalMailCPM money,
			ExternalMailCPM money,
			BlendedMailListCPM money,
			MailListCost money, /* See Logic below */
		VendorProductionCPM money,
		VendorProductionCost money, /* TotalProductionQuantity / 1000 * VendorProductionCPM  */
		OtherProductionCost money,
		ProductionTotal money, /* CreativeCost + SeparatorCost + TotalPrintCost + TotalPaperCost + ExternalMailCost + VendorProductionCost + OtherProductionCost */
	/* Assembly */
		/* PolyBag */
			PolyBagCost money, /* dbo.EstimatePolyBagCost */
			OnsertRate money,
			OnsertCost money, /* Only if component type is Host Onsert -- PolyBagQuantity / 1000 * OnsertRate */
			PolyBagTotal money, /* PolyBagCost + OnsertCost */
		StitchInRate money,
		StitchInCost money, /* StitchInRate * MediaQuantity (if comp type is stitch-in) / 1000 */
		BlowInRate money,
		BlowInCost money, /* BlowInRate * MediaQuantity (if comp type is blow-in) / 1000 */
		/* Ink Jet */
			InkJetRate money,
			InkJetMakeReadyRate money,
			InkJetCost money, /* DirectMailQuantity / 1000 * InkJetRate */
			GrossInkjetMakereadyCost money, /* InkJetMakeReadyRate * NumberOfPlants */
			InkjetMakereadyCost money, /* See Logic below */
			TotalInkJetCost money, /* InkJetCost + InkJetMakeReadyCost */
		/* Handling */
			/* Insert */
				CornerGuardRate money,
				CornerGuardCost money, /* (MediaQuantity * PieceWeight) / 2000 * CornerGuardRate */
				SkidRate money,
				SkidCost money, /* (MediaQuantity * PieceWeight) / 2000 * SkidRate */
				NumberOfCartons int,
				CartonRate money,
				CartonCost money, /* NumberOfCartons * CartonCost */
				InsertHandlingTotal money, /* CornerGuardCost + SkidCost + CartonCost */
			/* Mail */
				TimeValueSlipsCPM money,
				TimeValueSlipsCost money, /* See SQL Logic Below */
				GrossMailHouseAdminFee money, /* The Mail House Admin Fee. */
				MailHouseAdminFee money, /* See SQL Logic Below */
				GlueTackCPM money,
				GlueTackCost money, /* See SQL Logic Below */
				TabbingCPM money,
				TabbingCost money, /* See SQL Logic Below */
				LetterInsertionCPM money,
				LetterInsertionCost money, /* See SQL Logic Below */
				OtherMailHandlingCPM money,
				OtherMailHandlingCost money, /* See SQL Logic Below */
				MailHandlingTotal money, /* See SQL Logic Below */
			HandlingTotal money, /* InsertHandlingTotal + MailHandlngTotal */
		AssemblyTotal money, /* PolyBagTotal + StitchInCost + BlowInCost + TotalInkJetCost + HandlingTotal */
	/* Distribution */
 		/* Insert */
			InsertFreightCWT money,
			InsertFreightCost money, /* TotalEstimateWeight / 100 * InsertFreightCWT */
			InsertFuelSurchargePercent decimal(10,4),
			InsertFuelSurchargeCost money, /* InsertFreightCost * InsertFreightSurchargePercent */
			InsertFreightTotalCost money,
		SampleFreight money,
		GrossOtherFreight money,
		OtherFreight money,
	Pub_ID char(3),
	Pub_NM char(30),
	PubLoc_ID int,
	PubQuantityType_ID int,
	PubRate_ID bigint,
	PubRateType_ID int,
	ProductionPieceCost decimal(20,8),
	RunDate datetime,
	IssueDate datetime,
	IssueDOW int,
	InsertTime bit,
	AssemblyPieceCost decimal(20,8),
	ComponentTabPageCount decimal,
	PackageTabPageCount decimal,
	PubRateMapInsertQuantity int,
	InsertDiscountPercent decimal(10,4),
	PubRateMapInsertCost money
)

/* Get Raw Production Data */
insert into #tempComponentPubRateMap(EST_Component_ID, EST_Estimate_ID, EST_Package_ID, PUB_PubRate_Map_ID, EST_EstimateMediaType_ID,
	EST_ComponentType_ID, PST_PostalScenario_ID, AdNumber, Description, PageCount, Width,
	Height, PaperWeight, CreativeCPP, CreativeCost, SeparatorCPP, SeparatorCost,
	ExternalMailQuantity, SpoilagePct, ManualPrintCost, RunRate, NumberOfPlants,
	AdditionalPlates, PrinterPlateCost, NumberDigitalHandlePrepare, DigitalHandlePrepareRate, StitcherMakereadyRate, PressMakereadyRate,
	ReplacementPlateCost, EarlyPayPrintDiscountPercent, PrinterTaxableMediaPct,
	PrinterSalesTaxPct, ManualPaperCost, RunPounds, MakeReadyPounds, PlateChangePounds, NumberOfPressStops, PressStopPounds, PaperCWTRate,
	EarlyPayPaperDiscountPercent, PaperHandlingCWTRate, PaperTaxableMediaPct, PaperSalesTaxPct, InternalMailCPM, ExternalMailCPM,
	VendorProductionCPM, OtherProductionCost, OnsertRate, StitchInRate, BlowInRate, InkJetRate, InkJetMakeReadyRate, CornerGuardRate, SkidRate,
	NumberOfCartons, TimeValueSlipsCPM, GrossMailHouseAdminFee, GlueTackCPM, TabbingCPM, LetterInsertionCPM, OtherMailHandlingCPM, 
	InsertFreightCWT, InsertFuelSurchargePercent, GrossOtherFreight,
	Pub_ID, Pub_NM, PubLoc_ID, PubQuantityType_ID, PubRate_ID, RunDate, IssueDate, IssueDOW, InsertTime)
select c.EST_Component_ID, c.EST_Estimate_ID, p.EST_Package_ID, prm.PUB_PubRate_Map_ID, c.EST_EstimateMediaType_ID, c.EST_ComponentType_ID, ad.PST_PostalScenario_ID,
	c.AdNumber, c.Description, c.PageCount, c.Width, c.Height, pw.Weight,
	c.CreativeCPP, c.CreativeCPP * c.PageCount, c.SeparatorCPP, c.SeparatorCPP * c.PageCount, ad.ExternalMailQty, c.SpoilagePct, c.PrintCost, c.RunRate, c.NumberOfPlants,
	c.AdditionalPlates, pl.Rate, c.NumberDigitalHandlenPrepare, dh.Rate,
	case
		when c.StitcherMakeready_ID is null then c.StitcherMakereadyRate
		else smr.Rate
	end StitcherMakereadyRate,
	case
		when c.PressMakeready_ID is null then c.PressMakereadyRate
		else prmr.Rate
	end PressMakereadyRate,
	c.ReplacementPlateCost, c.EarlyPayPrintDiscount,
	c.PrinterTaxableMediaPct, c.PrinterSalesTaxPct, c.PaperCost, c.RunPounds, c.MakeReadyPounds, c.PlateChangePounds, c.NumberOfPressStops,
	c.PressStopPounds, pm.CWT, c.EarlyPayPaperDiscount, pr.PaperHandling, c.PaperTaxableMediaPct, c.PaperSalesTaxPct,
	case
		when c.EST_ComponentType_ID = 1 then ml.InternalListRate
		else null
	end InternalMailCPM,
	case
		when c.EST_ComponentType_ID = 1 then ad.ExternalMailCPM
		else null
	end ExternalMailCPM, c.VendorCPM, c.OtherProduction,
	oi.Rate, si.Rate, bi.Rate,
	case
		when c.EST_ComponentType_ID = 1 then mh.InkJetRate
		else null
	end InkJetRate,
	case
		when c.EST_ComponentType_ID = 1 then mh.InkJetMakeReady
		else null
	end InkJetMakeReady,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.CornerGuards = 1 then pr.CornerGuard
		else null
	end CornerGuardRate,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.Skids = 1 then pr.Skid
		else null
	end SkidRate,
	ad.NbrOfCartons,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then mh.TimeValueSlips
		else null
	end TimeValueSlipsCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then mh.AdminFee
		else null
	end GrossMailHouseAdminFee,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseGlueTack = 1 then mh.GlueTackRate
		else null
	end GlueTackCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseTabbing = 1 then mh.TabbingRate
		else null
	end TabbingCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseLetterInsertion = 1 then mh.LetterInsertionRate
		else null
	end LetterInsertionCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then ad.MailHouseOtherHandling
		else null
	end OtherMailHandlingCPM,
	ad.InsertFreightCWT, ad.InsertFuelSurcharge,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then ad.OtherFreight
		else null
	end GrossOtherFreight,
	prm.Pub_ID, admin_pub.Pub_NM, prm.PubLoc_ID, p.PUB_PubQuantityType_ID, dbo.CalcPubRateID(prm.PUB_PubRate_Map_ID, pid.IssueDate),
	e.RunDate, pid.IssueDate,
	case p.PUB_PubQuantityType_ID
		when 4 then 1 /* Thanksgiving always uses Sunday rates */
		when 5 then 1 /* Christmaes always uses Sunday rates */
		when 6 then 1 /* New Years always uses Sunday rates */
		else pid.IssueDOW
	end IssueDOW,
	ad.InsertTime
from EST_Estimate e join #tempEstimate te on e.EST_Estimate_ID = te.EST_Estimate_ID
	join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
	join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
	join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
	join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	join PUB_PubGroup pg on p.PUB_PubGroup_ID = pg.PUB_PubGroup_ID
	join PUB_PubPubGroup_Map ppgm on pg.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID
	join PUB_PubRate_Map prm on ppgm.PUB_PubRate_Map_ID = prm.PUB_PubRate_Map_ID
	join DBADVPROD.informix.pub admin_pub on prm.Pub_ID = admin_pub.Pub_ID
	join EST_PubIssueDates pid on e.EST_Estimate_ID = pid.EST_Estimate_ID and prm.PUB_PubRate_Map_ID = pid.PUB_PubRate_Map_ID
	left join VND_Printer pr on c.Printer_ID = pr.VND_Printer_ID
	left join PRT_PrinterRate pl on c.PlateCost_ID = pl.PRT_PrinterRate_ID /*Plates*/
	left join PRT_PrinterRate dh on c.DigitalHandlenPrepare_ID = dh.PRT_PrinterRate_ID /*Digi H&P*/
	left join PRT_PrinterRate smr on c.StitcherMakeready_ID = smr.PRT_PrinterRate_ID /*Stitcher Makeready*/
	left join PRT_PrinterRate prmr on c.PressMakeready_ID = prmr.PRT_PrinterRate_ID /*Press Makeready*/
	left join PPR_Paper_Map pm on c.Paper_Map_ID = pm.PPR_Paper_Map_ID
	left join PRT_PrinterRate si on c.StitchIn_ID = si.PRT_PrinterRate_ID
	left join PRT_PrinterRate bi on c.BlowIn_ID = bi.PRT_PrinterRate_ID
	left join PRT_PrinterRate oi on c.Onsert_ID = oi.PRT_PrinterRate_ID
	left join VND_MailTrackingRate mt on ad.MailTracking_ID = mt.VND_MailTrackingRate_ID
	join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
	left join VND_MailListResourceRate ml on ad.MailListResource_ID = ml.VND_MailListResourceRate_ID
where c.AdNumber is not null or c.EST_ComponentType_ID = 5

update tcpr
	set
	tcpr.SoloQuantity = tc.SoloQuantity,
	tcpr.OtherQuantity = tc.OtherQuantity,
	tcpr.SampleQuantity = tc.SampleQuantity,
	tcpr.MediaQuantity = tc.MediaQuantity,
	tcpr.TotalEstimateWeight = tc.TotalEstimateWeight
from #tempComponentPubRateMap tcpr join #tempComponent tc on tcpr.EST_Component_ID = tc.EST_Component_ID


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
	set PackageSize = c.Width * c.Height
from #tempComponentPubRateMap tc join EST_PackageComponentMapping pcm on tc.EST_Package_ID = pcm.EST_Package_ID
	join EST_Component c on pcm.EST_Component_ID = c.EST_Component_ID

/* Set the PackageSize to the size of the host component, if the package contains one */
update tc
	set PackageSize = c.Width * c.Height
from #tempComponentPubRateMap tc join EST_PackageComponentMapping pcm on tc.EST_Package_ID = pcm.EST_Package_ID
	join EST_Component c on pcm.EST_Component_ID = c.EST_Component_ID
where c.EST_ComponentType_ID = 1

update tc
	set PolyBagQuantity = isnull(pq.PolybagQuantity, 0)
from #tempComponentPubRateMap tc left join vwComponentPolybagQuantity pq on tc.EST_Component_ID = pq.EST_Component_ID

update #tempComponentPubRateMap
	set SpoilageQuantity = isnull(SpoilagePct, 0) * MediaQuantity,
		DirectMailQuantity = SoloQuantity + PolybagQuantity,
		InternalMailQuantity = (SoloQuantity + PolybagQuantity) - ExternalMailQuantity

update tc
	set PubRateType_ID = pr.PUB_RateType_ID
from #tempComponentPubRateMap tc join PUB_PubRate pr on tc.PubRate_ID = pr.PUB_PubRate_ID

update tc
	set PubRateMapInsertQuantity = isnull((
		select top 1 dowqty.Quantity
		from PUB_PubQuantity ppq join PUB_DayOfWeekQuantity dowqty on ppq.PUB_PubQuantity_ID = dowqty.PUB_PubQuantity_ID
			join PUB_PubQuantityType pqt on dowqty.PUB_PubQuantityType_ID = pqt.PUB_PubQuantityType_ID
		where ppq.PUB_PubRate_Map_ID = tc.PUB_PubRate_Map_ID and dowqty.PUB_PubQuantityType_ID = tc.PubQuantityType_ID
			and (pqt.Special = 1 or tc.IssueDOW = dowqty.InsertDow)
			and ppq.EffectiveDate <= tc.IssueDate
		order by EffectiveDate desc), 0)
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

update #tempComponentPubRateMap
	set TotalProductionQuantity = MediaQuantity + SpoilageQuantity + OtherQuantity + SampleQuantity

update #tempComponentPubRateMap
	set ProductionWeight = TotalProductionQuantity * PieceWeight

/* Perform Production Calculations */
update #tempComponentPubRateMap
	set GrossPrintCost = ManualPrintCost
where ManualPrintCost is not null

update #tempComponentPubRateMap
	set GrossPrintCost = TotalProductionQuantity / cast(1000 as decimal) * isnull(RunRate, 0)
		+ isnull(NumberOfPlants, 0) * (isnull(StitcherMakeReadyRate, 0) + isnull(PressMakeReadyRate, 0))
		+ isnull(AdditionalPlates, 0) * isnull(PrinterPlateCost, 0)
		+ isnull(NumberDigitalHandlePrepare, 0) * isnull(DigitalHandlePrepareRate, 0)
		+ isnull(ReplacementPlateCost, 0)
where ManualPrintCost is null

update #tempComponentPubRateMap
	set EarlyPayPrintDiscountAmount = GrossPrintCost * isnull(EarlyPayPrintDiscountPercent, 0),
		NetPrintCost = GrossPrintCost - (GrossPrintCost * isnull(EarlyPayPrintDiscountPercent, 0))

/*TODO: We don't have a paper handling amount if manualpapercost is entered.  How does this effect the print sales tax?? */
update #tempComponentPubRateMap
	set GrossPaperCost = ManualPaperCost
where ManualPaperCost is not null

update #tempComponentPubRateMap
	set TotalPaperPounds = TotalProductionQuantity / cast(1000 as decimal) * isnull(RunPounds, 0)
		+ cast(isnull(NumberOfPlants, 0) as bigint) * cast(isnull(MakeReadyPounds, 0) as bigint)
		+ cast(isnull(AdditionalPlates, 0) as bigint) * isnull(PlateChangePounds, 0)
		+ cast(isnull(NumberOfPressStops, 0) as bigint) * cast(isnull(PressStopPounds, 0) as bigint)
where ManualPaperCost is null

update #tempComponentPubRateMap
	set GrossPaperCost = isnull(TotalPaperPounds, 0) / cast(100 as decimal) * isnull(PaperCWTRate, 0)
where ManualPaperCost is null

update #tempComponentPubRateMap
	set EarlyPayPaperDiscountAmount = GrossPaperCost * isnull(EarlyPayPaperDiscountPercent, 0),
		NetPaperCost = GrossPaperCost - (GrossPaperCost * isnull(EarlyPayPaperDiscountPercent, 0))

update #tempComponentPubRateMap
	set PaperHandlingCost = isnull(TotalPaperPounds, 0) / cast(100 as decimal) * isnull(PaperHandlingCWTRate, 0)

update #tempComponentPubRateMap
	set PrinterSalesTaxAmount = (NetPrintCost + PaperHandlingCost) * isnull(PrinterTaxableMediaPct, 0) * isnull(PrinterSalesTaxPct, 0),
		PaperSalesTaxAmount = NetPaperCost * isnull(PaperTaxableMediaPct, 0) * isnull(PaperSalesTaxPct, 0)

update #tempComponentPubRateMap
	set TotalPrintCost = NetPrintCost + PrinterSalesTaxAmount,
		TotalPaperCost = NetPaperCost + PaperHandlingCost + PaperSalesTaxAmount,
		VendorProductionCost = TotalProductionQuantity / cast(1000 as decimal) * VendorProductionCPM

update #tempComponentPubRateMap
	set BlendedMailListCPM =
		case
			when dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) = 0 then 0
			else InternalMailCPM
					* cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) - ExternalMailQuantity as decimal)
					/ cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
				+ ExternalMailCPM
					* cast(ExternalMailQuantity as decimal)
					/ cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
		end
where EST_ComponentType_ID = 1

update #tempComponentPubRateMap
	set MailListCost = BlendedMailListCPM * SoloQuantity / 1000 + isnull(dbo.ComponentPolybagMailListCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponentPubRateMap
	set ProductionTotal = isnull(CreativeCost, 0) + isnull(SeparatorCost, 0) + TotalPrintCost + TotalPaperCost +  isnull(MailListCost, 0) +
		+ isnull(VendorProductionCost, 0) + isnull(OtherProductionCost, 0)

/* Assembly Calculations */

update #tempComponentPubRateMap
	set PolyBagCost = dbo.EstimatePolyBagCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1

update #tempComponentPubRateMap
	set OnsertCost = PolybagQuantity / cast(1000 as decimal) * OnsertRate
where EST_ComponentType_ID = 2

update #tempComponentPubRateMap
	set GrossInkjetMakereadyCost = NumberofPlants * InkjetMakereadyRate

update #tempComponentPubRateMap
	set PolyBagTotal = isnull(PolyBagCost, 0) + isnull(OnsertCost, 0),
		StitchInCost =
			case EST_ComponentType_ID
				when 3 then MediaQuantity / cast(1000 as decimal) * StitchInRate
				else null
			end,
		BlowInCost =
			case EST_ComponentType_ID
				when 4 then MediaQuantity / cast(1000 as decimal) * BlowInRate
			end,
		InkJetCost =
			case EST_ComponentType_ID
				when 1 then (isnull(SoloQuantity, 0) / cast(1000 as decimal) * InkJetRate) + isnull(dbo.EstimatePolybagInkjetCost(EST_Estimate_ID), 0)
			end,
		InkJetMakeReadyCost =
			case EST_ComponentType_ID
				when 1 then
					case
						when isnull(SoloQuantity, 0) = 0 then 0
						else GrossInkjetMakereadyCost * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
					end
					+ isnull(dbo.ComponentPolybagInkjetMakereadyCost(EST_Component_ID), 0)
			end

update #tempComponentPubRateMap
		set TotalInkJetCost = isnull(InkJetCost, 0) + isnull(InkJetMakeReadyCost, 0)

update tc
	set CartonRate = pr.Rate
from #tempComponentPubRateMap tc join EST_Component c on tc.EST_Component_ID = c.EST_Component_ID
	join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate pr on p.VND_Printer_ID = pr.VND_Printer_ID and pr.PRT_PrinterRateType_ID = 3 and pr.[Default] = 1

update #tempComponentPubRateMap
	set CornerGuardCost = TotalEstimateWeight / cast(2000 as decimal) * CornerGuardRate,
		SkidCost = TotalEstimateWeight / cast(2000 as decimal) * SkidRate,
		CartonCost = NumberOfCartons * CartonRate,
		TimeValueSlipsCost = TimeValueSlipsCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseTimeValueSlipsCost(EST_Component_ID), 0),
		MailHouseAdminFee =
			case
				when SoloQuantity = 0 then 0
				else GrossMailHouseAdminFee * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
			end
			+ isnull(dbo.ComponentPolybagMailHouseAdminFee(EST_Component_ID), 0),
		GlueTackCost = GlueTackCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseGlueTackCost(EST_Component_ID), 0),
		TabbingCost = TabbingCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseTabbingCost(EST_Component_ID), 0),
		LetterInsertionCost = LetterInsertionCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseLetterInsertionCost(EST_Component_ID), 0),
		OtherMailHandlingCost = OtherMailHandlingCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseOtherDirectMailHandlingCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponentPubRateMap
	set InsertHandlingTotal = isnull(CornerGuardCost, 0) + isnull(SkidCost, 0) + isnull(CartonCost, 0),
		MailHandlingTotal = isnull(TimeValueSlipsCost, 0) + isnull(MailHouseAdminFee, 0) + isnull(GlueTackCost, 0) + isnull(TabbingCost, 0)
			+ isnull(LetterInsertionCost, 0) + isnull(OtherMailHandlingCost, 0)

update #tempComponentPubRateMap
	set HandlingTotal = InsertHandlingTotal + MailHandlingTotal

update #tempComponentPubRateMap
	set AssemblyTotal = PolyBagTotal + isnull(StitchInCost, 0) + isnull(BlowInCost, 0) + TotalInkJetCost + HandlingTotal

/* Calculate the Piece Cost */
update #tempComponentPubRateMap
	set
		ProductionPieceCost =
			case TotalProductionQuantity
				when 0 then 0
				else ProductionTotal / cast(TotalProductionQuantity as decimal)
			end,
		AssemblyPieceCost =
			case MediaQuantity
				when 0 then 0
				else AssemblyTotal / cast(MediaQuantity as decimal)
			end

/* Calculate the Insert Cost */
-- Tab Page Count Rate Type
update tc
	set tc.PubRateMapInsertCost =
		case tc.PackageTabPageCount
			when 0 then 0
			else
				isnull((
			select top 1 wr.Rate
			from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
			where wrt.PUB_PubRate_ID = tc.PubRate_ID and wrt.RateTypeDescription >= tc.PackageTabPageCount and tc.IssueDOW = wr.InsertDow
			order by wrt.RateTypeDescription), 0)
			*
			case pr.QuantityChargeType
				when 1 then tc.PubRateMapInsertQuantity - (tc.PubRateMapInsertQuantity * pr.BilledPct)
				else tc.PubRateMapInsertQuantity
			end
			/ cast (1000 as decimal)
			* cast(tc.ComponentTabPageCount as decimal) / cast(tc.PackageTabPageCount as decimal)
		end
from #tempComponentPubRateMap tc join PUB_PubRate pr on tc.PubRate_ID = pr.PUB_PubRate_ID
where tc.PubRateType_ID = 1

-- Flat Rate Type
update tc
	set tc.PubRateMapInsertCost = isnull((
		select wr.Rate
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = tc.PubRate_ID and tc.IssueDOW = wr.InsertDow), 0) * tc.PieceWeight / tc.PackageWeight
from #tempComponentPubRateMap tc
where tc.PubRateType_ID = 2

-- CPM
update tc
	set tc.PubRateMapInsertCost = isnull((
		select top 1 wr.Rate
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = tc.PubRate_ID and wrt.RateTypeDescription >= tc.PubRateMapInsertQuantity and tc.IssueDOW = wr.InsertDow
		order by wrt.RateTypeDescription), 0)
		*
		case pr.QuantityChargeType
			when 1 then tc.PubRateMapInsertQuantity - (tc.PubRateMapInsertQuantity * pr.BilledPct)
			else tc.PubRateMapInsertQuantity
		end
		/ cast(1000 as decimal)
		* tc.PieceWeight / tc.PackageWeight
from #tempComponentPubRateMap tc join PUB_PubRate pr on tc.PubRate_ID = pr.PUB_PubRate_ID
where tc.PubRateType_ID = 3

-- Weight
update tc
	set tc.PubRateMapInsertCost = isnull((
		select top 1 wr.Rate
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = tc.PubRate_ID and wrt.RateTypeDescription >= tc.PackageWeight and tc.IssueDOW = wr.InsertDow
		order by wrt.RateTypeDescription), 0) * tc.PieceWeight / tc.PackageWeight
from #tempComponentPubRateMap tc
where tc.PubRateType_ID = 4

--Size
update tc
	set tc.PubRateMapInsertCost = isnull((
		select top 1 wr.Rate
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = tc.PubRate_ID and wrt.RateTypeDescription >= tc.PackageSize and tc.IssueDOW = wr.InsertDow
		order by wrt.RateTypeDescription), 0) * tc.PieceWeight / tc.PackageWeight
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
from #tempComponentPubRateMap t join #tempPackageInsertNumber pin on t.EST_Package_ID = pin.EST_Package_ID and t.PUB_PubRate_Map_ID = pin.PUB_PubRate_Map_ID
	join PUB_InsertDiscounts d on t.PubRate_ID = d.PUB_PubRate_ID
where pin.InsertNumber = d.[Insert]

drop table #tempPackageInsertNumber

/*----------------------------------------- */

update tc
	set PubRateMapInsertCost = PubRateMapInsertCost * (1 - isnull(InsertDiscountPercent, 0))
from #tempComponentPubRateMap tc

update #tempComponentPubRateMap
	set InsertFreightCost = TotalEstimateWeight / cast(100 as decimal) * InsertFreightCWT,
		InsertFuelSurchargeCost = (TotalEstimateWeight / cast(100 as decimal) * InsertFreightCWT) * InsertFuelSurchargePercent
where EST_ComponentType_ID = 1

update #tempComponentPubRateMap
	set InsertFreightTotalCost = isnull(InsertFreightCost, 0) + isnull(InsertFuelSurchargeCost, 0)

update #tempComponentPubRateMap
	set OtherFreight = GrossOtherFreight
where EST_ComponentType_ID = 1

update t
	set SampleFreight =
		case
			when isnull(s.FreightCWT, 0) <> 0 then dbo.EstimatePieceWeight(t.EST_Estimate_ID) * SampleQuantity / cast(100 as decimal) * s.FreightCWT
			else s.FreightFlat
		end
from #tempComponentPubRateMap t join EST_Samples s on t.EST_Estimate_ID = s.EST_Estimate_ID
where t.EST_ComponentType_ID = 1

update t
	set SampleFreight = SampleFreight * PubRateMapInsertQuantity / cast(MediaQuantity as decimal)
		, InsertFreightTotalCost = InsertFreightTotalCost * PubRateMapInsertQuantity / cast(MediaQuantity as decimal)
		, OtherFreight = OtherFreight * PubRateMapInsertQuantity / cast(MediaQuantity as decimal)
from #tempComponentPubRateMap t

/* Remove all previous adpub_cost records matching on ad number.  They will be replaced with new data. */
delete p
from adpub_cost p join #tempComponentPubRateMap c on p.AdNumber = c.AdNumber and p.Pub_ID = c.Pub_ID and p.PubLoc_ID = c.PubLoc_ID

/* Insert Ad Pub Costs into adpub_cost table */
insert into adpub_cost(AdNumber, Pub_ID, PubLoc_ID, RunDate, IssueDate, InsertTime, PieceCost, Quantity, CostwoInsert, InsertCost, TotalCost,
	CreatedBy, CreatedDate)
select AdNumber, Pub_ID, PubLoc_ID, RunDate, max(IssueDate) IssueDate,
	
	/* TODO: Remove InsertTime from the table... not required*/
	0 InsertTime,
	/* max(InsertTime) InsertTime, */
	case
		when isnull(sum(TotalProductionQuantity), 0) = 0 then 0
		else isnull(sum(ProductionTotal), 0) / sum(TotalProductionQuantity)
	end
	+
	case
		when isnull(sum(MediaQuantity), 0) = 0 then 0
		else isnull(sum(AssemblyTotal), 0) / sum(MediaQuantity)
	end PieceCost,
	sum(PubRateMapInsertQuantity) Quantity,
	sum((ProductionPieceCost + AssemblyPieceCost) * PubRateMapInsertQuantity)
		+ isnull(sum(SampleFreight), 0)
		+ isnull(sum(InsertFreightTotalCost), 0)
		+ isnull(sum(OtherFreight), 0)
		as CostWithoutInsert,
	isnull(sum(PubRateMapInsertCost), 0) PubRateMapInsertCost,
	sum((ProductionPieceCost + AssemblyPieceCost) * PubRateMapInsertQuantity)
		+ isnull(sum(SampleFreight), 0)
		+ isnull(sum(InsertFreightTotalCost), 0)
		+ isnull(sum(OtherFreight), 0)
		+ isnull(sum(PubRateMapInsertCost), 0)
		as TotalCost,
	@CreatedBy CreatedBy, getdate() CreatedDate
from #tempComponentPubRateMap
group by RunDate, AdNumber, Pub_ID, PubLoc_ID

drop table #tempComponentPubRateMap
drop table #tempComponent
drop table #tempEstimate
set nocount on

GO

GRANT  EXECUTE  ON [dbo].[adpubcost_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.AssocDatabases_s_All') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.AssocDatabases_s_All'
	DROP PROCEDURE dbo.AssocDatabases_s_All
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.AssocDatabases_s_All') IS NOT NULL
		PRINT '***********Drop of dbo.AssocDatabases_s_All FAILED.'
END
GO
PRINT 'Creating dbo.AssocDatabases_s_All'
GO

CREATE PROCEDURE dbo.AssocDatabases_s_All
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Returns a list of Databases ordered by DisplayOrder
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   assoc_databases		Read
*	assoc_databasetype	Read
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/01/2007      NLS             Initial Creation 
* 06/22/2007	  NLS			  Modified to connect to new assoc_databasetype table
*
*/
AS BEGIN

SELECT 
	D.database_id, 
	D.description, 
	D.display, 
	D.displayorder, 
	D.databasetype_id,
	T.description AS databasetype, 
	D.connectionstring, 
	D.databasename,
	D.createdby, 
	D.createddate, 
	D.modifiedby, 
	D.modifieddate 

FROM dbo.assoc_databases AS D INNER JOIN dbo.assoc_databasetype AS T
ON D.databasetype_id = T.databasetype_id

ORDER BY D.displayorder ASC

END
GO

GRANT  EXECUTE  ON [dbo].[AssocDatabases_s_All]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.AssocDatabases_u') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.AssocDatabases_u'
	DROP PROCEDURE dbo.AssocDatabases_u
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.AssocDatabases_u') IS NOT NULL
		PRINT '***********Drop of dbo.AssocDatabases_u FAILED.'
END
GO
PRINT 'Creating dbo.AssocDatabases_u'
GO

CREATE PROCEDURE dbo.AssocDatabases_u
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Updates a assoc_databases record
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   assoc_databases		  Update
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/05/2007      BJS             Initial Creation 
*
*/
@Database_ID int,
@Description varchar(50),
@Display bit,
@DisplayOrder int
as

update assoc_databases
	set
		Description = @Description,
		Display = @Display,
		DisplayOrder = @DisplayOrder
where Database_ID = @Database_ID
GO

GRANT  EXECUTE  ON [dbo].[AssocDatabases_u]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO


GO
IF OBJECT_ID('dbo.db_Copy') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.db_Copy'
	DROP PROCEDURE dbo.db_Copy
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.db_Copy') IS NOT NULL
		PRINT '***********Drop of dbo.db_Copy FAILED.'
END
GO
PRINT 'Creating dbo.db_Copy'
GO

create proc dbo.db_Copy
/*
* PARAMETERS:
*	@SourceDBName
*
* DESCRIPTION:
*	Initiates the copying of data from SourceDBName
*
* TABLES:
* Table Name Access
* ========== ======
* MANY DELETE
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date		Who		Comments
* ----------	----		-------------------------------------------------
* 09/12/2007	BJS		Initial Creation
* 10/08/2007	JRH		Redirect: see db_CopyAsOwner
*
*/
@SourceDBName varchar(50)
as


DECLARE	@job_name sysname
	, @step_name sysname
	, @db_name sysname
	, @job_run_status int

SELECT	@job_name = 'PMES_SyncCopy'
	, @step_name = 'ExecCopyStoredProc'
	, @db_name = DB_NAME()

INSERT dbo.assoc_database_sync (destination_db_name, source_db_name)
	VALUES (@db_name, @SourceDBName)

EXEC msdb..sp_start_pmes_job @job_name, @db_name, @step_name, @job_run_status output

-- Succeeded if run status = 1
IF (@job_run_status = 1)
	return 0
ELSE
	return 1 

GO

GRANT EXECUTE ON [dbo].[db_Copy] TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.db_CopyAsOwner') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.db_CopyAsOwner'
	DROP PROCEDURE dbo.db_CopyAsOwner
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.db_CopyAsOwner') IS NOT NULL
		PRINT '***********Drop of dbo.db_CopyAsOwner FAILED.'
END
GO
PRINT 'Creating dbo.db_CopyAsOwner'
GO

create proc dbo.db_CopyAsOwner
/*
* PARAMETERS:
*	@SourceDBName
*
* DESCRIPTION:
*	Performs the data copy from SourceDBName
*
* TABLES:
* Table Name Access
* ========== ======
* MANY DELETE
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date		Who	Comments
* ----------	---	-------------------------------------------------
* 09/12/2007	BJS	Initial Creation 
* 11/01/2007	JRH	Added assemblyvendor_id to EST_Component
*
*/
as

DECLARE @SourceDBName varchar(50)
	, @sql nvarchar(4000)

SELECT	@SourceDBName = source_db_name
FROM	dbo.assoc_database_sync

set @sql =
'SET IDENTITY_INSERT ppr_papergrade ON
INSERT INTO ppr_papergrade(ppr_papergrade_id, grade, createdby, createddate, modifiedby, modifieddate)
select ppr_papergrade_id, grade, createdby, createddate, modifiedby, modifieddate
from ' + @SourceDBName + '.dbo.ppr_papergrade
SET IDENTITY_INSERT ppr_papergrade OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT ppr_paperweight ON
INSERT INTO ppr_paperweight(ppr_paperweight_id, weight, createdby, createddate, modifiedby, modifieddate)
select ppr_paperweight_id, weight, createdby, createddate, modifiedby, modifieddate
from ' + @SourceDBName + '.dbo.ppr_paperweight
SET IDENTITY_INSERT ppr_paperweight OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pst_postalcategory ON
INSERT INTO pst_postalcategory(pst_postalcategory_id, description, createdby, createddate, modifiedby, modifieddate)
SELECT pst_postalcategory_id, description, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pst_postalcategory
SET IDENTITY_INSERT pst_postalcategory OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pub_insertscenario ON
INSERT INTO pub_insertscenario(pub_insertscenario_id, description, comments, active, createdby, createddate, modifiedby, modifieddate)
SELECT pub_insertscenario_id, description, comments, active, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pub_insertscenario
SET IDENTITY_INSERT pub_insertscenario OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pub_pubgroup ON
INSERT INTO pub_pubgroup(pub_pubgroup_id, description, comments, active, effectivedate, sortorder, customgroupforpackage, createdby, createddate, modifiedby, modifieddate)
SELECT pub_pubgroup_id, description, comments, active, effectivedate, sortorder, customgroupforpackage, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pub_pubgroup
SET IDENTITY_INSERT pub_pubgroup OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT vnd_vendor ON
INSERT INTO vnd_vendor(vnd_vendor_id, vendorcode, description, active, createdby, createddate, modifiedby, modifieddate)
SELECT vnd_vendor_id, vendorcode, description, active, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.vnd_vendor
SET IDENTITY_INSERT vnd_vendor OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT est_estimate ON
INSERT INTO est_estimate(est_estimate_id, description, comments, est_season_id, fiscalyear, rundate, est_status_id, fiscalmonth, parent_id, uploaddate, createdby, createddate, modifiedby, modifieddate)
SELECT est_estimate_id, description, comments, est_season_id, fiscalyear, rundate, est_status_id, fiscalmonth, parent_id, uploaddate, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.est_estimate
SET IDENTITY_INSERT est_estimate OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pst_postalscenario ON
INSERT INTO pst_postalscenario(pst_postalscenario_id, description, comments, effectivedate, pst_postalmailertype_id, pst_postalclass_id, createdby, createddate, modifiedby, modifieddate)
SELECT pst_postalscenario_id, description, comments, effectivedate, pst_postalmailertype_id, pst_postalclass_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pst_postalscenario
SET IDENTITY_INSERT pst_postalscenario OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pst_postalweights ON
INSERT INTO pst_postalweights(pst_postalweights_id, firstoverweightlimit, standardoverweightlimit, effectivedate, vnd_vendor_id, createdby, createddate, modifiedby, modifieddate)
SELECT pst_postalweights_id, firstoverweightlimit, standardoverweightlimit, effectivedate, vnd_vendor_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pst_postalweights
SET IDENTITY_INSERT pst_postalweights OFF'
exec sp_executesql @sql

set @sql =
'INSERT INTO pub_groupinsertscenario_map
SELECT *
FROM ' + @SourceDBName + '.dbo.pub_groupinsertscenario_map'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pub_pubrate_map ON
INSERT INTO pub_pubrate_map(pub_pubrate_map_id, pub_id, publoc_id, createdby, createddate, modifiedby, modifieddate)
SELECT pub_pubrate_map_id, pub_id, publoc_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pub_pubrate_map
SET IDENTITY_INSERT pub_pubrate_map OFF'
exec sp_executesql @sql

set @sql =
'INSERT INTO vnd_vendorvendortype_map
SELECT *
FROM ' + @SourceDBName + '.dbo.vnd_vendorvendortype_map'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT vnd_mailhouserate ON
INSERT INTO vnd_mailhouserate(vnd_mailhouserate_id, vnd_vendor_id, timevalueslips, inkjetrate, inkjetmakeready, adminfee, postaldropcwt, gluetackdefault, gluetackrate, tabbingdefault, tabbingrate, letterinsertiondefault, letterinsertionrate, effectivedate, createdby, createddate, modifiedby, modifieddate)
SELECT vnd_mailhouserate_id, vnd_vendor_id, timevalueslips, inkjetrate, inkjetmakeready, adminfee, postaldropcwt, gluetackdefault, gluetackrate, tabbingdefault, tabbingrate, letterinsertiondefault, letterinsertionrate, effectivedate, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.vnd_mailhouserate
SET IDENTITY_INSERT vnd_mailhouserate OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT vnd_maillistresourcerate ON
INSERT INTO vnd_maillistresourcerate(vnd_maillistresourcerate_id, vnd_vendor_id, internallistrate, effectivedate, createdby, createddate, modifiedby, modifieddate)
SELECT vnd_maillistresourcerate_id, vnd_vendor_id, internallistrate, effectivedate, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.vnd_maillistresourcerate
SET IDENTITY_INSERT vnd_maillistresourcerate OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT vnd_mailtrackingrate ON
INSERT INTO vnd_mailtrackingrate(vnd_mailtrackingrate_id, vnd_vendor_id, mailtracking, effectivedate, createdby, createddate, modifiedby, modifieddate)
SELECT vnd_mailtrackingrate_id, vnd_vendor_id, mailtracking, effectivedate, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.vnd_mailtrackingrate
SET IDENTITY_INSERT vnd_mailtrackingrate OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT vnd_paper ON
INSERT INTO vnd_paper(vnd_paper_id, effectivedate, vnd_vendor_id, createdby, createddate, modifiedby, modifieddate)
SELECT vnd_paper_id, effectivedate, vnd_vendor_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.vnd_paper
SET IDENTITY_INSERT vnd_paper OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT vnd_printer ON
INSERT INTO vnd_printer(vnd_printer_id, vnd_vendor_id, paperhandling, polybagbagweight, cornerguarddefault, cornerguard, skiddefault, skid, polybagmessagedefault, polybagmessage, polybagmessagemakereadydefault, polybagmessagemakeready, effectivedate, createdby, createddate, modifiedby, modifieddate)
SELECT vnd_printer_id, vnd_vendor_id, paperhandling, polybagbagweight, cornerguarddefault, cornerguard, skiddefault, skid, polybagmessagedefault, polybagmessage, polybagmessagemakereadydefault, polybagmessagemakeready, effectivedate, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.vnd_printer
SET IDENTITY_INSERT vnd_printer OFF'
exec sp_executesql @sql

set @sql =
'INSERT INTO est_assemdistriboptions(est_estimate_id, insertdow, insertfreightvendor_id, insertfreightcwt, insertfuelsurcharge, cornerguards, skids, inserttime, pst_postalscenario_id, mailfuelsurcharge, mailhouse_id, mailhouseotherhandling, usemailtracking, mailtracking_id, maillistresource_id, useexternalmaillist, externalmailqty, externalmailcpm, nbrofcartons, usegluetack, usetabbing, useletterinsertion, firstclass, otherfreight, postaldropflat, createdby, createddate, modifiedby, modifieddate)
SELECT est_estimate_id, insertdow, insertfreightvendor_id, insertfreightcwt, insertfuelsurcharge, cornerguards, skids, inserttime, pst_postalscenario_id, mailfuelsurcharge, mailhouse_id, mailhouseotherhandling, usemailtracking, mailtracking_id, maillistresource_id, useexternalmaillist, externalmailqty, externalmailcpm, nbrofcartons, usegluetack, usetabbing, useletterinsertion, firstclass, otherfreight, postaldropflat, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.est_assemdistriboptions'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT est_package ON
INSERT INTO est_package(est_package_id, est_estimate_id, description, comments, soloquantity, otherquantity, pub_pubquantitytype_id, pub_pubgroup_id, createdby, createddate, modifiedby, modifieddate)
SELECT est_package_id, est_estimate_id, description, comments, soloquantity, otherquantity, pub_pubquantitytype_id, pub_pubgroup_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.est_package
SET IDENTITY_INSERT est_package OFF'
exec sp_executesql @sql

set @sql =
'INSERT INTO est_pubissuedates
SELECT *
FROM ' + @SourceDBName + '.dbo.est_pubissuedates'
exec sp_executesql @sql

set @sql =
'INSERT INTO est_samples
SELECT *
FROM ' + @SourceDBName + '.dbo.est_samples'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT ppr_paper_map ON
INSERT INTO ppr_paper_map(ppr_paper_map_id, description, cwt, [default], ppr_papergrade_id, ppr_paperweight_id, vnd_paper_id, createdby, createddate, modifiedby, modifieddate)
SELECT ppr_paper_map_id, description, cwt, [default], ppr_papergrade_id, ppr_paperweight_id, vnd_paper_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.ppr_paper_map
SET IDENTITY_INSERT ppr_paper_map OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT prt_printerrate ON
INSERT INTO prt_printerrate(prt_printerrate_id, vnd_printer_id, prt_printerratetype_id, rate, description, [default], createdby, createddate, modifiedby, modifieddate)
SELECT prt_printerrate_id, vnd_printer_id, prt_printerratetype_id, rate, description, [default], createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.prt_printerrate
SET IDENTITY_INSERT prt_printerrate OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pst_postalcategoryrate_map ON
INSERT INTO pst_postalcategoryrate_map(pst_postalcategoryrate_map_id, pst_postalmailertype_id, pst_postalclass_id, pst_postalcategory_id, active, underweightpiecerate, overweightpoundrate, overweightpiecerate, pst_postalweights_id, createdby, createddate, modifiedby, modifieddate)
SELECT pst_postalcategoryrate_map_id, pst_postalmailertype_id, pst_postalclass_id, pst_postalcategory_id, active, underweightpiecerate, overweightpoundrate, overweightpiecerate, pst_postalweights_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pst_postalcategoryrate_map
SET IDENTITY_INSERT pst_postalcategoryrate_map OFF'
exec sp_executesql @sql

set @sql =
'INSERT INTO pub_pubpubgroup_map
SELECT *
FROM ' + @SourceDBName + '.dbo.pub_pubpubgroup_map'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pub_pubquantity ON
INSERT INTO pub_pubquantity(pub_pubquantity_id, effectivedate, pub_pubrate_map_id, createdby, createddate, modifiedby, modifieddate)
SELECT pub_pubquantity_id, effectivedate, pub_pubrate_map_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pub_pubquantity
SET IDENTITY_INSERT pub_pubquantity OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pub_pubrate ON
INSERT INTO pub_pubrate(pub_pubrate_id, pub_ratetype_id, chargeblowin, blowinrate, effectivedate, quantitychargetype, billedpct, pub_pubrate_map_id, createdby, createddate, modifiedby, modifieddate)
SELECT pub_pubrate_id, pub_ratetype_id, chargeblowin, blowinrate, effectivedate, quantitychargetype, billedpct, pub_pubrate_map_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pub_pubrate
SET IDENTITY_INSERT pub_pubrate OFF'
exec sp_executesql @sql

set @sql =
'INSERT INTO pub_pubrate_map_activate
SELECT *
FROM ' + @SourceDBName + '.dbo.pub_pubrate_map_activate'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT est_component ON
INSERT INTO est_component(est_component_id, est_estimate_id, description, comments, financialchangecomment, adnumber, est_estimatemediatype_id, est_componenttype_id, mediaqtywoinsert, spoilagepct, pagecount, width, height, otherproduction, vendorsupplied, vendorsupplied_id, vendorcpm, creativevendor_id, creativecpp, separator_id, separatorcpp, printer_id, calculateprintcost, printcost, numberofplants, additionalplates, platecost_id, replacementplatecost, runrate, numberdigitalhandlenprepare, digitalhandlenprepare_id, stitchin_id, blowin_id, onsert_id, stitchermakeready_id, stitchermakereadyrate, pressmakeready_id, pressmakereadyrate, earlypayprintdiscount, printerapplytax, printertaxablemediapct, printersalestaxpct, paper_id, paper_map_id, paperweight_id, papergrade_id, calculatepapercost, papercost, runpounds, makereadypounds, platechangepounds, pressstoppounds, numberofpressstops, earlypaypaperdiscount, paperapplytax, papertaxablemediapct, papersalestaxpct, createdby, createddate, modifiedby, modifieddate, assemblyvendor_id)
SELECT est_component_id, est_estimate_id, description, comments, financialchangecomment, adnumber, est_estimatemediatype_id, est_componenttype_id, mediaqtywoinsert, spoilagepct, pagecount, width, height, otherproduction, vendorsupplied, vendorsupplied_id, vendorcpm, creativevendor_id, creativecpp, separator_id, separatorcpp, printer_id, calculateprintcost, printcost, numberofplants, additionalplates, platecost_id, replacementplatecost, runrate, numberdigitalhandlenprepare, digitalhandlenprepare_id, stitchin_id, blowin_id, onsert_id, stitchermakeready_id, stitchermakereadyrate, pressmakeready_id, pressmakereadyrate, earlypayprintdiscount, printerapplytax, printertaxablemediapct, printersalestaxpct, paper_id, paper_map_id, paperweight_id, papergrade_id, calculatepapercost, papercost, runpounds, makereadypounds, platechangepounds, pressstoppounds, numberofpressstops, earlypaypaperdiscount, paperapplytax, papertaxablemediapct, papersalestaxpct, createdby, createddate, modifiedby, modifieddate, assemblyvendor_id
FROM ' + @SourceDBName + '.dbo.est_component
SET IDENTITY_INSERT est_component OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT est_polybaggroup ON
INSERT INTO est_polybaggroup(est_polybaggroup_id, description, comments, vnd_printer_id, prt_bagrate_id, prt_bagmakereadyrate_id, usemessage, createdby, createddate, modifiedby, modifieddate)
SELECT est_polybaggroup_id, description, comments, vnd_printer_id, prt_bagrate_id, prt_bagmakereadyrate_id, usemessage, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.est_polybaggroup
SET IDENTITY_INSERT est_polybaggroup OFF'
exec sp_executesql @sql

set @sql =
'INSERT INTO pst_postalcategoryscenario_map(pst_postalscenario_id, pst_postalcategoryrate_map_id, percentage, createdby, createddate, modifiedby, modifieddate)
SELECT pst_postalscenario_id, pst_postalcategoryrate_map_id, percentage, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pst_postalcategoryscenario_map'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pub_dayofweekquantity ON
INSERT INTO pub_dayofweekquantity(pub_dayofweekquantity_id, pub_pubquantity_id, pub_pubquantitytype_id, insertdow, quantity, createdby, createddate, modifiedby, modifieddate)
SELECT pub_dayofweekquantity_id, pub_pubquantity_id, pub_pubquantitytype_id, insertdow, quantity, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pub_dayofweekquantity
SET IDENTITY_INSERT pub_dayofweekquantity OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pub_dayofweekratetypes ON
INSERT INTO pub_dayofweekratetypes(pub_dayofweekratetypes_id, ratetypedescription, pub_pubrate_id, createdby, createddate, modifiedby, modifieddate)
SELECT pub_dayofweekratetypes_id, ratetypedescription, pub_pubrate_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pub_dayofweekratetypes
SET IDENTITY_INSERT pub_dayofweekratetypes OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pub_insertdiscounts ON
INSERT INTO pub_insertdiscounts(pub_insertdiscount_id, pub_pubrate_id, [insert], discount, createdby, createddate, modifiedby, modifieddate)
SELECT pub_insertdiscount_id, pub_pubrate_id, [insert], discount, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pub_insertdiscounts
SET IDENTITY_INSERT pub_insertdiscounts OFF'
exec sp_executesql @sql

set @sql =
'INSERT INTO est_estimatepolybaggroup_map
SELECT *
FROM ' + @SourceDBName + '.dbo.est_estimatepolybaggroup_map'
exec sp_executesql @sql

set @sql =
'INSERT INTO est_packagecomponentmapping
SELECT *
FROM ' + @SourceDBName + '.dbo.est_packagecomponentmapping'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT est_polybag ON
INSERT INTO est_polybag(est_polybag_id, est_polybaggroup_id, pst_postalscenario_id, quantity, createdby, createddate, modifiedby, modifieddate)
SELECT est_polybag_id, est_polybaggroup_id, pst_postalscenario_id, quantity, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.est_polybag
SET IDENTITY_INSERT est_polybag OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pub_dayofweekrates ON
INSERT INTO pub_dayofweekrates(pub_dayofweekrates_id, pub_dayofweekratetypes_id, rate, insertdow, createdby, createddate, modifiedby, modifieddate)
SELECT pub_dayofweekrates_id, pub_dayofweekratetypes_id, rate, insertdow, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pub_dayofweekrates
SET IDENTITY_INSERT pub_dayofweekrates OFF'
exec sp_executesql @sql

set @sql =
'INSERT INTO est_packagepolybag_map
SELECT *
FROM ' + @SourceDBName + '.dbo.est_packagepolybag_map'
exec sp_executesql @sql

DELETE	dbo.assoc_database_sync

GO

GRANT EXECUTE ON [dbo].[db_CopyAsOwner] TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.db_Purge') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.db_Purge'
	DROP PROCEDURE dbo.db_Purge
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.db_Purge') IS NOT NULL
		PRINT '***********Drop of dbo.db_Purge FAILED.'
END
GO
PRINT 'Creating dbo.db_Purge'
GO

CREATE PROCEDURE dbo.db_Purge
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Initiates the purging of the database of all non-essential data
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   MANY                DELETE
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date		Who		Comments
* ----------	----		-------------------------------------------------
* 09/12/2007	BJS		Initial Creation
* 09/26/2007	BJS		Modified reference to EST_PubIssueDates
* 10/08/2007	JRH		Redirect: see db_PurgeAsOwner
*
*/
AS

DECLARE	@job_name sysname
	, @step_name sysname
	, @db_name sysname
	, @job_run_status int

SELECT	@job_name = 'PMES_SyncPurge'
	, @step_name = 'ExecPurgeStoredProc'
	, @db_name = DB_NAME()

EXEC msdb..sp_start_pmes_job @job_name, @db_name, @step_name, @job_run_status output

-- Succeeded if run status = 1
IF (@job_run_status = 1)
	return 0
ELSE
	return 1 

GO
GRANT  EXECUTE  ON [dbo].[db_Purge]  TO [PMES_SuperAdmin], [PMES_RateAdmin]
GO
GO
IF OBJECT_ID('dbo.db_PurgeAsOwner') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.db_PurgeAsOwner'
	DROP PROCEDURE dbo.db_PurgeAsOwner
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.db_PurgeAsOwner') IS NOT NULL
		PRINT '***********Drop of dbo.db_PurgeAsOwner FAILED.'
END
GO
PRINT 'Creating dbo.db_PurgeAsOwner'
GO

CREATE PROCEDURE dbo.db_PurgeAsOwner
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Performs the database purging of all non-essential data.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   MANY                DELETE
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date		Who		Comments
* ----------	----		-------------------------------------------------
* 10/08/2007	JRH		Redirected from db_Purge
*
*/
AS

TRUNCATE TABLE est_packagepolybag_map
TRUNCATE TABLE pub_dayofweekrates

DELETE FROM est_polybag
DBCC CHECKIDENT('est_polybag', RESEED, 0)
DBCC CHECKIDENT('est_polybag')

TRUNCATE TABLE est_packagecomponentmapping
TRUNCATE TABLE est_estimatepolybaggroup_map

DELETE FROM pub_insertdiscounts
DBCC CHECKIDENT('pub_insertdiscounts', RESEED, 0)
DBCC CHECKIDENT('pub_insertdiscounts', RESEED)

DELETE FROM pub_dayofweekratetypes
DBCC CHECKIDENT('pub_dayofweekratetypes', RESEED, 0)
DBCC CHECKIDENT('pub_dayofweekratetypes', RESEED)

DELETE FROM pub_dayofweekquantity
DBCC CHECKIDENT('pub_dayofweekquantity', RESEED, 0)
DBCC CHECKIDENT('pub_dayofweekquantity', RESEED)

TRUNCATE TABLE pst_postalcategoryscenario_map

DELETE FROM est_polybaggroup
DBCC CHECKIDENT('est_polybaggroup', RESEED, 0)
DBCC CHECKIDENT('est_polybaggroup', RESEED)

DELETE FROM est_component
DBCC CHECKIDENT('est_component', RESEED, 0)
DBCC CHECKIDENT('est_component', RESEED)

TRUNCATE TABLE pub_pubrate_map_activate

DELETE FROM pub_pubrate
DBCC CHECKIDENT('pub_pubrate', RESEED, 0)
DBCC CHECKIDENT('pub_pubrate', RESEED)

DELETE FROM pub_pubquantity
DBCC CHECKIDENT('pub_pubquantity', RESEED, 0)
DBCC CHECKIDENT('pub_pubquantity', RESEED)

TRUNCATE TABLE pub_pubpubgroup_map

DELETE FROM pst_postalcategoryrate_map
DBCC CHECKIDENT('pst_postalcategoryrate_map', RESEED, 0)
DBCC CHECKIDENT('pst_postalcategoryrate_map', RESEED)

DELETE FROM prt_printerrate
DBCC CHECKIDENT('prt_printerrate', RESEED, 0)
DBCC CHECKIDENT('prt_printerrate', RESEED)

DELETE FROM ppr_paper_map
DBCC CHECKIDENT('ppr_paper_map', RESEED, 0)
DBCC CHECKIDENT('ppr_paper_map', RESEED)

TRUNCATE TABLE est_samples

TRUNCATE TABLE est_pubissuedates

DELETE FROM est_package
DBCC CHECKIDENT('est_package', RESEED, 0)
DBCC CHECKIDENT('est_package', RESEED)

TRUNCATE TABLE est_assemdistriboptions

DELETE FROM vnd_printer
DBCC CHECKIDENT('vnd_printer', RESEED, 0)
DBCC CHECKIDENT('vnd_printer', RESEED)

DELETE FROM vnd_paper
DBCC CHECKIDENT('vnd_paper', RESEED, 0)
DBCC CHECKIDENT('vnd_paper', RESEED)

DELETE FROM vnd_mailtrackingrate
DBCC CHECKIDENT('vnd_mailtrackingrate', RESEED, 0)
DBCC CHECKIDENT('vnd_mailtrackingrate', RESEED)

DELETE FROM vnd_maillistresourcerate
DBCC CHECKIDENT('vnd_maillistresourcerate', RESEED, 0)
DBCC CHECKIDENT('vnd_maillistresourcerate', RESEED)

DELETE FROM vnd_mailhouserate
DBCC CHECKIDENT('vnd_mailhouserate', RESEED, 0)
DBCC CHECKIDENT('vnd_mailhouserate', RESEED)

TRUNCATE TABLE vnd_vendorvendortype_map
TRUNCATE TABLE rpt_report

DELETE FROM pub_pubrate_map
DBCC CHECKIDENT('pub_pubrate_map', RESEED, 0)
DBCC CHECKIDENT('pub_pubrate_map', RESEED)

TRUNCATE TABLE pub_groupinsertscenario_map

DELETE FROM pst_postalweights
DBCC CHECKIDENT('pst_postalweights', RESEED, 0)
DBCC CHECKIDENT('pst_postalweights', RESEED)

DELETE FROM pst_postalscenario
DBCC CHECKIDENT('pst_postalscenario', RESEED, 0)
DBCC CHECKIDENT('pst_postalscenario', RESEED)

DELETE FROM est_estimate
DBCC CHECKIDENT('est_estimate', RESEED, 0)
DBCC CHECKIDENT('est_estimate', RESEED)

DELETE FROM vnd_vendor
DBCC CHECKIDENT('vnd_vendor', RESEED, 0)
DBCC CHECKIDENT('vnd_vendor', RESEED)

DELETE FROM pub_pubgroup
DBCC CHECKIDENT('pub_pubgroup', RESEED, 0)
DBCC CHECKIDENT('pub_pubgroup', RESEED)

DELETE FROM pub_insertscenario
DBCC CHECKIDENT('pub_insertscenario', RESEED, 0)
DBCC CHECKIDENT('pub_insertscenario', RESEED)

DELETE FROM pst_postalcategory
DBCC CHECKIDENT('pst_postalcategory', RESEED, 0)
DBCC CHECKIDENT('pst_postalcategory', RESEED)

DELETE FROM ppr_paperweight
DBCC CHECKIDENT('ppr_paperweight', RESEED, 0)
DBCC CHECKIDENT('ppr_paperweight', RESEED)

DELETE FROM ppr_papergrade
DBCC CHECKIDENT('ppr_papergrade', RESEED, 0)
DBCC CHECKIDENT('ppr_papergrade', RESEED)
GO

GRANT  EXECUTE  ON [dbo].[db_PurgeAsOwner]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.db_Purge_ByDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.db_Purge_ByDate'
	DROP PROCEDURE dbo.db_Purge_ByDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.db_Purge_ByDate') IS NOT NULL
		PRINT '***********Drop of dbo.db_Purge_ByDate FAILED.'
END
GO
PRINT 'Creating dbo.db_Purge_ByDate'
GO

CREATE PROCEDURE dbo.db_Purge_ByDate
/*
* PARAMETERS:
*	@PurgeDate - Date that is passed in to purge based off of.
*
* DESCRIPTION:
*	Purges the database of all estimates and unused rates that exist prior to the given date
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   MANY                DELETE
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/10/2007      NLS             Initial Creation
* 09/12/2007	  NLS			  Renamed proc and made changes to tables affected 
* 09/26/2007      NLS             Added Report purging
* 10/05/2007      NLS             Change order on estimate delete so children purged before parents
* 10/05/2007      NLS             Fixed up Publication Rate Purge
*
*/

@PurgeDate datetime

AS

-- Used when performing some transactional processing to determine whether to rollback
-- a transaction based on a partial failure on a delete
DECLARE @ErrorFlag bit

----------------------------------------------------------------------------------------------
-- Find all estimates that need to be purged based on the RunDate
-- Be sure to delete the estimates that have a parent first
----------------------------------------------------------------------------------------------
DECLARE @purge_estimate_id bigint
DECLARE EstimateCursor CURSOR LOCAL FORWARD_ONLY FOR
    SELECT est_estimate_id FROM Est_Estimate WHERE RunDate <= @PurgeDate
    ORDER BY isnull(parent_id, -1) DESC

OPEN EstimateCursor
FETCH NEXT FROM EstimateCursor INTO @purge_estimate_id
WHILE @@FETCH_STATUS = 0
BEGIN

	BEGIN TRAN EstimateTran
	SET @ErrorFlag = 0

	-- Delete from the tables where there are no dependencies except on est_estimate_id
	DELETE FROM est_samples WHERE est_estimate_id = @purge_estimate_id
	DELETE FROM est_assemdistriboptions WHERE est_estimate_id = @purge_estimate_id
	DELETE FROM est_pubissuedates WHERE est_estimate_id = @purge_estimate_id

	-- Delete everything that depends on a package
	DECLARE @purge_package_id bigint
    DECLARE PackageCursor CURSOR LOCAL FORWARD_ONLY FOR
    	SELECT est_package_id FROM Est_Package WHERE est_estimate_id <= @purge_estimate_id

	OPEN PackageCursor	
	FETCH NEXT FROM PackageCursor INTO @purge_package_id
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		DELETE FROM est_packagepolybag_map WHERE est_package_id = @purge_package_id
		DELETE FROM est_packagecomponentmapping WHERE est_package_id = @purge_package_id
	
		FETCH NEXT FROM PackageCursor INTO @purge_package_id
	END
	CLOSE PackageCursor
	DEALLOCATE PackageCursor
	
	-- Delete the Component Dependencies
	DELETE pcm FROM est_packagecomponentmapping pcm
		LEFT JOIN est_component c ON pcm.est_component_id = pcm.est_component_id
	WHERE c.est_estimate_id = @purge_estimate_id
	
	-- Now delete the packages, components and polybag maps now that the dependencies are gone
	DELETE FROM est_package WHERE est_estimate_id = @purge_estimate_id
	DELETE FROM est_component WHERE est_estimate_id = @purge_estimate_id
	DELETE FROM est_estimatepolybaggroup_map WHERE est_estimate_id = @purge_estimate_id
	
	-- Finally delete the estimate
	DELETE FROM est_estimate WHERE est_estimate_id = @purge_estimate_id
	
    -- Deleting an estimate could fail if all it's children weren't deleted
    -- in this case, rollback the estimate delete because you can't delete a parent
    -- if any children didn't meet the purge criteria
	IF (@@ERROR <> 0) BEGIN
		ROLLBACK TRAN EstimateTran
		SET @ErrorFlag = 1
	END
	
	IF (@ErrorFlag = 0) BEGIN
		COMMIT TRAN EstimateTran
	END

	-- Next Estimate
	FETCH NEXT FROM EstimateCursor INTO @purge_estimate_id
END

CLOSE EstimateCursor
DEALLOCATE EstimateCursor

----------------------------------------------------------------------------------------------
-- Now delete any Polybag Groups that are empty as a result of the estimate purge
----------------------------------------------------------------------------------------------
DECLARE @purge_polybaggroup_id bigint

DECLARE PolybagGroupCursor CURSOR LOCAL FORWARD_ONLY FOR
    SELECT pbg.est_polybaggroup_id FROM Est_PolybagGroup pbg
	    LEFT JOIN est_estimatepolybaggroup_map map ON pbg.est_polybaggroup_id = map.est_polybaggroup_id
	WHERE map.est_polybaggroup_id is null

OPEN PolybagGroupCursor
FETCH NEXT FROM PolybagGroupCursor INTO @purge_polybaggroup_id
WHILE @@FETCH_STATUS = 0
BEGIN
	DELETE FROM est_polybag WHERE est_polybaggroup_id = @purge_polybaggroup_id
	DELETE FROM est_polybaggroup WHERE est_polybaggroup_id = @purge_polybaggroup_id
	
	FETCH NEXT FROM PolybagGroupCursor INTO @purge_polybaggroup_id
END

CLOSE PolybagGroupCursor
DEALLOCATE PolybagGroupCursor

----------------------------------------------------------------------------------------------
-- Now find all rates that are effective before the purge date, but are not used by an estimate
----------------------------------------------------------------------------------------------

-- Mailhouse Rates
DELETE mhr FROM vnd_mailhouserate mhr
	LEFT JOIN est_assemdistriboptions ado ON mhr.vnd_mailhouserate_id = ado.mailhouse_id
WHERE ado.mailhouse_id is null AND mhr.effectivedate <= @PurgeDate

-- Paper Rates
DECLARE @purge_paper_id bigint

DECLARE PaperCursor CURSOR LOCAL FORWARD_ONLY FOR
    SELECT vnd_paper_id FROM vnd_paper WHERE effectivedate <= @PurgeDate

OPEN PaperCursor
FETCH NEXT FROM PaperCursor INTO @purge_paper_id
WHILE @@FETCH_STATUS = 0
BEGIN

	-- Attempt to delete the Paper Map and Paper Record.  If there is any failure, then rollback
	-- the entire tran for this rate
	BEGIN TRAN PaperTran
	SET @ErrorFlag = 0
	
	DELETE FROM ppr_paper_map WHERE vnd_paper_id = @purge_paper_id
	IF (@@ERROR <> 0) BEGIN
		ROLLBACK TRAN PaperTran
		SET @ErrorFlag = 1
	END
	
	IF (@ErrorFlag = 0) BEGIN
		DELETE FROM vnd_paper WHERE vnd_paper_id = @purge_paper_id
		IF (@@ERROR <> 0) BEGIN
			ROLLBACK TRAN PaperTran
			SET @ErrorFlag = 1
		END
	END
	
	IF (@ErrorFlag = 0) BEGIN
		COMMIT TRAN PaperTran
	END
	
	FETCH NEXT FROM PaperCursor INTO @purge_paper_id
END

CLOSE PaperCursor
DEALLOCATE PaperCursor

-- Mail Tracking Rates
DELETE mtr FROM vnd_mailtrackingrate mtr
	LEFT JOIN est_assemdistriboptions ado ON mtr.vnd_mailtrackingrate_id = ado.mailtracking_id
WHERE ado.mailtracking_id is null AND mtr.effectivedate <= @PurgeDate

-- Maillist Resource Rates
DELETE mlr FROM vnd_maillistresourcerate mlr
	LEFT JOIN est_assemdistriboptions ado ON mlr.vnd_maillistresourcerate_id = ado.maillistresource_id
WHERE ado.maillistresource_id is null AND mlr.effectivedate <= @PurgeDate

-- Printer Rates
DECLARE @purge_printer_id bigint

DECLARE PrinterCursor CURSOR LOCAL FORWARD_ONLY FOR
    SELECT vnd_printer_id FROM vnd_printer WHERE effectivedate <= @PurgeDate

OPEN PrinterCursor
FETCH NEXT FROM PrinterCursor INTO @purge_printer_id
WHILE @@FETCH_STATUS = 0
BEGIN

	-- Attempt to delete the Printer Rate and Paper Record.  If there is any failure, then rollback
	-- the entire tran for this rate
	BEGIN TRAN PrinterTran
	SET @ErrorFlag = 0
	
	DELETE FROM prt_printerrate WHERE vnd_printer_id = @purge_printer_id
	IF (@@ERROR <> 0) BEGIN
		ROLLBACK TRAN PrinterTran
		SET @ErrorFlag = 1
	END
	
	IF (@ErrorFlag = 0) BEGIN
		DELETE FROM vnd_printer WHERE vnd_printer_id = @purge_printer_id
		IF (@@ERROR <> 0) BEGIN
			ROLLBACK TRAN PrinterTran
			SET @ErrorFlag = 1
		END
	END
	
	IF (@ErrorFlag = 0) BEGIN
		COMMIT TRAN PrinterTran
	END
	
	FETCH NEXT FROM PrinterCursor INTO @purge_printer_id
END

CLOSE PrinterCursor
DEALLOCATE PrinterCursor

----------------------------------------------------------------------------------------------
-- Postal Scenarios that aren't used by polybags and meet the Purge Date criteria
----------------------------------------------------------------------------------------------
DECLARE @purge_postalscenario_id bigint

DECLARE PostalScenarioCursor CURSOR LOCAL FORWARD_ONLY FOR
    SELECT pst.pst_postalscenario_id FROM pst_postalscenario pst
	    LEFT JOIN est_polybag pb ON pst.pst_postalscenario_id = pb.pst_postalscenario_id
	WHERE pb.pst_postalscenario_id is null AND pst.effectivedate <= @PurgeDate

OPEN PostalScenarioCursor
FETCH NEXT FROM PostalScenarioCursor INTO @purge_postalscenario_id
WHILE @@FETCH_STATUS = 0
BEGIN

	DELETE FROM pst_postalcategoryscenario_map WHERE pst_postalscenario_id = @purge_postalscenario_id
	DELETE FROM pst_postalscenario WHERE pst_postalscenario_id = @purge_postalscenario_id

	FETCH NEXT FROM PostalScenarioCursor INTO @purge_postalscenario_id
END

CLOSE PostalScenarioCursor
DEALLOCATE PostalScenarioCursor

-- Postal Rates that aren't being used by a postal scenario and also meet the date criteria
DECLARE @purge_postalweight_id bigint

DECLARE PostalRatesCursor CURSOR LOCAL FORWARD_ONLY FOR
    SELECT pst_postalweights_id FROM pst_postalweights WHERE effectivedate <= @PurgeDate

OPEN PostalRatesCursor
FETCH NEXT FROM PostalRatesCursor INTO @purge_postalweight_id
WHILE @@FETCH_STATUS = 0
BEGIN
	
	-- Attempt to delete the Postal Rate Records.  If there is any failure, then rollback
	-- the entire tran for this rate.  Might fail because it's linked to a scenario which is still active.
	BEGIN TRAN PostalRatesTran
	SET @ErrorFlag = 0
	
	DELETE FROM pst_postalcategoryrate_map WHERE pst_postalweights_id = @purge_postalweight_id
	IF (@@ERROR <> 0) BEGIN
		ROLLBACK TRAN PostalRatesTran
		SET @ErrorFlag = 1
	END
	
	IF (@ErrorFlag = 0) BEGIN
		DELETE FROM pst_postalweights WHERE pst_postalweights_id = @purge_postalweight_id
		IF (@@ERROR <> 0) BEGIN
			ROLLBACK TRAN PostalRatesTran
			SET @ErrorFlag = 1
		END
	END
	
	IF (@ErrorFlag = 0) BEGIN
		COMMIT TRAN PostalRatesTran
	END

	FETCH NEXT FROM PostalRatesCursor INTO @purge_postalweight_id
END

CLOSE PostalRatesCursor
DEALLOCATE PostalRatesCursor

----------------------------------------------------------------------------------------------
-- Publication Rates
----------------------------------------------------------------------------------------------

-- First Purge pub_pubgroups and map tables
DECLARE @purge_pubgroup_id bigint

DECLARE PubGroupCursor CURSOR LOCAL FORWARD_ONLY FOR
    SELECT pg.pub_pubgroup_id FROM pub_pubgroup pg
        LEFT JOIN est_package pkg ON pg.pub_pubgroup_id = pkg.pub_pubgroup_id
    WHERE effectivedate <= @PurgeDate

OPEN PubGroupCursor
FETCH NEXT FROM PubGroupCursor INTO @purge_pubgroup_id
WHILE @@FETCH_STATUS = 0
BEGIN
    DELETE FROM pub_pubpubgroup_map WHERE pub_pubgroup_id = @purge_pubgroup_id
	FETCH NEXT FROM PubGroupCursor INTO @purge_pubgroup_id
END

CLOSE PubGroupCursor
DEALLOCATE PubGroupCursor

DELETE pg FROM pub_pubgroup pg
    LEFT JOIN pub_pubpubgroup_map pgmap ON pg.pub_pubgroup_id = pgmap.pub_pubgroup_id
WHERE pgmap.pub_pubgroup_id is null

DELETE gis FROM pub_groupinsertscenario_map gis
    LEFT JOIN pub_pubgroup pg ON pg.description = gis.pubgroupdescription
WHERE pg.description is null

DELETE pis FROM pub_insertscenario pis
    LEFT JOIN pub_groupinsertscenario_map gis ON pis.pub_insertscenario_id = gis.pub_insertscenario_id
WHERE gis.pub_insertscenario_id is null

-- Delete Pub Rates and Quantities for any pubrate maps where there are no groups anymore
DECLARE @purge_pubratemap_id bigint

DECLARE PubRateMapCursor CURSOR LOCAL FORWARD_ONLY FOR
    SELECT rmap.pub_pubrate_map_id FROM pub_pubrate_map rmap
        LEFT JOIN pub_pubpubgroup_map gmap ON rmap.pub_pubrate_map_id = gmap.pub_pubrate_map_id
    WHERE gmap.pub_pubrate_map_id is null

OPEN PubRateMapCursor
FETCH NEXT FROM PubRateMapCursor INTO @purge_pubratemap_id
WHILE @@FETCH_STATUS = 0
BEGIN

    -- DELETE pubrates and associated tables
    DELETE dow FROM pub_dayofweekrates dow
        JOIN pub_dayofweekratetypes dowtype ON dow.pub_dayofweekratetypes_id = dowtype.pub_dayofweekratetypes_id
        JOIN pub_pubrate rate ON dowtype.pub_pubrate_id = dowtype.pub_pubrate_id
        JOIN pub_pubrate_map map ON rate.pub_pubrate_map_id = map.pub_pubrate_map_id
    WHERE
        rate.effectivedate <= @PurgeDate AND
        map.pub_pubrate_map_id = @purge_pubratemap_id
    
    DELETE dowtype FROM pub_dayofweekratetypes dowtype 
        JOIN pub_pubrate rate ON dowtype.pub_pubrate_id = dowtype.pub_pubrate_id
        JOIN pub_pubrate_map map ON rate.pub_pubrate_map_id = map.pub_pubrate_map_id
    WHERE
        rate.effectivedate <= @PurgeDate AND
        map.pub_pubrate_map_id = @purge_pubratemap_id
    
    DELETE rate FROM pub_pubrate rate
        JOIN pub_pubrate_map map ON rate.pub_pubrate_map_id = map.pub_pubrate_map_id
    WHERE
        rate.effectivedate <= @PurgeDate AND
        map.pub_pubrate_map_id = @purge_pubratemap_id
    
    -- DELETE pubquantities and associated tables
    DELETE dow FROM pub_dayofweekquantity dow
        JOIN pub_pubquantity q ON dow.pub_pubquantity_id = q.pub_pubquantity_id
        JOIN pub_pubrate_map map ON q.pub_pubrate_map_id = map.pub_pubrate_map_id
    WHERE
        q.effectivedate <= @PurgeDate AND
        map.pub_pubrate_map_id = @purge_pubratemap_id

    DELETE q FROM pub_pubquantity q
        JOIN pub_pubrate_map map ON q.pub_pubrate_map_id = map.pub_pubrate_map_id
    WHERE
        q.effectivedate <= @PurgeDate AND
        map.pub_pubrate_map_id = @purge_pubratemap_id
    
	FETCH NEXT FROM PubRateMapCursor INTO @purge_pubratemap_id
END

CLOSE PubRateMapCursor
DEALLOCATE PubRateMapCursor

-- Delete Activate records pub rate maps where there are no more rates despite effective date
DELETE activate FROM pub_pubrate_map_activate 
    JOIN pub_pubrate_map map ON activate.pub_pubrate_map_id = map.pub_pubrate_map_id
    LEFT JOIN pub_pubrate r ON map.pub_pubrate_map_id = r.pub_pubrate_map_id
    LEFT JOIN pub_pubquantity q ON map.pub_pubrate_map_id = q.pub_pubrate_map_id
    LEFT JOIN pub_pubgroup g ON map.pub_pubrate_map_id = g.pub_pubrate_map_id
WHERE
    r.pub_pubrate_map_id is null AND
    q.pub_pubrate_map_id is null AND
    g.pub_pubrate_map_id is null

-- Delete All Orphaned pub rate maps
DELETE map FROM pub_pubrate_map
    LEFT JOIN pub_pubrate_map_activate a ON map.pub_pubrate_map_id = a.pub_pubrate_map_id
    LEFT JOIN pub_pubrate r ON map.pub_pubrate_map_id = r.pub_pubrate_map_id
    LEFT JOIN pub_pubquantity q ON map.pub_pubrate_map_id = q.pub_pubrate_map_id
    LEFT JOIN pub_pubgroup g ON map.pub_pubrate_map_id = g.pub_pubrate_map_id
WHERE
    a.pub_pubrate_map_id is null AND
    r.pub_pubrate_map_id is null AND
    q.pub_pubrate_map_id is null AND
    g.pub_pubrate_map_id is null

----------------------------------------------------------------------------------------------
-- Reports based on the date the report was run
----------------------------------------------------------------------------------------------
DELETE FROM rpt_report WHERE createddate <= @PurgeDate

GO

GRANT  EXECUTE  ON [dbo].[db_Purge_ByDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.directmailcost_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.directmailcost_i'
	DROP PROCEDURE dbo.directmailcost_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.directmailcost_i') IS NOT NULL
		PRINT '***********Drop of dbo.directmailcost_i FAILED.'
END
GO
PRINT 'Creating dbo.directmailcost_i'
GO

CREATE PROCEDURE dbo.directmailcost_i
/*
* PARAMETERS:
* EstimateIDs - XML string, the estimates that will be uploaded
*
*
* DESCRIPTION:
*		Populates directmail_cost table with mailing costs associated with the specified Estimate Id's
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   MANY
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
* 12/04/2007      BJS             Initial Creation - logic copied from rpt_DirectMailCosts
* 12/11/2007      BJS             Modified EstimateIDs parameter to allow 4000 characters
*                                   Fixed Piece Cost precision
* 12/17/2007      JRH             Calculate MediaQuantity from known quantities instead of view.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EstimateIDs varchar(4000),
@CreatedBy varchar(50)
as

set nocount on

declare @EstimateDocID int

create table #tempEstimate(EST_Estimate_ID BIGINT NOT NULL)

exec sp_xml_preparedocument @EstimateDocID output, @EstimateIDs
insert into #tempEstimate(EST_Estimate_ID)
select EST_Estimate_ID
from OPENXML(@EstimateDocID, '/root/estimate')
with(est_estimate_id BIGINT '@id')

/* Comments correlate to layout of Report/Extract #2 - Direct Mail Costs */
create table #tempComponent(
	EST_Component_ID bigint,
	EST_Estimate_ID bigint,
	EST_ComponentType_ID int,
	PST_PostalScenario_ID bigint,
	/* Main */
		RunDate datetime,
		AdNumber int,
		Description varchar(35),
	/* Specifications */
		PageCount int,
		Width decimal(10,4),
		Height decimal(10,4),
		/* Paper */
			PaperWeight int,
		PieceWeight decimal(12,6), /* (Width * Height) / 950,000 * PageCount * PaperWeight * 1.03 */
	/* Quantity */
		InsertQuantity int,
		SoloQuantity int,
		PolybagQuantity int,
		OtherQuantity int,
		SampleQuantity int,
		MediaQuantity int, /* Insert + Solo + PolyBag */
		SpoilagePct decimal(10,4),
		SpoilageQuantity int, /* SpoilagePct * Media Quantity */
		DirectMailQuantity int, /* Solo + Poly */
		InternalMailQuantity int, /* DirectMail - External List Quantity */
		ExternalMailQuantity int, /* External List Quantity */
		TotalProductionQuantity int, /* MediaQuantity + SpoilageQuantity + OtherQuantity + SampleQuantity */
	/* Production */
		ProductionWeight decimal(14,6), /* TotalProductionQuantity * PieceWeight */
		TotalEstimateWeight decimal(14,6), /* sum(ProductionWeight) group by EST_Estimate_ID*/
		CreativeCPP money,
		CreativeCost money,
		SeparatorCPP money,
		SeparatorCost money,
		/* Print */
			ManualPrintCost money,
			RunRate money,
			NumberOfPlants int,
			AdditionalPlates int,
			PrinterPlateCost money,
			NumberDigitalHandlePrepare int,
			DigitalHandlePrepareRate money,
			StitcherMakeReadyRate money,
			PressMakeReadyRate money,
			ReplacementPlateCost money,
			GrossPrintCost money, /* See Estimate Calculation Steps for formula */
			EarlyPayPrintDiscountPercent decimal(10,4),
			EarlyPayPrintDiscountAmount money, /* GrossPrintCost * EarlyPayPrintDiscountPercent */
			NetPrintCost money, /* GrossPrintCost - EarlyPayPrintDiscountAmount */
			PrinterTaxableMediaPct decimal(10,4),
			PrinterSalesTaxPct decimal(10,4),
			PrinterSalesTaxAmount money, /* (NetPrinterCost + PaperHandling) * PrinterTaxableMediaPct * PrinterSalesTaxPct */
			TotalPrintCost money, /* NetPrintCost + PrinterSalesTaxAmount */
		/* Paper */
			ManualPaperCost money,
			RunPounds decimal(10,2),
			MakereadyPounds int,
			PlateChangePounds decimal(10,2),
			NumberOfPressStops int,
			PressStopPounds int,
			TotalPaperPounds decimal(20,2), /* See Estimate Calculation Steps for formula */
			PaperCWTRate money, /* PPR_Paper_Map.CWT */
			GrossPaperCost money, /* TotalPaperPounds / 100 * PaperCWTRate */
			EarlyPayPaperDiscountPercent decimal(10,4),
			EarlyPayPaperDiscountAmount money, /* GrossPaperCost * EarlyPayPaperDiscountPercent */
			NetPaperCost money, /* GrossPaperCost - EarlyPayPaperDiscountAmount */
			PaperHandlingCWTRate money, /* VND_Printer.PaperHandling */
			PaperHandlingCost money, /* TotalPaperPounds * PaperHandlingCWTRate */
			PaperTaxableMediaPct decimal(10,4),
			PaperSalesTaxPct decimal(10,4),
			PaperSalesTaxAmount money, /* NetPaperCost * PaperTaxableMediaPct * PaperSalesTaxPct */
			TotalPaperCost money, /* NetPaperCost + PaperSalesTaxAmount */
		/* Mail List */
			InternalMailCPM money,
			ExternalMailCPM money,
			BlendedMailListCPM money,
			MailListCost money, /* See Logic below */
		VendorProductionCPM money,
		VendorProductionCost money, /* TotalProductionQuantity / 1000 * VendorProductionCPM  */
		OtherProductionCost money,
		ProductionTotal money, /* CreativeCost + SeparatorCost + TotalPrintCost + TotalPaperCost + ExternalMailCost + VendorProductionCost + OtherProductionCost */
		ProductionPieceCost decimal(20,6),

	/* Assembly */
		/* PolyBag */
			PolyBagCost money, /* dbo.EstimatePolyBagCost */
			OnsertRate money,
			OnsertCost money, /* Only if component type is Host Onsert -- PolyBagQuantity / 1000 * OnsertRate */
			PolyBagTotal money, /* PolyBagCost + OnsertCost */
		StitchInRate money,
		StitchInCost money, /* StitchInRate * MediaQuantity (if comp type is stitch-in) / 1000 */
		BlowInRate money,
		BlowInCost money, /* BlowInRate * MediaQuantity (if comp type is blow-in) / 1000 */
		/* Ink Jet */
			InkJetRate money,
			InkJetMakeReadyRate money,
			InkJetCost money, /* DirectMailQuantity / 1000 * InkJetRate */
			GrossInkjetMakereadyCost money, /* InkJetMakeReadyRate * NumberOfPlants */
			InkjetMakereadyCost money, /* See Logic below */
			TotalInkJetCost money, /* InkJetCost + InkJetMakeReadyCost */
		/* Mail Handling */
			TimeValueSlipsCPM money,
			TimeValueSlipsCost money, /* See SQL Logic Below */
			GrossMailHouseAdminFee money, /* The Mail House Admin Fee. */
			MailHouseAdminFee money, /* See SQL Logic Below */
			GlueTackCPM money,
			GlueTackCost money, /* See SQL Logic Below */
			TabbingCPM money,
			TabbingCost money, /* See SQL Logic Below */
			LetterInsertionCPM money,
			LetterInsertionCost money, /* See SQL Logic Below */
			OtherMailHandlingCPM money,
			OtherMailHandlingCost money, /* See SQL Logic Below */
			MailHandlingTotal money, /* See SQL Logic Below */
		AssemblyTotal money, /* PolyBagTotal + StitchInCost + BlowInCost + TotalInkJetCost + MailHandlingTotal */
		AssemblyPieceCost decimal(20,6),

	/* Distribution */
		/* Postal */
			PostalDropCost money, /* See Logic Below */
			PostalDropFuelSurchargeCost money, /* See Logic Below*/
			MailTrackingCPMRate money,
			MailTrackingCost money, /* DirectMailQuantity / 1000 * MailTrackingCPMRate */
			SoloPostageCost money,
			PolyPostageCost money,
			TotalPostageCost money, /* SoloPostageCost + PolyPostageCost */
			PostalTotal money, /*Postage PostalDropCost + PostalFuelSurcharge + MailTrackingCost + TotalPostageCost */
		SampleFreight money,
		GrossOtherFreight money,
		OtherFreight money,
		DistributionTotal money /* InsertTotal + PostalTotal + SampleFreight + OtherFreight */ 	
)

/* Get Raw Production Data */
insert into #tempComponent(EST_Component_ID, EST_Estimate_ID, EST_ComponentType_ID, PST_PostalScenario_ID,
	RunDate, AdNumber, Description, PageCount, Width,
	Height, PaperWeight, CreativeCPP, CreativeCost, SeparatorCPP, SeparatorCost,
	ExternalMailQuantity, SpoilagePct, ManualPrintCost, RunRate, NumberOfPlants, AdditionalPlates,
	PrinterPlateCost, NumberDigitalHandlePrepare, DigitalHandlePrepareRate, StitcherMakereadyRate, PressMakereadyRate,
	ReplacementPlateCost, EarlyPayPrintDiscountPercent, PrinterTaxableMediaPct,
	PrinterSalesTaxPct, ManualPaperCost, RunPounds, MakeReadyPounds, PlateChangePounds, NumberOfPressStops, PressStopPounds, PaperCWTRate,
	EarlyPayPaperDiscountPercent, PaperHandlingCWTRate, PaperTaxableMediaPct, PaperSalesTaxPct, InternalMailCPM, ExternalMailCPM,
	VendorProductionCPM, OtherProductionCost,
	OnsertRate, StitchInRate, BlowInRate, InkJetRate, InkJetMakeReadyRate, TimeValueSlipsCPM, GrossMailHouseAdminFee, GlueTackCPM, TabbingCPM,
	LetterInsertionCPM, OtherMailHandlingCPM, MailTrackingCPMRate, GrossOtherFreight)
select c.EST_Component_ID, c.EST_Estimate_ID, c.EST_ComponentType_ID, ad.PST_PostalScenario_ID,
	e.RunDate, c.AdNumber, c.Description, c.PageCount, c.Width, c.Height, pw.Weight,
	c.CreativeCPP, c.CreativeCPP * c.PageCount, c.SeparatorCPP, c.SeparatorCPP * c.PageCount,
	ad.ExternalMailQty, c.SpoilagePct, c.PrintCost, c.RunRate, c.NumberOfPlants,
	c.AdditionalPlates, pl.Rate, c.NumberDigitalHandlenPrepare, dh.Rate,
	case
		when c.StitcherMakeready_ID is null then c.StitcherMakereadyRate
		else smr.Rate
	end StitcherMakereadyRate,
	case
		when c.PressMakeready_ID is null then c.PressMakereadyRate
		else pmr.Rate
	end PressMakereadyRate,
	c.ReplacementPlateCost, c.EarlyPayPrintDiscount,
	c.PrinterTaxableMediaPct, c.PrinterSalesTaxPct, c.PaperCost, c.RunPounds, c.MakeReadyPounds, c.PlateChangePounds, c.NumberOfPressStops,
	c.PressStopPounds, pm.CWT, c.EarlyPayPaperDiscount, pr.PaperHandling, c.PaperTaxableMediaPct, c.PaperSalesTaxPct,
	case
		when c.EST_ComponentType_ID = 1 then ml.InternalListRate
		else null
	end InternalMailCPM,
	case
		when c.EST_ComponentType_ID = 1 then ad.ExternalMailCPM
		else null
	end ExternalMailCPM, c.VendorCPM, c.OtherProduction,
	oi.Rate, si.Rate, bi.Rate,
	case
		when c.EST_ComponentType_ID = 1 then mh.InkJetRate
		else null
	end InkJetRate,
	case
		when c.EST_ComponentType_ID = 1 then mh.InkJetMakeReady
		else null
	end InkJetMakeReady,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then mh.TimeValueSlips
		else null
	end TimeValueSlipsCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then mh.AdminFee
		else null
	end GrossMailHouseAdminFee,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseGlueTack = 1 then mh.GlueTackRate
		else null
	end GlueTackCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseTabbing = 1 then mh.TabbingRate
		else null
	end TabbingCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseLetterInsertion = 1 then mh.LetterInsertionRate
		else null
	end LetterInsertionCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then ad.MailHouseOtherHandling
		else null
	end OtherMailHandlingCPM,
	case
			when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseMailTracking = 1 then mt.MailTracking
			else null
		end MailTrackingCPMRate,
		case
			when c.EST_ComponentType_ID = 1 /*Host*/ then ad.OtherFreight
			else null
		end OtherFreight
from EST_Estimate e join #tempEstimate te on e.EST_Estimate_ID = te.EST_Estimate_ID
	join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
	join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
	left join VND_Printer pr on c.Printer_ID = pr.VND_Printer_ID
	left join PRT_PrinterRate pl on c.PlateCost_ID = pl.PRT_PrinterRate_ID
	left join PRT_PrinterRate dh on c.DigitalHandlenPrepare_ID = dh.PRT_PrinterRate_ID
	left join PRT_PrinterRate smr on c.StitcherMakeready_ID = smr.PRT_PrinterRate_ID
	left join PRT_PrinterRate pmr on c.PressMakeready_ID = pmr.PRT_PrinterRate_ID
	left join PPR_Paper_Map pm on c.Paper_Map_ID = pm.PPR_Paper_Map_ID
	left join PRT_PrinterRate si on c.StitchIn_ID = si.PRT_PrinterRate_ID
	left join PRT_PrinterRate bi on c.BlowIn_ID = bi.PRT_PrinterRate_ID
	left join PRT_PrinterRate oi on c.Onsert_ID = oi.PRT_PrinterRate_ID
	left join VND_MailTrackingRate mt on ad.MailTracking_ID = mt.VND_MailTrackingRate_ID
	left join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
	left join VND_MailListResourceRate ml on ad.MailListResource_ID = ml.VND_MailListResourceRate_ID
where c.AdNumber is not null or c.EST_ComponentType_ID = 5

/* Components of type +CVR share the Host Ad Number if they do not have an Ad Number*/
update cvrcomp
	set cvrcomp.AdNumber = hostcomp.AdNumber
from #tempComponent hostcomp join #tempComponent cvrcomp on hostcomp.EST_Estimate_ID = cvrcomp.EST_Estimate_ID
where hostcomp.EST_ComponentType_ID = 1 and cvrcomp.EST_ComponentType_ID = 5 and cvrcomp.AdNumber is null

/* Perform Specification Calculations */
update #tempComponent
	set PieceWeight = (Width * Height) / cast(950000 as decimal) * PageCount * PaperWeight * 1.03

update tc
	set tc.TotalEstimateWeight = tw.TotalEstimateWeight
from #tempComponent tc join vwTotalEstimateWeight tw on tc.EST_Estimate_ID = tw.EST_Estimate_ID
where tc.EST_ComponentType_ID = 1

/* Perform Quantity Calculations */
update tc
	set InsertQuantity = isnull(iq.InsertQuantity, 0)
from #tempComponent tc left join vwComponentInsertQuantity iq on tc.EST_Component_ID = iq.EST_Component_ID

update tc
	set SoloQuantity = isnull(sq.SoloQuantity, 0)
from #tempComponent tc left join vwComponentSoloQuantity sq on tc.EST_Component_ID = sq.EST_Component_ID

update tc
	set PolybagQuantity = isnull(pq.PolybagQuantity, 0)
from #tempComponent tc left join vwComponentPolybagQuantity pq on tc.EST_Component_ID = pq.EST_Component_ID

update tc
	set OtherQuantity = isnull(oq.OtherQuantity, 0)
from #tempComponent tc left join vwComponentOtherQuantity oq on tc.EST_Component_ID = oq.EST_Component_ID

update tc
	set SampleQuantity = isnull(sq.SampleQuantity, 0)
from #tempComponent tc left join vwComponentSampleQuantity sq on tc.EST_Component_ID = sq.EST_Component_ID

update tc
	set MediaQuantity = tc.InsertQuantity + tc.SoloQuantity + tc.PolybagQuantity
from #tempComponent tc

update #tempComponent
	set SpoilageQuantity = isnull(SpoilagePct, 0) * MediaQuantity,
		DirectMailQuantity = SoloQuantity + PolybagQuantity,
		InternalMailQuantity = DirectMailQuantity - ExternalMailQuantity

update #tempComponent
	set TotalProductionQuantity = MediaQuantity + SpoilageQuantity + OtherQuantity + SampleQuantity

update #tempComponent
	set ProductionWeight = TotalProductionQuantity * PieceWeight

/* Perform Production Calculations */
update #tempComponent
	set GrossPrintCost = ManualPrintCost
where ManualPrintCost is not null

update #tempComponent
	set GrossPrintCost = TotalProductionQuantity / cast(1000 as decimal) * isnull(RunRate, 0)
		+ isnull(NumberOfPlants, 0) * (isnull(StitcherMakeReadyRate, 0) + isnull(PressMakeReadyRate, 0))
		+ isnull(AdditionalPlates, 0) * isnull(PrinterPlateCost, 0)
		+ isnull(NumberDigitalHandlePrepare, 0) * isnull(DigitalHandlePrepareRate, 0)
		+ isnull(ReplacementPlateCost, 0)
where ManualPrintCost is null

update #tempComponent
	set EarlyPayPrintDiscountAmount = GrossPrintCost * isnull(EarlyPayPrintDiscountPercent, 0),
		NetPrintCost = GrossPrintCost - (GrossPrintCost * isnull(EarlyPayPrintDiscountPercent, 0))

/*TODO: We don't have a paper handling amount if manualpapercost is entered.  How does this effect the print sales tax?? */
update #tempComponent
	set GrossPaperCost = ManualPaperCost
where ManualPaperCost is not null

update #tempComponent
	set TotalPaperPounds = TotalProductionQuantity / cast(1000 as decimal) * isnull(RunPounds, 0)
		+ cast(isnull(NumberOfPlants, 0) as bigint) * cast(isnull(MakeReadyPounds, 0) as bigint)
		+ cast(isnull(AdditionalPlates, 0) as bigint) * isnull(PlateChangePounds, 0)
		+ cast(isnull(NumberOfPressStops, 0) as bigint) * cast(isnull(PressStopPounds, 0) as bigint)
where ManualPaperCost is null

update #tempComponent
	set GrossPaperCost = isnull(TotalPaperPounds, 0) / cast(100 as decimal) * isnull(PaperCWTRate, 0)
where ManualPaperCost is null

update #tempComponent
	set EarlyPayPaperDiscountAmount = GrossPaperCost * isnull(EarlyPayPaperDiscountPercent, 0),
		NetPaperCost = GrossPaperCost - (GrossPaperCost * isnull(EarlyPayPaperDiscountPercent, 0))

update #tempComponent
	set PaperHandlingCost = isnull(TotalPaperPounds, 0) / cast(100 as decimal) * isnull(PaperHandlingCWTRate, 0)

update #tempComponent
	set PrinterSalesTaxAmount = (NetPrintCost + PaperHandlingCost) * isnull(PrinterTaxableMediaPct, 0) * isnull(PrinterSalesTaxPct, 0),
		PaperSalesTaxAmount = NetPaperCost * isnull(PaperTaxableMediaPct, 0) * isnull(PaperSalesTaxPct, 0)

update #tempComponent
	set TotalPrintCost = NetPrintCost + PrinterSalesTaxAmount,
		TotalPaperCost = NetPaperCost + PaperHandlingCost + PaperSalesTaxAmount,
		VendorProductionCost = TotalProductionQuantity / cast(1000 as decimal) * VendorProductionCPM

update #tempComponent
	set BlendedMailListCPM =
		case
			when dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) = 0 then 0
			else InternalMailCPM
					* cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) - ExternalMailQuantity as decimal)
					/ cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
				+ ExternalMailCPM
					* cast(ExternalMailQuantity as decimal)
					/ cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
		end
where EST_ComponentType_ID = 1

update #tempComponent
	set MailListCost = BlendedMailListCPM * SoloQuantity / 1000 + isnull(dbo.ComponentPolybagMailListCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponent
	set ProductionTotal = isnull(CreativeCost, 0) + isnull(SeparatorCost, 0) + TotalPrintCost + TotalPaperCost +  isnull(MailListCost, 0) +
		+ isnull(VendorProductionCost, 0) + isnull(OtherProductionCost, 0)
		
update #tempComponent
	set ProductionPieceCost =
		case
			when TotalProductionQuantity = 0 then 0
			else ProductionTotal / cast(TotalProductionQuantity as decimal)
		end

/* Assembly Calculations */

update #tempComponent
	set PolyBagCost = dbo.EstimatePolyBagCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1

update #tempComponent
	set OnsertCost = PolybagQuantity / cast(1000 as decimal) * OnsertRate
where EST_ComponentType_ID = 2

update #tempComponent
	set GrossInkjetMakereadyCost = NumberofPlants * InkjetMakereadyRate

update #tempComponent
	set PolyBagTotal = isnull(PolyBagCost, 0) + isnull(OnsertCost, 0),
		StitchInCost =
			case EST_ComponentType_ID
				when 3 then MediaQuantity / cast(1000 as decimal) * StitchInRate
				else null
			end,
		BlowInCost =
			case EST_ComponentType_ID
				when 4 then MediaQuantity / cast(1000 as decimal) * BlowInRate
			end,
		InkJetCost =
			case EST_ComponentType_ID
				when 1 then (isnull(SoloQuantity, 0) / cast(1000 as decimal) * InkJetRate) + isnull(dbo.EstimatePolybagInkjetCost(EST_Estimate_ID), 0)
			end,
		InkJetMakeReadyCost =
			case EST_ComponentType_ID
				when 1 then
					case
						when isnull(SoloQuantity, 0) = 0 then 0
						else GrossInkjetMakereadyCost * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
					end
					+ isnull(dbo.ComponentPolybagInkjetMakereadyCost(EST_Component_ID), 0)
			end

update #tempComponent
		set TotalInkJetCost = isnull(InkJetCost, 0) + isnull(InkJetMakeReadyCost, 0)

update #tempComponent
	set TimeValueSlipsCost = TimeValueSlipsCPM * isnull(SoloQuantity, 0) / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseTimeValueSlipsCost(EST_Component_ID), 0),
		MailHouseAdminFee =
			case
				when isnull(SoloQuantity, 0) = 0 then 0
				else GrossMailHouseAdminFee * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
			end
			+ isnull(dbo.ComponentPolybagMailHouseAdminFee(EST_Component_ID), 0),
		GlueTackCost = GlueTackCPM * isnull(SoloQuantity, 0) / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseGlueTackCost(EST_Component_ID), 0),
		TabbingCost = TabbingCPM * isnull(SoloQuantity, 0) / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseTabbingCost(EST_Component_ID), 0),
		LetterInsertionCost = LetterInsertionCPM * isnull(SoloQuantity, 0) / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseLetterInsertionCost(EST_Component_ID), 0),
		OtherMailHandlingCost = OtherMailHandlingCPM * isnull(SoloQuantity, 0) / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseOtherDirectMailHandlingCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponent
	set MailHandlingTotal = isnull(TimeValueSlipsCost, 0) + isnull(MailHouseAdminFee, 0) + isnull(GlueTackCost, 0) + isnull(TabbingCost, 0)
			+ isnull(LetterInsertionCost, 0) + isnull(OtherMailHandlingCost, 0)

update #tempComponent
	set AssemblyTotal = PolyBagTotal + isnull(StitchInCost, 0) + isnull(BlowInCost, 0) + TotalInkJetCost + MailHandlingTotal

update #tempComponent
	set AssemblyPieceCost = 
		case
			when DirectMailQuantity = 0 then 0
			else AssemblyTotal / cast(DirectMailQuantity as decimal)
		end

update #tempComponent
	set PostalDropCost = dbo.EstimatePostalDropCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1


update #tempComponent
	set PostalDropFuelSurchargeCost = dbo.EstimatePostalDropFuelSurchargeCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1

update #tempComponent
	set	MailTrackingCost = isnull((SoloQuantity / cast(1000 as decimal) * MailTrackingCPMRate), 0) + isnull(dbo.ComponentPolybagMailTrackingCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponent
	set SoloPostageCost = isnull(dbo.ComponentSoloPostage(EST_Component_ID, PieceWeight, PST_PostalScenario_ID), 0)

update #tempComponent
	set PolyPostageCost = isnull(dbo.ComponentPolyPostage(EST_Component_ID, PieceWeight), 0)

update #tempComponent
	set TotalPostageCost = SoloPostageCost + PolyPostageCost

update #tempComponent
	set PostalTotal = isnull(PostalDropCost, 0) + isnull(PostalDropFuelSurchargeCost, 0) + isnull(MailTrackingCost, 0) + TotalPostageCost

update t
	set SampleFreight =
		case
			when isnull(s.FreightCWT, 0) <> 0 then dbo.EstimatePieceWeight(t.EST_Estimate_ID) * SampleQuantity / cast(100 as decimal) * s.FreightCWT
			else s.FreightFlat
		end
from #tempComponent t join EST_Samples s on t.EST_Estimate_ID = s.EST_Estimate_ID
where t.EST_ComponentType_ID = 1

update #tempComponent
	set OtherFreight = GrossOtherFreight
/* Not sure if this cost should be distributed across Polybags */
/*
			case
				when isnull(SoloQuantity, 0) = 0 then 0
				else GrossOtherFreight * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
			end
			+ isnull(dbo.ComponentPolybagOtherFreightCost(EST_Component_ID), 0)
*/
where EST_ComponentType_ID = 1

update #tempComponent
	set DistributionTotal = PostalTotal + isnull(OtherFreight, 0) + isnull(SampleFreight, 0)

/* Remove all previous directmail_cost records matching on ad number.  They will be replaced with new data. */
delete d
from directmail_cost d join #tempComponent c on d.AdNumber = c.AdNumber

/* Insert Direct Mail Costs into directmail_cost table */
insert into directmail_cost(AdNumber, Description, PieceCost, Quantity, CostwoDistribution, DistributionCost, TotalCost, CreatedBy, CreatedDate)
select AdNumber, max(Description) Description,
	case
		when isnull(sum(TotalProductionQuantity), 0) = 0 then 0
		else isnull(sum(ProductionTotal), 0) / sum(TotalProductionQuantity)
	end
	+
	case
		when isnull(sum(DirectMailQuantity), 0) = 0 then 0
		else isnull(sum(AssemblyTotal), 0) / sum(DirectMailQuantity)
	end PieceCost,
	sum(DirectMailQuantity) Quantity,
	sum(ProductionPieceCost * TotalProductionQuantity + AssemblyTotal) as CostWithoutDistribution,
	sum(DistributionTotal),
	sum(ProductionPieceCost * TotalProductionQuantity + AssemblyTotal + DistributionTotal) as TotalCost,
	@CreatedBy, getdate() CreatedDate
from #tempComponent
group by AdNumber

drop table #tempComponent
drop table #tempEstimate
GO

GRANT  EXECUTE  ON [dbo].[directmailcost_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstAssemDistribOptions_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstAssemDistribOptions_s_ByEstimateID'
	DROP PROCEDURE dbo.EstAssemDistribOptions_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstAssemDistribOptions_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstAssemDistribOptions_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstAssemDistribOptions_s_ByEstimateID'
GO

create proc dbo.EstAssemDistribOptions_s_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns the Assembly / Distribution Options for the estimate.  Used on the Assembly / Distribution Options Screen.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_AssemDistribOptions
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
* 05/25/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID int
as
select * from EST_AssemDistribOptions
where EST_Estimate_ID = @EST_Estimate_ID


GO

GRANT  EXECUTE  ON [dbo].[EstAssemDistribOptions_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstAssemDistribOptions_s_ForEstimateCopy') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstAssemDistribOptions_s_ForEstimateCopy'
	DROP PROCEDURE dbo.EstAssemDistribOptions_s_ForEstimateCopy
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstAssemDistribOptions_s_ForEstimateCopy') IS NOT NULL
		PRINT '***********Drop of dbo.EstAssemDistribOptions_s_ForEstimateCopy FAILED.'
END
GO
PRINT 'Creating dbo.EstAssemDistribOptions_s_ForEstimateCopy'
GO

create proc dbo.EstAssemDistribOptions_s_ForEstimateCopy
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns an estimate's assembly and distribution options and it's vendor and rate descriptions
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Component       READ
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
* 09/04/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint
as
select ad.*, f_vnd.Description InsertFreightDesc, ps.Description PostalScenarioDesc, mh_vnd.Description MailHouseDesc,
	mt_vnd.Description MailTrackingDesc, ml_vnd.Description MailListDesc
from EST_AssemDistribOptions ad join VND_Vendor f_vnd on ad.InsertFreightVendor_ID = f_vnd.VND_Vendor_ID
	join PST_PostalScenario ps on ad.PST_PostalScenario_ID = ps.PST_PostalScenario_ID
	join VND_MailHouseRate mh_rate on ad.MailHouse_ID = mh_rate.VND_MailHouseRate_ID
	join VND_Vendor mh_vnd on mh_rate.VND_Vendor_ID = mh_vnd.VND_Vendor_ID
	left join VND_MailTrackingRate mt_rate on ad.MailTracking_ID = mt_rate.VND_MailTrackingRate_ID
	left join VND_Vendor mt_vnd on mt_rate.VND_Vendor_ID = mt_vnd.VND_Vendor_ID
	left join VND_MaillistResourceRate ml_rate on ad.MailListResource_ID = ml_rate.VND_MailListResourceRate_ID
	left join VND_Vendor ml_vnd on ml_rate.VND_Vendor_ID = ml_vnd.VND_Vendor_ID
where ad.EST_Estimate_ID = @EST_Estimate_ID
GO

GRANT  EXECUTE  ON [dbo].[EstAssemDistribOptions_s_ForEstimateCopy]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstComponent_Search') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstComponent_Search'
	DROP PROCEDURE dbo.EstComponent_Search
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstComponent_Search') IS NOT NULL
		PRINT '***********Drop of dbo.EstComponent_Search FAILED.'
END
GO
PRINT 'Creating dbo.EstComponent_Search'
GO

CREATE proc dbo.EstComponent_Search
/*
* PARAMETERS:
* EST_Component_ID
* Description
* RunDateStart
* RunDateEnd
* EST_EstimateMediaType_ID
* EST_ComponentType_ID
* PaperWeight_ID
* PaperGrade_ID
* PageCount
* VendorSupplied - 1 = All Components, 2 = Only VS Components, 3 = Exclude VS Components

* DESCRIPTION:
*		Components are found based on search criteria for the Component Search screen.  At least one parameter is required.  The 
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate
*   EST_Component
*   EST_EstimateInsertScenario_Map
*   EST_PubInsertDates
*   EST_EstimateMediaType
*   EST_Season
*   EST_Status
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
* 08/21/2007      BJS             Initial Creation
* 09/19/2007      BJS             Defect 305 - Fixed Description search by adding like comparison
* 10/29/2007      JRH             Change to inner join.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Component_ID bigint,
@Description varchar(35),
@RunDateStart datetime,
@RunDateEnd datetime,
@EST_EstimateMediaType_ID int,
@EST_ComponentType_ID int,
@PaperWeight_ID int,
@PaperGrade_ID int,
@PageCount int,
@VendorSupplied tinyint
as

select c.EST_Component_ID, max(e.EST_Estimate_ID) EST_Estimate_ID, max(e.Parent_ID) Parent_ID, max(e.EST_Status_ID) EST_Status_ID,
	max(e.RunDate) RunDate, max(c.Description) Description, max(c.AdNumber) AdNumber
from EST_Estimate e inner join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
where (@EST_Component_ID is null or (@EST_Component_ID = c.EST_Component_ID))
	and (@Description is null or (c.Description like '%' + @Description + '%'))
	and (@RunDateStart is null or (@RunDateStart <= e.RunDate))
	and (@RunDateEnd is null or (@RunDateEnd >= e.RunDate))
	and (@EST_EstimateMediaType_ID is null or (@EST_EstimateMediaType_ID = c.EST_EstimateMediaType_ID))
	and (@EST_ComponentType_ID is null or (@EST_ComponentType_ID = c.EST_ComponentType_ID))
	and (@PaperWeight_ID is null or (@PaperWeight_ID = c.PaperWeight_ID))
	and (@PaperGrade_ID is null or (@PaperGrade_ID = c.PaperGrade_ID))
	and (@PageCount is null or (@PageCount = c.PageCount))
	and (@VendorSupplied = 1 or (@VendorSupplied = 2 and c.VendorSupplied = 1) or (@VendorSupplied = 3 and c.VendorSupplied = 0))
group by c.EST_Component_ID
GO

GRANT  EXECUTE  ON [dbo].[EstComponent_Search]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstComponent_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstComponent_s_ByEstimateID'
	DROP PROCEDURE dbo.EstComponent_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstComponent_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstComponent_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstComponent_s_ByEstimateID'
GO

create proc dbo.EstComponent_s_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns all of the components for the estimate.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Component
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
* 05/25/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint
as
select * from EST_Component
where EST_Estimate_ID = @EST_Estimate_ID


GO

GRANT  EXECUTE  ON [dbo].[EstComponent_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstComponent_s_ForEstimateCopy') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstComponent_s_ForEstimateCopy'
	DROP PROCEDURE dbo.EstComponent_s_ForEstimateCopy
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstComponent_s_ForEstimateCopy') IS NOT NULL
		PRINT '***********Drop of dbo.EstComponent_s_ForEstimateCopy FAILED.'
END
GO
PRINT 'Creating dbo.EstComponent_s_ForEstimateCopy'
GO

create proc dbo.EstComponent_s_ForEstimateCopy
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns an estimate's components and their vendor and rate descriptions
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Component       READ
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
* 09/04/2007      BJS             Initial Creation
* 09/24/2007      BJS             Added left join for Stitcher Makeready, Press Makeready and Digital Handle & Prepare
* 10/31/2007      BJS             Added left join for assembly vendor
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint
as
select c.*,
	vs_vnd.Description VendorSuppliedDesc, creative_vnd.Description CreativeDesc, separator_vnd.Description SeparatorDesc,
	printer_vnd.Description PrinterDesc, assembly_vnd.Description AssemblyVendorDesc,
	pc_rate.Description PlateCostDesc, si_rate.Description StitchInDesc, bi_rate.Description BlowInDesc,
	ons_rate.Description OnsertDesc, simr_rate.Description StitcherMakereadyDesc, prmr_rate.Description PressMakereadyDesc,
	dhp_rate.Description DigitalHandlenPrepareDesc,
	ppr_vnd.Description PaperDesc, ppr_map.Description PaperMapDesc, pw.Weight PaperWeight, pg.Grade PaperGrade
from EST_Component c
	left join VND_Vendor vs_vnd on c.VendorSupplied_ID = vs_vnd.VND_Vendor_ID
	left join VND_Vendor creative_vnd on c.CreativeVendor_ID = creative_vnd.VND_Vendor_ID
	left join VND_Vendor separator_vnd on c.Separator_ID = separator_vnd.VND_Vendor_ID
	left join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	left join VND_Vendor printer_vnd on p.VND_Vendor_ID = printer_vnd.VND_Vendor_ID
	left join VND_Printer ap on c.AssemblyVendor_ID = ap.VND_Printer_ID
	left join VND_Vendor assembly_vnd on ap.VND_Vendor_ID = assembly_vnd.VND_Vendor_ID
	left join PRT_PrinterRate pc_rate on c.PlateCost_ID = pc_rate.PRT_PrinterRate_ID
	left join PRT_PrinterRate si_rate on c.StitchIn_ID = si_rate.PRT_PrinterRate_ID
	left join PRT_PrinterRate bi_rate on c.BlowIn_ID = bi_rate.PRT_PrinterRate_ID
	left join PRT_PrinterRate ons_rate on c.Onsert_ID = ons_rate.PRT_PrinterRate_ID
	left join PRT_PrinterRate simr_rate on c.StitcherMakeready_ID = simr_rate.PRT_PrinterRate_ID
	left join PRT_PrinterRate prmr_rate on c.PressMakeready_ID = prmr_rate.PRT_PrinterRate_ID
	left join PRT_PrinterRate dhp_rate on c.DigitalHandlenPrepare_ID = dhp_rate.PRT_PrinterRate_ID
	left join VND_Paper ppr on c.Paper_ID = ppr.VND_Paper_ID
	left join VND_Vendor ppr_vnd on ppr.VND_Vendor_ID = ppr_vnd.VND_Vendor_ID
	left join PPR_Paper_Map ppr_map on c.Paper_Map_ID = ppr_map.PPR_Paper_Map_ID
	join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
	left join PPR_PaperGrade pg on c.PaperGrade_ID = pg.PPR_PaperGrade_ID
where c.EST_Estimate_ID = @EST_Estimate_ID
GO

GRANT  EXECUTE  ON [dbo].[EstComponent_s_ForEstimateCopy]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstEstimatePolybagGroupMap_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstEstimatePolybagGroupMap_s_ByEstimateID'
	DROP PROCEDURE dbo.EstEstimatePolybagGroupMap_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstEstimatePolybagGroupMap_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstEstimatePolybagGroupMap_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstEstimatePolybagGroupMap_s_ByEstimateID'
GO
CREATE PROC dbo.EstEstimatePolybagGroupMap_s_ByEstimateID
/*
* PARAMETERS:
*	EST_Estimate_ID - Required.
*
* DESCRIPTION:
*	Returns EST_EstimatePolybagGroup_Map records referencing the specified EST_Estimate_ID.
*
* TABLES:
*   Table Name                    Access
*   ==========                    ======
*   est_estimate		          READ
*   est_estimatepolybaggroup_map  READ
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 08/31/2007      BJS             Initial Creation 
*
*/
@EST_Estimate_ID bigint
as

select * from EST_EstimatePolybagGroup_Map where EST_Estimate_ID = @EST_Estimate_ID
GO

GRANT  EXECUTE  ON [dbo].[EstEstimatePolybagGroupMap_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstEstimate_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstEstimate_i'
	DROP PROCEDURE dbo.EstEstimate_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstEstimate_i') IS NOT NULL
		PRINT '***********Drop of dbo.EstEstimate_i FAILED.'
END
GO
PRINT 'Creating dbo.EstEstimate_i'
GO

create proc dbo.EstEstimate_i
/*
* PARAMETERS:
* EST_Estimate_ID - Output.  If a value is passed in it is ignored and the value is set to the ID of the new record.
*
*
* DESCRIPTION:
*		Inserts an estimate into the EST_Estimate table.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate
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
* 05/25/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint output,
@Description varchar(35),
@Comments varchar(255),
@EST_Season_ID int,
@FiscalYear int,
@RunDate datetime,
@EST_Status_ID int,
@FiscalMonth int,
@Parent_ID bigint,
@CreatedBy varchar(50)
as
insert into EST_Estimate(Description, Comments, EST_Season_ID, FiscalYear, RunDate, EST_Status_ID, FiscalMonth, Parent_ID, CreatedBy, CreatedDate)
values(@Description, @Comments, @EST_Season_ID, @FiscalYear, @RunDate, @EST_Status_ID, @FiscalMonth, @Parent_ID, @CreatedBy, getdate())
set @EST_Estimate_ID = @@identity


GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstEstimate_Search') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstEstimate_Search'
	DROP PROCEDURE dbo.EstEstimate_Search
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstEstimate_Search') IS NOT NULL
		PRINT '***********Drop of dbo.EstEstimate_Search FAILED.'
END
GO
PRINT 'Creating dbo.EstEstimate_Search'
GO

CREATE proc dbo.EstEstimate_Search
/*
* PARAMETERS:
*
*
*
* DESCRIPTION:
*		Estimates are found based on search criteria for the Estimate Search screen.  At least one parameter is required.
*
*
* TABLES:
*   Table Name			Access
*   ==========			======
*   EST_Estimate		READ
*   EST_Component		READ
*   EST_Package			READ
*   EST_PubInsertDates		READ
*   EST_EstimateMediaType	READ
*   EST_Season			READ
*   EST_Status			READ
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
* Date		Who		Comments
* ----------	----		-------------------------------------------------
* 05/25/2007	BJS		Initial Creation 
* 09/11/2007	NLS		Added SortOrder column and fixed UploadDate issue
* 09/13/2007	NLS		Changed join on PubInsertDates to AssemblyDistOptions
* 09/19/2007	JRH		Changed join on est_estimateinsertscenario_map to est_package
* 10/01/2007	NLS		Removed dummy TotalCost column.  Filled in client side from another proc
* 10/02/2007    NLS		Fixed Component Description to use LIKE
* 11/05/2007	JRH		Changed the SortOrder.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Description varchar(35),
@EST_Status_ID int,
@AdNumber int,
@ComponentDescription varchar(35),
@HostMediaType int,
@HostPageCount int,
@HostMediaQtyLow int,
@HostMediaQtyHigh int,
@PUB_InsertScenario_ID bigint,
@CreatedBy varchar(35),
@RunDateStart datetime,
@RunDateEnd datetime,
@ModifiedDateStart datetime,
@ModifiedDateEnd datetime,
@EST_Season_ID int,
@FiscalYear int,
@FiscalMonth int,
@InsertDOW int,
@EstimateComments varchar(255)
as

select
	EST_Estimate_ID,
	ParentId, 
	RunDate, 
	Description, 
	HostMediaDesc, 
	SeasonDesc,
	StatusDesc,
	UploadKillDate, 
	displayed,
	CAST(Description as char(35)) + substring(Est_ID_text, len(Est_ID_text)-9, 10) 
		+ CONVERT(varchar(30), isnull(UploadKillDate, '1/1/1900'), 12) SortOrder
FROM
	(select e.EST_Estimate_ID, 
		max(e.parent_id) ParentId, 
		max(e.RunDate) RunDate, 
		max(e.Description) Description, 
		max(mt.Description) HostMediaDesc, 
		max(s.Description) SeasonDesc,
		max(st.Description) StatusDesc,
		
		case max(e.EST_Status_ID)
			when 2 then max(e.uploaddate)
			when 3 then max(e.ModifiedDate)
			else null
		end UploadKillDate, 
		
		case
			when max(e.parent_id) is null then 1
			else 0
		end displayed,
		
		('0000000000' + CAST(isnull(max(e.parent_id),e.est_estimate_id) AS varchar(10))) Est_ID_text
	
	from EST_Estimate e left join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
		left join EST_Component hc on e.EST_Estimate_ID = hc.EST_Estimate_ID and hc.EST_ComponentType_ID = 1
		left join EST_Package ep on e.EST_Estimate_ID = ep.EST_Estimate_ID
		left join est_assemdistriboptions ado on e.EST_Estimate_ID = ado.EST_Estimate_ID
		left join EST_EstimateMediaType mt on hc.EST_EstimateMediaType_ID = mt.EST_EstimateMediaType_ID
		join EST_Season s on e.EST_Season_ID = s.EST_Season_ID
		join EST_Status st on e.EST_Status_ID = st.EST_Status_ID
	where (@Description is null or e.Description like '%' + @Description + '%')
		and (isnull(@EST_Status_ID, e.EST_Status_ID) = e.EST_Status_ID)
		and ((@AdNumber is null) or (@AdNumber = c.AdNumber))
		and ((@ComponentDescription is null) or (c.Description like '%' + @ComponentDescription + '%'))
		and ((@HostMediaType is null) or (@HostMediaType = hc.EST_EstimateMediaType_ID))
		and ((@HostPageCount is null) or (@HostPageCount = hc.PageCount))
		and (@HostMediaQtyLow is null or dbo.ComponentMediaQuantity(hc.EST_Component_ID) >= @HostMediaQtyLow)
		and (@HostMediaQtyHigh is null or dbo.ComponentMediaQuantity(hc.EST_Component_ID) <= @HostMediaQtyHigh)
		and ((@PUB_InsertScenario_ID is null) or (ep.PUB_InsertScenario_ID = @PUB_InsertScenario_ID))
		and ((@CreatedBy is null) or (@CreatedBy = e.CreatedBy))
		and (@RunDateStart is null or e.RunDate >= @RunDateStart)
		and (@RunDateEnd is null or e.RunDate <= @RunDateEnd)
		/*TODO: When a search is done on modified date.  Do we need to check the created date if modified date is null? */
		and (@ModifiedDateStart is null or isnull(e.ModifiedDate, e.CreatedDate) >= @ModifiedDateStart)
		and (@ModifiedDateEnd is null or isnull(e.ModifiedDate, e.CreatedDate) <= @ModifiedDateEnd)
		and ((@EST_Season_ID is null) or (@EST_Season_ID = e.EST_Season_ID))
		and ((@FiscalYear is null) or (@FiscalYear = e.FiscalYear))
		and ((@FiscalMonth is null) or (@FiscalMonth = e.FiscalMonth))
	    and (@InsertDOW is null or @InsertDOW = ado.InsertDOW)
		and (@EstimateComments is null or e.Comments like '%' + @EstimateComments + '%')
	group by e.EST_Estimate_ID)a

GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_Search]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstEstimate_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstEstimate_s_ByEstimateID'
	DROP PROCEDURE dbo.EstEstimate_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstEstimate_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstEstimate_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstEstimate_s_ByEstimateID'
GO

CREATE PROC dbo.EstEstimate_s_ByEstimateID
/*
* PARAMETERS:
*	@EstEstimateId - required.
*
*
* DESCRIPTION:
* Returns an Estimate record.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate        READ
*
* DATABASE:
*		All
*
*
* RETURN VALUE:
*   none
*
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/04/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/

@EST_Estimate_ID bigint

AS

select * from EST_Estimate
where EST_Estimate_ID = @EST_Estimate_ID
GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.EstEstimate_s_CostSummary_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstEstimate_s_CostSummary_ByEstimateID'
	DROP PROCEDURE dbo.EstEstimate_s_CostSummary_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstEstimate_s_CostSummary_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstEstimate_s_CostSummary_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstEstimate_s_CostSummary_ByEstimateID'
GO

CREATE proc dbo.EstEstimate_s_CostSummary_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns all of the component costs for the estimate.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate
*   EST_Component
*   EST_AssemDistribOptions
*   PPR_PaperWeight
*   PPR_PaperGrade
*   VND_Printer
*   PRT_PrinterRate
*   PPR_Paper_Map
*   VND_MailHouse
*   VND_MailTracker
*
*
* PROCEDURES CALLED:
*   dbo.TotalEstimateWeight
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
* 05/25/2007      BJS             Initial Creation 
* 06/20/2007      BJS             Added ExternalMailCPM, ExternalMailCost, GrandTotal
* 06/21/2007      BJS             Revised Mail Handling Calculations
* 07/09/2007      BJS             Revised Distribution Calculations -- Only calculated at Host Component
*                                 Added Mail List Calculations
* 07/10/2007      BJS             Revised Onsert Cost Calculations to include Host Components
* 07/12/2007      BJS             Revised Mail Handling Calculations.  Polybags now pull rates from the first estimate
* 07/12/2007      BJS             Revised Onsert Cost Calculations to NOT include Host Components
* 07/24/2007      BJS             Added reference to TotalEstimateWeight function
* 08/15/2007      TJU             Fixed reference to Onsert_ID and OnsertRate
* 08/16/2007      TJU             Eliminated reference to pg.GradeID
* 08/29/2007      BJS             Readded drop/create portion of script and grant permissions
*                                 Fixed join to PrinterRate table for plate cost
* 09/06/2007      BJS             Changed EST_Estimate_ID parameter to bigint.  Check for zero to prevent divide by zero error when calculating
*                                 BlendedMailListCPM
* 09/10/2007      BJS             Modified populate of tempPackagesByPubRate table
* 09/24/2007      BJS             Modified logic for Digi H&P, Stitcher MR and Press MR
* 10/08/2007      BJS             Explicitly cast hard-coded values (ie 1000) to decimal to prevent rounding errors.
* 10/16/2007      NLS             Add a description to the total row
* 10/24/2007      BJS             Changed join to vnd_printer to left join
* 11/09/2007      JRH             Fixed SampleFreight, InsertFuelSurchargeCost, and InsertFreightCost to allocate all to host.
* 11/14/2007      JRH             MediaQuantity needs to exclude OtherQuantity for calculations 
*                                 but includes OtherQuantity for report.
* 11/16/2007      JRH             Fixed InsertTotal to account for nulls.
* 11/16/2007      JRH             Check PostalTotal for null.
* 12/07/2007      JRH             Fixed the InsertDiscount calculation.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint
as

set nocount on

/* Comments correlate to layout of Report/Extract #4 - For Single Estimate Only */
create table #tempComponents(
	EST_Component_ID bigint,
	EST_Estimate_ID bigint,
	EST_ComponentType_ID int,
	PST_PostalScenario_ID bigint,
	/* Main */
		AdNumber int,
		Description varchar(35),
		Comments varchar(255),
	/* Specifications */
		PageCount int,
		Width decimal(10,4),
		Height decimal(10,4),
		/* Paper */
			PaperWeight int,
			PaperGradeDescription varchar(35),
		PieceWeight decimal(12,6), /* (Width * Height) / 950,000 * PageCount * PaperWeight * 1.03 */
	/* Quantity */
		InsertQuantity int,
		SoloQuantity int,
		PolybagQuantity int,
		OtherQuantity int,
		SampleQuantity int,
		MediaQuantity int, /* Insert + Solo + PolyBag (+ Other only for final report)*/
		SpoilagePct decimal(10,4),
		SpoilageQuantity int, /* SpoilagePct * Media Quantity */
		DirectMailQuantity int, /* Solo + Poly */
		InternalMailQuantity int, /* DirectMail - External List Quantity */
		ExternalMailQuantity int, /* External List Quantity */
		TotalProductionQuantity int, /* MediaQuantity + SpoilageQuantity + OtherQuantity + SampleQuantity */
	/* Production */
		ProductionWeight decimal(14,6), /* TotalProductionQuantity * PieceWeight */
		TotalEstimateWeight decimal(14,6), /* sum(ProductionWeight) group by EST_Estimate_ID*/
		CreativeCPP money,
		CreativeCost money,
		SeparatorCPP money,
		SeparatorCost money,
		/* Print */
			ManualPrintCost money,
			RunRate money,
			NumberOfPlants int,
			AdditionalPlates int,
			PrinterPlateCost money,
			NumberDigitalHandlePrepare int,
			DigitalHandlePrepareRate money,
			StitcherMakereadyRate money,
			StitcherMakereadyCost money,
			PressMakeReadyRate money,
			PressMakereadyCost money,
			ReplacementPlateCost money,
			GrossPrintCost money, /* See Estimate Calculation Steps for formula */
			EarlyPayPrintDiscountPercent decimal(10,4),
			EarlyPayPrintDiscountAmount money, /* GrossPrintCost * EarlyPayPrintDiscountPercent */
			NetPrintCost money, /* GrossPrintCost - EarlyPayPrintDiscountAmount */
			PrinterTaxableMediaPct decimal(10,4),
			PrinterSalesTaxPct decimal(10,4),
			PrinterSalesTaxAmount money, /* (NetPrinterCost + PaperHandling) * PrinterTaxableMediaPct * PrinterSalesTaxPct */
			TotalPrintCost money, /* NetPrintCost + PrinterSalesTaxAmount */
		/* Paper */
			ManualPaperCost money,
			RunPounds decimal(10,2),
			MakereadyPounds int,
			PlateChangePounds decimal(10,2),
			NumberOfPressStops int,
			PressStopPounds int,
			TotalPaperPounds decimal(20,2), /* See Estimate Calculation Steps for formula */
			PaperCWTRate money, /* PPR_Paper_Map.CWT */
			GrossPaperCost money, /* TotalPaperPounds / 100 * PaperCWTRate */
			EarlyPayPaperDiscountPercent decimal(10,4),
			EarlyPayPaperDiscountAmount money, /* GrossPaperCost * EarlyPayPaperDiscountPercent */
			NetPaperCost money, /* GrossPaperCost - EarlyPayPaperDiscountAmount */
			PaperHandlingCWTRate money, /* VND_Printer.PaperHandling */
			PaperHandlingCost money, /* TotalPaperPounds * PaperHandlingCWTRate */
			PaperTaxableMediaPct decimal(10,4),
			PaperSalesTaxPct decimal(10,4),
			PaperSalesTaxAmount money, /* NetPaperCost * PaperTaxableMediaPct * PaperSalesTaxPct */
			TotalPaperCost money, /* NetPaperCost + PaperSalesTaxAmount */
		/* Mail List */
			InternalMailCPM money,
			ExternalMailCPM money,
			BlendedMailListCPM money,
			MailListCost money, /* See Logic below */
		VendorProductionCPM money,
		VendorProductionCost money, /* TotalProductionQuantity / 1000 * VendorProductionCPM  */
		OtherProductionCost money,
		ProductionTotal money, /* CreativeCost + SeparatorCost + TotalPrintCost + TotalPaperCost + ExternalMailCost + VendorProductionCost + OtherProductionCost */
	/* Assembly */
		/* Polybag */
			PolybagCost money, /* dbo.EstimatePolyBagCost */
			OnsertRate money,
			OnsertCost money, /* Only if component type is Host Onsert -- PolyBagQuantity / 1000 * OnsertRate */
			PolyBagTotal money, /* PolyBagCost + OnsertCost */
		StitchInRate money,
		StitchInCost money, /* StitchInRate * MediaQuantity (if comp type is stitch-in) / 1000 */
		BlowInRate money,
		BlowInCost money, /* BlowInRate * MediaQuantity (if comp type is blow-in) / 1000 */
		/* Ink Jet */
			InkJetRate money,
			InkJetMakeReadyRate money,
			InkJetCost money, /* DirectMailQuantity / 1000 * InkJetRate */
			GrossInkjetMakereadyCost money, /* InkJetMakeReadyRate * NumberOfPlants */
			InkjetMakereadyCost money, /* See Logic below */
			TotalInkJetCost money, /* InkJetCost + InkJetMakeReadyCost */
		/* Handling */
			/* Insert */
				CornerGuardRate money,
				CornerGuardCost money, /* (MediaQuantity * PieceWeight) / 2000 * CornerGuardRate */
				SkidRate money,
				SkidCost money, /* (MediaQuantity * PieceWeight) / 2000 * SkidRate */
				NumberOfCartons int,
				CartonRate money,
				CartonCost money, /* NumberOfCartons * CartonCost */
				InsertHandlingTotal money, /* CornerGuardCost + SkidCost + CartonCost */
			/* Mail */
				TimeValueSlipsCPM money,
				TimeValueSlipsCost money, /* See SQL Logic Below */
				GrossMailHouseAdminFee money, /* The Mail House Admin Fee. */
				MailHouseAdminFee money, /* See SQL Logic Below */
				GlueTackCPM money,
				GlueTackCost money, /* See SQL Logic Below */
				TabbingCPM money,
				TabbingCost money, /* See SQL Logic Below */
				LetterInsertionCPM money,
				LetterInsertionCost money, /* See SQL Logic Below */
				OtherMailHandlingCPM money,
				OtherMailHandlingCost money, /* See SQL Logic Below */
				MailHandlingTotal money, /* See SQL Logic Below */
			HandlingTotal money, /* InsertHandlingTotal + MailHandlngTotal */
		AssemblyTotal money, /* PolyBagTotal + StitchInCost + BlowInCost + TotalInkJetCost + HandlingTotal */
	/* Distribution */
		/* Insert */
			InsertCost money, /* See Logic below */
			InsertFreightCWT money,
			InsertFreightCost money, /* TotalEstimateWeight / 100 * InsertFreightCWT */
			InsertFuelSurchargePercent decimal(10,4),
			InsertFuelSurchargeCost money, /* InsertFreightCost * InsertFreightSurchargePercent */
			InsertTotal money, /* InsertCost + InsertFreightCost + InsertFuelSurchargeCost */
		/* Postal */
			PostalDropCost money, /* See Logic Below */
			PostalDropFuelSurchargeCost money, /* See Logic Below*/
			MailTrackingCPMRate money,
			MailTrackingCost money, /* DirectMailQuantity / 1000 * MailTrackingCPMRate */
			SoloPostageCost money,
			PolyPostageCost money,
			TotalPostageCost money, /* SoloPostageCost + PolyPostageCost */
			PostalTotal money, /*Postage PostalDropCost + PostalFuelSurcharge + MailTrackingCost + TotalPostageCost */
		SampleFreight money,
		GrossOtherFreight money,
		OtherFreight money,
		DistributionTotal money, /* InsertTotal + PostalTotal + SampleFreight + OtherFreight */ 			
	GrandTotal money
)

/* Get Raw Production Data */
insert into #tempComponents(EST_Component_ID, EST_Estimate_ID, EST_ComponentType_ID, PST_PostalScenario_ID, AdNumber, Description, Comments,
	PageCount, Width,
	Height, PaperWeight, PaperGradeDescription, CreativeCPP, CreativeCost, SeparatorCPP, SeparatorCost,
	InsertQuantity, SoloQuantity, PolyBagQuantity, OtherQuantity,
	SampleQuantity, ExternalMailQuantity, SpoilagePct, ManualPrintCost, RunRate, NumberOfPlants, AdditionalPlates,
	PrinterPlateCost, NumberDigitalHandlePrepare, DigitalHandlePrepareRate, StitcherMakereadyRate, PressMakereadyRate,
	ReplacementPlateCost, EarlyPayPrintDiscountPercent, PrinterTaxableMediaPct,
	PrinterSalesTaxPct, ManualPaperCost, RunPounds, MakeReadyPounds, PlateChangePounds, NumberOfPressStops, PressStopPounds, PaperCWTRate,
	EarlyPayPaperDiscountPercent, PaperHandlingCWTRate, PaperTaxableMediaPct, PaperSalesTaxPct, InternalMailCPM, ExternalMailCPM,
	VendorProductionCPM, OtherProductionCost, OnsertRate, StitchInRate, BlowInRate, InkJetRate, InkJetMakeReadyRate, CornerGuardRate, SkidRate,
	NumberOfCartons, TimeValueSlipsCPM, GrossMailHouseAdminFee, GlueTackCPM, TabbingCPM, LetterInsertionCPM, OtherMailHandlingCPM,
	InsertFreightCWT, InsertFuelSurchargePercent, MailTrackingCPMRate, GrossOtherFreight)
select c.EST_Component_ID, c.EST_Estimate_ID, c.EST_ComponentType_ID, ad.PST_PostalScenario_ID, c.AdNumber, c.Description, e.Comments,
	c.PageCount, c.Width,
	c.Height, pw.Weight, pg.Grade, c.CreativeCPP, c.CreativeCPP * c.PageCount, c.SeparatorCPP, c.SeparatorCPP * c.PageCount,
	isnull(dbo.ComponentInsertQuantity(c.EST_Component_ID), 0),
	isnull(dbo.ComponentSoloMailQuantity(c.EST_Component_ID), 0), isnull(dbo.ComponentPolybagQuantity(c.EST_Component_ID), 0),
	isnull(dbo.ComponentOtherQuantity(c.EST_Component_ID), 0),
	isnull(dbo.ComponentSampleQuantity(c.EST_Component_ID), 0),
	ad.ExternalMailQty, c.SpoilagePct, c.PrintCost, c.RunRate, c.NumberOfPlants,
	c.AdditionalPlates, pl.Rate, c.NumberDigitalHandlenPrepare, dh.Rate,
	case
		when c.StitcherMakeready_ID is null then c.StitcherMakereadyRate
		else smr.Rate
	end StitcherMakereadyRate,
	case
		when c.PressMakeready_ID is null then c.PressMakereadyRate
		else pmr.Rate
	end PressMakereadyRate,
	c.ReplacementPlateCost, c.EarlyPayPrintDiscount,
	c.PrinterTaxableMediaPct, c.PrinterSalesTaxPct, c.PaperCost, c.RunPounds, c.MakeReadyPounds, c.PlateChangePounds, c.NumberOfPressStops,
	c.PressStopPounds, pm.CWT, c.EarlyPayPaperDiscount, p.PaperHandling, c.PaperTaxableMediaPct, c.PaperSalesTaxPct,
	case
		when c.EST_ComponentType_ID = 1 then ml.InternalListRate
		else null
	end InternalMailCPM,
	case
		when c.EST_ComponentType_ID = 1 then ad.ExternalMailCPM
		else null
	end ExternalMailCPM, c.VendorCPM, c.OtherProduction,
	oi.Rate, si.Rate, bi.Rate,
	case
		when c.EST_ComponentType_ID = 1 then mh.InkJetRate
		else null
	end InkJetRate,
	case
		when c.EST_ComponentType_ID = 1 then mh.InkJetMakeReady
		else null
	end InkJetMakeReady,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.CornerGuards = 1 then p.CornerGuard
		else null
	end CornerGuardRate,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.Skids = 1 then p.Skid
		else null
	end SkidRate,
	ad.NbrOfCartons,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then mh.TimeValueSlips
		else null
	end TimeValueSlipsCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then mh.AdminFee
		else null
	end GrossMailHouseAdminFee,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseGlueTack = 1 then mh.GlueTackRate
		else null
	end GlueTackCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseTabbing = 1 then mh.TabbingRate
		else null
	end TabbingCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseLetterInsertion = 1 then mh.LetterInsertionRate
		else null
	end LetterInsertionCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then ad.MailHouseOtherHandling
		else null
	end OtherMailHandlingCPM,
	ad.InsertFreightCWT, ad.InsertFuelSurcharge,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseMailTracking = 1 then mt.MailTracking
		else null
	end MailTrackingCPMRate,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then ad.OtherFreight
		else null
	end OtherFreight
from EST_Estimate e join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
	join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
	left join PPR_PaperGrade pg on c.PaperGrade_ID = pg.PPR_PaperGrade_ID
	left join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	left join PRT_PrinterRate pl on c.PlateCost_ID = pl.PRT_PrinterRate_ID /*Plates*/
	left join PRT_PrinterRate dh on c.DigitalHandlenPrepare_ID = dh.PRT_PrinterRate_ID /*Digi H&P*/
	left join PRT_PrinterRate smr on c.StitcherMakeready_ID = smr.PRT_PrinterRate_ID /*Stitcher Makeready*/
	left join PRT_PrinterRate pmr on c.PressMakeready_ID = pmr.PRT_PrinterRate_ID /*Press Makeready*/
	left join PPR_Paper_Map pm on c.Paper_Map_ID = pm.PPR_Paper_Map_ID
	left join PRT_PrinterRate si on c.StitchIn_ID = si.PRT_PrinterRate_ID
	left join PRT_PrinterRate bi on c.BlowIn_ID = bi.PRT_PrinterRate_ID
	left join PRT_PrinterRate oi on c.Onsert_ID = oi.PRT_PrinterRate_ID
	left join VND_MailTrackingRate mt on ad.MailTracking_ID = mt.VND_MailTrackingRate_ID
	left join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
	left join VND_MailListResourceRate ml on ad.MailListResource_ID = ml.VND_MailListResourceRate_ID
where e.EST_Estimate_ID = @EST_Estimate_ID

/* Perform Specification Calculations */
update #tempComponents
	set PieceWeight = (Width * Height) / cast(950000 as decimal) * PageCount * PaperWeight * 1.03

/* Perform Quantity Calculations */
update #tempComponents
	set MediaQuantity = InsertQuantity + SoloQuantity + PolyBagQuantity

update #tempComponents
	set SpoilageQuantity = isnull(SpoilagePct, 0) * MediaQuantity,
		DirectMailQuantity = SoloQuantity + PolybagQuantity

update #tempComponents
	set InternalMailQuantity = DirectMailQuantity - isnull(ExternalMailQuantity, 0)

update #tempComponents
	set TotalProductionQuantity = MediaQuantity + SpoilageQuantity + OtherQuantity + SampleQuantity

update tc
	set tc.TotalEstimateWeight = dbo.TotalEstimateWeight(EST_Estimate_ID)
from #tempComponents tc
where tc.EST_ComponentType_ID = 1

update #tempComponents
	set ProductionWeight = TotalProductionQuantity * PieceWeight

/* Perform Production Calculations */
update #tempComponents
	set GrossPrintCost = ManualPrintCost
where ManualPrintCost is not null

update #tempComponents
	set
		StitcherMakereadyCost = isnull(NumberOfPlants, 0) * isnull(StitcherMakereadyRate, 0),
		PressMakereadyCost = isnull(NumberOfPlants, 0) * isnull(PressMakereadyRate, 0)
where ManualPrintCost is null

update #tempComponents
	set GrossPrintCost = TotalProductionQuantity / cast(1000 as decimal) * isnull(RunRate, 0)
		+ StitcherMakereadyCost
		+ PressMakereadyCost
		+ isnull(AdditionalPlates, 0) * isnull(PrinterPlateCost, 0)
		+ isnull(NumberDigitalHandlePrepare, 0) * isnull(DigitalHandlePrepareRate, 0)
		+ isnull(ReplacementPlateCost, 0)
where ManualPrintCost is null

update #tempComponents
	set EarlyPayPrintDiscountAmount = GrossPrintCost * isnull(EarlyPayPrintDiscountPercent, 0),
		NetPrintCost = GrossPrintCost - (GrossPrintCost * isnull(EarlyPayPrintDiscountPercent, 0))

/*TODO: We don't have a paper handling amount if manualpapercost is entered.  How does this effect the print sales tax?? */
update #tempComponents
	set GrossPaperCost = ManualPaperCost
where ManualPaperCost is not null

update #tempComponents
	set TotalPaperPounds = TotalProductionQuantity / cast(1000 as decimal) * isnull(RunPounds, 0)
		+ cast(isnull(NumberOfPlants, 0) as bigint) * cast(isnull(MakeReadyPounds, 0) as bigint)
		+ cast(isnull(AdditionalPlates, 0) as bigint) * isnull(PlateChangePounds, 0)
		+ cast(isnull(NumberOfPressStops, 0) as bigint) * cast(isnull(PressStopPounds, 0) as bigint)
where ManualPaperCost is null

update #tempComponents
	set GrossPaperCost = isnull(TotalPaperPounds, 0) / cast(100 as decimal) * isnull(PaperCWTRate, 0)
where ManualPaperCost is null

update #tempComponents
	set EarlyPayPaperDiscountAmount = GrossPaperCost * isnull(EarlyPayPaperDiscountPercent, 0),
		NetPaperCost = GrossPaperCost - (GrossPaperCost * isnull(EarlyPayPaperDiscountPercent, 0))

update #tempComponents
	set PaperHandlingCost = isnull(TotalPaperPounds, 0) / cast(100 as decimal) * isnull(PaperHandlingCWTRate, 0)

update #tempComponents
	set PrinterSalesTaxAmount = (NetPrintCost + PaperHandlingCost) * isnull(PrinterTaxableMediaPct, 0) * isnull(PrinterSalesTaxPct, 0),
		PaperSalesTaxAmount = NetPaperCost * isnull(PaperTaxableMediaPct, 0) * isnull(PaperSalesTaxPct, 0)

update #tempComponents
	set TotalPrintCost = NetPrintCost + PrinterSalesTaxAmount,
		TotalPaperCost = NetPaperCost + PaperHandlingCost + PaperSalesTaxAmount,
		VendorProductionCost = TotalProductionQuantity / cast(1000 as decimal) * VendorProductionCPM

update #tempComponents
	set BlendedMailListCPM =
		case
			when dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) = 0 then 0
			else InternalMailCPM
					* cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) - ExternalMailQuantity as decimal)
					/ cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
				+ ExternalMailCPM
					* cast(ExternalMailQuantity as decimal)
					/ cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
		end
where EST_ComponentType_ID = 1

update #tempComponents
	set MailListCost = BlendedMailListCPM * SoloQuantity / 1000 + isnull(dbo.ComponentPolybagMailListCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponents
	set ProductionTotal = isnull(CreativeCost, 0) + isnull(SeparatorCost, 0) + TotalPrintCost + TotalPaperCost +  isnull(MailListCost, 0) +
		+ isnull(VendorProductionCost, 0) + isnull(OtherProductionCost, 0)

/* Assembly Calculations */

update #tempComponents
	set PolyBagCost = dbo.EstimatePolyBagCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1

update #tempComponents
	set OnsertCost = PolybagQuantity / cast(1000 as decimal) * OnsertRate
where EST_ComponentType_ID = 2

update #tempComponents
	set GrossInkjetMakereadyCost = NumberofPlants * InkjetMakereadyRate

update #tempComponents
	set PolyBagTotal = isnull(PolyBagCost, 0) + isnull(OnsertCost, 0),
		StitchInCost =
			case EST_ComponentType_ID
				when 3 then MediaQuantity / cast(1000 as decimal) * StitchInRate
				else null
			end,
		BlowInCost =
			case EST_ComponentType_ID
				when 4 then MediaQuantity / cast(1000 as decimal) * BlowInRate
			end,
		InkJetCost =
			case EST_ComponentType_ID
				when 1 then (isnull(SoloQuantity, 0) / cast(1000 as decimal) * InkJetRate) + isnull(dbo.EstimatePolybagInkjetCost(EST_Estimate_ID), 0)
			end,
		InkJetMakeReadyCost =
			case EST_ComponentType_ID
				when 1 then
					case
						when isnull(SoloQuantity, 0) = 0 then 0
						else GrossInkjetMakereadyCost * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
					end
					+ isnull(dbo.ComponentPolybagInkjetMakereadyCost(EST_Component_ID), 0)
			end

update #tempComponents
		set TotalInkJetCost = isnull(InkJetCost, 0) + isnull(InkJetMakeReadyCost, 0)

update tc
	set CartonRate = pr.Rate
from #tempComponents tc join EST_Component c on tc.EST_Component_ID = c.EST_Component_ID
	join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate pr on p.VND_Printer_ID = pr.VND_Printer_ID and pr.PRT_PrinterRateType_ID = 3 and pr.[Default] = 1

update #tempComponents
	set CornerGuardCost = TotalEstimateWeight / cast(2000 as decimal) * CornerGuardRate,
		SkidCost = TotalEstimateWeight / cast(2000 as decimal) * SkidRate,
		CartonCost = NumberOfCartons * CartonRate,
		TimeValueSlipsCost = TimeValueSlipsCPM * isnull(SoloQuantity, 0) / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseTimeValueSlipsCost(EST_Component_ID), 0),
		MailHouseAdminFee =
			case
				when isnull(SoloQuantity, 0) = 0 then 0
				else GrossMailHouseAdminFee * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
			end
			+ isnull(dbo.ComponentPolybagMailHouseAdminFee(EST_Component_ID), 0),
		GlueTackCost = GlueTackCPM * isnull(SoloQuantity, 0) / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseGlueTackCost(EST_Component_ID), 0),
		TabbingCost = TabbingCPM * isnull(SoloQuantity, 0) / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseTabbingCost(EST_Component_ID), 0),
		LetterInsertionCost = LetterInsertionCPM * isnull(SoloQuantity, 0) / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseLetterInsertionCost(EST_Component_ID), 0),
		OtherMailHandlingCost = OtherMailHandlingCPM * isnull(SoloQuantity, 0) / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseOtherDirectMailHandlingCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponents
	set InsertHandlingTotal = isnull(CornerGuardCost, 0) + isnull(SkidCost, 0) + isnull(CartonCost, 0),
		MailHandlingTotal = isnull(TimeValueSlipsCost, 0) + isnull(MailHouseAdminFee, 0) + isnull(GlueTackCost, 0) + isnull(TabbingCost, 0)
			+ isnull(LetterInsertionCost, 0) + isnull(OtherMailHandlingCost, 0)

update #tempComponents
	set HandlingTotal = InsertHandlingTotal + MailHandlingTotal

update #tempComponents
	set AssemblyTotal = PolyBagTotal + isnull(StitchInCost, 0) + isnull(BlowInCost, 0) + TotalInkJetCost + HandlingTotal

/* Distribution Calculations */

create table #tempPackagesByPubRate(
	EST_Component_ID bigint not null,
	EST_Package_ID bigint not null,
	PubRate_Map_ID bigint not null,
	PUB_PubRate_ID bigint not null,
	PUB_PubQuantity_ID bigint not null,
	QuantityType int not null,
	InsertDate datetime not null,
	IssueDOW int not null,
	BlowInRate int null,
	PackageTabPageCount int null,
	ComponentPieceWeight decimal(12,6) not null,
	PackageWeight decimal(12,6) not null,
	PackageSize int not null,
	BilledQuantity int,
	GrossPackageInsertCost money null,
	InsertDiscountPercent decimal(10,4),
	PackageComponentCost money null)

insert into #tempPackagesByPubRate(EST_Component_ID, EST_Package_ID, PubRate_Map_ID, PUB_PubRate_ID, PUB_PubQuantity_ID, QuantityType,
	InsertDate, IssueDOW, ComponentPieceWeight, PackageWeight, PackageSize)
select pcm.EST_Component_ID, p.EST_Package_ID, pprm.PUB_PubRate_Map_ID, dbo.CalcPubRateID(pprm.PUB_PubRate_Map_ID, pid.IssueDate),
	dbo.CalcPubQuantityID(pprm.PUB_PubRate_Map_ID, pid.IssueDate), p.PUB_PubQuantityType_ID, pid.IssueDate,
	case p.PUB_PubQuantityType_ID
		when 4 then 1 /* Thanksgiving always uses Sunday rates */
		when 5 then 1 /* Christmas always uses Sunday rates */
		when 6 then 1 /* New Years always uses Sunday rates */
		else pid.IssueDOW
	end IssueDOW,
	tc.PieceWeight, dbo.PackageWeight(p.EST_Package_ID), dbo.PackageSize(p.EST_Package_ID)
from #tempComponents tc join EST_PackageComponentMapping pcm on tc.EST_Component_ID = pcm.EST_Component_ID
	join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	join PUB_PubGroup pg on p.PUB_PubGroup_ID = pg.PUB_PubGroup_ID
	join PUB_PubPubGroup_Map ppgm on pg.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID
	join PUB_PubRate_Map pprm on ppgm.PUB_PubRate_Map_ID = pprm.PUB_PubRate_Map_ID
	join EST_PubIssueDates pid on p.EST_Estimate_ID = pid.EST_Estimate_ID and pprm.PUB_PubRate_Map_ID = pid.PUB_PubRate_Map_ID
where dbo.IsPubRateMapActive(pprm.PUB_PubRate_Map_ID, pid.IssueDate) = 1
	and dbo.CalcPubRateID(pprm.PUB_PubRate_Map_ID, pid.IssueDate) is not null
	and dbo.CalcPubQuantityID(pprm.PUB_PubRate_Map_ID, pid.IssueDate) is not null

update tp
	set tp.BlowInRate = pr.BlowInRate
from #tempPackagesByPubRate tp join PUB_PubRate pr on tp.PUB_PubRate_ID = pr.PUB_PubRate_ID

update #tempPackagesByPubRate
	set PackageTabPageCount = dbo.PackageTabPageCount(EST_Package_ID, BlowInRate)

update #tempPackagesByPubRate
	set BilledQuantity =
		case pr.QuantityChargeType
			when 1 then q.Quantity - (q.Quantity * pr.BilledPct)
			else q.Quantity
		end
from #tempPackagesByPubRate t join PUB_DayOfWeekQuantity q on t.PUB_PubQuantity_ID = q.PUB_PubQuantity_ID and t.QuantityType = q.PUB_PubQuantityType_ID
		and (t.QuantityType > 3 /*Holidays*/ or datepart(dw, t.InsertDate) = q.InsertDow /*Full Run / Contract Send*/) 
	join PUB_PubRate pr on t.PUB_PubRate_ID = pr.PUB_PubRate_ID

update #tempPackagesByPubRate
	set GrossPackageInsertCost = dbo.CalcGrossInsertCostforPackageandPub(PUB_PubRate_ID, InsertDate, IssueDOW, PackageTabPageCount, BilledQuantity, PackageWeight, PackageSize)

update t
	set InsertDiscountPercent = d.Discount
from #tempPackagesByPubRate t join PUB_InsertDiscounts d on t.PUB_PubRate_ID = d.PUB_PubRate_ID
where dbo.PackageInsertIndex(t.EST_Package_ID, t.PubRate_Map_ID, t.InsertDate) = d.[Insert]

update tp
	set PackageComponentCost =
		case
			when PackageWeight = 0 then 0
			else
				(GrossPackageInsertCost * (1 - isnull(InsertDiscountPercent, 0))) * ComponentPieceWeight / PackageWeight
		end
from #tempPackagesByPubRate tp

update tc
	set InsertCost = isnull((select sum(PackageComponentCost) from #tempPackagesByPubRate tp where tp.EST_Component_ID = tc.EST_Component_ID), 0)
from #tempComponents tc

drop table #tempPackagesByPubRate

update #tempComponents
	set InsertFreightCost = dbo.TotalEstimateWeight(#tempComponents.EST_Estimate_ID) / cast(100 as decimal) * InsertFreightCWT,
		InsertFuelSurchargeCost = (dbo.TotalEstimateWeight(#tempComponents.EST_Estimate_ID) / cast(100 as decimal) * InsertFreightCWT) * InsertFuelSurchargePercent
where #tempComponents.EST_ComponentType_ID = 1

update #tempComponents
	set InsertTotal = InsertCost + isnull(InsertFreightCost, 0) + isnull(InsertFuelSurchargeCost, 0)

update #tempComponents
	set PostalDropCost = dbo.EstimatePostalDropCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1


update #tempComponents
	set PostalDropFuelSurchargeCost = dbo.EstimatePostalDropFuelSurchargeCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1

update #tempComponents
	set	MailTrackingCost = isnull((SoloQuantity / cast(1000 as decimal) * MailTrackingCPMRate), 0) + isnull(dbo.ComponentPolybagMailTrackingCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponents
	set SoloPostageCost = isnull(dbo.ComponentSoloPostage(EST_Component_ID, PieceWeight, PST_PostalScenario_ID), 0)

update #tempComponents
	set PolyPostageCost = isnull(dbo.ComponentPolyPostage(EST_Component_ID, PieceWeight), 0)

update #tempComponents
	set TotalPostageCost = SoloPostageCost + PolyPostageCost

update #tempComponents
	set PostalTotal = isnull(PostalDropCost, 0) + isnull(PostalDropFuelSurchargeCost, 0) + isnull(MailTrackingCost, 0) + TotalPostageCost

update t
	set SampleFreight =
		case
			when isnull(s.FreightCWT, 0) <> 0 then dbo.EstimatePieceWeight(t.EST_Estimate_ID) * SampleQuantity / cast(100 as decimal) * s.FreightCWT
			else s.FreightFlat
		end
from #tempComponents t join EST_Samples s on t.EST_Estimate_ID = s.EST_Estimate_ID
where t.EST_ComponentType_ID = 1

update #tempComponents
	set OtherFreight = GrossOtherFreight
/* Not sure if this cost should be distributed across Polybags */
/*
			case
				when isnull(SoloQuantity, 0) = 0 then 0
				else GrossOtherFreight * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
			end
			+ isnull(dbo.ComponentPolybagOtherFreightCost(EST_Component_ID), 0)
*/
where EST_ComponentType_ID = 1

update #tempComponents
	set DistributionTotal = InsertTotal + isnull(PostalTotal, 0) + isnull(SampleFreight, 0) + isnull(OtherFreight, 0)

update #tempComponents
	set GrandTotal = ProductionTotal + AssemblyTotal + DistributionTotal

/* Add OtherQuantity to MediaQuantity for report */
update #tempComponents
	set MediaQuantity = MediaQuantity + OtherQuantity

/* Create the Total Cost Row for each estimate */
insert into #tempComponents(EST_Estimate_ID, Comments, Description,
	CreativeCost, SeparatorCost, ManualPrintCost, StitcherMakereadyCost, PressMakereadyCost, GrossPrintCost, EarlyPayPrintDiscountAmount,
	NetPrintCost,
	PrinterSalesTaxAmount, TotalPrintCost, ManualPaperCost, GrossPaperCost, EarlyPayPaperDiscountAmount, NetPaperCost, PaperHandlingCost,
	PaperSalesTaxAmount, TotalPaperCost, MailListCost, VendorProductionCost, OtherProductionCost, ProductionTotal, PolyBagCost, OnsertCost,
	PolyBagTotal, StitchInCost, BlowInCost, InkJetCost, InkJetMakeReadyCost, TotalInkJetCost, CornerGuardCost, SkidCost, CartonCost, InsertHandlingTotal,
	TimeValueSlipsCost, MailHouseAdminFee, GlueTackCost, TabbingCost, LetterInsertionCost, OtherMailHandlingCost, MailHandlingTotal,
	HandlingTotal, AssemblyTotal, InsertCost, InsertFreightCost, InsertFuelSurchargeCost, InsertTotal, PostalDropCost, PostalDropFuelSurchargeCost,
	MailTrackingCost, PostalTotal, SoloPostageCost, PolyPostageCost, TotalPostageCost, SampleFreight, OtherFreight, DistributionTotal, GrandTotal)
select EST_Estimate_ID, max(Comments), 'Estimate Total',
	sum(CreativeCost), sum(SeparatorCost), sum(ManualPrintCost), sum(StitcherMakereadyCost), sum(PressMakereadyCost), sum(GrossPrintCost),
	sum(EarlyPayPrintDiscountAmount),
	sum(NetPrintCost), sum(PrinterSalesTaxAmount), sum(TotalPrintCost), sum(ManualPaperCost), sum(GrossPaperCost), sum(EarlyPayPaperDiscountAmount),
	sum(NetPaperCost), sum(PaperHandlingCost), sum(PaperSalesTaxAmount), sum(TotalPaperCost), sum(MailListCost), sum(VendorProductionCost),
	sum(OtherProductionCost), sum(ProductionTotal), sum(PolyBagCost), sum(OnsertCost), sum(PolyBagTotal), sum(StitchInCost), sum(BlowInCost),
	sum(InkJetCost), sum(InkJetMakeReadyCost), sum(TotalInkJetCost), sum(CornerGuardCost), sum(SkidCost), sum(CartonCost), sum(InsertHandlingTotal),
	sum(TimeValueSlipsCost), sum(MailHouseAdminFee), sum(GlueTackCost), sum(TabbingCost), sum(LetterInsertionCost), sum(OtherMailHandlingCost),
	sum(MailHandlingTotal),
	sum(HandlingTotal), sum(AssemblyTotal), sum(InsertCost), sum(InsertFreightCost), sum(InsertFuelSurchargeCost), sum(InsertTotal),
	sum(PostalDropCost), sum(PostalDropFuelSurchargeCost), sum(MailTrackingCost),
	sum(PostalTotal), sum(SoloPostageCost), sum(PolyPostageCost), sum(TotalPostageCost),
	sum(SampleFreight), sum(OtherFreight), sum(DistributionTotal), sum(GrandTotal)
from #tempComponents
group by EST_Estimate_ID

set nocount off
select * from #tempComponents
order by EST_Estimate_ID,
	case
		when EST_Component_ID is null then 0
		else 1
	end,
	case
		when EST_ComponentType_ID = 1 then 0
		else 1
	end,
	EST_Component_ID

set nocount on

drop table #tempComponents
GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_s_CostSummary_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstEstimate_s_CreatedBy') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstEstimate_s_CreatedBy'
	DROP PROCEDURE dbo.EstEstimate_s_CreatedBy
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstEstimate_s_CreatedBy') IS NOT NULL
		PRINT '***********Drop of dbo.EstEstimate_s_CreatedBy FAILED.'
END
GO
PRINT 'Creating dbo.EstEstimate_s_CreatedBy'
GO

CREATE PROCEDURE dbo.EstEstimate_s_CreatedBy
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Returns a list of unique user names that have created estimates
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   est_estimate		Read
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/20/2007      NLS             Initial Creation 
*
*/
AS BEGIN

SELECT 
	DISTINCT createdby

FROM dbo.est_estimate

ORDER BY createdby ASC

END
GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_s_CreatedBy]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstEstimate_s_FiscalYear') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstEstimate_s_FiscalYear'
	DROP PROCEDURE dbo.EstEstimate_s_FiscalYear
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstEstimate_s_FiscalYear') IS NOT NULL
		PRINT '***********Drop of dbo.EstEstimate_s_FiscalYear FAILED.'
END
GO
PRINT 'Creating dbo.EstEstimate_s_FiscalYear'
GO

CREATE PROCEDURE dbo.EstEstimate_s_FiscalYear
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Returns a list of unique fiscal years used by estimates.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   est_estimate		Read
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 08/13/2007      NLS             Initial Creation 
*
*/
AS BEGIN

SELECT 
	DISTINCT fiscalyear

FROM dbo.est_estimate

ORDER BY fiscalyear ASC

END
GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_s_FiscalYear]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstEstimate_s_Search') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstEstimate_s_Search'
	DROP PROCEDURE dbo.EstEstimate_s_Search
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstEstimate_s_Search') IS NOT NULL
		PRINT '***********Drop of dbo.EstEstimate_s_Search FAILED.'
END
GO
PRINT 'Creating dbo.EstEstimate_s_Search'
GO

CREATE PROC dbo.EstEstimate_s_Search
/*
* PARAMETERS:
*	@EstEstimateId - Estimate ID to directly search for
*
*
* DESCRIPTION:
*		An Estimates isfound based on an estimate id for the Estimate Search screen.  At least one parameter is required.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate
*   EST_EstimateMediaType
*   EST_Season
*   EST_Status

* DATABASE:
*		All
*
*
* RETURN VALUE:
*	EstEstimate Row for the Search Grid
*
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 08/14/2007      NLS             Initial Creation 
* 09/04/2007      BJS             Renamed to EstEstimate_s_Search
* 09/11/2007	  NLS		  Added SortOrder column and fixed UploadDate issue
* 10/01/2007	  NLS		  Removed dummy TotalCost column.  Filled in client side from another proc
* 11/05/2007	  JRH		  Changed the SortOrder.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/

@EstEstimateId bigint

AS

select
	EST_Estimate_ID,
	ParentId, 
	RunDate, 
	Description, 
	HostMediaDesc, 
	SeasonDesc,
	StatusDesc,
	UploadKillDate, 
	displayed,
	CAST(Description as char(35)) + substring(Est_ID_text, len(Est_ID_text)-9, 10) 
		+ CONVERT(varchar(30), isnull(UploadKillDate, '1/1/1900'), 12) SortOrder
FROM
	(SELECT e.EST_Estimate_ID, 
		   max(e.parent_id) ParentId, 
		   max(e.RunDate) RunDate, 
		   max(e.Description) Description, 
		   max(mt.Description) HostMediaDesc, 
		   max(s.Description) SeasonDesc,
		   max(st.Description) StatusDesc,
	
		   case max(e.EST_Status_ID)
		       when 2 then max(e.uploaddate)
			   when 3 then max(e.ModifiedDate)
			   else null
		   end UploadKillDate, 
			
		   1 displayed,
		   
		('0000000000' + CAST(isnull(max(e.parent_id),e.est_estimate_id) AS varchar(10))) Est_ID_text
	
	FROM
	
		EST_Estimate e 	left join EST_Component hc on e.EST_Estimate_ID = hc.EST_Estimate_ID and hc.EST_ComponentType_ID = 1
	    left join EST_EstimateMediaType mt on hc.EST_EstimateMediaType_ID = mt.EST_EstimateMediaType_ID
		join EST_Season s on e.EST_Season_ID = s.EST_Season_ID
		join EST_Status st on e.EST_Status_ID = st.EST_Status_ID
	
	WHERE
	
		e.EST_Estimate_ID = @EstEstimateId
		
	GROUP BY e.EST_Estimate_ID)a

GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_s_Search]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.EstEstimate_u') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstEstimate_u'
	DROP PROCEDURE dbo.EstEstimate_u
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstEstimate_u') IS NOT NULL
		PRINT '***********Drop of dbo.EstEstimate_u FAILED.'
END
GO
PRINT 'Creating dbo.EstEstimate_u'
GO

create proc dbo.EstEstimate_u
/*
* PARAMETERS:
* EST_Estimate_ID - The ID of the record to update.
*
*
* DESCRIPTION:
*		Updates an Estimate in the EST_Estimate table.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate        UPDATE
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
* 05/25/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint,
@Description varchar(35),
@Comments varchar(255),
@EST_Season_ID int,
@RunDate datetime,
@EST_Status_ID int,
@Parent_ID int,
@UploadDate datetime,
@ModifiedBy varchar(50)
as

begin tran t

if (@EST_Status_ID = 3) begin
	-- Only Active Estimates can be killed
	if exists(select 1 from EST_Estimate where EST_Estimate_ID = @EST_Estimate_ID and EST_Status_ID not in (1,3)) begin
		rollback tran t
		raiserror('Error updating Estimate.', 16, 1)
		return
	end

	--Estimates cannot be killed if they are a member of a polybag
	if exists(select 1 from EST_Estimate e join EST_EstimatePolybagGroup_Map pbgm on e.EST_Estimate_ID = pbgm.EST_Estimate_ID
		where e.EST_Estimate_ID = @EST_Estimate_ID) begin

		rollback tran t
		raiserror('Error updating Estimate.', 16, 1)
		return
	end
end
		
update EST_Estimate
	set Description = @Description,
		Comments = @Comments,
		EST_Season_ID = @EST_Season_ID,
		FiscalYear = dbo.getFiscalYear(@RunDate),
		RunDate = @RunDate,
		EST_Status_ID = @EST_Status_ID,
		FiscalMonth = dbo.getFiscalMonth(@RunDate),
		Parent_ID = @Parent_ID,
		UploadDate = @UploadDate,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where EST_Estimate_ID = @EST_Estimate_ID
if (@@error <> 0) begin
	rollback tran t
	raiserror('Error updating EST_Estimate.', 16, 1)
	return
end

commit tran t

GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_u]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstEstimate_Upload') IS NOT NULL
	BEGIN
		PRINT 'Dropping dbo.EstEstimate_Upload'
		DROP PROCEDURE dbo.EstEstimate_Upload
		-- Tell user if drop failed.
		IF OBJECT_ID('dbo.EstEstimate_Upload') IS NOT NULL
			PRINT ' *** Drop of dbo.EstEstimate_Upload FAILED. *** '
	END
	GO
	
PRINT 'Creating dbo.EstEstimate_Upload'
GO

CREATE PROCEDURE [dbo].[EstEstimate_Upload] 
/*

* PARAMETERS:
* EstimateIDs - XML string, the estimates to upload
*
* DESCRIPTION:
*   Uploads the specified estimates to the admin system.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate
*   EST_Component
*   EST_AssemDistribOptions
*   PPR_PaperWeight
*   PPR_PaperGrade
*   VND_Printer
*   PRT_PrinterRate
*   PPR_Paper_Map
*   VND_MailHouse
*   VND_MailTracker
*
*
* PROCEDURES CALLED:
*   dbo.TotalEstimateWeight
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
* Date		    Who     Comments
* ----------	---		-------------------------------------------------
* 08/23/2007	TJU		Initial Creation 
* 08/29/2007	BJS		Fixed join to PrinterRate table for plate cost
* 09/10/2007	BJS		Modified to allow XML input
* 09/24/2007	BJS		Modified logic for Digi H&P, Stitcher MR and Press MR
* 10/04/2007	BJS		Zero Records are now written to pub_cost_est and ctlg_pubvr_distbn when a pub quantity & rate do not exist in PMES
* 10/08/2007	BJS		Explicitly cast integer values to decimal (ie 1000) to prevent rounding errors during calculations
* 10/23/2007	JRH		Added ability to insert into DBADVProd.informix.ctlg_pubvr_distbn.
* 10/24/2007    BJS     	Join to Paper Grade and VND_Printer are now left joins
* 10/23/2007	BJS		Performance Improvements.
* 10/23/2007	JRH		Handle null rate ids as zero.
* 11/07/2007	JRH		Set paper grade description to "NONE" if null.
* 11/26/2007	JRH		Fixed SampleFreight, InsertFuelSurchargeCost, and InsertFreightCost to allocate all to host.
*				Fixed InsertTotal to account for nulls.
*				Check PostalTotal for null.
* 12/07/2007	JRH		Fixed the InsertDiscount calculation.
* 12/11/2007    BJS     Modified EstimateIDs parameter to allow 4000 characters
* 12/12/2007    BJS     Insert Costs are now prorated to components by tab page count when using a tab page rate
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EstimateIDs varchar(4000)
as

set nocount on

declare @EstimateDocID int

create table #tempEstimate(EST_Estimate_ID BIGINT NOT NULL)

exec sp_xml_preparedocument @EstimateDocID output, @EstimateIDs
insert into #tempEstimate(EST_Estimate_ID)
select EST_Estimate_ID
from OPENXML(@EstimateDocID, '/root/estimate')
with(est_estimate_id BIGINT '@id')

create table #tempComponents(
	EST_Component_ID bigint,
	EST_Estimate_ID bigint,
	EST_ComponentType_ID int,
	EST_EstimateMediaType_ID int,
	PST_PostalScenario_ID bigint,
	Validation_msg char(40),
	EST_RunDate datetime,
	EST_Description varchar(35),
	ESTc_Description varchar(35),
	ESTc_MediaQtyWOInserts int,

	/* <Main> */
	AdNumber int,
	Description varchar(35),
	/* </Main> */ 

	/* <Specifications> */
	PageCount int,
	Width decimal(10,4),
	Height decimal(10,4),
	/* </Specifications> */

	/* <Paper> */
	PaperWeight int,
	PaperGradeID int,
	PaperGradeDescription varchar(35),
	PieceWeight decimal(12,6), 		/* (Width * Height) / 950,000 * PageCount * PaperWeight * 1.03 */
	/* </Paper> */

	/* <Quantity> */
	InsertQuantity int,
	SoloQuantity int,
	PolyBagQuantity int,
	OtherQuantity int,
	SampleQuantity int,
	MediaQuantity int, 			/* Insert + Solo + PolyBag */
	SpoilagePct decimal(10,4),
	SpoilageQuantity int, 			/* SpoilagePct * Media Quantity */
	DirectMailQuantity int, 		/* Solo + Poly */
	InternalMailQuantity int, 		/* DirectMail - External List Quantity */
	ExternalMailQuantity int, 		/* External List Quantity */
	TotalProductionQuantity int, 		/* MediaQuantity + SpoilageQuantity + OtherQuantity + SampleQuantity */
	/* </Quantity> */

	/* <Production> */
	ProductionWeight decimal(14,6), 	/* TotalProductionQuantity * PieceWeight */
	TotalEstimateWeight decimal(14,6), 	/* sum(ProductionWeight) group by EST_Estimate_ID*/
	CreativeCPP money,
	CreativeCost money,
	SeparatorCPP money,
	SeparatorCost money,
	/* </Production> */

	/* <Print> */
	ManualPrintCost money,
	RunRate money,
	NumberOfPlants int,
	AdditionalPlates int,
	PrinterPlateCost money,
	NumberDigitalHandlePrepare int,
	DigitalHandlePrepareRate money,
	StitcherMakeReadyRate money,
	PressMakeReadyRate money,
	ReplacementPlateCost money,
	GrossPrintCost money, 			/* See Estimate Calculation Steps for formula */
	EarlyPayPrintDiscountPercent decimal(10,4),
	EarlyPayPrintDiscountAmount money, 	/* GrossPrintCost * EarlyPayPrintDiscountPercent */
	NetPrintCost money, 			/* GrossPrintCost - EarlyPayPrintDiscountAmount */
	PrinterTaxableMediaPct decimal(10,4),
	PrinterSalesTaxPct decimal(10,4),
	PrinterSalesTaxAmount money, 		/* (NetPrinterCost + PaperHandling) * PrinterTaxableMediaPct * PrinterSalesTaxPct */
	TotalPrintCost money, 			/* NetPrintCost + PrinterSalesTaxAmount */
	
	/* </Print> */
	
	/* <Paper> */
	ManualPaperCost money,
	RunPounds decimal(10,2),
	MakereadyPounds int,
	PlateChangePounds decimal(10,2),
	NumberOfPressStops int,
	PressStopPounds int,
	TotalPaperPounds decimal(20,2),		/* See Estimate Calculation Steps for formula */
	PaperCWTRate money, 			/* PPR_Paper_Map.CWT */
	GrossPaperCost money, 			/* TotalPaperPounds / 100 * PaperCWTRate */
	EarlyPayPaperDiscountPercent decimal(10,4),
	EarlyPayPaperDiscountAmount money, 	/* GrossPaperCost * EarlyPayPaperDiscountPercent */
	NetPaperCost money, 			/* GrossPaperCost - EarlyPayPaperDiscountAmount */
	PaperHandlingCWTRate money, 		/* VND_Printer.PaperHandling */
	PaperHandlingCost money, 		/* TotalPaperPounds * PaperHandlingCWTRate */
	PaperTaxableMediaPct decimal(10,4),
	PaperSalesTaxPct decimal(10,4),
	PaperSalesTaxAmount money, 		/* NetPaperCost * PaperTaxableMediaPct * PaperSalesTaxPct */
	TotalPaperCost money, 			/* NetPaperCost + PaperSalesTaxAmount */
	/* </Paper> */

	/* <Mail List> */
	InternalMailCPM money,
	ExternalMailCPM money,
	BlendedMailListCPM money,
	MailListCost money, 			/* See Logic below */
	/* </Mail List> */		

	VendorProductionCPM money,
	VendorProductionCost money, 		/* TotalProductionQuantity / 1000 * VendorProductionCPM  */
	OtherProductionCost money,
	ProductionTotal money, 			/* CreativeCost + SeparatorCost + TotalPrintCost + TotalPaperCost + ExternalMailCost + VendorProductionCost + OtherProductionCost */
	
	/* <Assembly> */

	/* <PolyBag> */
	PolyBagCost money, 			/* dbo.EstimatePolyBagCost */
	OnsertRate money,
	OnsertCost money, 			/* Only if component type is Host Onsert -- PolyBagQuantity / 1000 * OnsertRate */
	PolyBagTotal money, 			/* PolyBagCost + OnsertCost */
	/* </PolyBag> */

	StitchInRate money,
	StitchInCost money, 			/* StitchInRate * MediaQuantity (if comp type is stitch-in) / 1000 */
	BlowInRate money,
	BlowInCost money, 			/* BlowInRate * MediaQuantity (if comp type is blow-in) / 1000 */
	
	/* <Ink Jet> */
	InkJetRate money,
	InkJetMakeReadyRate money,
	InkJetCost money, 			/* DirectMailQuantity / 1000 * InkJetRate */
	GrossInkjetMakereadyCost money, 	/* InkJetMakeReadyRate * NumberOfPlants */
	InkjetMakereadyCost money, 		/* See Logic below */
	TotalInkJetCost money, 			/* InkJetCost + InkJetMakeReadyCost */
	/* <Ink Jet> */

	/* Handling */

	/* <Insert> */
	CornerGuardRate money,
	CornerGuardCost money, 			/* (MediaQuantity * PieceWeight) / 2000 * CornerGuardRate */
	SkidRate money,
	SkidCost money, 			/* (MediaQuantity * PieceWeight) / 2000 * SkidRate */
	NumberOfCartons int,
	CartonRate money,
	CartonCost money,	 		/* NumberOfCartons * CartonCost */
	InsertHandlingTotal money, 		/* CornerGuardCost + SkidCost + CartonCost */
	/* </Insert> */

	/* <Mail> */
	TimeValueSlipsCPM money,
	TimeValueSlipsCost money, 		/* See SQL Logic Below */
	GrossMailHouseAdminFee money, 		/* The Mail House Admin Fee. */
	MailHouseAdminFee money, 		/* See SQL Logic Below */
	GlueTackCPM money,
	GlueTackCost money, 			/* See SQL Logic Below */
	TabbingCPM money,
	TabbingCost money, 			/* See SQL Logic Below */
	LetterInsertionCPM money,
	LetterInsertionCost money, 		/* See SQL Logic Below */
	OtherMailHandlingCPM money,
	OtherMailHandlingCost money, 		/* See SQL Logic Below */
	MailHandlingTotal money, 		/* See SQL Logic Below */
	/* </Mail> */

	HandlingTotal money, 			/* InsertHandlingTotal + MailHandlngTotal */
	AssemblyTotal money, 			/* PolyBagTotal + StitchInCost + BlowInCost + TotalInkJetCost + HandlingTotal */
	
	/* <Distribution> */

	/* <Insert> */
	InsertCost money, 			/* See Logic below */
	InsertFreightCWT money,
	InsertFreightCost money, 		/* InsertQuantity * PieceWeight / 100 * InsertFreightCWT */
	InsertFuelSurchargePercent decimal(10,4),
	InsertFuelSurchargeCost money, 		/* InsertFreightCost * InsertFreightSurchargePercent */
	InsertTotal money, 			/* InsertCost + InsertFreightCost + InsertFuelSurchargeCost */
	/* </Insert> */

	/* <Postal> */
	PostalDropCost money, 			    /* See Logic Below */
	PostalDropFuelSurchargeCost money, 	/* See Logic Below*/
	PostalDropTotalCost money,          /* PostalDropCost + PostalDropFuelSurchageCost */
	MailTrackingCPMRate money,
	MailTrackingCost money, 		/* DirectMailQuantity / 1000 * MailTrackingCPMRate */
	SoloPostageCost money,
	PolyPostageCost money,
	TotalPostageCost money, 		/* SoloPostageCost + PolyPostageCost */
	PostalTotal money, 			/*Postage PostalDropTotalCost + MailTrackingCost + TotalPostageCost */
	/* </Postal> */

	SampleFreight money,
	GrossOtherFreight money,
	OtherFreight money,

	DistributionTotal money, 		/* InsertTotal + PostalTotal + SampleFreight + OtherFreight */ 			

	GrandTotal money
	/* </Assembly> */
	/* </Distribution> */
)

/* Get Raw Production Data */
insert into #tempComponents
	(EST_Component_ID, EST_Estimate_ID, EST_ComponentType_ID, EST_EstimateMediaType_ID,
	PST_PostalScenario_ID, EST_RunDate, EST_Description, 
	ESTc_Description, ESTc_MediaQtyWOInserts, AdNumber, Description, PageCount, Width, Height, PaperWeight, PaperGradeDescription,
	CreativeCPP, CreativeCost, SeparatorCPP, SeparatorCost, ExternalMailQuantity, SpoilagePct, 
 	ManualPrintCost, RunRate, NumberOfPlants, AdditionalPlates, 
	PrinterPlateCost, NumberDigitalHandlePrepare, DigitalHandlePrepareRate, StitcherMakeReadyRate, PressMakeReadyRate, ReplacementPlateCost, 
	EarlyPayPrintDiscountPercent, PrinterTaxableMediaPct, PrinterSalesTaxPct, ManualPaperCost, 
	RunPounds, MakeReadyPounds, PlateChangePounds, NumberOfPressStops, 
	PressStopPounds, PaperCWTRate, EarlyPayPaperDiscountPercent, PaperHandlingCWTRate, 
	PaperTaxableMediaPct, PaperSalesTaxPct,  InternalMailCPM, ExternalMailCPM,
	VendorProductionCPM, OtherProductionCost, OnsertRate, StitchInRate, BlowInRate, InkJetRate, 
	InkJetMakeReadyRate, CornerGuardRate, SkidRate, NumberOfCartons, TimeValueSlipsCPM, 
	GrossMailHouseAdminFee, GlueTackCPM, TabbingCPM, LetterInsertionCPM, OtherMailHandlingCPM, InsertFreightCWT,
	InsertFuelSurchargePercent, MailTrackingCPMRate, GrossOtherFreight)

select 	c.EST_Component_ID, c.EST_Estimate_ID, c.EST_ComponentType_ID, c.EST_EstimateMediaType_ID,
	ad.PST_PostalScenario_ID, e.RunDate, e.Description,
	c.Description,  c.mediaqtywoinsert, c.AdNumber, c.Description, c.PageCount, c.Width, c.Height, pw.Weight, isnull(pg.Grade, 'NONE'),
	c.CreativeCPP, c.CreativeCPP * c.PageCount, c.SeparatorCPP, c.SeparatorCPP * c.PageCount, ad.ExternalMailQty, c.SpoilagePct, c.PrintCost, c.RunRate,
	c.NumberOfPlants, c.AdditionalPlates, pl.Rate, c.NumberDigitalHandlenPrepare, dh.Rate,
	case
		when c.StitcherMakeready_ID is null then c.StitcherMakereadyRate
		else simr.Rate
	end StitcherMakereadyRate,
	case
		when c.PressMakeready_ID is null then c.PressMakereadyRate
		else prmr.Rate
	end PressMakereadyRate,
	c.ReplacementPlateCost, c.EarlyPayPrintDiscount, c.PrinterTaxableMediaPct, c.PrinterSalesTaxPct, 
	c.PaperCost, c.RunPounds, c.MakeReadyPounds, c.PlateChangePounds, c.NumberOfPressStops,
	c.PressStopPounds, pm.CWT, c.EarlyPayPaperDiscount, p.PaperHandling, c.PaperTaxableMediaPct, c.PaperSalesTaxPct,
	case	when c.EST_ComponentType_ID = 1 then ml.InternalListRate
		else null end 
 	InternalMailCPM,
	case 	when c.EST_ComponentType_ID = 1 then ad.ExternalMailCPM
		else null end 
	ExternalMailCPM, 
	c.VendorCPM, c.OtherProduction,
	oi.Rate, 
	si.Rate, 
	bi.Rate,
	case
		when c.EST_ComponentType_ID = 1 then mh.InkJetRate
		else null end 
	InkJetRate,
	case
		when c.EST_ComponentType_ID = 1 then mh.InkJetMakeReady
		else null end 
	InkJetMakeReady,
	case when c.EST_ComponentType_ID = 1 /*Host*/ and ad.CornerGuards = 1 then p.CornerGuard
		else null end 
	CornerGuardRate,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.Skids = 1 then p.Skid
		else null end 
	SkidRate,
	ad.NbrOfCartons,
	case	when c.EST_ComponentType_ID = 1 /*Host*/ then mh.TimeValueSlips
		else null end 
	TimeValueSlipsCPM,
	case 	when c.EST_ComponentType_ID = 1 /*Host*/ then mh.AdminFee
		else null end 
	GrossMailHouseAdminFee,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseGlueTack = 1 then mh.GlueTackRate
		else null
	end GlueTackCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseTabbing = 1 then mh.TabbingRate
		else null
	end TabbingCPM,
	case 	when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseLetterInsertion = 1 then mh.LetterInsertionRate
		else null end 
	LetterInsertionCPM,
	case 	when c.EST_ComponentType_ID = 1 /*Host*/ then ad.MailHouseOtherHandling
		else null end 
	OtherMailHandlingCPM,
	ad.InsertFreightCWT, 
	ad.InsertFuelSurcharge,
	case	when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseMailTracking = 1 then mt.MailTracking
		else null end 
	MailTrackingCPMRate,
	case 	when c.EST_ComponentType_ID = 1 /*Host*/ then ad.OtherFreight
		else null end 
	OtherFreight

from #tempEstimate te join EST_Component c on te.EST_Estimate_ID = c.EST_Estimate_ID
	join dbo.EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
	join EST_Estimate e on e.EST_Estimate_ID = c.EST_Estimate_ID
	join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
	left join PPR_PaperGrade pg on c.PaperGrade_ID = pg.PPR_PaperGrade_ID
	left join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	left join PRT_PrinterRate pl on c.PlateCost_ID = pl.PRT_PrinterRate_ID /*Plates*/	
	left join PRT_PrinterRate dh on c.DigitalHandlenPrepare_ID = dh.PRT_PrinterRate_ID /*Digi H&P*/
	left join PRT_PrinterRate simr on c.StitcherMakeready_ID = simr.PRT_PrinterRate_ID /* Stitcher Makeready */
	left join PRT_PrinterRate prmr on c.PressMakeready_ID = prmr.PRT_PrinterRate_ID /* Press Makeready */
	left join PPR_Paper_Map pm on c.Paper_Map_ID = pm.PPR_Paper_Map_ID
	left join PRT_PrinterRate si on c.StitchIn_ID = si.PRT_PrinterRate_ID
	left join PRT_PrinterRate bi on c.BlowIn_ID = bi.PRT_PrinterRate_ID
	left join PRT_PrinterRate oi on c.Onsert_ID = oi.PRT_PrinterRate_ID
	left join VND_MailTrackingRate mt on ad.MailTracking_ID = mt.VND_MailTrackingRate_ID
	join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
	left join VND_MailListResourceRate ml on ad.MailListResource_ID = ml.VND_MailListResourceRate_ID
where c.AdNumber is not null or c.EST_ComponentType_ID = 5

/* Components of type +CVR share the Host Ad Number if they do not have an Ad Number*/
update cvrcomp
	set cvrcomp.AdNumber = hostcomp.AdNumber
from #tempComponents hostcomp join #tempComponents cvrcomp on hostcomp.EST_Estimate_ID = cvrcomp.EST_Estimate_ID
where hostcomp.EST_ComponentType_ID = 1 and cvrcomp.EST_ComponentType_ID = 5 and cvrcomp.AdNumber is null

/* Perform Specification Calculations */
update #tempComponents
	set PieceWeight = (Width * Height) / cast(950000 as decimal) * PageCount * PaperWeight * 1.03

/* Perform Quantity Calculations */
update tc
	set tc.InsertQuantity = isnull(iq.InsertQuantity, 0)
from #tempComponents tc left join vwComponentInsertQuantity iq on tc.EST_Component_ID = iq.EST_Component_ID

update tc
	set tc.SoloQuantity = isnull(sq.SoloQuantity, 0)
from #tempComponents tc left join vwComponentSoloQuantity sq on tc.EST_Component_ID = sq.EST_Component_ID

update tc
	set tc.PolybagQuantity = isnull(pq.PolybagQuantity, 0)
from #tempComponents tc left join vwComponentPolybagQuantity pq on tc.EST_Component_ID = pq.EST_Component_ID

update tc
	set tc.OtherQuantity = isnull(oq.OtherQuantity, 0)
from #tempComponents tc left join vwComponentOtherQuantity oq on tc.EST_Component_ID = oq.EST_Component_ID

update tc
	set tc.SampleQuantity = isnull(sq.SampleQuantity, 0)
from #tempComponents tc left join vwComponentSampleQuantity sq on tc.EST_Component_ID = sq.EST_Component_ID

update tc
	set tc.MediaQuantity = mq.MediaQuantity
from #tempComponents tc join vwComponentMediaQuantity mq on tc.EST_Component_ID = mq.EST_Component_ID

update #tempComponents
	set SpoilageQuantity = isnull(SpoilagePct, 0) * MediaQuantity,
		DirectMailQuantity = SoloQuantity + PolybagQuantity

update #tempComponents
		set InternalMailQuantity = DirectMailQuantity - isnull(ExternalMailQuantity, 0)

update #tempComponents
	set TotalProductionQuantity = MediaQuantity + SpoilageQuantity + OtherQuantity + SampleQuantity

update tc
	set tc.TotalEstimateWeight = tw.TotalEstimateWeight
from #tempComponents tc join vwTotalEstimateWeight tw on tc.EST_Estimate_ID = tw.EST_Estimate_ID
where tc.EST_ComponentType_ID = 1

update #tempComponents
	set ProductionWeight = TotalProductionQuantity * PieceWeight

/* Perform Production Calculations */
update #tempComponents
	set GrossPrintCost = ManualPrintCost
where ManualPrintCost is not null

update #tempComponents
	set GrossPrintCost = TotalProductionQuantity / cast(1000 as decimal) * isnull(RunRate, 0)
		+ isnull(NumberOfPlants, 0) * (isnull(StitcherMakeReadyRate, 0) + isnull(PressMakeReadyRate, 0))
		+ isnull(AdditionalPlates, 0) * isnull(PrinterPlateCost, 0)
		+ isnull(NumberDigitalHandlePrepare, 0) * isnull(DigitalHandlePrepareRate, 0)
		+ isnull(ReplacementPlateCost, 0)
where ManualPrintCost is null

update #tempComponents
	set EarlyPayPrintDiscountAmount = GrossPrintCost * isnull(EarlyPayPrintDiscountPercent, 0),
		NetPrintCost = GrossPrintCost - (GrossPrintCost * isnull(EarlyPayPrintDiscountPercent, 0))

/*TODO: We don't have a paper handling amount if manualpapercost is entered.  How does this effect the print sales tax?? */
update #tempComponents
	set GrossPaperCost = ManualPaperCost
where ManualPaperCost is not null

update #tempComponents
	set TotalPaperPounds = TotalProductionQuantity / cast(1000 as decimal) * isnull(RunPounds, 0)
		+ cast(isnull(NumberOfPlants, 0) as bigint) * cast(isnull(MakeReadyPounds, 0) as bigint)
		+ cast(isnull(AdditionalPlates, 0) as bigint) * isnull(PlateChangePounds, 0)
		+ cast(isnull(NumberOfPressStops, 0) as bigint) * cast(isnull(PressStopPounds, 0) as bigint)
where ManualPaperCost is null

update #tempComponents
	set GrossPaperCost = isnull(TotalPaperPounds, 0) / cast(100 as decimal) * isnull(PaperCWTRate, 0)
where ManualPaperCost is null

update #tempComponents
	set EarlyPayPaperDiscountAmount = GrossPaperCost * isnull(EarlyPayPaperDiscountPercent, 0),
		NetPaperCost = GrossPaperCost - (GrossPaperCost * isnull(EarlyPayPaperDiscountPercent, 0))

update #tempComponents
	set PaperHandlingCost = isnull(TotalPaperPounds, 0) / cast(100 as decimal) * isnull(PaperHandlingCWTRate, 0)

update #tempComponents
	set PrinterSalesTaxAmount = (NetPrintCost + PaperHandlingCost) * isnull(PrinterTaxableMediaPct, 0) * isnull(PrinterSalesTaxPct, 0),
		PaperSalesTaxAmount = NetPaperCost * isnull(PaperTaxableMediaPct, 0) * isnull(PaperSalesTaxPct, 0)

update #tempComponents
	set TotalPrintCost = NetPrintCost + PrinterSalesTaxAmount,
		TotalPaperCost = NetPaperCost + PaperHandlingCost + PaperSalesTaxAmount

update #tempComponents
	set VendorProductionCost = isnull(TotalProductionQuantity, 0) / cast(1000 as decimal) * isnull(VendorProductionCPM, 0)

update #tempComponents
	set BlendedMailListCPM =
		case
			when dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) = 0 then 0
			else InternalMailCPM
					* cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) - ExternalMailQuantity as decimal)
					/ cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
				+ ExternalMailCPM
					* cast(ExternalMailQuantity as decimal)
					/ cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
		end
where EST_ComponentType_ID = 1

update #tempComponents
	set MailListCost = BlendedMailListCPM * SoloQuantity / 1000 + isnull(dbo.ComponentPolybagMailListCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponents
	set ProductionTotal = isnull(CreativeCost, 0) + isnull(SeparatorCost, 0) + TotalPrintCost + TotalPaperCost + isnull(MailListCost, 0) +
		+ isnull(VendorProductionCost, 0) + isnull(OtherProductionCost, 0)

/* Assembly Calculations */

update #tempComponents set PolyBagCost = dbo.EstimatePolyBagCost(EST_Estimate_ID) where EST_ComponentType_ID = 1

update #tempComponents set OnsertCost = PolybagQuantity / cast(1000 as decimal) * isnull(OnsertRate, 0) where EST_ComponentType_ID = 2

update #tempComponents set GrossInkjetMakereadyCost = isnull(NumberofPlants, 0) * isnull(InkjetMakereadyRate, 0)

update #tempComponents
	set
		PolyBagTotal = isnull(PolyBagCost, 0) + isnull(OnsertCost, 0),
		StitchInCost =
			case EST_ComponentType_ID
				when 3 then MediaQuantity / cast(1000 as decimal) * StitchInRate
			end,
		BlowInCost =
			case EST_ComponentType_ID
				when 4 then MediaQuantity / cast(1000 as decimal) * BlowInRate
			end,
		InkJetCost =
			case EST_ComponentType_ID
				when 1 then (SoloQuantity / cast(1000 as decimal) * InkJetRate) + isnull(dbo.EstimatePolybagInkjetCost(EST_Estimate_ID), 0)
			end,
		InkJetMakeReadyCost =
			case EST_ComponentType_ID
				when 1 then
					case
						when isnull(SoloQuantity, 0) = 0 then 0
						else GrossInkjetMakereadyCost * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
					end
					+ isnull(dbo.ComponentPolybagInkjetMakereadyCost(EST_Component_ID), 0) 
			end

update #tempComponents set TotalInkJetCost = isnull(InkJetCost, 0) + isnull(InkJetMakeReadyCost, 0)

update tc set CartonRate = pr.Rate
from #tempComponents tc join EST_Component c on tc.EST_Component_ID = c.EST_Component_ID
	join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate pr on p.VND_Printer_ID = pr.VND_Printer_ID and pr.PRT_PrinterRateType_ID = 3 and pr.[Default] = 1

update #tempComponents 
	set
		CornerGuardCost = TotalEstimateWeight / cast(2000 as decimal) * CornerGuardRate,
		SkidCost = TotalEstimateWeight / cast(2000 as decimal) * SkidRate,
		CartonCost = NumberOfCartons * CartonRate,
		TimeValueSlipsCost = TimeValueSlipsCPM * SoloQuantity / cast(1000 as decimal) + isnull(dbo.ComponentPolybagMailHouseTimeValueSlipsCost(EST_Component_ID), 0),
		MailHouseAdminFee =
			case
				when SoloQuantity = 0 then 0
				else GrossMailHouseAdminFee * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
			end
			+ isnull(dbo.ComponentPolybagMailHouseAdminFee(EST_Component_ID), 0),
		GlueTackCost = GlueTackCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseGlueTackCost(EST_Component_ID), 0),
		TabbingCost = TabbingCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseTabbingCost(EST_Component_ID), 0),
		LetterInsertionCost = LetterInsertionCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseLetterInsertionCost(EST_Component_ID), 0),
		OtherMailHandlingCost = OtherMailHandlingCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseOtherDirectMailHandlingCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponents
	set
		InsertHandlingTotal = isnull(CornerGuardCost, 0) + isnull(SkidCost, 0) + isnull(CartonCost, 0),
		MailHandlingTotal = isnull(TimeValueSlipsCost, 0) + MailHouseAdminFee + isnull(GlueTackCost, 0) + isnull(TabbingCost, 0) + isnull(LetterInsertionCost, 0) + isnull(OtherMailHandlingCost, 0)

update #tempComponents set HandlingTotal = InsertHandlingTotal + MailHandlingTotal

update #tempComponents set AssemblyTotal = PolyBagTotal + isnull(StitchInCost, 0) + isnull(BlowInCost, 0) + TotalInkJetCost + HandlingTotal

/* Distribution Calculations */
create table #tempPackagesByPubRate(
	EST_Component_ID bigint not null,
	EST_Package_ID bigint not null,
	PubRate_Map_ID bigint not null,
	PUB_Pub_ID char(3)not null,
	PUB_PubLoc_ID smallint not null,
	PUB_PubRate_ID bigint  null,
	ChargeBlowIn bit,
	PUB_RateType_ID int not null,
	PUB_PubQuantity_ID bigint not null,
	QuantityType int not null,
	InsertDate datetime not null,
	IssueDOW int not null,
	ComponentTabPageCount decimal null,
	PackageTabPageCount int null,
	ComponentPieceWeight decimal(12,6) not null,
	PackageWeight decimal(12,6) not null,
	BlowInRate int null,
	PackageSize int not null,
	BilledQuantity int,
	GrossPackageInsertCost money null,
	InsertDiscountPercent decimal(10,4),
	PackageComponentCost money null)

insert into #tempPackagesByPubRate(EST_Component_ID, EST_Package_ID, PubRate_Map_ID, PUB_Pub_ID, PUB_PubLoc_ID,
	PUB_PubRate_ID, BlowInRate, PUB_RateType_ID,
	PUB_PubQuantity_ID, QuantityType, InsertDate, IssueDOW, ComponentPieceWeight, PackageWeight, 
	PackageSize, BilledQuantity, GrossPackageInsertCost)
select pcm.EST_Component_ID, p.EST_Package_ID, pprm.PUB_PubRate_Map_ID, pprm.pub_id, pprm.publoc_id,
	pr.PUB_PubRate_ID, pr.BlowInRate, pr.PUB_RateType_ID,
	dbo.CalcPubQuantityID(pprm.PUB_PubRate_Map_ID, pid.IssueDate), p.PUB_PubQuantityType_ID, pid.IssueDate,
	case p.PUB_PubQuantityType_ID
		when 4 then 1 /* Thanksgiving always uses Sunday rates */
		when 5 then 1 /* Christmas always uses Sunday rates */
		when 6 then 1 /* New Years always uses Sunday rates */
		else pid.IssueDOW
	end IssueDOW,
	tc.PieceWeight, dbo.PackageWeight(p.EST_Package_ID), 
	dbo.PackageSize(p.EST_Package_ID), 0, 0
from #tempComponents tc join EST_PackageComponentMapping pcm on tc.EST_Component_ID = pcm.EST_Component_ID
	join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	join PUB_PubGroup pg on p.PUB_PubGroup_ID = pg.PUB_PubGroup_ID
	join PUB_PubPubGroup_Map ppgm on pg.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID
	join PUB_PubRate_Map pprm on ppgm.PUB_PubRate_Map_ID = pprm.PUB_PubRate_Map_ID
	join PUB_PubRate pr on pprm.PUB_PubRate_Map_ID = pr.PUB_PubRate_Map_ID
	join EST_PubIssueDates pid on p.EST_Estimate_ID = pid.EST_Estimate_ID and pprm.PUB_PubRate_Map_ID = pid.PUB_PubRate_Map_ID
where dbo.IsPubRateMapActive(pprm.PUB_PubRate_Map_ID, pid.IssueDate) = 1
	and dbo.CalcPubQuantityID(pprm.PUB_PubRate_Map_ID, pid.IssueDate) is not null
	and dbo.CalcPubRateID(pprm.PUB_PubRate_Map_ID, pid.IssueDate) = pr.PUB_PubRate_ID

update tp
	set ComponentTabPageCount =
		case
			when tc.EST_EstimateMediaType_ID = 2 then 2 -- Broadsheet
			else 1
		end
		*
		case
			when tc.EST_ComponentType_ID = 4 and tp.BlowInRate = 0 then 0 --Blow-In not charged
			when tc.EST_ComponentType_ID = 4 and tp.BlowInRate = 1 then cast(tc.PageCount as decimal) / 2 -- Blow-In charged at 1/2 page
			else tc.PageCount
		end
from #tempComponents tc join #tempPackagesByPubRate tp on tc.EST_Component_ID = tp.EST_Component_ID

update #tempPackagesByPubRate
	set PackageTabPageCount = dbo.PackageTabPageCount(EST_Package_ID, BlowInRate)

update #tempPackagesByPubRate
	set BilledQuantity =
		case pr.QuantityChargeType
			when 1 then q.Quantity - (q.Quantity * pr.BilledPct)
			else q.Quantity
		end
from #tempPackagesByPubRate t join PUB_DayOfWeekQuantity q on t.PUB_PubQuantity_ID = q.PUB_PubQuantity_ID and t.QuantityType = q.PUB_PubQuantityType_ID
		and (t.QuantityType > 3 /*Holidays*/ or datepart(dw, t.InsertDate) = q.InsertDow /*Full Run / Contract Send*/) 
	join PUB_PubRate pr on t.PUB_PubRate_ID = pr.PUB_PubRate_ID

update #tempPackagesByPubRate
	set GrossPackageInsertCost = dbo.CalcGrossInsertCostforPackageandPub(PUB_PubRate_ID, InsertDate, IssueDOW, PackageTabPageCount, BilledQuantity, PackageWeight, PackageSize)

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
	set InsertDiscountPercent = d.Discount
from #tempPackagesByPubRate t join #tempPackageInsertNumber pin on t.EST_Package_ID = pin.EST_Package_ID and t.PubRate_Map_ID = pin.PUB_PubRate_Map_ID
	join PUB_InsertDiscounts d on t.PUB_PubRate_ID = d.PUB_PubRate_ID
where pin.InsertNumber = d.[Insert]

drop table #tempPackageInsertNumber

/*----------------------------------------- */

--Tab Page Count Rate Type
update tp
	set PackageComponentCost =
		case
			when PackageWeight = 0 then 0
			else
				(GrossPackageInsertCost *(1 - isnull(InsertDiscountPercent, 0))) * ComponentTabPageCount / PackageTabPageCount
		end
from #tempPackagesByPubRate tp
where tp.PUB_RateType_ID = 1

--Insertion Costs with other Rate Types are prorated to Components by component piece weight
update tp
	set PackageComponentCost =
		case
			when PackageWeight = 0 then 0
			else
				(GrossPackageInsertCost *(1 - isnull(InsertDiscountPercent, 0))) * ComponentPieceWeight / PackageWeight
		end
from #tempPackagesByPubRate tp
where tp.PUB_RateType_ID <> 1

update tc
	set InsertCost = isnull((select sum(PackageComponentCost) from #tempPackagesByPubRate tp where tp.EST_Component_ID = tc.EST_Component_ID), 0)
from #tempComponents tc

update #tempComponents
	set InsertFreightCost = dbo.TotalEstimateWeight(#tempComponents.EST_Estimate_ID) / cast(100 as decimal) * InsertFreightCWT,
		InsertFuelSurchargeCost = (dbo.TotalEstimateWeight(#tempComponents.EST_Estimate_ID) / cast(100 as decimal) * InsertFreightCWT) * InsertFuelSurchargePercent
where #tempComponents.EST_ComponentType_ID = 1


update #tempComponents set InsertTotal = InsertCost + isnull(InsertFreightCost, 0) + isnull(InsertFuelSurchargeCost, 0)

update #tempComponents set PostalDropCost = dbo.EstimatePostalDropCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1

update #tempComponents set PostalDropFuelSurchargeCost = dbo.EstimatePostalDropFuelSurchargeCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1

update #tempComponents
	set PostalDropTotalCost = isnull(PostalDropCost, 0) + isnull(PostalDropFuelSurchargeCost, 0)

update #tempComponents
	set MailTrackingCost = isnull((SoloQuantity / cast(1000 as decimal) * MailTrackingCPMRate), 0) + isnull(dbo.ComponentPolybagMailTrackingCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponents
	set SoloPostageCost = isnull(dbo.ComponentSoloPostage(EST_Component_ID, PieceWeight, PST_PostalScenario_ID), 0)

update #tempComponents
	set PolyPostageCost = isnull(dbo.ComponentPolyPostage(EST_Component_ID, PieceWeight), 0)

update #tempComponents
	set TotalPostageCost = SoloPostageCost + PolyPostageCost

update #tempComponents
	set PostalTotal = PostalDropTotalCost + isnull(PostalDropFuelSurchargeCost, 0) + isnull(MailTrackingCost, 0) + TotalPostageCost

update t
	set SampleFreight =
		case
			when isnull(s.FreightCWT, 0) <> 0 then dbo.EstimatePieceWeight(t.EST_Estimate_ID) * SampleQuantity / cast(100 as decimal) * s.FreightCWT
			else s.FreightFlat
		end
from #tempComponents t join EST_Samples s on t.EST_Estimate_ID = s.EST_Estimate_ID
where t.EST_ComponentType_ID = 1

update #tempComponents
	set OtherFreight = GrossOtherFreight
/* Not sure if this cost should be distributed across Polybags */
/*
			case
				when isnull(SoloQuantity, 0) = 0 then 0
				else GrossOtherFreight * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
			end
			+ isnull(dbo.ComponentPolybagOtherFreightCost(EST_Component_ID), 0)
*/
where EST_ComponentType_ID = 1

update #tempComponents set DistributionTotal = InsertTotal + isnull(PostalTotal, 0) + isnull(SampleFreight, 0) + isnull(OtherFreight, 0)
update #tempComponents set GrandTotal = ProductionTotal + AssemblyTotal + DistributionTotal

/* Costs have been computed now roll them up by Ad # and upload them*/
print 'Upload ad_est data'

--Update records for every ad number
update ae
	set ae.est_upld_file_nm = tc.EST_Estimate_ID,
		ae.estd_ad_desc = left(tc.Description, 30),
		ae.estd_media_qty = tc.MediaQuantity,
		ae.estd_base_pg_qty = tc.PageCount,
		ae.estd_trim_wdth_qty = tc.Width,
		ae.estd_trim_hght_qty = tc.Height,
		ae.estd_papr_wgt_desc = tc.PaperWeight,
		ae.estd_papr_stk_desc = left(tc.PaperGradeDescription, 15)
from #tempComponents tc join DBADVProd.informix.ad_est ae on tc.AdNumber = ae.ad_nbr

--Update records with host component info
update ae
	set ae.est_upld_file_nm = tc.EST_Estimate_ID,
		ae.estd_ad_desc = left(tc.Description, 30),
		ae.estd_media_qty = tc.MediaQuantity,
		ae.estd_base_pg_qty = tc.PageCount,
		ae.estd_trim_wdth_qty = tc.Width,
		ae.estd_trim_hght_qty = tc.Height,
		ae.estd_papr_wgt_desc = tc.PaperWeight,
		ae.estd_papr_stk_desc = left(tc.PaperGradeDescription, 15)
from #tempComponents tc join DBADVProd.informix.ad_est ae on tc.AdNumber = ae.ad_nbr
where tc.EST_ComponentType_ID = 1

print 'Upload ad_cost_est data'
print 'Delete Cost Data from Admin System'
delete ce
from DBADVProd.informix.ad_cost_est ce join #tempComponents tc on ce.ad_nbr = tc.AdNumber

print 'CostCode 530 (Creative)'
insert into DBADVProd.informix.ad_cost_est (ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '530' CostCode, sum(isnull(CreativeCost, 0)) CreativeCost
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'CostCode 595 (Separator)'
insert into DBADVProd.informix.ad_cost_est (ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '595' CostCode, sum(isnull(SeparatorCost, 0)) SeparatorCost
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'CostCode 610 (Net Print Cost)'
insert into DBADVProd.informix.ad_cost_est (ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '610' CostCode, sum(isnull(NetPrintCost, 0)) NetPrintCost
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'CostCode 605 (Net Paper Cost)'
insert into DBADVProd.informix.ad_cost_est (ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '605' CostCode, sum(isnull(NetPaperCost, 0)) NetPaperCost
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'Upload CostCode 606 (Paper Handling)'
insert into DBADVProd.informix.ad_cost_est (ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '606' CostCode, sum(isnull(PaperHandlingCost, 0)) PaperHandlingCost
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'Upload CostCode 615 (Print Sales Tax)'
insert into DBADVProd.informix.ad_cost_est(ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '615' CostCode, sum(isnull(PrinterSalesTaxAmount, 0) + isnull(PaperSalesTaxAmount, 0)) SalesTaxAmount
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'CostCode 760 (Mail List)'
insert into DBADVProd.informix.ad_cost_est (ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '760' CostCode, sum(isnull(MailListCost, 0)) MailListCost
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'CostCode 880 (Specialty Other)'
insert into DBADVProd.informix.ad_cost_est (ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '880' CostCode, sum(isnull(OtherProductionCost, 0)) OtherProductionCost
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'CostCode 870 (Vendor Production)'
insert into DBADVProd.informix.ad_cost_est (ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '870' CostCode, sum(isnull(VendorProductionCost, 0)) VendorProductionCost
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'CostCode 730 (Polybag Cost)'
insert into DBADVProd.informix.ad_cost_est (ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '730' CostCode, sum(isnull(PolyBagTotal, 0)) PolybagTotal
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'CostCode 720 (Stitch-In / Blow-In Cost)'
insert into DBADVProd.informix.ad_cost_est (ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '720' CostCode, sum(isnull(StitchInCost, 0) + isnull(BlowInCost, 0))
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'Upload CostCode 745 (Inkjet)'
insert into DBADVProd.informix.ad_cost_est (ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '745' CostCode, sum(isnull(TotalInkJetCost, 0))
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'Upload CostCode 740 (Handling)'
insert into DBADVProd.informix.ad_cost_est (ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '740' CostCode, sum(isnull(HandlingTotal, 0)) HandlingTotal
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'CostCode 820 (Postal Drop)'
insert into DBADVProd.informix.ad_cost_est (ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '820' CostCode, sum(isnull(PostalDropTotalCost, 0)) PostalDropTotalCost
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'CostCode 830 (Newspaper Freight)'
insert into DBADVProd.informix.ad_cost_est (ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '830' CostCode, sum(isnull(InsertFreightCost, 0) + isnull(InsertFuelSurchargeCost, 0))
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'CostCode 810 (Sample Shipping)'
insert into DBADVProd.informix.ad_cost_est (ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '810' CostCode, sum(isnull(SampleFreight, 0)) SampleFreight
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'CostCode 855 (Other Distribution)'
insert into DBADVProd.informix.ad_cost_est (ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '855' CostCode, sum(isnull(OtherFreight, 0)) OtherFreight
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'CostCode 750 (Mail Tracking)'
insert into DBADVProd.informix.ad_cost_est (ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '750' CostCode, sum(isnull(MailTrackingCost, 0)) MailTrackingCost
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'CostCode 840 (Postage)'
insert into DBADVProd.informix.ad_cost_est (ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '840' CostCode, sum(isnull(TotalPostageCost, 0)) TotalPostageCost
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'CostCode 850 (Insertion)'
insert into DBADVProd.informix.ad_cost_est (ad_nbr, cost_cd, est_ad_cost_amt)
select AdNumber, '850' CostCode, sum(isnull(InsertCost, 0)) InsertCost
from #tempComponents
where AdNumber is not null
group by AdNumber

print 'Delete existing pub_cost_est data'
delete pe
from DBADVProd.informix.pub_cost_est pe join #tempComponents tc on pe.ad_nbr = tc.AdNumber

print 'Upload pub_cost_est data'
insert into DBADVProd.informix.pub_cost_est(ad_nbr, pub_id, publoc_id, est_pub_cost_amt)
select tc.AdNumber, tp.PUB_Pub_ID, tp.PUB_PubLoc_ID, max(isnull(tp.GrossPackageInsertCost, 0))
from #tempComponents tc join #tempPackagesByPubRate tp on tc.EST_Component_ID = tp.EST_Component_ID
where tc.EST_ComponentType_ID = 1
group by tc.AdNumber, tp.PUB_Pub_ID, tp.PUB_PubLoc_ID

print 'Upload ctlg_pubvr_distbn'
-- Clear out unused records.
DELETE	cp
FROM	DBADVProd.informix.ctlg_pubvr_distbn cp
	INNER JOIN
			(SELECT DISTINCT tc.AdNumber
			FROM	#tempComponents tc INNER JOIN #tempPackagesByPubRate tp ON tc.EST_Component_ID = tp.EST_Component_ID) ads
		ON ads.AdNumber = cp.ad_nbr
	LEFT JOIN
			(SELECT DISTINCT tc.AdNumber, tp.PUB_Pub_ID, tp.PUB_PubLoc_ID
			FROM	#tempComponents tc INNER JOIN #tempPackagesByPubRate tp ON tc.EST_Component_ID = tp.EST_Component_ID) locs
		ON locs.AdNumber = cp.ad_nbr
		AND locs.PUB_Pub_ID = cp.pub_id
		AND locs.PUB_PubLoc_ID = cp.publoc_id
WHERE
	locs.PUB_PubLoc_ID is null

-- Insert new pub-locs
INSERT INTO DBADVProd.informix.ctlg_pubvr_distbn
	(ad_nbr
	, pub_id
	, publoc_id
	, vrsn_pub_issue_dt
	, media_qty)
SELECT 
	tc.AdNumber
	, tp.PUB_Pub_ID
	, tp.PUB_PubLoc_ID
	, tp.InsertDate
	, isnull(tp.BilledQuantity, 0)
FROM	#tempComponents tc 
	INNER JOIN #tempPackagesByPubRate tp 
		ON tc.EST_Component_ID = tp.EST_Component_ID
	LEFT JOIN DBADVProd.informix.ctlg_pubvr_distbn cp 
		ON tc.AdNumber = cp.ad_nbr 
		AND tp.PUB_Pub_ID = cp.pub_id 
		AND tp.PUB_PubLoc_ID = cp.publoc_id
WHERE
	cp.publoc_id is null

-- It is possible to have a package that does not contain a host component, update all matching pub-locs
update cp
	set cp.vrsn_pub_issue_dt = tp.InsertDate,
		cp.media_qty = tp.BilledQuantity
from #tempComponents tc join #tempPackagesByPubRate tp on tc.EST_Component_ID = tp.EST_Component_ID
	join DBADVProd.informix.ctlg_pubvr_distbn cp on tc.AdNumber = cp.ad_nbr and tp.PUB_Pub_ID = cp.pub_id and tp.PUB_PubLoc_ID = cp.publoc_id

-- I believe the host info is preferable, update the pub-locs with the package containing the host component when possible
update cp
	set cp.vrsn_pub_issue_dt = tp.InsertDate,
		cp.media_qty = isnull(tp.BilledQuantity, 0)
from #tempComponents tc join #tempPackagesByPubRate tp on tc.EST_Component_ID = tp.EST_Component_ID
	join DBADVProd.informix.ctlg_pubvr_distbn cp on tc.AdNumber = cp.ad_nbr and tp.PUB_Pub_ID = cp.pub_id and tp.PUB_PubLoc_ID = cp.publoc_id
where tc.EST_ComponentType_ID = 1

drop table #tempEstimate
drop table #tempComponents
drop table #tempPackagesByPubRate
GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_Upload]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstEstimate_Upload_Search') IS NOT NULL
	BEGIN
		PRINT 'Dropping dbo.EstEstimate_Upload_Search'
		DROP PROCEDURE dbo.EstEstimate_Upload_Search
		-- Tell user if drop failed.
		IF OBJECT_ID('dbo.EstEstimate_Upload_Search') IS NOT NULL
			PRINT ' *** Drop of dbo.EstEstimate_Upload_Search FAILED. *** '
	END
	GO
	
PRINT 'Creating dbo.EstEstimate_Upload_Search'
GO

CREATE PROCEDURE [dbo].[EstEstimate_Upload_Search] 
@Est_Estimate_ID bigint
AS

/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
* Performs pre-upload validation of Estimate
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate
*   EST_Component
*   EST_AssemDistribOptions
*   PPR_PaperWeight
*   PPR_PaperGrade
*   VND_Printer
*   PRT_PrinterRate
*   PPR_Paper_Map
*   VND_MailHouse
*   VND_MailTracker
*
*
* PROCEDURES CALLED:
*   dbo.TotalEstimateWeight
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
* Date		Who		Comments
* ----------	---		-------------------------------------------------
* 08/25/2007	TJU		Initial Creation 
* 08/29/2007	BJS		Fixed join to PrinterRate table for plate cost
* 09/10/2007	BJS		Join to MailTracking table is a left join
* 09/18/2007	BJS		Renamed to EstEstimate_Upload_Search
*				Changed references to EST_PubInsertDates to EST_PubIssueDates
*				Replaced calls to Quantity functions with views
*				Removed costing calculations.  They are not needed for validation.
* 10/23/2007	JRH		Removed check for DBADVProd.informix.ctlg_pubvr_distbn.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/



Set nocount on

/* Comments correlate to layout of Report/Extract #4 - For Single Estimate Only */
create table #tempComponents(
	EST_Component_ID bigint,
	EST_Estimate_ID bigint,
	Parent_ID bigint,
	EST_ComponentType_ID int,
	EST_RunDate datetime,
	EST_Description varchar(35),
	ESTc_Description varchar(35),
	ESTc_MediaQtyWOInserts int,
	StatusCode int,				/* 100=Passed, 200= Warning, 300=Failed */
	Validation_msg varchar(150),

	/* <Main> */
	AdNumber int,

	/* <Quantity> */
	SoloQuantity int,
	PolybagQuantity int
)

/* Get Raw Production Data */
insert into #tempComponents
	(EST_Component_ID, EST_Estimate_ID, Parent_ID, EST_ComponentType_ID, EST_RunDate, EST_Description, 
	ESTc_Description, ESTc_MediaQtyWOInserts, AdNumber)

select 	c.EST_Component_ID, e.EST_Estimate_ID, e.Parent_ID, c.EST_ComponentType_ID, e.RunDate,
	e.Description, c.Description, 
	c.mediaqtywoinsert, c.AdNumber
from EST_Estimate e join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
where e.EST_Estimate_ID = @EST_Estimate_ID

/* Perform Quantity Calculations */
update tc
	set tc.SoloQuantity = isnull(sq.SoloQuantity, 0)
from #tempComponents tc left join vwComponentSoloQuantity sq on tc.EST_Component_ID = sq.EST_Component_ID

update tc
	set tc.PolybagQuantity = isnull(pq.PolybagQuantity, 0)
from #tempComponents tc left join vwComponentPolybagQuantity pq on tc.EST_Component_ID = pq.EST_Component_ID

/* set StatusCode = Validation passed */
update #tempComponents 
	set StatusCode = 100,
		Validation_msg = 'Valid'

/* check for warning conditions */
update 	#tempComponents 
	Set StatusCode = 200,
	Validation_msg = 'Missing Ad Number'
	where EST_ComponentType_ID <> 1 and	/* Component Type <> HOST */
	(AdNumber = 0 or AdNumber is NULL)	/* no adnumber */

update 	#tempComponents
Set 	StatusCode = 200,
	Validation_msg = 'Media Qty w/o Insert does not match'
where 	ESTc_MediaQtyWOInserts <> (SoloQuantity + PolybagQuantity) /* mediawoinsert <>  SoloQty + PolyQty*/


/* check for failed conditions */
update 	#tempComponents
Set 	StatusCode = 300,
	Validation_msg = 'Host Component Missing Ad Number'
where 	EST_ComponentType_ID = 1 and 		/* Component Type = HOST */
	(AdNumber = 0 or AdNumber is NULL)	/* no adnumber */

update tc
	set
		StatusCode = 300,
		Validation_msg = 'Admin System has no record for Ad Number ' + cast(tc.AdNumber as varchar)
from #tempComponents tc left join DBADVProd.informix.ad_est ae on tc.AdNumber = ae.ad_nbr
where tc.AdNumber is not null and ae.ad_nbr is null

create table #tempComponentPubLoc(
	EST_Component_ID bigint,
	Pub_ID char(3),
	PubLoc_ID int)

insert into #tempComponentPubLoc(EST_Component_ID, Pub_ID, PubLoc_ID)
select tc.EST_Component_ID, prm.Pub_ID, prm.PubLoc_ID
from #tempComponents tc join EST_PackageComponentMapping pcm on tc.EST_Component_ID = pcm.EST_Component_ID
	join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	join PUB_PubGroup pg on p.PUB_PubGroup_ID = pg.PUB_PubGroup_ID
	join PUB_PubPubGroup_Map ppgm on pg.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID
	join PUB_PubRate_Map prm on ppgm.PUB_PubRate_Map_ID = prm.PUB_PubRate_Map_ID
where tc.AdNumber is not null

select 
	EST_Estimate_ID,
	Parent_ID,
 	EST_RunDate, 
	EST_Description, 
	ESTc_Description, 
	AdNumber,
	StatusCode,
	Validation_msg
from #tempComponents
order by EST_Estimate_ID, EST_Component_ID

set nocount off

drop table #tempComponents
drop table #tempComponentPubLoc

GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_Upload_Search]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstEstimate_u_Status') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstEstimate_u_Status'
	DROP PROCEDURE dbo.EstEstimate_u_Status
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstEstimate_u_Status') IS NOT NULL
		PRINT '***********Drop of dbo.EstEstimate_u_Status FAILED.'
END
GO
PRINT 'Creating dbo.EstEstimate_u_Status'
GO

CREATE PROC dbo.EstEstimate_u_Status
/*
* PARAMETERS:
*	EST_Estimate_ID - The ID of the record to update.
*	EST_Status_ID   - The new status ID to update the estimate with
*	ModifiedBy		- The user modifying this record
*
* DESCRIPTION:
*		Updates an Estimate status in the EST_Estimate table.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate        UPDATE
*
* PROCEDURES CALLED:
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 08/15/2007      NLS             Initial Creation
* 08/31/2007      BJS             Added begin/rollback/commit tran.  Added polybag reference check. 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
	@EST_Estimate_ID bigint,
	@EST_Status_ID int,
	@ModifiedBy varchar(50)

AS

begin tran t

if (@EST_Status_ID = 3) begin
	--You can only kill an active estimate
	if exists(select 1 from EST_Estimate where EST_Estimate_ID = @EST_Estimate_ID and EST_Status_ID not in (1,3)) begin
		rollback tran t
		raiserror('Error updating Estimate Status', 16, 1)
		return
	end

	--An estimate cannot be a member of a polybag group if you want to kill it
	if exists(select 1 from EST_Estimate e join EST_EstimatePolybagGroup_Map pbgm on e.EST_Estimate_ID = pbgm.EST_Estimate_ID where e.EST_Estimate_ID = @EST_Estimate_ID) begin
		rollback tran t
		raiserror('Error updating Estimate Status', 16, 1)
		return
	end
end

UPDATE EST_Estimate 

SET EST_Status_ID = @EST_Status_ID,
	Modifiedby    = @ModifiedBy,
	Modifieddate  = getdate()

WHERE EST_Estimate_ID = @EST_Estimate_ID
if (@@error <> 0) begin
	rollback tran t
	raiserror('Error updating Estimate Status', 16, 1)
	return
end

commit tran t
GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_u_Status]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.EstPackageComponentMapping_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPackageComponentMapping_s_ByEstimateID'
	DROP PROCEDURE dbo.EstPackageComponentMapping_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPackageComponentMapping_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstPackageComponentMapping_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstPackageComponentMapping_s_ByEstimateID'
GO

create proc dbo.EstPackageComponentMapping_s_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns all of the package-component mappings for an estimate.  This procedure is used by the Distribution Mapping Screen.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Package
*   EST_PackageComponentMapping
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
* 05/25/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint
as
select pcm.EST_Package_ID, pcm.EST_Component_ID
from EST_Package p join EST_PackageComponentMapping pcm on p.EST_Package_ID = pcm.EST_Package_ID
where p.EST_Estimate_ID = @EST_Estimate_ID
order by pcm.EST_Package_ID, pcm.EST_Component_ID


GO

GRANT  EXECUTE  ON [dbo].[EstPackageComponentMapping_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstPackagePolybagMap_d') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPackagePolybagMap_d'
	DROP PROCEDURE dbo.EstPackagePolybagMap_d
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPackagePolybagMap_d') IS NOT NULL
		PRINT '***********Drop of dbo.EstPackagePolybagMap_d FAILED.'
END
GO
PRINT 'Creating dbo.EstPackagePolybagMap_d'
GO

CREATE PROCEDURE dbo.EstPackagePolybagMap_d
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Deletes a polybag package map record and updates the package solo mail quantity
*
* TABLES:
*   Table Name				Access
*   ==========				======
*   est_packagepolybag_map	Insert
*	est_package				Update
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 08/31/2007      NLS             Initial Creation 
*
*/
@est_package_id bigint,
@est_polybag_id bigint

AS

DECLARE @oldsolomail int
DECLARE @polybagqty int

DELETE FROM est_packagepolybag_map
	WHERE est_package_id = @est_package_id AND est_polybag_id = @est_polybag_id

SELECT @polybagqty =  quantity	   FROM est_polybag WHERE est_polybag_id = @est_polybag_id
SELECT @oldsolomail = soloquantity FROM est_package WHERE est_package_id = @est_package_id

UPDATE est_package
	SET soloquantity = ( @oldsolomail + @polybagqty )
	WHERE est_package_id = @est_package_id
GO

GRANT  EXECUTE  ON [dbo].[EstPackagePolybagMap_d]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.EstPackagePolybagMap_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPackagePolybagMap_i'
	DROP PROCEDURE dbo.EstPackagePolybagMap_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPackagePolybagMap_i') IS NOT NULL
		PRINT '***********Drop of dbo.EstPackagePolybagMap_i FAILED.'
END
GO
PRINT 'Creating dbo.EstPackagePolybagMap_i'
GO

CREATE PROCEDURE dbo.EstPackagePolybagMap_i
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Inserts a polybag package map record and updates the package solo mail quantity
*
* TABLES:
*   Table Name				Access
*   ==========				======
*   est_packagepolybag_map	Insert
*	est_package				Update
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 08/31/2007      NLS             Initial Creation 
* 10/11/2007      NLS             Fixed precision bug on decimal parameter for dist pct
*
*/
@est_package_id bigint,
@est_polybag_id bigint,
@distributionpct decimal(10,4),
@createdby varchar(50)

AS

DECLARE @oldsolomail int
DECLARE @polybagqty int

INSERT INTO est_packagepolybag_map
		( est_package_id,  est_polybag_id,  distributionpct,  createdby,  createddate )
VALUES	( @est_package_id, @est_polybag_id, @distributionpct, @createdby, getdate() )

SELECT @polybagqty =  quantity	   FROM est_polybag WHERE est_polybag_id = @est_polybag_id
SELECT @oldsolomail = soloquantity FROM est_package WHERE est_package_id = @est_package_id

DECLARE @newsolomail int
SET @newsolomail = @oldsolomail - @polybagqty
IF @newsolomail < 0 BEGIN SET @newsolomail = 0 END

UPDATE est_package SET 
	soloquantity = ( @newsolomail ),
	modifiedby = @createdby,
	modifieddate = getdate()
	WHERE est_package_id = @est_package_id

GO

GRANT  EXECUTE  ON [dbo].[EstPackagePolybagMap_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.EstPackage_QuantitiesWithPBFlag_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPackage_QuantitiesWithPBFlag_ByEstimateID'
	DROP PROCEDURE dbo.EstPackage_QuantitiesWithPBFlag_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPackage_QuantitiesWithPBFlag_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstPackage_QuantitiesWithPBFlag_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstPackage_QuantitiesWithPBFlag_ByEstimateID'
GO

CREATE proc dbo.EstPackage_QuantitiesWithPBFlag_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns the Insert Qty and Polybag Qty for all packages in an Estimate
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Package
* 
* PROCEDURES CALLED:
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
* Date          Who         Comments
* ----------    ----        -------------------------------------------------
* 09/13/2007	NLS         Initial Creation 
* 10/08/2007	JRH         Re-Written to pick up real insertqty and sum of polybagqty.
*
**********************************************************************************************************
*/

@EST_Estimate_ID bigint

AS

SELECT 
	pkg.est_package_id
	, insertqty = coalesce(insertqty, 0)
	, polybagqty = coalesce(polybagqty, 0)
	, inPolyBag = coalesce(inPolyBag, 0)
FROM
	est_package pkg (nolock)
		LEFT JOIN (
				SELECT 
					p.est_package_id
					, insertqty = coalesce(sum(dbo.PubRateMapInsertQuantityByInsertDate(pid.issuedate, p.pub_pubquantitytype_id, gm.pub_pubrate_map_id)), 0)
				FROM
					est_package p (nolock)
						INNER JOIN dbo.pub_pubgroup grp (nolock)
							ON p.pub_pubgroup_id = grp.pub_pubgroup_id
						INNER JOIN dbo.pub_pubpubgroup_map gm (nolock)
							ON grp.pub_pubgroup_id = gm.pub_pubgroup_id
						INNER JOIN dbo.est_pubissuedates pid (nolock)
							ON gm.pub_pubrate_map_id = pid.pub_pubrate_map_id
							AND p.est_estimate_id = pid.est_estimate_id
				WHERE
					p.est_estimate_id = @EST_Estimate_ID
				GROUP BY
					p.est_package_id) iq
			ON pkg.est_package_id = iq.est_package_id
		LEFT JOIN (
				SELECT 
					p.est_package_id
					, sum(polybag.quantity) polybagqty
					, inPolyBag = CAST(1 AS bit)
				FROM
					est_package p (nolock)
						INNER JOIN est_packagepolybag_map pbmap (nolock)
							ON pbmap.est_package_id = p.est_package_id 
						INNER JOIN est_polybag polybag (nolock)
							ON pbmap.est_polybag_id = polybag.est_polybag_id
				WHERE
					p.est_estimate_id = @EST_Estimate_ID
				GROUP BY
					p.est_package_id) pq
			ON pkg.est_package_id = pq.est_package_id
	

WHERE
	pkg.est_estimate_id = @EST_Estimate_ID
	
GO

GRANT  EXECUTE  ON [dbo].[EstPackage_QuantitiesWithPBFlag_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.EstPackage_Quantities_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPackage_Quantities_ByEstimateID'
	DROP PROCEDURE dbo.EstPackage_Quantities_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPackage_Quantities_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstPackage_Quantities_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstPackage_Quantities_ByEstimateID'
GO

CREATE proc dbo.EstPackage_Quantities_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns the Insert Qty and Polybag Qty for all packages in an Estimate
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Package
* 
* PROCEDURES CALLED:
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
* Date          Who         Comments
* ----------    ----        -------------------------------------------------
* 09/13/2007	NLS         Initial Creation 
* 10/08/2007	JRH         Re-Written to pick up real insertqty and sum of polybagqty.
*
**********************************************************************************************************
*/

@EST_Estimate_ID bigint

AS

SELECT 
	pkg.est_package_id
	, insertqty = coalesce(insertqty, 0)
	, polybagqty = coalesce(polybagqty, 0)
FROM
	est_package pkg (nolock)
		LEFT JOIN (
				SELECT 
					p.est_package_id
					, insertqty = coalesce(sum(dbo.PubRateMapInsertQuantityByInsertDate(pid.issuedate, p.pub_pubquantitytype_id, gm.pub_pubrate_map_id)), 0)
				FROM
					est_package p (nolock)
						INNER JOIN dbo.pub_pubgroup grp (nolock)
							ON p.pub_pubgroup_id = grp.pub_pubgroup_id
						INNER JOIN dbo.pub_pubpubgroup_map gm (nolock)
							ON grp.pub_pubgroup_id = gm.pub_pubgroup_id
						INNER JOIN dbo.est_pubissuedates pid (nolock)
							ON gm.pub_pubrate_map_id = pid.pub_pubrate_map_id
							AND p.est_estimate_id = pid.est_estimate_id
				WHERE
					p.est_estimate_id = @EST_Estimate_ID
				GROUP BY
					p.est_package_id) iq
			ON pkg.est_package_id = iq.est_package_id
		LEFT JOIN (
				SELECT 
					p.est_package_id
					, sum(polybag.quantity) polybagqty
				FROM
					est_package p (nolock)
						INNER JOIN est_packagepolybag_map pbmap (nolock)
							ON pbmap.est_package_id = p.est_package_id 
						INNER JOIN est_polybag polybag (nolock)
							ON pbmap.est_polybag_id = polybag.est_polybag_id
				WHERE
					p.est_estimate_id = @EST_Estimate_ID
				GROUP BY
					p.est_package_id) pq
			ON pkg.est_package_id = pq.est_package_id
	

WHERE
	pkg.est_estimate_id = @EST_Estimate_ID
	
GO

GRANT  EXECUTE  ON [dbo].[EstPackage_Quantities_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.EstPackage_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPackage_s_ByEstimateID'
	DROP PROCEDURE dbo.EstPackage_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPackage_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstPackage_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstPackage_s_ByEstimateID'
GO

CREATE proc dbo.EstPackage_s_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns all of the packages for the estimate.  Used on the Distribution Mapping Screen.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Package         READ
*
*
* PROCEDURES CALLED:
*   dbo.PackageInsertQuantity
*   dbo.PackagePolybagQuantity
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
* 05/25/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint
as
select p.EST_Package_ID, p.Description, dbo.PackageInsertQuantity(p.EST_Package_ID),
	p.SoloQuantity, dbo.PackagePolybagQuantity(p.EST_Package_ID) PolyQuantity,
	p.SoloQuantity + dbo.PackagePolybagQuantity(p.EST_Package_ID) as TotalMailQuantity, p.OtherQuantity,
	dbo.PackageInsertQuantity(p.EST_Package_ID) + p.SoloQuantity + dbo.PackagePolybagQuantity(p.EST_Package_ID) + p.OtherQuantity as TotalQuantity
from EST_Package p
where p.EST_Estimate_ID = @EST_Estimate_ID


GO

GRANT  EXECUTE  ON [dbo].[EstPackage_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstPackage_s_ForEstimateCopy') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPackage_s_ForEstimateCopy'
	DROP PROCEDURE dbo.EstPackage_s_ForEstimateCopy
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPackage_s_ForEstimateCopy') IS NOT NULL
		PRINT '***********Drop of dbo.EstPackage_s_ForEstimateCopy FAILED.'
END
GO
PRINT 'Creating dbo.EstPackage_s_ForEstimateCopy'
GO

create proc dbo.EstPackage_s_ForEstimateCopy
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns an estimate's packages to be used during an Estimate Copy.  Moves any polybag quantities to solo quantity.
*
*
* TABLES:
*   Table Name              Access
*   ==========              ======
*   EST_Package             READ
*   EST_PackagePolybag_Map  READ
*   EST_Polybag             READ
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
* 09/04/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint
as

select p.EST_Package_ID, max(p.Description) Description, max(p.Comments) Comments, max(p.SoloQuantity) + isnull(sum(pb.Quantity), 0) SoloQuantity,
	max(p.OtherQuantity) OtherQuantity, max(p.PUB_PubQuantityType_ID) PUB_PubQuantityType_ID, max(p.PUB_PubGroup_ID) PUB_PubGroup_ID
from EST_Package p
	left join EST_PackagePolybag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
	left join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
where p.EST_Estimate_ID = @EST_Estimate_ID
group by p.EST_Package_ID
GO

GRANT  EXECUTE  ON [dbo].[EstPackage_s_ForEstimateCopy]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstPackage_s_InsertScenario') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPackage_s_InsertScenario'
	DROP PROCEDURE dbo.EstPackage_s_InsertScenario
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPackage_s_InsertScenario') IS NOT NULL
		PRINT '***********Drop of dbo.EstPackage_s_InsertScenario FAILED.'
END
GO
PRINT 'Creating dbo.EstPackage_s_InsertScenario'
GO

CREATE PROCEDURE dbo.EstPackage_s_InsertScenario
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Returns a list of insert scenarios that are actively being used by estimates.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   est_package		    Read
*   pub_insertscenario  Read
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/22/2007      NLS             Initial Creation 
*
*/
AS BEGIN

SELECT DISTINCT pub.pub_insertscenario_id, pub.description
    FROM pub_insertscenario pub LEFT JOIN est_package pkg ON pub.pub_insertscenario_id = pkg.pub_insertscenario_id
    WHERE pkg.pub_insertscenario_id is not null
ORDER BY pub.description ASC

END
GO

GRANT  EXECUTE  ON [dbo].[EstPackage_s_InsertScenario]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstPolybagGroup_ClearMaps_ByPolybagGroupId') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPolybagGroup_ClearMaps_ByPolybagGroupId'
	DROP PROCEDURE dbo.EstPolybagGroup_ClearMaps_ByPolybagGroupId
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPolybagGroup_ClearMaps_ByPolybagGroupId') IS NOT NULL
		PRINT '***********Drop of dbo.EstPolybagGroup_ClearMaps_ByPolybagGroupId FAILED.'
END
GO
PRINT 'Creating dbo.EstPolybagGroup_ClearMaps_ByPolybagGroupId'
GO

CREATE PROCEDURE dbo.EstPolybagGroup_ClearMaps_ByPolybagGroupId
/*
* PARAMETERS:
*	@est_polybaggroup_id - Polybag Group to clear map tables for
*
* DESCRIPTION:
*   Clears the est_estimatepolybaggroup_map and est_packagepolybag_map tables for the
*   given polybag group
*
* TABLES:
*   Table Name				        Access
*   ==========				        ======
*   est_estimatepolybaggroup_map    Delete
*   est_packagepolybag_Map  	    Delete
*   est_package                     Update
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/03/2007      NLS             Initial Creation 
*
*/
@est_polybaggroup_id bigint

AS

-- First update the solo quanities of all the packages I'm deleting a map to
UPDATE pkg
    SET pkg.soloquantity = isnull(pkg.soloquantity, 0) + pb.quantity
FROM est_package pkg
    JOIN est_packagepolybag_map map ON pkg.est_package_id = map.est_package_id
    JOIN est_polybag pb ON pb.est_polybag_id = map.est_polybag_id
WHERE
    pb.est_polybaggroup_id = @est_polybaggroup_id

-- Now clear both the map tables
DELETE map FROM est_packagepolybag_map map
    JOIN est_polybag pb ON map.est_polybag_id = pb.est_polybag_id
WHERE
    pb.est_polybaggroup_id = @est_polybaggroup_id

DELETE FROM est_estimatepolybaggroup_map
    WHERE est_polybaggroup_id = @est_polybaggroup_id

GO

GRANT  EXECUTE  ON [dbo].[EstPolybagGroup_ClearMaps_ByPolybagGroupId]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.EstPolybagGroup_d_ByPolybagID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPolybagGroup_d_ByPolybagID'
	DROP PROCEDURE dbo.EstPolybagGroup_d_ByPolybagID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPolybagGroup_d_ByPolybagID') IS NOT NULL
		PRINT '***********Drop of dbo.EstPolybagGroup_d_ByPolybagID FAILED.'
END
GO
PRINT 'Creating dbo.EstPolybagGroup_d_ByPolybagID'
GO

CREATE PROC dbo.EstPolybagGroup_d_ByPolybagID
/*
* PARAMETERS:
*   @est_polybaggroup_id - Required.  The polybag group to delete.
*
*
* DESCRIPTION:
*	Deletes a Polybag from the EST_Polybag table.
*
*
* TABLES:
*   Table Name						Access
*   ==========						======
*   EST_PolybagGroup				DELETE
*	EST_Polybag						DELETE
*	EST_PackagePolybag_Map			DELETE
*	EST_EstimatePolybagGroup_Map	DELETE
*	EST_Package						UPDATE
*
* PROCEDURES CALLED:
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 08/22/2007      BJS             Initial Creation 
* 10/12/2007	  NLS			  Fixed to delete the entire polybag group instead of just a polybag
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@est_polybaggroup_id bigint

AS

BEGIN TRAN t

-- Move the polybag quantity to any referenced packages' solo quantity
UPDATE pkg
    SET pkg.soloquantity = isnull(pkg.soloquantity, 0) + pb.quantity
FROM est_package pkg
    JOIN est_packagepolybag_map map ON pkg.est_package_id = map.est_package_id
    JOIN est_polybag pb ON pb.est_polybag_id = map.est_polybag_id
WHERE
    pb.est_polybaggroup_id = @est_polybaggroup_id

IF (@@error <> 0) BEGIN
	ROLLBACK TRAN t
	raiserror('Error updating Package record.', 16, 1)
END

-- Delete any references to the packages
DELETE map FROM est_packagepolybag_map map
	JOIN est_polybag p ON map.est_polybag_id = map.est_polybag_id
WHERE
	p.est_polybaggroup_id = @est_polybaggroup_id

IF (@@error <> 0) BEGIN
	ROLLBACK TRAN t
	raiserror('Error deleting Package Polybag Map record.', 16, 1)
END

-- Delete any references to the estimates
DELETE FROM est_estimatepolybaggroup_map
WHERE est_polybaggroup_id = @est_polybaggroup_id
IF (@@error <> 0) BEGIN
	ROLLBACK TRAN t
	raiserror('Error deleting Polybag record.', 16, 1)
END

-- Delete the polybags
DELETE FROM est_polybag
WHERE est_polybaggroup_id = @est_polybaggroup_id
IF (@@error <> 0) BEGIN
	ROLLBACK TRAN t
	raiserror('Error deleting Polybag record.', 16, 1)
END

-- Delete the polybag group
DELETE FROM est_polybaggroup
WHERE est_polybaggroup_id = @est_polybaggroup_id

COMMIT TRAN t

GO

GRANT  EXECUTE  ON [dbo].[EstPolybagGroup_d_ByPolybagID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstPolybag_Cleanup_ByPolybagGroupId') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPolybag_Cleanup_ByPolybagGroupId'
	DROP PROCEDURE dbo.EstPolybag_Cleanup_ByPolybagGroupId
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPolybag_Cleanup_ByPolybagGroupId') IS NOT NULL
		PRINT '***********Drop of dbo.EstPolybag_Cleanup_ByPolybagGroupId FAILED.'
END
GO
PRINT 'Creating dbo.EstPolybag_Cleanup_ByPolybagGroupId'
GO

CREATE PROCEDURE dbo.EstPolybag_Cleanup_ByPolybagGroupId
/*
* PARAMETERS:
*	@est_polybaggroup_id - Polybag Group to clean up polybags for
*
* DESCRIPTION:
*	Deletes any polybags for a particular Polybag Group that don't have packages associated with them
*
* TABLES:
*   Table Name				Access
*   ==========				======
*   est_polybag             Delete
*   est_packagepolybag_map	Read
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/03/2007      NLS             Initial Creation 
*
*/
@est_polybaggroup_id bigint

AS

DELETE pb FROM est_polybag pb
    LEFT JOIN est_packagepolybag_map map on pb.est_polybag_id = map.est_polybag_id
    WHERE map.est_polybag_id is null AND pb.est_polybaggroup_id = @est_polybaggroup_id

GO

GRANT  EXECUTE  ON [dbo].[EstPolybag_Cleanup_ByPolybagGroupId]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.EstPolybag_Search') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPolybag_Search'
	DROP PROCEDURE dbo.EstPolybag_Search
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPolybag_Search') IS NOT NULL
		PRINT '***********Drop of dbo.EstPolybag_Search FAILED.'
END
GO
PRINT 'Creating dbo.EstPolybag_Search'
GO

CREATE proc dbo.EstPolybag_Search
/*
* PARAMETERS:
* EST_Polybag_ID
* Description
* Comments
* EST_Season_ID
* FiscalYear
* FiscalMonth
* HostAdNumber
* StartRunDate
* EndRunDate
* CreatedBy
* EST_Status_ID

* DESCRIPTION:
*		Polybags are found based on search criteria for the Polybag Search screen.  At least one parameter is required.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
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
* 08/22/2007      BJS             Initial Creation 
* 09/27/2007      NLS             Changed group by to key around the PolybagGroup instead of Polybag
*                                 and fixed up associated search fields appropriately.
* 09/28/2007      NLS             Changed join to use EstimatePolybagGroupMap so that groups with no polybags searched correctly
* 10/02/2007      BJS             Fixed search on description
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Polybag_ID bigint,
@Description varchar(35),
@Comments varchar(255),
@EST_Season_ID int,
@FiscalYear int,
@FiscalMonth int,
@HostAdNumber int,
@StartRunDate datetime,
@EndRunDate datetime,
@CreatedBy varchar(50),
@EST_Status_ID int
as

select pbg.EST_PolybagGroup_ID EST_Polybag_ID, max(e.RunDate) RunDate, max(pbg.Description) Description, max(pbg.Comments) Comments, max(es.Description) Season,
	max(e.FiscalYear) FiscalYear, max(st.Description) EstimateStatus
from EST_PolybagGroup pbg left join EST_Polybag pb on pbg.EST_PolybagGroup_ID = pb.EST_PolybagGroup_ID
	left join EST_PackagePolybag_Map ppm on pb.EST_Polybag_ID = ppm.EST_Polybag_ID
	left join EST_Package p on ppm.EST_Package_ID = p.EST_Package_ID
	left join EST_PackageComponentMapping pcm on p.EST_Package_ID = pcm.EST_Package_ID
	left join EST_Component c on pcm.EST_Component_ID = c.EST_Component_ID
    left join EST_EstimatePolybagGroup_map epbgm on pbg.EST_PolybagGroup_ID = epbgm.EST_PolybagGroup_ID
	left join EST_Estimate e on epbgm.EST_Estimate_ID = e.EST_Estimate_ID
	left join EST_Season es on e.EST_Season_ID = es.EST_Season_ID
	left join EST_Status st on e.EST_Status_ID = st.EST_Status_ID
where (@EST_Polybag_ID is null or (@EST_Polybag_ID = pbg.EST_PolybagGroup_ID))
	and (@Description is null or (pbg.Description LIKE '%' + @Description + '%' ))
	and (@Comments is null or (pbg.Comments like '%' + @Comments + '%'))
	and (@EST_Season_ID is null or (@EST_Season_ID = e.EST_Season_ID))
	and (@FiscalYear is null or (@FiscalYear = e.FiscalYear))
	and (@FiscalMonth is null or (@FiscalMonth = e.FiscalMonth))
	and (@HostAdNumber is null or (c.EST_ComponentType_ID = 1 and @HostAdNumber = c.AdNumber))
	and (@StartRunDate is null or (@StartRunDate <= e.RunDate))
	and (@EndRunDate is null or (@EndRunDate >= e.RunDate))
	and (@CreatedBy is null or (@CreatedBy = pb.CreatedBy))
	and (@EST_Status_ID is null or (@EST_Status_ID = e.EST_Status_ID))
group by pbg.EST_PolybagGroup_ID
order by e.RunDate
GO

GRANT  EXECUTE  ON [dbo].[EstPolybag_Search]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstPolybag_u') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPolybag_u'
	DROP PROCEDURE dbo.EstPolybag_u
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPolybag_u') IS NOT NULL
		PRINT '***********Drop of dbo.EstPolybag_u FAILED.'
END
GO
PRINT 'Creating dbo.EstPolybag_u'
GO

CREATE PROCEDURE dbo.EstPolybag_u
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Updates a polybag record and updates the package solo mail quantity for all linked
*   packages in the polybag
*
* TABLES:
*   Table Name				Access
*   ==========				======
*   est_polybag				Update
*	est_package				Update
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 08/31/2007      NLS             Initial Creation 
* 09/27/2007      NLS             Fixed incorrect stored procedure
*
*/
@est_polybag_id bigint,
@est_polybaggroup_id bigint,
@pst_postalscenario_id bigint,
@quantity int,
@modifiedby varchar(50)

AS

DECLARE @oldquantity int
DECLARE @package_id bigint
DECLARE @oldsolomail int
DECLARE @newsolomail int

SELECT @oldquantity = quantity FROM est_polybag WHERE est_polybag_id = @est_polybag_id

UPDATE est_polybag SET 
	pst_postalscenario_id = @pst_postalscenario_id,
	quantity = @quantity,
	modifiedby = @modifiedby,
	modifieddate = getdate()
WHERE
	est_polybag_id = @est_polybag_id AND
	est_polybaggroup_id = @est_polybaggroup_id
    

DECLARE PackageCursor CURSOR LOCAL FORWARD_ONLY FOR
    SELECT est_package_id FROM est_packagepolybag_map WHERE est_polybag_id = @est_polybag_id

OPEN PackageCursor
FETCH NEXT FROM PackageCursor INTO @package_id

WHILE @@FETCH_STATUS = 0
BEGIN

	SELECT @oldsolomail = soloquantity FROM est_pakckage WHERE est_package_id = @package_id
	
	SET @newsolomail = @oldsolomail + @oldquantity - @quantity
	IF @newsolomail < 0 BEGIN SET @newsolomail = 0 END

	UPDATE est_package SET 
		soloquantity = ( @newsolomail ),
		modifiedby = @modifiedby,
		modifieddate = getdate()
		WHERE est_package_id = @package_id

	FETCH NEXT FROM PackageCursor INTO @package_id

END

CLOSE PackageCursor
DEALLOCATE PackageCursor

GO

GRANT  EXECUTE  ON [dbo].[EstPolybag_u]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.EstPubIssueDates_i_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPubIssueDates_i_ByEstimateID'
	DROP PROCEDURE dbo.EstPubIssueDates_i_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPubIssueDates_i_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstPubIssueDates_i_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstPubIssueDates_i_ByEstimateID'
GO

create proc dbo.EstPubIssueDates_i_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID
* CreatedBy
*
* DESCRIPTION:
*		Inserts an Est_PubIssueDate record for each PubRate Map referenced by the estimate.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Component       READ
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
* 09/12/2007      BJS             Initial Creation 
* 10/25/2007      BJS             Added logic to determine issue date offset
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint,
@CreatedBy varchar(50)
as

insert into EST_PubIssueDates(EST_Estimate_ID, PUB_PubRate_Map_ID, [Override], IssueDOW, IssueDate, CreatedBy, CreatedDate)
SELECT
	EST_Estimate_ID,
	PUB_PubRate_Map_ID,
	0,
	datepart(w, dbo.CalcIssueDate(InsertDate, InsertTime, AM_edition, AM_offset, PM_edition, PM_offset)),
	dbo.CalcIssueDate(InsertDate, InsertTime, AM_edition, AM_offset, PM_edition, PM_offset),
	@CreatedBy,
	getdate()
FROM
	(SELECT e.EST_Estimate_ID
		, rm.PUB_PubRate_Map_ID
		, InsertDate =
			case
				when datepart(dw, e.RunDate) > ad.InsertDOW then dateadd(d, -1 * (datepart(dw, e.RunDate) - ad.InsertDOW), e.RunDate)
				when datepart(dw, e.RunDate) < ad.InsertDOW then dateadd(d, ad.InsertDOW - datepart(dw, e.RunDate) - 7, e.RunDate)
				else e.RunDate
			end
		, ad.InsertTime
		, pl.pub_id
		, p.pub_nm
		, pl.publoc_id
		, pl.publoc_nm
		, AM_edition = 
			CASE ad.InsertDOW
				WHEN 1 THEN sun_edtn_cd
				WHEN 2 THEN mon_edtn_cd
				WHEN 3 THEN tue_edtn_cd
				WHEN 4 THEN wed_edtn_cd
				WHEN 5 THEN thu_edtn_cd
				WHEN 6 THEN fri_edtn_cd
				WHEN 7 THEN sat_edtn_cd
			END
		, AM_offset = 
			CASE ad.InsertDOW
				WHEN 1 THEN no_sun_edtn_nbr
				WHEN 2 THEN no_mon_edtn_nbr
				WHEN 3 THEN no_tue_edtn_nbr
				WHEN 4 THEN no_wed_edtn_nbr
				WHEN 5 THEN no_thu_edtn_nbr
				WHEN 6 THEN no_fri_edtn_nbr
				WHEN 7 THEN no_sat_edtn_nbr
			END
		, PM_edition = 
			CASE (ad.InsertDOW - 1) % 7
				WHEN 1 THEN sun_edtn_cd
				WHEN 2 THEN mon_edtn_cd
				WHEN 3 THEN tue_edtn_cd
				WHEN 4 THEN wed_edtn_cd
				WHEN 5 THEN thu_edtn_cd
				WHEN 6 THEN fri_edtn_cd
				WHEN 0 THEN sat_edtn_cd -- 7 modulus 7 is a zero
			END
		, PM_offset = 
			CASE (ad.InsertDOW - 1) % 7
				WHEN 1 THEN no_sun_edtn_nbr
				WHEN 2 THEN no_mon_edtn_nbr
				WHEN 3 THEN no_tue_edtn_nbr
				WHEN 4 THEN no_wed_edtn_nbr
				WHEN 5 THEN no_thu_edtn_nbr
				WHEN 6 THEN no_fri_edtn_nbr
				WHEN 0 THEN no_sat_edtn_nbr -- 7 modulus 7 is a zero
			END
	FROM
		DBADVProd.informix.pub p (nolock)
		INNER JOIN DBADVProd.informix.pub_loc pl (nolock)
			ON p.pub_id = pl.pub_id
		INNER JOIN dbo.pub_pubrate_map rm (nolock)
			ON pl.pub_id = rm.pub_id
			AND pl.publoc_id = rm.publoc_id
		JOIN PUB_PubPubGroup_Map ppgm on rm.PUB_PubRate_Map_ID = ppgm.PUB_PubRate_Map_ID
		JOIN EST_Package pkg on ppgm.PUB_PubGroup_ID = pkg.PUB_PubGroup_ID
		JOIN EST_Estimate e on pkg.EST_Estimate_ID = e.EST_Estimate_ID
		JOIN EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
		LEFT JOIN EST_PubIssueDates pid on pid.EST_Estimate_ID = @EST_Estimate_ID and ppgm.PUB_PubRate_Map_ID = pid.PUB_PubRate_Map_ID
	WHERE e.EST_Estimate_ID = @EST_Estimate_ID and pid.EST_Estimate_ID is null) a
GO

GRANT  EXECUTE  ON [dbo].[EstPubIssueDates_i_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstPubIssueDates_i_Override') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPubIssueDates_i_Override'
	DROP PROCEDURE dbo.EstPubIssueDates_i_Override
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPubIssueDates_i_Override') IS NOT NULL
		PRINT '***********Drop of dbo.EstPubIssueDates_i_Override FAILED.'
END
GO
PRINT 'Creating dbo.EstPubIssueDates_i_Override'
GO

create proc dbo.EstPubIssueDates_i_Override
/*
* PARAMETERS:
* Override
* IssueDOW
* IssueDate
* EST_Estimate_ID
* Pub_ID
* PubLoc_ID
* CreatedBy
*
* DESCRIPTION:
*		Identifies the PubRate Map record for the Pub and Pub-Loc and inserts an Est_PubIssueDate override record
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Component       READ
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
* 09/17/2007      BJS             Initial Creation 
* 11/30/2008      JRH             Change to @Pub_ID from Pub_ID.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Override bit,
@IssueDOW int,
@IssueDate datetime,
@EST_Estimate_ID bigint,
@Pub_ID char(3),
@PubLoc_ID int,
@CreatedBy varchar(50)
as

declare @PUB_PubRate_Map_ID bigint
select @PUB_PubRate_Map_ID = PUB_PubRate_Map_ID
from PUB_PubRate_Map
where Pub_ID = @Pub_ID and PubLoc_ID = @PubLoc_ID

insert into EST_PubIssueDates(Override, IssueDOW, IssueDate, EST_Estimate_ID, PUB_PubRate_Map_ID, CreatedBy, CreatedDate)
values(@Override, @IssueDOW, @IssueDate, @EST_Estimate_ID, @PUB_PubRate_Map_ID, @CreatedBy, getdate())
GO

GRANT  EXECUTE  ON [dbo].[EstPubIssueDates_i_Override]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstPubIssueDates_s_Overrides_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPubIssueDates_s_Overrides_ByEstimateID'
	DROP PROCEDURE dbo.EstPubIssueDates_s_Overrides_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPubIssueDates_s_Overrides_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstPubIssueDates_s_Overrides_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstPubIssueDates_s_Overrides_ByEstimateID'
GO

create proc dbo.EstPubIssueDates_s_Overrides_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns all EST_PubIssueDate overide records and corresponding pub_id and publoc_id's matching the EST_Estimate_ID
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PubIssueDates   READ
*   PUB_PubRate_Map     READ
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
* 09/11/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint
as
select pid.*, prm.Pub_ID, prm.PubLoc_ID
from EST_PubIssueDates pid join PUB_PubRate_Map prm on pid.PUB_PubRate_Map_ID = prm.PUB_PubRate_Map_ID
where pid.EST_Estimate_ID = @EST_Estimate_ID and pid.Override = 1

GO

GRANT  EXECUTE  ON [dbo].[EstPubIssueDates_s_Overrides_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstSamples_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstSamples_s_ByEstimateID'
	DROP PROCEDURE dbo.EstSamples_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstSamples_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstSamples_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstSamples_s_ByEstimateID'
GO

CREATE proc dbo.EstSamples_s_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns all of the samples for the estimate.  Used on the Samples Screen
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Samples
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
* 05/25/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint
as
select * from EST_Samples
where EST_Estimate_ID = @EST_Estimate_ID


GO

GRANT  EXECUTE  ON [dbo].[EstSamples_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.InsertSetupInfo_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.InsertSetupInfo_s_ByEstimateID'
	DROP PROCEDURE dbo.InsertSetupInfo_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.InsertSetupInfo_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.InsertSetupInfo_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.InsertSetupInfo_s_ByEstimateID'
GO

CREATE PROC dbo.InsertSetupInfo_s_ByEstimateID
/*
* PARAMETERS:
* Estimate ID - Required
*
* DESCRIPTION:
*		Returns information used on the Distribution Mapping -> Insert Setup tab. 
*
* TABLES:
*		Table Name					Access
*		==========					======
*		est_package					READ
*		pub_pubgroup					READ
*		pub_pubpubgroup_map				READ
*		vw_PubRateMap_withPubAndPublocNames		READ
*		est_pubissuedates				READ
*		pub_pubquantity					READ
*		pub_dayofweekquantity				READ
*
* PROCEDURES CALLED:
*		Procedure Name
*		==============
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
*	Date		Who		Comments
*	------------- 	--------        -------------------------------------------------
*	09/11/2007	JRH		Initial Creation 
*	09/19/2007	JRH		Removed pid.est_pubissuedates
*					Added display and displaySortOrder.
*	10/08/2007	JRH		Get quantity using function.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID	bigint

AS

SELECT
	p.est_package_id
	, p.description AS PackageName
	, p.pub_pubquantitytype_id
	, pqt.description AS pub_pubquantitytype
	, p.pub_pubgroup_id
	, grp.description AS GroupName
	, grp.active AS GroupActiveFlag
	, grp.effectivedate AS GroupEffectiveDate
	, grp.sortorder
	, grp.customgroupforpackage
	, gm.pub_pubrate_map_id
	, vNames.pub_nm
	, vNames.publoc_nm
	, pid.override
	, pid.issuedow
	, pid.issuedate
	, quantity = dbo.PubRateMapInsertQuantityByInsertDate(pid.issuedate, p.pub_pubquantitytype_id, gm.pub_pubrate_map_id)
	, p.description AS GridDescription
	, pid.override AS ScenarioFlag
	, pid.override AS GroupFlag
	, display = 1
	, displaySortOrder = substring('0000000000', 0, 10 - len(convert(varchar(50), sortorder))) + convert(varchar(50), sortorder)
			+ '.' + vNames.pub_id + '.' 
			+ substring('00000', 0, 5 - len(convert(varchar(50), vNames.publoc_id))) + convert(varchar(50), vNames.publoc_id)
FROM
	dbo.est_estimate e (nolock)
	INNER JOIN dbo.est_package p (nolock)
		ON e.est_estimate_id = p.est_estimate_id
	INNER JOIN dbo.pub_pubgroup grp (nolock)
		ON p.pub_pubgroup_id = grp.pub_pubgroup_id
	INNER JOIN dbo.pub_pubpubgroup_map gm (nolock)
		ON grp.pub_pubgroup_id = gm.pub_pubgroup_id
	INNER JOIN dbo.vw_PubRateMap_withPubAndPublocNames vNames (nolock)
		ON gm.pub_pubrate_map_id = vNames.pub_pubrate_map_id
	INNER JOIN dbo.est_pubissuedates pid (nolock)
		ON gm.pub_pubrate_map_id = pid.pub_pubrate_map_id
		AND p.est_estimate_id = pid.est_estimate_id
	INNER JOIN dbo.pub_pubquantitytype pqt (nolock)
		ON p.pub_pubquantitytype_id = pqt.pub_pubquantitytype_id

WHERE
	e.est_estimate_id = @EST_Estimate_ID
GO

GRANT  EXECUTE  ON [dbo].[InsertSetupInfo_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.Mailhouse_s_MailhouseIDandDescription_ByRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.Mailhouse_s_MailhouseIDandDescription_ByRunDate'
	DROP PROCEDURE dbo.Mailhouse_s_MailhouseIDandDescription_ByRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.Mailhouse_s_MailhouseIDandDescription_ByRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.Mailhouse_s_MailhouseIDandDescription_ByRunDate FAILED.'
END
GO
PRINT 'Creating dbo.Mailhouse_s_MailhouseIDandDescription_ByRunDate'
GO

CREATE PROC dbo.Mailhouse_s_MailhouseIDandDescription_ByRunDate
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns a list of Vendor mailhouse ID's and Vendor Descriptions for the specified RunDate
*
* TABLES:
*		Table Name					Access
*		==========					======
*		Vnd_Vendor					READ
*		vnd_mailhouserate			READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
*	Date				Who					Comments
*	------------- 	--------        -------------------------------------------------
*	08/28/2007			NLS					Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@RunDate datetime

AS
SELECT
	vendor.Description,
	mailhouse.vnd_vendor_id, 
    mailhouse.vnd_mailhouserate_id, 
	mailhouse.postaldropcwt, 
	mailhouse.gluetackdefault,
	mailhouse.tabbingdefault, 
	mailhouse.letterinsertiondefault
FROM
	vnd_vendor vendor
	INNER JOIN (
		SELECT 
			vnd_mailhouserate.vnd_vendor_id, 
			vnd_mailhouserate.vnd_mailhouserate_id, 
			vnd_mailhouserate.postaldropcwt, 
			vnd_mailhouserate.gluetackdefault,
			vnd_mailhouserate.tabbingdefault, 
			vnd_mailhouserate.letterinsertiondefault
		FROM 
			vnd_mailhouserate
			INNER JOIN ( 
				SELECT
					vnd_vendor_id,
					max(effectivedate) as effectivedate
				FROM
					vnd_mailhouserate
				WHERE
					effectivedate <= @RunDate
				GROUP BY
					vnd_vendor_id
				) effective
			ON vnd_mailhouserate.vnd_vendor_id = effective.vnd_vendor_id AND
			   vnd_mailhouserate.effectivedate = effective.effectivedate
		) mailhouse
	ON vendor.vnd_vendor_id = mailhouse.vnd_vendor_id

WHERE
	vendor.active = 1
ORDER BY
	vendor.Description ASC

GO

GRANT  EXECUTE  ON [dbo].[Mailhouse_s_MailhouseIDandDescription_ByRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.MailList_s_MailListIDandDescription_ByRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.MailList_s_MailListIDandDescription_ByRunDate'
	DROP PROCEDURE dbo.MailList_s_MailListIDandDescription_ByRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.MailList_s_MailListIDandDescription_ByRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.MailList_s_MailListIDandDescription_ByRunDate FAILED.'
END
GO
PRINT 'Creating dbo.MailList_s_MailListIDandDescription_ByRunDate'
GO

CREATE PROC dbo.MailList_s_MailListIDandDescription_ByRunDate
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns a list of Vendor MailList ID's and Vendor Descriptions for the specified RunDate
*
* TABLES:
*		Table Name					Access
*		==========					======
*		Vnd_Vendor					READ
*		vnd_maillistresourcerate    READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
*	Date				Who					Comments
*	------------- 	--------        -------------------------------------------------
*	08/28/2007			NLS					Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@RunDate datetime

AS

SELECT v.vnd_vendor_id, max(m.vnd_maillistresourcerate_id) vnd_maillistresourcerate_id, max(v.Description) Description
FROM Vnd_Vendor v JOIN vnd_maillistresourcerate m ON v.vnd_vendor_id = m.vnd_vendor_id
	LEFT JOIN vnd_maillistresourcerate newer_m ON v.vnd_vendor_id = newer_m.vnd_vendor_id and newer_m.EffectiveDate <= @RunDate and newer_m.EffectiveDate > m.EffectiveDate
WHERE m.EffectiveDate <= @RunDate and newer_m.vnd_maillistresourcerate_id is null
GROUP BY v.vnd_vendor_id
ORDER BY v.Description
GO

GRANT  EXECUTE  ON [dbo].[MailList_s_MailListIDandDescription_ByRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.MailTracking_s_MailTrackingIDandDescription_ByRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.MailTracking_s_MailTrackingIDandDescription_ByRunDate'
	DROP PROCEDURE dbo.MailTracking_s_MailTrackingIDandDescription_ByRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.MailTracking_s_MailTrackingIDandDescription_ByRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.MailTracking_s_MailTrackingIDandDescription_ByRunDate FAILED.'
END
GO
PRINT 'Creating dbo.MailTracking_s_MailTrackingIDandDescription_ByRunDate'
GO

CREATE PROC dbo.MailTracking_s_MailTrackingIDandDescription_ByRunDate
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns a list of Vendor mailtracking ID's and Vendor Descriptions for the specified RunDate
*
* TABLES:
*		Table Name					Access
*		==========					======
*		Vnd_Vendor					READ
*		vnd_mailtrackingrate	    READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
*	Date				Who					Comments
*	------------- 	--------        -------------------------------------------------
*	08/28/2007			NLS					Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@RunDate datetime

AS

SELECT v.vnd_vendor_id, max(m.vnd_mailtrackingrate_id) vnd_mailtrackingrate_id, max(v.Description) Description
FROM Vnd_Vendor v JOIN vnd_mailtrackingrate m ON v.vnd_vendor_id = m.vnd_vendor_id
	LEFT JOIN vnd_mailtrackingrate newer_m ON v.vnd_vendor_id = newer_m.vnd_vendor_id and newer_m.EffectiveDate <= @RunDate and newer_m.EffectiveDate > m.EffectiveDate
WHERE m.EffectiveDate <= @RunDate and newer_m.vnd_mailtrackingrate_id is null
GROUP BY v.vnd_vendor_id
ORDER BY v.Description
GO

GRANT  EXECUTE  ON [dbo].[MailTracking_s_MailTrackingIDandDescription_ByRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PaperMap_s_ByPaperIDPaperWeightIDPaperGradeIDRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PaperMap_s_ByPaperIDPaperWeightIDPaperGradeIDRunDate'
	DROP PROCEDURE dbo.PaperMap_s_ByPaperIDPaperWeightIDPaperGradeIDRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PaperMap_s_ByPaperIDPaperWeightIDPaperGradeIDRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PaperMap_s_ByPaperIDPaperWeightIDPaperGradeIDRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PaperMap_s_ByPaperIDPaperWeightIDPaperGradeIDRunDate'
GO

CREATE PROC dbo.PaperMap_s_ByPaperIDPaperWeightIDPaperGradeIDRunDate
@VND_Paper_ID bigint,
@PPR_PaperWeight_ID int,
@PPR_PaperGrade_ID int,
@RunDate datetime
as
/*
* PARAMETERS:
* VND_Paper_ID - required
* PPR_PaperWeight_ID required
* PPR_PaperGrade_ID - required
* RunDate - required
*
* DESCRIPTION:
* Returns all paper map rates vendor, paper weight, paper grade for the specified run date.
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   VND_Paper                      READ
*   PPR_Paper_Map                  READ
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
* 08/14/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/

select *
from PPR_Paper_Map
where VND_Paper_ID = @VND_Paper_ID and PPR_PaperWeight_ID = @PPR_PaperWeight_ID and PPR_PaperGrade_ID = @PPR_PaperGrade_ID
order by [default] desc
GO

GRANT  EXECUTE  ON [dbo].[PaperMap_s_ByPaperIDPaperWeightIDPaperGradeIDRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PaperMap_s_PaperMapID_ByOldPaperMapIDandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PaperMap_s_PaperMapID_ByOldPaperMapIDandRunDate'
	DROP PROCEDURE dbo.PaperMap_s_PaperMapID_ByOldPaperMapIDandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PaperMap_s_PaperMapID_ByOldPaperMapIDandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PaperMap_s_PaperMapID_ByOldPaperMapIDandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PaperMap_s_PaperMapID_ByOldPaperMapIDandRunDate'
GO

CREATE proc dbo.PaperMap_s_PaperMapID_ByOldPaperMapIDandRunDate
/*
* PARAMETERS:
* PPR_Paper_Map_ID - required
* RunDate - required
*
* DESCRIPTION:
*		Determines the parent vendor record of PPR_Paper_Map_ID.  Returns a paper map record with the same parent vendor for the specified run date
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   VND_Paper           READ
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
* 08/16/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PPR_Paper_Map_ID bigint,
@RunDate datetime
as

declare @VND_Vendor_ID bigint, @VND_Paper_ID bigint, @PaperMapDesc varchar(35)

-- Identify the VendorID and the original Paper Map description
select @VND_Vendor_ID = p.VND_Vendor_ID, @PaperMapDesc = pm.Description
from VND_Paper p join PPR_Paper_Map pm on p.VND_Paper_ID = pm.VND_Paper_ID
where pm.PPR_Paper_Map_ID = @PPR_Paper_Map_ID

-- Find the new PaperID
select top 1 @VND_Paper_ID = p.VND_Paper_ID
from VND_Paper p
where p.VND_Vendor_ID = @VND_Vendor_ID and p.EffectiveDate <= @RunDate
order by p.EffectiveDate desc

-- If the original Paper Map is still available return it
if exists(select 1 from PPR_Paper_Map where VND_Paper_ID = @VND_Paper_ID and PPR_Paper_Map_ID = @PPR_Paper_Map_ID) begin
	select * from PPR_Paper_Map where PPR_Paper_Map_ID = @PPR_Paper_Map_ID
end
--Otherwise try to return a paper map with an identical description
else if exists(select 1 from PPR_Paper_Map where VND_Paper_ID = @VND_Paper_ID and Description = @PaperMapDesc) begin
	select * from PPR_Paper_Map where VND_Paper_ID = @VND_Paper_ID and Description = @PaperMapDesc
end
-- The last resort is to try to return the default paper map
else begin
	select * From PPR_Paper_Map where VND_Paper_ID = @VND_Paper_ID and [default] = 1
end
GO

GRANT  EXECUTE  ON [dbo].[PaperMap_s_PaperMapID_ByOldPaperMapIDandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.Paper_s_PaperIDandDescription_ByRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.Paper_s_PaperIDandDescription_ByRunDate'
	DROP PROCEDURE dbo.Paper_s_PaperIDandDescription_ByRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.Paper_s_PaperIDandDescription_ByRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.Paper_s_PaperIDandDescription_ByRunDate FAILED.'
END
GO
PRINT 'Creating dbo.Paper_s_PaperIDandDescription_ByRunDate'
GO

create proc dbo.Paper_s_PaperIDandDescription_ByRunDate
/*
* PARAMETERS:
* RunDate - Required
*
*
* DESCRIPTION:
*		Returns a list of Vendor Paper ID's and Vendor Descriptions for the specified RunDate
*
*
* TABLES:
*  Table Name     Access
*  ==========     ======
*  VND_Vendor     READ
*  VND_Paper      READ
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
* Date              Who         Comments
* -------------     --------    -------------------------------------------------
* 08/16/2007        BJS         Initial Creation 
* 09/24/2007        BJS         Added VND_Paper_ID parameter
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Paper_ID bigint,
@RunDate datetime
as

select v.VND_Vendor_ID, max(p.VND_Paper_ID) VND_Paper_ID, max(v.Description) Description
from VND_Vendor v join VND_Paper p on v.VND_Vendor_ID = p.VND_Vendor_ID
	left join VND_Paper newer_p on v.VND_Vendor_ID = newer_p.VND_Vendor_ID and newer_p.EffectiveDate <= @RunDate and newer_p.EffectiveDate > p.EffectiveDate
where (v.Active = 1 or p.VND_Paper_ID = @VND_Paper_ID) and p.EffectiveDate <= @RunDate and newer_p.VND_Paper_ID is null
group by v.VND_Vendor_ID
order by v.Description
GO

GRANT  EXECUTE  ON [dbo].[Paper_s_PaperIDandDescription_ByRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.Paper_s_PaperID_ByOldPaperIDandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.Paper_s_PaperID_ByOldPaperIDandRunDate'
	DROP PROCEDURE dbo.Paper_s_PaperID_ByOldPaperIDandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.Paper_s_PaperID_ByOldPaperIDandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.Paper_s_PaperID_ByOldPaperIDandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.Paper_s_PaperID_ByOldPaperIDandRunDate'
GO

CREATE proc dbo.Paper_s_PaperID_ByOldPaperIDandRunDate
/*
* PARAMETERS:
* VND_Paper_ID - required
* RunDate - required
*
* DESCRIPTION:
*		Determines the parent vendor record of VND_Paper_ID.  Returns a paper record with the same parent vendor for the specified run date
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   VND_Paper         READ
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
* 08/16/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Paper_ID bigint,
@RunDate datetime
as

declare @VND_Vendor_ID bigint

select @VND_Vendor_ID = VND_Vendor_ID
from VND_Paper
where VND_Paper_ID = @VND_Paper_ID

select top 1 p.*
from VND_Paper p
where p.VND_Vendor_ID = @VND_Vendor_ID and p.EffectiveDate <= @RunDate
order by p.EffectiveDate desc
GO

GRANT  EXECUTE  ON [dbo].[Paper_s_PaperID_ByOldPaperIDandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
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

GO
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


GO
IF OBJECT_ID('dbo.PostalClass_s_all_ByPostalClassID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PostalClass_s_all_ByPostalClassID'
	DROP PROCEDURE dbo.PostalClass_s_all_ByPostalClassID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PostalClass_s_all_ByPostalClassID') IS NOT NULL
		PRINT '***********Drop of dbo.PostalClass_s_all_ByPostalClassID FAILED.'
END
GO
PRINT 'Creating dbo.PostalClass_s_all_ByPostalClassID'
GO

create proc dbo.PostalClass_s_all_ByPostalClassID
/*
* PARAMETERS:
* PST_PostalClass_ID - Required.  The PostalClassID.
*
*
* DESCRIPTION:
*		Returns the PST_PostalClass record matching PST_PostalClass_ID
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PST_PostalClass
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
* 05/23/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PST_PostalClass_ID bigint
as

select * from PST_PostalClass
where PST_PostalClass_ID = @PST_PostalClass_ID


GO

GRANT  EXECUTE  ON [dbo].[PostalClass_s_all_ByPostalClassID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PostalMailerType_s_all_ByPostalMailerTypeID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PostalMailerType_s_all_ByPostalMailerTypeID'
	DROP PROCEDURE dbo.PostalMailerType_s_all_ByPostalMailerTypeID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PostalMailerType_s_all_ByPostalMailerTypeID') IS NOT NULL
		PRINT '***********Drop of dbo.PostalMailerType_s_all_ByPostalMailerTypeID FAILED.'
END
GO
PRINT 'Creating dbo.PostalMailerType_s_all_ByPostalMailerTypeID'
GO

create proc dbo.PostalMailerType_s_all_ByPostalMailerTypeID
/*
* PARAMETERS:
* PST_PostalMailerType_ID - Required.  The PostalMailerTypeID.
*
*
* DESCRIPTION:
*		Returns the PST_PostalMailerType record matching PST_PostalMailerType_ID
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PST_PostalMailerType
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
* 05/23/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PST_PostalMailerType_ID bigint
as

select * from PST_PostalMailerType
where PST_PostalMailerType_ID = @PST_PostalMailerType_ID


GO

GRANT  EXECUTE  ON [dbo].[PostalMailerType_s_all_ByPostalMailerTypeID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PostalScenario_s_PostalScenarioIDandDescription_ByRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PostalScenario_s_PostalScenarioIDandDescription_ByRunDate'
	DROP PROCEDURE dbo.PostalScenario_s_PostalScenarioIDandDescription_ByRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PostalScenario_s_PostalScenarioIDandDescription_ByRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PostalScenario_s_PostalScenarioIDandDescription_ByRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PostalScenario_s_PostalScenarioIDandDescription_ByRunDate'
GO

CREATE PROC dbo.PostalScenario_s_PostalScenarioIDandDescription_ByRunDate
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns a list of postal scenario ID's and Descriptions for the specified RunDate
*
* TABLES:
*		Table Name					Access
*		==========					======
*		pst_postalscenario			READ
*		pst_postalclass				READ
*		pst_postalmailertype		READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
*	Date		Who		Comments
*	------------- 	--------        -------------------------------------------------
*	08/28/2007	NLS		Initial Creation 
*	11/20/2007	JRH		Fixed the selection based on effective date.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@RunDate datetime

AS
SELECT
	max(postal.pst_postalscenario_id) pst_postalscenario_id,
	postal.description description,
	max(mailer.description) postalmailertype,
	max(class.description) postalclass
FROM
	(SELECT
			postal.description description,
			max(postal.effectivedate) effectivedate
		FROM
			pst_postalscenario postal 
		WHERE
			postal.effectivedate <= @RunDate
		GROUP BY
		 	postal.description) eps
	INNER JOIN pst_postalscenario postal
		ON eps.description = postal.description
		AND eps.effectivedate = postal.effectivedate
	INNER JOIN pst_postalmailertype mailer ON
		postal.pst_postalmailertype_id = mailer.pst_postalmailertype_id
	INNER JOIN pst_postalclass class ON
		postal.pst_postalclass_id = class.pst_postalclass_id
GROUP BY
	postal.description
ORDER BY
	postal.description
	
GO

GRANT  EXECUTE  ON [dbo].[PostalScenario_s_PostalScenarioIDandDescription_ByRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.PprPaperGrade_s_ByGrade') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperGrade_s_ByGrade'
	DROP PROCEDURE dbo.PprPaperGrade_s_ByGrade
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperGrade_s_ByGrade') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperGrade_s_ByGrade FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperGrade_s_ByGrade'
GO

create proc dbo.PprPaperGrade_s_ByGrade
/*
* PARAMETERS:
* Grade - Required.
*
*
* DESCRIPTION:
*		Returns the Paper Grade for the specified Grade.
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		PPR_PaperGrade      READ
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
*	Date            Who             Comments
*	------------- 	--------        -------------------------------------------------
*   09/04/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Grade varchar(50)
as
select * from PPR_PaperGrade
where Grade = @Grade
GO

GRANT  EXECUTE  ON [dbo].[PprPaperGrade_s_ByGrade]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PprPaperGrade_s_Grade_ByPaperIDandPaperWeightID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperGrade_s_Grade_ByPaperIDandPaperWeightID'
	DROP PROCEDURE dbo.PprPaperGrade_s_Grade_ByPaperIDandPaperWeightID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperGrade_s_Grade_ByPaperIDandPaperWeightID') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperGrade_s_Grade_ByPaperIDandPaperWeightID FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperGrade_s_Grade_ByPaperIDandPaperWeightID'
GO

create proc dbo.PprPaperGrade_s_Grade_ByPaperIDandPaperWeightID
/*
* PARAMETERS:
* VND_Paper_ID - Required.  The PaperID.
*	VND_PaperWeight_ID - Required.  The PaperWeightID.
*
*
* DESCRIPTION:
*		Returns the Paper Grades for the VND_Paper_ID and VND_PaperWeight_ID.
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		PPR_Paper_Map
*		PPR_PaperGrade
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
*	Date						Who							Comments
*	------------- 	--------        -------------------------------------------------
* 05/23/2007			BJS							Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Paper_ID bigint,
@PPR_PaperWeight_ID int
as

select pg.PPR_PaperGrade_ID, pg.Grade
from PPR_Paper_Map pm join PPR_PaperGrade pg on pm.PPR_PaperGrade_ID = pg.PPR_PaperGrade_ID
where pm.VND_Paper_ID = @VND_Paper_ID and pm.PPR_PaperWeight_ID = @PPR_PaperWeight_ID
order by pg.Grade


GO

GRANT  EXECUTE  ON [dbo].[PprPaperGrade_s_Grade_ByPaperIDandPaperWeightID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
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

GO
IF OBJECT_ID('dbo.PprPaperMap_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperMap_i'
	DROP PROCEDURE dbo.PprPaperMap_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperMap_i') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperMap_i FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperMap_i'
GO

CREATE PROC dbo.PprPaperMap_i
@PPR_Paper_Map_ID bigint output,
@Description varchar(35),
@CWT money,
@Default bit,
@PPR_PaperGrade_ID int,
@PPR_PaperWeight_ID int,
@VND_Paper_ID bigint,
@CreatedBy varchar(50)
/*
* PARAMETERS:
* PPR_Paper_Map_ID
* Description
* CWT
* Default
* PPR_PaperGrade_ID
* PPR_PaperWeight_ID
* VND_Paper_ID
* CreatedBy
*
* DESCRIPTION:
* Creates a Paper Map record.  If it is the default, set the old default to "not-default".
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   PPR_Paper_Map                  INSERT
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
as

if (@Default = 1) begin
	update PPR_Paper_Map
		set
			[Default] = 0,
			ModifiedBy = @CreatedBy,
			ModifiedDate = getdate()
	where VND_Paper_ID = @VND_Paper_ID
		and [Default] = 1
end

insert PPR_Paper_Map(Description, CWT, [Default], PPR_PaperGrade_ID, PPR_PaperWeight_ID, VND_Paper_ID, CreatedBy, CreatedDate)
values(@Description, @CWT, @Default, @PPR_PaperGrade_ID, @PPR_PaperWeight_ID, @VND_Paper_ID, @CreatedBy, getdate())
set @PPR_Paper_Map_ID = @@identity
GO

GRANT  EXECUTE  ON [dbo].[PprPaperMap_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PprPaperMap_s_ByPaperIDandDescription') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperMap_s_ByPaperIDandDescription'
	DROP PROCEDURE dbo.PprPaperMap_s_ByPaperIDandDescription
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperMap_s_ByPaperIDandDescription') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperMap_s_ByPaperIDandDescription FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperMap_s_ByPaperIDandDescription'
GO

create proc dbo.PprPaperMap_s_ByPaperIDandDescription
/*
* PARAMETERS:
* VND_Paper_ID - Required.  The PaperID.
* Description - Required.
*
*
* DESCRIPTION:
*		Returns the Paper Map record for the Paper Vendor and Description specified
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		PPR_Paper_Map       READ
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
*	Date            Who             Comments
*	------------- 	--------        -------------------------------------------------
*   09/04/07        BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Paper_ID bigint,
@Description varchar(35)
as

--Try to find an exact match
if exists(select 1 from PPR_Paper_Map where VND_Paper_ID = @VND_Paper_ID and Description = @Description) begin
	select * from PPR_Paper_Map where VND_Paper_ID = @VND_Paper_ID and Description = @Description
end
--If no exact match can be found, return the default
else begin
	select * from PPR_Paper_Map where VND_Paper_ID = @VND_Paper_ID and [Default] = 1
end
GO

GRANT  EXECUTE  ON [dbo].[PprPaperMap_s_ByPaperIDandDescription]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PprPaperMap_s_ByPaperIDDescriptionWeightGrade') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperMap_s_ByPaperIDDescriptionWeightGrade'
	DROP PROCEDURE dbo.PprPaperMap_s_ByPaperIDDescriptionWeightGrade
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperMap_s_ByPaperIDDescriptionWeightGrade') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperMap_s_ByPaperIDDescriptionWeightGrade FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperMap_s_ByPaperIDDescriptionWeightGrade'
GO

create proc dbo.PprPaperMap_s_ByPaperIDDescriptionWeightGrade
/*
* PARAMETERS:
* VND_Paper_ID
* Description
* PPR_PaperWeight_ID
* PPR_PaperGrade_ID
*
*
* DESCRIPTION:
*		Returns the Paper Map record matching the specified parameters.
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		PPR_Paper_Map       READ
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
*	Date            Who             Comments
*	------------- 	--------        -------------------------------------------------
*   10/16/07        BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Paper_ID bigint,
@Description varchar(35),
@PPR_PaperGrade_ID int,
@PPR_PaperWeight_ID int
as

--Try to find an exact match
select * from PPR_Paper_Map
where VND_Paper_ID = @VND_Paper_ID and Description = @Description and PPR_PaperGrade_ID = @PPR_PaperGrade_ID
	and PPR_PaperWeight_ID = @PPR_PaperWeight_ID
GO

GRANT  EXECUTE  ON [dbo].[PprPaperMap_s_ByPaperIDDescriptionWeightGrade]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PprPaperMap_s_Default_ByPaperID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperMap_s_Default_ByPaperID'
	DROP PROCEDURE dbo.PprPaperMap_s_Default_ByPaperID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperMap_s_Default_ByPaperID') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperMap_s_Default_ByPaperID FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperMap_s_Default_ByPaperID'
GO

create proc dbo.PprPaperMap_s_Default_ByPaperID
/*
* PARAMETERS:
* VND_Paper_ID - Required.  The PaperID.
*
*
* DESCRIPTION:
*		Returns the default Paper Map record for the Paper Vendor specified.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
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
*	Date            Who             Comments
*	------------- 	--------        -------------------------------------------------
*   10/10/07        BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Paper_ID bigint
as

select * from PPR_Paper_Map
where VND_Paper_ID = @VND_Paper_ID and [Default] = 1
GO

GRANT  EXECUTE  ON [dbo].[PprPaperMap_s_Default_ByPaperID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PprPaperMap_s_PaperWeightIDandPaperGradeID_ByPaperID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperMap_s_PaperWeightIDandPaperGradeID_ByPaperID'
	DROP PROCEDURE dbo.PprPaperMap_s_PaperWeightIDandPaperGradeID_ByPaperID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperMap_s_PaperWeightIDandPaperGradeID_ByPaperID') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperMap_s_PaperWeightIDandPaperGradeID_ByPaperID FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperMap_s_PaperWeightIDandPaperGradeID_ByPaperID'
GO

create proc dbo.PprPaperMap_s_PaperWeightIDandPaperGradeID_ByPaperID
/*
* PARAMETERS:
* VND_Paper_ID - Required.  The PaperID.
* PPR_PaperGrade_ID - Output.
* PPR_PaperWeight_ID - Output.
*
*
* DESCRIPTION:
*		Returns the default PPR_PaperGrade_ID and PPR_PaperWeight_ID for the PaperID
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		PPR_Paper_Map
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
*	Date						Who							Comments
*	------------- 	--------        -------------------------------------------------
* 05/23/2007			BJS							Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Paper_ID bigint,
@PPR_PaperWeight_ID int output,
@PPR_PaperGrade_ID int output
as

select @PPR_PaperWeight_ID = PPR_PaperWeight_ID, @PPR_PaperGrade_ID = PPR_PaperGrade_ID
from PPR_Paper_Map
where VND_Paper_ID = @VND_Paper_ID and [Default] = 1


GO

GRANT  EXECUTE  ON [dbo].[PprPaperMap_s_PaperWeightIDandPaperGradeID_ByPaperID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PprPaperMap_u') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperMap_u'
	DROP PROCEDURE dbo.PprPaperMap_u
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperMap_u') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperMap_u FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperMap_u'
GO

CREATE PROC dbo.PprPaperMap_u
/*
* PARAMETERS:
* PPR_Paper_Map_ID
* Description
* CWT
* Default
* PPR_PaperGrade_ID
* PPR_PaperWeight_ID
* VND_Paper_ID
* ModifiedBy
*
* DESCRIPTION:
* Updates a Paper Map record.  If it is the default, set the old default to "not-default".
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   PPR_Paper_Map                  UPDATE
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
@PPR_Paper_Map_ID bigint,
@Description varchar(35),
@CWT money,
@Default bit,
@PPR_PaperGrade_ID int,
@PPR_PaperWeight_ID int,
@VND_Paper_ID bigint,
@ModifiedBy varchar(50)
as

if (@Default = 1) begin
	update PPR_Paper_Map
		set
			[Default] = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedDate = getdate()
	where PPR_Paper_Map_ID <> @PPR_Paper_Map_ID
		and VND_Paper_ID = @VND_Paper_ID
		and [Default] = 1
end

update PPR_Paper_Map
	set
		Description = @Description,
		CWT = @CWT,
		[Default] = @Default,
		PPR_PaperGrade_ID = @PPR_PaperGrade_ID,
		PPR_PaperWeight_ID = @PPR_PaperWeight_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where PPR_Paper_Map_ID = @PPR_Paper_Map_ID
GO

GRANT  EXECUTE  ON [dbo].[PprPaperMap_u]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PprPaperWeight_s_ByWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperWeight_s_ByWeight'
	DROP PROCEDURE dbo.PprPaperWeight_s_ByWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperWeight_s_ByWeight') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperWeight_s_ByWeight FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperWeight_s_ByWeight'
GO

create proc dbo.PprPaperWeight_s_ByWeight
/*
* PARAMETERS:
* Weight - Required.
*
*
* DESCRIPTION:
*		Returns the Paper Weights for the specified Weight.
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		PPR_PaperWeight     READ
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
*	Date            Who             Comments
*	------------- 	--------        -------------------------------------------------
*   09/04/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Weight int
as

select * from PPR_PaperWeight
where Weight = @Weight
GO

GRANT  EXECUTE  ON [dbo].[PprPaperWeight_s_ByWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PprPaperWeight_s_Weight_ByPaperID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperWeight_s_Weight_ByPaperID'
	DROP PROCEDURE dbo.PprPaperWeight_s_Weight_ByPaperID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperWeight_s_Weight_ByPaperID') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperWeight_s_Weight_ByPaperID FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperWeight_s_Weight_ByPaperID'
GO

create proc dbo.PprPaperWeight_s_Weight_ByPaperID
/*
* PARAMETERS:
* VND_Paper_ID - Required.  The PaperID.
*
*
* DESCRIPTION:
*		Returns the Paper Weights for the VND_Paper_ID.
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		PPR_Paper_Map
*		PPR_PaperWeight
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
*	Date						Who							Comments
*	------------- 	--------        -------------------------------------------------
* 05/23/2007			BJS							Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Paper_ID bigint
as

select pw.PPR_PaperWeight_ID, pw.Weight
from PPR_Paper_Map pm join PPR_PaperWeight pw on pm.PPR_PaperWeight_ID = pw.PPR_PaperWeight_ID
where pm.VND_Paper_ID = @VND_Paper_ID
order by pw.Weight


GO

GRANT  EXECUTE  ON [dbo].[PprPaperWeight_s_Weight_ByPaperID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PrinterRate_s_ByPrinterIDPrinterRateTypeID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PrinterRate_s_ByPrinterIDPrinterRateTypeID'
	DROP PROCEDURE dbo.PrinterRate_s_ByPrinterIDPrinterRateTypeID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PrinterRate_s_ByPrinterIDPrinterRateTypeID') IS NOT NULL
		PRINT '***********Drop of dbo.PrinterRate_s_ByPrinterIDPrinterRateTypeID FAILED.'
END
GO
PRINT 'Creating dbo.PrinterRate_s_ByPrinterIDPrinterRateTypeID'
GO

CREATE PROC dbo.PrinterRate_s_ByPrinterIDPrinterRateTypeID
@VND_Printer_ID bigint,
@PRT_PrinterRateType_ID int
/*
* PARAMETERS:
* VND_Vendor_ID - required
* PRT_PrinterRateType_ID required
*
* DESCRIPTION:
* Returns all printer rates for the printer and rate type.
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   PRT_PrinterRate                READ
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
* 08/16/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
as

select pr.*
from PRT_PrinterRate pr
where pr.VND_Printer_ID = @VND_Printer_ID and pr.PRT_PrinterRateType_ID = @PRT_PrinterRateType_ID
order by pr.[default] desc
GO

GRANT  EXECUTE  ON [dbo].[PrinterRate_s_ByPrinterIDPrinterRateTypeID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate'
	DROP PROCEDURE dbo.PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate'
GO

CREATE proc dbo.PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate
/*
* PARAMETERS:
* PRT_PrinterRate_ID - required
* RunDate - required
*
* DESCRIPTION:
*		Determines the parent vendor record of PRT_PrinterRate_ID.  Returns a printer rate record with the same parent vendor and rate type
*   for the specified run date
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   VND_Printer         READ
*   PRT_PrinterRate     READ
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
* 08/16/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PRT_PrinterRate_ID bigint,
@RunDate datetime
as

declare @VND_Vendor_ID bigint, @VND_Printer_ID bigint, @PrinterRateDesc varchar(35)

-- Identify the VendorID and the original Printer Rate description
select @VND_Vendor_ID = VND_Vendor_ID, @PrinterRateDesc = pr.Description
from VND_Printer p join PRT_PrinterRate pr on p.VND_Printer_ID = pr.VND_Printer_ID
where pr.PRT_PrinterRate_ID = @PRT_PrinterRate_ID

-- Find the new PrinterID
select top 1 @VND_Printer_ID = p.VND_Printer_ID
from VND_Printer p
where p.VND_Vendor_ID = @VND_Vendor_ID and p.EffectiveDate <= @RunDate
order by p.EffectiveDate desc

-- If the original Printer Rate is still available return it
if exists(select 1 from PRT_PrinterRate where VND_Printer_ID = @VND_Printer_ID and PRT_PrinterRate_ID = @PRT_PrinterRate_ID) begin
	select * from PRT_PrinterRate where PRT_PrinterRate_ID = @PRT_PrinterRate_ID
end
-- Otherwise try to return a printer rate with an identical description
else if exists(select 1 from PRT_PrinterRate where VND_Printer_ID = @VND_Printer_ID and Description = @PrinterRateDesc) begin
	select * from PRT_PrinterRate where VND_Printer_ID = @VND_Printer_ID and Description = @PrinterRateDesc
end
-- The last resort is to try to return the default rate
else begin
	select * from PRT_PrinterRate where VND_Printer_ID = @VND_Printer_ID and [default] = 1
end

GO

GRANT  EXECUTE  ON [dbo].[PrinterRate_s_PrinterRateID_ByOldPrinterRateIDandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.Printer_s_PrinterIDandDescription_ByRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.Printer_s_PrinterIDandDescription_ByRunDate'
	DROP PROCEDURE dbo.Printer_s_PrinterIDandDescription_ByRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.Printer_s_PrinterIDandDescription_ByRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.Printer_s_PrinterIDandDescription_ByRunDate FAILED.'
END
GO
PRINT 'Creating dbo.Printer_s_PrinterIDandDescription_ByRunDate'
GO

create proc dbo.Printer_s_PrinterIDandDescription_ByRunDate
/*
* PARAMETERS:
* RunDate - Required
*
*
* DESCRIPTION:
*		Returns a list of Vendor Printer ID's and Vendor Descriptions for the specified RunDate
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		VND_Vendor      READ
*		VND_Printer     READ
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
*	Date            Who             Comments
*	------------- 	--------        -------------------------------------------------
* 08/16/2007			BJS							Initial Creation 
* 08/23/2007			NLS							Added extra rates to the select
* 09/20/2007            BJS         Added VND_Printer_ID as an optional parameter
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Printer_ID bigint,
@RunDate datetime
as

select v.VND_Vendor_ID, max(p.VND_Printer_ID) VND_Printer_ID, max(v.Description) Description, max(p.polybagmessage) PolybagMessage, max(p.polybagmessagemakeready) PolybagMessageMakeready
from VND_Vendor v join VND_Printer p on v.VND_Vendor_ID = p.VND_Vendor_ID
	left join VND_Printer newer_p on v.VND_Vendor_ID = newer_p.VND_Vendor_ID and newer_p.EffectiveDate <= @RunDate and newer_p.EffectiveDate > p.EffectiveDate
where (v.Active = 1 or p.VND_Printer_ID = @VND_Printer_ID) and p.EffectiveDate <= @RunDate and newer_p.VND_Printer_ID is null
group by v.VND_Vendor_ID
order by v.Description
GO

GRANT  EXECUTE  ON [dbo].[Printer_s_PrinterIDandDescription_ByRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.Printer_s_PrinterID_ByOldPrinterIDandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.Printer_s_PrinterID_ByOldPrinterIDandRunDate'
	DROP PROCEDURE dbo.Printer_s_PrinterID_ByOldPrinterIDandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.Printer_s_PrinterID_ByOldPrinterIDandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.Printer_s_PrinterID_ByOldPrinterIDandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.Printer_s_PrinterID_ByOldPrinterIDandRunDate'
GO

CREATE proc dbo.Printer_s_PrinterID_ByOldPrinterIDandRunDate
/*
* PARAMETERS:
* VND_Printer_ID - required
* RunDate - required
*
* DESCRIPTION:
*		Determines the parent vendor record of VND_Printer_ID.  Returns a printer record with the same parent vendor for the specified run date
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   VND_Printer         READ
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
* 08/16/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Printer_ID bigint,
@RunDate datetime
as

declare @VND_Vendor_ID bigint

select @VND_Vendor_ID = VND_Vendor_ID
from VND_Printer
where VND_Printer_ID = @VND_Printer_ID

select top 1 p.*
from VND_Printer p
where p.VND_Vendor_ID = @VND_Vendor_ID and p.EffectiveDate <= @RunDate
order by p.EffectiveDate desc
GO

GRANT  EXECUTE  ON [dbo].[Printer_s_PrinterID_ByOldPrinterIDandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
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

GO
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


GO
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

GO
IF OBJECT_ID('dbo.PrtPrinterRate_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PrtPrinterRate_i'
	DROP PROCEDURE dbo.PrtPrinterRate_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PrtPrinterRate_i') IS NOT NULL
		PRINT '***********Drop of dbo.PrtPrinterRate_i FAILED.'
END
GO
PRINT 'Creating dbo.PrtPrinterRate_i'
GO

CREATE PROC dbo.PrtPrinterRate_i
@PRT_PrinterRate_ID bigint output,
@VND_Printer_ID bigint,
@PRT_PrinterRateType_ID int,
@Rate money,
@Description varchar(35),
@Default bit,
@CreatedBy varchar(50)
/*
* PARAMETERS:
* PRT_PrinterRate_ID
* VND_Printer_ID
* PRT_PrinterRateType_ID
* Rate
* Description
* Default
* CreatedBy
*
* DESCRIPTION:
* Creates a PrinterRate record.  If it is the default, set the old default to "not-default".
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   PRT_PrinterRate                INSERT
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
as

if (@Default = 1) begin
	update PRT_PrinterRate
		set
			[Default] = 0,
			ModifiedBy = @CreatedBy,
			ModifiedDate = getdate()
	where VND_Printer_ID = @VND_Printer_ID
		and PRT_PrinterRateType_ID = @PRT_PrinterRateType_ID
		and [Default] = 1
end

insert PRT_PrinterRate(VND_Printer_ID, PRT_PrinterRateType_ID, Rate, Description, [Default], CreatedBy, CreatedDate)
values(@VND_Printer_ID, @PRT_PrinterRateType_ID, @Rate, @Description, @Default, @CreatedBy, getdate())
set @PRT_PrinterRate_ID = @@identity
GO

GRANT  EXECUTE  ON [dbo].[PrtPrinterRate_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PrtPrinterRate_s_ByPrinterID_Description_PrinterRateTypeID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PrtPrinterRate_s_ByPrinterID_Description_PrinterRateTypeID'
	DROP PROCEDURE dbo.PrtPrinterRate_s_ByPrinterID_Description_PrinterRateTypeID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PrtPrinterRate_s_ByPrinterID_Description_PrinterRateTypeID') IS NOT NULL
		PRINT '***********Drop of dbo.PrtPrinterRate_s_ByPrinterID_Description_PrinterRateTypeID FAILED.'
END
GO
PRINT 'Creating dbo.PrtPrinterRate_s_ByPrinterID_Description_PrinterRateTypeID'
GO

create proc dbo.PrtPrinterRate_s_ByPrinterID_Description_PrinterRateTypeID
/*
* PARAMETERS:
* VND_Printer_ID
* Description
* PRT_PrinterRateType_ID
*
* DESCRIPTION:
* Returns a printer rate matching the criteria.  If a match cannot be found on description the default rate will be returned.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   PRT_PrinterRate                READ
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
@VND_Printer_ID bigint,
@Description varchar(35),
@PRT_PrinterRateType_ID int
as

--Try to find an exact match
if exists(select 1 from PRT_PrinterRate where VND_Printer_ID = @VND_Printer_ID and PRT_PrinterRateType_ID = @PRT_PrinterRateType_ID and Description = @Description) begin
	select * from PRT_PrinterRate
	where VND_Printer_ID = @VND_Printer_ID and PRT_PrinterRateType_ID = @PRT_PrinterRateType_ID
		and Description = @Description
end
--If no exact match can be found, return the default rate
else begin
	select * from PRT_PrinterRate
	where VND_Printer_ID = @VND_Printer_ID and PRT_PrinterRateType_ID = @PRT_PrinterRateType_ID
		and [Default] = 1
end
GO

GRANT  EXECUTE  ON [dbo].[PrtPrinterRate_s_ByPrinterID_Description_PrinterRateTypeID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PrtPrinterRate_s_DescriptionandRate_ByPrinterIDandPrinterRateTypeID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PrtPrinterRate_s_DescriptionandRate_ByPrinterIDandPrinterRateTypeID'
	DROP PROCEDURE dbo.PrtPrinterRate_s_DescriptionandRate_ByPrinterIDandPrinterRateTypeID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PrtPrinterRate_s_DescriptionandRate_ByPrinterIDandPrinterRateTypeID') IS NOT NULL
		PRINT '***********Drop of dbo.PrtPrinterRate_s_DescriptionandRate_ByPrinterIDandPrinterRateTypeID FAILED.'
END
GO
PRINT 'Creating dbo.PrtPrinterRate_s_DescriptionandRate_ByPrinterIDandPrinterRateTypeID'
GO

create proc dbo.PrtPrinterRate_s_DescriptionandRate_ByPrinterIDandPrinterRateTypeID
/*
* PARAMETERS:
* VND_Printer_ID
* PRT_PrinterRateType_ID
*
* DESCRIPTION:
* Returns Printer Rates for the PrinterID and Printer Rate specified.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   PRT_PrinterRate                READ
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
@VND_Printer_ID bigint,
@PRT_PrinterRateType_ID int
as
select PRT_PrinterRate_ID, Description, Rate
from PRT_PrinterRate
where VND_Printer_ID = @VND_Printer_ID and PRT_PrinterRateType_ID = @PRT_PrinterRateType_ID
order by [Default] desc, Description


GO

GRANT  EXECUTE  ON [dbo].[PrtPrinterRate_s_DescriptionandRate_ByPrinterIDandPrinterRateTypeID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PrtPrinterRate_u') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PrtPrinterRate_u'
	DROP PROCEDURE dbo.PrtPrinterRate_u
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PrtPrinterRate_u') IS NOT NULL
		PRINT '***********Drop of dbo.PrtPrinterRate_u FAILED.'
END
GO
PRINT 'Creating dbo.PrtPrinterRate_u'
GO

CREATE PROC dbo.PrtPrinterRate_u
@PRT_PrinterRate_ID bigint,
@VND_Printer_ID bigint,
@PRT_PrinterRateType_ID int,
@Rate money,
@Description varchar(35),
@Default bit,
@ModifiedBy varchar(50)
/*
* PARAMETERS:
* PRT_PrinterRate_ID
* VND_Printer_ID
* PRT_PrinterRateType_ID
* Rate
* Description
* Default
* ModifiedBy
*
* DESCRIPTION:
* Updates a PrinterRate record.  If it is the default, set the old default to "not-default".
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   PRT_PrinterRate                UPDATE
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
as

if (@Default = 1) begin
	update PRT_PrinterRate
		set
			[Default] = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedDate = getdate()
	where PRT_PrinterRate_ID <> @PRT_PrinterRate_ID
		and VND_Printer_ID = @VND_Printer_ID
		and PRT_PrinterRateType_ID = @PRT_PrinterRateType_ID
		and [Default] = 1
end

update PRT_PrinterRate
	set
		Rate = @Rate,
		Description = @Description,
		[Default] = @Default,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where PRT_PrinterRate_ID = @PRT_PrinterRate_ID
GO

GRANT  EXECUTE  ON [dbo].[PrtPrinterRate_u]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PstPostalScenario_s_ByDescriptionandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PstPostalScenario_s_ByDescriptionandRunDate'
	DROP PROCEDURE dbo.PstPostalScenario_s_ByDescriptionandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PstPostalScenario_s_ByDescriptionandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PST_PostalScenario_d FAILED.'
END
GO
PRINT 'Creating dbo.PstPostalScenario_s_ByDescriptionandRunDate'
GO

create proc dbo.PstPostalScenario_s_ByDescriptionandRunDate
/*
* PARAMETERS:
* Description - required.
* RunDate - required.
*
* DESCRIPTION:
* Returns the Postal Scenario that matches the Description on the date specified.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   PST_PostalScenario             READ
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
* 09/04/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Description varchar(35),
@RunDate datetime
as

select top 1 * from PST_PostalScenario
where Description = @Description and EffectiveDate <= @RunDate
order by EffectiveDate desc

GO

GRANT  EXECUTE  ON [dbo].[PstPostalScenario_s_ByDescriptionandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PstPostalScenario_s_ByOldIDandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PstPostalScenario_s_ByOldIDandRunDate'
	DROP PROCEDURE dbo.PstPostalScenario_s_ByOldIDandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PstPostalScenario_s_ByOldIDandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PST_PostalScenario_d FAILED.'
END
GO
PRINT 'Creating dbo.PstPostalScenario_s_ByOldIDandRunDate'
GO

create proc dbo.PstPostalScenario_s_ByOldIDandRunDate
/*
* PARAMETERS:
* PST_PostalScenario_ID - required.
* RunDate - required.
*
* DESCRIPTION:
* Determines the parent vendor of the postal scenario specified.  Returns the Postal Scenario that matches the vendor on the date specified.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   PST_PostalScenario             READ
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
* 11/02/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PST_PostalScenario_ID bigint,
@RunDate datetime
as

declare @VND_Vendor_ID bigint, @Description varchar(35), @PST_PostalMailerType_ID int, @PST_PostalClass_ID int

select @VND_Vendor_ID = pw.VND_Vendor_ID, @Description = Description, @PST_PostalMailerType_ID = ps.PST_PostalMailerType_ID,
	@PST_PostalClass_ID = ps.PST_PostalClass_ID
from PST_PostalScenario ps join PST_PostalCategoryScenario_Map pcsm on ps.PST_PostalScenario_ID = pcsm.PST_PostalScenario_ID
	join PST_PostalCategoryRate_Map pcrm on pcsm.PST_PostalCategoryRate_Map_ID = pcrm.PST_PostalCategoryRate_Map_ID
	join PST_PostalWeights pw on pcrm.PST_PostalWeights_ID = pw.PST_PostalWeights_ID
where ps.PST_PostalScenario_ID = @PST_PostalScenario_ID

select top 1 ps.*
from PST_PostalScenario ps join PST_PostalCategoryScenario_Map pcsm on ps.PST_PostalScenario_ID = pcsm.PST_PostalScenario_ID
	join PST_PostalCategoryRate_Map pcrm on pcsm.PST_PostalCategoryRate_Map_ID = pcrm.PST_PostalCategoryRate_Map_ID
	join PST_PostalWeights pw on pcrm.PST_PostalWeights_ID = pw.PST_PostalWeights_ID
where pw.VND_Vendor_ID = @VND_Vendor_ID and ps.Description = @Description and ps.PST_PostalMailerType_ID = @PST_PostalMailerType_ID
	and ps.PST_PostalClass_ID = @PST_PostalClass_ID and ps.EffectiveDate <= @RunDate
order by ps.EffectiveDate desc
GO

GRANT  EXECUTE  ON [dbo].[PstPostalScenario_s_ByOldIDandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PST_PostalCategoryScenario_Map_d') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PST_PostalCategoryScenario_Map_d'
	DROP PROCEDURE dbo.PST_PostalCategoryScenario_Map_d
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PST_PostalCategoryScenario_Map_d') IS NOT NULL
		PRINT '***********Drop of dbo.PST_PostalCategoryScenario_Map_d FAILED.'
END
GO
PRINT 'Creating dbo.PST_PostalCategoryScenario_Map_d'
GO

create proc dbo.PST_PostalCategoryScenario_Map_d
@PST_PostalScenario_ID bigint,
@PST_PostalCategoryRate_Map_ID bigint
/*
* PARAMETERS:
* PST_PostalScenario_ID - required.
* PST_PostalCategoryRate_Map_ID - required.
*
* DESCRIPTION:
* Checks to see that the postal scenario is not being referenced by any estimates.
* Then deletes the PST_PostalCategoryScenario_Map record.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   EST_AssemDistribOptions        READ
*   EST_PostalCategoryScenario_Map DELETE
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
* 08/01/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
as

begin tran t

if exists(select 1 from EST_AssemDistribOptions where PST_PostalScenario_ID = @PST_PostalScenario_ID) begin
	rollback tran t
	raiserror('Cannot delete Postal Scenario.  It is being referenced by an estimate', 16, 1)
	return
end

delete from PST_PostalCategoryScenario_Map
where PST_PostalScenario_ID = @PST_PostalScenario_ID and PST_PostalCategoryRate_Map_ID = @PST_PostalCategoryRate_Map_ID
if (@@error <> 0) begin
	rollback tran t
	raiserror('Error deleting PST_PostalCategoryScenario_Map record.', 16, 1)
	return
end

commit tran t
GO

GRANT  EXECUTE  ON [dbo].[PST_PostalCategoryScenario_Map_d]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PST_PostalScenario_d') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PST_PostalScenario_d'
	DROP PROCEDURE dbo.PST_PostalScenario_d
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PST_PostalScenario_d') IS NOT NULL
		PRINT '***********Drop of dbo.PST_PostalScenario_d FAILED.'
END
GO
PRINT 'Creating dbo.PST_PostalScenario_d'
GO

create proc dbo.PST_PostalScenario_d
@PST_PostalScenario_ID bigint/*
* PARAMETERS:
* PST_PostalScenario_ID - required.
*
* DESCRIPTION:
* Checks to see that the postal scenario is not being referenced.
* Then deletes the PST_PostalScenario record.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   EST_AssemDistribOptions        READ
*   PST_PostalCategoryScenario_Map READ
*   PST_PostalScenario             DELETE
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
* 08/01/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
as

begin tran t

if exists(select 1 from EST_AssemDistribOptions where PST_PostalScenario_ID = @PST_PostalScenario_ID) begin
	rollback tran t
	raiserror('Cannot delete Postal Scenario.  It is being referenced by an estimate', 16, 1)
	return
end

if exists(select 1 from PST_PostalCategoryScenario_Map where PST_PostalScenario_ID = @PST_PostalScenario_ID) begin
	rollback tran t
	raiserror('Cannot delete Postal Scenario.  It is being referenced by a PST_PostalCategoryScenario_Map record.', 16, 1)
	return
end

delete from PST_PostalScenario
where PST_PostalScenario_ID = @PST_PostalScenario_ID
if (@@error <> 0) begin
	rollback tran t
	raiserror('Error deleting PST_PostalScenario record.', 16, 1)
	return
end

commit tran t
GO

GRANT  EXECUTE  ON [dbo].[PST_PostalScenario_d]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubDayofWeekQuantity_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubDayofWeekQuantity_i'
	DROP PROCEDURE dbo.PubDayofWeekQuantity_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubDayofWeekQuantity_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubDayofWeekQuantity_i FAILED.'
END
GO
PRINT 'Creating dbo.PubDayofWeekQuantity_i'
GO

create proc dbo.PubDayofWeekQuantity_i
/*
* PARAMETERS:
* PUB_DayofWeekQuantity_ID - Output.  If a value is passed in it is ignored and the value is set to the ID of the new record.
*
*
* DESCRIPTION:
*		Inserts a record into the PUB_DayofWeekQuantity table.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_DayofWeekQuantity
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
* 05/25/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PUB_DayofWeekQuantity_ID bigint output,
@PUB_PubQuantity_ID bigint,
@PUB_PubQuantityType_ID bigint,
@InsertDOW int,
@Quantity int,
@CreatedBy int
as

insert into PUB_DayofWeekQuantity(PUB_PubQuantity_ID, PUB_PubQuantityType_ID, Quantity, InsertDOW, CreatedBy, CreatedDate)
values(@PUB_PubQuantity_ID, @PUB_PubQuantityType_ID, @Quantity, @InsertDOW, @CreatedBy, getdate())
set @PUB_DayofWeekQuantity_ID = @@identity


GO

GRANT  EXECUTE  ON [dbo].[PubDayofWeekQuantity_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubDayofWeekQuantity_s_ByRateMapQtyTypeDOW') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubDayofWeekQuantity_s_ByRateMapQtyTypeDOW'
	DROP PROCEDURE dbo.PubDayofWeekQuantity_s_ByRateMapQtyTypeDOW
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubDayofWeekQuantity_s_ByRateMapQtyTypeDOW') IS NOT NULL
		PRINT '***********Drop of dbo.PubDayofWeekQuantity_s_ByRateMapQtyTypeDOW FAILED.'
END
GO
PRINT 'Creating dbo.PubDayofWeekQuantity_s_ByRateMapQtyTypeDOW'
GO

CREATE PROC dbo.PubDayofWeekQuantity_s_ByRateMapQtyTypeDOW
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns distribution quantity for a publoc (@PubRateMap_ID),
*			quantity type (@QuantityType), 
*			and insert day of week (@InsertDOW).
*
* TABLES:
*		Table Name					Access
*		==========					======
*		pub_pubquantity             READ
*		pub_dayofweekquantity       READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
*	Date            Who             Comments
*	-------------   --------        -------------------------------------------------
*	09/12/2007      JRH             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PubRateMap_ID	bigint,
@QuantityType	int,
@InsertDOW	int

AS

SELECT
	pq.pub_pubrate_map_id
	, dowq.pub_pubquantitytype_id
	, dowq.insertdow
	, dowq.pub_dayofweekquantity_id
	, dowq.pub_pubquantity_id
	, dowq.quantity
FROM
dbo.pub_pubquantity pq (nolock)
	INNER JOIN dbo.pub_dayofweekquantity dowq (nolock)
		ON pq.pub_pubquantity_id = dowq.pub_pubquantity_id
		AND @QuantityType = dowq.pub_pubquantitytype_id
		AND @InsertDOW = dowq.insertdow

WHERE
	pq.pub_pubrate_map_id = @PubRateMap_ID
GO

GRANT  EXECUTE  ON [dbo].[PubDayofWeekQuantity_s_ByRateMapQtyTypeDOW]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.PubDayofWeekRates_d') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubDayofWeekRates_d'
	DROP PROCEDURE dbo.PubDayofWeekRates_d
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubDayofWeekRates_d') IS NOT NULL
		PRINT '***********Drop of dbo.PubDayofWeekRates_d FAILED.'
END
GO
PRINT 'Creating dbo.PubDayofWeekRates_d'
GO

CREATE PROCEDURE dbo.PubDayofWeekRates_d
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Deletes a pub_dayofweekrates record
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_DayofWeekRates  Delete
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/02/2007      BJS             Initial Creation 
*
*/
@PUB_DayofWeekRates_ID bigint
as

delete from PUB_DayofWeekRates
where PUB_DayofWeekRates_ID = @PUB_DayofWeekRates_ID
GO

GRANT  EXECUTE  ON [dbo].[PubDayofWeekRates_d]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubDayofWeekRates_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubDayofWeekRates_i'
	DROP PROCEDURE dbo.PubDayofWeekRates_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubDayofWeekRates_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubDayofWeekRates_i FAILED.'
END
GO
PRINT 'Creating dbo.PubDayofWeekRates_i'
GO

create proc dbo.PubDayofWeekRates_i
/*
* PARAMETERS:
* PUB_DayofWeekRates_ID - Output.  If a value is passed in it is ignored and the value is set to the ID of the new record.
*
*
* DESCRIPTION:
*		Inserts a record into the PUB_DayofWeekRates table.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_DayofWeekRates
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
* 05/25/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PUB_DayofWeekRates_ID bigint output,
@PUB_DayofWeekRateTypes_ID bigint,
@Rate money,
@InsertDOW int,
@CreatedBy int
as

insert into PUB_DayofWeekRates(PUB_DayofWeekRateTypes_ID, Rate, InsertDOW, CreatedBy, CreatedDate)
values(@PUB_DayofWeekRateTypes_ID, @Rate, @InsertDOW, @CreatedBy, getdate())
set @PUB_DayofWeekRates_ID = @@identity


GO

GRANT  EXECUTE  ON [dbo].[PubDayofWeekRates_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubDayofWeekRateTypes_d') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubDayofWeekRateTypes_d'
	DROP PROCEDURE dbo.PubDayofWeekRateTypes_d
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubDayofWeekRateTypes_d') IS NOT NULL
		PRINT '***********Drop of dbo.PubDayofWeekRateTypes_d FAILED.'
END
GO
PRINT 'Creating dbo.PubDayofWeekRateTypes_d'
GO

CREATE PROCEDURE dbo.PubDayofWeekRateTypes_d
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Deletes a pub_dayofweekratetypes record
*
* TABLES:
*   Table Name              Access
*   ==========              ======
*   PUB_DayofWeekRateTypes  DELETE
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/02/2007      BJS             Initial Creation 
*
*/
@PUB_DayofWeekRateTypes_ID bigint
as

delete from PUB_DayofWeekRateTypes
where PUB_DayofWeekRateTypes_ID = @PUB_DayofWeekRateTypes_ID
GO

GRANT  EXECUTE  ON [dbo].[PubDayofWeekRateTypes_d]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubDayofWeekRateTypes_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubDayofWeekRateTypes_i'
	DROP PROCEDURE dbo.PubDayofWeekRateTypes_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubDayofWeekRateTypes_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubDayofWeekRateTypes_i FAILED.'
END
GO
PRINT 'Creating dbo.PubDayofWeekRateTypes_i'
GO

create proc dbo.PubDayofWeekRateTypes_i
/*
* PARAMETERS:
* PUB_DayofWeekRateTypes_ID - Output.  If a value is passed in it is ignored and the value is set to the ID of the new record.
*
*
* DESCRIPTION:
*		Inserts a record into the PUB_DayofWeekRateTypes table.
*
*
* TABLES:
*   Table Name              Access
*   ==========              ======
*   PUB_DayofWeekRateTypes  INSERT
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
* 05/25/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PUB_DayofWeekRateTypes_ID bigint output,
@RateTypeDescription decimal,
@PUB_PubRate_ID bigint,
@CreatedBy varchar(50)
as

insert into PUB_DayofWeekRateTypes(RateTypeDescription, PUB_PubRate_ID, CreatedBy, CreatedDate)
values(@RateTypeDescription, @PUB_PubRate_ID, @CreatedBy, getdate())
set @PUB_DayofWeekRateTypes_ID = @@identity


GO

GRANT  EXECUTE  ON [dbo].[PubDayofWeekRateTypes_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubGroupInsertScenarioMap_d') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubGroupInsertScenarioMap_d'
	DROP PROCEDURE dbo.PubGroupInsertScenarioMap_d
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubGroupInsertScenarioMap_d') IS NOT NULL
		PRINT '***********Drop of dbo.PubGroupInsertScenarioMap_d FAILED.'
END
GO
PRINT 'Creating dbo.PubGroupInsertScenarioMap_d'
GO

create PROCEDURE dbo.PubGroupInsertScenarioMap_d
/*
* PARAMETERS:
*	PUB_InsertScenario_ID
* PUBGroupDescription

*
* DESCRIPTION:
* Deletes a new map between an insert scenario and a pub group description.
* If any estimates reference the Insert Scenario.  This will break that link.
*
* TABLES:
*   Table Name                       Access
*   ==========                       ======
*   EST_Package                      READ/UPDATE
*   PUB_InsertScenario               READ
*   PUB_GroupInsertScenario_Map      WRITE
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/29/2007      BJS             Initial Creation 
* 09/20/2007      JRH             Changed delete of EST_EstimateInsertScenario_Map 
*                                      to update of EST_Package
*
*/
@PUB_InsertScenario_ID bigint,
@PUBGroupDescription varchar(50)
as

begin tran t

UPDATE 	dbo.EST_Package
SET	pub_insertscenario_id = null
WHERE	pub_insertscenario_id = @PUB_InsertScenario_ID

if (@@error <> 0) begin
	rollback tran t
	raiserror('Error removing Insert Scenario Link From Estimate Package record.', 16, 1)
	return
end


delete from PUB_GroupInsertScenario_Map
where PUB_InsertScenario_ID = @PUB_InsertScenario_ID and PUBGroupDescription = @PUBGroupDescription
if (@@error <> 0) begin
	rollback tran t
	raiserror('Error deleting PUB_GroupInsertScenario_Map record.', 16, 1)
	return
end

commit tran t
GO

GRANT  EXECUTE  ON [dbo].[PubGroupInsertScenarioMap_d]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubGroupInsertScenarioMap_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubGroupInsertScenarioMap_i'
	DROP PROCEDURE dbo.PubGroupInsertScenarioMap_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubGroupInsertScenarioMap_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubGroupInsertScenarioMap_i FAILED.'
END
GO
PRINT 'Creating dbo.PubGroupInsertScenarioMap_i'
GO

create PROCEDURE dbo.PubGroupInsertScenarioMap_i
/*
* PARAMETERS:
*	PUB_InsertScenario_ID
* PUBGroupDescription
* CreatedBy

*
* DESCRIPTION:
* Creates a new map between an insert scenario and a pub group description.
* Check to see that a pub group with the specified description exists.
* If any estimates reference the Insert Scenario.  This will break that link.
*
* TABLES:
*   Table Name                       Access
*   ==========                       ======
*   EST_Package                      READ/UPDATE
*   PUB_InsertScenario               READ
*   PUB_PubGroup                     READ
*   PUB_GroupInsertScenario_Map      WRITE
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/29/2007      BJS             Initial Creation 
* 09/20/2007      JRH             Changed delete of EST_EstimateInsertScenario_Map 
*                                      to update of EST_Package
*
*/
@PUB_InsertScenario_ID bigint,
@PUBGroupDescription varchar(35),
@CreatedBy varchar(50)
as

begin tran t

if not exists(select 1 from PUB_PubGroup where Description = @PUBGroupDescription) begin
	rollback tran t
	raiserror('Insert Scenario Map cannot be created.  The specified Pub Group Description cannot be found.', 16, 1)
	return
end

UPDATE 	dbo.EST_Package
SET	pub_insertscenario_id = null
WHERE	pub_insertscenario_id = @PUB_InsertScenario_ID

if (@@error <> 0) begin
	rollback tran t
	raiserror('Error removing Estimate Insert Scenario Map record.', 16, 1)
	return
end

insert into PUB_GroupInsertScenario_Map(PUB_InsertScenario_ID, PUBGroupDescription, CreatedBy, CreatedDate)
values(@PUB_InsertScenario_ID, @PUBGroupDescription, @CreatedBy, getdate())
if (@@error <> 0) begin
	rollback tran t
	raiserror('Error creating PUB_GroupInsertScenario_Map record.', 16, 1)
	return
end

commit tran t
GO

GRANT  EXECUTE  ON [dbo].[PubGroupInsertScenarioMap_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubGroupInsertScenarioMap_s_ScenarioID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubGroupInsertScenarioMap_s_ScenarioID'
	DROP PROCEDURE dbo.PubGroupInsertScenarioMap_s_ScenarioID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubGroupInsertScenarioMap_s_ScenarioID') IS NOT NULL
		PRINT '***********Drop of dbo.PubGroupInsertScenarioMap_s_ScenarioID FAILED.'
END
GO
PRINT 'Creating dbo.PubGroupInsertScenarioMap_s_ScenarioID'
GO

CREATE PROC dbo.PubGroupInsertScenarioMap_s_ScenarioID
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns list of pubgroup descriptions mapped 
*			to a given scenario ID.
*
* TABLES:
*		Table Name                          Access
*		==========                          ======
*		pub_groupinsertscenario_map			READ
*
* PROCEDURES CALLED:
*		Procedure Name
*		==============
*		none
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
*	Date            Who             Comments
*	-------------   --------        -------------------------------------------------
*	09/17/2007      JRH             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@ScenarioID bigint

AS

SELECT 
	pubgroupdescription
	, pub_insertscenario_id
	, createdby
	, createddate
	, modifiedby
	, modifieddate
FROM
	dbo.pub_groupinsertscenario_map (nolock)
WHERE
	pub_insertscenario_id = @ScenarioID
GO

GRANT  EXECUTE  ON [dbo].[PubGroupInsertScenarioMap_s_ScenarioID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.PubGroup_s') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubGroup_s'
	DROP PROCEDURE dbo.PubGroup_s
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubGroup_s') IS NOT NULL
		PRINT '***********Drop of dbo.PubGroup_s FAILED.'
END
GO
PRINT 'Creating dbo.PubGroup_s'
GO

create proc dbo.PubGroup_s
/*
* PARAMETERS:
* none
*
* DESCRIPTION:
*		Returns all publication groups that are not for a specific package (customgroupforpackage = 0).
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubGroup        READ
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
* 06/22/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
as
select * from PUB_PubGroup
where CustomGroupForPackage = 0
order by sortorder
GO

GRANT  EXECUTE  ON [dbo].[PubGroup_s]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubGroup_s_ByDescriptionandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubGroup_s_ByDescriptionandRunDate'
	DROP PROCEDURE dbo.PubGroup_s_ByDescriptionandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubGroup_s_ByDescriptionandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PubGroup_s_ByDescriptionandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PubGroup_s_ByDescriptionandRunDate'
GO

create proc dbo.PubGroup_s_ByDescriptionandRunDate
/*
* PARAMETERS:
* Description
* RunDate
*
* DESCRIPTION:
*		Returns the Pub Group with a matching Description on the specified RunDate (customgroupforpackage = 0).
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubGroup        READ
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
* 09/04/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Description varchar(35),
@RunDate datetime
as
select top 1 * from PUB_PubGroup
where Description = @Description and EffectiveDate <= @RunDate and CustomGroupForPackage = 0
order by EffectiveDate desc
GO

GRANT  EXECUTE  ON [dbo].[PubGroup_s_ByDescriptionandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubGroup_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubGroup_s_ByEstimateID'
	DROP PROCEDURE dbo.PubGroup_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubGroup_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.PubGroup_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.PubGroup_s_ByEstimateID'
GO

create proc dbo.PubGroup_s_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID - required.
*
* DESCRIPTION:
*		Returns the publication group specified.
*
*
* TABLES:
*	Table Name		Access
*	==========		======
*	EST_Package		READ
*	PUB_PubGroup		READ
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
* Date          Who		Comments
* ----------	--------	-------------------------------------------------
* 09/10/2007	JRH		Initial Creation
* 09/11/2007	JRH		Add grp.pub_pubgroup_id
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint
AS

SELECT
	grp.[pub_pubgroup_id]
	, grp.[description]
	, grp.[comments]
	, grp.[active]
	, grp.[effectivedate]
	, grp.[sortorder]
	, grp.[customgroupforpackage]
	, grp.[createdby]
	, grp.[createddate]
	, grp.[modifiedby]
	, grp.[modifieddate] 
FROM
	dbo.est_package p (nolock)
	INNER JOIN dbo.pub_pubgroup grp (nolock)
		ON p.pub_pubgroup_id = grp.pub_pubgroup_id
WHERE
	p.est_estimate_id = @EST_Estimate_ID
GO

GRANT  EXECUTE  ON [dbo].[PubGroup_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubGroup_s_ByPubGroupID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubGroup_s_ByPubGroupID'
	DROP PROCEDURE dbo.PubGroup_s_ByPubGroupID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubGroup_s_ByPubGroupID') IS NOT NULL
		PRINT '***********Drop of dbo.PubGroup_s_ByPubGroupID FAILED.'
END
GO
PRINT 'Creating dbo.PubGroup_s_ByPubGroupID'
GO

create proc dbo.PubGroup_s_ByPubGroupID
/*
* PARAMETERS:
* PUB_PubGroup_ID - required.
*
* DESCRIPTION:
*		Returns the publication group specified.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubGroup        READ
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
* 09/04/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PUB_PubGroup_ID bigint
as
select * from PUB_PubGroup
where PUB_PubGroup_ID = @PUB_PubGroup_ID
order by sortorder
GO

GRANT  EXECUTE  ON [dbo].[PubGroup_s_ByPubGroupID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubGroup_s_DistinctDescription') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubGroup_s_DistinctDescription'
	DROP PROCEDURE dbo.PubGroup_s_DistinctDescription
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubGroup_s_DistinctDescription') IS NOT NULL
		PRINT '***********Drop of dbo.PubGroup_s_DistinctDescription FAILED.'
END
GO
PRINT 'Creating dbo.PubGroup_s_DistinctDescription'
GO

create proc dbo.PubGroup_s_DistinctDescription
/*
* PARAMETERS:
* none
*
* DESCRIPTION:
*		Returns distinct publication group descriptions that are not for a specific package (customgroupforpackage = 0).
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubGroup
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
* 06/22/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
as
select description, max(sortorder) sortorder
from PUB_PubGroup
where CustomGroupForPackage = 0
group by description
order by description
GO

GRANT  EXECUTE  ON [dbo].[PubGroup_s_DistinctDescription]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubInsertDiscounts_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubInsertDiscounts_i'
	DROP PROCEDURE dbo.PubInsertDiscounts_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubInsertDiscounts_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubInsertDiscounts_i FAILED.'
END
GO
PRINT 'Creating dbo.PubInsertDiscounts_i'
GO

create proc dbo.PubInsertDiscounts_i
/*
* PARAMETERS:
*
*
* DESCRIPTION:
*   Inserts a record into the PUB_InsertDiscounts table.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_InsertDiscounts INSERT
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
* 05/25/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PUB_PubRate_ID bigint,
@PUB_InsertDiscount_ID bigint,
@Insert int,
@Discount decimal,
@CreatedBy varchar(50)
as

insert into PUB_InsertDiscounts(PUB_PubRate_ID, PUB_InsertDiscount_ID, [Insert], Discount, CreatedBy, CreatedDate)
values(@PUB_PubRate_ID, @PUB_InsertDiscount_ID, @Insert, @Discount, @CreatedBy, getdate())


GO

GRANT  EXECUTE  ON [dbo].[PubInsertDiscounts_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubInsertScenario_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubInsertScenario_i'
	DROP PROCEDURE dbo.PubInsertScenario_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubInsertScenario_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubInsertScenario_i FAILED.'
END
GO
PRINT 'Creating dbo.PubInsertScenario_i'
GO

create proc dbo.PubInsertScenario_i
/*
* PARAMETERS:
* PUB_InsertScenario_ID - Output.  If a value is passed in it is ignored and the value is set to the ID of the new record.
*
* DESCRIPTION:
*   Inserts a record into the PUB_InsertScenario table.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_InsertScenario  INSERT
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
* 05/25/2007      BJS             Initial Creation
* 06/29/2007      BJS             Added error checking to ensure unique description. 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PUB_InsertScenario_ID bigint output,
@Description varchar(35),
@Comments varchar(255),
@Active bit,
@CreatedBy varchar(50)
as

if (@Description = '') begin
	raiserror('An Insert Scenario must have a description.', 16, 1)
	return
end

begin tran t
	if exists(select 1 from PUB_InsertScenario where Description = @Description) begin
		rollback tran t
		raiserror('An Insert Scenario already exists in the database with the same description.', 16, 1)
		return
	end

	insert into PUB_InsertScenario(Description, Comments, Active, CreatedBy, CreatedDate)
	values(@Description, @Comments, @Active, @CreatedBy, getdate())
	set @PUB_InsertScenario_ID = @@identity
	if (@@error <> 0) begin
		rollback tran t
		raiserror('An error occurred while creating the Insert Scenario record.', 16, 1)
		return
	end

commit tran t


GO

GRANT  EXECUTE  ON [dbo].[PubInsertScenario_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubInsertScenario_s_ScenarioID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubInsertScenario_s_ScenarioID'
	DROP PROCEDURE dbo.PubInsertScenario_s_ScenarioID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubInsertScenario_s_ScenarioID') IS NOT NULL
		PRINT '***********Drop of dbo.PubInsertScenario_s_ScenarioID FAILED.'
END
GO
PRINT 'Creating dbo.PubInsertScenario_s_ScenarioID'
GO

CREATE PROC dbo.PubInsertScenario_s_ScenarioID
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns scenario detail given a specified scenario ID.
*
* TABLES:
*		Table Name					Access
*		==========					======
*		pub_insertscenario          READ
*
* PROCEDURES CALLED:
*		Procedure Name
*		==============
*		none
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
*	Date            Who             Comments
*	-------------   --------        -------------------------------------------------
*	09/17/2007      JRH             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@ScenarioID bigint

AS

SELECT 
	pub_insertscenario_id
	, [description]
	, comments
	, active
	, createdby
	, createddate
	, modifiedby
	, modifieddate
FROM
	dbo.pub_insertscenario (nolock)
WHERE
	pub_insertscenario_id = @ScenarioID
GO


GRANT  EXECUTE  ON [dbo].[PubInsertScenario_s_ScenarioID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.PubIssueDate_s_ByPubRateMapInsertDateAMPM') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubIssueDate_s_ByPubRateMapInsertDateAMPM'
	DROP PROCEDURE dbo.PubIssueDate_s_ByPubRateMapInsertDateAMPM
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubIssueDate_s_ByPubRateMapInsertDateAMPM') IS NOT NULL
		PRINT '***********Drop of dbo.PubIssueDate_s_ByPubRateMapInsertDateAMPM FAILED.'
END
GO
PRINT 'Creating dbo.PubIssueDate_s_ByPubRateMapInsertDateAMPM'
GO

create PROCEDURE dbo.PubIssueDate_s_ByPubRateMapInsertDateAMPM
/*
* PARAMETERS:
*	none
*
* DESCRIPTION:
*	Retrieves a list of effective dates and the active status for a pub location.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubRate_Map
*   PUB_PubRate_Map_Activate
*
* FUNCTIONS CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date		Who	Comments
* ----------	---	-------------------------------------------------
* 09/13/2007	JRH	Initial Creation 
* 10/19/2007	JRH	@InsertDatePM should be previous date
*
*/
@pub_pubrate_map_id	bigint,
@InsertDate		datetime,
@AMPM			bit

AS

DECLARE @InsertDatePM datetime
SELECT @InsertDatePM = dateadd(day, -1, @InsertDate)

SELECT
	pub_id
	, publoc_id
	, issuedate = dbo.CalcIssueDate(@InsertDate, @AMPM, AM_edition, AM_offset, PM_edition, PM_offset)
	, issuedow = datepart(w, dbo.CalcIssueDate(@InsertDate, @AMPM, AM_edition, AM_offset, PM_edition, PM_offset))
FROM
	(SELECT 
		pl.pub_id
		, pl.publoc_id
		, AM_edition = 
			CASE datepart(w, @InsertDate)
				WHEN 1 THEN sun_edtn_cd
				WHEN 2 THEN mon_edtn_cd
				WHEN 3 THEN tue_edtn_cd
				WHEN 4 THEN wed_edtn_cd
				WHEN 5 THEN thu_edtn_cd
				WHEN 6 THEN fri_edtn_cd
				WHEN 7 THEN sat_edtn_cd
			END
		, AM_offset = 
			CASE datepart(w, @InsertDate)
				WHEN 1 THEN no_sun_edtn_nbr
				WHEN 2 THEN no_mon_edtn_nbr
				WHEN 3 THEN no_tue_edtn_nbr
				WHEN 4 THEN no_wed_edtn_nbr
				WHEN 5 THEN no_thu_edtn_nbr
				WHEN 6 THEN no_fri_edtn_nbr
				WHEN 7 THEN no_sat_edtn_nbr
			END
		, PM_edition = 
			CASE datepart(w, @InsertDatePM)
				WHEN 1 THEN sun_edtn_cd
				WHEN 2 THEN mon_edtn_cd
				WHEN 3 THEN tue_edtn_cd
				WHEN 4 THEN wed_edtn_cd
				WHEN 5 THEN thu_edtn_cd
				WHEN 6 THEN fri_edtn_cd
				WHEN 7 THEN sat_edtn_cd
			END
		, PM_offset = 
			CASE datepart(w, @InsertDatePM)
				WHEN 1 THEN no_sun_edtn_nbr
				WHEN 2 THEN no_mon_edtn_nbr
				WHEN 3 THEN no_tue_edtn_nbr
				WHEN 4 THEN no_wed_edtn_nbr
				WHEN 5 THEN no_thu_edtn_nbr
				WHEN 6 THEN no_fri_edtn_nbr
				WHEN 7 THEN no_sat_edtn_nbr
			END
	FROM
		DBADVProd.informix.pub_loc pl (nolock)
		INNER JOIN dbo.pub_pubrate_map rm (nolock)
			ON pl.pub_id = rm.pub_id
			AND pl.publoc_id = rm.publoc_id
	WHERE
		rm.pub_pubrate_map_id = @pub_pubrate_map_id) a
GO

GRANT  EXECUTE  ON [dbo].[PubIssueDate_s_ByPubRateMapInsertDateAMPM]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubIssueInfo_s_ByPubRateMapInsertDateAMPMQtyType') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubIssueInfo_s_ByPubRateMapInsertDateAMPMQtyType'
	DROP PROCEDURE dbo.PubIssueInfo_s_ByPubRateMapInsertDateAMPMQtyType
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubIssueInfo_s_ByPubRateMapInsertDateAMPMQtyType') IS NOT NULL
		PRINT '***********Drop of dbo.PubIssueInfo_s_ByPubRateMapInsertDateAMPMQtyType FAILED.'
END
GO
PRINT 'Creating dbo.PubIssueInfo_s_ByPubRateMapInsertDateAMPMQtyType'
GO

create PROCEDURE dbo.PubIssueInfo_s_ByPubRateMapInsertDateAMPMQtyType
/*
* PARAMETERS:
*	none
*
* DESCRIPTION:
*	Retrieves a list of effective dates and the active status for a pub location.
*
* TABLES:
*	Table Name          		Access
*	==========          		======
*	DBADVProd.informix.pub_loc	read
*	PUB_PubRate_Map
*	PUB_PubRate_Map_Activate
*
* FUNCTIONS CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date		Who	Comments
* ----------	----	-------------------------------------------------
* 09/13/2007	JRH	Initial Creation 
* 11/07/2007	JRH	PM is the day before so needed to subtract one day.
*
*/
@pub_pubrate_map_id	bigint,
@pub_pubquantitytype_id	int,
@InsertDate		datetime,
@AMPM			bit

AS

DECLARE @InsertDatePM datetime
SELECT @InsertDatePM = dateadd(day, -1, @InsertDate)

SELECT
	pub_id
	, pub_nm
	, publoc_id
	, publoc_nm
	, issuedate
	, issuedow = datepart(w, issuedate)
	, quantity = ISNULL(dbo.PubRateMapInsertQuantityByInsertDate(issuedate, @pub_pubquantitytype_id, @pub_pubrate_map_id), 0)
FROM
	(SELECT
		pub_id
		, pub_nm
		, publoc_id
		, publoc_nm
		, issuedate = dbo.CalcIssueDate(@InsertDate, @AMPM, AM_edition, AM_offset, PM_edition, PM_offset)
	FROM
		(SELECT 
			pl.pub_id
			, p.pub_nm
			, pl.publoc_id
			, pl.publoc_nm
			, AM_edition = 
				CASE datepart(w, @InsertDate)
					WHEN 1 THEN sun_edtn_cd
					WHEN 2 THEN mon_edtn_cd
					WHEN 3 THEN tue_edtn_cd
					WHEN 4 THEN wed_edtn_cd
					WHEN 5 THEN thu_edtn_cd
					WHEN 6 THEN fri_edtn_cd
					WHEN 7 THEN sat_edtn_cd
				END
			, AM_offset = 
				CASE datepart(w, @InsertDate)
					WHEN 1 THEN no_sun_edtn_nbr
					WHEN 2 THEN no_mon_edtn_nbr
					WHEN 3 THEN no_tue_edtn_nbr
					WHEN 4 THEN no_wed_edtn_nbr
					WHEN 5 THEN no_thu_edtn_nbr
					WHEN 6 THEN no_fri_edtn_nbr
					WHEN 7 THEN no_sat_edtn_nbr
				END
			, PM_edition = 
				CASE datepart(w, @InsertDatePM)
					WHEN 1 THEN sun_edtn_cd
					WHEN 2 THEN mon_edtn_cd
					WHEN 3 THEN tue_edtn_cd
					WHEN 4 THEN wed_edtn_cd
					WHEN 5 THEN thu_edtn_cd
					WHEN 6 THEN fri_edtn_cd
					WHEN 7 THEN sat_edtn_cd
				END
			, PM_offset = 
				CASE datepart(w, @InsertDatePM)
					WHEN 1 THEN no_sun_edtn_nbr
					WHEN 2 THEN no_mon_edtn_nbr
					WHEN 3 THEN no_tue_edtn_nbr
					WHEN 4 THEN no_wed_edtn_nbr
					WHEN 5 THEN no_thu_edtn_nbr
					WHEN 6 THEN no_fri_edtn_nbr
					WHEN 7 THEN no_sat_edtn_nbr
				END
		FROM
			DBADVProd.informix.pub p (nolock)
			INNER JOIN DBADVProd.informix.pub_loc pl (nolock)
				ON p.pub_id = pl.pub_id
			INNER JOIN dbo.pub_pubrate_map rm (nolock)
				ON pl.pub_id = rm.pub_id
				AND pl.publoc_id = rm.publoc_id
		WHERE
			rm.pub_pubrate_map_id = @pub_pubrate_map_id) a) b
GO

GRANT  EXECUTE  ON [dbo].[PubIssueInfo_s_ByPubRateMapInsertDateAMPMQtyType]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubLoc_s') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubLoc_s'
	DROP PROCEDURE dbo.PubLoc_s
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubLoc_s') IS NOT NULL
		PRINT '***********Drop of dbo.PubLoc_s FAILED.'
END
GO
PRINT 'Creating dbo.PubLoc_s'
GO

create PROCEDURE dbo.PubLoc_s
/*
* PARAMETERS:
*
* DESCRIPTION:
*	Retrieves a list of all publications.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   ADMINSYSTEM.pub
*   ADMINSYSTEM.pub_loc
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/26/2007      BJS             Initial Creation 
* 06/29/2007      BJS             Changed to return ALL pub locations.  Filtering is on front-end
* 07/25/2007      BJS             Modified reference to admin db.  No longer uses linked server
* 11/19/2007      JRH             Filter out location "99" from all pubs.  This is used for the
*                                 the old spreadsheets and means all locations.
*
*/
as
select p.Pub_ID, l.PubLoc_ID
from DBADVPROD.informix.pub p
	join DBADVPROD.informix.pub_loc l on p.Pub_ID = l.Pub_ID
where p.Pub_Type_CD = 'N'
	and l.PubLoc_ID <> 99
GO

GRANT  EXECUTE  ON [dbo].[PubLoc_s]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubPubGroupMap_d') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubPubGroupMap_d'
	DROP PROCEDURE dbo.PubPubGroupMap_d
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubPubGroupMap_d') IS NOT NULL
		PRINT '***********Drop of dbo.PubPubGroupMap_d FAILED.'
END
GO
PRINT 'Creating dbo.PubPubGroupMap_d'
GO

CREATE PROCEDURE dbo.PubPubGroupMap_d
/*
* PARAMETERS:
*	PUB_PubRate_Map_ID
*   PUB_PubGroup_ID
*
* DESCRIPTION:
*	Removes the pub location from the specified pub group.
*
* TABLES:
*   Table Name                 Access
*   ==========                 ======
*   EST_Package                READ
*   EST_PubIssueDates          DELETE
*   PUB_PubPubGroup_Map        DELETE
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/08/2007      BJS             Initial Creation
*
*/
@PUB_PubRate_Map_ID bigint,
@PUB_PubGroup_ID bigint
as

/* Delete the record if it exists */
if exists(select 1 from PUB_PubPubGroup_Map where PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID and PUB_PubGroup_ID = @PUB_PubGroup_ID) begin
	delete pid from EST_Package p join EST_PubIssueDates pid on p.EST_Estimate_ID = pid.EST_Estimate_ID
		join PUB_PubPubGroup_Map ppgm on p.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID and pid.PUB_PubRate_Map_ID = ppgm.PUB_PubRate_Map_ID
	where ppgm.PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID and ppgm.PUB_PubGroup_ID = @PUB_PubGroup_ID
	if (@@error <> 0) begin
		raiserror('Error deleting EST_PubIssueDate record(s).', 16, 1)
		return
	end

	delete from PUB_PubPubGroup_Map
	where PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID and PUB_PubGroup_ID = @PUB_PubGroup_ID
	if (@@error <> 0) begin
		raiserror('Error deleting PUB_PubPubGroup_Map record.', 16, 1)
		return
	end
end
GO

GRANT  EXECUTE  ON [dbo].[PubPubGroupMap_d]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubPubGroupMap_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubPubGroupMap_i'
	DROP PROCEDURE dbo.PubPubGroupMap_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubPubGroupMap_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubPubGroupMap_i FAILED.'
END
GO
PRINT 'Creating dbo.PubPubGroupMap_i'
GO

CREATE PROCEDURE dbo.PubPubGroupMap_i
/*
* PARAMETERS:
*	PUB_PubRate_Map_ID
*   PUB_PubGroup_ID
*   CreatedBy
*
* DESCRIPTION:
*	Adds pub locations to the specified pub group.
*
* TABLES:
*   Table Name                 Access
*   ==========                 ======
*   DBADVProd.informix.pub     READ
*   DBADVProd.informix.pub_loc READ
*   PUB_PubRate_Map            READ
*   EST_Package                READ
*   EST_Estimate               READ
*   EST_AssemDistribOptions    READ
*   EST_PubIssueDates          INSERT
*   PUB_PubPubGroup_Map        INSERT
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/08/2007      BJS             Initial Creation
*
*/
@PUB_PubRate_Map_ID bigint,
@PUB_PubGroup_ID bigint,
@CreatedBy varchar(50)
as

/* Add the record */
if not exists(select 1 from PUB_PubPubGroup_Map where PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID and PUB_PubGroup_ID = @PUB_PubGroup_ID) begin
	/* Adding the pubratemap to the pub group cannot cause an overlap in the distribution mappings */
	if exists(
		select 1 from EST_Package source_p join EST_Package other_p on source_p.EST_Estimate_ID = other_p.EST_Estimate_ID and source_p.EST_Package_ID <> other_p.EST_Package_ID
			join PUB_PubPubGroup_Map other_ppgm on other_p.PUB_PubGroup_ID = other_ppgm.PUB_PubGroup_ID
		where source_p.PUB_PubGroup_ID = @PUB_PubGroup_ID and other_ppgm.PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID) begin

		raiserror('Cannot add pub-loc to group.  It would cause a conflict in distribution mapping record(s).', 16, 1)
		return
	end

	insert into PUB_PubPubGroup_Map(PUB_PubRate_Map_ID, PUB_PubGroup_ID, CreatedBy, CreatedDate)
	values(@PUB_PubRate_Map_ID, @PUB_PubGroup_ID, @CreatedBy, getdate())
	if (@@error <> 0) begin
		raiserror('Error inserting pub_pubpubgroup_map record.', 16, 1)
		return
	end

	/* Code copied from PubIssueInfo_s_ByPubRateMapInsertDateAMPMQtyType */
	/* Create corresponding EST_PubIssueDate records */
	insert into EST_PubIssueDates(EST_Estimate_ID, PUB_PubRate_Map_ID, [override], IssueDOW, IssueDate, CreatedBy, CreatedDate)
	SELECT
		EST_Estimate_ID,
		PUB_PubRate_Map_ID,
		0,
		datepart(w, dbo.CalcIssueDate(InsertDate, InsertTime, AM_edition, AM_offset, PM_edition, PM_offset)),
		dbo.CalcIssueDate(InsertDate, InsertTime, AM_edition, AM_offset, PM_edition, PM_offset),
		@CreatedBy,
		getdate()
	FROM
		(SELECT e.EST_Estimate_ID
			, rm.PUB_PubRate_Map_ID
			, InsertDate =
				case
					when datepart(dw, e.RunDate) > ad.InsertDOW then dateadd(d, -1 * (datepart(dw, e.RunDate) - ad.InsertDOW), e.RunDate)
					when datepart(dw, e.RunDate) < ad.InsertDOW then dateadd(d, ad.InsertDOW - datepart(dw, e.RunDate) - 7, e.RunDate)
					else e.RunDate
				end
			, ad.InsertTime
			, pl.pub_id
			, p.pub_nm
			, pl.publoc_id
			, pl.publoc_nm
			, AM_edition = 
				CASE ad.InsertDOW
					WHEN 1 THEN sun_edtn_cd
					WHEN 2 THEN mon_edtn_cd
					WHEN 3 THEN tue_edtn_cd
					WHEN 4 THEN wed_edtn_cd
					WHEN 5 THEN thu_edtn_cd
					WHEN 6 THEN fri_edtn_cd
					WHEN 7 THEN sat_edtn_cd
				END
			, AM_offset = 
				CASE ad.InsertDOW
					WHEN 1 THEN no_sun_edtn_nbr
					WHEN 2 THEN no_mon_edtn_nbr
					WHEN 3 THEN no_tue_edtn_nbr
					WHEN 4 THEN no_wed_edtn_nbr
					WHEN 5 THEN no_thu_edtn_nbr
					WHEN 6 THEN no_fri_edtn_nbr
					WHEN 7 THEN no_sat_edtn_nbr
				END
			, PM_edition = 
				CASE (ad.InsertDOW - 1) % 7
					WHEN 1 THEN sun_edtn_cd
					WHEN 2 THEN mon_edtn_cd
					WHEN 3 THEN tue_edtn_cd
					WHEN 4 THEN wed_edtn_cd
					WHEN 5 THEN thu_edtn_cd
					WHEN 6 THEN fri_edtn_cd
					WHEN 0 THEN sat_edtn_cd -- 7 modulus 7 is a zero
				END
			, PM_offset = 
				CASE (ad.InsertDOW - 1) % 7
					WHEN 1 THEN no_sun_edtn_nbr
					WHEN 2 THEN no_mon_edtn_nbr
					WHEN 3 THEN no_tue_edtn_nbr
					WHEN 4 THEN no_wed_edtn_nbr
					WHEN 5 THEN no_thu_edtn_nbr
					WHEN 6 THEN no_fri_edtn_nbr
					WHEN 0 THEN no_sat_edtn_nbr -- 7 modulus 7 is a zero
				END
		FROM
			DBADVProd.informix.pub p (nolock)
			INNER JOIN DBADVProd.informix.pub_loc pl (nolock)
				ON p.pub_id = pl.pub_id
			INNER JOIN dbo.pub_pubrate_map rm (nolock)
				ON pl.pub_id = rm.pub_id
				AND pl.publoc_id = rm.publoc_id
			JOIN EST_Package pkg on pkg.PUB_PubGroup_ID = @PUB_PubGroup_ID
			JOIN EST_Estimate e on pkg.EST_Estimate_ID = e.EST_Estimate_ID
			JOIN EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
		WHERE
			rm.pub_pubrate_map_id = @PUB_PubRate_Map_ID) a
	if (@@error <> 0) begin
		raiserror('Error inserting est_pubissuedate record(s).', 16, 1)
		return
	end
end
GO

GRANT  EXECUTE  ON [dbo].[PubPubGroupMap_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubPubGroupMap_s') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubPubGroupMap_s'
	DROP PROCEDURE dbo.PubPubGroupMap_s
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubPubGroupMap_s') IS NOT NULL
		PRINT '***********Drop of dbo.PubPubGroupMap_s FAILED.'
END
GO
PRINT 'Creating dbo.PubPubGroupMap_s'
GO

CREATE PROCEDURE dbo.PubPubGroupMap_s
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Returns all PUB_PubPubGroup_Map records.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubPubGroupMap
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/22/2007      BJS             Initial Creation 
*
*/
as
select *
from PUB_PubPubGroup_Map

GO

GRANT  EXECUTE  ON [dbo].[PubPubGroupMap_s]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubPubGroupMap_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubPubGroupMap_s_ByEstimateID'
	DROP PROCEDURE dbo.PubPubGroupMap_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubPubGroupMap_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.PubPubGroupMap_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.PubPubGroupMap_s_ByEstimateID'
GO

create proc dbo.PubPubGroupMap_s_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID - required.
*
* DESCRIPTION:
*		Returns the publication groups used by the estimate specified.
*
*
* TABLES:
*	Table Name		Access
*	==========		======
*	EST_Package		READ
*	PUB_PubGroup		READ
*	PUB_PubPubGroupMap	READ
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
* Date          Who		Comments
* ----------	--------	-------------------------------------------------
* 09/10/2007	JRH		Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint
AS

SELECT
	map.[pub_pubrate_map_id]
	, map.[pub_pubgroup_id]
	, map.[createdby]
	, map.[createddate]
	, map.[modifiedby]
	, map.[modifieddate] 
FROM
	dbo.est_package p (nolock)
	INNER JOIN dbo.pub_pubpubgroup_map map (nolock)
		ON p.pub_pubgroup_id = map.pub_pubgroup_id
WHERE
	p.est_estimate_id = @EST_Estimate_ID

GO

GRANT  EXECUTE  ON [dbo].[PubPubGroupMap_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubPubGroupMap_s_ByGroupID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubPubGroupMap_s_ByGroupID'
	DROP PROCEDURE dbo.PubPubGroupMap_s_ByGroupID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubPubGroupMap_s_ByGroupID') IS NOT NULL
		PRINT '***********Drop of dbo.PubPubGroupMap_s_ByGroupID FAILED.'
END
GO
PRINT 'Creating dbo.PubPubGroupMap_s_ByGroupID'
GO

create proc dbo.PubPubGroupMap_s_ByGroupID
/*
* PARAMETERS:
* EST_Estimate_ID - required.
*
* DESCRIPTION:
*		Returns the publication group specified.
*
*
* TABLES:
*	Table Name		Access
*	==========		======
*	EST_Package		READ
*	PUB_PubGroup		READ
*	PUB_PubPubGroupMap	READ
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
* Date          Who		Comments
* ----------	--------	-------------------------------------------------
* 09/10/2007	JRH		Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PUB_PubGroup_ID bigint
AS

SELECT
	map.[pub_pubrate_map_id]
	, map.[pub_pubgroup_id]
	, map.[createdby]
	, map.[createddate]
	, map.[modifiedby]
	, map.[modifieddate] 
FROM
	dbo.pub_pubpubgroup_map map (nolock)
WHERE
	map.pub_pubgroup_id = @PUB_PubGroup_ID

GO

GRANT  EXECUTE  ON [dbo].[PubPubGroupMap_s_ByGroupID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubPubGroup_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubPubGroup_i'
	DROP PROCEDURE dbo.PubPubGroup_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubPubGroup_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubPubGroup_i FAILED.'
END
GO
PRINT 'Creating dbo.PubPubGroup_i'
GO

create PROCEDURE dbo.PubPubGroup_i
/*
* PARAMETERS:
*	PUB_PubGroup_ID - The new Pub Group ID
*   Description
*   Comments
*   EffectiveDate
*   SortOrder
*   CreatedBy
*
* DESCRIPTION:
* Inserts a new record into the PUB_PubGroup table.  If any est_package records
* referenced a pub_pubgroup with the same description reference the new pub_group
* if applicable (If Run Date is on or after the pub_group effective date AND prior to the
* next effective date.)
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubGroup
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/25/2007      BJS             Initial Creation
* 06/29/2007      BJS             Modified how SortOrder is determined. 
*
*/
@PUB_PubGroup_ID bigint output,
@Description varchar(35),
@Comments varchar(255),
@Active bit,
@EffectiveDate datetime,
@CreatedBy varchar(50)
as

/*You cannot insert a record without a description*/
if (@Description = '') begin
	raiserror('A Publication Group must have a description.', 16, 1)
	return
end


/*If a pub group of the same description an effective date already exist return an error */
if exists(select 1 from PUB_PubGroup (holdlock) where Description = @Description and EffectiveDate = @EffectiveDate and CustomGroupForPackage = 0) begin
	raiserror('A PUB_PubGroup with the same description and effective date already exists.', 16, 1)
	return
end

else begin

	declare @SortOrder int, @PreviousPubGroupID bigint

	select @SortOrder = SortOrder
	from PUB_PubGroup
	where Description = @Description and CustomGroupForPackage = 0

	if (@SortOrder is null) begin
		select @SortOrder = max(SortOrder) + 1
		from PUB_PubGroup
		where CustomGroupForPackage = 0
	end

	if (@SortOrder is null)
		set @SortOrder = 0

	select top 1 @PreviousPubGroupID = PUB_PubGroup_ID
	from PUB_PubGroup
	where Description = @Description and CustomGroupForPackage = 0 and EffectiveDate < @EffectiveDate
	order by EffectiveDate desc

	/* If the PUB Group is being created as "inactive".  Make sure that no estimates will need to use it.
     * An estimate cannot reference an inactive pub group. */
	if (@Active = 0
		and exists(select 1 from EST_Estimate e join EST_Package p on e.EST_Estimate_ID = p.EST_Estimate_ID
			where p.PUB_PubGroup_ID = @PreviousPubGroupID and e.RunDate >= @EffectiveDate)) begin

		raiserror('Cannot create inactive group.  There are distribution mapping record(s) which reference it.', 16, 1)
		return
	end


	insert into PUB_PubGroup(Description, Comments, Active, EffectiveDate, SortOrder, CustomGroupForPackage, CreatedBy, CreatedDate)
	values(@Description, @Comments, @Active, @EffectiveDate, @SortOrder, 0, @CreatedBy, getdate())
	set @PUB_PubGroup_ID = @@identity
	if (@@error <> 0) begin
		raiserror('Error inserting PUB_PubGroup record.', 16, 1)
		return
	end

	/* Remove any EST_PubIssueDate records referencing pub_rate_maps that were included by the old group */
	delete pid
	from EST_PubIssueDates pid join EST_Package p on pid.EST_Estimate_ID = p.EST_Estimate_ID
		join EST_Estimate e on pid.EST_Estimate_ID = pid.EST_Estimate_ID
		join PUB_PubPubGroup_Map ppgm on p.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID
	where p.PUB_PubGroup_ID = @PreviousPubGroupID and e.RunDate >= @EffectiveDate
	if (@@error <> 0) begin
		raiserror('Error removing EST_PubIssueDates record(s) referencing the old PUB_PubGroup record.', 16, 1)
		return
	end

	/* Update any estimates referencing the Pub Group */
	update e
		set
			e.ModifiedBy = @CreatedBy,
			e.ModifiedDate = getdate()
	from EST_Estimate e join EST_Package p on e.EST_Estimate_ID = p.EST_Estimate_ID
		join PUB_PubGroup g on p.PUB_PubGroup_ID = g.PUB_PubGroup_ID
	where g.PUB_PubGroup_ID = @PreviousPubGroupID and g.CustomGroupForPackage = 0 and e.RunDate >= @EffectiveDate
	if (@@error <> 0) begin
		raiserror('Error updating EST_Estimate last modified user/date.', 16, 1)
		return
	end

	update p
		set
			p.PUB_PubGroup_ID = @PUB_PubGroup_ID,
			p.ModifiedBy = @CreatedBy,
			p.ModifiedDate = getdate()
	from EST_Estimate e join EST_Package p on e.EST_Estimate_ID = p.EST_Estimate_ID
		join PUB_PubGroup g on p.PUB_PubGroup_ID = g.PUB_PubGroup_ID
	where g.PUB_PubGroup_ID = @PreviousPubGroupID and g.CustomGroupForPackage = 0 and e.RunDate >= @EffectiveDate
	if (@@error <> 0) begin
		raiserror('Error updating EST_Packages with new PUB_PubGroup_ID.', 16, 1)
		return
	end
end
GO

GRANT  EXECUTE  ON [dbo].[PubPubGroup_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubPubGroup_s_ActiveIDs_ByRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubPubGroup_s_ActiveIDs_ByRunDate'
	DROP PROCEDURE dbo.PubPubGroup_s_ActiveIDs_ByRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubPubGroup_s_ActiveIDs_ByRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PubPubGroup_s_ActiveIDs_ByRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PubPubGroup_s_ActiveIDs_ByRunDate'
GO

CREATE PROC dbo.PubPubGroup_s_ActiveIDs_ByRunDate
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns a list of pub group ID's for the specified RunDate
*
* TABLES:
*		Table Name					Access
*		==========					======
*		pub_pubgroup					READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
*	Date		Who		Comments
*	------------- 	--------        -------------------------------------------------
*	08/30/2007	JRH		Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@RunDate datetime

AS

SELECT
	grp.pub_pubgroup_id
	, grp.[description]
	, grp.sortorder
FROM
	dbo.pub_pubgroup grp (nolock)
	INNER JOIN (
		SELECT 
			[description]
			, max(effectivedate) as effectivedate
		FROM
			dbo.pub_pubgroup
		WHERE
			effectivedate <= @RunDate
			and customgroupforpackage = 0
		GROUP BY
			description
		) effective
	ON grp.[description] = effective.[description]
		AND grp.effectivedate = effective.effectivedate
WHERE
	grp.active = 1
	and customgroupforpackage = 0
ORDER BY
	grp.sortorder
	, grp.[description]	
GO

GRANT  EXECUTE  ON [dbo].[PubPubGroup_s_ActiveIDs_ByRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.PubPubGroup_u_ByID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubPubGroup_u_ByID'
	DROP PROCEDURE dbo.PubPubGroup_u_ByID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubPubGroup_u_ByID') IS NOT NULL
		PRINT '***********Drop of dbo.PubPubGroup_u_ByID FAILED.'
END
GO
PRINT 'Creating dbo.PubPubGroup_u_ByID'
GO

create PROCEDURE dbo.PubPubGroup_u_ByID
/*
* PARAMETERS:
*	PUB_PubGroup_ID
* Description
* Comments
* Active
* ModifiedBy
*
* DESCRIPTION:
*	Updates a PUB_PubGroup record and the sort order of all other groups with the same description.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubGroup
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/25/2007      BJS             Initial Creation 
* 06/29/2007      BJS             Added SortOrder update
* 10/08/2007      BJS             Added logic to link the update with Estimates
*
*/
@PUB_PubGroup_ID bigint,
@Comments varchar(255),
@Active bit,
@SortOrder int,
@ModifiedBy varchar(50)
as

declare @pActive bit, @pEffectiveDate datetime
select @pActive = Active, @pEffectiveDate = EffectiveDate
from PUB_PubGroup
where PUB_PubGroup_ID = @PUB_PubGroup_ID

/* Identify the pub group immediately preceding the pubgroup being updated. */
declare @previous_PUB_PubGroup_ID bigint
select top 1 @previous_PUB_PubGroup_ID = old_pg.PUB_PubGroup_ID
from PUB_PubGroup pg join PUB_PubGroup old_pg on pg.Description = old_pg.Description and old_pg.EffectiveDate < pg.EffectiveDate and old_pg.CustomGroupForPackage = 0
where pg.PUB_PubGroup_ID = @PUB_PubGroup_ID
order by old_pg.EffectiveDate desc

/* The user is trying to inactivate a pub group that is currently active. */
if (@Active = 0 and @pActive = 1) begin
	/* If any packages reference the pubgroup they need to be modified to reference the prior one.
       If a prior pubgroup does not exist or is inactive return an error. */
	if ((@previous_PUB_PubGroup_ID is null
			or exists (select 1 from PUB_PubGroup where PUB_PubGroup_ID = @previous_PUB_PubGroup_ID and Active = 0))
			and exists (select 1 from EST_Package p where PUB_PubGroup_ID = @PUB_PubGroup_ID)) begin
		
		raiserror('Cannot inactivate pub group record.  It is being reference by distribution mapping record(s).', 16, 1)
		return
	end

	/* Packages linked to this pub group will need to reference the prior pub group.  If any overlaps would occur this cannot be done.
     * Return an exception */
	if exists (select 1 from EST_Package source_p join EST_Estimate e on source_p.EST_Estimate_ID = e.EST_Estimate_ID
				join EST_Package other_p on e.EST_Estimate_ID = other_p.EST_Estimate_ID and source_p.EST_Package_ID <> other_p.EST_Package_ID
				join PUB_PubPubGroup_Map other_ppgm on other_p.PUB_PubGroup_ID = other_ppgm.PUB_PubGroup_ID
				join PUB_PubPubGroup_Map previous_ppgm on other_ppgm.PUB_PubRate_Map_ID = previous_ppgm.PUB_PubRate_Map_ID
			where source_p.PUB_PubGroup_ID = @PUB_PubGroup_ID and previous_ppgm.PUB_PubGroup_ID = @previous_PUB_PubGroup_ID) begin

		raiserror('Cannot inactivate pub group record.  Conflicts would be created in distribution mapping record(s).', 16, 1)
		return
	end

	/* Remove any EST_PubIssueDate records referencing pub-locs in this group */
	delete pid
	from EST_PubIssueDates pid join EST_Package p on pid.EST_Estimate_ID = p.EST_Estimate_ID
		join PUB_PubPubGroup_Map ppgm on p.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID
	where p.PUB_PubGroup_ID = @PUB_PubGroup_ID
	if (@@error <> 0) begin
		raiserror('Error removing EST_PubIssueDates record(s) referencing the PUB_PubGroup record.', 16, 1)
		return
	end

	update e
		set
			e.ModifiedBy = @ModifiedBy,
			e.ModifiedDate = getdate()
	from EST_Estimate e join EST_Package p on e.EST_Estimate_ID = p.EST_Estimate_ID
	where p.PUB_PubGroup_ID = @PUB_PubGroup_ID
	if (@@error <> 0) begin
		raiserror('Error updating Modified user/date on Estimate record(s).', 16, 1)
		return
	end

	/* Update packages with previous group */
	update EST_Package
		set
			PUB_PubGroup_ID = @previous_PUB_PubGroup_ID,
			ModifiedBy = @ModifiedBy,
			ModifiedDate = getdate()
	where PUB_PubGroup_ID = @PUB_PubGroup_ID
	if (@@error <> 0) begin
		raiserror('Error updating package record(s).', 16, 1)
		return
	end
	
	/* Create Pub Issue Date records for the previous group */
	insert into EST_PubIssueDates(EST_Estimate_ID, PUB_PubRate_Map_ID, [override], IssueDOW, IssueDate, CreatedBy, CreatedDate)
	SELECT
		EST_Estimate_ID,
		PUB_PubRate_Map_ID,
		0,
		datepart(w, dbo.CalcIssueDate(InsertDate, InsertTime, AM_edition, AM_offset, PM_edition, PM_offset)),
		dbo.CalcIssueDate(InsertDate, InsertTime, AM_edition, AM_offset, PM_edition, PM_offset),
		@ModifiedBy,
		getdate()
	FROM
		(SELECT e.EST_Estimate_ID
			, rm.PUB_PubRate_Map_ID
			, InsertDate =
				case
					when datepart(dw, e.RunDate) > ad.InsertDOW then dateadd(d, -1 * (datepart(dw, e.RunDate) - ad.InsertDOW), e.RunDate)
					when datepart(dw, e.RunDate) < ad.InsertDOW then dateadd(d, ad.InsertDOW - datepart(dw, e.RunDate) - 7, e.RunDate)
					else e.RunDate
				end
			, ad.InsertTime
			, pl.pub_id
			, p.pub_nm
			, pl.publoc_id
			, pl.publoc_nm
			, AM_edition = 
				CASE ad.InsertDOW
					WHEN 1 THEN sun_edtn_cd
					WHEN 2 THEN mon_edtn_cd
					WHEN 3 THEN tue_edtn_cd
					WHEN 4 THEN wed_edtn_cd
					WHEN 5 THEN thu_edtn_cd
					WHEN 6 THEN fri_edtn_cd
					WHEN 7 THEN sat_edtn_cd
				END
			, AM_offset = 
				CASE ad.InsertDOW
					WHEN 1 THEN no_sun_edtn_nbr
					WHEN 2 THEN no_mon_edtn_nbr
					WHEN 3 THEN no_tue_edtn_nbr
					WHEN 4 THEN no_wed_edtn_nbr
					WHEN 5 THEN no_thu_edtn_nbr
					WHEN 6 THEN no_fri_edtn_nbr
					WHEN 7 THEN no_sat_edtn_nbr
				END
			, PM_edition = 
				CASE (ad.InsertDOW - 1) % 7
					WHEN 1 THEN sun_edtn_cd
					WHEN 2 THEN mon_edtn_cd
					WHEN 3 THEN tue_edtn_cd
					WHEN 4 THEN wed_edtn_cd
					WHEN 5 THEN thu_edtn_cd
					WHEN 6 THEN fri_edtn_cd
					WHEN 0 THEN sat_edtn_cd -- 7 modulus 7 is a zero
				END
			, PM_offset = 
				CASE (ad.InsertDOW - 1) % 7
					WHEN 1 THEN no_sun_edtn_nbr
					WHEN 2 THEN no_mon_edtn_nbr
					WHEN 3 THEN no_tue_edtn_nbr
					WHEN 4 THEN no_wed_edtn_nbr
					WHEN 5 THEN no_thu_edtn_nbr
					WHEN 6 THEN no_fri_edtn_nbr
					WHEN 0 THEN no_sat_edtn_nbr -- 7 modulus 7 is a zero
				END
		FROM
			DBADVProd.informix.pub p (nolock)
			INNER JOIN DBADVProd.informix.pub_loc pl (nolock)
				ON p.pub_id = pl.pub_id
			INNER JOIN dbo.pub_pubrate_map rm (nolock)
				ON pl.pub_id = rm.pub_id
				AND pl.publoc_id = rm.publoc_id
			JOIN PUB_PubPubGroup_Map ppgm on rm.PUB_PubRate_Map_ID = ppgm.PUB_PubRate_Map_ID
			JOIN EST_Package pkg on pkg.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID and pkg.PUB_PubGroup_ID = @PUB_PubGroup_ID
			JOIN EST_Estimate e on pkg.EST_Estimate_ID = e.EST_Estimate_ID
			JOIN EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID) a
	if (@@error <> 0) begin
		raiserror('Error inserting est_pubissuedate record(s).', 16, 1)
		return
	end
end

/* The user is trying to activate a group that is currently inactive */
else if (@Active = 1 and @pActive = 0) begin
	/* Packages referencing the previous Pub Group may need to reference this pub group.  If any overlaps would occur, return an error. */
	if exists (select 1 from EST_Package source_p join EST_Estimate e on source_p.EST_Estimate_ID = e.EST_Estimate_ID
				join EST_package other_p on e.EST_Estimate_ID = other_p.EST_Estimate_ID and source_p.EST_Package_ID <> other_p.EST_Package_ID
				join PUB_PubPubGroup_Map other_ppgm on other_p.PUB_PubGroup_ID = other_ppgm.PUB_PubGroup_ID
				join PUB_PubPubGroup_Map new_ppgm on other_ppgm.PUB_PubRate_Map_ID = new_ppgm.PUB_PubRate_Map_ID
			where source_p.PUB_PubGroup_ID = @previous_PUB_PubGroup_ID and new_ppgm.PUB_PubGroup_ID = @PUB_PubGroup_ID and e.RunDate >= @pEffectiveDate) begin

		raiserror('Cannot activate pub group record.  Conflicts would be created in distribution mapping record(s).', 16, 1)
		return
	end

	/* Remove any EST_PubIssueDate records referencing pub-locs in the prior group */
	delete pid
	from EST_PubIssueDates pid join EST_Package p on pid.EST_Estimate_ID = p.EST_Estimate_ID
		join PUB_PubPubGroup_Map ppgm on p.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID
		join EST_Estimate e on pid.EST_Estimate_ID = e.EST_Estimate_ID
	where p.PUB_PubGroup_ID = @previous_PUB_PubGroup_ID and e.RunDate >= @pEffectiveDate
	if (@@error <> 0) begin
		raiserror('Error removing EST_PubIssueDates record(s) referencing the prior PUB_PubGroup record.', 16, 1)
		return
	end
	
	/* Create Pub Issue Date records for estimates that will reference the current group */
	insert into EST_PubIssueDates(EST_Estimate_ID, PUB_PubRate_Map_ID, [override], IssueDOW, IssueDate, CreatedBy, CreatedDate)
	SELECT
		EST_Estimate_ID,
		PUB_PubRate_Map_ID,
		0,
		datepart(w, dbo.CalcIssueDate(InsertDate, InsertTime, AM_edition, AM_offset, PM_edition, PM_offset)),
		dbo.CalcIssueDate(InsertDate, InsertTime, AM_edition, AM_offset, PM_edition, PM_offset),
		@ModifiedBy,
		getdate()
	FROM
		(SELECT e.EST_Estimate_ID
			, rm.PUB_PubRate_Map_ID
			, InsertDate =
				case
					when datepart(dw, e.RunDate) > ad.InsertDOW then dateadd(d, -1 * (datepart(dw, e.RunDate) - ad.InsertDOW), e.RunDate)
					when datepart(dw, e.RunDate) < ad.InsertDOW then dateadd(d, ad.InsertDOW - datepart(dw, e.RunDate) - 7, e.RunDate)
					else e.RunDate
				end
			, ad.InsertTime
			, pl.pub_id
			, p.pub_nm
			, pl.publoc_id
			, pl.publoc_nm
			, AM_edition = 
				CASE ad.InsertDOW
					WHEN 1 THEN sun_edtn_cd
					WHEN 2 THEN mon_edtn_cd
					WHEN 3 THEN tue_edtn_cd
					WHEN 4 THEN wed_edtn_cd
					WHEN 5 THEN thu_edtn_cd
					WHEN 6 THEN fri_edtn_cd
					WHEN 7 THEN sat_edtn_cd
				END
			, AM_offset = 
				CASE ad.InsertDOW
					WHEN 1 THEN no_sun_edtn_nbr
					WHEN 2 THEN no_mon_edtn_nbr
					WHEN 3 THEN no_tue_edtn_nbr
					WHEN 4 THEN no_wed_edtn_nbr
					WHEN 5 THEN no_thu_edtn_nbr
					WHEN 6 THEN no_fri_edtn_nbr
					WHEN 7 THEN no_sat_edtn_nbr
				END
			, PM_edition = 
				CASE (ad.InsertDOW - 1) % 7
					WHEN 1 THEN sun_edtn_cd
					WHEN 2 THEN mon_edtn_cd
					WHEN 3 THEN tue_edtn_cd
					WHEN 4 THEN wed_edtn_cd
					WHEN 5 THEN thu_edtn_cd
					WHEN 6 THEN fri_edtn_cd
					WHEN 0 THEN sat_edtn_cd -- 7 modulus 7 is a zero
				END
			, PM_offset = 
				CASE (ad.InsertDOW - 1) % 7
					WHEN 1 THEN no_sun_edtn_nbr
					WHEN 2 THEN no_mon_edtn_nbr
					WHEN 3 THEN no_tue_edtn_nbr
					WHEN 4 THEN no_wed_edtn_nbr
					WHEN 5 THEN no_thu_edtn_nbr
					WHEN 6 THEN no_fri_edtn_nbr
					WHEN 0 THEN no_sat_edtn_nbr -- 7 modulus 7 is a zero
				END
		FROM
			DBADVProd.informix.pub p (nolock)
			INNER JOIN DBADVProd.informix.pub_loc pl (nolock)
				ON p.pub_id = pl.pub_id
			INNER JOIN dbo.pub_pubrate_map rm (nolock)
				ON pl.pub_id = rm.pub_id
				AND pl.publoc_id = rm.publoc_id
			JOIN PUB_PubPubGroup_Map ppgm on rm.PUB_PubRate_Map_ID = ppgm.PUB_PubRate_Map_ID
			JOIN EST_Package pkg on ppgm.PUB_PubGroup_ID = pkg.PUB_PubGroup_ID and pkg.PUB_PubGroup_ID = @previous_PUB_PubGroup_ID
			JOIN EST_Estimate e on pkg.EST_Estimate_ID = e.EST_Estimate_ID and e.RunDate >= @pEffectiveDate
			JOIN EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID) a
	if (@@error <> 0) begin
		raiserror('Error inserting est_pubissuedate record(s).', 16, 1)
		return
	end


	update e
		set
			e.ModifiedBy = @ModifiedBy,
			e.ModifiedDate = getdate()
	from EST_Estimate e join EST_Package p on e.EST_Estimate_ID = p.EST_Estimate_ID
	where p.PUB_PubGroup_ID = @previous_PUB_PubGroup_ID and e.RunDate >= @pEffectiveDate
	if (@@error <> 0) begin
		raiserror('Error updating Modified user/date on Estimate record(s).', 16, 1)
		return
	end

	/* Update packages with current group */
	update p
		set
			PUB_PubGroup_ID = @PUB_PubGroup_ID,
			ModifiedBy = @ModifiedBy,
			ModifiedDate = getdate()
	from EST_Estimate e join EST_Package p on e.EST_Estimate_ID = p.EST_Estimate_ID
	where PUB_PubGroup_ID = @previous_PUB_PubGroup_ID and e.RunDate >= @pEffectiveDate
	if (@@error <> 0) begin
		raiserror('Error updating package record(s).', 16, 1)
		return
	end
end

update PUB_PubGroup
	set
		Comments = @Comments,
		Active = @Active,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where PUB_PubGroup_ID = @PUB_PubGroup_ID
if (@@error <> 0) begin
	raiserror('Error updating PUB_PubGroup record.', 16, 1)
	return
end

update g_samedescription
	set SortOrder = @SortOrder
from PUB_PubGroup g join PUB_PubGroup g_samedescription on g.Description = g_samedescription.Description
where g.PUB_PubGroup_ID = @PUB_PubGroup_ID and g_samedescription.CustomGroupForPackage = 0
if (@@error <> 0) begin
	raiserror('Error updating PUB_PubGroup sort order.', 16, 1)
	return
end

GO

GRANT  EXECUTE  ON [dbo].[PubPubGroup_u_ByID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubQuantity_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubQuantity_i'
	DROP PROCEDURE dbo.PubQuantity_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubQuantity_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubQuantity_i FAILED.'
END
GO
PRINT 'Creating dbo.PubQuantity_i'
GO

create proc dbo.PubQuantity_i
/*
* PARAMETERS:
* PUB_PubQuantity_ID - Output.  If a value is passed in it is ignored and the value is set to the ID of the new record.
*
*
* DESCRIPTION:
*		Inserts a record into the PUB_PubQuantity table.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubQuantity
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
* 05/25/2007      BJS             Initial Creation 
* 07/05/2007      BJS             Added logic to check for duplicate effective date
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PUB_PubQuantity_ID bigint output,
@EffectiveDate datetime,
@PUB_PubRate_Map_ID bigint,
@CreatedBy varchar(50)
as

begin tran t
	if exists(select 1 from PUB_PubQuantity where PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID and EffectiveDate = @EffectiveDate) begin
		rollback tran t
		raiserror('The publication location already has quantities set on the same effective date.', 16, 1)
		return
	end

	insert into PUB_PubQuantity(EffectiveDate, PUB_PubRate_Map_ID, CreatedBy, CreatedDate)
	values(@EffectiveDate, @PUB_PubRate_Map_ID, @CreatedBy, getdate())
	set @PUB_PubQuantity_ID = @@identity
	if (@@error <> 0) begin
		rollback tran t
		raiserror('Error inserting PUB_PubQuantity record.', 16, 1)
		return
	end

commit tran t
GO

GRANT  EXECUTE  ON [dbo].[PubQuantity_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO


GO
IF OBJECT_ID('dbo.PubRateMapActivate_s_ByPubIDandPubLocID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRateMapActivate_s_ByPubIDandPubLocID'
	DROP PROCEDURE dbo.PubRateMapActivate_s_ByPubIDandPubLocID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRateMapActivate_s_ByPubIDandPubLocID') IS NOT NULL
		PRINT '***********Drop of dbo.PubRateMapActivate_s_ByPubIDandPubLocID FAILED.'
END
GO
PRINT 'Creating dbo.PubRateMapActivate_s_ByPubIDandPubLocID'
GO

create PROCEDURE dbo.PubRateMapActivate_s_ByPubIDandPubLocID
/*
* PARAMETERS:
*	none
*
* DESCRIPTION:
*	Retrieves a list of effective dates and the active status for a pub location.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubRate_Map
*   PUB_PubRate_Map_Activate
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/26/2007      BJS             Initial Creation 
*
*/
@Pub_ID char(3),
@PubLoc_ID int
as

select prma.EffectiveDate, prma.Active
from PUB_PubRate_Map prm
	join PUB_PubRate_Map_Activate prma on prm.PUB_PubRate_Map_ID = prma.PUB_PubRate_Map_ID
where prm.Pub_ID = @Pub_ID and prm.PubLoc_ID = @PubLoc_ID
order by prma.EffectiveDate desc
GO

GRANT  EXECUTE  ON [dbo].[PubRateMapActivate_s_ByPubIDandPubLocID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubRateMap_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRateMap_i'
	DROP PROCEDURE dbo.PubRateMap_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRateMap_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubRateMap_i FAILED.'
END
GO
PRINT 'Creating dbo.PubRateMap_i'
GO

CREATE PROCEDURE dbo.PubRateMap_i
/*
* PARAMETERS:
*	PUB_PubRate_Map_ID
* Pub_ID
* PubLoc_ID
* CreatedBy
*
* DESCRIPTION:
*	Inserts a record into the PUB_PubRate_Map table
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubRate_Map			Insert
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/29/2007      BJS             Initial Creation 
*
*/
@PUB_PubRate_Map_ID bigint output,
@Pub_ID char(3),
@PubLoc_ID int,
@CreatedBy varchar(50)
AS

insert into PUB_PubRate_Map(Pub_ID, PubLoc_ID, CreatedBy, CreatedDate)
values(@Pub_ID, @PubLoc_ID, @CreatedBy, getdate())
set @PUB_PubRate_Map_ID = @@identity

GO

GRANT  EXECUTE  ON [dbo].[PubRateMap_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubRateMap_s_ActiveLocsByGroupIDRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRateMap_s_ActiveLocsByGroupIDRunDate'
	DROP PROCEDURE dbo.PubRateMap_s_ActiveLocsByGroupIDRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRateMap_s_ActiveLocsByGroupIDRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PubRateMap_s_ActiveLocsByGroupIDRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PubRateMap_s_ActiveLocsByGroupIDRunDate'
GO

CREATE PROC dbo.PubRateMap_s_ActiveLocsByGroupIDRunDate
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns a list of active pub locations for the specified Group ID and RunDate
*
* TABLES:
*		Table Name					Access
*		==========					======
*		pub_pubrate_map					READ
*		pub_pubrate_map_activate			READ
*		pub_loc						READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
*	Date		Who		Comments
*	------------- 	--------        -------------------------------------------------
*	09/17/2007	JRH		Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@RunDate	datetime,
@GroupID	bigint

AS

SELECT
	map.pub_pubrate_map_id
	, map.pub_id
	, map.publoc_id
	, map.createdby
	, map.createddate
	, map.modifiedby
	, map.modifieddate
FROM
	dbo.pub_pubrate_map map (nolock)
	INNER JOIN dbo.pub_pubrate_map_activate atv (nolock)
	ON map.pub_pubrate_map_id = atv.pub_pubrate_map_id
	INNER JOIN (
		SELECT 
			ma.pub_pubrate_map_id
			, max(ma.effectivedate) as effectivedate
		FROM
			dbo.pub_pubrate_map m (nolock)
			INNER JOIN dbo.pub_pubrate_map_activate ma (nolock)
				ON m.pub_pubrate_map_id = ma.pub_pubrate_map_id
			INNER JOIN dbo.pub_pubpubgroup_map gm (nolock)
				ON m.pub_pubrate_map_id = gm.pub_pubrate_map_id
		WHERE
			ma.effectivedate <= @RunDate
			AND gm.pub_pubgroup_id = @GroupID
		GROUP BY
			ma.pub_pubrate_map_id
		) effective
	ON atv.pub_pubrate_map_id = effective.pub_pubrate_map_id
		AND atv.effectivedate = effective.effectivedate
	INNER JOIN DBADVProd.informix.pub_loc loc (nolock)
	ON map.pub_id = loc.pub_id
		AND map.publoc_id = loc.publoc_id
WHERE
	atv.active = 1	
GO

GRANT  EXECUTE  ON [dbo].[PubRateMap_s_ActiveLocsByGroupIDRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.PubRateMap_s_ActiveLocsByRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRateMap_s_ActiveLocsByRunDate'
	DROP PROCEDURE dbo.PubRateMap_s_ActiveLocsByRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRateMap_s_ActiveLocsByRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PubRateMap_s_ActiveLocsByRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PubRateMap_s_ActiveLocsByRunDate'
GO

CREATE PROC dbo.PubRateMap_s_ActiveLocsByRunDate
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns a list of active pub locations for the specified RunDate
*
* TABLES:
*		Table Name					Access
*		==========					======
*		pub_pubrate_map					READ
*		pub_pubrate_map_activate			READ
*		pub_loc						READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
*	Date		Who		Comments
*	------------- 	--------        -------------------------------------------------
*	08/30/2007	JRH		Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@RunDate datetime

AS

SELECT
	map.pub_pubrate_map_id
	, map.pub_id
	, map.publoc_id
	, loc.publoc_nm
FROM
	dbo.pub_pubrate_map map (nolock)
	INNER JOIN dbo.pub_pubrate_map_activate atv (nolock)
	ON map.pub_pubrate_map_id = atv.pub_pubrate_map_id
	INNER JOIN (
		SELECT 
			ma.pub_pubrate_map_id
			, max(ma.effectivedate) as effectivedate
		FROM
			dbo.pub_pubrate_map m (nolock)
			INNER JOIN dbo.pub_pubrate_map_activate ma (nolock)
			ON m.pub_pubrate_map_id = ma.pub_pubrate_map_id
		WHERE
			ma.effectivedate <= @RunDate
		GROUP BY
			ma.pub_pubrate_map_id
		) effective
	ON atv.pub_pubrate_map_id = effective.pub_pubrate_map_id
		AND atv.effectivedate = effective.effectivedate
	INNER JOIN DBADVProd.informix.pub_loc loc (nolock)
	ON map.pub_id = loc.pub_id
		AND map.publoc_id = loc.publoc_id
WHERE
	atv.active = 1	
GO

GRANT  EXECUTE  ON [dbo].[PubRateMap_s_ActiveLocsByRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.PubRateMap_s_ActivePubsByRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRateMap_s_ActivePubsByRunDate'
	DROP PROCEDURE dbo.PubRateMap_s_ActivePubsByRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRateMap_s_ActivePubsByRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PubRateMap_s_ActivePubsByRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PubRateMap_s_ActivePubsByRunDate'
GO

CREATE PROC dbo.PubRateMap_s_ActivePubsByRunDate
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns a list of pub ID's with active locations 
*			for the specified RunDate
*
* TABLES:
*		Table Name					Access
*		==========					======
*		pub_pubrate_map					READ
*		pub_pubrate_map_activate			READ
*		pub						READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
*	Date		Who		Comments
*	------------- 	--------        -------------------------------------------------
*	08/30/2007	JRH		Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@RunDate datetime

AS

SELECT DISTINCT
	map.pub_id
	, pub.pub_nm
FROM
	dbo.pub_pubrate_map map (nolock)
	INNER JOIN dbo.pub_pubrate_map_activate atv (nolock)
	ON map.pub_pubrate_map_id = atv.pub_pubrate_map_id
	INNER JOIN (
		SELECT 
			ma.pub_pubrate_map_id
			, max(ma.effectivedate) as effectivedate
		FROM
			dbo.pub_pubrate_map m (nolock)
			INNER JOIN dbo.pub_pubrate_map_activate ma (nolock)
			ON m.pub_pubrate_map_id = ma.pub_pubrate_map_id
		WHERE
			ma.effectivedate <= @RunDate
		GROUP BY
			ma.pub_pubrate_map_id
		) effective
	ON atv.pub_pubrate_map_id = effective.pub_pubrate_map_id
		AND atv.effectivedate = effective.effectivedate
	INNER JOIN DBADVProd.informix.pub pub (nolock)
	ON map.pub_id = pub.pub_id
WHERE
	atv.active = 1	
GO

GRANT  EXECUTE  ON [dbo].[PubRateMap_s_ActivePubsByRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.PubRateMap_s_ByPubGroupID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRateMap_s_ByPubGroupID'
	DROP PROCEDURE dbo.PubRateMap_s_ByPubGroupID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRateMap_s_ByPubGroupID') IS NOT NULL
		PRINT '***********Drop of dbo.PubRateMap_s_ByPubGroupID FAILED.'
END
GO
PRINT 'Creating dbo.PubRateMap_s_ByPubGroupID'
GO

CREATE PROCEDURE dbo.PubRateMap_s_ByPubGroupID
/*
* PARAMETERS:
*	PUB_PubGroup_ID - required
*
* DESCRIPTION:
*	Returns all publication locations for the given Pub Group ID.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubRate_Map     READ
*   PUB_PubPubGroup_Map READ
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/04/2007      BJS             Initial Creation
*
*/
@PUB_PubGroup_ID bigint
as
select prm.*
from PUB_PubPubGroup_Map ppgm join PUB_PubRate_Map prm on ppgm.PUB_PubRate_Map_ID = prm.PUB_PubRate_Map_ID
where ppgm.PUB_PubGroup_ID = @PUB_PubGroup_ID
GO

GRANT  EXECUTE  ON [dbo].[PubRateMap_s_ByPubGroupID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubRateMap_s_ByPubIDandPubLocID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRateMap_s_ByPubIDandPubLocID'
	DROP PROCEDURE dbo.PubRateMap_s_ByPubIDandPubLocID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRateMap_s_ByPubIDandPubLocID') IS NOT NULL
		PRINT '***********Drop of dbo.PubRateMap_s_ByPubIDandPubLocID FAILED.'
END
GO
PRINT 'Creating dbo.PubRateMap_s_ByPubIDandPubLocID'
GO

CREATE PROCEDURE dbo.PubRateMap_s_ByPubIDandPubLocID
/*
* PARAMETERS:
*	Pub_ID - required
*   PubLoc_ID - required
*   RunDate - required.
*
* DESCRIPTION:
*	Returns the PubRate_Map for the Pub_ID and PubLoc_ID specified.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubRate_Map     READ
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/05/2007      BJS             Initial Creation
* 09/07/2007      BJS             Added RunDate as criteria.  A record is only returned if the pubratemap is active on the RunDate.
*
*/
@Pub_ID char(3),
@PubLoc_ID int,
@RunDate datetime
as

select * from PUB_PubRate_Map
where Pub_ID = @Pub_ID and PubLoc_ID = @PubLoc_ID and dbo.IsPubRateMapActive(PUB_PubRate_Map_ID, @RunDate) = 1
GO

GRANT  EXECUTE  ON [dbo].[PubRateMap_s_ByPubIDandPubLocID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubRateMap_s_WithDescription_ByPubGroupID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRateMap_s_WithDescription_ByPubGroupID'
	DROP PROCEDURE dbo.PubRateMap_s_WithDescription_ByPubGroupID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRateMap_s_WithDescription_ByPubGroupID') IS NOT NULL
		PRINT '***********Drop of dbo.PubRateMap_s_WithDescription_ByPubGroupID FAILED.'
END
GO
PRINT 'Creating dbo.PubRateMap_s_WithDescription_ByPubGroupID'
GO

CREATE PROCEDURE dbo.PubRateMap_s_WithDescription_ByPubGroupID
/*
* PARAMETERS:
*	PUB_PubGroup_ID - The pub group that limits the result set.
*
* DESCRIPTION:
*	Returns all PUB_PubRate_Map, Publication descriptions and whether or not they are being referenced by the specified Pub Group.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubPubGroupMap
*   PUB_PubRate_Map
*   ADMINSYSTEM.Pub
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/25/2007      BJS             Initial Creation 
*
*/
@PUB_PubGroup_ID bigint
as
select @PUB_PubGroup_ID PUB_PubGroup_ID, prm.PUB_PubRate_Map_ID, max(prm.Pub_ID) Pub_ID, max(prm.PubLoc_ID) PubLoc_ID, max(p.pub_nm) Pub_NM,
	case
		when max(ppgm.PUB_PubGroup_ID) is not null then 1
		else 0
	end InPubGroup
from PUB_PubRate_Map prm join DBADVPROD.informix.Pub p on prm.Pub_ID = p.Pub_ID
	left join PUB_PubPubGroup_Map ppgm on prm.PUB_PubRate_Map_ID = ppgm.PUB_PubRate_Map_ID and ppgm.PUB_PubGroup_ID = @PUB_PubGroup_ID
group by prm.PUB_PubRate_Map_ID
order by prm.PUB_ID, prm.PubLoc_ID
GO

GRANT  EXECUTE  ON [dbo].[PubRateMap_s_WithDescription_ByPubGroupID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubRateMap_s_withPubDescription') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRateMap_s_withPubDescription'
	DROP PROCEDURE dbo.PubRateMap_s_withPubDescription
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRateMap_s_withPubDescription') IS NOT NULL
		PRINT '***********Drop of dbo.PubRateMap_s_withPubDescription FAILED.'
END
GO
PRINT 'Creating dbo.PubRateMap_s_withPubDescription'
GO

CREATE PROCEDURE dbo.PubRateMap_s_withPubDescription
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Returns all publication locations in the database along with the publication description from the admin system.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubRate_Map
*   ADMINSYSTEM.Pub
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/22/2007      BJS             Initial Creation
* 06/28/2007      BJS             Added join to admin system
*
*/
as
select prm.*, p.Pub_NM
from PUB_PubRate_Map prm join DBADVPROD.informix.pub p on prm.Pub_ID = p.Pub_ID
where p.Pub_Type_CD = 'N'

GO

GRANT  EXECUTE  ON [dbo].[PubRateMap_s_withPubDescription]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubRate_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRate_i'
	DROP PROCEDURE dbo.PubRate_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRate_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubRate_i FAILED.'
END
GO
PRINT 'Creating dbo.PubRate_i'
GO

create proc dbo.PubRate_i
/*
* PARAMETERS:
* PUB_PubRate_ID - Output.  If a value is passed in it is ignored and the value is set to the ID of the new record.
*
*
* DESCRIPTION:
*   Inserts a record into the PUB_PubRate table.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubRate
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
* 05/25/2007      BJS             Initial Creation 
* 07/05/2007      BJS             Added logic to check for duplicate effective dates
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PUB_PubRate_ID bigint output,
@PUB_RateType_ID int,
@ChargeBlowIn bit,
@BlowInRate int,
@EffectiveDate datetime,
@QuantityChargeType int,
@BilledPct decimal,
@PUB_PubRate_Map_ID bigint,
@CreatedBy varchar(50)
as

begin tran t
	if exists(select 1 from PUB_PubRate where PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID and EffectiveDate = @EffectiveDate) begin
		rollback tran t
		raiserror('Rates already exist for the publication location on the same effective date.', 16, 1)
		return
	end

	insert into PUB_PubRate(PUB_RateType_ID, ChargeBlowIn, BlowInRate, EffectiveDate, QuantityChargeType, BilledPct, PUB_PubRate_Map_ID, CreatedBy,
		CreatedDate)
	values(@PUB_RateType_ID, @ChargeBlowIn, @BlowInRate, @EffectiveDate, @QuantityChargeType, @BilledPct, @PUB_PubRate_Map_ID, @CreatedBy, getdate())
	set @PUB_PubRate_ID = @@identity
	if (@@error <> 0) begin
		rollback tran t
		raiserror('Error inserting PUB_PubRate record.', 16, 1)
		return
	end

commit tran t
GO

GRANT  EXECUTE  ON [dbo].[PubRate_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.Pub_s') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.Pub_s'
	DROP PROCEDURE dbo.Pub_s
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.Pub_s') IS NOT NULL
		PRINT '***********Drop of dbo.Pub_s FAILED.'
END
GO
PRINT 'Creating dbo.Pub_s'
GO

create PROCEDURE dbo.Pub_s
/*
* PARAMETERS:
*	none
*
* DESCRIPTION:
*	Retrieves a list of all of the pubs available in the admin system.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   ADMINSYSTEM.pub
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/26/2007      BJS             Initial Creation
* 07/25/2007      BJS             Modified reference to admin system.  No longer uses a linked server 
*
*/
as

select Pub_ID, Pub_NM
from DBADVPROD.informix.pub
where pub_type_cd = 'N' /*Newspaper*/
order by Pub_NM
GO

GRANT  EXECUTE  ON [dbo].[Pub_s]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.RptReport_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.RptReport_i'
	DROP PROCEDURE dbo.RptReport_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.RptReport_i') IS NOT NULL
		PRINT '***********Drop of dbo.RptReport_i FAILED.'
END
GO
PRINT 'Creating dbo.Rpt_ReportHistory'
GO

CREATE PROCEDURE dbo.RptReport_i
/*
* PARAMETERS:
*	@RPT_Report_ID - Output.  Report ID of the newly inserted report
*	@RPT_ReportType_ID int - Report type Id
*	@Report image - Binary blob of the report
*	@CreatedBy varchar(50) - Username who created the report
*
* DESCRIPTION:
*	Inserts a record into the list of reports.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   RptReport			Insert
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	rpt_report_id 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/20/2007      NLS             Initial Creation 
*
*/
	@RPT_Report_ID bigint output,
	@RPT_ReportType_ID int,
	@Report image,
	@CreatedBy varchar(50)

AS

INSERT INTO RPT_Report( RPT_ReportType_ID,  Report,  CreatedBy,  CreatedDate )
VALUES			      ( @RPT_ReportType_ID, @Report, @CreatedBy, getdate() )

SET @RPT_Report_ID = @@identity

GO

GRANT  EXECUTE  ON [dbo].[RptReport_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.RPTReports_s_all') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.RPTReports_s_all'
	DROP PROCEDURE dbo.RPTReports_s_all
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.RPTReports_s_all') IS NOT NULL
		PRINT '***********Drop of dbo.RPTReports_s_all FAILED.'
END
GO
PRINT 'Creating dbo.RPTReports_s_all'
GO

create proc dbo.RPTReports_s_all
/*
* PARAMETERS:
*   None
*
* DESCRIPTION:
*	Returns a list of previously run reports
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   RptReport			Insert
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	rpt_report_id 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/20/2007      NLS             Initial Creation 
*
*/
as
select r.RPT_Report_ID, rt.Description, r.CreatedBy, r.CreatedDate
from RPT_Report r join RPT_ReportType rt on r.RPT_ReportType_ID = rt.RPT_ReportType_ID
order by r.CreatedDate desc


GO

GRANT  EXECUTE  ON [dbo].[RPTReports_s_all]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.RptReport_s_Report_ByReportID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.RptReport_s_Report_ByReportID'
	DROP PROCEDURE dbo.RptReport_s_Report_ByReportID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.RptReport_s_Report_ByReportID') IS NOT NULL
		PRINT '***********Drop of dbo.RptReport_s_Report_ByReportID FAILED.'
END
GO
PRINT 'Creating dbo.RptReport_s_Report_ByReportID'
GO

CREATE PROCEDURE dbo.RptReport_s_Report_ByReportID
/*
* PARAMETERS:
*	@RPT_Report_ID - Report ID of the report to retrieve
*
* DESCRIPTION:
*	Returns the binary blob for the specified report.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   RptReport			Read
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/20/2007      NLS             Initial Creation 
*
*/
	@RPT_Report_ID bigint

AS BEGIN
	
SELECT
	RPT_Report_ID,
	Report

FROM
	RPT_Report

WHERE RPT_Report_ID = @RPT_Report_ID

END
GO

GRANT  EXECUTE  ON [dbo].[RptReport_s_Report_ByReportID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.rpt_AdPublicationCosts') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.rpt_AdPublicationCosts'
	DROP PROCEDURE dbo.rpt_AdPublicationCosts
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.rpt_AdPublicationCosts') IS NOT NULL
		PRINT '***********Drop of dbo.rpt_AdPublicationCosts FAILED.'
END
GO
PRINT 'Creating dbo.rpt_AdPublicationCosts'
GO

CREATE PROCEDURE dbo.rpt_AdPublicationCosts
/*
* PARAMETERS:
* StartRunDate - Optional.  Estimates on or after this date will be returned.
* EndRunDate - Optional.  Estimates on or before this date will be returned.
* Pub_ID - Optional.  Publication cost information corresponding to distribution mapping referencing the Pub_ID will be returned.
* AdNumber - Optional.  Publication cost information corresponding to Components with matching AdNumber will be returned.
* EST_Estimate_ID - Optional.  Publication cost information for the Estimate will be returned.
* EST_Status_ID - Optional.  Estimate Status
*
* DESCRIPTION:
*		Returns publication costs for specified criteria.  Utilized by the "Ad Publication Costs Report" (Report/Extract #1)
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   MANY                READ
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
* 06/15/2007      BJS             Initial Creation 
* 06/25/2007	  JRH		      Changed OtherHandling to MailhouseOtherHandling
* 07/23/2007      BJS             Recomputed Estimate Costs to match calculations in EstEstimate_CostSummary_ByEstimateID
* 09/06/2007      BJS             Onsert Rate is now calculated from the Printer Rate table
*                                 Added check to prevent divide by zero error when determining the BlendedMailListCPM
*                                 Fixed join to PrinterRate table for plate cost.
* 09/17/2007      BJS             Replaced calls to Component Quantity functions with views
* 09/24/2007      BJS             Modified Digital H&P, Stitcher MR and Press MR logic
* 10/08/2007      BJS             Explicitly cast integer values (ie 1000) as decimal to prevent rounding errors.
* 11/16/2007      JRH             Fixed final select.
* 12/07/2007      JRH             Fixed insert cost by applying insert discount.
* 12/09/2007      JRH             Added Freight Costs into CostWithoutInsert.
* 12/10/2007      JRH             Changed calculation and precision of production and assembly piece costs. 
* 12/12/2007      JRH             Fixed TotalInsertQuantity.
* 12/18/2007      JRH             Calculate MediaQuantity from known quantities instead of view.
* 01/05/2008      JRH             Change final sort order.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@StartRunDate datetime,
@EndRunDate datetime,
@Pub_ID char(3),
@AdNumber integer,
@EST_Estimate_ID bigint,
@EST_Status_ID int
as

set nocount on

/* Comments correlate to layout of Report/Extract #4 - For Single Estimate Only */
create table #tempComponentPubRateMap(
	EST_Component_ID bigint,
	EST_Estimate_ID bigint,
	EST_Package_ID bigint,
	PUB_PubRate_Map_ID bigint,
	EST_EstimateMediaType_ID int,
	EST_ComponentType_ID int,
	PST_PostalScenario_ID bigint,
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
		InsertQuantity int,
		SoloQuantity int,
		PolybagQuantity int,
		OtherQuantity int,
		SampleQuantity int,
		MediaQuantity int, /* Insert + Solo + PolyBag */
		SpoilagePct decimal(10,4),
		SpoilageQuantity int, /* SpoilagePct * Media Quantity */
		DirectMailQuantity int, /* Solo + Poly */
		InternalMailQuantity int, /* DirectMail - External List Quantity */
		ExternalMailQuantity int, /* External List Quantity */
		TotalProductionQuantity int, /* MediaQuantity + SpoilageQuantity + OtherQuantity + SampleQuantity */
	/* Production */
		ProductionWeight decimal(14,6), /* TotalProductionQuantity * PieceWeight */
		TotalEstimateWeight decimal(14,6), /* sum(ProductionWeight) group by EST_Estimate_ID*/
		CreativeCPP money,
		CreativeCost money,
		SeparatorCPP money,
		SeparatorCost money,
		/* Print */
			ManualPrintCost money,
			RunRate money,
			NumberOfPlants int,
			AdditionalPlates int,
			PrinterPlateCost money,
			NumberDigitalHandlePrepare int,
			DigitalHandlePrepareRate money,
			StitcherMakeReadyRate money,
			PressMakeReadyRate money,
			ReplacementPlateCost money,
			GrossPrintCost money, /* See Estimate Calculation Steps for formula */
			EarlyPayPrintDiscountPercent decimal(10,4),
			EarlyPayPrintDiscountAmount money, /* GrossPrintCost * EarlyPayPrintDiscountPercent */
			NetPrintCost money, /* GrossPrintCost - EarlyPayPrintDiscountAmount */
			PrinterTaxableMediaPct decimal(10,4),
			PrinterSalesTaxPct decimal(10,4),
			PrinterSalesTaxAmount money, /* (NetPrinterCost + PaperHandling) * PrinterTaxableMediaPct * PrinterSalesTaxPct */
			TotalPrintCost money, /* NetPrintCost + PrinterSalesTaxAmount */
		/* Paper */
			ManualPaperCost money,
			RunPounds decimal(10,2),
			MakereadyPounds int,
			PlateChangePounds decimal(10,2),
			NumberOfPressStops int,
			PressStopPounds int,
			TotalPaperPounds decimal(20,2), /* See Estimate Calculation Steps for formula */
			PaperCWTRate money, /* PPR_Paper_Map.CWT */
			GrossPaperCost money, /* TotalPaperPounds / 100 * PaperCWTRate */
			EarlyPayPaperDiscountPercent decimal(10,4),
			EarlyPayPaperDiscountAmount money, /* GrossPaperCost * EarlyPayPaperDiscountPercent */
			NetPaperCost money, /* GrossPaperCost - EarlyPayPaperDiscountAmount */
			PaperHandlingCWTRate money, /* VND_Printer.PaperHandling */
			PaperHandlingCost money, /* TotalPaperPounds * PaperHandlingCWTRate */
			PaperTaxableMediaPct decimal(10,4),
			PaperSalesTaxPct decimal(10,4),
			PaperSalesTaxAmount money, /* NetPaperCost * PaperTaxableMediaPct * PaperSalesTaxPct */
			TotalPaperCost money, /* NetPaperCost + PaperSalesTaxAmount */
		/* Mail List */
			InternalMailCPM money,
			ExternalMailCPM money,
			BlendedMailListCPM money,
			MailListCost money, /* See Logic below */
		VendorProductionCPM money,
		VendorProductionCost money, /* TotalProductionQuantity / 1000 * VendorProductionCPM  */
		OtherProductionCost money,
		ProductionTotal money, /* CreativeCost + SeparatorCost + TotalPrintCost + TotalPaperCost + ExternalMailCost + VendorProductionCost + OtherProductionCost */
	/* Assembly */
		/* PolyBag */
			PolyBagCost money, /* dbo.EstimatePolyBagCost */
			OnsertRate money,
			OnsertCost money, /* Only if component type is Host Onsert -- PolyBagQuantity / 1000 * OnsertRate */
			PolyBagTotal money, /* PolyBagCost + OnsertCost */
		StitchInRate money,
		StitchInCost money, /* StitchInRate * MediaQuantity (if comp type is stitch-in) / 1000 */
		BlowInRate money,
		BlowInCost money, /* BlowInRate * MediaQuantity (if comp type is blow-in) / 1000 */
		/* Ink Jet */
			InkJetRate money,
			InkJetMakeReadyRate money,
			InkJetCost money, /* DirectMailQuantity / 1000 * InkJetRate */
			GrossInkjetMakereadyCost money, /* InkJetMakeReadyRate * NumberOfPlants */
			InkjetMakereadyCost money, /* See Logic below */
			TotalInkJetCost money, /* InkJetCost + InkJetMakeReadyCost */
		/* Handling */
			/* Insert */
				CornerGuardRate money,
				CornerGuardCost money, /* (MediaQuantity * PieceWeight) / 2000 * CornerGuardRate */
				SkidRate money,
				SkidCost money, /* (MediaQuantity * PieceWeight) / 2000 * SkidRate */
				NumberOfCartons int,
				CartonRate money,
				CartonCost money, /* NumberOfCartons * CartonCost */
				InsertHandlingTotal money, /* CornerGuardCost + SkidCost + CartonCost */
			/* Mail */
				TimeValueSlipsCPM money,
				TimeValueSlipsCost money, /* See SQL Logic Below */
				GrossMailHouseAdminFee money, /* The Mail House Admin Fee. */
				MailHouseAdminFee money, /* See SQL Logic Below */
				GlueTackCPM money,
				GlueTackCost money, /* See SQL Logic Below */
				TabbingCPM money,
				TabbingCost money, /* See SQL Logic Below */
				LetterInsertionCPM money,
				LetterInsertionCost money, /* See SQL Logic Below */
				OtherMailHandlingCPM money,
				OtherMailHandlingCost money, /* See SQL Logic Below */
				MailHandlingTotal money, /* See SQL Logic Below */
			HandlingTotal money, /* InsertHandlingTotal + MailHandlngTotal */
		AssemblyTotal money, /* PolyBagTotal + StitchInCost + BlowInCost + TotalInkJetCost + HandlingTotal */
	/* Distribution */
 		/* Insert */
			InsertFreightCWT money,
			InsertFreightCost money, /* TotalEstimateWeight / 100 * InsertFreightCWT */
			InsertFuelSurchargePercent decimal(10,4),
			InsertFuelSurchargeCost money, /* InsertFreightCost * InsertFreightSurchargePercent */
			InsertFreightTotalCost money,
		SampleFreight money,
		GrossOtherFreight money,
		OtherFreight money,
	Pub_ID char(3),
	Pub_NM char(30),
	PubLoc_ID int,
	PubQuantityType_ID int,
	PubRate_ID bigint,
	PubRateType_ID int,
	ProductionPieceCost decimal(20,8),
	RunDate datetime,
	IssueDate datetime,
	IssueDOW int,
	InsertTime bit,
	AssemblyPieceCost decimal(20,8),
	ComponentTabPageCount decimal,
	PackageTabPageCount decimal,
	PubRateMapInsertQuantity int,
	InsertDiscountPercent decimal(10,4),
	TotalInsertQuantity decimal(20,8),
	PubRateMapInsertCost money
)

/* Get Raw Production Data */
insert into #tempComponentPubRateMap(EST_Component_ID, EST_Estimate_ID, EST_Package_ID, PUB_PubRate_Map_ID, EST_EstimateMediaType_ID,
	EST_ComponentType_ID, PST_PostalScenario_ID, AdNumber, Description, PageCount, Width,
	Height, PaperWeight, CreativeCPP, CreativeCost, SeparatorCPP, SeparatorCost,
	ExternalMailQuantity, SpoilagePct, ManualPrintCost, RunRate, NumberOfPlants,
	AdditionalPlates, PrinterPlateCost, NumberDigitalHandlePrepare, DigitalHandlePrepareRate, StitcherMakereadyRate, PressMakereadyRate,
	ReplacementPlateCost, EarlyPayPrintDiscountPercent, PrinterTaxableMediaPct,
	PrinterSalesTaxPct, ManualPaperCost, RunPounds, MakeReadyPounds, PlateChangePounds, NumberOfPressStops, PressStopPounds, PaperCWTRate,
	EarlyPayPaperDiscountPercent, PaperHandlingCWTRate, PaperTaxableMediaPct, PaperSalesTaxPct, InternalMailCPM, ExternalMailCPM,
	VendorProductionCPM, OtherProductionCost, OnsertRate, StitchInRate, BlowInRate, InkJetRate, InkJetMakeReadyRate, CornerGuardRate, SkidRate,
	NumberOfCartons, TimeValueSlipsCPM, GrossMailHouseAdminFee, GlueTackCPM, TabbingCPM, LetterInsertionCPM, OtherMailHandlingCPM, 
	InsertFreightCWT, InsertFuelSurchargePercent, GrossOtherFreight,
	Pub_ID, Pub_NM, PubLoc_ID, PubQuantityType_ID, PubRate_ID, RunDate, IssueDate, IssueDOW, InsertTime)
select c.EST_Component_ID, c.EST_Estimate_ID, p.EST_Package_ID, prm.PUB_PubRate_Map_ID, c.EST_EstimateMediaType_ID, c.EST_ComponentType_ID, ad.PST_PostalScenario_ID,
	c.AdNumber, c.Description, c.PageCount, c.Width, c.Height, pw.Weight,
	c.CreativeCPP, c.CreativeCPP * c.PageCount, c.SeparatorCPP, c.SeparatorCPP * c.PageCount, ad.ExternalMailQty, c.SpoilagePct, c.PrintCost, c.RunRate, c.NumberOfPlants,
	c.AdditionalPlates, pl.Rate, c.NumberDigitalHandlenPrepare, dh.Rate,
	case
		when c.StitcherMakeready_ID is null then c.StitcherMakereadyRate
		else smr.Rate
	end StitcherMakereadyRate,
	case
		when c.PressMakeready_ID is null then c.PressMakereadyRate
		else prmr.Rate
	end PressMakereadyRate,
	c.ReplacementPlateCost, c.EarlyPayPrintDiscount,
	c.PrinterTaxableMediaPct, c.PrinterSalesTaxPct, c.PaperCost, c.RunPounds, c.MakeReadyPounds, c.PlateChangePounds, c.NumberOfPressStops,
	c.PressStopPounds, pm.CWT, c.EarlyPayPaperDiscount, pr.PaperHandling, c.PaperTaxableMediaPct, c.PaperSalesTaxPct,
	case
		when c.EST_ComponentType_ID = 1 then ml.InternalListRate
		else null
	end InternalMailCPM,
	case
		when c.EST_ComponentType_ID = 1 then ad.ExternalMailCPM
		else null
	end ExternalMailCPM, c.VendorCPM, c.OtherProduction,
	oi.Rate, si.Rate, bi.Rate,
	case
		when c.EST_ComponentType_ID = 1 then mh.InkJetRate
		else null
	end InkJetRate,
	case
		when c.EST_ComponentType_ID = 1 then mh.InkJetMakeReady
		else null
	end InkJetMakeReady,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.CornerGuards = 1 then pr.CornerGuard
		else null
	end CornerGuardRate,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.Skids = 1 then pr.Skid
		else null
	end SkidRate,
	ad.NbrOfCartons,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then mh.TimeValueSlips
		else null
	end TimeValueSlipsCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then mh.AdminFee
		else null
	end GrossMailHouseAdminFee,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseGlueTack = 1 then mh.GlueTackRate
		else null
	end GlueTackCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseTabbing = 1 then mh.TabbingRate
		else null
	end TabbingCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseLetterInsertion = 1 then mh.LetterInsertionRate
		else null
	end LetterInsertionCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then ad.MailHouseOtherHandling
		else null
	end OtherMailHandlingCPM,
	ad.InsertFreightCWT, ad.InsertFuelSurcharge,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then ad.OtherFreight
		else null
	end GrossOtherFreight,
	prm.Pub_ID, admin_pub.Pub_NM, prm.PubLoc_ID, p.PUB_PubQuantityType_ID, dbo.CalcPubRateID(prm.PUB_PubRate_Map_ID, pid.IssueDate),
	e.RunDate, pid.IssueDate,
	case p.PUB_PubQuantityType_ID
		when 4 then 1 /* Thanksgiving always uses Sunday rates */
		when 5 then 1 /* Christmaes always uses Sunday rates */
		when 6 then 1 /* New Years always uses Sunday rates */
		else pid.IssueDOW
	end IssueDOW,
	ad.InsertTime
from EST_Estimate e join vw_Estimate_excludeOldUploads ve on e.EST_Estimate_ID = ve.EST_Estimate_ID
	join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
	join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
	join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
	join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	join PUB_PubGroup pg on p.PUB_PubGroup_ID = pg.PUB_PubGroup_ID
	join PUB_PubPubGroup_Map ppgm on pg.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID
	join PUB_PubRate_Map prm on ppgm.PUB_PubRate_Map_ID = prm.PUB_PubRate_Map_ID
	join DBADVPROD.informix.pub admin_pub on prm.Pub_ID = admin_pub.Pub_ID
	join EST_PubIssueDates pid on e.EST_Estimate_ID = pid.EST_Estimate_ID and prm.PUB_PubRate_Map_ID = pid.PUB_PubRate_Map_ID
	left join VND_Printer pr on c.Printer_ID = pr.VND_Printer_ID
	left join PRT_PrinterRate pl on c.PlateCost_ID = pl.PRT_PrinterRate_ID /*Plates*/
	left join PRT_PrinterRate dh on c.DigitalHandlenPrepare_ID = dh.PRT_PrinterRate_ID /*Digi H&P*/
	left join PRT_PrinterRate smr on c.StitcherMakeready_ID = smr.PRT_PrinterRate_ID /*Stitcher Makeready*/
	left join PRT_PrinterRate prmr on c.PressMakeready_ID = prmr.PRT_PrinterRate_ID /*Press Makeready*/
	left join PPR_Paper_Map pm on c.Paper_Map_ID = pm.PPR_Paper_Map_ID
	left join PRT_PrinterRate si on c.StitchIn_ID = si.PRT_PrinterRate_ID
	left join PRT_PrinterRate bi on c.BlowIn_ID = bi.PRT_PrinterRate_ID
	left join PRT_PrinterRate oi on c.Onsert_ID = oi.PRT_PrinterRate_ID
	left join VND_MailTrackingRate mt on ad.MailTracking_ID = mt.VND_MailTrackingRate_ID
	join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
	left join VND_MailListResourceRate ml on ad.MailListResource_ID = ml.VND_MailListResourceRate_ID
where (@StartRunDate is null or e.RunDate >= @StartRunDate)
	and (@EndRunDate is null or e.RunDate <= @EndRunDate)
	and (@Pub_ID is null or prm.Pub_ID = @Pub_ID)
	and (@AdNumber is null or c.AdNumber = @AdNumber)
	and (@EST_Estimate_ID is null or e.EST_Estimate_ID = @EST_Estimate_ID)
	and (@EST_Status_ID is null or e.EST_Status_ID = @EST_Status_ID)

/*Second, if we are querying on AdNumber we need to make sure +CVR components that correspond to a HOST component with the AdNumber are included*/
/*We should still check to make sure the Pub_ID matches.*/
if (@AdNumber is not null)
	insert into #tempComponentPubRateMap(EST_Component_ID, EST_Estimate_ID, EST_Package_ID, PUB_PubRate_Map_ID, EST_EstimateMediaType_ID, EST_ComponentType_ID, PST_PostalScenario_ID,
		AdNumber, Description, PageCount, Width, Height, PaperWeight, CreativeCPP, CreativeCost, SeparatorCPP, SeparatorCost,
		ExternalMailQuantity, SpoilagePct, ManualPrintCost, RunRate, NumberOfPlants,
		AdditionalPlates, PrinterPlateCost, NumberDigitalHandlePrepare, DigitalHandlePrepareRate, StitcherMakereadyRate, PressMakereadyRate,
		ReplacementPlateCost,
		EarlyPayPrintDiscountPercent, PrinterTaxableMediaPct, PrinterSalesTaxPct, ManualPaperCost, RunPounds, MakeReadyPounds, PlateChangePounds,
		NumberOfPressStops, PressStopPounds, PaperCWTRate, EarlyPayPaperDiscountPercent, PaperHandlingCWTRate, PaperTaxableMediaPct,
		PaperSalesTaxPct, InternalMailCPM, ExternalMailCPM, VendorProductionCPM, OtherProductionCost, OnsertRate, StitchInRate, BlowInRate,
		InkJetRate, InkJetMakeReadyRate, CornerGuardRate, SkidRate, NumberOfCartons, TimeValueSlipsCPM, GrossMailHouseAdminFee, GlueTackCPM,
		LetterInsertionCPM, OtherMailHandlingCPM,
		Pub_ID, Pub_NM, PubLoc_ID, PubQuantityType_ID, PubRate_ID, RunDate, IssueDate, IssueDOW, InsertTime)
	select cvrcomp.EST_Component_ID, cvrcomp.EST_Estimate_ID, p.EST_Package_ID, prm.PUB_PubRate_Map_ID, cvrcomp.EST_EstimateMediaType_ID, cvrcomp.EST_ComponentType_ID,
		ad.PST_PostalScenario_ID, cvrcomp.AdNumber, cvrcomp.Description, cvrcomp.PageCount, cvrcomp.Width, cvrcomp.Height, pw.Weight,
		cvrcomp.CreativeCPP, cvrcomp.CreativeCPP * cvrcomp.PageCount, cvrcomp.SeparatorCPP, cvrcomp.SeparatorCPP * cvrcomp.PageCount,
		ad.ExternalMailQty, cvrcomp.SpoilagePct, cvrcomp.PrintCost, cvrcomp.RunRate,
		cvrcomp.NumberOfPlants, cvrcomp.AdditionalPlates, pl.Rate,
		cvrcomp.NumberDigitalHandlenPrepare, dh.Rate,
		case
			when cvrcomp.StitcherMakeready_ID is null then cvrcomp.StitcherMakereadyRate
			else smr.Rate
		end StitcherMakereadyRate,
		case
			when cvrcomp.PressMakeready_ID is null then cvrcomp.PressMakereadyRate
			else prmr.Rate
		end PressMakereadyRate,
		cvrcomp.ReplacementPlateCost, cvrcomp.EarlyPayPrintDiscount,
		cvrcomp.PrinterTaxableMediaPct,
		cvrcomp.PrinterSalesTaxPct, cvrcomp.PaperCost, cvrcomp.RunPounds, cvrcomp.MakeReadyPounds, cvrcomp.PlateChangePounds,
		cvrcomp.NumberOfPressStops, cvrcomp.PressStopPounds, pm.CWT, cvrcomp.EarlyPayPaperDiscount, pr.PaperHandling,
		cvrcomp.PaperTaxableMediaPct, cvrcomp.PaperSalesTaxPct,
		case
			when cvrcomp.EST_ComponentType_ID = 1 then ml.InternalListRate
			else null
		end InternalMailCPM,
		case
			when cvrcomp.EST_ComponentType_ID = 1 then ad.ExternalMailCPM
			else null
		end ExternalMailCPM,
		cvrcomp.VendorCPM, cvrcomp.OtherProduction, oi.Rate, si.Rate, bi.Rate,
		case
			when cvrcomp.EST_ComponentType_ID = 1 then mh.InkJetRate
			else null
		end InkJetRate,
		case
			when cvrcomp.EST_ComponentType_ID = 1 then mh.InkJetMakeReady
			else null
		end InkJetMakeReady,
		case
			when cvrcomp.EST_ComponentType_ID = 1 /*Host*/ and ad.CornerGuards = 1 then pr.CornerGuard
			else null
		end CornerGuardRate,
		case
			when cvrcomp.EST_ComponentType_ID = 1 /*Host*/ and ad.Skids = 1 then pr.Skid
			else null
		end SkidRate,
		ad.NbrOfCartons,
		case
			when cvrcomp.EST_ComponentType_ID = 1 /*Host*/ then mh.TimeValueSlips
			else null
		end TimeValueSlipsCPM,
		case
			when cvrcomp.EST_ComponentType_ID = 1 /*Host*/ then mh.AdminFee
			else null
		end GrossMailHouseAdminFee,
		case
			when cvrcomp.EST_ComponentType_ID = 1 /*Host*/ and ad.UseGlueTack = 1 then mh.GlueTackRate
			else null
		end GlueTackCPM,
		case
			when cvrcomp.EST_ComponentType_ID = 1 /*Host*/ and ad.UseLetterInsertion = 1 then mh.LetterInsertionRate
			else null
		end LetterInsertionCPM,
		case
			when cvrcomp.EST_ComponentType_ID = 1 /*Host*/ then ad.MailHouseOtherHandling
			else null
		end OtherMailHandlingCPM,
		prm.Pub_ID, admin_pub.Pub_NM, prm.PubLoc_ID, p.PUB_PubQuantityType_ID, dbo.CalcPubRateID(prm.PUB_PubRate_Map_ID, pid.IssueDate),
		tcprm.RunDate, pid.IssueDate,
		case p.PUB_PubQuantityType_ID
			when 4 then 1 /* Thanksgiving always uses Sunday rates */
			when 5 then 1 /* Christmaes always uses Sunday rates */
			when 6 then 1 /* New Years always uses Sunday rates */
			else pid.IssueDOW
		end IssueDOW,
		ad.InsertTime
	from #tempComponentPubRateMap tcprm join EST_Component hostcomp on tcprm.EST_Component_ID = hostcomp.EST_Component_ID
		join EST_Component cvrcomp on tcprm.EST_Estimate_ID = cvrcomp.EST_Estimate_ID
		join PPR_PaperWeight pw on cvrcomp.PaperWeight_ID = pw.PPR_PaperWeight_ID
		join EST_AssemDistribOptions ad on tcprm.EST_Estimate_ID = ad.EST_Estimate_ID
		join EST_PackageComponentMapping pcm on cvrcomp.EST_Component_ID = pcm.EST_Component_ID
		join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
		join PUB_PubGroup pg on p.PUB_PubGroup_ID = pg.PUB_PubGroup_ID
		join PUB_PubPubGroup_Map ppgm on pg.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID
		join Pub_PubRate_Map prm on ppgm.PUB_PubRate_Map_ID = prm.PUB_PubRate_Map_ID
		join DBADVPROD.informix.pub admin_pub on prm.Pub_ID = admin_pub.Pub_ID
		join EST_PubIssueDates pid on tcprm.EST_Estimate_ID = pid.EST_Estimate_ID and prm.PUB_PubRate_Map_ID = pid.PUB_PubRate_Map_ID
		left join VND_Printer pr on cvrcomp.Printer_ID = pr.VND_Printer_ID
		left join PRT_PrinterRate pl on cvrcomp.PlateCost_ID = pl.PRT_PrinterRate_ID /*Plates*/
		left join PRT_PrinterRate dh on cvrcomp.DigitalHandlenPrepare_ID = dh.PRT_PrinterRate_ID /*Digi H&P*/
		left join PRT_PrinterRate smr on cvrcomp.StitcherMakeready_ID = smr.PRT_PrinterRate_ID /* Stitcher Makeready */
		left join PRT_PrinterRate prmr on cvrcomp.PressMakeready_ID = prmr.PRT_PrinterRate_ID /* Press Makeready */
		left join PPR_Paper_Map pm on cvrcomp.Paper_Map_ID = pm.PPR_Paper_Map_ID
		left join PRT_PrinterRate si on cvrcomp.StitchIn_ID = si.PRT_PrinterRate_ID
		left join PRT_PrinterRate bi on cvrcomp.BlowIn_ID = bi.PRT_PrinterRate_ID
		left join PRT_PrinterRate oi on cvrcomp.Onsert_ID = oi.PRT_PrinterRate_ID
		left join VND_MailTrackingRate mt on ad.MailTracking_ID = mt.VND_MailTrackingRate_ID
		join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
		left join VND_MailListResourceRate ml on ad.MailListResource_ID = ml.VND_MailListResourceRate_ID
	where hostcomp.EST_ComponentType_ID = 1 and cvrcomp.EST_ComponentType_ID = 5 and cvrcomp.AdNumber is null
		and (@Pub_ID is null or prm.Pub_ID = @Pub_ID)


/* Perform Specification Calculations */
update #tempComponentPubRateMap
	set PieceWeight = (Width * Height) / cast(950000 as decimal) * PageCount * PaperWeight * 1.03

update #tempComponentPubRateMap
	set PackageWeight = dbo.PackageWeight(EST_Package_ID)

/* Set the PackageSize to the first matching component */
update tc
	set PackageSize = c.Width * c.Height
from #tempComponentPubRateMap tc join EST_PackageComponentMapping pcm on tc.EST_Package_ID = pcm.EST_Package_ID
	join EST_Component c on pcm.EST_Component_ID = c.EST_Component_ID

/* Set the PackageSize to the size of the host component, if the package contains one */
update tc
	set PackageSize = c.Width * c.Height
from #tempComponentPubRateMap tc join EST_PackageComponentMapping pcm on tc.EST_Package_ID = pcm.EST_Package_ID
	join EST_Component c on pcm.EST_Component_ID = c.EST_Component_ID
where c.EST_ComponentType_ID = 1

update tc
	set tc.TotalEstimateWeight = tw.TotalEstimateWeight
from #tempComponentPubRateMap tc join vwTotalEstimateWeight tw on tc.EST_Estimate_ID = tw.EST_Estimate_ID
where tc.EST_ComponentType_ID = 1

/* Perform Quantity Calculations */
update tc
	set InsertQuantity = isnull(iq.InsertQuantity, 0)
from #tempComponentPubRateMap tc left join vwComponentInsertQuantity iq on tc.EST_Component_ID = iq.EST_Component_ID

update tc
	set SoloQuantity = isnull(sq.SoloQuantity, 0)
from #tempComponentPubRateMap tc left join vwComponentSoloQuantity sq on tc.EST_Component_ID = sq.EST_Component_ID

update tc
	set PolyBagQuantity = isnull(pq.PolybagQuantity, 0)
from #tempComponentPubRateMap tc left join vwComponentPolybagQuantity pq on tc.EST_Component_ID = pq.EST_Component_ID

update tc
	set OtherQuantity = isnull(oq.OtherQuantity, 0)
from #tempComponentPubRateMap tc left join vwComponentOtherQuantity oq on tc.EST_Component_ID = oq.EST_Component_ID

update tc
	set SampleQuantity = isnull(sq.SampleQuantity, 0)
from #tempComponentPubRateMap tc left join vwComponentSampleQuantity sq on tc.EST_Component_ID = sq.EST_Component_ID

update tc
	set MediaQuantity = tc.InsertQuantity + tc.SoloQuantity + tc.PolybagQuantity
from #tempComponentPubRateMap tc

update #tempComponentPubRateMap
	set SpoilageQuantity = isnull(SpoilagePct, 0) * MediaQuantity,
		DirectMailQuantity = SoloQuantity + PolybagQuantity,
		InternalMailQuantity = (SoloQuantity + PolybagQuantity) - ExternalMailQuantity

update tc
	set PubRateType_ID = pr.PUB_RateType_ID
from #tempComponentPubRateMap tc join PUB_PubRate pr on tc.PubRate_ID = pr.PUB_PubRate_ID

update tc
	set PubRateMapInsertQuantity = isnull((
		select top 1 dowqty.Quantity
		from PUB_PubQuantity ppq join PUB_DayOfWeekQuantity dowqty on ppq.PUB_PubQuantity_ID = dowqty.PUB_PubQuantity_ID
			join PUB_PubQuantityType pqt on dowqty.PUB_PubQuantityType_ID = pqt.PUB_PubQuantityType_ID
		where ppq.PUB_PubRate_Map_ID = tc.PUB_PubRate_Map_ID and dowqty.PUB_PubQuantityType_ID = tc.PubQuantityType_ID
			and (pqt.Special = 1 or tc.IssueDOW = dowqty.InsertDow)
			and ppq.EffectiveDate <= tc.IssueDate
		order by EffectiveDate desc), 0)
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

update #tempComponentPubRateMap
	set TotalProductionQuantity = MediaQuantity + SpoilageQuantity + OtherQuantity + SampleQuantity

update #tempComponentPubRateMap
	set ProductionWeight = TotalProductionQuantity * PieceWeight

/* Perform Production Calculations */
update #tempComponentPubRateMap
	set GrossPrintCost = ManualPrintCost
where ManualPrintCost is not null

update #tempComponentPubRateMap
	set GrossPrintCost = TotalProductionQuantity / cast(1000 as decimal) * isnull(RunRate, 0)
		+ isnull(NumberOfPlants, 0) * (isnull(StitcherMakeReadyRate, 0) + isnull(PressMakeReadyRate, 0))
		+ isnull(AdditionalPlates, 0) * isnull(PrinterPlateCost, 0)
		+ isnull(NumberDigitalHandlePrepare, 0) * isnull(DigitalHandlePrepareRate, 0)
		+ isnull(ReplacementPlateCost, 0)
where ManualPrintCost is null

update #tempComponentPubRateMap
	set EarlyPayPrintDiscountAmount = GrossPrintCost * isnull(EarlyPayPrintDiscountPercent, 0),
		NetPrintCost = GrossPrintCost - (GrossPrintCost * isnull(EarlyPayPrintDiscountPercent, 0))

/*TODO: We don't have a paper handling amount if manualpapercost is entered.  How does this effect the print sales tax?? */
update #tempComponentPubRateMap
	set GrossPaperCost = ManualPaperCost
where ManualPaperCost is not null

update #tempComponentPubRateMap
	set TotalPaperPounds = TotalProductionQuantity / cast(1000 as decimal) * isnull(RunPounds, 0)
		+ cast(isnull(NumberOfPlants, 0) as bigint) * cast(isnull(MakeReadyPounds, 0) as bigint)
		+ cast(isnull(AdditionalPlates, 0) as bigint) * isnull(PlateChangePounds, 0)
		+ cast(isnull(NumberOfPressStops, 0) as bigint) * cast(isnull(PressStopPounds, 0) as bigint)
where ManualPaperCost is null

update #tempComponentPubRateMap
	set GrossPaperCost = isnull(TotalPaperPounds, 0) / cast(100 as decimal) * isnull(PaperCWTRate, 0)
where ManualPaperCost is null

update #tempComponentPubRateMap
	set EarlyPayPaperDiscountAmount = GrossPaperCost * isnull(EarlyPayPaperDiscountPercent, 0),
		NetPaperCost = GrossPaperCost - (GrossPaperCost * isnull(EarlyPayPaperDiscountPercent, 0))

update #tempComponentPubRateMap
	set PaperHandlingCost = isnull(TotalPaperPounds, 0) / cast(100 as decimal) * isnull(PaperHandlingCWTRate, 0)

update #tempComponentPubRateMap
	set PrinterSalesTaxAmount = (NetPrintCost + PaperHandlingCost) * isnull(PrinterTaxableMediaPct, 0) * isnull(PrinterSalesTaxPct, 0),
		PaperSalesTaxAmount = NetPaperCost * isnull(PaperTaxableMediaPct, 0) * isnull(PaperSalesTaxPct, 0)

update #tempComponentPubRateMap
	set TotalPrintCost = NetPrintCost + PrinterSalesTaxAmount,
		TotalPaperCost = NetPaperCost + PaperHandlingCost + PaperSalesTaxAmount,
		VendorProductionCost = TotalProductionQuantity / cast(1000 as decimal) * VendorProductionCPM

update #tempComponentPubRateMap
	set BlendedMailListCPM =
		case
			when dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) = 0 then 0
			else InternalMailCPM
					* cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) - ExternalMailQuantity as decimal)
					/ cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
				+ ExternalMailCPM
					* cast(ExternalMailQuantity as decimal)
					/ cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
		end
where EST_ComponentType_ID = 1

update #tempComponentPubRateMap
	set MailListCost = BlendedMailListCPM * SoloQuantity / 1000 + isnull(dbo.ComponentPolybagMailListCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponentPubRateMap
	set ProductionTotal = isnull(CreativeCost, 0) + isnull(SeparatorCost, 0) + TotalPrintCost + TotalPaperCost +  isnull(MailListCost, 0) +
		+ isnull(VendorProductionCost, 0) + isnull(OtherProductionCost, 0)

/* Assembly Calculations */

update #tempComponentPubRateMap
	set PolyBagCost = dbo.EstimatePolyBagCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1

update #tempComponentPubRateMap
	set OnsertCost = PolybagQuantity / cast(1000 as decimal) * OnsertRate
where EST_ComponentType_ID = 2

update #tempComponentPubRateMap
	set GrossInkjetMakereadyCost = NumberofPlants * InkjetMakereadyRate

update #tempComponentPubRateMap
	set PolyBagTotal = isnull(PolyBagCost, 0) + isnull(OnsertCost, 0),
		StitchInCost =
			case EST_ComponentType_ID
				when 3 then MediaQuantity / cast(1000 as decimal) * StitchInRate
				else null
			end,
		BlowInCost =
			case EST_ComponentType_ID
				when 4 then MediaQuantity / cast(1000 as decimal) * BlowInRate
			end,
		InkJetCost =
			case EST_ComponentType_ID
				when 1 then (isnull(SoloQuantity, 0) / cast(1000 as decimal) * InkJetRate) + isnull(dbo.EstimatePolybagInkjetCost(EST_Estimate_ID), 0)
			end,
		InkJetMakeReadyCost =
			case EST_ComponentType_ID
				when 1 then
					case
						when isnull(SoloQuantity, 0) = 0 then 0
						else GrossInkjetMakereadyCost * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
					end
					+ isnull(dbo.ComponentPolybagInkjetMakereadyCost(EST_Component_ID), 0)
			end

update #tempComponentPubRateMap
		set TotalInkJetCost = isnull(InkJetCost, 0) + isnull(InkJetMakeReadyCost, 0)

update tc
	set CartonRate = pr.Rate
from #tempComponentPubRateMap tc join EST_Component c on tc.EST_Component_ID = c.EST_Component_ID
	join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate pr on p.VND_Printer_ID = pr.VND_Printer_ID and pr.PRT_PrinterRateType_ID = 3 and pr.[Default] = 1

update #tempComponentPubRateMap
	set CornerGuardCost = TotalEstimateWeight / cast(2000 as decimal) * CornerGuardRate,
		SkidCost = TotalEstimateWeight / cast(2000 as decimal) * SkidRate,
		CartonCost = NumberOfCartons * CartonRate,
		TimeValueSlipsCost = TimeValueSlipsCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseTimeValueSlipsCost(EST_Component_ID), 0),
		MailHouseAdminFee =
			case
				when SoloQuantity = 0 then 0
				else GrossMailHouseAdminFee * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
			end
			+ isnull(dbo.ComponentPolybagMailHouseAdminFee(EST_Component_ID), 0),
		GlueTackCost = GlueTackCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseGlueTackCost(EST_Component_ID), 0),
		TabbingCost = TabbingCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseTabbingCost(EST_Component_ID), 0),
		LetterInsertionCost = LetterInsertionCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseLetterInsertionCost(EST_Component_ID), 0),
		OtherMailHandlingCost = OtherMailHandlingCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseOtherDirectMailHandlingCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponentPubRateMap
	set InsertHandlingTotal = isnull(CornerGuardCost, 0) + isnull(SkidCost, 0) + isnull(CartonCost, 0),
		MailHandlingTotal = isnull(TimeValueSlipsCost, 0) + isnull(MailHouseAdminFee, 0) + isnull(GlueTackCost, 0) + isnull(TabbingCost, 0)
			+ isnull(LetterInsertionCost, 0) + isnull(OtherMailHandlingCost, 0)

update #tempComponentPubRateMap
	set HandlingTotal = InsertHandlingTotal + MailHandlingTotal

update #tempComponentPubRateMap
	set AssemblyTotal = PolyBagTotal + isnull(StitchInCost, 0) + isnull(BlowInCost, 0) + TotalInkJetCost + HandlingTotal

/* Calculate the Piece Cost */
update tc
	set tc.TotalInsertQuantity = t.TotalInsertQuantity
from #tempComponentPubRateMap tc 
	join (
		select EST_Component_ID
			, TotalInsertQuantity = cast(sum(PubRateMapInsertQuantity) as decimal(20, 6))
		from #tempComponentPubRateMap
		group by EST_Component_ID) t
	on tc.EST_Component_ID = t.EST_Component_ID

update #tempComponentPubRateMap
	set
		ProductionPieceCost =
			case TotalInsertQuantity
				when 0 then 0
				else ProductionTotal / TotalInsertQuantity
			end,
		AssemblyPieceCost =
			case TotalInsertQuantity
				when 0 then 0
				else AssemblyTotal / TotalInsertQuantity
			end


/* Calculate the Insert Cost */
-- Tab Page Count Rate Type
update tc
	set tc.PubRateMapInsertCost =
		case tc.PackageTabPageCount
			when 0 then 0
			else
				isnull((
			select top 1 wr.Rate
			from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
			where wrt.PUB_PubRate_ID = tc.PubRate_ID and wrt.RateTypeDescription >= tc.PackageTabPageCount and tc.IssueDOW = wr.InsertDow
			order by wrt.RateTypeDescription), 0)
			*
			case pr.QuantityChargeType
				when 1 then tc.PubRateMapInsertQuantity - (tc.PubRateMapInsertQuantity * pr.BilledPct)
				else tc.PubRateMapInsertQuantity
			end
			/ cast (1000 as decimal)
			* cast(tc.ComponentTabPageCount as decimal) / cast(tc.PackageTabPageCount as decimal)
		end
from #tempComponentPubRateMap tc join PUB_PubRate pr on tc.PubRate_ID = pr.PUB_PubRate_ID
where tc.PubRateType_ID = 1

-- Flat Rate Type
update tc
	set tc.PubRateMapInsertCost = isnull((
		select wr.Rate
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = tc.PubRate_ID and tc.IssueDOW = wr.InsertDow), 0) * tc.PieceWeight / tc.PackageWeight
from #tempComponentPubRateMap tc
where tc.PubRateType_ID = 2

-- CPM
update tc
	set tc.PubRateMapInsertCost = isnull((
		select top 1 wr.Rate
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = tc.PubRate_ID and wrt.RateTypeDescription >= tc.PubRateMapInsertQuantity and tc.IssueDOW = wr.InsertDow
		order by wrt.RateTypeDescription), 0)
		*
		case pr.QuantityChargeType
			when 1 then tc.PubRateMapInsertQuantity - (tc.PubRateMapInsertQuantity * pr.BilledPct)
			else tc.PubRateMapInsertQuantity
		end
		/ cast(1000 as decimal)
		* tc.PieceWeight / tc.PackageWeight
from #tempComponentPubRateMap tc join PUB_PubRate pr on tc.PubRate_ID = pr.PUB_PubRate_ID
where tc.PubRateType_ID = 3

-- Weight
update tc
	set tc.PubRateMapInsertCost = isnull((
		select top 1 wr.Rate
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = tc.PubRate_ID and wrt.RateTypeDescription >= tc.PackageWeight and tc.IssueDOW = wr.InsertDow
		order by wrt.RateTypeDescription), 0) * tc.PieceWeight / tc.PackageWeight
from #tempComponentPubRateMap tc
where tc.PubRateType_ID = 4

--Size
update tc
	set tc.PubRateMapInsertCost = isnull((
		select top 1 wr.Rate
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = tc.PubRate_ID and wrt.RateTypeDescription >= tc.PackageSize and tc.IssueDOW = wr.InsertDow
		order by wrt.RateTypeDescription), 0) * tc.PieceWeight / tc.PackageWeight
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
from #tempComponentPubRateMap t join #tempPackageInsertNumber pin on t.EST_Package_ID = pin.EST_Package_ID and t.PUB_PubRate_Map_ID = pin.PUB_PubRate_Map_ID
	join PUB_InsertDiscounts d on t.PubRate_ID = d.PUB_PubRate_ID
where pin.InsertNumber = d.[Insert]

drop table #tempPackageInsertNumber

/*----------------------------------------- */

update tc
	set PubRateMapInsertCost = PubRateMapInsertCost * (1 - isnull(InsertDiscountPercent, 0))
from #tempComponentPubRateMap tc

update #tempComponentPubRateMap
	set InsertFreightCost = TotalEstimateWeight / cast(100 as decimal) * InsertFreightCWT,
		InsertFuelSurchargeCost = (TotalEstimateWeight / cast(100 as decimal) * InsertFreightCWT) * InsertFuelSurchargePercent
where EST_ComponentType_ID = 1

update #tempComponentPubRateMap
	set InsertFreightTotalCost = isnull(InsertFreightCost, 0) + isnull(InsertFuelSurchargeCost, 0)

update #tempComponentPubRateMap
	set OtherFreight = GrossOtherFreight
where EST_ComponentType_ID = 1

update t
	set SampleFreight =
		case
			when isnull(s.FreightCWT, 0) <> 0 then dbo.EstimatePieceWeight(t.EST_Estimate_ID) * SampleQuantity / cast(100 as decimal) * s.FreightCWT
			else s.FreightFlat
		end
from #tempComponentPubRateMap t join EST_Samples s on t.EST_Estimate_ID = s.EST_Estimate_ID
where t.EST_ComponentType_ID = 1

update t
	set SampleFreight = SampleFreight * PubRateMapInsertQuantity / TotalInsertQuantity
		, InsertFreightTotalCost = InsertFreightTotalCost * PubRateMapInsertQuantity / TotalInsertQuantity
		, OtherFreight = OtherFreight * PubRateMapInsertQuantity / TotalInsertQuantity
from #tempComponentPubRateMap t

set nocount off

select RunDate, IssueDate, AdNumber, InsertTime, Pub_ID, Pub_NM, PubLoc_ID, cast(PieceCost as money) PieceCost, PubRateMapInsertQuantity
	, cast((sum(CostWithoutInsert) + sum(OtherNonInsert)) as money) as CostWithoutInsert
	, sum(PubRateMapInsertCost) as PubRateMapInsertCost
	, sum(CostWithoutInsert) + sum(OtherNonInsert) + sum(PubRateMapInsertCost) as TotalCost
from
	(select RunDate, IssueDate, AdNumber,
		case InsertTime
			when 0 then 'AM/PM'
			else 'PM/AM'
		end InsertTime,
		Pub_ID,	Pub_NM, PubLoc_ID, 
		(ProductionPieceCost + AssemblyPieceCost) as PieceCost, 
		PubRateMapInsertQuantity,
		isnull(SampleFreight, 0) + isnull(InsertFreightTotalCost, 0) + isnull(OtherFreight, 0) OtherNonInsert,
		(ProductionPieceCost + AssemblyPieceCost) * PubRateMapInsertQuantity as CostWithoutInsert, 
		isnull(PubRateMapInsertCost, 0) PubRateMapInsertCost
	 from #tempComponentPubRateMap) a
group by RunDate, AdNumber, IssueDate, InsertTime, Pub_ID, Pub_NM, PubLoc_ID, PieceCost, PubRateMapInsertQuantity
order by RunDate, AdNumber, IssueDate, InsertTime, Pub_ID, PubLoc_ID

set nocount on

drop table #tempComponentPubRateMap
GO

GRANT  EXECUTE  ON [dbo].[rpt_AdPublicationCosts]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.rpt_DirectMailCosts') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.rpt_DirectMailCosts'
	DROP PROCEDURE dbo.rpt_DirectMailCosts
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.rpt_DirectMailCosts') IS NOT NULL
		PRINT '***********Drop of dbo.rpt_DirectMailCosts FAILED.'
END
GO
PRINT 'Creating dbo.rpt_DirectMailCosts'
GO

CREATE PROCEDURE dbo.rpt_DirectMailCosts
/*
* PARAMETERS:
* StartRunDate - Optional.  Estimates on or after this date will be returned.
* EndRunDate - Optional.  Estimates on or before this date will be returned.
* AdNumber - Optional.  Publication cost information corresponding to Components with matching AdNumber will be returned.
* EST_Estimate_ID - Optional.  Publication cost information for the Estimate will be returned.
* EST_Status_ID - Optional.
*
*
* DESCRIPTION:
*		Returns publication costs for specified criteria.  Utilized by the "Direct Mail Costs Report"
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   TBD
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
* 07/26/2007      BJS             Initial Creation 
* 09/06/2007      BJS             Added check to BlendedMailListCPM calculation to prevent divide by zero error
* 09/19/2007      BJS             Replaced calls to Quantity functions with views
* 09/24/2007      BJS             Modified Digital H&P, Stitcher Makeready and Press Makeready logic
* 10/05/2007      BJS             Explicitly cast integer values (ie 1000) as decimal to prevent rounding errors in calculations
* 10/29/2007      JRH             Fixed Typo #tempComponents to #tempComponent.
* 12/09/2007      JRH             Added SampleFreight Cost.
*                                 Changed calculation and precision of production and assembly piece costs. 
* 12/17/2007      JRH             Calculate MediaQuantity from known quantities instead of view.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@StartRunDate datetime,
@EndRunDate datetime,
@AdNumber integer,
@EST_Estimate_ID bigint,
@EST_Status_ID int
as

set nocount on

/* Comments correlate to layout of Report/Extract #2 - Direct Mail Costs */
create table #tempComponent(
	EST_Component_ID bigint,
	EST_Estimate_ID bigint,
	EST_ComponentType_ID int,
	PST_PostalScenario_ID bigint,
	/* Main */
		RunDate datetime,
		AdNumber int,
		Description varchar(35),
	/* Specifications */
		PageCount int,
		Width decimal(10,4),
		Height decimal(10,4),
		/* Paper */
			PaperWeight int,
		PieceWeight decimal(12,6), /* (Width * Height) / 950,000 * PageCount * PaperWeight * 1.03 */
	/* Quantity */
		InsertQuantity int,
		SoloQuantity int,
		PolybagQuantity int,
		OtherQuantity int,
		SampleQuantity int,
		MediaQuantity int, /* Insert + Solo + PolyBag */
		SpoilagePct decimal(10,4),
		SpoilageQuantity int, /* SpoilagePct * Media Quantity */
		DirectMailQuantity int, /* Solo + Poly */
		InternalMailQuantity int, /* DirectMail - External List Quantity */
		ExternalMailQuantity int, /* External List Quantity */
		TotalProductionQuantity int, /* MediaQuantity + SpoilageQuantity + OtherQuantity + SampleQuantity */
	/* Production */
		ProductionWeight decimal(14,6), /* TotalProductionQuantity * PieceWeight */
		TotalEstimateWeight decimal(14,6), /* sum(ProductionWeight) group by EST_Estimate_ID*/
		CreativeCPP money,
		CreativeCost money,
		SeparatorCPP money,
		SeparatorCost money,
		/* Print */
			ManualPrintCost money,
			RunRate money,
			NumberOfPlants int,
			AdditionalPlates int,
			PrinterPlateCost money,
			NumberDigitalHandlePrepare int,
			DigitalHandlePrepareRate money,
			StitcherMakeReadyRate money,
			PressMakeReadyRate money,
			ReplacementPlateCost money,
			GrossPrintCost money, /* See Estimate Calculation Steps for formula */
			EarlyPayPrintDiscountPercent decimal(10,4),
			EarlyPayPrintDiscountAmount money, /* GrossPrintCost * EarlyPayPrintDiscountPercent */
			NetPrintCost money, /* GrossPrintCost - EarlyPayPrintDiscountAmount */
			PrinterTaxableMediaPct decimal(10,4),
			PrinterSalesTaxPct decimal(10,4),
			PrinterSalesTaxAmount money, /* (NetPrinterCost + PaperHandling) * PrinterTaxableMediaPct * PrinterSalesTaxPct */
			TotalPrintCost money, /* NetPrintCost + PrinterSalesTaxAmount */
		/* Paper */
			ManualPaperCost money,
			RunPounds decimal(10,2),
			MakereadyPounds int,
			PlateChangePounds decimal(10,2),
			NumberOfPressStops int,
			PressStopPounds int,
			TotalPaperPounds decimal(20,2), /* See Estimate Calculation Steps for formula */
			PaperCWTRate money, /* PPR_Paper_Map.CWT */
			GrossPaperCost money, /* TotalPaperPounds / 100 * PaperCWTRate */
			EarlyPayPaperDiscountPercent decimal(10,4),
			EarlyPayPaperDiscountAmount money, /* GrossPaperCost * EarlyPayPaperDiscountPercent */
			NetPaperCost money, /* GrossPaperCost - EarlyPayPaperDiscountAmount */
			PaperHandlingCWTRate money, /* VND_Printer.PaperHandling */
			PaperHandlingCost money, /* TotalPaperPounds * PaperHandlingCWTRate */
			PaperTaxableMediaPct decimal(10,4),
			PaperSalesTaxPct decimal(10,4),
			PaperSalesTaxAmount money, /* NetPaperCost * PaperTaxableMediaPct * PaperSalesTaxPct */
			TotalPaperCost money, /* NetPaperCost + PaperSalesTaxAmount */
		/* Mail List */
			InternalMailCPM money,
			ExternalMailCPM money,
			BlendedMailListCPM money,
			MailListCost money, /* See Logic below */
		VendorProductionCPM money,
		VendorProductionCost money, /* TotalProductionQuantity / 1000 * VendorProductionCPM  */
		OtherProductionCost money,
		ProductionTotal money, /* CreativeCost + SeparatorCost + TotalPrintCost + TotalPaperCost + ExternalMailCost + VendorProductionCost + OtherProductionCost */
		ProductionPieceCost decimal(20,6),

	/* Assembly */
		/* PolyBag */
			PolyBagCost money, /* dbo.EstimatePolyBagCost */
			OnsertRate money,
			OnsertCost money, /* Only if component type is Host Onsert -- PolyBagQuantity / 1000 * OnsertRate */
			PolyBagTotal money, /* PolyBagCost + OnsertCost */
		StitchInRate money,
		StitchInCost money, /* StitchInRate * MediaQuantity (if comp type is stitch-in) / 1000 */
		BlowInRate money,
		BlowInCost money, /* BlowInRate * MediaQuantity (if comp type is blow-in) / 1000 */
		/* Ink Jet */
			InkJetRate money,
			InkJetMakeReadyRate money,
			InkJetCost money, /* DirectMailQuantity / 1000 * InkJetRate */
			GrossInkjetMakereadyCost money, /* InkJetMakeReadyRate * NumberOfPlants */
			InkjetMakereadyCost money, /* See Logic below */
			TotalInkJetCost money, /* InkJetCost + InkJetMakeReadyCost */
		/* Mail Handling */
			TimeValueSlipsCPM money,
			TimeValueSlipsCost money, /* See SQL Logic Below */
			GrossMailHouseAdminFee money, /* The Mail House Admin Fee. */
			MailHouseAdminFee money, /* See SQL Logic Below */
			GlueTackCPM money,
			GlueTackCost money, /* See SQL Logic Below */
			TabbingCPM money,
			TabbingCost money, /* See SQL Logic Below */
			LetterInsertionCPM money,
			LetterInsertionCost money, /* See SQL Logic Below */
			OtherMailHandlingCPM money,
			OtherMailHandlingCost money, /* See SQL Logic Below */
			MailHandlingTotal money, /* See SQL Logic Below */
		AssemblyTotal money, /* PolyBagTotal + StitchInCost + BlowInCost + TotalInkJetCost + MailHandlingTotal */
		AssemblyPieceCost decimal(20,6),

	/* Distribution */
		/* Postal */
			PostalDropCost money, /* See Logic Below */
			PostalDropFuelSurchargeCost money, /* See Logic Below*/
			MailTrackingCPMRate money,
			MailTrackingCost money, /* DirectMailQuantity / 1000 * MailTrackingCPMRate */
			SoloPostageCost money,
			PolyPostageCost money,
			TotalPostageCost money, /* SoloPostageCost + PolyPostageCost */
			PostalTotal money, /*Postage PostalDropCost + PostalFuelSurcharge + MailTrackingCost + TotalPostageCost */
		SampleFreight money,
		GrossOtherFreight money,
		OtherFreight money,
		DistributionTotal money /* InsertTotal + PostalTotal + SampleFreight + OtherFreight */ 	
)

/* Get Raw Production Data */
insert into #tempComponent(EST_Component_ID, EST_Estimate_ID, EST_ComponentType_ID, PST_PostalScenario_ID,
	RunDate, AdNumber, Description, PageCount, Width,
	Height, PaperWeight, CreativeCPP, CreativeCost, SeparatorCPP, SeparatorCost,
	ExternalMailQuantity, SpoilagePct, ManualPrintCost, RunRate, NumberOfPlants, AdditionalPlates,
	PrinterPlateCost, NumberDigitalHandlePrepare, DigitalHandlePrepareRate, StitcherMakereadyRate, PressMakereadyRate,
	ReplacementPlateCost, EarlyPayPrintDiscountPercent, PrinterTaxableMediaPct,
	PrinterSalesTaxPct, ManualPaperCost, RunPounds, MakeReadyPounds, PlateChangePounds, NumberOfPressStops, PressStopPounds, PaperCWTRate,
	EarlyPayPaperDiscountPercent, PaperHandlingCWTRate, PaperTaxableMediaPct, PaperSalesTaxPct, InternalMailCPM, ExternalMailCPM,
	VendorProductionCPM, OtherProductionCost,
	OnsertRate, StitchInRate, BlowInRate, InkJetRate, InkJetMakeReadyRate, TimeValueSlipsCPM, GrossMailHouseAdminFee, GlueTackCPM, TabbingCPM,
	LetterInsertionCPM, OtherMailHandlingCPM, MailTrackingCPMRate, GrossOtherFreight)
select c.EST_Component_ID, c.EST_Estimate_ID, c.EST_ComponentType_ID, ad.PST_PostalScenario_ID,
	e.RunDate, c.AdNumber, c.Description, c.PageCount, c.Width, c.Height, pw.Weight,
	c.CreativeCPP, c.CreativeCPP * c.PageCount, c.SeparatorCPP, c.SeparatorCPP * c.PageCount,
	ad.ExternalMailQty, c.SpoilagePct, c.PrintCost, c.RunRate, c.NumberOfPlants,
	c.AdditionalPlates, pl.Rate, c.NumberDigitalHandlenPrepare, dh.Rate,
	case
		when c.StitcherMakeready_ID is null then c.StitcherMakereadyRate
		else smr.Rate
	end StitcherMakereadyRate,
	case
		when c.PressMakeready_ID is null then c.PressMakereadyRate
		else pmr.Rate
	end PressMakereadyRate,
	c.ReplacementPlateCost, c.EarlyPayPrintDiscount,
	c.PrinterTaxableMediaPct, c.PrinterSalesTaxPct, c.PaperCost, c.RunPounds, c.MakeReadyPounds, c.PlateChangePounds, c.NumberOfPressStops,
	c.PressStopPounds, pm.CWT, c.EarlyPayPaperDiscount, pr.PaperHandling, c.PaperTaxableMediaPct, c.PaperSalesTaxPct,
	case
		when c.EST_ComponentType_ID = 1 then ml.InternalListRate
		else null
	end InternalMailCPM,
	case
		when c.EST_ComponentType_ID = 1 then ad.ExternalMailCPM
		else null
	end ExternalMailCPM, c.VendorCPM, c.OtherProduction,
	oi.Rate, si.Rate, bi.Rate,
	case
		when c.EST_ComponentType_ID = 1 then mh.InkJetRate
		else null
	end InkJetRate,
	case
		when c.EST_ComponentType_ID = 1 then mh.InkJetMakeReady
		else null
	end InkJetMakeReady,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then mh.TimeValueSlips
		else null
	end TimeValueSlipsCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then mh.AdminFee
		else null
	end GrossMailHouseAdminFee,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseGlueTack = 1 then mh.GlueTackRate
		else null
	end GlueTackCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseTabbing = 1 then mh.TabbingRate
		else null
	end TabbingCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseLetterInsertion = 1 then mh.LetterInsertionRate
		else null
	end LetterInsertionCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then ad.MailHouseOtherHandling
		else null
	end OtherMailHandlingCPM,
	case
			when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseMailTracking = 1 then mt.MailTracking
			else null
		end MailTrackingCPMRate,
		case
			when c.EST_ComponentType_ID = 1 /*Host*/ then ad.OtherFreight
			else null
		end OtherFreight
from EST_Estimate e join vw_Estimate_excludeOldUploads ve on e.EST_Estimate_ID = ve.EST_Estimate_ID
	join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
	join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
	left join VND_Printer pr on c.Printer_ID = pr.VND_Printer_ID
	left join PRT_PrinterRate pl on c.PlateCost_ID = pl.PRT_PrinterRate_ID
	left join PRT_PrinterRate dh on c.DigitalHandlenPrepare_ID = dh.PRT_PrinterRate_ID
	left join PRT_PrinterRate smr on c.StitcherMakeready_ID = smr.PRT_PrinterRate_ID
	left join PRT_PrinterRate pmr on c.PressMakeready_ID = pmr.PRT_PrinterRate_ID
	left join PPR_Paper_Map pm on c.Paper_Map_ID = pm.PPR_Paper_Map_ID
	left join PRT_PrinterRate si on c.StitchIn_ID = si.PRT_PrinterRate_ID
	left join PRT_PrinterRate bi on c.BlowIn_ID = bi.PRT_PrinterRate_ID
	left join PRT_PrinterRate oi on c.Onsert_ID = oi.PRT_PrinterRate_ID
	left join VND_MailTrackingRate mt on ad.MailTracking_ID = mt.VND_MailTrackingRate_ID
	left join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
	left join VND_MailListResourceRate ml on ad.MailListResource_ID = ml.VND_MailListResourceRate_ID
where (@StartRunDate is null or e.RunDate >= @StartRunDate)
	and (@EndRunDate is null or e.RunDate <= @EndRunDate)
	and (@AdNumber is null or dbo.EstimateHostAdNumber(e.EST_Estimate_ID) = @AdNumber)
	and (@EST_Estimate_ID is null or e.EST_Estimate_ID = @EST_Estimate_ID)
	and (@EST_Status_ID is null or e.EST_Status_ID = @EST_Status_ID)

if (@AdNumber is not null)
	insert into #tempComponent(EST_Component_ID, EST_Estimate_ID, EST_ComponentType_ID, PST_PostalScenario_ID,
		RunDate, AdNumber, Description, PageCount, Width,
		Height, PaperWeight, CreativeCPP, CreativeCost, SeparatorCPP, SeparatorCost,
		ExternalMailQuantity, SpoilagePct, ManualPrintCost, RunRate, NumberOfPlants, AdditionalPlates,
		PrinterPlateCost, NumberDigitalHandlePrepare, DigitalHandlePrepareRate, StitcherMakereadyRate, PressMakereadyRate,
		ReplacementPlateCost, EarlyPayPrintDiscountPercent, PrinterTaxableMediaPct,
		PrinterSalesTaxPct, ManualPaperCost, RunPounds, MakeReadyPounds, PlateChangePounds, NumberOfPressStops, PressStopPounds, PaperCWTRate,
		EarlyPayPaperDiscountPercent, PaperHandlingCWTRate, PaperTaxableMediaPct, PaperSalesTaxPct, InternalMailCPM, ExternalMailCPM,
		VendorProductionCPM, OtherProductionCost, OnsertRate, StitchInRate, BlowInRate, InkJetRate, InkJetMakeReadyRate,
		TimeValueSlipsCPM, GrossMailHouseAdminFee, GlueTackCPM, TabbingCPM, LetterInsertionCPM, OtherMailHandlingCPM,
		MailTrackingCPMRate, GrossOtherFreight)
	select cvrcomp.EST_Component_ID, cvrcomp.EST_Estimate_ID, cvrcomp.EST_ComponentType_ID, ad.PST_PostalScenario_ID,
		tc.RunDate, cvrcomp.AdNumber, cvrcomp.Description, cvrcomp.PageCount, cvrcomp.Width, cvrcomp.Height, pw.Weight,
		cvrcomp.CreativeCPP, cvrcomp.CreativeCPP * cvrcomp.PageCount, cvrcomp.SeparatorCPP, cvrcomp.SeparatorCPP * cvrcomp.PageCount,
		ad.ExternalMailQty, cvrcomp.SpoilagePct, cvrcomp.PrintCost, cvrcomp.RunRate,
		cvrcomp.NumberOfPlants, cvrcomp.AdditionalPlates, pl.Rate, cvrcomp.NumberDigitalHandlenPrepare, dh.Rate,
		case
			when cvrcomp.StitcherMakeready_ID is null then cvrcomp.StitcherMakereadyRate
			else smr.Rate
		end StitcherMakereadyRate,
		case
			when cvrcomp.PressMakeready_ID is null then cvrcomp.PressMakereadyRate
			else pmr.Rate
		end PressMakereadyRate,
		cvrcomp.ReplacementPlateCost, cvrcomp.EarlyPayPrintDiscount,
		cvrcomp.PrinterTaxableMediaPct, cvrcomp.PrinterSalesTaxPct, cvrcomp.PaperCost, cvrcomp.RunPounds, cvrcomp.MakeReadyPounds,
		cvrcomp.PlateChangePounds, cvrcomp.NumberOfPressStops, cvrcomp.PressStopPounds, pm.CWT, cvrcomp.EarlyPayPaperDiscount, pr.PaperHandling,
		cvrcomp.PaperTaxableMediaPct, cvrcomp.PaperSalesTaxPct,
		case
			when cvrcomp.EST_ComponentType_ID = 1 then ml.InternalListRate
			else null
		end InternalMailCPM,
		case
			when cvrcomp.EST_ComponentType_ID = 1 then ad.ExternalMailCPM
			else null
		end ExternalMailCPM, cvrcomp.VendorCPM, cvrcomp.OtherProduction,
		oi.Rate, si.Rate, bi.Rate,
		case
			when cvrcomp.EST_ComponentType_ID = 1 then mh.InkJetRate
			else null
		end InkJetRate,
		case
			when cvrcomp.EST_ComponentType_ID = 1 then mh.InkJetMakeReady
			else null
		end InkJetMakeReady,
		case
			when cvrcomp.EST_ComponentType_ID = 1 /*Host*/ then mh.TimeValueSlips
			else null
		end TimeValueSlipsCPM,
		case
			when cvrcomp.EST_ComponentType_ID = 1 /*Host*/ then mh.AdminFee
			else null
		end GrossMailHouseAdminFee,
		case
			when cvrcomp.EST_ComponentType_ID = 1 /*Host*/ and ad.UseGlueTack = 1 then mh.GlueTackRate
			else null
		end GlueTackCPM,
		case
			when cvrcomp.EST_ComponentType_ID = 1 /*Host*/ and ad.UseLetterInsertion = 1 then mh.LetterInsertionRate
			else null
		end LetterInsertionCPM,
		case
			when cvrcomp.EST_ComponentType_ID = 1 /*Host*/ and ad.UseTabbing = 1 then mh.TabbingRate
			else null
		end TabbingCPM,
		case
			when cvrcomp.EST_ComponentType_ID = 1 /*Host*/ then ad.MailHouseOtherHandling
			else null
		end OtherMailHandlingCPM,
		case
				when cvrcomp.EST_ComponentType_ID = 1 /*Host*/ and ad.UseMailTracking = 1 then mt.MailTracking
				else null
			end MailTrackingCPMRate,
			case
				when cvrcomp.EST_ComponentType_ID = 1 /*Host*/ then ad.OtherFreight
				else null
			end OtherFreight
	from #tempComponent tc join EST_Component hostcomp on tc.EST_Component_ID = hostcomp.EST_Component_ID
		join EST_Component cvrcomp on tc.EST_Estimate_ID = cvrcomp.EST_Estimate_ID
		join PPR_PaperWeight pw on cvrcomp.PaperWeight_ID = pw.PPR_PaperWeight_ID
		join EST_AssemDistribOptions ad on cvrcomp.EST_Estimate_ID = ad.EST_Estimate_ID
		left join VND_Printer pr on cvrcomp.Printer_ID = pr.VND_Printer_ID
		left join PRT_PrinterRate pl on cvrcomp.PlateCost_ID = pl.PRT_PrinterRate_ID
		left join PRT_PrinterRate dh on cvrcomp.DigitalHandlenPrepare_ID = dh.PRT_PrinterRate_ID
		left join PRT_PrinterRate smr on cvrcomp.StitcherMakeready_ID = smr.PRT_PrinterRate_ID
		left join PRT_PrinterRate pmr on cvrcomp.PressMakeready_ID = pmr.PRT_PrinterRate_ID
		left join PPR_Paper_Map pm on cvrcomp.Paper_Map_ID = pm.PPR_Paper_Map_ID
		left join PRT_PrinterRate si on cvrcomp.StitchIn_ID = si.PRT_PrinterRate_ID
		left join PRT_PrinterRate bi on cvrcomp.BlowIn_ID = bi.PRT_PrinterRate_ID
		left join PRT_PrinterRate oi on cvrcomp.Onsert_ID = oi.PRT_PrinterRate_ID
		left join VND_MailTrackingRate mt on ad.MailTracking_ID = mt.VND_MailTrackingRate_ID
		left join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
		left join VND_MailListResourceRate ml on ad.MailListResource_ID = ml.VND_MailListResourceRate_ID
	where hostcomp.EST_ComponentType_ID = 1 and cvrcomp.EST_ComponentType_ID = 5 and cvrcomp.AdNumber is null

/* Perform Specification Calculations */
update #tempComponent
	set PieceWeight = (Width * Height) / cast(950000 as decimal) * PageCount * PaperWeight * 1.03

update tc
	set tc.TotalEstimateWeight = tw.TotalEstimateWeight
from #tempComponent tc join vwTotalEstimateWeight tw on tc.EST_Estimate_ID = tw.EST_Estimate_ID
where tc.EST_ComponentType_ID = 1

/* Perform Quantity Calculations */
update tc
	set InsertQuantity = isnull(iq.InsertQuantity, 0)
from #tempComponent tc left join vwComponentInsertQuantity iq on tc.EST_Component_ID = iq.EST_Component_ID

update tc
	set SoloQuantity = isnull(sq.SoloQuantity, 0)
from #tempComponent tc left join vwComponentSoloQuantity sq on tc.EST_Component_ID = sq.EST_Component_ID

update tc
	set PolybagQuantity = isnull(pq.PolybagQuantity, 0)
from #tempComponent tc left join vwComponentPolybagQuantity pq on tc.EST_Component_ID = pq.EST_Component_ID

update tc
	set OtherQuantity = isnull(oq.OtherQuantity, 0)
from #tempComponent tc left join vwComponentOtherQuantity oq on tc.EST_Component_ID = oq.EST_Component_ID

update tc
	set SampleQuantity = isnull(sq.SampleQuantity, 0)
from #tempComponent tc left join vwComponentSampleQuantity sq on tc.EST_Component_ID = sq.EST_Component_ID

update tc
	set MediaQuantity = tc.InsertQuantity + tc.SoloQuantity + tc.PolybagQuantity
from #tempComponent tc

update #tempComponent
	set SpoilageQuantity = isnull(SpoilagePct, 0) * MediaQuantity,
		DirectMailQuantity = SoloQuantity + PolybagQuantity,
		InternalMailQuantity = DirectMailQuantity - ExternalMailQuantity

update #tempComponent
	set TotalProductionQuantity = MediaQuantity + SpoilageQuantity + OtherQuantity + SampleQuantity

update #tempComponent
	set ProductionWeight = TotalProductionQuantity * PieceWeight

/* Perform Production Calculations */
update #tempComponent
	set GrossPrintCost = ManualPrintCost
where ManualPrintCost is not null

update #tempComponent
	set GrossPrintCost = TotalProductionQuantity / cast(1000 as decimal) * isnull(RunRate, 0)
		+ isnull(NumberOfPlants, 0) * (isnull(StitcherMakeReadyRate, 0) + isnull(PressMakeReadyRate, 0))
		+ isnull(AdditionalPlates, 0) * isnull(PrinterPlateCost, 0)
		+ isnull(NumberDigitalHandlePrepare, 0) * isnull(DigitalHandlePrepareRate, 0)
		+ isnull(ReplacementPlateCost, 0)
where ManualPrintCost is null

update #tempComponent
	set EarlyPayPrintDiscountAmount = GrossPrintCost * isnull(EarlyPayPrintDiscountPercent, 0),
		NetPrintCost = GrossPrintCost - (GrossPrintCost * isnull(EarlyPayPrintDiscountPercent, 0))

/*TODO: We don't have a paper handling amount if manualpapercost is entered.  How does this effect the print sales tax?? */
update #tempComponent
	set GrossPaperCost = ManualPaperCost
where ManualPaperCost is not null

update #tempComponent
	set TotalPaperPounds = TotalProductionQuantity / cast(1000 as decimal) * isnull(RunPounds, 0)
		+ cast(isnull(NumberOfPlants, 0) as bigint) * cast(isnull(MakeReadyPounds, 0) as bigint)
		+ cast(isnull(AdditionalPlates, 0) as bigint) * isnull(PlateChangePounds, 0)
		+ cast(isnull(NumberOfPressStops, 0) as bigint) * cast(isnull(PressStopPounds, 0) as bigint)
where ManualPaperCost is null

update #tempComponent
	set GrossPaperCost = isnull(TotalPaperPounds, 0) / cast(100 as decimal) * isnull(PaperCWTRate, 0)
where ManualPaperCost is null

update #tempComponent
	set EarlyPayPaperDiscountAmount = GrossPaperCost * isnull(EarlyPayPaperDiscountPercent, 0),
		NetPaperCost = GrossPaperCost - (GrossPaperCost * isnull(EarlyPayPaperDiscountPercent, 0))

update #tempComponent
	set PaperHandlingCost = isnull(TotalPaperPounds, 0) / cast(100 as decimal) * isnull(PaperHandlingCWTRate, 0)

update #tempComponent
	set PrinterSalesTaxAmount = (NetPrintCost + PaperHandlingCost) * isnull(PrinterTaxableMediaPct, 0) * isnull(PrinterSalesTaxPct, 0),
		PaperSalesTaxAmount = NetPaperCost * isnull(PaperTaxableMediaPct, 0) * isnull(PaperSalesTaxPct, 0)

update #tempComponent
	set TotalPrintCost = NetPrintCost + PrinterSalesTaxAmount,
		TotalPaperCost = NetPaperCost + PaperHandlingCost + PaperSalesTaxAmount,
		VendorProductionCost = TotalProductionQuantity / cast(1000 as decimal) * VendorProductionCPM

update #tempComponent
	set BlendedMailListCPM =
		case
			when dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) = 0 then 0
			else InternalMailCPM
					* cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) - ExternalMailQuantity as decimal)
					/ cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
				+ ExternalMailCPM
					* cast(ExternalMailQuantity as decimal)
					/ cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
		end
where EST_ComponentType_ID = 1

update #tempComponent
	set MailListCost = BlendedMailListCPM * SoloQuantity / 1000 + isnull(dbo.ComponentPolybagMailListCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponent
	set ProductionTotal = isnull(CreativeCost, 0) + isnull(SeparatorCost, 0) + TotalPrintCost + TotalPaperCost +  isnull(MailListCost, 0) +
		+ isnull(VendorProductionCost, 0) + isnull(OtherProductionCost, 0)
		
update #tempComponent
	set ProductionPieceCost =
		case
			when TotalProductionQuantity = 0 then 0
			else ProductionTotal / cast(TotalProductionQuantity as decimal)
		end

/* Assembly Calculations */

update #tempComponent
	set PolyBagCost = dbo.EstimatePolyBagCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1

update #tempComponent
	set OnsertCost = PolybagQuantity / cast(1000 as decimal) * OnsertRate
where EST_ComponentType_ID = 2

update #tempComponent
	set GrossInkjetMakereadyCost = NumberofPlants * InkjetMakereadyRate

update #tempComponent
	set PolyBagTotal = isnull(PolyBagCost, 0) + isnull(OnsertCost, 0),
		StitchInCost =
			case EST_ComponentType_ID
				when 3 then MediaQuantity / cast(1000 as decimal) * StitchInRate
				else null
			end,
		BlowInCost =
			case EST_ComponentType_ID
				when 4 then MediaQuantity / cast(1000 as decimal) * BlowInRate
			end,
		InkJetCost =
			case EST_ComponentType_ID
				when 1 then (isnull(SoloQuantity, 0) / cast(1000 as decimal) * InkJetRate) + isnull(dbo.EstimatePolybagInkjetCost(EST_Estimate_ID), 0)
			end,
		InkJetMakeReadyCost =
			case EST_ComponentType_ID
				when 1 then
					case
						when isnull(SoloQuantity, 0) = 0 then 0
						else GrossInkjetMakereadyCost * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
					end
					+ isnull(dbo.ComponentPolybagInkjetMakereadyCost(EST_Component_ID), 0)
			end

update #tempComponent
		set TotalInkJetCost = isnull(InkJetCost, 0) + isnull(InkJetMakeReadyCost, 0)

update #tempComponent
	set TimeValueSlipsCost = TimeValueSlipsCPM * isnull(SoloQuantity, 0) / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseTimeValueSlipsCost(EST_Component_ID), 0),
		MailHouseAdminFee =
			case
				when isnull(SoloQuantity, 0) = 0 then 0
				else GrossMailHouseAdminFee * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
			end
			+ isnull(dbo.ComponentPolybagMailHouseAdminFee(EST_Component_ID), 0),
		GlueTackCost = GlueTackCPM * isnull(SoloQuantity, 0) / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseGlueTackCost(EST_Component_ID), 0),
		TabbingCost = TabbingCPM * isnull(SoloQuantity, 0) / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseTabbingCost(EST_Component_ID), 0),
		LetterInsertionCost = LetterInsertionCPM * isnull(SoloQuantity, 0) / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseLetterInsertionCost(EST_Component_ID), 0),
		OtherMailHandlingCost = OtherMailHandlingCPM * isnull(SoloQuantity, 0) / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseOtherDirectMailHandlingCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponent
	set MailHandlingTotal = isnull(TimeValueSlipsCost, 0) + isnull(MailHouseAdminFee, 0) + isnull(GlueTackCost, 0) + isnull(TabbingCost, 0)
			+ isnull(LetterInsertionCost, 0) + isnull(OtherMailHandlingCost, 0)

update #tempComponent
	set AssemblyTotal = PolyBagTotal + isnull(StitchInCost, 0) + isnull(BlowInCost, 0) + TotalInkJetCost + MailHandlingTotal

update #tempComponent
	set AssemblyPieceCost = 
		case
			when DirectMailQuantity = 0 then 0
			else AssemblyTotal / cast(DirectMailQuantity as decimal)
		end

update #tempComponent
	set PostalDropCost = dbo.EstimatePostalDropCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1


update #tempComponent
	set PostalDropFuelSurchargeCost = dbo.EstimatePostalDropFuelSurchargeCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1

update #tempComponent
	set	MailTrackingCost = isnull((SoloQuantity / cast(1000 as decimal) * MailTrackingCPMRate), 0) + isnull(dbo.ComponentPolybagMailTrackingCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponent
	set SoloPostageCost = isnull(dbo.ComponentSoloPostage(EST_Component_ID, PieceWeight, PST_PostalScenario_ID), 0)

update #tempComponent
	set PolyPostageCost = isnull(dbo.ComponentPolyPostage(EST_Component_ID, PieceWeight), 0)

update #tempComponent
	set TotalPostageCost = SoloPostageCost + PolyPostageCost

update #tempComponent
	set PostalTotal = isnull(PostalDropCost, 0) + isnull(PostalDropFuelSurchargeCost, 0) + isnull(MailTrackingCost, 0) + TotalPostageCost

update t
	set SampleFreight =
		case
			when isnull(s.FreightCWT, 0) <> 0 then dbo.EstimatePieceWeight(t.EST_Estimate_ID) * SampleQuantity / cast(100 as decimal) * s.FreightCWT
			else s.FreightFlat
		end
from #tempComponent t join EST_Samples s on t.EST_Estimate_ID = s.EST_Estimate_ID
where t.EST_ComponentType_ID = 1

update #tempComponent
	set OtherFreight = GrossOtherFreight
/* Not sure if this cost should be distributed across Polybags */
/*
			case
				when isnull(SoloQuantity, 0) = 0 then 0
				else GrossOtherFreight * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
			end
			+ isnull(dbo.ComponentPolybagOtherFreightCost(EST_Component_ID), 0)
*/
where EST_ComponentType_ID = 1

update #tempComponent
	set DistributionTotal = PostalTotal + isnull(OtherFreight, 0) + isnull(SampleFreight, 0)

select RunDate, AdNumber, Description, (ProductionPieceCost + AssemblyPieceCost) PieceCost,
	DirectMailQuantity,
	ProductionPieceCost * TotalProductionQuantity + AssemblyTotal as CostWithoutDistribution,
	DistributionTotal,
	ProductionPieceCost * TotalProductionQuantity + AssemblyTotal + DistributionTotal as TotalCost
from #tempComponent
order by RunDate, AdNumber

drop table #tempComponent
GO

GRANT  EXECUTE  ON [dbo].[rpt_DirectMailCosts]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.rpt_EstimateSummary') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.rpt_EstimateSummary'
	DROP PROCEDURE dbo.rpt_EstimateSummary
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.rpt_EstimateSummary') IS NOT NULL
		PRINT '***********Drop of dbo.rpt_EstimateSummary FAILED.'
END
GO
PRINT 'Creating dbo.rpt_EstimateSummary'
GO

CREATE PROC dbo.rpt_EstimateSummary
/*
* PARAMETERS:
* StartRunDate
* EndRunDate
* EST_Estimate_ID
* AdNumber
* EST_Status_ID
* VND_Vendor_ID
* VND_VendorType_ID - If specified limits the query to estimates 
* VendorSupplied - 1 = All Components, 2 = Only VS Components, 3 = Exclude VS Components
* EstimateMediaType - Xml formatted list of media types to query.  If null, all media types are queried.
* ComponentType - Xml formatted list of component types to query.  If null, all component types are queried.
*
* DESCRIPTION:
*		Returns information for matching estimate components.  Used by the Estimate Summary Report and Estimate Element Detail Report.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate
*   EST_Component
*   EST_AssemDistribOptions
*   PPR_PaperWeight
*   PPR_PaperGrade
*   VND_Printer
*   PRT_PrinterRate
*   PPR_Paper_Map
*   VND_MailHouse
*   VND_MailTracker
*
*
* PROCEDURES CALLED:
*   dbo.TotalEstimateWeight
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
* 07/25/2007      BJS             Initial Creation - Based on EstEstimate_s_CostSummary_ByEstimateID
* 09/06/2007      BJS             OnsertRate is now calculated from the appropriate PrinterRate
*                                 Added a check to prevent a divide by zero error when calculating the BlendedMailListCPM
* 09/13/2007      BJS             Replaced calls to Component Quantity functions with views
* 09/24/2007      BJS             Modified Digital H&P, Stitcher Makeready and Press Makeready logic
* 10/08/2007      BJS             Explicitly cast integer values (ie 1000) to prevent rounding errors in calculations
* 10/24/2007      BJS             Join to vnd_printer is now vnd_printer
* 10/31/2007      JRH             Added AssemblyVendor.
* 11/12/2007      JRH             Fixed SampleFreight, InsertFuelSurchargeCost, and InsertFreightCost to allocate all to host.
* 11/14/2007      JRH             MediaQuantity needs to exclude OtherQuantity for calculations 
*                                 but includes OtherQuantity for report.
* 11/16/2007      JRH             Check SampleFreight for null.
* 11/29/2007      JRH             Added CreativeVendor and MailingListVendor
* 12/07/2007      JRH             Fixed the InsertDiscount calculation.
* 12/12/2007      BJS             Insert Costs prorated by tab page count for tab page rate types
* 12/17/2007      JRH             Calculate MediaQuantity from known quantities instead of view.
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
@VND_Vendor_ID bigint,
@VND_VendorType_ID int,
@VendorSupplied tinyint,
@EstimateMediaType varchar(2000),
@ComponentType varchar(2000)
as

set nocount on

declare @MediaTypeDocID int, @ComponentTypeDocID int

create table #tempEstimateMediaType(EST_EstimateMediaType_ID INT NOT NULL, Description VARCHAR(35) NOT NULL)
create table #tempComponentType(EST_ComponentType_ID INT NOT NULL, Description VARCHAR(35) NOT NULL)

if (@EstimateMediaType is null) begin
	insert into #tempEstimateMediaType(EST_EstimateMediaType_ID, Description)
	select EST_EstimateMediaType_ID, Description
	from EST_EstimateMediaType
end
else begin
	exec sp_xml_preparedocument @MediaTypeDocID output, @EstimateMediaType
	insert into #tempEstimateMediaType(EST_EstimateMediaType_ID, Description)
	select emt.EST_EstimateMediaType_ID, emt.Description
	from OPENXML(@MediaTypeDocID, '/root/est_estimatemediatype')
	with(est_estimatemediatype_id INT '@est_estimatemediatype_id') xdata
		join EST_EstimateMediaType emt on xdata.EST_EstimateMediaType_ID = emt.EST_EstimateMediaType_ID
end

if (@ComponentType is null) begin
	insert into #tempComponentType(EST_ComponentType_ID, Description)
	select EST_ComponentType_ID, Description
	from EST_ComponentType
end
else begin
	exec sp_xml_preparedocument @ComponentTypeDocID output, @ComponentType
	insert into #tempComponentType(EST_ComponentType_ID, Description)
	select ct.EST_ComponentType_ID, ct.Description
	from OPENXML(@ComponentTypeDocID, '/root/est_componenttype')
	with(est_componenttype_id INT '@est_componenttype_id') xdata
		join EST_ComponentType ct on xdata.EST_ComponentType_ID = ct.EST_ComponentType_ID
end

/* Comments correlate to layout of Report/Extract #5 - Estimate Summary Report */
create table #tempComponents(
	EST_Component_ID bigint,
	EST_Estimate_ID bigint,
	EST_ComponentType_ID int,
	EST_EstimateMediaType_ID int,
	PST_PostalScenario_ID bigint,
	EstimateMediaType varchar(35),
	ComponentType varchar(35),
	VendorSuppliedDesc varchar(35),
	PrinterVendor varchar(35),
	PaperVendor varchar(35),
	CreativeVendor varchar(35),
	SeparatorVendor varchar(35),
	AssemblyVendor varchar(35),
	MailingHouseVendor varchar(35),
	MailingListVendor varchar(35),
	MailTrackerVendor varchar(35),
	/* Main */
		RunDate datetime,
		AdNumber int,
		Description varchar(50),
	/* Specifications */
		PageCount int,
		Width decimal(10,4),
		Height decimal(10,4),
		/* Paper */
			PaperWeight int,
			PaperGrade varchar(35),
		PieceWeight decimal(12,6), /* (Width * Height) / 950,000 * PageCount * PaperWeight * 1.03 */
	/* Quantity */
		InsertQuantity int,
		SoloQuantity int,
		PolyBagQuantity int,
		OtherQuantity int,
		SampleQuantity int,
		MediaQuantity int, /* Insert + Solo + PolyBag (+ Other only for final report)*/
		SpoilagePct decimal(10,4),
		SpoilageQuantity int, /* SpoilagePct * Media Quantity */
		DirectMailQuantity int, /* Solo + Poly */
		InternalMailQuantity int, /* DirectMail - External List Quantity */
		ExternalMailQuantity int, /* External List Quantity */
		TotalProductionQuantity int, /* MediaQuantity + SpoilageQuantity + OtherQuantity + SampleQuantity */
	/* Production */
		ProductionWeight decimal(14,6), /* TotalProductionQuantity * PieceWeight */
		TotalEstimateWeight decimal(14,6), /* sum(ProductionWeight) group by EST_Estimate_ID*/
		CreativeCPP money,
		CreativeCost money,
		SeparatorCPP money,
		SeparatorCost money,
		/* Print */
			ManualPrintCost money,
			RunRate money,
			NumberOfPlants int,
			AdditionalPlates int,
			PrinterPlateCost money,
			NumberDigitalHandlePrepare int,
			DigitalHandlePrepareRate money,
			StitcherMakeReadyRate money,
			PressMakeReadyRate money,
			ReplacementPlateCost money,
			GrossPrintCost money, /* See Estimate Calculation Steps for formula */
			EarlyPayPrintDiscountPercent decimal(10,4),
			EarlyPayPrintDiscountAmount money, /* GrossPrintCost * EarlyPayPrintDiscountPercent */
			NetPrintCost money, /* GrossPrintCost - EarlyPayPrintDiscountAmount */
			PrinterTaxableMediaPct decimal(10,4),
			PrinterSalesTaxPct decimal(10,4),
			PrinterSalesTaxAmount money, /* (NetPrinterCost + PaperHandling) * PrinterTaxableMediaPct * PrinterSalesTaxPct */
			TotalPrintCost money, /* NetPrintCost + PrinterSalesTaxAmount */
		/* Paper */
			ManualPaperCost money,
			RunPounds decimal(10,2),
			MakereadyPounds int,
			PlateChangePounds decimal(10,2),
			NumberOfPressStops int,
			PressStopPounds int,
			TotalPaperPounds decimal(20,2), /* See Estimate Calculation Steps for formula */
			PaperCWTRate money, /* PPR_Paper_Map.CWT */
			GrossPaperCost money, /* TotalPaperPounds / 100 * PaperCWTRate */
			EarlyPayPaperDiscountPercent decimal(10,4),
			EarlyPayPaperDiscountAmount money, /* GrossPaperCost * EarlyPayPaperDiscountPercent */
			NetPaperCost money, /* GrossPaperCost - EarlyPayPaperDiscountAmount */
			PaperHandlingCWTRate money, /* VND_Printer.PaperHandling */
			PaperHandlingCost money, /* TotalPaperPounds * PaperHandlingCWTRate */
			PaperTaxableMediaPct decimal(10,4),
			PaperSalesTaxPct decimal(10,4),
			PaperSalesTaxAmount money, /* NetPaperCost * PaperTaxableMediaPct * PaperSalesTaxPct */
			TotalPaperCost money, /* NetPaperCost + PaperSalesTaxAmount */
		/* Mail List */
			InternalMailCPM money,
			ExternalMailCPM money,
			BlendedMailListCPM money,
			MailListCost money, /* See Logic below */
		ProductionSalesTaxAmount money,
		VendorProductionCPM money,
		VendorProductionCost money, /* TotalProductionQuantity / 1000 * VendorProductionCPM  */
		OtherProductionCost money,
		ProductionTotal money, /* CreativeCost + SeparatorCost + TotalPrintCost + TotalPaperCost + ExternalMailCost + VendorProductionCost + OtherProductionCost */
	/* Assembly */
		/* PolyBag */
			PolyBagCost money, /* dbo.EstimatePolyBagCost */
			OnsertRate money,
			OnsertCost money, /* Only if component type is Host Onsert -- PolyBagQuantity / 1000 * OnsertRate */
			PolyBagTotal money, /* PolyBagCost + OnsertCost */
		StitchInRate money,
		StitchInCost money, /* StitchInRate * MediaQuantity (if comp type is stitch-in) / 1000 */
		BlowInRate money,
		BlowInCost money, /* BlowInRate * MediaQuantity (if comp type is blow-in) / 1000 */
		/* Ink Jet */
			InkJetRate money,
			InkJetMakeReadyRate money,
			InkJetCost money, /* DirectMailQuantity / 1000 * InkJetRate */
			GrossInkjetMakereadyCost money, /* InkJetMakeReadyRate * NumberOfPlants */
			InkjetMakereadyCost money, /* See Logic below */
			TotalInkJetCost money, /* InkJetCost + InkJetMakeReadyCost */
		/* Handling */
			/* Insert */
				CornerGuardRate money,
				CornerGuardCost money, /* (MediaQuantity * PieceWeight) / 2000 * CornerGuardRate */
				SkidRate money,
				SkidCost money, /* (MediaQuantity * PieceWeight) / 2000 * SkidRate */
				NumberOfCartons int,
				CartonRate money,
				CartonCost money, /* NumberOfCartons * CartonCost */
				InsertHandlingTotal money, /* CornerGuardCost + SkidCost + CartonCost */
			/* Mail */
				TimeValueSlipsCPM money,
				TimeValueSlipsCost money, /* See SQL Logic Below */
				GrossMailHouseAdminFee money, /* The Mail House Admin Fee. */
				MailHouseAdminFee money, /* See SQL Logic Below */
				GlueTackCPM money,
				GlueTackCost money, /* See SQL Logic Below */
				TabbingCPM money,
				TabbingCost money, /* See SQL Logic Below */
				LetterInsertionCPM money,
				LetterInsertionCost money, /* See SQL Logic Below */
				OtherMailHandlingCPM money,
				OtherMailHandlingCost money, /* See SQL Logic Below */
				MailHandlingTotal money, /* See SQL Logic Below */
			HandlingTotal money, /* InsertHandlingTotal + MailHandlngTotal */
		AssemblyTotal money, /* PolyBagTotal + StitchInCost + BlowInCost + TotalInkJetCost + HandlingTotal */
	/* Distribution */
		/* Insert */
			InsertGrossCost money, 
			InsertDiscountPercent decimal(10,4), 
			InsertDiscount money, 
			InsertCost money, /* See Logic below */
			InsertFreightCWT money,
			InsertFreightCost money, /* TotalEstimateWeight / 100 * InsertFreightCWT */
			InsertFuelSurchargePercent decimal(10,4),
			InsertFuelSurchargeCost money, /* InsertFreightCost * InsertFreightSurchargePercent */
			InsertFreightTotalCost money,
			InsertTotal money, /* InsertCost + InsertFreightCost + InsertFuelSurchargeCost */
		/* Postal */
			PostalDropCost money, /* See Logic Below */
			PostalDropFuelSurchargeCost money, /* See Logic Below*/
			TotalPostalDropCost money,
			MailTrackingCPMRate money,
			MailTrackingCost money, /* DirectMailQuantity / 1000 * MailTrackingCPMRate */
			SoloPostageCost money,
			PolyPostageCost money,
			TotalPostageCost money, /* SoloPostageCost + PolyPostageCost */
			PostalTotal money, /*Postage PostalDropCost + PostalFuelSurcharge + MailTrackingCost + TotalPostageCost */
		SampleFreight money,
		GrossOtherFreight money,
		OtherFreight money,
		DistributionTotal money, /* InsertTotal + PostalTotal + SampleFreight + OtherFreight */ 			
	GrandTotal money
)

/* Get Raw Production Data */
insert into #tempComponents(EST_Component_ID, EST_Estimate_ID, EST_ComponentType_ID, EST_EstimateMediaType_ID,
	PST_PostalScenario_ID, EstimateMediaType, ComponentType,
	VendorSuppliedDesc, PrinterVendor, PaperVendor, CreativeVendor, SeparatorVendor, AssemblyVendor, MailingHouseVendor, MailingListVendor, MailTrackerVendor,
	RunDate, AdNumber, Description, PageCount, Width, Height, PaperWeight, PaperGrade, CreativeCPP, CreativeCost, SeparatorCPP, SeparatorCost,
	ExternalMailQuantity, SpoilagePct, ManualPrintCost, RunRate, NumberOfPlants, AdditionalPlates,
	PrinterPlateCost, NumberDigitalHandlePrepare, DigitalHandlePrepareRate, StitcherMakereadyRate, PressMakereadyRate,
	ReplacementPlateCost, EarlyPayPrintDiscountPercent, PrinterTaxableMediaPct,
	PrinterSalesTaxPct, ManualPaperCost, RunPounds, MakeReadyPounds, PlateChangePounds, NumberOfPressStops, PressStopPounds, PaperCWTRate,
	EarlyPayPaperDiscountPercent, PaperHandlingCWTRate, PaperTaxableMediaPct, PaperSalesTaxPct, InternalMailCPM, ExternalMailCPM,
	VendorProductionCPM, OtherProductionCost, OnsertRate, StitchInRate, BlowInRate, InkJetRate, InkJetMakeReadyRate, CornerGuardRate, SkidRate,
	NumberOfCartons, TimeValueSlipsCPM, GrossMailHouseAdminFee, GlueTackCPM, TabbingCPM, LetterInsertionCPM, OtherMailHandlingCPM,
	InsertFreightCWT, InsertFuelSurchargePercent, MailTrackingCPMRate, GrossOtherFreight)
select c.EST_Component_ID, c.EST_Estimate_ID, c.EST_ComponentType_ID, c.EST_EstimateMediaType_ID,
	ad.PST_PostalScenario_ID, emt.Description, ct.Description,
	vs_vnd.Description, prt_vnd.Description, ppr_vnd.Description, crtv_vnd.Description, sep_vnd.Description, asmbly_vnd.Description, mh_vnd.Description, mlr_vnd.Description, mt_vnd.Description,
	e.RunDate, c.AdNumber, c.Description, c.PageCount, c.Width, c.Height, pw.Weight, pg.Grade,
	c.CreativeCPP, c.CreativeCPP * c.PageCount, c.SeparatorCPP, c.SeparatorCPP * c.PageCount,
	ad.ExternalMailQty, c.SpoilagePct, c.PrintCost, c.RunRate, c.NumberOfPlants, c.AdditionalPlates, pl.Rate, c.NumberDigitalHandlenPrepare,
	dh.Rate,
	case
		when c.StitcherMakeready_ID is null then c.StitcherMakereadyRate
		else smr.Rate
	end StitcherMakereadyRate,
	case
		when c.PressMakeready_ID is null then c.PressMakereadyRate
		else pmr.Rate
	end PressMakereadyRate,
	c.ReplacementPlateCost, c.EarlyPayPrintDiscount,
	c.PrinterTaxableMediaPct, c.PrinterSalesTaxPct, c.PaperCost, c.RunPounds, c.MakeReadyPounds, c.PlateChangePounds, c.NumberOfPressStops,
	c.PressStopPounds, pm.CWT, c.EarlyPayPaperDiscount, p.PaperHandling, c.PaperTaxableMediaPct, c.PaperSalesTaxPct,
	case
		when c.EST_ComponentType_ID = 1 then ml.InternalListRate
		else null
	end InternalMailCPM,
	case
		when c.EST_ComponentType_ID = 1 then ad.ExternalMailCPM
		else null
	end ExternalMailCPM, c.VendorCPM, c.OtherProduction,
	oi.Rate, si.Rate, bi.Rate,
	case
		when c.EST_ComponentType_ID = 1 then mh.InkJetRate
		else null
	end InkJetRate,
	case
		when c.EST_ComponentType_ID = 1 then mh.InkJetMakeReady
		else null
	end InkJetMakeReady,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.CornerGuards = 1 then p.CornerGuard
		else null
	end CornerGuardRate,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.Skids = 1 then p.Skid
		else null
	end SkidRate,
	ad.NbrOfCartons,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then mh.TimeValueSlips
		else null
	end TimeValueSlipsCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then mh.AdminFee
		else null
	end GrossMailHouseAdminFee,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseGlueTack = 1 then mh.GlueTackRate
		else null
	end GlueTackCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseTabbing = 1 then mh.TabbingRate
		else null
	end TabbingCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseLetterInsertion = 1 then mh.LetterInsertionRate
		else null
	end LetterInsertionCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then ad.MailHouseOtherHandling
		else null
	end OtherMailHandlingCPM,
	ad.InsertFreightCWT, ad.InsertFuelSurcharge,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseMailTracking = 1 then mt.MailTracking
		else null
	end MailTrackingCPMRate,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then ad.OtherFreight
		else null
	end OtherFreight
from EST_Estimate e join vw_Estimate_excludeOldUploads eo on e.EST_Estimate_ID = eo.EST_Estimate_ID
	join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join #tempEstimateMediaType emt on c.EST_EstimateMediaType_ID = emt.EST_EstimateMediaType_ID
	join #tempComponentType ct on c.EST_ComponentType_ID = ct.EST_ComponentType_ID
	join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
	join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
	left join PPR_PaperGrade pg on c.PaperGrade_ID = pg.PPR_PaperGrade_ID
	left join VND_Vendor vs_vnd on c.VendorSupplied_ID = vs_vnd.VND_Vendor_ID
	left join VND_Vendor sep_vnd on c.Separator_ID = sep_vnd.VND_Vendor_ID
	left join VND_Vendor crtv_vnd on c.CreativeVendor_ID = crtv_vnd.VND_Vendor_ID
	left join VND_Vendor asmbly_vnd on c.assemblyvendor_id = asmbly_vnd.VND_Vendor_ID
	left join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	left join VND_Vendor prt_vnd on p.VND_Vendor_ID = prt_vnd.VND_Vendor_ID
	left join VND_Paper ppr on c.Paper_ID = ppr.VND_Paper_ID
	left join VND_Vendor ppr_vnd on ppr.VND_Vendor_ID = ppr_vnd.VND_Vendor_ID
	left join PRT_PrinterRate pl on c.PlateCost_ID = pl.PRT_PrinterRate_ID /*Plates*/
	left join PRT_PrinterRate dh on c.DigitalHandlenPrepare_ID = dh.PRT_PrinterRate_ID /* Digi H&P */
	left join PRT_PrinterRate smr on c.StitcherMakeready_ID = smr.PRT_PrinterRate_ID /* Stitcher Makeready */
	left join PRT_PrinterRate pmr on c.PressMakeready_ID = pmr.PRT_PrinterRate_ID /* Press Makeready */	
	left join PPR_Paper_Map pm on c.Paper_Map_ID = pm.PPR_Paper_Map_ID
	left join PRT_PrinterRate si on c.StitchIn_ID = si.PRT_PrinterRate_ID
	left join PRT_PrinterRate bi on c.BlowIn_ID = bi.PRT_PrinterRate_ID
	left join PRT_PrinterRate oi on c.Onsert_ID = oi.PRT_PrinterRate_ID
	left join VND_MailTrackingRate mt on ad.MailTracking_ID = mt.VND_MailTrackingRate_ID
	left join VND_Vendor mt_vnd on mt.VND_Vendor_ID = mt_vnd.VND_Vendor_ID
	left join VND_MailListResourceRate mlr on ad.MailListResource_ID = mlr.VND_MailListResourceRate_ID
	left join VND_Vendor mlr_vnd on mlr.VND_Vendor_ID = mlr_vnd.VND_Vendor_ID
	join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
	join VND_Vendor mh_vnd on mh.VND_Vendor_ID = mh_vnd.VND_Vendor_ID
	left join VND_MailListResourceRate ml on ad.MailListResource_ID = ml.VND_MailListResourceRate_ID
where (@StartRunDate is null or e.RunDate >= @StartRunDate)
	and (@EndRunDate is null or e.RunDate <= @EndRunDate)
	and (@EST_Estimate_ID is null or e.EST_Estimate_ID = @EST_Estimate_ID)
	and (@AdNumber is null or dbo.EstimateHostAdNumber(e.EST_Estimate_ID) = @AdNumber)
	and (@EST_Status_ID is null or e.EST_Status_ID = @EST_Status_ID)
	and ((@VND_Vendor_ID is null and @VND_VendorType_ID is null)
		or (@VND_VendorType_ID = 1 and p.VND_Vendor_ID = @VND_Vendor_ID)
		or (@VND_VendorType_ID = 2 and ppr.VND_Vendor_ID = @VND_Vendor_ID)
		or (@VND_VendorType_ID = 3 and c.CreativeVendor_ID = @VND_Vendor_ID)
		or (@VND_VendorType_ID = 4 and c.Separator_ID = @VND_Vendor_ID)
		or (@VND_VendorType_ID = 5 and mh.VND_Vendor_ID = @VND_Vendor_ID)
		or (@VND_VendorType_ID = 6 and ml.VND_Vendor_ID = @VND_Vendor_ID)
		or (@VND_VendorType_ID = 7 and mt.VND_Vendor_ID = @VND_Vendor_ID)
		or (@VND_VendorType_ID = 8 and dbo.IsEstimateUsingPostalVendor(e.EST_Estimate_ID, @VND_Vendor_ID) = 1)
		or (@VND_VendorType_ID = 9 and c.VendorSupplied_ID = @VND_Vendor_ID)
		or (@VND_VendorType_ID = 10 and ad.InsertFreightVendor_ID = @VND_Vendor_ID))
	and (@VendorSupplied = 1 or (@VendorSupplied = 2 and c.VendorSupplied = 1) or (@VendorSupplied = 3 and c.VendorSupplied = 0))

/* Perform Specification Calculations */
update #tempComponents
	set PieceWeight = (Width * Height) / cast(950000 as decimal) * PageCount * PaperWeight * 1.03

/* Perform Quantity Calculations */
update tc
	set InsertQuantity = isnull(iq.InsertQuantity, 0)
from #tempComponents tc left join vwComponentInsertQuantity iq on tc.EST_Component_ID = iq.EST_Component_ID

update tc
	set SoloQuantity = isnull(sq.SoloQuantity, 0)
from #tempComponents tc left join vwComponentSoloQuantity sq on tc.EST_Component_ID = sq.EST_Component_ID

update tc
	set PolyBagQuantity = isnull(pq.PolybagQuantity, 0)
from #tempComponents tc left join vwComponentPolybagQuantity pq on tc.EST_Component_ID = pq.EST_Component_ID

update tc
	set OtherQuantity = isnull(oq.OtherQuantity, 0)
from #tempComponents tc left join vwComponentOtherQuantity oq on tc.EST_Component_ID = oq.EST_Component_ID

update tc
	set SampleQuantity = isnull(sq.SampleQuantity, 0)
from #tempComponents tc left join vwComponentSampleQuantity sq on tc.EST_Component_ID = sq.EST_Component_ID

update tc
	set MediaQuantity = tc.InsertQuantity + tc.SoloQuantity + tc.PolyBagQuantity
from #tempComponents tc

update #tempComponents
	set SpoilageQuantity = isnull(SpoilagePct, 0) * MediaQuantity,
		DirectMailQuantity = SoloQuantity + PolybagQuantity,
		InternalMailQuantity = (SoloQuantity + PolybagQuantity) - ExternalMailQuantity

update #tempComponents
	set TotalProductionQuantity = MediaQuantity + SpoilageQuantity + OtherQuantity + SampleQuantity

update tc
	set tc.TotalEstimateWeight = tw.TotalEstimateWeight
from #tempComponents tc join vwTotalEstimateWeight tw on tc.EST_Estimate_ID = tw.EST_Estimate_ID
where tc.EST_ComponentType_ID = 1

update #tempComponents
	set ProductionWeight = TotalProductionQuantity * PieceWeight

/* Perform Production Calculations */
update #tempComponents
	set GrossPrintCost = ManualPrintCost
where ManualPrintCost is not null

update #tempComponents
	set GrossPrintCost = TotalProductionQuantity / cast(1000 as decimal) * isnull(RunRate, 0)
		+ isnull(NumberOfPlants, 0) * (isnull(StitcherMakeReadyRate, 0) + isnull(PressMakeReadyRate, 0))
		+ isnull(AdditionalPlates, 0) * isnull(PrinterPlateCost, 0)
		+ isnull(NumberDigitalHandlePrepare, 0) * isnull(DigitalHandlePrepareRate, 0)
		+ isnull(ReplacementPlateCost, 0)
where ManualPrintCost is null

update #tempComponents
	set EarlyPayPrintDiscountAmount = GrossPrintCost * isnull(EarlyPayPrintDiscountPercent, 0),
		NetPrintCost = GrossPrintCost - (GrossPrintCost * isnull(EarlyPayPrintDiscountPercent, 0))

/*TODO: We don't have a paper handling amount if manualpapercost is entered.  How does this effect the print sales tax?? */
update #tempComponents
	set GrossPaperCost = ManualPaperCost
where ManualPaperCost is not null

update #tempComponents
	set TotalPaperPounds = TotalProductionQuantity / cast(1000 as decimal) * isnull(RunPounds, 0)
		+ cast(isnull(NumberOfPlants, 0) as bigint) * cast(isnull(MakeReadyPounds, 0) as bigint)
		+ cast(isnull(AdditionalPlates, 0) as bigint) * isnull(PlateChangePounds, 0)
		+ cast(isnull(NumberOfPressStops, 0) as bigint) * cast(isnull(PressStopPounds, 0) as bigint)
where ManualPaperCost is null

update #tempComponents
	set GrossPaperCost = isnull(TotalPaperPounds, 0) / cast(100 as decimal) * isnull(PaperCWTRate, 0)
where ManualPaperCost is null

update #tempComponents
	set EarlyPayPaperDiscountAmount = GrossPaperCost * isnull(EarlyPayPaperDiscountPercent, 0),
		NetPaperCost = GrossPaperCost - (GrossPaperCost * isnull(EarlyPayPaperDiscountPercent, 0))

update #tempComponents
	set PaperHandlingCost = isnull(TotalPaperPounds, 0) / cast(100 as decimal) * isnull(PaperHandlingCWTRate, 0)

update #tempComponents
	set PrinterSalesTaxAmount = (NetPrintCost + PaperHandlingCost) * isnull(PrinterTaxableMediaPct, 0) * isnull(PrinterSalesTaxPct, 0),
		PaperSalesTaxAmount = NetPaperCost * isnull(PaperTaxableMediaPct, 0) * isnull(PaperSalesTaxPct, 0)

update #tempComponents
	set ProductionSalesTaxAmount = PrinterSalesTaxAmount + PaperSalesTaxAmount

update #tempComponents
	set TotalPrintCost = NetPrintCost + PrinterSalesTaxAmount,
		TotalPaperCost = NetPaperCost + PaperHandlingCost + PaperSalesTaxAmount,
		VendorProductionCost = TotalProductionQuantity / cast(1000 as decimal) * VendorProductionCPM

update #tempComponents
	set BlendedMailListCPM =
		case
			when dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) = 0 then 0
			else InternalMailCPM
					* cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) - ExternalMailQuantity as decimal)
					/ cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
				+ ExternalMailCPM
					* cast(ExternalMailQuantity as decimal)
					/ cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
		end
where EST_ComponentType_ID = 1

update #tempComponents
	set MailListCost = BlendedMailListCPM * SoloQuantity / 1000 + isnull(dbo.ComponentPolybagMailListCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponents
	set ProductionTotal = isnull(CreativeCost, 0) + isnull(SeparatorCost, 0) + TotalPrintCost + TotalPaperCost +  isnull(MailListCost, 0) +
		+ isnull(VendorProductionCost, 0) + isnull(OtherProductionCost, 0)

/* Assembly Calculations */

update #tempComponents
	set PolyBagCost = dbo.EstimatePolyBagCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1

update #tempComponents
	set OnsertCost = PolybagQuantity / cast(1000 as decimal) * OnsertRate
where EST_ComponentType_ID = 2

update #tempComponents
	set GrossInkjetMakereadyCost = NumberofPlants * InkjetMakereadyRate

update #tempComponents
	set PolyBagTotal = isnull(PolyBagCost, 0) + isnull(OnsertCost, 0),
		StitchInCost =
			case EST_ComponentType_ID
				when 3 then MediaQuantity / cast(1000 as decimal) * StitchInRate
				else null
			end,
		BlowInCost =
			case EST_ComponentType_ID
				when 4 then MediaQuantity / cast(1000 as decimal) * BlowInRate
			end,
		InkJetCost =
			case EST_ComponentType_ID
				when 1 then (SoloQuantity / cast(1000 as decimal) * InkJetRate) + isnull(dbo.EstimatePolybagInkjetCost(EST_Estimate_ID), 0)
			end,
		InkJetMakeReadyCost =
			case EST_ComponentType_ID
				when 1 then
					case
						when SoloQuantity = 0 then 0
						else GrossInkjetMakereadyCost * SoloQuantity / dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID)
					end
					+ isnull(dbo.ComponentPolybagInkjetMakereadyCost(EST_Component_ID), 0)
			end

update #tempComponents
		set TotalInkJetCost = isnull(InkJetCost, 0) + isnull(InkJetMakeReadyCost, 0)

update tc
	set CartonRate = pr.Rate
from #tempComponents tc join EST_Component c on tc.EST_Component_ID = c.EST_Component_ID
	join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate pr on p.VND_Printer_ID = pr.VND_Printer_ID and pr.PRT_PrinterRateType_ID = 3 and pr.[Default] = 1

update #tempComponents
	set CornerGuardCost = TotalEstimateWeight / cast(2000 as decimal) * CornerGuardRate,
		SkidCost = TotalEstimateWeight / cast(2000 as decimal) * SkidRate,
		CartonCost = NumberOfCartons * CartonRate,
		TimeValueSlipsCost = TimeValueSlipsCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseTimeValueSlipsCost(EST_Component_ID), 0),
		MailHouseAdminFee =
			case
				when SoloQuantity = 0 then 0
				else GrossMailHouseAdminFee * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
			end
			+ isnull(dbo.ComponentPolybagMailHouseAdminFee(EST_Component_ID), 0),
		GlueTackCost = GlueTackCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseGlueTackCost(EST_Component_ID), 0),
		TabbingCost = TabbingCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseTabbingCost(EST_Component_ID), 0),
		LetterInsertionCost = LetterInsertionCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseLetterInsertionCost(EST_Component_ID), 0),
		OtherMailHandlingCost = OtherMailHandlingCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseOtherDirectMailHandlingCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponents
	set InsertHandlingTotal = isnull(CornerGuardCost, 0) + isnull(SkidCost, 0) + isnull(CartonCost, 0),
		MailHandlingTotal = isnull(TimeValueSlipsCost, 0) + isnull(MailHouseAdminFee, 0) + isnull(GlueTackCost, 0) + isnull(TabbingCost, 0)
			+ isnull(LetterInsertionCost, 0) + isnull(OtherMailHandlingCost, 0)

update #tempComponents
	set HandlingTotal = InsertHandlingTotal + MailHandlingTotal

update #tempComponents
	set AssemblyTotal = PolyBagTotal + isnull(StitchInCost, 0) + isnull(BlowInCost, 0) + TotalInkJetCost + HandlingTotal

/* Distribution Calculations */

create table #tempPackagesByPubRate(
	EST_Component_ID bigint not null,
	EST_Package_ID bigint not null,
	PubRate_Map_ID bigint not null,
	PUB_PubRate_ID bigint not null,
	PUB_RateType_ID int not null,
	PUB_PubQuantity_ID bigint not null,
	QuantityType int not null,
	InsertDate datetime not null,
	BlowInRate int null,
	ComponentTabPageCount int null,
	PackageTabPageCount int null,
	ComponentPieceWeight decimal(12,6) not null,
	PackageWeight decimal(12,6) not null,
	PackageSize int not null,
	BilledQuantity int,
	GrossPackageInsertCost money null,
	InsertDiscountPercent decimal(10,4),
	GrossPackageComponentCost money null,
	PackageComponentInsertDiscount money null,
	NetPackageComponentCost money null)

insert into #tempPackagesByPubRate(EST_Component_ID, EST_Package_ID, PubRate_Map_ID, PUB_PubRate_ID, PUB_RateType_ID,
	PUB_PubQuantity_ID, QuantityType,
	InsertDate, BlowInRate, ComponentPieceWeight, PackageWeight, PackageSize)
select pcm.EST_Component_ID, p.EST_Package_ID, pprm.PUB_PubRate_Map_ID, pr.PUB_PubRate_ID, pr.PUB_RateType_ID,
	dbo.CalcPubQuantityID(pprm.PUB_PubRate_Map_ID, pid.IssueDate), p.PUB_PubQuantityType_ID, pid.IssueDate,
	pr.BlowInRate, tc.PieceWeight, dbo.PackageWeight(p.EST_Package_ID), dbo.PackageSize(p.EST_Package_ID)
from #tempComponents tc join EST_PackageComponentMapping pcm on tc.EST_Component_ID = pcm.EST_Component_ID
	join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	join PUB_PubGroup pg on p.PUB_PubGroup_ID = pg.PUB_PubGroup_ID
	join PUB_PubPubGroup_Map ppgm on pg.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID
	join PUB_PubRate_Map pprm on ppgm.PUB_PubRate_Map_ID = pprm.PUB_PubRate_Map_ID
	join PUB_PubRate pr on pprm.PUB_PubRate_Map_ID = pr.PUB_PubRate_Map_ID
	join EST_PubIssueDates pid on p.EST_Estimate_ID = pid.EST_Estimate_ID and pprm.PUB_PubRate_Map_ID = pid.PUB_PubRate_Map_ID
where dbo.IsPubRateMapActive(pprm.PUB_PubRate_Map_ID, pid.IssueDate) = 1
	and dbo.CalcPubRateID(pprm.PUB_PubRate_Map_ID, pid.IssueDate) = pr.PUB_PubRate_ID
	and dbo.CalcPubQuantityID(pprm.PUB_PubRate_Map_ID, pid.IssueDate) is not null

update tp
	set ComponentTabPageCount =
		case
			when tc.EST_EstimateMediaType_ID = 2 then 2 -- Broadsheet
			else 1
		end
		*
		case
			when tc.EST_ComponentType_ID = 4 and tp.BlowInRate = 0 then 0 --Blow-In not charged
			when tc.EST_ComponentType_ID = 4 and tp.BlowInRate = 1 then cast(tc.PageCount as decimal) / 2 -- Blow-In charged at 1/2 page
			else tc.PageCount
		end
from #tempComponents tc join #tempPackagesByPubRate tp on tc.EST_Component_ID = tp.EST_Component_ID

update #tempPackagesByPubRate
	set PackageTabPageCount = dbo.PackageTabPageCount(EST_Package_ID, BlowInRate)

update #tempPackagesByPubRate
	set BilledQuantity =
		case pr.QuantityChargeType
			when 1 then q.Quantity - (q.Quantity * pr.BilledPct)
			else q.Quantity
		end
from #tempPackagesByPubRate t join PUB_DayOfWeekQuantity q on t.PUB_PubQuantity_ID = q.PUB_PubQuantity_ID and t.QuantityType = q.PUB_PubQuantityType_ID
		and (t.QuantityType > 3 /*Holidays*/ or datepart(dw, t.InsertDate) = q.InsertDow /*Full Run / Contract Send*/) 
	join PUB_PubRate pr on t.PUB_PubRate_ID = pr.PUB_PubRate_ID

update #tempPackagesByPubRate
	set GrossPackageInsertCost = dbo.CalcGrossInsertCostforPackageandPub(PUB_PubRate_ID, InsertDate,
		case QuantityType
			when 4 then 1 /* Thanksgiving always uses Sunday rates */
			when 5 then 1 /* Christmas always uses Sunday rates */
			when 6 then 1 /* New Years always uses Sunday rates */
			else datepart(dw, InsertDate)
		end,
		PackageTabPageCount, BilledQuantity, PackageWeight, PackageSize)

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
	set InsertDiscountPercent = d.Discount
from #tempPackagesByPubRate t join #tempPackageInsertNumber pin on t.EST_Package_ID = pin.EST_Package_ID and t.PubRate_Map_ID = pin.PUB_PubRate_Map_ID
	join PUB_InsertDiscounts d on t.PUB_PubRate_ID = d.PUB_PubRate_ID
where pin.InsertNumber = d.[Insert]

drop table #tempPackageInsertNumber

/*----------------------------------------- */

--Tab Page Count Rate Type
update tp
	set GrossPackageComponentCost =
			case
				when PackageWeight = 0 then 0
				else
					(GrossPackageInsertCost * ComponentTabPageCount / PackageTabPageCount)
			end,
		PackageComponentInsertDiscount =
			case
				when PackageWeight = 0 then 0
				else
					(GrossPackageInsertCost * isnull(InsertDiscountPercent, 0)) * ComponentTabPageCount / PackageTabPageCount
			end,
		NetPackageComponentCost =
			case
				when PackageWeight = 0 then 0
				else
					(GrossPackageInsertCost * (1 - isnull(InsertDiscountPercent, 0))) * ComponentTabPageCount / PackageTabPageCount
			end 
from #tempPackagesByPubRate tp
where tp.PUB_RateType_ID = 1

--Insertion Costs with other Rate Types are prorated to Components by component piece weight
update tp
	set GrossPackageComponentCost =
			case
				when PackageWeight = 0 then 0
				else
					(GrossPackageInsertCost * ComponentPieceWeight / PackageWeight)
			end,
		PackageComponentInsertDiscount =
			case
				when PackageWeight = 0 then 0
				else
					(GrossPackageInsertCost * isnull(InsertDiscountPercent, 0)) * ComponentPieceWeight / PackageWeight
			end,
		NetPackageComponentCost =
			case
				when PackageWeight = 0 then 0
				else
					(GrossPackageInsertCost * (1 - isnull(InsertDiscountPercent, 0))) * ComponentPieceWeight / PackageWeight
			end 
from #tempPackagesByPubRate tp
where tp.PUB_RateType_ID <> 1

update tc
	set InsertGrossCost = isnull((select sum(GrossPackageInsertCost) from #tempPackagesByPubRate tp where tp.EST_Component_ID = tc.EST_Component_ID), 0)
	, InsertDiscount = isnull((select sum(PackageComponentInsertDiscount) from #tempPackagesByPubRate tp where tp.EST_Component_ID = tc.EST_Component_ID), 0)
	, InsertCost = isnull((select sum(NetPackageComponentCost) from #tempPackagesByPubRate tp where tp.EST_Component_ID = tc.EST_Component_ID), 0)
from #tempComponents tc

drop table #tempPackagesByPubRate

update #tempComponents
	set InsertFreightCost = dbo.TotalEstimateWeight(#tempComponents.EST_Estimate_ID) / cast(100 as decimal) * InsertFreightCWT,
		InsertFuelSurchargeCost = (dbo.TotalEstimateWeight(#tempComponents.EST_Estimate_ID) / cast(100 as decimal) * InsertFreightCWT) * InsertFuelSurchargePercent
where #tempComponents.EST_ComponentType_ID = 1

update #tempComponents
	set InsertFreightTotalCost = isnull(InsertFreightCost, 0) + isnull(InsertFuelSurchargeCost, 0)

update #tempComponents
	set InsertTotal = InsertCost + InsertFreightTotalCost

update #tempComponents
	set PostalDropCost = dbo.EstimatePostalDropCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1

update #tempComponents
	set PostalDropFuelSurchargeCost = dbo.EstimatePostalDropFuelSurchargeCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1

update #tempComponents
	set TotalPostalDropCost = isnull(PostalDropCost, 0) + isnull(PostalDropFuelSurchargeCost, 0)
where EST_ComponentType_ID = 1

update #tempComponents
	set	MailTrackingCost = isnull((SoloQuantity / cast(1000 as decimal) * MailTrackingCPMRate), 0) + isnull(dbo.ComponentPolybagMailTrackingCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponents
	set SoloPostageCost = isnull(dbo.ComponentSoloPostage(EST_Component_ID, PieceWeight, PST_PostalScenario_ID), 0)

update #tempComponents
	set PolyPostageCost = isnull(dbo.ComponentPolyPostage(EST_Component_ID, PieceWeight), 0)

update #tempComponents
	set TotalPostageCost = SoloPostageCost + PolyPostageCost

update #tempComponents
	set PostalTotal = isnull(TotalPostalDropCost, 0) + isnull(MailTrackingCost, 0) + TotalPostageCost

update t
	set SampleFreight =
		case
			when isnull(s.FreightCWT, 0) <> 0 then dbo.EstimatePieceWeight(t.EST_Estimate_ID) * SampleQuantity / cast(100 as decimal) * s.FreightCWT
			else s.FreightFlat
		end
from #tempComponents t join EST_Samples s on t.EST_Estimate_ID = s.EST_Estimate_ID
where t.EST_ComponentType_ID = 1

update #tempComponents
	set OtherFreight = GrossOtherFreight
/* Not sure if this cost should be distributed across Polybags */
/*
			case
				when isnull(SoloQuantity, 0) = 0 then 0
				else GrossOtherFreight * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
			end
			+ isnull(dbo.ComponentPolybagOtherFreightCost(EST_Component_ID), 0)
*/
where EST_ComponentType_ID = 1

update #tempComponents
	set DistributionTotal = InsertTotal + isnull(PostalTotal, 0) + isnull(SampleFreight, 0) + isnull(OtherFreight, 0)

update #tempComponents
	set GrandTotal = ProductionTotal + AssemblyTotal + DistributionTotal

/* Add OtherQuantity to MediaQuantity for report */
update #tempComponents
	set MediaQuantity = MediaQuantity + OtherQuantity

set nocount off
select * from #tempComponents
order by EST_Estimate_ID, EST_Component_ID
set nocount on

drop table #tempEstimateMediaType
drop table #tempComponentType
drop table #tempComponents
GO

GRANT  EXECUTE  ON [dbo].[rpt_EstimateSummary]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.rpt_FinancialChange_UploadData') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.rpt_FinancialChange_UploadData'
	DROP PROCEDURE dbo.rpt_FinancialChange_UploadData
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.rpt_FinancialChange_UploadData') IS NOT NULL
		PRINT '***********Drop of dbo.rpt_FinancialChange_UploadData FAILED.'
END
GO
PRINT 'Creating dbo.rpt_FinancialChange_UploadData'
GO

CREATE PROC dbo.rpt_FinancialChange_UploadData
/*
* PARAMETERS:
* EstimateIDs - Xml formatted list of estimates to query.
*
* DESCRIPTION:
*		Returns information for financial change report.
*
*
* TABLES:
*   Table Name                  Access
*   ==========                  ======
*   EST_Estimate                READ
*   EST_Component               READ
*   ADMINSYSTEM.informix.ad_est READ
*
*
* PROCEDURES CALLED:
*   None
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
* 09/10/2007      BJS             Initial Creation
* 12/16/2007      BJS             Modified EstimateIDs parameter and xml schema to allow for more estimates
* 12/17/2007      JRH             Drop #tempAdNumbers temp table.
* 12/18/2007      BJS             Changed population of #tempAdNumbers to prevent duplicate ad numbers from
*                                 being inserted and multiplying costs.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EstimateIDs varchar(4000)
as

set nocount on

declare @EstimateDocID int

create table #tempEstimate(EST_Estimate_ID BIGINT NOT NULL)

exec sp_xml_preparedocument @EstimateDocID output, @EstimateIDs
insert into #tempEstimate(EST_Estimate_ID)
select EST_Estimate_ID
from OPENXML(@EstimateDocID, '/root/estimate')
with(est_estimate_id BIGINT '@id')

set nocount off

/* Gather the Ad Numbers used by the estimates */
create table #tempAdNumbers(
	AdNumber INT NOT NULL,
	RunDate datetime NOT NULL,
	FiscalSeason varchar(35) NOT NULL,
	FiscalYear INT NOT NULL,
	FiscalMonth INT NOT NULL)
insert into #tempAdNumbers(AdNumber, RunDate, FiscalSeason, FiscalYear, FiscalMonth)
select c.AdNumber, max(e.RunDate), max(es.Description), max(e.FiscalYear), max(e.FiscalMonth)
from EST_Component c join #tempEstimate te on c.EST_Estimate_ID = te.EST_Estimate_ID
	join EST_Estimate e on te.EST_Estimate_ID = e.EST_Estimate_ID
	join EST_Season es on e.EST_Season_ID = es.EST_Season_ID
where c.AdNumber is not null
group by c.AdNumber

select max(tad.RunDate) RunDate, max(tad.FiscalSeason) Season, max(tad.FiscalYear) FiscalYear, max(tad.FiscalMonth) FiscalMonth,
	ae.ad_nbr AdNumber, max(ae.estd_ad_desc) AdDescription, max(ae.estd_media_qty) MediaQty, cast(max(ae.estd_base_pg_qty) as int) PageCount,
	sum(ce.est_ad_cost_amt) AdCost
from #tempAdNumbers tad	join DBADVProd.informix.ad_est ae on tad.AdNumber = ae.ad_nbr
	join DBADVProd.informix.ad_cost_est ce on ae.ad_nbr = ce.ad_nbr
group by ae.ad_nbr
order by e.FiscalYear, e.FiscalMonth, ae.ad_nbr

set nocount on
drop table #tempEstimate
drop table #tempAdNumbers
GO

GRANT  EXECUTE  ON [dbo].[rpt_FinancialChange_UploadData]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
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

GO
IF OBJECT_ID('dbo.rpt_PubChange_UploadData') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.rpt_PubChange_UploadData'
	DROP PROCEDURE dbo.rpt_PubChange_UploadData
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.rpt_PubChange_UploadData') IS NOT NULL
		PRINT '***********Drop of dbo.rpt_PubChange_UploadData FAILED.'
END
GO
PRINT 'Creating dbo.rpt_PubChange_UploadData'
GO

CREATE PROC dbo.rpt_PubChange_UploadData
/*
* PARAMETERS:
* EstimateIDs - Xml formatted list of estimates to query.
*
* DESCRIPTION:
*		Returns information for pub change report.
*
*
* TABLES:
*   Table Name                              Access
*   ==========                              ======
*   EST_Estimate                            READ
*   EST_Component                           READ
*   ADMINSYSTEM.informix.ad_est             READ
*   ADMINSYSTEM.informix.pub_cost_est       READ
*   ADMINSYSTEM.informix.pub                READ
*   ADMINSYSTEM.informix.ctlg_pubvr_distbn  READ
*
*
* PROCEDURES CALLED:
*   None
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
* 09/25/2007      BJS             Initial Creation
* 12/16/2007      BJS             Modified EstimateIDs parameter to allow for a larger set of estimates.
* 12/18/2007      BJS             Added #tempAdNumbers.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EstimateIDs varchar(4000)
as

set nocount on

declare @EstimateDocID int

create table #tempEstimate(EST_Estimate_ID BIGINT NOT NULL)

exec sp_xml_preparedocument @EstimateDocID output, @EstimateIDs
insert into #tempEstimate(EST_Estimate_ID)
select EST_Estimate_ID
from OPENXML(@EstimateDocID, '/root/estimate')
with(est_estimate_id BIGINT '@id')

/* Gather the Ad Numbers used by the estimates */
create table #tempAdNumbers(
	AdNumber INT NOT NULL,
	RunDate datetime NOT NULL,
	FiscalSeason varchar(35) NOT NULL,
	FiscalYear INT NOT NULL,
	FiscalMonth INT NOT NULL)
insert into #tempAdNumbers(AdNumber, RunDate, FiscalSeason, FiscalYear, FiscalMonth)
select c.AdNumber, max(e.RunDate), max(es.Description), max(e.FiscalYear), max(e.FiscalMonth)
from EST_Component c join #tempEstimate te on c.EST_Estimate_ID = te.EST_Estimate_ID
	join EST_Estimate e on te.EST_Estimate_ID = e.EST_Estimate_ID
	join EST_Season es on e.EST_Season_ID = es.EST_Season_ID
where c.AdNumber is not null
group by c.AdNumber

set nocount off

select max(tad.RunDate) RunDate, ae.ad_nbr AdNumber,
	max(ae.estd_ad_desc) AdDescription, pe.pub_id, max(p.pub_nm) PubDesc, pe.publoc_id, max(pd.media_qty) MediaQuantity,
	max(pd.vrsn_pub_issue_dt) IssueDate
from #tempAdNumbers tad join DBADVProd.informix.ad_est ae on tad.AdNumber = ae.ad_nbr
	join DBADVProd.informix.pub_cost_est pe on ae.ad_nbr = pe.ad_nbr
	join DBADVProd.informix.pub p on pe.pub_id = p.pub_id
	join DBADVProd.informix.ctlg_pubvr_distbn pd on ae.ad_nbr = pd.ad_nbr and pe.pub_id = pd.pub_id and pe.publoc_id = pd.publoc_id
group by ae.ad_nbr, pe.pub_id, pe.publoc_id
order by tad.RunDate, ae.ad_nbr, pe.pub_id, pe.publoc_id

set nocount on
drop table #tempEstimate
drop table #tempAdNumbers
GO

GRANT  EXECUTE  ON [dbo].[rpt_PubChange_UploadData]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO


GO
IF OBJECT_ID('dbo.Rpt_ReportHistory') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.Rpt_ReportHistory'
	DROP PROCEDURE dbo.Rpt_ReportHistory
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.Rpt_ReportHistory') IS NOT NULL
		PRINT '***********Drop of dbo.Rpt_ReportHistory FAILED.'
END
GO
PRINT 'Creating dbo.Rpt_ReportHistory'
GO

CREATE PROCEDURE dbo.Rpt_ReportHistory
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Returns a list of previously run reports
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   RptReport
*   RptReportType
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/20/2007      NLS             Initial Creation 
*
*/
AS BEGIN

SELECT 
	R.rpt_report_id, 
	T.description,
	R.createdby, 
	R.createddate
	
FROM
	rpt_report AS R INNER JOIN
	rpt_reporttype AS T 
ON
	R.rpt_reporttype_id = T.rpt_reporttype_id

ORDER BY R.createddate DESC

END
GO

GRANT  EXECUTE  ON [dbo].[Rpt_ReportHistory]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.rpt_TYLY') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.rpt_TYLY'
	DROP PROCEDURE dbo.rpt_TYLY
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.rpt_TYLY') IS NOT NULL
		PRINT '***********Drop of dbo.rpt_TYLY FAILED.'
END
GO
PRINT 'Creating dbo.rpt_TYLY'
GO

CREATE PROC dbo.rpt_TYLY
/*
* PARAMETERS:
*	EST_Season_ID - optional
*   FiscalYear
*   VendorSupplied    - 1 = All Components, 2 = Only VS Components, 3 = Exclude VS Components
*   EstimateMediaType - Xml formatted list of media types to query.  If null, all media types are queried.
*   ComponentType     - Xml formatted list of component types to query.  If null, all component types are queried.
*
* DESCRIPTION:
*	Returns data used by the This Year - Last Year Comparison Report.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Estimate                    READ
*   EST_Component                   READ
    DBADVProd.informix.ad_est       READ
*   DBADVProd.informix.ad_cost_est  READ
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/01/2007      BJS             Initial Creation
* 11/19/2007      JRH             Fixed Season.
*
*/
@EST_Season_ID int,
@FiscalYear int,
@VendorSupplied tinyint,
@EstimateMediaType varchar(2000),
@ComponentType varchar(2000)
as

set nocount on

declare @MediaTypeDocID int, @ComponentTypeDocID int

create table #tempEstimateMediaType(EST_EstimateMediaType_ID INT NOT NULL, Description VARCHAR(35) NOT NULL)
create table #tempComponentType(EST_ComponentType_ID INT NOT NULL, Description VARCHAR(35) NOT NULL)

if (@EstimateMediaType is null) begin
	insert into #tempEstimateMediaType(EST_EstimateMediaType_ID, Description)
	select EST_EstimateMediaType_ID, Description
	from EST_EstimateMediaType
end
else begin
	exec sp_xml_preparedocument @MediaTypeDocID output, @EstimateMediaType
	insert into #tempEstimateMediaType(EST_EstimateMediaType_ID, Description)
	select emt.EST_EstimateMediaType_ID, emt.Description
	from OPENXML(@MediaTypeDocID, '/root/est_estimatemediatype')
	with(est_estimatemediatype_id INT '@est_estimatemediatype_id') xdata
		join EST_EstimateMediaType emt on xdata.EST_EstimateMediaType_ID = emt.EST_EstimateMediaType_ID
end

if (@ComponentType is null) begin
	insert into #tempComponentType(EST_ComponentType_ID, Description)
	select EST_ComponentType_ID, Description
	from EST_ComponentType
end
else begin
	exec sp_xml_preparedocument @ComponentTypeDocID output, @ComponentType
	insert into #tempComponentType(EST_ComponentType_ID, Description)
	select ct.EST_ComponentType_ID, ct.Description
	from OPENXML(@ComponentTypeDocID, '/root/est_componenttype')
	with(est_componenttype_id INT '@est_componenttype_id') xdata
		join EST_ComponentType ct on xdata.EST_ComponentType_ID = ct.EST_ComponentType_ID
end

--Retrieve Ad Numbers that match the search criteria
create table #tempAdNumbers(AdNumber int, AdDesc varchar(35), RunDate datetime, FiscalYear int, EST_Season_ID int, SeasonDesc varchar(35))
insert into #tempAdNumbers(AdNumber, RunDate, FiscalYear, EST_Season_ID, SeasonDesc)
select c.AdNumber, max(e.RunDate) RunDate, max(e.FiscalYear) FiscalYear, es.EST_Season_ID, max(es.Description) SeasonDesc
from EST_Estimate e join vw_Estimate_ExcludeOldUploads eo on e.EST_Estimate_ID = eo.EST_Estimate_ID
	join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join EST_Season es on e.EST_Season_ID = es.EST_Season_ID
	join #tempEstimateMediaType emt on c.EST_EstimateMediaType_ID = emt.EST_EstimateMediaType_ID
	join #tempComponentType ct on c.EST_ComponentType_ID = ct.EST_ComponentType_ID
where e.EST_Status_ID = 2
	and (@EST_Season_ID is null or e.EST_Season_ID = @EST_Season_ID)
	and e.FiscalYear = @FiscalYear
	and (@VendorSupplied = 1 or (@VendorSupplied = 2 and c.VendorSupplied = 1) or (@VendorSupplied = 3 and c.VendorSupplied = 0))
	and c.AdNumber is not null
group by c.AdNumber, es.EST_Season_ID

update #tempAdNumbers
set AdDesc = dbo.FindComponentDescription(AdNumber, RunDate, @VendorSupplied)

set nocount off

select tad.AdNumber, max(tad.RunDate) RunDate, max(tad.FiscalYear) FiscalYear, tad.EST_Season_ID SeasonID, max(tad.SeasonDesc) SeasonDesc,
	tad.AdDesc Description, max(ae.estd_base_pg_qty) PageCount, max(ae.estd_media_qty) MediaQuantity,
	sum(ace.est_ad_cost_amt) EstimatedCost
from #tempAdNumbers tad join DBADVProd.informix.ad_est ae on tad.AdNumber = ae.ad_nbr
	join DBADVProd.informix.ad_cost_est ace on ae.ad_nbr = ace.ad_nbr
group by tad.AdNumber, tad.AdDesc, tad.EST_Season_ID
order by tad.RunDate, tad.AdNumber, tad.AdDesc

set nocount on

drop table #tempAdNumbers
drop table #tempEstimateMediaType
drop table #tempComponentType
GO

GRANT  EXECUTE  ON [dbo].[rpt_TYLY]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
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


GO
IF OBJECT_ID('dbo.rpt_VendorCommitment_Publisher') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.rpt_VendorCommitment_Publisher'
	DROP PROCEDURE dbo.rpt_VendorCommitment_Publisher
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.rpt_VendorCommitment_Publisher') IS NOT NULL
		PRINT '***********Drop of dbo.rpt_VendorCommitment_Publisher FAILED.'
END
GO
PRINT 'Creating dbo.rpt_VendorCommitment_Publisher'
GO

CREATE PROC dbo.rpt_VendorCommitment_Publisher
/*
* PARAMETERS:
* StartRunDate
* EndRunDate
* EST_Estimate_ID
* AdNumber
* EST_Status_ID
* Pubs
*
* DESCRIPTION:
*	Returns data for the Vendor Commitment report for Publications.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*
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
* 09/18/2007      BJS             Initial Creation - Based on rpt_EstimateSummary
* 10/08/2007      BJS             Explicitly cast values to decimal (ie 1000) to prevent rounding errors in calculations
* 11/05/2007      JRH             Fixed Flat Rate Calculation.
* 12/03/2007      JRH             Added "distinct" to initial select to prevent double counting.
*                                 Changed the way ad number is used in filter and eliminated second select.
*                                 Added grouping and sum in the final select. 
* 12/07/2007      BJS             Fixed Insert Discount calculation.
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
@Pubs varchar(2000)
as

set nocount on

declare @PubDocID int

create table #tempPubs(Pub_ID char(3) NOT NULL)

if (@Pubs is null) begin
	insert into #tempPubs(Pub_ID)
	select distinct Pub_ID
	from PUB_PubRate_Map
end
else begin
	exec sp_xml_preparedocument @PubDocID output, @Pubs
	insert into #tempPubs(Pub_ID)
	select prm.Pub_ID
	from OPENXML(@PubDocID, '/root/pub')
	with(pub_id CHAR(3) '@pub_id') xdata
		join PUB_PubRate_Map prm on xdata.Pub_ID = prm.Pub_ID
end

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
from EST_Estimate e join vw_Estimate_excludeOldUploads ve on e.EST_Estimate_ID = ve.EST_Estimate_ID
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
where (@StartRunDate is null or e.RunDate >= @StartRunDate)
	and (@EndRunDate is null or e.RunDate <= @EndRunDate)
	and (@EST_Estimate_ID is null or e.EST_Estimate_ID = @EST_Estimate_ID)
	and (@AdNumber is null or dbo.EstimateHostAdNumber(e.EST_Estimate_ID) = @AdNumber)
	and (@EST_Status_ID is null or e.EST_Status_ID = @EST_Status_ID)

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
				* tc.ComponentTabPageCount / tc.PackageTabPageCount
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

set nocount off

select Pub_ID, RunDate, AdNumber, Description, sum(PageCount) PageCount, MediaQuantity, InsertQuantity, sum(GrossInsertCost) GrossInsertCost, sum(InsertDiscount) InsertDiscount, sum(NetInsertCost) NetInsertCost
from #tempComponentPubRateMap
group by Pub_ID, RunDate, AdNumber, Description, MediaQuantity, InsertQuantity
order by RunDate, AdNumber
-- select Pub_ID, RunDate, AdNumber, Description, PageCount, MediaQuantity, InsertQuantity, GrossInsertCost, InsertDiscount, NetInsertCost
-- from #tempComponentPubRateMap
-- order by RunDate, AdNumber

set nocount on

drop table #tempComponentPubRateMap
drop table #tempPubs
GO

GRANT  EXECUTE  ON [dbo].[rpt_VendorCommitment_Publisher]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO


GO
IF OBJECT_ID('dbo.rpt_VendorCommitment_Vendor') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.rpt_VendorCommitment_Vendor'
	DROP PROCEDURE dbo.rpt_VendorCommitment_Vendor
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.rpt_VendorCommitment_Vendor') IS NOT NULL
		PRINT '***********Drop of dbo.rpt_VendorCommitment_Vendor FAILED.'
END
GO
PRINT 'Creating dbo.rpt_VendorCommitment_Vendor'
GO

CREATE PROC dbo.rpt_VendorCommitment_Vendor
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
*		Returns data for the Vendor Commitment report for Vendors.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate
*   EST_Component
*   EST_AssemDistribOptions
*   PPR_PaperWeight
*   PPR_PaperGrade
*   VND_Printer
*   PRT_PrinterRate
*   PPR_Paper_Map
*   VND_MailHouse
*   VND_MailTracker
*
*
* PROCEDURES CALLED:
*   dbo.TotalEstimateWeight
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
* 09/13/2007      BJS             Initial Creation - Based on rpt_EstimateSummary
* 09/24/2007      BJS             Modified Digi H&P, Stitcher Makeready and Press Makeready logic
* 10/08/2007      BJS             Explicitly cast values as decimal (ie 1000) to prevent rounding errors in calculations
* 11/26/2007      JRH             Fixed SampleFreight, InsertFuelSurchargeCost, and InsertFreightCost to allocate all to host.
* 12/07/2007      JRH             Fixed the InsertDiscount calculation.
* 12/17/2007      BJS             Added Onsert Vendor
* 12/18/2007      BJS             Added Sample Shipping Vendor (Host Printer)
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

/* Comments correlate to layout of Report/Extract #5 - Estimate Summary Report */
create table #tempComponents(
	EST_Component_ID bigint,
	EST_Estimate_ID bigint,
	EST_ComponentType_ID int,
	PST_PostalScenario_ID bigint,
	VendorSupplied_ID bigint,
	CreativeVendor_ID bigint,
	SeparatorVendor_ID bigint,
	PrinterVendor_ID bigint,
	PaperVendor_ID bigint,
	AssemblyVendor_ID bigint,
	MailTrackingVendor_ID bigint,
	MailHouseVendor_ID bigint,
	MailListResourceVendor_ID bigint,
	OnsertVendor_ID bigint,
	SampleFreightVendor_ID bigint,
	PostalVendor_ID bigint,
	EstimateMediaType varchar(35),
	ComponentType varchar(35),
	VendorSuppliedDesc varchar(35),
	CreativeVendor varchar(35),
	SeparatorVendor varchar(35),
	PrinterVendor varchar(35),
	PaperVendor varchar(35),
	AssemblyVendor varchar(35),
	MailTrackingVendor varchar(35),
	MailHouseVendor varchar(35),
	MailListResourceVendor varchar(35),
	OnsertVendor varchar(35),
	SampleFreightVendor varchar(35),
	PostalVendor varchar(35),
	/* Main */
		RunDate datetime,
		AdNumber int,
		Description varchar(50),
	/* Specifications */
		PageCount int,
		Width decimal(10,4),
		Height decimal(10,4),
		/* Paper */
			PaperWeight int,
			PaperGrade varchar(35),
		PieceWeight decimal(12,6), /* (Width * Height) / 950,000 * PageCount * PaperWeight * 1.03 */
	/* Quantity */
		InsertQuantity int,
		SoloQuantity int,
		PolyBagQuantity int,
		OtherQuantity int,
		SampleQuantity int,
		MediaQuantity int, /* Insert + Solo + PolyBag */
		SpoilagePct decimal(10,4),
		SpoilageQuantity int, /* SpoilagePct * Media Quantity */
		DirectMailQuantity int, /* Solo + Poly */
		InternalMailQuantity int, /* DirectMail - External List Quantity */
		ExternalMailQuantity int, /* External List Quantity */
		TotalProductionQuantity int, /* MediaQuantity + SpoilageQuantity + OtherQuantity + SampleQuantity */
	/* Production */
		ProductionWeight decimal(14,6), /* TotalProductionQuantity * PieceWeight */
		TotalEstimateWeight decimal(14,6), /* sum(ProductionWeight) group by EST_Estimate_ID*/
		CreativeCPP money,
		CreativeCost money,
		SeparatorCPP money,
		SeparatorCost money,
		/* Print */
			ManualPrintCost money,
			RunRate money,
			NumberOfPlants int,
			AdditionalPlates int,
			PrinterPlateCost money,
			NumberDigitalHandlePrepare int,
			DigitalHandlePrepareRate money,
			StitcherMakeReadyRate money,
			PressMakeReadyRate money,
			ReplacementPlateCost money,
			GrossPrintCost money, /* See Estimate Calculation Steps for formula */
			EarlyPayPrintDiscountPercent decimal(10,4),
			EarlyPayPrintDiscountAmount money, /* GrossPrintCost * EarlyPayPrintDiscountPercent */
			NetPrintCost money, /* GrossPrintCost - EarlyPayPrintDiscountAmount */
			PrinterTaxableMediaPct decimal(10,4),
			PrinterSalesTaxPct decimal(10,4),
			PrinterSalesTaxAmount money, /* (NetPrinterCost + PaperHandling) * PrinterTaxableMediaPct * PrinterSalesTaxPct */
			TotalPrintCost money, /* NetPrintCost + PrinterSalesTaxAmount */
		/* Paper */
			ManualPaperCost money,
			RunPounds decimal(10,2),
			MakereadyPounds int,
			PlateChangePounds decimal(10,2),
			NumberOfPressStops int,
			PressStopPounds int,
			TotalPaperPounds decimal(20,2), /* See Estimate Calculation Steps for formula */
			PaperCWTRate money, /* PPR_Paper_Map.CWT */
			GrossPaperCost money, /* TotalPaperPounds / 100 * PaperCWTRate */
			EarlyPayPaperDiscountPercent decimal(10,4),
			EarlyPayPaperDiscountAmount money, /* GrossPaperCost * EarlyPayPaperDiscountPercent */
			NetPaperCost money, /* GrossPaperCost - EarlyPayPaperDiscountAmount */
			PaperHandlingCWTRate money, /* VND_Printer.PaperHandling */
			PaperHandlingCost money, /* TotalPaperPounds * PaperHandlingCWTRate */
			PaperTaxableMediaPct decimal(10,4),
			PaperSalesTaxPct decimal(10,4),
			PaperSalesTaxAmount money, /* NetPaperCost * PaperTaxableMediaPct * PaperSalesTaxPct */
			TotalPaperCost money, /* NetPaperCost + PaperSalesTaxAmount */
		/* Mail List */
			InternalMailCPM money,
			ExternalMailCPM money,
			BlendedMailListCPM money,
			InternalMailListCost money,
			ExternalMailListCost money,
			MailListCost money, /* See Logic below */
		ProductionSalesTaxAmount money,
		VendorProductionCPM money,
		VendorProductionCost money, /* TotalProductionQuantity / 1000 * VendorProductionCPM  */
		OtherProductionCost money,
		ProductionTotal money, /* CreativeCost + SeparatorCost + TotalPrintCost + TotalPaperCost + ExternalMailCost + VendorProductionCost + OtherProductionCost */
	/* Assembly */
		/* PolyBag */
			PolyBagCost money, /* dbo.EstimatePolyBagCost */
			OnsertRate money,
			OnsertCost money, /* Only if component type is Host Onsert -- PolyBagQuantity / 1000 * OnsertRate */
			PolyBagTotal money, /* PolyBagCost + OnsertCost */
		StitchInRate money,
		StitchInCost money, /* StitchInRate * MediaQuantity (if comp type is stitch-in) / 1000 */
		BlowInRate money,
		BlowInCost money, /* BlowInRate * MediaQuantity (if comp type is blow-in) / 1000 */
		/* Ink Jet */
			InkJetRate money,
			InkJetMakeReadyRate money,
			InkJetCost money, /* DirectMailQuantity / 1000 * InkJetRate */
			GrossInkjetMakereadyCost money, /* InkJetMakeReadyRate * NumberOfPlants */
			InkjetMakereadyCost money, /* See Logic below */
			TotalInkJetCost money, /* InkJetCost + InkJetMakeReadyCost */
		/* Handling */
			/* Insert */
				CornerGuardRate money,
				CornerGuardCost money, /* (MediaQuantity * PieceWeight) / 2000 * CornerGuardRate */
				SkidRate money,
				SkidCost money, /* (MediaQuantity * PieceWeight) / 2000 * SkidRate */
				NumberOfCartons int,
				CartonRate money,
				CartonCost money, /* NumberOfCartons * CartonCost */
				InsertHandlingTotal money, /* CornerGuardCost + SkidCost + CartonCost */
			/* Mail */
				TimeValueSlipsCPM money,
				TimeValueSlipsCost money, /* See SQL Logic Below */
				GrossMailHouseAdminFee money, /* The Mail House Admin Fee. */
				MailHouseAdminFee money, /* See SQL Logic Below */
				GlueTackCPM money,
				GlueTackCost money, /* See SQL Logic Below */
				TabbingCPM money,
				TabbingCost money, /* See SQL Logic Below */
				LetterInsertionCPM money,
				LetterInsertionCost money, /* See SQL Logic Below */
				OtherMailHandlingCPM money,
				OtherMailHandlingCost money, /* See SQL Logic Below */
				MailHandlingTotal money, /* See SQL Logic Below */
			HandlingTotal money, /* InsertHandlingTotal + MailHandlngTotal */
		AssemblyTotal money, /* PolyBagTotal + StitchInCost + BlowInCost + TotalInkJetCost + HandlingTotal */
	/* Distribution */
		/* Insert */
			InsertCost money, /* See Logic below */
			InsertFreightCWT money,
			InsertFreightCost money, /* InsertQuantity * PieceWeight / 100 * InsertFreightCWT */
			InsertFuelSurchargePercent decimal(10,4),
			InsertFuelSurchargeCost money, /* InsertFreightCost * InsertFreightSurchargePercent */
			InsertFreightTotalCost money,
			InsertTotal money, /* InsertCost + InsertFreightCost + InsertFuelSurchargeCost */
		/* Postal */
			PostalDropCost money, /* See Logic Below */
			PostalDropFuelSurchargeCost money, /* See Logic Below*/
			TotalPostalDropCost money,
			MailTrackingCPMRate money,
			MailTrackingCost money, /* DirectMailQuantity / 1000 * MailTrackingCPMRate */
			SoloPostageCost money,
			PolyPostageCost money,
			TotalPostageCost money, /* SoloPostageCost + PolyPostageCost */
			PostalTotal money, /*Postage PostalDropCost + PostalFuelSurcharge + MailTrackingCost + TotalPostageCost */
		SampleFreight money,
		GrossOtherFreight money,
		OtherFreight money,
		DistributionTotal money, /* InsertTotal + PostalTotal + SampleFreight + OtherFreight */ 			
	GrandTotal money
)

/* Get Raw Production Data */
insert into #tempComponents(EST_Component_ID, EST_Estimate_ID, EST_ComponentType_ID, PST_PostalScenario_ID,
	VendorSupplied_ID, CreativeVendor_ID, SeparatorVendor_ID, PrinterVendor_ID, PaperVendor_ID, AssemblyVendor_ID, MailTrackingVendor_ID,
	MailHouseVendor_ID, MailListResourceVendor_ID, OnsertVendor_ID,
	EstimateMediaType, ComponentType,
	VendorSuppliedDesc, CreativeVendor, SeparatorVendor, PrinterVendor, PaperVendor, AssemblyVendor, MailTrackingVendor, MailHouseVendor,
	MailListResourceVendor, OnsertVendor,
	RunDate, AdNumber, Description, PageCount, Width, Height, PaperWeight, PaperGrade, CreativeCPP, CreativeCost, SeparatorCPP, SeparatorCost,
	ExternalMailQuantity, SpoilagePct, ManualPrintCost, RunRate, NumberOfPlants, AdditionalPlates,
	PrinterPlateCost, NumberDigitalHandlePrepare, DigitalHandlePrepareRate, StitcherMakereadyRate, PressMakereadyRate,
	ReplacementPlateCost, EarlyPayPrintDiscountPercent, PrinterTaxableMediaPct,
	PrinterSalesTaxPct, ManualPaperCost, RunPounds, MakeReadyPounds, PlateChangePounds, NumberOfPressStops, PressStopPounds, PaperCWTRate,
	EarlyPayPaperDiscountPercent, PaperHandlingCWTRate, PaperTaxableMediaPct, PaperSalesTaxPct, InternalMailCPM, ExternalMailCPM,
	VendorProductionCPM, OtherProductionCost, OnsertRate, StitchInRate, BlowInRate, InkJetRate, InkJetMakeReadyRate, CornerGuardRate, SkidRate,
	NumberOfCartons, TimeValueSlipsCPM, GrossMailHouseAdminFee, GlueTackCPM, TabbingCPM, LetterInsertionCPM, OtherMailHandlingCPM,
	InsertFreightCWT, InsertFuelSurchargePercent, MailTrackingCPMRate, GrossOtherFreight)
select c.EST_Component_ID, c.EST_Estimate_ID, c.EST_ComponentType_ID, ad.PST_PostalScenario_ID,
	vs_vnd.VND_Vendor_ID, c.CreativeVendor_ID, c.Separator_ID, prt_vnd.VND_Vendor_ID, ppr_vnd.VND_Vendor_ID, assy_vnd.VND_Vendor_ID,
	mt_vnd.VND_Vendor_ID, mh_vnd.VND_Vendor_ID, ml_vnd.VND_Vendor_ID, ons_vnd.VND_Vendor_ID,
	emt.Description, ct.Description,
	vs_vnd.Description, c_vnd.Description, sep_vnd.Description, prt_vnd.Description, ppr_vnd.Description, assy_vnd.Description, mt_vnd.Description,
	mh_vnd.Description, ml_vnd.Description, ons_vnd.Description,
	e.RunDate, c.AdNumber, c.Description, c.PageCount, c.Width, c.Height, pw.Weight, pg.Grade,
	c.CreativeCPP, c.CreativeCPP * c.PageCount, c.SeparatorCPP, c.SeparatorCPP * c.PageCount,
	ad.ExternalMailQty, c.SpoilagePct, c.PrintCost, c.RunRate, c.NumberOfPlants,
	c.AdditionalPlates, pl.Rate, c.NumberDigitalHandlenPrepare, dh.Rate,
	case
		when c.StitcherMakeready_ID is null then c.StitcherMakereadyRate
		else smr.Rate
	end StitcherMakereadyRate,
	case
		when c.PressMakeready_ID is null then c.PressMakereadyRate
		else pmr.Rate
	end PressMakereadyRate,
	c.ReplacementPlateCost, c.EarlyPayPrintDiscount,
	c.PrinterTaxableMediaPct, c.PrinterSalesTaxPct, c.PaperCost, c.RunPounds, c.MakeReadyPounds, c.PlateChangePounds, c.NumberOfPressStops,
	c.PressStopPounds, pm.CWT, c.EarlyPayPaperDiscount, p.PaperHandling, c.PaperTaxableMediaPct, c.PaperSalesTaxPct,
	case
		when c.EST_ComponentType_ID = 1 then ml.InternalListRate
		else null
	end InternalMailCPM,
	case
		when c.EST_ComponentType_ID = 1 then ad.ExternalMailCPM
		else null
	end ExternalMailCPM, c.VendorCPM, c.OtherProduction,
	oi.Rate, si.Rate, bi.Rate,
	case
		when c.EST_ComponentType_ID = 1 then mh.InkJetRate
		else null
	end InkJetRate,
	case
		when c.EST_ComponentType_ID = 1 then mh.InkJetMakeReady
		else null
	end InkJetMakeReady,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.CornerGuards = 1 then p.CornerGuard
		else null
	end CornerGuardRate,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.Skids = 1 then p.Skid
		else null
	end SkidRate,
	ad.NbrOfCartons,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then mh.TimeValueSlips
		else null
	end TimeValueSlipsCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then mh.AdminFee
		else null
	end GrossMailHouseAdminFee,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseGlueTack = 1 then mh.GlueTackRate
		else null
	end GlueTackCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseLetterInsertion = 1 then mh.LetterInsertionRate
		else null
	end LetterInsertionCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseTabbing = 1 then mh.TabbingRate
		else null
	end TabbingCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then ad.MailHouseOtherHandling
		else null
	end OtherMailHandlingCPM,
	ad.InsertFreightCWT, ad.InsertFuelSurcharge,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseMailTracking = 1 then mt.MailTracking
		else null
	end MailTrackingCPMRate,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then ad.OtherFreight
		else null
	end OtherFreight
from EST_Estimate e join vw_Estimate_excludeOldUploads eo on e.EST_Estimate_ID = eo.EST_Estimate_ID
	join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join EST_EstimateMediaType emt on c.EST_EstimateMediaType_ID = emt.EST_EstimateMediaType_ID
	join EST_ComponentType ct on c.EST_ComponentType_ID = ct.EST_ComponentType_ID
	join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
	join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
	left join PPR_PaperGrade pg on c.PaperGrade_ID = pg.PPR_PaperGrade_ID
	left join VND_Vendor vs_vnd on c.VendorSupplied_ID = vs_vnd.VND_Vendor_ID
	left join VND_Vendor c_vnd on c.CreativeVendor_ID = c_vnd.VND_Vendor_ID
	left join VND_Vendor sep_vnd on c.Separator_ID = sep_vnd.VND_Vendor_ID
	left join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	left join VND_Vendor prt_vnd on p.VND_Vendor_ID = prt_vnd.VND_Vendor_ID
	left join VND_Paper ppr on c.Paper_ID = ppr.VND_Paper_ID
	left join VND_Vendor ppr_vnd on ppr.VND_Vendor_ID = ppr_vnd.VND_Vendor_ID
	left join VND_Printer ap on c.AssemblyVendor_ID = ap.VND_Printer_ID
	left join VND_Vendor assy_vnd on ap.VND_Vendor_ID = assy_vnd.VND_Vendor_ID
	left join PRT_PrinterRate pl on c.PlateCost_ID = pl.PRT_PrinterRate_ID /*Plates*/
	left join PRT_PrinterRate dh on c.DigitalHandlenPrepare_ID = dh.PRT_PrinterRate_ID /*Digi H&P*/
	left join PRT_PrinterRate smr on c.StitcherMakeready_ID = smr.PRT_PrinterRate_ID /*Stitcher Makeready*/
	left join PRT_PrinterRate pmr on c.PressMakeready_ID = pmr.PRT_PrinterRate_ID /*Press Makeready*/
	left join PPR_Paper_Map pm on c.Paper_Map_ID = pm.PPR_Paper_Map_ID
	left join PRT_PrinterRate si on c.StitchIn_ID = si.PRT_PrinterRate_ID
	left join PRT_PrinterRate bi on c.BlowIn_ID = bi.PRT_PrinterRate_ID
	left join PRT_PrinterRate oi on c.Onsert_ID = oi.PRT_PrinterRate_ID
	left join VND_MailTrackingRate mt on ad.MailTracking_ID = mt.VND_MailTrackingRate_ID
	left join VND_Vendor mt_vnd on mt.VND_Vendor_ID = mt_vnd.VND_Vendor_ID
	join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
	join VND_Vendor mh_vnd on mh.VND_Vendor_ID = mh_vnd.VND_Vendor_ID
	left join VND_MailListResourceRate ml on ad.MailListResource_ID = ml.VND_MailListResourceRate_ID
	left join VND_Vendor ml_vnd on ml.VND_Vendor_ID = ml_vnd.VND_Vendor_ID
	left join VND_Printer ons_prt on oi.VND_Printer_ID = ons_prt.VND_Printer_ID
	left join VND_Vendor ons_vnd on ons_prt.VND_Vendor_ID = ons_vnd.VND_Vendor_ID
where (@StartRunDate is null or e.RunDate >= @StartRunDate)
	and (@EndRunDate is null or e.RunDate <= @EndRunDate)
	and (@EST_Estimate_ID is null or e.EST_Estimate_ID = @EST_Estimate_ID)
	and (@AdNumber is null or dbo.EstimateHostAdNumber(e.EST_Estimate_ID) = @AdNumber)
	and (@EST_Status_ID is null or e.EST_Status_ID = @EST_Status_ID)
	and (
		exists(select 1 from #tempVendors tv where c.VendorSupplied_ID = tv.VND_Vendor_ID)
			or exists(select 1 from #tempVendors tv where prt_vnd.VND_Vendor_ID = tv.VND_Vendor_ID)
			or exists(select 1 from #tempVendors tv where ppr_vnd.VND_Vendor_ID = tv.VND_Vendor_ID)
			or exists(select 1 from #tempVendors tv where assy_vnd.VND_Vendor_ID = tv.VND_Vendor_ID)
			or exists(select 1 from #tempVendors tv where mt.VND_Vendor_ID = tv.VND_Vendor_ID)
			or exists(select 1 from #tempVendors tv where mh.VND_Vendor_ID = tv.VND_Vendor_ID)
			or exists(select 1 from #tempVendors tv where ml.VND_Vendor_ID = tv.VND_Vendor_ID)
			or exists(select 1 from #tempVendors tv where dbo.IsEstimateUsingPostalVendor(e.EST_Estimate_ID, tv.VND_Vendor_ID) = 1))

/* Identify the Host Printer Vendor.  The Sample Freight is allocated to this vendor. */
update tc
	set
		tc.SampleFreightVendor_ID = sample_vnd.VND_Vendor_ID,
		tc.SampleFreightVendor = sample_vnd.Description
from #tempComponents tc join EST_Component c on tc.EST_Estimate_ID = c.EST_Estimate_ID and c.EST_ComponentType_ID = 1
	join VND_Printer sample_prt on c.Printer_ID = sample_prt.VND_Printer_ID
	join VND_Vendor sample_vnd on sample_prt.VND_Vendor_ID = sample_vnd.VND_Vendor_ID
	join #tempVendors tv on sample_vnd.VND_Vendor_ID = tv.VND_Vendor_ID

/* Identify the Postal Vendor.  (Technically there can be more than one, but realistically it is just USPS so this will work) */
update tc
	set tc.PostalVendor_ID = v.VND_Vendor_ID,
		PostalVendor = v.Description
from #tempComponents tc join EST_AssemDistribOptions ad on tc.EST_Estimate_ID = ad.EST_Estimate_ID
	join PST_PostalScenario ps on ad.PST_PostalScenario_ID = ps.PST_PostalScenario_ID
	join PST_PostalCategoryScenario_Map pcsm on ps.PST_PostalScenario_ID = pcsm.PST_PostalScenario_ID
	join PST_PostalCategoryRate_Map pcrm on pcsm.PST_PostalCategoryRate_Map_ID = pcrm.PST_PostalCategoryRate_Map_ID
	join PST_PostalWeights pw on pcrm.PST_PostalWeights_ID = pw.PST_PostalWeights_ID
	join VND_Vendor v on pw.VND_Vendor_ID = v.VND_Vendor_ID
	
/* Perform Specification Calculations */
update #tempComponents
	set PieceWeight = (Width * Height) / cast(950000 as decimal) * PageCount * PaperWeight * 1.03

/* Perform Quantity Calculations */
update tc
	set InsertQuantity = isnull(iq.InsertQuantity, 0)
from #tempComponents tc left join vwComponentInsertQuantity iq on tc.EST_Component_ID = iq.EST_Component_ID

update tc
	set SoloQuantity = isnull(sq.SoloQuantity, 0)
from #tempComponents tc left join vwComponentSoloQuantity sq on tc.EST_Component_ID = sq.EST_Component_ID

update tc
	set PolyBagQuantity = isnull(pq.PolybagQuantity, 0)
from #tempComponents tc left join vwComponentPolybagQuantity pq on tc.EST_Component_ID = pq.EST_Component_ID

update tc
	set OtherQuantity = isnull(oq.OtherQuantity, 0)
from #tempComponents tc left join vwComponentOtherQuantity oq on tc.EST_Component_ID = oq.EST_Component_ID

update tc
	set SampleQuantity = isnull(sq.SampleQuantity, 0)
from #tempComponents tc left join vwComponentSampleQuantity sq on tc.EST_Component_ID = sq.EST_Component_ID

update tc
	set MediaQuantity = mq.MediaQuantity
from #tempComponents tc join vwComponentMediaQuantity mq on tc.EST_Component_ID = mq.EST_Component_ID

update #tempComponents
	set SpoilageQuantity = isnull(SpoilagePct, 0) * MediaQuantity,
		DirectMailQuantity = SoloQuantity + PolybagQuantity,
		InternalMailQuantity = (SoloQuantity + PolybagQuantity) - ExternalMailQuantity

update #tempComponents
	set TotalProductionQuantity = MediaQuantity + SpoilageQuantity + OtherQuantity + SampleQuantity

update tc
	set tc.TotalEstimateWeight = tw.TotalEstimateWeight
from #tempComponents tc join vwTotalEstimateWeight tw on tc.EST_Estimate_ID = tw.EST_Estimate_ID
where tc.EST_ComponentType_ID = 1

update #tempComponents
	set ProductionWeight = TotalProductionQuantity * PieceWeight

/* Perform Production Calculations */
update #tempComponents
	set GrossPrintCost = ManualPrintCost
where ManualPrintCost is not null

update #tempComponents
	set GrossPrintCost = TotalProductionQuantity / cast(1000 as decimal) * isnull(RunRate, 0)
		+ isnull(NumberOfPlants, 0) * (isnull(StitcherMakeReadyRate, 0) + isnull(PressMakeReadyRate, 0))
		+ isnull(AdditionalPlates, 0) * isnull(PrinterPlateCost, 0)
		+ isnull(NumberDigitalHandlePrepare, 0) * isnull(DigitalHandlePrepareRate, 0)
		+ isnull(ReplacementPlateCost, 0)
where ManualPrintCost is null

update #tempComponents
	set EarlyPayPrintDiscountAmount = GrossPrintCost * isnull(EarlyPayPrintDiscountPercent, 0),
		NetPrintCost = GrossPrintCost - (GrossPrintCost * isnull(EarlyPayPrintDiscountPercent, 0))

/*TODO: We don't have a paper handling amount if manualpapercost is entered.  How does this effect the print sales tax?? */
update #tempComponents
	set GrossPaperCost = ManualPaperCost
where ManualPaperCost is not null

update #tempComponents
	set TotalPaperPounds = TotalProductionQuantity / cast(1000 as decimal) * isnull(RunPounds, 0)
		+ cast(isnull(NumberOfPlants, 0) as bigint) * cast(isnull(MakeReadyPounds, 0) as bigint)
		+ cast(isnull(AdditionalPlates, 0) as bigint) * isnull(PlateChangePounds, 0)
		+ cast(isnull(NumberOfPressStops, 0) as bigint) * cast(isnull(PressStopPounds, 0) as bigint)
where ManualPaperCost is null

update #tempComponents
	set GrossPaperCost = TotalPaperPounds / cast(100 as decimal) * PaperCWTRate
where ManualPaperCost is null

update #tempComponents
	set EarlyPayPaperDiscountAmount = GrossPaperCost * isnull(EarlyPayPaperDiscountPercent, 0),
		NetPaperCost = GrossPaperCost - (GrossPaperCost * isnull(EarlyPayPaperDiscountPercent, 0))

update #tempComponents
	set PaperHandlingCost = isnull(TotalPaperPounds, 0) / cast(100 as decimal) * isnull(PaperHandlingCWTRate, 0)

update #tempComponents
	set PrinterSalesTaxAmount = (NetPrintCost + PaperHandlingCost) * isnull(PrinterTaxableMediaPct, 0) * isnull(PrinterSalesTaxPct, 0),
		PaperSalesTaxAmount = NetPaperCost * isnull(PaperTaxableMediaPct, 0) * isnull(PaperSalesTaxPct, 0)

update #tempComponents
	set ProductionSalesTaxAmount = PrinterSalesTaxAmount + PaperSalesTaxAmount

update #tempComponents
	set TotalPrintCost = NetPrintCost + PrinterSalesTaxAmount,
		TotalPaperCost = NetPaperCost + PaperHandlingCost + PaperSalesTaxAmount,
		VendorProductionCost = TotalProductionQuantity / cast(1000 as decimal) * VendorProductionCPM

update #tempComponents
	set BlendedMailListCPM =
		case
			when dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) = 0 then 0
			else InternalMailCPM
					* cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) - ExternalMailQuantity as decimal)
					/ cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
				+ ExternalMailCPM
					* cast(ExternalMailQuantity as decimal)
					/ cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
		end
where EST_ComponentType_ID = 1

update #tempComponents
	set MailListCost = BlendedMailListCPM * SoloQuantity / 1000 + isnull(dbo.ComponentPolybagMailListCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponents
	set ProductionTotal = isnull(CreativeCost, 0) + isnull(SeparatorCost, 0) + TotalPrintCost + TotalPaperCost + isnull(MailListCost, 0) +
		+ isnull(VendorProductionCost, 0) + isnull(OtherProductionCost, 0)

/* Assembly Calculations */

update #tempComponents
	set PolyBagCost = dbo.EstimatePolyBagCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1

update #tempComponents
	set OnsertCost = PolybagQuantity / cast(1000 as decimal) * OnsertRate
where EST_ComponentType_ID = 2

update #tempComponents
	set GrossInkjetMakereadyCost = NumberofPlants * InkjetMakereadyRate

update #tempComponents
	set PolyBagTotal = isnull(PolyBagCost, 0) + isnull(OnsertCost, 0),
		StitchInCost =
			case EST_ComponentType_ID
				when 3 then MediaQuantity / cast(1000 as decimal) * StitchInRate
				else null
			end,
		BlowInCost =
			case EST_ComponentType_ID
				when 4 then MediaQuantity / cast(1000 as decimal) * BlowInRate
			end,
		InkJetCost =
			case EST_ComponentType_ID
				when 1 then (SoloQuantity / cast(1000 as decimal) * InkJetRate) + isnull(dbo.EstimatePolybagInkjetCost(EST_Estimate_ID), 0)
			end,
		InkJetMakeReadyCost =
			case EST_ComponentType_ID
				when 1 then
					case
						when SoloQuantity = 0 then 0
						else GrossInkjetMakereadyCost * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
					end
					+ isnull(dbo.ComponentPolybagInkjetMakereadyCost(EST_Component_ID), 0)
			end

update #tempComponents
		set TotalInkJetCost = isnull(InkJetCost, 0) + isnull(InkJetMakeReadyCost, 0)

update tc
	set CartonRate = pr.Rate
from #tempComponents tc join EST_Component c on tc.EST_Component_ID = c.EST_Component_ID
	join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate pr on p.VND_Printer_ID = pr.VND_Printer_ID and pr.PRT_PrinterRateType_ID = 3 and pr.[Default] = 1

update #tempComponents
	set CornerGuardCost = TotalEstimateWeight / cast(2000 as decimal) * CornerGuardRate,
		SkidCost = TotalEstimateWeight / cast(2000 as decimal) * SkidRate,
		CartonCost = NumberOfCartons * CartonRate,
		TimeValueSlipsCost = TimeValueSlipsCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseTimeValueSlipsCost(EST_Component_ID), 0),
		MailHouseAdminFee =
			case
				when SoloQuantity = 0 then 0
				else GrossMailHouseAdminFee * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
			end
			+ isnull(dbo.ComponentPolybagMailHouseAdminFee(EST_Component_ID), 0),
		GlueTackCost = GlueTackCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseGlueTackCost(EST_Component_ID), 0),
		TabbingCost = TabbingCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseTabbingCost(EST_Component_ID), 0),
		LetterInsertionCost = LetterInsertionCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseLetterInsertionCost(EST_Component_ID), 0),
		OtherMailHandlingCost = OtherMailHandlingCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseOtherDirectMailHandlingCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponents
	set InsertHandlingTotal = isnull(CornerGuardCost, 0) + isnull(SkidCost, 0) + isnull(CartonCost, 0),
		MailHandlingTotal = isnull(TimeValueSlipsCost, 0) + isnull(MailHouseAdminFee, 0) + isnull(GlueTackCost, 0) + isnull(TabbingCost, 0)
			+ isnull(LetterInsertionCost, 0) + isnull(OtherMailHandlingCost, 0)

update #tempComponents
	set HandlingTotal = InsertHandlingTotal + MailHandlingTotal

update #tempComponents
	set AssemblyTotal = PolyBagTotal + isnull(StitchInCost, 0) + isnull(BlowInCost, 0) + TotalInkJetCost + HandlingTotal

/* Distribution Calculations */

create table #tempPackagesByPubRate(
	EST_Component_ID bigint not null,
	EST_Package_ID bigint not null,
	PubRate_Map_ID bigint not null,
	PUB_PubRate_ID bigint not null,
	PUB_PubQuantity_ID bigint not null,
	QuantityType int not null,
	InsertDate datetime not null,
	InsertDOW int not null,
	BlowInRate int null,
	PackageTabPageCount int null,
	ComponentPieceWeight decimal(12,6) not null,
	PackageWeight decimal(12,6) not null,
	PackageSize int not null,
	BilledQuantity int,
	GrossPackageInsertCost money null,
	InsertDiscountPercent decimal(10,4),
	PackageComponentCost money null)

insert into #tempPackagesByPubRate(EST_Component_ID, EST_Package_ID, PubRate_Map_ID, PUB_PubRate_ID, PUB_PubQuantity_ID, QuantityType,
	InsertDate, InsertDOW, ComponentPieceWeight, PackageWeight, PackageSize)
select pcm.EST_Component_ID, p.EST_Package_ID, pprm.PUB_PubRate_Map_ID, dbo.CalcPubRateID(pprm.PUB_PubRate_Map_ID, pid.IssueDate),
	dbo.CalcPubQuantityID(pprm.PUB_PubRate_Map_ID, pid.IssueDate), p.PUB_PubQuantityType_ID, pid.IssueDate,
	case p.PUB_PubQuantityType_ID
		when 4 then 1 /* Thanksgiving always uses Sunday Rates */
		when 5 then 1 /* Christmas always uses Sunday Rates */
		when 6 then 1 /* New Years always uses Sunday Rates */
		else pid.IssueDOW
	end IssueDOW,
	tc.PieceWeight, dbo.PackageWeight(p.EST_Package_ID), dbo.PackageSize(p.EST_Package_ID)
from #tempComponents tc join EST_PackageComponentMapping pcm on tc.EST_Component_ID = pcm.EST_Component_ID
	join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	join PUB_PubGroup pg on p.PUB_PubGroup_ID = pg.PUB_PubGroup_ID
	join PUB_PubPubGroup_Map ppgm on pg.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID
	join PUB_PubRate_Map pprm on ppgm.PUB_PubRate_Map_ID = pprm.PUB_PubRate_Map_ID
	join EST_PubIssueDates pid on p.EST_Estimate_ID = pid.EST_Estimate_ID and pprm.PUB_PubRate_Map_ID = pid.PUB_PubRate_Map_ID
where dbo.IsPubRateMapActive(pprm.PUB_PubRate_Map_ID, pid.IssueDate) = 1
	and dbo.CalcPubRateID(pprm.PUB_PubRate_Map_ID, pid.IssueDate) is not null
	and dbo.CalcPubQuantityID(pprm.PUB_PubRate_Map_ID, pid.IssueDate) is not null

update tp
	set tp.BlowInRate = pr.BlowInRate
from #tempPackagesByPubRate tp join PUB_PubRate pr on tp.PUB_PubRate_ID = pr.PUB_PubRate_ID

update #tempPackagesByPubRate
	set PackageTabPageCount = dbo.PackageTabPageCount(EST_Package_ID, BlowInRate)

update #tempPackagesByPubRate
	set BilledQuantity =
		case pr.QuantityChargeType
			when 1 then q.Quantity - (q.Quantity * pr.BilledPct)
			else q.Quantity
		end
from #tempPackagesByPubRate t join PUB_DayOfWeekQuantity q on t.PUB_PubQuantity_ID = q.PUB_PubQuantity_ID and t.QuantityType = q.PUB_PubQuantityType_ID
		and (t.QuantityType > 3 /*Holidays*/ or datepart(dw, t.InsertDate) = q.InsertDow /*Full Run / Contract Send*/) 
	join PUB_PubRate pr on t.PUB_PubRate_ID = pr.PUB_PubRate_ID

update #tempPackagesByPubRate
	set GrossPackageInsertCost = dbo.CalcGrossInsertCostforPackageandPub(PUB_PubRate_ID, InsertDate, InsertDOW, PackageTabPageCount, BilledQuantity, PackageWeight, PackageSize)

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
	set InsertDiscountPercent = d.Discount
from #tempPackagesByPubRate t join #tempPackageInsertNumber pin on t.EST_Package_ID = pin.EST_Package_ID and t.PubRate_Map_ID = pin.PUB_PubRate_Map_ID
	join PUB_InsertDiscounts d on t.PUB_PubRate_ID = d.PUB_PubRate_ID
where pin.InsertNumber = d.[Insert]

drop table #tempPackageInsertNumber

/*----------------------------------------- */

update tp
	set PackageComponentCost =
		case
			when PackageWeight = 0 then 0
			else
				(GrossPackageInsertCost * (1 - isnull(InsertDiscountPercent, 0))) * ComponentPieceWeight / PackageWeight
		end
from #tempPackagesByPubRate tp

update tc
	set InsertCost = isnull((select sum(PackageComponentCost) from #tempPackagesByPubRate tp where tp.EST_Component_ID = tc.EST_Component_ID), 0)
from #tempComponents tc

drop table #tempPackagesByPubRate

update #tempComponents
	set InsertFreightCost = dbo.TotalEstimateWeight(#tempComponents.EST_Estimate_ID) / cast(100 as decimal) * InsertFreightCWT,
		InsertFuelSurchargeCost = (dbo.TotalEstimateWeight(#tempComponents.EST_Estimate_ID) / cast(100 as decimal) * InsertFreightCWT) * InsertFuelSurchargePercent
where #tempComponents.EST_ComponentType_ID = 1

update #tempComponents
	set InsertFreightTotalCost = isnull(InsertFreightCost, 0) + isnull(InsertFuelSurchargeCost, 0)

update #tempComponents
	set InsertTotal = InsertCost + InsertFreightTotalCost

update #tempComponents
	set PostalDropCost = dbo.EstimatePostalDropCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1

update #tempComponents
	set PostalDropFuelSurchargeCost = dbo.EstimatePostalDropFuelSurchargeCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1

update #tempComponents
	set TotalPostalDropCost = isnull(PostalDropCost, 0) + isnull(PostalDropFuelSurchargeCost, 0)
where EST_ComponentType_ID = 1

update #tempComponents
	set	MailTrackingCost = isnull((SoloQuantity / cast(1000 as decimal) * MailTrackingCPMRate), 0) + isnull(dbo.ComponentPolybagMailTrackingCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponents
	set SoloPostageCost = isnull(dbo.ComponentSoloPostage(EST_Component_ID, PieceWeight, PST_PostalScenario_ID), 0)

update #tempComponents
	set PolyPostageCost = isnull(dbo.ComponentPolyPostage(EST_Component_ID, PieceWeight), 0)

update #tempComponents
	set TotalPostageCost = SoloPostageCost + PolyPostageCost

update #tempComponents
	set PostalTotal = TotalPostalDropCost + isnull(MailTrackingCost, 0) + TotalPostageCost

update t
	set SampleFreight =
		case
			when isnull(s.FreightCWT, 0) <> 0 then dbo.EstimatePieceWeight(t.EST_Estimate_ID) * SampleQuantity / cast(100 as decimal) * s.FreightCWT
			else s.FreightFlat
		end
from #tempComponents t join EST_Samples s on t.EST_Estimate_ID = s.EST_Estimate_ID
where t.EST_ComponentType_ID = 1

update #tempComponents
	set OtherFreight = GrossOtherFreight
/* Not sure if this cost should be distributed across Polybags */
/*
			case
				when isnull(SoloQuantity, 0) = 0 then 0
				else GrossOtherFreight * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
			end
			+ isnull(dbo.ComponentPolybagOtherFreightCost(EST_Component_ID), 0)
*/
where EST_ComponentType_ID = 1

update #tempComponents
	set DistributionTotal = isnull(InsertTotal, 0) + isnull(PostalTotal, 0) + isnull(SampleFreight, 0) + isnull(OtherFreight, 0)

update #tempComponents
	set GrandTotal = ProductionTotal + AssemblyTotal + DistributionTotal

set nocount off
select * from #tempComponents
order by EST_Estimate_ID, EST_Component_ID
set nocount on

drop table #tempVendors
drop table #tempComponents
GO

GRANT  EXECUTE  ON [dbo].[rpt_VendorCommitment_Vendor]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO


GO
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
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EstimateIDs varchar(4000),
@CreatedBy varchar(50)
as

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



GO
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
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EstimateIDs varchar(4000),
@CreatedBy varchar(50)
as

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
				* tc.ComponentTabPageCount / tc.PackageTabPageCount
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


GO
IF OBJECT_ID('dbo.vendorcost_vnd_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vendorcost_vnd_i'
	DROP PROCEDURE dbo.vendorcost_vnd_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vendorcost_vnd_i') IS NOT NULL
		PRINT '***********Drop of dbo.vendorcost_vnd_i FAILED.'
END
GO
PRINT 'Creating dbo.vendorcost_vnd_i'
GO

CREATE PROC dbo.vendorcost_vnd_i
/*
* PARAMETERS:
* EstimateIDs - XML string, the estimates that will be uploaded
*
* DESCRIPTION:
*		Populates vendor_cost table with vendor costs associated with the specified Estimate Ids
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate
*   EST_Component
*   EST_AssemDistribOptions
*   PPR_PaperWeight
*   PPR_PaperGrade
*   VND_Printer
*   PRT_PrinterRate
*   PPR_Paper_Map
*   VND_MailHouse
*   VND_MailTracker
*
*
* PROCEDURES CALLED:
*   dbo.TotalEstimateWeight
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
* 12/06/2007      BJS             Initial Creation - Copied logic from rpt_VendorCommitment_Vendor
* 12/11/2007      BJS             Modified EstimateIDs parameter to allow 4000 characters
* 12/17/2007      BJS             Added Onsert Vendor
* 12/17/2007      JRH             Calculate MediaQuantity from known quantities instead of view.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EstimateIDs varchar(4000),
@CreatedBy varchar(50)
as

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
create table #tempComponents(
	EST_Component_ID bigint,
	EST_Estimate_ID bigint,
	EST_ComponentType_ID int,
	PST_PostalScenario_ID bigint,
	VendorSupplied_ID bigint,
	CreativeVendor_ID bigint,
	SeparatorVendor_ID bigint,
	PrinterVendor_ID bigint,
	PaperVendor_ID bigint,
	AssemblyVendor_ID bigint,
	MailTrackingVendor_ID bigint,
	MailHouseVendor_ID bigint,
	MailListResourceVendor_ID bigint,
	OnsertVendor_ID bigint,
	SampleFreightVendor_ID bigint,
	PostalVendor_ID bigint,
	EstimateMediaType varchar(35),
	ComponentType varchar(35),
	VendorSuppliedDesc varchar(35),
	CreativeVendor varchar(35),
	SeparatorVendor varchar(35),
	PrinterVendor varchar(35),
	PaperVendor varchar(35),
	AssemblyVendor varchar(35),
	MailTrackingVendor varchar(35),
	MailHouseVendor varchar(35),
	MailListResourceVendor varchar(35),
	OnsertVendor varchar(35),
	SampleFreightVendor varchar(35),
	PostalVendor varchar(35),
	/* Main */
		RunDate datetime,
		AdNumber int,
		Description varchar(50),
	/* Specifications */
		PageCount int,
		Width decimal(10,4),
		Height decimal(10,4),
		/* Paper */
			PaperWeight int,
			PaperGrade varchar(35),
		PieceWeight decimal(12,6), /* (Width * Height) / 950,000 * PageCount * PaperWeight * 1.03 */
	/* Quantity */
		InsertQuantity int,
		SoloQuantity int,
		PolyBagQuantity int,
		OtherQuantity int,
		SampleQuantity int,
		MediaQuantity int, /* Insert + Solo + PolyBag */
		SpoilagePct decimal(10,4),
		SpoilageQuantity int, /* SpoilagePct * Media Quantity */
		DirectMailQuantity int, /* Solo + Poly */
		InternalMailQuantity int, /* DirectMail - External List Quantity */
		ExternalMailQuantity int, /* External List Quantity */
		TotalProductionQuantity int, /* MediaQuantity + SpoilageQuantity + OtherQuantity + SampleQuantity */
	/* Production */
		ProductionWeight decimal(14,6), /* TotalProductionQuantity * PieceWeight */
		TotalEstimateWeight decimal(14,6), /* sum(ProductionWeight) group by EST_Estimate_ID*/
		CreativeCPP money,
		CreativeCost money,
		SeparatorCPP money,
		SeparatorCost money,
		/* Print */
			ManualPrintCost money,
			RunRate money,
			NumberOfPlants int,
			AdditionalPlates int,
			PrinterPlateCost money,
			NumberDigitalHandlePrepare int,
			DigitalHandlePrepareRate money,
			StitcherMakeReadyRate money,
			PressMakeReadyRate money,
			ReplacementPlateCost money,
			GrossPrintCost money, /* See Estimate Calculation Steps for formula */
			EarlyPayPrintDiscountPercent decimal(10,4),
			EarlyPayPrintDiscountAmount money, /* GrossPrintCost * EarlyPayPrintDiscountPercent */
			NetPrintCost money, /* GrossPrintCost - EarlyPayPrintDiscountAmount */
			PrinterTaxableMediaPct decimal(10,4),
			PrinterSalesTaxPct decimal(10,4),
			PrinterSalesTaxAmount money, /* (NetPrinterCost + PaperHandling) * PrinterTaxableMediaPct * PrinterSalesTaxPct */
			TotalPrintCost money, /* NetPrintCost + PrinterSalesTaxAmount */
		/* Paper */
			ManualPaperCost money,
			RunPounds decimal(10,2),
			MakereadyPounds int,
			PlateChangePounds decimal(10,2),
			NumberOfPressStops int,
			PressStopPounds int,
			TotalPaperPounds decimal(20,2), /* See Estimate Calculation Steps for formula */
			PaperCWTRate money, /* PPR_Paper_Map.CWT */
			GrossPaperCost money, /* TotalPaperPounds / 100 * PaperCWTRate */
			EarlyPayPaperDiscountPercent decimal(10,4),
			EarlyPayPaperDiscountAmount money, /* GrossPaperCost * EarlyPayPaperDiscountPercent */
			NetPaperCost money, /* GrossPaperCost - EarlyPayPaperDiscountAmount */
			PaperHandlingCWTRate money, /* VND_Printer.PaperHandling */
			PaperHandlingCost money, /* TotalPaperPounds * PaperHandlingCWTRate */
			PaperTaxableMediaPct decimal(10,4),
			PaperSalesTaxPct decimal(10,4),
			PaperSalesTaxAmount money, /* NetPaperCost * PaperTaxableMediaPct * PaperSalesTaxPct */
			TotalPaperCost money, /* NetPaperCost + PaperSalesTaxAmount */
		/* Mail List */
			InternalMailCPM money,
			ExternalMailCPM money,
			BlendedMailListCPM money,
			InternalMailListCost money,
			ExternalMailListCost money,
			MailListCost money, /* See Logic below */
		ProductionSalesTaxAmount money,
		VendorProductionCPM money,
		VendorProductionCost money, /* TotalProductionQuantity / 1000 * VendorProductionCPM  */
		OtherProductionCost money,
		ProductionTotal money, /* CreativeCost + SeparatorCost + TotalPrintCost + TotalPaperCost + ExternalMailCost + VendorProductionCost + OtherProductionCost */
	/* Assembly */
		/* PolyBag */
			PolyBagCost money, /* dbo.EstimatePolyBagCost */
			OnsertRate money,
			OnsertCost money, /* Only if component type is Host Onsert -- PolyBagQuantity / 1000 * OnsertRate */
			PolyBagTotal money, /* PolyBagCost + OnsertCost */
		StitchInRate money,
		StitchInCost money, /* StitchInRate * MediaQuantity (if comp type is stitch-in) / 1000 */
		BlowInRate money,
		BlowInCost money, /* BlowInRate * MediaQuantity (if comp type is blow-in) / 1000 */
		/* Ink Jet */
			InkJetRate money,
			InkJetMakeReadyRate money,
			InkJetCost money, /* DirectMailQuantity / 1000 * InkJetRate */
			GrossInkjetMakereadyCost money, /* InkJetMakeReadyRate * NumberOfPlants */
			InkjetMakereadyCost money, /* See Logic below */
			TotalInkJetCost money, /* InkJetCost + InkJetMakeReadyCost */
		/* Handling */
			/* Insert */
				CornerGuardRate money,
				CornerGuardCost money, /* (MediaQuantity * PieceWeight) / 2000 * CornerGuardRate */
				SkidRate money,
				SkidCost money, /* (MediaQuantity * PieceWeight) / 2000 * SkidRate */
				NumberOfCartons int,
				CartonRate money,
				CartonCost money, /* NumberOfCartons * CartonCost */
				InsertHandlingTotal money, /* CornerGuardCost + SkidCost + CartonCost */
			/* Mail */
				TimeValueSlipsCPM money,
				TimeValueSlipsCost money, /* See SQL Logic Below */
				GrossMailHouseAdminFee money, /* The Mail House Admin Fee. */
				MailHouseAdminFee money, /* See SQL Logic Below */
				GlueTackCPM money,
				GlueTackCost money, /* See SQL Logic Below */
				TabbingCPM money,
				TabbingCost money, /* See SQL Logic Below */
				LetterInsertionCPM money,
				LetterInsertionCost money, /* See SQL Logic Below */
				OtherMailHandlingCPM money,
				OtherMailHandlingCost money, /* See SQL Logic Below */
				MailHandlingTotal money, /* See SQL Logic Below */
			HandlingTotal money, /* InsertHandlingTotal + MailHandlngTotal */
		AssemblyTotal money, /* PolyBagTotal + StitchInCost + BlowInCost + TotalInkJetCost + HandlingTotal */
	/* Distribution */
		/* Insert */
			InsertCost money, /* See Logic below */
			InsertFreightCWT money,
			InsertFreightCost money, /* InsertQuantity * PieceWeight / 100 * InsertFreightCWT */
			InsertFuelSurchargePercent decimal(10,4),
			InsertFuelSurchargeCost money, /* InsertFreightCost * InsertFreightSurchargePercent */
			InsertFreightTotalCost money,
			InsertTotal money, /* InsertCost + InsertFreightCost + InsertFuelSurchargeCost */
		/* Postal */
			PostalDropCost money, /* See Logic Below */
			PostalDropFuelSurchargeCost money, /* See Logic Below*/
			TotalPostalDropCost money,
			MailTrackingCPMRate money,
			MailTrackingCost money, /* DirectMailQuantity / 1000 * MailTrackingCPMRate */
			SoloPostageCost money,
			PolyPostageCost money,
			TotalPostageCost money, /* SoloPostageCost + PolyPostageCost */
			PostalTotal money, /*Postage PostalDropCost + PostalFuelSurcharge + MailTrackingCost + TotalPostageCost */
		SampleFreight money,
		GrossOtherFreight money,
		OtherFreight money,
		DistributionTotal money, /* InsertTotal + PostalTotal + SampleFreight + OtherFreight */ 			
	GrandTotal money
)

/* Get Raw Production Data */
insert into #tempComponents(EST_Component_ID, EST_Estimate_ID, EST_ComponentType_ID, PST_PostalScenario_ID,
	VendorSupplied_ID, CreativeVendor_ID, SeparatorVendor_ID, PrinterVendor_ID, PaperVendor_ID, AssemblyVendor_ID, MailTrackingVendor_ID,
	MailHouseVendor_ID, MailListResourceVendor_ID, OnsertVendor_ID,
	EstimateMediaType, ComponentType,
	VendorSuppliedDesc, CreativeVendor, SeparatorVendor, PrinterVendor, PaperVendor, AssemblyVendor, MailTrackingVendor, MailHouseVendor,
	MailListResourceVendor, OnsertVendor,
	RunDate, AdNumber, Description, PageCount, Width, Height, PaperWeight, PaperGrade, CreativeCPP, CreativeCost, SeparatorCPP, SeparatorCost,
	ExternalMailQuantity, SpoilagePct, ManualPrintCost, RunRate, NumberOfPlants, AdditionalPlates,
	PrinterPlateCost, NumberDigitalHandlePrepare, DigitalHandlePrepareRate, StitcherMakereadyRate, PressMakereadyRate,
	ReplacementPlateCost, EarlyPayPrintDiscountPercent, PrinterTaxableMediaPct,
	PrinterSalesTaxPct, ManualPaperCost, RunPounds, MakeReadyPounds, PlateChangePounds, NumberOfPressStops, PressStopPounds, PaperCWTRate,
	EarlyPayPaperDiscountPercent, PaperHandlingCWTRate, PaperTaxableMediaPct, PaperSalesTaxPct, InternalMailCPM, ExternalMailCPM,
	VendorProductionCPM, OtherProductionCost, OnsertRate, StitchInRate, BlowInRate, InkJetRate, InkJetMakeReadyRate, CornerGuardRate, SkidRate,
	NumberOfCartons, TimeValueSlipsCPM, GrossMailHouseAdminFee, GlueTackCPM, TabbingCPM, LetterInsertionCPM, OtherMailHandlingCPM,
	InsertFreightCWT, InsertFuelSurchargePercent, MailTrackingCPMRate, GrossOtherFreight)
select c.EST_Component_ID, c.EST_Estimate_ID, c.EST_ComponentType_ID, ad.PST_PostalScenario_ID,
	vs_vnd.VND_Vendor_ID, c.CreativeVendor_ID, c.Separator_ID, prt_vnd.VND_Vendor_ID, ppr_vnd.VND_Vendor_ID, assy_vnd.VND_Vendor_ID,
	mt_vnd.VND_Vendor_ID, mh_vnd.VND_Vendor_ID, ml_vnd.VND_Vendor_ID, ons_vnd.VND_Vendor_ID,
	emt.Description, ct.Description,
	vs_vnd.Description, c_vnd.Description, sep_vnd.Description, prt_vnd.Description, ppr_vnd.Description, assy_vnd.Description, mt_vnd.Description,
	mh_vnd.Description, ml_vnd.Description, ons_vnd.Description,
	e.RunDate, c.AdNumber, c.Description, c.PageCount, c.Width, c.Height, pw.Weight, pg.Grade,
	c.CreativeCPP, c.CreativeCPP * c.PageCount, c.SeparatorCPP, c.SeparatorCPP * c.PageCount,
	ad.ExternalMailQty, c.SpoilagePct, c.PrintCost, c.RunRate, c.NumberOfPlants,
	c.AdditionalPlates, pl.Rate, c.NumberDigitalHandlenPrepare, dh.Rate,
	case
		when c.StitcherMakeready_ID is null then c.StitcherMakereadyRate
		else smr.Rate
	end StitcherMakereadyRate,
	case
		when c.PressMakeready_ID is null then c.PressMakereadyRate
		else pmr.Rate
	end PressMakereadyRate,
	c.ReplacementPlateCost, c.EarlyPayPrintDiscount,
	c.PrinterTaxableMediaPct, c.PrinterSalesTaxPct, c.PaperCost, c.RunPounds, c.MakeReadyPounds, c.PlateChangePounds, c.NumberOfPressStops,
	c.PressStopPounds, pm.CWT, c.EarlyPayPaperDiscount, p.PaperHandling, c.PaperTaxableMediaPct, c.PaperSalesTaxPct,
	case
		when c.EST_ComponentType_ID = 1 then ml.InternalListRate
		else null
	end InternalMailCPM,
	case
		when c.EST_ComponentType_ID = 1 then ad.ExternalMailCPM
		else null
	end ExternalMailCPM, c.VendorCPM, c.OtherProduction,
	oi.Rate, si.Rate, bi.Rate,
	case
		when c.EST_ComponentType_ID = 1 then mh.InkJetRate
		else null
	end InkJetRate,
	case
		when c.EST_ComponentType_ID = 1 then mh.InkJetMakeReady
		else null
	end InkJetMakeReady,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.CornerGuards = 1 then p.CornerGuard
		else null
	end CornerGuardRate,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.Skids = 1 then p.Skid
		else null
	end SkidRate,
	ad.NbrOfCartons,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then mh.TimeValueSlips
		else null
	end TimeValueSlipsCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then mh.AdminFee
		else null
	end GrossMailHouseAdminFee,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseGlueTack = 1 then mh.GlueTackRate
		else null
	end GlueTackCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseLetterInsertion = 1 then mh.LetterInsertionRate
		else null
	end LetterInsertionCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseTabbing = 1 then mh.TabbingRate
		else null
	end TabbingCPM,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then ad.MailHouseOtherHandling
		else null
	end OtherMailHandlingCPM,
	ad.InsertFreightCWT, ad.InsertFuelSurcharge,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ and ad.UseMailTracking = 1 then mt.MailTracking
		else null
	end MailTrackingCPMRate,
	case
		when c.EST_ComponentType_ID = 1 /*Host*/ then ad.OtherFreight
		else null
	end OtherFreight
from EST_Estimate e join #tempEstimate te on e.EST_Estimate_ID = te.EST_Estimate_ID
	join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join EST_EstimateMediaType emt on c.EST_EstimateMediaType_ID = emt.EST_EstimateMediaType_ID
	join EST_ComponentType ct on c.EST_ComponentType_ID = ct.EST_ComponentType_ID
	join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
	join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
	left join PPR_PaperGrade pg on c.PaperGrade_ID = pg.PPR_PaperGrade_ID
	left join VND_Vendor vs_vnd on c.VendorSupplied_ID = vs_vnd.VND_Vendor_ID
	left join VND_Vendor c_vnd on c.CreativeVendor_ID = c_vnd.VND_Vendor_ID
	left join VND_Vendor sep_vnd on c.Separator_ID = sep_vnd.VND_Vendor_ID
	left join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	left join VND_Vendor prt_vnd on p.VND_Vendor_ID = prt_vnd.VND_Vendor_ID
	left join VND_Paper ppr on c.Paper_ID = ppr.VND_Paper_ID
	left join VND_Vendor ppr_vnd on ppr.VND_Vendor_ID = ppr_vnd.VND_Vendor_ID
	left join VND_Printer ap on c.AssemblyVendor_ID = ap.VND_Printer_ID
	left join VND_Vendor assy_vnd on ap.VND_Vendor_ID = assy_vnd.VND_Vendor_ID
	left join PRT_PrinterRate pl on c.PlateCost_ID = pl.PRT_PrinterRate_ID /*Plates*/
	left join PRT_PrinterRate dh on c.DigitalHandlenPrepare_ID = dh.PRT_PrinterRate_ID /*Digi H&P*/
	left join PRT_PrinterRate smr on c.StitcherMakeready_ID = smr.PRT_PrinterRate_ID /*Stitcher Makeready*/
	left join PRT_PrinterRate pmr on c.PressMakeready_ID = pmr.PRT_PrinterRate_ID /*Press Makeready*/
	left join PPR_Paper_Map pm on c.Paper_Map_ID = pm.PPR_Paper_Map_ID
	left join PRT_PrinterRate si on c.StitchIn_ID = si.PRT_PrinterRate_ID
	left join PRT_PrinterRate bi on c.BlowIn_ID = bi.PRT_PrinterRate_ID
	left join PRT_PrinterRate oi on c.Onsert_ID = oi.PRT_PrinterRate_ID
	left join VND_Printer ons_prt on oi.VND_Printer_ID = ons_prt.VND_Printer_ID
	left join VND_Vendor ons_vnd on ons_prt.VND_Vendor_ID = ons_vnd.VND_Vendor_ID
	left join VND_MailTrackingRate mt on ad.MailTracking_ID = mt.VND_MailTrackingRate_ID
	left join VND_Vendor mt_vnd on mt.VND_Vendor_ID = mt_vnd.VND_Vendor_ID
	join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
	join VND_Vendor mh_vnd on mh.VND_Vendor_ID = mh_vnd.VND_Vendor_ID
	left join VND_MailListResourceRate ml on ad.MailListResource_ID = ml.VND_MailListResourceRate_ID
	left join VND_Vendor ml_vnd on ml.VND_Vendor_ID = ml_vnd.VND_Vendor_ID
where c.AdNumber is not null or c.EST_ComponentType_ID = 5

/* Sample Freight is allocated to the host printer.  Set it here. */
update tc
	set
		tc.SampleFreightVendor_ID = sample_vnd.VND_Vendor_ID,
		tc.SampleFreightVendor = sample_vnd.Description
from #tempComponents tc join EST_Component c on tc.EST_Estimate_ID = c.EST_Estimate_ID and c.EST_ComponentType_ID = 1
	join VND_Printer sample_prt on c.Printer_ID = sample_prt.VND_Printer_ID
	join VND_Vendor sample_vnd on sample_prt.VND_Vendor_ID = sample_vnd.VND_Vendor_ID

/* Components of type +CVR share the Host Ad Number if they do not have an Ad Number*/
update cvrcomp
	set cvrcomp.AdNumber = hostcomp.AdNumber
from #tempComponents hostcomp join #tempComponents cvrcomp on hostcomp.EST_Estimate_ID = cvrcomp.EST_Estimate_ID
where hostcomp.EST_ComponentType_ID = 1 and cvrcomp.EST_ComponentType_ID = 5 and cvrcomp.AdNumber is null

/* Identify the Postal Vendor.  (Technically there can be more than one, but realistically it is just USPS so this will work) */
update tc
	set tc.PostalVendor_ID = v.VND_Vendor_ID,
		PostalVendor = v.Description
from #tempComponents tc join EST_AssemDistribOptions ad on tc.EST_Estimate_ID = ad.EST_Estimate_ID
	join PST_PostalScenario ps on ad.PST_PostalScenario_ID = ps.PST_PostalScenario_ID
	join PST_PostalCategoryScenario_Map pcsm on ps.PST_PostalScenario_ID = pcsm.PST_PostalScenario_ID
	join PST_PostalCategoryRate_Map pcrm on pcsm.PST_PostalCategoryRate_Map_ID = pcrm.PST_PostalCategoryRate_Map_ID
	join PST_PostalWeights pw on pcrm.PST_PostalWeights_ID = pw.PST_PostalWeights_ID
	join VND_Vendor v on pw.VND_Vendor_ID = v.VND_Vendor_ID
	
/* Perform Specification Calculations */
update #tempComponents
	set PieceWeight = (Width * Height) / cast(950000 as decimal) * PageCount * PaperWeight * 1.03

/* Perform Quantity Calculations */
update tc
	set InsertQuantity = isnull(iq.InsertQuantity, 0)
from #tempComponents tc left join vwComponentInsertQuantity iq on tc.EST_Component_ID = iq.EST_Component_ID

update tc
	set SoloQuantity = isnull(sq.SoloQuantity, 0)
from #tempComponents tc left join vwComponentSoloQuantity sq on tc.EST_Component_ID = sq.EST_Component_ID

update tc
	set PolyBagQuantity = isnull(pq.PolybagQuantity, 0)
from #tempComponents tc left join vwComponentPolybagQuantity pq on tc.EST_Component_ID = pq.EST_Component_ID

update tc
	set OtherQuantity = isnull(oq.OtherQuantity, 0)
from #tempComponents tc left join vwComponentOtherQuantity oq on tc.EST_Component_ID = oq.EST_Component_ID

update tc
	set SampleQuantity = isnull(sq.SampleQuantity, 0)
from #tempComponents tc left join vwComponentSampleQuantity sq on tc.EST_Component_ID = sq.EST_Component_ID

update tc
	set MediaQuantity = tc.InsertQuantity + tc.SoloQuantity + tc.PolyBagQuantity
from #tempComponents tc

update #tempComponents
	set SpoilageQuantity = isnull(SpoilagePct, 0) * MediaQuantity,
		DirectMailQuantity = SoloQuantity + PolybagQuantity,
		InternalMailQuantity = (SoloQuantity + PolybagQuantity) - ExternalMailQuantity

update #tempComponents
	set TotalProductionQuantity = MediaQuantity + SpoilageQuantity + OtherQuantity + SampleQuantity

update tc
	set tc.TotalEstimateWeight = tw.TotalEstimateWeight
from #tempComponents tc join vwTotalEstimateWeight tw on tc.EST_Estimate_ID = tw.EST_Estimate_ID
where tc.EST_ComponentType_ID = 1

update #tempComponents
	set ProductionWeight = TotalProductionQuantity * PieceWeight

/* Perform Production Calculations */
update #tempComponents
	set GrossPrintCost = ManualPrintCost
where ManualPrintCost is not null

update #tempComponents
	set GrossPrintCost = TotalProductionQuantity / cast(1000 as decimal) * isnull(RunRate, 0)
		+ isnull(NumberOfPlants, 0) * (isnull(StitcherMakeReadyRate, 0) + isnull(PressMakeReadyRate, 0))
		+ isnull(AdditionalPlates, 0) * isnull(PrinterPlateCost, 0)
		+ isnull(NumberDigitalHandlePrepare, 0) * isnull(DigitalHandlePrepareRate, 0)
		+ isnull(ReplacementPlateCost, 0)
where ManualPrintCost is null

update #tempComponents
	set EarlyPayPrintDiscountAmount = GrossPrintCost * isnull(EarlyPayPrintDiscountPercent, 0),
		NetPrintCost = GrossPrintCost - (GrossPrintCost * isnull(EarlyPayPrintDiscountPercent, 0))

/*TODO: We don't have a paper handling amount if manualpapercost is entered.  How does this effect the print sales tax?? */
update #tempComponents
	set GrossPaperCost = ManualPaperCost
where ManualPaperCost is not null

update #tempComponents
	set TotalPaperPounds = TotalProductionQuantity / cast(1000 as decimal) * isnull(RunPounds, 0)
		+ cast(isnull(NumberOfPlants, 0) as bigint) * cast(isnull(MakeReadyPounds, 0) as bigint)
		+ cast(isnull(AdditionalPlates, 0) as bigint) * isnull(PlateChangePounds, 0)
		+ cast(isnull(NumberOfPressStops, 0) as bigint) * cast(isnull(PressStopPounds, 0) as bigint)
where ManualPaperCost is null

update #tempComponents
	set GrossPaperCost = TotalPaperPounds / cast(100 as decimal) * PaperCWTRate
where ManualPaperCost is null

update #tempComponents
	set EarlyPayPaperDiscountAmount = GrossPaperCost * isnull(EarlyPayPaperDiscountPercent, 0),
		NetPaperCost = GrossPaperCost - (GrossPaperCost * isnull(EarlyPayPaperDiscountPercent, 0))

update #tempComponents
	set PaperHandlingCost = isnull(TotalPaperPounds, 0) / cast(100 as decimal) * isnull(PaperHandlingCWTRate, 0)

update #tempComponents
	set PrinterSalesTaxAmount = (NetPrintCost + PaperHandlingCost) * isnull(PrinterTaxableMediaPct, 0) * isnull(PrinterSalesTaxPct, 0),
		PaperSalesTaxAmount = NetPaperCost * isnull(PaperTaxableMediaPct, 0) * isnull(PaperSalesTaxPct, 0)

update #tempComponents
	set ProductionSalesTaxAmount = PrinterSalesTaxAmount + PaperSalesTaxAmount

update #tempComponents
	set TotalPrintCost = NetPrintCost + PrinterSalesTaxAmount,
		TotalPaperCost = NetPaperCost + PaperHandlingCost + PaperSalesTaxAmount,
		VendorProductionCost = TotalProductionQuantity / cast(1000 as decimal) * VendorProductionCPM

update #tempComponents
	set BlendedMailListCPM =
		case
			when dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) = 0 then 0
			else InternalMailCPM
					* cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) - ExternalMailQuantity as decimal)
					/ cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
				+ ExternalMailCPM
					* cast(ExternalMailQuantity as decimal)
					/ cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
		end
where EST_ComponentType_ID = 1

update #tempComponents
	set MailListCost = BlendedMailListCPM * SoloQuantity / 1000 + isnull(dbo.ComponentPolybagMailListCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponents
	set ProductionTotal = isnull(CreativeCost, 0) + isnull(SeparatorCost, 0) + TotalPrintCost + TotalPaperCost + isnull(MailListCost, 0) +
		+ isnull(VendorProductionCost, 0) + isnull(OtherProductionCost, 0)

/* Assembly Calculations */

update #tempComponents
	set PolyBagCost = dbo.EstimatePolyBagCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1

update #tempComponents
	set OnsertCost = PolybagQuantity / cast(1000 as decimal) * OnsertRate
where EST_ComponentType_ID = 2

update #tempComponents
	set GrossInkjetMakereadyCost = NumberofPlants * InkjetMakereadyRate

update #tempComponents
	set PolyBagTotal = isnull(PolyBagCost, 0) + isnull(OnsertCost, 0),
		StitchInCost =
			case EST_ComponentType_ID
				when 3 then MediaQuantity / cast(1000 as decimal) * StitchInRate
				else null
			end,
		BlowInCost =
			case EST_ComponentType_ID
				when 4 then MediaQuantity / cast(1000 as decimal) * BlowInRate
			end,
		InkJetCost =
			case EST_ComponentType_ID
				when 1 then (SoloQuantity / cast(1000 as decimal) * InkJetRate) + isnull(dbo.EstimatePolybagInkjetCost(EST_Estimate_ID), 0)
			end,
		InkJetMakeReadyCost =
			case EST_ComponentType_ID
				when 1 then
					case
						when SoloQuantity = 0 then 0
						else GrossInkjetMakereadyCost * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
					end
					+ isnull(dbo.ComponentPolybagInkjetMakereadyCost(EST_Component_ID), 0)
			end

update #tempComponents
		set TotalInkJetCost = isnull(InkJetCost, 0) + isnull(InkJetMakeReadyCost, 0)

update tc
	set CartonRate = pr.Rate
from #tempComponents tc join EST_Component c on tc.EST_Component_ID = c.EST_Component_ID
	join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate pr on p.VND_Printer_ID = pr.VND_Printer_ID and pr.PRT_PrinterRateType_ID = 3 and pr.[Default] = 1

update #tempComponents
	set CornerGuardCost = TotalEstimateWeight / cast(2000 as decimal) * CornerGuardRate,
		SkidCost = TotalEstimateWeight / cast(2000 as decimal) * SkidRate,
		CartonCost = NumberOfCartons * CartonRate,
		TimeValueSlipsCost = TimeValueSlipsCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseTimeValueSlipsCost(EST_Component_ID), 0),
		MailHouseAdminFee =
			case
				when SoloQuantity = 0 then 0
				else GrossMailHouseAdminFee * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
			end
			+ isnull(dbo.ComponentPolybagMailHouseAdminFee(EST_Component_ID), 0),
		GlueTackCost = GlueTackCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseGlueTackCost(EST_Component_ID), 0),
		TabbingCost = TabbingCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseTabbingCost(EST_Component_ID), 0),
		LetterInsertionCost = LetterInsertionCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseLetterInsertionCost(EST_Component_ID), 0),
		OtherMailHandlingCost = OtherMailHandlingCPM * SoloQuantity / cast(1000 as decimal)
			+ isnull(dbo.ComponentPolybagMailHouseOtherDirectMailHandlingCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponents
	set InsertHandlingTotal = isnull(CornerGuardCost, 0) + isnull(SkidCost, 0) + isnull(CartonCost, 0),
		MailHandlingTotal = isnull(TimeValueSlipsCost, 0) + isnull(MailHouseAdminFee, 0) + isnull(GlueTackCost, 0) + isnull(TabbingCost, 0)
			+ isnull(LetterInsertionCost, 0) + isnull(OtherMailHandlingCost, 0)

update #tempComponents
	set HandlingTotal = InsertHandlingTotal + MailHandlingTotal

update #tempComponents
	set AssemblyTotal = PolyBagTotal + isnull(StitchInCost, 0) + isnull(BlowInCost, 0) + TotalInkJetCost + HandlingTotal

/* Distribution Calculations */

create table #tempPackagesByPubRate(
	EST_Component_ID bigint not null,
	EST_Package_ID bigint not null,
	PubRate_Map_ID bigint not null,
	PUB_PubRate_ID bigint not null,
	PUB_PubQuantity_ID bigint not null,
	QuantityType int not null,
	InsertDate datetime not null,
	InsertDOW int not null,
	BlowInRate int null,
	PackageTabPageCount int null,
	ComponentPieceWeight decimal(12,6) not null,
	PackageWeight decimal(12,6) not null,
	PackageSize int not null,
	BilledQuantity int,
	GrossPackageInsertCost money null,
	InsertDiscountPercent decimal(10,4),
	PackageComponentCost money null)

insert into #tempPackagesByPubRate(EST_Component_ID, EST_Package_ID, PubRate_Map_ID, PUB_PubRate_ID, PUB_PubQuantity_ID, QuantityType,
	InsertDate, InsertDOW, ComponentPieceWeight, PackageWeight, PackageSize)
select pcm.EST_Component_ID, p.EST_Package_ID, pprm.PUB_PubRate_Map_ID, dbo.CalcPubRateID(pprm.PUB_PubRate_Map_ID, pid.IssueDate),
	dbo.CalcPubQuantityID(pprm.PUB_PubRate_Map_ID, pid.IssueDate), p.PUB_PubQuantityType_ID, pid.IssueDate,
	case p.PUB_PubQuantityType_ID
		when 4 then 1 /* Thanksgiving always uses Sunday Rates */
		when 5 then 1 /* Christmas always uses Sunday Rates */
		when 6 then 1 /* New Years always uses Sunday Rates */
		else pid.IssueDOW
	end IssueDOW,
	tc.PieceWeight, dbo.PackageWeight(p.EST_Package_ID), dbo.PackageSize(p.EST_Package_ID)
from #tempComponents tc join EST_PackageComponentMapping pcm on tc.EST_Component_ID = pcm.EST_Component_ID
	join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	join PUB_PubGroup pg on p.PUB_PubGroup_ID = pg.PUB_PubGroup_ID
	join PUB_PubPubGroup_Map ppgm on pg.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID
	join PUB_PubRate_Map pprm on ppgm.PUB_PubRate_Map_ID = pprm.PUB_PubRate_Map_ID
	join EST_PubIssueDates pid on p.EST_Estimate_ID = pid.EST_Estimate_ID and pprm.PUB_PubRate_Map_ID = pid.PUB_PubRate_Map_ID
where dbo.IsPubRateMapActive(pprm.PUB_PubRate_Map_ID, pid.IssueDate) = 1
	and dbo.CalcPubRateID(pprm.PUB_PubRate_Map_ID, pid.IssueDate) is not null
	and dbo.CalcPubQuantityID(pprm.PUB_PubRate_Map_ID, pid.IssueDate) is not null

update tp
	set tp.BlowInRate = pr.BlowInRate
from #tempPackagesByPubRate tp join PUB_PubRate pr on tp.PUB_PubRate_ID = pr.PUB_PubRate_ID

update #tempPackagesByPubRate
	set PackageTabPageCount = dbo.PackageTabPageCount(EST_Package_ID, BlowInRate)

update #tempPackagesByPubRate
	set BilledQuantity =
		case pr.QuantityChargeType
			when 1 then q.Quantity - (q.Quantity * pr.BilledPct)
			else q.Quantity
		end
from #tempPackagesByPubRate t join PUB_DayOfWeekQuantity q on t.PUB_PubQuantity_ID = q.PUB_PubQuantity_ID and t.QuantityType = q.PUB_PubQuantityType_ID
		and (t.QuantityType > 3 /*Holidays*/ or datepart(dw, t.InsertDate) = q.InsertDow /*Full Run / Contract Send*/) 
	join PUB_PubRate pr on t.PUB_PubRate_ID = pr.PUB_PubRate_ID

update #tempPackagesByPubRate
	set GrossPackageInsertCost = dbo.CalcGrossInsertCostforPackageandPub(PUB_PubRate_ID, InsertDate, InsertDOW, PackageTabPageCount, BilledQuantity, PackageWeight, PackageSize)

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
from #tempPackagesByPubRate t join #tempPackageInsertNumber pin on t.EST_Package_ID = pin.EST_Package_ID and t.PubRate_Map_ID = pin.PUB_PubRate_Map_ID
	join PUB_InsertDiscounts d on t.PUB_PubRate_ID = d.PUB_PubRate_ID
where pin.InsertNumber = d.[Insert]

drop table #tempPackageInsertNumber

/*----------------------------------------- */

update tp
	set PackageComponentCost =
		case
			when PackageWeight = 0 then 0
			else
				(GrossPackageInsertCost * (1 - isnull(InsertDiscountPercent, 0))) * ComponentPieceWeight / PackageWeight
		end
from #tempPackagesByPubRate tp

update tc
	set InsertCost = isnull((select sum(PackageComponentCost) from #tempPackagesByPubRate tp where tp.EST_Component_ID = tc.EST_Component_ID), 0)
from #tempComponents tc

drop table #tempPackagesByPubRate

update #tempComponents
	set InsertFreightCost = dbo.TotalEstimateWeight(#tempComponents.EST_Estimate_ID) / cast(100 as decimal) * InsertFreightCWT,
		InsertFuelSurchargeCost = (dbo.TotalEstimateWeight(#tempComponents.EST_Estimate_ID) / cast(100 as decimal) * InsertFreightCWT) * InsertFuelSurchargePercent
where #tempComponents.EST_ComponentType_ID = 1

update #tempComponents
	set InsertFreightTotalCost = isnull(InsertFreightCost, 0) + isnull(InsertFuelSurchargeCost, 0)

update #tempComponents
	set InsertTotal = InsertCost + InsertFreightTotalCost

update #tempComponents
	set PostalDropCost = dbo.EstimatePostalDropCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1

update #tempComponents
	set PostalDropFuelSurchargeCost = dbo.EstimatePostalDropFuelSurchargeCost(EST_Estimate_ID)
where EST_ComponentType_ID = 1

update #tempComponents
	set TotalPostalDropCost = isnull(PostalDropCost, 0) + isnull(PostalDropFuelSurchargeCost, 0)
where EST_ComponentType_ID = 1

update #tempComponents
	set	MailTrackingCost = isnull((SoloQuantity / cast(1000 as decimal) * MailTrackingCPMRate), 0) + isnull(dbo.ComponentPolybagMailTrackingCost(EST_Component_ID), 0)
where EST_ComponentType_ID = 1

update #tempComponents
	set SoloPostageCost = isnull(dbo.ComponentSoloPostage(EST_Component_ID, PieceWeight, PST_PostalScenario_ID), 0)

update #tempComponents
	set PolyPostageCost = isnull(dbo.ComponentPolyPostage(EST_Component_ID, PieceWeight), 0)

update #tempComponents
	set TotalPostageCost = SoloPostageCost + PolyPostageCost

update #tempComponents
	set PostalTotal = TotalPostalDropCost + isnull(MailTrackingCost, 0) + TotalPostageCost

update t
	set SampleFreight =
		case
			when isnull(s.FreightCWT, 0) <> 0 then dbo.EstimatePieceWeight(t.EST_Estimate_ID) * SampleQuantity / cast(100 as decimal) * s.FreightCWT
			else s.FreightFlat
		end
from #tempComponents t join EST_Samples s on t.EST_Estimate_ID = s.EST_Estimate_ID
where t.EST_ComponentType_ID = 1

update #tempComponents
	set OtherFreight = GrossOtherFreight
/* Not sure if this cost should be distributed across Polybags */
/*
			case
				when isnull(SoloQuantity, 0) = 0 then 0
				else GrossOtherFreight * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
			end
			+ isnull(dbo.ComponentPolybagOtherFreightCost(EST_Component_ID), 0)
*/
where EST_ComponentType_ID = 1

update #tempComponents
	set DistributionTotal = isnull(InsertTotal, 0) + isnull(PostalTotal, 0) + isnull(SampleFreight, 0) + isnull(OtherFreight, 0)

update #tempComponents
	set GrandTotal = ProductionTotal + AssemblyTotal + DistributionTotal

/* Delete records from the vendor_cost table that match on ad number.  Current costs will be written. */
delete v
from vendor_cost v join #tempComponents tc on v.AdNumber = tc.AdNumber and v.RunDate = tc.RunDate

/* Insert new records into the vendor_cost table */
/* 530 - Creative / Photography */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select CreativeVendor_ID, 530, AdNumber, isnull(max(CreativeVendor), 'N/A') VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Creative / Photography' CostDescription, sum(CreativeCost) GrossCost,
	null Discount, sum(CreativeCost) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where CreativeCost > 0
group by CreativeVendor_ID, AdNumber, RunDate

/* 595 - Color Separations */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select SeparatorVendor_ID, 595, AdNumber, isnull(max(SeparatorVendor), 'N/A') VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Color Separations' CostDescription, sum(SeparatorCost) GrossCost,
	null Discount, sum(SeparatorCost) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where SeparatorCost > 0
group by SeparatorVendor_ID, AdNumber, RunDate

/* 610 - Print */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select PrinterVendor_ID, 610, AdNumber, isnull(max(PrinterVendor), 'N/A') VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Print' CostDescription, sum(GrossPrintCost) GrossCost,
	sum(EarlyPayPrintDiscountAmount) Discount, sum(NetPrintCost) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where (GrossPrintCost > 0 or EarlyPayPrintDiscountAmount > 0 or NetPrintCost > 0)
group by PrinterVendor_ID, AdNumber, RunDate

/* 605 - Paper */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select PaperVendor_ID, 605, AdNumber, isnull(max(PaperVendor), 'N/A') VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Paper' CostDescription, sum(GrossPaperCost) GrossCost,
	sum(EarlyPayPaperDiscountAmount) Discount, sum(NetPaperCost) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where (GrossPaperCost > 0 or EarlyPayPaperDiscountAmount > 0 or NetPaperCost > 0)
group by PaperVendor_ID, AdNumber, RunDate

/* 606 - Paper Handling */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select PaperVendor_ID, 606, AdNumber, isnull(max(PaperVendor), 'N/A') VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Paper Handling' CostDescription, sum(PaperHandlingCost) GrossCost,
	null Discount, sum(PaperHandlingCost) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where PaperHandlingCost > 0
group by PaperVendor_ID, AdNumber, RunDate

/* 615 - Sales Tax (Printer)*/
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select PrinterVendor_ID, 615, AdNumber, isnull(max(PrinterVendor), 'N/A') VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Sales Tax (Printer)' CostDescription, sum(PrinterSalesTaxAmount) GrossCost,
	null Discount, sum(PrinterSalesTaxAmount) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where PrinterSalesTaxAmount > 0
group by PrinterVendor_ID, AdNumber, RunDate

/* 615 - Sales Tax (Paper) */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select PaperVendor_ID, 615, AdNumber, isnull(max(PaperVendor), 'N/A') VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Sales Tax (Paper)' CostDescription, sum(PaperSalesTaxAmount) GrossCost,
	null Discount, sum(PaperSalesTaxAmount) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where PaperSalesTaxAmount > 0
group by PaperVendor_ID, AdNumber, RunDate

/* 760 - Mail List (Internal) */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select MailListResourceVendor_ID, 760, AdNumber, isnull(max(MailListResourceVendor), 'N/A') VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Mail List' CostDescription, sum(MailListCost) GrossCost,
	null Discount, sum(MailListCost) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where MailListCost > 0
group by MailListResourceVendor_ID, AdNumber, RunDate

/* 880 - Specialty Other */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select isnull(PrinterVendor_ID, -1) VendorID, 880, AdNumber, isnull(max(PrinterVendor), 'N/A') VendorDescription,
	RunDate, max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Specialty Other' CostDescription, sum(OtherProductionCost) GrossCost,
	null Discount, sum(OtherProductionCost) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where OtherProductionCost > 0
group by PrinterVendor_ID, AdNumber, RunDate

/* 870 - VS Production */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select VendorSupplied_ID, 870, AdNumber, isnull(max(VendorSuppliedDesc), 'N/A') VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'VS Production' CostDescription, sum(VendorProductionCost) GrossCost,
	null Discount, sum(VendorProductionCost) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where VendorProductionCost > 0
group by VendorSupplied_ID, AdNumber, RunDate

/* 720 - Stitch-Ins / Blow-Ins (Stitch-Ins) */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select AssemblyVendor_ID, 720, AdNumber, isnull(max(AssemblyVendor), 'N/A') VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Stitch-Ins / Blow-Ins' CostDescription, sum(StitchInCost) GrossCost,
	null Discount, sum(StitchInCost) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where StitchInCost > 0
group by AssemblyVendor_ID, AdNumber, RunDate

/* 720 - Stitch-Ins / Blow-Ins (Blow-Ins) */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select AssemblyVendor_ID, 720, AdNumber, isnull(max(AssemblyVendor), 'N/A') VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Stitch-Ins / Blow-Ins' CostDescription, sum(BlowInCost) GrossCost,
	null Discount, sum(BlowInCost) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where BlowInCost > 0
group by AssemblyVendor_ID, AdNumber, RunDate

/* 730 - Polybag */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select OnsertVendor_ID, 730, AdNumber, isnull(max(OnsertVendor), 'N/A') VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Polybag' CostDescription, sum(OnsertCost) GrossCost,
	null Discount, sum(OnsertCost) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where OnsertCost > 0
group by OnsertVendor_ID, AdNumber, RunDate

/* 745 - Ink Jet */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select MailHouseVendor_ID, 745, AdNumber, isnull(max(MailHouseVendor), 'N/A') VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Ink Jet' CostDescription, sum(TotalInkJetCost) GrossCost,
	null Discount, sum(TotalInkJetCost) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where TotalInkJetCost > 0
group by MailHouseVendor_ID, AdNumber, RunDate

/* 740 - Handling (Mail House) */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select MailHouseVendor_ID, 740, AdNumber, isnull(max(MailHouseVendor), 'N/A') VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Handling' CostDescription, sum(MailHandlingTotal) GrossCost,
	null Discount, sum(MailHandlingTotal) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where MailHandlingTotal > 0
group by MailHouseVendor_ID, AdNumber, RunDate

/* 740 - Handling (Insert) */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select PrinterVendor_ID, 740, AdNumber, isnull(max(PrinterVendor), 'N/A') VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Handling' CostDescription, sum(InsertHandlingTotal) GrossCost,
	null Discount, sum(InsertHandlingTotal) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where InsertHandlingTotal > 0
group by PrinterVendor_ID, AdNumber, RunDate

/* 820 - Postal Drop */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select PrinterVendor_ID, 820, AdNumber, isnull(max(PrinterVendor), 'N/A') VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Postal Drop' CostDescription, sum(TotalPostalDropCost) GrossCost,
	null Discount, sum(TotalPostalDropCost) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where TotalPostalDropCost > 0
group by PrinterVendor_ID, AdNumber, RunDate

/* 830 - Newspaper Freight */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select PrinterVendor_ID, 830, AdNumber, isnull(max(PrinterVendor), 'N/A') VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Newspaper Freight' CostDescription, sum(InsertFreightTotalCost) GrossCost,
	null Discount, sum(InsertFreightTotalCost) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where InsertFreightTotalCost > 0
group by PrinterVendor_ID, AdNumber, RunDate

/* 810 - Sample Shipping */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select isnull(SampleFreightVendor_ID, -1) VendorID, 810, AdNumber, isnull(max(SampleFreightVendor), 'N/A') VendorDescription,
	RunDate, max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Sample Shipping' CostDescription, sum(SampleFreight) GrossCost,
	null Discount, sum(SampleFreight) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where SampleFreight > 0
group by SampleFreightVendor_ID, AdNumber, RunDate

/* 855 - Other Distribution */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select isnull(PrinterVendor_ID, -1) VendorID, 810, AdNumber, isnull(max(PrinterVendor), 'N/A') VendorDescription,
	RunDate, max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Other Distribution' CostDescription, sum(OtherFreight) GrossCost,
	null Discount, sum(OtherFreight) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where OtherFreight > 0
group by PrinterVendor_ID, AdNumber, RunDate

/* 750 - Mail Tracking */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select MailTrackingVendor_ID, 750, AdNumber, isnull(max(MailTrackingVendor), 'N/A') VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Mail Tracking' CostDescription, sum(MailTrackingCost) GrossCost,
	null Discount, sum(MailTrackingCost) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where MailTrackingCost > 0
group by MailTrackingVendor_ID, AdNumber, RunDate

/* 840 - Postage */
insert into vendor_cost(VND_Vendor_ID, CostCode, AdNumber, VendorDescription, RunDate, AdDescription,
	Pages, MediaQuantity, PubQuantity, CostDescription, GrossCost, Discount, NetCost, CreatedBy, CreatedDate)
select PostalVendor_ID, 840, AdNumber, isnull(max(PostalVendor), 'N/A') VendorDescription, RunDate,
	max(Description) AdDescription, sum(PageCount) pages, sum(MediaQuantity) MediaQuantity,
	null PubQuantity, 'Postage' CostDescription, sum(TotalPostageCost) GrossCost,
	null Discount, sum(TotalPostageCost) NetCost, @CreatedBy, getdate() CreatedDate
from #tempComponents
where TotalPostageCost > 0
group by PostalVendor_ID, AdNumber, RunDate

drop table #tempVendors
drop table #tempComponents
drop table #tempEstimate
GO

GRANT  EXECUTE  ON [dbo].[vendorcost_vnd_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO


GO
IF OBJECT_ID('dbo.VndMailHouseRate_d_ByMailHouseRateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailHouseRate_d_ByMailHouseRateID'
	DROP PROCEDURE dbo.VndMailHouseRate_d_ByMailHouseRateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailHouseRate_d_ByMailHouseRateID') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailHouseRate_d_ByMailHouseRateID FAILED.'
END
GO
PRINT 'Creating dbo.VndMailHouseRate_d_ByMailHouseRateID'
GO

CREATE PROC dbo.VndMailHouseRate_d_ByMailHouseRateID
@VND_MailHouseRate_ID bigint,
@ModifiedBy varchar(50)
/*
* PARAMETERS:
* VND_MailHouseRate_ID
*
* DESCRIPTION:
* Deletes a MailHouse record.  Updates any A&D records that may be affected.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   VND_MailHouseRate              DELETE
*   EST_AssemDistribOptions        UPDATE
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
* 10/05/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
as

declare @new_MailHouseRate_ID bigint
select top 1 @new_MailHouseRate_ID = new_mh.VND_MailHouseRate_ID
from VND_MailHouseRate orig_mh join VND_MailHouseRate new_mh on orig_mh.VND_Vendor_ID = new_mh.VND_Vendor_ID
where orig_mh.VND_MailHouseRate_ID = @VND_MailHouseRate_ID and orig_mh.VND_MailHouseRate_ID <> new_mh.VND_MailHouseRate_ID
order by new_mh.EffectiveDate

--If there is not an earlier set of Mail House Rates for this Vendor, delete the record
if (@new_MailHouseRate_ID is null) begin
	if exists(select 1 from EST_AssemDistribOptions where MailHouse_ID = @VND_MailHouseRate_ID) begin
		raiserror('Cannot delete Mail House Rate.  It is being referenced by an estimate.', 16, 1)
		return
	end
	else begin
		delete from VND_MailHouseRate
		where VND_MailHouseRate_ID = @VND_MailHouseRate_ID
		return
	end
end

update e
	set
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_Estimate e join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
where ad.MailHouse_ID = @VND_MailHouseRate_ID

update ad
	set
		MailHouse_ID = @new_MailHouseRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_AssemDistribOptions ad
where ad.MailHouse_ID = @VND_MailHouseRate_ID

if exists(select 1 from EST_AssemDistribOptions where MailHouse_ID = @VND_MailHouseRate_ID) begin
	raiserror('Cannot delete Mail House Rate.  It is being referenced by an estimate.', 16, 1)
	return
end
else begin
	delete from VND_MailHouseRate
	where VND_MailHouseRate_ID = @VND_MailHouseRate_ID
	return
end
GO

GRANT  EXECUTE  ON [dbo].[VndMailHouseRate_d_ByMailHouseRateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.VndMailHouseRate_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailHouseRate_i'
	DROP PROCEDURE dbo.VndMailHouseRate_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailHouseRate_i') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailHouseRate_i FAILED.'
END
GO
PRINT 'Creating dbo.VndMailHouseRate_i'
GO

CREATE PROC dbo.VndMailHouseRate_i
@VND_MailHouseRate_ID bigint output,
@VND_Vendor_ID bigint,
@TimeValueSlips money,
@InkjetRate money,
@InkjetMakeready money,
@AdminFee money,
@PostalDropCWT money,
@GlueTackDefault bit,
@GlueTackRate money,
@TabbingDefault bit,
@TabbingRate money,
@LetterInsertionDefault bit,
@LetterInsertionRate money,
@EffectiveDate datetime,
@CreatedBy varchar(50)
/*
* PARAMETERS:
* VND_MailHouseRate_ID
* VND_Vendor_ID
* TimeValueSlips
* InkjetRate
* InkjetMakeready
* AdminFee
* PostalDropCWT
* GlueTackDefault
* GlueTackRate
* TabbingDefault
* TabbingRate
* LetterInsertionDefault
* LetterInsertionRate
* EffectiveDate
* CreatedBy
*
* DESCRIPTION:
* Inserts a MailHouse record.  Updates any A&D records that may be affected. Returns the MailHouse ID.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   VND_MailHouseRate              INSERT
*   EST_AssemDistribOptions        UPDATE
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
* 10/05/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
as

declare @old_MailHouseRate_ID bigint
select top 1 @old_MailHouseRate_ID = VND_MailHouseRate_ID
from VND_MailHouseRate
where VND_Vendor_ID = @VND_Vendor_ID and EffectiveDate < @EffectiveDate
order by EffectiveDate

insert into VND_MailHouseRate(VND_Vendor_ID, TimeValueSlips, InkjetRate, InkjetMakeready, AdminFee, PostalDropCWT, GlueTackDefault,
	GlueTackRate, TabbingDefault, TabbingRate, LetterInsertionDefault, LetterInsertionRate, EffectiveDate, CreatedBy, CreatedDate)
values(@VND_Vendor_ID, @TimeValueSlips, @InkjetRate, @InkjetMakeready, @AdminFee, @PostalDropCWT, @GlueTackDefault,
	@GlueTackRate, @TabbingDefault, @TabbingRate, @LetterInsertionDefault, @LetterInsertionRate, @EffectiveDate, @CreatedBy, getdate())
set @VND_MailHouseRate_ID = @@identity
if (@@error <> 0) begin
	raiserror('Cannot create VND_MailHouseRate record.', 16, 1)
	return
end

update e
	set e.ModifiedBy = @CreatedBy,
		e.ModifiedDate = getdate()
from EST_Estimate e join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
where e.RunDate >= @EffectiveDate and ad.MailHouse_ID = @old_MailHouseRate_ID
if (@@error <> 0) begin
	raiserror('Cannot update EST_Estimate record.', 16, 1)
	return
end

update ad
	set
		ad.MailHouse_ID = @VND_MailHouseRate_ID,
		ad.ModifiedBy = @CreatedBy,
		ad.ModifiedDate = getdate()
from EST_Estimate e join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
where e.RunDate >= @EffectiveDate and ad.MailHouse_ID = @old_MailHouseRate_ID
if (@@error <> 0) begin
	raiserror('Cannot update EST_AssemDistribOptions record.', 16, 1)
	return
end
GO

GRANT  EXECUTE  ON [dbo].[VndMailHouseRate_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.VndMailHouseRate_s_ByDescriptionandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailHouseRate_s_ByDescriptionandRunDate'
	DROP PROCEDURE dbo.VndMailHouseRate_s_ByDescriptionandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailHouseRate_s_ByDescriptionandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailHouseRate_s_ByDescriptionandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.VndMailHouseRate_s_ByDescriptionandRunDate'
GO

create proc dbo.VndMailHouseRate_s_ByDescriptionandRunDate
/*
* PARAMETERS:
* Description - Required
* RunDate - Required
*
*
* DESCRIPTION:
*		Returns a MailHouseRate Record for the given Vendor Description and Run Date.
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		VND_Vendor          READ
*		VND_MailHouseRate   READ
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
*	Date            Who             Comments
*	-------------   --------        -------------------------------------------------
*   09/05/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Description varchar(35),
@RunDate datetime
as

select mh.*
from VND_Vendor v join VND_MailHouseRate mh on v.VND_Vendor_ID = mh.VND_Vendor_ID
	left join VND_MailHouseRate newer_mh on v.VND_Vendor_ID = newer_mh.VND_Vendor_ID and newer_mh.EffectiveDate <= @RunDate and newer_mh.EffectiveDate > mh.EffectiveDate
where v.Description = @Description and mh.EffectiveDate <= @RunDate and newer_mh.VND_MailHouseRate_ID is null
GO

GRANT  EXECUTE  ON [dbo].[VndMailHouseRate_s_ByDescriptionandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.VndMailHouseRate_s_ByOldIDandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailHouseRate_s_ByOldIDandRunDate'
	DROP PROCEDURE dbo.VndMailHouseRate_s_ByOldIDandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailHouseRate_s_ByOldIDandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailHouseRate_s_ByOldIDandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.VndMailHouseRate_s_ByOldIDandRunDate'
GO

CREATE proc dbo.VndMailHouseRate_s_ByOldIDandRunDate
/*
* PARAMETERS:
* VND_MailHouseRate_ID - required
* RunDate              - required
*
* DESCRIPTION:
*		Determines the parent vendor record of VND_MailHouseRate_ID.  Returns a mailhouse record with the same parent vendor for the specified run date
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   VND_MailHouseRate   READ
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
* 11/02/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_MailHouseRate_ID bigint,
@RunDate datetime
as

declare @VND_Vendor_ID bigint

select @VND_Vendor_ID = VND_Vendor_ID
from VND_MailHouseRate
where VND_MailHouseRate_ID = @VND_MailHouseRate_ID

select top 1 mh.*
from VND_MailHouseRate mh
where mh.VND_Vendor_ID = @VND_Vendor_ID and mh.EffectiveDate <= @RunDate
order by mh.EffectiveDate desc
GO

GRANT  EXECUTE  ON [dbo].[VndMailHouseRate_s_ByOldIDandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.VndMailListResourceRate_d_ByMailListResourceRateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailListResourceRate_d_ByMailListResourceRateID'
	DROP PROCEDURE dbo.VndMailListResourceRate_d_ByMailListResourceRateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailListResourceRate_d_ByMailListResourceRateID') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailListResourceRate_d_ByMailListResourceRateID FAILED.'
END
GO
PRINT 'Creating dbo.VndMailListResourceRate_d_ByMailListResourceRateID'
GO

CREATE PROC dbo.VndMailListResourceRate_d_ByMailListResourceRateID
@VND_MailListResourceRate_ID bigint,
@ModifiedBy varchar(50)
/*
* PARAMETERS:
* VND_MailListResourceRate_ID
*
* DESCRIPTION:
* Deletes a MailListResource record.  Updates any A&D records that may be affected.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   VND_MailListResourceRate       DELETE
*   EST_AssemDistribOptions        UPDATE
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
* 10/08/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
as

declare @new_MailListResourceRate_ID bigint
select top 1 @new_MailListResourceRate_ID = new_ml.VND_MailListResourceRate_ID
from VND_MailListResourceRate orig_ml join VND_MailListResourceRate new_ml on orig_ml.VND_Vendor_ID = new_ml.VND_Vendor_ID
where orig_ml.VND_MailListResourceRate_ID = @VND_MailListResourceRate_ID and orig_ml.VND_MailListResourceRate_ID <> new_ml.VND_MailListResourceRate_ID
order by new_ml.EffectiveDate

--If there is not an earlier set of Mail House Rates for this Vendor, delete the record
if (@new_MailListResourceRate_ID is null) begin
	if exists(select 1 from EST_AssemDistribOptions where MailListResource_ID = @VND_MailListResourceRate_ID) begin
		raiserror('Cannot delete Mail List Resource Rate.  It is being referenced by an estimate.', 16, 1)
		return
	end
	else begin
		delete from VND_MailListResourceRate
		where VND_MailListResourceRate_ID = @VND_MailListResourceRate_ID
		return
	end
end

update e
	set
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_Estimate e join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
where ad.MailListResource_ID = @VND_MailListResourceRate_ID

update ad
	set
		MailListResource_ID = @new_MailListResourceRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_AssemDistribOptions ad
where ad.MailListResource_ID = @VND_MailListResourceRate_ID

if exists(select 1 from EST_AssemDistribOptions where MailListResource_ID = @VND_MailListResourceRate_ID) begin
	raiserror('Cannot delete Mail List Resource Rate.  It is being referenced by an estimate.', 16, 1)
	return
end
else begin
	delete from VND_MailListResourceRate
	where VND_MailListResourceRate_ID = @VND_MailListResourceRate_ID
	return
end
GO

GRANT  EXECUTE  ON [dbo].[VndMailListResourceRate_d_ByMailListResourceRateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.VndMailListResourceRate_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailListResourceRate_i'
	DROP PROCEDURE dbo.VndMailListResourceRate_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailListResourceRate_i') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailListResourceRate_i FAILED.'
END
GO
PRINT 'Creating dbo.VndMailListResourceRate_i'
GO

CREATE PROC dbo.VndMailListResourceRate_i
@VND_MailListResourceRate_ID bigint output,
@VND_Vendor_ID bigint,
@InternalListRate money,
@EffectiveDate datetime,
@CreatedBy varchar(50)
/*
* PARAMETERS:
* VND_MailListResourceRate_ID
* VND_Vendor_ID
* EffectiveDate
* CreatedBy
*
* DESCRIPTION:
* Inserts a Mail List Resource record.  Updates any A&D records that may be affected. Returns the Mail List ID.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   VND_MailListResourceRate       INSERT
*   EST_Estimate                   UPDATE
*   EST_AssemDistribOptions        UPDATE
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
* 10/08/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
as

declare @old_MailListResourceRate_ID bigint
select top 1 @old_MailListResourceRate_ID = VND_MailListResourceRate_ID
from VND_MailListResourceRate
where VND_Vendor_ID = @VND_Vendor_ID and EffectiveDate < @EffectiveDate
order by EffectiveDate

insert into VND_MailListResourceRate(VND_Vendor_ID, InternalListRate, EffectiveDate, CreatedBy, CreatedDate)
values(@VND_Vendor_ID, @InternalListRate, @EffectiveDate, @CreatedBy, getdate())
set @VND_MailListResourceRate_ID = @@identity
if (@@error <> 0) begin
	raiserror('Cannot create VND_MailListResourceRate record.', 16, 1)
	return
end

update e
	set e.ModifiedBy = @CreatedBy,
		e.ModifiedDate = getdate()
from EST_Estimate e join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
where e.RunDate >= @EffectiveDate and ad.MailListResource_ID = @old_MailListResourceRate_ID
if (@@error <> 0) begin
	raiserror('Cannot update EST_Estimate record.', 16, 1)
	return
end

update ad
	set
		ad.MailListResource_ID = @VND_MailListResourceRate_ID,
		ad.ModifiedBy = @CreatedBy,
		ad.ModifiedDate = getdate()
from EST_Estimate e join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
where e.RunDate >= @EffectiveDate and ad.MailListResource_ID = @old_MailListResourceRate_ID
if (@@error <> 0) begin
	raiserror('Cannot update EST_AssemDistribOptions record.', 16, 1)
	return
end
GO

GRANT  EXECUTE  ON [dbo].[VndMailListResourceRate_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.VndMailListResourceRate_s_ByDescriptionandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailListResourceRate_s_ByDescriptionandRunDate'
	DROP PROCEDURE dbo.VndMailListResourceRate_s_ByDescriptionandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailListResourceRate_s_ByDescriptionandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailListResourceRate_s_ByDescriptionandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.VndMailListResourceRate_s_ByDescriptionandRunDate'
GO

create proc dbo.VndMailListResourceRate_s_ByDescriptionandRunDate
/*
* PARAMETERS:
* Description - Required
* RunDate - Required
*
*
* DESCRIPTION:
*		Returns a MailListResourceRate Record for the given Vendor Description and Run Date.
*
*
* TABLES:
*		Table Name			       Access
*		==========			       ======
*		VND_Vendor                 READ
*		VND_MailListResourceRate   READ
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
*	Date            Who             Comments
*	-------------   --------        -------------------------------------------------
*   09/05/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Description varchar(35),
@RunDate datetime
as

select ml.*
from VND_Vendor v join VND_MailListResourceRate ml on v.VND_Vendor_ID = ml.VND_Vendor_ID
	left join VND_MailListResourceRate newer_ml on v.VND_Vendor_ID = newer_ml.VND_Vendor_ID and newer_ml.EffectiveDate <= @RunDate and newer_ml.EffectiveDate > ml.EffectiveDate
where v.Description = @Description and ml.EffectiveDate <= @RunDate and newer_ml.VND_MailListResourceRate_ID is null
GO

GRANT  EXECUTE  ON [dbo].[VndMailListResourceRate_s_ByDescriptionandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.VndMailListResourceRate_s_ByOldIDandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailListResourceRate_s_ByOldIDandRunDate'
	DROP PROCEDURE dbo.VndMailListResourceRate_s_ByOldIDandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailListResourceRate_s_ByOldIDandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailListResourceRate_s_ByOldIDandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.VndMailListResourceRate_s_ByOldIDandRunDate'
GO

CREATE proc dbo.VndMailListResourceRate_s_ByOldIDandRunDate
/*
* PARAMETERS:
* VND_MailListResourceRate_ID - required
* RunDate              - required
*
* DESCRIPTION:
*		Determines the parent vendor record of VND_MailListResourceRate_ID.  Returns a MailListResource record with the same parent vendor for the specified run date
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   VND_MailListResourceRate   READ
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
* 11/02/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_MailListResourceRate_ID bigint,
@RunDate datetime
as

declare @VND_Vendor_ID bigint

select @VND_Vendor_ID = VND_Vendor_ID
from VND_MailListResourceRate
where VND_MailListResourceRate_ID = @VND_MailListResourceRate_ID

select top 1 ml.*
from VND_MailListResourceRate ml
where ml.VND_Vendor_ID = @VND_Vendor_ID and ml.EffectiveDate <= @RunDate
order by ml.EffectiveDate desc
GO

GRANT  EXECUTE  ON [dbo].[VndMailListResourceRate_s_ByOldIDandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.VndMailTrackingRate_d_ByMailTrackingRateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailTrackingRate_d_ByMailTrackingRateID'
	DROP PROCEDURE dbo.VndMailTrackingRate_d_ByMailTrackingRateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailTrackingRate_d_ByMailTrackingRateID') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailTrackingRate_d_ByMailTrackingRateID FAILED.'
END
GO
PRINT 'Creating dbo.VndMailTrackingRate_d_ByMailTrackingRateID'
GO

CREATE PROC dbo.VndMailTrackingRate_d_ByMailTrackingRateID
@VND_MailTrackingRate_ID bigint,
@ModifiedBy varchar(50)
/*
* PARAMETERS:
* VND_MailTrackingRate_ID
*
* DESCRIPTION:
* Deletes a MailTracking record.  Updates any A&D records that may be affected.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   VND_MailTrackingRate           DELETE
*   EST_AssemDistribOptions        UPDATE
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
* 10/08/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
as

declare @new_MailTrackingRate_ID bigint
select top 1 @new_MailTrackingRate_ID = new_mh.VND_MailTrackingRate_ID
from VND_MailTrackingRate orig_mh join VND_MailTrackingRate new_mh on orig_mh.VND_Vendor_ID = new_mh.VND_Vendor_ID
where orig_mh.VND_MailTrackingRate_ID = @VND_MailTrackingRate_ID and orig_mh.VND_MailTrackingRate_ID <> new_mh.VND_MailTrackingRate_ID
order by new_mh.EffectiveDate

--If there is not an earlier set of Mail House Rates for this Vendor, delete the record
if (@new_MailTrackingRate_ID is null) begin
	if exists(select 1 from EST_AssemDistribOptions where MailTracking_ID = @VND_MailTrackingRate_ID) begin
		raiserror('Cannot delete Mail Tracking Rate.  It is being referenced by an estimate.', 16, 1)
		return
	end
	else begin
		delete from VND_MailTrackingRate
		where VND_MailTrackingRate_ID = @VND_MailTrackingRate_ID
		return
	end
end

update e
	set
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_Estimate e join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
where ad.MailTracking_ID = @VND_MailTrackingRate_ID

update ad
	set
		MailTracking_ID = @new_MailTrackingRate_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
from EST_AssemDistribOptions ad
where ad.MailTracking_ID = @VND_MailTrackingRate_ID

if exists(select 1 from EST_AssemDistribOptions where MailTracking_ID = @VND_MailTrackingRate_ID) begin
	raiserror('Cannot delete Mail Tracking Rate.  It is being referenced by an estimate.', 16, 1)
	return
end
else begin
	delete from VND_MailTrackingRate
	where VND_MailTrackingRate_ID = @VND_MailTrackingRate_ID
	return
end
GO

GRANT  EXECUTE  ON [dbo].[VndMailTrackingRate_d_ByMailTrackingRateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.VndMailTrackingRate_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailTrackingRate_i'
	DROP PROCEDURE dbo.VndMailTrackingRate_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailTrackingRate_i') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailTrackingRate_i FAILED.'
END
GO
PRINT 'Creating dbo.VndMailTrackingRate_i'
GO

CREATE PROC dbo.VndMailTrackingRate_i
@VND_MailTrackingRate_ID bigint output,
@VND_Vendor_ID bigint,
@MailTracking money,
@EffectiveDate datetime,
@CreatedBy varchar(50)
/*
* PARAMETERS:
* VND_MailTrackingRate_ID
* VND_Vendor_ID
* MailTracking
* EffectiveDate
* CreatedBy
*
* DESCRIPTION:
* Inserts a Mail Tracking record.  Updates any A&D records that may be affected. Returns the Mail Tracking ID.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   VND_MailTrackingRate           INSERT
*   EST_Estimate                   UPDATE
*   EST_AssemDistribOptions        UPDATE
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
* 10/08/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
as

declare @old_MailTrackingRate_ID bigint
select top 1 @old_MailTrackingRate_ID = VND_MailTrackingRate_ID
from VND_MailTrackingRate
where VND_Vendor_ID = @VND_Vendor_ID and EffectiveDate < @EffectiveDate
order by EffectiveDate

insert into VND_MailTrackingRate(VND_Vendor_ID, MailTracking, EffectiveDate, CreatedBy, CreatedDate)
values(@VND_Vendor_ID, @MailTracking, @EffectiveDate, @CreatedBy, getdate())
set @VND_MailTrackingRate_ID = @@identity
if (@@error <> 0) begin
	raiserror('Cannot create VND_MailTrackingRate record.', 16, 1)
	return
end

update e
	set e.ModifiedBy = @CreatedBy,
		e.ModifiedDate = getdate()
from EST_Estimate e join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
where e.RunDate >= @EffectiveDate and ad.MailTracking_ID = @old_MailTrackingRate_ID
if (@@error <> 0) begin
	raiserror('Cannot update EST_Estimate record.', 16, 1)
	return
end

update ad
	set
		ad.MailHouse_ID = @VND_MailTrackingRate_ID,
		ad.ModifiedBy = @CreatedBy,
		ad.ModifiedDate = getdate()
from EST_Estimate e join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
where e.RunDate >= @EffectiveDate and ad.MailTracking_ID = @old_MailTrackingRate_ID
if (@@error <> 0) begin
	raiserror('Cannot update EST_AssemDistribOptions record.', 16, 1)
	return
end
GO

GRANT  EXECUTE  ON [dbo].[VndMailTrackingRate_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.VndMailTrackingRate_s_ByDescriptionandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailTrackingRate_s_ByDescriptionandRunDate'
	DROP PROCEDURE dbo.VndMailTrackingRate_s_ByDescriptionandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailTrackingRate_s_ByDescriptionandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailTrackingRate_s_ByDescriptionandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.VndMailTrackingRate_s_ByDescriptionandRunDate'
GO

create proc dbo.VndMailTrackingRate_s_ByDescriptionandRunDate
/*
* PARAMETERS:
* Description - Required
* RunDate - Required
*
*
* DESCRIPTION:
*		Returns a MailTrackingRate Record for the given Vendor Description and Run Date.
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		VND_Vendor          READ
*		VND_MailTrackingRate   READ
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
*	Date            Who             Comments
*	-------------   --------        -------------------------------------------------
*   09/05/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Description varchar(35),
@RunDate datetime
as

select mt.*
from VND_Vendor v join VND_MailTrackingRate mt on v.VND_Vendor_ID = mt.VND_Vendor_ID
	left join VND_MailTrackingRate newer_mt on v.VND_Vendor_ID = newer_mt.VND_Vendor_ID and newer_mt.EffectiveDate <= @RunDate and newer_mt.EffectiveDate > mt.EffectiveDate
where v.Description = @Description and mt.EffectiveDate <= @RunDate and newer_mt.VND_MailTrackingRate_ID is null
GO

GRANT  EXECUTE  ON [dbo].[VndMailTrackingRate_s_ByDescriptionandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.VndMailTrackingRate_s_ByOldIDandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailTrackingRate_s_ByOldIDandRunDate'
	DROP PROCEDURE dbo.VndMailTrackingRate_s_ByOldIDandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailTrackingRate_s_ByOldIDandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailTrackingRate_s_ByOldIDandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.VndMailTrackingRate_s_ByOldIDandRunDate'
GO

CREATE proc dbo.VndMailTrackingRate_s_ByOldIDandRunDate
/*
* PARAMETERS:
* VND_MailTrackingRate_ID - required
* RunDate              - required
*
* DESCRIPTION:
*		Determines the parent vendor record of VND_MailTrackingRate_ID.  Returns a MailTracking record with the same parent vendor for the specified run date
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   VND_MailTrackingRate   READ
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
* 11/02/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_MailTrackingRate_ID bigint,
@RunDate datetime
as

declare @VND_Vendor_ID bigint

select @VND_Vendor_ID = VND_Vendor_ID
from VND_MailTrackingRate
where VND_MailTrackingRate_ID = @VND_MailTrackingRate_ID

select top 1 mt.*
from VND_MailTrackingRate mt
where mt.VND_Vendor_ID = @VND_Vendor_ID and mt.EffectiveDate <= @RunDate
order by mt.EffectiveDate desc
GO

GRANT  EXECUTE  ON [dbo].[VndMailTrackingRate_s_ByOldIDandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.VndPaperGrade_s_Grade_ByPaperIDandPaperWeightID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndPaperGrade_s_Grade_ByPaperIDandPaperWeightID'
	DROP PROCEDURE dbo.VndPaperGrade_s_Grade_ByPaperIDandPaperWeightID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndPaperGrade_s_Grade_ByPaperIDandPaperWeightID') IS NOT NULL
		PRINT '***********Drop of dbo.VndPaperGrade_s_Grade_ByPaperIDandPaperWeightID FAILED.'
END
GO
PRINT 'Creating dbo.VndPaperGrade_s_Grade_ByPaperIDandPaperWeightID'
GO

create proc dbo.VndPaperGrade_s_Grade_ByPaperIDandPaperWeightID
/*
* PARAMETERS:
* VND_Paper_ID - Required.  The Paper ID that is associated with the PPR_PaperWeight_ID.
* PPR_PaperWeight_ID - Required.  The component must have a Paper Weight in order to determine which Paper Grades are available.
* PPR_PaperGrade_ID - Optional.  If the component already references a PPR_PaperGrade record we need to make sure we return it.
*
*
* DESCRIPTION:
*		Returns a list of Paper Grades for the specified Paper Weight and VND_Paper_ID.
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		PPR_Paper_Map
*		PPR_PaperGrade
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
*	Date						Who							Comments
*	------------- 	--------        -------------------------------------------------
* 05/23/2007			BJS							Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Paper_ID bigint,
@PPR_PaperWeight_ID int,
@PPR_PaperGrade_ID int
as

select pg.PPR_PaperGrade_ID, pg.Grade
from PPR_Paper_Map pm	join PPR_PaperGrade pg on pm.PPR_PaperGrade_ID = pg.PPR_PaperGrade_ID
where pm.VND_Paper_ID = @VND_Paper_ID and pm.PPR_PaperWeight_ID = @PPR_PaperWeight_ID
union
select PPR_PaperGrade_ID, Grade
from PPR_PaperGrade
where PPR_PaperGrade_ID = @PPR_PaperGrade_ID


GO

GRANT  EXECUTE  ON [dbo].[VndPaperGrade_s_Grade_ByPaperIDandPaperWeightID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.VndPaper_s_ByDescriptionandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndPaper_s_ByDescriptionandRunDate'
	DROP PROCEDURE dbo.VndPaper_s_ByDescriptionandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndPaper_s_ByDescriptionandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.VndPaper_s_ByDescriptionandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.VndPaper_s_ByDescriptionandRunDate'
GO

create proc dbo.VndPaper_s_ByDescriptionandRunDate
/*
* PARAMETERS:
* Description - Required
* RunDate - Required
*
*
* DESCRIPTION:
*		Returns a Paper Record for the given Vendor Description and Run Date.
*
*
* TABLES:
*  Table Name          Access
*  ==========          ======
*  VND_Vendor          READ
*  VND_Printer         READ
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
*	Date            Who             Comments
*	-------------   --------        -------------------------------------------------
*   09/25/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Description varchar(35),
@RunDate datetime
as

select p.*
from VND_Vendor v join VND_Paper p on v.VND_Vendor_ID = p.VND_Vendor_ID
	left join VND_Paper newer_p on v.VND_Vendor_ID = newer_p.VND_Vendor_ID and newer_p.EffectiveDate <= @RunDate and newer_p.EffectiveDate > p.EffectiveDate
where v.Description = @Description and p.EffectiveDate <= @RunDate and newer_p.VND_Paper_ID is null
GO

GRANT  EXECUTE  ON [dbo].[VndPaper_s_ByDescriptionandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.VndPaper_s_PaperID_ByVendorIDandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndPaper_s_PaperID_ByVendorIDandRunDate'
	DROP PROCEDURE dbo.VndPaper_s_PaperID_ByVendorIDandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndPaper_s_PaperID_ByVendorIDandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.VndPaper_s_PaperID_ByVendorIDandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.VndPaper_s_PaperID_ByVendorIDandRunDate'
GO

create proc dbo.VndPaper_s_PaperID_ByVendorIDandRunDate
/*
* PARAMETERS:
* VND_Vendor_ID - Required.  The Vendor.
* RunDate - Required.  The Estimate Run Date.
* VND_Paper_ID - Output.
*
*
* DESCRIPTION:
*		Returns the PaperID that is active on the RunDate for the VND_Vendor_ID.
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		VND_Paper
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
*	Date						Who							Comments
*	------------- 	--------        -------------------------------------------------
* 05/23/2007			BJS							Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Vendor_ID bigint,
@RunDate datetime,
@VND_Paper_ID bigint output
as

select top 1 @VND_Paper_ID = VND_Paper_ID
from VND_Paper
where VND_Vendor_ID = @VND_Vendor_ID and EffectiveDate <= @RunDate
order by EffectiveDate desc


GO

GRANT  EXECUTE  ON [dbo].[VndPaper_s_PaperID_ByVendorIDandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.VndPrinter_s_ByDescriptionandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndPrinter_s_ByDescriptionandRunDate'
	DROP PROCEDURE dbo.VndPrinter_s_ByDescriptionandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndPrinter_s_ByDescriptionandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.VndPrinter_s_ByDescriptionandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.VndPrinter_s_ByDescriptionandRunDate'
GO

create proc dbo.VndPrinter_s_ByDescriptionandRunDate
/*
* PARAMETERS:
* Description - Required
* RunDate - Required
*
*
* DESCRIPTION:
*		Returns a Printer Record for the given Vendor Description and Run Date.
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		VND_Vendor      READ
*		VND_Printer     READ
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
*	Date            Who             Comments
*	-------------   --------        -------------------------------------------------
*   09/04/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Description varchar(35),
@RunDate datetime
as

select p.*
from VND_Vendor v join VND_Printer p on v.VND_Vendor_ID = p.VND_Vendor_ID
	left join VND_Printer newer_p on v.VND_Vendor_ID = newer_p.VND_Vendor_ID and newer_p.EffectiveDate <= @RunDate and newer_p.EffectiveDate > p.EffectiveDate
where v.Description = @Description and p.EffectiveDate <= @RunDate and newer_p.VND_Printer_ID is null
GO

GRANT  EXECUTE  ON [dbo].[VndPrinter_s_ByDescriptionandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.VndVendor_s_ByDescriptionandVendorType') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndVendor_s_ByDescriptionandVendorType'
	DROP PROCEDURE dbo.VndVendor_s_ByDescriptionandVendorType
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndVendor_s_ByDescriptionandVendorType') IS NOT NULL
		PRINT '***********Drop of dbo.VndVendor_s_ByDescriptionandVendorType FAILED.'
END
GO
PRINT 'Creating dbo.VndVendor_s_ByDescriptionandVendorType'
GO

create proc dbo.VndVendor_s_ByDescriptionandVendorType
/*
* PARAMETERS:
* Description       - Required
* VND_VendorType_ID - Required
*
*
* DESCRIPTION:
*		Returns a Vendor Record matching the Description and Vendor Type
*
*
* TABLES:
*   Table Name      Access
*   ==========      ======
*   VND_Vendor      READ
*   VND_Printer     READ
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
*	Date            Who             Comments
*	-------------   --------        -------------------------------------------------
*   09/04/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Description varchar(35),
@VND_VendorType_ID int
as
select v.*
from VND_Vendor v join VND_VendorVendorType_Map vvtm on v.VND_Vendor_ID = vvtm.VND_Vendor_ID
where v.Description = @Description and vvtm.VND_VendorType_ID = @VND_VendorType_ID
GO

GRANT  EXECUTE  ON [dbo].[VndVendor_s_ByDescriptionandVendorType]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubInsertScenario_s_ActiveByRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubInsertScenario_s_ActiveByRunDate'
	DROP PROCEDURE dbo.PubInsertScenario_s_ActiveByRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubInsertScenario_s_ActiveByRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PubInsertScenario_s_ActiveByRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PubInsertScenario_s_ActiveByRunDate'
GO

CREATE PROC dbo.PubInsertScenario_s_ActiveByRunDate
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns a list of scenarios containing active pub groups 
*			for the specified RunDate.
*
* TABLES:
*		Table Name					Access
*		==========	                ======
*		pub_insertscenario          READ
*
* PROCEDURES CALLED:
*		Procedure Name
*		==============
*		dbo.PubPubGroup_s_ActiveIDs_ByRunDate
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
*	Date            Who             Comments
*	------------- 	--------        -------------------------------------------------
*	08/30/2007      JRH             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@RunDate datetime

AS

CREATE TABLE #groups
	(
	[pub_pubgroup_id]	bigint not null
	, [description]		varchar(35)
	, [sortorder]		int
	)

INSERT INTO #groups
	EXEC dbo.PubPubGroup_s_ActiveIDs_ByRunDate @RunDate

SELECT DISTINCT
	scen.pub_insertscenario_id
	, scen.[description]
FROM
	dbo.pub_insertscenario scen (nolock)
	INNER JOIN dbo.pub_groupinsertscenario_map m (nolock)
		ON scen.pub_insertscenario_id = m.pub_insertscenario_id
	INNER JOIN #groups
		ON m.pubgroupdescription = #groups.[description]
WHERE
	scen.active = 1


DROP TABLE #groups
GO

GRANT  EXECUTE  ON [dbo].[PubInsertScenario_s_ActiveByRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
