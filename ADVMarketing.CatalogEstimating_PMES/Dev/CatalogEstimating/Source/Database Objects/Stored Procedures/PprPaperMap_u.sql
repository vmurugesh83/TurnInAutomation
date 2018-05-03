IF OBJECT_ID('dbo.PprPaperMap_u') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperMap_u'
	DROP PROCEDURE dbo.PprPaperMap_u
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperMap_u') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperMap_u FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperMap_u'
GO

CREATE PROC dbo.PprPaperMap_u
/*
* PARAMETERS:
* PPR_Paper_Map_ID
* Description
* CWT
* Default
* PPR_PaperGrade_ID
* PPR_PaperWeight_ID
* VND_Paper_ID
* ModifiedBy
*
* DESCRIPTION:
* Updates a Paper Map record.  If it is the default, set the old default to "not-default".
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   PPR_Paper_Map                  UPDATE
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
* 09/28/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PPR_Paper_Map_ID bigint,
@Description varchar(35),
@CWT money,
@Default bit,
@PPR_PaperGrade_ID int,
@PPR_PaperWeight_ID int,
@VND_Paper_ID bigint,
@ModifiedBy varchar(50)
as

if (@Default = 1) begin
	update PPR_Paper_Map
		set
			[Default] = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedDate = getdate()
	where PPR_Paper_Map_ID <> @PPR_Paper_Map_ID
		and VND_Paper_ID = @VND_Paper_ID
		and [Default] = 1
end

update PPR_Paper_Map
	set
		Description = @Description,
		CWT = @CWT,
		[Default] = @Default,
		PPR_PaperGrade_ID = @PPR_PaperGrade_ID,
		PPR_PaperWeight_ID = @PPR_PaperWeight_ID,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where PPR_Paper_Map_ID = @PPR_Paper_Map_ID
GO

GRANT  EXECUTE  ON [dbo].[PprPaperMap_u]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
