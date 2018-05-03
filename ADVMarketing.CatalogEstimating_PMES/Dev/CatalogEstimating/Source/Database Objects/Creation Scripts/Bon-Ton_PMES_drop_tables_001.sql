USE [BonTon_PMES_LIVE]
-- USE [BonTon_PMES_TEST1]
-- USE [BonTon_PMES_TEST2]

IF OBJECT_ID('dbo.est_packagepolybag_map') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.est_packagepolybag_map'
	DROP TABLE dbo.est_packagepolybag_map
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.est_packagepolybag_map') IS NOT NULL
		PRINT '***********Drop of dbo.est_packagepolybag_map FAILED.'
END
GO

-----------------------------------------------------------------

IF OBJECT_ID('dbo.pub_dayofweekrates') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pub_dayofweekrates'
	DROP TABLE dbo.pub_dayofweekrates
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pub_dayofweekrates') IS NOT NULL
		PRINT '***********Drop of dbo.pub_dayofweekrates FAILED.'
END
GO

IF OBJECT_ID('dbo.est_polybag') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.est_polybag'
	DROP TABLE dbo.est_polybag
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.est_polybag') IS NOT NULL
		PRINT '***********Drop of dbo.est_polybag FAILED.'
END
GO

IF OBJECT_ID('dbo.est_packagecomponentmapping') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.est_packagecomponentmapping'
	DROP TABLE dbo.est_packagecomponentmapping
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.est_packagecomponentmapping') IS NOT NULL
		PRINT '***********Drop of dbo.est_packagecomponentmapping FAILED.'
END
GO

IF OBJECT_ID('dbo.est_estimatepolybaggroup_map') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.est_estimatepolybaggroup_map'
	DROP TABLE dbo.est_estimatepolybaggroup_map
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.est_estimatepolybaggroup_map') IS NOT NULL
		PRINT '***********Drop of dbo.est_estimatepolybaggroup_map FAILED.'
END
GO

-----------------------------------------------------------------

IF OBJECT_ID('dbo.pub_insertdiscounts') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pub_insertdiscounts'
	DROP TABLE dbo.pub_insertdiscounts
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pub_insertdiscounts') IS NOT NULL
		PRINT '***********Drop of dbo.pub_insertdiscounts FAILED.'
END
GO

IF OBJECT_ID('dbo.pub_dayofweekratetypes') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pub_dayofweekratetypes'
	DROP TABLE dbo.pub_dayofweekratetypes
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pub_dayofweekratetypes') IS NOT NULL
		PRINT '***********Drop of dbo.pub_dayofweekratetypes FAILED.'
END
GO

IF OBJECT_ID('dbo.pub_dayofweekquantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pub_dayofweekquantity'
	DROP TABLE dbo.pub_dayofweekquantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pub_dayofweekquantity') IS NOT NULL
		PRINT '***********Drop of dbo.pub_dayofweekquantity FAILED.'
END
GO

IF OBJECT_ID('dbo.pst_postalcategoryscenario_map') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pst_postalcategoryscenario_map'
	DROP TABLE dbo.pst_postalcategoryscenario_map
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pst_postalcategoryscenario_map') IS NOT NULL
		PRINT '***********Drop of dbo.pst_postalcategoryscenario_map FAILED.'
END
GO

IF OBJECT_ID('dbo.est_polybaggroup') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.est_polybaggroup'
	DROP TABLE dbo.est_polybaggroup
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.est_polybaggroup') IS NOT NULL
		PRINT '***********Drop of dbo.est_polybaggroup FAILED.'
END
GO

IF OBJECT_ID('dbo.est_component') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.est_component'
	DROP TABLE dbo.est_component
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.est_component') IS NOT NULL
		PRINT '***********Drop of dbo.est_component FAILED.'
END
GO

-----------------------------------------------------------------

IF OBJECT_ID('dbo.pub_pubrate_map_activate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pub_pubrate_map_activate'
	DROP TABLE dbo.pub_pubrate_map_activate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pub_pubrate_map_activate') IS NOT NULL
		PRINT '***********Drop of dbo.pub_pubrate_map_activate FAILED.'
END
GO

IF OBJECT_ID('dbo.pub_pubrate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pub_pubrate'
	DROP TABLE dbo.pub_pubrate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pub_pubrate') IS NOT NULL
		PRINT '***********Drop of dbo.pub_pubrate FAILED.'
END
GO

IF OBJECT_ID('dbo.pub_pubquantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pub_pubquantity'
	DROP TABLE dbo.pub_pubquantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pub_pubquantity') IS NOT NULL
		PRINT '***********Drop of dbo.pub_pubquantity FAILED.'
END
GO

IF OBJECT_ID('dbo.pub_pubpubgroup_map') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pub_pubpubgroup_map'
	DROP TABLE dbo.pub_pubpubgroup_map
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pub_pubpubgroup_map') IS NOT NULL
		PRINT '***********Drop of dbo.pub_pubpubgroup_map FAILED.'
END
GO

IF OBJECT_ID('dbo.pst_postalcategoryrate_map') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pst_postalcategoryrate_map'
	DROP TABLE dbo.pst_postalcategoryrate_map
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pst_postalcategoryrate_map') IS NOT NULL
		PRINT '***********Drop of dbo.pst_postalcategoryrate_map FAILED.'
END
GO

IF OBJECT_ID('dbo.prt_printerrate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.prt_printerrate'
	DROP TABLE dbo.prt_printerrate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.prt_printerrate') IS NOT NULL
		PRINT '***********Drop of dbo.prt_printerrate FAILED.'
END
GO
PRINT 'Creating dbo.prt_printerrate'
GO

IF OBJECT_ID('dbo.ppr_paper_map') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ppr_paper_map'
	DROP TABLE dbo.ppr_paper_map
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ppr_paper_map') IS NOT NULL
		PRINT '***********Drop of dbo.ppr_paper_map FAILED.'
END
GO

IF OBJECT_ID('dbo.est_samples') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.est_samples'
	DROP TABLE dbo.est_samples
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.est_samples') IS NOT NULL
		PRINT '***********Drop of dbo.est_samples FAILED.'
END
GO

IF OBJECT_ID('dbo.est_pubissuedates') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.est_pubissuedates'
	DROP TABLE dbo.est_pubissuedates
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.est_pubissuedates') IS NOT NULL
		PRINT '***********Drop of dbo.est_pubissuedates FAILED.'
END
GO

IF OBJECT_ID('dbo.est_package') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.est_package'
	DROP TABLE dbo.est_package
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.est_package') IS NOT NULL
		PRINT '***********Drop of dbo.est_package FAILED.'
END
GO

IF OBJECT_ID('dbo.est_assemdistriboptions') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.est_assemdistriboptions'
	DROP TABLE dbo.est_assemdistriboptions
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.est_assemdistriboptions') IS NOT NULL
		PRINT '***********Drop of dbo.est_assemdistriboptions FAILED.'
END
GO

-----------------------------------------------------------------

IF OBJECT_ID('dbo.vnd_printer') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vnd_printer'
	DROP TABLE dbo.vnd_printer
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vnd_printer') IS NOT NULL
		PRINT '***********Drop of dbo.vnd_printer FAILED.'
END
GO

IF OBJECT_ID('dbo.vnd_paper') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vnd_paper'
	DROP TABLE dbo.vnd_paper
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vnd_paper') IS NOT NULL
		PRINT '***********Drop of dbo.vnd_paper FAILED.'
END
GO

IF OBJECT_ID('dbo.vnd_mailtrackingrate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vnd_mailtrackingrate'
	DROP TABLE dbo.vnd_mailtrackingrate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vnd_mailtrackingrate') IS NOT NULL
		PRINT '***********Drop of dbo.vnd_mailtrackingrate FAILED.'
END
GO

IF OBJECT_ID('dbo.vnd_maillistresourcerate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vnd_maillistresourcerate'
	DROP TABLE dbo.vnd_maillistresourcerate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vnd_maillistresourcerate') IS NOT NULL
		PRINT '***********Drop of dbo.vnd_maillistresourcerate FAILED.'
END
GO

IF OBJECT_ID('dbo.vnd_mailhouserate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vnd_mailhouserate'
	DROP TABLE dbo.vnd_mailhouserate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vnd_mailhouserate') IS NOT NULL
		PRINT '***********Drop of dbo.vnd_mailhouserate FAILED.'
END
GO

IF OBJECT_ID('dbo.vnd_vendorvendortype_map') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vnd_vendorvendortype_map'
	DROP TABLE dbo.vnd_vendorvendortype_map
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vnd_vendorvendortype_map') IS NOT NULL
		PRINT '***********Drop of dbo.vnd_vendorvendortype_map FAILED.'
END
GO

IF OBJECT_ID('dbo.rpt_report') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.rpt_report'
	DROP TABLE dbo.rpt_report
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.rpt_report') IS NOT NULL
		PRINT '***********Drop of dbo.rpt_report FAILED.'
END
GO

-- IF OBJECT_ID('dbo.rpt_parameter') IS NOT NULL
-- BEGIN
-- 	PRINT 'Dropping dbo.rpt_parameter'
-- 	DROP TABLE dbo.rpt_parameter
-- 	-- Tell user if drop failed.
-- 	IF OBJECT_ID('dbo.rpt_parameter') IS NOT NULL
-- 		PRINT '***********Drop of dbo.rpt_parameter FAILED.'
-- END
-- GO

IF OBJECT_ID('dbo.pub_pubrate_map') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pub_pubrate_map'
	DROP TABLE dbo.pub_pubrate_map
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pub_pubrate_map') IS NOT NULL
		PRINT '***********Drop of dbopub_pubrate_map. FAILED.'
END
GO

IF OBJECT_ID('dbo.pub_groupinsertscenario_map') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pub_groupinsertscenario_map'
	DROP TABLE dbo.pub_groupinsertscenario_map
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pub_groupinsertscenario_map') IS NOT NULL
		PRINT '***********Drop of dbo.pub_groupinsertscenario_map FAILED.'
END
GO

IF OBJECT_ID('dbo.pst_postalweights') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pst_postalweights'
	DROP TABLE dbo.pst_postalweights
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pst_postalweights') IS NOT NULL
		PRINT '***********Drop of dbo.pst_postalweights FAILED.'
END
GO

IF OBJECT_ID('dbo.pst_postalscenario') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pst_postalscenario'
	DROP TABLE dbo.pst_postalscenario
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pst_postalscenario') IS NOT NULL
		PRINT '***********Drop of dbo.pst_postalscenario FAILED.'
END
GO

-- IF OBJECT_ID('dbo.est_costcodevendortype_map') IS NOT NULL
-- BEGIN
-- 	PRINT 'Dropping dbo.est_costcodevendortype_map'
-- 	DROP TABLE dbo.est_costcodevendortype_map
-- 	-- Tell user if drop failed.
-- 	IF OBJECT_ID('dbo.est_costcodevendortype_map') IS NOT NULL
-- 		PRINT '***********Drop of dbo.est_costcodevendortype_map FAILED.'
-- END
-- GO

IF OBJECT_ID('dbo.est_estimate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.est_estimate'
	DROP TABLE dbo.est_estimate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.est_estimate') IS NOT NULL
		PRINT '***********Drop of dbo.est_estimate FAILED.'
END
GO

IF OBJECT_ID('dbo.assoc_databases') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.assoc_databases'
	DROP TABLE dbo.assoc_databases
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.assoc_databases') IS NOT NULL
		PRINT '***********Drop of dbo.assoc_databases FAILED.'
END
GO

-----------------------------------------------------------------

IF OBJECT_ID('dbo.vnd_vendortype') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vnd_vendortype'
	DROP TABLE dbo.vnd_vendortype
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vnd_vendortype') IS NOT NULL
		PRINT '***********Drop of dbo.vnd_vendortype FAILED.'
END
GO

IF OBJECT_ID('dbo.vnd_vendor') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vnd_vendor'
	DROP TABLE dbo.vnd_vendor
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vnd_vendor') IS NOT NULL
		PRINT '***********Drop of dbo.vnd_vendor FAILED.'
END
GO

IF OBJECT_ID('dbo.rpt_reporttype') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.rpt_reporttype'
	DROP TABLE dbo.rpt_reporttype
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.rpt_reporttype') IS NOT NULL
		PRINT '***********Drop of dbo.rpt_reporttype FAILED.'
END
GO

-- IF OBJECT_ID('dbo.rpt_parametertype') IS NOT NULL
-- BEGIN
-- 	PRINT 'Dropping dbo.rpt_parametertype'
-- 	DROP TABLE dbo.rpt_parametertype
-- 	-- Tell user if drop failed.
-- 	IF OBJECT_ID('dbo.rpt_parametertype') IS NOT NULL
-- 		PRINT '***********Drop of dbo.rpt_parametertype FAILED.'
-- END
-- GO

IF OBJECT_ID('dbo.pub_ratetype') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pub_ratetype'
	DROP TABLE dbo.pub_ratetype
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pub_ratetype') IS NOT NULL
		PRINT '***********Drop of dbo.pub_ratetype FAILED.'
END
GO

IF OBJECT_ID('dbo.pub_pubquantitytype') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pub_pubquantitytype'
	DROP TABLE dbo.pub_pubquantitytype
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pub_pubquantitytype') IS NOT NULL
		PRINT '***********Drop of dbo.pub_pubquantitytype FAILED.'
END
GO

IF OBJECT_ID('dbo.pub_pubgroup') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pub_pubgroup'
	DROP TABLE dbo.pub_pubgroup
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pub_pubgroup') IS NOT NULL
		PRINT '***********Drop of dbo.pub_pubgroup FAILED.'
END
GO

IF OBJECT_ID('dbo.pub_insertscenario') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pub_insertscenario'
	DROP TABLE dbo.pub_insertscenario
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pub_insertscenario') IS NOT NULL
		PRINT '***********Drop of dbo.pub_insertscenario FAILED.'
END
GO

IF OBJECT_ID('dbo.prt_printerratetype') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.prt_printerratetype'
	DROP TABLE dbo.prt_printerratetype
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.prt_printerratetype') IS NOT NULL
		PRINT '***********Drop of dbo.prt_printerratetype FAILED.'
END
GO

IF OBJECT_ID('dbo.pst_postalmailertype') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pst_postalmailertype'
	DROP TABLE dbo.pst_postalmailertype
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pst_postalmailertype') IS NOT NULL
		PRINT '***********Drop of dbo.pst_postalmailertype FAILED.'
END
GO

IF OBJECT_ID('dbo.pst_postalclass') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pst_postalclass'
	DROP TABLE dbo.pst_postalclass
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pst_postalclass') IS NOT NULL
		PRINT '***********Drop of dbo.pst_postalclass FAILED.'
END
GO

IF OBJECT_ID('dbo.pst_postalcategory') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.pst_postalcategory'
	DROP TABLE dbo.pst_postalcategory
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.pst_postalcategory') IS NOT NULL
		PRINT '***********Drop of dbo.pst_postalcategory FAILED.'
END
GO

IF OBJECT_ID('dbo.ppr_paperweight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ppr_paperweight'
	DROP TABLE dbo.ppr_paperweight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ppr_paperweight') IS NOT NULL
		PRINT '***********Drop of dbo.ppr_paperweight FAILED.'
END
GO

IF OBJECT_ID('dbo.ppr_papergrade') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ppr_papergrade'
	DROP TABLE dbo.ppr_papergrade
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ppr_papergrade') IS NOT NULL
		PRINT '***********Drop of dbo.ppr_papergrade FAILED.'
END
GO

IF OBJECT_ID('dbo.est_status') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.est_status'
	DROP TABLE dbo.est_status
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.est_status') IS NOT NULL
		PRINT '***********Drop of dbo.est_status FAILED.'
END
GO

IF OBJECT_ID('dbo.est_season') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.est_season'
	DROP TABLE dbo.est_season
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.est_season') IS NOT NULL
		PRINT '***********Drop of dbo.est_season FAILED.'
END
GO

IF OBJECT_ID('dbo.est_estimatemediatype') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.est_estimatemediatype'
	DROP TABLE dbo.est_estimatemediatype
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.est_estimatemediatype') IS NOT NULL
		PRINT '***********Drop of dbo.est_estimatemediatype FAILED.'
END
GO

IF OBJECT_ID('dbo.est_costcodes') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.est_costcodes'
	DROP TABLE dbo.est_costcodes
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.est_costcodes') IS NOT NULL
		PRINT '***********Drop of dbo.est_costcodes FAILED.'
END
GO

IF OBJECT_ID('dbo.est_componenttype') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.est_componenttype'
	DROP TABLE dbo.est_componenttype
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.') IS NOT NULL
		PRINT '***********Drop of dbo.est_componenttype FAILED.'
END
GO

IF OBJECT_ID('dbo.assoc_databasetype') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.assoc_databasetype'
	DROP TABLE dbo.assoc_databasetype
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.assoc_databasetype') IS NOT NULL
		PRINT '***********Drop of dbo.assoc_databasetype FAILED.'
END
GO

PRINT 'Completed dropping tables.'
GO

USE [master]

