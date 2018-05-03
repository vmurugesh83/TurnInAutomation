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
* 05/21/2008      BJS             Removed call to IsPubRateMapActive, replaced with join and where logic
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
