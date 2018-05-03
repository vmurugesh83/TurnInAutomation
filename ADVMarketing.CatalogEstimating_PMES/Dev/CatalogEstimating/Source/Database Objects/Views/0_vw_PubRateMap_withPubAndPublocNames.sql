IF OBJECT_ID('dbo.vw_PubRateMap_withPubAndPublocNames') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vw_PubRateMap_withPubAndPublocNames'
	DROP VIEW dbo.vw_PubRateMap_withPubAndPublocNames
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vw_PubRateMap_withPubAndPublocNames') IS NOT NULL
		PRINT '***********Drop of dbo.vw_PubRateMap_withPubAndPublocNames FAILED.'
END
GO
PRINT 'Creating dbo.vw_PubRateMap_withPubAndPublocNames'
GO

create view dbo.vw_PubRateMap_withPubAndPublocNames
/*
* DESCRIPTION:
*		Returns PubRateMap record, pub description and pub loc description.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   
*
* REVISION HISTORY:
*
* Date            Who             Comments
* ----------      -----           -------------------------------------------------
* 09/11/2007      JRH             Initial Creation
*/
AS

SELECT
	prm.pub_pubrate_map_id
	, prm.pub_id
	, prm.publoc_id
	, p.pub_nm
	, pl.publoc_nm
FROM
	dbo.pub_pubrate_map prm (nolock)
	INNER JOIN DBADVProd.informix.pub p (nolock)
		ON prm.pub_id = p.pub_id
	INNER JOIN DBADVProd.informix.pub_loc pl (nolock)
		ON prm.pub_id = pl.pub_id
		AND prm.publoc_id = pl.publoc_id

GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vw_PubRateMap_withPubAndPublocNames]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
