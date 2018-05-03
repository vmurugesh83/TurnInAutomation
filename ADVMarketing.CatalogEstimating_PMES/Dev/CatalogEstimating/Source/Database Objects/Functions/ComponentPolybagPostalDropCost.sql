IF OBJECT_ID('dbo.ComponentPolybagPostalDropCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentPolybagPostalDropCost'
	DROP FUNCTION dbo.ComponentPolybagPostalDropCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentPolybagPostalDropCost') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentPolybagPostalDropCost FAILED.'
END
GO
PRINT 'Creating dbo.ComponentPolybagPostalDropCost'
GO

CREATE function dbo.ComponentPolybagPostalDropCost(@EST_Component_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID - A Host Component ID
*
* DESCRIPTION:
*	Calculates the Component's portion of the polybag postal drop cost.
*   Totals each of the costs and returns it as the Total Component Polybag postal drop cost.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PackageComponentMapping
*
* PROCEDURES CALLED:
*	  dbo.PolybagPostalDropCost
*   dbo.PackageWeight
*   dbo.PolyBagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Component Polybag postal drop cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/18/2007      BJS             Initial Creation 
*
*/
as
begin
	return(select sum(dbo.PolybagPostalDropCost(pb.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolyBagWeight(pb.EST_Polybag_ID)))
		from EST_Component c join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
			join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
			join EST_PackagePolyBag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
			join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
		where c.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentPolybagPostalDropCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
