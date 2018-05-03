IF OBJECT_ID('dbo.EstPolybag_Cleanup_ByPolybagGroupId') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPolybag_Cleanup_ByPolybagGroupId'
	DROP PROCEDURE dbo.EstPolybag_Cleanup_ByPolybagGroupId
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPolybag_Cleanup_ByPolybagGroupId') IS NOT NULL
		PRINT '***********Drop of dbo.EstPolybag_Cleanup_ByPolybagGroupId FAILED.'
END
GO
PRINT 'Creating dbo.EstPolybag_Cleanup_ByPolybagGroupId'
GO

CREATE PROCEDURE dbo.EstPolybag_Cleanup_ByPolybagGroupId
/*
* PARAMETERS:
*	@est_polybaggroup_id - Polybag Group to clean up polybags for
*
* DESCRIPTION:
*	Deletes any polybags for a particular Polybag Group that don't have packages associated with them
*
* TABLES:
*   Table Name				Access
*   ==========				======
*   est_polybag             Delete
*   est_packagepolybag_map	Read
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
* 10/03/2007      NLS             Initial Creation 
*
*/
@est_polybaggroup_id bigint

AS

DELETE pb FROM est_polybag pb
    LEFT JOIN est_packagepolybag_map map on pb.est_polybag_id = map.est_polybag_id
    WHERE map.est_polybag_id is null AND pb.est_polybaggroup_id = @est_polybaggroup_id

GO

GRANT  EXECUTE  ON [dbo].[EstPolybag_Cleanup_ByPolybagGroupId]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO