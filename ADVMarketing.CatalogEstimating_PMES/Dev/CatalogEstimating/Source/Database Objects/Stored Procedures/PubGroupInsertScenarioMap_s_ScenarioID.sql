IF OBJECT_ID('dbo.PubGroupInsertScenarioMap_s_ScenarioID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubGroupInsertScenarioMap_s_ScenarioID'
	DROP PROCEDURE dbo.PubGroupInsertScenarioMap_s_ScenarioID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubGroupInsertScenarioMap_s_ScenarioID') IS NOT NULL
		PRINT '***********Drop of dbo.PubGroupInsertScenarioMap_s_ScenarioID FAILED.'
END
GO
PRINT 'Creating dbo.PubGroupInsertScenarioMap_s_ScenarioID'
GO

CREATE PROC dbo.PubGroupInsertScenarioMap_s_ScenarioID
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns list of pubgroup descriptions mapped 
*			to a given scenario ID.
*
* TABLES:
*		Table Name                          Access
*		==========                          ======
*		pub_groupinsertscenario_map			READ
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
	pubgroupdescription
	, pub_insertscenario_id
	, createdby
	, createddate
	, modifiedby
	, modifieddate
FROM
	dbo.pub_groupinsertscenario_map (nolock)
WHERE
	pub_insertscenario_id = @ScenarioID
GO

GRANT  EXECUTE  ON [dbo].[PubGroupInsertScenarioMap_s_ScenarioID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO