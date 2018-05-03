ALTER TABLE dbo.[est_component]
	ADD [assemblyvendor_id] BIGINT null
GO
ALTER TABLE dbo.[est_component] ADD CONSTRAINT [vnd_printer_est_component_FK19] FOREIGN KEY ([assemblyvendor_id])
	REFERENCES dbo.[vnd_printer] ([vnd_printer_id])
	ON UPDATE no action
	ON DELETE no action
GO

UPDATE dbo.[est_component]
SET assemblyvendor_id = printer_id