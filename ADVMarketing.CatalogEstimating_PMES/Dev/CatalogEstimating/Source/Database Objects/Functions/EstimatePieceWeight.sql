IF OBJECT_ID('dbo.EstimatePieceWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimatePieceWeight'
	DROP FUNCTION dbo.EstimatePieceWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimatePieceWeight') IS NOT NULL
		PRINT '***********Drop of dbo.EstimatePieceWeight FAILED.'
END
GO
PRINT 'Creating dbo.EstimatePieceWeight'
GO

create function dbo.EstimatePieceWeight(@EST_Estimate_ID bigint)
returns decimal(12,6)
/*
* PARAMETERS:
* EST_Estimate_ID - Required.  The EstimateID.
*
*
* DESCRIPTION:
*		Returns the estimate piece weight.  (The total piece weight of each of its components).
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Component
*   PPR_PaperWeight
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
* 06/20/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
as
begin
	return(
		select sum((c.Width * c.Height) / cast(950000 as decimal) * c.PageCount * pw.Weight * 1.03)
		from EST_Component c join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
		where EST_Estimate_ID = @EST_Estimate_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[EstimatePieceWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
