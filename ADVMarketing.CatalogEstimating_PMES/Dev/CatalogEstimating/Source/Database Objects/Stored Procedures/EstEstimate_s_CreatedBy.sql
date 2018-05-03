IF OBJECT_ID('dbo.EstEstimate_s_CreatedBy') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstEstimate_s_CreatedBy'
	DROP PROCEDURE dbo.EstEstimate_s_CreatedBy
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstEstimate_s_CreatedBy') IS NOT NULL
		PRINT '***********Drop of dbo.EstEstimate_s_CreatedBy FAILED.'
END
GO
PRINT 'Creating dbo.EstEstimate_s_CreatedBy'
GO

CREATE PROCEDURE dbo.EstEstimate_s_CreatedBy
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Returns a list of unique user names that have created estimates
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   est_estimate		Read
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
* 07/20/2007      NLS             Initial Creation 
*
*/
AS BEGIN

SELECT 
	DISTINCT createdby

FROM dbo.est_estimate

ORDER BY createdby ASC

END
GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_s_CreatedBy]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
