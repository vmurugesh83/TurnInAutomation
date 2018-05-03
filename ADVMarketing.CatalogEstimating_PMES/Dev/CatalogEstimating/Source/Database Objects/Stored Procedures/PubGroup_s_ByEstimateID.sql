IF OBJECT_ID('dbo.PubGroup_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubGroup_s_ByEstimateID'
	DROP PROCEDURE dbo.PubGroup_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubGroup_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.PubGroup_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.PubGroup_s_ByEstimateID'
GO

create proc dbo.PubGroup_s_ByEstimateID
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
* 09/11/2007	JRH		Add grp.pub_pubgroup_id
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint
AS

SELECT
	grp.[pub_pubgroup_id]
	, grp.[description]
	, grp.[comments]
	, grp.[active]
	, grp.[effectivedate]
	, grp.[sortorder]
	, grp.[customgroupforpackage]
	, grp.[createdby]
	, grp.[createddate]
	, grp.[modifiedby]
	, grp.[modifieddate] 
FROM
	dbo.est_package p (nolock)
	INNER JOIN dbo.pub_pubgroup grp (nolock)
		ON p.pub_pubgroup_id = grp.pub_pubgroup_id
WHERE
	p.est_estimate_id = @EST_Estimate_ID
GO

GRANT  EXECUTE  ON [dbo].[PubGroup_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
