IF OBJECT_ID('dbo.PprPaperGrade_s_ByGrade') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperGrade_s_ByGrade'
	DROP PROCEDURE dbo.PprPaperGrade_s_ByGrade
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperGrade_s_ByGrade') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperGrade_s_ByGrade FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperGrade_s_ByGrade'
GO

create proc dbo.PprPaperGrade_s_ByGrade
/*
* PARAMETERS:
* Grade - Required.
*
*
* DESCRIPTION:
*		Returns the Paper Grade for the specified Grade.
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		PPR_PaperGrade      READ
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
*	Date            Who             Comments
*	------------- 	--------        -------------------------------------------------
*   09/04/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Grade varchar(50)
as
select * from PPR_PaperGrade
where Grade = @Grade
GO

GRANT  EXECUTE  ON [dbo].[PprPaperGrade_s_ByGrade]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
