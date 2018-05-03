IF OBJECT_ID('dbo.EstPolybagGroup_d_ByPolybagID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPolybagGroup_d_ByPolybagID'
	DROP PROCEDURE dbo.EstPolybagGroup_d_ByPolybagID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPolybagGroup_d_ByPolybagID') IS NOT NULL
		PRINT '***********Drop of dbo.EstPolybagGroup_d_ByPolybagID FAILED.'
END
GO
PRINT 'Creating dbo.EstPolybagGroup_d_ByPolybagID'
GO

CREATE PROC dbo.EstPolybagGroup_d_ByPolybagID
/*
* PARAMETERS:
*   @est_polybaggroup_id - Required.  The polybag group to delete.
*
*
* DESCRIPTION:
*	Deletes a Polybag from the EST_Polybag table.
*
*
* TABLES:
*   Table Name						Access
*   ==========						======
*   EST_PolybagGroup				DELETE
*	EST_Polybag						DELETE
*	EST_PackagePolybag_Map			DELETE
*	EST_EstimatePolybagGroup_Map	DELETE
*	EST_Package						UPDATE
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
* 08/22/2007      BJS             Initial Creation 
* 10/12/2007	  NLS			  Fixed to delete the entire polybag group instead of just a polybag
* 05/19/2008	JLS	          Fix to JOIN statment from duplicate field table name (MAP to MAP) to
*                                 (MAP to P) for the deleting references to the packages.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@est_polybaggroup_id bigint

AS

BEGIN TRAN t

-- Move the polybag quantity to any referenced packages' solo quantity
UPDATE pkg
    SET pkg.soloquantity = isnull(pkg.soloquantity, 0) + pb.quantity
FROM est_package pkg
    JOIN est_packagepolybag_map map ON pkg.est_package_id = map.est_package_id
    JOIN est_polybag pb ON pb.est_polybag_id = map.est_polybag_id
WHERE
    pb.est_polybaggroup_id = @est_polybaggroup_id

IF (@@error <> 0) BEGIN
	ROLLBACK TRAN t
	raiserror('Error updating Package record.', 16, 1)
END

-- Delete any references to the packages
DELETE map FROM est_packagepolybag_map map
	JOIN est_polybag p ON map.est_polybag_id = p.est_polybag_id
WHERE
	p.est_polybaggroup_id = @est_polybaggroup_id

IF (@@error <> 0) BEGIN
	ROLLBACK TRAN t
	raiserror('Error deleting Package Polybag Map record.', 16, 1)
END

-- Delete any references to the estimates
DELETE FROM est_estimatepolybaggroup_map
WHERE est_polybaggroup_id = @est_polybaggroup_id
IF (@@error <> 0) BEGIN
	ROLLBACK TRAN t
	raiserror('Error deleting Polybag record.', 16, 1)
END

-- Delete the polybags
DELETE FROM est_polybag
WHERE est_polybaggroup_id = @est_polybaggroup_id
IF (@@error <> 0) BEGIN
	ROLLBACK TRAN t
	raiserror('Error deleting Polybag record.', 16, 1)
END

-- Delete the polybag group
DELETE FROM est_polybaggroup
WHERE est_polybaggroup_id = @est_polybaggroup_id

COMMIT TRAN t

GO

GRANT  EXECUTE  ON [dbo].[EstPolybagGroup_d_ByPolybagID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
