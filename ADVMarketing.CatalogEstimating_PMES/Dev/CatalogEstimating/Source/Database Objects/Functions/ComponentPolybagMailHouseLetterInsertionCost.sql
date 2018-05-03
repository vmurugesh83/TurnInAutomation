IF OBJECT_ID('dbo.ComponentPolybagMailHouseLetterInsertionCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentPolybagMailHouseLetterInsertionCost'
	DROP FUNCTION dbo.ComponentPolybagMailHouseLetterInsertionCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentPolybagMailHouseLetterInsertionCost') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentPolybagMailHouseLetterInsertionCost FAILED.'
END
GO
PRINT 'Creating dbo.ComponentPolybagMailHouseLetterInsertionCost'
GO

CREATE function dbo.ComponentPolybagMailHouseLetterInsertionCost(@EST_Component_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID - A Host Component ID
*
* DESCRIPTION:
*	Calculates the Component's portion of the polybag LetterInsertion cost.
*   Totals each of the costs and returns it as the Total Component Polybag LetterInsertion cost.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   EST_PackageComponentMapping  READ
*   EST_Component                READ
*   EST_Package                  READ
*   EST_PackagePolybag_Map       READ
*   EST_Polybag                  READ
*
* PROCEDURES CALLED:
*	dbo.PolybagMailHouseLetterInsertionCost
*   dbo.PackageWeight
*   dbo.PolybagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Component Polybag LetterInsertion cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
* 10/15/2007      BJS             Removed multiply by quantity, it is already handled in the PolybagMailHouseLetterInsertionCost function
*
*/
as
begin
	return(select sum(dbo.PolybagMailHouseLetterInsertionCost(pb.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolybagWeight(pb.EST_Polybag_ID)))
		from EST_Component c join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
			join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
			join EST_PackagePolybag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
			join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
		where c.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentPolybagMailHouseLetterInsertionCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO