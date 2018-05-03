IF OBJECT_ID('dbo.ComponentOtherQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentOtherQuantity'
	DROP FUNCTION dbo.ComponentOtherQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentOtherQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentOtherQuantity FAILED.'
END
GO
PRINT 'Creating dbo.ComponentOtherQuantity'
GO

/* Component Other (sometimes referred to as Sample) Quantity */
create function dbo.ComponentOtherQuantity(@EST_Component_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Component_ID
*
* DESCRIPTION:
*	Returns the other quantity of a component.
*
* TABLES:
*   Table Name                    Access
*   ==========                    ======
*   EST_PackageComponentMapping   READ
*   EST_Package                   READ
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Component other quantity
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/10/2007      BJS             Initial Creation 
*
*/
as
begin
	return(select sum(p.OtherQuantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	where pcm.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentOtherQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
