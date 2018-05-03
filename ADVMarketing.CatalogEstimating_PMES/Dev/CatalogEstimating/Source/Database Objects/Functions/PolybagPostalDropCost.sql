IF OBJECT_ID('dbo.PolybagPostalDropCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagPostalDropCost'
	DROP FUNCTION dbo.PolybagPostalDropCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagPostalDropCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagPostalDropCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagPostalDropCost'
GO

create function dbo.PolybagPostalDropCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag's total postal drop cost.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_AssemDistribOptions         READ
*
* PROCEDURES CALLED:
*   PolybagPrimaryHost
*   PolybagWeightIncludeBagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total postal drop cost for polybag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/18/2007      BJS             Initial Creation 
* 07/23/2007      BJS             Now references PolybagWeightIncludeBagWeight to include the weight
*                                   of the plastic bag.
*                                 Removed calculation for fuel surcharge
* 07/24/2007      JRH             Get PostalDropCWT from VND_MailhouseRate
* 10/15/2007      BJS             When using PostalDropCWT, calculate totalweight / 100 * rate
*
*/
begin
	declare
		@EST_Estimate_ID  bigint,
		@EST_Component_ID bigint,
		@Quantity         int,
		@PostalDropCost   money,
		@PostalDropCWT    money,
		@retval           money

	-- Get the values needed to perform the calculations
	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)

	select @EST_Estimate_ID = EST_Estimate_ID
	from EST_Component
	where EST_Component_ID = @EST_Component_ID

	select @Quantity = Quantity
	from EST_Polybag
	where EST_Polybag_ID = @EST_Polybag_ID

	-- The Primary Host is using a flat postal drop cost
	if exists(select 1 from EST_AssemDistribOptions where EST_Estimate_ID = @EST_Estimate_ID and PostalDropFlat is not null) begin
		select @PostalDropCost = PostalDropFlat
		from EST_AssemDistribOptions
		where EST_Estimate_ID = @EST_Estimate_ID

		set @retval = @PostalDropCost * dbo.PolybagWeightIncludeBagWeight(@EST_Polybag_ID) / dbo.EstimateSoloAndPrimaryPolybagWeight(@EST_Estimate_ID)
	end
	-- The Primary Host is using the Mailhouse Postal Drop rate
	else begin
		select @PostalDropCWT = mh.PostalDropCWT
		from EST_Component c 
			join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
			join VND_MailhouseRate mh on ad.Mailhouse_ID = mh.VND_MailhouseRate_ID
		where c.EST_Component_ID = @EST_Component_ID

		select @retval = isnull(@PostalDropCWT, 0) * dbo.PolybagWeightIncludeBagWeight(@EST_Polybag_ID) * @Quantity / 100
	end

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagPostalDropCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
