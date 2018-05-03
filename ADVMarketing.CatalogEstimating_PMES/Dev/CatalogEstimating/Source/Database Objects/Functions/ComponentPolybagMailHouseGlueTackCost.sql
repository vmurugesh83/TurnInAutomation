IF OBJECT_ID('dbo.ComponentPolybagMailHouseGlueTackCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentPolybagMailHouseGlueTackCost'
	DROP FUNCTION dbo.ComponentPolybagMailHouseGlueTackCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentPolybagMailHouseGlueTackCost') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentPolybagMailHouseGlueTackCost FAILED.'
END
GO
PRINT 'Creating dbo.ComponentPolybagMailHouseGlueTackCost'
GO

create function dbo.ComponentPolybagMailHouseGlueTackCost(@EST_Component_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID - A Host Component ID
*
* DESCRIPTION:
*	Calculates the Component's portion of the polybag mailhouse glue tack cost.
* Totals each of the costs and returns it as the Total Component Polybag glue tack cost.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PackageComponentMapping
*
* PROCEDURES CALLED:
*	dbo.PolybagMailHouseGlueTackCost
*   dbo.PackageWeight
*   dbo.PolyBagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Component Polybag mailhouse gluetack cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
* 08/16/2007      TJU             Corrected call to table join EST_Polybag pb (not EST_Polybag_ID)
* 10/15/2007      BJS             Removed multiply by quantity, it is handled in the PolybagMailHouseGlueTackCost function
*/
begin
	return(select sum(dbo.PolybagMailHouseGlueTackCost(pb.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolyBagWeight(pb.EST_Polybag_ID)))
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
		join EST_PackagePolybag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
		join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
	where pcm.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentPolybagMailHouseGlueTackCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
