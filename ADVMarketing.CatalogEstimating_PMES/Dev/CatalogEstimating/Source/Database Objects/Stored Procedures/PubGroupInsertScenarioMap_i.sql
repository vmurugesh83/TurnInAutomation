IF OBJECT_ID('dbo.PubGroupInsertScenarioMap_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubGroupInsertScenarioMap_i'
	DROP PROCEDURE dbo.PubGroupInsertScenarioMap_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubGroupInsertScenarioMap_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubGroupInsertScenarioMap_i FAILED.'
END
GO
PRINT 'Creating dbo.PubGroupInsertScenarioMap_i'
GO

create PROCEDURE dbo.PubGroupInsertScenarioMap_i
/*
* PARAMETERS:
*	PUB_InsertScenario_ID
* PUBGroupDescription
* CreatedBy

*
* DESCRIPTION:
* Creates a new map between an insert scenario and a pub group description.
* Check to see that a pub group with the specified description exists.
* If any estimates reference the Insert Scenario.  This will break that link.
*
* TABLES:
*   Table Name                       Access
*   ==========                       ======
*   EST_Package                      READ/UPDATE
*   PUB_InsertScenario               READ
*   PUB_PubGroup                     READ
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
@PUBGroupDescription varchar(35),
@CreatedBy varchar(50)
as

begin tran t

if not exists(select 1 from PUB_PubGroup where Description = @PUBGroupDescription) begin
	rollback tran t
	raiserror('Insert Scenario Map cannot be created.  The specified Pub Group Description cannot be found.', 16, 1)
	return
end

UPDATE 	dbo.EST_Package
SET	pub_insertscenario_id = null
WHERE	pub_insertscenario_id = @PUB_InsertScenario_ID

if (@@error <> 0) begin
	rollback tran t
	raiserror('Error removing Estimate Insert Scenario Map record.', 16, 1)
	return
end

insert into PUB_GroupInsertScenario_Map(PUB_InsertScenario_ID, PUBGroupDescription, CreatedBy, CreatedDate)
values(@PUB_InsertScenario_ID, @PUBGroupDescription, @CreatedBy, getdate())
if (@@error <> 0) begin
	rollback tran t
	raiserror('Error creating PUB_GroupInsertScenario_Map record.', 16, 1)
	return
end

commit tran t
GO

GRANT  EXECUTE  ON [dbo].[PubGroupInsertScenarioMap_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
