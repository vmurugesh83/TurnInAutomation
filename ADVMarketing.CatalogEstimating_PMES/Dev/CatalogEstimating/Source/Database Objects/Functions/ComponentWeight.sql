IF OBJECT_ID('dbo.ComponentWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentWeight'
	DROP FUNCTION dbo.ComponentWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentWeight') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentWeight FAILED.'
END
GO
PRINT 'Creating dbo.ComponentWeight'
GO

create function dbo.ComponentWeight(@EST_Component_ID bigint)
returns decimal(12,6)
/*
* PARAMETERS:
*	EST_Component_ID - A Component ID
*
* DESCRIPTION:
*	Calculates the Component's Piece Weight
*
* TABLES:
*   Table Name                    Access
*   ==========                    ======
*   EST_Component                 READ
*   PPR_PaperWeight               READ
*
* PROCEDURES CALLED:
*   None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Component Piece Weight
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/05/2007      BJS             Initial Creation 
*
*/
as
begin
	return(select c.Width * c.Height / cast(950000 as decimal) * c.PageCount * pw.Weight * 1.03
	from EST_Component c join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
	where c.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
