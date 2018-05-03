IF OBJECT_ID('dbo.PolybagMailHouseLetterInsertionCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagMailHouseLetterInsertionCost'
	DROP FUNCTION dbo.PolybagMailHouseLetterInsertionCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagMailHouseLetterInsertionCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagMailHouseLetterInsertionCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagMailHouseLetterInsertionCost'
GO

create function dbo.PolybagMailHouseLetterInsertionCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag's total letter insertion cost.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_AssemDistribOptions         READ
*   EST_PackageComponentMapping     READ
*   EST_Package                     READ
*   EST_PackagePolybag_Map          READ
*   EST_Polybag                     READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Mail House letter insertion cost for polybag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
*
*/
begin
	declare @EST_Estimate_ID bigint, @EST_Component_ID bigint, @LetterInsertionCPM money, @SoloQuantity int, @PrimaryPolybagQuantity int, @retval money

	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)
	select @EST_Estimate_ID = EST_Estimate_ID
	from EST_Component
	where EST_Component_ID = @EST_Component_ID

	select @LetterInsertionCPM = mh.LetterInsertionRate
	from EST_Component c join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
		join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
	where c.EST_Component_ID = @EST_Component_ID and ad.UseLetterInsertion = 1

	select @SoloQuantity = sum(p.SoloQuantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	where pcm.EST_Component_ID = @EST_Component_ID

	select @PrimaryPolybagQuantity = dbo.EstimatePrimaryPolybagQuantity(@EST_Estimate_ID)

	select @retval = (@LetterInsertionCPM * pb.Quantity / 1000) * pb.Quantity / (isnull(@SoloQuantity, 0) + isnull(@PrimaryPolybagQuantity, 0))
	from EST_Polybag pb
	where pb.EST_Polybag_ID = @EST_Polybag_ID

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagMailHouseLetterInsertionCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
