IF OBJECT_ID('dbo.EstimatePostalDropFuelSurchargeCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimatePostalDropFuelSurchargeCost'
	DROP FUNCTION dbo.EstimatePostalDropFuelSurchargeCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimatePostalDropFuelSurchargeCost') IS NOT NULL
		PRINT '***********Drop of dbo.EstimatePostalDropFuelSurchargeCost FAILED.'
END
GO
PRINT 'Creating dbo.EstimatePostalDropFuelSurchargeCost'
GO

CREATE function dbo.EstimatePostalDropFuelSurchargeCost(@EST_Estimate_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Estimate_ID - An Estimate ID.
*
* DESCRIPTION:
*	1 - Calculates the Estimate's portion of the polybag postal drop fuel surcharge cost.
*	2 - Totals each of the polybag postal drop fuel surcharge costs.
*	3 - Calculates the solo mail postal drop fuel surcharge cost.
*	4 - Returns the total estimate postal drop fuel surcharge cost.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PackageComponentMapping
*
* PROCEDURES CALLED:
*	  dbo.PolybagPostalDropFuelSurchargeCost
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
* 07/24/2007	  JRH		  Get PostalDropCWT from VND_MailhouseRate
* 10/15/2007      BJS             SoloPostalDropCost -- divide total weight by 100
*
*/
as
begin
	declare @retval money
	
	select @retval = dbo.EstimatePostalDropCost(@EST_Estimate_ID) * isnull(MailFuelSurcharge, 0)
	from EST_AssemDistribOptions
	where EST_Estimate_ID = @EST_Estimate_ID
	
	return(isnull(@retval, 0))
end
GO

GRANT  EXECUTE  ON [dbo].[EstimatePostalDropFuelSurchargeCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
