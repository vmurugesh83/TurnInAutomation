ALTER TABLE est_component
	ALTER COLUMN creativevendor_id BIGINT NULL
GO

ALTER TABLE est_component
	ALTER COLUMN separator_id BIGINT NULL
GO

ALTER TABLE est_component
	ALTER COLUMN printer_id BIGINT NULL
GO

ALTER TABLE est_component
	ALTER COLUMN paper_id BIGINT NULL
GO

ALTER TABLE est_component
	ALTER COLUMN papergrade_id INT NULL
GO

ALTER TABLE est_component
	ADD stitchermakereadyrate MONEY NULL
GO

ALTER TABLE est_component
	ADD pressmakereadyrate MONEY NULL
GO
