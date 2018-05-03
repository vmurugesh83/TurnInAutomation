IF OBJECT_ID('dbo.vwComponentSoloQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vwComponentSoloQuantity'
	DROP VIEW dbo.vwComponentSoloQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vwComponentSoloQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.vwComponentSoloQuantity FAILED.'
END
GO
PRINT 'Creating dbo.vwComponentSoloQuantity'
GO

CREATE VIEW dbo.vwComponentSoloQuantity
/*
* DESCRIPTION:
*		Returns ComponentID, EstimateID and Component Solo Mail Quantity.
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
SELECT pcm.EST_Component_ID, max(p.EST_Estimate_ID) EST_Estimate_ID, sum(p.SoloQuantity) SoloQuantity
FROM EST_PackageComponentMapping pcm (nolock) join EST_Package p (nolock) on pcm.EST_Package_ID = p.EST_Package_ID
GROUP BY pcm.EST_Component_ID
GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vwComponentSoloQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO