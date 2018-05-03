IF OBJECT_ID('dbo.PolybagPrimaryHost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagPrimaryHost'
	DROP FUNCTION dbo.PolybagPrimaryHost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagPrimaryHost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagPrimaryHost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagPrimaryHost'
GO

create function dbo.PolybagPrimaryHost(@EST_Polybag_ID bigint)
returns bigint
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the primary host component ID for the specified polybag.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_Polybag                     READ
*   EST_PolybagGroup                READ
*   EST_EstimatePolybagGroup_Map    READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Polybag Primary Host Component
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
*
*/
as
begin
	declare @retval bigint
	select top 1 @retval = EST_Component_ID
	from EST_Polybag pb join EST_PolybagGroup pbg on pb.EST_PolybagGroup_ID = pbg.EST_PolybagGroup_ID
		join EST_EstimatePolybagGroup_Map pbgm on pbg.EST_PolybagGroup_ID = pbgm.EST_PolybagGroup_ID
		join EST_Component c on pbgm.EST_Estimate_ID = c.EST_Estimate_ID
	where pb.EST_Polybag_ID = @EST_Polybag_ID and c.EST_ComponentType_ID = 1
	order by pbgm.EstimateOrder

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagPrimaryHost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
