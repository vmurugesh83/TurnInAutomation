IF OBJECT_ID('dbo.PackageSize') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PackageSize'
	DROP FUNCTION dbo.PackageSize
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PackageSize') IS NOT NULL
		PRINT '***********Drop of dbo.PackageSize FAILED.'
END
GO
PRINT 'Creating dbo.PackageSize'
GO

create function dbo.PackageSize(@EST_Package_ID bigint)
returns decimal(10,4)
/*
* PARAMETERS:
*	EST_Package_ID
*
* DESCRIPTION:
*	Calculates the size of a package
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   EST_Component                READ
*   EST_Package                  READ
*
* PROCEDURES CALLED:
*   None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	The package size
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation
* 10/12/2007      BJS             Added logic to handle packages w/o a Host Component 
*
*/
as
begin
	declare @retval decimal(10,4)
	
	-- Package Size is the size of the host component
	select @retval = c.Height * c.Width
	from EST_Component c join EST_Package p on c.EST_Estimate_ID = p.EST_Estimate_ID
	where p.EST_Package_ID = @EST_Package_ID and c.EST_ComponentType_ID = 1
	
	-- If there is no host component in the package, return the size of the first component
	if (@retval is null) begin
		select @retval = c.Height * c.Width
		from EST_Component c join EST_Package p on c.EST_Estimate_ID = p.EST_Estimate_ID
		where p.EST_Package_ID = @EST_Package_ID and c.EST_ComponentType_ID = 1
	end	
			
	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PackageSize]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
