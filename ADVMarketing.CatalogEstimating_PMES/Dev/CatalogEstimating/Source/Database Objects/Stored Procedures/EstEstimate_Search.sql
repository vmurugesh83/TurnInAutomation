IF OBJECT_ID('dbo.EstEstimate_Search') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstEstimate_Search'
	DROP PROCEDURE dbo.EstEstimate_Search
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstEstimate_Search') IS NOT NULL
		PRINT '***********Drop of dbo.EstEstimate_Search FAILED.'
END
GO
PRINT 'Creating dbo.EstEstimate_Search'
GO

CREATE proc dbo.EstEstimate_Search
/*
* PARAMETERS:
*
*
*
* DESCRIPTION:
*		Estimates are found based on search criteria for the Estimate Search screen.  At least one parameter is required.
*
*
* TABLES:
*   Table Name			Access
*   ==========			======
*   EST_Estimate		READ
*   EST_Component		READ
*   EST_Package			READ
*   EST_PubInsertDates		READ
*   EST_EstimateMediaType	READ
*   EST_Season			READ
*   EST_Status			READ
*
*
* PROCEDURES CALLED:
*
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
* ----------	----		-------------------------------------------------
* 05/25/2007	BJS		Initial Creation 
* 09/11/2007	NLS		Added SortOrder column and fixed UploadDate issue
* 09/13/2007	NLS		Changed join on PubInsertDates to AssemblyDistOptions
* 09/19/2007	JRH		Changed join on est_estimateinsertscenario_map to est_package
* 10/01/2007	NLS		Removed dummy TotalCost column.  Filled in client side from another proc
* 10/02/2007    NLS		Fixed Component Description to use LIKE
* 11/05/2007	JRH		Changed the SortOrder.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Description varchar(35),
@EST_Status_ID int,
@AdNumber int,
@ComponentDescription varchar(35),
@HostMediaType int,
@HostPageCount int,
@HostMediaQtyLow int,
@HostMediaQtyHigh int,
@PUB_InsertScenario_ID bigint,
@CreatedBy varchar(35),
@RunDateStart datetime,
@RunDateEnd datetime,
@ModifiedDateStart datetime,
@ModifiedDateEnd datetime,
@EST_Season_ID int,
@FiscalYear int,
@FiscalMonth int,
@InsertDOW int,
@EstimateComments varchar(255)
as

select
	EST_Estimate_ID,
	ParentId, 
	RunDate, 
	Description, 
	HostMediaDesc, 
	SeasonDesc,
	StatusDesc,
	UploadKillDate, 
	displayed,
	CAST(Description as char(35)) + substring(Est_ID_text, len(Est_ID_text)-9, 10) 
		+ CONVERT(varchar(30), isnull(UploadKillDate, '1/1/1900'), 12) SortOrder
FROM
	(select e.EST_Estimate_ID, 
		max(e.parent_id) ParentId, 
		max(e.RunDate) RunDate, 
		max(e.Description) Description, 
		max(mt.Description) HostMediaDesc, 
		max(s.Description) SeasonDesc,
		max(st.Description) StatusDesc,
		
		case max(e.EST_Status_ID)
			when 2 then max(e.uploaddate)
			when 3 then max(e.ModifiedDate)
			else null
		end UploadKillDate, 
		
		case
			when max(e.parent_id) is null then 1
			else 0
		end displayed,
		
		('0000000000' + CAST(isnull(max(e.parent_id),e.est_estimate_id) AS varchar(10))) Est_ID_text
	
	from EST_Estimate e left join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
		left join EST_Component hc on e.EST_Estimate_ID = hc.EST_Estimate_ID and hc.EST_ComponentType_ID = 1
		left join EST_Package ep on e.EST_Estimate_ID = ep.EST_Estimate_ID
		left join est_assemdistriboptions ado on e.EST_Estimate_ID = ado.EST_Estimate_ID
		left join EST_EstimateMediaType mt on hc.EST_EstimateMediaType_ID = mt.EST_EstimateMediaType_ID
		join EST_Season s on e.EST_Season_ID = s.EST_Season_ID
		join EST_Status st on e.EST_Status_ID = st.EST_Status_ID
	where (@Description is null or e.Description like '%' + @Description + '%')
		and (isnull(@EST_Status_ID, e.EST_Status_ID) = e.EST_Status_ID)
		and ((@AdNumber is null) or (@AdNumber = c.AdNumber))
		and ((@ComponentDescription is null) or (c.Description like '%' + @ComponentDescription + '%'))
		and ((@HostMediaType is null) or (@HostMediaType = hc.EST_EstimateMediaType_ID))
		and ((@HostPageCount is null) or (@HostPageCount = hc.PageCount))
		and (@HostMediaQtyLow is null or dbo.ComponentMediaQuantity(hc.EST_Component_ID) >= @HostMediaQtyLow)
		and (@HostMediaQtyHigh is null or dbo.ComponentMediaQuantity(hc.EST_Component_ID) <= @HostMediaQtyHigh)
		and ((@PUB_InsertScenario_ID is null) or (ep.PUB_InsertScenario_ID = @PUB_InsertScenario_ID))
		and ((@CreatedBy is null) or (@CreatedBy = e.CreatedBy))
		and (@RunDateStart is null or e.RunDate >= @RunDateStart)
		and (@RunDateEnd is null or e.RunDate <= @RunDateEnd)
		/*TODO: When a search is done on modified date.  Do we need to check the created date if modified date is null? */
		and (@ModifiedDateStart is null or isnull(e.ModifiedDate, e.CreatedDate) >= @ModifiedDateStart)
		and (@ModifiedDateEnd is null or isnull(e.ModifiedDate, e.CreatedDate) <= @ModifiedDateEnd)
		and ((@EST_Season_ID is null) or (@EST_Season_ID = e.EST_Season_ID))
		and ((@FiscalYear is null) or (@FiscalYear = e.FiscalYear))
		and ((@FiscalMonth is null) or (@FiscalMonth = e.FiscalMonth))
	    and (@InsertDOW is null or @InsertDOW = ado.InsertDOW)
		and (@EstimateComments is null or e.Comments like '%' + @EstimateComments + '%')
	group by e.EST_Estimate_ID)a

GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_Search]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
