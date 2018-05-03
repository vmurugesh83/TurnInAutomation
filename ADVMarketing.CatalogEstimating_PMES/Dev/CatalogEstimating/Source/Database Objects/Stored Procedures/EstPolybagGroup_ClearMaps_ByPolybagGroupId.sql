IF OBJECT_ID('dbo.EstPolybagGroup_ClearMaps_ByPolybagGroupId') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPolybagGroup_ClearMaps_ByPolybagGroupId'
	DROP PROCEDURE dbo.EstPolybagGroup_ClearMaps_ByPolybagGroupId
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPolybagGroup_ClearMaps_ByPolybagGroupId') IS NOT NULL
		PRINT '***********Drop of dbo.EstPolybagGroup_ClearMaps_ByPolybagGroupId FAILED.'
END
GO
PRINT 'Creating dbo.EstPolybagGroup_ClearMaps_ByPolybagGroupId'
GO

CREATE PROCEDURE dbo.EstPolybagGroup_ClearMaps_ByPolybagGroupId
/*
* PARAMETERS:
*	@est_polybaggroup_id - Polybag Group to clear map tables for
*
* DESCRIPTION:
*   Clears the est_estimatepolybaggroup_map and est_packagepolybag_map tables for the
*   given polybag group
*
* TABLES:
*   Table Name				        Access
*   ==========				        ======
*   est_estimatepolybaggroup_map    Delete
*   est_packagepolybag_Map  	    Delete
*   est_package                     Update
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

-- First update the solo quanities of all the packages I'm deleting a map to
UPDATE pkg
    SET pkg.soloquantity = isnull(pkg.soloquantity, 0) + pb.quantity
FROM est_package pkg
    JOIN est_packagepolybag_map map ON pkg.est_package_id = map.est_package_id
    JOIN est_polybag pb ON pb.est_polybag_id = map.est_polybag_id
WHERE
    pb.est_polybaggroup_id = @est_polybaggroup_id

-- Now clear both the map tables
DELETE map FROM est_packagepolybag_map map
    JOIN est_polybag pb ON map.est_polybag_id = pb.est_polybag_id
WHERE
    pb.est_polybaggroup_id = @est_polybaggroup_id

DELETE FROM est_estimatepolybaggroup_map
    WHERE est_polybaggroup_id = @est_polybaggroup_id

GO

GRANT  EXECUTE  ON [dbo].[EstPolybagGroup_ClearMaps_ByPolybagGroupId]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO