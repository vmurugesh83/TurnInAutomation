IF OBJECT_ID('dbo.EstimatePolybagInkjetCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimatePolybagInkjetCost'
	DROP FUNCTION dbo.EstimatePolybagInkjetCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimatePolybagInkjetCost') IS NOT NULL
		PRINT '***********Drop of dbo.EstimatePolybagInkjetCost FAILED.'
END
GO
PRINT 'Creating dbo.EstimatePolybagInkjetCost'
GO

CREATE function dbo.EstimatePolybagInkjetCost(@EST_Estimate_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Estimate_ID - An Estimate ID
*
* DESCRIPTION:
*	Calculates the Estimate's portion of the polybag inkjet cost.
*   Totals each of the costs and returns it as the Total Estimate Polybag inkjet cost.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PackageComponentMapping
*
* PROCEDURES CALLED:
*	  dbo.PolybagInkjetCost
*   dbo.PackageWeight
*   dbo.PolyBagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Estimate Polybag inkjet cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/18/2007      BJS             Initial Creation
* 07/26/2007      BJS             Renamed ComponentPolybagInkjetCost->EstimatePolybagInkjetCost
* 10/15/2007      BJS             Removed multiply by quantity, it is already handled in the PolybagInkjetCost function
*
*/
as
begin
	return(select sum(dbo.PolybagInkjetCost(pb.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolyBagWeight(pb.EST_Polybag_ID)))
		from EST_Package p join EST_PackagePolyBag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
			join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
		where p.EST_Estimate_ID = @EST_Estimate_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[EstimatePolybagInkjetCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
