IF OBJECT_ID('dbo.EstPackage_Quantities_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPackage_Quantities_ByEstimateID'
	DROP PROCEDURE dbo.EstPackage_Quantities_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPackage_Quantities_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstPackage_Quantities_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstPackage_Quantities_ByEstimateID'
GO

CREATE proc dbo.EstPackage_Quantities_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns the Insert Qty and Polybag Qty for all packages in an Estimate
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Package
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
* Date          Who         Comments
* ----------    ----        -------------------------------------------------
* 09/13/2007	NLS         Initial Creation 
* 10/08/2007	JRH         Re-Written to pick up real insertqty and sum of polybagqty.
*
**********************************************************************************************************
*/

@EST_Estimate_ID bigint

AS

SELECT 
	pkg.est_package_id
	, insertqty = coalesce(insertqty, 0)
	, polybagqty = coalesce(polybagqty, 0)
FROM
	est_package pkg (nolock)
		LEFT JOIN (
				SELECT 
					p.est_package_id
					, insertqty = coalesce(sum(dbo.PubRateMapInsertQuantityByInsertDate(pid.issuedate, p.pub_pubquantitytype_id, gm.pub_pubrate_map_id)), 0)
				FROM
					est_package p (nolock)
						INNER JOIN dbo.pub_pubgroup grp (nolock)
							ON p.pub_pubgroup_id = grp.pub_pubgroup_id
						INNER JOIN dbo.pub_pubpubgroup_map gm (nolock)
							ON grp.pub_pubgroup_id = gm.pub_pubgroup_id
						INNER JOIN dbo.est_pubissuedates pid (nolock)
							ON gm.pub_pubrate_map_id = pid.pub_pubrate_map_id
							AND p.est_estimate_id = pid.est_estimate_id
				WHERE
					p.est_estimate_id = @EST_Estimate_ID
				GROUP BY
					p.est_package_id) iq
			ON pkg.est_package_id = iq.est_package_id
		LEFT JOIN (
				SELECT 
					p.est_package_id
					, sum(polybag.quantity) polybagqty
				FROM
					est_package p (nolock)
						INNER JOIN est_packagepolybag_map pbmap (nolock)
							ON pbmap.est_package_id = p.est_package_id 
						INNER JOIN est_polybag polybag (nolock)
							ON pbmap.est_polybag_id = polybag.est_polybag_id
				WHERE
					p.est_estimate_id = @EST_Estimate_ID
				GROUP BY
					p.est_package_id) pq
			ON pkg.est_package_id = pq.est_package_id
	

WHERE
	pkg.est_estimate_id = @EST_Estimate_ID
	
GO

GRANT  EXECUTE  ON [dbo].[EstPackage_Quantities_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO