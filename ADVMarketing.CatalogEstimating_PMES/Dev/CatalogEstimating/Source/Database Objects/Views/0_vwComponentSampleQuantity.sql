IF OBJECT_ID('dbo.vwComponentSampleQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vwComponentSampleQuantity'
	DROP VIEW dbo.vwComponentSampleQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vwComponentSampleQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.vwComponentSampleQuantity FAILED.'
END
GO
PRINT 'Creating dbo.vwComponentSampleQuantity'
GO

CREATE VIEW dbo.vwComponentSampleQuantity
/*
* DESCRIPTION:
*		Returns ComponentID, EstimateID and Component Sample Quantity.
*
*
* TABLES:
*   Table Name                  Access
*   ==========                  ======
*   EST_Component               READ
*   EST_Samples                 READ
*
* PROCEDURES CALLED:
*   None
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/13/2007      BJS             Initial Creation
* 11/26/2007      JRH             Only for Host
* 05/20/2008      BJS             Added locking hints to improve performance
*
*/
AS
SELECT
	c.EST_Component_ID, 
	c.EST_Estimate_ID, 
	case c.est_componenttype_id
		when 1 then s.Quantity
		else 0
	end SampleQuantity
FROM EST_Component c (nolock) join EST_Samples s (nolock) on c.EST_Estimate_ID = s.EST_Estimate_ID

GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vwComponentSampleQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO