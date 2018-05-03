IF OBJECT_ID('dbo.PolybagWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagWeight'
	DROP FUNCTION dbo.PolybagWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagWeight') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagWeight FAILED.'
END
GO
PRINT 'Creating dbo.PolybagWeight'
GO

create function dbo.PolybagWeight(@EST_Polybag_ID bigint)
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the sum of the package weights contained by the polybag (not including the bag weight).
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
*	The polybag weight
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/28/2007      BJS             Initial Creation 
*
*/
returns decimal(12,6)
as
begin
	return(select sum(dbo.PackageWeight(ppm.EST_Package_ID))
		from EST_PackagePolybag_Map ppm
		where ppm.EST_Polybag_ID = @EST_Polybag_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
