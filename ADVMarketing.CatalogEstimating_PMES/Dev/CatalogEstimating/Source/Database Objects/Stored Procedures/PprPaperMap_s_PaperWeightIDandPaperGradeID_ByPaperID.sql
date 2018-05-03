IF OBJECT_ID('dbo.PprPaperMap_s_PaperWeightIDandPaperGradeID_ByPaperID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperMap_s_PaperWeightIDandPaperGradeID_ByPaperID'
	DROP PROCEDURE dbo.PprPaperMap_s_PaperWeightIDandPaperGradeID_ByPaperID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperMap_s_PaperWeightIDandPaperGradeID_ByPaperID') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperMap_s_PaperWeightIDandPaperGradeID_ByPaperID FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperMap_s_PaperWeightIDandPaperGradeID_ByPaperID'
GO

create proc dbo.PprPaperMap_s_PaperWeightIDandPaperGradeID_ByPaperID
/*
* PARAMETERS:
* VND_Paper_ID - Required.  The PaperID.
* PPR_PaperGrade_ID - Output.
* PPR_PaperWeight_ID - Output.
*
*
* DESCRIPTION:
*		Returns the default PPR_PaperGrade_ID and PPR_PaperWeight_ID for the PaperID
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		PPR_Paper_Map
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
@PPR_PaperWeight_ID int output,
@PPR_PaperGrade_ID int output
as

select @PPR_PaperWeight_ID = PPR_PaperWeight_ID, @PPR_PaperGrade_ID = PPR_PaperGrade_ID
from PPR_Paper_Map
where VND_Paper_ID = @VND_Paper_ID and [Default] = 1


GO

GRANT  EXECUTE  ON [dbo].[PprPaperMap_s_PaperWeightIDandPaperGradeID_ByPaperID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
