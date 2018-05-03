IF OBJECT_ID('dbo.EstSamples_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstSamples_s_ByEstimateID'
	DROP PROCEDURE dbo.EstSamples_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstSamples_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstSamples_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstSamples_s_ByEstimateID'
GO

CREATE proc dbo.EstSamples_s_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns all of the samples for the estimate.  Used on the Samples Screen
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Samples
*
*
* PROCEDURES CALLED:
*
*
* DATABASE:
*		All
*
*
* RETURN VALUE:
* None 
*
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 05/25/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint
as
select * from EST_Samples
where EST_Estimate_ID = @EST_Estimate_ID


GO

GRANT  EXECUTE  ON [dbo].[EstSamples_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
