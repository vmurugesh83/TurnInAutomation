IF OBJECT_ID('dbo.ComponentPolybagQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentPolybagQuantity'
	DROP FUNCTION dbo.ComponentPolybagQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentPolybagQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentPolybagQuantity FAILED.'
END
GO
PRINT 'Creating dbo.ComponentPolybagQuantity'
GO

/* Component PolyBag Quantity */
create function dbo.ComponentPolybagQuantity(@EST_Component_ID bigint)
/*
* PARAMETERS:
*	EST_Component_ID - A Component ID
*
* DESCRIPTION:
*	Calculates the Component's polybag quantity.
*
* TABLES:
*   Table Name                    Access
*   ==========                    ======
*   EST_PackageComponentMapping   READ
*   EST_PackagePolybag_Map        READ
*   EST_Polybag                   READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Component Polybag Quantity.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/05/2007      BJS             Initial Creation 
*
*/
returns int
as
begin
	return(select sum(pb.Quantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
		join EST_PackagePolybag_Map ppbm on p.EST_Package_ID = ppbm.EST_Package_ID
		join EST_Polybag pb on ppbm.EST_PolyBag_ID = pb.EST_PolyBag_ID
	where pcm.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentPolybagQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
