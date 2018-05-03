IF OBJECT_ID('dbo.PolybagMailHouseOtherDirectMailHandlingCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagMailHouseOtherDirectMailHandlingCost'
	DROP FUNCTION dbo.PolybagMailHouseOtherDirectMailHandlingCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagMailHouseOtherDirectMailHandlingCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagMailHouseOtherDirectMailHandlingCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagMailHouseOtherDirectMailHandlingCost'
GO

create function dbo.PolybagMailHouseOtherDirectMailHandlingCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag's total other direct mail handling cost.
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
*	Total Mail House other direct mail handling cost for polybag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
*
*/
begin
	declare @EST_Estimate_ID bigint, @EST_Component_ID bigint, @MailHouseOtherDirectMailHandlingCost money, @SoloQuantity int, @PrimaryPolybagQuantity int, @retval money

	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)
	select @EST_Estimate_ID = EST_Estimate_ID
	from EST_Component
	where EST_Component_ID = @EST_Component_ID

	select @MailHouseOtherDirectMailHandlingCost = ad.MailHouseOtherHandling
	from EST_Component c join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
	where c.EST_Component_ID = @EST_Component_ID

	select @SoloQuantity = sum(p.SoloQuantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	where pcm.EST_Component_ID = @EST_Component_ID

	select @PrimaryPolybagQuantity = dbo.EstimatePrimaryPolybagQuantity(@EST_Estimate_ID)

	select @retval = (@MailHouseOtherDirectMailHandlingCost * pb.Quantity / 1000) * pb.Quantity / (isnull(@SoloQuantity, 0) + isnull(@PrimaryPolybagQuantity, 0))
	from EST_Polybag pb
	where pb.EST_Polybag_ID = @EST_Polybag_ID

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagMailHouseOtherDirectMailHandlingCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO