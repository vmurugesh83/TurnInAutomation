IF OBJECT_ID('dbo.EstPackagePolybagMap_d') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPackagePolybagMap_d'
	DROP PROCEDURE dbo.EstPackagePolybagMap_d
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPackagePolybagMap_d') IS NOT NULL
		PRINT '***********Drop of dbo.EstPackagePolybagMap_d FAILED.'
END
GO
PRINT 'Creating dbo.EstPackagePolybagMap_d'
GO

CREATE PROCEDURE dbo.EstPackagePolybagMap_d
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Deletes a polybag package map record and updates the package solo mail quantity
*
* TABLES:
*   Table Name				Access
*   ==========				======
*   est_packagepolybag_map	Insert
*	est_package				Update
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
* 08/31/2007      NLS             Initial Creation 
*
*/
@est_package_id bigint,
@est_polybag_id bigint

AS

DECLARE @oldsolomail int
DECLARE @polybagqty int

DELETE FROM est_packagepolybag_map
	WHERE est_package_id = @est_package_id AND est_polybag_id = @est_polybag_id

SELECT @polybagqty =  quantity	   FROM est_polybag WHERE est_polybag_id = @est_polybag_id
SELECT @oldsolomail = soloquantity FROM est_package WHERE est_package_id = @est_package_id

UPDATE est_package
	SET soloquantity = ( @oldsolomail + @polybagqty )
	WHERE est_package_id = @est_package_id
GO

GRANT  EXECUTE  ON [dbo].[EstPackagePolybagMap_d]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO