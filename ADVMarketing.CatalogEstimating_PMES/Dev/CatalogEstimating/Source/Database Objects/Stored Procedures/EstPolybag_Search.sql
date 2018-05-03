IF OBJECT_ID('dbo.EstPolybag_Search') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPolybag_Search'
	DROP PROCEDURE dbo.EstPolybag_Search
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPolybag_Search') IS NOT NULL
		PRINT '***********Drop of dbo.EstPolybag_Search FAILED.'
END
GO
PRINT 'Creating dbo.EstPolybag_Search'
GO

CREATE proc dbo.EstPolybag_Search
/*
* PARAMETERS:
* EST_Polybag_ID
* Description
* Comments
* EST_Season_ID
* FiscalYear
* FiscalMonth
* HostAdNumber
* StartRunDate
* EndRunDate
* CreatedBy
* EST_Status_ID

* DESCRIPTION:
*		Polybags are found based on search criteria for the Polybag Search screen.  At least one parameter is required.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
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
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 08/22/2007      BJS             Initial Creation 
* 09/27/2007      NLS             Changed group by to key around the PolybagGroup instead of Polybag
*                                 and fixed up associated search fields appropriately.
* 09/28/2007      NLS             Changed join to use EstimatePolybagGroupMap so that groups with no polybags searched correctly
* 10/02/2007      BJS             Fixed search on description
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Polybag_ID bigint,
@Description varchar(35),
@Comments varchar(255),
@EST_Season_ID int,
@FiscalYear int,
@FiscalMonth int,
@HostAdNumber int,
@StartRunDate datetime,
@EndRunDate datetime,
@CreatedBy varchar(50),
@EST_Status_ID int
as

select pbg.EST_PolybagGroup_ID EST_Polybag_ID, max(e.RunDate) RunDate, max(pbg.Description) Description, max(pbg.Comments) Comments, max(es.Description) Season,
	max(e.FiscalYear) FiscalYear, max(st.Description) EstimateStatus
from EST_PolybagGroup pbg left join EST_Polybag pb on pbg.EST_PolybagGroup_ID = pb.EST_PolybagGroup_ID
	left join EST_PackagePolybag_Map ppm on pb.EST_Polybag_ID = ppm.EST_Polybag_ID
	left join EST_Package p on ppm.EST_Package_ID = p.EST_Package_ID
	left join EST_PackageComponentMapping pcm on p.EST_Package_ID = pcm.EST_Package_ID
	left join EST_Component c on pcm.EST_Component_ID = c.EST_Component_ID
    left join EST_EstimatePolybagGroup_map epbgm on pbg.EST_PolybagGroup_ID = epbgm.EST_PolybagGroup_ID
	left join EST_Estimate e on epbgm.EST_Estimate_ID = e.EST_Estimate_ID
	left join EST_Season es on e.EST_Season_ID = es.EST_Season_ID
	left join EST_Status st on e.EST_Status_ID = st.EST_Status_ID
where (@EST_Polybag_ID is null or (@EST_Polybag_ID = pbg.EST_PolybagGroup_ID))
	and (@Description is null or (pbg.Description LIKE '%' + @Description + '%' ))
	and (@Comments is null or (pbg.Comments like '%' + @Comments + '%'))
	and (@EST_Season_ID is null or (@EST_Season_ID = e.EST_Season_ID))
	and (@FiscalYear is null or (@FiscalYear = e.FiscalYear))
	and (@FiscalMonth is null or (@FiscalMonth = e.FiscalMonth))
	and (@HostAdNumber is null or (c.EST_ComponentType_ID = 1 and @HostAdNumber = c.AdNumber))
	and (@StartRunDate is null or (@StartRunDate <= e.RunDate))
	and (@EndRunDate is null or (@EndRunDate >= e.RunDate))
	and (@CreatedBy is null or (@CreatedBy = pb.CreatedBy))
	and (@EST_Status_ID is null or (@EST_Status_ID = e.EST_Status_ID))
group by pbg.EST_PolybagGroup_ID
order by e.RunDate
GO

GRANT  EXECUTE  ON [dbo].[EstPolybag_Search]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
