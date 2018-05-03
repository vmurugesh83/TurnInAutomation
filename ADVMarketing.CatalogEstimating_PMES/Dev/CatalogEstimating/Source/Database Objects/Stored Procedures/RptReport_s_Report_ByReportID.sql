IF OBJECT_ID('dbo.RptReport_s_Report_ByReportID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.RptReport_s_Report_ByReportID'
	DROP PROCEDURE dbo.RptReport_s_Report_ByReportID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.RptReport_s_Report_ByReportID') IS NOT NULL
		PRINT '***********Drop of dbo.RptReport_s_Report_ByReportID FAILED.'
END
GO
PRINT 'Creating dbo.RptReport_s_Report_ByReportID'
GO

CREATE PROCEDURE dbo.RptReport_s_Report_ByReportID
/*
* PARAMETERS:
*	@RPT_Report_ID - Report ID of the report to retrieve
*
* DESCRIPTION:
*	Returns the binary blob for the specified report.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   RptReport			Read
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
	@RPT_Report_ID bigint

AS BEGIN
	
SELECT
	RPT_Report_ID,
	Report

FROM
	RPT_Report

WHERE RPT_Report_ID = @RPT_Report_ID

END
GO

GRANT  EXECUTE  ON [dbo].[RptReport_s_Report_ByReportID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
