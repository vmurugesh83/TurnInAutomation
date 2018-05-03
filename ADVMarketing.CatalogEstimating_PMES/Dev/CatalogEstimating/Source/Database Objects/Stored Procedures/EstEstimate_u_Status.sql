IF OBJECT_ID('dbo.EstEstimate_u_Status') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstEstimate_u_Status'
	DROP PROCEDURE dbo.EstEstimate_u_Status
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstEstimate_u_Status') IS NOT NULL
		PRINT '***********Drop of dbo.EstEstimate_u_Status FAILED.'
END
GO
PRINT 'Creating dbo.EstEstimate_u_Status'
GO

CREATE PROC dbo.EstEstimate_u_Status
/*
* PARAMETERS:
*	EST_Estimate_ID - The ID of the record to update.
*	EST_Status_ID   - The new status ID to update the estimate with
*	ModifiedBy		- The user modifying this record
*
* DESCRIPTION:
*		Updates an Estimate status in the EST_Estimate table.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate        UPDATE
*
* PROCEDURES CALLED:
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 08/15/2007      NLS             Initial Creation
* 08/31/2007      BJS             Added begin/rollback/commit tran.  Added polybag reference check. 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
	@EST_Estimate_ID bigint,
	@EST_Status_ID int,
	@ModifiedBy varchar(50)

AS

begin tran t

if (@EST_Status_ID = 3) begin
	--You can only kill an active estimate
	if exists(select 1 from EST_Estimate where EST_Estimate_ID = @EST_Estimate_ID and EST_Status_ID not in (1,3)) begin
		rollback tran t
		raiserror('Error updating Estimate Status', 16, 1)
		return
	end

	--An estimate cannot be a member of a polybag group if you want to kill it
	if exists(select 1 from EST_Estimate e join EST_EstimatePolybagGroup_Map pbgm on e.EST_Estimate_ID = pbgm.EST_Estimate_ID where e.EST_Estimate_ID = @EST_Estimate_ID) begin
		rollback tran t
		raiserror('Error updating Estimate Status', 16, 1)
		return
	end
end

UPDATE EST_Estimate 

SET EST_Status_ID = @EST_Status_ID,
	Modifiedby    = @ModifiedBy,
	Modifieddate  = getdate()

WHERE EST_Estimate_ID = @EST_Estimate_ID
if (@@error <> 0) begin
	rollback tran t
	raiserror('Error updating Estimate Status', 16, 1)
	return
end

commit tran t
GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_u_Status]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO