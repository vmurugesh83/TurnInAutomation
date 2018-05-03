DROP TABLE dbo.[est_pubinsertdates]
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
ALTER TABLE dbo.[est_pubissuedates] ADD CONSTRAINT [est_pubissuedates_PK] PRIMARY KEY ([est_estimate_id], [pub_pubrate_map_id])
GO
ALTER TABLE dbo.[est_pubissuedates] ADD CONSTRAINT [est_estimate_est_pubissuedates_FK1] FOREIGN KEY ([est_estimate_id])
	REFERENCES dbo.[est_estimate] ([est_estimate_id]) 
	ON UPDATE NO ACTION ON DELETE NO ACTION
GO
ALTER TABLE dbo.[est_pubissuedates] ADD CONSTRAINT [pub_pubrate_map_est_pubissuedates_FK2] FOREIGN KEY ([pub_pubrate_map_id])
	REFERENCES dbo.[pub_pubrate_map] ([pub_pubrate_map_id]) 
	ON UPDATE NO ACTION ON DELETE NO ACTION
GO

------------------------------------------------------------------------

ALTER TABLE dbo.est_assemdistriboptions
	ADD postaldropflat money null
GO

------------------------------------------------------------------------

DROP TABLE dbo.est_estimateinsertscenario_map
GO

ALTER TABLE dbo.[est_package]
	ADD [pub_insertscenario_id] BIGINT null
GO

ALTER TABLE dbo.[est_package] ADD CONSTRAINT [pub_insertscenario_est_package_FK1] FOREIGN KEY ([pub_insertscenario_id])
	REFERENCES dbo.[pub_insertscenario] ([pub_insertscenario_id]) 
	ON UPDATE NO ACTION ON DELETE NO ACTION
GO

------------------------------------------------------------------------

CREATE TABLE dbo.[assoc_database_sync] (
	[destination_db_name] VARCHAR(35) not null,
	[source_db_name] VARCHAR(35) not null)
GO
GRANT  SELECT ,  INSERT  ON [dbo].[assoc_database_sync]  TO [PMES_SuperAdmin], [PMES_RateAdmin]
GO
ALTER TABLE dbo.[assoc_database_sync] ADD CONSTRAINT [assoc_database_sync_PK] PRIMARY KEY ([destination_db_name])
GO

------------------------------------------------------------------------

ALTER TABLE [dbo].[est_assemdistriboptions]
	ALTER COLUMN insertfreightvendor_id bigint NOT NULL
GO

------------------------------------------------------------------------
UPDATE
	dbo.pst_postalcategoryscenario_map
SET
	percentage = percentage / 100
WHERE
	pst_postalscenario_id in
	(SELECT DISTINCT
		pst_postalscenario_id
	FROM 
		dbo.pst_postalcategoryscenario_map
	GROUP BY
		pst_postalscenario_id
	HAVING
		sum(percentage) = 100)

