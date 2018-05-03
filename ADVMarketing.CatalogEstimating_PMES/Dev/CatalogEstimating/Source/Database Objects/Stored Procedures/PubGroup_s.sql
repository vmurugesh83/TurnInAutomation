IF OBJECT_ID('dbo.PubGroup_s') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubGroup_s'
	DROP PROCEDURE dbo.PubGroup_s
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubGroup_s') IS NOT NULL
		PRINT '***********Drop of dbo.PubGroup_s FAILED.'
END
GO
PRINT 'Creating dbo.PubGroup_s'
GO

create proc dbo.PubGroup_s
/*
* PARAMETERS:
* none
*
* DESCRIPTION:
*		Returns all publication groups that are not for a specific package (customgroupforpackage = 0).
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubGroup        READ
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
select * from PUB_PubGroup
where CustomGroupForPackage = 0
order by sortorder
GO

GRANT  EXECUTE  ON [dbo].[PubGroup_s]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
