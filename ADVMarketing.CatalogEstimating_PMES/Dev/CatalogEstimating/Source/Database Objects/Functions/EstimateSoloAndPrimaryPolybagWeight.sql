IF OBJECT_ID('dbo.EstimateSoloAndPrimaryPolybagWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimateSoloAndPrimaryPolybagWeight'
	DROP FUNCTION dbo.EstimateSoloAndPrimaryPolybagWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimateSoloAndPrimaryPolybagWeight') IS NOT NULL
		PRINT '***********Drop of dbo.EstimateSoloAndPrimaryPolybagWeight FAILED.'
END
GO
PRINT 'Creating dbo.EstimateSoloAndPrimaryPolybagWeight'
GO

create function dbo.EstimateSoloAndPrimaryPolybagWeight(@EST_Estimate_ID bigint)
returns decimal(14,6)
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
*   dbo.PolybagWeightIncludeBagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Mail Weight used to perform A&D calculations.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/19/2007      BJS             Initial Creation
* 10/17/2007      BJS             Changed reference to dbo.PolybagPrimaryHost -> dbo.PolybagPrimaryEstimate
* 05/21/2008      BJS             Replaced call to dbo.PolybagPrimaryEstimate with join logic to improve performance
*
*/
begin
	declare @SoloWeight decimal(14,6), @PrimaryPolybagWeight decimal(14,6), @retval decimal(14,6)

	select @SoloWeight = sum(isnull(p.SoloQuantity, 0) * (c.Width * c.Height) / 950000 * c.PageCount * pw.Weight * 1.03)
	from EST_Package p join EST_PackageComponentMapping pcm on p.EST_Package_ID = pcm.EST_Package_ID
		join EST_Component c on pcm.EST_Component_ID = c.EST_Component_ID
		join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
	where p.EST_Estimate_ID = @EST_Estimate_ID

	select @PrimaryPolybagWeight = sum(dbo.PolybagWeightIncludeBagWeight(pb.EST_Polybag_ID))
	from EST_Estimate e join EST_EstimatePolybagGroup_Map epgm on e.EST_Estimate_ID = epgm.EST_Estimate_ID
		join EST_Polybag pb on epgm.EST_PolybagGroup_ID = pb.EST_PolybagGroup_ID
		LEFT JOIN EST_EstimatePolybagGroup_Map epgm_primary ON epgm.EST_PolybagGroup_ID = epgm_primary.EST_PolybagGroup_ID
			AND epgm_primary.EstimateOrder < epgm.EstimateOrder
	where e.EST_Estimate_ID = @EST_Estimate_ID AND epgm_primary.EST_Estimate_ID IS NULL

	set @retval = isnull(@SoloWeight, 0) + isnull(@PrimaryPolybagWeight, 0)
	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[EstimateSoloAndPrimaryPolybagWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
