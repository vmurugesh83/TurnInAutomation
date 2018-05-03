IF OBJECT_ID('dbo.PackagePolyQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PackagePolyQuantity'
	DROP FUNCTION dbo.PackagePolyQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PackagePolyQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.PackagePolyQuantity FAILED.'
END
GO
PRINT 'Creating dbo.PackagePolyQuantity'
GO

create function dbo.PackagePolyQuantity(@EST_Package_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Package_ID
*
* DESCRIPTION:
*	Returns the total polybag quantity for a package.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PackagePolyBag_Map
*   EST_PolyBag
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Polybag Quantity for Package
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/21/2007      BJS             Initial Creation 
*
*/
as
begin
return (select sum(pb.Quantity)
	from EST_PackagePolyBag_Map ppm	join EST_PolyBag pb on ppm.EST_PolyBag_ID = pb.EST_PolyBag_ID
	where ppm.EST_Package_ID = @EST_Package_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[PackagePolyQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
