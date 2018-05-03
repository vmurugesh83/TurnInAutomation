IF OBJECT_ID('dbo.PolybagWeightIncludeBagWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagWeightIncludeBagWeight'
	DROP FUNCTION dbo.PolybagWeightIncludeBagWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagWeightIncludeBagWeight') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagWeightIncludeBagWeight FAILED.'
END
GO
PRINT 'Creating dbo.PolybagWeightIncludeBagWeight'
GO

create function dbo.PolybagWeightIncludeBagWeight(@EST_Polybag_ID bigint)
returns decimal(12,6)
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag weight including the weight of the bag (default is .0213).
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_PackageComponentMapping     READ
*   EST_Package                     READ
*   EST_PackagePolybag_Map          READ
*   EST_Polybag                     READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	The polybag weight including the weight of the bag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/23/2007      BJS             Initial Creation 
*
*/
as
begin
	declare @TotalPackageWeight decimal(12,6)
	declare @PolybagBagWeight decimal(12,6)

	select @TotalPackageWeight = sum(dbo.PackageWeight(ppm.EST_Package_ID))
	from EST_PackagePolybag_Map ppm
	where ppm.EST_Polybag_ID = @EST_Polybag_ID

	select @PolybagBagWeight = prt.PolybagBagWeight
	from EST_Polybag pb join EST_PolybagGroup pbg on pb.EST_PolybagGroup_ID = pbg.EST_PolybagGroup_ID
		join VND_Printer prt on pbg.VND_Printer_ID = prt.VND_Printer_ID
	where pb.EST_Polybag_ID = @EST_Polybag_ID

	return(isnull(@TotalPackageWeight, 0) + isnull(@PolybagBagWeight, 0))
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagWeightIncludeBagWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
