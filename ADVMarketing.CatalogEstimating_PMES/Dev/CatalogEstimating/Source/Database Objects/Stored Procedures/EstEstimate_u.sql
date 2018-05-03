IF OBJECT_ID('dbo.EstEstimate_u') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstEstimate_u'
	DROP PROCEDURE dbo.EstEstimate_u
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstEstimate_u') IS NOT NULL
		PRINT '***********Drop of dbo.EstEstimate_u FAILED.'
END
GO
PRINT 'Creating dbo.EstEstimate_u'
GO

create proc dbo.EstEstimate_u
/*
* PARAMETERS:
* EST_Estimate_ID - The ID of the record to update.
*
*
* DESCRIPTION:
*		Updates an Estimate in the EST_Estimate table.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate        UPDATE
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
@EST_Estimate_ID bigint,
@Description varchar(35),
@Comments varchar(255),
@EST_Season_ID int,
@RunDate datetime,
@EST_Status_ID int,
@Parent_ID int,
@UploadDate datetime,
@ModifiedBy varchar(50)
as

begin tran t

if (@EST_Status_ID = 3) begin
	-- Only Active Estimates can be killed
	if exists(select 1 from EST_Estimate where EST_Estimate_ID = @EST_Estimate_ID and EST_Status_ID not in (1,3)) begin
		rollback tran t
		raiserror('Error updating Estimate.', 16, 1)
		return
	end

	--Estimates cannot be killed if they are a member of a polybag
	if exists(select 1 from EST_Estimate e join EST_EstimatePolybagGroup_Map pbgm on e.EST_Estimate_ID = pbgm.EST_Estimate_ID
		where e.EST_Estimate_ID = @EST_Estimate_ID) begin

		rollback tran t
		raiserror('Error updating Estimate.', 16, 1)
		return
	end
end
		
update EST_Estimate
	set Description = @Description,
		Comments = @Comments,
		EST_Season_ID = @EST_Season_ID,
		FiscalYear = dbo.getFiscalYear(@RunDate),
		RunDate = @RunDate,
		EST_Status_ID = @EST_Status_ID,
		FiscalMonth = dbo.getFiscalMonth(@RunDate),
		Parent_ID = @Parent_ID,
		UploadDate = @UploadDate,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where EST_Estimate_ID = @EST_Estimate_ID
if (@@error <> 0) begin
	rollback tran t
	raiserror('Error updating EST_Estimate.', 16, 1)
	return
end

commit tran t

GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_u]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
