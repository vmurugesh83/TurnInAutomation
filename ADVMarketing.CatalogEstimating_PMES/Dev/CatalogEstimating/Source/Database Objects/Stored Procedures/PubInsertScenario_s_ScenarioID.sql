IF OBJECT_ID('dbo.PubInsertScenario_s_ScenarioID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubInsertScenario_s_ScenarioID'
	DROP PROCEDURE dbo.PubInsertScenario_s_ScenarioID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubInsertScenario_s_ScenarioID') IS NOT NULL
		PRINT '***********Drop of dbo.PubInsertScenario_s_ScenarioID FAILED.'
END
GO
PRINT 'Creating dbo.PubInsertScenario_s_ScenarioID'
GO

CREATE PROC dbo.PubInsertScenario_s_ScenarioID
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns scenario detail given a specified scenario ID.
*
* TABLES:
*		Table Name					Access
*		==========					======
*		pub_insertscenario          READ
*
* PROCEDURES CALLED:
*		Procedure Name
*		==============
*		none
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
*	Date            Who             Comments
*	-------------   --------        -------------------------------------------------
*	09/17/2007      JRH             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@ScenarioID bigint

AS

SELECT 
	pub_insertscenario_id
	, [description]
	, comments
	, active
	, createdby
	, createddate
	, modifiedby
	, modifieddate
FROM
	dbo.pub_insertscenario (nolock)
WHERE
	pub_insertscenario_id = @ScenarioID
GO


GRANT  EXECUTE  ON [dbo].[PubInsertScenario_s_ScenarioID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO