IF OBJECT_ID('dbo.PubGroup_s_ByPubGroupID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubGroup_s_ByPubGroupID'
	DROP PROCEDURE dbo.PubGroup_s_ByPubGroupID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubGroup_s_ByPubGroupID') IS NOT NULL
		PRINT '***********Drop of dbo.PubGroup_s_ByPubGroupID FAILED.'
END
GO
PRINT 'Creating dbo.PubGroup_s_ByPubGroupID'
GO

create proc dbo.PubGroup_s_ByPubGroupID
/*
* PARAMETERS:
* PUB_PubGroup_ID - required.
*
* DESCRIPTION:
*		Returns the publication group specified.
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
@PUB_PubGroup_ID bigint
as
select * from PUB_PubGroup
where PUB_PubGroup_ID = @PUB_PubGroup_ID
order by sortorder
GO

GRANT  EXECUTE  ON [dbo].[PubGroup_s_ByPubGroupID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
