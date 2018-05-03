ALTER TABLE pub_pubrate_map
	ADD CONSTRAINT UQ_pub_pubrate_map UNIQUE (pub_id, publoc_id)
GO

ALTER TABLE pub_pubrate_map_activate
	ADD CONSTRAINT UQ_pub_pubrate_map_activate UNIQUE (pub_pubrate_map_id, effectivedate)
GO

ALTER TABLE pub_pubrate
	ADD CONSTRAINT UQ_pub_pubrate UNIQUE (pub_pubrate_map_id, effectivedate)
GO

ALTER TABLE pub_dayofweekratetypes
	ADD CONSTRAINT UQ_pub_dayofweekratetypes UNIQUE (pub_pubrate_id, ratetypedescription)
GO

ALTER TABLE pub_dayofweekrates
	ADD CONSTRAINT UQ_pubdayofweekrates UNIQUE (pub_dayofweekratetypes_id, insertdow)
GO

ALTER TABLE pub_pubquantity
	ADD CONSTRAINT UQ_pub_pubquantity UNIQUE (pub_pubrate_map_id, effectivedate)
GO

ALTER TABLE pub_dayofweekquantity
	ADD CONSTRAINT UQ_pub_dayofweekquantity UNIQUE (pub_pubquantity_id, pub_pubquantitytype_id, insertdow)
GO

ALTER TABLE pub_insertscenario
	ADD CONSTRAINT UQ_pub_insertscenario UNIQUE (description)
GO

ALTER TABLE vnd_vendor
	ADD CONSTRAINT UQ_vnd_vendor UNIQUE (description)
GO

ALTER TABLE vnd_printer
	ADD CONSTRAINT UQ_vnd_printer UNIQUE (vnd_vendor_id, effectivedate)
GO

ALTER TABLE vnd_paper
	ADD CONSTRAINT UQ_vnd_paper UNIQUE (vnd_vendor_id, effectivedate)
GO

ALTER TABLE vnd_mailhouserate
	ADD CONSTRAINT UQ_vnd_mailhouserate UNIQUE (vnd_vendor_id, effectivedate)
GO

ALTER TABLE vnd_mailtrackingrate
	ADD CONSTRAINT UQ_vnd_mailtrackingrate UNIQUE (vnd_vendor_id, effectivedate)
GO

ALTER TABLE vnd_maillistresourcerate
	ADD CONSTRAINT UQ_vnd_maillistresourcerate UNIQUE (vnd_vendor_id, effectivedate)
GO

ALTER TABLE prt_printerrate
	ADD CONSTRAINT UQ_prt_printerrate UNIQUE (vnd_printer_id, prt_printerratetype_id, description)
GO

ALTER TABLE ppr_paper_map
	ADD CONSTRAINT UQ_ppr_paper_map UNIQUE (vnd_paper_id, ppr_papergrade_id, ppr_paperweight_id, description)
GO

ALTER TABLE pst_postalscenario
	ADD CONSTRAINT UQ_pst_postalscenario UNIQUE (description, effectivedate)
GO

ALTER TABLE pst_postalcategory
	ADD CONSTRAINT UQ_pst_postalcategory UNIQUE (description)
GO

ALTER TABLE pst_postalweights
	ADD CONSTRAINT UQ_pst_postalweights UNIQUE (vnd_vendor_id, effectivedate)
GO
