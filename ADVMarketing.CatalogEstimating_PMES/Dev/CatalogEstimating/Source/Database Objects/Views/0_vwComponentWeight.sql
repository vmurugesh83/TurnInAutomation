IF OBJECT_ID('dbo.vwComponentWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vwComponentWeight'
	DROP VIEW dbo.vwComponentWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vwComponentWeight') IS NOT NULL
		PRINT '***********Drop of dbo.vwComponentWeight FAILED.'
END
GO
PRINT 'Creating dbo.vwComponentWeight'
GO

CREATE VIEW dbo.vwComponentWeight
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
* 09/13/2007      BJS             Initial Creation
* 10/15/2007      BJS             Added additional precision to the ComponentPieceWeight
* 05/20/2008      BJS             Added locking hints... to improve performance
*
*/
AS
SELECT c.EST_Component_ID, max(c.EST_Estimate_ID) EST_Esttimate_ID,
	cast(max(c.Width) * max(c.Height) / cast(950000 as decimal) * max(c.PageCount) * max(pw.Weight) * 1.03 as decimal(12,6)) as ComponentPieceWeight
FROM EST_Component c (nolock) join PPR_PaperWeight pw (nolock) on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
GROUP BY c.EST_Component_ID
GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vwComponentWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO