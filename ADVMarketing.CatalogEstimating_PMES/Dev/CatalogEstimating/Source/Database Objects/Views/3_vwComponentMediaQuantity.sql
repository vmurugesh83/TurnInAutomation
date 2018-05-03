IF OBJECT_ID('dbo.vwComponentMediaQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vwComponentMediaQuantity'
	DROP VIEW dbo.vwComponentMediaQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vwComponentMediaQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.vwComponentMediaQuantity FAILED.'
END
GO
PRINT 'Creating dbo.vwComponentMediaQuantity'
GO

CREATE VIEW dbo.vwComponentMediaQuantity
/*
* DESCRIPTION:
*		Returns ComponentID, EstimateID and Component Media Quantity.
*
*
* TABLES:
*   Table Name                 Access
*   ==========                 ======
*   EST_Component              READ
*   vwComponentInsertQuantity  READ
*   vwComponentSoloQuantity    READ
*   vwComponentPolybagQuantity READ
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

SELECT c.EST_Component_ID, max(c.EST_Estimate_ID) EST_Estimate_ID,
	isnull(max(iq.InsertQuantity), 0) + isnull(max(sq.SoloQuantity), 0) + isnull(max(pq.PolybagQuantity), 0) as MediaQuantity
FROM EST_Component c (nolock) left join vwComponentInsertQuantity iq (nolock) on c.EST_Component_ID = iq.EST_Component_ID
	left join vwComponentSoloQuantity sq (nolock) on c.EST_Component_ID = sq.EST_Component_ID
	left join vwComponentPolybagQuantity pq (nolock) on c.EST_Component_ID = pq.EST_Component_ID
GROUP BY c.EST_Component_ID
GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vwComponentMediaQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO