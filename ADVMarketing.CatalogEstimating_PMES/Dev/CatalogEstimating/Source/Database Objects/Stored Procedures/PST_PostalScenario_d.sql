IF OBJECT_ID('dbo.PST_PostalScenario_d') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PST_PostalScenario_d'
	DROP PROCEDURE dbo.PST_PostalScenario_d
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PST_PostalScenario_d') IS NOT NULL
		PRINT '***********Drop of dbo.PST_PostalScenario_d FAILED.'
END
GO
PRINT 'Creating dbo.PST_PostalScenario_d'
GO

create proc dbo.PST_PostalScenario_d
@PST_PostalScenario_ID bigint/*
* PARAMETERS:
* PST_PostalScenario_ID - required.
*
* DESCRIPTION:
* Checks to see that the postal scenario is not being referenced.
* Then deletes the PST_PostalScenario record.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   EST_AssemDistribOptions        READ
*   PST_PostalCategoryScenario_Map READ
*   PST_PostalScenario             DELETE
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
* 08/01/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
as

begin tran t

if exists(select 1 from EST_AssemDistribOptions where PST_PostalScenario_ID = @PST_PostalScenario_ID) begin
	rollback tran t
	raiserror('Cannot delete Postal Scenario.  It is being referenced by an estimate', 16, 1)
	return
end

if exists(select 1 from PST_PostalCategoryScenario_Map where PST_PostalScenario_ID = @PST_PostalScenario_ID) begin
	rollback tran t
	raiserror('Cannot delete Postal Scenario.  It is being referenced by a PST_PostalCategoryScenario_Map record.', 16, 1)
	return
end

delete from PST_PostalScenario
where PST_PostalScenario_ID = @PST_PostalScenario_ID
if (@@error <> 0) begin
	rollback tran t
	raiserror('Error deleting PST_PostalScenario record.', 16, 1)
	return
end

commit tran t
GO

GRANT  EXECUTE  ON [dbo].[PST_PostalScenario_d]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
