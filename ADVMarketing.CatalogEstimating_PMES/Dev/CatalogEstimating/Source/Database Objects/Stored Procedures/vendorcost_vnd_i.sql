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
* 05/21/2008      BJS             Modified @EstimateIDs from varchar to text.
*                                      Replaced call to dbo.IsPubRateMapActive with join logic to improve performance.
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
	JOIN PUB_PubRate_Map_Activate prma on ppgm.PUB_PubRate_Map_ID = prma.PUB_PubRate_Map_ID AND prma.Active = 1
		and prma.EffectiveDate <= pid.IssueDate
WHERE NOT EXISTS (SELECT 1 FROM PUB_PubRate_Map_Activate prma_newer
		WHERE prma.PUB_PubRate_Map_ID = prma_newer.PUB_PubRate_Map_ID and prma_newer.Active = 1
			and prma_newer.EffectiveDate <= pid.IssueDate and prma_newer.EffectiveDate > prma.EffectiveDate)
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

