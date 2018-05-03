IF OBJECT_ID('dbo.EstEstimate_s_Search') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstEstimate_s_Search'
	DROP PROCEDURE dbo.EstEstimate_s_Search
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstEstimate_s_Search') IS NOT NULL
		PRINT '***********Drop of dbo.EstEstimate_s_Search FAILED.'
END
GO
PRINT 'Creating dbo.EstEstimate_s_Search'
GO

CREATE PROC dbo.EstEstimate_s_Search
/*
* PARAMETERS:
*	@EstEstimateId - Estimate ID to directly search for
*
*
* DESCRIPTION:
*		An Estimates isfound based on an estimate id for the Estimate Search screen.  At least one parameter is required.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate
*   EST_EstimateMediaType
*   EST_Season
*   EST_Status

* DATABASE:
*		All
*
*
* RETURN VALUE:
*	EstEstimate Row for the Search Grid
*
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 08/14/2007      NLS             Initial Creation 
* 09/04/2007      BJS             Renamed to EstEstimate_s_Search
* 09/11/2007	  NLS		  Added SortOrder column and fixed UploadDate issue
* 10/01/2007	  NLS		  Removed dummy TotalCost column.  Filled in client side from another proc
* 11/05/2007	  JRH		  Changed the SortOrder.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/

@EstEstimateId bigint

AS

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
	(SELECT e.EST_Estimate_ID, 
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
			
		   1 displayed,
		   
		('0000000000' + CAST(isnull(max(e.parent_id),e.est_estimate_id) AS varchar(10))) Est_ID_text
	
	FROM
	
		EST_Estimate e 	left join EST_Component hc on e.EST_Estimate_ID = hc.EST_Estimate_ID and hc.EST_ComponentType_ID = 1
	    left join EST_EstimateMediaType mt on hc.EST_EstimateMediaType_ID = mt.EST_EstimateMediaType_ID
		join EST_Season s on e.EST_Season_ID = s.EST_Season_ID
		join EST_Status st on e.EST_Status_ID = st.EST_Status_ID
	
	WHERE
	
		e.EST_Estimate_ID = @EstEstimateId
		
	GROUP BY e.EST_Estimate_ID)a

GO

GRANT  EXECUTE  ON [dbo].[EstEstimate_s_Search]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO