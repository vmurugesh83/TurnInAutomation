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
