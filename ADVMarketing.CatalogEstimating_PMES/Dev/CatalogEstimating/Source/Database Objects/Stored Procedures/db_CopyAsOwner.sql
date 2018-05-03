IF OBJECT_ID('dbo.db_CopyAsOwner') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.db_CopyAsOwner'
	DROP PROCEDURE dbo.db_CopyAsOwner
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.db_CopyAsOwner') IS NOT NULL
		PRINT '***********Drop of dbo.db_CopyAsOwner FAILED.'
END
GO
PRINT 'Creating dbo.db_CopyAsOwner'
GO

create proc dbo.db_CopyAsOwner
/*
* PARAMETERS:
*	@SourceDBName
*
* DESCRIPTION:
*	Performs the data copy from SourceDBName
*
* TABLES:
* Table Name Access
* ========== ======
* MANY DELETE
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
* Date		Who	Comments
* ----------	---	-------------------------------------------------
* 09/12/2007	BJS	Initial Creation 
* 11/01/2007	JRH	Added assemblyvendor_id to EST_Component
*
*/
as

DECLARE @SourceDBName varchar(50)
	, @sql nvarchar(4000)

SELECT	@SourceDBName = source_db_name
FROM	dbo.assoc_database_sync

set @sql =
'SET IDENTITY_INSERT ppr_papergrade ON
INSERT INTO ppr_papergrade(ppr_papergrade_id, grade, createdby, createddate, modifiedby, modifieddate)
select ppr_papergrade_id, grade, createdby, createddate, modifiedby, modifieddate
from ' + @SourceDBName + '.dbo.ppr_papergrade
SET IDENTITY_INSERT ppr_papergrade OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT ppr_paperweight ON
INSERT INTO ppr_paperweight(ppr_paperweight_id, weight, createdby, createddate, modifiedby, modifieddate)
select ppr_paperweight_id, weight, createdby, createddate, modifiedby, modifieddate
from ' + @SourceDBName + '.dbo.ppr_paperweight
SET IDENTITY_INSERT ppr_paperweight OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pst_postalcategory ON
INSERT INTO pst_postalcategory(pst_postalcategory_id, description, createdby, createddate, modifiedby, modifieddate)
SELECT pst_postalcategory_id, description, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pst_postalcategory
SET IDENTITY_INSERT pst_postalcategory OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pub_insertscenario ON
INSERT INTO pub_insertscenario(pub_insertscenario_id, description, comments, active, createdby, createddate, modifiedby, modifieddate)
SELECT pub_insertscenario_id, description, comments, active, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pub_insertscenario
SET IDENTITY_INSERT pub_insertscenario OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pub_pubgroup ON
INSERT INTO pub_pubgroup(pub_pubgroup_id, description, comments, active, effectivedate, sortorder, customgroupforpackage, createdby, createddate, modifiedby, modifieddate)
SELECT pub_pubgroup_id, description, comments, active, effectivedate, sortorder, customgroupforpackage, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pub_pubgroup
SET IDENTITY_INSERT pub_pubgroup OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT vnd_vendor ON
INSERT INTO vnd_vendor(vnd_vendor_id, vendorcode, description, active, createdby, createddate, modifiedby, modifieddate)
SELECT vnd_vendor_id, vendorcode, description, active, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.vnd_vendor
SET IDENTITY_INSERT vnd_vendor OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT est_estimate ON
INSERT INTO est_estimate(est_estimate_id, description, comments, est_season_id, fiscalyear, rundate, est_status_id, fiscalmonth, parent_id, uploaddate, createdby, createddate, modifiedby, modifieddate)
SELECT est_estimate_id, description, comments, est_season_id, fiscalyear, rundate, est_status_id, fiscalmonth, parent_id, uploaddate, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.est_estimate
SET IDENTITY_INSERT est_estimate OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pst_postalscenario ON
INSERT INTO pst_postalscenario(pst_postalscenario_id, description, comments, effectivedate, pst_postalmailertype_id, pst_postalclass_id, createdby, createddate, modifiedby, modifieddate)
SELECT pst_postalscenario_id, description, comments, effectivedate, pst_postalmailertype_id, pst_postalclass_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pst_postalscenario
SET IDENTITY_INSERT pst_postalscenario OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pst_postalweights ON
INSERT INTO pst_postalweights(pst_postalweights_id, firstoverweightlimit, standardoverweightlimit, effectivedate, vnd_vendor_id, createdby, createddate, modifiedby, modifieddate)
SELECT pst_postalweights_id, firstoverweightlimit, standardoverweightlimit, effectivedate, vnd_vendor_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pst_postalweights
SET IDENTITY_INSERT pst_postalweights OFF'
exec sp_executesql @sql

set @sql =
'INSERT INTO pub_groupinsertscenario_map
SELECT *
FROM ' + @SourceDBName + '.dbo.pub_groupinsertscenario_map'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pub_pubrate_map ON
INSERT INTO pub_pubrate_map(pub_pubrate_map_id, pub_id, publoc_id, createdby, createddate, modifiedby, modifieddate)
SELECT pub_pubrate_map_id, pub_id, publoc_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pub_pubrate_map
SET IDENTITY_INSERT pub_pubrate_map OFF'
exec sp_executesql @sql

set @sql =
'INSERT INTO vnd_vendorvendortype_map
SELECT *
FROM ' + @SourceDBName + '.dbo.vnd_vendorvendortype_map'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT vnd_mailhouserate ON
INSERT INTO vnd_mailhouserate(vnd_mailhouserate_id, vnd_vendor_id, timevalueslips, inkjetrate, inkjetmakeready, adminfee, postaldropcwt, gluetackdefault, gluetackrate, tabbingdefault, tabbingrate, letterinsertiondefault, letterinsertionrate, effectivedate, createdby, createddate, modifiedby, modifieddate)
SELECT vnd_mailhouserate_id, vnd_vendor_id, timevalueslips, inkjetrate, inkjetmakeready, adminfee, postaldropcwt, gluetackdefault, gluetackrate, tabbingdefault, tabbingrate, letterinsertiondefault, letterinsertionrate, effectivedate, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.vnd_mailhouserate
SET IDENTITY_INSERT vnd_mailhouserate OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT vnd_maillistresourcerate ON
INSERT INTO vnd_maillistresourcerate(vnd_maillistresourcerate_id, vnd_vendor_id, internallistrate, effectivedate, createdby, createddate, modifiedby, modifieddate)
SELECT vnd_maillistresourcerate_id, vnd_vendor_id, internallistrate, effectivedate, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.vnd_maillistresourcerate
SET IDENTITY_INSERT vnd_maillistresourcerate OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT vnd_mailtrackingrate ON
INSERT INTO vnd_mailtrackingrate(vnd_mailtrackingrate_id, vnd_vendor_id, mailtracking, effectivedate, createdby, createddate, modifiedby, modifieddate)
SELECT vnd_mailtrackingrate_id, vnd_vendor_id, mailtracking, effectivedate, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.vnd_mailtrackingrate
SET IDENTITY_INSERT vnd_mailtrackingrate OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT vnd_paper ON
INSERT INTO vnd_paper(vnd_paper_id, effectivedate, vnd_vendor_id, createdby, createddate, modifiedby, modifieddate)
SELECT vnd_paper_id, effectivedate, vnd_vendor_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.vnd_paper
SET IDENTITY_INSERT vnd_paper OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT vnd_printer ON
INSERT INTO vnd_printer(vnd_printer_id, vnd_vendor_id, paperhandling, polybagbagweight, cornerguarddefault, cornerguard, skiddefault, skid, polybagmessagedefault, polybagmessage, polybagmessagemakereadydefault, polybagmessagemakeready, effectivedate, createdby, createddate, modifiedby, modifieddate)
SELECT vnd_printer_id, vnd_vendor_id, paperhandling, polybagbagweight, cornerguarddefault, cornerguard, skiddefault, skid, polybagmessagedefault, polybagmessage, polybagmessagemakereadydefault, polybagmessagemakeready, effectivedate, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.vnd_printer
SET IDENTITY_INSERT vnd_printer OFF'
exec sp_executesql @sql

set @sql =
'INSERT INTO est_assemdistriboptions(est_estimate_id, insertdow, insertfreightvendor_id, insertfreightcwt, insertfuelsurcharge, cornerguards, skids, inserttime, pst_postalscenario_id, mailfuelsurcharge, mailhouse_id, mailhouseotherhandling, usemailtracking, mailtracking_id, maillistresource_id, useexternalmaillist, externalmailqty, externalmailcpm, nbrofcartons, usegluetack, usetabbing, useletterinsertion, firstclass, otherfreight, postaldropflat, createdby, createddate, modifiedby, modifieddate)
SELECT est_estimate_id, insertdow, insertfreightvendor_id, insertfreightcwt, insertfuelsurcharge, cornerguards, skids, inserttime, pst_postalscenario_id, mailfuelsurcharge, mailhouse_id, mailhouseotherhandling, usemailtracking, mailtracking_id, maillistresource_id, useexternalmaillist, externalmailqty, externalmailcpm, nbrofcartons, usegluetack, usetabbing, useletterinsertion, firstclass, otherfreight, postaldropflat, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.est_assemdistriboptions'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT est_package ON
INSERT INTO est_package(est_package_id, est_estimate_id, description, comments, soloquantity, otherquantity, pub_pubquantitytype_id, pub_pubgroup_id, createdby, createddate, modifiedby, modifieddate)
SELECT est_package_id, est_estimate_id, description, comments, soloquantity, otherquantity, pub_pubquantitytype_id, pub_pubgroup_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.est_package
SET IDENTITY_INSERT est_package OFF'
exec sp_executesql @sql

set @sql =
'INSERT INTO est_pubissuedates
SELECT *
FROM ' + @SourceDBName + '.dbo.est_pubissuedates'
exec sp_executesql @sql

set @sql =
'INSERT INTO est_samples
SELECT *
FROM ' + @SourceDBName + '.dbo.est_samples'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT ppr_paper_map ON
INSERT INTO ppr_paper_map(ppr_paper_map_id, description, cwt, [default], ppr_papergrade_id, ppr_paperweight_id, vnd_paper_id, createdby, createddate, modifiedby, modifieddate)
SELECT ppr_paper_map_id, description, cwt, [default], ppr_papergrade_id, ppr_paperweight_id, vnd_paper_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.ppr_paper_map
SET IDENTITY_INSERT ppr_paper_map OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT prt_printerrate ON
INSERT INTO prt_printerrate(prt_printerrate_id, vnd_printer_id, prt_printerratetype_id, rate, description, [default], createdby, createddate, modifiedby, modifieddate)
SELECT prt_printerrate_id, vnd_printer_id, prt_printerratetype_id, rate, description, [default], createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.prt_printerrate
SET IDENTITY_INSERT prt_printerrate OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pst_postalcategoryrate_map ON
INSERT INTO pst_postalcategoryrate_map(pst_postalcategoryrate_map_id, pst_postalmailertype_id, pst_postalclass_id, pst_postalcategory_id, active, underweightpiecerate, overweightpoundrate, overweightpiecerate, pst_postalweights_id, createdby, createddate, modifiedby, modifieddate)
SELECT pst_postalcategoryrate_map_id, pst_postalmailertype_id, pst_postalclass_id, pst_postalcategory_id, active, underweightpiecerate, overweightpoundrate, overweightpiecerate, pst_postalweights_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pst_postalcategoryrate_map
SET IDENTITY_INSERT pst_postalcategoryrate_map OFF'
exec sp_executesql @sql

set @sql =
'INSERT INTO pub_pubpubgroup_map
SELECT *
FROM ' + @SourceDBName + '.dbo.pub_pubpubgroup_map'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pub_pubquantity ON
INSERT INTO pub_pubquantity(pub_pubquantity_id, effectivedate, pub_pubrate_map_id, createdby, createddate, modifiedby, modifieddate)
SELECT pub_pubquantity_id, effectivedate, pub_pubrate_map_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pub_pubquantity
SET IDENTITY_INSERT pub_pubquantity OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pub_pubrate ON
INSERT INTO pub_pubrate(pub_pubrate_id, pub_ratetype_id, chargeblowin, blowinrate, effectivedate, quantitychargetype, billedpct, pub_pubrate_map_id, createdby, createddate, modifiedby, modifieddate)
SELECT pub_pubrate_id, pub_ratetype_id, chargeblowin, blowinrate, effectivedate, quantitychargetype, billedpct, pub_pubrate_map_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pub_pubrate
SET IDENTITY_INSERT pub_pubrate OFF'
exec sp_executesql @sql

set @sql =
'INSERT INTO pub_pubrate_map_activate
SELECT *
FROM ' + @SourceDBName + '.dbo.pub_pubrate_map_activate'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT est_component ON
INSERT INTO est_component(est_component_id, est_estimate_id, description, comments, financialchangecomment, adnumber, est_estimatemediatype_id, est_componenttype_id, mediaqtywoinsert, spoilagepct, pagecount, width, height, otherproduction, vendorsupplied, vendorsupplied_id, vendorcpm, creativevendor_id, creativecpp, separator_id, separatorcpp, printer_id, calculateprintcost, printcost, numberofplants, additionalplates, platecost_id, replacementplatecost, runrate, numberdigitalhandlenprepare, digitalhandlenprepare_id, stitchin_id, blowin_id, onsert_id, stitchermakeready_id, stitchermakereadyrate, pressmakeready_id, pressmakereadyrate, earlypayprintdiscount, printerapplytax, printertaxablemediapct, printersalestaxpct, paper_id, paper_map_id, paperweight_id, papergrade_id, calculatepapercost, papercost, runpounds, makereadypounds, platechangepounds, pressstoppounds, numberofpressstops, earlypaypaperdiscount, paperapplytax, papertaxablemediapct, papersalestaxpct, createdby, createddate, modifiedby, modifieddate, assemblyvendor_id)
SELECT est_component_id, est_estimate_id, description, comments, financialchangecomment, adnumber, est_estimatemediatype_id, est_componenttype_id, mediaqtywoinsert, spoilagepct, pagecount, width, height, otherproduction, vendorsupplied, vendorsupplied_id, vendorcpm, creativevendor_id, creativecpp, separator_id, separatorcpp, printer_id, calculateprintcost, printcost, numberofplants, additionalplates, platecost_id, replacementplatecost, runrate, numberdigitalhandlenprepare, digitalhandlenprepare_id, stitchin_id, blowin_id, onsert_id, stitchermakeready_id, stitchermakereadyrate, pressmakeready_id, pressmakereadyrate, earlypayprintdiscount, printerapplytax, printertaxablemediapct, printersalestaxpct, paper_id, paper_map_id, paperweight_id, papergrade_id, calculatepapercost, papercost, runpounds, makereadypounds, platechangepounds, pressstoppounds, numberofpressstops, earlypaypaperdiscount, paperapplytax, papertaxablemediapct, papersalestaxpct, createdby, createddate, modifiedby, modifieddate, assemblyvendor_id
FROM ' + @SourceDBName + '.dbo.est_component
SET IDENTITY_INSERT est_component OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT est_polybaggroup ON
INSERT INTO est_polybaggroup(est_polybaggroup_id, description, comments, vnd_printer_id, prt_bagrate_id, prt_bagmakereadyrate_id, usemessage, createdby, createddate, modifiedby, modifieddate)
SELECT est_polybaggroup_id, description, comments, vnd_printer_id, prt_bagrate_id, prt_bagmakereadyrate_id, usemessage, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.est_polybaggroup
SET IDENTITY_INSERT est_polybaggroup OFF'
exec sp_executesql @sql

set @sql =
'INSERT INTO pst_postalcategoryscenario_map(pst_postalscenario_id, pst_postalcategoryrate_map_id, percentage, createdby, createddate, modifiedby, modifieddate)
SELECT pst_postalscenario_id, pst_postalcategoryrate_map_id, percentage, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pst_postalcategoryscenario_map'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pub_dayofweekquantity ON
INSERT INTO pub_dayofweekquantity(pub_dayofweekquantity_id, pub_pubquantity_id, pub_pubquantitytype_id, insertdow, quantity, createdby, createddate, modifiedby, modifieddate)
SELECT pub_dayofweekquantity_id, pub_pubquantity_id, pub_pubquantitytype_id, insertdow, quantity, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pub_dayofweekquantity
SET IDENTITY_INSERT pub_dayofweekquantity OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pub_dayofweekratetypes ON
INSERT INTO pub_dayofweekratetypes(pub_dayofweekratetypes_id, ratetypedescription, pub_pubrate_id, createdby, createddate, modifiedby, modifieddate)
SELECT pub_dayofweekratetypes_id, ratetypedescription, pub_pubrate_id, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pub_dayofweekratetypes
SET IDENTITY_INSERT pub_dayofweekratetypes OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pub_insertdiscounts ON
INSERT INTO pub_insertdiscounts(pub_insertdiscount_id, pub_pubrate_id, [insert], discount, createdby, createddate, modifiedby, modifieddate)
SELECT pub_insertdiscount_id, pub_pubrate_id, [insert], discount, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pub_insertdiscounts
SET IDENTITY_INSERT pub_insertdiscounts OFF'
exec sp_executesql @sql

set @sql =
'INSERT INTO est_estimatepolybaggroup_map
SELECT *
FROM ' + @SourceDBName + '.dbo.est_estimatepolybaggroup_map'
exec sp_executesql @sql

set @sql =
'INSERT INTO est_packagecomponentmapping
SELECT *
FROM ' + @SourceDBName + '.dbo.est_packagecomponentmapping'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT est_polybag ON
INSERT INTO est_polybag(est_polybag_id, est_polybaggroup_id, pst_postalscenario_id, quantity, createdby, createddate, modifiedby, modifieddate)
SELECT est_polybag_id, est_polybaggroup_id, pst_postalscenario_id, quantity, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.est_polybag
SET IDENTITY_INSERT est_polybag OFF'
exec sp_executesql @sql

set @sql =
'SET IDENTITY_INSERT pub_dayofweekrates ON
INSERT INTO pub_dayofweekrates(pub_dayofweekrates_id, pub_dayofweekratetypes_id, rate, insertdow, createdby, createddate, modifiedby, modifieddate)
SELECT pub_dayofweekrates_id, pub_dayofweekratetypes_id, rate, insertdow, createdby, createddate, modifiedby, modifieddate
FROM ' + @SourceDBName + '.dbo.pub_dayofweekrates
SET IDENTITY_INSERT pub_dayofweekrates OFF'
exec sp_executesql @sql

set @sql =
'INSERT INTO est_packagepolybag_map
SELECT *
FROM ' + @SourceDBName + '.dbo.est_packagepolybag_map'
exec sp_executesql @sql

DELETE	dbo.assoc_database_sync

GO

GRANT EXECUTE ON [dbo].[db_CopyAsOwner] TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
