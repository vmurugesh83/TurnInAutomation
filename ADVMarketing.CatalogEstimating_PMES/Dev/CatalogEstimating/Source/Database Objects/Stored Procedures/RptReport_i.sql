IF OBJECT_ID('dbo.RptReport_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.RptReport_i'
	DROP PROCEDURE dbo.RptReport_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.RptReport_i') IS NOT NULL
		PRINT '***********Drop of dbo.RptReport_i FAILED.'
END
GO
PRINT 'Creating dbo.Rpt_ReportHistory'
GO

CREATE PROCEDURE dbo.RptReport_i
/*
* PARAMETERS:
*	@RPT_Report_ID - Output.  Report ID of the newly inserted report
*	@RPT_ReportType_ID int - Report type Id
*	@Report image - Binary blob of the report
*	@CreatedBy varchar(50) - Username who created the report
*
* DESCRIPTION:
*	Inserts a record into the list of reports.
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
	@RPT_Report_ID bigint output,
	@RPT_ReportType_ID int,
	@Report image,
	@CreatedBy varchar(50)

AS

INSERT INTO RPT_Report( RPT_ReportType_ID,  Report,  CreatedBy,  CreatedDate )
VALUES			      ( @RPT_ReportType_ID, @Report, @CreatedBy, getdate() )

SET @RPT_Report_ID = @@identity

GO

GRANT  EXECUTE  ON [dbo].[RptReport_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
