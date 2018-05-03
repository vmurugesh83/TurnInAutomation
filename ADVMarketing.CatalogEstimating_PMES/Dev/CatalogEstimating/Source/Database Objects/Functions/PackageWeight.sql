IF OBJECT_ID('dbo.PackageWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PackageWeight'
	DROP FUNCTION dbo.PackageWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PackageWeight') IS NOT NULL
		PRINT '***********Drop of dbo.PackageWeight FAILED.'
END
GO
PRINT 'Creating dbo.PackageWeight'
GO

create function dbo.PackageWeight(@EST_Package_ID bigint)
/*
* PARAMETERS:
*	EST_Package_ID
*
* DESCRIPTION:
*	Returns the total weight of all the components in a package.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   est_component			     READ
*	est_packagecomponentmapping	 READ
*	ppr_paperweight				 READ
*
* PROCEDURES CALLED:
*   None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	decimal(12,6) - Package Weight
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/07/2007      BJS             Initial Creation
*
*/
returns decimal(12,6)
as
begin
	return(select sum(c.Width * c.Height / cast(950000 as decimal) * c.PageCount * pw.Weight * 1.03)
		from EST_Component c join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
			join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
		where pcm.EST_Package_ID = @EST_Package_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[PackageWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
