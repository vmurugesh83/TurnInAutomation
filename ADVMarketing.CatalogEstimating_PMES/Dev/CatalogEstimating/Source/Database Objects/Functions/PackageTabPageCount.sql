IF OBJECT_ID('dbo.PackageTabPageCount') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PackageTabPageCount'
	DROP FUNCTION dbo.PackageTabPageCount
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PackageTabPageCount') IS NOT NULL
		PRINT '***********Drop of dbo.PackageTabPageCount FAILED.'
END
GO
PRINT 'Creating dbo.PackageTabPageCount'
GO

create function dbo.PackageTabPageCount(@EST_Package_ID bigint, @BlowInRate int)
returns int
/*
* PARAMETERS:
*	EST_Package_ID
*   PUB_PubRate_ID
*
* DESCRIPTION:
*	Determines the tab page count of the package and calculates blow-in page counts according to the BlowInRate.
*
* TABLES:
*   Table Name              Access
*   ==========              ======
*   EST_Component           READ
*   EST_Package             READ
*
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	The package tab page count.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/15/2007      BJS             Initial Creation
*
*/
as
begin
	return(select sum(
			case
				when c.EST_EstimateMediaType_ID = 2 then 2 -- Broadsheet
				else 1
			end
			*
			case
				when c.EST_ComponentType_ID = 4 and @BlowInRate = 0 then 0 --Blow-In not charged
				when c.EST_ComponentType_ID = 4 and @BlowInRate = 1 then cast(c.PageCount as decimal) / 2 -- Blow-In charged at 1/2 page
				else c.PageCount
			end)
	from EST_PackageComponentMapping pcm join EST_Component c on pcm.EST_Component_ID = c.EST_Component_ID
	where pcm.EST_Package_ID = @EST_Package_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[PackageTabPageCount]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
