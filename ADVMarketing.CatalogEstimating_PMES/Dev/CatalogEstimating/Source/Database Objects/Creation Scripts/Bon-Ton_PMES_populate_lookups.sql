-- USE [BonTon_PMES_LIVE]
-- USE [BonTon_PMES_SEASONAL]
-- USE [BonTon_PMES_TEST1]
-- USE [BonTon_PMES_TEST2]
-- USE [BonTon_PMES_TEST3]
-- USE [BonTon_PMES_TEST4]

/*EST_Season*/
insert into est_season(est_season_id, description, createdby, createddate)
values(1, 'Spring', 'admin', getdate())
go
insert into est_season(est_season_id, description, createdby, createddate)
values(2, 'Fall', 'admin', getdate())
go

/*EST_Status*/
insert into est_status(est_status_id, description, createdby, createddate)
values(1, 'Active', 'admin', getdate())
go
insert into est_status(est_status_id, description, createdby, createddate)
values(2, 'Uploaded', 'admin', getdate())
go
insert into est_status(est_status_id, description, createdby, createddate)
values(3, 'Killed', 'admin', getdate())
go

/*EST_EstimateMediaType*/
insert into est_estimatemediatype(est_estimatemediatype_id, description, comments, createdby, createddate)
values(1, 'CAT', 'Catalog', 'admin', getdate())
go
insert into est_estimatemediatype(est_estimatemediatype_id, description, comments, createdby, createddate)
values(2, 'BST', 'Broadsheet', 'admin', getdate())
go
insert into est_estimatemediatype(est_estimatemediatype_id, description, comments, createdby, createddate)
values(3, 'TAB', 'Tab', 'admin', getdate())
go
insert into est_estimatemediatype(est_estimatemediatype_id, description, comments, createdby, createddate)
values(4, 'MJR COL', 'Major Collateral', 'admin', getdate())
go
insert into est_estimatemediatype(est_estimatemediatype_id, description, comments, createdby, createddate)
values(5, 'min col', 'Minor Collateral', 'admin', getdate())
go

/*EST_ComponentType*/
insert into est_componenttype(est_componenttype_id, description, comments, createdby, createddate)
values(1, 'HOST', 'Host', 'admin', getdate())
go
insert into est_componenttype(est_componenttype_id, description, comments, createdby, createddate)
values(2, 'ONS', 'Onsert', 'admin', getdate())
go
insert into est_componenttype(est_componenttype_id, description, comments, createdby, createddate)
values(3, 'SI', 'Stitch-In', 'admin', getdate())
go
insert into est_componenttype(est_componenttype_id, description, comments, createdby, createddate)
values(4, 'BI', 'Blow-In', 'admin', getdate())
go
insert into est_componenttype(est_componenttype_id, description, comments, createdby, createddate)
values(5, '+CVR', 'Plus Cover', 'admin', getdate())
go
insert into est_componenttype(est_componenttype_id, description, comments, createdby, createddate)
values(6, 'WRAP', 'Wrap', 'admin', getdate())
go
insert into est_componenttype(est_componenttype_id, description, comments, createdby, createddate)
values(7, 'OTHER', 'Other', 'admin', getdate())
go

/*EST_CostCodes*/
-- set identity_insert est_costcodes on
insert into est_costcodes(costcode, description, createdby, createddate)
values(530, 'Creative/Photography', 'admin', getdate())
-- set identity_insert est_costcodes off
go
-- set identity_insert est_costcodes on
insert into est_costcodes(costcode, description, createdby, createddate)
values(595, 'Color Separations', 'admin', getdate())
-- set identity_insert est_costcodes off
go
-- set identity_insert est_costcodes on
insert into est_costcodes(costcode, description, createdby, createddate)
values(610, 'Print', 'admin', getdate())
-- set identity_insert est_costcodes off
go
-- set identity_insert est_costcodes on
insert into est_costcodes(costcode, description, createdby, createddate)
values(605, 'Paper', 'admin', getdate())
-- set identity_insert est_costcodes off
go
-- set identity_insert est_costcodes on
insert into est_costcodes(costcode, description, createdby, createddate)
values(606, 'Paper Handling', 'admin', getdate())
-- set identity_insert est_costcodes off
go
-- set identity_insert est_costcodes on
insert into est_costcodes(costcode, description, createdby, createddate)
values(615, 'Sales Tax', 'admin', getdate())
-- set identity_insert est_costcodes off
go
-- set identity_insert est_costcodes on
insert into est_costcodes(costcode, description, createdby, createddate)
values(760, 'Mail List', 'admin', getdate())
-- set identity_insert est_costcodes off
go
-- set identity_insert est_costcodes on
insert into est_costcodes(costcode, description, createdby, createddate)
values(880, 'Specialty Other', 'admin', getdate())
-- set identity_insert est_costcodes off
go
-- set identity_insert est_costcodes on
insert into est_costcodes(costcode, description, createdby, createddate)
values(870, 'Vendor Production', 'admin', getdate())
-- set identity_insert est_costcodes off
go
-- set identity_insert est_costcodes on
insert into est_costcodes(costcode, description, createdby, createddate)
values(730, 'Polybag', 'admin', getdate())
-- set identity_insert est_costcodes off
go
-- set identity_insert est_costcodes on
insert into est_costcodes(costcode, description, createdby, createddate)
values(720, 'Stitch-ins/Blow-ins', 'admin', getdate())
-- set identity_insert est_costcodes off
go
-- set identity_insert est_costcodes on
insert into est_costcodes(costcode, description, createdby, createddate)
values(745, 'Ink Jet', 'admin', getdate())
-- set identity_insert est_costcodes off
go
-- set identity_insert est_costcodes on
insert into est_costcodes(costcode, description, createdby, createddate)
values(740, 'Handling', 'admin', getdate())
-- set identity_insert est_costcodes off
go

/*VND_VendorType*/
insert into vnd_vendortype(vnd_vendortype_id, description, createdby, createddate)
values(1, 'Printer', 'admin', getdate())
go
insert into vnd_vendortype(vnd_vendortype_id, description, createdby, createddate)
values(2, 'Paper', 'admin', getdate())
go
insert into vnd_vendortype(vnd_vendortype_id, description, createdby, createddate)
values(3, 'Creative', 'admin', getdate())
go
insert into vnd_vendortype(vnd_vendortype_id, description, createdby, createddate)
values(4, 'Separator', 'admin', getdate())
go
insert into vnd_vendortype(vnd_vendortype_id, description, createdby, createddate)
values(5, 'Mail House', 'admin', getdate())
go
insert into vnd_vendortype(vnd_vendortype_id, description, createdby, createddate)
values(6, 'Mail List Resource', 'admin', getdate())
go
insert into vnd_vendortype(vnd_vendortype_id, description, createdby, createddate)
values(7, 'Mail Tracker', 'admin', getdate())
go
insert into vnd_vendortype(vnd_vendortype_id, description, createdby, createddate)
values(8, 'Postal', 'admin', getdate())
go
insert into vnd_vendortype(vnd_vendortype_id, description, createdby, createddate)
values(9, 'Vendor Supplied', 'admin', getdate())
go
insert into vnd_vendortype(vnd_vendortype_id, description, createdby, createddate)
values(10, 'Insert Freight', 'admin', getdate())
go

/*PUB_RateType*/
insert into pub_ratetype(pub_ratetype_id, description, createdby, createddate)
values(1, 'Tab Page Count', 'admin', getdate())
go
insert into pub_ratetype(pub_ratetype_id, description, createdby, createddate)
values(2, 'Flat', 'admin', getdate())
go
insert into pub_ratetype(pub_ratetype_id, description, createdby, createddate)
values(3, 'CPM', 'admin', getdate())
go
insert into pub_ratetype(pub_ratetype_id, description, createdby, createddate)
values(4, 'Cost Per Weight', 'admin', getdate())
go
insert into pub_ratetype(pub_ratetype_id, description, createdby, createddate)
values(5, 'Cost Per Size', 'admin', getdate())
go

/*PUB_PubQuantityType*/
insert into pub_pubquantitytype(pub_pubquantitytype_id, description, special, createdby, createddate)
values(1, 'Minimum', 0, 'admin', getdate())
go
insert into pub_pubquantitytype(pub_pubquantitytype_id, description, special, createdby, createddate)
values(2, 'Contract Send', 0, 'admin', getdate())
go
insert into pub_pubquantitytype(pub_pubquantitytype_id, description, special, createdby, createddate)
values(3, 'Full Run', 0, 'admin', getdate())
go
insert into pub_pubquantitytype(pub_pubquantitytype_id, description, special, createdby, createddate)
values(4, 'Thanksgiving', 1, 'admin', getdate())
go
insert into pub_pubquantitytype(pub_pubquantitytype_id, description, special, createdby, createddate)
values(5, 'Christmas', 1, 'admin', getdate())
go
insert into pub_pubquantitytype(pub_pubquantitytype_id, description, special, createdby, createddate)
values(6, 'New Years', 1, 'admin', getdate())
go

-- /*RPT_ParameterType*/
-- insert into rpt_parametertype(rpt_parametertype_id, description, createdby, createddate)
-- values(1, 'Media Type', 'admin', getdate())
-- go
-- insert into rpt_parametertype(rpt_parametertype_id, description, createdby, createddate)
-- values(2, 'Component Type', 'admin', getdate())
-- go
-- insert into rpt_parametertype(rpt_parametertype_id, description, createdby, createddate)
-- values(3, 'Vendor', 'admin', getdate())
-- go
-- insert into rpt_parametertype(rpt_parametertype_id, description, createdby, createddate)
-- values(4, 'Cost Code', 'admin', getdate())
-- go

/*RPT_ReportType*/
insert into rpt_reporttype(rpt_reporttype_id, display, displayorder, description, createdby, createddate)
values(1, 1, 7, 'Ad Publication Costs', 'admin', getdate())
go
insert into rpt_reporttype(rpt_reporttype_id, display, displayorder, description, createdby, createddate)
values(2, 1, 8, 'Direct Mail Costs', 'admin', getdate())
go
insert into rpt_reporttype(rpt_reporttype_id, display, displayorder, description, createdby, createddate)
values(3, 1, 4, 'Postage Category Usage', 'admin', getdate())
go
insert into rpt_reporttype(rpt_reporttype_id, display, displayorder, description, createdby, createddate)
values(4, 1, 1, 'Estimate Report', 'admin', getdate())
go
insert into rpt_reporttype(rpt_reporttype_id, display, displayorder, description, createdby, createddate)
values(5, 1, 3, 'Estimate Summary Report', 'admin', getdate())
go
insert into rpt_reporttype(rpt_reporttype_id, display, displayorder, description, createdby, createddate)
values(6, 0, 9, 'Financial Change', 'admin', getdate())
go
insert into rpt_reporttype(rpt_reporttype_id, display, displayorder, description, createdby, createddate)
values(7, 1, 6, 'Vendor Commitment', 'admin', getdate())
go
insert into rpt_reporttype(rpt_reporttype_id, display, displayorder, description, createdby, createddate)
values(8, 1, 5, 'This Year vs Last Year', 'admin', getdate())
go
insert into rpt_reporttype(rpt_reporttype_id, display, displayorder, description, createdby, createddate)
values(9, 1, 2, 'Estimate Element Detail', 'admin', getdate())
go
insert into rpt_reporttype(rpt_reporttype_id, display, displayorder, description, createdby, createddate)
values(10, 0, 10, 'Estimate Pub Changes', 'admin', getdate())
go

/*PPR_PaperWeight*/
set identity_insert PPR_PaperWeight on
insert into ppr_paperweight(ppr_paperweight_id, weight, createdby, createddate)
values(1, 10, 'admin', getdate())
set identity_insert PPR_PaperWeight off
go
set identity_insert PPR_PaperWeight on
insert into ppr_paperweight(ppr_paperweight_id, weight, createdby, createddate)
values(2, 20, 'admin', getdate())
set identity_insert PPR_PaperWeight off
go
set identity_insert PPR_PaperWeight on
insert into ppr_paperweight(ppr_paperweight_id, weight, createdby, createddate)
values(3, 30, 'admin', getdate())
set identity_insert PPR_PaperWeight off
go
set identity_insert PPR_PaperWeight on
insert into ppr_paperweight(ppr_paperweight_id, weight, createdby, createddate)
values(4, 40, 'admin', getdate())
set identity_insert PPR_PaperWeight off
go
set identity_insert PPR_PaperWeight on
insert into ppr_paperweight(ppr_paperweight_id, weight, createdby, createddate)
values(5, 50, 'admin', getdate())
set identity_insert PPR_PaperWeight off
go

/*PRT_PrinterRateType*/
insert into prt_printerratetype(prt_printerratetype_id, description, gridtitle, displayorder, createdby, createddate)
values(1, 'Stitch-In', 'Cost per Thousand', 2, 'admin', getdate())
go
insert into prt_printerratetype(prt_printerratetype_id, description, gridtitle, displayorder, createdby, createddate)
values(2, 'Blow-In', 'Cost per Thousand', 1, 'admin', getdate())
go
insert into prt_printerratetype(prt_printerratetype_id, description, gridtitle, displayorder, createdby, createddate)
values(3, 'Carton', 'Cost per Carton', 6, 'admin', getdate())
go
insert into prt_printerratetype(prt_printerratetype_id, description, gridtitle, displayorder, createdby, createddate)
values(4, 'Stitcher Makeready', 'Per Plant Cost', 8, 'admin', getdate())
go
insert into prt_printerratetype(prt_printerratetype_id, description, gridtitle, displayorder, createdby, createddate)
values(5, 'Digital Handle & Prep', 'Cost', 7, 'admin', getdate())
go
insert into prt_printerratetype(prt_printerratetype_id, description, gridtitle, displayorder, createdby, createddate)
values(6, 'PB Makeready', 'Per Plant Cost', 5, 'admin', getdate())
go
insert into prt_printerratetype(prt_printerratetype_id, description, gridtitle, displayorder, createdby, createddate)
values(7, 'PB Bag', 'Cost per Thousand', 4, 'admin', getdate())
go
insert into prt_printerratetype(prt_printerratetype_id, description, gridtitle, displayorder, createdby, createddate)
values(8, 'Plates', 'Per Plate Costs', 3, 'admin', getdate())
go
insert into prt_printerratetype(prt_printerratetype_id, description, gridtitle, displayorder, createdby, createddate)
values(9, 'Onsert', 'Cost per Thousand', 9, 'admin', getdate())
go
insert into prt_printerratetype(prt_printerratetype_id, description, gridtitle, displayorder, createdby, createddate)
values(10, 'Press Makeready', 'Per Plant Cost', 10, 'admin', getdate())
go

/*PST_PostalClass*/
insert into pst_postalclass(pst_postalclass_id, description, createdby, createddate)
values(1, 'First', 'admin', getdate())
go
insert into pst_postalclass(pst_postalclass_id, description, createdby, createddate)
values(2, 'Standard', 'admin', getdate())
go

/*PST_PostalMailerType*/
insert into pst_postalmailertype(pst_postalmailertype_id, description, createdby, createddate)
values(1, 'Flat', 'admin', getdate())
go
insert into pst_postalmailertype(pst_postalmailertype_id, description, createdby, createddate)
values(2, 'Letter', 'admin', getdate())
go
insert into pst_postalmailertype(pst_postalmailertype_id, description, createdby, createddate)
values(3, 'Card', 'admin', getdate())
go

/*PST_PostalCategory*/
set identity_insert pst_postalcategory on
insert into pst_postalcategory(pst_postalcategory_id, description, createdby, createddate)
values(1, 'DDU Presort Basic', 'admin', getdate())
set identity_insert pst_postalcategory off
go
set identity_insert pst_postalcategory on
insert into pst_postalcategory(pst_postalcategory_id, description, createdby, createddate)
values(2, 'None Saturated', 'admin', getdate())
set identity_insert pst_postalcategory off
go
set identity_insert pst_postalcategory on
insert into pst_postalcategory(pst_postalcategory_id, description, createdby, createddate)
values(3, 'None Presort Basic', 'admin', getdate())
set identity_insert pst_postalcategory off
go
set identity_insert pst_postalcategory on
insert into pst_postalcategory(pst_postalcategory_id, description, createdby, createddate)
values(4, 'Saturated', 'admin', getdate())
set identity_insert pst_postalcategory off
go
set identity_insert pst_postalcategory on
insert into pst_postalcategory(pst_postalcategory_id, description, createdby, createddate)
values(5, 'DDU Saturated', 'admin', getdate())
set identity_insert pst_postalcategory off
go

