ALTER TABLE est_component
	DROP CONSTRAINT ppr_paper_map_est_component_FK14
GO

ALTER TABLE est_component
	DROP COLUMN paperdescription_id
GO

ALTER TABLE est_component
	ALTER COLUMN runpounds decimal(10,2) NULL
GO

ALTER TABLE est_component
	ALTER COLUMN platechangepounds decimal(10,2) NULL
GO

