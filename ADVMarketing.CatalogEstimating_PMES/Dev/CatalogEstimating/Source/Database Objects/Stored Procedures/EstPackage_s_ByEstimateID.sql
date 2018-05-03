IF OBJECT_ID('dbo.EstPackage_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPackage_s_ByEstimateID'
	DROP PROCEDURE dbo.EstPackage_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPackage_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstPackage_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstPackage_s_ByEstimateID'
GO

CREATE proc dbo.EstPackage_s_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns all of the packages for the estimate.  Used on the Distribution Mapping Screen.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Package         READ
*
*
* PROCEDURES CALLED:
*   dbo.PackageInsertQuantity
*   dbo.PackagePolybagQuantity
*
* DATABASE:
*		All
*
*
* RETURN VALUE:
* None 
*
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 05/25/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint
as
select p.EST_Package_ID, p.Description, dbo.PackageInsertQuantity(p.EST_Package_ID),
	p.SoloQuantity, dbo.PackagePolybagQuantity(p.EST_Package_ID) PolyQuantity,
	p.SoloQuantity + dbo.PackagePolybagQuantity(p.EST_Package_ID) as TotalMailQuantity, p.OtherQuantity,
	dbo.PackageInsertQuantity(p.EST_Package_ID) + p.SoloQuantity + dbo.PackagePolybagQuantity(p.EST_Package_ID) + p.OtherQuantity as TotalQuantity
from EST_Package p
where p.EST_Estimate_ID = @EST_Estimate_ID


GO

GRANT  EXECUTE  ON [dbo].[EstPackage_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
