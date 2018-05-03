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
* 05/21/2008      BJS             Replaced call to dbo.IsPubRateMapActive with join logic
* 06/13/2008      BJS             Insert Costs are now prorated to components by weight per AGordon 6/13/08
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
	JOIN PUB_PubRate_Map_Activate prma on ppgm.PUB_PubRate_Map_ID = prma.PUB_PubRate_Map_ID AND prma.Active = 1
		and prma.EffectiveDate <= pid.IssueDate
WHERE NOT EXISTS (SELECT 1 FROM PUB_PubRate_Map_Activate prma_newer
		WHERE prma.PUB_PubRate_Map_ID = prma_newer.PUB_PubRate_Map_ID and prma_newer.Active = 1
			and prma_newer.EffectiveDate <= pid.IssueDate and prma_newer.EffectiveDate > prma.EffectiveDate)
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
