IF OBJECT_ID('dbo.EstPackage_s_ForEstimateCopy') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPackage_s_ForEstimateCopy'
	DROP PROCEDURE dbo.EstPackage_s_ForEstimateCopy
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPackage_s_ForEstimateCopy') IS NOT NULL
		PRINT '***********Drop of dbo.EstPackage_s_ForEstimateCopy FAILED.'
END
GO
PRINT 'Creating dbo.EstPackage_s_ForEstimateCopy'
GO

create proc dbo.EstPackage_s_ForEstimateCopy
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns an estimate's packages to be used during an Estimate Copy.  Moves any polybag quantities to solo quantity.
*
*
* TABLES:
*   Table Name              Access
*   ==========              ======
*   EST_Package             READ
*   EST_PackagePolybag_Map  READ
*   EST_Polybag             READ
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
* 09/04/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint
as

select p.EST_Package_ID, max(p.Description) Description, max(p.Comments) Comments, max(p.SoloQuantity) + isnull(sum(pb.Quantity), 0) SoloQuantity,
	max(p.OtherQuantity) OtherQuantity, max(p.PUB_PubQuantityType_ID) PUB_PubQuantityType_ID, max(p.PUB_PubGroup_ID) PUB_PubGroup_ID
from EST_Package p
	left join EST_PackagePolybag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
	left join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
where p.EST_Estimate_ID = @EST_Estimate_ID
group by p.EST_Package_ID
GO

GRANT  EXECUTE  ON [dbo].[EstPackage_s_ForEstimateCopy]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
