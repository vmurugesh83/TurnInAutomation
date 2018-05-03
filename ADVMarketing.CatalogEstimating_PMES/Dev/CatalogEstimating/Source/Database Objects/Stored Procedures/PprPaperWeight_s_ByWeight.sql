IF OBJECT_ID('dbo.PprPaperWeight_s_ByWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperWeight_s_ByWeight'
	DROP PROCEDURE dbo.PprPaperWeight_s_ByWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperWeight_s_ByWeight') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperWeight_s_ByWeight FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperWeight_s_ByWeight'
GO

create proc dbo.PprPaperWeight_s_ByWeight
/*
* PARAMETERS:
* Weight - Required.
*
*
* DESCRIPTION:
*		Returns the Paper Weights for the specified Weight.
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		PPR_PaperWeight     READ
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
@Weight int
as

select * from PPR_PaperWeight
where Weight = @Weight
GO

GRANT  EXECUTE  ON [dbo].[PprPaperWeight_s_ByWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
