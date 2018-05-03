IF OBJECT_ID('dbo.EstEstimate_Upload_Search') IS NOT NULL
	BEGIN
		PRINT 'Dropping dbo.EstEstimate_Upload_Search'
		DROP PROCEDURE dbo.EstEstimate_Upload_Search
		-- Tell user if drop failed.
		IF OBJECT_ID('dbo.EstEstimate_Upload_Search') IS NOT NULL
			PRINT ' *** Drop of dbo.EstEstimate_Upload_Search FAILED. *** '
	END
	GO
	
PRINT 'Creating dbo.EstEstimate_Upload_Search'
GO

CREATE PROCEDURE [dbo].[EstEstimate_Upload_Search] 
@Est_Estimate_ID bigint
AS

/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
* Performs pre-upload validation of Estimate
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate
*   EST_Component
*   EST_AssemDistribOptions
*   PPR_PaperWeight
*   PPR_PaperGrade
*   VND_Printer
*   PRT_PrinterRate
*   PPR_Paper_Map
*   VND_MailHouse
*   VND_MailTracker
*
*
* PROCEDURES CALLED:
*   dbo.TotalEstimateWeight
*
* DATABASE:
*		All
*
*
* RETURN VALUE:
* None 
*
*
* REVISION HISTORY:
*
* Date		Who		Comments
* ----------	---		-------------------------------------------------
* 08/25/2007	TJU		Initial Creation 
* 08/29/2007	BJS		Fixed join to PrinterRate table for plate cost
* 09/10/2007	BJS		Join to MailTracking table is a left join
* 09/18/2007	BJS		Renamed to EstEstimate_Upload_Search
*				Changed references to EST_PubInsertDates to EST_PubIssueDates
*				Replaced calls to Quantity functions with views
*				Removed costing calculations.  They are not needed for validation.
* 10/23/2007	JRH		Removed check for DBADVProd.informix.ctlg_pubvr_distbn.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/



Set nocount on

/* Comments correlate to layout of Report/Extract #4 - For Single Estimate Only */
create table #tempComponents(
	EST_Component_ID bigint,
	EST_Estimate_ID bigint,
	Parent_ID bigint,
	EST_ComponentType_ID int,
	EST_RunDate datetime,
	EST_Description varchar(35),
	ESTc_Description varchar(35),
	ESTc_MediaQtyWOInserts int,
	StatusCode int,				/* 100=Passed, 200= Warning, 300=Failed */
	Validation_msg varchar(150),

	/* <Main> */
	AdNumber int,

	/* <Quantity> */
	SoloQuantity int,
	PolybagQuantity int
)

/* Get Raw Production Data */
insert into #tempComponents
	(EST_Component_ID, EST_Estimate_ID, Parent_ID, EST_ComponentType_ID, EST_RunDate, EST_Description, 
	ESTc_Description, ESTc_MediaQtyWOInserts, AdNumber)

select 	c.EST_Component_ID, e.EST_Estimate_ID, e.Parent_ID, c.EST_ComponentType_ID, e.RunDate,
	e.Description, c.Description, 
	c.mediaqtywoinsert, c.AdNumber
from EST_Estimate e join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
where e.EST_Estimate_ID = @EST_Estimate_ID

/* Perform Quantity Calculations */
update tc
	set tc.SoloQuantity = isnull(sq.SoloQuantity, 0)
from #tempComponents tc left join vwComponentSoloQuantity sq on tc.EST_Component_ID = sq.EST_Component_ID

update tc
	set tc.PolybagQuantity = isnull(pq.PolybagQuantity, 0)
from #tempComponents tc left join vwComponentPolybagQuantity pq on tc.EST_Component_ID = pq.EST_Component_ID

/* set StatusCode = Validation passed */
update #tempComponents 
	set StatusCode = 100,
		Validation_msg = 'Valid'

/* check for warning conditions */
update 	#tempComponents 
	Set StatusCode = 200,
	Validation_msg = 'Missing Ad Number'
	where EST_ComponentType_ID <> 1 and	/* Component Type <> HOST */
	(AdNumber = 0 or AdNumber is NULL)	/* no adnumber */

update 	#tempComponents
Set 	StatusCode = 200,
	Validation_msg = 'Media Qty w/o Insert does not match'
where 	ESTc_MediaQtyWOInserts <> (SoloQuantity + PolybagQuantity) /* mediawoinsert <>  SoloQty + PolyQty*/


/* check for failed conditions */
update 	#tempComponents
Set 	StatusCode = 300,
	Validation_msg = 'Host Component Missing Ad Number'
where 	EST_ComponentType_ID = 1 and 		/* Component Type = HOST */
	(AdNumber = 0 or AdNumber is NULL)	/* no adnumber */

update tc
	set
		StatusCode = 300,
		Validation_msg = 'Admin System has no record for Ad Number ' + cast(tc.AdNumber as varchar)
from #tempComponents tc left join DBADVProd.informix.ad_est ae on tc.AdNumber = ae.ad_nbr
where tc.AdNumber is not null and ae.ad_nbr is null

create table #tempComponentPubLoc(
	EST_Component_ID bigint,
	Pub_ID char(3),
	PubLoc_ID int)

insert into #tempComponentPubLoc(EST_Component_ID, Pub_ID, PubLoc_ID)
select tc.EST_Component_ID, prm.Pub_ID, prm.PubLoc_ID
from #tempComponents tc join EST_PackageComponentMapping pcm on tc.EST_Component_ID = pcm.EST_Component_ID
	join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	join PUB_PubGroup pg on p.PUB_PubGroup_ID = pg.PUB_PubGroup_ID
	join PUB_PubPubGroup_Map ppgm on pg.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID
	join PUB_PubRate_Map prm on ppgm.PUB_PubRate_Map_ID = prm.PUB_PubRate_Map_ID
where tc.AdNumber is not null

select 
	EST_Estimate_ID,
	Parent_ID,
 	EST_RunDate, 
	EST_Description, 
	ESTc_Description, 
	AdNumber,
	StatusCode,
	Validation_msg
from #tempComponents
order by EST_Estimate_ID, EST_Component_ID

set nocount off

drop table #tempComponents
drop table #tempComponentPubLoc

GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_Upload_Search]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
