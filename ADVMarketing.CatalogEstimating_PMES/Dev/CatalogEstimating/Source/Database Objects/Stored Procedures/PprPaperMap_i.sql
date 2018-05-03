IF OBJECT_ID('dbo.PprPaperMap_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PprPaperMap_i'
	DROP PROCEDURE dbo.PprPaperMap_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PprPaperMap_i') IS NOT NULL
		PRINT '***********Drop of dbo.PprPaperMap_i FAILED.'
END
GO
PRINT 'Creating dbo.PprPaperMap_i'
GO

CREATE PROC dbo.PprPaperMap_i
@PPR_Paper_Map_ID bigint output,
@Description varchar(35),
@CWT money,
@Default bit,
@PPR_PaperGrade_ID int,
@PPR_PaperWeight_ID int,
@VND_Paper_ID bigint,
@CreatedBy varchar(50)
/*
* PARAMETERS:
* PPR_Paper_Map_ID
* Description
* CWT
* Default
* PPR_PaperGrade_ID
* PPR_PaperWeight_ID
* VND_Paper_ID
* CreatedBy
*
* DESCRIPTION:
* Creates a Paper Map record.  If it is the default, set the old default to "not-default".
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   PPR_Paper_Map                  INSERT
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
as

if (@Default = 1) begin
	update PPR_Paper_Map
		set
			[Default] = 0,
			ModifiedBy = @CreatedBy,
			ModifiedDate = getdate()
	where VND_Paper_ID = @VND_Paper_ID
		and [Default] = 1
end

insert PPR_Paper_Map(Description, CWT, [Default], PPR_PaperGrade_ID, PPR_PaperWeight_ID, VND_Paper_ID, CreatedBy, CreatedDate)
values(@Description, @CWT, @Default, @PPR_PaperGrade_ID, @PPR_PaperWeight_ID, @VND_Paper_ID, @CreatedBy, getdate())
set @PPR_Paper_Map_ID = @@identity
GO

GRANT  EXECUTE  ON [dbo].[PprPaperMap_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
