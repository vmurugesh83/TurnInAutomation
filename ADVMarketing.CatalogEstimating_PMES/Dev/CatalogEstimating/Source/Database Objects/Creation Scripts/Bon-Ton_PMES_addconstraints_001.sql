-- USE [BonTon_PMES_LIVE]
-- USE [BonTon_PMES_SEASONAL]
-- USE [BonTon_PMES_TEST1]
-- USE [BonTon_PMES_TEST2]
-- USE [BonTon_PMES_TEST3]
-- USE [BonTon_PMES_TEST4]

-----------------------------------------------------------------
-- Primary Keys
-----------------------------------------------------------------
PRINT 'Create Primary Keys'
go
PRINT '		Group #1'
go

alter table dbo.[assoc_databasetype] add constraint [assoc_databasetype_PK] primary key ([databasetype_id])
go
alter table dbo.[assoc_database_sync] add constraint [assoc_database_sync_PK] primary key ([destination_db_name])
go
alter table dbo.[adpub_cost] add constraint [adpub_cost_PK] primary key clustered ([adnumber], [pub_id], [publoc_id], [rundate]) 
go
alter table dbo.[directmail_cost] add constraint [directmail_cost_PK] primary key clustered ([adnumber]) 
go
alter table dbo.[est_componenttype] add constraint [est_componenttype_PK] primary key ([est_componenttype_id])
go
alter table dbo.[est_costcodes] add constraint [est_costcodes_PK] primary key ([costcode])
go
alter table dbo.[est_estimatemediatype] add constraint [est_estimatemediatype_PK] primary key ([est_estimatemediatype_id])
go
alter table dbo.[est_season] add constraint [est_season_PK] primary key ([est_season_id])
go
alter table dbo.[est_status] add constraint [est_status_PK] primary key ([est_status_id])
go
alter table dbo.[ppr_papergrade] add constraint [ppr_papergrade_PK] primary key ([ppr_papergrade_id])
go
alter table dbo.[ppr_paperweight] add constraint [ppr_paperweight_PK] primary key ([ppr_paperweight_id])
go
alter table dbo.[pst_postalcategory] add constraint [pst_postalcategory_PK] primary key ([pst_postalcategory_id])
go
alter table dbo.[pst_postalclass] add constraint [pst_postalclass_PK] primary key ([pst_postalclass_id])
go
alter table dbo.[pst_postalmailertype] add constraint [pst_postalmailertype_PK] primary key ([pst_postalmailertype_id])
go
alter table dbo.[prt_printerratetype] add constraint [prt_printerratetype_PK] primary key ([prt_printerratetype_id])
go
alter table dbo.[pub_insertscenario] add constraint [pub_insertscenario_PK] primary key ([pub_insertscenario_id])
go
alter table dbo.[pub_pubgroup] add constraint [pub_pubgroup_PK] primary key ([pub_pubgroup_id])
go
alter table dbo.[pub_pubquantitytype] add constraint [pub_pubquantitytype_PK] primary key ([pub_pubquantitytype_id])
go
alter table dbo.[pub_ratetype] add constraint [pub_ratetype_PK] primary key ([pub_ratetype_id])
go
-- alter table dbo.[rpt_parametertype] add constraint [rpt_parametertype_PK] primary key ([rpt_parametertype_id])
-- go
alter table dbo.[rpt_reporttype] add constraint [rpt_reporttype_PK] primary key ([rpt_reporttype_id])
go
alter table dbo.[vnd_vendor] add constraint [vnd_vendor_PK] primary key ([vnd_vendor_id])
go
alter table dbo.[vnd_vendortype] add constraint [vnd_vendortype_PK] primary key ([vnd_vendortype_id])
go
-----------------------------------------------------------------
PRINT '		Group #2'
go

alter table dbo.[assoc_databases] add constraint [assoc_databases_PK] primary key ([database_id])
go
alter table dbo.[est_estimate] add constraint [est_estimate_PK] primary key ([est_estimate_id])
go
-- alter table dbo.[est_costcodevendortype_map] add constraint [est_costcodevendortype_map_PK] primary key ([vnd_vendortype_id], [est_costcode_id])
-- go
alter table dbo.[pst_postalscenario] add constraint [pst_postalscenario_PK] primary key ([pst_postalscenario_id])
go
alter table dbo.[pst_postalweights] add constraint [pst_postalweights_PK] primary key ([pst_postalweights_id])
go
alter table dbo.[pub_groupinsertscenario_map] add constraint [pub_groupinsertscenario_map_PK] primary key ([pubgroupdescription], [pub_insertscenario_id])
go
alter table dbo.[pub_pubrate_map] add constraint [pub_pubrate_map_PK] primary key ([pub_pubrate_map_id])
go
alter table dbo.[rpt_report] add constraint [rpt_report_PK1] primary key ([rpt_report_id])
go
alter table dbo.[vnd_vendorvendortype_map] add constraint [vnd_vendorvendortype_map_PK] primary key ([vnd_vendortype_id], [vnd_vendor_id])
go
alter table dbo.[vnd_mailhouserate] add constraint [vnd_mailhouserate_PK] primary key ([vnd_mailhouserate_id])
go
alter table dbo.[vnd_maillistresourcerate] add constraint [vnd_maillistresourcerate_PK] primary key ([vnd_maillistresourcerate_id])
go
alter table dbo.[vnd_mailtrackingrate] add constraint [vnd_mailtrackingrate_PK] primary key ([vnd_mailtrackingrate_id])
go
alter table dbo.[vnd_paper] add constraint [vnd_paper_PK] primary key ([vnd_paper_id])
go
alter table dbo.[vnd_printer] add constraint [vnd_printer_PK] primary key ([vnd_printer_id])
go
-----------------------------------------------------------------
PRINT '		Group #3'
go

alter table dbo.[est_assemdistriboptions] add constraint [est_assemdistriboptions_PK] primary key ([est_estimate_id]) -- ([est_assemdistriboptions_id])
go
alter table dbo.[est_package] add constraint [est_package_PK] primary key ([est_package_id])
go
alter table dbo.[est_pubissuedates] add constraint [est_pubissuedates_PK] primary key ([est_estimate_id], [pub_pubrate_map_id])
go
alter table dbo.[est_samples] add constraint [est_samples_PK] primary key ([est_estimate_id])  -- ([est_sample_id])
go
alter table dbo.[ppr_paper_map] add constraint [ppr_paper_map_PK] primary key ([ppr_paper_map_id])
go
alter table dbo.[prt_printerrate] add constraint [prt_printerrate_PK] primary key ([prt_printerrate_id])
go
alter table dbo.[pst_postalcategoryrate_map] add constraint [pst_postalcategoryrate_map_PK] primary key ([pst_postalcategoryrate_map_id])
go
alter table dbo.[pub_pubpubgroup_map] add constraint [pub_pubpubgroup_map_PK] primary key ([pub_pubrate_map_id], [pub_pubgroup_id])
go
alter table dbo.[pub_pubquantity] add constraint [pub_pubquantity_PK] primary key ([pub_pubquantity_id])
go
alter table dbo.[pub_pubrate] add constraint [pub_pubrate_PK] primary key ([pub_pubrate_id])
go
alter table dbo.[pub_pubrate_map_activate] add constraint [pub_pubrate_map_activate_PK] primary key ([pub_pubrate_map_id], [effectivedate])
go
-----------------------------------------------------------------
PRINT '		Group #4'
go

alter table dbo.[est_component] add constraint [est_component_PK] primary key ([est_component_id])
go
alter table dbo.[est_polybaggroup] add constraint [est_polybaggroup_PK] primary key ([est_polybaggroup_id])
go
alter table dbo.[pst_postalcategoryscenario_map] add constraint [pst_postalcategoryscenario_map_PK] primary key ([pst_postalcategoryrate_map_id], [pst_postalscenario_id])
go
alter table dbo.[pub_dayofweekquantity] add constraint [pub_dayofweekquantity_PK] primary key ([pub_dayofweekquantity_id])
go
alter table dbo.[pub_dayofweekratetypes] add constraint [pub_dayofweekratetypes_PK] primary key ([pub_dayofweekratetypes_id])
go
alter table dbo.[pub_insertdiscounts] add constraint [pub_insertdiscounts_PK] primary key ([pub_insertdiscount_id])
go
-----------------------------------------------------------------
PRINT '		Group #5'
go

alter table dbo.[est_estimatepolybaggroup_map] add constraint [est_estimatepolybaggroup_map_PK] primary key ([est_estimate_id], [est_polybaggroup_id])
go
alter table dbo.[est_packagecomponentmapping] add constraint [est_packagecomponentmapping_PK] primary key ([est_package_id], [est_component_id])
go
alter table dbo.[est_polybag] add constraint [est_polybag_PK] primary key ([est_polybag_id])
go
alter table dbo.[pub_dayofweekrates] add constraint [pub_dayofweekrates_PK] primary key ([pub_dayofweekrates_id])
go
-----------------------------------------------------------------
PRINT '		Group #6'
go

alter table dbo.[est_packagepolybag_map] add constraint [est_packagepolybag_map_PK] primary key ([est_package_id], [est_polybag_id])
go


-----------------------------------------------------------------
-----------------------------------------------------------------
-- Foreign Keys
-----------------------------------------------------------------
PRINT 'Create Foreign Keys'
go
PRINT '		Group #1 has no foreign keys'
go
PRINT '		Group #2'
go

alter table dbo.[assoc_databases] add constraint [assoc_databases_assoc_databasetype_FK1] foreign key ([databasetype_id])
	references dbo.[assoc_databasetype] ([databasetype_id]) 
	on update no action on delete no action
go
alter table dbo.[est_estimate] add constraint [est_status_est_estimate_FK1] foreign key ([est_status_id])
	references dbo.[est_status] ([est_status_id]) 
	on update no action on delete no action
go
alter table dbo.[est_estimate] add constraint [est_season_est_estimate_FK2] foreign key ([est_season_id])
	references dbo.[est_season] ([est_season_id]) 
	on update no action on delete no action
go
alter table dbo.[est_estimate] add constraint [est_estimate_est_estimate_FK3] foreign key ([parent_id])
	references dbo.[est_estimate] ([est_estimate_id]) 
	on update no action on delete no action
go
----------
-- alter table dbo.[est_costcodevendortype_map] add constraint [est_costcodes_est_costcodevendortype_map_FK1] foreign key ([est_costcode_id])
-- 	references dbo.[est_costcodes] ([est_costcode_id]) 
-- 	on update no action on delete no action
-- go
-- alter table dbo.[est_costcodevendortype_map] add constraint [vnd_vendortype_est_costcodevendortype_map_FK2] foreign key ([vnd_vendortype_id])
-- 	references dbo.[vnd_vendortype] ([vnd_vendortype_id]) 
-- 	on update no action on delete no action
-- go
----------
alter table dbo.[pst_postalscenario] add constraint [pst_postalmailertype_pst_postalscenario_FK1] foreign key ([pst_postalmailertype_id])
	references dbo.[pst_postalmailertype] ([pst_postalmailertype_id]) 
	on update no action on delete no action
go
alter table dbo.[pst_postalscenario] add constraint [pst_postalclass_pst_postalscenario_FK2] foreign key ([pst_postalclass_id])
	references dbo.[pst_postalclass] ([pst_postalclass_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[pst_postalweights] add constraint [vnd_vendor_pst_postalweights_FK1] foreign key ([vnd_vendor_id])
	references dbo.[vnd_vendor] ([vnd_vendor_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[pub_groupinsertscenario_map] add constraint [pub_insertscenario_pub_groupinsertscenario_map_FK1] foreign key ([pub_insertscenario_id])
	references dbo.[pub_insertscenario] ([pub_insertscenario_id]) 
	on update no action on delete no action
go

--alter table dbo.[pub_groupinsertscenario_map] add constraint [pub_pubgroup_pub_groupinsertscenario_map_FK2] foreign key ([pub_pubgroup_id])
--	references dbo.[pub_pubgroup] ([pub_pubgroup_id]) 
--	on update no action on delete no action
--go
----------
-- alter table dbo.[pub_pubrate_map] add constraint [Imported Pub Table_pub_pubrate_map_FK1] foreign key ([pub_id], [publoc_id])
-- 	references dbo.[Imported Pub Table] ([pub_id], [publoc_id]) 
-- 	on update no action on delete no action
-- go
----------
-- alter table dbo.[rpt_parameter] add constraint [rpt_parametertype_rpt_parameter_FK1] foreign key ([rpt_parametertype_id])
-- 	references dbo.[rpt_parametertype] ([rpt_parametertype_id]) 
-- 	on update no action on delete no action
-- go
----------
alter table dbo.[rpt_report] add constraint [rpt_reporttype_rpt_report_FK1] foreign key ([rpt_reporttype_id])
	references dbo.[rpt_reporttype] ([rpt_reporttype_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[vnd_vendorvendortype_map] add constraint [vnd_vendor_vnd_vendorvendortype_map_FK1] foreign key ([vnd_vendor_id])
	references dbo.[vnd_vendor] ([vnd_vendor_id]) 
	on update no action on delete no action
go
alter table dbo.[vnd_vendorvendortype_map] add constraint [vnd_vendortype_vnd_vendorvendortype_map_FK2] foreign key ([vnd_vendortype_id])
	references dbo.[vnd_vendortype] ([vnd_vendortype_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[vnd_mailhouserate] add constraint [vnd_vendor_vnd_mailhouserate_FK1] foreign key ([vnd_vendor_id])
	references dbo.[vnd_vendor] ([vnd_vendor_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[vnd_maillistresourcerate] add constraint [vnd_vendor_vnd_maillistresourcerate_FK1] foreign key ([vnd_vendor_id])
	references dbo.[vnd_vendor] ([vnd_vendor_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[vnd_mailtrackingrate] add constraint [vnd_vendor_vnd_mailtrackingrate_FK1] foreign key ([vnd_vendor_id])
	references dbo.[vnd_vendor] ([vnd_vendor_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[vnd_paper] add constraint [vnd_vendor_vnd_paper_FK1] foreign key ([vnd_vendor_id])
	references dbo.[vnd_vendor] ([vnd_vendor_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[vnd_printer] add constraint [vnd_vendor_vnd_printer_FK1] foreign key ([vnd_vendor_id])
	references dbo.[vnd_vendor] ([vnd_vendor_id]) 
	on update no action on delete no action
go
-----------------------------------------------------------------
PRINT '		Group #3'
go

alter table dbo.[est_assemdistriboptions] add constraint [est_estimate_est_assemdistriboptions_FK1] foreign key ([est_estimate_id])
	references dbo.[est_estimate] ([est_estimate_id]) 
	on update no action on delete no action
go
alter table dbo.[est_assemdistriboptions] add constraint [pst_postalscenario_est_assemdistriboptions_FK2] foreign key ([pst_postalscenario_id])
	references dbo.[pst_postalscenario] ([pst_postalscenario_id]) 
	on update no action on delete no action
go
alter table dbo.[est_assemdistriboptions] add constraint [vnd_mailhouserate_est_assemdistriboptions_FK3] foreign key ([mailhouse_id])
	references dbo.[vnd_mailhouserate] ([vnd_mailhouserate_id]) 
	on update no action on delete no action
go
alter table dbo.[est_assemdistriboptions] add constraint [vnd_mailtrackingrate_est_assemdistriboptions_FK4] foreign key ([mailtracking_id])
	references dbo.[vnd_mailtrackingrate] ([vnd_mailtrackingrate_id]) 
	on update no action on delete no action
go
alter table dbo.[est_assemdistriboptions] add constraint [vnd_maillistresourcerate_est_assemdistriboptions_FK5] foreign key ([maillistresource_id])
	references dbo.[vnd_maillistresourcerate] ([vnd_maillistresourcerate_id])
	on update no action on delete no action
go
----------
alter table dbo.[est_package] add constraint [est_estimate_est_package_FK1] foreign key ([est_estimate_id])
	references dbo.[est_estimate] ([est_estimate_id]) 
	on update no action on delete no action
go
alter table dbo.[est_package] add constraint [pub_pubgroup_est_package_FK2] foreign key ([pub_pubgroup_id])
	references dbo.[pub_pubgroup] ([pub_pubgroup_id]) 
	on update no action on delete no action
go
alter table dbo.[est_package] add constraint [pub_pubquantitytype_est_package_FK3] foreign key ([pub_pubquantitytype_id])
	references dbo.[pub_pubquantitytype] ([pub_pubquantitytype_id]) 
	on update no action on delete no action
go
alter table dbo.[est_package] add constraint [pub_insertscenario_est_package_FK4] foreign key ([pub_insertscenario_id])
	references dbo.[pub_insertscenario] ([pub_insertscenario_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[est_pubissuedates] add constraint [est_estimate_est_pubissuedates_FK1] foreign key ([est_estimate_id])
	references dbo.[est_estimate] ([est_estimate_id]) 
	on update no action on delete no action
go
alter table dbo.[est_pubissuedates] add constraint [pub_pubrate_map_est_pubissuedates_FK2] foreign key ([pub_pubrate_map_id])
	references dbo.[pub_pubrate_map] ([pub_pubrate_map_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[est_samples] add constraint [est_estimate_est_samples_FK1] foreign key ([est_estimate_id])
	references dbo.[est_estimate] ([est_estimate_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[ppr_paper_map] add constraint [ppr_papergrade_ppr_paper_map_FK1] foreign key ([ppr_papergrade_id])
	references dbo.[ppr_papergrade] ([ppr_papergrade_id]) 
	on update no action on delete no action
go
alter table dbo.[ppr_paper_map] add constraint [ppr_paperweight_ppr_paper_map_FK2] foreign key ([ppr_paperweight_id])
	references dbo.[ppr_paperweight] ([ppr_paperweight_id]) 
	on update no action on delete no action
go
alter table dbo.[ppr_paper_map] add constraint [vnd_paper_ppr_paper_map_FK3] foreign key ([vnd_paper_id])
	references dbo.[vnd_paper] ([vnd_paper_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[prt_printerrate] add constraint [vnd_printer_prt_printerrate_FK1] foreign key ([vnd_printer_id])
	references dbo.[vnd_printer] ([vnd_printer_id]) 
	on update no action on delete no action
go
alter table dbo.[prt_printerrate] add constraint [prt_printerratetype_prt_printerrate_FK2] foreign key ([prt_printerratetype_id])
	references dbo.[prt_printerratetype] ([prt_printerratetype_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[pst_postalcategoryrate_map] add constraint [pst_postalmailertype_pst_postalcategoryrate_map_FK1] foreign key ([pst_postalmailertype_id])
	references dbo.[pst_postalmailertype] ([pst_postalmailertype_id]) 
	on update no action on delete no action
go
alter table dbo.[pst_postalcategoryrate_map] add constraint [pst_postalclass_pst_postalcategoryrate_map_FK2] foreign key ([pst_postalclass_id])
	references dbo.[pst_postalclass] ([pst_postalclass_id]) 
	on update no action on delete no action
go
alter table dbo.[pst_postalcategoryrate_map] add constraint [pst_postalcategory_pst_postalcategoryrate_map_FK3] foreign key ([pst_postalcategory_id])
	references dbo.[pst_postalcategory] ([pst_postalcategory_id]) 
	on update no action on delete no action
go
alter table dbo.[pst_postalcategoryrate_map] add constraint [pst_postalweights_pst_postalcategoryrate_map_FK4] foreign key ([pst_postalweights_id])
	references dbo.[pst_postalweights] ([pst_postalweights_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[pub_pubpubgroup_map] add constraint [pub_pubrate_map_pub_pubpubgroup_map_FK1] foreign key ([pub_pubrate_map_id])
	references dbo.[pub_pubrate_map] ([pub_pubrate_map_id]) 
	on update no action on delete no action
go
alter table dbo.[pub_pubpubgroup_map] add constraint [pub_pubgroup_pub_pubpubgroup_map_FK2] foreign key ([pub_pubgroup_id])
	references dbo.[pub_pubgroup] ([pub_pubgroup_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[pub_pubquantity] add constraint [pub_pubrate_map_pub_pubquantity_FK1] foreign key ([pub_pubrate_map_id])
	references dbo.[pub_pubrate_map] ([pub_pubrate_map_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[pub_pubrate] add constraint [pub_pubrate_map_pub_pubrate_FK1] foreign key ([pub_pubrate_map_id])
	references dbo.[pub_pubrate_map] ([pub_pubrate_map_id]) 
	on update no action on delete no action
go
alter table dbo.[pub_pubrate] add constraint [pub_ratetype_pub_pubrate_FK2] foreign key ([pub_ratetype_id])
	references dbo.[pub_ratetype] ([pub_ratetype_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[pub_pubrate_map_activate] add constraint [pub_pubrate_map_pub_pubrate_map_activate_FK1] foreign key ([pub_pubrate_map_id])
	references dbo.[pub_pubrate_map] ([pub_pubrate_map_id]) 
	on update no action on delete no action
go
-----------------------------------------------------------------
PRINT '		Group #4'
go

alter table dbo.[est_component] add constraint [est_componenttype_est_component_FK1] foreign key ([est_componenttype_id])
	references dbo.[est_componenttype] ([est_componenttype_id]) 
	on update no action on delete no action
go
alter table dbo.[est_component] add constraint [est_estimatemediatype_est_component_FK2] foreign key ([est_estimatemediatype_id])
	references dbo.[est_estimatemediatype] ([est_estimatemediatype_id]) 
	on update no action on delete no action
go
alter table dbo.[est_component] add constraint [est_estimate_est_component_FK3] foreign key ([est_estimate_id])
	references dbo.[est_estimate] ([est_estimate_id]) 
	on update no action on delete no action
go
alter table dbo.[est_component] add constraint [vnd_vendor_est_component_FK4] foreign key ([vendorsupplied_id])
	references dbo.[vnd_vendor] ([vnd_vendor_id]) 
	on update no action on delete no action
go
alter table dbo.[est_component] add constraint [vnd_vendor_est_component_FK5] foreign key ([creativevendor_id])
	references dbo.[vnd_vendor] ([vnd_vendor_id]) 
	on update no action on delete no action
go
alter table dbo.[est_component] add constraint [vnd_printer_est_component_FK6] foreign key ([printer_id])
	references dbo.[vnd_printer] ([vnd_printer_id]) 
	on update no action on delete no action
go
alter table dbo.[est_component] add constraint [vnd_paper_est_component_FK7] foreign key ([paper_id])
	references dbo.[vnd_paper] ([vnd_paper_id]) 
	on update no action on delete no action
go
alter table dbo.[est_component] add constraint [ppr_paperweight_est_component_FK8] foreign key ([paperweight_id])
	references dbo.[ppr_paperweight] ([ppr_paperweight_id]) 
	on update no action on delete no action
go
alter table dbo.[est_component] add constraint [ppr_papergrade_est_component_FK9] foreign key ([papergrade_id])
	references dbo.[ppr_papergrade] ([ppr_papergrade_id]) 
	on update no action on delete no action
go
alter table dbo.[est_component] add constraint [vnd_vendor_est_component_FK10] foreign key ([separator_id])
	references dbo.[vnd_vendor] ([vnd_vendor_id])
	on update no action on delete no action
go

alter table dbo.[est_component] add constraint [prt_printerrate_est_component_FK11] foreign key ([stitchin_id])
	references dbo.[prt_printerrate] ([prt_printerrate_id]) 
	on update no action on delete no action
go
alter table dbo.[est_component] add constraint [prt_printerrate_est_component_FK12] foreign key ([blowin_id])
	references dbo.[prt_printerrate] ([prt_printerrate_id]) 
	on update no action on delete no action
go
alter table dbo.[est_component] add constraint [ppr_paper_map_est_component_FK13] foreign key ([paper_map_id])
	references dbo.[ppr_paper_map] ([ppr_paper_map_id]) 
	on update no action on delete no action
go

/*8/14/07 - Added*/
alter table dbo.[est_component] add constraint [prt_printerrate_est_component_FK15] foreign key ([onsert_id])
	references dbo.[prt_printerrate] ([prt_printerrate_id])
	on update no action on delete no action
go

/*9/24/07 - Added*/
ALTER TABLE dbo.[est_component] ADD CONSTRAINT [prt_printerrate_est_component_FK16] FOREIGN KEY ([platecost_id])
	REFERENCES dbo.[prt_printerrate] ([prt_printerrate_id])
	ON UPDATE no action
	ON DELETE no action
go

ALTER TABLE dbo.[est_component] ADD CONSTRAINT [prt_printerrate_est_component_FK17] FOREIGN KEY ([stitchermakeready_id])
	REFERENCES dbo.[prt_printerrate] ([prt_printerrate_id])
	ON UPDATE no action
	ON DELETE no action
go

ALTER TABLE dbo.[est_component] ADD CONSTRAINT [prt_printerrate_est_component_FK18] FOREIGN KEY ([pressmakeready_id])
	REFERENCES dbo.[prt_printerrate] ([prt_printerrate_id])
	ON UPDATE no action
	ON DELETE no action
go

ALTER TABLE dbo.[est_component] ADD CONSTRAINT [vnd_printer_est_component_FK19] FOREIGN KEY ([assemblyvendor_id])
	REFERENCES dbo.[vnd_printer] ([vnd_printer_id])
	ON UPDATE no action
	ON DELETE no action
go

----------
alter table dbo.[est_polybaggroup] add constraint [_est_polybaggroup_FK1] foreign key ([vnd_printer_id])
	references dbo.[vnd_printer] ([vnd_printer_id]) 
	on update no action on delete no action
go
alter table dbo.[est_polybaggroup] add constraint [_est_polybaggroup_FK2] foreign key ([prt_bagrate_id])
	references dbo.[prt_printerrate] ([prt_printerrate_id]) 
	on update no action on delete no action
go
alter table dbo.[est_polybaggroup] add constraint [_est_polybaggroup_FK3] foreign key ([prt_bagmakereadyrate_id])
	references dbo.[prt_printerrate] ([prt_printerrate_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[pst_postalcategoryscenario_map] add constraint [pst_postalscenario_pst_postalcategoryscenario_map_FK1] foreign key ([pst_postalscenario_id])
	references dbo.[pst_postalscenario] ([pst_postalscenario_id]) 
	on update no action on delete no action
go
alter table dbo.[pst_postalcategoryscenario_map] add constraint [pst_postalcategoryrate_map_pst_postalcategoryscenario_map_FK2] foreign key ([pst_postalcategoryrate_map_id])
	references dbo.[pst_postalcategoryrate_map] ([pst_postalcategoryrate_map_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[pub_dayofweekquantity] add constraint [pub_pubquantity_pub_dayofweekquantity_FK1] foreign key ([pub_pubquantity_id])
	references dbo.[pub_pubquantity] ([pub_pubquantity_id]) 
	on update no action on delete no action
go
alter table dbo.[pub_dayofweekquantity] add constraint [pub_pubquantitytype_pub_dayofweekquantity_FK2] foreign key ([pub_pubquantitytype_id])
	references dbo.[pub_pubquantitytype] ([pub_pubquantitytype_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[pub_dayofweekratetypes] add constraint [pub_pubrate_pub_dayofweekratetypes_FK1] foreign key ([pub_pubrate_id])
	references dbo.[pub_pubrate] ([pub_pubrate_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[pub_insertdiscounts] add constraint [pub_pubrate_pub_insertdiscounts_FK1] foreign key ([pub_pubrate_id])
	references dbo.[pub_pubrate] ([pub_pubrate_id]) 
	on update no action on delete no action
go
-----------------------------------------------------------------
PRINT '		Group #5'
go

alter table dbo.[est_estimatepolybaggroup_map] add constraint [est_estimate_est_estimatepolybaggroup_map_FK1] foreign key ([est_estimate_id])
	references dbo.[est_estimate] ([est_estimate_id]) 
	on update no action on delete no action
go
alter table dbo.[est_estimatepolybaggroup_map] add constraint [est_polybaggroup_est_estimatepolybaggroup_map_FK2] foreign key ([est_polybaggroup_id])
	references dbo.[est_polybaggroup] ([est_polybaggroup_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[est_packagecomponentmapping] add constraint [est_package_est_packagecomponentmapping_FK1] foreign key ([est_package_id])
	references dbo.[est_package] ([est_package_id]) 
	on update no action on delete no action
go
alter table dbo.[est_packagecomponentmapping] add constraint [est_component_est_packagecomponentmapping_FK2] foreign key ([est_component_id])
	references dbo.[est_component] ([est_component_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[est_polybag] add constraint [est_polybaggroup_est_polybag_FK1] foreign key ([est_polybaggroup_id])
	references dbo.[est_polybaggroup] ([est_polybaggroup_id]) 
	on update no action on delete no action
go
alter table dbo.[est_polybag] add constraint [pst_postalscenario_est_polybag_FK2] foreign key ([pst_postalscenario_id])
	references dbo.[pst_postalscenario] ([pst_postalscenario_id]) 
	on update no action on delete no action
go
----------
alter table dbo.[pub_dayofweekrates] add constraint [pub_dayofweekratetypes_pub_dayofweekrates_FK1] foreign key ([pub_dayofweekratetypes_id])
	references dbo.[pub_dayofweekratetypes] ([pub_dayofweekratetypes_id]) 
	on update no action on delete no action
go
-----------------------------------------------------------------
PRINT '		Group #6'
go

alter table dbo.[est_packagepolybag_map] add constraint [est_package_est_packagepolybag_map_FK1] foreign key ([est_package_id])
	references dbo.[est_package] ([est_package_id]) 
	on update no action on delete no action
go
alter table dbo.[est_packagepolybag_map] add constraint [est_polybag_est_packagepolybag_map_FK2] foreign key ([est_polybag_id])
	references dbo.[est_polybag] ([est_polybag_id]) 
	on update no action on delete no action
go
-----------------------------------------------------------------

PRINT 'Unique Constraints'
GO

ALTER TABLE pub_pubrate_map
	ADD CONSTRAINT UQ_pub_pubrate_map UNIQUE (pub_id, publoc_id)
GO

ALTER TABLE pub_pubrate_map_activate
	ADD CONSTRAINT UQ_pub_pubrate_map_activate UNIQUE (pub_pubrate_map_id, effectivedate)
GO

ALTER TABLE pub_pubrate
	ADD CONSTRAINT UQ_pub_pubrate UNIQUE (pub_pubrate_map_id, effectivedate)
GO

ALTER TABLE pub_dayofweekratetypes
	ADD CONSTRAINT UQ_pub_dayofweekratetypes UNIQUE (pub_pubrate_id, ratetypedescription)
GO

ALTER TABLE pub_dayofweekrates
	ADD CONSTRAINT UQ_pubdayofweekrates UNIQUE (pub_dayofweekratetypes_id, insertdow)
GO

ALTER TABLE pub_pubquantity
	ADD CONSTRAINT UQ_pub_pubquantity UNIQUE (pub_pubrate_map_id, effectivedate)
GO

ALTER TABLE pub_dayofweekquantity
	ADD CONSTRAINT UQ_pub_dayofweekquantity UNIQUE (pub_pubquantity_id, pub_pubquantitytype_id, insertdow)
GO

ALTER TABLE pub_insertscenario
	ADD CONSTRAINT UQ_pub_insertscenario UNIQUE (description)
GO

ALTER TABLE vnd_vendor
	ADD CONSTRAINT UQ_vnd_vendor UNIQUE (description)
GO

ALTER TABLE vnd_printer
	ADD CONSTRAINT UQ_vnd_printer UNIQUE (vnd_vendor_id, effectivedate)
GO

ALTER TABLE vnd_paper
	ADD CONSTRAINT UQ_vnd_paper UNIQUE (vnd_vendor_id, effectivedate)
GO

ALTER TABLE vnd_mailhouserate
	ADD CONSTRAINT UQ_vnd_mailhouserate UNIQUE (vnd_vendor_id, effectivedate)
GO

ALTER TABLE vnd_mailtrackingrate
	ADD CONSTRAINT UQ_vnd_mailtrackingrate UNIQUE (vnd_vendor_id, effectivedate)
GO

ALTER TABLE vnd_maillistresourcerate
	ADD CONSTRAINT UQ_vnd_maillistresourcerate UNIQUE (vnd_vendor_id, effectivedate)
GO

ALTER TABLE prt_printerrate
	ADD CONSTRAINT UQ_prt_printerrate UNIQUE (vnd_printer_id, prt_printerratetype_id, description)
GO

ALTER TABLE ppr_paper_map
	ADD CONSTRAINT UQ_ppr_paper_map UNIQUE (vnd_paper_id, ppr_papergrade_id, ppr_paperweight_id, description)
GO

ALTER TABLE pst_postalscenario
	ADD CONSTRAINT UQ_pst_postalscenario UNIQUE (description, effectivedate)
GO

ALTER TABLE pst_postalcategory
	ADD CONSTRAINT UQ_pst_postalcategory UNIQUE (description)
GO

ALTER TABLE pst_postalweights
	ADD CONSTRAINT UQ_pst_postalweights UNIQUE (vnd_vendor_id, effectivedate)
GO

USE [master]
