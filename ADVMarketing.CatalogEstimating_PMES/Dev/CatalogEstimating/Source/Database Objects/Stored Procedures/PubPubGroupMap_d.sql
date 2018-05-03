IF OBJECT_ID('dbo.PubPubGroupMap_d') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubPubGroupMap_d'
	DROP PROCEDURE dbo.PubPubGroupMap_d
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubPubGroupMap_d') IS NOT NULL
		PRINT '***********Drop of dbo.PubPubGroupMap_d FAILED.'
END
GO
PRINT 'Creating dbo.PubPubGroupMap_d'
GO

CREATE PROCEDURE dbo.PubPubGroupMap_d
/*
* PARAMETERS:
*	PUB_PubRate_Map_ID
*   PUB_PubGroup_ID
*
* DESCRIPTION:
*	Removes the pub location from the specified pub group.
*
* TABLES:
*   Table Name                 Access
*   ==========                 ======
*   EST_Package                READ
*   EST_PubIssueDates          DELETE
*   PUB_PubPubGroup_Map        DELETE
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
* 10/08/2007      BJS             Initial Creation
*
*/
@PUB_PubRate_Map_ID bigint,
@PUB_PubGroup_ID bigint
as

/* Delete the record if it exists */
if exists(select 1 from PUB_PubPubGroup_Map where PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID and PUB_PubGroup_ID = @PUB_PubGroup_ID) begin
	delete pid from EST_Package p join EST_PubIssueDates pid on p.EST_Estimate_ID = pid.EST_Estimate_ID
		join PUB_PubPubGroup_Map ppgm on p.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID and pid.PUB_PubRate_Map_ID = ppgm.PUB_PubRate_Map_ID
	where ppgm.PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID and ppgm.PUB_PubGroup_ID = @PUB_PubGroup_ID
	if (@@error <> 0) begin
		raiserror('Error deleting EST_PubIssueDate record(s).', 16, 1)
		return
	end

	delete from PUB_PubPubGroup_Map
	where PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID and PUB_PubGroup_ID = @PUB_PubGroup_ID
	if (@@error <> 0) begin
		raiserror('Error deleting PUB_PubPubGroup_Map record.', 16, 1)
		return
	end
end
GO

GRANT  EXECUTE  ON [dbo].[PubPubGroupMap_d]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
