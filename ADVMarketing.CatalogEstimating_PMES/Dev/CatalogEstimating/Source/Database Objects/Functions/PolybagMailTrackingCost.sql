IF OBJECT_ID('dbo.PolybagMailTrackingCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagMailTrackingCost'
	DROP FUNCTION dbo.PolybagMailTrackingCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagMailTrackingCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagMailTrackingCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagMailTrackingCost'
GO

create function dbo.PolybagMailTrackingCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag's total mail tracking cost.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_AssemDistribOptions         READ
*   VND_MailTrackingRate            READ
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
*	Total mail tracking cost for polybag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
* 11/12/2007      JRH             Multiply @MailTrackingCPM by total PB Qty.
*
*/
begin
	declare @EST_Estimate_ID bigint, @EST_Component_ID bigint, @MailTrackingCPM money, @PrimaryPolybagQuantity int, @retval money

	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)
	select @EST_Estimate_ID = EST_Estimate_ID
	from EST_Component
	where EST_Component_ID = @EST_Component_ID

	select @MailTrackingCPM = mr.MailTracking
	from EST_Component c join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
		join VND_MailTrackingRate mr on ad.MailTracking_ID = mr.VND_MailTrackingRate_ID
	where c.EST_Component_ID = @EST_Component_ID and ad.UseMailTracking = 1

	select @PrimaryPolybagQuantity = isnull(dbo.EstimatePrimaryPolybagQuantity(@EST_Estimate_ID), 0)


	select @retval = case when @PrimaryPolybagQuantity = 0 then 0
				else (@MailTrackingCPM * cast(@PrimaryPolybagQuantity as decimal(18,6)) / cast(1000 as decimal(18,6))) * pb.Quantity / @PrimaryPolybagQuantity
			end
	from EST_Polybag pb
	where pb.EST_Polybag_ID = @EST_Polybag_ID

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagMailTrackingCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
