IF OBJECT_ID('dbo.TotalEstimateWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.TotalEstimateWeight'
	DROP FUNCTION dbo.TotalEstimateWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.TotalEstimateWeight') IS NOT NULL
		PRINT '***********Drop of dbo.TotalEstimateWeight FAILED.'
END
GO
PRINT 'Creating dbo.TotalEstimateWeight'
GO

CREATE function dbo.TotalEstimateWeight(@EST_Estimate_ID bigint)
returns decimal(14,6)
/*
* PARAMETERS:
*	EST_Estimate_ID - The Estimate ID
*
* DESCRIPTION:
*	Returns the total estimate weight (total quantity * piece weight)
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Component       READ
*   EST_Samples         READ
*
* PROCEDURES CALLED:
*   dbo.ComponentWeight
*   dbo.ComponentMediaQuantity
*   dbo.ComponentOtherQuantity
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total estimate weight
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/24/2007      BJS             Initial Creation 
*
*/
as
begin
	return(
		select sum(
			dbo.ComponentWeight(c.EST_Component_ID) * (isnull(dbo.ComponentMediaQuantity(c.EST_Component_ID), 0) * (1 + isnull(c.SpoilagePct, 0))
			+ isnull(dbo.ComponentOtherQuantity(c.EST_Component_ID), 0) + isnull(s.Quantity, 0)))
		from EST_Component c left join EST_Samples s on c.EST_Estimate_ID = s.EST_Estimate_ID
		where c.EST_Estimate_ID = @EST_Estimate_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[TotalEstimateWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
