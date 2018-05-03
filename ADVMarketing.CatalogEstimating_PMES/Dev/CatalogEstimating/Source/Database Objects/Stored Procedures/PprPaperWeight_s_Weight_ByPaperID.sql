IF OBJECT_ID('dbo.PprPaperWeight_s_Weight_ByPaperID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperWeight_s_Weight_ByPaperID'
	DROP PROCEDURE dbo.PprPaperWeight_s_Weight_ByPaperID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperWeight_s_Weight_ByPaperID') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperWeight_s_Weight_ByPaperID FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperWeight_s_Weight_ByPaperID'
GO

create proc dbo.PprPaperWeight_s_Weight_ByPaperID
/*
* PARAMETERS:
* VND_Paper_ID - Required.  The PaperID.
*
*
* DESCRIPTION:
*		Returns the Paper Weights for the VND_Paper_ID.
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		PPR_Paper_Map
*		PPR_PaperWeight
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
*	Date						Who							Comments
*	------------- 	--------        -------------------------------------------------
* 05/23/2007			BJS							Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Paper_ID bigint
as

select pw.PPR_PaperWeight_ID, pw.Weight
from PPR_Paper_Map pm join PPR_PaperWeight pw on pm.PPR_PaperWeight_ID = pw.PPR_PaperWeight_ID
where pm.VND_Paper_ID = @VND_Paper_ID
order by pw.Weight


GO

GRANT  EXECUTE  ON [dbo].[PprPaperWeight_s_Weight_ByPaperID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
