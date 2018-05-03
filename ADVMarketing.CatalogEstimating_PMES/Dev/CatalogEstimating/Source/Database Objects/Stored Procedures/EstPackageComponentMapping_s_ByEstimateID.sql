IF OBJECT_ID('dbo.EstPackageComponentMapping_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPackageComponentMapping_s_ByEstimateID'
	DROP PROCEDURE dbo.EstPackageComponentMapping_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPackageComponentMapping_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstPackageComponentMapping_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstPackageComponentMapping_s_ByEstimateID'
GO

create proc dbo.EstPackageComponentMapping_s_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns all of the package-component mappings for an estimate.  This procedure is used by the Distribution Mapping Screen.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Package
*   EST_PackageComponentMapping
*
*
* PROCEDURES CALLED:
*
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
select pcm.EST_Package_ID, pcm.EST_Component_ID
from EST_Package p join EST_PackageComponentMapping pcm on p.EST_Package_ID = pcm.EST_Package_ID
where p.EST_Estimate_ID = @EST_Estimate_ID
order by pcm.EST_Package_ID, pcm.EST_Component_ID


GO

GRANT  EXECUTE  ON [dbo].[EstPackageComponentMapping_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
