IF OBJECT_ID('dbo.Rpt_ReportHistory') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.Rpt_ReportHistory'
	DROP PROCEDURE dbo.Rpt_ReportHistory
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.Rpt_ReportHistory') IS NOT NULL
		PRINT '***********Drop of dbo.Rpt_ReportHistory FAILED.'
END
GO
PRINT 'Creating dbo.Rpt_ReportHistory'
GO

CREATE PROCEDURE dbo.Rpt_ReportHistory
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Returns a list of previously run reports
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   RptReport
*   RptReportType
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
* 06/20/2007      NLS             Initial Creation 
*
*/
AS BEGIN

SELECT 
	R.rpt_report_id, 
	T.description,
	R.createdby, 
	R.createddate
	
FROM
	rpt_report AS R INNER JOIN
	rpt_reporttype AS T 
ON
	R.rpt_reporttype_id = T.rpt_reporttype_id

ORDER BY R.createddate DESC

END
GO

GRANT  EXECUTE  ON [dbo].[Rpt_ReportHistory]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
