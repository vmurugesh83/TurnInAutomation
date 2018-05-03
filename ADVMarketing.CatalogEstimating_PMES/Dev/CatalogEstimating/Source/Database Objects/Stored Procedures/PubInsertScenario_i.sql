IF OBJECT_ID('dbo.PubInsertScenario_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubInsertScenario_i'
	DROP PROCEDURE dbo.PubInsertScenario_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubInsertScenario_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubInsertScenario_i FAILED.'
END
GO
PRINT 'Creating dbo.PubInsertScenario_i'
GO

create proc dbo.PubInsertScenario_i
/*
* PARAMETERS:
* PUB_InsertScenario_ID - Output.  If a value is passed in it is ignored and the value is set to the ID of the new record.
*
* DESCRIPTION:
*   Inserts a record into the PUB_InsertScenario table.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_InsertScenario  INSERT
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
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 05/25/2007      BJS             Initial Creation
* 06/29/2007      BJS             Added error checking to ensure unique description. 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PUB_InsertScenario_ID bigint output,
@Description varchar(35),
@Comments varchar(255),
@Active bit,
@CreatedBy varchar(50)
as

if (@Description = '') begin
	raiserror('An Insert Scenario must have a description.', 16, 1)
	return
end

begin tran t
	if exists(select 1 from PUB_InsertScenario where Description = @Description) begin
		rollback tran t
		raiserror('An Insert Scenario already exists in the database with the same description.', 16, 1)
		return
	end

	insert into PUB_InsertScenario(Description, Comments, Active, CreatedBy, CreatedDate)
	values(@Description, @Comments, @Active, @CreatedBy, getdate())
	set @PUB_InsertScenario_ID = @@identity
	if (@@error <> 0) begin
		rollback tran t
		raiserror('An error occurred while creating the Insert Scenario record.', 16, 1)
		return
	end

commit tran t


GO

GRANT  EXECUTE  ON [dbo].[PubInsertScenario_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
