IF OBJECT_ID('dbo.EstEstimate_s_FiscalYear') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstEstimate_s_FiscalYear'
	DROP PROCEDURE dbo.EstEstimate_s_FiscalYear
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstEstimate_s_FiscalYear') IS NOT NULL
		PRINT '***********Drop of dbo.EstEstimate_s_FiscalYear FAILED.'
END
GO
PRINT 'Creating dbo.EstEstimate_s_FiscalYear'
GO

CREATE PROCEDURE dbo.EstEstimate_s_FiscalYear
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Returns a list of unique fiscal years used by estimates.
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
* 08/13/2007      NLS             Initial Creation 
*
*/
AS BEGIN

SELECT 
	DISTINCT fiscalyear

FROM dbo.est_estimate

ORDER BY fiscalyear ASC

END
GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_s_FiscalYear]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
