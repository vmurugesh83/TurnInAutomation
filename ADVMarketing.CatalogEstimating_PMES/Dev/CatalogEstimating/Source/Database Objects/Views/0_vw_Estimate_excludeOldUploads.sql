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
* 05/20/2008      BJS             Added locking hints to improve performance
*/
as
select max(EST_Estimate_ID) EST_Estimate_ID
from EST_Estimate (nolock)
where Parent_ID is not null
group by Parent_ID, EST_Status_ID
union
select EST_Estimate_ID
from EST_Estimate
where Parent_ID is null
GO

GRANT  SELECT ,  UPDATE ,  INSERT ,  DELETE  ON [dbo].[vw_Estimate_excludeOldUploads]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
