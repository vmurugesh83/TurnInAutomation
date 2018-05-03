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
* 05/20/2008	JRH	    Added 'DISTINCT' when inserting into ctlg_pubvr_distbn
* 05/21/2008    BJS     Modified @EstimateIDs from varchar to text.
*                            Replaced reference to dbo.IsPubRateMapActive with join logic to improve performance.
* 05/22/2008    BJS     Added tempHostPackagePolybagMap to improve performance.
* 06/13/2008    BJS     Insert Costs are now prorated to components by weight per AGordon 6/13/08
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EstimateIDs text
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

/* tempComponentPolybagMap represents a join of all components in #tempComponent with any polybags they are included in. */
CREATE TABLE #tempHostPackagePolybagMap(
	EST_Component_ID BIGINT,
	EST_Package_ID BIGINT,
	EST_Polybag_ID BIGINT,
	Quantity INT,
	ProratePct DECIMAL(12,6),
	PolybagCost MONEY,
	InkjetCost MONEY,
	InkjetMakereadyCost MONEY,
	MailHouseAdminFee MONEY,
	MailHouseGluetackCost MONEY,
	MailHouseLetterInsertionCost MONEY,
	MailHouseOtherDirectMailHandlingCost MONEY,
	MailHouseTabbingCost MONEY,
	MailHouseTimeValueSlipsCost MONEY,
	MailListCost MONEY,
	MailTrackingCost MONEY,
	OtherFreightCost MONEY,
	PostalDropCost MONEY
)

INSERT INTO #tempHostPackagePolybagMap(EST_Component_ID, EST_Package_ID, EST_Polybag_ID, Quantity, ProratePct)
SELECT tc.EST_Component_ID, p.EST_Package_ID, pb.EST_Polybag_ID, pb.Quantity,
	ISNULL(ppbm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolybagWeight(pb.EST_Polybag_ID))
FROM #tempComponents tc JOIN EST_PackageComponentMapping pcm ON tc.EST_Component_ID = pcm.EST_Component_ID
	JOIN EST_Package p ON pcm.EST_Package_ID = p.EST_Package_ID
	JOIN EST_PackagePolybag_Map ppbm ON p.EST_Package_ID = ppbm.EST_Package_ID
	JOIN EST_Polybag pb ON ppbm.EST_Polybag_ID = pb.EST_Polybag_ID
WHERE tc.EST_ComponentType_ID = 1

UPDATE #tempHostPackagePolybagMap
	SET
		PolybagCost = dbo.PolybagCost(EST_Polybag_ID) * ProratePct,
		InkjetCost = dbo.PolybagInkjetCost(EST_Polybag_ID) * ProratePct,
		InkjetMakereadyCost = dbo.PolybagInkjetMakereadyCost(EST_Polybag_ID) * ProratePct,
		MailHouseTimeValueSlipsCost = dbo.PolybagMailHouseTimeValueSlipsCost(EST_Polybag_ID) * ProratePct,
		MailListCost = dbo.PolybagMailListCost(EST_Polybag_ID) * ProratePct,
		MailHouseAdminFee = dbo.PolybagMailHouseAdminFee(EST_Polybag_ID) * ProratePct,
		MailHouseGlueTackCost = dbo.PolybagMailHouseGlueTackCost(EST_Polybag_ID) * ProratePct,
		MailHouseTabbingCost = dbo.PolybagMailHouseTabbingCost(EST_Polybag_ID) * ProratePct,
		MailHouseLetterInsertionCost = dbo.PolybagMailHouseLetterInsertionCost(EST_Polybag_ID) * ProratePct,
		MailHouseOtherDirectMailHandlingCost = dbo.PolybagMailHouseOtherDirectMailHandlingCost(EST_Polybag_ID) * ProratePct

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

update tc
	set MailListCost = BlendedMailListCPM * SoloQuantity / 1000 + ISNULL((SELECT SUM(MailListCost) FROM #tempHostPackagePolybagMap pb WHERE tc.EST_Component_ID = pb.EST_Component_ID), 0)
FROM #tempComponents tc
where tc.EST_ComponentType_ID = 1

update #tempComponents
	set ProductionTotal = isnull(CreativeCost, 0) + isnull(SeparatorCost, 0) + TotalPrintCost + TotalPaperCost + isnull(MailListCost, 0) +
		+ isnull(VendorProductionCost, 0) + isnull(OtherProductionCost, 0)

/* Assembly Calculations */

update #tempComponents set PolyBagCost = dbo.EstimatePolyBagCost(EST_Estimate_ID) where EST_ComponentType_ID = 1

update #tempComponents set OnsertCost = PolybagQuantity / cast(1000 as decimal) * isnull(OnsertRate, 0) where EST_ComponentType_ID = 2

update #tempComponents set GrossInkjetMakereadyCost = isnull(NumberofPlants, 0) * isnull(InkjetMakereadyRate, 0)

update tc
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
				when 1 then (SoloQuantity / cast(1000 as decimal) * InkJetRate) + ISNULL((SELECT SUM(InkjetCost) FROM #tempHostPackagePolybagMap pb WHERE tc.EST_Component_ID = pb.EST_Component_ID), 0)
			end,
		InkJetMakeReadyCost =
			case EST_ComponentType_ID
				when 1 then
					case
						when isnull(SoloQuantity, 0) = 0 then 0
						else GrossInkjetMakereadyCost * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
					end
					+ ISNULL((SELECT SUM(InkjetMakereadyCost) FROM #tempHostPackagePolybagMap pb WHERE tc.EST_Component_ID = pb.EST_Component_ID), 0)
			end
FROM #tempComponents tc

update #tempComponents set TotalInkJetCost = isnull(InkJetCost, 0) + isnull(InkJetMakeReadyCost, 0)

update tc set CartonRate = pr.Rate
from #tempComponents tc join EST_Component c on tc.EST_Component_ID = c.EST_Component_ID
	join VND_Printer p on c.Printer_ID = p.VND_Printer_ID
	join PRT_PrinterRate pr on p.VND_Printer_ID = pr.VND_Printer_ID and pr.PRT_PrinterRateType_ID = 3 and pr.[Default] = 1

update tc 
	set
		CornerGuardCost = TotalEstimateWeight / cast(2000 as decimal) * CornerGuardRate,
		SkidCost = TotalEstimateWeight / cast(2000 as decimal) * SkidRate,
		CartonCost = NumberOfCartons * CartonRate,
		TimeValueSlipsCost = TimeValueSlipsCPM * SoloQuantity / cast(1000 as decimal) + ISNULL((SELECT SUM(MailHouseTimeValueSlipsCost) FROM #tempHostPackagePolybagMap pb WHERE tc.EST_Component_ID = pb.EST_Component_ID), 0),
		MailHouseAdminFee =
			case
				when SoloQuantity = 0 then 0
				else GrossMailHouseAdminFee * cast(SoloQuantity as decimal) / cast(dbo.EstimateSoloAndPrimaryPolybagQuantity(EST_Estimate_ID) as decimal)
			end
			+ ISNULL((SELECT SUM(MailHouseAdminFee) FROM #tempHostPackagePolybagMap pb WHERE tc.EST_Component_ID = pb.EST_Component_ID), 0),
		GlueTackCost = GlueTackCPM * SoloQuantity / cast(1000 as decimal)
			+ ISNULL((SELECT SUM(MailHouseGlueTackCost) FROM #tempHostPackagePolybagMap pb WHERE tc.EST_Component_ID = pb.EST_Component_ID), 0),
		TabbingCost = TabbingCPM * SoloQuantity / cast(1000 as decimal)
			+ ISNULL((SELECT SUM(MailHouseTabbingCost) FROM #tempHostPackagePolybagMap pb WHERE tc.EST_Component_ID = pb.EST_Component_ID), 0),
		LetterInsertionCost = LetterInsertionCPM * SoloQuantity / cast(1000 as decimal)
			+ ISNULL((SELECT SUM(MailHouseLetterInsertionCost) FROM #tempHostPackagePolybagMap pb WHERE tc.EST_Component_ID = pb.EST_Component_ID), 0),
		OtherMailHandlingCost = OtherMailHandlingCPM * SoloQuantity / cast(1000 as decimal)
			+ ISNULL((SELECT SUM(MailHouseOtherDirectMailHandlingCost) FROM #tempHostPackagePolybagMap pb WHERE tc.EST_Component_ID = pb.EST_Component_ID), 0)
FROM #tempComponents tc
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
	JOIN PUB_PubRate_Map_Activate prma on ppgm.PUB_PubRate_Map_ID = prma.PUB_PubRate_Map_ID AND prma.Active = 1
		and prma.EffectiveDate <= pid.IssueDate
WHERE NOT EXISTS (SELECT 1 FROM PUB_PubRate_Map_Activate prma_newer
		WHERE prma.PUB_PubRate_Map_ID = prma_newer.PUB_PubRate_Map_ID and prma_newer.Active = 1
			and prma_newer.EffectiveDate <= pid.IssueDate and prma_newer.EffectiveDate > prma.EffectiveDate)
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

--Insertion Costs
update tp
	set PackageComponentCost =
		case
			when PackageWeight = 0 then 0
			else
				(GrossPackageInsertCost *(1 - isnull(InsertDiscountPercent, 0))) * ComponentPieceWeight / PackageWeight
		end
from #tempPackagesByPubRate tp

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
SELECT DISTINCT
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
drop table #tempHostPackagePolybagMap
drop table #tempPackagesByPubRate
GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_Upload]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
