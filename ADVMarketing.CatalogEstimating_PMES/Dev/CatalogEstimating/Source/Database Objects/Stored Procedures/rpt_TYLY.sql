IF OBJECT_ID('dbo.rpt_TYLY') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.rpt_TYLY'
	DROP PROCEDURE dbo.rpt_TYLY
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.rpt_TYLY') IS NOT NULL
		PRINT '***********Drop of dbo.rpt_TYLY FAILED.'
END
GO
PRINT 'Creating dbo.rpt_TYLY'
GO

CREATE PROC dbo.rpt_TYLY
/*
* PARAMETERS:
*	EST_Season_ID - optional
*   FiscalYear
*   VendorSupplied    - 1 = All Components, 2 = Only VS Components, 3 = Exclude VS Components
*   EstimateMediaType - Xml formatted list of media types to query.  If null, all media types are queried.
*   ComponentType     - Xml formatted list of component types to query.  If null, all component types are queried.
*
* DESCRIPTION:
*	Returns data used by the This Year - Last Year Comparison Report.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Estimate                    READ
*   EST_Component                   READ
    DBADVProd.informix.ad_est       READ
*   DBADVProd.informix.ad_cost_est  READ
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/01/2007      BJS             Initial Creation
* 11/19/2007      JRH             Fixed Season.
*
*/
@EST_Season_ID int,
@FiscalYear int,
@VendorSupplied tinyint,
@EstimateMediaType varchar(2000),
@ComponentType varchar(2000)
as

set nocount on

declare @MediaTypeDocID int, @ComponentTypeDocID int

create table #tempEstimateMediaType(EST_EstimateMediaType_ID INT NOT NULL, Description VARCHAR(35) NOT NULL)
create table #tempComponentType(EST_ComponentType_ID INT NOT NULL, Description VARCHAR(35) NOT NULL)

if (@EstimateMediaType is null) begin
	insert into #tempEstimateMediaType(EST_EstimateMediaType_ID, Description)
	select EST_EstimateMediaType_ID, Description
	from EST_EstimateMediaType
end
else begin
	exec sp_xml_preparedocument @MediaTypeDocID output, @EstimateMediaType
	insert into #tempEstimateMediaType(EST_EstimateMediaType_ID, Description)
	select emt.EST_EstimateMediaType_ID, emt.Description
	from OPENXML(@MediaTypeDocID, '/root/est_estimatemediatype')
	with(est_estimatemediatype_id INT '@est_estimatemediatype_id') xdata
		join EST_EstimateMediaType emt on xdata.EST_EstimateMediaType_ID = emt.EST_EstimateMediaType_ID
end

if (@ComponentType is null) begin
	insert into #tempComponentType(EST_ComponentType_ID, Description)
	select EST_ComponentType_ID, Description
	from EST_ComponentType
end
else begin
	exec sp_xml_preparedocument @ComponentTypeDocID output, @ComponentType
	insert into #tempComponentType(EST_ComponentType_ID, Description)
	select ct.EST_ComponentType_ID, ct.Description
	from OPENXML(@ComponentTypeDocID, '/root/est_componenttype')
	with(est_componenttype_id INT '@est_componenttype_id') xdata
		join EST_ComponentType ct on xdata.EST_ComponentType_ID = ct.EST_ComponentType_ID
end

--Retrieve Ad Numbers that match the search criteria
create table #tempAdNumbers(AdNumber int, AdDesc varchar(35), RunDate datetime, FiscalYear int, EST_Season_ID int, SeasonDesc varchar(35))
insert into #tempAdNumbers(AdNumber, RunDate, FiscalYear, EST_Season_ID, SeasonDesc)
select c.AdNumber, max(e.RunDate) RunDate, max(e.FiscalYear) FiscalYear, es.EST_Season_ID, max(es.Description) SeasonDesc
from EST_Estimate e join vw_Estimate_ExcludeOldUploads eo on e.EST_Estimate_ID = eo.EST_Estimate_ID
	join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
	join EST_Season es on e.EST_Season_ID = es.EST_Season_ID
	join #tempEstimateMediaType emt on c.EST_EstimateMediaType_ID = emt.EST_EstimateMediaType_ID
	join #tempComponentType ct on c.EST_ComponentType_ID = ct.EST_ComponentType_ID
where e.EST_Status_ID = 2
	and (@EST_Season_ID is null or e.EST_Season_ID = @EST_Season_ID)
	and e.FiscalYear = @FiscalYear
	and (@VendorSupplied = 1 or (@VendorSupplied = 2 and c.VendorSupplied = 1) or (@VendorSupplied = 3 and c.VendorSupplied = 0))
	and c.AdNumber is not null
group by c.AdNumber, es.EST_Season_ID

update #tempAdNumbers
set AdDesc = dbo.FindComponentDescription(AdNumber, RunDate, @VendorSupplied)

set nocount off

select tad.AdNumber, max(tad.RunDate) RunDate, max(tad.FiscalYear) FiscalYear, tad.EST_Season_ID SeasonID, max(tad.SeasonDesc) SeasonDesc,
	tad.AdDesc Description, max(ae.estd_base_pg_qty) PageCount, max(ae.estd_media_qty) MediaQuantity,
	sum(ace.est_ad_cost_amt) EstimatedCost
from #tempAdNumbers tad join DBADVProd.informix.ad_est ae on tad.AdNumber = ae.ad_nbr
	join DBADVProd.informix.ad_cost_est ace on ae.ad_nbr = ace.ad_nbr
group by tad.AdNumber, tad.AdDesc, tad.EST_Season_ID
order by tad.RunDate, tad.AdNumber, tad.AdDesc

set nocount on

drop table #tempAdNumbers
drop table #tempEstimateMediaType
drop table #tempComponentType
GO

GRANT  EXECUTE  ON [dbo].[rpt_TYLY]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO