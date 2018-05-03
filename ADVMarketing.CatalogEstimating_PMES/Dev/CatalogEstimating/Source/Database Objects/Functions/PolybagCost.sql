IF OBJECT_ID('dbo.PolybagCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagCost'
	DROP FUNCTION dbo.PolybagCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagCost'
GO

CREATE function dbo.PolybagCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID - A Host Component ID
*
* DESCRIPTION:
*	Calculates the cost of a polybag.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   EST_Component                READ
*   EST_PackageComponentMapping  READ
*   EST_Package                  READ
*   EST_PackagePolybag_Map       READ
*   EST_Polybag                  READ
*   EST_PolybagGroup             READ
*   VND_Printer                  READ
*   PRT_PrinterRate              READ
*   EST_EstimatePolybagGroup_Map READ
*
* PROCEDURES CALLED:
*   None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Polybag Cost (Bag Cost and Message Cost).
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
*
*/
begin
	return(select top 1 (pb.Quantity + (isnull(c.SpoilagePct, 0) * pb.Quantity)) / 1000 * (br.Rate + case when pbg.UseMessage = 1 then pr.PolybagMessage else 0 end)
			+ c.NumberOfPlants * (mrr.Rate + case when pbg.UseMessage = 1 then pr.PolybagMessageMakeready else 0 end)
		from EST_Polybag pb join EST_PolybagGroup pbg on pb.EST_PolybagGroup_ID = pbg.EST_PolybagGroup_ID
			join VND_Printer pr on pbg.VND_Printer_ID = pr.VND_Printer_ID
			join PRT_PrinterRate br on pbg.PRT_BagRate_ID = br.PRT_PrinterRate_ID
			join PRT_PrinterRate mrr on pbg.PRT_BagMakereadyRate_ID = mrr.PRT_PrinterRate_ID
			join EST_EstimatePolybagGroup_Map pbgm on pb.EST_PolybagGroup_ID = pbgm.EST_PolybagGroup_ID
			join EST_Component c on pbgm.EST_Estimate_ID = c.EST_Estimate_ID
		where pb.EST_Polybag_ID = @EST_Polybag_ID and c.EST_ComponentType_ID = 1
		order by pbgm.EstimateOrder)
end	
GO

GRANT  EXECUTE  ON [dbo].[PolybagCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO