IF OBJECT_ID('dbo.EstimatePrimaryPolybagQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimatePrimaryPolybagQuantity'
	DROP FUNCTION dbo.EstimatePrimaryPolybagQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimatePrimaryPolybagQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.EstimatePrimaryPolybagQuantity FAILED.'
END
GO
PRINT 'Creating dbo.EstimatePrimaryPolybagQuantity'
GO

create function dbo.EstimatePrimaryPolybagQuantity(@EST_Estimate_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Estimate_ID - An estimate ID.
*
* DESCRIPTION:
*	Returns the Estimate's Primary Polybag Quantity
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Estimate                    READ
*   EST_EstimatePolybagGroup_Map    READ
*   EST_Polybag                     READ
*
* PROCEDURES CALLED:
*   dbo.PolybagPrimaryEstimate
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
* 10/17/2007      BJS             Initial Creation - Logic copied from EstimateSoloandPrimaryPolybagQuantity
* 05/21/2008      BJS             Replaced call to dbo.PolybagPrimaryEstimate with join logic
*
*/
begin
	declare @retval int

	select @retval = sum(pb.Quantity)
	from EST_Estimate e join EST_EstimatePolybagGroup_Map epgm on e.EST_Estimate_ID = epgm.EST_Estimate_ID
		join EST_Polybag pb on epgm.EST_PolybagGroup_ID = pb.EST_PolybagGroup_ID
		LEFT JOIN EST_EstimatePolybagGroup_Map epgm_primary ON epgm.EST_PolybagGroup_ID = epgm_primary.EST_PolybagGroup_ID
			AND epgm_primary.EstimateOrder < epgm.EstimateOrder
	where e.EST_Estimate_ID = @EST_Estimate_ID AND epgm_primary.EST_Estimate_ID IS NULL

	return(isnull(@retval, 0))
end
GO

GRANT  EXECUTE  ON [dbo].[EstimatePrimaryPolybagQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
