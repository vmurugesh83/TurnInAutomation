IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'BonTon_PMES_LIVE')
BEGIN
	PRINT 'Dropping DATABASE [BonTon_PMES_LIVE]'
	DROP DATABASE [BonTon_PMES_LIVE]

	-- Tell user if drop failed.
	IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'BonTon_PMES_LIVE')
		PRINT 'Drop of DATABASE [BonTon_PMES_LIVE] FAILED.'
END
GO

PRINT 'Creating DATABASE [BonTon_PMES_LIVE]'
GO

CREATE DATABASE [BonTon_PMES_LIVE]  
	ON (
		NAME = N'BonTon_PMES_LIVE_Data', 
		FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\BonTon_PMES_LIVE_Data.MDF' , 
		SIZE = 1, 
		FILEGROWTH = 10%) 
	LOG ON (
		NAME = N'BonTon_PMES_LIVE_Log', 
		FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\BonTon_PMES_LIVE_Log.LDF' , 
		SIZE = 1, 
		FILEGROWTH = 10%)
 COLLATE SQL_Latin1_General_CP1_CI_AS
GO

-- Tell user if create failed.
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'BonTon_PMES_LIVE')
	PRINT 'Create of DATABASE [BonTon_PMES_LIVE] FAILED.'
GO

IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'BonTon_PMES_SEASONAL')
BEGIN
	PRINT 'Dropping DATABASE [BonTon_PMES_SEASONAL]'
	DROP DATABASE [BonTon_PMES_SEASONAL]

	-- Tell user if drop failed.
	IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'BonTon_PMES_SEASONAL')
		PRINT 'Drop of DATABASE [BonTon_PMES_SEASONAL] FAILED.'
END
GO

PRINT 'Creating DATABASE [BonTon_PMES_SEASONAL]'
GO

CREATE DATABASE [BonTon_PMES_SEASONAL]  
	ON (
		NAME = N'BonTon_PMES_SEASONAL_Data', 
		FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\BonTon_PMES_SEASONAL_Data.MDF' , 
		SIZE = 1, 
		FILEGROWTH = 10%) 
	LOG ON (
		NAME = N'BonTon_PMES_SEASONAL_Log', 
		FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\BonTon_PMES_SEASONAL_Log.LDF' , 
		SIZE = 1, 
		FILEGROWTH = 10%)
 COLLATE SQL_Latin1_General_CP1_CI_AS
GO

-- Tell user if create failed.
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'BonTon_PMES_SEASONAL')
	PRINT 'Create of DATABASE [BonTon_PMES_SEASONAL] FAILED.'
GO

IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'BonTon_PMES_TEST1')
BEGIN
	PRINT 'Dropping DATABASE [BonTon_PMES_TEST1]'
	DROP DATABASE [BonTon_PMES_TEST1]

	-- Tell user if drop failed.
	IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'BonTon_PMES_TEST1')
		PRINT 'Drop of DATABASE [BonTon_PMES_TEST1] FAILED.'
END
GO

PRINT 'Creating DATABASE [BonTon_PMES_TEST1]'
GO

CREATE DATABASE [BonTon_PMES_TEST1]  
	ON (
		NAME = N'BonTon_PMES_TEST1_Data', 
		FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\BonTon_PMES_TEST1_Data.MDF' , 
		SIZE = 1, 
		FILEGROWTH = 10%) 
	LOG ON (
		NAME = N'BonTon_PMES_TEST1_Log', 
		FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\BonTon_PMES_TEST1_Log.LDF' , 
		SIZE = 1, 
		FILEGROWTH = 10%)
 COLLATE SQL_Latin1_General_CP1_CI_AS
GO

-- Tell user if create failed.
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'BonTon_PMES_TEST1')
	PRINT 'Create of DATABASE [BonTon_PMES_TEST1] FAILED.'
GO

IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'BonTon_PMES_TEST2')
BEGIN
	PRINT 'Dropping DATABASE [BonTon_PMES_TEST2]'
	DROP DATABASE [BonTon_PMES_TEST2]

	-- Tell user if drop failed.
	IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'BonTon_PMES_TEST2')
		PRINT 'Drop of DATABASE [BonTon_PMES_TEST2] FAILED.'
END
GO

PRINT 'Creating DATABASE [BonTon_PMES_TEST2]'
GO

CREATE DATABASE [BonTon_PMES_TEST2]  
	ON (
		NAME = N'BonTon_PMES_TEST2_Data', 
		FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\BonTon_PMES_TEST2_Data.MDF' , 
		SIZE = 1, 
		FILEGROWTH = 10%) 
	LOG ON (
		NAME = N'BonTon_PMES_TEST2_Log', 
		FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\BonTon_PMES_TEST2_Log.LDF' , 
		SIZE = 1, 
		FILEGROWTH = 10%)
 COLLATE SQL_Latin1_General_CP1_CI_AS
GO

-- Tell user if create failed.
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'BonTon_PMES_TEST2')
	PRINT 'Create of DATABASE [BonTon_PMES_TEST2] FAILED.'
GO

IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'BonTon_PMES_TEST3')
BEGIN
	PRINT 'Dropping DATABASE [BonTon_PMES_TEST3]'
	DROP DATABASE [BonTon_PMES_TEST3]

	-- Tell user if drop failed.
	IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'BonTon_PMES_TEST3')
		PRINT 'Drop of DATABASE [BonTon_PMES_TEST3] FAILED.'
END
GO

PRINT 'Creating DATABASE [BonTon_PMES_TEST3]'
GO

CREATE DATABASE [BonTon_PMES_TEST3]  
	ON (
		NAME = N'BonTon_PMES_TEST3_Data', 
		FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\BonTon_PMES_TEST3_Data.MDF' , 
		SIZE = 1, 
		FILEGROWTH = 10%) 
	LOG ON (
		NAME = N'BonTon_PMES_TEST3_Log', 
		FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\BonTon_PMES_TEST3_Log.LDF' , 
		SIZE = 1, 
		FILEGROWTH = 10%)
 COLLATE SQL_Latin1_General_CP1_CI_AS
GO

-- Tell user if create failed.
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'BonTon_PMES_TEST3')
	PRINT 'Create of DATABASE [BonTon_PMES_TEST3] FAILED.'
GO

IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'BonTon_PMES_TEST4')
BEGIN
	PRINT 'Dropping DATABASE [BonTon_PMES_TEST4]'
	DROP DATABASE [BonTon_PMES_TEST4]

	-- Tell user if drop failed.
	IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'BonTon_PMES_TEST4')
		PRINT 'Drop of DATABASE [BonTon_PMES_TEST2] FAILED.'
END
GO

PRINT 'Creating DATABASE [BonTon_PMES_TEST4]'
GO

CREATE DATABASE [BonTon_PMES_TEST4]  
	ON (
		NAME = N'BonTon_PMES_TEST4_Data', 
		FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\BonTon_PMES_TEST4_Data.MDF' , 
		SIZE = 1, 
		FILEGROWTH = 10%) 
	LOG ON (
		NAME = N'BonTon_PMES_TEST4_Log', 
		FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\BonTon_PMES_TEST4_Log.LDF' , 
		SIZE = 1, 
		FILEGROWTH = 10%)
 COLLATE SQL_Latin1_General_CP1_CI_AS
GO

-- Tell user if create failed.
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'BonTon_PMES_TEST4')
	PRINT 'Create of DATABASE [BonTon_PMES_TEST4] FAILED.'
GO


