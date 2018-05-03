IF OBJECT_ID('dbo.PubPubGroup_s_ActiveIDs_ByRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubPubGroup_s_ActiveIDs_ByRunDate'
	DROP PROCEDURE dbo.PubPubGroup_s_ActiveIDs_ByRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubPubGroup_s_ActiveIDs_ByRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PubPubGroup_s_ActiveIDs_ByRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PubPubGroup_s_ActiveIDs_ByRunDate'
GO

CREATE PROC dbo.PubPubGroup_s_ActiveIDs_ByRunDate
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns a list of pub group ID's for the specified RunDate
*
* TABLES:
*		Table Name					Access
*		==========					======
*		pub_pubgroup					READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
*	Date		Who		Comments
*	------------- 	--------        -------------------------------------------------
*	08/30/2007	JRH		Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@RunDate datetime

AS

SELECT
	grp.pub_pubgroup_id
	, grp.[description]
	, grp.sortorder
FROM
	dbo.pub_pubgroup grp (nolock)
	INNER JOIN (
		SELECT 
			[description]
			, max(effectivedate) as effectivedate
		FROM
			dbo.pub_pubgroup
		WHERE
			effectivedate <= @RunDate
			and customgroupforpackage = 0
		GROUP BY
			description
		) effective
	ON grp.[description] = effective.[description]
		AND grp.effectivedate = effective.effectivedate
WHERE
	grp.active = 1
	and customgroupforpackage = 0
ORDER BY
	grp.sortorder
	, grp.[description]	
GO

GRANT  EXECUTE  ON [dbo].[PubPubGroup_s_ActiveIDs_ByRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO