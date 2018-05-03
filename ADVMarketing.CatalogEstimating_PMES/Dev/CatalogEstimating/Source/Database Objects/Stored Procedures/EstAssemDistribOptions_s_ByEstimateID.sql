IF OBJECT_ID('dbo.EstAssemDistribOptions_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstAssemDistribOptions_s_ByEstimateID'
	DROP PROCEDURE dbo.EstAssemDistribOptions_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstAssemDistribOptions_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstAssemDistribOptions_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstAssemDistribOptions_s_ByEstimateID'
GO

create proc dbo.EstAssemDistribOptions_s_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns the Assembly / Distribution Options for the estimate.  Used on the Assembly / Distribution Options Screen.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_AssemDistribOptions
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
@EST_Estimate_ID int
as
select * from EST_AssemDistribOptions
where EST_Estimate_ID = @EST_Estimate_ID


GO

GRANT  EXECUTE  ON [dbo].[EstAssemDistribOptions_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
