IF OBJECT_ID('dbo.RPTReports_s_all') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.RPTReports_s_all'
	DROP PROCEDURE dbo.RPTReports_s_all
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.RPTReports_s_all') IS NOT NULL
		PRINT '***********Drop of dbo.RPTReports_s_all FAILED.'
END
GO
PRINT 'Creating dbo.RPTReports_s_all'
GO

create proc dbo.RPTReports_s_all
/*
* PARAMETERS:
*   None
*
* DESCRIPTION:
*	Returns a list of previously run reports
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   RptReport			Insert
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	rpt_report_id 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/20/2007      NLS             Initial Creation 
*
*/
as
select r.RPT_Report_ID, rt.Description, r.CreatedBy, r.CreatedDate
from RPT_Report r join RPT_ReportType rt on r.RPT_ReportType_ID = rt.RPT_ReportType_ID
order by r.CreatedDate desc


GO

GRANT  EXECUTE  ON [dbo].[RPTReports_s_all]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
