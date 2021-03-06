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

alter table dbo.[adpub_cost] add constraint [adpub_cost_PK] primary key clustered ([adnumber], [pub_id], [publoc_id], [rundate]) 
go
alter table dbo.[directmail_cost] add constraint [directmail_cost_PK] primary key clustered ([adnumber]) 
go
