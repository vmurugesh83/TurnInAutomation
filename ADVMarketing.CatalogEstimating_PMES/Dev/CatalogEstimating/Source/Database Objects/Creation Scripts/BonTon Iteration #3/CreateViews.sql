IF OBJECT_ID('dbo.vwComponentInsertQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vwComponentInsertQuantity'
	DROP VIEW dbo.vwComponentInsertQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vwComponentInsertQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.vwComponentInsertQuantity FAILED.'
END
GO
PRINT 'Creating dbo.vwComponentInsertQuantity'
GO

CREATE VIEW dbo.vwComponentInsertQuantity
/*
* DESCRIPTION:
*		Returns ComponentID, EstimateID and Component Insert Quantity.
*
*
* TABLES:
*   Table Name                  Access
*   ==========                  ======
*   EST_PackageComponentMapping READ
*   EST_Package           READ
*   PUB_PubGroup          READ
*   PUB_PubPubGroup_Map   READ
*   EST_PubIssueDates     READ
*   PUB_PubQuantity       READ
*   PUB_DayOfWeekQuantity READ
*
* PROCEDURES CALLED:
*   dbo.IsPubRateMapActive
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/13/2007      BJS             Initial Creation
*/
AS
SELECT pcm.EST_Component_ID, max(p.EST_Estimate_ID) EST_Estimate_ID, sum(dowqty.Quantity) InsertQuantity
FROM EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	join PUB_PubGroup pg on p.PUB_PubGroup_ID = pg.PUB_PubGroup_ID
	join PUB_PubPubGroup_Map pppgm on pg.PUB_PubGroup_ID = pppgm.PUB_PubGroup_ID
	join EST_PubIssueDates pid on p.EST_Estimate_ID = pid.EST_Estimate_ID and pppgm.PUB_PubRate_Map_ID = pid.PUB_PubRate_Map_ID
	join PUB_PubQuantity ppq on pppgm.PUB_PubRate_Map_ID = ppq.PUB_PubRate_Map_ID
		and ppq.PUB_PubQuantity_ID = dbo.CalcPubQuantityID(pppgm.PUB_PubRate_Map_ID, pid.IssueDate)
	join PUB_DayOfWeekQuantity dowqty on ppq.PUB_PubQuantity_ID = dowqty.PUB_PubQuantity_ID
		and p.PUB_PubQuantityType_ID = dowqty.PUB_PubQuantityType_ID
WHERE dbo.IsPubRateMapActive(pppgm.PUB_PubRate_Map_ID, pid.IssueDate) = 1
	and (/*holidays*/ p.PUB_PubQuantityType_ID > 3 or /*fullrun / contract send*/ pid.IssueDOW = dowqty.InsertDow)
GROUP BY pcm.EST_Component_ID
GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vwComponentInsertQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.vwComponentOtherQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vwComponentOtherQuantity'
	DROP VIEW dbo.vwComponentOtherQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vwComponentOtherQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.vwComponentOtherQuantity FAILED.'
END
GO
PRINT 'Creating dbo.vwComponentOtherQuantity'
GO

CREATE VIEW dbo.vwComponentOtherQuantity
/*
* DESCRIPTION:
*		Returns ComponentID, EstimateID and Component Other Quantity.
*
*
* TABLES:
*   Table Name                  Access
*   ==========                  ======
*   EST_PackageComponentMapping READ
*   EST_Package                 READ
*
* PROCEDURES CALLED:
*   None
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/13/2007      BJS             Initial Creation
*/
AS
SELECT pcm.EST_Component_ID, max(p.EST_Estimate_ID) EST_Estimate_ID, sum(p.OtherQuantity) OtherQuantity
FROM EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
GROUP BY pcm.EST_Component_ID
GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vwComponentOtherQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.vwComponentPolybagQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vwComponentPolybagQuantity'
	DROP VIEW dbo.vwComponentPolybagQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vwComponentPolybagQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.vwComponentPolybagQuantity FAILED.'
END
GO
PRINT 'Creating dbo.vwComponentPolybagQuantity'
GO

CREATE VIEW dbo.vwComponentPolybagQuantity
/*
* DESCRIPTION:
*		Returns ComponentID, EstimateID and Component Polybag Mail Quantity.
*
*
* TABLES:
*   Table Name                  Access
*   ==========                  ======
*   EST_PackageComponentMapping READ
*   EST_Package                 READ
*
* PROCEDURES CALLED:
*   None
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/13/2007      BJS             Initial Creation
*/
AS
SELECT pcm.EST_Component_ID, max(p.EST_Estimate_ID) EST_Estimate_ID, sum(pb.Quantity) PolybagQuantity
FROM EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	join EST_PackagePolyBag_Map ppbm on p.EST_Package_ID = ppbm.EST_Package_ID
	join EST_Polybag pb on ppbm.EST_PolyBag_ID = pb.EST_Polybag_ID
GROUP BY pcm.EST_Component_ID
GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vwComponentPolybagQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.vwComponentSampleQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vwComponentSampleQuantity'
	DROP VIEW dbo.vwComponentSampleQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vwComponentSampleQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.vwComponentSampleQuantity FAILED.'
END
GO
PRINT 'Creating dbo.vwComponentSampleQuantity'
GO

CREATE VIEW dbo.vwComponentSampleQuantity
/*
* DESCRIPTION:
*		Returns ComponentID, EstimateID and Component Sample Quantity.
*
*
* TABLES:
*   Table Name                  Access
*   ==========                  ======
*   EST_Component               READ
*   EST_Samples                 READ
*
* PROCEDURES CALLED:
*   None
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/13/2007      BJS             Initial Creation
* 11/26/2007      JRH             Only for Host
*
*/
AS
SELECT
	c.EST_Component_ID, 
	c.EST_Estimate_ID, 
	case c.est_componenttype_id
		when 1 then s.Quantity
		else 0
	end SampleQuantity
FROM EST_Component c join EST_Samples s on c.EST_Estimate_ID = s.EST_Estimate_ID

GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vwComponentSampleQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.vwComponentSoloQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vwComponentSoloQuantity'
	DROP VIEW dbo.vwComponentSoloQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vwComponentSoloQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.vwComponentSoloQuantity FAILED.'
END
GO
PRINT 'Creating dbo.vwComponentSoloQuantity'
GO

CREATE VIEW dbo.vwComponentSoloQuantity
/*
* DESCRIPTION:
*		Returns ComponentID, EstimateID and Component Solo Mail Quantity.
*
*
* TABLES:
*   Table Name                  Access
*   ==========                  ======
*   EST_PackageComponentMapping READ
*   EST_Package                 READ
*
* PROCEDURES CALLED:
*   None
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/13/2007      BJS             Initial Creation
*/
AS
SELECT pcm.EST_Component_ID, max(p.EST_Estimate_ID) EST_Estimate_ID, sum(p.SoloQuantity) SoloQuantity
FROM EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
GROUP BY pcm.EST_Component_ID
GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vwComponentSoloQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.vwComponentWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vwComponentWeight'
	DROP VIEW dbo.vwComponentWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vwComponentWeight') IS NOT NULL
		PRINT '***********Drop of dbo.vwComponentWeight FAILED.'
END
GO
PRINT 'Creating dbo.vwComponentWeight'
GO

CREATE VIEW dbo.vwComponentWeight
/*
* DESCRIPTION:
*		Returns ComponentID, EstimateID and Component Weight.
*
*
* TABLES:
*   Table Name                  Access
*   ==========                  ======
*   EST_Component               READ
*   PPR_PaperWeight             READ
*
* PROCEDURES CALLED:
*   None
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/13/2007      BJS             Initial Creation
* 10/15/2007      BJS             Added additional precision to the ComponentPieceWeight
*
*/
AS
SELECT c.EST_Component_ID, max(c.EST_Estimate_ID) EST_Esttimate_ID,
	cast(max(c.Width) * max(c.Height) / cast(950000 as decimal) * max(c.PageCount) * max(pw.Weight) * 1.03 as decimal(12,6)) as ComponentPieceWeight
FROM EST_Component c join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
GROUP BY c.EST_Component_ID
GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vwComponentWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.vwPackageWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vwPackageWeight'
	DROP VIEW dbo.vwPackageWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vwPackageWeight') IS NOT NULL
		PRINT '***********Drop of dbo.vwPackageWeight FAILED.'
END
GO
PRINT 'Creating dbo.vwPackageWeight'
GO

CREATE VIEW dbo.vwPackageWeight
/*
* DESCRIPTION:
*		Returns ComponentID, EstimateID and Component Weight.
*
*
* TABLES:
*   Table Name                  Access
*   ==========                  ======
*   EST_Component               READ
*   PPR_PaperWeight             READ
*
* PROCEDURES CALLED:
*   None
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 11/19/2007      JRH             Initial Creation
*
*/
AS
SELECT 
	EST_Package_ID
	, dbo.PackageWeight(EST_Package_ID) PackageWeight
FROM EST_Package 
GROUP BY EST_Package_ID
GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vwPackageWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.vw_Estimate_excludeOldUploads') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vw_Estimate_excludeOldUploads'
	DROP VIEW dbo.vw_Estimate_excludeOldUploads
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vw_Estimate_excludeOldUploads') IS NOT NULL
		PRINT '***********Drop of dbo.vw_Estimate_excludeOldUploads FAILED.'
END
GO
PRINT 'Creating dbo.vw_Estimate_excludeOldUploads'
GO

create view dbo.vw_Estimate_excludeOldUploads
/*
* DESCRIPTION:
*		Returns Estimate IDs for all Active, Killed and most recently uploaded estimates.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/21/2007      BJS             Initial Creation
*/
as
select max(EST_Estimate_ID) EST_Estimate_ID
from EST_Estimate
where Parent_ID is not null
group by Parent_ID, EST_Status_ID
union
select EST_Estimate_ID
from EST_Estimate
where Parent_ID is null
GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vw_Estimate_excludeOldUploads]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.vw_PubRateMap_withPubAndPublocNames') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vw_PubRateMap_withPubAndPublocNames'
	DROP VIEW dbo.vw_PubRateMap_withPubAndPublocNames
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vw_PubRateMap_withPubAndPublocNames') IS NOT NULL
		PRINT '***********Drop of dbo.vw_PubRateMap_withPubAndPublocNames FAILED.'
END
GO
PRINT 'Creating dbo.vw_PubRateMap_withPubAndPublocNames'
GO

create view dbo.vw_PubRateMap_withPubAndPublocNames
/*
* DESCRIPTION:
*		Returns PubRateMap record, pub description and pub loc description.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   
*
* REVISION HISTORY:
*
* Date		Who		Comments
* ----------	-----		-------------------------------------------------
* 09/11/2007	JRH		Initial Creation
*/
AS

SELECT
	prm.pub_pubrate_map_id
	, prm.pub_id
	, prm.publoc_id
	, p.pub_nm
	, pl.publoc_nm
FROM
	dbo.pub_pubrate_map prm (nolock)
	INNER JOIN DBADVProd.informix.pub p (nolock)
		ON prm.pub_id = p.pub_id
	INNER JOIN DBADVProd.informix.pub_loc pl (nolock)
		ON prm.pub_id = pl.pub_id
		AND prm.publoc_id = pl.publoc_id

GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vw_PubRateMap_withPubAndPublocNames]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.vwComponentMediaQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vwComponentMediaQuantity'
	DROP VIEW dbo.vwComponentMediaQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vwComponentMediaQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.vwComponentMediaQuantity FAILED.'
END
GO
PRINT 'Creating dbo.vwComponentMediaQuantity'
GO

CREATE VIEW dbo.vwComponentMediaQuantity
/*
* DESCRIPTION:
*		Returns ComponentID, EstimateID and Component Media Quantity.
*
*
* TABLES:
*   Table Name                 Access
*   ==========                 ======
*   EST_Component              READ
*   vwComponentInsertQuantity  READ
*   vwComponentSoloQuantity    READ
*   vwComponentPolybagQuantity READ
*
* PROCEDURES CALLED:
*   None
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/13/2007      BJS             Initial Creation
*/
AS
SELECT c.EST_Component_ID, max(c.EST_Estimate_ID) EST_Estimate_ID,
	isnull(max(iq.InsertQuantity), 0) + isnull(max(sq.SoloQuantity), 0) + isnull(max(pq.PolybagQuantity), 0) as MediaQuantity
FROM EST_Component c left join vwComponentInsertQuantity iq on c.EST_Component_ID = iq.EST_Component_ID
	left join vwComponentSoloQuantity sq on c.EST_Component_ID = sq.EST_Component_ID
	left join vwComponentPolybagQuantity pq on c.EST_Component_ID = pq.EST_Component_ID
GROUP BY c.EST_Component_ID
GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vwComponentMediaQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.vwTotalEstimateWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.vwTotalEstimateWeight'
	DROP VIEW dbo.vwTotalEstimateWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.vwTotalEstimateWeight') IS NOT NULL
		PRINT '***********Drop of dbo.vwTotalEstimateWeight FAILED.'
END
GO
PRINT 'Creating dbo.vwTotalEstimateWeight'
GO

CREATE VIEW dbo.vwTotalEstimateWeight
/*
* DESCRIPTION:
*		Returns EstimateID and Total Estimate Weight.
*
*
* TABLES:
*   Table Name                Access
*   ==========                ======
*   EST_Component             READ
*   vwComponentWeight         READ
*   vwComponentMediaQuantity  READ
*   vwComponentOtherQuantity  READ
*   vwComponentSampleQuantity READ
*
* PROCEDURES CALLED:
*   None
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/13/2007      BJS             Initial Creation
*/
as
SELECT c.EST_Estimate_ID, sum(cw.ComponentPieceWeight * (mq.MediaQuantity * (1 + isnull(c.SpoilagePct, 0))
	+ isnull(oq.OtherQuantity, 0) + isnull(sq.SampleQuantity, 0))) TotalEstimateWeight
FROM EST_Component c join vwComponentWeight cw on c.EST_Component_ID = cw.EST_Component_ID
	join vwComponentMediaQuantity mq on c.EST_Component_ID = mq.EST_Component_ID
	left join vwComponentOtherQuantity oq on c.EST_Component_ID = oq.EST_Component_ID
	left join vwComponentSampleQuantity sq on c.EST_Component_ID = sq.EST_Component_ID
GROUP BY c.EST_Estimate_ID
GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vwTotalEstimateWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
