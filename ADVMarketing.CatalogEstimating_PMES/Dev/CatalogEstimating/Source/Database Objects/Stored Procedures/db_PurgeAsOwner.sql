IF OBJECT_ID('dbo.db_PurgeAsOwner') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.db_PurgeAsOwner'
	DROP PROCEDURE dbo.db_PurgeAsOwner
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.db_PurgeAsOwner') IS NOT NULL
		PRINT '***********Drop of dbo.db_PurgeAsOwner FAILED.'
END
GO
PRINT 'Creating dbo.db_PurgeAsOwner'
GO

CREATE PROCEDURE dbo.db_PurgeAsOwner
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Performs the database purging of all non-essential data.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   MANY                DELETE
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date		Who		Comments
* ----------	----		-------------------------------------------------
* 10/08/2007	JRH		Redirected from db_Purge
*
*/
AS

TRUNCATE TABLE est_packagepolybag_map
TRUNCATE TABLE pub_dayofweekrates

DELETE FROM est_polybag
DBCC CHECKIDENT('est_polybag', RESEED, 0)
DBCC CHECKIDENT('est_polybag')

TRUNCATE TABLE est_packagecomponentmapping
TRUNCATE TABLE est_estimatepolybaggroup_map

DELETE FROM pub_insertdiscounts
DBCC CHECKIDENT('pub_insertdiscounts', RESEED, 0)
DBCC CHECKIDENT('pub_insertdiscounts', RESEED)

DELETE FROM pub_dayofweekratetypes
DBCC CHECKIDENT('pub_dayofweekratetypes', RESEED, 0)
DBCC CHECKIDENT('pub_dayofweekratetypes', RESEED)

DELETE FROM pub_dayofweekquantity
DBCC CHECKIDENT('pub_dayofweekquantity', RESEED, 0)
DBCC CHECKIDENT('pub_dayofweekquantity', RESEED)

TRUNCATE TABLE pst_postalcategoryscenario_map

DELETE FROM est_polybaggroup
DBCC CHECKIDENT('est_polybaggroup', RESEED, 0)
DBCC CHECKIDENT('est_polybaggroup', RESEED)

DELETE FROM est_component
DBCC CHECKIDENT('est_component', RESEED, 0)
DBCC CHECKIDENT('est_component', RESEED)

TRUNCATE TABLE pub_pubrate_map_activate

DELETE FROM pub_pubrate
DBCC CHECKIDENT('pub_pubrate', RESEED, 0)
DBCC CHECKIDENT('pub_pubrate', RESEED)

DELETE FROM pub_pubquantity
DBCC CHECKIDENT('pub_pubquantity', RESEED, 0)
DBCC CHECKIDENT('pub_pubquantity', RESEED)

TRUNCATE TABLE pub_pubpubgroup_map

DELETE FROM pst_postalcategoryrate_map
DBCC CHECKIDENT('pst_postalcategoryrate_map', RESEED, 0)
DBCC CHECKIDENT('pst_postalcategoryrate_map', RESEED)

DELETE FROM prt_printerrate
DBCC CHECKIDENT('prt_printerrate', RESEED, 0)
DBCC CHECKIDENT('prt_printerrate', RESEED)

DELETE FROM ppr_paper_map
DBCC CHECKIDENT('ppr_paper_map', RESEED, 0)
DBCC CHECKIDENT('ppr_paper_map', RESEED)

TRUNCATE TABLE est_samples

TRUNCATE TABLE est_pubissuedates

DELETE FROM est_package
DBCC CHECKIDENT('est_package', RESEED, 0)
DBCC CHECKIDENT('est_package', RESEED)

TRUNCATE TABLE est_assemdistriboptions

DELETE FROM vnd_printer
DBCC CHECKIDENT('vnd_printer', RESEED, 0)
DBCC CHECKIDENT('vnd_printer', RESEED)

DELETE FROM vnd_paper
DBCC CHECKIDENT('vnd_paper', RESEED, 0)
DBCC CHECKIDENT('vnd_paper', RESEED)

DELETE FROM vnd_mailtrackingrate
DBCC CHECKIDENT('vnd_mailtrackingrate', RESEED, 0)
DBCC CHECKIDENT('vnd_mailtrackingrate', RESEED)

DELETE FROM vnd_maillistresourcerate
DBCC CHECKIDENT('vnd_maillistresourcerate', RESEED, 0)
DBCC CHECKIDENT('vnd_maillistresourcerate', RESEED)

DELETE FROM vnd_mailhouserate
DBCC CHECKIDENT('vnd_mailhouserate', RESEED, 0)
DBCC CHECKIDENT('vnd_mailhouserate', RESEED)

TRUNCATE TABLE vnd_vendorvendortype_map
TRUNCATE TABLE rpt_report

DELETE FROM pub_pubrate_map
DBCC CHECKIDENT('pub_pubrate_map', RESEED, 0)
DBCC CHECKIDENT('pub_pubrate_map', RESEED)

TRUNCATE TABLE pub_groupinsertscenario_map

DELETE FROM pst_postalweights
DBCC CHECKIDENT('pst_postalweights', RESEED, 0)
DBCC CHECKIDENT('pst_postalweights', RESEED)

DELETE FROM pst_postalscenario
DBCC CHECKIDENT('pst_postalscenario', RESEED, 0)
DBCC CHECKIDENT('pst_postalscenario', RESEED)

DELETE FROM est_estimate
DBCC CHECKIDENT('est_estimate', RESEED, 0)
DBCC CHECKIDENT('est_estimate', RESEED)

DELETE FROM vnd_vendor
DBCC CHECKIDENT('vnd_vendor', RESEED, 0)
DBCC CHECKIDENT('vnd_vendor', RESEED)

DELETE FROM pub_pubgroup
DBCC CHECKIDENT('pub_pubgroup', RESEED, 0)
DBCC CHECKIDENT('pub_pubgroup', RESEED)

DELETE FROM pub_insertscenario
DBCC CHECKIDENT('pub_insertscenario', RESEED, 0)
DBCC CHECKIDENT('pub_insertscenario', RESEED)

DELETE FROM pst_postalcategory
DBCC CHECKIDENT('pst_postalcategory', RESEED, 0)
DBCC CHECKIDENT('pst_postalcategory', RESEED)

DELETE FROM ppr_paperweight
DBCC CHECKIDENT('ppr_paperweight', RESEED, 0)
DBCC CHECKIDENT('ppr_paperweight', RESEED)

DELETE FROM ppr_papergrade
DBCC CHECKIDENT('ppr_papergrade', RESEED, 0)
DBCC CHECKIDENT('ppr_papergrade', RESEED)
GO

GRANT  EXECUTE  ON [dbo].[db_PurgeAsOwner]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO