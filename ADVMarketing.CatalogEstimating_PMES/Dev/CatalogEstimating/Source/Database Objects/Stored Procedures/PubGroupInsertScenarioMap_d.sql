IF OBJECT_ID('dbo.PubGroupInsertScenarioMap_d') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubGroupInsertScenarioMap_d'
	DROP PROCEDURE dbo.PubGroupInsertScenarioMap_d
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubGroupInsertScenarioMap_d') IS NOT NULL
		PRINT '***********Drop of dbo.PubGroupInsertScenarioMap_d FAILED.'
END
GO
PRINT 'Creating dbo.PubGroupInsertScenarioMap_d'
GO

create PROCEDURE dbo.PubGroupInsertScenarioMap_d
/*
* PARAMETERS:
*	PUB_InsertScenario_ID
* PUBGroupDescription

*
* DESCRIPTION:
* Deletes a new map between an insert scenario and a pub group description.
* If any estimates reference the Insert Scenario.  This will break that link.
*
* TABLES:
*   Table Name                       Access
*   ==========                       ======
*   EST_Package                      READ/UPDATE
*   PUB_InsertScenario               READ
*   PUB_GroupInsertScenario_Map      WRITE
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/29/2007      BJS             Initial Creation 
* 09/20/2007      JRH             Changed delete of EST_EstimateInsertScenario_Map 
*                                      to update of EST_Package
*
*/
@PUB_InsertScenario_ID bigint,
@PUBGroupDescription varchar(50)
as

begin tran t

UPDATE 	dbo.EST_Package
SET	pub_insertscenario_id = null
WHERE	pub_insertscenario_id = @PUB_InsertScenario_ID

if (@@error <> 0) begin
	rollback tran t
	raiserror('Error removing Insert Scenario Link From Estimate Package record.', 16, 1)
	return
end


delete from PUB_GroupInsertScenario_Map
where PUB_InsertScenario_ID = @PUB_InsertScenario_ID and PUBGroupDescription = @PUBGroupDescription
if (@@error <> 0) begin
	rollback tran t
	raiserror('Error deleting PUB_GroupInsertScenario_Map record.', 16, 1)
	return
end

commit tran t
GO

GRANT  EXECUTE  ON [dbo].[PubGroupInsertScenarioMap_d]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
