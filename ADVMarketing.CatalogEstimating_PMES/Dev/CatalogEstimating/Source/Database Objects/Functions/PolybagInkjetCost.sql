IF OBJECT_ID('dbo.PolybagInkjetCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagInkjetCost'
	DROP FUNCTION dbo.PolybagInkjetCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagInkjetCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagInkjetCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagInkjetCost'
GO

create function dbo.PolybagInkjetCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag's total inkjet cost.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_AssemDistribOptions         READ
*   VND_MailHouseRate               READ
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
*	Total inkjet cost for polybag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/18/2007      BJS             Initial Creation 
*
*/
begin
	declare @EST_Estimate_ID bigint, @EST_Component_ID bigint, @InkjetCPM money, @SoloQuantity int, @PrimaryPolybagQuantity int, @retval money

	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)
	select @EST_Estimate_ID = EST_Estimate_ID
	from EST_Component
	where EST_Component_ID = @EST_Component_ID

	select @InkjetCPM = mh.InkjetRate
	from EST_Component c join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
		join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
	where c.EST_Component_ID = @EST_Component_ID

	select @SoloQuantity = sum(p.SoloQuantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	where pcm.EST_Component_ID = @EST_Component_ID

	select @PrimaryPolybagQuantity = dbo.EstimatePrimaryPolybagQuantity(@EST_Estimate_ID)

	select @retval = (@InkjetCPM * pb.Quantity / 1000) * pb.Quantity / (isnull(@SoloQuantity, 0) + isnull(@PrimaryPolybagQuantity, 0))
	from EST_Polybag pb
	where pb.EST_Polybag_ID = @EST_Polybag_ID

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagInkjetCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
