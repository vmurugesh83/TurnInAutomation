IF OBJECT_ID('dbo.EstPubIssueDates_i_Override') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPubIssueDates_i_Override'
	DROP PROCEDURE dbo.EstPubIssueDates_i_Override
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPubIssueDates_i_Override') IS NOT NULL
		PRINT '***********Drop of dbo.EstPubIssueDates_i_Override FAILED.'
END
GO
PRINT 'Creating dbo.EstPubIssueDates_i_Override'
GO

create proc dbo.EstPubIssueDates_i_Override
/*
* PARAMETERS:
* Override
* IssueDOW
* IssueDate
* EST_Estimate_ID
* Pub_ID
* PubLoc_ID
* CreatedBy
*
* DESCRIPTION:
*		Identifies the PubRate Map record for the Pub and Pub-Loc and inserts an Est_PubIssueDate override record
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Component       READ
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
* 09/17/2007      BJS             Initial Creation 
* 11/30/2008      JRH             Change to @Pub_ID from Pub_ID.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Override bit,
@IssueDOW int,
@IssueDate datetime,
@EST_Estimate_ID bigint,
@Pub_ID char(3),
@PubLoc_ID int,
@CreatedBy varchar(50)
as

declare @PUB_PubRate_Map_ID bigint
select @PUB_PubRate_Map_ID = PUB_PubRate_Map_ID
from PUB_PubRate_Map
where Pub_ID = @Pub_ID and PubLoc_ID = @PubLoc_ID

insert into EST_PubIssueDates(Override, IssueDOW, IssueDate, EST_Estimate_ID, PUB_PubRate_Map_ID, CreatedBy, CreatedDate)
values(@Override, @IssueDOW, @IssueDate, @EST_Estimate_ID, @PUB_PubRate_Map_ID, @CreatedBy, getdate())
GO

GRANT  EXECUTE  ON [dbo].[EstPubIssueDates_i_Override]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
