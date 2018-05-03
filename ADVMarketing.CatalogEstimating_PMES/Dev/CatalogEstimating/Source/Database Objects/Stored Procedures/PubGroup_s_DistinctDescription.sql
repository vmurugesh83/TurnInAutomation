IF OBJECT_ID('dbo.PubGroup_s_DistinctDescription') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubGroup_s_DistinctDescription'
	DROP PROCEDURE dbo.PubGroup_s_DistinctDescription
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubGroup_s_DistinctDescription') IS NOT NULL
		PRINT '***********Drop of dbo.PubGroup_s_DistinctDescription FAILED.'
END
GO
PRINT 'Creating dbo.PubGroup_s_DistinctDescription'
GO

create proc dbo.PubGroup_s_DistinctDescription
/*
* PARAMETERS:
* none
*
* DESCRIPTION:
*		Returns distinct publication group descriptions that are not for a specific package (customgroupforpackage = 0).
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubGroup
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
* 06/22/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
as
select description, max(sortorder) sortorder
from PUB_PubGroup
where CustomGroupForPackage = 0
group by description
order by description
GO

GRANT  EXECUTE  ON [dbo].[PubGroup_s_DistinctDescription]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
