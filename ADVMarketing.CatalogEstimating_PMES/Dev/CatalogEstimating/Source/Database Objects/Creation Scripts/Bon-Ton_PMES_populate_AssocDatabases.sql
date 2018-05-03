DECLARE @DB_Server varchar(20)
SET @DB_Server = 'MILDEVSQL2000'
DECLARE @PMES_Env varchar(20)
SET @PMES_Env = '' 
DECLARE @PMES_Offset int
SET @PMES_Offset = 0


SET IDENTITY_INSERT [BonTon_PMES_LIVE].[dbo].[assoc_databasetype] ON
INSERT INTO [BonTon_PMES_LIVE].[dbo].[assoc_databasetype]
	([databasetype_id], [description], [createdby], [createddate])
	VALUES
	(1, 'Live Estimate Database', 'dbo', getdate())
INSERT INTO [BonTon_PMES_LIVE].[dbo].[assoc_databasetype]
	([databasetype_id], [description], [createdby], [createddate])
	VALUES
	(2, 'Seasonal Test Estimate Database', 'dbo', getdate())
INSERT INTO [BonTon_PMES_LIVE].[dbo].[assoc_databasetype]
	([databasetype_id], [description], [createdby], [createddate])
	VALUES
	(3, 'WhatIF Test Estimate Database', 'dbo', getdate())
INSERT INTO [BonTon_PMES_LIVE].[dbo].[assoc_databasetype]
	([databasetype_id], [description], [createdby], [createddate])
	VALUES
	(4, 'Admin Database', 'dbo', getdate())
SET IDENTITY_INSERT [BonTon_PMES_LIVE].[dbo].[assoc_databasetype] OFF


SET IDENTITY_INSERT [BonTon_PMES_LIVE].[dbo].[assoc_databases] ON
INSERT INTO [BonTon_PMES_LIVE].[dbo].[assoc_databases]
	([database_id], [description], [display], [displayorder], [databasetype_id], [connectionstring], [createdby], [createddate])
	VALUES
	(@PMES_Offset + 1, 'Live ' + @PMES_Env +' Database', 1, 1, 1, 'Data Source=' + @DB_Server + ';Initial Catalog=BonTon_PMES_LIVE;Integrated Security=True', 'dbo', getdate())
INSERT INTO [BonTon_PMES_LIVE].[dbo].[assoc_databases]
	([database_id], [description], [display], [displayorder], [databasetype_id], [connectionstring], [createdby], [createddate])
	VALUES
	(@PMES_Offset + 2, 'Seasonal ' + @PMES_Env +' Estimate Database', 1, 2, 2, 'Data Source=' + @DB_Server + ';Initial Catalog=BonTon_PMES_SEASONAL;Integrated Security=True', 'dbo', getdate())
INSERT INTO [BonTon_PMES_LIVE].[dbo].[assoc_databases]
	([database_id], [description], [display], [displayorder], [databasetype_id], [connectionstring], [createdby], [createddate])
	VALUES
	(@PMES_Offset + 3, '1st ' + @PMES_Env +' Sandbox Estimate Database', 1, 3, 3, 'Data Source=' + @DB_Server + ';Initial Catalog=BonTon_PMES_TEST1;Integrated Security=True', 'dbo', getdate())
INSERT INTO [BonTon_PMES_LIVE].[dbo].[assoc_databases]
	([database_id], [description], [display], [displayorder], [databasetype_id], [connectionstring], [createdby], [createddate])
	VALUES
	(@PMES_Offset + 4, '2nd ' + @PMES_Env +' Sandbox Estimate Database', 1, 4, 3, 'Data Source=' + @DB_Server + ';Initial Catalog=BonTon_PMES_TEST2;Integrated Security=True', 'dbo', getdate())
INSERT INTO [BonTon_PMES_LIVE].[dbo].[assoc_databases]
	([database_id], [description], [display], [displayorder], [databasetype_id], [connectionstring], [createdby], [createddate])
	VALUES
	(@PMES_Offset + 5, '3rd ' + @PMES_Env +' Sandbox Estimate Database', 1, 5, 3, 'Data Source=' + @DB_Server + ';Initial Catalog=BonTon_PMES_TEST3;Integrated Security=True', 'dbo', getdate())
INSERT INTO [BonTon_PMES_LIVE].[dbo].[assoc_databases]
	([database_id], [description], [display], [displayorder], [databasetype_id], [connectionstring], [createdby], [createddate])
	VALUES
	(@PMES_Offset + 6, '4th ' + @PMES_Env +' Sandbox Estimate Database', 1, 6, 3, 'Data Source=' + @DB_Server + ';Initial Catalog=BonTon_PMES_TEST4;Integrated Security=True', 'dbo', getdate())
SET IDENTITY_INSERT [BonTon_PMES_LIVE].[dbo].[assoc_databases] OFF
