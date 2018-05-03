IF OBJECT_ID('dbo.EstEstimate_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstEstimate_i'
	DROP PROCEDURE dbo.EstEstimate_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstEstimate_i') IS NOT NULL
		PRINT '***********Drop of dbo.EstEstimate_i FAILED.'
END
GO
PRINT 'Creating dbo.EstEstimate_i'
GO

create proc dbo.EstEstimate_i
/*
* PARAMETERS:
* EST_Estimate_ID - Output.  If a value is passed in it is ignored and the value is set to the ID of the new record.
*
*
* DESCRIPTION:
*		Inserts an estimate into the EST_Estimate table.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate
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
@EST_Estimate_ID bigint output,
@Description varchar(35),
@Comments varchar(255),
@EST_Season_ID int,
@FiscalYear int,
@RunDate datetime,
@EST_Status_ID int,
@FiscalMonth int,
@Parent_ID bigint,
@CreatedBy varchar(50)
as
insert into EST_Estimate(Description, Comments, EST_Season_ID, FiscalYear, RunDate, EST_Status_ID, FiscalMonth, Parent_ID, CreatedBy, CreatedDate)
values(@Description, @Comments, @EST_Season_ID, @FiscalYear, @RunDate, @EST_Status_ID, @FiscalMonth, @Parent_ID, @CreatedBy, getdate())
set @EST_Estimate_ID = @@identity


GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
