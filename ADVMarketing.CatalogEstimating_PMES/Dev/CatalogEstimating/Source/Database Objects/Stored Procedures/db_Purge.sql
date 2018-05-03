IF OBJECT_ID('dbo.db_Purge') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.db_Purge'
	DROP PROCEDURE dbo.db_Purge
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.db_Purge') IS NOT NULL
		PRINT '***********Drop of dbo.db_Purge FAILED.'
END
GO
PRINT 'Creating dbo.db_Purge'
GO

CREATE PROCEDURE dbo.db_Purge
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Initiates the purging of the database of all non-essential data
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   MANY                DELETE
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
* Date		Who		Comments
* ----------	----		-------------------------------------------------
* 09/12/2007	BJS		Initial Creation
* 09/26/2007	BJS		Modified reference to EST_PubIssueDates
* 10/08/2007	JRH		Redirect: see db_PurgeAsOwner
*
*/
AS

DECLARE	@job_name sysname
	, @step_name sysname
	, @db_name sysname
	, @job_run_status int

SELECT	@job_name = 'PMES_SyncPurge'
	, @step_name = 'ExecPurgeStoredProc'
	, @db_name = DB_NAME()

EXEC msdb..sp_start_pmes_job @job_name, @db_name, @step_name, @job_run_status output

-- Succeeded if run status = 1
IF (@job_run_status = 1)
	return 0
ELSE
	return 1 

GO
GRANT  EXECUTE  ON [dbo].[db_Purge]  TO [PMES_SuperAdmin], [PMES_RateAdmin]
GO