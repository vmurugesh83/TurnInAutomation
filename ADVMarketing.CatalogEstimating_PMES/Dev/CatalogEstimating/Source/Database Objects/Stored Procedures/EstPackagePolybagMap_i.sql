IF OBJECT_ID('dbo.EstPackagePolybagMap_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPackagePolybagMap_i'
	DROP PROCEDURE dbo.EstPackagePolybagMap_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPackagePolybagMap_i') IS NOT NULL
		PRINT '***********Drop of dbo.EstPackagePolybagMap_i FAILED.'
END
GO
PRINT 'Creating dbo.EstPackagePolybagMap_i'
GO

CREATE PROCEDURE dbo.EstPackagePolybagMap_i
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Inserts a polybag package map record and updates the package solo mail quantity
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
* 10/11/2007      NLS             Fixed precision bug on decimal parameter for dist pct
*
*/
@est_package_id bigint,
@est_polybag_id bigint,
@distributionpct decimal(10,4),
@createdby varchar(50)

AS

DECLARE @oldsolomail int
DECLARE @polybagqty int

INSERT INTO est_packagepolybag_map
		( est_package_id,  est_polybag_id,  distributionpct,  createdby,  createddate )
VALUES	( @est_package_id, @est_polybag_id, @distributionpct, @createdby, getdate() )

SELECT @polybagqty =  quantity	   FROM est_polybag WHERE est_polybag_id = @est_polybag_id
SELECT @oldsolomail = soloquantity FROM est_package WHERE est_package_id = @est_package_id

DECLARE @newsolomail int
SET @newsolomail = @oldsolomail - @polybagqty
IF @newsolomail < 0 BEGIN SET @newsolomail = 0 END

UPDATE est_package SET 
	soloquantity = ( @newsolomail ),
	modifiedby = @createdby,
	modifieddate = getdate()
	WHERE est_package_id = @est_package_id

GO

GRANT  EXECUTE  ON [dbo].[EstPackagePolybagMap_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO