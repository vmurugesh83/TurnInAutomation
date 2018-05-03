IF OBJECT_ID('dbo.EstComponent_Search') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstComponent_Search'
	DROP PROCEDURE dbo.EstComponent_Search
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstComponent_Search') IS NOT NULL
		PRINT '***********Drop of dbo.EstComponent_Search FAILED.'
END
GO
PRINT 'Creating dbo.EstComponent_Search'
GO

CREATE proc dbo.EstComponent_Search
/*
* PARAMETERS:
* EST_Component_ID
* Description
* RunDateStart
* RunDateEnd
* EST_EstimateMediaType_ID
* EST_ComponentType_ID
* PaperWeight_ID
* PaperGrade_ID
* PageCount
* VendorSupplied - 1 = All Components, 2 = Only VS Components, 3 = Exclude VS Components

* DESCRIPTION:
*		Components are found based on search criteria for the Component Search screen.  At least one parameter is required.  The 
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Estimate
*   EST_Component
*   EST_EstimateInsertScenario_Map
*   EST_PubInsertDates
*   EST_EstimateMediaType
*   EST_Season
*   EST_Status
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
* 08/21/2007      BJS             Initial Creation
* 09/19/2007      BJS             Defect 305 - Fixed Description search by adding like comparison
* 10/29/2007      JRH             Change to inner join.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Component_ID bigint,
@Description varchar(35),
@RunDateStart datetime,
@RunDateEnd datetime,
@EST_EstimateMediaType_ID int,
@EST_ComponentType_ID int,
@PaperWeight_ID int,
@PaperGrade_ID int,
@PageCount int,
@VendorSupplied tinyint
as

select c.EST_Component_ID, max(e.EST_Estimate_ID) EST_Estimate_ID, max(e.Parent_ID) Parent_ID, max(e.EST_Status_ID) EST_Status_ID,
	max(e.RunDate) RunDate, max(c.Description) Description, max(c.AdNumber) AdNumber
from EST_Estimate e inner join EST_Component c on e.EST_Estimate_ID = c.EST_Estimate_ID
where (@EST_Component_ID is null or (@EST_Component_ID = c.EST_Component_ID))
	and (@Description is null or (c.Description like '%' + @Description + '%'))
	and (@RunDateStart is null or (@RunDateStart <= e.RunDate))
	and (@RunDateEnd is null or (@RunDateEnd >= e.RunDate))
	and (@EST_EstimateMediaType_ID is null or (@EST_EstimateMediaType_ID = c.EST_EstimateMediaType_ID))
	and (@EST_ComponentType_ID is null or (@EST_ComponentType_ID = c.EST_ComponentType_ID))
	and (@PaperWeight_ID is null or (@PaperWeight_ID = c.PaperWeight_ID))
	and (@PaperGrade_ID is null or (@PaperGrade_ID = c.PaperGrade_ID))
	and (@PageCount is null or (@PageCount = c.PageCount))
	and (@VendorSupplied = 1 or (@VendorSupplied = 2 and c.VendorSupplied = 1) or (@VendorSupplied = 3 and c.VendorSupplied = 0))
group by c.EST_Component_ID
GO

GRANT  EXECUTE  ON [dbo].[EstComponent_Search]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
