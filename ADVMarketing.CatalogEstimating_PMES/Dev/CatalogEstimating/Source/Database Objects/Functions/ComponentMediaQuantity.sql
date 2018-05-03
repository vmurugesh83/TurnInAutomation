IF OBJECT_ID('dbo.ComponentMediaQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentMediaQuantity'
	DROP FUNCTION dbo.ComponentMediaQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentMediaQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentMediaQuantity FAILED.'
END
GO
PRINT 'Creating dbo.ComponentMediaQuantity'
GO

create function dbo.ComponentMediaQuantity(@EST_Component_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Component_ID - The Component ID
*
* DESCRIPTION:
*	Returns the total media quantity for the specified component
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*
* PROCEDURES CALLED:
*   dbo.ComponentInsertQuantity
*   dbo.ComponentSoloMailQuantity
*   dbo.ComponentPolyBagQuantity
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total media quantity for the specified component.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/23/2007      BJS             Initial Creation 
*
*/
as
begin
	return isnull(dbo.ComponentInsertQuantity(@EST_Component_ID), 0)
		+ isnull(dbo.ComponentSoloMailQuantity(@EST_Component_ID), 0)
		+ isnull(dbo.ComponentPolyBagQuantity(@EST_Component_ID), 0)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentMediaQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
