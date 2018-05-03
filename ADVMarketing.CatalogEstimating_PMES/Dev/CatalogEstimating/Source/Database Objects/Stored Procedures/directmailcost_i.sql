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
* 05/21/2008      BJS             EstimateIDS changed from varchar to text
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
