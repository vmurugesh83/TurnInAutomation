IF OBJECT_ID('dbo.PubRateMap_s_ActiveLocsByGroupIDRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRateMap_s_ActiveLocsByGroupIDRunDate'
	DROP PROCEDURE dbo.PubRateMap_s_ActiveLocsByGroupIDRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRateMap_s_ActiveLocsByGroupIDRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PubRateMap_s_ActiveLocsByGroupIDRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PubRateMap_s_ActiveLocsByGroupIDRunDate'
GO

CREATE PROC dbo.PubRateMap_s_ActiveLocsByGroupIDRunDate
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns a list of active pub locations for the specified Group ID and RunDate
*
* TABLES:
*		Table Name					Access
*		==========					======
*		pub_pubrate_map					READ
*		pub_pubrate_map_activate			READ
*		pub_loc						READ
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
*	09/17/2007	JRH		Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@RunDate	datetime,
@GroupID	bigint

AS

SELECT
	map.pub_pubrate_map_id
	, map.pub_id
	, map.publoc_id
	, map.createdby
	, map.createddate
	, map.modifiedby
	, map.modifieddate
FROM
	dbo.pub_pubrate_map map (nolock)
	INNER JOIN dbo.pub_pubrate_map_activate atv (nolock)
	ON map.pub_pubrate_map_id = atv.pub_pubrate_map_id
	INNER JOIN (
		SELECT 
			ma.pub_pubrate_map_id
			, max(ma.effectivedate) as effectivedate
		FROM
			dbo.pub_pubrate_map m (nolock)
			INNER JOIN dbo.pub_pubrate_map_activate ma (nolock)
				ON m.pub_pubrate_map_id = ma.pub_pubrate_map_id
			INNER JOIN dbo.pub_pubpubgroup_map gm (nolock)
				ON m.pub_pubrate_map_id = gm.pub_pubrate_map_id
		WHERE
			ma.effectivedate <= @RunDate
			AND gm.pub_pubgroup_id = @GroupID
		GROUP BY
			ma.pub_pubrate_map_id
		) effective
	ON atv.pub_pubrate_map_id = effective.pub_pubrate_map_id
		AND atv.effectivedate = effective.effectivedate
	INNER JOIN DBADVProd.informix.pub_loc loc (nolock)
	ON map.pub_id = loc.pub_id
		AND map.publoc_id = loc.publoc_id
WHERE
	atv.active = 1	
GO

GRANT  EXECUTE  ON [dbo].[PubRateMap_s_ActiveLocsByGroupIDRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO