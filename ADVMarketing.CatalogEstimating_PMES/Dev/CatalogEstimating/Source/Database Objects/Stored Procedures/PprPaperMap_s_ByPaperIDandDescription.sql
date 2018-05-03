IF OBJECT_ID('dbo.PprPaperMap_s_ByPaperIDandDescription') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperMap_s_ByPaperIDandDescription'
	DROP PROCEDURE dbo.PprPaperMap_s_ByPaperIDandDescription
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperMap_s_ByPaperIDandDescription') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperMap_s_ByPaperIDandDescription FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperMap_s_ByPaperIDandDescription'
GO

create proc dbo.PprPaperMap_s_ByPaperIDandDescription
/*
* PARAMETERS:
* VND_Paper_ID - Required.  The PaperID.
* Description - Required.
*
*
* DESCRIPTION:
*		Returns the Paper Map record for the Paper Vendor and Description specified
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
*   09/04/07        BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Paper_ID bigint,
@Description varchar(35)
as

--Try to find an exact match
if exists(select 1 from PPR_Paper_Map where VND_Paper_ID = @VND_Paper_ID and Description = @Description) begin
	select * from PPR_Paper_Map where VND_Paper_ID = @VND_Paper_ID and Description = @Description
end
--If no exact match can be found, return the default
else begin
	select * from PPR_Paper_Map where VND_Paper_ID = @VND_Paper_ID and [Default] = 1
end
GO

GRANT  EXECUTE  ON [dbo].[PprPaperMap_s_ByPaperIDandDescription]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
