-- USE [BonTon_PMES_LIVE]
-- USE [BonTon_PMES_SEASONAL]
-- USE [BonTon_PMES_TEST1]
-- USE [BonTon_PMES_TEST2]
-- USE [BonTon_PMES_TEST3]
-- USE [BonTon_PMES_TEST4]

CREATE TABLE dbo.[assoc_databasetype] (
	[databasetype_id] INTEGER IDENTITY (1, 1) not null,
	[description] VARCHAR(35) not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[assoc_databasetype]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[assoc_database_sync] (
	[destination_db_name] VARCHAR(35) not null,
	[source_db_name] VARCHAR(35) not null)
GO
GRANT  SELECT ,  INSERT  ON [dbo].[assoc_database_sync]  TO [PMES_SuperAdmin], [PMES_RateAdmin]
GO

CREATE TABLE [dbo].[adpub_cost] (
	[adnumber] [int] NOT NULL ,
	[pub_id] [char] (3) NOT NULL ,
	[publoc_id] [int] NOT NULL ,
	[rundate] [datetime] NOT NULL ,
	[issuedate] [datetime] NOT NULL ,
	[inserttime] [bit] NOT NULL ,
	[piececost] [money] NOT NULL ,
	[quantity] [int] NOT NULL ,
	[costwoinsert] [money] NOT NULL ,
	[insertcost] [money] NOT NULL ,
	[totalcost] [money] NOT NULL ,
	[createdby] [varchar] (50) NOT NULL ,
	[createddate] [datetime] NOT NULL 
) ON [PRIMARY]
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[adpub_cost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE [dbo].[directmail_cost] (
	[adnumber] [int] NOT NULL ,
	[description] [varchar] (35) NOT NULL ,
	[piececost] [money] NOT NULL ,
	[quantity] [int] NOT NULL ,
	[costwodistribution] [money] NOT NULL ,
	[distributioncost] [money] NOT NULL ,
	[totalcost] [money] NOT NULL ,
	[createdby] [varchar] (50) NOT NULL ,
	[createddate] [datetime] NOT NULL 
) ON [PRIMARY]
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[directmail_cost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE [dbo].[vendor_cost] (
	[vnd_vendor_id] [bigint] NULL ,
	[pub_id] [char] (3) NULL ,
	[costcode] [int] NOT NULL ,
	[adnumber] [int] NOT NULL ,
	[vendordescription] [varchar] (50) NOT NULL ,
	[rundate] [datetime] NOT NULL ,
	[addescription] [varchar] (50) NOT NULL ,
	[pages] [int] NOT NULL ,
	[mediaquantity] [int] NOT NULL ,
	[pubquantity] [int] NULL ,
	[costdescription] [varchar] (50) NOT NULL ,
	[grosscost] [money] NOT NULL ,
	[discount] [money] NULL ,
	[netcost] [money] NOT NULL ,
	[createdby] [varchar] (50) NOT NULL ,
	[createddate] [datetime] NOT NULL 
) ON [PRIMARY]
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vendor_cost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[est_componenttype] (
	[est_componenttype_id] INTEGER not null,
	[description] VARCHAR(35) not null,
	[comments] VARCHAR(255) null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[est_componenttype]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[est_costcodes] (
	[costcode] INTEGER not null,
	[description] VARCHAR(35) not null,
	[comments] VARCHAR(255) null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[est_costcodes]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[est_estimatemediatype] (
	[est_estimatemediatype_id] INTEGER not null,
	[description] VARCHAR(35) not null,
	[comments] VARCHAR(255) null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[est_estimatemediatype]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[est_season] (
	[est_season_id] INTEGER not null,
	[description] VARCHAR(35) not null,
	[comments] VARCHAR(255) null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[est_season]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[est_status] (
	[est_status_id] INTEGER not null,
	[description] VARCHAR(35) not null,
	[comments] VARCHAR(255) null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[est_status]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[ppr_papergrade] (
	[ppr_papergrade_id] INTEGER IDENTITY (1, 1) not null,
	[grade] VARCHAR(50) not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[ppr_papergrade]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[ppr_paperweight] (
	[ppr_paperweight_id] INTEGER IDENTITY (1, 1) not null,
	[weight] INTEGER not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[ppr_paperweight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pst_postalcategory] (
	[pst_postalcategory_id] INTEGER IDENTITY (1, 1) not null,
	[description] VARCHAR(35) not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pst_postalcategory]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pst_postalclass] (
	[pst_postalclass_id] INTEGER not null,
	[description] VARCHAR(35) not null,
	[comments] VARCHAR(255) null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pst_postalclass]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pst_postalmailertype] (
	[pst_postalmailertype_id] INTEGER not null,
	[description] VARCHAR(35) not null,
	[comments] VARCHAR(255) null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pst_postalmailertype]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[prt_printerratetype] (
	[prt_printerratetype_id] INTEGER not null,
	[description] VARCHAR(35) not null,
	[gridtitle] VARCHAR(50) not null,
	[displayorder] INTEGER not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[prt_printerratetype]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pub_insertscenario] (
	[pub_insertscenario_id] BIGINT IDENTITY (1, 1) not null,
	[description] VARCHAR(35) not null,
	[comments] VARCHAR(255) null,
	[active] BIT not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pub_insertscenario]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pub_pubgroup] (
	[pub_pubgroup_id] BIGINT IDENTITY (1, 1) not null,
	[description] VARCHAR(35) not null,
	[comments] VARCHAR(255) null,
	[active] BIT not null,
	[effectivedate] DATETIME not null,
	[sortorder] INTEGER not null,
	[customgroupforpackage] BIT not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pub_pubgroup]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pub_pubquantitytype] (
	[pub_pubquantitytype_id] INTEGER not null,
	[description] VARCHAR(20) not null,
	[special] BIT not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pub_pubquantitytype]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pub_ratetype] (
	[pub_ratetype_id] INTEGER not null,
	[description] VARCHAR(35) not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pub_ratetype]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE [dbo].[rpt_reporttype] (
	[rpt_reporttype_id] INTEGER not null,
	[description] VARCHAR(100) not null,
	[display] BIT not null,
	[displayorder] INTEGER not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[rpt_reporttype]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[vnd_vendor] (
	[vnd_vendor_id] BIGINT IDENTITY (1, 1) not null,
	[vendorcode] CHAR(10) not null,
	[description] VARCHAR(35) not null,
	[active] BIT not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vnd_vendor]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[vnd_vendortype] (
	[vnd_vendortype_id] INTEGER not null,
	[description] VARCHAR(35) not null,
	[comments] VARCHAR(255) null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vnd_vendortype]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

-----------------------------------------------------------------

CREATE TABLE dbo.[assoc_databases] (
	[database_id] INTEGER IDENTITY (1, 1) not null,
	[description] VARCHAR(35) not null,
	[display] BIT not null,
	[displayorder] INTEGER not null,
	[databasetype_id] INTEGER not null,
	[connectionstring] VARCHAR(255) not null,
	[databasename] VARCHAR(50) not null,
	[lastsyncratesdate] DATETIME null,
	[lastsyncestimatesdate] DATETIME null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[assoc_databases]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[est_estimate] (
	[est_estimate_id] BIGINT IDENTITY (1, 1) not null,
	[description] VARCHAR(35) not null,
	[comments] VARCHAR(255) null,
	[est_season_id] INTEGER not null,
	[fiscalyear] INTEGER not null,
	[rundate] DATETIME not null,
	[est_status_id] INTEGER not null,
	[fiscalmonth] INTEGER not null,
	[parent_id] BIGINT null,
	[uploaddate] DATETIME null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[est_estimate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pst_postalscenario] (
	[pst_postalscenario_id] BIGINT IDENTITY (1, 1) not null,
	[description] VARCHAR(35) not null,
	[comments] VARCHAR(255) null,
	[effectivedate] DATETIME not null,
	[pst_postalmailertype_id] INTEGER not null,
	[pst_postalclass_id] INTEGER not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pst_postalscenario]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pst_postalweights] (
	[pst_postalweights_id] INTEGER IDENTITY (1, 1) not null,
	[firstoverweightlimit] DECIMAL(12,4) not null,
	[standardoverweightlimit] DECIMAL(12,4) not null,
	[effectivedate] DATETIME not null,
	[vnd_vendor_id] BIGINT not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pst_postalweights]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pub_groupinsertscenario_map] (
	[pubgroupdescription] VARCHAR(50) not null,
	[pub_insertscenario_id] BIGINT not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pub_groupinsertscenario_map]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pub_pubrate_map] (
	[pub_pubrate_map_id] BIGINT IDENTITY (1, 1) not null,
	[pub_id] CHAR(3) not null,
	[publoc_id] INTEGER not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pub_pubrate_map]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE [dbo].[rpt_report] (
	[rpt_report_id] BIGINT IDENTITY (1, 1) not null,
	[rpt_reporttype_id] INTEGER not null,
	[report] IMAGE not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[rpt_report]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[vnd_vendorvendortype_map] (
	[vnd_vendor_id] BIGINT not null,
	[vnd_vendortype_id] INTEGER not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vnd_vendorvendortype_map]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[vnd_mailhouserate] (
	[vnd_mailhouserate_id] BIGINT IDENTITY (1, 1) not null,
	[vnd_vendor_id] BIGINT not null,
	[timevalueslips] MONEY not null,
	[inkjetrate] MONEY not null,
	[inkjetmakeready] MONEY not null,
	[adminfee] MONEY not null,
	[postaldropcwt] MONEY not null,
	[gluetackdefault] BIT not null,
	[gluetackrate] MONEY not null,
	[tabbingdefault] BIT not null,
	[tabbingrate] MONEY not null,
	[letterinsertiondefault] BIT not null,
	[letterinsertionrate] MONEY not null,
	[effectivedate] DATETIME not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vnd_mailhouserate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[vnd_maillistresourcerate] (
	[vnd_maillistresourcerate_id] BIGINT IDENTITY (1, 1) not null,
	[vnd_vendor_id] BIGINT not null,
	[internallistrate] MONEY not null,
	[effectivedate] DATETIME not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vnd_maillistresourcerate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[vnd_mailtrackingrate] (
	[vnd_mailtrackingrate_id] BIGINT IDENTITY (1, 1) not null,
	[vnd_vendor_id] BIGINT not null,
	[mailtracking] MONEY not null,
	[effectivedate] DATETIME not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vnd_mailtrackingrate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[vnd_paper] (
	[vnd_paper_id] BIGINT IDENTITY (1, 1) not null,
	[effectivedate] DATETIME not null,
	[vnd_vendor_id] BIGINT not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vnd_paper]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[vnd_printer] (
	[vnd_printer_id] BIGINT IDENTITY (1, 1) not null,
	[vnd_vendor_id] BIGINT not null,
	[paperhandling] MONEY not null,
	[polybagbagweight] DECIMAL(10,4) not null,
	[cornerguarddefault] BIT not null,
	[cornerguard] MONEY not null,
	[skiddefault] BIT not null,
	[skid] MONEY not null,
	[polybagmessagedefault] BIT not null,
	[polybagmessage] MONEY not null,
	[polybagmessagemakereadydefault] BIT not null,
	[polybagmessagemakeready] MONEY not null,
	[effectivedate] DATETIME not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vnd_printer]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

-----------------------------------------------------------------

CREATE TABLE dbo.[est_assemdistriboptions] (
-- 	[est_assemdistriboptions_id] BIGINT IDENTITY (1, 1) not null,
	[est_estimate_id] BIGINT not null,
	[insertdow] INTEGER not null,
	[insertfreightvendor_id] BIGINT not null,
	[insertfreightcwt] MONEY not null,
	[insertfuelsurcharge] DECIMAL(10,4) not null,
	[cornerguards] BIT not null,
	[skids] BIT not null,
	[inserttime] BIT not null,
	[pst_postalscenario_id] BIGINT not null,
	[mailfuelsurcharge] DECIMAL(10,4) not null,
	[mailhouse_id] BIGINT not null,
	[mailhouseotherhandling] MONEY not null,
	[usemailtracking] BIT not null,
	[mailtracking_id] BIGINT null,
	[maillistresource_id] BIGINT null,
	[useexternalmaillist] BIT not null,
	[externalmailqty] INTEGER not null,
	[externalmailcpm] MONEY not null,
	[nbrofcartons] INTEGER not null,
	[usegluetack] BIT not null,
	[usetabbing] BIT not null,
	[useletterinsertion] BIT not null,
	[firstclass] BIT not null,
	[otherfreight] MONEY not null,
	[postaldropflat] MONEY null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[est_assemdistriboptions]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[est_package] (
	[est_package_id] BIGINT IDENTITY (1, 1) not null,
	[est_estimate_id] BIGINT not null,
	[description] VARCHAR(35) not null,
	[comments] VARCHAR(255) null,
	[soloquantity] INTEGER not null,
	[otherquantity] INTEGER not null,
	[pub_pubquantitytype_id] INTEGER NULL,
	[pub_pubgroup_id] BIGINT NULL,
	[pub_insertscenario_id] BIGINT null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[est_package]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[est_pubissuedates] (
	[est_estimate_id] BIGINT not null,
	[pub_pubrate_map_id] BIGINT not null,
	[override] BIT not null,
	[issuedow] INTEGER not null,
	[issuedate] DATETIME not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[est_pubissuedates]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[est_samples] (
--	[est_sample_id] BIGINT IDENTITY (1, 1) not null,
	[est_estimate_id] BIGINT not null,
	[quantity] INTEGER not null,
	[freightcwt] MONEY not null,
	[freightflat] MONEY not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[est_samples]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[ppr_paper_map] (
	[ppr_paper_map_id] BIGINT IDENTITY (1, 1) not null,
	[description] VARCHAR(35) not null,
	[cwt] MONEY not null,
	[default] BIT not null,
	[ppr_papergrade_id] INTEGER not null,
	[ppr_paperweight_id] INTEGER not null,
	[vnd_paper_id] BIGINT not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[ppr_paper_map]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[prt_printerrate] (
	[prt_printerrate_id] BIGINT IDENTITY (1, 1) not null,
	[vnd_printer_id] BIGINT not null,
	[prt_printerratetype_id] INTEGER not null,
	[rate] MONEY not null,
	[description] VARCHAR(35) not null,
	[default] BIT not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[prt_printerrate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pst_postalcategoryrate_map] (
	[pst_postalcategoryrate_map_id] BIGINT IDENTITY (1, 1) not null,
	[pst_postalmailertype_id] INTEGER not null,
	[pst_postalclass_id] INTEGER not null,
	[pst_postalcategory_id] INTEGER not null,
	[active] BIT not null,
	[underweightpiecerate] MONEY not null,
	[overweightpoundrate] MONEY not null,
	[overweightpiecerate] MONEY not null,
	[pst_postalweights_id] INTEGER not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pst_postalcategoryrate_map]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pub_pubpubgroup_map] (
	[pub_pubrate_map_id] BIGINT not null,
	[pub_pubgroup_id] BIGINT not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pub_pubpubgroup_map]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pub_pubquantity] (
	[pub_pubquantity_id] BIGINT IDENTITY (1, 1) not null,
	[effectivedate] DATETIME not null,
	[pub_pubrate_map_id] BIGINT null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pub_pubquantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pub_pubrate] (
	[pub_pubrate_id] BIGINT IDENTITY (1, 1) not null,
	[pub_ratetype_id] INTEGER not null,
	[chargeblowin] BIT not null,
	[blowinrate] INTEGER not null,
	[effectivedate] DATETIME not null,
	[quantitychargetype] INTEGER not null,
	[billedpct] DECIMAL(10,2) not null,
	[pub_pubrate_map_id] BIGINT null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pub_pubrate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pub_pubrate_map_activate] (
	[pub_pubrate_map_id] BIGINT not null,
	[effectivedate] DATETIME not null,
	[active] BIT not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pub_pubrate_map_activate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

-----------------------------------------------------------------

CREATE TABLE dbo.[est_component] (
	[est_component_id] BIGINT IDENTITY (1, 1) not null,
	[est_estimate_id] BIGINT not null,
	[description] VARCHAR(35) not null,
	[comments] VARCHAR(255) null,
	[financialchangecomment] VARCHAR(255) null,
	[adnumber] INTEGER null,
	[est_estimatemediatype_id] INTEGER not null,
	[est_componenttype_id] INTEGER not null,
	[mediaqtywoinsert] INTEGER null,
	[spoilagepct] DECIMAL(10,4) null,
	[pagecount] INTEGER not null,
	[width] DECIMAL(10,4) not null,
	[height] DECIMAL(10,4) not null,
	[otherproduction] MONEY null,
	[vendorsupplied] BIT not null,
	[vendorsupplied_id] BIGINT null,
	[vendorcpm] MONEY null,
	[creativevendor_id] BIGINT null,
	[creativecpp] MONEY null,
	[separator_id] BIGINT null,
	[separatorcpp] MONEY null,
	[printer_id] BIGINT null,
	[calculateprintcost] BIT not null,
	[printcost] MONEY null,
	[numberofplants] INTEGER null,
	[additionalplates] INTEGER null,
	[platecost_id] BIGINT null,
	[replacementplatecost] MONEY null,
	[runrate] MONEY null,
	[numberdigitalhandlenprepare] INTEGER null,
	[digitalhandlenprepare_id] BIGINT NULL,
	[stitchin_id] BIGINT null,
	[blowin_id] BIGINT null,
	[onsert_id] BIGINT null,
	[stitchermakeready_id] BIGINT null,
	[stitchermakereadyrate] money null,
	[pressmakeready_id] BIGINT null,
	[pressmakereadyrate] money null,
	[earlypayprintdiscount] DECIMAL(10,4) null,
	[printerapplytax] BIT not null,
	[printertaxablemediapct] DECIMAL(10,4) null,
	[printersalestaxpct] DECIMAL(10,4) null,
	[paper_id] BIGINT null,
	[paper_map_id] BIGINT null,
	[paperweight_id] INTEGER not null,
	[papergrade_id] INTEGER not null,
	[calculatepapercost] BIT not null,
	[papercost] MONEY null,
	[runpounds] DECIMAL(10,2) null,
	[makereadypounds] INTEGER null,
	[platechangepounds] DECIMAL(10,2) null,
	[pressstoppounds] INTEGER null,
	[numberofpressstops] INTEGER null,
	[earlypaypaperdiscount] DECIMAL(10,4) null,
	[paperapplytax] BIT not null,
	[papertaxablemediapct] DECIMAL(10,4) null,
	[papersalestaxpct] DECIMAL(10,4) null,
	[assemblyvendor_id] BIGINT null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[est_component]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[est_polybaggroup] (
	[est_polybaggroup_id] BIGINT IDENTITY (1, 1) not null,
	[description] VARCHAR(35) not null,
	[comments] VARCHAR(255) null,
	[vnd_printer_id] BIGINT not null,
	[prt_bagrate_id] BIGINT not null,
	[prt_bagmakereadyrate_id] BIGINT NOT NULL,
	[usemessage] BIT not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[est_polybaggroup]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pst_postalcategoryscenario_map] (
	[pst_postalscenario_id] BIGINT not null,
	[pst_postalcategoryrate_map_id] BIGINT not null,
	[percentage] DECIMAL(10,4) not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pst_postalcategoryscenario_map]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pub_dayofweekquantity] (
	[pub_dayofweekquantity_id] BIGINT IDENTITY (1, 1) not null,
	[pub_pubquantity_id] BIGINT not null,
	[pub_pubquantitytype_id] INTEGER not null,
	[insertdow] INTEGER null,
	[quantity] INTEGER not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pub_dayofweekquantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pub_dayofweekratetypes] (
	[pub_dayofweekratetypes_id] BIGINT IDENTITY (1, 1) not null,
	[ratetypedescription] DECIMAL(10,2) not null,
	[pub_pubrate_id] BIGINT not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pub_dayofweekratetypes]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pub_insertdiscounts] (
	[pub_insertdiscount_id] BIGINT IDENTITY (1, 1) not null,
	[pub_pubrate_id] BIGINT not null,
	[insert] INTEGER not null,
	[discount] DECIMAL(10,4) not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pub_insertdiscounts]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

-----------------------------------------------------------------

CREATE TABLE dbo.[est_estimatepolybaggroup_map] (
	[est_estimate_id] BIGINT not null,
	[est_polybaggroup_id] BIGINT not null,
	[estimateorder] INT NOT NULL,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[est_estimatepolybaggroup_map]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[est_packagecomponentmapping] (
	[est_package_id] BIGINT not null,
	[est_component_id] BIGINT not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[est_packagecomponentmapping]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[est_polybag] (
	[est_polybag_id] BIGINT IDENTITY (1, 1) not null,
	[est_polybaggroup_id] BIGINT not null,
	[pst_postalscenario_id] BIGINT not null,
	[quantity] INTEGER not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[est_polybag]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

CREATE TABLE dbo.[pub_dayofweekrates] (
	[pub_dayofweekrates_id] BIGINT IDENTITY (1, 1) not null,
	[pub_dayofweekratetypes_id] BIGINT not null,
	[rate] MONEY not null,
	[insertdow] INTEGER not null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[pub_dayofweekrates]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

-----------------------------------------------------------------

CREATE TABLE dbo.[est_packagepolybag_map] (
	[est_package_id] BIGINT not null,
	[est_polybag_id] BIGINT not null,
	[distributionpct] DECIMAL(10,4) null,
	[createdby] VARCHAR(50) not null,
	[createddate] DATETIME not null,
	[modifiedby] VARCHAR(50) null,
	[modifieddate] DATETIME null)
GO
GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[est_packagepolybag_map]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

PRINT 'Completed creating tables.'
GO

USE [master]

