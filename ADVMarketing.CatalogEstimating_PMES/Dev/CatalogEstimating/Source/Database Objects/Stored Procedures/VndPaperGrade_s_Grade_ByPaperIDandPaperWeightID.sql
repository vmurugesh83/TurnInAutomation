IF OBJECT_ID('dbo.VndPaperGrade_s_Grade_ByPaperIDandPaperWeightID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndPaperGrade_s_Grade_ByPaperIDandPaperWeightID'
	DROP PROCEDURE dbo.VndPaperGrade_s_Grade_ByPaperIDandPaperWeightID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndPaperGrade_s_Grade_ByPaperIDandPaperWeightID') IS NOT NULL
		PRINT '***********Drop of dbo.VndPaperGrade_s_Grade_ByPaperIDandPaperWeightID FAILED.'
END
GO
PRINT 'Creating dbo.VndPaperGrade_s_Grade_ByPaperIDandPaperWeightID'
GO

create proc dbo.VndPaperGrade_s_Grade_ByPaperIDandPaperWeightID
/*
* PARAMETERS:
* VND_Paper_ID - Required.  The Paper ID that is associated with the PPR_PaperWeight_ID.
* PPR_PaperWeight_ID - Required.  The component must have a Paper Weight in order to determine which Paper Grades are available.
* PPR_PaperGrade_ID - Optional.  If the component already references a PPR_PaperGrade record we need to make sure we return it.
*
*
* DESCRIPTION:
*		Returns a list of Paper Grades for the specified Paper Weight and VND_Paper_ID.
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
@PPR_PaperWeight_ID int,
@PPR_PaperGrade_ID int
as

select pg.PPR_PaperGrade_ID, pg.Grade
from PPR_Paper_Map pm	join PPR_PaperGrade pg on pm.PPR_PaperGrade_ID = pg.PPR_PaperGrade_ID
where pm.VND_Paper_ID = @VND_Paper_ID and pm.PPR_PaperWeight_ID = @PPR_PaperWeight_ID
union
select PPR_PaperGrade_ID, Grade
from PPR_PaperGrade
where PPR_PaperGrade_ID = @PPR_PaperGrade_ID


GO

GRANT  EXECUTE  ON [dbo].[VndPaperGrade_s_Grade_ByPaperIDandPaperWeightID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
