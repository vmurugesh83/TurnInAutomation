IF OBJECT_ID('dbo.PprPaperMap_s_ByPaperIDDescriptionWeightGrade') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperMap_s_ByPaperIDDescriptionWeightGrade'
	DROP PROCEDURE dbo.PprPaperMap_s_ByPaperIDDescriptionWeightGrade
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperMap_s_ByPaperIDDescriptionWeightGrade') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperMap_s_ByPaperIDDescriptionWeightGrade FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperMap_s_ByPaperIDDescriptionWeightGrade'
GO

create proc dbo.PprPaperMap_s_ByPaperIDDescriptionWeightGrade
/*
* PARAMETERS:
* VND_Paper_ID
* Description
* PPR_PaperWeight_ID
* PPR_PaperGrade_ID
*
*
* DESCRIPTION:
*		Returns the Paper Map record matching the specified parameters.
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		PPR_Paper_Map       READ
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
*   10/16/07        BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Paper_ID bigint,
@Description varchar(35),
@PPR_PaperGrade_ID int,
@PPR_PaperWeight_ID int
as

--Try to find an exact match
select * from PPR_Paper_Map
where VND_Paper_ID = @VND_Paper_ID and Description = @Description and PPR_PaperGrade_ID = @PPR_PaperGrade_ID
	and PPR_PaperWeight_ID = @PPR_PaperWeight_ID
GO

GRANT  EXECUTE  ON [dbo].[PprPaperMap_s_ByPaperIDDescriptionWeightGrade]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
