IF OBJECT_ID('dbo.EstComponent_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstComponent_s_ByEstimateID'
	DROP PROCEDURE dbo.EstComponent_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstComponent_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstComponent_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstComponent_s_ByEstimateID'
GO

create proc dbo.EstComponent_s_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns all of the components for the estimate.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Component
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
select * from EST_Component
where EST_Estimate_ID = @EST_Estimate_ID


GO

GRANT  EXECUTE  ON [dbo].[EstComponent_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
