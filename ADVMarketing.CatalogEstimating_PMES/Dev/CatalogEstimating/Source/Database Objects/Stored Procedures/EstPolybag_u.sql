IF OBJECT_ID('dbo.EstPolybag_u') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPolybag_u'
	DROP PROCEDURE dbo.EstPolybag_u
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPolybag_u') IS NOT NULL
		PRINT '***********Drop of dbo.EstPolybag_u FAILED.'
END
GO
PRINT 'Creating dbo.EstPolybag_u'
GO

CREATE PROCEDURE dbo.EstPolybag_u
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Updates a polybag record and updates the package solo mail quantity for all linked
*   packages in the polybag
*
* TABLES:
*   Table Name				Access
*   ==========				======
*   est_polybag				Update
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
* 09/27/2007      NLS             Fixed incorrect stored procedure
*
*/
@est_polybag_id bigint,
@est_polybaggroup_id bigint,
@pst_postalscenario_id bigint,
@quantity int,
@modifiedby varchar(50)

AS

DECLARE @oldquantity int
DECLARE @package_id bigint
DECLARE @oldsolomail int
DECLARE @newsolomail int

SELECT @oldquantity = quantity FROM est_polybag WHERE est_polybag_id = @est_polybag_id

UPDATE est_polybag SET 
	pst_postalscenario_id = @pst_postalscenario_id,
	quantity = @quantity,
	modifiedby = @modifiedby,
	modifieddate = getdate()
WHERE
	est_polybag_id = @est_polybag_id AND
	est_polybaggroup_id = @est_polybaggroup_id
    

DECLARE PackageCursor CURSOR LOCAL FORWARD_ONLY FOR
    SELECT est_package_id FROM est_packagepolybag_map WHERE est_polybag_id = @est_polybag_id

OPEN PackageCursor
FETCH NEXT FROM PackageCursor INTO @package_id

WHILE @@FETCH_STATUS = 0
BEGIN

	SELECT @oldsolomail = soloquantity FROM est_pakckage WHERE est_package_id = @package_id
	
	SET @newsolomail = @oldsolomail + @oldquantity - @quantity
	IF @newsolomail < 0 BEGIN SET @newsolomail = 0 END

	UPDATE est_package SET 
		soloquantity = ( @newsolomail ),
		modifiedby = @modifiedby,
		modifieddate = getdate()
		WHERE est_package_id = @package_id

	FETCH NEXT FROM PackageCursor INTO @package_id

END

CLOSE PackageCursor
DEALLOCATE PackageCursor

GO

GRANT  EXECUTE  ON [dbo].[EstPolybag_u]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO