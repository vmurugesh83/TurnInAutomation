IF OBJECT_ID('dbo.rpt_PubChange_UploadData') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.rpt_PubChange_UploadData'
	DROP PROCEDURE dbo.rpt_PubChange_UploadData
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.rpt_PubChange_UploadData') IS NOT NULL
		PRINT '***********Drop of dbo.rpt_PubChange_UploadData FAILED.'
END
GO
PRINT 'Creating dbo.rpt_PubChange_UploadData'
GO

CREATE PROC dbo.rpt_PubChange_UploadData
/*
* PARAMETERS:
* EstimateIDs - Xml formatted list of estimates to query.
*
* DESCRIPTION:
*		Returns information for pub change report.
*
*
* TABLES:
*   Table Name                              Access
*   ==========                              ======
*   EST_Estimate                            READ
*   EST_Component                           READ
*   ADMINSYSTEM.informix.ad_est             READ
*   ADMINSYSTEM.informix.pub_cost_est       READ
*   ADMINSYSTEM.informix.pub                READ
*   ADMINSYSTEM.informix.ctlg_pubvr_distbn  READ
*
*
* PROCEDURES CALLED:
*   None
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
* Date            Who             Comments
* -------------   --------        ------------------------------------------------------------------
* 09/25/2007      BJS             Initial Creation
* 12/16/2007      BJS             Modified EstimateIDs parameter to allow for a larger set of estimates.
* 12/18/2007      BJS             Added #tempAdNumbers.
* 05/21/2008      BJS             Modified @EstimateIDs from varchar to text.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EstimateIDs text
as

set nocount on

declare @EstimateDocID int

create table #tempEstimate(EST_Estimate_ID BIGINT NOT NULL)

exec sp_xml_preparedocument @EstimateDocID output, @EstimateIDs
insert into #tempEstimate(EST_Estimate_ID)
select EST_Estimate_ID
from OPENXML(@EstimateDocID, '/root/estimate')
with(est_estimate_id BIGINT '@id')

/* Gather the Ad Numbers used by the estimates */
create table #tempAdNumbers(
	AdNumber INT NOT NULL,
	RunDate datetime NOT NULL,
	FiscalSeason varchar(35) NOT NULL,
	FiscalYear INT NOT NULL,
	FiscalMonth INT NOT NULL)
insert into #tempAdNumbers(AdNumber, RunDate, FiscalSeason, FiscalYear, FiscalMonth)
select c.AdNumber, max(e.RunDate), max(es.Description), max(e.FiscalYear), max(e.FiscalMonth)
from EST_Component c join #tempEstimate te on c.EST_Estimate_ID = te.EST_Estimate_ID
	join EST_Estimate e on te.EST_Estimate_ID = e.EST_Estimate_ID
	join EST_Season es on e.EST_Season_ID = es.EST_Season_ID
where c.AdNumber is not null
group by c.AdNumber

set nocount off

select max(tad.RunDate) RunDate, ae.ad_nbr AdNumber,
	max(ae.estd_ad_desc) AdDescription, pe.pub_id, max(p.pub_nm) PubDesc, pe.publoc_id, max(pd.media_qty) MediaQuantity,
	max(pd.vrsn_pub_issue_dt) IssueDate
from #tempAdNumbers tad join DBADVProd.informix.ad_est ae on tad.AdNumber = ae.ad_nbr
	join DBADVProd.informix.pub_cost_est pe on ae.ad_nbr = pe.ad_nbr
	join DBADVProd.informix.pub p on pe.pub_id = p.pub_id
	join DBADVProd.informix.ctlg_pubvr_distbn pd on ae.ad_nbr = pd.ad_nbr and pe.pub_id = pd.pub_id and pe.publoc_id = pd.publoc_id
group by ae.ad_nbr, pe.pub_id, pe.publoc_id
order by tad.RunDate, ae.ad_nbr, pe.pub_id, pe.publoc_id

set nocount on
drop table #tempEstimate
drop table #tempAdNumbers
GO

GRANT  EXECUTE  ON [dbo].[rpt_PubChange_UploadData]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

