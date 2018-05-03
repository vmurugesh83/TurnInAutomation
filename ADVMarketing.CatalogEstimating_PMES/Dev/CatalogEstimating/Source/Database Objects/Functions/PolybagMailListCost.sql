IF OBJECT_ID('dbo.PolybagMailListCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagMailListCost'
	DROP FUNCTION dbo.PolybagMailListCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagMailListCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagMailListCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagMailListCost'
GO

create function dbo.PolybagMailListCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag's mail list cost.
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
*	Total Mail House time value slips cost for polybag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
*
*/
begin
	declare @retval money

	declare @EST_Estimate_ID bigint, @EST_Component_ID bigint,
		@TotalMailQuantity int, @ExternalMailQuantity int, @InternalMailQuantity int, @PrimaryMailQuantity int,
		@InternalMailCPM money, @ExternalMailCPM money, @BlendedMailListCPM money

	/* Determine the Primary Host Component */
	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)

	select @EST_Estimate_ID = EST_Estimate_ID
	from EST_Component
	where EST_Component_ID = @EST_Component_ID

	select @ExternalMailQuantity = ad.ExternalMailQty, @ExternalMailCPM = ad.ExternalMailCPM, @InternalMailCPM = isnull(ml.InternalListRate, 0)
	from EST_AssemDistribOptions ad
		left join VND_MailListResourceRate ml on ad.MailListResource_ID = ml.VND_MailListResourceRate_ID
	where ad.EST_Estimate_ID = @EST_Estimate_ID

	select @PrimaryMailQuantity = dbo.EstimateSoloAndPrimaryPolybagQuantity(@EST_Estimate_ID)
	select @InternalMailQuantity = @PrimaryMailQuantity - @ExternalMailQuantity

	/* Determine the Blended Rate */
	set @BlendedMailListCPM =
		case
			when @PrimaryMailQuantity = 0 then 0
			else @InternalMailCPM
					* cast(@InternalMailQuantity as decimal)
					/ cast(@PrimaryMailQuantity as decimal)
				+ @ExternalMailCPM
					* cast(@ExternalMailQuantity as decimal)
					/ cast(@PrimaryMailQuantity as decimal)
		end

	select @retval = @BlendedMailListCPM * Quantity / 1000
	from EST_Polybag
	where EST_Polybag_ID = @EST_Polybag_ID

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagMailListCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
