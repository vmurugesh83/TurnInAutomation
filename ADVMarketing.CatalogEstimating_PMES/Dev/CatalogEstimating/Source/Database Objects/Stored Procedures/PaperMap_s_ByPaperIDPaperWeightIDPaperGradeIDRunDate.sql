IF OBJECT_ID('dbo.PaperMap_s_ByPaperIDPaperWeightIDPaperGradeIDRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PaperMap_s_ByPaperIDPaperWeightIDPaperGradeIDRunDate'
	DROP PROCEDURE dbo.PaperMap_s_ByPaperIDPaperWeightIDPaperGradeIDRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PaperMap_s_ByPaperIDPaperWeightIDPaperGradeIDRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PaperMap_s_ByPaperIDPaperWeightIDPaperGradeIDRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PaperMap_s_ByPaperIDPaperWeightIDPaperGradeIDRunDate'
GO

CREATE PROC dbo.PaperMap_s_ByPaperIDPaperWeightIDPaperGradeIDRunDate
@VND_Paper_ID bigint,
@PPR_PaperWeight_ID int,
@PPR_PaperGrade_ID int,
@RunDate datetime
as
/*
* PARAMETERS:
* VND_Paper_ID - required
* PPR_PaperWeight_ID required
* PPR_PaperGrade_ID - required
* RunDate - required
*
* DESCRIPTION:
* Returns all paper map rates vendor, paper weight, paper grade for the specified run date.
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   VND_Paper                      READ
*   PPR_Paper_Map                  READ
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
* 08/14/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/

select *
from PPR_Paper_Map
where VND_Paper_ID = @VND_Paper_ID and PPR_PaperWeight_ID = @PPR_PaperWeight_ID and PPR_PaperGrade_ID = @PPR_PaperGrade_ID
order by [default] desc
GO

GRANT  EXECUTE  ON [dbo].[PaperMap_s_ByPaperIDPaperWeightIDPaperGradeIDRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
