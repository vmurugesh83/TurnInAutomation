IF OBJECT_ID('dbo.PackagePolyPostage') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PackagePolyPostage'
	DROP FUNCTION dbo.PackagePolyPostage
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PackagePolyPostage') IS NOT NULL
		PRINT '***********Drop of dbo.PackagePolyPostage FAILED.'
END
GO
PRINT 'Creating dbo.PackagePolyPostage'
GO

create function dbo.PackagePolyPostage(@EST_Package_ID bigint, @PackageWeight decimal(12,6), @EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Package_ID
*	PackageWeight
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the total polybag postage for a package.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PolyBag
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Polybag postage for Package
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/18/2007      BJS             Initial Creation 
* 10/03/2007      BJS             Modified select to select sum(...) so that one value is returned
* 10/26/2007      BJS             Added 6 points of precision to the Distribution Percentage
* 11/25/2007      JRH             Modified to return the postage per polybag.
*
*/
as
begin
return (
	select dbo.CalcItemPostage(pb.Quantity, dbo.PolyBagWeight(pb.EST_Polybag_ID) + pr.PolybagBagWeight, pb.PST_PostalScenario_ID)
		* isnull(cast(ppm.DistributionPct as decimal(12,6)), @PackageWeight / dbo.PolyBagWeight(pb.EST_Polybag_ID))
	from EST_PackagePolybag_Map ppm join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
		join EST_PolybagGroup pbg on pb.EST_PolybagGroup_ID = pbg.EST_PolybagGroup_ID
		join VND_Printer pr on pbg.VND_Printer_ID = pr.VND_Printer_ID
	where ppm.EST_Package_ID = @EST_Package_ID
		and pb.EST_Polybag_ID = @EST_Polybag_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[PackagePolyPostage]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
