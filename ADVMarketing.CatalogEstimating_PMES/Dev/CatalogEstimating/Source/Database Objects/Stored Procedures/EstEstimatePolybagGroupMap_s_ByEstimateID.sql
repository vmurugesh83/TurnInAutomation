IF OBJECT_ID('dbo.EstEstimatePolybagGroupMap_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstEstimatePolybagGroupMap_s_ByEstimateID'
	DROP PROCEDURE dbo.EstEstimatePolybagGroupMap_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstEstimatePolybagGroupMap_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstEstimatePolybagGroupMap_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstEstimatePolybagGroupMap_s_ByEstimateID'
GO
CREATE PROC dbo.EstEstimatePolybagGroupMap_s_ByEstimateID
/*
* PARAMETERS:
*	EST_Estimate_ID - Required.
*
* DESCRIPTION:
*	Returns EST_EstimatePolybagGroup_Map records referencing the specified EST_Estimate_ID.
*
* TABLES:
*   Table Name                    Access
*   ==========                    ======
*   est_estimate		          READ
*   est_estimatepolybaggroup_map  READ
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 08/31/2007      BJS             Initial Creation 
*
*/
@EST_Estimate_ID bigint
as

select * from EST_EstimatePolybagGroup_Map where EST_Estimate_ID = @EST_Estimate_ID
GO

GRANT  EXECUTE  ON [dbo].[EstEstimatePolybagGroupMap_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
