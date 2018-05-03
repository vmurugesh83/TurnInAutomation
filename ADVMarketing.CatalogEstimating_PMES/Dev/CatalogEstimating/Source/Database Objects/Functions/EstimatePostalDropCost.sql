IF OBJECT_ID('dbo.EstimatePostalDropCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimatePostalDropCost'
	DROP FUNCTION dbo.EstimatePostalDropCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimatePostalDropCost') IS NOT NULL
		PRINT '***********Drop of dbo.EstimatePostalDropCost FAILED.'
END
GO
PRINT 'Creating dbo.EstimatePostalDropCost'
GO

CREATE function dbo.EstimatePostalDropCost(@EST_Estimate_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Estimate_ID - An Estimate ID.
*
* DESCRIPTION:
*	Calculates the Estimate's portion of the polybag postal drop cost.
*   Totals each of the polybag postal drop costs.
*   Calculates the solo mail postal drop cost.
*   Returns the total estimate postal drop cost.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PackageComponentMapping
*
* PROCEDURES CALLED:
*   dbo.PolybagPostalDropCost
*   dbo.PackageWeight
*   dbo.PolyBagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Estimate postal drop cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/18/2007      BJS             Initial Creation 
* 07/23/2007      BJS             Removed fuel surcharge calculation
* 07/24/2007	  JRH		      Get PostalDropCWT from VND_MailhouseRate
* 09/19/2007      BJS             Check for Postal Drop Flat Override
*/
as
begin
	declare
		@PolybagPostalDropCost money,
		@SoloPostalDropCost    money

	select @PolybagPostalDropCost = sum(dbo.PolybagPostalDropCost(pb.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolyBagWeight(pb.EST_Polybag_ID)))
	from EST_Package p join EST_PackagePolyBag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
		join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
	where p.EST_Estimate_ID = @EST_Estimate_ID

	if exists (select 1 from EST_AssemDistribOptions where EST_Estimate_ID = @EST_Estimate_ID and PostalDropFlat is not null) begin
		declare
			@PostalDropFlat              money,
			@SoloWeight                  decimal(14,6),
			@SoloAndPrimaryPolybagWeight decimal(14,6)

		select @PostalDropFlat = PostalDropFlat
		from EST_AssemDistribOptions
		where EST_Estimate_ID = @EST_Estimate_ID

		set @SoloAndPrimaryPolybagWeight = isnull(dbo.EstimateSoloAndPrimaryPolybagWeight(@EST_Estimate_ID), 0)

		select @SoloWeight = sum(isnull(p.SoloQuantity, 0) * (c.Width * c.Height) / cast(950000 as decimal) * c.PageCount * pw.Weight * 1.03)
		from EST_Package p join EST_PackageComponentMapping pcm on p.EST_Package_ID = pcm.EST_Package_ID
			join EST_Component c on pcm.EST_Component_ID = c.EST_Component_ID
			join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
		where p.EST_Estimate_ID = @EST_Estimate_ID

		if (@SoloAndPrimaryPolybagWeight = 0)
			set @SoloPostalDropCost = @PostalDropFlat
		else
			set @SoloPostalDropCost = @PostalDropFlat * @SoloWeight / @SoloAndPrimaryPolybagWeight

	end
	else begin
		select @SoloPostalDropCost = sum(
			isnull(p.SoloQuantity, 0)
				* isnull(dbo.PackageWeight(p.EST_Package_ID), 0)
				* mh.PostalDropCWT / 100)
		from EST_Estimate e 
			join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
			join EST_Package p on e.EST_Estimate_ID = p.EST_Estimate_ID
			join VND_MailhouseRate mh on ad.Mailhouse_ID = mh.VND_MailhouseRate_ID
		where e.EST_Estimate_ID = @EST_Estimate_ID
	end

	return (isnull(@PolybagPostalDropCost, 0) + isnull(@SoloPostalDropCost, 0))
end
GO

GRANT  EXECUTE  ON [dbo].[EstimatePostalDropCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

