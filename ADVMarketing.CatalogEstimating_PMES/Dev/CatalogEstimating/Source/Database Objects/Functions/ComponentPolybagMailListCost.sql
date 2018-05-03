IF OBJECT_ID('dbo.ComponentPolybagMailListCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentPolybagMailListCost'
	DROP FUNCTION dbo.ComponentPolybagMailListCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentPolybagMailListCost') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentPolybagMailListCost FAILED.'
END
GO
PRINT 'Creating dbo.ComponentPolybagMailListCost'
GO

CREATE function dbo.ComponentPolybagMailListCost(@EST_Component_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID
*
* DESCRIPTION:
*	Calculates the Component's portion of the polybag mail list cost.
* Totals each of the costs and returns it as the Total Component Polybag Mail List cost.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   EST_Component                READ
*   EST_PackageComponentMapping  READ
*   EST_Package                  READ
*   EST_PackagePolybag_Map       READ
*   EST_Polybag                  READ
*
* PROCEDURES CALLED:
*   dbo.PolybagMailListCost
*	dbo.PackageWeight
*   dbo.PolybagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Component Polybag Mail List cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/09/2007      BJS             Initial Creation 
* 10/16/2007      BJS             Added Reference to PolybagMailList cost, removed blended mail list rate parameter
*
*/
as
begin
	return(select sum(dbo.PolybagMailListCost(pb.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolybagWeight(ppm.EST_PolyBag_ID)))
		from EST_Component c join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
			join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
			join EST_PackagePolybag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
			join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
		where c.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentPolybagMailListCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
