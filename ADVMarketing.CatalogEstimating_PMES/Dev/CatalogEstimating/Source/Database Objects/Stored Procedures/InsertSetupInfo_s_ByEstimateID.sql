IF OBJECT_ID('dbo.InsertSetupInfo_s_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.InsertSetupInfo_s_ByEstimateID'
	DROP PROCEDURE dbo.InsertSetupInfo_s_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.InsertSetupInfo_s_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.InsertSetupInfo_s_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.InsertSetupInfo_s_ByEstimateID'
GO

CREATE PROC dbo.InsertSetupInfo_s_ByEstimateID
/*
* PARAMETERS:
* Estimate ID - Required
*
* DESCRIPTION:
*		Returns information used on the Distribution Mapping -> Insert Setup tab. 
*
* TABLES:
*		Table Name					Access
*		==========					======
*		est_package					READ
*		pub_pubgroup					READ
*		pub_pubpubgroup_map				READ
*		vw_PubRateMap_withPubAndPublocNames		READ
*		est_pubissuedates				READ
*		pub_pubquantity					READ
*		pub_dayofweekquantity				READ
*
* PROCEDURES CALLED:
*		Procedure Name
*		==============
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
*	09/11/2007	JRH		Initial Creation 
*	09/19/2007	JRH		Removed pid.est_pubissuedates
*					Added display and displaySortOrder.
*	10/08/2007	JRH		Get quantity using function.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID	bigint

AS

SELECT
	p.est_package_id
	, p.description AS PackageName
	, p.pub_pubquantitytype_id
	, pqt.description AS pub_pubquantitytype
	, p.pub_pubgroup_id
	, grp.description AS GroupName
	, grp.active AS GroupActiveFlag
	, grp.effectivedate AS GroupEffectiveDate
	, grp.sortorder
	, grp.customgroupforpackage
	, gm.pub_pubrate_map_id
	, vNames.pub_nm
	, vNames.publoc_nm
	, pid.override
	, pid.issuedow
	, pid.issuedate
	, quantity = dbo.PubRateMapInsertQuantityByInsertDate(pid.issuedate, p.pub_pubquantitytype_id, gm.pub_pubrate_map_id)
	, p.description AS GridDescription
	, pid.override AS ScenarioFlag
	, pid.override AS GroupFlag
	, display = 1
	, displaySortOrder = substring('0000000000', 0, 10 - len(convert(varchar(50), sortorder))) + convert(varchar(50), sortorder)
			+ '.' + vNames.pub_id + '.' 
			+ substring('00000', 0, 5 - len(convert(varchar(50), vNames.publoc_id))) + convert(varchar(50), vNames.publoc_id)
FROM
	dbo.est_estimate e (nolock)
	INNER JOIN dbo.est_package p (nolock)
		ON e.est_estimate_id = p.est_estimate_id
	INNER JOIN dbo.pub_pubgroup grp (nolock)
		ON p.pub_pubgroup_id = grp.pub_pubgroup_id
	INNER JOIN dbo.pub_pubpubgroup_map gm (nolock)
		ON grp.pub_pubgroup_id = gm.pub_pubgroup_id
	INNER JOIN dbo.vw_PubRateMap_withPubAndPublocNames vNames (nolock)
		ON gm.pub_pubrate_map_id = vNames.pub_pubrate_map_id
	INNER JOIN dbo.est_pubissuedates pid (nolock)
		ON gm.pub_pubrate_map_id = pid.pub_pubrate_map_id
		AND p.est_estimate_id = pid.est_estimate_id
	INNER JOIN dbo.pub_pubquantitytype pqt (nolock)
		ON p.pub_pubquantitytype_id = pqt.pub_pubquantitytype_id

WHERE
	e.est_estimate_id = @EST_Estimate_ID
GO

GRANT  EXECUTE  ON [dbo].[InsertSetupInfo_s_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO