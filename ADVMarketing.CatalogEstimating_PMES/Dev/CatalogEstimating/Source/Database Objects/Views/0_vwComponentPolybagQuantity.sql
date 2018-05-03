IF OBJECT_ID('dbo.vwComponentPolybagQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vwComponentPolybagQuantity'
	DROP VIEW dbo.vwComponentPolybagQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vwComponentPolybagQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.vwComponentPolybagQuantity FAILED.'
END
GO
PRINT 'Creating dbo.vwComponentPolybagQuantity'
GO

CREATE VIEW dbo.vwComponentPolybagQuantity
/*
* DESCRIPTION:
*		Returns ComponentID, EstimateID and Component Polybag Mail Quantity.
*
*
* TABLES:
*   Table Name                  Access
*   ==========                  ======
*   EST_PackageComponentMapping READ
*   EST_Package                 READ
*
* PROCEDURES CALLED:
*   None
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/13/2007      BJS             Initial Creation
* 05/20/2008      BJS             Added locking hints to improve performance
*/
AS
SELECT pcm.EST_Component_ID, max(p.EST_Estimate_ID) EST_Estimate_ID, sum(pb.Quantity) PolybagQuantity
FROM EST_PackageComponentMapping pcm (nolock) join EST_Package p (nolock) on pcm.EST_Package_ID = p.EST_Package_ID
	join EST_PackagePolyBag_Map ppbm (nolock) on p.EST_Package_ID = ppbm.EST_Package_ID
	join EST_Polybag pb (nolock) on ppbm.EST_PolyBag_ID = pb.EST_Polybag_ID
GROUP BY pcm.EST_Component_ID
GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vwComponentPolybagQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO