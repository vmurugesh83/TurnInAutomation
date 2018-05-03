USE msdb

IF OBJECT_ID('dbo.sp_start_pmes_job') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.sp_start_pmes_job'
	DROP PROCEDURE dbo.sp_start_pmes_job
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.sp_start_pmes_job') IS NOT NULL
		PRINT '***********Drop of dbo.sp_start_pmes_job FAILED.'
END
GO
PRINT 'Creating dbo.sp_start_pmes_job'
GO

CREATE PROCEDURE dbo.sp_start_pmes_job
	@job_name sysname
	, @db_name sysname
	, @step_name sysname
	, @job_run_status int output
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*
*/
AS BEGIN
-- declare variables used in the stored procedure
DECLARE
	@job_id uniqueidentifier,
	@step_id int,
	@current_on_success_action tinyint,
	@current_on_success_step_id int,
	@nt_user_name varchar(50),
	@retval int,
	@retmsg varchar(50),
	@max_instance_id int,
	@history_count int

select
	@retval = 0,	-- success
	@retmsg = NULL,
	@nt_user_name = ISNULL(NT_CLIENT(),ISNULL(SUSER_SNAME(),'Not Found')),
	@history_count = 0

-- Retrieve the job_id information 
SELECT	@job_id = job_id
FROM	msdb..sysjobs
WHERE	name = @job_name

-- Run in the requested database
UPDATE	msdb..sysjobsteps
SET	database_name = @db_name
WHERE	job_id = @job_id

--  Assure the job is a PMES job before continuing. 
--  This stored proc was written specifiaclly for starting of Print Media Estimating System Jobs.
IF (@job_name not like '%PMES%')
	BEGIN
		SELECT	@retval = 1,
			@retmsg = 'Starting of Job Not Allowed.  Must be a PMES job.'
		RAISERROR(@retmsg, -1, -1)
	END
ELSE
	IF (@job_id is NULL)
		BEGIN
			SELECT 	@retval = 1,
				@retmsg = 'Job or Step Not Found'
			RAISERROR(@retmsg,-1,-1)
		END
	ELSE
		BEGIN	
			-- get last entry for monitoring job
			SELECT	@max_instance_id = MAX(instance_id)
			FROM	msdb..sysjobhistory

			-- launch job
			EXEC 	@retval = master.dbo.xp_sqlagent_notify 'J',
	     			@job_id = @job_id, 
	     			@schedule_id = NULL, 
	     			@alert_id = NULL,
	     			@action_type = 'R',   -- R = Run, C = Stop
	     			@nt_user_name = @nt_user_name,
	     			@error_flag = '1'

			-- loop until job completes
			WHILE (@history_count = 0)
				SELECT	@history_count = count(*)
				FROM	msdb..sysjobhistory
				WHERE	job_id = @job_id
					and step_name = @step_name
					and instance_id > @max_instance_id

			-- return instance id to caller
			SELECT TOP 1
				@job_run_status = run_status,
			 	@retval = 0,
			 	@retmsg = 'Job/Step Completed'
				FROM	msdb..sysjobhistory
				WHERE	job_id = @job_id
					and step_name = @step_name
					and instance_id > @max_instance_id
				ORDER BY instance_id DESC

			RAISERROR(@retmsg, -1, -1)
		END
END
GO

GRANT  EXECUTE  ON [dbo].[sp_start_pmes_job]  TO [PMES_SuperAdmin], [PMES_RateAdmin]
GO
