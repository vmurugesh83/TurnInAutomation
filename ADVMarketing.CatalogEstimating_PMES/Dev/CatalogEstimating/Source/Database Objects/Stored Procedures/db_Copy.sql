IF OBJECT_ID('dbo.db_Copy') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.db_Copy'
	DROP PROCEDURE dbo.db_Copy
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.db_Copy') IS NOT NULL
		PRINT '***********Drop of dbo.db_Copy FAILED.'
END
GO
PRINT 'Creating dbo.db_Copy'
GO

create proc dbo.db_Copy
/*
* PARAMETERS:
*	@SourceDBName
*
* DESCRIPTION:
*	Initiates the copying of data from SourceDBName
*
* TABLES:
* Table Name Access
* ========== ======
* MANY DELETE
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
* 10/08/2007	JRH		Redirect: see db_CopyAsOwner
*
*/
@SourceDBName varchar(50)
as


DECLARE	@job_name sysname
	, @step_name sysname
	, @db_name sysname
	, @job_run_status int

SELECT	@job_name = 'PMES_SyncCopy'
	, @step_name = 'ExecCopyStoredProc'
	, @db_name = DB_NAME()

INSERT dbo.assoc_database_sync (destination_db_name, source_db_name)
	VALUES (@db_name, @SourceDBName)

EXEC msdb..sp_start_pmes_job @job_name, @db_name, @step_name, @job_run_status output

-- Succeeded if run status = 1
IF (@job_run_status = 1)
	return 0
ELSE
	return 1 

GO

GRANT EXECUTE ON [dbo].[db_Copy] TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
