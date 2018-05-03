IF OBJECT_ID('dbo.PprPaperGrade_s_Grade_ByPaperIDandPaperWeightID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperGrade_s_Grade_ByPaperIDandPaperWeightID'
	DROP PROCEDURE dbo.PprPaperGrade_s_Grade_ByPaperIDandPaperWeightID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperGrade_s_Grade_ByPaperIDandPaperWeightID') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperGrade_s_Grade_ByPaperIDandPaperWeightID FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperGrade_s_Grade_ByPaperIDandPaperWeightID'
GO

create proc dbo.PprPaperGrade_s_Grade_ByPaperIDandPaperWeightID
/*
* PARAMETERS:
* VND_Paper_ID - Required.  The PaperID.
*	VND_PaperWeight_ID - Required.  The PaperWeightID.
*
*
* DESCRIPTION:
*		Returns the Paper Grades for the VND_Paper_ID and VND_PaperWeight_ID.
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		PPR_Paper_Map
*		PPR_PaperGrade
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
@VND_Paper_ID bigint,
@PPR_PaperWeight_ID int
as

select pg.PPR_PaperGrade_ID, pg.Grade
from PPR_Paper_Map pm join PPR_PaperGrade pg on pm.PPR_PaperGrade_ID = pg.PPR_PaperGrade_ID
where pm.VND_Paper_ID = @VND_Paper_ID and pm.PPR_PaperWeight_ID = @PPR_PaperWeight_ID
order by pg.Grade


GO

GRANT  EXECUTE  ON [dbo].[PprPaperGrade_s_Grade_ByPaperIDandPaperWeightID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
