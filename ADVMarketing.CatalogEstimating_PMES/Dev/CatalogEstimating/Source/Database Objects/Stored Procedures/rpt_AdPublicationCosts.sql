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
* 05/19/2008      JRH             Add additional group by PubRateMapInsertQuantity in final select.
* 06/18/2008      BJS             All pub rates are prorated by weight
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
			* tc.PieceWeight / tc.PackageWeight
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

select RunDate, IssueDate, AdNumber, InsertTime, Pub_ID, Pub_NM, PubLoc_ID
	, sum(PieceCost) as PieceCost
	, PubRateMapInsertQuantity
	, sum(CostWithoutInsert) as CostWithoutInsert
	, sum(PubRateMapInsertCost) as PubRateMapInsertCost
	, sum(TotalCost) as TotalCost
from
	(
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
	) b
group by RunDate, AdNumber, IssueDate, InsertTime, Pub_ID, Pub_NM, PubLoc_ID, PubRateMapInsertQuantity
order by RunDate, AdNumber, IssueDate, InsertTime, Pub_ID, PubLoc_ID

set nocount on

drop table #tempComponentPubRateMap
GO

GRANT  EXECUTE  ON [dbo].[rpt_AdPublicationCosts]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
