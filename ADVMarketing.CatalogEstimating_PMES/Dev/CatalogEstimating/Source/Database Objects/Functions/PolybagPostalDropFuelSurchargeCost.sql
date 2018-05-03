IF OBJECT_ID('dbo.PolybagPostalDropFuelSurchargeCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagPostalDropFuelSurchargeCost'
	DROP FUNCTION dbo.PolybagPostalDropFuelSurchargeCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagPostalDropFuelSurchargeCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagPostalDropFuelSurchargeCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagPostalDropFuelSurchargeCost'
GO

create function dbo.PolybagPostalDropFuelSurchargeCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag's total postal drop fuel surcharge cost.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_AssemDistribOptions         READ
*
* PROCEDURES CALLED:
*   dbo.PolybagPrimaryHost
*   dbo.PolybagWeightIncludeBagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total postal drop fuel surcharge cost for polybag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/23/2007      BJS             Initial Creation 
* 07/24/2007      JRH             Get PostalDropCWT from VND_MailhouseRate
* 10/15/2007      BJS             References PolybagPostalDropCost
*
*/
begin
	declare @EST_Component_ID bigint, @PostalDropFuelSurcharge money, @retval money

	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)

	select @PostalDropFuelSurcharge = ad.MailFuelSurcharge
	from EST_Component c 
		join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
		join VND_MailhouseRate mh on ad.Mailhouse_ID = mh.VND_MailhouseRate_ID
	where c.EST_Component_ID = @EST_Component_ID

	select @retval = isnull(dbo.PolybagPostalDropCost(@EST_Polybag_ID), 0) * isnull(@PostalDropFuelSurcharge, 0)

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagPostalDropFuelSurchargeCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
