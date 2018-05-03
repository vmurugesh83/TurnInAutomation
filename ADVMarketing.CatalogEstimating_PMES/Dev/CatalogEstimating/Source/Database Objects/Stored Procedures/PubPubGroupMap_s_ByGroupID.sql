IF OBJECT_ID('dbo.PubPubGroupMap_s_ByGroupID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubPubGroupMap_s_ByGroupID'
	DROP PROCEDURE dbo.PubPubGroupMap_s_ByGroupID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubPubGroupMap_s_ByGroupID') IS NOT NULL
		PRINT '***********Drop of dbo.PubPubGroupMap_s_ByGroupID FAILED.'
END
GO
PRINT 'Creating dbo.PubPubGroupMap_s_ByGroupID'
GO

create proc dbo.PubPubGroupMap_s_ByGroupID
/*
* PARAMETERS:
* EST_Estimate_ID - required.
*
* DESCRIPTION:
*		Returns the publication group specified.
*
*
* TABLES:
*	Table Name		Access
*	==========		======
*	EST_Package		READ
*	PUB_PubGroup		READ
*	PUB_PubPubGroupMap	READ
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
* Date          Who		Comments
* ----------	--------	-------------------------------------------------
* 09/10/2007	JRH		Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PUB_PubGroup_ID bigint
AS

SELECT
	map.[pub_pubrate_map_id]
	, map.[pub_pubgroup_id]
	, map.[createdby]
	, map.[createddate]
	, map.[modifiedby]
	, map.[modifieddate] 
FROM
	dbo.pub_pubpubgroup_map map (nolock)
WHERE
	map.pub_pubgroup_id = @PUB_PubGroup_ID

GO

GRANT  EXECUTE  ON [dbo].[PubPubGroupMap_s_ByGroupID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
