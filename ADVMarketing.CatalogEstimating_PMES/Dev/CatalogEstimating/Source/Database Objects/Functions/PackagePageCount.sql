IF OBJECT_ID('dbo.PackagePageCount') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PackagePageCount'
	DROP FUNCTION dbo.PackagePageCount
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PackagePageCount') IS NOT NULL
		PRINT '***********Drop of dbo.PackagePageCount FAILED.'
END
GO
PRINT 'Creating dbo.PackagePageCount'
GO

create function dbo.PackagePageCount(@EST_Package_ID bigint)
/*
* PARAMETERS:
*	EST_Package_iD
*
* DESCRIPTION:
*	Returns the total number of pages for all components in a package.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   est_component			     READ
*	est_packagecomponentmapping  READ
*
* PROCEDURES CALLED:
*   None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	int - Number of pages
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/07/2007      BJS             Initial Creation
*
*/
returns int
as
begin
	return(select sum(c.PageCount)
		from EST_Component c join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
		where pcm.EST_Package_ID = @EST_Package_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[PackagePageCount]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
