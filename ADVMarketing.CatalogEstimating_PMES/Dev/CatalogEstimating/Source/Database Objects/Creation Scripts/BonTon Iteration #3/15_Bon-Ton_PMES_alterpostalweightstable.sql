alter table dbo.[pst_postalweights]
	alter column [firstoverweightlimit] decimal(12, 4) NOT NULL
go

alter table dbo.[pst_postalweights]
	alter column [standardoverweightlimit] decimal(12, 4) NOT NULL
go

