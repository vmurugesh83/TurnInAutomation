IF OBJECT_ID('dbo.EstimateSoloAndPrimaryPolybagQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimateSoloAndPrimaryPolybagQuantity'
	DROP FUNCTION dbo.EstimateSoloAndPrimaryPolybagQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimateSoloAndPrimaryPolybagQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.EstimateSoloAndPrimaryPolybagQuantity FAILED.'
END
GO
PRINT 'Creating dbo.EstimateSoloAndPrimaryPolybagQuantity'
GO

create function dbo.EstimateSoloAndPrimaryPolybagQuantity(@EST_Estimate_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Estimate_ID - An estimate ID.
*
* DESCRIPTION:
*	Returns the total mailing quantity that will use this estimate's a&d rates.
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
*   dbo.PolybagPrimaryEstimate
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Mail Quantity used to perform A&D calculations.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation
* 07/26/2007      BJS             Renamed ComponentSoloAndPrimaryPolybagQuantity -> EstimateSoloAndPrimaryPolybagQuantity
* 10/17/2007      BJS             Changed reference to dbo.PolybagPrimaryHost -> dbo.PolybagPrimaryEstimate
* 05/21/2008      BJS             Replaced call to dbo.PolybagPrimaryEstimate with join logic
*
*/
begin
	declare @SoloQuantity int, @PrimaryPolybagQuantity int, @retval int

	select @SoloQuantity = sum(p.SoloQuantity)
	from EST_Package p
	where p.EST_Estimate_ID = @EST_Estimate_ID

	select @PrimaryPolybagQuantity = sum(pb.Quantity)
	from EST_Estimate e join EST_EstimatePolybagGroup_Map epgm on e.EST_Estimate_ID = epgm.EST_Estimate_ID
		join EST_Polybag pb on epgm.EST_PolybagGroup_ID = pb.EST_PolybagGroup_ID
		LEFT JOIN EST_EstimatePolybagGroup_Map epgm_primary ON epgm.EST_PolybagGroup_ID = epgm_primary.EST_PolybagGroup_ID
			AND epgm_primary.EstimateOrder < epgm.EstimateOrder
	where e.EST_Estimate_ID = @EST_Estimate_ID AND epgm_primary.EST_Estimate_ID IS NULL

	set @retval = isnull(@SoloQuantity, 0) + isnull(@PrimaryPolybagQuantity, 0)
	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[EstimateSoloAndPrimaryPolybagQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
