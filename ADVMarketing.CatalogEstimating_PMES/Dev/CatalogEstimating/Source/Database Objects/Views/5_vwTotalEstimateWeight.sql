IF OBJECT_ID('dbo.vwTotalEstimateWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vwTotalEstimateWeight'
	DROP VIEW dbo.vwTotalEstimateWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vwTotalEstimateWeight') IS NOT NULL
		PRINT '***********Drop of dbo.vwTotalEstimateWeight FAILED.'
END
GO
PRINT 'Creating dbo.vwTotalEstimateWeight'
GO

CREATE VIEW dbo.vwTotalEstimateWeight
/*
* DESCRIPTION:
*		Returns EstimateID and Total Estimate Weight.
*
*
* TABLES:
*   Table Name                Access
*   ==========                ======
*   EST_Component             READ
*   vwComponentWeight         READ
*   vwComponentMediaQuantity  READ
*   vwComponentOtherQuantity  READ
*   vwComponentSampleQuantity READ
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
as
SELECT c.EST_Estimate_ID, sum(cw.ComponentPieceWeight * (mq.MediaQuantity * (1 + isnull(c.SpoilagePct, 0))
	+ isnull(oq.OtherQuantity, 0) + isnull(sq.SampleQuantity, 0))) TotalEstimateWeight
FROM EST_Component c (nolock) join vwComponentWeight cw (nolock) on c.EST_Component_ID = cw.EST_Component_ID
	join vwComponentMediaQuantity mq (nolock) on c.EST_Component_ID = mq.EST_Component_ID
	left join vwComponentOtherQuantity oq (nolock) on c.EST_Component_ID = oq.EST_Component_ID
	left join vwComponentSampleQuantity sq (nolock) on c.EST_Component_ID = sq.EST_Component_ID
GROUP BY c.EST_Estimate_ID
GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vwTotalEstimateWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO