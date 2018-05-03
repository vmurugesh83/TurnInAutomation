IF OBJECT_ID('dbo.EstimateHostAdNumber') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimateHostAdNumber'
	DROP FUNCTION dbo.EstimateHostAdNumber
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimateHostAdNumber') IS NOT NULL
		PRINT '***********Drop of dbo.EstimateHostAdNumber FAILED.'
END
GO
PRINT 'Creating dbo.EstimateHostAdNumber'
GO

CREATE FUNCTION dbo.EstimateHostAdNumber(@EST_Estimate_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Estimate_ID
*
* DESCRIPTION:
*	Returns the ad number of the estimate's host component.
*
* TABLES:
*   Table Name              Access
*   ==========              ======
*   EST_Component           READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	The ad number of the estimate's host component.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/25/2007      BJS             Initial Creation 
*
*/
as
begin
	return(select c.AdNumber
		from EST_Component c
		where c.EST_Estimate_ID = @EST_Estimate_ID and c.EST_ComponentType_ID = 1)
end
GO

GRANT  EXECUTE  ON [dbo].[EstimateHostAdNumber]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO