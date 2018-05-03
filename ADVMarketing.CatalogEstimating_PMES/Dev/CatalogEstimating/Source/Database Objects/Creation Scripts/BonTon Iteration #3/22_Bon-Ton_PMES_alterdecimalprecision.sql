ALTER TABLE est_component ALTER COLUMN spoilagepct decimal(10,4) NULL
GO
ALTER TABLE est_component ALTER COLUMN earlypayprintdiscount decimal(10,4) NULL
GO
ALTER TABLE est_component ALTER COLUMN printertaxablemediapct decimal(10,4) NULL
GO
ALTER TABLE est_component ALTER COLUMN printersalestaxpct decimal(10,4) NULL
GO
ALTER TABLE est_component ALTER COLUMN earlypaypaperdiscount decimal(10,4) NULL
GO
ALTER TABLE est_component ALTER COLUMN papertaxablemediapct decimal(10,4) NULL
GO
ALTER TABLE est_component ALTER COLUMN papersalestaxpct decimal(10,4) NULL
GO

ALTER TABLE est_assemdistriboptions ALTER COLUMN insertfuelsurcharge decimal(10,4) NOT NULL
GO
ALTER TABLE est_assemdistriboptions ALTER COLUMN mailfuelsurcharge decimal(10,4) NOT NULL
GO

ALTER TABLE est_packagepolybag_map ALTER COLUMN distributionpct decimal(10,4) NULL
GO

ALTER TABLE pub_insertdiscounts ALTER COLUMN discount decimal(10,4) NOT NULL
GO

ALTER TABLE pst_postalcategoryscenario_map ALTER COLUMN percentage decimal(10,4) NOT NULL
GO

UPDATE EST_AssemDistribOptions
	set
		insertfuelsurcharge = insertfuelsurcharge / 100,
		mailfuelsurcharge = mailfuelsurcharge / 100
GO

UPDATE EST_PackagePolybag_Map
	set
		distributionpct = distributionpct / 100
GO

UPDATE PUB_InsertDiscounts
	set
		discount = discount / 100
GO
