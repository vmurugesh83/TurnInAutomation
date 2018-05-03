IF OBJECT_ID('dbo.ComponentSampleQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentSampleQuantity'
	DROP FUNCTION dbo.ComponentSampleQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentSampleQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentSampleQuantity FAILED.'
END
GO
PRINT 'Creating dbo.ComponentSampleQuantity'
GO

/* Component Sample Quantity */
create function dbo.ComponentSampleQuantity(@EST_Component_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Component_ID
*
* DESCRIPTION:
*	Returns the sample quantity of a component.
*
* TABLES:
*   Table Name                    Access
*   ==========                    ======
*   EST_Component                 READ
*   EST_Samples                   READ
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Component sample quantity
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/10/2007      BJS             Initial Creation 
* 11/26/2007      JRH             For Host components only
*
*/
as
begin
	return(select sum(s.Quantity)
	from EST_Component c join EST_Samples s on c.EST_Estimate_ID = s.EST_Estimate_ID
	where c.EST_Component_ID = @EST_Component_ID
		and c.EST_ComponentType_ID = 1)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentSampleQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
