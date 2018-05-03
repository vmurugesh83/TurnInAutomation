IF OBJECT_ID('dbo.PprPaperMap_s_Default_ByPaperID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperMap_s_Default_ByPaperID'
	DROP PROCEDURE dbo.PprPaperMap_s_Default_ByPaperID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperMap_s_Default_ByPaperID') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperMap_s_Default_ByPaperID FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperMap_s_Default_ByPaperID'
GO

create proc dbo.PprPaperMap_s_Default_ByPaperID
/*
* PARAMETERS:
* VND_Paper_ID - Required.  The PaperID.
*
*
* DESCRIPTION:
*		Returns the default Paper Map record for the Paper Vendor specified.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PPR_Paper_Map       READ
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
*   10/10/07        BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Paper_ID bigint
as

select * from PPR_Paper_Map
where VND_Paper_ID = @VND_Paper_ID and [Default] = 1
GO

GRANT  EXECUTE  ON [dbo].[PprPaperMap_s_Default_ByPaperID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
