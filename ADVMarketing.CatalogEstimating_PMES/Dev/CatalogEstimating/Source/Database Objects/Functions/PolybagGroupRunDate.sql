IF OBJECT_ID('dbo.PolybagGroupRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagGroupRunDate'
	DROP FUNCTION dbo.PolybagGroupRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagGroupRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagGroupRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PolybagGroupRunDate'
GO

create function dbo.PolybagGroupRunDate(@EST_PolybagGroup_ID bigint)
returns datetime
/*
* PARAMETERS:
*	EST_PolybagGroup_ID
*
* DESCRIPTION:
*	Returns the "newest" run date of each of the estimate(s) in the Polybag Group.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   EST_EstimatePolybagGroup_Map READ
*   EST_Estimate                 READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	The polybag run date.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/27/2007      BJS             Initial Creation 
*
*/
as
begin
	return(
	select max(e.RunDate)
	from EST_EstimatePolybagGroup_Map pbgm join EST_Estimate e on pbgm.EST_Estimate_ID = e.EST_Estimate_ID
	where pbgm.EST_PolybagGroup_ID = @EST_PolybagGroup_ID)
end

GO

GRANT  EXECUTE  ON [dbo].[PolybagGroupRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO