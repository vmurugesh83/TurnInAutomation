IF OBJECT_ID('dbo.PolybagPrimaryEstimate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagPrimaryEstimate'
	DROP FUNCTION dbo.PolybagPrimaryEstimate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagPrimaryEstimate') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagPrimaryEstimate FAILED.'
END
GO
-- PRINT 'Creating dbo.PolybagPrimaryEstimate'
-- GO

/* 05/21/2008   BJS   Dropped, no longer needed */

/*
create function dbo.PolybagPrimaryEstimate(@EST_Polybag_ID bigint)
returns bigint
*/

/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the primary Estimate ID for the specified polybag.
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
* 10/17/2007      BJS             Initial Creation - Logic Copied from PolybagPrimaryHost
*
*/

-- as
-- begin
-- 	declare @retval bigint
-- 	select top 1 @retval = pbgm.EST_Estimate_ID
-- 	from EST_Polybag pb join EST_PolybagGroup pbg on pb.EST_PolybagGroup_ID = pbg.EST_PolybagGroup_ID
-- 		join EST_EstimatePolybagGroup_Map pbgm on pbg.EST_PolybagGroup_ID = pbgm.EST_PolybagGroup_ID
-- 	where pb.EST_Polybag_ID = @EST_Polybag_ID
-- 	order by pbgm.EstimateOrder
-- 
-- 	return(@retval)
-- end
-- GO
-- 
-- GRANT  EXECUTE  ON [dbo].[PolybagPrimaryEstimate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
-- GO
