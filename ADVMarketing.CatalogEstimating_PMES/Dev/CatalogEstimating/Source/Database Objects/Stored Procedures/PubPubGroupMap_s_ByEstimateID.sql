IF OBJECT_ID('dbo.PubPubGroupMap_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubPubGroupMap_s_ByEstimateID'
	DROP PROCEDURE dbo.PubPubGroupMap_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubPubGroupMap_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.PubPubGroupMap_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.PubPubGroupMap_s_ByEstimateID'
GO

create proc dbo.PubPubGroupMap_s_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID - required.
*
* DESCRIPTION:
*		Returns the publication groups used by the estimate specified.
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
@EST_Estimate_ID bigint
AS

SELECT
	map.[pub_pubrate_map_id]
	, map.[pub_pubgroup_id]
	, map.[createdby]
	, map.[createddate]
	, map.[modifiedby]
	, map.[modifieddate] 
FROM
	dbo.est_package p (nolock)
	INNER JOIN dbo.pub_pubpubgroup_map map (nolock)
		ON p.pub_pubgroup_id = map.pub_pubgroup_id
WHERE
	p.est_estimate_id = @EST_Estimate_ID

GO

GRANT  EXECUTE  ON [dbo].[PubPubGroupMap_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
