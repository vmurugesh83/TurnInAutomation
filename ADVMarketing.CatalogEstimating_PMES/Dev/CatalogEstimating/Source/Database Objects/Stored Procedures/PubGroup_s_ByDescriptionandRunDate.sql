IF OBJECT_ID('dbo.PubGroup_s_ByDescriptionandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubGroup_s_ByDescriptionandRunDate'
	DROP PROCEDURE dbo.PubGroup_s_ByDescriptionandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubGroup_s_ByDescriptionandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PubGroup_s_ByDescriptionandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PubGroup_s_ByDescriptionandRunDate'
GO

create proc dbo.PubGroup_s_ByDescriptionandRunDate
/*
* PARAMETERS:
* Description
* RunDate
*
* DESCRIPTION:
*		Returns the Pub Group with a matching Description on the specified RunDate (customgroupforpackage = 0).
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
* 09/04/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Description varchar(35),
@RunDate datetime
as
select top 1 * from PUB_PubGroup
where Description = @Description and EffectiveDate <= @RunDate and CustomGroupForPackage = 0
order by EffectiveDate desc
GO

GRANT  EXECUTE  ON [dbo].[PubGroup_s_ByDescriptionandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
