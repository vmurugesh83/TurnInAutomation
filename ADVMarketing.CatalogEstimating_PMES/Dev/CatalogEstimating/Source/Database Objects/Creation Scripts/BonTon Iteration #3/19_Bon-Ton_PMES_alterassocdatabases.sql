ALTER TABLE dbo.assoc_databases
	ADD databasename varchar(50) NULL
GO

update dbo.assoc_databases
	set databasename = 	substring(connectionstring,
		charindex(';', connectionstring) + 17,
		charindex(';', connectionstring, charindex(';', connectionstring) + 1) - (charindex(';', connectionstring) + 17))
GO

ALTER TABLE dbo.assoc_databases
	ALTER COLUMN databasename varchar(50) NOT NULL
GO
