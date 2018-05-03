IF OBJECT_ID('dbo.PackageInsertIndex') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PackageInsertIndex'
	DROP FUNCTION dbo.PackageInsertIndex
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PackageInsertIndex') IS NOT NULL
		PRINT '***********Drop of dbo.PackageInsertIndex FAILED.'
END
GO
PRINT 'Creating dbo.PackageInsertIndex'
GO

create function dbo.PackageInsertIndex(@EST_Package_ID bigint, @PubRate_Map_ID bigint, @InsertDate datetime)
/*
* PARAMETERS:
*	EST_Package_ID
*   PubRate_Map_ID
*	InsertDate
*
* DESCRIPTION:
*	Returns the order an insert is placed into a package, by weight, with the heaviest first and the lightest last.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   est_estimate			     READ
*	pub_pubgroup				 READ
*	pub_pubpubgroup_map			 READ
*	est_pubissuedates			 READ
*
* PROCEDURES CALLED:
*   None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	int - 0 for the heaviest and 1 for the lightest
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/07/2007      BJS             Initial Creation
* 11/19/2007      JRH             Performance improvements.
* 12/06/2007      JRH             Use vpw.PackageWeight to compare to @MyPackageWeight.
*
*/
returns int
as
begin

	declare @MyPackageWeight decimal(12,6)
	select @MyPackageWeight = dbo.PackageWeight(@EST_Package_ID)

	return(select count(*) + 1 -- Because the discount table is 1 based.
		from EST_Estimate e 
			join EST_Package p  
				on e.EST_Estimate_ID = p.EST_Estimate_ID  
				and e.EST_Status_ID = 1
			join dbo.vwPackageWeight vpw  
				on p.EST_Package_ID = vpw.EST_Package_ID
			join PUB_PubGroup pg  
				on p.PUB_PubGroup_ID = pg.PUB_PubGroup_ID
			join PUB_PubPubGroup_Map ppgm  
				on pg.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID  
				and ppgm.PUB_PubRate_Map_ID = @PubRate_Map_ID
			join EST_PubIssueDates pid  
				on e.EST_Estimate_ID = pid.EST_Estimate_ID  
				and ppgm.PUB_PubRate_Map_ID = pid.PUB_PubRate_Map_ID  
				and pid.IssueDate = @InsertDate
		/* Two packageweight comparisons needed in case separate packages have identical weights. */
		where p.EST_Package_ID <> @EST_Package_ID and
			(vpw.PackageWeight > @MyPackageWeight  
				or (vpw.PackageWeight = @MyPackageWeight and p.EST_Package_ID < @EST_Package_ID)))

end
GO

GRANT  EXECUTE  ON [dbo].[PackageInsertIndex]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
