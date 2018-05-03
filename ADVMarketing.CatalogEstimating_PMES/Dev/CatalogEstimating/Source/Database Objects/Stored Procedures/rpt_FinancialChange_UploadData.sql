IF OBJECT_ID('dbo.rpt_FinancialChange_UploadData') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.rpt_FinancialChange_UploadData'
	DROP PROCEDURE dbo.rpt_FinancialChange_UploadData
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.rpt_FinancialChange_UploadData') IS NOT NULL
		PRINT '***********Drop of dbo.rpt_FinancialChange_UploadData FAILED.'
END
GO
PRINT 'Creating dbo.rpt_FinancialChange_UploadData'
GO

CREATE PROC dbo.rpt_FinancialChange_UploadData
/*
* PARAMETERS:
* EstimateIDs - Xml formatted list of estimates to query.
*
* DESCRIPTION:
*		Returns information for financial change report.
*
*
* TABLES:
*   Table Name                  Access
*   ==========                  ======
*   EST_Estimate                READ
*   EST_Component               READ
*   ADMINSYSTEM.informix.ad_est READ
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
* 09/10/2007      BJS             Initial Creation
* 12/16/2007      BJS             Modified EstimateIDs parameter and xml schema to allow for more estimates
* 12/17/2007      JRH             Drop #tempAdNumbers temp table.
* 12/18/2007      BJS             Changed population of #tempAdNumbers to prevent duplicate ad numbers from
*                                 being inserted and multiplying costs.
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

set nocount off

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

select max(tad.RunDate) RunDate, max(tad.FiscalSeason) Season, max(tad.FiscalYear) FiscalYear, max(tad.FiscalMonth) FiscalMonth,
	ae.ad_nbr AdNumber, max(ae.estd_ad_desc) AdDescription, max(ae.estd_media_qty) MediaQty, cast(max(ae.estd_base_pg_qty) as int) PageCount,
	sum(ce.est_ad_cost_amt) AdCost
from #tempAdNumbers tad	join DBADVProd.informix.ad_est ae on tad.AdNumber = ae.ad_nbr
	join DBADVProd.informix.ad_cost_est ce on ae.ad_nbr = ce.ad_nbr
group by ae.ad_nbr
order by e.FiscalYear, e.FiscalMonth, ae.ad_nbr

set nocount on
drop table #tempEstimate
drop table #tempAdNumbers
GO

GRANT  EXECUTE  ON [dbo].[rpt_FinancialChange_UploadData]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
