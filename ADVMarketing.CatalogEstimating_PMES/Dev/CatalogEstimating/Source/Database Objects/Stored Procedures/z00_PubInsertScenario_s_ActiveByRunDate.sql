IF OBJECT_ID('dbo.PubInsertScenario_s_ActiveByRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubInsertScenario_s_ActiveByRunDate'
	DROP PROCEDURE dbo.PubInsertScenario_s_ActiveByRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubInsertScenario_s_ActiveByRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PubInsertScenario_s_ActiveByRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PubInsertScenario_s_ActiveByRunDate'
GO

CREATE PROC dbo.PubInsertScenario_s_ActiveByRunDate
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns a list of scenarios containing active pub groups 
*			for the specified RunDate.
*
* TABLES:
*		Table Name					Access
*		==========	                ======
*		pub_insertscenario          READ
*
* PROCEDURES CALLED:
*		Procedure Name
*		==============
*		dbo.PubPubGroup_s_ActiveIDs_ByRunDate
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
*	------------- 	--------        -------------------------------------------------
*	08/30/2007      JRH             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@RunDate datetime

AS

CREATE TABLE #groups
	(
	[pub_pubgroup_id]	bigint not null
	, [description]		varchar(35)
	, [sortorder]		int
	)

INSERT INTO #groups
	EXEC dbo.PubPubGroup_s_ActiveIDs_ByRunDate @RunDate

SELECT DISTINCT
	scen.pub_insertscenario_id
	, scen.[description]
FROM
	dbo.pub_insertscenario scen (nolock)
	INNER JOIN dbo.pub_groupinsertscenario_map m (nolock)
		ON scen.pub_insertscenario_id = m.pub_insertscenario_id
	INNER JOIN #groups
		ON m.pubgroupdescription = #groups.[description]
WHERE
	scen.active = 1


DROP TABLE #groups
GO

GRANT  EXECUTE  ON [dbo].[PubInsertScenario_s_ActiveByRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO