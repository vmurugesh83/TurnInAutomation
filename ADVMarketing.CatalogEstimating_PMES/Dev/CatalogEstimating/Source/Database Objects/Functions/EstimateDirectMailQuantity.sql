IF OBJECT_ID('dbo.EstimateDirectMailQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimateDirectMailQuantity'
	DROP FUNCTION dbo.EstimateDirectMailQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimateDirectMailQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.EstimateDirectMailQuantity FAILED.'
END
GO
PRINT 'Creating dbo.EstimateDirectMailQuantity'
GO

create function dbo.EstimateDirectMailQuantity(@EST_Estimate_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Estimate_ID
*
* DESCRIPTION:
*	Returns the total mail quantity for the estimate.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Package
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
*	Total Mail Quantity for Estimate (Solo + Poly).
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

declare @SoloMailQuantity int
declare @PolyBagQuantity int

select @SoloMailQuantity = sum(SoloQuantity)
from EST_Package
where EST_Estimate_ID = @EST_Estimate_ID

select @PolyBagQuantity = sum(pb.Quantity)
from EST_Package p join EST_PackagePolyBag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
	join EST_PolyBag pb on ppm.EST_PolyBag_ID = pb.EST_PolyBag_ID
where p.EST_Estimate_ID = @EST_Estimate_ID

return (isnull(@SoloMailQuantity, 0) + isnull(@PolyBagQuantity, 0))
end
GO

GRANT  EXECUTE  ON [dbo].[EstimateDirectMailQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
