UPDATE dbo.EST_Component
	SET StitcherMakeready = null,
		PressMakeready = null
GO

exec sp_rename 'EST_Component.stitchermakeready', 'stitchermakeready_id', 'COLUMN'
GO

exec sp_rename 'EST_Component.pressmakeready', 'pressmakeready_id', 'COLUMN'
GO

ALTER TABLE EST_Component
	ALTER COLUMN stitchermakeready_id BIGINT NULL
GO

ALTER TABLE EST_Component
	ALTER COLUMN pressmakeready_id BIGINT NULL
GO

ALTER TABLE EST_Component
	ADD digitalhandlenprepare_id BIGINT NULL
GO

ALTER TABLE dbo.[est_component] ADD CONSTRAINT [prt_printerrate_est_component_FK16] FOREIGN KEY ([platecost_id])
	REFERENCES dbo.[prt_printerrate] ([prt_printerrate_id])
	ON UPDATE no action
	ON DELETE no action
go

ALTER TABLE dbo.[est_component] ADD CONSTRAINT [prt_printerrate_est_component_FK17] FOREIGN KEY ([stitchermakeready_id])
	REFERENCES dbo.[prt_printerrate] ([prt_printerrate_id])
	ON UPDATE no action
	ON DELETE no action
go

ALTER TABLE dbo.[est_component] ADD CONSTRAINT [prt_printerrate_est_component_FK18] FOREIGN KEY ([pressmakeready_id])
	REFERENCES dbo.[prt_printerrate] ([prt_printerrate_id])
	ON UPDATE no action
	ON DELETE no action
go

ALTER TABLE dbo.[est_component] ADD CONSTRAINT [prt_printerrate_est_component_FK19] FOREIGN KEY ([digitalhandlenprepare_id])
	REFERENCES dbo.[prt_printerrate] ([prt_printerrate_id])
	ON UPDATE no action
	ON DELETE no action
go

insert into prt_printerratetype(prt_printerratetype_id, description, gridtitle, displayorder, createdby, createddate)
values(10, 'Press Makeready', 'Per Plant Cost', 10, 'admin', getdate())
go
