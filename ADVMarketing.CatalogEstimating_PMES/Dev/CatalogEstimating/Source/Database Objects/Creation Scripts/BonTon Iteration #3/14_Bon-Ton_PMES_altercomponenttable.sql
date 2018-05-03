alter table dbo.[est_component] drop constraint [prt_printerrate_est_component_FK10]
go

alter table dbo.[est_component]
	alter column [separator_id] bigint NOT NULL
go

exec sp_rename 'dbo.est_component.onsertrate', 'onsert_id', 'COLUMN'
go

alter table dbo.[est_component]
	alter column [onsert_id] bigint NULL
go

alter table dbo.[est_component] add constraint [vnd_vendor_est_component_FK10] foreign key ([separator_id])
	references dbo.[vnd_vendor] ([vnd_vendor_id])
	on update no action on delete no action
go

alter table dbo.[est_component] add constraint [prt_printerrate_est_component_FK15] foreign key ([onsert_id])
	references dbo.[prt_printerrate] ([prt_printerrate_id])
	on update no action on delete no action
go
