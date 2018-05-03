IF OBJECT_ID('dbo.PubRateMap_s_ActiveLocsByRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRateMap_s_ActiveLocsByRunDate'
	DROP PROCEDURE dbo.PubRateMap_s_ActiveLocsByRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRateMap_s_ActiveLocsByRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PubRateMap_s_ActiveLocsByRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PubRateMap_s_ActiveLocsByRunDate'
GO

CREATE PROC dbo.PubRateMap_s_ActiveLocsByRunDate
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns a list of active pub locations for the specified RunDate
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
*	08/30/2007	JRH		Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@RunDate datetime

AS

SELECT
	map.pub_pubrate_map_id
	, map.pub_id
	, map.publoc_id
	, loc.publoc_nm
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
		WHERE
			ma.effectivedate <= @RunDate
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

GRANT  EXECUTE  ON [dbo].[PubRateMap_s_ActiveLocsByRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO