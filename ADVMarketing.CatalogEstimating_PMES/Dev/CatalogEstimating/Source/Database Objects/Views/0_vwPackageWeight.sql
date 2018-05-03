IF OBJECT_ID('dbo.vwPackageWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vwPackageWeight'
	DROP VIEW dbo.vwPackageWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vwPackageWeight') IS NOT NULL
		PRINT '***********Drop of dbo.vwPackageWeight FAILED.'
END
GO
PRINT 'Creating dbo.vwPackageWeight'
GO

CREATE VIEW dbo.vwPackageWeight
/*
* DESCRIPTION:
*		Returns ComponentID, EstimateID and Component Weight.
*
*
* TABLES:
*   Table Name                  Access
*   ==========                  ======
*   EST_Component               READ
*   PPR_PaperWeight             READ
*
* PROCEDURES CALLED:
*   None
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 11/19/2007      JRH             Initial Creation
* 05/20/2008      BJS             Added locking hints to improve performance
*
*/
AS
SELECT 
	EST_Package_ID
	, dbo.PackageWeight(EST_Package_ID) PackageWeight
FROM EST_Package (nolock)
GROUP BY EST_Package_ID
GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vwPackageWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO