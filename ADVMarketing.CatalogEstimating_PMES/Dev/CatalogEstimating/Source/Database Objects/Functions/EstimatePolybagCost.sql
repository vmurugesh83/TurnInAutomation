IF OBJECT_ID('dbo.EstimatePolybagCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimatePolybagCost'
	DROP FUNCTION dbo.EstimatePolybagCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimatePolybagCost') IS NOT NULL
		PRINT '***********Drop of dbo.EstimatePolybagCost FAILED.'
END
GO
PRINT 'Creating dbo.EstimatePolybagCost'
GO

CREATE function dbo.EstimatePolybagCost(@EST_Estimate_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Estimate_ID - An Estimate_ID
*
* DESCRIPTION:
*	Calculates an estimate's portion of polybag costs.
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
* 07/26/2007      BJS             Renamed ComponentPolybagCost -> EstimatePolybagCost
*
*/
begin
	return(select sum(dbo.PolybagCost(ppm.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolyBagWeight(ppm.EST_Polybag_ID)))
		from EST_Package p join EST_PackagePolybag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
		where p.EST_Estimate_ID = @EST_Estimate_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[EstimatePolybagCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
