IF OBJECT_ID('dbo.ComponentSoloMailQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentSoloMailQuantity'
	DROP FUNCTION dbo.ComponentSoloMailQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentSoloMailQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentSoloMailQuantity FAILED.'
END
GO
PRINT 'Creating dbo.ComponentSoloMailQuantity'
GO

/* Component Solo Mail Quantity */
create function dbo.ComponentSoloMailQuantity(@EST_Component_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Component_ID
*
* DESCRIPTION:
*	Returns the solo quantity of a component.
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
*	Component solo mail quantity
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
	return(select sum(p.SoloQuantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	where pcm.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentSoloMailQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
