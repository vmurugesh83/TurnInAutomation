IF OBJECT_ID('dbo.EstEstimate_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstEstimate_s_ByEstimateID'
	DROP PROCEDURE dbo.EstEstimate_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstEstimate_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstEstimate_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstEstimate_s_ByEstimateID'
GO

CREATE PROC dbo.EstEstimate_s_ByEstimateID
/*
* PARAMETERS:
*	@EstEstimateId - required.
*
*
* DESCRIPTION:
* Returns an Estimate record.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate        READ
*
* DATABASE:
*		All
*
*
* RETURN VALUE:
*   none
*
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/04/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/

@EST_Estimate_ID bigint

AS

select * from EST_Estimate
where EST_Estimate_ID = @EST_Estimate_ID
GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO