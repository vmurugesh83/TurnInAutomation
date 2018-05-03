IF OBJECT_ID('dbo.IsEstimateUsingPostalVendor') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.IsEstimateUsingPostalVendor'
	DROP FUNCTION dbo.IsEstimateUsingPostalVendor
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.IsEstimateUsingPostalVendor') IS NOT NULL
		PRINT '***********Drop of dbo.IsEstimateUsingPostalVendor FAILED.'
END
GO
PRINT 'Creating dbo.IsEstimateUsingPostalVendor'
GO

CREATE FUNCTION dbo.IsEstimateUsingPostalVendor(@EST_Estimate_ID bigint, @VND_Vendor_ID bigint)
returns bit
/*
* PARAMETERS:
*	EST_Estimate_ID
*   VND_Vendor_ID
*
* DESCRIPTION:
*	Determines whether the Estimate utilizes the postal vendor.
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   EST_AssemDistribOptions        READ
*   PST_PostalScenario             READ
*   PST_PostalCategoryScenario_Map READ
*   PST_PostalCategoryRate_Map     READ
*   PST_PostalWeights              READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	0 - Estimate does not utilize the specified postal vendor
*   1 - Estimate does utilize the specified postal vendor
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/25/2007      BJS             Initial Creation 
*
*/
as
begin
	declare @retval bit
	if exists(
			select 1
			from EST_AssemDistribOptions ad join PST_PostalScenario ps on ad.PST_PostalScenario_ID = ps.PST_PostalScenario_ID
				join PST_PostalCategoryScenario_Map pcsm on ps.PST_PostalScenario_ID = pcsm.PST_PostalScenario_ID
				join PST_PostalCategoryRate_Map pcrm on pcsm.PST_PostalCategoryRate_Map_ID = pcrm.PST_PostalCategoryRate_Map_ID
				join PST_PostalWeights pw on pcrm.PST_PostalWeights_ID = pw.PST_PostalWeights_ID
			where ad.EST_Estimate_ID = @EST_Estimate_ID and pw.VND_Vendor_ID = @VND_Vendor_ID)
		set @retval = 1
	else
		set @retval = 0

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[IsEstimateUsingPostalVendor]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO