IF OBJECT_ID('dbo.EstPubIssueDates_s_Overrides_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPubIssueDates_s_Overrides_ByEstimateID'
	DROP PROCEDURE dbo.EstPubIssueDates_s_Overrides_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPubIssueDates_s_Overrides_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstPubIssueDates_s_Overrides_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstPubIssueDates_s_Overrides_ByEstimateID'
GO

create proc dbo.EstPubIssueDates_s_Overrides_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns all EST_PubIssueDate overide records and corresponding pub_id and publoc_id's matching the EST_Estimate_ID
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PubIssueDates   READ
*   PUB_PubRate_Map     READ
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
* 09/11/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint
as
select pid.*, prm.Pub_ID, prm.PubLoc_ID
from EST_PubIssueDates pid join PUB_PubRate_Map prm on pid.PUB_PubRate_Map_ID = prm.PUB_PubRate_Map_ID
where pid.EST_Estimate_ID = @EST_Estimate_ID and pid.Override = 1

GO

GRANT  EXECUTE  ON [dbo].[EstPubIssueDates_s_Overrides_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
