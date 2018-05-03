IF OBJECT_ID('dbo.ComponentPolybagMailTrackingCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentPolybagMailTrackingCost'
	DROP FUNCTION dbo.ComponentPolybagMailTrackingCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentPolybagMailTrackingCost') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentPolybagMailTrackingCost FAILED.'
END
GO
PRINT 'Creating dbo.ComponentPolybagMailTrackingCost'
GO

CREATE function dbo.ComponentPolybagMailTrackingCost(@EST_Component_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID - A Host Component ID
*
* DESCRIPTION:
*	Calculates the Component's portion of the polybag mail tracking cost.
*   Totals each of the costs and returns it as the Total Component Polybag mail tracking cost.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PackageComponentMapping
*
* PROCEDURES CALLED:
*	dbo.PolybagMailTrackingCost
*   dbo.PackageWeight
*   dbo.PolybagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Component Polybag mail tracking cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation
* 10/15/2007      BJS             Removed multiply by quantity, it is already handled in the PolybagMailTrackingCost function
*
*/
as
begin
	return(select sum(dbo.PolybagMailTrackingCost(pb.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolybagWeight(pb.EST_Polybag_ID)))
		from EST_Component c join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
			join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
			join EST_PackagePolybag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
			join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
		where c.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentPolybagMailTrackingCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
